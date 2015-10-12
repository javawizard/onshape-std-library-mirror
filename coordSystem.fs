FeatureScript 236; /* Automatically generated version */
import(path : "onshape/std/mathUtils.fs", version : "");
import(path : "onshape/std/units.fs", version : "");

/**
 * TODO: description
 */
export type CoordSystem typecheck canBeCoordSystem;

export predicate canBeCoordSystem(value)
{
    value is map;
    is3dLengthVector(value.origin);
    is3dDirection(value.xAxis);
    is3dDirection(value.zAxis);
    abs(dot(value.xAxis, value.zAxis)) < TOLERANCE.zeroAngle;
}

/**
 * TODO: description
 * @param origin
 * @param xAxis
 * @param zAxis
 */
export function coordSystem(origin is Vector, xAxis is Vector, zAxis is Vector) returns CoordSystem
{
    return { "origin" : origin, "xAxis" : normalize(xAxis), "zAxis" : normalize(zAxis) } as CoordSystem;
}

/**
 * Create a CoordSystem from the result of a builtin call.
 * For Onshape internal use.
 */
export function coordSystemFromBuiltin(cSys is map) returns CoordSystem
{
    return coordSystem((cSys.origin as Vector) * meter, cSys.xAxis as Vector, cSys.zAxis as Vector);
}

/**
 * TODO: description
 * @param cSys
 */
export function toWorld(cSys is CoordSystem) returns Transform
{
    const rotation = transpose([cSys.xAxis, cross(cSys.zAxis, cSys.xAxis), cSys.zAxis] as Matrix);
    return transform(rotation, cSys.origin);
}

/**
 * TODO: description
 * @param cSys
 * @param worldPoint
 */
export function fromWorld(cSys is CoordSystem, worldPoint is Vector) returns Vector
precondition
{
    is3dLengthVector(worldPoint);
}
{
    worldPoint -= cSys.origin;
    return vector(dot(worldPoint, cSys.xAxis), dot(worldPoint, cross(cSys.zAxis, cSys.xAxis)));
}

export function fromWorld(cSys is CoordSystem) returns Transform
{
    const rotation = [cSys.xAxis, cross(cSys.zAxis, cSys.xAxis), cSys.zAxis] as Matrix;
    return transform(rotation, rotation * -cSys.origin);
}

export operator*(transform is Transform, cSys is CoordSystem) returns CoordSystem
{
    return coordSystem(transform * cSys.origin,
                       transform.linear * (cSys.xAxis as Vector),
                       inverse(transpose(transform.linear)) * cSys.zAxis);
}

/**
 * TODO: description
 * @param cSys
 */
export function toString(cSys is CoordSystem) returns string
{
    return "origin" ~ toString(cSys.origin) ~ "\n" ~ "x-Axis" ~ toString(cSys.xAxis) ~ "\n" ~ "z-Axis" ~ toString(cSys.zAxis);
}

