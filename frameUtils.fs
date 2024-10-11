FeatureScript 2491; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs",version : "2491.0");
import(path : "onshape/std/containers.fs",version : "2491.0");
import(path : "onshape/std/context.fs",version : "2491.0");
import(path : "onshape/std/coordSystem.fs",version : "2491.0");
import(path : "onshape/std/evaluate.fs",version : "2491.0");
import(path : "onshape/std/feature.fs",version : "2491.0");
import(path : "onshape/std/frameAttributes.fs",version : "2491.0");
import(path : "onshape/std/query.fs",version : "2491.0");
import(path : "onshape/std/surfaceGeometry.fs",version : "2491.0");
import(path : "onshape/std/units.fs",version : "2491.0");
import(path : "onshape/std/valueBounds.fs",version : "2491.0");
import(path : "onshape/std/vector.fs",version : "2491.0");

/** @internal */
export enum FrameCornerType
{
    annotation { "Name" : "Miter" }
    MITER,
    annotation { "Name" : "Butt" }
    BUTT,
    annotation { "Name" : "Coped butt" }
    COPED_BUTT,
    annotation { "Name" : "None" }
    NONE,
    // TANGENT type is not intended for UI or manual control. It aids in tagging G1 continuous frame segments for compositing.
    annotation { "Name" : "Tangent", "Hidden" : true }
    TANGENT
}

// Default cutlist table column ids/headers
/** @internal */
export const CUTLIST_ITEM = "Item";
/** @internal */
export const CUTLIST_QTY = "Qty";
/** @internal */
export const CUTLIST_STANDARD = "Standard";
/** @internal */
export const CUTLIST_DESCRIPTION = "Description";
/** @internal */
export const CUTLIST_LENGTH = "Length";
/** @internal */
export const CUTLIST_ANGLE_1 = "Angle 1";
/** @internal */
export const CUTLIST_ANGLE_2 = "Angle 2";

// Default descriptions for various cutlist entries
const CUTLIST_DESCRIPTION_CUSTOM_PROFILE = "Custom profile";
/** @internal */
export const CUTLIST_DESCRIPTION_CUTLIST_ENTRY = "Cutlist entry";

function qFrameTopology(queryToFilter is Query, topologyType is FrameTopologyType, isStart, isFrameTerminus, isCompositeTerminus)
{
    const attributeQuery = qHasAttributeWithValueMatching(queryToFilter, FRAME_ATTRIBUTE_TOPOLOGY_NAME, {
                "topologyType" : topologyType,
                "isStart" : isStart,
                "isFrameTerminus" : isFrameTerminus,
                "isCompositeTerminus" : isCompositeTerminus
            });
    return attributeQuery;
}

function isFaceOfTopologyType(context is Context, face is Query, topologyType is FrameTopologyType, isStart,
    isFrameTerminus, isCompositeTerminus) returns boolean
{
    const attributeQuery = qFrameTopology(face, topologyType, isStart, isFrameTerminus, isCompositeTerminus);
    return !isQueryEmpty(context, attributeQuery);
}

/** @internal */
export function isCapFace(context is Context, face is Query) returns boolean
{
    return isFaceOfTopologyType(context, face, FrameTopologyType.CAP_FACE, undefined, undefined, undefined);
}

/** @internal */
export function isStartFace(context is Context, face is Query) returns boolean
{
    return isFaceOfTopologyType(context, face, FrameTopologyType.CAP_FACE, true, undefined, undefined);
}

/** @internal */
export function isSweptFace(context is Context, face is Query) returns boolean
{
    return isFaceOfTopologyType(context, face, FrameTopologyType.SWEPT_FACE, undefined, undefined, undefined);
}

/** @internal */
export function isInternalCapFace(context is Context, face is Query) returns boolean
{
    return isFaceOfTopologyType(context, face, FrameTopologyType.CAP_FACE, undefined, false, undefined);
}

/** @internal */
export function isFrameCapFace(context is Context, face is Query) returns boolean
{
    return isFaceOfTopologyType(context, face, FrameTopologyType.CAP_FACE, undefined, true, undefined);
}

/** @internal */
export function isCompositeFrameCapFace(context is Context, face is Query) returns boolean
{
    return isFaceOfTopologyType(context, face, FrameTopologyType.CAP_FACE, undefined, true, undefined);
}

/** @internal */
export function qFrameStartFace(frame is Query) returns Query
{
    frame = qUnion(frame, qContainedInCompositeParts(frame));
    const faceQuery = qFrameTopology(qOwnedByBody(frame, EntityType.FACE), FrameTopologyType.CAP_FACE, true, undefined, undefined);
    return faceQuery;
}

/** @internal */
export function qFrameEndFace(frame is Query) returns Query
{
    frame = qUnion(frame, qContainedInCompositeParts(frame));
    const faceQuery = qFrameTopology(qOwnedByBody(frame, EntityType.FACE), FrameTopologyType.CAP_FACE, false, undefined, undefined);
    return faceQuery;
}

/** @internal */
export function qFrameOppositeFace(context is Context, frame is Query, face is Query) returns Query
{
    frame = qUnion(frame, qContainedInCompositeParts(frame));
    const allFaces = qOwnedByBody(frame, EntityType.FACE);
    const sweptFaces = qFrameSweptFace(frame);
    const oppositeFace = qSubtraction(allFaces, qUnion([sweptFaces, face]));
    return oppositeFace;
}

/** @internal */
export function qFrameSweptEdge(frame is Query) returns Query
{
    frame = qUnion(frame, qContainedInCompositeParts(frame));
    const edgeQuery = qFrameTopology(qOwnedByBody(frame, EntityType.EDGE), FrameTopologyType.SWEPT_EDGE, undefined, undefined, undefined);
    return edgeQuery;
}

/** @internal */
export function qFrameSweptFace(frame is Query) returns Query
{
    frame = qUnion(frame, qContainedInCompositeParts(frame));
    const sweptFaceQuery = qFrameTopology(qOwnedByBody(frame, EntityType.FACE), FrameTopologyType.SWEPT_FACE, undefined, undefined, undefined);
    return sweptFaceQuery;
}

/** @internal */
export function qFrameCompositeTerminusFace(frame is Query) returns Query
{
    frame = qUnion(frame, qContainedInCompositeParts(frame));
    const sweptFaceQuery = qFrameTopology(qOwnedByBody(frame, EntityType.FACE), FrameTopologyType.CAP_FACE, undefined, undefined, true);
    return sweptFaceQuery;
}

/** @internal */
export function qFrameCompositeTerminusStartFace(frame is Query) returns Query
{
    frame = qUnion(frame, qContainedInCompositeParts(frame));
    const sweptFaceQuery = qFrameTopology(qOwnedByBody(frame, EntityType.FACE), FrameTopologyType.CAP_FACE, true, undefined, true);
    return sweptFaceQuery;
}

/** @internal */
export function qFrameCompositeTerminusEndFace(frame is Query) returns Query
{
    frame = qUnion(frame, qContainedInCompositeParts(frame));
    const sweptFaceQuery = qFrameTopology(qOwnedByBody(frame, EntityType.FACE), FrameTopologyType.CAP_FACE, false, undefined, true);
    return sweptFaceQuery;
}

/** @internal */
export function qFrameAllFaces(frame is Query) returns Query
{
    frame = qUnion(frame, qContainedInCompositeParts(frame));
    const allFrameTopologyFaces = qOwnedByBody(frame, EntityType.FACE)->qHasAttribute(FRAME_ATTRIBUTE_TOPOLOGY_NAME);
    return allFrameTopologyFaces;
}

/** @internal */
export function qFrameAllBodies()
{
    return qAllModifiableSolidBodies()->qHasAttribute(FRAME_ATTRIBUTE_PROFILE_NAME);
}

/** @internal */
export function qFrameAllClosedCompositeSegments()
{
    const allClosedCompositeFrameSegments = qEverything(EntityType.BODY) -> qCompositePartTypeFilter(CompositePartType.CLOSED) -> qHasAttribute(FRAME_ATTRIBUTE_PROFILE_NAME);
    return allClosedCompositeFrameSegments;
}

/** @internal */
export function qFrameCutlist(compositeBody is Query) returns Query
{
    const cutlistCompositeBodyQuery = qHasAttribute(compositeBody, FRAME_ATTRIBUTE_CUTLIST_NAME);
    return cutlistCompositeBodyQuery;
}

/** @internal */
export function getFrameProfileAttributeOrDefault(context is Context, profile is Query, configuration) returns FrameProfileAttribute
precondition
{
    configuration is map || configuration == undefined;
}
{
    const profileAttribute = getFrameProfileAttribute(context, profile);
    if (profileAttribute != undefined)
    {
        return profileAttribute;
    }

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1730_NO_CUSTOM_DESCRIPTION))
    {
        return frameProfileAttribute({});
    }
    else
    {
        // Old functionality: create a default description based off of configuration. Disabled because it does not play
        // well with config booleans and config variables.
        var description = CUTLIST_DESCRIPTION_CUSTOM_PROFILE;
        if (configuration != undefined)
        {
            for (var _, value in configuration)
            {
                description = description ~ " - " ~ value;
            }
        }
        return frameProfileAttribute({ (CUTLIST_DESCRIPTION) : description });
    }
}

/** @internal */
export function max(elements is array, lessThan is function)
{
    var max = undefined;
    for (var element in elements)
    {
        if (max == undefined || lessThan(max, element))
        {
            max = element;
        }
    }
    return max;
}

/** @internal */
export function isFrameCompositeSegment(context is Context, frame is Query) returns boolean
{
    const compositeFilteredQuery = qCompositePartTypeFilter(frame, CompositePartType.CLOSED);
    const frameProfileAttribute = try silent(getFrameProfileAttribute(context, compositeFilteredQuery));
    return frameProfileAttribute != undefined;
}


/** @internal */
export function getAlignmentPointOffsetData(context is Context, sketchRegions is Query, sketchCS is CoordSystem) returns map
{
    const bb = evBox3d(context, {
                "topology" : sketchRegions,
                "cSys" : sketchCS,
                "tight" : true
            });

    // center2dProfileCS is usually but not always and not required to be at sketchPlane center so this offset is also needed.
    const center2dProfileCS = 0.5 * (bb.maxCorner + bb.minCorner);
    const halfExtents = 0.5 * (bb.maxCorner - bb.minCorner);

    //compute standard offsets with layout:
    //8 7 6
    //5 4 3
    //2 1 0
    var standardOffsets = makeArray(9);
    standardOffsets[8] = vector(-halfExtents[0], halfExtents[1], 0 * meter);
    standardOffsets[7] = vector(0 * meter, halfExtents[1], 0 * meter);
    standardOffsets[6] = vector(halfExtents[0], halfExtents[1], 0 * meter);
    standardOffsets[5] = vector(-halfExtents[0], 0 * meter, 0 * meter);
    standardOffsets[4] = vector(0, 0, 0) * meter;
    standardOffsets[3] = -standardOffsets[5];
    standardOffsets[2] = -standardOffsets[6];
    standardOffsets[1] = -standardOffsets[7];
    standardOffsets[0] = -standardOffsets[8];

    return {
            "offsets" : standardOffsets,
            "centerOffset" : center2dProfileCS,
            "halfExtents" : halfExtents
        };
}

