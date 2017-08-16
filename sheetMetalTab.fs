FeatureScript 660; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "660.0");
export import(path : "onshape/std/tool.fs", version : "660.0");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "660.0");
import(path : "onshape/std/boolean.fs", version : "660.0");
import(path : "onshape/std/containers.fs", version : "660.0");
import(path : "onshape/std/evaluate.fs", version : "660.0");
import(path : "onshape/std/feature.fs", version : "660.0");
import(path : "onshape/std/math.fs", version : "660.0");
import(path : "onshape/std/moveFace.fs", version : "660.0");
import(path : "onshape/std/transform.fs", version : "660.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "660.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "660.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "660.0");
import(path : "onshape/std/valueBounds.fs", version : "660.0");
import(path : "onshape/std/vector.fs", version : "660.0");

/**
* TODO: stub
* @internal
*/
annotation { "Feature Type Name" : "Sheet metal tab",
        "Editing Logic Function" : "sheetMetalTabEditingLogic" }
export const sheetMetalTab = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Tab", "Filter" : EntityType.FACE && GeometryType.PLANE && ConstructionObject.NO }
        definition.faces is Query;

        annotation { "Name" : "Boolean offset" }
        isLength(definition.booleanOffset, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

        annotation { "Name" : "Union scope", "Filter" : SheetMetalDefinitionEntityType.FACE && AllowFlattenedGeometry.YES && ModifiableEntityOnly.YES }
        definition.booleanUnionScope is Query;

        annotation { "Name" : "Subtraction scope", "Filter" : (SheetMetalDefinitionEntityType.FACE && AllowFlattenedGeometry.YES && ModifiableEntityOnly.YES) || (BodyType.SOLID && EntityType.BODY) }
        definition.booleanSubtractScope is Query;
    }
    {
        createTools(context, id + "extract", definition.faces);
        const tabQuery = qCreatedBy(id + "extract", EntityType.BODY);

        const unionEntities = try silent(getSMDefinitionEntities(context, definition.booleanUnionScope));
        if (unionEntities is undefined)
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

        const evaluatedBodies = evaluateQuery(context, tabQuery);
        var tabIndex = 0;
        var deripCandidates = [];
        for (var tabBody in evaluatedBodies)
        {
            deripCandidates = concatenateArrays([identifyEdgesForDeripping(context, id + "identify" + unstableIdComponent(tabIndex), tabBody, definition.booleanUnionScope),
                        deripCandidates]);
            tabIndex += 1;
        }
        if (!deripEdges(context, id + "derip", qUnion(deripCandidates)))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_TAB_NO_BEND, ["booleanUnionScope"]);
        }

        const separatedSubtractQueries = separateSheetMetalQueries(context, definition.booleanSubtractScope);
        for (var wall in evaluateQuery(context, unionEntityPersistantQuery))
        {
            var oneSuccess = false;
            for (var tabBody in evaluatedBodies)
            {
                var status = doOneTab(context, id + unstableIdComponent(tabIndex), definition, tabBody, wall, separatedSubtractQueries);

                if (status.statusEnum != ErrorStringEnum.BOOLEAN_UNION_NO_OP)
                {
                    oneSuccess = true;
                }
                tabIndex += 1;
            }

            if (!oneSuccess)
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_TAB_NO_MERGE, wall);
            }
        }

        const toUpdate = assignSMAttributesToNewOrSplitEntities(context, sheetMetalBodiesQuery,
                originalEntities, initialAssociationAttributes);

        try(updateSheetMetalGeometry(context, id + "smUpdate", {
                        "entities" : qUnion([toUpdate.modifiedEntities, unionEntityPersistantQuery]),
                        "deletedAttributes" : toUpdate.deletedAttributes,
                        "associatedChanges" : associateChanges }));
        processSubfeatureStatus(context, id, { "subfeatureId" : id + "smUpdate", "propagateErrorDisplay" : true });

        opDeleteBodies(context, id + "deleteBodies", {
                    "entities" : tabQuery
                });
    }, { booleanOffset : 0 * meter, booleanSubtractScope : qNothing() });

// For now, enforce that all tools are on parallel planes to simplify things.
function createTools(context is Context, id is Id, tools is Query)
{
    const faces = evaluateQuery(context, tools);
    if (size(faces) == 0)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_TAB_NO_TAB, ["faces"]);
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
    for (var aPlane in planes)
    {
        if (!parallelVectors(direction, aPlane.normal))
        {
            throw regenError("Tabs not parallel");
        }
    }
    opExtractSurface(context, id, { "faces" : tools });
}

function moveTabToFace(context is Context, id is Id, tabBody is Query, wall is Query)
{
    const wallPlane = evPlane(context, { "face" : wall });
    const tabFace = qOwnedByBody(tabBody, EntityType.FACE);
    const tabPlane = evPlane(context, { "face" : tabFace });
    const sheetMetalBody = evaluateQuery(context, qOwnerBody(wall));

    if (parallelVectors(tabPlane.normal, wallPlane.normal))
    {
        const translationVector = dot(wallPlane.origin - tabPlane.origin, tabPlane.normal) * tabPlane.normal;
        const tabTransform = dot(wallPlane.normal, tabPlane.normal) > 0 ? transform(translationVector) : transform(translationVector) * mirrorAcross(tabPlane);
        opTransform(context, id, {
                    "bodies" : tabBody,
                    "transform" : tabTransform
                });
    }
    else
    {
        throw regenError("Tabs need to be parallel");
    }
}

function identifyEdgesForDeripping(context is Context, id is Id, tabBody is Query, unionFaces is Query) returns array
{
    var edgesForDerip = [];
    for (var wall in evaluateQuery(context, unionFaces))
    {
        startFeature(context, id, {});
        moveTabToFace(context, id + "subfeature", tabBody, wall);
        const collisions = evCollision(context, {
                    "tools" : tabBody,
                    "targets" : qEdgeAdjacent(unionFaces, EntityType.FACE)
                });
        abortFeature(context, id);
        for (var collision in collisions)
        {
            const relatedEdge = qUnion(getSMDefinitionEntities(context, collision.target));
            edgesForDerip = append(edgesForDerip, relatedEdge);
        }
    }
    return edgesForDerip;
}

function smSubtractTab(context is Context, id is Id, tab is Query, subtractFaces, unionBody is Query)
{
    if (subtractFaces is undefined)
    {
        return;
    }
    var index = 0;
    for (var face in subtractFaces)
    {
        const targetModelParameters = try silent(getModelParameters(context, unionBody));
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

function subtractTab(context is Context, id is Id, definition is map, tabBody is Query, subtractQueries is map, unionBody is Query)
{
    const subtractSMFaces = try silent(getSMDefinitionEntities(context, subtractQueries.sheetMetalQueries));
    const modelParameters = try silent(getModelParameters(context, unionBody));
    if (modelParameters is undefined)
        throw regenError(ErrorStringEnum.REGEN_ERROR);

    opThicken(context, id + "thicken", {
                "entities" : qOwnedByBody(tabBody, EntityType.FACE),
                "thickness1" : modelParameters.frontThickness,
                "thickness2" : modelParameters.backThickness
            });

    const moveFaceDefinition = {
            "moveFaces" : qCreatedBy(id + "thicken", EntityType.FACE),
            "moveFaceType" : MoveFaceType.OFFSET,
            "offsetDistance" : definition.booleanOffset,
            "reFillet" : false };

    opOffsetFace(context, id + "move", moveFaceDefinition);

    smSubtractTab(context, id + "sm", qCreatedBy(id + "thicken", EntityType.BODY), subtractSMFaces, unionBody);
    solidSubtractTab(context, id + "solid", qCreatedBy(id + "thicken", EntityType.BODY), subtractQueries.nonSheetMetalQueries);

    try silent(opDeleteBodies(context, id + "deleteBodies", {
                    "entities" : qCreatedBy(id + "thicken", EntityType.BODY)
                }));
}

function doOneTab(context is Context, id is Id, definition is map, tabBody is Query, wall is Query, subtractQueries is map) returns FeatureStatus
{
    moveTabToFace(context, id + "transform", tabBody, wall);

    // Do the remove now because the tab sheet body has been oriented but not yet joined.
    subtractTab(context, id, definition, tabBody, subtractQueries, qOwnerBody(wall));

    opPattern(context, id + "copyTool", {
                "entities" : tabBody,
                "transforms" : [identityTransform()],
                "instanceNames" : ["1"]
            });

    opBoolean(context, id + "boolean", {
                "tools" : qUnion([qOwnerBody(wall), qCreatedBy(id + "copyTool", EntityType.BODY)]),
                "operationType" : BooleanOperationType.UNION,
                "allowSheets" : true
            });

    return getFeatureStatus(context, id + "boolean");
}

/**
 * Editing logic.
 * Fills in offset distance with minimal gap and finds the default merge scopes.
 * @internal
 */
export function sheetMetalTabEditingLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map) returns map
{
    if (definition.faces != oldDefinition.faces && (!specifiedParameters.booleanUnionScope || !specifiedParameters.booleanSubtractScope))
    {
        const faces = evaluateQuery(context, definition.faces);
        if (size(faces) == 0)
        {
            definition.booleanSubtractScope = qNothing();
            definition.booleanUnionScope = qNothing();
            return definition;
        }
        createTools(context, id + "extractHeuristic", definition.faces);
        const wallQuery = qAttributeQuery(asSMAttribute({ "objectType" : SMObjectType.WALL }));
        var entityAssociations = try silent(getAttributes(context, {
                    "entities" : wallQuery,
                    "attributePattern" : {} as SMAssociationAttribute
                }));
        var allSMWalls = [];
        if (entityAssociations != undefined)
        {
            for (var attribute in entityAssociations)
            {
                const associatedEntities = evaluateQuery(context, qSubtraction(qAttributeFilter(qEverything(EntityType.FACE), attribute), wallQuery));
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

            const sheetMetalFacePlane = evPlane(context, { "face" : collision.target });
            if (parallelVectors(tabPlane.normal, sheetMetalFacePlane.normal))
            {
                union = append(union, collision.target);
            }
            else
            {
                subtraction = append(subtraction, collision.target);
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

function filterSimilarSMFaces(context is Context, faces is Query)
{
    var filteredOutArray = [];
    const definitionFaceArray = try silent(getSMDefinitionEntities(context, faces, EntityType.FACE));
    if (definitionFaceArray is undefined)
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
