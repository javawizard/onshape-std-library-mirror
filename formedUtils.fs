FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/context.fs", version : "✨");
import(path : "onshape/std/query.fs", version : "✨");

/** @internal */
const FORM_BODY_ATTRIBUTE_NAME = "formBodyAttribute";

/** @internal */
export const FORM_BODY_POSITIVE_PART = "positivePart";

/** @internal */
export const FORM_BODY_NEGATIVE_PART = "negativePart";

/** @internal */
export const FORM_BODY_SKETCH_FOR_FLAT_VIEW = "sketchForFlatView";

/** @internal */
export const FORM_BODY_CSYS_MATE_CONNECTOR = "cSysMateConnector";

// == FormAttribute ==

/**
 * An attribute attached to the parts, sketch and mate connector in a form part studio
 * which define the positive and negative volumes, sketch for the flat view and
 * coordinate system associated with the form.
 */
type FormAttribute typecheck canBeFormAttribute;

predicate canBeFormAttribute(value)
{
    value is string;
}

/**
 * Construct a FormAttribute.
 */
function formAttribute(value is string) returns FormAttribute
{
    return value as FormAttribute;
}

/**
 * Attach the given FormAttribute to the `bodies`.
 */
export function setFormAttribute(context is Context, bodies is Query, attribute is string)
{
    setAttribute(context, {
        "entities" : bodies,
        "name" : FORM_BODY_ATTRIBUTE_NAME,
        "attribute" : formAttribute(attribute)
    });
}

/**
 * Query for all bodies marked with a FormAttribute and value exactly equal to `attribute`
 * @seealso [setFormAttribute]
 */
export function qBodiesWithFormAttribute(attribute is string)
{
    return qHasAttributeWithValue(FORM_BODY_ATTRIBUTE_NAME, formAttribute(attribute));
}

/**
 * Query for all bodies in `queryToFilter` marked with a FormAttribute and value exactly equal to `attribute`
 * @seealso [setFormAttribute]
 */
export function qBodiesWithFormAttribute(queryToFilter is Query, attribute is string) returns Query
{
    return qHasAttributeWithValue(queryToFilter, FORM_BODY_ATTRIBUTE_NAME, formAttribute(attribute));
}

/**
 * Query for all bodies in `queryToFilter` marked with a FormAttribute and value exactly equal to one of the `attributes`
 * @seealso [setFormAttribute]
 */
export function qBodiesWithFormAttributes(queryToFilter is Query, attributes is array) returns Query
{
    var subQueries = [];
    for (var attribute in attributes)
    {
        subQueries = append(subQueries, qHasAttributeWithValue(queryToFilter, FORM_BODY_ATTRIBUTE_NAME, formAttribute(attribute)));
    }
    return qUnion(subQueries);
}

/**
 *  Used in derived to ensure that form bodies attached to flat pattern about to be deleted are also deleted
 *  BEL-238166
 */
export function computeFormArtifactsToDelete(context is Context, bodiesToKeep is Query, toDelete is Query) returns Query
{
    if (isQueryEmpty(context, toDelete))
    {
        return qNothing();
    }
    const toDeleteEvaluatedQ = qUnion(evaluateQuery(context, toDelete));
    const smFormedArtifactsQ = bodiesToKeep->qSheetMetalFormFilter(SMFormType.YES);
    var canNotKeep = [];
    //Consider form artifacts in bodiesToKeep, add to canNotKeep those attached to a body to be deleted
    for (var form in evaluateQuery(context, smFormedArtifactsQ))
    {
        if (!isQueryEmpty(context, qIntersection([form->qPartsAttachedTo(), toDeleteEvaluatedQ])))
            canNotKeep = append(canNotKeep, form);
    }
    return qUnion(canNotKeep);
 }
