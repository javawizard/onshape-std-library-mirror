FeatureScript 275; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/chamfertype.gen.fs", version : "");
export import(path : "onshape/std/query.fs", version : "");

// Imports used internally
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/valueBounds.fs", version : "");

/**
 * The chamfer feature just performs a chamfer operation.  @see `opChamfer`.
 */
annotation { "Feature Type Name" : "Chamfer", "Filter Selector" : "allparts" }
export const chamfer = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities to chamfer",
                     "Filter" : ((EntityType.EDGE && EdgeTopology.TWO_SIDED) || EntityType.FACE) && ConstructionObject.NO && SketchObject.NO }
        definition.entities is Query;

        if (definition.chamferType != undefined)
        {
            annotation { "Name" : "Chamfer type" }
            definition.chamferType is ChamferType;
        }

        //first quantity input (length)
        if (definition.chamferType != ChamferType.TWO_OFFSETS)
        {
            annotation { "Name" : "Distance" }
            isLength(definition.width, BLEND_BOUNDS);
        }
        else
        {
            annotation { "Name" : "Distance 1" }
            isLength(definition.width1, BLEND_BOUNDS);
        }

        //opposite direction button
        if (definition.chamferType == ChamferType.OFFSET_ANGLE ||
            definition.chamferType == ChamferType.TWO_OFFSETS)
        {
            annotation { "Name" : "Opposite direction", "Default" : false, "UIHint" : "OPPOSITE_DIRECTION" }
            definition.oppositeDirection is boolean;
        }

        //second quantity input (length or angle depending on type)
        if (definition.chamferType == ChamferType.TWO_OFFSETS)
        {
            annotation { "Name" : "Distance 2" }
            isLength(definition.width2, BLEND_BOUNDS);
        }
        else if (definition.chamferType == ChamferType.OFFSET_ANGLE)
        {
            annotation { "Name" : "Angle" }
            isAngle(definition.angle, CHAMFER_ANGLE_BOUNDS);
        }


        //tangent propagation option (checkbox)
        annotation { "Name" : "Tangent propagation", "Default" : true }
        definition.tangentPropagation is boolean;
    }
    {
        opChamfer(context, id, definition);
    }, { oppositeDirection : false, tangentPropagation : false });

