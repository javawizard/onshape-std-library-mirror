FeatureScript 236; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/errorstringenum.gen.fs", version : "");

// Imports used internally
import(path : "onshape/std/context.fs", version : "");

/**
 * regenError functions are used to construct maps for throwing to signal feature regeneration errors.
 * @eg ```throw regenError(ErrorStringEnum.POINTS_COINCIDENT, ["points"]);```
 * Overloads allow for specifying parameters for the UI to indicate error state and error entities for the UI to
 * display in red.
 * @param message
 */
export function regenError(message is ErrorStringEnum)
{
    return { "message" : message };
}

export function regenError(message is ErrorStringEnum, faultyParameters is array)
{
    return { "message" : message, "faultyParameters" : faultyParameters };
}

export function regenError(message is ErrorStringEnum, entities is Query)
{
    return { "message" : message, "entities" : entities };
}

export function regenError(message is ErrorStringEnum, faultyParameters is array, entities is Query)
{
    return { "message" : message, "faultyParameters" : faultyParameters, "entities" : entities };
}

/**
 * TODO: description
 * @param context
 * @param id
 * @param error {{
 *      @field TODO
 * }}
 */
export function processError(context is Context, id is Id, error is map) returns boolean
{
    var messageEnum = try(error.message as ErrorStringEnum);
    if (messageEnum == undefined)
        messageEnum = ErrorStringEnum.REGEN_ERROR;
    @reportFeatureError(context, id, { "message" : messageEnum, "faultyParameters" : error.faultyParameters });
    if (error.entities != undefined)
        @setErrorEntities(context, id, error);
    return true;
}

export function processError(context is Context, id is Id, error is ErrorStringEnum) returns boolean
{
    @reportFeatureError(context, id, { "message" : error });
    return true;
}

export function processError(context is Context, id is Id, error) returns boolean // Default overload
{
    @reportFeatureError(context, id, { "message" : ErrorStringEnum.REGEN_ERROR });
    return true;
}

/**
 * TODO: description
 * @param context
 * @param id
 * @param message
 */
export function reportFeatureError(context is Context, id is Id, message is ErrorStringEnum) returns boolean
{
    @reportFeatureError(context, id, { "message" : message });
    return true;
}

export function reportFeatureError(context is Context, id is Id, message is ErrorStringEnum, faultyParameters is array) returns boolean
{
    @reportFeatureError(context, id, { "message" : message, "faultyParameters" : faultyParameters });
    return true;
}

/**
 * TODO: description
 * @param context
 * @param id
 * @param message
 */
export function reportFeatureWarning(context is Context, id is Id, message is ErrorStringEnum) returns boolean
{
    @reportFeatureWarning(context, id, { "message" : message });
    return true;
}

/**
 * TODO: description
 * @param context
 * @param id
 * @param message
 */
export function reportFeatureInfo(context is Context, id is Id, message is ErrorStringEnum) returns boolean
{
    @reportFeatureInfo(context, id, { "message" : message });
    return true;
}

/**
 * TODO: description
 * @param context
 * @param subId
 * @param id
 */
export function processSubfeatureStatus(context is Context, subId is Id, id is Id) returns boolean
{
    // If an operation contains sub-operations, e.g. an extruded boss is an extrusion and a boolean
    // then we want to propagate any errors/warning from the boolean (subId) to the extrusion (id)
    // We return true if anything was copied over

    var madeChanges = false;
    var result = getFeatureError(context, subId);
    if (result != undefined)
    {
        reportFeatureError(context, id, result);
        madeChanges = true;
    }
    result = getFeatureWarning(context, subId);
    if (result != undefined)
    {
        reportFeatureWarning(context, id, result);
        madeChanges = true;
    }
    result = getFeatureInfo(context, subId);
    if (result != undefined)
    {
        reportFeatureInfo(context, id, result);
        madeChanges = true;
    }

    return madeChanges;
}

/**
 * TODO: description
 * @param context
 * @param id
 */
export function getFeatureError(context is Context, id is Id)
{
    const result = @getFeatureError(context, id);
    return result == undefined ? undefined : result as ErrorStringEnum;
}

/**
 * TODO: description
 * @param context
 * @param id
 */
export function getFeatureWarning(context is Context, id is Id)
{
    const result = @getFeatureWarning(context, id);
    return result == undefined ? undefined : result as ErrorStringEnum;
}

/**
 * TODO: description
 * @param context
 * @param id
 */
export function getFeatureInfo(context is Context, id is Id)
{
    const result = @getFeatureInfo(context, id);
    return result == undefined ? undefined : result as ErrorStringEnum;
}

/**
 * TODO: description
 * @param context
 * @param id
 * @param definition {{
 *      @field TODO
 * }}
 */
export function setErrorEntities(context is Context, id is Id, definition is map)
{
    @setErrorEntities(context, id, definition);
}

/**
 * TODO: description
 * @param context
 * @param id
 */
export function featureHasError(context is Context, id is Id) returns boolean
{
    return @getFeatureError(context, id) != undefined;
}

