FeatureScript 920; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/**
 * Attributes are data attached to entities, which can be set and retrieved in
 * FeatureScript. The data can be of any type (except undefined), and multiple
 * attributes can be associated with the same topological entity.
 *
 * The common use case for attributes is to set attributes on an entity in one
 * feature, and get them in another. For global data, this can be done more
 * simply via [setVariable] and [getVariable].
 *
 * Entities can be queried by attributes with [qAttributeFilter] and
 * [qAttributeQuery].
 */
import(path : "onshape/std/context.fs", version : "920.0");
import(path : "onshape/std/query.fs", version : "920.0");
import(path : "onshape/std/containers.fs", version : "920.0");

/**
 * Attach an attribute to one or several entities.
 *
 * @param definition {{
 *      @field entities {Query} : Entities to attach attribute to. Throws an
 *              error if the query resolves to nothing.
 *      @field attribute : The data to attach. @eg `"myAttribute"`
 * }}
 */
export function setAttribute(context is Context, definition is map)
precondition
{
    definition.entities is Query;
    definition.attribute != undefined;
}
{
    @setAttribute(context, definition);
}

/**
 * Get attributes attached to entities.
 *
 * @param definition {{
 *      @field entities {Query} : Entities to get attributes on.
 *              If query resolves to nothing, empty array is returned
 *              @eg `qEverything()`
 *      @field attributePattern : @optional
 *              If provided, will only return attributes of this type. If a map
 *              is provided, will also only match attributes whose fields match
 *              every field of the map provided.
 *
 *              @ex `""` matches all `string` attributes.
 *              @ex `{}` matches all `map` attributes.
 *              @ex `"" as MyStringAttributeType` matches all attributes of
 *                      type `MyStringAttributeType`.
 *              @ex `{ "odd" : true }` matches all `map` attributes that have a
 *                      field `"odd"` whose value is `true`.
 * }}
 *
 * @return {array} : An array of all unique attributes on the given entities
 *          matching the pattern.
 */
export function getAttributes(context is Context, definition is map) returns array
precondition
{
    definition.entities is Query;
}
{
    return @getAttributes(context, definition);
}

/**
 * Remove matching attributes attached to entities
 *
 * @param definition {{
 *      @field entities {Query} : @optional
 *              Entities to remove attributes from. Default is everything.
 *      @field attributePattern : @optional
 *              If provided, will only remove attributes of this type. See
 *              `getAttributes` for details.
 * }}
 */
export function removeAttributes(context is Context, definition is map)
{
    @removeAttributes(context, definition);
}

/**
* @internal
*/
export function toAttributeId(id is Id) returns string
{
    var out = "";
    for (var i = 0; i < size(id); i += 1)
    {
        if (i > 0)
        {
            out = out ~ ".";
        }
        out = out ~ id[i];
    }
    return out;
}



