export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");
export import(path : "onshape/std/transform.fs", version : "");
export import(path : "onshape/std/print.fs", version : "");
export import(path : "onshape/std/featurescriptversionnumber.gen.fs", version : "");
export import(path : "onshape/std/errorstringenum.gen.fs", version : "");

export const CURRENT_VERSION = FeatureScriptVersionNumber.V115_SKETCH_LINEAR_PATTERNS;

//====================== Context ========================

export type Context typecheck canBeContext;

export predicate canBeContext(value)
{
    value is builtin;
}

export function newContext() returns Context
{
   return @newContext(CURRENT_VERSION) as Context;
}

//====================== Version compatibility ========================

/* Return false if a feature description [definition] contains
   a version number less than version [introduced] that changed behavior. */
export function isAtVersionOrLater(introduced is FeatureScriptVersionNumber,
                                   definition is map) returns boolean
{
    const asVersion = definition.asVersion;
    if (! (asVersion is FeatureScriptVersionNumber))
        return true;
    /* Map literals evaluate left to right. */
    for (var result in { (asVersion) : false, (introduced) : true })
        return result.value;
    return true; /* can't happen, but code analysis tools might complain */
}

//====================== Query evaluation ========================

export function evaluateQuery(context is Context, query is Query) returns array
{
    var out = @evaluateQuery(context, { "query" : query }).result;
    for(var i = 0; i < @size(out); i += 1)
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
    @reportFeatureError(context, id, {"message" : message});
    return true;
}

export function reportFeatureError(context is Context, id is Id, message is string, faultyParameters is array) returns boolean
{
    @reportFeatureError(context, id, {"message" : message, "faultyParameters": faultyParameters});
    return true;
}

export function reportFeatureWarning(context is Context, id is Id, message is string) returns boolean
{
    @reportFeatureWarning(context, id, {"message" : message});
    return true;
}

export function reportFeatureInfo(context is Context, id is Id, message is string) returns boolean
{
    @reportFeatureInfo(context, id, {"message" : message});
    return true;
}

//====================== operations ========================

//These functions are here because they are versioned with BelScript, while the builtin functions are not.
//So when we need to update a builtin, e.g. to @Fillet2, we can repoint opFillet to the new builtin without
//affecting old models.

export function opImportForeign(context is Context, id is Id, definition is map)
{
  return @opImportForeign(context, id, definition);
}

export function opDeleteBodies(context is Context, id is Id, definition is map)
{
  return @opDeleteBodies(context, id, definition);
}

export function opTransform(context is Context, id is Id, definition is map)
{
  return @opTransform(context, id, definition);
}

export function opBoolean(context is Context, id is Id, definition is map)
{
  return @opBoolean(context, id, definition);
}

export function opFillet(context is Context, id is Id, definition is map)
{
  return @opFillet(context, id, definition);
}

export function opChamfer(context is Context, id is Id, definition is map)
{
  return @opChamfer(context, id, definition);
}

export function opDraft(context is Context, id is Id, definition is map)
{
  return @opDraft(context, id, definition);
}

export function opExtrude(context is Context, id is Id, definition is map)
{
  return @opExtrude(context, id, definition);
}

export function opPattern(context is Context, id is Id, definition is map)
{
  return @opPattern(context, id, definition);
}

export function opPlane(context is Context, id is Id, definition is map)
{
  return @opPlane(context, id, definition);
}

export function opHelix(context is Context, id is Id, definition is map)
{
  return @opHelix(context, id, definition);
}

export function opRevolve(context is Context, id is Id, definition is map)
{
  return @opRevolve(context, id, definition);
}

export function opShell(context is Context, id is Id, definition is map)
{
  return @opShell(context, id, definition);
}

export function opSplitPart(context is Context, id is Id, definition is map)
{
  return @opSplitPart(context, id, definition);
}
export function opPoint(context is Context, id is Id, definition is map)
{
    return @opPoint(context, id, definition);
}
export function opSweep(context is Context, id is Id, definition is map)
{
    return @opSweep(context, id, definition);
}

export function opDeleteFace(context is Context, id is Id, definition is map)
{
  return @opDeleteFace(context, id, definition);
}

export function opMoveFace(context is Context, id is Id, definition is map)
{
  return @opMoveFace(context, id, definition);
}

export function opOffsetFace(context is Context, id is Id, definition is map)
{
  return @opOffsetFace(context, id, definition);
}

export function opReplaceFace(context is Context, id is Id, definition is map)
{
  return @opReplaceFace(context, id, definition);
}

export function opModifyFillet(context is Context, id is Id, definition is map)
{
  return @opModifyFillet(context, id, definition);
}

export function opMateConnector(context is Context, id is Id, definition is map)
{
  return @opMateConnector(context, id, definition);
}

export function opThicken(context is Context, id is Id, definition is map)
{
  return @opThicken(context, id, definition);
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
                if(started)
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
    if(result.result != undefined)
    {
        reportFeatureError(context, id, result.result);
        madeChanges = true;
    }
    result = getFeatureWarning(context, subId);
    if(result.result != undefined)
    {
        reportFeatureWarning(context, id, result.result);
        madeChanges = true;
    }
    result = getFeatureInfo(context, subId);
    if(result.result != undefined)
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
    if (@size(id) == 1 &&  @getFeatureError(context, id).result != undefined)
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
            @recordQuery(context, id, {paramEntry.key : paramEntry.value});
        }
    }
}

export function featureHasError(context is Context, id is Id) returns boolean
{
    return  @getFeatureError(context, id).result != undefined;
}

