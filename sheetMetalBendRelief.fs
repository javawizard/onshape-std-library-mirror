FeatureScript 2221; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.


export import(path : "onshape/std/smbendreliefstyle.gen.fs", version : "2221.0");

import(path : "onshape/std/attributes.fs", version : "2221.0");
import(path : "onshape/std/containers.fs", version : "2221.0");
import(path : "onshape/std/evaluate.fs", version : "2221.0");
import(path : "onshape/std/feature.fs", version : "2221.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2221.0");
import(path : "onshape/std/sheetMetalStart.fs", version : "2221.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2221.0");
import(path : "onshape/std/smreliefstyle.gen.fs", version : "2221.0");
import(path : "onshape/std/valueBounds.fs", version : "2221.0");

/**
 * Bend relief feature is used to override default bend relief of sheet metal model
 * at individual bend end.
 */
annotation { "Feature Type Name" : "Bend relief", "Filter Selector" : "allparts" }
export const sheetMetalBendRelief = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Bend relief", "MaxNumberOfPicks" : 1,
                    "Filter" : (SheetMetalDefinitionEntityType.VERTEX || (SheetMetalDefinitionEntityType.EDGE && EntityType.EDGE))
                        && AllowFlattenedGeometry.YES && ModifiableEntityOnly.YES }
        definition.bendRelief is Query;

        annotation { "Name" : "Bend relief type", "Default" : SMBendReliefStyle.SIZED_OBROUND, "UIHint" : UIHint.SHOW_LABEL }
        definition.bendReliefStyle is SMBendReliefStyle;

        if (definition.bendReliefStyle == SMBendReliefStyle.RECTANGLE || definition.bendReliefStyle == SMBendReliefStyle.OBROUND)
        {
            annotation { "Name" : "Bend relief depth scale" }
            isReal(definition.bendReliefDepthScale, BEND_RELIEF_DEPTH_SCALE_BOUNDS);
            annotation { "Name" : "Bend relief width scale" }
            isReal(definition.bendReliefWidthScale, BEND_RELIEF_WIDTH_SCALE_BOUNDS);
        }
        if (definition.bendReliefStyle == SMBendReliefStyle.SIZED_RECTANGLE || definition.bendReliefStyle == SMBendReliefStyle.SIZED_OBROUND)
        {
            annotation { "Name" : "Bend relief depth" }
            isLength(definition.bendReliefDepth, SM_RELIEF_SIZE_BOUNDS);
        }
        if (definition.bendReliefStyle != SMBendReliefStyle.TEAR)
        {
            annotation { "Name" : "Extend bend relief" }
            definition.extendBendRelief is boolean;
        }
    }
    {
        if (isQueryEmpty(context, definition.bendRelief))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_BEND_RELIEF_SELECT_ENTITIES, ['bendRelief']);
        }
        var corner;
        try
        {
            corner = findCornerDefinitionVertex(context, definition.bendRelief);
        }
        catch
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_BEND_RELIEF_NO_CORNER, ['bendRelief'], definition.bendRelief);
        }
        var cornerInfo = evCornerType(context, {
                "vertex" : corner
            });
        if (cornerInfo.cornerType == SMCornerType.NOT_A_CORNER)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_BEND_RELIEF_NO_CORNER, ['bendRelief'], definition.bendRelief);
        }
        else if (cornerInfo.cornerType != SMCornerType.BEND_END)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_CORNER_NOT_A_BEND_END, ['bendRelief'], definition.bendRelief);
        }

        corner = cornerInfo.primaryVertex;

        var existingAttribute = getCornerAttribute(context, corner);
        var baseAttribute = undefined; // We want to make entirely new attributes
        // In OLD versions we use the existing attribute as the basis of new ones. There is no good reason for that
        // and it causes stale data to hang around
        if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V671_FRESH_CORNER_OVERRIDE))
        {
            baseAttribute = existingAttribute;
        }
        var newAttribute = createNewCornerAttribute(id, baseAttribute, definition);
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
    }, {
            bendReliefStyle : SMBendReliefStyle.OBROUND,
            bendReliefDepthScale : 1.5,
            bendReliefWidthScale : 1.0625,
            bendReliefDepth : 0 * meter,
            extendBendRelief : false
        });

function createNewCornerAttribute(id is Id, existingAttribute, definition is map) returns SMAttribute
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

    const bendReliefStyle = definition.bendReliefStyle;
    var reliefStyle;
    if (bendReliefStyle == SMBendReliefStyle.OBROUND)
    {
        reliefStyle = SMReliefStyle.OBROUND;
    }
    else if (bendReliefStyle == SMBendReliefStyle.RECTANGLE)
    {
        reliefStyle = SMReliefStyle.RECTANGLE;
    }
    else if (bendReliefStyle == SMBendReliefStyle.TEAR)
    {
        reliefStyle = SMReliefStyle.TEAR;
    }
    else if (bendReliefStyle == SMBendReliefStyle.SIZED_OBROUND)
    {
        reliefStyle = SMReliefStyle.SIZED_OBROUND;
    }
    else if (bendReliefStyle == SMBendReliefStyle.SIZED_RECTANGLE)
    {
        reliefStyle = SMReliefStyle.SIZED_RECTANGLE;
    }
    else
    {
        reliefStyle = SMReliefStyle.OBROUND;
    }

    cornerAttribute.cornerStyle = {
            "value" : reliefStyle,
            "canBeEdited" : false
        };
    if (bendReliefStyle == SMBendReliefStyle.RECTANGLE || bendReliefStyle == SMBendReliefStyle.OBROUND)
    {
        if (definition.bendReliefDepthScale != undefined)
        {
            cornerAttribute.bendReliefDepthScale = {
                    "value" : definition.bendReliefDepthScale,
                    "canBeEdited" : false
                };
        }
        if (definition.bendReliefWidthScale != undefined)
        {
            cornerAttribute.bendReliefScale = {
                    "value" : definition.bendReliefWidthScale,
                    "canBeEdited" : false
                };
        }
    }
    else if (bendReliefStyle == SMBendReliefStyle.SIZED_RECTANGLE || bendReliefStyle == SMBendReliefStyle.SIZED_OBROUND)
    {
        if (definition.bendReliefDepth != undefined)
        {
            cornerAttribute.bendReliefDepth = {
                    "value" : definition.bendReliefDepth,
                    "canBeEdited" : false
                };
        }
    }
    if (bendReliefStyle != SMBendReliefStyle.TEAR && definition.extendBendRelief != undefined)
    {
        cornerAttribute.extendBendRelief = {
                "value" : definition.extendBendRelief,
                "canBeEdited" : false
            };
    }
    return cornerAttribute;
}

