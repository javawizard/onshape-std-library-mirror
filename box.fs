FeatureScript 190; /* Automatically generated version */
export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/vector.fs", version : "");
export import(path : "onshape/std/units.fs", version : "");


export type Box3d typecheck canBeBox3d;

export predicate canBeBox3d(value)
{
    value is map;
    isLengthVector(value.minCorner);
    isLengthVector(value.maxCorner);
}

export function box3d(minCorner is Vector, maxCorner is Vector) returns Box3d
{
    return { 'minCorner' : minCorner, 'maxCorner' : maxCorner } as Box3d;
}

export function extendBox3d(bBox is Box3d, absoluteValue is ValueWithUnits, factor is number) returns Box3d
precondition
{
    isLength(absoluteValue);
}
{
    var midPoint is Vector = (bBox.minCorner + bBox.maxCorner) * 0.5;
    var halfDiagonal is Vector = (bBox.maxCorner - bBox.minCorner) * 0.5;
    var absoluteIncrement is Vector = vector(absoluteValue, absoluteValue, absoluteValue);

    return box3d(midPoint - absoluteIncrement - halfDiagonal * (1 + factor),
                 midPoint + absoluteIncrement + halfDiagonal * (1 + factor));

}

export function evBox3d(context is Context, arg is map) returns map
precondition
{
    arg.topology is Query;
    arg.cSys == undefined || arg.cSys is Transform;
}
{
    var result = @evBox(context, arg);
    if (result.error == undefined)
        result.result = box3d(meter * vector(result.result.minCorner), meter * vector(result.result.maxCorner));
    return result;
}


