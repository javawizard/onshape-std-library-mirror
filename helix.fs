FeatureScript 2737; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

export import(path : "onshape/std/query.fs", version : "2737.0");
export import(path : "onshape/std/manipulator.fs", version : "2737.0");

import(path : "onshape/std/box.fs", version : "2737.0");
import(path : "onshape/std/containers.fs", version : "2737.0");
import(path : "onshape/std/coordSystem.fs", version : "2737.0");
import(path : "onshape/std/curveGeometry.fs", version : "2737.0");
import(path : "onshape/std/debug.fs", version : "2737.0");
import(path : "onshape/std/evaluate.fs", version : "2737.0");
import(path : "onshape/std/feature.fs", version : "2737.0");
import(path : "onshape/std/mathUtils.fs", version : "2737.0");
import(path : "onshape/std/sketch.fs", version : "2737.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2737.0");
import(path : "onshape/std/valueBounds.fs", version : "2737.0");


/**
 * Describes the type of body on which to create a helix.
 * @value SURFACE : Impose a helix onto a cylinder or cone.
 * @value AXIS : Revolve a helix around an axial edge or mate connector.
 * @value CIRCLE : Revolve a helix along a circular path.
 */
export enum AxisType
{
    annotation { "Name" : "Cylinder/Cone" }
    SURFACE,
    annotation { "Name" : "Axis" }
    AXIS,
    annotation { "Name" : "Circle" }
    CIRCLE
}

/**
 * Describes the parameter(s) to vary when computing the path of a helix.
 * @value TURNS : Allow only the number of turns (revolutions) to be varied.
 * @value PITCH : Allow only the space between each turn (helical pitch) to be varied.
 * @value TURNS_PITCH : Allow both turns and pitch to be varied. Always results in a cylindrical helix.
 */
export enum PathType
{
    annotation { "Name" : "Turns" }
    TURNS,
    annotation { "Name" : "Pitch" }
    PITCH,
    annotation { "Name" : "Turns and pitch" }
    TURNS_PITCH
}

/**
 * Describes the starting condition of a helix.
 * @value START_ANGLE : Start the helix at an angle around the origin of the base.
 * @value START_POINT : Start the helix at a point of choice along the plane of the base.
 */
export enum StartType
{
    annotation { "Name" : "Start angle" }
    START_ANGLE,
    annotation { "Name" : "Start point" }
    START_POINT
}

/**
 * Describes the ending condition of a helix.
 * @value HEIGHT : End the helix at a specified height.
 * @value END_POINT : End the helix at a specified point along a perpendicular plane to the base.
 */
export enum EndType
{
    annotation { "Name" : "Height" }
    HEIGHT,
    annotation { "Name" : "End point" }
    END_POINT
}

/**
 * Describes the direction a helix turns while traveling along its axis.
 * @value CW : Clockwise.
 * @value CCW : Counterclockwise.
 */
export enum Direction
{
    annotation { "Name" : "Clockwise" }
    CW,
    annotation { "Name" : "Counterclockwise" }
    CCW
}

/**
 * @internal
 *
 * Describes the "type" of a helix created prior to FeatureScript version 1998 - specifies how the helix will be defined.
 * @value TURNS : User defines the number of turns, and the helical pitch is
 *                  computed based on the input entity's height.
 * @value PITCH : User defines the helical pitch, and the number of turns is
 *                  computed based on the input entity's height.
 * @value HEIGHT_TURNS : User defines both the height and number of turns, and
 *                  the helical pitch is computed based on these terms.
 * @value HEIGHT_PITCH : User defines both the height and helical pitch, and
 *                  the number of turns is computed based on these terms.
 * @value TURNS_PITCH : User defines both the number of turns and helical pitch,
 *                  and the height is computed based on these terms.
 */
enum HelixType
{
    annotation { "Name" : "Turns" }
    TURNS,
    annotation { "Name" : "Pitch" }
    PITCH,
    annotation { "Name" : "Height and turns" }
    HEIGHT_TURNS,
    annotation { "Name" : "Height and pitch" }
    HEIGHT_PITCH,
    annotation { "Name" : "Turns and pitch" }
    TURNS_PITCH
}

type AxisTypeData typecheck canBeAxisTypeData;
predicate canBeAxisTypeData(value)
{
    value is map;
    value.remainingTransform is Transform;
    value.localCoordSys is CoordSystem;
    isLength(value.startRadius, NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS);
}

type StartConditionData typecheck canBeStartConditionData;
predicate canBeStartConditionData(value)
{
    value is map;
    isAngle(value.angleToStart, ANGLE_360_ZERO_DEFAULT_BOUNDS);
    value.startPtLocal is Vector;
    value.startPtWorld is Vector;
}

type EndConditionData typecheck canBeEndConditionData;
predicate canBeEndConditionData(value)
{
    value is map;
    isReal(value.revolutions, HELIX_TURN_BOUNDS);
    isLength(value.endRadius, NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS);
    value.endPtWorld is Vector;
}

const needConeOrCylinderMessage = ErrorStringEnum.HELIX_INPUT_CONE;
const needAxisObjectMessage = ErrorStringEnum.HELIX_INPUT_AXIS;
const needCircleMessage = ErrorStringEnum.HELIX_INPUT_CIRCLE;

const HEIGHT_MANIPULATOR = "heightManipulator";
const DIRECTION_MANIPULATOR = "directionManipulator";
const HELIX_TURN_BOUNDS =
{
    (unitless) : [1e-5, 4, 1000]
} as RealBoundSpec;
const HELIX_TURN_TOLERANCE = HELIX_TURN_BOUNDS[unitless][0];

/**
 * Feature performing an [opHelix].
 */
annotation {
    "Feature Type Name" : "Helix",
    "Manipulator Change Function" : "helixManipulatorChange",
    "Editing Logic Function" : "helixEditingLogic"
}
export const helix = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        // This hidden "placeholder" Query allows preselection of cones/cylinders, circles/arcs, AND any miscellaneous helix axes.
        // We purposely accept all EDGEs to allow for preselection in the feature tree.
        // Further constraints are applied in the helixEditingLogic preselection function
        annotation {
            "Name" : "Initial Entities",
            "UIHint" : UIHint.ALWAYS_HIDDEN,
            "Filter" : QueryFilterCompound.ALLOWS_AXIS
        }
        definition.initEntities is Query;

        annotation {
            "Name" : "Helix axis type",
            "UIHint" : UIHint.HORIZONTAL_ENUM
        }
        definition.axisType is AxisType;

        if (definition.axisType == AxisType.SURFACE)
        {
            annotation {
                "Name" : "Conical or cylindrical face",
                "MaxNumberOfPicks" : 1,
                "Filter" : EntityType.FACE && QueryFilterCompound.ALLOWS_AXIS,
                "UIHint" : UIHint.PREVENT_CREATING_NEW_MATE_CONNECTORS
            }
            definition.entities is Query;
        }
        else if (definition.axisType == AxisType.AXIS)
        {
            annotation {
                "Name" : "Helix axis",
                "MaxNumberOfPicks" : 1,
                "Filter" : QueryFilterCompound.ALLOWS_AXIS
            }
            definition.axis is Query;
        }
        else // StartType.CIRCLE
        {
            annotation {
                "Name" : "Circular edge",
                "MaxNumberOfPicks" : 1,
                "Filter" : GeometryType.CIRCLE || GeometryType.ARC
            }
            definition.edge is Query;
        }

        annotation {
            "Name" : "Input type",
            "UIHint" : UIHint.SHOW_LABEL
        }
        definition.pathType is PathType;

        annotation {
            "Name" : "Opposite direction",
            "UIHint" : UIHint.OPPOSITE_DIRECTION
        }
        definition.oppositeDirection is boolean;

        annotation {
            "Name" : "Start condition",
            "UIHint" : UIHint.SHOW_LABEL
        }
        definition.startType is StartType;

        if (definition.axisType == AxisType.AXIS)
        {
            annotation {
                "Name" : "Opposite origin",
                "UIHint" : UIHint.OPPOSITE_DIRECTION
            }
            definition.oppositeOrigin is boolean;
        }

        if (definition.startType == StartType.START_ANGLE)
        {
            annotation {
                "Name" : "Start angle"
            }
            isAngle(definition.startAngle, ANGLE_360_ZERO_DEFAULT_BOUNDS);

            if (definition.axisType == AxisType.AXIS)
            {
                annotation {
                    "Name" : "Start radius"
                }
                isLength(definition.startRadius, NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS);
            }
        }
        else // StartType.START_POINT
        {
            annotation {
                "Name" : "Start point",
                "MaxNumberOfPicks" : 1,
                "Filter" : EntityType.VERTEX,
                "UIHint" : UIHint.PREVENT_CREATING_NEW_MATE_CONNECTORS
            }
            definition.startPoint is Query;
        }

        if (definition.pathType != PathType.TURNS_PITCH) // no end conditions for TURNS_PITCH. end is calculated
        {
            annotation {
                "Name" : "End condition",
                "UIHint" : UIHint.SHOW_LABEL
            }
            definition.endType is EndType;

            if (definition.axisType != AxisType.SURFACE && definition.endType == EndType.HEIGHT)
            {
                annotation {
                    "Name" : "Height"
                }
                isLength(definition.height, NONNEGATIVE_LENGTH_BOUNDS);

                if (definition.axisType == AxisType.AXIS)
                {
                    annotation {
                        "Name" : "End radius"
                    }
                    definition.endRadToggle is boolean;

                    if (definition.endRadToggle)
                    {
                        annotation {
                            "Name" : "End radius"
                        }
                        isLength(definition.endRadius, NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS);
                    }
                }
            }
            if (definition.endType == EndType.END_POINT)
            {
                annotation {
                    "Name" : "End point",
                    "MaxNumberOfPicks" : 1,
                    "Filter" : EntityType.VERTEX,
                    "UIHint" : UIHint.PREVENT_CREATING_NEW_MATE_CONNECTORS
                }
                definition.endPoint is Query;
            }
        }

        if (definition.pathType == PathType.TURNS || definition.pathType == PathType.TURNS_PITCH)
        {
            annotation {
                "Name" : "Target revolutions"
            }
            isReal(definition.revolutions, HELIX_TURN_BOUNDS);

            if (definition.pathType != PathType.TURNS_PITCH)
            {
                if (definition.endType == EndType.END_POINT)
                {
                    annotation {
                        "Name" : "Computed revolutions",
                        "UIHint" : UIHint.READ_ONLY
                    }
                    isReal(definition.actualRevolutions, HELIX_TURN_BOUNDS);

                    annotation {
                        "Name" : "Flip rounding",
                        "UIHint" : UIHint.OPPOSITE_DIRECTION
                    }
                    definition.roundUpTurns is boolean;
                }
            }
        }

        if (definition.pathType == PathType.PITCH || definition.pathType == PathType.TURNS_PITCH)
        {
            annotation {
                "Name" : "Target pitch"
            }
            isLength(definition.helicalPitch, NONNEGATIVE_LENGTH_BOUNDS);

            if (definition.pathType != PathType.TURNS_PITCH)
            {
                if (definition.endType == EndType.END_POINT)
                {
                    annotation {
                        "Name" : "Computed pitch",
                        "UIHint" : UIHint.READ_ONLY
                    }
                    isLength(definition.actualPitch, NONNEGATIVE_LENGTH_BOUNDS);
                    annotation {
                        "Name" : "Flip rounding",
                        "UIHint" : UIHint.OPPOSITE_DIRECTION
                    }
                    definition.roundUpPitch is boolean;
                }
            }
        }

        annotation {
            "Name" : "Handedness"
        }
        definition.handedness is Direction;

        annotation {
            "Name" : "Show start and end profiles"
        }
        definition.showStartEndProfiles is boolean;
    }
    {
        if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V1998_HELIX_UI_UPGRADE) && definition.heldBack_PRE_V1998 == true)
        {
            definition.helixType = getHelixType(definition.axisType, definition.pathType);
            doHelix_PRE_V1998(context, id, definition);
        }
        else
        {
            doHelix(context, id, definition);
        }
    },
    {
        initEntities : qNothing(),
        startType : StartType.START_ANGLE,
        endType : EndType.HEIGHT,
        oppositeDirection : false,
        oppositeOrigin : false,
        endRadToggle : false,
        roundUpTurns : false,
        roundUpPitch : false,
        showStartEndProfiles : false
    });

function getHelixType(axisType is AxisType, pathType is PathType) returns HelixType
{
    return switch (axisType)
    {
        AxisType.SURFACE : switch (pathType)
        {
            PathType.TURNS : HelixType.TURNS,
            PathType.PITCH : HelixType.PITCH
        },
        AxisType.CIRCLE : switch (pathType)
        {
            PathType.TURNS : HelixType.HEIGHT_TURNS,
            PathType.PITCH : HelixType.HEIGHT_PITCH,
            PathType.TURNS_PITCH : HelixType.TURNS_PITCH
        }
    };
}

function doHelix(context is Context, id is Id, definition is map)
{
    const axisTypeData = switch (definition.axisType)
    {
        AxisType.SURFACE : getSurfaceData(context, id, definition),
        AxisType.AXIS : getAxisData(context, id, definition),
        AxisType.CIRCLE : getCircleData(context, id, definition)
    };

    const alignedLocalCoordSys = getCanonicallyAlignedCoordSys(context, axisTypeData.localCoordSys);

    const startConditionData = getStartConditionData(context, id, definition, alignedLocalCoordSys, axisTypeData.startRadius);
    const endConditionData = getEndConditionData(context, id, definition, alignedLocalCoordSys, axisTypeData, startConditionData);

    const startPtWorldBox = new box(startConditionData.startPtWorld);
    const endPtWorldBox = new box(endConditionData.endPtWorld);

    const startRadiusBox = new box(axisTypeData.startRadius);
    const endRadiusBox = new box(endConditionData.endRadius);

    const directionEnforcedLocalCoordSys = enforceDirectionTowardsConeApex(alignedLocalCoordSys, startRadiusBox, startPtWorldBox, endRadiusBox, endPtWorldBox);
    drawConstructionEnds(context, id, startPtWorldBox[], endPtWorldBox[], directionEnforcedLocalCoordSys, definition.showStartEndProfiles);

    const definitionOut = pointBasedHelixDefinition(definition.handedness, directionEnforcedLocalCoordSys, startPtWorldBox[], endPtWorldBox[], startRadiusBox[], endRadiusBox[], endConditionData.revolutions);
    opHelix(context, id, definitionOut);

    transformResultIfNecessary(context, id, axisTypeData.remainingTransform);
}

function getSurfaceData(context is Context, id is Id, definition is map) returns AxisTypeData
{
    verifyNoMesh(context, definition, "entities");
    verifyNoSheetMetalFlatQuery(context, definition.entities, "entities", ErrorStringEnum.FLATTENED_SHEET_METAL_SKETCH_PROHIBITED);

    const remainingTransform = getRemainderPatternTransform(context, { "references" : definition.entities });

    var surface;
    try silent
    {
        surface = evSurfaceDefinition(context, { "face" : definition.entities });
    }
    catch
    {
        throw regenError(needConeOrCylinderMessage, { "faultyParameters" : ["entities"] });
    }

    verify(surface is Cone || surface is Cylinder, needConeOrCylinderMessage, { "faultyParameters" : ["entities"] });

    var localCoordSys = surface.coordSystem;

    const boxResult = evBox3d(context, { "topology" : definition.entities, "cSys" : localCoordSys });
    const height = boxResult.maxCorner[2] - boxResult.minCorner[2];

    localCoordSys.origin += boxResult.minCorner[2] * localCoordSys.zAxis;

    var startRadius;
    var endRadius;

    if (surface is Cylinder)
    {
        startRadius = surface.radius;
        endRadius = surface.radius;
    }
    else if (surface is Cone)
    {
        // Origin of coordinate system at this point is guaranteed to be on the apex of a cone.
        // The bounding box can be oversized by some tolerance (~1E-10m), to ensure helix won't cross through apex.
        const minZ = max(0 * meter, boxResult.minCorner[2]);
        const maxZ = boxResult.maxCorner[2];

        const slope = tan(surface.halfAngle);
        startRadius = slope * minZ;
        endRadius = slope * maxZ;
    }

    const manipulatorBase = localCoordSys.origin + height / 2 * localCoordSys.zAxis;
    addHelixDirectionManipulator(context, id, definition, manipulatorBase, localCoordSys.zAxis, false);

    if (definition.oppositeDirection)
    {
        const tempRad = startRadius;
        startRadius = endRadius;
        endRadius = tempRad;

        localCoordSys.zAxis = -localCoordSys.zAxis;
        localCoordSys.origin -= height * localCoordSys.zAxis;
    }

    return {
        'remainingTransform' : remainingTransform,
        'localCoordSys' : localCoordSys,
        'startRadius' : startRadius,
        'height' : height,
        'endRadius' : endRadius
    } as AxisTypeData;
}

function getAxisData(context is Context, id is Id, definition is map) returns AxisTypeData
{
    const remainingTransform = getRemainderPatternTransform(context, { "references" : definition.axis });

    var axisObj;
    try silent
    {
        axisObj = evAxis(context, { "axis" : definition.axis });
    }
    catch
    {
        throw regenError(needAxisObjectMessage, { "faultyParameters" : ["axis"] });
    }

    const boxResult = evBox3d(context, { "topology" : definition.axis, "cSys" : coordSystem(axisObj.origin, perpendicularVector(axisObj.direction), axisObj.direction) });
    const axisLength = boxResult.maxCorner[2] - boxResult.minCorner[2];

    axisObj.origin -= flipHeightIfOppositeDirection(axisLength / 2, definition.oppositeOrigin) * axisObj.direction;
    if (definition.endType == EndType.HEIGHT && definition.pathType != PathType.TURNS_PITCH)
    {
        addHelixHeightManipulator(context, id, definition, axisObj.origin, definition.oppositeOrigin ? -axisObj.direction : axisObj.direction);
    }

    axisObj.direction = (definition.oppositeDirection != definition.oppositeOrigin) ? -axisObj.direction : axisObj.direction;
    const localCoordSys = coordSystem(axisObj.origin, perpendicularVector(axisObj.direction), axisObj.direction);

    var startRadius;
    if (definition.startType == StartType.START_ANGLE)
    {
        startRadius = definition.startRadius;
    }
    else // StartType.START_POINT
    {
        const startPtWorld = evVertexPoint(context, { "vertex" : definition.startPoint });
        const startPtLocal = fromWorld(localCoordSys, startPtWorld);
        startRadius = norm(vector(startPtLocal[0], startPtLocal[1], 0 * meter));
    }

    return {
        'remainingTransform' : remainingTransform,
        'localCoordSys' : localCoordSys,
        'startRadius' : startRadius
    } as AxisTypeData;
}

function getCircleData(context is Context, id is Id, definition is map) returns AxisTypeData
{
    verifyNoMesh(context, definition, "edge");
    verifyNoSheetMetalFlatQuery(context, definition.edge, "edge", ErrorStringEnum.FLATTENED_SHEET_METAL_SKETCH_PROHIBITED);

    const remainingTransform = getRemainderPatternTransform(context, { "references" : definition.edge });

    var edge;
    try silent
    {
        edge = evCurveDefinition(context, { "edge" : definition.edge, "returnBSplinesAsOther" : true });
    }
    catch
    {
        throw regenError(needCircleMessage, { "faultyParameters" : ["edge"] });
    }

    verify(edge is Circle, needCircleMessage, { "faultyParameters" : ["edge"] });

    var localCoordSys = edge.coordSystem;
    if (definition.endType == EndType.HEIGHT && definition.pathType != PathType.TURNS_PITCH)
    {
        addHelixHeightManipulator(context, id, definition, localCoordSys.origin, localCoordSys.zAxis);
    }

    if (definition.oppositeDirection)
    {
        localCoordSys.zAxis = -localCoordSys.zAxis;
    }

    return {
        'remainingTransform' : remainingTransform,
        'localCoordSys' : localCoordSys,
        'startRadius' : edge.radius,
        'endRadius' : edge.radius
    } as AxisTypeData;
}

function getCanonicallyAlignedCoordSys(context is Context, localCoordSys is CoordSystem) returns CoordSystem
{
    return coordSystem(alignCanonically(context, plane(localCoordSys)));
}

function getStartConditionData(context is Context, id is Id, definition is map, localCoordSys is CoordSystem, startRadius is ValueWithUnits) returns StartConditionData
{
    var angleToStart;
    var startPtLocal;
    var startPtWorld;

    if (definition.startType == StartType.START_ANGLE)
    {
        angleToStart = adjustAngle(context, definition.startAngle);
        startPtLocal = getPointFromAngle(angleToStart, startRadius);
        startPtLocal = vector(startPtLocal[0], startPtLocal[1], 0 * meter);
        startPtWorld = toWorld(localCoordSys, startPtLocal);
    }
    else // StartType.START_POINT
    {
        const axisPtStart = zeroVector(3) * meter;

        startPtWorld = evVertexPoint(context, { "vertex" : definition.startPoint });
        startPtWorld = clampWorldPointToRadius(startPtWorld, localCoordSys, startRadius, axisPtStart, ErrorStringEnum.HELIX_START_POINT_MISALIGNED, definition.startPoint);
        startPtLocal = fromWorld(localCoordSys, startPtWorld);
        angleToStart = angleBetween(vector(1, 0, 0), startPtLocal, vector(0, 0, 1));
    }

    return {
        'angleToStart' : angleToStart,
        'startPtLocal' : startPtLocal,
        'startPtWorld' : startPtWorld
    } as StartConditionData;
}

function getEndConditionData(context is Context, id is Id, definition is map, localCoordSys is CoordSystem, axisTypeData is AxisTypeData, startConditionData is StartConditionData) returns EndConditionData
{
    return switch (definition.pathType)
    {
        PathType.TURNS : getTurnsOrPitchData(context, id, definition, localCoordSys, axisTypeData, startConditionData),
        PathType.PITCH : getTurnsOrPitchData(context, id, definition, localCoordSys, axisTypeData, startConditionData),
        PathType.TURNS_PITCH : getTurnsAndPitchData(context, id, definition, localCoordSys, axisTypeData, startConditionData)
    };
}

function getTurnsOrPitchData(context is Context, id is Id, definition is map, localCoordSys is CoordSystem, axisTypeData is AxisTypeData, startConditionData is StartConditionData) returns EndConditionData
{
    return switch (definition.endType)
    {
        EndType.HEIGHT : getEndHeightData(context, definition, localCoordSys, axisTypeData, startConditionData),
        EndType.END_POINT : getEndPointData(context, id, definition, localCoordSys, axisTypeData, startConditionData)
    };
}

function getEndHeightData(context is Context, definition is map, localCoordSys is CoordSystem, axisTypeData is AxisTypeData, startConditionData is StartConditionData) returns EndConditionData
{
    const pitch = definition.helicalPitch;
    const height = definition.axisType == AxisType.SURFACE ? axisTypeData.height : definition.height;
    const revolutions = (definition.pathType == PathType.PITCH) ? (abs(height.value) / pitch.value) : definition.revolutions;

    const angleToAdd = switch (definition.pathType) {
        PathType.PITCH : (definition.handedness == Direction.CW) ? getAngleFromRevolutions(revolutions) : -getAngleFromRevolutions(revolutions),
        PathType.TURNS : 0 * radian
    };
    const angleToEnd = getAngleToEnd(context, definition, startConditionData.angleToStart) + angleToAdd;

    const endRadius = (definition.axisType != AxisType.AXIS) ? axisTypeData.endRadius : (definition.endRadToggle ? definition.endRadius : axisTypeData.startRadius);

    var endPtLocal = getPointFromAngle(angleToEnd, endRadius);
    endPtLocal = vector(endPtLocal[0], endPtLocal[1], startConditionData.startPtLocal[2] + height);

    const endPtWorld = toWorld(localCoordSys, endPtLocal);

    return {
        'revolutions' : revolutions,
        'endRadius' : endRadius,
        'endPtWorld' : endPtWorld
    } as EndConditionData;
}

function getEndPointData(context is Context, id is Id, definition is map, localCoordSys is CoordSystem, axisTypeData is AxisTypeData, startConditionData is StartConditionData) returns EndConditionData
{
    const pitch = definition.helicalPitch;

    var endPtWorld = evVertexPoint(context, { "vertex" : definition.endPoint });
    if (fromWorld(localCoordSys, endPtWorld)[2] < startConditionData.startPtLocal[2])
    {
        localCoordSys.zAxis = -localCoordSys.zAxis;
    }

    const ptData = getPointData(localCoordSys, startConditionData.startPtWorld, endPtWorld);
    const endRadius = (definition.axisType == AxisType.AXIS) ? ptData.endRadius : axisTypeData.endRadius;

    const height = (definition.axisType == AxisType.SURFACE) ? axisTypeData.height : ptData.height;
    const axisPtEnd = vector(0 * meter, 0 * meter, height); // local end of axis

    endPtWorld = clampWorldPointToRadius(endPtWorld, localCoordSys, endRadius, axisPtEnd, ErrorStringEnum.HELIX_END_POINT_MISALIGNED, definition.endPoint);

    var revolutions = (definition.pathType == PathType.PITCH) ? (abs(ptData.height.value) / pitch.value) : definition.revolutions;
    revolutions = computeClosestPossibleRevolutionsToPoint(revolutions, ptData.angleStartToEnd, definition);

    setFeatureComputedParameter(context, id, { "name" : "actualRevolutions", "value" : revolutions });
    setFeatureComputedParameter(context, id, { "name" : "actualPitch", "value" : abs(ptData.height) / revolutions });

    return {
        'revolutions' : revolutions,
        'endRadius' : endRadius,
        'endPtWorld' : endPtWorld
    } as EndConditionData;
}

function getTurnsAndPitchData(context is Context, id is Id, definition is map, localCoordSys is CoordSystem, axisTypeData is AxisTypeData, startConditionData is StartConditionData) returns EndConditionData
{
    const revolutions = definition.revolutions;
    const pitch = definition.helicalPitch;
    const height = pitch * revolutions; // "Turns and Pitch" results in a fixed height, independent of surface shape

    const positiveDirection = definition.oppositeDirection ? -localCoordSys.zAxis : localCoordSys.zAxis;
    addHelixDirectionManipulator(context, id, definition, toWorld(localCoordSys, vector(0 * meter, 0 * meter, height)), positiveDirection, true);

    const followSurfaceProfile = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2279_HELIX_TURNS_PITCH_FOLLOW_SURFACE_PROFILE_FIX) && definition.axisType == AxisType.SURFACE;
    const endRadius = getTurnsAndPitchEndRadius(axisTypeData, revolutions, pitch, followSurfaceProfile);

    const angleToAdd = (definition.handedness == Direction.CW) ? getAngleFromRevolutions(revolutions) : -getAngleFromRevolutions(revolutions);
    const angleToEnd = getAngleToEnd(context, definition, startConditionData.angleToStart) + angleToAdd;

    var endPtLocal = getPointFromAngle(angleToEnd, endRadius);
    endPtLocal = vector(endPtLocal[0], endPtLocal[1], startConditionData.startPtLocal[2] + height);

    const endPtWorld = toWorld(localCoordSys, endPtLocal);

    return {
        'revolutions' : revolutions,
        'endRadius' : endRadius,
        'endPtWorld' : endPtWorld
    } as EndConditionData;
}

function getTurnsAndPitchEndRadius(axisTypeData is AxisTypeData, revolutions is number, pitch is ValueWithUnits, followSurfaceProfile is boolean) returns ValueWithUnits
{
    if (!followSurfaceProfile)
    {
        return axisTypeData.startRadius;
    }

    const spiralPitch = (axisTypeData.endRadius - axisTypeData.startRadius) / (axisTypeData.height / pitch);

    // using the spiral pitch derived from the surface profile, compute the end radius at the fixed number of revolutions
    return spiralPitch * revolutions + axisTypeData.startRadius;
}

function getAngleToEnd(context is Context, definition is map, angleToStart is ValueWithUnits) returns ValueWithUnits
{
    var angleToEnd = switch (definition.handedness)
    {
        Direction.CW : angleToStart,
        Direction.CCW : -angleToStart
    };

    if (definition.pathType == PathType.TURNS)
    {
        angleToEnd += getAngleFromRevolutions(definition.revolutions);
    }

    angleToEnd = adjustAngle(context, angleToEnd);
    angleToEnd = getRevolutionFraction(angleToEnd, definition.handedness) * 360 * degree;

    return angleToEnd;
}

// Helix will always end at specified or derived end point, so number of revolutions may differ slightly from specified input.
function computeClosestPossibleRevolutionsToPoint(revolutions is number, angleStartToEnd is ValueWithUnits, definition is map) returns number
{
    const revolutionFraction = getRevolutionFraction(angleStartToEnd, definition.handedness);

    // Skip rounding if specified number revolutions is close enough to actual number of revolutions
    if (abs(floor(revolutions) + revolutionFraction - revolutions) > HELIX_TURN_TOLERANCE)
    {
        // rounding settings
        const roundUp = (definition.pathType == PathType.TURNS && definition.roundUpTurns) || (definition.pathType == PathType.PITCH && definition.roundUpPitch);
        const revToRound = roundUp ? revolutions : revolutions - 1;

        revolutions = (revolutions % 1 < revolutionFraction) ? floor(revToRound) + revolutionFraction : ceil(revToRound) + revolutionFraction;
    }
    // prevents 0-revolution helix when both endpoints share the same angle from the center
    if (revolutions <= 0)
    {
        revolutions += 1;
    }

    return revolutions;
}

function enforceDirectionTowardsConeApex(localCoordSys is CoordSystem, startRadiusBox is box, startPtWorldBox is box, endRadiusBox is box, endPtWorldBox is box) returns CoordSystem
{
    const startRadiusIsZero = tolerantEquals(startRadiusBox[], 0 * meter);
    const endRadiusIsZero = tolerantEquals(endRadiusBox[], 0 * meter);

    verify(!(startRadiusIsZero && endRadiusIsZero), ErrorStringEnum.HELIX_BOTH_RADII_ZERO);

    const ptData = getPointData(localCoordSys, startPtWorldBox[], endPtWorldBox[]);
    if (startRadiusIsZero && !endRadiusIsZero)
    {
        const tempPt = startPtWorldBox[];
        startPtWorldBox[] = endPtWorldBox[];
        endPtWorldBox[] = tempPt;

        const tempRad = startRadiusBox[];
        startRadiusBox[] = endRadiusBox[];
        endRadiusBox[] = tempRad;

        localCoordSys.origin = toWorld(localCoordSys, ptData.axisPtEnd);
        localCoordSys.zAxis = -localCoordSys.zAxis;
    }
    else if (ptData.height < 0)
    {
        localCoordSys.zAxis = -localCoordSys.zAxis;
    }

    return localCoordSys;
}

function pointBasedHelixDefinition(handedness is Direction, localCoordSys is CoordSystem, startPtWorld is Vector, endPtWorld is Vector, startRadius is ValueWithUnits, endRadius is ValueWithUnits, revolutions is number) returns map
{
    const ptData = getPointData(localCoordSys, startPtWorld, endPtWorld);

    return {
        'direction' : localCoordSys.zAxis,
        'axisStart' : toWorld(localCoordSys, ptData.axisPtStart),
        'startPoint' : startPtWorld,
        'spiralPitch' : (endRadius - startRadius) / revolutions,
        'helicalPitch' : ptData.height / revolutions,
        'interval' : [0, revolutions],
        'clockwise' : (handedness == Direction.CW)
    };
}

function clampWorldPointToRadius(ptWorld is Vector, localCoordSys is CoordSystem, radius is ValueWithUnits, axisPt is Vector, misalignedPointMessage is ErrorStringEnum, defPoint is Query) returns Vector
{
    const ptLocal = fromWorld(localCoordSys, ptWorld);

    const faultyPointName = switch(misalignedPointMessage)
    {
        ErrorStringEnum.HELIX_START_POINT_MISALIGNED : "startPoint",
        ErrorStringEnum.HELIX_END_POINT_MISALIGNED : "endPoint"
    };

    verify(tolerantEquals(ptLocal[2], axisPt[2]), misalignedPointMessage, { "faultyParameters" : [faultyPointName], "entities" : defPoint });

    const clampedPtLocal = clampToRadius(axisPt, ptLocal, radius);
    const clampedPtWorld = toWorld(localCoordSys, clampedPtLocal);

    return clampedPtWorld;
}

function flipHeightIfOppositeDirection(height is ValueWithUnits, oppositeDirection is boolean) returns ValueWithUnits
{
    return oppositeDirection ? -height : height;
}

function getAngleFromRevolutions(revolutions is number) returns ValueWithUnits
{
    return (revolutions % 1) * 360 * degree;
}

function getPointFromAngle(angle is ValueWithUnits, radius is ValueWithUnits) returns Vector
{
    return radius * vector(cos(angle), sin(angle), 0);
}

function clampToRadius(localCenterPt is Vector, localPt is Vector, radius is ValueWithUnits) returns Vector
{
    return localCenterPt + (tolerantEquals(localCenterPt, localPt) ? vector(1, 0, 0) : normalize(localPt - localCenterPt)) * radius;
}

function getRevolutionFraction(angleStartToEnd is ValueWithUnits, handedness is Direction) returns number
{
    var revolutionFraction = angleStartToEnd / (2.0 * PI * radian); // turn number of radians into a fraction of 1 circle

    if (handedness == Direction.CCW)
    {
        revolutionFraction = 1 - revolutionFraction;
    }

    // allow < 1 full revolution, CW case
    if (revolutionFraction < 0)
    {
        revolutionFraction += 1;
    }
    else if (revolutionFraction > 1) // CCW case
    {
        revolutionFraction -= 1;
    }

    return revolutionFraction % 1;
}

function drawConstructionEnds(context is Context, id is Id, startPoint is Vector, endPoint is Vector, localCoordSys is CoordSystem, showStartEndProfiles is boolean)
{
    // Top-level ID check prevents start and end profiles from being drawn
    // if feature generation is part of a Mirror or Pattern applied by instance.
    if (!showStartEndProfiles || !isTopLevelId(id))
    {
        return;
    }

    const ptData = getPointData(localCoordSys, startPoint, endPoint);

    const startPlane = plane(toWorld(localCoordSys, ptData.axisPtStart), localCoordSys.zAxis, localCoordSys.xAxis);
    const startId = id + "drawStart";

    const endPlane = plane(toWorld(localCoordSys, ptData.axisPtEnd), localCoordSys.zAxis, localCoordSys.xAxis);
    const endId = id + "drawEnd";

    drawHelixEnd(context, startId, startPlane, startPoint, ptData.vectorToStart);
    drawHelixEnd(context, endId, endPlane, endPoint, ptData.vectorToEnd);
}

function drawHelixEnd(context is Context, sketchId is Id, plane is Plane, point is Vector, vectorToPoint is Vector)
{
    startFeature(context, sketchId);

    const sketch = newSketchOnPlane(context, sketchId, { "sketchPlane" : plane });

    const radius = norm(vectorToPoint);
    if (tolerantEquals(radius, 0 * meter))
    {
        skPoint(sketch, "point", {
            "position" : zeroVector(2) * meter
        });
    }
    else
    {
        skCircle(sketch, "circle", {
            "center" : zeroVector(2) * meter,
            "radius" : radius, // fails if close enough to 0 - drawing on apex
            "construction" : true
        });
    }

    skSolve(sketch);

    const sketchQuery = qCreatedBy(sketchId);
    addDebugEntities(context, sketchQuery, DebugColor.MAGENTA);

    abortFeature(context, sketchId);
}

function getPointData(localCoordSys is CoordSystem, startPtWorld is Vector, endPtWorld is Vector) returns map
{
    const startPtLocal = fromWorld(localCoordSys, startPtWorld);
    const endPtLocal = fromWorld(localCoordSys, endPtWorld);

    const vectorToStart = vector(startPtLocal[0], startPtLocal[1], 0 * meter);
    const vectorToEnd = vector(endPtLocal[0], endPtLocal[1], 0 * meter);

    const angleToStart = angleBetween(vector(1, 0, 0), vectorToStart, vector(0, 0, 1));
    const angleToEnd = angleBetween(vector(1, 0, 0), vectorToEnd, vector(0, 0, 1));

    return {
        'startPtLocal' : startPtLocal,
        'endPtLocal' : endPtLocal,

        'vectorToStart' : vectorToStart, // vector from axis to start point
        'vectorToEnd' : vectorToEnd, // vector from axis to end point

        'height' : endPtLocal[2] - startPtLocal[2],
        'axisPtStart' : startPtLocal - vectorToStart,
        'axisPtEnd' : endPtLocal - vectorToEnd,

        'startRadius' : norm(vectorToStart),
        'endRadius' : norm(vectorToEnd),
        'angleStartToEnd' : angleToEnd - angleToStart
    };
}

function addHelixDirectionManipulator(context is Context, id is Id, definition is map, base is Vector, direction is Vector, subtleFlip is boolean)
{
    addManipulators(context, id, {
        (DIRECTION_MANIPULATOR) : flipManipulator({
            "base" : base,
            "direction" : direction,
            "flipped" : definition.oppositeDirection,
            "updateClientGraphicsOnFlip" : !subtleFlip
        })
    });
}

function addHelixHeightManipulator(context is Context, id is Id, definition is map, base is Vector, direction is Vector)
{
    const offset = flipHeightIfOppositeDirection(definition.height, definition.oppositeDirection);

    addManipulators(context, id, {
        (HEIGHT_MANIPULATOR) : linearManipulator({
            "base" : base,
            "direction" : direction,
            "offset" : offset,
            "primaryParameterId" : "height"
        })
    });
}

/**
 * @internal
 * The manipulator change function used in the `helix` feature.
 */
export function helixManipulatorChange(context is Context, definition is map, manipulators is map) returns map
{
    if (manipulators[HEIGHT_MANIPULATOR] is map)
    {
        const newOffset = manipulators[HEIGHT_MANIPULATOR].offset;
        definition.oppositeDirection = newOffset < 0 * meter;
        definition.height = abs(newOffset);
    }

    if (manipulators[DIRECTION_MANIPULATOR] is map)
    {
        definition.oppositeDirection = manipulators[DIRECTION_MANIPULATOR].flipped;
    }

    return definition;
}

/**
 * @internal
 * Preselection Logic:  Heuristics to determine the type of helix to be constructed, based on user preselection.
 */
export function helixEditingLogic(context is Context, id is Id, oldDefinition is map, definition is map) returns map
{
    // Preselection only
    if (oldDefinition == {})
    {
        const faces = qEntityFilter(definition.initEntities, EntityType.FACE);
        const lines = qGeometry(definition.initEntities, GeometryType.LINE);
        const mates = qBodyType(definition.initEntities, BodyType.MATE_CONNECTOR);
        const circles = qGeometry(definition.initEntities, GeometryType.CIRCLE);
        const arcs = qGeometry(definition.initEntities, GeometryType.ARC);
        const edges = qUnion([circles, arcs]);

        if (evaluateQueryCount(context, faces) == 1)
        {
            definition.axisType = AxisType.SURFACE;
            definition.entities = faces;
        }
        else if (evaluateQueryCount(context, edges) == 1)
        {
            definition.axisType = AxisType.CIRCLE;
            definition.edge = edges;
        }
        else if (evaluateQueryCount(context, lines) == 1)
        {
            definition.axisType = AxisType.AXIS;
            definition.axis = lines;
        }
        else if (evaluateQueryCount(context, mates) == 1)
        {
            definition.axisType = AxisType.AXIS;
            definition.axis = mates;
        }
        // Clear out the pre-selection data: this is especially important if the query is to imported data
        definition.initEntities = qNothing();
    }

    return definition;
}

//===================================================<PRE_V1998>=======================================================

function doHelix_PRE_V1998(context is Context, id is Id, definition is map)
{
    if (definition.helixType == HelixType.TURNS || definition.helixType == HelixType.PITCH)
    {
        verifyNoMesh(context, definition, "entities");
    }
    else
    {
        verifyNoMesh(context, definition, "edge");
    }

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V858_SM_FLAT_BUG_FIXES))
    {
        if (definition.helixType == HelixType.TURNS || definition.helixType == HelixType.PITCH)
        {
            verifyNoSheetMetalFlatQuery(context, definition.entities, "entities", ErrorStringEnum.FLATTENED_SHEET_METAL_SKETCH_PROHIBITED);
        }
        else
        {
            verifyNoSheetMetalFlatQuery(context, definition.edge, "edge", ErrorStringEnum.FLATTENED_SHEET_METAL_SKETCH_PROHIBITED);
        }
    }
    var definitionOut = {};
    var remainingTransform;
    var revolutions;

    definition.startAngle = adjustAngle(context, definition.startAngle);
    if (definition.helixType == HelixType.TURNS || definition.helixType == HelixType.PITCH)
    {
        remainingTransform = getRemainderPatternTransform(context, { "references" : definition.entities });
        var surface = evSurfaceDefinition(context, { "face" : definition.entities });
        if ((surface is Cone) || (surface is Cylinder))
        {
            var endRadius;
            var baseRadius;
            const boxResult = evBox3d(context, { "topology" : definition.entities, "cSys" : surface.coordSystem });
            var minZ = boxResult.minCorner[2];
            var maxZ = boxResult.maxCorner[2];
            if (surface is Cylinder)
            {
                endRadius = surface.radius;
                baseRadius = surface.radius;
            }
            else if (surface is Cone)
            {
                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V421_TRIM_HELIX_AT_CONE_APEX))
                {
                    // Origin of coordinate system is guaranteed to be on the apex of a cone. The bounding box can be
                    // oversized by some tolerance (~1E-10m), so ensure helix won't cross through apex.
                    minZ = max(0 * meter, minZ);
                }
                const slope = tan(surface.halfAngle);
                baseRadius = slope * minZ;
                endRadius = slope * maxZ;
            }
            const height = maxZ - minZ;
            surface.coordSystem.origin += minZ * surface.coordSystem.zAxis;

            if (definition.helixType == HelixType.PITCH)
            {
                definitionOut.helicalPitch = definition.helicalPitch;
                revolutions = height.value / definition.helicalPitch.value;
            }
            else
            {
                revolutions = definition.revolutions;
                definitionOut.helicalPitch = height / revolutions;
            }

            definitionOut = updateDefinition(context, definitionOut, surface, baseRadius, endRadius, revolutions);
        }
        else
        {
            throw regenError(needConeOrCylinderMessage, ["entities"]);
        }
    }
    else
    {
        remainingTransform = getRemainderPatternTransform(context, { "references" : definition.edge });
        var edge = evCurveDefinition(context, { "edge" : definition.edge, "returnBSplinesAsOther" : true });
        if (!(edge is Circle))
        {
            throw regenError(needCircleMessage, ["entities"]);
        }
        var height;
        var baseRadius = edge.radius;
        var endRadius = baseRadius;

        if (definition.helixType == HelixType.TURNS_PITCH)
        {
            revolutions = definition.revolutions;
            definitionOut.helicalPitch = definition.helicalPitch;
            height = revolutions * definition.helicalPitch;
        }
        else
        {
            height = definition.height;
            if (definition.helixType == HelixType.HEIGHT_TURNS)
            {
                revolutions = definition.revolutions;
                definitionOut.helicalPitch = height / revolutions;
            }
            else if (definition.helixType == HelixType.HEIGHT_PITCH)
            {
                definitionOut.helicalPitch = definition.helicalPitch;
                revolutions = height / definition.helicalPitch;
            }
            // Add manipulator for the height of the helix
            var extrudeAxis = line(edge.coordSystem.origin, edge.coordSystem.zAxis);
            addHelixHeightManipulator(context, id, definition, extrudeAxis.origin, extrudeAxis.direction);
        }

        if (definition.oppositeDirection)
        {
            edge.coordSystem.zAxis *= -1;
        }

        definitionOut = updateDefinition(context, definitionOut, edge, baseRadius, endRadius, revolutions);
    }

    const startPointVector = definitionOut.startPoint - definitionOut.axisStart;
    definitionOut.startPoint = (rotationMatrix3d(definitionOut.direction, definition.startAngle) * startPointVector) + definitionOut.axisStart;

    definitionOut.interval = [0, revolutions];

    if (definition.handedness == Direction.CW)
    {
        definitionOut.clockwise = true;
    }
    else
    {
        definitionOut.clockwise = false;
    }
    opHelix(context, id, definitionOut);
    transformResultIfNecessary(context, id, remainingTransform);
}

function updateDefinition(context is Context, definitionOut is map, geometry is map, baseRadius is map, endRadius is map, revolutions is number) returns map
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V374_HELIX_ALIGNMENT))
    {
        // Align the "base" of the helix based on the alignCanonically() function
        // This keeps the coordSystems of circles/arcs and cylinders/cones consistent
        var alignmentPlane = plane(geometry.coordSystem);
        alignmentPlane = alignCanonically(context, alignmentPlane);

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V397_FLIP_CONE_HELIX))
        {
            // Cones always start drawing the helix from the "wrong" side (the tip)
            // This turns it around
            if (geometry is Cone)
            {
                // Relocate the origin:  Direction (unit vector) * Height (magnitude)
                alignmentPlane.origin += alignmentPlane.normal * definitionOut.helicalPitch * revolutions;
                // Flip the direction
                alignmentPlane.normal *= -1;
                // Realign the plane in its new location
                alignmentPlane = alignCanonically(context, alignmentPlane);
                // Swap base and end radius
                const swp = baseRadius;
                baseRadius = endRadius;
                endRadius = swp;
            }
        }
        definitionOut.direction = alignmentPlane.normal;
        definitionOut.axisStart = alignmentPlane.origin;
        definitionOut.startPoint = (alignmentPlane.x * baseRadius) + definitionOut.axisStart;
    }
    else
    {
        definitionOut.direction = geometry.coordSystem.zAxis;
        definitionOut.axisStart = geometry.coordSystem.origin;
        definitionOut.startPoint = (geometry.coordSystem.xAxis * baseRadius) + definitionOut.axisStart;
    }
    definitionOut.spiralPitch = (endRadius - baseRadius) / revolutions;
    return definitionOut;
}
