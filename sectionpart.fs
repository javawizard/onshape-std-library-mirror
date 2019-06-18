FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "✨");
export import(path : "onshape/std/surfaceGeometry.fs", version : "✨");

// Imports used internally
import(path : "onshape/std/box.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/coordSystem.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/extrude.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");
import(path : "onshape/std/sketch.fs", version : "✨");
import(path : "onshape/std/tool.fs", version : "✨");
import(path : "onshape/std/units.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");

// Expand bounding box by 1% for purposes of creating cutting geometry
const BOX_TOLERANCE = 0.01;
// Expand bounding box absolutely a small amount to account for bounding boxes with zero length in a dimension
const BOX_ABSOLUTE_TOLERANCE = 1e-5 * meter;

// ATTENTION DEVELOPERS:
// If you version a fix to functionality used in section view
// Bump SBTAppElementViewVersionNumber and change BTPartStudioRenderingAgent.getSectionVersion()
// to return new FS version For new views

//Given a plane definition and a input part query will return a list of bodies that one needs to delete so that
//the only bodies that remain are the ones split by the plane, unless none are split by the plane, in which case
//the only bodies that remain are the ones behind the plane. Used by drawings to render a section view
function performSectionCutAndGetBodiesToDelete(context is Context, id is Id, plane is Plane, partToSection is Query) returns Query
{
    var allBodies = qBodyType(qEverything(EntityType.BODY), BodyType.SOLID);

    const useTightBox = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V932_SPLIT_PART_BOX);
    // The bbox of the body in plane coordinate system with positive z being in front of the plane
    const boxResult = evBox3d(context, { 'topology' : partToSection,
                                         'cSys' : planeToCSys(plane),
                                         'tight' : useTightBox });

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
    const splitPartResultQ = qSplitBy(splitPartId, EntityType.BODY, true);
    //Split plane might miss the body because the box was not sufficiently accurate
    //check body location by midBox z coordinate
    if (!useTightBox && evaluateQuery(context, splitPartResultQ) == [])
    {
        var midBox = 0.5 * (boxResult.minCorner[2] + boxResult.maxCorner[0]);
        if (midBox < TOLERANCE.zeroLength * meter)
        {
            return qSubtraction(allBodies, partToSection);
        }
        else
        {
            return allBodies;
        }
    }

    // Split was success. Retain everything behind the plane
    return qSubtraction(allBodies, splitPartResultQ);
}

//Section Part Feature

/**
 * Feature creating a section of a part behind a plane. Internally, performs
 * an [opSplitPart] followed by an [opDeleteBodies].
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
        const useTightBox = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V932_SPLIT_PART_BOX);
        var boxResult = evBox3d(context, { 'topology' : definition.target,
                                           'cSys' : coordinateSystem,
                                           'tight' : useTightBox });

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
        definition.jogPoints = convertToPointsArray(false, cutPoints, []);
        definition.sketchPlane = sketchPlane;
        jogSectionCut(context, id, definition);
    }, {"isPartialSection" : false, "keepSketches" : false, "isBrokenOut" : false, "isCropView" : false});

/**
 * Split a part down a jogged section line and delete all back bodies. Used by drawings. Needs to be a feature
 * so that drawings created by queries can resolve. Any bodies not included in the target query are deleted.
 * @param definition {{
 *      @field target {Query} : Bodies to be split.
 *      @field sketchPlane {Plane} :  Plane that the jog line will be drawn in and extruded normal to. Everything
 *                                    on the positive x side of the jog line will be removed.
 *      @field jogPoints {array} : Points that the cutting line goes through in world coordinates.
 *      @field isPartialSection {boolean} : Whether or not it is a partial section cut.
 *      @field keepSketches {boolean} : Whether or not sketches will be kept in the section cut results.
 *      @field isBrokenOut {boolean} : Whether or not it is a broken-out section cut.
 *      @field isCropView {boolean} : Whether or not it is a crop section cut.
 *      @field brokenOutPointNumbers {array} : Array of the number of spline points of each broken-out section cut.
 *      @field brokenOutEndConditions {array} : Array of end conditions of each broken-out section cut.
 *      @field offsetPoints {array} : Array of points for offsetting the section lines.
* }}
 */
export const jogSectionPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        definition.target is Query;
        definition.sketchPlane is Plane;
        definition.jogPoints is array;
        for (var point in definition.jogPoints)
        {
            if (point != undefined)
            {
                is3dLengthVector(point);
            }
        }
        if (definition.bbox != undefined)
        {
            definition.bbox is Box3d;
        }
        definition.isPartialSection is boolean;
        definition.keepSketches is boolean;
        definition.isBrokenOut is boolean;
        definition.isCropView is boolean;
        definition.brokenOutPointNumbers is array;
        definition.brokenOutEndConditions is array;
        definition.offsetPoints is array;
    }
    {
        const numberOfPoints = definition.jogPoints != undefined ? size(definition.jogPoints) : 0;
        const brokenOutPointNumbers = definition.brokenOutPointNumbers != undefined ? definition.brokenOutPointNumbers : [];
        definition.jogPoints = convertToPointsArray(definition.isBrokenOut || definition.isCropView, definition.jogPoints, brokenOutPointNumbers);
        definition.offsetDistances = definition.brokenOutEndConditions != undefined ? getOffsetDistancesArray(definition.brokenOutEndConditions) : [];
        jogSectionCut(context, id, definition);
    }, {isPartialSection : false, keepSketches : false, isBrokenOut : false, isCropView : false, brokenOutPointNumbers : [], brokenOutEndConditions : [], offsetPoints : [] });

/**
 * @internal
 * Calling this method will clear all intermediate sheet metal data, limited to internal use only
 */
export const jogSectionPartInternal = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        definition.target is Query;
        definition.sketchPlane is Plane;
        definition.jogPoints is array;
        for (var point in definition.jogPoints)
        {
            if (point != undefined)
            {
                is3dLengthVector(point);
            }
        }
        if (definition.bbox != undefined)
        {
            definition.bbox is Box3d;
        }
        definition.isPartialSection is boolean;
        definition.keepSketches is boolean;
        definition.isBrokenOut is boolean;
        definition.isCropView is boolean;
        definition.brokenOutPointNumbers is array;
        definition.brokenOutEndConditions is array;
        definition.offsetPoints is array;
    }
    {
        // remove sheet metal attributes and helper bodies
        clearSheetMetalData(context, id + "sheetMetal", undefined);
        const brokenOutPointNumbers = definition.brokenOutPointNumbers != undefined ? definition.brokenOutPointNumbers : [];
        definition.jogPoints = convertToPointsArray(definition.isBrokenOut || definition.isCropView, definition.jogPoints, brokenOutPointNumbers);
        definition.offsetDistances  = definition.brokenOutEndConditions != undefined ? getOffsetDistancesArray(definition.brokenOutEndConditions) : [];
        jogSectionCut(context, id, definition);
    }, {isPartialSection : false, keepSketches : false, isBrokenOut : false, isCropView : false, brokenOutPointNumbers : [], brokenOutEndConditions : [], offsetPoints : [] });

/**
 * Collect the spline points and the depth point from each broken-out section and convert it into an array of array
 * for broken-out section, we store the spline points and depth points for multiple broken-out sections in 'jogPoints'.
 * Format of 'jogPoints': [pointsForBrokenOut1, depthPoint1, pointsForBrokenOut2, depthPoint2, ...]
 * 'brokenOutPointNumbers' tells us how many points each broken-out section has: [size(pointsForBrokenOut1), size(pointsForBrokenOut2), ...]
 */
function convertToPointsArray(isBrokenOutOrCropView is boolean, jogPoints is array, brokenOutPointNumbers is array)
{
    var jogPointsArray;
    if (isBrokenOutOrCropView && brokenOutPointNumbers != undefined && size(brokenOutPointNumbers) > 0)
    {
        var jogPointIndex = 0;
        var numberOfBrokenOut = size(brokenOutPointNumbers);
        jogPointsArray = makeArray(numberOfBrokenOut);
        for (var i = 0; i < numberOfBrokenOut; i = i + 1)
        {
            const numberOfJogPoints = brokenOutPointNumbers[i];
            var points = makeArray(numberOfJogPoints + 1); // need to add an additional one for the depth point
            for (var j = 0; j < numberOfJogPoints + 1; j = j + 1)
            {
                points[j] = jogPoints[jogPointIndex];
                jogPointIndex = jogPointIndex + 1;
            }
            jogPointsArray[i] = points;
        }
    }
    else
    {
        jogPointsArray = makeArray(1);
        jogPointsArray[0] = jogPoints;
    }
    return jogPointsArray;
}

function getOffsetDistancesArray(endConditions is array)
{
    var offsetDistancesArray = [];
    if (endConditions != undefined && size(endConditions) > 0)
    {
        var numberOfOffset = size(endConditions);
        offsetDistancesArray = makeArray(numberOfOffset);
        for (var i = 0; i < numberOfOffset; i = i + 1)
        {
            var offset = undefined;
            if (endConditions[i] != undefined && endConditions[i].hasOffset && endConditions[i].offsetDistance != undefined)
            {
                offset = endConditions[i].offsetDistance;
                if (endConditions[i].offsetOppositeDirection)
                {
                    offset *= -1;
                }
            }
            offsetDistancesArray[i] = offset;
        }
    }
    return offsetDistancesArray;
}

function jogSectionCut(context is Context, id is Id, definition is map)
{
    const target = definition.target;
    const sketchPlane = definition.sketchPlane;
    const bboxIn = definition.bbox;
    const isPartialSection = definition.isPartialSection;
    const jogPointsArray = definition.jogPoints;
    const offsetDistancesArray = definition.offsetDistances;
    const keepSketches = definition.keepSketches;
    const isBrokenOut = definition.isBrokenOut;
    const isCropView = definition.isCropView;
    const offsetPoints = definition.offsetPoints;

    var toDeleteQ = qSubtraction(qEverything(EntityType.BODY), target);
    if (keepSketches)
    {
       toDeleteQ = qSketchFilter(toDeleteQ, SketchObject.NO);
    }
    opDeleteBodies(context, id + "initialDelete", { "entities" : toDeleteQ });

    try
    {
        var bboxInSketchCS = undefined;
        if (bboxIn != undefined)
        {
            // convert the bbox from world coordinate system to the sketch plane
            bboxInSketchCS = transformBox3d(bboxIn, fromWorld(planeToCSys(sketchPlane)));
        }
        if (isBrokenOut || isCropView)
        {
            brokenOutSectionCut(context, id, target, sketchPlane, bboxInSketchCS, jogPointsArray, offsetDistancesArray, isCropView);
        }
        else if (jogPointsArray != undefined && size(jogPointsArray) == 1)
        {
            const jogPoints = jogPointsArray[0];
            const coordinateSystem = planeToCSys(sketchPlane);
            const useTightBox = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V932_SPLIT_PART_BOX);
            var boxResult = evBox3d(context, { 'topology' : target,
                                               'cSys' : coordinateSystem,
                                               'tight' : useTightBox });
            boxResult = extendBox3d(boxResult, 0 * meter, BOX_TOLERANCE);
            // Shift the plane and box to the box's min corner
            var origin = toWorld(coordinateSystem, boxResult.minCorner);
            const offsetPlane = plane(origin, sketchPlane.normal, sketchPlane.x);
            boxResult.maxCorner = boxResult.maxCorner - boxResult.minCorner;
            boxResult.minCorner = vector(0, 0, 0) * meter;
            const numberOfPoints = size(jogPoints);
            var projectedPoints = makeArray(numberOfPoints);
            for (var i = 0; i < numberOfPoints; i = i + 1)
            {
                projectedPoints[i] = worldToPlane(offsetPlane, jogPoints[i]);
            }
            checkJogDirection(projectedPoints);
            // check if this is a section cut with offset
            var isOffsetCut = false;
            var offsetDistance = 0.0 * meter;
            const hasOffsetDistance = offsetDistancesArray != undefined && size(offsetDistancesArray) == 1 && offsetDistancesArray[0] != undefined;
            const offsetPoint = offsetPoints != undefined && size(offsetPoints) > 0 ? offsetPoints[0] : undefined;
            if (numberOfPoints == 2 && (hasOffsetDistance || offsetPoint != undefined))
            {
                isOffsetCut = true;
                if (hasOffsetDistance)
                {
                    offsetDistance = offsetDistancesArray[0];
                }
                // adjust the offset distance if the offset point is specified
                if (offsetPoint != undefined)
                {
                    const projectedOffsetPoint = worldToPlane(offsetPlane, offsetPoint);
                    offsetDistance += (projectedOffsetPoint[0] - projectedPoints[0][0]);
                }
                // we do not allow negative offset. If it happens, we treat it as a regular section cut
                if (offsetDistance < 0)
                {
                    isOffsetCut = false;
                }
            }

            var polygon;
            if (isOffsetCut)
            {
                polygon = createJogPolygonForOffsetCut(projectedPoints, boxResult, offsetPlane, offsetDistance);
            }
            else if (isPartialSection)
            {
                polygon = createJogPolygonForPartialSection(projectedPoints, boxResult, offsetPlane);
            }
            else
            {
                polygon = createJogPolygon(projectedPoints, boxResult, offsetPlane);
            }
            const sketchId = id + "sketch";

            sketchPolyline(context, sketchId, polygon, offsetPlane);
            const extrudeId = id + "extrude";
            const sketchRegionQuery = qCreatedBy(sketchId, EntityType.FACE);
            extrudeCut(context, extrudeId, target, sketchRegionQuery, boxResult.maxCorner[2], isOffsetCut);
            opDeleteBodies(context, id + "deleteSketch", { "entities" : qCreatedBy(sketchId, EntityType.BODY) });
        }
    }
    catch
    {
        opDeleteBodies(context, id + "delete", { "entities" : qEverything(EntityType.BODY) });
    }
}

/**
 * 'jogPointsArray' is an array of array, each array contains the spline section points and the depth point
 */
function brokenOutSectionCut(context is Context, id is Id, target is Query, sketchPlane is Plane, bboxIn, jogPointsArray is array, offsetDistancesArray is array, isCropView is boolean)
{
    const coordinateSystem = planeToCSys(sketchPlane);
    const useTightBox = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V932_SPLIT_PART_BOX);
    var boxResult = bboxIn != undefined ? bboxIn : evBox3d(context, { 'topology' : target,
                                       'cSys' : coordinateSystem,
                                       'tight' : useTightBox });
    if (bboxIn == undefined)
    {
        boxResult = extendBox3d(boxResult, BOX_ABSOLUTE_TOLERANCE, BOX_TOLERANCE);
    }
    // Shift the plane and box to the box's min corner
    var defaultOrigin = toWorld(coordinateSystem, boxResult.minCorner);
    var numberOfBrokenOut = size(jogPointsArray);
    for (var brokenOutIndex = 0; brokenOutIndex < numberOfBrokenOut; brokenOutIndex = brokenOutIndex + 1)
    {
        const jogPoints = jogPointsArray[brokenOutIndex];
        var numberOfJogPoints = size(jogPoints) - 1;
        var jogPointsForBrokenOut = makeArray(numberOfJogPoints);
        for (var i = 0; i < numberOfJogPoints; i = i + 1)
        {
            jogPointsForBrokenOut[i] = jogPoints[i];
        }
        var uptoPoint = jogPoints[numberOfJogPoints]; //the last point in the array is the depth point

        // Shift the plane and box to 'uptoPoint' if it is a broken-out section view and uptoPoint is specified
        var offset = 0 * meter;
        if (isCropView) // for crop view, there is no depth, always from minCorner to maxCorner
        {
            uptoPoint = toWorld(coordinateSystem, boxResult.minCorner);
        }

        if (uptoPoint == undefined)
        {
            uptoPoint = toWorld(coordinateSystem, boxResult.maxCorner);
        }
        if (uptoPoint != undefined)
        {
            const dir = uptoPoint - defaultOrigin;
            offset = dot(dir, sketchPlane.normal);
        }
        if (offsetDistancesArray != undefined && size(offsetDistancesArray) > brokenOutIndex && offsetDistancesArray[brokenOutIndex] != undefined && !isCropView)
        {
            offset += offsetDistancesArray[brokenOutIndex];
        }
        var origin = defaultOrigin + offset * sketchPlane.normal;
        const offsetPlane = plane(origin, sketchPlane.normal, sketchPlane.x);

        const isClosedSpline = tolerantEquals(jogPointsForBrokenOut[0], jogPointsForBrokenOut[numberOfJogPoints - 1]);
        // if it is not closed, we add one to close it
        if (!isClosedSpline)
        {
            jogPointsForBrokenOut = concatenateArrays([jogPointsForBrokenOut, makeArray(1)]);
            jogPointsForBrokenOut[numberOfJogPoints] = jogPointsForBrokenOut[0];
            numberOfJogPoints = numberOfJogPoints + 1;
        }
        var projectedPoints = makeArray(numberOfJogPoints);
        for (var i = 0; i < numberOfJogPoints; i = i + 1)
        {
            projectedPoints[i] = worldToPlane(offsetPlane, jogPointsForBrokenOut[i]);
        }

        const sketchId = id + ("sketch" ~ brokenOutIndex);
        sketchSplineSection(context, sketchId, projectedPoints, offsetPlane);

        const extrudeId = id + ("extrude" ~ brokenOutIndex);
        const sketchRegionQuery = qCreatedBy(sketchId, EntityType.FACE);
        extrudeCut(context, extrudeId, target, sketchRegionQuery, undefined, isCropView);
        opDeleteBodies(context, id + ("deleteSketch" ~ brokenOutIndex), { "entities" : qCreatedBy(sketchId, EntityType.BODY) });
    }
}

function extrudeCut(context is Context, id is Id, target is Query, sketchRegionQuery is Query, depth, isIntersect is boolean)
{
    var noMerge = isAtVersionOrLater(context, FeatureScriptVersionNumber.V620_DONT_MERGE_SECTION_FACE);
    if (depth != undefined && depth < 2 * TOLERANCE.booleanDefaultTolerance * meter)
    {
        depth = (2 * TOLERANCE.booleanDefaultTolerance) * meter;
    }
    const extrudeDefinition = {"bodyType" : ToolBodyType.SOLID,
            "operationType" : isIntersect ? NewBodyOperationType.INTERSECT : NewBodyOperationType.REMOVE,
            "entities" : sketchRegionQuery,
            "endBound" : depth == undefined ? BoundingType.THROUGH_ALL : BoundingType.BLIND,
            "depth" : depth,
            "defaultScope" : false,
            "eraseImprintedEdges" : noMerge ? false : true,
            "allowSheets" : true,
            "booleanScope" : target};

    extrude(context, id, extrudeDefinition);
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

function createJogPolygonForOffsetCut(points is array, boundingBox is Box3d, sketchPlane is Plane, offsetDistance is ValueWithUnits) returns array
{
    var polygonVertices = concatenateArrays([makeArray(1), points, makeArray(2)]);

    const pointCount = size(points);
    polygonVertices[0] = vector(points[pointCount - 1][0] + offsetDistance, points[0][1]);
    polygonVertices[pointCount + 1] = vector(points[pointCount - 1][0] + offsetDistance, points[pointCount - 1][1]);
    polygonVertices[pointCount + 2] = polygonVertices[0];

    return polygonVertices;
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

function createJogPolygonForPartialSection(points is array, boundingBox is Box3d, sketchPlane is Plane) returns array
{
    var polygonVertices = concatenateArrays([points, makeArray(7)]);

    const pointCount = size(points);
    const boxRadius = norm(boundingBox.maxCorner) / 2;
    const boxCenterInPlane = vector(boundingBox.maxCorner[0] / 2, boundingBox.maxCorner[1] / 2);
    const alignedDistanceToJogStart = abs(boxCenterInPlane[0] - points[0][0]);
    const alignedDistanceToJogEnd = abs(boxCenterInPlane[0] - points[pointCount - 1][0]);
    const flipY = points[pointCount - 1][1] < points[0][1];

    polygonVertices[pointCount] = vector(boundingBox.maxCorner[0], points[pointCount - 1][1]);
    polygonVertices[pointCount + 1] = vector(polygonVertices[pointCount][0], flipY ? boundingBox.minCorner[1] : boundingBox.maxCorner[1]);
    polygonVertices[pointCount + 2] = vector(boundingBox.minCorner[0], polygonVertices[pointCount + 1][1]);
    polygonVertices[pointCount + 3] = vector(polygonVertices[pointCount + 2][0], flipY ? boundingBox.maxCorner[1] : boundingBox.minCorner[1]);
    polygonVertices[pointCount + 4] = vector(boundingBox.maxCorner[0], polygonVertices[pointCount + 3][1]);
    polygonVertices[pointCount + 5] = vector(polygonVertices[pointCount + 4][0], points[0][1]);
    polygonVertices[pointCount + 6] = polygonVertices[0];

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

function sketchSplineSection(context is Context, sketchId is Id, points is array, sketchPlane is Plane)
{
    const sketch = newSketchOnPlane(context, sketchId, { "sketchPlane" : sketchPlane });
    skFitSpline(sketch, "spline", {
                "points" : points
            });
    skSolve(sketch);
}

