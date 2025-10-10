FeatureScript 2780; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/containers.fs", version : "2780.0");
import(path : "onshape/std/context.fs", version : "2780.0");
import(path : "onshape/std/curveGeometry.fs", version : "2780.0");
import(path : "onshape/std/error.fs", version : "2780.0");
import(path : "onshape/std/evaluate.fs", version : "2780.0");
import(path : "onshape/std/feature.fs", version : "2780.0");
import(path : "onshape/std/math.fs", version : "2780.0");
import(path : "onshape/std/query.fs", version : "2780.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2780.0");
import(path : "onshape/std/units.fs", version : "2780.0");
import(path : "onshape/std/vector.fs", version : "2780.0");

/**
 * Represents the `source` or `destination` surface for [opWrap].
 * Exactly one of `face`, `plane`, `cylinder`, or `cone` must be defined.
 *
 * (Formerly `RollSurface`)
 *
 * @type {{
 *      @field face {Query}        : The face entity defining this `WrapSurface`.
 *      @field plane {Plane}       : The plane geometry defining this `WrapSurface`.
 *      @field cylinder {Cylinder} : The cylinder geometry defining this `WrapSurface`.
 *      @field cone {Cone}         : The cone geometry defining this `WrapSurface`.
 *
 *      @field anchorPoint {Vector}     : The anchor point of the `WrapSurface`, used to align the `source` and
 *                                        `destination` `WrapSurface`s for [opWrap].  Must lie on the `WrapSurface`.
 *                                        If this condition is not met, [opWrap] will fail.
 *      @field anchorDirection {Vector} : The anchor direction of the `WrapSurface`, used to align the `source` and
 *                                        `destination` `WrapSurface`s for [opWrap]. Must lie on the tangent plane
 *                                        of the `WrapSurface` at the `anchorPoint`. If this condition is not met,
 *                                        [opWrap] will fail.
 * }}
 */
export type WrapSurface typecheck canBeWrapSurface;

/** @internal */
export predicate canBeWrapSurface(val)
{
    val is map;
    hasOneDefinedValue(val, ["face", "plane", "cylinder", "cone"]);
    is3dLengthVector(val.anchorPoint);
    is3dDirection(val.anchorDirection);
}

/** @internal */
function hasOneDefinedValue(val is map, keys is array)
{
    var foundOneValue = false;
    for (var key in keys)
    {
        if (val[key] != undefined)
        {
            if (!foundOneValue)
            {
                foundOneValue = true;
            }
            else
            {
                return false;
            }
        }
    }
    return foundOneValue;
}

/**
 * Returns whether the given [WrapSurface] is a plane.
 * @param context {Context}
 * @param val {WrapSurface} : The `WrapSurface` to check.
 */
export predicate isWrapPlane(context is Context, val is WrapSurface)
{
    val.plane is Plane || size(evaluateQuery(context, qGeometry(val.face != undefined ? val.face : qNothing(), GeometryType.PLANE))) == 1;
}

/**
 * Returns whether the given [WrapSurface] is a cylinder.
 * @param context {Context}
 * @param val {WrapSurface} : The `WrapSurface` to check.
 */
export predicate isWrapCylinder(context is Context, val is WrapSurface)
{
    val.cylinder is Cylinder || size(evaluateQuery(context, qGeometry(val.face != undefined ? val.face : qNothing(), GeometryType.CYLINDER))) == 1;
}

/**
 * Returns whether the given [WrapSurface] is a cone.
 * @param context {Context}
 * @param val {WrapSurface} : The `WrapSurface` to check.
 */
export predicate isWrapCone(context is Context, val is WrapSurface)
{
    val.cone is Cone || size(evaluateQuery(context, qGeometry(val.face != undefined ? val.face : qNothing(), GeometryType.CONE))) == 1;
}

//////////////////// FACE ////////////////////

/**
 * Make a [WrapSurface] for [opWrap] defined by a planar or cylindrical face.
 * @param face {Query} : A face to use as the definition face for the `WrapSurface`.
 *                       For a planar face: The plane `origin` will be used as the `anchorPoint` and the plane `x` direction
 *                       will be used as the `anchorDirection`.
 *                       For a cylindrical face: The point at which the cylinder's `coordSystem`'s positive `xAxis`
 *                       intersects the cylinder will be used as the `anchorPoint`. The cylinder's `yAxis` will be used
 *                       as the `anchorDirection`.
 *                       See [WrapSurface] documentation for descriptions of `anchorPoint` and `anchorDirection`.
 */
export function makeWrapSurface(context is Context, face is Query) returns WrapSurface
precondition
{
    size(evaluateQuery(context, qEntityFilter(face, EntityType.FACE))) == 1;
}
{
    var anchorPoint;
    var anchorDirection;
    const surfaceDef = try silent(evSurfaceDefinition(context, { "face" : face }));
    if (surfaceDef is Plane)
    {
        anchorPoint = surfaceDef.origin;
        anchorDirection = surfaceDef.x;
    }
    else if (surfaceDef is Cylinder)
    {
        const cSys = surfaceDef.coordSystem;
        anchorPoint = cSys.origin + (cSys.xAxis * surfaceDef.radius);
        anchorDirection = yAxis(cSys);
    }
    else
    {
        throw regenError(ErrorStringEnum.INVALID_ROLL_SURFACE);
    }
    return makeWrapSurface(context, face, anchorPoint, anchorDirection);
}

/**
 * Make a [WrapSurface] for [opWrap] defined by a planar, cylindrical or conical face.
 * @param face {Query} : A face to use as the definition face for the `WrapSurface`.
 * @param anchorPoint {Vector} : The anchor point of the `WrapSurface`. See `WrapSurface` documentation.
 * @param spin {ValueWithUnits} : For a planar face: Angle of a counter-clockwise spin to apply to the `plane`'s `x` axis
 *                                about its positive `z` axis, which is then used as the `anchorDirection`.
 *                                For a cylindrical face: Angle of a counter-clockwise spin to apply to the `cylinder`'s
 *                                canonical `anchorDirection` at the given `anchorPoint` about the direction vector from
 *                                the cylinder axis to the `anchorPoint`. The canonical `anchorDirection` at the given
 *                                `anchorPoint` is defined as the "rightward" direction when looking at the cylinder
 *                                in the direction from the `anchorPoint` to the cylinder axis with the cylinder's `zAxis` up.
 *                                In other words:
 *                                `cross(cylinder z axis, direction vector from cylinder axis to anchorPoint)`.
 *                                This spun direction is then used as the `anchorDirection`.
 *                                For a conical face: Angle of a counter-clockwise spin to apply to the `cone`'s
 *                                canonical `anchorDirection` at the given `anchorPoint` about the normal at `anchorPoint`.
 *                                The canonical `anchorDirection` at the given `anchorPoint` is defined as the "rightward"
 *                                direction when looking at the cone from the normal.
 *
 *                                See [WrapSurface] documentation for a description of `anchorDirection`.
 */
export function makeWrapSurface(context is Context, face is Query, anchorPoint is Vector, spin is ValueWithUnits) returns WrapSurface
precondition
{
    size(evaluateQuery(context, qEntityFilter(face, EntityType.FACE))) == 1;
    isAngle(spin);
}
{
    var anchorDirection;
    const surfaceDef = try silent(evSurfaceDefinition(context, { "face" : face }));
    if (surfaceDef is Plane)
    {
        anchorDirection = (cos(spin) * surfaceDef.x) + (sin(spin) * yAxis(surfaceDef));
    }
    else if (surfaceDef is Cylinder)
    {
        const cSys = surfaceDef.coordSystem;
        const anchorOnAxis = project(line(cSys.origin, cSys.zAxis), anchorPoint);
        const axisToAnchor = normalize(anchorPoint - anchorOnAxis);
        const canonicalAnchorDirection = cross(cSys.zAxis, axisToAnchor);
        anchorDirection = (cos(spin) * canonicalAnchorDirection) + (sin(spin) * cSys.zAxis);
    }
    else if (surfaceDef is Cone)
    {
        const cSys = surfaceDef.coordSystem;
        const normalAtAnchor = normalAtAnchor(surfaceDef, anchorPoint);
        const dirToApex = normalize(anchorPoint - cSys.origin);
        const canonicalAnchorDirection = cross(dirToApex, normalAtAnchor);
        anchorDirection = (cos(spin) * canonicalAnchorDirection) + (sin(spin) * dirToApex);
    }
    else
    {
        throw regenError(ErrorStringEnum.INVALID_ROLL_SURFACE);
    }
    return makeWrapSurface(context, face, anchorPoint, anchorDirection);
}

/**
 * Make a [WrapSurface] for [opWrap] defined by a planar, cylindrical or conical face.
 * @param face {Query} : A face to use as the definition face for the `WrapSurface`.
 * @param anchorPoint {Vector} : The anchor point of the `WrapSurface`. See [WrapSurface] documentation.
 * @param anchorDirection {Vector} : The anchor direction of the `WrapSurface`. See [WrapSurface] documentation.
 */
export function makeWrapSurface(context is Context, face is Query, anchorPoint is Vector, anchorDirection is Vector) returns WrapSurface
precondition
{
    size(evaluateQuery(context, qEntityFilter(face, EntityType.FACE))) == 1;
}
{
    return {
        "face" : face,
        "anchorPoint" : anchorPoint,
        "anchorDirection" : anchorDirection
    } as WrapSurface;
}

//////////////////// PLANE ////////////////////

/**
 * Make a [WrapSurface] for [opWrap] from a [Plane].
 * @param plane {Plane} : The definition plane for the `WrapSurface`.  The plane `origin` will be used as the `anchorPoint`
 *                        and the plane `x` direction will be used as the `anchorDirection`. See [WrapSurface] documentation
 *                        for descriptions of `anchorPoint` and `anchorDirection`.
 */
export function makeWrapPlane(plane is Plane) returns WrapSurface
{
    return makeWrapPlane(plane, plane.origin, plane.x);
}

/**
 * Make a [WrapSurface] for [opWrap] from a [Plane].
 * @param plane {Plane} : The definition plane for the `WrapSurface`.
 * @param anchorPoint {Vector} : The anchor point of the `WrapSurface`. See [WrapSurface] documentation.
 * @param spin {ValueWithUnits} : Angle of a counter-clockwise spin to apply to the `plane`'s `x` axis about its positive `z` axis,
 *                                which is then used as the `anchorDirection`. See [WrapSurface] documentation
 *                                for a description of `anchorDirection`.
 */
export function makeWrapPlane(plane is Plane, anchorPoint is Vector, spin is ValueWithUnits) returns WrapSurface
precondition
{
    isAngle(spin);
}
{
    const anchorDirection = (cos(spin) * plane.x) + (sin(spin) * yAxis(plane));
    return makeWrapPlane(plane, anchorPoint, anchorDirection);
}

/**
 * Make a [WrapSurface] for [opWrap] from a [Plane].
 * @param plane {Plane} : The definition plane for the `WrapSurface`.
 * @param anchorPoint {Vector} : The anchor point of the `WrapSurface`. See [WrapSurface] documentation.
 * @param anchorDirection {Vector} : The anchor direction of the `WrapSurface`. See [WrapSurface] documentation.
 */
export function makeWrapPlane(plane is Plane, anchorPoint is Vector, anchorDirection is Vector) returns WrapSurface
precondition
{
    // Ensure anchorPoint lies within the plane
    is3dLengthVector(anchorPoint);
    abs(dot(anchorPoint - plane.origin, plane.normal)) < (TOLERANCE.zeroLength * meter);

    // Ensure that anchorDirection is parallel with the plane
    is3dDirection(anchorDirection);
    abs(dot(anchorDirection, plane.normal)) < TOLERANCE.zeroLength;
}
{
    return {
        "plane" : plane,
        "anchorPoint" : anchorPoint,
        "anchorDirection" : anchorDirection
    } as WrapSurface;
}

/**
 * Make a [WrapSurface] for [opWrap] from a planar face.
 * @param planarFace {Query} : A planar face to use as the definition plane for the `WrapSurface`.
 * @param anchorPoint {Vector} : The anchor point of the `WrapSurface`. See [WrapSurface] documentation.
 * @param anchorDirection {Vector} : The anchor direction of the `WrapSurface`. See [WrapSurface] documentation.
 */
export function makeWrapPlane(context is Context, planarFace is Query, anchorPoint is Vector, anchorDirection is Vector) returns WrapSurface
{
    return makeWrapPlane(evPlane(context, { "face" : planarFace }), anchorPoint, anchorDirection);
}

/**
 * Flip the normal direction of the [Plane] described by this [WrapSurface].  Do not change the `anchorPoint` or `anchorDirection`.
 */
export function flipWrapPlane(wrapPlane is WrapSurface) returns WrapSurface
precondition
{
    wrapPlane.plane is Plane;
}
{
    wrapPlane.plane.normal *= -1;
    return wrapPlane;
}

//////////////////// CYLINDER ////////////////////

// Get the cylinder definition for `makeWrapCylinder` operations.
function getCylinder(context is Context, cylindricalFace is Query) returns Cylinder
{
    const surfaceDef = evSurfaceDefinition(context, { "face" : cylindricalFace });
    if (!(surfaceDef is Cylinder))
    {
        throw regenError(ErrorStringEnum.INVALID_ROLL_SURFACE);
    }
    return surfaceDef;
}

// Check anchor inputs for `makeWrapCylinder` operations.
function anchorsOnCylinder(cylinder is Cylinder, anchorPoint is Vector, anchorDirection is Vector) returns boolean
precondition
{
    is3dLengthVector(anchorPoint);
    is3dDirection(anchorDirection);
}
{
    // Ensure that the anchorPoint lies on the cylinder
    const axisLine = line(cylinder.coordSystem.origin, cylinder.coordSystem.zAxis);
    const anchorOnAxis = project(axisLine, anchorPoint);
    const axisToAnchor = anchorPoint - anchorOnAxis;
    if (abs(norm(axisToAnchor) - cylinder.radius) > (TOLERANCE.zeroLength * meter))
    {
        // anchor point is not `radius` away from the axis
        return false;
    }

    // Ensure that the anchorDirection is parallel with the tangent plane of the cylinder at anchorPoint
    const tangentPlaneNormal = normalize(axisToAnchor);
    if (abs(dot(anchorDirection, tangentPlaneNormal)) > TOLERANCE.zeroLength)
    {
        // Anchor direction is not perpendicular to tangent plane normal
        return false;
    }

    return true;
}

/**
 * Make a [WrapSurface] for [opWrap] from a [Cylinder].
 * @param cylinder {Cylinder} : The definition cylinder for the `WrapSurface`. The point at which the cylinder's
 *                              `coordSystem`'s positive `xAxis` intersects the cylinder will be used as the `anchorPoint`.
 *                              The cylinder's `yAxis` will be used as the `anchorDirection`. See [WrapSurface] documentation
 *                              for descriptions of `anchorPoint` and `anchorDirection`.
 */
export function makeWrapCylinder(cylinder is Cylinder) returns WrapSurface
{
    return makeWrapCylinder(cylinder, cylinder.coordSystem.origin + (cylinder.coordSystem.xAxis * cylinder.radius), yAxis(cylinder.coordSystem));
}

/**
 * Make a [WrapSurface] for [opWrap] from a [Cylinder].
 * @param cylinder {Cylinder} : The definition cylinder for the `WrapSurface`.
 * @param anchorPoint {Vector} : The anchor point of the `WrapSurface`. See [WrapSurface] documentation.
 * @param spin {ValueWithUnits} : Angle of a counter-clockwise spin to apply to the `cylinder`'s canonical `anchorDirection` at
 *                                the given `anchorPoint` about the direction vector from the cylinder axis to the `anchorPoint`.
 *                                The canonical `anchorDirection` at the given `anchorPoint` is defined as the "rightward"
 *                                direction when looking at the cylinder in the direction from the `anchorPoint` to the
 *                                cylinder axis with the cylinder's `zAxis` up.  In other words:
 *                                `cross(cylinder z axis, direction vector from cylinder axis to anchorPoint)`.
 *                                This spun direction is then used as the `anchorDirection`.  See [WrapSurface] documentation
 *                                for a description of `anchorDirection`.
 */
export function makeWrapCylinder(cylinder is Cylinder, anchorPoint is Vector, spin is ValueWithUnits) returns WrapSurface
precondition
{
    isAngle(spin);
}
{
    const anchorOnAxis = project(line(cylinder.coordSystem.origin, cylinder.coordSystem.zAxis), anchorPoint);
    const axisToAnchor = normalize(anchorPoint - anchorOnAxis);
    const canonicalAnchorDirection = cross(cylinder.coordSystem.zAxis, axisToAnchor);

    const anchorDirection = (cos(spin) * canonicalAnchorDirection) + (sin(spin) * cylinder.coordSystem.zAxis);
    return makeWrapCylinder(cylinder, anchorPoint, anchorDirection);
}

/**
 * Make a [WrapSurface] for [opWrap] from a [Cylinder].
 * @param cylinder {Cylinder} : The definition cylinder for the `WrapSurface`.
 * @param anchorPoint {Vector} : The anchor point of the `WrapSurface`. See [WrapSurface] documentation.
 * @param anchorDirection {Vector} : The anchor direction of the `WrapSurface`. See [WrapSurface] documentation.
 */
export function makeWrapCylinder(cylinder is Cylinder, anchorPoint is Vector, anchorDirection is Vector) returns WrapSurface
precondition
{
    anchorsOnCylinder(cylinder, anchorPoint, anchorDirection);
}
{
    return {
        "cylinder" : cylinder,
        "anchorPoint" : anchorPoint,
        "anchorDirection" : anchorDirection
    } as WrapSurface;
}

/**
 * Make a [WrapSurface] for [opWrap] from a cylindrical face.
 * @param cylindricalFace {Query} : A cylindrical face to use as the definition cylinder for the `WrapSurface`.
 * @param anchorPoint {Vector} : The anchor point of the `WrapSurface`. See [WrapSurface] documentation.
 * @param anchorDirection {Vector} : The anchor direction of the `WrapSurface`. See [WrapSurface] documentation.
 */
export function makeWrapCylinder(context is Context, cylindricalFace is Query, anchorPoint is Vector, anchorDirection is Vector) returns WrapSurface
{
    return makeWrapCylinder(getCylinder(context, cylindricalFace), anchorPoint, anchorDirection);
}

// //////////////////// CONE ////////////////////

// Get the cone definition for `makeWrapCone` operations.
function getCone(context is Context, conicalFace is Query) returns Cone
{
    const surfaceDef = evSurfaceDefinition(context, { "face" : conicalFace });
    if (!(surfaceDef is Cone))
    {
        throw regenError(ErrorStringEnum.INVALID_ROLL_SURFACE);
    }
    return surfaceDef;
}

// Check anchor inputs for `makeWrapCone` operations.
function anchorsOnCone(cone is Cone, anchorPoint is Vector, anchorDirection is Vector) returns boolean
precondition
{
    is3dLengthVector(anchorPoint);
    is3dDirection(anchorDirection);
}
{
    // Ensure that the anchorPoint lies on the cone
    const axisLine = line(cone.coordSystem.origin, cone.coordSystem.zAxis);
    const anchorOnAxis = project(axisLine, anchorPoint);
    const axisToAnchor = anchorPoint - anchorOnAxis;

    const distToApex = norm(anchorOnAxis - cone.coordSystem.origin);
    const radius = distToApex * tan(cone.halfAngle);
    if (abs(norm(axisToAnchor) - radius) > (TOLERANCE.zeroLength * meter))
    {
        // anchor point is not `radius` away from the axis
        return false;
    }

    // Ensure that the anchorDirection is parallel with the tangent plane of the cone at anchorPoint
    const height = tan(cone.halfAngle) * radius;
    const lowerPoint = anchorOnAxis + height * cone.coordSystem.zAxis;
    const tangentPlaneNormal = normalize(anchorPoint - lowerPoint);

    if (abs(dot(anchorDirection, tangentPlaneNormal)) > TOLERANCE.zeroLength)
    {
        // Anchor direction is not perpendicular to tangent plane normal
        return false;
    }

    return true;
}

function normalAtAnchor(cone is Cone, anchorPoint is Vector)
{
    const axisLine = line(cone.coordSystem.origin, cone.coordSystem.zAxis);
    const anchorOnAxis = project(axisLine, anchorPoint);

    const distToApex = norm(anchorOnAxis - cone.coordSystem.origin);
    const radius = distToApex * tan(cone.halfAngle);

    const height = tan(cone.halfAngle) * radius;
    const lowerPoint = anchorOnAxis + height * cone.coordSystem.zAxis;
    return normalize(anchorPoint - lowerPoint);
}

/**
 * Make a [WrapSurface] for [opWrap] from a [Cone].
 * @param cone {Cone} : The definition cone for the `WrapSurface`.
 * @param anchorPoint {Vector} : The anchor point of the `WrapSurface`. See [WrapSurface] documentation.
 * @param spin {ValueWithUnits} : Angle of a counter-clockwise spin to apply to the `cone`'s canonical `anchorDirection` at
 *                                the given `anchorPoint` about the normal at the `anchorPoint`.
 *                                This spun direction is then used as the `anchorDirection`.  See [WrapSurface] documentation
 *                                for a description of `anchorDirection`.
 */
export function makeWrapCone(cone is Cone, anchorPoint is Vector, spin is ValueWithUnits) returns WrapSurface
precondition
{
    isAngle(spin);
}
{
    const cSys = cone.coordSystem;
    const normalAtAnchor = normalAtAnchor(cone, anchorPoint);

    const dirToApex = normalize(anchorPoint - cSys.origin);
    const canonicalAnchorDirection = cross(dirToApex, normalAtAnchor);
    const anchorDirection = (cos(spin) * canonicalAnchorDirection) + (sin(spin) * dirToApex);
    return makeWrapCone(cone, anchorPoint, anchorDirection);
}

/**
 * Make a [WrapSurface] for [opWrap] from a [Cone].
 * @param cone {Cone} : The definition cone for the `WrapSurface`.
 * @param anchorPoint {Vector} : The anchor point of the `WrapSurface`. See [WrapSurface] documentation.
 * @param anchorDirection {Vector} : The anchor direction of the `WrapSurface`. See [WrapSurface] documentation.
 */
export function makeWrapCone(cone is Cone, anchorPoint is Vector, anchorDirection is Vector) returns WrapSurface
precondition
{
    anchorsOnCone(cone, anchorPoint, anchorDirection);
}
{
    return {
        "cone" : cone,
        "anchorPoint" : anchorPoint,
        "anchorDirection" : anchorDirection
    } as WrapSurface;
}

/**
 * Make a [WrapSurface] for [opWrap] from a conical face.
 * @param conicalFace {Query} : A conical face to use as the definition cone for the `WrapSurface`.
 * @param anchorPoint {Vector} : The anchor point of the `WrapSurface`. See [WrapSurface] documentation.
 * @param anchorDirection {Vector} : The anchor direction of the `WrapSurface`. See [WrapSurface] documentation.
 */
export function makeWrapCone(context is Context, conicalFace is Query, anchorPoint is Vector, anchorDirection is Vector) returns WrapSurface
{
    return makeWrapCone(getCone(context, conicalFace), anchorPoint, anchorDirection);
}


