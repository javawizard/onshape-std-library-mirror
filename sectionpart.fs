FeatureScript 422; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "422.0");
export import(path : "onshape/std/surfaceGeometry.fs", version : "422.0");

// Imports used internally
import(path : "onshape/std/box.fs", version : "422.0");
import(path : "onshape/std/containers.fs", version : "422.0");
import(path : "onshape/std/coordSystem.fs", version : "422.0");
import(path : "onshape/std/evaluate.fs", version : "422.0");
import(path : "onshape/std/extrude.fs", version : "422.0");
import(path : "onshape/std/feature.fs", version : "422.0");
import(path : "onshape/std/math.fs", version : "422.0");
import(path : "onshape/std/sketch.fs", version : "422.0");
import(path : "onshape/std/tool.fs", version : "422.0");
import(path : "onshape/std/units.fs", version : "422.0");
import(path : "onshape/std/vector.fs", version : "422.0");

// Expand bounding box by 1% for purposes of creating cutting geometry
const BOX_TOLERANCE = 0.01;

//Given a plane definition and a input part query will return a list of bodies that one needs to delete so that
//the only bodies that remain are the ones split by the plane, unless none are split by the plane, in which case
//the only bodies that remain are the ones behind the plane. Used by drawings to render a section view
function performSectionCutAndGetBodiesToDelete(context is Context, id is Id, plane is Plane, partToSection is Query) returns Query
{
    var allBodies = qBodyType(qEverything(EntityType.BODY), BodyType.SOLID);

    // The bbox of the body in plane coordinate system with positive z being in front of the plane
    const boxResult = evBox3d(context, { 'topology' : partToSection, 'cSys' : planeToCSys(plane) });

    // Body is fully behind the plane. Retain only the input body. no splitting needed
    if (boxResult.maxCorner[2] < TOLERANCE.zeroLength * meter)
    {
        return qSubtraction(allBodies, partToSection);
    }

    // Body is fully in front of plane. Delete all bodies no splitting needed
    if (boxResult.minCorner[2] > -TOLERANCE.zeroLength * meter)
    {
        return allBodies;
    }

    // Create construction plane for sectioning
    const cplaneDefinition =
    {
        "plane" : plane,
        "width" : 1 * meter,
        "height" : 1 * meter
    };

    const planeId = id + "plane";
    opPlane(context, planeId, cplaneDefinition);
    const planeTool = qOwnerBody(qCreatedBy(planeId));

    //The plane needs to be deleted so that it is not processed as a section face
    allBodies = qUnion([allBodies, planeTool]);

    // Split part on plane
    const splitPartDefinition =
    {
        "targets" : partToSection,
        "tool" : planeTool,
        "keepTools" : false
    };

    const splitPartId = id + "splitPart";
    opSplitPart(context, splitPartId, splitPartDefinition);

    // Split was success. Retain everything behind the plane
    return qSubtraction(allBodies, qSplitBy(splitPartId, EntityType.BODY, true));
}

//Section Part Feature

/**
 * Feature creating a section of a part behind a plane. Internally, performs
 * an `opSplitPart` followed by an `opDeleteBodies`.
 *
 * Not exposed in the Part Studio UI.
 */
// Drawings don't have an upgrade process like part studios, so this cannot be changed or it will break
// drawing queries.
export const sectionPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        definition.targets is Query;
        definition.plane is Plane;
    }
    {
        var bodiesToDelete = qEverything(EntityType.BODY); // Delete everything if there's an error
        try
        {
            bodiesToDelete = performSectionCutAndGetBodiesToDelete(context, id, definition.plane, definition.targets);
        }
        // TODO: how are errors reported?
        const deleteBodiesId = id + "deleteBody";
        opDeleteBodies(context, deleteBodiesId, { "entities" : bodiesToDelete });
    });


/**
 * Split a set of parts with a plane and delete all bodies in front of the face. Unlike sectionPart, bodies which are
 * entirely behind the split plane are retained. Any bodies not included in the target query are deleted.
 * @param definition {{
 *      @field target {Query} : Bodies to be split.
 *      @field plane {Plane} :  Plane that splits the bodies. Everything
 *              on the positive z side of the plane will be removed.
 * }}
 */
export const planeSectionPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        definition.target is Query;
        definition.plane is Plane;
    }
    {
        // define sketch plane orthogonal to the cut plane, with the cut plane's x axis as the sketch plane's normal
        // and the cut plane's reversed normal (jog & plane section have reversed sense of cutting plane direction)
        // as the x axis, which is an arbitrary choice.
        const sketchPlane = plane(definition.plane.origin, definition.plane.x, -definition.plane.normal);

        const coordinateSystem = planeToCSys(sketchPlane);

        // The bbox of the bodies in sketchPlane coordinate system with positive x being in front of the plane
        var boxResult = evBox3d(context, { 'topology' : definition.target, 'cSys' : coordinateSystem });

        // Boolean remove will complain if we don't hit anything
        if (boxResult.maxCorner[0] < -TOLERANCE.zeroLength * meter)
        {
            return;
        }

        // Extend the box slightly to make sure we get everything
        boxResult = extendBox3d(boxResult, 0 * meter, BOX_TOLERANCE);

        // Define the "jog section" points as a line extending across the bounding box y extents
        const cutPoints = [ toWorld(coordinateSystem, vector(0 * meter, boxResult.minCorner[1], boxResult.minCorner[2])),
                            toWorld(coordinateSystem, vector(0 * meter, boxResult.maxCorner[1], boxResult.minCorner[2])) ];

        jogSectionCut(context, id, definition.target, sketchPlane, cutPoints);
    });

/**
 * Split a part down a jogged section line and delete all back bodies. Used by drawings. Needs to be a feature
 * so that drawings created by queries can resolve. Any bodies not included in the target query are deleted.
 * @param definition {{
 *      @field target {Query} : Bodies to be split.
 *      @field sketchPlane {Plane} :  Plane that the jog line will be drawn in and extruded normal to. Everything
 *                                    on the positive x side of the jog line will be removed.
 *      @field jogPoints {array} : Points that the cutting line goes through in world coordinates.
 * }}
 */
export const jogSectionPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        definition.target is Query;
        definition.sketchPlane is Plane;
        definition.jogPoints is array;
        for (var point in definition.jogPoints)
             is3dLengthVector(point);
    }
    {
        jogSectionCut(context, id, definition.target, definition.sketchPlane, definition.jogPoints);
    });

function jogSectionCut(context is Context, id is Id, target is Query, sketchPlane is Plane, jogPoints is array)
{
    opDeleteBodies(context, id + "initialDelete", {"entities" : qSubtraction(qEverything(EntityType.BODY), target)});

    try
    {
        const coordinateSystem = planeToCSys(sketchPlane);
        var boxResult = evBox3d(context, { 'topology' : target, 'cSys' : coordinateSystem });
        boxResult = extendBox3d(boxResult, 0 * meter, BOX_TOLERANCE);
        // Shift the plane and box to the box's min corner
        const offsetPlane = plane(toWorld(coordinateSystem, boxResult.minCorner), sketchPlane.normal, sketchPlane.x);
        boxResult.maxCorner = boxResult.maxCorner - boxResult.minCorner;
        boxResult.minCorner = vector(0, 0, 0) * meter;
        const numberOfPoints = size(jogPoints);
        var projectedPoints = makeArray(numberOfPoints);
        for (var i = 0; i < numberOfPoints; i = i + 1)
        {
            projectedPoints[i] = worldToPlane(offsetPlane, jogPoints[i]);
        }
        checkJogDirection(projectedPoints);
        const polygon = createJogPolygon(projectedPoints, boxResult, offsetPlane);
        const sketchId = id + "sketch";

        sketchPolyline(context, sketchId, polygon, offsetPlane);
        const extrudeId = id + "extrude";
        const sketchRegionQuery = qCreatedBy(sketchId, EntityType.FACE);
        extrudeCut(context, extrudeId, target, sketchRegionQuery, boxResult.maxCorner[2]);
        opDeleteBodies(context, id + "deleteSketch", {"entities" : qCreatedBy(sketchId, EntityType.BODY)});
    }
    catch
    {
        opDeleteBodies(context, id + "delete", { "entities" : qEverything(EntityType.BODY) });
    }
}

function extrudeCut(context is Context, id is Id, target is Query, sketchRegionQuery is Query, depth is ValueWithUnits)
{
    extrude(context, id, {"bodyType" : ToolBodyType.SOLID,
                          "operationType" : NewBodyOperationType.REMOVE,
                          "entities" : sketchRegionQuery,
                          "endBound" : BoundingType.BLIND,
                          "depth" : depth,
                          "defaultScope" : false,
                          "booleanScope" : target});
}

function checkJogDirection(pointsInPlane is array)
{
    var increasingYCount = 0;
    var decreasingYCount = 0;
    var length = size(pointsInPlane);
    for (var i = 0; i < length - 1; i = i + 1)
    {
        const deltaY = pointsInPlane[i + 1][1] - pointsInPlane[i][1];
        if (deltaY > TOLERANCE.zeroLength * meter)
        {
            increasingYCount = increasingYCount + 1;
        }
        if (deltaY < -TOLERANCE.zeroLength * meter)
        {
            decreasingYCount = decreasingYCount + 1;
        }
    }
    if (increasingYCount == 0 && decreasingYCount == 0)
    {
        throw regenError(ErrorStringEnum.SELF_INTERSECTING_CURVE_SELECTED);
    }
    if (increasingYCount > 0 && decreasingYCount > 0)
    {
        throw regenError(ErrorStringEnum.SELF_INTERSECTING_CURVE_SELECTED);
    }
}

function createJogPolygon(points is array, boundingBox is Box3d, sketchPlane is Plane) returns array
{
    var polygonVertices = concatenateArrays([makeArray(1), points, makeArray(2)]);

    const pointCount = size(points);
    const boxRadius = norm(boundingBox.maxCorner) / 2;
    const boxCenterInPlane = vector(boundingBox.maxCorner[0] / 2, boundingBox.maxCorner[1] / 2);
    const alignedDistanceToJogStart = abs(boxCenterInPlane[0] - points[0][0]);
    const alignedDistanceToJogEnd = abs(boxCenterInPlane[0] - points[pointCount - 1][0]);
    polygonVertices[0] = vector(0 * meter, points[0][1]);
    polygonVertices[pointCount + 1] = vector(0 * meter, points[pointCount - 1][1]);
    polygonVertices[pointCount + 2] = polygonVertices[0];

    return polygonVertices;
}

function sketchPolyline(context is Context, sketchId is Id, points is array, sketchPlane is Plane)
{
    const numberOfPoints = size(points);
    const sketch = newSketchOnPlane(context, sketchId, { "sketchPlane" : sketchPlane });

    for (var i = 0; i < numberOfPoints - 1; i = i + 1)
    {
        skLineSegment(sketch, "line_" ~ i, { "start" : points[i], "end" : points[i + 1] });
    }
    skSolve(sketch);
}

