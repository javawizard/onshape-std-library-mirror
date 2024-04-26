FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/profilecontrolmode.gen.fs", version : "✨");
export import(path : "onshape/std/query.fs", version : "✨");
export import(path : "onshape/std/sidegeometryrule.gen.fs", version : "✨");
export import(path : "onshape/std/tool.fs", version : "✨");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "✨");
import(path : "onshape/std/booleanHeuristics.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");
import(path : "onshape/std/path.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/topologyUtils.fs", version : "✨");
import(path : "onshape/std/transform.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");

/**
 * Feature performing an [opSweep], followed by an [opBoolean]. For simple sweeps, prefer using
 * [opSweep] directly.
 */
annotation { "Feature Type Name" : "Sweep",
        "Filter Selector" : "allparts",
        "Editing Logic Function" : "sweepEditLogic" }
export const sweep = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Creation type", "UIHint" : [UIHint.HORIZONTAL_ENUM, UIHint.REMEMBER_PREVIOUS_VALUE] }
        definition.bodyType is ExtendedToolBodyType;

        if (definition.bodyType == ExtendedToolBodyType.SOLID || definition.bodyType == ExtendedToolBodyType.THIN)
        {
            booleanStepTypePredicate(definition);
        }
        else
        {
            surfaceOperationTypePredicate(definition);
        }

        if (definition.bodyType == ExtendedToolBodyType.SOLID)
        {
            annotation { "Name" : "Faces and sketch regions to sweep",
                        "Filter" : (EntityType.FACE && GeometryType.PLANE) && ConstructionObject.NO }
            definition.profiles is Query;
        }
        else if (definition.bodyType == ExtendedToolBodyType.SURFACE)
        {
            annotation { "Name" : "Edges and sketch curves to sweep",
                        "Filter" : (EntityType.EDGE && ConstructionObject.NO) || (EntityType.BODY && BodyType.WIRE && SketchObject.NO)}
            definition.surfaceProfiles is Query;
        }
        else if (definition.bodyType == ExtendedToolBodyType.THIN)
        {
            annotation { "Name" : "Faces and sketch regions to sweep", "Filter" : (EntityType.EDGE || EntityType.FACE || (EntityType.BODY && BodyType.WIRE && SketchObject.NO)) && ConstructionObject.NO }
            definition.wallShape is Query;

            annotation { "Name" : "Mid plane", "Default" : false }
            definition.midplane is boolean;

            if (!definition.midplane)
            {
                annotation { "Name" : "Thickness 1" }
                isLength(definition.thickness1, ZERO_INCLUSIVE_OFFSET_BOUNDS);

                annotation { "Name" : "Flip wall", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                definition.flipWall is boolean;

                annotation { "Name" : "Thickness 2" }
                isLength(definition.thickness2, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
            }
            else
            {
                annotation { "Name" : "Thickness" }
                isLength(definition.thickness, ZERO_INCLUSIVE_OFFSET_BOUNDS);
            }
        }

        annotation { "Name" : "Sweep path", "Filter" : (EntityType.EDGE && ConstructionObject.NO) || (EntityType.BODY && BodyType.WIRE && SketchObject.NO) }
        definition.path is Query;

        annotation { "Name" : "Profile control", "Default" : ProfileControlMode.NONE }
        definition.profileControl is ProfileControlMode;

        if (definition.profileControl == ProfileControlMode.LOCK_FACES)
        {
            annotation { "Name" : "Faces to lock", "Filter" : EntityType.FACE && ConstructionObject.NO }
            definition.lockFaces is Query;
        }
        else if (definition.profileControl == ProfileControlMode.LOCK_DIRECTION)
        {
            annotation { "Name" : "Direction to lock", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
            definition.lockDirectionQuery is Query;
        }

        if (definition.bodyType == ExtendedToolBodyType.THIN)
        {
            annotation { "Name" : "Trim ends", "Default" : true }
            definition.trimEnds is boolean;
        }

        if (definition.bodyType == ExtendedToolBodyType.SOLID || definition.bodyType == ExtendedToolBodyType.THIN)
        {
            booleanStepScopePredicate(definition);
        }
        else
        {
            surfaceJoinStepScopePredicate(definition);
        }
    }
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V203_SWEEP_PATH_NO_CONSTRUCTION))
        {
            const pathQuery = definition.path;
            definition.path = qConstructionFilter(definition.path, ConstructionObject.NO);
            if (pathQuery.queryType == QueryType.UNION && size(pathQuery.subqueries) > 0)
            {
                verifyNonemptyQuery(context, definition, "path", ErrorStringEnum.SWEEP_PATH_NO_CONSTRUCTION);
            }
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V576_GET_WIRE_LAMINAR_DEPENDENCIES))
            {
                definition.path = followWireEdgesToLaminarSource(context, definition.path);
            }
        }
        if (definition.bodyType == ExtendedToolBodyType.SURFACE)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V177_CONSTRUCTION_OBJECT_FILTER))
            {
                definition.profiles = qConstructionFilter(definition.surfaceProfiles, ConstructionObject.NO);
            }
            else
            {
                definition.profiles = definition.surfaceProfiles;
            }
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V576_GET_WIRE_LAMINAR_DEPENDENCIES))
            {
                definition.profiles = followWireEdgesToLaminarSource(context, definition.profiles);
            }
            verifyNoMesh(context, { "surfaceProfiles" : definition.profiles }, "surfaceProfiles");
        }
        else
        {
            verifyNoMesh(context, definition, "profiles");
        }
        verifyNoMesh(context, definition, "path");

        if (definition.profileControl == ProfileControlMode.KEEP_ORIENTATION)
        {
            definition.keepProfileOrientation = true;
        }
        else if (definition.profileControl == ProfileControlMode.LOCK_DIRECTION)
        {
            definition.lockDirection = extractDirection(context, definition.lockDirectionQuery);
            if (definition.lockDirection == undefined)
            {
                throw regenError(ErrorStringEnum.SWEEP_SELECT_DIRECTION, ["lockDirectionQuery"]);
            }
        }

        const remainingTransform = getRemainderPatternTransform(context,
            { "references" : qUnion([definition.profiles, definition.path]) });

        const sweepFunction = definition.bodyType == ExtendedToolBodyType.THIN ? thinSweep : opSweep;

        sweepFunction(context, id, definition);
        transformResultIfNecessary(context, id, remainingTransform);

        const reconstructOp = function(id)
            {
                sweepFunction(context, id, definition);
                transformResultIfNecessary(context, id, remainingTransform);
            };

        if (definition.bodyType == ExtendedToolBodyType.SOLID || definition.bodyType == ExtendedToolBodyType.THIN)
        {
            processNewBodyIfNeeded(context, id, definition, reconstructOp);
        }
        else if (definition.surfaceOperationType == NewSurfaceOperationType.ADD)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1197_DETECT_SURFACE_JOIN_CPP))
            {
                joinSurfaceBodiesWithAutoMatching(context, id, definition, false, reconstructOp);
            }
            else
            {
                var matches = createMatchesForSurfaceJoin(context, id, definition, remainingTransform);
                joinSurfaceBodies(context, id, matches, false, reconstructOp);
            }
        }
    }, { bodyType : ExtendedToolBodyType.SOLID, operationType : NewBodyOperationType.NEW, keepProfileOrientation : false, surfaceOperationType : NewSurfaceOperationType.NEW, defaultSurfaceScope : true, profileControl : ProfileControlMode.NONE });


/**
 * @internal
 * Editing logic function for sweep feature.
 */
export function sweepEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (definition.bodyType == ExtendedToolBodyType.SOLID || definition.bodyType == ExtendedToolBodyType.THIN)
    {
        return booleanStepEditLogic(context, id, oldDefinition, definition,
            specifiedParameters, hiddenBodies, sweep);
    }
    else
    {
        return surfaceOperationTypeEditLogic(context, id, definition, specifiedParameters, definition.surfaceProfiles, hiddenBodies);
    }
    return definition;
}

function createMatchesForSurfaceJoin(context is Context, id is Id, definition is map, transform is Transform) returns array
{
    var matches = [];
    if (definition.bodyType == ExtendedToolBodyType.SURFACE && definition.surfaceOperationType == NewSurfaceOperationType.ADD)
    {
        var capMatches = createTopologyMatchesForSurfaceJoin(context, id, definition, qCapEntity(id, CapType.EITHER), definition.profiles, transform);
        var sweptMatches = createTopologyMatchesForSurfaceJoin(context, id, definition, makeQuery(id, "SWEPT_EDGE", EntityType.EDGE, {}), definition.path, transform);
        matches = concatenateArrays([capMatches, sweptMatches]);
        checkForNotJoinableSurfacesInScope(context, id, definition, matches);
    }
    return matches;
}

//Thin wall - thickess value definition
function setWallThickness(definition is map) returns map
{
    definition.wallThickness_1 = definition.thickness1;
    definition.wallThickness_2 = definition.thickness2;

    if (definition.midplane)
    {
        definition.wallThickness_1 = definition.thickness / 2;
        definition.wallThickness_2 = definition.wallThickness_1;
        return definition;
    }
    if (definition.flipWall)
    {
        definition.wallThickness_1 = definition.thickness2;
        definition.wallThickness_2 = definition.thickness1;
    }
    return definition;
}

function evaluatePathEdge(context is Context, edge is Query, isFlipped is boolean, parameter is number) returns Line
{
    parameter = isFlipped ? 1 - parameter : parameter;
    verify(parameter >= 0 && parameter <= 1, ErrorStringEnum.SWEEP_INVALID_PATH);

    var edgeLine;
    try
    {
        edgeLine = evEdgeTangentLine(context, {
                    "edge" : edge,
                    "parameter" : parameter
                });
        edgeLine.direction = isFlipped ? -edgeLine.direction : edgeLine.direction;
    }
    catch
    {
        throw regenError(ErrorStringEnum.SWEEP_INVALID_PATH, ["path"]);
    }
    return edgeLine;
}

function getPathStartVertex(context is Context, path is Path) returns Query
{
    const pathEndVertices = getPathEndVertices(context, path);
    if (isQueryEmpty(context, pathEndVertices))
    {
        return qNothing();
    }

    const pathStart = evaluatePathEdge(context, path.edges[0], path.flipped[0], 0);

    const vertexEndPoint1 = evVertexPoint(context, {
            "vertex" : pathEndVertices->qNthElement(0)
    });

    if (tolerantEquals(pathStart.origin, vertexEndPoint1))
    {
        return pathEndVertices->qNthElement(0);
    }
    else
    {
        return pathEndVertices->qNthElement(1);
    }
}

function getFirstPathPlaneIntersection(context is Context, path is Path, plane is Plane)
{
    for (var i = 0; i < path.edges->size(); i += 1)
    {
        const edge = path.edges[i];

        const distanceResult = evDistance(context, {
                "side0" : path.edges[i],
                "side1" : plane
        });

        if (!tolerantEquals(distanceResult.distance, 0.0 * meter))
        {
            continue;
        }

        const edgeIntersectionInfo = distanceResult.sides[0];

        return evaluatePathEdge(context, path.edges[i], path.flipped[i], edgeIntersectionInfo.parameter);
    }

    return undefined;
}

function formPlaneFromEdgesAndNormal(context is Context, edges is Query, normal is Vector)
{
    const vertex = edges->qAdjacent(AdjacencyType.VERTEX, EntityType.VERTEX)->qNthElement(0);
    const point = evVertexPoint(context, {
            "vertex" : vertex
    });
    return plane(point, normal);
}

function getCapPlanes(context is Context, topLevelId is Id, definition is map, profile is Path, path is Path, bottomCapEdges is Query, topCapEdges is Query) returns map
{
    var capPlanes = {
        "bottomCapPlane": undefined,
        "topCapPlane": undefined
        };

    capPlanes.bottomCapPlane = getEdgesCommmonPlane(context, bottomCapEdges);
    capPlanes.topCapPlane = getEdgesCommmonPlane(context, topCapEdges);

    if (capPlanes.bottomCapPlane != undefined && capPlanes.topCapPlane != undefined)
    {
        return capPlanes;
    }

    const allProfileEdgesAreLines = profile.edges->size() == evaluateQuery(context, profile.edges->qUnion()->qGeometry(GeometryType.LINE))->size();
    if (!allProfileEdgesAreLines)
    {
        reportFeatureWarning(context, topLevelId, ErrorStringEnum.THIN_SWEEP_3D_PROFILE_TRIM_WARNING);
        return capPlanes;
    }

    var profilePlane = undefined;
    try silent
    {
        profilePlane = evOwnerSketchPlane(context, {
            "entity" : profile.edges->qUnion(),
            "checkAllEntities" : true
        });
    }

    if (profilePlane == undefined)
    {
        reportFeatureWarning(context, topLevelId, ErrorStringEnum.THIN_SWEEP_FAILED_TO_FIND_TRIM_PLANES_WARNING);
        return capPlanes;
    }

    const pathSize = path.edges->size();
    const pathStart = evaluatePathEdge(context, path.edges[0], path.flipped[0], 0);
    const pathEnd = evaluatePathEdge(context, path.edges[pathSize - 1], path.flipped[pathSize - 1], 1);

    const pathProfilePlaneIntersection = getFirstPathPlaneIntersection(context, path, profilePlane);

    if (pathProfilePlaneIntersection == undefined)
    {
        reportFeatureInfo(context, topLevelId, ErrorStringEnum.THIN_SWEEP_FAILED_TO_FIND_TRIM_PLANES_WARNING, ["trimEnds"]);
        return capPlanes;
    }

    const pathDirectionToProfileNormalRotation = rotationMatrix3d(pathProfilePlaneIntersection.direction, profilePlane.normal);

    capPlanes.bottomCapPlane = formPlaneFromEdgesAndNormal(context, bottomCapEdges, pathDirectionToProfileNormalRotation * pathStart.direction);
    capPlanes.topCapPlane = formPlaneFromEdgesAndNormal(context, topCapEdges, pathDirectionToProfileNormalRotation * pathEnd.direction);

    return capPlanes;
}

function getSideSurfacesEdgeFaceGroups(context is Context, topLevelId is Id, id is Id, definition is map, profile is Path, path is Path, bottomCapEdges is Query, topCapEdges is Query) returns array
{
    const thereIsNoCapEdges = isQueryEmpty(context, bottomCapEdges) || isQueryEmpty(context, topCapEdges);
    if (thereIsNoCapEdges)
    {
        return [];
    }

    const capPlanes = getCapPlanes(context, topLevelId, definition, profile, path, bottomCapEdges, topCapEdges);

    const anyOfCapPlanesUndefined = capPlanes.bottomCapPlane == undefined || capPlanes.topCapPlane == undefined;
    if (anyOfCapPlanesUndefined)
    {
        return [];
    }

    opPlane(context, id + "createBottomCapPlane", {
            "plane" : capPlanes.bottomCapPlane
    });

    opPlane(context, id + "createTopCapPlane", {
            "plane" : capPlanes.topCapPlane
    });

    const bottomConstructionPlane = qCreatedBy(id + "createBottomCapPlane", EntityType.FACE);
    const topConstructionPlane = qCreatedBy(id + "createTopCapPlane", EntityType.FACE);

    var sideSurfacesEdgeFaceGroups = [
        {"edges" : bottomCapEdges, "face" : bottomConstructionPlane},
        {"edges" : topCapEdges, "face" : topConstructionPlane}
        ];

    if (profile.closed)
    {
        const topLaminarNonCapEdges = qSubtraction(topCapEdges->qLoopEdges(), topCapEdges);
        const bottomLaminarNonCapEdges = qSubtraction(bottomCapEdges->qLoopEdges(), bottomCapEdges);
        const sideSurfacesEdgeGroups = [bottomLaminarNonCapEdges, topLaminarNonCapEdges];

        for (var i = 0; i < sideSurfacesEdgeGroups->size(); i += 1)
        {
            const iterationId = id + i;
            const edgeGroup = sideSurfacesEdgeGroups[i];

            if (isQueryEmpty(context, edgeGroup))
            {
                continue;
            }

            const edgeGroupPlane = getEdgesCommmonPlane(context, edgeGroup);

            if (edgeGroupPlane == undefined)
            {
                return [];
            }

            opPlane(context, iterationId + "constructionPlane", {
                        "plane" : edgeGroupPlane
                    });

            sideSurfacesEdgeFaceGroups = append(sideSurfacesEdgeFaceGroups, {"edges" : edgeGroup, "face" : qCreatedBy(iterationId + "constructionPlane", EntityType.FACE)});
        }
    }

    return sideSurfacesEdgeFaceGroups;
}

const thinSweep = function(context is Context, id is Id, definition is map)
    {
        //For Thin wall creation - prepare thickness value
        definition = setWallThickness(definition);

        //planar faces may be selected, only their edges should be taken
        const faces = qEntityFilter(definition.wallShape, EntityType.FACE);
        if (!isQueryEmpty(context, faces))
        {
            const extractedEdges = qAdjacent(faces, AdjacencyType.EDGE, EntityType.EDGE);
            definition.wallShape = qSubtraction(definition.wallShape, faces);
            definition.wallShape = qUnion([definition.wallShape, extractedEdges]);
        }

        const wireBodies = qEntityFilter(definition.wallShape, EntityType.BODY);
        if (!isQueryEmpty(context, wireBodies))
        {
            const extractedEdges = wireBodies->qOwnedByBody(EntityType.EDGE);
            definition.wallShape = qSubtraction(definition.wallShape, wireBodies);
            definition.wallShape = qUnion([definition.wallShape, extractedEdges]);
        }

        definition.profiles = qConstructionFilter(definition.wallShape, ConstructionObject.NO);
        verifyNonemptyQuery(context, definition, "wallShape", ErrorStringEnum.SWEEP_SELECT_PROFILE);
        verifyNonemptyQuery(context, definition, "path", ErrorStringEnum.SWEEP_SELECT_PATH);

        var profiles = [];
        try
        {
            profiles = constructPaths(context, qConstructionFilter(definition.profiles, ConstructionObject.NO), {});
        }
        catch
        {
            throw regenError(ErrorStringEnum.SWEEP_PROFILE_FAILED, ["wallShape"], definition.profiles);
        }

        var paths = [];
        try
        {
            paths = constructPaths(context, definition.path, {});
        }
        catch
        {
            throw regenError(ErrorStringEnum.SWEEP_INVALID_PATH, ["path"], definition.path);
        }

        const providedPathEdgesResolvedToOnePath = paths->size() == 1;
        verify(providedPathEdgesResolvedToOnePath, ErrorStringEnum.SWEEP_INVALID_PATH, { "faultyParameters" : ["path"], "entities" : definition.path });

        const path = paths[0];
        const pathStartVertex = getPathStartVertex(context, path);
        const pathStartVertexTracked = startTracking(context, { "subquery" : pathStartVertex, "trackPartialDependency" : true });

        for (var i = 0; i < profiles->size(); i += 1)
        {
            const iterationId = id + i;
            const profile = profiles[i];

            definition.profiles = qUnion(profile.edges);
            callSubfeatureAndProcessStatus(id, opSweep, context, iterationId + "sweep", definition);

            const sweptBody = qBodyType(qCreatedBy(iterationId + "sweep", EntityType.BODY), BodyType.SHEET);

            var thickenDefinition = {
                    "entities" : sweptBody,
                    "thickness1" : definition.wallThickness_1,
                    "thickness2" : definition.wallThickness_2
                };

            var sideSurfacesEdgeFaceGroups = [];
            if (!path.closed && definition.trimEnds)
            {
                const topCapEdges = qCapEntity(iterationId + "sweep", CapType.END, EntityType.EDGE);
                const bottomCapEdges = qCapEntity(iterationId + "sweep", CapType.START, EntityType.EDGE);
                const reorientPath = isQueryEmpty(context, qIntersection([bottomCapEdges, pathStartVertexTracked]));
                const pathForCurrentBody = reorientPath ? reverse(path) : path;
                sideSurfacesEdgeFaceGroups = getSideSurfacesEdgeFaceGroups(context, id, iterationId, definition, profile, pathForCurrentBody, bottomCapEdges, topCapEdges);

                if (sideSurfacesEdgeFaceGroups->size() != 0)
                {
                    thickenDefinition.sideGeometryRule = { "type" : SideGeometryRule.SUPPLIED, "sideSurfacesEdgeFaceGroups" : sideSurfacesEdgeFaceGroups };
                }
            }

            try
            {
                opThicken(context, iterationId + "thicken", thickenDefinition);
            }
            catch (error)
            {
                const message = try(error.message as ErrorStringEnum);
                const overrideStatus = message == ErrorStringEnum.THICKEN_FAILED ? ErrorStringEnum.THIN_SWEEP_THICKEN_FAILED : message;
                processSubfeatureStatus(context, id, { "subfeatureId" : iterationId + "thicken", "propagateErrorDisplay" : true, "overrideStatus" : overrideStatus });
                throw regenError(overrideStatus);
            }

            if (sideSurfacesEdgeFaceGroups->size() != 0)
            {
                opDeleteBodies(context, iterationId + "deleteSideSurfaceFaces", {
                            "entities" : sideSurfacesEdgeFaceGroups->mapArray(function(edgeFaceGroup) { return edgeFaceGroup.face;})->qUnion()});
            }

            opDeleteBodies(context, iterationId + "deleteSurfaceBodies", {
                        "entities" : sweptBody });
        }
    };

