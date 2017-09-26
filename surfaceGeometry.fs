FeatureScript 686; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/**
 * This module contains methods for creating and working with primitive
 * surfaces: planes, cylinders, cones, spheres, and tori.
 */
import(path : "onshape/std/context.fs", version : "686.0");
import(path : "onshape/std/coordSystem.fs", version : "686.0");
import(path : "onshape/std/curveGeometry.fs", version : "686.0");
import(path : "onshape/std/mathUtils.fs", version : "686.0");
import(path : "onshape/std/string.fs", version : "686.0");
import(path : "onshape/std/units.fs", version : "686.0");
export import(path : "onshape/std/surfacetype.gen.fs", version : "686.0");

//===================================== Plane ======================================

/**
 * The default XY plane, whose normal points along the Z axis.
 */
export const XY_PLANE = plane(vector(0, 0, 0) * meter, vector(0, 0, 1));

/**
 * A `Plane` is a data type representing an origin, a normal vector, and an X direction,
 * perpendicular to the normal direction.
 * @type {{
 *      @field origin {Vector} : A 3D point, in world space.
 *      @field normal {Vector} : A 3D unit vector in world space.
 *      @field x {Vector} : A 3D unit vector in world space. Must be perpendicular to `normal`.
 * }}
 */
export type Plane typecheck canBePlane;

/** Typecheck for [Plane] */
export predicate canBePlane(value)
{
    value is map;
    is3dLengthVector(value.origin);
    is3dDirection(value.x);
    is3dDirection(value.normal);
    abs(dot(value.x, value.normal)) < TOLERANCE.zeroAngle;
}

/**
 * Create a [Plane] on the XY plane of a specified coordinate system.
 */
export function plane(cSys is CoordSystem) returns Plane
{
    return plane(cSys.origin, cSys.zAxis, cSys.xAxis);
}

/**
 * Create a [Plane] which fully specifies its orientation.
 *
 * @param origin : A 3D point in world space.
 * @param normal : A 3D vector in world space. Need not be normalized.
 * @param x      : A 3D vector in world space. Need not be normalized.
 */
export function plane(origin is Vector, normal is Vector, x is Vector) returns Plane
{
    return { "origin" : origin, "normal" : normalize(normal), "x" : normalize(x) } as Plane;
}

/**
 * Create a [Plane] from a point and a normal.
 *
 * The x-axis of this [Plane]'s coordinate system will be an arbitrary vector
 * perpindicular to the `normal`.
 *
 * @param origin : A 3D point in world space.
 * @param normal : A 3D vector in world space. Need not be normalized.
 */
export function plane(origin is Vector, normal is Vector) returns Plane
{
    return plane(origin, normal, perpendicularVector(normal));
}

/**
 * @internal
 * Create a [Plane] from the result of a builtin call.
 */
export function planeFromBuiltin(definition is map) returns Plane
{
    return plane((definition.origin as Vector) * meter, definition.normal as Vector, definition.x as Vector);
}

/**
 * Returns the plane that would represent the coordinate system of a face coplanar with the input plane.
 * Used in plane transformation for computing sketch patterns.
 */
export function alignCanonically(context is Context, plane is Plane) returns Plane
{
    return planeFromBuiltin(@alignCanonically(context, {"plane" : plane}));
}

/**
 * Returns the y-axis of the specified plane as a 3D Vector in world space.
 */
export function yAxis(plane is Plane) returns Vector
{
    return cross(plane.normal, plane.x);
}

/**
 * Check that two [Plane]s are the same up to tolerance, including the origin and local coordinate system.
 */
export predicate tolerantEquals(plane1 is Plane, plane2 is Plane)
{
    tolerantEquals(plane1.origin, plane2.origin);
    tolerantEquals(plane1.normal, plane2.normal);
    tolerantEquals(plane1.x, plane2.x);
}

/**
 * Create a coordinate system whose XY-plane is a specified plane, with its origin at the
 * plane's origin.
 */
export function planeToCSys(plane is Plane) returns CoordSystem
{
    return coordSystem(plane.origin, plane.x, plane.normal);
}

/**
 * Create a coordinate system whose XY-plane is a specified plane, with its origin at the
 * plane's origin.
 *
 * Alias for `planeToCSys`.
 */
export function coordSystem(plane is Plane) returns CoordSystem
{
    return planeToCSys(plane);
}

export function toString(value is Plane) returns string
{
    return "normal" ~ toString(value.normal) ~ " " ~ "origin" ~ toString(value.origin) ~ " " ~ "x" ~ toString(value.x);
}

/**
 * Projects a 3D `point` onto a [Plane], returning a 3D point on the plane.
 */
export function project(plane is Plane, point is Vector) returns Vector
precondition
{
    is3dLengthVector(point);
}
{
    return point + plane.normal * dot(plane.normal, plane.origin - point);
}

/**
 * Projects a [Line] onto a [Plane], returning a [Line] whose origin is
 * on the [Plane] and whose direction is a normalized [Vector] on the [Plane]
 *
 * Throws an error if the [Line] is in the same direction as the normal of the [Plane]
 */
export function project(plane is Plane, line is Line) returns Line
precondition
{
    !parallelVectors(plane.normal, line.direction);
}
{
    var origin = project(plane, line.origin);
    var direction = normalize(line.direction - project(plane.normal, line.direction));

    return { "origin" : origin, "direction" : direction } as Line;
}

export operator*(transform is Transform, planeRhs is Plane) returns Plane
{
    return plane(transform * planeRhs.origin,
                 inverse(transpose(transform.linear)) * planeRhs.normal, //The normal is a co-vector
                 transform.linear * planeRhs.x);
}

/**
 * Transforms a 2D `point` in a [Plane]'s coordinates to its corresponding 3D
 * point in world coordinates.
 */
export function planeToWorld(plane is Plane, planePoint is Vector) returns Vector
precondition
{
    isLengthVector(planePoint);
    @size(planePoint) == 2;
}
{
    return plane.origin + plane.x * planePoint[0] + cross(plane.normal, plane.x) * planePoint[1];
}

/**
 * Returns a [Transform] which takes 3D points measured with respect to a [Plane]
 * (such that points which lie on the plane will have a z-coordinate of
 * approximately `0`) and transforms them into world coordinates.
 */
export function planeToWorld3D(plane is Plane) returns Transform
{
    return toWorld(coordSystem(plane.origin, plane.x, plane.normal));
}

/**
 * Transforms a 3D `point` in world coordinates into a 3D point measured in a
 * [Plane]'s coordinates. If the `point` lies on the [Plane], the result will
 * have a z-coordinate of approximately `0`.
 */
export function worldToPlane3D(plane is Plane, worldPoint is Vector) returns Vector
{
    return fromWorld(coordSystem(plane.origin, plane.x, plane.normal), worldPoint);
}

/**
* Transforms a 3D `worldPoint` in world coordinates into a 2D point measured in a
* [Plane]'s (x,y) coordinates.
*
* This is modified as of FeatureScript version 363.0. Older versions of FeatureScript
* use `worldToPlane` to return 3D vectors composed of the plane coordinate system baseis.
* This functionality is still available in the `worldToPlane` function above.
*/
export function worldToPlane(plane is Plane, worldPoint is Vector) returns Vector
{
    var planeY = cross(plane.normal, plane.x);
    var planeOriginToPoint = worldPoint - plane.origin;
    return vector(dot(plane.x, planeOriginToPoint), dot(planeY, planeOriginToPoint));
}

/**
 * Returns a [Transform] which takes 3D points measured in world coordinates
 * and transforms them into 3D points measured in plane coordinates (such that
 * points which lie on the plane will have a z-coordinate of approximately `0`).
 */
export function worldToPlane3D(plane is Plane) returns Transform
{
    return fromWorld(coordSystem(plane.origin, plane.x, plane.normal));
}

/**
 * Returns a [Transform] which maps the plane `from` to the plane `to`.
 */
export function transform(from is Plane, to is Plane) returns Transform
{
    return planeToWorld3D(to) * worldToPlane3D(from);
}

/**
 * Returns a [Transform] which takes points on one side of a plane and
 * transforms them to the other side. The resulting transform is non-rigid,
 * and using this transform in an [opTransform] or similar operations will
 * invert the transformed bodies.
 */
export function mirrorAcross(plane is Plane) returns Transform
{
    const normalMatrix = [plane.normal] as Matrix;
    const linear = identityMatrix(3) - 2 * transpose(normalMatrix) * normalMatrix;
    return transform(linear, plane.origin - linear * plane.origin);
}

/**
 * Returns a [Line] at the intersection between the two [Plane]s. If the planes
 * are parallel or coincident, returns `undefined`.
 */
export function intersection(plane1 is Plane, plane2 is Plane) // Returns Line or undefined if there's no good intersection
{
    var direction = cross(plane1.normal, plane2.normal);
    if (norm(direction) < TOLERANCE.zeroAngle)
        return;
    direction = normalize(direction);
    const rhs = vector(dot(plane1.normal, plane1.origin), dot(plane2.normal, plane2.origin),
        dot(direction, 0.5 * (plane1.origin + plane2.origin)));
    const point = inverse([plane1.normal, plane2.normal, direction] as Matrix) * rhs;
    return line(point, direction);
}

/**
 * Represents an intersection between a line an a plane. Depending on the
 * line and plane, this intersection may be a point, a line, or nothing.
 *
 * @type {{
 *      @field dim {number} : Integer representing the dimension of the intersection.
 *              @eg `0` indicates that `intersection` is a 3D length `Vector`.
 *              @eg `1` indicates that `intersection` is a [Line].
 *              @eg `-1` indicates that the intersection does not exist (i.e.
 *                  the line and the plane are parallel).
 *      @field intersection : `undefined` or `Vector` or [Line] (depending on `dim`) that represents the intersection.
 * }}
 */
export type LinePlaneIntersection typecheck canBeLinePlaneIntersection;

/** Typecheck for [LinePlaneIntersection] */
export predicate canBeLinePlaneIntersection(value)
{
    value is map;
    (value.dim == 0 || value.dim == 1 || value.dim == -1);
    if (value.dim == 0)
        value.intersection is Vector;
    if (value.dim == 1)
        value.intersection is Line;
}

/**
 * Returns a `LinePlaneIntersection` representing the intersection between
 * `line` and [Plane].
 */
export function intersection(plane is Plane, line is Line) returns LinePlaneIntersection
{
    const dotPr = dot(plane.normal, line.direction);
    if (abs(dotPr) < TOLERANCE.zeroAngle)
    {
        if (tolerantEquals(line.origin, project(plane, line.origin)))
            return { 'dim' : 1, 'intersection' : line } as LinePlaneIntersection;
        else
            return { 'dim' : -1 } as LinePlaneIntersection; //line is parallel to plane
    }
    const t = dot(plane.origin - line.origin, plane.normal) / dotPr;
    return { 'dim' : 0, 'intersection' : line.origin + t * line.direction } as LinePlaneIntersection;
}

/**
 * Returns true if the two planes are coplanar.
 */
export function coplanarPlanes(plane1 is Plane, plane2 is Plane) returns boolean
{
    const point1 = project(plane1, vector(0, 0, 0) * meter);
    const point2 = project(plane2, vector(0, 0, 0) * meter);
    return tolerantEquals(point1, point2);
}

// ===================================== Cone ======================================

/**
 * Type representing a cone which extends infinitely down the positive z-axis of its
 * `coordSystem`.
 *
 * @type {{
 *      @field coordSystem {CoordSystem} : The position and orientation of the cone.
 *      @field halfAngle {ValueWithUnits} : The angle from z-axis of `coordSystem`
 *              to the surface of the cone.
 * }}
 */
export type Cone typecheck canBeCone;

/** Typecheck for [Cone] */
export predicate canBeCone(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    isAngle(value.halfAngle);
}

/**
 * Constructs a `Cone` from a coordinate system and a half angle.
 */
export function cone(cSys is CoordSystem, halfAngle is ValueWithUnits) returns Cone
{
    return { "coordSystem" : cSys, "halfAngle" : halfAngle } as Cone;
}

/**
 * @internal
 *
 * Create a `Cone` from the result of a builtin call.
 */
export function coneFromBuiltin(definition is map) returns Cone
{
    return cone(coordSystemFromBuiltin(definition.coordSystem), definition.halfAngle * radian);
}

/**
 * Check that two `Cone`s are the same up to tolerance, including the local coordinate system.
 */
export predicate tolerantEquals(cone1 is Cone, cone2 is Cone)
{
    tolerantEquals(cone1.coordSystem, cone2.coordSystem);
    tolerantEquals(cone1.halfAngle, cone2.halfAngle);
}

export function toString(value is Cone) returns string
{
    return "half angle" ~ toString(value.halfAngle) ~ "\n" ~ "basis" ~ toString(value.coordSystem);
}

// ===================================== Cylinder ======================================

/**
 * Type representing a Cylinder which extends infinitely along the z-axis of its
 * `coordSystem`.
 *
 * @type {{
 *      @field coordSystem {CoordSystem} : The position and orientation of the cylinder.
 *      @field radius {ValueWithUnits} : The cylinder's radius.
 * }}
 */
export type Cylinder typecheck canBeCylinder;

/** Typecheck for [Cylinder] */
export predicate canBeCylinder(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    isLength(value.radius);
}

/**
 * Constructs a cylinder from a coordinate system and a radius.
 */
export function cylinder(cSys is CoordSystem, radius is ValueWithUnits) returns Cylinder
{
    return { "coordSystem" : cSys, "radius" : radius } as Cylinder;
}

/**
 * @internal
 * Create a `Cylinder` from the result of a builtin call.
 */
export function cylinderFromBuiltin(definition is map) returns Cylinder
{
    return cylinder(coordSystemFromBuiltin(definition.coordSystem), definition.radius * meter);
}

/**
 * Check that two `Cylinder`s are the same up to tolerance, including the local coordinate system.
 */
export predicate tolerantEquals(cylinder1 is Cylinder, cylinder2 is Cylinder)
{
    tolerantEquals(cylinder1.coordSystem, cylinder2.coordSystem);
    tolerantEquals(cylinder1.radius, cylinder2.radius);
}

export function toString(value is Cylinder) returns string
{
    return "radius" ~ toString(value.radius) ~ "\n" ~ "basis" ~ toString(value.coordSystem);
}

// ===================================== Torus ======================================

/**
 * Type representing a torus, the shape of a circle revolved around a
 * coplanar axis.
 *
 * The torus represented is revolved about the z-axis of the `coordSystem`
 * and centered in its xy-plane.
 *
 * @type {{
 *      @field coordSystem {CoordSystem} : The position and orientation of the torus.
 *      @field radius {ValueWithUnits} : The major radius, i.e. the distance from the center of
 *              the torus to the center of the revolved circle.
 *      @field minorRadius {ValueWithUnits} : The minor radius, i.e. the radius of the revolved
 *              circle.
 * }}
 */
export type Torus typecheck canBeTorus;

/** Typecheck for [Torus] */
export predicate canBeTorus(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    isLength(value.radius);
    isLength(value.minorRadius);
}

/**
 * Constructs a torus from a coordinate system, the minor radius, and the major radius.
 */
export function torus(cSys is CoordSystem, minorRadius is ValueWithUnits, radius is ValueWithUnits) returns Torus
{
    return { "coordSystem" : cSys, "minorRadius" : minorRadius, "radius" : radius } as Torus;
}

/**
 * @internal
 * Create a `Torus` from the result of a builtin call.
 */
export function torusFromBuiltin(definition is map) returns Torus
{
    return torus(coordSystemFromBuiltin(definition.coordSystem), definition.minorRadius * meter, definition.radius * meter);
}

/**
 * Check that two tori are the same up to tolerance, including the local coordinate system.
 */
export predicate tolerantEquals(torus1 is Torus, torus2 is Torus)
{
    tolerantEquals(torus1.coordSystem, torus2.coordSystem);
    tolerantEquals(torus1.radius, torus2.radius);
    tolerantEquals(torus1.minorRadius, torus2.minorRadius);
}

export function toString(value is Torus) returns string
{
    return "radius" ~ toString(value.radius) ~ "\n" ~ "minor radius" ~ toString(value.minorRadius) ~ "\n" ~ "basis" ~ toString(value.coordSystem);
}

// ===================================== Sphere ======================================

/**
 * Type representing a sphere of a given radius centered around a 3D point.
 *
 * @type {{
 *      @field coordSystem {CoordSystem} : The position and orientation of the sphere.
 *      @field radius {ValueWithUnits} : The sphere's radius.
 * }}
 */
export type Sphere typecheck canBeSphere;

/** Typecheck for [Sphere] */
export predicate canBeSphere(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    isLength(value.radius);
}

export function sphere(cSys is CoordSystem, radius is ValueWithUnits) returns Sphere
{
    return { "coordSystem" : cSys, "radius" : radius } as Sphere;
}

/**
 * @internal
 * Create a `Sphere` from the result of a builtin call.
 */
export function sphereFromBuiltin(definition is map) returns Sphere
{
    return sphere(coordSystemFromBuiltin(definition.coordSystem), definition.radius * meter);
}

/**
 * Check that two `Sphere`s are the same up to tolerance, including the local coordinate system.
 */
export predicate tolerantEquals(sphere1 is Sphere, sphere2 is Sphere)
{
    tolerantEquals(sphere1.coordSystem, sphere2.coordSystem);
    tolerantEquals(sphere1.radius, sphere2.radius);
}

export function toString(value is Sphere) returns string
{
    return "radius" ~ toString(value.radius) ~ "\n" ~ "basis" ~ toString(value.coordSystem);
}

