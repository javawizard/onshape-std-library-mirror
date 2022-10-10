FeatureScript 1867; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/units.fs", version : "1867.0");
import(path : "onshape/std/valueBounds.fs", version : "1867.0");
import(path : "onshape/std/frameUtils.fs", version : "1867.0");
import(path : "onshape/std/feature.fs", version : "1867.0");
import(path : "onshape/std/evaluate.fs", version : "1867.0");
import(path : "onshape/std/containers.fs", version : "1867.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1867.0");
import(path : "onshape/std/string.fs", version : "1867.0");
import(path : "onshape/std/vector.fs", version : "1867.0");
import(path : "onshape/std/coordSystem.fs", version : "1867.0");
import(path : "onshape/std/sketch.fs", version : "1867.0");
import(path : "onshape/std/curveGeometry.fs", version : "1867.0");
import(path : "onshape/std/manipulator.fs", version : "1867.0");
import(path : "onshape/std/frameAttributes.fs", version : "1867.0");

const MIN_SIZE = NONNEGATIVE_LENGTH_BOUNDS[meter][0] * meter;
const MIN_THICKNESS = MIN_SIZE;

const THICKNESS_MANIPULATOR_ID = "Thickness manipulator";
const SIZE_MANIPULATOR_ID = "Size manipulator";
const OFFSET_MANIPULATOR_ID = "Offset manipulator";

enum GussetError
{
    InvalidChamferSize,
    NonFrameEdgeSelected,
    SweptEdgeSelected,
    GenericError
}

const ErrorToErrorMessageMap =
{
    GussetError.InvalidChamferSize: ErrorStringEnum.CHAMFER_SIZE_EXCEED_GUSSET_SIZE,
    GussetError.NonFrameEdgeSelected: ErrorStringEnum.NON_FRAME_EDGE_SELECTED,
    GussetError.SweptEdgeSelected: ErrorStringEnum.SWEPT_EDGE_SELECTED,
    GussetError.GenericError: ErrorStringEnum.CANNOT_FIT_A_GUSSET
};

const CHAMFER_SIZE_BOUNDS =
{
    (meter)      : [1e-5, 0.001, 500],
    (centimeter) : 0.1,
    (millimeter) : 1.0,
    (inch)       : 0.04,
    (foot)       : 0.003,
    (yard)       : 0.001
} as LengthBoundSpec;

/**
 * Defines the shape of the gusset.
 */
export enum GussetStyleType
{
    annotation { "Name" : "Triangle" }
    TRIANGLE,
    annotation { "Name" : "Rectangle" }
    RECTANGLE
}

/**
 * Create gussets based on the selected edges.
 */
annotation { "Feature Type Name" : "Gusset", "Manipulator Change Function" : "gussetManipulatorChange", "Editing Logic Function" : "gussetEditLogic"  }
export const gusset = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Edges", "Description" : "Edges that define the bases of the gussets", "Filter" : GeometryType.LINE  && BodyType.SOLID}
        definition.edges is Query;

        annotation { "Name" : "Size", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        isLength(definition.size, NONNEGATIVE_LENGTH_BOUNDS);

        annotation { "Name" : "Thickness", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        isLength(definition.thickness, NONNEGATIVE_LENGTH_BOUNDS);

        annotation { "Name" : "Offset", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        isLength(definition.offset, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.shouldFlipOffset is boolean;

        annotation { "Name" : "Gusset type", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE, UIHint.SHOW_LABEL] }
        definition.gussetType is GussetStyleType;

        if (definition.gussetType == GussetStyleType.RECTANGLE)
        {
            annotation { "Name" : "Flip base sides", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.shouldFlipBaseSides is boolean;

            annotation { "Name" : "Chamfer", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE, UIHint.DISPLAY_SHORT] }
            definition.chamfer is boolean;

            if (definition.chamfer)
            {
                annotation { "Name" : "Chamfer size", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE, UIHint.DISPLAY_SHORT] }
                isLength(definition.chamferSize, CHAMFER_SIZE_BOUNDS);
            }
        }

        annotation { "Name" : "Base faces", "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.baseSweptFaces is Query;
    }
    {
        verifyNonemptyQuery(context, definition, "edges", ErrorStringEnum.EMPTY_GUSSET_SELECTION);
        var lastTangentLine = new box(undefined);
        var lastManipulatorData = new box(undefined);
        var lastGussetId = new box(undefined);
        forEachEntity(context, id + "edgeLoop", definition.edges, function(currentEdge is Query, id is Id)
            {
                const gussetError = verifyGussetEdge(context, currentEdge);
                if (gussetError != undefined)
                {
                    throw regenError(ErrorToErrorMessageMap[gussetError], ["edges"], currentEdge);
                }
                const tangentLine = evEdgeTangentLine(context, {
                            "edge" : currentEdge,
                            "parameter" : 0.5
                        });
                var midpoint = tangentLine.origin;

                const closestSweptFaces = qContainsPoint(definition.baseSweptFaces, midpoint);
                const faceVerificationResult = verifyGussetBaseFaces(context, closestSweptFaces, currentEdge);
                if (faceVerificationResult.gussetError != undefined)
                {
                    throw regenError(ErrorToErrorMessageMap[faceVerificationResult.gussetError], ["edges"], qUnion([currentEdge, closestSweptFaces]));
                }

                midpoint += (definition.shouldFlipOffset ? 1 : -1) * tangentLine.direction * definition.offset;
                const manipulatorData = createGussetSolid(context, id, definition, faceVerificationResult.planeA, faceVerificationResult.planeB, midpoint);
                if (manipulatorData.gussetError != undefined)
                {
                    throw regenError(ErrorToErrorMessageMap[manipulatorData.gussetError], ["edges"], qUnion([currentEdge, closestSweptFaces]));
                }

                lastTangentLine[] = tangentLine;
                lastManipulatorData[] = manipulatorData;
                lastGussetId[] = id + "finalExtrude";
            });

        createMidpointManipulator(context, id, definition, lastTangentLine[]);
        createThicknessManipulator(context, id, definition, lastTangentLine[], lastGussetId[]);
        createSizeManipulator(context, id, lastManipulatorData[].sizeLine, lastManipulatorData[].sizeOffset);
    });

function createGussetSolid(context is Context, id is Id, definition is map, lhsPlane is Plane, rhsPlane is Plane, midpoint is Vector) returns map
{
    var planeA = undefined;
    var planeB = undefined;

    if (definition.shouldFlipBaseSides)
    {
        planeA = rhsPlane;
        planeB = lhsPlane;
    }
    else
    {
        planeA = lhsPlane;
        planeB = rhsPlane;
    }

    var result = {
        sizeLine : undefined,
        chamferLine : undefined,
        gussetError : undefined
    };

    const sketchPlaneNormal = cross(planeA.normal, planeB.normal);
    const endResultSketchPlane = plane(midpoint, sketchPlaneNormal, planeA.normal);
    const sketchCoord = coordSystem(endResultSketchPlane);

    const firstFaceDirection = cross(planeA.normal, sketchPlaneNormal);
    const firstCoord = coordSystem(midpoint, firstFaceDirection, sketchPlaneNormal);
    const triangleP1 = -getPointInFacePlane(firstCoord, sketchCoord, definition.size);

    const secondFaceDirection = cross(planeB.normal, sketchPlaneNormal);
    const secondCoord = coordSystem(midpoint, secondFaceDirection, sketchPlaneNormal);
    const triangleP2 = getPointInFacePlane(secondCoord, sketchCoord, definition.size);

    const startPoint = vector(0 * meter, 0 * meter);

    var points = [];
    points = append(points, startPoint);
    points = append(points, triangleP1);
    if (definition.gussetType == GussetStyleType.TRIANGLE)
    {
        const firstCoordInWorld = planeToWorld(endResultSketchPlane, triangleP1);
        const line = line(midpoint, firstCoordInWorld - midpoint);
        result.sizeLine = line;
        result.sizeOffset = norm(firstCoordInWorld - midpoint);
    }
    else if (definition.gussetType == GussetStyleType.RECTANGLE)
    {
        if (definition.chamfer)
        {
            const firstCoordInWorld = planeToWorld(endResultSketchPlane, triangleP1);
            const cornerPointInWorld = planeToWorld(endResultSketchPlane, vector(triangleP2[0], triangleP1[1]));
            const lastCoordInWorld = planeToWorld(endResultSketchPlane, triangleP2);

            const firstSideLength = norm(firstCoordInWorld - cornerPointInWorld);
            const secondSideLength = norm(cornerPointInWorld - lastCoordInWorld);
            if ((firstSideLength < definition.chamferSize) || (secondSideLength < definition.chamferSize))
            {
                result.gussetError = GussetError.InvalidChamferSize;
                return result;
            }

            points = append(points, vector(triangleP2[0] - definition.chamferSize, triangleP1[1]));
            points = append(points, vector(triangleP2[0], triangleP1[1] - definition.chamferSize));

            const secondCoordInWorld = planeToWorld(endResultSketchPlane, vector(triangleP2[0] - definition.chamferSize, triangleP1[1]));
            const sizeManipulatorFaceMidpoint = (firstCoordInWorld + secondCoordInWorld) / 2.0;

            const thirdCoordInWorld = planeToWorld(endResultSketchPlane, vector(triangleP2[0] - definition.chamferSize, 0 * meter));
            const forthCoordInWorld = planeToWorld(endResultSketchPlane, startPoint);
            const newOrigin = (thirdCoordInWorld + forthCoordInWorld) / 2.0;

            result.sizeLine = line(newOrigin, sizeManipulatorFaceMidpoint - newOrigin);
            result.sizeOffset = norm(sizeManipulatorFaceMidpoint - newOrigin);
        }
        else
        {
            points = append(points, vector(triangleP2[0], triangleP1[1]));

            const firstCoordInWorld = planeToWorld(endResultSketchPlane, triangleP1);
            const secondCoordInWorld = planeToWorld(endResultSketchPlane, vector(triangleP2[0], triangleP1[1]));
            const sizeManipulatorFaceMidpoint = (firstCoordInWorld + secondCoordInWorld) / 2.0;

            const thirdCoordInWorld = planeToWorld(endResultSketchPlane, vector(triangleP2[0], 0 * meter));
            const forthCoordInWorld = planeToWorld(endResultSketchPlane, startPoint);
            const newOrigin = (thirdCoordInWorld + forthCoordInWorld) / 2.0;

            result.sizeLine = line(newOrigin, sizeManipulatorFaceMidpoint - newOrigin);
            result.sizeOffset = norm(sizeManipulatorFaceMidpoint - newOrigin);
        }
    }

    points = append(points, triangleP2);
    points = append(points, startPoint);
    const endSketch = newSketchOnPlane(context, id + "profileEndSketch", {
                "sketchPlane" : endResultSketchPlane
            });
    skPolyline(endSketch, "gussetPolyline", {
                "points" : points
            });
    skSolve(endSketch);

    // This intersection call is already checked in verifyGussetBaseFaces, it can't fail
    const intersectionLine = intersection(planeA, planeB);
    opExtrude(context, id + "finalExtrude", {
                "entities" : qCreatedBy(id + "profileEndSketch", EntityType.FACE),
                "direction" : intersectionLine.direction,
                "endBound" : BoundingType.BLIND,
                "endDepth" : definition.thickness / 2,
                "startBound" : BoundingType.BLIND,
                "startDepth" : definition.thickness / 2
            });
    opDeleteBodies(context, id + "deleteFinalSketch", { "entities" : qCreatedBy(id + "profileEndSketch", EntityType.BODY)
            });

    return result;
}

function verifyGussetEdge(context is Context, edge is Query)
{
    const edgeOwner = qOwnerBody(edge);
    const frameProfileAttributes = getFrameProfileAttributes(context, edgeOwner);
    if (size(frameProfileAttributes) == 0)
    {
        return GussetError.NonFrameEdgeSelected;
    }

    const parentFaces = evaluateQuery(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE));
    for (var face in parentFaces)
    {
        if (isCapFace(context, face))
        {
            return undefined;
        }
    }
    return GussetError.SweptEdgeSelected;
}

function getPointInFacePlane(faceCoord is CoordSystem, sketchCoord is CoordSystem, size is ValueWithUnits) returns Vector
{
    const point = toWorld(faceCoord, vector(size, size * 0, size * 0));
    return worldToPlane(plane(sketchCoord), point);
}

function verifyGussetBaseFaces(context is Context, closestSweptFacesQuery is Query, currentEdge is Query) returns map
{
    var result = {
        planeA : undefined,
        planeB : undefined,
        gussetError : undefined
    };
    const owners = qOwnerBody(closestSweptFacesQuery);
    const edgeHasCorrectOwner = !isQueryEmpty(context, qIntersection([owners, qOwnerBody(currentEdge)]));
    const edgeOnTheJointOfTwoBodies = size(evaluateQuery(context, owners)) == 2;
    if (!(edgeHasCorrectOwner && edgeOnTheJointOfTwoBodies))
    {
        result.gussetError = GussetError.GenericError;
        return result;
    }

    const closestSweptFaces = evaluateQuery(context, closestSweptFacesQuery);
    const closestSweptPlanes = mapArray(closestSweptFaces, function(face)
    {
        // evPlane should always succeed due to the qGeometry(..., GeometryType.PLANE) in gussetEditLogic
        const plane = evPlane(context, {
                    "face" : face
                });
        return plane;
    });

    var parentFace = undefined;
    var planeA = undefined;
    var planeB = undefined;
    if (isQueryEmpty(context, qIntersection([qOwnerBody(currentEdge), qOwnerBody(closestSweptFaces[1])])))
    {
        parentFace = closestSweptFaces[0];
        planeA = closestSweptPlanes[1];
        planeB = closestSweptPlanes[0];
    }
    else
    {
        parentFace = closestSweptFaces[1];
        planeA = closestSweptPlanes[0];
        planeB = closestSweptPlanes[1];
    }

    const intersectionLine = intersection(planeA, planeB);
    if (isUndefinedOrEmptyString(intersectionLine))
    {
        result.gussetError = GussetError.GenericError;
        return result;
    }

    const tangentLineByFace = evEdgeTangentLine(context, {
                "edge" : currentEdge,
                "parameter" : 0.5,
                "face" : parentFace
            });
    const faceDirection = cross(planeA.normal, -tangentLineByFace.direction);
    if (dot(planeB.normal, faceDirection) < 0)
    {
        result.gussetError = GussetError.GenericError;
        return result;
    }

    result.planeA = planeA;
    result.planeB = planeB;
    return result;
}

function createMidpointManipulator(context is Context, id is Id, definition is map, tangentLine is Line)
{
    const midpointManipulator = linearManipulator({
                "base" : tangentLine.origin,
                "direction" : tangentLine.direction,
                "offset" : definition.shouldFlipOffset ? definition.offset : -definition.offset,
                "primaryParameterId" : "offset"
            });
    addManipulators(context, id, {
                (OFFSET_MANIPULATOR_ID) : midpointManipulator
            });
}

function createThicknessManipulator(context is Context, id is Id, definition is map, tangentLine is Line, solidId is Id)
{
    const centroid = evApproximateCentroid(context, {
                "entities" : qCreatedBy(solidId, EntityType.BODY)
            });
    const thicknessManipulator = linearManipulator({
                "base" : centroid,
                "direction" : tangentLine.direction,
                "offset" : definition.thickness / 2.0,
                "primaryParameterId" : "thickness",
                "minValue" : MIN_THICKNESS
            });
    addManipulators(context, id, {
                (THICKNESS_MANIPULATOR_ID) : thicknessManipulator
            });
}

function createSizeManipulator(context is Context, id is Id, tangentLine is Line, offset is ValueWithUnits)
{
    const sizeManipulator = linearManipulator({
                "base" : tangentLine.origin,
                "direction" : tangentLine.direction,
                "offset" : offset,
                "primaryParameterId" : "size",
                "minValue" : MIN_SIZE
            });
    addManipulators(context, id, {
                (SIZE_MANIPULATOR_ID) : sizeManipulator
            });
}

/** @internal */
export function gussetManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    for (var key, value in newManipulators)
    {
        if (key == OFFSET_MANIPULATOR_ID)
        {
            definition.offset = abs(value.offset);
            definition.shouldFlipOffset = value.offset > 0;
        }
        if (key == THICKNESS_MANIPULATOR_ID)
        {
            definition.thickness = value.offset * 2;
        }
        if (key == SIZE_MANIPULATOR_ID)
        {
            definition.size = value.offset;
        }
    }
    return definition;
}

/** @internal */
export function gussetEditLogic(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    var addedBaseSweptFaces = qNothing();
    const edges = evaluateQuery(context, definition.edges);
    for (var edge in edges)
    {
        const tangentLine = evEdgeTangentLine(context, {
                        "edge" : edge,
                        "parameter" : 0.5
                    });
        const midpoint = tangentLine.origin;

        const closestFaces = qHasAttributeWithValueMatching(FRAME_ATTRIBUTE_TOPOLOGY_NAME, { "topologyType" : FrameTopologyType.SWEPT_FACE })
            ->qSubtraction(qOwnedByBody(hiddenBodies, EntityType.FACE))
            ->qGeometry(GeometryType.PLANE)
            ->qContainsPoint(midpoint);

        addedBaseSweptFaces = qUnion([addedBaseSweptFaces, closestFaces]);
    }
    definition.baseSweptFaces = addedBaseSweptFaces;
    return definition;
}

