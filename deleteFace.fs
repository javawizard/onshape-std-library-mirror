FeatureScript 2581; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2581.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "2581.0");
import(path : "onshape/std/evaluate.fs", version : "2581.0");
import(path : "onshape/std/feature.fs", version : "2581.0");
import(path : "onshape/std/transform.fs", version : "2581.0");

/**
 * Specifies how the void resulting from delete face should be closed, if at all.
 *
 * @value HEAL : Close void by shrinking or growing adjacent faces.
 * @value CAP : Close void by a simple surface passing through all edges.
 * @value VOID : Do not close the void. Creates surface out of solids.
 */
export enum DeleteFaceType
{
    annotation { "Name" : "Heal" }
    HEAL,
    annotation { "Name" : "Cap" }
    CAP,
    annotation { "Name" : "Leave open" }
    VOID
}

/**
 * Feature performing an [opDeleteFace]. Has options to heal the void created by removing the selected faces,
 * or to leave it open.
 */
annotation { "Feature Type Name" : "Delete face", "Filter Selector" : "allparts", "Editing Logic Function" : "deleteFaceEditLogic" }
export const deleteFace = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Delete faces",
                    "UIHint" : UIHint.SHOW_CREATE_SELECTION,
                    "Filter" : (EntityType.FACE) && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES && AllowMeshGeometry.YES }
        definition.deleteFaces is Query;

        annotation { "Name" : "Type", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        definition.healType is DeleteFaceType;

        annotation { "Name" : "Delete fillet faces", "Default" : true }
        definition.includeFillet is boolean;

    }
    //============================ Body =============================
    {
        definition.capVoid = (definition.healType == DeleteFaceType.CAP);
        definition.leaveOpen = (definition.healType == DeleteFaceType.VOID);

        opDeleteFace(context, id, definition);
    }, { includeFillet : true, healType : DeleteFaceType.HEAL });

/**
 * @internal
 * Editing logic function for delete face feature
 */
export function deleteFaceEditLogic(context is Context, id is Id, oldDefinition is map, definition is map, specifiedParameters is map) returns map
{
    if (!specifiedParameters.healType && oldDefinition.deleteFaces != definition.deleteFaces && !isQueryEmpty(context, definition.deleteFaces))
    {
        const allSheetFaces = isQueryEmpty(context, qSubtraction(definition.deleteFaces, qBodyType(definition.deleteFaces, BodyType.SHEET)));
        const oldFacesEmpty = oldDefinition.deleteFaces == undefined || isQueryEmpty(context, oldDefinition.deleteFaces);
        if (allSheetFaces && oldFacesEmpty)
        {
            //We only change the heal type if all of the selected faces are sheet faces.
            //oldFacesEmpty alows the type to change via both preselection and first selection inside the feature
            definition.healType = DeleteFaceType.VOID;
        }
    }
    return definition;
}

