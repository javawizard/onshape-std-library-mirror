FeatureScript 464; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */

export import(path : "onshape/std/smcornerstyle.gen.fs", version : "464.0");

import(path : "onshape/std/attributes.fs", version : "464.0");
import(path : "onshape/std/containers.fs", version : "464.0");
import(path : "onshape/std/feature.fs", version : "464.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "464.0");
import(path : "onshape/std/sheetMetalStart.fs", version : "464.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "464.0");
import(path : "onshape/std/valueBounds.fs", version : "464.0");

/**
 * @internal
 */
annotation { "Feature Type Name" : "Corner", "Filter Selector" : "allparts"}
export const sheetMetalCorner = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Corner", "Filter" : EntityType.EDGE || EntityType.VERTEX || EntityType.FACE, "MaxNumberOfPicks" : 1 }
        definition.corner is Query;

        annotation { "Name" : "Corner relief type", "Default" : SMCornerStyle.SQUARE, "UIHint" : "SHOW_LABEL" }
        definition.cornerStyle is SMCornerStyle;

        if (definition.cornerStyle == SMCornerStyle.SQUARE || definition.cornerStyle == SMCornerStyle.ROUND || definition.cornerStyle == SMCornerStyle.OBROUND)
        {
            annotation { "Name" : "Corner relief scale" }
            isReal(definition.cornerReliefScale, CORNER_RELIEF_SCALE_BOUNDS);
        }

        if (definition.cornerStyle == SMCornerStyle.OBROUND || definition.cornerStyle == SMCornerStyle.SQUARE || definition.cornerStyle == SMCornerStyle.TEAR)
        {
            annotation { "Name" : "Bend relief scale" }
            isReal(definition.bendReliefScale, BEND_RELIEF_SCALE_BOUNDS);
        }
    }
    {
        var corner = findCornerDefinitionVertex(context, definition.corner);
        var existingAttribute = getCornerAttribute(context, corner);
        var newAttribute = createNewCornerAttribute(id, existingAttribute, definition.cornerStyle, definition.cornerReliefScale, definition.bendReliefScale);
        var cornerVerticesQ = corner;
        if (existingAttribute != undefined)
        {
            cornerVerticesQ = replaceSMAttribute(context, existingAttribute, newAttribute);
        }
        else
        {
            setAttribute(context, { "entities" : corner, "attribute" : newAttribute });
        }
        updateSheetMetalGeometry(context, id, { "entities" : cornerVerticesQ });
    }, {});

function createNewCornerAttribute(id is Id, existingAttribute, cornerStyle is SMCornerStyle, cornerReliefScale is number, bendReliefScale is number) returns SMAttribute
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

    cornerAttribute.cornerStyle = {
            "value" : cornerStyle,
            "canBeEdited" : false
        };
    if (cornerReliefScale != undefined)
    {
        cornerAttribute.cornerReliefScale = {
                "value" : cornerReliefScale,
                "canBeEdited" : false
            };
    }
    if (bendReliefScale != undefined)
    {
        cornerAttribute.bendReliefScale = {
                "value" : bendReliefScale,
                "canBeEdited" : false
            };
    }
    return cornerAttribute;
}

