FeatureScript 2411; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/** Support functions for feature lists (as used for featurePattern) */

import(path : "onshape/std/context.fs", version : "2411.0");

/**
 * Parameter type for inputting a list of features, stored as a map from
 * feature `Id` to feature function. For an example, see the `circularPattern`
 * module.
 */
export type FeatureList typecheck canBeFeatureList;

/** Typecheck for [FeatureList] */
export predicate canBeFeatureList(value)
{
    value is map;
    for (var key, currentValue in value)
    {
        key is Id;
        currentValue is function;
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
 * Takes a context and a map whose keys are subfeature ids from that context.
 * Returns the values from that map sorted in the order that the subfeatures
 * were started.
 */
export function valuesSortedById(context is Context, idToValue is map) returns array
{
    return @valuesSortedById(context, idToValue);
}

/**
 * @internal
 * Returns true if any features listed in idToValue is a sketch.
 */
export function containsSketch(context is Context, idToValue is map)
{
    return @containsSketch(context, idToValue);
}

