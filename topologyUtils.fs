FeatureScript 581; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/containers.fs", version : "581.0");
import(path : "onshape/std/context.fs", version : "581.0");
import(path : "onshape/std/evaluate.fs", version : "581.0");
import(path : "onshape/std/feature.fs", version : "581.0");
import(path : "onshape/std/query.fs", version : "581.0");

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
    if (size(evaluateQuery(context, nonWireBodies)) != 0)
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


