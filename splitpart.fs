FeatureScript 1963; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/projectiontype.gen.fs", version : "1963.0");
export import(path : "onshape/std/query.fs", version : "1963.0");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "1963.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "1963.0");
import(path : "onshape/std/boolean.fs", version : "1963.0");
import(path : "onshape/std/containers.fs", version : "1963.0");
import(path : "onshape/std/evaluate.fs", version : "1963.0");
import(path : "onshape/std/feature.fs", version : "1963.0");
import(path : "onshape/std/manipulator.fs", version : "1963.0");
import(path : "onshape/std/math.fs", version : "1963.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "1963.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "1963.0");
import(path : "onshape/std/sketch.fs", version : "1963.0");
import(path : "onshape/std/splitoperationkeeptype.gen.fs", version : "1963.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1963.0");
import(path : "onshape/std/tool.fs", version : "1963.0");
import(path : "onshape/std/topologyUtils.fs", version : "1963.0");
import(path : "onshape/std/vector.fs", version : "1963.0");

/**
 * Defines whether a `split` should split whole parts, or just faces.
 */

export enum SplitType
{
    annotation { "Name" : "Part" }
    PART,
    annotation { "Name" : "Face" }
    FACE
}

/**
 * Feature performing an [opSplitPart].
 */
annotation { "Feature Type Name" : "Split", "Filter Selector" : "allparts",
             "Editing Logic Function" : "splitEditLogic",
             "Manipulator Change Function" : "splitManipulatorChange" }
export const splitPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Split type", "UIHint" : UIHint.HORIZONTAL_ENUM }
        definition.splitType is SplitType;

        if (definition.splitType == SplitType.PART)
        {
            annotation { "Name" : "Parts, surfaces, or curves to split",
                         "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET || BodyType.WIRE) && ModifiableEntityOnly.YES && AllowMeshGeometry.YES }
            definition.targets is Query;

            annotation { "Name" : "Entity to split with",
                        "Filter" : ((EntityType.BODY && BodyType.SHEET) || EntityType.FACE || BodyType.MATE_CONNECTOR) && AllowMeshGeometry.YES,
                        "MaxNumberOfPicks" : 1 }
            definition.tool is Query;

            annotation { "Name" : "Keep tools" }
            definition.keepTools is boolean;

            annotation { "Name" : "Trim to face boundaries" }
            definition.useTrimmed is boolean;

            annotation { "Name" : "Keep both sides", "Default" : true}
            definition.keepBothSides is boolean;
            if (!definition.keepBothSides)
            {
                annotation { "Name" : "Opposite direction", "Default" : true, "UIHint" : UIHint.OPPOSITE_DIRECTION }
                definition.keepFront is boolean;
            }
        }
        else
        {
            annotation { "Name" : "Faces to split", "Filter" : EntityType.FACE && SketchObject.NO && ConstructionObject.NO && ModifiableEntityOnly.YES && AllowMeshGeometry.YES }
            definition.faceTargets is Query;

            annotation { "Name" : "Entities to split with",
                        "Filter" : (EntityType.EDGE && SketchObject.YES && ModifiableEntityOnly.YES && ConstructionObject.NO) || //Sketch edge
                            (EntityType.BODY && (BodyType.SHEET || BodyType.WIRE) && ModifiableEntityOnly.NO) || //Sheet Body (surface) or Wire Body (curve)
                            EntityType.FACE || //Face or Construction Plane
                            BodyType.MATE_CONNECTOR
                            && AllowMeshGeometry.YES
                    }
            definition.faceTools is Query;

            annotation { "Group Name" : "Edge projection options", "Collapsed By Default" : false }
            {
                annotation { "Name" : "Projection direction type" }
                definition.projectionType is ProjectionType;

                if (definition.projectionType == ProjectionType.DIRECTION)
                {
                    annotation { "Name" : "Use sketch plane direction", "Default" : true }
                    definition.useSketchPlaneDirection is boolean;

                    if (!definition.useSketchPlaneDirection)
                    {
                        annotation { "Name" : "Direction", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION, "MaxNumberOfPicks" : 1 }
                        definition.directionQuery is Query;
                    }
                }
            }

            annotation { "Name" : "Keep tool surfaces and curves", "Default" : true }
            definition.keepToolSurfaces is boolean;
        }
    }
    {
        switch (definition.splitType)
        {
            SplitType.PART : performSplitPart(context, id, definition),
            SplitType.FACE : performSplitFace(context, id, definition)
        };
}, { keepTools : false, splitType : SplitType.PART, useTrimmed : false, keepBothSides : true, keepFront : true, keepToolSurfaces : true,
     projectionType : ProjectionType.DIRECTION, useSketchPlaneDirection : true });

function performSplitPart(context is Context, topLevelId is Id, definition is map)
{
    var tempPlaneQueries = [];

    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V747_SPLIT_ALLOW_FACES))
    {
        definition.tool = qOwnerBody(definition.tool);
    }
    else
    {
        const toolIsMultiFaceBody = (size(evaluateQuery(context, qOwnedByBody(definition.tool, EntityType.FACE))) > 1);
        if (toolIsMultiFaceBody && definition.useTrimmed)
        {
            reportFeatureInfo(context, topLevelId, ErrorStringEnum.SPLIT_TRIM_WITH_SINGLE_FACE);
        }

        // Create temporary planes from mate connectors if needed
        tempPlaneQueries = createTemporaryPlanesForMateConnectors(context, topLevelId, definition.tool);
        const sizeTempPlaneQueries = size(tempPlaneQueries);
        if (sizeTempPlaneQueries == 1)
        {
            definition.tool = tempPlaneQueries[0];
        }
        else if (sizeTempPlaneQueries > 1)
        {
            throw regenError(ErrorStringEnum.TOO_MANY_ENTITIES_SELECTED, ["tool"], definition.tool);
        }

        // Split part doesn't delete single faces, construction planes, or mate connectors
        // For mate connector tools, `definition.tool` has already been set to the temporary plane.
        const toolIsSingleFace = (size(evaluateQuery(context, qEntityFilter(definition.tool, EntityType.FACE))) == 1);
        if (toolIsSingleFace && !definition.keepTools)
        {
            const toolIsConstructionPlane = (!isQueryEmpty(context, qConstructionFilter(definition.tool, ConstructionObject.YES)));
            if (toolIsConstructionPlane)
            {
                reportFeatureInfo(context, topLevelId, ErrorStringEnum.SPLIT_KEEP_PLANES_AND_MATE_CONNECTORS);
            }
            else
            {
                reportFeatureInfo(context, topLevelId, ErrorStringEnum.SPLIT_KEEP_TOOLS_WITH_FACE);
            }
        }
    }

    definition.keepType = definition.keepBothSides ? SplitOperationKeepType.KEEP_ALL : (definition.keepFront ? SplitOperationKeepType.KEEP_FRONT : SplitOperationKeepType.KEEP_BACK);
    addKeepSideManipulator(context, topLevelId, definition);

    opSplitPart(context, topLevelId, definition);
    // `opSplitPart` doesn't delete planes regardless of `keepToolSurfaces` so we delete them here.
    if (tempPlaneQueries != [])
    {
        opDeleteBodies(context, topLevelId + "deleteBodies1", { "entities" : qUnion(tempPlaneQueries) });
    }
}

function performSplitFace(context is Context, topLevelId is Id, definition is map)
{
    const edgeTools = getEdgeTools(context, definition);
    const faceTools = qConstructionFilter(qEntityFilter(definition.faceTools, EntityType.FACE), ConstructionObject.NO);

    // bodyTools are sheet or wire bodies
    var bodyTools = qConstructionFilter(qEntityFilter(definition.faceTools, EntityType.BODY), ConstructionObject.NO);
    // Older documents require an additional filter to remove the mate connectors
    bodyTools = removeMateConnectors(context, bodyTools);

    // planeTools are construction planes and temp planes created from mate connectors
    const constructionPlaneQuery = qConstructionFilter(qEntityFilter(definition.faceTools, EntityType.FACE), ConstructionObject.YES);
    const tempPlaneQueries = createTemporaryPlanesForMateConnectors(context, topLevelId, definition.faceTools);
    const planeTools = qUnion(append(tempPlaneQueries, constructionPlaneQuery));

    // Split face doesn't delete construction planes or mate connectors
    const hasConstructionTools = (!isQueryEmpty(context, planeTools));
    if (hasConstructionTools && !definition.keepToolSurfaces)
    {
        reportFeatureInfo(context, topLevelId, ErrorStringEnum.SPLIT_KEEP_PLANES_AND_MATE_CONNECTORS);
    }

    var splitFaceDefinition = {
        "faceTargets" : definition.faceTargets,
        "edgeTools" : edgeTools,
        "bodyTools" : bodyTools,
        "planeTools" : planeTools,
        "faceTools" : faceTools,
        "projectionType" : definition.projectionType,
        "keepToolSurfaces" : definition.keepToolSurfaces
    };

    splitFaceDefinition = setDirectionForEdgeTools(context, definition, splitFaceDefinition);
    opSplitFace(context, topLevelId, splitFaceDefinition);
    // `opSplitFace` doesn't delete planes regardless of `keepToolSurfaces` so we delete them here.
    if (tempPlaneQueries != [])
    {
        opDeleteBodies(context, topLevelId + "deleteBodies1", { "entities" : qUnion(tempPlaneQueries) });
    }
}

function getEdgeTools(context is Context, definition is map) returns Query
{
    return qEntityFilter(definition.faceTools, EntityType.EDGE)->qConstructionFilter(ConstructionObject.NO);
}

function setDirectionForEdgeTools(context is Context, definition is map, splitFaceDefinition is map) returns map
{
    // edge and curve tools need an explicit direction if projectionType is Direction
    if (definition.projectionType == ProjectionType.DIRECTION &&
        !isQueryEmpty(context, splitFaceDefinition.faceTargets) &&
        (!isQueryEmpty(context, splitFaceDefinition.edgeTools) || !isQueryEmpty(context, qBodyType(splitFaceDefinition.bodyTools, BodyType.WIRE))))
    {
        // if Use sketch plane direction is checked, we set the direction to be the sketch plane normal
        if (definition.useSketchPlaneDirection)
        {
            const planeResult = getSketchPlaneOfEdgeTools(context, splitFaceDefinition.edgeTools);
            if (planeResult != undefined)
            {
                splitFaceDefinition.direction = planeResult.normal;
            }
        }
        else
        {
            splitFaceDefinition.direction = extractDirection(context, definition.directionQuery);
        }
        if (splitFaceDefinition.direction == undefined)
        {
            throw regenError(ErrorStringEnum.SPLIT_SELECT_FACE_DIRECTION, ["directionQuery"]);
        }
    }
    return splitFaceDefinition;
}

function getSketchPlaneOfEdgeTools(context is Context, edgeTools is Query)
{
    return try silent(evOwnerSketchPlane(context, { "entity" : qSketchFilter(edgeTools, SketchObject.YES) }));
}

function createTemporaryPlanesForMateConnectors(context is Context, id is Id, tools is Query) returns array
{
    var tempPlaneQueries = [];
    var mateConnectorIndex = 0;
    const toolsArray = evaluateQuery(context, tools);
    for (var tool in toolsArray)
    {
        const cSys = try silent(evMateConnector(context, { "mateConnector" : tool }));
        if (cSys != undefined)
        {
            const idPlane = id + "plane" + unstableIdComponent(mateConnectorIndex);
            setExternalDisambiguation(context, idPlane, tool);
            opPlane(context, idPlane, { "plane" : plane(cSys) });
            tempPlaneQueries = append(tempPlaneQueries, qEntityFilter(qCreatedBy(idPlane), EntityType.FACE));
            mateConnectorIndex += 1;
        }
    }
    return tempPlaneQueries;
}

function removeMateConnectors(context is Context, queryToFilter is Query) returns Query
{
    // mate connectors return false, all other queries return true
    const filterFunction = function(query)
    {
        const cs = try silent(evMateConnector(context, { "mateConnector" : query }));
        return cs == undefined;
    };

    const queryArray = filter(evaluateQuery(context, queryToFilter), filterFunction);
    return qUnion(queryArray);
}

/**
 * @internal
 * Edit logic to set keepTools to true when a single face is selected and
 * to set useSketchPlaneDirection to true when the user selects a sketch edge into the dialog
 */
export function splitEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    const numFaces = size(evaluateQuery(context, qEntityFilter(definition.tool, EntityType.FACE)));
    if (numFaces == 1 && !specifiedParameters.keepTools)
    {
        definition.keepTools = true;
    }

    if ((definition.projectionType != oldDefinition.projectionType || definition.faceTools != oldDefinition.faceTools) &&
        definition.splitType == SplitType.FACE && definition.projectionType == ProjectionType.DIRECTION && !specifiedParameters.useSketchPlaneDirection)
    {
        const edgeTools = getEdgeTools(context, definition);
        const planeResult = getSketchPlaneOfEdgeTools(context, edgeTools);
        definition.useSketchPlaneDirection = (planeResult != undefined);
    }
    return definition;
}


/**
 * @internal
 * Manipulator to keep front/back side of split
 */
function addKeepSideManipulator(context is Context, id is Id, definition is map)
{
    if (!definition.keepBothSides)
    {
        var toolFaces;
        if (!isQueryEmpty(context, qOwnedByBody(definition.tool, EntityType.FACE)))
        {
            toolFaces = qOwnedByBody(definition.tool, EntityType.FACE);
        }
        else if (!isQueryEmpty(context, qEntityFilter(definition.tool, EntityType.FACE)))
        {
            toolFaces = qEntityFilter(definition.tool, EntityType.FACE);
        }
        else
        {
            return;
        }
        if (isQueryEmpty(context, definition.targets))
        {
            return;
        }
        const distResult = try silent(evDistance(context, {
            "side0" : toolFaces,
            "side1" : definition.targets
        }));
        if (distResult == undefined)
        {
            return;
        }
        const tangentPlane = evFaceTangentPlane(context, {
            "face" : qNthElement(toolFaces, distResult.sides[0].index),
            "parameter" : distResult.sides[0].parameter
        });
        var manipulator is Manipulator = flipManipulator({
            "base" : tangentPlane.origin,
            "direction" : tangentPlane.normal,
            "flipped" : !definition.keepFront
        });
        addManipulators(context, id, {
            "flipManipulator" : manipulator
        });
    }
}

/**
 * @internal
 */
export function splitManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    for (var manipulator in newManipulators)
    {
        if (manipulator.key == "flipManipulator")
        {
            definition.keepFront = !manipulator.value.flipped;
            return definition;
        }
    }
}

