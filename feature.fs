FeatureScript 275; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports that most features will need to use.
export import(path : "onshape/std/context.fs", version : "");
export import(path : "onshape/std/error.fs", version : "");
export import(path : "onshape/std/geomOperations.fs", version : "");
export import(path : "onshape/std/query.fs", version : "");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/string.fs", version : "");
import(path : "onshape/std/transform.fs", version : "");

/**
 * This function takes a regeneration function and wraps it to create a feature. The wrapper handles certain argument
 * recording for the UI, default parameters, and error handling.  A typical usage is something like:
 * ```
 * annotation { "Feature Type Name" : "Widget" } // This annotation is required for Onshape to recognize widget as a feature.
 * export const widget = defineFeature(function(context is Context, id is Id, definition is map)
 *     precondition
 *     {
 *         ... // Specify the parameters that this feature takes
 *         definition.useMoreFillets is boolean;
 *     }
 *     {
 *     ... // Specify what the feature does when regenerating
 *     }, { "useMoreFillets" : false }); // if useMoreFillets is not passed, set it to false.
 * ```
 *
 * TODO: precondition spec, Manipulator Change Function, Editing Logic Function
 *
 * @param feature : A function that takes a `context`, and `id`, and a `definition` and regenerates the feature.
 * @param defaults : A map of default parameter values that are used to supplement the definition.
 */
export function defineFeature(feature is function, defaults is map) returns function
{
    return function(context is Context, id is Id, definition is map)
        {
            var started = false;
            try
            {
                //TODO: definition = @convert(definition, CurrentVersion);
                definition = mergeMaps(defaults, definition);
                startFeature(context, id, definition);
                definition.asVersion = undefined; // Don't let the feature body know if there's been an upgrade
                started = true;
                feature(context, id, definition);
                if (!isTopLevelId(id) && getFeatureError(context, id) != undefined)
                    throw regenError(getFeatureError(context, id));
                endFeature(context, id);
            }
            catch (error)
            {
                if (try(processError(context, id, error)) == undefined)
                    reportFeatureError(context, id, ErrorStringEnum.REGEN_ERROR);
                if (started)
                    abortFeature(context, id);
                if (!isTopLevelId(id))
                    throw error; // rethrow
            }
        };
}

export function defineFeature(feature is function) returns function
{
    return defineFeature(feature, {});
}

// =====================================================================
/**
 * For Onshape internal use.
 *
 * Starts the feature and associates the queries with the feature id in the context.
 */
export function startFeature(context is Context, id is Id, definition is map)
{
    @startFeature(context, id, definition);
    recordQueries(context, id, definition);
}

/**
 * For Onshape internal use.
 *
 * Rolls back the feature.
 */
export function abortFeature(context is Context, id is Id)
{
    @abortFeature(context, id);
}

/**
 * For Onshape internal use.
 *
 * Ends the feature; if the feature has an associated error, it is rolled back.
 */
export function endFeature(context is Context, id is Id)
{
    if (getFeatureError(context, id) != undefined)
    {
        @abortFeature(context, id);
    }
    else
    {
        @endFeature(context, id);
    }
}

/**
 * For Onshape internal use.
 *
 * Returns the id used by the innermost call to `startFeature`.  Temporary operations may be started by adding to this id.
 */
export function getCurrentSubfeatureId(context is Context) returns Id
{
    return @getCurrentSubfeatureId(context) as Id;
}

/**
 * For Onshape internal use.
 */
export function recordQueries(context is Context, id is Id, definition is map)
{
    for (var paramEntry in definition)
    {
        if (paramEntry.value is Query)
        {
            @recordQuery(context, id, { paramEntry.key : paramEntry.value });
        }
    }
}

/**
 * Associates a FeatureScript value with a given string. This value can then be referenced in a feature name using
 * the string. See `variable.fs` for an example of this usage.
 * @param definition {{
 *      @field name {string}
 *      @field value
 * }}
 */
export function setFeatureComputedParameter(context is Context, id is Id, definition is map)
{
    @setFeatureComputedParameter(context, id, definition);
}

/**
 * Builds stack of patternInstanceData, featureEnd/featureAbort on id parent pops the stack.
 * @param id {Id} : instance id
 * @param definition {{
 *      @field transform {Transform}
 *  }}
 */
export function setFeaturePatternInstanceData(context is Context, id is Id, definition is map)
{
    @setFeaturePatternInstanceData(context, id, definition);
}

/**
 * pop patternInstanceData stack if id matches throw otherwice
 * @param id {Id} : instance id
 */
export function unsetFeaturePatternInstanceData(context is Context, id is Id)
{
    @unsetFeaturePatternInstanceData(context, id);
}

/**
 * When in feature pattern scope returns composition of all pattern transforms pushed by setFeaturePatternInstanceData
 * returns identity transform when out of scope
 */
export function getFullPatternTransform(context is Context) returns Transform
{
    return transformFromBuiltin(@getFullPatternTransform(context));
}

/**
 *  Among references find topology created by pattern instance deepest in the stack.
 *  If transformation on the stack in that instance is S and full transformation is F, the remainder R is such that S*R = F
 *
 *  @param definition {{
 *      @field references {Query}
 *  }}
 */
export function getRemainderPatternTransform(context is Context, definition is map) returns Transform
precondition
{
    definition.references is Query;
}
{
    return transformFromBuiltin(@getRemainderPatternTransform(context, definition));
}

//====================== Query evaluation ========================

/**
 * Returns a list of the entities in a context which match a specified query.
 * The entities are returned in the form of transient queries, which are valid
 * only until the context is modified again.
 *
 * It is usually not necessary to evaluate queries, since operation and
 * evaluation functions can accept non-evaluated queries. Rather, the evaluated
 * queries can be used to count the number of entities (if any) that match a
 * query, or to iterate through the list to process entities individually.
 *
 * The order of entities returned by this function is arbitrary (and generally
 * not predictable) except in the case of a `qUnion` query. In that case, the
 * entities matched by earlier queries in the argument to `qUnion` are
 * returned first.
 *
 * @see `qTransient`
 */
export function evaluateQuery(context is Context, query is Query) returns array
{
    var out = @evaluateQuery(context, { "query" : query });
    for (var i = 0; i < @size(out); i += 1)
        out[i] = qTransient(out[i] as TransientId);
    return out;
}

//================ Compatibility with early expressions ================
export predicate isAnything(value) // used to create a generic feature parameter that can be any featurescript expression
{
}


/**
 * Returns id of operation that created or last modified the first entity to which query resolves
 * throws if query resolves to nothing
 * @param context
 * @param query
 */
export function lastModifyingOperationId(context is Context, query is Query) returns Id
{
    return @lastModifyingOperationId(context, {"entity" : query}) as Id;
}

/**
 * Parameter type that has a list of feature lambdas
 */
export type FeatureList typecheck canBeFeatureList;
export predicate canBeFeatureList(value)
{
    value is array;
    for (var entry in value)
    {
        entry is function;
    }
}

/**
 * Takes an array to return it as type FeatureList
 */
export function featureList(features is array) returns FeatureList
{
    return features as FeatureList;
}


