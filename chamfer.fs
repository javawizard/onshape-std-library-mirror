FeatureScript 2878; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/chamfermethod.gen.fs", version : "2878.0");
export import(path : "onshape/std/chamfertype.gen.fs", version : "2878.0");
export import(path : "onshape/std/edgeBlendCommon.fs", version : "2878.0");
export import(path : "onshape/std/query.fs", version : "2878.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "2878.0");
import(path : "onshape/std/feature.fs", version : "2878.0");
import(path : "onshape/std/math.fs", version : "2878.0");
import(path : "onshape/std/matrix.fs", version : "2878.0");
import(path : "onshape/std/sheetMetalCornerBreakAttributeBased.fs", version : "2878.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2878.0");
import(path : "onshape/std/valueBounds.fs", version : "2878.0");

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

        annotation { "Name" : "Measurement", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE] }
        definition.chamferMethod is ChamferMethod;

        if (definition.chamferType != undefined)
        {
            annotation { "Name" : "Chamfer type", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE] }
            definition.chamferType is ChamferType;
        }

        //first quantity input (length)
        if (definition.chamferType != ChamferType.TWO_OFFSETS)
        {
            annotation { "Name" : "Distance", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE, UIHint.CAN_BE_TOLERANT] }
            isLength(definition.width, BLEND_BOUNDS);
        }
        else
        {
            annotation { "Name" : "Distance 1", "UIHint" : UIHint.CAN_BE_TOLERANT }
            isLength(definition.width1, BLEND_BOUNDS);
        }

        //opposite direction button
        if (definition.chamferType == ChamferType.OFFSET_ANGLE ||
            definition.chamferType == ChamferType.TWO_OFFSETS)
        {
            annotation { "Name" : "Opposite direction", "Default" : false,  "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.oppositeDirection is boolean;
        }

        //second quantity input (length or angle depending on type)
        if (definition.chamferType == ChamferType.TWO_OFFSETS)
        {
            annotation { "Name" : "Distance 2", "UIHint" : UIHint.CAN_BE_TOLERANT }
            isLength(definition.width2, BLEND_BOUNDS);
        }
        else if (definition.chamferType == ChamferType.OFFSET_ANGLE)
        {
            annotation { "Name" : "Angle", "UIHint" : UIHint.CAN_BE_TOLERANT }
            isAngle(definition.angle, CHAMFER_ANGLE_BOUNDS);
        }

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
    const tolerantIds = getActiveTolerantParameterIds(context, definition);
    if (size(tolerantIds) > 0)
    {
        definition.tolerantParameterIds = tolerantIds;
    }

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

function getActiveTolerantParameterIds(context is Context, definition is map) returns map
{
    const activeParameterIds = {
        ChamferType.EQUAL_OFFSETS: {width1: 'width', width2: 'width'},
        ChamferType.TWO_OFFSETS: {width1: 'width1', width2: 'width2'},
        ChamferType.OFFSET_ANGLE: {width1: 'width', angle: 'angle'}
    }[definition.chamferType];

    if (activeParameterIds == undefined)
    {
        return {};
    }
    const tolerant = getTolerantParameterIds(context, definition);
    if (size(tolerant) == 0)
    {
        return {};
    }

    var activeTolerantIds = {};
    for (var defId, parameterId in activeParameterIds)
    {
        if (tolerant[parameterId] != undefined)
        {
            activeTolerantIds[defId] = parameterId;
        }
    }
    return activeTolerantIds;
}

