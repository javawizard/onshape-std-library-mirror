export import(path: "onshape/std/geomUtils.fs", version : "");
export import(path: "onshape/std/evaluate.fs", version : "");
export import(path:"onshape/std/transform.fs", version : "");
export import(path:"onshape/std/print.fs", version : "");

annotation {"Feature Type Name" : "Delete face"}
export function deleteFace(context is Context, id is Id, definition is map)
precondition
{
    annotation {"Name" : "Delete faces",
                "UIHint" : "ShowCreateSelection",
                "Filter": (EntityType.FACE) && ConstructionObject.NO && SketchObject.NO }
    definition.deleteFaces is Query;

    if(definition.includeFillet != undefined)
    {
        annotation {"Name" : "Deleted fillet faces", "Default" : true}
        definition.includeFillet is boolean;
    }

    if(definition.capVoid != undefined)
    {
        annotation {"Name" : "Cap void", "Default" : false}
        definition.capVoid is boolean;
    }

}
//============================ Body =============================
{
    startFeature(context, id, definition);

    if(definition.includeFillet == undefined)
        definition.includeFillet = true;

    if(definition.capVoid == undefined)
        definition.capVoid = false;

    opDeleteFace(context, id, definition);

    endFeature(context, id);
}

