FeatureScript 2296; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
* Specifies topology option for opLoft.
* @default @value MINIMAL : Minimal number of faces created.
* @value COLUMNS : One face is created for each matching set of profile segments.
* @value GRID : Faces created for COLUMNS option are split at each profile.
 */
export enum LoftTopology
{
    MINIMAL,
    COLUMNS,
    GRID
}


