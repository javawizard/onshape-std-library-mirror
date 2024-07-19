FeatureScript 2411; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * Describes how a Ruled Surface is defined.
 *
 * @value ANGLE_FROM_FACE: Ruled surface is perpendicular to reference faces.
 * @value ALIGNED_WITH_VECTOR: Ruled surface is in direction of reference direction.
 * @value ANGLE_FROM_VECTOR: Ruled surface will meet a reference direction at an angle when viewed from the path tangent direction. */
export enum RuledSurfaceType
{
    annotation {"Name" : "Normal"}
    ANGLE_FROM_FACE,
    annotation {"Name" : "Aligned with vector"}
    ALIGNED_WITH_VECTOR,
    annotation {"Name" : "Angle from vector"}
    ANGLE_FROM_VECTOR
}


