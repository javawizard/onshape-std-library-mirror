FeatureScript 581; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "581.0");
import(path : "onshape/std/feature.fs", version : "581.0");
import(path : "onshape/std/holeUtils.fs", version : "581.0");

/**
 * @internal
 */

export type HoleAttribute typecheck canBeHoleAttribute ;

/**
 * @internal
 */

 /* parameters in HoleAttribute (e.g. simple in SIMPLE, cbore in C_BORE, csink in C_SINK)
 *  are specified as maps {
 *  value : {ValueWithUnits},
 *  canBeEdited : {boolean},
 *  controllingFeatureId : {string}, : feature to be edited when editing this parameter
 *  parameterIdInFeature : {string}
 *  }
 */
export predicate canBeHoleAttribute (value)
{
    value is map;
    value.attributeId == undefined || value.attributeId is string;
    value.holeType == undefined || value.holeType is HoleStyle;
}

/**
 * @internal
 */
export const holeAttributeDefault = {} as HoleAttribute;

/**
 * @internal
 */
export function asHoleAttribute(value is map) returns HoleAttribute
{
    return value as HoleAttribute;
}

/**
* @internal
*/
export function makeHoleAttribute(attributeId is string, holeStyle is HoleStyle) returns HoleAttribute
{
    return asHoleAttribute({'holeType' : holeStyle,
            'attributeId' : attributeId });
}

/**
* @internal
*/
export function makeSimpleHoleAttribute(attributeId is string) returns HoleAttribute
{
    return asHoleAttribute({'holeType' : HoleStyle.SIMPLE,
            'attributeId' : attributeId });
}

/**
* @internal
*/
export function makeCBoreHoleAttribute(attributeId is string) returns HoleAttribute
{
    return asHoleAttribute({'holeType' : HoleStyle.C_BORE,
            'attributeId' : attributeId });
}

/**
* @internal
*/
export function makeCSinkHoleAttribute(attributeId is string) returns HoleAttribute
{
    return asHoleAttribute({'holeType' : HoleStyle.C_SINK,
            'attributeId' : attributeId });
}

/**
* @internal
*/
export function getHoleTypeAttributes(context is Context, topology is Query, holeType is HoleStyle) returns array
{
    return getAttributes(context, {
            "entities" : topology,
            "attributePattern" : asHoleAttribute({'holeType' : holeType})
    });
}

/**
* @internal
*/
export function clearHoleAttributes(context is Context, entities is Query)
{
    removeAttributes(context, {
        "entities" : entities,
        "attributePattern" : asHoleAttribute({})
    });
}


