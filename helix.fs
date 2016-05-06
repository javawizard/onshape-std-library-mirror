FeatureScript 347; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "347.0");

// Imports used internally
import(path : "onshape/std/box.fs", version : "347.0");
import(path : "onshape/std/curveGeometry.fs", version : "347.0");
import(path : "onshape/std/evaluate.fs", version : "347.0");
import(path : "onshape/std/feature.fs", version : "347.0");
import(path : "onshape/std/mathUtils.fs", version : "347.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "347.0");
import(path : "onshape/std/valueBounds.fs", version : "347.0");

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
 * Specifies how the helical pitch will be defined.
 * @value TURNS : User defines the number of turns, and the pitch defined based
 *          on the input entity's height.
 * @default @value PITCH : User defines the helical pitch directly.
 */
export enum HelixType
{
    annotation { "Name" : "Turns" }
    TURNS,
    annotation { "Name" : "Pitch" }
    PITCH
}

const needConeOrCylinderMessage = ErrorStringEnum.HELIX_INPUT_CONE;

const HELIX_TURN_BOUNDS =
{
    "min"      : 0,
    "max"      : 1e9,
    (unitless) : [1e-5, 4, 1e5]
} as RealBoundSpec;

/**
 * Feature performing an `opHelix`.
 */
annotation { "Feature Type Name" : "Helix", "UIHint" : "CONTROL_VISIBILITY" }
export const helix = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Helix type" }
        definition.helixType is HelixType;

        annotation { "Name" : "Conical or cylindrical face", "MaxNumberOfPicks" : 1, "Filter" : EntityType.FACE && QueryFilterCompound.ALLOWS_AXIS }
        definition.entities is Query;

        annotation { "Name" : "Handedness" }
        definition.handedness is Direction;

        if (definition.helixType == HelixType.PITCH)
        {
            annotation { "Name" : "Helical pitch" }
            isLength(definition.helicalPitch, NONNEGATIVE_LENGTH_BOUNDS);
        }
        else
        {
            annotation { "Name" : "Revolutions" }
            isReal(definition.revolutions, HELIX_TURN_BOUNDS);
        }

        annotation { "Name" : "Start angle" }
        isAngle(definition.startAngle, ANGLE_360_ZERO_DEFAULT_BOUNDS);
    }
    //===================================================<body>=======================================================
    {
        var definitionOut = {};
        var revolutions;

        var remainingTransform = getRemainderPatternTransform(context,
            {"references" : definition.entities});

        var surface = evSurfaceDefinition(context, { 'face' : definition.entities });
        if ((surface is Cone) || (surface is Cylinder))
        {
            var endRadius = 0;
            var baseRadius = 0;
            const boxResult = evBox3d(context, { 'topology' : definition.entities, 'cSys' : surface.coordSystem });
            const height = boxResult.maxCorner[2] - boxResult.minCorner[2];
            if (surface is Cylinder)
            {
                endRadius = surface.radius;
                baseRadius = surface.radius;
            }
            else if (surface is Cone)
            {
                const slope = tan(surface.halfAngle);
                baseRadius = slope * boxResult.minCorner[2];
                endRadius = slope * boxResult.maxCorner[2];
            }
            surface.coordSystem.origin = surface.coordSystem.origin + boxResult.minCorner[2] * surface.coordSystem.zAxis;

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

            definitionOut.direction = surface.coordSystem.zAxis;
            definitionOut.axisStart = surface.coordSystem.origin;
            definitionOut.spiralPitch = (endRadius - baseRadius) / revolutions;
            definitionOut.startPoint = (surface.coordSystem.xAxis * baseRadius) + definitionOut.axisStart;
        }
        else
        {
            throw regenError(needConeOrCylinderMessage, ["entities"]);
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

    });

