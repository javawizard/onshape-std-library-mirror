FeatureScript ✨; /* Automatically generated version */
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/errorstringenum.gen.fs", version : "");

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

export function reportFeatureWarning(context is Context, id is Id, message is ErrorStringEnum) returns boolean
{
    @reportFeatureWarning(context, id, { "message" : message });
    return true;
}

export function reportFeatureInfo(context is Context, id is Id, message is ErrorStringEnum) returns boolean
{
    @reportFeatureInfo(context, id, { "message" : message });
    return true;
}

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

export function getFeatureError(context is Context, id is Id)
{
    var result = @getFeatureError(context, id);
    return result == undefined ? undefined : result as ErrorStringEnum;
}

export function getFeatureWarning(context is Context, id is Id)
{
    var result = @getFeatureWarning(context, id);
    return result == undefined ? undefined : result as ErrorStringEnum;
}

export function getFeatureInfo(context is Context, id is Id)
{
    var result = @getFeatureInfo(context, id);
    return result == undefined ? undefined : result as ErrorStringEnum;
}

export function setErrorEntities(context is Context, id is Id, definition is map)
{
    @setErrorEntities(context, id, definition);
}

export function featureHasError(context is Context, id is Id) returns boolean
{
    return @getFeatureError(context, id) != undefined;
}

