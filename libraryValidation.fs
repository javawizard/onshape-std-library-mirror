FeatureScript 2473; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

export import(path : "onshape/std/context.fs", version : "2473.0");
export import(path : "onshape/std/templatestring.fs", version : "2473.0");

/**
 * A container for a list of distinct problems found when validating a part studio for use in a library
 */
export type LibraryValidationProblems typecheck canBeLibraryValidationProblems;

predicate canBeLibraryValidationProblems(value)
{
    value is array;
    for (var problem in value)
        problem is string || problem is TemplateString;
}

/**
   Library validation is performed by a function that takes a Context
   and returns a LibraryValidationProblems. Here is an example, that checks
   whether the context contains two solid bodies and one surface

export function validate(context is Context) returns LibraryValidationProblems
{
    var allProblems = [];
    const partCount = size(evaluateQuery(context, qEverything(EntityType.BODY)->qBodyType(BodyType.SOLID)));
    if (partCount != 2)
    {
        allProblems = append(allProblems, templateString({ "template": "There should be two parts, but there are #count", "count": partCount}));
    }
    const surfaceCount = size(evaluateQuery(context, qEverything(EntityType.BODY)->qBodyType(BodyType.SHEET)));
    if (surfaceCount != 1)
    {
        allProblems = append(allProblems, templateString({ "template": "There should be one surface, but there are #count", "count": surfaceCount}));
    }
    return allProblems as LibraryValidationProblems;
}
*/

