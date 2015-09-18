FeatureScript ✨; /* Automatically generated version */
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/matrix.fs", version : "");
import(path : "onshape/std/units.fs", version : "");
import(path : "onshape/std/vector.fs", version : "");

/**
 * TODO: description
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

export function transform(linear is Matrix, translation is Vector) returns Transform
precondition
{
    matrixSize(linear) == [3, 3];
    is3dLengthVector(translation);
}
{
    return { "linear" : linear, "translation" : translation } as Transform;
}

export function transform(translation is Vector) returns Transform
precondition
{
    is3dLengthVector(translation);
}
{
    return { "linear" : identityMatrix(3), "translation" : translation } as Transform;
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function transformFromBuiltin(definition is map) returns Transform
{
    return transform(definition.linear as Matrix, (definition.translation as Vector) * meter);
}

/**
 * TODO: description
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

export function inverse(t is Transform) returns Transform
{
    const linear = inverse(t.linear);
    return transform(linear, -linear * t.translation);
}

// This lives here because we can see definitions of both ValueWithUnits and Matrix.
export function rotationMatrix3d(axis is array, angle is ValueWithUnits) returns Matrix
precondition size(axis) == 3;
{
    return @matrixRotation3d(axis, angle.value) as Matrix;
}

