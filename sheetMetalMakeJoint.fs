FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

export import(path : "onshape/std/smjointstyle.gen.fs", version : "✨");
export import(path: "onshape/std/smjointtype.gen.fs", version: "✨");

import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/boolean.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/error.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/extendsheetboundingtype.gen.fs", version : "✨");
import(path : "onshape/std/geomOperations.fs", version : "✨");
import(path : "onshape/std/query.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/topologyUtils.fs", version : "✨");
import(path : "onshape/std/units.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");

/**
* MakeJointType is a subset of SMJointType to restrict options visible in sheetMetalMakeJoint
*/
export enum MakeJointType
{
    annotation {"Name" : "Bend"}
    BEND,
    annotation {"Name" : "Rip"}
    RIP
}

/**
*  Produces a sheet metal joint of type RIP or BEND by extending or trimming
*  walls of selected edges. Rip is created as an edge joint by default.
*/
annotation { "Feature Type Name" : "Make joint", "Filter Selector" : "allparts", "Editing Logic Function" : "makeJointEditLogic"  }
export const sheetMetalMakeJoint = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation {"Name" : "Edges or side faces to join",
                    "Filter" : SheetMetalDefinitionEntityType.EDGE && ModifiableEntityOnly.YES,
                    "MaxNumberOfPicks" : 2}
        definition.entities is Query;

        annotation {"Name" : "Joint type", "Default" : MakeJointType.RIP }
        definition.joint is MakeJointType;

        if (definition.joint == MakeJointType.BEND)
        {
            annotation { "Name" : "Use model bend radius", "Default" : true }
            definition.useDefaultRadius is boolean;
            if (!definition.useDefaultRadius)
            {
                annotation { "Name" : "Bend radius" }
                isLength(definition.radius, BLEND_BOUNDS);
            }
        }

        if (definition.joint == MakeJointType.RIP)
        {
            annotation { "Name" : "Joint style" }
            definition.jointType is SMJointStyle;
        }

    }
    {
        //this is not necessary but helps with correct error reporting in feature pattern
        checkNotInFeaturePattern(context, definition.entities, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        //entities should be from the same sm model, but can be from different parts
        if (!areEntitiesFromSingleActiveSheetMetalModel(context, definition.entities))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_SINGLE_MODEL_NEEDED_EDGES, ["entities"]);
        }

        var smEntities = qUnion(getSMDefinitionEntities(context, definition.entities));
        createEdgeJoint(context, id, smEntities, definition);
    }, {joint: MakeJointType.RIP, jointType : SMJointStyle.EDGE}
    );


/**
* @internal
*  Editing logic makes sure that when the user deselects default radius option, we default to the default radius
*  in the input field (if the user didn't already change it)
*/
export function makeJointEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    var singleModelEntities = try silent(areEntitiesFromSingleActiveSheetMetalModel(context, definition.entities));
    if (singleModelEntities == undefined || !singleModelEntities)
        return definition;

    var edges = try silent(qUnion(getSMDefinitionEntities(context, definition.entities, EntityType.EDGE)));
    if (edges == undefined)
        return definition;

    var evaluatedEdgeQuery = evaluateQuery(context, edges);
    if (size(evaluatedEdgeQuery) == 0)
        return definition;

    var adjacentFace = qEdgeAdjacent(evaluatedEdgeQuery[0], EntityType.FACE);
    if (size(evaluateQuery(context, adjacentFace)) == 0)
    {
        return definition;
    }

    var modelParams = getModelParameters(context, qOwnerBody(adjacentFace));
    if (modelParams == undefined)
        return definition;

    if (!definition.useDefaultRadius && !specifiedParameters.radius &&
        definition.useDefaultRadius != oldDefinition.useDefaultRadius) // do this only once
    {
        definition.radius =  modelParams.defaultBendRadius;
    }
    return definition;
}

function createEdgeJoint(context is Context, id is Id, smEntities is Query, definition is map)
{
    var edges = evaluateQuery(context, smEntities);
    var faces = [];

    var jointType = definition.joint as SMJointType;
    var jointStyle = definition.jointType;
    //For selected edges, we need to get to the wall they're on.
    for (var edge in edges)
    {
        var adjacentFaces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
        if (size(adjacentFaces) != 1)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_RIP_FAIL_INTERNAL_EDGE, ["entities"]);
        }
        faces = append(faces, adjacentFaces[0]);
    }
    if (size(faces) != 2)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_MAKE_JOINT_FAIL, ["entities"]);
    }

    var modelParameters = getModelParameters(context, qOwnerBody(faces[0]));
    var plane1 = try(evPlane(context, {"face" : faces[0]}));
    var plane2 = try(evPlane(context, {"face" : faces[1]}));
    if (plane1 == undefined || plane2 == undefined)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_RIP_FAIL_NON_PLANAR, ["entities"]);
    }

    if (tolerantEquals(plane1, plane2))
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_MAKE_JOINT_FAIL, ["entities"]);
    }

    //make sure the two faces are not currently sharing edges
    var commonEdges = evaluateQuery(context, qIntersection([qEdgeAdjacent(faces[0], EntityType.FACE), faces[1]]));
    if (size(commonEdges) != 0)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_JOINT_FAIL_ADJACENT_FACES, ["entities"]);
    }

    //get originals before any changes
    var smBodies = evaluateQuery(context, qOwnerBody(smEntities));
    if (size(smBodies) > 1)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_RIP_MULTI_BODY, ["entities"]);
    }

    var smBodiesQ = qUnion(smBodies);
    const initialData = getInitialEntitiesAndAttributes(context, smBodiesQ);
    var originalEdges = startTracking(context, smEntities);

    var intersectionData = intersection(plane1, plane2);
    if (intersectionData == undefined)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_MAKE_JOINT_FAIL, ["entities"]);
    }

    // Both walls are being extended/trimmed to the bisector plane.
    var planeToExtendTo = plane(intersectionData.origin, plane2.normal - plane1.normal);

    try
    {
        sheetMetalExtendSheetBodyCall(context, id + "extend", {
            "extendMethod" : ExtendSheetBoundingType.EXTEND_TO_SURFACE,
            "offset" : 0 * inch,
            "entities" : qUnion([edges[0], edges[1]]),
            "limitEntity" : planeToExtendTo
        });
        if (getFeatureError(context, id + "extend") != undefined)
            throw regenError(ErrorStringEnum.SHEET_METAL_MAKE_JOINT_FAIL, ["entities"]);
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_MAKE_JOINT_FAIL, ["entities"]);
    }

    var count = 0;
    for (var resultingEdge in evaluateQuery(context, originalEdges))
    {
        if (edgeIsTwoSided(context, resultingEdge))
        {
            count += 1;
            if (jointType == SMJointType.RIP)
            {
                setRipAttribute(context, resultingEdge, toAttributeId(id), jointStyle);
            }
            else if (jointType == SMJointType.BEND)
            {
                var bendRadius = definition.useDefaultRadius? modelParameters.defaultBendRadius : definition.radius;
                setBendAttribute(context, id, resultingEdge, bendRadius,  definition.useDefaultRadius);
            }
        }
    }
    if (count != 1)
    {
        //we should have exactly one new joint edge
        throw regenError(ErrorStringEnum.SHEET_METAL_MAKE_JOINT_FAIL, ["entities"]);
    }

    // Add association attributes where needed and compute deleted attributes
    var toUpdate = assignSMAttributesToNewOrSplitEntities(context, smBodiesQ, initialData);
    updateSheetMetalGeometry(context, id, { "entities" : toUpdate.modifiedEntities,
                                           "deletedAttributes" : toUpdate.deletedAttributes,
                                           "associatedChanges" : originalEdges});
}

function setRipAttribute(context is Context, entity is Query, id is string, jointStyle)
{
    var ripAttribute = createRipAttribute(context, entity, id, jointStyle, undefined);
    ripAttribute.jointType = mergeMaps(ripAttribute.jointType, {"controllingFeatureId" : id, "parameterIdInFeature" : "joint"});
    ripAttribute.jointStyle = mergeMaps(ripAttribute.jointStyle, {"controllingFeatureId" : id, "parameterIdInFeature" : "jointType"});
    setAttribute(context, {"entities" : entity, "attribute" : ripAttribute});
}

function setBendAttribute(context is Context, id is Id, entity is Query, bendRadius is ValueWithUnits, isDefaultRadius is boolean)
{
    var bendAttribute = createBendAttribute(context, id, entity, toAttributeId(id), bendRadius,
                 isAtVersionOrLater(context, FeatureScriptVersionNumber.V695_SM_SWEPT_SUPPORT));
    if (bendAttribute == undefined)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_NO_0_ANGLE_BEND, ["entities"]);
    }
    bendAttribute.jointType = mergeMaps(bendAttribute.jointType, {"controllingFeatureId" : id, "parameterIdInFeature" : "joint"});
    if (!isDefaultRadius)
    {
        bendAttribute.radius = mergeMaps(bendAttribute.radius, {"controllingFeatureId" : id, "parameterIdInFeature" : "radius"});
    }
    setAttribute(context, {"entities" : entity, "attribute" : bendAttribute});
}

