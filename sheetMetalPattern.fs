FeatureScript 2796; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "2796.0");
import(path : "onshape/std/boolean.fs", version : "2796.0");
import(path : "onshape/std/containers.fs", version : "2796.0");
import(path : "onshape/std/curveGeometry.fs", version : "2796.0");
import(path : "onshape/std/evaluate.fs", version : "2796.0");
import(path : "onshape/std/feature.fs", version : "2796.0");
import(path : "onshape/std/holeAttribute.fs", version : "2796.0");
import(path : "onshape/std/holepropagationtype.gen.fs", version : "2796.0");
import(path : "onshape/std/math.fs", version : "2796.0");
import(path : "onshape/std/patternCommon.fs", version : "2796.0");
import(path : "onshape/std/registerSheetMetalBooleanTools.fs", version : "2796.0");
import(path : "onshape/std/registerSheetMetalFormedTools.fs", version : "2796.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2796.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2796.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2796.0");
import(path : "onshape/std/topologyUtils.fs", version : "2796.0");
import(path : "onshape/std/transform.fs", version : "2796.0");
import(path : "onshape/std/units.fs", version : "2796.0");
import(path : "onshape/std/vector.fs", version : "2796.0");

/**
 * @internal
 * Apply pattern to sheet metal entities.
 */
export const sheetMetalGeometryPattern = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    {
        const topLevelId = definition.topLevelId;
        var attributeIdCounter = new box(0);
        var errorEntities = qNothing();
        var formedErrorSketchBodies = qNothing();
        var updateMap;
        if (isPartPattern(definition.patternType))
        {
            var solidBodyInput = qBodyType(qEntityFilter(definition.entities, EntityType.BODY), BodyType.SOLID);
            if (size(evaluateQuery(context, definition.entities)) != size(evaluateQuery(context, solidBodyInput)))
            {
                throw "Entries should be solid bodies";
            }

            // Part pattern in sheet metal is executed as a face pattern of the walls corresponding to the selected part.
            // We cannot body pattern the entire underlying sheet body because the selected part may only correspond to
            // a subset of the walls of the sheet body. This happens when the sheet metal has a rip or rips that leave the
            // underlying sheet body as one body, but builds out as multiple thickened sheet metal parts.
            const definitionFaces = getSMDefinitionEntities(context, qOwnedByBody(definition.entities), EntityType.FACE);

            var patternMap = sheetMetalWallPattern(context, topLevelId, id, definitionFaces, definition, attributeIdCounter);
            if (patternMap.booleanWasNoOp)
            {
                reportBooleanUnionNoOp(context, topLevelId, id + "noOp", definitionFaces, definition);
            }

            // Store only the modifiedEntities and deletedAttributes
            patternMap.booleanWasNoOp = undefined;
            updateMap = patternMap;
        }
        else if (isFacePattern(definition.patternType))
        {
            if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V764_EDGE_PATTERN))
                throw regenError(ErrorStringEnum.SHEET_METAL_PARTS_PROHIBITED);

            const separatedEntities = separateEntitiesForFacePattern(context, topLevelId, definition);
            const definitionWalls = separatedEntities.definitionWalls;

            // Combine edges with their tracking queries such that if wall pattern changes their identity,
            // they can still be evaluated
            var definitionEdgesQ = qUnion(separatedEntities.definitionEdges);
            definitionEdgesQ = qUnion([definitionEdgesQ, startTracking(context, definitionEdgesQ)]);

            var modifiedEntities = [];
            var deletedAttributes = [];
            if (size(definitionWalls) > 0)
            {
                const wallUpdateMap = sheetMetalWallPattern(context, topLevelId, id + "wallPattern", definitionWalls,
                        definition, attributeIdCounter);
                modifiedEntities = [wallUpdateMap.modifiedEntities];
                deletedAttributes = wallUpdateMap.deletedAttributes;
            }

            //sheetMetalEdgePattern may change bodyIds so process formToolMap before that
            if (separatedEntities.formToolMap != {})
            {
                const formPatternResult = sheetMetalFormPattern(context, id + "formPattern", definition, separatedEntities.formToolMap, definitionWalls);
                modifiedEntities = concatenateArrays([modifiedEntities, formPatternResult.modifiedWalls]);
                const formedErrorSolidBodies = qBodyType(formPatternResult.patternedFormTools, BodyType.SOLID);
                errorEntities = qUnion(errorEntities, formedErrorSolidBodies);
                formedErrorSketchBodies = qSubtraction(formPatternResult.patternedFormTools, formedErrorSolidBodies);
            }

            const definitionEdges = evaluateQuery(context, definitionEdgesQ);
            if (size(definitionEdges) > 0)
            {
                const edgeUpdateMap = sheetMetalEdgePattern(context, topLevelId, id + "edgePattern", definitionEdges,
                        definition, attributeIdCounter);
                modifiedEntities = append(modifiedEntities, edgeUpdateMap.modifiedEntities);
                deletedAttributes = concatenateArrays([deletedAttributes, edgeUpdateMap.deletedAttributes]);
            }

            const holeToolBodies = separatedEntities.holeToolMap.sheetMetalHoleToolBodies;
            if (holeToolBodies != [])
            {
                const holePatternResult = sheetMetalHolePattern(context, id + "holePattern", definition, separatedEntities.holeToolMap, definitionWalls);
                modifiedEntities = concatenateArrays([modifiedEntities, holePatternResult.modifiedWalls]);
                errorEntities = qUnion(errorEntities, holePatternResult.patternedHoleTools);
            }

            updateMap = {
                "modifiedEntities" : qUnion(modifiedEntities),
                "deletedAttributes" : deletedAttributes
            };
        }
        else
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN, ["patternType"]);
        }

        // Build out final sheet metal
        const smUpdateId = id + "smUpdate";
        callSubfeatureAndProcessStatus(topLevelId, updateSheetMetalGeometry, context, smUpdateId, {
                    "entities" : updateMap.modifiedEntities,
                    "deletedAttributes" : updateMap.deletedAttributes
                });

        /* If any of the patterned hole tool bodies remain public after the updateSheetMetalGeometry, it is because
           they do not collide with the sheet metal model and so should be shown as error entities and deleted. */
        if (!isQueryEmpty(context, errorEntities))
        {
            setErrorEntities(context, topLevelId, {
                    "entities" : errorEntities
            });
            opDeleteBodies(context, id + "deleteErrorEntities", { "entities" : qUnion(errorEntities, formedErrorSketchBodies) });
        }

    }, { filterVertices: false });

//////////////////// FACE PATTERN ENTITY SORTING ////////////////////

function separateEntitiesForFacePattern(context is Context, topLevelId is Id, definition is map) returns map
{
    const definitionEntities = getSMDefinitionEntities(context, definition.entities);

    const definitionFacesQ = qEntityFilter(qUnion(definitionEntities), EntityType.FACE);
    const definitionFaces = evaluateQuery(context, definitionFacesQ);

    // Allow the user to select definition edges and vertices that lie on a definition face, but ignore them internally.
    // They will be successfully patterned by the wall pattern of the definition face.
    const definitionFaceEdges = qAdjacent(definitionFacesQ, AdjacencyType.EDGE, EntityType.EDGE);
    const originalDefinitionEdges = qEntityFilter(qUnion(definitionEntities), EntityType.EDGE);
    const definitionEdgesQ = qSubtraction(originalDefinitionEdges, definitionFaceEdges);
    const definitionEdges = evaluateQuery(context, definitionEdgesQ);

    // Cannot pattern two sided edges (joints)
    const twoSidedEdges = evaluateQuery(context, qEdgeTopologyFilter(definitionEdgesQ, EdgeTopology.TWO_SIDED));
    if (twoSidedEdges != [])
    {
        var errorEntities = getSelectionsForSMDefinitionEntities(context, qUnion(twoSidedEdges), definition.entities);
        setErrorEntities(context, topLevelId, { "entities" : errorEntities });
        throw regenError(ErrorStringEnum.SHEET_METAL_FACE_PATTERN_NO_JOINT, ["entities"]);
    }

    // Vertices (fillets/chamfers/reliefs) cannot be face patterned by themselves
    const definitionFaceVertices = qAdjacent(definitionFacesQ, AdjacencyType.VERTEX, EntityType.VERTEX);
    const definitionEdgeVertices = qAdjacent(definitionEdgesQ, AdjacencyType.VERTEX, EntityType.VERTEX);
    const allAbsorbedVertices = qUnion([definitionFaceVertices, definitionEdgeVertices]);
    const originalDefinitionVertices = qEntityFilter(qUnion(definitionEntities), EntityType.VERTEX);
    const definitionVerticesQ = qSubtraction(originalDefinitionVertices, allAbsorbedVertices);
    const definitionVertices = evaluateQuery(context, definitionVerticesQ);

    const holeToolMap = evSheetMetalHoleToolBodies(context, { "sheetMetalHoleFaces" : qAttributeFilter(definition.entities, asHoleAttribute({}))});
    const formToolMap = evSheetMetalFormToolBodies(context, { "sheetMetalFormFaces" : definition.entities});
    if (definition.filterVertices && (definitionFaces == [] && definitionEdges == [] && holeToolMap.sheetMetalHoleToolBodies == [] && formToolMap == {}) ||
        (!definition.filterVertices && definitionVertices != []))
    {
        //error out if we have vertices, or when we do allow vertices if there's no other entities to pattern left
        var errorEntities = getSelectionsForSMDefinitionEntities(context, qUnion(definitionVertices), definition.entities);
        setErrorEntities(context, topLevelId, { "entities" : errorEntities });
        throw regenError(ErrorStringEnum.SHEET_METAL_FACE_PATTERN_NO_VERTEX, ["entities"]);
    }

    return {
        "definitionWalls" : definitionFaces,
        "definitionEdges" : definitionEdges,
        "holeToolMap" : holeToolMap,
        "formToolMap" : formToolMap
    };
}

//////////////////// WALL PATTERN ////////////////////

/**
 * Execute a sheet metal wall pattern on the specified faces of the sheet metal definition sheet body.
 * @returns {{
 *     @field modifiedEntities {Query} : entities created or modified by the wall pattern
 *     @field deletedAttributes {array} : attributes deleted by the wall pattern
 * }}
 */
function sheetMetalWallPattern(context is Context, topLevelId is Id, id is Id, definitionFaces is array, definition is map,
        attributeIdCounter is box) returns map
{
    const definitionFacesQ = qUnion(definitionFaces);

    checkMirrorBodiesWillBuild(context, topLevelId, definitionFacesQ, definition);

    var modelIdToModelAndEntities = groupEntitiesByModelAttribute(context, definitionFaces);

    var modifiedEntities = [];
    var deletedAttributes = [];
    var booleanWasNoOp = true;
    for (var modelIdToModelAndEntitiesPair in modelIdToModelAndEntities)
    {
        // Pattern the faces of the given model
        const modelAttribute = modelIdToModelAndEntitiesPair.value.modelAttribute;
        const patternModelId = id + modelIdToModelAndEntitiesPair.key;
        const faces = qUnion(modelIdToModelAndEntitiesPair.value.entities);
        const patternResult = patternWallsForModel(context, topLevelId, patternModelId, definition, modelAttribute,
                faces, attributeIdCounter);

        // Store the results for later use
        modifiedEntities = append(modifiedEntities, patternResult.modifiedEntities);
        deletedAttributes = concatenateArrays([deletedAttributes, patternResult.deletedAttributes]);
        booleanWasNoOp = (booleanWasNoOp && patternResult.booleanWasNoOp);
    }

    return {
        "modifiedEntities" : qUnion(modifiedEntities),
        "deletedAttributes" : deletedAttributes,
        "booleanWasNoOp" : booleanWasNoOp
    };
}

/**
 * Pattern the faces of one sheet metal model.  Return a map containing modified entities and deleted attributes.
 */
function patternWallsForModel(context is Context, topLevelId is Id, id is Id, definition is map,
        modelAttribute is SMAttribute, faces is Query, attributeIdCounter is box) returns map
{
    const modelId = modelAttribute.attributeId;
    var allBodiesOfModel = qAttributeQuery(asSMAttribute({
                "objectType" : SMObjectType.MODEL,
                "attributeId" : modelId
            }));

    const initialData = getInitialEntitiesAndAttributes(context, allBodiesOfModel);

    // Collect attributes preset on the underlying sheet bodies of the seeds
    const facesAndSurrounding = qUnion([
                faces,                                    // Faces
                qAdjacent(faces, AdjacencyType.EDGE, EntityType.EDGE),    // Edges
                qAdjacent(faces, AdjacencyType.VERTEX, EntityType.VERTEX) // Vertices
            ]);

    const smTrackingAndAttributeByType = createSMTrackingAndAttributeByType(context, facesAndSurrounding);
    const holeTrackingAndAttribute = createHoleTrackingAndAttribute(context, facesAndSurrounding);
    const edgesHeldForUnfoldTracking = createEdgesHeldForUnfoldTracking(context, faces);
    var adjustForRips = isAtVersionOrLater(context, FeatureScriptVersionNumber.V706_SM_PATTERN_RIP) && isPartPattern(definition.patternType);
    const limitingDataForRipsAtRisk = (adjustForRips) ? collectLimitingDataForRipsAtRisk(context, faces, qOwnerBody(definition.entities)) : [];

    // Extracted the selected faces into isolated sheet bodies. Connected selected faces will stay connected as a single
    // body with multiple faces.
    const extractId = id + "extractFaces";
    opExtractSurface(context, extractId, { "faces" : faces });
    if (adjustForRips)
        adjustForLostRips(context, topLevelId, id, limitingDataForRipsAtRisk);

    // Pattern the seeds and delete them
    const createdBodies = patternSeeds(context, id, qCreatedBy(extractId, EntityType.BODY), definition);
    const numCreatedBodies = size(evaluateQuery(context, createdBodies));

    // Assign necessary attributes for created sheets to be built out as sheet metal
    // Assign these attributes before the patterned bodies are booleaned back onto owner sheet model
    reapplyJointAttributes(context, topLevelId, smTrackingAndAttributeByType, attributeIdCounter);
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2257_REAPPLY_CUTTING_TOOL_BODY_IDS))
    {
        // Need to call reapplyWallAttributes() on the intermediate patterned wall(s) as well so that the boolean of
        // the patterned wall(s) sees the cuttingToolBodyIds on the wall(s) and carries them forward during the union.
        reapplyWallAttributes(context, topLevelId, smTrackingAndAttributeByType, attributeIdCounter);
    }
    reapplyHoleAttributes(context, topLevelId, holeTrackingAndAttribute, attributeIdCounter);
    if (isFacePattern(definition.patternType))
    {
        var thickness = 0 * meter;
        if (modelAttribute.frontThickness != undefined)
           thickness += modelAttribute.frontThickness.value;
        if (modelAttribute.backThickness != undefined)
           thickness += modelAttribute.backThickness.value;
        adjustTargetAlignment(context, topLevelId, id, smTrackingAndAttributeByType[SMObjectType.JOINT], allBodiesOfModel, thickness);
    }

    // Apply booleans based on options set in the definition.
    // Face patterns should always boolean, user has control of part pattern boolean.
    const booleanWasNoOp = booleanSMBodiesIfNecessary(context, topLevelId, id + "boolean", faces, createdBodies,
            allBodiesOfModel, definition);

    // Apply model attribute to bodies that did not manage to boolean
    const numRemainingBodies = size(evaluateQuery(context, createdBodies));
    if (numRemainingBodies > 0)
    {
        if (isPartPattern(definition.patternType))
        {
            setAttribute(context, { "entities" : createdBodies, "attribute" : modelAttribute });
            allBodiesOfModel = qUnion([allBodiesOfModel, createdBodies]);
        }
        else
        {
            const errorEntities = qUnion([createdBodies, qOwnedByBody(createdBodies, EntityType.EDGE)]);
            setErrorEntities(context, topLevelId, { "entities" : errorEntities });
            if (numRemainingBodies == numCreatedBodies) // No bodies attached
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_FACE_PATTERN_FLOATING_WALL);
            }
            else // some bodies attached
            {
                reportFeatureInfo(context, topLevelId, ErrorStringEnum.SHEET_METAL_FACE_PATTERN_PARTIAL_FLOATING_WALL);
                opDeleteBodies(context, id + "deleteFloating", { "entities" : createdBodies });
            }
        }
    }

    // Assign association attributes and gather modified entities
    const toUpdate = assignSMAttributesToNewOrSplitEntities(context, allBodiesOfModel, initialData, id);

    fixJointAttributes(context, id, qEntityFilter(toUpdate.modifiedEntities, EntityType.EDGE), attributeIdCounter);

    // Wall attributes and corner attributes mut be applied after booleaning bodies, applying model attributes and
    // fixing joint attributes.  See function headers for details.
    // This second call to reapplyWallAttributes() is needed to get information of the walls which have been merged.
    const oldWallIdToNewWallIdsByBody = reapplyWallAttributes(context, topLevelId, smTrackingAndAttributeByType, attributeIdCounter);
    reapplyCornerAttributes(context, topLevelId, smTrackingAndAttributeByType, oldWallIdToNewWallIdsByBody, attributeIdCounter);
    reapplyHoldEdgeForUnfoldAttribute(context, edgesHeldForUnfoldTracking);

    return {
        "modifiedEntities" : toUpdate.modifiedEntities,
        "deletedAttributes" : toUpdate.deletedAttributes,
        "booleanWasNoOp" : booleanWasNoOp
    };
}

/**
 * BEL-80023: Fail the feature if any sheet metal definition faces to mirror are coplanar with the mirror plane, and our
 * feature is a face mirror or a part mirror set to ADD.  If we allow these cases through, the seed will consume the
 * antiparallel patterned face, and the user will see a passing feature with no change.
 */
function checkMirrorBodiesWillBuild(context is Context, topLevelId is Id, faces is Query, definition is map)
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V715_SM_PATTERN_FAIL_MIRROR))
        return;

    const usesBoolean = isMirror(definition.patternType) &&
            (isFacePattern(definition.patternType) || definition.operationType == NewBodyOperationType.ADD);
    if (!usesBoolean)
        return;

    const parallelSeeds = qParallelPlanes(faces, definition.mirrorPlaneCalculated);
    for (var seed in evaluateQuery(context, parallelSeeds))
    {
        const seedPlane = evPlane(context, { "face" : seed });
        if (versionedCoplanarPlanes(context, definition.mirrorPlaneCalculated, seedPlane))
        {
            setErrorEntities(context, topLevelId, { "entities" : qUnion([seed, qAdjacent(seed, AdjacencyType.EDGE, EntityType.EDGE)]) });
            throw regenError(ErrorStringEnum.BOOLEAN_INVALID, ["entities"]);
        }
    }
}

/**
 * Deprecated in favor of a server side implementation.
 *
 * Create an array of maps containing a tracking query and hole attribute for each entity of `definitionTopology` which
 * has a hole attribute.
 */
function createHoleTrackingAndAttribute(context is Context, definitionTopology is Query) returns array
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V732_HOLE_PROPAGATE_EDGE))
        return [];

    var holeTrackingAndAttribute = [];
    const holeCandidates = qEntityFilter(definitionTopology, EntityType.EDGE);
    for (var entity in evaluateQuery(context, holeCandidates))
    {
        const attributes = getHoleAttributes(context, entity);
        if (size(attributes) != 0)
        {
            const attribute = attributes[0];
            holeTrackingAndAttribute = append(holeTrackingAndAttribute, {
                        "tracking" : startTracking(context, entity),
                        "attribute" : attribute
                    });
        }
    }
    return holeTrackingAndAttribute;
}

/**
 * Deprecated in favor of a server side implementation.
 *
 * Reapply hole attributes to patterned sheet metal entities.
 */
function reapplyHoleAttributes(context is Context, topLevelId is Id, holeTrackingAndAttribute is array, attributeIdCounter is box)
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V732_HOLE_PROPAGATE_EDGE))
        return;

    for (var trackingAndAttribute in holeTrackingAndAttribute)
    {
        var newEdges = evaluateQuery(context, trackingAndAttribute.tracking);
        var attribute = trackingAndAttribute.attribute;
        for (var newEdge in newEdges)
        {
            attribute.attributeId = toAttributeId(topLevelId + attributeIdCounter[]);
            attributeIdCounter[] += 1;
            setAttribute(context, { "entities" : newEdge, "attribute" : attribute });
        }
    }
}

/**
 * Create tracking queries for edges of `definitionFaces` which have been marked to be held during unfold.
 */
function createEdgesHeldForUnfoldTracking(context is Context, definitionFaces is Query) returns Query
{
    var edgesTracked = [];
    for (var laminarEdge in evaluateQuery(context, definitionFaces->qAdjacent(AdjacencyType.EDGE, EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.ONE_SIDED)))
    {
        if (getAttribute(context, {"entity" : laminarEdge, "name" : "holdEdgeForUnfold"}) == true)
        {
            edgesTracked = append(edgesTracked, startTracking(context, laminarEdge));
        }
    }
    return qUnion(edgesTracked);
}

/**
 * Reapply the `holdEdgeForUnfold` attribute to patterned sheet metal definition edges.
 */
function reapplyHoldEdgeForUnfoldAttribute(context is Context, edgesHeldForUnfoldTracking is Query)
{
    for (var newEdge in evaluateQuery(context, edgesHeldForUnfoldTracking->qEntityFilter(EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.ONE_SIDED)))
    {
        setAttribute(context, { "entities" : newEdge, "name" : "holdEdgeForUnfold", "attribute" : true });
    }
}

/**
 * Create an array of maps containing a tracking query and `objectType` attribute for each entity of `entities` which has an `objectType`
 * attribute. For CORNER objects, also returns the result of `evCornerType`. If `required` is set to `true`, enforce that
 * every entity of `entities` must have an `objectType` attribute.
 */
function createSMTrackingAndAttribute(context is Context, entities is Query, objectType is SMObjectType) returns array
{
    var smTrackingAndAttribute = [];
    for (var entity in evaluateQuery(context, entities))
    {
        const attributes = getSmObjectTypeAttributes(context, entity, objectType);
        if (size(attributes) != 0)
        {
            var attribute = attributes[0];
            if (objectType != SMObjectType.MODEL)
            {
                // Remove controllingFeatureId and parameterIdInFeature from the source attribute (making sure to apply
                // the adjusted attribute to the original `entity`), and store the adjusted attribute.
                attribute = sanitizeControllingInformation(context, attribute, true);
            }
            // Store the original corner type for corner attributes
            var cornerType = undefined;
            if (objectType == SMObjectType.CORNER)
            {
                cornerType = evCornerType(context, { "vertex" : entity }).cornerType;
            }

            smTrackingAndAttribute = append(smTrackingAndAttribute, {
                        "tracking" : startTracking(context, entity),
                        "attribute" : attribute,
                        "cornerType" : cornerType
                    });
        }
    }
    return smTrackingAndAttribute;
}

/**
 * Use createSMTrackingAndAttribute to create a master map of SMObjectType -> smTrackingAndAttribute array for applicable
 * SMObjectTypes.
 */
function createSMTrackingAndAttributeByType(context is Context, definitionTopology is Query) returns map
{
    var smTrackingAndAttributeByType = {};
    smTrackingAndAttributeByType[SMObjectType.WALL] =
        createSMTrackingAndAttribute(context, qEntityFilter(definitionTopology, EntityType.FACE), SMObjectType.WALL);

    const jointQ = qUnion([qEntityFilter(definitionTopology, EntityType.EDGE), qGeometry(definitionTopology, GeometryType.CYLINDER)]);
    smTrackingAndAttributeByType[SMObjectType.JOINT] = createSMTrackingAndAttribute(context, jointQ, SMObjectType.JOINT);

    smTrackingAndAttributeByType[SMObjectType.CORNER] =
        createSMTrackingAndAttribute(context, qEntityFilter(definitionTopology, EntityType.VERTEX), SMObjectType.CORNER);

    return smTrackingAndAttributeByType;
}

/**
 * Reapply wall attributes to patterned sheet metal faces.  Return `oldWallIdToNewWallIdsByBody`: a mapping from
 * old wall ids -> body -> a set of new wall ids for all old wall ids that appear in a corner break attribute (used in
 * reapplyCornerAttributes)
 *
 * Should be called after sheet boolean for oldWallIdToNewWallIdsByBody to be correct.
 */
function reapplyWallAttributes(context is Context, topLevelId is Id, smTrackingAndAttributeByType is map, attributeIdCounter is box) returns map
{
    // Collect wallIds present in a corner break attributes.
    var oldWallIdToNewWallIdsByBody = {};
    for (var trackingAndAttribute in smTrackingAndAttributeByType[SMObjectType.CORNER])
    {
        const attribute = trackingAndAttribute.attribute;
        if (attribute.cornerBreaks != undefined)
        {
            for (var cornerBreak in attribute.cornerBreaks)
            {
                oldWallIdToNewWallIdsByBody[cornerBreak.value.wallId] = {};
            }
        }
    }

    // Apply wall attributes to patterned faces and augment oldWallIdToNewWallIdsByBody
    for (var trackingAndAttribute in smTrackingAndAttributeByType[SMObjectType.WALL])
    {
        // Filter the tracking query, otherwise the tracking query will also resolve to the extracted bodies
        var newFaces = evaluateQuery(context, qEntityFilter(trackingAndAttribute.tracking, EntityType.FACE));
        var attribute = trackingAndAttribute.attribute;
        const oldWallId = attribute.attributeId;
        for (var newFace in newFaces)
        {
            var newWallId;
            const existingWallAttribute = getWallAttribute(context, newFace);
            if (existingWallAttribute != undefined)
            {
                // If patterned face attaches back to sheet metal model, there will already be a wall attribute. Patterned
                // wall could have attached to any face of the sheet metal model; newWallId is not necessarily the same
                // as oldWallId.
                newWallId = existingWallAttribute.attributeId;
            }
            else
            {
                newWallId = toAttributeId(topLevelId + attributeIdCounter[]);
                attributeIdCounter[] += 1;

                // Apply new wall id
                attribute.attributeId = newWallId;
                setAttribute(context, { "entities" : newFace, "attribute" : attribute });
            }

            // Fill mapping from old wall ids to new wall ids if this wallId is required for corner break
            if (oldWallIdToNewWallIdsByBody[oldWallId] != undefined)
            {
                var body = evaluateQuery(context, qOwnerBody(newFace))[0];
                if (oldWallIdToNewWallIdsByBody[oldWallId][body] == undefined)
                    oldWallIdToNewWallIdsByBody[oldWallId][body] = {};
                oldWallIdToNewWallIdsByBody[oldWallId][body][newWallId] = true;
            }
        }
    }

    return oldWallIdToNewWallIdsByBody;
}

/**
 * Reapply joint attributes to patterned sheet metal edges.
 *
 * Should be called before sheet boolean so that the bend along a patterned edge is only given one bend attribute.
 * TODO: Investigate whether we should do this after the boolean and be smart about cases where we need one bend
 *       attribute or many.
 */
function reapplyJointAttributes(context is Context, topLevelId is Id, smTrackingAndAttributeByType is map, attributeIdCounter is box)
{
    for (var trackingAndAttribute in smTrackingAndAttributeByType[SMObjectType.JOINT])
    {
        var newEdges = evaluateQuery(context, trackingAndAttribute.tracking);
        var attribute = trackingAndAttribute.attribute;
        for (var newEdge in newEdges)
        {
            attribute.attributeId = toAttributeId(topLevelId + attributeIdCounter[]);
            attributeIdCounter[] += 1;
            setAttribute(context, { "entities" : newEdge, "attribute" : attribute });
        }
    }
}

/**
 * Find the new wallId from oldWallIdToNewWallIdsByBody given the old wall id, the body, and the surrounding wall ids of a vertex
 */
function mapToNewWallId(context is Context, oldWallId is string, body is Query, surroundingWallIds is array,
        oldWallIdToNewWallIdsByBody is map)
{
    if (oldWallIdToNewWallIdsByBody[oldWallId][body] != undefined)
    {
        for (var wallId in surroundingWallIds)
        {
            // Take the first matched wall id. This is deterministic from the evaluation of qAdjacent
            if (oldWallIdToNewWallIdsByBody[oldWallId][body][wallId] != undefined)
            {
                return wallId;
            }
        }
    }
    return undefined;
}

/**
 * Create a set of corner breaks for the specified vertex based on the corner breaks of the seed vertex (if any) and the
 * breaks already existing on the vertex (if the patterned vertex is being merged into an existing vertex with corner
 * breaks).
 */
function adjustCornerBreaks(context is Context, vertex is Query, originalCornerBreaks, existingCornerBreaks,
        oldWallIdToNewWallIdsByBody is map) returns map
{
    var madeChanges = false;
    var adjustedCornerBreaks = [];
    var existingWallIds = {};
    if (existingCornerBreaks != undefined)
    {
        adjustedCornerBreaks = existingCornerBreaks;
        for (var cornerBreak in existingCornerBreaks)
        {
            existingWallIds[cornerBreak.value.wallId] = true;
        }
    }

    // Remap original corner breaks into adjustedCornerBreaks
    if (originalCornerBreaks != undefined)
    {
        var body = evaluateQuery(context, qOwnerBody(vertex))[0];
        const surroundingWalls = evaluateQuery(context, qAdjacent(vertex, AdjacencyType.VERTEX, EntityType.FACE));
        const surroundingWallIds = mapArray(surroundingWalls, function(wall) {
                    return getWallAttribute(context, wall).attributeId;
                });
        for (var originalCornerBreak in originalCornerBreaks)
        {
            // Only remap corner breaks onto walls that were created or altered by this pattern
            const adjustedWallId = mapToNewWallId(context, originalCornerBreak.value.wallId, body, surroundingWallIds,
                    oldWallIdToNewWallIdsByBody);
            if (adjustedWallId != undefined && existingWallIds[adjustedWallId] == undefined)
            {
                var adjustedCornerBreak = originalCornerBreak;
                adjustedCornerBreak.value.wallId = adjustedWallId;
                adjustedCornerBreaks = append(adjustedCornerBreaks, adjustedCornerBreak);
                existingWallIds[adjustedWallId] = true;
                madeChanges = true;
            }
        }
    }

    return {
            "cornerBreaks" : adjustedCornerBreaks,
            "madeChanges" : madeChanges
    };
}

/**
 * Adjust a corner attribute from the seed vertex to fit the patterned vertex.  Apply the attribute if necessary.
 */
function adjustAndApplyCornerAttribute(context is Context, topLevelId is Id, originalAttribute is SMAttribute,
        originalCornerType is SMCornerType, newVertex is Query, oldWallIdToNewWallIdsByBody is map, attributeIdCounter is box)
{
    var newAttribute = {};
    var applyAttribute = false;

    const existingAttribute = getCornerAttribute(context, newVertex);

    // Apply corner overrides if corner matches original corner
    const cornerType = evCornerType(context, { "vertex" : newVertex }).cornerType;
    if (cornerType != SMCornerType.NOT_A_CORNER && cornerType == originalCornerType)
    {
        // Null out unrelated info.  It would be harder to grab a positive copy of the corner override info because it
        // can manifest as a number of different fields depending on what type of override is present.
        var attributeCopy = originalAttribute;
        attributeCopy.attributeId = undefined;
        attributeCopy.objectType = undefined;
        attributeCopy.cornerBreaks = undefined;

        newAttribute = mergeMaps(attributeCopy, newAttribute);
        applyAttribute = true;
    }
    else if (existingAttribute != undefined && existingAttribute.cornerStyle != undefined)
    {
        // If there was an existing corner override and we can't take corner override from the patterned attribute,
        // remove the existing override and return to default corner geometry.
        applyAttribute = true;
        // TODO: check if the existing override is still valid, and keep it if it is
    }

    // Decide what corner breaks the new attribute needs
    const existingCornerBreaks = existingAttribute?.cornerBreaks;
    const cornerBreakReturn = adjustCornerBreaks(context, newVertex, originalAttribute.cornerBreaks, existingCornerBreaks,
            oldWallIdToNewWallIdsByBody);
    if (size(cornerBreakReturn.cornerBreaks) > 0)
    {
        newAttribute.cornerBreaks = cornerBreakReturn.cornerBreaks;
    }
    applyAttribute = applyAttribute || cornerBreakReturn.madeChanges;

    // Only apply attribute if it has content
    if (applyAttribute)
    {
        // We found an existing corner override, and the corner override from the pattern could not be applied, and
        // there are no corner breaks
        if (size(newAttribute) == 0)
        {
            removeAttributes(context, { "entities" : newVertex, "attributePattern" : existingAttribute });
        }
        else
        {
            newAttribute.objectType = SMObjectType.CORNER;
            newAttribute.attributeId = toAttributeId(topLevelId + attributeIdCounter[]);
            attributeIdCounter[] += 1;
            if (existingAttribute != undefined)
            {
                replaceSMAttribute(context, existingAttribute, newAttribute as SMAttribute);
            }
            else
            {
                setAttribute(context, { "entities" : newVertex, "attribute" : newAttribute as SMAttribute });
            }
        }
    }
}

/**
 * Reapply corner attributes to patterned sheet metal vertices. Must be done after applying model attributes to bodies
 * and calling fixJointAttributes or classifying corner types will fail.
 * oldWallIdToNewWallIdsByBody is calculated during reapplyWallAttributes.
 */
function reapplyCornerAttributes(context is Context, topLevelId is Id, smTrackingAndAttributeByType is map,
        oldWallIdToNewWallIdsByBody is map, attributeIdCounter is box)
{
    for (var trackingAndAttribute in smTrackingAndAttributeByType[SMObjectType.CORNER])
    {
        const originalAttribute = trackingAndAttribute.attribute;
        var newVertices = evaluateQuery(context, trackingAndAttribute.tracking);
        for (var newVertex in newVertices)
        {
            adjustAndApplyCornerAttribute(context, topLevelId, trackingAndAttribute.attribute,
                    trackingAndAttribute.cornerType, newVertex, oldWallIdToNewWallIdsByBody, attributeIdCounter);
        }
    }
}

/**
 * Boolean bodies onto master sheet body if necessary.
 * Returns true if boolean was executed, but was a no-op.
 */
function booleanSMBodiesIfNecessary(context is Context, topLevelId is Id, id is Id, seedFaces is Query, bodiesToAttach is Query,
        allBodiesOfModel is Query, definition is map) returns boolean
{
    var needsBoolean = false;
    var booleanNoOp = false;
    var tools = bodiesToAttach;
    var targets;
    var targetsAndToolsNeedGrouping = true;
    if (isFacePattern(definition.patternType))
    {
        // Always attempt to boolean the face pattern
        needsBoolean = true;
        targets = allBodiesOfModel;
    }
    else if (isPartPattern(definition.patternType))
    {
        // NEW will continue with needsBoolean = false
        // ADD should boolean as appropriate
        // REMOVE and INTERSECT should fail
        if (definition.operationType == NewBodyOperationType.ADD)
        {
            needsBoolean = true;
            if (definition.defaultScope)
            {
                targets = allBodiesOfModel;
            }
            else
            {
                targets = qNothing();

                if (!isQueryEmpty(context, definition.booleanScope))
                {
                    if (queryContainsNonSheetMetal(context, definition.booleanScope))
                    {
                        var nonSheetMetal = separateSheetMetalQueries(context, definition.booleanScope).nonSheetMetalQueries;
                        setErrorEntities(context, topLevelId, { "entities" : qUnion([bodiesToAttach, nonSheetMetal]) });
                        throw regenError(ErrorStringEnum.SHEET_METAL_ADD_WRONG_MODEL, ["booleanScope"]);
                    }
                    var targetsArr = getSMDefinitionEntities(context, definition.booleanScope);
                    targets = qUnion(targetsArr);
                    var scopeFromCorrectModel = qIntersection([targets, allBodiesOfModel]);
                    if (size(targetsArr) != size(evaluateQuery(context, scopeFromCorrectModel)))
                    {
                        var scopeFromIncorrectModel = qSubtraction(targets, scopeFromCorrectModel);
                        setErrorEntities(context, topLevelId, { "entities" : qUnion([bodiesToAttach, scopeFromIncorrectModel]) });
                        throw regenError(ErrorStringEnum.SHEET_METAL_ADD_WRONG_MODEL, ["booleanScope"]);
                    }
                }
            }

            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V712_SKIP_TARGET_BOOLEAN))
            {
                // mimic subfeatureToolsTargets(...) in boolean.fs by adding the seed bodies to tools and setting grouping as appropriate
                var seedBodies = qOwnerBody(seedFaces);
                tools = qUnion([seedBodies, tools]);
                targets = qSubtraction(targets, seedBodies);
                if (isQueryEmpty(context, targets))
                {
                    targetsAndToolsNeedGrouping = false;
                }
            }

            if (targetsAndToolsNeedGrouping && isQueryEmpty(context, targets))
            {
                setErrorEntities(context, topLevelId, { "entities" : bodiesToAttach });
                throw regenError(ErrorStringEnum.BOOLEAN_NEED_ONE_SOLID, ["booleanScope"]);
            }
        }
        else if (definition.operationType == NewBodyOperationType.REMOVE || definition.operationType == NewBodyOperationType.INTERSECT)
        {
             setErrorEntities(context, topLevelId, { "entities" : bodiesToAttach });
             throw regenError(ErrorStringEnum.SHEET_METAL_PATTERN_DISABLED_BOOLEANS, ["entities", "operationType"]);
        }
    }

    if (needsBoolean)
    {
        // Boolean patterned sheet bodies back onto master body
        try
        {
            opBoolean(context, id, {
                        "tools" : tools,
                        "targets" : targets,
                        "targetsAndToolsNeedGrouping" : targetsAndToolsNeedGrouping,
                        "operationType" : BooleanOperationType.UNION,
                        "allowSheets" : true
                    });

            const booleanInfo = getFeatureInfo(context, id);
            if (booleanInfo != undefined && booleanInfo == ErrorStringEnum.BOOLEAN_UNION_NO_OP)
                booleanNoOp = true;
        }
        catch (error)
        {
            // Error entities will show boolean bodies and error edges.
            setErrorEntities(context, topLevelId, { "entities" : bodiesToAttach });
            processSubfeatureStatus(context, topLevelId, {"subfeatureId" : id, "propagateErrorDisplay" : true});
            throw error;
        }
    }

    return booleanNoOp;
}

/**
 * Remove any inappropriate joint attributes
 * Add rip attributes to blank two sided edges
 */
function fixJointAttributes(context is Context, topLevelId is Id, edges is Query, attributeIdCounter is box)
{
    for (var edge in evaluateQuery(context, edges))
    {
        const attribute = getJointAttribute(context, edge);
        var hasAttribute = (attribute != undefined);

        if (hasAttribute && !isEntityAppropriateForAttribute(context, edge, attribute))
        {
            // We do not need to log these removals as deletedAttributes in updateSheetMetalGeometry(...) because any
            // attributes we find and delete here are just patterned copied attributes that have not been logged with
            // a previous updateSheetMetalGeometry(...)
            removeAttributes(context, { "entities" : edge, "attributePattern" : attribute });
            hasAttribute = false;
        }
        if (!hasAttribute && edgeIsTwoSided(context, edge))
        {
            const newRip = createRipAttribute(context, edge, toAttributeId(topLevelId + attributeIdCounter[]), SMJointStyle.EDGE, {});
            if (isEntityAppropriateForAttribute(context, edge, newRip))
            {
                setAttribute(context, {
                            "entities" : edge,
                            "attribute" : newRip
                        });
                attributeIdCounter[] += 1;
            }
        }
    }
}

/**
* If patterned copy of a bend edge does not coincide with a laminar edge in targets,
* look for a laminar edge within thickness and adjust it to coincide
*/
function adjustTargetAlignment(context is Context, topLevelId is Id, id is Id,
                                jointTrackingAndAttributes is array, allBodiesOfModel is Query, thickness)
{
    // organize laminar linear edges of the target for quick lookup
    const lineEdgesByLineDirection = putLaminarLineEdgesToBuckets(context, allBodiesOfModel);

    var edgeLimitOptions = [];
    var edgesToExtend = {}; // for quick check to avoid duplication
    for (var jointData in jointTrackingAndAttributes)
    {
        // For now do adjustment only for bends
        if (jointData.attribute.jointType == undefined ||
            jointData.attribute.jointType.value != SMJointType.BEND)
            continue;
        for (var instanceEdge in evaluateQuery(context, jointData.tracking))
        {
            var instanceFaceQ = qAdjacent(instanceEdge, AdjacencyType.EDGE, EntityType.FACE);
            if (size(evaluateQuery(context, instanceFaceQ)) != 1)
                continue;
            var edgeWithinThickness = getEdgeWithinThickness(context, {
                    "instanceEdge" : instanceEdge,
                    "instanceFace" : instanceFaceQ,
                    "lineEdgesByLineDirection" : lineEdgesByLineDirection,
                    "thickness" : thickness});

            if (edgeWithinThickness != undefined && edgesToExtend[edgeWithinThickness] == undefined)
            {
                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V744_SM_FLANGE_PATTERN_EDGE_CHANGE))
                {
                    edgeLimitOptions = append(edgeLimitOptions, {
                            "edge" : edgeWithinThickness.edge,
                            "face" : qAdjacent(edgeWithinThickness.edge, AdjacencyType.EDGE, EntityType.FACE),
                            "offset" : edgeWithinThickness.offset
                    });
                }
                else
                {
                    edgeLimitOptions = append(edgeLimitOptions, {
                            "edge" : edgeWithinThickness.edge,
                            "limitEntity" : instanceFaceQ
                    });
                }
                edgesToExtend[edgeWithinThickness.edge] = true;
            }
        }
    }
    if (size(edgesToExtend) > 0)
    {
        try
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V744_SM_FLANGE_PATTERN_EDGE_CHANGE))
            {
                sheetMetalEdgeChangeCall(context, id + "adjustTarget", qUnion(keys(edgesToExtend)), {
                        "edgeChangeOptions" : edgeLimitOptions
                });
            }
            else
            {
                sheetMetalExtendSheetBodyCall(context, id + "adjustTarget", {
                        "extendMethod" : ExtendSheetBoundingType.EXTEND_TO_SURFACE,
                        "entities" : qUnion(keys(edgesToExtend)),
                        "edgeLimitOptions" : edgeLimitOptions
                });
            }
        }
        if (getFeatureError(context, id + "adjustTarget") != undefined)
        {
            setErrorEntities(context, topLevelId, {
                    "entities" : qUnion(keys(edgesToExtend))
            });
            throw regenError(ErrorStringEnum.SHEET_METAL_REBUILD_ERROR);
        }
    }
}
/**
 *   @param arg {{
 *      @field instanceEdge{Query}
 *      @field instanceFace{Query}
 *      @field lineEdgesByLineDirection{map} : generated by putLaminarLineEdgesToBuckets
 *      @field thickness : sheet metal model thickness
 *   }}
 *
 *   @returns {{
 *      @field edge{Query} : the edge within thickness
 *      @field offset{ValueWithUnits} : offset that the edge should be extended (undefined before V744)
 *   }}
**/
function getEdgeWithinThickness(context is Context, args is map)
{
    var edgeLine = try silent(evLine(context, {
                    "edge" : args.instanceEdge
                    }));
    if (edgeLine == undefined)
        return undefined;

    var normalAtInstanceEdge = evFaceNormalAtEdge(context, {
                        "edge" : args.instanceEdge,
                        "face" : args.instanceFace,
                        "parameter" : 0.5
                        });

    var bucketArr = lineDirectionBuckets(edgeLine.direction);
    var edgeWithinThickness;
    var seenEdges = {};
    for (var idx in bucketArr)
    {
        var candidateEdges = args.lineEdgesByLineDirection[idx];
        if (candidateEdges == undefined)
        {
            continue;
        }
        for (var edgeAndLine in candidateEdges)
        {
            if (seenEdges[edgeAndLine.edge] == true)
                continue;
            seenEdges[edgeAndLine.edge] = true;
            if (!parallelVectors(edgeLine.direction, edgeAndLine.line.direction))
                continue;

            var distanceResult = evDistance(context, {
                    "side0" : args.instanceEdge,
                    "side1" : edgeAndLine.edge,
                    "arcLengthParameterization" : false
            });
            if (distanceResult.distance < TOLERANCE.zeroLength * meter)
            {
                // This candidate edge is already touching. Nothing should be extended.
                return undefined;
            }
            if (distanceResult.distance > (TOLERANCE.zeroLength * meter + args.thickness))
            {
                // This candidate edge is too far away.
                continue;
            }

            var diff = distanceResult.sides[0].point - distanceResult.sides[1].point;
            if (!perpendicularVectors(edgeLine.direction, diff))
            {
                // We know the edge and candidate edge are parallel, if the diff is not perpendicular to the edge direction,
                // then the edges are lying end-to-end (possibly with slight misalignment); extension cannot make them meet.
                continue;
            }

            var adjacentFaceQ = qAdjacent(edgeAndLine.edge, AdjacencyType.EDGE, EntityType.FACE);
            var normal = evFaceNormalAtEdge(context, {
                        "edge" : edgeAndLine.edge,
                        "face" : adjacentFaceQ,
                        "parameter" : 0.5
                    });
            if (parallelVectors(normalAtInstanceEdge, normal))
            {
                // Faces are parallel to each other. up-to-face extension would fail. Offset would form one long face rather than a joint.
                continue;
            }

            var offset;
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V744_SM_FLANGE_PATTERN_EDGE_CHANGE))
            {
                if (try silent(evPlane(context, { "face" : adjacentFaceQ })) == undefined)
                {
                    // Laminar edge is not attached to a plane.  Cannot safely extend by offset.
                    continue;
                }

                // tangent such that adjacentFace is 'left' of candidate edge with adjacentFace normal as 'up'
                var directionedTangent = evEdgeTangentLine(context, {
                            "edge" : edgeAndLine.edge,
                            "face" : adjacentFaceQ,
                            "parameter" : 0.5
                        });
                var extensionDirection = cross(directionedTangent.direction, normal);
                if (!parallelVectors(extensionDirection, diff))
                {
                    // ExtensionDirection does not match perpendicular path between edges; edges cannot be made to meet
                    // by extending the candidate edge.
                    continue;
                }
                offset = dot(extensionDirection, diff);
            }

            // All checks satisfied.
            if (edgeWithinThickness == undefined)
            {
                edgeWithinThickness = {
                    "edge" : edgeAndLine.edge,
                    "offset" : offset
                };
            }
            else   // do not extend if ambiguous
            {
                return undefined;
            }
        }
    }
    return edgeWithinThickness;
}

// Use this grid of upper hemisphere to hash direction vectors ( index of closest vector in the grid)
const cos30 = sqrt(3) * 0.5;
const sin30 = 0.5;
const DIRECTION_SET = [ vector(1, 0, 0), vector(cos30, sin30, 0), vector(sin30, cos30, 0),
                    vector(0, 1, 0), vector(-sin30, cos30, 0), vector(-cos30, sin30, 0),
                    vector(cos30, 0, sin30), vector(sin30, 0, cos30), vector(0, 0, 1),
                    vector(-cos30, 0, sin30), vector(-sin30, 0, cos30),
                    vector(cos30 * cos30, cos30 * sin30, sin30), vector(cos30 * sin30, cos30 * cos30,  sin30),
                    vector(0, cos30, sin30), vector(0, sin30, cos30),
                    vector(-cos30 * sin30, cos30 * cos30,  sin30), vector(-cos30 * cos30, cos30 * sin30, sin30),
                    vector(-cos30 * cos30, -cos30 * sin30, sin30), vector(-cos30 * sin30, -cos30 * cos30,  sin30),
                    vector(0, -cos30, sin30), vector(0, -sin30, cos30),
                    vector(cos30 * cos30, -cos30 * sin30, sin30), vector(cos30 * sin30, -cos30 * cos30,  sin30)
                    ];

const SET_SIZE = size(DIRECTION_SET);

function lineDirectionBuckets(direction is Vector) returns array
{
    var idxs = [];
    const cosTol = cos30;
    for (var i = 0; i < SET_SIZE; i += 1)
    {
        var dot = abs(dot(direction, DIRECTION_SET[i]));
        if (dot > cosTol)
        idxs = append(idxs, i);
    }
    return idxs;
}

function putLaminarLineEdgesToBuckets(context is Context, smBodies is Query) returns map
{
    var bucketsOfEdgeAndLine = {};
    const linearEdges = qGeometry(qOwnedByBody(smBodies, EntityType.EDGE), GeometryType.LINE);
    const oneSidedEdges = evaluateQuery(context, linearEdges->qEdgeTopologyFilter(EdgeTopology.ONE_SIDED));
    for (var edge in oneSidedEdges)
    {
        var line = evLine(context, {
                "edge" : edge
        });
        var bucketArr = lineDirectionBuckets(line.direction);
        for (var idx in bucketArr)
        {
            if (bucketsOfEdgeAndLine[idx] == undefined)
            {
                bucketsOfEdgeAndLine[idx] = [{"edge" : edge, "line" : line}];
            }
            else
            {
                bucketsOfEdgeAndLine[idx] = append(bucketsOfEdgeAndLine[idx], {"edge" : edge, "line" : line});
            }
        }
    }
    return bucketsOfEdgeAndLine;
}

function collectLimitingDataForRipsAtRisk(context is Context, faces is Query, bodiesOfSelection is Query) returns array
{
    var limitingDataArr = [];
    const allEdgesQ = qAdjacent(faces, AdjacencyType.EDGE, EntityType.EDGE);
    for (var edge in evaluateQuery(context, allEdgesQ))
    {
        var attributes = getSmObjectTypeAttributes(context, edge, SMObjectType.JOINT);
        if (size(attributes) != 1 || attributes[0].jointType == undefined ||
            attributes[0].jointType.value != SMJointType.RIP)   //process only RIPs
            continue;
        var facesIn = evaluateQuery(context, qIntersection([qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE), faces]));
        if (size(facesIn) != 1)  //RIP is at risk if only one bounding wall is included in the set
            continue;
        var limitingFace = getRipSideFace(context, edge, bodiesOfSelection, facesIn[0]);
        if (limitingFace == undefined)
            continue;

        limitingDataArr = append(limitingDataArr, { "edgeTracking" : startTracking(context, edge),
                                                    "limitingFace" : limitingFace
                                                    });
    }
    return limitingDataArr;
}

function adjustForLostRips(context is Context, topLevelId is Id, id is Id, limitingDataTracking is array)
{
    var edgeLimitOptions = [];
    var edgesToExtend = [];
    for (var data in limitingDataTracking)
    {
        var edges = evaluateQuery(context, data.edgeTracking);
        if (size(edges) != 1 || edgeIsTwoSided(context, edges[0]))
            continue;
        edgesToExtend = append(edgesToExtend, edges[0]);
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V744_SM_FLANGE_PATTERN_EDGE_CHANGE))
        {
            edgeLimitOptions = append(edgeLimitOptions, {
                    "edge" : edges[0],
                    "face" : qAdjacent(edges[0], AdjacencyType.EDGE, EntityType.FACE),
                    "replaceFace" : data.limitingFace
            });
        }
        else
        {
            edgeLimitOptions = append(edgeLimitOptions, {
                    "edge" : edges[0],
                    "limitEntity" : data.limitingFace
            });
        }
    }

    if (size(edgesToExtend) > 0)
    {
        try
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V744_SM_FLANGE_PATTERN_EDGE_CHANGE))
            {
                sheetMetalEdgeChangeCall(context, id + "adjustForRips", qUnion(edgesToExtend), {
                        "edgeChangeOptions" : edgeLimitOptions
                });
            }
            else
            {
                sheetMetalExtendSheetBodyCall(context, id + "adjustForRips", {
                        "extendMethod" : ExtendSheetBoundingType.EXTEND_TO_SURFACE,
                        "entities" : qUnion(edgesToExtend),
                        "edgeLimitOptions" : edgeLimitOptions
                });
            }
        }
        if (getFeatureError(context, id + "adjustForRips") != undefined)
        {
            setErrorEntities(context, topLevelId, {
                    "entities" : qUnion(edgesToExtend)
            });
            throw regenError(ErrorStringEnum.SHEET_METAL_REBUILD_ERROR, ["entities"]);
         }
    }
}

function getRipSideFace(context is Context, ripEdge is Query, inBodies is Query, nextToWall is Query)
{
    var edgeAssociationAttributes = getSMAssociationAttributes(context, ripEdge);
    if (size(edgeAssociationAttributes) != 1)
        return undefined;
    var associatedFacesQ = qEntityFilter(qAttributeQuery(edgeAssociationAttributes[0]), EntityType.FACE);
    var facesIn3dQ = qOwnedByBody(inBodies, EntityType.FACE);
    var candidateFaces = evaluateQuery(context, qIntersection([associatedFacesQ, facesIn3dQ]));

    var nCandidates = size(candidateFaces);
    if (nCandidates == 1)
        return candidateFaces[0];

    if (nCandidates != 2)
        return undefined;

    var wallAssociationAttributes = getSMAssociationAttributes(context, nextToWall);
    if (size(wallAssociationAttributes) != 1)
        return undefined;
    var associatedWallFacesQ = qEntityFilter(qAttributeQuery(wallAssociationAttributes[0]), EntityType.FACE);

    var adjacentWallQ = qAdjacent(candidateFaces[0], AdjacencyType.EDGE, EntityType.FACE);
    if (isQueryEmpty(context, qIntersection([associatedWallFacesQ, adjacentWallQ])))
        return candidateFaces[1];
    else
        return candidateFaces[0];
}

//////////////////// EDGE PATTERN ////////////////////

/**
 * Execute a sheet metal edge pattern on the specified edges of the sheet metal definition sheet body.
 * @returns {{
 *     @field modifiedEntities {Query} : entities created or modified by the wall pattern
 *     @field deletedAttributes {array} : attributes deleted by the wall pattern
 * }}
 */
function sheetMetalEdgePattern(context is Context, topLevelId is Id, id is Id, definitionEdges is array, definition is map,
        attributeIdCounter is box) returns map
{
    const definitionEdgesQ = qUnion(definitionEdges);
    var allAffectedBodies = qUnion(evaluateQuery(context, qOwnerBody(definitionEdgesQ)));
    // opPattern of edges may change body identity.  Make sure this query is robust.
    allAffectedBodies = qUnion([allAffectedBodies, startTracking(context, allAffectedBodies)]);
    const initialData = getInitialEntitiesAndAttributes(context, allAffectedBodies);
    const cornerBreakTrackingAndAttribute = createCornerBreakTrackingAndAttribute(context, qAdjacent(definitionEdgesQ, AdjacencyType.VERTEX, EntityType.VERTEX));

    var definitionForPatternOp = definition;
    definitionForPatternOp.entities = definitionEdgesQ;
    const patternId = id + "pattern";
    try
    {
        opPattern(context, patternId, definitionForPatternOp);
    }
    processSubfeatureStatus(context, topLevelId, {"subfeatureId" : patternId, "propagateErrorDisplay" : true});
    if (getFeatureError(context, topLevelId) != undefined)
    {
        // No need for error entities, they are already created by opPattern.
        throwFacePatternError(context, topLevelId, qNothing(), definition);
    }

    reapplyCornerBreaks(context, topLevelId, cornerBreakTrackingAndAttribute, attributeIdCounter);

    // Assign association attributes and gather modified entities
    return assignSMAttributesToNewOrSplitEntities(context, allAffectedBodies, initialData, id);
}

/**
 * Create an array of maps with "tracking" (a tracking query) and "attribute" (a corner attribute) fields for every
 * vertex of `vertices` which has exactly one corner break.  The "attribute" will be stripped of any unnecessary
 * information besides the one corner break.
 */
function createCornerBreakTrackingAndAttribute(context is Context, vertices is Query) returns array
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V794_EDGE_PATTERN_BREAKS))
        return [];

    const cornerTrackingAndAttribute = createSMTrackingAndAttribute(context, vertices, SMObjectType.CORNER);
    var cornerBreakTrackingAndAttribute = [];
    for (var trackingAndAttribute in cornerTrackingAndAttribute)
    {
        const attribute = trackingAndAttribute.attribute;
        // Corners with more than one break are ambiguous for edge pattern.  Skip them.
        if (attribute.cornerBreaks != undefined && size(attribute.cornerBreaks) == 1)
        {
            // Erase all the map values except ones needed for corner breaks.
            const newAttribute = {
                "attributeId" : attribute.attributeId,
                "cornerBreaks" : attribute.cornerBreaks,
                "objectType" : attribute.objectType
            } as SMAttribute;

            cornerBreakTrackingAndAttribute = append(cornerBreakTrackingAndAttribute, {
                        "tracking" : trackingAndAttribute.tracking,
                        "attribute" : newAttribute
                    });
        }
    }

    return cornerBreakTrackingAndAttribute;
}

/**
 * Reapply the corner breaks in `cornerBreakTrackingAndAttribute` to whatever wall the specified vertices were patterned
 * onto.  If the vertex falls in an ambiguous location (a location where it is difficult to determine which wall to pattern
 * the corner break onto), skip that patterned vertex.
 */
function reapplyCornerBreaks(context is Context, topLevelId is Id, cornerBreakTrackingAndAttribute is array, attributeIdCounter is box)
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V794_EDGE_PATTERN_BREAKS))
            return;

    for (var trackingAndAttribute in cornerBreakTrackingAndAttribute)
    {
        const newVertices = evaluateQuery(context, trackingAndAttribute.tracking);
        var attribute = trackingAndAttribute.attribute;

        for (var vertex in newVertices)
        {
            const isOriginalVertex = getCornerAttribute(context, vertex) != undefined;
            if (isOriginalVertex)
            {
                // Tracking query of edge pattern will also evaluate to the seed entities. If a vertex already has
                // a corner attribute, it is an original vertex and should be skipped.
                continue;
            }

            const adjacentWalls = evaluateQuery(context, qAdjacent(vertex, AdjacencyType.VERTEX, EntityType.FACE));
            if (size(adjacentWalls) != 1)
            {
                // If the vertex touches more than one wall, it is ambiguous and we should skip it.
                continue;
            }

            const newCornerBreakId = toAttributeId(topLevelId + attributeIdCounter[]);
            attributeIdCounter[] += 1;
            attribute.attributeId = newCornerBreakId;

            const wallId = getWallAttribute(context, adjacentWalls[0]).attributeId;
            // createCornerBreakTrackingAndAttribute only stores corners with one corner break
            attribute.cornerBreaks[0].value.wallId = wallId;

            setAttribute(context, { "entities" : vertex, "attribute" : attribute });
        }
    }
}

//////////////////// HOLE PATTERN ////////////////////

/**
 * Apply pattern to sheet metal holes.
 */
function sheetMetalHolePattern(context is Context, id is Id, definition is map, holeToolMap is map, definitionWallsAlreadyPatterned is array) returns map
{
    const holeDefinitionWalls = evaluateQuery(context, qUnion(holeToolMap.sheetMetalHoleToolWalls));
    const skipDefinitionWallsAlreadyPatterend = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2235_SM_HOLE_PATTERN_SKIP_WALLS_ALREADY_PATTERNED);
    const idGenerator = getUnstableIncrementingId(id);
    var transforms = makeArray(size(definition.transforms));
    var instanceNames = makeArray(size(definition.transforms));
    var modifiedWalls = [];
    var patternedHoleTools = qNothing();
    for (var holeDefinitionWall in holeDefinitionWalls)
    {
        if (skipDefinitionWallsAlreadyPatterend && isIn(holeDefinitionWall, definitionWallsAlreadyPatterned))
        {
            continue;
        }
        const smDefinitionBody = evaluateQuery(context, qOwnerBody(holeDefinitionWall));
        if (size(smDefinitionBody) != 1)
        {
            continue;
        }
        const associationAttributes = getSMAssociationAttributes(context, smDefinitionBody[0]);
        if (size(associationAttributes) != 1)
        {
            continue;
        }
        const wallAttribute = getWallAttribute(context, holeDefinitionWall);
        var bodiesToPattern = [];
        var wallCSysToWorld;
        for (var i, holeToolBody in holeToolMap.sheetMetalHoleToolBodies)
        {
            const currentWall = evaluateQuery(context, holeToolMap.sheetMetalHoleToolWalls[i]);
            if (size(currentWall) == 1 &&
                holeDefinitionWall == currentWall[0] &&
                isIn(holeToolBody.transientId, wallAttribute.cuttingToolBodyIds))
            {
                bodiesToPattern = append(bodiesToPattern, holeToolBody);
                wallCSysToWorld = holeToolMap.sheetMetalHoleToolTransforms[i];
            }
        }
        const baseId = idGenerator();
        const opPatternId = baseId + "patternTools";
        for (var i, transform in definition.transforms)
        {
            transforms[i] = transform * wallCSysToWorld;
            instanceNames[i] = "instance" ~ i;
        }
        opPattern(context, opPatternId, {
            "entities" : qUnion(bodiesToPattern),
            "transforms" : transforms,
            "instanceNames" : instanceNames,
            "holePropagationType" : HolePropagationType.PROPAGATE_NEW_HOLE
        });
        const patternedTools = qBodyType(qCreatedBy(opPatternId, EntityType.BODY), BodyType.SOLID);
        const booleanId = baseId + "registerTools";
        var wallToCuttingToolBodyIds = callSubfeatureAndProcessStatus(id, registerSheetMetalBooleanTools, context, booleanId, {
                        "targets" : qSheetMetalFlatFilter(qBodyType(qOwnerBody(qAttributeQuery(associationAttributes[0])), BodyType.SOLID), SMFlatType.NO),
                        "subtractiveTools" : patternedTools,
                        "doUpdateSMGeometry" : false
                    });
        modifiedWalls = concatenateArrays([modifiedWalls, keys(wallToCuttingToolBodyIds)]);
        patternedHoleTools = qUnion(patternedHoleTools, patternedTools);
    }
    return { "modifiedWalls" : modifiedWalls, "patternedHoleTools" : patternedHoleTools };
}


//////////////////// FORM PATTERN ////////////////////

function findCorrespondingToolBodiesInAttribute(formToolBody is string, wallAttribute is map) returns map
{
    for (var formedToolBodies in wallAttribute.formedToolBodyIds)
    {
        if (!isValueIn(formToolBody, formedToolBodies))
        {
            continue;
        }
        var formTools = [];
        for (var formedRole in ["negativeBodyId", "positiveBodyId", "placementBodyId"])
        {
            if (formedToolBodies[formedRole] != undefined)
            {
                 formTools = append(formTools, qTransient(formedToolBodies[formedRole]));
            }
        }
        if (formedToolBodies.sketchBodyIds != undefined)
        {
            for (var sketchBodyId in formedToolBodies.sketchBodyIds)
            {
                formTools = append(formTools, qTransient(sketchBodyId));
            }
        }
        return { "formTools" : formTools, "positiveBodyId" : formedToolBodies.positiveBodyId, "negativeBodyId" : formedToolBodies.negativeBodyId };
    }
    throw formToolBody.transientId ~ " not found in " ~ wallAttribute.formedToolBodyIds;
    return {};
}

/**
 * Apply pattern to sheet metal form.
 */
function sheetMetalFormPattern(context is Context, id is Id, definition is map, formToolMap is map, definitionWallsAlreadyPatterned is array) returns map
{
    const idGenerator = getUnstableIncrementingId(id);
    var transforms = makeArray(size(definition.transforms));
    var instanceNames = makeArray(size(definition.transforms));
    var definitionFaceToFormedBodies = {};
    var patternedFormTools = qNothing();
    for (var wall, toolsAndTransform in  formToolMap)
    {
        const formDefinitionWall = qTransient(wall);
        if (isIn(formDefinitionWall, definitionWallsAlreadyPatterned))
        {
            continue;
        }
        const wallAttribute = getWallAttribute(context, formDefinitionWall);
        if (wallAttribute == undefined)
        {
            throw regenError("Missing wall attribute");
        }

        var bodiesToPattern = [];
        var bodiesToPatternSet = {};
        var trackedForms = [];
        for (var formToolBody in toolsAndTransform.formToolBodies)
        {
            if (bodiesToPatternSet[formToolBody] == true)
            {
                continue;
            }
            const correspondingToolBodies = findCorrespondingToolBodiesInAttribute(formToolBody, wallAttribute);
            bodiesToPattern = concatenateArrays([bodiesToPattern, correspondingToolBodies.formTools]);
            bodiesToPatternSet[correspondingToolBodies.negativeBodyId] = true;
            bodiesToPatternSet[correspondingToolBodies.positiveBodyId] = true;
            trackedForms = append(trackedForms, startTracking(context, qUnion(correspondingToolBodies.formTools)));
        }
        const opPatternId = idGenerator() + "patternFormTools";
        for (var iTransform, transform in definition.transforms)
        {
            transforms[iTransform] = transform * toolsAndTransform.transform;
            instanceNames[iTransform] = "instance" ~ iTransform;
        }
        opPattern(context, opPatternId, {
                "entities" : qUnion(bodiesToPattern),
                "transforms" : transforms,
                "instanceNames" : instanceNames,
                "markCreatedBodiesPulic" : true
            });
        for (var instanceName in instanceNames)
        {
            for (var trackedForm in trackedForms)
            {
                const patternedTools = qIntersection(qPatternInstances(opPatternId, instanceName, EntityType.BODY), trackedForm);
                definitionFaceToFormedBodies = insertIntoMapOfArrays(definitionFaceToFormedBodies, formDefinitionWall, patternedTools);
            }
        }
        patternedFormTools = qUnion(patternedFormTools, qCreatedBy(opPatternId, EntityType.BODY));
    }
    const wallToFormedToolBodyIds = callSubfeatureAndProcessStatus(id, registerSheetMetalFormedTools, context, id + "registerFormTools", {
                                    "definitionFaceToFormedBodies" : definitionFaceToFormedBodies,
                                    "doUpdateSMGeometry" : false
                                });
    return { "modifiedWalls" : keys(wallToFormedToolBodyIds), "patternedFormTools" : patternedFormTools };
}

//////////////////// UTILITIES ////////////////////

/**
 * Pattern the seeds and delete them (seeds should be a query for bodies).  Returned the patterned copies.
 */
function patternSeeds(context is Context, id is Id, seeds is Query, definition is map) returns Query
{
    // Pattern the sheet bodies
    var definitionForOp = definition;
    definitionForOp.entities = seeds;
    const patternId = id + "pattern";
    opPattern(context, patternId, definitionForOp);

    // Delete the seed bodies
    opDeleteBodies(context, id + "deleteBodies", { "entities" : seeds});

    return qCreatedBy(patternId, EntityType.BODY);
}

/**
 * Throw an appropriate face pattern error given the pattern type.
 */
function throwFacePatternError(context is Context, topLevelId is Id, errorEntities is Query, definition is map)
{
    if (!isQueryEmpty(context, errorEntities))
    {
        setErrorEntities(context, topLevelId, { "entities" : errorEntities });
    }
    const error = (definition.patternType == PatternType.FACE) ? ErrorStringEnum.PATTERN_FACE_FAILED : ErrorStringEnum.MIRROR_FACE_FAILED;
    throw regenError(error);
}

/**
 * Return a map from model id to a map of  {
 *         "modelAttribute" : the model attribute corresponding to the model
 *         "entities" : an array of the subset of input entities which belong to the model
 * }
 */
function groupEntitiesByModelAttribute(context is Context, entities is array) returns map
{
    var modelIdToModelAndEntities = {};
    for (var entity in entities)
    {
        const modelAttribute = try silent(getSmObjectTypeAttributes(context, qOwnerBody(entity), SMObjectType.MODEL)[0]);
        if (modelAttribute == undefined)
            throw "Sheet metal entity owner body should have an associated model attribute";
        const modelId = modelAttribute.attributeId;
        if (modelIdToModelAndEntities[modelId] == undefined)
            modelIdToModelAndEntities[modelId] = { "modelAttribute" : modelAttribute, "entities" : [entity] };
        else
            modelIdToModelAndEntities[modelId].entities = append(modelIdToModelAndEntities[modelId].entities, entity);
    }
    return modelIdToModelAndEntities;
}

/**
 * If PART pattern with ADD does not make any geometry change, use this function to report a boolean no-op using
 * reconstructed sheets as error entities.
 */
function reportBooleanUnionNoOp(context is Context, topLevelId is Id, id is Id, definitionFaces is array, definition is map)
{
    reportFeatureInfo(context, topLevelId, ErrorStringEnum.BOOLEAN_UNION_NO_OP);

    const mockTopLevelId = id;
    const mockId = mockTopLevelId + "reconstruct";
    const mockCounter = new box(0);
    var mockDefinition = definition;
    mockDefinition.operationType = NewBodyOperationType.NEW;
    startFeature(context, mockTopLevelId, { "isSheetMetal" : true });
    try silent
    {
        sheetMetalWallPattern(context, mockTopLevelId, mockId, definitionFaces, mockDefinition, mockCounter);
        setErrorEntities(context, topLevelId, { "entities" : qCreatedBy(mockId, EntityType.BODY) });
    }
    abortFeature(context, mockTopLevelId);
}

