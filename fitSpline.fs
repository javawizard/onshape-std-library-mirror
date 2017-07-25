FeatureScript 638; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "638.0");

import(path : "onshape/std/containers.fs", version : "638.0");
import(path : "onshape/std/evaluate.fs", version : "638.0");
import(path : "onshape/std/feature.fs", version : "638.0");
import(path : "onshape/std/manipulator.fs", version : "638.0");
import(path : "onshape/std/math.fs", version : "638.0");
import(path : "onshape/std/valueBounds.fs", version : "638.0");
import(path : "onshape/std/vector.fs", version : "638.0");

/**
 * Feature performing an [opFitSpline]
 */
annotation { "Feature Type Name" : "Fit spline",
        "Manipulator Change Function" : "fitSplineManipulatorChange",
        "UIHint" : "NO_PREVIEW_PROVIDED" }
export const fitSpline = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Vertices", "Filter" : EntityType.VERTEX }
        definition.vertices is Query;

        annotation { "Name" : "Closed spline" }
        definition.closed is boolean;

        if (!definition.closed)
        {
            annotation { "Name" : "Start direction", "Filter" : EntityType.EDGE, "MaxNumberOfPicks" : 1 }
            definition.startDirection is Query;

            annotation { "Name" : "Start magnitude" }
            isReal(definition.startMagnitude, CLAMP_MAGNITUDE_REAL_BOUNDS);

            annotation { "Name" : "End direction", "Filter" : EntityType.EDGE, "MaxNumberOfPicks" : 1 }
            definition.endDirection is Query;

            annotation { "Name" : "End magnitude" }
            isReal(definition.endMagnitude, CLAMP_MAGNITUDE_REAL_BOUNDS);
        }

    }
    {
        // Part 1 of 2 calls for making the feature patternable via feature pattern.
        var remainingTransform = getRemainderPatternTransform(context, { "references" : definition.vertices });

        var points = [];
        for (var vertex in evaluateQuery(context, definition.vertices))
        {
            points = append(points, evVertexPoint(context, { "vertex" : vertex }));
        }

        if (definition.closed)
        {
            if (size(points) < 3)
                throw regenError(ErrorStringEnum.CLOSED_SPLINE_THREE_POINTS, ["vertices"]);
            points = append(points, points[0]);
        }
        else if (size(points) < 2)
        {
            throw regenError(ErrorStringEnum.SPLINE_TWO_POINTS, ["vertices"]);
        }

        const boundingBox = evBox3d(context, {
                    "topology" : definition.vertices,
                    "tight" : true
                });

        const totalSpan = norm(boundingBox.maxCorner - boundingBox.minCorner);
        if (tolerantEquals(totalSpan, 0 * meter))
        {
            throw regenError(ErrorStringEnum.FIT_SPLINE_REPEATED_POINT, ['vertices']);
        }

        var startDerivative = undefined;
        var endDerivative = undefined;
        if (!definition.closed)
        {
            // The sum of the square roots of distances between interpolation points.
            // Important since the server uses centripetal knots, and some knowledge of the parametrization is necessary
            // to get the end conditions correct.
            const sqrtDistance = getSumSqrtDistances(points);
            const startCondition = getEndCondition(context, definition, points, totalSpan, sqrtDistance, true);
            const endCondition = getEndCondition(context, definition, points, totalSpan, sqrtDistance, false);

            addFitSplineManipulators(context, id, definition, startCondition, endCondition, points, totalSpan);

            if (startCondition != undefined)
            {
                if (tolerantEquals(startCondition.magnitude, 0 * meter))
                {
                    throw regenError(ErrorStringEnum.FIT_SPLINE_ZERO_START_MAGNITUDE, ['startMagnitude']);
                }
                startDerivative = startCondition.magnitude * startCondition.direction;
            }

            if (endCondition != undefined)
            {
                if (tolerantEquals(endCondition.magnitude, 0 * meter))
                {
                    throw regenError(ErrorStringEnum.FIT_SPLINE_ZERO_END_MAGNITUDE, ['endMagnitude']);
                }
                endDerivative = endCondition.direction * endCondition.magnitude;
            }
        }

        opFitSpline(context, id, { "points" : points,
                    "startDerivative" : startDerivative,
                    "endDerivative" : endDerivative });

        // Part 2 of 2 calls for making the feature patternable via feature pattern.
        transformResultIfNecessary(context, id, remainingTransform);
    }, { closed : false, startMagnitude : 1, endMagnitude : 1, startDirection : qNothing(), endDirection : qNothing() });

// Returns direction and magnitude for end condition.
// Returns a direction and magnitude instead of just a vector to avoid some expensive operations downstream.
function getEndCondition(context is Context, definition is map, points is array, totalSpan, sqrtDistance, isStart is boolean)
{
    const directionProperty = isStart ? "startDirection" : "endDirection";
    const magnitudeProperty = isStart ? "startMagnitude" : "endMagnitude";
    // Index of either the first or the last point.
    const pointIndex = isStart ? 0 : size(points) - 1;

    const interpolationPointDistance = norm(isStart ? points[0] - points[1] : points[pointIndex] - points[pointIndex - 1]);
    if (tolerantEquals(interpolationPointDistance, 0 * meter))
    {
        throw regenError(ErrorStringEnum.FIT_SPLINE_REPEATED_POINT, ["vertices"]);
    }

    const startEdges = evaluateQuery(context, definition[directionProperty]);
    if (size(startEdges) > 1)
    {
        throw regenError(ErrorStringEnum.TANGENCY_ONE_EDGE, [directionProperty]);
    }
    else if (size(startEdges) == 1)
    {
        try silent
        {
            var result = evDistance(context, {
                    "side0" : startEdges[0],
                    "side1" : points[pointIndex]
                });

            const direction = evEdgeTangentLine(context, {
                            "edge" : startEdges[0],
                            "parameter" : result.sides[0].parameter
                        }).direction;

            // The ratio between the manipulator magnitude and the actual magnitude is the difference in parameters to the next point.
            // This keeps the influence of the manipulator constant as the number of interpolation points and their spacing changes.
            // The other reason for this choice is to make the manipulators equivalent to the handles for sketch splines.
            const magnitude = definition[magnitudeProperty] * totalSpan * sqrtDistance / sqrt(interpolationPointDistance.value);
            return { "direction" : direction, "magnitude" : magnitude };
        }
        catch
        {
            throw regenError(ErrorStringEnum.FIT_SPLINE_CANNOT_EVALUATE_END_CONDITION, [directionProperty]);
        }
    }

    return undefined;
}

function getSumSqrtDistances(points is array) returns number
{
    var sqrtLength = 0;
    for (var i = 1; i < size(points); i += 1)
    {
        const first = points[i];
        const second = points[i - 1];
        // Calculating the distance without units gives a noticeable time saving especially for manipulator drag.
        sqrtLength += sqrt(norm(vector(first[0].value - second[0].value, first[1].value - second[1].value, first[2].value - second[2].value)));
    }

    return sqrtLength;
}

// Manipulator functions
const START_MANIPULATOR = "startManipulator";
const END_MANIPULATOR = "endManipulator";

// Picking the same factor as used in the sketch.
const MANIPULATOR_SCALE_FACTOR = 3;

function addFitSplineManipulators(context is Context, id is Id, definition, startCondition, endCondition, points, totalSpan)
{
    var manipulators = {};

    if (startCondition != undefined)
    {
        const manipulatorMagnitude = definition.startMagnitude * totalSpan / MANIPULATOR_SCALE_FACTOR;
        manipulators[START_MANIPULATOR] = linearManipulator(points[0], startCondition.direction, manipulatorMagnitude);
    }

    if (endCondition != undefined)
    {
        const manipulatorMagnitude = definition.endMagnitude * totalSpan / MANIPULATOR_SCALE_FACTOR;
        manipulators[END_MANIPULATOR] = linearManipulator(points[size(points) - 1], -endCondition.direction, manipulatorMagnitude);
    }

    addManipulators(context, id, manipulators);
}

/**
 * @internal
 * The manipulator change function used in the `fitSpline` feature.
 */
export function fitSplineManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    const startManipulator = newManipulators[START_MANIPULATOR];
    const endManipulator = newManipulators[END_MANIPULATOR];

    var points = [];
    for (var vertex in evaluateQuery(context, definition.vertices))
    {
        points = append(points, evVertexPoint(context, { "vertex" : vertex }));
    }
    const boundingBox = evBox3d(context, {
                "topology" : definition.vertices,
                "tight" : true
            });

    const totalSpan = norm(boundingBox.maxCorner - boundingBox.minCorner);
    const sqrtDistance = getSumSqrtDistances(points);
    const startCondition = getEndCondition(context, definition, points, totalSpan, sqrtDistance, true);
    const endCondition = getEndCondition(context, definition, points, totalSpan, sqrtDistance, false);

    if (startManipulator != undefined)
    {
        definition.startMagnitude = startManipulator.offset * MANIPULATOR_SCALE_FACTOR / totalSpan;
    }

    if (endManipulator != undefined)
    {
        definition.endMagnitude = endManipulator.offset * MANIPULATOR_SCALE_FACTOR / totalSpan;
    }

    return definition;
}

