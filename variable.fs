FeatureScript ✨; /* Automatically generated version */
export import(path : "onshape/std/evaluate.fs", version : "");

export enum AssignmentType
{
    annotation { "Name" : "Value" }
    VALUE,
    annotation { "Name" : "Measurement" }
    MEASUREMENT
}


annotation {"Feature Type Name" : "Assign variable", "Feature Name Template": "Assign #name", "UIHint" : "NO_PREVIEW_PROVIDED"}
export const assignVariable = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Name" }
        definition.name is string;

        annotation { "Name" : "Type" }
        definition.assignmentType is AssignmentType;

        if (definition.assignmentType == AssignmentType.VALUE)
        {
            annotation { "Name" : "Value" }
            isLength(definition.value, ZERO_DEFAULT_LENGTH_BOUNDS);
        }

        if (definition.assignmentType == AssignmentType.MEASUREMENT)
        {
            annotation { "Name" : "Entities", "Filter" : EntityType.VERTEX }
            definition.entities is Query;
        }
    }
    {
        var replaceNameWithRegExpShouldBeBlank = replace(definition.name, '[a-zA-Z_][a-zA-Z_0-9]*', '');
        if (definition.name == '' || replaceNameWithRegExpShouldBeBlank != '') {
            throw regenError(ErrorStringEnum.VARIABLE_NAME_INVALID);
        }

        if (definition.assignmentType == AssignmentType.MEASUREMENT)
        {
            var v0 = evVertexPoint(context, { "vertex" : qNthElement(definition.entities, 0) });
            var v1 = evVertexPoint(context, { "vertex" : qNthElement(definition.entities, 1) });
            var v2 = try(evVertexPoint(context, { "vertex" : qNthElement(definition.entities, 2) }));

            if (v2 == undefined)
                definition.value = norm(v0 - v1);
            else
                definition.value = angleBetween(v0 - v1, v2 - v1);
        }

        @setVariable(context, definition);
    });

export function makeLookupFunction(context is Context, id is Id) returns function
{
    return function(name is string)
        {
            return @getVariable(context, { "name" : name });
        };
}

