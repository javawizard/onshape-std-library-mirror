FeatureScript ✨; /* Automatically generated version */
export import(path : "onshape/std/feature.fs", version : "");
export import(path : "onshape/std/extrude.fs", version : "");
export import(path : "onshape/std/revolve.fs", version : "");
export import(path : "onshape/std/boolean.fs", version : "");
export import(path : "onshape/std/shell.fs", version : "");
export import(path : "onshape/std/splitpart.fs", version : "");
export import(path : "onshape/std/draft.fs", version : "");
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/sketch.fs", version : "");
export import(path : "onshape/std/chamfer.fs", version : "");
export import(path : "onshape/std/fillet.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/primitives.fs", version : "");
export import(path : "onshape/std/pattern.fs", version : "");
export import(path : "onshape/std/transformCopy.fs", version : "");
export import(path : "onshape/std/cplane.fs", version : "");
export import(path : "onshape/std/mirror.fs", version : "");
export import(path : "onshape/std/sweep.fs", version : "");
export import(path : "onshape/std/deleteFace.fs", version : "");
export import(path : "onshape/std/moveFace.fs", version : "");
export import(path : "onshape/std/replaceFace.fs", version : "");
export import(path : "onshape/std/modifyFillet.fs", version : "");
export import(path : "onshape/std/mateConnector.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");
export import(path : "onshape/std/print.fs", version : "");
export import(path : "onshape/std/sectionpart.fs", version : "");
export import(path : "onshape/std/helix.fs", version : "");
export import(path : "onshape/std/thicken.fs", version : "");
export import(path : "onshape/std/loft.fs", version : "");
export import(path : "onshape/std/variable.fs", version : "");
export import(path : "onshape/std/hole.fs", version : "");

//start model context
export function newContextWithDefaults() returns Context
{
    return newContextWithDefaults(meter);
}

export function newContextWithDefaults(defLengthUnit is ValueWithUnits)
{
    var context = newContext();
    origin(context);

    var ranges = PLANE_SIZE_BOUNDS[defLengthUnit];
    var size = ranges[1] * defLengthUnit;

    defaultPlane(context, makeId("Front"), DefaultPlaneType.XZ, size);
    defaultPlane(context, makeId("Top"), DefaultPlaneType.XY, size);
    defaultPlane(context, makeId("Right"), DefaultPlaneType.YZ, size);
    return context;
}

