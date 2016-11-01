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
import(path : "onshape/std/sketch.fs", version : "✨");
import(path : "onshape/std/smobjecttype.gen.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/tool.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");

export import(path : "onshape/std/query.fs", version : "✨");
export import(path : "onshape/std/sheetMetalBend.fs", version : "✨");

const RIP_GAP = 0.0005 * inch;
const HEIGHT_MANIPULATOR = "heightManipulator";
const ANGLE_MANIPULATOR = "angleManipulator";
const WIDTH_MANIPULATOR = "widthManipulator";
const MULTIPLE_MANIPULATORS = false;

const FLANGE_ANGLE_BOUNDS =
{
    (degree) : [0, 90, 180],
    (radian) : 1
} as AngleBoundSpec;

/**
* @internal
*/
export enum smFlangeJointType
{
    annotation { "Name" : "None" }
    NONE,
    annotation { "Name" : "Inner flush with inner" }
    FLUSH_INNER_INNER,
    annotation { "Name" : "Outer flush with inner" }
    FLUSH_OUTER_INNER,
    annotation { "Name" : "Inner flush with outer" }
    FLUSH_INNER_OUTER

}

/**
* @internal
*  This feature produces a sheet metal flange
*/
annotation { "Feature Type Name" : "smFlangeOld",
             "Manipulator Change Function" : "flangeManipulatorChange" }
export const smFlangeOld = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Height", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        isLength(definition.height, NONNEGATIVE_LENGTH_BOUNDS);

        annotation { "Name" : "Inner radius", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        isLength(definition.innerRadius, BLEND_BOUNDS);

        annotation { "Name" : "Bend angle", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        isAngle(definition.bendAngle, FLANGE_ANGLE_BOUNDS);

        annotation { "Name" : "Bend type", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        definition.bendType is smFlangeJointType;

        annotation { "Name" : "Mitered", "UIHint" : "REMEMBER_PREVIOUS_VALUE", "Default" : false }
        definition.mitered is boolean;

        if (definition.mitered)
        {
            annotation { "Name" : "Miter bend", "UIHint" : "REMEMBER_PREVIOUS_VALUE", "Default" : false }
            definition.miterBend is boolean;

            annotation { "Name" : "Symmetric miter", "UIHint" : "REMEMBER_PREVIOUS_VALUE", "Default" : true }
            definition.symmetricMiter is boolean;
            if (definition.symmetricMiter)
            {
                annotation { "Name" : "Miter angle", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
                isAngle(definition.miterAngle, ANGLE_360_BOUNDS);
            }
            else
            {
                annotation { "Name" : "Left miter angle", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
                isAngle(definition.leftMiterAngle, ANGLE_360_BOUNDS);
                annotation { "Name" : "Right miter angle", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
                isAngle(definition.rightMiterAngle, ANGLE_360_BOUNDS);
            }
        }
        else
        {
            annotation { "Name" : "Flush sides", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            definition.flushSides is boolean;
        }
        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Edges and faces", "Filter" : (EntityType.EDGE && GeometryType.LINE && SketchObject.NO) ||
                                                  (EntityType.FACE && GeometryType.PLANE && SketchObject.NO) && BodyType.SOLID }
        definition.topols is Query;
    }
    {
        smFlangeFunction(context, id, true, definition);
    },{
        "mitered" : false,
        "symmetricMiter" : true
    });

/**
* @internal
*/
export function smFlangeFunction(context is Context, id is Id, wantManipulators is boolean, definition is map) returns map
{
    var innerEdgeQueries = [];
    var outerEdgeQueries = [];
    var edges = getSelectedEdges(context, definition);
    edges = orientEdges(context, edges, definition);
    if (size(edges) != 0)
    {
        edges = makeRelief(context, id, edges, definition);

        if (wantManipulators)
        {
        // Add manipulator
        addHeightManipulator(context, id, edges, definition);
        addAngleManipulator(context, id, edges, definition);
        }
        var index = -1;
        for (var edge in edges)
        {
            index += 1;
            var flangeResult = makeFlange(context, id, index, edge, definition);
            innerEdgeQueries = append(innerEdgeQueries, flangeResult.innerEdgeQuery);
            outerEdgeQueries = append(outerEdgeQueries, flangeResult.outerEdgeQuery);
        }
    }
    return {
        "innerEdgeQueries" : qUnion(innerEdgeQueries),
        "outerEdgeQueries" : qUnion(outerEdgeQueries)
    };
}

function getSelectedEdges(context is Context, definition is map) returns array
{
    var faces = evaluateQuery(context, qEntityFilter(definition.topols, EntityType.FACE));
    var edges = evaluateQuery(context, qEntityFilter(definition.topols, EntityType.EDGE));
    for (var face in faces)
    {
        var faceEdges = evaluateQuery(context, qGeometry(qEdgeAdjacent(face, EntityType.EDGE), GeometryType.LINE));
        edges = concatenateArrays([edges, faceEdges]);
    }
    return edges;
}

/*
 * Auto relief cut when needed. Doesn't work because the adjacent faces are not exactly smooth.
 */
function makeRelief(context is Context, id is Id, edges is array, definition is map) returns array
{
    var result = [];
    var sketchQueries = [];
    var index = -1;
    for (var edge in edges)
    {
        index += 1;
        var offsetData = findOffsetEdgeData(context, edge);
        var thickness = offsetData.offsetDistance;
        var edgeLine = evEdgeTangentLine(context, {
                "edge" : edge,
                "parameter" : 0.5
        });
        var topPlane = evPlane(context, {
                "face" : offsetData.topFace
        });
        var sidePlane = evPlane(context, {
                "face" : offsetData.sideFace
        });
        var testNormal = cross(sidePlane.normal, edgeLine.direction);
        if (dot(topPlane.normal, testNormal) < 0)
            edgeLine.direction = -edgeLine.direction;

        var bBox = evBox3d(context, {
            "topology" : offsetData.sideFace
        });
        var faceCenter = 0.5 * (bBox.minCorner + bBox.maxCorner);


        var length = evLength(context, {
                "entities" : edge
        });
        var vertices = evaluateQuery(context, qVertexAdjacent(edge, EntityType.VERTEX));
        if (size(vertices) != 2)
            continue;
        var p0 = evVertexPoint(context, {
                "vertex" : vertices[0]
        });
        var p1 = evVertexPoint(context, {
                "vertex" : vertices[1]
        });
        var v = faceCenter - 0.5 * (p0 + p1);
        v = normalize(v - edgeLine.direction * dot(v, edgeLine.direction));

        sidePlane.origin = edgeLine.origin;
        sidePlane.x = edgeLine.direction;

        var sketch = newSketchOnPlane(context, id + ("relief_sketch_" ~ index), {
            "sketchPlane" : sidePlane
        });
        sketchQueries = append(sketchQueries, qCreatedBy(id + ("relief_sketch_" ~ index), EntityType.BODY));
        var halfLength = length / 2 + definition.innerRadius + offsetData.offsetDistance;
        skRectangle(sketch, "relief_rectangle", {
            "firstCorner" : vector(-halfLength, -offsetData.offsetDistance),
            "secondCorner" : vector(halfLength, definition.innerRadius)
        });
        skSolve(sketch);

        var extrudeDistance = calculateOffset(offsetData.offsetDistance, definition);
        if (extrudeDistance < TOLERANCE.zeroLength * meter)
        {
            result = append(result, edge);
            continue;
        }

        opExtrude(context, id + ("relief_extrude_" ~ index), {
            "entities" : qSketchRegion(id + ("relief_sketch_" ~ index), true),
            "direction" : -sidePlane.normal,
            "endBound" : BoundingType.BLIND,
            "endDepth" : extrudeDistance
        });
        opBoolean(context, id + ("relief_cut_" ~ index), {
            "eraseImprintedEdges" : false,
            "localizedInFaces" : true,
            "targets" : qEdgeAdjacent(qEdgeAdjacent(offsetData.sideFace, EntityType.FACE), EntityType.FACE),
            "tools" : qCreatedBy(id + ("relief_extrude_" ~ index), EntityType.FACE),
            "operationType" : BooleanOperationType.SUBTRACTION
        });

        var newFaces = evaluateQuery(context, qUnion([
            qCreatedBy(id + ("relief_cut_" ~ index), EntityType.FACE),
            qCreatedBy(id + ("relief_extrude_" ~ index), EntityType.FACE)
        ]));
        var j = -1;
        for (var face in newFaces)
        {
            var area = evArea(context, {
                    "entities" : face
            });
            if (area < 1.5 * thickness * thickness)
            {
                j += 1;
                try
                {
                    opDeleteFace(context, id + ("deleteFace_" ~ index ~ "_" ~ j), {
                            "deleteFaces" : face,
                            "includeFillet" : false,
                            "capVoid" : false
                    });
                }
            }
        }

        var newEdge = qNothing();
        var newEdges = evaluateQuery(context,qIntersection([
            qUnion([
                qCreatedBy(id + ("relief_extrude_" ~ index), EntityType.EDGE),
                qCreatedBy(id + ("relief_cut_" ~ index), EntityType.EDGE)]),
            qEdgeAdjacent(offsetData.topFace, EntityType.EDGE)
        ]));
        if (size(newEdges) == 1)
        {
            newEdge = newEdges[0];
            result = append(result, newEdge);
        }
    }

    opDeleteBodies(context, id + "delete_relief_sketches", {
            "entities" : qUnion(sketchQueries)
    });

    return result;
}

function calculateOffset(offsetDistance is ValueWithUnits, definition is map)
{
    var offset = 0 * meter;
    if (definition.bendType == smFlangeJointType.FLUSH_INNER_INNER)
        offset = definition.innerRadius;
    else if (definition.bendType == smFlangeJointType.FLUSH_OUTER_INNER)
        offset = definition.innerRadius + offsetDistance;
    else if (definition.bendType == smFlangeJointType.FLUSH_INNER_OUTER)
        offset = definition.innerRadius - offsetDistance - 2 *RIP_GAP;
    return offset;
}

function makeFlange(context is Context, id is Id, index is number, edge is Query, definition is map) returns map
{
    var result = {};
    var faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
    if (size(faces) != 2)
        throw regenError("Edges does not have two faces");
    for (var face in faces)
    {
        var surfaceDefinition = evSurfaceDefinition(context, {
                "face" : face
        });
        if (!(surfaceDefinition is Plane))
            throw regenError("One or more adjacent faces are not planes");
    }

    var offsetEdgeData = findOffsetEdgeData(context, edge);
    if (offsetEdgeData.offsetEdge == undefined)
        throw regenError("Failed to find an offset edge");

    var offsetEdge = offsetEdgeData.offsetEdge;
    var thickness = offsetEdgeData.offsetDistance;
    var sideFace = offsetEdgeData.sideFace;

    if (definition.mitered)
        result = makeMiteredFlange(context, id + index, edge, offsetEdge, thickness, sideFace, definition);
        // Mitered flange calls smBend, which sets the bend attributes
    else
    {
        result = makeSimpleFlange(context, id + index, edge, offsetEdge, thickness, sideFace, definition);
        setBendAttributes(context, id, index, definition);
    }
    return result;
}

function setBendAttributes(context is Context, id is Id, index is number, definition is map)
{
    var cylFaces = evaluateQuery(context, qGeometry(qCreatedBy(id + index, EntityType.FACE), GeometryType.CYLINDER));
    var attributeId = toAttributeId(id + index);
    var wallAttrib = makeSMWallAttribute(attributeId);
    var bendAttribute = makeSMJointAttribute(attributeId);
    bendAttribute.jointType = { "value" : "BEND", "canBeEdited": true };

    bendAttribute.radius = {
        "value" : definition.innerRadius,
        "controllingFeatureId" : toAttributeId(id),
        "parameterIdInFeature" : "radius",
        "canBeEdited" : true
    };
    for (var cylFace in cylFaces)
    {
        setAttribute(context, {
            "entities" : cylFace,
            "attribute" : bendAttribute
        });

        var edges = qGeometry(qEdgeAdjacent(cylFace, EntityType.EDGE), GeometryType.LINE);
        var walls = evaluateQuery(context, qSubtraction(qEdgeAdjacent(edges, EntityType.FACE), cylFace));
        for (var wall in walls)
        {
            if (!hasSheetMetalAttribute(context, wall, SMObjectType.WALL))
            {
                setAttribute(context, {
                    "entities" : wall,
                    "attribute" : wallAttrib
                });
            }
        }
    }
}

function makeMiteredFlange(context is Context, id is Id, edge is Query, offsetEdge is Query, thickness is ValueWithUnits, sideFace is Query, definition is map) returns map
{
    var result = makeTab(context, id, edge, offsetEdge, thickness, sideFace, definition);
    var seedQuery = makeQuery(id + "flange", "CAP_EDGE", EntityType.EDGE ,{
        "disambiguationData":[
            originalSetDisambiguation([
                sketchEntityQuery(id + "top_sketch", EntityType.EDGE, "top")
            ])
        ], "isStart":true });
    smBend(context, id + "bend", {
        "deformationForm" : SMDeformationForm.KFACTOR,
        "kFactor" : 0.5,
        "radius" : definition.innerRadius,
        "angle" : definition.bendAngle,
        "bendEdge" : edge,
        "seedEntity" : seedQuery
    });
    return result;
}

function makeTab(context is Context, id is Id, edge is Query, offsetEdge is Query, thickness is ValueWithUnits, sideFace is Query, definition is map)
{
    var edgeLine = evLine(context, {
            "edge" : edge
    });
    var extraWidth = 0 * meter;

    var length = evLength(context, {
            "entities" : edge
    }) / 2 + extraWidth;

    var adjFaces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
    var topFace;
    if (adjFaces[0] == sideFace)
        topFace = adjFaces[1];
    else
        topFace = adjFaces[0];
    var sidePlane = evPlane(context, {
            "face" : sideFace
    });
    var topPlane = evPlane(context, {
            "face" : topFace
    });
    var sketchId = id + "top_sketch";
    var yAxis = cross(sidePlane.normal, topPlane.normal);
    var cSys = coordSystem(edgeLine.origin, yAxis, topPlane.normal);
    var sketch = newSketchOnPlane(context, sketchId, { "sketchPlane" : plane(cSys) });

    const midRadius = definition.innerRadius + thickness / 2;
    var miterAngle0 = definition.miterAngle;
    var miterAngle1 = miterAngle0;
    if (!definition.symmetricMiter)
    {
        miterAngle0 = definition.leftMiterAngle;
        miterAngle1 = definition.rightMiterAngle;
    }

    const sign = miterAngle0 < 0 ? -1 : 1;
    const sign1 = miterAngle1 < 0 ? -1 : 1;
    const radius0 = miterAngle0 > 0 ? definition.innerRadius : definition.innerRadius + thickness;
    const radius1 = miterAngle1 > 0 ? definition.innerRadius : definition.innerRadius + thickness;
    var height = definition.height - thickness;

    if (definition.miterBend)
    {
        var p0 = vector(-length, 0 * meter);
        var p1 = vector(length, 0 * meter);
        var p2 = ptAtAngle(p1, -1, radius0, midRadius, miterAngle0, definition.bendAngle);
        var p3 = vector(length, 0 * meter) + vector(-height * tan(miterAngle0), height + midRadius * definition.bendAngle / radian - radius0);
        var p4 = vector(-length, 0 * meter) + vector(height * tan(miterAngle1), height + midRadius * definition.bendAngle / radian - radius1);
        var p5 = ptAtAngle(p0, 1, radius1, midRadius, miterAngle1, definition.bendAngle);

        skLineSegment(sketch, "bottom", { "start" : p0, "end" : p1 });
        addSpline(sketch, "right_bend", p1, -1, radius0, midRadius, miterAngle0, definition);
        skLineSegment(sketch, "right", { "start" : p2, "end" : p3 });
        skLineSegment(sketch, "top", { "start" : p3, "end" : p4 });
        skLineSegment(sketch, "left", { "start" : p4, "end" : p5 });
        addSpline(sketch, "left_bend", p0, 1, radius1, midRadius, miterAngle1, definition);
    }
    else
    {
        var y = definition.height - (definition.innerRadius + thickness);
        var l0 = y / sin(miterAngle0);
        var l1 = y / sin(miterAngle1);
        var p0 = vector(-length, 0 * meter);
        var p1 = vector(length, 0 * meter);
        var p2 = p1 + vector(0 * meter, midRadius * definition.bendAngle / radian);
        var p3 = p2 + l0 * vector(-cos(miterAngle0), sin(miterAngle0));
        var p5 = p0 + vector(0 * meter, midRadius * definition.bendAngle / radian);
        var p4 = p5 + l1 * vector(cos(miterAngle1), sin(miterAngle1));

        skLineSegment(sketch, "bottom", { "start" : p0, "end" : p1 });
        skLineSegment(sketch, "right_flat", { "start" : p1, "end" : p2 });
        skLineSegment(sketch, "right_slope", { "start" : p2, "end" : p3 });
        skLineSegment(sketch, "top", { "start" : p3, "end" : p4 });
        skLineSegment(sketch, "left_slope", { "start" : p4, "end" : p5 });
        skLineSegment(sketch, "left_flat", { "start" : p5, "end" : p0 });
    }
    skSolve(sketch);

    opExtrude(context, id + "flange", {
            "entities" : qSketchRegion(sketchId, true),
            "direction" : -topPlane.normal,
            "startBound" : BoundingType.BLIND,
            "startDepth" : 0,
            "endBound" : BoundingType.BLIND,
            "endDepth" : thickness
    });

    opBoolean(context, id + "boolean_top_flange", {
        "eraseImprintedEdges" : false,
        "localizedInFaces" : true,
        "targets" : qEdgeAdjacent(edge, EntityType.FACE),
        "tools" : qCreatedBy(id + "flange", EntityType.FACE),
        "operationType" : BooleanOperationType.UNION
    });

    opDeleteBodies(context, id + "delete_flange_sketches", {
        "entities" : qCreatedBy(sketchId, EntityType.BODY)
    });

    return {
        "innerEdgeQuery" : makeQuery(id + "flange", "CAP_EDGE", EntityType.EDGE, {
                "disambiguationData":[
                    originalSetDisambiguation([
                        sketchEntityQuery(id + "top_sketch", EntityType.EDGE, "top")
                    ])
                ], "isStart" : true}),
        "outerEdgeQuery" : makeQuery(id + "flange", "CAP_EDGE", EntityType.EDGE, {
                "disambiguationData":[
                    originalSetDisambiguation([
                        sketchEntityQuery(id + "top_sketch", EntityType.EDGE, "top")
                    ])
                ], "isStart" : false})
    };
}

function addSpline(sketch is Sketch, label is string, startPoint is Vector, sign is number, radius is ValueWithUnits, midRadius is ValueWithUnits, miterAngle is ValueWithUnits, definition is map)
{
    var points = [];
    var samples = round(10 * definition.bendAngle / (PI / 2 * radian));
    if (samples < 3)
        samples = 3;
    for (var i = 0; i < samples; i += 1)
    {
        var angle = i / (samples - 1) * definition.bendAngle;
        points = append(points, ptAtAngle(startPoint, sign, radius, midRadius, miterAngle, angle));
    }
    skFitSpline(sketch, label, {
            "points" : points
    });
}

function ptAtAngle(startPoint is Vector, sign is number, radius is ValueWithUnits, midRadius is ValueWithUnits, miterAngle is ValueWithUnits, angle is ValueWithUnits) returns Vector
{
    var y = (1 - cos(angle)) * radius;
    var x = sign * y * tan(miterAngle);
    y = angle / radian * midRadius;
    var pt = startPoint + vector(x, y);
    return pt;
}

function makeSimpleFlange(context is Context, id is Id, edge is Query, offsetEdge is Query, thickness is ValueWithUnits, sideFace is Query, definition is map) returns map
{
    var edgeLine = evEdgeTangentLine(context, {
            "edge" : edge,
            "parameter" : 0.5
    });

    var offsetEdgeLine = evEdgeTangentLine(context, {
            "edge" : offsetEdge,
            "parameter" : 0.5
    });

    var xAxis = offsetEdgeLine.origin - edgeLine.origin;
    xAxis = normalize(xAxis - edgeLine.direction * dot(edgeLine.direction, xAxis));

    var sidePlane = evPlane(context, {
            "face" : sideFace
    });
    var yAxis = sidePlane.normal;

    var zAxis = cross(xAxis, yAxis);
    var cSys = coordSystem(edgeLine.origin, xAxis, zAxis);

    makeBend(context, id, edge, cSys, thickness, definition);
    return makeWall(context, id, edge, sidePlane, thickness, definition);
}

function makeBend(context is Context, id is Id, edge is Query, cSys is CoordSystem, thickness is ValueWithUnits, definition is map)
{
    var sketchId = id + "bend_1_sketch";
    var sketch = newSketchOnPlane(context, sketchId, { "sketchPlane" : plane(cSys) });
    var center = vector(-definition.innerRadius, 0 * meter);
    var rInner = definition.innerRadius;
    var rOuter = definition.innerRadius + thickness;
    var startInner = center + rInner * vector(1, 0);
    var midInner = center + rInner * vector(cos(definition.bendAngle / 2), sin(definition.bendAngle / 2));
    var endInner = center + rInner * vector(cos(definition.bendAngle), sin(definition.bendAngle));
    skArc(sketch, "innerArc", {
            "start" : startInner,
            "mid" : midInner,
            "end" : endInner
    });
    var startOuter = center + rOuter * vector(1, 0);
    var midOuter = center + rOuter * vector(cos(definition.bendAngle / 2), sin(definition.bendAngle / 2));
    var endOuter = center + rOuter * vector(cos(definition.bendAngle), sin(definition.bendAngle));
    skArc(sketch, "outerArc", {
            "start" : startOuter,
            "mid" : midOuter,
            "end" : endOuter
    });
    skLineSegment(sketch, "start_cap", {
            "start" : startInner,
            "end" : startOuter
    });
    skLineSegment(sketch, "end_cap", {
            "start" : endInner,
            "end" : endOuter
    });

    skSolve(sketch);
    var length = evLength(context, {
            "entities" : edge
    });
    opExtrude(context, id + "bend_1", {
            "entities" : qSketchRegion(sketchId, true),
            "direction" : cSys.zAxis,
            "startBound" : BoundingType.BLIND,
            "startDepth" : length / 2,
            "endBound" : BoundingType.BLIND,
            "endDepth" : length / 2
    });
    opBoolean(context, id + "boolean_bend_1", {
        "eraseImprintedEdges" : false,
        "localizedInFaces" : true,
        "targets" : qEdgeAdjacent(edge, EntityType.FACE),
        "tools" : qCreatedBy(id + "bend_1", EntityType.FACE),
        "operationType" : BooleanOperationType.UNION
    });

    opDeleteBodies(context, id + "delete_flange_bend_1_sketches", {
        "entities" : qCreatedBy(sketchId, EntityType.BODY)
    });
}

function fromWorld3(cSys is CoordSystem, pt is Vector)
{
    var v = pt - cSys.origin;
    const yAxis = cross(cSys.zAxis, cSys.xAxis);
    return vector(dot(v, cSys.xAxis), dot(v, yAxis), dot(v, cSys.zAxis));
}

function toWorld3(cSys is CoordSystem, pt is Vector)
{
    const yAxis = cross(cSys.zAxis, cSys.xAxis);
    return cSys.origin + pt[0] * cSys.xAxis + pt[1] * yAxis + pt[2] * cSys.zAxis;
}

function findBisectingPlane(angle is ValueWithUnits, sidePlane is Plane, edgeLine is Line, length is ValueWithUnits, isLeft is boolean)
{
    var sign = isLeft ? 1 : -1;
    var xAxis = sin(angle/2) * sidePlane.normal - sign * cos(angle/2) * edgeLine.direction;
    var normal = sign * edgeLine.direction;
    normal = normalize(normal - xAxis * dot(xAxis, normal));
    var origin = edgeLine.origin - sign * length / 2 * edgeLine.direction + RIP_GAP * normal;
    var result = plane(origin, normal, xAxis);
    return result;
}

function findWallData(context is Context, edge is Query, sidePlane is Plane, innerBendFace is Query, innerTopEdge is Query, definition is map)
{
    var adjFaces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
    var topFace;
    if (innerBendFace == adjFaces[0])
        topFace = adjFaces[1];
    else
        topFace = adjFaces[0];
    var topPlane = evFaceTangentPlane(context, {
        "face" : topFace,
        "parameter" : vector(0.5, 0.5)
    });
    var edgeLine = evEdgeTangentLine(context, {
        "edge" : edge,
        "parameter" : 0.5
    });
    var sideNormal = sidePlane.normal;
    if (dot(cross(sideNormal, edgeLine.direction), topPlane.normal) < 0)
    {
        edgeLine.direction = -edgeLine.direction;
    }
    var topEdges = qSubtraction(qEdgeAdjacent(topFace, EntityType.EDGE), edge);
    var adjEdges = evaluateQuery(context, qIntersection([topEdges, qVertexAdjacent(edge, EntityType.EDGE)]));
    if (size(adjEdges) != 2)
        throw regenError("expected 2 edges");
    var pt0 = evEdgeTangentLine(context, {
        "edge" : adjEdges[0],
        "parameter" : 0.5
    }).origin;
    var pt1 = evEdgeTangentLine(context, {
        "edge" : adjEdges[1],
        "parameter" : 0.5
    }).origin;
    var v;
    v = pt0 - edgeLine.origin;
    if (dot(v, edgeLine.direction) > 0)
    {
        var temp = pt0;
        pt0 = pt1;
        pt1 = temp;
    }
    var length = evLength(context, {
            "entities" : edge
    });
    var endPt0 = edgeLine.origin - length / 2 * edgeLine.direction;
    var endPt1 = edgeLine.origin + length / 2 * edgeLine.direction;
    var v0 = normalize(pt0 - endPt0);
    var v1 = normalize(pt1 - endPt1);
    var angle0 = atan2(dot(cross(edgeLine.direction, v0), topPlane.normal), dot(edgeLine.direction, v0));
    var angle1 = PI * radian - atan2(dot(cross(edgeLine.direction, v1), topPlane.normal), dot(edgeLine.direction, v1));
    var plane0 = findBisectingPlane(angle0, sidePlane, edgeLine, length, true);
    var plane1 = findBisectingPlane(angle1, sidePlane, edgeLine, length, false);
    return {
        "angle0" : angle0,
        "angle1" : angle1,
        "edgeLine" : edgeLine,
        "topPlane" : topPlane,
        "plane0" : plane0,
        "plane1" : plane1
    };
}

function makeWall(context is Context, id is Id, edge is Query, sidePlane is Plane, thickness is ValueWithUnits, definition is map) returns map
{
    var innerBendFace = makeQuery(id + "bend_1", "SWEPT_FACE" ,EntityType.FACE, { "disambiguationData":[
        originalSetDisambiguation([sketchEntityQuery(id + "bend_1_sketch", EntityType.EDGE, "innerArc")])]});
    var innerTopEdge = makeQuery(id + "bend_1", "SWEPT_EDGE", EntityType.EDGE, {"disambiguationData":[
        originalSetDisambiguation([
            sketchEntityQuery(id + "bend_1_sketch", EntityType.EDGE, "innerArc"),
            sketchEntityQuery(id + "bend_1_sketch", EntityType.EDGE, "end_cap")])]});

    var wallData = findWallData(context, edge, sidePlane, innerBendFace, innerTopEdge, definition);

    var normal = evFaceNormalAtEdge(context, {
        "edge" : innerTopEdge,
        "face" : innerBendFace,
        "parameter" : 0.0
    });

    var edgeLine = wallData.edgeLine;
    var topPlane = wallData.topPlane;
    var angle0 = wallData.angle0;
    var angle1 = wallData.angle1;
    var bisectPlane0 = wallData.plane0;
    var bisectPlane1 = wallData.plane1;


    var length = evLength(context, {
            "entities" : edge
    });
    var edgeLine2 = evEdgeTangentLine(context, {
            "edge" : innerTopEdge,
            "parameter" : 0.5
    });
    var sketch = newSketchOnPlane(context, id + "wall_sketch", {
        "sketchPlane" : plane(edgeLine2.origin, normal, edgeLine.direction)
    });

    var cSysWall = coordSystem(edgeLine2.origin, edgeLine.direction, normal);

    const height = definition.height - (definition.innerRadius + thickness) * tan(definition.bendAngle / 2);
    var endPt0 = edgeLine2.origin - length / 2 * edgeLine2.direction;
    var endPt1 = edgeLine2.origin + length / 2 * edgeLine2.direction;
    var cSysTop0 = coordSystem(endPt0, edgeLine.direction, topPlane.normal);
    var cSysTop1 = coordSystem(endPt1, edgeLine.direction, topPlane.normal);

    var y = -cos(definition.bendAngle) * height;
    var z = sin(definition.bendAngle) * height;
    var x = -y / tan(angle1 / 2);
    var v0 = vector(x, y, z);
    var pt2 = toWorld3(cSysTop1, v0);

    y = -cos(definition.bendAngle) * height;
    z = sin(definition.bendAngle) * height;
    x = y / tan(angle0 / 2);
    var v1 = vector(x, y, z);
    var pt3 = toWorld3(cSysTop0, v1);
    if (definition.flushSides)
    {
        endPt0 = intersection(bisectPlane0, line(endPt0, edgeLine.direction)).intersection;
        endPt1 = intersection(bisectPlane1, line(endPt1, edgeLine.direction)).intersection;
        pt2 = intersection(bisectPlane1, line(pt2, edgeLine.direction)).intersection;
        pt3 = intersection(bisectPlane0, line(pt3, edgeLine.direction)).intersection;
    }
    endPt0 = fromWorld3(cSysWall, endPt0);
    endPt1 = fromWorld3(cSysWall, endPt1);
    pt2 = fromWorld3(cSysWall, pt2);
    pt3 = fromWorld3(cSysWall, pt3);

    skLineSegment(sketch, "bottom", {
            "start" : vector(endPt0[0], endPt0[1]),
            "end" : vector(endPt1[0], endPt1[1])
    });
    skLineSegment(sketch, "left", {
            "start" : vector(endPt1[0], endPt1[1]),
            "end" : vector(pt2[0], pt2[1])
    });
    skLineSegment(sketch, "top", {
            "start" : vector(pt2[0], pt2[1]),
            "end" : vector(pt3[0], pt3[1])
    });
    skLineSegment(sketch, "right", {
            "start" : vector(pt3[0], pt3[1]),
            "end" : vector(endPt0[0], endPt0[1])
    });
    skSolve(sketch);

    opExtrude(context, id + "extrude_wall", {
            "entities" : qSketchRegion(id + "wall_sketch", true),
            "direction" : -normal,
            "endBound" : BoundingType.BLIND,
            "endDepth" : thickness
    });

    opBoolean(context, id + "boolean_wall", {
        "eraseImprintedEdges" : false,
        "localizedInFaces" : true,
        "targets" : qEdgeAdjacent(innerTopEdge, EntityType.FACE),
        "tools" : qCreatedBy(id + "extrude_wall", EntityType.FACE),
        "operationType" : BooleanOperationType.UNION
    });

    opDeleteBodies(context, id + "delete_flange_wall_1_sketches", {
        "entities" : qCreatedBy(id + "wall_sketch", EntityType.BODY)
    });

    return {
        "innerEdgeQuery" : makeQuery(id + "extrude_wall", "CAP_EDGE", EntityType.EDGE, {
                "disambiguationData":[
                    originalSetDisambiguation([
                        sketchEntityQuery(id + "wall_sketch", EntityType.EDGE, "top")
                    ])
                ], "isStart" : true}),
        "outerEdgeQuery" : makeQuery(id + "extrude_wall", "CAP_EDGE", EntityType.EDGE, {
                "disambiguationData":[
                    originalSetDisambiguation([
                        sketchEntityQuery(id + "wall_sketch", EntityType.EDGE, "top")
                    ])
                ], "isStart" : false})
    };
}

function orientEdges(context is Context, edges is array, definition is map) returns array
{
    var result = [];
    for (var edge in edges)
    {
        var offsetData = findOffsetEdgeData(context, edge);
        if (definition.oppositeDirection)
        {
            result = append(result, offsetData.offsetEdge);
        }
        else
        {
            result = append(result, edge);
        }
    }
    return result;
}

function addAngleManipulator(context is Context, id is Id, edges is array, definition is map)
{
    if (size(edges) == 0)
        return;
    var i = -1;
    for (var edge in edges)
    {
        i += 1;
        if (i == 0 || MULTIPLE_MANIPULATORS)
        {
            var length = evLength(context, {
                    "entities" : edges[0]
            });
            const usedEntities = edge;
            var cSys = getEdgeCSys(context, edge);
            var yAxis = yAxis(cSys);

            addManipulators(context, id, { (ANGLE_MANIPULATOR ~ "_" ~ i) :
                angularManipulator({ "axisOrigin" : cSys.origin,
                    "axisDirection" : -cSys.xAxis,
                    "rotationOrigin" : cSys.origin + 0.5 * length * -yAxis,
                    "angle" : definition.bendAngle,
                    "sources" : usedEntities,
                    "minValue" : 0 * radian,
                    "maxValue" : PI * radian })});
        }
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

/**
* @internal
*/
export function flangeManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    for (var entry in newManipulators)
    {
        var matchResult;

        matchResult = match(entry.key, HEIGHT_MANIPULATOR ~ "_.*" );
        if (matchResult.hasMatch && newManipulators[entry.key] is map)
        {
            var newOffset = newManipulators[entry.key].offset;
            definition.oppositeDirection = newOffset < 0 * meter;
            definition.height = abs(newOffset);
        }
        matchResult = match(entry.key, ANGLE_MANIPULATOR ~ "_.*" );
        if (matchResult.hasMatch && newManipulators[entry.key] is map)
        {
            definition.bendAngle = newManipulators[entry.key].angle;
        }
    }

    return definition;
}

