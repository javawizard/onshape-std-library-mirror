FeatureScript 2506; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2506.0");
export import(path : "onshape/std/tool.fs", version : "2506.0");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "2506.0");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "2506.0");
import(path : "onshape/std/booleanHeuristics.fs", version : "2506.0");
import(path : "onshape/std/containers.fs", version : "2506.0");
import(path : "onshape/std/evaluate.fs", version : "2506.0");
import(path : "onshape/std/feature.fs", version : "2506.0");
import(path : "onshape/std/string.fs", version : "2506.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2506.0");
import(path : "onshape/std/topologyUtils.fs", version : "2506.0");
import(path : "onshape/std/valueBounds.fs", version : "2506.0");
import(path : "onshape/std/vector.fs", version : "2506.0");


/**
 * Specifies an end condition for one side of a loft.
 */
export enum BSurfEndDerivativeType
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
 * How the boundary surface is computed
 */
export enum BSurfComputationType
{
    annotation { "Name": "Coons" }
    COONS,
    annotation { "Name": "Minimization" }
    MINIMIZATION
}

// Constants for profile parameter names
const PROFILE_ENTITIES = "ProfileEntities";
const CONDITION = "Condition";
const MAGNITUDE = "Magnitude";
const REFERENCE = "Reference";

/** UI definition for a profile.  The string uOrV must be either "u" or "v". */
predicate isProfile(profile is map, uOrV is string)
{
    annotation { "Name" : "Edges, curves and sketches",
                 "Filter" : (EntityType.EDGE || (EntityType.BODY && BodyType.WIRE && SketchObject.NO)) && AllowMeshGeometry.NO && ConstructionObject.NO }
    profile[uOrV ~ PROFILE_ENTITIES] is Query;

    annotation { "Name" : "Boundary condition", "UIHint" : UIHint.SHOW_LABEL }
    profile[uOrV ~ CONDITION] is BSurfEndDerivativeType;

    if (profile[uOrV ~ CONDITION] != BSurfEndDerivativeType.DEFAULT)
    {
        annotation { "Name" : "Magnitude" }
        isReal(profile[uOrV ~ MAGNITUDE], CLAMP_MAGNITUDE_REAL_BOUNDS);
    }

    if (profile[uOrV ~ CONDITION] == BSurfEndDerivativeType.MATCH_TANGENT ||
        profile[uOrV ~ CONDITION] == BSurfEndDerivativeType.MATCH_CURVATURE)
    {
        annotation { "Name" : "Faces",
                     "Filter" : EntityType.FACE && AllowMeshGeometry.NO }
        profile[uOrV ~ REFERENCE] is Query;
    }
}

 /**
  * Feature for testing Boundary Surface strategies
  */
annotation { "Feature Type Name" : "Boundary surface",
             "Filter Selector" : "allparts",
             "Editing Logic Function" : "bsurfEditLogic" }
export const boundarySurface = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        surfaceOperationTypePredicate(definition);

        annotation { "Name" : "U profiles", "Item name" : "profile",
            "Driven query" : "u" ~ PROFILE_ENTITIES, "Item label template" : "[#uCondition] #uProfileEntities",
            "UIHint" : UIHint.COLLAPSE_ARRAY_ITEMS, "MaxNumberOfPicks" : 2 }
        definition.uProfilesArray is array;
        for (var profile in definition.uProfilesArray)
        {
            isProfile(profile, "u");
        }

        annotation { "Name" : "V profiles", "Item name" : "profile",
            "Driven query" : "v" ~ PROFILE_ENTITIES, "Item label template" : "[#vCondition] #vProfileEntities",
            "UIHint" : UIHint.COLLAPSE_ARRAY_ITEMS, "MaxNumberOfPicks" : 2 }
        definition.vProfilesArray is array;
        for (var profile in definition.vProfilesArray)
        {
            isProfile(profile, "v");
        }

        annotation { "Name" : "Show isocurves" }
        definition.showIsocurves is boolean;

        if (definition.showIsocurves)
        {
            annotation { "Name" : "Count" }
            isInteger(definition.curveCount, ISO_GRID_BOUNDS);
        }

        surfaceJoinStepScopePredicate(definition);
    }
    {
        definition.uProfileSubqueries = [];
        definition.vProfileSubqueries = [];
        {
            definition.uProfileSubqueries = collectSubParameters(definition.uProfilesArray, "u" ~ PROFILE_ENTITIES);
            definition.vProfileSubqueries = collectSubParameters(definition.vProfilesArray, "v" ~ PROFILE_ENTITIES);
        }

        definition.uProfileSubqueries = wrapSubqueriesInFilters(context, definition.uProfileSubqueries);
        definition.vProfileSubqueries = wrapSubqueriesInFilters(context, definition.vProfileSubqueries);

        if (countNonEmptyQueries(context, definition.uProfileSubqueries) + countNonEmptyQueries(context, definition.vProfileSubqueries) < 2)
        {
            throw regenError(ErrorStringEnum.BSURF_2_PROFILES, ["uProfilesArray", "vProfilesArray"]);
        }


        var queriesForTransform = definition.uProfileSubqueries;
        queriesForTransform = concatenateArrays([definition.uProfileSubqueries, definition.vProfileSubqueries]);
        var uDerivatives = [];
        var vDerivatives = [];

        for (var profileIndex, profile in definition.uProfilesArray)
        {
            if (profile["u" ~ CONDITION] != BSurfEndDerivativeType.DEFAULT && isNonEmptyQuery(context, profile["u" ~ PROFILE_ENTITIES]))
            {
                uDerivatives = append(uDerivatives, createProfileConditions(context, profile["u" ~ CONDITION],
                                                            profile["u" ~ PROFILE_ENTITIES], profileIndex, profile["u" ~ MAGNITUDE],
                                                            profile["u" ~ REFERENCE], "uProfilesArray"));
            }
        }
        for (var profileIndex, profile in definition.vProfilesArray)
        {
            if (profile["v" ~ CONDITION] != BSurfEndDerivativeType.DEFAULT && isNonEmptyQuery(context, profile["v" ~ PROFILE_ENTITIES]))
            {
                vDerivatives = append(vDerivatives, createProfileConditions(context, profile["v" ~ CONDITION],
                                                            profile["v" ~ PROFILE_ENTITIES], profileIndex, profile["v" ~ MAGNITUDE],
                                                            profile["v" ~ REFERENCE], "vProfilesArray"));
            }
        }

        definition.uDerivativeInfo = uDerivatives;
        definition.vDerivativeInfo = vDerivatives;

        var remainingTransform = getRemainderPatternTransform(context,
                {"references" : qUnion(queriesForTransform)});

        // it is not a subfeature, but need to remap parameter ids
        callSubfeatureAndProcessStatus(id, opBoundarySurface, context, id, definition, {
                    "featureParameterMappingFunction" :
                        function(arrayParameterId)
                        {
                            return mapBoundarySurfaceArrayParameters(arrayParameterId);
                        }
                });

        transformResultIfNecessary(context, id, remainingTransform);

        const reconstructOp = function(id) {
            try silent(opBoundarySurface(context, id, definition));
            transformResultIfNecessary(context, id, remainingTransform);
        };

        var makeSolid = false;

        if (definition.surfaceOperationType == NewSurfaceOperationType.ADD)
        {
            joinSurfaceBodiesWithAutoMatching(context, id, definition, makeSolid, reconstructOp);
        }

    }, { surfaceOperationType : NewSurfaceOperationType.NEW,
        defaultSurfaceScope : true,
        showIsocurves : false,
        refineEdgeConnectivity : true,
        refineFaceConnectivity : false });

function createProfileConditions(context is Context, endCondition is BSurfEndDerivativeType, profileQuery is Query,
                                    profileIndex is number, magnitude is number, reference is Query, parameterId is string) returns map
{
    if (endCondition == BSurfEndDerivativeType.NORMAL_TO_PROFILE || endCondition == BSurfEndDerivativeType.TANGENT_TO_PROFILE)
    {
        var derivativeInfo = { "profileIndex" : profileIndex,
                               "magnitude" : magnitude,
                               "tangentToPlane" : endCondition == BSurfEndDerivativeType.TANGENT_TO_PROFILE };
        //If profile consists of sketch edges, use the sketch plane
        const planeResult = try(evOwnerSketchPlane(context, {"entity" : profileQuery }));
        if (planeResult is Plane)
        {
           derivativeInfo.vector = normalize(planeResult.normal);
        }
        else
        {
            throw regenError(profileIndex == 0 ? ErrorStringEnum.LOFT_NO_PLANE_FOR_START_CLAMP : ErrorStringEnum.LOFT_NO_PLANE_FOR_END_CLAMP);
        }
        return derivativeInfo;
    }
    else if (endCondition == BSurfEndDerivativeType.MATCH_TANGENT ||
             endCondition == BSurfEndDerivativeType.MATCH_CURVATURE)
    {
        if (isQueryEmpty(context, reference))
        {
            reference = getAdjacentFacesOfWireProfiles(context, profileQuery, qNothing() /* no hiddenBodies info here */);
        }
        if (isQueryEmpty(context, reference))
        {
            throw regenError(profileIndex == 0 ? ErrorStringEnum.LOFT_NO_FACE_FOR_START_CLAMP : ErrorStringEnum.LOFT_NO_FACE_FOR_END_CLAMP, [parameterId]);
        }
        const derivativeInfo = { "profileIndex" : profileIndex,
                                 "magnitude" : magnitude,
                                 "matchCurvature" : endCondition == BSurfEndDerivativeType.MATCH_CURVATURE,
                                 "adjacentFaces" : reference};
        return derivativeInfo;
    }
}

function wrapSubqueriesInFilters(context is Context, subqueries is array) returns array
{
    var wrappedSubqueries = [];
    for (var i = 0; i < @size(subqueries); i += 1)
    {
        var wrappedSubquery = subqueries[i]->qConstructionFilter(ConstructionObject.NO);
        wrappedSubquery = wrappedSubquery->qMeshGeometryFilter(MeshGeometry.NO);
        wrappedSubquery = qUnion([wrappedSubquery->qEntityFilter(EntityType.EDGE), wrappedSubquery->qEntityFilter(EntityType.BODY)->qBodyType(BodyType.WIRE)]);
        wrappedSubqueries = append(wrappedSubqueries, wrappedSubquery);
    }
    return wrappedSubqueries;
}

function countNonEmptyQueries(context is Context, queries is array) returns number
{
    var count = 0;
    for (var q in queries)
    {
        if (!isQueryEmpty(context, q))
            count += 1;
    }
    return count;
}

/**
 * @internal
 * Editing logic function for bsurf feature.
 */
export function bsurfEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    return surfaceOperationTypeEditLogic(context, id, definition,
                                    specifiedParameters, uAndVProfiles(definition), hiddenBodies);
}

function uAndVProfiles(definition is map) returns Query
{
    var subqueries = [];
    if (undefined != definition.uProfilesArray)
        subqueries = concatenateArrays([subqueries, collectSubParameters(definition.uProfilesArray, "u" ~ PROFILE_ENTITIES)]);
    if (undefined != definition.vProfilesArray)
        subqueries = concatenateArrays([subqueries, collectSubParameters(definition.vProfilesArray, "v" ~ PROFILE_ENTITIES)]);
    return qUnion(subqueries);
}

function collectSubParameters(parameterArray is array, parameterName is string) returns array
{
    var retSubParameters = [];

    for (var param in parameterArray)
    {
        retSubParameters = append(retSubParameters, param[parameterName]);
    }

    return retSubParameters;
}

const arrayParameterMappingSheet = {
        'uProfileSubqueries' : ['uProfilesArray', 'u' ~ PROFILE_ENTITIES],
        'vProfileSubqueries' : ['vProfilesArray', 'v' ~ PROFILE_ENTITIES]
        };

/*
 *  e.g "uProfileSubqueries[0]." is getting mapped to "uProfilesArray[0].uProfileEntities"
 */
function mapBoundarySurfaceArrayParameters(arrayParameterId is string)
{
    const matched = match(arrayParameterId, "(.+)(\\[[0-9]+\\]\\.)(.*)");
    if (!matched.hasMatch)
    {
        return undefined;
    }
    const strippedId = matched.captures[1];
    const substitutionArray = arrayParameterMappingSheet[strippedId];
    if (substitutionArray == undefined)
    {
        return undefined;
    }
    return substitutionArray[0] ~ matched.captures[2] ~ substitutionArray[1];
}

function isNonEmptyQuery(context is Context, query) returns boolean
{
     return (query != undefined && !isQueryEmpty(context, query));
}

