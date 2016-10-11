FeatureScript 432; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "432.0");
import(path : "onshape/std/boolean.fs", version : "432.0");
import(path : "onshape/std/containers.fs", version : "432.0");
import(path : "onshape/std/curveGeometry.fs", version : "432.0");
import(path : "onshape/std/extrude.fs", version : "432.0");
import(path : "onshape/std/evaluate.fs", version : "432.0");
import(path : "onshape/std/feature.fs", version : "432.0");
import(path : "onshape/std/math.fs", version : "432.0");
import(path : "onshape/std/query.fs", version : "432.0");
import(path : "onshape/std/sketch.fs", version : "432.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "432.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "432.0");
import(path : "onshape/std/smjointtype.gen.fs", version : "432.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "432.0");
import(path : "onshape/std/topologyUtils.fs", version : "432.0");
import(path : "onshape/std/units.fs", version : "432.0");
import(path : "onshape/std/valueBounds.fs", version : "432.0");
import(path : "onshape/std/vector.fs", version : "432.0");
import(path : "onshape/std/extendsheetboundingtype.gen.fs", version : "432.0");


const FLANGE_ANGLE_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (PI + TOLERANCE.zeroAngle) * radian,
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
    annotation { "Name" : "Middle" }
    MIDDLE,
    annotation { "Name" : "Inner" }
    INNER,
    annotation { "Name" : "Outer" }
    OUTER
}

const SURFACE_SUFFIX = "surface";
const CLEARANCE = 6e-4 * meter; //TODO : this should work with 2e-5 but currently doesnt :(

/**
* @internal
*  This feature produces a sheet metal flange
*/
annotation { "Feature Type Name" : "Flange" }
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
        isLength(definition.distance, LENGTH_BOUNDS);
    }
    else if (definition.limitType == SMFlangeBoundingType.UP_TO_ENTITY || definition.limitType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
    {
        annotation {"Name" : "Up to entity", "Filter" : EntityType.EDGE || EntityType.FACE || EntityType.VERTEX}
        definition.limitEntity is Query;
        if (definition.limitType == SMFlangeBoundingType.UP_TO_ENTITY_OFFSET)
        {
            annotation { "Name" : "Offset"}
            isLength(definition.offset, LENGTH_BOUNDS);
        }
    }

    annotation { "Name" : "Bend Radius" }
    isLength(definition.bendRadius, BLEND_BOUNDS);

    annotation { "Name" : "Bend angle" }
    isAngle(definition.bendAngle, FLANGE_ANGLE_BOUNDS);

    annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
    definition.oppositeDirection is boolean;

    //TODO :
    // Type : inner/outer flush/etc. -- if single edge, allow defining entity + distance
    // Relief type : round/obround/slot?? -- need catalogue of sorts from scott
    // Miter -- checkbox? (only symmetric to begin with)
    // Partial -- mutually exclusive with miter? have default distances from both ends and allow changing
}
{

    if (!areEntitiesFromSingleActiveSheetMetalModel(context, definition.edges))
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_EDGE_NEEDED, ["edges"]);
    }

    var edges = qUnion(getSMDefinitionEntities(context, definition.edges));
    var evaluatedEdgeQuery = evaluateQuery(context, edges);
    if (size(evaluatedEdgeQuery) == 0)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_EDGE_NEEDED, ["edges"]);
    }

    //get originals before any changes
    var allOriginalEntities = evaluateQuery(context, qOwnedByBody(qOwnerBody(edges)));

    // add thickness to definition. flange uses thickness derived from underlying sheet metal model
    definition.thickness = findThickness(context, evaluatedEdgeQuery[0]);

    var alignmentChanges = changeUnderlyingSheetForAlignment(context, id, definition, edges);
    var originalCornerVertices = alignmentChanges.cornerVertices;
    var modifiedEntities = alignmentChanges.modifiedEntities;
    edges = alignmentChanges.updatedEdges;

    var edgeToFlangeData = {};
    for (var edge in evaluateQuery(context,edges))
    {
        edgeToFlangeData[edge] = getFlangeData(context, edge, definition);
    }

    var index = 0;
    var surfaceBodies = [];
    var originalEntities = [];
    for (var edge in evaluateQuery(context,edges))
    {
        var ownerBody = qOwnerBody(edge);
        surfaceBodies = append(surfaceBodies, ownerBody);
        originalEntities = append(originalEntities, qSubtraction(qOwnedByBody(ownerBody), modifiedEntities));
        var indexedId = id + unstableIdComponent(index);
        var surfaceId = indexedId + SURFACE_SUFFIX;

        var updatedDefinition = updateDefinitionIfNeedsTrimming(context, edge, definition, edgeToFlangeData, originalCornerVertices);
        var bendEdge = createFlangeSurfaceReturnBendEdge(context, indexedId, edge, updatedDefinition);
        addBendAttribute(context, bendEdge, id, index, definition);

        //add wall attributes to faces
        for (var face in evaluateQuery(context, qCreatedBy(surfaceId, EntityType.FACE)))
        {
            setAttribute(context, {
                    "entities" : face,
                    "attribute" : makeSMWallAttribute(toAttributeId(id + index))
            });
        }
        surfaceBodies = append(surfaceBodies, qCreatedBy(surfaceId, EntityType.BODY));
        index += 1;
    }

    var originalBodies = qOwnerBody(qUnion(originalEntities));

    //boolean the original sm surface and the newly created surfaces together
    var allSurfaces = qUnion(surfaceBodies);
    try
    {
        opBoolean(context, id + ("flange_boolean"), {
            "allowSheets" : true,
            "tools" : allSurfaces,
            "operationType" : BooleanOperationType.UNION,
            "makeSolid" : false
        });
    }
    catch
    {
        // Error display
        var subfeatureId = id + ("flange_boolean");
        processSubfeatureStatus(context, id, {"subfeatureId" : subfeatureId, "propagateErrorDisplay" : true});
        if (getFeatureWarning(context, subfeatureId) != undefined || getFeatureError(context, subfeatureId) != undefined)
        {
            const errorId = id + "errorEntities";
            setErrorEntities(context, id, { "entities" : allSurfaces });
            opDeleteBodies(context, id + "delete", { "entities" : qSubtraction(allSurfaces, originalBodies) });
        }
    }

    //add rips to new interior edges
    for (var entity in evaluateQuery(context, qOwnedByBody(allSurfaces, EntityType.EDGE)))
    {
        var attributes = getAttributes(context, {"entities" : entity, "attributePattern" : asSMAttribute({})});
        if (size(attributes) == 0 && edgeIsTwoSided(context, entity))
        {
            addRipAttribute(context, entity, id);
        }
    }

    // Add association attributes where needed
    // Can not be done with subtraction query, because it will convert transient queries to entities, while new transient ids need to be considered
    var entitiesToAddAssociations = filter(evaluateQuery(context, qOwnedByBody(allSurfaces)), function (entry){ return !isIn(entry, allOriginalEntities);});
    for (var entity in entitiesToAddAssociations)
    {
        //can have attributes if created via split/merge
        removeAttributes(context, { "entities" : entity, "attributePattern" : {} as SMAssociationAttribute });
    }

    assignSmAssociationAttributes(context, qUnion([qUnion(entitiesToAddAssociations), modifiedEntities]));
    updateSheetMetalGeometry(context, id, { "entities" : qUnion(entitiesToAddAssociations) });

}, { oppositeDirection : false, limitType : SMFlangeBoundingType.BLIND,  flangeAlignment : SMFlangeAlignment.MIDDLE});

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
    var originalCornerVertices = trackCornerVertices(context, edges, []);

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
            setErrorEntities(context, id, { "entities" : qEdgeAdjacent(evaluatedEdges, EntityType.FACE) });
            throw regenError("Extend should not split a flanged edge");
        }
        var updatedEdge = evaluatedEdges[0];
        var flangeData = getFlangeData(context, updatedEdge, definition);

        var faceAttributes = getAttributes(context, {"entities" : flangeData.adjacentFace, "attributePattern" : asSMAttribute({})});
        var trackedEdge = startTracking(context, updatedEdge);

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

        //TODO : not clear why this is happening. technically this should not be necessary
        var faceCreated = evaluateQuery(context, qCreatedBy(extendIndexedId, EntityType.FACE));
        if (size(faceAttributes) == 1 && size(faceCreated) > 0)
        {
            setAttribute(context, {
                    "entities" : faceCreated[0],
                    "attribute" : faceAttributes[0]
            });
        }

        //This is to handle the new laminar edges created via the edge split. For now, clear attributes
        var allEdges = qVertexAdjacent(trackedEdge, EntityType.EDGE);
        for (var newEdge in evaluateQuery(context, allEdges))
        {
            if (!edgeIsTwoSided(context, newEdge))
                removeAttributes(context, { "entities" : newEdge, "attributePattern" : {} as SMAttribute });
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

function trackCornerVertices(context is Context, edges is Query, originalCornerVertices is array) returns array
{
    var vertices = evaluateQuery(context, qVertexAdjacent(edges, EntityType.VERTEX));
    for (var v in vertices)
    {
        if (size(evaluateQuery(context, qVertexAdjacent(v, EntityType.EDGE))) == 2)
        {
            originalCornerVertices = append(originalCornerVertices, startTracking(context, v));
            //have to add original vertex here as well, as the tracking query would skip it if it did not change.
            originalCornerVertices = append(originalCornerVertices, v);
        }
    }
    return originalCornerVertices;
}
function updateDefinitionIfNeedsTrimming(context is Context, edge is Query, definition is map, edgeToFlangeData is map, cornerVertices is array) returns map
{
    var offsetDirections = [undefined, undefined];
    var moveBase = [false, false];
    var edgeVertices = getOrderedEdgeVertices(context, edge);
    for (var i = 0; i < size(edgeVertices.vertices); i += 1 )
    {
        if (isIn(edgeVertices.vertices[i], cornerVertices))
            continue;

        var cSysAtVertex = getXYAtVertex(context, edgeVertices.vertices[i], edgeVertices.points[i].origin, edge, edgeToFlangeData );
        if (cSysAtVertex != undefined)
        {
            var flangeData = edgeToFlangeData[edge];
            if (flangeData == undefined)
            {
                continue; // we might want to throw a regen error here
            }
            var flangeDir = flangeData.direction;
            var flangePlane = flangeData.plane;

            var projOnY = dot(flangeDir, cSysAtVertex.y);
            var projOnX = dot(flangeDir, cSysAtVertex.x);

            var sidePlane = evPlane(context, {"face" : cSysAtVertex.sideFace});
            var adjPlane = evPlane(context, {"face" : flangeData.adjacentFace});

            if (angleBetween(adjPlane.normal, sidePlane.normal) < .5 * PI * radian)
            {
                continue; // there's no intersection
            }

            //if the default flange intersects the side face, update offset directions used in the underlying sketch
            if (projOnY > TOLERANCE.zeroLength && projOnX > -TOLERANCE.zeroLength)
            {
                moveBase[i] = true;
                var intersectionData = intersection(flangePlane, sidePlane);
                if (intersectionData != undefined)
                {
                    var isOppositeFlangedir = dot(flangeDir, intersectionData.direction) < 0;
                    var offsetDirection = isOppositeFlangedir ? -1 * intersectionData.direction : intersectionData.direction;
                    offsetDirections[i] = offsetDirection;
                }
            }
        }
    }
    return mergeMaps(definition, {"offsetDirections" : offsetDirections, "moveBase" : moveBase});
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
function getXYAtVertex(context is Context, vertex is Query, position is Vector, edge is Query, edgeToFlangeData is map)
{
    var vertexToUse = vertex;
    var vertexEdges = evaluateQuery(context, qSubtraction(qVertexAdjacent(vertex, EntityType.EDGE), edge));
    if (size(vertexEdges) == 1)
    {
        //this might be a result of extend sheet out (for inner alignment)
        vertexToUse = qSubtraction(qVertexAdjacent(vertexEdges[0], EntityType.VERTEX), vertex);
        position = evVertexPoint(context, {"vertex" : vertexToUse});
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
                vertexToUse = qSubtraction(qVertexAdjacent(vertexEdges[0], EntityType.VERTEX), vertex);
            else
            {
                vertexToUse = qSubtraction(qVertexAdjacent(vertexEdges[1], EntityType.VERTEX), vertex);
            }
            position = evVertexPoint(context, {"vertex" : vertexToUse});
            vertexEdges = evaluateQuery(context, qVertexAdjacent(vertexToUse, EntityType.EDGE));
        }
    }

    if (size(vertexEdges) != 2)
    {
        return undefined;
    }

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

    if (edgeToFlangeData[edgeY] != undefined) //adjacent laminar edge is also being flanged, it should be handled via miter
    {
        return undefined;
    }

    return { "x" : getVectorForEdge(context, edgeX, position),
             "y" : getVectorForEdge(context, edgeY, position),
             "sideFace" : sideFace };
}


function addBendAttribute(context is Context, edge is Query, id is Id, index is number, definition is map)
{
    var attributeId = toAttributeId(id + index);
    var bendAttribute = makeSMJointAttribute(attributeId);
    bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited": true };
    bendAttribute.radius = {
        "value" : definition.bendRadius,
        "controllingFeatureId" : toAttributeId(id),
        "parameterIdInFeature" : "radius",
        "canBeEdited" : true
    };

    bendAttribute.angle = definition.bendAngle;
    setAttribute(context, {"entities" : edge, "attribute" : bendAttribute});
}

function addRipAttribute(context is Context, edge is Query, id is Id)
{
    var ripAttribute = makeSMJointAttribute(toAttributeId(id));
    ripAttribute.jointType = { "value" : SMJointType.RIP, "canBeEdited": true };
    ripAttribute.jointStyle = { "value" : SMJointStyle.EDGE, "canBeEdited": true };
    // TODO : currently disabled so that I can visualize the walls without offsets.
    // Will be reconsidered when we decide on what we want to do with different rip types.
    // Note that this rip should not affect existing side walls, only offset the flange
    // wall as needed
    /*
    var angle = try(edgeAngle(context, edge));
    if (angle != undefined)
    {
        ripAttribute.angle = {"value" : angle, "canBeEdited" : false};
    }
    */
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

function getOffsetsForSideEdges(offsetDirectionsIn is array, offsetDirectionComputed is Vector, distance is ValueWithUnits)
{
    var defaultOffset = offsetDirectionComputed * distance;
    var offsets = [defaultOffset, defaultOffset];
    if (size(offsetDirectionsIn) != 2)
    {
        return offsets;
    }

    for (var i = 0; i < 2; i += 1)
    {
        var offsetDir = offsetDirectionsIn[i];
        if (offsetDir != undefined)
        {
            offsets[i] = offsetDir * (distance / dot(offsetDirectionComputed, offsetDir));
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

function createAndSolveSketch(context is Context, id is Id, edge is Query, definition is map)
{
    var flangeData = getFlangeData(context, edge, definition);
    var thickness = definition.thickness;

    var distance = definition.distance  - ((0.5 * thickness) / tan(0.5 * definition.bendAngle));
    var offsetDirection = flangeData.direction;
    var offsets = getOffsetsForSideEdges(definition.offsetDirections, offsetDirection, distance);

    var delta = .5 * thickness + CLEARANCE;
    var edgeEndPoints = flangeData.edgeEndPoints;
    var basePoints = [edgeEndPoints[0].origin, edgeEndPoints[1].origin];
    if (definition.moveBase[0])
        basePoints[0] += (delta / dot(offsetDirection, definition.offsetDirections[0])) * edgeEndPoints[0].direction;
    if (definition.moveBase[1])
        basePoints[1] -= (delta / dot(offsetDirection, definition.offsetDirections[1]))* edgeEndPoints[1].direction;

    var sketchPlane = flangeData.plane;
    var points = [  worldToPlane(sketchPlane, basePoints[0]),
                    worldToPlane(sketchPlane, basePoints[1]),
                    worldToPlane(sketchPlane, basePoints[1] + offsets[1]),
                    worldToPlane(sketchPlane, basePoints[0] + offsets[0]) ];
    points = append(points, points[0]);

    var sketch = newSketchOnPlane(context, id, {"sketchPlane" : sketchPlane});
    skPolyline(sketch, "polyline", {"points" : points });
    skSolve(sketch);
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

function findThickness(context is Context, edge is Query) returns ValueWithUnits
{
    var adjacentFace = qEdgeAdjacent(edge, EntityType.FACE);
    if (size(evaluateQuery(context, adjacentFace)) != 1)
    {
        throw regenError("Edge to flange is not laminar");
    }
    var attr = getAttributes(context, {"entities" : qOwnerBody(adjacentFace), "attributePattern" : asSMAttribute({})});

    if (size(attr) != 1 || attr[0].thickness == undefined || attr[0].thickness.value == undefined)
        throw regenError("Bad sheet metal attribute");
    return attr[0].thickness.value;
}

