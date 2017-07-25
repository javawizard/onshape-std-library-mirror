FeatureScript 638; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/curvetype.gen.fs", version : "638.0");

// Imports used internally
import(path : "onshape/std/coordSystem.fs", version : "638.0");
import(path : "onshape/std/mathUtils.fs", version : "638.0");
import(path : "onshape/std/units.fs", version : "638.0");

// ===================================== Line ======================================

/**
 * Represents a parameterized line in 3D space.
 * @type {{
 *      @field origin {Vector} : A point on the line, as a 3D Vector with length units.
 *      @field direction {Vector} : A unitless normalized 3D Vector.
 * }}
 */
export type Line typecheck canBeLine;

/** Typecheck for [Line] */
export predicate canBeLine(value)
{
    value is map;
    is3dLengthVector(value.origin);
    is3dDirection(value.direction);
}

/**
 * Creates a line from a point and a direction.
 * @param direction : The direction gets normalized by this function.
 */
export function line(origin is Vector, direction is Vector) returns Line
{
    return { "origin" : origin, "direction" : normalize(direction) } as Line;
}

/**
 * @internal
 *
 * Create a [Line] from the result of a builtin call.
 */
export function lineFromBuiltin(definition is map) returns Line
{
    return line((definition.origin as Vector) * meter, definition.direction as Vector);
}

/**
 * Check that two [Line]s are the same up to tolerance, including the origin.
 */
export predicate tolerantEquals(line1 is Line, line2 is Line)
{
    tolerantEquals(line1.origin, line2.origin);
    tolerantEquals(line1.direction, line2.direction);
}

export operator*(transform is Transform, lineRhs is Line) returns Line
{
    return line(transform * lineRhs.origin, transform.linear * lineRhs.direction);
}

/**
 * Returns the transformation that transforms the line `from` to the line `to` (including the origin) using the minimum
 * rotation angle.
 */
export function transform(from is Line, to is Line) returns Transform
{
    const rotation = rotationMatrix3d(from.direction, to.direction);
    return transform(rotation, to.origin - rotation * from.origin);
}

/**
 * Returns the projection of the point onto the line. See also other overloads of `project`.
 */
export function project(line is Line, point is Vector) returns Vector
precondition
{
    is3dLengthVector(point);
}
{
    return line.origin + line.direction * dot(line.direction, point - line.origin);
}

/**
 * Returns a [Transform] that represents the rotation around the given line by the given angle. The rotation is
 * counterclockwise looking against the line direction.
 */
export function rotationAround(line is Line, angle is ValueWithUnits) returns Transform
precondition
{
    isAngle(angle);
}
{
    const rotation = rotationMatrix3d(line.direction, angle);
    return transform(rotation, line.origin - rotation * line.origin);
}

export function toString(value is Line) returns string
{
    return "direction" ~ toString(value.direction) ~ "\n" ~ "origin" ~ toString(value.origin);
}


// ===================================== Circle ======================================

/**
 * Represents a circle in 3D space.
 * @type {{
 *      @field coordSystem {CoordSystem} : The circle lies in the xy plane of this coordinate
 *              system and the origin of its parameterization is the x axis.
 *      @field radius {ValueWithUnits} : The radius of the circle.
 * }}
 */
export type Circle typecheck canBeCircle;

/** Typecheck for [Circle] */
export predicate canBeCircle(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    isLength(value.radius);
}

/**
 * Returns a new `Circle` in the given coordinate system `cSys`.
 */
export function circle(cSys is CoordSystem, radius is ValueWithUnits) returns Circle
{
    return { "coordSystem" : cSys, "radius" : radius } as Circle;
}

/**
 * Returns a new `Circle` with the given parameters. `xDirection` and `normal` must be perpendicular.
 */
export function circle(center is Vector, xDirection is Vector, normal is Vector, radius is ValueWithUnits) returns Circle
{
    return circle(coordSystem(center, xDirection, normal), radius);
}

/**
 * @internal
 *
 * Create a `Circle` from the result of a builtin call.
 */
export function circleFromBuiltin(definition is map) returns Circle
{
    return circle(coordSystemFromBuiltin(definition.coordSystem), definition.radius * meter);
}

/**
 * Check that two `Circle`s are the same up to tolerance, including the coordinate system.
 */
export predicate tolerantEquals(circle1 is Circle, circle2 is Circle)
{
    tolerantEquals(circle1.coordSystem, circle2.coordSystem);
    tolerantEquals(circle1.radius, circle2.radius);
}

export function toString(value is Circle) returns string
{
    return "radius" ~ toString(value.radius) ~ "\n" ~ "center" ~ toString(value.coordSystem.origin);
}


// ===================================== Ellipse ======================================

/**
 * Represents an ellipse in 3D space.
 * @type {{
 *      @field coordSystem {CoordSystem} : The ellipse lies in the xy plane of this coordinate
 *              system and the x axis corresponds to the major radius.
 *      @field majorRadius {ValueWithUnits} : The larger of the two radii.
 *      @field minorRadius {ValueWithUnits} : The smaller of the two radii.
 * }}
 */
export type Ellipse typecheck canBeEllipse;

/** Typecheck for [Ellipse] */
export predicate canBeEllipse(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    isLength(value.majorRadius);
    isLength(value.minorRadius);
    value.majorRadius >= value.minorRadius;
}

/**
 * Returns a new `Ellipse` with the given parameters.
 */
export function ellipse(cSys is CoordSystem, majorRadius is ValueWithUnits, minorRadius is ValueWithUnits) returns Ellipse
{
    return { "coordSystem" : cSys, "majorRadius" : majorRadius, "minorRadius" : minorRadius } as Ellipse;
}

/**
 * Returns a new `Ellipse` with the given parameters. `xDirection` and `normal` must be perpendicular.
 */
export function ellipse(center is Vector, xDirection is Vector, normal is Vector, majorRadius is ValueWithUnits, minorRadius is ValueWithUnits) returns Ellipse
{
    return ellipse(coordSystem(center, xDirection, normal), majorRadius, minorRadius);
}

/**
 * @internal
 *
 * Create an `Ellipse` from the result of a builtin call.
 */
export function ellipseFromBuiltin(definition is map) returns Ellipse
{
    return ellipse(coordSystemFromBuiltin(definition.coordSystem), definition.majorRadius * meter, definition.minorRadius * meter);
}

/**
 * Check that two `Ellipse`s are the same up to tolerance, including the coordinate system.
 */
export predicate tolerantEquals(ellipse1 is Ellipse, ellipse2 is Ellipse)
{
    tolerantEquals(ellipse1.coordSystem, ellipse2.coordSystem);
    tolerantEquals(ellipse1.majorRadius, ellipse2.majorRadius);
    tolerantEquals(ellipse1.minorRadius, ellipse2.minorRadius);
}

export function toString(value is Ellipse) returns string
{
    return "majorRadius" ~ toString(value.majorRadius) ~ "\n" ~ "minorRadius" ~ toString(value.minorRadius) ~ "\n" ~ "center" ~ toString(value.coordSystem.origin);
}

/**
 * The definition of a spline in 3D space.
 * @type {{
 *      @field degree {number} : The degree of the spline
 *      @field dimension {number} : The dimension of the spline. Must be 2 or 3.
 *      @field isRational {boolean} : Is the spline rational?
 *      @field isPeriodic {boolean} : Is the spline periodic?
 *      @field controlPoints {array} : An array of control points of the required dimension.
 *              Size should be at least degree + 1
 *      @field weights {array} : An array of unitless values of same size as the control points.
 *              Required for a rational spline
 *           @optional
 *      @field knots {array} : An array of non-decreasing knots of size equal to 1 + degree + size(controlPoints)
 * }}
 */
export type BSplineCurve typecheck canBeBSplineCurve;

/** Typecheck for [BSplineCurve] */
export predicate canBeBSplineCurve(value)
{
    value is map;
    isPositiveInteger(value.degree);
    isPositiveInteger(value.dimension);
    value.isRational is boolean;

    value.dimension > 1 && value.dimension < 4;

    value.isPeriodic is boolean;

    value.controlPoints is array;
    @size(value.controlPoints) > value.degree;
    for (var controlPoint in value.controlPoints)
    {
        isLengthVector(controlPoint);
        @size(controlPoint) == value.dimension;
    }

    !value.isRational || (value.weights is array && @size(value.weights) == @size(value.controlPoints));
    if (value.isRational)
    {
        for (var weight in value.weights)
        {
            weight is number;
            weight >= 0;
        }
    }

    value.knots is array;
    @size(value.knots) == value.degree + @size(value.controlPoints) + 1;
    for (var i = 0; i < @size(value.knots); i += 1)
    {
        value.knots[i] is number;
        i == 0 || value.knots[i] >= value.knots[i - 1];
    }
}

/**
 * Returns a new polynomial `BSplineCurve`.
 *      @param degree {number} : The degree of the spline
 *      @param isPeriodic {boolean} : Is the spline periodic?
 *      @param controlPoints {array} : An array of control points of the required dimension.
 *              Size should be at least degree + 1
 *      @param knots {array} : An array of non-decreasing knots of size equal to 1 + degree + size(controlPoints)
 */
export function bSplineCurve(degree is number, isPeriodic is boolean, controlPoints is array, knots is array) returns BSplineCurve
{
    var dimension = 3;
    if (@size(controlPoints) > 0 && controlPoints[0] is Vector)
    {
        dimension = @size(controlPoints[0]);
    }
    return {
        'degree' : degree,
        'dimension' : dimension,
        'isRational' : false,
        'isPeriodic' : isPeriodic,
        'controlPoints' : controlPoints,
        'knots' : knots
    } as BSplineCurve;
}

/**
 * Returns a new rational `BSplineCurve`.
 *      @param degree {number} : The degree of the spline
 *      @param isPeriodic {boolean} : Is the spline periodic?
 *      @param controlPoints {array} : An array of control points of the required dimension.
 *              Size should be at least degree + 1
 *      @param weights {array} : An array of unitless values of same size as the control points.
 *      @param knots {array} : An array of non-decreasing knots of size equal to 1 + degree + size(controlPoints)
 */
export function bSplineCurve(degree is number, isPeriodic is boolean, controlPoints is array, weights is array, knots is array) returns BSplineCurve
{
    var dimension = 3;
    if (@size(controlPoints) > 0 && controlPoints[0] is Vector)
    {
        dimension = @size(controlPoints[0]);
    }
    return {
        'degree' : degree,
        'dimension' : dimension,
        'isRational' : true,
        'isPeriodic' : isPeriodic,
        'controlPoints' : controlPoints,
        'weights' : weights,
        'knots' : knots
    } as BSplineCurve;
}

