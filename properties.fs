FeatureScript 1247; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/**
 * Properties include name, appearance, material, and part number (see [PropertyType]).  They can be set in FeatureScript, but not read.
 */
import(path : "onshape/std/context.fs", version : "1247.0");
import(path : "onshape/std/query.fs", version : "1247.0");
import(path : "onshape/std/string.fs", version : "1247.0");
import(path : "onshape/std/units.fs", version : "1247.0");

export import(path : "onshape/std/propertytype.gen.fs", version : "1247.0");

/**
 * Sets a property on a set of bodies.  The allowed properties are listed in [PropertyType].
 *
 * Note: Any properties set in this way will be overridden if they are set directly in the Part Studio
 * (via "Rename", "Set appearance", or the properties dialog).  In that case the property
 * value provided in FeatureScript will become shadowed. For example, if a part number is set in a
 * custom feature based on the configuration, manually editing the part number from the properties dialog
 * will override the custom feature's part number for all configurations.
 * @param definition {{
 *      @field entities {Query} : The bodies to apply the property to.
 *      @field propertyType {PropertyType} : The property to set.
 *          @eg `PropertyType.APPEARANCE` to change the part appearance.
 *      @field customPropertyId {string} : @requiredif {`propertyType` is `CUSTOM`}
 *          The id of the custom property.  The property id is available from your
 *          [company's custom properties page](https://cad.onshape.com/help/Content/company-properties.htm).
 *          Note that this call performs no checks as to whether the custom property value is valid, so
 *          invalid property values may be recorded.
 *      @field value : A [Color] if the `propertyType` is `APPEARANCE`, a [Material] if it is `MATERIAL`,
 *          a boolean if it is `EXCLUDE_FROM_BOM`, and a string otherwise.  The value should be a string
 *          for a `CUSTOM` property even if the property is of a non-string type.
 *          @eg `color(1, 0, 0)` to make the part red.
 * }}
 */
export function setProperty(context is Context, definition is map)
precondition
{
    definition.entities is Query; // Bodies only for now
    definition.propertyType is PropertyType;

    if (definition.propertyType == PropertyType.APPEARANCE)
        definition.value is Color;
    else if (definition.propertyType == PropertyType.MATERIAL)
        definition.value is Material;
    else if (definition.propertyType == PropertyType.EXCLUDE_FROM_BOM)
        definition.value is boolean;
    else
        definition.value is string;

    if (definition.propertyType == PropertyType.CUSTOM)
    {
        definition.customPropertyId is string;
        annotation { 'Message' : 'customPropertyId must be 24 hexadecimal digits' }
        match(definition.customPropertyId, "[0-9a-fA-F]{24}").hasMatch; // mongo id
    }
}
{
    @setProperty(context, definition);
}

/** @internal Works only in editing logic and manipulators */
export function getProperty(context is Context, definition is map)
precondition
{
    definition.entity is Query;
    definition.propertyType is PropertyType;

    if (definition.propertyType == PropertyType.CUSTOM)
    {
        definition.customPropertyId is string;
        annotation { 'Message' : 'customPropertyId must be 24 hexadecimal digits' }
        match(definition.customPropertyId, "[0-9a-fA-F]{24}").hasMatch; // mongo id
    }
}
{
    var result = @getProperty(context, definition);
    if (result != undefined)
    {
        if (definition.propertyType == PropertyType.APPEARANCE)
        {
            result = result as Color;
        }
        else if (definition.propertyType == PropertyType.MATERIAL)
        {
            result.density *= kilogram / meter ^ 3;
            result = result as Material;
        }
    }
    return result;
}

/** Represents a color as red, green, blue, and alpha transparency components, each between 0 and 1 (inclusive). */
export type Color typecheck canBeColor;

/** Typecheck for [Color] */
export predicate canBeColor(value)
{
    value is map;
    @size(value) == 4;
    value.red is number;
    value.green is number;
    value.blue is number;
    value.alpha is number;
    for (var channel in value)
    {
        channel.value >= 0;
        channel.value <= 1;
    }
}

/** Create a [Color] from RGBA values. */
export function color(red is number, green is number, blue is number, alpha is number) returns Color
{
    return { "red" : red, "green" : green, "blue" : blue, "alpha" : alpha } as Color;
}

/** Create an opaque [Color] from RGB values. */
export function color(red is number, green is number, blue is number) returns Color
{
    return color(red, green, blue, 1);
}

/** Represents a material. */
export type Material typecheck canBeMaterial;

/** Typecheck for [Material] */
export predicate canBeMaterial(value)
{
    value is map;
    value.name is string;
    value.density is ValueWithUnits;
    value.density.unit == DENSITY_UNITS;
}

/**
 * Constructs a material with a name and a density.
 * @param name : The displayed name of the material. @autocomplete `"My material"`
 * @param density : @eg `19.3 * gram / centimeter ^ 3`
 */
export function material(name is string, density is ValueWithUnits)
precondition density.unit == DENSITY_UNITS;
{
    return { "name" : name, "density" : density } as Material;
}

