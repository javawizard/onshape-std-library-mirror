FeatureScript 1777; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "1777.0");

import(path : "onshape/std/containers.fs", version : "1777.0");
import(path : "onshape/std/evaluate.fs", version : "1777.0");
import(path : "onshape/std/feature.fs", version : "1777.0");
import(path : "onshape/std/manipulator.fs", version : "1777.0");
import(path : "onshape/std/math.fs", version : "1777.0");
import(path : "onshape/std/topologyUtils.fs", version : "1777.0");
import(path : "onshape/std/valueBounds.fs", version : "1777.0");
import(path : "onshape/std/vector.fs", version : "1777.0");

/**
 * The type of fit spline.
 * @value VERTICES : Creates spline through selected vertices.
 * @value EDGES : Approximates a set of edges by a single spline.
 */
export enum FitSplineType
{
    annotation { "Name" : "Vertices" }
    VERTICES,
    annotation { "Name" : "Edges" }
    EDGES
}

/**
 * Feature performing either [opFitSpline] or [opSplineThroughEdges] depending on selection
 */
annotation { "Feature Type Name" : "3D fit spline",
        "Manipulator Change Function" : "fitSplineManipulatorChange",
        "Editing Logic Function" : "fitSplineEditLogic",
        "UIHint" : UIHint.NO_PREVIEW_PROVIDED }
export const fitSpline = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        // This hidden "placeholder" Query allows preselection of both vertices AND edges
        // It contents are directed to where necessary in the fitSplineEditLogic function
        annotation { "Name" : "Entities", "UIHint" : UIHint.ALWAYS_HIDDEN, "Filter" : EntityType.VERTEX || EntityType.EDGE }
        definition.initEntities is Query;

        annotation { "Name" : "Approximation type", "UIHint" : UIHint.HORIZONTAL_ENUM}
        definition.fitType is FitSplineType;

        if (definition.fitType != FitSplineType.EDGES)
        {
            annotation { "Name" : "Vertices", "Filter" : EntityType.VERTEX, "UIHint" : UIHint.ALLOW_QUERY_ORDER }
            definition.vertices is Query;

            annotation { "Name" : "Closed spline" }
            definition.closed is boolean;

            if (!definition.closed)
            {
                annotation { "Name" : "Start direction", "Filter" : EntityType.EDGE || QueryFilterCompound.ALLOWS_DIRECTION, "MaxNumberOfPicks" : 1 }
                definition.startDirection is Query;

                annotation { "Name" : "Start magnitude" }
                isReal(definition.startMagnitude, CLAMP_MAGNITUDE_REAL_BOUNDS);

                annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                definition.oppositeDirectionStart is boolean;

                annotation { "Name" : "Match curvature at start" }
                definition.matchStartCurvature is boolean;

                annotation { "Name" : "End direction", "Filter" : EntityType.EDGE || QueryFilterCompound.ALLOWS_DIRECTION, "MaxNumberOfPicks" : 1 }
                definition.endDirection is Query;

                annotation { "Name" : "End magnitude" }
                isReal(definition.endMagnitude, CLAMP_MAGNITUDE_REAL_BOUNDS);

                annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                definition.oppositeDirectionEnd is boolean;

                annotation { "Name" : "Match curvature at end" }
                definition.matchEndCurvature is boolean;

                annotation { "Name" : "Has start direction", "UIHint" : UIHint.ALWAYS_HIDDEN }
                definition.hasStartDirection is boolean;

                annotation { "Name" : "Has end direction", "UIHint" : UIHint.ALWAYS_HIDDEN }
                definition.hasEndDirection is boolean;
            }
        }
        else
        {
            annotation { "Name" : "Edges", "Filter" : EntityType.EDGE }
            definition.edges is Query;
        }
    }
    {
        if (!definition.closed)
        {
            verifyNoMesh(context, definition, "startDirection");
            verifyNoMesh(context, definition, "endDirection");
        }

        if (definition.fitType == FitSplineType.EDGES)
        {
            verifyNoSheetMetalFlatQuery(context, definition.edges, "edges", ErrorStringEnum.FLATTENED_SHEET_METAL_SKETCH_PROHIBTED);
        }
        else if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V858_SM_FLAT_BUG_FIXES) )
        {
            verifyNoSheetMetalFlatQuery(context, definition.vertices, "vertices", ErrorStringEnum.FLATTENED_SHEET_METAL_SKETCH_PROHIBTED);
            verifyNoSheetMetalFlatQuery(context, definition.startDirection, "startDirection", ErrorStringEnum.FLATTENED_SHEET_METAL_SKETCH_PROHIBTED);
            verifyNoSheetMetalFlatQuery(context, definition.endDirection, "endDirection", ErrorStringEnum.FLATTENED_SHEET_METAL_SKETCH_PROHIBTED);
        }
        // Part 1 of 2 calls for making the feature patternable via feature pattern.
        const qReferences = definition.fitType == FitSplineType.VERTICES ? definition.vertices : definition.edges;
        var remainingTransform = getRemainderPatternTransform(context, { "references" : qReferences });

        if (definition.fitType != FitSplineType.EDGES)
        {
            const fitSplineDefn = getFitSplineThroughPointsDefinition(context, id, definition);
            opFitSpline(context, id, fitSplineDefn);
        }
        else
        {
            // Don't evaluateQuery(context, definition.edges); doing so prevents sel-intersection edges warning.
            // Rely on opSpline to check for non-empty selection of edges
            opSplineThroughEdges(context, id, {"edges" : definition.edges});
        }

        // Part 2 of 2 calls for making the feature patternable via feature pattern.
        transformResultIfNecessary(context, id, remainingTransform);
    }, { closed : false, startMagnitude : 1, endMagnitude : 1, startDirection : qNothing(), endDirection : qNothing(),
        matchStartCurvature : false, matchEndCurvature : false, oppositeDirectionStart : false, oppositeDirectionEnd : false,
        hasStartDirection : false, hasEndDirection : false, fitType : FitSplineType.VERTICES, initEntities : qNothing()});

function getFitSplineThroughPointsDefinition(context is Context, id is Id, definition is map) returns map
{
    var points = [];
    for (var vertex in evaluateQuery(context, definition.vertices))
    {
        points = append(points, evVertexPoint(context, { "vertex" : vertex }));
    }

    if (definition.closed)
    {
        if (size(points) < 3)
        {
            throw regenError(ErrorStringEnum.CLOSED_SPLINE_THREE_POINTS, ["vertices"]);
        }
        points = append(points, points[0]);
    }
    else if (size(points) < 2)
    {
        throw regenError(ErrorStringEnum.SPLINE_TWO_POINTS, ["vertices"]);
    }

    if (definition.oppositeDirectionStart)
    {
        definition.startMagnitude *= -1;
    }

    if (definition.oppositeDirectionEnd)
    {
        definition.endMagnitude *= -1;
    }

    const boundingBox = evBox3d(context, {
                "topology" : definition.vertices,
                "tight" : true
            });

    const totalSpan = box3dDiagonalLength(boundingBox);
    if (tolerantEquals(totalSpan, 0 * meter))
    {
        throw regenError(ErrorStringEnum.FIT_SPLINE_REPEATED_POINT, ['vertices']);
    }

    var startDerivative = undefined;
    var endDerivative = undefined;
    var start2ndDerivative = undefined;
    var end2ndDerivative = undefined;

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
            start2ndDerivative =  startCondition.second;
        }

        if (endCondition != undefined)
        {
            if (tolerantEquals(endCondition.magnitude, 0 * meter))
            {
                throw regenError(ErrorStringEnum.FIT_SPLINE_ZERO_END_MAGNITUDE, ['endMagnitude']);
            }
            endDerivative = endCondition.direction * endCondition.magnitude;
            end2ndDerivative = endCondition.second;
        }
    }
    var fitSplineDefn = { "points" : points,
                "startDerivative" : startDerivative,
                "endDerivative" : endDerivative };
    if (definition.matchStartCurvature)
    {
        fitSplineDefn = mergeMaps(fitSplineDefn, {"start2ndDerivative" : start2ndDerivative });
    }
    if (definition.matchEndCurvature)
    {
        fitSplineDefn = mergeMaps(fitSplineDefn, {"end2ndDerivative" : end2ndDerivative });
    }
    return fitSplineDefn;
}

// Returns direction and magnitude for end condition.
// Returns a direction and magnitude instead of just a vector to avoid some expensive operations downstream.
function getEndCondition(context is Context, definition is map, points is array, totalSpan, sqrtDistance, isStart is boolean)
{
    const directionProperty = isStart ? "startDirection" : "endDirection";
    const magnitudeProperty = isStart ? "startMagnitude" : "endMagnitude";
    const matchingCurvature = isStart ? definition.matchStartCurvature : definition.matchEndCurvature;

    // Index of either the first or the last point.
    const pointIndex = isStart ? 0 : size(points) - 1;

    var magnitude = definition[magnitudeProperty] * totalSpan;
    const interpolationPointDistance = norm(isStart ? points[0] - points[1] : points[pointIndex] - points[pointIndex - 1]);
    if (tolerantEquals(interpolationPointDistance, 0 * meter))
    {
        throw regenError(ErrorStringEnum.FIT_SPLINE_REPEATED_POINT, ["vertices"]);
    }

    const directionFaces = evaluateQuery(context, qEntityFilter(definition[directionProperty], EntityType.FACE));
    const directionEdges = evaluateQuery(context, qEntityFilter(definition[directionProperty], EntityType.EDGE));
    var directionConnectors = [];
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V740_PROPAGATE_PROPERTIES_IN_PATTERNS)) // prior to this version mate connectors don't filter
        directionConnectors = evaluateQuery(context, qBodyType(definition[directionProperty], BodyType.MATE_CONNECTOR));

    if (size(directionFaces) + size(directionEdges) + size(directionConnectors) > 1)
    {
        throw regenError(ErrorStringEnum.TANGENCY_ONE_EDGE, [directionProperty]);
    }

    if (size(directionFaces) == 1)
    {
        if (matchingCurvature)
        {
            throw regenError(ErrorStringEnum.FIT_SPLINE_CURVATURE_FACE, [directionProperty]);
        }
        const direction = extractDirection(context, directionFaces[0]);
        return { "direction" : direction, "magnitude" : magnitude };
    }
    else if (size(directionEdges) == 1)
    {
        // After V934, we will use fast parameterization ("arcLengthParameterization" : false), since this is what is returned from evDistance.
        const useArcLengthParam = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V934_FIT_SPLINE_PARAM);

        var param = 0.0;
        var tangentDirection = undefined;
        try silent
        {
            var result = evDistance(context, {
                    "side0" : directionEdges[0],
                    "side1" : points[pointIndex],
                    "arcLengthParameterization" : false
                });
            param = result.sides[0].parameter;
            tangentDirection = evEdgeTangentLine(context, {
                            "edge" : directionEdges[0],
                            "parameter" : param,
                            "arcLengthParameterization" : useArcLengthParam
                }).direction;

            //creates better looking curves given the centripetal parameterization (and creates same geometry for legacy features)
            magnitude *= (sqrtDistance / sqrt(interpolationPointDistance.value));
            if (!matchingCurvature)
            {
               return { "direction" : tangentDirection, "magnitude" : magnitude };
            }
        }
        catch
        {
            throw regenError(ErrorStringEnum.FIT_SPLINE_CANNOT_EVALUATE_END_CONDITION, [directionProperty]);
        }
        try silent
        {
            const edgeCurvatureData = evEdgeCurvature(context, {
                    "edge" : directionEdges[0],
                    "parameter" : param,
                    "arcLengthParameterization" : useArcLengthParam
                });

            //Using f'' = |f'|^2 * k * n
            var secondDerivative = magnitude * magnitude * edgeCurvatureData.curvature * curvatureFrameNormal(edgeCurvatureData);
            return { "direction" : tangentDirection, "magnitude" : magnitude, "second" : secondDerivative};
        }
        catch
        {
            throw regenError(ErrorStringEnum.FIT_SPLINE_CANNOT_EVALUATE_CURVATURE_END_CONDITION, [directionProperty]);
        }
    }
    else if (size(directionConnectors) == 1)
    {
        if (matchingCurvature)
        {
            throw regenError(ErrorStringEnum.FIT_SPLINE_CURVATURE_FACE, [directionProperty]);
        }
        const direction = extractDirection(context, directionConnectors[0]);
        return { "direction" : direction, "magnitude" : magnitude };
    }

    // we don't have direction input, cannot match curvature
    if (matchingCurvature)
    {
        throw regenError(ErrorStringEnum.FIT_SPLINE_NEED_DIRECTION_FOR_CURVATURE, [directionProperty]);
    }

    //if we do have selections but we could not use them
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V705_G2_CURVES) &&
       (isStart ? definition.hasStartDirection : definition.hasEndDirection))
    {
        throw regenError(ErrorStringEnum.FIT_SPLINE_CANNOT_EVALUATE_END_CONDITION, [directionProperty]);
    }

    return undefined;
}

/**
 * @internal
 * Handle preselection of either vertices or edges, choose fitType accordingly
 * Keep track of whether a selection was made for directions so that we can give correct error if it goes missing
 */
export function fitSplineEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
        isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (oldDefinition == {}) // check preselection
    {
        const qEdges = qEntityFilter(definition.initEntities, EntityType.EDGE);
        const qVertices = qEntityFilter(definition.initEntities, EntityType.VERTEX);
        if (!isQueryEmpty(context, qEdges))
        {
            definition.fitType = FitSplineType.EDGES;
            definition.edges = qEdges;
        }
        else if (!isQueryEmpty(context, qVertices))
        {
            definition.fitType = FitSplineType.VERTICES;
            definition.vertices = qEntityFilter(definition.initEntities, EntityType.VERTEX);
        }
        // else keep fitType of previous invocation
        definition.initEntities = qNothing();
        return definition;
    }

    if (definition.fitType == FitSplineType.EDGES)
    {
        return definition;
    }
    definition.hasStartDirection = false;
    definition.hasEndDirection = false;

    if (specifiedParameters.startDirection && !isQueryEmpty(context, definition.startDirection))
    {
        definition.hasStartDirection = true;
    }

    if (specifiedParameters.endDirection && !isQueryEmpty(context, definition.endDirection))
    {
        definition.hasEndDirection = true;
    }
    return definition;
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
        manipulators[START_MANIPULATOR] = linearManipulator({
                    "base" : points[0],
                    "direction" : startCondition.direction,
                    "offset" : manipulatorMagnitude,
                    "primaryParameterId" : "startMagnitude"
                });
    }

    if (endCondition != undefined)
    {
        const manipulatorMagnitude = definition.endMagnitude * totalSpan / MANIPULATOR_SCALE_FACTOR;
        manipulators[END_MANIPULATOR] = linearManipulator({
                    "base" : points[size(points) - 1],
                    "direction" : -endCondition.direction,
                    "offset" : manipulatorMagnitude,
                    "primaryParameterId" : "endMagnitude"
                });
    }

    addManipulators(context, id, manipulators);
}

/**
 * @internal
 * The manipulator change function used in the `fitSpline` feature.
 */
export function fitSplineManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    if (definition.fitType == FitSplineType.EDGES)
    {
        return definition;
    }
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

    const totalSpan = box3dDiagonalLength(boundingBox);
    const oppositeDirectionStart = definition.oppositeDirectionStart ? -1 : 1;
    const oppositeDirectionEnd = definition.oppositeDirectionEnd ? -1 : 1;

    if (startManipulator != undefined)
    {
        definition.startMagnitude = startManipulator.offset * MANIPULATOR_SCALE_FACTOR / totalSpan * oppositeDirectionStart;
    }

    if (endManipulator != undefined)
    {
        definition.endMagnitude = endManipulator.offset * MANIPULATOR_SCALE_FACTOR / totalSpan * oppositeDirectionEnd;
    }

    return definition;
}

