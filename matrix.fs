FeatureScript 1867; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

//Matrices are in row major order so that the first index is the row and the second is the column.
import(path : "onshape/std/containers.fs", version : "1867.0");
import(path : "onshape/std/math.fs", version : "1867.0");

/**
 * A `Matrix` is an array of rows, all the same size, each of which
 * is an array of numbers
 */
export type Matrix typecheck canBeMatrix;

/** Typecheck for [Matrix] */
export predicate canBeMatrix(val)
{
    @isMatrix(val);
}

/**
 * Return a 2 element array containing the numbers of rows and columns
 * of a matrix.
 */
export function matrixSize(matrix is Matrix) returns array //returns [rows, cols]
{
    return [@size(matrix), @size(matrix[0])];
}

/**
 * Check whether a matrix is square.
 */
export predicate isSquare(matrix is Matrix)
{
    @size(matrix) == @size(matrix[0]);
}

/**
 * Cast a two-dimensional array to a matrix.
 */
export function matrix(value is array) returns Matrix
precondition
{
    canBeMatrix(value);
}
{
    return value as Matrix;
}

/**
 * Construct an identity matrix of a given dimension.
 */
export function identityMatrix(size is number) returns Matrix
precondition isPositiveInteger(size);
{
    return @matrixIdentity(size) as Matrix;
}

/**
 * Construct an all-zero matrix of a given dimension.
 */
export function zeroMatrix(rows is number, cols is number) returns Matrix
precondition
{
    isPositiveInteger(rows);
    isPositiveInteger(cols);
}
{
    return makeArray(rows, makeArray(cols, 0)) as Matrix;
}

/**
 * Given an array of `diagonalValues` of size `n`, construct an `n`x`n` matrix
 * which has those values along its main diagonal (starting in the top-left),
 * and `0` everywhere else.
 */
export function diagonalMatrix(diagonalValues is array) returns Matrix
precondition
{
    for (var value in diagonalValues)
    {
        value is number;
    }
}
{
    var matrixSize = size(diagonalValues);
    var result = zeroMatrix(matrixSize, matrixSize);
    for (var i = 0; i < matrixSize; i += 1)
    {
        result[i][i] = diagonalValues[i];
    }
    return result;
}

export operator+(m1 is Matrix, m2 is Matrix) returns Matrix
precondition matrixSize(m1) == matrixSize(m2);
{
    return @matrixSum(m1, m2) as Matrix;
}

export operator-(m1 is Matrix, m2 is Matrix) returns Matrix
precondition matrixSize(m1) == matrixSize(m2);
{
    return @matrixDifference(m1, m2) as Matrix;
}

/**
 * Construct a matrix by multiplying corresponding elements of two
 * matrices (which must be the same size).
 */
export function cwiseProduct(m1 is Matrix, m2 is Matrix) returns Matrix
precondition matrixSize(m1) == matrixSize(m2);
{
    return @matrixCwiseProduct(m1, m2) as Matrix;
}

export operator-(m1 is Matrix) returns Matrix
{
    return @matrixNegate(m1) as Matrix;
}

export operator*(m1 is Matrix, m2 is Matrix) returns Matrix
precondition @size(m1[0]) == @size(m2);
{
    return @matrixMultiply(m1, m2) as Matrix;
}

export operator*(m is Matrix, n is number) returns Matrix
{
    return @matrixMultiply(m, n) as Matrix;
}

export operator/(m is Matrix, n is number) returns Matrix
{
    return @matrixMultiply(m, (1 / n)) as Matrix;
}

export operator*(m1 is number, m2 is Matrix) returns Matrix
{
    return @matrixMultiply(m1, m2) as Matrix;
}

/**
 * Return the transpose of a matrix.
 */
export function transpose(m is Matrix) returns Matrix
{
    return @matrixTranspose(m) as Matrix;
}

/**
 * Compute the inverse of a matrix.  Throws an exception
 * if the matrix is not square.  If the matrix is singular
 * the resulting matrix will contain infinities.
 */
export function inverse(m is Matrix) returns Matrix
precondition isSquare(m);
{
    return @matrixInverse(m) as Matrix;
}

/**
 * Return the sum of the squares of matrix elements.
 */
export function squaredNorm(m is Matrix) returns number
{
    return @matrixSquaredNorm(m);
}

/**
 * Return the square root of the sum of the squares of matrix elements.
 */
export function norm(m is Matrix) returns number
{
    return sqrt(squaredNorm(m));
}

/**
 * Compute the singular value decomposition of a matrix,
 * i.e. `s`, `u`, and `v`, where `m == u * s * transpose(v)` and s is a
 * diagonal matrix of singular values.
 *
 * @param m {Matrix} : an n-by-p matrix.
 * @return {{
 *     @field u {Matrix} : An n-by-n unitary matrix
 *     @field s {Matrix} : An n-by-p diagonal matrix
 *     @field v {Matrix} : A p-by-p unitary matrix
 * }}
 */
export function svd(m is Matrix) returns map
{
    const result = @matrixSvd(m);
    return { "u" : result.u as Matrix, "s" : result.s as Matrix, "v" : result.v as Matrix };
}

/**
 * Return the determinant of the matrix.
 */
export function determinant(m is Matrix) returns number
{
   return @matrixDeterminant(m);
}

export function toString(value is Matrix) returns string
{
    var result = "[[";
    var firstRow = true;
    for (var row in value)
    {
        if (!firstRow)
            result ~= "]\n[";
        firstRow = false;
        var firstCol = true;
        for (var col in row)
        {
            if (!firstCol)
                result ~= ", ";
            firstCol = false;
            result ~= col;
        }
    }
    return result ~ "]]\n";
}


