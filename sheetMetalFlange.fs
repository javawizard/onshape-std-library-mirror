FeatureScript 559; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "559.0");
import(path : "onshape/std/boolean.fs", version : "559.0");
import(path : "onshape/std/containers.fs", version : "559.0");
import(path : "onshape/std/curveGeometry.fs", version : "559.0");
import(path : "onshape/std/extrude.fs", version : "559.0");
import(path : "onshape/std/evaluate.fs", version : "559.0");
import(path : "onshape/std/feature.fs", version : "559.0");
import(path : "onshape/std/math.fs", version : "559.0");
import(path : "onshape/std/matrix.fs", version : "559.0");
import(path : "onshape/std/query.fs", version : "559.0");
import(path : "onshape/std/sketch.fs", version : "559.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "559.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "559.0");
import(path : "onshape/std/smjointtype.gen.fs", version : "559.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "559.0");
import(path : "onshape/std/topologyUtils.fs", version : "559.0");
import(path : "onshape/std/units.fs", version : "559.0");
import(path : "onshape/std/valueBounds.fs", version : "559.0");
import(path : "onshape/std/vector.fs", version : "559.0");
import(path : "onshape/std/extendsheetboundingtype.gen.fs", version : "559.0");


const FLANGE_ANGLE_BOUNDS =
{
    (degree) : [1, 90, 179],
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
    annotation { "Name" : "Edges to flange", "Filter" : (EntityType.EDGE && GeometryType.LINE && SketchObject.NO) && BodyType.SOLID && AllowFlattenedGeometry.YES}
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

    annotation { "Name" : "Bend angle" }
    isAngle(definition.bendAngle, FLANGE_ANGLE_BOUNDS);

    annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
    definition.oppositeDirection is boolean;
}
{
    //this is not necessary but helps with corrrect error reporting in feature pattern
    checkNotInFeaturePattern(context, definition.edges, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

    var edges = qUnion(getSMDefinitionEntities(context, definition.edges));
    var evaluatedEdgeQuery = evaluateQuery(context, edges);
    if (size(evaluatedEdgeQuery) == 0)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_EDGE_NEEDED, ["edges"]);
    }
    var edgeMaps = groupEdgesByBodyOrModel(context, evaluatedEdgeQuery);
    var modelToEdgeMap = edgeMaps.modelToEdgeMap;
    if (size(modelToEdgeMap) > 1 && definition.useDefaultRadius)
        throw regenError(ErrorStringEnum.SHEET_METAL_MULTI_SM_DEFAULT_RADIUS, ["useDefaultRadius"]);
    if (definition.oppositeOffsetDirection)
        definition.offset *= -1;

    //get originals before any changes
    var smBodies = evaluateQuery(context, qOwnerBody(edges));
    var smBodiesQ = qUnion(smBodies);
    var initialAssociationAttributes = getAttributes(context, {
            "entities" : qOwnedByBody(smBodiesQ),
            "attributePattern" : {} as SMAssociationAttribute
    });
    var allOriginalEntities = evaluateQuery(context, qOwnedByBody(smBodiesQ));

    var objectCounter = 0; // counter for all sheet metal objects created. Guarantees unique attribute ids.
    var containerToLoop = isAtVersionOrLater(context, FeatureScriptVersionNumber.V483_FLAT_QUERY_EVAL_FIX) ? edgeMaps.bodyToEdgeMap : edgeMaps.modelToEdgeMap;
    for (var entry in containerToLoop)
    {
        objectCounter = updateSheetMetalModelForFlange(context, id, objectCounter, qUnion(entry.value), definition);
    }

    // Add association attributes where needed and compute deleted attributes
    var toUpdate = assignSMAttributesToNewOrSplitEntities(context, smBodiesQ, allOriginalEntities, initialAssociationAttributes);
    updateSheetMetalGeometry(context, id, { "entities" : toUpdate.modifiedEntities,
                                           "deletedAttributes" : toUpdate.deletedAttributes});

}, { oppositeDirection : false, limitType : SMFlangeBoundingType.BLIND,  flangeAlignment : SMFlangeAlignment.INNER,
     autoMiter : true, useDefaultRadius : true, oppositeOffsetDirection: false});


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
    var edges = try silent(qUnion(getSMDefinitionEntities(context, definition.edges)));
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
        var flangeData = getFlangeData(context, evaluatedEdgeQuery[0], definition);
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

function updateSheetMetalModelForFlange(context is Context, topLevelId is Id,  objectCounter is number, edges is Query, definition is map) returns number
{
    var evaluatedEdgeQuery = evaluateQuery(context, edges);

    // add thickness, minimalClearance and defaultBendRadius to definition.
    // Flange uses thickness, minimalClearance and potentially defaultBendRadius derived from underlying sheet metal model
    definition = mergeMaps(definition, getModelParametersFromEdge(context, evaluatedEdgeQuery[0]));
    if (definition.useDefaultRadius)
    {
        definition.bendRadius = definition.defaultBendRadius;
    }

    var alignmentChanges = changeUnderlyingSheetForAlignment(context, topLevelId, topLevelId + unstableIdComponent(objectCounter), definition, edges);
    var originalCornerVertices = alignmentChanges.cornerVertices;
    var modifiedEntities = alignmentChanges.modifiedEntities;
    edges = alignmentChanges.updatedEdges;

    var edgeToFlangeData = {};
    for (var edge in evaluateQuery(context,edges))
    {
        edgeToFlangeData[edge] = getFlangeData(context, edge, definition);
    }

    var useExternalDisambiguation = isAtVersionOrLater(context, FeatureScriptVersionNumber.V500_EXTERNAL_DISAMBIGUATION);

    var surfaceBodies = [];
    var originalEntities = [];
    for (var edge in evaluateQuery(context,edges))
    {
        var ownerBody = qOwnerBody(edge);
        surfaceBodies = append(surfaceBodies, ownerBody);
        originalEntities = append(originalEntities, qSubtraction(qOwnedByBody(ownerBody), modifiedEntities));
        var indexedId = topLevelId + unstableIdComponent(objectCounter);
        if (useExternalDisambiguation)
        {
            setExternalDisambiguation(context, indexedId, edge);
        }
        var surfaceId = indexedId + SURFACE_SUFFIX;

        var updatedDefinition = try(updateDefinition(context, edge, definition, edgeToFlangeData, originalCornerVertices));
        if (updatedDefinition == undefined)
        {
            setErrorEntities(context, (useExternalDisambiguation) ? topLevelId : indexedId, { "entities" : edge });
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL, ["edges"]);
        }
        var bendEdge = createFlangeSurfaceReturnBendEdge(context, topLevelId, indexedId, edge, updatedDefinition);
        addBendAttribute(context, bendEdge, topLevelId, objectCounter, definition);
        objectCounter += 1;
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
        setErrorEntities(context, (useExternalDisambiguation) ? topLevelId : indexedId, { "entities" : allSurfaces });
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

    //add rips to new interior edges
    for (var entity in evaluateQuery(context, qOwnedByBody(allSurfaces, EntityType.EDGE)))
    {
        var attributes = getAttributes(context, {"entities" : entity, "attributePattern" : asSMAttribute({})});
        if (size(attributes) == 0 && edgeIsTwoSided(context, entity))
        {
            setAttribute(context, {"entities" : entity,
                "attribute" : createRipAttribute(context, entity, toAttributeId(topLevelId + objectCounter), SMJointStyle.EDGE, undefined)});
            objectCounter += 1;
        }
    }

    return objectCounter;
}

/**
 * Depending on whether we want to align the flange to the inner or outer wall face, we will need to move
 * the edge selected for flange, therefore changing the underlying sheet face.
 * Returns a map of results:
 *  updatedEdges : list of edges that got moved, or original edge list if not.
 *  modifiedEntities : all changed entities so that association attributes can be added correctly.
 *  cornerVertices : we keep track of all vertices that started as a laminar corner, as those do not
 *                   need tweaking to find corresponding vertices after alignment.
 */
function changeUnderlyingSheetForAlignment(context is Context, topLevelId is Id, id is Id, definition is map, edges is Query)
{
    var modifiedEntities = qNothing();
    var originalCornerVertices = trackCornerVertices(context, edges);

    var result = { "updatedEdges" : edges,
                   "modifiedEntities" : modifiedEntities,
                   "cornerVertices" : evaluateQuery(context, qUnion(originalCornerVertices))};

    if (definition.flangeAlignment == SMFlangeAlignment.MIDDLE)
        return result;


    var extendDistance = .5 * definition.thickness * (1 - cos(definition.bendAngle));
    var flipForAlignment = definition.flangeAlignment == SMFlangeAlignment.OUTER ? -1 : 1;
    var flipForDirection = definition.oppositeDirection ? -1 : 1;
    extendDistance = flipForAlignment * flipForDirection * extendDistance;

    var changedEntities = [];
    var index = 0;
    var oldEdgeToNewEdgeMap = {};
    var originalFlangeEdges = evaluateQuery(context, edges);

    // populate the map
    for (var edge in originalFlangeEdges)
    {
        oldEdgeToNewEdgeMap[edge] = qUnion([edge, startTracking(context, edge)]);
    }

    var reportErrorToTopLevel = isAtVersionOrLater(context, FeatureScriptVersionNumber.V500_EXTERNAL_DISAMBIGUATION);
    for (var edge in originalFlangeEdges)
    {
        var evaluatedEdges = evaluateQuery(context, oldEdgeToNewEdgeMap[edge]);
        if (size(evaluatedEdges) != 1)
        {
            setErrorEntities(context, (reportErrorToTopLevel) ? topLevelId : id, { "entities" : qEdgeAdjacent(qUnion(evaluatedEdges), EntityType.FACE) });
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_ALIGNMENT, ["flangeAlignment"]);
        }
        var updatedEdge = evaluatedEdges[0];
        var flangeData = getFlangeData(context, updatedEdge, definition);

        //create a plane as a limit surface for extending the underlying sheet
        var planeNormal = flangeData.plane.normal;
        var edgeMidpoint = .5 * (flangeData.edgeEndPoints[1].origin + flangeData.edgeEndPoints[0].origin);
        var origin = edgeMidpoint + extendDistance * planeNormal;
        var extendIndexedId = id + "extend" + unstableIdComponent(index);
        try
        {
            opExtendSheetBody(context, extendIndexedId, {
                    "extendMethod" : ExtendSheetBoundingType.EXTEND_TO_SURFACE,
                    "entities" : updatedEdge,
                    "limitEntity" : plane(origin, planeNormal)
            });
        }
        catch
        {
            // Error display
            processSubfeatureStatus(context, topLevelId, {"subfeatureId" : extendIndexedId, "propagateErrorDisplay" : true});
            setErrorEntities(context, topLevelId, { "entities" : updatedEdge });
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_ALIGNMENT, ["edges"]);
        }
        changedEntities = append(changedEntities, qCreatedBy(extendIndexedId));
        index += 1;
    }
    var updatedEdges = [];
    for (var e in originalFlangeEdges)
    {
        var newEdge = evaluateQuery(context, oldEdgeToNewEdgeMap[e]);
        updatedEdges = append(updatedEdges, newEdge[0]);
    }

    result.updatedEdges = qUnion(updatedEdges);
    result.modifiedEntities = qUnion(changedEntities);
    result.cornerVertices = evaluateQuery(context, qUnion(originalCornerVertices));
    return result;
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

function updateDefinition(context is Context, edge is Query, definition is map, edgeToFlangeData is map, cornerVertices is array) returns map
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
        var vertexData = getVertexData(context, edge, edgeVertices.vertices[i], edgeToFlangeData, definition, i, cornerVertices);
        if (vertexData != undefined)
        {
            flangeSideDirs[i] = vertexData.flangeSideDir;
            flangeBasePoints[i] = vertexData.flangeBasePoint;
        }
    }
    return mergeMaps(definition, {"flangeSideDirections" : flangeSideDirs, "flangeBasePoints" : flangeBasePoints});
}

function getOffsetForClearance(context is Context, sidePlane is Plane, clearance is ValueWithUnits, definition is map, flangePlane is Plane)
{
    var minDelta = clearance;

    var angleClearance = .5 * definition.thickness * abs(dot(flangePlane.normal, sidePlane.normal));
    var offsetForClearance = minDelta + angleClearance;
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
    var offset = (definition.thickness * 0.5 + edgeBendRadius) * tan(.5 * edgeBendAngle);
    // "move" side edge towards the inside of adjacent plane by offset (based on side edge bend info)
    var directionToMoveEdgeBy = cross(adjacentPlane.normal, sideEdgeMidPt[0].direction);
    var planeFromSideEdge = plane(vertexPoint + directionToMoveEdgeBy * offset, directionToMoveEdgeBy);

    //find flange edge direction on adjacent plane
    const flangeEdgeMidPt = evEdgeTangentLines(context, {"edge": flangeEdge, "parameters" : [0.5], "face": flangeData.adjacentFace});
    offset = (definition.thickness * 0.5 + definition.bendRadius) * tan(.5 * definition.bendAngle);
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
        offset = (definition.thickness * 0.5 + definition.bendRadius) * tan(.5 * definition.bendAngle);
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
    var d1 = norm(edgeEndPoints[0].origin - position);
    var d2 = norm(edgeEndPoints[1].origin - position);
    var vectorForEdge;
    if (d1 >= d2)
        vectorForEdge = edgeEndPoints[0].origin - edgeEndPoints[1].origin;
    else
        vectorForEdge = edgeEndPoints[1].origin - edgeEndPoints[0].origin;
    return stripUnits(vectorForEdge) as Vector;
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
    if (size(vertexEdgesArray) == 1)
    {
        var flangeDataForNeighbor = edgeToFlangeData[vertexEdgesArray[0]];
        if (flangeDataForNeighbor != undefined && flangeDataForNeighbor.adjFace == edgeToFlangeData[edge].adjFace)
        {
            return {"edgeX" : vertexEdgesArray[0], "position" : evVertexPoint(context, {"vertex" : vertexToUse}) };
        }
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

    if (size(evaluateQuery(context, edgeY)) != 0)
    {
        //if edgeY is collinear with edgeX,look for next edge on sideFace
        var line1 = evLine(context, {"edge" : edgeX});
        var line2 = evLine(context, {"edge" : edgeY});
        if (line1 != undefined && line2 != undefined && tolerantCoLinear(line1, line2))
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
        var lineNewX = evLine(context, {"edge" : edgeX});
        if (lineNewX == undefined || !tolerantCoLinear(lineOrigX, lineNewX, !isAtVersionOrLater(context, FeatureScriptVersionNumber.V493_FLANGE_BASE_SHIFT_FIX)))
            return undefined;
        sideFace =  qSubtraction(qEdgeAdjacent(edgeX, EntityType.FACE), flangeAdjacentFace);
        edgeY = qSubtraction(qIntersection([qEdgeAdjacent(sideFace, EntityType.EDGE), vertexEdges]), edgeX);
    }

    var edgeXEvaluated = evaluateQuery(context, edgeX);
    var edgeYEvaluated = evaluateQuery(context, edgeY);
    if (size(edgeYEvaluated) == 0 || size(edgeXEvaluated) == 0)
        return undefined;
    return { "edgeX" : edgeXEvaluated[0],
             "edgeY" : edgeYEvaluated[0],
             "sideFace" : evaluateQuery(context, sideFace)[0],
             "position" : evVertexPoint(context, {"vertex" : vertexToUse})};
}

function createPlaneForMiter(context is Context, flangeData is map, plane1 is Plane, plane2 is Plane, sideEdge, sideEdgeIsBend is boolean, position is Vector, index is number, miterAngle)
{
    // if sideEdge is a bend, don't miter if flaps are on the "outside"
    if (sideEdgeIsBend)
    {
        var convexity = evEdgeConvexity(context, {"edge" : sideEdge});
        if (convexity == EdgeConvexityType.CONCAVE && dot(flangeData.direction, plane1.normal) < TOLERANCE.zeroAngle)
        {
            return undefined;
        }
        else if (convexity == EdgeConvexityType.CONVEX && dot(flangeData.direction, plane1.normal) > TOLERANCE.zeroAngle)
        {
            return undefined;
        }
    }
    var midPlaneNormal = plane2.normal - plane1.normal;
    if (miterAngle == undefined)
    {
        if (norm(midPlaneNormal) < TOLERANCE.zeroLength)
            return undefined;
        return plane(position, midPlaneNormal);
    }
    else
    {
        //make sure the plane is rotated correctly for non-90-degree flanges
        var edgeDirection = index == 0 ? flangeData.edgeEndPoints[index].direction : -1 * flangeData.edgeEndPoints[index].direction;
        var axisDirection = cross(edgeDirection, flangeData.direction);
        return plane(position, rotationMatrix3d(axisDirection, miterAngle) * plane2.normal);
    }
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
    if (edgeY == undefined)
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

function getVertexData(context is Context, edge is Query, vertex is Query, edgeToFlangeData is map, definition is map, i is number, cornerVertices) returns map
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
            var vertexEdges = evaluateQuery(context, qSubtraction(qVertexAdjacent(vertex, EntityType.EDGE), edge));
            if (size(vertexEdges) == 1)
            {
                var adjacentPlane = evPlane(context, {"face": flangeData.adjacentFace});
                var sidePlane = plane(position, flangeData.edgeEndPoints[i].direction);
                sidePlane = createPlaneForMiter(context, flangeData, adjacentPlane, sidePlane, vertexEdges[0], false, position, i, getAngleForAngledMiter(definition));
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

    if (sideFace == undefined && edgeY == undefined)
    {
        if (edgeToFlangeData[sideEdge] != undefined)
            sidePlane = edgeToFlangeData[sideEdge].plane;
       else
            sidePlane = plane(position, edgeEndDirection);
        adjPlane = edgeToFlangeData[edge].plane;
    }
    else
    {
        sidePlane = evPlane(context, {"face" : sideFace});
        adjPlane = evPlane(context, {"face" : flangeData.adjacentFace});
    }
    var needsTrimChanges = false;
    var autoMitered = false; // vertex is a corner where an actual miter happened when auto-miter is on
    var tighterClearance = isAtVersionOrLater(context, FeatureScriptVersionNumber.V521_SM_CLEARANCE);
    var clearanceFromSide = definition.minimalClearance + definition.thickness * 0.5;
    if (!definition.autoMiter)
    {
        var sidePlane = plane(position, edgeEndDirection);
        sidePlane = createPlaneForMiter(context, flangeData, adjPlane, sidePlane, sideEdge, false, vertexAndEdges.position, i, getAngleForAngledMiter(definition));
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
        sidePlane = createPlaneForMiter(context, flangeData, adjPlane, sidePlane, sideEdge, sideEdgeIsBend, vertexAndEdges.position, i, undefined);
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

function addBendAttribute(context is Context, edge is Query, topLevelId is Id, index is number, definition is map)
{
    var attributeId = toAttributeId(topLevelId + index);
    var bendAttribute = makeSMJointAttribute(attributeId);
    bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited": true };
    bendAttribute.radius = {
        "value" : definition.bendRadius,
        "canBeEdited" : true
    };
    bendAttribute.angle = {
        "value" : definition.bendAngle,
        "canBeEdited" : true
    };
    setAttribute(context, {"entities" : edge, "attribute" : bendAttribute});
}

function createSketchPlane(context is Context, edgeLine is Line, sidePlaneNormal is Vector, definition is map) returns Plane
{
    var flipFactor = definition.oppositeDirection ? -1 : 1;
    var angle = flipFactor * definition.bendAngle;
    var sketchPlaneNormal = rotationMatrix3d(edgeLine.direction, angle) * sidePlaneNormal;

    //create plane passing through edge endpoint, with normal
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

function getFlangeData(context is Context, edge is Query, definition is map) {
    const adjacentFace = qEdgeAdjacent(edge, EntityType.FACE);
    if (size(evaluateQuery(context, adjacentFace)) != 1)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_INTERNAL);
    }
    const edgeEndPoints = evEdgeTangentLines(context, { "edge" : edge, "parameters" : [0, 1] , "face": adjacentFace});
    const sidePlane = evPlane(context, {"face" : adjacentFace});
    var sketchPlane = createSketchPlane(context, edgeEndPoints[0], sidePlane.normal, definition);
    var result = {};
    result.direction = cross(edgeEndPoints[0].direction, sketchPlane.normal);
    result.plane = sketchPlane;
    result.edgeEndPoints = edgeEndPoints;
    result.adjacentFace = adjacentFace;
    return result;
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
    planeResult = movePlaneForFlangeClearance(flangePlane, planeResult, true, thickness, minDelta);
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
    var angleClearance = .5 * thickness * abs(dot(flangePlane.normal, otherPlane.normal));
    var delta = minDelta + angleClearance;
    delta = (withPlaneNormal ? 1 : -1) * delta;
    otherPlane.origin = otherPlane.origin + delta * otherPlane.normal;

    return otherPlane;
}

function createAndSolveSketch(context is Context, topLevelId is Id, id is Id, edge is Query, definition is map)
{
    var flangeData = getFlangeData(context, edge, definition);
    var thickness = definition.thickness;
    var flangeDirection = flangeData.direction;

    var distance = 0.0 * meter;
    var offsets = makeArray(2);

    var basePoints = definition.flangeBasePoints;
    if (definition.limitType == SMFlangeBoundingType.BLIND)
    {
        distance = definition.distance  - ((0.5 * thickness) / tan(.5 * (PI * radian - definition.bendAngle)));
        offsets = getOffsetsForSideEdgesForBlind(definition.flangeSideDirections, flangeDirection, distance);
    }
    else
    {
        var planeResult = getPlaneForLimitEntity(context, definition, flangeData, thickness);
        offsets = getOffsetsForSideEdgesUpToPlane(context, definition.flangeSideDirections, flangeDirection, basePoints, planeResult);
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

function createFlangeSurfaceReturnBendEdge(context is Context, topLevelId is Id,  indexedId is Id, edge is Query, definition is map) returns Query
{
    var sketchId = indexedId + "sketch";
    createAndSolveSketch(context, topLevelId, sketchId, edge, definition);
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


function tolerantParallel(line0 is Line, line1 is Line, stricter is boolean) returns boolean
{
    var limit = stricter ? (TOLERANCE.zeroAngle * TOLERANCE.zeroAngle) : (TOLERANCE.g1Angle * TOLERANCE.g1Angle);
    return squaredNorm(cross(line0.direction, line1.direction)) < limit;
}

function tolerantCoLinear(line0 is Line, line1 is Line, stricter is boolean) returns boolean
{
    if (tolerantParallel(line0, line1, stricter)) {
        var v = line1.origin - line0.origin;
        v = v - line0.direction * dot(v, line0.direction);
        var lengthTolerance = TOLERANCE.zeroLength * meter;
        return squaredNorm(v) < lengthTolerance * lengthTolerance;
    }
    return false;
}

function tolerantCoLinear(line0 is Line, line1 is Line) returns boolean
{
    if (tolerantParallel(line0, line1, true)) {
        var v = line1.origin - line0.origin;
        v = v - line0.direction * dot(v, line0.direction);
        var lengthTolerance = TOLERANCE.zeroLength * meter;
        return squaredNorm(v) < lengthTolerance * lengthTolerance;
    }
    return false;
}

