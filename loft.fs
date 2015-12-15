FeatureScript 275; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/tool.fs", version : "");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/evaluate.fs", version : "");
import(path : "onshape/std/boolean.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/surfaceGeometry.fs", version : "");
import(path : "onshape/std/units.fs", version : "");
import(path : "onshape/std/valueBounds.fs", version : "");

/**
 * TODO: description
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
 * TODO: description
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

export const CLAMP_MAGNITUDE_REAL_BOUNDS =
{
    "min"      : -1e9,
    "max"      : 1e9,
    (unitless) : [-1e5, 1, 1e5]
} as RealBoundSpec;

/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
annotation { "Feature Type Name" : "Loft", "Filter Selector" : "allparts" }
export const loft = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {

        annotation { "Name" : "Creation type" }
        definition.bodyType is ToolBodyType;

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            booleanStepTypePredicate(definition);
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
            annotation { "Name" : "Guides", "Filter" : EntityType.EDGE && ConstructionObject.NO }
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
        const profileQuery = (definition.bodyType == ToolBodyType.SOLID) ? definition.sheetProfiles : definition.wireProfiles;
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

        if (definition.addGuides || definition.shapeControl == LoftShapeControlType.ADD_GUIDES)
        {
            definition.shapeControl = LoftShapeControlType.ADD_GUIDES;
            const guideQuery = definition.guides;
            if (guideQuery.queryType == QueryType.UNION)
            {
                const subQ = guideQuery.subqueries;
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

        opLoft(context, id, definition);

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            const reconstructOp = function(id) { opLoft(context, id, definition); };
            processNewBodyIfNeeded(context, id, definition, reconstructOp);
        }

    }, { makePeriodic : false, bodyType : ToolBodyType.SOLID, operationType : NewBodyOperationType.NEW, addGuides : false, matchVertices : false,
        shapeControl : LoftShapeControlType.DEFAULT, startCondition : LoftEndDerivativeType.DEFAULT, endCondition : LoftEndDerivativeType.DEFAULT,
        startMagnitude : 1, endMagnitude : 1 });

/**
 * TODO: description
 * @param context
 * @param endCondition
 * @param profileQuery
 * @param profileIndex
 * @param magnitude
 */
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

/**
 * TODO: description
 * @param context
 * @param subqueries
 */
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

