FeatureScript 190; /* Automatically generated version */
export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/geomOperations.fs", version : "");

//Chamfer Feature
annotation { "Feature Type Name" : "Chamfer", "Filter Selector" : "allparts" }
export const chamfer = defineFeature(function(context is Context, id is Id, chamferDefinition is map)
    precondition
    {
        annotation { "Name" : "Entities to chamfer",
                     "Filter" : ((EntityType.EDGE && EdgeTopology.TWO_SIDED) || EntityType.FACE) && ConstructionObject.NO && SketchObject.NO }
        chamferDefinition.entities is Query;

        if (chamferDefinition.chamferType != undefined)
        {
            annotation { "Name" : "Chamfer type" }
            chamferDefinition.chamferType is ChamferType;
        }

        //first quantity input (length)
        if (chamferDefinition.chamferType != ChamferType.TWO_OFFSETS)
        {
            annotation { "Name" : "Distance" }
            isLength(chamferDefinition.width, BLEND_BOUNDS);
        }
        else
        {
            annotation { "Name" : "Distance 1" }
            isLength(chamferDefinition.width1, BLEND_BOUNDS);
        }

        //opposite direction button
        if (chamferDefinition.chamferType == ChamferType.OFFSET_ANGLE ||
            chamferDefinition.chamferType == ChamferType.TWO_OFFSETS)
        {
            annotation { "Name" : "Opposite direction", "Default" : false, "UIHint" : "OPPOSITE_DIRECTION" }
            chamferDefinition.oppositeDirection is boolean;
        }

        //second quantity input (length or angle depending on type)
        if (chamferDefinition.chamferType == ChamferType.TWO_OFFSETS)
        {
            annotation { "Name" : "Distance 2" }
            isLength(chamferDefinition.width2, BLEND_BOUNDS);
        }
        else if (chamferDefinition.chamferType == ChamferType.OFFSET_ANGLE)
        {
            annotation { "Name" : "Angle" }
            isAngle(chamferDefinition.angle, CHAMFER_ANGLE_BOUNDS);
        }


        //tangent propagation option (checkbox)
        annotation { "Name" : "Tangent propagation", "Default" : true }
        chamferDefinition.tangentPropagation is boolean;
    }
    {
        opChamfer(context, id, chamferDefinition);
    }, { oppositeDirection : false, tangentPropagation : false });

