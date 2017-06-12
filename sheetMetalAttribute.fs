FeatureScript 608; /* Automatically generated version */
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
export import(path : "onshape/std/smcornerbreakstyle.gen.fs", version : "608.0");
export import(path : "onshape/std/smreliefstyle.gen.fs", version : "608.0");
export import(path : "onshape/std/smjointtype.gen.fs", version : "608.0");
export import(path : "onshape/std/smjointstyle.gen.fs", version : "608.0");
export import(path : "onshape/std/smobjecttype.gen.fs", version : "608.0");
export import(path : "onshape/std/context.fs", version : "608.0");
export import(path : "onshape/std/query.fs", version : "608.0");
import(path : "onshape/std/attributes.fs", version : "608.0");
import(path : "onshape/std/containers.fs", version : "608.0");
import(path : "onshape/std/units.fs", version : "608.0");
import(path : "onshape/std/feature.fs", version : "608.0");
import(path : "onshape/std/string.fs", version : "608.0");

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
        value.thickness == undefined || isLength(value.thickness.value);
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
            value.radius == undefined || isLength(value.radius.value);
            value.unfolded == undefined || value.unfolded is boolean;
        }
        else if (value.jointType.value == SMJointType.RIP)
        {
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
 * solid body entities.
 */
export type SMAssociationAttribute typecheck canBeSMAssociationAttribute;

/**
 * Association attribute stores attributeId. The association is established by assigning same attribute to
 * associated entities. Every entity in sheet metal master sheet body has a distinct association attribute.
 */
export predicate canBeSMAssociationAttribute (value)
{
    value is map;
    value.attributeId == undefined || value.attributeId is string;
}

/**
 * create an association attribute
 */
export function makeSMAssociationAttribute(attributeId is string) returns SMAssociationAttribute
{
    return {"attributeId" : attributeId} as SMAssociationAttribute;
}

/**
 * Assign new association attributes to entities using their transient queries to generate attribute ids.
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
    if (attribute.cornerBreaks is undefined)
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

