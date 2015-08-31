FeatureScript 213; /* Automatically generated version */
export import(path : "onshape/std/geomOperations.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");

//Draft Operation
annotation { "Feature Type Name" : "Draft", "Filter Selector" : "allparts" }
export const draft = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Neutral plane",
                     "Filter" : GeometryType.PLANE,
                     "MaxNumberOfPicks" : 1 }
        definition.neutralPlane is Query;

        annotation { "Name" : "Entities to draft", "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
        definition.draftFaces is Query;

        annotation { "Name" : "Draft angle" }
        isAngle(definition.angle, ANGLE_STRICT_90_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.pullDirection is boolean;

        annotation { "Name" : "Tangent propagation", "Default" : true }
        definition.tangentPropagation is boolean;

        annotation { "Name" : "Reapply fillet", "Default" : false }
        definition.reFillet is boolean;
    }
    {
        var planeResult = try(evFaceTangentPlane(context, { "face" : definition.neutralPlane, "parameter" : vector(0.5, 0.5) }));
        if (planeResult == undefined)
            throw regenError(ErrorStringEnum.DRAFT_SELECT_NEUTRAL, ["neutralPlane"]);

        definition.pullVec = planeResult.normal;

        if (definition.pullDirection)
            definition.pullVec = -definition.pullVec;

        opDraft(context, id, definition);
    }, { pullDirection : false, tangentPropagation : false, reFillet : false });

