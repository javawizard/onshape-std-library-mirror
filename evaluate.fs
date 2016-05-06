FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/**
 * Evaluation functions return information about the topological entities in the context, like bounding boxes, tangent
 * planes, projections, and collisions. Evaluation functions take a context and a map that specifies the
 * computation to be performed and return a ValueWithUnits, a FeatureScript geometry type (like `Line` or `Plane`), or a special
 * type like `DistanceResult`. They may also throw errors if a query fails to evaluate or the input is otherwise invalid.
 */
import(path : "onshape/std/box.fs", version : "✨");
export import(path : "onshape/std/clashtype.gen.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/context.fs", version : "✨");
import(path : "onshape/std/coordSystem.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
export import(path : "onshape/std/edgeconvexitytype.gen.fs", version : "✨");
import(path : "onshape/std/mathUtils.fs", version : "✨");
import(path : "onshape/std/query.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/units.fs", version : "✨");

/**
 * Return the total area of all the entities.
 * If no matching 2D faces are found the total area will be zero.
 * @param arg {{
 *      @field entities{Query}
 * }}
 */
export function evArea(context is Context, arg is map) returns ValueWithUnits
precondition
{
    arg.entities is Query;
}
{
    var faces = qEntityFilter(arg.entities, EntityType.FACE);
    var ownedFaces = qOwnedByBody(arg.entities, EntityType.FACE);
    return @evArea(context, { "faces" : qUnion([faces, ownedFaces]) }) * meter ^ 2;
}

/**
 * If the query finds one entity with an axis -- a line, circle,
 * plane, cylinder, cone, sphere, torus, or revolved surface -- return
 * the axis.
 * Otherwise throw an exception.
 * @param arg {{
 *      @field axis{Query}
 * }}
 */
export function evAxis(context is Context, arg is map) returns Line
precondition
{
    arg.axis is Query;
}
{
    return lineFromBuiltin(@evAxis(context, arg));
}

/**
 * Find a bounding box around an entity, optionally with respect
 * to a given coordinate system.
 * @param arg {{
 *      @field topology{Query} : The entity to find the bounding box of.
 *      @field cSys{CoordSystem} : The coordinate system to use (if not the standard coordinate system). @optional
 * }}
 */
export function evBox3d(context is Context, arg is map) returns Box3d
precondition
{
    arg.topology is Query;
    arg.cSys == undefined || arg.cSys is CoordSystem;
}
{
    var result = @evBox(context, arg);
    return box3d(meter * vector(result.minCorner), meter * vector(result.maxCorner));
}

/**
 * Find collisions between tools and targets.  Each collision is a
 * map with field `type` of type `ClashType` and fields `target`,
 * `targetBody`, `tool`, and `toolBody` of type `Query`.
 * @param context
 * @param arg {{
 *      @field tools{Query} @field targets{Query}
 * }}
 * @returns {array}
 */
export function evCollision(context is Context, arg is map) returns array
precondition
{
    arg.tools is Query;
    arg.targets is Query;
    if (arg.passOwners != undefined)
        arg.passOwners is boolean;
}
{
    if (arg.passOwners == undefined)
        arg.passOwners = false;

    var collisions is array = @evCollisionDetection(context, { "tools" : arg.tools, "targets" : arg.targets, "owners" : arg.passOwners });
    for (var i = 0; i < size(collisions); i += 1)
    {
        /* Each collision is a map with fields type, target, targetBody, tool, toolBody */
        collisions[i]['type'] = collisions[i]['type'] as ClashType;
        for (var entry in collisions[i])
        {
            if (entry.value is builtin)
            {
                collisions[i][entry.key] = qTransient(entry.value as TransientId);
            }
        }
    }
    return collisions;
}

/**
 * Given a query for a curve, return a `Circle`, `Ellipse`, or `Line`
 * value for the curve.  If the curve is none of these types, return
 * a map with unspecified contents.  If the query does not find a curve,
 * throw an exception.
 * @param arg {{
 *      @field edge{Query} : The curve to evaluate.
 * }}
 * @throws
 */
export function evCurveDefinition(context is Context, arg is map) returns map
precondition
{
    arg.edge is Query;
}
{
    var result = @evCurveDefinition(context, arg);
    if (result is map)
    {
        if (result.curveType == (CurveType.CIRCLE as string))
        {
            result = circleFromBuiltin(result);
        }
        else if (result.curveType == (CurveType.ELLIPSE as string))
        {
            result = ellipseFromBuiltin(result);
        }
        else if (result.curveType == (CurveType.LINE as string))
        {
            result = lineFromBuiltin(result);
        }
    }

    return result;
}

// =========== evDistance stuff ===========

/**
 * The result of an evDistance call -- information about the extremal distance and the attaining point / line / entity.
 *
 * @type {{
 *      @field distance {ValueWithUnits} : The minimal or maximal distance.
 *      @field sides {array} : An array of 2 maps, containing information about where the extremum was found for each side.  Each map has a:
 *
 *          `point` (`Vector` of lengths) : represents the position that attains the minimum or maximum on that side.
 *
 *          `index` (integer) :  the index into the line or point array or into the query results, if a query is passed in.
 *
 *          `parameter` (number or length or array of two numbers) : If the `index` refers to an edge,
 *                  the parameter is a number between 0 and 1 (unless extend for that side was passed in).  It is in the form that
 *                  `evEdgeTangentLine` consumes (with `arcLengthParameterization` set to `false`).  If the side has `Line`(s),
 *                  the parameter is a length representing the distance along the direction.
 *                  If the `index` refers to a face, the parameter is a 2D `Vector` in the form that `evFaceTangentPlane` consumes.
 * }}
 */
export type DistanceResult typecheck canBeDistanceResult;

predicate canBeDistanceResult(value)
{
    value is map;
    isLength(value.distance);
    value.sides is array;
    size(value.sides) == 2;
    for (var sideResult in value.sides)
    {
        sideResult is map;
        isNonNegativeInteger(sideResult.index); // Index into either input array or results of input query evaluation
        is3dLengthVector(sideResult.point);
        // The parameter is either one number (for a curve) or an array of two (for a surface).  For bodies or points, the parameter is 0.
        // For lines, the parameter is a length representing the distance along the direction.
        if (!(sideResult.parameter is number || isLength(sideResult.parameter)))
        {
            sideResult.parameter is Vector;
            size(sideResult.parameter) == 2;
            sideResult.parameter[0] is number;
            sideResult.parameter[1] is number;
        }
    }
}

/**
 * Computes the minimum or maximum distance between geometry on `side0` and geometry on `side1`.  "Geometry" means entities, points, or lines.
 * When the minimum or the maximum is not uniquely defined, ties will be broken arbitrarily.  @see `DistanceResult`
 * @example `evDistance(context, { "side0" : vector(1, 2, 3) * meter, "side1" : query }).distance` returns the minimum distance from any entity
 * returned by `query` to the point `(1, 2, 3) meters`.
 * @example `result = evDistance(context, { "side0" : qEverything(EntityType.VERTEX), "side1" : qEverything(EntityType.VERTEX), "maximum" : true })`
 * computes the pair of vertices farthest apart.  `qNthElement(qEverything(EntityType.VERTEX), result.sides[0].index)` queries for one of these vertices.
 * @param context {Context}
 * @param arg {{
 *      @field side0 : One of the following: A query, or a point (3D Length Vector), or a `Line`, or an array of points, or an array of `Line`s.
 *          @eg `qNthElement(qEverything(EntityType.FACE), 0)` or `vector(1, 2, 3) * meter` or `line(vector(1, 0, 1) * meter, vector(1, 1, 1)`
 *      @field extendSide0 {boolean} : If `true` and side0 is a query, bodies will be ignored and edges and faces extended to
 *          their possibly infinite underlying surfaces.  Defaults to `false`. @optional
 *      @field side1 : Like `side0`.
 *          @autocomplete `vector(0, 0, 0) * meter`
 *      @field extendSide1 {boolean} : Like `extendSide0`. @optional
 *      @field maximum {boolean} : If `true`, compute the maximum instead of the minimum.  Defaults to `false`.
 *          Not allowed to be `true` if a line is passed in in either side or if either `extend` is true. @optional
 * }}
 */
export function evDistance(context is Context, arg is map) returns DistanceResult
{
    var result = @evDistance(context, arg);
    result.distance *= meter;
    for (var side in [0, 1])
    {
        result.sides[side].point = vector(result.sides[side].point) * meter;
        if (result.sides[side].parameter is array)
        {
            result.sides[side].parameter = result.sides[side].parameter as Vector;
        }
        else
        {
            var argSide = arg["side" ~ side];
            if (argSide is Line || (argSide is array && argSide[result.sides[side].index] is Line))
                result.sides[side].parameter *= meter;
        }
    }
    return result as DistanceResult;
}

// =========== end of evDistance stuff ===========

/**
 * Return the convexity type of the given edge,
 * `CONVEX`, `CONCAVE`, `SMOOTH`, or `VARIABLE`.
 * If the edge is part of a body with inside and outside
 * convex and concave have the obvious meanings.
 * Throws an exception if the query does not evaluate to a single edge.
 * @param context
 * @param arg {{
 *      @field edge{Query}
 * }}
 */
export function evEdgeConvexity(context is Context, arg is map) returns EdgeConvexityType
precondition
{
    arg.edge is Query;
}
{
    return @evEdgeConvexity(context, arg) as EdgeConvexityType;
}

/**
 * Return one tangent line to an edge.
 * @param arg {{
 *      @field edge {Query}: The curve to use @eg `qNthElement(qEverything(EntityType.EDGE), 1)`
 *      @field parameter {number}:
 *             A number in the range 0..1 indicating a point along
 *             the curve to evaluate the tangent at.
 *      @field arcLengthParameterization :
 *             If true (default), the parameter measures distance
 *             along the edge, so `0.5` is the midpoint.
 *             If false, use an arbitrary but faster-to-evaluate parameterization.
 *             For efficiency, use false if calculating the tangent only to an end point of the edge
 *             because the result will be identical.
 *          @optional
 * }}
 */
export function evEdgeTangentLine(context is Context, arg is map) returns Line
{
    arg.parameters = [arg.parameter];
    return evEdgeTangentLines(context, arg)[0];
}

/**
 * Return tangent lines to a edge.
 * @param arg {{
 *      @field edge {Query}: The curve to use @eg `qNthElement(qEverything(EntityType.EDGE), 1)`
 *      @field parameters {array}:
 *             An array of numbers in the range 0..1 indicating points along
 *             the curve to evaluate tangents at.
 *      @field arcLengthParameterization :
 *             If true (default), the parameter measures distance
 *             along the edge, so `0.5` is the midpoint.
 *             If false, use an arbitrary but faster-to-evaluate parameterization.
 *             For efficiency, use false if calculating the tangent only to an end point of the edge
 *             because the result will be identical.
 *          @optional
 * }}
 * @returns {array} : An array of `Line`s.
 */
export function evEdgeTangentLines(context is Context, arg is map) returns array
precondition
{
    arg.edge is Query;
    arg.parameters is array;
    if (arg.arcLengthParameterization != undefined)
        arg.arcLengthParameterization is boolean;

    for (var i in arg.parameters)
        i is number;
}
{
    if (arg.arcLengthParameterization == undefined)
        arg.arcLengthParameterization = true;

    var result = @evEdgeTangentLines(context, arg);
    for (var i = 0; i < @size(result); i += 1)
        result[i] = lineFromBuiltin(result[i]);
    return result;
}

/**
 * Given a face, calculate and return a Plane tangent to that face,
 * where the plane's origin is at the point specified by its parameter-space coordinates.
 * @param context {Context}
 * @param arg {{
 *      @field face {Query}: The face to evaluate
 *          @eg `qNthElement(qEverything(EntityType.FACE), 1)`
 *      @field parameter {Vector}: 2d unitless parameter-space vector specifying the location of tangency on the face.  The coordinates are relative to the parameter-space bounding box of the face.
 *          @eg `vector(0.5, 0.5)` places the origin at the bounding box's center.
 * }}
 */
export function evFaceTangentPlane(context is Context, arg is map) returns Plane
{
    arg.parameters = [arg.parameter];
    return evFaceTangentPlanes(context, arg)[0];
}

/**
 * Given a face, calculate and return an array of Planes tangent to that face,
 * where each plane's origin is located at the point specified by its parameter-space coordinates.
 * @param context {Context}
 * @param arg {{
 *      @field face {Query}: The face to evaluate
 *          @eg `qNthElement(qEverything(EntityType.FACE), 1)`
 *      @field parameters {array}: an array of 2d unitless parameter-space vectors specifying locations of tangency on the face.  The coordinates are relative to the parameter-space bounding box of the face.
 *          @eg `[ vector(0.5, 0.5), vector(0, 1) ]`
 * }}
 */
export function evFaceTangentPlanes(context is Context, arg is map) returns array
precondition
{
    arg.face is Query;
    arg.parameters is array;
    for (var uv in arg.parameters)
    {
        uv is Vector;
        @size(uv) == 2;
    }
}
{
    var result = @evFaceTangentPlanes(context, arg);
    for (var i = 0; i < @size(result); i += 1)
        result[i] = planeFromBuiltin(result[i]);
    return result;
}

/**
 * Given a face of a fillet, return the radius of the fillet.
 * @param arg {{
 *      @field face{Query}
 * }}
 */
export function evFilletRadius(context is Context, arg is map) returns ValueWithUnits
precondition
{
    arg.face is Query;
}
{
    return @evFilletRadius(context, arg) * meter;
}

/**
 * Return the total length of all the entities (if they are edges)
 * and edges belonging to entities (if they are bodies).  If no edges
 * are found the total length will be zero.
 * @param arg {{
 *      @field entities{Query}
 * }}
 */
export function evLength(context is Context, arg is map) returns ValueWithUnits
precondition
{
    arg.entities is Query;
}
{
    var edges = qEntityFilter(arg.entities, EntityType.EDGE);
    var ownedEdges = qOwnedByBody(arg.entities, EntityType.EDGE);
    return @evLength(context, { "edges" : qUnion([edges, ownedEdges]) }) * meter;
}

/**
 * If the edge is a line, return a Line value for the given edge.
 * @param arg {{
 *      @field edge{Query}
 * }}
 */
export function evLine(context is Context, arg is map) returns Line
precondition
{
    arg.edge is Query;
}
{
    return lineFromBuiltin(@evLine(context, arg));
}

/**
 * Gets the coordinate system of the given mate connector
 * @param context
 * @param arg {{
 *      @field mateConnector{Query} : The mate connector to evaluate.
 * }}
 */
export function evMateConnector(context is Context, arg is map) returns CoordSystem
{
    return coordSystemFromBuiltin(@evMateConnector(context, arg));
}

/**
 * @internal
 *
 * Given the picks and inferences for defining a mate connector, returns the desired coordinate system.
 */
export function evMateConnectorCoordSystem(context is Context, arg is map) returns CoordSystem
{
    return coordSystemFromBuiltin(@evMateConnectorCoordSystem(context, arg));
}

/**
 * Return the plane of the sketch that created the given entity. Throws if the entity was not created by a sketch.
 * @param context
 * @param arg {{
 *      @field entity {Query} : The sketch entity. May be a vertex, edge, face, or body.
 * }}
 */
export function evOwnerSketchPlane(context is Context, arg is map) returns Plane
precondition
{
    arg.entity is Query;
}
{
    return planeFromBuiltin(@evOwnerSketchPlane(context, arg));
}

/**
 * If the face is a plane, return a `Plane` value for the given face.
 * @param arg {{
 *      @field face{Query}
 * }}
 */
export function evPlane(context is Context, arg is map) returns Plane
precondition
{
    arg.face is Query;
}
{
    return planeFromBuiltin(@evPlane(context, arg));
}

/**
 * Return a descriptive value for a face, or the first face if the query
 * finds more than one.  Return a `Cone`, `Cylinder`, `Plane`, `Sphere`,
 * or `Torus` as appropriate for the face, or an unspecified map value
 * if the face is none of these.
 *
 * If the first result is not a face, throw an exception.
 * @param arg {{
 *      @field face{Query}
 * }}
 */
export function evSurfaceDefinition(context is Context, arg is map) returns map
precondition
{
    arg.face is Query;
}
{
    var result = @evSurfaceDefinition(context, arg);
    if (result is map)
    {
        if (result.surfaceType == (SurfaceType.CYLINDER as string))
        {
            result = cylinderFromBuiltin(result);
        }
        else if (result.surfaceType == (SurfaceType.CONE as string))
        {
            result = coneFromBuiltin(result);
        }
        else if (result.surfaceType == (SurfaceType.TORUS as string))
        {
            result = torusFromBuiltin(result);
        }
        else if (result.surfaceType == (SurfaceType.SPHERE as string))
        {
            result = sphereFromBuiltin(result);
        }
        else if (result.surfaceType == (SurfaceType.PLANE as string))
        {
            result = planeFromBuiltin(result);
        }
    }

    return result;
}

/**
 * Return the coordinates of a point.
 * @param arg {{
 *      @field vertex {Query}
 * }}
 */
export function evVertexPoint(context is Context, arg is map) returns Vector
precondition
{
    arg.vertex is Query;
}
{
    return meter * vector(@evVertexPoint(context, arg));
}

/**
 * Return the total volume of all the entities.
 * If no matching 3D bodies are found, the total volume will be zero.
 * @param arg {{
 *      @field entities{Query}
 * }}
 */
export function evVolume(context is Context, arg is map) returns ValueWithUnits
precondition
{
    arg.entities is Query;
}
{
    return @evVolume(context, { "bodies" : qEntityFilter(arg.entities, EntityType.BODY) }) * meter ^ 3;
}

// ========================= Internal stuff follows ==========================

/**
 * @internal
 */
export type OffsetGroup typecheck isOffsetGroup;

/**
 * @internal
 */
export predicate isOffsetGroup(value)
{
    value is map;
    value.side0 is array;
    value.side1 is array;
    size(value.side0) == size(value.side1);
    isLength(value.offsetLow);
    isLength(value.offsetHigh);
}

/**
 * @internal
 * Detects pairs of offset faces in the bodies
 */
export function evOffsetDetection(context is Context, definition is map) returns array
precondition
{
    definition.bodies is Query;
    definition.offsetTolerance is undefined || isLength(definition.offsetTolerance);
}
{
    var groups = @evOffsetDetection(context, definition);
    var out = [];
    for (var group in groups)
    {
        out = append(out, offsetGroup(group));
    }
    return out;
}

function offsetGroup(group is map) returns OffsetGroup
{
    var n = size(group.side0);
    if (size(group.side1) != n)
    {
        throw "Sides should be same length";
    }
    var side0 = makeArray(n);
    var side1 = makeArray(n);
    for (var i = 0; i < n; i += 1)
    {
        side0[i] = qTransient(group.side0[i] as TransientId);
        side1[i] = qTransient(group.side1[i] as TransientId);
    }
    return {'side0' : side0, 'side1' : side1,
        'offsetLow' : group.offsetLow * meter, 'offsethHigh' : group.offsetHigh * meter} as OffsetGroup;
}


