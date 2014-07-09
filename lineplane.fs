export import(path:"onshape/std/transform.fs", version : "");

// ===================================== Line ======================================

export type Line typecheck canBeLine;

export predicate canBeLine(value)
{
    value is map;
    is3dLengthVector(value.origin);
    is3dDirection(value.direction);
}

export function line(origin is Vector, direction is Vector) returns Line
{
    return {"origin" : origin, "direction" : normalize(direction)} as Line;
}

export function lineFromBuiltin(line is map) returns Line
{
    return line((line.origin as Vector) * meter, line.direction as Vector);
}

export operator*(transform is Transform, line is Line) returns Line
{
    return line(transform * line.origin, transform.linear * line.direction);
}

export function transform(from is Line, to is Line) returns Transform
{
    var rotation = rotationMatrix3d(from.direction, to.direction);
    return transform(rotation, to.origin - rotation * from.origin);
}

export function project(line is Line, point is Vector) returns Vector
precondition
{
    is3dLengthVector(point);
}
{
    return line.origin + line.direction * dotProduct(line.direction, point - line.origin);
}

export function rotationAround(line is Line, angle is ValueWithUnits) returns Transform
precondition
{
    isAngle(angle);
}
{
    var rotation = rotationMatrix3d(line.direction, angle.value);
    return transform(rotation, line.origin - rotation * line.origin);
}

export function toString(value is Line) returns string
{
    var str = "direction" ~ toString(value.direction) ~ "\n" ~ "origin" ~ toString(value.origin);
    return str;
}

//===================================== Plane ======================================

export const XY_PLANE = plane(vector(0, 0, 0) * meter, vector(0, 0, 1));

export type Plane typecheck canBePlane;

export predicate canBePlane(value)
{
    value is map;
    is3dLengthVector(value.origin);
    is3dDirection(value.normal);
    is3dDirection(value.x);
    abs(dotProduct(value.normal, value.x)) < TOLERANCE.zeroAngle;
}

export function plane(origin is Vector, normal is Vector, x is Vector) returns Plane
{
    return {"origin" : origin, "normal" : normalize(normal), "x" : normalize(x)} as Plane;
}

export function plane(origin is Vector, normal is Vector) returns Plane //Arbitrary x
{
    return plane(origin, normal, perpendicularVector(normal));
}

export function planeFromBuiltin(plane is map) returns Plane
{
    return plane((plane.origin as Vector) * meter, plane.normal as Vector, plane.x as Vector);
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

export operator*(transform is Transform, plane is Plane) returns Plane
{
    return plane(transform * plane.origin,
                 inverse(transpose(transform.linear)) * plane.normal, //The normal is a co-vector
                 transform.linear * plane.x);
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
    var rotation = transpose([plane.x, crossProduct(plane.normal, plane.x), plane.normal] as Matrix);
    return transform(rotation, plane.origin);
}

export function worldToPlane(plane is Plane, worldPoint is Vector) returns Vector
precondition
{
    is3dLengthVector(worldPoint);
}
{
    worldPoint -= plane.origin;
    return vector(dotProduct(worldPoint, plane.x), dotProduct(worldPoint, crossProduct(plane.normal, plane.x)));
}

export function worldToPlane(plane is Plane) returns Transform
{
    var rotation = [plane.x, crossProduct(plane.normal, plane.x), plane.normal] as Matrix;
    return transform(rotation, rotation * -plane.origin);
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

