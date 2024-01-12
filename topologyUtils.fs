FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/context.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/query.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");
import(path : "onshape/std/geomOperations.fs", version : "✨");

const ON_EDGE_TEST_PARAMETER = 0.37; // A pretty arbitrary number for somewhere along an edge

/**
 * Returns true if `edge` has two adjacent faces, false if `edge` is a laminar or wire edge.
 */
export function edgeIsTwoSided(context is Context, edge is Query) returns boolean
{
    return size(evaluateQuery(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE))) == 2;
}

/**
 * Returns true if `edge` is closed, false if `edge` is open
 */
export function isClosed(context is Context, edge is Query) returns boolean
{
    return size(evaluateQuery(context, qAdjacent(edge, AdjacencyType.VERTEX, EntityType.VERTEX))) < 2;
}

/**
 *  Returns the union of any edges from the input query and all the edges of any body from the input query
 */
export function dissolveWires(edgesAndWires is Query) returns Query
{
    return qUnion([
                qEntityFilter(edgesAndWires, EntityType.EDGE),
                qOwnedByBody(qEntityFilter(edgesAndWires, EntityType.BODY), EntityType.EDGE)
            ]);
}

/**
 * If the query contains wire edges then this function will track the wire edges back through
 * creation history to find laminar edges that the edge was copied from (or will return the
 * original edge if none).
 */
export function followWireEdgesToLaminarSource(context is Context, query is Query) returns Query
{
    // We only want wire edges and wires
    const bodiesQ = qOwnerBody(query);
    const wireBodiesQ = qBodyType(bodiesQ, BodyType.WIRE);
    const nonWireBodies = qSubtraction(bodiesQ, wireBodiesQ);
    var hasNonWireBodies = !isQueryEmpty(context, nonWireBodies);
    var handleNonWireEdges = isAtVersionOrLater(context, FeatureScriptVersionNumber.V588_MERGE_IN_FILLET_FIX);
    if (!handleNonWireEdges)
    {
        // before the fix, return query unchanged if there are any laminar edges in query
        // (even if it contained some edges from wire bodies, e.g. for fill surface)
        if (hasNonWireBodies)
        {
            return query;
        }
    }
    else if (isQueryEmpty(context, wireBodiesQ)) //there are no wire bodies, return query unchanged
    {
        return query;
    }

    var edges = evaluateQuery(context, dissolveWires(query));
    const edgeCount = size(edges);
    if (edgeCount == 0)
    {
        return query;
    }
    var doCoincidenceCheck = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1890_FOLLOW_LAMINAR_SOURCE_COINCIDENCE_CHECK);
    for (var index = 0; index < edgeCount; index += 1)
    {
        if (handleNonWireEdges && hasNonWireBodies)
        {
            //if edge is not from a wire body, dont modify it
            var ownerBody = qBodyType(qOwnerBody(edges[index]), BodyType.WIRE);
            var isOwnerBodyWire = !isQueryEmpty(context, ownerBody);
            if (!isOwnerBodyWire)
                continue;
        }
        const sourceQ = qLaminarDependency(edges[index]);
        if (isQueryEmpty(context, sourceQ))
        {
            continue;
        }
        if (doCoincidenceCheck)
        {
            const coincidentEdges = qCoincidentFilter(sourceQ, edges[index]);
            if (size(evaluateQuery(context, coincidentEdges)) == 1)
            {
                edges[index] = coincidentEdges;
            }
        }
        else
        {
            var edgePoint = evEdgeTangentLine(context, {
                        "edge" : edges[index],
                        "parameter" : ON_EDGE_TEST_PARAMETER,
                        "arcLengthParameterization" : false
                    }).origin;

            if (!isQueryEmpty(context, qContainsPoint(sourceQ, edgePoint)))
            {
                edges[index] = sourceQ;
            }
        }
    }
    return qUnion(edges);
}

/**
 * Extract a direction from an axis or a plane.  Useful for processing query parameters with the
 * [QueryFilterCompound.ALLOWS_DIRECTION](/FsDoc/library.html#QueryFilterCompound) filter.
 * @return: a 3D unit [Vector] if a direction can be extracted, otherwise `undefined`.
 */
export function extractDirection(context is Context, entity is Query)
{
    try silent
    {
        return evAxis(context, { "axis" : entity }).direction;
    }
    try silent
    {
        return evPlane(context, {"face" : entity}).normal;
    }
    return undefined;
}

/**
 * Find connected components in the topological graph of provided entities. Each component is a group of topologically
 * connected entities, and each component is disjoint with (does not connect topologically with) any other component.
 * Connectivity is tested using [qAdjacent] with the specified `adjacencyType`.
 *
 * Returns an array of components. Each component is an array of individual queries. The queries in any component will respect the
 * query evaluation order of the supplied `entities` [Query]. The components themselves will also be ordered by query evaluation
 * order, sorted by the first entity in each component.
 *
 * Unlike [constructPaths], this function operates on topological connections (underlying connections by a vertex or edge). Distinct
 * bodies are not topologically connected, so even if two entities on distinct bodies are geometrically related by having a
 * coincident vertex or edge, the entities connected to these coincident vertices or edges will fall into different components.
 * Sketch edges are each represented as a distinct wire body, and are not topologically connected, so this method cannot be used for them.
 */
export function connectedComponents(context is Context, entities is Query, adjacencyType is AdjacencyType) returns array
precondition
{
    annotation { "Message" : "Bodies do not share edges or vertices with any other entities" }
    isQueryEmpty(context, qEntityFilter(entities, EntityType.BODY));

    // Vertices are not edge-adjacent to any other entities, so they would always fall into their own group. Instead, disallow this.
    annotation { "Message" : "Entities cannot have AdjacencyType.EDGE with vertices, use AdjacencyType.VERTEX instead" }
    adjacencyType != AdjacencyType.EDGE || isQueryEmpty(context, qEntityFilter(entities, EntityType.VERTEX));
}
{
    var groups = [];
    var remainingEntities = evaluateQuery(context, entities);
    while (remainingEntities != [])
    {
        var group = [remainingEntities[0]];
        var prevAdded = [remainingEntities[0]];

        while (size(prevAdded) > 0)
        {
            var adjacentEntities = qAdjacent(qUnion(prevAdded), adjacencyType);
            var edgesToAdd = qIntersection([qUnion(remainingEntities), adjacentEntities]);
            prevAdded = evaluateQuery(context, qSubtraction(edgesToAdd, qUnion(group)));

            group = append(group, qUnion(prevAdded));
        }
        // Intersect the group with the initial edge set to sort by initial query evaluation order
        groups = append(groups, evaluateQuery(context, qIntersection([entities, qUnion(group)])));
        // Subtraction respects evaluation ordering of the first parameter, so remainingEntities stays ordered
        remainingEntities = evaluateQuery(context, qSubtraction(qUnion(remainingEntities), qUnion(group)));
    }
    return groups;
}

/** @internal */
annotation { "Deprecated" : "[connectedComponentsOfEdges] has been replaced by [connectedComponents] with `AdjacencyType.VERTEX`" }
export function connectedComponentsOfEdges(context is Context, edges is Query) returns array
{
    var edgesFiltered = qEntityFilter(edges, EntityType.EDGE);
    return connectedComponents(context, edgesFiltered, EntityType.VERTEX);
}

/**
 * Group `entities` by their owner body.
 * @return: a map from the transient query for an individual body to an array of transient queries for the individual entities
 * which belong to that body.
 */
export function groupEntitiesByBody(context is Context, entities is Query) returns map
{
    var bodyToEntities = {};
    for (var entity in evaluateQuery(context, entities))
    {
        const body = evaluateQuery(context, qOwnerBody(entity))[0];
        if (bodyToEntities[body] != undefined)
            bodyToEntities[body] = append(bodyToEntities[body], entity);
        else
            bodyToEntities[body] = [entity];
    }
    return bodyToEntities;
}

/**
 * Check whether a face is swept along a specified direction.
 */
export function sweptAlong(context is Context, face is Query, direction is Vector) returns boolean
{
    return sweptAlong(context, face, direction, undefined);
}

/**
 * @internal
 * sweptAlong but use the passed in cache for getting face swept data if possible. `faceSweptDataCache` should be made from
 * [makeFaceSweptDataCache] or be undefined if no caching is desired.
 */
export function sweptAlong(context is Context, face is Query, direction is Vector, faceSweptDataCache) returns boolean
{
    var sweptData = faceSweptDataCache != undefined ? faceSweptDataCache(face) : getFaceSweptData(context, face);
    if (sweptData.planeNormal != undefined)
        return perpendicularVectors(sweptData.planeNormal, direction);
    else if (sweptData.extrudeDirection != undefined)
        return parallelVectors(sweptData.extrudeDirection, direction);

    return false;
}

/**
 * @internal
 * Create a cache for sweptFaceData that can be used to efficiently retrieve the
 * face swept data as keyed by the face query itself.
 */
export function makeFaceSweptDataCache(context is Context) returns function
{
    return memoizeFunction(function(face)
    {
        return getFaceSweptData(context, face);
    });
}

/**
 * @internal
 * Compute face sweptData as used in [sweptAlong]. If caching is required, please use
 * [makeFaceSweptDataCache].
 */
function getFaceSweptData(context is Context, face is Query) returns map
{
    var sweptData = {};
    const surface = evSurfaceDefinition(context, {
                "face" : face
        });
    if (surface is Plane)
        sweptData.planeNormal = surface.normal;
    else if (surface is Cylinder)
        sweptData.extrudeDirection = surface.coordSystem.zAxis;
    else if (surface.surfaceType == SurfaceType.EXTRUDED)
        sweptData.extrudeDirection = extrudedSurfaceDirection(context, face);
    return sweptData;
}

function extrudedSurfaceDirection(context is Context, face is Query)
{
    //EXTRUDED surface always has a curve along u direction and linear component along v direction
    const planes = evFaceTangentPlanes(context, {
            "face" : face,
            "parameters" : [ vector(0.5, 0.), vector(0.5, 1) ]
    });

    return normalize(planes[1].origin - planes[0].origin);
}

/**
 * Groups bodies into clusters of identical topology and identical geometry (up to a relative tolerance), allowing
 * arbitrary 3D rotations (but not reflection).
 * @param definition {{
 *      @field bodies {Query} : The bodies to cluster
 *      @field relativeTolerance {number} : A tolerance, expressed as a decimal value, to compare bodies with.
 *                                          @eg `0.01` to cluster bodies that have a 1% similarity
 * }}
 */
export function clusterBodies(context is Context, definition is map) returns array
{
    return @clusterBodies(context, definition);
}

function getTerminalNonMeshFace(context is Context, topologies is Query, firstFace is boolean)
{
    const nonMeshFaces = evaluateQuery(context, qMeshGeometryFilter(qEntityFilter(topologies, EntityType.FACE), MeshGeometry.NO));
    if (nonMeshFaces == [])
    {
        return qNothing();
    }
    else
    {
        return nonMeshFaces[firstFace ? 0 : size(nonMeshFaces) - 1];
    }
}

/**
 * @internal
 * Returns the last non-mesh face of a query.
 * When using evFaceTangentPlane to create a manipulator, we want it to be on the last non-mesh face the user selected.
 * This is not bundled with actual manipulator creation because some features (eg. Move Face) modify the plane before creating the manipulator.
 */
export function getLastNonMeshFace(context is Context, topologies is Query) returns Query
{
    return getTerminalNonMeshFace(context, topologies, false);
}

/**
 * @internal
 * Returns the first non-mesh face of a query.
 * When dealing with mesh and non-mesh faces, we should prioritize non-mesh faces if any.
 */
export function getFirstNonMeshFace(context is Context, topologies is Query) returns Query
{
    return getTerminalNonMeshFace(context, topologies, true);
}

/** @internal */
export function getAdjacentFacesOfWireProfiles(context is Context, profileQ is Query,  hiddenBodies is Query) returns Query
{
    var faces = [];
    for (var edge in evaluateQuery(context, profileQ))
    {
        var laminarEdgesQ = qLaminarDependency(edge);
        const edgeTangents = evEdgeTangentLines(context, {
                    "edge" : edge,
                    "parameters" : [0.1, 0.5, 0.8]
            });
        //For laminar edges selected or recovered through dependency
        if (!isQueryEmpty(context, laminarEdgesQ))
        {
            laminarEdgesQ = filterByCoincidence(laminarEdgesQ, edgeTangents);
        }
        //For wire edges look for derived laminar edges
        if (isQueryEmpty(context, laminarEdgesQ) && !isQueryEmpty(context, edge->qBodyType(BodyType.WIRE)))
        {
            const tracking = startTracking(context,
                    {"subquery" : edge, "trackPartialDependency" : true, "lastOperationId" : lastModifyingOperationId(context, edge) });
            var laminarEdgesQ = tracking->qEntityFilter(EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.ONE_SIDED);
            laminarEdgesQ = filterByCoincidence(laminarEdgesQ, edgeTangents);
            laminarEdgesQ = qSubtraction(laminarEdgesQ, qOwnedByBody(hiddenBodies, EntityType.EDGE));
        }
        if (isQueryEmpty(context, laminarEdgesQ))
            continue;
        const adjacentFaceQ = qAdjacent(laminarEdgesQ, AdjacencyType.EDGE, EntityType.FACE);
        // only use the adjacent face if it is unambiguous
        if (size(evaluateQuery(context, adjacentFaceQ)) == 1)
        {
            faces = append(faces, adjacentFaceQ);
        }
    }
    return qUnion(faces);
}

function filterByCoincidence(edgeQ is Query, edgeTangents is array) returns Query
{
    var out = edgeQ;
    for (var tangent in edgeTangents)
    {
        out = qContainsPoint(out, tangent.origin);
    }
    return out;
}

/**
 * @internal
 * Returns the flat regions achieved by offsetting  the initial edge set
 * Offset direction of the single edge depends on its curve direction
 * If edges are chained then the initial offset direction is defined by the direction of the first selected edge
 * But if edges are from a closed loop then the default offset direction points  outward this loop
 * The function is meant to be used to support of "Thin" option of "Extrude", "Revolve", and "Sweep" tools.
 */
export function getThinWallRegions(context is Context, id is Id, definition is map) returns Query
{
    const useAxisCoPlane = definition.revolveData != undefined;
    if (useAxisCoPlane)
    {
        return createRegionsOnCoplanes(context, id, definition);
    }
    return createSeparateRegions(context, id, definition);
}

function createRegionsOnCoplanes(context is Context, id is Id, definition is map) returns Query
{
    var wallRegions = [];
    var planeId = 0;
    for (var commonPlane, edgeSet in definition.revolveData)
    {
        planeId += 1;

        //apply offset to coplanar edges
        callSubfeatureAndProcessStatus(id, opOffsetWire, context, id + "getWallShape" + unstableIdComponent(planeId), {
                    "edges" : edgeSet,
                    "offset1" : definition.midplane != true ? definition.thickness1 : definition.thickness / 2,
                    "offset2" : definition.midplane != true ? definition.thickness2 : definition.thickness / 2,
                    "flip" : definition.flipWall,
                    "normal" : commonPlane.normal,
                    "makeRegions" : true });

        //store regions
        wallRegions = append(wallRegions, qCreatedBy(id + "getWallShape" + unstableIdComponent(planeId), EntityType.FACE));
    }
    return qUnion(wallRegions);
}

function createSeparateRegions(context is Context, id is Id, definition is map) returns Query
{
    const useCommonDirection = definition.commonDirection != undefined;

    var planeId = 0;
    var wallRegions = [];
    var shapeProvider = definition.entities;

    while (!isQueryEmpty(context, shapeProvider))
    {
        planeId += 1;

        //Get next unprocessed shape
        const nextShape = qNthElement(shapeProvider, 0);

        //Get common plane
        const commonPlane = getCommonPlane(context, definition, nextShape, useCommonDirection);

        //collect coplanar edges
        const coplanarEdges = qCoincidesWithPlane(shapeProvider, commonPlane);

        //Check if coplanar edges been selected
        if (useCommonDirection && isQueryEmpty(context, coplanarEdges))
        {
            throw regenError("Selected entities should lay in parallel planes");
        }

        //remove selected edges from provider
        shapeProvider = qSubtraction(shapeProvider, coplanarEdges);

        //apply offset to coplanar edges
        callSubfeatureAndProcessStatus(id, opOffsetWire, context, id + "getWallShape" + unstableIdComponent(planeId), {
                  "edges"       : coplanarEdges,
                  "offset1"     : definition.midplane != true ? definition.thickness1 : definition.thickness / 2,
                  "offset2"     : definition.midplane != true ? definition.thickness2 : definition.thickness / 2,
                  "flip"        : definition.flipWall,
                  "normal"      : commonPlane.normal,
                  "makeRegions" : true });

        //store regions
        wallRegions = append(wallRegions, qCreatedBy(id + "getWallShape" + unstableIdComponent(planeId), EntityType.FACE));
    }
    return qUnion(wallRegions);
}

function getCommonPlane(context is Context, definition is map, shape is Query, useCommonDirection is boolean) returns Plane
{
    //Get plane where it is lay: try sketch first than just plane.
    var commonPlane = try silent(evOwnerSketchPlane(context, {"entity" : shape}));

    if (!(commonPlane is Plane))
    {
        commonPlane = evPlanarEdge(context, {
                    "edge" : shape
                });
        if (useCommonDirection)
        {
            commonPlane.normal = definition.commonDirection;
        }
    }
    return commonPlane;
}
