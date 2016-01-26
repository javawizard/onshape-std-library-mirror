FeatureScript 293; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/coordSystem.fs", version : "");
import(path : "onshape/std/curveGeometry.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/mathUtils.fs", version : "");
import(path : "onshape/std/primitives.fs", version : "");
import(path : "onshape/std/sketch.fs", version : "");
import(path : "onshape/std/string.fs", version : "");
import(path : "onshape/std/surfaceGeometry.fs", version : "");
import(path : "onshape/std/units.fs", version : "");

const DEBUG_ID_STRING = "debug314159"; // Unlikely to clash
const ARROW_LENGTH = 0.05 * meter;
const ARROW_RADIUS = 0.05 * ARROW_LENGTH;

/**
 * Dump and, if applicable, display `value`, whatever it may be (query, geometry, etc.)
 * Current values that can be displayed are:
 *
 * `Query`,
 * 3D `Vector` (as a 3D point if entries are lengths, or as a direction from the origin if it is normalized),
 * `Line`,
 * `CoordSystem`,
 * `Plane`.
 */
export function debug(context is Context, value) // TODO: `Circle`, `Ellipse`, `Cylinder`, `Cone`, `Torus`, `Transform`, `Box`.
{
    print("debug: ");
    println("" ~ value);
}

export function debug(context is Context, value is ValueWithUnits)
{
    print("debug: ");
    println(value);
}

export function debug(context is Context, value is Vector)
{
    print("debug: ");
    if (is3dLengthVector(value))
    {
        print("Vector ");
        println(value);
        addDebugPoint(context, value);
    }
    else if (is3dDirection(value))
    {
        print("Direction ");
        println(value);
        addDebugArrow(context, vector(0, 0, 0) * meter, value * ARROW_LENGTH, ARROW_RADIUS);
    }
    else
    {
        println("Vector " ~ value);
    }
}

export function debug(context is Context, value is Query)
{
    print("debug: Query resolves to ");
    const entities = evaluateQuery(context, value);
    if (entities == [])
    {
        println("nothing");
        return;
    }
    var first is boolean = true;
    for (var entityType in EntityType)
    {
        const count = size(evaluateQuery(context, qEntityFilter(qUnion(entities), entityType.value)));
        if (count == 0)
            continue;
        const entityString = { EntityType.VERTEX : "vertices",
                    EntityType.EDGE : "edges",
                    EntityType.FACE : "faces",
                    EntityType.BODY : "bodies" }[entityType.value];
        print((first ? "" : ", ") ~ count ~ " " ~ entityString);
        first = false;
    }
    print("\n");
    addDebugEntities(context, value);
}

export function debug(context is Context, value is Line)
{
    print("debug: Line ");
    println(value);
    addDebugArrow(context, value.origin, value.origin + value.direction * ARROW_LENGTH, ARROW_RADIUS);
    addDebugArrow(context, value.origin + value.direction * ARROW_LENGTH, value.origin, ARROW_RADIUS * 0.5);
}

export function debug(context is Context, value is CoordSystem)
{
    print("debug: CoordSystem ");
    println(value);
    addDebugArrow(context, value.origin, value.origin + value.xAxis * ARROW_LENGTH, ARROW_RADIUS);
    addDebugArrow(context, value.origin, value.origin + yAxis(value) * ARROW_LENGTH, ARROW_RADIUS * (2 / 3));
    addDebugArrow(context, value.origin, value.origin + value.zAxis * ARROW_LENGTH, ARROW_RADIUS * 0.5);
}

export function debug(context is Context, value is Plane)
{
    print("debug: Plane ");
    println(value);
    addDebugArrow(context, value.origin, value.origin + value.x * ARROW_LENGTH, ARROW_RADIUS);
    addDebugArrow(context, value.origin, value.origin + yAxis(value) * ARROW_LENGTH, ARROW_RADIUS * (2 / 3));
    addDebugArrow(context, value.origin, value.origin + value.normal * ARROW_LENGTH * 0.5, ARROW_RADIUS * 0.5);

    const planeId = getCurrentSubfeatureId(context) + DEBUG_ID_STRING + "plane";
    startFeature(context, planeId, {});
    try
    {
        const sketch = newSketchOnPlane(context, planeId, { "sketchPlane" : value });
        skRectangle(sketch, "rectangle", {
                "firstCorner" : vector(0, 0) * meter,
                "secondCorner" : vector(ARROW_LENGTH, ARROW_LENGTH)
            });
        skSolve(sketch);
        addDebugEntities(context, qCreatedBy(planeId, EntityType.FACE));
    }
    abortFeature(context, planeId);
}

// Utility functions below

function addDebugPoint(context is Context, point is Vector)
{
    const pointId = getCurrentSubfeatureId(context) + DEBUG_ID_STRING + "point";
    startFeature(context, pointId, {});
    try
    {
        opPoint(context, pointId, { "point" : point });
        addDebugEntities(context, qCreatedBy(pointId));
    }
    abortFeature(context, pointId);
}

function addDebugArrow(context is Context, from is Vector, to is Vector, radius is ValueWithUnits)
{
    const arrowId = getCurrentSubfeatureId(context) + DEBUG_ID_STRING + "arrow";
    startFeature(context, arrowId, {});
    try
    {
        const length = norm(to - from);
        const orth = perpendicularVector(to - from);

        var lineDef = { "end" : vector(length, 0 * meter) };

        const sketch1 = newSketchOnPlane(context, arrowId + "sketch1", {
                    "sketchPlane" : plane(from, orth, to - from)
                });
        lineDef.start = vector(0, 0) * meter;
        skLineSegment(sketch1, "line1", lineDef);
        lineDef.start = vector(length - radius, radius);
        skLineSegment(sketch1, "line2", lineDef);
        lineDef.start = vector(length - radius, -radius);
        skLineSegment(sketch1, "line3", lineDef);
        skSolve(sketch1);

        const sketch2 = newSketchOnPlane(context, arrowId + "sketch2", {
                    "sketchPlane" : plane(from, cross(orth, to - from), to - from)
                });
        lineDef.start = vector(length - radius, radius);
        skLineSegment(sketch2, "line2", lineDef);
        lineDef.start = vector(length - radius, -radius);
        skLineSegment(sketch2, "line3", lineDef);
        skSolve(sketch2);

        addDebugEntities(context, qCreatedBy(arrowId, EntityType.EDGE));
    }
    abortFeature(context, arrowId);
}

function addDebugEntities(context is Context, entities is Query)
{
    @addDebugEntities(context, { "entities" : entities });
}

