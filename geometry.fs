FeatureScript 225; /* Automatically generated version */
/* Onshape standard library utilities */
/* Features! */
export import(path : "onshape/std/context.fs", version : "");
export import(path : "onshape/std/defaultFeatures.fs", version : "");
export import(path : "onshape/std/feature.fs", version : "");

/* Bounding box */
export import(path : "onshape/std/box.fs", version : "");

/* Query and evaluation */
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/query.fs", version : "");

/* Units and bounds */
export import(path : "onshape/std/units.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");

/* Math, string, vector, matrix, and support functions */
export import(path : "onshape/std/containers.fs", version : "");
export import(path : "onshape/std/curveGeometry.fs", version : "");
export import(path : "onshape/std/mathUtils.fs", version : "");
export import(path : "onshape/std/string.fs", version : "");
export import(path : "onshape/std/surfaceGeometry.fs", version : "");

/* Onshape standard library features */
export import(path : "onshape/std/boolean.fs", version : "");
export import(path : "onshape/std/chamfer.fs", version : "");
export import(path : "onshape/std/cplane.fs", version : "");
export import(path : "onshape/std/deleteBodies.fs", version : "");
export import(path : "onshape/std/deleteFace.fs", version : "");
export import(path : "onshape/std/draft.fs", version : "");
export import(path : "onshape/std/extrude.fs", version : "");
export import(path : "onshape/std/fillet.fs", version : "");
export import(path : "onshape/std/helix.fs", version : "");
export import(path : "onshape/std/hole.fs", version : "");
export import(path : "onshape/std/importDerived.fs", version : "");
export import(path : "onshape/std/importForeign.fs", version : "");
export import(path : "onshape/std/loft.fs", version : "");
export import(path : "onshape/std/mateConnector.fs", version : "");
export import(path : "onshape/std/mirror.fs", version : "");
export import(path : "onshape/std/modifyFillet.fs", version : "");
export import(path : "onshape/std/moveFace.fs", version : "");
export import(path : "onshape/std/pattern.fs", version : "");
export import(path : "onshape/std/primitives.fs", version : "");
export import(path : "onshape/std/replaceFace.fs", version : "");
export import(path : "onshape/std/revolve.fs", version : "");
export import(path : "onshape/std/shell.fs", version : "");
export import(path : "onshape/std/sketch.fs", version : "");
export import(path : "onshape/std/sectionpart.fs", version : "");
export import(path : "onshape/std/splitpart.fs", version : "");
export import(path : "onshape/std/sweep.fs", version : "");
export import(path : "onshape/std/thicken.fs", version : "");
export import(path : "onshape/std/transformCopy.fs", version : "");
export import(path : "onshape/std/variable.fs", version : "");

//start model context
/**
 * TODO: description
 */
export function newContextWithDefaults() returns Context
{
    return newContextWithDefaults(meter);
}

export function newContextWithDefaults(defLengthUnit is ValueWithUnits)
{
    var context = newContext();
    origin(context);

    const ranges = PLANE_SIZE_BOUNDS[defLengthUnit];
    const size = ranges[1] * defLengthUnit;

    defaultPlane(context, makeId("Front"), DefaultPlaneType.XZ, size);
    defaultPlane(context, makeId("Top"), DefaultPlaneType.XY, size);
    defaultPlane(context, makeId("Right"), DefaultPlaneType.YZ, size);
    return context;
}

