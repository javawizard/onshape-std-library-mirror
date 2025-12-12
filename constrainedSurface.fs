FeatureScript ✨; /* Automatically generated version */
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/approximationUtils.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/topologyUtils.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");

/**
 * Constrained surface input type
 */
export enum ConstrainedSurfaceType
{
    annotation { "Name" : "Points" }
    POINTS,
    annotation { "Name" : "Mesh" }
    MESH
}

/**
 * Constrained surface deviation display type
 * @internal
 */
export enum DeviationType
{
    annotation { "Name" : "Max deviation" }
    MAX,
    annotation { "Name" : "All deviations" }
    ALL
}

/**
 * Constrained surface optimization type
 * @value PERF : Faster performance that may produce lower-quality surfaces.
 * @value SMOOTH :  Typically produces higher-quality surfaces with lower curvatures, but with slower performance and more control points.
 */
export enum OptimizationMethod
{
    annotation { "Name" : "Performance" }
    PERF,
    annotation { "Name" : "Smoothness" }
    SMOOTH
}

/**
 * @internal
 * Predicate for deviation computation. Provides required inputs for evPointsDeviation.
 */
export predicate deviationParameters(definition is map)
{
    annotation { "Name" : "Deviation" }
    definition.computeDeviation is boolean;

    annotation { "Group Name" : "Deviation", "Collapsed By Default" : false, "Driving Parameter" : "computeDeviation" }
    {
        if (definition.computeDeviation)
        {
            annotation { "Name" : "Maximum deviation", "UIHint" : UIHint.READ_ONLY }
            isLength(definition.maxDeviation, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

            annotation { "Name" : "Show deviation" }
            definition.showDeviation is boolean;
            annotation { "Group Name" : "Show deviation", "Collapsed By Default" : false, "Driving Parameter" : "showDeviation" }
            {
                if (definition.showDeviation)
                {
                    annotation { "Name" : "Deviation type" }
                    definition.deviationType is DeviationType;

                    annotation { "Name" : "Limit" }
                    isLength(definition.limit, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
                }
            }
        }
    }
}

/**
 * Constrained surface feature. Takes an arbitrary number of vertices and optional normal directions or meshes and creates a surface passing through the vertices/mesh points within a provided tolerance.
 *  ```constrainedSurface(context, id + "FG2MuQGCNLssvyp_123", {
 *      "csType" : ConstrainedSurfaceType.MESH,
 *      "vertices" : [],
 *      "meshes" : qUnion([iLEjUaBPUqVOpC_query]),
 *      "tolerance" : { 'value' : try(1 * inch), 'expression' : "1 in" }.value,
 *      "optimize" : OptimizationMethod.PERF
 * });```
 *
 * @param id : @autocomplete `id + "constrainedSurface1"`
 * @param definition {{
 *      @field csType {ConstrainedSurfaceType}:
 *              Determines if the input is a mesh or a collection of points.
 *      @field vertices {array}: @requiredif{`csType` is `ConstrainedSurfaceType.POINTS`}
 *              An array of elements, each with a `vertex` on the surface to include. Optionally, specify a directional object to set the normal in the `normal` field, and set `flipNormal` to `true` to flip the normal.
 * @ex ```[
 *     {"vertex": qVertex1},
 *     {"vertex": qVertex2},
 *     {"vertex": qVertex3, "normal": qNormal1},
 *     {"vertex": qVertex4, "normal": qNormal2, "flipNormal": true}
 * ]```
 *      @field meshes {Query}: @requiredif{`csType` is `ConstrainedSurfaceType.MESH`}
 *              A query of mesh faces or bodies. Each mesh vertex of the query elements is on the constrained surface.
 *              @ex `"meshes" : qUnion([iLEjUaBPUqVOpC_query])`
 *      @field tolerance {ValueWithUnits}:
 *               Length to define how far from each vertex the constrained surface can be.
 *      @field optimize {OptimizationMethod}:
 *               Whether to optimize for performance (`OptimizationMethod.PERF`) or smoothness (`OptimizationMethod.SMOOTH`).
 * }}
 */
annotation { "Feature Type Name" : "Constrained surface", "Feature Type Description" : "" }
export const constrainedSurface = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Type", "UIHint" : UIHint.HORIZONTAL_ENUM }
        definition.csType is ConstrainedSurfaceType;

        if (definition.csType == ConstrainedSurfaceType.POINTS)
        {
            annotation { "Name" : "Vertices", "Item name" : "Vertex", "Item label template" : "#vertex", "Driven query" : "vertex", "UIHint" : [UIHint.COLLAPSE_ARRAY_ITEMS, UIHint.PREVENT_ARRAY_REORDER] }
            definition.vertices is array;
            for (var vertex in definition.vertices)
            {
                annotation { "Name" : "Vertex", "Filter" : EntityType.VERTEX, "MaxNumberOfPicks" : 1 }
                vertex.vertex is Query;
                annotation { "Name" : "Normal", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
                vertex.normal is Query;
                annotation { "Name" : "Flip normal", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                vertex.flipNormal is boolean;
            }
        }
        else
        {
            annotation { "Name" : "Mesh bodies or faces", "Filter" : (EntityType.FACE || EntityType.BODY) && !AllowMeshGeometry.NO }
            definition.meshes is Query;
        }

        annotation { "Name" : "Tolerance" }
        isLength(definition.tolerance, TOLERANCE_BOUND);

        annotation { "Name" : "Optimize", "UIHint" : UIHint.SHOW_LABEL }
        definition.optimize is OptimizationMethod;

        deviationParameters(definition);
    }
    {
        var constrainedSurfaceDefinition = { "points" : [] };

        if (definition.csType == ConstrainedSurfaceType.POINTS)
        {
            for (var i = 0; i < size(definition.vertices); i += 1)
            {
                if (isQueryEmpty(context, definition.vertices[i].vertex))
                {
                    continue;
                }
                var point = {};
                point.point = evVertexPoint(context, {
                            "vertex" : definition.vertices[i].vertex
                        });
                if (!isQueryEmpty(context, definition.vertices[i].normal))
                {
                    point.normal = (definition.vertices[i].flipNormal ? -1 : 1) * extractDirection(context, definition.vertices[i].normal);
                }
                constrainedSurfaceDefinition.points = append(constrainedSurfaceDefinition.points, point);
            }
            if (size(constrainedSurfaceDefinition.points) < 3)
            {
                throw regenError(ErrorStringEnum.CONSTRAINED_SURFACE_TOO_FEW_POINTS, ["vertices"]);
            }
        }
        else
        {
            if (isQueryEmpty(context, definition.meshes))
            {
                throw regenError(ErrorStringEnum.CONSTRAINED_SURFACE_SELECT_MESH, ["meshes"]);
            }
            constrainedSurfaceDefinition.references = definition.meshes;
            const points = evMeshPoints(context, { "meshes" : definition.meshes });
            constrainedSurfaceDefinition.points = mapArray(points, point => { "point" : point });
        }

        constrainedSurfaceDefinition.smooth = definition.optimize == OptimizationMethod.SMOOTH;
        constrainedSurfaceDefinition.tolerance = definition.tolerance;

        opConstrainedSurface(context, id, constrainedSurfaceDefinition);

        if (definition.computeDeviation)
        {
            processDeviation(context, id, definition, mapArray(constrainedSurfaceDefinition.points, point => point.point), qCreatedBy(id, EntityType.BODY));
        }

    });

function processDeviation(context is Context, id is Id, definition is map, points is array, surface is Query)
{
    const allDeviations = definition.showDeviation && definition.deviationType == DeviationType.ALL;
    const deviations = evPointsDeviation(context, {
                "points" : points,
                "topologies" : surface,
                "showDeviation" : definition.showDeviation,
                "allDeviations" : allDeviations,
                "limit" : definition.limit
            });

    var maxDeviation = 0 * meter;
    if (allDeviations)
    {
        maxDeviation = max(mapArray(deviations, function(deviation)
                {
                    return deviation.deviation;
                }));
    }
    else
    {
        maxDeviation = deviations[0].deviation;
    }

    setFeatureComputedParameter(context, id, { "name" : "maxDeviation", "value" : maxDeviation });
}

