FeatureScript 2695; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.


export import(path : "onshape/std/smjointtype.gen.fs", version : "2695.0");
export import(path : "onshape/std/smjointstyle.gen.fs", version : "2695.0");

import(path : "onshape/std/sheetMetalAttribute.fs", version : "2695.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2695.0");
import(path : "onshape/std/feature.fs", version : "2695.0");
import(path : "onshape/std/valueBounds.fs", version : "2695.0");
import(path : "onshape/std/containers.fs", version : "2695.0");
import(path : "onshape/std/attributes.fs", version : "2695.0");
import(path : "onshape/std/evaluate.fs", version : "2695.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2695.0");
import(path : "onshape/std/math.fs", version : "2695.0");
import(path : "onshape/std/modifyFillet.fs", version : "2695.0");
import(path : "onshape/std/string.fs", version : "2695.0");

/**
 * sheetMetalJoint feature modifies sheet metal joint by changing its attribute.
 */
annotation { "Feature Type Name" : "Modify joint",
        "Filter Selector" : "allparts",
        "Editing Logic Function" : "sheetMetalJointEditLogic" }
export const sheetMetalJoint = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Joint",
                    "Filter" : (SheetMetalDefinitionEntityType.FACE || SheetMetalDefinitionEntityType.EDGE) && AllowFlattenedGeometry.YES && ModifiableEntityOnly.YES,
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
                isLength(definition.radius, SM_BEND_RADIUS_BOUNDS);
            }
            annotation { "Name" : "Use model K Factor", "Default" : true }
            definition.useDefaultKFactor is boolean;
            if (!definition.useDefaultKFactor)
            {
                annotation { "Name" : "K Factor" }
                isReal(definition.kFactor, K_FACTOR_BOUNDS);
            }
        }

        if (definition.jointType == SMJointType.RIP)
        {
            annotation { "Name" : "Has style", "Default" : true, "UIHint" : UIHint.ALWAYS_HIDDEN }
            definition.hasStyle is boolean;
            if (definition.hasStyle)
            {
                annotation { "Name" : "Joint style" }
                definition.jointStyle is SMJointStyle;
            }
        }
    }
    {
        //this is not necessary but helps with correct error reporting in feature pattern
        checkNotInFeaturePattern(context, definition.entity, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        if (!areEntitiesFromSingleActiveSheetMetalModel(context, definition.entity))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_JOIN_NEEDED, ["entity"]);
        }

        var jointEntity = findJointDefinitionEntity(context, definition.entity, EntityType.EDGE);
        var isFaceBend = false;
        if (jointEntity == undefined)
        {
            // Not an edge, is it a face?
            jointEntity = findJointDefinitionEntity(context, definition.entity, EntityType.FACE);
            isFaceBend = true;
        }
        if (jointEntity == undefined)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_JOIN_NEEDED, ["entity"]);
        }

        var existingAttribute = getJointAttribute(context, jointEntity);
        if (existingAttribute == undefined)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_JOIN_NEEDED, ["entity"]);
        }

        var newAttribute;
        if (definition.jointType == SMJointType.BEND)
        {
            if (definition.useDefaultRadius)
            {
                // The radius gets set for both edge and face bends but is not used for face bends.
                // And it should NOT be used for face bends because it is incorrect, the radius of a face bend
                // is defined by the geometry, not by the sheet metal model.
                definition.radius = getDefaultSheetMetalRadius(context, definition.entity);
            }
            else if (isFaceBend)
            {
                throw regenError(ErrorStringEnum.MUST_USE_DEFAULT_RADIUS_WITH_FACE_BEND, ["useDefaultRadius"]);
            }
            if (definition.useDefaultKFactor)
            {
                definition.kFactor = getDefaultSheetMetalKFactor(context, definition.entity);
            }

            if (!isFaceBend)
            {
                newAttribute = createNewEdgeBendAttribute(context, id, jointEntity, existingAttribute,
                    definition.radius, definition.useDefaultRadius,
                    definition.kFactor, definition.useDefaultKFactor);
            }
            else
            {
                newAttribute = createNewFaceBendAttribute(context, id, jointEntity, existingAttribute,
                    definition.kFactor, definition.useDefaultKFactor);
            }
        }
        else if (definition.jointType == SMJointType.RIP)
        {
            if (isFaceBend)
            {
                throw regenError(ErrorStringEnum.CANNOT_RIP_A_FACE_BEND, ["jointType"]);
            }
            newAttribute = createNewRipAttribute(id, existingAttribute, definition.jointStyle);
        }
        else if (definition.jointType == SMJointType.TANGENT)
        {
            if (isFaceBend)
            {
                throw regenError(ErrorStringEnum.CANNOT_MAKE_A_FACE_BEND_TANGENT, ["jointType"]);
            }
            newAttribute = createNewTangentAttribute(id, existingAttribute);
        }
        else
        {
            throw "This joint type is not supported";
        }

        if (!isEntityAppropriateForAttribute(context, jointEntity, newAttribute))
        {
            throw "Can not assign attribute type";
        }

        var jointEdgesQ = replaceSMAttribute(context, existingAttribute, newAttribute);
        updateSheetMetalGeometry(context, id, { "entities" : jointEdgesQ,
                    "associatedChanges" : jointEdgesQ
                });
    }, { jointStyle : SMJointStyle.EDGE, useDefaultRadius : true, hasStyle : true, useDefaultKFactor : true });


function getDefaultSheetMetalRadius(context is Context, entity is Query)
{
    var sheetmetalEntity = qUnion(getSMDefinitionEntities(context, entity));
    var modelParameters = getModelParameters(context, qOwnerBody(sheetmetalEntity));
    return modelParameters.defaultBendRadius;
}

function getDefaultSheetMetalKFactor(context is Context, entity is Query)
{
    var sheetmetalEntity = qUnion(getSMDefinitionEntities(context, entity));
    var modelParameters = getModelParameters(context, qOwnerBody(sheetmetalEntity));
    return modelParameters["k-factor"];
}

function findJointDefinitionEntity(context is Context, entity is Query, entityType is EntityType)
{
    const entityQ = qUnion(getSMDefinitionEntities(context, entity));
    var sheetEntities = qEntityFilter(entityQ, entityType);
    if (size(evaluateQuery(context, sheetEntities)) != 1)
    {
        return undefined;
    }
    else
    {
        return sheetEntities;
    }
}


function createNewEdgeBendAttribute(context is Context, id is Id, jointEdge is Query,
    existingAttribute is SMAttribute,
    radius, useDefaultRadius is boolean,
    kFactor, useDefaultKFactor is boolean) returns SMAttribute
precondition
{
    isLength(radius);
}
{
    var bendAttribute;
    if (existingAttribute.jointType.value != SMJointType.BEND)
    {
        bendAttribute = makeSMJointAttribute(existingAttribute.attributeId);
        bendAttribute.angle = existingAttribute.angle;
    }
    else
    {
        bendAttribute = existingAttribute;
    }

    const planarFacesQ = qGeometry(qAdjacent(jointEdge, AdjacencyType.EDGE, EntityType.FACE), GeometryType.PLANE);
    if (size(evaluateQuery(context, planarFacesQ)) != 2)
    {
        // If walls are non-planar bend angle depends on the radius and needs to be re-computed
        const angle = try silent(bendAngle(context, id, jointEdge, radius));
        if (angle == undefined || abs(angle) < TOLERANCE.zeroAngle * radian)
            throw regenError(ErrorStringEnum.SHEET_METAL_NO_0_ANGLE_BEND, ["entity"]);
        bendAttribute.angle = { "value" : angle, "canBeEdited" : false };
    }

    bendAttribute.jointType = {
            "value" : SMJointType.BEND,
            "controllingFeatureId" : toAttributeId(id),
            "parameterIdInFeature" : "jointType",
            "canBeEdited" : true
        };
    bendAttribute.bendType = {
            "value" : SMBendType.STANDARD,
            "canBeEdited" : false
        };
    bendAttribute.radius = {
            "value" : radius,
            "canBeEdited" : true,
            "isDefault" : useDefaultRadius
        };
    bendAttribute['k-factor'] = {
            "value" : kFactor,
            "canBeEdited" : true,
            "isDefault" : useDefaultKFactor
        };
    if (!useDefaultRadius || !useDefaultKFactor)
    {
        // If EITHER of the radius or k-factor are changed then we need to mark BOTH as being controlled by this feature so that subsequent
        // changes triggered through the sheet metal table modify this feature, rather than using separate ones
        const attributeId = toAttributeId(id);
        bendAttribute.radius.controllingFeatureId = attributeId;
        bendAttribute.radius.parameterIdInFeature = "radius";
        bendAttribute.radius.defaultIdInFeature = "useDefaultRadius";
        bendAttribute['k-factor'].controllingFeatureId = attributeId;
        bendAttribute['k-factor'].parameterIdInFeature = "kFactor";
        bendAttribute['k-factor'].defaultIdInFeature = "useDefaultKFactor";
    }
    return bendAttribute;
}

function createNewFaceBendAttribute(context is Context, id is Id, jointFace is Query,
    existingAttribute is SMAttribute,
    kFactor, useDefaultKFactor is boolean) returns SMAttribute
{
    var bendAttribute = existingAttribute;

    bendAttribute['k-factor'] = {
            "value" : kFactor,
            "canBeEdited" : true,
            "isDefault" : useDefaultKFactor
        };
    if (!useDefaultKFactor)
    {
        const attributeId = toAttributeId(id);
        bendAttribute['k-factor'].controllingFeatureId = attributeId;
        bendAttribute['k-factor'].parameterIdInFeature = "kFactor";
        bendAttribute['k-factor'].defaultIdInFeature = "useDefaultKFactor";
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
        abs(ripAttribute.angle.value / radian) > TOLERANCE.zeroAngle)
    {
        ripAttribute.jointStyle = {
                "value" : jointStyle,
                "controllingFeatureId" : toAttributeId(id),
                "parameterIdInFeature" : "jointStyle",
                "canBeEdited" : true
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

/**
 * @internal
 * Editing logic for sheetMetalJoint feature.
 * Parameter isCreating is needed for this method to be called when editing.
 */
export function sheetMetalJointEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    const definitionEntities = try silent(getSMDefinitionEntities(context, definition.entity, EntityType.EDGE));
    if (definitionEntities == undefined || size(definitionEntities) == 0)
    {
        return definition;
    }
    const jointEdgesQ = qUnion(definitionEntities);
    var existingAttribute = try silent(getJointAttribute(context, jointEdgesQ));
    if (existingAttribute != undefined &&
        existingAttribute.angle != undefined &&
        existingAttribute.angle.value != undefined &&
        abs(existingAttribute.angle.value / radian) > TOLERANCE.zeroAngle)
        definition.hasStyle = true;
    else
        definition.hasStyle = false;
    return definition;
}

