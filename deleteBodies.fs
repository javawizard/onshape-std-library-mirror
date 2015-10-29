FeatureScript 244; /* Automatically generated version */
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

