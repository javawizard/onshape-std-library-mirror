FeatureScript 464; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */


export import(path : "onshape/std/smjointtype.gen.fs", version : "464.0");
export import(path : "onshape/std/smjointstyle.gen.fs", version : "464.0");

import(path : "onshape/std/sheetMetalAttribute.fs", version : "464.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "464.0");
import(path : "onshape/std/feature.fs", version : "464.0");
import(path : "onshape/std/valueBounds.fs", version : "464.0");
import(path : "onshape/std/containers.fs", version : "464.0");
import(path : "onshape/std/attributes.fs", version : "464.0");
import(path : "onshape/std/evaluate.fs", version : "464.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "464.0");
import(path : "onshape/std/math.fs", version : "464.0");
import(path : "onshape/std/modifyFillet.fs", version : "464.0");
import(path : "onshape/std/string.fs", version : "464.0");

/**
 * @internal
 */
annotation { "Feature Type Name" : "Modify joint", "Filter Selector" : "allparts" }
export const sheetMetalJoint = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entity",
                     "Filter" : (EntityType.FACE || EntityType.EDGE) && SketchObject.NO && BodyType.SOLID,
                     "MaxNumberOfPicks" : 1 }
        definition.entity is Query;

        annotation { "Name" : "Joint type", "Default" : SMJointType.BEND }
        definition.jointType is SMJointType;

        if (definition.jointType == SMJointType.BEND)
        {
            annotation { "Name" : "Use default radius", "Default" : true }
            definition.useDefaultRadius is boolean;
            if (!definition.useDefaultRadius)
            {
                annotation { "Name" : "Bend Radius" }
                isLength(definition.radius, BLEND_BOUNDS);
            }
        }

        if (definition.jointType == SMJointType.RIP)
        {
            annotation { "Name" : "Joint style" }
            definition.jointStyle is SMJointStyle;
        }
    }
    {
        //this is not necessary but helps with corrrect error reporting in feature pattern
        checkNotInFeaturePattern(context, definition.entity);

        if (!areEntitiesFromSingleActiveSheetMetalModel(context, definition.entity))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_JOIN_NEEDED, ["entity"]);
        }

        var jointEdge = findJointDefinitionEdge(context, definition.entity);
        var existingAttribute = getJointAttribute(context, jointEdge);
        if (existingAttribute.jointType.value == SMJointType.RIP &&
            existingAttribute.jointStyle.value == SMJointStyle.FLAT)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_FLAT_RIP_NO_EDIT);
        }
        var newAttribute;
        if (definition.jointType == SMJointType.BEND)
        {

            if (definition.useDefaultRadius)
            {
                definition.radius = getDefaultSheetMetalRadius(context, definition.entity);
            }
            newAttribute = createNewBendAttribute(id, existingAttribute, definition.radius, definition.useDefaultRadius);
        }
        else if (definition.jointType == SMJointType.RIP)
        {
            if (definition.jointStyle == SMJointStyle.FLAT)
            {
               throw regenError(ErrorStringEnum.SHEET_METAL_CANT_CHANGE_TO_FLAT);
            }
            newAttribute = createNewRipAttribute(id, existingAttribute, definition.jointStyle);
        }
        else
        {
            throw "This joint type is not supported";
        }

        var jointEdgesQ = replaceSMAttribute(context, existingAttribute, newAttribute);
        updateSheetMetalGeometry(context, id, { "entities" : jointEdgesQ });
    }, { jointStyle : SMJointStyle.EDGE, useDefaultRadius : true });

function getDefaultSheetMetalRadius(context is Context, entity is Query)
{
    var sheetmetalEntity = qUnion(getSMDefinitionEntities(context, entity));
    var sheetmetalBody = qOwnerBody(sheetmetalEntity);
    var attr = getAttributes(context, {"entities" : sheetmetalBody, "attributePattern" : asSMAttribute({})});
    if (size(attr) != 1 || attr[0].defaultBendRadius == undefined || attr[0].defaultBendRadius.value == undefined)
    {
        throw regenError("Bad sheet metal attribute");
    }
    return attr[0].defaultBendRadius.value;
}

function findJointDefinitionEdge(context is Context, entity is Query) returns Query
{
    var sheetEdges = qEntityFilter(qUnion(getSMDefinitionEntities(context, entity)), EntityType.EDGE);
    if (size(evaluateQuery(context, sheetEdges)) != 1)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_JOIN_NEEDED, ["entity"]);
    }
    return sheetEdges;
}


function createNewBendAttribute(id is Id, existingAttribute is SMAttribute, radius, useDefaultRadius is boolean) returns SMAttribute
precondition
{
    isLength(radius);
}
{
    var bendAttribute;
    if ( existingAttribute.jointType.value != SMJointType.BEND )
    {
        bendAttribute = makeSMJointAttribute(existingAttribute.attributeId);
        bendAttribute.angle = existingAttribute.angle;
    }
    else
    {
        bendAttribute = existingAttribute;
    }

    bendAttribute.jointType = {
        "value" : SMJointType.BEND,
        "controllingFeatureId" : toAttributeId(id),
        "parameterIdInFeature" : "jointType",
        "canBeEdited" : true
    };
    bendAttribute.radius = {
            "value" : radius,
            "canBeEdited" : true,
            "isDefault" : useDefaultRadius
    };
    if (!useDefaultRadius) {
        bendAttribute.radius.controllingFeatureId = toAttributeId(id);
        bendAttribute.radius.parameterIdInFeature = "radius";
    }
    return bendAttribute;
}

function createNewRipAttribute(id is Id, existingAttribute is SMAttribute, jointStyle) returns SMAttribute
{
    var ripAttribute = makeSMJointAttribute(existingAttribute.attributeId);
    ripAttribute.jointType = {
        "value" : SMJointType.RIP,
        "controllingFeatureId" : toAttributeId(id),
        "parameterIdInFeature" : "jointType",
        "canBeEdited" : true
    };
    ripAttribute.angle = existingAttribute.angle;
    ripAttribute.jointStyle = {
        "value" : jointStyle,
        "controllingFeatureId" : toAttributeId(id),
        "parameterIdInFeature" : "jointStyle",
        "canBeEdited": true
    };
    return ripAttribute;
}


