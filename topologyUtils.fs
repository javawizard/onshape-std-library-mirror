FeatureScript 638; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/containers.fs", version : "638.0");
import(path : "onshape/std/context.fs", version : "638.0");
import(path : "onshape/std/evaluate.fs", version : "638.0");
import(path : "onshape/std/feature.fs", version : "638.0");
import(path : "onshape/std/query.fs", version : "638.0");

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

