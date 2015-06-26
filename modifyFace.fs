FeatureScript ✨; /* Automatically generated version */
export import(path: "onshape/std/geomUtils.fs", version : "");
export import(path: "onshape/std/evaluate.fs", version : "");
export import(path:"onshape/std/transform.fs", version : "");
export import(path:"onshape/std/print.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");

export enum ModifyFaceType
{
    annotation {"Name" : "Specify Parallel distance"}
    DISTANCE_SPEC,
    annotation {"Name" : "Specify angle"}
    ANGLE_SPEC,
    annotation {"Name" : "Specify Diameter"}
    DIAMETER_SPEC,
    annotation {"Name" : "Concentric"}
    CONCENTRIC
}

annotation {"Feature Type Name" : "Modify face"}
export const modifyFace = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation {"Name" : "Faces to be modified",
                    "Filter": EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
        definition.modifyFaces is Query;

        annotation {"Name" : "Modification type"}
        definition.modifyFaceType is ModifyFaceType;

        if(definition.modifyFaceType == ModifyFaceType.DISTANCE_SPEC || definition.modifyFaceType == ModifyFaceType.ANGLE_SPEC || definition.modifyFaceType == ModifyFaceType.CONCENTRIC)
        {
            annotation {"Name" : "Anchor entity",
                        "Filter": QueryFilterCompound.ALLOWS_AXIS ||  GeometryType.PLANE || EntityType.VERTEX,
                        "MaxNumberOfPicks" : 1}
            definition.anchorEntity is Query;

            if(definition.modifyFaceType == ModifyFaceType.DISTANCE_SPEC || definition.modifyFaceType == ModifyFaceType.ANGLE_SPEC)
            {
                annotation {"Name" : "Measure entity",
                            "Filter": QueryFilterCompound.ALLOWS_AXIS ||  GeometryType.PLANE || EntityType.VERTEX,
                            "MaxNumberOfPicks" : 1}
                definition.measureEntity is Query;
            }

            if (definition.modifyFaceType == ModifyFaceType.DISTANCE_SPEC)
            {
                annotation {"Name" : "Distance"}
                isLength(definition.distance, NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS);
            }

            if (definition.modifyFaceType == ModifyFaceType.ANGLE_SPEC)
            {
                annotation {"Name" : "Angle"}
                isAngle(definition.angle, ANGLE_STRICT_180_BOUNDS);
            }

            if(definition.modifyFaceType != ModifyFaceType.CONCENTRIC)
            {
                annotation {"Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION"}
                definition.oppositeDirection is boolean;
            }
        }
        if(definition.modifyFaceType == ModifyFaceType.DIAMETER_SPEC )
        {
            annotation {"Name" : "Diameter"}
            isLength(definition.diameter, NONNEGATIVE_LENGTH_BOUNDS);
        }

        annotation {"Name" : "Retain fillet", "Default" : true}
        definition.reFillet is boolean;
    }
    //============================ Body =============================
    {
        opModifyFace(context, id, definition);
    }, { oppositeDirection : false, reFillet : false });

