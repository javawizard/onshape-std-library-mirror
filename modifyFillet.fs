FeatureScript 2599; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2599.0");

// Imports used internally
import(path : "onshape/std/evaluate.fs", version : "2599.0");
import(path : "onshape/std/feature.fs", version : "2599.0");
import(path : "onshape/std/valueBounds.fs", version : "2599.0");

/**
 * Defines the action of a `modifyFillet` feature.
 */
export enum ModifyFilletType
{
    annotation { "Name" : "Change radius" }
    CHANGE_RADIUS,
    annotation { "Name" : "Remove fillet" }
    REMOVE_FILLET
}

/**
 * Feature performing an [opModifyFillet].
 */
annotation { "Feature Type Name" : "Modify fillet", "Filter Selector" : "allparts", "Editing Logic Function" : "modifyFilletLogic" }
export const modifyFillet = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Fillet faces to modify",
                     "Filter" : (EntityType.FACE) && ConstructionObject.NO && SketchObject.NO && !GeometryType.PLANE && ModifiableEntityOnly.YES }
        definition.faces is Query;

        annotation { "Name" : "Modification type" }
        definition.modifyFilletType is ModifyFilletType;

        if (definition.modifyFilletType == ModifyFilletType.CHANGE_RADIUS)
        {
            annotation { "Name" : "Fillet radius" }
            isLength(definition.radius, BLEND_BOUNDS);

            annotation { "Name" : "Reapply fillet", "Default" : true }
            definition.reFillet is boolean;
        }
    }
    {
        verifyNoMesh(context, definition, "faces");
        opModifyFillet(context, id, definition);
    }, { reFillet : false });

/**
 * @internal
 * Editing logic function for automatically setting the radius of the fillet.
 */
export function modifyFilletLogic(context is Context, id is Id, oldDefinition is map, definition is map, specified is map) returns map
{
    if (oldDefinition.faces == definition.faces ||
            definition.modifyFilletType != ModifyFilletType.CHANGE_RADIUS ||
            specified.radius)
        return definition;
    // If we're keeping any old faces, don't change the radius
    var oldFaces = oldDefinition.faces == undefined ? [] : evaluateQuery(context, oldDefinition.faces);
    if (oldFaces != [])
        return definition;
    try
    {
        definition.radius = evFilletRadius(context, { "face" : definition.faces });
    }
    return definition;
}

