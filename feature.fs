FeatureScript ✨; /* Automatically generated version */
// Imports that most features will need to use.
export import(path : "onshape/std/context.fs", version : "");
export import(path : "onshape/std/error.fs", version : "");
export import(path : "onshape/std/geomOperations.fs", version : "");
export import(path : "onshape/std/query.fs", version : "");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/string.fs", version : "");

/**
 * TODO: description
 * @param feature
 * @param defaults {{
 *      @field TODO
 * }}
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
 * TODO: description
 * @param context
 * @param id
 * @param definition {{
 *      @field TODO
 * }}
 */
export function startFeature(context is Context, id is Id, definition is map)
{
    @startFeature(context, id, definition);
    recordQueries(context, id, definition);
}

/**
 * TODO: description
 * @param context
 * @param id
 */
export function abortFeature(context is Context, id is Id)
{
    @abortFeature(context, id);
}

/**
 * TODO: description
 * @param context
 * @param id
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
 * TODO: description
 * @param context
 * @param id
 * @param definition {{
 *      @field TODO
 * }}
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
 * TODO: description
 * @param context
 * @param id
 * @param definition {{
 *      @field TODO
 * }}
 */
export function setFeatureComputedParameter(context is Context, id is Id, definition is map)
{
    @setFeatureComputedParameter(context, id, definition);
}


//====================== Query evaluation ========================

/**
 * TODO: description
 * @param context
 * @param query
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


