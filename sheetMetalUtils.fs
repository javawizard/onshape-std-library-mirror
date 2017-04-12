FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "✨");
import(path : "onshape/std/boundingtype.gen.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/coordSystem.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");
import(path : "onshape/std/manipulator.fs", version : "✨");
import(path : "onshape/std/query.fs", version : "✨");
import(path : "onshape/std/sketch.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/smobjecttype.gen.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/tool.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");
import(path : "onshape/std/topologyUtils.fs", version : "✨");
import(path : "onshape/std/transform.fs", version : "✨");



/**
 * Exposes sheet metal definition sheet body to the queries within feature.
 */
export function defineSheetMetalFeature(feature is function, defaults is map) returns function
{
    defaults.isSheetMetal = true;
    return defineFeature(feature, defaults);
}


/**
 * Based on current state of sheet metal definition sheet body update solid bodies
 * @param args {{
 *      @field entities{Query} : sheet metal definition entities changed (or attributes changed) in this feature
 *      @field deletedAttributes{array} : associated attributes of deleted sheet metal definition entities
 *      @field associatedChanges{Query} : sheet metal definition entities representing the change of this feature
 * }}
 */
 export function updateSheetMetalGeometry(context is Context, id is Id, args is map)
 {
    @updateSheetMetalGeometry(context, id, args);
 }


/**
* Assign SMAttributes to topology of sheet metal definition sheet body
* @param args {{
*       @field surfaceBodies{Query}
*       @field bendEdges{Query}
*       @field specialRadiiBends{array} : array of pairs "(edge, bendRadius)"
*       @field defaultRadius{ValueWithUnits} : bend radius to be applied to bendEdges
*       @field controlsThickness{boolean}
*       @field thickness{ValueWithUnits}
*       @field defaultCornerReliefScale{number}
*       @field defaultBendReliefScale{number}
* }}
*/
export function annotateSmSurfaceBodies(context is Context, id is Id, args is map, objectCount is number) returns number
{
    var surfaceBodies = evaluateQuery(context, args.surfaceBodies);
    if (size(surfaceBodies) == 0)
    {
        return 0;
    }
    var featureIdString = toAttributeId(id);
    var thicknessData = {"value" : args.thickness, "canBeEdited" : args.controlsThickness};
    if (args.controlsThickness)
    {
        thicknessData.controllingFeatureId = featureIdString;
        thicknessData.parameterIdInFeature = "thickness";
    }
    var kFactorData = {"value" : args.kFactor,
        "canBeEdited" : true,
        "controllingFeatureId" : featureIdString,
        "parameterIdInFeature" : "kFactor"
        };
    var minimalClearanceData = {"value" : args.minimalClearance,
        "canBeEdited" : true,
        "controllingFeatureId" : featureIdString,
        "parameterIdInFeature" : "minimalClearance"
        };
    var defaultCornerReliefScale = { "value" : args.defaultCornerReliefScale,
        "canBeEdited" : true,
        "controllingFeatureId" : featureIdString,
        "parameterIdInFeature" : "defaultCornerReliefScale"
        };
    var defaultBendReliefScale = { "value" : args.defaultBendReliefScale,
        "canBeEdited" : true,
        "controllingFeatureId" : featureIdString,
        "parameterIdInFeature" : "defaultBendReliefScale"
        };
    var defaultBendReliefDepthScale = { "value" : args.defaultBendReliefDepthScale,
        "canBeEdited" : true,
        "controllingFeatureId" : featureIdString,
        "parameterIdInFeature" : "defaultBendReliefDepthScale"
        };

    var modelAttribute = asSMAttribute({"attributeId" : featureIdString,
                    "objectType" : SMObjectType.MODEL,
                    "active" : true,
                    "thickness" : thicknessData,
                    "k-factor" : kFactorData,
                    "minimalClearance" : minimalClearanceData,
                    "defaultBendRadius" : {"value" : args.defaultRadius},
                    "defaultCornerReliefScale" : defaultCornerReliefScale,
                    "defaultBendReliefDepthScale" : defaultBendReliefDepthScale,
                    "defaultBendReliefScale" : defaultBendReliefScale,
                    "fsVersion" : getCurrentVersion(context)});
    if (args.defaultTwoCornerStyle != undefined)
    {
        modelAttribute.defaultTwoCornerStyle = args.defaultTwoCornerStyle;
    }
    if (args.defaultThreeCornerStyle != undefined)
    {
        modelAttribute.defaultThreeCornerStyle = args.defaultThreeCornerStyle;
    }
    if (args.defaultBendReliefStyle != undefined)
    {
        modelAttribute.defaultBendReliefStyle = args.defaultBendReliefStyle;
    }

    var facesQ =  qOwnedByBody(args.surfaceBodies, EntityType.FACE);
    var count = objectCount;
    for (var face in evaluateQuery(context, facesQ))
    {
        var surface = evSurfaceDefinition(context, {
                "face" : face
        });
        if (surface is Plane)
        {
            setAttribute(context, {
                    "entities" : face,
                    "attribute" : makeSMWallAttribute(toAttributeId(id + count))
            });
            count += 1;
        }
        else if (surface is Cylinder)
        {
            var bendAttribute = makeSMJointAttribute(toAttributeId(id + count));
            bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited": false };

            var bendRadius = surface.radius - 0.5 * args.thickness;
            bendAttribute.radius = { "value" : bendRadius, "canBeEdited" : false, "isDefault" : false};
            setAttribute(context, {
                    "entities" : face,
                    "attribute" : bendAttribute
            });
            count += 1;
        }
        else
        {
            regenError("Only planar walls are supported");
        }
    }
    var bendMap = {};
    for (var edge in evaluateQuery(context, args.bendEdges))
    {
        bendMap[edge] = true;
    }
    for (var edgeAndRadius in args.specialRadiiBends)
    {
        bendMap[edgeAndRadius[0]] = edgeAndRadius[1];
    }
    var edgesQ = qOwnedByBody(args.surfaceBodies, EntityType.EDGE);
    for (var edge in evaluateQuery(context, edgesQ))
    {
        var faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
        if (size(faces) != 2)
        {
            continue; // TODO : warning if edge is selected as a bend
        }
        if (size(getSmObjectTypeAttributes(context, qUnion(faces), SMObjectType.WALL)) != 2)
        {
            continue;
        }
        var bendRadius = bendMap[edge];
        var angleVal = try(edgeAngle(context, edge));
        var zeroAngle = angleVal == undefined || angleVal < TOLERANCE.zeroAngle * radian;
        if (bendRadius != undefined)
        {
            if (zeroAngle)
            {
                setErrorEntities(context, id, {"entities" : edge});
                reportFeatureError(context, id, ErrorStringEnum.SHEET_METAL_NO_0_ANGLE_BEND);
                return 0;
            }
            var bendAttribute = makeSMJointAttribute(toAttributeId(id + count));
            bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited": true };
            if (bendRadius == true)
            {
                bendRadius = args.defaultRadius;
            }
            bendAttribute.radius = { "value" : bendRadius, "canBeEdited" : true, "isDefault" : true};
            bendAttribute.angle = {"value" : angleVal, "canBeEdited" : false};
            setAttribute(context, {
                    "entities" : edge,
                    "attribute" : bendAttribute
            });
            count += 1;
        }
        else
        {
            var ripAttribute = makeSMJointAttribute(toAttributeId(id + count));
            ripAttribute.jointType = { "value" : SMJointType.RIP, "canBeEdited": true };
            if (angleVal != undefined)
            {
                ripAttribute.angle = {"value" : angleVal, "canBeEdited" : false};
            }
            if (!zeroAngle)
            {
                ripAttribute.jointStyle = { "value" : SMJointStyle.EDGE, "canBeEdited": true };
            }
            setAttribute(context, {
                    "entities" : edge,
                    "attribute" : ripAttribute
            });
        }
        count += 1;
    }
    for (var body in surfaceBodies)
    {
        setAttribute(context, {
                "entities" : body,
                "attribute" : modelAttribute
        });
    }
    var verticesQ = qOwnedByBody(args.surfaceBodies, EntityType.VERTEX);
    assignSmAssociationAttributes(context, qUnion([args.surfaceBodies, facesQ, edgesQ, verticesQ]));
    return count;
}

/**
 * For an edge between two planes computes angle between plane normals.
 */
export function edgeAngle(context is Context, edge is Query) returns ValueWithUnits
{
    var faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
    if (size(faces) != 2)
    {
        throw "Expects 2-sided faces";
    }
    var plane0 = evPlane(context, {
            "face" : faces[0]
    });
    var plane1 = evPlane(context, {
            "face" : faces[1]
    });
    return angleBetween(plane0.normal, plane1.normal);
}

/**
 * @internal
 */
export function updateJointAngle(context is Context, edges is Query)
{
    for (var edge in evaluateQuery(context, edges))
    {
        const jointAttribute = try silent(getJointAttribute(context, edge));
        if (jointAttribute == undefined || jointAttribute.angle == undefined)
        {
            continue;
        }

        var angleVal = try silent(edgeAngle(context, edge));
        if (angleVal == undefined || tolerantEquals(angleVal, jointAttribute.angle.value))
        {
            continue;
        }

        var replacementAttribute = jointAttribute;

        if (abs(angleVal) < TOLERANCE.zeroAngle * radian)
        {
            replacementAttribute.jointStyle = undefined;
            replacementAttribute.jointType = { "value" : SMJointType.RIP, "canBeEdited" : false };
        }
        else if (replacementAttribute.jointType != undefined && replacementAttribute.jointType.value == SMJointType.RIP)
        {
            replacementAttribute.jointStyle = { "value" : SMJointStyle.EDGE, "canBeEdited" : true };
            replacementAttribute.jointType.canBeEdited = true;
        }

        replacementAttribute.angle = { "value" : angleVal, "canBeEdited" : jointAttribute.angle.canBeEdited };
        replaceSMAttribute(context, jointAttribute, replacementAttribute);
    }
}

/**
 * A `RealBoundSpec` for sheet metal K-factor between 0. and 1., defaulting to `.45`.
 */
export const K_FACTOR_BOUNDS =
{
    (unitless) : [0, 0.45, 1]
} as RealBoundSpec;


/**
 * A `LengthBoundSpec` for minimal clearance of sheet  metal rips
 */
export const SM_MINIMAL_CLEARANCE_BOUNDS =
{
    (meter)      : [2e-5, 2e-5, 1],
    (centimeter) : 2e-3,
    (millimeter) : 0.02,
    (inch)       : 1e-3,
    (foot)       : 1e-4,
    (yard)       : 2.2e-5
} as LengthBoundSpec;


/**
 * A `LengthBoundSpec` for bend radius in sheet metal features
 */
export const SM_BEND_RADIUS_BOUNDS =
{
    (meter)      : [1e-5, 0.0023 , 500],
    (centimeter) : 0.23,
    (millimeter) : 2.3,
    (inch)       : 0.09,
    (foot)       : 0.0075,
    (yard)       : 0.0025
} as LengthBoundSpec;

/**
 * A `LengthBoundSpec` for thickness in sheet metal features. default to `(1/16)"` (i.e. steel)
 */
export const SM_THICKNESS_BOUNDS =
{
    (meter)      : [1e-5, 0.0016, 500],
    (centimeter) : 0.16,
    (millimeter) : 1.6,
    (inch)       : 0.0625,
    (foot)       : 0.005,
    (yard)       : 0.002
} as LengthBoundSpec;



/**
 * Partitions allParts into non-sheet metal parts and sheet metal parts.
 * To preserve existing behavior of code the returned non-sm query is exactly the same as what is passed in
 * for non-sm cases and a query is returned for them.
 * The sheet metal results will usually be iterated through and so are returned as a map with
 * the keys being the sheet metal ID and the values being the parts associated with that model.
 */
export function partitionSheetMetalParts(context is Context, allParts is Query)
{
    if (!queryContainsActiveSheetMetal(context, allParts))
    {
        // Don't mess with the query, for performance and legacy reasons
        return { "nonSheetMetalPartsQuery" : allParts, "sheetMetalPartsMap" : {} };
    }
    var parts = evaluateQuery(context, allParts);
    var nonSheetMetal = [];
    var sheetMetal = {};
    for (var part in parts)
    {
        if (isActiveSheetMetalPart(context, part))
        {
            const sheetMetalId = getActiveSheetMetalId(context, part);
            var parts = try(sheetMetal[sheetMetalId]);
            if (parts == undefined) {
                sheetMetal[sheetMetalId] = [part];
            } else {
                sheetMetal[sheetMetalId] = append(parts, part);
            }
        }
        else
        {
            nonSheetMetal = append(nonSheetMetal, part);
        }
    }
    return { "nonSheetMetalPartsQuery" : qUnion(nonSheetMetal), "sheetMetalPartsMap" : sheetMetal };
}

/**
 * Get the first id of active sheet metal model the entities of query belong to.
 */
export function getActiveSheetMetalId(context is Context, query is Query)
{
    const partQuery = qOwnerBody(query);
    if (size(evaluateQuery(context, partQuery)) == 0)
    {
        return undefined;
    }
    const attributes = getAttributes(context, {
                                    "entities" : partQuery,
                                    "attributePattern" : {} as SMAssociationAttribute
                                    });
    if (size(attributes) == 0)
    {
        return undefined;
    }
    for (var attribute in attributes)
    {
        const modelAttributes = getSmObjectTypeAttributes(context, qAttributeQuery(attribute), SMObjectType.MODEL);
        if (size(modelAttributes) == 1 && modelAttributes[0].active == true)
        {
            return modelAttributes[0].attributeId;
        }
    }
    return undefined;
}

/**
 * @internal
 */
export function queryContainsActiveSheetMetal(context is Context, query is Query) returns boolean
{
    return getActiveSheetMetalId(context, query) != undefined;
}

/**
 * @internal
 */
export function getSheetMetalModelForPart(context is Context, partQuery is Query) returns Query
{
    const definitionEntities = getSMDefinitionEntities(context, partQuery);
    return qUnion(definitionEntities);
}

/**
 * @internal
 */
export function isActiveSheetMetalPart(context is Context, partQuery is Query) returns boolean
{
    var entityAssociations = getAttributes(context, {
            "entities" : partQuery,
            "attributePattern" : {} as SMAssociationAttribute
        });
    if (size(entityAssociations) != 1)
    {
        return false;
    }
    // The intermediate attribute query will get back more entities than just the model but the NEXT query will only
    // get back the model attributes
    const attributes = getSmObjectTypeAttributes(context, qAttributeQuery(entityAssociations[0]), SMObjectType.MODEL);
    if (size(attributes) != 1)
    {
        return false;
    }
    else
    {
        return attributes[0].active == true;
    }
}

/**
 * @internal
 * originalEntities is an array of queries that contains all the entities before changes were made to the model
 *    effectively it is the result of an earlier call to evaluateQuery(context, qOwnedByBody(sheetMetalModel))
 */
export function assignSMAttributesToNewOrSplitEntities(context is Context, sheetMetalModels is Query,
                                            originalEntities is array, originalAttributes is array) returns map
{
    //Transient queries new to sheet metal body
    var entitiesToAddAssociations = filter(evaluateQuery(context, qOwnedByBody(sheetMetalModels)),
                    function(entry)
                    {
                        return !isIn(entry, originalEntities);
                    });
    var entitiesToAddAssociationsQ = qUnion(entitiesToAddAssociations);

    const attributes = getAttributes(context, { "entities" : entitiesToAddAssociationsQ,
                "attributePattern" : {} as SMAssociationAttribute });
    for (var attribute in attributes)
    {
        // If we find now that we have multiple entities with an association attribute then something like a wall
        // or bend has been split into multiple
        // Those entities need to get a new association attribute.
        // However, we need to propagate any bend/wall information and so we will now take a look to
        // see which attributes need copying and copy all the data, but with a new ID
        const entitiesWithAttribute = qAttributeFilter(qOwnedByBody(sheetMetalModels), attribute);
        const existingEntitiesWithAttribute = qSubtraction(entitiesWithAttribute, entitiesToAddAssociationsQ);
        const newEntitiesWithAttribute = qSubtraction(entitiesWithAttribute, existingEntitiesWithAttribute);

        // First case: There is an existing entity that still has the attribute (masterEntities).
        var masterEntities = existingEntitiesWithAttribute;
        var entitiesToModify = newEntitiesWithAttribute;
        var nMaster = size(evaluateQuery(context, masterEntities));
        if (nMaster == 0)
        {
            // Second case: There are no existing entities that have the attribute
            // But there may be multiple new entities with the same attribute
            // Let the 'master' be the first of the new entities. It might be
            // reassigned below to the first one keeping the definition attribute
            masterEntities = qNthElement(newEntitiesWithAttribute, 0);
            entitiesToModify = qSubtraction(newEntitiesWithAttribute, masterEntities);
        }
        else if (nMaster > 1)
        {
            throw "Can't have multiple master entities";
        }

        const definitionAttributes = getAttributes(context, { "entities" : masterEntities, "attributePattern" : {} as SMAttribute });
        var nDefAttributes = size(definitionAttributes);
        if (nDefAttributes > 1)
        {
            throw "Entities with same association attribute can't have different definition attributes";
        }

        if (nDefAttributes == 1)
        {
            // qUnion does not re-order, so master entity will be considered first
            var evaluatedEntitiesToUpdateSmAttribute = evaluateQuery(context, qUnion([masterEntities, entitiesToModify]));
            var attributeSurvived = false;
            for (var count = 0; count < size(evaluatedEntitiesToUpdateSmAttribute); count += 1)
            {
                var entity = evaluatedEntitiesToUpdateSmAttribute[count];
                // Remove definition attribute from entities which are not fit to have it
                if (!isEntityAppropriateForAttribute(context, entity, definitionAttributes[0]))
                {
                    if (count == 0 && nMaster > 0)
                    {
                        throw "Existing entity not fit for definition attribute";
                    }
                    removeAttributes(context, { "entities" : entity, "attributePattern" : {} as SMAttribute });
                }
                else if (!attributeSurvived) // favore one of the entities inheriting the attribute to be a masterEntity
                {
                    masterEntities = entity;
                    attributeSurvived = true;
                }
            }
            entitiesToModify = qSubtraction(newEntitiesWithAttribute, masterEntities);
        }

        // Clean up association attributes off of the entities that need new attributes
        removeAttributes(context, { "entities" : entitiesToModify, "attributePattern" : attribute });
    }

    //After propagating attributes with split entities we can look to see what is left, i.e. entities
    // with NO attribute and add completely new attributes to those
    var entitiesWithExistingAttributesQ = qAttributeFilter(entitiesToAddAssociationsQ, {} as SMAssociationAttribute);

    var entitiesThatNeedAssociationQ = qSubtraction(entitiesToAddAssociationsQ, entitiesWithExistingAttributesQ);
    assignSmAssociationAttributes(context, entitiesThatNeedAssociationQ);

    var finalAssociationAttributes = getAttributes(context, {
            "entities" : qOwnedByBody(sheetMetalModels), // counting on body transient query surviving
            "attributePattern" : {} as SMAssociationAttribute
    });
    var deletedAttributes = filter(originalAttributes, function(attribute){ return !isIn(attribute, finalAssociationAttributes);});
    return { "modifiedEntities" : entitiesToAddAssociationsQ, "deletedAttributes" : deletedAttributes};
}

function isEntityAppropriateForAttribute(context is Context, entity is Query, attribute is SMAttribute) returns boolean
{
    var filteredQ;
    if (attribute.objectType == SMObjectType.MODEL)
        filteredQ = qEntityFilter(entity, EntityType.BODY);
    else if (attribute.objectType == SMObjectType.JOINT)
    {
        filteredQ = qEntityFilter(entity, EntityType.EDGE);
        if (attribute.jointType.value == SMJointType.BEND)
        {
            if (size(evaluateQuery(context, qGeometry(entity, GeometryType.CYLINDER))) == 1)
            {
                //TODO : check tangent to walls around
                return true;
            }
            if (size(evaluateQuery(context, qGeometry(entity, GeometryType.LINE))) != 1)
            {
                return false;
            }
        }
        return edgeIsTwoSided(context, filteredQ);
    }
    else if (attribute.objectType == SMObjectType.WALL)
    {
        filteredQ = qEntityFilter(entity, EntityType.FACE);
    }
    else if (attribute.objectType == SMObjectType.CORNER)
    {
        filteredQ = qEntityFilter(entity, EntityType.VERTEX);
    }
    else
    {
        throw ("Unhandled SMObjectType" ~ attribute.objectType);
    }
    if (size(evaluateQuery(context, filteredQ)) != 1)
    {
        return false;
    }
    return true;
}

/**
 * Extract sheet metal model parameters in a convenient form
 */
export function getModelParameters(context is Context, model is Query) returns map
{
    var attr = getAttributes(context, {"entities" : model, "attributePattern" : asSMAttribute({})});

    if (size(attr) != 1 || attr[0].thickness == undefined || attr[0].thickness.value == undefined ||
        attr[0].minimalClearance == undefined || attr[0].minimalClearance.value == undefined ||
        attr[0].defaultBendRadius == undefined || attr[0].defaultBendRadius.value == undefined)
    {
        throw "Could not get sheet metal attribute.";
    }
    return {"thickness" : attr[0].thickness.value, "minimalClearance" : attr[0].minimalClearance.value, "defaultBendRadius" : attr[0].defaultBendRadius.value};
}

/**
 * @internal
 */
export function separateSheetMetalQueries(context is Context, targets is Query) returns map
{
    var sheetMetalEntities = [];
    var nonSheetMetalEntities = [];
    for (var entity in evaluateQuery(context, targets))
    {
        if (queryContainsActiveSheetMetal(context, entity))
        {
            sheetMetalEntities = append(sheetMetalEntities, entity);
        }
        else
        {
            nonSheetMetalEntities = append(nonSheetMetalEntities, entity);
        }
    }
    return { "sheetMetalQueries" : qUnion(sheetMetalEntities), "nonSheetMetalQueries" : qUnion(nonSheetMetalEntities) };
}

/**
 * @internal
 */
export function checkNotInFeaturePattern(context is Context, references is Query, error is ErrorStringEnum)
{
    var remainingTransform = getRemainderPatternTransform(context, {"references" : references});
    if (remainingTransform != identityTransform())
    {
        throw regenError(error);
    }
}

/**
 * @internal
 * Used in importDerived to strip sheet metal related data off the imported context
 * returns query of all sheet metal parts (3d and flattened)
 */
export function clearSheetMetalData(context, id) returns Query
{
    // All the attribute queries are evaluated immediately because this function
    // removes all SMAttributes and SMAssociationAttribute
    var smModelsQ = qAttributeQuery(asSMAttribute({objectType : SMObjectType.MODEL}));
    var smModelsEvaluated = evaluateQuery(context, smModelsQ);

    if (size(smModelsEvaluated) == 0)
        return qNothing();

    var smModelsActiveQ = qAttributeQuery(asSMAttribute({objectType : SMObjectType.MODEL,
                                                  active : true}));
    var smModelsActiveEvaluated = evaluateQuery(context, smModelsActiveQ);

    // Solid bodies 3d and Flat and only they are associated with sheet bodies
    var associationAttributes = getAttributes(context, {"entities" : smModelsQ, "attributePattern" : {} as SMAssociationAttribute});
    var smPartNBendLineQArr = [];
    for (var attribute in associationAttributes)
    {
        smPartNBendLineQArr = append(smPartNBendLineQArr, qAttributeQuery(attribute));
    }

    // Bend centerlines are wire bodies associated with edges of sheet bodies
    // there is more topology associated with edges that  why we have to apply filters to qAttributeQuery
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V557_NO_BEND_CENTERLINE_IN_DERIVED))
    {
        associationAttributes = getAttributes(context, {"entities" : qOwnedByBody(smModelsQ, EntityType.EDGE), "attributePattern" : {} as SMAssociationAttribute});
        for (var attribute in associationAttributes)
        {
            smPartNBendLineQArr = append(smPartNBendLineQArr, qEntityFilter(qBodyType(qAttributeQuery(attribute), BodyType.WIRE), EntityType.BODY));
        }
    }


    var smPartNBendLineQEvaluated = evaluateQuery(context, qUnion(smPartNBendLineQArr));

    // remove all SMAttributes
    removeAttributes(context, {
        "attributePattern" : asSMAttribute({})
    });

    // Deactivating active sheet metal models
    if (size(smModelsActiveEvaluated) > 0)
    {
        updateSheetMetalGeometry(context, id, { "entities" : qUnion(smModelsActiveEvaluated) });
    }

    // remove all SMAssociationAttribute
    removeAttributes(context, {
        "attributePattern" : {} as SMAssociationAttribute
    });

    // Deleting all sheet bodies
    opDeleteBodies(context, id + "deleteSheetBodies", {
            "entities" : qUnion(smModelsEvaluated)
    });

   return qUnion(smPartNBendLineQEvaluated);
}

/**
 * @internal
 */
export function createRipAttribute(context is Context, entity is Query, ripId is string, ripStyle is SMJointStyle, jointAttributes)
{
    var ripAttribute = makeSMJointAttribute(ripId);
    ripAttribute.jointType = { "value" : SMJointType.RIP, "canBeEdited": true };

    var angle = try silent(edgeAngle(context, entity));
    if (angle != undefined)
    {
        ripAttribute.angle = {"value" : angle, "canBeEdited" : false};
    }
    // If the angle is zero then this rip should not have a style
    if (abs(angle) >= TOLERANCE.zeroAngle * radian) {
        ripAttribute.jointStyle = { "value" : ripStyle, "canBeEdited": true };
    }

    if (jointAttributes != undefined && jointAttributes.minimalClearance != undefined)
    {
        ripAttribute.minimalClearance = jointAttributes.minimalClearance;
    }
    return ripAttribute;
}

/**
 * @internal
 */
export function createBendAttribute(context is Context, entity is Query, bendId is string, jointAttributes)
{
    var bendAttribute = makeSMJointAttribute(bendId);
    bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited": true };
    if (jointAttributes != undefined && jointAttributes.bendRadius != undefined)
    {
        bendAttribute.radius = {
            "value" : jointAttributes.bendRadius,
            "canBeEdited" : true
        };
    }
    var angle = try silent(edgeAngle(context, entity));
    if (angle != undefined)
    {
        bendAttribute.angle = {"value" : angle, "canBeEdited" : false};
    }
    return bendAttribute;
}

/**
 * @internal
 */
export function findCornerDefinitionVertex(context is Context, entity is Query) returns Query
{
    var definitionEntities = qUnion(getSMDefinitionEntities(context, entity));
    var sheetVertices = qEntityFilter(definitionEntities, EntityType.VERTEX);
    var found = size(evaluateQuery(context, sheetVertices));
    if (found == 0)
    {
        // Look for the vertices adjacent to any edge to get to it from them
        sheetVertices = qUnion(getSMDefinitionEntities(context, qVertexAdjacent(qEntityFilter(entity, EntityType.EDGE), EntityType.VERTEX)));
        found = size(evaluateQuery(context, sheetVertices));
    }
    if (found != 1)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_RIP_NO_CORNER);
    }
    return sheetVertices;
}

