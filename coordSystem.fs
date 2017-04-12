FeatureScript 559; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/mathUtils.fs", version : "559.0");
import(path : "onshape/std/units.fs", version : "559.0");

/**
 * A right-handed Cartesian coordinate system. Used for converting points and
 * geometry between different reference frames, or for creating planes and
 * mate connectors.
 *
 * The y-axis of a coordinate system is not stored, but it can be obtained by
 * calling `yAxis`, which simply performs a cross product.
 *
 * @seealso [toWorld(CoordSystem)]
 * @seealso [fromWorld(CoordSystem)]
 * @seealso [coordSystem(Plane)]
 * @seealso [plane(CoordSystem)]
 * @seealso [opMateConnector]
 *
 * @type {{
 *      @field origin {Vector}: A 3D point, in world space, representing the origin of the coordinate system.
 *      @field xAxis {Vector}: A 3D unit vector, in world space, representing the x-axis of the coordinate system.
 *      @field zAxis {Vector}: A 3D unit vector, in world space, representing the z-axis of the coordinate system.
 *          Must be perpendicular to the `xAxis`.
 * }}
 */
export type CoordSystem typecheck canBeCoordSystem;

/** @internal */
export predicate canBeCoordSystem(value)
{
    value is map;
    is3dLengthVector(value.origin);
    is3dDirection(value.xAxis);
    is3dDirection(value.zAxis);
    abs(dot(value.xAxis, value.zAxis)) < TOLERANCE.zeroAngle;
}

/**
 * Creates a Cartesian coordinate system.
 *
 * @seealso [CoordSystem]
 *
 * @param origin : A 3D point in world space.
 * @param xAxis : A 3D vector in world space. Need not be normalized.
 * @param zAxis : A 3D vector in world space. Need not be normalized but must be orthogonal to xAxis.
 */
export function coordSystem(origin is Vector, xAxis is Vector, zAxis is Vector) returns CoordSystem
precondition perpendicularVectors(xAxis, zAxis);
{
    return { "origin" : origin, "xAxis" : normalize(xAxis), "zAxis" : normalize(zAxis) } as CoordSystem;
}

/**
 * @internal
 *
 * Create a CoordSystem from the result of a builtin call.
 */
export function coordSystemFromBuiltin(cSys is map) returns CoordSystem
{
    return coordSystem((cSys.origin as Vector) * meter, cSys.xAxis as Vector, cSys.zAxis as Vector);
}

/**
 * Check that two [CoordSystem]s are the same up to tolerance.
 */
export predicate tolerantEquals(cSys1 is CoordSystem, cSys2 is CoordSystem)
{
    tolerantEquals(cSys1.origin, cSys2.origin);
    tolerantEquals(cSys1.xAxis, cSys2.xAxis);
    tolerantEquals(cSys1.zAxis, cSys2.zAxis);
}

/**
 * Convert a specified point from a specified coordinate system into world space.
 * @param cSys
 * @param pointInCSys : A 3D vector, measured with respect to the `cSys` provided.
 * @returns {Vector} : A 3D vector in world space.
 */
export function toWorld(cSys is CoordSystem, pointInCSys is Vector) returns Vector
precondition
{
    is3dLengthVector(pointInCSys);
}
{
    const transform = toWorld(cSys);
    return transform * pointInCSys;
}

/**
 * Returns a [Transform] which will move geometry from a specified coordinate system into world
 * space.
 *
 * @seealso [opTransform]
 */
export function toWorld(cSys is CoordSystem) returns Transform
{
    const rotation = transpose([cSys.xAxis, cross(cSys.zAxis, cSys.xAxis), cSys.zAxis] as Matrix);
    return transform(rotation, cSys.origin);
}

/**
 * Convert a specified point from world space into a specified coordinate system.
 * @param cSys
 * @param worldPoint : A 3D vector, measured in world space.
 */
export function fromWorld(cSys is CoordSystem, worldPoint is Vector) returns Vector
precondition
{
    is3dLengthVector(worldPoint);
}
{
    worldPoint -= cSys.origin;
    return vector(dot(worldPoint, cSys.xAxis), dot(worldPoint, cross(cSys.zAxis, cSys.xAxis)), dot(worldPoint, cSys.zAxis));
}

/**
 * Returns a [Transform] which will move geometry from world space into a
 * specified coordinate system.
 *
 * @seealso [opTransform]
 */
export function fromWorld(cSys is CoordSystem) returns Transform
{
    const rotation = [cSys.xAxis, cross(cSys.zAxis, cSys.xAxis), cSys.zAxis] as Matrix;
    return transform(rotation, rotation * -cSys.origin);
}

export operator*(transform is Transform, cSys is CoordSystem) returns CoordSystem
{
    return coordSystem(transform * cSys.origin,
                       transform.linear * (cSys.xAxis as Vector),
                       inverse(transpose(transform.linear)) * cSys.zAxis);
}

/**
 * Returns the y-axis of a coordinate system
 * @returns {Vector} : A 3D vector in world space.
 */
export function yAxis(cSys is CoordSystem) returns Vector
{
    return cross(cSys.zAxis, cSys.xAxis);
}

/**
 * Returns a representation of the coordinate system as a string.
 */
export function toString(cSys is CoordSystem) returns string
{
    return "origin" ~ toString(cSys.origin) ~ "\n" ~ "x-Axis" ~ toString(cSys.xAxis) ~ "\n" ~ "z-Axis" ~ toString(cSys.zAxis);
}

