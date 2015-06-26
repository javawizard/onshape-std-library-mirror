FeatureScript 156; /* Automatically generated version */
//Vector math
export import(path:"onshape/std/math.fs", version : "");
export import(path:"onshape/std/utils.fs", version : "");
export import(path:"onshape/std/units.fs", version : "");
export import(path:"onshape/std/matrix.fs", version : "");

export type Vector typecheck canBeVector;

export predicate canBeVector(value)
{
    value is array;
    @size(value) > 0;
}

export function vector(value is array) returns Vector
{
    return value as Vector;
}

export function vector(x, y) returns Vector
{
    return [x, y] as Vector;
}

export function vector(x, y, z) returns Vector
{
    return [x, y, z] as Vector;
}

export predicate isLengthVector(value)
{
    value is Vector;
    for(var i = 0; i < @size(value); i += 1)
    {
        isLength(value[i]);
    }
}

export predicate isUnitlessVector(value)
{
    value is Vector;
    for(var i = 0; i < @size(value); i += 1)
    {
        value[i] is number;
    }
}

export predicate is3dLengthVector(value)
{
    isLengthVector(value);
    @size(value) == 3;
}

export predicate is3dDirection(value)
{
    isUnitlessVector(value);
    @size(value) == 3;
    abs(squaredNorm(value) - 1) < TOLERANCE.zeroAngle;
}

export function zeroVector(size is number) returns Vector
precondition
{
    isPositiveInteger(size);
}
{
    return makeArray(size, 0) as Vector;
}

export function squaredNorm(vector is Vector)
{
    return dotProduct(vector, vector);
}

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
    for(var i = 0; i < @size(vector1); i += 1)
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
    for(var i = 0; i < @size(vector1); i += 1)
    {
        vector1[i] -= vector2[i];
    }
    return vector1;
}

export operator-(vector is Vector) returns Vector
{
    for(var i = 0; i < @size(vector); i += 1)
    {
        vector[i] = -vector[i];
    }
    return vector;
}

export function dotProduct(vector1 is Vector, vector2 is Vector)
precondition
{
    @size(vector1) == @size(vector2);
}
{
    var dot = vector1[0] * vector2[0];
    for(var i = 1; i < @size(vector1); i += 1)
    {
        dot += vector1[i] * vector2[i];
    }
    return dot;
}

export function crossProduct(vector1 is Vector, vector2 is Vector) returns Vector
precondition
{
    @size(vector1) == 3;
    @size(vector2) == 3;
}
{
    var nx = vector1[1] * vector2[2] - vector2[1] * vector1[2];
    var ny = vector1[2] * vector2[0] - vector2[2] * vector1[0];
    var nz = vector1[0] * vector2[1] - vector2[0] * vector1[1];
    return [nx, ny, nz] as Vector;
}

export function angleBetween(vector1 is Vector, vector2 is Vector)
precondition
{
    @size(vector1) == 3;
    @size(vector2) == 3;
}
{
    return atan2(norm(crossProduct(vector1, vector2)), dotProduct(vector1, vector2));
}

export function normalize(vector is Vector) returns Vector
{
    return vector / norm(vector);
}

export operator*(vector is Vector, scalar) returns Vector
{
    for(var i = 0; i < @size(vector); i += 1)
    {
        vector[i] *= scalar;
    }
    return vector;
}

export operator*(scalar, vector is Vector) returns Vector
{
    for(var i = 0; i < @size(vector); i += 1)
    {
        vector[i] = scalar * vector[i];
    }
    return vector;
}

export operator*(matrix is Matrix, vector is Vector) returns Vector
precondition
{
    matrixSize(matrix)[1] == @size(vector);
}
{
    if(vector[0] is ValueWithUnits)
    {
        return (@matrixMultiply(matrix, stripUnits(vector)) as Vector) *
                    ({ "value" : 1, "unit" : vector[0].unit } as ValueWithUnits);
    }
    else if(vector[0] is number)
    {
        return @matrixMultiply(matrix, vector) as Vector;
    }
    //Multiply "by hand"
    var transposed = transpose(matrix);
    var result = (transposed[0] as Vector) * vector[0];
    for(var i = 1; i < @size(vector); i += 1)
    {
        result += transposed[i] * vector[i];
    }
    return result as Vector;
}

export operator/(vector is Vector, scalar) returns Vector
{
    for(var i = 0; i < @size(vector); i += 1)
    {
        vector[i] /= scalar;
    }
    return vector;
}

export function project(vector1 is Vector, vector2 is Vector) returns Vector
{
    var dot = dotProduct(vector1, vector2);
    return vector2 * (dot / squaredNorm(vector2));
}

export function perpendicularVector(vec is Vector) returns Vector
precondition @size(vec) == 3;
{
    if(vec[0] is ValueWithUnits)
        vec = [vec[0].value, vec[1].value, vec[2].value] as Vector;
    if(squaredNorm(vec) < TOLERANCE.zeroLength * TOLERANCE.zeroLength)
        return vector(1, 0, 0);
    var different = vector(0, 0, 0);
    // The numbers are "random" and close to 1.  They're to avoid instability in cases likely to occur.
    if(abs(vec[0]) >  1.036663652861932668633 * abs(vec[1]))
    {
        if(abs(vec[0]) > .951702989392233451722 * abs(vec[2]))
            different[2] = 1;
        else
            different[1] = 1;
    }
    else
    {
        if(abs(vec[1]) > .920419947455385938102 * abs(vec[2]))
            different[0] = 1;
        else
            different[1] = 1;
    }
    return normalize(crossProduct(different, vec));
}

export function rotationMatrix3d(from is Vector, to is Vector) returns Matrix
precondition
{
    @size(from) == 3;
    @size(to) == 3;
}
{
    var axis = crossProduct(from, to);
    if(squaredNorm(axis) < TOLERANCE.zeroLength * TOLERANCE.zeroLength)
    {
        if(dotProduct(from, to) > 0)
        {
            return identityMatrix(3);
        }
        else
        {
            var perp = perpendicularVector(from);
            return rotationMatrix3d(perp, PI);
        }
    }
    return rotationMatrix3d(axis, @atan2(norm(axis), dotProduct(from, to)));
}

export function scalarTripleProduct(vector1 is Vector, vector2 is Vector, vector3 is Vector)
precondition
{
    @size(vector1) == 3;
    @size(vector2) == 3;
    @size(vector3) == 3;
}
{
    var v2Crossv3 = crossProduct(vector2, vector3);
    return dotProduct(vector1, v2Crossv3);
}

export function toString(value is Vector) returns string
{
    var str = "(" ~ toString(value[0]);
    for(var i = 1; i < @size(value); i += 1)
    {
        str = str ~ ", " ~ toString(value[i]);
    }
    str = str ~ ")";
    return str;
}

export function samePoint(point1 is Vector, point2 is Vector) returns boolean
{
    return stripUnits(squaredNorm(point1 - point2)) < TOLERANCE.zeroLength * TOLERANCE.zeroLength;
}

export function parallelVectors(vector1 is Vector, vector2 is Vector) returns boolean
{
    var dotP = stripUnits(dotProduct(vector1, vector2));
    var v1v2 = stripUnits(squaredNorm(vector1) * squaredNorm(vector2));
    return (v1v2 - dotP * dotP) < 0.5 * (v1v2 * TOLERANCE.zeroAngle * TOLERANCE.zeroAngle);
}

export function perpendicularVectors(vector1 is Vector, vector2 is Vector) returns boolean
{
    var dotP = stripUnits(dotProduct(vector1, vector2));
    var v1v2 = stripUnits(squaredNorm(vector1) * squaredNorm(vector2));
    return  dotP * dotP < v1v2 * TOLERANCE.zeroAngle * TOLERANCE.zeroAngle;
}
