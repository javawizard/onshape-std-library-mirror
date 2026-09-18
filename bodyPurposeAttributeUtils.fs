FeatureScript 3083; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "3083.0");
import(path : "onshape/std/context.fs", version : "3083.0");
import(path : "onshape/std/partpurpose.gen.fs", version : "3083.0");
import(path : "onshape/std/query.fs", version : "3083.0");

// == BodyPurposeAttribute ==

/**
 * An attribute attached to a part which defines the purpose of the body
 */
type BodyPurposeAttribute typecheck canBeBodyPurposeAttribute;

predicate canBeBodyPurposeAttribute(value)
{
    value is PartPurpose;
}

/**
 * Construct a BodyPurposeAttribute from a PartPurpose enum value.
 */
function bodyPurposeAttribute(purpose is PartPurpose) returns BodyPurposeAttribute
{
    return purpose as BodyPurposeAttribute;
}

/**
 * Attach the given body purpose to the `bodies` using the specified attribute name.
 */
export function setBodyPurposeAttribute(context is Context, bodies is Query, attributeName is string, purpose is PartPurpose)
{
    setAttribute(context, {
        "entities" : bodies,
        "name" : attributeName,
        "attribute" : bodyPurposeAttribute(purpose)
    });
}

/**
 * Query for all bodies marked with a body purpose attribute equal to `purpose`
 * @seealso [setBodyPurposeAttribute]
 */
export function qBodiesWithBodyPurpose(attributeName is string, purpose is PartPurpose)
{
    return qHasAttributeWithValue(attributeName, bodyPurposeAttribute(purpose));
}

/**
 * Query for all bodies in `queryToFilter` marked with a body purpose attribute equal to `purpose`
 * @seealso [setBodyPurposeAttribute]
 */
export function qBodiesWithBodyPurpose(queryToFilter is Query, attributeName is string, purpose is PartPurpose) returns Query
{
    return qHasAttributeWithValue(queryToFilter, attributeName, bodyPurposeAttribute(purpose));
}

