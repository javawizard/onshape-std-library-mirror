FeatureScript 1991; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * Specifies how an extrude should terminate.
 *
 * @value BLIND : Extrude a specific distance.
 * @value UP_TO_NEXT : Extrude up to the next solid body or surface body in the
 *          context.
 * @value UP_TO_SURFACE : Extrude up to the specified face, construction plane,
 *          or surface body.
 * @value UP_TO_BODY : Extrude up to the specified solid body.
 * @value UP_TO_VERTEX : Extrude up to the specified vertex.
 * @value THROUGH_ALL : Extrude an unspecified distance, guaranteed to be further
 *          in the extrude direction than any other solid or surface in the context.
 */
export enum BoundingType
{
    annotation {"Name" : "Blind"}
    BLIND,
    annotation {"Name" : "Up to next"}
    UP_TO_NEXT,
    annotation {"Name" : "Up to face"}
    UP_TO_SURFACE,
    annotation {"Name" : "Up to part"}
    UP_TO_BODY,
    annotation {"Name" : "Up to vertex"}
    UP_TO_VERTEX,
    annotation {"Name" : "Through all"}
    THROUGH_ALL,
    annotation {"Hidden" : true}
    UP_TO_FACE
}


