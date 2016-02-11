FeatureScript 307; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

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
 * Create a cube of a specified size, with one corner on the origin.
 * @param definition {{
 *      @field sideLength {valueWithUnits}
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

// Defined in the old way to overload with the sphere functions in surfaceGeometry.
/**
 * Create a solid sphere. The feature version of fSphere.
 *
 * TODO: rename and merge this with fSphere.
 *
 * @param id : @autocomplete `id + "sphere1"`
 * @param definition {{
 *      @field center {Vector} : A 3D length vector in world space. @eg `vector(0, 0, 0) * inch`
 *      @field radius {ValueWithUnits} : @eg `1 * inch`
 * }}
 */
annotation { "Feature Type Name" : "Sphere" }
export function sphere(context is Context, id is Id, definition is map)
{
    fSphere(context, id, definition);
}

/**
 * Create a solid sphere.
 * @param id : @autocomplete `id + "sphere1"`
 * @param definition {{
 *      @field center {Query} : A single point marking the sphere's center.
 *      @field radius {ValueWithUnits}
 * }}
 */
export const fSphere = defineFeature(function(context is Context, id is Id, definition is map)
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
        var remainingTransform = undefined;
        if (definition.center is Query)
        {
            remainingTransform = getRemainderPatternTransform(context, {"references" : definition.center});
        }
        startFeature(context, id, definition);
        if (definition.center is Query)
            definition.center = try(evVertexPoint(context, { "vertex" : definition.center }));
        if (definition.center == undefined)
            definition.center = vector(0, 0, 0) * meter;

        definition.radius = vector(1, 1, 1) * definition.radius;

        fEllipsoid(context, id, definition);
        endFeature(context, id);

        if (remainingTransform != undefined)
        {
            transformResultIfNecessary(context, id, remainingTransform);
        }
    });

/**
 * Create a simple rectangular prism between two specified corners.
 * @param id : @autocomplete `id + "cuboid1"`
 * @param definition {{
 *      @field corner1 {Vector} : A 3D length vector in world space. @eg `vector(0, 0, 0) * inch`
 *      @field corner2 {Vector} : A 3D length vector in world space. @eg `vector(1, 1, 1) * inch`
 * }}
 */
export const fCuboid = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        is3dLengthVector(definition.corner1);
        is3dLengthVector(definition.corner2);
    }
    {
        var remainingTransform = getRemainderPatternTransform(context, {"references" : qNothing()});
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
        transformResultIfNecessary(context, id, remainingTransform);
    });

/**
 * Create a simple cylindrical solid between two points, with a specified radius.
 * @param id : @autocomplete `id + "cylinder1"`
 * @param definition {{
 *      @field topCenter {Vector} : A 3D length vector in world space. @eg `vector(0, 0, 0) * inch`
 *      @field bottomCenter {Vector} : A 3D length vector in world space. @eg `vector(1, 1, 1) * inch`
 *      @field radius {ValueWithUnits} : @eg `1 * inch`
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

/**
 * Create a solid cone, possibly truncated.
 * @param definition {{
 *      @field topCenter {Vector} : A 3D length vector in world space.
 *      @field bottomCenter {Vector} : A 3D length vector in world space.
 *      @field topRadius {ValueWithUnits} : The radius at the top center.
 *      @field bottomRadius {ValueWithUnits} : The radius at the bottom center.
 *          @eg `0` produces a standard, non-truncated cone.
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
 * Create an ellipsoid (that is, a sphere scaled independently along the three major axes).
 * @param definition {{
 *      @field center {Vector} : A 3D vector in world coordinates
 *      @field radius {Vector} : The three radii, as measured along the x, y, and z axes, given as
 *              a single 3D length vector.
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

            opTransform(context, id + "transformResult",
                        { "bodies" : query,
                          "transform" : transform
                        });
        }
    });

