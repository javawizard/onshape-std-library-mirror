FeatureScript 244; /* Automatically generated version */
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/matrix.fs", version : "");
import(path : "onshape/std/units.fs", version : "");
import(path : "onshape/std/vector.fs", version : "");

/**
 * A transform is rotation and scaling followed by a vector translation.
 */
export type Transform typecheck canBeTransform;

export predicate canBeTransform(value)
{
    value is map;
    value.linear is Matrix;
    matrixSize(value.linear) == [3, 3];
    is3dLengthVector(value.translation);
}

export function transform(value is map) returns Transform
{
    return value as Transform;
}

/**
 * Construct a `Transform` using the matrix argument for rotation
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
 * Construct a `Transform` that translates without rotation or scaling.
 */
export function transform(translation is Vector) returns Transform
precondition
{
    is3dLengthVector(translation);
}
{
    return { "linear" : identityMatrix(3), "translation" : translation } as Transform;
}

/**
 * Create a `Transform` from the result of a builtin call.
 * For Onshape internal use.
 */
export function transformFromBuiltin(definition is map) returns Transform
{
    return transform(definition.linear as Matrix, (definition.translation as Vector) * meter);
}

/**
 * Construct a transform that does nothing, no rotation, scaling, or translation.
 */
export function identityTransform() returns Transform
{
    return { "linear" : identityMatrix(3), "translation" : vector(0, 0, 0) * meter } as Transform;
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
 * Compute the inverse of a `Transform`, such that
 * `inverse(t) * t == identityTransform()`.
 */
export function inverse(t is Transform) returns Transform
{
    const linear = inverse(t.linear);
    return transform(linear, -linear * t.translation);
}

