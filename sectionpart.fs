FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/surfaceGeometry.fs", version : "");

// Imports used internally
import(path : "onshape/std/box.fs", version : "");
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/evaluate.fs", version : "");
import(path : "onshape/std/extrude.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/math.fs", version : "");
import(path : "onshape/std/sketch.fs", version : "");
import(path : "onshape/std/tool.fs", version : "");
import(path : "onshape/std/units.fs", version : "");
import(path : "onshape/std/vector.fs", version : "");

// Expand bounding box by 1% for purposes of creating cutting geometry
const BOX_TOLERANCE = 0.01;

//Given a plane definition and a input part query will return a list of bodies that one needs to delete so that
//the only bodies that remain are the ones split by the plane and behind it. Used by drawings to render a section view
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
        "size" : 1 * meter
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
 * Drawings don't have an upgrade process like part studios, so this cannot be changed or it will break
 * drawing queries.
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
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
 * Split a part down a jogged section line and delete all back bodies. Used by drawings. Needs to be a feature
 * so that drawings created by queries can resolve.
 * @param definition {{
 *      @field target {Query} : Body to be split.
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
        opDeleteBodies(context, id + "initialDelete", {"entities" : qSubtraction(qEverything(EntityType.BODY), definition.target)});

        try
        {
            const coordinateSystem = planeToCSys(definition.sketchPlane);
            var boxResult = evBox3d(context, { 'topology' : definition.target, 'cSys' : coordinateSystem });
            boxResult = extendBox3d(boxResult, 0 * meter, BOX_TOLERANCE);
            // Shift the plane and box to the box's min corner
            const offsetPlane = plane(toWorld(coordinateSystem, boxResult.minCorner), definition.sketchPlane.normal, definition.sketchPlane.x);
            boxResult.maxCorner = boxResult.maxCorner - boxResult.minCorner;
            boxResult.minCorner = vector(0, 0, 0) * meter;
            const numberOfPoints = size(definition.jogPoints);
            var projectedPoints = makeArray(numberOfPoints);
            for (var i = 0; i < numberOfPoints; i = i + 1)
            {
                projectedPoints[i] = worldToPlane(offsetPlane, definition.jogPoints[i]);
            }
            checkJogDirection(projectedPoints);
            const polygon = createJogPolygon(projectedPoints, boxResult, offsetPlane);
            const sketchId = id + "sketch";

            sketchPolyline(context, sketchId, polygon, offsetPlane);
            const extrudeId = id + "extrude";
            const sketchRegionQuery = qCreatedBy(sketchId, EntityType.FACE);
            extrudeCut(context, extrudeId, definition.target, sketchRegionQuery, boxResult.maxCorner[2]);
            opDeleteBodies(context, id + "deleteSketch", {"entities" : qCreatedBy(sketchId, EntityType.BODY)});
        }
        catch
        {
            opDeleteBodies(context, id + "delete", { "entities" : qEverything(EntityType.BODY) });
        }
    });

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
        if (deltaY < TOLERANCE.zeroLength * meter)
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

