FeatureScript 1483; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "1483.0");
export import(path : "onshape/std/errorstringenum.gen.fs", version : "1483.0");

// Imports used internally
import(path : "onshape/std/context.fs", version : "1483.0");
import(path : "onshape/std/containers.fs", version : "1483.0");
import(path : "onshape/std/string.fs", version : "1483.0");

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
export function regenError(customMessage is string)
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
export function regenError(customMessage is string, faultyParameters is array)
{
    return { "message" : ErrorStringEnum.CUSTOM_ERROR, "customMessage" : customMessage, "faultyParameters" : faultyParameters };
}
/**
 * @param entities : A query for entities to highlight in the Part Studio. Multiple queries can be
 *      combined and highlighted using the `qUnion` function. The entities are only highlighted
 *      when the feature dialog is open.
 * @param customMessage : @autocomplete `"message"`
 */
export function regenError(customMessage is string, entities is Query)
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
 * @param faultyParameters : An array of strings that correspond to keys in the feature definition
 *      map. Throwing a `regenError` with `faultyParameters` will highlight them in red inside the
 *      feature dialog.
 */
export function regenError(message is ErrorStringEnum, faultyParameters is array)
{
    return { "message" : message, "faultyParameters" : faultyParameters };
}

/**
 * @param entities : A query for entities to highlight in the Part Studio. Multiple queries can be
 *      combined and highlighted using the `qUnion` function. The entities are only highlighted
 *      when the feature dialog is open.
 */
export function regenError(message is ErrorStringEnum, entities is Query)
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
 * @param definition {{
 *      @field subfeatureId {Id} : The Id of the subfeature.
 *      @field featureParameterMap {map} : A mapping of the field names from subfeature to feature. @optional
 *      @field featureParameterMappingFunction {function} : A function to map field names from subfeature to feature. @optional
 *      @field propagateErrorDisplay {boolean} : Use subfeature error display when present.  Default is false. @optional
 * }}
 */
export function processSubfeatureStatus(context is Context, id is Id, definition is map) returns boolean
{
    var subStatus is FeatureStatus = getFeatureStatus(context, definition.subfeatureId);
    if (subStatus == undefined)
    {
        return false;
    }
    var status = subStatus;
    status.faultyParameters = undefined;
    if (subStatus.faultyParameters != undefined)
    {
        var faultyParameters = [];
        const featureParameterMap = definition.featureParameterMap;
        const mappingFunction = definition.featureParameterMappingFunction;
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
    reportFeatureStatus(context, id, status);
    if (definition.propagateErrorDisplay == true)
    {
        @transferSubfeatureErrorDisplay(context, id, {"subfeatureId" : definition.subfeatureId});
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

/** @internal */
enum StatusType {OK, ERROR, WARNING, INFO}

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

