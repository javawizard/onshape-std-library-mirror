FeatureScript 225; /* Automatically generated version */
export import(path : "onshape/std/featurescriptversionnumber.gen.fs", version : "");
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/string.fs", version : "");

/**
 * TODO: description
 */
export function libraryLanguageVersion()
{
    return @getLanguageVersion();
}

//====================== Context ========================

/**
 * A context is a builtin that stores modeling data, including bodies (solid, sheet, wire, and point), their
 * constituent topological entities (faces, edges, and vertices), variables, feature error states, etc.
 * All features, operations, and evaluation functions require a context to operate on. Different contexts do not
 * interact, but data may be transferred from one to another using `opMergeContexts`.
 *
 * Each context keeps track of the version at which it was created. While regenerating a feature that has been
 * "held back" to an older version, the version reported by the context will be the older version, causing subfeatures
 * and operations to emulate old behavior.
 */
export type Context typecheck canBeContext;

export predicate canBeContext(value)
{
    value is builtin;
}

/**
 * TODO: description
 */
export function newContext() returns Context
{
    return @newContext(FeatureScriptVersionNumberCurrent) as Context;
}

/* Return false if the active feature is running at a version number
   at least as new as [introduced] that changed behavior. */
/**
 * TODO: description
 * @param context
 * @param introduced
 */
export function isAtVersionOrLater(context is Context, introduced is FeatureScriptVersionNumber) returns boolean
{
    return @isAtVersionOrLater(context, introduced);
}

//====================== Id ========================

/**
 * An Id identifies a feature in a context. Each feature, subfeature, and operation must have a unique id. Ids are
 * used in queries, error reporting, and accessing data associated with features. Ids are hierarchical: each subfeature
 * must have an id that is the id of its parent feature plus optionally its own subId. The root id is constructed by
 * `newId()` and subIds are added with the overloadeded addition operator: `id + "foo"` represents the id `id` with the
 * extra level "foo".
 */
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

/**
 * TODO: description
 */
export function newId() returns Id
{
    return [] as Id;
}

/**
 * TODO: description
 * @param idComp
 */
export function makeId(idComp is string) returns Id
{
    return [idComp] as Id;
}

export predicate isTopLevelId(id is Id)
{
    size(id) == 1;
}

export const ANY_ID = '*';

/**
 * TODO: description
 * @param addend
 */
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


