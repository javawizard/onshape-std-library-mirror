FeatureScript 1389; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "1389.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "1389.0");
import(path : "onshape/std/evaluate.fs", version : "1389.0");
import(path : "onshape/std/feature.fs", version : "1389.0");
import(path : "onshape/std/string.fs", version : "1389.0");
import(path : "onshape/std/tool.fs", version : "1389.0");
import(path : "onshape/std/valueBounds.fs", version : "1389.0");
import(path : "onshape/std/manipulator.fs", version : "1389.0");

/**
 * Specifies the type of values `assignVariable` is allowed to set.
 *
 * @value ANY : The variable can be any immutable FeatureScript value; boxes and builtins are not allowed.
 */
export enum VariableType
{
    annotation { "Name" : "Length" }
    LENGTH,
    annotation { "Name" : "Angle" }
    ANGLE,
    annotation { "Name" : "Number" }
    NUMBER,
    annotation { "Name" : "Any" }
    ANY
}

/**
 * Feature performing a `setVariable` allowing a user to assign a FeatureScript
 * value to a context variable. This variable may be retrieved within a feature by
 * calling `getVariable`, or in a Part Studio using `#` syntax (e.g. `#foo`)
 * inside any parameter which allows an expression (including the `value`
 * parameter of another variable!)
 *
 * @param definition {{
 *      @field variableType {VariableType} : The type of variable.  If it is not ANY, the value is restricted
 *          to be a length, angle, or number and is passed through the `lengthValue`, `angleValue`, or `numberValue`
 *          field, respectively.
 *      @field name {string} : Must be an identifier.
 *      @field lengthValue {ValueWithUnits} : Used if `variableType` is `LENGTH`.
 *      @field angleValue {ValueWithUnits} : Used if `variableType` is `ANGLE`.
 *      @field numberValue {number} : Used if `variableType` is `NUMBER`.
 *      @field anyValue : Used if `variableType` is `ANY`.  Can be any immutable FeatureScript value, including a length, an array, or a function.
 * }}
 */
annotation {"Feature Type Name" : "Variable", "Feature Name Template": "###name = #value", "UIHint" : UIHint.NO_PREVIEW_PROVIDED, "Editing Logic Function" : "variableEditLogic"}
export const assignVariable = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Variable type", "UIHint" : ["HORIZONTAL_ENUM", "UNCONFIGURABLE"] }
        definition.variableType is VariableType;

        annotation { "Name" : "Name", "UIHint" : [UIHint.UNCONFIGURABLE, UIHint.VARIABLE_NAME], "MaxLength": 10000 }
        definition.name is string;

        if (definition.variableType == VariableType.LENGTH)
        {
            annotation { "Name" : "Value", "UIHint" : UIHint.INITIAL_FOCUS_ON_EDIT }
            isLength(definition.lengthValue, ZERO_DEFAULT_LENGTH_BOUNDS);
        }
        if (definition.variableType == VariableType.ANGLE)
        {
            annotation { "Name" : "Value", "UIHint" : UIHint.INITIAL_FOCUS_ON_EDIT }
            isAngle(definition.angleValue, ANGLE_360_ZERO_DEFAULT_BOUNDS);
        }
        if (definition.variableType == VariableType.NUMBER)
        {
            annotation { "Name" : "Value", "UIHint" : UIHint.INITIAL_FOCUS_ON_EDIT }
            isReal(definition.numberValue, { (unitless) : [-1e12, 0, 1e12] } as RealBoundSpec);
        }
        if (definition.variableType == VariableType.ANY)
        {
            annotation { "Name" : "Value", "UIHint" : UIHint.INITIAL_FOCUS_ON_EDIT }
            isAnything(definition.anyValue);
        }

        annotation { "UIHint" : UIHint.ALWAYS_HIDDEN }
        isAnything(definition.value);
    }
    {
        verifyVariableName(definition.name);

        if (definition.variableType == VariableType.ANGLE &&
                !isAtVersionOrLater(context, FeatureScriptVersionNumber.V694_FILL_GUIDE_CURVES_FS))
        {
            definition.angleValue = adjustAngle(context, definition.angleValue);
        }

        var value;

        if (definition.variableType == VariableType.LENGTH)
            value = definition.lengthValue;
        else if (definition.variableType == VariableType.ANGLE)
            value = definition.angleValue;
        else if (definition.variableType == VariableType.NUMBER)
            value = definition.numberValue;
        else if (definition.variableType == VariableType.ANY)
            value = definition.anyValue;

        setFeatureComputedParameter(context, id, { "name" : "value", "value" : value });

        setVariable(context, definition.name, value);

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V424_VARIABLE_WARNINGS))
        {
            if (value == undefined)
            {
                reportFeatureWarning(context, id, ErrorStringEnum.VARIABLE_CANNOT_EVALUATE);
            }
        }
    }, { variableType : VariableType.ANY });

/**
 * Throws an error if `name` is not a valid identifier.
 */
export function verifyVariableName(name is string)
{
    if (length(name) > 10000)
        throw regenError(ErrorStringEnum.VARIABLE_NAME_TOO_LONG);
    const replaceNameWithRegExpShouldBeBlank = replace(name, '[a-zA-Z_][a-zA-Z_0-9]*', '');
    if (name == '' || replaceNameWithRegExpShouldBeBlank != '')
        throw regenError(ErrorStringEnum.VARIABLE_NAME_INVALID);
}

/**
 * Make a function to look up variables from the given context.  Used in generated part studio code.
 */
export function makeLookupFunction(context is Context, id is Id) returns function
{
    return function(name is string)
        {
            return getVariable(context, name);
        };
}

/**
 * @internal
 * Editing logic function for populating the displayValue parameter
 */
export function variableEditLogic(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean) returns map
{
    if (definition.variableType == VariableType.LENGTH)
        definition.value = copyParameter("lengthValue");
    else if (definition.variableType == VariableType.ANGLE)
        definition.value = copyParameter("angleValue");
    else if (definition.variableType == VariableType.NUMBER)
        definition.value = copyParameter("numberValue");
    else
        definition.value = copyParameter("anyValue");
    return definition;
}


