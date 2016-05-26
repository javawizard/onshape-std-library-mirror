FeatureScript 355; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/feature.fs", version : "355.0");
import(path : "onshape/std/valueBounds.fs", version : "355.0");

/**
 * A `string` representing a foreign element, such as the `dataId` from an
 * imported tab.
 * @type {string}
 */
export type ForeignId typecheck canBeForeignId;

/** Typecheck for `ForeignId` */
export predicate canBeForeignId(value)
{
    value is string;
    //TODO: other checks
}

/**
 * Feature performing an `opImportForeign`, transforming the result if necessary.
 */
annotation { "Feature Type Name" : "Import" }
export const importForeign = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Foreign Id" }
        definition.foreignId is ForeignId;

        annotation { "Name" : "Source is 'Y Axis Up'" }
        definition.yAxisIsUp is boolean;

        annotation {"Name" : "Flatten assembly", "UIHint" : "ALWAYS_HIDDEN"}
        definition.flatten is boolean;

        annotation {"Name" : "Maximum number of assemblies created", "UIHint" : "ALWAYS_HIDDEN"}
        isInteger(definition.maxAssembliesToCreate, POSITIVE_COUNT_BOUNDS);
    }
    {
        var remainingTransform = getRemainderPatternTransform(context,
            {"references" : qNothing()});
        opImportForeign(context, id, definition);

        transformResultIfNecessary(context, id, remainingTransform);

    }, { yAxisIsUp : false, flatten : false, maxAssembliesToCreate : 10});

