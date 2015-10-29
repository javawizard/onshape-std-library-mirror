FeatureScript 244; /* Automatically generated version */
import(path : "onshape/std/units.fs", version : "");
import(path : "onshape/std/vector.fs", version : "");

/**
 * A three-dimensional bounding box.
 */
export type Box3d typecheck canBeBox3d;

export predicate canBeBox3d(value)
{
    value is map;
    isLengthVector(value.minCorner);
    isLengthVector(value.maxCorner);
}

/**
 * Construct a bounding box from two opposite corners.
 */
export function box3d(minCorner is Vector, maxCorner is Vector) returns Box3d
{
    return { 'minCorner' : minCorner, 'maxCorner' : maxCorner } as Box3d;
}

/**
 * Enlarge a bounding box.
 * @param bBox
 * @param absoluteValue {ValueWithUnits} : The absolute distance to move
 *     each face of the box.  The corners move sqrt(3) times as far.
 * @param factor {number} : The relative amount to expand the box, with
 *     `0` leaving it unchanged.
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

