FeatureScript 3095; /* Automatically generated version */
import(path : "onshape/std/feature.fs", version : "3095.0");
import(path : "onshape/std/libraryValidation.fs", version : "3095.0");
import(path : "onshape/std/configurationInfo.fs", version : "3095.0");
import(path : "onshape/std/containers.fs", version : "3095.0");
import(path : "onshape/std/formedUtils.fs", version : "3095.0");

const THICKNESS_PARAMETER_ID = "thickness";
const MIN_THICKNESS = 0.00002 * meter;
const MAX_THICKNESS = 1 * meter;

/**
 * Problems are reported as keys in BTResources.properties (with substitutions in TemplateString form) so that
 * validateForLibraryUse localizes them.
 */
const THICKNESS_PARAMETER_PROBLEM = {
        template : "ELEMENT_LIBRARY_CONFIGURATION_PARAMETER_FAILURE",
        "type" : "length",
        id : THICKNESS_PARAMETER_ID
    };

const THICKNESS_RANGE_PROBLEM = {
        template : "ELEMENT_LIBRARY_BOUNDED_CONFIGURATION_PARAMETER_FAILURE",
        "type" : "length",
        id : THICKNESS_PARAMETER_ID,
        "min" : MIN_THICKNESS,
        "max" : MAX_THICKNESS
    };

/** Validates that a part studio can be part of a sheet metal form library */
export function validate(context is Context, configurationInfo is ConfigurationInfo) returns LibraryValidationProblems
{
    var allProblems = new box([]);

    const thicknessInputs is array = filter(configurationInfo.inputs,
        input => input.id == THICKNESS_PARAMETER_ID);
    if (libraryVerify(size(thicknessInputs) == 1, allProblems, THICKNESS_PARAMETER_PROBLEM))
    {
        const thickness is ConfigurationInputInfo = thicknessInputs[0];
        if (libraryVerify(thickness.inputType == ConfigurationInputType.LENGTH, allProblems,
                THICKNESS_PARAMETER_PROBLEM))
        {
            libraryVerify(thickness.minValue * thickness.units <= MIN_THICKNESS &&
                    thickness.maxValue * thickness.units >= MAX_THICKNESS, allProblems, THICKNESS_RANGE_PROBLEM);
        }
    }

    const positiveBodies = evaluateQuery(context, qBodiesWithFormAttribute(FORM_BODY_POSITIVE_PART)->qBodyType(BodyType.SOLID));
    const nPositiveBodies = size(positiveBodies);
    libraryVerify(nPositiveBodies <= 1, allProblems,
        {template : "ELEMENT_LIBRARY_TOO_MANY_POSITIVE_BODIES", count : nPositiveBodies});

    const negativeBodies = evaluateQuery(context, qBodiesWithFormAttribute(FORM_BODY_NEGATIVE_PART)->qBodyType(BodyType.SOLID));
    const nNegativeBodies = size(negativeBodies);
    libraryVerify(nNegativeBodies <= 1, allProblems,
        {template : "ELEMENT_LIBRARY_TOO_MANY_NEGATIVE_BODIES", count : nNegativeBodies});

    libraryVerify(nPositiveBodies + nNegativeBodies > 0, allProblems, "ELEMENT_LIBRARY_NO_FORM_BODIES");

    return allProblems[] as LibraryValidationProblems;
}

