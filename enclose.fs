FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "✨");

// Imports used internally
import(path : "onshape/std/feature.fs", version : "✨");

/**
 * Feature performing an [opEnclose].
 */
annotation { "Feature Type Name" : "Enclose" }
export const enclose = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities", "Filter" : EntityType.BODY || EntityType.FACE }
        definition.entities is Query;

        annotation {"Name" : "Merge results"}
        definition.mergeResults is boolean;
    }
    {
        opEnclose(context, id + "enclose", {
                    "entities" : definition.entities,
                    "mergeResults" : definition.mergeResults
                });
    });
