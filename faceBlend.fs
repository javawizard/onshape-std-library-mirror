FeatureScript 2770; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/blendcontroltype.gen.fs", version : "2770.0");
export import(path : "onshape/std/faceblendcrosssection.gen.fs", version : "2770.0");
export import(path : "onshape/std/faceblendpropagation.gen.fs", version : "2770.0");
export import(path : "onshape/std/faceblendtrimtype.gen.fs", version : "2770.0");
export import(path : "onshape/std/faceblendcrosssectionshape.gen.fs", version : "2770.0");

export import(path : "onshape/std/manipulator.fs", version : "2770.0");

// Imports used internally
import(path : "onshape/std/evaluate.fs", version : "2770.0");
import(path : "onshape/std/feature.fs", version : "2770.0");
import(path : "onshape/std/geomOperations.fs", version : "2770.0");
import(path : "onshape/std/valueBounds.fs", version : "2770.0");
import(path : "onshape/std/vector.fs", version : "2770.0");
import(path : "onshape/std/containers.fs", version : "2770.0");
import(path : "onshape/std/math.fs", version : "2770.0");

const RATIO_BOUNDS =
{
    (unitless) : [0.0001, 1, 10000]
} as RealBoundSpec;

const PROPAGATION_ANGLE_BOUNDS =
{
    (degree) : [0, 0, 180],
    (radian) : 0
} as AngleBoundSpec;

/**
 * Feature performing a face blend.
 */
annotation { "Feature Type Name" : "Face blend",
             "Manipulator Change Function" : "faceBlendManipulatorChanges" }
export const faceBlend = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Side 1",
                    "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES && ActiveSheetMetal.NO }
        definition.side1 is Query;

        annotation { "Name" : "Flip side 1", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.flipSide1 is boolean;

        annotation { "Name" : "Side 2",
                    "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES && ActiveSheetMetal.NO }
        definition.side2 is Query;

        annotation { "Name" : "Flip side 2", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.flipSide2 is boolean;

        annotation { "Name" : "Propagation", "Default" : true }
        definition.propagation is boolean;

        annotation { "Group Name" : "Propagation", "Driving Parameter" : "propagation", "Collapsed By Default" : false }
        {
            if (definition.propagation)
            {
                annotation { "Name" : "Propagation type", "Default" : FaceBlendPropagation.TANGENT }
                definition.propagationType is FaceBlendPropagation;

                if (definition.propagationType == FaceBlendPropagation.CUSTOM) {
                    annotation { "Name" : "Maximum angle" }
                    isAngle(definition.propagationAngle, PROPAGATION_ANGLE_BOUNDS);
                }
            }
        }

        annotation { "Group Name" : "Cross section", "Collapsed By Default" : false }
        {
            annotation { "Name" : "Cross section", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE], "Default" : FaceBlendCrossSection.ROLLING_BALL }
            definition.crossSection is FaceBlendCrossSection;

            if (definition.crossSection == FaceBlendCrossSection.SWEPT_PROFILE)
            {
                annotation { "Name" : "Spine",
                            "Filter" : EntityType.EDGE, "MaxNumberOfPicks" : 1 }
                definition.spine is Query;
            }

            annotation { "Name" : "Measurement", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE], "Default" : BlendControlType.RADIUS }
            definition.blendControlType is BlendControlType;

            if (definition.blendControlType == BlendControlType.RADIUS)
            {
                annotation { "Name" : "Radius", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition.radius, BLEND_BOUNDS);
            }
            else if (definition.blendControlType == BlendControlType.WIDTH)
            {
                annotation { "Name" : "Width", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition.width, BLEND_BOUNDS);
            }

            annotation { "Name" : "Asymmetric", "Default" : false }
            definition.asymmetric is boolean;

            if (definition.asymmetric)
            {
                if (definition.blendControlType == BlendControlType.RADIUS)
                {
                    annotation { "Name" : "Second radius", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                    isLength(definition.secondRadius, BLEND_BOUNDS);
                }
                else if (definition.blendControlType == BlendControlType.WIDTH)
                {
                    annotation { "Name" : "Ratio", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                    isReal(definition.widthRatio, RATIO_BOUNDS);
                }
                annotation { "Name" : "Switch side", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                definition.switchAsymmetric is boolean;
            }

            annotation { "Name" : "Control", "Description" : "Cross sectional control", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE] }
            definition.crossSectionShape is FaceBlendCrossSectionShape;

            if (definition.crossSectionShape == FaceBlendCrossSectionShape.CONIC)
            {
                annotation { "Name" : "Rho" }
                isReal(definition.rho, FILLET_RHO_BOUNDS);
            }
            else if (definition.crossSectionShape == FaceBlendCrossSectionShape.CURVATURE)
            {
                annotation { "Name" : "Magnitude" }
                isReal(definition.magnitude, FILLET_RHO_BOUNDS);
            }
        }

        annotation { "Group Name" : "Constraints and limits", "Collapsed By Default" : true }
        {
            annotation { "Name" : "Tangent hold lines" }
            definition.tangentHoldLines is boolean;

            if (definition.tangentHoldLines)
            {
                annotation { "Name" : "Tangent edges",
                            "Filter" : EntityType.EDGE && ActiveSheetMetal.NO && ConstructionObject.NO && SketchObject.NO }
                definition.tangentEdges is Query;

                annotation { "Name" : "Inverse tangent edges",
                            "Filter" : EntityType.EDGE && ActiveSheetMetal.NO && ConstructionObject.NO && SketchObject.NO }
                definition.inverseTangentEdges is Query;
            }

            annotation { "Name" : "Conic hold lines" }
            definition.conicHoldLines is boolean;

            if (definition.conicHoldLines)
            {
                annotation { "Name" : "Conic edges",
                            "Filter" : EntityType.EDGE && ActiveSheetMetal.NO && ConstructionObject.NO && SketchObject.NO }
                definition.conicEdges is Query;

                annotation { "Name" : "Inverse conic edges",
                            "Filter" : EntityType.EDGE && ActiveSheetMetal.NO && ConstructionObject.NO && SketchObject.NO }
                definition.inverseConicEdges is Query;
            }

            annotation { "Name" : "Cliff edges" }
            definition.hasCliffEdges is boolean;

            if (definition.hasCliffEdges)
            {
                annotation { "Name" : "Cliff edges",
                            "Filter" : EntityType.EDGE && ActiveSheetMetal.NO && ConstructionObject.NO && SketchObject.NO }
                definition.cliffEdges is Query;
            }

            annotation { "Name" : "Caps" }
            definition.hasCaps is boolean;

            if (definition.hasCaps)
            {
                annotation { "Name" : "Caps", "Item name" : "cap", "Driven query" : "entity", "Item label template" : "#entity" }
                definition.caps is array;
                for (var cap in definition.caps)
                {
                    annotation { "Name" : "Cap entity", "Filter" : EntityType.FACE, "MaxNumberOfPicks" : 1 }
                    cap.entity is Query;

                    annotation { "Name" : "Flip cap", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                    cap.capFlip is boolean;
                }
            }

            annotation { "Name" : "Limits" }
            definition.hasLimits is boolean;

            if (definition.hasLimits)
            {
                annotation { "Name" : "Plane limit", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1 }
                definition.limitPlane1 is Query;
                annotation { "Name" : "Flip first plane", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                definition.limitPlane1Flip is boolean;
                annotation { "Name" : "Second plane limit", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1 }
                definition.limitPlane2 is Query;
                annotation { "Name" : "Flip second plane", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                definition.limitPlane2Flip is boolean;

                annotation { "Name" : "Face limits",
                            "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES && ActiveSheetMetal.NO }
                definition.faceLimits is Query;

                annotation { "Name" : "Edge limits", "Item name" : "edge limit", "Driven query" : "edge", "Item label template" : "#edge" }
                definition.edgeLimits is array;
                for (var edgeLimit in definition.edgeLimits)
                {
                    annotation { "Name" : "Edge limit", "Filter" : EntityType.EDGE, "MaxNumberOfPicks" : 1 }
                    edgeLimit.edge is Query;

                    annotation { "Name" : "Side", "Filter" : EntityType.FACE, "MaxNumberOfPicks" : 1 }
                    edgeLimit.edgeLimitSide is Query;
                }
            }

            annotation { "Name" : "Help point" }
            definition.hasHelpPoint is boolean;

            if (definition.hasHelpPoint)
            {
                annotation { "Name" : "Help point", "Filter" : EntityType.VERTEX || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
                definition.helpPoint is Query;
            }
        }

        annotation { "Name" : "Trim type", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE], "Default" : FaceBlendTrimType.WALLS }
        definition.trim is FaceBlendTrimType;

        annotation { "Name" : "Detached", "Default" : false }
        definition.detach is boolean;

        annotation { "Name" : "Show isocurves", "Default" : false }
        definition.showIsocurves is boolean;

        if (definition.showIsocurves)
        {
            annotation {"Name" : "Count" }
            isInteger(definition.curveCount, ISO_GRID_BOUNDS);
        }
    }
    {
        checkParameters(context, definition);

        const alignment = wallsNormalAlignment(context, definition);
        addFlipManipulators(context, id, definition, alignment);

        definition.flipSide1Normal = alignment.side1 ? definition.flipSide1 : !definition.flipSide1;
        definition.flipSide2Normal = alignment.side2 ? definition.flipSide2 : !definition.flipSide2;

        if (definition.asymmetric && definition.switchAsymmetric)
        {
            if (definition.blendControlType == BlendControlType.RADIUS)
            {
                var tmp = definition.radius;
                definition.radius = definition.secondRadius;
                definition.secondRadius = tmp;
            }
            else
            {
                definition.widthRatio = 1/definition.widthRatio;
            }
        }

        definition = extractHelpPointPosition(context, definition);
        opFaceBlend(context, id, definition);
    });

function checkParameters(context is Context, definition is map)
{
    if (isQueryEmpty(context, definition.side1))
    {
        throw regenError(ErrorStringEnum.FACE_BLEND_SELECT_FACES, ["side1"]);
    }
    if (size(evaluateQuery(context, qOwnerBody(definition.side1))) > 1)
    {
        throw regenError(ErrorStringEnum.FACE_BLEND_LEFT_WALL_MULTIPLE_BODIES, ["side1"]);
    }
    if (isQueryEmpty(context, definition.side2))
    {
        throw regenError(ErrorStringEnum.FACE_BLEND_SELECT_FACES, ["side2"]);
    }
    if (size(evaluateQuery(context, qOwnerBody(definition.side2))) > 1)
    {
        throw regenError(ErrorStringEnum.FACE_BLEND_RIGHT_WALL_MULTIPLE_BODIES, ["side2"]);
    }
    const solidBodies = qIntersection(qAllSolidBodies(), qUnion(qOwnerBody(definition.side1), qOwnerBody(definition.side2)));
    if ((definition.trim == FaceBlendTrimType.LONG || definition.trim == FaceBlendTrimType.NO_TRIM) &&
        !definition.detach &&
        !isQueryEmpty(context, solidBodies))
    {
        throw regenError(ErrorStringEnum.FACE_BLEND_SOLID_LONG_ATTACH, solidBodies);
    }
    if (definition.crossSection == FaceBlendCrossSection.SWEPT_PROFILE)
    {
        if (isQueryEmpty(context, definition.spine))
        {
            throw regenError(ErrorStringEnum.FACE_BLEND_SELECT_SPINE, ["spine"]);
        }
        if (definition.propagation)
        {
            if (definition.propagationType == FaceBlendPropagation.TANGENT)
            {
                throw regenError(ErrorStringEnum.FACE_BLEND_SPINE_TANGENT_PROPAGATION, ["propagationAngle"]);
            }
            if (definition.propagationType == FaceBlendPropagation.CUSTOM && definition.propagationAngle < TOLERANCE.zeroAngle * radian)
            {
                throw regenError(ErrorStringEnum.FACE_BLEND_SPINE_ZERO_ANGLE_PROPAGATION, ["propagationAngle"]);
            }
        }
    }
    var holdLines = qNothing();
    if (definition.tangentHoldLines)
    {
        if (definition.asymmetric && !isQueryEmpty(context, holdLines))
        {
            throw regenError(ErrorStringEnum.FACE_BLEND_TANGENT_HL_ASYMMETRIC, holdLines);
        }
        holdLines = qUnion([definition.tangentEdges, definition.inverseTangentEdges]);
    }
    if (definition.conicHoldLines)
    {
        holdLines = qUnion([holdLines, definition.conicEdges, definition.inverseConicEdges]);
    }
    if (definition.hasCliffEdges)
    {
        holdLines = qUnion([holdLines, definition.cliffEdges]);
    }
    if (definition.blendControlType == BlendControlType.WIDTH && !isQueryEmpty(context, holdLines))
    {
        throw regenError(ErrorStringEnum.FACE_BLEND_WIDTH_HOLD_LINES, holdLines);
    }
    var walls = qUnion([definition.side1, definition.side2]);
    var faultyHoldLines = qNothing();
    for (var holdLine in evaluateQuery(context, holdLines))
    {
        if (isQueryEmpty(context, qIntersection([walls, qAdjacent(holdLine, AdjacencyType.EDGE, EntityType.FACE)])))
        {
            faultyHoldLines = qUnion([faultyHoldLines, holdLine]);
        }
    }
    if (!isQueryEmpty(context, faultyHoldLines))
    {
        throw regenError(ErrorStringEnum.FACE_BLEND_HOLD_LINE_IN_WALLS, faultyHoldLines);
    }
    if (definition.hasLimits && size(definition.edgeLimits) > 0)
    {
        for (var edgeLimit in definition.edgeLimits)
        {
            if (!isQueryEmpty(context, edgeLimit.edge))
            {
                if (isQueryEmpty(context, edgeLimit.edgeLimitSide))
                {
                    throw regenError(ErrorStringEnum.FACE_BLEND_EDGE_LIMIT_NEEDS_SIDE);
                }
                const adjacentFaces = qAdjacent(edgeLimit.edge, AdjacencyType.EDGE, EntityType.FACE);
                if (isQueryEmpty(context, qIntersection([adjacentFaces, edgeLimit.edgeLimitSide])))
                {
                    throw regenError(ErrorStringEnum.FACE_BLEND_EDGE_LIMIT_NEEDS_SIDE);
                }
            }
        }
    }
}

function extractHelpPointPosition(context is Context, definition is map) returns map
{
    if (!definition.hasHelpPoint)
    {
        return definition;
    }
    const vertex = qEntityFilter(definition.helpPoint, EntityType.VERTEX);
    const mateConnector = qBodyType(definition.helpPoint, BodyType.MATE_CONNECTOR);
    if (size(evaluateQuery(context, vertex)) == 1)
    {
        definition.helpPointPosition = evVertexPoint(context, {
                "vertex" : vertex
            });
        return definition;
    }
    if (size(evaluateQuery(context, mateConnector)) == 1)
    {
        definition.helpPointPosition = evMateConnector(context, {
                "mateConnector" : mateConnector
            }).origin;
    }
    return definition;
}

// Determinines if the "correct" way to specify flips is aligned with the faces' normals.
// Only works for topologically adjacent faces and faces that don't intersect.
function wallsNormalAlignment(context is Context, definition is map) returns map
{
    var result = { "side1" : true, "side2" : true };
    const adjacentFacesRightWall = qIntersection([definition.side2, qAdjacent(definition.side1, AdjacencyType.EDGE, EntityType.FACE)]);
    if (!isQueryEmpty(context, adjacentFacesRightWall))
    {
        const adjacentFaceRightWall = qNthElement(adjacentFacesRightWall, 0);
        const adjacentFaceLeftWall = qNthElement(qIntersection([definition.side1, qAdjacent(adjacentFaceRightWall, AdjacencyType.EDGE, EntityType.FACE)]), 0);

        const commonEdge = qIntersection([qAdjacent(adjacentFaceLeftWall, AdjacencyType.EDGE, EntityType.EDGE), qAdjacent(adjacentFaceRightWall, AdjacencyType.EDGE, EntityType.EDGE)]);

        const convexity = evEdgeConvexity(context, {
                "edge" : qNthElement(commonEdge, 0)
        });

        const aligned = convexity == EdgeConvexityType.CONCAVE;

        result.side1 = aligned;
        result.side2 = aligned;
    }
    else
    {
        const distanceResult = evDistance(context, {
                "side0" : qNthElement(definition.side1, 0),
                "side1" : qNthElement(definition.side2, 0)
        });
        if (distanceResult.distance > 0)
        {
            const paramSide1 = distanceResult.sides[0].parameter;
            const pointSide1 = distanceResult.sides[0].point;
            const paramSide2 = distanceResult.sides[1].parameter;
            const pointSide2 = distanceResult.sides[1].point;

            const normalAtPointSide1 = evFaceTangentPlane(context, {
                    "face" : qNthElement(definition.side1, 0),
                    "parameter" : vector(paramSide1[0], paramSide1[1])
            }).normal;
            const normalAtPointSide2 = evFaceTangentPlane(context, {
                    "face" : qNthElement(definition.side2, 0),
                    "parameter" : vector(paramSide2[0], paramSide2[1])
            }).normal;

            const side1ToSide2 = pointSide2 - pointSide1;

            result.side1 = dot(normalAtPointSide1, side1ToSide2) >= 0;
            result.side2 = dot(normalAtPointSide2, side1ToSide2) <= 0;
        }
    }
    return result;
}

const LEFT_WALL_FLIP_MANIPULATOR = "side1FlipManipulator";
const RIGHT_WALL_FLIP_MANIPULATOR = "side2FlipManipulator";
const CAP_FLIP_MANIPULATOR = "capFlipManipulator";
const LIMIT_PLANE_FLIP_MANIPULATOR = "limitPlaneFlipManipulator";

function addFlipManipulators(context is Context, id is Id, definition is map, alignmentMap is map)
{
    var manipulators = {};
    var result = evFaceTangentPlane(context, {
            "face" : qNthElement(definition.side1, 0),
            "parameter" : vector(0.5, 0.5)
    });
    manipulators[LEFT_WALL_FLIP_MANIPULATOR] = flipManipulator({
            "base" : result.origin,
            "direction" : (alignmentMap.side1 ? 1 : -1) * result.normal,
            "flipped" : definition.flipSide1
    });
    result = evFaceTangentPlane(context, {
            "face" : qNthElement(definition.side2, 0),
            "parameter" : vector(0.5, 0.5)
    });
    manipulators[RIGHT_WALL_FLIP_MANIPULATOR] = flipManipulator({
            "base" : result.origin,
            "direction" : (alignmentMap.side2 ? 1 : -1) * result.normal,
            "flipped" : definition.flipSide2
    });
    if (definition.hasCaps)
    {
        for (var i = 0; i < size(definition.caps); i += 1)
        {
            if (isQueryEmpty(context, definition.caps[i].entity))
            {
                continue;
            }
            result = evFaceTangentPlane(context, {
                    "face" : definition.caps[i].entity,
                    "parameter" : vector(0.5, 0.5)
            });
            manipulators[CAP_FLIP_MANIPULATOR ~ "." ~ i] = flipManipulator({
                    "base" : result.origin,
                    "direction" : result.normal,
                    "flipped" : definition.caps[i].capFlip
            });
        }
    }
    if (definition.hasLimits)
    {
        if (!isQueryEmpty(context, definition.limitPlane1))
        {
            const plane = evPlane(context, {
                    "face" : definition.limitPlane1
            });
            manipulators[LIMIT_PLANE_FLIP_MANIPULATOR ~ ".1"] = flipManipulator({
                    "base" : plane.origin,
                    "direction" : plane.normal,
                    "flipped" : definition.limitPlane1Flip
            });
        }
        if (!isQueryEmpty(context, definition.limitPlane2))
        {
            const plane = evPlane(context, {
                    "face" : definition.limitPlane2
            });
            manipulators[LIMIT_PLANE_FLIP_MANIPULATOR ~ ".2"] = flipManipulator({
                    "base" : plane.origin,
                    "direction" : plane.normal,
                    "flipped" : definition.limitPlane2Flip
            });
        }
    }

    addManipulators(context, id, manipulators);
}

/**
 * @internal
 * The manipulator change function for [faceBlend].
 */
export function faceBlendManipulatorChanges(context is Context, definition is map, newManipulators is map) returns map
{
    if (newManipulators[LEFT_WALL_FLIP_MANIPULATOR] is map)
    {
        definition.flipSide1 = newManipulators[LEFT_WALL_FLIP_MANIPULATOR].flipped;
    }
    if (newManipulators[RIGHT_WALL_FLIP_MANIPULATOR] is map)
    {
        definition.flipSide2 = newManipulators[RIGHT_WALL_FLIP_MANIPULATOR].flipped;
    }
    for (var i = 0; i < size(definition.caps); i += 1)
    {
        if (newManipulators[CAP_FLIP_MANIPULATOR ~ "." ~ i] is map)
        {
            definition.caps[i].capFlip = newManipulators[CAP_FLIP_MANIPULATOR ~ "." ~ i].flipped;
        }
    }
    if (newManipulators[LIMIT_PLANE_FLIP_MANIPULATOR ~ ".1"] is map)
    {
        definition.limitPlane1Flip = newManipulators[LIMIT_PLANE_FLIP_MANIPULATOR ~ ".1"].flipped;
    }
    if (newManipulators[LIMIT_PLANE_FLIP_MANIPULATOR ~ ".2"] is map)
    {
        definition.limitPlane2Flip = newManipulators[LIMIT_PLANE_FLIP_MANIPULATOR ~ ".2"].flipped;
    }
    return definition;
}

