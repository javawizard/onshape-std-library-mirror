FeatureScript ✨; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");

// Imports used internally
import(path : "onshape/std/boundingtype.gen.fs", version : "");
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/curveGeometry.fs", version : "");
import(path : "onshape/std/evaluate.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/mathUtils.fs", version : "");
import(path : "onshape/std/sketch.fs", version : "");
import(path : "onshape/std/surfaceGeometry.fs", version : "");
import(path : "onshape/std/tool.fs", version : "");
import(path : "onshape/std/valueBounds.fs", version : "");

/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
annotation { "Feature Type Name" : "Cube" }
export const cube = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Side length" }
        isLength(definition.sideLength, NONNEGATIVE_LENGTH_BOUNDS);
    }
    {
        definition.corner1 = vector(0, 0, 0) * meter;
        definition.corner2 = vector(1, 1, 1) * definition.sideLength;
        fCuboid(context, id, definition);
    });

// Defined in the old way to overload with the sphere functions in surfaceGeometry.  TODO: rename features to start with f.
/**
 * TODO: description
 * @param context
 * @param id
 * @param definition {{
 *      @field TODO
 * }}
 */
annotation { "Feature Type Name" : "Sphere" }
export function sphere(context is Context, id is Id, definition is map)
precondition
{
    if (definition.center != undefined)
    {
        annotation { "Name" : "Center", "Filter" : EntityType.VERTEX, "MaxNumberOfPicks" : 1 }
        definition.center is Query;
    }

    annotation { "Name" : "Radius" }
    isLength(definition.radius, NONNEGATIVE_LENGTH_BOUNDS);
}
{
    startFeature(context, id, definition);
    if (definition.center is Query)
        definition.center = try(evVertexPoint(context, { "vertex" : definition.center }));
    if (definition.center == undefined)
        definition.center = vector(0, 0, 0) * meter;

    definition.radius = vector(1, 1, 1) * definition.radius;

    fEllipsoid(context, id, definition);
    endFeature(context, id);
}

/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
export const fCuboid = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        is3dLengthVector(definition.corner1);
        is3dLengthVector(definition.corner2);
    }
    {
        const sketchId = id + "sketch";
        {
            var plane = XY_PLANE;
            plane.origin[2] = min(definition.corner1[2], definition.corner2[2]);
            const sketch = newSketchOnPlane(context, sketchId, { sketchPlane : plane });
            skRectangle(sketch, "rectangle",
                    { "firstCorner"  : vector(resize(definition.corner1, 2)),
                      "secondCorner" : vector(resize(definition.corner2, 2))
                    });

            skSolve(sketch);
        }
        {
            const query = makeQuery(sketchId + "imprint", "IMPRINT", EntityType.FACE, {});
            opExtrude(context, id + "extrude",
                      { "entities"   : query,
                        "startBound" : BoundingType.BLIND,
                        "endBound"   : BoundingType.BLIND,
                        "startDepth" : 0 * meter,
                        "direction"  : [0, 0, 1],
                        "endDepth"   : abs(definition.corner2[2] - definition.corner1[2])
                    });
        }
        {
            const query = qCreatedBy(sketchId, EntityType.BODY);
            opDeleteBodies(context, id + "deleteSketch", { "entities" : query });
        }
    });

/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
export const fCylinder = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        is3dLengthVector(definition.bottomCenter);
        is3dLengthVector(definition.topCenter);
        isLength(definition.radius, NONNEGATIVE_LENGTH_BOUNDS);
    }
    {
        const sketchId = id + "sketch";
        const direction = normalize(definition.topCenter - definition.bottomCenter);
        {
            const plane = plane(definition.bottomCenter, direction);
            const sketch = newSketchOnPlane(context, sketchId, { sketchPlane : plane });

            skCircle(sketch, "circle1",
                     { "center" : vector(0, 0) * meter,
                       "radius" : definition.radius
                     });

            skSolve(sketch);
        }
        {
            const query = makeQuery(sketchId + "imprint", "IMPRINT", EntityType.FACE, {});
            opExtrude(context, id + "extrude",
                    { "entities" : query,
                      "direction" : direction,
                      "endBound" : BoundingType.BLIND,
                      "endDepth"    : norm(definition.topCenter - definition.bottomCenter)
                    });
        }
        {
            const query = qCreatedBy(sketchId, EntityType.BODY);
            opDeleteBodies(context, id + "deleteSketch", { "entities" : query });
        }
    });

//Truncated cone, actually
/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
export const fCone = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        is3dLengthVector(definition.bottomCenter);
        is3dLengthVector(definition.topCenter);
        isLength(definition.bottomRadius, NONNEGATIVE_LENGTH_BOUNDS);
        isLength(definition.topRadius, NONNEGATIVE_LENGTH_BOUNDS);
    }
    {
        const sketchId = id + "sketch";
        const dir = normalize(definition.topCenter - definition.bottomCenter);
        {
            const normal = perpendicularVector(dir);
            const plane = plane(definition.bottomCenter, normal, cross(dir, normal));
            const sketch = newSketchOnPlane(context, sketchId, { sketchPlane : plane });
            const height = norm(definition.topCenter - definition.bottomCenter);
            const base = vector(0, 0) * meter;
            {
                const points = [base,
                    base + vector(0 * meter, height),
                    base + vector(definition.topRadius, height),
                    base + vector(definition.bottomRadius, 0 * meter)];
                for (var i = 0; i < size(points); i += 1)
                {
                    skLineSegment(sketch, "line." ~ i,
                                  { "start" : points[i],
                                    "end"   : points[(i + 1) % size(points)]
                                  });
                }
            }

            skSolve(sketch);
        }
        {
            const query = makeQuery(sketchId + "imprint", "IMPRINT", EntityType.FACE, {});
            const axisResult = evLine(context, {"edge" : sketchEntityQuery(sketchId + "wireOp", EntityType.EDGE, "line.0")});
            opRevolve(context, id + "revolve",
                    { "entities"    : query,
                      "axis"        : axisResult,
                      "angleForward" : 2 * PI
                    });
        }
        {
            const query = qCreatedBy(sketchId, EntityType.BODY);
            opDeleteBodies(context, id + "deleteSketch", { "entities" : query });
        }
    });

/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
export const fEllipsoid = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        is3dLengthVector(definition.center);
        is3dLengthVector(definition.radius);
    }
    {
        const sketchId = id + "sketch";
        {
            const sketch = newSketchOnPlane(context, sketchId, { sketchPlane : XY_PLANE });
            skLineSegment(sketch, "line1",
                          { "start" : vector(0, 1) * meter,
                             "end"  : vector(0, -1) * meter
                          });
            skArc(sketch, "arc1",
                  { "start" : vector(0, 1) * meter,
                    "mid"   : vector(1, 0) * meter,
                    "end"   : vector(0, -1) * meter
                  });

            skSolve(sketch);
        }
        {
            const query = qCreatedBy(sketchId, EntityType.FACE);
            const axis = line(vector(0, 1, 0) * meter, vector(0, -1, 0));
            opRevolve(context, id + "revolve",
                    { "entities" : query,
                       "axis" : axis,
                       "angleForward" : 2 * PI
                    });
        }
        {
            const query = qCreatedBy(sketchId, EntityType.BODY);
            opDeleteBodies(context, id + "deleteSketch", { "entities" : query });
        }
        {
            const query = qCreatedBy(id, EntityType.BODY);
            var matrix = identityMatrix(3);
            matrix[0][0] = definition.radius[0].value;
            matrix[1][1] = definition.radius[1].value;
            matrix[2][2] = definition.radius[2].value;
            const translation = definition.center;
            const transform = transform(matrix, translation);

            opTransform(context, id + "transform",
                        { "bodies" : query,
                          "transform" : transform
                        });
        }
    });

