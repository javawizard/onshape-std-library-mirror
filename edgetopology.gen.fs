FeatureScript 2737; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
* Specifies the topology of an edge entity.
*
* Can be used in a filter on a query parameter to only allow certain selections:
*``` annotation { "Name" : "Surface edges", "Filter" : EntityType.EDGE && EdgeTopology.ONE_SIDED } definition.edges is Query; ```
*
* @seeAlso [qEdgeTopologyFilter]
*
* @value WIRE : edge without adjacent faces, belonging to a wire body.
* @value ONE_SIDED : An edge adjacent to one face (e.g. the edge of a surface extrude).
* @default @value TWO_SIDED : An edge which joins two faces (e.g. the edge of a cube).
* @internal @value LAMINAR
*/
export enum EdgeTopology
{
    WIRE,
    ONE_SIDED,
    TWO_SIDED,
    LAMINAR
}


