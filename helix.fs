FeatureScript ✨; /* Automatically generated version */
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/boolean.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");
export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/curveGeometry.fs", version : "");

const needConeOrCylinderMessage = ErrorStringEnum.HELIX_INPUT_CONE;

export enum HelixType
{
    annotation { "Name" : "Turns" }
    TURNS,
    annotation { "Name" : "Pitch" }
    PITCH
}

export enum Direction
{
    annotation { "Name" : "Clockwise" }
    CW,
    annotation { "Name" : "Counterclockwise" }
    CCW
}

annotation { "Feature Type Name" : "Helix", "UIHint" : "CONTROL_VISIBILITY" }
export const helix = defineFeature(function(context is Context, id is Id, helixDefinition is map)
    precondition
    {
        annotation { "Name" : "Helix type" }
        helixDefinition.helixType is HelixType;

        annotation { "Name" : "Conical or cylindrical face", "MaxNumberOfPicks" : 1, "Filter" : EntityType.FACE && QueryFilterCompound.ALLOWS_AXIS }
        helixDefinition.entities is Query;

        annotation { "Name" : "Handedness" }
        helixDefinition.handedness is Direction;

        if (helixDefinition.helixType == HelixType.PITCH)
        {
            annotation { "Name" : "Helical pitch" }
            isLength(helixDefinition.helicalPitch, NONNEGATIVE_LENGTH_BOUNDS);
        }
        else
        {
            annotation { "Name" : "Revolutions" }
            isReal(helixDefinition.revolutions, HELIX_TURN_BOUNDS);
        }

        annotation { "Name" : "Start angle" }
        isAngle(helixDefinition.startAngle, ANGLE_360_ZERO_DEFAULT_BOUNDS);
    }
    //===================================================<body>=======================================================
    {
        var helixDefinitionOut = {};
        var revolutions;

        var face = evSurfaceDefinition(context, { 'face' : helixDefinition.entities });

        if (reportFeatureError(context, id, face.error, ["entities"]))
            return;
        var surface = face.result;
        if ((surface is Cone) || (surface is Cylinder))
        {
            var endRadius = 0;
            var baseRadius = 0;
            var boxResult = evBox3d(context, { 'topology' : helixDefinition.entities, 'cSys' : toWorld(surface.coordSystem) });
            var height = boxResult.result.maxCorner[2] - boxResult.result.minCorner[2];
            if (surface is Cylinder)
            {
                endRadius = surface.radius;
                baseRadius = surface.radius;
            }
            else if (surface is Cone)
            {
                var slope = tan(surface.halfAngle);
                baseRadius = slope * boxResult.result.minCorner[2];
                endRadius = slope * boxResult.result.maxCorner[2];
            }
            surface.coordSystem.origin = surface.coordSystem.origin + boxResult.result.minCorner[2] * surface.coordSystem.zAxis;

            if (helixDefinition.helixType == HelixType.PITCH)
            {
                helixDefinitionOut.helicalPitch = helixDefinition.helicalPitch;
                revolutions = height.value / helixDefinition.helicalPitch.value;
            }
            else
            {
                revolutions = helixDefinition.revolutions;
                helixDefinitionOut.helicalPitch = height / revolutions;
            }

            helixDefinitionOut.direction = surface.coordSystem.zAxis;
            helixDefinitionOut.axisStart = surface.coordSystem.origin;
            helixDefinitionOut.spiralPitch = (endRadius - baseRadius) / revolutions;
            helixDefinitionOut.startPoint = (surface.coordSystem.xAxis * baseRadius) + helixDefinitionOut.axisStart;
        }
        else
        {
            reportFeatureError(context, id, needConeOrCylinderMessage, ["entities"]);
            return;
        }
        var startPointVector = helixDefinitionOut.startPoint - helixDefinitionOut.axisStart;
        helixDefinitionOut.startPoint = (rotationMatrix3d(helixDefinitionOut.direction, helixDefinition.startAngle) * startPointVector) + helixDefinitionOut.axisStart;

        helixDefinitionOut.interval = [0, revolutions];

        if (helixDefinition.handedness == Direction.CW)
        {
            helixDefinitionOut.clockwise = true;
        }
        else
        {
            helixDefinitionOut.clockwise = false;
        }

        opHelix(context, id, helixDefinitionOut);
    });

