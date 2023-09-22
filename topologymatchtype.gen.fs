FeatureScript 2144; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * @internal
 * Specifies topology matches for [opBoolean].
 * @value COINCIDENT : Two regions are fully coincident (e.g. edges have the same curve geometry and coincident end points).
 * @value OVERLAPING : Two regions overlap (e.g. edges have the same curve geometry, share a common segment, but ends might not coincide).
 * @value CONTAINED_2_IN_1 : Region 2 is fully contained in region 1.
 */
export enum TopologyMatchType
{
    COINCIDENT,
    OVERLAPING,
    CONTAINED_2_IN_1
}


