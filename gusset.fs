FeatureScript 2130; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/units.fs", version : "2130.0");
import(path : "onshape/std/valueBounds.fs", version : "2130.0");
import(path : "onshape/std/frameUtils.fs", version : "2130.0");
import(path : "onshape/std/feature.fs", version : "2130.0");
import(path : "onshape/std/evaluate.fs", version : "2130.0");
import(path : "onshape/std/containers.fs", version : "2130.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2130.0");
import(path : "onshape/std/string.fs", version : "2130.0");
import(path : "onshape/std/vector.fs", version : "2130.0");
import(path : "onshape/std/coordSystem.fs", version : "2130.0");
import(path : "onshape/std/sketch.fs", version : "2130.0");
import(path : "onshape/std/curveGeometry.fs", version : "2130.0");
import(path : "onshape/std/manipulator.fs", version : "2130.0");
import(path : "onshape/std/frameAttributes.fs", version : "2130.0");

const MIN_SIZE = NONNEGATIVE_LENGTH_BOUNDS[meter][0] * meter;

const LENGTH_MANIPULATOR_ID = "Length manipulator";
const OFFSET_MANIPULATOR_ID = "Offset manipulator";

const CHAMFER_SIZE_BOUNDS =
{
            (meter) : [1e-5, 0.001, 500],
            (centimeter) : 0.1,
            (millimeter) : 1.0,
            (inch) : 0.04,
            (foot) : 0.003,
            (yard) : 0.001
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
 * Defines the alignment of the gusset.
 */
export enum GussetPosition
{
    annotation { "Name" : "Centered" }
    CENTERED,
    annotation { "Name" : "Aligned" }
    ALIGNED
}

type GussetDefinition typecheck canBeGussetDefinition;

predicate canBeGussetDefinition(value)
{
    value is map;
    value.lhsPlane is Plane;
    value.rhsPlane is Plane;
    value.gussetMidpoint is Vector;
    value.closestSweptFacesQuery is Query;
    value.edgeMidpoint is Vector;
}

type ManipulatorDefinition typecheck canBeManipulatorDefinition;

predicate canBeManipulatorDefinition(value)
{
    value is map;
    value.sizeLine is Line;
    value.sizeOffset is ValueWithUnits;
}

type GussetCreationResult typecheck canBeGussetCreationResult;

predicate canBeGussetCreationResult(value)
{
    value is map;
    value.manipulatorDefinition is ManipulatorDefinition;
    value.points is array;
    for (var point in value.points)
        is2dPointVector(point);
    value.gussetError is ErrorStringEnum;
}

/**
 * Create gussets based on the selected edges.
 */
annotation { "Feature Type Name" : "Gusset", "Manipulator Change Function" : "gussetManipulatorChange", "Editing Logic Function" : "gussetEditLogic" }
export const gusset = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Edges", "Description" : "Edges that define the bases of the gussets", "Filter" : GeometryType.LINE && BodyType.SOLID }
        definition.edges is Query;

        annotation { "Name" : "Gusset type", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE, UIHint.SHOW_LABEL] }
        definition.gussetType is GussetStyleType;


        annotation { "Name" : "Length", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        isLength(definition.length, NONNEGATIVE_LENGTH_BOUNDS);

        if (definition.gussetType == GussetStyleType.RECTANGLE)
        {
            annotation { "Name" : "Flip base sides", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.shouldFlipBaseSides is boolean;
        }

        annotation { "Name" : "Thickness", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        isLength(definition.thickness, NONNEGATIVE_LENGTH_BOUNDS);

        if (definition.gussetType == GussetStyleType.RECTANGLE)
        {
            annotation { "Name" : "Chamfer", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE, UIHint.DISPLAY_SHORT] }
            definition.chamfer is boolean;

            if (definition.chamfer)
            {
                annotation { "Name" : "Chamfer size", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE, UIHint.DISPLAY_SHORT] }
                isLength(definition.chamferSize, CHAMFER_SIZE_BOUNDS);
            }
        }

        annotation { "Name" : "Gusset position", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE, UIHint.SHOW_LABEL] }
        definition.gussetPosition is GussetPosition;

        if (definition.gussetPosition == GussetPosition.ALIGNED)
        {
            annotation { "Name" : "Alignment entity", "Filter" : GeometryType.LINE && BodyType.SOLID, "MaxNumberOfPicks" : 1 }
            definition.alignedReference is Query;

            annotation { "Name" : "Opposite alignment", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.shouldFlipAlignment is boolean;
        }

        annotation { "Name" : "Offset distance", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        isLength(definition.offset, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.shouldFlipOffset is boolean;

        annotation { "Name" : "Base faces", "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.baseSweptFaces is Query;
    }
    {
        verifyNonemptyQuery(context, definition, "edges", ErrorStringEnum.EMPTY_GUSSET_SELECTION);
        if (!edgesAreParallel(context, definition.edges))
        {
            if (definition.gussetPosition == GussetPosition.ALIGNED)
            {
                throw regenError(ErrorStringEnum.GUSSET_ALIGNED_OFFSET_NOT_PARALLEL, ["edges"], definition.edges);
            }
            if (!tolerantEquals(definition.offset, 0 * meter))
            {
                throw regenError(ErrorStringEnum.GUSSET_OFFSET_NOT_PARALLEL, ["edges"], definition.edges);
            }
        }
        var remainingTransform;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1958_MIRROR_TOOL_GUSSET_SUPPORT))
        {
            remainingTransform = getRemainderPatternTransform(context, {
                    "references" : qUnion([definition.edges, definition.baseSweptFaces])
            });
        }
        const alignedOffset = getAlignedOffset(context, definition);
        const offsetDirection = getOffsetDirection(context, definition);
        const edges = reverse(evaluateQuery(context, definition.edges));
        for (var i = 0; i < size(edges); i += 1)
        {
            const currentEdge = edges[i];
            const loopId = id + unstableIdComponent(i);
            verifyGussetEdge(context, currentEdge);
            const tangentLine = evEdgeTangentLine(context, {
                        "edge" : currentEdge,
                        "parameter" : 0.5
                    });
            const midpoint = tangentLine.origin;

            const closestSweptFaces = qContainsPoint(definition.baseSweptFaces, midpoint);
            const gussetBasePlanes = getGussetBasePlanes(context, closestSweptFaces, currentEdge);
            const offset = (definition.shouldFlipOffset ? 1 : -1) * offsetDirection * definition.offset;
            const gussetDefinition = {
                        "lhsPlane" : gussetBasePlanes.planeA,
                        "rhsPlane" : gussetBasePlanes.planeB,
                        "gussetMidpoint" : midpoint + offset + alignedOffset,
                        "closestSweptFacesQuery" : closestSweptFaces,
                        "edgeMidpoint" : tangentLine.origin
                    } as GussetDefinition;
            const gussetCreationResult = createGussetSolid(context, loopId, definition, gussetDefinition);

            if (i == 0) // Add manipulators only to the last edge
            {
                createMidpointManipulator(context, id, definition, offsetDirection, midpoint + alignedOffset);
                createLengthManipulator(context, id, gussetCreationResult.manipulatorDefinition);
            }

            if (gussetCreationResult.gussetError != undefined)
            {
                throw regenError(gussetCreationResult.gussetError, ["edges"], qUnion([currentEdge, closestSweptFaces]));
            }
        }
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1958_MIRROR_TOOL_GUSSET_SUPPORT))
        {
            transformResultIfNecessary(context, id, remainingTransform);
        }
    });

function createGussetSolid(context is Context, id is Id, definition is map, gussetDefinition is GussetDefinition) returns GussetCreationResult
{
    var planeA = undefined;
    var planeB = undefined;

    if (definition.shouldFlipBaseSides)
    {
        planeA = gussetDefinition.rhsPlane;
        planeB = gussetDefinition.lhsPlane;
    }
    else
    {
        planeA = gussetDefinition.lhsPlane;
        planeB = gussetDefinition.rhsPlane;
    }

    const sketchPlaneNormal = cross(planeA.normal, planeB.normal);
    const endResultSketchPlane = plane(gussetDefinition.gussetMidpoint, sketchPlaneNormal, planeA.normal);
    const gussetBasePoints = getGussetBasePoints(definition.length, endResultSketchPlane, planeA, planeB, sketchPlaneNormal, gussetDefinition.gussetMidpoint);
    const startPoint = zeroVector(2) * meter;

    var result = definition.gussetType == GussetStyleType.TRIANGLE ?
    createTriangularGusset(definition, gussetDefinition.gussetMidpoint, endResultSketchPlane, gussetBasePoints) :
    createRectangularGusset(definition, gussetDefinition.gussetMidpoint, endResultSketchPlane, gussetBasePoints);

    var points = [startPoint, gussetBasePoints[0]];
    for (var point in result.points)
    {
        points = append(points, point);
    }
    points = append(points, gussetBasePoints[1]);
    points = append(points, startPoint);

    const endSketch = newSketchOnPlane(context, id + "profileEndSketch", {
                "sketchPlane" : endResultSketchPlane
            });
    skPolyline(endSketch, "gussetPolyline", {
                "points" : points
            });
    skSolve(endSketch);

    // This intersection call is already checked in getGussetBasePlanes, it can't fail
    const intersectionLine = intersection(planeA, planeB);
    if (definition.gussetPosition == GussetPosition.ALIGNED && !isQueryEmpty(context, definition.alignedReference))
    {
        const direction = normalize(gussetDefinition.gussetMidpoint - gussetDefinition.edgeMidpoint) * (definition.shouldFlipAlignment ? 1 : -1);
        opExtrude(context, id + "finalExtrude", {
                    "entities" : qCreatedBy(id + "profileEndSketch", EntityType.FACE),
                    "direction" : direction,
                    "endBound" : BoundingType.BLIND,
                    "endDepth" : definition.thickness
                });
        result.manipulatorDefinition.sizeLine.origin += direction * definition.thickness / 2;
    }
    else
    {
        opExtrude(context, id + "finalExtrude", {
                    "entities" : qCreatedBy(id + "profileEndSketch", EntityType.FACE),
                    "direction" : intersectionLine.direction,
                    "endBound" : BoundingType.BLIND,
                    "endDepth" : definition.thickness / 2,
                    "startBound" : BoundingType.BLIND,
                    "startDepth" : definition.thickness / 2
                });
    }
    opDeleteBodies(context, id + "deleteFinalSketch", { "entities" : qCreatedBy(id + "profileEndSketch", EntityType.BODY)
            });

    return result;
}

function getAlignedOffset(context is Context, definition is map) returns Vector
{
    var alignedOffset = zeroVector(3) * meter;
    if (definition.gussetPosition == GussetPosition.ALIGNED)
    {
        verifyNonemptyQuery(context, definition, "alignedReference", ErrorStringEnum.GUSSET_EMPTY_ALIGNMENT_SELECTION);
        const edge = qNthElement(definition.edges, 0);
        const alignmentLine = evEdgeTangentLine(context, {
                    "edge" : definition.alignedReference,
                    "parameter" : 0.5
                });
        const tangentEdgeLine = evEdgeTangentLine(context, {
                    "edge" : edge,
                    "parameter" : 0.5
                });
        const intersectionResult = intersection(tangentEdgeLine, alignmentLine);
        if (intersectionResult.dim == 0) // intersection is a point
        {
            alignedOffset = intersectionResult.intersection - tangentEdgeLine.origin;
        }
        else
        {
            throw regenError(ErrorStringEnum.GUSSET_ALIGNMENT_NO_INTERSECTION, ["alignedReference"], qUnion([edge, definition.alignedReference]));
        }
    }
    return alignedOffset;
}

function manipulatorDefinition(sizeLine is Line, offset is ValueWithUnits) returns ManipulatorDefinition
{
    return { "sizeLine" : sizeLine, "sizeOffset" : offset } as ManipulatorDefinition;
}

function getOffsetDirection(context is Context, definition is map) returns Vector
{
    const edges = evaluateQuery(context, definition.edges);
    const lastEdge = edges[size(edges) - 1];
    const tangentLine = evEdgeTangentLine(context, {
                "edge" : lastEdge,
                "parameter" : 0.5
            });
    return tangentLine.direction;
}

function createTriangularGusset(definition is map, midpoint is Vector, endResultSketchPlane is Plane, gussetBasePoints is array) returns map
{
    const firstCoordInWorld = planeToWorld(endResultSketchPlane, gussetBasePoints[0]);
    const line = line(midpoint, firstCoordInWorld - midpoint);
    const offset = norm(firstCoordInWorld - midpoint);
    return { "manipulatorDefinition" : manipulatorDefinition(line, offset), "points" : [], "gussetError" : undefined } as GussetCreationResult;
}

function createRectangularGusset(definition is map, midpoint is Vector, endResultSketchPlane is Plane, gussetBasePoints is array) returns GussetCreationResult
{
    const startPoint = zeroVector(2) * meter;
    const triangleP1 = gussetBasePoints[0];
    const triangleP2 = gussetBasePoints[1];

    var result = undefined;
    if (definition.chamfer)
    {
        if (!gussetChamferIsValid(definition, endResultSketchPlane, triangleP1, triangleP2))
        {
            result = createTriangularGusset(definition, midpoint, endResultSketchPlane, gussetBasePoints);
            result.gussetError = ErrorStringEnum.CHAMFER_SIZE_EXCEED_GUSSET_SIZE;
        }
        else
        {
            const firstCoordInWorld = planeToWorld(endResultSketchPlane, triangleP1);
            const secondCoordInWorld = planeToWorld(endResultSketchPlane, vector(triangleP2[0] - definition.chamferSize, triangleP1[1]));
            const lengthManipulatorFaceMidpoint = (firstCoordInWorld + secondCoordInWorld) / 2.0;
            const thirdCoordInWorld = planeToWorld(endResultSketchPlane, vector(triangleP2[0] - definition.chamferSize, 0 * meter));
            const forthCoordInWorld = planeToWorld(endResultSketchPlane, startPoint);
            const newOrigin = (thirdCoordInWorld + forthCoordInWorld) / 2.0;
            const points = [vector(triangleP2[0] - definition.chamferSize, triangleP1[1]), vector(triangleP2[0], triangleP1[1] - definition.chamferSize)];

            const sizeLine = line(newOrigin, lengthManipulatorFaceMidpoint - newOrigin);
            const sizeOffset = norm(lengthManipulatorFaceMidpoint - newOrigin);
            result = { "manipulatorDefinition" : manipulatorDefinition(sizeLine, sizeOffset), "points" : points, "gussetError" : undefined } as GussetCreationResult;
        }
    }
    else
    {
        const firstCoordInWorld = planeToWorld(endResultSketchPlane, triangleP1);
        const secondCoordInWorld = planeToWorld(endResultSketchPlane, vector(triangleP2[0], triangleP1[1]));
        const lengthManipulatorFaceMidpoint = (firstCoordInWorld + secondCoordInWorld) / 2.0;
        const thirdCoordInWorld = planeToWorld(endResultSketchPlane, vector(triangleP2[0], 0 * meter));
        const forthCoordInWorld = planeToWorld(endResultSketchPlane, startPoint);
        const newOrigin = (thirdCoordInWorld + forthCoordInWorld) / 2.0;
        const points = [vector(triangleP2[0], triangleP1[1])];

        const sizeLine = line(newOrigin, lengthManipulatorFaceMidpoint - newOrigin);
        const sizeOffset = norm(lengthManipulatorFaceMidpoint - newOrigin);
        result = { "manipulatorDefinition" : manipulatorDefinition(sizeLine, sizeOffset), "points" : points, "gussetError" : undefined } as GussetCreationResult;
    }
    return result;
}

function verifyGussetEdge(context is Context, edge is Query)
{
    const edgeOwner = qOwnerBody(edge);
    const frameProfileAttributes = getFrameProfileAttributes(context, edgeOwner);
    if (size(frameProfileAttributes) == 0)
    {
        throw regenError(ErrorStringEnum.NON_FRAME_EDGE_SELECTED, ["edges"], edge);
    }
    const parentFaces = evaluateQuery(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE));
    if (!any(parentFaces, function(f) { return isCapFace(context, f); }))
    {
        throw regenError(ErrorStringEnum.SWEPT_EDGE_SELECTED, ["edges"], edge);
    }
}

function getPointInFacePlane(faceCoord is CoordSystem, sketchCoord is CoordSystem, size is ValueWithUnits) returns Vector
{
    const point = toWorld(faceCoord, vector(size, size * 0, size * 0));
    return worldToPlane(plane(sketchCoord), point);
}

function getGussetBasePoints(gussetSize is ValueWithUnits, endResultSketchPlane is Plane, planeA is Plane, planeB is Plane, sketchPlaneNormal is Vector, midpoint is Vector) returns array
{
    const sketchCoord = coordSystem(endResultSketchPlane);

    const firstFaceDirection = cross(planeA.normal, sketchPlaneNormal);
    const firstCoord = coordSystem(midpoint, firstFaceDirection, sketchPlaneNormal);
    const triangleP1 = -getPointInFacePlane(firstCoord, sketchCoord, gussetSize);

    const secondFaceDirection = cross(planeB.normal, sketchPlaneNormal);
    const secondCoord = coordSystem(midpoint, secondFaceDirection, sketchPlaneNormal);
    const triangleP2 = getPointInFacePlane(secondCoord, sketchCoord, gussetSize);

    return [triangleP1, triangleP2];
}

function gussetChamferIsValid(definition is map, endResultSketchPlane is Plane, triangleP1 is Vector, triangleP2 is Vector) returns boolean
{
    const firstCoordInWorld = planeToWorld(endResultSketchPlane, triangleP1);
    const cornerPointInWorld = planeToWorld(endResultSketchPlane, vector(triangleP2[0], triangleP1[1]));
    const lastCoordInWorld = planeToWorld(endResultSketchPlane, triangleP2);

    const firstSideLength = norm(firstCoordInWorld - cornerPointInWorld);
    const secondSideLength = norm(cornerPointInWorld - lastCoordInWorld);
    return (firstSideLength > definition.chamferSize) && (secondSideLength > definition.chamferSize);
}

function getGussetBasePlanes(context is Context, closestSweptFacesQuery is Query, currentEdge is Query) returns map
{
    const genericError = regenError(ErrorStringEnum.CANNOT_FIT_A_GUSSET, ["edges"], qUnion([currentEdge, closestSweptFacesQuery]));
    var result = {
        planeA : undefined,
        planeB : undefined
    };
    const owners = qOwnerBody(closestSweptFacesQuery);
    const edgeHasCorrectOwner = !isQueryEmpty(context, qIntersection([owners, qOwnerBody(currentEdge)]));
    const edgeOnTheJointOfTwoBodies = size(evaluateQuery(context, owners)) == 2;
    if (!(edgeHasCorrectOwner && edgeOnTheJointOfTwoBodies))
    {
        throw genericError;
    }

    const closestSweptFaces = evaluateQuery(context, closestSweptFacesQuery);
    if (size(closestSweptFaces) != 2)
    {
        // We are in a situation when there are more than 2 swept faces that are adjacent to a selected edge midpoint,
        // but they belong only to 2 owners
        // This is possible in a scenario when the "Butt" join type is used, and a side edge is selected
        // See BEL-190472 for more details
        // It's technically not a swept edge, so throw GenericError
        throw genericError;
    }

    // evPlane might actually fail if upstream changes will change the geometry type
    // Such an event will not trigger edit logit, so old data is still stored in the closestSweptFacesQuery
    const closestSweptPlanes = closestSweptFaces->mapArray(function(face)
        {
            if (isQueryEmpty(context, qGeometry(face, GeometryType.PLANE)))
            {
                throw genericError;
            }
            return evPlane(context, {
                        "face" : face
                    });
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
        throw genericError;
    }

    const tangentLineByFace = evEdgeTangentLine(context, {
                "edge" : currentEdge,
                "parameter" : 0.5,
                "face" : parentFace
            });
    const faceDirection = cross(planeA.normal, -tangentLineByFace.direction);
    if (dot(planeB.normal, faceDirection) < 0)
    {
        throw genericError;
    }

    result.planeA = planeA;
    result.planeB = planeB;
    return result;
}

function edgesAreParallel(context is Context, edges is Query)
{
    return isQueryEmpty(context, qSubtraction(edges, qParallelEdges(edges, qNthElement(edges, 0))));
}

function createMidpointManipulator(context is Context, id is Id, definition is map, offsetDirection is Vector, manipulatorBase is Vector)
{
    const midpointManipulator = linearManipulator({
                "base" : manipulatorBase,
                "direction" : offsetDirection,
                "offset" : definition.shouldFlipOffset ? definition.offset : -definition.offset,
                "primaryParameterId" : "offset"
            });
    addManipulators(context, id, {
                (OFFSET_MANIPULATOR_ID) : midpointManipulator
            });
}

function createLengthManipulator(context is Context, id is Id, manipulatorDefinition is ManipulatorDefinition)
{
    const lengthManipulator = linearManipulator({
                "base" : manipulatorDefinition.sizeLine.origin,
                "direction" : manipulatorDefinition.sizeLine.direction,
                "offset" : manipulatorDefinition.sizeOffset,
                "primaryParameterId" : "length",
                "minValue" : MIN_SIZE
            });
    addManipulators(context, id, {
                (LENGTH_MANIPULATOR_ID) : lengthManipulator
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
        if (key == LENGTH_MANIPULATOR_ID)
        {
            definition.length = value.offset;
        }
    }
    return definition;
}

function collectBaseSweptFaces(context is Context, edgesToVisit is Query) returns Query
{
    const getClosestFaces = function(edge)
        {
            const tangentLine = evEdgeTangentLine(context, {
                        "edge" : edge,
                        "parameter" : 0.5
                    });
            const midpoint = tangentLine.origin;

            return qHasAttributeWithValueMatching(FRAME_ATTRIBUTE_TOPOLOGY_NAME, { "topologyType" : FrameTopologyType.SWEPT_FACE })
                ->qGeometry(GeometryType.PLANE)
                ->qContainsPoint(midpoint);
        };
    return evaluateQuery(context, edgesToVisit)->mapArray(getClosestFaces)->qUnion();
}

/** @internal */
export function gussetEditLogic(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    const addedBaseSweptFaces = collectBaseSweptFaces(context, qSubtraction(definition.edges, oldDefinition.edges == undefined ? qNothing() : oldDefinition.edges));
    definition.baseSweptFaces = qUnion([definition.baseSweptFaces, addedBaseSweptFaces]);
    return definition;
}

