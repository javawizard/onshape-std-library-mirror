FeatureScript 901; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/box.fs", version : "901.0");
import(path : "onshape/std/containers.fs", version : "901.0");
import(path : "onshape/std/coordSystem.fs", version : "901.0");
import(path : "onshape/std/curveGeometry.fs", version : "901.0");
import(path : "onshape/std/feature.fs", version : "901.0");
import(path : "onshape/std/mathUtils.fs", version : "901.0");
import(path : "onshape/std/primitives.fs", version : "901.0");
import(path : "onshape/std/sketch.fs", version : "901.0");
import(path : "onshape/std/string.fs", version : "901.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "901.0");
import(path : "onshape/std/units.fs", version : "901.0");

const DEBUG_ID_STRING = "debug314159"; // Unlikely to clash
const ARROW_LENGTH = 0.05 * meter;
const ARROW_RADIUS = 0.05 * ARROW_LENGTH;

/**
 * Print and, if applicable, display `value` in a Part Studio, highlighting or
 * creating entities in red.
 *
 * The displayed data will ONLY be visible when the feature calling the
 * `debug` function is being edited. Entities displayed during debug are for
 * display only, and will not appear in any queries.
 *
 * Values which can be debugged are:
 *
 * `Query`: Highlights entities matching the `Query` (bodies, faces, edges,
 * and vertices) in red.
 *
 * 3D length `Vector`: Displays a single point in world space.
 *
 * Unitless, normalized 3D `Vector`: Displays an arrow starting at the world
 * origin, pointing in the given direction.
 *
 * [Line]: Displays an arrow starting at the line's origin, pointing in the
 * line's direction.
 *
 * [CoordSystem]: Displays three perpendicular arrows from the coordinate
 * system's origin, along its three axes. The arrowhead for the x-axis is
 * largest, and the z-axis is smallest.
 *
 * [Plane]: Displays a large square in the positive quadrant of the plane,
 * along with three arrows along the plane's x-axis, y-axis, and normal.
 *
 * [Box3d]: Displays the edges of the bounding box (in the given coordinate
 * system, if provided)
 *
 * The overloads in this module define these behaviors.
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

/**
 * Draws a line between `point1` and `point2` and prints the points with the distance between them.
 */
export function debug(context is Context, point1 is Vector, point2 is Vector)
{
    print("debug: Two vectors: ");
    print(point1);
    print(" and ");
    print(point2);
    if (is3dLengthVector(point1) && is3dLengthVector(point2))
    {
        println(", distance = " ~ toString(norm(point2 - point1)));
        const lineId = getCurrentSubfeatureId(context) + DEBUG_ID_STRING + "line";
        startFeature(context, lineId, {});
        try
        {
            const length = norm(point2 - point1);
            const orth = perpendicularVector(point2 - point1);

            var lineDef = { "end" : vector(length, 0 * meter) };

            const sketch1 = newSketchOnPlane(context, lineId + "sketch1", {
                        "sketchPlane" : plane(point1, orth, point2 - point1)
                    });
            lineDef.start = vector(0, 0) * meter;
            skLineSegment(sketch1, "line1", lineDef);
            skSolve(sketch1);

            addDebugEntities(context, qCreatedBy(lineId, EntityType.EDGE));
        }
        abortFeature(context, lineId);
    }
    else
    {
        print('\n');
    }
}

/**
 * Displays the edges of a bounding box in the world coordinate system.
 */
export function debug(context is Context, boundingBox is Box3d)
{
    debug(context, boundingBox, undefined);
}

/**
 * Displays the edges of a bounding box in the given coordinate system.
 *
 * @example ```
 * const myBox = evBox3d(context, { "topology" : entities, "cSys" : myCSys });
 * debug(context, myBox, myCSys);
 * ```
 */
export function debug(context is Context, boundingBox is Box3d, cSys)
{
    print("debug: Bounding box with corners: " ~ toString(boundingBox.minCorner) ~ " and " ~ toString(boundingBox.minCorner));
    if (cSys == undefined)
    {
        cSys = WORLD_COORD_SYSTEM;
        print("\n");
    }
    else
    {
        println(" in cSys: " ~ toString(cSys));
    }

    const transform = toWorld(cSys);
    const boxId = getCurrentSubfeatureId(context) + DEBUG_ID_STRING + "box";

    startFeature(context, boxId, {});
    try
    {
        fCuboid(context, boxId + "cube", {
            "corner1" : boundingBox.minCorner,
            "corner2" : boundingBox.maxCorner
        });
        opTransform(context, boxId + "transform", {
                "bodies" : qCreatedBy(boxId + "cube", EntityType.BODY),
                "transform" : transform
        });
        addDebugEntities(context, qCreatedBy(boxId, EntityType.EDGE));
    }
    abortFeature(context, boxId);
}

/**
 * Highlights `entities` in red, without printing anything.
 *
 * As with [debug], highlighted entities are only visible while the debugged feature's edit dialog is open.
 */
export function addDebugEntities(context is Context, entities is Query)
{
    @addDebugEntities(context, { "entities" : entities });
}

// Timers for very basic profiling

/** Starts the timer associated with the string `timer` or resets it.  Use with [printTimer(string)]. */
export function startTimer(timer is string)
{
    @startTimer(timer);
}

/** Starts the global timer associated with the empty string or resets it.  Use with [printTimer()]. */
export function startTimer()
{
    startTimer("");
}

/**
 * Prints the elapsed milliseconds for the timer associated with the string `timer`.  Use with [startTimer(string)].
 *
 * Note that if the timer was set in a prior feature, the elapsed time may be very large because features can
 * be regenerated at different times.
 *
 * Throws an error if no such timer has been started.
 */
export function printTimer(timer is string)
{
    @printTimer(timer);
}

/**
 * Prints the elapsed milliseconds for the global timer associated with the empty string.  Use with [startTimer()].
 *
 * Note that if the timer was set in a prior feature, the elapsed time may be very large because features can
 * be regenerated at different times.
 *
 * Throws an error if no such timer has been started.
 */
export function printTimer()
{
    printTimer("");
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

