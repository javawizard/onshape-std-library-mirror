FeatureScript 593; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.


export import(path : "onshape/std/smcornerreliefstyle.gen.fs", version : "593.0");

import(path : "onshape/std/attributes.fs", version : "593.0");
import(path : "onshape/std/containers.fs", version : "593.0");
import(path : "onshape/std/evaluate.fs", version : "593.0");
import(path : "onshape/std/feature.fs", version : "593.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "593.0");
import(path : "onshape/std/sheetMetalStart.fs", version : "593.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "593.0");
import(path : "onshape/std/smreliefstyle.gen.fs", version : "593.0");
import(path : "onshape/std/valueBounds.fs", version : "593.0");

/**
 * Corner feature is used to override default sheet metal model corner relief style or dimensions for an individual corner
 */
annotation { "Feature Type Name" : "Corner", "Filter Selector" : "allparts"}
export const sheetMetalCorner = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Corner", "MaxNumberOfPicks" : 1,
                     "Filter" : (SheetMetalDefinitionEntityType.VERTEX || (SheetMetalDefinitionEntityType.EDGE && EntityType.EDGE))
                                && AllowFlattenedGeometry.YES && ModifiableEntityOnly.YES }
        definition.corner is Query;

        annotation { "Name" : "Corner relief type", "Default" : SMCornerReliefStyle.RECTANGLE, "UIHint" : "SHOW_LABEL" }
        definition.cornerStyle is SMCornerReliefStyle;

        if (definition.cornerStyle == SMCornerReliefStyle.RECTANGLE || definition.cornerStyle == SMCornerReliefStyle.ROUND)
        {
            annotation { "Name" : "Corner relief scale" }
            isReal(definition.cornerReliefScale, CORNER_RELIEF_SCALE_BOUNDS);
        }
    }
    {
        var corner = findCornerDefinitionVertex(context, definition.corner);
        var cornerInfo = evCornerType(context, {
                "vertex" : corner
        });
        if (cornerInfo.cornerType == SMCornerType.NOT_A_CORNER) {
            throw regenError(ErrorStringEnum.SHEET_METAL_RIP_NO_CORNER, ['corner']);
        }
        else if (cornerInfo.cornerType == SMCornerType.BEND_END) {
            throw regenError(ErrorStringEnum.SHEET_METAL_BEND_END_NOT_A_CORNER, ['corner']);
        }
        if (cornerInfo.cornerType !=  SMCornerType.CLOSED_CORNER && definition.cornerStyle == SMCornerReliefStyle.CLOSED) {
            throw regenError(ErrorStringEnum.SHEET_METAL_NOT_A_CLOSED_CORNER, ['cornerStyle']);
        }

        corner = cornerInfo.primaryVertex;

        var existingAttribute = getCornerAttribute(context, corner);
        var newAttribute = createNewCornerAttribute(id, existingAttribute, definition.cornerStyle, definition.cornerReliefScale);
        var cornerVerticesQ = corner;
        if (existingAttribute != undefined)
        {
            cornerVerticesQ = replaceSMAttribute(context, existingAttribute, newAttribute);
        }
        else
        {
            setAttribute(context, { "entities" : corner, "attribute" : newAttribute });
        }
        updateSheetMetalGeometry(context, id, { "entities" : cornerVerticesQ ,
                                                "associatedChanges" : cornerVerticesQ
                                                });
    }, {});

function createNewCornerAttribute(id is Id, existingAttribute, cornerStyle is SMCornerReliefStyle, cornerReliefScale is number) returns SMAttribute
precondition
{
    if (existingAttribute != undefined)
    {
        existingAttribute is SMAttribute;
    }
}
{
    var cornerAttribute;
    if (existingAttribute != undefined)
    {
        cornerAttribute = existingAttribute;
    }
    else
    {
        cornerAttribute = makeSMCornerAttribute(toAttributeId(id));
    }

    var reliefStyle;
    if (cornerStyle == SMCornerReliefStyle.RECTANGLE) {
        reliefStyle = SMReliefStyle.RECTANGLE;
    } else if (cornerStyle == SMCornerReliefStyle.ROUND) {
        reliefStyle = SMReliefStyle.ROUND;
    } else if (cornerStyle == SMCornerReliefStyle.CLOSED) {
        reliefStyle = SMReliefStyle.CLOSED;
    } else if (cornerStyle == SMCornerReliefStyle.SIMPLE) {
        reliefStyle = SMReliefStyle.SIMPLE;
    } else {
        reliefStyle = SMReliefStyle.RECTANGLE;
    }

    cornerAttribute.cornerStyle = {
            "value" : reliefStyle,
            "canBeEdited" : false
        };
    if (cornerReliefScale != undefined)
    {
        cornerAttribute.cornerReliefScale = {
                "value" : cornerReliefScale,
                "canBeEdited" : false
            };
    }
    return cornerAttribute;
}

