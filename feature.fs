FeatureScript 244; /* Automatically generated version */
// Imports that most features will need to use.
export import(path : "onshape/std/context.fs", version : "");
export import(path : "onshape/std/error.fs", version : "");
export import(path : "onshape/std/geomOperations.fs", version : "");
export import(path : "onshape/std/query.fs", version : "");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/string.fs", version : "");

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
                started = true;
                feature(context, id, definition);
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
    if (@size(id) == 1 && getFeatureError(context, id) != undefined)
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

