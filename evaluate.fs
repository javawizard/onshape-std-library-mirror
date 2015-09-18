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
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
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
 * Given an edge, calculate and return a Line tangent to that edge with an
 * origin and a direction.
 * @param context {Context}
 * @param arg {{
 *      @field edge {Query}: @eg `qNthElement(qEverything(EntityType.EDGE), 1)`
 *      @field parameter {number}: Offset of the resulting line's origin, relative to the given edge.
 *          @eg `0.5` places the origin at the edge's midpoint
 *      @field arcLengthParameterization : TODO: document me
 *          @optional
 * }}
 * @returns {Line}
 */
export function evEdgeTangentLine(context is Context, arg is map) returns Line
{
    arg.parameters = [arg.parameter];
    return evEdgeTangentLines(context, arg)[0];
}

/**
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
 * }}
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
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
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
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
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
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
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
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
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
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
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

// Project point on curve, will return the parameter linearly scaled based on the interval of the edge. So parameter interval of the edge is
// always [0, 1]. Parameter output can be negative or greater than 1 (project on curve outside of the edge).
/**
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
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

// return a parameter value of the point projected onto the face, the param value is scaled by the face interval [[0, 1], [0, 1]]
// param return will not be outside the parameterization of the face, projection will always project to closest point on face, not underlying surface
/**
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
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
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
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
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
 * }}
 */
export function evLength(context is Context, arg is map) returns ValueWithUnits
precondition
{
    arg.entities is Query;
}
{
    var edges = qEntityFilter(arg.entities, EntityType.EDGE);
    var ownedEdges = qOwnedByPart(arg.entities, EntityType.EDGE);
    return @evLength(context, { "edges" : qUnion([edges, ownedEdges]) }) * meter;
}

/**
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
 * }}
 */
export function evArea(context is Context, arg is map) returns ValueWithUnits
precondition
{
    arg.entities is Query;
}
{
    var faces = qEntityFilter(arg.entities, EntityType.FACE);
    var ownedFaces = qOwnedByPart(arg.entities, EntityType.FACE);
    return @evArea(context, { "faces" : qUnion([faces, ownedFaces]) }) * meter ^ 2;
}

/**
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
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
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
 * }}
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
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
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
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
 * }}
 */
export function evMateConnectorTransform(context is Context, arg is map) returns Transform
{
    return transformFromBuiltin(@evMateConnectorTransform(context, arg));
}

/**
 * TODO: description
 * @param context
 * @param arg {{
 *      @field TODO
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


