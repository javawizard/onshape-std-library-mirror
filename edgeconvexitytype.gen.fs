FeatureScript 1913; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * Specifies how two faces join at a given edge. This is a function of the interior
 * angle between the adjoining surfaces, measured inside the owner body.
 * @value CONVEX   : The interior angle is less than 180 degrees along the whole edge.
 * @value CONCAVE  : The interior angle is greater than 180 degrees along the whole edge.
 * @value SMOOTH   : The interior angle is equal to 180 degrees along the whole edge.
 * @value VARIABLE : None of the above, i.e. the edge convexity varies along the edge.
 */
export enum EdgeConvexityType
{
    CONVEX,
    CONCAVE,
    SMOOTH,
    VARIABLE
}


