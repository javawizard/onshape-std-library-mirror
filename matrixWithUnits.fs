FeatureScript 2641; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/containers.fs", version : "2641.0");
import(path : "onshape/std/math.fs", version : "2641.0");
import(path : "onshape/std/units.fs", version : "2641.0");
import(path : "onshape/std/matrix.fs", version : "2641.0");
import(path : "onshape/std/vector.fs", version : "2641.0");
import(path : "onshape/std/string.fs", version : "2641.0");

/**
 * A `MatrixWithUnits` is analogous to `ValueWithUnits`, but wrapping a matrix.
 */
export type MatrixWithUnits typecheck canBeMatrixWithUnits;

/** Typecheck for [MatrixWithUnits] */
export predicate canBeMatrixWithUnits(value)
{
    value is map;
    value.value is Matrix;
    value.unit is UnitSpec;
}

/**
 * Gets an element of a MatrixWithUnits, returning a ValueWithUnits.
 */
export function get(matrix is MatrixWithUnits, i is number, j is number) returns ValueWithUnits
{
    return { "value" : matrix.value[i][j], "unit" : matrix.unit } as ValueWithUnits;
}

/**
 * Return a 2-element array containing the numbers of rows and columns
 * of a matrix.
 */
export function matrixSize(matrix is MatrixWithUnits) returns array
{
    return [@size(matrix.value), @size(matrix.value[0])];
}

/**
 * Can be used to construct a MatrixWithUnits from a Matrix and a ValueWithUnits, e.g.,
 * matrix = matrix0 * meter;
 */

export operator*(lhs is Matrix, rhs is ValueWithUnits) returns MatrixWithUnits
{
    return { "value" : lhs * rhs.value, "unit" : rhs.unit } as MatrixWithUnits;
}

export operator*(lhs is ValueWithUnits, rhs is Matrix) returns MatrixWithUnits
{
    return { "value" : lhs.value * rhs, "unit" : lhs.unit } as MatrixWithUnits;
}

export operator+(lhs is MatrixWithUnits, rhs is MatrixWithUnits) returns MatrixWithUnits
precondition lhs.unit == rhs.unit;
{
    lhs.value += rhs.value;
    return lhs;
}

export operator-(lhs is MatrixWithUnits, rhs is MatrixWithUnits) returns MatrixWithUnits
precondition lhs.unit == rhs.unit;
{
    lhs.value -= rhs.value;
    return lhs;
}

export operator-(rhs is MatrixWithUnits) returns MatrixWithUnits
{
    rhs.value = -rhs.value;
    return rhs;
}

export operator*(lhs is number, rhs is MatrixWithUnits) returns MatrixWithUnits
{
    rhs.value *= lhs;
    return rhs;
}

export operator*(lhs is MatrixWithUnits, rhs is number) returns MatrixWithUnits
{
    lhs.value *= rhs;
    return lhs;
}

export operator*(lhs is ValueWithUnits, rhs is MatrixWithUnits) returns MatrixWithUnits
{
    var newUnit = unitProduct(lhs.unit, rhs.unit);

    if (newUnit == unitless)
        return lhs.value * rhs.value;

    return { "value" : lhs.value * rhs.value, "unit" : newUnit } as MatrixWithUnits;
}

export operator*(lhs is MatrixWithUnits, rhs is ValueWithUnits)
{
    var newUnit = unitProduct(lhs.unit, rhs.unit);

    if (newUnit == unitless)
        return lhs.value * rhs.value;

    return { "value" : lhs.value * rhs.value, "unit" : newUnit } as MatrixWithUnits;
}

export operator*(lhs is MatrixWithUnits, rhs is MatrixWithUnits)
{
    var newUnit = unitProduct(lhs.unit, rhs.unit);

    if (newUnit == unitless)
        return lhs.value * rhs.value;

    return { "value" : lhs.value * rhs.value, "unit" : newUnit } as MatrixWithUnits;
}

export operator*(lhs is Matrix, rhs is MatrixWithUnits) returns MatrixWithUnits
precondition @size(lhs[0]) == @size(rhs.value);
{
    return { "value" : lhs * rhs.value, "unit" : rhs.unit } as MatrixWithUnits;
}

export operator*(lhs is MatrixWithUnits, rhs is Matrix) returns MatrixWithUnits
precondition @size(lhs.value[0]) == @size(rhs);
{
    return { "value" : lhs.value * rhs, "unit" : lhs.unit } as MatrixWithUnits;
}

export operator/(lhs is MatrixWithUnits, rhs is number) returns MatrixWithUnits
{
    lhs.value /= rhs;
    return lhs;
}

export operator/(lhs is MatrixWithUnits, rhs is ValueWithUnits)
{
    return lhs * reciprocal(rhs);
}

export operator*(matrix is MatrixWithUnits, vector is Vector) returns Vector
precondition
{
    matrixSize(matrix.value)[1] == @size(vector);
}
{
    if (vector[0] is ValueWithUnits)
    {
        var newUnit = unitProduct(matrix.unit, vector[0].unit);

        if (newUnit == unitless)
            return matrix.value * (stripUnits(vector) as Vector);

        return (matrix.value * (stripUnits(vector) as Vector)) * ({ "value" : 1, "unit" : newUnit } as ValueWithUnits);
    }
    else if (vector[0] is number)
    {
        return (matrix.value * vector) * ({ "value" : 1, "unit" : matrix.unit } as ValueWithUnits);
    }
    else
    {
        const transposed = transpose(matrix);
        var result = (transposed[0] as Vector) * vector[0];
        for (var i = 1; i < @size(vector); i += 1)
        {
            result += transposed[i] * vector[i];
        }
        return result as Vector;
    }
}

export function toString(value is MatrixWithUnits) returns string
{
    var result = toString(value.value);
    for (var unit in value.unit)
    {
        result ~= " " ~ unit.key;
        if (unit.value != 1)
            result ~= "^" ~ unit.value;
    }
    return result;
}
