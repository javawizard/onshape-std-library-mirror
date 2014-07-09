export import(path: "onshape/std/geomUtils.fs", version : "");
export import(path: "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");

export enum ModifyFilletType
{
    annotation {"Name" : "Change radius"}
    CHANGE_RADIUS,
    annotation {"Name" : "Remove fillet"}
    REMOVE_FILLET
}

annotation {"Feature Type Name" : "Modify fillet"}
export function modifyFillet(context is Context, id is Id, definition is map)
precondition
{
    annotation {"Name" : "Fillet faces to modify",
                "Filter": (EntityType.FACE) && ConstructionObject.NO && SketchObject.NO && !GeometryType.PLANE}
    definition.faces is Query;

    annotation {"Name" : "Modification type"}
    definition.modifyFilletType is ModifyFilletType;

    if(definition.modifyFilletType == ModifyFilletType.CHANGE_RADIUS)
    {
        annotation {"Name" : "Fillet radius"}
        isLength(definition.radius, BLEND_BOUNDS);
    }
}
//============================ Body =============================
{
    startFeature(context, id, definition);

    if(definition.radius == undefined)
        definition.radius = 0.0;

    opModifyFillet(context, id, definition);

    endFeature(context, id);
}



