export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");

//Draft Operation
annotation {"Feature Type Name" : "Draft"}
export const draft = defineFeature(function(context is Context, id is Id, draftDefinition is map)
    precondition
    {
        annotation {"Name" : "Neutral plane",
                    "Filter" : GeometryType.PLANE,
                    "MaxNumberOfPicks" : 1}
        draftDefinition.neutralPlane is Query;

        annotation {"Name" : "Entities to draft", "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
        draftDefinition.draftFaces is Query;

        annotation {"Name" : "Draft angle"}
        isAngle(draftDefinition.angle, ANGLE_STRICT_90_BOUNDS);

        annotation {"Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION"}
        draftDefinition.pullDirection is boolean;

        annotation {"Name" : "Tangent propagation", "Default" : true}
        draftDefinition.tangentPropagation is boolean;

        annotation {"Name" : "Reapply fillet", "Default" : false}
        draftDefinition.reFillet is boolean;
    }
    {
        var planeResult = evFaceTangentPlane(context, {"face" : draftDefinition.neutralPlane, "parameter" : vector(0.5, 0.5)});
        if (planeResult.error != undefined)
        {
            reportFeatureError(context, id, ErrorStringEnum.DRAFT_SELECT_NEUTRAL, ["neutralPlane"]);
            return;
        }

        draftDefinition.pullVec = planeResult.result.normal;

        if(draftDefinition.pullDirection)
            draftDefinition.pullVec = -draftDefinition.pullVec;

        opDraft(context, id, draftDefinition);
    }, { pullDirection : false, tangentPropagation : false, reFillet : false });

