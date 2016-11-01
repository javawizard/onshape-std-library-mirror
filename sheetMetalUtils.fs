FeatureScript 442; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "442.0");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "442.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "442.0");
import(path : "onshape/std/containers.fs", version : "442.0");
import(path : "onshape/std/coordSystem.fs", version : "442.0");
import(path : "onshape/std/curveGeometry.fs", version : "442.0");
import(path : "onshape/std/evaluate.fs", version : "442.0");
import(path : "onshape/std/feature.fs", version : "442.0");
import(path : "onshape/std/math.fs", version : "442.0");
import(path : "onshape/std/manipulator.fs", version : "442.0");
import(path : "onshape/std/query.fs", version : "442.0");
import(path : "onshape/std/sketch.fs", version : "442.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "442.0");
import(path : "onshape/std/smobjecttype.gen.fs", version : "442.0");
import(path : "onshape/std/string.fs", version : "442.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "442.0");
import(path : "onshape/std/tool.fs", version : "442.0");
import(path : "onshape/std/valueBounds.fs", version : "442.0");
import(path : "onshape/std/vector.fs", version : "442.0");
import(path : "onshape/std/topologyUtils.fs", version : "442.0");

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */


/**
 * @internal
 */
export enum ToleranceLevel
{
    annotation { "Name" : "Very tight" }
    VERY_TIGHT,
    annotation { "Name" : "Tight" }
    TIGHT,
    annotation { "Name" : "Medium" }
    MEDIUM,
    annotation { "Name" : "Loose" }
    LOOSE
}

/**
 * @internal
 */
export function getEdgeCSys(context is Context, edge is Query) returns CoordSystem
{
    var faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
    if (size(faces) != 2)
        throw regenError("Bad edge");
    var offsetData = findOffsetEdgeData(context, edge);

    var topPlane = evPlane(context, {
            "face" : offsetData.topFace
    });

    var sidePlane = evPlane(context, {
            "face" : offsetData.sideFace
    });

    var edgeLine = evEdgeTangentLine(context, {
            "edge" : edge,
            "parameter" : 0.5
    });

    var normal = cross(sidePlane.normal, edgeLine.direction);
    if (dot(normal, topPlane.normal) < 0)
    {
        edgeLine.direction = -edgeLine.direction;
    }
    return coordSystem(edgeLine.origin, edgeLine.direction, topPlane.normal);
}

/**
 * @internal
 */
// TODO BRT - Change this to use sheet metal attributes instead of geometry.
export function findOffsetEdgeData(context is Context, edge is Query) returns map
{
    const BIG_NUMBER = 1.0e20 * meter;
    var attr = getAttributes(context, {
            "entities" : qOwnerBody(edge)
    });
    if (size(attr) != 1 || attr[0].thickness == undefined || attr[0].thickness.value == undefined)
        throw regenError("Bad sheet metal attribute");
    var offsetDistance = attr[0].thickness.value;
    if (offsetDistance is number)
        offsetDistance *= meter;
    var edgeLine = evEdgeTangentLine(context, {
            "edge" : edge,
            "parameter" : 0.0
    });
    var edgeLength = evLength(context, {
            "entities" : edge
    });
    var minDistance = BIG_NUMBER;
    var offsetEdge;
    var faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
    if (size(faces) != 2)
        throw regenError("Could not find offset data");

    var topFace;
    var sideFace;
    if (hasSheetMetalAttribute(context, faces[0], SMObjectType.WALL))
    {
        topFace = faces[0];
        sideFace = faces[1];
    }
    else
    {
        topFace = faces[1];
        sideFace = faces[0];
    }
    var edges = evaluateQuery(context, qEdgeAdjacent(sideFace, EntityType.EDGE));
    for (var testEdge in edges)
    {
        if (testEdge == edge)
            continue;
        var testEdgeLine = evEdgeTangentLine(context, {
                "edge" : testEdge,
                "parameter" : 0.0
        });
        if (!tolerantParallel(edgeLine, testEdgeLine) || tolerantCoLinear(edgeLine, testEdgeLine))
            continue;
        var testEdgeLength = evLength(context, {
                "entities" : testEdge
        });
        if (abs(testEdgeLength - edgeLength) > TOLERANCE.zeroLength * meter)
            continue;
        var v = testEdgeLine.origin - edgeLine.origin;
        v = v - edgeLine.direction * dot(edgeLine.direction, v);
        var distance = norm(v);
        if (distance < minDistance)
        {
            minDistance = distance;
            offsetEdge = testEdge;
        }
    }

    if (minDistance == BIG_NUMBER)
        throw regenError("Could not find offset edge");

    return {
        "offsetEdge" : offsetEdge,
        "offsetDistance" : minDistance,
        "sideFace" : sideFace,
        "topFace" : topFace
    };
}

/**
 * @internal
 */
export function tolerantParallel(line0 is Line, line1 is Line) returns boolean
{
    return squaredNorm(cross(line0.direction, line1.direction)) < TOLERANCE.zeroAngle * TOLERANCE.zeroAngle;
}

/**
 * @internal
 */
export function tolerantCoLinear(line0 is Line, line1 is Line) returns boolean
{
    if (tolerantParallel(line0, line1)) {
        var v = line1.origin - line0.origin;
        v = v - line0.direction * dot(v, line0.direction);
        var lengthTolerance = TOLERANCE.zeroLength * meter;
        return squaredNorm(v) < lengthTolerance * lengthTolerance;
    }
    return false;
}

/**
 * @internal
 */
export function hasSheetMetalAttribute(context is Context, entities is Query, objectType is SMObjectType) returns boolean
{
    return size(getSmObjectTypeAttributes(context, entities, objectType)) != 0;
}

/**
 * @internal
 */
export function defineSheetMetalFeature(feature is function, defaults is map) returns function
{
    defaults.isSheetMetal = true;
    return defineFeature(feature, defaults);
}


/**
 * @internal
 */
 export function updateSheetMetalGeometry(context is Context, id is Id, args is map)
 {
    @updateSheetMetalGeometry(context, id, args);
 }


/**
* @internal
* @param args {{
*       @field surfaceBodies{Query}
*       @field bendEdges{Query}
*       @field specialRadiiBends{array} : array of pairs "(edge, bendRadius)"
*       @field defaultRadius{ValueWithUnits} : bend radius to be applied to bendEdges
*       @field controlsThickness{boolean}
*       @field thickness{ValueWithUnits}
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
    var modelAttribute = asSMAttribute({"attributeId" : featureIdString,
                    "objectType" : SMObjectType.MODEL,
                    "active" : true,
                    "thickness" : thicknessData,
                    "k-factor" : kFactorData,
                    "minimalClearance" : minimalClearanceData,
                    "defaultBendRadius" : {"value" : args.defaultRadius}});

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
            if (zeroAngle)
            {
                ripAttribute.jointStyle = { "value" : SMJointStyle.FLAT, "canBeEdited": false };
                ripAttribute.jointType.canBeEdited = false;
            }
            else
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
 * @internal
 * For an edge between two planes computes angle between plane normals
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
 * A `RealBoundSpec` for sheet metal K-factor between 0. and 1., defaulting to `.45`.
 */
export const K_FACTOR_BOUNDS =
{
    (unitless) : [0, 0.45, 1]
} as RealBoundSpec;


/**
 * @internal
 * A `LengthBoundSpec` for minimal clearance of sheet  metal rips
 */
export const SM_MINIMAL_CLEARANCE_BOUNDS =
{
    (meter)      : [2e-5, 2e-5, 1],
    (centimeter) : 2e-3,
    (millimeter) : 0.02,
    (inch)       : 1e-3,
    (foot)       : 1e-4,
    (yard)       : 2e-5
} as LengthBoundSpec;

/**
 * @internal
 * Partitions allParts into non-sheetmetal parts and sheetmetal parts
 * To preserve existing behavior of code the returned non-sm query is exactly the same as what is passed in
 * for non-sm cases and a query is returned for them
 * The sheetmetal results will usually be iterated through and so are returned as an array
 */
export function partitionSheetMetalParts(context is Context, allParts is Query)
{
    if (!queryContainsSheetMetal(context, allParts))
    {
        // Don't mess with the query, for performance and legacy reasons
        return { "nonSheetMetalPartsQuery" : allParts, "sheetMetalPartsArray" : [] };
    }
    var parts = evaluateQuery(context, allParts);
    var nonSheetMetal = [];
    var sheetMetal = [];
    for (var part in parts)
    {
        if (isActiveSheetMetalPart(context, part))
        {
            sheetMetal = append(sheetMetal, part);
        }
        else
        {
            nonSheetMetal = append(nonSheetMetal, part);
        }
    }
    return { "nonSheetMetalPartsQuery" : qUnion(nonSheetMetal), "sheetMetalPartsArray" : sheetMetal };
}

/**
 * @internal
 */
export function queryContainsSheetMetal(context is Context, query is Query) returns boolean
{
    return size(evaluateQuery(context, query)) > 0 &&
                                 size(getAttributes(context, {
                                     "entities" : query,
                                     "attributePattern" : {} as SMAssociationAttribute
                                 })) > 0;
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
            //Remove SMAttribute and reassign to appropriate entities with tweaked id.
            removeAttributes(context, { "entities" : qUnion(evaluatedEntitiesToUpdateSmAttribute), "attributePattern" : {} as SMAttribute });
            var index = 0; //index assigned attributes
            for (var count = 0; count < size(evaluatedEntitiesToUpdateSmAttribute); count += 1)
            {
                var entity = evaluatedEntitiesToUpdateSmAttribute[count];
                if (isEntityAppropriateForAttribute(context, entity, definitionAttributes[0]))
                {
                    var tweakedAttribute = definitionAttributes[0];
                    if (index > 0 && tweakedAttribute.objectType != SMObjectType.MODEL)
                    {
                        //TODO: share definition attribute between entities
                        tweakedAttribute.attributeId = definitionAttributes[0].attributeId ~ "." ~ toString(index);
                    }
                    // If masterEntities is a new entity, make sure it is the one inheriting the definition attribute
                    if (index == 0)
                    {
                        masterEntities = entity;
                    }
                    setAttribute(context, { "entities" : entity, "attribute" : tweakedAttribute });
                    index += 1;
                }
                else if (count == 0 && nMaster > 0)
                {
                    throw "Existing entity not fit for definition attribute";
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

