FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/**
 * Evaluation functions return information about the topological entities in the context, like bounding boxes, tangent
 * planes, projections, and collisions. Evaluation functions take a context and a map that specifies the
 * computation to be performed and return a ValueWithUnits, a FeatureScript geometry type (like [Line] or [Plane]), or a special
 * type like [DistanceResult]. They may also throw errors if a query fails to evaluate or the input is otherwise invalid.
 */
export import(path : "onshape/std/box.fs", version : "✨");
export import(path : "onshape/std/clashtype.gen.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/context.fs", version : "✨");
import(path : "onshape/std/coordSystem.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
export import(path : "onshape/std/edgeconvexitytype.gen.fs", version : "✨");
import(path : "onshape/std/mathUtils.fs", version : "✨");
import(path : "onshape/std/query.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");
export import(path : "onshape/std/smcornertype.gen.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/units.fs", version : "✨");

/**
 * Find the centroid of an entity or group of entities. This is
 * equivalent to the center of mass for a constant density object.
 * Warning: This is an approximate value and it is not recommended to use this
 * for modeling purposes that will be negatively affected in case the
 * approximation changes. Consider using the center of a bounding box
 * instead.
 * @param arg {{
 *      @field entities {Query} : The entities to take the center of mass of.
 * }}
 */
export function evApproximateCentroid(context is Context, arg is map) returns Vector
precondition
{
    arg.entities is Query;
}
{
    var centroid = @evApproximateCentroid(context, { "entities" : arg.entities });

    return vector(centroid) * meter;
}

/**
 * The result of an [evApproximateMassProperties] call.
 *
 * @type {{
 *      @field mass {ValueWithUnits} : The total mass.
 *      @field centroid {Vector} : The center of mass with respect to the given reference frame, or with respect to the origin if a reference frame is not specified.
 *      @field inertia {MatrixWithUnits} : The 3D inertia tensor, with units of mass * length ^ 2. Evaluated with respect to the reference frame, or with respect to the centroid if a reference frame is not specified.
 *      @field volume {ValueWithUnits} : Total volume. Only returned for solid bodies.
 *      @field area {ValueWithUnits} : Total area. Only returned for faces.
 *      @field length {ValueWithUnits} : Total length. Only returned for edges.
 *      @field count {number} : Total count. Only returned for vertices.
 * }}
 */
export type MassProperties typecheck canBeMassProperties;

/** @internal */
export predicate canBeMassProperties(value)
{
    value is map;
    value.mass is ValueWithUnits && value.mass.unit == MASS_UNITS;
    value.volume == undefined || (value.volume is ValueWithUnits && value.volume.unit == VOLUME_UNITS);
    value.area == undefined || (value.area is ValueWithUnits && value.area.unit == AREA_UNITS);
    value.length == undefined || (value.length is ValueWithUnits && value.length.unit == LENGTH_UNITS);
    value.count == undefined || value.count is number;
    value.centroid is Vector && value.centroid[0].unit == LENGTH_UNITS;
    value.inertia is MatrixWithUnits && value.inertia.unit == INERTIA_UNITS;
}

/**
 * Calculates approximate mass properties of an entity or group of entities.
 * Returns mass, centroid, inertia tensor, and volume/area/length/count for
 * bodies/faces/edges/vertices, respectively.
 * Warning: These are approximate values and it is not recommended to
 * use them for modeling purposes that will be negatively affected in
 * case the approximation changes.
 * @param arg {{
 *      @field entities {Query} : The entities of which to compute mass properties. Only entities of the highest dimensionality will be considered.
 *      @field density {ValueWithUnits} : The density of the entities, with appropriate units.
 *          @eg `1 * kilogram / meter ^ 3` could be the density of 3D solid bodies
 *          @eg `1 * kilogram / meter ^ 2` could be the density of 2D faces or sheet bodies
 *          @eg `1 * kilogram / meter` could be the density of 1D edges or wire bodies
 *          @eg `1 * kilogram` could be the mass of each 0D vertex or point body
 *      @field referenceFrame {CoordSystem} : Optional coordinate system. Defaults to the centroid with world axes for the inertia tensor, and world coordinates for the centroid. @optional
 * }}
 */
export function evApproximateMassProperties(context is Context, arg is map) returns MassProperties
precondition
{
    arg.entities is Query;
    arg.density is ValueWithUnits;
    arg.referenceFrame == undefined || arg.referenceFrame is CoordSystem;
}
{
    var massProperties = @evApproximateMassProperties(context, { "entities" : arg.entities, "referenceFrame" : arg.referenceFrame });
    var density = arg.density;
    var highestDimension = massProperties.highestDimension;

    if (highestDimension == 0)
    {
        if (density.unit != MASS_UNITS)
            throw "Density for a 0-dimensional entity must have units of mass.";
    }
    else
    {
        if ((density * meter ^ highestDimension).unit != MASS_UNITS)
            throw "Density for a " ~ highestDimension ~ "-dimensional entity must have units of mass / length^" ~ highestDimension ~ ".";
    }

    var result = {};

    if (highestDimension == 3)
    {
        result.volume = massProperties.volume * meter ^ 3;
        result.mass = density * result.volume;
    }
    else if (highestDimension == 2)
    {
        result.area = massProperties.area * meter ^ 2;
        result.mass = density * result.area;
    }
    else if (highestDimension == 1)
    {
        result.length = massProperties.length * meter;
        result.mass = density * result.length;
    }
    else if (highestDimension == 0)
    {
        result.count = massProperties.count;
        result.mass = density * result.count;
    }

    result.centroid = vector(massProperties.centroid) * meter;
    result.inertia = (massProperties.inertia as Matrix) * meter ^ (highestDimension + 2) * density;

    return result as MassProperties;
}

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
    var ownedFaces = qOwnedByBody(qFlattenedCompositeParts(arg.entities), EntityType.FACE);
    return @evArea(context, { "faces" : qUnion([faces, ownedFaces]) }) * meter ^ 2;
}

/**
 * If the query finds one entity with an axis -- a line, circle,
 * plane, cylinder, cone, sphere, torus, mate connector, or revolved surface -- return
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
 * to a given coordinate system. There is also an option to use
 * a faster but less accurate method.
 * @param arg {{
 *      @field topology{Query} : The entity to find the bounding box of.
 *      @field cSys{CoordSystem} : The coordinate system to use (if not the standard coordinate system). @optional
 *      @field tight{boolean} : Get the tightest possible bounding box. Defaults to `true`.
 *              @eg `true`for a bounding box precisely at the extents of the given entities (and no bigger).
 *              @eg `false` for a bounding box at least as big as the given entities, using a faster algorithm.
 *              @optional
 * }}
 */
export function evBox3d(context is Context, arg is map) returns Box3d
precondition
{
    arg.topology is Query;
    arg.cSys == undefined || arg.cSys is CoordSystem;
    arg.tight == undefined || arg.tight is boolean;
}
{
    var result = @evBox(context, arg);
    return box3d(meter * vector(result.minCorner), meter * vector(result.maxCorner));
}

/**
 * Find collisions between tools and targets.  Each collision is a
 * map with field `type` of type [ClashType] and fields `target`,
 * `targetBody`, `tool`, and `toolBody` of type [Query].
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
        collisions[i] = {
                            'type' : collisions[i]['type'] as ClashType,
                            'target' : qTransient(collisions[i]['target']),
                            'targetBody' : qTransient(collisions[i]['targetBody']),
                            'tool' : qTransient(collisions[i]['tool']),
                            'toolBody' : qTransient(collisions[i]['toolBody'])
                        };
    }
    return collisions;
}

/**
 * Return the type of corner found at a vertex of a sheet metal model
 * @param context
 * @param arg {{
 *      @field vertex{Query}
 * }}
 * @throws {GBTErrorStringEnum.BAD_GEOMETRY} : The query does not evaluate to a single vertex
 * @returns {{
  *      @field cornerType {SMCornerType} : the type of the corner
  *      @field primaryVertex {Query} : the vertex that defines the corner
  *      @field allVertices {array} : array of transient queries for all definition vertices associated with the corner
  * }}
 */
export function evCornerType(context is Context, arg is map) returns map
precondition
{
    arg.vertex is Query;
}
{
    var data = @evCornerType(context, arg);

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V488_CLASSIFY_CORNER_RETURNS_MAP))
    {
        var allVertices = [];
        if (data.allVertices != undefined)
        {
            for (var vert in data.allVertices)
            {
                allVertices = append(allVertices, qTransient(vert));
            }
        }
        return {
            "cornerType" : data.cornerType as SMCornerType,
            "primaryVertex" : qTransient(data.primaryVertex),
            "allVertices" : allVertices
        };
    }
    else
    {
        return {
            "cornerType" : data as SMCornerType,
            "primaryVertex" : arg.vertex
        };
    }
}

/**
 * Given a query for a curve, return a [Circle], [Ellipse], or [Line]
 * value for the curve.  If the curve is none of these types, return
 * a map with unspecified contents.
 * @param arg {{
 *      @field edge{Query} : The curve to evaluate.
 * }}
 * @throws {GBTErrorStringEnum.INVALID_INPUT} : The first resolved entity was not an edge.
 * @throws {GBTErrorStringEnum.CANNOT_RESOLVE_ENTITIES} : Input entities are invalid or there are no input entities.
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
 * The result of an [evDistance] call -- information about the extremal distance and the attaining point / line / entity.
 *
 * @type {{
 *      @field distance {ValueWithUnits} : The minimal or maximal distance.
 *      @field sides {array} : An array of 2 maps, containing information about where the extremum was found for each side.  Each map has a:
 *
 *          `point` (Vector) : The position in world space that is closest or farthest to the other side. The `distance` field is measured between the two values of `point`.
 *
 *          `index` (integer) :  the index into the line or point array or into the query results, if a query is passed in.
 *
 *          `parameter` (number or length or array of two numbers) : If the `index` refers to an edge,
 *                  the `parameter` is a number between 0 and 1 (unless `extend` for that side was passed in).  It is in the form that
 *                  [evEdgeTangentLine] and [evEdgeCurvature] consume (with `arcLengthParameterization` set to the same value that was passed into [evDistance]).
 *
 *                  If the `index` refers to a point, the `parameter` is 0.
 *
 *                  If the `index` refers to a [Line], the `parameter` is a length representing the distance along the direction.
 *
 *                  If the `index` refers to a face, the `parameter` is a 2D [Vector] in the form that [evFaceTangentPlane] consumes. If this face is a mesh or a plane, the parameter is a 2D [Vector] of zeroes.
 *
 *                  If the `index` refers to a [Plane], the `parameter` is a 2D [Vector] representing the lengths along the plane's x and y axes.
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
        // The parameter is either one number (for a curve) or an array of two (for a surface) or a 2D length vector (for a plane). For bodies or points, the parameter is 0.
        // For lines, the parameter is a length representing the distance along the direction.
        if (!(sideResult.parameter is number || isLength(sideResult.parameter) || is2dPoint(sideResult.parameter)))
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
 * When the minimum or the maximum is not uniquely defined, ties will be broken arbitrarily.
 *
 * @example `evDistance(context, { "side0" : vector(1, 2, 3) * meter, "side1" : query }).distance`
 *          returns the minimum distance from any entity returned by `query` to the point `(1, 2, 3) meters`.
 *
 * @example `result = evDistance(context, { "side0" : qEverything(EntityType.VERTEX), "side1" : qEverything(EntityType.VERTEX), "maximum" : true })`
 *          computes the pair of vertices farthest apart. `qNthElement(qEverything(EntityType.VERTEX), result.sides[0].index)`
 *          queries for one of these vertices.
 *
 * @seealso [DistanceResult]
 *
 * @param context {Context}
 * @param arg {{
 *      @field side0 : One of the following: A query, or a point (3D Length Vector), or a [Line], or a [Plane], or an array of points, or an array of [Line]s, or an array of [Plane]s.
 *          @eg `qNthElement(qEverything(EntityType.FACE), 0)` or `vector(1, 2, 3) * meter` or `line(vector(1, 0, 1) * meter, vector(1, 1, 1)` or `plane(vector(1,1,1) * meter, vector(0,0,1), vector(1,0,0))`.
 *      @field extendSide0 {boolean} : If `true` and side0 is a query, bodies will be ignored and edges and faces extended to
 *          their possibly infinite underlying surfaces.  Defaults to `false`. @optional
 *      @field side1 : Like `side0`.
 *          @autocomplete `vector(0, 0, 0) * meter`
 *      @field extendSide1 {boolean} : Like `extendSide0`. @optional
 *      @field maximum {boolean} : If `true`, compute the maximum instead of the minimum.  Defaults to `false`.
 *          Not allowed to be `true` if a line is passed in in either side or if either `extend` is true. @optional
 *      @field arcLengthParameterization {boolean} :
 *             If true (default), the parameter returned for edges measures distance
 *             along the edge, so `0.5` is the midpoint.
 *             If false, use an arbitrary but faster-to-calculate parameterization.
 *             This field only controls the parameter returned for edges.  It does not control the
 *             parameter returned for points, [Line]s, faces, or [Plane]s.
 *          @optional
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

            var argSide = arg["side" ~ side];
            if (argSide is Plane || (argSide is array && argSide[result.sides[side].index] is Plane))
                result.sides[side].parameter *= meter;
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
 * Map containing the results of one collision between a ray and an entity.
 *
 * @type {{
 *      @field entity {Query} : A query for the entity hit by the ray.
 *      @field entityType {EntityType} : The type of the entity.
 *      @field parameter : Parameters for where the ray hit the entity. A unitless 2-vector for a face, a number for an edge, else undefined.
 *      @field intersection {Vector} : Intersection point.
 *      @field distance {ValueWithUnits} : Distance of the intersection point from the ray origin.
 * }}
 */
export type RaycastResult typecheck canBeRaycastResult;

/** @internal */
export predicate canBeRaycastResult(value)
{
    value is map;
    value.entity is Query;
    value.entityType is EntityType;
    if (value.entityType == EntityType.FACE)
        isUnitlessVector(value.parameter) && size(value.parameter) == 2;
    if (value.entityType == EntityType.EDGE)
        value.parameter is number;
    is3dLengthVector(value.intersection);
    value.distance is ValueWithUnits && value.distance.unit == LENGTH_UNITS;
}

/**
 * Detect intersections between a ray and the given entities.
 * @param arg {{
 *      @field entities{Query} : A query for target entities. If bodies are provided, the result will contain intersections for individual entities owned by the body.
 *      @field ray{Line} : The ray to intersect the entities. Because the passed-in `Line` is interpreted as a ray,
 *          by default, intersections with entities "behind" the ray origin are not detected. `includeIntersectionsBehind`
 *          can be set to `true` if those intersections are desired.
 *          @eg `line(vector(0, 0, 0) * inch, vector(1, 0, 0))` specifies the positive x-axis
 *      @field closest{boolean} : Get only the closest intersection with any of the entities. Defaults to `true`.
 *          @autocomplete `true`
 *          @optional
 *      @field includeIntersectionsBehind{boolean} : Return intersections that are behind the ray origin.
 *          Defaults to `false`. Cannot be set to `true` if `closest` is `true`.
 *          @optional
 * }}
 * @returns {array} : An array of [RaycastResult]s for each intersection in front of the ray, ordered from closest to farthest.
 */
export function evRaycast(context is Context, arg is map) returns array
precondition
{
    arg.entities is Query;
    arg.ray is Line;
    arg.closest == undefined || arg.closest is boolean;
    arg.includeIntersectionsBehind == undefined || arg.includeIntersectionsBehind is boolean;
}
{
    var data = @evRaycast(context, arg);
    var results = [];

    for (var i = 0; i < size(data); i += 1)
    {
        var result = {};

        result.entity = qTransient(data[i].entity);
        result.intersection = (data[i].intersection as Vector) * meter;
        result.distance = data[i].distance * meter;
        result.entityType = data[i].entityType as EntityType;
        if (result.entityType == EntityType.FACE)
        {
            result.parameter = data[i].parameter as Vector;
        }
        else if (result.entityType == EntityType.EDGE)
        {
            result.parameter = data[i].parameter;
        }

        results = append(results, result as RaycastResult);
    }

    return results;
}

/**
 * Return the convexity type of the given edge,
 * `CONVEX`, `CONCAVE`, `SMOOTH`, or `VARIABLE`.
 * If the edge is part of a body with inside and outside
 * convex and concave have the obvious meanings.
 * @param context
 * @param arg {{
 *      @field edge{Query}
 * }}
 * @throws {GBTErrorStringEnum.TOO_MANY_ENTITIES_SELECTED} : The query evaluates to more than one entity
 * @throws {GBTErrorStringEnum.BAD_GEOMETRY} : The query does not evaluate to a single edge.
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
 * The result of an [evEdgeCurvature] call -- a coordinate system for the Frenet frame and the curvature defined at a point
 *
 * @type {{
 *      @field frame {CoordSystem} : The frame. The Z vector is the tangent, the X vector is the normal and the Y vector is the binormal
 *      @field curvature {ValueWithUnits} : The curvature (inverse length units).
 * }}
 */
export type EdgeCurvatureResult typecheck canBeEdgeCurvatureResult;

predicate canBeEdgeCurvatureResult(value)
{
    value is map;
    value.curvature is ValueWithUnits;
    value.curvature.unit == ({ "meter" : -1 } as UnitSpec);
}

/**
 * Return a Frenet frame along an edge, with curvature.
 * If the curve has zero curvature at an evaluated point then the returned normal and binormal are arbitrary
 * and only the tangent is significant.
 *
 * @param arg {{
 *      @field edge {Query}: The curve to use @eg `qNthElement(qEverything(EntityType.EDGE), 1)`
 *      @field parameter {number}:
 *             A number in the range 0..1 indicating the point along the curve to evaluate the frame at.
 *      @field arcLengthParameterization {boolean} :
 *             If true (default), the parameter measures distance
 *             along the edge, so `0.5` is the midpoint.
 *             If false, use an arbitrary but faster-to-evaluate parameterization.
 *             The parameterization is identical to that used by [evEdgeTangentLines].
 *             Results obtained with arcLengthParameterization will have lower accuracy due to approximation.
 *          @optional
 *      @field face {Query} :
 *             If present, the edge orientation used is such that walking along the edge
 *             with "up" being the `face` normal will keep `face` to the left.
 *             Must be adjacent to `edge`.
 *          @optional
 * }}
 * @throws {GBTErrorStringEnum.NO_TANGENT_LINE} : A frame could not be calculated for the specified input.
 */
export function evEdgeCurvature(context is Context, arg is map) returns EdgeCurvatureResult
{
    arg.parameters = [arg.parameter];
    return evEdgeCurvatures(context, arg)[0];
}

/**
 * Return Frenet frames along an edge, with curvature.
 * If the curve has zero curvature at an evaluated point then the returned normal and binormal are arbitrary
 * and only the tangent is significant.
 *
 * @param arg {{
 *      @field edge {Query}: The curve to use @eg `qNthElement(qEverything(EntityType.EDGE), 1)`
 *      @field parameters {array}:
 *             An array of numbers in the range 0..1 indicating points along
 *             the curve to evaluate frames at.
 *      @field arcLengthParameterization {boolean} :
 *             If true (default), the parameter measures distance
 *             along the edge, so `0.5` is the midpoint.
 *             If false, use an arbitrary but faster-to-evaluate parameterization.
 *             The parameterization is identical to that used by [evEdgeTangentLines].
 *             Results obtained with arcLengthParameterization will have lower accuracy due to approximation.
 *          @optional
 *      @field face {Query} :
 *             If present, the edge orientation used is such that walking along the edge
 *             with "up" being the `face` normal will keep `face` to the left.
 *             Must be adjacent to `edge`.
 *          @optional
 * }}
 * @returns {array} : An array of [EdgeCurvatureResult]s.
 * @throws {GBTErrorStringEnum.NO_TANGENT_LINE} : A frame could not be calculated for the specified input.
 */
export function evEdgeCurvatures(context is Context, arg is map) returns array
precondition
{
    arg.edge is Query;
    arg.parameters is array;
    for (var i in arg.parameters)
        i is number;
}
{
    var results = @evEdgeCurvatures(context, arg);
    var resultsWithUnits = [];
    for (var result in results)
    {
        resultsWithUnits = append(resultsWithUnits, {
                        'curvature' : result.curvature / meter,
                        'frame' : coordSystemFromBuiltin(result.frame)
                    } as EdgeCurvatureResult);
    }
    return resultsWithUnits;
}

/**
 * Returns the tangent vector of a curvature frame
 * @returns {Vector} : A unit 3D vector in world space.
 */
export function curvatureFrameTangent(curvatureResult is EdgeCurvatureResult) returns Vector
{
    return curvatureResult.frame.zAxis;
}

/**
 * Returns the normal vector of a curvature frame
 * @returns {Vector} : A unit 3D vector in world space.
 */
export function curvatureFrameNormal(curvatureResult is EdgeCurvatureResult) returns Vector
{
    return curvatureResult.frame.xAxis;
}

/**
 * Returns the binormal vector of a curvature frame
 * @returns {Vector} : A unit 3D vector in world space.
 */
export function curvatureFrameBinormal(curvatureResult is EdgeCurvatureResult) returns Vector
{
    return yAxis(curvatureResult.frame);
}

/**
 * Return one tangent [Line] to an edge.
 * @param arg {{
 *      @field edge {Query}: The curve to use @eg `qNthElement(qEverything(EntityType.EDGE), 1)`
 *      @field parameter {number}:
 *             A number in the range 0..1 indicating a point along
 *             the curve to evaluate the tangent at.
 *      @field arcLengthParameterization {boolean} :
 *             If true (default), the parameter measures distance
 *             along the edge, so `0.5` is the midpoint.
 *             If false, use an arbitrary but faster-to-evaluate parameterization.
 *          @optional
 *      @field face {Query} :
 *             If present, the edge orientation used is such that walking along the edge
 *             with "up" being the `face` normal will keep `face` to the left.
 *             Must be adjacent to `edge`.
 *          @optional
 * }}
 * @throws {GBTErrorStringEnum.NO_TANGENT_LINE} : A tangent line could not be evaluated for the given query.
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
 *      @field arcLengthParameterization {boolean} :
 *             If true (default), the parameter measures distance
 *             along the edge, so `0.5` is the midpoint.
 *             If false, use an arbitrary but faster-to-evaluate parameterization.
 *          @optional
 *      @field face {Query} :
 *             If present, the edge orientation used is such that walking along the edge
 *             with "up" being the `face` normal will keep `face` to the left.
 *             Must be adjacent to `edge`.
 *          @optional
 * }}
 * @returns {array} : An array of [Line]s.
 * @throws {GBTErrorStringEnum.NO_TANGENT_LINE} : A tangent line could not be evaluated for the given query.
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
    var result = @evEdgeTangentLines(context, arg);
    for (var i = 0; i < @size(result); i += 1)
        result[i] = lineFromBuiltin(result[i]);
    return result;
}

/**
 * Return the periodicity in primary and secondary direction of a face, returned in an array of booleans.
 *
 * A particular direction is periodic when the face's underlying surface definition is wrapped along that direction.
 * For instance, if primary direction is periodic, the parameters `[0, v]` and `[1, v]` will prepresent the same point
 * for all valid `v`. If the secondary direction is periodic, the parameters `[u, 0]` and `[u, 1]` represent the same
 * point for all valid `u`.
 * @param arg {{
 *      @field face{Query} : The face on which to evaluate periodicity
 *      @field trimmed{boolean} : If `true` (default), return trimmed face periodicity instead of the underlying surface's. @optional
 * }}
 */
export function evFacePeriodicity(context is Context, arg is map) returns array
precondition
{
    arg.face is Query;
    arg.trimmed is boolean || arg.trimmed is undefined;
}
{
    return @evFacePeriodicity(context, arg);
}

/**
 * The result of an [evFaceCurvature] call -- principal directions and curvatures at a point.
 *
 * The curvature along a particular direction (in the tangent plane) is the inverse of the radius of curvature
 * in that direction.  This curvature is positive if the radius of curvature points away from the normal direction,
 * negative if it points along the normal direction, or zero if there is no curvature in that direction. The
 * principal curvatures at a point are the directions of minimal and maximal curvature along the surface at that
 * point.
 *
 * @type {{
 *      @field minCurvature {ValueWithUnits} : The smaller of the two principal curvatures (inverse length units).
 *      @field maxCurvature {ValueWithUnits} : The larger of the two principal curvatures (inverse length units).
 *      @field minDirection {Vector} : A 3D unit vector corresponding to `minCurvature`.
 *      @field maxDirection {Vector} : A 3D unit vector corresponding to `maxCurvature`.
 * }}
 */
export type FaceCurvatureResult typecheck canBeFaceCurvatureResult;

predicate canBeFaceCurvatureResult(value)
{
    value is map;
    value.minCurvature is ValueWithUnits;
    value.minCurvature.unit == ({ "meter" : -1 } as UnitSpec);
    value.maxCurvature is ValueWithUnits;
    value.maxCurvature.unit == ({ "meter" : -1 } as UnitSpec);
    is3dDirection(value.minDirection);
    is3dDirection(value.maxDirection);
    perpendicularVectors(value.minDirection, value.maxDirection);
}

function faceCurvatureResultFromBuiltin(builtinResult is map) returns FaceCurvatureResult
{
    builtinResult.minDirection = builtinResult.minDirection as Vector;
    builtinResult.maxDirection = builtinResult.maxDirection as Vector;
    builtinResult.minCurvature /= meter;
    builtinResult.maxCurvature /= meter;
    return builtinResult as FaceCurvatureResult;
}

/**
 * Given a face, calculate and return principal curvatures at a point on that face,
 * specified by its parameter-space coordinates.
 *
 * @example ```
 *  // Ellipsoid measuring 10in x 4in x 6in
 * fEllipsoid(context, id + "ellipsoid", {
 *             "center" : vector(0, 0, 0) * inch,
 *             "radius" : vector(5 * inch, 2 * inch, 3 * inch)
 *         });
 *
 * const ellipseFace = qCreatedBy(id + "ellipsoid", EntityType.FACE);
 * const topPoint = vector(0, 0, 3) * inch; // Point on top of ellipsoid
 * const distanceResult = evDistance(context, { // Closest position to topPoint on ellipseFace
 *             "side0" : ellipseFace,
 *             "side1" : topPoint
 *         });
 * var uvCoordinatesAtTopPoint = distanceResult.sides[0].parameter;
 *
 * var curvatureResult = evFaceCurvature(context, {
 *         "face" : ellipseFace,
 *         "parameter" : uvCoordinatesAtTopPoint
 *     });
 * //  curvatureResult is {
 * //      minCurvature: 3 * inch / (5 * inch)^2,
 * //      maxCurvature: 3 * inch / (2 * inch)^2,
 * //      minDirection: vector(1, 0, 0),
 * //      maxDirection: vector(0, 1, 0)
 * //  }
 * ```
 *
 * @param context {Context}
 * @param arg {{
 *      @field face {Query}: The face on which to evaluate the curvature. The face cannot be a mesh.
 *          @eg `qNthElement(qEverything(EntityType.FACE), 1)`
 *      @field parameter {Vector}: a 2d unitless parameter-space vector specifying the location on the face.
 *          The coordinates are relative to the parameter-space bounding box of the face.
 *          @eg `vector(0.5, 0.5)`
 * }}
 */
export function evFaceCurvature(context is Context, arg is map) returns FaceCurvatureResult
{
    arg.parameters = [arg.parameter];
    return evFaceCurvatures(context, arg)[0];
}

/**
 * Given a face, calculate and return an array of principal curvatures at points on that face,
 * specified by its parameter-space coordinates.
 * @param context {Context}
 * @param arg {{
 *      @field face {Query}: A single face on which to evaluate the curvatures. The face cannot be a mesh.
 *          @eg `qNthElement(qEverything(EntityType.FACE), 1)`
 *      @field parameters {array}: an array of 2d unitless parameter-space vectors specifying locations on the face.
 *          The coordinates are relative to the parameter-space bounding box of the face.
 *          @eg `[ vector(0.5, 0.5), vector(0, 1) ]`
 * }}
 * @returns {array} : An array of [FaceCurvatureResult]s.
 */
export function evFaceCurvatures(context is Context, arg is map) returns array
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
    var builtinResult = @evFaceCurvatures(context, arg);
    var result = [];
    for (var builtinResultItem in builtinResult)
        result = append(result, faceCurvatureResultFromBuiltin(builtinResultItem));
    return result;
}

/**
 * Return the surface normal of a face at a position on one of its edges.
 *
 * If the first result is not a face, throw an exception.
 * @param arg {{
 *      @field edge{Query}
 *      @field face{Query}
 *      @field parameter {number}:
 *             A number in the range 0..1 indicating a point along
 *             the edge to evaluate the tangent at.
 *      @field arcLengthParameterization {boolean} :
 *             If true (default), the parameter measures distance
 *             along the edge, so `0.5` is the midpoint.
 *             If false, use an arbitrary but faster-to-evaluate parameterization.
 *          @optional
 *      @field usingFaceOrientation{boolean}:
 *             If true, the edge orientation used is such that walking along the edge with "up" being the `face`
 *             normal will keep `face` to the left. If false, use the default orientation of the edge,
 *             which is the same orientation used by [evEdgeTangentLine]. Default is `false`.
 *          @optional
 * }}
 */
export function evFaceNormalAtEdge(context is Context, arg is map) returns Vector
{
    return evFaceTangentPlaneAtEdge(context, arg).normal;
}

/**
 * Return a [Plane] tangent to face at a position on one of its edges.
 *
 * If the first result is not a face, throw an exception.
 * @param arg {{
 *      @field edge{Query}
 *      @field face{Query}
 *      @field parameter {number}:
 *             A number in the range 0..1 indicating a point along
 *             the edge to evaluate the tangent at.
 *      @field arcLengthParameterization {boolean} :
 *             If true (default), the parameter measures distance
 *             along the edge, so `0.5` is the midpoint.
 *             If false, use an arbitrary but faster-to-evaluate parameterization.
  *          @optional
 *      @field usingFaceOrientation{boolean}:
 *             If true, the edge orientation used is such that walking along the edge with "up" being the `face`
 *             normal will keep `face` to the left. If false, use the default orientation of the edge,
 *             which is the same orientation used by [evEdgeTangentLine]. Default is `false`.
 *          @optional
 * }}
 */
export function evFaceTangentPlaneAtEdge(context is Context, arg is map) returns Plane
precondition
{
    arg is map;
    arg.edge is Query;
    arg.face is Query;
    arg.parameter is number;
    arg.arcLengthParameterization is undefined || arg.arcLengthParameterization is boolean;
    arg.usingFaceOrientation is undefined || arg.usingFaceOrientation is boolean;
}
{
    var edgeTangent = evEdgeTangentLine(context, {
            "edge" : arg.edge,
            "parameter" : arg.parameter,
            "face" : (arg.usingFaceOrientation == true) ? arg.face : undefined,
            "arcLengthParameterization" : arg.arcLengthParameterization
    });
    var distData = evDistance(context, {
            "side0" : arg.face,
            "side1" : edgeTangent.origin
    });
    var parameter = distData.sides[0].parameter;
    var faceTangent = evFaceTangentPlane(context, {
            "face" : arg.face,
            "parameter" : parameter
    });
    return faceTangent;
}

/**
 * Given a face, calculate and return a [Plane] tangent to that face,
 * where the plane's origin is at the point specified by its parameter-space coordinates.
 * @param context {Context}
 * @param arg {{
 *      @field face {Query}: The face to evaluate. The face cannot be a mesh.
 *          @eg `qNthElement(qEverything(EntityType.FACE), 1)`
 *      @field parameter {Vector}: 2d unitless parameter-space vector specifying the location of tangency on the face.  The coordinates are relative to the parameter-space bounding box of the face.
 *          @eg `vector(0.5, 0.5)` places the origin at the bounding box's center.
 * }}
 * @throws {GBTErrorStringEnum.NO_TANGENT_PLANE} : Could not find a tangent plane or there was a problem with face parameterization.
 */
export function evFaceTangentPlane(context is Context, arg is map) returns Plane
{
    arg.parameters = [arg.parameter];
    return evFaceTangentPlanes(context, arg)[0];
}

/**
 * Given a face, calculate and return an array of [Plane]s tangent to that face,
 * where each plane's origin is located at the point specified by its parameter-space coordinates.
 * @param context {Context}
 * @param arg {{
 *      @field face {Query}: The face to evaluate. The face cannot be a mesh.
 *          @eg `qNthElement(qEverything(EntityType.FACE), 1)`
 *      @field parameters {array}: an array of 2d unitless parameter-space vectors specifying locations of tangency on the face.  The coordinates are relative to the parameter-space bounding box of the face.
 *          @eg `[ vector(0.5, 0.5), vector(0, 1) ]`
 * }}
 * @throws {GBTErrorStringEnum.NO_TANGENT_PLANE} : Could not find a tangent plane or there was a problem with face parameterization.
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
 * Given a face of a constant radius fillet, return the radius of fillet.
 * @param arg {{
 *      @field face{Query}
 * }}
 * @throws {GBTErrorStringEnum.BAD_GEOMETRY} : The first resolved entity was not a filleted face.
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
    var ownedEdges = qOwnedByBody(qFlattenedCompositeParts(arg.entities), EntityType.EDGE);
    return @evLength(context, { "edges" : qUnion([edges, ownedEdges]) }) * meter;
}

/**
 * If the edge is a line, return a [Line] value for the given edge.
 * @param arg {{
 *      @field edge{Query}
 * }}
 * @throws {GBTErrorStringEnum.INVALID_INPUT} : The first resolved entity was not a line.
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
 * Return the plane of the sketch that created the given entity.
 * @param context
 * @param arg {{
 *      @field entity {Query} : The sketch entity. May be a vertex, edge, face, or body.
 * }}
 * @throws {GBTErrorStringEnum.CANNOT_RESOLVE_PLANE} : Entity was not created by a sketch.
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
 * If the face is a planar face or a mate connector, return the [Plane] it represents.
 * @param arg {{
 *      @field face{Query}
 * }}
 * @throws {GBTErrorStringEnum.CANNOT_RESOLVE_PLANE} : The first resolved entity was not a planar face or mate connector.
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
 * finds more than one.  Return a [Cone], [Cylinder], [Plane], [Sphere],
 * or [Torus] as appropriate for the face, or an unspecified map value
 * if the face is none of these with surfaceType filled of type SurfaceType
 * @param arg {{
 *      @field face{Query}
 * }}
 * @throws {"GBTErrorStringEnum.CANNOT_RESOLVE_PLANE"} : The first result is not a face.
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
        else if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V695_SM_SWEPT_SUPPORT))
        {
            result.surfaceType = result.surfaceType as SurfaceType;
        }
    }

    return result;
}

/**
 * Return the coordinates of a point, or the origin of a mate connector.
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
        side0[i] = qTransient(group.side0[i]);
        side1[i] = qTransient(group.side1[i]);
    }
    return {'side0' : side0, 'side1' : side1,
        'offsetLow' : group.offsetLow * meter, 'offsetHigh' : group.offsetHigh * meter} as OffsetGroup;
}

