FeatureScript 1247; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "1247.0");
export import(path : "onshape/std/tool.fs", version : "1247.0");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "1247.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "1247.0");
import(path : "onshape/std/evaluate.fs", version : "1247.0");
import(path : "onshape/std/feature.fs", version : "1247.0");
import(path : "onshape/std/primitives.fs", version : "1247.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1247.0");
import(path : "onshape/std/valueBounds.fs", version : "1247.0");
import(path : "onshape/std/vector.fs", version : "1247.0");

export import(path : "onshape/std/extendendtype.gen.fs", version : "1247.0");
export import(path : "onshape/std/extendsheetshapetype.gen.fs", version : "1247.0");

/**
 * Bounding type used with extend.
 */
export enum ExtendBoundingType
{
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Up to face" }
    UP_TO_FACE,
    annotation { "Name" : "Up to part/surface" }
    UP_TO_BODY,
    annotation { "Name" : "Up to vertex" }
    UP_TO_VERTEX
}

const targetBounds = 100 * meter;


/**
 * Extends a surface body by calling [opExtendSheetBody].
 */
annotation { "Feature Type Name" : "Extend surface", "Manipulator Change Function" : "extendManipulatorChange" }
export const extendSurface = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {

        annotation { "Name" : "Entities to extend", "Filter" : ((EntityType.BODY && BodyType.SHEET) ||
                                                               (EntityType.EDGE && EdgeTopology.LAMINAR)) &&
                                                                ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES }
        definition.entities is Query;

        annotation { "Name" : "Tangent propagation", "Default" : true }
        definition.tangentPropagation is boolean;

        annotation { "Name" : "Extend end condition" }
        definition.endCondition is ExtendBoundingType;
        if (definition.endCondition == ExtendBoundingType.BLIND)
        {
            annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
            definition.oppositeDirection is boolean;

            annotation { "Name" : "Extend distance" }
            isLength(definition.extendDistance, LENGTH_BOUNDS);
        }
        else if (definition.endCondition == ExtendBoundingType.UP_TO_BODY)
        {
            annotation { "Name" : "Extend target", "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO, "MaxNumberOfPicks" : 1 }
            definition.targetPart is Query;
        }
        else if (definition.endCondition == ExtendBoundingType.UP_TO_FACE)
        {
            annotation { "Name" : "Extend target", "Filter" : (EntityType.FACE && SketchObject.NO) || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
            definition.targetFace is Query;
        }
        else if (definition.endCondition == ExtendBoundingType.UP_TO_VERTEX)
        {
            annotation { "Name" : "Extend target", "Filter" : QueryFilterCompound.ALLOWS_VERTEX, "MaxNumberOfPicks" : 1 }
            definition.targetVertex is Query;
        }

        annotation {"Name" : "Maintain curvature"}
        definition.maintainCurvature is boolean;
    }
    {
        if (definition.maintainCurvature)
            definition.extensionShape = ExtendSheetShapeType.SOFT;
        else
            definition.extensionShape = ExtendSheetShapeType.LINEAR;

        if (definition.endCondition == ExtendBoundingType.BLIND)
        {
            definition.endCondition = ExtendEndType.EXTEND_BLIND;

            if (definition.oppositeDirection)
            {
                definition.extendDistance *= -1;
            }
            try(addExtendManipulator(context, id, definition));
            if (definition.extendDistance > 0)
            {
                opExtendSheetBody(context, id, definition);
            }
            else //use edge change for trimming back
            {
                const trackedEdges = getTrackedEdges(context, definition);
                var edgeChangeOptions = [];

                for (var i = 0; i < size(trackedEdges); i += 1)
                {
                    var edge = trackedEdges[i];
                    edgeChangeOptions = append(edgeChangeOptions, { "edge" : edge,
                                "face" : qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE),
                                "offset" : definition.extendDistance });
                }

                if (size(edgeChangeOptions) == 0)
                {
                    throw regenError(ErrorStringEnum.EXTEND_SHEET_BODY_NO_BODY, ["entities"]);
                }

                trimEdges(context, id + "edgeChange", edgeChangeOptions);
            }
        }
        else //up to target => up to face,part,vertex
        {
            if (evaluateQuery(context, definition.entities) == [])
            {
                throw regenError(ErrorStringEnum.EXTEND_SHEET_BODY_NO_BODY, ["entities"]);
            }

            var toDelete = [];
            definition.target = definition.targetPart;
            var errorEntityString = "targetPart";
            if (definition.endCondition == ExtendBoundingType.UP_TO_FACE)
            {
                if (evaluateQuery(context, definition.targetFace) == [])
                {
                    throw regenError(ErrorStringEnum.EXTEND_SHEET_BODY_NO_TARGET, ["targetFace"]);
                }
                definition.target = getTargetFromFace(context, id, definition);
                toDelete = append(toDelete, definition.target);
                errorEntityString = "targetFace";
            }
            else if (definition.endCondition == ExtendBoundingType.UP_TO_VERTEX)
            {
                if (evaluateQuery(context, definition.targetVertex) == [])
                {
                    throw regenError(ErrorStringEnum.EXTEND_SHEET_BODY_NO_TARGET, ["targetVertex"]);
                }
                definition.target = getTargetFromVertex(context, id, definition);
                toDelete = append(toDelete, definition.target);
                errorEntityString = "targetVertex";
            }

            if (evaluateQuery(context, definition.target) == [])
            {
                throw regenError(ErrorStringEnum.EXTEND_SHEET_BODY_NO_TARGET, [errorEntityString]);
            }

            definition.endCondition = ExtendEndType.EXTEND_TO_TARGET;

            try
            {
                extendToTarget(context, id, definition);
                if (evaluateQuery(context, qUnion(toDelete)) != [])
                {
                    opDeleteBodies(context, id + "deleteBodiesCleanup", { "entities" : qUnion(toDelete) });
                }
            }
            catch(e)
            {
                if (evaluateQuery(context, qUnion(toDelete)) != [])
                {
                    opDeleteBodies(context, id + "deleteBodiesCleanup", { "entities" : qUnion(toDelete) });
                }
                throw e;
            }
        }
    }, { oppositeDirection : false, tangentPropagation : true, endCondition : ExtendBoundingType.BLIND, maintainCurvature : false });


function extendToTarget(context is Context, id is Id, definition is map)
{
    var edgesToExtend = [];
    var edgeChangeOptions = [];
    const trackedEdges = getTrackedEdges(context, definition);
    var bodyToCollision = {}; //keep track to avoid calling evCollusion more than once per body
    if (trackedEdges == [])
    {
      throw regenError(ErrorStringEnum.EXTEND_SHEET_BODY_NO_BODY, ["entities"]);
    }
    for (var i = 0; i < size(trackedEdges); i += 1)
    {
        var edge = trackedEdges[i];
        const ownerBody = evaluateQuery(context, qOwnerBody(edge))[0];
        if (bodyToCollision[ownerBody] == false ||
            (bodyToCollision[ownerBody] == undefined && evCollision(context, { "tools" : ownerBody, "targets" : qOwnerBody(definition.target) }) == []))
        {
            bodyToCollision[ownerBody] = false;
            //no collision, use extend
            edgesToExtend = append(edgesToExtend, edge);
        }
        else //it's a trim
        {
            bodyToCollision[ownerBody] = true;
            var targetFace = qOwnedByBody(definition.target, EntityType.FACE);
            if (size(evaluateQuery(context, targetFace)) > 1)
            {
                setErrorEntities(context, id, { "entities" : definition.target });
                throw regenError(ErrorStringEnum.TRIM_TO_MULTI_FAILED); //cannot trim to multi-face targets
            }
            edgeChangeOptions = append(edgeChangeOptions, { "edge" : edge,
                        "face" : qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE),
                        "replaceFace" : targetFace });
        }
    }

    if (size(edgeChangeOptions) > 0)
    {
        trimEdges(context, id + "edgeChange", edgeChangeOptions);
    }
    else if (size(edgesToExtend) > 0)
    {
        definition.entities = qUnion(edgesToExtend);
        opExtendSheetBody(context, id + "extend", definition);
    }
}

function trimEdges(context is Context, id is Id, edgeChangeOptions is array)
{
    try silent
    {
        opEdgeChange(context, id + "edgeChange", { "edgeChangeOptions" : edgeChangeOptions });
    }
    catch
    {
        var edgesToTrim = [];
        for (var i = 0; i < size(edgeChangeOptions); i += 1)
        {
            edgesToTrim = append(edgesToTrim, edgeChangeOptions[i].edge);
        }
        setErrorEntities(context, id, { "entities" : qUnion(edgesToTrim) });
        throw regenError(ErrorStringEnum.TRIM_FAILED);
    }
}

function extendTarget(context is Context, id is Id, surfaceDefinition is map) returns Query
{
    var newTarget;
    if (surfaceDefinition is Plane)
    {
        opPlane(context, id + "plane", { "plane" : surfaceDefinition,
                            "width" : targetBounds, "height" : targetBounds });
        newTarget = qCreatedBy(id + "plane", EntityType.BODY);
    }
    else if (surfaceDefinition is Cylinder)
    {
        const cyl = surfaceDefinition;
        fCylinder(context, id + "cylinder", { "topCenter" : cyl.coordSystem.origin + targetBounds * cyl.coordSystem.zAxis,
                    "bottomCenter" : cyl.coordSystem.origin - targetBounds * cyl.coordSystem.zAxis,
                    "radius" : cyl.radius });
        const capFaces = qUnion([qCapEntity(id + "cylinder", CapType.START, EntityType.FACE),
                               qCapEntity(id + "cylinder", CapType.END, EntityType.FACE)]);
        opDeleteFace(context, id + "deleteCaps", {"deleteFaces": capFaces, "leaveOpen" : true, "includeFillet" :false, "capVoid" :false});
        newTarget = qCreatedBy(id + "cylinder", EntityType.BODY);
    }
    return newTarget;
}

function getTrackedEdges(context is Context, definition is map) returns array
{
    var selectedEdges = qEntityFilter(definition.entities, EntityType.EDGE);
    if (definition.tangentPropagation)
    {
        selectedEdges = qUnion([selectedEdges, qTangentConnectedEdges(selectedEdges)]);
    }
    var allEdges = qEdgeTopologyFilter(qUnion([selectedEdges, qOwnedByBody(definition.entities, EntityType.EDGE)]), EdgeTopology.LAMINAR);

    var trackedEdges = [];
    for (var edge in evaluateQuery(context, allEdges))
    {
        trackedEdges = append(trackedEdges, qUnion([edge, startTracking(context, edge)]));
    }
    return trackedEdges;
}

function getTargetFromFace(context is Context, id is Id, definition is map) returns Query
{
    try
    {
        const mateConnectorCSys = try silent(evMateConnector(context, { "mateConnector" : definition.targetFace }));
        if (mateConnectorCSys != undefined)
        {
            opPlane(context, id + "plane", { "plane" : plane(mateConnectorCSys),  "width" : targetBounds, "height" : targetBounds });
            return qCreatedBy(id + "plane", EntityType.BODY);
        }
        else
        {
            var targetFace = definition.targetFace;
            var surface = evSurfaceDefinition(context, { "face" : targetFace });
            if (surface is Plane || surface is Cylinder)
            {
                return extendTarget(context, id, surface);
            }
            else
            {
                opExtractSurface(context, id + "extractFace", { "faces" : definition.targetFace });
                return qCreatedBy(id + "extractFace", EntityType.BODY);
            }
        }
    }
    catch
    {
        setErrorEntities(context, id, { "entities" : definition.targetFace });

        throw regenError(ErrorStringEnum.EXTEND_TO_FACE_FAILED);
    }
}

function getTargetFromVertex(context is Context, id is Id, definition is map) returns Query
{
    try
    {
            const returnMap = getExtendDirection(context, definition.entities, false); //use edge with min detId
            if (returnMap.extendDirection != undefined)
            {
                const vertexPoint = evVertexPoint(context, { "vertex" : definition.targetVertex });
                opPlane(context, id + "plane", { "plane" : plane(vertexPoint, returnMap.extendDirection),
                            "width" : targetBounds, "height" : targetBounds });
                return qCreatedBy(id + "plane", EntityType.BODY);
            }
            else
            {
                throw regenError(ErrorStringEnum.EXTEND_TO_VERTEX_FAILED);
            }
    }
    catch
    {
        setErrorEntities(context, id, { "entities" : definition.targetVertex });
        throw regenError(ErrorStringEnum.EXTEND_TO_VERTEX_FAILED);
    }
}


//manipulator related:
const EXTEND_MANIPULATOR = "extendManipulator";

function addExtendManipulator(context is Context, id is Id, definition is map)
{
    const useLastSelectedEdge = true;
    const returnMap = try silent(getExtendDirection(context, definition.entities, useLastSelectedEdge));
    if (returnMap != undefined && returnMap.extendDirection != undefined)
    {
        addManipulators(context, id, { (EXTEND_MANIPULATOR) :
                        linearManipulator({ "base" : returnMap.origin,
                                "direction" : returnMap.extendDirection,
                                "offset" : definition.extendDistance,
                                "primaryParameterId" : "extendDistance" }) });
    }
}


function getFirstLaminarEdge(context is Context, query is Query)
{
    const laminarEdges = qEdgeTopologyFilter(query, EdgeTopology.LAMINAR);
    const laminarQ = evaluateQuery(context, laminarEdges);
    if (laminarQ == [])
    {
        throw regenError(ErrorStringEnum.EXTEND_NON_LAMINAR, ["entities"]);
    }
    return laminarQ[0];
}

function getEdgeToUse(context is Context, entities is Query, useLastSelected is boolean)
{
    const resolvedEntities = evaluateQuery(context, entities);
    var edgeToUse = undefined;
    if (@size(resolvedEntities) > 0)
    {
        if (useLastSelected) //used in manipulator attachment
        {
            edgeToUse = resolvedEntities[@size(resolvedEntities) - 1];
            if (@size(evaluateQuery(context, qEntityFilter(edgeToUse, EntityType.BODY))) != 0)
            {
                edgeToUse = getFirstLaminarEdge(context, qOwnedByBody(edgeToUse, EntityType.EDGE));
            }
        }
        else //use edge with min deterministic id, used in feature regen
        {
            const edges = evaluateQuery(context, qEntityFilter(entities, EntityType.EDGE));
            if (size(edges) > 0)
            {
                edgeToUse = edges[0];
            }
            else
            {
                //if there are no edges, use the laminar edge with min det id
                edgeToUse = getFirstLaminarEdge(context, qOwnedByBody(qEntityFilter(entities, EntityType.BODY), EntityType.EDGE));
            }
        }
    }
    return edgeToUse;

}

function getExtendDirection(context is Context, entities is Query, useLastSelectedEdge is boolean)
{
    var returnMap = {};
    const edgeToUse = getEdgeToUse(context, entities, useLastSelectedEdge);
    if (edgeToUse != undefined)
    {
        var faceNormal;
        const faces = evaluateQuery(context, qAdjacent(edgeToUse, AdjacencyType.EDGE, EntityType.FACE));
        if (size(faces) == 1)
        {
            const midParam = .5;
            const tangentLine = evEdgeTangentLine(context, { "edge" : edgeToUse, "face" : faces[0], "parameter" : midParam });
            returnMap.origin = tangentLine.origin;

            const edgeDirection = tangentLine.direction;
            const faceTangentPlane = evFaceTangentPlaneAtEdge(context, {
                        "face" : faces[0],
                        "edge" : edgeToUse,
                        "parameter" : midParam,
                        "usingFaceOrientation" : true
                    });
            faceNormal = faceTangentPlane.normal;

            if (faceNormal != undefined)
                returnMap.extendDirection = cross(edgeDirection, faceNormal);
        }
    }
    return returnMap;
}

/**
 * @internal
 */
export function extendManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    try
    {
        if (newManipulators[EXTEND_MANIPULATOR] is map)
        {
            const newValue = newManipulators[EXTEND_MANIPULATOR].offset;

            definition.extendDistance = abs(newValue);
            definition.oppositeDirection = newValue.value < 0;
        }
    }
    return definition;
}

