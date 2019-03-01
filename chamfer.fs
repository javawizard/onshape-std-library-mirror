FeatureScript 1024; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/chamfertype.gen.fs", version : "1024.0");
export import(path : "onshape/std/query.fs", version : "1024.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "1024.0");
import(path : "onshape/std/feature.fs", version : "1024.0");
import(path : "onshape/std/math.fs", version : "1024.0");
import(path : "onshape/std/matrix.fs", version : "1024.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "1024.0");
import(path : "onshape/std/sheetMetalCornerBreak.fs", version : "1024.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "1024.0");
import(path : "onshape/std/valueBounds.fs", version : "1024.0");

const CHAMFER_ANGLE_BOUNDS =
{
    (degree) : [0.1, 45, 179.9],
    (radian) : 0.25 * PI
} as AngleBoundSpec;

/**
 * The chamfer feature directly performs an [opChamfer] operation.
 */
annotation { "Feature Type Name" : "Chamfer", "Filter Selector" : "allparts" }
export const chamfer = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities to chamfer",
                     "Filter" : ((ActiveSheetMetal.NO && ((EntityType.EDGE && EdgeTopology.TWO_SIDED) || EntityType.FACE))
                                || (EntityType.EDGE && SheetMetalDefinitionEntityType.VERTEX))
                                && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES,
                     "AdditionalBoxSelectFilter" : EntityType.EDGE }
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
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V575_SHEET_METAL_FILLET_CHAMFER))
        {
            sheetMetalAwareChamfer(context, id, definition);
        }
        else
        {
            standardChamfer(context, id, definition);
        }

    }, { oppositeDirection : false, tangentPropagation : false });

/*
 * Call sheetMetalCornerBreak on active sheet metal entities and opChamfer on the remaining entities
 */
function sheetMetalAwareChamfer(context is Context, id is Id, definition is map)
{
    var separatedQueries = separateSheetMetalQueries(context, definition.entities);
    var hasSheetMetalQueries = size(evaluateQuery(context, separatedQueries.sheetMetalQueries)) > 0;
    var hasNonSheetMetalQueries = size(evaluateQuery(context, separatedQueries.nonSheetMetalQueries)) > 0;

    if (!hasSheetMetalQueries && !hasNonSheetMetalQueries)
    {
        throw regenError(ErrorStringEnum.CHAMFER_SELECT_EDGES, ["entities"]);
    }

    if (hasSheetMetalQueries)
    {
        if (definition.chamferType != ChamferType.EQUAL_OFFSETS)
        {
            if (definition.chamferType == ChamferType.TWO_OFFSETS)
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_CHAMFER_NO_TWO_OFFSETS, ["chamferType"]);
            }
            else if (definition.chamferType == ChamferType.OFFSET_ANGLE)
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_CHAMFER_NO_OFFSET_ANGLE, ["chamferType"]);
            }
            else
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_CHAMFER_MUST_BE_EQUAL_OFFSETS, ["chamferType"]);
            }
        }

        var cornerBreakDefinition = {
                    "entities" : separatedQueries.sheetMetalQueries,
                    "cornerBreakStyle" : SMCornerBreakStyle.CHAMFER,
                    "range" : definition.width
                };
        try(sheetMetalCornerBreak(context, id + "smChamfer", cornerBreakDefinition));
        processSubfeatureStatus(context, id, {"subfeatureId" : id + "smChamfer", "propagateErrorDisplay" : true});
        if (featureHasError(context, id))
            return;
    }

    if (hasNonSheetMetalQueries)
    {
        definition.entities = separatedQueries.nonSheetMetalQueries;
        standardChamfer(context, id, definition);
    }
}

/*
 * Call opChamfer on non-sheet metal edges
 */
function standardChamfer(context is Context, id is Id, definition is map)
{
    if ( isAtVersionOrLater(context, FeatureScriptVersionNumber.V414_ASYMMETRIC_CHAMFER_MIRROR_BUG) &&
         (definition.chamferType == ChamferType.OFFSET_ANGLE || definition.chamferType == ChamferType.TWO_OFFSETS))
    {
        var fullTransform = getFullPatternTransform(context);
        if (abs(determinant(fullTransform.linear) + 1) < TOLERANCE.zeroLength) //det == -1
        {
            //we have a reflection on the input body, flip direction
            definition.oppositeDirection = !definition.oppositeDirection;
        }
    }
    opChamfer(context, id, definition);
}

