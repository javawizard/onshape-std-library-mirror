FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/boundingtype.gen.fs", version : "✨");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "✨");
import(path : "onshape/std/chamfer.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/cutlistMath.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/coordSystem.fs", version : "✨");
import(path : "onshape/std/error.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/frameUtils.fs", version : "✨");
import(path : "onshape/std/fillet.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");
import(path : "onshape/std/manipulator.fs", version : "✨");
import(path : "onshape/std/offsetSurface.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");
import(path : "onshape/std/sketch.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/splitpart.fs", version : "✨");
import(path : "onshape/std/units.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");

const THICKNESS_MANIPULATOR_ID = "Thickness manipulator";
const OFFSET_MANIPULATOR_ID = "Offset manipulator";
const INTERNAL_MANIPULATOR_ID = "Inset manipulator";

const OFFSET_BOUNDS =
{
            (meter) : [0.001, 0.005, 500],
            (centimeter) : 0.5,
            (millimeter) : 5.0,
            (inch) : 0.25,
            (foot) : 0.025,
            (yard) : 0.01
        } as LengthBoundSpec;

const POSITIVE_LENGTH_BOUNDS =
{
            (meter) : [0.0, 0.0, 500],
            (centimeter) : 0.0,
            (millimeter) : 0.0,
            (inch) : 0.0,
            (foot) : 0.0,
            (yard) : 0.0
        } as LengthBoundSpec;

/** @internal */
export enum ProfileType
{
    annotation { "Name" : "Match profile" }
    MATCH_PROFILE,
    annotation { "Name" : "Rectangle" }
    RECTANGLE,
    annotation { "Name" : "Circle" }
    CIRCLE,
    annotation { "Name" : "Internal" }
    INTERNAL
}

/** @internal */
export enum CornerType
{
    annotation { "Name" : "None" }
    NONE,
    annotation { "Name" : "Chamfer" }
    CHAMFER,
    annotation { "Name" : "Fillet" }
    FILLET
}

/* @internal */
const ROUND_PRECISION = 3;

/**
 * Construct a endcap.
 */
annotation { "Feature Type Name" : "End cap",
        "Manipulator Change Function" : "manipulatorChange" }
export const endcap = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Faces", "Filter" : GeometryType.PLANE && ConstructionObject.NO }
        definition.faces is Query;

        annotation { "Name" : "Profile type" }
        definition.profileType is ProfileType;

        annotation { "Name" : "Thickness" }
        isLength(definition.thickness, OFFSET_BOUNDS);

        if (definition.profileType != ProfileType.INTERNAL)
        {
            annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.thicknessDirection is boolean;

            annotation { "Name" : "Offset", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE] }
            isLength(definition.offsetDistance, POSITIVE_LENGTH_BOUNDS);

            annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.offsetDirection is boolean;
        }

        if (definition.profileType == ProfileType.INTERNAL)
        {
            annotation { "Name" : "Offset", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE] }
            isLength(definition.internalOffsetDistance, POSITIVE_LENGTH_BOUNDS);

            annotation { "Name" : "Inset", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE] }
            isLength(definition.internalShiftDistance, POSITIVE_LENGTH_BOUNDS);
        }

        if (definition.profileType == ProfileType.RECTANGLE)
        {
            annotation { "Name" : "Treatment type", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE, UIHint.SHOW_LABEL] }
            definition.cornersType is CornerType;

            if (definition.cornersType == CornerType.CHAMFER)
            {
                annotation { "Name" : "Length", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE] }
                isLength(definition.cornerDistance, OFFSET_BOUNDS);
            }
            else if (definition.cornersType == CornerType.FILLET)
            {
                annotation { "Name" : "Radius", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE] }
                isLength(definition.cornerRadius, OFFSET_BOUNDS);
            }
        }
    }
    {
        verifyNonemptyQuery(context, definition, "faces", ErrorStringEnum.NO_CAP_FACE_SELECTED_ERROR);

        const featureId = new box(id);
        var isManipulatorAdded = new box(false);

        // To add an manipulator to the last face in QLV, we have to reverse the qlv sequence
        const faces = qUnion(reverse(evaluateQuery(context, definition.faces)));

        forEachEntity(context, id + "faceLoop", faces, function(face is Query, id is Id)
            {
                //Selected face might be one of many those form the end geometry of the beam
                //Collecting of all of them necessary for proper Cap geometry creation.
                //For a beam, check if the user has selected a “start” face or “end face”.
                //Then gather up all the other faces belonging to the body that have the same attribution.
                if (!isCapFace(context, face))
                {
                    throw regenError(ErrorStringEnum.INVALID_CAP_FACE_SELECTED_ERROR, ["faces"], face);
                }

                const body = qOwnerBody(face);
                var endCapFaces = qNothing();

                face = isStartFace(context, face) ? qFrameStartFace(body) : qFrameEndFace(body);

                if (definition.profileType != ProfileType.RECTANGLE && size(evaluateQuery(context, face)) > 1)
                {
                    throw regenError(ErrorStringEnum.CAP_MULTI_FACE_SELECTED_ERROR, ["faces"], face);
                }

                var selectedFace = face;

                const frameSegment = qOwnerBody(selectedFace);
                var bodiesToDelete = new box([]);
                var facesToDelete = qNothing();
                var lengthAndAngle = getCutlistLengthAndAngles(context, featureId[], id + "lengthAndAngle1", frameSegment, bodiesToDelete);

                if (!isQueryEmpty(context, qUnion(bodiesToDelete[])))
                {
                    opDeleteBodies(context, id + "deleteCutlistBodies", {
                                "entities" : qUnion(bodiesToDelete[])
                            });
                }

                if (!definition.thicknessDirection && definition.profileType != ProfileType.INTERNAL)
                {
                    if (lengthAndAngle.length > definition.thickness)
                    {
                        selectedFace = startTracking(context, face);
                        opOffsetFace(context, id + "offsetFace1", {
                                    "moveFaces" : face,
                                    "offsetDistance" : -definition.thickness
                                });
                    }
                    else
                    {
                        offsetSurface(context, id + "tempFace", {
                                    "surfacesAndFaces" : face,
                                    "offset" : 0 * meter
                                });

                        selectedFace = qCreatedBy(id + "tempFace", EntityType.FACE);

                        opOffsetFace(context, id + "offsetFace1", {
                                    "moveFaces" : selectedFace,
                                    "offsetDistance" : -definition.thickness
                                });

                        facesToDelete = selectedFace;

                        opDeleteBodies(context, id + "deleteBodies1", {
                                    "entities" : frameSegment
                                });
                    }
                }

                var selectedFaceGeometryMap = getFaceContours(context, id, selectedFace);
                const facePlane = selectedFaceGeometryMap.facePlane;

                //Only work for single lumen frame segments. Fail if multiple internal lumens detected.
                if (definition.profileType == ProfileType.INTERNAL && selectedFaceGeometryMap.loopsCount != 2)
                {
                    throw regenError(ErrorStringEnum.CAP_MULTI_LUMENS_SELECTED_ERROR, ["faces"], face);
                }

                facesToDelete = qUnion([facesToDelete,
                            selectedFaceGeometryMap.outerFace,
                            selectedFaceGeometryMap.tempFace,
                            selectedFaceGeometryMap.innerFace]);

                if (definition.profileType == ProfileType.INTERNAL)
                {
                    const internalCapFaceMap = generateProfileInternal(context, id, definition, selectedFace, selectedFaceGeometryMap);
                    const outerFaces = qSubtraction(internalCapFaceMap.innerFace, qFacesParallelToDirection(internalCapFaceMap.innerFace, facePlane.normal));
                    const edgeFaces = qSubtraction(internalCapFaceMap.innerFace, outerFaces);

                    if (definition.internalOffsetDistance > 0)
                    {
                        opOffsetFace(context, id + "offsetFace7", {
                                    "moveFaces" : edgeFaces,
                                    "offsetDistance" : -definition.internalOffsetDistance
                                });
                    }

                    //Internal shift manipulator
                    if (!isManipulatorAdded[])
                    {
                        var internalManipulator = linearManipulator({
                                "base" : internalCapFaceMap.drivePoint,
                                "direction" : internalCapFaceMap.direction,
                                "offset" : definition.internalShiftDistance,
                                "minValue" : TOLERANCE.zeroLength * meter,
                                "primaryParameterId" : "internalShiftDistance"
                            });

                        addManipulators(context, featureId[], {
                                    toString(INTERNAL_MANIPULATOR_ID) : internalManipulator
                                });

                        isManipulatorAdded[] = true;
                    }
                }
                else if (definition.profileType == ProfileType.MATCH_PROFILE)
                {
                    endCapFaces = generateProfileMatch(context, id, definition, selectedFace, selectedFaceGeometryMap);
                }
                else if (definition.profileType == ProfileType.RECTANGLE)
                {
                    endCapFaces = generateProfileRectangle(context, id, definition, selectedFace, facePlane);
                }
                else if (definition.profileType == ProfileType.CIRCLE)
                {
                    endCapFaces = generateProfileCircle(context, id, definition, selectedFace, selectedFaceGeometryMap);
                }

                if (definition.profileType != ProfileType.INTERNAL)
                {
                    const outerFaces = qSubtraction(endCapFaces, qFacesParallelToDirection(endCapFaces, facePlane.normal));
                    const edgeFaces = qSubtraction(endCapFaces, outerFaces);

                    //Thickness manipulator
                    if (!isManipulatorAdded[])
                    {
                        const manipulatorBase = definition.thicknessDirection ? facePlane.origin : facePlane.origin + (facePlane.normal * definition.thickness);
                        var thicknessManipulator = linearManipulator({
                                "base" : manipulatorBase,
                                "direction" : facePlane.normal,
                                "offset" : (definition.thicknessDirection ? 1 : -1) * definition.thickness,
                                "primaryParameterId" : "thickness"
                            });

                        addManipulators(context, featureId[], {
                                    toString(THICKNESS_MANIPULATOR_ID) : thicknessManipulator
                                });

                        const extrudePlane = evFaceTangentPlane(context, {
                                    "face" : qNthElement(edgeFaces, 0),
                                    "parameter" : vector(0.5, 0.5)
                                });

                        //Offset manipulator
                        var offsetManipulator = linearManipulator({
                                "base" : extrudePlane.origin,
                                "direction" : -extrudePlane.normal,
                                "offset" : (definition.offsetDirection ? 1 : -1) * definition.offsetDistance,
                                "primaryParameterId" : "offsetDistance"
                            });

                        addManipulators(context, featureId[], {
                                    toString(OFFSET_MANIPULATOR_ID) : offsetManipulator
                                });

                        isManipulatorAdded[] = true;
                    }

                    if (definition.offsetDistance > 0)
                    {
                        opOffsetFace(context, id + "offsetFace4", {
                                    "moveFaces" : edgeFaces,
                                    "offsetDistance" : definition.offsetDirection ? -definition.offsetDistance : definition.offsetDistance
                                });
                    }

                    if (definition.profileType == ProfileType.RECTANGLE && definition.cornersType != CornerType.NONE)
                    {
                        const chamferEdges = qNonCapEntity(id + "extrude", EntityType.EDGE);

                        if (definition.cornersType == CornerType.CHAMFER)
                        {
                            callSubfeatureAndProcessStatus(featureId[], chamfer, context, id + "chamfer1", {
                                        "entities" : chamferEdges,
                                        "chamferType" : ChamferType.EQUAL_OFFSETS,
                                        "width" : definition.cornerDistance
                                    });
                        }
                        else
                        {
                            callSubfeatureAndProcessStatus(featureId[], fillet, context, id + "fillet1", {
                                        "entities" : chamferEdges,
                                        "radius" : definition.cornerRadius
                                    });
                        }
                    }
                }

                if (!isQueryEmpty(context, facesToDelete))
                {
                    opDeleteFace(context, id + "deleteFace1", {
                                "deleteFaces" : facesToDelete,
                                "includeFillet" : false,
                                "capVoid" : false,
                                "leaveOpen" : false
                            });
                }
            });
    });

/** @internal */
function generateProfileInternal(context is Context, id is Id, definition is map, face is Query, faceGeometryMap is map) returns map
{
    //Get face for cap creation
    var internalCapFaceMap = createInternalCapFaceMap(context, id, face, faceGeometryMap);

    //Move face to initial position
    opOffsetFace(context, id + "offsetInnerFace", {
                "moveFaces" : internalCapFaceMap.innerFace,
                "offsetDistance" : internalCapFaceMap.invertedNormal ?
                -definition.internalShiftDistance + internalCapFaceMap.zeroOffset :
                definition.internalShiftDistance - internalCapFaceMap.zeroOffset
            });
    const drivePoint = evApproximateCentroid(context, {
                "entities" : internalCapFaceMap.innerFace
            });
    const thickness1 = internalCapFaceMap.invertedNormal ? 0 * inch : definition.thickness;
    const thickness2 = internalCapFaceMap.invertedNormal ? definition.thickness : 0 * inch;

    const thickenInnerCapId = id + "thickenInnerCap";
    opThicken(context, thickenInnerCapId, {
                "entities" : internalCapFaceMap.innerFace,
                "thickness1" : thickness1,
                "thickness2" : thickness2,
                "keepTools" : false
            });

    internalCapFaceMap.innerFace = qCreatedBy(thickenInnerCapId, EntityType.FACE);
    return internalCapFaceMap;
}

/** @internal */
function generateProfileMatch(context is Context, id is Id, definition is map, face is Query, faceGeometryMap is map) returns Query
{
    const facePlane = faceGeometryMap.facePlane;

    const extrudeId = id + "extrude";
    opExtrude(context, extrudeId, {
                "entities" : faceGeometryMap.outerFace,
                "direction" : definition.profileType == ProfileType.INTERNAL ? -facePlane.normal : facePlane.normal,
                "endBound" : BoundingType.BLIND,
                "endDepth" : definition.thickness
            });

    return qCreatedBy(extrudeId, EntityType.FACE);
}

/** @internal */
function generateProfileRectangle(context is Context, id is Id, definition is map, face is Query, facePlane is Plane) returns Query
{
    const partBB = evBox3d(context, {
                "topology" : face,
                "cSys" : coordSystem(facePlane),
                "tight" : true
            });

    const sketchId = id + "sketch";
    const extrudeId = id + "extrude";

    var rectangleSketch = newSketchOnPlane(context, sketchId, { "sketchPlane" : facePlane });

    skRectangle(rectangleSketch, "rectangle", {
                "firstCorner" : vector(partBB.minCorner[0], partBB.minCorner[1]),
                "secondCorner" : vector(partBB.maxCorner[0], partBB.maxCorner[1])
            });

    skSolve(rectangleSketch);

    opExtrude(context, extrudeId, {
                "entities" : qCreatedBy(sketchId, EntityType.FACE),
                "direction" : facePlane.normal,
                "endBound" : BoundingType.BLIND,
                "endDepth" : abs(definition.thickness)
            });

    opDeleteBodies(context, id + "deleteBodies", {
                "entities" : qCreatedBy(sketchId, EntityType.BODY)
            });

    return qCreatedBy(extrudeId, EntityType.FACE);
}

/** @internal */
function generateProfileCircle(context is Context, id is Id, definition is map, face is Query, faceGeometryMap is map) returns Query
{
    const facePlane = faceGeometryMap.facePlane;
    const edges = qAdjacent(faceGeometryMap.innerFace, AdjacencyType.EDGE, EntityType.EDGE);

    const partBB = evBox3d(context, {
                "topology" : face,
                "cSys" : WORLD_COORD_SYSTEM,
                "tight" : true
            });

    const centerBB = vector((partBB.maxCorner[0] + partBB.minCorner[0]) / 2,
        (partBB.maxCorner[1] + partBB.minCorner[1]) / 2,
        (partBB.maxCorner[2] + partBB.minCorner[2]) / 2);

    const radius = evDistance(context, { "side0" : centerBB, "side1" : edges, "maximum" : true }).distance;

    const sketchId = id + "sketch";
    const extrudeId = id + "extrude";

    var circleSketch = newSketchOnPlane(context, sketchId, { "sketchPlane" : facePlane });

    skCircle(circleSketch, "circle", {
                "center" : vector(0, 0) * meter,
                "radius" : radius
            });

    skSolve(circleSketch);

    opExtrude(context, extrudeId, {
                "entities" : qCreatedBy(sketchId, EntityType.FACE),
                "direction" : facePlane.normal,
                "endBound" : BoundingType.BLIND,
                "endDepth" : abs(definition.thickness)
            });

    opDeleteBodies(context, id + "deleteBodies", {
                "entities" : qCreatedBy(sketchId, EntityType.BODY)
            });

    return qCreatedBy(extrudeId, EntityType.FACE);
}

/** @internal */
function getOuterInnerFace(context is Context, id is Id, isOuter is boolean, faceGeometryMap is map) returns Query
{
    const loopsArray = faceGeometryMap.loopsArray;
    const loopCounter = faceGeometryMap.loopsCount;
    const pathArray = loopCounter == 0 ? [] : range(0, loopCounter - 1)->mapArray(function(i)
        {
            const bBox = evBox3d(context, {
                    "topology" : loopsArray[i],
                    "tight" : true
                });
            return { "size" : box3dDiagonalLength(bBox), "topology" : loopsArray[i] };
        }) -> sort(function(a, b)
        {
            return b.size - a.size;
        });

    const fillSurfaceId = id + "fillSurface";
    opFillSurface(context, fillSurfaceId, {
                "edgesG0" : pathArray[(loopCounter > 1 && !isOuter) ? 1 : 0].topology
            });

    return qCreatedBy(fillSurfaceId, EntityType.FACE);
}

/** @internal */
export function getFaceContours(context is Context, id is Id, face is Query) returns map
{
    offsetSurface(context, id + "isolatedFace",
        { "surfacesAndFaces" : face,
                "offset" : 0 * meter
            });
    const tempFace = qCreatedBy(id + "isolatedFace", EntityType.FACE);
    var surroundingEdges = qAdjacent(tempFace, AdjacencyType.EDGE, EntityType.EDGE);

    var loopsArray = [];
    while (!isQueryEmpty(context, surroundingEdges))
    {
        const seedEdge = qNthElement(surroundingEdges, 0);
        const newLoop = qLoopEdges(seedEdge);
        loopsArray = append(loopsArray, newLoop);
        surroundingEdges = qSubtraction(surroundingEdges, newLoop);
    }
    const facePlane = evPlane(context, {
                "face" : face
            });

    var result = {
        "tempFace" : tempFace,
        "facePlane" : facePlane,
        "loopsArray" : loopsArray,
        "loopsCount" : size(loopsArray)
    };

    result.innerFace = getOuterInnerFace(context, id + "innerFace", false, result);
    result.outerFace = getOuterInnerFace(context, id + "outerFace", true, result);

    return result;
}

/** @internal */
export function createInternalCapFaceMap(context is Context, id is Id, face is Query, faceGeometryMap is map) returns map
{
    //only straight frame segment can be processed
    const frameSegment = qOwnerBody(face);
    var direction = undefined;
    var innerFace = faceGeometryMap.innerFace;
    var invertedNormal = true;
    var zeroOffset = 0 * meter;

    const dirOutsideFrameSegment = evPlane(context, {
                    "face" : face
                }).normal;

    if (!parallelVectors(getFrameAxis(context, frameSegment), dirOutsideFrameSegment))
    {
        throw regenError(ErrorStringEnum.CAP_INCLINED_CUT_FRAME_ERROR, ["faces"], face);
    }

    //Processing 90 deg cut on frame segment
    direction = -dirOutsideFrameSegment;

    const newFaceNormal = evPlane(context, {
                    "face" : innerFace
                }).normal;
    invertedNormal = norm(dirOutsideFrameSegment + newFaceNormal) > 1;

    //delete temporary face - it's no longer required
    opDeleteBodies(context, id + "deleteFaceFromGeomMap", {
                "entities" : faceGeometryMap.tempFace
            });

    const drivePoint = evApproximateCentroid(context, {
                "entities" : innerFace
            });

    return {
            "innerFace" : innerFace,
            "invertedNormal" : invertedNormal,
            "direction" : direction,
            "drivePoint" : drivePoint,
            "zeroOffset" : zeroOffset
        };
}

/** @internal */
export function getFrameAxis(context is Context, body is Query) returns Vector
{
    const sweptEdges = qFrameSweptEdge(body);
    const sweptFace = qFrameSweptFace(body);

    if (!isQueryEmpty(context, sweptEdges))
    {
        const lineEdges = qGeometry(sweptEdges, GeometryType.LINE);
        if (!isQueryEmpty(context, lineEdges))
        {
            return evLine(context, {
                            "edge" : qNthElement(lineEdges, 0)
                        }).direction;
        }
    }

    try silent
    {
        const cylinderFaces = qGeometry(qNthElement(sweptFace, 0), GeometryType.CYLINDER);
        return evAxis(context, {
                        "axis" : qNthElement(cylinderFaces, 0)
                    }).direction;
    }

    throw regenError(ErrorStringEnum.CAP_FRAME_AXIS_ERROR);
}

/** @internal */
export function manipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    for (var key, value in newManipulators)
    {
        if (key == THICKNESS_MANIPULATOR_ID)
        {
            definition.thickness = abs(value.offset);
            definition.thicknessDirection = value.offset > 0;
        }
        if (key == OFFSET_MANIPULATOR_ID)
        {
            definition.offsetDistance = abs(value.offset);
            definition.offsetDirection = value.offset > 0;
        }
        if (key == INTERNAL_MANIPULATOR_ID)
        {
            if (tolerantEquals(definition.internalShiftDistance - value.offset, TOLERANCE.zeroLength * meter))
                continue;

            definition.internalShiftDistance = roundToPrecision(value.offset / meter, ROUND_PRECISION) * meter;
        }
    }

    return definition;
}
