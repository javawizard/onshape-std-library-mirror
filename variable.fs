FeatureScript ✨; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/evaluate.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/string.fs", version : "");
import(path : "onshape/std/tool.fs", version : "");
import(path : "onshape/std/valueBounds.fs", version : "");

/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
// TODO: annotation {"Feature Type Name" : "Variable", "Feature Name Template": "Variable #name = #computedValue", "UIHint" : "NO_PREVIEW_PROVIDED"}
export const assignVariable = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Name" }
        definition.name is string;

        annotation { "Name" : "Value" }
        isAnything(definition.value);
    }
    {
        const replaceNameWithRegExpShouldBeBlank = replace(definition.name, '[a-zA-Z_][a-zA-Z_0-9]*', '');
        if (definition.name == '' || replaceNameWithRegExpShouldBeBlank != '') {
            throw regenError(ErrorStringEnum.VARIABLE_NAME_INVALID);
        }

        @setVariable(context, definition);
        setFeatureComputedParameter(context, id, { "name" : "computedValue", "value" : definition.value });
    });

/**
 * TODO: description
 * @param context
 * @param id
 */
export function makeLookupFunction(context is Context, id is Id) returns function
{
    return function(name is string)
        {
            return @getVariable(context, { "name" : name });
        };
}

