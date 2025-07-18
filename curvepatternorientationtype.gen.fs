FeatureScript 2716; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * Specifies the orientation type of patterned entities in curve pattern.
 *
 * @value DEFAULT : Patterned entities will be reoriented tangent to curve (default orientation).
 * @value LOCK_FACES : Lock the orientation of patterned entities normal to a set of faces.
 * @value KEEP_ORIENTATION : Match the orientation of patterned entities with the seed entity orientation.
 */
export enum CurvePatternOrientationType
{
    annotation {"Name" : "Tangent to curve"}
    DEFAULT,
    annotation {"Name" : "Normal to face"}
    LOCK_FACES,
    annotation {"Name" : "Locked"}
    KEEP_ORIENTATION
}


