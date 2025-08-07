FeatureScript 2737; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * Describes how a Ruled Surface creates corners.
 *
 * @value SPUN: Conical corners are created by spinning an edge.
 * @value EXTENDED: Ruled surfaces are altered so that they meet at corners.
 * @value LOFTED: Curvature continuous lofts are created patching corners.
 * @value NO_CORNER: Leave corners open.
 */
export enum RuledSurfaceCornerType
{
    annotation {"Name" : "Spun corner"}
    SPUN,
    annotation {"Name" : "Extended corner"}
    EXTENDED,
    annotation {"Name" : "Lofted corner"}
    LOFTED,
    annotation {"Name" : "No corner"}
    NO_CORNER
}


