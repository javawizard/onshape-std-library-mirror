FeatureScript 347; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "347.0");

// Imports used internally
import(path : "onshape/std/evaluate.fs", version : "347.0");
import(path : "onshape/std/feature.fs", version : "347.0");
import(path : "onshape/std/geomOperations.fs", version : "347.0");
import(path : "onshape/std/valueBounds.fs", version : "347.0");

/**
 * @internal
 * The type of construction point.
 */
export enum PointType
{
    annotation {"Name": "Edge point"}
    EDGE_POINT
}

/**
 * @internal
 * A `RealBoundSpec` for a normalized parameter along an edge's length, with 0 being the start of the edge and 1 the end
 * Default UI value is 0.5, i.e. the midpoint of an open edge
 */
export const EDGE_PARAMETER_BOUNDS =
{
    "min"      : 0.0,
    "max"      : 1.0,
    (unitless) : [0.0, 0.5, 1]
} as RealBoundSpec;


/**
 * @internal
 * Creates a construction point, calling `opPoint`. Not exposed through the UI.
 */
annotation { "Feature Type Name" : "Point" }
export const cPoint = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        // Define the parameters of the feature type. Since the cpoint only takes a single edge I will call it edge
        // for UI purposes but for future compatibility I will call the item map entry 'entities'
        annotation { "Name" : "Edge", "Filter" : EntityType.EDGE, "MaxNumberOfPicks" : 1 }
        definition.entities is Query;

        annotation { "Name" : "Point type" }
        definition.pointType is PointType;

        annotation { "Name" : "Parameter" }
        isReal(definition.parameter,  EDGE_PARAMETER_BOUNDS);
    }
    {
        if (definition.pointType == PointType.EDGE_POINT)
        {
            definition.point = evEdgeTangentLine(context, { "edge" : definition.entities,
                                                        "parameter" : definition.parameter,
                                                        "arcLengthParameterization" : true }).origin;
        }
        opPoint(context, id, definition);
    }, { pointType: PointType.EDGE_POINT, parameter: 0.5 });

