FeatureScript 1472; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * Specifies a specific type of interactive manipulator.
 *
 * @value LINEAR_1D : A single arrow which can move along a single axis. See
 *         `extrude` for an example.
 * @value LINEAR_3D : A triad of perpendicular arrows which specify a 3D
 *         position. See `transformCopy` for an example.
 * @value ANGULAR   : A curved arrow, with two radii, which can move along a
 *         circumference to specify an angle. See `revolve` for an example.
 * @value FLIP      : A static arrow which can be clicked to toggle a flip
 *         direction. See `extrude` (with BoundingType.THROUGH_ALL) for an
 *         example.
 * @value POINTS    : A series of points which can be selected one at a time.
 */
export enum ManipulatorType
{
    LINEAR_1D,
    LINEAR_3D,
    ANGULAR,
    FLIP,
    POINTS
}


