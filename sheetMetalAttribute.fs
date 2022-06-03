FeatureScript 1777; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/**
 * Modules prefixed with "sheetMetal" here and below control functionality related to working with
 * sheet metal models in Onshape.
 *
 * Sheet metal models are created with the [sheetMetalStart] feature. The geometry of these models
 * is not modifiable with ordinary geometry operations, and an operation which attempts to modify a
 * sheet metal model will always throw the error `ErrorStringEnum.SHEET_METAL_PARTS_PROHIBITED`
 *
 * Onshape's sheet metal operations are instead encapsulated in features defined with
 * [defineSheetMetalFeature]. These features directly modify the underlying sheet metal master
 * body, a hidden surface body not accessible from other features. The master body (along with
 * [SMAttribute]s set on its entities) provides the information necessary for
 * [updateSheetMetalGeometry] to build both sheet metal bodies: the 3D folded body and the
 * flat pattern. The result is simultaneous sheet metal editing, where geometry and errors are
 * always available to the end user on both the folded and the flat sheet metal models.
 *
 * Most custom features will function only on bodies which are not active sheet metal, because the
 * feature's effects are not readily translatable from the folded model back to the flattened
 * master model. A custom feature's user will typically discover this when an operation within the
 * custom feature throws a `SHEET_METAL_PARTS_PROHIBITED` error, giving the user-facing message
 * "Active sheet metal models are not allowed."
 *
 * Additionally, a custom feature's query parameter can disallow selection of entities from
 * sheet metal bodies using the [ActiveSheetMetal.NO](ActiveSheetMetal) filter. Any other query can
 * be filtered for non-sheet-metal geometry using [separateSheetMetalQueries].
 */
export import(path : "onshape/std/context.fs", version : "1777.0");
export import(path : "onshape/std/query.fs", version : "1777.0");
export import(path : "onshape/std/smbendtype.gen.fs", version : "1777.0");
export import(path : "onshape/std/smcornerbreakstyle.gen.fs", version : "1777.0");
export import(path : "onshape/std/smjointstyle.gen.fs", version : "1777.0");
export import(path : "onshape/std/smjointtype.gen.fs", version : "1777.0");
export import(path : "onshape/std/smobjecttype.gen.fs", version : "1777.0");
export import(path : "onshape/std/smreliefstyle.gen.fs", version : "1777.0");

import(path : "onshape/std/attributes.fs", version : "1777.0");
import(path : "onshape/std/containers.fs", version : "1777.0");
import(path : "onshape/std/units.fs", version : "1777.0");
import(path : "onshape/std/feature.fs", version : "1777.0");
import(path : "onshape/std/string.fs", version : "1777.0");

/**
 * Sheet metal object definition attribute type.
 */
export type SMAttribute typecheck canBeSMAttribute;

/**
 *  parameters in SMAttribute (e.g. radius in BEND, angle in JOINT, thickness in MODEL)
 *  are specified as maps @eg ```{
 *  value : {ValueWithUnits},
 *  canBeEdited : {boolean},
 *  controllingFeatureId : {string}, : feature to be edited when editing this parameter
 *  parameterIdInFeature : {string}
 *  }```
 */
export predicate canBeSMAttribute (value)
{
    value is map;
    value.attributeId == undefined || value.attributeId is string;
    value.objectType == undefined || value.objectType is SMObjectType;
    if (value.objectType == SMObjectType.MODEL)
    {
        value.frontThickness == undefined || isLength(value.frontThickness.value);
        value.backThickness == undefined || isLength(value.backThickness.value);
        value.minimalClearance == undefined || isLength(value.minimalClearance.value);
    }
    else if (value.objectType == SMObjectType.JOINT)
    {
        value.jointType == undefined || value.jointType.value is SMJointType;
    }
    else if (value.objectType == SMObjectType.CORNER)
    {
        value.cornerStyle == undefined || value.cornerStyle.value is SMReliefStyle;
        value.cornerReliefScale == undefined || value.cornerReliefScale.value is number;
        value.bendReliefScale == undefined || value.bendReliefScale.value is number;

        value.cornerBreaks == undefined || value.cornerBreaks is array;
        if (value.cornerBreaks != undefined)
        {
            for (var cornerBreakEntry in value.cornerBreaks)
            {
                cornerBreakEntry.value is SMCornerBreak;
            }
        }
    }
    if (value.jointType != undefined)
    {
        if (value.jointType.value == SMJointType.BEND)
        {
            value.bendType == undefined || value.bendType.value is SMBendType;
            value.radius == undefined || isLength(value.radius.value);
            value.angle == undefined || isAngle(value.angle.value);
            // This parameter does not follow the `value`/`canBeEdited` pattern.  It is just a raw boolean.
            value.unfolded == undefined || value.unfolded is boolean;
        }
        else if (value.jointType.value == SMJointType.RIP)
        {
            value.jointStyle == undefined || value.jointStyle.value is SMJointStyle;
            value.angle == undefined || isAngle(value.angle.value);
            value.minimalClearance == undefined || isLength(value.minimalClearance.value);
        }
    }
}

/**
 * Empty map as SMAttribute convenient for attribute lookup
 */
export const smAttributeDefault = {} as SMAttribute;

/**
 * Attach SMAttribute type to a map. convenient for attribute lookup and queries.
 */
export function asSMAttribute(value is map) returns SMAttribute
{
    return value as SMAttribute;
}

/**
* Start SMAttribute for joint.
*/
export function makeSMJointAttribute(attributeId is string) returns SMAttribute
{
    return asSMAttribute({'objectType' : SMObjectType.JOINT,
            'attributeId' : attributeId });
}

/**
* Start SMAttribute for wall.
*/
export function makeSMWallAttribute(attributeId is string) returns SMAttribute
{
    return asSMAttribute({'objectType' : SMObjectType.WALL,
            'attributeId' : attributeId });
}

/**
* Start SMAttribute for corner.
*/
export function makeSMCornerAttribute(attributeId is string) returns SMAttribute
{
    return asSMAttribute({ 'objectType' : SMObjectType.CORNER,
                'attributeId' : attributeId });
}

/**
* Get attributes with matching objectType.
*/
export function getSmObjectTypeAttributes(context is Context, topology is Query, objectType is SMObjectType) returns array
{
    return getAttributes(context, {
            "entities" : topology,
            "attributePattern" : asSMAttribute({'objectType' : objectType})
    });
}

/**
* Clear SM attributes from entities.
*/
export function clearSmAttributes(context is Context, entities is Query)
{
    removeAttributes(context, {
        "entities" : entities,
        "attributePattern" : asSMAttribute({})
    });
}

/**
 * Check for presence of SMAttribute types.
 */
export function hasSheetMetalAttribute(context is Context, entities is Query, objectType is SMObjectType) returns boolean
{
    return size(getSmObjectTypeAttributes(context, entities, objectType)) != 0;
}

/**
 * For all entities annotated with attribute matching existingAttribute pattern, replace it with newAttribute.
 * Return query for entities whose attributes have been modified.
 */
export function replaceSMAttribute(context is Context, existingAttribute is SMAttribute, newAttribute is SMAttribute) returns Query
{
    // Have to evaluate attribute query before removing the attribute
    var entities = qUnion(evaluateQuery(context, qAttributeQuery(existingAttribute)));
    removeAttributes(context, { "entities" : entities, "attributePattern" : existingAttribute });
    setAttribute(context, { "entities" : entities, "attribute" : newAttribute });
    return entities;
}

/**
 * Find sheet metal master body entities corresponding to feature input.
 */
export function getSMDefinitionEntities(context is Context, selection is Query) returns array
{
    // Before this version, this overload always assumed we were inside sheet metal scope.
    const assumeInsideScope = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V1062_GET_SM_ENTS);
    if (assumeInsideScope || isInSheetMetalFeature(context))
    {
        return getSMDefinitionEntitiesInsideSheetMetalFeature(context, selection, undefined);
    }
    else
    {
        return getSMDefinitionEntitiesOutsideSheetMetalFeature(context, selection, undefined);
    }
}

export function getSMDefinitionEntities(context is Context, selection is Query, entityType is EntityType) returns array
{
    // Before this version, this overload always assumed we were outside sheet metal scope.
    const assumeOutsideScope = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V1062_GET_SM_ENTS);
    if (assumeOutsideScope || !isInSheetMetalFeature(context))
    {
        return getSMDefinitionEntitiesOutsideSheetMetalFeature(context, selection, entityType);
    }
    else
    {
        return getSMDefinitionEntitiesInsideSheetMetalFeature(context, selection, entityType);
    }
}

// If entityType is undefined, disregard it.
function getSMDefinitionEntitiesInsideSheetMetalFeature(context is Context, selection is Query, entityType) returns array
{
    const entityAssociations = getSMAssociationAttributes(context, qBodyType(selection, BodyType.SOLID));

    var attributeQueries = [];
    for (var attribute in entityAssociations)
    {
        attributeQueries = append(attributeQueries, qAttributeQuery(attribute));
    }
    var associatedEntities = qUnion(attributeQueries);
    if (entityType != undefined)
    {
        associatedEntities = qEntityFilter(associatedEntities, entityType);
    }
    return evaluateQuery(context, qBodyType(associatedEntities, BodyType.SHEET));
}

// If entityType is undefined, disregard it.
function getSMDefinitionEntitiesOutsideSheetMetalFeature(context is Context, selection is Query, entityType) returns array
{
    const entityAssociations = try silent(getSMAssociationAttributes(context, qBodyType(selection, BodyType.SOLID)));
    if (entityAssociations == undefined)
    {
        return [];
    }

    const allSheets = qAttributeQuery(asSMAttribute({ "objectType" : SMObjectType.MODEL }));
    const allSMDefinitionEntitiesOfType = (entityType != undefined) ? qOwnedByBody(allSheets, entityType) : qOwnedByBody(allSheets);

    const returnInactive = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V495_MOVE_FACE_ROTATION_AXIS);
    const useSpecificOwnerBody = isAtVersionOrLater(context, FeatureScriptVersionNumber.V522_MOVE_FACE_NONPLANAR);

    var allSheetsConsideredActive;
    if (!useSpecificOwnerBody)
    {
        allSheetsConsideredActive = try silent(isSheetMetalModelActive(context, allSheets));
    }

    var out = [];
    for (var attribute in entityAssociations)
    {
        const associatedEntities = evaluateQuery(context, qIntersection([qAttributeQuery(attribute), allSMDefinitionEntitiesOfType]));
        const ownerBody = qOwnerBody(qUnion(associatedEntities));
        if (returnInactive || useSpecificOwnerBody ? try silent(isSheetMetalModelActive(context, ownerBody)) == true : allSheetsConsideredActive == true)
        {
            out = append(out, associatedEntities);
        }
    }
    return concatenateArrays(out);
}

/**
 * Check if sheet metal model is active.
 */
export function isSheetMetalModelActive(context is Context, sheetMetalModel is Query) returns boolean
{
    const attributes = getSmObjectTypeAttributes(context, sheetMetalModel, SMObjectType.MODEL);
    return size(attributes) == 1 && attributes[0].active == true;
}

/**
 * Check if all entities belong to the same sheet metal model
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
 * Check if all entities belong to the same active sheet metal model
 */
export function areEntitiesFromSingleActiveSheetMetalModel(context is Context, entities is Query) returns boolean
{
    const info = areEntitiesFromSingleSheetMetalModel(context, entities);
    return info.fromSingleSheetMetalModel && info.active;
}

/**
 * Get wall attribute on a single entity
 */
export function getWallAttribute(context is Context, wallFace is Query)
{
    var attributes = getSmObjectTypeAttributes(context, wallFace, SMObjectType.WALL);
    if (size(attributes) != 1)
    {
        return undefined;
    }
    else
    {
        return attributes[0];
    }
}

/**
 * Get joint attribute on a single entity
 */
export function getJointAttribute(context is Context, jointEdge is Query)
{
    var attributes = getSmObjectTypeAttributes(context, jointEdge, SMObjectType.JOINT);
    if (size(attributes) != 1)
    {
        return undefined;
    }
    else
    {
        return attributes[0];
    }
}

/**
 * Get corner attribute on a single entity
 */
export function getCornerAttribute(context is Context, cornerVertex is Query)
{
    var attributes = getSmObjectTypeAttributes(context, cornerVertex, SMObjectType.CORNER);
    if (size(attributes) != 1)
    {
        return undefined;
    }
    else
    {
        return attributes[0];
    }
}



/**
 * Used by sheet metal features to maintain correspondence between master sheet body entities and
 * folded and flat solid body entities.
 */
export type SMAssociationAttribute typecheck canBeSMAssociationAttribute;

/**
 * Association attribute stores `attributeId`. The association is established by assigning the same attribute to
 * associated entities. Every entity in sheet metal master sheet body has a distinct association attribute.
 */
export predicate canBeSMAssociationAttribute (value)
{
    value is map;
    value.attributeId == undefined || value.attributeId is string;
}

/**
 * Create an association attribute with the given `attributeId`.
 */
export function makeSMAssociationAttribute(attributeId is string) returns SMAssociationAttribute
{
    return {"attributeId" : attributeId} as SMAssociationAttribute;
}

/**
 * Assign new association attributes to entities using their transient queries to generate attribute ids.
 */
export function assignSMAssociationAttributes(context is Context, entities is Query)
{
    for (var ent in evaluateQuery(context, entities))
    {
        setAttribute(context, {
                "entities" : ent,
                "attribute" : makeSMAssociationAttribute(toString(ent))
        });
    }
}

/** @internal Fixed for correct capitalization */
annotation { "Deprecated" : "Use [assignSMAssociationAttributes]" }
export function assignSmAssociationAttributes(context is Context, entities is Query)
{
    assignSMAssociationAttributes(context, entities);
}

/**
 * An attribute pattern for finding attributes which are [SMAssociationAttribute]s.
 */
export const smAssociationAttributePattern = {} as SMAssociationAttribute;

/**
 * Get all of the association attributes for a given set of `entities`.
 */
export function getSMAssociationAttributes(context is Context, entities is Query)
{
    return getAttributes(context, {
                "entities" : entities,
                "attributePattern" : smAssociationAttributePattern
            });
}

/**
 * Information corresponding to a single sheet metal corner break (fillet or chamfer)
 */
export type SMCornerBreak typecheck canBeSMCornerBreak;

/**
 * Corner break must hold the break style, the range (radius and distance of fillet and chamfer respectively), and the
 * wallId of the wall that owns the corner.
 */
export predicate canBeSMCornerBreak(value)
{
    value is map;
    value.cornerBreakStyle is SMCornerBreakStyle;
    value.range is ValueWithUnits;
    value.wallId is string;
}

/**
 * Create a corner break
 */
export function makeSMCornerBreak(cornerBreakStyle is SMCornerBreakStyle, range is ValueWithUnits, wallId is string) returns SMCornerBreak
{
    return { "cornerBreakStyle" : cornerBreakStyle, "range" : range, "wallId" : wallId } as SMCornerBreak;
}

/**
 * Adds an SMCornerBreak and any additional information to an SMAttribute, initializing the cornerBreaks array if necessary.
 */
export function addCornerBreakToSMAttribute(attribute is SMAttribute, cornerBreakMap is map) returns SMAttribute
precondition
{
    cornerBreakMap.value is SMCornerBreak;
}
{
    if (attribute.cornerBreaks == undefined)
    {
        attribute.cornerBreaks = [];
    }
    attribute.cornerBreaks = append(attribute.cornerBreaks, cornerBreakMap);
    return asSMAttribute(attribute);
}

/**
 * Finds an SMCornerBreak in attribute corresponding to wallId, returns undefined if nothing found
 */
export function findCornerBreak(attribute is SMAttribute, wallId is string)
{
    if (attribute.cornerBreaks == undefined)
    {
        return undefined;
    }
    for (var cBreak in attribute.cornerBreaks)
    {
        if (cBreak.value.wallId == wallId)
        {
            return cBreak;
        }
    }
    return undefined;
}

/**
* Clears existing SMAttribute, sets new one only if non-trivial
*/
export function updateCornerAttribute(context is Context, vertex is Query, attribute is SMAttribute)
precondition
{
    attribute.objectType == SMObjectType.CORNER;
}
{
    removeAttributes(context, {"entities" : vertex,
                               "attributePattern" : asSMAttribute({})
                });
    if (size(attribute.cornerBreaks) == 0)
    {
       attribute.cornerBreaks = undefined;
    }
    if (attribute.cornerStyle != undefined ||
        attribute.cornerBreaks != undefined)
    {
        setAttribute(context, {
                "entities" : vertex,
                "attribute" : attribute
        });
    }
}

/**
 * Removes all controllingFeatureId and parameterIdInFeature information from an SMAttribute. Replaces the provided
 * attribute with sanitized attribute if `replaceExisting` is true. Return the sanitized attribute
 *
 * Fail if the attribute is a model attribute as a safety precaution, as removing the controllingFeatureId information
 * from a model attribute would invalidate it from being the same model attribute as it was before
 */
export function sanitizeControllingInformation(context is Context, attribute is SMAttribute, replaceExisting is boolean) returns SMAttribute
precondition
{
    attribute.objectType is SMObjectType;
    attribute.objectType != SMObjectType.MODEL;
}
{
    var newAttribute = clearControllingInformation(attribute);
    if ((attribute != newAttribute) && replaceExisting)
        replaceSMAttribute(context, attribute, newAttribute);

    return newAttribute;
}

function clearControllingInformation(m is map) returns map
{
    m.controllingFeatureId = undefined;
    m.parameterIdInFeature = undefined;
    for (var key in keys(m))
    {
        if (m[key] is map || m[key] is array)
        {
            m[key] = clearControllingInformation(m[key]);
        }
    }
    return m;
}

function clearControllingInformation(arr is array) returns array
{
    for (var i = 0; i < size(arr); i += 1)
    {
        if (arr[i] is map || arr[i] is array)
        {
            arr[i] = clearControllingInformation(arr[i]);
        }
    }
    return arr;
}

