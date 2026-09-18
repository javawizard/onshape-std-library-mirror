FeatureScript 3083; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.


export import(path : "onshape/std/smjointtype.gen.fs", version : "3083.0");
export import(path : "onshape/std/smjointstyle.gen.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalStart.fs", version : "3083.0");

import(path : "onshape/std/sheetMetalAttribute.fs", version : "3083.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "3083.0");
import(path : "onshape/std/feature.fs", version : "3083.0");
import(path : "onshape/std/valueBounds.fs", version : "3083.0");
import(path : "onshape/std/containers.fs", version : "3083.0");
import(path : "onshape/std/attributes.fs", version : "3083.0");
import(path : "onshape/std/math.fs", version : "3083.0");
import(path : "onshape/std/modifyFillet.fs", version : "3083.0");

const K_FACTOR_TOLERANCE = TOLERANCE.zeroLength * 100;

/**
 * A `RealBoundSpec` for sheet metal K-factor between -1.5 and 1., defaulting to `.45`. -1.5 is chosen arbitrarily to
 * allow for negative k-factors, the actual lower bound is determined by the bend radius and thickness of the sheet metal part.
 */
export const JOINT_K_FACTOR_BOUNDS =
{
    (unitless) : [-1.5, 0.45, 1]
} as RealBoundSpec;

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
                annotation { "Name" : "Bend calculation",
                    "Default" : SMBendCalculationType.K_FACTOR,
                    "UIHint" : "SHOW_LABEL" }
                definition.bendCalculationType is SMBendCalculationType;
                if (definition.bendCalculationType == SMBendCalculationType.K_FACTOR)
                {
                    annotation { "Name" : "K Factor" }
                    isReal(definition.kFactor, JOINT_K_FACTOR_BOUNDS);
                }
                else if (definition.bendCalculationType == SMBendCalculationType.BEND_ALLOWANCE)
                {
                    annotation { "Name" : "Bend allowance" }
                    isLength(definition.bendAllowance, LENGTH_BOUNDS);
                }
                else if (definition.bendCalculationType == SMBendCalculationType.BEND_DEDUCTION)
                {
                    annotation { "Name" : "Bend deduction" }
                    isLength(definition.bendDeduction, LENGTH_BOUNDS);
                }
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
            else
            {
                // The k-factor is the value which drives the geometry, so a bend which is specified by its bend
                // allowance or its bend deduction has to be converted to the equivalent k-factor. The sheet metal
                // table makes the same conversion when one of those columns is edited.
                definition.kFactor = getAndValidateKFactorFromBendCalculation(context, definition, existingAttribute, isFaceBend);
                checkKFactorModificationForPcb(context, id, definition, definition.entity);
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
    }, { jointStyle : SMJointStyle.EDGE, useDefaultRadius : true, hasStyle : true,
        useDefaultKFactor : true, "bendCalculationType" : SMBendCalculationType.K_FACTOR });


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

function getSheetMetalThickness(context is Context, entity is Query)
{
    var sheetmetalEntity = qUnion(getSMDefinitionEntities(context, entity));
    var modelParameters = getModelParameters(context, qOwnerBody(sheetmetalEntity));
    return modelParameters.frontThickness + modelParameters.backThickness;
}

/**
 * The bend allowance of a bend given the k-factor
 */
function bendAllowanceFromKFactor(kFactor is number, radius is ValueWithUnits, angle is ValueWithUnits, thickness is ValueWithUnits) returns ValueWithUnits
precondition
{
    isLength(radius);
    isAngle(angle);
    isLength(thickness);
}
{
    return (angle / radian) * (radius + (kFactor * thickness));
}

/**
 * The inverse of bendAllowanceFromKFactor.
 */
function kFactorFromBendAllowance(bendAllowance is ValueWithUnits, radius is ValueWithUnits, angle is ValueWithUnits, thickness is ValueWithUnits) returns number
precondition
{
    isLength(bendAllowance);
    isLength(radius);
    isAngle(angle);
    isLength(thickness);
}
{
    return ((bendAllowance / (angle / radian)) - radius) / thickness;
}

/**
 * Compute kFactor given bendDeduction.
 */
function kFactorFromBendDeduction(bendDeduction is ValueWithUnits, radius is ValueWithUnits, angle is ValueWithUnits, thickness is ValueWithUnits) returns number
precondition
{
    isLength(bendDeduction);
    isLength(radius);
    isAngle(angle);
    isLength(thickness);
}
{
    const setBack = (radius + thickness) * tan(angle / 2);
    return kFactorFromBendAllowance((2 * setBack) - bendDeduction, radius, angle, thickness);
}

/**
 * The k-factor equivalent to the bend allowance or the bend deduction which the definition specifies.
 */
function getAndValidateKFactorFromBendCalculation(context is Context, definition is map, existingAttribute is SMAttribute,
    isFaceBend is boolean) returns number
{
    if (existingAttribute.angle == undefined || existingAttribute.angle.value == undefined ||
        abs(existingAttribute.angle.value / radian) < TOLERANCE.zeroAngle)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_NO_0_ANGLE_BEND, ["entity"]);
    }
    const angle = existingAttribute.angle.value;

    // The radius of a face bend is defined by its geometry. The radius in the definition is the model default, which
    // is not the radius of this bend.
    var radius = definition.radius;
    if (isFaceBend)
    {
        if (existingAttribute.radius == undefined || existingAttribute.radius.value == undefined)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_JOIN_NEEDED, ["entity"]);
        }
        radius = existingAttribute.radius.value;
    }

    const thickness = getSheetMetalThickness(context, definition.entity);

    var kFactor = definition.kFactor;
    var parameterId = "kFactor";
    if (definition.bendCalculationType == SMBendCalculationType.BEND_ALLOWANCE)
    {
        kFactor = kFactorFromBendAllowance(definition.bendAllowance, radius, angle, thickness);
        parameterId = "bendAllowance";
    }
    else if (definition.bendCalculationType == SMBendCalculationType.BEND_DEDUCTION)
    {
        // The bend deduction measures the bend against going around the outside of it, so it only means anything for a
        // bend which turns through less than half a circle. A hem, for instance, does not have one.
        if (angle >= (PI - TOLERANCE.zeroAngle) * radian)
        {
            throw regenError(ErrorStringEnum.PARAMETER_OUT_OF_RANGE, ["bendDeduction"]);
        }
        kFactor = kFactorFromBendDeduction(definition.bendDeduction, radius, angle, thickness);
        parameterId = "bendDeduction";
    }

    //  Allow negative k-factors without letting bend allowance go negative
    if (!kFactorIsValid(kFactor, radius, thickness))
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_JOINT_K_FACTOR, [parameterId]);
    }
    return kFactor;
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
    const definitionEntities = try silent(getSMDefinitionEntities(context, definition.entity));
    var jointQuery = qUnion(definitionEntities)->qEntityFilter(EntityType.EDGE);
    var existingAttribute = undefined;
    var isFaceBend = false;
    if (!isQueryEmpty(context, jointQuery))
    {
        const jointEdgesQ = qUnion(definitionEntities);
        existingAttribute = try silent(getJointAttribute(context, jointEdgesQ));
        if (existingAttribute?.angle?.value != undefined &&
            abs(existingAttribute.angle.value / radian) > TOLERANCE.zeroAngle)
            definition.hasStyle = true;
        else
            definition.hasStyle = false;
    }
    else
    {
        jointQuery = qUnion(definitionEntities)->qEntityFilter(EntityType.FACE);
        isFaceBend = true;
    }

    if (definition.jointType == SMJointType.BEND && !isQueryEmpty(context, jointQuery))
    {
        var firstEditOfEnum = specifiedParameters.bendCalculationType != undefined && specifiedParameters.bendCalculationType &&
                              specifiedParameters.bendAllowance != undefined && !specifiedParameters.bendAllowance &&
                              specifiedParameters.bendDeduction != undefined && !specifiedParameters.bendDeduction;
        definition = updateBendCalculationValues(context, definition, isFaceBend, firstEditOfEnum,
                            existingAttribute != undefined ? existingAttribute : try silent(getJointAttribute(context, jointQuery)));
    }
    return definition;
}

/**
 * Keep the three ways of specifying a bend consistent with one another: the one which is selected is left as is
 * and the other two are calculated from it, so that changing the selection does not change the bend and the values
 * are not stale. Needed especially for upgraded features to show correct BA/BD values after upgrade.
 */
function updateBendCalculationValues(context is Context, definition is map, isFaceBend is boolean, firstEditOfEnum is boolean, existingAttribute) returns map
{
    const angle = existingAttribute?.angle?.value;
    if (angle == undefined || abs(angle / radian) < TOLERANCE.zeroAngle)
    {
        return definition;
    }

    // if it's a hem, bend deduction does not make sense.
    if (angle >= (PI - TOLERANCE.zeroAngle) * radian && definition.bendCalculationType == SMBendCalculationType.BEND_DEDUCTION)
    {
        return definition;
    }

    var radius = !definition.useDefaultRadius ? definition.radius : try silent(getDefaultSheetMetalRadius(context, definition.entity));
    if (isFaceBend)
    {
        const faceBendRadius = existingAttribute?.radius?.value;
        if (faceBendRadius != undefined)
        {
            radius = faceBendRadius;
        }
    }

    const thickness = try silent(getSheetMetalThickness(context, definition.entity));
    if (radius == undefined || thickness == undefined)
    {
        return definition;
    }

    const setBack = (radius + thickness) * tan(angle / 2);
    var kFactor = 0.0;
    if (!firstEditOfEnum && definition.bendCalculationType == SMBendCalculationType.BEND_ALLOWANCE)
    {
        kFactor = try silent(kFactorFromBendAllowance(definition.bendAllowance, radius, angle, thickness));
        if (kFactor != undefined && kFactorIsValid(kFactor, radius, thickness))
        {
            definition.kFactor = kFactor;
            definition.bendDeduction = 2 * setBack - definition.bendAllowance;
        }
    }
    else if (!firstEditOfEnum && definition.bendCalculationType == SMBendCalculationType.BEND_DEDUCTION)
    {
        kFactor = try silent(kFactorFromBendDeduction(definition.bendDeduction, radius, angle, thickness));
        if (kFactor != undefined && kFactorIsValid(kFactor, radius, thickness))
        {
            definition.kFactor = kFactor;
            definition.bendAllowance = 2 * setBack - definition.bendDeduction;
        }
    }
    else
    {
        // if this is the first time the enum got edited (e.g. after an upgrade) or we're editing the kFactor,
        // BA and BD should get updated to match the kFactor
        definition.bendAllowance = bendAllowanceFromKFactor(definition.kFactor, radius, angle, thickness);
        definition.bendDeduction = 2 * setBack - definition.bendAllowance;
    }

    return definition;
}

function kFactorIsValid(kFactor is number, radius is ValueWithUnits, thickness is ValueWithUnits) returns boolean
{
    return (kFactor >= (K_FACTOR_TOLERANCE * meter - radius) / thickness && kFactor <= 1);
}

