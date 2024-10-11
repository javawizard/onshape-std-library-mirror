FeatureScript 2491; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2491.0");
export import(path : "onshape/std/tool.fs", version : "2491.0");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "2491.0");
export import(path : "onshape/std/sidegeometryrule.gen.fs", version : "2491.0");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "2491.0");
import(path : "onshape/std/booleanHeuristics.fs", version : "2491.0");
import(path : "onshape/std/containers.fs", version : "2491.0");
import(path : "onshape/std/evaluate.fs", version : "2491.0");
import(path : "onshape/std/feature.fs", version : "2491.0");
import(path : "onshape/std/math.fs", version : "2491.0");
import(path : "onshape/std/string.fs", version : "2491.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2491.0");
import(path : "onshape/std/topologyUtils.fs", version : "2491.0");
import(path : "onshape/std/transform.fs", version : "2491.0");
import(path : "onshape/std/units.fs", version : "2491.0");
import(path : "onshape/std/valueBounds.fs", version : "2491.0");
import(path : "onshape/std/vector.fs", version : "2491.0");

/**
 * Specifies an end condition for one side of a loft.
 */
export enum LoftEndDerivativeType
{
    annotation { "Name" : "None" }
    DEFAULT,
    annotation { "Name" : "Normal to profile" }
    NORMAL_TO_PROFILE,
    annotation { "Name" : "Tangent to profile" }
    TANGENT_TO_PROFILE,
    annotation { "Name" : "Match tangent" }
    MATCH_TANGENT,
    annotation { "Name" : "Match curvature" }
    MATCH_CURVATURE
}

/**
 * Specifies derivative condition for a guide
 */
export enum LoftGuideDerivativeType
{
    annotation { "Name" : "None" }
    DEFAULT,
    annotation { "Name" : "Normal to guide" }
    NORMAL_TO_GUIDE,
    annotation { "Name" : "Tangent to guide" }
    TANGENT_TO_GUIDE,
    annotation { "Name" : "Match tangent" }
    MATCH_TANGENT,
    annotation { "Name" : "Match curvature" }
    MATCH_CURVATURE
}

/**
 * Internal
 */
const LOFT_INTERNAL_SECTIONS_COUNT =
{
    (unitless) : [1, 5, 50]
}   as IntegerBoundSpec;

/* @internal */
const EDGE_INTERIOR_PARAMETER_BOUNDS = [0.001, 0.5, 0.999];

/* @internal */
const fsConnectionsArcLengthParameterization = false; // For better performance. Must use name different from definition map keys
// [evDistance], [evEdgeTangentLine] etc. called in conjunction with connections should use this as value of arcLengthParameterization

/**
 * Feature performing an [opLoft].
 */
annotation { "Feature Type Name" : "Loft",
             "Filter Selector" : "allparts",
             "Manipulator Change Function" : "loftManipulator",
             "Editing Logic Function" : "loftEditLogic" }
export const loft = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Creation type", "UIHint" : [UIHint.HORIZONTAL_ENUM, UIHint.REMEMBER_PREVIOUS_VALUE]}
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
            annotation { "Name" : "Profiles", "Item name" : "profile",
                "Driven query" : "sheetProfileEntities", "Item label template" : "#sheetProfileEntities", "UIHint" : UIHint.COLLAPSE_ARRAY_ITEMS }
            definition.sheetProfilesArray is array;
            for (var profile in definition.sheetProfilesArray)
            {
                annotation { "Name" : "Faces and sketch regions",
                         "Filter" : ((EntityType.FACE || (EntityType.BODY && BodyType.SHEET && SketchObject.NO)) && ConstructionObject.NO)
                                    || EntityType.VERTEX }
                profile.sheetProfileEntities is Query;
            }
        }
        else
        {
            annotation { "Name" : "Profiles", "Item name" : "profile",
                "Driven query" : "wireProfileEntities", "Item label template" : "#wireProfileEntities", "UIHint" : UIHint.COLLAPSE_ARRAY_ITEMS }
            definition.wireProfilesArray is array;
            for (var profile in definition.wireProfilesArray)
            {
                annotation { "Name" : "Edges, curves and sketches",
                         "Filter" : ((EntityType.EDGE || EntityType.FACE || (EntityType.BODY && (BodyType.WIRE || BodyType.SHEET) && SketchObject.NO)) && ConstructionObject.NO)
                                    || EntityType.VERTEX }
                profile.wireProfileEntities is Query;
            }
        }

        if (definition.bodyType == ExtendedToolBodyType.THIN)
        {
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

        annotation { "Group Name" : "End conditions", "Collapsed By Default" : false }
        {
            annotation { "Name" : "Start profile condition", "UIHint" : UIHint.SHOW_LABEL }
            definition.startCondition is LoftEndDerivativeType;

            if (definition.startCondition == LoftEndDerivativeType.MATCH_TANGENT || definition.startCondition == LoftEndDerivativeType.MATCH_CURVATURE)
            {
                annotation { "Name" : "Adjacent faces", "Column Name" : "Start profile adjacent faces", "Filter" : EntityType.FACE, "UIHint" : UIHint.FOCUS_ON_VISIBLE }
                definition.adjacentFacesStart is Query;
            }
            if (definition.startCondition != LoftEndDerivativeType.DEFAULT)
            {
                annotation { "Name" : "Start magnitude" }
                isReal(definition.startMagnitude, CLAMP_MAGNITUDE_REAL_BOUNDS);
            }

            annotation { "Name" : "End profile condition", "UIHint" : UIHint.SHOW_LABEL }
            definition.endCondition is LoftEndDerivativeType;

            if (definition.endCondition == LoftEndDerivativeType.MATCH_TANGENT || definition.endCondition == LoftEndDerivativeType.MATCH_CURVATURE)
            {
                annotation { "Name" : "Adjacent faces", "Column Name" : "End profile adjacent faces", "Filter" : EntityType.FACE, "UIHint" : UIHint.FOCUS_ON_VISIBLE }
                definition.adjacentFacesEnd is Query;
            }
            if (definition.endCondition != LoftEndDerivativeType.DEFAULT)
            {
                annotation { "Name" : "End magnitude" }
                isReal(definition.endMagnitude, CLAMP_MAGNITUDE_REAL_BOUNDS);
            }
        }

        if (definition.bodyType == ExtendedToolBodyType.SURFACE || definition.bodyType == ExtendedToolBodyType.THIN)
        {
            annotation { "Name" : "Trim profiles" , "Default" : false}
            definition.trimProfiles is boolean;
        }

        annotation { "Name" : "Guides and continuity" }
        definition.addGuides is boolean;

        if (definition.addGuides)
        {
            annotation { "Name" : "Guides", "Item name" : "guide",
                "Driven query" : "guideEntities", "Item label template" : "#guideEntities", "UIHint" : [UIHint.COLLAPSE_ARRAY_ITEMS, UIHint.FOCUS_ON_VISIBLE] }
            definition.guidesArray is array;
            for (var guide in definition.guidesArray)
            {
                annotation { "Name" : "Edges, curves and sketches", "Filter" : (EntityType.EDGE && ConstructionObject.NO) || (EntityType.BODY && BodyType.WIRE && SketchObject.NO) }
                guide.guideEntities is Query;

                annotation { "Name" : "Continuity", "UIHint" : [ UIHint.SHOW_LABEL, UIHint.MATCH_LAST_ARRAY_ITEM ] }
                guide.guideDerivativeType is LoftGuideDerivativeType;

                annotation { "Name" : "Magnitude", "UIHint" : UIHint.ALWAYS_HIDDEN }
                isReal(guide.guideDerivativeMagnitude, CLAMP_MAGNITUDE_REAL_BOUNDS);
            }
            if (definition.bodyType == ExtendedToolBodyType.SURFACE || definition.bodyType == ExtendedToolBodyType.THIN)
            {
                annotation { "Name" : "Trim guides" , "Default" : true}
                definition.trimGuidesByProfiles is boolean;
            }
        }

        annotation { "Name" : "Path" }
        definition.addSections is boolean;

        if (definition.addSections)
        {
            annotation { "Name" : "Edges, curves and sketches", "Filter" : (EntityType.EDGE && ConstructionObject.NO)  || (EntityType.BODY && BodyType.WIRE), "UIHint" : UIHint.FOCUS_ON_VISIBLE }
            definition.spine is Query;

            annotation { "Name" : "Section count"}
            isInteger(definition.sectionCount, LOFT_INTERNAL_SECTIONS_COUNT);
        }

        annotation { "Name" : "Connections" }
        definition.matchConnections is boolean;
        annotation { "Group Name" : "Connections", "Driving Parameter" : "matchConnections", "Collapsed By Default" : false }
        {
            if (definition.matchConnections)
            {
                annotation { "Name" : "Match connections", "Item name" : "connection", "UIHint" : [UIHint.FOCUS_INNER_QUERY, UIHint.FOCUS_ON_VISIBLE],
                    "Driven query" :  "connectionEntities", "Item label template" : "#connectionEntities"}
                definition.connections is array;
                for (var connection in definition.connections)
                {
                    annotation { "Name" : "Vertices or edges",
                        "Filter" : (EntityType.EDGE && ConstructionObject.NO) || (EntityType.VERTEX && AllowEdgePoint.NO) }
                    connection.connectionEntities is Query;

                    annotation { "Name" : "Edge queries" , "UIHint" : UIHint.ALWAYS_HIDDEN }
                    connection.connectionEdgeQueries is Query; // Unioned array of individual edge queries synchronized with connectionEdgeParameters

                    // Synced array of edge parameters (numbers) defined in accordance with fsConnectionsArcLengthParameterization
                    annotation { "Name" : "Edge parameters", "UIHint" : UIHint.ALWAYS_HIDDEN }
                    isAnything(connection.connectionEdgeParameters);
                }
            }
        }

        annotation { "Name" : "Make periodic", "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.makePeriodic is boolean;

        annotation {"Name" : "Show isocurves"}
        definition.showIsocurves is boolean;

        if (definition.showIsocurves)
        {
            annotation {"Name" : "Count" }
            isInteger(definition.curveCount, ISO_GRID_BOUNDS);
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
        definition.profileSubqueries = [];
        if (definition.bodyType == ExtendedToolBodyType.SURFACE || definition.bodyType == ExtendedToolBodyType.THIN)
        {
            definition.profileSubqueries = collectSubParameters(definition.wireProfilesArray, "wireProfileEntities");
            definition.profileSubqueries = replaceWireQueriesWithDependencies(context, definition.profileSubqueries, true);
            // Replace sketch faces with sketch wire edges so that created loft cap edges can be traced back easily and joined with other surfaces created from the same sketch.
            definition.profileSubqueries = replaceEndSketchFacesWithWireEdges(context, definition.profileSubqueries);
            verifyNoMesh(context, { "wireProfileEntities" : qUnion(definition.profileSubqueries) }, "wireProfileEntities");
        }
        else
        {
            definition.profileSubqueries = collectSubParameters(definition.sheetProfilesArray, "sheetProfileEntities");
            verifyNoMesh(context, { "sheetProfileEntities" : qUnion(definition.profileSubqueries) }, "sheetProfileEntities");
        }

        const allowConstructionVerticesAsProfiles = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1103_CONSTRUCTION_VERTEX_CHANGE);
        definition.profileSubqueries = wrapSubqueriesInConstructionFilter(context, definition.profileSubqueries, allowConstructionVerticesAsProfiles);

        if (size(definition.profileSubqueries) < 1)
        {
            const errorEntities = (definition.bodyType == ExtendedToolBodyType.SOLID) ? "sheetProfilesArray" : "wireProfilesArray";
            throw regenError(ErrorStringEnum.LOFT_SELECT_PROFILES, [errorEntities]);
        }

        var queriesForTransform = definition.profileSubqueries;
        var derivatives = [];
        if (definition.addGuides)
        {
            definition.guideSubqueries = collectSubParameters(definition.guidesArray, "guideEntities");
            definition.guideSubqueries = replaceWireQueriesWithDependencies(context, definition.guideSubqueries, false);
            var setQueriesForTransformAfterConstructionFilter = isAtVersionOrLater(context, FeatureScriptVersionNumber.V683_LOFT_ARRAY_PARAMETERS);
            if (!setQueriesForTransformAfterConstructionFilter)
            {
                queriesForTransform = concatenateArrays([queriesForTransform, definition.guideSubqueries]);
            }
            definition.guideSubqueries = wrapSubqueriesInConstructionFilter(context, definition.guideSubqueries, false);
            verifyNoMesh(context, { "guideEntities" : qUnion(definition.guideSubqueries) }, "guideEntities");
            if (setQueriesForTransformAfterConstructionFilter)
            {
                queriesForTransform = concatenateArrays([queriesForTransform, definition.guideSubqueries]);
            }
            derivatives = concatenateArrays([derivatives, collectGuideDerivatives(context, definition)]);
        }

        verifyNoMesh(context, definition, "spine");

        if (definition.startCondition != LoftEndDerivativeType.DEFAULT)
        {
            derivatives = append(derivatives, createProfileConditions(context, definition.startCondition,
                                                        definition.profileSubqueries[0], 0, definition.startMagnitude, definition.adjacentFacesStart));
        }
        if (definition.endCondition != LoftEndDerivativeType.DEFAULT)
        {
            const lastProfileIndex = @size(definition.profileSubqueries) - 1;
            derivatives = append(derivatives, createProfileConditions(context, definition.endCondition,
                                                        definition.profileSubqueries[lastProfileIndex], lastProfileIndex, definition.endMagnitude, definition.adjacentFacesEnd));
        }
        definition.derivativeInfo = derivatives;

        if (definition.addSections)
        {
            var spineNoConstructionQuery = qConstructionFilter(definition.spine, ConstructionObject.NO);
            if (isQueryEmpty(context, spineNoConstructionQuery) && !isQueryEmpty(context, definition.spine))
            {
                throw regenError(ErrorStringEnum.SWEEP_PATH_NO_CONSTRUCTION, ["spine"]);
            }
            if (definition.addGuides && !isQueryEmpty(context, definition.spine) && size(definition.guideSubqueries) > 3 )
            {
                throw regenError(ErrorStringEnum.LOFT_SPINE_TOO_MANY_GUIDES, ["spine", "guides"]);
            }
            definition.spine = dissolveWires(spineNoConstructionQuery);
        }

        if (!definition.matchConnections)
        {
            definition.connections = [];
        }
        else
        {
            // connectionsArcLengthParameterization is not a feature parameter, but we pass it to server using definition
            definition.connectionsArcLengthParameterization = fsConnectionsArcLengthParameterization;
            if (!isInFeaturePattern(context))
            {
                addConnectionManipulators(context, id, definition);
            }
            for (var connection in definition.connections)
            {
                queriesForTransform = append(queriesForTransform, connection.connectionEntities);
                queriesForTransform = append(queriesForTransform, connection.connectionEdgeQueries);
            }
        }

        var remainingTransform = getRemainderPatternTransform(context,
                {"references" : qUnion(queriesForTransform)});

        for (var ii = 0; ii < size(definition.connections); ii += 1)
        {
            // opLoft expects an array of individual connection edge queries
            definition.connections[ii].connectionEdges =
                evaluateQuery(context, definition.connections[ii].connectionEdgeQueries);

            // Can happen is modeling changes happen upstream such that query evaluates to more or less entities, but
            // editing logic does not have a chance to run
            const hasEdgeParameterMismatch = size(definition.connections[ii].connectionEdges)
                != size(definition.connections[ii].connectionEdgeParameters);
            if (hasEdgeParameterMismatch && isAtVersionOrLater(context, FeatureScriptVersionNumber.V1560_STATUS_ON_THROW))
            {
                throw regenError(ErrorStringEnum.LOFT_CONNECTION_MATCHING, ["connections[" ~ ii ~"].connectionEntities"]);
            }
        }

        if (definition.bodyType == ExtendedToolBodyType.THIN)
        {
            thinLoft(context, id, definition);
        }
        else
        {
            // it is not a subfeature, but need to remap parameter ids
            callSubfeatureAndProcessStatus(id, opLoft, context, id, definition, {
                        "featureParameterMappingFunction" :
                            function(arrayParameterId)
                            {
                                return mapOpLoftArrayParameters(arrayParameterId, definition.bodyType == ExtendedToolBodyType.SOLID);
                            }
                    });
        }

        transformResultIfNecessary(context, id, remainingTransform);

        const reconstructOp = function(id) {
            if (definition.bodyType == ExtendedToolBodyType.THIN)
            {
                try silent(thinLoft(context, id, definition));
            }
            else
            {
                try silent(opLoft(context, id, definition));
            }
            transformResultIfNecessary(context, id, remainingTransform);
        };

        var makeSolid = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1130_SURFACING_IMPROVEMENTS) ? true : false;

        if (definition.bodyType == ExtendedToolBodyType.SOLID || definition.bodyType == ExtendedToolBodyType.THIN)
        {
            processNewBodyIfNeeded(context, id, definition, reconstructOp);
        }
        else if (definition.surfaceOperationType == NewSurfaceOperationType.ADD)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1197_DETECT_SURFACE_JOIN_CPP))
            {
                joinSurfaceBodiesWithAutoMatching(context, id, definition, makeSolid, reconstructOp);
            }
            else
            {
                var matches = createLoftTopologyMatchesForSurfaceJoin(context, id, definition, remainingTransform);
                joinSurfaceBodies(context, id, matches, makeSolid, reconstructOp);
            }
        }

    }, { makePeriodic : false, bodyType : ExtendedToolBodyType.SOLID, operationType : NewBodyOperationType.NEW,
        addGuides : false, matchConnections : false,
        startCondition : LoftEndDerivativeType.DEFAULT, endCondition : LoftEndDerivativeType.DEFAULT,
        startMagnitude : 1, endMagnitude : 1, surfaceOperationType : NewSurfaceOperationType.NEW,
        addSections : false, sectionCount : 0, defaultSurfaceScope : true,
        trimGuidesByProfiles : false, trimProfiles : false, showIsocurves : false,
        adjacentFacesStart : qNothing(), adjacentFacesEnd : qNothing(),
        showImplicitConnections : false });

/** @internal */
export function createProfileConditions(context is Context, endCondition is LoftEndDerivativeType, profileQuery is Query, profileIndex is number, magnitude is number, adjFaceQuery is Query) returns map
{
    if (endCondition == LoftEndDerivativeType.NORMAL_TO_PROFILE || endCondition == LoftEndDerivativeType.TANGENT_TO_PROFILE)
    {
        var derivativeInfo = { "profileIndex" : profileIndex,
                               "magnitude" : magnitude,
                               "tangentToPlane" : endCondition == LoftEndDerivativeType.TANGENT_TO_PROFILE };
        var planeResult = try silent(evPlane(context, { "face" : profileQuery }));
        if (planeResult is Plane)
        {
            derivativeInfo.vector = normalize(planeResult.normal);
        }
        else
        {
            //it might be that we have just edges in the profile, if on sketch, use the sketch plane
            planeResult = try(evOwnerSketchPlane(context, {"entity" : profileQuery }));
            if (planeResult is Plane)
            {
               derivativeInfo.vector = normalize(planeResult.normal);
            }
            else
            {
                throw regenError(profileIndex == 0 ? ErrorStringEnum.LOFT_NO_PLANE_FOR_START_CLAMP : ErrorStringEnum.LOFT_NO_PLANE_FOR_END_CLAMP);
            }
        }
        return derivativeInfo;
    }
    else if (endCondition == LoftEndDerivativeType.MATCH_TANGENT ||
             endCondition == LoftEndDerivativeType.MATCH_CURVATURE)
    {
        if (!isQueryEmpty(context, adjFaceQuery))
        {
            const derivativeInfo = { "profileIndex" : profileIndex,
                                    "magnitude" : magnitude,
                                    "matchCurvature" : endCondition == LoftEndDerivativeType.MATCH_CURVATURE,
                                    "adjacentFaces" : adjFaceQuery,
                                    "userDefinedAdjacentFaces" : true};
            return derivativeInfo;
        }
        const adjacentFaceQuery = qAdjacent(profileQuery, AdjacencyType.EDGE, EntityType.FACE);
        if (isQueryEmpty(context, adjacentFaceQuery))
        {
            throw regenError(profileIndex == 0 ? ErrorStringEnum.LOFT_NO_FACE_FOR_START_CLAMP : ErrorStringEnum.LOFT_NO_FACE_FOR_END_CLAMP);
        }
        const derivativeInfo = { "profileIndex" : profileIndex,
                                 "magnitude" : magnitude,
                                 "matchCurvature" : endCondition == LoftEndDerivativeType.MATCH_CURVATURE,
                                 "adjacentFaces" : adjacentFaceQuery,
                                 "userDefinedAdjacentFaces" : false};
        return derivativeInfo;
    }
}

/** @internal */
export function wrapSubqueriesInConstructionFilter(context is Context, subqueries is array, allowConstructionVertices is boolean) returns array
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V177_CONSTRUCTION_OBJECT_FILTER))
    {
        var wrappedSubqueries = [];
        for (var i = 0; i < @size(subqueries); i += 1)
        {
            var wrappedSubquery = qConstructionFilter(subqueries[i], ConstructionObject.NO);
            if (allowConstructionVertices)
            {
                wrappedSubquery = qUnion([wrappedSubquery, qEntityFilter(subqueries[i], EntityType.VERTEX)]);
            }
            wrappedSubqueries = append(wrappedSubqueries, wrappedSubquery);
        }
        return wrappedSubqueries;
    }
    return subqueries;
}

/**
 * @internal
 * Editing logic function for loft feature.
 */
export function loftEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (definition.matchConnections)
    {
        definition.connections = updateConnections(context, definition.connections);
    }

    if (!isCreating)
    {
        return definition;
    }
    if (definition.bodyType == ExtendedToolBodyType.SOLID)
    {
        if (oldDefinition.bodyType == ExtendedToolBodyType.SURFACE || oldDefinition.bodyType == ExtendedToolBodyType.THIN && specifiedParameters.sheetProfilesArray != true)
        {
            definition = mergeMaps(definition, copyFaceOrVertexSelections(context, oldDefinition.wireProfilesArray, arrayParameterMappingSheet, arrayParameterMappingSolid));
        }
        definition = booleanStepEditLogic(context, id, oldDefinition, definition,
                                specifiedParameters, hiddenBodies, loft);
    }
    else
    {
        if (oldDefinition.bodyType == ExtendedToolBodyType.SOLID && specifiedParameters.wireProfilesArray != true)
        {
            definition = mergeMaps(definition, copyFaceOrVertexSelections(context, oldDefinition.sheetProfilesArray, arrayParameterMappingSolid, arrayParameterMappingSheet));
        }
       definition = surfaceOperationTypeEditLogic(context, id, definition,
                                    specifiedParameters, wireProfilesAndGuides(definition), hiddenBodies);
    }

    return adjacentFacesEditingLogic(context, oldDefinition, definition, specifiedParameters, hiddenBodies);
}

function adjacentFacesEditingLogic(context is Context, oldDefinition is map, definition is map,
                                   specifiedParameters is map, hiddenBodies is Query) returns map
{
    var profileArrayKey;
    var profileSubqueryKey;
    if (definition.bodyType == ExtendedToolBodyType.SOLID)
    {
        profileArrayKey = "sheetProfilesArray";
        profileSubqueryKey = "sheetProfileEntities";
    }
    else
    {
        profileArrayKey = "wireProfilesArray";
        profileSubqueryKey = "wireProfileEntities";
    }
    if (definition[profileArrayKey] == undefined || size(definition[profileArrayKey]) == 0)
    {
        return definition;
    }
    if (specifiedParameters.adjacentFacesStart != true)
    {
        definition.adjacentFacesStart = populateAjdacentFacesIfNeeded(context, definition, oldDefinition, hiddenBodies, "startCondition", profileArrayKey, profileSubqueryKey, 0, "adjacentFacesStart");
    }

    if (size(definition[profileArrayKey]) == 1)
    {
        return definition;
    }
    if (specifiedParameters.adjacentFacesEnd != true)
    {
        definition.adjacentFacesEnd = populateAjdacentFacesIfNeeded(context, definition, oldDefinition, hiddenBodies, "endCondition", profileArrayKey, profileSubqueryKey, size(definition[profileArrayKey]) - 1, "adjacentFacesEnd");
    }
    return definition;
}

function populateAjdacentFacesIfNeeded(context is Context, definition is map, oldDefinition is map,  hiddenBodies is Query,
            condition is string, profileArray is string, profileSubquery is string, startEnd is number, reference is string) returns Query
{
    if (definition[condition] != LoftEndDerivativeType.MATCH_TANGENT &&
        definition[condition] != LoftEndDerivativeType.MATCH_CURVATURE)
    {
        return definition[reference];
    }
    const profileQ = definition[profileArray][startEnd][profileSubquery];
    if (isQueryEmpty(context, profileQ))
    {
        return qNothing();
    }
    // No new profile elements
    if (oldDefinition[profileArray] != undefined &&
        (oldDefinition[condition] == LoftEndDerivativeType.MATCH_TANGENT ||
        oldDefinition[condition] == LoftEndDerivativeType.MATCH_CURVATURE))
    {
        const oldProfileQ = oldDefinition[profileArray][startEnd][profileSubquery];
        if (evaluateQuery(context, profileQ) == evaluateQuery(context, oldProfileQ))
        {
            return definition[reference];
        }
    }
    const faces = qEntityFilter(profileQ, EntityType.FACE);
    if (!isQueryEmpty(context, faces))
    {
        return getAdjacentFacesOfSheetProfiles(context, faces, hiddenBodies);
    }
    else
    {
        return getAdjacentFacesOfWireProfiles(context, profileQ, hiddenBodies);
    }
}

function getAdjacentFacesOfSheetProfiles(context is Context, profileQ is Query,  hiddenBodies is Query) returns Query
{
    var faces = [];
    for (var face in evaluateQuery(context, profileQ))
    {
        var adjacentFaceQ = qAdjacent(face, AdjacencyType.EDGE, EntityType.FACE);
        faces = append(faces, qSubtraction(adjacentFaceQ, profileQ));
    }
    return qUnion(faces);
}

/**
 * @internal
 * Manipulator function for loft feature.
 */
export function loftManipulator(context is Context, definition is map, newManipulators is map) returns map
{
    var manipulatorKeys = keys(newManipulators);
    const regexStr = "(\\d+),(\\d+)";
    for (var ii = 0; ii < size(manipulatorKeys); ii += 1)
    {
        var manipulator = newManipulators[manipulatorKeys[ii]];
        if (abs(manipulator.offset) < TOLERANCE.zeroLength * meter)
            continue;
        var parsed = match(manipulatorKeys[ii], regexStr);
        if (!parsed.hasMatch)   // must be bug
        {
            throw regenError("Cannot parse an entry in manipulators map");
        }
        var connectorIndex = stringToNumber(parsed.captures[1]);
        var edgeIndex = stringToNumber(parsed.captures[2]);
        var pos = manipulator.base + manipulator.direction * manipulator.offset;
        var qEdges = evaluateQuery(context, definition.connections[connectorIndex].connectionEdgeQueries);
        var distanceResult is DistanceResult = evDistance(context, { "side0" : pos, "side1" : qEdges[edgeIndex],
                "arcLengthParameterization" : fsConnectionsArcLengthParameterization });
        var parameter = distanceResult.sides[1].parameter;
        if (parameter < EDGE_INTERIOR_PARAMETER_BOUNDS[0])
        {
            parameter = EDGE_INTERIOR_PARAMETER_BOUNDS[0];
        }
        else if (parameter > EDGE_INTERIOR_PARAMETER_BOUNDS[2])
        {
            parameter = EDGE_INTERIOR_PARAMETER_BOUNDS[2];
        }
        definition.connections[connectorIndex].connectionEdgeParameters[edgeIndex] = parameter;
    }
    return definition;
}

function addConnectionManipulators(context is Context, id is Id, definition is map)
{
    if (!definition.matchConnections)
    {
        return;
    }
    for (var ii = 0; ii < size(definition.connections); ii += 1)
    {
        var connection = definition.connections[ii];
        var qEdges = evaluateQuery(context, connection.connectionEdgeQueries);
        for (var jj = 0; jj < size(qEdges); jj += 1)
        {
            var line = evEdgeTangentLine(context, { "edge" : qEdges[jj], "parameter" : connection.connectionEdgeParameters[jj],
                        "arcLengthParameterization" : fsConnectionsArcLengthParameterization });
            addManipulators(context, id,    { toString(ii) ~ "," ~ toString(jj) :
                                linearManipulator({
                                                "base" : line.origin,
                                                "direction" : line.direction,
                                                "offset" : 0 * inch,
                                                "style" : ManipulatorStyleEnum.TANGENTIAL
                                                })
                                            });
        }
    }
}

function updateConnections(context is Context, connections is array) returns array
{
    for (var ii = 0; ii < size(connections); ii += 1)
    {
        // User actions may result in connections[ii].connectionEntities changes (QLV)
        // The order of connectionEntities returned by QLV evaluation is not deterministic with an exception of qUnion
        // connectionEdgeQueries uses qUnion to assure synchronization with connectionEdgeParameters.
        // when connectionEntities QLV changes, we need to change both connectionEdgeQueries QLV and connectionEdgeParameters
        // Since connectionEdgeQueries are changed only during editing logic call, they may be used to find
        // the now to old connection correspondence
        var oldQueries = evaluateQuery(context, connections[ii].connectionEdgeQueries);

        // At this point oldQueries array is syncronized with connectionEdgeParameters.
        // Avoid n^2 algorithm by taking advantage of FS maps ordering entries by key
        var mapOld = {};
        for (var jj = 0; jj < size(oldQueries); jj += 1)
        {
            mapOld[oldQueries[jj]] = connections[ii].connectionEdgeParameters[jj];
        }

        var nowQueries = evaluateQuery(context, qEntityFilter(connections[ii].connectionEntities, EntityType.EDGE));
        const nowSize = size(nowQueries);
        // Construct current connectionEdgeParameters
        var nowParameters = [];
        for (var jj = 0; jj < nowSize; jj += 1)
        {
            var oldParameter = mapOld[nowQueries[jj]];
            if (oldParameter == undefined)
            {
                // TODO - Need to do better.see BEL-148540
                var value = EDGE_INTERIOR_PARAMETER_BOUNDS[1];
                nowParameters = append(nowParameters, value);
            }
            else
            {
                nowParameters = append(nowParameters, oldParameter);
            }
        }
        connections[ii].connectionEdgeParameters = nowParameters;
        // to make sure that connectionEdgeQueries are updated only by editing logic.
        // E.G. change propagation via merge does not update it
        connections[ii].connectionEdgeQueries = qUnion(nowQueries);
    }
    return connections;
}

function wireProfilesAndGuides(definition is map) returns Query
{
    var subqueries = [];
    if (undefined != definition.wireProfilesArray)
    {
        subqueries = concatenateArrays([subqueries, collectSubParameters(definition.wireProfilesArray, "wireProfileEntities")]);
    }
    if (undefined != definition.guidesArray)
    {
        subqueries = concatenateArrays([subqueries, collectSubParameters(definition.guidesArray, "guideEntities")]);
    }
    return qUnion(subqueries);
}

function replaceWireSubQueriesWithDependencies(context is Context, query is Query) returns Query
precondition
{
    query.subqueries is array;
}
{
    const count = size(query.subqueries);
    for (var index = 0; index < count; index += 1)
    {
        query.subqueries[index] = followWireEdgesToLaminarSource(context, query.subqueries[index]);
    }
    return query;
}

function edgeMatchesChain(context is Context, edge is Query, edges is array) returns boolean
{
    var parameters;
    var testSetQ;
    if (size(evaluateQuery(context, qAdjacent(edge, AdjacencyType.VERTEX, EntityType.VERTEX))) == 2)
    {
        parameters = [0., 1.];
        testSetQ = qAdjacent(qUnion(edges), AdjacencyType.VERTEX, EntityType.VERTEX);
    }
    else // closed edge
    {
        parameters = [0.23, 0.56, 0.91];   // "random parameters"
        testSetQ = qUnion(edges);
    }

    const edgeLines = evEdgeTangentLines(context, {
                "edge" : edge,
                "parameters" : parameters,
                "arcLengthParameterization" : false
                });
    for (var line in edgeLines)
    {
        if (isQueryEmpty(context, qContainsPoint(testSetQ, line.origin)))
        {
            return false;
        }
    }
    return true;

}

function replaceSketchFaceWithWireEdges(context is Context, query is Query) returns Query
{
    var sketchFaces = qSketchFilter(qEntityFilter(query, EntityType.FACE), SketchObject.YES);
    if (isQueryEmpty(context, sketchFaces))
    {
        return query;
    }
    else
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1031_BODY_NET_IN_LOFT))
        {
            // Check that the set of edges we replace with a dependency matches dependency geometry
            // BEL-111916
            const faceEdgesQ = qAdjacent(sketchFaces, AdjacencyType.EDGE, EntityType.EDGE);
            var dependencyToEdges = {};
            var edgesToUse = [];
            for (var edge in evaluateQuery(context, faceEdgesQ))
            {
                const adjacentSelectedFaceQ = qIntersection([qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE), sketchFaces]);
                //consider only boundary edges of selected faceSet
                if (size(evaluateQuery(context, adjacentSelectedFaceQ)) != 1)
                {
                    continue;
                }
                const dependencies = evaluateQuery(context, qDependency(edge));
                if (size(dependencies) != 1)
                {
                    return query;
                }
                if (dependencyToEdges[dependencies[0]] == undefined)
                {
                    dependencyToEdges[dependencies[0]] = [edge];
                }
                else
                {
                    dependencyToEdges[dependencies[0]] = append(dependencyToEdges[dependencies[0]], edge);
                }
                edgesToUse = append(edgesToUse, edge);
            }
            for (var dependencyAndEdges in dependencyToEdges)
            {
                if (!edgeMatchesChain(context, dependencyAndEdges.key, dependencyAndEdges.value))
                {
                    return query;
                }
            }
            return qDependency(qUnion(edgesToUse));
        }
        return qDependency(qAdjacent(sketchFaces, AdjacencyType.EDGE, EntityType.EDGE));
    }
}

/** @internal **/
export function replaceEndSketchFacesWithWireEdges(context is Context, queries is array) returns array
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V657_SURFACE_JOIN_BUGS))
    {
        return queries;
    }

    const count = size(queries);
    if (count > 0)
    {
        queries[0] = replaceSketchFaceWithWireEdges(context, queries[0]);
    }

    if (count > 1)
    {
        queries[count - 1] = replaceSketchFaceWithWireEdges(context, queries[count - 1]);
    }

    return queries;
}

/** @internal **/
export function replaceWireQueriesWithDependencies(context is Context, queries is array, firstAndLastOnly is boolean) returns array
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V576_GET_WIRE_LAMINAR_DEPENDENCIES))
    {
        return queries;
    }

    const count = size(queries);
    for (var index = 0; index < count; index += 1)
    {
        if (firstAndLastOnly && index != 0 && index != count - 1)
        {
            continue;
        }
        queries[index] = replaceWireSubQueriesWithDependencies(context, queries[index]);
    }

    return queries;
}

/** @internal **/
export function collectSubParameters(parameterArray is array, parameterName is string) returns array
{
    var retSubParameters = [];

    for (var param in parameterArray)
    {
        retSubParameters = append(retSubParameters, param[parameterName]);
    }

    return retSubParameters;
}

function collectGuideDerivatives(context is Context, definition is map) returns array
{
    var derivatives = [];

    for (var index = 0; index < size(definition.guidesArray); index +=1)
    {
        var parameter = definition.guidesArray[index];
        if (parameter.guideDerivativeType != LoftGuideDerivativeType.DEFAULT)
        {
            if (parameter.guideDerivativeType == LoftGuideDerivativeType.NORMAL_TO_GUIDE || parameter.guideDerivativeType == LoftGuideDerivativeType.TANGENT_TO_GUIDE)
            {
                var derivativeInfo = { "profileIndex" : index,
                               "magnitude" : parameter.guideDerivativeMagnitude,
                               "tangentToPlane" : parameter.guideDerivativeType == LoftGuideDerivativeType.TANGENT_TO_GUIDE,
                               "forGuide" : true };
                var planeResult = try(evOwnerSketchPlane(context, {
                        "entity" : parameter.guideEntities,
                        "checkAllEntities" : true
                    }));
                if (planeResult is Plane)
                {
                    derivativeInfo.vector = normalize(planeResult.normal);
                }
                else
                {
                    planeResult = try(evPlanarEdges(context, {
                        "edges" : parameter.guideEntities
                    }));
                    if (planeResult is Plane)
                    {
                        derivativeInfo.vector = normalize(planeResult.normal);
                    }
                    else
                    {
                        throw regenError(ErrorStringEnum.LOFT_PLANAR_GUIDE_NOT_FOUND, parameter.guideEntities);
                    }
                }
                derivatives = append(derivatives, derivativeInfo);
            }
            else
            {
                const adjacentFaceQuery = qAdjacent(parameter.guideEntities, AdjacencyType.EDGE, EntityType.FACE);
                if (isQueryEmpty(context, adjacentFaceQuery))
                {
                    throw regenError(ErrorStringEnum.LOFT_NO_FACE_FOR_GUIDE_CLAMP, parameter.guideEntities);
                }
                var derivativeInfo = { "profileIndex" : index,
                             "magnitude" : parameter.guideDerivativeMagnitude,
                             "matchCurvature" : parameter.guideDerivativeType == LoftGuideDerivativeType.MATCH_CURVATURE,
                             "adjacentFaces" : adjacentFaceQuery,
                             "forGuide" : true };
                derivatives = append(derivatives, derivativeInfo);
            }
        }
    }

    return derivatives;
}

function createLoftTopologyMatchesForSurfaceJoin(context is Context, id is Id, definition is map, transform is Transform) returns array
{
    var matches = [];
    if (definition.bodyType == ExtendedToolBodyType.SURFACE && definition.surfaceOperationType == NewSurfaceOperationType.ADD)
    {
        var capEdgesQuery = qUnion([
            makeQuery(id, "CAP_EDGE", EntityType.EDGE, {}),
            makeQuery(id, "MID_CAP_EDGE", EntityType.EDGE, {})
        ]);
        var profileMatches = createTopologyMatchesForSurfaceJoin(context, id, definition, capEdgesQuery, qUnion(definition.profileSubqueries), transform);

        if (undefined != definition.guideSubqueries)
        {
            var guideMatches = createTopologyMatchesForSurfaceJoin(context, id, definition, makeQuery(id, "SWEPT_EDGE", EntityType.EDGE, {}), qUnion(definition.guideSubqueries), transform);
            matches = concatenateArrays([profileMatches, guideMatches]);
        }
        else
        {
            matches = profileMatches;
        }
        checkForNotJoinableSurfacesInScope(context, id, definition, matches);
    }
    return matches;
}

const arrayParameterMappingSheet = {
        'profileSubqueries' : ['wireProfilesArray', 'wireProfileEntities'],
        'guideSubqueries' : ['guidesArray', 'guideEntities']
        };

const arrayParameterMappingSolid = {
        'profileSubqueries' : ['sheetProfilesArray', 'sheetProfileEntities'],
        'guideSubqueries' : ['guidesArray', 'guideEntities']
        };

/*
 *  e.g "guideSubqueries[0]." is getting mapped to "guidesArray[0].guideEntities"
 */
function  mapOpLoftArrayParameters(arrayParameterId is string, isSolid is boolean)
{
    const matched = match(arrayParameterId, "(.+)(\\[[0-9]+\\]\\.)(.*)");
    if (!matched.hasMatch)
    {
        return undefined;
    }
    const strippedId = matched.captures[1];
    const substitutionArray = (isSolid) ? arrayParameterMappingSolid[strippedId] : arrayParameterMappingSheet[strippedId];
    if (substitutionArray == undefined)
    {
        return undefined;
    }
    return substitutionArray[0] ~ matched.captures[2] ~ substitutionArray[1];
}

function copyFaceOrVertexSelections(context is Context, profiles is array, arrayParameterMappingFrom is map, arrayParameterMappingTo is map) returns map
{
    var newProfiles = [];
    for ( var profileFrom in profiles)
    {
        const profileQ = profileFrom[arrayParameterMappingFrom.profileSubqueries[1]];
        const faceOrVertexQ = qUnion([qEntityFilter(profileQ, EntityType.FACE), qEntityFilter(profileQ, EntityType.VERTEX)]);
        if (!(profileQ is Query) || isQueryEmpty(context, faceOrVertexQ))
            continue;
        newProfiles = append(newProfiles, {arrayParameterMappingTo.profileSubqueries[1] : profileQ});
    }
    if (newProfiles == [])
        return {};
    return {arrayParameterMappingTo.profileSubqueries[0] : newProfiles};
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

function getSideSurfacesEdgeFaceGroup(context is Context, topLevelId is Id, id is Id, capEdges is Query, initialProfileEdges is Query) returns map
{
    var commonPlane = getEdgesCommmonPlane(context, capEdges);

    if (commonPlane == undefined)
    {
        const allProfileEdgesAreLines = evaluateQuery(context, capEdges)->size() == evaluateQuery(context, capEdges->qGeometry(GeometryType.LINE))->size();

        if (!allProfileEdgesAreLines)
        {
            reportFeatureWarning(context, topLevelId, ErrorStringEnum.THIN_LOFT_3D_PROFILE_TRIM_WARNING);
            return {};
        }

        try silent
        {
            commonPlane = evOwnerSketchPlane(context, {
                "entity" : initialProfileEdges,
                "checkAllEntities" : true
            });
        }

        if (commonPlane == undefined)
        {
            reportFeatureWarning(context, topLevelId, ErrorStringEnum.THIN_LOFT_FAILED_TO_FIND_TRIM_PLANES_WARNING);
            return {};
        }
    }

    opPlane(context, id + "constructionPlane", {
            "plane" : commonPlane
        });

    return {"edges" : capEdges, "face" : qCreatedBy(id + "constructionPlane", EntityType.FACE)};
}

function getSideSurfacesEdgeFaceGroups(context is Context, topLevelId is Id, id is Id, definition is map, bottomCapEdges is Query, topCapEdges is Query) returns array
{
    var sideSurfacesEdgeFaceGroups = [];

    const edgeGroupsForSideSurfaces = [bottomCapEdges, topCapEdges];
    const edgeGroupInitialProfiles = [definition.profileSubqueries[0], definition.profileSubqueries[definition.profileSubqueries->size() - 1]];

    for (var i = 0; i < edgeGroupsForSideSurfaces->size(); i += 1)
    {
        if (isQueryEmpty(context, edgeGroupsForSideSurfaces[i]))
        {
            continue;
        }

        const iterationId = id + i;
        const sideSurfacesEdgeFaceGroup = getSideSurfacesEdgeFaceGroup(context, topLevelId, iterationId, edgeGroupsForSideSurfaces[i], edgeGroupInitialProfiles[i]);
        if (sideSurfacesEdgeFaceGroup.face != undefined)
        {
            sideSurfacesEdgeFaceGroups = append(sideSurfacesEdgeFaceGroups, sideSurfacesEdgeFaceGroup);
        }
        else
        {
            return [];
        }
    }

    return sideSurfacesEdgeFaceGroups;
}

function thinLoft(context is Context, id is Id, definition is map)
{
    //For Thin wall creation - prepare thickness value
    definition = setWallThickness(definition);

    definition.bodyType = ExtendedToolBodyType.SURFACE;
    const tempSupportingGeometryId = id + "tempSupportingGeometryId";
    const surfaceLoftId = tempSupportingGeometryId + "surfaceLoft";

    try
    {
        opLoft(context, surfaceLoftId, definition);
    }
    catch (error)
    {
        // propagate display of isocurves and sections
        processSubfeatureStatus(context, id, {
                "subfeatureId" : surfaceLoftId,
                "featureParameterMappingFunction" :
                function(arrayParameterId)
                {
                    return mapOpLoftArrayParameters(arrayParameterId, false);
                },
                "propagateErrorDisplay" : true
        });
        throw error;
    }

    const surfaceBodies = evaluateQuery(context, qBodyType(qCreatedBy(surfaceLoftId, EntityType.BODY), BodyType.SHEET));

    var thickenDefinition = {
            "entities" : surfaceBodies->qUnion(),
            "thickness1" : definition.wallThickness_1,
            "thickness2" : definition.wallThickness_2
        };

    var sideSurfacesEdgeFaceGroups = [];
    if (definition.trimEnds)
    {
        const topCapEdges = qCapEntity(surfaceLoftId, CapType.END, EntityType.EDGE);
        const bottomCapEdges = qCapEntity(surfaceLoftId, CapType.START, EntityType.EDGE);
        sideSurfacesEdgeFaceGroups = getSideSurfacesEdgeFaceGroups(context, id, tempSupportingGeometryId, definition, bottomCapEdges, topCapEdges);

        if (sideSurfacesEdgeFaceGroups != [])
        {
            thickenDefinition.sideGeometryRule = { "type" : SideGeometryRule.SUPPLIED, "sideSurfacesEdgeFaceGroups" : sideSurfacesEdgeFaceGroups };
        }
    }

    try
    {
        opThicken(context, id + "thicken", thickenDefinition);
    }
    catch (error)
    {
        const message = try(error.message as ErrorStringEnum);
        const overrideStatus = message == ErrorStringEnum.THICKEN_FAILED ? ErrorStringEnum.THIN_LOFT_THICKEN_FAILED : message;
        processSubfeatureStatus(context, id, { "subfeatureId" : id + "thicken", "propagateErrorDisplay" : true, "overrideStatus" : overrideStatus });
        throw regenError(overrideStatus);
    }

    // propagate display of isocurves and sections
    processSubfeatureStatus(context, id, {
            "subfeatureId" : surfaceLoftId,
            "propagateErrorDisplay" : true
    });

    opDeleteBodies(context, id + "deleteTempSupportingGeometry", {
                "entities" : qCreatedBy(tempSupportingGeometryId, EntityType.BODY) });
}

