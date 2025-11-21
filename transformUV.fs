FeatureScript 2815; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/math.fs", version : "2815.0");
import(path : "onshape/std/matrix.fs", version : "2815.0");
import(path : "onshape/std/units.fs", version : "2815.0");
import(path : "onshape/std/vector.fs", version : "2815.0");

/**
 * A `TransformUV` represents a change of position and orientation in unitless 2D space.
 *
 * A `TransformUV` contains a linear portion (rotation, scaling, or shearing), which is applied
 * first, and a translation vector, which is applied second.
 *
 * @type {{
 *      @field linear {Matrix} : A linear motion, which is generally a rotation,
 *              but can also be a scaling, inversion, or shearing.
 *      @field translation {Vector} : A 2D translation vector.
 * }}
 */
export type TransformUV typecheck canBeTransformUV;

/**
 * True for a single 2D unitless `Vector`
 * @ex `vector(0.5, 1)`
 */
export predicate isUvVector(value)
{
    isUnitlessVector(value);
    @size(value) == 2;
}

/** Typecheck for [Transform] */
export predicate canBeTransformUV(value)
{
    value is map;
    value.linear is Matrix;
    matrixSize(value.linear) == [2, 2];
    isUvVector(value.translation);
}

/**
 * Construct a [TransformUV] using the matrix argument for rotation
 * and scaling and the vector argument for translation.
 */
export function transformUV(linear is Matrix, translation is Vector) returns TransformUV
precondition
{
    matrixSize(linear) == [2, 2];
    isUvVector(translation);
}
{
    return { "linear" : linear, "translation" : translation } as TransformUV;
}

/**
 * Construct a [TransformUV] that translates without rotation or scaling.
 */
export function transformUV(translation is Vector) returns TransformUV
precondition
{
    isUvVector(translation);
}
{
    return { "linear" : identityMatrix(2), "translation" : translation } as TransformUV;
}

export function transformUV(value is map) returns TransformUV
{
    return value as TransformUV;
}

/**
 * Construct a transform that does nothing (no rotation, scaling, or translation).
 */
export function identityTransformUV() returns TransformUV
{
    return { "linear" : identityMatrix(2), "translation" : vector(0, 0) } as TransformUV;
}

/**
 * Check that two [TransformUV]s are the same up to tolerance.
 */
export predicate tolerantEquals(transform1 is TransformUV, transform2 is TransformUV)
{
    tolerantEquals(transform1.translation, transform2.translation);
    squaredNorm(transform1.linear - transform2.linear) < 9 * TOLERANCE.zeroAngle * TOLERANCE.zeroAngle;
}

export operator*(t1 is TransformUV, t2 is TransformUV) returns TransformUV
{
    return { "linear" : t1.linear * t2.linear, "translation" : t1 * t2.translation } as TransformUV;
}

export operator*(t is TransformUV, v is Vector) returns Vector
precondition
{
    isUvVector(v);
}
{
    return t.translation + t.linear * v;
}

/**
 * Compute the inverse of a [TransformUV], such that
 * `inverse(t) * t == identityTransform()`.
 */
export function inverse(t is TransformUV) returns TransformUV
{
    const linear = inverse(t.linear);
    return transformUV(linear, -linear * t.translation);
}

/**
 * Returns a [TransformUV] that represents a uniform scaling around
 * the origin.
 */
export function scaleUniformlyUV(scale is number) returns TransformUV
{
    return transformUV(identityMatrix(2) * scale, vector(0, 0));
}

/**
 * Returns a [TransformUV] that represents two independent scalings along the X and Y axes,
 * centered around the origin.
 */
export function scaleNonuniformly(xScale is number, yScale is number) returns TransformUV
{
    return transformUV(diagonalMatrix([xScale, yScale]), vector(0, 0));
}

/**
 * Returns a [TransformUV] that represents a rotation around the origin with
 * the given angle.
 */
export function rotate(angle is ValueWithUnits) returns TransformUV
{
    var rotation = identityMatrix(2);
    rotation[0][0] = cos(angle);
    rotation[1][0] = sin(angle);
    rotation[0][1] =  -rotation[1][0];
    rotation[1][1] = rotation[0][0];
    return transformUV(rotation, vector(0, 0));
}

