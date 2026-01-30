FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * Used to specify the type of a feature dimension, as in [setDimensionedEntities].
 *
 * @value DISTANCE : Indicates the distance between two faces.
 * @value ANGLE : Indicates the angle between two planar faces.
 * @value RADIUS : Indicates the radius of a face with a circular cross-section of consistent radius, e.g. cylinder, torus.
 * @value DIAMETER : Indicates the diameter of a face with a circular cross-section of consistent radius.
 * @value AXIS_DISTANCE : Indicates the distance between the axes of two faces (cones or cylinders), or the distance between one such face and a planar face.
 */
export enum FeatureDimensionType
{
    DISTANCE,
    ANGLE,
    RADIUS,
    DIAMETER,
    AXIS_DISTANCE
}


