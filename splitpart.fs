FeatureScript 455; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "455.0");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "455.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "455.0");
import(path : "onshape/std/containers.fs", version : "455.0");
import(path : "onshape/std/evaluate.fs", version : "455.0");
import(path : "onshape/std/feature.fs", version : "455.0");
import(path : "onshape/std/math.fs", version : "455.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "455.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "455.0");
import(path : "onshape/std/sketch.fs", version : "455.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "455.0");
import(path : "onshape/std/tool.fs", version : "455.0");
import(path : "onshape/std/vector.fs", version : "455.0");

/**
 * Defines whether a `split` should split whole parts, or just faces.
 */

export enum SplitType
{
    annotation { "Name" : "Part" }
    PART,
    annotation { "Name" : "Face" }
    FACE
}

/**
 * Feature performing an [opSplitPart].
 */
annotation { "Feature Type Name" : "Split", "Filter Selector" : "allparts" }
export const splitPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Split type", "UIHint" : "HORIZONTAL_ENUM" }
        definition.splitType is SplitType;

        if (definition.splitType == SplitType.PART)
        {
            annotation { "Name" : "Parts or surfaces to split", "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && ModifiableEntityOnly.YES }
            definition.targets is Query;

            annotation { "Name" : "Entity to split with",
                        "Filter" : (EntityType.BODY && BodyType.SHEET) || (GeometryType.PLANE && ConstructionObject.YES),
                        "MaxNumberOfPicks" : 1 }
            definition.tool is Query;

            annotation { "Name" : "Keep tools" }
            definition.keepTools is boolean;
        }
        else
        {
            annotation { "Name" : "Faces to split", "Filter" : (EntityType.FACE && SketchObject.NO && ConstructionObject.NO && ModifiableEntityOnly.YES) }
            definition.faceTargets is Query;

            annotation { "Name" : "Entities to split with",
                        "Filter" : (EntityType.EDGE && SketchObject.YES && ConstructionObject.NO) ||
                            (EntityType.BODY && BodyType.SHEET) ||
                            (EntityType.FACE && GeometryType.PLANE && ConstructionObject.YES) }
            definition.faceTools is Query;
        }
    }
    {
        if (shouldPerformSheetMetalAwareSplit(context, definition))
        {
            sheetMetalAwareSplit(context, id, definition);
        }
        else
        {
            performRegularSplit(context, id, definition);
        }
    }, { keepTools : false, splitType : SplitType.PART });

function shouldPerformSheetMetalAwareSplit(context is Context, definition is map) returns boolean
{
    return isAtVersionOrLater(context, FeatureScriptVersionNumber.V447_SPLIT_SHEET_METAL);
}

function findSheetMetalFaces(context is Context, id is Id, entities is Query) returns Query
{
    var sheetEdges = qEntityFilter(qUnion(getSMDefinitionEntities(context, entities)), EntityType.EDGE);
    if (size(evaluateQuery(context, sheetEdges)) != 0)
    {
        throw regenError(ErrorStringEnum.CANT_SPLIT_SHEET_METAL_BEND_FACE, ["faceTargets"], entities);
    }
    return qEntityFilter(qUnion(getSMDefinitionEntities(context, entities)), EntityType.FACE);
}

function sheetMetalAwareSplit(context is Context, id is Id, definition is map)
{
    if (definition.splitType == SplitType.PART)
    {
        const parts = partitionSheetMetalParts(context, definition.targets);
        const sheetMetalPartCount = size(parts.sheetMetalPartsMap);
        var toolBodies = evaluateQuery(context, qEntityFilter(definition.tool, EntityType.BODY));
        const deleteToolsAtEnd = sheetMetalPartCount != 0 && !definition.keepTools && size(toolBodies) > 0;
        if (sheetMetalPartCount != 0)
        {
            definition.keepTools = true;
        }
        else
        {
            performRegularSplit(context, id, definition);
            return;
        }
        if (size(evaluateQuery(context, parts.nonSheetMetalPartsQuery)) > 0)
        {
            definition.targets = parts.nonSheetMetalPartsQuery;
            performRegularSplit(context, id, definition);
        }
        var index = 0;
        for (var idAndParts in parts.sheetMetalPartsMap)
        {
            const subId = id + unstableIdComponent(index);
            index += 1;
            definition.part = qUnion(idAndParts.value);
            splitSheetMetalPartFeatureWrapper(context, subId, definition);
            processSubfeatureStatus(context, id, { "subfeatureId" : subId, "propagateErrorDisplay" : true });
        }
        if (deleteToolsAtEnd)
        {
            try(opDeleteBodies(context, id + "delete", { "entities" : qEntityFilter(definition.tool, EntityType.BODY) }));
        }
    }
    else
    {
        definition.tool = definition.faceTools;
        definition.keepTools = true;
        var queries = separateSheetMetalQueries(context, id, definition.faceTargets);
        const nonSheetMetalQueryCount = size(evaluateQuery(context, queries.nonSheetMetalQueries));
        const sheetMetalQueryCount = size(evaluateQuery(context, queries.sheetMetalQueries));
        if (nonSheetMetalQueryCount > 0 || sheetMetalQueryCount == 0)
        {
            definition.faceTargets = queries.nonSheetMetalQueries;
            performRegularSplit(context, id, definition);
        }
        if (sheetMetalQueryCount > 0)
        {
            definition.targets = queries.sheetMetalQueries;
            splitSheetMetalFaceFeatureWrapper(context, id + "splitSubId", definition);
            processSubfeatureStatus(context, id, { "subfeatureId" : id + "splitSubId", "propagateErrorDisplay" : true });
        }
    }
}

function splitAndUpdateSM(context is Context, id is Id, definition, originalEntities)
{
    var output;
    if (definition.splitType == SplitType.PART)
    {
        output = performSheetMetalSplit(context, id, definition);
    }
    else
    {
        output = performSheetMetalSplitWithExtend(context, id, definition);
    }
    const initialAssociationAttributes = getAttributes(context, {
                "entities" : qOwnedByBody(definition.sheetMetalModel),
                "attributePattern" : {} as SMAssociationAttribute });
    const modifiedFaces = output.modifiedFaces;
    const toUpdate = assignSMAttributesToNewOrSplitEntities(context, definition.sheetMetalModel,
            originalEntities, initialAssociationAttributes);

    var index = 0;
    for (var edge in evaluateQuery(context, output.newEdges))
    {
        var ripAttribute = makeSMJointAttribute(toAttributeId(id + ("rip" ~ index)));
        ripAttribute.jointType = { "value" : SMJointType.RIP, "canBeEdited" : false };
        ripAttribute.jointStyle = { "value" : SMJointStyle.FLAT, "canBeEdited" : false };
        setAttribute(context, {
                    "entities" : edge,
                    "attribute" : ripAttribute
                });
        index += 1;
    }
    updateSheetMetalGeometry(context, id + "smUpdate", {
                "entities" : qUnion([toUpdate.modifiedEntities, modifiedFaces]),
                "deletedAttributes" : toUpdate.deletedAttributes });
}

function performRegularSplit(context is Context, id is Id, definition is map)
{
    if (definition.splitType == SplitType.PART)
    {
        definition.tool = qOwnerBody(definition.tool);
        opSplitPart(context, id, definition);
    }
    else
    {
        var edgeTools = qConstructionFilter(qEntityFilter(definition.faceTools, EntityType.EDGE),
            ConstructionObject.NO);
        var bodyTools = qConstructionFilter(qEntityFilter(definition.faceTools, EntityType.BODY),
            ConstructionObject.NO);
        var planeTools = qEntityFilter(definition.faceTools, EntityType.FACE);

        var splitFaceDefinition = {
            "faceTargets" : definition.faceTargets,
            "edgeTools" : edgeTools,
            "bodyTools" : bodyTools,
            "planeTools" : planeTools
        };

        // edge tools could be empty
        const planeResult = try silent (evOwnerSketchPlane(context, { "entity" : edgeTools }));
        if (planeResult != undefined)
        {
            splitFaceDefinition.direction = planeResult.normal;
        }

        opSplitFace(context, id, splitFaceDefinition);
    }
}

function getSMModelFacesForPart(context is Context, partQuery is Query) returns Query
{
    const faceQuery = qOwnedByBody(partQuery, EntityType.FACE);
    const sheetMetalEntities = qUnion(getSMDefinitionEntities(context, faceQuery));
    return qEntityFilter(sheetMetalEntities, EntityType.FACE);
}

const splitSheetMetalPartFeatureWrapper = defineSheetMetalFeature(function(context is Context, id is Id, definition)
    {
        const sheetMetalModel = getSheetMetalModelForPart(context, definition.part);
        const originalEntities = evaluateQuery(context, qOwnedByBody(sheetMetalModel));
        definition.faceTargets = getSMModelFacesForPart(context, definition.part);
        definition.sheetMetalModel = sheetMetalModel;
        splitAndUpdateSM(context, id, definition, originalEntities);
    }, {});

const splitSheetMetalFaceFeatureWrapper = defineSheetMetalFeature(function(context is Context, id is Id, definition)
    {
        // Separate faces to split by which sheet metal model they belong to.
        var bodyToFacesMap = {};
        for (var query in evaluateQuery(context, definition.targets))
        {
            const smFaceQuery = evaluateQuery(context, findSheetMetalFaces(context, id, query));
            if (size(smFaceQuery) == 0)
            {
                throw "Could not find sheet metal model face associated with sheet metal part face.";
            }
            const ownerBodies = evaluateQuery(context, qOwnerBody(smFaceQuery[0]));
            if (size(ownerBodies) != 1)
            {
                throw "Sheet metal face does not have exactly one owner body.";
            }
            const ownerBody = ownerBodies[0];
            var existingFaceQueries = bodyToFacesMap[ownerBody];
            if (existingFaceQueries == undefined)
            {
                existingFaceQueries = [];
            }
            bodyToFacesMap[ownerBody] = append(existingFaceQueries, query);
        }

        var index = 0;
        for (var entry in bodyToFacesMap)
        {
            var ownerBody = entry.key;
            var faceQueries = qUnion(entry.value);

            const originalEntities = evaluateQuery(context, qOwnedByBody(ownerBody));
            definition.faceTargets = faceQueries;
            definition.sheetMetalModel = ownerBody;
            var subId = id + unstableIdComponent(index);
            splitAndUpdateSM(context, subId, definition, originalEntities);
            processSubfeatureStatus(context, id, {
                        "subfeatureId" : subId
                    });
            index += 1;
        }
    }, {});

function performSheetMetalSplit(context is Context, id is Id, definition is map) returns map
{
    const splitId = id + "split";
    definition.splitType = SplitType.FACE;
    definition.faceTools = definition.tool;
    performRegularSplit(context, splitId, definition);
    var newEdgeQuery = qCreatedBy(splitId, EntityType.EDGE);
    return { newEdges : newEdgeQuery, modifiedFaces : qEdgeAdjacent(newEdgeQuery, EntityType.FACE) };
}

function performSheetMetalSplitWithExtend(context is Context, id is Id, definition is map) returns map
{
    definition.targets = definition.faceTargets;
    definition.thickness = getModelParameters(context, definition.sheetMetalModel).thickness;

    splitFacesFromSingleSheetMetalModel(context, id, definition);
    var newEdgeQuery = qCreatedBy(id, EntityType.EDGE);
    return { newEdges : newEdgeQuery, modifiedFaces : qEdgeAdjacent(newEdgeQuery, EntityType.FACE) };
}

function splitFacesFromSingleSheetMetalModel(context is Context, id is Id, definition is map)
{
    var index = 0;
    for (var evaluatedFace in evaluateQuery(context, definition.targets))
    {
        index += 1;
        const subId = id + unstableIdComponent(index);
        const toolRoot = subId + "tool";
        var sheetMetalFaceQuery = qEntityFilter(qUnion(getSMDefinitionEntities(context, evaluatedFace)), EntityType.FACE);

        const optimisticSplit = defineFeature(function(context is Context, id is Id, definition is map)
            {
                if (!splitSMFace(context, id + "split", definition))
                {
                    throw regenError("Optimistic split failed");
                }
            }, {});
        try
        {
            optimisticSplit(context, subId + "split", { tool : definition.tool, target : sheetMetalFaceQuery });
            continue;
        }

        // Otherwise, so long as the sheet metal part face is split, extend the split across the sheet metal model face.
        try
        {
            const info = createSplitFaceInfo(context, definition.thickness, toolRoot, evaluatedFace, definition.tool);

            if (info.featureStatus != undefined)
            {
                reportFeatureStatus(context, id, info.featureStatus);
            }
            if (info.featureInfo != ErrorStringEnum.SPLIT_FACE_NO_CHANGE)
            {

                var sheetMetalPlane = evPlane(context, { "face" : sheetMetalFaceQuery });
                const extendedTools = qUnion([createExtensions(context, toolRoot + "extension", info.splitVertexLocations, info.splitEdgeVertexLocations, sheetMetalPlane, evaluatedFace),
                            definition.tool]);

                var edgeTools = qConstructionFilter(qEntityFilter(extendedTools, EntityType.EDGE),
                    ConstructionObject.NO);
                var bodyTools = qConstructionFilter(qEntityFilter(extendedTools, EntityType.BODY),
                    ConstructionObject.NO);
                var planeTools = qEntityFilter(extendedTools, EntityType.FACE);
                const faceSplitDefinition = { "edgeTools" : edgeTools,
                        "bodyTools" : bodyTools,
                        "planeTools" : planeTools,
                        "faceTargets" : sheetMetalFaceQuery,
                        "splitType" : SplitType.FACE,
                        "direction" : sheetMetalPlane.normal };
                opSplitFace(context, subId + "split", faceSplitDefinition);
            }
        }

        try
        {
            opDeleteBodies(context, subId + "delete", {
                        "entities" : qCreatedBy(toolRoot)
                    });
        }
    }
}

function addLineSegment(context is Context, id is Id, start is Vector, end is Vector) returns Query
{
    opFitSpline(context, id, {
                "points" : [start, end]
            });
    return qCreatedBy(id, EntityType.EDGE);
}

function createExtensions(context is Context, id is Id, splitVertexLocations is array, splitEdgeVertexLocations is array, sheetMetalModelFacePlane is Plane, sheetMetalPartFace is Query) returns Query
{
    var index = 0;
    var newEntityQueries = [];
    var facePlane = evPlane(context, { "face" : sheetMetalPartFace });
    for (var splitVertexLocation in splitVertexLocations)
    {
        const originalVertexQuery = qContainsPoint(qVertexAdjacent(sheetMetalPartFace, EntityType.VERTEX), splitVertexLocation);
        var sheetMetalVertexQuery = qEntityFilter(qUnion(getSMDefinitionEntities(context, originalVertexQuery)), EntityType.VERTEX);
        const smPointLocation = try(evVertexPoint(context, {
                        "vertex" : sheetMetalVertexQuery
                    }));
        if (smPointLocation == undefined)
        {
            continue;
        }
        const wireId = id + unstableIdComponent(index) + "wire";
        const sheetMetalVertexLocation = evVertexPoint(context, {
                    "vertex" : sheetMetalVertexQuery
                });
        const newEdgeQuery = addLineSegment(context, wireId, splitVertexLocation, sheetMetalVertexLocation);
        newEntityQueries = append(newEntityQueries, newEdgeQuery);
        index += 1;
    }

    for (var vertexLocation in splitEdgeVertexLocations)
    {
        var originatingEdge = qContainsPoint(qEdgeAdjacent(sheetMetalPartFace, EntityType.EDGE), project(facePlane, vertexLocation));
        const sheetMetalModelEdge = qEntityFilter(qUnion(getSMDefinitionEntities(context, originatingEdge)), EntityType.EDGE);
        const distanceResult = evDistance(context, {
                    "side0" : project(sheetMetalModelFacePlane, vertexLocation),
                    "side1" : sheetMetalModelEdge
                });
        if (distanceResult.distance > TOLERANCE.zeroLength * meter)
        {
            const wireId = id + unstableIdComponent(index) + "wire";
            const newEdgeQuery = addLineSegment(context, wireId, vertexLocation, distanceResult.sides[1].point);
            newEntityQueries = append(newEntityQueries, newEdgeQuery);
            index += 1;
        }
    }

    return qUnion(newEntityQueries);
}

/**
 * returns {"featureStatus" : status of split feature for propagation after the feature this starts is aborted,
 *          "splitEdgeVertexLocations" : locations of vertices that split creates by splitting edges of the target face,
 *          "splitVertexLocations" : locations of vertices created by the split operation, will later be checked against
 *                                   the ones on the sheet metal part face}
 */
function createSplitFaceInfo(context is Context, thickness, toolRoot is Id, createFrom is Query, tool is Query)
{
    const extrudeId = toolRoot + "extrude";
    const splitId = toolRoot + "split";
    var info = {};
    {
        var started = false;
        var token is map = {};
        try
        {
            const token = startFeature(context, toolRoot);
            started = true;

            const direction = -evPlane(context, {
                                "face" : createFrom
                            }).normal;
            const extrudeDefinition = { "entities" : createFrom,
                    "direction" : direction,
                    "endBound" : BoundingType.BLIND,
                    "endDepth" : thickness / 2
                };
            opExtrude(context, extrudeId, extrudeDefinition);

            var splitDefinition = { "splitType" : SplitType.FACE,
                "faceTools" : tool,
                "faceTargets" : qEntityFilter(qCapEntity(extrudeId, false), EntityType.FACE),
                "keepTools" : true };
            performRegularSplit(context, splitId, splitDefinition);

            const splitVertexQ = qCreatedBy(splitId, EntityType.VERTEX);
            const evaluatedSplitVertices = evaluateQuery(context, splitVertexQ);
            var splitEdgeVertexQuery = qIntersection([qCreatedBy(splitId, EntityType.VERTEX),
                    qVertexAdjacent(qSplitBy(splitId, EntityType.EDGE, false), EntityType.VERTEX)]);
            const evaluatedSplitEdgeVertices = evaluateQuery(context, splitEdgeVertexQuery);

            var extractPoint = function(vertexQuery)
                {
                    return evVertexPoint(context, {
                                "vertex" : vertexQuery
                            });
                };
            var splitVertexLocations = mapArray(evaluatedSplitVertices, extractPoint);
            var splitEdgeVertexLocations = mapArray(evaluatedSplitEdgeVertices, extractPoint);
            var subStatus is FeatureStatus = getFeatureStatus(context, splitId);
            info = {"featureStatus" : subStatus,
                    "splitEdgeVertexLocations" : splitEdgeVertexLocations,
                    "splitVertexLocations" : splitVertexLocations};
        }
        if (started)
        {
            @abortFeature(context, toolRoot, token);
        }
        return info;
    }
}

function splitSMFace(context is Context, id is Id, definition is map) returns boolean
{
    try
    {
        const splitDefinition = { "splitType" : SplitType.FACE,
                "faceTools" : definition.tool,
                "faceTargets" : definition.target,
                "keepTools" : true };
        performRegularSplit(context, id, splitDefinition);
        return size(evaluateQuery(context, qCreatedBy(id))) != 0;
    }
    catch
    {
        return false;
    }
}

