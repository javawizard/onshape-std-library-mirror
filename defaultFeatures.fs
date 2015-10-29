FeatureScript 244; /* Automatically generated version */
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/surfaceGeometry.fs", version : "");
import(path : "onshape/std/units.fs", version : "");
import(path : "onshape/std/valueBounds.fs", version : "");
import(path : "onshape/std/vector.fs", version : "");

/**
 * Creates a `Context` with default planes and an origin.
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
    var definition = { "defaultType" : defaultType, "size" : size };

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

