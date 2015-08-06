FeatureScript 189; /* Automatically generated version */
export import(path : "onshape/std/evaluate.fs", version : "");

export enum AssignmentType
{
    annotation { "Name" : "Value" }
    VALUE,
    annotation { "Name" : "Measurement" }
    MEASUREMENT
}


//TODO: annotation {"Feature Type Name" : "Assign Variable", "UIHint" : "NO_PREVIEW_PROVIDED"}
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
            reportFeatureError(context, id, ErrorStringEnum.VARIABLE_NAME_INVALID);
        }

        if (definition.assignmentType == AssignmentType.MEASUREMENT)
        {
            var v0 = evVertexPoint(context, { "vertex" : qNthElement(definition.entities, 0) }).result;
            var v1 = evVertexPoint(context, { "vertex" : qNthElement(definition.entities, 1) }).result;
            var v2 = evVertexPoint(context, { "vertex" : qNthElement(definition.entities, 2) }).result;

            if (v0 == undefined || v1 == undefined)
            {
                reportFeatureError(context, id, ErrorStringEnum.CANNOT_EVALUATE_VERTEX);
                return;
            }

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
            return @getVariable(context, { "name" : name }).result;
        };
}

