FeatureScript ✨; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/curvetype.gen.fs", version : "");

// Imports used internally
import(path : "onshape/std/coordSystem.fs", version : "");
import(path : "onshape/std/mathUtils.fs", version : "");
import(path : "onshape/std/units.fs", version : "");

// ===================================== Line ======================================

/**
 * Represents a parameterized line in space.
 * @value {{
 *      @field origin {Vector} : A 3D Vector with length units.
 *      @field direction {Vector} : A unitless normalized 3D Vector.
 * }}
 */
export type Line typecheck canBeLine;

export predicate canBeLine(value)
{
    value is map;
    is3dLengthVector(value.origin);
    is3dDirection(value.direction);
}

/**
 * Creates a line from an origin and a direction.
 * @param origin
 * @param direction : The direction gets normalized by this function.
 */
export function line(origin is Vector, direction is Vector) returns Line
{
    return { "origin" : origin, "direction" : normalize(direction) } as Line;
}

/**
 * Create a `Line` from the result of a builtin call.
 * For Onshape internal use.
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
 * Returns the transformation that transforms the line `from` to the line `to` (including the origin) using the minimum
 * rotation angle.
 */
export function transform(from is Line, to is Line) returns Transform
{
    const rotation = rotationMatrix3d(from.direction, to.direction);
    return transform(rotation, to.origin - rotation * from.origin);
}

/**
 * Returns the projection of the point onto the line. See also other overloads of `project`.
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
 * Returns a `Transform` that represents the rotation around the given line by the given angle. The rotation is
 * counterclockwise looking against the line direction.
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
 * Represents a circle in 3D space.
 * @value {{
 *      @field coordSystem {CoordSystem} : The circle lies in the xy plane of this coordinate
 *          system and the origin of its parameterization is the x axis.
 *      @field radius {ValueWithUnits} : The radius of the circle.
 * }}
 */
export type Circle typecheck canBeCircle;

export predicate canBeCircle(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    isLength(value.radius);
}

/**
 * Returns a circle given a `CoordSystem` and a radius.
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
 * Create a `Circle` from the result of a builtin call.
 * For Onshape internal use.
 */
export function circleFromBuiltin(definition is map) returns Circle
{
    return circle(coordSystemFromBuiltin(definition.coordSystem), definition.radius * meter);
}

export function toString(value is Circle) returns string
{
    return "radius" ~ toString(value.radius) ~ "\n" ~ "center" ~ toString(value.coordSystem.origin);
}


// ===================================== Ellipse ======================================

/**
 * Represents an ellipse in 3D space.
 * @value {{
 *      @field coordSystem {CoordSystem} : The ellipse lies in the xy plane of this coordinate
 *          system and the x axis corresponds to the major radius.
 *      @field majorRadius {ValueWithUnits} : The larger of the two radii.
 *      @field minorRadius {ValueWithUnits} : The smaller of the two radii.
 * }}
 */
export type Ellipse typecheck canBeEllipse;

export predicate canBeEllipse(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    isLength(value.majorRadius);
    isLength(value.minorRadius);
    value.majorRadius >= value.minorRadius;
}

/**
 * Constructs an ellipse according to the definition.
 */
export function ellipse(cSys is CoordSystem, majorRadius is ValueWithUnits, minorRadius is ValueWithUnits) returns Ellipse
{
    return { "coordSystem" : cSys, "majorRadius" : majorRadius, "minorRadius" : minorRadius } as Ellipse;
}

export function ellipse(center is Vector, xDirection is Vector, normal is Vector, majorRadius is ValueWithUnits, minorRadius is ValueWithUnits) returns Ellipse
{
    return ellipse(coordSystem(center, xDirection, normal), majorRadius, minorRadius);
}

/**
 * Create an `Ellipse` from the result of a builtin call.
 * For Onshape internal use.
 */
export function ellipseFromBuiltin(definition is map) returns Ellipse
{
    return ellipse(coordSystemFromBuiltin(definition.coordSystem), definition.majorRadius * meter, definition.minorRadius * meter);
}

export function toString(value is Ellipse) returns string
{
    return "majorRadius" ~ toString(value.majorRadius) ~ "\n" ~ "minorRadius" ~ toString(value.minorRadius) ~ "\n" ~ "center" ~ toString(value.coordSystem.origin);
}

