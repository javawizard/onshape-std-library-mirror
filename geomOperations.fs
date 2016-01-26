FeatureScript 293; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/**
 * Operations are the basic modeling primitives of FeatureScript. Operations can do extrusion, filleting, transforms,
 * etc. An operation takes a context, an id, and a definition and modifies the context in accordance to the
 * definition. The modifications can be referenced by the passed-in id. Operations can fail by throwing an error or
 * they can report warnings or infos. The status can be read with the getFeature... functions in error.fs.
 *
 * When an operation parameter that requires one entity receives a query that resolves to multiple entities, it takes
 * the first resolved entity.
 *
 * This file contains wrappers around builtin operations and no actual logic.
 */
import(path : "onshape/std/context.fs", version : "");

/**
 * Performs boolean operations on solid bodies.
 * @param id : @autocomplete `id + "boolean1"`
 * @param definition {{
 *      @field tools {Query} : The tool bodies.
 *      @field targets {Query} : The target bodies. Required if the operation is `SUBTRACTION` or `SUBTRACT_COMPLEMENT` or
 *          if `targetsAndToolsNeedGrouping` is true. Otherwise ignored.
 *      @field operationType {BooleanOperationType} : The boolean operation to perform.
 *          @eg `BooleanOperationType.UNION` will merge any tool bodies that intersect or abut. When several bodies merge, the first one inherits
 *              the identity.
 *          @eg `BooleanOperationType.SUBTRACTION` will remove the union of all tools bodies from every target body.
 *          @eg `BooleanOperationType.INTERSECTION` will create the intersection of all tool bodies.
 *          @eg `BooleanOperationType.SUBTRACT_COMPLEMENT` will remove the complement of the union of all tool bodies from every target body.
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
 * @param id : @autocomplete `id + "chamfer1"`
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
 * @param id : @autocomplete `id + "deleteBodies1"`
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
 * @param id : @autocomplete `id + "deleteFace1"`
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
 * @param id : @autocomplete `id + "draft1"`
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
 * @param id : @autocomplete `id + "extrude1"`
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
 * @param id : @autocomplete `id + "fillet1"`
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
 * @param id : @autocomplete `id + "fitSpline1"`
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
 * @param id : @autocomplete `id + "helix1"`
 * @param definition {{
 *      @field direction {Vector} : The direction of the helix axis.
 *      @field axisStart {Vector} : A point on the helix axis.
 *      @field startPoint {Vector} : The start point of the infinite helix.  Must be off the axis.  This is the point at
 *          which the created curve would actually start if the first number of `interval` is 0.
 *      @field interval {Vector} : An array of two numbers denoting the interval of the helix in terms of revolution counts.
 *      @field clockwise {boolean} : True if this is a clockwise helix when viewed along `direction`.
 *      @field helicalPitch {ValueWithUnits} : Distance along the axis between successive revolutions.
 *          @eg `0 * inch` produces a planar Archimedean spiral.
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
 * @param id : @autocomplete `id + "importForeign1"`
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
 * Creates a surface or solid loft fitting an ordered set of profiles, optionally constrained by guide curves.
 * @param id : @autocomplete `id + "loft1"`
 * @param definition {{
 *      @field profileSubqueries {array} : An ordered array of queries for the profiles. For a solid loft, these must be
 *              surface bodies, faces, or vertices. For a surface loft, these could be wire bodies, surface bodies, faces, edges, or vertices.
 *      @field guideSubqueries {array} : An array of queries for guide curves. Each guide curve should intersect each profile once. @optional
 *      @field vertices {Query} : An array of vertices, one per profile, used in alignment of profiles. @optional
 *      @field makePeriodic {boolean} : Defaults to false. A closed guide creates a periodic loft regardless of this option. @optional
 *      @field bodyType {ToolBodyType} : Whether this is a solid (default) or a surface loft. @optional
 *      @field derivativeInfo {array} :  @optional An array of maps that contain shape constraints at start and end profiles. Each map entry
 *              is required to have a profileIndex that refers to the affected profile. Optional fields include a vector to match surface tangent to,
 *              a magnitude, and booleans for matching tangents or curvature derived from faces adjacent to affected profile.
 *              @eg `[ { "profileIndex" : 0, "vector" : vector(1, 0, 0), "magnitude" : 2., "tangentToPlane" : true}, { "profileIndex" : 1, "matchCurvature" : true, "adjacentFaces" : qFaces }]`
 *              The first map would constrain the resulting loft at the start profile to be tangent to plane with normal vector(1,0,0) and magnitude 2.
 *              The second map constrains the loft at the end profile to match the curvature of faces defined by the query qFaces.
 * }}
 */
export function opLoft(context is Context, id is Id, definition is map)
{
    return @opLoft(context, id, definition);
}

/**
 * Creates a mate connector, which represents a coordinate system in the context. Currently it is a special type of
 * point body.
 * @param id : @autocomplete `id + "mateConnector1"`
 * @param definition {{
 *      @field coordSystem {CoordSystem} : The mate connector coordinate system.
 *      @field owner {Query} : The owner body of the mate connector: when the owner is brought into an assembly, owned
 *          mate connectors will be brought in and move rigidly with it.  If the query resolves to multiple bodies, the
 *          first is taken as the owner.
 * }}
 */
export function opMateConnector(context is Context, id is Id, definition is map)
{
    return @opMateConnector(context, id, definition);
}

/**
 * Bring all of the information from `contextFrom` into `context`.  This is used, for example, for the Derived feature.
 * @param context {Context} : The target context.
 * @param id : @autocomplete `id + "mergeContexts1"`
 * @param definition {{
 *      @field contextFrom {Context} : The source context. It is rendered unusable by this operation.
 * }}
 */
export function opMergeContexts(context is Context, id is Id, definition is map)
{
    return @opMergeContexts(context, id, definition);
}

/**
 * This is a direct editing operation that modifies or deletes fillets.
 * @param id : @autocomplete `id + "modifyFillet1"`
 * @param definition {{
 *      @field faces {Query} : The fillets to modify.
 *      @field modifyFilletType {ModifyFilletType} : Whether to change the fillet radii or remove them altogether.
 *      @field radius {ValueWithUnits} : If `modifyFilletType` is `CHANGE_RADIUS`, the new radius.
 *      @field reFillet {boolean} : If `modifyFilletType` is `CHANGE_RADIUS` whether to reapply adjacent fillets.
 *          Defaults to false. @optional
 * }}
 */
export function opModifyFillet(context is Context, id is Id, definition is map)
{
    return @opModifyFillet(context, id, definition);
}

/**
 * This is a direct editing operation that applies a transform to one or more faces.
 * @param id : @autocomplete `id + "moveFace1"`
 * @param definition {{
 *      @field moveFaces {Query} : The faces to transform.
 *      @field transform {Transform} : The transform to apply.
 *      @field reFillet {boolean} : If true, attempt defillet `moveFaces` prior to the move and reapply the fillet
 *          after. Defaults to false. @optional
 * }}
 */
export function opMoveFace(context is Context, id is Id, definition is map)
{
    return @opMoveFace(context, id, definition);
}

/**
 * This is a direct editing operation that offsets one or more faces.
 * @param id : @autocomplete `id + "offsetFace1"`
 * @param definition {{
 *      @field moveFaces {Query} : The faces to offset.
 *      @field offsetDistance {ValueWithUnits} : The positive or negative distance by which to offset.
 *      @field reFillet {boolean} : If true, attempt defillet `moveFaces` prior to the offset and reapply the fillet
 *          after. Defaults to false. @optional
 * }}
 */
export function opOffsetFace(context is Context, id is Id, definition is map)
{
    return @opOffsetFace(context, id, definition);
}

/**
 * Patterns input faces and/or bodies by applying transforms to them. The original faces and bodies are preserved.
 * @param id : @autocomplete `id + "pattern1"`
 * @param definition {{
 *      @field entities {Query} : Bodies and faces to pattern.
 *      @field transforms {array} : An array of `transforms` to apply to `entities`. The transforms do not have to be
 *          rigid.
 *      @field instanceNames {array} : An array of distinct non-empty strings the same size as `transforms` to identify
 *          the patterned entities. TODO: make it easy to query for them.
 * }}
 */
export function opPattern(context is Context, id is Id, definition is map)
{
    return @opPattern(context, id, definition);
}

/**
 * Creates a construction plane.
 * @param id : @autocomplete `id + "plane1"`
 * @param definition {{
 *      @field plane {Plane} : The plane to create.
 *      @field size {ValueWithUnits} : The side length of the construction plane, as it is initially displayed.
 *      @field defaultType {DefaultPlaneType} : For Onshape internal use. @optional
 * }}
 */
export function opPlane(context is Context, id is Id, definition is map)
{
    return @opPlane(context, id, definition);
}

/**
 * Creates a construction point (a `BodyType.POINT` with one vertex). TODO: doesn't seem to display if `origin` is
 * false.
 * @param id : @autocomplete `id + "point1"`
 * @param definition {{
 *      @field point {Vector} : The location of the point. Has length units.
 *      @field origin {boolean} : For Onshape internal use. @optional
 * }}
 */
export function opPoint(context is Context, id is Id, definition is map)
{
    return @opPoint(context, id, definition);
}

/**
 * This is a direct editing operation that replaces the geometry one or more faces with that of another face, possibly
 * with an offset.
 * @param id : @autocomplete `id + "replaceFace1"`
 * @param definition {{
 *      @field replaceFaces {Query} : The faces whose geometry to replace.
 *      @field templateFace {Query} : The face whose geometry to use as the replacement.
 *      @field offset {ValueWithUnits} : The positive or negative distance by which to offset the `templateFace`. @optional
 *      @field oppositeSense {boolean} : If true, flip the normal of the templateFace. Default is false. In many cases,
 *          only one of these settings will work. @optional
 * }}
 */
export function opReplaceFace(context is Context, id is Id, definition is map)
{
    return @opReplaceFace(context, id, definition);
}

/**
 * Revolves edges and faces about an axis to produce sheet and solid bodies. The edges and faces may abut, but not
 * strictly intersect the axis.
 * @param id : @autocomplete `id + "revolve1"`
 * @param definition {{
 *      @field entities {Query} : The edges and faces to revolve.
 *      @field axis {Line} : The axis around which to revolve.
 *      @field angleForward {ValueWithUnits} : The angle where the revolve ends relative to `entities`. Normalized to the range \[0, 2 PI).
 *      @field angleBack {ValueWithUnits} : The angle where the revolve starts relative to `entities`. Normalized to the range \[0, 2 PI).
 *          If `angleForward == angleBack`, the revolve is a full (360-degree) revolve. Defaults to 0. @optional
 * }}
 */
export function opRevolve(context is Context, id is Id, definition is map)
{
    return @opRevolve(context, id, definition);
}

/**
 * Shell solid bodies. The bodies that are passed in are hollowed. The faces passed in are removed in order to hollow
 * their bodies.
 * @param id : @autocomplete `id + "shell1"`
 * @param definition {{
 *      @field entities {Query} : The faces to shell and solid bodies to hollow.
 *      @field thickness {ValueWithUnits} : The distance by which to shell. Positive means shell outward, and negative
 *          means shell inward.
 * }}
 */
export function opShell(context is Context, id is Id, definition is map)
{
    return @opShell(context, id, definition);
}

/**
 * Split solid and sheet bodies with the given sheet body.
 * @param id : @autocomplete `id + "splitPart1"`
 * @param definition {{
 *      @field targets {Query} : The solid and sheet bodies to split. TODO: why not wires?
 *      @field tool {Query} : The sheet body or construction plane to cut with.
 *      @field keepTools {boolean} : If false (default), the tool is deleted. @optional
 * }}
 */
export function opSplitPart(context is Context, id is Id, definition is map)
{
    return @opSplitPart(context, id, definition);
}

/**
 * Split faces with the given edges or faces.
 * @param id : @autocomplete `id + "splitFace1"`
 * @param definition {{
 *      @field faceTargets {Query} : The faces to split.
 *      @field edgeTools {Query} : The edges to cut with. @optional
 *      @field direction {Vector} : The projection direction. It has to be set when there are edge tools. @optional
 *      @field bodyTools {Query} : The bodies to cut with. @optional
 *      @field planeTools {Query} : The planes to cut with. @optional
 * }}
 */
export function opSplitFace(context is Context, id is Id, definition is map)
{
    return @opSplitFace(context, id, definition);
}

/**
 * Sweep the given edges and faces along a path resulting in sheet and solid bodies.
 * @param id : @autocomplete `id + "sweep1"`
 * @param definition {{
 *      @field profiles {Query} : Edges and faces to sweep.
 *      @field path {Query} : Edges that comprise the path along which to sweep. The edges can be in any order but
 *          must form a connected path.
 *      @field keepProfileOrientation {boolean} : If true, the profile maintains its original orientation as it is
 *          swept. If false (default), the profile rotates to remain normal to the path. @optional
 * }}
 */
export function opSweep(context is Context, id is Id, definition is map)
{
    return @opSweep(context, id, definition);
}

/**
 * Thicken sheet bodies and faces into solid bodies.
 * @param id : @autocomplete `id + "thicken1"`
 * @param definition {{
 *      @field entities {Query} : The sheet bodies and faces to thicken.
 *      @field thickness1 {ValueWithUnits} : The distance by which to thicken in the direction along the normal. @autocomplete `0.1 * inch`
 *      @field thickness2 {ValueWithUnits} : The distance by which to thicken in the opposite direction. @autocomplete `0.1 * inch`
 * }}
 */
export function opThicken(context is Context, id is Id, definition is map)
{
    return @opThicken(context, id, definition);
}

/**
 * Applies a given transform to one or more bodies. To make transformed copies of bodies, use `opPattern`.
 * @param id : @autocomplete `id + "transform1"`
 * @param definition {{
 *      @field bodies {Query} : The bodies to transform.
 *      @field transform {Transform} : The transform to apply. Need not be rigid.
 * }}
 */
export function opTransform(context is Context, id is Id, definition is map)
{
    return @opTransform(context, id, definition);
}

