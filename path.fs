FeatureScript 2022; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/context.fs", version : "2022.0");
export import(path : "onshape/std/query.fs", version : "2022.0");
export import(path : "onshape/std/units.fs", version : "2022.0");

// Imports used internally
import(path : "onshape/std/box.fs", version : "2022.0");
import(path : "onshape/std/containers.fs", version : "2022.0");
import(path : "onshape/std/debug.fs", version : "2022.0");
import(path : "onshape/std/evaluate.fs", version : "2022.0");
import(path : "onshape/std/feature.fs", version : "2022.0");
import(path : "onshape/std/mathUtils.fs", version : "2022.0");
import(path : "onshape/std/topologyUtils.fs", version : "2022.0");
import(path : "onshape/std/valueBounds.fs", version : "2022.0");

/**
 * Represents a series of connected edges which form a continuous path.
 * @type {{
 *      @field edges {array}: The edges that form this Path, in the order of the Path.
 *      @field flipped {array}: An array of booleans corresponding to each edge in the path, set to `true` to traverse
 *          the edge backwards.
 *      @field closed {boolean}: Whether the Path is a closed path.
 *      @field adjacentFaces {Query}: @optional All adjacent faces on one side of the Path.
 * }}
 */
export type Path typecheck canBePath;

/** Typecheck for [Path] */
export predicate canBePath(value)
{
    value is map;

    value.edges is array;
    for (var edge in value.edges)
    {
        edge is Query;
    }

    value.flipped is array;
    for (var direction in value.flipped)
    {
        direction is boolean;
    }

    value.closed is boolean;

    value.adjacentFaces == undefined || value.adjacentFaces is Query ;
}

/**
 * Distance information returned by `constructPath` and `evPathTangentLines` when either function is provided with `referenceGeometry`
 * @type {{
 *      @field distance {ValueWithUnits} : The distance between the the start of the [Path] and the center of the
 *          bounding box of `referenceGeometry`, or infinity if `referenceGeometry` is empty
 *      @field withinBoundingBox {boolean} : Whether the start of the [Path] is within the bounding box of
 *          `referenceGeometry`, or `false` if `referenceGeometry` is empty
 * }}
 */
export type PathDistanceInformation typecheck canBePathDistanceInformation;

 /** Typecheck for [PathDistanceInformation] */
export predicate canBePathDistanceInformation(value)
{
    value is map;
    value.distance is ValueWithUnits;
    value.withinBoundingBox is boolean;
}

/**
 * Returns a PathDistanceInformation with `distance` set to infinity and `withinBoundingBox` set to false
 */
export function defaultPathDistanceInformation() returns PathDistanceInformation
{
    return { "distance" : inf * meter, "withinBoundingBox" : false } as PathDistanceInformation;
}

/**
 * Reverse the direction of a [Path]
 *
 * @param path {Path}: the [Path] to reverse.
 */
export function reverse(path is Path) returns Path
{
    path.edges = reverse(path.edges);
    path.flipped = reverse(path.flipped);
    path.flipped = mapArray(path.flipped, function(flipped)
        {
            return !flipped;
        });

    return path;
}

/**
 * @internal
 * Construct an array of [Path]s from a [Query] of edges.
 *
 * @param context {Context}
 * @param edgesQuery {Query}: A [Query] of edges to form into a [Path].
 * @param options {{
 *      @field adjacentSeedFaces {Query}: @optional If adjacent faces to the path are provided, each [Path] returned will
 *           include an `adjacentFaces` property that has all faces on the same side of the path as the seed faces. If there are seed
 *           faces on both sides, constructPaths will error.
 * }}
 */
export function constructPaths(context is Context, edgesQuery is Query, options is map) returns array
{
    var paths = @constructPaths(context, { "edges" : edgesQuery, "seedFaces" : options.adjacentSeedFaces });
    for (var j = 0; j < paths->size(); j += 1)
    {
        var path = paths[j];
        for (var i = 0; i < path.edges->size(); i += 1)
        {
            path.edges[i] = qTransient(path.edges[i]);
        }
        if (path.adjacentFaces != undefined)
        {
            for (var i = 0; i < path.adjacentFaces->size(); i += 1)
            {
                path.adjacentFaces[i] = qTransient(path.adjacentFaces[i]);
            }
            path.adjacentFaces = qUnion(path.adjacentFaces);
        }

        paths[j] = path as Path;
    }
    return paths;
}

/**
 * Construct a [Path] from a [Query] of edges, picking the starting point of the path based on query evaluation order for `edgesQuery`
 *
 * @param context {Context}
 * @param edgesQuery {Query}: A [Query] of edges to form into a [Path]. The edges are ordered with query evaluation
 *      order, so a [qUnion] should be used to ensure a stable ordering.
 */
export function constructPath(context is Context, edgesQuery is Query) returns Path
{
    var constructPathResult = constructPath(context, edgesQuery, qNothing());
    return constructPathResult.path;
}

/**
 * @internal Explicitly ensure old calls with (Context, Query, Query) don't get redirected straight to (Context, Query, map)
 */
annotation { "Deprecated" : "Use [constructPath(Context, Query, map)]" }
export function constructPath(context is Context, edgesQuery is Query, referenceGeometry is Query) returns map
{
    return constructPath(context, edgesQuery, {
        "referenceGeometry" : referenceGeometry
    });
}

/** @internal */
annotation { "Deprecated" : "Use [constructPath(Context, Query, map)]" }
export function constructPath(context is Context, edgesQuery is Query, referenceGeometry is undefined) returns map
{
    return constructPath(context, edgesQuery, {
        "referenceGeometry" : referenceGeometry
    });
}

/**
 * Construct a [Path] from a [Query] of edges, optionally picking the starting point as the closest viable starting point to the
 * supplied `referenceGeometry`
 *
 * @param context {Context}
 * @param edgesQuery {Query}: A [Query] of edges to form into a [Path].
 * @param options {{
 *      @field referenceGeometry: @optional A geometry [Query] to determine the start of the [Path]. If unspecified,
 *          (or the query is empty) the starting point of the path will be based on query evaluation order for `edgesQuery`.
 *      @field tolerance {ValueWithUnits}: @optional Tolerance with length units indicating how close endpoints need
 *          to be to be considered part of the same path. Default is `1e-8 * meter`
 *          @eg `1e-5 * meter`
 * }}
 * @return {{
 *      @field path {Path} : The resulting constructed [Path]
 *      @field pathDistanceInformation {PathDistanceInformation} : A map containing the distance from the [Path] start
 *          point to the center of the bounding box of `referenceGeometry` and whether the [Path] start point falls
 *          inside that bounding box.
 * }}
 */
export function constructPath(context is Context, edgesQuery is Query, options is map) returns map
precondition
{
    options.referenceGeometry is Query || options.referenceGeometry == undefined;
    isLength(options.tolerance, NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS) || options.tolerance == undefined;
}
{
    var edges is array = evaluateQuery(context, edgesQuery);

    if (size(edges) == 0)
    {
        throw "Cannot form path with no edges.";
    }

    var referenceGeometryProvided = checkReferenceGeometryProvided(context, options.referenceGeometry);

    if (size(edges) == 1 && !referenceGeometryProvided)
    {
        return {
                "path" : ({ "edges" : edges, "flipped" : [false], "closed" : isClosed(context, edgesQuery) } as Path),
                "pathDistanceInformation" : defaultPathDistanceInformation()
            };
    }

    var graphInformation = computeGraphInformation(context, edges, referenceGeometryProvided, options);
    var startPointIndex = graphInformation.start;
    var pointIndexToGroup = graphInformation.pointIndexToGroup;
    var pathClosed = graphInformation.closed;

    // To find the path:
    // Maintain a current group (a group represents many geometric vertices meeting at one graph vertex) and a stack of
    // geometric vertices. Until we have completed the path, do the following:
    // a) If the current group has no more available outgoing edges, pop the stack to find which edges lead to this group,
    //    and add that edge to the end of the path. Set the current group to the group at the starting vertex of the edge.
    // b) If the current group has available outgoing edges, pick one of them (in Query evaluation order) and push the
    //    vertex at the end of that edge onto the stack. Set the group to the group of the same vertex.

    // current group and start of the next outgoing edge
    var currGroup = pointIndexToGroup[startPointIndex];
    var nextPointIndex = startPointIndex;

    // store whether points have been used to know which outgoing edges are available
    var pointsUsed = makeArray(size(edges) * 2, false);

    // stack of edges
    var edgeStack = makeArray(size(edges));
    var stackPushIndex = 0;

    // edges are added from last to first
    var edgeAddIndex = size(edges) - 1;
    var orderedEdges = makeArray(size(edges));
    var edgesFlipped = makeArray(size(edges));

    while (size(currGroup) > 0)
    {
        // no unvisisted neighbors in current group
        if (nextPointIndex == -1)
        {
            if (stackPushIndex > 0) // there are more edges in the stack
            {
                stackPushIndex = stackPushIndex - 1;
                var edgeEndIndex = edgeStack[stackPushIndex];

                var flip = edgeEndIndex % 2 == 0;
                var edgeStartIndex = flip ? edgeEndIndex + 1 : edgeEndIndex - 1;

                orderedEdges[edgeAddIndex] = floor(edgeEndIndex / 2);
                edgesFlipped[edgeAddIndex] = flip;
                edgeAddIndex = edgeAddIndex - 1;

                currGroup = pointIndexToGroup[edgeStartIndex];
            }
            else // the stack is empty
            {
                currGroup = [];
            }
        }
        else
        {
            var otherPointIndex = nextPointIndex % 2 == 0 ? nextPointIndex + 1 : nextPointIndex - 1;

            pointsUsed[nextPointIndex] = true;
            pointsUsed[otherPointIndex] = true;

            currGroup = pointIndexToGroup[otherPointIndex];
            edgeStack[stackPushIndex] = otherPointIndex;
            stackPushIndex = stackPushIndex + 1;
        }

        nextPointIndex = getAvailablePointInGroup(currGroup, pointsUsed);
    }

    // error if the attempted Path consists of multiple unconnected cycles
    if (edgeAddIndex != -1)
    {
        throw "Edges do not form a continuous path.";
    }

    // remap ordered edges to hold edges rather than edge indicies
    orderedEdges = mapArray(orderedEdges, function(edgeIndex)
        {
           return edges[edgeIndex];
        });

    var path = { "edges" : orderedEdges, "flipped" : edgesFlipped, "closed" : pathClosed } as Path;
    return { "path" : path, "pathDistanceInformation" : graphInformation.pathDistanceInformation };
}

/**
 * Compute and return the start index of the path, the groups of indices that form vertices in the graph, whether
 * the path is closed, and the path distance information (as defined in [PathDistanceInformation])
 */
function computeGraphInformation(context is Context, edges is array, referenceGeometryProvided is boolean, options is map) returns map
{
    var pathPoints = [];
    for (var edge in edges)
    {
        var endpoints = evEdgeTangentLines(context, { "edge" : edge, "parameters" : [0, 1] });
        pathPoints = append(pathPoints, endpoints[0].origin);
        pathPoints = append(pathPoints, endpoints[1].origin);
    }

    if (options.tolerance == undefined)
    {
        options.tolerance = TOLERANCE.zeroLength * meter;
    }
    var pointGroups = clusterPoints(pathPoints, options.tolerance);

    var oddNumberedGroups = []; // array of point vectors
    var pointIndexToGroup = {}; // map: endpoint -> group in pointGroups

    var possibleStartPoints = [];
    var possibleStartPointsIndexToPathPointsIndex = {};

    for (var group in pointGroups)
    {
        var isOdd = size(group) % 2 == 1;

        if (isOdd)
        {
            oddNumberedGroups = append(oddNumberedGroups, pathPoints[group[0]]);
        }

        for (var point in group)
        {
            pointIndexToGroup[point] = group;
            if (isOdd)
            {
                possibleStartPointsIndexToPathPointsIndex[size(possibleStartPoints)] = point;
                possibleStartPoints = append(possibleStartPoints, pathPoints[point]);
            }
        }
    }

    var pathClosed = false;

    if (size(oddNumberedGroups) != 0 && size(oddNumberedGroups) != 2)
    {
        for (var point in oddNumberedGroups)
        {
            addDebugPoint(context, point);
        }
        throw "Edges do not form a continuous path.";
    }
    else if (size(oddNumberedGroups) == 0)
    {
        pathClosed = true;
        possibleStartPoints = pathPoints;
    }

    // decide on the starting point
    var startPointIndex;
    var pathDistanceInformation;
    if (referenceGeometryProvided)
    {
        var heuristic = computeDistanceHeuristic(context, possibleStartPoints, options.referenceGeometry);
        var distanceResult = heuristic.distanceResult;
        pathDistanceInformation = heuristic.pathDistanceInformation;

        startPointIndex = distanceResult.sides[0].index;
        if (!pathClosed)
        {
            startPointIndex = possibleStartPointsIndexToPathPointsIndex[startPointIndex];
        }
    }
    else
    {
        pathDistanceInformation = defaultPathDistanceInformation();

        if (pathClosed)
        {
            // pick the start of the first edge as the starting point unless the start touches the second edge,
            // then use the end of the first edge as the starting point
            startPointIndex = 0;
            if (isIn(2, pointIndexToGroup[0]) || isIn(3, pointIndexToGroup[0]))
            {
                startPointIndex = 1;
            }
        }
        else
        {
            // take the first possible start point
            startPointIndex = possibleStartPointsIndexToPathPointsIndex[0];
        }
    }

    return { "start" : startPointIndex, "pointIndexToGroup" : pointIndexToGroup, "closed" : pathClosed, "pathDistanceInformation" : pathDistanceInformation };
}

/**
 * Return the first available point index in a group.
 */
function getAvailablePointInGroup(group is array, pointsUsed is array) returns number
{
    for (var point in group)
    {
        if (!pointsUsed[point])
        {
            return point;
        }
    }

    return -1;
}

/**
 * @param context {Context}
 * @param path {Path}
 *
 * @returns {ValueWithUnits}: The total length of the [Path].
 */
export function evPathLength(context is Context, path is Path) returns ValueWithUnits
{
    return evLength(context, { "entities" : qUnion(path.edges) });
}

/**
 * Return tangent lines to a [Path] using arc length parameterization.
 *
 * @param context {Context}
 * @param path {Path}: The [Path] to use.
 * @param parameters {array}: An array of numbers in the range 0..1 indicating where along the [Path] to evaluate tangents.
 *
 * @returns {{
 *      @field tangentLines {array}: The tangent [Line]s corresponding to each parameter
 *      @field edgeIndices {array}: The indices of the edges in the [Path] corresponding to each parameter
 * }}
 */
export function evPathTangentLines(context is Context, path is Path, parameters is array) returns map
{
    var evPathTangentLinesResult = evPathTangentLines(context, path, parameters, qNothing());
    return {"tangentLines" : evPathTangentLinesResult.tangentLines, "edgeIndices" : evPathTangentLinesResult.edgeIndices};
}

/**
 * Return tangent lines to a [Path] using arc length parameterization. By default, the 0 parameter of the [Path] will be the
 * start of the [Path]. If evaluating a closed path, providing reference geometry will alter the 0 parameter to be the
 * closest point on the [Path] to the reference geometry. Providing reference geometry for a non-closed [Path] will not
 * change the parameterization of the [Path]
 *
 * @param context {Context}
 * @param path {Path}: The [Path] to use.
 * @param parameters {array}: An array of numbers in the range 0..1 indicating where along the [Path] to evaluate tangents.
 * @param referenceGeometry: A geometry [Query] to determine the 0 parameter of the [Path], or `undefined`. If an empty
 *      [Query] or `undefined` is specified, the tangents will be evaluated starting at the beginning of the path.
 *
 * @returns {{
 *      @field tangentLines {array}: The tangent [Line]s corresponding to each parameter
 *      @field edgeIndices {array}: The indices of the edges in the [Path] corresponding to each parameter
 *      @field pathDistanceInformation {PathDistanceInformation} : A map containing the distance from the remapped 0
 *          parameter to the center of the bounding box of `referenceGeometry` and whether the remapped 0 parameter falls
 *          inside that bounding box. Only valid if the path is closed.
 * }}
 */
export function evPathTangentLines(context is Context, path is Path, parameters is array, referenceGeometry) returns map
precondition
{
    referenceGeometry is Query || referenceGeometry == undefined;
}
{
    var totalLength is ValueWithUnits = evPathLength(context, path);

    // get the parameterization of each endpoint along the path
    var pointParameters = [0];
    for (var i = 0; i < size(path.edges) - 1; i += 1)
    {
        var length = evLength(context, { "entities" : path.edges[i] });
        pointParameters = append(pointParameters, pointParameters[i] + (length / totalLength));
    }
    pointParameters = append(pointParameters, 1);

    var pathDistanceInformation = defaultPathDistanceInformation();
    if (path.closed && checkReferenceGeometryProvided(context, referenceGeometry))
    {
        var heuristic = computeDistanceHeuristic(context, qUnion(path.edges), referenceGeometry);
        var distanceResult = heuristic.distanceResult;
        pathDistanceInformation = heuristic.pathDistanceInformation;

        var closestEdge = distanceResult.sides[0].index;
        // percentage of the total path that the closest edge occupies
        var closestEdgeFractionOfPath = pointParameters[closestEdge + 1] - pointParameters[closestEdge];

        // parameterization on the closest edge where the closest point falls
        var fractionOfEdge = distanceResult.sides[0].parameter;
        fractionOfEdge = path.flipped[closestEdge] ? 1 - fractionOfEdge : fractionOfEdge;

        // remapped start is (start of closest edge) + (length of closest edge * parameter of closest point)
        var newStartParameter = pointParameters[closestEdge] + (closestEdgeFractionOfPath * fractionOfEdge);

        for (var i = 0; i < size(parameters); i += 1)
        {
            parameters[i] = (parameters[i] + newStartParameter) % 1.0;
        }
    }

    var edgeIndices = [];
    var tangentLines = [];
    var edgeIndex;
    for (var i = 0; i < size(parameters); i += 1)
    {
        var parameter = parameters[i];

        if (parameter < 0 || parameter > 1)
            throw parameter ~ " does not fall between 0 and 1.";

        // find the edge that this parameter falls on
        if (i == 0 || (parameters[i - 1] > parameter))
        {
            edgeIndex = 0;
        }
        while (pointParameters[edgeIndex + 1] < parameter)
        {
            edgeIndex += 1;
        }

        var fractionOfEdge = (parameter - pointParameters[edgeIndex]) / (pointParameters[edgeIndex + 1] - pointParameters[edgeIndex]);
        var parameterOnEdge = path.flipped[edgeIndex] ? 1 - fractionOfEdge : fractionOfEdge;

        var tangentLine = evEdgeTangentLine(context, { "edge" : path.edges[edgeIndex], "parameter" : parameterOnEdge });
        tangentLine.direction = path.flipped[edgeIndex] ? -tangentLine.direction : tangentLine.direction;

        tangentLines = append(tangentLines, tangentLine);
        edgeIndices = append(edgeIndices, edgeIndex);
    }
    return { "tangentLines" : tangentLines, "edgeIndices" : edgeIndices, "pathDistanceInformation" : pathDistanceInformation };
}

/**
 *  @return whether referenceGeometry contains geometry (`true`) or is empty (`false`)
 */
function checkReferenceGeometryProvided(context is Context, referenceGeometry) returns boolean
{
    // reference geometry is absent if referenceGeometry is an empty query or undefined
    var isEmpty = ((referenceGeometry is Query) && isQueryEmpty(context, referenceGeometry)) ||
                  (referenceGeometry == undefined);

    return !isEmpty;
}

/**
 * Take the distance between some path geometry and the center of the bounding box of the reference geometry; return
 * the relevant heuristic information.
 * @return {{
 *      @field distanceResult {DistanceResult}
 *      @field pathDistanceInformation {PathDistanceInformation}
 * }}
 */
function computeDistanceHeuristic(context is Context, pathGeometry, referenceGeometry is Query) returns map
{
    var boundingBox  = evBox3d(context, { "topology" : referenceGeometry, "tight" : true });
    var center = box3dCenter(boundingBox);

    // Before V947 we were assuming that evDistance returned arc length parameters when it was not
    const arcLengthForEvDistance = isAtVersionOrLater(context, FeatureScriptVersionNumber.V947_EVDISTANCE_ARCLENGTH);
    var distanceResult = evDistance(context, {
                "side0" : pathGeometry,
                "side1" : center,
                "arcLengthParameterization" : arcLengthForEvDistance
            });

    var pathDistanceInformation = {
                "distance" : distanceResult.distance,
                "withinBoundingBox" : insideBox3d(distanceResult.sides[0].point, boundingBox)
            } as PathDistanceInformation;

    return {"distanceResult" : distanceResult, "pathDistanceInformation" : pathDistanceInformation};
}

/**
 * Return query to end vertices of path if open or [qNothing] if closed.
 *
 * @param context {Context}
 * @param path {Path}: The [Path] to use.
 *
 * @returns {Query}
 */
export function getPathEndVertices(context is Context, path is Path) returns Query
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1585_HOLE_NEW_PIPELINE))
    {
        return getPathEndVerticesNotTopologicallyConnected(context, path);
    }
    const size = path.edges->size();
    if (size == 0 || path.closed)
    {
        return qNothing();
    }
    const startVertices = qAdjacent(path.edges[0], AdjacencyType.VERTEX, EntityType.VERTEX);
    const endVertices = qAdjacent(path.edges[size - 1], AdjacencyType.VERTEX, EntityType.VERTEX);
    if (size == 1)
    {
        return startVertices;
    }
    else if (size == 2)
    {
        return qSubtraction(qUnion([startVertices, endVertices]), qIntersection([startVertices, endVertices]));
    }
    else
    {
        const secondVertices = qAdjacent(path.edges[1], AdjacencyType.VERTEX, EntityType.VERTEX);
        const secondToLastVertices = qAdjacent(path.edges[size - 2], AdjacencyType.VERTEX, EntityType.VERTEX);
        return qSubtraction(qUnion([startVertices, endVertices]), qUnion([secondVertices, secondToLastVertices]));
    }
}

/**
 * Unlike implemenation in `getPathEndVertices`, works for paths with edges that are not topologically connected.
 */
function getPathEndVerticesNotTopologicallyConnected(context is Context, path is Path) returns Query
{
    const size = path.edges->size();
    if (size == 0 || path.closed)
    {
        return qNothing();
    }
    const firstEdgeVertex = path.edges[0]->qEdgeVertex(!path.flipped[0]);
    const lastEdgeVertex = path.edges[size - 1]->qEdgeVertex(path.flipped[size - 1]);
    return qUnion([firstEdgeVertex, lastEdgeVertex]);
}

