FeatureScript 3095; /* Automatically generated version */
import(path : "onshape/std/feature.fs", version : "3095.0");
import(path : "onshape/std/libraryValidation.fs", version : "3095.0");
import(path : "onshape/std/configurationInfo.fs", version : "3095.0");
import(path : "onshape/std/containers.fs", version : "3095.0");

/** Validates that a variable studio can be used to define a default tolerance library */
export function validate(context is Context, configurationInfo is ConfigurationInfo) returns LibraryValidationProblems
{
    var allProblems = new box(@validateToleranceSchema(context));

    // A key in BTResources.properties, so that validateForLibraryUse localizes it
    libraryVerify(size(configurationInfo.inputs) == 0, allProblems, "ELEMENT_LIBRARY_CANNOT_BE_CONFIGURED");

    return allProblems[] as LibraryValidationProblems;
}

