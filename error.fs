FeatureScript 2473; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2473.0");
export import(path : "onshape/std/errorstringenum.gen.fs", version : "2473.0");

// Imports used internally
import(path : "onshape/std/context.fs", version : "2473.0");
import(path : "onshape/std/containers.fs", version : "2473.0");
import(path : "onshape/std/string.fs", version : "2473.0");

/**
 * `regenError` functions are used to construct maps for throwing to signal feature regeneration errors.
 * Can either take a string for a custom message or an `ErrorStringEnum` for a built-in message.
 * Custom messages are limited to ASCII characters. Messages longer than 200 characters will not
 * be displayed fully.
 * @example `throw regenError("Failed to attach widget: Boolean union failed")`
 * @example `throw regenError("Wall is too thin for this feature", ["wallWidth"]);`
 * @example `throw regenError(ErrorStringEnum.POINTS_COINCIDENT, ["points"]);`
 *
 * @param customMessage : @autocomplete `"message"`
 */
export function regenError(customMessage is string) returns map
{
    return { "message" : ErrorStringEnum.CUSTOM_ERROR, "customMessage" : customMessage };
}

/**
 * @param customMessage : @autocomplete `"message"`
 * @param faultyParameters : @autocomplete `["faultyParameter"]`
 * @param faultyParameters : An array of strings that correspond to keys in the feature definition
 *      map. Throwing a regenError with faultyParameters will highlight them in red inside the
 *      feature dialog.
 */
export function regenError(customMessage is string, faultyParameters is array) returns map
{
    return { "message" : ErrorStringEnum.CUSTOM_ERROR, "customMessage" : customMessage, "faultyParameters" : faultyParameters };
}
/**
 * @param entities : A query for entities to highlight in the Part Studio. Multiple queries can be
 *      combined and highlighted using the `qUnion` function. The entities are only highlighted
 *      when the feature dialog is open.
 * @param customMessage : @autocomplete `"message"`
 */
export function regenError(customMessage is string, entities is Query) returns map
{
    return { "message" : ErrorStringEnum.CUSTOM_ERROR, "customMessage" : customMessage, "entities" : entities };
}

/**
 * @param customMessage : @autocomplete `"message"`
 * @param faultyParameters : @autocomplete `["faultyParameter"]`
 * @param faultyParameters : An array of strings that correspond to keys in the feature definition
 *      map. Throwing a `regenError` with `faultyParameters` will highlight them in red inside the
 *      feature dialog.
 * @param entities : A query for entities to highlight in the Part Studio. Multiple queries can be
 *      combined and highlighted using the `qUnion` function. The entities are only highlighted
 *      when the feature dialog is open.
 */
export function regenError(customMessage is string, faultyParameters is array, entities is Query) returns map
{
    return { "message" : ErrorStringEnum.CUSTOM_ERROR, "customMessage" : customMessage, "faultyParameters" : faultyParameters, "entities" : entities };
}

/**
 * The following overloads take an `ErrorStringEnum` rather than a custom
 * message, and are using for all errors withing the Onshape Standard Library.
 * The enum values correspond to messages which can be translated into multiple
 * languages.
 */
export function regenError(message is ErrorStringEnum) returns map
{
    return { "message" : message };
}

/**
 * @param faultyParameters : @autocomplete `["faultyParameter"]`
 * @param faultyParameters : An array of strings that correspond to keys in the feature definition
 *      map. Throwing a `regenError` with `faultyParameters` will highlight them in red inside the
 *      feature dialog.
 */
export function regenError(message is ErrorStringEnum, faultyParameters is array) returns map
{
    return { "message" : message, "faultyParameters" : faultyParameters };
}

/**
 * @param entities : A query for entities to highlight in the Part Studio. Multiple queries can be
 *      combined and highlighted using the `qUnion` function. The entities are only highlighted
 *      when the feature dialog is open.
 */
export function regenError(message is ErrorStringEnum, entities is Query) returns map
{
    return { "message" : message, "entities" : entities };
}

/**
 * @param faultyParameters : @autocomplete `["faultyParameter"]`
 * @param faultyParameters : An array of strings that correspond to keys in the feature definition
 *      map. Throwing a `regenError` with `faultyParameters` will highlight them in red inside the
 *      feature dialog.
 * @param entities : A query for entities to highlight in the Part Studio. Multiple queries can be
 *      combined and highlighted using the `qUnion` function. The entities are only highlighted
 *      when the feature dialog is open.
 */
export function regenError(message is ErrorStringEnum, faultyParameters is array, entities is Query) returns map
{
    return { "message" : message, "faultyParameters" : faultyParameters, "entities" : entities };
}

/**
 * @param regenErrorOptions {{
 *     @field faultyParameters : An array of strings that correspond to keys in the feature definition
 *         map. Throwing a `regenError` with `faultyParameters` will highlight them in red inside the
 *         feature dialog.
 *     @field entities : A query for entities to highlight in the Part Studio. Multiple queries can be
 *         combined and highlighted using the `qUnion` function. The entities are only highlighted
 *         when the feature dialog is open.
 * }}
 */
export function regenError(message is ErrorStringEnum, regenErrorOptions is map) returns map
precondition
{
    regenErrorOptions.message == undefined;
    regenErrorOptions.customMessage == undefined;
    regenErrorOptions.faultyParameters == undefined || regenErrorOptions.faultyParameters is array;
    regenErrorOptions.entities == undefined || regenErrorOptions.entities is Query;
}
{
    regenErrorOptions.message = message;
    return regenErrorOptions;
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
    @functionReportFeatureStatus(context, id, {"statusType" : "ERROR", "statusEnum" : messageEnum, "statusMsg" : error.customMessage, "faultyParameters" : error.faultyParameters});
    if (error.entities != undefined)
        @setErrorEntities(context, id, error);
    return true;
}

/** @internal */
export function processError(context is Context, id is Id, error is ErrorStringEnum) returns boolean
{
    reportFeatureStatus(context, id, {"statusType" : StatusType.ERROR, "statusEnum" : error} as FeatureStatus);
    return true;
}

/** @internal */
export function processError(context is Context, id is Id, error) returns boolean // Default overload
{
    reportFeatureStatus(context, id, {"statusType" : StatusType.ERROR, "statusEnum" : ErrorStringEnum.REGEN_ERROR} as FeatureStatus);
    return true;
}

/**
 * @internal
 *
 * For parameter syntax errors
 */
export function syntaxError()
{
    try
    {
        throw regenError(ErrorStringEnum.PARAMETER_SYNTAX_ERROR);
    }
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
    reportFeatureStatus(context, id, {"statusType" : StatusType.ERROR, "statusEnum" : message} as FeatureStatus);
    return true;
}

/** @internal */
export function reportFeatureError(context is Context, id is Id, message is ErrorStringEnum, faultyParameters is array) returns boolean
{
    reportFeatureStatus(context, id, {"statusType" : StatusType.ERROR, "faultyParameters" : faultyParameters, "statusEnum" : message} as FeatureStatus);
    return true;
}

/** @internal */
export function reportFeatureError(context is Context, id is Id, customMessage is string) returns boolean
{
    reportFeatureStatus(context, id, {"statusType" : StatusType.ERROR, "statusEnum" : ErrorStringEnum.CUSTOM_ERROR, "statusMsg" :customMessage} as FeatureStatus);
    return true;
}

/**
 * Attaches a warning-level status to the given feature id.
 */
export function reportFeatureWarning(context is Context, id is Id, message is ErrorStringEnum) returns boolean
{
    reportFeatureStatus(context, id, {"statusType" : StatusType.WARNING, "statusEnum" : message} as FeatureStatus);
    return true;
}

/**
 * Attaches a custom warning-level status to the given feature id. Will display a notification to the user containing the specified message.
 */
export function reportFeatureWarning(context is Context, id is Id, customMessage is string) returns boolean
{
    reportFeatureStatus(context, id, {"statusType" : StatusType.WARNING, "statusEnum" : ErrorStringEnum.CUSTOM_ERROR, "statusMsg" : customMessage} as FeatureStatus);
    return true;
}

/**
 * Attaches custom warning-level status to the given feature id. Will display a notification to the user containing the specified message.
 */
export function reportFeatureWarning(context is Context, id is Id, message is ErrorStringEnum, associatedParameters is array) returns boolean
{
    reportFeatureStatus(context, id, {"statusType" : StatusType.WARNING,
                                        "statusEnum" : message,
                                        "faultyParameters" : associatedParameters} as FeatureStatus);
    return true;
}

/**
 * Attaches an info-level status to the given feature id. Will display a notification to the user containing the specified message.
 */
export function reportFeatureInfo(context is Context, id is Id, message is ErrorStringEnum) returns boolean
{
    reportFeatureStatus(context, id, {"statusType" : StatusType.INFO, "statusEnum" : message} as FeatureStatus);
    return true;
}

/**
 * Attaches an info-level status to the given feature id. Will display a notification to the user containing the specified message.
 */
export function reportFeatureInfo(context is Context, id is Id, message is ErrorStringEnum, associatedParameters is array) returns boolean
{
    reportFeatureStatus(context, id, {"statusType" : StatusType.INFO,
                                        "statusEnum" : message,
                                        "faultyParameters" : associatedParameters} as FeatureStatus);
    return true;
}

/**
 * Attaches a custom info-level status to the given feature id.
 */
export function reportFeatureInfo(context is Context, id is Id, customMessage is string) returns boolean
{
    reportFeatureStatus(context, id, {"statusType" : StatusType.INFO, "statusEnum" : ErrorStringEnum.CUSTOM_ERROR, "statusMsg" : customMessage} as FeatureStatus);
    return true;
}

/**
 * Propagate the status of a subfeature to a feature.
 * @param options {{
 *      @field subfeatureId {Id} : The Id of the subfeature.
 *      @field overrideStatus {ErrorStringEnum} : A status enum to use instead of the subfeature status enum if the
 *                                                subfeature has an info, warning, or error status. @optional
 *      @field featureParameterMap {map} : A mapping of the field names from subfeature to feature. @optional
 *      @field featureParameterMappingFunction {function} : A function to map field names from subfeature to feature. @optional
 *      @field propagateErrorDisplay {boolean} : Use subfeature error display when present.  Default is false. @optional
 *      @field additionalErrorEntities {Query} : Additional error entities to display if the subfeature has an info,
 *                                               warning, or error status. @optional
 * }}
 */
export function processSubfeatureStatus(context is Context, id is Id, options is map) returns boolean
{
    const subStatus is FeatureStatus = getFeatureStatus(context, options.subfeatureId);
    if (subStatus == undefined) // Should never happen, even status from nonexistent id returns OK status
    {
        return false;
    }
    var status = subStatus;
    const statusIsOk = (status.statusType == StatusType.OK);

    // = status enum =
    // Only have to check `statusEnum` because when a custom `statusMsg` is provided, the `statusEnum` is also set to
    // ErrorStringEnum.CUSTOM_ERROR
    if (!statusIsOk && options.overrideStatus != undefined)
    {
        status.statusMsg = undefined; // Unset custom message if the status is a string rather than an enum
        status.statusEnum = options.overrideStatus;
    }

    // = faulty parameters =
    status.faultyParameters = undefined;
    if (subStatus.faultyParameters != undefined)
    {
        var faultyParameters = [];
        const featureParameterMap = options.featureParameterMap;
        const mappingFunction = options.featureParameterMappingFunction;
        if (featureParameterMap != undefined || mappingFunction != undefined)
        {
            for (var param in subStatus.faultyParameters)
            {
                var mappedParam;
                if (featureParameterMap != undefined)
                {
                    mappedParam = featureParameterMap[param];
                }
                if (mappedParam == undefined && mappingFunction != undefined)
                {
                    mappedParam = mappingFunction(param);
                }
                if (mappedParam != undefined)
                {
                    faultyParameters = append(faultyParameters, mappedParam);
                }
            }
            if (size(faultyParameters) != 0)
            {
                status.faultyParameters = faultyParameters;
            }
        }
    }

    // = commit the status =
    reportFeatureStatus(context, id, status);

    // = error display =
    if (options.propagateErrorDisplay == true)
    {
        @transferSubfeatureErrorDisplay(context, id, { "subfeatureId" : options.subfeatureId });
    }
    if (!statusIsOk && options.additionalErrorEntities != undefined)
    {
        setErrorEntities(context, id, { "entities" : options.additionalErrorEntities });
    }

    return true;
}

/**
 * Return the status of a feature as a FeatureStatus
 * @param id {Id}
 */
export function getFeatureStatus(context is Context, id is Id) returns FeatureStatus
{
    var builtInStatus = @functionGetFeatureStatus(context, id);
    builtInStatus.statusType = builtInStatus.statusType as StatusType;
    if (ErrorStringEnum[builtInStatus.statusEnum] != undefined)
        builtInStatus.statusEnum = builtInStatus.statusEnum as ErrorStringEnum;
    return featureStatus(builtInStatus);
}

/**
 * Report the status of a feature
 * @param id {Id}
 * @param status {FeatureStatus}
 */
export function reportFeatureStatus(context is Context, id is Id, status is FeatureStatus) returns boolean
{
    @functionReportFeatureStatus(context, id, status);
    return true;
}

/**
 * Clear the status of a feature to StatusType.OK
 * * @param definition {{
 *      @field withDisplayData {boolean} : Clear status display data attached to feature. Default true. @optional
 * }}
 * @param id {Id}
 */
export function clearFeatureStatus(context is Context, id is Id, definition is map) returns boolean
{
    @clearFeatureStatus(context, id, definition);
    return true;
}

/**
 * @internal
 *
 * To get the statusEnum as ErrorStringEnum or the statusMsg as a string of a feature if feature status is of statusType
 * @param id {Id}
 * @param statusType {StatusType}
 */
function getFeatureStatusString(context is Context, id is Id, statusType is StatusType)
{
    var status is FeatureStatus = getFeatureStatus(context, id);
    if (status != undefined && status.statusType == statusType)
    {
        if (status.statusEnum != ErrorStringEnum.CUSTOM_ERROR)
            return status.statusEnum as ErrorStringEnum;
        else
            return status.statusMsg;
    }
}

/**
 * Returns the error (as a string or an `ErrorStringEnum`) associated with the given feature id or `undefined` if none.
 */
export function getFeatureError(context is Context, id is Id)
{
    return getFeatureStatusString(context, id, StatusType.ERROR);
}

/**
 * Returns the warning (as a string or an `ErrorStringEnum`) associated with the given feature id or `undefined` if none.
 */
export function getFeatureWarning(context is Context, id is Id)
{
    return getFeatureStatusString(context, id, StatusType.WARNING);
}

/**
 * Returns the info status (as a string or an `ErrorStringEnum`) associated with the given feature id or `undefined` if none.
 */
export function getFeatureInfo(context is Context, id is Id)
{
    return getFeatureStatusString(context, id, StatusType.INFO);
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
    return getFeatureError(context, id) != undefined;
}

/**
 * @param id
 * @returns {boolean} : `true` if the feature with the given id has an associated status different from OK.
 */
export function featureHasNonTrivialStatus(context is Context, id is Id) returns boolean
{
    var status is FeatureStatus = getFeatureStatus(context, id);
    return status != undefined && status.statusType != StatusType.OK;
}

/**
 * @return: A string identifier for marking an error on an array parameter when using the `faultyParameters`
 * argument in any of the error reporting functions in `error.fs`.
 */
export function faultyArrayParameterId(arrayParameter is string, itemIndex is number, innerParameter is string) returns string
{
    return arrayParameterId(arrayParameter, itemIndex, innerParameter);
}

/**
 * @internal
 */
export function arrayParameterId(arrayParameter is string, itemIndex is number, innerParameter is string) returns string
{
    return arrayParameter ~ "[" ~ itemIndex ~ "]." ~ innerParameter;
}

// This should be left un-exported to ensure that all error handling is implemented in this file.
/** @internal */
enum StatusType { OK, ERROR, WARNING, INFO }

/**
 * The status of a feature
 *
 * @type {{
 *      @field statusType @internalType {StatusType}
 *      @field faultyParameters {array}
 *      @field statusEnum {ErrorStringEnum}
 *      @field statusMsg {string}
 * }}
 */
export type FeatureStatus typecheck canBeFeatureStatus;

/**
 * The faultyParameters cannot exist when the statusType is StatusType.OK.
 * The statusEnum must be ErrorStringEnum.CUSTOM_ERROR if the statusMsg exists.
 */
export predicate canBeFeatureStatus(value)
{
    value is map;
    value.statusType is StatusType;
    if (value.statusType != StatusType.OK)
    {
        value.faultyParameters == undefined || value.faultyParameters is array;
        value.statusEnum is ErrorStringEnum;
    }
    if (value.statusEnum == ErrorStringEnum.CUSTOM_ERROR)
    {
        value.statusMsg is string;
    }
}

/**
 * @internal
 *
 * Construct a FeatureStatus from a map
 * @param status {{
 *              @field statusType {string}
 *              @field faultyParameters {array}
 *              @field statusEnum {ErrorStringEnum}
 *              @field statusMsg {string}
 * }}
 */
function featureStatus(status is map)
{
    return status as FeatureStatus;
}

/**
 * If the condition check fails, this function throws the error.
 * @param condition {boolean} : The condition to test.
 * @param error {ErrorStringEnum} : The error to throw if `condition` is `false`.
 */
export function verify(condition is boolean, error)
precondition
{
    error is string || error is ErrorStringEnum;
}
{
    if (!condition)
    {
        // If a raw string is provided as the error this is thrown as a raw string
        // rather than wrapping the string in a generic ErrorStringEnum
        if (error is ErrorStringEnum)
        {
            throw regenError(error);
        }
        else
        {
            throw error;
        }
    }
}

/**
 * If the condition check fails, this function throws the error.
 * @param condition {boolean} : The condition to test.
 * @param error {ErrorStringEnum} : The error to throw if `condition` is `false`.
 * @param regenErrorOptions {map} : The key-value pairs to pass to the thrown `regenError`, e.g.
 *     `entities` or `faultyParameters`.
 */
export function verify(condition is boolean, error is ErrorStringEnum, regenErrorOptions is map)
{
    if (!condition)
    {
        throw regenError(error, regenErrorOptions);
    }
}

