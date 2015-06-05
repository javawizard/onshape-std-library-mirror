export import(path:"onshape/std/coordSystem.fs", version : "");
export import(path:"onshape/std/curveGeometry.fs", version : "");
export import(path:"onshape/std/surfacetype.gen.fs", version : "");
export import(path:"onshape/std/print.fs", version : "");

//===================================== Plane ======================================

export const XY_PLANE = plane(vector(0, 0, 0) * meter, vector(0, 0, 1));

export type Plane typecheck canBePlane;

export predicate canBePlane(value)
{
    value is map;
    is3dLengthVector(value.origin);
    is3dDirection(value.x);
    is3dDirection(value.normal);
    abs(dotProduct(value.x, value.normal)) < TOLERANCE.zeroAngle;
}

export function plane(cSys is CoordSystem) returns Plane
{
    return plane(cSys.origin, cSys.zAxis, cSys.xAxis);
}

export function plane(origin is Vector, normal is Vector, x is Vector) returns Plane
{
    return {"origin" : origin, "normal" : normalize(normal), "x" : normalize(x)} as Plane;
}

export function plane(origin is Vector, normal is Vector) returns Plane //Arbitrary x
{
    return plane(origin, normal, perpendicularVector(normal));
}

export function planeFromBuiltin(definition is map) returns Plane
{
    return plane((definition.origin as Vector) * meter, definition.normal as Vector, definition.x as Vector);
}

export function toString(value is Plane) returns string
{
    var str = "normal" ~ toString(value.normal) ~ "\n" ~ "origin" ~ toString(value.origin) ~ "\n" ~ "x-axis" ~ toString(value.x);
    return str;
}

export function project(plane is Plane, point is Vector) returns Vector
precondition
{
    is3dLengthVector(point);
}
{
    return point + plane.normal * dotProduct(plane.normal, plane.origin - point);
}

export operator*(transform is Transform, planeRhs is Plane) returns Plane
{
    return plane(transform * planeRhs.origin,
                 inverse(transpose(transform.linear)) * planeRhs.normal, //The normal is a co-vector
                 transform.linear * planeRhs.x);
}

export function planeToWorld(plane is Plane, planePoint is Vector) returns Vector
precondition
{
    isLengthVector(planePoint);
    @size(planePoint) == 2;
}
{
    return plane.origin + plane.x * planePoint[0] + crossProduct(plane.normal, plane.x) * planePoint[1];
}

export function planeToWorld(plane is Plane) returns Transform
{
    return toWorld(coordSystem(plane.origin, plane.x, plane.normal));
}

export function worldToPlane(plane is Plane, worldPoint is Vector) returns Vector
{
    return fromWorld(coordSystem(plane.origin, plane.x, plane.normal), worldPoint);
}

/* Return the projection of a point onto a sketch, in sketch coordinates.
   The origin of a sketch is the projection of the world origin onto the
   sketch plane. */
export function worldToSketch(plane is Plane, worldPoint is Vector) returns Vector
{
    return vector(dotProduct(worldPoint, plane.x),
                  dotProduct(worldPoint, crossProduct(plane.normal, plane.x)));
}

export function worldToPlane(plane is Plane) returns Transform
{
    return fromWorld(coordSystem(plane.origin, plane.x, plane.normal));
}

export function transform(from is Plane, to is Plane) returns Transform
{
    return planeToWorld(to) * worldToPlane(from);
}

export function mirrorAcross(plane is Plane) returns Transform
{
    var normalMatrix = [plane.normal] as Matrix;
    var linear = identityMatrix(3) - 2 * transpose(normalMatrix) * normalMatrix;
    return transform(linear, plane.origin - linear * plane.origin);
}

export function intersection(p1 is Plane, p2 is Plane) // Returns Line or undefined if there's no good intersection
{
    var direction = crossProduct(p1.normal, p2.normal);
    if(norm(direction) < TOLERANCE.zeroAngle)
        return;
    direction = normalize(direction);
    var rhs = vector(dotProduct(p1.normal, p1.origin), dotProduct(p2.normal, p2.origin),
                     dotProduct(direction, 0.5 * (p1.origin + p2.origin)));
    var point = inverse([p1.normal, p2.normal, direction] as Matrix) * rhs;
    return line(point, direction);
}

export type LinePlaneIntersection typecheck canBeLinePlaneIntersection;
predicate canBeLinePlaneIntersection(value)
{
    value is map;
    (value.dim == 0 || value.dim == 1 || value.dim == -1);
    if (value.dim == 0)
        value.intersection is Vector;
    if (value.dim == 1)
        value.intersection is Line;
}
export function intersection(p is Plane, l is Line) // Returns LinePlaneIntersection or undefined
{
    var dotPr = dotProduct(p.normal, l.direction);
    if (abs(dotPr) < TOLERANCE.zeroAngle)
    {
        if (samePoint(l.origin, project(p, l.origin)))
            return {'dim' : 1, 'intersection' : l} as LinePlaneIntersection;
        else
            return {'dim' : -1} as LinePlaneIntersection; //line is parallel to plane
    }
    var t = dotProduct(p.origin - l.origin, p.normal)/dotPr;
    return {'dim' : 0, 'intersection' : l.origin + t * l.direction} as LinePlaneIntersection;
}

// ===================================== Cone ======================================

export type Cone typecheck canBeCone;

export predicate canBeCone(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    isAngle(value.halfAngle);
}

export function cone(cSys is CoordSystem, halfAngle is ValueWithUnits) returns Cone
{
    return {"coordSystem" : cSys, "halfAngle" : halfAngle} as Cone;
}

export function coneFromBuiltin(definition is map) returns Cone
{
    return cone(coordSystemFromBuiltin(definition.coordSystem), definition.halfAngle * radian);
}

export function toString(value is Cone) returns string
{
    var str = "half angle" ~ toString(value.halfAngle) ~ "\n" ~ "basis" ~ toString(value.coordSystem);
    return str;
}

// ===================================== Cylinder ======================================

export type Cylinder typecheck canBeCylinder;

export predicate canBeCylinder(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    isLength(value.radius);
}

export function cylinder(cSys is CoordSystem, radius is ValueWithUnits) returns Cylinder
{
    return {"coordSystem" : cSys, "radius" : radius} as Cylinder;
}

export function cylinderFromBuiltin(definition is map) returns Cylinder
{
    return cylinder(coordSystemFromBuiltin(definition.coordSystem), definition.radius * meter);
}

export function toString(value is Cylinder) returns string
{
    var str = "radius" ~ toString(value.radius) ~ "\n" ~ "basis" ~ toString(value.coordSystem);
    return str;
}

// ===================================== Torus ======================================

export type Torus typecheck canBeTorus;

export predicate canBeTorus(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    isLength(value.radius);
    isLength(value.minorRadius);
}

export function torus(cSys is CoordSystem, minorRadius is ValueWithUnits, radius is ValueWithUnits) returns Torus
{
    return {"coordSystem" : cSys, "minorRadius" : minorRadius, "radius" : radius} as Torus;
}

export function torusFromBuiltin(definition is map) returns Torus
{
    return torus(coordSystemFromBuiltin(definition.coordSystem), definition.minorRadius * meter, definition.radius * meter);
}

export function toString(value is Torus) returns string
{
    var str = "radius" ~ toString(value.radius) ~ "\n" ~ "minor radius" ~ toString(value.minorRadius) ~ "\n" ~ "basis" ~ toString(value.coordSystem);
    return str;
}

// ===================================== Sphere ======================================

export type Sphere typecheck canBeSphere;

export predicate canBeSphere(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    isLength(value.radius);
}

export function sphere(cSys is CoordSystem, radius is ValueWithUnits) returns Sphere
{
    return {"coordSystem" : cSys, "radius" : radius} as Sphere;
}

export function sphereFromBuiltin(definition is map) returns Sphere
{
    return sphere(coordSystemFromBuiltin(definition.coordSystem), definition.radius * meter);
}

export function toString(value is Sphere) returns string
{
    var str = "radius" ~ toString(value.radius) ~ "\n" ~ "basis" ~ toString(value.coordSystem);
    return str;
}

