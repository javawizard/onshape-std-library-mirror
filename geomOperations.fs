FeatureScript ✨; /* Automatically generated version */
export import(path : "onshape/std/geomUtils.fs", version : "");

//====================== operations ========================

export function opImportForeign(context is Context, id is Id, definition is map)
{
    return @opImportForeign(context, id, definition);
}

export function opDeleteBodies(context is Context, id is Id, definition is map)
{
    return @opDeleteBodies(context, id, definition);
}

export function opTransform(context is Context, id is Id, definition is map)
{
    return @opTransform(context, id, definition);
}

export function opBoolean(context is Context, id is Id, definition is map)
{
    return @opBoolean(context, id, definition);
}

export function opFillet(context is Context, id is Id, definition is map)
{
    return @opFillet(context, id, definition);
}

export function opChamfer(context is Context, id is Id, definition is map)
{
    return @opChamfer(context, id, definition);
}

export function opDraft(context is Context, id is Id, definition is map)
{
    return @opDraft(context, id, definition);
}

export function opExtrude(context is Context, id is Id, definition is map)
{
    return @opExtrude(context, id, definition);
}

export function opPattern(context is Context, id is Id, definition is map)
{
    return @opPattern(context, id, definition);
}

export function opPlane(context is Context, id is Id, definition is map)
{
    return @opPlane(context, id, definition);
}

export function opHelix(context is Context, id is Id, definition is map)
{
    return @opHelix(context, id, definition);
}

export function opRevolve(context is Context, id is Id, definition is map)
{
    return @opRevolve(context, id, definition);
}

export function opShell(context is Context, id is Id, definition is map)
{
    return @opShell(context, id, definition);
}

export function opSplitPart(context is Context, id is Id, definition is map)
{
    return @opSplitPart(context, id, definition);
}

export function opPoint(context is Context, id is Id, definition is map)
{
    return @opPoint(context, id, definition);
}

export function opSweep(context is Context, id is Id, definition is map)
{
    return @opSweep(context, id, definition);
}

export function opDeleteFace(context is Context, id is Id, definition is map)
{
    return @opDeleteFace(context, id, definition);
}

export function opMoveFace(context is Context, id is Id, definition is map)
{
    return @opMoveFace(context, id, definition);
}

export function opOffsetFace(context is Context, id is Id, definition is map)
{
    return @opOffsetFace(context, id, definition);
}

export function opReplaceFace(context is Context, id is Id, definition is map)
{
    return @opReplaceFace(context, id, definition);
}

export function opModifyFillet(context is Context, id is Id, definition is map)
{
    return @opModifyFillet(context, id, definition);
}

export function opMateConnector(context is Context, id is Id, definition is map)
{
    return @opMateConnector(context, id, definition);
}

export function opThicken(context is Context, id is Id, definition is map)
{
    return @opThicken(context, id, definition);
}

export function opLoft(context is Context, id is Id, definition is map)
{
    return @opLoft(context, id, definition);
}

//======================================== enums used in operation definition =============
// TODO: these should be generated
// boolean
export enum BooleanOperationType
{
    annotation { "Name" : "Union" }
    UNION,
    annotation { "Name" : "Subtract" }
    SUBTRACTION,
    annotation { "Name" : "Intersect" }
    INTERSECTION,
    annotation { "Hidden" : true }
    SUBTRACT_COMPLEMENT
}

// extrude
export enum BoundingType
{
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Symmetric" }
    SYMMETRIC,
    annotation { "Name" : "Up to next" }
    UP_TO_NEXT,
    annotation { "Name" : "Up to surface" }
    UP_TO_SURFACE,
    annotation { "Name" : "Up to part" }
    UP_TO_BODY,
    annotation { "Name" : "Through all" }
    THROUGH_ALL
}
// extrude
export enum SecondDirectionBoundingType
{
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Up to next" }
    UP_TO_NEXT,
    annotation { "Name" : "Up to surface" }
    UP_TO_SURFACE,
    annotation { "Name" : "Up to part" }
    UP_TO_BODY,
    annotation { "Name" : "Through all" }
    THROUGH_ALL
}

// chamfer
export enum ChamferType
{
    annotation { "Name" : "Equal distance" }
    EQUAL_OFFSETS,
    annotation { "Name" : "Two distances" }
    TWO_OFFSETS,
    annotation { "Name" : "Distance and angle" }
    OFFSET_ANGLE
}

// constraintFace
export enum ConstraintFaceType
{
    annotation { "Name" : "Coincident" }
    COINCIDENT,
    annotation { "Name" : "Concentric" }
    CONCENTRIC,
    annotation { "Name" : "Equal radius" }
    EQUAL_RADIUS,
    annotation { "Name" : "Parallel" }
    PARALLEL,
    annotation { "Name" : "Perpendicular" }
    PERPENDICULAR
}

// cplane
 export enum CPlaneType
{
    annotation { "Name" : "Offset" }
    OFFSET,
    annotation { "Name" : "Plane Point" }
    PLANE_POINT,
    annotation { "Name" : "Line Angle" }
    LINE_ANGLE,
    annotation { "Name" : "Line Point" }
    LINE_POINT,
    annotation { "Name" : "Three Point" }
    THREE_POINT,
    annotation { "Name" : "Mid Plane" }
    MID_PLANE,
    annotation { "Name" : "Curve Point" }
    CURVE_POINT
}

 // helix
 export enum HelixType
{
    annotation { "Name" : "Turns" }
    TURNS,
    annotation { "Name" : "Pitch" }
    PITCH
}

export enum Direction
{
    annotation { "Name" : "Clockwise" }
    CW,
    annotation { "Name" : "Counterclockwise" }
    CCW
}

// moveFace
export enum MoveFaceType
{
    annotation { "Name" : "Translate" }
    TRANSLATE,
    annotation { "Name" : "Rotate" }
    ROTATE,
    annotation { "Name" : "Offset" }
    OFFSET
}

