export import(path : "onshape/std/lineplane.fs", version : "");
export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/print.fs", version : "");

export function evFaceTangentPlane(context is Context, arg is map) returns map
{
    arg.parameters = [arg.parameter];
    var result = evFaceTangentPlanes(context, arg);
    if(result.result is array)
        result.result = result.result[0];
    return result;
}

export function evFaceTangentPlanes(context is Context, arg is map) returns map
precondition
{
    arg.face is Query;
    arg.parameters is array;
    for(var uv in arg.parameters)
    {
        uv is Vector;
        @size(uv) == 2;
    }
}
{
    var result = @evFaceTangentPlanes(context, arg);
    if(result.result is array)
    {
        for(var i = 0; i < @size(result.result); i += 1)
            result.result[i] = planeFromBuiltin(result.result[i]);
    }
    return result;
}

export function evEdgeTangentLine(context is Context, arg is map) returns map
{
    arg.parameters = [arg.parameter];
    var result = evEdgeTangentLines(context, arg);
    if(result.result is array)
        result.result = result.result[0];
    return result;
}

export function evEdgeTangentLines(context is Context, arg is map) returns map
precondition
{
    arg.edge is Query;
    arg.parameters is array;
    if(arg.arcLengthParameterization != undefined)
        arg.arcLengthParameterization is boolean;

    for(var i in arg.parameters)
        i is number;
}
{
    if(arg.arcLengthParameterization == undefined)
        arg.arcLengthParameterization = true;

    var result = @evEdgeTangentLines(context, arg);
    if(result.result is array)
    {
        for(var i = 0; i < @size(result.result); i += 1)
            result.result[i] = lineFromBuiltin(result.result[i]);
    }
    return result;
}

export function evVertexPoint(context is Context, arg is map) returns map
precondition
{
    arg.vertex is Query;
}
{
    var result = @evVertexPoint(context, arg);
    if(result.result is array)
        result.result = meter * vector(result.result);
    return result;
}

export function evLine(context is Context, arg is map) returns map
precondition
{
    arg.edge is Query;
}
{
    var result = @evLine(context, arg);
    if(result.result is map)
        result.result = lineFromBuiltin(result.result);
    return result;
}

export function evPlane(context is Context, arg is map) returns map
precondition
{
    arg.face is Query;
}
{
    var result = @evPlane(context, arg);
    if(result.result is map)
        result.result = planeFromBuiltin(result.result);
    return result;
}

export function evAxis(context is Context, arg is map) returns map
precondition
{
    arg.axis is Query;
}
{
    var result = @evAxis(context, arg);
    if(result.result is map)
        result.result = lineFromBuiltin(result.result);
    return result;
}

// Project point on curve, will return the parameter linearly scaled based on the interval of the edge. So parameter interval of the edge is
// always [0, 1]. Parameter output can be negative or greater than 1 (project on curve outside of the edge).
export function evProjectPointOnCurve(context is Context, arg is map) returns map
precondition
{
    arg.edge is Query;
    arg.vertex is Query;
}
{
    var result = @evProjectPointOnCurve(context, arg);
    return result;
}

export function evOwnerSketchPlane(context is Context, arg is map) returns map
precondition
{
    arg.entity is Query;
}
{
    var result = @evOwnerSketchPlane(context, arg);
    if(result.result is map)
        result.result = planeFromBuiltin(result.result);
    return result;
}

export function evLength(context is Context, arg is map) returns ValueWithUnits
precondition
{
    arg.entities is Query;
}
{
    var edges = qEntityFilter(arg.entities, EntityType.EDGE);
    var ownedEdges = qOwnedByPart(arg.entities, EntityType.EDGE);
    return @evLength(context, {"edges" : qUnion([edges, ownedEdges])}).result * meter;
}

export function evArea(context is Context, arg is map) returns ValueWithUnits
precondition
{
    arg.entities is Query;
}
{
    var faces = qEntityFilter(arg.entities, EntityType.FACE);
    var ownedFaces = qOwnedByPart(arg.entities, EntityType.FACE);
    return @evArea(context, {"faces" : qUnion([faces, ownedFaces])}).result * meter ^ 2;
}

export function evVolume(context is Context, arg is map) returns ValueWithUnits
precondition
{
    arg.entities is Query;
}
{
    return @evVolume(context, {"bodies" : qEntityFilter(arg.entities, EntityType.BODY)}).result * meter ^ 3;
}


export function evCollision(context is Context, arg is map)
precondition
{
    arg.tools is Query;
    arg.targets is Query;
    if( arg.passOwners != undefined)
        arg.passOwners is boolean;
}
{
    if(arg.passOwners == undefined)
        arg.passOwners = false;

    var result = @evCollisionDetection(context, {"tools" : arg.tools , "targets" : arg.targets, "owners" : arg.passOwners});
    var collisions = result.result;
    if (collisions is array)
    {
        for (var i = 0; i < size(collisions); i += 1)
        {
            for (var entry in collisions[i])
            {
                if (entry.value is builtin)
                {
                    collisions[i][entry.key] = qTransient(entry.value as TransientId);
                }
            }
        }
    }
    result.result = collisions;
    return result;

}

export type Box3d typecheck canBeBox3d;
export predicate canBeBox3d(value)
{
    value is map;
    isLengthVector(value.minCorner);
    isLengthVector(value.maxCorner);
}

export function box3d(minCorner is Vector, maxCorner is Vector) returns Box3d
{
    return {'minCorner' : minCorner, 'maxCorner' : maxCorner} as Box3d;
}

export function evBox3d(context is Context, arg is map) returns map
precondition
{
    arg.topology is Query;
    arg.cSys == undefined ||
    arg.cSys is Transform;
}
{
    var result = @evBox(context, arg);
    if (result.error == undefined)
        result.result = box3d(meter * vector(result.result.minCorner) , meter * vector(result.result.maxCorner));
    return result;
}

export function evMateConnectorTransform(context is Context, arg is map) returns map
{
    var result = @evMateConnectorTransform(context, arg);
    if (result.error == undefined)
    {
        result.result = transformFromBuiltin(result.result);
    }
    return result;
}

