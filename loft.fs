FeatureScript 581; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "581.0");
export import(path : "onshape/std/tool.fs", version : "581.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "581.0");
import(path : "onshape/std/evaluate.fs", version : "581.0");
import(path : "onshape/std/boolean.fs", version : "581.0");
import(path : "onshape/std/booleanHeuristics.fs", version : "581.0");
import(path : "onshape/std/feature.fs", version : "581.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "581.0");
import(path : "onshape/std/transform.fs", version : "581.0");
import(path : "onshape/std/units.fs", version : "581.0");
import(path : "onshape/std/valueBounds.fs", version : "581.0");
import(path : "onshape/std/vector.fs", version : "581.0");
import(path : "onshape/std/topologyUtils.fs", version : "581.0");

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
 * Specifies how the shape of the sides of a loft should be controlled.
 */
export enum LoftShapeControlType
{
    annotation { "Name" : "None" }
    DEFAULT,
    annotation { "Name" : "Guides" }
    ADD_GUIDES,
    annotation { "Name" : "End conditions" }
    ADD_END_CONDITIONS
}

/**
 * Feature performing an [opLoft].
 */
annotation { "Feature Type Name" : "Loft",
             "Filter Selector" : "allparts",
             "Editing Logic Function" : "loftEditLogic" }
export const loft = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {

        annotation { "Name" : "Creation type", "UIHint" : "HORIZONTAL_ENUM" }
        definition.bodyType is ToolBodyType;

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            booleanStepTypePredicate(definition);
        }
        else
        {
            surfaceOperationTypePredicate(definition);
        }

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            annotation { "Name" : "Profiles",
                         "Filter" : (EntityType.FACE || EntityType.VERTEX ||
                                    (EntityType.BODY && BodyType.SHEET))
                                    && ConstructionObject.NO }
            definition.sheetProfiles is Query;
        }
        else
        {
            annotation { "Name" : "Profiles",
                         "Filter" : (EntityType.VERTEX || EntityType.EDGE || EntityType.FACE ||
                                    (EntityType.BODY && (BodyType.WIRE || BodyType.SHEET)))
                                    && ConstructionObject.NO }
            definition.wireProfiles is Query;
        }

        annotation { "Name" : "Control type" }
        definition.shapeControl is LoftShapeControlType;

        if (definition.shapeControl == LoftShapeControlType.ADD_GUIDES)
        {
            annotation { "Name" : "Guides", "Filter" : (EntityType.EDGE && ConstructionObject.NO) || (EntityType.BODY && BodyType.WIRE) }
            definition.guides is Query;
        }
        else if (definition.shapeControl == LoftShapeControlType.ADD_END_CONDITIONS)
        {
            annotation { "Name" : "Start profile condition" }
            definition.startCondition is LoftEndDerivativeType;
            if (definition.startCondition != LoftEndDerivativeType.DEFAULT)
            {
                annotation { "Name" : "Start magnitude" }
                isReal(definition.startMagnitude, CLAMP_MAGNITUDE_REAL_BOUNDS);
            }

            annotation { "Name" : "End profile condition" }
            definition.endCondition is LoftEndDerivativeType;
            if (definition.endCondition != LoftEndDerivativeType.DEFAULT)
            {
                annotation { "Name" : "End magnitude" }
                isReal(definition.endMagnitude, CLAMP_MAGNITUDE_REAL_BOUNDS);
            }
        }

        annotation { "Name" : "Match vertices" }
        definition.matchVertices is boolean;
        if (definition.matchVertices)
        {
            annotation { "Name" : "Vertices", "Filter" : EntityType.VERTEX }
            definition.vertices is Query;
        }

        annotation { "Name" : "Make periodic", "UIHint" : "ALWAYS_HIDDEN" }
        definition.makePeriodic is boolean;

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            booleanStepScopePredicate(definition);
        }
    }
    {
        var profileQuery = definition.sheetProfiles;
        if (definition.bodyType != ToolBodyType.SOLID)
        {
            definition.wireProfileDependencies = replaceWireSubQueriesWithDependencies(context, definition.wireProfiles, true);
            profileQuery = definition.wireProfileDependencies;
        }
        if (profileQuery.queryType == QueryType.UNION)
        {
            const subQ = wrapSubqueriesInConstructionFilter(context, profileQuery.subqueries);
            if (size(subQ) < 1)
            {
                const errorEntities = (definition.bodyType == ToolBodyType.SOLID) ? "sheetProfiles" : "wireProfiles";
                throw regenError(ErrorStringEnum.LOFT_SELECT_PROFILES, [errorEntities]);
            }

            definition.profileSubqueries = subQ;
        }
        var queriesForTransform = [profileQuery];

        if (definition.addGuides || definition.shapeControl == LoftShapeControlType.ADD_GUIDES)
        {
            definition.shapeControl = LoftShapeControlType.ADD_GUIDES;
            definition.guideDependencies = replaceWireSubQueriesWithDependencies(context, definition.guides, false);
            queriesForTransform = append(queriesForTransform, definition.guideDependencies);
            if (definition.guideDependencies.queryType == QueryType.UNION)
            {
                const subQ = definition.guideDependencies.subqueries;
                definition.guideSubqueries = wrapSubqueriesInConstructionFilter(context, subQ);
            }
        }
        else if (definition.shapeControl == LoftShapeControlType.ADD_END_CONDITIONS)
        {
            var derivatives = [];
            if (definition.startCondition != LoftEndDerivativeType.DEFAULT)
            {
                derivatives = append(derivatives, createProfileConditions(context, definition.startCondition,
                                                            definition.profileSubqueries[0], 0, definition.startMagnitude));
            }
            if (definition.endCondition != LoftEndDerivativeType.DEFAULT)
            {
                const lastProfileIndex = @size(definition.profileSubqueries) - 1;
                derivatives = append(derivatives, createProfileConditions(context, definition.endCondition,
                                                            definition.profileSubqueries[lastProfileIndex], lastProfileIndex, definition.endMagnitude));
            }
            definition.derivativeInfo = derivatives;
        }

        if (!definition.matchVertices)
        {
            definition.vertices = qUnion([]);
        }
        else
        {
            queriesForTransform = append(queriesForTransform, definition.vertices);
        }

        var remainingTransform = getRemainderPatternTransform(context,
                {"references" : qUnion(queriesForTransform)});
        opLoft(context, id, definition);
        transformResultIfNecessary(context, id, remainingTransform);

        const reconstructOp = function(id) {
            opLoft(context, id, definition);
            transformResultIfNecessary(context, id, remainingTransform);
        };

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            processNewBodyIfNeeded(context, id, definition, reconstructOp);
        }
        else if (definition.surfaceOperationType == NewSurfaceOperationType.ADD)
        {
            var matches = createLoftTopologyMatchesForSurfaceJoin(context, id, definition, remainingTransform);
            joinSurfaceBodies(context, id, matches, reconstructOp);
        }

    }, { makePeriodic : false, bodyType : ToolBodyType.SOLID, operationType : NewBodyOperationType.NEW, addGuides : false, matchVertices : false,
        shapeControl : LoftShapeControlType.DEFAULT, startCondition : LoftEndDerivativeType.DEFAULT, endCondition : LoftEndDerivativeType.DEFAULT,
        startMagnitude : 1, endMagnitude : 1, surfaceOperationType : NewSurfaceOperationType.NEW });

/** @internal */
export function createProfileConditions(context is Context, endCondition is LoftEndDerivativeType, profileQuery is Query, profileIndex is number, magnitude is number) returns map
{
    if (endCondition == LoftEndDerivativeType.NORMAL_TO_PROFILE || endCondition == LoftEndDerivativeType.TANGENT_TO_PROFILE)
    {
        var derivativeInfo = { "profileIndex" : profileIndex,
                               "magnitude" : magnitude,
                               "tangentToPlane" : endCondition == LoftEndDerivativeType.TANGENT_TO_PROFILE };
        var planeResult = try(evPlane(context, { "face" : profileQuery }));
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
        const adjacentFaceQuery = qEdgeAdjacent(profileQuery, EntityType.FACE);
        if (@size(evaluateQuery(context, adjacentFaceQuery)) == 0)
        {
            throw regenError(profileIndex == 0 ? ErrorStringEnum.LOFT_NO_FACE_FOR_START_CLAMP : ErrorStringEnum.LOFT_NO_FACE_FOR_END_CLAMP);
        }
        const derivativeInfo = { "profileIndex" : profileIndex,
                                 "magnitude" : magnitude,
                                 "matchCurvature" : endCondition == LoftEndDerivativeType.MATCH_CURVATURE,
                                 "adjacentFaces" : qEdgeAdjacent(profileQuery, EntityType.FACE)};
        return derivativeInfo;
    }
}

/** @internal */
export function wrapSubqueriesInConstructionFilter(context is Context, subqueries is array) returns array
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V177_CONSTRUCTION_OBJECT_FILTER))
    {
        var wrappedSubqueries = [];
        for (var i = 0; i < @size(subqueries); i += 1)
        {
            wrappedSubqueries = append(wrappedSubqueries, qConstructionFilter(subqueries[i], ConstructionObject.NO));
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
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (definition.bodyType == ToolBodyType.SOLID)
    {
        return booleanStepEditLogic(context, id, oldDefinition, definition,
                                specifiedParameters, hiddenBodies, loft);
    }
    else
    {
        return surfaceOperationTypeEditLogic(context, id, definition, specifiedParameters, wireProfilesAndGuides(definition));
    }
}

function wireProfilesAndGuides(definition is map) returns Query
{
    var subqueries = [];
    if (undefined != definition.wireProfiles)
    {
        subqueries = append(subqueries, definition.wireProfiles);
    }
    if (undefined != definition.guides)
    {
        subqueries = append(subqueries, definition.guides);
    }
    return qUnion(subqueries);
}

function replaceWireSubQueriesWithDependencies(context is Context, query is Query, firstAndLastOnly is boolean) returns Query
precondition
{
    query.subqueries is array;
}
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V576_GET_WIRE_LAMINAR_DEPENDENCIES))
    {
        return query;
    }

    const count = size(query.subqueries);
    if (firstAndLastOnly)
    {
        if (count > 0)
        {
            query.subqueries[0] = followWireEdgesToLaminarSource(context, query.subqueries[0]);
        }
        if (count > 1)
        {
            query.subqueries[count - 1] = followWireEdgesToLaminarSource(context, query.subqueries[count - 1]);
        }
    }
    else
    {
        for (var index = 0; index < count; index += 1)
        {
            query.subqueries[index] = followWireEdgesToLaminarSource(context, query.subqueries[index]);
        }
    }
    return query;
}

function createLoftTopologyMatchesForSurfaceJoin(context is Context, id is Id, definition is map, transform is Transform) returns array
{
    var matches = [];
    if (definition.bodyType == ToolBodyType.SURFACE && definition.surfaceOperationType == NewSurfaceOperationType.ADD)
    {
        var profileMatches = createTopologyMatchesForSurfaceJoin(context, id, makeQuery(id, "MID_CAP_EDGE", EntityType.EDGE, {}), definition.wireProfileDependencies, transform);

        if (undefined != definition.guideDependencies)
        {
            var guideMatches = createTopologyMatchesForSurfaceJoin(context, id, makeQuery(id, "SWEPT_EDGE", EntityType.EDGE, {}), definition.guideDependencies, transform);
            matches = concatenateArrays([profileMatches, guideMatches]);
        }
        else
        {
            matches = profileMatches;
        }
    }
    return matches;
}

