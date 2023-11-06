FeatureScript 2180; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.


export import(path : "onshape/std/smcornerreliefstyle.gen.fs", version : "2180.0");

import(path : "onshape/std/attributes.fs", version : "2180.0");
import(path : "onshape/std/containers.fs", version : "2180.0");
import(path : "onshape/std/evaluate.fs", version : "2180.0");
import(path : "onshape/std/feature.fs", version : "2180.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2180.0");
import(path : "onshape/std/sheetMetalStart.fs", version : "2180.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2180.0");
import(path : "onshape/std/smreliefstyle.gen.fs", version : "2180.0");
import(path : "onshape/std/valueBounds.fs", version : "2180.0");

/**
 * Corner feature is used to override default sheet metal model corner relief style or dimensions for an individual corner
 */
annotation { "Feature Type Name" : "Corner", "Filter Selector" : "allparts" }
export const sheetMetalCorner = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Corner", "MaxNumberOfPicks" : 1,
                    "Filter" : (SheetMetalDefinitionEntityType.VERTEX || (SheetMetalDefinitionEntityType.EDGE && EntityType.EDGE))
                        && AllowFlattenedGeometry.YES && ModifiableEntityOnly.YES }
        definition.corner is Query;

        annotation { "Name" : "Corner relief type", "Default" : SMCornerReliefStyle.SIZED_RECTANGLE, "UIHint" : UIHint.SHOW_LABEL }
        definition.cornerStyle is SMCornerReliefStyle;

        if (definition.cornerStyle == SMCornerReliefStyle.RECTANGLE || definition.cornerStyle == SMCornerReliefStyle.ROUND)
        {
            annotation { "Name" : "Corner relief scale" }
            isReal(definition.cornerReliefScale, CORNER_RELIEF_SCALE_BOUNDS);
        }
        if (definition.cornerStyle == SMCornerReliefStyle.SIZED_ROUND)
        {
            annotation { "Name" : "Corner relief diameter", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            isLength(definition.roundReliefDiameter, SM_RELIEF_SIZE_BOUNDS);
        }
        if (definition.cornerStyle == SMCornerReliefStyle.SIZED_RECTANGLE)
        {
            annotation { "Name" : "Corner relief width", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            isLength(definition.squareReliefWidth, SM_RELIEF_SIZE_BOUNDS);
        }
    }
    {
        if (isQueryEmpty(context, definition.corner))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_CORNER_SELECT_ENTITIES, ['corner']);
        }
        var corner;
        try
        {
            corner = findCornerDefinitionVertex(context, definition.corner);
        }
        catch
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_RIP_NO_CORNER, ['corner'], definition.corner);
        }
        var cornerInfo = evCornerType(context, {
                "vertex" : corner
            });
        if (cornerInfo.cornerType == SMCornerType.NOT_A_CORNER)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_RIP_NO_CORNER, ['corner'], definition.corner);
        }
        else if (cornerInfo.cornerType == SMCornerType.BEND_END)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_BEND_END_NOT_A_CORNER, ['corner'], definition.corner);
        }
        if (cornerInfo.cornerType != SMCornerType.CLOSED_CORNER && definition.cornerStyle == SMCornerReliefStyle.CLOSED)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_NOT_A_CLOSED_CORNER, ['cornerStyle'], definition.corner);
        }

        if (definition.cornerStyle != SMCornerReliefStyle.SIMPLE && isAtVersionOrLater(context, FeatureScriptVersionNumber.V727_SM_SUPPORT_ROLLED))
        {
            var facesQ = qAdjacent(qUnion(cornerInfo.allVertices), AdjacencyType.VERTEX, EntityType.FACE);
            var rolledQ = qSubtraction(facesQ, qGeometry(facesQ, GeometryType.PLANE));
            if (!isQueryEmpty(context, rolledQ))
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_ROLLED_CORNER_RELIF, ['cornerStyle'], rolledQ);
            }
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
                    "associatedChanges" : cornerVerticesQ
                });
    }, { cornerStyle : SMCornerReliefStyle.RECTANGLE, cornerReliefScale : 1.5, roundReliefDiameter: 0 * meter, squareReliefWidth: 0 * meter});

function createNewCornerAttribute(id is Id, existingAttribute, definition is map) returns SMAttribute
precondition
{
    if (existingAttribute != undefined)
    {
        existingAttribute is SMAttribute;
    }
    definition.cornerStyle is SMCornerReliefStyle;
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
    const cornerStyle = definition.cornerStyle;
    var reliefStyle;
    if (cornerStyle == SMCornerReliefStyle.RECTANGLE)
    {
        reliefStyle = SMReliefStyle.RECTANGLE;
    }
    else if (cornerStyle == SMCornerReliefStyle.ROUND)
    {
        reliefStyle = SMReliefStyle.ROUND;
    }
    else if (cornerStyle == SMCornerReliefStyle.CLOSED)
    {
        reliefStyle = SMReliefStyle.CLOSED;
    }
    else if (cornerStyle == SMCornerReliefStyle.SIMPLE)
    {
        reliefStyle = SMReliefStyle.SIMPLE;
    }
    else if (cornerStyle == SMCornerReliefStyle.SIZED_RECTANGLE)
    {
        reliefStyle = SMReliefStyle.SIZED_RECTANGLE;
    }
    else if (cornerStyle == SMCornerReliefStyle.SIZED_ROUND)
    {
        reliefStyle = SMReliefStyle.SIZED_ROUND;
    }
    else
    {
        reliefStyle = SMReliefStyle.RECTANGLE;
    }

    cornerAttribute.cornerStyle = {
            "value" : reliefStyle,
            "canBeEdited" : false
        };
    if (definition.cornerReliefScale != undefined && (cornerStyle == SMCornerReliefStyle.ROUND || cornerStyle == SMCornerReliefStyle.RECTANGLE))
    {
        cornerAttribute.cornerReliefScale = {
                "value" : definition.cornerReliefScale,
                "canBeEdited" : false
            };
    }
    if (cornerStyle == SMCornerReliefStyle.SIZED_ROUND)
    {
        cornerAttribute.roundReliefDiameter = {
                "value" : definition.roundReliefDiameter,
                "canBeEdited" : false
            };
    }
    if (cornerStyle == SMCornerReliefStyle.SIZED_RECTANGLE)
    {
        cornerAttribute.squareReliefWidth = {
                "value" : definition.squareReliefWidth,
                "canBeEdited" : false
            };
    }
    return cornerAttribute;
}

