FeatureScript 2345; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/**
 * This module contains methods for creating and working with primitive
 * surfaces: planes, cylinders, cones, spheres, and tori.
 */
import(path : "onshape/std/containers.fs", version : "2345.0");
import(path : "onshape/std/context.fs", version : "2345.0");
import(path : "onshape/std/coordSystem.fs", version : "2345.0");
import(path : "onshape/std/curveGeometry.fs", version : "2345.0");
import(path : "onshape/std/mathUtils.fs", version : "2345.0");
import(path : "onshape/std/string.fs", version : "2345.0");
import(path : "onshape/std/units.fs", version : "2345.0");
export import(path : "onshape/std/surfacetype.gen.fs", version : "2345.0");

//===================================== Plane ======================================

/**
 * The world XY plane, equivalent to `plane(vector(0, 0, 0) * meter, vector(0, 0, 1), vector(1, 0, 0))`
 */
export const XY_PLANE = plane(WORLD_ORIGIN, Z_DIRECTION, X_DIRECTION);

/**
 * The world YZ plane, equivalent to `plane(vector(0, 0, 0) * meter, vector(1, 0, 0), vector(0, 1, 0))`
 */
export const YZ_PLANE = plane(WORLD_ORIGIN, X_DIRECTION, Y_DIRECTION);

/**
 * The world XZ plane, equivalent to `plane(vector(0, 0, 0) * meter, vector(0, 1, 0), vector(0, 0, 1))`
 */
export const XZ_PLANE = plane(WORLD_ORIGIN, Y_DIRECTION, Z_DIRECTION);

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
 * perpendicular to the `normal`.
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
 * Check that two [Plane]s are the same up to tolerance, including checking that they have the same the origin and local coordinate system.
 *
 * To check if two [Plane]s are equivalent (rather than equal), use [coplanarPlanes].
 */
export predicate tolerantEquals(plane1 is Plane, plane2 is Plane)
{
    tolerantEquals(plane1.origin, plane2.origin);
    tolerantEquals(plane1.normal, plane2.normal);
    tolerantEquals(plane1.x, plane2.x);
}

/**
 * Returns `true` if the two planes are coplanar.
 */
export function coplanarPlanes(plane1 is Plane, plane2 is Plane) returns boolean
{
    const point1 = project(plane1, vector(0, 0, 0) * meter);
    const point2 = project(plane2, vector(0, 0, 0) * meter);
    return tolerantEquals(point1, point2) && parallelVectors(plane1.normal, plane2.normal);
}

/** @internal */
export function versionedCoplanarPlanes(context is Context, plane1 is Plane, plane2 is Plane) returns boolean
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1044_COPLANAR_PLANES))
    {
        return coplanarPlanes(plane1, plane2);
    }
    // Old behavior: use an incorrect check for plane normal parallel
    const point1 = project(plane1, vector(0, 0, 0) * meter);
    const point2 = project(plane2, vector(0, 0, 0) * meter);
    return tolerantEquals(point1, point2) && abs(dot(plane1.normal, plane2.normal)) > 1 - TOLERANCE.zeroAngle;
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
    return "normal " ~ toString(value.normal) ~ " origin " ~ toString(value.origin) ~ " x " ~ toString(value.x);
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
    const normalMatrix = matrix([plane.normal]);
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
    const point = inverse(matrix([plane1.normal, plane2.normal, direction])) * rhs;
    return line(point, direction);
}

/**
 * Represents an intersection between a line and a plane. Depending on the
 * line and plane, this intersection may be a point, a line, or nothing.
 *
 * @type {{
 *      @field dim {number} : Integer representing the dimension of the intersection.
 *              @eg `0` indicates that `intersection` is a 3D length [Vector].
 *              @eg `1` indicates that `intersection` is a [Line].
 *              @eg `-1` indicates that the intersection does not exist (i.e.
 *                  the line and the plane are parallel).
 *      @field intersection : `undefined` or [Vector] or [Line] (depending on `dim`) that represents the intersection.
 * }}
 */
export type LinePlaneIntersection typecheck canBeLinePlaneIntersection;

/** Typecheck for [LinePlaneIntersection] */
export predicate canBeLinePlaneIntersection(value)
{
    value is map;
    value.dim == 0 || value.dim == 1 || value.dim == -1;
    if (value.dim == 0)
        value.intersection is Vector;
    if (value.dim == 1)
        value.intersection is Line;
}

/**
 * Returns a [LinePlaneIntersection] representing the intersection between
 * `line` and `plane`.
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
 * Returns true if the point lies on the plane.
 */
export function isPointOnPlane(point is Vector, plane is Plane) returns boolean
precondition
{
    is3dLengthVector(point);
}
{
    var originToPoint = point - plane.origin;
    return abs(dot(originToPoint, plane.normal)) < (TOLERANCE.zeroLength * meter);
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
    return "half angle " ~ toString(value.halfAngle) ~ "\n" ~ "basis " ~ toString(value.coordSystem);
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
    return "radius " ~ toString(value.radius) ~ "\n" ~ "basis " ~ toString(value.coordSystem);
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
    return "radius " ~ toString(value.radius) ~ "\n" ~ "minor radius " ~ toString(value.minorRadius) ~ "\n" ~ "basis " ~ toString(value.coordSystem);
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
    return "radius " ~ toString(value.radius) ~ "\n" ~ "basis " ~ toString(value.coordSystem);
}

/**
 * A two-dimensional array of 3D position vectors. Reading across a row represents a change in v, and reading down a
 * column represents a change in u.
 */
export type ControlPointMatrix typecheck canBeControlPointMatrix;

/** Typecheck for [ControlPointMatrix] */
export predicate canBeControlPointMatrix(value)
{
    value is array;
    for (var row in value)
    {
        row is array;
        size(row) == size(value[0]);
        for (var item in row)
        {
            is3dLengthVector(item);
        }
    }
}

/**
 *  Cast a two-dimensional array of 3D position vectors to a [ControlPointMatrix].
 */
export function controlPointMatrix(value is array) returns ControlPointMatrix
precondition
{
    canBeControlPointMatrix(value);
}
{
    return value as ControlPointMatrix;
}

/**
 * The definition of a spline in 3D space.  For all matrices of the spline definition, reading across a row represents a change in v,
 * and reading down a column represents a change in u.
 * @seeAlso [bSplineSurface]
 * @type {{
 *      @field uDegree {number} : The degree of the spline in u.
 *      @field vDegree {number} : The degree of the spline in v.
 *      @field isRational {boolean} : Whether the spline is rational.
 *      @field isUPeriodic {boolean} : Whether the spline periodic in u.
 *      @field isVPeriodic {boolean} : Whether the spline periodic in v.
 *      @field controlPoints {ControlPointMatrix} : A grid of 3d control points.
 *              Must have at least `uDegree + 1` rows and `vDegree + 1` columns.
 *      @field weights {Matrix} : A matrix of unitless values with the same shape as the control points grid.
 *              @requiredIf {`rational` is `true`}
 *      @field uKnots {KnotArray} : An array of non-decreasing knots of size equal to 1 + `uDegree` + number of rows in `controlPoints`
 *      @field vKnots {KnotArray} : An array of non-decreasing knots of size equal to 1 + `vDegree` + number of columns in `controlPoints`
 * }}
 */
export type BSplineSurface typecheck canBeBSplineSurface;

/** Typecheck for [BSplineSurface] */
export predicate canBeBSplineSurface(value)
{
    value is map;
    isPositiveInteger(value.uDegree);
    isPositiveInteger(value.vDegree);
    value.isRational is boolean;
    value.isUPeriodic is boolean;
    value.isVPeriodic is boolean;

    value.controlPoints is ControlPointMatrix;
    size(value.controlPoints) > value.uDegree;
    size(value.controlPoints[0]) > value.vDegree;

    if (value.isRational)
    {
        value.weights is Matrix;
        size(value.weights) == size(value.controlPoints);
        for (var weightRow in value.weights)
        {
            size(weightRow) == size(value.controlPoints[0]);
            for (var weight in weightRow)
            {
                weight is number;
                weight >= 0;
            }
        }
    }

    value.uKnots is KnotArray;
    knotArrayIsCorrectSize(value.uKnots, value.uDegree, /* rows of control points */ size(value.controlPoints));

    value.vKnots is KnotArray;
    knotArrayIsCorrectSize(value.vKnots, value.vDegree, /* columns of control points */ size(value.controlPoints[0]));
}

/**
 * Update control points and knots for either u or v.
 */
function updateControlPointsAndKnots(controlPoints is box, weights is box, knots is box, degree is number, otherDegree is number, isPeriodic is boolean, forU is boolean)
{
    // Care about rows for u, and columns for v
    var controlPointCount = forU ? size(controlPoints[]) : size(controlPoints[][0]);

    // -----
    // If either knot array is already the correct size for the given control points, no adjustments need to be made to
    // that knot array; It is guaranteed that only a full knot array and a full set of control points will return true
    // from `knotArrayIsCorrectSize`. A full set of control points with a truncated knot array, a truncated set of control
    // points with a full knot array, and a truncated set of control points with a truncated knot array are all guaranteed
    // to return false from `knotArrayIsCorrectSize` for any degree greater than 0, because of the way their sizes interact.
    // -----
    if (knots[] != undefined && knotArrayIsCorrectSize(knots[], degree, controlPointCount))
    {
        return;
    }

    // -- Fill in control points if necessary --
    if (isPeriodic)
    {
        // Trust the first otherDegree + 1 columns (for u) or rows (for v) to determine whether the control point matrix
        // needs overlap. Need to test this many because any contiguous group of otherDegree columns or rows can validly
        // be a list of a single point (which will report that it does not need overlap), while the rows that are not a
        // single point may need overlap, or may already be overlapped.
        var controlPointArraysToTestOverlap = [];
        if (forU)
        {
            const nColumnsToCheck = min(otherDegree + 1, size(controlPoints[][0])); // Take care not to overflow
            for (var v = 0; v < nColumnsToCheck; v += 1)
            {
                controlPointArraysToTestOverlap = append(controlPointArraysToTestOverlap, mapArray(controlPoints[], function(row) { return row[v]; }));
            }
        }
        else // forV
        {
            const nRowsToCheck = min(otherDegree + 1, size(controlPoints[])); // Take care not to overflow
            for (var u = 0; u < nRowsToCheck; u += 1)
            {
                controlPointArraysToTestOverlap = append(controlPointArraysToTestOverlap, controlPoints[][u]);
            }
        }

        var needsOverlap = false;
        for (var controlPointArray in controlPointArraysToTestOverlap)
        {
            if (controlPointsNeedsOverlap(controlPointArray, degree))
            {
                // If any row needs overlap, assume that the whole matrix needs overlap across the parameter in question.
                needsOverlap = true;
                break;
            }
        }

        if (needsOverlap)
        {
            if (forU)
            {
                // Overlap entire rows
                controlPoints[] = overlapControlPoints(controlPoints[], degree);
                if (weights[] != undefined)
                {
                    weights[] = overlapControlPoints(weights[], degree);
                }
                controlPointCount = size(controlPoints[]);
            }
            else // forV
            {
                // Overlap last `degree` control points of each row
                for (var u = 0; u < size(controlPoints[]); u += 1)
                {

                    controlPoints[][u] = overlapControlPoints(controlPoints[][u], degree);
                    if (weights[] != undefined)
                    {
                        weights[][u] = overlapControlPoints(weights[][u], degree);
                    }
                }
                controlPointCount = size(controlPoints[][0]);
            }
        }
    }

    // -- Update knots --
    knots[] = createOrAdjustKnotArray(knots[], degree, controlPointCount, isPeriodic);
}

/**
 * @internal
 *
 * Create a `BSplineSurface` from the result of a builtin call.
 */
export function bSplineSurfaceFromBuiltin(definition is map) returns BSplineSurface
{
    definition.uKnots = definition.uKnots as KnotArray;
    definition.vKnots = definition.vKnots as KnotArray;
    definition.surfaceType = SurfaceType.SPLINE;
    definition.controlPoints = mapArray(definition.controlPoints, function(row)
            {
                return mapArray(row, function(controlPoint)
                    {
                        // Unrolled / inlined for performance
                        return [controlPoint[0] * meter, controlPoint[1] * meter, controlPoint[2] * meter] as Vector;
                    });
            }) as ControlPointMatrix;
    if (definition.isRational)
        definition.weights = definition.weights as Matrix;
    return definition as BSplineSurface;
}

/**
 * Returns a new [BSplineSurface], adding knot padding and control point overlap as necessary.
 * @example
 * ```
 * opCreateBSplineSurface(context, id + "bSplineSurface1", {
 *             "bSplineSurface" : bSplineSurface({
 *                         "uDegree" : 2,
 *                         "vDegree" : 2,
 *                         "isUPeriodic" : false,
 *                         "isVPeriodic" : false,
 *                         "controlPoints" : controlPointMatrix([
 *                                     [vector(-2,  2, 0) * inch, vector(-1,  2, 0) * inch, vector(0,  2, 0) * inch, vector(1,  2, 0) * inch, vector(2,  2, 0) * inch],
 *                                     [vector(-2,  1, 0) * inch, vector(-1,  1, 0) * inch, vector(0,  1, 0) * inch, vector(1,  1, 0) * inch, vector(2,  1, 0) * inch],
 *                                     [vector(-2,  0, 0) * inch, vector(-1,  0, 0) * inch, vector(0,  0, 1) * inch, vector(1,  0, 0) * inch, vector(2,  0, 0) * inch],
 *                                     [vector(-2, -2, 0) * inch, vector(-1, -2, 0) * inch, vector(0, -2, 0) * inch, vector(1, -2, 0) * inch, vector(2, -2, 0) * inch]
 *                                 ]),
 *                         "uKnots" : knotArray([0, .1, 1]), // Will be padded to [0, 0, 0, .1, 1, 1, 1]
 *                         "vKnots" : knotArray([0, 1/3, 2/3, 1]) // Same as default when no knots provided.  Will be padded to [0, 0, 0, 1/3, 2/3, 1, 1, 1]
 *                     })
 *         });
 * ```
 * Creates a new spline surface on the XY plane with a protrusion at the origin, falling back to the XY plane more quickly in the +Y direction.
 * @example
 * ```
 * opCreateBSplineSurface(context, id + "bSplineSurface1", {
 *             "bSplineSurface" : bSplineSurface({
 *                         "uDegree" : 2,
 *                         "vDegree" : 1,
 *                         "isUPeriodic" : true,
 *                         "isVPeriodic" : false,
 *                         "controlPoints" : controlPointMatrix([
 *                                     [vector(0,  0, 1) * inch, vector(-1,  0, 0) * inch, vector(-2,  0, -1) * inch],
 *                                     [vector(1,  1, 1) * inch, vector( 0,  1, 0) * inch, vector(-1,  1, -1) * inch],
 *                                     [vector(2,  0, 1) * inch, vector( 1,  0, 0) * inch, vector( 0,  0, -1) * inch],
 *                                     [vector(1, -1, 1) * inch, vector( 0, -1, 0) * inch, vector(-1, -1, -1) * inch]
 *                                     // Will be overlapped by repeating the first two rows
 *                                 ]),
 *                         "uKnots" : knotArray([0, .25, .5, .75, 1]), // Same as default when no knots provided. Will be padded to [-.5, -.25, 0, .25, .5, .75, 1, 1.25, 1.5]
 *                         "vKnots" : knotArray([0, .5, 1]) // Same as default when no knots provided.  Will be padded to [0, 0, .5, 1, 1]
 *                     })
 *         });
 * ```
 * Creates a new spline surface which is a tube surrounding the origin, sheared in the X direction.
 * @param definition {{
 *      @field uDegree {number} : The degree of the spline in u.
 *              @autocomplete `2`
 *      @field vDegree {number} : The degree of the spline in v.
 *              @autocomplete `1`
 *      @field isUPeriodic {boolean} : Whether the spline periodic in u.
 *              @autocomplete `true`
 *      @field isVPeriodic {boolean} : Whether the spline periodic in v.
 *              @autocomplete `false`
 *      @field controlPoints {ControlPointMatrix} : A matrix of control points. See [BSplineSurface] for specific detail.
 *              If u or v is periodic, you may provide the necessary overlap, or provide control points without any
 *              overlap.  If no overlap is provided, `degree` overlapping control point rows or columns (corresponding to
 *              the first `degree` control point rows or columns) will be added. (unless you provide a set of knots that
 *              show no overlap is necessary).
 * @eg ```
 * controlPointMatrix([
 *     [vector(-1, -1, -1) * inch, vector(-1, 0, 0) * inch, vector(-1, -2, 1) * inch],
 *     [vector( 0,  1, -1) * inch, vector( 0, 2, 0) * inch, vector( 0,  0, 1) * inch],
 *     [vector( 1, -1, -1) * inch, vector( 1, 0, 0) * inch, vector( 1, -2, 1) * inch]
 * ])
 * ```
 *      @field weights {array} : @optional A matrix of weights. See [BSplineSurface] for specific detail.
 *      @field uKnots {KnotArray} : @optional An array of knots. See [BSplineSurface] for specific detail.  If knots are not provided
 *              a uniform parameterization will be created such that the u parameterization exists on the range `[0, 1]`. For
 *              non-periodic u with `n` control point rows, you may provide the full set of `n + degree + 1` knots,
 *              or you may provide `n - degree + 1` knots, and multiplicity will by padded onto the ends (which has the
 *              effect of clamping the surface to the control points in the first and last rows).  For periodic u with
 *              `n` unique control points (and optionally an additional `degree` overlapping control points), you
 *              may provide the full set of `n + 2 * degree + 1` knots, or you may provide `n + 1` knots, and the periodic
 *              knots will be padded onto the ends.
 *      @field vKnots {KnotArray} : @optional See `uKnots`.
 * }}
 */
export function bSplineSurface(definition is map)
precondition
{
    definition.uDegree is number;
    definition.vDegree is number;
    definition.isUPeriodic is boolean;
    definition.isVPeriodic is boolean;
    definition.controlPoints is ControlPointMatrix;
    definition.weights is undefined || definition.weights is Matrix;
    definition.uKnots is undefined || definition.uKnots is KnotArray;
    definition.vKnots is undefined || definition.vKnots is KnotArray;
}
{
    var controlPoints = new box(definition.controlPoints);
    var weights = new box(definition.weights);
    var uKnots = new box(definition.uKnots);
    var vKnots = new box(definition.vKnots);

    // Do v knots first, because it will be more efficient to replicate entire knot rows for u after updating knot columns for v.
    updateControlPointsAndKnots(controlPoints, weights, vKnots, definition.vDegree, definition.uDegree, definition.isVPeriodic, false);
    updateControlPointsAndKnots(controlPoints, weights, uKnots, definition.uDegree, definition.vDegree, definition.isUPeriodic, true);

    return {
        'uDegree' : definition.uDegree,
        'vDegree' : definition.vDegree,
        'isRational' : weights[] != undefined,
        'isUPeriodic' : definition.isUPeriodic,
        'isVPeriodic' : definition.isVPeriodic,
        'controlPoints' : controlPoints[],
        'weights' : weights[],
        'uKnots' : uKnots[],
        'vKnots' : vKnots[]
    } as BSplineSurface;
}

// ===================================== Mesh ======================================

/**
 * A `MeshFaceParameter` is a 2D array unitless array.
 * It is functionnally indentical to a 2D vector but because there is no guarantee of continuity for mesh parameters,
 * it does not make sense to expose vector math for it.
 */
export type MeshFaceParameter typecheck canBeMeshFaceParameter;

/** Typecheck for [MeshFaceParameter] */
export predicate canBeMeshFaceParameter(value)
{
    value is array;
    @size(value) == 2;
    value[0] is number;
    value[1] is number;
}

/**
 * Make a [MeshFaceParameter] from an array.
 */
export function meshFaceParameter(value is array) returns MeshFaceParameter
precondition
{
    @size(value) == 2;
    value[0] is number;
    value[1] is number;
}
{
    return value as MeshFaceParameter;
}
