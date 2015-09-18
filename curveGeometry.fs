FeatureScript 225; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/curvetype.gen.fs", version : "");

// Imports used internally
import(path : "onshape/std/coordSystem.fs", version : "");
import(path : "onshape/std/mathUtils.fs", version : "");
import(path : "onshape/std/units.fs", version : "");

// ===================================== Line ======================================

/**
 * TODO: description
 */
export type Line typecheck canBeLine;

export predicate canBeLine(value)
{
    value is map;
    is3dLengthVector(value.origin);
    is3dDirection(value.direction);
}

/**
 * TODO: description
 * @param origin
 * @param direction
 */
export function line(origin is Vector, direction is Vector) returns Line
{
    return { "origin" : origin, "direction" : normalize(direction) } as Line;
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function lineFromBuiltin(definition is map) returns Line
{
    return line((definition.origin as Vector) * meter, definition.direction as Vector);
}

export operator*(transform is Transform, lineRhs is Line) returns Line
{
    return line(transform * lineRhs.origin, transform.linear * lineRhs.direction);
}

/**
 * TODO: description
 * @param from
 * @param to
 */
export function transform(from is Line, to is Line) returns Transform
{
    const rotation = rotationMatrix3d(from.direction, to.direction);
    return transform(rotation, to.origin - rotation * from.origin);
}

/**
 * TODO: description
 * @param line
 * @param point
 */
export function project(line is Line, point is Vector) returns Vector
precondition
{
    is3dLengthVector(point);
}
{
    return line.origin + line.direction * dot(line.direction, point - line.origin);
}

/**
 * TODO: description
 * @param line
 * @param angle
 */
export function rotationAround(line is Line, angle is ValueWithUnits) returns Transform
precondition
{
    isAngle(angle);
}
{
    const rotation = rotationMatrix3d(line.direction, angle);
    return transform(rotation, line.origin - rotation * line.origin);
}

export function toString(value is Line) returns string
{
    return "direction" ~ toString(value.direction) ~ "\n" ~ "origin" ~ toString(value.origin);
}


// ===================================== Circle ======================================

/**
 * TODO: description
 */
export type Circle typecheck canBeCircle;

export predicate canBeCircle(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    isLength(value.radius);
}

/**
 * TODO: description
 * @param cSys
 * @param radius
 */
export function circle(cSys is CoordSystem, radius is ValueWithUnits) returns Circle
{
    return { "coordSystem" : cSys, "radius" : radius } as Circle;
}

export function circle(center is Vector, xDirection is Vector, normal is Vector, radius is ValueWithUnits) returns Circle
{
    return circle(coordSystem(center, xDirection, normal), radius);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function circleFromBuiltin(definition is map) returns Circle
{
    return circle(coordSystemFromBuiltin(definition.coordSystem), definition.radius * meter);
}

export function toString(value is Circle) returns string
{
    return "radius" ~ toString(value.radius) ~ "\n" ~ "center" ~ toString(value.coordSystem.origin);
}

