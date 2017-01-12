FeatureScript 477; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */

export import(path : "onshape/std/smcornerreliefstyle.gen.fs", version : "477.0");

import(path : "onshape/std/attributes.fs", version : "477.0");
import(path : "onshape/std/containers.fs", version : "477.0");
import(path : "onshape/std/feature.fs", version : "477.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "477.0");
import(path : "onshape/std/sheetMetalStart.fs", version : "477.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "477.0");
import(path : "onshape/std/smreliefstyle.gen.fs", version : "477.0");
import(path : "onshape/std/valueBounds.fs", version : "477.0");

/**
 * @internal
 */
annotation { "Feature Type Name" : "Corner", "Filter Selector" : "allparts"}
export const sheetMetalCorner = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Corner", "Filter" : (EntityType.EDGE || EntityType.VERTEX || EntityType.FACE)
            && AllowFlattenedGeometry.YES, "MaxNumberOfPicks" : 1 }
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

