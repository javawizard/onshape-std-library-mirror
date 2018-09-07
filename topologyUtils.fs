FeatureScript 901; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/containers.fs", version : "901.0");
import(path : "onshape/std/context.fs", version : "901.0");
import(path : "onshape/std/evaluate.fs", version : "901.0");
import(path : "onshape/std/feature.fs", version : "901.0");
import(path : "onshape/std/query.fs", version : "901.0");

const ON_EDGE_TEST_PARAMETER = 0.37; // A pretty arbitrary number for somewhere along an edge

/**
 * Returns true if edge has two adjacent faces, false if edge is laminar
 */
export function edgeIsTwoSided(context is Context, edge is Query) returns boolean
{
    return size(evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE))) == 2;
}

/**
 * Returns true if edge is closed, false if edge is open
 */
export function isClosed(context is Context, edge is Query) returns boolean
{
    return size(evaluateQuery(context, qVertexAdjacent(edge, EntityType.VERTEX))) < 2;
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
 * Extract a direction from an axis or a plane
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
 * Find connected components in the topological graph of provided edges. Each component is a chain of topologically
 * connected edges, and each component is disjoint with (does not connect topologically with) any other component.
 *
 * Returns an array of components. Each component is an array of individual queries. The queries in any group will respect the
 * query evaluation order of the supplied `edges` [Query]. The components themselves will also be ordered by query evaluation
 * order, sorted by the first edge in each component.
 *
 * Unlike [constructPath], this function operates on topological connections (underlying connections by a vertex). Distinct
 * bodies are not topologically connected, so even if two edges on distinct bodies are geometrically related by having a
 * vertex in the same location, the edges connected to these similar vertices will fall into different components.
 * Notice that wire edges representing sketch curves are not topologically connected, this method cannot be used for them.
 */
export function connectedComponentsOfEdges(context is Context, edges is Query)
{
    var remainingEdges = evaluateQuery(context, qEntityFilter(edges, EntityType.EDGE));

    var groups = [];
    while (remainingEdges != [])
    {
        var group = remainingEdges[0];
        var prevAdded = [remainingEdges[0]];

        while (size(prevAdded) > 0)
        {
            var adjacentEdges = qVertexAdjacent(qUnion(prevAdded), EntityType.EDGE);
            var edgesToAdd = qIntersection([qUnion(remainingEdges), adjacentEdges]);
            prevAdded = evaluateQuery(context, qSubtraction(edgesToAdd, group));

            group = qUnion([group, edgesToAdd]);
        }
        // Intersect the group with the initial edge set to sort by initial query evaluation order
        groups = append(groups, evaluateQuery(context, qIntersection([edges, group])));
        // Subtraction respects evaluation ordering of the first parameter, so remainingEdges stays ordered
        remainingEdges = evaluateQuery(context, qSubtraction(qUnion(remainingEdges), group));
    }
    return groups;
}

