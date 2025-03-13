FeatureScript 2615; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

export import(path : "onshape/std/debugcolor.gen.fs", version : "2615.0");
import(path : "onshape/std/box.fs", version : "2615.0");
import(path : "onshape/std/containers.fs", version : "2615.0");
import(path : "onshape/std/coordSystem.fs", version : "2615.0");
import(path : "onshape/std/curveGeometry.fs", version : "2615.0");
import(path : "onshape/std/feature.fs", version : "2615.0");
import(path : "onshape/std/mathUtils.fs", version : "2615.0");
import(path : "onshape/std/primitives.fs", version : "2615.0");
import(path : "onshape/std/sketch.fs", version : "2615.0");
import(path : "onshape/std/string.fs", version : "2615.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2615.0");
import(path : "onshape/std/units.fs", version : "2615.0");

const DEBUG_ID_STRING = "debug314159"; // Unlikely to clash
const ARROW_LENGTH = 0.05 * meter;
const ARROW_RADIUS = 0.05 * ARROW_LENGTH;

/**
 * Print and, if applicable, display `value` in a Part Studio, highlighting
 * or creating entities in a chosen color, red by default.
 *
 * The displayed data will ONLY be visible when the feature calling the
 * `debug` function is being edited. Entities displayed during debug are for
 * display only, and will not appear in any queries.
 *
 * Values which can be debugged are:
 *
 * [Query]: Highlights entities matching the `Query` (bodies, faces, edges,
 * and vertices) in red.
 *
 * 3D length [Vector]: Displays a single point in world space.
 *
 * Unitless, normalized 3D [Vector]: Displays an arrow starting at the world
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
 * @param color : @optional The color of the debug highlight
 */
export function debug(context is Context, value, color is DebugColor) // TODO: `Circle`, `Ellipse`, `Cylinder`, `Cone`, `Torus`, `Transform`, `Box`.
{
    print("debug: ");
    println("" ~ value);
}

// This functions handles dispatching to all the specific implementations of debug() when a color parameter is not supplied
export function debug(context is Context, value)
{
    debug(context, value, DebugColor.RED);
}

export function debug(context is Context, value is ValueWithUnits, color is DebugColor)
{
    print("debug: ");
    println(value);
}

export function debug(context is Context, value is Vector, color is DebugColor)
{
    print("debug: ");
    if (is3dLengthVector(value))
    {
        print("Vector ");
        println(value);
        addDebugPoint(context, value, color);
    }
    else if (is3dDirection(value))
    {
        print("Direction ");
        println(value);
        addDebugArrow(context, vector(0, 0, 0) * meter, value * ARROW_LENGTH, ARROW_RADIUS, color);
    }
    else
    {
        println("Vector " ~ value);
    }
}

export function debug(context is Context, value is Query, color is DebugColor)
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

        print(first ? "" : ", ");
        first = false;

        var entityString;
        if (count == 1)
        {
            entityString = {
                        EntityType.VERTEX : "vertex",
                        EntityType.EDGE : "edge",
                        EntityType.FACE : "face",
                        EntityType.BODY : "body" }[entityType.value];
        }
        else
        {
            entityString = {
                        EntityType.VERTEX : "vertices",
                        EntityType.EDGE : "edges",
                        EntityType.FACE : "faces",
                        EntityType.BODY : "bodies" }[entityType.value];
        }
        print(count ~ " " ~ entityString);

        if (entityType.value == EntityType.BODY)
        {
            print(" (");
            var firstBodyType = true;
            for (var bodyType in BodyType)
            {
                const bodyCount = size(evaluateQuery(context, qBodyType(qEntityFilter(qUnion(entities), EntityType.BODY), bodyType.value)));
                if (bodyCount == 0)
                    continue;

                print(firstBodyType ? "" : ", ");
                firstBodyType = false;

                var bodyString = {
                        BodyType.SOLID : "solid",
                        BodyType.SHEET : "sheet",
                        BodyType.WIRE : "wire",
                        BodyType.POINT : "point",
                        BodyType.MATE_CONNECTOR : "mate connector",
                        BodyType.COMPOSITE : "composite" }[bodyType.value];
                print(bodyCount ~ " " ~ bodyString);
            }
            print(")");
        }
        else if (entityType.value == EntityType.VERTEX)
        {
            const mcVertexCount = size(evaluateQuery(context, qBodyType(qEntityFilter(qUnion(entities), EntityType.VERTEX), BodyType.MATE_CONNECTOR)));
            if (mcVertexCount > 0)
            {
                print(" (" ~ mcVertexCount ~ " ");
                print(mcVertexCount == 1 ? "mate connector" : "mate connectors");
                print(")");
            }
        }
    }
    print("\n");

    addDebugEntities(context, qUnion([value, qContainedInCompositeParts(value)]), color);
}

export function debug(context is Context, value is Line, color is DebugColor)
{
    print("debug: Line ");
    println(value);
    addDebugArrow(context, value.origin, value.origin + value.direction * ARROW_LENGTH, ARROW_RADIUS, color);
    addDebugArrow(context, value.origin + value.direction * ARROW_LENGTH, value.origin, ARROW_RADIUS * 0.5, color);
}

export function debug(context is Context, value is CoordSystem)
{
    debug(context, value, DebugColor.RED, DebugColor.GREEN, DebugColor.BLUE);
}

export function debug(context is Context, value is CoordSystem, color is DebugColor)
{
    debug(context, value, color, color, color);
}

export function debug(context is Context, value is CoordSystem, xColor is DebugColor, yColor is DebugColor, zColor is DebugColor)
{
    print("debug: CoordSystem ");
    println(value);
    addDebugArrow(context, value.origin, value.origin + value.xAxis * ARROW_LENGTH, ARROW_RADIUS, xColor);
    addDebugArrow(context, value.origin, value.origin + yAxis(value) * ARROW_LENGTH, ARROW_RADIUS * (2 / 3), yColor);
    addDebugArrow(context, value.origin, value.origin + value.zAxis * ARROW_LENGTH, ARROW_RADIUS * 0.5, zColor);
}

export function debug(context is Context, value is Plane, color is DebugColor)
{
    print("debug: Plane ");
    println(value);
    addDebugArrow(context, value.origin, value.origin + value.x * ARROW_LENGTH, ARROW_RADIUS, color);
    addDebugArrow(context, value.origin, value.origin + yAxis(value) * ARROW_LENGTH, ARROW_RADIUS * (2 / 3), color);
    addDebugArrow(context, value.origin, value.origin + value.normal * ARROW_LENGTH * 0.5, ARROW_RADIUS * 0.5, color);

    const planeId = getLastActiveId(context) + DEBUG_ID_STRING + "plane";
    startFeature(context, planeId, {});
    try
    {
        const sketch = newSketchOnPlane(context, planeId, { "sketchPlane" : value });
        skRectangle(sketch, "rectangle", {
                    "firstCorner" : vector(0, 0) * meter,
                    "secondCorner" : vector(ARROW_LENGTH, ARROW_LENGTH)
                });
        skSolve(sketch);
        addDebugEntities(context, qCreatedBy(planeId, EntityType.FACE), color);
    }
    abortFeature(context, planeId);
}

/**
 * Draws a line between `point1` and `point2` and prints the points with the distance between them.
 */
export function debug(context is Context, point1 is Vector, point2 is Vector, color is DebugColor)
{
    print("debug: Two vectors: ");
    print(point1);
    print(" and ");
    print(point2);
    if (is3dLengthVector(point1) && is3dLengthVector(point2))
    {
        println(", distance = " ~ toString(norm(point2 - point1)));
        addDebugLine(context, point1, point2, color);
    }
    else
    {
        print('\n');
    }
}

export function debug(context is Context, point1 is Vector, point2 is Vector)
{
    debug(context, point1, point2, DebugColor.RED);
}

/**
 * Displays the edges of a [Box3d] in the world coordinate system with a chosen [DebugColor].
 */
export function debug(context is Context, boundingBox is Box3d, color is DebugColor)
{
    debug(context, boundingBox, undefined, color);
}

/**
 * Displays the edges of a [Box3d] in the given coordinate system with a chosen [DebugColor].
 *
 * @example ```
 * const myBox = evBox3d(context, { "topology" : entities, "cSys" : myCSys });
 * debug(context, myBox, myCSys, DebugColor.RED);
 * ```
 */
export function debug(context is Context, boundingBox is Box3d, cSys, color is DebugColor)
{
    print("debug: Bounding box with corners: " ~ toString(boundingBox.minCorner) ~ " and " ~ toString(boundingBox.maxCorner));
    if (cSys == undefined)
    {
        print("\n");
    }
    else
    {
        println(" in cSys: " ~ toString(cSys));
    }

    const diagonal = boundingBox.maxCorner - boundingBox.minCorner;

    const boxId = getLastActiveId(context) + DEBUG_ID_STRING + "box";

    startFeature(context, boxId, {});
    try
    {
        for (var i in [0, 1, 2])
        {
            if (diagonal[i].value <= TOLERANCE.zeroLength)
                continue;

            var i1 = (i + 1) % 3;
            var i2 = (i + 2) % 3;

            var firstPoint = vector(0, 0, 0);
            firstPoint[i] = boundingBox.minCorner[i];

            for (var min1 in [false, true]) for (var min2 in [false, true])
            {
                // Skip redundant lines
                if (min1 && diagonal[i1].value <= TOLERANCE.zeroLength)
                    continue;
                if (min2 && diagonal[i2].value <= TOLERANCE.zeroLength)
                    continue;

                // Draw a line parallel to axis i
                firstPoint[i1] = min1 ? boundingBox.minCorner[i1] : boundingBox.maxCorner[i1];
                firstPoint[i2] = min2 ? boundingBox.minCorner[i2] : boundingBox.maxCorner[i2];

                var secondPoint = firstPoint;
                secondPoint[i] = boundingBox.maxCorner[i];

                opFitSpline(context, boxId + i + (min1 ~ min2), { "points" : [firstPoint, secondPoint] });
            }
        }
        if (cSys != undefined)
        {
            opTransform(context, boxId + "transform", {
                        "bodies" : qCreatedBy(boxId, EntityType.BODY),
                        "transform" : toWorld(cSys)
                    });
        }
        addDebugEntities(context, qCreatedBy(boxId, EntityType.EDGE), color);
    }
    abortFeature(context, boxId);
}

export function debug(context is Context, boundingBox is Box3d, cSys)
{
    debug(context, boundingBox, cSys, DebugColor.RED);
}

/**
 * Highlights `entities` in a given [DebugColor], without printing anything.
 *
 * As with [debug], highlighted entities are only visible while the debugged feature's edit dialog is open.
 * @param color : @autocomplete `DebugColor.RED`
 */
export function addDebugEntities(context is Context, entities is Query, color is DebugColor)
{
    @addDebugEntities(context, { "entities" : entities, "color" : color });
}

export function addDebugEntities(context is Context, entities is Query)
{
    addDebugEntities(context, entities, DebugColor.RED);
}

/**
 * Highlights a 3D `point` in a given [DebugColor], without printing anything.
 *
 * As with [debug], highlighted entities are only visible while the debugged feature's edit dialog is open.
 * @param color : @autocomplete `DebugColor.RED`
 */
export function addDebugPoint(context is Context, point is Vector, color is DebugColor)
precondition
{
    is3dLengthVector(point);
}
{
    const pointId = getLastActiveId(context) + DEBUG_ID_STRING + "point";
    startFeature(context, pointId, {});
    try
    {
        opPoint(context, pointId, { "point" : point });
        addDebugEntities(context, qCreatedBy(pointId), color);
    }
    abortFeature(context, pointId);
}

export function addDebugPoint(context is Context, point is Vector)
{
    addDebugPoint(context, point, DebugColor.RED);
}

/**
 * Draws a line in 3D space from `point1` to `point2` with a chosen [DebugColor].
 *
 * As with [debug], highlighted entities are only visible while the debugged feature's edit dialog is open.
 *
 * @param point1: one endpoint of the line.
 * @param point2: the other endpoint of the line.
 * @param color : @autocomplete `DebugColor.RED`
 */
export function addDebugLine(context is Context, point1 is Vector, point2 is Vector, color is DebugColor)
{
    const lineId = getLastActiveId(context) + DEBUG_ID_STRING + "line";
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

        addDebugEntities(context, qCreatedBy(lineId, EntityType.EDGE), color);
    }
    abortFeature(context, lineId);
}

export function addDebugLine(context is Context, point1 is Vector, point2 is Vector)
{
    addDebugLine(context, point1, point2, DebugColor.RED);
}

/**
 * Draws an arrow in 3D space from `from` to `to` with a chosen [DebugColor].
 *
 * As with [debug], highlighted entities are only visible while the debugged feature's edit dialog is open.
 *
 * @param radius : Width of the four arrowhead lines @eg `.25 * centimeter`
 * @param color : @autocomplete `DebugColor.RED`
 */
export function addDebugArrow(context is Context, from is Vector, to is Vector, radius is ValueWithUnits, color is DebugColor)
{
    const arrowId = getLastActiveId(context) + DEBUG_ID_STRING + "arrow";
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

        addDebugEntities(context, qCreatedBy(arrowId, EntityType.EDGE), color);
    }
    abortFeature(context, arrowId);
}

export function addDebugArrow(context is Context, from is Vector, to is Vector, radius is ValueWithUnits)
{
    addDebugArrow(context, from, to, radius, DebugColor.RED);
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

