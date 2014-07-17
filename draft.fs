export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");
export import(path: "onshape/std/errorstringenum.gen.fs", version : "");

//Draft Operation
annotation {"Feature Type Name" : "Draft"}
export function draft(context is Context, id is Id, draftDefinition is map)
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
    annotation {"Name" : "Opposite direction", "UIHint" : "OppositeDirection"}
    draftDefinition.pullDirection is boolean;
    if(draftDefinition.tangentPropagation != undefined)
    {
        annotation {"Name" : "Tangent propagation", "Default" : true}
        draftDefinition.tangentPropagation is boolean;
    }
    if(draftDefinition.reFillet != undefined)
    {
        annotation {"Name" : "Reapply fillet", "Default" : false}
        draftDefinition.reFillet is boolean;
    }

}
{
    startFeature(context, id, draftDefinition);
    var planeResult = evFaceTangentPlane(context, {"face" : draftDefinition.neutralPlane, "parameter" : vector(0.5, 0.5)});
    if (planeResult.error != undefined)
    {
        reportFeatureError(context, id, ErrorStringEnum.DRAFT_SELECT_NEUTRAL);
        return;
    }

    draftDefinition.pullVec = planeResult.result.normal;
    if(draftDefinition.pullDirection)
        draftDefinition.pullVec = -draftDefinition.pullVec;

    if(draftDefinition.tangentPropagation == undefined)
        draftDefinition.tangentPropagation = false;

    if(draftDefinition.reFillet == undefined)
        draftDefinition.reFillet = false;

    opDraft(context, id, draftDefinition);
    endFeature(context, id);
}

