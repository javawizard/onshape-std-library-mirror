FeatureScript ✨; /* Automatically generated version */
/**
 * Evaluation functions return information about the topological entities in the context, like bounding boxes, tangent
 * planes, projections, and collisions. Evaluation functions typically take a context and a map that specifies the
 * computation to be performed and return a ValueWithUnits or a FeatureScript geometry type (like Line or Plane). They
 * may also throw errors if a query fails to evaluate or the input is otherwise invalid.
 */
import(path : "onshape/std/box.fs", version : "");
import(path : "onshape/std/clashtype.gen.fs", version : "");
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/context.fs", version : "");
import(path : "onshape/std/coordSystem.fs", version : "");
import(path : "onshape/std/curveGeometry.fs", version : "");
import(path : "onshape/std/edgeconvexitytype.gen.fs", version : "");
import(path : "onshape/std/mathUtils.fs", version : "");
import(path : "onshape/std/query.fs", version : "");
import(path : "onshape/std/string.fs", version : "");
import(path : "onshape/std/surfaceGeometry.fs", version : "");
import(path : "onshape/std/units.fs", version : "");

/**
 * Given a face, calculate and return a Plane tangent to that face with an
 * origin, a normal, and an x axis.
 * @param context {Context}
 * @param arg {{
 *      @field face {Query}: The face to evaluate
 *          @eg `qNthElement(qEverything(EntityType.FACE), 1)`
 *      @field parameter {Vector}: 2d vector specifying the plane's origin's offset, relative to the given edge's bounding box.
 *          @eg `vector(0.5, 0.5)` places the origin at the bounding box's center.
 * }}
 * @returns {Plane}
 */
export function evFaceTangentPlane(context is Context, arg is map) returns Plane
{
    arg.parameters = [arg.parameter];
    return evFaceTangentPlanes(context, arg)[0];
}

/**
 * Same as `evFaceTangentPlane`, but input is an array of points
 * instead of one point and result is an array of planes instead of
 * one Plane.
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
 * Return one tangent to an edge.
 * @see `evEdgeTangentLines`
 */
export function evEdgeTangentLine(context is Context, arg is map) returns Line
{
    arg.parameters = [arg.parameter];
    return evEdgeTangentLines(context, arg)[0];
}

/**
 * Return tangents to a line.
 * @param arg {{
 *      @field edge {Query}: The line to use @eg `qNthElement(qEverything(EntityType.EDGE), 1)`
 *      @field parameter {array}:
 *             An array of numbers in the range 0..1 indicating points along
 *             the line to evaluate tangents at.
 *      @field arcLengthParameterization :
 *             If true (default), the parameter measures distance
 *             along the edge, so `0.5` is the midpoint.
 *             If false, use an arbitrary but faster-to-evaluate parameterization.
 *          @optional
 * }}
 * @returns {array} : array of `Line`
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
 * If the face is a planem, return a Plane value for the given face.
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
 * Project a point onto a curve.  The result is in curve parameter space,
 * so in [0, 1] if the point projects onto the curve and negative or
 * greater than 1 if the point projects onto an extension of the curve.
 * @param arg {{
 *      @field edge{Query} : The curve to project onto
 *      @field vertex{Query} : The point to project
 * }}
 */
export function evProjectPointOnCurve(context is Context, arg is map) returns number
precondition
{
    arg.edge is Query;
    arg.vertex is Query;
}
{
    return @evProjectPointOnCurve(context, arg);
}

/**
 * Project point onto face.  The result is an array of two numbers
 * in parameter space of the face, range [0, 1].  The result will
 * not be outside of the face (unlike `evProjectPointOnCurve`).
 * @param arg {{
 *      @field face{Query}
 *      @field point
 * }}
 */
export function evProjectPointOnFace(context is Context, arg is map) returns array
precondition
{
    arg.face is Query;
    is3dLengthVector(arg.point);
}
{
    return @evProjectPointOnFace(context, arg);
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
 * Return the convexity type of the given edge,
 * `CONVEX`, `CONCAVE`, `SMOOTH`, or `VARIABLE`.
 * If the edge is part of a body with inside and outside
 * convex and concave have the obvious meanings.
 * Throws an exception if the query does not evaluate to a single edge.
 * @param context
 * @param arg {{
 *      @field edge
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
 * For internal use.  Given the picks and inferences for defining a mate connector, returns the desired coordinate system.
 */
export function evMateConnectorCoordSystem(context is Context, arg is map) returns CoordSystem
{
    return coordSystemFromBuiltin(@evMateConnectorCoordSystem(context, arg));
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


