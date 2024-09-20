FeatureScript 2473; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import (path : "onshape/std/attributes.fs", version : "2473.0");
import (path : "onshape/std/containers.fs", version : "2473.0");
import (path : "onshape/std/context.fs", version : "2473.0");
import (path : "onshape/std/evaluate.fs", version : "2473.0");
import (path : "onshape/std/feature.fs", version : "2473.0");
import (path : "onshape/std/query.fs", version : "2473.0");
import (path : "onshape/std/table.fs", version : "2473.0");

/**
 * The possible types of a [FrameTopologyAttribute].
 *
 * @value SWEPT_FACE: The side faces of a frame, swept from the edges of the profile
 * @value SWEPT_EDGE: The side edges of a frame, swept from the vertices of the profile
 * @value CAP_FACE: The start and end cap faces of a frame
 */
export enum FrameTopologyType
{
    SWEPT_FACE,
    SWEPT_EDGE,
    CAP_FACE
}

/** @internal */
export const FRAME_ATTRIBUTE_TOPOLOGY_NAME = "frameTopology";

/** @internal */
export const FRAME_ATTRIBUTE_PROFILE_NAME = "frameProfile";

/** @internal */
export const FRAME_ATTRIBUTE_CUTLIST_NAME = "cutlist";

/** @internal */
const FRAME_ATTRIBUTE_CUSTOM_ALIGNMENT_POINT_NAME = "frameAlignmentPoint";

// == FrameProfileAttribute ==

/**
 * An attribute attached to the frame profile and the constructed frame body which defines the default cutlist
 * associated with the frame.
 */
export type FrameProfileAttribute typecheck canBeFrameProfileAttribute;

predicate canBeFrameProfileAttribute(value)
{
    value is map;
    for (var columnHeader, defaultValue in value)
    {
        columnHeader is string && columnHeader != "";
        defaultValue is string && defaultValue != "";
    }
}

/**
 * Construct a [FrameProfileAttribute].
 */
export function frameProfileAttribute(value is map) returns FrameProfileAttribute
{
    return value as FrameProfileAttribute;
}

/**
 * Get all [FrameProfileAttribute]s attached to the `frames`.
 */
export function getFrameProfileAttributes(context is Context, frames is Query)
{
    return getAttributes(context, {
        "entities" : frames,
        "name" : FRAME_ATTRIBUTE_PROFILE_NAME
    });
}

/**
 * Get the [FrameProfileAttribute] attached to the `frame`.  Throw if a single attribute is not found.
 */
export function getFrameProfileAttribute(context is Context, frame is Query)
{
    return getAttribute(context, {
        "entity" : frame,
        "name" : FRAME_ATTRIBUTE_PROFILE_NAME
    });
}

/**
 * Attach the given [FrameProfileAttribute] to the `frame`.
 */
export function setFrameProfileAttribute(context is Context, frame is Query, attribute is FrameProfileAttribute)
{
    setAttribute(context, {
        "entities" : frame,
        "name" : FRAME_ATTRIBUTE_PROFILE_NAME,
        "attribute" : attribute
    });
}

// == FrameTopologyAttribute ==

/**
 * An attribute assigned to certain faces and edges of the frames to aid in tracking the frame as it changes over the
 * series of frame-altering features.
 */
export type FrameTopologyAttribute typecheck canBeFrameTopologyAttribute;

/** @internal */
predicate canBeFrameTopologyAttribute(value)
{
    value is map;
    value.topologyType is FrameTopologyType;
    if (value.topologyType == FrameTopologyType.CAP_FACE)
    {
        value.isStart is boolean;
        value.isFrameTerminus is boolean;
        value.isCompositeTerminus is boolean;
    }
}

/**
 * Construct a [FrameTopologyAttribute] for `SWEPT_*` types.
 * @seealso [frameTopologyAttributeForCapFace]
 */
export function frameTopologyAttributeForSwept(topologyType is FrameTopologyType) returns FrameTopologyAttribute
precondition
{
    topologyType == FrameTopologyType.SWEPT_FACE || topologyType == FrameTopologyType.SWEPT_EDGE;
}
{
    return {
        "topologyType" : topologyType
    } as FrameTopologyAttribute;
}

/**
 * Construct a [FrameTopologyAttribute] for `CAP_FACE`.
 * @seealso [frameTopologyAttributeForSwept]
 */
export function frameTopologyAttributeForCapFace(isStartFace is boolean, isFrameTerminus is boolean, isCompositeTerminus is boolean) returns FrameTopologyAttribute
{
    return {
        "topologyType" : FrameTopologyType.CAP_FACE,
        "isStart" : isStartFace,
        "isFrameTerminus" : isFrameTerminus,
        "isCompositeTerminus" : isCompositeTerminus
    } as FrameTopologyAttribute;
}

/**
 * Get all [FrameTopologyAttribute]s attached to the `faces`.
 */
export function getFrameTopologyAttributes(context is Context, faces is Query)
{
    return getAttributes(context, {
        "entities" : faces,
        "name" : FRAME_ATTRIBUTE_TOPOLOGY_NAME
    });
}

/**
 * Get the [FrameTopologyAttribute] attached to the `face`.  Throw if a single attribute is not found.
 */
export function getFrameTopologyAttribute(context is Context, face is Query)
{
    return getAttribute(context, {
        "entity" : face,
        "name" : FRAME_ATTRIBUTE_TOPOLOGY_NAME
    });
}

/**
 * Attach the given [FrameTopologyAttribute] to each of the `entities`.
 */
export function setFrameTopologyAttribute(context is Context, entities is Query, attribute is FrameTopologyAttribute)
{
    setAttribute(context, {
        "entities" : entities,
        "name" : FRAME_ATTRIBUTE_TOPOLOGY_NAME,
        "attribute" : attribute
    });
}

// == CutlistAttribute ==

/**
 * An attribute attached to the composite created by the Cutlist feature which contains the cutlist table for that
 * composite.
 */
export type CutlistAttribute typecheck canBeCutlistAttribute;

/** @internal */
predicate canBeCutlistAttribute(value)
{
    value is map;
    value.featureId is Id;
    value.table is Table;
}

/**
 * Construct a [CutlistAttribute].
 */
export function cutlistAttribute(featureId is Id, table is Table) returns CutlistAttribute
{
    return {
        "featureId" : featureId,
        "table" : table
    } as CutlistAttribute;
}

/**
 * Get all [CutlistAttribute]s attached to the `composites`.
 */
export function getCutlistAttributes(context is Context, composites is Query)
{
    return getAttributes(context, {
        "entities" : composites,
        "name" : FRAME_ATTRIBUTE_CUTLIST_NAME
    });
}

/**
 * Get the [getCutlistAttribute] attached to the `composite`.  Throw if a single attribute is not found.
 */
export function getCutlistAttribute(context is Context, composite is Query)
{
    return getAttribute(context, {
        "entity" : composite,
        "name" : FRAME_ATTRIBUTE_CUTLIST_NAME
    });
}

/**
 * Attach the given [CutlistAttribute] to the `composite`.
 */
export function setCutlistAttribute(context is Context, composite is Query, attribute is CutlistAttribute)
{
    setAttribute(context, {
        "entities" : composite,
        "name" : FRAME_ATTRIBUTE_CUTLIST_NAME,
        "attribute" : attribute
    });
}

/**
 * Sets an attribute on the sketch entity point queries for later discovery and use during frame creation.
 */
export function setCustomFrameAlignmentPointAttribute(context is Context, pointsQuery is Query)
{
    if (!isQueryEmpty(context, pointsQuery))
    {

        setAttribute(context, {
                    "entities" : pointsQuery,
                    "name" : FRAME_ATTRIBUTE_CUSTOM_ALIGNMENT_POINT_NAME,
                    "attribute" : true
                });
    }
}

/**
 * Finds the sketch points in the `profileId` sketch with the custom alignment point attribute.
 */
export function getCustomFrameAlignmentPoints(context is Context, profileId is Id) returns Query
{
    return qCreatedBy(profileId, EntityType.VERTEX)->qHasAttribute(FRAME_ATTRIBUTE_CUSTOM_ALIGNMENT_POINT_NAME);
}
