export import(path:"onshape/std/transform.fs", version : "");

export type CoordSystem typecheck canBeCoordSystem;

export predicate canBeCoordSystem(value)
{
    value is map;
    is3dLengthVector(value.origin);
    is3dDirection(value.xAxis);
    is3dDirection(value.zAxis);
    abs(dotProduct(value.xAxis, value.zAxis)) < TOLERANCE.zeroAngle;
}

export function coordSystem(origin is Vector, xAxis is Vector, zAxis is Vector) returns CoordSystem
{
    return {"origin" : origin, "xAxis" : normalize(xAxis), "zAxis" : normalize(zAxis)} as CoordSystem;
}

export function coordSystemFromBuiltin(cSys is map) returns CoordSystem
{
    return coordSystem((cSys.origin as Vector) * meter, cSys.xAxis as Vector, cSys.zAxis as Vector);
}

export function toWorld(cSys is CoordSystem) returns Transform
{
    var rotation = transpose([cSys.xAxis, crossProduct(cSys.zAxis, cSys.xAxis), cSys.zAxis] as Matrix);
    return transform(rotation, cSys.origin);
}

export function fromWorld(cSys is CoordSystem, worldPoint is Vector) returns Vector
precondition
{
    is3dLengthVector(worldPoint);
}
{
    worldPoint -= cSys.origin;
    return vector(dotProduct(worldPoint, cSys.xAxis), dotProduct(worldPoint, crossProduct(cSys.zAxis, cSys.xAxis)));
}

export function fromWorld(cSys is CoordSystem) returns Transform
{
    var rotation = [cSys.xAxis, crossProduct(cSys.zAxis, cSys.xAxis), cSys.zAxis] as Matrix;
    return transform(rotation, rotation * -cSys.origin);
}

export operator*(transform is Transform, cSys is CoordSystem) returns CoordSystem
{
    return coordSystem(transform * cSys.origin,
                       transform.linear * (cSys.xAxis as Vector),
                       inverse(transpose(transform.linear)) * cSys.zAxis);
}

export function toString(cSys is CoordSystem) returns string
{
    return "origin" ~ toString(cSys.origin) ~ "\n" ~ "x-Axis" ~ toString(cSys.xAxis) ~ "\n" ~ "z-Axis" ~ toString(cSys.zAxis);
}
