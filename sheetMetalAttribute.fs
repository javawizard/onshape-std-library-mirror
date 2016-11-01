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
export import(path : "onshape/std/smobjecttype.gen.fs", version : "✨");
export import(path : "onshape/std/context.fs", version : "✨");
export import(path : "onshape/std/query.fs", version : "✨");
import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/units.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");

/**
 * @internal
 */

export type SMAttribute typecheck canBeSMAttribute ;

/**
 * @internal
 */

 /* parameters in SMAttribute (e.g. radius in BEND, angle in JOINT, thickness in MODEL)
 *  are specified as maps {
 *  value : {ValueWithUnits},
 *  canBeEdited : {boolean},
 *  controllingFeatureId : {string}, : feature to be edited when editing this parameter
 *  parameterIdInFeature : {string}
 *  }
 */
export predicate canBeSMAttribute (value)
{
    value is map;
    value.attributeId == undefined || value.attributeId is string;
    value.objectType == undefined || value.objectType is SMObjectType;
    if (value.objectType == SMObjectType.MODEL)
    {
        value.thickness == undefined || isLength(value.thickness.value);
        value.minimalClearance == undefined || isLength(value.minimalClearance.value);
    }
    else if (value.objectType == SMObjectType.JOINT)
    {
        value.jointType == undefined || value.jointType.value is SMJointType;
    }
    if (value.jointType != undefined && value.jointType.value == SMJointType.BEND)
    {
        value.radius == undefined || isLength(value.radius.value);
        value.unfolded == undefined || value.unfolded is boolean;
    }

}



/**
 * @internal
 */
export const smAttributeDefault = {} as SMAttribute;

/**
 * @internal
 */
export function asSMAttribute(value is map) returns SMAttribute
{
    return value as SMAttribute;
}

/**
* @internal
*/
export function makeSMJointAttribute(attributeId is string) returns SMAttribute
{
    return asSMAttribute({'objectType' : SMObjectType.JOINT,
            'attributeId' : attributeId });
}

/**
* @internal
*/
export function makeSMWallAttribute(attributeId is string) returns SMAttribute
{
    return asSMAttribute({'objectType' : SMObjectType.WALL,
            'attributeId' : attributeId });
}

/**
* @internal
*/
export function makeSMModelAttribute(attributeId is string) returns SMAttribute
{
    return asSMAttribute({'objectType' : SMObjectType.MODEL,
            'attributeId' : attributeId });
}

/**
* @internal
*/
export function getSmObjectTypeAttributes(context is Context, topology is Query, objectType is SMObjectType) returns array
{
    return getAttributes(context, {
            "entities" : topology,
            "attributePattern" : asSMAttribute({'objectType' : objectType})
    });
}

/**
* @internal
*/
export function clearSmAttributes(context is Context, entities is Query)
{
    removeAttributes(context, {
        "entities" : entities,
        "attributePattern" : asSMAttribute({})
    });
}

/**
 * @internal
 */
export function replaceSMAttribute(context is Context, entity is Query, existingAttribute is SMAttribute, newAttribute is SMAttribute)
{
    removeAttributes(context, { "entities" : entity, "attributePattern" : existingAttribute });
    setAttribute(context, { "entities" : entity, "attribute" : newAttribute });
}


/**
 * @internal
 */
export type SMAssociationAttribute typecheck canBeSMAssociationAttribute;

/**
 * @internal
 */
export predicate canBeSMAssociationAttribute (value)
{
    value is map;
    value.attributeId == undefined || value.attributeId is string;
}

/**
 * @internal
 */
export function makeSMAssociationAttribute(attributeId is string) returns SMAssociationAttribute
{
    return {"attributeId" : attributeId} as SMAssociationAttribute;
}

/**
 * @internal
 */
export function assignSmAssociationAttributes(context is Context, entities is Query)
{
    for (var ent in evaluateQuery(context, entities))
    {
        setAttribute(context, {
                "entities" : ent,
                "attribute" : makeSMAssociationAttribute(toString(ent))
        });
    }
}

/**
 * @internal
 */
export function getSMDefinitionEntities(context is Context, selection is Query) returns array
{
    var entityAssociations = getAttributes(context, {
            "entities" : qBodyType(selection, BodyType.SOLID),
            "attributePattern" : {} as SMAssociationAttribute
        });
    var out = [];
    for (var attribute in entityAssociations)
    {
        var associatedEntities = evaluateQuery(context, qBodyType(qAttributeQuery(attribute), BodyType.SHEET));
        out = concatenateArrays([out, associatedEntities]);
    }
    return out;
}

/**
 * @internal
 */
export function isSheetMetalModelActive(context is Context, sheetMetalModel is Query) returns boolean
{
    const attributes = getSmObjectTypeAttributes(context, sheetMetalModel, SMObjectType.MODEL);
    return size(attributes) == 1 && attributes[0].active == true;
}

/**
 * @internal
 */
export function areEntitiesFromSingleSheetMetalModel(context is Context, entities is Query) returns map
{
    var result = {
        "fromSingleSheetMetalModel" : false,
        "active" : false
    };
    const partFaces = qOwnedByBody(qEntityFilter(entities, EntityType.BODY), EntityType.FACE);
    const sheetMetalEntities = getSMDefinitionEntities(context, qUnion([entities, partFaces]));
    const sheetMetalModels = qOwnerBody(qUnion(sheetMetalEntities));
    const sheetMetalModelArray = evaluateQuery(context, sheetMetalModels);

    var foundAttribute = undefined;
    for (var model in sheetMetalModelArray)
    {
        const attributes = getSmObjectTypeAttributes(context, model, SMObjectType.MODEL);
        if (size(attributes) != 1)
            throw regenError("Found model with more than one SMObjectType.MODEL attribute");
        if (foundAttribute == undefined)
            foundAttribute = attributes[0].attributeId;
        else if (foundAttribute != attributes[0].attributeId)
        {
            //found a new attribute, i.e. a different sheet metal model
            return result;
        }
    }
    if (foundAttribute != undefined)
    {
        result.fromSingleSheetMetalModel = true;
        result.active = isSheetMetalModelActive(context, sheetMetalModelArray[0]);
    }
    return result;
}

/**
 * @internal
 */
export function areEntitiesFromSingleActiveSheetMetalModel(context is Context, entities is Query) returns boolean
{
    const info = areEntitiesFromSingleSheetMetalModel(context, entities);
    return info.fromSingleSheetMetalModel && info.active;
}

/**
 * @internal
 */
export function getJointAttribute(context is Context, jointEdge is Query) returns map
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

