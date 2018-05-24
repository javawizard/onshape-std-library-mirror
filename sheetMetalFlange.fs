FeatureScript 834; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "834.0");
import(path : "onshape/std/boolean.fs", version : "834.0");
import(path : "onshape/std/containers.fs", version : "834.0");
import(path : "onshape/std/curveGeometry.fs", version : "834.0");
import(path : "onshape/std/extrude.fs", version : "834.0");
import(path : "onshape/std/evaluate.fs", version : "834.0");
import(path : "onshape/std/feature.fs", version : "834.0");
import(path : "onshape/std/math.fs", version : "834.0");
import(path : "onshape/std/matrix.fs", version : "834.0");
import(path : "onshape/std/query.fs", version : "834.0");
import(path : "onshape/std/sketch.fs", version : "834.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "834.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "834.0");
import(path : "onshape/std/smjointtype.gen.fs", version : "834.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "834.0");
import(path : "onshape/std/topologyUtils.fs", version : "834.0");
import(path : "onshape/std/units.fs", version : "834.0");
import(path : "onshape/std/valueBounds.fs", version : "834.0");
import(path : "onshape/std/vector.fs", version : "834.0");
import(path : "onshape/std/extendsheetboundingtype.gen.fs", version : "834.0");

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
    MIDDLE
}

const SURFACE_SUFFIX = "surface";

/**
* Create sheet metal flanges on selected edges of sheet metal parts. Length of flange may be
* defined by distance (as measured from the virtual sharp along outer edge of wall) or limiting
* entity. Bend angle may be flipped using the oppositeDirection flag. When auto-miter is not
* selected, flange sides are rotated by miter angle.
*/
annotation { "Feature Type Name" : "Flange",
             "Filter Selector" : "allparts",
             "Editing Logic Function" : "flangeEditLogic" }
export const sheetMetalFlange = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
precondition
{
    annotation { "Name" : "Edges or side faces to flange",
                 "Filter" : (SheetMetalDefinitionEntityType.EDGE && (GeometryType.LINE || GeometryType.PLANE))
                            && AllowFlattenedGeometry.YES && ModifiableEntityOnly.YES }
    definition.edges is Query;

    annotation {"Name" : "Flange alignment", "UIHint" : "SHOW_LABEL"}
    definition.flangeAlignment is SMFlangeAlignment;

    annotation { "Name" : "End type", "UIHint" : "SHOW_LABEL" }
    definition.limitType is SMFlangeBoundingType;

    if (definition.limitType == SMFlangeBoundingType.BLIND)
    {
        annotation { "Name" : "Distance"}
        isLength(definition.distance, NONNEGATIVE_LENGTH_BOUNDS);
    }
    else if (definition.limitType == SMFlangeBoundingType.UP_TO_ENTITY || definition.limitType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
    {
        annotation {"Name" : "Up to entity", "Filter" : EntityType.FACE || EntityType.VERTEX, "MaxNumberOfPicks" : 1}
        definition.limitEntity is Query;
        if (definition.limitType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
        {
            annotation { "Name" : "Offset"}
            isLength(definition.offset, NONNEGATIVE_LENGTH_BOUNDS);

            annotation { "Name" : "Opposite offset direction", "UIHint" : "OPPOSITE_DIRECTION" }
            definition.oppositeOffsetDirection is boolean;
        }
    }

    annotation { "Name" : "Angle control type" }
    definition.angleControlType is SMFlangeAngleControlType;

    annotation { "Name" : "Opposite side", "UIHint" : "OPPOSITE_DIRECTION" }
    definition.oppositeDirection is boolean;

    if (definition.angleControlType == SMFlangeAngleControlType.BEND_ANGLE)
    {
        annotation { "Name" : "Bend angle" }
        isAngle(definition.bendAngle, FLANGE_BEND_ANGLE_BOUNDS);
    }
    else if (definition.angleControlType == SMFlangeAngleControlType.ALIGN_GEOMETRY)
    {
        annotation { "Name" : "Parallel to", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION, "MaxNumberOfPicks" : 1 }
        definition.parallelEntity is Query;
    }
    else if (definition.angleControlType == SMFlangeAngleControlType.ANGLE_FROM_DIRECTION)
    {
        annotation { "Name" : "Direction", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION, "MaxNumberOfPicks" : 1 }
        definition.directionEntity is Query;

        annotation { "Name" : "Angle" }
        isAngle(definition.angleFromDirection, FLANGE_DIRECTION_ANGLE_BOUNDS);

        annotation { "Name" : "Opposite angle", "UIHint" : "OPPOSITE_DIRECTION" }
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
        isLength(definition.bendRadius, BLEND_BOUNDS);
    }
}
{
    // this is not necessary but helps with correct error reporting in feature pattern
    checkNotInFeaturePattern(context, definition.edges, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

    if (size(evaluateQuery(context, definition.edges)) == 0)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_NO_EDGES, ["edges"]);
    }

    var edges = qUnion(getSMDefinitionEntities(context, definition.edges));
    var nonLineEdgeQ = qSubtraction(qEntityFilter(edges, EntityType.EDGE), qGeometry(edges, GeometryType.LINE));
    if (size(evaluateQuery(context, nonLineEdgeQ)) != 0)
    {
        setErrorEntities(context, id, {"entities" : nonLineEdgeQ});
        edges = qGeometry(edges, GeometryType.LINE);
        if (size(evaluateQuery(context, edges)) != 0)
        {
            reportFeatureWarning(context, id , ErrorStringEnum.SHEET_METAL_FLANGE_NON_LINEAR_EDGES);
        }
        else
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_NON_LINEAR_EDGES, ["edges"]);
        }
    }
    var evaluatedEdgeQuery = evaluateQuery(context, edges);
    if (size(evaluatedEdgeQuery) == 0)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_EDGE_NEEDED, ["edges"]);
    }

    removeCornerBreaksAtEdgeVertices(context, edges);
    var edgeMaps = groupEdgesByBodyOrModel(context, evaluatedEdgeQuery);
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

    var objectCounter = 0; // counter for all sheet metal objects created. Guarantees unique attribute ids.
    var containerToLoop = isAtVersionOrLater(context, FeatureScriptVersionNumber.V483_FLAT_QUERY_EVAL_FIX) ? edgeMaps.bodyToEdgeMap : edgeMaps.modelToEdgeMap;
    for (var entry in containerToLoop)
    {
        objectCounter = updateSheetMetalModelForFlange(context, id, objectCounter, qUnion(entry.value), definition);
    }

    // Add association attributes where needed and compute deleted attributes
    var toUpdate = assignSMAttributesToNewOrSplitEntities(context, robustSMBodiesQ, initialData);
    updateSheetMetalGeometry(context, id, { "entities" : toUpdate.modifiedEntities,
                                           "deletedAttributes" : toUpdate.deletedAttributes});

}, { oppositeDirection : false, limitType : SMFlangeBoundingType.BLIND,  flangeAlignment : SMFlangeAlignment.INNER,
     angleControlType : SMFlangeAngleControlType.BEND_ANGLE, autoMiter : true, useDefaultRadius : true, oppositeOffsetDirection: false});


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

    return {"modelToEdgeMap" : modelToEdgeMap, "bodyToEdgeMap" : bodyToEdgeMap};
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
        definition.bendRadius =  modelParams.defaultBendRadius;
    }
    if (!specifiedParameters.offset)
        definition.offset = modelParams.minimalClearance;

    //make sure we're pointing in the direction of the limit entity
    if (isCreating && specifiedParameters.limitEntity && !specifiedParameters.oppositeDirection)
    {
        var flangeData = getFlangeData(context, id, evaluatedEdgeQuery[0], definition);
        const edgePoint = flangeData.edgeEndPoints[0].origin;
        var pointOnLimit = undefined;
        var planeResult = try silent(evPlane(context, {"face" : definition.limitEntity}));
        if (planeResult == undefined)
        {
            pointOnLimit = try silent(evVertexPoint(context, {"vertex" : definition.limitEntity}));
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
    return definition;
}

function trackAllFaces(context, allSurfaces, originals)
{
    var newSurfaces = qSubtraction(allSurfaces, originals);
    var trackedFaces = [];
    var trackedFacesNew = [];
    for (var face in evaluateQuery(context, qOwnedByBody(newSurfaces, EntityType.FACE)))
    {
        trackedFacesNew = append(trackedFacesNew, qUnion([face, startTracking(context, face)]));
    }
    trackedFaces = concatenateArrays([trackedFaces, trackedFacesNew]);
    for (var face in evaluateQuery(context, qOwnedByBody(originals, EntityType.FACE)))
    {
        trackedFaces = append(trackedFaces, qUnion([face, startTracking(context, face)]));
    }
    return {"allFaces" : trackedFaces, "newFaces" : trackedFacesNew};
}

function updateSheetMetalModelForFlange(context is Context, topLevelId is Id, objectCounter is number, edges is Query, definition is map) returns number
{
    var evaluatedEdgeQuery = evaluateQuery(context, edges);

    // add thickness, minimalClearance and defaultBendRadius to definition.
    // Flange uses thickness, minimalClearance and potentially defaultBendRadius derived from underlying sheet metal model
    definition = mergeMaps(definition, getModelParametersFromEdge(context, evaluatedEdgeQuery[0]));
    if (definition.useDefaultRadius)
    {
        definition.bendRadius = definition.defaultBendRadius;
    }
    definition.inFlangeThickness = inFlangeThickness(definition);

    // Collect flangeData for each edge, and store a mapping from each edge to a tracking query of itself
    var edgeToFlangeData = {};
    var oldEdgeToNewEdge = {};
    var originalFlangeEdges = evaluateQuery(context, edges);
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
        originalCornerVertices, edgeToFlangeData, definition);
    var edgeToFlangeDistance = collectEdgeToFlangeDistance(context, topLevelId, edges, edgeToFlangeData, edgeToSideAndBase, definition);

    // Sketch each flange and add it to the underlying sheet body
    var surfaceBodies = [];
    var originalEntities = [];
    var trackingBendEdges = [];
    var setBendAttributesAfterBoolean = isAtVersionOrLater(context, FeatureScriptVersionNumber.V695_SM_SWEPT_SUPPORT);
    for (var edge in evaluateQuery(context,edges))
    {
        var ownerBody = qOwnerBody(edge);
        surfaceBodies = append(surfaceBodies, ownerBody);
        originalEntities = append(originalEntities, qSubtraction(qOwnedByBody(ownerBody), modifiedEntities));
        var indexedId = topLevelId + unstableIdComponent(objectCounter);
        if (definition.useExternalDisambiguation)
        {
            setExternalDisambiguation(context, indexedId, edge);
        }
        var surfaceId = indexedId + SURFACE_SUFFIX;

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
    const nOriginalBodies = size(evaluateQuery(context, originalBodies));
    var indexedId = topLevelId + unstableIdComponent(objectCounter);

    var allSurfaces = qUnion(surfaceBodies);
    var trackedFaces = trackAllFaces(context, allSurfaces, originalBodies);
    var booleanId = indexedId + ("flange_boolean");
    try
    {
        opBoolean(context, booleanId, {
            "allowSheets" : true,
            "tools" : allSurfaces,
            "operationType" : BooleanOperationType.UNION,
            "makeSolid" : false,
            "eraseImprintedEdges" : false
        });
    }
    catch
    {
        // Error display
        processSubfeatureStatus(context, topLevelId, {"subfeatureId" : booleanId, "propagateErrorDisplay" : true});
        setErrorEntities(context, topLevelId, { "entities" : qSubtraction(allSurfaces, originalBodies) });
        //cleanup of all new surfaces should happen in abortFeature
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL, ["edges"]);
    }

    // we could check for no-op info here but it won't catch cases where some flanges could be joined and others couldnt
    // also check if boolean created an extra body trying to avoid non-manifold geometry
    if (size(evaluateQuery(context, allSurfaces)) != nOriginalBodies || size(evaluateQuery(context, qCreatedBy(booleanId, EntityType.BODY))) > 0)
    {
        setErrorEntities(context, (definition.useExternalDisambiguation) ? topLevelId : indexedId, { "entities" : allSurfaces });
        //cleanup of all new surfaces should happen in abortFeature
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL, ["edges"]);
    }

    //check that none got split
    for (var trackedFace in trackedFaces.allFaces)
    {
        var evaluatedFaces = evaluateQuery(context, qEntityFilter(trackedFace, EntityType.FACE));
        if (size(evaluatedFaces) > 1)
        {
            var newFaces = qEntityFilter(qUnion(trackedFaces.newFaces), EntityType.FACE);
            var newEdges = qEdgeAdjacent(qUnion(trackedFaces.newFaces), EntityType.EDGE);
            setErrorEntities(context, topLevelId, { "entities" : qUnion([newFaces, newEdges])});
            throw regenError(ErrorStringEnum.SHEET_METAL_SELF_INTERSECTING_MODEL, ["edges"]);
        }
    }

    for (var bendEdge in trackingBendEdges)
    {
        var bendAttribute = createBendAttribute(context, topLevelId, bendEdge,
                                                toAttributeId(topLevelId + objectCounter), definition.bendRadius, false);
        if (bendAttribute != undefined)
        {
            setAttribute(context, {"entities" : bendEdge, "attribute" : bendAttribute});
            objectCounter += 1;
        }
    }

    //add rips to new interior edges
    for (var entity in evaluateQuery(context, qOwnedByBody(allSurfaces, EntityType.EDGE)))
    {
        var attributes = getAttributes(context, {"entities" : entity, "attributePattern" : asSMAttribute({})});
        if (size(attributes) == 0)
        {
            var jointAttribute = makeNewJointAttributeIfNeeded(context, entity,  toAttributeId(topLevelId + objectCounter));
            if (jointAttribute != undefined)
            {
                setAttribute(context, {"entities" : entity, "attribute" : jointAttribute});
                objectCounter += 1;
            }
        }
    }

    return objectCounter;
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
        if (size(newEdge) == 0)
        {
            const errorFace = edgeToFlangeData[e].adjacentFace;
            setErrorEntities(context, topLevelId, { "entities" : qUnion([errorFace, qEdgeAdjacent(errorFace, EntityType.EDGE)]) });
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
        processSubfeatureStatus(context, topLevelId, {"subfeatureId" : edgeChangeId, "propagateErrorDisplay" : true});
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
        processSubfeatureStatus(context, topLevelId, {"subfeatureId" : extendSheetBodyId, "propagateErrorDisplay" : true});
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
            processSubfeatureStatus(context, topLevelId, {"subfeatureId" : extendIndexedId, "propagateErrorDisplay" : true});
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
    setErrorEntities(context, (useExternalDisambiguation) ? topLevelId : id, { "entities" : qEdgeAdjacent(edges, EntityType.FACE) });
    throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_ALIGNMENT, ["flangeAlignment"]);
}

function failDueToAlignmentIssue(context is Context, topLevelId is Id, edges is Query)
{
    failDueToAlignmentIssue(context, topLevelId, newId(), true, edges);
}

function trackCornerVertices(context is Context, edges is Query) returns array
{
    var originalCornerVertices = [];
    var vertices = evaluateQuery(context, qVertexAdjacent(edges, EntityType.VERTEX));
    for (var v in vertices)
    {
        var adjacentEdges = evaluateQuery(context, qVertexAdjacent(v, EntityType.EDGE));
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

function getSideAndBase(context is Context, topLevelId is Id, edge is Query, definition is map, edgeToFlangeData is map, cornerVertices is array) returns map
{
    var edgeVertices = getOrderedEdgeVertices(context, edge);
    var flangeData = edgeToFlangeData[edge];
    if (flangeData == undefined)
    {
        throw "Could not find flange data for edge";
    }
    var flangeSideDirs = [flangeData.direction, flangeData.direction];
    var flangeBasePoints = [edgeVertices.points[0].origin, edgeVertices.points[1].origin];
    for (var i = 0; i < size(edgeVertices.vertices); i += 1 )
    {
        var vertexData = getVertexData(context, topLevelId, edge, edgeVertices.vertices[i], edgeToFlangeData, definition, i, cornerVertices);
        if (vertexData != undefined)
        {
            flangeSideDirs[i] = vertexData.flangeSideDir;
            flangeBasePoints[i] = vertexData.flangeBasePoint;
        }
    }
    return {"sideDirections" : flangeSideDirs, "basePoints" : flangeBasePoints};
}

function collectEdgeToSideAndBase(context is Context, topLevelId is Id, useExternalDisambiguation is boolean, edges is Query,
        originalCornerVertices is array, edgeToFlangeData is map, definition is map) returns map
{
    var edgeToSideAndBase = {};
    for (var edge in evaluateQuery(context, edges))
    {
        try
        {
            edgeToSideAndBase[edge] = getSideAndBase(context, topLevelId, edge, definition, edgeToFlangeData, originalCornerVertices);
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

function getFlangeBasePoint(context is Context, flangeEdge is Query, sideEdge is Query, definition is map,
    flangeData is map, vertexPoint is Vector, needsTrimChanges is boolean, sidePlane is Plane, clearance is ValueWithUnits)
{
    var ignoreSideEdge = isAtVersionOrLater(context, FeatureScriptVersionNumber.V526_FLANGE_SIDE_PLANE_DIR) &&
                        size(evaluateQuery(context, sideEdge)) == 0;
    var sideEdgeIsBend = false;
    var jointAttribute;
    if (!ignoreSideEdge)
    {
        jointAttribute = try silent(getJointAttribute(context, sideEdge));
        if (jointAttribute == undefined)
            return vertexPoint;
        sideEdgeIsBend = (jointAttribute.jointType.value == SMJointType.BEND);
    }
    var edgeLine = evLine(context, {"edge" : flangeEdge});
    var offsetFromClearance = getOffsetForClearance(context , sidePlane, clearance, definition, flangeData.plane);
    if (!sideEdgeIsBend)
    {
        //use the minimal clearance to shift by
        return computeBaseFromShiftedPlane(context, offsetFromClearance, sidePlane, edgeLine);
    }

    var edgeBendRadius = jointAttribute.radius.value;
    var edgeBendAngle = jointAttribute.angle.value;

    //find direction of side edge on the adjacent plane
    const sideEdgeMidPt = evEdgeTangentLines(context, { "edge" : sideEdge, "parameters" : [0.5] , "face": flangeData.adjacentFace});
    var adjacentPlane = evPlane(context, {"face" : flangeData.adjacentFace});
    var thickness = 0;
    var convexity = evEdgeConvexity(context, {"edge" : sideEdge});
    if (convexity == EdgeConvexityType.CONVEX)
       thickness = definition.backThickness;
    else if (convexity == EdgeConvexityType.CONCAVE)
       thickness = definition.frontThickness;

    var offset = (thickness + edgeBendRadius) * tan(.5 * edgeBendAngle);
    // "move" side edge towards the inside of adjacent plane by offset (based on side edge bend info)
    var directionToMoveEdgeBy = cross(adjacentPlane.normal, sideEdgeMidPt[0].direction);
    var planeFromSideEdge = plane(vertexPoint + directionToMoveEdgeBy * offset, directionToMoveEdgeBy);

    //find flange edge direction on adjacent plane
    const flangeEdgeMidPt = evEdgeTangentLines(context, {"edge": flangeEdge, "parameters" : [0.5], "face": flangeData.adjacentFace});
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

        if ( isAtVersionOrLater(context, FeatureScriptVersionNumber.V493_FLANGE_BASE_SHIFT_FIX) && edgeBendAngle < 90 * degree)
        {
            //just use the original planeFromSideEdge for base shift
            return computeBaseFromShiftedPlane(context, 0.0 * meter, planeFromSideEdge, edgeLine);
        }
        var lineFromBentEdge = line(vertexPoint + flangeData.direction * offset, flangeEdgeMidPt[0].direction);
        var updatedProjection = project(lineFromBentEdge, intersectionData.intersection);
        var offsetFromBend = evDistance(context, {"side0" : updatedProjection, "side1": project(sidePlane, updatedProjection)}).distance;
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

function getOrderedEdgeVertices(context is Context, edge is Query) returns map
{
    var edgeVertices = evaluateQuery(context, qVertexAdjacent(edge, EntityType.VERTEX));
    if (size(edgeVertices) != 2)
        throw "Edge to flange has wrong number of vertices";

    var p0 = evVertexPoint(context, {"vertex" : edgeVertices[0]});

    const adjacentFace = qEdgeAdjacent(edge, EntityType.FACE);
    if (size(evaluateQuery(context, adjacentFace)) != 1)
    {
        throw "Edge to flange is not laminar";
    }
    const edgeEndPoints = evEdgeTangentLines(context, { "edge" : edge, "parameters" : [0, 1] , "face": adjacentFace});
    if (!tolerantEquals(edgeEndPoints[0].origin, p0))
    {
        return { "vertices" : reverse(edgeVertices), "points" : edgeEndPoints };
    }
    return { "vertices" : edgeVertices, "points" :edgeEndPoints };
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

function filterSmoothEdges(context is Context, inputEdges is Query) returns array
{
    var evaluatedInputEdges = evaluateQuery(context, inputEdges);
    var resultingEdges = filter(evaluatedInputEdges, function(edge){
        var convexity = try silent(evEdgeConvexity(context, {"edge" : edge}));
        return (convexity != EdgeConvexityType.SMOOTH);
    });
    return resultingEdges;
}

function getXYAtVertex(context is Context, vertex is Query, edge is Query, edgeToFlangeData is map)
{
    var vertexToUse = vertex;
    var vertexEdges = qSubtraction(qVertexAdjacent(vertex, EntityType.EDGE), edge);
    var vertexEdgesArray = filterSmoothEdges(context, vertexEdges);
    if (size(vertexEdgesArray) == 1 && edgeToFlangeData[vertexEdgesArray[0]] != undefined)
    {
        // There is only one non-smooth edge besides `edge` extending from `vertex`.  This edge is also part of this
        // flange operation, so it must be laminar and linear.  Due to these constraints, the edge must lie on the
        // same face as `edge`, and therefore qualifies as `edgeX`
        return {"edgeX" : vertexEdgesArray[0], "position" : evVertexPoint(context, {"vertex" : vertexToUse}) };
    }

    vertexEdges = qUnion(vertexEdgesArray);
    var flangeAdjacentFace = edgeToFlangeData[edge].adjacentFace;
    //sideEdge(edgeX) will be the edge shared by vertexEdgesExcludingEdge and flangeAdjacentFace
    var edgeX = qIntersection([vertexEdges, qEdgeAdjacent(flangeAdjacentFace, EntityType.EDGE)]);

    //sideFace is adjacent to flangeAdjacentFace, and edgeX
    var sideFace =  qSubtraction(qEdgeAdjacent(edgeX, EntityType.FACE), flangeAdjacentFace);

    //edgeY is the other edge on sideFace that is also adjacent to vertex. Often this will be a laminar. but not necessarily
    // e.g. if the edgeY is a bendEdge of a flange with angled miter.
    var edgeY = qSubtraction(qIntersection([qEdgeAdjacent(sideFace, EntityType.EDGE), vertexEdges]), edgeX);

    var failIfNotLines = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V695_SM_SWEPT_SUPPORT);
    var lineX;
    if (size(evaluateQuery(context, edgeY)) != 0)
    {
        //if edgeY is collinear with edgeX,look for next edge on sideFace
        var line1 = (failIfNotLines) ? evLine(context, {"edge" : edgeX}) : try silent(evLine(context, {"edge" : edgeX}));
        var line2 = (failIfNotLines) ? evLine(context, {"edge" : edgeY}) : try silent(evLine(context, {"edge" : edgeY}));

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

        if (line1 != undefined && line2 != undefined && tolerantCoLinear(line1, line2, !isAtVersionOrLater(context, FeatureScriptVersionNumber.V649_FLANGE_LOOSEN_EDGE_Y)))
        {
            edgeY = qIntersection([qEdgeAdjacent(sideFace, EntityType.EDGE), qSubtraction(qVertexAdjacent(edgeY, EntityType.EDGE), edgeX)]);
        }
    }
    else
    {
        var lineOrigX = try silent(evLine(context, {"edge" : edgeX}));
        if (lineOrigX == undefined)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V489_FLANGE_BUGS))
            {
                //flange is adjacent to a non-linear edge, move along.
                return {"edgeX" : edgeX, "position" : evVertexPoint(context, {"vertex" : vertex}) };
            }
            return undefined;
        }
        vertexToUse = qSubtraction(qVertexAdjacent(edgeX, EntityType.VERTEX), vertex);
        vertexEdges = qSubtraction(qVertexAdjacent(vertexToUse, EntityType.EDGE), edgeX);
        vertexEdgesArray = filterSmoothEdges(context, vertexEdges);
        if (size(vertexEdgesArray) == 1)
        {
            return {"edgeX" : vertexEdgesArray[0], "position" : evVertexPoint(context, {"vertex" : vertex}) };
        }
        vertexEdges = qUnion(vertexEdgesArray);
        //find Edge among vertexEdges also adjacent to adjacentFace
        edgeX = qIntersection([vertexEdges, qEdgeAdjacent(flangeAdjacentFace, EntityType.EDGE)]);
        //check for sanity that the newly found edgeX is collinear with the one we found initially
        var lineNewX = isAtVersionOrLater(context, FeatureScriptVersionNumber.V714_SM_BEND_DETERMINISM) ?
                            try silent(evLine(context, {"edge" : edgeX})) : evLine(context, {"edge" : edgeX});
        if (lineNewX == undefined || !tolerantCoLinear(lineOrigX, lineNewX, !isAtVersionOrLater(context, FeatureScriptVersionNumber.V493_FLANGE_BASE_SHIFT_FIX)))
            return undefined;
        sideFace =  qSubtraction(qEdgeAdjacent(edgeX, EntityType.FACE), flangeAdjacentFace);
        edgeY = qSubtraction(qIntersection([qEdgeAdjacent(sideFace, EntityType.EDGE), vertexEdges]), edgeX);
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
             "position" : evVertexPoint(context, {"vertex" : vertexToUse})};
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
    // if sideEdge is a bend, don't miter if flaps are on the "outside"
    if (sideEdgeIsBend)
    {
        var convexity = evEdgeConvexity(context, {"edge" : sideEdge});
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

function representInVectors(vector1 is Vector, vector2 is Vector, inVector is Vector)
{
    var matrixM is Matrix = [[squaredNorm(vector1), dot(vector2, vector1)],
                             [dot(vector1, vector2), squaredNorm(vector2)]] as Matrix;
    var rhs = vector(dot(inVector, vector1), dot(inVector, vector2));

    var components = inverse(matrixM) * rhs;
    return components;
}

function isInProblemHalfSpace(context is Context, flangeDir is Vector, position is Vector, edgeY, sideEdge is Query) returns boolean
{
    if (edgeY == undefined || sideEdge == undefined)
        return false;
    var components = representInVectors(getVectorForEdge(context, edgeY, position), getVectorForEdge(context, sideEdge, position), flangeDir);
    return (components[0] > TOLERANCE.zeroLength);
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

function getVertexData(context is Context, topLevelId is Id, edge is Query, vertex is Query, edgeToFlangeData is map, definition is map, i is number, cornerVertices) returns map
{
    var position = evVertexPoint(context, {"vertex" : vertex});
    var result = {
        "flangeBasePoint" : position,
        "flangeSideDir" : undefined
    };

    var needsSideDirUpdate = false;
    var flangeData = edgeToFlangeData[edge];
    var vertexAndEdges = getXYAtVertex(context, vertex, edge, edgeToFlangeData);
    if (vertexAndEdges == undefined || (isIn(vertex, cornerVertices) && vertexAndEdges.edgeY == undefined ))
    {
        if (!definition.autoMiter)
        {
            var computeMiter = isAtVersionOrLater(context, FeatureScriptVersionNumber.V569_FLANGE_NEXT_TO_RIP);
            if (!computeMiter)
            {
               var vertexEdges = evaluateQuery(context, qSubtraction(qVertexAdjacent(vertex, EntityType.EDGE), edge));
               computeMiter = (size(vertexEdges) == 1);
            }
            if (computeMiter)
            {
                var sidePlane = plane(position, flangeData.edgeEndPoints[i].direction);
                sidePlane = createPlaneForManualMiter(flangeData, sidePlane, position, i, definition.adjustedMiterAngle);
                result.flangeSideDir = getFlangeSideDir(flangeData, sidePlane, i, false, definition);
            }
        }
        return  result;
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

        if ( dot(sidePlane.normal, edgeEndDirection) > 0)
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
            var components = representInVectors(getVectorForEdge(context, edgeY, position), getVectorForEdge(context, sideEdge, position), flangeData.direction);
            if (alignFlippedFlange)
            {
                needsBaseUpdate = true;
            }
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
        var vertexPositionToUse = needsTrimChanges ?  vertexAndEdges.position : position;
        result.flangeBasePoint = getFlangeBasePoint(context, edge, useAsSideEdge, definition, flangeData, vertexPositionToUse, needsTrimChanges,
                                sidePlane, clearanceFromSide);
    }
    if (needsSideDirUpdate) // need a trim on the flange sides by a plane
    {
        result.flangeSideDir = getFlangeSideDir(flangeData, sidePlane, i, autoMitered, definition);
    }
    return result;
}

function getFlangeSideDir(flangeData is map, sidePlane is Plane, i is number, autoMitered is boolean, definition is map)
{
    var intersectionDirection = normalize(cross(flangeData.plane.normal, sidePlane.normal));
    var isOppositeFlangedir = dot(flangeData.direction, intersectionDirection) < 0;
    var flangeSideDirection = isOppositeFlangedir ? -1 * intersectionDirection : intersectionDirection;
    var edgeDirection =  (i == 0) ? flangeData.edgeEndPoints[i].direction : -1 * flangeData.edgeEndPoints[i].direction;

    //if angle between edge and computed side dir is > 90 then flange will add material
    var edgeToComputedSideDirAngle = angleBetween(edgeDirection, flangeSideDirection);

    var miterAddsMaterial = !definition.autoMiter && (definition.miterAngle - 90 * degree) > TOLERANCE.zeroAngle * degree;

    if (autoMitered || miterAddsMaterial ||  edgeToComputedSideDirAngle < (90 + TOLERANCE.zeroAngle) * degree)
        return flangeSideDirection;
    else //make sure we don't add material when we shouldn't
        return flangeData.direction;
}

function addBendAttribute(context is Context, edge is Query, flangeData is map, topLevelId is Id, index is number, definition is map)
{
    var attributeId = toAttributeId(topLevelId + index);
    var bendAttribute = makeSMJointAttribute(attributeId);
    bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited": true };
    bendAttribute.radius = {
        "value" : definition.bendRadius,
        "canBeEdited" : true
    };
    bendAttribute.angle = {
        "value" : flangeData.bendAngle,
        "canBeEdited" : true
    };
    setAttribute(context, {"entities" : edge, "attribute" : bendAttribute});
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
    for (var i = 0; i < 2; i +=1)
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
    var adjacentFaces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
    if (size(adjacentFaces) != 1)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_INTERNAL);
    }
    const adjacentFace = adjacentFaces[0];
    const edgeEndPoints = evEdgeTangentLines(context, { "edge" : edge, "parameters" : [0, 1] , "face": adjacentFace});
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
        alignedDistance = definition.distance  - thickness * tan(.5 * bendAngle);
    }

    return {
        "bendAngle" : bendAngle,
        "alignedDistance" : alignedDistance,
        "direction" : direction,
        "plane" : sketchPlane,
        "wallExtendDirection" : cross(edgeEndPoints[0].direction, sidePlane.normal),
        "wallPlane" : sidePlane,
        "edgeEndPoints" : edgeEndPoints,
        "adjacentFace" : qUnion([adjacentFace, startTracking(context, adjacentFace)])
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
    flangeData.plane = getExtendedFlangePlane(flangeData, extensionDistance);
    flangeData.edgeEndPoints = evEdgeTangentLines(context, { "edge" : edge, "parameters" : [0, 1] , "face": flangeData.adjacentFace});

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

    var entity = evaluateQuery(context, definition.limitEntity);
    if (size(entity) < 1)
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_UP_TO_ENTITY, ["limitEntity"]);

    //see if it's a plane:
    var planeResult = try silent(evPlane(context, {"face" : entity[0]}));
    if (planeResult == undefined)
    {   //see if it's an vertex
        var limitVertex = try silent(evVertexPoint(context, {"vertex" : entity[0]}));
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
    var  minDelta = 0.0 * meter;
    if (definition.limitType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
    {
        minDelta += definition.offset;
    }

    var withPlaneNormal = isAtVersionOrLater(context, FeatureScriptVersionNumber.V629_SM_MODEL_FRONT_N_BACK) ?
                            dot(flangeDirection, planeResult.normal) < 0 : true;
    planeResult = movePlaneForFlangeClearance(flangePlane, planeResult, withPlaneNormal , thickness, minDelta);
    return planeResult;
}

/*
 * During trimming or extending a flange up to an entity, make sure that the minimum distance between the
 * thickened flange and the target plane equals minDelta. This function computes the distance the otherPlane needs
 * to be moved depending on angle between flangePlane and otherPlane.
 */
function movePlaneForFlangeClearance(flangePlane is Plane, otherPlane is Plane, withPlaneNormal is boolean, thickness, minDelta) returns Plane
{
    //angle between flange plane and side plane
    var angleClearance = thickness * abs(dot(flangePlane.normal, otherPlane.normal));
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
    var points = [  worldToPlane(sketchPlane, basePoints[0]),
                    worldToPlane(sketchPlane, basePoints[1]),
                    worldToPlane(sketchPlane, basePoints[1] + offsets[1]),
                    worldToPlane(sketchPlane, basePoints[0] + offsets[0]) ];
    points = append(points, points[0]);

    var sketch = newSketchOnPlane(context, id, {"sketchPlane" : sketchPlane});
    skPolyline(sketch, "polyline", {"points" : points });
    skSolve(sketch);

    var regions = evaluateQuery(context, qCreatedBy(id, EntityType.FACE));
    if (size(regions) != 1)
    {
        //This can happen in up-to-vertex flanges with auto-miter. (creating hour glass shape)
        setErrorEntities(context, topLevelId, { "entities" : qCreatedBy(id, EntityType.FACE) });
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL);
    }
}

function createFlangeSurfaceReturnBendEdge(context is Context, topLevelId is Id,  indexedId is Id, edge is Query,
        flangeData is map, sideAndBase is map, flangeDistance, definition is map) returns Query
{
    var sketchId = indexedId + "sketch";
    createAndSolveSketch(context, topLevelId, sketchId, edge, flangeData, sideAndBase, flangeDistance, definition);
    var bendLine = startTracking(context, sketchId, "polyline.line0");
    opExtractSurface(context, indexedId + SURFACE_SUFFIX, {"faces" : qSketchRegion(sketchId, false)});

    opDeleteBodies(context, indexedId + "delete_sketch", { "entities" : qCreatedBy(sketchId, EntityType.BODY) });

    return qIntersection([qCreatedBy(indexedId + SURFACE_SUFFIX, EntityType.EDGE), bendLine]);
}

function getModelParametersFromEdge(context is Context, edge is Query) returns map
{
    var adjacentFace = qEdgeAdjacent(edge, EntityType.FACE);
    if (size(evaluateQuery(context, adjacentFace)) != 1)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_INTERNAL);
    }
    return getModelParameters(context, qOwnerBody(adjacentFace));
}


function tolerantParallel(direction0 is Vector, direction1 is Vector, stricter is boolean) returns boolean
{
    var limit = stricter ? (TOLERANCE.zeroAngle * TOLERANCE.zeroAngle) : (TOLERANCE.g1Angle * TOLERANCE.g1Angle);
    return squaredNorm(cross(direction0, direction1)) < limit;
}

function tolerantCoLinear(line0 is Line, line1 is Line, stricter is boolean) returns boolean
{
    if (tolerantParallel(line0.direction, line1.direction, stricter)) {
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

        return {"parallelDirection" : direction};
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
                        coplanarPlanes(parentFlangeData.wallPlane, currFlangeData.wallPlane) :
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
    var edgeGroups = connectedComponentsOfEdges(context, edges);
    for (var group in edgeGroups)
    {
        var queue = [{"edge" : group[0]}];
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
                    var adjacentEdges = qVertexAdjacent(currEdge, EntityType.EDGE);
                    var edgesToProcess = qSubtraction(qIntersection([adjacentEdges, qUnion(group)]), qUnion(keys(edgeToData)));
                    for (var edgeToProcess in evaluateQuery(context, edgesToProcess))
                    {
                        queue = append(queue, {"edge" : edgeToProcess, "parent" : currEdge});
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
                queue = append(queue, {"edge" : anomalousQueue[anomalousQueueIndex]});
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
        return evPlane(context, {"face" : face});
    }
}

