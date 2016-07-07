FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */

export import(path : "onshape/std/smjointtype.gen.fs", version : "✨");
export import(path : "onshape/std/smobjecttype.gen.fs", version : "✨");
export import(path : "onshape/std/context.fs", version : "✨");
export import(path : "onshape/std/query.fs", version : "✨");
import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/units.fs", version : "✨");

/**
 * @internal
 */

export type SMAttribute typecheck canBeSMAttribute ;

/**
 * @internal
 */
export predicate canBeSMAttribute (value)
{
    value is map;
    value.attributeId == undefined || value.attributeId is string;
    value.objectType == undefined || value.objectType is SMObjectType;
    if (value.objectType == SMObjectType.MODEL)
    {
        value.thickness == undefined || isLength(value.thickness);
    }
    else if (value.objectType == SMObjectType.JOINT)
    {
        value.jointType == undefined || value.jointType is SMJointType;
    }

}

/**
 * @internal
 */
export const smAttributeDefault = {} as SMAttribute;

/**
 * @internal
 */
export function asSMAttribute(value is map) returns SMAttribute
{
    return value as SMAttribute;
}

/**
* @internal
*/
export function makeSMJointAttribute(attributeId is string, jointType is SMJointType) returns SMAttribute
{
    return asSMAttribute({'objectType' : SMObjectType.JOINT,
            'attributeId' : attributeId,
            'jointType' : jointType });
}

/**
* @internal
*/
export function makeSMWallAttribute(attributeId is string) returns SMAttribute
{
    return asSMAttribute({'objectType' : SMObjectType.WALL,
            'attributeId' : attributeId });
}

/**
* @internal
*/
export function makeSMModelAttribute(attributeId is string) returns SMAttribute
{
    return asSMAttribute({'objectType' : SMObjectType.MODEL,
            'attributeId' : attributeId });
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

/**
* @internal
*/
export function getSmObjectTypeAttributes(context is Context, topology is Query, objectType is SMObjectType) returns array
{
    return getAttributes(context, {
            "entities" : topology,
            "attributePattern" : asSMAttribute({'objectType' : objectType})
    });
}

/**
* @internal
*/
export function clearSmAttributes(context is Context, entities is Query)
{
    removeAttributes(context, {
        "entities" : entities,
        "attributePattern" : asSMAttribute({})
    });
}

