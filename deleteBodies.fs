FeatureScript 2155; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2155.0");

// Imports used internally
import(path : "onshape/std/feature.fs", version : "2155.0");

/**
 * Options that determine how the Delete part feature handles composite parts
 *
 * @value DELETE : Any selected composite parts are deleted along with their constituents
 * @value DISSOLVE : Constituents of composites are only deleted if explicitly selected
 * @value IGNORE : Disallow selection of composite parts
 */
export enum CompositePartDeleteOptions
{
    annotation { "Name" : "Delete composite parts" }
    DELETE,
    annotation { "Name" : "Dissolve composite parts" }
    DISSOLVE,
    annotation { "Name" : "Ignore composite parts" }
    IGNORE
}

/**
 * Feature performing an [opDeleteBodies].
 */
annotation { "Feature Type Name" : "Delete part", "Editing Logic Function" : "deletePartEditLogic" }
export const deleteBodies = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Composite part options" }
        definition.compositePartOption is CompositePartDeleteOptions;

        if (definition.compositePartOption != CompositePartDeleteOptions.IGNORE)
        {
            annotation { "Name" : "Entities to delete",
                         "Filter" : EntityType.BODY && AllowMeshGeometry.YES && ModifiableEntityOnly.YES }
            definition.entities is Query;
        }
        else
        {
            annotation { "Name" : "Entities to delete",
                         "Filter" : EntityType.BODY && !BodyType.COMPOSITE && AllowMeshGeometry.YES && ModifiableEntityOnly.YES }
            definition.nonCompositeEntities is Query;
        }
    }
    {
        if (definition.compositePartOption == CompositePartDeleteOptions.IGNORE)
        {
            definition.entities = definition.nonCompositeEntities;
        }
        else if (definition.compositePartOption == CompositePartDeleteOptions.DELETE)
        {
            definition.entities = qUnion([definition.entities, qContainedInCompositeParts(definition.entities)]);
        }

        opDeleteBodies(context, id, definition);
    }, {
        compositePartOption : CompositePartDeleteOptions.DELETE
    });

/**
 * @internal
 * Edit logic to synchronize the two QLVs
 */
export function deletePartEditLogic(context is Context, id is Id, oldDefinition is map, definition is map) returns map
{
    // if entities changed, set nonCompositeEntities to the same but without composites
    if (oldDefinition.entities != definition.entities)
    {
        definition.nonCompositeEntities = qSubtraction(definition.entities, qBodyType(definition.entities, BodyType.COMPOSITE));
    }
    // else, if nonCompositeEntities changed, set entities to the same
    else if (oldDefinition.nonCompositeEntities != definition.nonCompositeEntities)
    {
        definition.entities = definition.nonCompositeEntities;
    }

    return definition;
}
