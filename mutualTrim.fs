FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/query.fs", version : "✨");
import(path : "onshape/std/boolean.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/manipulator.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/transform.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");
import(path : "onshape/std/units.fs", version : "✨");

const OTHER_SIDE_1_MANIPULATOR_NAME = "Keep first surface opposite side manipulator";
const OTHER_SIDE_2_MANIPULATOR_NAME = "Keep second surface opposite side manipulator";
const SPLIT_SUFFIX = "split";

/**
 * @type {{
 *      @field facesToKeep {Query} : faces to delete.
 *      @field facesToDelete {Query} : faces to keep.
 * }}
 **/
type ClassifiedFaces typecheck canBeClassifiedFaces;

predicate canBeClassifiedFaces(value)
{
    value.facesToKeep is Query;
    value.facesToDelete is Query;
}

/**
 * Trim two adjacent surfaces by extending intersections to complete the trim.
 */
annotation { "Feature Type Name" : "Mutual trim",
        "Manipulator Change Function" : "mutualTrimMaipulatorChange",
        "Editing Logic Function" : "mutualTrimEditLogic",
        "Filter Selector" : "allparts" }
export const mutualTrim = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "First surface",
                    "Filter" : EntityType.BODY && BodyType.SHEET && ModifiableEntityOnly.YES && SketchObject.NO && ConstructionObject.NO,
                    "MaxNumberOfPicks" : 1 }
        definition.body1 is Query;

        annotation { "Name" : "Display flips", "Default" : false, "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.displayFlips is boolean;

        if (definition.displayFlips)
        {
            annotation { "Name" : "Keep opposite side", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.keepOtherSide1 is boolean;
        }

        annotation { "Name" : "Second surface",
                    "Filter" : EntityType.BODY && BodyType.SHEET && ModifiableEntityOnly.YES && SketchObject.NO && ConstructionObject.NO,
                    "MaxNumberOfPicks" : 1 }
        definition.body2 is Query;

        if (definition.displayFlips)
        {
            annotation { "Name" : "Keep opposite side", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.keepOtherSide2 is boolean;
        }

        annotation { "Name" : "Merge",
                    "Default" : true,
                    "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        definition.merge is boolean;
    }
    {
        checkOneSelection(context, definition.body1, "body1");
        checkOneSelection(context, definition.body2, "body2");
        if (!isQueryEmpty(context, definition.body1) && isQueryEmpty(context, qSubtraction(definition.body1, definition.body2)))
        {
            throw regenError(ErrorStringEnum.MUTUAL_TRIM_SAME_SURFACE_USED, ["body1", "body2"]);
        }

        const splitFaceDefinition1 = {
                "faceTargets" : qOwnedByBody(definition.body1, EntityType.FACE),
                "bodyTools" : definition.body2,
                "keepToolSurfaces" : true,
                "extendToCompletion" : true,
                "mutualImprint" : true
            };
        const splitId = id + SPLIT_SUFFIX;
        opSplitFace(context, splitId, splitFaceDefinition1);

        const facesToDelete = findFacesToDelete(context, id, definition, splitId);
        if (!isQueryEmpty(context, facesToDelete))
        {
            const deleteFacesId = id + "deleteFaces";
            opDeleteFace(context, deleteFacesId, {
                        "deleteFaces" : facesToDelete,
                        "includeFillet" : false,
                        "capVoid" : false,
                        "leaveOpen" : true
                    });
        }
        if (definition.merge)
        {
            callSubfeatureAndProcessStatus(id, opBoolean, context, id + "merge", {
                        "tools" : qUnion([definition.body1, definition.body2]),
                        "operationType" : BooleanOperationType.UNION
                    }, { "propagateErrorDisplay" : true });
        }

    }, { "merge" : true, "displayFlips" : true });

function findFacesToDelete(context is Context, id is Id, definition is map, splitId is Id) returns Query
{
    const imprintEdgesInBody1Q = qIntersection([qCreatedBy(id, EntityType.EDGE),
                    qOwnedByBody(definition.body1, EntityType.EDGE)])->qEdgeTopologyFilter(EdgeTopology.TWO_SIDED);
    const imprintEdgesInBody2Q = qIntersection([qCreatedBy(id, EntityType.EDGE),
                    qOwnedByBody(definition.body2, EntityType.EDGE)])->qEdgeTopologyFilter(EdgeTopology.TWO_SIDED);
    if (isQueryEmpty(context, imprintEdgesInBody1Q) || isQueryEmpty(context, imprintEdgesInBody2Q))
    {
        return qNothing();
    }
    const manipulatorData = {
            "keepOtherSide2" : definition.keepOtherSide2,
            "keepOtherSide1" : definition.keepOtherSide1,
            "id" : id };
    createManipulators(context, imprintEdgesInBody1Q, imprintEdgesInBody2Q, manipulatorData);

    const facesToDeleteQ1 = getAllFacesToDelete(context, qOwnedByBody(definition.body1, EntityType.FACE),
        imprintEdgesInBody1Q, definition.keepOtherSide1, splitId);
    const facesToDeleteQ2 = getAllFacesToDelete(context, qOwnedByBody(definition.body2, EntityType.FACE),
        imprintEdgesInBody2Q, definition.keepOtherSide2, splitId);
    return qUnion([facesToDeleteQ1, facesToDeleteQ2]);
}

/**
 * imprintEdges are expected to partition facesToClassify.
 * There are edges in imprintEdges touching those in imprintEdgesOnOtherBody.
 * In the loop previous classification is used as a seed for flood fill. If flood fill reaches other connected components of
 * imprintEdges, addAdjacentToClassification produces seeds on another side of these components.
 **/
function getAllFacesToDelete(context is Context, facesToClassify is Query, imprintEdges is Query,
    flipSide is boolean, splitId is Id)
{
    const nFaces = size(evaluateQuery(context, facesToClassify));
    var classifiedFaces = { "facesToKeep" : qSplitBy(splitId, EntityType.FACE, !flipSide), "facesToDelete" : qSplitBy(splitId, EntityType.FACE, flipSide) } as ClassifiedFaces;
    classifiedFaces.facesToDelete = qIntersection([classifiedFaces.facesToDelete, facesToClassify]);
    classifiedFaces.facesToKeep = qIntersection([classifiedFaces.facesToKeep, facesToClassify]);

    // This should be a while loop, but limiting iterations to be safe
    for (var i = 0; i < nFaces; i += 1)
    {
        classifiedFaces.facesToKeep = floodFill(context, classifiedFaces.facesToKeep, imprintEdges);
        classifiedFaces.facesToDelete = floodFill(context, classifiedFaces.facesToDelete, imprintEdges);
        const classifiedQ = qUnion([classifiedFaces.facesToKeep, classifiedFaces.facesToDelete]);
        var leftToClassifyQ = qSubtraction(facesToClassify, classifiedQ);
        if (isQueryEmpty(context, leftToClassifyQ))
        {
            break;
        }
        const edgesToCheckQ = qIntersection([qAdjacent(leftToClassifyQ, AdjacencyType.EDGE, EntityType.EDGE),
                    qAdjacent(classifiedQ, AdjacencyType.EDGE, EntityType.EDGE), imprintEdges]);
        if (isQueryEmpty(context, edgesToCheckQ))
        {
            throw regenError(ErrorStringEnum.MUTUAL_TRIM_GENERIC_ERROR);
        }
        classifiedFaces = addAdjacentToClassification(context, edgesToCheckQ, classifiedFaces);
        leftToClassifyQ = qSubtraction(leftToClassifyQ, classifiedFaces.facesToKeep);
        if (isQueryEmpty(context, leftToClassifyQ))
        {
            break;
        }
    }

    return classifiedFaces.facesToDelete;
}

function createManipulators(context is Context, edges is Query, otherBodyEdges is Query, manipulatorData is map)
{
    var edge;
    var edgeToEdges;
    for (edge in evaluateQuery(context, edges))
    {
        edgeToEdges = evDistance(context, {
                    "side0" : edge,
                    "side1" : otherBodyEdges
                });

        if (edgeToEdges != undefined && tolerantEquals(edgeToEdges.distance, 0 * meter))
        {
            break;
        }
    }
    if (edge == undefined || edgeToEdges == undefined)
    {
        return;
    }

    const edgeFacesQ = edge->qAdjacent(AdjacencyType.EDGE, EntityType.FACE);
    const edgeFaces = evaluateQuery(context, edgeFacesQ);
    const faceTangentPlane = evFaceTangentPlaneAtEdge(context, {
                "edge" : edge,
                "face" : edgeFaces[0],
                "parameter" : edgeParameterToCoEdge(context, edge, edgeFaces[0], edgeToEdges.sides[0].parameter),
                "usingFaceOrientation" : true
            });
    const otherEdge = otherBodyEdges->qNthElement(edgeToEdges.sides[1].index);
    const otherFace = otherEdge->qAdjacent(AdjacencyType.EDGE, EntityType.FACE)->qNthElement(0);
    const otherFaceTangentPlane = evFaceTangentPlaneAtEdge(context, {
                "edge" : otherEdge,
                "face" : otherFace,
                "parameter" : edgeParameterToCoEdge(context, otherEdge, otherFace, edgeToEdges.sides[1].parameter),
                "usingFaceOrientation" : true
            });

    setMutualTrimManipulators(context, manipulatorData, faceTangentPlane, otherFaceTangentPlane);
}

function floodFill(context is Context, seedFaces is Query, boundaryEdges is Query) returns Query
{
    const allBodyFacesQ = seedFaces->qOwnerBody()->qOwnedByBody(EntityType.FACE);
    const nMaxIterations = size(evaluateQuery(context, allBodyFacesQ));
    var result = seedFaces;
    var frontier = seedFaces;
    for (var i = 0; i < nMaxIterations; i += 1)
    {
        const facesToAdd = frontier->qAdjacent(AdjacencyType.EDGE, EntityType.EDGE)->qSubtraction(boundaryEdges)->qAdjacent(AdjacencyType.EDGE, EntityType.FACE);
        frontier = qSubtraction(facesToAdd, result);
        result = qUnion([result, facesToAdd]);
        if (isQueryEmpty(context, frontier))
        {
            break;
        }
    }
    return result;
}

/**
 * edgesQ contains imprint edges that both classified and unclassified faces have in common.
 *  For each edge find classified adjacent face and put the opposite face into the other set.
 **/
function addAdjacentToClassification(context is Context, edgesQ is Query, classifiedFaces is map) returns ClassifiedFaces
{
    var facesToKeep = evaluateQuery(context, classifiedFaces.facesToKeep);
    var facesToDelete = evaluateQuery(context, classifiedFaces.facesToDelete);
    for (var edge in evaluateQuery(context, edgesQ))
    {
        const edgeFacesQ = edge->qAdjacent(AdjacencyType.EDGE, EntityType.FACE);
        const edgeFaces = evaluateQuery(context, edgeFacesQ);
        for (var i = 0; i < 2; i += 1)
        {
            if (!isQueryEmpty(context, qIntersection([edgeFaces[i], qUnion(facesToKeep)])))
            {
                facesToDelete = append(facesToDelete, edgeFaces[1 - i]);
                break;
            }
            else if (!isQueryEmpty(context, qIntersection([edgeFaces[i], qUnion(facesToDelete)])))
            {
                facesToKeep = append(facesToKeep, edgeFaces[1 - i]);
                break;
            }
        }
    }
    return { "facesToKeep" : qUnion(facesToKeep), "facesToDelete" : qUnion(facesToDelete) } as ClassifiedFaces;
}

function checkOneSelection(context is Context, query is Query, parameterName is string)
{
    if (isQueryEmpty(context, query))
    {
        throw regenError(ErrorStringEnum.MUTUAL_TRIM_SURFACE_NOT_SELECTED, [parameterName]);
    }
}

function setMutualTrimManipulators(context is Context, manipulatorData is map, plane1 is Plane, plane2 is Plane)
{
    if (manipulatorData.id == undefined)
    {
        return;
    }
    addManipulators(context, manipulatorData.id, {
                (OTHER_SIDE_1_MANIPULATOR_NAME) : makeTrimManipulator(plane1, plane2.normal, manipulatorData.keepOtherSide1),
                (OTHER_SIDE_2_MANIPULATOR_NAME) : makeTrimManipulator(plane2, plane1.normal, manipulatorData.keepOtherSide2)
            });
}

function edgeParameterToCoEdge(context is Context, edge is Query, face is Query, edgeParameter is number) returns number
{
    const edgeMidTangent = evEdgeTangentLine(context, {
                "edge" : edge,
                "parameter" : 0.5
            });

    const coEdgeMidTangent = evEdgeTangentLine(context, {
                "edge" : edge,
                "parameter" : 0.5,
                "face" : face
            });

    if (dot(edgeMidTangent.direction, coEdgeMidTangent.direction) > 0)
    {
        return edgeParameter;
    }
    else
    {
        return 1 - edgeParameter;
    }
}

function makeTrimManipulator(facePlane is Plane, otherFaceNormal is Vector, isFlipped is boolean) returns Manipulator
{
    var manipulatorDir = cross(facePlane.normal, facePlane.x);
    if (dot(manipulatorDir, otherFaceNormal) > 0)
        manipulatorDir = -manipulatorDir;
    return flipManipulator({
                "base" : facePlane.origin,
                "direction" : manipulatorDir,
                "flipped" : isFlipped });
}

/** @internal */
export function mutualTrimMaipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    for (var manipulator in newManipulators)
    {
        if (manipulator.key == OTHER_SIDE_2_MANIPULATOR_NAME)
        {
            definition.keepOtherSide2 = manipulator.value.flipped;
            return definition;
        }
        if (manipulator.key == OTHER_SIDE_1_MANIPULATOR_NAME)
        {
            definition.keepOtherSide1 = manipulator.value.flipped;
            return definition;
        }
    }
}

/** @internal */
export function mutualTrimEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreation is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    definition.displayFlips = definition.displayFlips || (specifiedParameters.body2 == true);
    return definition;
}

