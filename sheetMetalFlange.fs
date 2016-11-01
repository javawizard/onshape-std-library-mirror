FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/boolean.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/extrude.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");
import(path : "onshape/std/query.fs", version : "✨");
import(path : "onshape/std/sketch.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/smjointtype.gen.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/topologyUtils.fs", version : "✨");
import(path : "onshape/std/units.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");
import(path : "onshape/std/extendsheetboundingtype.gen.fs", version : "✨");


const FLANGE_ANGLE_BOUNDS =
{
    (degree) : [0, 90, 179],
    (radian) : 1
} as AngleBoundSpec;

/**
* @internal
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
* @internal
*  This feature produces a sheet metal flange
*/
annotation { "Feature Type Name" : "Flange", "Editing Logic Function" : "flangeEditLogic" }
export const smFlange = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
precondition
{
    annotation { "Name" : "Edges to flange", "Filter" : (EntityType.EDGE && GeometryType.LINE && SketchObject.NO) && BodyType.SOLID }
    definition.edges is Query;

    annotation {"Name" : "Flange alignment"}
    definition.flangeAlignment is SMFlangeAlignment;

    annotation { "Name" : "End type" }
    definition.limitType is SMFlangeBoundingType;

    if (definition.limitType == SMFlangeBoundingType.BLIND)
    {
        annotation { "Name" : "Distance"}
        isLength(definition.distance, NONNEGATIVE_LENGTH_BOUNDS);
    }
    else if (definition.limitType == SMFlangeBoundingType.UP_TO_ENTITY || definition.limitType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
    {
        annotation {"Name" : "Up to entity", "Filter" : EntityType.FACE || EntityType.VERTEX}
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

    annotation { "Name" : "Use default radius", "Default" : true }
    definition.useDefaultRadius is boolean;
    if (!definition.useDefaultRadius)
    {
        annotation { "Name" : "Bend Radius" }
        isLength(definition.bendRadius, BLEND_BOUNDS);
    }


    annotation { "Name" : "Bend angle" }
    isAngle(definition.bendAngle, FLANGE_ANGLE_BOUNDS);

    annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
    definition.oppositeDirection is boolean;
}
{
    var edges = qUnion(getSMDefinitionEntities(context, definition.edges));
    var evaluatedEdgeQuery = evaluateQuery(context, edges);
    if (size(evaluatedEdgeQuery) == 0)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_EDGE_NEEDED, ["edges"]);
    }

    var modelToEdgeMap = getModelToEdgeMap(context, evaluatedEdgeQuery);
    if (size(modelToEdgeMap)  > 1 && definition.useDefaultRadius)
        throw regenError ("Default bend radius is not available for selections from multiple sheet metal models.");

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
    for (var entry in modelToEdgeMap)
    {
        objectCounter = updateSheetMetalModelForFlange(context, id, objectCounter, qUnion(entry.value), definition);
    }

    // Add association attributes where needed and compute deleted attributes
    var toUpdate = assignSMAttributesToNewOrSplitEntities(context, smBodiesQ, allOriginalEntities, initialAssociationAttributes);
    updateSheetMetalGeometry(context, id, { "entities" : toUpdate.modifiedEntities,
                                            "deletedAttributes" : toUpdate.deletedAttributes});

}, { oppositeDirection : false, limitType : SMFlangeBoundingType.BLIND,  flangeAlignment : SMFlangeAlignment.INNER,
     autoMiter : true, useDefaultRadius : true, oppositeOffsetDirection: false,
     isPartialFlange : false, userAssignedSides : false}); //TODO: make user visible


function getModelToEdgeMap(context is Context, edges is array) returns map
{
    var modelToEdgeMap = {};
    for (var edge in edges)
    {
        const attributes = getSmObjectTypeAttributes(context, qOwnerBody(edge), SMObjectType.MODEL);
        const attributeId = attributes[0].attributeId;
        if (modelToEdgeMap[attributeId] != undefined)
            modelToEdgeMap[attributeId] = append(modelToEdgeMap[attributeId], edge);
        else
            modelToEdgeMap[attributeId] = [edge];
    }
    return modelToEdgeMap;
}

/**
* @internal
*  This makes sure that when the user deselects defaultradius option, we default to the default radius
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

    var modelParams = getModelParameters(context, evaluatedEdgeQuery[0]);
    if (isCreating)
    {
        var modelToEdgeMap = getModelToEdgeMap(context, evaluatedEdgeQuery);
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

    return definition;
}


function updateSheetMetalModelForFlange(context is Context, topLevelId is Id,  objectCounter is number, edges is Query, definition is map) returns number
{
    var evaluatedEdgeQuery = evaluateQuery(context, edges);

    // add thickness, minimalClearance and defaultBendRadius to definition.
    // Flange uses thickness, minimalClearance and potentially defaultBendRadius derived from underlying sheet metal model
    definition = mergeMaps(definition, getModelParameters(context, evaluatedEdgeQuery[0]));
    if (definition.useDefaultRadius)
    {
        definition.bendRadius = definition.defaultBendRadius;
    }

    var alignmentChanges = changeUnderlyingSheetForAlignment(context, topLevelId + unstableIdComponent(objectCounter), definition, edges);
    var originalCornerVertices = alignmentChanges.cornerVertices;
    var modifiedEntities = alignmentChanges.modifiedEntities;
    edges = alignmentChanges.updatedEdges;

    var edgeToFlangeData = {};
    for (var edge in evaluateQuery(context,edges))
    {
        edgeToFlangeData[edge] = getFlangeData(context, edge, definition);
    }

    var surfaceBodies = [];
    var originalEntities = [];
    for (var edge in evaluateQuery(context,edges))
    {
        var ownerBody = qOwnerBody(edge);
        surfaceBodies = append(surfaceBodies, ownerBody);
        originalEntities = append(originalEntities, qSubtraction(qOwnedByBody(ownerBody), modifiedEntities));
        var indexedId = topLevelId + unstableIdComponent(objectCounter);
        var surfaceId = indexedId + SURFACE_SUFFIX;

        var updatedDefinition = updateDefinition(context, edge, definition, edgeToFlangeData, originalCornerVertices);
        var bendEdge = createFlangeSurfaceReturnBendEdge(context, indexedId, edge, updatedDefinition);
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
    var indexedId = topLevelId + unstableIdComponent(objectCounter);

    //boolean the original sm surface and the newly created surfaces together
    var allSurfaces = qUnion(surfaceBodies);
    try
    {
        opBoolean(context, indexedId + ("flange_boolean"), {
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
        var subfeatureId = indexedId + ("flange_boolean");
        processSubfeatureStatus(context, topLevelId, {"subfeatureId" : subfeatureId, "propagateErrorDisplay" : true});
        if (getFeatureWarning(context, subfeatureId) != undefined || getFeatureError(context, subfeatureId) != undefined)
        {
            const errorId = indexedId + "errorEntities";
            setErrorEntities(context, indexedId, { "entities" : allSurfaces });
            opDeleteBodies(context, indexedId + "delete", { "entities" : qSubtraction(allSurfaces, originalBodies) });
        }
    }
    //add rips to new interior edges
    for (var entity in evaluateQuery(context, qOwnedByBody(allSurfaces, EntityType.EDGE)))
    {
        var attributes = getAttributes(context, {"entities" : entity, "attributePattern" : asSMAttribute({})});
        if (size(attributes) == 0 && edgeIsTwoSided(context, entity))
        {
            addRipAttribute(context, entity, topLevelId, objectCounter);
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
function changeUnderlyingSheetForAlignment(context is Context, id is Id, definition is map, edges is Query)
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

    for (var edge in originalFlangeEdges)
    {
        var evaluatedEdges = evaluateQuery(context, oldEdgeToNewEdgeMap[edge]);
        if (size(evaluatedEdges) != 1)
        {
            setErrorEntities(context, id, { "entities" : qEdgeAdjacent(qUnion(evaluatedEdges), EntityType.FACE) });
            throw regenError("Extend should not split a flanged edge");
        }
        var updatedEdge = evaluatedEdges[0];
        var flangeData = getFlangeData(context, updatedEdge, definition);

        //create a plane as a limit surface for extending the underlying sheet
        var planeNormal = flangeData.plane.normal;
        var edgeMidpoint = .5 * (flangeData.edgeEndPoints[1].origin + flangeData.edgeEndPoints[0].origin);
        var origin = edgeMidpoint + extendDistance * planeNormal;
        var extendIndexedId = id + "extend" + unstableIdComponent(index);
        opExtendSheetBody(context, extendIndexedId, {
                    "extendMethod" : ExtendSheetBoundingType.EXTEND_TO_SURFACE,
                    "offset" : 0 * inch,
                    "entities" : updatedEdge,
                    "limitEntity" : plane(origin, planeNormal)
        });

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
        throw regenError("Could not find flange data for edge");
    }
    var flangeSideDirs = [flangeData.direction, flangeData.direction];
    var flangeBasePoints = [edgeVertices.points[0].origin, edgeVertices.points[1].origin];
    for (var i = 0; i < size(edgeVertices.vertices); i += 1 )
    {
        if (isIn(edgeVertices.vertices[i], cornerVertices))
            continue;
        var vertexData = getVertexData(context, edge, edgeVertices.vertices[i], edgeToFlangeData, definition);
        if (vertexData != undefined)
        {
            flangeSideDirs[i] = vertexData.flangeSideDir;
            flangeBasePoints[i] = vertexData.flangeBasePoint;
        }
    }
    return mergeMaps(definition, {"flangeSideDirections" : flangeSideDirs, "flangeBasePoints" : flangeBasePoints});
}

function getOffsetForClearance(context is Context, sidePlane is Plane, definition is map, flangePlane is Plane)
{
    var minDelta = .5 * definition.thickness + definition.minimalClearance;

    var angleClearance = .5 * definition.thickness * abs(dot(flangePlane.normal, sidePlane.normal));
    var offsetForClearance = minDelta + angleClearance;
    return offsetForClearance;
}

function getFlangeBasePoint(context is Context, flangeEdge is Query, sideEdge is Query, definition is map,
    flangeData is map, vertexPoint is Vector, isTrimmed is boolean, sidePlane is Plane)
{
    var jointAttribute = getJointAttribute(context, sideEdge);
    var sideEdgeIsBend = (jointAttribute != undefined && jointAttribute.jointType.value == SMJointType.BEND);
    if (!sideEdgeIsBend)
    {
        return vertexPoint;
    }
    var edgeLine = evLine(context, {"edge" : flangeEdge});

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
    if (!isTrimmed)
    {
        //project onto the flange edge
        var projection = project(edgeLine, intersectionData.intersection);
        //check that the projection lies within parameter bounds of edge
        var edgesFound = evaluateQuery(context, qContainsPoint(flangeEdge, projection));
        if (size(edgesFound) != 1)
            return vertexPoint;
        return projection;
    }
    else //we want to project the point onto the bent edge on the flange plane, then use that to do plane shift
    {
        offset = (definition.thickness * 0.5 + definition.bendRadius) * tan(.5 * definition.bendAngle);
        var lineFromBentEdge = line(vertexPoint + flangeData.direction * offset, flangeEdgeMidPt[0].direction);
        var updatedProjection = project(lineFromBentEdge, intersectionData.intersection);
        var offsetFromBend = evDistance(context, {"side0" : updatedProjection, "side1": project(sidePlane, updatedProjection)}).distance;

        //compute default clearance, and only offset with max between the two.
        var offsetFromClearance = getOffsetForClearance(context , sidePlane, definition, flangeData.plane);

        //shift plane
        var delta = abs(offsetFromClearance) > abs(offsetFromBend) ? offsetFromClearance : offsetFromBend;
        var moveWithNormal = evEdgeConvexity(context, { "edge" : sideEdge }) == EdgeConvexityType.CONCAVE;
        delta =  (moveWithNormal ? 1 : -1) * delta;
        sidePlane.origin = sidePlane.origin + delta * sidePlane.normal;

        //use shifted plane for base move
        var intersectionData = intersection(sidePlane, edgeLine);
        if (intersectionData.dim != 0)
            throw regenError("Found a non-point intersection between plane and line");
        return intersectionData.intersection;
    }
}

function getOrderedEdgeVertices(context is Context, edge is Query) returns map
{
    var edgeVertices = evaluateQuery(context, qVertexAdjacent(edge, EntityType.VERTEX));
    if (size(edgeVertices) != 2)
        throw regenError("Edge to flange has wrong number of vertices");

    var p0 = evVertexPoint(context, {"vertex" : edgeVertices[0]});

    const adjacentFace = qEdgeAdjacent(edge, EntityType.FACE);
    if (size(evaluateQuery(context, adjacentFace)) != 1)
    {
        throw regenError("Edge to flange is not laminar");
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
    var result;
    var edgeEndPoints = evEdgeTangentLines(context, { "edge" : edge, "parameters" : [0, 1] });
    if (tolerantEquals(edgeEndPoints[0].origin, position))
        result = edgeEndPoints[1].origin - position;
    else
        result = edgeEndPoints[0].origin - position;

    return stripUnits(result) as Vector;
}

/*
 * Returns an XY coordinate system at the end vertex of edge plus the query for sideFace vertex is adj to.
 * x will be in the direction of the interior edge, and y will be direction of the laminar edge
 */
function getXYAtVertex(context is Context, vertex is Query, edge is Query, edgeToFlangeData is map)
{
    var vertexToUse = vertex;
    var vertexEdges = evaluateQuery(context, qSubtraction(qVertexAdjacent(vertex, EntityType.EDGE), edge));
    var potentialSideEdge = undefined;
    if (size(vertexEdges) == 1)
    {
        var flangeDataForNeighbor = edgeToFlangeData[vertexEdges[0]];
        if (flangeDataForNeighbor != undefined && flangeDataForNeighbor.adjFace == edgeToFlangeData[edge].adjFace)
        {
            return {"edgeX" : vertexEdges[0], "position" : evVertexPoint(context, {"vertex" : vertexToUse}) };
        }
        //this might be a result of extend sheet out (for inner alignment)
        vertexToUse = qSubtraction(qVertexAdjacent(vertexEdges[0], EntityType.VERTEX), vertex);
        // note that we don't need the collinearity check here as we did below because we already filtered out
        // existing corner vertices
        vertexEdges = evaluateQuery(context, qSubtraction(qVertexAdjacent(vertexToUse, EntityType.EDGE), vertexEdges[0]));
    }
    else if (size(vertexEdges) == 2)
    {
        var line1 = evLine(context, {"edge" : vertexEdges[0]});
        var line2 = evLine(context, {"edge" : vertexEdges[1]});
        if (line1 != undefined && line2 != undefined && tolerantCoLinear(line1, line2))
        {
            //this might be a result of trimming a sheet in (for outer alignment)
            //one vertex edge should be laminar. find the vertex on the other side of the laminar edge.
            if (!edgeIsTwoSided(context, vertexEdges[0]))
            {
                vertexToUse = qSubtraction(qVertexAdjacent(vertexEdges[0], EntityType.VERTEX), vertex);
                potentialSideEdge = vertexEdges[1];
            }
            else
            {
                vertexToUse = qSubtraction(qVertexAdjacent(vertexEdges[1], EntityType.VERTEX), vertex);
                potentialSideEdge = vertexEdges[0];
            }
            vertexEdges = evaluateQuery(context, qVertexAdjacent(vertexToUse, EntityType.EDGE));
        }
    }
    if (size(vertexEdges) != 2)
    {
        return undefined;
    }

    // decide which edge is the interior (edgeX), which edge is the laminar one (edgeY)
    var edgeY;
    var edgeX;
    var sideFace;
    var sideFaces = evaluateQuery(context, qEdgeAdjacent(vertexEdges[0], EntityType.FACE));
    if (size(sideFaces) == 1)
    {
        edgeY = vertexEdges[0];
        edgeX = vertexEdges[1];
        sideFace = sideFaces[0];
    }
    else
    {
        edgeY = vertexEdges[1];
        edgeX = vertexEdges[0];
        var sideFaces = evaluateQuery(context, qEdgeAdjacent(vertexEdges[1], EntityType.FACE));
        sideFace = sideFaces[0];
    }
    var position = evVertexPoint(context, {"vertex" : vertexToUse});
    return {"edgeX" : edgeX, "edgeY" : edgeY, "sideFace" : sideFace, "potentialSideEdge" : potentialSideEdge, "position" : position};
}

function createMidPlaneForMiter(context is Context, flangeData is map, plane1 is Plane, plane2 is Plane, sideEdge, sideEdgeIsBend is boolean, position is Vector)
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
    return plane(position, midPlaneNormal);
}

function checkIfNeedsBaseUpdate(definition is map, sideEdgeIsBend is boolean) returns boolean
{
    if (definition.isPartialFlange)
        return true;
    else if (sideEdgeIsBend)
        return true;
    return false;
}

function getVertexData(context is Context, edge is Query, vertex is Query, edgeToFlangeData is map, definition is map) returns map
{
    var result = {
        "flangeBasePoint" : evVertexPoint(context, {"vertex" : vertex}),
        "flangeSideDir" : undefined
    };

    var needsSideDirUpdate = false;
    var flangeData = edgeToFlangeData[edge];
    var vertexAndEdges = getXYAtVertex(context, vertex, edge, edgeToFlangeData);
    if (vertexAndEdges == undefined)
    {
        if (definition.isPartialFlange)
        {
            //TODO : make sure this works when partial flange implemented
            //result.flangeBasePoint = getFlangeBasePointForPartial();
        }
        return  result;
    }
    var edgeX = vertexAndEdges.edgeX;
    var edgeY = vertexAndEdges.edgeY;
    var sideFace = vertexAndEdges.sideFace;

    var sideEdge = vertexAndEdges.potentialSideEdge == undefined ? edgeX : vertexAndEdges.potentialSideEdge;
    var jointAttribute = try silent(getJointAttribute(context, sideEdge));
    var sideEdgeIsBend = (jointAttribute != undefined && jointAttribute.jointType.value == SMJointType.BEND);

    var needsBaseUpdate = checkIfNeedsBaseUpdate(definition, sideEdgeIsBend);
    var sidePlane = undefined;
    var adjPlane = undefined;
    if (sideFace == undefined && edgeY == undefined)
    {
        sidePlane = edgeToFlangeData[edgeX].plane;
        adjPlane = edgeToFlangeData[edge].plane;
    }
    else
    {
        sidePlane = evPlane(context, {"face" : sideFace});
        adjPlane = evPlane(context, {"face" : flangeData.adjacentFace});
    }

    var isTrimmed = false;
    if (edgeY == undefined || edgeToFlangeData[edgeY] != undefined) //adjacent laminar edge is also being flanged, it should be handled via miter
    {
        if (!definition.autoMiter)
        {
            result.flangeBasePoint = getFlangeBasePoint(context, edge, sideEdge, definition, flangeData, vertexAndEdges.position, isTrimmed, sidePlane);
            return result;
        }
        var originalSidePlane = sidePlane;
        sidePlane = createMidPlaneForMiter(context, flangeData, adjPlane, sidePlane, sideEdge, sideEdgeIsBend, vertexAndEdges.position);
        if (sidePlane != undefined)
        {
            needsSideDirUpdate = true;
            needsBaseUpdate = false;
        }
        else
        {
            sidePlane = originalSidePlane;
        }
    }
    else if (definition.userAssignedSides)
    {
        needsSideDirUpdate = true;
        //TODO : user defined side controls for flanges
    }
    else
    {
        if (angleBetween(adjPlane.normal, sidePlane.normal) >= .5 * PI * radian)
        {
            var flangeDir = flangeData.direction;
            var position = vertexAndEdges.position;
            var projOnY = dot(flangeDir, getVectorForEdge(context, edgeY, position));
            var projOnX = dot(flangeDir, getVectorForEdge(context, edgeX, position));
            needsSideDirUpdate = (projOnY > TOLERANCE.zeroLength && projOnX > -TOLERANCE.zeroLength);
            needsBaseUpdate = needsSideDirUpdate || needsBaseUpdate;
            isTrimmed = needsSideDirUpdate;
        }
    }

    if (needsBaseUpdate)
    {
        result.flangeBasePoint = getFlangeBasePoint(context, edge, sideEdge, definition, flangeData, vertexAndEdges.position, isTrimmed, sidePlane);
    }
    if (needsSideDirUpdate) // need a trim on the flange sides by a plane
    {
        result.flangeSideDir = getFlangeSideDir(flangeData, sidePlane);
    }
    return result;
}

function getFlangeSideDir(flangeData is map, sidePlane is Plane)
{
    var intersectionDirection = normalize(cross(flangeData.plane.normal, sidePlane.normal));
    var isOppositeFlangedir = dot(flangeData.direction, intersectionDirection) < 0;
    var flangeSideDirection = isOppositeFlangedir ? -1 * intersectionDirection : intersectionDirection;
    return flangeSideDirection;
}
function addBendAttribute(context is Context, edge is Query, topLevelId is Id, index is number, definition is map)
{
    var attributeId = toAttributeId(topLevelId + index);
    var bendAttribute = makeSMJointAttribute(attributeId);
    bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited": true };
    bendAttribute.radius = {
        "value" : definition.bendRadius,
        "controllingFeatureId" : toAttributeId(topLevelId),
        "parameterIdInFeature" : "bendRadius",
        "canBeEdited" : true
    };
    bendAttribute.angle = {
        "value" : definition.bendAngle,
        "controllingFeatureId" : toAttributeId(topLevelId),
        "parameterIdInFeature" : "bendAngle",
        "canBeEdited" : true
    };
    setAttribute(context, {"entities" : edge, "attribute" : bendAttribute});
}

function addRipAttribute(context is Context, edge is Query, topLevelId is Id, index is number)
{
    var ripAttribute = makeSMJointAttribute(toAttributeId(topLevelId + index));
    ripAttribute.jointType = { "value" : SMJointType.RIP, "canBeEdited": true };
    ripAttribute.jointStyle = { "value" : SMJointStyle.EDGE, "canBeEdited": true };

    //TODO :  re-examine once rip styling is in, and check whether we need to change anything for flange rips.
    var angle = try(edgeAngle(context, edge));
    if (angle != undefined)
    {
        ripAttribute.angle = {"value" : angle, "canBeEdited" : false};
    }

    setAttribute(context, {"entities" : edge, "attribute" : ripAttribute});
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
            throw regenError("Found a non-point intersection between plane and line");

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
        throw regenError("Edge to flange is not laminar.");
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
        throw regenError("Cannot resolve limit entity");

    //see if it's a plane:
    var planeResult = try silent(evPlane(context, {"face" : entity[0]}));
    if (planeResult == undefined)
    {   //see if it's an vertex
        var limitVertex = try silent(evVertexPoint(context, {"vertex" : entity[0]}));
        if (limitVertex == undefined)
        {
            throw regenError("Unrecognized limit entity", ["limitEntity"]);
        }
        else
        {
            //create plane from point & flangeDirection
            planeResult = plane(limitVertex, -flangeDirection);
        }
    }

    // if direction to limit entity is opposite of flange direction, fail
    var pointOnEdge = flangeData.edgeEndPoints[0].origin;
    var upToDirection = planeResult.origin - pointOnEdge;
    if (dot(upToDirection, flangeDirection) < TOLERANCE.zeroLength * meter)
    {
        throw regenError("Flange direction is opposite of limit entity");
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

function createAndSolveSketch(context is Context, id is Id, edge is Query, definition is map)
{
    var flangeData = getFlangeData(context, edge, definition);
    var thickness = definition.thickness;
    var flangeDirection = flangeData.direction;

    var distance = 0.0 * meter;
    var offsets = makeArray(2);

    var basePoints = definition.flangeBasePoints;
    if (definition.limitType == SMFlangeBoundingType.BLIND)
    {
        distance = definition.distance  - ((0.5 * thickness) / tan(0.5 * definition.bendAngle));
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
        throw regenError("Could not create flange.");
    }
}

function createFlangeSurfaceReturnBendEdge(context is Context, indexedId is Id, edge is Query, definition is map) returns Query
{
    var sketchId = indexedId + "sketch";
    createAndSolveSketch(context, sketchId, edge, definition);
    var bendLine = startTracking(context, sketchId, "polyline.line0");
    opExtractSurface(context, indexedId + SURFACE_SUFFIX, {"faces" : qSketchRegion(sketchId, false)});

    opDeleteBodies(context, indexedId + "delete_sketch", { "entities" : qCreatedBy(sketchId, EntityType.BODY) });

    return qIntersection([qCreatedBy(indexedId + SURFACE_SUFFIX, EntityType.EDGE), bendLine]);
}

function getModelParameters(context is Context, edge is Query) returns map
{
    var adjacentFace = qEdgeAdjacent(edge, EntityType.FACE);
    if (size(evaluateQuery(context, adjacentFace)) != 1)
    {
        throw regenError("Edge to flange is not laminar");
    }
    var attr = getAttributes(context, {"entities" : qOwnerBody(adjacentFace), "attributePattern" : asSMAttribute({})});

    if (size(attr) != 1 || attr[0].thickness == undefined || attr[0].thickness.value == undefined ||
        attr[0].minimalClearance == undefined || attr[0].minimalClearance.value == undefined ||
        attr[0].defaultBendRadius == undefined || attr[0].defaultBendRadius.value == undefined)
    {
        throw regenError("Bad sheet metal attribute");
    }
    return {"thickness" : attr[0].thickness.value, "minimalClearance" : attr[0].minimalClearance.value, "defaultBendRadius" : attr[0].defaultBendRadius.value};
}

