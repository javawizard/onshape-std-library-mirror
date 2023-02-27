FeatureScript 1977; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/feature.fs", version : "1977.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1977.0");
import(path : "onshape/std/units.fs", version : "1977.0");
import(path : "onshape/std/valueBounds.fs", version : "1977.0");
import(path : "onshape/std/vector.fs", version : "1977.0");

const TOP_PLANE_ID = makeId("Top");
const RIGHT_PLANE_ID = makeId("Right");
const FRONT_PLANE_ID = makeId("Front");
const ORIGIN_ID = makeId("Origin");

/**
 * Creates a [Context] with default planes and an origin.
 */
export function newContextWithDefaults() returns Context
{
    return newContextWithDefaults(meter);
}

/** @internal */
export function newContextWithDefaults(defaultLengthUnit is ValueWithUnits)
{
    var context = newContext();
    addDefaultFeatures(context, defaultLengthUnit);
    return context;
}

/** @internal */
export function addDefaultFeatures(context is Context, defaultLengthUnit is ValueWithUnits)
{
    createOrigin(context);

    const range = PLANE_SIZE_BOUNDS[defaultLengthUnit];
    const size = (range is number ? range : range[1]) * defaultLengthUnit;

    createDefaultPlane(context, FRONT_PLANE_ID, DefaultPlaneType.XZ, size);
    createDefaultPlane(context, TOP_PLANE_ID, DefaultPlaneType.XY, size);
    createDefaultPlane(context, RIGHT_PLANE_ID, DefaultPlaneType.YZ, size);
}

enum DefaultPlaneType
{
    XY,
    YZ,
    XZ
}

function createDefaultPlane(context is Context, id is Id, defaultType is DefaultPlaneType, size)
precondition
{
    isLength(size);
}
{
    var definition = { "defaultType" : defaultType, "width" : size, "height" : size };

    startFeature(context, id, definition);
    const origin = vector(0, 0, 0) * meter;
    if (defaultType == DefaultPlaneType.XY)
        definition.plane = plane(origin, vector(0, 0, 1), vector(1, 0, 0));
    else if (defaultType == DefaultPlaneType.YZ)
        definition.plane = plane(origin, vector(1, 0, 0), vector(0, 1, 0));
    else if (defaultType == DefaultPlaneType.XZ)
        definition.plane = plane(origin, vector(0, -1, 0), vector(1, 0, 0));
    opPlane(context, id, definition);
    endFeature(context, id);
}

function createOrigin(context is Context)
{
    startFeature(context, ORIGIN_ID, {});
    const out = opPoint(context, ORIGIN_ID, { "point" : vector(0, 0, 0) * meter, "origin" : true });
    endFeature(context, ORIGIN_ID);
    return out;
}

/**
 * A query for all default created bodies in a context, that is, the top, right, front planes and the origin point.
 */
export function qDefaultBodies() returns Query
{
    return qUnion([
                qFrontPlane(EntityType.BODY),
                qRightPlane(EntityType.BODY),
                qTopPlane(EntityType.BODY),
                qOrigin(EntityType.BODY)
            ]);
}

/**
 * A query for the front plane.
 * @param entityType {EntityType}: Specify type `FACE` or `BODY`.
 */
export function qFrontPlane(entityType is EntityType) returns Query
precondition
{
    entityType == EntityType.FACE || entityType == EntityType.BODY;
}
{
    return qCreatedBy(FRONT_PLANE_ID, entityType);
}

/**
 * A query for the right plane.
 * @param entityType {EntityType}: Specify type `FACE` or `BODY`.
 */
export function qRightPlane(entityType is EntityType) returns Query
precondition
{
    entityType == EntityType.FACE || entityType == EntityType.BODY;
}
{
    return qCreatedBy(RIGHT_PLANE_ID, entityType);
}

/**
 * A query for the top plane.
 * @param entityType {EntityType}: Specify type `FACE` or `BODY`.
 */
export function qTopPlane(entityType is EntityType) returns Query
precondition
{
    entityType == EntityType.FACE || entityType == EntityType.BODY;
}
{
    return qCreatedBy(TOP_PLANE_ID, entityType);
}

/**
 * A query for the origin point.
 * @param entityType {EntityType}: Specify type `VERTEX` or `BODY`.
 */
export function qOrigin(entityType is EntityType) returns Query
precondition
{
    entityType == EntityType.VERTEX || entityType == EntityType.BODY;
}
{
    return qCreatedBy(ORIGIN_ID, entityType);
}
