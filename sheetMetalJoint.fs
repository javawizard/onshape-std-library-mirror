FeatureScript 736; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.


export import(path : "onshape/std/smjointtype.gen.fs", version : "736.0");
export import(path : "onshape/std/smjointstyle.gen.fs", version : "736.0");

import(path : "onshape/std/sheetMetalAttribute.fs", version : "736.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "736.0");
import(path : "onshape/std/feature.fs", version : "736.0");
import(path : "onshape/std/valueBounds.fs", version : "736.0");
import(path : "onshape/std/containers.fs", version : "736.0");
import(path : "onshape/std/attributes.fs", version : "736.0");
import(path : "onshape/std/evaluate.fs", version : "736.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "736.0");
import(path : "onshape/std/math.fs", version : "736.0");
import(path : "onshape/std/modifyFillet.fs", version : "736.0");
import(path : "onshape/std/string.fs", version : "736.0");

/**
 * sheetMetalJoint feature modifies sheet metal joint by changing its attribute.
 */
annotation { "Feature Type Name" : "Modify joint", "Filter Selector" : "allparts" }
export const sheetMetalJoint = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Joint",
                     "Filter" : SheetMetalDefinitionEntityType.EDGE && AllowFlattenedGeometry.YES && ModifiableEntityOnly.YES,
                     "MaxNumberOfPicks" : 1 }
        definition.entity is Query;

        annotation { "Name" : "Joint type", "Default" : SMJointType.BEND }
        definition.jointType is SMJointType;

        if (definition.jointType == SMJointType.BEND)
        {
            annotation { "Name" : "Use model bend radius", "Default" : true }
            definition.useDefaultRadius is boolean;
            if (!definition.useDefaultRadius)
            {
                annotation { "Name" : "Bend radius" }
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
        //this is not necessary but helps with correct error reporting in feature pattern
        checkNotInFeaturePattern(context, definition.entity, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        if (!areEntitiesFromSingleActiveSheetMetalModel(context, definition.entity))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_JOIN_NEEDED, ["entity"]);
        }

        var jointEdge = findJointDefinitionEdge(context, definition.entity);
        var existingAttribute = getJointAttribute(context, jointEdge);
        if (existingAttribute == undefined)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_JOIN_NEEDED, ["entity"]);
        }
        var newAttribute;
        if (definition.jointType == SMJointType.BEND)
        {

            if (definition.useDefaultRadius)
            {
                definition.radius = getDefaultSheetMetalRadius(context, definition.entity);
            }
            newAttribute = createNewBendAttribute(context, id, jointEdge, existingAttribute, definition.radius, definition.useDefaultRadius);
        }
        else if (definition.jointType == SMJointType.RIP)
        {
            newAttribute = createNewRipAttribute(id, existingAttribute, definition.jointStyle);
        }
        else if (definition.jointType == SMJointType.TANGENT)
        {
            newAttribute = createNewTangentAttribute(id, existingAttribute);
        }
        else
        {
            throw "This joint type is not supported";
        }

        if (!isEntityAppropriateForAttribute(context, jointEdge, newAttribute))
        {
            throw "Can not assign attribute type";
        }

        var jointEdgesQ = replaceSMAttribute(context, existingAttribute, newAttribute);
        updateSheetMetalGeometry(context, id, { "entities" : jointEdgesQ ,
                                                "associatedChanges" : jointEdgesQ
                                                });
    }, { jointStyle : SMJointStyle.EDGE, useDefaultRadius : true });

function getDefaultSheetMetalRadius(context is Context, entity is Query)
{
    var sheetmetalEntity = qUnion(getSMDefinitionEntities(context, entity));
    var modelParameters = getModelParameters(context, qOwnerBody(sheetmetalEntity));
    return modelParameters.defaultBendRadius;
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


function createNewBendAttribute(context is Context, id is Id, jointEdge is Query,
       existingAttribute is SMAttribute, radius, useDefaultRadius is boolean) returns SMAttribute
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

    const planarFacesQ = qGeometry(qEdgeAdjacent(jointEdge, EntityType.FACE), GeometryType.PLANE);
    if (size(evaluateQuery(context, planarFacesQ)) != 2)
    {
        // If walls are non-planar bend angle depends on the radius and needs to be re-computed
        const angle = try silent(bendAngle(context, id, jointEdge, radius));
        if (angle == undefined || abs(angle) < TOLERANCE.zeroAngle * radian)
            throw regenError(ErrorStringEnum.SHEET_METAL_NO_0_ANGLE_BEND, ["entity"]);
        bendAttribute.angle = {"value" : angle, "canBeEdited" : false};
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
    if (ripAttribute.angle != undefined &&
        ripAttribute.angle.value != undefined &&
        abs(ripAttribute.angle.value/radian) > TOLERANCE.zeroAngle)
    {
        ripAttribute.jointStyle = {
            "value" : jointStyle,
            "controllingFeatureId" : toAttributeId(id),
            "parameterIdInFeature" : "jointStyle",
            "canBeEdited": true
        };
    }
    return ripAttribute;
}

function createNewTangentAttribute(id is Id, existingAttribute is SMAttribute) returns SMAttribute
{
    var tangentAttribute = makeSMJointAttribute(existingAttribute.attributeId);
    tangentAttribute.jointType = {
        "value" : SMJointType.TANGENT,
        "controllingFeatureId" : toAttributeId(id),
        "parameterIdInFeature" : "jointType",
        "canBeEdited" : true
    };
    return tangentAttribute;
}


