FeatureScript 2581; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "2581.0");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "2581.0");
import(path : "onshape/std/context.fs", version : "2581.0");
import(path : "onshape/std/containers.fs", version : "2581.0");
import(path : "onshape/std/debug.fs", version : "2581.0");
import(path : "onshape/std/evaluate.fs", version : "2581.0");
import(path : "onshape/std/flatOperationType.fs", version : "2581.0");
import(path : "onshape/std/feature.fs", version : "2581.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2581.0");
import(path : "onshape/std/sheetMetalBuiltIns.fs", version : "2581.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2581.0");
import(path : "onshape/std/query.fs", version : "2581.0");

/**
 *  Adds or removes material of sheet metal part as specified by faces attached to its its flat pattern
 *  Is the implementation of sheet metal extrude in flat.
 *  @param definition {{
 *          @field faces {Query} :
 *              Faces associated with sheet metal flat,  typically sketch faces or region of sketch in flat
 *          @field flatOperationType {FlatOperationType}
 *  }}
 **/
export const SMFlatOp = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    {
        const bodyQ = qUnion([qPartsAttachedTo(definition.faces), qOwnerBody(definition.faces)]);
        if (!areEntitiesFromSingleActiveSheetMetalModel(context, bodyQ))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_MODEL_REQUIRED);
        }
        const smDefinitionBodiesQ = getSheetMetalModelForPart(context, bodyQ);
        const sheetMetalEntitiesQ = qUnion([qOwnedByBody(smDefinitionBodiesQ, EntityType.EDGE), qOwnedByBody(smDefinitionBodiesQ, EntityType.FACE), smDefinitionBodiesQ]);
        const tracking = startTracking(context, sheetMetalEntitiesQ);

        var initialDataPerBody = [];
        var initialData;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V860_SM_FLAT_ERRORS))
        {
            for (var smBody in evaluateQuery(context, smDefinitionBodiesQ))
            {
                initialDataPerBody = append(initialDataPerBody, {  'query' : qUnion([smBody, startTracking(context, smBody)]),
                                                'entitiesAndAttributes' : getInitialEntitiesAndAttributes(context, smBody) });
            }
        }
        else
        {
            initialData = getInitialEntitiesAndAttributes(context, smDefinitionBodiesQ);
        }

        definition.operationType = definition.flatOperationType == FlatOperationType.ADD ? BooleanOperationType.UNION : BooleanOperationType.SUBTRACTION;
        opSMFlatOperation(context, id, definition);

        makeFaceJointsUneditable(context, qCreatedBy(id, EntityType.FACE));

        const newEntities = qUnion([qCreatedBy(id), tracking]);
        const affectedBodyQ = qOwnerBody(newEntities);
        if (initialData == undefined) // data was collected in initialDataPerBody
        {
            initialData = combineInitialData(context, initialDataPerBody, affectedBodyQ);
        }
        const toUpdate = assignSMAttributesToNewOrSplitEntities(context, affectedBodyQ, initialData, id);

        callSubfeatureAndProcessStatus(id, updateSheetMetalGeometry, context, id + "smUpdate", {
                    "entities" : toUpdate.modifiedEntities,
                    "deletedAttributes" : toUpdate.deletedAttributes,
                    "associatedChanges" : tracking
                });
    }, {});

/**
 *  Applies a requested feature in sheet metal flat, changes definition surfaces to reflect this and updates sheet metal geometry
 *  Used by fillet and chamfer
 *  @param definition {{
 *      @field entities {Query}:
 *                 Sheet metal edges or vertices (flat selections are allowed) associated with vertices in definition
 *      @field crossSection {FilletCrossSection}
 *      @field chamferType {ChamferType} : @requiredif {`crossSection` is `CHAMFER`}
 *      @field width {ValueWithUnits} : @requiredif {`chamferType` is `EQUAL_OFFSETS` or `OFFSET_ANGLE`.}
 *      @field width1 {ValueWithUnits} : @requiredIf {`chamferType` is `TWO_OFFSETS`.}
 *      @field width2 {ValueWithUnits} : @requiredIf {`chamferType` is `TWO_OFFSETS`.}
 *      @field angle {ValueWithUnits} : @requiredIf {`chamferType` is `OFFSET_ANGLE`.}
 *      @field oppositeDirection {boolean} : @optional
 *      @field radius {ValueWithUnits} : @requiredIf {`crossSection` is `CIRCULAR` or `CONIC` or `CURVATURE`.}
 *      @field isAsymmetric {boolean} : @optional
 *      @field flipAsymmetric {boolean} : @optional
 *      @field otherRadius {ValueWithUnits} : @requiredIf {`isAsymmetric` is true.}
 *      @field rho {number} : @requiredif {`crossSection` is `CONIC`.}
 *      @field magnitude {number} : @requiredif {`crossSection` is `CURVATURE`.}
 *  }}
 **/
export const SMEdgeBlendImpl = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
{
    const entities = filterOutInvalidEdges(context, id, definition.entities);
    const bodyQ = qOwnerBody(entities);
    const smDefinitionBodiesQ = getSheetMetalModelForPart(context, bodyQ);
    const sheetMetalEntitiesQ = qUnion([qOwnedByBody(smDefinitionBodiesQ, EntityType.EDGE), qOwnedByBody(smDefinitionBodiesQ, EntityType.FACE), smDefinitionBodiesQ]);
    const tracking = startTracking(context, sheetMetalEntitiesQ);

    const edge2SidedQ = smDefinitionBodiesQ->qOwnedByBody(EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.TWO_SIDED);
    const edgeJointAttributes = getSmObjectTypeAttributes(context, edge2SidedQ, SMObjectType.JOINT);

    var initialDataPerBody = [];
    for (var smBody in evaluateQuery(context, smDefinitionBodiesQ))
    {
        var bodyEntsAndAttributes = getInitialEntitiesAndAttributes(context, smBody) ;
        const bendSheetBodiesQ = getAssociatedBendSheetBodies(context,  smBody->qOwnedByBody(EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.TWO_SIDED));
        const bendSheetBodyEnts = evaluateQuery(context, bendSheetBodiesQ->qOwnedByBody(EntityType.FACE));
        bodyEntsAndAttributes.originalEntities = concatenateArrays([bodyEntsAndAttributes.originalEntities, bendSheetBodyEnts]);
        bodyEntsAndAttributes.originalEntitiesTracking = concatenateArrays([bodyEntsAndAttributes.originalEntitiesTracking,
            startTrackingIdentityBatched(context, qUnion(bendSheetBodyEnts))]);
        initialDataPerBody = append(initialDataPerBody, {  'query' : qUnion([smBody, startTracking(context, smBody)]),
                                                            'entitiesAndAttributes' : bodyEntsAndAttributes});
    }

    sheetMetalApplyInFlat(context, id, mergeMaps(definition, {"entities" : entities}));

    const newBendFacesQ = cleanUpAttributes(context, edgeJointAttributes, qCreatedBy(id, EntityType.EDGE));
    makeFaceJointsUneditable(context, newBendFacesQ);
    blockReliefAtBendEnds(context, id, newBendFacesQ);
    const newBodiesQ = tracking->qEntityFilter(EntityType.BODY);
    const initialData = combineInitialData(context, initialDataPerBody, newBodiesQ);
    var toUpdate = assignSMAttributesToNewOrSplitEntities(context, newBodiesQ, initialData, id);
    callSubfeatureAndProcessStatus(id, updateSheetMetalGeometry, context, id + "smUpdate", {
                "entities" : toUpdate.modifiedEntities,
                "deletedAttributes" : toUpdate.deletedAttributes
    });
},{oppositeDirection : false, isAsymmetric : false, flipAsymmetric : false});


// Find any faces in `faces` which are joints, and make sure their radius and jointType are uneditable
function makeFaceJointsUneditable(context is Context, faces is Query)
{
    const jointTypeNotEditable = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1045_SHEET_BOOLEAN_ALIGN_FACE);
    const faceJoints = qAttributeFilter(faces, asSMAttribute({ 'objectType' : SMObjectType.JOINT }));
    for (var face in evaluateQuery(context, faceJoints))
    {
        const oldJointAttribute = getJointAttribute(context, face);
        if (oldJointAttribute == undefined)
        {
            if (jointTypeNotEditable)
                throw "qAttributeFilter failed";
            else
                continue;
        }

        var newJointAttribute = oldJointAttribute;
        if (oldJointAttribute.radius != undefined && oldJointAttribute.radius.canBeEdited)
        {
            // Joints that are faces cannot have their radius changed
            newJointAttribute.radius.canBeEdited = false;
        }
        if (jointTypeNotEditable && oldJointAttribute.jointType.canBeEdited)
        {
            // Joints that are faces cannot have their joint type changed
            newJointAttribute.jointType.canBeEdited = false;
        }

        if (newJointAttribute != oldJointAttribute)
        {
            removeAttributes(context, { "entities" : face, "attributePattern" : oldJointAttribute });
            setAttribute(context, { "entities" : face, "attribute" : newJointAttribute });
        }
    }
}

// Initial data was collected for all model definition bodies, bodiesQ - bodies affected by operation
// combine initial data for those bodies only
function combineInitialData(context is Context, initialDataPerBody is array, bodiesQ is Query)
{
    var targets = {};
    for (var body in evaluateQuery(context, bodiesQ))
    {
        targets[body] = true;
    }
    var originalEntitiesArr = [];
    var initialAssociationAttributesArr = [];
    var originalEntitiesTrackingArr =[];
    for (var bodyData in initialDataPerBody)
    {
        for (var body in evaluateQuery(context, bodyData.query))
        {
            if (targets[body] == true)
            {
                originalEntitiesArr = append(originalEntitiesArr, bodyData.entitiesAndAttributes.originalEntities);
                initialAssociationAttributesArr = append(initialAssociationAttributesArr,
                                            bodyData.entitiesAndAttributes.initialAssociationAttributes);
                originalEntitiesTrackingArr = append(originalEntitiesTrackingArr, bodyData.entitiesAndAttributes.originalEntitiesTracking);
            }
        }
    }
    return {'originalEntities' : concatenateArrays(originalEntitiesArr),
            'initialAssociationAttributes' : concatenateArrays(initialAssociationAttributesArr),
            'originalEntitiesTracking' : concatenateArrays(originalEntitiesTrackingArr)};
}

/**
 * precondition filters might let through edges invalid for fillet and chamfer - filtering them out here
 * */
function filterOutInvalidEdges(context is Context, id is Id, entities is Query) returns Query
{
    const activeEntities = requireActiveModels(context, id, entities);
    var edgesToSkip = [];
    for (var edge in evaluateQuery(context, activeEntities->qEntityFilter(EntityType.EDGE)))
    {
        const convex = evEdgeConvexity(context, {
                "edge" : edge
        });
        if (convex != EdgeConvexityType.CONVEX && convex != EdgeConvexityType.CONCAVE)
        {
            edgesToSkip = append(edgesToSkip, edge);
            continue;
        }
        const edgeFacesQ = qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE);
        const faceDefinitionEntsQ = qUnion(getSMDefinitionEntities(context, edgeFacesQ));
        if (!isQueryEmpty(context, faceDefinitionEntsQ->qEntityFilter(EntityType.FACE)))
        {
            edgesToSkip = append(edgesToSkip, edge);
        }
    }

    var out = activeEntities;
    if (edgesToSkip != [])
    {
        const toSkipQ = qUnion(edgesToSkip);
        out = activeEntities->qSubtraction(toSkipQ);
        if (isQueryEmpty(context, out))
        {
            throw regenError(ErrorStringEnum.INVALID_INPUT, toSkipQ);
        }
        addDebugEntities(context, toSkipQ, DebugColor.RED);
        reportFeatureWarning(context, id, ErrorStringEnum.PARTIALLY_INVALID_INPUT);
        setErrorEntities(context, id, {
                "entities" : toSkipQ
        });
    }
    return out;
}

function requireActiveModels(context is Context, id is Id, entities is Query) returns Query
{
    if (isQueryEmpty(context, entities->qActiveSheetMetalFilter(ActiveSheetMetal.YES)))
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_ENTITY_NEEDED, ["entities"], entities);
    }
    const inactiveQ = entities->qActiveSheetMetalFilter(ActiveSheetMetal.NO);
    if (!isQueryEmpty(context, inactiveQ))
    {
        addDebugEntities(context, inactiveQ, DebugColor.RED);
        reportFeatureWarning(context, id, ErrorStringEnum.SHEET_METAL_ACTIVE_ENTITY_NEEDED);
        setErrorEntities(context, id, {
            "entities" : inactiveQ
            });
    }
    return entities->qActiveSheetMetalFilter(ActiveSheetMetal.YES);
}

function blockReliefAtBendEnds(context is Context, id is Id, bendFaces is Query)
{
    var processedVerts = {};
    var count = 0;
    for (var vertex in evaluateQuery(context, bendFaces->qAdjacent(AdjacencyType.VERTEX, EntityType.VERTEX)))
    {
        if (processedVerts[vertex.transientId] == true)
        {
            continue;
        }
        const cornerInfo = evCornerType(context, {
                "vertex" : vertex
            });
        if (cornerInfo.cornerType == SMCornerType.BEND_END)
        {
            var cornerAttribute = makeSMCornerAttribute(toAttributeId(id + count));
            count += 1;
            cornerAttribute.cornerStyle = {
                "value" : SMReliefStyle.TEAR,
                "canBeEdited" : false
            };
            setAttribute(context, { "entities" : cornerInfo.primaryVertex, "attribute" : cornerAttribute });
            for (var v in cornerInfo.allVertices)
            {
                processedVerts[v.transientId] = true;
            }
        }
    }
}

function cleanUpAttributes(context is Context, edgeJointAttributes is array, newEdgesQ is Query) returns Query
{
    var toRemoveJoint = [];
    var bendFaceQs = [];
    for (var attribute in edgeJointAttributes)
    {
        const attribQ = qAttributeQuery(attribute);
        if (!isQueryEmpty(context, attribQ->qEntityFilter(EntityType.FACE)))
        {
            bendFaceQs = append(bendFaceQs, attribQ->qEntityFilter(EntityType.FACE));
            const edgesWithAttribute = evaluateQuery(context, attribQ->qEntityFilter(EntityType.EDGE));
            if (edgesWithAttribute != [])
            {
                toRemoveJoint = concatenateArrays([toRemoveJoint, edgesWithAttribute]);
            }
        }
        else
        {
            const edgesWithAttribute = evaluateQuery(context, attribQ->qEntityFilter(EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.ONE_SIDED));
            if (edgesWithAttribute != [])
            {
                toRemoveJoint = concatenateArrays([toRemoveJoint, edgesWithAttribute]);
            }
        }
    }

    if (toRemoveJoint != [])
    {
        removeAttributes(context, { "entities" : qUnion(toRemoveJoint),
            "attributePattern" : asSMAttribute({})
        });
    }

    const newBendFaces = evaluateQuery(context, qUnion(bendFaceQs));
    var toRemoveAssociation = [];
    const smBodiesQ = qOwnerBody(qUnion(newBendFaces));
    for (var bendFace in newBendFaces)
    {
        const associatedWithFaceQ = getAssociatedQ(context, bendFace);
        const toRemoveInDefQ = associatedWithFaceQ->qOwnedByBody(smBodiesQ)->qEntityFilter(EntityType.EDGE);
        if (!isQueryEmpty(context, toRemoveInDefQ))
            toRemoveAssociation = concatenateArrays([toRemoveAssociation, evaluateQuery(context, toRemoveInDefQ)]);

        var bodiesAssociatedWithFaceQ = associatedWithFaceQ->qEntityFilter(EntityType.BODY);
        bodiesAssociatedWithFaceQ = qSubtraction(bodiesAssociatedWithFaceQ, bodiesAssociatedWithFaceQ->qBodyType(BodyType.WIRE));
        for (var edge in evaluateQuery(context, qAdjacent(bendFace, AdjacencyType.VERTEX, EntityType.EDGE)))
        {
            const associatedWithEdgeQ = getAssociatedQ(context, edge);
            const vertsToRemoveInDefQ = associatedWithEdgeQ->qOwnedByBody(smBodiesQ)->qEntityFilter(EntityType.VERTEX);
            if (!isQueryEmpty(context,vertsToRemoveInDefQ))
            {
                toRemoveAssociation = concatenateArrays([toRemoveAssociation, evaluateQuery(context, vertsToRemoveInDefQ)]);
            }
        }
    }
   if (toRemoveAssociation != [])
        removeAttributes(context, { "entities" : qUnion(toRemoveAssociation),
                "attributePattern" :  {} as SMAssociationAttribute
            });
   const seedQ = qUnion(append(newBendFaces, newEdgesQ));
   const verticesInAdjacentFacesQ = seedQ->qAdjacent(AdjacencyType.VERTEX, EntityType.FACE)->qAdjacent(AdjacencyType.VERTEX, EntityType.VERTEX);
   const all2SidedEdgesQ = seedQ->qOwnerBody()->qOwnedByBody(EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.TWO_SIDED);
   const jointEdgesQ = all2SidedEdgesQ->qAttributeFilter(asSMAttribute({'objectType' : SMObjectType.JOINT}));
   const verticesNextToJointEdgesQ = jointEdgesQ->qAdjacent(AdjacencyType.VERTEX, EntityType.VERTEX);
   removeAttributes(context, { "entities" : qSubtraction(verticesInAdjacentFacesQ, verticesNextToJointEdgesQ),
                "attributePattern" :  asSMAttribute({})
        });
   return qUnion(newBendFaces);
}

function getAssociatedQ(context is Context, ent is Query) returns Query
{
    const attributes = getAttributes(context, {
        "entities" : ent,
        "attributePattern" : {} as SMAssociationAttribute
    });
    if (attributes == [])
        return qNothing();
    return qAttributeQuery(attributes[0]);
}

function getAssociatedBendSheetBodies(context is Context,  edge2SidedQ is Query) returns Query
{
    const attributes = getSMAssociationAttributes(context, edge2SidedQ);
    var sheetBodies = [];
    for (var att in attributes)
    {
        const attQ = qAttributeQuery(att)->qEntityFilter(EntityType.BODY);
        for (var b in evaluateQuery(context, attQ))
        {
            if (size(evaluateQuery(context, b->qOwnedByBody(EntityType.FACE))) == 1)
            {
                sheetBodies = append(sheetBodies, b);
                break;
            }
        }
    }
    return qUnion(sheetBodies);
}

