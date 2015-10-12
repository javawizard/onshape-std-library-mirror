FeatureScript 236; /* Automatically generated version */
/**
 * Operations are the basic modeling primitives of FeatureScript. Operations can do extrusion, filleting, transforms,
 * etc. An operation takes a context, an id, and a definition and modifies the context in accordance to the
 * definition. The modifications can be referenced by the passed-in id. Operations can fail by throwing an error or
 * they can report warnings or infos. The status can be read with the getFeature... functions in error.fs.
 *
 * This file contains wrappers around builtin operations and no actual logic.
 */
import(path : "onshape/std/context.fs", version : "");

/**
 * Performs boolean operations on solid bodies.
 * @param definition {{
 *      @field tools {Query} : The tool bodies.
 *      @field targets {Query} : The target bodies. Required if the operation is `SUBTRACTION` or `SUBTRACT_COMPLEMENT` or
 *          if `targetsAndToolsNeedGrouping` is true. Otherwise ignored.
 *      @field operationType {BooleanOperationType} : The boolean operation to perform.
 *          @eg `BooleanOperationType.UNION` will merge any tool bodies that intersect or abut. When several bodies merge, the first one inherits
 *              the identity.
 *          @eg `BooleanOperationType.SUBTRACTION` will remove the union of all target bodies from every tool body.
 *          @eg `BooleanOperationType.INTERSECTION` will create the intersection of all tool bodies.
 *          @eg `BooleanOperationType.SUBTRACT_COMPLEMENT` will remove the complement of the union of all target bodies from every tool body.
 *      @field targetsAndToolsNeedGrouping {boolean} : This option is for adjusting the behavior to be more suitable for doing the boolean
 *          as part of a body-creating feature (such as extrude). Default is false.  If it is set to true, the changes are as follows:
 *
 *          For `BooleanOperationType.UNION`, unions together connected components of the bipartite tool/target intersection graph. In other words,
 *          if two tool bodies intersect each other, they may not end up unioned together if the targets they intersect are disjoint.
 *          TODO: this may not actually be accurate.
 *          @optional
 *
 *      @field keepTools {boolean} : If true, the tools do not get consumed by the operation. Default is false. @optional
 * }}
 */
export function opBoolean(context is Context, id is Id, definition is map)
{
    return @opBoolean(context, id, definition);
}

/**
 * Adds a chamfer to given edges and faces.  TODO: make this interface more like an operation and less like a feature.
 * @param definition {{
 *      @field entities {Query} : Edges and faces to chamfer.
 *      @field chamferType {ChamferType} : Determines how the offsets are specified.
 *      @field width {ValueWithUnits} : If `chamferType` is `EQUAL_OFFSETS` or `OFFSET_ANGLE`.
 *      @field width1 {ValueWithUnits} : If `chamferType` is `TWO_OFFSETS`.
 *      @field width2 {ValueWithUnits} : If `chamferType` is `TWO_OFFSETS`.
 *      @field angle {ValueWithUnits} : If `chamferType` is `OFFSET_ANGLE`.
 *      @field oppositeDirection {boolean} : If `chamferType` is `OFFSET_ANGLE` or `TWO_OFFSETS`.
 *      @field tangentPropagation {boolean} : Whether to propagate the chamfer along edges tangent to those passed in. Defaults to false. @optional
 * }}
 */
export function opChamfer(context is Context, id is Id, definition is map)
{
    return @opChamfer(context, id, definition);
}

/**
 * Deletes bodies from the context.
 * @param definition {{
 *      @field entities {Query} : Entities to delete. Passing in entities other than bodies deletes their owning bodies.
 * }}
 */
export function opDeleteBodies(context is Context, id is Id, definition is map)
{
    return @opDeleteBodies(context, id, definition);
}

/**
 * This is a direct editing operation that attempts to delete faces of a solid body and extend other faces to fill the hole.
 * @param definition {{
 *      @field deleteFaces {Query} : Faces to delete.
 *      @field includeFillet {boolean} : If true, also delete fillets adjacent to the input faces.
 *      @field capVoid {boolean} : TODO
 * }}
 */
export function opDeleteFace(context is Context, id is Id, definition is map)
{
    return @opDeleteFace(context, id, definition);
}

/**
 * Applies a given draft angle to faces.
 * @param definition {{
 *      @field neutralPlane {Query} : The face defining the neutral plane.  The intersection of the drafted faces
 *          and the neutral plane remains unchanged.
 *      @field pullVec {Vector} : The 3d direction relative to which the draft is applied.
 *      @field draftFaces {Query} : The faces to draft.
 *      @field angle {ValueWithUnits} : The draft angle, must be between 0 and 89 degrees.
 *      @field tangentPropagation {boolean} : If true, propagate draft across tangent faces. Defaults to false. @optional
 *      @field reFillet {boolean} : If true, attempt to defillet draft faces before the draft and reapply the fillets
 *          after. Defaults to false. TODO: Verify that this works right... @optional
 * }}
 */
export function opDraft(context is Context, id is Id, definition is map)
{
    return @opDraft(context, id, definition);
}

/**
 * Extrudes one or more edges or faces in a given direction with one or two end conditions.
 * @param definition {{
 *      @field entities {Query} : Edges and faces to extrude.
 *      @field direction {Vector} : The 3d direction in which to extrude.
 *      @field endBound {BoundingType} : The type of bound of the end of the extrusion.  Cannot be `SYMMETRIC`.
 *      @field endDepth {ValueWithUnits} : When `endBound` is `BLIND`, how far from the `entities` to extrude.
 *      @field endBoundEntity {Query} : When `endBound` is `UP_TO_SURFACE` or `UP_TO_BODY`, the face or body that provides the bound.
 *      @field startBound {BoundingType} : The type of start bound. Default is for the extrude to start at `entities`. Cannot be `SYMMETRIC`. @optional
 *      @field startDepth {ValueWithUnits} : When `startBound` is `BLIND`, how far from the `entities` to start the extrude.
 *      @field startBoundEntity {Query} : When `startBound` is `UP_TO_SURFACE` or `UP_TO_BODY`, the face or body that provides the bound.
 * }}
 */
export function opExtrude(context is Context, id is Id, definition is map)
{
    return @opExtrude(context, id, definition);
}

/**
 * For edges, performs a fillet on the edge. For faces, performs a fillet on all edges adjacent to the face.
 * @param definition {{
 *      @field entities {Query} : Edges and faces to fillet.
 *      @field radius {ValueWithUnits} : The fillet radius.
 *      @field tangentPropagation {boolean} : Whether to propagate the fillet along edges tangent to those passed in. Defaults to false. @optional
 *      @field conicFillet {boolean} : If true, the fillet is conic, rather than rolling ball. Defaults to false. @optional
 *      @field rho {number} : For conic fillets, a number between 0 and 1.  Rho close to zero makes the fillet behave
 *          like a chamfer, while rho close to 1 makes the fillet sharper.
 * }}
 */
export function opFillet(context is Context, id is Id, definition is map)
{
    return @opFillet(context, id, definition);
}

/**
 * Creates a 3D cubic spline curve through an array of 3D points.
 * @param definition {{
 *      @field points {array} : An array of `Vector`s with length units for the spline to interpolate. If the first
 *          point is the same as the last point, the spline is closed.
 * }}
 */
export function opFitSpline(context is Context, id is Id, definition is map)
{
    return @opFitSpline(context, id, definition);
}

/**
 * Creates a helical and possibly spiral curve.
 * @param definition {{
 *      @field direction {Vector} : The direction of the helix axis.
 *      @field axisStart {Vector} : A point on the helix axis.
 *      @field startPoint {Vector} : The start point of the infinite helix.  Must be off the axis.  This is the point at
 *          which the created curve would actually start if the first number of `interval` is 0.
 *      @field interval {Vector} : An array of two numbers denoting the interval of the helix in terms of revolution counts.
 *      @field clockwise {boolean} : True if this is a clockwise helix when viewed along `direction`.
 *      @field helicalPitch {ValueWithUnits} : Distance along the axis between successive revolutions.
 *          @eg `0 * inch` produces a planar spiral.
 *      @field spiralPitch {ValueWithUnits} : Change in radius between successive revolutions.
 *          @eg `0 * inch` produces a helix that lies on a cylinder.
 * }}
 */
export function opHelix(context is Context, id is Id, definition is map)
{
    return @opHelix(context, id, definition);
}

/**
 * Brings foreign geometry into the context. This function is used for importing uploaded parts.
 * @param definition {{
 *      @field foreignId {string} : The foreign data id (dataId from an imported blob tab).
 *      @field flatten {boolean} : Whether to flatten assemblies; defaults to false. @optional
 *      @field yAxisIsUp {boolean} : If true, the y axis in the import maps to the z axis and z maps to -y.
            If false (default), the coordinates are unchanged. @optional
 * }}
 */
export function opImportForeign(context is Context, id is Id, definition is map)
{
    return @opImportForeign(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opLoft(context is Context, id is Id, definition is map)
{
    return @opLoft(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opMateConnector(context is Context, id is Id, definition is map)
{
    return @opMateConnector(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opMergeContexts(context is Context, id is Id, definition is map)
{
    return @opMergeContexts(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opModifyFillet(context is Context, id is Id, definition is map)
{
    return @opModifyFillet(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opMoveFace(context is Context, id is Id, definition is map)
{
    return @opMoveFace(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opOffsetFace(context is Context, id is Id, definition is map)
{
    return @opOffsetFace(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opPattern(context is Context, id is Id, definition is map)
{
    return @opPattern(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opPlane(context is Context, id is Id, definition is map)
{
    return @opPlane(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opPoint(context is Context, id is Id, definition is map)
{
    return @opPoint(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opReplaceFace(context is Context, id is Id, definition is map)
{
    return @opReplaceFace(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opRevolve(context is Context, id is Id, definition is map)
{
    return @opRevolve(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opShell(context is Context, id is Id, definition is map)
{
    return @opShell(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opSplitPart(context is Context, id is Id, definition is map)
{
    return @opSplitPart(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opSweep(context is Context, id is Id, definition is map)
{
    return @opSweep(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opThicken(context is Context, id is Id, definition is map)
{
    return @opThicken(context, id, definition);
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
 * }}
 */
export function opTransform(context is Context, id is Id, definition is map)
{
    return @opTransform(context, id, definition);
}

