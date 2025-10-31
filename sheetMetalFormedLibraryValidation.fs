FeatureScript 2796; /* Automatically generated version */
import(path : "onshape/std/feature.fs", version : "2796.0");
import(path : "onshape/std/libraryValidation.fs", version : "2796.0");
import(path : "onshape/std/containers.fs", version : "2796.0");
import(path : "onshape/std/formedUtils.fs", version : "2796.0");

/** Validates that a part studio can be part of a sheet metal form library */
export function validate(context is Context) returns LibraryValidationProblems
{
    var allProblems = [];
    const positiveBodies = evaluateQuery(context, qBodiesWithFormAttribute(FORM_BODY_POSITIVE_PART)->qBodyType(BodyType.SOLID));
    const nPositiveBodies = size(positiveBodies);
    if (nPositiveBodies > 1)
       allProblems = append(allProblems, {template : "There should be at most one solid positive body, found #count", count : nPositiveBodies});

    const negativeBodies = evaluateQuery(context, qBodiesWithFormAttribute(FORM_BODY_NEGATIVE_PART)->qBodyType(BodyType.SOLID));
    const nNegativeBodies = size(negativeBodies);
    if (nNegativeBodies > 1)
       allProblems = append(allProblems, {template : "There should be at most one solid negtive body, found #count", count : nNegativeBodies});

    if (nPositiveBodies + nNegativeBodies == 0)
         allProblems = append(allProblems, {template : "There should be at at least one solid body to be marked as positive or negative"});
    return allProblems as LibraryValidationProblems;
}

