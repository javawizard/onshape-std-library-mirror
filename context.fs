FeatureScript 1691; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

export import(path : "onshape/std/featurescriptversionnumber.gen.fs", version : "1691.0");
import(path : "onshape/std/containers.fs", version : "1691.0");
import(path : "onshape/std/string.fs", version : "1691.0");

//====================== Context ========================

/**
 * A `Context` is a `builtin` that stores modeling data, including bodies
 * (solids, sheets, wires, and points), their constituent topological entities
 * (faces, edges, and vertices), variables, feature error states, etc.
 *
 * Every Onshape Part Studio uses a single [Context]. All features, operations,
 * and evaluation functions require a context to operate on. Different contexts
 * do not interact, but data may be transferred from one to another using
 * [opMergeContexts].
 *
 * Each context keeps track of the version of the Onshape Standard Library at
 * which it was created. While regenerating a feature that has been "held back"
 * to an older version, the version reported by the context will be the older
 * version, causing subfeatures and operations to emulate old behavior.
 */
export type Context typecheck canBeContext;

/** Typecheck for [Context] */
export predicate canBeContext(value)
{
    @isContext(value); /* implies (value is builtin) */
}

/**
 * @internal
 * Returns a new, empty [Context].
 */
export function newContext() returns Context
{
    return @newContext(FeatureScriptVersionNumberCurrent) as Context;
}

/**
 * @internal
 * Returns `true` if the active feature of `context` is running at a version
 * number at least as new as `introduced`.
 */
export function isAtVersionOrLater(context is Context, introduced is FeatureScriptVersionNumber) returns boolean
{
    return @isAtVersionOrLater(context, introduced);
}

export function isAtVersionOrLater(versionToCheck is FeatureScriptVersionNumber, versionToCompareAgainst is FeatureScriptVersionNumber) returns boolean
{
    // Enum sort order within maps is based on the ordinal
    const mapOfVersions = { (versionToCheck) : true, (versionToCompareAgainst) : true };
    var firstKey;
    for (var key, _ in mapOfVersions)
    {
        firstKey = key;
        return firstKey == versionToCompareAgainst;
    }
}

/**
 * @internal
 * Returns version at which the active feature of `context` is running
 */
export function getCurrentVersion(context is Context) returns FeatureScriptVersionNumber
{
    return @getCurrentVersion(context) as FeatureScriptVersionNumber;
}

/**
 * @internal
 * Returns `true` if the active feature of `context` is a sheet metal feature, or is a subfeature of a sheet metal feature
 */
export function isInSheetMetalFeature(context is Context)
{
    return @isInSheetMetalFeature(context);
}


//====================== Id ========================

/**
 * An Id identifies a feature or operation in a context. Each feature,
 * subfeature, and operation must have a unique id. Ids are used in queries,
 * error reporting, and accessing data associated with features.
 *
 * Ids are hierarchical. That is, each operation's id must have a parent id.
 * The root id is constructed with `newId()` and subIds are added with the
 * overloaded `+` operator.
 *
 * @example `id + "foo"` represents an id named `"foo"` whose parent is `id`
 * @example `id + "foo" + "bar"` represents an id named `"bar"` whose parent
 *          equals `id + "foo"`
 *
 * Internally, an `Id` is just an array whose elements are strings,
 * representing the full path of the `Id`.
 * @example `newId() + "foo" + "bar"` is equivalent to `["foo", "bar"] as Id`,
 *          though the expressions like the latter are not recommended in
 *          practice.
 *
 * Within a feature, all operations' ids should be children of the feature's
 * `Id` (which is always passed into the feature function as the variable
 * `id`).
 *
 * Subfeatures should use a similar pattern. For instance, in the snippet
 * below, `mySubfeature` is a minimal example following good practices
 * for breaking out a set of operations into a subroutine.
 * ```
 * annotation { "Feature Type Name" : "My Feature" }
 * export const myFeature = defineFeature(function(context is Context, id is Id, definition is map)
 *     precondition {}
 *     {
 *         fCuboid(context, id + "startingCube", {
 *                 "corner1" : vector(0, 0, 0) * inch,
 *                 "corner2" : vector(1, 1, 1) * inch
 *         });
 *
 *         mySubfeature(context, id + "subFeature", qCreatedBy(id + "startingCube", EntityType.EDGE));
 *
 *         fCuboid(context, id + "endingCube", {
 *                 "corner1" : vector(0, 0, 0) * inch,
 *                 "corner2" : vector(-1, -1, -1) * inch
 *         });
 *     }, {});
 *
 * function mySubfeature(context is Context, id is Id, entities is Query)
 * {
 *     opChamfer(context, id + "chamfer", {
 *             "entities" : entities,
 *             "chamferType" : ChamferType.EQUAL_OFFSETS,
 *             "width" : 0.1 * inch
 *     });
 *     opFillet(context, id + "fillet1", {
 *         "entities" : qCreatedBy(id + "chamfer", EntityType.EDGE),
 *         "radius" : 0.05 * inch
 *     });
 * }
 * ```
 *
 * The full id hierarchy must reflect creation history. That is, each `Id`
 * (including parents) must refer to a contiguous region of operations on the
 * context.
 *
 * Thus, the following code will fail because `id + "extrude"` alone refers to
 * two non-contiguous regions of history:
 * ```
 * for (var i in [1, 2])
 * {
 *     opExtrude(context, id + "extrude" + i, {...}); // Fails on second iteration.
 *     opChamfer(context, id + "chamfer" + i, {...});
 * }
 * ```
 *
 * For the above code, a pattern like `id + i + "extrude"` or
 * `id + ("loop" ~ i) + "extrude"` would work as expected, as would the
 * unnested `id + ("extrude" ~ i)`.
 *
 * Only the following characters are allowed in a string that makes up an `Id`: `a-z`,
 * `A-Z`, `0-9`, `_`, `+`, `-`, `/`.  An asterisk `*` is allowed at the beginning of
 * the string to mark it an "unstable" component (see below).
 */
export type Id typecheck canBeId;

/** Typecheck for [Id] */
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
 * Returns an empty id.
 */
export function newId() returns Id
{
    return [] as Id;
}

/**
 * Returns an id specified by the given string.
 */
export function makeId(idComp is string) returns Id
{
    return [idComp] as Id;
}

/**
 * True if the `Id` represents a top-level feature or default geometry (i.e.
 * if the `Id` has length `1`)
 */
export predicate isTopLevelId(id is Id)
{
    size(id) == 1;
}

/**
 * The string literal `"*"`, which matches any id inside certain queries.
 * @ex `qCreatedBy(id + ANY_ID + "fillet")`
 */
export const ANY_ID = '*';

/**
 * Marks a given id component as "unstable" causing queries to treat it as a
 * wildcard. This is useful for when the id component is not expected to be
 * robust, such as an index into the results of an evaluated query.
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

//====================== Variable builtins ========================

/**
 * Attach a variable to the context, which can be retrieved by another feature
 * defined later. If a variable of the same name already exists, this function
 * will overwrite it.
 *
 * @example `setVariable(context, "foo", 1)` attaches a variable named `"foo"`,
 *      with value set to `1`, on the context.
 *
 * @param value : Can be any value, including an array or map with many elements.
 */
export function setVariable(context is Context, name is string, value)
{
    @setVariable(context, { "name" : name, "value" : value });
}

/**
 * Retrieve a variable attached to the context by name.
 * Throws an exception if variable by the given name is not found.
 *
 * @example `getVariable(context, "foo")` returns the value assigned to a
 *      previously-set variable named `"foo"`.
 *
 * Variables on a context can also be accessed within a Part Studio using
 * `#` syntax (e.g. `#foo`) inside any parameter which allows an expression.
 */
export function getVariable(context is Context, name is string)
{
    return @getVariable(context, { "name" : name });
}

/**
 * @internal
 * Retrieves all variables (including configuration variables) attached to the
 * context as a map from the variable name to the variable value.
 */
export function getAllVariables(context is Context) returns map
{
    return @getAllVariables(context);
}

/**
 * Returns the language version of the library.  Note: this function calls `@getLanguageVersion` internally,
 * but if you call `@getLanguageVersion` directly, you may get a different result.  That is because
 * `@getLanguageVersion` returns the language version of the module making the call (which, for a module in std
 * will coincide with the version of std.)
 */
export function libraryLanguageVersion()
{
    return @getLanguageVersion();
}

