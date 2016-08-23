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
            annotation { "Name" : "Bend Radius" }
            isLength(definition.radius, BLEND_BOUNDS);
        }

        if (definition.jointType == SMJointType.RIP)
        {
            annotation { "Name" : "Joint style" }
            definition.jointStyle is SMJointStyle;
        }
    }
    {
        var jointEdge = findJointDefinitionEdge(context, definition.entity);
        var existingAttribute = getJointAttribute(context, jointEdge);
        var newAttribute;
        if (definition.jointType == SMJointType.BEND)
        {
            newAttribute = createNewBendAttribute(id, existingAttribute, definition.radius);
        }
        else if (definition.jointType == SMJointType.RIP)
        {
            newAttribute = createNewRipAttribute(id, existingAttribute);
        }
        else
        {
            throw "This joint type is not supported";
        }
        replaceSMAttribute(context, jointEdge, existingAttribute, newAttribute);
        updateSheetMetalGeometry(context, id, { "entities" : jointEdge });
    }, {});

function findJointDefinitionEdge(context is Context, entity is Query) returns Query
{
    var sheetEdges = qEntityFilter(qUnion(getSMDefinitionEntities(context, entity)), EntityType.EDGE);
    if (size(evaluateQuery(context, sheetEdges)) != 1)
    {
        throw "Selected entity does not belong to a recognized sheet metal joint";
    }
    return sheetEdges;
}

function getJointAttribute(context is Context, jointEdge is Query) returns map
{
    var attributes = getSmObjectTypeAttributes(context, jointEdge, SMObjectType.JOINT);
    if (size(attributes) != 1)
    {
        throw "Selected entity does not belong to a recognized sheet metal joint";
    }
    else
    {
        return attributes[0];
    }
}

function createNewBendAttribute(id is Id, existingAttribute is SMAttribute, radius) returns SMAttribute
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
            "isDefault" : false,
            "controllingFeatureId" : toAttributeId(id),
            "parameterIdInFeature" : "radius" };
    return bendAttribute;
}

function createNewRipAttribute(id is Id, existingAttribute is SMAttribute) returns SMAttribute
{
    var ripAttribute = makeSMJointAttribute(existingAttribute.attributeId);
    ripAttribute.jointType = {
        "value" : SMJointType.RIP,
        "controllingFeatureId" : toAttributeId(id),
        "parameterIdInFeature" : "jointType",
        "canBeEdited" : true
    };
    ripAttribute.angle = existingAttribute.angle;
    return ripAttribute;
}

function replaceJointAttribute(context is Context, entity is Query, existingAttribute is SMAttribute, newAttribute is SMAttribute)
{
    removeAttributes(context, { "entities" : entity, "attributePattern" : existingAttribute });
    setAttribute(context, { "entities" : entity, "attribute" : newAttribute });
}


