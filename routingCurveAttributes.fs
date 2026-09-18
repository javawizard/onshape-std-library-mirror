FeatureScript 3083; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "3083.0");
import(path : "onshape/std/context.fs", version : "3083.0");
import(path : "onshape/std/query.fs", version : "3083.0");
import(path : "onshape/std/table.fs", version : "3083.0");

/** @internal */
export const ROUTING_CURVE_ATTRIBUTE_TABLE_NAME = "routingCurveLengthTable";

// Column ids/headers for the routing curve table
/** @internal */
export const ROUTING_CURVE_ITEM = "Item";
/** @internal */
export const ROUTING_CURVE_LENGTH = "Length";
/** @internal */
export const ROUTING_CURVE_CURVE_TYPE = "Curve type";
/** @internal */
export const ROUTING_CURVE_X = "X";
/** @internal */
export const ROUTING_CURVE_Y = "Y";
/** @internal */
export const ROUTING_CURVE_Z = "Z";
/** @internal */
export const ROUTING_CURVE_SEGMENT_LENGTH = "L";
/** @internal */
export const ROUTING_CURVE_BEND_ANGLE = "A";
/** @internal */
export const ROUTING_CURVE_CLOCKING_ANGLE = "R";
/** @internal */
export const ROUTING_CURVE_BEND_RADIUS = "Bend radius";

// == RoutingCurveTableAttribute ==

/**
 * An attribute attached to the wire body created by the Routing Curve feature which contains the routing curve table
 * for that body.
 */
export type RoutingCurveTableAttribute typecheck canBeRoutingCurveTableAttribute;

/** @internal */
predicate canBeRoutingCurveTableAttribute(value)
{
    value is map;
    value.featureId is Id;
    value.isPolyline is boolean;
}

/**
 * Construct a [RoutingCurveTableAttribute] marking a wire body as a routing curve with a table to be built by
 * defineTable.
 */
export function routingCurveTableAttribute(featureId is Id, isPolyline is boolean) returns RoutingCurveTableAttribute
{
    return {
        "featureId" : featureId,
        "isPolyline" : isPolyline
    } as RoutingCurveTableAttribute;
}

/**
 * Get the [RoutingCurveTableAttribute] attached to the `wireBody`.  Returns undefined if not found.
 */
export function getRoutingCurveTableAttribute(context is Context, wireBody is Query)
{
    return getAttribute(context, {
        "entity" : wireBody,
        "name" : ROUTING_CURVE_ATTRIBUTE_TABLE_NAME
    });
}

/**
 * Attach the given [RoutingCurveTableAttribute] to the `wireBody`.
 */
export function setRoutingCurveTableAttribute(context is Context, wireBody is Query, attribute is RoutingCurveTableAttribute)
{
    setAttribute(context, {
        "entities" : wireBody,
        "name" : ROUTING_CURVE_ATTRIBUTE_TABLE_NAME,
        "attribute" : attribute
    });
}

/**
 * Query for wire bodies that have a routing curve table attribute.
 */
export function qRoutingCurveWithTable(wireBody is Query) returns Query
{
    return qHasAttribute(wireBody, ROUTING_CURVE_ATTRIBUTE_TABLE_NAME);
}

// == Start vertex marker ==

/** @internal */
export const ROUTING_CURVE_START_VERTEX_ATTRIBUTE_NAME = "routingCurveStartVertex";

/**
 * Marks `vertex` as the wire vertex corresponding to the routing curve's first specified point. This lets the
 * table always begin its traversal at the point the user actually specified first, regardless of where
 * `constructPath` happens to start on a closed curve.
 */
export function setRoutingCurveStartVertex(context is Context, vertex is Query)
{
    setAttribute(context, {
        "entities" : vertex,
        "name" : ROUTING_CURVE_START_VERTEX_ATTRIBUTE_NAME,
        "attribute" : true
    });
}

/**
 * Query for the wire vertex (if any) marked as the routing curve's first specified point.
 */
export function qRoutingCurveStartVertex(wireBody is Query) returns Query
{
    return qHasAttribute(qOwnedByBody(wireBody, EntityType.VERTEX), ROUTING_CURVE_START_VERTEX_ATTRIBUTE_NAME);
}

/**
 * Removes every routing curve attribute (if any) from `wireBody` and its vertices. Intended for features that
 * can reshape a routing curve's wire body into something that's no longer a valid routing curve (e.g. Edit
 * Curve), so that stale attributes copied onto/left on the body don't cause the routing curve table to be built
 * from geometry they no longer describe.
 */
export function clearRoutingCurveAttributes(context is Context, wireBody is Query)
{
    if (!isQueryEmpty(context, wireBody))
    {
        setAttribute(context, {
            "entities" : wireBody,
            "name" : ROUTING_CURVE_ATTRIBUTE_TABLE_NAME,
            "attribute" : undefined
        });
    }
    const startVertex = qRoutingCurveStartVertex(wireBody);
    if (!isQueryEmpty(context, startVertex))
    {
        setAttribute(context, {
            "entities" : startVertex,
            "name" : ROUTING_CURVE_START_VERTEX_ATTRIBUTE_NAME,
            "attribute" : undefined
        });
    }
}

