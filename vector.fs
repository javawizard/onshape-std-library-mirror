FeatureScript 2368; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

//Vector math
import(path : "onshape/std/containers.fs", version : "2368.0");
import(path : "onshape/std/math.fs", version : "2368.0");
import(path : "onshape/std/units.fs", version : "2368.0");
import(path : "onshape/std/matrix.fs", version : "2368.0");
import(path : "onshape/std/string.fs", version : "2368.0");

/**
 * A `Vector` is a non-empty array.  It should contain numbers or lengths.
 *
 * Operators `+`, `-`, `*`, and `/` are overloaded for vectors,
 * and other operations such as dot product are available.
 * If a vector does not contain numbers or lengths, operations
 * that assume number-like properties may fail.
 */
export type Vector typecheck canBeVector;

/** Typecheck for [Vector] */
export predicate canBeVector(value)
{
    value is array;
    @size(value) > 0;
}

/**
 * Make a Vector from an array.
 */
export function vector(value is array) returns Vector
precondition
{
    @size(value) > 0;
}
{
    return value as Vector;
}

/**
 * Construct a 2-dimensional vector.
 */
export function vector(x, y) returns Vector
{
    return [x, y] as Vector;
}

/**
 * Construct a 3-dimensional vector.
 */
export function vector(x, y, z) returns Vector
{
    return [x, y, z] as Vector;
}

/**
 * True for a `Vector` where all members are values with length units.
 * @ex `vector([1, 2, 3, 4, 5]) * inch`
 */
export predicate isLengthVector(value)
{
    value is Vector;
    for (var v in value)
        isLength(v);
}

/**
 * True for a `Vector` where all members are simple `number`s.
 * @ex `vector([1, 2, 3, 4, 5])`
 */
export predicate isUnitlessVector(value)
{
    value is Vector;
    for (var v in value)
        v is number;
}

/**
 * True for a single 2D length `Vector`
 * @ex `vector(0.5, 1) * inch`
 */
export predicate is2dPoint(value)
{
    isLengthVector(value);
    size(value) == 2;
}

/**
 * True for an `array` where all members are 2D lengths.
 * @ex `[vector(0, 0) * inch, vector(0, 1) * inch, vector(1, 0) * inch]`
 */
export predicate is2dPointVector(value)
{
   value is array;
   for (var point in value)
        is2dPoint(point);
}

/**
 * True for a unitless 2D `Vector` that is normalized (i.e. has length `1`)
 * @ex `vector(0, 1)`
 */
export predicate is2dDirection(value)
{
    isUnitlessVector(value);
    @size(value) == 2;
    abs(squaredNorm(value) - 1) < TOLERANCE.zeroAngle;
}

/**
 * True for a 3D `Vector` where all members are values with length units.
 * @ex `vector(0, 1.5, 30) * inch`
 */
export predicate is3dLengthVector(value)
{
    isLengthVector(value);
    @size(value) == 3;
}

/**
 * True for a unitless 3D `Vector` that is normalized (i.e. has length `1`)
 * @ex `vector(0, 0, 1)`
 */
export predicate is3dDirection(value)
{
    isUnitlessVector(value);
    @size(value) == 3;
    abs(squaredNorm(value) - 1) < TOLERANCE.zeroAngle;
}

/**
 * Make an array filled with 0.
 * @example `zeroVector(3)` is equivalent to `vector(0, 0, 0)`
 */
export function zeroVector(size is number) returns Vector
precondition
{
    isPositiveInteger(size);
}
{
    return makeArray(size, 0) as Vector;
}

/**
 * Returns the squared length of a vector.
 * This is slightly faster to calculate than the length.
 */
export function squaredNorm(vector is Vector)
{
    return dot(vector, vector);
}

/**
 * Returns the length (norm) of a vector.
 */
export function norm(vector is Vector)
{
    return sqrt(squaredNorm(vector));
}

export operator+(vector1 is Vector, vector2 is Vector) returns Vector
precondition
{
    @size(vector1) == @size(vector2);
}
{
    for (var i = 0; i < @size(vector1); i += 1)
    {
        vector1[i] += vector2[i];
    }
    return vector1;
}

export operator-(vector1 is Vector, vector2 is Vector) returns Vector
precondition
{
    @size(vector1) == @size(vector2);
}
{
    for (var i = 0; i < @size(vector1); i += 1)
    {
        vector1[i] -= vector2[i];
    }
    return vector1;
}

export operator-(vector is Vector) returns Vector
{
    for (var i = 0; i < @size(vector); i += 1)
    {
        vector[i] = -vector[i];
    }
    return vector;
}

/**
 * Returns the dot product of two vectors.
 */
export function dot(vector1 is Vector, vector2 is Vector)
precondition
{
    @size(vector1) == @size(vector2);
}
{
    var dot = vector1[0] * vector2[0];
    for (var i = 1; i < @size(vector1); i += 1)
    {
        dot += vector1[i] * vector2[i];
    }
    return dot;
}

/**
 * Returns the cross product of two 3-dimensional vectors.
 */
export function cross(vector1 is Vector, vector2 is Vector) returns Vector
precondition
{
    @size(vector1) == 3;
    @size(vector2) == 3;
}
{
    const nx = vector1[1] * vector2[2] - vector2[1] * vector1[2];
    const ny = vector1[2] * vector2[0] - vector2[2] * vector1[0];
    const nz = vector1[0] * vector2[1] - vector2[0] * vector1[1];
    return [nx, ny, nz] as Vector;
}

/**
 * Returns the angle between two 3-dimensional vectors. Values are within the range `[0, PI] * radian`.
 * @ex `angleBetween(X_DIRECTION, Y_DIRECTION)` equals `PI/2 * radian`
 * @ex `angleBetween(Y_DIRECTION, X_DIRECTION)` equals `PI/2 * radian`
 *
 * A plane is fitted to the two vectors and the shortest angle between them is measured on that plane.
 */
export function angleBetween(vector1 is Vector, vector2 is Vector) returns ValueWithUnits
precondition
{
    @size(vector1) == 3;
    @size(vector2) == 3;
}
{
    return atan2(norm(cross(vector1, vector2)), dot(vector1, vector2));
}

/**
 * Returns the counterclockwise angle between two 3-dimensional vectors as witnessed from the tip of a third 3-dimensional vector. Values are within the range `(-PI, PI] * radian` with negative values indicating clockwise angles.
 * @ex `angleBetween(X_DIRECTION, Y_DIRECTION, Z_DIRECTION)` equals `PI/2 * radian`
 * @ex `angleBetween(Y_DIRECTION, X_DIRECTION, Z_DIRECTION)` equals `-PI/2 * radian`
 *
 * The first two vectors are projected onto a plane perpendicular to the reference vector and the angle is measured according to that projection.
 */
export function angleBetween(vector1 is Vector, vector2 is Vector, ref is Vector) returns ValueWithUnits
precondition
{
    @size(vector1) == 3;
    @size(vector2) == 3;
    @size(ref) == 3;
}
{
    var dotProd = dot(vector1, vector2);
    var area = dot(cross(vector1, vector2), normalize(ref));
    return atan2(area, dotProd);
}

/**
 * Returns the (unitless) result of normalizing vector. Throws if the input is zero-length.
 * @param vector : A Vector with any units.
 */
export function normalize(vector is Vector) returns Vector
{
    return @normalize(vector) as Vector;
}

export operator*(vector is Vector, scalar) returns Vector
{
    for (var i = 0; i < @size(vector); i += 1)
    {
        vector[i] *= scalar;
    }
    return vector;
}

export operator*(scalar, vector is Vector) returns Vector
{
    for (var i = 0; i < @size(vector); i += 1)
    {
        vector[i] = scalar * vector[i];
    }
    return vector;
}

export operator*(vec1 is Vector, vec2 is Vector) returns Vector
{
    throw "Cannot multiply two vectors. Did you mean to use dot() or cross()?";
}

export operator*(matrix is Matrix, vector is Vector) returns Vector
precondition
{
    matrixSize(matrix)[1] == @size(vector);
}
{
    if (vector[0] is ValueWithUnits)
    {
        return (@matrixMultiply(matrix, stripUnits(vector)) as Vector) *
            ({ "value" : 1, "unit" : vector[0].unit } as ValueWithUnits);
    }
    else if (vector[0] is number)
    {
        return @matrixMultiply(matrix, vector) as Vector;
    }
    //Multiply "by hand"
    const transposed = transpose(matrix);
    var result = (transposed[0] as Vector) * vector[0];
    for (var i = 1; i < @size(vector); i += 1)
    {
        result += transposed[i] * vector[i];
    }
    return result as Vector;
}

export operator*(vector is Vector, matrix is Matrix) returns Vector
{
    throw "Cannot right-multiply vector by matrix. Matrices must be multiplied on the left.";
}

export operator/(vector is Vector, scalar) returns Vector
{
    for (var i = 0; i < @size(vector); i += 1)
    {
        vector[i] /= scalar;
    }
    return vector;
}

/**
 * Project the `source` vector onto the `target` vector.  Equivalent to `target * dot(source, target) / squaredNorm(target)`.
 */
export function project(target is Vector, source is Vector) returns Vector
{
    return target * (dot(source, target) / squaredNorm(target));
}

/**
 * Returns a vector perpendicular to the given vector.
 * The choice of which perpendicular vector to return
 * is arbitrary but consistent for the same input.
 * The returned vector is unitless and of length 1.
 */
export function perpendicularVector(vec is Vector) returns Vector
precondition @size(vec) == 3;
{
    if (vec[0] is ValueWithUnits)
        vec = [vec[0].value, vec[1].value, vec[2].value] as Vector;
    if (squaredNorm(vec) < TOLERANCE.zeroLength * TOLERANCE.zeroLength)
        return vector(1, 0, 0);
    var different = vector(0, 0, 0);
    // The numbers are "random" and close to 1.  They're to avoid instability in cases likely to occur.
    if (abs(vec[0]) > 1.036663652861932668633 * abs(vec[1]))
    {
        if (abs(vec[0]) > .951702989392233451722 * abs(vec[2]))
            different[2] = 1;
        else
            different[1] = 1;
    }
    else
    {
        if (abs(vec[1]) > .920419947455385938102 * abs(vec[2]))
            different[0] = 1;
        else
            different[1] = 1;
    }
    return normalize(cross(different, vec));
}

/**
 * Construct a 3D rotation matrix that represents the minimum rotation that takes the normalized `from` vector to the
 * normalized `to` vector. The inputs may have any units.
 */
export function rotationMatrix3d(from is Vector, to is Vector) returns Matrix
precondition
{
    @size(from) == 3;
    @size(to) == 3;
}
{
    const axis = cross(from, to);
    if (squaredNorm(axis) < TOLERANCE.zeroLength * TOLERANCE.zeroLength)
    {
        if (dot(from, to) > 0)
        {
            return identityMatrix(3);
        }
        else
        {
            const perp = perpendicularVector(from);
            return rotationMatrix3d(perp, PI * radian);
        }
    }
    return rotationMatrix3d(axis, atan2(norm(axis), dot(from, to)));
}

/**
 * Construct a 3D matrix representing a counterclockwise (looking against the axis) rotation
 * around the given axis by the given rotation angle.
 */
export function rotationMatrix3d(axis is Vector, angle is ValueWithUnits) returns Matrix
precondition @size(axis) == 3;
{
    return @matrixRotation3d(axis, angle.value) as Matrix;
}

/**
 * Returns the scalar triple product, a dot (b cross c), of
 * three 3-dimensional vectors.
 */
export function scalarTripleProduct(vector1 is Vector, vector2 is Vector, vector3 is Vector)
precondition
{
    @size(vector1) == 3;
    @size(vector2) == 3;
    @size(vector3) == 3;
}
{
    return dot(vector1, cross(vector2, vector3));
}

export function toString(value is Vector) returns string
{
    var str = "(" ~ toString(value[0]);
    for (var i = 1; i < @size(value); i += 1)
    {
        str = str ~ ", " ~ toString(value[i]);
    }
    str = str ~ ")";
    return str;
}

/**
 * Returns true if two vectors designate the same point (within tolerance) or the same direction (within tolerance).
 */
export predicate tolerantEquals(point1 is Vector, point2 is Vector)
{
    if (point1[0] is number) // Assume direction
        squaredNorm(point1 - point2) < TOLERANCE.zeroAngle * TOLERANCE.zeroAngle;
    else
        squaredNorm(point1 - point2).value < TOLERANCE.zeroLength * TOLERANCE.zeroLength;
}

/**
 * Returns true if two vectors are parallel (within tolerance).
 */
export function parallelVectors(vector1 is Vector, vector2 is Vector) returns boolean
{
    const crossP2 = squaredNorm(cross(vector1, vector2));
    const v1v2 = squaredNorm(vector1) * squaredNorm(vector2);
    return crossP2 < v1v2 * TOLERANCE.zeroAngle * TOLERANCE.zeroAngle;
}

/**
 * Returns true if two vectors are perpendicular (within tolerance).
 */
export function perpendicularVectors(vector1 is Vector, vector2 is Vector) returns boolean
{
    const dotP = dot(vector1, vector2);
    const v1v2 = squaredNorm(vector1) * squaredNorm(vector2);
    return dotP * dotP < v1v2 * TOLERANCE.zeroAngle * TOLERANCE.zeroAngle;
}

/**
 * Groups points into clusters. Two points farther than tolerance apart are
 * guaranteed to be in separate clusters. A set of points all within tolerance
 * of each other that has no other points within tolerance is guaranteed to be
 * a single cluster.
 *
 * @returns {array} : Array of arrays, where each array is a cluster of nearby
 *          points, represented as indices into points array.
 */
export function clusterPoints(points is array, tolerance is ValueWithUnits) returns array
precondition
{
    for (var point in points)
    {
        is3dLengthVector(point);
    }
    isLength(tolerance);
}
{
    return @clusterPoints(stripUnits(points), tolerance.value);
}

