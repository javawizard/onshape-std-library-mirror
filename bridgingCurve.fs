FeatureScript 1120; /* Automatically generated version */
import(path : "onshape/std/containers.fs", version : "1120.0");
import(path : "onshape/std/coordSystem.fs", version : "1120.0");
import(path : "onshape/std/curveGeometry.fs", version : "1120.0");
import(path : "onshape/std/evaluate.fs", version : "1120.0");
import(path : "onshape/std/feature.fs", version : "1120.0");
import(path : "onshape/std/manipulator.fs", version : "1120.0");
import(path : "onshape/std/math.fs", version : "1120.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1120.0");
import(path : "onshape/std/valueBounds.fs", version : "1120.0");
import(path : "onshape/std/vector.fs", version : "1120.0");

/**
 * Specifies how the bridging curve will match the vertex or edge at each side
 * @value POSITION : The bridging curve will end at the provided vertex. Direction of the curve is unspecified
 * @value TANGENCY : The bridging curve will end at the vertex and the curve will be tangent to the edge
 * @value CURVATURE : The bridging curve will end at the vertex and the curve will have same curvature as the edge at the vertex
 */
export enum BridgingCurveMatchType
{
    annotation { "Name" : "Match position" }
    POSITION,
    annotation { "Name" : "Match tangent" }
    TANGENCY,
    annotation { "Name" : "Match curvature" }
    CURVATURE
}

/**
 * A `RealBoundSpec` for bias of a tangency/tangency bridge, defaulting to `0.5`.
 */
export const BIAS_BOUNDS =
{
    (unitless) : [0.0001, 0.5, 0.9999]
} as RealBoundSpec;

const UI_SCALING = 0.5;
const ANGLE_RANGE = 30 * degree;
const MAGNITUDE_MANIPULATOR = "magnitudeManipulator";
const CENTRAL_MAGNITUDE_MANIPULATOR = "centralMagnitudeManipulator";
const BIAS_MANIPULATOR = "biasManipulator";
const DEFAULT_BIAS_MINIMUM = 0.25;
const DEFAULT_G1G1_SCALE = 2 / 3;

/**
 * Creates a curve between two points, optionally with matching of tangency or curvature to other curves at that point
 */
annotation { "Feature Type Name" : "Bridging curve",
        "Editing Logic Function" : "onFeatureChange",
        "Manipulator Change Function" : "onManipulatorChange",
        "UIHint" : "NO_PREVIEW_PROVIDED" }
export const bridgingCurve = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Preselection", "UIHint" : "ALWAYS_HIDDEN", "Filter" : EntityType.EDGE || EntityType.VERTEX }
        definition.preselectedEntities is Query;
        annotation { "Name" : "First side", "Filter" : EntityType.EDGE || EntityType.VERTEX, "MaxNumberOfPicks" : 2 }
        definition.side1 is Query;
        annotation { "Name" : "Match", "Default" : BridgingCurveMatchType.TANGENCY }
        definition.match1 is BridgingCurveMatchType;
        annotation { "Name" : "Second side", "Filter" : EntityType.EDGE || EntityType.VERTEX, "MaxNumberOfPicks" : 2 }
        definition.side2 is Query;
        annotation { "Name" : "Match", "Column Name" : "Second match", "Default" : BridgingCurveMatchType.TANGENCY }
        definition.match2 is BridgingCurveMatchType;
        if (definition.match1 != BridgingCurveMatchType.POSITION || definition.match2 != BridgingCurveMatchType.POSITION)
        {
            annotation { "Name" : "Magnitude" }
            isReal(definition.magnitude, POSITIVE_REAL_BOUNDS);
        }
        if (definition.match1 != BridgingCurveMatchType.POSITION && definition.match2 != BridgingCurveMatchType.POSITION)
        {
            annotation { "Name" : "Bias" }
            isReal(definition.bias, BIAS_BOUNDS);
        }
    }
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V858_SM_FLAT_BUG_FIXES))
        {
            verifyNoSheetMetalFlatQuery(context, definition.side1, "side1", ErrorStringEnum.FLATTENED_SHEET_METAL_SKETCH_PROHIBTED);
            verifyNoSheetMetalFlatQuery(context, definition.side2, "side2", ErrorStringEnum.FLATTENED_SHEET_METAL_SKETCH_PROHIBTED);
        }
        var remainingTransform = getRemainderPatternTransform(context,
            {
                "references" : qUnion([definition.side1, definition.side2])
            });

        const side1Data = getDataForSide(context, definition.side1, definition.match1, "side1", definition.side2);
        const side2Data = getDataForSide(context, definition.side2, definition.match2, "side2", definition.side1);

        if (definition.match1 == BridgingCurveMatchType.POSITION)
        {
            if (definition.match2 == BridgingCurveMatchType.POSITION)
            {
                createG0G0BridgingCurve(context, id, side1Data.point, side2Data.point);
            }
            else if (definition.match2 == BridgingCurveMatchType.TANGENCY)
            {
                createG0G1BridgingCurve(context, id, side1Data.point, side2Data.frame, definition.magnitude, false);
            }
            else if (definition.match2 == BridgingCurveMatchType.CURVATURE)
            {
                createG0G2BridgingCurve(context, id, side1Data.point, side2Data.frame, definition.magnitude, false);
            }
        }
        else if (definition.match1 == BridgingCurveMatchType.TANGENCY)
        {
            if (definition.match2 == BridgingCurveMatchType.POSITION)
            {
                createG0G1BridgingCurve(context, id, side2Data.point, side1Data.frame, definition.magnitude, true);
            }
            else if (definition.match2 == BridgingCurveMatchType.TANGENCY)
            {
                createG1G1BridgingCurve(context, id, side1Data.frame, side2Data.frame, definition.magnitude, definition.bias);
            }
            else if (definition.match2 == BridgingCurveMatchType.CURVATURE)
            {
                createG1G2BridgingCurve(context, id, side1Data.frame, side2Data.frame, definition.magnitude, definition.bias, false);
            }
        }
        else if (definition.match1 == BridgingCurveMatchType.CURVATURE)
        {
            if (definition.match2 == BridgingCurveMatchType.POSITION)
            {
                createG0G2BridgingCurve(context, id, side2Data.point, side1Data.frame, definition.magnitude, true);
            }
            else if (definition.match2 == BridgingCurveMatchType.TANGENCY)
            {
                createG1G2BridgingCurve(context, id, side2Data.frame, side1Data.frame, definition.magnitude, definition.bias, true);
            }
            else if (definition.match2 == BridgingCurveMatchType.CURVATURE)
            {
                createG2G2BridgingCurve(context, id, side1Data.frame, side2Data.frame, definition.magnitude, definition.bias);
            }
        }
        transformResultIfNecessary(context, id, remainingTransform);
    }, {
            'match1' : BridgingCurveMatchType.TANGENCY,
            'match2' : BridgingCurveMatchType.TANGENCY,
            'preselectedEntities' : qNothing()
        }
    );


/**
 * @internal
 * The feature change function used in the `bridgingCurve` feature.
 */
export function onFeatureChange(context is Context, id is Id, oldDefinition is map, definition is map) returns map
{
    if (oldDefinition != {})
    {
        // We are only going to modify the definition on pre-selection
        return definition;
    }


    // We have a few things we want to do here but we're not going to be too clever. We could try to work out whether a point
    // is isolated and the user wants a non-default POSITION bridging but it is more likely that they pre-selected points and
    // now need to select an edge to correctly specify tangency.
    // So we will limit the behavior to reshuffling of the selections.
    // If the user started out with odd selections then we will ignore the pre-selections.

    const vertices = evaluateQuery(context, qEntityFilter(definition.preselectedEntities, EntityType.VERTEX));
    const edges = evaluateQuery(context, qEntityFilter(definition.preselectedEntities, EntityType.EDGE));
    definition.preselectedEntities = qNothing();

    const vertexCount = size(vertices);
    const edgeCount = size(edges);

    if (vertexCount > 2 || edgeCount > 2 || vertexCount + edgeCount == 0)
    {
        return definition;
    }

    if (vertexCount == 0)
    {
        definition.side1 = edges[0];
        if (edgeCount > 1)
        {
            definition.side2 = edges[1];
        }
    }
    else if (edgeCount == 0)
    {
        definition.side1 = vertices[0];
        if (vertexCount > 1)
        {
            definition.side2 = vertices[1];
        }
    }
    else if (edgeCount == 1)
    {
        definition = matchVerticesToEdge(context, definition, edges[0], vertices);
    }
    else if (vertexCount == 1)
    {
        definition = matchEdgesToVertex(context, definition, vertices[0], edges);
    }
    else
    {
        definition = matchEdgeAndVertexPairs(context, definition, edges, vertices);
    }

    return definition;
}


function matchVerticesToEdge(context is Context, definition is map, edge is Query, vertices is array) returns map
{
    const endVertices = matchVerticesAtEndsOfEdge(context, edge, vertices);
    if (size(endVertices) == 0)
    {
        definition.side1 = edge;
        if (size(vertices) == 1)
        {
            // Didn't match a vertex so assume it is the other end of the bridge
            definition.side2 = vertices[0];
        }
    }
    else if (size(endVertices) == 1)
    {
        definition.side1 = qUnion([edge, endVertices[0]]);
        if (size(vertices) == 2)
        {
            // Matched one vertex but have two, so assume the other is at the other end
            if (vertices[0] == endVertices[0])
            {
                definition.side2 = vertices[1];
            }
            else
            {
                definition.side2 = vertices[0];
            }
        }
    }
    else
    {
        // One edge, with both its vertices selected? Maybe the user wants to bridge the two ends of the edge
        // So lets put the edge in both the queries
        definition.side1 = qUnion([edge, endVertices[0]]);
        definition.side2 = qUnion([edge, endVertices[1]]);
    }
    return definition;
}

function matchEdgesToVertex(context is Context, definition is map, vertex is Query, edges is array) returns map
{
    const matchedEdges = matchEdgesThatEndAtVertex(context, vertex, edges);
    if (size(matchedEdges) == 0)
    {
        definition.side1 = vertex;
        if (size(edges) == 1)
        {
            // Assume the edge is at the other side
            definition.side2 = edges[0];
        }
    }
    else if (size(matchedEdges) == 1)
    {
        definition.side1 = qUnion([vertex, matchedEdges[0]]);
        if (size(edges) == 2)
        {
            // Matched one vertex but have two, so assume the other is at the other end
            if (edges[0] == matchedEdges[0])
            {
                definition.side2 = edges[1];
            }
            else
            {
                definition.side2 = edges[0];
            }
        }
    }
    else
    {
        // Two matched edges? No idea what the user intends. Just throw one in the first box and one in the second
        definition.side1 = qUnion([vertex, matchedEdges[0]]);
        definition.side2 = matchedEdges[1];
    }
    return definition;
}

function matchEdgeAndVertexPairs(context is Context, definition is map, edges is array, vertices is array) returns map
{
    if (size(edges) != 2 || size(vertices) != 2)
    {
        return definition;
    }
    const matched1 = matchEdgesThatEndAtVertex(context, vertices[0], edges);
    const matched2 = matchEdgesThatEndAtVertex(context, vertices[1], edges);
    if (size(matched1) == 1 && size(matched2) == 1 && matched1[0] != matched2[0])
    {
        definition.side1 = qUnion([vertices[0], matched1[0]]);
        definition.side2 = qUnion([vertices[1], matched2[0]]);
    }
    return definition;
}

function matchVerticesAtEndsOfEdge(context is Context, edge is Query, vertices is array) returns array
{
    const ends = evEdgeTangentLines(context, {
                "edge" : edge,
                "parameters" : [0, 1]
            });
    if (size(ends) != 2)
    {
        return [];
    }

    var matches = [];
    for (var vertex in vertices)
    {
        const point = evVertexPoint(context, {
                    "vertex" : vertex
                });
        if (tolerantEquals(point, ends[0].origin) || tolerantEquals(point, ends[1].origin))
        {
            matches = append(matches, vertex);
        }
    }
    return matches;
}

function matchEdgesThatEndAtVertex(context is Context, vertex is Query, edges is array) returns array
{
    const point = evVertexPoint(context, {
                "vertex" : vertex
            });

    var matches = [];
    for (var edge in edges)
    {
        const ends = evEdgeTangentLines(context, {
                    "edge" : edge,
                    "parameters" : [0, 1]
                });
        if (size(ends) == 2)
        {
            if (tolerantEquals(point, ends[0].origin) || tolerantEquals(point, ends[1].origin))
            {
                matches = append(matches, edge);
            }
        }
    }
    return matches;
}

/**
 * @internal
 * The manipulator change function used in the `bridgingCurve` feature.
 */
export function onManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    const side1Data = getDataForSide(context, definition.side1, definition.match1, "side1", definition.side2);
    const side2Data = getDataForSide(context, definition.side2, definition.match2, "side2", definition.side1);

    if (newManipulators[MAGNITUDE_MANIPULATOR] is map)
    {
        const manipulator = newManipulators[MAGNITUDE_MANIPULATOR];
        var defaultSpeed;
        if (definition.match1 != BridgingCurveMatchType.POSITION)
        {
            defaultSpeed = determineDefaultG0G1Speed(side2Data.point, side1Data.point, curvatureFrameTangent(side1Data.frame));
        }
        else if (definition.match2 != BridgingCurveMatchType.POSITION)
        {
            defaultSpeed = determineDefaultG0G1Speed(side1Data.point, side2Data.point, curvatureFrameTangent(side2Data.frame));
        }
        definition.magnitude = manipulator.offset / (defaultSpeed * UI_SCALING);
    }
    if (newManipulators[CENTRAL_MAGNITUDE_MANIPULATOR] is map)
    {
        const manipulator = newManipulators[CENTRAL_MAGNITUDE_MANIPULATOR];
        const speeds = determineDefaultG1G1Speed(side1Data.frame, side2Data.frame);
        var scaling = UI_SCALING * (speeds[0] + speeds[1]) * 0.5;
        definition.magnitude = manipulator.offset / scaling;
    }
    if (newManipulators[BIAS_MANIPULATOR] is map)
    {
        const manipulator = newManipulators[BIAS_MANIPULATOR];
        definition.bias = (manipulator.angle / ANGLE_RANGE) + 0.5;
        definition.bias = max(0.01, min(0.99, definition.bias));
    }
    return definition;
}

function inferVertex(context is Context, edge is Query, otherSide is Query) returns Query
{
    var otherVertex = qEntityFilter(otherSide, EntityType.VERTEX);
    var otherEdge = qEntityFilter(otherSide, EntityType.EDGE);
    var inferred = qNothing();
    if (size(evaluateQuery(context, otherVertex)) == 1)
    {
        const edgeVertices = qAdjacent(edge, AdjacencyType.VERTEX, EntityType.VERTEX);
        inferred = qClosestTo(edgeVertices, evVertexPoint(context, {
                        "vertex" : otherVertex
                    }));
    }
    else if (size(evaluateQuery(context, otherEdge)) == 1)
    {
        // In this case we want to get the vertex closest to one of the vertices of the other edge
        const edgeVertices = qAdjacent(edge, AdjacencyType.VERTEX, EntityType.VERTEX);
        const otherEdgeVertices = qAdjacent(otherEdge, AdjacencyType.VERTEX, EntityType.VERTEX);
        var bestDistance = -2 * meter;
        for (var vertex in evaluateQuery(context, otherEdgeVertices))
        {
            const vertexPoint = evVertexPoint(context, { "vertex" : vertex });
            const found = qClosestTo(edgeVertices, vertexPoint);
            if (size(evaluateQuery(context, found)) == 1)
            {
                const distance = norm(evVertexPoint(context, { "vertex" : found }) - vertexPoint);
                if (bestDistance < -1 * meter || distance < bestDistance)
                {
                    inferred = found;
                    bestDistance = distance;
                }
            }
        }
    }
    return inferred;
}

/**
 * This will find the frame that exactly matches the point passed in. It's not useful in general cases and is deprecated.
 * It will reverse the direction of the start frame to point out of the edge
 */
function findMatchingEndFrame(point is Vector, startFrame is EdgeCurvatureResult, endFrame is EdgeCurvatureResult)
{
    if (tolerantEquals(startFrame.frame.origin, point))
    {
        // This is the frame at the start of the edge and we want a frame that points out of the edge
        // so we invert the zAxis (which is the tangent, see curvatureFrameTangent)
        var frame = startFrame;
        frame.frame.zAxis *= -1;
        return frame;
    }
    else if (tolerantEquals(endFrame.frame.origin, point))
    {
        return endFrame;
    }
    else
    {
        return undefined;
    }
}

/**
 * This will find the frame that is closest to the point passed in..
 * It will reverse the direction of the start frame to point out of the edge
 */
function findClosestEndFrame(point is Vector, startFrame is EdgeCurvatureResult, endFrame is EdgeCurvatureResult) returns EdgeCurvatureResult
{
    const startDistance = squaredNorm(startFrame.frame.origin - point);
    const endDistance = squaredNorm(endFrame.frame.origin - point);
    if (startDistance < endDistance)
    {
        var frame = startFrame;
        frame.frame.zAxis *= -1;
        return frame;
    }
    else
    {
        return endFrame;
    }
}

function getDataForSide(context is Context, side is Query, match is BridgingCurveMatchType, sideName is string, otherSide is Query) returns map
{
    var points = qEntityFilter(side, EntityType.VERTEX);
    var edges = qEntityFilter(side, EntityType.EDGE);
    var edgeCount = size(evaluateQuery(context, edges));

    if (size(evaluateQuery(context, points)) == 0 && edgeCount == 1)
    {
        // The user hasn't selected a vertex but if they selected an edge we may be able to work out what they want from the other side selections
        points = inferVertex(context, edges, otherSide);
    }
    if (size(evaluateQuery(context, points)) != 1)
    {
        throw regenError(ErrorStringEnum.BRIDGING_CURVE_VERTEX_BOTH_SIDES, [sideName]);
    }
    const point = evVertexPoint(context, { "vertex" : points });

    if (edgeCount != 1 && match != BridgingCurveMatchType.POSITION)
    {
        // Try to get the edge from the vertex
        edges = qAdjacent(points, AdjacencyType.VERTEX, EntityType.EDGE);
        edgeCount = size(evaluateQuery(context, edges));
        if (edgeCount != 1)
        {
            throw regenError(ErrorStringEnum.BRIDGING_CURVE_ONE_EDGE_EACH_SIDE, [sideName]);
        }
    }

    var frame;
    if (match != BridgingCurveMatchType.POSITION)
    {
        // This code deliberately only considers the ends of the edge but we could just as easily match to an
        // edge that passes through the specified point but doesn't end there.
        const frames = evEdgeCurvatures(context, {
                    "edge" : edges,
                    "parameters" : [0, 1]
                });
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V606_TOLERANT_BRIDGING_CURVE))
        {
            frame = findClosestEndFrame(point, frames[0], frames[1]);
        }
        else
        {
            frame = findMatchingEndFrame(point, frames[0], frames[1]);
        }
        if (frame == undefined)
        {
            throw regenError(ErrorStringEnum.BRIDGING_CURVE_VERTEX_AT_END_OF_EDGE, [sideName]);
        }
    }

    return
    {
            "point" : point,
            "frame" : frame
        };
}

function createG0G0BridgingCurve(context is Context, id is Id, point1 is Vector, point2 is Vector)
{
    const bCurve = {
                'degree' : 1,
                'dimension' : 3,
                'isPeriodic' : false,
                'isRational' : false,
                'knots' : [0, 0, 1, 1],
                'controlPoints' : [point1, point2]
            } as BSplineCurve;
    opCreateBSplineCurve(context, id, {
                "bSplineCurve" : bCurve
            });
}

function determineDefaultG0G1Speed(point1 is Vector, point2 is Vector, direction is Vector) returns ValueWithUnits
{
    // One argument I could make is that we want the distance between the middle control point and the other two to be equal
    // This would yield a symmetric curve. However, we may want to cap that based on distance between the two given points.
    // We'll start out with something simple.

    // 1. If the points are the same then there is nothing we can do
    if (tolerantEquals(point1, point2))
    {
        throw regenError(ErrorStringEnum.REGEN_ERROR);
    }

    // 2. First look at the intersection of the tangent vector with the bisecting plane of the points.
    //    If we can find one then the curve will be symmetric which may be nice

    // At the same time we don't want discontinuous behavior as point1 falls behind point2.
    // So cap the maximum distance to be the distance between the source points
    const maximumDistance = norm(point2 - point1);

    const line = line(point2, direction);
    const plane = plane((point1 + point2) * 0.5, normalize(point2 - point1));
    const intersection = intersection(plane, line);
    var distance = maximumDistance;
    if (intersection.dim == 0)
    {
        const candidate = intersection.intersection;
        const calculatedDistance = dot(candidate - point2, direction);
        if (calculatedDistance > 0 && calculatedDistance < maximumDistance)
        {
            distance = calculatedDistance;
        }
    }
    return distance;
}

function createG0G1BridgingCurve(context is Context, id is Id,
    point is Vector, curvatureFrame is EdgeCurvatureResult, magnitude is number, flipDirection is boolean)
{

    const defaultSpeed = determineDefaultG0G1Speed(point, curvatureFrame.frame.origin, curvatureFrameTangent(curvatureFrame));
     var magnitudeManipulator is Manipulator = linearManipulator(
        curvatureFrame.frame.origin,
        curvatureFrameTangent(curvatureFrame),
        magnitude * UI_SCALING * defaultSpeed
    );
    magnitudeManipulator.minValue = TOLERANCE.zeroLength;
    addManipulators(context, id, {
                (MAGNITUDE_MANIPULATOR) : magnitudeManipulator
            });

    // OK. The tangent vector of the edge curvature result goes away from the edge
    // Very simple to begin with
    const speed = magnitude * defaultSpeed;
    var middlePoint = curvatureFrame.frame.origin + (curvatureFrameTangent(curvatureFrame) * speed);

    var controlPoints = [
        point,
        middlePoint,
        curvatureFrame.frame.origin
    ];

    return createBezierCurve(context, id, flipDirection ? reverse(controlPoints) : controlPoints);
}

function determineDefaultG1G1Speed(frame1 is EdgeCurvatureResult, frame2 is EdgeCurvatureResult) returns array
{
    const line1 is Line = line(frame1.frame.origin, curvatureFrameTangent(frame1));
    const line2 is Line = line(frame2.frame.origin, curvatureFrameTangent(frame2));

    // Given the two points and tangent vectors we want to first of all calculate the default bias
    // which we will do by getting the distance of the point from the other line
    const bias1 = norm(project(line2, line1.origin) - line1.origin);
    const bias2 = norm(project(line1, line2.origin) - line2.origin);

    const jointBias = bias1 + bias2;
    var bias = 0.5;
    if (jointBias > TOLERANCE.zeroLength * meter)
    {
        bias = clamp(bias2 / jointBias, DEFAULT_BIAS_MINIMUM, 1 - DEFAULT_BIAS_MINIMUM);
    }
    // With the bias in hand we can now factor it in to the initial speeds
    const baseDistance = norm(line1.origin - line2.origin);
    return [DEFAULT_G1G1_SCALE * (1 - bias) * baseDistance, DEFAULT_G1G1_SCALE * bias * baseDistance];
}

function createG1G1Manipulators(context is Context, id is Id,
    line1 is Line, line2 is Line,
    speed1 is ValueWithUnits, speed2 is ValueWithUnits,
    magnitude is number, bias is number)
{
    var averageTangent = (line1.direction + line2.direction) * 0.5;
    if (tolerantEquals(averageTangent, vector(0, 0, 0)))
    {
        averageTangent = line1.direction;
    }
    var normal = cross(averageTangent, line2.origin - line1.origin);
    if (tolerantEquals(normal, vector(0, 0, 0) * meter))
    {
        return;
    }
    const direction = normalize(cross(line2.origin - line1.origin, normal));
    const base = (line1.origin + line2.origin) * 0.5;
    const offset = magnitude * UI_SCALING * (speed1 + speed2) * 0.5;
    var magnitudeManipulator is Manipulator = linearManipulator(
        base,
        direction,
        offset
    );
    magnitudeManipulator.minValue = TOLERANCE.zeroLength;

    const halfWidth = norm(line1.origin - line2.origin) * 0.5;
    const radius = halfWidth / sin(ANGLE_RANGE * 0.5);
    var biasManipulator is Manipulator = angularManipulator({
            "axisOrigin" : base + ((offset - radius) * direction),
            "axisDirection" : -normalize(normal), // negated so that the bias arrow matches how the shape changes
            "rotationOrigin" : base + (offset * direction),
            "angle" : (bias - 0.5) * ANGLE_RANGE,
            "minValue" : -ANGLE_RANGE * 0.49,
            "maxValue" : ANGLE_RANGE * 0.49,
            "style" : ManipulatorStyleEnum.SIMPLE
        });

    addManipulators(context, id, {
                (CENTRAL_MAGNITUDE_MANIPULATOR) : magnitudeManipulator,
                (BIAS_MANIPULATOR) : biasManipulator
            });
}

function createG1G1BridgingCurve(context is Context, id is Id,
    curvatureFrame1 is EdgeCurvatureResult, curvatureFrame2 is EdgeCurvatureResult, magnitude is number, bias is number)
{
    // OK. The tangent vector of the edge curvature result goes away from the edge
    // Very simple to begin with

    const speeds = determineDefaultG1G1Speed(curvatureFrame1, curvatureFrame2);

    const speed1 = speeds[0] * magnitude * 2 * (1 - bias);
    const speed2 = speeds[1] * magnitude * 2 * bias;

    const point1 = curvatureFrame1.frame.origin;
    const point2 = curvatureFrame2.frame.origin;
    const tangent1 = curvatureFrameTangent(curvatureFrame1);
    const tangent2 = curvatureFrameTangent(curvatureFrame2);

    createG1G1Manipulators(context, id, line(point1, tangent1), line(point2, tangent2),
            speeds[0], speeds[1],
            magnitude, bias);

    var inner1 = point1 + (speed1 * tangent1);
    var inner2 = point2 + (speed2 * tangent2);

    const controlPoints = [
            curvatureFrame1.frame.origin,
            inner1,
            inner2,
            curvatureFrame2.frame.origin
        ];

    return createBezierCurve(context, id, controlPoints);
}

function createG0G2BridgingCurve(context is Context, id is Id,
    point is Vector, curvatureFrame is EdgeCurvatureResult, magnitude is number, flipDirection is boolean)
{
    const defaultSpeed = determineDefaultG0G1Speed(point, curvatureFrame.frame.origin, curvatureFrameTangent(curvatureFrame));
    const speed = magnitude * defaultSpeed;

    var P2 = curvatureFrame.frame.origin + (curvatureFrameTangent(curvatureFrame) * speed);
    var P3 = curvatureFrame.frame.origin;
    var degree = 3;
    // using k = ((degree - 1) / degree) * h / |t| ^ 2, where k is curvature, t is tangent vector, h is the minimum distance
    // from the next control point to the extension of tangent direction
    var distance = (degree / (degree - 1)) * curvatureFrame.curvature * speed * speed;
    var P1 = P2 + distance * curvatureFrameNormal(curvatureFrame);

    var controlPoints = [
        point,
        P1,
        P2,
        P3
    ];

    var magnitudeManipulator is Manipulator = linearManipulator(
        curvatureFrame.frame.origin,
        curvatureFrameTangent(curvatureFrame),
        magnitude * UI_SCALING * defaultSpeed
    );
    magnitudeManipulator.minValue = TOLERANCE.zeroLength;
    addManipulators(context, id, {
                (MAGNITUDE_MANIPULATOR) : magnitudeManipulator
            });

    return createBezierCurve(context, id, flipDirection ? reverse(controlPoints) : controlPoints);
}

function createG1G2BridgingCurve(context is Context, id is Id,
    curvatureFrame1 is EdgeCurvatureResult, curvatureFrame2 is EdgeCurvatureResult, magnitude is number, bias is number, flipDirection is boolean)
{
    const speeds = determineDefaultG1G1Speed(curvatureFrame1, curvatureFrame2);

    const speed1 = speeds[0] * magnitude * 2 * (1 - bias);
    const speed2 = speeds[1] * magnitude * 2 * bias;

    const point1 = curvatureFrame1.frame.origin;
    const point2 = curvatureFrame2.frame.origin;
    const tangent1 = curvatureFrameTangent(curvatureFrame1);
    const tangent2 = curvatureFrameTangent(curvatureFrame2);

    createG1G1Manipulators(context, id, line(point1, tangent1), line(point2, tangent2),
            speeds[0], speeds[1],
            magnitude, bias);

    // using k = ((degree - 1) / degree) * h / |t| ^ 2, where k is curvature, t is tangent vector, h is the minimum distance
    // from the next control point to the extension of tangent direction
    var P1 = point1 + (speed1 * tangent1);
    var P3 = point2 + (speed2 * tangent2);
    var degree = 4;
    var distance = (degree / (degree - 1)) * curvatureFrame2.curvature * speed2 * speed2;
    var P2 = P3 + distance * curvatureFrameNormal(curvatureFrame2);

    var controlPoints = [
            curvatureFrame1.frame.origin,
            P1,
            P2,
            P3,
            curvatureFrame2.frame.origin
    ];
    return createBezierCurve(context, id, flipDirection ? reverse(controlPoints) : controlPoints);
}

function createG2G2BridgingCurve(context is Context, id is Id,
    curvatureFrame1 is EdgeCurvatureResult, curvatureFrame2 is EdgeCurvatureResult, magnitude is number, bias is number)
{
    const speeds = determineDefaultG1G1Speed(curvatureFrame1, curvatureFrame2);

    const speed1 = speeds[0] * magnitude * 2 * (1 - bias);
    const speed2 = speeds[1] * magnitude * 2 * bias;

    const point1 = curvatureFrame1.frame.origin;
    const point2 = curvatureFrame2.frame.origin;
    const tangent1 = curvatureFrameTangent(curvatureFrame1);
    const tangent2 = curvatureFrameTangent(curvatureFrame2);

    createG1G1Manipulators(context, id, line(point1, tangent1), line(point2, tangent2),
            speeds[0], speeds[1],
            magnitude, bias);

    // using k = ((degree - 1) / degree) * h / |t| ^ 2, where k is curvature, t is tangent vector, h is the minimum distance
    // from the next control point to the extension of tangent direction
    var degree = 5;
    var P1 = point1 + (speed1 * tangent1);
    var P4 = point2 + (speed2 * tangent2);
    var distance1 = (degree / (degree - 1)) * curvatureFrame1.curvature * speed1 * speed1;
    var P2 = P1 + distance1 * curvatureFrameNormal(curvatureFrame1);
    var distance2 = (degree / (degree - 1)) * curvatureFrame2.curvature * speed2 * speed2;
    var P3 = P4 + distance2 * curvatureFrameNormal(curvatureFrame2);

    var controlPoints = [
            curvatureFrame1.frame.origin,
            P1,
            P2,
            P3,
            P4,
            curvatureFrame2.frame.origin
    ];

    return createBezierCurve(context, id, controlPoints);
}

function createBezierCurve(context is Context, id is Id, controlPoints is array)
{
    const degree = size(controlPoints) - 1;
    var knots = makeArray(2 * size(controlPoints));
    for (var i = 0; i < degree + 1; i += 1)
    {
        knots[i] = 0;
        knots[i + degree + 1] = 1;
    }

    const bCurve = {
                'degree' : degree,
                'dimension' : 3,
                'isPeriodic' : false,
                'isRational' : false,
                'knots' : knots,
                'controlPoints' : controlPoints
            } as BSplineCurve;

    opCreateBSplineCurve(context, id, {
                "bSplineCurve" : bCurve
            });
}

