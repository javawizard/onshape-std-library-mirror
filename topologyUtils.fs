FeatureScript 1311; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/containers.fs", version : "1311.0");
import(path : "onshape/std/context.fs", version : "1311.0");
import(path : "onshape/std/evaluate.fs", version : "1311.0");
import(path : "onshape/std/feature.fs", version : "1311.0");
import(path : "onshape/std/query.fs", version : "1311.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1311.0");
import(path : "onshape/std/vector.fs", version : "1311.0");

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
    var hasNonWireBodies = @size(evaluateQuery(context, nonWireBodies)) > 0;
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
    else if (@size(evaluateQuery(context, wireBodiesQ)) == 0) //there are no wire bodies, return query unchanged
    {
        return query;
    }

    var edges = evaluateQuery(context, dissolveWires(query));
    const edgeCount = size(edges);
    if (edgeCount == 0)
    {
        return query;
    }

    for (var index = 0; index < edgeCount; index += 1)
    {
        if (handleNonWireEdges && hasNonWireBodies)
        {
            //if edge is not from a wire body, dont modify it
            var ownerBody = qBodyType(qOwnerBody(edges[index]), BodyType.WIRE);
            var isOwnerBodyWire = @size(evaluateQuery(context, ownerBody)) > 0;
            if (!isOwnerBodyWire)
                continue;
        }
        var edgePoint = evEdgeTangentLine(context, {
                    "edge" : edges[index],
                    "parameter" : ON_EDGE_TEST_PARAMETER,
                    "arcLengthParameterization" : false
                }).origin;
        const sourceQ = qLaminarDependency(edges[index]);
        if (size(evaluateQuery(context, qContainsPoint(sourceQ, edgePoint))) > 0)
        {
            edges[index] = sourceQ;
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
 * Unlike [constructPath], this function operates on topological connections (underlying connections by a vertex or edge). Distinct
 * bodies are not topologically connected, so even if two entities on distinct bodies are geometrically related by having a
 * coincident vertex or edge, the entities connected to these coincident vertices or edges will fall into different components.
 * Sketch edges are each represented as a distinct wire body, and are not topologically connected, so this method cannot be used for them.
 */
export function connectedComponents(context is Context, entities is Query, adjacencyType is AdjacencyType) returns array
precondition
{
    annotation { "Message" : "Bodies do not share edges or vertices with any other entities" }
    evaluateQuery(context, qEntityFilter(entities, EntityType.BODY)) == [];

    // Vertices are not edge-adjacent to any other entities, so they would always fall into their own group. Instead, disallow this.
    annotation { "Message" : "Entities cannot have AdjacencyType.EDGE with vertices, use AdjacencyType.VERTEX instead" }
    adjacencyType != AdjacencyType.EDGE || evaluateQuery(context, qEntityFilter(entities, EntityType.VERTEX)) == [];
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
 * [sweptAlong] with additional caching in the `faceSweptData`. `faceSweptData` should be a box of a map, or undefined
 * if no caching is desired.
 */
export function sweptAlong(context is Context, face is Query, direction is Vector, faceSweptData) returns boolean
{
    var sweptData = getFaceSweptData(context, face, faceSweptData);
    if (sweptData.planeNormal != undefined)
        return perpendicularVectors(sweptData.planeNormal, direction);
    else if (sweptData.extrudeDirection != undefined)
        return parallelVectors(sweptData.extrudeDirection, direction);

    return false;
}

/**
 * @internal
 * Compute face sweptData as used in [sweptAlong] with additional caching in the `faceSweptData`. `faceSweptData` should be a box of a map, or undefined
 * if no caching is desired.
 */
export function getFaceSweptData(context is Context, face is Query, faceSweptData) returns map
{
    var sweptData = undefined;
    if (faceSweptData != undefined)
    {
        sweptData = faceSweptData[][face];
    }

    if (sweptData == undefined)
    {
        const surface = evSurfaceDefinition(context, {
                "face" : face
        });
        sweptData = {};
        if (surface is Plane)
            sweptData.planeNormal = surface.normal;
        else if (surface is Cylinder)
            sweptData.extrudeDirection = surface.coordSystem.zAxis;
        else if (surface.surfaceType == SurfaceType.EXTRUDED)
            sweptData.extrudeDirection = extrudedSurfaceDirection(context, face);

        if (faceSweptData != undefined)
        {
            faceSweptData[][face] = sweptData;
        }
    }
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

