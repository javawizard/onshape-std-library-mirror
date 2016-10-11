FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */


export import(path : "onshape/std/smjointtype.gen.fs", version : "✨");
export import(path : "onshape/std/smjointstyle.gen.fs", version : "✨");

import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");
import(path : "onshape/std/modifyFillet.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");

/**
 * @internal
 */
annotation { "Feature Type Name" : "smJoint" }
export const smJoint = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "entity",
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
        if (!areEntitiesFromSingleActiveSheetMetalModel(context, definition.entity))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_JOIN_NEEDED, ["entity"]);
        }

        var jointEdge = findJointDefinitionEdge(context, definition.entity);
        var existingAttribute = getJointAttribute(context, jointEdge);
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
            newAttribute = createNewRipAttribute(id, existingAttribute, definition.jointStyle);
        }
        else
        {
            throw "This joint type is not supported";
        }
        replaceSMAttribute(context, jointEdge, existingAttribute, newAttribute);
        updateSheetMetalGeometry(context, id, { "entities" : jointEdge });
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

function getJointAttribute(context is Context, jointEdge is Query) returns map
{
    var attributes = getSmObjectTypeAttributes(context, jointEdge, SMObjectType.JOINT);
    if (size(attributes) != 1)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_JOIN_NEEDED, ["entity"]);
    }
    else
    {
        return attributes[0];
    }
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

function replaceJointAttribute(context is Context, entity is Query, existingAttribute is SMAttribute, newAttribute is SMAttribute)
{
    removeAttributes(context, { "entities" : entity, "attributePattern" : existingAttribute });
    setAttribute(context, { "entities" : entity, "attribute" : newAttribute });
}


