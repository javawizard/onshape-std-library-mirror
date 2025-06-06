FeatureScript 2679; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "2679.0");
import(path : "onshape/std/boolean.fs", version : "2679.0");
import(path : "onshape/std/containers.fs", version : "2679.0");
import(path : "onshape/std/coordSystem.fs", version : "2679.0");
import(path : "onshape/std/curveGeometry.fs", version : "2679.0");
import(path : "onshape/std/debug.fs", version : "2679.0");
import(path : "onshape/std/extrude.fs", version : "2679.0");
import(path : "onshape/std/evaluate.fs", version : "2679.0");
import(path : "onshape/std/feature.fs", version : "2679.0");
import(path : "onshape/std/math.fs", version : "2679.0");
import(path : "onshape/std/matrix.fs", version : "2679.0");
import(path : "onshape/std/path.fs", version : "2679.0");
import(path : "onshape/std/query.fs", version : "2679.0");
import(path : "onshape/std/sketch.fs", version : "2679.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2679.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2679.0");
import(path : "onshape/std/smjointtype.gen.fs", version : "2679.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2679.0");
import(path : "onshape/std/string.fs", version : "2679.0");
import(path : "onshape/std/topologyUtils.fs", version : "2679.0");
import(path : "onshape/std/units.fs", version : "2679.0");
import(path : "onshape/std/valueBounds.fs", version : "2679.0");
import(path : "onshape/std/vector.fs", version : "2679.0");
import(path : "onshape/std/extendsheetboundingtype.gen.fs", version : "2679.0");

const FLANGE_BEND_ANGLE_BOUNDS =
{
    (degree) : [1, 90, 179],
    (radian) : 1
} as AngleBoundSpec;

const FLANGE_DIRECTION_ANGLE_BOUNDS =
{
    (degree) : [0, 90, 180],
    (radian) : 1
} as AngleBoundSpec;

const FLANGE_MITER_ANGLE_BOUNDS =
{
    (degree) : [0, 45, 179],
    (radian) : 1
} as AngleBoundSpec;


/**
 * @internal
 * Bounding types of sheet metal flange
 */
export enum SMFlangeBoundingType
{
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Up to entity" }
    UP_TO_ENTITY,
    annotation { "Name" : "Up to entity with offset" }
    UP_TO_ENTITY_OFFSET
}

/**
 * @internal
 * Angle types of sheet metal flange
 */
export enum SMFlangeAngleControlType
{
    annotation { "Name" : "Bend angle" }
    BEND_ANGLE,
    annotation { "Name" : "Align to geometry" }
    ALIGN_GEOMETRY,
    annotation { "Name" : "Angle from direction"}
    ANGLE_FROM_DIRECTION
}

/**
*  Describes the position of a virtual sharp with respect to "flange face", defined as the face that
*  corresponds to the edge selected for flange in the underlying sheet.

*  For Middle alignment, the virtual sharp at the intersection of the existing sheet
*  and the sheet that corresponds to the flange wall, lies in the middle of the flange face.
*
*  For Inner alignment, the virtual sharp at the intersection of the planes defined by the faces
*  of the solid to the interior of the bend, lies coincident with the edge of the flange face
*  closest to the bend.
*
*  For Outer alignment, the virtual sharp at the intersection of the planes defined by the faces
*  of the solid to the exterior of the bend, lies coincident with the edge of the flange face
*  farthest from the bend.
*/
export enum SMFlangeAlignment
{
    annotation { "Name" : "Inner" }
    INNER,
    annotation { "Name" : "Outer" }
    OUTER,
    annotation { "Name" : "Middle" }
    MIDDLE,
    annotation { "Name" : "Hold line" }
    BEND
}



/**
 * Sets the handling for chains of edges while creating a partial flange.
 * @value PER_EDGE: The partial flange conditions will be applied to the ends of each individual edge.
 * @value PER_CHAIN: The partial flange conditions will be applied to the ends of each chain of selected edges.
 */
export enum SMPartialFlangeChainType
{
    annotation { "Name" : "Per edge" }
    PER_EDGE,
    annotation { "Name" : "Per chain" }
    PER_CHAIN
}

const SURFACE_SUFFIX = "surface";
const PARTIAL_FLANGE_ON_START_EDGE_MANIPULATOR_ID = "Partial flange on start edge manipulator";
const PARTIAL_FLANGE_ON_END_EDGE_MANIPULATOR_ID = "Partial flange on end edge manipulator";
const PARTIAL_FLANGE_ON_START_ENTITY_OFFSET_MANIPULATOR_ID = "Partial flange on start entity offset manipulator";
const PARTIAL_FLANGE_ON_END_ENTITY_OFFSET_MANIPULATOR_ID = "Partial flange on end entity offset manipulator";

/**
* Create sheet metal flanges on selected edges of sheet metal parts. Length of flange may be
* defined by distance (as measured from the virtual sharp along outer edge of wall) or limiting
* entity. Bend angle may be flipped using the oppositeDirection flag. When auto-miter is not
* selected, flange sides are rotated by miter angle.
*/
annotation { "Feature Type Name" : "Flange",
             "Manipulator Change Function" : "flangeManipulatorChange",
             "Filter Selector" : "allparts",
             "Editing Logic Function" : "flangeEditLogic" }
export const sheetMetalFlange = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
precondition
{
    // This should match sheetMetalHem
    annotation { "Name" : "Edges or side faces to flange",
                "Filter" : ModifiableEntityOnly.YES && SheetMetalDefinitionEntityType.EDGE
                && ((GeometryType.LINE && AllowFlattenedGeometry.YES) || (GeometryType.PLANE && AllowFlattenedGeometry.NO)) }
    definition.edges is Query;

    annotation { "Name" : "Flange alignment", "UIHint" : UIHint.SHOW_LABEL }
    definition.flangeAlignment is SMFlangeAlignment;

    annotation { "Name" : "End type", "UIHint" : UIHint.SHOW_LABEL }
    definition.limitType is SMFlangeBoundingType;

    if (definition.limitType == SMFlangeBoundingType.BLIND)
    {
        annotation { "Name" : "Distance" }
        isLength(definition.distance, NONNEGATIVE_LENGTH_BOUNDS);
    }
    else if (definition.limitType == SMFlangeBoundingType.UP_TO_ENTITY || definition.limitType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
    {
        annotation { "Name" : "Up to entity", "Filter" : EntityType.FACE || EntityType.VERTEX, "MaxNumberOfPicks" : 1 }
        definition.limitEntity is Query;
        if (definition.limitType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
        {
            annotation { "Name" : "Offset" }
            isLength(definition.offset, NONNEGATIVE_LENGTH_BOUNDS);

            annotation { "Name" : "Opposite offset direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.oppositeOffsetDirection is boolean;
        }
    }

    annotation { "Name" : "Angle control type" }
    definition.angleControlType is SMFlangeAngleControlType;

    annotation { "Name" : "Opposite side", "UIHint" : UIHint.OPPOSITE_DIRECTION }
    definition.oppositeDirection is boolean;

    if (definition.angleControlType == SMFlangeAngleControlType.BEND_ANGLE)
    {
        annotation { "Name" : "Bend angle" }
        isAngle(definition.bendAngle, FLANGE_BEND_ANGLE_BOUNDS);
    }
    else if (definition.angleControlType == SMFlangeAngleControlType.ALIGN_GEOMETRY)
    {
        annotation { "Name" : "Parallel to", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION && !BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
        definition.parallelEntity is Query;
    }
    else if (definition.angleControlType == SMFlangeAngleControlType.ANGLE_FROM_DIRECTION)
    {
        annotation { "Name" : "Direction", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION && !BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
        definition.directionEntity is Query;

        annotation { "Name" : "Angle" }
        isAngle(definition.angleFromDirection, FLANGE_DIRECTION_ANGLE_BOUNDS);

        annotation { "Name" : "Opposite angle", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.angleFromDirectionOppositeAngle is boolean;
    }

    annotation { "Name" : "Automatic miter", "Default" : true }
    definition.autoMiter is boolean;
    if (!definition.autoMiter)
    {
        annotation { "Name" : "Miter angle" }
        isAngle(definition.miterAngle, FLANGE_MITER_ANGLE_BOUNDS);
    }

    annotation { "Name" : "Use model bend radius", "Default" : true }
    definition.useDefaultRadius is boolean;
    if (!definition.useDefaultRadius)
    {
        annotation { "Name" : "Bend radius" }
        isLength(definition.bendRadius, SM_BEND_RADIUS_BOUNDS);
    }

    partialFlangePredicate(definition);
}
{
    // this is not necessary but helps with correct error reporting in feature pattern
    checkNotInFeaturePattern(context, definition.edges, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

    const edgesArr = getSMDefinitionEdgesForFlangeTypeFeature(context, id, {
                "edges" : definition.edges,
                "errorForNoEdges" : ErrorStringEnum.SHEET_METAL_FLANGE_NO_EDGES,
                "errorForInternal" : ErrorStringEnum.SHEET_METAL_FLANGE_INTERNAL,
                "errorForNonLinearEdges" : ErrorStringEnum.SHEET_METAL_FLANGE_NON_LINEAR_EDGES,
                "errorForEdgesNextToCylinderBend" : ErrorStringEnum.SHEET_METAL_FLANGE_NEXT_TO_CYLINDER_BEND,
                "improveConsistency" : isAtVersionOrLater(context, FeatureScriptVersionNumber.V1048_FLANGE_AND_HEM_EDGES)
            });
    const edges = qUnion(edgesArr);

    // if any edge is adjacent to a cone and alignment is not bend, fail.
    const adjacentConeFacesQ = edges->qAdjacent(AdjacencyType.EDGE, EntityType.FACE)->qGeometry(GeometryType.CONE);
    if (!isQueryEmpty(context, adjacentConeFacesQ) && definition.flangeAlignment != SMFlangeAlignment.BEND)
    {
        setErrorEntities(context, id, { "entities" : qUnion([adjacentConeFacesQ, qAdjacent(adjacentConeFacesQ, AdjacencyType.EDGE, EntityType.EDGE)]) });
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_ADJACENT_CONE, ["edges"]);
    }

    removeCornerBreaksAtEdgeVertices(context, edges);
    var edgeMaps = groupEdgesByBodyOrModel(context, edgesArr);
    var modelToEdgeMap = edgeMaps.modelToEdgeMap;
    if (size(modelToEdgeMap) > 1 && definition.useDefaultRadius)
        throw regenError(ErrorStringEnum.SHEET_METAL_MULTI_SM_DEFAULT_RADIUS, ["useDefaultRadius"]);
    if (definition.oppositeOffsetDirection)
        definition.offset *= -1;
    if (!definition.autoMiter)
        definition.adjustedMiterAngle = getAngleForAngledMiter(definition);
    if (definition.angleControlType != SMFlangeAngleControlType.BEND_ANGLE)
        definition = mergeMaps(definition, processParallelEntity(context, definition));

    definition.useExternalDisambiguation = isAtVersionOrLater(context, FeatureScriptVersionNumber.V500_EXTERNAL_DISAMBIGUATION);

    //get originals before any changes
    var smBodies = evaluateQuery(context, qOwnerBody(edges));
    var smBodiesQ = qUnion(smBodies);
    const initialData = getInitialEntitiesAndAttributes(context, smBodiesQ);
    const robustSMBodiesQ = qUnion([smBodiesQ, startTracking(context, smBodiesQ)]);

    var containerToLoop = isAtVersionOrLater(context, FeatureScriptVersionNumber.V483_FLAT_QUERY_EVAL_FIX) ? edgeMaps.bodyToEdgeMap : edgeMaps.modelToEdgeMap;
    var flangeDataOverrides = {};
    if (definition.isPartialFlange)
    {
        var modelIndex = 0;
        const splitIdBase = id + "split";
        for (var key, value in containerToLoop)
        {
            const limitEntitiesAndSplitEdges = splitAllEdgesForPartialFlange(context, id, splitIdBase + unstableIdComponent(modelIndex), definition, qUnion(value));
            modelIndex += 1;
            flangeDataOverrides = mergeMaps(flangeDataOverrides, limitEntitiesAndSplitEdges.limitEntities);
            containerToLoop[key] = limitEntitiesAndSplitEdges.splitEdgeQueries;
        }
    }

    var bodiesToDelete = new box([]);
    var objectCounter = 0; // counter for all sheet metal objects created. Guarantees unique attribute ids.
    for (var entry in containerToLoop)
    {
        objectCounter = updateSheetMetalModelForFlange(context, id, objectCounter, qUnion(entry.value), definition, flangeDataOverrides, bodiesToDelete);
    }

    if (!isQueryEmpty(context, qUnion(bodiesToDelete[])))
    {
        // Delete the helper bodies that are no longer needed
        opDeleteBodies(context, id + "deleteBodies", {
                    "entities" : qUnion(bodiesToDelete[])
                });
    }
    // Add association attributes where needed and compute deleted attributes
    var toUpdate = assignSMAttributesToNewOrSplitEntities(context, robustSMBodiesQ, initialData, id);
    updateSheetMetalGeometry(context, id, { "entities" : toUpdate.modifiedEntities,
                "deletedAttributes" : toUpdate.deletedAttributes });

}, { oppositeDirection : false, limitType : SMFlangeBoundingType.BLIND, flangeAlignment : SMFlangeAlignment.INNER,
        angleControlType : SMFlangeAngleControlType.BEND_ANGLE, autoMiter : true, useDefaultRadius : true, oppositeOffsetDirection : false,
        isPartialFlange : false, hasSecondBound : false, firstBoundingEntityOppositeOffsetDirection : false,
        secondBoundingEntityOppositeOffsetDirection : false });

/*
 * Create a linear manipulators for the Partial Flange
 */
function addPartialFlangeManipulators(context is Context, topLevelId is Id, definition is map, flangeBound is PartialFlangeBound, manipPosition is map)
{
    const firstBound = flangeBound.isFirstBound;
    const blind = flangeBound.partialFlangeType == SMFlangeBoundingType.BLIND;
    const upToEntityOffset = flangeBound.partialFlangeType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET;
    var manipulatorName;
    var primaryParameterId;
    var offset;

    if (blind && firstBound)
    {
        manipulatorName = PARTIAL_FLANGE_ON_START_EDGE_MANIPULATOR_ID;
        primaryParameterId = "firstBoundOffset";
        offset = definition.firstBoundOffset;
    }
    else if (blind)
    {
        manipulatorName = PARTIAL_FLANGE_ON_END_EDGE_MANIPULATOR_ID;
        primaryParameterId = "secondBoundOffset";
        offset = definition.secondBoundOffset;
    }
    else if (upToEntityOffset && firstBound)
    {
        manipulatorName = PARTIAL_FLANGE_ON_START_ENTITY_OFFSET_MANIPULATOR_ID;
        primaryParameterId = "firstBoundingEntityOffset";
        offset = definition.firstBoundingEntityOffset;
    }
    else if (upToEntityOffset)
    {
        manipulatorName = PARTIAL_FLANGE_ON_END_ENTITY_OFFSET_MANIPULATOR_ID;
        primaryParameterId = "secondBoundingEntityOffset";
        offset = definition.secondBoundingEntityOffset;
    }
    addPartialFlangeManipulator(context, topLevelId, manipulatorName, primaryParameterId, offset, manipPosition);
}

/*
 * Create a linear manipulator for the Partial Flange
 */
function addPartialFlangeManipulator(context is Context, id is Id, manipulatorName is string, primaryParameterId is string, offset is ValueWithUnits, manipulatorPosition is Line)
{
    addManipulators(context, id, { (manipulatorName) :
                linearManipulator({
                        "base" : manipulatorPosition.origin,
                        "direction" : manipulatorPosition.direction,
                        "offset" : offset,
                        "minValue" : 0 * meter,
                        "style" : ManipulatorStyleEnum.TANGENTIAL,
                        "primaryParameterId" : primaryParameterId }) });
}

function groupEdgesByBodyOrModel(context is Context, edges is array) returns map
{
    var modelToEdgeMap = {};
    var bodyToEdgeMap = {};
    for (var edge in edges)
    {
        var ownerBody = qOwnerBody(edge);
        const attributes = getSmObjectTypeAttributes(context, ownerBody, SMObjectType.MODEL);
        const attributeId = attributes[0].attributeId;
        if (modelToEdgeMap[attributeId] != undefined)
            modelToEdgeMap[attributeId] = append(modelToEdgeMap[attributeId], edge);
        else
            modelToEdgeMap[attributeId] = [edge];

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V483_FLAT_QUERY_EVAL_FIX))
        {
            var ownerEvaluated = evaluateQuery(context, ownerBody);
            if (size(ownerEvaluated) != 1)
                throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL);

            if (bodyToEdgeMap[ownerEvaluated[0]] != undefined)
                bodyToEdgeMap[ownerEvaluated[0]] = append(bodyToEdgeMap[ownerEvaluated[0]], edge);
            else
                bodyToEdgeMap[ownerEvaluated[0]] = [edge];
        }
    }

    return { "modelToEdgeMap" : modelToEdgeMap, "bodyToEdgeMap" : bodyToEdgeMap };
}

/**
 * @internal
 *  Editing logic makes sure that when the user deselects default radius option, we default to the default radius
 *  in the input field (if the user didn't already change it)
 */
export function flangeEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    var edges = try silent(qUnion(getSMDefinitionEntities(context, definition.edges, EntityType.EDGE)));
    if (edges == undefined)
        return definition;
    var evaluatedEdgeQuery = evaluateQuery(context, edges);
    if (size(evaluatedEdgeQuery) == 0)
        return definition;

    var modelParams = try(getModelParametersFromEdge(context, evaluatedEdgeQuery[0]));
    if (modelParams == undefined)
        return definition;

    if (!specifiedParameters.useDefaultRadius)
    {
        var modelToEdgeMap = groupEdgesByBodyOrModel(context, evaluatedEdgeQuery).modelToEdgeMap;
        if (size(modelToEdgeMap) > 1)
        {
            definition.useDefaultRadius = false;
        }
    }
    if (!definition.useDefaultRadius && !specifiedParameters.bendRadius &&
        definition.useDefaultRadius != oldDefinition.useDefaultRadius) // do this only once
    {
        definition.bendRadius = modelParams.defaultBendRadius;
    }
    if (isCreating && !specifiedParameters.offset)
        definition.offset = modelParams.minimalClearance;

    //make sure we're pointing in the direction of the limit entity
    if (isCreating && specifiedParameters.limitEntity && !specifiedParameters.oppositeDirection)
    {
        var flangeData = getFlangeData(context, id, evaluatedEdgeQuery[0], definition);
        const edgePoint = flangeData.edgeEndPoints[0].origin;
        var pointOnLimit = undefined;
        var planeResult = try silent(evPlane(context, { "face" : definition.limitEntity }));
        if (planeResult == undefined)
        {
            pointOnLimit = try silent(evVertexPoint(context, { "vertex" : definition.limitEntity }));
        }
        else
        {
            pointOnLimit = planeResult.origin;
        }
        if (pointOnLimit == undefined)
            return definition;

        var upToDirection = pointOnLimit - edgePoint;
        if (dot(upToDirection, flangeData.direction) < TOLERANCE.zeroLength * meter)
        {
            definition.oppositeDirection = true;
        }
    }

    const adjacentConeFaces = edges->qAdjacent(AdjacencyType.EDGE, EntityType.FACE)->qGeometry(GeometryType.CONE);
    if (!isQueryEmpty(context, adjacentConeFaces) && isCreating)
    {
        if (!specifiedParameters.flangeAlignment)
        {
            definition.flangeAlignment = SMFlangeAlignment.BEND; //only alignment that works with cone edges
        }
    }

    return definition;
}

/**
 * @internal
 * Manipulator change function for `flange`.
 */
export function flangeManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    try
    {
        for (var key, manipulator in newManipulators)
        {
            if (tolerantEquals(manipulator.offset, TOLERANCE.zeroLength * meter))
                continue;

            if (key == PARTIAL_FLANGE_ON_START_EDGE_MANIPULATOR_ID)
            {
                definition.firstBoundOffset = manipulator.offset;
            }
            else if (key == PARTIAL_FLANGE_ON_END_EDGE_MANIPULATOR_ID)
            {
                definition.secondBoundOffset = manipulator.offset;
            }
            else if (key == PARTIAL_FLANGE_ON_START_ENTITY_OFFSET_MANIPULATOR_ID)
            {
                definition.firstBoundingEntityOffset = manipulator.offset;
            }
            else if (key == PARTIAL_FLANGE_ON_END_ENTITY_OFFSET_MANIPULATOR_ID)
            {
                definition.secondBoundingEntityOffset = manipulator.offset;
            }
        }
    }
    return definition;
}

function updateSheetMetalModelForFlange(context is Context, topLevelId is Id, objectCounter is number, edges is Query, definition is map, flangeDataOverrides is map, bodiesToDelete is box) returns number
{
    const originalFlangeEdges = evaluateQuery(context, edges);

    // add thickness, minimalClearance and defaultBendRadius to definition.
    // Flange uses thickness, minimalClearance and potentially defaultBendRadius derived from underlying sheet metal model
    definition = mergeMaps(definition, getModelParametersFromEdge(context, originalFlangeEdges[0]));
    if (definition.useDefaultRadius)
    {
        definition.bendRadius = definition.defaultBendRadius;
    }
    definition.inFlangeThickness = inFlangeThickness(definition);

    // Collect flangeData for each edge, and store a mapping from each edge to a tracking query of itself
    var edgeToFlangeData = {};
    var oldEdgeToNewEdge = {};
    for (var edge in originalFlangeEdges)
    {
        edgeToFlangeData[edge] = getFlangeData(context, topLevelId, edge, definition);
        oldEdgeToNewEdge[edge] = qUnion([edge, startTracking(context, edge)]);
    }

    // Extend or retract each wall that is receiving a flange to comply with user specified SMFlangeAlignment.
    var edgeToExtensionDistance = collectEdgeToExtensionDistance(context, topLevelId, edges, edgeToFlangeData, definition);

    var alignmentChanges = changeUnderlyingSheetForAlignment(context, topLevelId, topLevelId + unstableIdComponent(objectCounter),
    definition.useExternalDisambiguation, edges, edgeToFlangeData, oldEdgeToNewEdge, edgeToExtensionDistance);
    var originalCornerVertices = alignmentChanges.cornerVertices;
    var modifiedEntities = alignmentChanges.modifiedEntities;
    edges = alignmentChanges.updatedEdges;

    edgeToFlangeData = updateEdgeToFlangeDataAfterAlignmentChange(context, topLevelId, originalFlangeEdges, edgeToFlangeData,
        oldEdgeToNewEdge, edgeToExtensionDistance);

    // Collect information about the shape of each flange
    var edgeToSideAndBase = collectEdgeToSideAndBase(context, topLevelId, definition.useExternalDisambiguation, edges,
    originalCornerVertices, edgeToFlangeData, definition, flangeDataOverrides);
    var edgeToFlangeDistance = collectEdgeToFlangeDistance(context, topLevelId, edges, edgeToFlangeData, edgeToSideAndBase, definition);

    // Sketch each flange and add it to the underlying sheet body
    var surfaceBodies = [];
    var originalEntities = [];
    var trackingBendEdges = [];
    var matches = [];

    var setBendAttributesAfterBoolean = isAtVersionOrLater(context, FeatureScriptVersionNumber.V695_SM_SWEPT_SUPPORT);
    for (var edge in evaluateQuery(context, edges))
    {
        var ownerBody = qOwnerBody(edge);
        originalEntities = append(originalEntities, qSubtraction(qOwnedByBody(ownerBody), modifiedEntities));
        var indexedId = topLevelId + unstableIdComponent(objectCounter);
        if (definition.useExternalDisambiguation)
        {
            setExternalDisambiguation(context, indexedId, edge);
        }
        var surfaceId = indexedId + SURFACE_SUFFIX;

        if (edgeToFlangeData[edge].isConeAdjacent)
        {
            const result = createFlangeSurfacesAdjacentToCone(context, topLevelId, indexedId, edge, edgeToFlangeData[edge],
                        edgeToSideAndBase[edge], edgeToFlangeDistance[edge], definition, bodiesToDelete); //bend and wall

            addBendAttribute(context, result.bendFace, edgeToFlangeData[edge], topLevelId, objectCounter, definition);
            objectCounter += 1;


            setAttribute(context, {
                        "entities" : result.wallFace,
                        "attribute" : makeSMWallAttribute(toAttributeId(topLevelId + objectCounter))
            });
            objectCounter += 1;

            surfaceBodies = append(surfaceBodies, qUnion([qOwnerBody(result.bendFace), qOwnerBody(result.wallFace)]));
            matches = concatenateArrays([matches, createMatches(context, edge, result)]);
            continue;
        }

        var bendEdge = createFlangeSurfaceReturnBendEdge(context, topLevelId, indexedId, edge, edgeToFlangeData[edge],
        edgeToSideAndBase[edge], edgeToFlangeDistance[edge], definition);
        if (setBendAttributesAfterBoolean)
        {
            trackingBendEdges = append(trackingBendEdges, qUnion([bendEdge, startTracking(context, bendEdge)]));
        }
        else
        {
            addBendAttribute(context, bendEdge, edgeToFlangeData[edge], topLevelId, objectCounter, definition);
            objectCounter += 1;
        }
        //add wall attributes to faces
        for (var face in evaluateQuery(context, qCreatedBy(surfaceId, EntityType.FACE)))
        {
            setAttribute(context, {
                        "entities" : face,
                        "attribute" : makeSMWallAttribute(toAttributeId(topLevelId + objectCounter))
                    });
            objectCounter += 1;
        }
        surfaceBodies = append(surfaceBodies, qCreatedBy(surfaceId, EntityType.BODY));
    }

    var originalBodies = qOwnerBody(qUnion(originalEntities));
    var indexedId = topLevelId + unstableIdComponent(objectCounter);
    var attributeIdCounter = new box(objectCounter);

    mergeSheetMetal(context, indexedId + ("flange_boolean"), {
                "topLevelId" : topLevelId,
                "surfacesToAdd" : qUnion(surfaceBodies),
                "originalSurfaces" : originalBodies,
                "matches" : matches,
                "detectAdjacencyForSheets" : true,
                "trackingBendEdges" : trackingBendEdges,
                "bendRadius" : definition.bendRadius,
                "attributeIdCounter" : attributeIdCounter,
                "error" : ErrorStringEnum.SHEET_METAL_FLANGE_FAIL,
                "errorParameters" : ["edges"],
                "legacyId" : (definition.useExternalDisambiguation) ? topLevelId : indexedId,
                "holdBendAdjacentEdges" : definition.isPartialFlange && definition.holdAdjacentEdges });

    return attributeIdCounter[];
}


function createMatches(context is Context, edge is Query, inputData is map)
{
    const arcSheetEdges =  qAdjacent(inputData.bendFace, AdjacencyType.EDGE, EntityType.EDGE);
    const coincidentToEdge = evaluateQuery(context, qContainsPoint(arcSheetEdges, inputData.edgeMidpt));
    if (size(coincidentToEdge) != 1)
    {
        // Flange extrusion did not result in expected edge
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL);
    }

    var matches = [{
            "topology1" : edge,
            "topology2" : coincidentToEdge[0],
            "matchType" : TopologyMatchType.COINCIDENT
        }];


    const wallSheetEdges = qAdjacent(inputData.wallFace, AdjacencyType.EDGE, EntityType.EDGE);
    const matchedBetweenArcAndOther = evaluateQuery(context, qContainsPoint(qUnion([arcSheetEdges, wallSheetEdges]), inputData.otherEdgePt));
    if (size(matchedBetweenArcAndOther) != 2)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL);
    }

    matches = append(matches, {
                "topology1" : matchedBetweenArcAndOther[0],
                "topology2" : matchedBetweenArcAndOther[1],
                "matchType" : TopologyMatchType.COINCIDENT
            });
    return matches;
}

/**
 * Depending on whether we want to align the flange to the inner or outer wall face, we will need to move
 * the edge selected for flange, therefore changing the underlying sheet face.
 * Returns a map of results:
 *  updatedEdges : all flange edges after applying alignment changes where necessary.
 *  modifiedEntities : all changed entities so that association attributes can be added correctly.
 *  cornerVertices : we keep track of all vertices that started as a laminar corner, as those do not
 *                   need tweaking to find corresponding vertices after alignment.
 */
function changeUnderlyingSheetForAlignment(context is Context, topLevelId is Id, id is Id, useExternalDisambiguation is boolean,
    edges is Query, edgeToFlangeData is map, oldEdgeToNewEdge is map, edgeToExtensionDistance is map)
{
    var originalCornerVertices = trackCornerVertices(context, edges);

    var changedEntitiesQ;
    var originalFlangeEdges = evaluateQuery(context, edges);
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V744_SM_FLANGE_PATTERN_EDGE_CHANGE))
    {
        changedEntitiesQ = changeUnderlyingSheetBatched(context, topLevelId, id, originalFlangeEdges,
            edgeToFlangeData, edgeToExtensionDistance);
    }
    else
    {
        changedEntitiesQ = changeUnderlyingSheetUnbatched(context, topLevelId, id, useExternalDisambiguation,
            originalFlangeEdges, edgeToFlangeData, oldEdgeToNewEdge, edgeToExtensionDistance);
    }

    var updatedEdges = [];
    for (var e in originalFlangeEdges)
    {
        var newEdge = evaluateQuery(context, oldEdgeToNewEdge[e]);
        if (newEdge == [])
        {
            const errorFace = edgeToFlangeData[e].adjacentFace;
            setErrorEntities(context, topLevelId, { "entities" : qUnion([errorFace, qAdjacent(errorFace, AdjacencyType.EDGE, EntityType.EDGE)]) });
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_ALIGNMENT, ["flangeAlignment"]);
        }
        // checking for multiples happens in updateEdgeToFlangeDataAfterAlignmentChange(...)
        updatedEdges = append(updatedEdges, newEdge[0]);
    }

    return {
            "updatedEdges" : qUnion(updatedEdges),
            "modifiedEntities" : changedEntitiesQ,
            "cornerVertices" : evaluateQuery(context, qUnion(originalCornerVertices))
        };
}

function changeUnderlyingSheetBatched(context is Context, topLevelId is Id, id is Id, flangeEdges is array,
    edgeToFlangeData is map, edgeToExtensionDistance is map) returns Query
{
    var edgesForEdgeChange = [];
    var edgesForExtendSheetBody = [];
    for (var edge in flangeEdges)
    {
        if (abs(edgeToExtensionDistance[edge]) < TOLERANCE.zeroLength * meter)
            continue;

        if (edgeToFlangeData[edge].isConeAdjacent)
        {
            //do not change edge adjacent to cone
            continue;
        }

        // BEL-87432 Flange edges on non-planar walls will not land on the desired plane using offset option of edgeChange.
        // Instead use limitEntity option of extendSheetBody.
        const useOnlyEdgeChange = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V762_FLANGE_NEAR_ROLLED);
        if (useOnlyEdgeChange || evSurfaceDefinition(context, { "face" : edgeToFlangeData[edge].adjacentFace }) is Plane)
        {
            edgesForEdgeChange = append(edgesForEdgeChange, edge);
        }
        else
        {
            edgesForExtendSheetBody = append(edgesForExtendSheetBody, edge);
        }
    }

    var edgeChangeOptions = [];
    for (var edge in edgesForEdgeChange)
    {
        edgeChangeOptions = append(edgeChangeOptions, {
                    "edge" : edge,
                    "face" : edgeToFlangeData[edge].adjacentFace,
                    "offset" : edgeToExtensionDistance[edge]
                });
    }

    var extendSheetBodyOptions = [];
    var trackedEdgesForExtendSheetBody = [];
    for (var edge in edgesForExtendSheetBody)
    {
        const flangeData = edgeToFlangeData[edge];
        var extensionPlane = getExtendedFlangePlane(flangeData, edgeToExtensionDistance[edge]);

        // Ensure plane is facing the correct direction for extendSheetBody
        if (dot(extensionPlane.normal, flangeData.wallExtendDirection) < 0)
        {
            extensionPlane.normal = -1 * extensionPlane.normal;
        }

        // extend sheet body is called after edge change. Make sure we can find the edge even if its identity changes.
        const trackedEdge = qUnion([edge, startTracking(context, edge)]);
        trackedEdgesForExtendSheetBody = append(trackedEdgesForExtendSheetBody, trackedEdge);
        extendSheetBodyOptions = append(extendSheetBodyOptions, {
                    "edge" : trackedEdge,
                    "limitEntity" : extensionPlane,
                    "faceToExtend" : flangeData.adjacentFace
                });
    }

    const edgeChangeId = id + "edgeChange";
    if (size(edgesForEdgeChange) > 0)
    {
        try
        {
            sheetMetalEdgeChangeCall(context, edgeChangeId, qUnion(edgesForEdgeChange), {
                        "edgeChangeOptions" : edgeChangeOptions
                    });
        }
        processSubfeatureStatus(context, topLevelId, { "subfeatureId" : edgeChangeId, "propagateErrorDisplay" : true });
        if (featureHasError(context, topLevelId))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_ALIGNMENT, ["flangeAlignment"]);
        }
    }

    const extendSheetBodyId = id + "extendSheetBody";
    if (size(edgesForExtendSheetBody) > 0)
    {
        // Ensure none of the edges in edgesForExtendSheetBody have been split
        for (var edge in trackedEdgesForExtendSheetBody)
        {
            if (size(evaluateQuery(context, edge)) != 1)
            {
                failDueToAlignmentIssue(context, topLevelId, edge);
            }
        }

        try
        {
            sheetMetalExtendSheetBodyCall(context, extendSheetBodyId, {
                        "entities" : qUnion(trackedEdgesForExtendSheetBody),
                        "extendMethod" : ExtendSheetBoundingType.EXTEND_TO_SURFACE,
                        "edgeLimitOptions" : extendSheetBodyOptions
                    });
        }
        processSubfeatureStatus(context, topLevelId, { "subfeatureId" : extendSheetBodyId, "propagateErrorDisplay" : true });
        if (featureHasError(context, topLevelId))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_ALIGNMENT, ["flangeAlignment"]);
        }
    }

    return qUnion([qUnion(edgesForEdgeChange), qUnion(edgesForExtendSheetBody), qCreatedBy(edgeChangeId), qCreatedBy(extendSheetBodyId)]);
}

// Deprecated.  Use changeUnderlyingSheetBatched(...) instead
function changeUnderlyingSheetUnbatched(context is Context, topLevelId is Id, id is Id, useExternalDisambiguation is boolean,
    originalFlangeEdges is array, edgeToFlangeData is map, oldEdgeToNewEdge is map, edgeToExtensionDistance is map) returns Query
{
    var index = 0;
    var changedEntities = [];
    for (var edge in originalFlangeEdges)
    {
        if (abs(edgeToExtensionDistance[edge]) < TOLERANCE.zeroLength * meter)
            continue;

        var evaluatedEdges = evaluateQuery(context, oldEdgeToNewEdge[edge]);
        if (size(evaluatedEdges) != 1)
        {
            failDueToAlignmentIssue(context, topLevelId, id, useExternalDisambiguation, qUnion(evaluatedEdges));
        }
        var updatedEdge = evaluatedEdges[0];
        var flangeData = edgeToFlangeData[edge];

        //create a plane as a limit surface for extending the underlying sheet
        const planeNormal = isAtVersionOrLater(context, FeatureScriptVersionNumber.V685_EXTEND_SHEET_BODY_STEP_EDGES) ?
            cross(normalize(flangeData.edgeEndPoints[1].origin - flangeData.edgeEndPoints[0].origin), flangeData.wallPlane.normal) : flangeData.plane.normal;
        var edgeMidpoint = .5 * (flangeData.edgeEndPoints[1].origin + flangeData.edgeEndPoints[0].origin);
        var origin = edgeMidpoint + edgeToExtensionDistance[edge] * flangeData.wallExtendDirection;
        var extendIndexedId = id + "extend" + unstableIdComponent(index);
        try
        {
            sheetMetalExtendSheetBodyCall(context, extendIndexedId, {
                        "extendMethod" : ExtendSheetBoundingType.EXTEND_TO_SURFACE,
                        "entities" : updatedEdge,
                        "limitEntity" : plane(origin, planeNormal),
                        "fence" : true
                    });
        }
        catch
        {
            // Error display
            processSubfeatureStatus(context, topLevelId, { "subfeatureId" : extendIndexedId, "propagateErrorDisplay" : true });
            setErrorEntities(context, topLevelId, { "entities" : updatedEdge });
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_ALIGNMENT, ["flangeAlignment"]);
        }
        changedEntities = append(changedEntities, qCreatedBy(extendIndexedId));
        index += 1;
    }
    return qUnion(changedEntities);
}

function failDueToAlignmentIssue(context is Context, topLevelId is Id, id is Id, useExternalDisambiguation is boolean, edges is Query)
{
    setErrorEntities(context, (useExternalDisambiguation) ? topLevelId : id, { "entities" : qAdjacent(edges, AdjacencyType.EDGE, EntityType.FACE) });
    throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_ALIGNMENT, ["flangeAlignment"]);
}

function failDueToAlignmentIssue(context is Context, topLevelId is Id, edges is Query)
{
    failDueToAlignmentIssue(context, topLevelId, newId(), true, edges);
}

function trackCornerVertices(context is Context, edges is Query) returns array
{
    var originalCornerVertices = [];
    var vertices = evaluateQuery(context, qAdjacent(edges, AdjacencyType.VERTEX, EntityType.VERTEX));
    for (var v in vertices)
    {
        var adjacentEdges = evaluateQuery(context, qAdjacent(v, AdjacencyType.VERTEX, EntityType.EDGE));
        if (size(adjacentEdges) == 2)
        {
            //if both edges are being flanged, skip (it's a miter)
            var intersection = qIntersection([qUnion(adjacentEdges), edges]);
            if (size(evaluateQuery(context, intersection)) == 2)
                continue;
            originalCornerVertices = append(originalCornerVertices, startTracking(context, v));
            //have to add original vertex here as well, as the tracking query would skip it if it did not change.
            originalCornerVertices = append(originalCornerVertices, v);
        }
    }
    return originalCornerVertices;
}

function getSideAndBase(context is Context, topLevelId is Id, edge is Query, definition is map, edgeToFlangeData is map, cornerVertices is array, overridesForEdge is map) returns map
{
    var flangeData = edgeToFlangeData[edge];
    if (flangeData == undefined)
    {
        throw "Could not find flange data for edge";
    }
    var possibleSourceEdges = definition.edges;
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2215_SM_FLANGE_FIX))
    {
        // If a face is selected, we need to find the edge adjacent to the face from the definition.
        possibleSourceEdges = [definition.edges, qAdjacent(definition.edges, AdjacencyType.EDGE, EntityType.EDGE)]->qUnion()->qEntityFilter(EntityType.EDGE);
    }
    const sourceEdges = getSelectionsForSMDefinitionEntities(context, edge, possibleSourceEdges)->qSubtraction(edge);
    const edgeVertices = getOrderedEdgeVertices(context, edge, sourceEdges);
    edgeToFlangeData[edge].sourceVertices = edgeVertices.sourceVertices;
    var flangeSideDirs = [flangeData.direction, flangeData.direction];
    var flangeBasePoints = [edgeVertices.points[0].origin, edgeVertices.points[1].origin];
    for (var i = 0; i < size(edgeVertices.vertices); i += 1)
    {
        const vertexOverride = overridesForEdge[edgeVertices.vertices[i].transientId];
        const vertexData = getVertexData(context, topLevelId, edge, edgeVertices.vertices[i], edgeToFlangeData, definition, i, cornerVertices, vertexOverride);
        if (vertexData != undefined)
        {
            flangeSideDirs[i] = vertexData.flangeSideDir;
            flangeBasePoints[i] = vertexData.flangeBasePoint;
        }
    }
    return { "sideDirections" : flangeSideDirs, "basePoints" : flangeBasePoints };
}

// The keys of the map holding partial flange side position data are queries because the sheet is altered for flange
// alignment between production and consumption of the data. This converts the queries to transient ids for consumption.
function convertOverrideMapToTransientIds(context is Context, overridesAtEdges is map) returns map
{
    var convertedOverrides = {};
    for (var edgeQuery, vertexMap in overridesAtEdges)
    {
        var convertedVertexMap = {};
        for (var vertexQuery, limitPlanes in vertexMap)
        {
            const evaluatedVertex = evaluateQuery(context, vertexQuery);
            if (evaluatedVertex->size() != 1)
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL, vertexQuery);
            }
            convertedVertexMap[evaluatedVertex[0].transientId] = limitPlanes;
        }
        const evaluatedEdge = evaluateQuery(context, edgeQuery);
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2668_SM_CONE))
        {
            // if we have multi-model partial flange selections, edges of processed
            // models would be converted into flanges alread, so we may evaluate to
            // zero edges here. It's not an error
            if (evaluatedEdge->size() == 1)
            {
                convertedOverrides[evaluatedEdge[0].transientId] = convertedVertexMap;
            }
            else if (evaluatedEdge->size() > 1)
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL);
            }
        }
        else if (evaluatedEdge->size() != 1)
        {
           throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL);
        }
        else
        {
            convertedOverrides[evaluatedEdge[0].transientId] = convertedVertexMap;
        }
    }
    return convertedOverrides;
}

function collectEdgeToSideAndBase(context is Context, topLevelId is Id, useExternalDisambiguation is boolean, edges is Query,
    originalCornerVertices is array, edgeToFlangeData is map, definition is map, overridesAtEdges) returns map
{
    var edgeToSideAndBase = {};
    const convertedOverrides = convertOverrideMapToTransientIds(context, overridesAtEdges);
    for (var edge in evaluateQuery(context, edges))
    {
        try
        {
            var overridesForOneEdge = convertedOverrides[edge.transientId];
            overridesForOneEdge = overridesForOneEdge == undefined ? {} : overridesForOneEdge;
            edgeToSideAndBase[edge] = getSideAndBase(context, topLevelId, edge, definition, edgeToFlangeData, originalCornerVertices, overridesForOneEdge);
        }
        catch (error)
        {
            // Translate raw errors into regen errors
            if (error is string)
            {
                if (useExternalDisambiguation)
                    setErrorEntities(context, topLevelId, { "entities" : edge });
                throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL, ["edges"]);
            }
            throw error;
        }
    }
    return edgeToSideAndBase;
}

function getOffsetForClearance(context is Context, sidePlane is Plane, clearance is ValueWithUnits, definition is map, flangePlane is Plane)
{
    var offsetForClearance = clearance;
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V629_SM_MODEL_FRONT_N_BACK) ||
        !perpendicularVectors(flangePlane.normal, sidePlane.normal))
    {
        var normalDot = dot(flangePlane.normal, sidePlane.normal);
        var thickness = (normalDot > 0) ? definition.backThickness : definition.frontThickness;
        offsetForClearance += thickness * abs(normalDot);
    }
    return offsetForClearance;
}

/**
 * Adjust the flange width to account for the presence of a joint.
 * @returns {Vector} : 3D point vector to be used in the flange sketch.
 **/
function getFlangeBasePoint(context is Context, flangeEdge is Query, sideEdge is Query, definition is map,
    flangeData is map, vertexPoint is Vector, needsTrimChanges is boolean, sidePlane is Plane, clearance is ValueWithUnits) returns Vector
{
    var ignoreSideEdge = isAtVersionOrLater(context, FeatureScriptVersionNumber.V526_FLANGE_SIDE_PLANE_DIR) &&
    isQueryEmpty(context, sideEdge);
    var sideEdgeIsBend = false;
    var jointAttribute;
    if (!ignoreSideEdge)
    {
        jointAttribute = try silent(getJointAttribute(context, sideEdge));
        if (jointAttribute == undefined)
            return vertexPoint;
        sideEdgeIsBend = (jointAttribute.jointType.value == SMJointType.BEND);
    }
    var edgeLine = evLine(context, { "edge" : flangeEdge });
    var offsetFromClearance = getOffsetForClearance(context, sidePlane, clearance, definition, flangeData.plane);
    if (!sideEdgeIsBend)
    {
        //use the minimal clearance to shift by
        return computeBaseFromShiftedPlane(context, offsetFromClearance, sidePlane, edgeLine);
    }

    var edgeBendRadius = jointAttribute.radius.value;
    var edgeBendAngle = jointAttribute.angle.value;

    //find direction of side edge on the adjacent plane
    const sideEdgeMidPt = evEdgeTangentLines(context, { "edge" : sideEdge, "parameters" : [0.5], "face" : flangeData.adjacentFace });
    var adjacentPlane = evPlane(context, { "face" : flangeData.adjacentFace });
    var thickness = 0;
    var convexity = evEdgeConvexity(context, { "edge" : sideEdge });
    if (convexity == EdgeConvexityType.CONVEX)
        thickness = definition.backThickness;
    else if (convexity == EdgeConvexityType.CONCAVE)
        thickness = definition.frontThickness;

    var offset = (thickness + edgeBendRadius) * tan(.5 * edgeBendAngle);
    // "move" side edge towards the inside of adjacent plane by offset (based on side edge bend info)
    var directionToMoveEdgeBy = cross(adjacentPlane.normal, sideEdgeMidPt[0].direction);
    var planeFromSideEdge = plane(vertexPoint + directionToMoveEdgeBy * offset, directionToMoveEdgeBy);

    //find flange edge direction on adjacent plane
    const flangeEdgeMidPt = evEdgeTangentLines(context, { "edge" : flangeEdge, "parameters" : [0.5], "face" : flangeData.adjacentFace });
    offset = (thickness + definition.bendRadius) * tan(.5 * flangeData.bendAngle);
    // "move" flange edge towards the inside of adjacent plane by offset (based on flange bend info)
    directionToMoveEdgeBy = cross(adjacentPlane.normal, flangeEdgeMidPt[0].direction);
    var lineFromFlangeEdge = line(vertexPoint + directionToMoveEdgeBy * offset, flangeEdgeMidPt[0].direction);

    //intersect the two lines
    var intersectionData = intersection(planeFromSideEdge, lineFromFlangeEdge);
    if (!needsTrimChanges)
    {
        //project onto the flange edge
        var projection = project(edgeLine, intersectionData.intersection);
        //check that the projection lies within parameter bounds of edge
        var edgesFound = evaluateQuery(context, qContainsPoint(flangeEdge, projection));
        if (size(edgesFound) != 1)
            return vertexPoint;
        return projection;
    }
    else
    {
        // We want to project the point onto the bent edge on the flange plane, then use that to do plane shift
        // See review request #40859 for images

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V493_FLANGE_BASE_SHIFT_FIX) && edgeBendAngle < 90 * degree)
        {
            //just use the original planeFromSideEdge for base shift
            return computeBaseFromShiftedPlane(context, 0.0 * meter, planeFromSideEdge, edgeLine);
        }
        var lineFromBentEdge = line(vertexPoint + flangeData.direction * offset, flangeEdgeMidPt[0].direction);
        var updatedProjection = project(lineFromBentEdge, intersectionData.intersection);
        var offsetFromBend = norm(project(sidePlane, updatedProjection) - updatedProjection);
        //offset with max between bend clearance and default offset clearance
        var delta = abs(offsetFromClearance) > abs(offsetFromBend) ? offsetFromClearance : offsetFromBend;
        return computeBaseFromShiftedPlane(context, delta, sidePlane, edgeLine);
    }
}

function computeBaseFromShiftedPlane(context is Context, delta is ValueWithUnits, sidePlane is Plane, edgeLine is Line)
{
    sidePlane.origin = sidePlane.origin + delta * sidePlane.normal;

    //use shifted plane for base move
    var intersectionData = intersection(sidePlane, edgeLine);
    if (intersectionData.dim != 0)
        throw "Found a non-point intersection between plane and line";
    return intersectionData.intersection;
}

/**
 * The vertices are ordered according to traversal around the corresponding SM face.
 * @returns {{
 *      @field vertices {array} : An array of queries for the vertices.
 *      @field points {array} : The corresponding location of each vertex.
 *      @field sourceVertices {array} : An array of queries for each of the corresponding source selection vertices.
 * }}
 **/
function getOrderedEdgeVertices(context is Context, edge is Query, sourceEdge is Query) returns map
{
    var edgeVertices = evaluateQuery(context, qAdjacent(edge, AdjacencyType.VERTEX, EntityType.VERTEX));
    if (size(edgeVertices) != 2)
        throw "Edge to flange has wrong number of vertices";

    var p0 = evVertexPoint(context, { "vertex" : edgeVertices[0] });

    const adjacentFace = qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE);
    if (size(evaluateQuery(context, adjacentFace)) != 1)
    {
        throw "Edge to flange is not laminar";
    }
    const edgeEndPoints = evEdgeTangentLines(context, { "edge" : edge, "parameters" : [0, 1], "face" : adjacentFace });
    if (!tolerantEquals(edgeEndPoints[0].origin, p0))
    {
        edgeVertices = reverse(edgeVertices);
    }
    const sourceVertices = [sourceEdge->qEdgeVertex(true), sourceEdge->qEdgeVertex(false)];
    const sourceV0MappedToSMVertex = getSMDefinitionEntities(context, sourceVertices[0])->mapValue(function(smVertex) { return smVertex == [] ? edgeVertices[0] : smVertex[0]; });
    const reverseSourceVertices = sourceV0MappedToSMVertex != edgeVertices[0];
    return { "vertices" : edgeVertices, "points" : edgeEndPoints, "sourceVertices" : reverseSourceVertices ? reverse(sourceVertices) : sourceVertices };
}

/*
 * Creates vector from position to opposite end point of edge
 */
function getVectorForEdge(context is Context, edge is Query, position is Vector) returns Vector
{
    var edgeEndPoints = evEdgeTangentLines(context, { "edge" : edge, "parameters" : [0, 1] });
    var closerPointIdx = (squaredNorm(edgeEndPoints[0].origin - position) < squaredNorm(edgeEndPoints[1].origin - position)) ? 0 : 1;
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V725_FLANGE_ROLLED_SIDE))
    {
        // vectorForEdge should always be pointing into the edge
        const sign = (closerPointIdx == 0) ? 1 : -1;
        return (sign * edgeEndPoints[closerPointIdx].direction);
    }
    else
    {
        return stripUnits(edgeEndPoints[1 - closerPointIdx].origin - edgeEndPoints[closerPointIdx].origin) as Vector;
    }
}

/**
 * Resulting edges are either laminar or non-g1.
 **/
function filterSmoothEdges(context is Context, inputEdges is Query) returns array
{
    var evaluatedInputEdges = evaluateQuery(context, inputEdges);
    var resultingEdges = filter(evaluatedInputEdges, function(edge)
    {
        return (!edgeIsTwoSided(context, edge)) || (evEdgeConvexity(context, { "edge" : edge }) != EdgeConvexityType.SMOOTH);
    });
    return resultingEdges;
}

/**
 * In the case where a flange is in a cutout, its ends need to be offset inwards to provide clearance for the bend reliefs.
 * In the case of tear reliefs, it needs to be offset by the minimal clearance to avoid collisions in the flat.
 *
 * Returns the vector to offset the flange end with/
 **/
function getInternalFlangeOffset(context is Context, vertex is Query, edge is Query, flangeData, edgeIndex is number) returns Vector
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V2669_INTERNAL_FLANGE_OFFSET))
    {
        return vector(0, 0, 0) * meter;
    }
    const adjacentEdges = evaluateQuery(context, qSubtraction(qAdjacent(vertex, AdjacencyType.VERTEX, EntityType.EDGE), edge));
    if (adjacentEdges->size() != 1)
    {
        return vector(0, 0, 0) * meter;
    }
    const adjacentEdgeDirection = getVectorForEdge(context, adjacentEdges[0], flangeData.edgeEndPoints[edgeIndex].origin);
    const outDirection = flangeData.wallExtendDirection;
    if (dot(outDirection, adjacentEdgeDirection) < TOLERANCE.zeroAngle)
    {
        return vector(0, 0, 0) * meter;
    }

    const edgeEndDirection = edgeIndex == 0 ? flangeData.edgeEndPoints[edgeIndex].direction : -1 * flangeData.edgeEndPoints[edgeIndex].direction;
    const gap = getDefaultBendReliefWidth(context, qOwnerBody(edge));
    return edgeEndDirection * gap;
}

/**
 * Get the two adjacent bounding edges on the SM sheet body in the event of this flange starting from an edge that meets
 * another flange. If there is no adjacent flange meeting at this vertex, this will return `undefined`.
 * @returns {{
 *      @field edgeX {Query} : The adjacent flange's edge that is adjacent to this flange's associated face.
 *      @field edgeY {Query} : The adjacent flange's other bounding edge that meets at this vertex.
 *      @field sideFace {Query} : The adjacent flange SM face.
 *      @field position {Vector} : The position of the vertex that was selected.
 * }}
 **/
function getXYAtVertex(context is Context, vertex is Query, edge is Query, edgeToFlangeData is map)
{
    var vertexToUse = vertex;
    var vertexEdges = qSubtraction(qAdjacent(vertex, AdjacencyType.VERTEX, EntityType.EDGE), edge);
    var vertexEdgesArray = filterSmoothEdges(context, vertexEdges);
    if (size(vertexEdgesArray) == 1 && edgeToFlangeData[vertexEdgesArray[0]] != undefined)
    {
        // There is only one non-smooth edge besides `edge` extending from `vertex`.  This edge is also part of this
        // flange operation, so it must be laminar and linear.  Due to these constraints, the edge must lie on the
        // same face as `edge`, and therefore qualifies as `edgeX`
        return { "edgeX" : vertexEdgesArray[0], "position" : evVertexPoint(context, { "vertex" : vertexToUse }) };
    }

    vertexEdges = qUnion(vertexEdgesArray);
    var flangeAdjacentFace = edgeToFlangeData[edge].adjacentFace;
    //sideEdge(edgeX) will be the edge shared by vertexEdgesExcludingEdge and flangeAdjacentFace
    var edgeX = qIntersection([vertexEdges, qAdjacent(flangeAdjacentFace, AdjacencyType.EDGE, EntityType.EDGE)]);

    //sideFace is adjacent to flangeAdjacentFace, and edgeX
    var sideFace = qSubtraction(qAdjacent(edgeX, AdjacencyType.EDGE, EntityType.FACE), flangeAdjacentFace);

    //edgeY is the other edge on sideFace that is also adjacent to vertex. Often this will be a laminar. but not necessarily
    // e.g. if the edgeY is a bendEdge of a flange with angled miter.
    var edgeY = qSubtraction(qIntersection([qAdjacent(sideFace, AdjacencyType.EDGE, EntityType.EDGE), vertexEdges]), edgeX);

    var failIfNotLines = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V695_SM_SWEPT_SUPPORT);
    var lineX;
    if (!isQueryEmpty(context, edgeY))
    {
        //if edgeY is collinear with edgeX,look for next edge on sideFace
        var line1 = (failIfNotLines) ? evLine(context, { "edge" : edgeX }) : try silent(evLine(context, { "edge" : edgeX }));
        var line2 = (failIfNotLines) ? evLine(context, { "edge" : edgeY }) : try silent(evLine(context, { "edge" : edgeY }));

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V762_FLANGE_NEAR_ROLLED))
        {
            // We are looking for the next edge that is not continuous with edgeX. For rolled sheet metal, the side
            // edge may not be linear, but we should still skip to the next edge if the edgeX and edgeY are tangent.
            const vertexPoint = evVertexPoint(context, { "vertex" : vertexToUse });
            if (line1 == undefined)
            {
                line1 = line(vertexPoint, getVectorForEdge(context, edgeX, vertexPoint));
            }
            if (line2 == undefined)
            {
                line2 = line(vertexPoint, getVectorForEdge(context, edgeY, vertexPoint));
            }
        }

        lineX = line1;

        if (line1 != undefined && line2 != undefined && tolerantCollinear(line1, line2, !isAtVersionOrLater(context, FeatureScriptVersionNumber.V649_FLANGE_LOOSEN_EDGE_Y)))
        {
            edgeY = qIntersection([qAdjacent(sideFace, AdjacencyType.EDGE, EntityType.EDGE), qSubtraction(qAdjacent(edgeY, AdjacencyType.VERTEX, EntityType.EDGE), edgeX)]);
        }
    }
    else
    {
        var lineOrigX = try silent(evLine(context, { "edge" : edgeX }));
        if (lineOrigX == undefined)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V489_FLANGE_BUGS))
            {
                //flange is adjacent to a non-linear edge, move along.
                return { "edgeX" : edgeX, "position" : evVertexPoint(context, { "vertex" : vertex }) };
            }
            return undefined;
        }
        vertexToUse = qSubtraction(qAdjacent(edgeX, AdjacencyType.VERTEX, EntityType.VERTEX), vertex);
        vertexEdges = qSubtraction(qAdjacent(vertexToUse, AdjacencyType.VERTEX, EntityType.EDGE), edgeX);
        vertexEdgesArray = filterSmoothEdges(context, vertexEdges);
        if (size(vertexEdgesArray) == 1)
        {
            return { "edgeX" : vertexEdgesArray[0], "position" : evVertexPoint(context, { "vertex" : vertex }) };
        }
        vertexEdges = qUnion(vertexEdgesArray);
        //find Edge among vertexEdges also adjacent to adjacentFace
        edgeX = qIntersection([vertexEdges, qAdjacent(flangeAdjacentFace, AdjacencyType.EDGE, EntityType.EDGE)]);
        //check for sanity that the newly found edgeX is collinear with the one we found initially
        var lineNewX = isAtVersionOrLater(context, FeatureScriptVersionNumber.V714_SM_BEND_DETERMINISM) ?
        try silent(evLine(context, { "edge" : edgeX })) : evLine(context, { "edge" : edgeX });
        if (lineNewX == undefined || !tolerantCollinear(lineOrigX, lineNewX, !isAtVersionOrLater(context, FeatureScriptVersionNumber.V493_FLANGE_BASE_SHIFT_FIX)))
            return undefined;
        sideFace = qSubtraction(qAdjacent(edgeX, AdjacencyType.EDGE, EntityType.FACE), flangeAdjacentFace);
        edgeY = qSubtraction(qIntersection([qAdjacent(sideFace, AdjacencyType.EDGE, EntityType.EDGE), vertexEdges]), edgeX);
        lineX = lineNewX;
    }

    var edgeXEvaluated = evaluateQuery(context, edgeX);
    var edgeYEvaluated = evaluateQuery(context, edgeY);
    if (size(edgeYEvaluated) == 0 || size(edgeXEvaluated) == 0)
        return undefined;
    if (lineX != undefined &&
        isAtVersionOrLater(context, FeatureScriptVersionNumber.V569_FLANGE_NEXT_TO_RIP))
    {
        var edgeLine = evLine(context, {
                "edge" : edge
            });
        if (parallelVectors(lineX.direction, edgeLine.direction))
        {
            return undefined;
        }
    }
    return { "edgeX" : edgeXEvaluated[0],
            "edgeY" : edgeYEvaluated[0],
            "sideFace" : evaluateQuery(context, sideFace)[0],
            "position" : evVertexPoint(context, { "vertex" : vertexToUse }) };
}

function createPlaneForManualMiter(flangeData is map, sidePlane is Plane, position is Vector, index is number, miterAngle is ValueWithUnits) returns Plane
{
    // make sure the plane is rotated correctly for non-90-degree flanges
    var edgeDirection = flangeData.edgeEndPoints[index].direction * (index == 0 ? 1 : -1);
    var axisDirection = cross(edgeDirection, flangeData.direction);
    return plane(position, rotationMatrix3d(axisDirection, miterAngle) * sidePlane.normal);
}

function createPlaneForAutoMiter(context is Context, topLevelId is Id, angleControlType is SMFlangeAngleControlType, edge is Query, flangeData is map,
    edgeOther, flangeDataOther, adjPlane is Plane, sidePlane is Plane, sideEdge, sideEdgeIsBend is boolean, position is Vector, index is number)
{
    if (flangeData.isConeAdjacent || (flangeDataOther != undefined && flangeDataOther.isConeAdjacent))
    {
        return undefined;
    }

    // if sideEdge is a bend, don't miter if flaps are on the "outside"
    if (sideEdgeIsBend)
    {
        var convexity = evEdgeConvexity(context, { "edge" : sideEdge });
        if (convexity == EdgeConvexityType.CONCAVE && dot(flangeData.direction, adjPlane.normal) < TOLERANCE.zeroAngle)
        {
            return undefined;
        }
        else if (convexity == EdgeConvexityType.CONVEX && dot(flangeData.direction, adjPlane.normal) > TOLERANCE.zeroAngle)
        {
            return undefined;
        }
    }

    // find a normal for the cutting plane of the auto-miter
    var simpleNormal = sidePlane.normal - adjPlane.normal;
    var midPlaneNormal;
    if (angleControlType == SMFlangeAngleControlType.BEND_ANGLE || flangeDataOther == undefined)
    {
        midPlaneNormal = simpleNormal;
    }
    else
    {
        if (!parallelVectors(flangeData.plane.normal, flangeDataOther.plane.normal))
        {
            // Use the bisector of the flange planes if possible
            midPlaneNormal = flangeData.plane.normal - flangeDataOther.plane.normal;

            // Ensure that the cutting plane actually falls between the flange edges. Checking that the plane normal
            // falls between (i.e. creates opposing cross products with) the edge directions is a proxy for this check.
            var crossA = cross(midPlaneNormal, flangeData.edgeEndPoints[0].direction);
            var crossB = cross(midPlaneNormal, flangeDataOther.edgeEndPoints[0].direction);
            if (dot(crossA, crossB) > 0)
            {
                setErrorEntities(context, topLevelId, { "entities" : qUnion([edge, edgeOther]) });
                throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_AUTO_MITER, ["edges"]);
            }
        }
        else
        {
            if (flangeData.alignedDistance != undefined && flangeDataOther.alignedDistance != undefined)
            {
                // Intersect a plane that caps the far end of this flange and the line across the far end of the other flange.
                // Cut the auto-miter along the line from the shared base point and this intersection. This ensures that
                // The flanges meet at a shared point at their far end.
                var flangeEndPlane = plane(flangeData.edgeEndPoints[0].origin + (flangeData.direction * flangeData.alignedDistance),
                flangeData.direction);
                var flangeEndOther = line(flangeDataOther.edgeEndPoints[0].origin + (flangeDataOther.direction * flangeDataOther.alignedDistance),
                flangeDataOther.edgeEndPoints[0].direction);

                var linePlaneResult = intersection(flangeEndPlane, flangeEndOther);
                if (linePlaneResult.dim != 0)
                {
                    setErrorEntities(context, topLevelId, { "entities" : qUnion([edge, edgeOther]) });
                    throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_AUTO_MITER, ["edges"]);
                }

                midPlaneNormal = cross(flangeData.wallExtendDirection, stripUnits(linePlaneResult.intersection - position) as Vector);
            }
            else
            {
                // Flange end bound condition is an 'up-to'
                midPlaneNormal = simpleNormal;
            }
        }
    }

    if (squaredNorm(midPlaneNormal) < (TOLERANCE.zeroLength * TOLERANCE.zeroLength))
    {
        return undefined;
    }
    return plane(position, midPlaneNormal);
}

function checkIfNeedsBaseUpdate(definition is map, sideEdgeIsBend is boolean) returns boolean
{
    if (sideEdgeIsBend)
        return true;
    return false;
}

function getAngleForAngledMiter(definition is map) returns ValueWithUnits
{
    var miterAngle = definition.miterAngle;
    //rotate by minimum amount so that normals of created sidePlanes are always points towards the edge
    if (abs(90 * degree + miterAngle) < abs(miterAngle - 90 * degree))
        return 90 * degree + miterAngle;
    else
        return miterAngle - 90 * degree;
}

function representInVectors(context is Context, vector1 is Vector, vector2 is Vector, inVector is Vector)
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V992_FLANGE_END_FIX) &&
        parallelVectors(vector1, vector2))
    {
        return undefined;
    }

    var matrixM is Matrix = matrix([[squaredNorm(vector1), dot(vector2, vector1)],
            [dot(vector1, vector2), squaredNorm(vector2)]]);
    var rhs = vector(dot(inVector, vector1), dot(inVector, vector2));

    var components = inverse(matrixM) * rhs;
    return components;
}

/**
 * The problem halfspace is when a flange is determined to interfere with another prexisting SM edge. In that scenario, the
 * edge to be created is pulled back from the interfering edge.
 **/
function isInProblemHalfSpace(context is Context, flangeDir is Vector, position is Vector, edgeY, sideEdge is Query) returns boolean
{
    if (edgeY == undefined || sideEdge == undefined)
        return false;
    var components = representInVectors(context, getVectorForEdge(context, edgeY, position), getVectorForEdge(context, sideEdge, position), flangeDir);
    return (components != undefined && components[0] > TOLERANCE.zeroLength);
}

// make sure to move base when the two adjacent flanges have adjacent faces that are drafted so that the two flaps
// arent adjacent.
function determineBaseUpdateForAutoMiter(flangePlane1 is Plane, flangePlane2 is Plane, miterPlane is Plane) returns boolean
{
    var needsBaseUpdate = true;
    if (parallelVectors(flangePlane1.normal, flangePlane2.normal))
        needsBaseUpdate = false;
    else
    {
        var planeIntersection = intersection(flangePlane1, flangePlane2);
        if (planeIntersection != undefined)
        {
            var miterPlaneIntersection = intersection(miterPlane, planeIntersection);

            if (miterPlaneIntersection != undefined && miterPlaneIntersection.dim == 1)
                needsBaseUpdate = false;
        }
    }
    return needsBaseUpdate;
}

/**
 * Calculates the precise location for the base vertices of the flange, taking into account interference with pre-existing walls and joints,
 * miter-related updates, and partial flange implications.
 * @param i {number} : Either 0 or 1 corresponding to the index of the processed edge vertices.
 * @returns {{
 *      @field flangeSideDir {Direction} : The computed direction that the side of the flange points to.
 *      @field flangeBasePoint {Vector} : The updated location for the base point.
 * }}
 **/
function getVertexData(context is Context, topLevelId is Id, edge is Query, vertex is Query, edgeToFlangeData is map, definition is map, i is number, cornerVertices, vertexOverride) returns map
{
    var position = evVertexPoint(context, { "vertex" : vertex });
    var result = {
        "flangeBasePoint" : position,
        "flangeSideDir" : undefined
    };

    var flangeData = edgeToFlangeData[edge];
    const handleMiter = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1925_HANDLE_MITER_IF_HOLD_ADJACENT_EDGES_FIX);
    if (vertexOverride != undefined)
    {
        if (vertexOverride.position != undefined)
        {
            result.flangeBasePoint = vertexOverride.position;
            // Edge may have moved due to flange alignment.
            const projectToEdge = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1930_PARTIAL_FLANGE_ALIGNMENT_FIX);
            if (projectToEdge)
            {
                const distanceResult = try silent(evDistance(context, {
                                "side0" : edge,
                                "side1" : vertexOverride.position
                            }));
                if (distanceResult == undefined)
                {
                    throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL);
                }
                result.flangeBasePoint = distanceResult.sides[0].point;
            }
        }
        const sidePlane = vertexOverride.limitPlane;
        if (sidePlane != undefined)
        {
            const forceResult = true; // Use the automiter flag to use the plane direction even if it adds material.
            result.flangeSideDir = getFlangeSideDir(flangeData, sidePlane, i, forceResult, definition);
            if (handleMiter)
            {
                return result;
            }
        }
        if (!handleMiter)
        {
            return result;
        }
    }

    var needsSideDirUpdate = false;
    // In the event of a partial flange, getXYAtVertex should not be trusted since the edge of the flange no
    // longer sits at the vertex that this is looking at.
    const ignoreXYAtVertex = vertexOverride != undefined && isAtVersionOrLater(context, FeatureScriptVersionNumber.V2099_FLANGE_MITER_FIX);
    var vertexAndEdges = ignoreXYAtVertex ? undefined : getXYAtVertex(context, vertex, edge, edgeToFlangeData);
    if (vertexAndEdges == undefined || (isIn(vertex, cornerVertices) && vertexAndEdges.edgeY == undefined))
    {
        result.flangeBasePoint += getInternalFlangeOffset(context, vertex, edge, flangeData, i);
        if (!definition.autoMiter)
        {
            var computeMiter = isAtVersionOrLater(context, FeatureScriptVersionNumber.V569_FLANGE_NEXT_TO_RIP);
            if (!computeMiter)
            {
                var vertexEdges = evaluateQuery(context, qSubtraction(qAdjacent(vertex, AdjacencyType.VERTEX, EntityType.EDGE), edge));
                computeMiter = (size(vertexEdges) == 1);
            }
            if (computeMiter)
            {
                var sidePlane = plane(position, flangeData.edgeEndPoints[i].direction);
                sidePlane = createPlaneForManualMiter(flangeData, sidePlane, position, i, definition.adjustedMiterAngle);
                result.flangeSideDir = getFlangeSideDir(flangeData, sidePlane, i, false, definition);
            }
        }
        return result;
    }

    // This was a buggy fix that exited processing early, incorrectly skipping over subsequent miter processing.
    if (vertexOverride != undefined && isAtVersionOrLater(context, FeatureScriptVersionNumber.V1939_PARTIAL_FLANGE_HOLD_ADJACENT_EDGES_FIX) && !isAtVersionOrLater(context, FeatureScriptVersionNumber.V2099_FLANGE_MITER_FIX))
    {
        return result;
    }

    var sideEdge = vertexAndEdges.edgeX;
    var edgeY = vertexAndEdges.edgeY;
    var sideFace = vertexAndEdges.sideFace;

    var jointAttribute = try silent(getJointAttribute(context, sideEdge));
    var sideEdgeIsBend = (jointAttribute != undefined && jointAttribute.jointType.value == SMJointType.BEND);

    var needsBaseUpdate = checkIfNeedsBaseUpdate(definition, sideEdgeIsBend);
    var sidePlane = undefined;
    var adjPlane = undefined;
    // Edge direction at vertex, pointing from vertex
    var edgeEndDirection = i == 0 ? flangeData.edgeEndPoints[i].direction : -1 * flangeData.edgeEndPoints[i].direction;
    var alignFlippedFlange = isAtVersionOrLater(context, FeatureScriptVersionNumber.V629_SM_MODEL_FRONT_N_BACK);
    var clearanceFromSide = definition.minimalClearance;
    if (sideFace == undefined && edgeY == undefined)
    {
        if (edgeToFlangeData[sideEdge] != undefined)
            sidePlane = edgeToFlangeData[sideEdge].plane;
        else
            sidePlane = plane(position, edgeEndDirection);
        adjPlane = edgeToFlangeData[edge].plane;
        if (!alignFlippedFlange)
            clearanceFromSide += 0.5 * (definition.frontThickness + definition.backThickness);
    }
    else
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V762_FLANGE_NEAR_ROLLED))
        {
            sidePlane = getFacePlane(context, sideFace, sideEdge, vertexAndEdges.position);
        }
        else
        {
            sidePlane = getFacePlane(context, sideFace, sideEdge);
        }
        adjPlane = getFacePlane(context, flangeData.adjacentFace, edge);

        if (dot(sidePlane.normal, edgeEndDirection) > 0)
        {
            clearanceFromSide += definition.frontThickness;
        }
        else
        {
            clearanceFromSide += definition.backThickness;
        }
    }
    var needsTrimChanges = false;
    var autoMitered = false; // vertex is a corner where an actual miter happened when auto-miter is on
    var tighterClearance = isAtVersionOrLater(context, FeatureScriptVersionNumber.V521_SM_CLEARANCE);

    // No autoMiter, but with adjacent flanges at the start.
    if (!definition.autoMiter)
    {
        var sidePlane = plane(position, edgeEndDirection);
        sidePlane = createPlaneForManualMiter(flangeData, sidePlane, vertexAndEdges.position, i, definition.adjustedMiterAngle);
        result.flangeSideDir = getFlangeSideDir(flangeData, sidePlane, i, false, definition);
        var noAdjacentFlange = isAtVersionOrLater(context, FeatureScriptVersionNumber.V493_FLANGE_BASE_SHIFT_FIX) ? (edgeToFlangeData[edgeY] == undefined) : true;
        var reducingMiter = tighterClearance && dot(sidePlane.normal, flangeData.direction) < 0 && !perpendicularVectors(sidePlane.normal, flangeData.direction);
        if (noAdjacentFlange &&
            isInProblemHalfSpace(context, flangeData.direction, vertexAndEdges.position, edgeY, sideEdge) &&
            !reducingMiter)
        {
            result.flangeBasePoint = getFlangeBasePoint(context, edge, sideEdge, definition, flangeData, vertexAndEdges.position, needsTrimChanges, sidePlane, clearanceFromSide);
        }
        // Case when the flange should not extend all the way to the outside of the SM face
        // and instead be restricted by the edge selected by the user
        else if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2099_FLANGE_MITER_FIX) && !reducingMiter)
        {
            const sourcePoint = evVertexPoint(context, {
                        "vertex" : flangeData.sourceVertices[i]
                    });
            // Ensure that the adjusted point is on the flange plane and the base plane.
            result.flangeBasePoint = project(adjPlane, project(flangeData.wallPlane, sourcePoint));
        }
        return result;
    }
    if (edgeY == undefined || edgeToFlangeData[edgeY] != undefined) //adjacent laminar edge is also being flanged, it should be handled via miter
    {
        var originalSidePlane = sidePlane;
        sidePlane = createPlaneForAutoMiter(context, topLevelId, definition.angleControlType, edge, flangeData, edgeY, edgeToFlangeData[edgeY],
            adjPlane, sidePlane, sideEdge, sideEdgeIsBend, vertexAndEdges.position, i);
        if (sidePlane != undefined)
        {
            needsSideDirUpdate = true;
            needsBaseUpdate = (edgeY == undefined) ? false : determineBaseUpdateForAutoMiter(flangeData.plane, edgeToFlangeData[edgeY].plane, sidePlane);
            if (tighterClearance)
            {
                // clearance from another mitered flange does not need to include half-thickness
                clearanceFromSide = definition.minimalClearance;
            }
        }
        else
        {
            sidePlane = originalSidePlane;
        }
        autoMitered = true;
    }
    else
    {
        if (edgeY != undefined)
        {
            if (alignFlippedFlange)
            {
                needsBaseUpdate = true;
            }
            const components = representInVectors(context, getVectorForEdge(context, edgeY, position),
                getVectorForEdge(context, sideEdge, position), flangeData.direction);
            if (components != undefined)
            {
                if (components[0] > TOLERANCE.zeroLength)
                {
                    needsBaseUpdate = true;
                    needsTrimChanges = true;
                    if (components[1] > -TOLERANCE.zeroLength)
                    {
                        needsSideDirUpdate = true;
                    }
                }
            }
        }
    }

    if (needsBaseUpdate)
    {
        var useAsSideEdge = sideEdge;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V526_FLANGE_SIDE_PLANE_DIR))
        {
            // We never want to grow the edge, flip the plane normal, if it is pointing in wrong direction
            var dotPr = dot(sidePlane.normal, edgeEndDirection);
            if (dotPr < 0)
            {
                sidePlane.normal *= -1;
            }
            // If the bend is on the other side of sideFace from the flangeEdge ignore it in getFlangeBasePoint
            if (sideEdge != undefined && (evEdgeConvexity(context, { "edge" : sideEdge }) == EdgeConvexityType.CONVEX) != (dotPr < 0))
            {
                useAsSideEdge = qNothing();
            }
        }
        else if ((autoMitered || needsTrimChanges) && sideEdge != undefined && evEdgeConvexity(context, { "edge" : sideEdge }) == EdgeConvexityType.CONVEX)
            sidePlane.normal *= -1;

        // If we're not trimming and the projection for position falls outside of existing edges we return vertexPositionToUse.
        // So we need to make sure it points to the original vertex location, and not the adjusted location. (BEL-57722)
        var vertexPositionToUse = needsTrimChanges ? vertexAndEdges.position : position;
        result.flangeBasePoint = getFlangeBasePoint(context, edge, useAsSideEdge, definition, flangeData, vertexPositionToUse, needsTrimChanges,
            sidePlane, clearanceFromSide);
    }
    if (needsSideDirUpdate) // need a trim on the flange sides by a plane
    {
        result.flangeSideDir = getFlangeSideDir(flangeData, sidePlane, i, autoMitered, definition);
    }
    return result;
}

function getFlangeSideDir(flangeData is map, sidePlane is Plane, i is number, autoMitered is boolean, definition is map) returns Vector
{
    var intersectionDirection = normalize(cross(flangeData.plane.normal, sidePlane.normal));
    var isOppositeFlangedir = dot(flangeData.direction, intersectionDirection) < 0;
    var flangeSideDirection = isOppositeFlangedir ? -1 * intersectionDirection : intersectionDirection;
    var edgeDirection = (i == 0) ? flangeData.edgeEndPoints[i].direction : -1 * flangeData.edgeEndPoints[i].direction;

    //if angle between edge and computed side dir is > 90 then flange will add material
    var edgeToComputedSideDirAngle = angleBetween(edgeDirection, flangeSideDirection);

    var miterAddsMaterial = !definition.autoMiter && (definition.miterAngle - 90 * degree) > TOLERANCE.zeroAngle * degree;

    if (autoMitered || miterAddsMaterial || edgeToComputedSideDirAngle < (90 + TOLERANCE.zeroAngle) * degree)
        return flangeSideDirection;
    else //make sure we don't add material when we shouldn't
        return flangeData.direction;
}

function addBendAttribute(context is Context, edge is Query, flangeData is map, topLevelId is Id, index is number, definition is map)
{
    var attributeId = toAttributeId(topLevelId + index);
    var bendAttribute = makeSMJointAttribute(attributeId);
    bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited" : !flangeData.isConeAdjacent };
    bendAttribute.bendType = { "value" : SMBendType.STANDARD, "canBeEdited" : false };
    bendAttribute.radius = {
            "value" : definition.bendRadius,
            "canBeEdited" : !flangeData.isConeAdjacent
        };
    bendAttribute.angle = {
            "value" : flangeData.bendAngle,
            "canBeEdited" : !flangeData.isConeAdjacent
        };
    setAttribute(context, { "entities" : edge, "attribute" : bendAttribute });
}

function createSketchPlane(context is Context, topLevelId is Id, edge is Query, edgeLine is Line, sidePlaneNormal is Vector, definition is map) returns Plane
{
    var sketchPlaneNormal;
    var flipFactor = definition.oppositeDirection ? -1 : 1;
    if (definition.angleControlType == SMFlangeAngleControlType.BEND_ANGLE)
    {
        // Normal is calculated directly from bend angle off of the wall being flanged
        var angle = flipFactor * definition.bendAngle;
        sketchPlaneNormal = rotationMatrix3d(edgeLine.direction, angle) * sidePlaneNormal;
    }
    else if (definition.angleControlType == SMFlangeAngleControlType.ALIGN_GEOMETRY)
    {
        // See processParallelEntity(...)
        if (definition.parallelLineDirection != undefined)
        {
            // ALIGN_GEOMETRY with a line supplied.  Flange should be parallel to the supplied line (sketchPlaneNormal
            // should be orthogonal to the line direction).
            var parallelDirection = definition.parallelLineDirection;
            if (parallelVectors(parallelDirection, edgeLine.direction))
            {
                setErrorEntities(context, topLevelId, { "entities" : qUnion([edge, definition.parallelEntity]) });
                throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_PARALLEL_EDGE, ["edges", "parallelEntity"]);
            }
            var sameDirection = dot(parallelDirection, sidePlaneNormal) > 0;
            sketchPlaneNormal = sameDirection ? cross(edgeLine.direction, parallelDirection) : cross(parallelDirection, edgeLine.direction);
        }
        else if (definition.parallelPlaneNormal != undefined)
        {
            // ALIGN_GEOMETRY with a plane supplied.  Flange should be parallel to the supplied plane (sketchPlaneNormal
            // should be parallel with the normal of the plane).
            var planeNormal = definition.parallelPlaneNormal;
            if (1 - squaredNorm(cross(planeNormal, edgeLine.direction)) > TOLERANCE.zeroLength)
            {
                setErrorEntities(context, topLevelId, { "entities" : qUnion([edge, definition.parallelEntity]) });
                throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_PARALLEL_PLANE, ["edges", "parallelEntity"]);
            }
            var sameDirection = dot(cross(planeNormal, edgeLine.direction), sidePlaneNormal) > 0;
            sketchPlaneNormal = sameDirection ? planeNormal : -1 * planeNormal;
        }
        sketchPlaneNormal = sketchPlaneNormal * flipFactor;
    }
    else if (definition.angleControlType == SMFlangeAngleControlType.ANGLE_FROM_DIRECTION)
    {
        // Flange should be some angle off of parallel with the supplied direction.
        var direction = definition.parallelDirection;
        if (parallelVectors(direction, edgeLine.direction))
        {
            setErrorEntities(context, topLevelId, { "entities" : qUnion([edge, definition.directionEntity]) });
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_PARALLEL_DIRECTION, ["edges", "directionEntity"]);
        }
        var oppositeDirectionFlip = definition.angleFromDirectionOppositeAngle ? -1 : 1;
        var angle = flipFactor * oppositeDirectionFlip * definition.angleFromDirection;
        var rotatedDirection = rotationMatrix3d(edgeLine.direction, angle) * direction;
        var sameDirection = dot(rotatedDirection, sidePlaneNormal) > 0;
        sketchPlaneNormal = sameDirection ? cross(edgeLine.direction, rotatedDirection) : cross(rotatedDirection, edgeLine.direction);
        sketchPlaneNormal *= flipFactor;
    }

    // create plane passing through edge endpoint, with normal
    return plane(edgeLine.origin, sketchPlaneNormal);
}

function getOffsetsForSideEdgesUpToPlane(context is Context, flangeSideDirections is array, flangeDirection is Vector, basePoints is array, planeResult is Plane)
{
    var offsets = makeArray(2);
    for (var i = 0; i < 2; i += 1)
    {
        var offsetDir = flangeSideDirections[i];
        if (offsetDir == undefined)
        {
            offsetDir = flangeDirection;
        }
        var line = line(basePoints[i], offsetDir);
        const intersection = intersection(planeResult, line);
        if (intersection.dim != 0)
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_UP_TO, ["limitEntity"]);

        offsets[i] = intersection.intersection - basePoints[i];
    }
    return offsets;
}

function getOffsetsForSideEdgesForBlind(flangeSideDirections is array, flangeDirection is Vector, distance is ValueWithUnits)
{
    var defaultOffset = flangeDirection * distance;
    var offsets = [defaultOffset, defaultOffset];
    if (size(flangeSideDirections) != 2)
        return offsets;

    for (var i = 0; i < 2; i += 1)
    {
        var offsetDir = flangeSideDirections[i];
        if (offsetDir != undefined)
        {
            offsets[i] = offsetDir * (distance / dot(flangeDirection, offsetDir));
        }
    }
    return offsets;
}

// Get useful information about a flange on a specific edge.
// If this is changed, make sure to also check if updateFlangeDataAfterAlignmentChange needs changes
function getFlangeData(context is Context, topLevelId is Id, edge is Query, definition is map) returns map
{
    var adjacentFaces = evaluateQuery(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE));
    if (size(adjacentFaces) != 1)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_INTERNAL, ["edges"]);
    }
    const adjacentFace = adjacentFaces[0];
    const edgeEndPoints = evEdgeTangentLines(context, { "edge" : edge, "parameters" : [0, 1], "face" : adjacentFace });
    const sidePlane = getFacePlane(context, adjacentFace, edge);
    var sketchPlane = createSketchPlane(context, topLevelId, edge, edgeEndPoints[0], sidePlane.normal, definition);
    var direction = cross(edgeEndPoints[0].direction, sketchPlane.normal);
    var bendAngle = angleBetween(sketchPlane.normal, sidePlane.normal);
    if (tolerantEquals(bendAngle, 0 * degree) || tolerantEquals(bendAngle, 180 * degree))
    {
        // In ALIGN_GEOMETRY and ANGLE_FROM_DIRECTION cases, the user can accidentally make a 0 degree flange
        throwNoBendError(context, topLevelId, edge, definition);
    }
    var alignedDistance = undefined;
    if (definition.limitType == SMFlangeBoundingType.BLIND)
    {
        // The extension distance of a flange must be adjusted based on its flange angle
        var thickness = (definition.oppositeDirection) ? definition.backThickness : definition.frontThickness;
        alignedDistance = definition.distance - thickness * tan(.5 * bendAngle);
    }

    return {
            "bendAngle" : bendAngle,
            "alignedDistance" : alignedDistance,
            "direction" : direction,
            "plane" : sketchPlane,
            "wallExtendDirection" : cross(edgeEndPoints[0].direction, sidePlane.normal),
            "wallPlane" : sidePlane,
            "edgeEndPoints" : edgeEndPoints,
            "adjacentFace" : qUnion([adjacentFace, startTracking(context, adjacentFace)]),
            "isConeAdjacent" : !isQueryEmpty(context, adjacentFace->qGeometry(GeometryType.CONE))
        };
}

function throwNoBendError(context is Context, topLevelId is Id, edge is Query, definition is map)
{
    var faultyParameters = ["edges"];
    var faultyQueries = edge;
    if (definition.angleControlType == SMFlangeAngleControlType.ALIGN_GEOMETRY)
    {
        faultyParameters = append(faultyParameters, "parallelEntity");
        faultyQueries = qUnion([faultyQueries, definition.parallelEntity]);
    }
    else if (definition.angleControlType == SMFlangeAngleControlType.ANGLE_FROM_DIRECTION)
    {
        faultyParameters = append(faultyParameters, "directionEntity");
        faultyQueries = qUnion([faultyQueries, definition.directionEntity]);
    }
    setErrorEntities(context, topLevelId, { "entities" : faultyQueries });
    throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_NO_BEND, faultyParameters);
}

function getExtendedFlangePlane(flangeData is map, extensionDistance is ValueWithUnits) returns Plane
{
    var plane = flangeData.plane;
    plane.origin += (flangeData.wallExtendDirection * extensionDistance);
    return plane;
}

// Update flange data after the wall attaching to the flange is moved extensionDistance along flangeData.wallExtendDirection
function updateFlangeDataAfterAlignmentChange(context is Context, topLevelId is Id, edge is Query, flangeData is map,
    extensionDistance is ValueWithUnits) returns map
{
    //if we're adj to a cone, there was no extension, only update/test if there was one
    if (flangeData.isConeAdjacent)
        return flangeData;

    flangeData.plane = getExtendedFlangePlane(flangeData, extensionDistance);
    flangeData.edgeEndPoints = evEdgeTangentLines(context, { "edge" : edge, "parameters" : [0, 1], "face" : flangeData.adjacentFace });

    // Ensure that plane was extended correctly
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V762_FLANGE_NEAR_ROLLED))
    {
        for (var endPoint in flangeData.edgeEndPoints)
        {
            if (!isPointOnPlane(endPoint.origin, flangeData.plane))
            {
                failDueToAlignmentIssue(context, topLevelId, edge);
            }
        }
    }

    return flangeData;
}

function updateEdgeToFlangeDataAfterAlignmentChange(context is Context, topLevelId is Id, originalFlangeEdges is array,
    edgeToFlangeData is map, oldEdgeToNewEdge is map, edgeToExtensionDistance is map) returns map
{
    var newEdgeToFlangeData = edgeToFlangeData;

    for (var edge in originalFlangeEdges)
    {
        // Make sure the edge was not split or otherwise topologically changed
        var newEdge = evaluateQuery(context, oldEdgeToNewEdge[edge]);
        if (size(newEdge) != 1)
        {
            failDueToAlignmentIssue(context, topLevelId, qUnion(newEdge));
        }
        newEdge = newEdge[0];
        if (newEdge != edge)
        {
            // Edge was updated
            newEdgeToFlangeData[newEdge] = updateFlangeDataAfterAlignmentChange(context, topLevelId, newEdge, edgeToFlangeData[edge], edgeToExtensionDistance[edge]);
        }
        else
        {
            // Edge was skipped, flangeData does not need to change
            newEdgeToFlangeData[newEdge] = edgeToFlangeData[edge];
        }
    }

    return newEdgeToFlangeData;
}

function getPlaneForLimitEntity(context is Context, definition is map, flangeData is map, thickness is ValueWithUnits) returns Plane
{
    var flangeDirection = flangeData.direction;
    var flangePlane = flangeData.plane;

    const entities = verifyNonemptyQuery(context, definition, "limitEntity", ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_UP_TO_ENTITY);
    const entity = entities[0];

    //see if it's a plane:
    var planeResult = try silent(evPlane(context, { "face" : entity }));
    if (planeResult == undefined)
    { //see if it's an vertex
        var limitVertex = try silent(evVertexPoint(context, { "vertex" : entity }));
        if (limitVertex == undefined)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_UP_TO_ENTITY, ["limitEntity"]);
        }
        else
        {
            //create plane from point & flangeDirection
            planeResult = plane(limitVertex, -flangeDirection);
        }
    }

    // if direction to limit entity is opposite of flange direction, fail
    var pointOnEdge = flangeData.edgeEndPoints[0].origin;
    var pointOnPlane = isAtVersionOrLater(context, FeatureScriptVersionNumber.V489_FLANGE_BUGS) ? project(planeResult, pointOnEdge) : planeResult.origin;
    var upToDirection = pointOnPlane - pointOnEdge;
    if (dot(upToDirection, flangeDirection) < TOLERANCE.zeroLength * meter)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_LIMIT_OPP_FLANGE, ["oppositeDirection"]);
    }

    //we don't need to add half thickness here because the limit entity cannot be the underlying surface
    var minDelta = 0.0 * meter;
    if (definition.limitType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
    {
        minDelta += definition.offset;
    }

    var withPlaneNormal = isAtVersionOrLater(context, FeatureScriptVersionNumber.V629_SM_MODEL_FRONT_N_BACK) ?
    dot(flangeDirection, planeResult.normal) < 0 : true;
    planeResult = movePlaneForFlangeClearance(context, flangePlane, planeResult, withPlaneNormal, thickness, minDelta);
    return planeResult;
}

/*
 * During trimming or extending a flange up to an entity, make sure that the minimum distance between the
 * thickened flange and the target plane equals minDelta. This function computes the distance the otherPlane needs
 * to be moved depending on angle between flangePlane and otherPlane.
 */
function movePlaneForFlangeClearance(context is Context, flangePlane is Plane, otherPlane is Plane, withPlaneNormal is boolean, thickness, minDelta) returns Plane
{
    //angle between flange plane and side plane
    var dotProd = dot(flangePlane.normal, otherPlane.normal);
    dotProd = (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2304_SM_FLANGE_FIX) && !withPlaneNormal) ? dotProd : abs(dotProd);
    var angleClearance = thickness * dotProd;
    var delta = minDelta + angleClearance;
    delta = (withPlaneNormal ? 1 : -1) * delta;
    otherPlane.origin = otherPlane.origin + delta * otherPlane.normal;

    return otherPlane;
}

function createAndSolveSketch(context is Context, topLevelId is Id, id is Id, edge is Query, flangeData is map,
    sideAndBase is map, flangeDistance, definition is map)
{
    var flangeDirection = flangeData.direction;
    var basePoints = sideAndBase.basePoints;

    var offsets = makeArray(2);
    if (definition.limitType == SMFlangeBoundingType.BLIND)
    {
        offsets = getOffsetsForSideEdgesForBlind(sideAndBase.sideDirections, flangeDirection, flangeDistance);
    }
    else
    {
        var obtuseAngle = flangeData.bendAngle > (0.5 * PI + TOLERANCE.zeroAngle) * radian;
        var thickness = (obtuseAngle == definition.oppositeDirection) ? definition.backThickness : definition.frontThickness;
        var planeResult = getPlaneForLimitEntity(context, definition, flangeData, thickness);
        offsets = getOffsetsForSideEdgesUpToPlane(context, sideAndBase.sideDirections, flangeDirection, basePoints, planeResult);
    }

    var sketchPlane = flangeData.plane;
    var points = [worldToPlane(sketchPlane, basePoints[0]),
        worldToPlane(sketchPlane, basePoints[1]),
        worldToPlane(sketchPlane, basePoints[1] + offsets[1]),
        worldToPlane(sketchPlane, basePoints[0] + offsets[0])];
    points = append(points, points[0]);

    var sketch = newSketchOnPlane(context, id, { "sketchPlane" : sketchPlane });
    skPolyline(sketch, "polyline", { "points" : points });
    skSolve(sketch);

    var regions = evaluateQuery(context, qCreatedBy(id, EntityType.FACE));
    if (size(regions) != 1)
    {
        //This can happen in up-to-vertex flanges with auto-miter. (creating hour glass shape)
        setErrorEntities(context, topLevelId, { "entities" : qCreatedBy(id, EntityType.FACE) });
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL);
    }
}

function createFlangeSurfaceReturnBendEdge(context is Context, topLevelId is Id, indexedId is Id, edge is Query,
    flangeData is map, sideAndBase is map, flangeDistance, definition is map) returns Query
{
    var sketchId = indexedId + "sketch";
    createAndSolveSketch(context, topLevelId, sketchId, edge, flangeData, sideAndBase, flangeDistance, definition);
    var bendLine = startTracking(context, sketchId, "polyline.line0");
    opExtractSurface(context, indexedId + SURFACE_SUFFIX, { "faces" : qSketchRegion(sketchId, false) });

    opDeleteBodies(context, indexedId + "delete_sketch", { "entities" : qCreatedBy(sketchId, EntityType.BODY) });

    return qIntersection([qCreatedBy(indexedId + SURFACE_SUFFIX, EntityType.EDGE), bendLine]);
}

function getBoundField(isStart is boolean) returns string
{
    return isStart ? "startBound" : "endBound";
}

function makeBlindBoundingMap(isStart is boolean, upToPoint is Vector, edgeMidpt is Vector, edgeDirection is Vector) returns map
{
    const depthField = isStart ? "startDepth" : "endDepth";
    return {
            getBoundField(isStart) : BoundingType.BLIND,
            (depthField) : dot(upToPoint - edgeMidpt, (isStart ? -1 : 1) * edgeDirection)
        };
}

function createArcToExtrude(context is Context, sketchId is Id, edge is Query,  edgeMidpt is map, flangeData is map,
                    definition is map, flangeDistance, bodiesToDelete is box) returns map
{
    const faceTangentPlaneAtEdge = evFaceTangentPlaneAtEdge(context, {
                "edge" : edge,
                "face" : flangeData.adjacentFace,
                "parameter" : 0.5,
                "usingFaceOrientation" : true
            });
    const faceNormal = faceTangentPlaneAtEdge.normal;
    const outFromFace = cross(edgeMidpt.direction, faceNormal);
    const sketchPlane = plane(edgeMidpt.origin, edgeMidpt.direction, outFromFace);
    const sketch =  newSketchOnPlane(context, sketchId, {
                "sketchPlane" : sketchPlane
            });

    const modelParameters = getModelParametersFromEdge(context, edge);

    const arcInfo = sketchArc(sketch, definition.oppositeDirection, modelParameters, definition.bendRadius, flangeData.bendAngle);
    skSolve(sketch);

    bodiesToDelete[] = append(bodiesToDelete[], qCreatedBy(sketchId, EntityType.BODY));
    const arcEndEntity = sketchEntityQuery(sketchId, EntityType.VERTEX, arcInfo.arcEndId);
    return {
        "arcEdge" : startTracking(context, sketchEntityQuery(sketchId, EntityType.EDGE, arcInfo.arcId)),
        "arcEnd" : qUnion([arcEndEntity, startTracking(context, arcEndEntity)])
    };
}

function extrudeArc(context is Context, id is Id, sketchId is Id, sideAndBase is map, edgeMidpt is Line)
{
    const boundingMapStart = makeBlindBoundingMap(true, sideAndBase.basePoints[0],  edgeMidpt.origin, edgeMidpt.direction);
    const boundingMapEnd = makeBlindBoundingMap(false, sideAndBase.basePoints[1],  edgeMidpt.origin, edgeMidpt.direction);

    var extDefinition = {
        "entities" : qCreatedBy(sketchId, EntityType.EDGE),
        "direction" : edgeMidpt.direction
    };
    extDefinition = mergeMaps(extDefinition, boundingMapStart);
    extDefinition = mergeMaps(extDefinition, boundingMapEnd);

    try
    {
        opExtrude(context, id, extDefinition);
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL);
    }

}
/**
 * Create a tangent cylinder based on bend angle and a tangent planar face off of the cylinder
 */
function createFlangeSurfacesAdjacentToCone(context is Context, topLevelId is Id, indexedId is Id, edge is Query,
    flangeData is map, sideAndBase is map, flangeDistance, definition is map, bodiesToDelete is box) returns map
{
    //create sketch profile (arc)
    const edgeMidpt = evEdgeTangentLine(context, {
            "edge" : edge,
            "parameter" : .5,
            "face" : flangeData.adjacentFace
    });
    const sketchId1 = indexedId + SURFACE_SUFFIX + "sketch1";
    const sketchEntities = createArcToExtrude(context, sketchId1, edge, edgeMidpt, flangeData, definition, flangeDistance, bodiesToDelete);
    const arcEndVertex = qIntersection([qCreatedBy(sketchId1, EntityType.VERTEX), sketchEntities.arcEnd]);

    //extrude the arc
    extrudeArc(context, indexedId + SURFACE_SUFFIX, sketchId1, sideAndBase, edgeMidpt);

    //update basePoints and flange plane
    const newBaseVertices = qIntersection([qCreatedBy(indexedId + SURFACE_SUFFIX, EntityType.VERTEX), sketchEntities.arcEnd]);
    const basePt0 = evVertexPoint(context, {
            "vertex" : qIntersection([qCapEntity(indexedId + SURFACE_SUFFIX, CapType.START, EntityType.VERTEX), newBaseVertices])
    });
    const basePt1 = evVertexPoint(context, {
            "vertex" : qIntersection([qCapEntity(indexedId + SURFACE_SUFFIX, CapType.END, EntityType.VERTEX), newBaseVertices])
    });
    sideAndBase.basePoints = [basePt0, basePt1];
    flangeData.plane = plane(basePt0, cross(flangeData.direction, edgeMidpt.direction));

    //sketch the flange face, extract surface
    var sketchId2 = indexedId + SURFACE_SUFFIX + "sketch2";
    createAndSolveSketch(context, topLevelId, sketchId2, edge, flangeData, sideAndBase, flangeDistance, definition);
    opExtractSurface(context, indexedId + SURFACE_SUFFIX + "extract", { "faces" : qSketchRegion(sketchId2, false) });

    bodiesToDelete[] = append(bodiesToDelete[], qCreatedBy(sketchId2, EntityType.BODY));

    const facesCreated = qCreatedBy(indexedId + SURFACE_SUFFIX, EntityType.FACE);
    return { "bendFace" : qIntersection([facesCreated, sketchEntities.arcEdge]),
             "wallFace" : qCreatedBy(indexedId + SURFACE_SUFFIX + "extract", EntityType.FACE),
             "edgeMidpt" : edgeMidpt.origin, //used in matching
             "otherEdgePt" : evVertexPoint(context, {"vertex" : arcEndVertex }) //used in matching
             };
}

function sketchArc(sketch is Sketch, wrapAroundFront is boolean, modelParameters is map,
    innerRadius is ValueWithUnits, angle is ValueWithUnits) returns map
{
    const arcId = "initialFlangeArc";
    const arcResult = prepareInitialArcSketch(wrapAroundFront, modelParameters, innerRadius, angle);
    skArc(sketch, arcId, {
                "start" : vector(0, 0) * inch,
                "mid" : arcResult.arcMid,
                "end" : arcResult.arcEnd
            });

    return {
            "arcId" : arcId,
            "arcEnd" : arcResult.arcEnd,
            "arcEndId" : arcId ~ "." ~ (wrapAroundFront ? "start" : "end"),
            "tangentAtEnd" : arcResult.tangentAtEnd
        };
}

function getModelParametersFromEdge(context is Context, edge is Query) returns map
{
    var adjacentFace = qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE);
    if (size(evaluateQuery(context, adjacentFace)) != 1)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_INTERNAL, ["edges"]);
    }
    return getModelParameters(context, qOwnerBody(adjacentFace));
}


function tolerantParallel(direction0 is Vector, direction1 is Vector, stricter is boolean) returns boolean
{
    var limit = stricter ? (TOLERANCE.zeroAngle * TOLERANCE.zeroAngle) : (TOLERANCE.g1Angle * TOLERANCE.g1Angle);
    return squaredNorm(cross(direction0, direction1)) < limit;
}

function tolerantCollinear(line0 is Line, line1 is Line, stricter is boolean) returns boolean
{
    if (tolerantParallel(line0.direction, line1.direction, stricter))
    {
        var v = line1.origin - line0.origin;
        v = v - line0.direction * dot(v, line0.direction);
        var lengthTolerance = TOLERANCE.zeroLength * meter;
        return squaredNorm(v) < lengthTolerance * lengthTolerance;
    }
    return false;
}

// Used in alignment change calculation. See getSimpleExtensionDistance(...)
function inFlangeThickness(definition is map)
{
    if (definition.flangeAlignment == SMFlangeAlignment.OUTER)
    {
        return (definition.oppositeDirection) ? -definition.backThickness : -definition.frontThickness;
    }
    else if (definition.flangeAlignment == SMFlangeAlignment.MIDDLE)
    {
        var sign = (definition.oppositeDirection) ? -1 : 1;
        return sign * 0.5 * (definition.backThickness - definition.frontThickness);
    }
    else if (definition.flangeAlignment == SMFlangeAlignment.INNER)
    {
        return (definition.oppositeDirection) ? definition.frontThickness : definition.backThickness;
    }
    else if (definition.flangeAlignment == SMFlangeAlignment.BEND)
    {
        return ((definition.oppositeDirection) ? definition.frontThickness : definition.backThickness) + definition.bendRadius;
    }
}

function processParallelEntity(context is Context, definition is map) returns map
{
    if (definition.angleControlType == SMFlangeAngleControlType.ALIGN_GEOMETRY)
    {
        var parallelEntities = evaluateQuery(context, definition.parallelEntity);
        if (size(parallelEntities) != 1)
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_NO_PARALLEL_ENTITY, ["parallelEntity"]);
        var parallelEntity = parallelEntities[0];

        var direction = extractDirection(context, parallelEntity);
        if (direction == undefined)
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_NO_PARALLEL_ENTITY, ["parallelEntity"]);

        // Check parallelEntity for its GeometryType.
        var lineEntity = qUnion([qGeometry(parallelEntity, GeometryType.LINE), qGeometry(parallelEntity, GeometryType.CYLINDER)]);
        var isLineEntity = size(evaluateQuery(context, lineEntity)) == 1;
        var planeEntity = qUnion([
                qGeometry(parallelEntity, GeometryType.PLANE),
                qGeometry(parallelEntity, GeometryType.CIRCLE),
                qGeometry(parallelEntity, GeometryType.ARC)
            ]);
        var isPlaneEntity = size(evaluateQuery(context, planeEntity)) == 1;

        // return differently depending on whether the flange should be parallel to the direction of the entity, or parallel
        // to the plane whose normal is the direction of the entity.  See usage in `createSketchPlane`
        if (isLineEntity)
        {
            return { "parallelLineDirection" : direction };
        }
        else if (isPlaneEntity)
        {
            return { "parallelPlaneNormal" : direction };
        }
        else
        {
            // Should not be thrown as definition.parallelEntity uses QueryFilterCompound.ALLOWS_DIRECTION
            throw "Unexpected entity type used for parallel entity";
        }
    }
    else if (definition.angleControlType == SMFlangeAngleControlType.ANGLE_FROM_DIRECTION)
    {
        // Make sure there is one direction entity
        var directionEntities = evaluateQuery(context, definition.directionEntity);
        if (size(directionEntities) != 1)
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_NO_DIRECTION_ENTITY, ["directionEntity"]);

        // Make sure the direction entity supplies a valid direction
        var direction = extractDirection(context, directionEntities[0]);
        if (direction == undefined)
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_NO_DIRECTION_ENTITY, ["directionEntity"]);

        return { "parallelDirection" : direction };
    }

    return {};
}

function findMatchingPoint(points1 is array, points2 is array) returns array
{
    for (var i = 0; i < size(points1); i += 1)
    {
        for (var j = 0; j < size(points2); j += 1)
        {
            if (tolerantEquals(points1[i], points2[j]))
            {
                return [i, j];
            }
        }
    }
    return [-1, -1];
}

// Match the length of a flange such that, when auto-mitered, it aligns with the flange extending from the parent edge
// Args should contain topLevelId, edgeToFlangeData, and edgeToSideAndBase
const calculateMatchedFlangeDistance = function(context is Context, currEdge is Query, parentEdge, edgeToFlangeDistance is map, args is map) returns map
{
    var flangeDistance;
    var success = true;
    var currFlangeData = args.edgeToFlangeData[currEdge];

    if (parentEdge == undefined)
    {
        // Base case: flange distance is the alignedDistance
        flangeDistance = currFlangeData.alignedDistance;
    }
    else
    {
        // Follower case: find how far the flange of the parent edge is going to extend, and match such that the
        // auto-miter meets at a shared point.

        var parentFlangeData = args.edgeToFlangeData[parentEdge];

        // Make sure that the neighboring flanges share a base point.
        var currBasePoints = args.edgeToSideAndBase[currEdge].basePoints;
        var parentBasePoints = args.edgeToSideAndBase[parentEdge].basePoints;
        var basePointIndices = findMatchingPoint(currBasePoints, parentBasePoints);
        if (basePointIndices[0] == -1)
        {
            // If they do not share a base point, come back to this edge later
            return { "success" : false };
        }
        // The matching basePoint in currBasePoints and parentBasePoints are tolerantEqual, so pick the one in
        // currBasePoints arbitrarily
        var basePoint = currBasePoints[basePointIndices[0]];

        // Make sure the neighboring flanges share a side direction
        var currSideDirection = args.edgeToSideAndBase[currEdge].sideDirections[basePointIndices[0]];
        var parentSideDirection = args.edgeToSideAndBase[parentEdge].sideDirections[basePointIndices[1]];
        if (!parallelVectors(currSideDirection, parentSideDirection))
        {
            // If they do not share a side direction, come back to this edge later
            return { "success" : false };
        }

        // Intersect a plane capping the end of the parent flange with a line representing the auto-miter. The current
        // flange should extend so that it reaches exactly this point.
        var parentFlangeEndPoint = basePoint + (parentFlangeData.direction * edgeToFlangeDistance[parentEdge]);
        var parentFlangeEndPlane = plane(parentFlangeEndPoint, parentFlangeData.direction);
        var sideLine = line(basePoint, currSideDirection);
        var intersectionResult = intersection(parentFlangeEndPlane, sideLine);
        if (intersectionResult.dim != 0)
        {
            setErrorEntities(context, args.topLevelId, { "entities" : qUnion([currEdge, parentEdge]) });
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_AUTO_MITER, ["edges"]);
        }
        flangeDistance = dot(intersectionResult.intersection - basePoint, currFlangeData.direction);
    }

    return {
            "success" : success,
            "data" : flangeDistance
        };
};


function collectEdgeToFlangeDistance(context is Context, topLevelId is Id, edges is Query, edgeToFlangeData is map, edgeToSideAndBase is map, definition is map) returns map
{
    // No distances needed
    if (definition.limitType != SMFlangeBoundingType.BLIND)
        return {};

    // Simple case
    if (!definition.autoMiter || definition.angleControlType == SMFlangeAngleControlType.BEND_ANGLE)
    {
        var edgeToFlangeDistance = {};
        for (var edge in evaluateQuery(context, edges))
        {
            edgeToFlangeDistance[edge] = edgeToFlangeData[edge].alignedDistance;
        }
        return edgeToFlangeDistance;
    }

    return collectEdgeDataFromOrderedTraversal(context, edges, calculateMatchedFlangeDistance, {
                "topLevelId" : topLevelId,
                "edgeToFlangeData" : edgeToFlangeData,
                "edgeToSideAndBase" : edgeToSideAndBase
            });
}

function getSimpleExtensionDistance(definition is map, flangeData is map)
{
    return definition.inFlangeThickness * tan(flangeData.bendAngle / 2);
}

// Match the extension distance of a flange wall to the extension distance of the flange wall of the parent edge
// Args should contain topLevelId, definition, and edgeToFlangeData
const calculateMatchedExtensionDistance = function(context is Context, currEdge is Query, parentEdge, edgeToExtensionDistance is map, args is map) returns map
{
    var extensionLength;
    var currFlangeData = args.edgeToFlangeData[currEdge];

    if (parentEdge == undefined)
    {
        // Base case: extension distance is calculated directly off of SMFlangeAlignment
        extensionLength = getSimpleExtensionDistance(args.definition, currFlangeData);
    }
    else
    {
        // Follower case: set the extension distance such that topologically connected edges remain topologically connected
        // after the extension is applied

        var parentFlangeData = args.edgeToFlangeData[parentEdge];
        var samePlane = (isAtVersionOrLater(context, FeatureScriptVersionNumber.V695_SM_SWEPT_SUPPORT)) ?
        versionedCoplanarPlanes(context, parentFlangeData.wallPlane, currFlangeData.wallPlane) :
        tolerantEquals(parentFlangeData.wallPlane, currFlangeData.wallPlane);

        if (samePlane)
        {
            // Parent edge and current edge belong to the same wall.  Any extension will align.
            extensionLength = getSimpleExtensionDistance(args.definition, currFlangeData);
        }
        else
        {
            // Intersect a line representing the new location of the parent edge with the plane of the current wall. The current
            // edge should be moved such that it contains this intersection point.
            var parentExtension = parentFlangeData.wallExtendDirection * edgeToExtensionDistance[parentEdge];
            var parentLine = line(parentFlangeData.edgeEndPoints[0].origin + parentExtension, parentFlangeData.edgeEndPoints[0].direction);

            var intersectionResult = intersection(currFlangeData.wallPlane, parentLine);
            if (intersectionResult.dim != 0)
            {
                failDueToAlignmentIssue(context, args.topLevelId, currEdge);
            }

            // Project the result of the intersection onto a line representing the current edge to find the distance that
            // the current edge should be moved.
            var currLine = currFlangeData.edgeEndPoints[0];
            var projectedPoint = project(currLine, intersectionResult.intersection);

            var extension = intersectionResult.intersection - projectedPoint;
            extensionLength = dot(extension, currFlangeData.wallExtendDirection);
        }
    }

    return {
            "success" : true,
            "data" : extensionLength
        };
};

function collectEdgeToExtensionDistance(context is Context, topLevelId is Id, edges is Query, edgeToFlangeData is map, definition is map) returns map
{
    // Simple case
    if (definition.angleControlType == SMFlangeAngleControlType.BEND_ANGLE)
    {
        var edgeToExtensionDistance = {};
        for (var edge in evaluateQuery(context, edges))
        {
            edgeToExtensionDistance[edge] = getSimpleExtensionDistance(definition, edgeToFlangeData[edge]);
        }
        return edgeToExtensionDistance;
    }

    return collectEdgeDataFromOrderedTraversal(context, edges, calculateMatchedExtensionDistance, {
                "topLevelId" : topLevelId,
                "definition" : definition,
                "edgeToFlangeData" : edgeToFlangeData
            });
}

/**
 * This functions runs a breadth-first traversal over connected edges in the `edges` query, calling a supplied function
 * to gather some piece of data about each edge.  The caller of this function should supply a function `calculation`
 * which follows the following signature:
 *
 * function calculation(context is Context, currEdge is Query, parentEdge, edgeToData is map, args is map) returns map
 *
 * where:
 * `currEdge` is the current edge to be calculated
 * `parentEdge` is an adjacent edge to `currEdge` which has already been calculated, or `undefined` if `currEdge` is the first edge in the traversal
 * `edgeToData` is the map of all the data that has already been calculated.  If `parentEdge` is not `undefined`, its data will be in `edgeToData`
 * `args` is a copy of the `args` passed to `collectEdgeDataFromOrderedTraversal` containing additional data `edgeCalculation` may require.
 *
 * `calculation` should return a map with the following fields:
 *     `success` : whether the calculation was successful. If `false`, the `currEdge` will be treated as anomalous.
 *                 Anomalous edges are reserved for the end of the breadth-first traversal.  If, at the end of the traversal,
 *                 there are anomalous edges that were not calculated successfully at some point in the traversal, the first
 *                 unsuccessful anomalous edge will start a new traversal. As the first edge in this new traversal, the
 *                 anomalous edge will have no parent edge.
 *     `data` : the data calculated for `currEdge`.  Only entered in `edgeToData` if `success` is `true`.
 *
 *
 * The case where `calculation` is called with a `parentEdge` of `undefined` should be treated as a base case, and should
 * always succeed. If a `calculation` returns `{ "success" : false }` when `parentEdge` is `undefined`, an error will be thrown.
 * If this rule were not enforced, a deterministic `calculation` that failed with an `undefined` `parentEdge` would repeatedly
 * insert its `currEdge` into the anomalous queue and spin forever trying (and failing) to calculate that edge.
 */
function collectEdgeDataFromOrderedTraversal(context is Context, edges is Query, calculation is function, args is map)
{
    var edgeToData = {};
    var edgeGroups = connectedComponents(context, edges, AdjacencyType.VERTEX);
    for (var group in edgeGroups)
    {
        var queue = [{ "edge" : group[0] }];
        var queueIndex = 0;
        var anomalousQueue = [];
        var anomalousQueueIndex = 0;

        while (queueIndex < size(queue))
        {
            var currEdge = queue[queueIndex].edge;
            var parentEdge = queue[queueIndex].parent;

            // skip edge if it has already been successfully decorated
            if (edgeToData[currEdge] == undefined)
            {
                var result = calculation(context, currEdge, parentEdge, edgeToData, args);
                if (result.success)
                {
                    edgeToData[currEdge] = result.data;

                    // add adjacent unprocessed edges to the queue
                    var adjacentEdges = qAdjacent(currEdge, AdjacencyType.VERTEX, EntityType.EDGE);
                    var edgesToProcess = qSubtraction(qIntersection([adjacentEdges, qUnion(group)]), qUnion(keys(edgeToData)));
                    for (var edgeToProcess in evaluateQuery(context, edgesToProcess))
                    {
                        queue = append(queue, { "edge" : edgeToProcess, "parent" : currEdge });
                    }
                }
                else
                {
                    if (parentEdge == undefined)
                        throw "Calculation without parent should not be unsuccessful";
                    anomalousQueue = append(anomalousQueue, currEdge);
                }
            }

            queueIndex += 1;
            if (queueIndex == size(queue) && anomalousQueueIndex != size(anomalousQueue))
            {
                queue = append(queue, { "edge" : anomalousQueue[anomalousQueueIndex] });
                anomalousQueueIndex += 1;
            }
        }
    }

    return edgeToData;
}

// For use when `edge` may not be linear
function getFacePlane(context is Context, face is Query, edge is Query, position is Vector) returns Plane
{
    var edgeEndPoints = evEdgeTangentLines(context, { "edge" : edge, "parameters" : [0, 1] });
    var closerPointParameter = (squaredNorm(edgeEndPoints[0].origin - position) < squaredNorm(edgeEndPoints[1].origin - position)) ? 0 : 1;
    return evFaceTangentPlaneAtEdge(context, {
                "face" : face,
                "edge" : edge,
                "parameter" : closerPointParameter
            });
}

// For use when `edge` is linear
function getFacePlane(context is Context, face is Query, edge is Query) returns Plane
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V695_SM_SWEPT_SUPPORT))
    {
        return evFaceTangentPlaneAtEdge(context, {
                    "face" : face,
                    "edge" : edge,
                    "parameter" : 0.5 });
    }
    else
    {
        return evPlane(context, { "face" : face });
    }
}

predicate partialFlangePredicate(flangeDefinition is map)
{
    annotation { "Name" : "Partial flange" }
    flangeDefinition.isPartialFlange is boolean;

    if (flangeDefinition.isPartialFlange)
    {
        annotation { "Group Name" : "Overall parameters", "Collapsed By Default" : false }
        {
            annotation { "Name" : "Chain type" }
            flangeDefinition.chainType is SMPartialFlangeChainType;

            annotation { "Name" : "Flip sides", "UIHint" : UIHint.OPPOSITE_DIRECTION, "Default" : false }
            flangeDefinition.flipFlangeBounds is boolean;

            annotation { "Name" : "Hold adjacent edges", "Default" : true }
            flangeDefinition.holdAdjacentEdges is boolean;
        }

        annotation { "Group Name" : "End conditions", "Collapsed By Default" : false }
        {
            annotation { "Name" : "Bound type" }
            flangeDefinition.firstBoundType is SMFlangeBoundingType;
            if (flangeDefinition.firstBoundType == SMFlangeBoundingType.BLIND)
            {
                annotation { "Name" : "Distance" }
                isLength(flangeDefinition.firstBoundOffset, NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS);
            }
            else if (flangeDefinition.firstBoundType == SMFlangeBoundingType.UP_TO_ENTITY || flangeDefinition.firstBoundType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
            {
                annotation { "Name" : "Up to entity", "Filter" : EntityType.FACE || EntityType.VERTEX, "MaxNumberOfPicks" : 1 }
                flangeDefinition.firstBoundingEntity is Query;

                if (flangeDefinition.firstBoundType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
                {
                    annotation { "Name" : "Distance" }
                    isLength(flangeDefinition.firstBoundingEntityOffset, NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS);

                    annotation { "Name" : "Opposite offset direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                    flangeDefinition.firstBoundingEntityOppositeOffsetDirection is boolean;
                }
            }

            annotation { "Name" : "Second bound" }
            flangeDefinition.hasSecondBound is boolean;
            if (flangeDefinition.hasSecondBound)
            {
                annotation { "Name" : "Second bound type" }
                flangeDefinition.secondBoundType is SMFlangeBoundingType;
                if (flangeDefinition.secondBoundType == SMFlangeBoundingType.BLIND)
                {
                    annotation { "Name" : "Second distance" }
                    isLength(flangeDefinition.secondBoundOffset, NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS);
                }
                else if (flangeDefinition.secondBoundType == SMFlangeBoundingType.UP_TO_ENTITY || flangeDefinition.secondBoundType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
                {
                    annotation { "Name" : "Up to entity", "Filter" : EntityType.FACE || EntityType.VERTEX, "MaxNumberOfPicks" : 1 }
                    flangeDefinition.secondBoundingEntity is Query;

                    if (flangeDefinition.secondBoundType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
                    {
                        annotation { "Name" : "Second distance" }
                        isLength(flangeDefinition.secondBoundingEntityOffset, NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS);

                        annotation { "Name" : "Opposite offset direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                        flangeDefinition.secondBoundingEntityOppositeOffsetDirection is boolean;
                    }
                }
            }
        }
    }
}

type PartialFlangeBound typecheck canBePartialFlangeBound;

predicate canBePartialFlangeBound(value)
{
    value is map;
    value.partialFlangeType is SMFlangeBoundingType;
    if (value.partialFlangeType == SMFlangeBoundingType.BLIND)
    {
        value.offset is ValueWithUnits;
    }
    else if (value.partialFlangeType == SMFlangeBoundingType.UP_TO_ENTITY || value.partialFlangeType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
    {
        value.boundEntity is Query;
        if (value.partialFlangeType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
        {
            value.offset is ValueWithUnits;
        }
    }
}

// Take the user definition and convert it to a consistent type for typechecks. Workaround for
// precondition limitations on parameter ids.
function convertDefinitionToFlangeBound(definition is map) returns array
{
    var bounds = [];
    if (definition.isPartialFlange)
    {
        var firstFlangeBound = {
            "partialFlangeType" : definition.firstBoundType,
            "isFirstBound" : true,
            "partialFlangeOppositeOffsetDirection" : definition.firstBoundingEntityOppositeOffsetDirection
        };
        if (definition.firstBoundType == SMFlangeBoundingType.BLIND)
        {
            firstFlangeBound.offset = definition.firstBoundOffset;
        }
        else if (definition.firstBoundType == SMFlangeBoundingType.UP_TO_ENTITY ||
            definition.firstBoundType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
        {
            firstFlangeBound.boundEntity = definition.firstBoundingEntity;
            if (definition.firstBoundType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
            {
                firstFlangeBound.offset = definition.firstBoundingEntityOppositeOffsetDirection ? -definition.firstBoundingEntityOffset : definition.firstBoundingEntityOffset;
            }
        }
        bounds = bounds->append(firstFlangeBound as PartialFlangeBound);
        if (definition.hasSecondBound)
        {
            var secondFlangeBound = {
                "partialFlangeType" : definition.secondBoundType,
                "isFirstBound" : false,
                "partialFlangeOppositeOffsetDirection" : definition.secondBoundingEntityOppositeOffsetDirection
            };
            if (definition.secondBoundType == SMFlangeBoundingType.BLIND)
            {
                secondFlangeBound.offset = definition.secondBoundOffset;
            }
            else if (definition.secondBoundType == SMFlangeBoundingType.UP_TO_ENTITY ||
                definition.secondBoundType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
            {
                secondFlangeBound.boundEntity = definition.secondBoundingEntity;
                if (definition.secondBoundType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
                {
                    secondFlangeBound.offset = definition.secondBoundingEntityOppositeOffsetDirection ? -definition.secondBoundingEntityOffset : definition.secondBoundingEntityOffset;
                }
            }

            bounds = bounds->append(secondFlangeBound as PartialFlangeBound);
        }
    }
    return bounds;
}

function getUpToEntityParameter(boundIndex is number)
{
    return boundIndex == 0 ? "firstBoundingEntity" : "secondBoundingEntity";
}

// Take a flange bound and apply it to a particular edge from the start or the end.
// Return the parameter that the bound needs to split the edge at.
function processOneFlangeBound(context is Context, flangeBound is PartialFlangeBound, edgeLength is ValueWithUnits,
    modelEdge is Query, trimFromEdgeStart is boolean, boundIndex is number, holdAdjacentEdges is boolean) returns map
{
    const includePosition = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1903_HOLD_ADJACENT_EDGES);
    if (flangeBound.partialFlangeType == SMFlangeBoundingType.BLIND)
    {
        var parameter = flangeBound.offset / edgeLength;
        if (!trimFromEdgeStart)
        {
            parameter = 1 - parameter;
        }
        if (includePosition && !holdAdjacentEdges)
        {
            const position = try silent(evEdgeTangentLine(context, {
                                "edge" : modelEdge,
                                "parameter" : parameter
                            }).origin);
            if (position == undefined)
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL, ["edges"]);
            }
            return { "parameter" : parameter, "limit" : { "position" : position } };
        }
        return { "parameter" : parameter };
    }
    else if (flangeBound.partialFlangeType == SMFlangeBoundingType.UP_TO_ENTITY ||
        flangeBound.partialFlangeType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
    {
        const distanceResult = try silent(evDistance(context, {
                        "side0" : modelEdge,
                        "side1" : flangeBound.boundEntity
                    }));
        if (distanceResult == undefined)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_NO_BOUNDING_ENTITY, [getUpToEntityParameter(boundIndex)]);
        }
        var parameter = distanceResult.sides[0].parameter;
        if (flangeBound.partialFlangeType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
        {
            var parameterOffset = flangeBound.offset / edgeLength;
            parameterOffset = trimFromEdgeStart ? -parameterOffset : parameterOffset;
            parameter += parameterOffset;
        }

        var limit = { "limitPlane" : getPlaneForLimitEntity(context, modelEdge, flangeBound.boundEntity, parameter, boundIndex) };
        if (includePosition && !holdAdjacentEdges)
        {
            if (flangeBound.partialFlangeType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
            {
                const position = try silent(evEdgeTangentLine(context, {
                                    "edge" : modelEdge,
                                    "parameter" : parameter
                                }).origin);
                if (position == undefined)
                {
                    throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL);
                }
                limit.position = position;
            }
            else
            {
                limit.position = distanceResult.sides[0].point;
            }
        }
        return { "parameter" : parameter, "limit" : limit };
    }
}

function getPlaneForLimitEntity(context is Context, edge is Query, limitEntity is Query, splitParameter is number, boundIndex is number)
{
    if (isQueryEmpty(context, limitEntity))
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_NO_BOUNDING_ENTITY, [getUpToEntityParameter(boundIndex)]);
    }

    const tangent = try silent(evEdgeTangentLine(context, {
                    "edge" : edge,
                    "parameter" : splitParameter
                }));

    if (tangent == undefined)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL);
    }

    const vertex = try silent(evVertexPoint(context, {
                    "vertex" : limitEntity
                }));
    if (vertex == undefined)
    {
        const plane = try silent(evPlane(context, {
                        "face" : limitEntity
                    }));
        if (plane == undefined)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_BOUNDING_ENTITY_NOT_SUPPORTED, [getUpToEntityParameter(boundIndex)]);
        }
        if (perpendicularVectors(plane.normal, tangent.direction))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_BOUNDING_ENTITY_PARALLEL, [getUpToEntityParameter(boundIndex)]);
        }
        return plane;
    }
    else
    {
        return plane(tangent.origin, tangent.direction);
    }
}

function showSplitErrorLocations(context is Context, modelEdge is Query, splitParameters is array, length)
{
    const tangent = evEdgeTangentLine(context, {
                "edge" : modelEdge,
                "parameter" : 0
            });
    var points = [];
    for (var parameter in splitParameters)
    {
        const point = tangent.origin + tangent.direction * parameter * length;
        addDebugPoint(context, point, DebugColor.RED);
        points = points->append(point);
    }
    if (points->size() == 2)
    {
        if (!tolerantEquals(points[0], points[1]))
        {
            addDebugLine(context, points[0], points[1], DebugColor.RED);
        }
    }
}

/**
 * Convert transient ids in return to queries that will survive edge setback.
 */
function convertReturnToQuery(context is Context, operationId is Id, modelEdge is Query, limitEntities is map, midPoint)
{
    const edgeQuery = midPoint != undefined ?
        makeRobustQuery(context, qContainsPoint(qSplitBy(operationId, EntityType.EDGE, false), midPoint)) :
        makeRobustQuery(context, modelEdge);

    var convertedLimits = {};
    for (var index, limit in limitEntities)
    {
        // Use a query for the key that will work whether or not the edge is moved as part of flange alignment.
        // This will be converted into a transient query before the map is consumed.
        var vertexQuery = midPoint != undefined ?
            makeRobustQuery(context, qEdgeVertex(qContainsPoint(qSplitBy(operationId, EntityType.EDGE, false), midPoint), index == 0)) :
            makeRobustQuery(context, qEdgeVertex(modelEdge, index == 0));
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2678_HEM_FLANGE_FIXES) && midPoint == undefined)
        {
            //use robustQuery of the edge to create vertexQuery
            vertexQuery = qEdgeVertex(edgeQuery, index == 0);
        }
        convertedLimits[vertexQuery] = limit;
    }
    return {
            "splitEdgeQuery" : edgeQuery,
            "limitEntities" : convertedLimits
        };
}

// Divide an edge for the given bounds in order to localize a flange.
// Returns a query for the split edge that the flange should be attached to.
function splitEdgeForPartialFlange(context is Context, topLevelId is Id, definition is map, operationId is Id, modelEdge is Query, bounds is array, isAlignedWithEdge is boolean, holdAdjacentEdges is boolean, addManipulators is boolean) returns map
{
    var limitEntities = {};
    const length = try silent(evLength(context, {
                    "entities" : modelEdge
                }));
    if (length == undefined)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL, "edges");
    }

    //edge adjacent to cone should not get affected by the holdAdjacentEdges flag
    const adjacentConeFacesQ = modelEdge->qAdjacent(AdjacencyType.EDGE, EntityType.FACE)->qGeometry(GeometryType.CONE);
    if (!isQueryEmpty(context, adjacentConeFacesQ))
        holdAdjacentEdges = true;

    var splitParameters = [];
    for (var index, flangeBound in bounds)
    {
        const trimAtStart = (index == 0) == isAlignedWithEdge;
        const parameterAndLimit = processOneFlangeBound(context, flangeBound, length, modelEdge, trimAtStart, index, holdAdjacentEdges);
        if (parameterAndLimit.limit != undefined)
        {
            const vertexIndex = trimAtStart ? 0 : 1;
            limitEntities[vertexIndex] = parameterAndLimit.limit;
        }
        splitParameters = splitParameters->append(parameterAndLimit.parameter);

        if (addManipulators && (flangeBound.partialFlangeType == SMFlangeBoundingType.BLIND
            || (flangeBound.partialFlangeType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET && parameterAndLimit.limit != undefined)))
        {
            var manipPosition = evEdgeTangentLine(context, {
                    "edge" : modelEdge,
                    "parameter" : trimAtStart ? 0 : 1
                });

            if (flangeBound.partialFlangeType == SMFlangeBoundingType.BLIND)
            {
                manipPosition.direction *= trimAtStart ? 1 : -1;
            }
            else
            {
                const distanceResult = try silent(evDistance(context, {
                        "side0" : modelEdge,
                        "side1" : flangeBound.boundEntity
                    }));
                const parameter = distanceResult.sides[0].parameter < 0 ? 0 : distanceResult.sides[0].parameter;

                manipPosition = evEdgeTangentLine(context, {
                    "edge" : modelEdge,
                    "parameter" : parameter
                });

                if (parameter > parameterAndLimit.parameter)
                {
                    manipPosition.direction = -manipPosition.direction;
                }
            }

            addPartialFlangeManipulators(context, topLevelId, definition, flangeBound, manipPosition);
        }
    }

    // Skip splitting the edge but provide an end position for the flange instead.
    // This stops flange alignment from creating a step edge.
    const splitConditionally = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1903_HOLD_ADJACENT_EDGES);
    if (splitConditionally && !holdAdjacentEdges)
    {
        const splitPoint = undefined;
        return convertReturnToQuery(context, operationId, modelEdge, limitEntities, splitPoint);
    }

    if (splitParameters->size() == 1)
    {
        splitParameters = splitParameters->append(isAlignedWithEdge ? 1 : 0);
    }

    const lowerParameterIndex = isAlignedWithEdge ? 0 : 1;
    const higherParameterIndex = 1 - lowerParameterIndex;
    if (length * (splitParameters[higherParameterIndex] - splitParameters[lowerParameterIndex]) < TOLERANCE.zeroLength * meter)
    {
        showSplitErrorLocations(context, modelEdge, splitParameters, length);
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_ZERO_WIDTH);
    }

    if (splitParameters[lowerParameterIndex] * length < -TOLERANCE.zeroLength * meter ||
        (splitParameters[higherParameterIndex] - 1) * length > TOLERANCE.zeroLength * meter)
    {
        showSplitErrorLocations(context, modelEdge, splitParameters, length);
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_PARAMETER_BOUND);
    }

    if (abs(splitParameters[lowerParameterIndex]) * length < TOLERANCE.zeroLength * meter &&
        abs(splitParameters[1 - lowerParameterIndex] - 1) * length < TOLERANCE.zeroLength * meter)
    {
        const splitPoint = undefined;
        return convertReturnToQuery(context, operationId, modelEdge, limitEntities, splitPoint);
    }
    const midSplitPoint = evEdgeTangentLine(context, {
                    "edge" : modelEdge,
                    "parameter" : (splitParameters[0] + splitParameters[1]) / 2
                }).origin;

    try
    {
        @opSplitEdges(context, operationId, {
                    "edges" : modelEdge,
                    "parameters" : [splitParameters]
                });

    }
    catch (error)
    {
        showSplitErrorLocations(context, modelEdge, splitParameters, length);
        if (error == ErrorStringEnum.SPLIT_EDGE_PARAMETER_BOUND)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_PARAMETER_BOUND);
        }
        else
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL);
        }
    }

    return convertReturnToQuery(context, operationId, modelEdge, limitEntities, midSplitPoint);
}

function alignPathDirectionByInput(context is Context, path is Path, inputEdges is Query) returns Path
{
    const edges = qIntersection(inputEdges, qUnion(path.edges));
    if (path.edges->size() > 1 && !isQueryEmpty(context, edges))
    {
        const firstEdge = qNthElement(edges, 0);
        for (var index, edge in path.edges)
        {
            if (!isQueryEmpty(context, qIntersection(firstEdge, edge)))
            {
                if (path.flipped[index])
                {
                    return reverse(path);
                }
                break;
            }
        }
    }
    return path;
}

function splitAllEdgesForPartialFlange(context is Context, topLevelId is Id, operationId, definition is map, modelEdges is Query) returns map
{
    var splitEdgeQueries = [];
    var limitEntities = {};
    const bounds = convertDefinitionToFlangeBound(definition);
    const paths = constructPaths(context, modelEdges, {});
    var operationIndex = 0;
    const keepInputOrder = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1934_PARTIAL_FLANGE_STABLE_EDGE_ORDER);
    var addManipulators = !isInFeaturePattern(context);
    if (definition.chainType == SMPartialFlangeChainType.PER_EDGE)
    {
        // SMPartialFlangeChainType.PER_EDGE => bounds are applied to every edge identically.
        for (var path in paths)
        {
            if (keepInputOrder)
            {
                path = alignPathDirectionByInput(context, path, modelEdges);
            }
            for (var index, edge in path.edges)
            {
                const alignedWithEdge = definition.flipFlangeBounds == path.flipped[index];
                const splitQueryAndLimitEntity = splitEdgeForPartialFlange(context, topLevelId, definition, operationId + unstableIdComponent(operationIndex),
                    edge, bounds, alignedWithEdge, definition.holdAdjacentEdges, addManipulators);
                if (splitQueryAndLimitEntity.limitEntities != undefined)
                {
                    limitEntities[splitQueryAndLimitEntity.splitEdgeQuery] = splitQueryAndLimitEntity.limitEntities;
                }
                splitEdgeQueries = splitEdgeQueries->append(splitQueryAndLimitEntity.splitEdgeQuery);
                operationIndex += 1;
                addManipulators = false;
            }
        }
    }
    else
    {
        // SMPartialFlangeChainType.PER_CHAIN => bounds are applied to the start and ends of chains.
        for (var path in paths)
        {
            // If a path has more than one edge, apply bounds to the first and last edges in the path.
            // Otherwise, apply all bounds to the only edge.
            if (path.edges->size() == 1)
            {
                // Use path direction since it is stabilized by vertex deterministic id.
                const alignedWithEdge = definition.flipFlangeBounds == path.flipped[0];
                const splitQueryAndLimitEntity = splitEdgeForPartialFlange(context, topLevelId, definition, operationId + unstableIdComponent(operationIndex),
                    path.edges[0], bounds, alignedWithEdge, definition.holdAdjacentEdges, addManipulators);
                if (splitQueryAndLimitEntity.limitEntities != undefined)
                {
                    limitEntities[splitQueryAndLimitEntity.splitEdgeQuery] = splitQueryAndLimitEntity.limitEntities;
                }
                splitEdgeQueries = splitEdgeQueries->append(splitQueryAndLimitEntity.splitEdgeQuery);
                operationIndex += 1;
            }
            else
            {
                // Options for a closed path are to either not do a partial flange at all or fail.
                if (path.closed)
                {
                    splitEdgeQueries = splitEdgeQueries->append(qUnion(path.edges));
                    continue;
                }
                if (keepInputOrder)
                {
                    path = alignPathDirectionByInput(context, path, modelEdges);
                }
                const startOffset = (bounds->size() == 1) && definition.flipFlangeBounds ? 0 : 1;
                const endOffset = (bounds->size() == 1) && !definition.flipFlangeBounds ? 0 : 1;
                // Middle edges of the path get added unchanged.
                for (var i = startOffset; i < path.edges->size() - endOffset; i += 1)
                {
                    splitEdgeQueries = splitEdgeQueries->append(path.edges[i]);
                }
                // Bounds get applied to the end edges
                for (var boundIndex, bound in bounds)
                {
                    const applyToStart = boundIndex == (definition.flipFlangeBounds ? 1 : 0);
                    const edgeIndex = applyToStart ? 0 : path.edges->size() - 1;
                    const alignedWithEdge = path.flipped[edgeIndex] != applyToStart;
                    const splitQueryAndLimitEntity = splitEdgeForPartialFlange(context, topLevelId, definition, operationId + unstableIdComponent(operationIndex),
                        path.edges[edgeIndex], [bound], alignedWithEdge, definition.holdAdjacentEdges, addManipulators);
                    if (splitQueryAndLimitEntity.limitEntities != undefined)
                    {
                        limitEntities[splitQueryAndLimitEntity.splitEdgeQuery] = splitQueryAndLimitEntity.limitEntities;
                    }
                    splitEdgeQueries = splitEdgeQueries->append(splitQueryAndLimitEntity.splitEdgeQuery);
                    operationIndex += 1;
                }
            }
            addManipulators = false;
        }
    }
    return {
            "splitEdgeQueries" : splitEdgeQueries,
            "limitEntities" : limitEntities
        };
}

