FeatureScript 328; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/**
* Attribute functionality allows us to associate any value with topology. The value can be of any type except undefined.
* Multiple attributes can be associated with the same topological entity. Entities can be queried by attributes.
* The attributePattern field accepted by some of the methods here supports partial match.
*  @eg `attributePattern : "anyString"` would match any string attribute that does not have a custom type,
*  @eg `attributePattern : "anyString" as StringAttributeType` would match all attributes of StringAttributeType
*  @eg `attributePattern : {"odd" : true} as MapAttributeType` would match all attributes of MapAttributeType that have a field "odd" with value true
*
*/

import(path : "onshape/std/context.fs", version : "");
import(path : "onshape/std/query.fs", version : "");

/**
 * Associate an attribute with entities
 * @param definition {{
 *      @field entities {Query} : entities to assign attribute to
 *      @field attribute
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
 * Get attributes assigned to entities
 * @param definition {{
 *      @field entities {Query} : entities to query attributes on
 *      @field attributePattern : pattern to match. Here and below matching means matching of type.
 *      If attribute is a map, then all fields specified in attributePattern will be matched. @optional
 * }}
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
 * Remove matching attributes from entities
 * @param definition {{
 *      @field entities {Query} : entities to remove attributes from. If query is not specified all matching attributes are removed. @optional
 *      @field attributePattern : pattern to match @optional
 * }}
 */
export function removeAttributes(context is Context, definition is map)
{
    @removeAttributes(context, definition);
}

