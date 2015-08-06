FeatureScript 189; /* Automatically generated version */
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");
export import(path : "onshape/std/transform.fs", version : "");
export import(path : "onshape/std/print.fs", version : "");
export import(path : "onshape/std/featurescriptversionnumber.gen.fs", version : "");
export import(path : "onshape/std/errorstringenum.gen.fs", version : "");

//====================== Context ========================

export type Context typecheck canBeContext;

export predicate canBeContext(value)
{
    value is builtin;
}

export function newContext() returns Context
{
    return @newContext(FeatureScriptVersionNumberCurrent) as Context;
}

//====================== Version compatibility ========================

/* Return false if the active feature is running at a version number
   at least as new as [introduced] that changed behavior. */
export function isAtVersionOrLater(context is Context, introduced is FeatureScriptVersionNumber) returns boolean
{
    return @isAtVersionOrLater(context, introduced);
}

//====================== Query evaluation ========================

export function evaluateQuery(context is Context, query is Query) returns array
{
    var out = @evaluateQuery(context, { "query" : query }).result;
    for (var i = 0; i < @size(out); i += 1)
        out[i] = qTransient(out[i] as TransientId);
    return out;
}

//====================== Error Reporting ===================

export function reportFeatureError(context is Context, id is Id, message is undefined) returns boolean
{
    return false;
}

export function reportFeatureError(context is Context, id is Id, message is undefined, faultyParameters) returns boolean
{
    return false;
}

export function reportFeatureError(context is Context, id is Id, message is string) returns boolean
{
    @reportFeatureError(context, id, { "message" : message });
    return true;
}

export function reportFeatureError(context is Context, id is Id, message is string, faultyParameters is array) returns boolean
{
    @reportFeatureError(context, id, { "message" : message, "faultyParameters" : faultyParameters });
    return true;
}

export function reportFeatureWarning(context is Context, id is Id, message is string) returns boolean
{
    @reportFeatureWarning(context, id, { "message" : message });
    return true;
}

export function reportFeatureInfo(context is Context, id is Id, message is string) returns boolean
{
    @reportFeatureInfo(context, id, { "message" : message });
    return true;
}

// =========================== defineFeature ===========================

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
            catch
            {
                reportFeatureError(context, id, ErrorStringEnum.REGEN_ERROR);
                if (started)
                    abortFeature(context, id);
            }
        };
}

export function defineFeature(feature is function) returns function
{
    return defineFeature(feature, {});
}

// =====================================================================
export function startFeature(context is Context, id is Id, definition is map)
{
    @startFeature(context, id, definition);
    recordQueries(context, id, definition);
}

export function processSubfeatureStatus(context is Context, subId is Id, id is Id) returns boolean
{
    // If an operation contains sub-operations, e.g. an extruded boss is an extrusion and a boolean
    // then we want to propagate any errors/warning from the boolean (subId) to the extrusion (id)
    // We return true if anything was copied over

    var madeChanges = false;
    var result = getFeatureError(context, subId);
    if (result.result != undefined)
    {
        reportFeatureError(context, id, result.result);
        madeChanges = true;
    }
    result = getFeatureWarning(context, subId);
    if (result.result != undefined)
    {
        reportFeatureWarning(context, id, result.result);
        madeChanges = true;
    }
    result = getFeatureInfo(context, subId);
    if (result.result != undefined)
    {
        reportFeatureInfo(context, id, result.result);
        madeChanges = true;
    }

    return madeChanges;
}

export function getFeatureError(context is Context, id is Id)
{
    return @getFeatureError(context, id);
}

export function getFeatureWarning(context is Context, id is Id)
{
    return @getFeatureWarning(context, id);
}

export function getFeatureInfo(context is Context, id is Id)
{
    return @getFeatureInfo(context, id);
}

export function setErrorEntities(context is Context, id is Id, definition is map)
{
    @setErrorEntities(context, id, definition);
}

export function abortFeature(context is Context, id is Id)
{
    @abortFeature(context, id);
}

export function endFeature(context is Context, id is Id)
{
    if (@size(id) == 1 && @getFeatureError(context, id).result != undefined)
    {
        @abortFeature(context, id);
    }
    else
    {
        @endFeature(context, id);
    }
}

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

export function featureHasError(context is Context, id is Id) returns boolean
{
    return @getFeatureError(context, id).result != undefined;
}

