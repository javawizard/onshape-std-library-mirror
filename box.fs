FeatureScript 225; /* Automatically generated version */
import(path : "onshape/std/units.fs", version : "");
import(path : "onshape/std/vector.fs", version : "");

/**
 * TODO: description
 */
export type Box3d typecheck canBeBox3d;

export predicate canBeBox3d(value)
{
    value is map;
    isLengthVector(value.minCorner);
    isLengthVector(value.maxCorner);
}

/**
 * TODO: description
 * @param minCorner
 * @param maxCorner
 */
export function box3d(minCorner is Vector, maxCorner is Vector) returns Box3d
{
    return { 'minCorner' : minCorner, 'maxCorner' : maxCorner } as Box3d;
}

/**
 * TODO: description
 * @param bBox
 * @param absoluteValue
 * @param factor
 */
export function extendBox3d(bBox is Box3d, absoluteValue is ValueWithUnits, factor is number) returns Box3d
precondition
{
    isLength(absoluteValue);
}
{
    const midPoint is Vector = (bBox.minCorner + bBox.maxCorner) * 0.5;
    const halfDiagonal is Vector = (bBox.maxCorner - bBox.minCorner) * 0.5;
    const absoluteIncrement is Vector = vector(absoluteValue, absoluteValue, absoluteValue);

    return box3d(midPoint - absoluteIncrement - halfDiagonal * (1 + factor),
                 midPoint + absoluteIncrement + halfDiagonal * (1 + factor));

}

