FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the COPYING tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/coordSystem.fs", version : "");
import(path : "onshape/std/curveGeometry.fs", version : "");
import(path : "onshape/std/mathUtils.fs", version : "");
import(path : "onshape/std/string.fs", version : "");
import(path : "onshape/std/units.fs", version : "");
export import(path : "onshape/std/surfacetype.gen.fs", version : "");

//===================================== Plane ======================================

export const XY_PLANE = plane(vector(0, 0, 0) * meter, vector(0, 0, 1));

/**
 * A plane is represented by an origin, a normal vector, and an X direction,
 * perpendicular to the normal direction.
 * @value {{
 *      @field origin {Vector} : A 3D point, in world space.
 *      @field normal {Vector} : A 3D unit vector in world space.
 *      @field x {Vector} : A 3D unit vector in world space. Must be perpendicular to `normal`.
 * }}
 */
export type Plane typecheck canBePlane;

export predicate canBePlane(value)
{
    value is map;
    is3dLengthVector(value.origin);
    is3dDirection(value.x);
    is3dDirection(value.normal);
    abs(dot(value.x, value.normal)) < TOLERANCE.zeroAngle;
}

/**
 * Create a `Plane` on the XY plane of a specified coordinate system.
 */
export function plane(cSys is CoordSystem) returns Plane
{
    return plane(cSys.origin, cSys.zAxis, cSys.xAxis);
}

/**
 * Create a `Plane`.
 * @param origin : A 3D point in world space.
 * @param normal : A 3D vector in world space. Need not be normalized.
 * @param x      : A 3D vector in world space. Need not be normalized.
 */
export function plane(origin is Vector, normal is Vector, x is Vector) returns Plane
{
    return { "origin" : origin, "normal" : normalize(normal), "x" : normalize(x) } as Plane;
}

export function plane(origin is Vector, normal is Vector) returns Plane //Arbitrary x
{
    return plane(origin, normal, perpendicularVector(normal));
}

/**
 * Create a `Plane` from the result of a builtin call.
 * For Onshape internal use.
 */
export function planeFromBuiltin(definition is map) returns Plane
{
    return plane((definition.origin as Vector) * meter, definition.normal as Vector, definition.x as Vector);
}

/**
 * Returns the y-axis of the specified plane as a 3D Vector in world space.
 */
export function yAxis(plane is Plane) returns Vector
{
    return cross(plane.normal, plane.x);
}

/**
 * Check that two `Plane`s are the same up to tolerance, including the origin and local coordinate system.
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
 * TODO: rename this to coordSystem(plane is Plane)?
 */
export function planeToCSys(plane is Plane) returns CoordSystem
{
    return coordSystem(plane.origin, plane.x, plane.normal);
}

export function toString(value is Plane) returns string
{
    return "normal" ~ toString(value.normal) ~ " " ~ "origin" ~ toString(value.origin) ~ " " ~ "x-axis" ~ toString(value.x);
}

/**
 * TODO: description
 * @param plane
 * @param point
 */
export function project(plane is Plane, point is Vector) returns Vector
precondition
{
    is3dLengthVector(point);
}
{
    return point + plane.normal * dot(plane.normal, plane.origin - point);
}

export operator*(transform is Transform, planeRhs is Plane) returns Plane
{
    return plane(transform * planeRhs.origin,
                 inverse(transpose(transform.linear)) * planeRhs.normal, //The normal is a co-vector
                 transform.linear * planeRhs.x);
}

/**
 * TODO: description
 * @param plane
 * @param planePoint
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

export function planeToWorld(plane is Plane) returns Transform
{
    return toWorld(coordSystem(plane.origin, plane.x, plane.normal));
}

/**
 * TODO: description
 * @param plane
 * @param worldPoint
 */
export function worldToPlane(plane is Plane, worldPoint is Vector) returns Vector
{
    return fromWorld(coordSystem(plane.origin, plane.x, plane.normal), worldPoint);
}

export function worldToPlane(plane is Plane) returns Transform
{
    return fromWorld(coordSystem(plane.origin, plane.x, plane.normal));
}

/**
 * TODO: description
 * @param from
 * @param to
 */
export function transform(from is Plane, to is Plane) returns Transform
{
    return planeToWorld(to) * worldToPlane(from);
}

/**
 * TODO: description
 * @param plane
 */
export function mirrorAcross(plane is Plane) returns Transform
{
    const normalMatrix = [plane.normal] as Matrix;
    const linear = identityMatrix(3) - 2 * transpose(normalMatrix) * normalMatrix;
    return transform(linear, plane.origin - linear * plane.origin);
}

/**
 * TODO: description
 * @param p1
 * @param p2
 */
export function intersection(p1 is Plane, p2 is Plane) // Returns Line or undefined if there's no good intersection
{
    var direction = cross(p1.normal, p2.normal);
    if (norm(direction) < TOLERANCE.zeroAngle)
        return;
    direction = normalize(direction);
    const rhs = vector(dot(p1.normal, p1.origin), dot(p2.normal, p2.origin),
        dot(direction, 0.5 * (p1.origin + p2.origin)));
    const point = inverse([p1.normal, p2.normal, direction] as Matrix) * rhs;
    return line(point, direction);
}

/**
 * TODO: description
 */
export type LinePlaneIntersection typecheck canBeLinePlaneIntersection;

predicate canBeLinePlaneIntersection(value)
{
    value is map;
    (value.dim == 0 || value.dim == 1 || value.dim == -1);
    if (value.dim == 0)
        value.intersection is Vector;
    if (value.dim == 1)
        value.intersection is Line;
}

export function intersection(p is Plane, l is Line) // Returns LinePlaneIntersection or undefined
{
    const dotPr = dot(p.normal, l.direction);
    if (abs(dotPr) < TOLERANCE.zeroAngle)
    {
        if (tolerantEquals(l.origin, project(p, l.origin)))
            return { 'dim' : 1, 'intersection' : l } as LinePlaneIntersection;
        else
            return { 'dim' : -1 } as LinePlaneIntersection; //line is parallel to plane
    }
    const t = dot(p.origin - l.origin, p.normal) / dotPr;
    return { 'dim' : 0, 'intersection' : l.origin + t * l.direction } as LinePlaneIntersection;
}

// ===================================== Cone ======================================

/**
 * TODO: description
 */
export type Cone typecheck canBeCone;

export predicate canBeCone(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    isAngle(value.halfAngle);
}

/**
 * Constructs a cone from a coordinate system and a half angle.
 */
export function cone(cSys is CoordSystem, halfAngle is ValueWithUnits) returns Cone
{
    return { "coordSystem" : cSys, "halfAngle" : halfAngle } as Cone;
}

/**
 * Create a `Cone` from the result of a builtin call.
 * For Onshape internal use.
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
 * TODO: description
 */
export type Cylinder typecheck canBeCylinder;

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
 * Create a `Cylinder` from the result of a builtin call.
 * For Onshape internal use.
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
 * TODO: description
 */
export type Torus typecheck canBeTorus;

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
 * Create a `Torus` from the result of a builtin call.
 * For Onshape internal use.
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
 * TODO: description
 */
export type Sphere typecheck canBeSphere;

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
 * Create a `Sphere` from the result of a builtin call.
 * For Onshape internal use.
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

