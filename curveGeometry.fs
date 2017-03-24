FeatureScript 543; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/curvetype.gen.fs", version : "543.0");

// Imports used internally
import(path : "onshape/std/coordSystem.fs", version : "543.0");
import(path : "onshape/std/mathUtils.fs", version : "543.0");
import(path : "onshape/std/units.fs", version : "543.0");

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

