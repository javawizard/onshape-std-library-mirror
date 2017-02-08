FeatureScript 505; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.


export import(path : "onshape/std/smbendreliefstyle.gen.fs", version : "505.0");

import(path : "onshape/std/attributes.fs", version : "505.0");
import(path : "onshape/std/containers.fs", version : "505.0");
import(path : "onshape/std/evaluate.fs", version : "505.0");
import(path : "onshape/std/feature.fs", version : "505.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "505.0");
import(path : "onshape/std/sheetMetalStart.fs", version : "505.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "505.0");
import(path : "onshape/std/smreliefstyle.gen.fs", version : "505.0");
import(path : "onshape/std/valueBounds.fs", version : "505.0");

/**
 * Bend relief feature is used to override default bend relief of sheet metal model
 * at individual bend end.
 */
annotation { "Feature Type Name" : "Bend relief", "Filter Selector" : "allparts"}
export const sheetMetalBendRelief = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Bend relief", "Filter" : (EntityType.EDGE || EntityType.VERTEX || EntityType.FACE)
            && AllowFlattenedGeometry.YES && AllowEdgePoint.NO, "MaxNumberOfPicks" : 1 }
        definition.bendRelief is Query;

        annotation { "Name" : "Bend relief type", "Default" : SMBendReliefStyle.OBROUND, "UIHint" : "SHOW_LABEL" }
        definition.bendReliefStyle is SMBendReliefStyle;

        if (definition.bendReliefStyle == SMBendReliefStyle.RECTANGLE || definition.bendReliefStyle == SMBendReliefStyle.OBROUND)
        {
            annotation { "Name" : "Bend relief depth scale" }
            isReal(definition.bendReliefDepthScale, CORNER_RELIEF_SCALE_BOUNDS);
            annotation { "Name" : "Bend relief width scale" }
            isReal(definition.bendReliefWidthScale, BEND_RELIEF_SCALE_BOUNDS);
        }
    }
    {
        var corner = findCornerDefinitionVertex(context, definition.bendRelief);
        var cornerInfo = evCornerType(context, {
                "vertex" : corner
        });

        if (cornerInfo.cornerType == SMCornerType.NOT_A_CORNER) {
            throw regenError(ErrorStringEnum.SHEET_METAL_RIP_NO_CORNER, ['corner']);
        }
        else if (cornerInfo.cornerType != SMCornerType.BEND_END) {
            throw regenError(ErrorStringEnum.SHEET_METAL_CORNER_NOT_A_BEND_END, ['corner']);
        }

        corner = cornerInfo.primaryVertex;

        var existingAttribute = getCornerAttribute(context, corner);
        var newAttribute = createNewCornerAttribute(id, existingAttribute, definition.bendReliefStyle, definition.bendReliefDepthScale, definition.bendReliefWidthScale);
        var cornerVerticesQ = corner;
        if (existingAttribute != undefined)
        {
            cornerVerticesQ = replaceSMAttribute(context, existingAttribute, newAttribute);
        }
        else
        {
            setAttribute(context, { "entities" : corner, "attribute" : newAttribute });
        }
        updateSheetMetalGeometry(context, id, { "entities" : cornerVerticesQ,
                                                "associatedChanges" : cornerVerticesQ });
    }, {});

function createNewCornerAttribute(id is Id, existingAttribute, bendReliefStyle is SMBendReliefStyle, bendReliefDepthScale is number, bendReliefWidthScale is number) returns SMAttribute
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
    if (bendReliefStyle == SMBendReliefStyle.OBROUND) {
        reliefStyle = SMReliefStyle.OBROUND;
    } else if (bendReliefStyle == SMBendReliefStyle.RECTANGLE) {
        reliefStyle = SMReliefStyle.RECTANGLE;
    } else if (bendReliefStyle == SMBendReliefStyle.TEAR) {
        reliefStyle = SMReliefStyle.TEAR;
    } else {
        reliefStyle = SMReliefStyle.OBROUND;
    }

    cornerAttribute.cornerStyle = {
            "value" : reliefStyle,
            "canBeEdited" : false
        };
    if (bendReliefDepthScale != undefined)
    {
        cornerAttribute.bendReliefDepthScale = {
                "value" : bendReliefDepthScale,
                "canBeEdited" : false
            };
    }
    if (bendReliefWidthScale != undefined)
    {
        cornerAttribute.bendReliefScale = {
                "value" : bendReliefWidthScale,
                "canBeEdited" : false
            };
    }
    return cornerAttribute;
}

