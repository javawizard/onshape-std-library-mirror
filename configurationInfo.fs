FeatureScript 3112; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/units.fs", version : "3112.0");

/**
 * @internal
 *
 * A map from parameter id to the `TabLibraryInfo` of the library that parameter references. Passed to editing logic
 * functions that declare a final `ParameterLibraryInfo` argument.
 */
export type ParameterLibraryInfo typecheck canBeParameterLibraryInfo;

predicate canBeParameterLibraryInfo(value)
{
    value is map;
    for (var entry in value)
    {
        entry.key is string;
        entry.value is TabLibraryInfo;
    }
}

/**
 * @internal
 *
 * Describes every tab of one parameter library, along with which tab the feature currently references.
 */
export type TabLibraryInfo typecheck canBeTabLibraryInfo;

predicate canBeTabLibraryInfo(value)
{
    value is map;
    value.selectedElementId is string;
    value.selectedConfiguration is map;
    value.tabs is array;
    for (var tab in value.tabs)
        tab is TabInfo;
}

/**
 * @internal
 */
export type TabInfo typecheck canBeTabInfo;

predicate canBeTabInfo(value)
{
    value is map;
    value.elementId is string;
    value.name is string;
    value.configuration is ConfigurationInfo;
}

/**
 * @internal
 */
export type ConfigurationInfo typecheck canBeConfigurationInfo;

predicate canBeConfigurationInfo(value)
{
    value is map;
    value.inputs is array;
    for (var input in value.inputs)
        input is ConfigurationInputInfo;
}

/**
 * @internal
 */
export enum ConfigurationInputType
{
    ENUM,
    BOOLEAN,
    LENGTH,
    ANGLE,
    INTEGER,
    REAL,
    STRING
}

/**
 * @internal
 */
export type ConfigurationInputInfo typecheck canBeConfigurationInputInfo;

predicate canBeConfigurationInputInfo(value)
{
    value is map;
    value.id is string;
    value.name is string;
    value.excludeFromProperties is boolean;
    value.inputType is ConfigurationInputType;
    switch (value.inputType)
    {
        ConfigurationInputType.ENUM : canBeEnumConfigurationInputInfo(value),
        ConfigurationInputType.BOOLEAN : canBeBooleanConfigurationInputInfo(value),
        ConfigurationInputType.LENGTH : canBeLengthConfigurationInputInfo(value),
        ConfigurationInputType.ANGLE : canBeAngleConfigurationInputInfo(value),
        ConfigurationInputType.INTEGER : canBeNumberConfigurationInputInfo(value),
        ConfigurationInputType.REAL : canBeNumberConfigurationInputInfo(value),
        ConfigurationInputType.STRING : canBeStringConfigurationInputInfo(value)
    };
}

predicate canBeEnumConfigurationInputInfo(value)
{
    value.defaultValue is string; // an id of one of the options
    for (var option in value.options)
        option is ConfigurationEnumOptionInfo;
}

predicate canBeBooleanConfigurationInputInfo(value)
{
    value.defaultValue is boolean;
}

predicate canBeStringConfigurationInputInfo(value)
{
    value.defaultValue is string;
}

predicate canBeNumberConfigurationInputInfo(value)
{
    hasBounds(value);
}

predicate canBeLengthConfigurationInputInfo(value)
{
    hasBounds(value);
    isLength(value.units);
}

predicate canBeAngleConfigurationInputInfo(value)
{
    hasBounds(value);
    isAngle(value.units);
}

predicate hasBounds(value is map)
{
    value.defaultValue is number;
    value.minValue is number;
    value.maxValue is number;
}

/**
 * @internal
 */
export type ConfigurationEnumOptionInfo typecheck canBeConfigurationEnumOptionInfo;

predicate canBeConfigurationEnumOptionInfo(value)
{
    value is map;
    value.id is string;
    value.name is string;
    // May also have an associatedValue
}

