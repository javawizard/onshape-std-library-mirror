FeatureScript 2615; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/chamfermethod.gen.fs", version : "2615.0");
export import(path : "onshape/std/chamfertype.gen.fs", version : "2615.0");
export import(path : "onshape/std/edgeBlendCommon.fs", version : "2615.0");
export import(path : "onshape/std/query.fs", version : "2615.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "2615.0");
import(path : "onshape/std/feature.fs", version : "2615.0");
import(path : "onshape/std/math.fs", version : "2615.0");
import(path : "onshape/std/matrix.fs", version : "2615.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2615.0");
import(path : "onshape/std/sheetMetalCornerBreakAttributeBased.fs", version : "2615.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2615.0");
import(path : "onshape/std/valueBounds.fs", version : "2615.0");


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

        chamferCommonOptions(definition);

        if (definition.chamferType == ChamferType.OFFSET_ANGLE ||
            definition.chamferType == ChamferType.TWO_OFFSETS)
        {
            annotation {"Name" : "Direction overrides",
                     "Filter" : ((ActiveSheetMetal.NO && (EntityType.EDGE && EdgeTopology.TWO_SIDED))
                                || (EntityType.EDGE && SheetMetalDefinitionEntityType.VERTEX))
                                && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES,
                     "AdditionalBoxSelectFilter" : EntityType.EDGE }
            definition.directionOverrides is Query;
        }

        //tangent propagation option (checkbox)
        annotation { "Name" : "Tangent propagation", "Default" : true }
        definition.tangentPropagation is boolean;
    }
    {
        verifyNoMesh(context, definition, "entities");

        if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V2211_CHAMFER_IMPROVEMENTS))
        {
            if (definition.chamferMethod == ChamferMethod.FACE_OFFSET && definition.chamferType != ChamferType.EQUAL_OFFSETS &&
                !isQueryEmpty(context, definition.directionOverrides))
            {
                reportFeatureInfo(context, id, ErrorStringEnum.CHAMFER_HELD_BACK);
            }
        }

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V575_SHEET_METAL_FILLET_CHAMFER))
        {
            sheetMetalAwareChamfer(context, id, definition);
        }
        else
        {
            standardChamfer(context, id, definition);
        }

    }, { oppositeDirection : false, tangentPropagation : false, chamferMethod : ChamferMethod.FACE_OFFSET, directionOverrides : qNothing() });

/*
 * Call sheetMetalCornerBreakAttributeBased on active sheet metal entities and opChamfer on the remaining entities
 */
function sheetMetalAwareChamfer(context is Context, id is Id, definition is map)
{
    var separatedQueries = separateSheetMetalQueries(context, definition.entities);
    var hasSheetMetalQueries = !isQueryEmpty(context, separatedQueries.sheetMetalQueries);
    var hasNonSheetMetalQueries = !isQueryEmpty(context, separatedQueries.nonSheetMetalQueries);

    if (!hasSheetMetalQueries && !hasNonSheetMetalQueries)
    {
        throw regenError(ErrorStringEnum.CHAMFER_SELECT_EDGES, ["entities"]);
    }

    if (hasSheetMetalQueries)
    {
        if (definition.chamferMethod != ChamferMethod.FACE_OFFSET)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_CHAMFER_OPTIONS_USE_CORNER_BREAK, ["chamferMethod"]);
        }
        if (definition.chamferType != ChamferType.EQUAL_OFFSETS)
        {
            if (definition.chamferType == ChamferType.TWO_OFFSETS)
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_CHAMFER_OPTIONS_USE_CORNER_BREAK, ["chamferType"]);
            }
            else if (definition.chamferType == ChamferType.OFFSET_ANGLE)
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_CHAMFER_OPTIONS_USE_CORNER_BREAK, ["chamferType"]);
            }
            else
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_CHAMFER_OPTIONS_USE_CORNER_BREAK, ["chamferType"]);
            }
        }

        reportFeatureInfo(context, id, ErrorStringEnum.SHEET_METAL_USE_CORNER_BREAK_INFO);

        var cornerBreakDefinition = {
                    "entities" : separatedQueries.sheetMetalQueries,
                    "cornerBreakStyle" : SMCornerBreakStyle.CHAMFER,
                    "range" : definition.width
                };
        callSubfeatureAndProcessStatus(id, sheetMetalCornerBreakAttributeBased, context, id + "smChamfer", cornerBreakDefinition);
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
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2211_CHAMFER_IMPROVEMENTS))
    {
        opChamfer(context, id, definition);
        return;
    }

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V414_ASYMMETRIC_CHAMFER_MIRROR_BUG) &&
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

