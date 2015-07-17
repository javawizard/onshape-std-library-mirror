FeatureScript ✨; /* Automatically generated version */
export import(path: "onshape/std/geomUtils.fs", version : "");
export import(path: "onshape/std/evaluate.fs", version : "");
export import(path:"onshape/std/transform.fs", version : "");
export import(path:"onshape/std/print.fs", version : "");

annotation { "Feature Type Name" : "Delete face", "Filter Selector" : "allparts" }
export const deleteFace = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation {"Name" : "Delete faces",
                    "UIHint" : "SHOW_CREATE_SELECTION",
                    "Filter": (EntityType.FACE) && ConstructionObject.NO && SketchObject.NO }
        definition.deleteFaces is Query;

        annotation {"Name" : "Delete fillet faces", "Default" : true}
        definition.includeFillet is boolean;

        annotation {"Name" : "Cap void", "Default" : false}
        definition.capVoid is boolean;
    }
    //============================ Body =============================
    {
        opDeleteFace(context, id, definition);
    }, { includeFillet : true, capVoid : false });

