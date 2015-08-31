FeatureScript 206; /* Automatically generated version */
export import(path : "onshape/std/geomOperations.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/manipulator.fs", version : "");
export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/edgeconvexitytype.gen.fs", version : "");

//Fillet feature
annotation { "Feature Type Name" : "Fillet", "Manipulator Change Function" : "filletManipulatorChange", "Filter Selector" : "allparts" }
export const fillet = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities to fillet",
                     "Filter" : ((EntityType.EDGE && EdgeTopology.TWO_SIDED) || EntityType.FACE) && ConstructionObject.NO && SketchObject.NO }
        definition.entities is Query;

        annotation { "Name" : "Radius" }
        isLength(definition.radius, BLEND_BOUNDS);

        annotation { "Name" : "Tangent propagation", "Default" : true }
        definition.tangentPropagation is boolean;

        annotation { "Name" : "Conic fillet" }
        definition.conicFillet is boolean;

        if (definition.conicFillet)
        {
            annotation { "Name" : "Rho" }
            isReal(definition.rho, FILLET_RHO_BOUNDS);
        }
    }
    {
        try(addFilletManipulator(context, id, definition));
        opFillet(context, id, definition);
    }, { tangentPropagation : false, conicFillet : false });

/*
 * Create a linear manipulator for the fillet
 */
function addFilletManipulator(context is Context, id is Id, definition is map)
{
    // get last last edge (or arbitrary edge of the last face) from the qlv
    var operativeEntity = try(findManipulationEntity(context, definition));
    if (operativeEntity != undefined)
    {
        // convert given radius and edge topology into origin, direction, and offset
        var origin = evEdgeTangentLine(context, { "edge" : operativeEntity, "parameter" : 0.5 }).origin;
        var normals = try(findSurfaceNormalsAtEdge(context, operativeEntity, origin));
        if (normals != undefined && !parallelVectors(normals[0], normals[1]))
        {
            var direction = normalize(normals[0] + normals[1]);

            var convexity = 1.0;
            var bounds = tightestBounds(BLEND_BOUNDS);
            var minDragValue = bounds[0];
            var maxDragValue = bounds[1];
            if (isEdgeConvex(context, operativeEntity))
            {
                convexity = -1.0;
                var tempMin = minDragValue;
                minDragValue = -maxDragValue;
                maxDragValue = -tempMin;
            }

            var offset = convexity * definition.radius * findRadiusToOffsetRatio(normals);

            addManipulators(context, id, {"filletRadiusManipulator" :
                            linearManipulator({ "base" : origin,
                                                "direction" : direction,
                                                "offset" : offset,
                                                "minValue" : minDragValue,
                                                "maxValue" : maxDragValue }) });
        }
    }
}

/*
 * Respond to drag changes to the fillet manipulator
 */
export function filletManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    try
    {
        if (newManipulators["filletRadiusManipulator"] is map)
        {
            // convert given offset and edge topology into new radius
            var operativeEntity = findManipulationEntity(context, definition);
            var origin = evEdgeTangentLine(context, { "edge" : operativeEntity, "parameter" : 0.5 }).origin;
            var normals = findSurfaceNormalsAtEdge(context, operativeEntity, origin);
            var convexity = isEdgeConvex(context, operativeEntity) ? -1.0 : 1.0;

            definition.radius = convexity * newManipulators["filletRadiusManipulator"].offset / findRadiusToOffsetRatio(normals);
        }
    }

    return definition;
}

/*
 * Find the final element in the qlv.
 * If it is an edge, return it.
 * If it is a face, return one of its edges arbitrarily
 */
function findManipulationEntity(context is Context, definition is map) returns Query
{
    var resolvedEntities = evaluateQuery(context, definition.entities);
    if (@size(resolvedEntities) > 0)
    {
        var operativeEntity = resolvedEntities[@size(resolvedEntities) - 1];
        if (@size(evaluateQuery(context, qEntityFilter(operativeEntity, EntityType.FACE))) != 0)
        {
            operativeEntity = evaluateQuery(context, qEdgeAdjacent(operativeEntity, EntityType.EDGE))[0];
        }
        return operativeEntity;
    }
    throw {};
}

/*
 * Find surface normals at the point closest to edgePoint on the two faces attached to the given edge
 */
function findSurfaceNormalsAtEdge(context is Context, edge is Query, edgePoint is Vector) returns array
{
    var faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));

    var normals = makeArray(2);
    for (var i = 0; i < 2; i += 1)
    {
        var param = evProjectPointOnFace(context, { "face" : faces[i], "point" : edgePoint }) as Vector;
        var plane = evFaceTangentPlane(context, {
                    "face" : faces[i],
                    "parameter" : param
                });

        normals[i] = plane.normal;
    }
    return normals;
}

function isEdgeConvex(context is Context, edge is Query) returns boolean
{
    return evEdgeConvexity(context, { "edge" : edge }) == EdgeConvexityType.CONVEX;
}

/*
 * The distance from the center of a corner-inscribed circle to the corner itself is:
 * radius / cos(0.5 * angle between surface normals)
 * Therefore, the distance between the outer ege of the circle and the corner (the offset of the manipulator) is:
 * (radius / cos(0.5 * angle between surface normals)) - radius
 * So:
 * offset = radius * ((1.0 / cos(0.5 * angle between surface normals)) - 1.0)
 */
function findRadiusToOffsetRatio(normalArray is array) returns number
{
    return (1.0 / cos(0.5 * angleBetween(normalArray[0], normalArray[1]))) - 1.0;
}

