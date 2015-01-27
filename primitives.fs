export import(path : "onshape/std/extrude.fs", version : "");
export import(path : "onshape/std/revolve.fs", version : "");
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/sketch.fs", version : "");
export import(path : "onshape/std/feature.fs", version : "");

annotation {"Feature Type Name" : "Cube"}
export function cube(context is Context, id is Id, definition is map)
precondition
{
    annotation {"Name" : "Side length"}
    isLength(definition.sideLength, NONNEGATIVE_LENGTH_BOUNDS);
}
{
    definition.corner1 = vector(0, 0, 0) * meter;
    definition.corner2 = vector(1, 1, 1) * definition.sideLength;
    cuboid(context, id, definition);
}

annotation {"Feature Type Name" : "Sphere"}
export function sphere(context is Context, id is Id, definition is map)
precondition
{
    if(definition.center != undefined)
    {
        annotation {"Name" : "Center", "Filter" : EntityType.VERTEX, "MaxNumberOfPicks" : 1}
        definition.center is Query;
    }
    annotation {"Name" : "Radius"}
    isLength(definition.radius, NONNEGATIVE_LENGTH_BOUNDS);
}
{
    var center;
    if(definition.center != undefined)
    {
        var pointResult = evVertexPoint(context, { "vertex" : definition.center });
        center = pointResult.result;
    }
    if(center == undefined)
        center = vector(0, 0, 0) * meter;

    definition.center = center;
    definition.radius = vector(1, 1, 1) * definition.radius;

    ellipsoid(context, id, definition);
}

export function cuboid(context is Context, id is Id, definition is map)
precondition
{
    is3dLengthVector(definition.corner1);
    is3dLengthVector(definition.corner2);
}
{
    startFeature(context, id, definition);
    var sketchId = id + "sketch";
    {
        var plane = XY_PLANE;
        plane.origin[2] = min(definition.corner1[2], definition.corner2[2]);
        var sketch = newSketchOnPlane(context, sketchId, {sketchPlane : plane});
        skRectangle(sketch, "rectangle",
                    { "firstCorner"  : vector(resize(definition.corner1, 2)),
                      "secondCorner" : vector(resize(definition.corner2, 2))
                    });

        skSolve(sketch);
    }
    {
        var query = makeQuery(sketchId + "imprint", "IMPRINT", EntityType.FACE, {});
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
        var query = qCreatedBy(sketchId, EntityType.BODY);
        deleteBodies(context, id + "deleteSketch", { "entities" : query });
    }
    endFeature(context, id);
}

export function cylinder(context is Context, id is Id, definition is map)
precondition
{
    is3dLengthVector(definition.bottomCenter);
    is3dLengthVector(definition.topCenter);
    isLength(definition.radius, NONNEGATIVE_LENGTH_BOUNDS);
}
{
    startFeature(context, id, definition);
    var sketchId = id + "sketch";
    {
        var plane = plane(definition.bottomCenter, normalize(definition.topCenter - definition.bottomCenter));
        var sketch = newSketchOnPlane(context, sketchId, {sketchPlane : plane});

        skCircle(sketch, "circle1",
                 { "center" : vector(0, 0) * meter,
                   "radius" : definition.radius
                 });

        skSolve(sketch);
    }
    {
        var query = makeQuery(sketchId + "imprint", "IMPRINT", EntityType.FACE, {});
        extrude(context, id + "extrude",
                { "entities" : query,
                  "endBound" : BoundingType.BLIND,
                  "depth"    : norm(definition.topCenter - definition.bottomCenter)
                });
    }
    {
      var query = qCreatedBy(sketchId, EntityType.BODY);
      deleteBodies(context, id + "deleteSketch", { "entities" : query });
    }
    endFeature(context, id);
}

//Truncated cone, actually
export function cone(context is Context, id is Id, definition is map)
precondition
{
    is3dLengthVector(definition.bottomCenter);
    is3dLengthVector(definition.topCenter);
    isLength(definition.bottomRadius, NONNEGATIVE_LENGTH_BOUNDS);
    isLength(definition.topRadius, NONNEGATIVE_LENGTH_BOUNDS);
}
{
    startFeature(context, id, definition);
    var sketchId = id + "sketch";
    {
        var dir = normalize(definition.topCenter - definition.bottomCenter);
        var normal = perpendicularVector(dir);
        var plane = plane(definition.bottomCenter, normal, crossProduct(dir, normal));
        var sketch = newSketchOnPlane(context, sketchId, {sketchPlane : plane});
        var height = norm(definition.topCenter - definition.bottomCenter);
        {
            var points = [vector(0, 0) * meter,
                          vector(0 * meter, height),
                          vector(definition.topRadius, height),
                          vector(definition.bottomRadius, 0 * meter)];
            for(var i = 0; i < size(points); i += 1)
            {
                skLineSegment(sketch, "line." ~ i,
                              { "start"  : points[i],
                                "end"    : points[(i + 1) % size(points)]
                              });
            }
        }


        skSolve(sketch);
    }
    {
        var query = makeQuery(sketchId + "imprint", "IMPRINT", EntityType.FACE, {});
        var axis = sketchEntityQuery(sketchId + "wireOp", EntityType.EDGE, "line.0");
        revolve(context, id + "revolve",
                {  "entities"    : query,
                  "axis"        : axis,
                  "revolveType" : RevolveType.FULL
                });
    }
    {
        var query = qCreatedBy(sketchId, EntityType.BODY);
        deleteBodies(context, id + "deleteSketch", { "entities" : query });
    }
    endFeature(context, id);
}

export function ellipsoid(context is Context, id is Id, definition is map)
precondition
{
    is3dLengthVector(definition.center);
    is3dLengthVector(definition.radius);
}
{
    startFeature(context, id, definition);
    var sketchId = id + "sketch";
    {
        var sketch = newSketchOnPlane(context, sketchId, {sketchPlane : XY_PLANE} );
        skLineSegment(sketch, "line1",
                      { "start"  : vector(0, 1) * meter,
                        "end"    : vector(0, -1) * meter
                      });
        skArc(sketch, "arc1",
              { "start"  : vector(0, 1) * meter,
                "mid"    : vector(1, 0) * meter,
                "end"    : vector(0, -1) * meter
              });

        skSolve(sketch);
    }
    {
        var query = qCreatedBy(sketchId, EntityType.FACE);
        var axis = sketchEntityQuery(sketchId + "wireOp", EntityType.EDGE, "line1");
        revolve(context, id + "revolve",
                { "entities"    : query,
                  "axis"        : axis,
                  "revolveType" : RevolveType.FULL
                });
    }
    {
      var query = qCreatedBy(sketchId, EntityType.BODY);
      deleteBodies(context, id + "deleteSketch", { "entities" : query });
    }
    {
        var query = qCreatedBy(id, EntityType.BODY);
        var matrix = identityMatrix(3);
        matrix[0][0] = definition.radius[0].value;
        matrix[1][1] = definition.radius[1].value;
        matrix[2][2] = definition.radius[2].value;
        var translation = definition.center;
        var transform = transform(matrix, translation);

        transformBodies(context, id + "transform" ,
                        { "bodies"     : query,
                          "transform" : transform
                        });
    }
    endFeature(context, id);
}

