FeatureScript 2856; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * Specifies the type of corner at a vertex of a sheet metal model.
 * @value OPEN_CORNER   : The corner has more than two bends or, when folded the edges of the metal do not meet.
 * @value CLOSED_CORNER : The corner has two bends and when folded the edge of the metal meet.
 * @value BEND_END      : The 'corner' is the end of a bend and there may be bend relief.
 * @value NOT_A_CORNER  : The vertex is not associated with a corner */
export enum SMCornerType
{
    OPEN_CORNER,
    CLOSED_CORNER,
    BEND_END,
    NOT_A_CORNER
}


