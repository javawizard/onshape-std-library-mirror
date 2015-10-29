FeatureScript ✨; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");

// Imports used internally
import(path : "onshape/std/evaluate.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/transform.fs", version : "");

/**
 * @see `opDeleteFace`.
 */
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

