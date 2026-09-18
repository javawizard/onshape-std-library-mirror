FeatureScript 3083; /* Automatically generated version */
import(path : "onshape/std/bodyPurposeAttributeUtils.fs", version : "3083.0");
import(path : "onshape/std/containers.fs", version : "3083.0");
import(path : "onshape/std/context.fs", version : "3083.0");
import(path : "onshape/std/feature.fs", version : "3083.0");
import(path : "onshape/std/libraryValidation.fs", version : "3083.0");
import(path : "onshape/std/query.fs", version : "3083.0");
import(path : "onshape/std/partpurpose.gen.fs", version : "3083.0");

/**
 * Body purpose attribute name used to tag bodies
 * This must match the value in tag.fs
 */
const BODY_PURPOSE_ATTRIBUTE_NAME = "bodyPurposeAttribute";

/**
 * Validates that a part studio can be part of a custom standard content library
 *
 * Requirements:
 * - Must have exactly one solid body or composite part tagged with standard content body purpose attribute
 */
export function validate(context is Context) returns LibraryValidationProblems
{
    var allProblems = [];

    // Query for all solid bodies or composite parts with custom standard content body purpose attribute
    const taggedBodies = evaluateQuery(context, qBodiesWithBodyPurpose(
        BODY_PURPOSE_ATTRIBUTE_NAME,
        PartPurpose.STANDARD_CONTENT_PART)->qBodyType([BodyType.SOLID, BodyType.COMPOSITE]));
    const nTaggedBodies = size(taggedBodies);

    // Check that exactly one solid body or composite part is tagged
    if (nTaggedBodies != 1)
    {
        allProblems = append(allProblems, {
            template : "There should be exactly one solid body or composite part marked as standard content, found #count",
            count : nTaggedBodies
        });
    }

    return allProblems as LibraryValidationProblems;
}

