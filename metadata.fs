FeatureScript 1660; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/math.fs", version : "1660.0");

/**
 * @internal
 * Specifies the type of values allowed on a metadata property
 *
 * @value DATE : Represented as a string in ISO format
 * @value ENUM : Represented as a string matching the `value` from a list of possible `enumValues`
 * @value OBJECT : Represented as a map
 * @value USER : Represented as a list of user ids
 * @value CATEGORY : Represented as a list of metadata category objects
 * @value COMPUTED : For onshape internal use
 */
export enum MetadataValueType {
    annotation { "Name" : "String" }
    STRING,
    annotation { "Name" : "Boolean" }
    BOOL,
    annotation { "Name" : "Integer" }
    INT,
    annotation { "Name" : "Double" }
    DOUBLE,
    annotation { "Name" : "Date" }
    DATE,
    annotation { "Name" : "Enum" }
    ENUM,
    annotation { "Name" : "Object" }
    OBJECT,
    annotation { "Name" : "User" }
    USER,
    annotation { "Name" : "Category" }
    CATEGORY,
    annotation { "Name" : "Computed" }
    COMPUTED,
    annotation { "Name" : "Value with units" }
    VALUE_WITH_UNITS
}

/**
 * @internal
 * Determines whether a metadata value matches a certain metadata value type
 */
export predicate matchesMetadataValueType(value, valueType is MetadataValueType)
{
    if (value != undefined)
    {
        if (valueType == MetadataValueType.STRING)
        {
            value is string;
        }
        else if (valueType == MetadataValueType.BOOL)
        {
            value is boolean;
        }
        else if (valueType == MetadataValueType.INT)
        {
            value is number && isInteger(value);
        }
        else if (valueType == MetadataValueType.DOUBLE)
        {
            value is number;
        }
        else if (valueType == MetadataValueType.DATE)
        {
            value is string;    // Dates come as an ISO date string
        }
        else if (valueType == MetadataValueType.ENUM)
        {
            value is string;
        }
        else if (valueType == MetadataValueType.OBJECT)
        {
            // Builtin properties come as objects, but custom OBJECT properties today come as JSON strings
            value is map || value is string;
        }
        else if (valueType == MetadataValueType.USER)
        {
            value is array;
            for (var elem in value)
            {
                elem is map;
                elem.id is string;
            }
        }
        else if (valueType == MetadataValueType.CATEGORY)
        {
            value is array;
            for (var category in value)
            {
                category is map;
            }
        }
        else if (valueType == MetadataValueType.COMPUTED)
        {
            value is map;
        }
        else if (valueType == MetadataValueType.VALUE_WITH_UNITS)
        {
            value is string;    // Value with units comes in as a string like "42 inch"
        }
    }
}

/**
 * @internal
 * A metadata property for a part or tab.
 * @type {{
 *      @field propertyId {string} : Custom property id
 *      @field name {string} : Custom property name
 *      @field valueType {MetadataValueType} : Type of the property value
 *      @field value : Property value
 *      @field defaultValue : Property default value
 *      @field enumValues : If valueType is ENUM, a list of valid enum values
 * }}
 */
export type MetadataProperty typecheck canBeMetadataProperty;

/** @internal Typecheck for [MetadataProperty] */
export predicate canBeMetadataProperty(value)
{
    value is map;
    value.valueType is MetadataValueType;

    matchesMetadataValueType(value.defaultValue, value.valueType);
    matchesMetadataValueType(value.value, value.valueType);

    if (value.valueType == MetadataValueType.ENUM)
    {
        isMetadataEnumValueList(value.enumValues);
        isValidMetadataEnumValue(value.value, value.enumValues);
        isValidMetadataEnumValue(value.defaultValue, value.enumValues);
    }
    else
    {
        value.enumValues is undefined;
    }

    value.name is string;
    value.propertyId is string;
}

predicate isMetadataEnumValueList(value)
{
    value is array;
    for (var enumValue in value)
    {
        enumValue is map;
        enumValue.label is undefined || enumValue.label is string;
        enumValue.value is string;
    }

    areEnumValuesUnique(value);
}

function areEnumValuesUnique(enumValues is array) returns boolean
precondition
{
    for (var enumValue in enumValues)
    {
        enumValue is map;
        enumValue.label is undefined || enumValue.label is string;
        enumValue.value is string;
    }
}
{
    // Track which values are present and fail if we find a duplicate
    var valuesPresent = {};
    for (var enumValue in enumValues)
    {
        if (valuesPresent[enumValue.value] == true)
        {
            return false;
        }

        valuesPresent[enumValue.value] = true;
    }

    return true;
}

function isValidMetadataEnumValue(value, enumValues is array) returns boolean
precondition
{
    value is undefined || value is string;
    isMetadataEnumValueList(enumValues);
}
{
    if (value == undefined)
    {
        return true;
    }

    for (var enumValue in enumValues)
    {
        if (value == enumValue.value)
        {
            return true;
        }
    }

    return false;
}

