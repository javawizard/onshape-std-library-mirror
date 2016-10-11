FeatureScript 432; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/containers.fs", version : "432.0");
import(path : "onshape/std/context.fs", version : "432.0");
import(path : "onshape/std/feature.fs", version : "432.0");
import(path : "onshape/std/query.fs", version : "432.0");

/**
 * Returns true if edge has two adjacent faces, false if edge is laminar
 */
export function edgeIsTwoSided(context is Context, edge is Query) returns boolean
{
    return size(evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE))) == 2;
}


