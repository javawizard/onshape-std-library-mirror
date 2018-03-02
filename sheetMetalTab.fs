FeatureScript 765; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "765.0");
export import(path : "onshape/std/tool.fs", version : "765.0");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "765.0");
import(path : "onshape/std/boolean.fs", version : "765.0");
import(path : "onshape/std/containers.fs", version : "765.0");
import(path : "onshape/std/evaluate.fs", version : "765.0");
import(path : "onshape/std/feature.fs", version : "765.0");
import(path : "onshape/std/math.fs", version : "765.0");
import(path : "onshape/std/moveFace.fs", version : "765.0");
import(path : "onshape/std/transform.fs", version : "765.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "765.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "765.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "765.0");
import(path : "onshape/std/topologyUtils.fs", version : "765.0");
import(path : "onshape/std/valueBounds.fs", version : "765.0");
import(path : "onshape/std/vector.fs", version : "765.0");

/**
 * Feature adding tabs to parallel sheet metal faces.
 * @internal
 */
annotation { "Feature Type Name" : "Tab",
        "Editing Logic Function" : "sheetMetalTabEditingLogic" }
export const sheetMetalTab = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Tab profile", "Filter" : EntityType.FACE && GeometryType.PLANE && ConstructionObject.NO }
        definition.tabFaces is Query;

        annotation { "Name" : "Flange to merge", "Filter" : SheetMetalDefinitionEntityType.FACE && AllowFlattenedGeometry.YES && ModifiableEntityOnly.YES }
        definition.booleanUnionScope is Query;

        annotation { "Name" : "Subtraction offset" }
        isLength(definition.booleanOffset, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

        annotation { "Name" : "Subtraction scope", "Filter" : (SheetMetalDefinitionEntityType.FACE && AllowFlattenedGeometry.YES && ModifiableEntityOnly.YES) || (BodyType.SOLID && EntityType.BODY) }
        definition.booleanSubtractScope is Query;
    }
    {
        // this is not necessary but helps with correct error reporting in feature pattern
        checkNotInFeaturePattern(context, definition.tabFaces, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        createTools(context, id + "extract", definition.tabFaces);

        const unionEntities = try silent(getSMDefinitionEntities(context, definition.booleanUnionScope));
        if (unionEntities is undefined || size(unionEntities) == 0)
            throw regenError(ErrorStringEnum.SHEET_METAL_TAB_NO_WALL, ["booleanUnionScope"]);
        const unionEntityQuery = qUnion(unionEntities);
        const sheetMetalBodies = evaluateQuery(context, qOwnerBody(unionEntityQuery));

        const subtractBodies = getOwnerSMModel(context, definition.booleanSubtractScope);
        var sheetMetalBodiesQuery = qUnion(concatenateArrays([subtractBodies, sheetMetalBodies]));
        sheetMetalBodiesQuery = qUnion([startTracking(context, sheetMetalBodiesQuery), sheetMetalBodiesQuery]);
        const originalEntities = evaluateQuery(context, qOwnedByBody(sheetMetalBodiesQuery));

        // The deripping step breaks these queries otherwise.
        const unionEntityPersistantQuery = qUnion([unionEntityQuery, startTracking(context, unionEntityQuery)]);

        const initialAssociationAttributes = getAttributes(context, {
                    "entities" : qOwnedByBody(sheetMetalBodiesQuery),
                    "attributePattern" : {} as SMAssociationAttribute });
        const associateChanges = startTracking(context, qOwnedByBody(sheetMetalBodiesQuery, EntityType.FACE));

        const selectionsByModelId = partitionSheetMetalQueriesByModel(context, unionEntities);
        var index = 0;
        var oneSuccess = false;
        for (var pair in selectionsByModelId)
        {
            oneSuccess = applyTab(context, id + unstableIdComponent(index), definition, qCreatedBy(id + "extract", EntityType.FACE), pair.value, id) || oneSuccess;
            index += 1;
        }

        if (!oneSuccess)
        {
            reportFeatureInfo(context, id, ErrorStringEnum.SHEET_METAL_TAB_NO_EFFECT);
            setErrorEntities(context, id, { "entities" : definition.tabFaces });
        }

        opDeleteBodies(context, id + "deleteBodies", {
                    "entities" : qCreatedBy(id + "extract", EntityType.BODY)
                });

        const toUpdate = assignSMAttributesToNewOrSplitEntities(context, sheetMetalBodiesQuery,
                originalEntities, initialAssociationAttributes);

        updateSheetMetalGeometry(context, id, {
                    "entities" : qUnion([toUpdate.modifiedEntities, unionEntityPersistantQuery]),
                    "deletedAttributes" : toUpdate.deletedAttributes,
                    "associatedChanges" : associateChanges });
    }, { booleanSubtractScope : qNothing() });

function applyTab(context is Context, id is Id, definition is map, tabQuery is Query, unionEntities is array, rootId is Id) returns boolean
{
    const evaluatedTabFaces = evaluateQuery(context, tabQuery);
    const separatedSubtractQueries = separateSheetMetalQueries(context, definition.booleanSubtractScope);

    const tabTuples = groupByCoincidentPlanes(makeTopologyTuples(context, evaluatedTabFaces));
    const wallTuples = groupByCoincidentPlanes(makeTopologyTuples(context, unionEntities));
    const pairedTuples = matchParallelWalls(tabTuples, wallTuples);
    var oneSuccess = false;
    for (var i = 0; i < size(pairedTuples); i += 1)
    {
        oneSuccess = doOneTabGroup(context, id + unstableIdComponent(i) + "group", definition, pairedTuples[i], separatedSubtractQueries, rootId) || oneSuccess;
    }
    return oneSuccess;
}

/**
 * Takes a sheet metal query and returns the results as a map where the keys are sheet metal model ids
 * and the values are arrays of queries.
 */
function partitionSheetMetalQueriesByModel(context is Context, selections is array) returns map
{
    var out = {};
    for (var selection in selections)
    {
        const withTracking = qUnion([selection, startTracking(context, selection)]);
        const id = getActiveSheetMetalId(context, selection);
        const existing = out[id];
        if (existing is undefined)
        {
            out[id] = [withTracking];
        }
        else
        {
            out[id] = append(existing, withTracking);
        }
    }
    return out;
}

/**
 * Flattens an array of maps with the topology property into a single Query.
 */
function flattenToQuery(topology is array)
{
    var out = [];
    for (var item in topology)
    {
        if (item is array)
        {
            out = append(out, flattenToQuery(item));
        }
        else
        {
            out = append(out, item.topology);
        }
    }
    return qUnion(out);
}

/**
 * Takes a map which contains arrays of walls and tabs that are all parallel. Both are grouped into coplanar groups that
 * can be booleaned as a unit. Grouping the walls in this manner is necessary to make merging them with a tab possible.
 */
function doOneTabGroup(context is Context, id is Id, definition is map, pairedTuple is map, subtractionScope is map, rootId is Id) returns boolean
{
    var oneSuccess = false;
    var index = 0;
    for (var coincidentWalls in pairedTuple.walls)
    {
        for (var coincidentTabs in pairedTuple.tabs)
        {
            const coincidentGrouping = { "tabs" : qOwnerBody(flattenToQuery(coincidentTabs)), "walls" : flattenToQuery(coincidentWalls), "plane" : coincidentWalls[0].plane };
            const status = booleanOneTabGroup(context, id + unstableIdComponent(index), definition, coincidentGrouping, subtractionScope, rootId);
            index += 1;

            if (status.statusEnum != ErrorStringEnum.BOOLEAN_UNION_NO_OP)
            {
                oneSuccess = true;
            }
        }
    }
    return oneSuccess;
}

/**
 * Takes an array of queries for planar faces and converts it into an array of maps in the form
 * { topology : input query, plane : plane of the face }
 */
function makeTopologyTuples(context is Context, topologyArray is array) returns array
{
    var out = [];
    for (var topology in topologyArray)
    {
        const plane = evPlane(context, { "face" : topology });
        if (plane is undefined)
            throw regenError(ErrorStringEnum.SHEET_METAL_TAB_NONPLANAR, topology);
        out = append(out, { "topology" : topology, "plane" : plane });
    }
    return out;
}

/**
 * Takes output from separate coincident planes and pairs the inner arrays based on normal to group walls and tabs
 * that can be joined.
 */
function matchParallelWalls(tabs is array, walls is array) returns array
{
    var paired = [];
    for (var i = 0; i < size(tabs); i += 1)
    {
        const pair = findParallelWalls(tabs[i], walls);
        if (pair is undefined)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_TAB_NO_PARALLEL_WALL, ["tabFaces"], flattenToQuery(tabs[i]));
        }
        paired = append(paired, { "tabs" : tabs[i], "walls" : pair });
    }
    return paired;
}

/**
 * Takes an array of parallel tabs grouped based on coincidence and looks for an array of parallel walls which are
 * also grouped based on coincidence.
 */
function findParallelWalls(tabs is array, walls is array)
{
    for (var j = 0; j < size(walls); j += 1)
    {
        if (parallelVectors(tabs[0][0].plane.normal, walls[j][0][0].plane.normal))
        {
            return walls[j];
        }
    }
    return undefined;
}

/**
 * Takes in an array of maps with a "plane" key and returns an array of arrays with parallel planes.
 */
function groupByParallelPlanes(tuples is array) returns array
{
    var out = [];
    for (var planeToSeparate in tuples)
    {
        var i = 0;
        for ( ; i < size(out); i += 1)
        {
            var grouping = out[i];
            if (parallelVectors(planeToSeparate.plane.normal, grouping[0].plane.normal))
            {
                out[i] = append(grouping, planeToSeparate);
                break;
            }
        }
        if (i == size(out))
        {
            out = append(out, [planeToSeparate]);
        }
    }

    return out;
}

/**
 * Takes in an array of maps with a .plane property and returns an array of arrays with parallel planes
 * grouped into arrays with coincident planes.
 */
function groupByCoincidentPlanes(tuples is array) returns array
{
    const parallel = groupByParallelPlanes(tuples);
    var out = [];
    for (var i = 0; i < size(parallel); i += 1)
    {
        var grouping = parallel[i];
        var separated = [];
        const n = size(grouping);
        for (var j = 0; j < n; j += 1)
        {
            var k = j + 1;
            for ( ; k < n; k += 1)
            {
                if (coplanarPlanes(grouping[j].plane, grouping[k].plane))
                {
                    continue;
                }
                else
                {
                    k -= 1;
                    break;
                }
            }
            k = min(k, n - 1);
            separated = append(separated, subArray(grouping, j, k + 1));
            j = k;
        }
        out = append(out, separated);
    }
    return out;
}

/**
 * Converts tools into sheet bodies.
 */
function createTools(context is Context, id is Id, tools is Query)
{
    const faces = evaluateQuery(context, tools);
    if (size(faces) == 0)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_TAB_NO_TAB, ["tabFaces"]);
    }
    var planes = [];
    for (var face in faces)
    {
        const aPlane = evPlane(context, { "face" : face });
        if (aPlane is undefined)
            throw regenError(ErrorStringEnum.SHEET_METAL_TAB_NONPLANAR, face);
        planes = append(planes, aPlane);
    }

    if (size(planes) == 0)
        return;

    const direction = planes[0].normal;
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V696_REMOVE_ADDED_REDUNDANCY))
        opExtractSurface(context, id, { "faces" : tools, "redundancyType" : ExtractSurfaceRedundancyType.REMOVE_ALL_REDUNDANCY });
    else
        opExtractSurface(context, id, { "faces" : tools, "removeRedundant" : true });
}

function applyPlaneToPlaneTransform(context is Context, id is Id, bodies is Query, fromPlane is Plane, toPlane is Plane)
{
    const translationVector = dot(toPlane.origin - fromPlane.origin, fromPlane.normal) * fromPlane.normal;
    const tabTransform = dot(toPlane.normal, fromPlane.normal) > 0 ? transform(translationVector) : transform(translationVector) * mirrorAcross(fromPlane);
    opTransform(context, id, {
                "bodies" : bodies,
                "transform" : tabTransform
            });
}

/**
 * Takes bodies with single planar faces and transforms them to be coincident to a plane.
 */
function moveTabsToPlane(context is Context, id is Id, tabs is Query, toPlane is Plane)
{
    const tabFace = qOwnedByBody(tabs, EntityType.FACE);
    const tabPlane = evPlane(context, { "face" : tabFace });
    applyPlaneToPlaneTransform(context, id, tabs, tabPlane, toPlane);
}

function reportBooleanIssues(context is Context, id is Id, tabBody is Query, wallFaces is Query)
{
    const collisions = evCollision(context, {
                "tools" : tabBody,
                "targets" : wallFaces
            });

    if (size(collisions) == 0)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_TAB_NO_MERGE, ["tabFaces"], wallFaces);
    }
}

function identifyEdgesForDeripping(context is Context, id is Id, tabBody is Query, partEntities is Query) returns array
{
    try silent
    {
        var edgesForDerip = [];
        const collisions = evCollision(context, {
                    "tools" : qOwnedByBody(tabBody, EntityType.FACE),
                    "targets" : partEntities
                });
        for (var collision in collisions)
        {
            if (collision["type"] != ClashType.ABUT_NO_CLASS)
            {
                edgesForDerip = concatenateArrays([edgesForDerip, getSMDefinitionEntities(context, collision.target, EntityType.EDGE)]);
            }
        }
        return edgesForDerip;
    }
    catch
    {
        return [];
    }
}

function smSubtractTab(context is Context, id is Id, tab is Query, subtractFaces)
{
    if (subtractFaces is undefined)
    {
        return;
    }
    var index = 0;
    for (var face in subtractFaces)
    {
        const targetModelParameters = try silent(getModelParameters(context, qOwnerBody(face)));
        if (targetModelParameters is undefined)
            throw regenError(ErrorStringEnum.REGEN_ERROR);
        const tool = createBooleanToolsForFace(context, id + unstableIdComponent(index) + "tool", face, tab, targetModelParameters);
        if (tool != undefined)
        {
            opBoolean(context, id + unstableIdComponent(index) + "booleanSubtract", {
                        "tools" : qCreatedBy(id + unstableIdComponent(index) + "tool", EntityType.FACE),
                        "targets" : face,
                        "operationType" : BooleanOperationType.SUBTRACTION,
                        "localizedInFaces" : true,
                        "allowSheets" : true
                    });
        }
        index += 1;
    }

}

function solidSubtractTab(context is Context, id is Id, tab is Query, targets)
{
    if (targets is undefined)
    {
        return;
    }
    try silent(opBoolean(context, id, {
                    "tools" : tab,
                    "targets" : targets,
                    "operationType" : BooleanOperationType.SUBTRACTION,
                    "allowSheets" : true
                }));
}

/**
 * Given a query for sheet metal model faces. Return a query for all sheet metal part faces corresponding to joints
 * on the edges of the input faces.
 */
function getCorrespondingJointEntitiesInPart(context is Context, selection is Query) returns Query
{
    const evaluatedEdges = evaluateQuery(context, selection);
    var toCollectFaces = [];
    var toCollectEdges = [];
    for (var edge in evaluatedEdges)
    {
        if (edgeIsTwoSided(context, edge))
        {
            const jointAttributes = getSmObjectTypeAttributes(context, edge, SMObjectType.JOINT);
            if (size(jointAttributes) == 0 ||
                jointAttributes[0].jointType == undefined ||
                jointAttributes[0].jointType.value != SMJointType.TANGENT)
            {
                toCollectFaces = append(toCollectFaces, edge);
            }
            else
            {
                toCollectEdges = append(toCollectEdges, edge);
            }
        }
    }
    const nToFaces = size(toCollectFaces);
    const nToEdges = size(toCollectEdges);
    if (nToFaces == 0 && nToEdges == 0)
    {
        return qNothing();
    }
    const facesQ = (nToFaces == 0) ? qNothing() : getSMCorrespondingInPart(context, qUnion(toCollectFaces), EntityType.FACE);
    const edgesQ = (nToEdges == 0) ? qNothing() : getSMCorrespondingInPart(context, qUnion(toCollectEdges), EntityType.EDGE);
    return qUnion([facesQ, edgesQ]);
}

/**
 * Thicken the input sheet body based on the parameters of the sheet metal model that is the current union target and
 * subtract it from all subtraction targets.
 */
function subtractTab(context is Context, id is Id, definition is map, subtractQueries is map, coincidentGrouping is map, rootId is Id)
{
    const unionBody = qOwnerBody(coincidentGrouping.walls);
    const modelParameters = try silent(getModelParameters(context, unionBody));
    if (modelParameters is undefined)
        throw regenError(ErrorStringEnum.REGEN_ERROR);

    try silent
    {
        opThicken(context, id + "thicken", {
                    "entities" : qOwnedByBody(coincidentGrouping.tabs, EntityType.FACE),
                    "thickness1" : modelParameters.frontThickness,
                    "thickness2" : modelParameters.backThickness
                });
    }
    catch (error)
    {
        processSubfeatureStatus(context, rootId, { "subfeatureId" : id + "thicken", "propagateErrorDisplay" : true });
        throw error;
    }

    const tabPlane = evPlane(context, { "face" : qOwnedByBody(coincidentGrouping.tabs, EntityType.FACE) });
    applyPlaneToPlaneTransform(context, id, qCreatedBy(id + "thicken", EntityType.BODY), tabPlane, coincidentGrouping.plane);

    const unionPartFaces = getSMCorrespondingInPart(context, coincidentGrouping.walls, EntityType.FACE);
    reportBooleanIssues(context, id + "union", qCreatedBy(id + "thicken", EntityType.BODY), unionPartFaces);

    const corresponding = getCorrespondingJointEntitiesInPart(context, qEdgeAdjacent(coincidentGrouping.walls, EntityType.EDGE));
    const deripCandidates = identifyEdgesForDeripping(context, id + "identify", qCreatedBy(id + "thicken", EntityType.BODY), corresponding);

    if (!deripEdges(context, id + "derip", qUnion(deripCandidates)))
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_TAB_NO_BEND, ["booleanUnionScope"]);
    }

    const subtractFaces = qUnion([qOwnedByBody(qEntityFilter(subtractQueries.sheetMetalQueries, EntityType.BODY), EntityType.FACE), qEntityFilter(subtractQueries.sheetMetalQueries, EntityType.FACE)]);
    const unionComplementTracking = startTracking(context, qSubtraction(qOwnedByBody(qOwnerBody(coincidentGrouping.walls), EntityType.FACE), coincidentGrouping.walls));
    var subtractSMFaces = try silent(getSMDefinitionEntities(context, subtractFaces, EntityType.FACE));
    if (subtractSMFaces is undefined)
    {
        subtractSMFaces = [];
    }

    if (size(subtractSMFaces) != 0 || size(evaluateQuery(context, subtractQueries.nonSheetMetalQueries)) != 0)
    {
        if (definition.booleanOffset > 0 * meter)
        {
            const moveFaceDefinition = {
                    "moveFaces" : qCreatedBy(id + "thicken", EntityType.FACE),
                    "moveFaceType" : MoveFaceType.OFFSET,
                    "offsetDistance" : definition.booleanOffset,
                    "reFillet" : false };

            opOffsetFace(context, id + "move", moveFaceDefinition);
        }
        smSubtractTab(context, id + "sm", qCreatedBy(id + "thicken", EntityType.BODY), subtractSMFaces);
        solidSubtractTab(context, id + "solid", qCreatedBy(id + "thicken", EntityType.BODY), subtractQueries.nonSheetMetalQueries);
    }

    if (modelParameters.minimalClearance > definition.booleanOffset && size(evaluateQuery(context, unionComplementTracking)) > 0)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_TAB_LOW_CLEARANCE, ["booleanOffset"], getSMCorrespondingInPart(context, unionComplementTracking, EntityType.FACE));
    }

    try silent(opDeleteBodies(context, id + "deleteBodies", {
                    "entities" : qCreatedBy(id + "thicken", EntityType.BODY)
                }));
}

/**
 * Takes an array of coplanar tabs and coplanar walls and handles the boolean union and subtract operations.
 */
function booleanOneTabGroup(context is Context, id is Id, definition is map, coincidentGrouping is map, subtractQueries is map, rootId is Id)
{
    const wallBodies = qOwnerBody(coincidentGrouping.walls);

    var cornerBreakTracking;
    const fixCornerBreaks = isAtVersionOrLater(context, FeatureScriptVersionNumber.V723_REMAP_TAB_BREAKS);
    if (fixCornerBreaks)
        cornerBreakTracking = collectCornerBreakTracking(context, wallBodies);

    subtractTab(context, id + "subtract", definition, subtractQueries, coincidentGrouping, rootId);
    moveTabsToPlane(context, id + "transform", coincidentGrouping.tabs, coincidentGrouping.plane);

    opPattern(context, id + "copyTool", {
                "entities" : coincidentGrouping.tabs,
                "transforms" : [identityTransform()],
                "instanceNames" : ["1"]
            });

    const toolsQ = qCreatedBy(id + "copyTool", EntityType.BODY);
    try silent
    {
        opBoolean(context, id + "boolean", {
                    "tools" : qUnion([wallBodies, toolsQ]),
                    "operationType" : BooleanOperationType.UNION,
                    "allowSheets" : true
                });
    }
    catch
    {
        const unionComplement = qSubtraction(qOwnedByBody(qOwnerBody(coincidentGrouping.walls), EntityType.FACE), coincidentGrouping.walls);
        const collisions = evCollision(context, {
                    "tools" : toolsQ,
                    "targets" : unionComplement
                });

        var errorGeom = [];
        for (var collision in collisions)
        {
           if (collision['type'] != ClashType.NONE)
           {
              errorGeom = append(errorGeom, collision.tool);
              errorGeom = append(errorGeom, getSMCorrespondingInPart(context, collision.target,  EntityType.FACE));
           }
        }
        if (size(errorGeom) > 0)
        {
           setErrorEntities(context, rootId, { "entities" : qUnion(errorGeom) });
           throw regenError(ErrorStringEnum.SHEET_METAL_TAB_COLLISION);
        }
        else
        {
            setErrorEntities(context, rootId, { "entities" : toolsQ});
            throw regenError(ErrorStringEnum.SHEET_METAL_TAB_FAILS_MERGE);
        }
    }
    try silent(opDeleteBodies(context, id + "unionDelete", { "entities" : qCreatedBy(id + "copyTool", EntityType.BODY) }));

    if (fixCornerBreaks)
        remapCornerBreaks(context, cornerBreakTracking);

    return getFeatureStatus(context, id + "boolean");
}

/**
 * If multiple input faces share the same sheet metal model faces, only return one of those input faces.
 */
function filterSimilarSMFaces(context is Context, faces is Query) returns Query
{
    var filteredOutArray = [];
    const definitionFaceArray = try silent(getSMDefinitionEntities(context, faces, EntityType.FACE));
    if (definitionFaceArray is undefined || size(definitionFaceArray) == 0)
        return qNothing();
    for (var definitionFace in definitionFaceArray)
    {
        const attributes = getAttributes(context, { "entities" : definitionFace, "attributePattern" : {} as SMAssociationAttribute });
        if (size(attributes) != 1)
        {
            throw regenError(ErrorStringEnum.REGEN_ERROR);
        }
        const smPartFaces = evaluateQuery(context, qSubtraction(qAttributeFilter(qEverything(EntityType.FACE), attributes[0]), definitionFace));
        if (size(smPartFaces) == 2)
        {
            filteredOutArray = append(filteredOutArray, smPartFaces[0]);
        }
    }
    return qUnion(filteredOutArray);
}

/**
 * Editing logic.
 * Fills in offset distance with minimal gap and finds the default merge scopes.
 * @internal
 */
export function sheetMetalTabEditingLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (definition.tabFaces != oldDefinition.tabFaces && (!specifiedParameters.booleanUnionScope || !specifiedParameters.booleanSubtractScope))
    {
        const faces = evaluateQuery(context, definition.tabFaces);
        if (size(faces) == 0)
        {
            if (!specifiedParameters.booleanUnionScope)
            {
                definition.booleanUnionScope = qNothing();
            }
            if (!specifiedParameters.booleanSubtractScope)
            {
                definition.booleanSubtractScope = qNothing();
            }
            return definition;
        }
        createTools(context, id + "extractHeuristic", definition.tabFaces);
        const wallQuery = qAttributeQuery(asSMAttribute({ "objectType" : SMObjectType.WALL }));
        var entityAssociations = try silent(getAttributes(context, {
                    "entities" : wallQuery,
                    "attributePattern" : {} as SMAssociationAttribute
                }));
        var allSMWalls = [];
        if (entityAssociations != undefined && size(entityAssociations) > 0)
        {
            for (var attribute in entityAssociations)
            {
                const visibleFaces = qSubtraction(qEverything(EntityType.FACE), qOwnedByBody(hiddenBodies, EntityType.FACE));
                const associatedEntities = evaluateQuery(context, qSubtraction(qAttributeFilter(visibleFaces, attribute), wallQuery));
                const ownerBody = getOwnerSMModel(context, qUnion(associatedEntities));
                const isActive = isSheetMetalModelActive(context, ownerBody[0]);
                if (isActive != undefined && isActive)
                {
                    allSMWalls = concatenateArrays([allSMWalls, associatedEntities]);
                }
            }
        }
        const collisions = evCollision(context, {
                    "tools" : qCreatedBy(id + "extractHeuristic", EntityType.BODY),
                    "targets" : qUnion(allSMWalls)
                });
        var union = [];
        var subtraction = [];
        for (var collision in collisions)
        {
            const tabPlane = try silent(evPlane(context, {
                            "face" : collision.tool
                        }));
            if (tabPlane is undefined)
                continue;

            const sheetMetalFacePlane = try silent(evPlane(context, { "face" : collision.target }));
            if (sheetMetalFacePlane != undefined && parallelVectors(tabPlane.normal, sheetMetalFacePlane.normal))
            {
                union = append(union, collision.target);
            }
            else
            {
                if (collision["type"] != ClashType.ABUT_NO_CLASS)
                {
                    subtraction = append(subtraction, collision.target);
                }
            }
        }

        if (!specifiedParameters.booleanUnionScope)
        {
            definition.booleanUnionScope = filterSimilarSMFaces(context, qUnion(union));
        }
        if (!specifiedParameters.booleanSubtractScope)
        {
            definition.booleanSubtractScope = filterSimilarSMFaces(context, qUnion(subtraction));
        }
    }
    const sheetMetalBodies = try silent(getOwnerSMModel(context, qOwnerBody(definition.booleanUnionScope)));
    if (sheetMetalBodies is undefined || size(sheetMetalBodies) != 1)
        return definition;
    if (oldDefinition == {} || (tolerantEquals(definition.booleanOffset, 0 * meter) && size(evaluateQuery(context, oldDefinition.booleanUnionScope)) == 0))
    {
        const modelParameters = try silent(getModelParameters(context, sheetMetalBodies[0]));
        if (!(modelParameters is undefined))
        {
            definition.booleanOffset = modelParameters.minimalClearance;
        }
    }
    return definition;
}

