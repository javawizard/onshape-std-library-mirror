FeatureScript 347; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "347.0");
export import(path : "onshape/std/errorstringenum.gen.fs", version : "347.0");

// Imports used internally
import(path : "onshape/std/context.fs", version : "347.0");

/**
 * `regenError` functions are used to construct maps for throwing to signal feature regeneration errors.
 * Can either take a string for a custom message or an `ErrorStringEnum` for a built-in message.
 * Overloads allow for specifying parameters for the UI to indicate error state and error entities for the UI to
 * display in red.
 * @example `throw regenError("Failed to attach widget: Boolean union failed")`
 * @example `throw regenError("Wall is too thin for this feature", ["wallWidth"]);`
 * @example `throw regenError(ErrorStringEnum.POINTS_COINCIDENT, ["points"]);`
 *
 * @param customMessage : @autocomplete `"message"`
 */
export function regenError(customMessage is string)
{
    return { "message" : ErrorStringEnum.CUSTOM_ERROR, "customMessage" : customMessage };
}

/**
 * @param customMessage : @autocomplete `"message"`
 * @param faultyParameters : @autocomplete `["faultyParameter"]`
 */
export function regenError(customMessage is string, faultyParameters is array)
{
    return { "message" : ErrorStringEnum.CUSTOM_ERROR, "customMessage" : customMessage, "faultyParameters" : faultyParameters };
}
/**
 * @param customMessage : @autocomplete `"message"`
 */
export function regenError(customMessage is string, entities is Query)
{
    return { "message" : ErrorStringEnum.CUSTOM_ERROR, "customMessage" : customMessage, "entities" : entities };
}

/**
 * @param customMessage : @autocomplete `"message"`
 * @param faultyParameters : @autocomplete `["faultyParameter"]`
 */
export function regenError(customMessage is string, faultyParameters is array, entities is Query)
{
    return { "message" : ErrorStringEnum.CUSTOM_ERROR, "customMessage" : customMessage, "faultyParameters" : faultyParameters, "entities" : entities };
}

/**
 * The following overloads take an `ErrorStringEnum` rather than a custom
 * message, and are using for all errors withing the Onshape Standard Library.
 * The enum values correspond to messages which can be translated into multiple
 * languages.
 */
export function regenError(message is ErrorStringEnum)
{
    return { "message" : message };
}

/**
 * @param faultyParameters : @autocomplete `["faultyParameter"]`
 */
export function regenError(message is ErrorStringEnum, faultyParameters is array)
{
    return { "message" : message, "faultyParameters" : faultyParameters };
}

export function regenError(message is ErrorStringEnum, entities is Query)
{
    return { "message" : message, "entities" : entities };
}

/**
 * @param faultyParameters : @autocomplete `["faultyParameter"]`
 */
export function regenError(message is ErrorStringEnum, faultyParameters is array, entities is Query)
{
    return { "message" : message, "faultyParameters" : faultyParameters, "entities" : entities };
}

/**
 * @internal
 *
 * Used by defineFeature to try to process the thrown error by attaching it to the feature status and showing the
 * entities.
 * @param error {{
 *      @field message {ErrorStringEnum}
 *      @field entities {Query}
 * }}
 */
export function processError(context is Context, id is Id, error is map) returns boolean
{
    var messageEnum = try(error.message as ErrorStringEnum);
    if (messageEnum == undefined)
        messageEnum = ErrorStringEnum.REGEN_ERROR;
    @reportFeatureError(context, id, { "message" : messageEnum, "customMessage" : error.customMessage, "faultyParameters" : error.faultyParameters });
    if (error.entities != undefined)
        @setErrorEntities(context, id, error);
    return true;
}

/** @internal */
export function processError(context is Context, id is Id, error is ErrorStringEnum) returns boolean
{
    @reportFeatureError(context, id, { "message" : error });
    return true;
}

/** @internal */
export function processError(context is Context, id is Id, error) returns boolean // Default overload
{
    @reportFeatureError(context, id, { "message" : ErrorStringEnum.REGEN_ERROR });
    return true;
}

/**
 * @internal
 *
 * To report errors from features, use `throw regenError(...);`.
 *
 * Attaches an error to the given feature id. If it is a top-level id, when the feature finishes executing, it will
 * be rolled back.
 * @param id {Id}
 * @param message {ErrorStringEnum}
 */
export function reportFeatureError(context is Context, id is Id, message is ErrorStringEnum) returns boolean
{
    @reportFeatureError(context, id, { "message" : message });
    return true;
}

/** @internal */
export function reportFeatureError(context is Context, id is Id, message is ErrorStringEnum, faultyParameters is array) returns boolean
{
    @reportFeatureError(context, id, { "message" : message, "faultyParameters" : faultyParameters });
    return true;
}

/** @internal */
export function reportFeatureError(context is Context, id is Id, customMessage is string) returns boolean
{
    @reportFeatureError(context, id, { "message" : ErrorStringEnum.CUSTOM_ERROR, "customMessage" : customMessage });
    return true;
}

/**
 * Attaches a warning-level status to the given feature id.
 */
export function reportFeatureWarning(context is Context, id is Id, message is ErrorStringEnum) returns boolean
{
    @reportFeatureWarning(context, id, { "message" : message });
    return true;
}

/**
 * Attaches a custom warning-level status to the given feature id.
 */
export function reportFeatureWarning(context is Context, id is Id, customMessage is string) returns boolean
{
    @reportFeatureWarning(context, id, { "message" : ErrorStringEnum.CUSTOM_ERROR, "customMessage" : customMessage });
    return true;
}

/**
 * Attaches an info-level status to the given feature id.
 */
export function reportFeatureInfo(context is Context, id is Id, message is ErrorStringEnum) returns boolean
{
    @reportFeatureInfo(context, id, { "message" : message });
    return true;
}

/**
 * Attaches a custom info-level status to the given feature id.
 */
export function reportFeatureInfo(context is Context, id is Id, customMessage is string) returns boolean
{
    @reportFeatureInfo(context, id, { "message" : ErrorStringEnum.CUSTOM_ERROR, "customMessage" : customMessage });
    return true;
}

/**
 * This function propagates a warning or info from a subfeature to the current feature.
 *
 * @param subId : The id of the subfeature
 * @param id : The id of the current feature.
 */
// TODO: precondition check that `id` is the prefix of `subId`.
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
 * Returns the error (as a string or an `ErrorStringEnum`) associated with the given feature id or `undefined` if none.
 */
export function getFeatureError(context is Context, id is Id)
{
    const result = @getFeatureError(context, id);
    if (ErrorStringEnum[result] != undefined)
        return result as ErrorStringEnum;
    return result;
}

/**
 * Returns the warning (as a string or an `ErrorStringEnum`) associated with the given feature id or `undefined` if none.
 */
export function getFeatureWarning(context is Context, id is Id)
{
    const result = @getFeatureWarning(context, id);
    if (ErrorStringEnum[result] != undefined)
        return result as ErrorStringEnum;
    return result;
}

/**
 * Returns the info status (as a string or an `ErrorStringEnum`) associated with the given feature id or `undefined` if none.
 */
export function getFeatureInfo(context is Context, id is Id)
{
    const result = @getFeatureInfo(context, id);
    if (ErrorStringEnum[result] != undefined)
        return result as ErrorStringEnum;
    return result;
}

/**
 * Causes the given entities to be shown in red. This display is not rolled back even if the feature fails and
 * the entities themselves are rolled back.
 * @param definition {{
 *      @field entities {Query} : The entities to display.
 * }}
 */
export function setErrorEntities(context is Context, id is Id, definition is map)
{
    @setErrorEntities(context, id, definition);
}

/**
 * @param id
 * @returns {boolean} : `true` if the feature with the given id has an associated regeneration error.
 */
export function featureHasError(context is Context, id is Id) returns boolean
{
    return @getFeatureError(context, id) != undefined;
}

