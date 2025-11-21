FeatureScript 2815; /* Automatically generated version */
import(path : "onshape/std/feature.fs", version : "2815.0");
import(path : "onshape/std/libraryValidation.fs", version : "2815.0");

/** Validates that a variable studio can be used to define a default tolerance library */
export function validate(context is Context) returns LibraryValidationProblems
{
    return @validateToleranceSchema(context) as LibraryValidationProblems;
}
