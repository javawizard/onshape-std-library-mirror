FeatureScript 2241; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/**
 * Attributes are data attached to individual entities, which can be set and retrieved by name in FeatureScript.
 * The data can be of any type, and multiple attributes with different names can be associated with the same
 * topological entity.
 *
 * One common use case for attributes is to set attributes on an entity in one feature, and get them
 * in another. For data not associated with entities, the same thing can be accomplished simply via [setVariable]
 * and [getVariable], but attributes allow that data to be set on specific bodies, faces, edges, or vertices.
 *
 * ```
 * setAttribute(context, {
 *    "entities" : somePart,
 *    "name" : "refPoint",
 *    "attribute" : vector(0, 0, 1) * inch
 * });
 *
 * // Later, possibly in another feature:
 * const partRefPoint = getAttribute(context, {
 *    "entity" : somePart,
 *    "name" : "refPoint"
 * });
 * if (partRefPoint != undefined)
 * {
 *     // use partRefPoint...
 * }
 * ```
 *
 * Attributes are also a useful way to mark important groups of entities for other features or other deriving Part
 * Studios. You can query for entities with a specific attribute, a specific attribute value, or a value matching a
 * given pattern with the query functions [qHasAttribute], [qHasAttributeWithValue], or [qHasAttributeWithValueMatching],
 * respectively.
 *
 * Attributes stay with the entity they are defined on, even as the Part Studio changes. An attribute on
 * a face, edge, or body which is split in two will be set with the same name and value on both split pieces.
 * An attribute on a patterned entity will be set on each patterned copy. If two or more entities are merged
 * together (e.g. with a boolean union), then the attributes on both are kept on the result, though if they have
 * attributes with the same name, the value of the primary entity (e.g. the first resolved body in the
 * boolean `tools`) will be used.
 *
 * Legacy unnamed attributes:
 * A previous use of these attribute functions involved setting unnamed attributes by calling `setAttribute`
 * without a `"name"`. This workflow is still supported, but is no longer recommended. Legacy unnamed attributes
 * can be identified and retrieved only by type, and two attributes of the same type are not allowed on the same
 * entity. The behavior of these unnamed attributes, described in "Legacy unnamed attribute" notes like this one,
 * can be safely ignored if all your attributes are set with a `"name"`.
 */
import(path : "onshape/std/context.fs", version : "2241.0");
import(path : "onshape/std/query.fs", version : "2241.0");
import(path : "onshape/std/containers.fs", version : "2241.0");

/**
 * Attach an attribute to one or several entities. Will overwrite any attribute previously set on the same entity
 * with the same name.
 *
 * @param definition {{
 *      @field entities {Query} : Entities to attach attribute to. Throws an error if the query resolves to nothing.
 *      @field name {string} : The name of the attribute @autocomplete `"myName"`
 *      @field attribute : The data to set. Can be any type. If `undefined` is provided, any existing attribute
 *          will be unset (and this entity will no longer resolve in [qHasAttribute] and similar functions)
 *          @autocomplete `"myValue"`
 *
 *          Legacy unnamed attributes:
 *          If name is not provided, adds an unnamed attribute to the entities. If more than one unnamed attribute
 *          with the same type is set on any entity, throws an error.
 * }}
 */
export function setAttribute(context is Context, definition is map)
precondition
{
    definition.entities is Query;
    definition.name is string || definition.attribute != undefined;
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
 *      @field name {string} : The name of the attribute to get. @autocomplete `"myName"`
 *      @field attributePattern : @optional
 *              Providing a map here will also filter out attributes which do not
 *              have entries precisely matching the keys and values of `attributePattern`,
 *              similar to [qHasAttributeWithValueMatching].
 *              @ex `{ "odd" : true }` matches all attributes values are maps with a
 *                      field `"odd"` whose value is `true`.
 *
 *              Legacy unnamed attributes:
 *              If `attributePattern` is provided and `name` is not, getAttributes will only
 *              return unnamed attributes with the same type as `attributePattern`, using the
 *              same behavior documented in the legacy function [qAttributeFilter].
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
 * Get the value of a single named attribute attached to a single entity, or `undefined` if no attribute
 * of that name has been set.
 *
 * @example ```
 * setAttribute(context, { "entities" : someEntities, "name" : "importantData", "attribute" : 42});
 * for (const entity in evaluateQuery(entities)) {
 *     const value = getAttribute(context, {
 *         "entity" : entity,
 *         "name" : "importantData"
 *     });
 *     println(value); // prints 42
 * }
 * ```
 * @param definition {{
 *      @field entity {Query}: Query resolving to a single entity to get the attribute from. If multiple entities are resolved,
 *          the first resolved entity is considered.
 *      @field name {string}: Name of the attribute
 * }}
 */
export function getAttribute(context is Context, definition is map)
precondition
{
    definition.entity is Query;
    annotation { "message" : "Getting a single attribute requires a name. To get unnamed attributes use `getAttributes`" }
    definition.name is string;
}
{
    return @getAttribute(context, definition);
}

/**
 * Has no effect on named attributes, instead use `setAttribute` with `"attribute" : undefined`.
 *
 * Legacy unnamed attributes:
 * Remove matching unnamed attributes attached to entities.
 *
 * @param definition {{
 *      @field entities {Query} : @optional
 *              Entities to remove unnamed attributes from. Default is everything.
 *      @field attributePattern : @optional
 *              If provided, will only remove attributes with the same type, using the same behavior
 *              documented in the legacy function [qAttributeFilter].
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



