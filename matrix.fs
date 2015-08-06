FeatureScript 189; /* Automatically generated version */
//Matrices are in row major order so that the first index is the row and the second is the column.
export import(path : "onshape/std/utils.fs", version : "");
export import(path : "onshape/std/math.fs", version : "");

export type Matrix typecheck canBeMatrix;

export predicate canBeMatrix(val)
{
    //A matrix is an array of rows each of which is an array of numbers
    @isMatrix(val);
}

export function matrixSize(matrix is Matrix) returns array //returns [rows, cols]
{
    return [@size(matrix), @size(matrix[0])];
}

export predicate isSquare(matrix is Matrix)
{
    @size(matrix) == @size(matrix[0]);
}

export function identityMatrix(size is number) returns Matrix
precondition isPositiveInteger(size);
{
    return @matrixIdentity(size) as Matrix;
}

export function zeroMatrix(rows is number, cols is number) returns Matrix
precondition
{
    isPositiveInteger(rows);
    isPositiveInteger(cols);
}
{
    return makeArray(rows, makeArray(cols, 0)) as Matrix;
}

export function rotationMatrix3d(axis is array, angle is number) returns Matrix
precondition size(axis) == 3;
{
    return @matrixRotation3d(axis, angle) as Matrix;
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

export operator*(m1 is Matrix, m2 is number) returns Matrix
{
    return @matrixMultiply(m1, m2) as Matrix;
}

export operator*(m1 is number, m2 is Matrix) returns Matrix
{
    return @matrixMultiply(m1, m2) as Matrix;
}

export function transpose(m is Matrix) returns Matrix
{
    return @matrixTranspose(m) as Matrix;
}

export function inverse(m is Matrix) returns Matrix
precondition isSquare(m);
{
    return @matrixInverse(m) as Matrix;
}

export function squaredNorm(m is Matrix) returns number
{
    return @matrixSquaredNorm(m);
}

export function norm(m is Matrix) returns number
{
    return sqrt(squaredNorm(m));
}

export function svd(m is Matrix) returns map
{
    var result = @matrixSvd(m);
    return { "u" : result.u as Matrix, "s" : result.s as Matrix, "v" : result.v as Matrix };
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


