FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "✨");
import(path : "onshape/std/boundingtype.gen.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/coordSystem.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");
import(path : "onshape/std/manipulator.fs", version : "✨");
import(path : "onshape/std/query.fs", version : "✨");
import(path : "onshape/std/sketch.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/smobjecttype.gen.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/tool.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */


/**
 * @internal
 */
export enum ToleranceLevel
{
    annotation { "Name" : "Very tight" }
    VERY_TIGHT,
    annotation { "Name" : "Tight" }
    TIGHT,
    annotation { "Name" : "Medium" }
    MEDIUM,
    annotation { "Name" : "Loose" }
    LOOSE
}

/**
 * @internal
 */
export function getEdgeCSys(context is Context, edge is Query) returns CoordSystem
{
    var faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
    if (size(faces) != 2)
        throw regenError("Bad edge");
    var offsetData = findOffsetEdgeData(context, edge);

    var topPlane = evPlane(context, {
            "face" : offsetData.topFace
    });

    var sidePlane = evPlane(context, {
            "face" : offsetData.sideFace
    });

    var edgeLine = evEdgeTangentLine(context, {
            "edge" : edge,
            "parameter" : 0.5
    });

    var normal = cross(sidePlane.normal, edgeLine.direction);
    if (dot(normal, topPlane.normal) < 0)
    {
        edgeLine.direction = -edgeLine.direction;
    }
    return coordSystem(edgeLine.origin, edgeLine.direction, topPlane.normal);
}

/**
 * @internal
 */
// TODO BRT - Change this to use sheet metal attributes instead of geometry.
export function findOffsetEdgeData(context is Context, edge is Query) returns map
{
    const BIG_NUMBER = 1.0e20 * meter;
    var attr = getAttributes(context, {
            "entities" : qOwnerBody(edge)
    });
    if (size(attr) != 1 || attr[0].thickness == undefined || attr[0].thickness.value == undefined)
        throw regenError("Bad sheet metal attribute");
    var offsetDistance = attr[0].thickness.value;
    if (offsetDistance is number)
        offsetDistance *= meter;
    var edgeLine = evEdgeTangentLine(context, {
            "edge" : edge,
            "parameter" : 0.0
    });
    var edgeLength = evLength(context, {
            "entities" : edge
    });
    var minDistance = BIG_NUMBER;
    var offsetEdge;
    var faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
    if (size(faces) != 2)
        throw regenError("Could not find offset data");

    var topFace;
    var sideFace;
    if (hasSheetMetalAttribute(context, faces[0], SMObjectType.WALL))
    {
        topFace = faces[0];
        sideFace = faces[1];
    }
    else
    {
        topFace = faces[1];
        sideFace = faces[0];
    }
    var edges = evaluateQuery(context, qEdgeAdjacent(sideFace, EntityType.EDGE));
    for (var testEdge in edges)
    {
        if (testEdge == edge)
            continue;
        var testEdgeLine = evEdgeTangentLine(context, {
                "edge" : testEdge,
                "parameter" : 0.0
        });
        if (!tolerantParallel(edgeLine, testEdgeLine) || tolerantCoLinear(edgeLine, testEdgeLine))
            continue;
        var testEdgeLength = evLength(context, {
                "entities" : testEdge
        });
        if (abs(testEdgeLength - edgeLength) > TOLERANCE.zeroLength * meter)
            continue;
        var v = testEdgeLine.origin - edgeLine.origin;
        v = v - edgeLine.direction * dot(edgeLine.direction, v);
        var distance = norm(v);
        if (distance < minDistance)
        {
            minDistance = distance;
            offsetEdge = testEdge;
        }
    }

    if (minDistance == BIG_NUMBER)
        throw regenError("Could not find offset edge");

    return {
        "offsetEdge" : offsetEdge,
        "offsetDistance" : minDistance,
        "sideFace" : sideFace,
        "topFace" : topFace
    };
}

/**
 * @internal
 */
export function tolerantParallel(line0 is Line, line1 is Line) returns boolean
{
    return squaredNorm(cross(line0.direction, line1.direction)) < TOLERANCE.zeroAngle * TOLERANCE.zeroAngle;
}

/**
 * @internal
 */
export function tolerantCoLinear(line0 is Line, line1 is Line) returns boolean
{
    if (tolerantParallel(line0, line1)) {
        var v = line1.origin - line0.origin;
        v = v - line0.direction * dot(v, line0.direction);
        var lengthTolerance = TOLERANCE.zeroLength * meter;
        return squaredNorm(v) < lengthTolerance * lengthTolerance;
    }
    return false;
}

/**
 * @internal
 */
export function hasSheetMetalAttribute(context is Context, entities is Query, objectType is SMObjectType) returns boolean
{
    return size(getSmObjectTypeAttributes(context, entities, objectType)) != 0;
}

