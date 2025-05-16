FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/**
 * Operations are the basic modeling primitives of FeatureScript. Operations can do extrusion, filleting, transforms,
 * etc. An operation takes a context, an id, and a definition and modifies the context in accordance with the
 * definition. The modifications can be referenced by the passed-in id. Operations return `undefined` but can fail by
 * throwing an error or they can report warnings or infos. The status can be read with the getFeature... functions in
 * error.fs.
 *
 * When an operation parameter that requires one entity receives a query that resolves to multiple entities, it takes
 * the first resolved entity.
 *
 * The geomOperations.fs module contains wrappers around built-in Onshape operations and no actual logic.
 */
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/context.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/query.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");

/* enumerations used by opBodyDraft */
export import(path : "onshape/std/bodydraftconcaverepairtype.gen.fs", version : "✨");
export import(path : "onshape/std/bodydraftcornertype.gen.fs", version : "✨");
export import(path : "onshape/std/bodydraftmatchfacetype.gen.fs", version : "✨");
export import(path : "onshape/std/bodydraftselectiontype.gen.fs", version : "✨");
/* opBoolean uses enumerations from TopologyMatchType */
export import(path : "onshape/std/topologymatchtype.gen.fs", version : "✨");
/* opCreateCurvesOnFace uses enumerations from FaceCurveCreationType */
export import(path : "onshape/std/facecurvecreationtype.gen.fs", version : "✨");
/* opDraft uses enumerations from DraftType */
export import(path : "onshape/std/drafttype.gen.fs", version : "✨");
/* opExtendSheet uses enumerations from ExtendSheetBoundingType */
export import(path : "onshape/std/extendsheetboundingtype.gen.fs", version : "✨");
/* opExtractSurface uses enumerations from ExtractSurfaceRedundancyType */
export import(path : "onshape/std/extractsurfaceredundancytype.gen.fs", version : "✨");
/* opExtrude uses enumerations from BoundingType */
export import(path : "onshape/std/boundingtype.gen.fs", version : "✨");
/* opFillet uses enumerations from FilletCrossSection */
export import(path : "onshape/std/filletcrosssection.gen.fs", version : "✨");
/* opFillSurface uses enumerations from GeometricContinuity */
export import(path : "onshape/std/geometriccontinuity.gen.fs", version : "✨");
/* opHole uses objects from holeUtils, as well as enums `export import`ed in that file */
export import(path : "onshape/std/holeUtils.fs", version : "✨");
/* opSplitPart uses enumerations from SplitOperationKeepType */
export import(path : "onshape/std/splitoperationkeeptype.gen.fs", version : "✨");
/* opWrap uses enumerations from WrapType */
export import(path : "onshape/std/wraptype.gen.fs", version : "✨");

/**
 * Trims or extends a wire body to an entity or by a distance.
 * @param id : @autocomplete `id + "moveCurveBoundary1"`
 * @param definition {{
 *      @field wires {Query} : The wire bodies to modify.
 *      @field moveBoundaryType {MoveCurveBoundaryType} : @optional Whether to trim or extend. Default is `TRIM`.
 *      @field trimTo {Query} : @requiredif {`moveBoundaryType` is `TRIM`.} Single entity to trim `wires` to.
 *      @field endCondition {CurveExtensionEndCondition} : @optional If `moveBoundaryType` is `EXTEND` defines
 *          whether to extend `wires` by a distance of to an entity. Default is `BLIND`.
 *      @field extensionDistance {ValueWithUnits} : @requiredIf {`endCondition` is `BLIND`} Distance to extend `wires`.
 *      @field extendTo {Query}: @requiredIf {`endCondition` is `BLIND`} Single entity to extend `wires` to.
 *      @field extensionShape {CurveExtensionShape} : @optional Specifies how to transition into the curve extensions. Default is `SOFT`.
 *      @field helpPoint {Query} : @requiredIf {`endCondition` is `BLIND`} Specifies vertex used to choose a solution.
 *          If this is not provided, the closest vertex to the bounding entity will be used.
 *      @field flipHeuristics {boolean} : @optional If `true`, will trim or extend from the opposite end of `wires`.
 *          Default is `false`.
 * }}
 */
export const opMoveCurveBoundary = function(context is Context, id is Id, definition is map)
{
    return @opMoveCurveBoundary(context, id, definition);
};

/**
  * Apply draft to a part by adding material. The material is added between reference edges and
  * a parting object. These reference edges can be supplied directly, or they can be inferred
  * from face or part queries.
  * @param id : @autocomplete `id + "bodyDraft1"`
  * @param definition {{
  *      @field selectionType {BodyDraftSelectionType} : @optional Topology class for input.
  * Default is `EDGES`
  *      @field topEdges {Query} : Edges to draft above parting entity. @requiredif {`selectionType` is `EDGES`}
  *      @field bottomEdges {Query} : Edges to draft with `angleBelow`.
  * @requiredif {`selectionType` is `EDGES` and `bothSides` is `true`}
  *      @field faces {Query} : Faces to draft. This will split the face with isoclines those for the draft.
  * Additionally, any existing edges bounding `faces` will be used if they mark a transition from steep faces to
  * non-steep faces for the given pull direction and draft angle. @requiredif {`selectionType` is `FACES`}
  *      @field bodies {Query} : Parts to draft. This is equivalent to supplying all faces of the part in `faces`.
  * @requiredif {`selectionType` is `PARTS`}
  *      @field excludeFaces {Query} : @optional Faces of `bodies` to exclude from drafting.
  *      @field angle {ValueWithUnits} : The draft angle above the parting object.
  *      @field bothSides {boolean} : @optional If `true`, draft on both sides of the parting object.
  * Default is `false`.
  *      @field pullDirection {ValueWithUnits} : The pull direction.
  *      @field draftOnSelf {boolean} : @optional If true, then using the drafted part as the
  * parting object. Default is `false`.
  *      @field partingObject : A surface from surfaceGeometry.fs or a query to a face or sheet body.
  * @requiredif {`draftOnSelf` is `false`}
  *      @field matchFacesAtParting {boolean} : @optional If true, then additional material will
  * be added to make the top and bottom of the draft align. Default is `false`.
  *      @field matchFaceType {BodyDraftMatchFaceType} : @optional How to add material for `matchFacesAtParting`
  * Default is `REFERENCE_EDGE`.
  *      @field cornerType {BodyDraftCornerType} : @optional The corner treatment to apply. Default is `EXTEND`.
  *      @field concaveRepair {BodyDraftConcaveRepairType} : @optional How to resolve intersecting draft
  * faces. Default is `NONE`.
  *      @field concaveRepairRadius {ValueWithUnits} : The radius for intersection repair.
  * @requiredif {`concaveRepair` is `RADIUS` or `MIX`.}
  *      @field keepMaterial {boolean} : @optional If true, an attempt will be made to keep the regions of the part
  * protruding from the tapered faces. Default is false.
  *      @field showRefs {boolean} : @optional If true, then debug data will be generated to
  * show the parting surface and draft edges.
  * }}
  */
export const opBodyDraft = function(context is Context, id is Id, definition is map)
{
    return @opBodyDraft(context, id, definition);
};

/**
 * Performs a boolean operation on multiple solid and surface bodies.
 * @seealso [processNewBodyIfNeeded] for merging new solids.
 * @seealso [joinSurfaceBodiesWithAutoMatching] for merging new surfaces.
 * @param id : @autocomplete `id + "boolean1"`
 * @param definition {{
 *      @field tools {Query} : The tool bodies.
 *      @field targets {Query} : @requiredif {`OperationType` is `SUBTRACTION` or `SUBTRACT_COMPLEMENT`, or
 *          if `targetsAndToolsNeedGrouping` is true.} The target bodies.
 *      @field operationType {BooleanOperationType} : The boolean operation to perform.
 *          @eg `BooleanOperationType.UNION` will merge any tool bodies that intersect or abut. All tool bodies have to be of
 *              the same type (solid or surface). When operating on surfaces, surfaces must have coincident or overlapping edges.
 *              When several bodies merge, the identity of the tool that appears earliest in the query is preserved
 *              (in particular, body color and body name are taken from it).
 *          @eg `BooleanOperationType.SUBTRACTION` will remove the union of all tools bodies from every target body.
 *              All tool bodies must be solid bodies. Target bodies could be either solids or surfaces.
 *          @eg `BooleanOperationType.INTERSECTION` will create the intersection of all tool bodies. All bodies must be solid bodies.
 *          @eg `BooleanOperationType.SUBTRACT_COMPLEMENT` will remove the complement of the union of all tool bodies from every target body.
 *              All tool bodies must be solid bodies. Target bodies could be either solids or surfaces.
 *      @field targetsAndToolsNeedGrouping {boolean} : @optional
 *              This option is for adjusting the behavior to be more suitable for doing the boolean
 *              as part of a body-creating feature (such as extrude). Default is `false`.
 *
 *      @field keepTools {boolean} : If true, the tools do not get consumed by the operation. Default is false. @optional
 *      @field makeSolid {boolean}: In case of surface union try to join surfaces into a solid. Default is false. @optional
 * }}
 */
/* TODO: describe `targetsAndToolsNeedGrouping` in fuller detail */
export const opBoolean = function(context is Context, id is Id, definition is map)
{
    return @opBoolean(context, id, definition);
};


/**
 * Creates a boundary surface fitting two ordered sets of profiles.
 * @param id : @autocomplete `id + "bsurf1"`
 * @param definition {{
 *      @field uProfileSubqueries {array} : An ordered array of two or fewer queries for the profiles in the u direction.
 *                  These can be edges or wire bodies.
 *              @eg `[ profileQuery1, profileQuery2 ]`
 *      @field vProfileSubqueries {array} : @optional An ordered array of two or fewer queries for the profiles in the v direction.
 *                  These can be edges or wire bodies.
 *              @eg `[ profileQuery1, profileQuery2 ]`
 *      @field uDerivativeInfo {array} :  @optional An array of maps that contain shape constraints at start and end profiles. Each map entry
 *              is required to have a profileIndex that refers to the affected profile. Optional fields include a vector to match surface tangent to,
 *              a magnitude, and booleans for matching tangents or curvature derived from faces adjacent to affected profile.
 *              @ex `[ { "profileIndex" : 0, "vector" : vector(1, 0, 0), "magnitude" : 2., "tangentToPlane" : true}, { "profileIndex" : 1, "adjacentFaces" : qFaces } ]`
 *              The first map would constrain the resulting boundary surface at the first u profile to be tangent to plane with normal vector(1,0,0) and magnitude 2.
 *              The second map constrains the boundary surface at the second u profile to match tangents of faces defined by the query qFaces.
 *      @field vDerivativeInfo {array} :  @optional An array of maps analogous to uDerivativeInfo, but for v profiles.
 *      @field showIsocurves {boolean} : Show graphical representation of a subset of isoparameteric curves on each face of the created boundary surface. Default `false`. @optional
 *      @field curveCount {number} : When `showIsocurves` is `true`, the number of curves to draw in each direction of each face's grid. Default `10`. @optional
 * }}
 */
export const opBoundarySurface = function(context is Context, id is Id, definition is map)
{
    return @opBoundarySurface(context, id, definition);
};

/**
 * Generates a wire body given a [BSplineCurve] definition.
 * The spline must have dimension of 3 and be G1-continuous.
 * @param id : @autocomplete `id + "bSplineCurve1"`
 * @param definition {{
 *      @field bSplineCurve {BSplineCurve} : The definition of the spline.
 * }}
 */
export const opCreateBSplineCurve = function(context is Context, id is Id, definition is map)
{
    return @opCreateBSplineCurve(context, id, definition);
};

/**
 * Generates a sheet body given a [BSplineSurface] definition.
 * The spline must have dimension of 3 and be G1-continuous.
 * @param id : @autocomplete `id + "bSplineSurface1"`
 * @param definition {{
 *      @field bSplineSurface {BSplineSurface} : The definition of the spline surface.
 *      @field boundaryBSplineCurves {array} : @optional An array of [BSplineCurve]s defined in the parameter space of
 *                  `bSplineSurface` to use as the boundary of the created sheet body.  The boundary must form a single
 *                  closed loop on the surface.  The `dimension` of each curve must be `2`, as the boundaries are being
 *                  defined in UV space of the created surface.  If no boundary is supplied, the created sheet body will
 *                  cover the full parameter range of `bSplineSurface`.
 *      @field makeSolid {boolean} : @optional When set to `true`, the operation will produce a solid instead of a sheet
 *                  if the surface encloses a region.  Default is `false`.
 * }}
 */
export const opCreateBSplineSurface = function(context is Context, id is Id, definition is map)
{
    return @opCreateBSplineSurface(context, id, definition);
};

/**
 * @internal
 * Generates surfaces representing the outlines of parts or surfaces projected onto a surface
 * @param id : @autocomplete `id + "createOutline1"`
 * @param definition {{
 *      @field tools {Query} : The tool parts or surfaces
 *      @field target {Query} : The face whose surface will be used to create outline.
 *              Currently only planes, cylinders or extruded surfaces are supported.
 *      @field offsetFaces {Query} : @optional Faces in tools which are offsets of target face. If `offsetFaces` are
 *              provided, the operation will fail if `target` is non-planar and there are not exactly two offset faces per tool.
 * }}
 */
export const opCreateOutline = function(context is Context, id is Id, definition is map)
{
    return @opCreateOutline(context, id, definition);
};

/**
 * Create a composite part.
 * @param id : @autocomplete `id + "compositePart1"`
 * @param definition {{
 *      @field bodies {Query} : Bodies from which to create the composite part.
 *.     @field closed {boolean} : @optional
 *              A `closed` composite part consumes its constituent bodies, so that they are not available interactively for individual selection. Default is `false`.
 * }}
 */
export const opCreateCompositePart = function(context is Context, id is Id, definition is map)
{
  return @opCreateCompositePart(context, id, definition);
};

/**
 * Modifies a composite part.
 * @param id : @autocomplete `id + "modifyCompositePart1"`
 * @param definition {{
 *      @field composite {Query} : Existing composite part to modify.
 *      @field toAdd {Query} : Bodies to add to the composite part.
 *      @field toRemove {Query} : Bodies to remove from the composite part.
 * }}
 */
export const opModifyCompositePart = function(context is Context, id is Id, definition is map)
{
    @opModifyCompositePart(context, id, definition);
};

/**
 * Adds a chamfer to given edges and faces.
 * @param id : @autocomplete `id + "chamfer1"`
 * @param definition {{
 *      @field entities {Query} : Edges and faces to chamfer.
 *      @field chamferType {ChamferType} : Determines where the new edges of the chamfer are positioned.
 *              @eg `ChamferType.EQUAL_OFFSETS` places both new edges `width` away from each original edge.
 *              @eg `ChamferType.TWO_OFFSETS` places edges `width1` and `width2` away.
 *              @eg `ChamferType.OFFSET_ANGLE` places one edge `width` away, and chamfers at an angle from that edge.
 *      @field width {ValueWithUnits} : @requiredif {`chamferType` is `EQUAL_OFFSETS` or `OFFSET_ANGLE`.}
 *              @eg `0.2 * inch`
 *      @field width1 {ValueWithUnits} : @requiredIf {`chamferType` is `TWO_OFFSETS`.}
 *      @field width2 {ValueWithUnits} : @requiredIf {`chamferType` is `TWO_OFFSETS`.}
 *      @field angle {ValueWithUnits} : @requiredIf {`chamferType` is `OFFSET_ANGLE`.}
 *      @field oppositeDirection {boolean} : @requiredIf {`chamferType` is `OFFSET_ANGLE` or `TWO_OFFSETS`.}
 *              `true` to reverse the two edges.
 *      @field tangentPropagation {boolean} : @optional
 *              `true` to propagate the chamfer along edges tangent to those passed in. Defaults to `false`.
 * }}
 */
/* TODO: make this interface more like an operation and less like a feature. */
export const opChamfer = function(context is Context, id is Id, definition is map)
{
    return @opChamfer(context, id, definition);
};

/**
 * Creates a constrained surface from the points.
 * @param id : @autocomplete `id + "constrainedSurface1"`
 * @param definition {{
 *      @field points {array} : An array of maps containing:
 *          `point` (Vector) : The position of the point we want the surface to pass through.
 *          `normal` (Vector) (Optional) : The direction of the normal of the surface at this point.
 *      @field tolerance {ValueWithUnits} : @optional The distance the surface is allowed to deviate from the given points.
 *                                      Default is `1e-6 meter`.
 *      @field smooth {boolean} : @optional If true, the surface will be optimized for smoothness. If false, it will be optimized for performance.
 *                                      Default is `false`.
 *      @field references {Query} : For Onshape internal use. @optional
 * }}
 */
export const opConstrainedSurface = function(context is Context, id is Id, definition is map)
{
    return @opConstrainedSurface(context, id, definition);
};

/**
 * Describes a set of isoparametric curves on a face.
 * @type {{
 *      @field face {Query} : Face the curves are meant to lie on.
 *      @field creationType {FaceCurveCreationType} : Determines the type of curves. Currently supports isoparameter curves only
 *              in primary or secondary directions, either equally spaced or defined by a parameter array.
 *      @field names {array} : An array of distinct non-empty strings to identify the curves created.
 *      @field nCurves {number} : @requiredif {`creationType` is `DIR1_AUTO_SPACED_ISO` or `DIR2_AUTO_SPACED_ISO`} Number of curves.
 *      @field parameters {array} : @requiredif {`creationType` is `DIR1_ISO` or `DIR2_ISO`} Parameters to create curves at.
 * }}
 */
export type CurveOnFaceDefinition typecheck canBeCurveOnFaceDefinition;
/** Typecheck for [CurveOnFaceDefinition] */
export predicate canBeCurveOnFaceDefinition(value)
{
    value is map;
    value.face is Query;
    value.creationType is FaceCurveCreationType;
    value.names is array;
    for (var name in value.names)
    {
        name is string;
    }

    value.nCurves is number || value.nCurves is undefined;
    if (value.nCurves is number)
    {
        value.nCurves == size(value.names);
    }
    value.parameters is array || value.parameters is undefined;
    if (value.parameters is array)
    {
        size(value.parameters) == size(value.names);
        for (var parameter in value.parameters)
        {
            parameter is number;
        }
    }
}

function curveOnFaceDefinitionPrivate(face is Query, creationType is FaceCurveCreationType, names is array, nCurves, parameters)
precondition
{
    nCurves is number || nCurves is undefined;
    parameters is array || parameters is undefined;
    if (parameters is array)
    {
        for (var parameter in parameters)
        {
            parameter is number;
        }
    }
}
{
    return {
        "face" : face,
        "creationType" : creationType,
        "names" : names,
        "nCurves" : nCurves,
        "parameters" : parameters
    } as CurveOnFaceDefinition;
}

/**
 * Returns a new [CurveOnFaceDefinition].
 */
export function curveOnFaceDefinition(face is Query, creationType is FaceCurveCreationType, names is array, nCurves is number) returns CurveOnFaceDefinition
{
    return curveOnFaceDefinitionPrivate(face, creationType, names, nCurves, undefined);
}

export function curveOnFaceDefinition(face is Query, creationType is FaceCurveCreationType, names is array, parameters is array) returns CurveOnFaceDefinition
{
    return curveOnFaceDefinitionPrivate(face, creationType, names, undefined, parameters);
}

/**
 * Generates isoparametric curves of faces. That is, for each specified surface parameter value, creates a new wire body
 * following the curve which keeps the surface parameter at that constant value.
 * @param id : @autocomplete `id + "curvesOnFace"`
 * @param definition {{
 *      @field curveDefinition {array} : An array of [CurveOnFaceDefinition]s that describe group of curves per face.
        @field showCurves {boolean} : Whether to display isoparameteric curves in color in the preview. @optional
        @field skipTrim {boolean} : Whether to skip trimming of curves by the face boundaries. Lines and arcs are always trimmed. @optional
        @field useFaceParameter {boolean} : For Onshape internal use. @optional
 * }}
 */
export const opCreateCurvesOnFace = function(context is Context, id is Id, definition is map)
{
    return @opCreateCurvesOnFace(context, id, definition);
};

/**
 * Deletes bodies from the context.
 * @param id : @autocomplete `id + "deleteBodies1"`
 * @param definition {{
 *      @field entities {Query} : Entities to delete. Passing in entities other than bodies deletes their owning bodies.
 * }}
 */
export const opDeleteBodies = function(context is Context, id is Id, definition is map)
{
    return @opDeleteBodies(context, id, definition);
};

/**
 * This is a direct editing operation that attempts to delete faces of a solid body and extend other faces to fill the hole.
 * @param id : @autocomplete `id + "deleteFace1"`
 * @param definition {{
 *      @field deleteFaces {Query} : Faces to delete.
 *      @field includeFillet {boolean} : `true` to also delete fillets adjacent to the input faces.
 *              @autocomplete `false`
 *      @field capVoid {boolean} : If `capVoid` is `true` and the deleted face
 *              cannot be filled by extending the surrounding faces, will
 *              attempt to replace the face with a planar face.
 *              @autocomplete `false`
 *      @field leaveOpen {boolean} : @optional
 *              If `leaveOpen` is `true` the void from deleting faces is left open, potentially creating a surface out
 *              of a solid body. Default is `false`.
 *              @autocomplete `false`
 * }}
 */
export const opDeleteFace = function(context is Context, id is Id, definition is map)
{
    return @opDeleteFace(context, id, definition);
};

/**
 * @internal
 * Takes in a set of bodies and faces and creates solid bodies for the enclosed regions.
 * @param id : @autocomplete `id + "enclose"`
 * @param definition {{
 *      @field entities {Query} : Bodies and faces for enclosure.
 * }}
 */
export const opEnclose = function(context is Context, id is Id, definition is map)
{
    return @opEnclose(context, id, definition);
};

/**
 * Applies a given draft angle to faces.
 * @param id : @autocomplete `id + "draft1"`
 * @param definition {{
 *      @field draftType {DraftType} :
 *              Specifies a reference surface (e.g. a neutral plane) or reference entity draft.
 *              @eg `DraftType.REFERENCE_SURFACE` for a reference surface draft
 *      @field draftFaces {Query} : @requiredif { `draftType` is `REFERENCE_SURFACE` }
 *              The faces to draft for a `REFERENCE_SURFACE` draft.
 *              @autocomplete `draftFaces`
 *      @field referenceSurface : @requiredif { `draftType` is `REFERENCE_SURFACE` }
 *              A face or plane that defines the neutral surface for a `REFERENCE_SURFACE` draft. `draftFaces` will remain unchanged
 *              where they intersect `referenceSurface`. Can be either a [Query] or a [Plane].
 *      @field referenceEntityDraftOptions {array} : @requiredif { `draftType` is `REFERENCE_ENTITY` }
 *              An array of maps of the form ("face", "references", "angle").  "face" should be a [Query] for exactly one
 *              face.  "references" should be a [Query] for at least one edge attached to the face.  The "face" will
 *              be drafted while the geometry of the "references" remains unchanged. "angle" is an optional [ValueWithUnits]
 *              parameter between -89.9 and 89.9 degrees which overrides the default `angle` parameter.
 *      @field pullVec {Vector} : The 3d direction relative to which the draft is applied.
 *              @eg `vector(0, 0, 1)`.
 *      @field angle {ValueWithUnits} : The draft angle, must be between 0 and 89.9 degrees.
 *              @eg `3 * degree`
 *      @field tangentPropagation {boolean} : @optional
 *              For a `REFERENCE_SURFACE` draft, `true` to propagate draft across tangent faces.
 *              Default is `false`.
 *      @field referenceEntityPropagation {boolean} : @optional
 *              For a `REFERENCE_ENTITY` draft, `true` to collect new reference entities and faces by pulling in edges
 *              connected to the specified reference edges.  Connected edges on the same face or on tangent connected
 *              faces will be pulled in.
 *              Default is `false`.
 *      @field reFillet {boolean} : @optional
 *              `true` to attempt to defillet draft faces before the draft and reapply the fillets
 *              after. Default is `false`.
 * }}
 */
export const opDraft = function(context is Context, id is Id, definition is map)
{
    return @opDraft(context, id, definition);
};

/**
 * @internal
 * @param id : @autocomplete `id + "changeEdge1"`
 * @param definition {{
 *    @field edgeChangeOptions {array} : An array of maps of the form ("edge", "face", "offset", "transformList", "replaceFace").
 *                                      Edge and face are required and are the edge and face being modified.
 *                                      The other parameters are optional. If offset is a length parameter, the edge will
 *                                      be offset. If transform list is an array of transforms, they will be applied to the edge.
 *                                      If replaceFace is a face, the edge will be moved to that face.
 * }}
 */
export const opEdgeChange = function(context is Context, id is Id, definition is map)
{
    return @opEdgeChange(context, id, definition);
};

/**
 * @internal
 * @param id : @autocomplete `id + "editCurve1"`
 * @param definition {{
 *      @field wire {Query} : The wire body to change.
 *      @field edge {Query} : The edge whose curve the wire body should match.
 * }}
 */
export const opEditCurve = function(context is Context, id is Id, definition is map)
{
    return @opEditCurve(context, id, definition);
};

/**
 * @internal
 * Extends or trims the perimeter of a sheet body by moving sheet edges by distance or up to a target sheet body
 * @param id : @autocomplete `id + "extendBody1"`
 * @param definition {{
 *    @field endCondition {ExtendEndType} : @autocomplete `ExtendEndType.EXTEND_BLIND`. Condition that terminates the extension. May be blind or up to target.
 *    @field entities {Query} : Bodies or edges to extend.
 *.   @field tangentPropagation {boolean} : Whether additional edges should be added to the selection by tangent propagation. Default `true`. @optional
 *    @field extendDistance {ValueWithUnits} : @requiredif{'endCondition' is 'ExtendEndType.EXTEND_BLIND'} The distance to extend by. Negative values may be used to trim.
 *                                       @autocomplete `0.1 * inch`
 *    @field target : @requiredif{'endCondition' is 'ExtendSheetBoundingType.EXTEND_TO_TARGET'} Target part to extend up to.
 *    @field extensionShape {ExtendSheetShapeType} : @autocomplete `ExtendSheetShapeType.LINEAR`. Shape characteristic of extension, whether curvature continuity is maintained or not.
 * }}
 */
export const opExtendSheetBody = function(context is Context, id is Id, definition is map)
{
    return @opExtendSheetBody(context, id, definition);
};

/**
 * @internal
 * This function takes a list of faces and creates one or more surfaces from those faces.
 * The source faces and body are not affected.
 * @param id : @autocomplete `id + "extractSurface1"`
 * @param definition {{
 *    @field faces {Query} : List of faces to be converted. If `tangentPropagation` is `true`, these are the seed faces.
 *    @field tangentPropagation {boolean} : Whether additional faces should be added to the selection by tangent propagation @optional
 *    @field offset {ValueWithUnits} : Offset extracted surface faces by this distance along normal @optional
 *    @field useFacesAroundToTrimOffset {boolean} : Use surrounding faces extensions to trim offset. Default `true`. @optional
 *    @field redundancyType {ExtractSurfaceRedundancyType} : @optional
 *              Controls the culling of redundant geometry on the result body, such as tangent edges and vertices.
 *              `ALLOW_REDUNDANCY` does not delete any redundant geometry. `REMOVE_ADDED_REDUNDANCY` removes redundancy
 *              created by this operation.  `REMOVE_ALL_REDUNDANCY` removes all redundant edges and vertices.
 *              `REMOVE_ADDED_REDUNDANCY` is the default.
 * }}
 */
export const opExtractSurface = function(context is Context, id is Id, definition is map)
{
    @opExtractSurface(context, id, definition);
};

/**
 * Generates wire bodies from the supplied edges.
 * If the edges are disjoint multiple wires will be returned.
 * If the edges overlap or cross, or more than two meet at a point, the function will fail.
 * @param id : @autocomplete `id + "opExtractWires1"`
 * @param definition {{
 *      @field edges {Query} : The edges to be extracted.
 * }}
 */
export const opExtractWires = function(context is Context, id is Id, definition is map)
{
    return @opExtractWires(context, id, definition);
};

/**
 * Extrudes one or more edges or faces in a given direction with one or two end conditions.
 * Faces get extruded into solid bodies and edges get extruded into sheet bodies.
 * @param id : @autocomplete `id + "extrude1"`
 * @param definition {{
 *      @field entities {Query} : Edges and faces to extrude.
 *      @field direction {Vector} : The 3d direction in which to extrude.
 *              @eg `evOwnerSketchPlane(context, {"entity" : entities}).normal` to extrude perpendicular to the owner sketch
 *              @eg `evPlane(context, {"face" : entities}).normal` to extrude perpendicular to the first planar entity
 *      @field endBound {BoundingType} : The type of bound at the end of the extrusion. Cannot be `SYMMETRIC` or `UP_TO_VERTEX`.
 *              @eg `BoundingType.BLIND`
 *      @field endDepth {ValueWithUnits} : @requiredif {`endBound` is `BLIND`.}
 *              How far from the `entities` to extrude.
 *              @eg `1 * inch`
 *      @field endBoundEntity {Query} : @requiredif {`endBound` is `UP_TO_SURFACE` or `UP_TO_BODY`.}
 *              The face or body that provides the bound.
 *      @field endTranslationalOffset {ValueWithUnits} : @optional
 *              The translational offset between the extrude end cap and the end bound entity. The direction vector for
 *              this is the same as `direction`: negative values pull the end cap away from the bound entity when
 *              `endDepth` is positive. `endOffset` will only have an effect if `endBound` is `UP_TO_SURFACE`,
 *              `UP_TO_BODY`, or `UP_TO_NEXT`.
 *      @field startBound {BoundingType} : @optional
 *              The type of start bound. Default is for the extrude to start at `entities`. Cannot be `SYMMETRIC` or `UP_TO_VERTEX`.
 *      @field isStartBoundOpposite : @requiredif {is `UP_TO_SURFACE`, `UP_TO_BODY`, or `UP_TO_NEXT`.}
 *              Whether the `startBound` extends in the opposite direction from the profile as the `endBound`. Defaults
 *              to `true` if not supplied.
 *      @field startDepth {ValueWithUnits} : @requiredif {`startBound` is `BLIND`.}
 *              How far from the `entities` to start the extrude.  The direction vector for this is the negative of `direction`:
 *              positive values make the extrusion longer when `endDepth` is positive.
 *      @field startBoundEntity {Query} : @requiredif {`startBound` is `UP_TO_SURFACE` or `UP_TO_BODY`.}
 *              The face or body that provides the bound.
 *      @field startTranslationalOffset {ValueWithUnits} : @optional
 *              The translational offset between the extrude start cap and the start bound entity. The direction vector for
 *              this is the negative of `direction`: negative values pull the end cap away from the bound entity when
 *              `startDepth` is positive. `startOffset` will only have an effect if `startBound` is `UP_TO_SURFACE`,
 *              `UP_TO_BODY`, or `UP_TO_NEXT`.
 * }}
 */
export const opExtrude = function(context is Context, id is Id, definition is map)
{
    return @opExtrude(context, id, definition);
};

/**
 * Performs a face blend between two walls of faces.
 * @param id : @autocomplete `id + "faceBlend1"`
 * @param definition {{
 *      @field side1 {Query} : First set of faces, must belong to the same body.
 *      @field side2 {Query} : Second set of faces, must belong to the same body.
 *      @field flipSide1Normal {boolean} : Wether to flip side 1's normal.
 *      @field flipSide2Normal {boolean} : Wether to flip side 2's normal.
 *      @field propagation {boolean} : @optional Whether to propagate the blend.
 *      @field propagationType {FaceBlendPropagation} : @optional How to propagate the blend.
 *             FaceBlendPropagation.TANGENT will propagate over tangent faces.
 *             FaceBlendPropagation.ADJACENT will propagate over all adjacent faces.
 *             FaceBlendPropagation.CUSTOM allows specification of a maximum angle between faces for propagation.
 *             Defaults is FaceBlendPropagation.TANGENT.
 *      @field propagationAngle {ValueWithUnits} : @optional Maximum angle between faces for propagation.
 *             Used if propagationType is FaceBlendPropagation.CUSTOM. Default is 0 * radian.
 *      @field crossSection {FaceBlendCrossSection} : The cross section type of the blend.
 *      @field spine {Query} : @requiredif {`crossSection` is `DISC`.}
 *             The spine of the blend. The blend's cross section will be orthogonal to the spine.
 *      @field blendControlType {BlendControlType} : Whether the blend is controled by a constant radius or a constant width.
 *      @field crossSectionShape {FaceBlendCrossSectionShape} : What shape the cross section of the blend will be.
 *      @field radius {ValueWithUnits} : @requiredif {`blendControlType` is `RADIUS`.} The radius of the cross section.
 *      @field width {ValueWithUnits} : @requiredif {`blendControlType` is `WIDTH`.} The width of the blend.
 *      @field asymmetric {boolean} : @optional Wether the radius or width is the same on each wall.
 *      @field secondRadius {ValueWithUnits} : @requiredif {`blendControlType` is `RADIUS` and `asymmetric` is `true`.}
 *             The radius of the cross section from side 2.
 *      @field widthRatio {number} : @requiredif {`blendControlType` is `WIDTH` and `asymmetric` is `true`.}
 *             How the blend will be divided between the walls. If widthRatio < 1, it will be wider towards side 1.
 *             If widthRatio > 1, it will be wider towards side 2.
 *      @field rho {number} : @requiredif {`crossSectionShape` is `CONIC`}
 *             Parameter of the conic cross section shape.
 *      @field magnitude {number} : @requiredif {`crossSectionShape` is `CURVATURE`}
 *             Parameter of the curvature cross section shape.
 *      @field tangentHoldLines {boolean} : @optional Whether to use the content of `tangentEdges` and `inverseTangentEdges`.
 *      @field tangentEdges {Query} : @optional Edges to use as tangent hold lines.
 *      @field inverseTangentEdges {Query} : @optional Edges to use as inverse tangent hold lines.
 *      @field conicHoldLines {boolean} : @optional Whether to use the content of `conicEdges` and `inverseConicEdges`.
 *      @field conicEdges {Query} : @optional Edges to use as conic hold lines.
 *      @field inverseConicEdges {Query} : @optional Edges to use as inverse conic hold lines.
 *      @field hasCliffEdges {boolean} : @optional Whether to use the content of `cliffEdges`.
 *      @field cliffEdges {Query} : @optional Edges to use as cliff edges.
 *      @field hasCaps {boolean} : @optional Whether to use the content of `caps`.
 *      @field caps {array} : @optional
 *             Entities and flip flags to use as caps. Each map should contain an `entity` Query element and a `flip` boolean element.
 *      @field hasLimits {boolean} : @optional Whether to use the content of `limitPlane1`, `limitPlane2`, `faceLimits` and `edgeLimit`.
 *      @field limitPlane1 {Query} : @optional First plane to limit the blend.
 *      @field limitPlane1Flip {boolean} : Whether to flip the first plane normal. Defines which side of the plane the blend will be.
 *      @field limitPlane2 {Query} : @optional Second plane to limit the blend.
 *      @field limitPlane2Flip {boolean} : Whether to flip the first plane normal. Defines which side of the plane the blend will be.
 *      @field faceLimits {Query} : @optional Faces to use as limits to the blend.
 *      @field edgeLimit {array} : @optional
 *             Edges and adjacent faces to use as limits. Each map should contain an `edge` Query element and an `edgeLimitSide` query element.
 *      @field hasHelpPoint {boolean} : @optional Whether to use the content of `helpPoint`.
 *      @field helpPoint {Query} : @optional Vertex or mate connector to use as help point. In case the blend parameters create several blends,
 *             only the blend closest to the help point will be kept.
 *      @field trim {FaceBlendTrimType} : @optional How to trim the blend.
 *      @field detach {boolean} : @optional Whether not to attach the blend(s) to the sides, creating new sheet bodies.
 *      @field showIsocurves {boolean} : Show graphical representation of a subset of isoparameteric curves of the created surface. Default `false`. @optional
 *      @field curveCount {number} : When `showIsocurves` is `true`, the number of curves to draw in each direction of the grid. Default `10`. @optional
 * }}
 */
export const opFaceBlend = function(context is Context, id is Id, definition is map)
{
    return @opFaceBlend(context, id, definition);
};

/**
 * For edges, performs a fillet on the edge. For faces, performs a fillet on all edges adjacent to the face.
 * @param id : @autocomplete `id + "fillet1"`
 * @param definition {{
 *      @field entities {Query} : Edges and faces to fillet.
 *      @field radius {ValueWithUnits} : The fillet radius.
 *              @eg `1 * inch`
 *      @field tangentPropagation {boolean} : @optional
 *              `true` to propagate the fillet along edges tangent to those passed in. Default is `false`.
 *      @field crossSection {FilletCrossSection} : @optional
 *              Fillet cross section. One of `CIRCULAR`, `CONIC`, `CURVATURE`. Default is `CIRCULAR`.
 *      @field rho {number} : @requiredif {`crossSection` is `CONIC`.}
 *              A number between 0 and 1, specifying the Rho value of a conic fillet
 *              @ex `0.01` creates a flat, nearly-chamfered shape.
 *              @ex `0.99` creates a pointed, nearly-unchanged shape.
 *      @field magnitude {number} : @requiredif {`crossSection` is `CURVATURE`.}
 *              A number between 0 and 1, specifying the magnitude of curvature match.
 *      @field partialFilletBounds {array} : @optional An array of maps representing the boundaries of a partial fillet. Each map should
 *              contain a `boundaryEdge` query, a `boundaryParameter` value, a `isFlipped` boolean
 *              @ex `[{ "boundaryEdge" : edgeQuery0, "boundaryParameter" : 0.3, "isFlipped" : false }, { "boundaryEdge" : edgeQuery1, "boundaryParameter" : 0.6, "isFlipped" : true }]`
 *      @field isVariable {boolean} : @optional Fillet controls can be varied at vertices via `vertexSettings`. Default is `false`.
 *      @field vertexSettings {array} : @optional An array of maps representing fillet settings at specified vertices.  Each map should
 *              contain a `vertex` query, a `vertexRadius` value, a `variableMagnitude` if the `crossSection` is
 *              `FilletCrossSection.CURVATURE`, and a `variableRho` if the `crossSection` is `FilletCrossSection.CONIC`.
 *              @ex `[{ "vertex" : vertexQuery0, "vertexRadius" : 1 * inch, "variableRho" : 0.2 }, { "vertex" : vertexQuery1, "vertexRadius" : 2 * inch, "variableRho" : 0.8 }]`
 *      @field pointOnEdgeSettings {array} : @optional An array of maps representing fillet settings at specified points on edges.  Each map should
 *              contain an `edge` query, an `edgeParameter` value, a `pointOnEdgeRadius` value, a `pointOnEdgeVariablMagnitude` if the `crossSection` is
 *              `FilletCrossSection.CURVATURE`, and a `pointOnEdgeVariableRho` if the `crossSection` is `FilletCrossSection.CONIC`.
 *              @ex `[{ "edge" : edgeQuery0, "edgeParameter" : 0.3, "pointOnEdgeRadius" : 1 * inch }, { "edge" : edgeQuery1, "edgeParameter" : 0.6, "pointOnEdgeRadius" : 2 * inch }]`
 *      @field smoothTransition {boolean} : @requiredif { `isVariable` is `true` }  Whether to create a smoother transition
 *              between each vertex.
 *      @field allowEdgeOverflow {boolean} : @optional Allow `opFillet` to modify nearby edges to maintain the fillet profile. Default is `true`.
 *      @field keepEdges {Query} : @optional Edges you do not want `opFillet` to modify if `allowEdgeOverflow` is `true`.
 *      @field smoothCorners {boolean} : @optional Allow `opFillet` to smooth all suitable corners and prevent creation of sharp edges. Default is `false`.
 *      @field smoothCornerExceptions {Query} : @optional Vertices you do not want `opFillet` to smooth if `smoothCorners` is `true`.
 *      @field createDetachedSurface {boolean} : @optional
 *              Operation does not modify the body of the selected edges, but results in surface geometry of fillet. Default is `false`.
 * }}
 */
export const opFillet = function(context is Context, id is Id, definition is map)
{
    return @opFillet(context, id, definition);
};

/**
 * Generates a surface body from supplied boundary and internal constraints. The boundaries are defined as
 * edge queries for each continuity constraint. The internal constraints may be defined as a set of support vertices.
 * @param id : @autocomplete `id + "opFillSurface1"`
 * @param definition {{
 *      @field edgesG0 {Query} : The edges with position constraints.
 *      @field edgesG1 {Query} : The edges with tangency constraints.
 *      @field edgesG2 {Query} : The edges with curvature constraints.
 *      @field guideVertices {Query} : The vertices the resulting surface is expected to interpolate.
 *      @field showIsocurves {boolean} : Show graphical representation of a subset of isoparameteric curves of the created surface. Default `false`. @optional
 *      @field curveCount {number} : When `showIsocurves` is `true`, the number of curves to draw in each direction of the grid. Default `10`. @optional
 * }}
 */
export const opFillSurface = function(context is Context, id is Id, definition is map)
{
    return @opFillSurface(context, id, definition);
};

/**
 * Creates a 3D cubic spline curve through an array of 3D points.
 * @param id : @autocomplete `id + "fitSpline1"`
 * @param definition {{
 *      @field points {array} : An array of `Vector`s with length units for the spline to interpolate. If the first
 *          point is the same as the last point, the spline is closed.
 *              @eg
 * ```
 * [
 *     vector( 1,  1,  1) * inch,
 *     vector(-1,  1, -1) * inch,
 *     vector( 1, -1, -1) * inch,
 *     vector(-1, -1,  1) * inch,
 *     vector( 1,  1,  1) * inch
 * ]
 * ```
 *      @field parameters {array} : An array of doubles, parameters corresponding to the points. @optional
 *      @field startDerivative {Vector} : A `Vector` with length units that specifies the derivative at the start of
 *          the resulting spline (according to the `arcLengthParameterization` set to `false`). @optional
 *      @field endDerivative {Vector} : A `Vector` with length units that specifies the derivative at the end of
 *          the resulting spline.  Ignored if spline is closed.  @optional
 *      @field start2ndDerivative {Vector} : A `Vector` with length units that specifies the second derivative at the start of
 *          the resulting spline.  Ignored if spline is closed, or if `startDerivative` is not defined @optional
 *      @field end2ndDerivative {Vector} : A `Vector` with length units that specifies the second derivative at the end of
 *          the resulting spline.  Ignored if spline is closed, or if `endDerivative` is not defined @optional
 *      @field derivatives {map} : A map of derivatives at non-end points.
 *           Entries should be `index : derivative`, where `index` is an integer between 1 and `size(points) - 2`
 *           and `derivative` is a `Vector` that specifies the derivative at `points[index]`. @optional
 * }}
 */
export const opFitSpline = function(context is Context, id is Id, definition is map)
{
    return @opFitSpline(context, id, definition);
};

/**
 * Reverses the orientation of sheet bodies.
 * @param id : @autocomplete `id + "flipOrientation"`
 * @param definition {{
 *      @field bodies {Query} : Sheet bodies whose orientation should be flipped.
 * }}
 */
export const opFlipOrientation = function(context is Context, id is Id, definition is map)
{
    return @opFlipOrientation(context, id, definition);
};

/**
 * Creates a full round fillet, replacing the center face(s) with circular profile face(s) of varying radius, joining the selected side faces.
 * @param id : @autocomplete `id + "fullRoundFillet1"`
 * @param definition {{
 *      @field side1Face {Query} : A face on one side of the blend.
 *      @field side2Face {Query} : A face on another side of the blend.
 *      @field centerFaces {Query} : The face(s) to be replaced.
 *      @field tangentPropagation {boolean} : @optional
 *              `true` to propagate the fillet across side face tangencies. Default is `true`.
 * }}
 */
export const opFullRoundFillet = function(context is Context, id is Id, definition is map)
{
    return @opFullRoundFillet(context, id, definition);
};

/**
 * Creates a helical and possibly spiral curve.
 * @param id : @autocomplete `id + "helix1"`
 * @param definition {{
 *      @field direction {Vector} : The direction of the helix axis.
 *              @eg `vector(0, 0, 1)`
 *      @field axisStart {Vector} : A point on the helix axis.
 *              @eg `vector(0, 0, 0) * inch`
 *      @field startPoint {Vector} : The start point of the infinite helix.  Must be off the axis.  This is the point at
 *          which the created curve would actually start if the first number of `interval` is 0.
 *              @eg `vector(1, 0, 0) * inch`
 *      @field interval {Vector} : An array of two numbers denoting the interval of the helix in terms of revolution counts.
 *              @eg `[0, 10]` will create a curve with 10 revolutions.
 *      @field clockwise {boolean} :
 *              @eg `true` if this is a clockwise helix when viewed along `direction`.
 *      @field helicalPitch {ValueWithUnits} : Distance along the axis between successive revolutions.
 *              @eg `0.1 * inch` will create a helix with 10 revolutions per inch.
 *          @eg `0 * inch` produces a planar Archimedean spiral.
 *      @field spiralPitch {ValueWithUnits} : Change in radius between successive revolutions.
 *          @eg `0 * inch` produces a helix that lies on a cylinder.
 * }}
 */
export const opHelix = function(context is Context, id is Id, definition is map)
{
    return @opHelix(context, id, definition);
};

/**
 * Creates hole tools referencing a set of targets, optionally subtracting the tools from the targets. If some tools
 * cannot be built, the operation will still succeed and indicate in its return value which holes failed to build. If no
 * tools can be built, the operation will fail.
 *
 * @param id : @autocomplete `id + "hole1"`
 * @param definition {{
 *      @field holeDefinition {HoleDefinition} : The definition of the shape of the desired holes.
 *          @eg `holeDefinition([holeProfile(HolePositionReference.AXIS_POINT, 0 * inch, 0.1 * inch), holeProfile(HolePositionReference.AXIS_POINT, 1 * inch, 0 * inch)])`
 *      @field axes {array} : An array of [Line]s each of whose `origin` represents the start position of a hole, and whose
 *          `direction` represents the drill direction of the hole.
 *          @eg `[line(vector(-1, -1, 0) * inch, vector(0, 0, -1)), line(vector(1, 1, 0) * inch, vector(0, 0, -1))]`
 *      @field identities {array} : @optional An array of queries, one per axis in `axes`, used to disambiguate each of
 *          the created holes.  Each query should resolve to exactly one entity.  Providing this information does not change
 *          the geometric outcome, but stabilizes references to the holes with respect to upstream changes to the model.
 *      @field targets {Query} : @requiredif { `holeDefinition` contains any `profiles` that do not reference
 *          `HolePositionReference.AXIS_POINT`, or if `subtractFromTargets` is `true` } A set of bodies to target. The
 *          shape of the produced holes is dependent on the shape of these targets (as specified in the supplied
 *          [HoleDefinition]), so the full set of targeted bodies should always be supplied, even if
 *          `subtractFromTargets` is `false`.
 *          @autocomplete `qAllModifiableSolidBodies()`
 *      @field subtractFromTargets {boolean} : @optional `true` if the hole geometry should be subtracted from the targets.
 *          `false` if the targets should not be modified, and the hole tools should be outputted as new solid bodies.  Default
 *          is `true`. To subtract from a subset of targets, set this to `true` and supply a set of excluded targets as
 *          `targetsToExcludeFromSubtraction`. Removing the set of excluded targets from `targets` instead of using
 *          `targetsToExcludeFromSubtraction` is not the correct way to call this interface, and may result in the shape
 *           of the hole changing.
 *      @field targetsToExcludeFromSubtraction {Query} : @optional If supplied and `subtractFromTargets` is `true`,
 *          the given targets are excluded from the subtraction. Ignored if `subtractFromTargets` is `false`
 *      @field keepTools {boolean} : @optional If `subtractFromTargets` is `true`, controls whether the hole tools should
 *          be outputted as new solid bodies. Default is `false`. Ignored if `subtractFromTargets` is `false`; in that
 *          case hole tools are always outputted as new solid bodies.
 * }}
 *
 * @return {array}: An array representing target intersection information for each hole. The array is aligned with the
 *                  `axes` input. Each item in the array is a map containing a `boolean` field `success`, which
 *                  indicates whether the tool was successfully built. If `success` is `true` the map will contain
 *                  three additional entries: `targetToDepthExtremes`, `positionReferenceInfo` and `holeDepth`.
 *
 *                  The value of `targetToDepthExtremes` is a `map` mapping the `targets` that the given hole intersects
 *                  to a map of intersection information for those targets. Only targets that are intersected by the
 *                  hole will be present in the map. Each map key is a [Query] for one of the targets, and the
 *                  corresponding value is itself a map of the form
 *                  `{ "firstEntrance" : firstEntranceDistance, "fullEntrance" : fullEntranceDistance, "firstExit" : firstExitDistance, "fullExit" : fullExitDistance }`
 *
 *                  `firstEntranceDistance`, `fullEntranceDistance`, `firstExitDistance`, and `fullExitDistance` are
 *                  [ValueWithUnits] representing distances, along the axis, from the origin point of the axis to
 *                  various important markers on the infinite hole cylinder. `firstEntranceDistance` and
 *                  `fullEntranceDistance` represent the range over which the infinite hole cylinder enters the part,
 *                  with `firstEntranceDistance` representing where the infinite hole cylinder first enters the part,
 *                  and `fullEntranceDistance` representing where the infinite hole cylinder fully enters the part.
 *                  These values are distinct when the entrance face into the part is slanted (or otherwise irregular).
 *                  `firstExitDistance` and `fullExitDistance` similarly represent the range over which the infinite
 *                  hole cylinder exits the part.
 *
 *                  The value of `positionReferenceInfo` is a `map` whose keys are the [HolePositionReference]s found in
 *                  the `holeDefinition` and whose value is a map of the form
 *                  `{ "referenceRootStart" : referenceRootStartDistance, "referenceRootEnd" : referenceRootEndDistance, "target" : targetQuery }`.
 *
 *                  `referenceRootStartDistance` is a [ValueWithUnits] representing the distance, along the axis, from
 *                  the origin point of the axis to the first coincidence between the infinite hole cylinder and the
 *                  reference in question. `referenceRootEndDistance` is a similar measurement to the last coincidence
 *                  between the infinite hole cylinder and the reference in question. For flat references, such as a
 *                  `TARGET_START` referencing the top face of a cube, these two values will be the same. The values
 *                  will differ for slanted (or otherwise irregular) references where the infinite hole cylinder
 *                  interacts with the reference over a range, rather than at a single distance.
 *
 *                  `targetQuery` is a [Query] for the `target` that defines that position reference.
 *
 *                  `holeDepth` is a [ValueWithUnits] representing the distance, along the axis from the first entrance of
 *                  the intersected targets to the termination entity. Used for references such as UP_TO_ENTITY and UP_TO_NEXT
 *                  to get a calculated depth of a hole.
 *
 *                  @example
 * ```
 * // For an opHole operation creating two holes, both going into two stacked
 * // parts, the first of which being 1 inch thick with a slightly slanted top
 * // and flat bottom and the second being 3 inches thick with a flat top and
 * // bottom, and the holeDefinition referencing both TARGET_START and
 * // LAST_TARGET_START the return value may look like:
 * [
 *     { // First hole (successful)
 *         "success" : true,
 *         "targetToDepthExtremes" : {
 *                     (firstTargetQuery)  : {
 *                             "firstEntrance" : 0.1 * inch,
 *                             "fullEntrance" : 0.3 * inch,
 *                             "firstExit" : 1 * inch,
 *                             "fullExit" : 1 * inch
 *                         },
 *                     (secondTargetQuery) : {
 *                             "firstEntrance" : 1 * inch,
 *                             "fullEntrance" : 1 * inch,
 *                             "firstExit" : 4 * inch,
 *                             "fullExit" : 4 * inch
 *                         }
 *                 },
 *         "positionReferenceInfo" : {
 *                     HolePositionReference.TARGET_START : {
 *                             "referenceRootStart" : 0.1 * inch,
 *                             "referenceRootEnd" : 0.3 * inch,
 *                             "target" : firstTargetQuery
 *                         },
 *                     HolePositionReference.LAST_TARGET_START : {
 *                             "referenceRootStart" : 1 * inch,
 *                             "referenceRootEnd" : 1 * inch,
 *                             "target" : secondTargetQuery
 *                         },
 *                 }
 *     },
 *     { // Second hole (successful)
 *         "success" : true,
 *         "targetToDepthExtremes" : {
 *                     (firstTargetQuery)  : {
 *                             "firstEntrance" : 0.4 * inch,
 *                             "fullEntrance" : 0.6 * inch,
 *                             "firstExit" : 1 * inch,
 *                             "fullExit" : 1 * inch
 *                         },
 *                     (secondTargetQuery) : {
 *                             "firstEntrance" : 1 * inch,
 *                             "fullEntrance" : 1 * inch,
 *                             "firstExit" : 4 * inch,
 *                             "fullExit" : 4 * inch
 *                         }
 *                 },
 *         "positionReferenceInfo" : {
 *                     HolePositionReference.TARGET_START : {
 *                             "referenceRootStart" : 0.4 * inch,
 *                             "referenceRootEnd" : 0.6 * inch,
 *                             "target" : firstTargetQuery
 *                         },
 *                     HolePositionReference.LAST_TARGET_START : {
 *                             "referenceRootStart" : 1 * inch,
 *                             "referenceRootEnd" : 1 * inch,
 *                             "target" : secondTargetQuery
 *                         },
 *                 }
 *     },
 *     { // Third hole (unsuccessful)
 *         "success" : false
 *     }
 * ]
 * ```
 */
export const opHole = function(context is Context, id is Id, definition is map) returns array
{
    const result = @opHole(context, id, definition);
    var out = [];
    for (var rawMap in result)
    {
        const success = rawMap.success;
        var processedMap = { "success" : success };

        if (success)
        {
            // The rest of the fields are only returned if the hole tool was successfully built
            const holeDepth = rawMap.holeDepth;
            processedMap.holeDepth = holeDepth * meter;

            var transientQueryToDepthExtremes = {};
            for (var transientId, rawDepthExtremes in rawMap.targetToDepthExtremes)
            {
                const depthExtremes = {
                        "firstEntrance" : rawDepthExtremes.firstEntrance * meter,
                        "fullEntrance" : rawDepthExtremes.fullEntrance * meter,
                        "firstExit" : rawDepthExtremes.firstExit * meter,
                        "fullExit" : rawDepthExtremes.fullExit * meter
                    };
                transientQueryToDepthExtremes[qTransient(transientId)] = depthExtremes;
            }
            processedMap.targetToDepthExtremes = transientQueryToDepthExtremes;

            var referenceEnumToInfo = {};
            for (var referenceString, rawInfo in rawMap.positionReferenceInfo)
            {
                referenceEnumToInfo[referenceString as HolePositionReference] = {
                        "referenceRootStart" : rawInfo.referenceRootStart * meter,
                        "referenceRootEnd" : rawInfo.referenceRootEnd * meter,
                        "target" : rawInfo.target == undefined ? undefined : qTransient(rawInfo.target)
                    };
            }
            processedMap.positionReferenceInfo = referenceEnumToInfo;
        }

        out = append(out, processedMap);
    }
    return out;
};

/* TODO: Example of importing from a blob tab */
/**
 * Brings foreign geometry into the context. This function is used for importing uploaded parts.
 * @param id : @autocomplete `id + "importForeign1"`
 * @param definition {{
 *      @field blobData {CADImportData} : Reference to a blob element hosting uploaded CAD data.
 *      @field flatten {boolean} : Whether to flatten assemblies; defaults to false. @optional
 *      @field yAxisIsUp {boolean} : If true, the y axis in the import maps to the z axis and z maps to -y.
 *              If false (default), the coordinates are unchanged. @optional
 *      @field isModifiable {boolean} : Whether the imported data is modifiable (default) or not. @optional
 * }}
 */
export const opImportForeign = function(context is Context, id is Id, definition is map)
{
    return @opImportForeign(context, id, definition);
};

/**
 * Creates wires corresponding to the given `faces`'s isocline edges.
 * Each isocline follows a path along a face with a constant `angle` in the (-90, 90) degree range (e.g., lines of
 * latitude on a sphere). This `angle` is the face tangent plane's angle with respect to the `direction` with its sign
 * determined by the dot product of `direction` and the face normal, and is analogous to the angle used in draft
 * analysis. Depending on the face geometry, there may be zero, one, or multiple isoclines on each face.
 * @param id : @autocomplete `id + "isocline1"`
 * @param definition {{
 *      @field faces {Query} : The faces on which to imprint isoclines.
 *      @field direction {Vector} : A reference direction.
 *      @field angle {ValueWithUnits} : The isocline angle with respect to the direction in the (-90, 90) degree range.
 * }}
 */
export const opCreateIsocline = function(context is Context, id is Id, definition is map)
{
    return @opCreateIsocline(context, id, definition);
};

/**
 * Creates a surface or solid loft fitting an ordered set of profiles, optionally constrained by guide curves.
 * @param id : @autocomplete `id + "loft1"`
 * @param definition {{
 *      @field profileSubqueries {array} : An ordered array of queries for the profiles. For a solid loft, these must be
 *              sheet bodies, faces, or vertices. For a surface loft, these could be wire bodies, sheet bodies, faces, edges, or vertices.
 *              @eg `[ profileQuery1, profileQuery2 ]`
 *      @field guideSubqueries {array} : An array of queries for guide curves. Each guide curve should intersect each profile once. @optional
 *      @field connections {array} : @optional An array of maps to define multiple profile alignments. Each connection map should contain:

                (1) connectionEntities query describing an array of vertices or edges (one per profile),


 *              (2) connectionEdges an array of individual queries for edges in connectionEntities. The order of individual
 *              edge queries should be synchronized with connectionEdgeParameters.


                (3) connectionEdgeParameters array - an ordered and synchronized array of  parameters on edges in connectionEdgeQueries
 *              @eg `[ {"connectionEntities" : qVertexAndEdge1, "connectionEdges" : [qEdge1], "connectionEdgeParameters" : [0.25]} {"connectionEntities" : qVertexAndEdge2, "connectionEdges" : [qEdge2], "connectionEdgeParameters" : [0.75]}]`
 *      @field connectionsArcLengthParameterization {boolean} : Defaults to false for better performance. Controls interpretation of connectionEdgeParameters.
 *              If [evDistance], [evEdgeTangentLine] etc. are called in conjunction with opLoft the same value should be passed as `arcLengthParameterization` there. @optional
 *      @field makePeriodic {boolean} : Defaults to false. A closed guide creates a periodic loft regardless of this option. @optional
 *      @field bodyType {ToolBodyType} : Specifies a `SOLID` or `SURFACE` loft. Default is `SOLID`. @optional
 *      @field trimGuidesByProfiles {boolean} : If false (default) use full length of guides. If true restrict resulting surface by the first and last profile.
 *                                              Meaningful only for non-periodic surface loft. @optional
 *      @field trimProfiles {boolean} : If false (default) use full length of profiles. If true restrict resulting surface by the first and last guide or connection.
 *                                              Meaningful only for surface loft with open profiles. @optional
 *      @field derivativeInfo {array} :  @optional An array of maps that contain shape constraints at start and end profiles. Each map entry
 *              is required to have a profileIndex that refers to the affected profile. Optional fields include a vector to match surface tangent to,
 *              a magnitude, and booleans for matching tangents or curvature derived from faces adjacent to affected profile.
 *              @ex `[ { "profileIndex" : 0, "vector" : vector(1, 0, 0), "magnitude" : 2., "tangentToPlane" : true}, { "profileIndex" : 1, "matchCurvature" : true, "adjacentFaces" : qFaces } ]`
 *              The first map would constrain the resulting loft at the start profile to be tangent to plane with normal vector(1,0,0) and magnitude 2.
 *              The second map constrains the loft at the end profile to match the curvature of faces defined by the query qFaces.
 *      @field showIsocurves {boolean} : Show graphical representation of a subset of isoparameteric curves on each face of the created loft. Default `false`. @optional
 *      @field curveCount {number} : When `showIsocurves` is `true`, the number of curves to draw in each direction of each face's grid. Default `10`. @optional
 *      @field loftTopology {LoftTopology} : Specifies topology of lofted body. Default is `MINIMAL`. @optional
 *      @field addSections {boolean} : `true` to add a path. Default is `false`. @optional
 *      @field spine {Query} : Query to specify the edge for the path. @optional
 *      @field sectionCount {number} : Number of sections to add. Required if `addSections=true`. @optional
 * }}
 */
export const opLoft = function(context is Context, id is Id, definition is map)
{
    return @opLoft(context, id, definition);
};

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
export const opMateConnector = function(context is Context, id is Id, definition is map)
{
    return @opMateConnector(context, id, definition);
};

/**
 * Bring all of the information from `contextFrom` into `context`.  This is used, for example, for the Derived feature.
 * @param context {Context} : The target context.
 * @param id : @autocomplete `id + "mergeContexts1"`
 * @param definition {{
 *      @field contextFrom {Context} : The source context. It is rendered unusable by this operation.
 *              @eg `MyPartStudio::build()`
 *       @field trackThroughMerge {array} : @optional
 *              Array of queries to map evaluation result in contextFrom to context post-merge
 * }}
 * @return {array} : Returns array of the same size as trackThroughMerge
 *                   with evaluation results for each query(array of arrays of transient queries).
 */
export const opMergeContexts = function(context is Context, id is Id, definition is map) returns array
{
    const result = @opMergeContexts(context, id, definition);
    var out = [];
    for (var query in result)
    {
        var queryResult = [];
        for (var queryItem in query)
            queryResult = append(queryResult, qTransient(queryItem));
        out = append(out, queryResult);
    }
    return out;
};

/**
 * This is a direct editing operation that modifies or deletes fillets.
 * @param id : @autocomplete `id + "modifyFillet1"`
 * @param definition {{
 *      @field faces {Query} : The fillets to modify.
 *      @field modifyFilletType {ModifyFilletType} : Whether to change the fillet radii or remove them altogether.
 *      @field radius {ValueWithUnits} : @requiredif {`modifyFilletType` is `CHANGE_RADIUS`.} The new radius.
 *      @field reFillet {boolean} : @requiredif {`modifyFilletType` is `CHANGE_RADIUS`.}
 *              `true` to reapply adjacent fillets. Default is `false`.
 * }}
 */
export const opModifyFillet = function(context is Context, id is Id, definition is map)
{
    return @opModifyFillet(context, id, definition);
};

/**
 * This is a direct editing operation that applies a transform to one or more faces.
 * @param id : @autocomplete `id + "moveFace1"`
 * @param definition {{
 *      @field moveFaces {Query} : The faces to transform.
 *      @field transform {Transform} : The transform to apply to the face.
 *              @eg `transform(vector(0, 0, 1) * inch)` will translate the face 1 inch along the world's z-axis.
 *      @field reFillet {boolean} : @optional
 *              `true` to attempt to defillet `moveFaces` prior to the move and reapply the fillet
 *              after. Default is `false`.
 *      @field mergeFaces {boolean} : @optional
 *              `true` to remove redundant edges from moveFaces. Default is `true`.
 * }}
 */
export const opMoveFace = function(context is Context, id is Id, definition is map)
{
    return @opMoveFace(context, id, definition);
};

/**
 * @internal
 * Under development, not for general use.
 *
 * Assigns name to the entity. This will allow using qNamed() to query the entity.
 * When historical queries are generated, qNamed() will be used as a shortcut.
 * If definition.entity resolves to multiple entities, the operation completes with a warning status.
 * @param id : @autocomplete `id + "nameEntity1"`
 * @param definition {{
 *      @field entity {Query} : The entity to be named.
 *      @field entityName {string} : The name, should be unique in the part studio.
 * }}
*/
export const opNameEntity = function(context is Context, id is Id, definition is map)
{
    return @opNameEntity(context, id, definition);
};

/**
 * This is a direct editing operation that offsets one or more faces.
 * @param id : @autocomplete `id + "offsetFace1"`
 * @param definition {{
 *      @field moveFaces {Query} : The faces to offset.
 *      @field offsetDistance {ValueWithUnits} : The positive or negative distance by which to offset.
 *              @eg `0.1 * inch` will offset the face 0.1 inches, normal to the face.
 *      @field reFillet {boolean} : @optional
 *              `true` to attempt to defillet `moveFaces` prior to the offset and reapply the fillet
 *              after. Default is `false`.
 *      @field mergeFaces {boolean} : @optional
 *              `true` to remove redundant edges from moveFaces. Default is `true`.
 * }}
 */
export const opOffsetFace = function(context is Context, id is Id, definition is map)
{
    return @opOffsetFace(context, id, definition);
};

/**
* @internal
* This operation creates a new wire body by applying offset on planar edges.
* Wire body is located in the same plane where the source edges are.
*
* Multiple edges may be selected at once - single entities and edge chains: open and closed.
*
* Edges are not allowed to intersect or have full or partial coincidence.
*
* The default offset direction of the OPEN edge chain is defined by its first selected edge.
* The default offset direction of the CLOSED edge chain is outward of the edge-enclosed region.
*
* Also can be created sheet region surrounded by original and offset wire bodies.
* Region creation supports both - closed and open-edge sequences.
* @param id : @autocomplete `id + "offsetWire1"`
* @param definition {{
*       @field edges {Query} : Edges for the offset.
*       @field normal {Vector} : Normal of the plane where edges are lie. Affects the offset direction.
*       @field offset1 {ValueWithUnits} : Distance by which to offset in forward direction.
*       @field offset2 {ValueWithUnits} : Distance by which to offset in reverse direction.
*       @field flip {boolean} : @optional
*           `true` to swap the offset1 and offset2 Default is `false`.
*       @field makeRegions {boolean} :  @optional
*           `true` to create flat sheet body surrounded by the original and the offset wires. Default is `false`.
* }}
*/
export const opOffsetWire = function(context is Context, id is Id, definition is map)
{
    return @opOffsetWire(context, id, definition);
};

/**
 * Patterns input faces and/or bodies by applying transforms to them. The original faces and bodies are preserved.
 * @param id : @autocomplete `id + "pattern1"`
 * @param definition {{
 *      @field entities {Query} : Bodies and faces to pattern.
 *      @field transforms {array} : An array of [Transform]s to apply to `entities`. The transforms do not have to be
 *          rigid.
 *      @field instanceNames {array} : An array of distinct non-empty strings the same size as `transforms` to identify
 *              the patterned entities.  Similar to an `Id`, an instance names may consist only of letters, numbers, and any of `+`, `-`, `/`, `_`.
 *      @field copyPropertiesAndAttributes {boolean} : If true (default), copies properties and attributes to patterned entities. @optional
 * }}
 */
export const opPattern = function(context is Context, id is Id, definition is map)
{
    return @opPattern(context, id, definition);
};

/**
 * @internal
 * TODO!!
 */
export const opBooleanedPattern = function(context is Context, id is Id, definition is map)
{
    return @opBooleanedPattern(context, id, definition);
};

/**
 * Creates a construction plane.
 * @param id : @autocomplete `id + "plane1"`
 * @param definition {{
 *      @field plane {Plane} : The plane to create.
 *              @eg `plane(vector(0, 0, 6) * inch, vector(0, 0, 1))`
 *      @field width {ValueWithUnits} : @optional
 *              The side length of the construction plane, as it is initially displayed.
 *      @field height {ValueWithUnits} : @optional
 *              The side length of the construction plane, as it is initially displayed.
 *      @field defaultType @internalType {DefaultPlaneType} : For Onshape internal use. @optional
 * }}
 */
export const opPlane = function(context is Context, id is Id, definition is map)
{
    return @opPlane(context, id, definition);
};

/**
 * Creates a polyline passing through an array of 3D points.
 * @param id : @autocomplete `id + "polyline1"`
 * @param definition {{
 *      @field points {array} : An array of `Vector`s with length units corresponding the points of the polyline.
 *          If the firstpoint is the same as the last point, the polyline is closed.
 *              @eg
 * ```
 * [
 *     vector( 1,  1,  1) * inch,
 *     vector(-1,  1, -1) * inch,
 *     vector( 1, -1, -1) * inch,
 *     vector(-1, -1,  1) * inch,
 *     vector( 1,  1,  1) * inch
 * ]
 * ```
 *      @field bendRadii {array} : An array of `ValueWithUnits`, corresponding to the bend radii at each point.
 *          The value at index i corresponds to the bend at the point at index i + 1 in `points`.
 *          A value of `0 * meter` means no bend at the corresponding point.
 *          Its size should be `size(points) - 2` for an open polyline, `size(points) - 1` for a closed one.
 *          @optional
 * }}
 */
export const opPolyline = function(context is Context, id is Id, definition is map)
{
    return @opPolyline(context, id, definition);
};

/**
 * Projects curves on a face.
 * @param id : @autocomplete `id + "dropCurve1"`
 * @param definition {{
 *      @field tools {Query} : The edges to project.
 *      @field targets {Query} : The faces/bodies the edges are to be projected onto.
        @field projectionType {ProjectionType} : Projection method.
 *      @field direction {Vector} : @optional
                The direction in which to project the curve. Required if projectionType is ProjectionType.DIRECTION.
 * }}
 */
export const opDropCurve = function(context is Context, id is Id, definition is map)
{
    return @opDropCurve(context, id, definition);
};

/**
 * Intersect two faces, creating curves appropriately.
 * @param id : @autocomplete `id + "intersectFaces1"`
 * @param definition {{
 *      @field tools {Query} : First array of faces to intersect.
 *      @field targets {Query} : Second array of faces to intersect.
 * }}
 */
export const opIntersectFaces = function(context is Context, id is Id, definition is map)
{
    return @opIntersectFaces(context, id, definition);
};

/**
 * Creates a sphere.
 * @param definition {{
 *      @field radius {ValueWithUnits} : The radius of the sphere.
            @eg `1 * inch`
 *      @field center {Vector} : The location of the center of the sphere.
            @eg `vector(1, 1, 1) * inch`
 * }}
 */
export const opSphere = function(context is Context, id is Id, definition is map)
{
    return @opSphere(context, id, definition);
};

/**
 * Creates a 3D spline curve representing a sequence of edges.
 * The edges must form a tangent-continuous chain.
 * @param id : @autocomplete `id + "splineThroughEdges1"`
 * @param definition {{
 *       @field edges {Query} : Edges to approximate.
 * }}
 */
export const opSplineThroughEdges = function(context is Context, id is Id, definition is map)
{
    return @opSplineThroughEdges(context, id, definition);
};

/**
 * Splits an array of edges with an entity or at specified parameters.
 * @param id : @autocomplete `id + "splitEdges1"`
 * @param definition {{
 *       @field edges {Query} : Edges to split.
 *       @field parameters {array} :
 *              An array of array of parameters. Each edge gets split at the parameter values at the matching index
 *              in this array.
 *       @field splittingSurface {Query} : A sheet body, a construction plane or a face to cut with. Can be either a [Query] or a [Plane].
 *              Either `splittingSurface` or `parameters` must be specified but not both.
 *       @field arcLengthParameterization {boolean} : @optional
 *              If true (default), the parameter returned for edges measures distance
 *              along the edge, so `0.5` is the midpoint.
 *              If false, use an arbitrary but faster-to-calculate parameterization.
 *              Default is `true`.
 * }}
 */
export const opSplitEdges = function(context is Context, id is Id, definition is map)
{
    return @opSplitEdges(context, id, definition);
};

/**
 * Creates a construction point (a `BodyType.POINT` with one vertex).
 * @param id : @autocomplete `id + "point1"`
 * @param definition {{
 *      @field point {Vector} : The location of the point.
 *              @eg `vector(0, 0, 1) * inch`
 *      @field origin {boolean} : For Onshape internal use. @optional
 * }}
 */
export const opPoint = function(context is Context, id is Id, definition is map)
{
    return @opPoint(context, id, definition);
};

/**
 * This is a direct editing operation that replaces the geometry one or more faces with that of another face or multiple adjacent faces, possibly
 * with an offset.
 * @param id : @autocomplete `id + "replaceFace1"`
 * @param definition {{
 *      @field replaceFaces {Query} : The faces whose geometry to replace.
 *      @field templateFace {Query} : The face or multiple adjacent faces whose geometry to use as the replacement.
 *      @field offset {ValueWithUnits} : @optional
 *              The positive or negative distance by which to offset the resulting face.
 *      @field oppositeSense {boolean} : @optional
 *              If true, flip the surface normal of the resulting face, which may
 *              be necessary to match the surface normals of surrounding faces.
 *              Default is `false`.
 * }}
 */
export const opReplaceFace = function(context is Context, id is Id, definition is map)
{
    return @opReplaceFace(context, id, definition);
};

/**
 * Revolves edges and faces about an axis to produce sheet and solid bodies. The edges and faces may abut, but not
 * strictly intersect the axis.
 * @param id : @autocomplete `id + "revolve1"`
 * @param definition {{
 *      @field entities {Query} : The edges and faces to revolve.
 *      @field axis {Line} : The axis around which to revolve.
 *              @eg `line(vector(0, 0, 0) * inch, vector(0, 0, 1))`
 *      @field angleForward {ValueWithUnits} : The angle where the revolve ends relative to `entities`. Normalized to the range \[0, 2 PI).
 *              @eg `30 * degree`
 *      @field angleBack {ValueWithUnits} : The angle where the revolve starts relative to `entities`. Normalized to the range \[0, 2 PI).
 *          If `angleForward == angleBack`, the revolve is a full (360-degree) revolve. Defaults to `0`. @optional
 * }}
 */
export const opRevolve = function(context is Context, id is Id, definition is map)
{
    return @opRevolve(context, id, definition);
};

/**
 * Creates a ruled surface along a set of paths. Direction of ruled surface is specified with either `ruledDirection`
 *      or `angleFromFaces`.
 * @param id : @autocomplete `id + "ruledSurface1"`
 * @param definition {{
 *      @field path {Query} : The edges that form the paths for the ruled surface.
 *      @field cornerType {RuledSurfaceCornerType} : How corners in the ruled surface are handled. Default is
 *          `RuledSurfaceCornerType.SPUN`. @optional
 *      @field useCubicInterpolation {boolean} : Ruled surface will use a cubic interpolation if `true`. Otherwise,
 *          a linear interpolation will be used. Default is `true`. @optional
 *      @field width {ValueWithUnits} : The width of the ruled surface.
 *      @field ruledSurfaceType {RuledSurfaceType} : Specifies how the ruled surface is constructed.
 *      @field ruledDirection {Vector} : @requiredIf { `ruledSurfaceType` is `ALIGNED_WITH_VECTOR` }
 *          Ruled surface will be aligned with this vector.
 *      @field angle {ValueWithUnits} : The angle at which the ruled surface meets reference faces or `ruledDirection`. Default is `0`.
 *      @field referenceFaces {Query} : A set of faces from which to measure `angleFromFaces`. @requiredIf { `ruledSurfaceType` is `ANGLE_FROM_FACE` }
 *      @field vertexOverrides {{
 *          @field vertex {Query} : A vertex on the path where the override is applied.
 *          @field ruledDirection {Vector} : If specified, override will specify local direction of ruled surface along
 *              this vector.
 *          @field width {ValueWithUnits} : @requiredIf { `ruledDirection` != undefined or `angleFromFaces` != undefined }
 *              Width of ruled surface at this override.
 *          @field angleFromFaces {ValueWithUnits} : If specified, override will specify direction as an angle to reference
 *              faces. This is only applicable if angleFromFaces is also specified at the top level.
 *          @field upToEntity {Query} : If specified, override will specify that ruled surface touches upToEntity at override.
 *    }}
 * }}
 */
export const opRuledSurface = function(context is Context, id is Id, definition is map)
{
    return @opRuledSurface(context, id, definition);
};

/**
 * Create a shell of a solid body with uniform thickness. The bodies that are passed
 * in are hollowed, omitting the walls on the `face` entities passed in.
 * @param id : @autocomplete `id + "shell1"`
 * @param definition {{
 *      @field entities {Query} : The faces to shell and solid bodies to hollow.
 *      @field thickness {ValueWithUnits} : The distance by which to shell. Positive means shell outward, and negative
 *              means shell inward. @autocomplete `0.1 * inch`
 * }}
 */
export const opShell = function(context is Context, id is Id, definition is map)
{
    return @opShell(context, id, definition);
};

/**
 * Either adds or removes material from the flat.
 * @internal
 * @param id : @autocomplete `id + "flatOp"`
 * @param definition {{
 *      @field faces {Query} : Faces to add or remove.
 *      @field operationType {BooleanOperationType} : The boolean operation to perform. Must be union or subtraction.
 * }}
 */
export const opSMFlatOperation = function(context is Context, id is Id, definition is map)
{
    return @opSMFlatOperation(context, id, definition);
};

/**
 * Split solid, sheet, and wire bodies with the given sheet body.
 * @param id : @autocomplete `id + "splitPart1"`
 * @param definition {{
 *      @field targets {Query} : The solid, sheet, and wire bodies to split.
 *      @field tool : A sheet body, a construction plane or a face to cut with. Can be either a [Query] or a [Plane].
 *              If a planar face is passed in, the split will extend the plane infinitely unless `useTrimmed` is `true`.
 *      @field keepTools {boolean} : If false, the tool is deleted. Default is `false`. @optional
 *      @field keepType {SplitOperationKeepType} : Controls which pieces to keep. Default is `KEEP_ALL`. @optional
 *      @field useTrimmed {boolean} : If true, the trimmed face boundaries are used as the tool, rather than the underlying surface. Default is `false`. @optional
 * }}
 */
export const opSplitPart = function(context is Context, id is Id, definition is map)
{
    return @opSplitPart(context, id, definition);
};

/**
 * Split faces with the given edges or faces.
 * @param id : @autocomplete `id + "splitFace1"`
 * @param definition {{
 *      @field faceTargets {Query} : The faces to split.
 *      @field edgeTools {Query} : @optional
 *              The edges to cut with.
 *      @field projectionType {ProjectionType} : @optional Edge projection method. Default is `ProjectionType.NORMAL_TO_TARGET`
 *      @field direction {Vector} : @requiredif {`edgeTools` are present and `projectionType` is `ProjectionType.DIRECTION`.}
 *              The projection direction.
 *      @field faceTools {Query} : @optional
 *              The faces to cut with.
 *      @field bodyTools {Query} : @optional
 *              The sheet or wire bodies to cut with.
 *      @field keepToolSurfaces {boolean} : @optional
 *              If `true`, the `bodyTools` do not get consumed by the operation.  Default is `true`.
 *      @field planeTools {Query} : @optional
 *              These planar faces are treated as infinite, rather than bounded to the face extents.
 *      @field extendToCompletion {boolean} : @optional
 *             if `true`, imprinted edges are extended to complete split of faces. Default is `false`.
 *      @field mutualImprint {boolean} : @optional
 *             if `true` and only `bodyTools` are supplied target faces are used to split faces of `bodyTools`.
 *             Default is `false`.
 * }}
 */
export const opSplitFace = function(context is Context, id is Id, definition is map)
{
    return @opSplitFace(context, id, definition);
};

/**
 * Map containing the results of splitting faces by their isoclines. Some faces may have been split, others
 * may have been left intact.
 *
 * @type {{
 *      @field steepFaces {array} : An array of steep faces.
 *      @field nonSteepFaces {array} : An array of non-steep faces.
 *      @field boundaryEdges {array} : An array of edges at the transition from non-steep faces to steep faces.
 * }}
 */
export type SplitByIsoclineResult typecheck canBeSplitByIsoclineResult;

/** @internal */
export predicate canBeSplitByIsoclineResult(value)
{
    value is map;
    value.steepFaces is array;
    value.nonSteepFaces is array;
    value.boundaryEdges is array;
}

/**
 * Split the given `faces` by isocline edges.
 * Each isocline follows a path along a face with a constant `angle` in the (-90, 90) degree range (e.g., lines of
 * latitude on a sphere). This `angle` is the face tangent plane's angle with respect to the `direction` with its sign
 * determined by the dot product of `direction` and the face normal, and is analogous to the angle used in draft
 * analysis. Depending on the face geometry, there may be zero, one, or multiple isoclines on each face.
 * The isocline edges are created as new edges which split the provided `faces`. The resulting faces are either steep
 * (i.e., the angle is less than the input `angle`) or non-steep. To instead leave the original faces intact, you can
 * first extract the faces with opExtractSurface(), and create isoclines on the resulting surfaces.
 * The isocline edges can be queried for with [qCreatedBy]. The split orientation is consistent such that the
 * non-steep faces are always in "front" of the split, and can be reliably queried for with [qSplitBy]:
 * @example ```
 * const isoclineEdges = qCreatedBy(id + "splitByIsocline1", EntityType.EDGE);
 * const steepFaces = qSplitBy(id + "splitByIsocline1", EntityType.FACE, true);
 * const nonSteepFaces = qSplitBy(id + "splitByIsocline1", EntityType.FACE, false);
 * ```
 * Note that [qSplitBy] will return only those faces that were split, while the returned [SplitByIsoclineResult] will
 * include the intact faces as well.
 * @param id : @autocomplete `id + "splitByIsocline1"`
 * @param definition {{
 *      @field faces {Query} : The faces on which to imprint isoclines.
 *      @field direction {Vector} : A reference direction.
 *      @field angle {ValueWithUnits} : The isocline angle with respect to the direction in the (-90, 90) degree range.
 * }}
 */
export const opSplitByIsocline = function(context is Context, id is Id, definition is map) returns SplitByIsoclineResult
{
    const data = @opSplitByIsocline(context, id, definition);

    var steepFaces = [];
    for (var transientId in data.steepFaces)
    {
        steepFaces = append(steepFaces, qTransient(transientId));
    }
    var nonSteepFaces = [];
    for (var transientId in data.nonSteepFaces)
    {
        nonSteepFaces = append(nonSteepFaces, qTransient(transientId));
    }

    var boundaryEdges = [];
    for (var transientId in data.boundaryEdges)
    {
        boundaryEdges = append(boundaryEdges, qTransient(transientId));
    }

    return {
        "steepFaces": steepFaces,
        "nonSteepFaces": nonSteepFaces,
        "boundaryEdges": boundaryEdges
    } as SplitByIsoclineResult;
};

/**
 * Map containing the results of splitting bodies by their shadow curves. Some faces may have been split, others
 * may have been left intact.
 *
 * @type {{
 *      @field visibleFaces {array} : An array of visible faces.
 *      @field invisibleFaces {array} : An array of invisible faces.
 * }}
 */
export type SplitBySelfShadowResult typecheck canBeSplitBySelfShadowResult;

/** @internal */
export predicate canBeSplitBySelfShadowResult(value)
{
    value is map;
    value.visibleFaces is array;
    value.invisibleFaces is array;
}

/**
 * Splits the faces of the given `bodies` into visible and invisible regions with respect to
 * the given `viewDirection`, creating shadow curves as necessary. A shadow curve represents
 * the transition of one face from visible to invisible. Depending on the geometry, there may
 * be zero, one, or more shadow curves per face.
 * The shadow curve edges are created as new edges which split the faces of the provided
 * `bodies`. Each of the resulting faces is wholly visible or wholly invisible. Edge-on faces
 * are considered invisible.
 * The shadow curve edges can be queried for with [qCreatedBy]. The split orientation is
 * consistent such that the visible faces are always in "front" of the split, and can be
 * reliably queried for with [qSplitBy]:
 * @example ```
 * const shadowEdges = qCreatedBy(id + "splitBySelfShadow1", EntityType.EDGE);
 * const invisibleFaces = qSplitBy(id + "splitBySelfShadow1", EntityType.FACE, true);
 * const visibleFaces = qSplitBy(id + "splitBySelfShadow1", EntityType.FACE, false);
 * ```
 * Note that [qSplitBy] will return only those faces that were split, while the returned
 * [SplitBySelfShadowResult] will include the intact faces as well.
 * @param id : @autocomplete `id + "splitBySelfShadow1"`
 * @param definition {{
 *      @field bodies {Query} : The bodies which cast shadows and on which to imprint shadow curves.
 *      @field viewDirection {Vector} : The viewing direction.
 * }}
 */
export const opSplitBySelfShadow = function(context is Context, id is Id, definition is map) returns SplitBySelfShadowResult
{
    const data = @opSplitBySelfShadow(context, id, definition);

    var visibleFaces = [];
    for (var transientId in data.visibleFaces)
    {
        visibleFaces = append(visibleFaces, qTransient(transientId));
    }
    var invisibleFaces = [];
    for (var transientId in data.invisibleFaces)
    {
        invisibleFaces = append(invisibleFaces, qTransient(transientId));
    }

    return {
        "visibleFaces": visibleFaces,
        "invisibleFaces": invisibleFaces
    } as SplitBySelfShadowResult;
};

/**
 * Sweep the given edges and faces along a path resulting in solid and/or sheet bodies.
 * @param id : @autocomplete `id + "sweep1"`
 * @param definition {{
 *      @field profiles {Query} : Edges and faces to sweep.
 *      @field path {Query} : Edges that comprise the path along which to sweep. The edges can be in any order but
 *          must form a connected path.
 *      @field keepProfileOrientation {boolean} : If `true`, the profile maintains its original orientation as it is
 *              swept. If `false` (default), the profile rotates to remain normal to the path. @optional
 *      @field lockFaces {Query} : Keep profile aligned to the normals of these faces. @optional
 *      @field lockDirection {Query} : Keep profile perpendicular to this direction. @optional
 *      @field profileControl {ProfileControlMode} : @optional Default is NONE
            @eg `ProfileControlMode.KEEP_ORIENTATION` the profile maintains its original orientation as it is swept.
            @eg `ProfileControlMode.LOCK_DIRECTION` the profile is perpendicular to given direction.
            @eg `ProfileControlMode.LOCK_FACES` the profile is aligned to the normals of given faces.
 * }}
 */
export const opSweep = function(context is Context, id is Id, definition is map)
{
    return @opSweep(context, id, definition);
};

/**
 * Thicken sheet bodies and faces into solid bodies.
 * @param id : @autocomplete `id + "thicken1"`
 * @param definition {{
 *      @field entities {Query} : The sheet bodies and faces to thicken.
 *      @field thickness1 {ValueWithUnits} : The distance by which to thicken in the direction along the normal.
 *              @autocomplete `0.1 * inch`
 *      @field thickness2 {ValueWithUnits} : The distance by which to thicken in the opposite direction.
 *              @autocomplete `0.1 * inch`
 *      @field keepTools {boolean} : @optional Default is `true`. If `false` the operation will attempt to delete the
 *              `entities`. The operation will not delete sheet bodies unless the sheet body or all faces of
 *              the sheet body are selected. The operation will not delete sketches or solid bodies.
 * }}
 */
export const opThicken = function(context is Context, id is Id, definition is map)
{
    return @opThicken(context, id, definition);
};

/**
 * Applies a given transform to one or more bodies. To make transformed copies of bodies, use [opPattern].
 *
 * @param id : @autocomplete `id + "transform1"`
 * @param definition {{
 *      @field bodies {Query} : The bodies to transform.
 *      @field transform {Transform} : The transform to apply.
 *              @eg `transform(vector(0, 0, 1) * inch)` will translate the body 1 inch along the world's z-axis.
 *              @eg `rotationAround(myLine, 30 * degree)` will rotate around a [Line] object.
 *              @eg `scaleUniformly(factor)` will scale uniformly about the origin.
 *              @eg `scaleUniformly(factor, point)` will scale uniformly about a given point.
 *              @eg `toWorld(cSys)` will (somewhat counterintuitively) perform a transform such that geometry on
 *                  the world's origin and axes will move to the `cSys` origin and axes.
 *              @eg `fromWorld(cSys)` will (somewhat counterintuitively) perform a transform such that geometry on
 *                  the `cSys` origin and axes will move to the world origin and axes.
 *              @eg `transform2 * transform1` will perform `transform1`, followed by `transform2`.
 * }}
 */
export const opTransform = function(context is Context, id is Id, definition is map)
{
    return @opTransform(context, id, definition);
};

/**
 * Wraps or unwraps faces from one surface onto another.  The location and orientation of the wrapped faces on the destination
 * surface are controlled by the `anchorPoint` and `anchorDirection` of the `source` and `destination` [WrapSurface]s.
 * The `entities` of the operation are not affected, the result of this operation is a new set of surface bodies or imprinted edges
 * representing the wrapped or unwrapped faces. Faces that are topologically connected will remain topologically connected in the result
 * body for `WrapType.SIMPLE` and `WrapType.TRIM`. This operation currently supports wrapping from a plane onto a cylinder or a cone,
 * and unwrapping from a cylinder or a cone onto a plane.
 *
 * (Formerly `opRoll`)
 *
 * @param id : @autocomplete `id + "wrap1"`
 * @param definition {{
 *      @field wrapType {WrapType} : The type of wrap to execute.
 *              @eg `WrapType.SIMPLE` wraps `entities` around the infinite definition of `destination`.
 *      @field entities {Query} : Faces to wrap from `source` to `destination`.
 *      @field source {WrapSurface}      : The surface to wrap from. All `entities` must lie on this surface.
 *      @field destination {WrapSurface} : The surface to wrap onto. Must be defined using the `face` field for `WrapType.TRIM` or `WrapType.IMPRINT`.
 *      @field orientWithDestination {boolean} : @optional If true (default), the normals of the resulting surface will point
 *                                               in the same direction as the `destination`. If false, the normals of the
 *                                               resulting surface will point in the opposite direction. For the purpose
 *                                               of this parameter, the normals of a [WrapSurface] defined by an infinite
 *                                               [Cylinder] are always pointing outwards.
 * }}
 */
export const opWrap = function(context is Context, id is Id, definition is map)
{
    return @opWrap(context, id, definition);
};

// NOTE: For documentation readability, new operations in this file should be sorted alphabetically

