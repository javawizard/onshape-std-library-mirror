FeatureScript 293; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");

// Imports used internally
import(path : "onshape/std/feature.fs", version : "");

/**
 * @see `opDeleteBodies`.
 */
annotation { "Feature Type Name" : "Delete part" }
export const deleteBodies = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities to delete",
                     "Filter" : EntityType.BODY }
        definition.entities is Query;
    }
    {
        opDeleteBodies(context, id, definition);
    });

