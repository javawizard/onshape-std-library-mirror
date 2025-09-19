FeatureScript 2770; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * Specifies the class of intersection between two bodies. * @seealso [evCollision]
 *
 * @value NONE                 : No intersection.
 * @value INTERFERE            : Bounding topologies interfere.
 * @value EXISTS               : Vertex or edge intersection.
 * @value ABUT_NO_CLASS        : Bounding topologies abut.
 * @value ABUT_TOOL_IN_TARGET  : Bounding tool abuts bounding target on inside.
 * @value ABUT_TOOL_OUT_TARGET : Bounding tool abuts bounding target on outside.
 * @value TARGET_IN_TOOL       : Target completely inside tool.
 * @value TOOL_IN_TARGET       : Tool completely inside target.
 */
export enum ClashType
{
    NONE,
    INTERFERE,
    EXISTS,
    ABUT_NO_CLASS,
    ABUT_TOOL_IN_TARGET,
    ABUT_TOOL_OUT_TARGET,
    TARGET_IN_TOOL,
    TOOL_IN_TARGET
}


