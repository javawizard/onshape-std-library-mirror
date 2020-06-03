FeatureScript 1301; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/feature.fs", version : "1301.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1301.0");
import(path : "onshape/std/units.fs", version : "1301.0");
import(path : "onshape/std/valueBounds.fs", version : "1301.0");
import(path : "onshape/std/vector.fs", version : "1301.0");

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
    origin(context);

    const range = PLANE_SIZE_BOUNDS[defaultLengthUnit];
    const size = (range is number ? range : range[1]) * defaultLengthUnit;

    defaultPlane(context, makeId("Front"), DefaultPlaneType.XZ, size);
    defaultPlane(context, makeId("Top"), DefaultPlaneType.XY, size);
    defaultPlane(context, makeId("Right"), DefaultPlaneType.YZ, size);
}

enum DefaultPlaneType
{
    XY,
    YZ,
    XZ
}

function defaultPlane(context is Context, id is Id, defaultType is DefaultPlaneType, size)
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

function origin(context is Context)
{
    const id = makeId("Origin");
    startFeature(context, id, {});
    const out = opPoint(context, id, { "point" : vector(0, 0, 0) * meter, "origin" : true });
    endFeature(context, id);
    return out;
}

