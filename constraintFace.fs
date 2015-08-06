FeatureScript ✨; /* Automatically generated version */
export import(path : "onshape/std/geomOperations.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/transform.fs", version : "");
export import(path : "onshape/std/print.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");

export const CONSTRAINT_FACE_OFFSET_BOUNDS = BLEND_BOUNDS;

annotation { "Feature Type Name" : "Constrain face" }
export const constraintFace = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Anchor face", "Filter" : EntityType.FACE, "MaxNumberOfPicks" : 1 }
        definition.anchorFace is Query;

        annotation { "Name" : "Constraint type" }
        definition.constraintFaceType is ConstraintFaceType;

        if (definition.constraintFaceType == ConstraintFaceType.PARALLEL)
        {
            annotation { "Name" : "Offset distance" }
            isLength(definition.offset, CONSTRAINT_FACE_OFFSET_BOUNDS);

            annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
            definition.oppositeDirection is boolean;
        }

        annotation { "Name" : "Constrained faces",
                    "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
        definition.constraintFaces is Query;
    }
    //============================ Body =============================
    {
        opConstraintFace(context, id, definition);
    }, { oppositeDirection : false });

