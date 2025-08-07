FeatureScript 2737; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2737.0");

// Imports used internally
import(path : "onshape/std/feature.fs", version : "2737.0");
import(path : "onshape/std/transform.fs", version : "2737.0");

/**
 * Feature that creates a composite part from provided bodies. Performs an [opCreateCompositePart].
 */
annotation { "Feature Type Name" : "Composite part" }
export const compositePart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities", "Filter" : EntityType.BODY && ModifiableEntityOnly.YES && AllowMeshGeometry.YES && SketchObject.NO }
        definition.bodies is Query;
        annotation { "Name" : "Closed", "Default" : false }
        definition.closed is boolean;
    }
    {
        const remainderTransform = getRemainderPatternTransform(context, {
                "references" : definition.bodies
        });
        if (remainderTransform != identityTransform())
        {
            throw regenError(ErrorStringEnum.PATTERN_FEATURE_FAILED);
        }
        opCreateCompositePart(context, id, definition);
    });

