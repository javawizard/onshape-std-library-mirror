FeatureScript 2796; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * Specifies how a profile should be controlled while sweeping.
 *
 * @value NONE : No additional profile control.
 * @value KEEP_ORIENTATION : Keep the orientation constant.
 * @value LOCK_FACES : Lock the sweep operation to a set of faces.
 * @value LOCK_DIRECTION : Lock the sweep operation to a direction.: 
 */
export enum ProfileControlMode
{
    annotation {"Name" : "None"}
    NONE,
    annotation {"Name" : "Keep profile orientation"}
    KEEP_ORIENTATION,
    annotation {"Name" : "Lock profile faces"}
    LOCK_FACES,
    annotation {"Name" : "Lock profile direction"}
    LOCK_DIRECTION
}


