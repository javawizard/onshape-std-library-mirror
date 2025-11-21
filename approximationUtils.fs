FeatureScript 2815; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/valueBounds.fs", version : "2815.0");
import(path : "onshape/std/feature.fs", version : "2815.0");
import(path : "onshape/std/path.fs", version : "2815.0");
import(path : "onshape/std/splineUtils.fs", version : "2815.0");
import(path : "onshape/std/containers.fs", version : "2815.0");
import(path : "onshape/std/curveGeometry.fs", version : "2815.0");
import(path : "onshape/std/math.fs", version : "2815.0");
import(path : "onshape/std/evaluate.fs", version : "2815.0");

/**
 * Number of sample taken on the curve to perform the approximation.
 */
export const APPROXIMATION_SAMPLES = 200;
/**
 * Maximum number of control points to approximate the curve with.
 */
export const MAX_CONTROL_POINTS = 100;
/**
 * Maximum degree of the curve.
 */
export const MAX_DEGREE = 15;

/**
 * A `LengthBoundSpec` for approximation tolerance.
 */
export const TOLERANCE_BOUND =
{
            (meter) : [1e-8, 1e-5, 1],
            (centimeter) : 1e-3,
            (millimeter) : 1e-2,
            (inch) : 2e-3,
            (foot) : 2e-4,
            (yard) : 1e-5
        } as LengthBoundSpec;

/**
 * An `IntegerBoundSpec` for curve degree.
 */
export const DEGREE_BOUND =
{
            (unitless) : [2, 3, MAX_DEGREE]
        } as IntegerBoundSpec;

/**
 * A predicate to add curve approximation parameters to a feature.
 */
export predicate curveApproximationPredicate(definition is map)
{
    annotation { "Name" : "Approximate" }
    definition.approximate is boolean;
    annotation { "Group Name" : "Approximation parameters", "Driving Parameter" : "approximate", "Collapsed By Default" : false }
    {
        if (definition.approximate)
        {
            annotation { "Name" : "Target degree", "Column Name" : "Approximation target degree" }
            isInteger(definition.approximationDegree, DEGREE_BOUND);

            annotation { "Name" : "Maximum control points" }
            isInteger(definition.approximationMaxCPs, { (unitless) : [4, 15, MAX_CONTROL_POINTS] } as IntegerBoundSpec);

            annotation { "Name" : "Tolerance" }
            isLength(definition.approximationTolerance, TOLERANCE_BOUND);

            annotation { "Name" : "Keep start derivative" }
            definition.keepStartDerivative is boolean;

            annotation { "Name" : "Keep end derivative" }
            definition.keepEndDerivative is boolean;

            annotation { "Name" : "Show deviation" }
            definition.approximationShowDeviation is boolean;
            annotation { "Group Name" : "Show deviation", "Driving Parameter" : "approximationShowDeviation", "Collapsed By Default" : false }
            {
                if (definition.approximationShowDeviation)
                {
                    annotation { "Name" : "Maximum deviation", "UIHint" : UIHint.READ_ONLY }
                    isLength(definition.maxDeviation, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
                }
            }
        }
    }
}

/**
 * Checks approximation options to see if the approximation would succeed and highlights the issue if not.
 */
export function checkApproximationParameters(definition is map, path is Path)
{
    if (definition.approximationDegree < 2)
    {
        throw regenError(ErrorStringEnum.EDIT_CURVE_APPROXIMATION_DEGREE_TOO_SMALL, ["approximationDegree"]);
    }
    var extraCtrlPts = 0;
    if (definition.keepStartDerivative)
    {
        extraCtrlPts += 1;
    }
    if (definition.keepEndDerivative)
    {
        extraCtrlPts += 1;
    }
    if (path.closed && extraCtrlPts > 0)
    {
        throw regenError(ErrorStringEnum.EDIT_CURVE_CLOSED_APPROXIMATION_NO_DERIVATIVE);
    }
    // This check comes from the server-side approximation code.
    const minControlPoints = max(4, definition.approximationDegree + 1) + (path.closed ? definition.approximationDegree - 1 : 0) + extraCtrlPts;
    if (definition.approximationMaxCPs < minControlPoints)
    {
        throw regenError("Approximation needs at least " ~ minControlPoints ~ " control points for a " ~ (path.closed ? "closed " : "") ~ "curve of degree " ~ definition.approximationDegree ~ ".", ["approximationMaxCPs"]);
    }
}

/** @internal */
export function makeApproximationTarget(context is Context, path is Path, keepStartDerivative is boolean, keepEndDerivative is boolean) returns ApproximationTarget
{
    var points = [];

    var parameters = makeArray(APPROXIMATION_SAMPLES, 0);
    for (var i = 1; i < APPROXIMATION_SAMPLES; i += 1)
    {
        parameters[i] = i / (APPROXIMATION_SAMPLES - 1);
    }
    const tangentLines = evPathTangentLines(context, path, parameters).tangentLines;
    for (var i = 0; i < APPROXIMATION_SAMPLES; i += 1)
    {
        points = append(points, tangentLines[i].origin);
    }

    // Create the map for positions and optionally add derivatives
    var approximateMap = { 'positions' : points };
    if (keepStartDerivative)
    {
        approximateMap.startDerivative = tangentLines[0].direction;
    }
    if (keepEndDerivative)
    {
        approximateMap.endDerivative = tangentLines[APPROXIMATION_SAMPLES - 1].direction;
    }
    return approximationTarget(approximateMap);
}

function approximateOneCurve(context is Context, curve is Query, definition is map) returns BSplineCurve
{
    var path;
    try silent
    {
        path = constructPath(context, qOwnedByBody(curve, EntityType.EDGE), { "tolerance" : 1e-5 * meter }).path;
    }
    catch (error)
    {
        throw regenError(error, curve);
    }
    checkApproximationParameters(definition, path);
    const approximationTarget = makeApproximationTarget(context, path, definition.keepStartDerivative, definition.keepEndDerivative);
    return approximateSpline(context, {
                    "degree" : definition.approximationDegree,
                    "tolerance" : definition.approximationTolerance,
                    "isPeriodic" : path.closed,
                    "targets" : [approximationTarget],
                    "maxControlPoints" : definition.approximationMaxCPs
                })[0];
}

/**
 * Approximates the curves created by the feature id.
 * This meant to be used along with `curveApproximationPredicate` and it expects `definition` to have the parameters defined in the predicate.
 */
export function approximateResults(context is Context, id is Id, definition is map)
{
    const curves = evaluateQuery(context, qCreatedBy(id, EntityType.BODY)->qBodyType(BodyType.WIRE));
    const numCurves = size(curves);
    if (numCurves == 0)
    {
        return;
    }
    var maxOfMaxDeviations = 0;
    for (var i = 0; i < numCurves; i += 1)
    {
        const bspline = approximateOneCurve(context, curves[i], definition);

        const baseId = id + "approximate" + i;
        opCreateBSplineCurve(context, baseId + "bSplineCurve", {
                    "bSplineCurve" : bspline
                });
        if (definition.approximationShowDeviation)
        {
            const maxDeviationResult = evMaxPathDeviation(context, {
                        "side1" : qCreatedBy(baseId + "bSplineCurve", EntityType.EDGE),
                        "side2" : curves[i],
                        "showDeviation" : true });
            if (maxOfMaxDeviations < maxDeviationResult.deviation)
            {
                maxOfMaxDeviations = maxDeviationResult.deviation;
            }
        }
        opEditCurve(context, baseId + "editCurve", {
                    "wire" : curves[i],
                    "edge" : qCreatedBy(baseId + "bSplineCurve", EntityType.EDGE),
                    "showCurves" : true
                });
        opDeleteBodies(context, baseId + "deleteBSplineCurve", {
                    "entities" : qCreatedBy(baseId + "bSplineCurve", EntityType.BODY)
                });
    }

    setFeatureComputedParameter(context, id, { "name" : "maxDeviation", "value" : maxOfMaxDeviations });
}

