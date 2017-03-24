FeatureScript 543; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/containers.fs", version : "543.0");
import(path : "onshape/std/math.fs", version : "543.0");
import(path : "onshape/std/matrix.fs", version : "543.0");
import(path : "onshape/std/units.fs", version : "543.0");
import(path : "onshape/std/vector.fs", version : "543.0");

/**
 * Type typically representing a change of position, orientation in 3D space
 * (other affine transformations such as scaling and sheering can also be
 * represented).
 *
 * Transforms are commonly used with their `*` operator overloads to easily work
 * with geometry in multiple coordinate systems.
 *
 * @example `transform * (vector(1, 1, 1) * inch)` yields a point which is the
 *      given point, transformed by the `transform`.
 * @example `transform2 * transform1` yields a new transform which is equivalent
 *      to applying `transform1` followed by `transform2`.
 *
 * When applying a single transform transform, the linear potion is applied
 * first, followed by the vector translation. Generally, the individual fields
 * on this type don't need to be directly used, and everything you need can be
 * accomplished through the operator overloads above, and the functions in this
 * module and the `coordSystem` module.
 *
 * [rotationAround], [scaleUniformly], [toWorld(CoordSystem)] and [fromWorld(CoordSystem)] return
 * transforms for common operations.
 *
 * @type {{
 *      @field linear {Matrix} : A linear motion, which is generally a rotation,
 *              but can also be a scaling, inversion, or sheering.
 *      @field translation {Vector} : A 3D translation vector.
 * }}
 */
export type Transform typecheck canBeTransform;

/** Typecheck for [Transform] */
export predicate canBeTransform(value)
{
    value is map;
    value.linear is Matrix;
    matrixSize(value.linear) == [3, 3];
    is3dLengthVector(value.translation);
}

/**
 * Construct a [Transform] using the matrix argument for rotation
 * and scaling and the vector argument for translation.
 */
export function transform(linear is Matrix, translation is Vector) returns Transform
precondition
{
    matrixSize(linear) == [3, 3];
    is3dLengthVector(translation);
}
{
    return { "linear" : linear, "translation" : translation } as Transform;
}

/**
 * Construct a [Transform] that translates without rotation or scaling.
 */
export function transform(translation is Vector) returns Transform
precondition
{
    is3dLengthVector(translation);
}
{
    return { "linear" : identityMatrix(3), "translation" : translation } as Transform;
}

export function transform(value is map) returns Transform
{
    return value as Transform;
}

/**
 * @internal
 * Create a [Transform] from the result of a builtin call.
 */
export function transformFromBuiltin(definition is map) returns Transform
{
    return transform(definition.linear as Matrix, (definition.translation as Vector) * meter);
}

/**
 * Construct a transform that does nothing (no rotation, scaling, or translation).
 */
export function identityTransform() returns Transform
{
    return { "linear" : identityMatrix(3), "translation" : vector(0, 0, 0) * meter } as Transform;
}

/**
 * Check that two [Transform]s are the same up to tolerance.
 */
export predicate tolerantEquals(transform1 is Transform, transform2 is Transform)
{
    tolerantEquals(transform1.translation, transform2.translation);
    squaredNorm(transform1.linear - transform2.linear) < 9 * TOLERANCE.zeroAngle * TOLERANCE.zeroAngle;
}

export operator*(t1 is Transform, t2 is Transform) returns Transform
{
    return { "linear" : t1.linear * t2.linear, "translation" : t1 * t2.translation } as Transform;
}

export operator*(t is Transform, v is Vector) returns Vector
precondition
{
    is3dLengthVector(v);
}
{
    return t.translation + t.linear * v;
}

/**
 * Compute the inverse of a [Transform], such that
 * `inverse(t) * t == identityTransform()`.
 */
export function inverse(t is Transform) returns Transform
{
    const linear = inverse(t.linear);
    return transform(linear, -linear * t.translation);
}

/**
 * Returns a [Transform] that represents a uniform scaling around
 * the origin.
 */
export function scaleUniformly(scale is number) returns Transform
{
    return transform(identityMatrix(3) * scale, vector(0, 0, 0) * meter);
}

/**
 * Returns a [Transform] that represents a uniform scaling around
 * the given point.
 */
export function scaleUniformly(scale is number, point is Vector) returns Transform
{
    return transform(identityMatrix(3) * scale, point * (1 - scale));
}

