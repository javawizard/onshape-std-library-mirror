FeatureScript 2022; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/containers.fs", version : "2022.0");
import(path : "onshape/std/context.fs", version : "2022.0");
import(path : "onshape/std/curveGeometry.fs", version : "2022.0");
import(path : "onshape/std/mathUtils.fs", version : "2022.0");
import(path : "onshape/std/units.fs", version : "2022.0");


/**
 * A set of points and optionally derivatives to approximate by a spline.
 * @seeAlso [approximateSpline(Context,map)]
 * @type {{
 *      @field positions {array} : An ordered array of points for the spline to pass closely to.
 *      @field startDerivative {Vector} : @requiredIf { `start2ndDerivative` is provided } The desired start derivative of the spline.
 *      @field start2ndDerivative {Vector} : @optional The desired start second derivative of the spline.
 *      @field endDerivative {Vector} : @requiredIf { `end2ndDerivative` is provided } The desired end derivative of the spline.
 *      @field end2ndDerivative {Vector} : @optional The desired end second derivative of the spline.
 * }}
*/
export type ApproximationTarget typecheck canBeApproximationTarget;

/** Typecheck for ApproximationTarget */
export predicate canBeApproximationTarget(value)
{
    value is map;
    value.positions is array;
    size(value.positions) >= 2;
    for (var position in value.positions)
        isLengthVector(position);
    value.startDerivative == undefined || value.startDerivative is Vector;
    value.start2ndDerivative == undefined || value.start2ndDerivative is Vector;
    value.endDerivative == undefined || value.endDerivative is Vector;
    value.end2ndDerivative == undefined || value.end2ndDerivative is Vector;
}

/** Construct an [ApproximationTarget] */
export function approximationTarget(value is map) returns ApproximationTarget
{
    return value as ApproximationTarget;
}

/**
 * Compute a family of splines that approximates a family of [ApproximationTarget]s to within a given tolerance. The
 * resulting splines are consistently parameterized, so that, for example, lofting between them will match corresponding
 * target positions.
 * Note: If `parameters` are not specified, the magnitude of start and end derivatives in targets is ignored as well as
 * the second component parallel with the first derivative.
 * @param definition {{
 *      @field degree {number} : The desired degree of the curve.  The output may have a different degree, if for example there aren't enough points.
 *              @autocomplete `3`
 *      @field tolerance {ValueWithUnits} : How far the output is allowed to deviate from the input.  Must be at least 1e-8 meters.
 *              @autocomplete `1e-5`
 *      @field isPeriodic {boolean} : Whether the output spline is periodic.
 *              @autocomplete `false`
 *      @field targets {array} : An array of [ApproximationTarget]s.  All targets must have the same number of positions and specify corresponding
 *                               derivative information.
 *              @autocomplete `[approximationTarget({ 'positions' : points })]`
 *      @field parameters {array} : @optional An array of numbers representing the parameters corresponding to the target points. Must be strictly increasing.
 *                                  If specified, the output spline at those parameters will match the target points.
 *                                  If specified, derivatives in approximation targets will not be rescaled.
 *      @field maxControlPoints {number} : @optional The maximum number of control points that will be returned by this function's output.
 *                                         Tolerance will not be satisfied if this limit is reached. Default is 10000.
 * }}
 * @returns {array} : An array of [BSplineCurve]s, one for each target.
 */
export function approximateSpline(context is Context, definition is map) returns array
{
    return mapArray(@approximateSpline(context, definition), function(result) { return bSplineCurveFromBuiltin(result); });
}

/**
 * Evaluate a 3D spline at several parameters, possibly with derivatives.
 * @param definition {{
 *      @field spline {BSplineCurve} : The 3D spline to evaluate.
 *      @field parameters {array} : An array of numbers in the range of the spline's knot vector.
 *      @field nDerivatives {number} : @optional The number of derivatives to compute, in addition to the positions. Default is 0.
 * }}
 * @returns {array} : An array of arrays of points.  If `result` is returned, `result[i][j]` is the `i`th derivative at `j`th parameter.
 */
export function evaluateSpline(definition is map) returns array
{
    return mapArray(@evaluateSpline(definition), function(derivative) { return mapArray(derivative, function(parameter) { return parameter as Vector * meter; }); });
}

