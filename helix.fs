FeatureScript 213; /* Automatically generated version */
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/boolean.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");
export import(path : "onshape/std/geomOperations.fs", version : "");
export import(path : "onshape/std/curveGeometry.fs", version : "");
export import(path : "onshape/std/box.fs", version : "");

const needConeOrCylinderMessage = ErrorStringEnum.HELIX_INPUT_CONE;

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

        var surface = evSurfaceDefinition(context, { 'face' : definition.entities });
        if ((surface is Cone) || (surface is Cylinder))
        {
            var endRadius = 0;
            var baseRadius = 0;
            var boxResult = evBox3d(context, { 'topology' : definition.entities, 'cSys' : toWorld(surface.coordSystem) });
            var height = boxResult.maxCorner[2] - boxResult.minCorner[2];
            if (surface is Cylinder)
            {
                endRadius = surface.radius;
                baseRadius = surface.radius;
            }
            else if (surface is Cone)
            {
                var slope = tan(surface.halfAngle);
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

        var startPointVector = definitionOut.startPoint - definitionOut.axisStart;
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
    });

