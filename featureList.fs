FeatureScript 293; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/** Support functions for the feature list parameter */

import(path : "onshape/std/context.fs", version : "");

/**
 * Parameter type that has a list of feature lambdas
 */
export type FeatureList typecheck canBeFeatureList;
export predicate canBeFeatureList(value)
{
    value is map;
    for (var entry in value)
    {
        entry.key is Id;
        entry.value is function;
    }
}

/**
 * Takes a map from id to lambda to return it as type FeatureList
 */
export function featureList(features is map) returns FeatureList
{
    return features as FeatureList;
}

/**
 * Takes a context and a map whose keys are subfeature ids from that context.  Returns the values
 * from that map sorted in the order that the subfeatures were started.
 */
export function valuesSortedById(context is Context, idToValue is map) returns array
{
    return @valuesSortedById(context, idToValue);
}

