FeatureScript 1077; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

export import(path : "onshape/std/rollsurfacetype.gen.fs", version : "1077.0");

import(path : "onshape/std/context.fs", version : "1077.0");
import(path : "onshape/std/curveGeometry.fs", version : "1077.0");
import(path : "onshape/std/error.fs", version : "1077.0");
import(path : "onshape/std/evaluate.fs", version : "1077.0");
import(path : "onshape/std/math.fs", version : "1077.0");
import(path : "onshape/std/query.fs", version : "1077.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1077.0");
import(path : "onshape/std/units.fs", version : "1077.0");
import(path : "onshape/std/vector.fs", version : "1077.0");

/**
 * Represents the `source` or `destination` surface for [opRoll].
 * @type {{
 *      @field rollSurfaceType {RollSurfaceType} : The type of surface that this `RollSurface` describes.
 *      @field cylinder {Cylinder} : @requiredIf{`rollSurfaceType` is `CYLINDER`}
 *      @field plane {Plane}       : @requiredIf{`rollSurfaceType` is `PLANE`}
 *      @field anchorPoint {Vector}     : The anchor point of the `RollSurface`, used to align the `source` and
 *                                        `destination` `RollSurface`s for [opRoll].  Must lie on the `RollSurface`.
 *                                        If this condition is not met, [opRoll] will fail.
 *      @field anchorDirection {Vector} : The anchor direction of the `RollSurface`, used to align the `source` and
 *                                        `destination` `RollSurface`s for [opRoll]. Must lie on the tangent plane
 *                                        of the `RollSurface` at the `anchorPoint`. If this condition is not met,
 *                                        [opRoll] will fail.
 * }}
 */
export type RollSurface typecheck canBeRollSurface;

/** @internal */
export predicate canBeRollSurface(val)
{
    val is map;
    val.rollSurfaceType is RollSurfaceType;
    if (val.rollSurfaceType == RollSurfaceType.CYLINDER)
    {
        val.cylinder is Cylinder;
    }
    else // val.rollSurfaceType == RollSurfaceType.PLANE
    {
        val.plane is Plane;
    }
    is3dLengthVector(val.anchorPoint);
    is3dDirection(val.anchorDirection);
}

//////////////////// PLANE ////////////////////

/**
 * Make a [RollSurface] for [opRoll] from a [Plane].
 * @param plane {Plane} : The definition plane for the [RollSurface].  The plane `origin` will be used as the `anchorPoint`
 *                        and the plane `x` direction will be used as the `anchorDirection`. See [RollSurface] documentation
 *                        for descriptions of `anchorPoint` and `anchorDirection`.
 */
export function makeRollPlane(plane is Plane) returns RollSurface
{
    return makeRollPlane(plane, plane.origin, plane.x);
}

/**
 * Make a [RollSurface] for [opRoll] from a [Plane].
 * @param plane {Plane} : The definition plane for the [RollSurface].
 * @param anchorPoint {Vector} : The anchor point of the [RollSurface]. See [RollSurface] documentation.
 * @param spin {ValueWithUnits} : Angle of a counter-clockwise spin to apply to the `plane`'s `x` axis about its positive `z` axis,
 *                                which is then used as the `anchorDirection`. See [RollSurface] documentation
 *                                for a description of `anchorDirection`.
 */
export function makeRollPlane(plane is Plane, anchorPoint is Vector, spin is ValueWithUnits) returns RollSurface
precondition
{
    isAngle(spin);
}
{
    const anchorDirection = (cos(spin) * plane.x) + (sin(spin) * yAxis(plane));
    return makeRollPlane(plane, anchorPoint, anchorDirection);
}

/**
 * Make a [RollSurface] for [opRoll] from a [Plane].
 * @param plane {Plane} : The definition plane for the [RollSurface].
 * @param anchorPoint {Vector} : The anchor point of the [RollSurface]. See [RollSurface] documentation.
 * @param anchorDirection {Vector} : The anchor direction of the [RollSurface]. See [RollSurface] documentation.
 */
export function makeRollPlane(plane is Plane, anchorPoint is Vector, anchorDirection is Vector) returns RollSurface
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
        "rollSurfaceType" : RollSurfaceType.PLANE,
        "plane" : plane,
        "anchorPoint" : anchorPoint,
        "anchorDirection" : anchorDirection
    } as RollSurface;
}

/**
 * Make a [RollSurface] for [opRoll] from a planar face.
 * @param planarFace {Query} : A planar face to use as the definition plane for the [RollSurface].
 * @param anchorPoint {Vector} : The anchor point of the [RollSurface]. See [RollSurface] documentation.
 * @param anchorDirection {Vector} : The anchor direction of the [RollSurface]. See [RollSurface] documentation.
 */
export function makeRollPlane(context is Context, planarFace is Query, anchorPoint is Vector, anchorDirection is Vector) returns RollSurface
{
    return makeRollPlane(evPlane(context, { "face" : planarFace }), anchorPoint, anchorDirection);
}

/**
 * Flip the normal direction of the [Plane] described by this [RollSurface].  Do not change the `anchorPoint` or `anchorDirection`.
 */
export function flipRollPlane(rollPlane is RollSurface) returns RollSurface
precondition
{
    rollPlane.rollSurfaceType == RollSurfaceType.PLANE;
}
{
    rollPlane.plane.normal *= -1;
    return rollPlane;
}

//////////////////// CYLINDER ////////////////////

// Get the cylinder definition for `makeRollCylinder` operations.
function getCylinder(context is Context, cylindricalFace is Query) returns Cylinder
{
    const surfaceDef = evSurfaceDefinition(context, { "face" : cylindricalFace });
    if (!(surfaceDef is Cylinder))
    {
        throw regenError(ErrorStringEnum.INVALID_ROLL_SURFACE);
    }
    return surfaceDef;
}

// Check anchor inputs for `makeRollCylinder` operations.
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
 * Make a [RollSurface] for [opRoll] from a [Cylinder].
 * @param cylinder {Cylinder} : The definition cylinder for the [RollSurface]. The point at which the cylinder's
 *                              `coordSystem`'s positive `xAxis` intersects the cylinder will be used as the `anchorPoint`.
 *                              The cylinder's `yAxis` will be used as the `anchorDirection`. See [RollSurface] documentation
 *                              for descriptions of `anchorPoint` and `anchorDirection`.
 */
export function makeRollCylinder(cylinder is Cylinder) returns RollSurface
{
    return makeRollCylinder(cylinder, cylinder.coordSystem.origin + (cylinder.coordSystem.xAxis * cylinder.radius), yAxis(cylinder.coordSystem));
}

/**
 * Make a [RollSurface] for [opRoll] from a [Cylinder].
 * @param cylinder {Cylinder} : The definition cylinder for the [RollSurface].
 * @param anchorPoint {Vector} : The anchor point of the [RollSurface]. See [RollSurface] documentation.
 * @param spin {ValueWithUnits} : Angle of a counter-clockwise spin to apply to the `cylinder`'s canonical `anchorDirection` at
 *                                the given `anchorPoint` about the direction vector from the cylinder axis to the `anchorPoint`.
 *                                The canonical `anchorDirection` at the given `anchorPoint` is defined as the "rightward"
 *                                direction when looking at the cylinder in the direction from the `anchorPoint` to the
 *                                cylinder axis with the cylinder's `zAxis` up.  In other words:
 *                                `cross(cylinder z axis, direction vector from cylinder axis to anchorPoint)`.
 *                                This spun direction is then used as the `anchorDirection`.  See [RollSurface] documentation
 *                                for a description of `anchorDirection`.
 */
export function makeRollCylinder(cylinder is Cylinder, anchorPoint is Vector, spin is ValueWithUnits) returns RollSurface
precondition
{
    isAngle(spin);
}
{
    const anchorOnAxis = project(line(cylinder.coordSystem.origin, cylinder.coordSystem.zAxis), anchorPoint);
    const axisToAnchor = normalize(anchorPoint - anchorOnAxis);
    const canonicalAnchorDirection = cross(cylinder.coordSystem.zAxis, axisToAnchor);

    const anchorDirection = (cos(spin) * canonicalAnchorDirection) + (sin(spin) * cylinder.coordSystem.zAxis);
    return makeRollCylinder(cylinder, anchorPoint, anchorDirection);
}

/**
 * Make a [RollSurface] for [opRoll] from a [Cylinder].
 * @param cylinder {Cylinder} : The definition cylinder for the [RollSurface].
 * @param anchorPoint {Vector} : The anchor point of the [RollSurface]. See [RollSurface] documentation.
 * @param anchorDirection {Vector} : The anchor direction of the [RollSurface]. See [RollSurface] documentation.
 */
export function makeRollCylinder(cylinder is Cylinder, anchorPoint is Vector, anchorDirection is Vector) returns RollSurface
precondition
{
    anchorsOnCylinder(cylinder, anchorPoint, anchorDirection);
}
{
    return {
        "rollSurfaceType" : RollSurfaceType.CYLINDER,
        "cylinder" : cylinder,
        "anchorPoint" : anchorPoint,
        "anchorDirection" : anchorDirection
    } as RollSurface;
}

/**
 * Make a [RollSurface] for [opRoll] from a cylindrical face.
 * @param cylindricalFace {Query} : A cylindrical face to use as the definition cylinder for the [RollSurface].
 * @param anchorPoint {Vector} : The anchor point of the [RollSurface]. See [RollSurface] documentation.
 * @param anchorDirection {Vector} : The anchor direction of the [RollSurface]. See [RollSurface] documentation.
 */
export function makeRollCylinder(context is Context, cylindricalFace is Query, anchorPoint is Vector, anchorDirection is Vector) returns RollSurface
{
    return makeRollCylinder(getCylinder(context, cylindricalFace), anchorPoint, anchorDirection);
}

