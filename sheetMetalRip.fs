FeatureScript 1717; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */

import(path : "onshape/std/attributes.fs", version : "1717.0");
import(path : "onshape/std/containers.fs", version : "1717.0");
import(path : "onshape/std/error.fs", version : "1717.0");
import(path : "onshape/std/feature.fs", version : "1717.0");
import(path : "onshape/std/evaluate.fs", version : "1717.0");
import(path : "onshape/std/geomOperations.fs", version : "1717.0");
import(path : "onshape/std/mathUtils.fs", version : "1717.0");
import(path : "onshape/std/query.fs", version : "1717.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "1717.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "1717.0");
import(path : "onshape/std/splitpart.fs", version : "1717.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1717.0");
import(path : "onshape/std/units.fs", version : "1717.0");
import(path : "onshape/std/valueBounds.fs", version : "1717.0");


/**
* @internal
*  TODO : This feature produces a sheet metal rip by splitting a wall
*/
annotation { "Feature Type Name" : "Rip", "Filter Selector" : "allparts" }
export const sheetMetalRip = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Vertex pairs to split with",
                     "Filter" : EntityType.VERTEX && BodyType.SOLID && AllowFlattenedGeometry.YES } //vertex pairs
        definition.vertices is Query;

        annotation { "Name" : "Use default minimal gap", "Default" : true }
        definition.useDefaultGap is boolean;
        if (!definition.useDefaultGap)
        {
            annotation { "Name" : "Minimal gap" }
            isLength(definition.minimalClearance, SM_MINIMAL_CLEARANCE_BOUNDS);
        }
    }
    {
        //this is not necessary but helps with correct error reporting in feature pattern
        checkNotInFeaturePattern(context, definition.vertices, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        //entities should be from the same sm model, but can be from different parts
        if (!areEntitiesFromSingleActiveSheetMetalModel(context, definition.vertices))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_SINGLE_MODEL_NEEDED, ["vertices"]);
        }

        createFlatJointWithSplit(context, id, definition);

    }, {useDefaultGap : true});


function getCornerVertex(context is Context, vertex is Query) returns Query
{
    var result = try silent(evaluateQuery(context, findCornerDefinitionVertex(context, vertex)));
    if (result == undefined || size(result) != 1)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_RIP_NO_CORNER, ["vertices"], vertex);
    }
    return result[0];
}

function getSplitEdgesAndFaces(context is Context, id is Id, vertices is Query) returns map
{
    if (vertices.queryType != QueryType.UNION)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_RIP_FAIL);
    }

    if (size(vertices.subqueries) % 2 != 0)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_RIP_EVEN, ["vertices"]);
    }
    var edgeQueryList = [];
    var faceQueryList = [];
    for (var i = 0; i < size(vertices.subqueries); i += 2)
    {
        var userVertices = [vertices.subqueries[i], vertices.subqueries[i + 1]];
        var vertex1 = getCornerVertex(context, userVertices[0]);
        var vertex2 = getCornerVertex(context, userVertices[1]);

        // Need this to check that all vertices lie on the same wall in the smModel.
        var facesFrom1 = qAdjacent(vertex1, AdjacencyType.VERTEX, EntityType.FACE);
        var facesFrom2 = qAdjacent(vertex2, AdjacencyType.VERTEX, EntityType.FACE);
        var sharedFace = qIntersection([facesFrom1, facesFrom2]);
        if (size(evaluateQuery(context, sharedFace)) != 1)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_RIP_WALL_NOT_FOUND, qUnion(userVertices));
        }
        faceQueryList = append(faceQueryList, sharedFace);

        //Make sure selecting the same underlying vertex shows correct error
        var evVertex1 = evVertexPoint(context, {"vertex" : vertex1});
        var evVertex2 = evVertexPoint(context, {"vertex" : vertex2});
        if (tolerantEquals(evVertex1, evVertex2))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_RIP_SAME_VERTEX, qUnion(userVertices));
        }

        const newEdgeQuery = addLineSegment(context, id + "wire" + unstableIdComponent(i), evVertex1, evVertex2);
        edgeQueryList = append(edgeQueryList, newEdgeQuery);
    }
    return {"edges" : qUnion(edgeQueryList), "faces" : qUnion(faceQueryList)};
}

function createFlatJointWithSplit(context is Context, id is Id, definition is map)
{
    var splitData = getSplitEdgesAndFaces(context, id, definition.vertices);
    var splitLines = splitData.edges;
    var splitFacesEvaluated = evaluateQuery(context, splitData.faces);
    if (size(splitFacesEvaluated) != 1)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_RIP_WALL_NOT_FOUND);
    }

    var sheetMetalModel = qOwnerBody(splitFacesEvaluated[0]);
    const initialData = getInitialEntitiesAndAttributes(context, sheetMetalModel);
    const trackingSMModel = startTracking(context, sheetMetalModel);

    const sheetMetalPlane = evPlane(context, {"face" : splitFacesEvaluated[0]});
    var originalFace = startTracking(context, splitFacesEvaluated[0]);
    const faceSplitDefinition = { "edgeTools" : splitLines,
            "faceTargets" : splitFacesEvaluated[0],
            "splitType" : SplitType.FACE,
            "direction" : sheetMetalPlane.normal };
    opSplitFace(context, id + "split", faceSplitDefinition);
    opDeleteBodies(context, id + "del", {"entities" : qOwnerBody(splitLines)});

    var createdFaces = evaluateQuery(context, originalFace);
    if (createdFaces == [])
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_RIP_NEED_MORE_VERTICES, ["vertices"]);
    }

    var newEdges = evaluateQuery(context, qCreatedBy(id + "split", EntityType.EDGE));
    var count = 0;
    var ripAttributes = {"minimalClearance" : definition.useDefaultGap ? undefined : definition.minimalClearance};
    for (var e in newEdges)
    {
        setAttribute(context, {"entities" : e,
            "attribute" : createRipAttribute(context, e, toAttributeId(id + count ), SMJointStyle.EDGE, ripAttributes)});
        count += 1;
    }
    const toUpdate = assignSMAttributesToNewOrSplitEntities(context, qUnion([trackingSMModel, sheetMetalModel]), initialData, id);

    updateSheetMetalGeometry(context, id + "smUpdate", {
                "entities" : toUpdate.modifiedEntities,
                "deletedAttributes" : toUpdate.deletedAttributes });
}

function addLineSegment(context is Context, id is Id, start is Vector, end is Vector) returns Query
{
    opFitSpline(context, id, {
                "points" : [start, end]
            });
    return qCreatedBy(id, EntityType.EDGE);
}

