FeatureScript 2321; /* Automatically generated version */
import(path : "onshape/std/feature.fs", version : "2321.0");
import(path : "onshape/std/valueBounds.fs", version : "2321.0");
import(path : "onshape/std/topologyUtils.fs", version : "2321.0");
import(path : "onshape/std/math.fs", version : "2321.0");
import(path : "onshape/std/vector.fs", version : "2321.0");

/**
 *  Creates curves or split faces in a given direction at a given degree.
 */
annotation { "Feature Type Name" : "Isocline",
        "UIHint" : UIHint.NO_PREVIEW_PROVIDED }
export const isocline = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Faces", "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
        definition.faces is Query;

        annotation { "Name" : "Direction", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION, "MaxNumberOfPicks" : 1 }
        definition.direction is Query;

        annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Angle" }
        isAngle(definition.angle, ANGLE_STRICT_90_BOUNDS);

        annotation { "Name" : "Split faces" }
        definition.split is boolean;
    }
    {
        if (isQueryEmpty(context, definition.faces))
        {
            throw regenError(ErrorStringEnum.ISOCLINE_SELECT_FACES, ["faces"]);
        }
        if (isQueryEmpty(context, definition.direction))
        {
            throw regenError(ErrorStringEnum.ISOCLINE_SELECT_DIRECTION, ["direction"]);
        }
        var direction = extractDirection(context, definition.direction);
        if (norm(direction) < TOLERANCE.zeroLength)
        {
            throw regenError(ErrorStringEnum.ISOCLINE_SELECT_DIRECTION, ["direction"]);
        }
        if (definition.oppositeDirection)
        {
            direction = -direction;
        }
        const isoclineDefinition = {
                "faces" : definition.faces,
                "direction" : direction,
                "angle" : definition.angle
            };
        if (definition.split)
        {
            opSplitByIsocline(context, id, isoclineDefinition);
            if (isQueryEmpty(context, qCreatedBy(id)))
            {
                reportFeatureWarning(context, id, ErrorStringEnum.ISOCLINE_NO_RESULT);
            }
        }
        else
        {
            opCreateIsocline(context, id, isoclineDefinition);
        }
    });

