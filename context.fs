FeatureScript ✨; /* Automatically generated version */
export import(path : "onshape/std/featurescriptversionnumber.gen.fs", version : "");
export import(path : "onshape/std/print.fs", version : "");
export import(path : "onshape/std/utils.fs", version : "");

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

/* Return false if the active feature is running at a version number
   at least as new as [introduced] that changed behavior. */
export function isAtVersionOrLater(context is Context, introduced is FeatureScriptVersionNumber) returns boolean
{
    return @isAtVersionOrLater(context, introduced);
}

//====================== Id ========================

export type Id typecheck canBeId;

export predicate canBeId(value)
{
    value is array;
    for (var comp in value)
    {
        comp is string;
        replace(comp, "\\*?[a-zA-Z0-9_.+/\\-]", "") == ""; //All characters should be of this form
    }
}

export function newId() returns Id
{
    return [] as Id;
}

export function makeId(idComp is string) returns Id
{
    return [idComp] as Id;
}

export predicate isTopLevelId(id is Id)
{
    size(id) == 1;
}

export const ANY_ID = '*';

export function unstableIdComponent(addend) returns string
{
    return (ANY_ID ~ addend);
}

export operator+(id is Id, addend is string) returns Id
precondition
{
        replace(addend, "^\\.", "_") == addend;
}
{
    return append(id, addend) as Id;
}

export operator+(id is Id, addend is number) returns Id
{
    return id + replace("" ~ addend, "\\.", "_");
}

export operator+(id is Id, addend is Id) returns Id
{
    return concatenateArrays([id, addend]) as Id;
}

