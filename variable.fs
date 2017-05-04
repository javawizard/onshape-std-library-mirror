FeatureScript 581; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "581.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "581.0");
import(path : "onshape/std/evaluate.fs", version : "581.0");
import(path : "onshape/std/feature.fs", version : "581.0");
import(path : "onshape/std/string.fs", version : "581.0");
import(path : "onshape/std/tool.fs", version : "581.0");
import(path : "onshape/std/valueBounds.fs", version : "581.0");

/**
 * Feature performing a `setVariable` allowing a user to assign a FeatureScript
 * value to a context variable. This variable may be retrieved within a feature by
 * calling `getVariable`, or in a Part Studio using `#` syntax (e.g. `#foo`)
 * inside any parameter which allows an expression (including the `value`
 * parameter of another variable!)
 *
 * @param definition {{
 *      @field name : Must be an identifier.
 *      @field value : Can be anything, including a length, an array, or a function.
 * }}
 */
annotation {"Feature Type Name" : "Variable", "Feature Name Template": "Variable ###name = #value", "UIHint" : "NO_PREVIEW_PROVIDED"}
export const assignVariable = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Name", "UIHint" : "UNCONFIGURABLE" }
        definition.name is string;

        annotation { "Name" : "Value" }
        isAnything(definition.value);
    }
    {
        verifyVariableName(definition.name);

        setVariable(context, definition.name, definition.value);

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V424_VARIABLE_WARNINGS))
        {
            if (definition.value == undefined)
            {
                reportFeatureWarning(context, id, ErrorStringEnum.VARIABLE_CANNOT_EVALUATE);
            }
        }
    });

/**
 * Throws an error if `name` is not a valid identifier.
 */
export function verifyVariableName(name is string)
{
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

