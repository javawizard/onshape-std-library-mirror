FeatureScript 2615; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * Enumerates failure modes for tessellated loft.
 *
 * @value OK : No error.
 * @value CRISS_CROSSED_MATCHES : At least two matches cross each other.
 * @value UNDERNOURISHED_MATCHES : At least one match consists of fewer than 2 match points.
 * @value NON_CONSECUTIVE_MATCHES : A match jumps across a profile without specifying a match point.
 * @value MATCHING_CYLINDER_CREATION_FAILED : Most likely due to two profiles touching.
 * @value NON_CURVE_INPUT : Only curves are allowed within profiles.
 * @value UNIDENTIFIED : Some unknown error occurred.
 * @value INVALID_MATCH_POINT_INDEX : At least one of the match indices points to a non-existent 
 * match point.
 * @value INVALID_MATCH_POINT : At least one match point is specified on non-profile topology. 
 */
export enum TessellatedLoftReturnStatus
{
    OK,
    CRISS_CROSSED_MATCHES,
    UNDERNOURISHED_MATCHES,
    NON_CONSECUTIVE_MATCHES,
    MATCHING_CYLINDER_CREATION_FAILED,
    NON_CURVE_INPUT,
    UNIDENTIFIED,
    INVALID_MATCH_POINT_INDEX,
    INVALID_MATCH_POINT
}


