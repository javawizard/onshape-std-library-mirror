FeatureScript 2411; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/curvetype.gen.fs", version : "2411.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "2411.0");
import(path : "onshape/std/coordSystem.fs", version : "2411.0");
import(path : "onshape/std/mathUtils.fs", version : "2411.0");
import(path : "onshape/std/units.fs", version : "2411.0");

// ===================================== Line ======================================

/**
 * The global X axis, equivalent to `line(vector(0, 0, 0) * meter, vector(1, 0, 0))`
 */
export const X_AXIS = line(WORLD_ORIGIN, X_DIRECTION);

/**
 * The global Y axis, equivalent to `line(vector(0, 0, 0) * meter, vector(0, 1, 0))`
 */
export const Y_AXIS = line(WORLD_ORIGIN, Y_DIRECTION);

/**
 * The global Z axis, equivalent to `line(vector(0, 0, 0) * meter, vector(0, 0, 1))`
 */
export const Z_AXIS = line(WORLD_ORIGIN, Z_DIRECTION);

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
 * Check that two [Line]s are the same up to tolerance, including checking that they have the same origin.
 *
 * To check if two [Line]s are equivalent (rather than equal), use [collinearLines].
 */
export predicate tolerantEquals(line1 is Line, line2 is Line)
{
    tolerantEquals(line1.origin, line2.origin);
    tolerantEquals(line1.direction, line2.direction);
}

/**
 * Returns `true` if the two lines are collinear.
 */
export function collinearLines(line1 is Line, line2 is Line) returns boolean
{
    const point1 = project(line1, vector(0, 0, 0) * meter);
    const point2 = project(line2, vector(0, 0, 0) * meter);
    return tolerantEquals(point1, point2) && parallelVectors(line1.direction, line2.direction);
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

/**
 * Represents an intersection between two lines. Depending on the
 * lines, this intersection may be a point, a line, or nothing.
 *
 * @type {{
 *      @field dim {number} : Integer representing the dimension of the intersection.
 *              @eg `0` indicates that `intersection` is a 3D length [Vector].
 *              @eg `1` indicates that `intersection` is a [Line]. (i.e. the lines are collinear)
 *              @eg `-1` indicates that the intersection does not exist.
 *      @field intersection : `undefined` or [Vector] or [Line] (depending on `dim`) that represents the intersection.
 * }}
 */
export type LineLineIntersection typecheck canBeLineLineIntersection;

/** Typecheck for [LineLineIntersection] */
export predicate canBeLineLineIntersection(value)
{
    value is map;
    value.dim == 0 || value.dim == 1 || value.dim == -1;
    if (value.dim == 0)
        value.intersection is Vector;
    if (value.dim == 1)
        value.intersection is Line;
}

/**
 * Returns a [LineLineIntersection] representing the intersection between two lines.  If the lines are collinear, `line1`
 * will be stored in the `intersection` field of that `LineLineIntersection`.
 */
export function intersection(line1 is Line, line2 is Line) returns LineLineIntersection
{
    const crossDirections = cross(line1.direction, line2.direction);
    if (tolerantEquals(crossDirections, vector(0, 0, 0))) // Lines are parallel
    {
        if (isPointOnLine(line1.origin, line2))
            return { "dim" : 1, "intersection" : line1 } as LineLineIntersection; // lines are collinear
        else
            return { "dim" : -1 } as LineLineIntersection; // lines are parallel but not collinear
    }
    const p1ToP2 = line2.origin - line1.origin;
    if (!tolerantEquals(0 * meter, dot(p1ToP2, crossDirections)))
        return { "dim" : -1 } as LineLineIntersection; // No intersection
    const p1ToP2CrossLine2Dir = cross(p1ToP2, line2.direction);
    const t = dot(p1ToP2CrossLine2Dir, crossDirections) / squaredNorm(crossDirections);
    return { "dim" : 0, "intersection" : line1.origin + t * line1.direction } as LineLineIntersection;
}

/**
 * Returns true if the point lies on the line.
 */
export function isPointOnLine(point is Vector, line is Line) returns boolean
precondition
{
    is3dLengthVector(point);
}
{
    return tolerantEquals(project(line, point), point);
}

export function toString(value is Line) returns string
{
    return "direction " ~ toString(value.direction) ~ "\n" ~ "origin " ~ toString(value.origin);
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
    return "radius " ~ toString(value.radius) ~ "\n" ~ "center " ~ toString(value.coordSystem.origin);
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
    return "majorRadius " ~ toString(value.majorRadius) ~ "\n" ~ "minorRadius " ~ toString(value.minorRadius) ~ "\n" ~ "center " ~ toString(value.coordSystem.origin);
}

/**
 *  An array of non-decreasing numbers representing the knots of a spline
 */
export type KnotArray typecheck canBeKnotArray;

/** Typecheck for [KnotArray] */
export predicate canBeKnotArray(value)
{
    value is array;
    for (var i = 0; i < size(value); i += 1)
    {
        value[i] is number;
        i == 0 || value[i] >= value[i - 1];
    }
}

/**
 * Cast an array on non-decreasing numbers to a KnotArray
 */
export function knotArray(value is array) returns KnotArray
precondition
{
    canBeKnotArray(value);
}
{
    return value as KnotArray;
}

/**
 * Assure that `size(knotArray)` is `1 + degree + nControlPoints`
 */
export predicate knotArrayIsCorrectSize(knots is KnotArray, degree is number, nControlPoints is number)
{
    size(knots) == degree + nControlPoints + 1;
}

/**
 * The definition of a spline in 3D or 2D world space, or unitless 2D parameter space.
 * @seeAlso [bSplineCurve(map)]
 * @type {{
 *      @field degree {number} : The degree of the spline.
 *      @field dimension {number} : The dimension of the spline. Must be 2 or 3.
 *      @field isRational {boolean} : Whether the spline is rational.
 *      @field isPeriodic {boolean} : Whether the spline is periodic.
 *      @field controlPoints {array} : An array of control points of the required dimension. Size should be at least
 *              degree + 1. 2D spline control points can have world space units, or be unitless if they are intended
 *              to represent locations in parameter space (e.g. as a boundary input to [opCreateBSplineSurface]).
 *      @field weights {array} : An array of unitless values with the same size as the control points array.
 *              @requiredIf{`isRational` is `true`}
 *      @field knots {KnotArray} : An array of non-decreasing knots of size equal to `1 + degree + size(controlPoints)`
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
    size(value.controlPoints) > value.degree;
    for (var controlPoint in value.controlPoints)
    {
        // 3d control points must have units.  2d can can have units or be unitless, depending on whether the spline describes
        // a 2d curve in 3d space (such as a sketch curve), or a 2d curve in unitless parameter space (such as boundary
        // curves for opCreateBSplineCurve)
        isLengthVector(controlPoint) || (value.dimension == 2 && isUnitlessVector(controlPoint));
        // All control points should either have units, or be unitless
        isLengthVector(value.controlPoints[0]) == isLengthVector(controlPoint);
        size(controlPoint) == value.dimension;
    }

    !value.isRational || (value.weights is array && size(value.weights) == size(value.controlPoints));
    if (value.isRational)
    {
        for (var weight in value.weights)
        {
            weight is number;
            weight >= 0;
        }
    }

    value.knots is KnotArray;
    knotArrayIsCorrectSize(value.knots, value.degree, size(value.controlPoints));
}

/**
 * @internal
 * Check whether a periodic control point array already has overlap, or needs overlap
 */
export function controlPointsNeedsOverlap(controlPoints is array, degree is number) returns boolean
{
    const nControlPoints = size(controlPoints);
    if (nControlPoints <= degree)
    {
        return true;
    }
    var numOverlaps = 0;
    for (var i = 0; i < degree; i += 1)
    {
        if (tolerantEquals(controlPoints[i], controlPoints[nControlPoints - degree + i]))
        {
            numOverlaps += 1;
        }
    }
    if (numOverlaps != 0 && numOverlaps != degree)
    {
        throw "Found " ~ numOverlaps ~ " overlapping control points.  Expected 0 or " ~ degree;
    }
    return numOverlaps == 0;
}

/**
 * @internal
 * Add overlap to the given control point (or weight) array by the given degree
 */
export function overlapControlPoints(controlPoints is array, degree is number) returns array
{
    if (size(controlPoints) < degree)
    {
        throw "Not enough control points to overlap control point array";
    }

    for (var i = 0; i < degree; i += 1)
    {
        controlPoints = append(controlPoints, controlPoints[i]);
    }
    return controlPoints;
}

/**
 * @internal
 * Make an appropriate knot array for the given parameters which is uniform across its parameterization
 */
export function makeUniformKnotArray(degree is number, nControlPoints is number, isPeriodic is boolean) returns KnotArray
{
    if (nControlPoints < (degree + 1))
    {
        throw "Must have at least " ~ (degree + 1) ~ " control points for a spline of degree " ~ degree;
    }

    const nKnots = nControlPoints + degree + 1;

    // `degree` knots on either end of the knot array are helpers that differ in value based on `isPeriodic`.  Between
    //     these two ends, the center of knots array goes from 0 -> 1 in uniform increments.
    // If the spline is periodic, the ends will be overlapped to create periodicity. For example, the knot array of a
    //     11 control point, degree 3 spline will be:
    //     [-0.375, -0.25, -0.125, 0, 0.125, 0.25, 0.375, 0.5, 0.625, 0.75, 0.875, 1, 1.125, 1.25, 1.375]
    // If the spline is not periodic, multiplicities of 0 and 1 will be created at the ends (to clamp the spline to its
    //     start and end control points). For example, the knot array of a 11 control point, degree 3 spline will be:
    //     [0, 0, 0, 0, 0.125, 0.25, 0.375, 0.5, 0.625, 0.75, 0.875, 1, 1, 1, 1]
    var knots = makeArray(nKnots);
    const increment = 1 / (nKnots - (2 * degree) - 1);
    for (var i = 0; i < nKnots; i += 1)
    {
        knots[i] = (i - degree) * increment;
        if (!isPeriodic)
        {
            knots[i] = min(max(knots[i], 0), 1);
        }
    }

    return knotArray(knots);
}

/**
 * Create padding for the start or end of a periodic wrapped knot array.  Padding is calculated by copying the marginal
 * difference between the knots at the other end of the unpadded knot array. For example, a knot array:
 * [k0, k1, k2, k3, k4, k5]
 * will look like this once padded (using degree 3 as an example here):
 * [k-3 = k-2 - (k3 - k2), k-2 = k-1 - (k4 - k3), k-1 = k0 - (k5 - k4), k0, k1, k2, k3, k4, k5, k6 = k5 + (k1 - k0), k7 = k6 + (k2 - k1), k9 = k8 + (k3 - k2)]
 */
function makePeriodicKnotArrayPadding(unpaddedKnotArray is KnotArray, degree is number, forStart is boolean) returns array
{
    const nUnpaddedKnots = size(unpaddedKnotArray);
    const finalKnotIndex = nUnpaddedKnots - 1;

    if (nUnpaddedKnots < (degree + 1))
    {
        throw "Not enough knot spans to pad periodic knot array";
    }

    var padding = makeArray(degree);
    if (forStart)
    {
        // Reverse the knot array and pretend we are making padding for the end, to simplify index management for the for loop
        unpaddedKnotArray = reverse(unpaddedKnotArray);
    }

    var previousKnot = unpaddedKnotArray[finalKnotIndex];
    for (var i = 0; i < degree; i += 1)
    {
        const knotDiff = unpaddedKnotArray[i + 1] - unpaddedKnotArray[i];
        padding[i] = previousKnot + knotDiff;
        previousKnot = padding[i];
    }

    if (forStart)
    {
        // Reverse back so that the knot array can be used as start padding
        padding = reverse(padding);
    }

    return padding;
}

/**
 * @internal
 * Pad an existing knot array with `degree` additional knots on both sides to make it a valid array for the given degree and number of control points.
 */
export function padKnotArray(unpaddedKnotArray is KnotArray, degree is number, nControlPoints is number, isPeriodic is boolean) returns KnotArray
{
    const unpaddedKnotArraySize = size(unpaddedKnotArray);
    if (unpaddedKnotArraySize != ((nControlPoints + degree + 1) - (2 * degree)))
    {
        throw "unpaddedKnotArray passed into padKnotArray should be missing `degree` knots on either side";
    }

    var startPadding;
    var endPadding;
    if (isPeriodic)
    {
        // This ensure that we do not go out of bounds while filling padding
        if (nControlPoints < degree * 2)
        {
            throw "Cannot pad a periodic array that does not repeat its first `degree` control points";
        }

        startPadding = makePeriodicKnotArrayPadding(unpaddedKnotArray, degree, true);
        endPadding = makePeriodicKnotArrayPadding(unpaddedKnotArray, degree, false);
    }
    else
    {
        // Repeat start knots at the beginning and end knots at the end
        startPadding = makeArray(degree, unpaddedKnotArray[0]);
        endPadding = makeArray(degree, unpaddedKnotArray[unpaddedKnotArraySize - 1]);
    }

    return knotArray(concatenateArrays([startPadding, unpaddedKnotArray, endPadding]));
}

/**
 * @internal
 * Adjust knot array (if necessary) if it exists, or create a new uniform knot array if `undefined` is passed.
 */
export function createOrAdjustKnotArray(knots, degree is number, nControlPoints is number, isPeriodic is boolean)
precondition
{
    knots is undefined || knots is KnotArray;
}
{
    if (knots is undefined)
    {
        knots = makeUniformKnotArray(degree, nControlPoints, isPeriodic);
    }
    else if (!knotArrayIsCorrectSize(knots, degree, nControlPoints))
    {
        knots = padKnotArray(knots, degree, nControlPoints, isPeriodic);
    }

    if (!knotArrayIsCorrectSize(knots, degree, nControlPoints))
    {
        throw "Unexpected error: knot array could not be adjusted";
    }
    return knots;
}

/**
 * Returns a new [BSplineCurve], adding knot padding and control point overlap as necessary.
 * @example
 * ```
 * opCreateBSplineCurve(context, id + "bSplineCurve1", {
 *             "bSplineCurve" : bSplineCurve({
 *                         "degree" : 2,
 *                         "isPeriodic" : false,
 *                         "controlPoints" : [
 *                                 vector(0, 0, 0) * inch,
 *                                 vector(1, 0, 0) * inch,
 *                                 vector(1, 1, 0) * inch,
 *                                 vector(0, 1, 0) * inch],
 *                         "knots" : knotArray([0, .2, 1]) // Will be padded to [0, 0, 0, .2, 1, 1, 1]
 *                     })
 *         });
 * ```
 * Creates a spline starting at the origin, moving first quickly along the x-axis (toward the second point), and finishing on the y-axis.
 * @example
 * ```
 * opCreateBSplineCurve(context, id + "bSplineCurve1", {
 *             "bSplineCurve" : bSplineCurve({
 *                         "degree" : 3,
 *                         "isPeriodic" : true,
 *                         "controlPoints" : [
 *                                 vector(0, 0, 0) * inch,
 *                                 vector(1, 0, 0) * inch,
 *                                 vector(1, 1, 0) * inch,
 *                                 vector(0, 1, 0) * inch], // Will be overlapped by repeating the first 3 points
 *                         "knots" : knotArray([0, .25, .5, .75, 1]) // Same as default when no knots provided. Will be padded to [-.75, -.5, -.25, 0, .25, .5, .75, 1, 1.25, 1.5, 1.75]
 *                     })
 *         });
 * ```
 * Creates a closed, curvature-continuous, symmetric curve between the four given points.
 * @param definition {{
 *      @field degree {number} : The degree of the spline.
 *              @autocomplete `3`
 *      @field isPeriodic {boolean} : Whether the spline is periodic.
 *              @autocomplete `true`
 *      @field controlPoints {array} : An array of control points. See [BSplineCurve] for specific detail.  For periodic
 *              splines, you may provide the necessary overlap, or provide control points without any overlap.  If no
 *              overlap is provided, `degree` overlapping control points (corresponding to the first `degree` control
 *              points) will be added to the end of the control points list (unless you provide a set of knots that
 *              show no overlap is necessary).
 *              @eg `[vector(-1, 1, 0) * inch, vector(1, 1, 0) * inch, vector(1, -1, 0) * inch, vector(-1, -1, 0) * inch]`
 *      @field weights {array} : @optional An array of weights. See [BSplineCurve] for specific detail.
 *      @field knots {KnotArray} : @optional An array of knots. See [BSplineCurve] for specific detail.  If knots are not provided
 *              a uniform parameterization wil be created such that the curve exists on the parameter range `[0, 1]`. For
 *              non-periodic curves with `n` control points, you may provide the full set of `n + degree + 1` knots,
 *              or you may provide `n - degree + 1` knots, and multiplicity will by padded onto the ends (which has the
 *              effect of clamping the spline to its two endpoints).  For periodic curves with `n` unique control points
 *              (and optionally an additional `degree` overlapping control points), you may provide the full set
 *              of `n + 2 * degree + 1` knots, or you may provide `n + 1` knots, and the periodic knots will be padded
 *              onto the ends.
 * }}
 */
export function bSplineCurve(definition is map) returns BSplineCurve
precondition
{
    definition.degree is number;
    definition.isPeriodic is boolean;
    definition.controlPoints is array && size(definition.controlPoints) > 0 && definition.controlPoints[0] is Vector;
    definition.weights is undefined || definition.weights is array;
    definition.knots is undefined || definition.knots is KnotArray;
}
{
    const dimension = size(definition.controlPoints[0]);

    var controlPoints = definition.controlPoints;
    var nControlPoints = size(controlPoints);
    var weights = definition.weights;
    var knots = definition.knots;

    // -----
    // If the knot array is already the correct size for the given control points, no adjustments need to be made; It is
    // guaranteed that only a full knot array and a full control point array will return true from `knotArrayIsCorrectSize`.
    // A full control point array with a truncated knot array, a truncated control point array with a full knot array, and
    // a truncated control point array with a truncated knot array are all guaranteed to return false from `knotArrayIsCorrectSize`
    // for any degree greater than 0, because of the way their sizes interact.
    // -----
    if (knots == undefined || !knotArrayIsCorrectSize(knots, definition.degree, nControlPoints))
    {
        // Fill in control points if necessary
        if (definition.isPeriodic && controlPointsNeedsOverlap(controlPoints, definition.degree))
        {
            controlPoints = overlapControlPoints(controlPoints, definition.degree);
            if (weights != undefined)
            {
                weights = overlapControlPoints(weights, definition.degree);
            }
            nControlPoints = size(controlPoints);
        }

        // Update knots
        knots = createOrAdjustKnotArray(knots, definition.degree, nControlPoints, definition.isPeriodic);
    }

    return {
        'degree' : definition.degree,
        'dimension' : dimension,
        'isRational' : weights != undefined,
        'isPeriodic' : definition.isPeriodic,
        'controlPoints' : controlPoints,
        'weights' : weights,
        'knots' : knots
    } as BSplineCurve;
}

const controlPointFromBuiltin2d = function(controlPoint)
        {
            return controlPoint as Vector; // control points in UV space do not have units
        };

const controlPointFromBuiltin3d = function(controlPoint)
        {
            // Unrolled / inlined for performance
            return [controlPoint[0] * meter, controlPoint[1] * meter, controlPoint[2] * meter] as Vector;
        };

/**
 * @internal
 *
 * Create a `BSplineCurve` from the result of a builtin call.
 */
export function bSplineCurveFromBuiltin(definition is map) returns BSplineCurve
{
    definition.knots = definition.knots as KnotArray;
    definition.curveType = CurveType.SPLINE;
    definition.controlPoints = mapArray(definition.controlPoints,
                                        definition.dimension == 2 ? controlPointFromBuiltin2d : controlPointFromBuiltin3d);
    return definition as BSplineCurve;
}

annotation { "Deprecated" : "Use [bSplineCurve(map)]" }
export function bSplineCurve(degree is number, isPeriodic is boolean, controlPoints is array, knots is KnotArray) returns BSplineCurve
{
    return bSplineCurve({
        'degree' : degree,
        'isPeriodic' : isPeriodic,
        'controlPoints' : controlPoints,
        'knots' : knots
    });
}

annotation { "Deprecated" : "Use [bSplineCurve(map)]" }
export function bSplineCurve(degree is number, isPeriodic is boolean, controlPoints is array, weights is array, knots is KnotArray) returns BSplineCurve
{
    return bSplineCurve({
        'degree' : degree,
        'isPeriodic' : isPeriodic,
        'controlPoints' : controlPoints,
        'weights' : weights,
        'knots' : knots
    });
}

