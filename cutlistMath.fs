FeatureScript 2679; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/box.fs", version : "2679.0");
import(path : "onshape/std/containers.fs", version : "2679.0");
import(path : "onshape/std/context.fs", version : "2679.0");
import(path : "onshape/std/coordSystem.fs", version : "2679.0");
import(path : "onshape/std/error.fs", version : "2679.0");
import(path : "onshape/std/evaluate.fs", version : "2679.0");
import(path : "onshape/std/frameUtils.fs", version : "2679.0");
import(path : "onshape/std/geomOperations.fs", version : "2679.0");
import(path : "onshape/std/math.fs", version : "2679.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2679.0");
import(path : "onshape/std/units.fs", version : "2679.0");
import(path : "onshape/std/vector.fs", version : "2679.0");

const NUM_ISOPARAM_CURVES = 7;

/**
 * @internal
 * Calculate the length and angles of each frame selected for the cut list
 */
export function getCutlistLengthAndAngles(context is Context, topLevelId is Id, createdCurveId is Id, beam is Query,
    bodiesToDelete is box) returns map
{
    const sweptEdgeQuery = qFrameSweptEdge(beam);
    const sweptFaceQuery = qFrameSweptFace(beam);
    const startFaceQuery = qFrameStartFace(beam);
    const endFaceQuery = qFrameEndFace(beam);

    // early exit for spline path bodies
    // versioned improvement: possible to create spline surfaces from flat profile so use edge information if available
    // if unavailable, use faces
    const splinePathQuery = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1772_STRAIGHT_SEGMENT_IDENTIFICATION_FIX) && !isQueryEmpty(context, sweptEdgeQuery)
    ? qGeometry(sweptEdgeQuery, GeometryType.OTHER_CURVE)
    : qGeometry(sweptFaceQuery, GeometryType.OTHER_SURFACE);
    if (!isQueryEmpty(context, splinePathQuery))
    {
        const approximateLength = getApproximateLengthForSplines(context, topLevelId, createdCurveId, sweptEdgeQuery,
            sweptFaceQuery, startFaceQuery, endFaceQuery, bodiesToDelete);
        return buildBeamCutlistData(approximateLength, undefined, undefined);
    }

    // can't get anywhere without end attribution
    if (isQueryEmpty(context, startFaceQuery) || isQueryEmpty(context, endFaceQuery))
    {
        reportFeatureWarning(context, topLevelId, ErrorStringEnum.FRAME_MISSING_CAP_FACES);
        return buildBeamCutlistData(undefined, undefined, undefined);
    }

    const result = isQueryEmpty(context, sweptEdgeQuery) ?
        getLengthAndAnglesWithoutEdges(context, topLevelId, createdCurveId, beam, startFaceQuery, endFaceQuery,
            sweptFaceQuery, bodiesToDelete) :
        getLengthAndAnglesWithEdges(context, topLevelId, beam, startFaceQuery, endFaceQuery, sweptEdgeQuery);

    return result;
}

function buildBeamCutlistData(length, startAngle, endAngle) returns map
{
    return {
            "length" : length,
            "angle1" : startAngle,
            "angle2" : endAngle
        };
}

function getApproximateLengthForSplines(context is Context, topLevelId is Id, createdCurveId is Id, sweptEdgeQuery is Query,
    sweptFaceQuery is Query, startFaceQuery is Query, endFaceQuery is Query, bodiesToDelete is box)
{
    // first look for any real swept edges that touch a start and end face
    const maxRealEdge = getMaxLengthEdge(context, sweptEdgeQuery);
    var startFaceAdjacentPair;
    var endFaceAdjacentPair;

    try silent
    {
        startFaceAdjacentPair = getFaceEdgeAdjacentPair(context, startFaceQuery, maxRealEdge.edge);
        endFaceAdjacentPair = getFaceEdgeAdjacentPair(context, endFaceQuery, maxRealEdge.edge);
    }

    if (maxRealEdge != undefined && startFaceAdjacentPair != undefined && endFaceAdjacentPair != undefined)
    {
        return maxRealEdge.length;
    }

    // if that fails attempt to create edges
    const constructedSweptEdgeQuery = createIsoParamCurves(context, createdCurveId + "curves", sweptFaceQuery,
        bodiesToDelete);
    const maxProxyEdge = getMaxLengthEdge(context, constructedSweptEdgeQuery);
    if (maxProxyEdge != undefined)
    {
        return maxProxyEdge.length;
    }
    return undefined;
}

function getMaxLengthEdge(context is Context, edgeQuery)
{
    if (edgeQuery == undefined)
    {
        return undefined;
    }
    const edges = evaluateQuery(context, edgeQuery);
    const edgeData = mapArray(edges, edge =>
        {
            "edge" : edge,
            "length" : evLength(context, { "entities" : edge })
        });
    const maxEdge = max(edgeData, (a, b) => (a.length < b.length));
    return maxEdge;
}

function getMaxRadiusEdge(context is Context, sweptEdgeQuery is Query)
{
    var edgeDefinitions = [];
    const edges = evaluateQuery(context, sweptEdgeQuery);
    for (var edge in edges)
    {
        try silent
        {
            const def = evCurveDefinition(context, { "edge" : edge, "returnBSplinesAsOther" : true });
            if (def.radius != undefined)
            {
                edgeDefinitions = append(edgeDefinitions, def);
            }
        }
    }

    return max(edgeDefinitions, (a, b) => (a.radius < b.radius));
}

function getStraightCylinderDirection(context is Context, sweptFaceQuery is Query)
{
    const numSweptFaces = size(evaluateQuery(context, sweptFaceQuery));
    const cylinderFaces = evaluateQuery(context, qGeometry(sweptFaceQuery, GeometryType.CYLINDER));
    const numCylinderFaces = size(cylinderFaces);
    if (numCylinderFaces == 0 || numCylinderFaces != numSweptFaces)
    {
        return undefined;
    }
    // handle tube and extrusions with multiple lumens
    const firstCylinder = evSurfaceDefinition(context, { "face" : cylinderFaces[0] });
    const cylinderDirectionWCS = firstCylinder.coordSystem.zAxis;

    for (var faceIndex = 1; faceIndex < size(cylinderFaces) - 1; faceIndex += 1)
    {
        const otherCylinder = evSurfaceDefinition(context, { "face" : cylinderFaces[faceIndex] });

        if (!parallelVectors(cylinderDirectionWCS, otherCylinder.coordSystem.zAxis))
        {
            return undefined; // not a straight cylinder
        }
    }
    return cylinderDirectionWCS;
}

// There are two possible cases for finding no swept edges:
// 1. the beam never had swept edges (pipe or tube). This case is acceptable.
// 2. geometric operations have removed them. This case is an error.
function getLengthAndAnglesWithoutEdges(context is Context, topLevelId is Id, createdCurveId is Id, beam is Query,
    startFaceQuery is Query, endFaceQuery is Query, sweptFaceQuery is Query, bodiesToDelete is box) returns map
{
    // handle straight cylinder pipe or tube
    const cylinderDirectionWCS = getStraightCylinderDirection(context, sweptFaceQuery);
    if (cylinderDirectionWCS != undefined)
    {
        return getDimensionsForStraightBeam(context, beam, startFaceQuery, endFaceQuery, cylinderDirectionWCS);
    }

    // handle circular swept pipe or tube
    const constructedSweptEdgeQuery = createIsoParamCurves(context, createdCurveId + "curves", sweptFaceQuery, bodiesToDelete);
    var result = {};
    if (constructedSweptEdgeQuery != undefined)
    {
        // use the constructed edges as proxy
        result = getCircularArcBeamDimensions(context, topLevelId, beam, startFaceQuery, endFaceQuery, constructedSweptEdgeQuery);
    }
    else
    {
        // report failure case
        reportFeatureWarning(context, topLevelId, ErrorStringEnum.FRAME_MISSING_SWEPT_EDGES);
        result = buildBeamCutlistData(undefined, undefined, undefined);
    }
    return result;
}

function getLengthAndAnglesWithEdges(context is Context, topLevelId is Id, beam is Query, startFaceQuery is Query,
    endFaceQuery is Query, sweptEdgeQuery is Query) returns map
{
    var result = {};
    const straightSweptEdgeQuery = qGeometry(sweptEdgeQuery, GeometryType.LINE);
    const straightSweptEdges = evaluateQuery(context, straightSweptEdgeQuery);
    if (straightSweptEdges != [])
    {
        const lineData = evEdgeTangentLine(context, {
                    "edge" : straightSweptEdges[0],
                    "parameter" : 0
                });
        result = getDimensionsForStraightBeam(context, beam, startFaceQuery, endFaceQuery, lineData.direction);
    }
    else
    {
        result = getCircularArcBeamDimensions(context, topLevelId, beam, startFaceQuery, endFaceQuery, sweptEdgeQuery);
    }
    return result;
}

// assumes beam is straight so uses the bounding box to determine total length
// use cap faces to compute angles.
function getDimensionsForStraightBeam(context is Context, beam is Query, startFaceQuery is Query,
    endFaceQuery is Query, direction is Vector) returns map
{
    const bbox = evBox3d(context, { "topology" : beam, "cSys" : coordSystem(plane(WORLD_ORIGIN, direction)), "tight" : true });
    const length = (bbox.maxCorner[2] - bbox.minCorner[2]);
    const startFacePlane = getPlaneFromCapFace(context, startFaceQuery);
    const endFacePlane = getPlaneFromCapFace(context, endFaceQuery);
    const startAngle = (startFacePlane != undefined) ? getAngle(startFacePlane.normal, direction) : undefined;
    const endAngle = (endFacePlane != undefined) ? getAngle(endFacePlane.normal, direction) : undefined;
    return buildBeamCutlistData(length, startAngle, endAngle);
}

// our heuristic is to use the LARGEST RADIUS edge to compute our lengths.
// use attribute information to determine where the swept body lies in space.
// use cap faces to compute angles.
function getCircularArcBeamDimensions(context is Context, topLevelId is Id, beam is Query, startFaceQuery is Query,
    endFaceQuery is Query, sweptEdgeQuery is Query) returns map
{
    // get our max radius edge
    // modify it to have a transform-invariant coordinate system
    var maxEdge = getMaxRadiusEdge(context, sweptEdgeQuery);
    if (maxEdge == undefined)
    {
        reportFeatureWarning(context, topLevelId, ErrorStringEnum.FRAME_MISSING_SWEPT_EDGES);
        return buildBeamCutlistData(undefined, undefined, undefined);
    }

    const beamCentroid = evApproximateCentroid(context, {
                "entities" : beam
            });
    const beamCentroidInArcPlane = project(plane(maxEdge.coordSystem), beamCentroid);
    const xAxis = beamCentroidInArcPlane - maxEdge.coordSystem.origin;
    const arcCoordinateSystem = coordSystem(maxEdge.coordSystem.origin, xAxis, maxEdge.coordSystem.zAxis);

    const startBoundingBox = evBox3d(context, {
                "topology" : startFaceQuery,
                "tight" : true,
                "coordSystem" : arcCoordinateSystem
            });

    const endBoundingBox = evBox3d(context, {
                "topology" : endFaceQuery,
                "tight" : true,
                "coordSystem" : arcCoordinateSystem
            });
    maxEdge.coordSystem = arcCoordinateSystem;

    const startCenterInPlane = project(plane(arcCoordinateSystem), box3dCenter(startBoundingBox));
    const endCenterInPlane = project(plane(arcCoordinateSystem), box3dCenter(endBoundingBox));
    const sweptLength = getCircularArcBeamLength(context, topLevelId, startFaceQuery,
        endFaceQuery, sweptEdgeQuery, startCenterInPlane, endCenterInPlane, maxEdge);
    const startAngle = getCircularArcBeamFaceAngleAtClosestBoundingBoxCorner(context, maxEdge, startCenterInPlane, startFaceQuery);
    const endAngle = getCircularArcBeamFaceAngleAtClosestBoundingBoxCorner(context, maxEdge, endCenterInPlane, endFaceQuery);
    return buildBeamCutlistData(sweptLength, startAngle, endAngle);
}

function getCircularArcBeamFaceAngleAtClosestBoundingBoxCorner(context is Context, maxEdge is map,
    faceCenterInPlaneWCS is Vector, faceQuery is Query)
{
    // WCS: world coordinate system
    // arcCoordinateSystem: The arc sits in the x-y plane of the is coordinate system, with origin at arc center
    // point.
    // edgeAlignedCoordinateSystem: edge This coordinate system has its z-axis as the beam tangent, centered on the
    // face bounding box.
    const arcCoordinateSystem = maxEdge.coordSystem;
    // only report angles for cut faces where the cutface normal is in the plane of the arc (ie clean cut along arc
    // axis)
    const facePlaneWCS = getPlaneFromCapFace(context, faceQuery);
    if (facePlaneWCS == undefined || !perpendicularVectors(facePlaneWCS.normal, arcCoordinateSystem.zAxis))
    {
        return undefined;
    }

    // this bounding box simplifies finding points in the cut face plane closest to arc central axis.
    const edgeAlignedCoordinateSystem = coordSystem(faceCenterInPlaneWCS, arcCoordinateSystem.zAxis, facePlaneWCS.normal);
    const boundingBoxECS = evBox3d(context, {
                "topology" : faceQuery,
                "tight" : true,
                "cSys" : edgeAlignedCoordinateSystem
            });
    // Ths finds the point of the cut face bounding box that is closest to the arc axis.
    // First, construct an edge aligned bounding box around the cut faces. The min and max lie in the cut plane.
    const candidatePointsECS = [
            boundingBoxECS.minCorner,
            boundingBoxECS.maxCorner
        ];
    // convert the points to WCS and project the corner points in to the arc plane.
    // Find the shortest vector between arc center and point. This is the closer corner in the cut face plane.
    const toCornersInPlaneWCS = mapArray(candidatePointsECS,
        point => arcCoordinateSystem.origin - project(plane(arcCoordinateSystem), toWorld(edgeAlignedCoordinateSystem, point)));

    const toClosestCornerInPlaneWCS = norm(toCornersInPlaneWCS[0]) < norm(toCornersInPlaneWCS[1]) ?
        toCornersInPlaneWCS[0] :
        toCornersInPlaneWCS[1];
    // use the cross product to get the beam tangent
    const tangentAtClosestCornerWCS = cross(arcCoordinateSystem.zAxis, toClosestCornerInPlaneWCS);
    const angle = getAngle(facePlaneWCS.normal, tangentAtClosestCornerWCS);
    return angle;
}

// attempts to create `NUM_ISOPARAM_CURVES` on the surface of a body
// these can be used as swept edge proxies for subsequent calculations
// returns undefined if surface is too complex to create curves
function createIsoParamCurves(context is Context, createdCurveId is Id, sweptFaceQuery is Query, bodiesToDelete is box)
{
    var curveNames = [];
    for (var i = 0; i < NUM_ISOPARAM_CURVES; i += 1)
    {
        curveNames = append(curveNames, "c" ~ i);
    }

    var curveDefinitions = [];
    for (var face in evaluateQuery(context, sweptFaceQuery))
    {
        const periodicity = evFacePeriodicity(context, { "face" : face });
        const curveType = getCurveType(periodicity);
        if (curveType == undefined)
        {
            // fail immediately if there is any unusable surface
            return undefined;
        }
        const curveDef = curveOnFaceDefinition(face, curveType, curveNames, NUM_ISOPARAM_CURVES);
        curveDefinitions = append(curveDefinitions, curveDef);
    }
    opCreateCurvesOnFace(context, createdCurveId, { "curveDefinition" : curveDefinitions });
    const curveQuery = qCreatedBy(createdCurveId, EntityType.EDGE);
    bodiesToDelete[] = append(bodiesToDelete[], qOwnerBody(curveQuery));
    return curveQuery;
}

function getCurveType(periodicity is array)
{
    if (periodicity[0])
    {
        return FaceCurveCreationType.DIR1_AUTO_SPACED_ISO;
    }
    else if (periodicity[1])
    {
        return FaceCurveCreationType.DIR2_AUTO_SPACED_ISO;
    }
    return undefined;
}

// A note on convention: Report the angle between the beam direction and the face normal.
// Reported value will be in [0,90] even if plane and tangent normals are misaligned.
// An uncut beam is 0 degrees. An angle "nearly" zero means the cap face is "nearly" perpendicular.
function getAngle(capPlaneNormal is Vector, beamTangent is Vector)
{
    // this corrects for (anti) direction or (anti) face normal
    var angle = angleBetween(capPlaneNormal, beamTangent);
    if (angle > 90 * degree)
    {
        angle = 180 * degree - angle;
    }

    // return exactly zero if near zero
    if (angle < TOLERANCE.zeroAngle * degree)
    {
        angle = 0 * degree;
    }
    return angle;
}

// This calculates the sweep angle, the angle subtended by the beam.
// From the `startWCS` point, the frame segment is in the `startBeamDirectionWCS` direction
function getSweptAngle(context is Context, startWCS is Vector, endWCS is Vector, startBeamDirectionWCS is Vector, maxEdge is map) returns ValueWithUnits
{
    // If the cross products are codirectional the angle is the minor arc angle.
    const startInPlaneWCS = project(plane(maxEdge.coordSystem), startWCS);
    const endInPlaneWCS = project(plane(maxEdge.coordSystem), endWCS);
    const arcCenterWCS = maxEdge.coordSystem.origin;
    // convert points in space to direction vectors
    const toStartInPlaneWCS = startInPlaneWCS - arcCenterWCS;
    const toEndInPlaneWCS = endInPlaneWCS - arcCenterWCS;
    // there are three cases: 0 degree sweep, 360 degree sweep, and 180 degree sweep
    // the first two are impossible so we early-exit here
    if (parallelVectors(toStartInPlaneWCS, toEndInPlaneWCS))
    {
        return 180 * degree;
    }
    const startCrossEnd = cross(toStartInPlaneWCS, toEndInPlaneWCS);
    const startCrossBeamDir = cross(toStartInPlaneWCS, startBeamDirectionWCS);
    // dot is stable directional indication IFF:
    // 1. cross products are non-zero (startCrossBeamDir is always non-zero)
    // 2. cross products are collinear (either parallel or antiparallel)
    const minorSweptAngle = angleBetween(toStartInPlaneWCS, toEndInPlaneWCS);
    // assumption 1 was already handled by the 180-degree early exit case
    // check assumption 2 and error if beams are malformed
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1758_SWEPT_ANGLE_FIX))
    {
        verify(parallelVectors(startCrossEnd, startCrossBeamDir), "Non-collinear cross products");
    }
    // use dot product to determine if sweptASngle is major or minor
    const isAligned = dot(startCrossEnd, startCrossBeamDir) > 0;
    const sweptAngle = isAligned ? minorSweptAngle : (2 * PI * radian) - minorSweptAngle;
    return sweptAngle;
}

// uses bounding boxes at edges to find the "max extent bounding box point" then uses that to compute largest possible
// sweep.
function getCircularArcBeamLength(context is Context, topLevelId is Id, startFaceQuery is Query, endFaceQuery is Query,
    sweptEdgeQuery is Query, startInPlaneWCS is Vector, endInPlaneWCS is Vector, maxEdge is map)
{
    var startBeamDirectionWCS;
    var endBeamDirectionWCS;
    try silent
    {
        startBeamDirectionWCS = getBeamDirectionAtFace(context, startFaceQuery, sweptEdgeQuery);
        endBeamDirectionWCS = getBeamDirectionAtFace(context, endFaceQuery, sweptEdgeQuery);
    }
    catch
    {
        reportFeatureWarning(context, topLevelId, ErrorStringEnum.FRAME_CUTLIST_NO_END_FACE_EDGE_GEOMETRY_PAIR);
        return undefined;
    }
    const maxStartPointWCS = getApproximateFarthestPointAlongArc(context, startInPlaneWCS, startFaceQuery, startBeamDirectionWCS,
        maxEdge);
    const maxEndPointWCS = getApproximateFarthestPointAlongArc(context, endInPlaneWCS, endFaceQuery, endBeamDirectionWCS,
        maxEdge);

    var sweptAngle;
    try silent
    {
        sweptAngle = getSweptAngle(context, maxStartPointWCS, maxEndPointWCS, startBeamDirectionWCS, maxEdge);
    }
    catch
    {
        reportFeatureWarning(context, topLevelId, ErrorStringEnum.FRAME_CUTLIST_NO_END_FACE_EDGE_GEOMETRY_PAIR);
        return undefined;
    }
    const sweptLength = maxEdge.radius * sweptAngle.value;
    return sweptLength;
}

function getApproximateFarthestPointAlongArc(context is Context, faceCenterInPlaneWCS is Vector, faceQuery is Query,
    beamDirWCS is Vector, maxEdge is map)
{
    const xAxis = faceCenterInPlaneWCS - maxEdge.coordSystem.origin;
    const zAxis = maxEdge.coordSystem.zAxis;
    // This coordinate system is "ECS".
    const edgeAlignedCoordinateSystem = coordSystem(faceCenterInPlaneWCS, xAxis, zAxis);
    const edgeAlignedBB = evBox3d(context, {
                "topology" : faceQuery,
                "tight" : true,
                "cSys" : edgeAlignedCoordinateSystem
            });

    // There are ways to iteratively converge to an exact solution, but users only need a strict overestimate.
    // A simple strict overestimate comes from the bounding box around the end faces.
    // The "near" corners are the corners closer to the center.
    // Of the near corners, use the one that is opposite the beam direction.
    const minPoint = edgeAlignedBB.minCorner;
    const maxPoint = edgeAlignedBB.maxCorner;
    const nearCornersECS = [vector(minPoint[0], minPoint[1], minPoint[2]), vector(minPoint[0], maxPoint[1], minPoint[2])];
    const beamDirECS = fromWorld(edgeAlignedCoordinateSystem).linear * beamDirWCS;
    const nearPointECS = dot(nearCornersECS[0], beamDirECS) < dot(nearCornersECS[1], beamDirECS) ?
        nearCornersECS[0] : nearCornersECS[1];
    const nearPointWCS = toWorld(edgeAlignedCoordinateSystem, nearPointECS);
    return nearPointWCS;
}

// real swept edges would have adjacency information but created curves do not.
// Use proximity instead of topology and find the first pair with distance = 0.
function getFaceEdgeAdjacentPair(context is Context, faceQuery is Query, sweptEdgeQuery is Query) returns map
{
    const edges = evaluateQuery(context, sweptEdgeQuery);
    const faces = evaluateQuery(context, faceQuery);

    for (var edge in edges)
    {
        for (var face in faces)
        {
            const distanceResult = try silent(evDistance(context, {
                            "side0" : edge,
                            "side1" : face
                        }));

            if (distanceResult == undefined)
            {
                continue;
            }

            if (tolerantEquals(0 * meter, distanceResult.distance))
            {
                const parameter = distanceResult.sides[0].parameter;
                // verify at a line endpoint
                verify(tolerantEquals(parameter, 0) || tolerantEquals(parameter, 1), "Didn't find edge end point");
                return { "face" : face, "edge" : edge, "parameter" : parameter };
            }
        }
    }
    throw regenError("No face-edge adjacent pair at cap face");
}

// uses a face-edge adjacent pair (in terms of distance, not topology) to the direction 'towards' the beam body
function getBeamDirectionAtFace(context is Context, faceQuery is Query, sweptEdgeQuery is Query) returns Vector
{
    const adjacentPair = getFaceEdgeAdjacentPair(context, faceQuery, sweptEdgeQuery);
    const edgeTangentLine = evEdgeTangentLine(context, {
                "edge" : adjacentPair.edge,
                "parameter" : adjacentPair.parameter
            });
    const sweepDirection = tolerantEquals(adjacentPair.parameter, 0) ?
        edgeTangentLine.direction :
        -1 * edgeTangentLine.direction;
    return sweepDirection;
}

function getPlaneFromCapFace(context is Context, capFaceQuery is Query)
{
    const capFaces = evaluateQuery(context, capFaceQuery);
    const capPlane = try silent(evPlane(context, { "face" : capFaces[0] }));

    // if multiple faces they must all be coplanar
    for (var i = 1; i < size(capFaces); i += 1)
    {
        const otherPlane = try silent(evPlane(context, { "face" : capFaces[i] }));
        if (capPlane == undefined || otherPlane == undefined || !coplanarPlanes(capPlane, otherPlane))
        {
            return undefined;
        }
    }
    return capPlane;
}

