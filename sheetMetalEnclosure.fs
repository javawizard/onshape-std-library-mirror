FeatureScript 392; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */

import(path : "onshape/std/attributes.fs", version : "392.0");
import(path : "onshape/std/box.fs", version : "392.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "392.0");
import(path : "onshape/std/containers.fs", version : "392.0");
import(path : "onshape/std/coordSystem.fs", version : "392.0");
import(path : "onshape/std/curveGeometry.fs", version : "392.0");
import(path : "onshape/std/evaluate.fs", version : "392.0");
import(path : "onshape/std/feature.fs", version : "392.0");
import(path : "onshape/std/geomOperations.fs", version : "392.0");
import(path : "onshape/std/manipulator.fs", version : "392.0");
import(path : "onshape/std/math.fs", version : "392.0");
import(path : "onshape/std/sketch.fs", version : "392.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "392.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "392.0");
import(path : "onshape/std/string.fs", version : "392.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "392.0");
import(path : "onshape/std/query.fs", version : "392.0");
import(path : "onshape/std/tool.fs", version : "392.0");
import(path : "onshape/std/valueBounds.fs", version : "392.0");
import(path : "onshape/std/vector.fs", version : "392.0");

export import(path : "onshape/std/query.fs", version : "392.0");
export import(path : "onshape/std/sheetMetalFlange.fs", version : "392.0");
export import(path : "onshape/std/sheetMetalRecognize.fs", version : "392.0");

const HEIGHT_MANIPULATOR = "heightManipulator";
const WIDTH_MANIPULATOR = "widthManipulator";
const MULTIPLE_MANIPULATORS = false;

const ANGLE_90_DEGREE = 90 * degree;

/**
* @internal
*/
export enum SMEnclosureType
{
    annotation { "Name" : "Use profile" }
    PROFILE,
    annotation { "Name" : "Enclose parts" }
    PARTS
}

/**
* @internal
*/
annotation { "Feature Type Name" : "smEnclosure",
             "Manipulator Change Function" : "smEnclosureManipulatorChange" }
export const smEnclosure = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Enclosure type" }
        definition.SMEnclosureType is SMEnclosureType;
        if (definition.SMEnclosureType == SMEnclosureType.PROFILE)
        {
            annotation { "Name" : "Height", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            isLength(definition.height, LENGTH_BOUNDS);

            annotation { "Name" : "Bend type", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            definition.bendType is smFlangeJointType;
        }
        else
        {
            annotation { "Name" : "Margin", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            isLength(definition.margin, LENGTH_BOUNDS);
        }

        annotation { "Name" : "Thickness", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        isLength(definition.thickness, SHELL_OFFSET_BOUNDS);

        annotation { "Name" : "Inner radius", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        isLength(definition.innerRadius, BLEND_BOUNDS);

        annotation { "Name" : "Top flange", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        definition.topFlange is boolean;

        if (definition.topFlange)
        {
            annotation { "Name" : "Width", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            isLength(definition.width, LENGTH_BOUNDS);
        }

        annotation { "Name" : "Cover", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        definition.cover is boolean;

        if (definition.SMEnclosureType == SMEnclosureType.PROFILE)
        {
            annotation { "Name" : "Faces", "Filter" : EntityType.FACE && GeometryType.PLANE }
            definition.faces is Query;
        }
        else
        {
            annotation { "Name" : "parts", "Filter" : EntityType.BODY }
            definition.parts is Query;

            annotation { "Name" : "Reference mate connector",
                         "Filter" : BodyType.MATE_CONNECTOR,
                         "MaxNumberOfPicks" : 1 }
            definition.refConnector is Query;
        }
    }
    {
        if (definition.SMEnclosureType == SMEnclosureType.PROFILE)
        {
            makeProfileEnclosure(context, id, definition);
        }
        else
        {
            makeBoundBoxEnclosure(context, id, definition);
        }
    });

function makeBase(context is Context, id is Id, index is number, entities is Query, normal is Vector, definition is map) returns Query
{
    opExtrude(context, id + ("extrude_base_" ~ index), {
        "entities" : entities,
        "direction" : normal,
        "endBound" : BoundingType.BLIND,
        "endDepth" : definition.thickness
    });
    smRecognize(context, id + ("recognize_" ~ index), {
        bodies : qCreatedBy(id + ("extrude_base_" ~ index), EntityType.BODY)
    });

    var topolQuery = makeQuery(id + ("extrude_base_" ~ index), "CAP_FACE", EntityType.FACE,{});
    return topolQuery;
}

function makeCover(context is Context, id is Id, index is number, entities is Query, normal is Vector, definition is map)
{
    opExtrude(context, id + ("extrude_cover_" ~ index), {
        "entities" : entities,
        "direction" : normal,
        "startBound" : BoundingType.BLIND,
        "startDepth" : -definition.height + definition.thickness,
        "endBound" : BoundingType.BLIND,
        "endDepth" : definition.height
    });
    smRecognize(context, id + ("recognize_cover_" ~ index), {
        bodies : qCreatedBy(id + ("extrude_cover_" ~ index), EntityType.BODY)
    });
    var growSize = 0 * meter;
    if (definition.bendType == smFlangeJointType.NONE)
        growSize = definition.innerRadius + definition.thickness;
    else if (definition.bendType == smFlangeJointType.FLUSH_INNER_INNER)
        growSize = definition.thickness;
    else if (definition.bendType == smFlangeJointType.FLUSH_OUTER_INNER)
        growSize = 0 * meter;
    else if (definition.bendType == smFlangeJointType.FLUSH_INNER_OUTER)
        growSize = 2 * definition.thickness;
    if (growSize != 0)
    {
        var faces = qSubtraction(qCreatedBy(id + ("extrude_cover_" ~ index), EntityType.FACE),
        qUnion([
            qCapEntity(id + ("extrude_cover_" ~ index), true),
            qCapEntity(id + ("extrude_cover_" ~ index), false)
        ]));
        opOffsetFace(context, id + ("offsetFace_" ~ index), {
            "moveFaces" : faces,
            "offsetDistance" : growSize
        });
    }
}

function isFaceRectangular(context is Context, face is Query) returns boolean
{
    var edges = evaluateQuery(context, qEdgeAdjacent(face, EntityType.EDGE));
    var lines = [];
    for (var edge in edges)
    {
        try
        {
            var line = evLine(context, {
                    "edge" : edge
            });
            lines = append(lines, line);
        }
        catch
        {
            return false;
        }
    }
    if (size(lines) != 4)
        return false;
    var numPerpendicular = 0;
    var numParallel = 0;
    for (var i = 1; i < size(lines); i += 1)
    {
        if (squaredNorm(cross(lines[0].direction, lines[i].direction)) < TOLERANCE.zeroAngle * TOLERANCE.zeroAngle)
        {
            numParallel += 1;
        }
        else if (dot(lines[0].direction, lines[i].direction) < TOLERANCE.zeroAngle)
        {
            numPerpendicular += 1;
        }
    }
    return numParallel == 1 && numPerpendicular == 2;
}

function makeProfileEnclosure(context is Context, id is Id, definition is map)
{
    definition.bendAngle = ANGLE_90_DEGREE;
    definition.oppositeDirection = false;
    var index = -1;
    var faces = evaluateQuery(context, definition.faces);
    for (var face in faces)
    {
        index += 1;
        if (!isFaceRectangular(context, face))
            throw regenError(ErrorStringEnum.FACE_IS_NOT_RECTANGLE);
        var facePlane = evPlane(context, {
                "face" : face
        });
        var topolQuery = makeBase(context, id, index, face, facePlane.normal, definition);
        if (index == 0)
        {
            var edges = evaluateQuery(context, qEdgeAdjacent(topolQuery, EntityType.EDGE));
            addHeightManipulator(context, id, edges, definition);
            addTopFlangeWidthManipulator(context, id, edges, definition);
        }
        var cSys = coordSystem(facePlane.origin, facePlane.x, facePlane.normal);
        makeEnclosure(context, id, topolQuery, cSys, definition);
        if (definition.cover)
        {
            makeCover(context, id, index, face, facePlane.normal, definition);
        }
    }
}

function makeBoundBoxEnclosure(context is Context, id is Id, definition is map)
{
    var cSys = coordSystem(vector(0, 0, 0) * meter, vector(1, 0, 0), vector(0, 0, 1));
    var mateConnectors = evaluateQuery(context, definition.refConnector);
    if (size(mateConnectors) == 1)
    {
        cSys = evMateConnector(context, {
            "mateConnector" : definition.refConnector
        });
    }
    var bBox = evBox3d(context, {
        "topology" : definition.parts,
        "cSys" : cSys
    });

    bBox = extendBox3d(bBox, definition.margin, 0);
    var width = bBox.maxCorner[0] - bBox.minCorner[0];
    var depth = bBox.maxCorner[1] - bBox.minCorner[1];
    var height = bBox.maxCorner[2] - bBox.minCorner[2];

    var origin = toWorld(cSys, bBox.minCorner);
    var sketchCSys = coordSystem(origin, cSys.xAxis, cSys.zAxis);
    var sketch = newSketchOnPlane(context, id + "enclosure_sketch", {
        "sketchPlane" : plane(sketchCSys)
    });
    skRectangle(sketch, "base", {
        "firstCorner" : vector(0, 0) * inch,
        "secondCorner" : vector(width, depth)
    });

    skSolve(sketch);

    var topolQuery = makeBase(context, id, 0, qSketchRegion(id + "enclosure_sketch", true), cSys.zAxis, definition);

    var edges = evaluateQuery(context, qEdgeAdjacent(topolQuery, EntityType.EDGE));
    definition.height = height;
    addTopFlangeWidthManipulator(context, id, edges, definition);
    makeEnclosure(context, id, topolQuery, cSys, definition);
    if (definition.cover)
    {
        makeCover(context, id, 0, qSketchRegion(id + "enclosure_sketch", true), cSys.zAxis, definition);
    }
}

function makeEnclosure(context is Context, id is Id, topolQuery is Query, cSys is CoordSystem, definition is map)
{
    if (definition.cover)
    {
        definition.height -= definition.thickness;
    }

    var flangeResult = smFlangeFunction(context, id + "base_flange", false, {
        "innerRadius" : definition.innerRadius,
        "height" : definition.height,
        "bendAngle" : ANGLE_90_DEGREE,
        "bendType" : definition.bendType == undefined ? smFlangeJointType.FLUSH_INNER_INNER : definition.bendType,
        "flushSides" : true,
        "mitered" : false,
        "oppositeDirection" : false,
        "relief" : false,
        "topols" : topolQuery
    });
    if (definition.topFlange)
    {
        smFlange(context, id + "top_flange", {
            "innerRadius" : definition.innerRadius,
            "height" : definition.width,
            "bendAngle" : ANGLE_90_DEGREE,
            "bendType" : smFlangeJointType.FLUSH_OUTER_INNER,
            "flushSides" : false,
            "mitered" : true,
            "miterBend" : true,
            "miterAngle" : 45 * degree,
            "oppositeDirection" : false,
            "relief" : false,
            "topols" : flangeResult.innerEdgeQueries
        });
    }
}

function addHeightManipulator(context is Context, id is Id, edges is array, definition is map)
{
    if (size(edges) == 0)
        return;
    var i = -1;
    for (var edge in edges)
    {
        i += 1;
        if (i == 0 || MULTIPLE_MANIPULATORS)
        {
            var cSys = getEdgeCSys(context, edge);
            var yAxis = yAxis(cSys);
            const usedEntities = edge;
            var v = cos(definition.bendAngle) * -yAxis + sin(definition.bendAngle) * cSys.zAxis;

            var offset = definition.height;
            addManipulators(context, id, { (HEIGHT_MANIPULATOR ~ "_" ~ i) :
                            linearManipulator(cSys.origin,
                                v,
                                offset,
                                usedEntities) });
        }
    }
}

function addTopFlangeWidthManipulator(context is Context, id is Id, edges is array, definition is map)
{
    if (size(edges) == 0)
        return;
    var i = -1;
    for (var edge in edges)
    {
        i += 1;
        if (i == 0 || MULTIPLE_MANIPULATORS)
        {
            var cSys = getEdgeCSys(context, edge);
            var yAxis = yAxis(cSys);
            const usedEntities = edge;
            var angle = definition.bendAngle == undefined ? ANGLE_90_DEGREE : definition.bendAngle;
            var v0 = cos(angle) * -yAxis + sin(angle) * cSys.zAxis;
            var v1 = -sin(angle) * -yAxis + cos(angle) * cSys.zAxis;

            var offset = definition.width;
            addManipulators(context, id, { (WIDTH_MANIPULATOR ~ "_" ~ i) :
                            linearManipulator(cSys.origin + definition.height * v0 - (definition.innerRadius + 2 * definition.thickness) * v1,
                                v1,
                                offset,
                                usedEntities) });
        }
    }
}

/**
 * @internal
 */
export function smEnclosureManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    for (var entry in newManipulators)
    {
        var matchResult;

        matchResult = match(entry.key, HEIGHT_MANIPULATOR ~ "_.*" );
        if (matchResult.hasMatch && newManipulators[entry.key] is map)
        {
            var newOffset = newManipulators[entry.key].offset;
            definition.height = abs(newOffset);
        }

        matchResult = match(entry.key, WIDTH_MANIPULATOR ~ "_.*" );
        if (matchResult.hasMatch && newManipulators[entry.key] is map)
        {
            definition.width = newManipulators[entry.key].offset;
        }
    }

    return definition;
}

