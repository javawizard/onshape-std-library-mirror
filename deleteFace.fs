FeatureScript 1063; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "1063.0");

// Imports used internally
import(path : "onshape/std/evaluate.fs", version : "1063.0");
import(path : "onshape/std/feature.fs", version : "1063.0");
import(path : "onshape/std/transform.fs", version : "1063.0");


/**
 * Specifies how the void resulting from delete face should be closed, if at all.
 *
 * @value HEAL : Close void by shrinking or growing adjacent faces.
 * @value CAP : Close void by a simple surface passing through all edges.
 * @value VOID : Do not close the void. Creates surface out of solids.
 */
export enum DeleteFaceType
{
    annotation {"Name" : "Heal"}
    HEAL,
    annotation {"Name" : "Cap"}
    CAP,
    annotation {"Name" : "Leave open"}
    VOID
}

/**
 * Feature performing an [opDeleteFace]. Has options to heal the void created by removing the selected faces,
 * or to leave it open.
 */
annotation { "Feature Type Name" : "Delete face", "Filter Selector" : "allparts" }
export const deleteFace = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation {"Name" : "Delete faces",
                    "UIHint" : "SHOW_CREATE_SELECTION",
                    "Filter": (EntityType.FACE) && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES }
        definition.deleteFaces is Query;

        annotation {"Name" : "Type"}
        definition.healType is DeleteFaceType;

        annotation {"Name" : "Delete fillet faces", "Default" : true}
        definition.includeFillet is boolean;

    }
    //============================ Body =============================
    {
        definition.capVoid = (definition.healType == DeleteFaceType.CAP);
        definition.leaveOpen = (definition.healType == DeleteFaceType.VOID);

        opDeleteFace(context, id, definition);
    }, { includeFillet : true,  healType : DeleteFaceType.HEAL});

