FeatureScript 2796; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/mathUtils.fs", version : "2796.0");
import(path : "onshape/std/units.fs", version : "2796.0");


/**
 * Position of the world origin, equivalent to `vector(0, 0, 0) * meter`
 */
export const WORLD_ORIGIN = vector(0, 0, 0) * meter;

/**
 * Direction parallel to the X axis, equivalent to `vector(1, 0, 0)`
 */
export const X_DIRECTION = vector(1, 0, 0);

/**
 * Direction parallel to the Y axis, equivalent to `vector(0, 1, 0)`
 */
export const Y_DIRECTION = vector(0, 1, 0);

/**
 * Direction parallel to the Z axis, equivalent to `vector(0, 0, 1)`
 */
export const Z_DIRECTION = vector(0, 0, 1);

/**
 * The world coordinate system, equivalent to `coordSystem(vector(0, 0, 0) * meter, vector(1, 0, 0), vector(0, 0, 1))`
 */
export const WORLD_COORD_SYSTEM = coordSystem(WORLD_ORIGIN, X_DIRECTION, Z_DIRECTION);

/**
 * A right-handed Cartesian coordinate system. Used for converting points and
 * geometry between different reference frames, or for creating planes and
 * mate connectors.
 *
 * The y-axis of a coordinate system is not stored, but it can be obtained by
 * calling the [yAxis](yAxis(CoordSystem)) function, which simply performs a cross product.
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
 * @ex `toWorld(cSys, vector(0, 0, 0))` equals `cSys.origin`
 *
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
 * Returns a [Transform] which will transform coordinates measured in `cSys` into
 * world coordinates.
 * @ex `toWorld(cSys) * vector(0, 0, 0)` equals `cSys.origin`
 *
 * When used in operations which place or move parts (like [opTransform], [opPattern],
 * or [addInstance]), this transform will (somewhat counterintuitively) move parts from
 * the world origin and orientation to the `cSys` origin and orientation.
 */
export function toWorld(cSys is CoordSystem) returns Transform
{
    const rotation = transpose(matrix([cSys.xAxis, cross(cSys.zAxis, cSys.xAxis), cSys.zAxis]));
    return transform(rotation, cSys.origin);
}

/**
 * Convert a specified point from world space into a specified coordinate system.
 * @ex `fromWorld(cSys, cSys.origin)` equals `vector(0, 0, 0)`
 *
 * @param worldPoint : A 3D vector, measured in world space.
 * @returns {Vector} : A 3D vector measured in `cSys`
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
 * Returns a [Transform] which will transform coordinates measured in world space into
 * `cSys` coordinates.
 * @ex `fromWorld(cSys) * cSys.origin` equals `vector(0, 0, 0)`
 *
 * When used in operations which place or move parts (like [opTransform], [opPattern],
 * or [addInstance]), this transform will (somewhat counterintuitively) move parts from
 * the `cSys` origin and orientation to the world origin and orientation.
 */
export function fromWorld(cSys is CoordSystem) returns Transform
{
    const rotation = matrix([cSys.xAxis, cross(cSys.zAxis, cSys.xAxis), cSys.zAxis]);
    return transform(rotation, rotation * -cSys.origin);
}

export operator*(transform is Transform, cSys is CoordSystem) returns CoordSystem
{
    return coordSystem(transform * cSys.origin,
                       transform.linear * (cSys.xAxis as Vector),
                       inverse(transpose(transform.linear)) * cSys.zAxis);
}

/**
 * Returns a [Transform] that represents 3 independent scalings along the X, Y, and Z axes
 * of a particular `cSys`, centered around `cSys.origin`.
 */
export function scaleNonuniformly(xScale is number, yScale is number, zScale is number, cSys is CoordSystem) returns Transform
{
    var scaling = diagonalMatrix([xScale, yScale, zScale]);
    return toWorld(cSys) * transform(scaling, vector(0, 0, 0) * meter) * fromWorld(cSys);
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
    return "origin " ~ toString(cSys.origin) ~ "\n" ~ "x-Axis " ~ toString(cSys.xAxis) ~ "\n" ~ "z-Axis " ~ toString(cSys.zAxis);
}

