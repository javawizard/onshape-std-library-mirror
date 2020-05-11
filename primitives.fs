FeatureScript 1287; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "1287.0");

// Imports used internally
import(path : "onshape/std/boundingtype.gen.fs", version : "1287.0");
import(path : "onshape/std/containers.fs", version : "1287.0");
import(path : "onshape/std/curveGeometry.fs", version : "1287.0");
import(path : "onshape/std/evaluate.fs", version : "1287.0");
import(path : "onshape/std/feature.fs", version : "1287.0");
import(path : "onshape/std/mathUtils.fs", version : "1287.0");
import(path : "onshape/std/sketch.fs", version : "1287.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1287.0");
import(path : "onshape/std/tool.fs", version : "1287.0");
import(path : "onshape/std/valueBounds.fs", version : "1287.0");

/**
 * Create a cube of a specified size, with one corner on the origin.
 * @param definition {{
 *      @field sideLength {ValueWithUnits} :
 *              @eg `1 * inch`
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
        var remainingTransform = getRemainderPatternTransform(context, {"references" : qNothing()});
        definition.corner1 = vector(0, 0, 0) * meter;
        definition.corner2 = vector(1, 1, 1) * definition.sideLength;
        fCuboid(context, id, definition);
        transformResultIfNecessary(context, id, remainingTransform);
    });

// Defined in the old way to overload with the sphere functions in surfaceGeometry.
// TODO: rename and merge this with fSphere.
/**
 * Feature creating a sphere. Internally, calls [opSphere].
 *
 * @param id : @autocomplete `id + "sphere1"`
 * @param definition {{
 *      @field center {Query} : A vertex query marking the sphere's center.
 *      @field radius {ValueWithUnits} :
 *              @eg `1 * inch`
 * }}
 */
annotation { "Feature Type Name" : "Sphere" }
export function sphere(context is Context, id is Id, definition is map)
{
    fSphere(context, id, definition);
}

/** @internal */
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
        if (definition.center is Query)
            definition.center = try silent(evVertexPoint(context, { "vertex" : definition.center }));
        if (definition.center == undefined)
            definition.center = vector(0, 0, 0) * meter;

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V406_SPHERE_PRIMITIVE))
        {
            opSphere(context, id, definition);
        }
        else
        {
            definition.radius = vector(1, 1, 1) * definition.radius;
            fEllipsoid(context, id, definition);
        }

        if (remainingTransform != undefined)
        {
            transformResultIfNecessary(context, id, remainingTransform);
        }
    });

/**
 * Create a simple rectangular prism between two specified corners.
 * @param id : @autocomplete `id + "cuboid1"`
 * @param definition {{
 *      @field corner1 {Vector} :
 *              @eg `vector(0, 0, 0) * inch`
 *      @field corner2 {Vector} :
 *              @eg `vector(1, 1, 1) * inch`
 * }}
 */
export const fCuboid = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        is3dLengthVector(definition.corner1);
        is3dLengthVector(definition.corner2);
        for (var dim in [0, 1, 2])
            !tolerantEquals(definition.corner1[dim], definition.corner2[dim]);
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
 * Create a simple cylindrical solid between two points, with a specified radius.
 * @param id : @autocomplete `id + "cylinder1"`
 * @param definition {{
 *      @field topCenter {Vector} : A 3D length vector in world space.
 *              @eg `vector(0, 0, 0) * inch`
 *      @field bottomCenter {Vector} : A 3D length vector in world space.
 *              @eg `vector(1, 1, 1) * inch`
 *      @field radius {ValueWithUnits} :
 *              @eg `1 * inch`
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
 *      @field topCenter {Vector} :
 *              @eg `vector(1, 1, 1) * inch`
 *      @field bottomCenter {Vector} :
 *              @eg `vector(0, 0, 0) * inch`
 *      @field topRadius {ValueWithUnits} : The radius at the top center.
 *              @eg `1 * inch`
 *      @field bottomRadius {ValueWithUnits} : The radius at the bottom center.
 *              @eg `0 * inch` produces a standard, non-truncated cone.
 * }}
 */
export const fCone = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        is3dLengthVector(definition.bottomCenter);
        is3dLengthVector(definition.topCenter);
        isLength(definition.bottomRadius, NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS);
        isLength(definition.topRadius, NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS);
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
 *      @field center {Vector} :
 *              @eg `vector(0, 0, 0) * inch`
 *      @field radius {Vector} : The three radii, as measured along the x, y, and z axes.
 *              @eg `vector(0.5 * inch, 1 * inch, 2 * inch)`
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

