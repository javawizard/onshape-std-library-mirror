FeatureScript 2679; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "2679.0");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "2679.0");
import(path : "onshape/std/bridgingCurve.fs", version : "2679.0");
import(path : "onshape/std/containers.fs", version : "2679.0");
import(path : "onshape/std/coordSystem.fs", version : "2679.0");
import(path : "onshape/std/curveGeometry.fs", version : "2679.0");
import(path : "onshape/std/error.fs", version : "2679.0");
import(path : "onshape/std/evaluate.fs", version : "2679.0");
import(path : "onshape/std/feature.fs", version : "2679.0");
import(path : "onshape/std/frameAttributes.fs", version : "2679.0");
import(path : "onshape/std/instantiator.fs", version : "2679.0");
import(path : "onshape/std/manipulator.fs", version : "2679.0");
import(path : "onshape/std/path.fs", version : "2679.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2679.0");
import(path : "onshape/std/tabReferences.fs", version : "2679.0");
import(path : "onshape/std/tool.fs", version : "2679.0");
import(path : "onshape/std/topologyUtils.fs", version : "2679.0");
import(path : "onshape/std/transform.fs", version : "2679.0");
import(path : "onshape/std/valueBounds.fs", version : "2679.0");
import(path : "onshape/std/vector.fs", version : "2679.0");

export import(path : "onshape/std/profilecontrolmode.gen.fs", version : "2679.0");
export import(path : "onshape/std/frameUtils.fs", version : "2679.0");

/** @internal */
export const FRAME_NINE_POINT_COUNT =
{
            (unitless) : [0, 4, 100]
        }
    as IntegerBoundSpec;

/** @internal */
export const FRAME_NINE_POINT_CENTER_INDEX = 4;

// in `extendFrames` we pad our frame extrusion length to help avoid non-manifold cases in boolean operations
const EXTEND_FRAMES_PAD_LENGTH = .1 * millimeter;

/**
 * Create frames from a profile and set of path selections.
 */
annotation {
        "Feature Type Name" : "Frame",
        "Manipulator Change Function" : "frameManipulators",
        "Filter Selector" : "allparts",
        "Editing Logic Function" : "frameEditLogicFunction"
    }
export const frame = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation {
                    "Library Definition" : "65dcc2a02c4ff1c239467ec9", // This is the id of the Onshape Frame Profile Library definition
                    "Name" : "Sketch profile",
                    "Filter" : PartStudioItemType.SKETCH,
                    "MaxNumberOfPicks" : 1,
                    "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE
                }
        definition.profileSketch is PartStudioData;

        annotation {
                    "Name" : "Selections",
                    "Description" : "Faces, edges, and vertices that define sweep paths",
                    "Filter" : ((EntityType.FACE && ConstructionObject.NO) || EntityType.EDGE || (EntityType.VERTEX && AllowEdgePoint.NO) || (EntityType.BODY && BodyType.WIRE && SketchObject.NO))
                }
        definition.selections is Query;

        annotation {
                    "Name" : "Lock profile faces",
                    "Default" : false
                }
        definition.lockProfile is boolean;

        if (definition.lockProfile)
        {
            annotation { "Group Name" : "Lock profile faces", "Collapsed By Default" : false, "Driving Parameter" : "lockProfile" }
            {
                annotation { "Name" : "Faces to lock", "Filter" : EntityType.FACE && ConstructionObject.NO }
                definition.profileLockFaces is Query;
            }
        }

        annotation { "Name" : "Merge tangent segments", "Default" : true }
        definition.mergeTangentSegments is boolean;

        annotation { "Name" : "Angle" }
        isAngle(definition.angle, ANGLE_360_ZERO_DEFAULT_BOUNDS);

        annotation {
                    "Name" : "Mirror across Y axis",
                    "UIHint" : UIHint.OPPOSITE_DIRECTION
                }
        definition.mirrorProfile is boolean;

        annotation { "Name" : "Angle reference", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
        definition.angleReference is Query;

        annotation { "Name" : "Default corner type", "UIHint" : UIHint.SHOW_LABEL }
        definition.defaultCornerType is FrameCornerType;

        if (definition.defaultCornerType == FrameCornerType.BUTT || definition.defaultCornerType == FrameCornerType.COPED_BUTT)
        {
            annotation {
                        "Name" : "Flip corner",
                        "UIHint" : UIHint.OPPOSITE_DIRECTION
                    }
            definition.defaultButtFlip is boolean;
        }

        annotation {
                    "Name" : "Corner overrides",
                    "Item name" : "vertex",
                    "Driven query" : "vertex",
                    "Item label template" : "#vertex [#cornerType]"
                }
        definition.cornerOverrides is array;
        for (var corner in definition.cornerOverrides)
        {
            annotation {
                        "Name" : "Vertex",
                        "Filter" : EntityType.VERTEX && ConstructionObject.NO,
                        "MaxNumberOfPicks" : 1,
                        "UIHint" : UIHint.ALWAYS_HIDDEN
                    }
            corner.vertex is Query;

            annotation {
                        "Name" : "Corner type",
                        "UIHint" : UIHint.SHOW_LABEL
                    }
            corner.cornerType is FrameCornerType;

            if (corner.cornerType == FrameCornerType.BUTT || corner.cornerType == FrameCornerType.COPED_BUTT)
            {
                annotation {
                            "Name" : "Flip corner",
                            "UIHint" : UIHint.OPPOSITE_DIRECTION
                        }
                corner.cornerButtFlip is boolean;
            }
        }

        annotation {
                    "Name" : "Limit frame ends",
                    "Default" : false
                }
        definition.trim is boolean;

        if (definition.trim)
        {
            annotation {
                        "Group Name" : "Trimming",
                        "Collapsed By Default" : false,
                        "Driving Parameter" : "trim"
                    }
            {
                annotation {
                            "Name" : "Faces to trim to",
                            "Description" : "Planes and planar faces to use as trim tools",
                            "Filter" : (EntityType.FACE && GeometryType.PLANE)
                        }
                definition.trimPlanes is Query;

                annotation {
                            "Name" : "Parts to trim to",
                            "Description" : "Parts to use as trim tools",
                            "Filter" : EntityType.BODY && BodyType.SOLID
                        }
                definition.trimBodies is Query;
            }
        }
        annotation { "Name" : "Index", "UIHint" : UIHint.ALWAYS_HIDDEN }
        isInteger(definition.index, FRAME_NINE_POINT_COUNT); // for points manipulator
    }
    {
        doFrame(context, id, definition);
    },
    {
            mirrorProfile : false,
            defaultCornerType : FrameCornerType.MITER,
            index : FRAME_NINE_POINT_CENTER_INDEX,
            angle : 0 * degree,
            cornerOverrides : [],
            trim : false,
            mergeTangentSegments : true,
            angleReference : qNothing(),
            lockProfile : false
        });

/** @internal */
export function frameEditLogicFunction(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean) returns map
{
    if (oldDefinition.cornerOverrides != undefined)
    {
        definition = handleNewCornerOverride(context, oldDefinition, definition);
    }
    return definition;
}

/** @internal */
export function frameManipulators(context is Context, definition is map, newManipulators is map) returns map
{
    try silent
    {
        var newAngle is ValueWithUnits = newManipulators["angleManipulator"].angle;
        definition.angle = newAngle;
    }
    try silent
    {
        definition.index = newManipulators["points"].index;
    }
    for (var i = 0; i < size(definition.cornerOverrides); i += 1)
    {
        if (newManipulators["flip" ~ i] != undefined)
        {
            definition.cornerOverrides[i].cornerButtFlip = newManipulators["flip" ~ i].flipped;
        }
    }
    return definition;
}

/** @internal */
export function setFrameAttributes(context is Context, frame is Query, profileData is map, frameData is map)
{
    setFrameProfileAttribute(context, frame, profileData.profileAttribute);
    setFrameTopologyAttribute(context, frameData.sweptFaces, frameTopologyAttributeForSwept(FrameTopologyType.SWEPT_FACE));
    if (!isQueryEmpty(context, frameData.sweptEdges))
    {
        setFrameTopologyAttribute(context, frameData.sweptEdges, frameTopologyAttributeForSwept(FrameTopologyType.SWEPT_EDGE));
    }
    setFrameTopologyAttribute(context, frameData.startFace, frameTopologyAttributeForCapFace(true, false, false));
    setFrameTopologyAttribute(context, frameData.endFace, frameTopologyAttributeForCapFace(false, false, false));
}

/** @internal */
export function setFrameTerminusAttributes(context is Context, startFace is Query, endFace is Query)
{
    setFrameTopologyAttribute(context, startFace, frameTopologyAttributeForCapFace(true, true, false));
    setFrameTopologyAttribute(context, endFace, frameTopologyAttributeForCapFace(false, true, false));
}

function handleNewCornerOverride(context is Context, oldDefinition is map, definition is map) returns map
{
    // on joint override creation, set the starting value to the global default
    const newOverrideSize = size(definition.cornerOverrides);
    const oldOverrideSize = size(oldDefinition.cornerOverrides);
    if (newOverrideSize == oldOverrideSize + 1)
    {
        var newOverride = last(definition.cornerOverrides);
        newOverride.cornerType = definition.defaultCornerType;
        if (isButtCorner(newOverride.cornerType))
        {
            newOverride.cornerButtFlip = definition.defaultButtFlip;
        }
        definition.cornerOverrides[newOverrideSize - 1] = newOverride;
    }
    return definition;
}

function doFrame(context is Context, id is Id, definition is map)
{
    if (definition.lockProfile)
    {
        definition.profileControl = ProfileControlMode.LOCK_FACES;
        definition.lockFaces = definition.profileLockFaces;
    }

    var bodiesToDelete = new box([]);
    const profileData = getProfile(context, id, definition, bodiesToDelete);

    const sweepData = (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1742_STABLE_FRAME_SWEEP))
       ? sweepFrames(context, id, definition, profileData, bodiesToDelete)
       : sweepFrames_PRE_V1742(context, id, definition, profileData, bodiesToDelete);

    addManipulators(context, id, sweepData.manipulators);
    trimFrame(context, id, definition, sweepData.trimEnds, sweepData.sweepBodies, bodiesToDelete);
    createComposites(context, id, definition.mergeTangentSegments, sweepData.compositeGroups, profileData);
    cleanUpBodies(context, id, bodiesToDelete);
    const remainingTransform = getRemainderPatternTransform(context, { "references" : definition.selections });
    transformResultIfNecessary(context, id, remainingTransform);
}

function createComposites(context is Context, id is Id, mergeSegments is boolean, compositeGroups is array, profileData is map)
{
    if (!mergeSegments)
    {
        return;
    }

    var index = 0;
    for (var group in compositeGroups)
    {
        if (size(group.segments) < 2)
        {
            continue;
        }

        const bodyQuery = qUnion(group.segments);
        // BEL-209918 Disambiguate between frames created in the same operation to improve downstream stability-under-edit.
        var compositeId;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2123_COMPOSITE_SEGMENT_DISAMBIGUATION))
        {
            compositeId = id + "compositePart" + unstableIdComponent(index);
            setExternalDisambiguation(context, compositeId, bodyQuery);
        }
        else
        {
            compositeId = id + "compositePart" + index;
        }
        index += 1;
        opCreateCompositePart(context, compositeId, {
                "bodies" : bodyQuery,
                "closed" : true
        });

        //update composite frame segment terminus attributes
        const startFaceQuery = qFrameStartFace(group.startTerminus);
        var startFaceAttributes = getFrameTopologyAttribute(context, startFaceQuery);
        startFaceAttributes.isCompositeTerminus = true;
        setFrameTopologyAttribute(context, startFaceQuery, startFaceAttributes);

        //update end attributes
        const endFaceQuery = qFrameEndFace(group.endTerminus);
        var endFaceAttributes = getFrameTopologyAttribute(context, endFaceQuery);
        endFaceAttributes.isCompositeTerminus = true;
        setFrameTopologyAttribute(context, endFaceQuery, endFaceAttributes);

        //set composite body attribute
        const closedCompositeBodyQuery = qCreatedBy(compositeId, EntityType.BODY)->qCompositePartTypeFilter(CompositePartType.CLOSED);
        setFrameProfileAttribute(context, closedCompositeBodyQuery, profileData.profileAttribute);
    }
}

function sweepFrames(context is Context, topLevelId is Id, definition is map, profileData is map, bodiesToDelete is box) returns map
{
    verify(!isQueryEmpty(context, definition.selections), ErrorStringEnum.FRAME_SELECT_PATH, { "faultyParameters" : ["selections"] });
    const cornerOverrides = gatherCornerOverrides(context, definition.cornerOverrides);
    const selectionData = createPathsFromSelections(context, topLevelId, definition.selections, bodiesToDelete);

    const frameManipulators = createStableFrameManipulators(context, topLevelId, definition, selectionData.manipulatorEdge, profileData);
    // The points manipulator _must_ succeed because there is no mapping for the points manipulator back to any user-editable field in the UI.
    // Therefore the points manipulator *may* modify the index as required.
    definition.index = frameManipulators.points.index;
    // Immediately display the manipulators before the sweep operation is performed.
    // This guarantees the point and angle manipulator show even in the event of failure.
    // The user thus always has a way to undo the fail state.
    addManipulators(context, topLevelId, frameManipulators);
    const sweepData = doStablePaths(context, topLevelId, definition, profileData, cornerOverrides, selectionData.paths, selectionData.stableEdges, bodiesToDelete);

    return {
            "trimEnds" : sweepData.trimEnds,
            "manipulators" : mergeMaps(sweepData.manipulators, frameManipulators),
            "sweepBodies" : sweepData.sweepBodies,
            "compositeGroups" : sweepData.compositeGroups
        };
}

function createStableFrameManipulators(context is Context, topLevelId is Id, definition is map, manipulatorEdge is Query, profileData is map) returns map
{
    // Desired behavior is to set the frames manipulator at the middle of the first user-selected swept segment.
    // Due to the heuristic for creating planes the manipulators can lose correct orientation with the frame.
    // Detect this case and use the mid-segment manipulator plane if possible.
    // Otherwise use the start of the manipulator edge which is always correct.
    const manipulatorStart = evaluatePathEdge(context, manipulatorEdge, false, 0);
    const manipulatorMid = evaluatePathEdge(context, manipulatorEdge, false, 0.5);
    const startXDir = getXDirFromHeuristic(manipulatorStart);
    const midXDir = getXDirFromHeuristic(manipulatorMid);
    return tolerantEquals(startXDir, midXDir)
        ? createFrameManipulators(context, topLevelId, definition, manipulatorMid, profileData)
        : createFrameManipulators(context, topLevelId, definition, manipulatorStart, profileData);
}

function doStablePaths(context is Context, topLevelId is Id, definition is map, profileData is map, cornerOverrides is array,
    paths is array, stableEdges is array, bodiesToDelete is box) returns map
{
    const createPathId = getUnstableIncrementingId(topLevelId);

    var allManipulators = {};
    var allTrimEnds = [];
    var allSweepBodies = [];
    var allCompositeGroups = [];
    for (var pathIndex = 0; pathIndex < size(paths); pathIndex += 1)
    {
        const path = paths[pathIndex];
        const stableEdge = stableEdges[pathIndex];
        const pathData = doStablePath(context, topLevelId, createPathId(), definition, profileData, cornerOverrides, path, stableEdge, bodiesToDelete);
        // aggregate data
        allTrimEnds = concatenateArrays([allTrimEnds, pathData.trimEnds]);
        allSweepBodies = concatenateArrays([allSweepBodies, pathData.sweepBodies]);
        allManipulators = mergeMaps(allManipulators, pathData.manipulators);
        allCompositeGroups = concatenateArrays([allCompositeGroups, pathData.compositeGroups]);
    }

    return {
            "trimEnds" : allTrimEnds,
            "manipulators" : allManipulators,
            "sweepBodies" : allSweepBodies,
            "compositeGroups" : allCompositeGroups
        };
}

function doStablePath(context is Context, topLevelId is Id, pathId is Id, definition is map, profileData is map,
    cornerOverrides is array, path is Path, stableEdge is map, bodiesToDelete is box) returns map
{
    // A "stable path" is geometrically stable while the user edits the path.
    // If the user selects additional segments, the path can grow but it won't twist or change orientation.
    // This is needed to prevent BEL-179102 (downstream trim fails after editing upstream frames).
    // To achieve this, each frame is "stabilized" around the first selected edge.

    // case 1: path is closed
    // rotate path till stable edge becomes 1st element of the path then sweep as normal
    if (path.closed)
    {
        const currentIndex = stableEdge.pathIndex;
        verify(currentIndex >= 0, "Can't find manipulator index");
        path.edges = rotateArray(path.edges, -currentIndex);
        path.flipped = rotateArray(path.flipped, -currentIndex);
        return doOneStablePath(context, topLevelId, definition, pathId, profileData, cornerOverrides, path, bodiesToDelete);
    }

    // case 2: stableEdge is first segment of path
    // sweep as normal
    if (path.edges[0] == stableEdge.edge)
    {
        return doOneStablePath(context, topLevelId, definition, pathId, profileData, cornerOverrides, path, bodiesToDelete);
    }

    // case 3: stableEdge is intermediate segment along an open path
    return doSplitPath(context, topLevelId, pathId, definition, profileData, cornerOverrides, path, stableEdge, bodiesToDelete);
}

function doOneStablePath(context is Context, topLevelId is Id, definition is map, pathId is Id, profileData is map, cornerOverrides is array, path is Path, bodiesToDelete is box) returns map
{
    const createSweepId = getDisambiguatedIncrementingId(context, pathId);
    const pathData = sweepOnePath(context, topLevelId, createSweepId, definition, profileData, cornerOverrides, path, bodiesToDelete);
    setAttributesOnPath(context, profileData, pathData.sweepData);
    const createCornerId = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2149_DEPRECATE_INCREMENTING_ID_GENERATOR)
        ? getUnstableIncrementingId(pathId + "corner")
        : getIncrementingId(pathId + "corner");

    createCorners(context, topLevelId, createCornerId, pathData.sweepData, pathData.cornerData, bodiesToDelete);
    const compositeGroups = groupTangentSegments(context, pathData.sweepData, pathData.cornerData);

    return {
            "trimEnds" : [pathData.sweepData[0].startFace, last(pathData.sweepData).endFace],
            "manipulators" : pathData.manipulators,
            "sweepBodies" : getSweepBodies(pathData),
            "compositeGroups" : compositeGroups
        };
}

function groupTangentSegments(context, sweepData is array, cornerData is array) returns array
{
    const numCorners = size(cornerData);
    const numSegments = size(sweepData);
    const isClosed = (numCorners == numSegments);

    //create lookup for checking if a beam is ungrouped.
    //the value doesnt matter - only key existence is used.
    var ungroupedSegment = {};
    for (var index = 0; index < numSegments; index += 1)
    {
        ungroupedSegment[index] = true;
    }

    var allGroups = [];
    while (size(ungroupedSegment) > 0)
    {
        // this algorithm starts at and index and explores segments "before" (lower index) and "after" (higher index)
        // to find the extents of the composite segment group.
        //it also tracks the starting and ending indexes of the composite beam for later attribution.
        const startBeamIndex = getFirstKey(ungroupedSegment);
        var compositeSegments = [];
        var startTerminus;
        var endTerminus;

        //search lower index to limit
        var beamIndex = startBeamIndex;
        while (true)
        {
            ungroupedSegment[beamIndex] = undefined;
            compositeSegments = append(compositeSegments, beamIndex);

            var cornerIndex = beamIndex - 1;
            if (cornerIndex < 0)
            {
                //handles wrap case for closed beams
                if (isClosed)
                {
                    cornerIndex = numCorners - 1;
                }
                else
                {
                    break;
                }
            }

            if (cornerData[cornerIndex].cornerType == FrameCornerType.TANGENT && ungroupedSegment[cornerIndex] != undefined)
            {
                beamIndex = cornerIndex;
                continue;
            }
            else
            {
                break;
            }
        }
        startTerminus = beamIndex;

        //search upper indexes
        beamIndex = startBeamIndex;
        while (true)
        {
            var cornerIndex = beamIndex;
            // the closed path wraparound is handled in the lower index loop so if now encountered just exit
            if (cornerIndex == numCorners)
            {
                break;
            }

            if (cornerData[cornerIndex].cornerType != FrameCornerType.TANGENT)
            {
                //not a tangent corner - end of composite segment
                break;
            }

            beamIndex = (beamIndex + 1) % numSegments;
            if (ungroupedSegment[beamIndex] != undefined)
            {
                //add to current composite group
                compositeSegments = append(compositeSegments, beamIndex);
                ungroupedSegment[beamIndex] = undefined;
                continue;
            }
            else
            {
                //already a part of this composite segment (in event of closed path)
                break;
            }
        }
        endTerminus = beamIndex;

        const compositeBodies = mapArray(compositeSegments, function(index) { return sweepData[index].body; });
        const currentGroup = {
                "segments" : compositeBodies,
                "startTerminus" : sweepData[startTerminus].body,
                "endTerminus" : sweepData[endTerminus].body
            };
        allGroups = append(allGroups, currentGroup);
    }
    return allGroups;
}

// This function retrieves the first available key in a map.
function getFirstKey(keys is map)
{
    for (var k, _ in keys)
    {
        return k;
    }
    return undefined;
}

function doSplitPath(context is Context, topLevelId is Id, pathId is Id, definition is map, profileData is map, cornerOverrides is array, path is Path, stableEdge is map, bodiesToDelete is box) returns map
{
    // 1. The path is split in to a frontPath and a backPath, both starting at the stable edge start.
    // frontPath is subpath [stableEdge.pathIndex, last segment of path)
    // backPath is subpath [0, stableEdge.pathIndex)
    // 2. sweep frontPath as normal
    // 3. sweep backPath in reverse from first face of frontPath
    // 4. fix up attribution so frame appears as if it was a swept from start to finish segment.
    const splitPaths = splitPathAtEdge(path, stableEdge);
    const createSweepId = getDisambiguatedIncrementingId(context, pathId);

    const frontPathData = sweepOnePath(context, topLevelId, createSweepId, definition, profileData, cornerOverrides, splitPaths.frontPath, bodiesToDelete);

    for (var sweepData in frontPathData.sweepData)
    {
        const frameData = getFrameData(context, sweepData.id, sweepData);
        setFrameAttributes(context, qCreatedBy(sweepData.id, EntityType.BODY), profileData, frameData);
    }

    // Backpath sweep is complicated.
    // The backpath is swept from some intermediate segment "backward" to its end.
    // This then requires some manual reorganization of the attribution to make the beam appear to be swept in one continuous sequence.
    const backPath = splitPaths.backPath;
    // the backPath needs a starting face and direction
    const previousFace = frontPathData.sweepData[0].startFace;
    var previousEdgeEnd = evaluatePathEdge(context, stableEdge.edge, false, 0);
    previousEdgeEnd.direction = -previousEdgeEnd.direction;
    var backPathSweepData = sweepContinuingPath(context, definition, createSweepId, backPath.edges, backPath.flipped, previousEdgeEnd, previousFace, cornerOverrides, profileData, bodiesToDelete);
    // because the backPath was swept backward, swap the attribution on the ends.
    for (var sweepData in backPathSweepData.sweepData)
    {
        var frameData = getFrameData(context, sweepData.id, sweepData);
        const temp = frameData.endFace;
        frameData.endFace = frameData.startFace;
        frameData.startFace = temp;
        setFrameAttributes(context, qCreatedBy(sweepData.id, EntityType.BODY), profileData, frameData);
    }

    // the last face of the last element of the backpath is the 'start' of the frame
    setFrameTerminusAttributes(context, last(backPathSweepData.sweepData).endFace, last(frontPathData.sweepData).endFace);

    // the createCorners function requires the 'previous face' so insert it at the front of the array here
    const tempBackSweepData = concatenateArrays([
                [{ "endFace" : frontPathData.sweepData[0].startFace }],
                backPathSweepData.sweepData]);

    const createCornerId = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2149_DEPRECATE_INCREMENTING_ID_GENERATOR)
        ? getUnstableIncrementingId(pathId + "corner")
        : getIncrementingId(pathId + "corner");

    createCorners(context, topLevelId, createCornerId, frontPathData.sweepData, frontPathData.cornerData, bodiesToDelete);
    createCorners(context, topLevelId, createCornerId, tempBackSweepData, backPathSweepData.cornerData, bodiesToDelete);
    // to create composite groups, the cornerData and sweepPath data need to be ordered correctly
    const totalPathSweepData = concatenateArrays([reverse(backPathSweepData.sweepData), frontPathData.sweepData]);
    const totalPathCornerData = concatenateArrays([reverse(backPathSweepData.cornerData), frontPathData.cornerData]);
    const compositeGroups = groupTangentSegments(context, totalPathSweepData, totalPathCornerData);

    // Versioned bugfix: Selecting wrong trim ends during frame trims
    const trimEnds = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1770_TRIM_END_FIX)
        ? [last(backPathSweepData.sweepData).endFace, last(frontPathData.sweepData).endFace]
        : [previousFace, last(frontPathData.sweepData).endFace];

    return {
            "trimEnds" : trimEnds,
            "manipulators" : mergeMaps(frontPathData.manipulators, backPathSweepData.manipulators),
            "sweepBodies" : concatenateArrays([getSweepBodies(frontPathData), backPathSweepData.sweepBodies]),
            "compositeGroups" : compositeGroups
        };
}

function sweepOnePath(context is Context, topLevelId is Id, createSweepId is function, definition is map, profileData is map, cornerOverrides is array, path is Path, bodiesToDelete is box) returns map
{
    // sweep start edge
    var sweepData = [];
    const startEdgeSweepData = sweepStartingEdge(context, definition, createSweepId(path.edges[0]), path, profileData, bodiesToDelete);
    sweepData = append(sweepData, startEdgeSweepData);
    const previousEdgeEnd = evaluatePathEdge(context, path.edges[0], path.flipped[0], 1);
    const previousFace = startEdgeSweepData.endFace;

    // sweep continuing edges
    const continuingEdges = subArray(path.edges, 1, size(path.edges));
    const continuingFlips = subArray(path.flipped, 1, size(path.flipped));
    const continuingData = sweepContinuingPath(context, definition, createSweepId, continuingEdges, continuingFlips, previousEdgeEnd, previousFace, cornerOverrides, profileData, bodiesToDelete);
    sweepData = concatenateArrays([sweepData, continuingData.sweepData]);
    var manipulators = continuingData.manipulators;
    var cornerData = continuingData.cornerData;

    if (path.closed)
    {
        const closedLoopCornerData = handleCornerForClosedLoop(context, definition, cornerOverrides, sweepData, path, size(path.edges) - 1);
        manipulators = updateManipulators(manipulators, closedLoopCornerData.manipulator);
        cornerData = updateCornerData(cornerData, closedLoopCornerData);
    }

    return {
            "sweepData" : sweepData,
            "cornerData" : cornerData,
            "manipulators" : manipulators
        };
}

function sweepContinuingPath(context is Context, definition is map, createSweepId is function, edges is array, flips is array,
    previousEdgeEnd is Line, previousFace is Query, cornerOverrides is array, profileData is map, bodiesToDelete is box) returns map
{
    verify(size(edges) == size(flips), "Bad path data");

    var sweepData = [];
    var cornerData = [];
    var manipulators = {};
    var sweepBodies = [];

    for (var index = 0; index < size(edges); index += 1)
    {
        const edge = edges[index];
        const flipped = flips[index];
        const edgeStart = evaluatePathEdge(context, edge, flipped, 0);
        const currentEdgeData = sweepContinuingEdge(context, definition, createSweepId(edge), edge, edgeStart, previousEdgeEnd, previousFace, cornerOverrides, profileData, bodiesToDelete);
        previousEdgeEnd = evaluatePathEdge(context, edge, flipped, 1);
        previousFace = currentEdgeData.sweepData.endFace;
        // aggregation
        sweepData = append(sweepData, currentEdgeData.sweepData);
        cornerData = append(cornerData, currentEdgeData.cornerData);
        manipulators = updateManipulators(manipulators, currentEdgeData.cornerData.manipulator);
        sweepBodies = append(sweepBodies, currentEdgeData.sweepData.body);
    }

    return {
            "sweepData" : sweepData,
            "cornerData" : cornerData,
            "manipulators" : manipulators,
            "sweepBodies" : sweepBodies
        };
}


function sweepStartingEdge(context is Context, definition is map, edgeId is Id, edgePath is Path, profileData is map, bodiesToDelete is box) returns map
{
    const edge = edgePath.edges[0]; // by definition the starting edge has index 0
    const edgeLine = evaluatePathEdge(context, edge, edgePath.flipped[0], 0);
    const planeAtEdgeStart = getPlaneAtLineStart(context, definition, edgeLine);
    const profilePlane = getProfilePlane(profileData, definition);
    const mirrorAndAngleTransform = getMirrorAndAngleTransform(definition.mirrorProfile, edgeLine, planeAtEdgeStart, definition.angle);
    const profileTransform = mirrorAndAngleTransform * transform(profilePlane, planeAtEdgeStart);

    const profileId = edgeId + "profile";
    opPattern(context, profileId, {
                "entities" : profileData.profileBody,
                "transforms" : [profileTransform],
                "instanceNames" : ["1"]
            });

    cleanUpAtEndOfFeature(bodiesToDelete, qCreatedBy(profileId, EntityType.BODY));
    const sweepId = edgeId + "sweep";
    const sweepData = sweepOneEdge(context, definition, sweepId, qCreatedBy(profileId, EntityType.FACE), edge);
    return sweepData;
}

function sweepContinuingEdge(context, definition, edgeId, edge is Query, edgeStart is Line, previousEdgeEnd is Line, previousFace, cornerOverrides is array, profileData is map, bodiesToDelete is box)
{
    opExtractSurface(context, edgeId + "extract", { "faces" : previousFace });
    const extractedBody = qCreatedBy(edgeId + "extract", EntityType.BODY);
    cleanUpAtEndOfFeature(bodiesToDelete, extractedBody);
    const sweepProfileTransform = transform(line(previousEdgeEnd.origin, previousEdgeEnd.direction), line(edgeStart.origin,
            edgeStart.direction));

    const profileId = edgeId + "profile";
    opTransform(context, profileId, {
                "bodies" : extractedBody,
                "transform" : sweepProfileTransform
            });
    cleanUpAtEndOfFeature(bodiesToDelete, qCreatedBy(profileId, EntityType.BODY));
    const faceToSweep = qOwnedByBody(extractedBody, EntityType.FACE);
    const cornerData = getCurrentCornerData(context, previousEdgeEnd, edgeStart, cornerOverrides, definition, faceToSweep, previousFace);
    const sweepId = edgeId + "sweep";
    const sweepData = sweepOneEdge(context, definition, sweepId, faceToSweep, edge);
    return {
            "sweepData" : sweepData,
            "cornerData" : cornerData
        };
}

function splitPathAtEdge(path is Path, stableEdge is map) returns map
{
    // split the path path in to two paths.
    // This creates two paths that both start at the start point of the path edge.
    // backPath: path.edges[0, splitIndex)
    // frontPath: path.edges[splitIndex, N)
    // backPath is then reversed.
    const pathLength = size(path.edges);
    verify(pathLength > 0, "Can't split a zero length path");
    verify(!path.closed, "Can't split a closed path");
    const splitIndex = stableEdge.pathIndex;
    var backPath = {
            "edges" : subArray(path.edges, 0, splitIndex),
            "flipped" : subArray(path.flipped, 0, splitIndex),
            "closed" : false
        } as Path;
    backPath = reverse(backPath);

    const frontPath = {
                "edges" : subArray(path.edges, splitIndex, pathLength),
                "flipped" : subArray(path.flipped, splitIndex, pathLength),
                "closed" : false
            } as Path;

    return {
            "frontPath" : frontPath,
            "backPath" : backPath
        };
}

function createFrameManipulators(context is Context, topLevelId is Id, definition is map, manipulatorLine is Line, profileData is map) returns map
{
    var manipulatorPlane = getPlaneAtLineStart(context, definition, manipulatorLine);

    // The manipulator plane "x" is the base of the manipulator widget so we dont want to "rotate" the manipulator plane.
    // We also want to keep the "y" axis in the same direction after the flip so that the "drag" behaves properly.
    // To achieve both requirements with a mirrored profile we reverse both the X and Z axis.
    if (definition.mirrorProfile)
    {
        manipulatorPlane = plane(manipulatorPlane.origin, -manipulatorPlane.normal, -manipulatorPlane.x);
    }

    const angleManipulator = angularManipulator({
                "primaryParameterId" : "angle",
                "axisOrigin" : manipulatorPlane.origin,
                "axisDirection" : manipulatorPlane.normal,
                "angle" : definition.angle,
                "minValue" : 0 * degree,
                "maxValue" : 360 * degree,
                "rotationOrigin" : manipulatorPlane.origin + manipulatorPlane.x * profileData.pointsManipulatorData.halfExtents[0] * 2
            });

    const pointsManipulator = createPointsManipulator(context, topLevelId, definition.index, profileData, manipulatorPlane, definition.angle);

    return { "angleManipulator" : angleManipulator, "points" : pointsManipulator };
}

function getProfilePlane(profileData is map, definition is map) returns Plane
{
    // We create the profile plane centered at the center communicated from the point data, rather than the profilePlane's default center.
    const profilePlaneOrigin = profileData.pointsManipulatorData.center + profileData.pointsManipulatorData.offset[definition.index];
    const centeredProfilePlaneInWorld = plane(planeToWorld(profileData.profilePlane, vector(profilePlaneOrigin[0], profilePlaneOrigin[1])), profileData.profilePlane.normal, profileData.profilePlane.x);
    return centeredProfilePlaneInWorld;
}

function evaluatePathEdge(context is Context, edge is Query, isFlipped is boolean, parameter is number) returns Line
{
    parameter = isFlipped ? 1 - parameter : parameter;
    verify(parameter >= 0 && parameter <= 1, ErrorStringEnum.FRAME_BAD_PATH);

    var edgeLine;
    try
    {
        edgeLine = evEdgeTangentLine(context, {
                    "edge" : edge,
                    "parameter" : parameter
                });
        edgeLine.direction = isFlipped ? -edgeLine.direction : edgeLine.direction;
    }
    catch
    {
        throw regenError(ErrorStringEnum.FRAME_BAD_PATH, edge);
    }
    return edgeLine;
}


function evaluatePathEdgeFromIndex(context is Context, path is Path, edgeIndex is number, parameter is number) returns Line
{
    const edge = path.edges[edgeIndex];
    const isFlipped = path.flipped[edgeIndex];
    return evaluatePathEdge(context, edge, isFlipped, parameter);
}


function getMirrorAndAngleTransform(mirrorProfile is boolean, edgeLine is Line, planeAtEdgeStart is Plane, angle is ValueWithUnits) returns Transform
{
    if (mirrorProfile)
    {
        // the desired behavior is to mirror the profile sketch across its Y-axis.
        // But to keep the "twist" direction correct, we also negate the angle.
        const mirrorPlane = plane(edgeLine.origin, planeAtEdgeStart.x);
        return rotationAround(edgeLine, -angle) * mirrorAcross(mirrorPlane);
    }
    else
    {
        return rotationAround(edgeLine, angle);
    }
}

function sweepOneEdge(context is Context, definition is map, sweepId is Id, face is Query, edge is Query) returns map
{
    try silent
    {
        opSweep(context, sweepId, { "profiles" : face, "path" : edge, "profileControl" : definition.profileControl, "lockFaces" : definition.lockFaces });
    }
    catch
    {
        throw regenError(ErrorStringEnum.FRAME_SWEEP_FAILED, edge);
    }

    const startFaceQuery = qCapEntity(sweepId, CapType.START, EntityType.FACE);
    const endFaceQuery = qCapEntity(sweepId, CapType.END, EntityType.FACE);
    verify(!isQueryEmpty(context, startFaceQuery) && !isQueryEmpty(context, endFaceQuery),
        ErrorStringEnum.FRAME_CANDIDATE_FACES, { "entities" : edge });

    const sweepData = {
            "id" : sweepId,
            "startFace" : startFaceQuery,
            "endFace" : endFaceQuery,
            "body" : qCreatedBy(sweepId, EntityType.BODY)
        };

    return sweepData;
}

function handleCornerForClosedLoop(context is Context, definition is map, cornerOverrides is array, sweepData is array,
    edgePath is Path, edgeIndex is number) returns map
{
    if (edgePath.closed && size(edgePath.edges) - 1 == edgeIndex)
    {
        return createAdditionalCornerForClosedLoop(context, definition, cornerOverrides, sweepData, edgePath, edgeIndex);
    }
    else
    {
        return {};
    }
}

function createAdditionalCornerForClosedLoop(context is Context, definition is map, cornerOverrides is array,
    sweepData is array, edgePath is Path, edgeIndex is number) returns map
{
    const prevEdgeEnd = evaluatePathEdgeFromIndex(context, edgePath, edgeIndex, 1);
    const lineAtStart = evaluatePathEdgeFromIndex(context, edgePath, 0, 0);
    return getCurrentCornerData(context, prevEdgeEnd, lineAtStart, cornerOverrides, definition, sweepData[0].startFace, sweepData[edgeIndex].endFace);
}


function updateManipulators(manipulators is map, currentManipulators) returns map
{
    if (currentManipulators != undefined)
    {
        return mergeMaps(manipulators, currentManipulators);
    }
    else
    {
        return manipulators;
    }
}

function updateCornerData(cornerData is array, currentCornerData is map) returns array
{
    if (currentCornerData != {})
    {
        return append(cornerData, currentCornerData);
    }
    else
    {
        return cornerData;
    }
}

// Bodies added to the `bodiesToDelete` box will be deleted at the very end of the frame feature
function cleanUpAtEndOfFeature(bodiesToDelete is box, bodies is Query)
{
    bodiesToDelete[] = append(bodiesToDelete[], bodies);
}

// Called at end of feature to clean up the collected helper bodies
function cleanUpBodies(context is Context, topLevelId is Id, bodiesToDelete is box)
{
    if (bodiesToDelete[] != [])
    {
        opDeleteBodies(context, topLevelId + "cleanup", { "entities" : qUnion(bodiesToDelete[]) });
    }
}

function trimFrame(context is Context, topLevelId is Id, definition is map, trimEnds is array, sweepBodies is array,
    bodiesToDelete is box)
{
    if (definition.trim)
    {
        const trimData = preprocessForTrim(context, topLevelId, definition, trimEnds, sweepBodies);
        extendFrames(context, topLevelId, trimData);
        trimFramesByPlanes(context, topLevelId, trimData.capFaceToTrimPlane);
        trimFramesByBodies(context, topLevelId, trimData.frameToTrimFrameData, bodiesToDelete);
    }
}

function preprocessForTrim(context is Context, id is Id, definition is map, trimEnds is array, sweepBodies is array) returns map
{
    const collisions = getTrimCandidates(context, definition.trimPlanes, definition.trimBodies, trimEnds, sweepBodies);
    const collisionData = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2057_FRAME_TRIM_GROUP_BY_TRANSIENT_QUERY)
    ? groupCollisionResults(context, collisions)
    : groupCollisionResults_PRE_2057(context, collisions);

    var capFaceToToolBodies = collisionData.capFaceToToolBodies;
    const framesToBodies = collisionData.framesToBodies;

    var capFaceToTrimPlane = {};
    var frameToTrimFrameData = {};
    for (var entry in framesToBodies)
    {
        const target = entry.key;
        const tools = entry.value;
        // get trimmable ends of frames, where a cap face is "trimmable" if it is an open end of a frame (not at a corner)
        const capFaces = getTrimmableCapFaces(context, target, trimEnds);
        const toolsPerSide = groupToolsPerSide(context, capFaces, tools);

        // process plane selections
        for (var side in ["start", "end"])
        {
            if (isQueryEmpty(context, toolsPerSide[side]))
            {
                continue;
            }

            var trimPlanesQ = qIntersection([qOwnedByBody(toolsPerSide[side], EntityType.FACE), definition.trimPlanes]);
            var trimPlanes = evaluateQuery(context, trimPlanesQ);
            if (trimPlanes == [])
            {
                continue;
            }

            if (foundMultiplePlanes(context, trimPlanes))
            {
                setErrorEntities(context, id, { "entities" : qOwnerBody(target) });
                reportFeatureWarning(context, id, ErrorStringEnum.FRAME_MULTIPLE_TRIM_PLANES);
                capFaceToToolBodies[capFaces[side]] = undefined; // skip extending
            }
            else
            {
                const trackedCapFace = qUnion([capFaces[side], startTracking(context, capFaces[side])]);
                capFaceToTrimPlane[trackedCapFace] = trimPlanes[0];
            }
        }

        // The "target" body queries are dependent on cap faces which may get replaced as part of the plane trim.
        // Operations ahead of a boolean trim would not invalidate a transient bodyId (face extension uses opMoveFace, and trimToPlane uses opReplaceFace)
        // but the most robust solution is a robust query.
        const robustTarget = makeRobustQuery(context, target);
        frameToTrimFrameData[robustTarget] = {
                "startFrames" : qIntersection([toolsPerSide.start, definition.trimBodies]),
                "endFrames" : qIntersection([toolsPerSide.end, definition.trimBodies])
            };
    }

    return {
            "capFaceToTrimPlane" : capFaceToTrimPlane,
            "frameToTrimFrameData" : frameToTrimFrameData,
            "capFaceToToolBodies" : capFaceToToolBodies
        };
}

function groupCollisionResults(context is Context, collisions is array) returns map
{
    var capFaceToToolBodies = {};
    var framesToBodies = {};

    for (var collision in collisions)
    {
        // if capFaces are trimmed, we will extend their beam end to improve trimming
        if (isFrameCapFace(context, collision.target))
        {
            capFaceToToolBodies = insertIntoMapOfArrays(capFaceToToolBodies, collision.target, collision.toolBody);
        }

        // Collect only collisions with overlapping volume and ignore adjacency or containment collisions
        if (collision["type"] == ClashType.INTERFERE)
        {
            // Many of these collision records are between frame segment swept faces and tool bodies (and faces)
            // Downstream trim processing only requires a map between frame segment bodies and colliding tools
            // Evaluating the query to get a transient query to the frame segment substantially reduces the number of
            // collision pairs to be subsequently sorted.
            const frameSegmentBodyQuery = qOwnerBody(collision.targetBody);
            const frameSegmentTransientQueries = evaluateQuery(context, frameSegmentBodyQuery);
            if (frameSegmentTransientQueries != [])
            {
                const frameSegmentTransientQuery = frameSegmentTransientQueries[0];
                framesToBodies = insertIntoMapOfArrays(framesToBodies, frameSegmentTransientQuery, collision.toolBody);
            }
        }
    }

    return
    {
            "capFaceToToolBodies" : capFaceToToolBodies,
            "framesToBodies" : framesToBodies
        };
}

function trimFramesByPlanes(context is Context, topLevelId is Id, capFaceToTrimPlane is map)
{
    const trimId = getUnstableIncrementingId(topLevelId + "planeTrim");
    for (var entry in capFaceToTrimPlane)
    {
        const trimPlane = evPlane(context, { "face" : entry.value });
        const capFacePlane = evPlane(context, { "face" : entry.key });

         // Using replace face instead of split keeps startFace/endFace attributes intact which are needed for cutlist
        try silent
        {
            opReplaceFace(context, trimId(), {
                        "replaceFaces" : entry.key,
                        "templateFace" : entry.value,
                        "oppositeSense" : dot(trimPlane.normal, capFacePlane.normal) < -TOLERANCE.zeroLength
                    });
        }
        catch
        {
            throw regenError(ErrorStringEnum.FRAME_TRIM_FAILED, qOwnerBody(entry.key));
        }
    }
}

function trimFramesByBodies(context is Context, topLevelId is Id, frameToTrimData is map, bodiesToDelete is box)
{
    // A note on the trim and tracking logic.
    // A boolean subtraction operation may produce multiple bodies.
    // Parasolid arbitrarily calls one of these bodies the 'old body'. The 'new' bodies receive new ids.
    // If there are attributes on the 'old body' they are preserved, but lost on any 'new bodies'.
    // so we can not use the original target as the body to preserve.
    // The strategy we employ is:
    // get the attributes we will lose during the trim
    // track the beam end that is not being trimmed
    // perform the trim
    // find the body that owns the tracked preserved face
    // delete the other bodies
    // reapply the trimmed attributes to any newly created faces.
    // our trims may change the beam detid so we have to pass back the 'new' target.
    const trimId = getUnstableIncrementingId(topLevelId + "trimFrame");
    for (var entry in frameToTrimData)
    {
        // BEL-172580: Ended trims fail for circular tools
        const trimData = entry.value;
        var target = entry.key;
        // trim the "start" of the beam
        target = doOneEndTrim(context, topLevelId, trimId(), target, trimData.startFrames, true, bodiesToDelete);
        // trim the "end" of the beam
        doOneEndTrim(context, topLevelId, trimId(), target, trimData.endFrames, false, bodiesToDelete);
    }
}

function doOneEndTrim(context is Context, topLevelId is Id, trimId is Id, target is Query, tools is Query,
    isStart is boolean, bodiesToDelete is box) returns Query
{
    // NB: In typical operation, the target will resolve to at least one body so cutFaceQuery will resolve.
    // The `extendFrames` and `trimByPlanes` actions both precede `doOneEndTrim` and may affect cap faces
    // but they use attribute-preserving functions. The `target` frame segment here is a robustQuery to the body itself
    // and should be unaffected by intermediate modifications.

    if (isQueryEmpty(context, tools) || isQueryEmpty(context, target))
    {
        // nothing to trim - return original body
        return target;
    }

    // qUnion handles cases where trimmed end is not entirely removed (coped joints, partial trim, etc)
    const trackedTarget = qUnion(target, startTracking(context, target));

    var keptFaceQuery;
    var cutFaceQuery;
    if (isStart)
    {
        cutFaceQuery = qFrameStartFace(target);
        keptFaceQuery = qFrameEndFace(target);
    }
    else
    {
        cutFaceQuery = qFrameEndFace(target);
        keptFaceQuery = qFrameStartFace(target);
    }

    // body will not survive trim operation so we evaluate the keptFaceQuery now
    const attributesToReapply = getFrameTopologyAttribute(context, cutFaceQuery);
    const keptFace = evaluateQuery(context, keptFaceQuery);

    verify(keptFace != [], ErrorStringEnum.FRAME_TRIM_FAILED, { "entities" : target });
    doTrim(context, topLevelId, trimId, target, tools);
    const keptBody = qOwnerBody(keptFace[0]);
    const leftOverBodies = evaluateQuery(context, qSubtraction(trackedTarget, keptBody));
    cleanUpAtEndOfFeature(bodiesToDelete, qUnion(leftOverBodies));
    reapplyAttributes(context, keptBody, attributesToReapply);
    return keptBody;
}

function doTrim(context is Context, topLevelId is Id, trimId is Id, target is Query, tools is Query)
{
    callSubfeatureAndProcessStatus(topLevelId, opBoolean, context, trimId, {
                "tools" : tools,
                "targets" : target,
                "keepTools" : true,
                "operationType" : BooleanOperationType.SUBTRACTION
            }, {
                "overrideStatus" : ErrorStringEnum.FRAME_TRIM_FAILED,
                "propagateErrorDisplay" : true,
                "additionalErrorEntities" : qUnion([tools, target])
            });
}

function reapplyAttributes(context is Context, bodyToPreserve is Query, attributesToReapply is FrameTopologyAttribute)
{
    // only reapply attributes to newly created faces if the trim operation removed ALL the original start or end faces
    const oldStartFaces = qFrameStartFace(bodyToPreserve);
    const oldEndFaces = qFrameEndFace(bodyToPreserve);
    if (isQueryEmpty(context, oldStartFaces) || isQueryEmpty(context, oldEndFaces))
    {
        const facesWithAttributes = qFrameAllFaces(bodyToPreserve);
        const allFaces = qOwnedByBody(bodyToPreserve, EntityType.FACE);
        const newFaces = qSubtraction(allFaces, facesWithAttributes);
        setFrameTopologyAttribute(context, newFaces, attributesToReapply);
    }
}

function computeCollisionsAgainstTools(context is Context, trimPlanes is Query, trimBodies is Query, targets is Query) returns array
{
    // Treat planes as infinite planes.
    const geometryPlanesQuery = qConstructionFilter(trimPlanes, ConstructionObject.YES);
    const geometryCollisions = getGeometryCollisions(context, geometryPlanesQuery, targets);

    // Treat faces as infinite planes first. If no collision is detected, trim to face's owner body.
    // This is useful for handling the extension case.
    const otherPlanesQuery = qSubtraction(trimPlanes, geometryPlanesQuery);
    const planarFaceCollisions = getPlanarFaceCollisions(context, otherPlanesQuery, targets);

    const topologyCollisions = getTopologyCollisions(context, trimBodies, targets);

    const collisions = concatenateArrays([topologyCollisions, planarFaceCollisions, geometryCollisions]);
    return collisions;
}

function getTopologyCollisions(context is Context, tools is Query, targets is Query) returns array
{
    var collisions = [];
    // tools or targets might be empty
    try silent
    {
        collisions = evCollision(context, {
                    "tools" : tools,
                    "targets" : targets
                });
    }

    const outputCollisions = mapArray(collisions, function(collision)
        {
            return {
                    "type" : collision["type"],
                    "target" : collision.target,
                    "targetBody" : collision.targetBody,
                    "toolBody" : collision.toolBody
                };
        });
    return outputCollisions;
}

function getPlanarFaceCollisions(context is Context, faceToolQuery is Query, targets is Query) returns array
{
    const faces = evaluateQuery(context, faceToolQuery);
    var faceGeometryCollisions = [];
    var faceOwnerBodies = [];
    for (var face in faces)
    {
        const collision = getOneGeometryCollision(context, face, targets);
        if (collision == [])
        {
            faceOwnerBodies = append(faceOwnerBodies, qOwnerBody(face));
        }
        faceGeometryCollisions = concatenateArrays([faceGeometryCollisions, collision]);
    }

    const topologyCollisions = getTopologyCollisions(context, qUnion(faceOwnerBodies), targets);
    const planarFaceCollisions = concatenateArrays([faceGeometryCollisions, topologyCollisions]);
    return planarFaceCollisions;
}

function getGeometryCollisions(context is Context, planeTools is Query, targets is Query) returns array
{
    const planes = evaluateQuery(context, planeTools);
    var collisions = [];
    for (var planeQuery in planes)
    {
        const collision = getOneGeometryCollision(context, planeQuery, targets);
        collisions = concatenateArrays([collisions, collision]);
    }
    return collisions;
}

function getOneGeometryCollision(context is Context, planeQuery is Query, targets is Query) returns array
{
    const splitPlane = evPlane(context, { "face" : planeQuery });
    const intersectingFacesQuery = qOwnedByBody(targets, EntityType.FACE)->qIntersectsPlane(splitPlane);
    const intersectingFaces = evaluateQuery(context, intersectingFacesQuery);

    var collisions = [];
    for (var intersectingFace in intersectingFaces)
    {
        const collision = {
                "type" : ClashType.INTERFERE,
                "target" : intersectingFace,
                "targetBody" : qOwnerBody(intersectingFace),
                "toolBody" : qOwnerBody(planeQuery)
            };
        collisions = append(collisions, collision);
    }
    return collisions;
}

function groupEntitiesBySide(context is Context, capFaces is map, toolBodiesQuery is Query) returns map
{
    var startEntities = [];
    var endEntities = [];
    const entitiesToGroup = evaluateQuery(context, toolBodiesQuery);

    for (var ent in entitiesToGroup)
    {
        var distStart = evDistance(context, {
                "side0" : capFaces.start,
                "side1" : ent
            });
        var distEnd = evDistance(context, {
                "side0" : capFaces.end,
                "side1" : ent
            });

        if (distStart.distance < distEnd.distance)
        {
            startEntities = append(startEntities, ent);
        }
        else
        {
            endEntities = append(endEntities, ent);
        }
    }

    return { "start" : qUnion(startEntities), "end" : qUnion(endEntities) };
}

function getTrimmableCapFaces(context is Context, frame is Query, trimEnds is array) returns map
{
    const startQ = qFrameStartFace(frame);
    const endQ = qFrameEndFace(frame);
    const start = evaluateQuery(context, qIntersection([startQ, qUnion(trimEnds)]));
    const end = evaluateQuery(context, qIntersection([endQ, qUnion(trimEnds)]));

    verify(size(start) <= 1 && size(end) <= 1, ErrorStringEnum.FRAME_MALFORMED_SEGMENT, { "entities" : frame });
    verify(size(start) > 0 || size(end) > 0, ErrorStringEnum.FRAME_TRIM_FAILED, { "entities" : frame });

    return { "start" : start == [] ? undefined : start[0],
            "end" : end == [] ? undefined : end[0] };
}

// extend frames for better booleans/trims
function extendFrames(context is Context, id is Id, trimData is map)
{
    const capFaceToToolBodies = trimData.capFaceToToolBodies;

    /**
     * Boolean operations can be finicky if bodies just barely overlap. To make the behavior more stable
     * we add a padding to the offset we apply to the cap face. This is okay to do when it's followed by
     * a replace face or a boolean as that face gets updated with the correct geometry. However, if
     * there's no trimming to planes or bodies, the padding doesn't do anything useful and it gives
     * incorrect frame length (BEL-209139).
     * Now we check if there will be a trim before we add padding.
     */
    const skipPadding = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2124_FRAME_PADDING_FIX) &&
        (trimData.capFaceToTrimPlane == {} && trimData.frameToTrimFrameData == {});
    const offsetId = getUnstableIncrementingId(id + "extendFrame");
    for (var entry in capFaceToToolBodies)
    {
        var toolsQ = qUnion(entry.value);
        const faceBox = evBox3d(context, {
                    "topology" : toolsQ,
                    "cSys" : coordSystem(evPlane(context, { "face" : entry.key }))
                });

        const trackedFace = startTracking(context, entry.key);
        try silent
        {
            opOffsetFace(context, offsetId(), {
                        "moveFaces" : entry.key,
                        "offsetDistance" : faceBox.maxCorner[2] + (skipPadding ? 0 * meter : EXTEND_FRAMES_PAD_LENGTH)
                    });
        }
        catch
        {
            setErrorEntities(context, id, { "entities" : qOwnerBody(trackedFace) });
            throw regenError(ErrorStringEnum.FRAME_TRIM_FAILED);
        }
    }
}

function foundMultiplePlanes(context is Context, toolPlanes is array) returns boolean
{
    const inputPlane = evPlane(context, { "face" : toolPlanes[0] });
    if (size(toolPlanes) > 1)
    {
        // it's okay if they're all coplanar, if not, skip processing this frame and give warning
        for (var i = 1; i < size(toolPlanes); i += 1)
        {
            const otherPlane = evPlane(context, { "face" : toolPlanes[i] });
            if (!coplanarPlanes(inputPlane, otherPlane))
            {
                return true;
            }
        }
    }
    return false;
}

function gatherCornerOverrides(context is Context, definitionOverrides is array) returns array
{
    var cornerOverrides = [];
    // gather positions for vertex overrides
    for (var i = 0; i < size(definitionOverrides); i += 1)
    {
        const corner = definitionOverrides[i];
        const position = evVertexPoint(context, { "vertex" : corner.vertex });
        cornerOverrides = append(cornerOverrides, mergeMaps(corner, { "position" : position }));
    }
    return cornerOverrides;
}

function getCurrentCornerData(context is Context, prevEdgeEnd is map, edgeStart is map, cornerOverrides is array,
    definition is map, face is Query, prevFace is Query) returns map
{
    const position = prevEdgeEnd.origin;
    const cornerOverride = cornerOverrideFound(position, cornerOverrides);
    var cornerType = cornerOverride.found ? cornerOverride.cornerType : definition.defaultCornerType;
    const angle = angleBetween(prevEdgeEnd.direction, edgeStart.direction);

    if (abs(sin(angle)) <= TOLERANCE.zeroLength)
    {
        cornerType = FrameCornerType.TANGENT;
    }

    if (isButtCorner(cornerType))
    {
        return getButtCornerData(context, definition, prevEdgeEnd, edgeStart, cornerOverride, face, prevFace, cornerType);
    }
    else if (cornerType == FrameCornerType.MITER)
    {
        return getMiterCornerData(prevEdgeEnd, edgeStart, position);
    }
    else if (cornerType == FrameCornerType.NONE || cornerType == FrameCornerType.TANGENT)
    {
        return { "cornerType" : cornerType };
    }
    else
    {
        throw regenError(ErrorStringEnum.FRAME_BAD_CORNER_TYPE);
    }
}

function getButtPlaneOffsets(faceBox is Box3d, previousFaceBox is Box3d, isCoped is boolean, isFlipped is boolean) returns map
{
    var offset = {};
    if (isCoped)
    {
        // for coped, we extend both prev and next beams to their max extents and then trim later
        offset.distance = faceBox.maxCorner[0];
        offset.prevDistance = previousFaceBox.maxCorner[0];
    }
    else if (isFlipped)
    {
        // by convention, a corner flip means "extend the next beam to the min extent of the previous face"
        // In a butt joint, the "previous" beam end face will butt up against the side of the "next" beam
        offset.distance = faceBox.minCorner[0];
        offset.prevDistance = previousFaceBox.maxCorner[0];
    }
    else
    {
        // by convention, no corner flip means "extend the previous beam to the max extent of the next face"
        // In a butt joint, the "next" beam start face will butt up against the side of the "previous" beam
        offset.distance = faceBox.maxCorner[0];
        offset.prevDistance = previousFaceBox.minCorner[0];
    }
    return offset;
}

function flipPlaneIfNeeded(plane1 is Plane, plane2 is Plane) returns Plane
{
    if (dot(plane1.normal, plane2.normal) < -TOLERANCE.zeroLength)
    {
        plane2.normal *= -1;
    }
    return plane2;
}

function createCorners(context is Context, topLevelId is Id, createCornerId is function, sweepData is array, cornerData is array, bodiesToDelete is box)
{
    for (var i = 0; i < size(cornerData); i += 1)
    {
        const corner = cornerData[i];
        const cornerId = createCornerId();
        const face = sweepData[(i + 1) % size(sweepData)].startFace;
        const facePlane = evPlane(context, { "face" : face });
        const prevFace = sweepData[i].endFace;
        const prevFacePlane = evPlane(context, { "face" : prevFace });

        if (corner.cornerType == FrameCornerType.BUTT)
        {
            createButtCorner(context, corner, cornerId, prevFace, prevFacePlane, face, facePlane, bodiesToDelete);
        }
        else if (corner.cornerType == FrameCornerType.COPED_BUTT)
        {
            createButtCorner(context, corner, cornerId, prevFace, prevFacePlane, face, facePlane, bodiesToDelete);
            copeButtCorner(context, topLevelId, cornerId, corner, prevFace, face, bodiesToDelete);
        }
        else if (corner.cornerType == FrameCornerType.MITER)
        {
            createMiterCorner(context, corner, cornerId, prevFace, prevFacePlane, face, facePlane, bodiesToDelete);
        }
    }
}

function cornerOverrideFound(position is Vector, cornerOverrides is array) returns map
{
    for (var index = 0; index < size(cornerOverrides); index += 1)
    {
        var cornerOverride = cornerOverrides[index];
        if (tolerantEquals(cornerOverride.position, position))
        {
            return {
                    "found" : true,
                    "cornerType" : cornerOverride.cornerType,
                    "cornerButtFlip" : cornerOverride.cornerButtFlip,
                    "index" : index
                };
        }
    }
    return { "found" : false };
}

function createEdgesFromSelectedVertices(context is Context, topLevelId is Id, selections is Query, bodiesToDelete is box) returns Query
{
    const idSpline = topLevelId + "spline";
    const allVertexQueries = evaluateQuery(context, qEntityFilter(selections, EntityType.VERTEX));

    for (var i = 0; i < size(allVertexQueries) - 1; i += 1)
    {
        const idOneSpline = idSpline + unstableIdComponent(i);
        const edgeVertexQueries = [allVertexQueries[i], allVertexQueries[i + 1]];
        // BEL-168513: tracking scheme with splines
        setExternalDisambiguation(context, idOneSpline, qUnion(edgeVertexQueries));
        const edgeVertexes = mapArray(edgeVertexQueries, function(v)
            {
                return evVertexPoint(context, { "vertex" : v });
            });
        opFitSpline(context, idOneSpline, { "points" : edgeVertexes });
    }

    cleanUpAtEndOfFeature(bodiesToDelete, qCreatedBy(idSpline, EntityType.BODY));
    return qCreatedBy(idSpline, EntityType.EDGE);
}

function createPathsFromSelections(context is Context, id is Id, selections is Query, bodiesToDelete is box) returns map
{
    const edgesFromVertexesQ = createEdgesFromSelectedVertices(context, id, selections, bodiesToDelete);
    const edgesFromFacesQ = qEntityFilter(qAdjacent(qEntityFilter(selections, EntityType.FACE), AdjacencyType.EDGE), EntityType.EDGE);

    const allEdgesQ = qUnion([
                // edges
                qEntityFilter(selections, EntityType.EDGE),
                // faces
                edgesFromFacesQ,
                // bodies
                qOwnedByBody(qBodyType(qEntityFilter(selections, EntityType.BODY), BodyType.WIRE), EntityType.EDGE),
                // constructed edges from vertexes
                edgesFromVertexesQ
            ]);

    const allEdges = evaluateQuery(context, allEdgesQ);
    const paths = constructPaths(context, allEdgesQ, {});
    verify(pathsAreValid(paths), ErrorStringEnum.FRAME_BAD_PATH);
    const pathData = enforceFirstSelectedEdgeUnflipped(paths, allEdges);

    return {
            "paths" : pathData.correctedPaths,
            "stableEdges" : pathData.firstSelectedEdges,
            "manipulatorEdge" : allEdges[0]
        };
}

function getProfile(context is Context, id is Id, definition is map, bodiesToDelete is box) returns map
{
    const profileId = id + "sketch";
    const instantiator = newInstantiator(profileId);

    // Selections from element libraries pass in qEverything(EntityType.BODY), which needs to be filtered
    // Sketch selections outside element libraries should be unaffected.
    verify(definition.profileSketch.partQuery != undefined, ErrorStringEnum.FRAME_SELECT_PROFILE, {
                "faultyParameters" : ["profileSketch"] });
    definition.profileSketch.partQuery = definition.profileSketch.partQuery->qSketchFilter(SketchObject.YES);

    try silent
    {
        addInstance(instantiator, definition.profileSketch, {});
    }
    catch
    {
        throw regenError(ErrorStringEnum.FRAME_SELECT_PROFILE, ["profileSketch"]);
    }
    instantiate(context, instantiator);
    cleanUpAtEndOfFeature(bodiesToDelete, qCreatedBy(profileId, EntityType.BODY));

    const customPointsQuery = getCustomFrameAlignmentPoints(context, profileId);
    const customPoints = evaluateQuery(context, customPointsQuery);

    const facesCreated = evaluateQuery(context, qCreatedBy(profileId, EntityType.FACE));
    verify(facesCreated != [], ErrorStringEnum.FRAME_PROFILE_REGION);

    // Use the faces adjacent to the exterior loop
    const allEdges = qAdjacent(qUnion(facesCreated), AdjacencyType.EDGE, EntityType.EDGE);
    const laminarEdges = allEdges->qEdgeTopologyFilter(EdgeTopology.ONE_SIDED);
    // Use both EDGE and VERTEX adjacency.  EDGE is necessary for pipe-like edges that have no vertices, and VERTEX is
    // necessary to avoid creating non-manifold sets of faces, such as when two rectangles are sketched over each other
    // to create a "+" shape (we need the center square of the + too, not just the outer 4 squares).
    const outerFaces = qUnion([
                qAdjacent(laminarEdges, AdjacencyType.EDGE, EntityType.FACE),
                qAdjacent(laminarEdges, AdjacencyType.VERTEX, EntityType.FACE)
            ]);


    const surfaceId = id + "profileSurface";
    opExtractSurface(context, surfaceId, {
                "faces" : outerFaces,
                "redundancyType" : ExtractSurfaceRedundancyType.REMOVE_ALL_REDUNDANCY
            });
    const surfaces = evaluateQuery(context, qCreatedBy(surfaceId, EntityType.BODY));
    if (size(surfaces) != 1)
    {
        throw regenError("Could not determine single frame profile.", outerFaces);
    }
    const profileBody = surfaces[0];
    cleanUpAtEndOfFeature(bodiesToDelete, profileBody);

    const profileFace = qOwnedByBody(profileBody, EntityType.FACE);
    if (size(evaluateQuery(context, profileFace)) != 1)
    {
        // If all faces were coplanar, redundancy removal would have removed all intermediate edges. Multiple faces
        // means not coplanar. We are already assured that the face is planar because we filter on sketch earlier in
        // the feature, and if the face somehow is not planar, it will fail the evPlane call a few lines ahead anyway.
        throw regenError("Frame profile is not planar", profileBody);
    }

    var faceData = {};
    faceData.profileBody = profileBody;
    faceData.profilePlane = evPlane(context, { "face" : qOwnedByBody(profileBody, EntityType.FACE) });
    faceData.pointsManipulatorData = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2442_USE_BOUNDING_BOX_CENTER)
        ? getPointsManipulatorData(context, faceData, customPoints)
        : getPointsManipulatorData_PRE_V2442(context, faceData, customPoints);
    // For cutlists:
    // Must use original profile, not extracted.  opExtractSurface seems to clear the attribute here.
    faceData.profileAttribute = getFrameProfileAttributeOrDefault(context, outerFaces, definition.profileSketch.configuration);
    return faceData;
}

function getPointsManipulatorData(context is Context, faceData is map, customPoints is array) returns map
{
    // By convention: Both default and additional alignments points are with respect to the bounding box
    // coordinate system, which is upright-aligned with the profileCS but with the bounding box center as origin.
    // So to account for the rare occasion where the profileCS != boundingBoxCS we must use the offsetData.centerOffset
    // to adjust our point locations.
    const profileCS = coordSystem(faceData.profilePlane);
    const offsetData = getAlignmentPointOffsetData(context, faceData.profileBody, profileCS);

    const customOffsets = mapArray(customPoints, function(pointQuery) {
        const pointWCS = evVertexPoint(context, { "vertex" : pointQuery });
        const pointProfileCS = worldToPlane3D(faceData.profilePlane, pointWCS);
        const offset = pointProfileCS - offsetData.centerOffset;
        return offset;
        });

    return {
        "center" : offsetData.centerOffset,
        "halfExtents" : offsetData.halfExtents,
        "offset" : concatenateArrays([offsetData.offsets, customOffsets])
    };
}

function getPointsManipulatorData_PRE_V2442(context is Context, faceData is map, customPoints is array) returns map
{
    const bb = evBox3d(context, {
                "topology" : faceData.profileBody,
                "cSys" : coordSystem(faceData.profilePlane)
            });
    const center2d = 0.5 * (bb.maxCorner + bb.minCorner);
    const halfExtents = 0.5 *(bb.maxCorner - bb.minCorner);
    //compute standard offsets with layout:
    //8 7 6
    //5 4 3
    //2 1 0
    var standardOffsets = makeArray(9);
    standardOffsets[8] = vector(-halfExtents[0], halfExtents[1], 0 * meter);
    standardOffsets[7] = vector(0 * meter, halfExtents[1], 0 * meter);
    standardOffsets[6] = vector(halfExtents[0], halfExtents[1], 0 * meter);
    standardOffsets[5] = vector(-halfExtents[0], 0 * meter, 0 * meter);
    standardOffsets[FRAME_NINE_POINT_CENTER_INDEX] = vector(0, 0, 0) * meter;
    standardOffsets[3] = -standardOffsets[5];
    standardOffsets[2] = -standardOffsets[6];
    standardOffsets[1] = -standardOffsets[7];
    standardOffsets[0] = -standardOffsets[8];

    const customOffsets = mapArray(customPoints, function(pointQuery) {
        const pointWCS = evVertexPoint(context, { "vertex" : pointQuery });
        return worldToPlane3D(faceData.profilePlane, pointWCS);
        });

    return {
        "center" : center2d,
        "halfExtents" : halfExtents,
        "offset" : concatenateArrays([standardOffsets, customOffsets])
    };
}

// Creates a plane with the Z-axis along the line's direction.
function getPlaneAtLineStart(context is Context, definition is map, edgeLine is Line) returns Plane
{
    if (isQueryEmpty(context, definition.angleReference))
    {
        //no user selection, fall back to heuristic
        const xDir = getXDirFromHeuristic(edgeLine);
        return plane(edgeLine.origin, edgeLine.direction, cross(xDir, edgeLine.direction));
    }

    const referenceDirection = extractDirection(context, definition.angleReference);
    //verify that the selection is a valid direction
    verify(referenceDirection != undefined, ErrorStringEnum.FRAME_ANGLE_REFERENCE_INVALID_ENTITY, {"faultyParameters" : ["angleReference"] });
    const yDir = cross(edgeLine.direction, referenceDirection);
    //verify direction is not aligned with edgeLine direction (i.e. normal of plane)
    verify(norm(yDir) > TOLERANCE.zeroLength, ErrorStringEnum.FRAME_ANGLE_REFERENCE_INVALID_ENTITY, {"faultyParameters" : ["angleReference"] });
    return plane(edgeLine.origin, edgeLine.direction, cross(yDir, edgeLine.direction));
}

// For consistency we use a heuristic to select the X-axis.
// When the path has no Z-component, use global Z. Otherwise use global Y.
function getXDirFromHeuristic(edgeLine is Line) returns Vector
{
    return (abs(edgeLine.direction[2]) > TOLERANCE.computational) ? Y_DIRECTION : Z_DIRECTION;
}

function createPointsManipulator(context is Context, topLevelId is Id, index is number, profileData is map, manipulatorPlane is Plane, rotationAngle is ValueWithUnits)
{
    // offset is defined from center point (index 4)
    // For every point, compute the offset as if the current index is the center
    // ex: if current index == 2, the offset to index 7 will be found by treating index 2 as zero.
    // For offset 7 when current index is 2: (-offset2 + offset7)
    // To confirm observe: offset 2 when current index is 2 works out to -offset 2 + offset 2 == 0.

    const manipulatorPlaneX = manipulatorPlane.x;
    const manipulatorPlaneY = cross(manipulatorPlane.normal, manipulatorPlane.x);
    const rotation = rotationMatrix3d(manipulatorPlane.normal, rotationAngle);

    // if user changes profile or deletes a custom tagged alignment point this will silently reset to center
    // ideally this would warn the user but because reference parameter changes are hard to reliably detect in editing logic
    // the decision to silently fail is better than to warn or fail in a way the user doesn't understand how to fix.
    if (index >= size(profileData.pointsManipulatorData.offset))
    {
        index = FRAME_NINE_POINT_CENTER_INDEX;
    }

    const points = mapArray(profileData.pointsManipulatorData.offset, function(offset) {
        const offsetFromCenter = -profileData.pointsManipulatorData.offset[index] + offset;
        var point = (offsetFromCenter[0] * manipulatorPlaneX) + (offsetFromCenter[1] * manipulatorPlaneY);
        point = manipulatorPlane.origin + rotation * point;
        return point;
    });
    return pointsManipulator({ "points" : points, "index" : index });
}

function getValueToIndexMap(container is array) returns map
{
    var index = 0;
    var indexMap = {};
    for (var value in container)
    {
        indexMap[value] = index;
        index += 1;
    }
    return indexMap;
}

function pathsAreValid(paths is array) returns boolean
{
    for (var path in paths)
    {
        const size = path.edges;
        if ((size == 0) || (size == 1 && path.closed))
        {
            return false;
        }
    }
    return true;
}

// constructPaths creates contiguous paths and sets arbitrary edge direction.
// For simplifying downstream logic we enforce the following convention:
// The first selected segment of any path will be unflipped.
// NB: The first selected segment may or may not be the first segment of the path.
function enforceFirstSelectedEdgeUnflipped(paths is array, allEdges is array) returns map
{
    var correctedPaths = [];
    var firstSelectedEdges = [];
    const edgeSelectionToIndexMap = getValueToIndexMap(allEdges);
    for (var path in paths)
    {
        const correctedPathData = correctOnePath(path, edgeSelectionToIndexMap);
        correctedPaths = append(correctedPaths, correctedPathData.path);
        firstSelectedEdges = append(firstSelectedEdges, correctedPathData.firstSelectedEdge);
    }
    return {
            "correctedPaths" : correctedPaths,
            "firstSelectedEdges" : firstSelectedEdges
        };
}

function correctOnePath(path is Path, edgeSelectionToIndexMap is map) returns map
{
    var lowestSelectionIndex = inf;
    var edgeIndexInPath = undefined;
    const numEdges = size(path.edges);
    for (var edgeIndex = 0; edgeIndex < numEdges; edgeIndex += 1)
    {
        const edge = path.edges[edgeIndex];
        const selectionIndex = edgeSelectionToIndexMap[edge];
        if (selectionIndex < lowestSelectionIndex)
        {
            lowestSelectionIndex = selectionIndex;
            edgeIndexInPath = edgeIndex;
        }
    }
    if (path.flipped[edgeIndexInPath])
    {
        path = reverse(path);
        edgeIndexInPath = (numEdges - 1) - edgeIndexInPath;
    }
    return {
            "path" : path,
            "firstSelectedEdge" : {
                "edge" : path.edges[edgeIndexInPath],
                "pathIndex" : edgeIndexInPath
            }
        };
}

function groupToolsPerSide(context is Context, capFaces is map, tools is array) returns map
{
    const toolBodiesQuery = qUnion(tools);

    var toolsPerSide = {
        "start" : qNothing(),
        "end" : qNothing()
    };

    if (capFaces.start != undefined && capFaces.end != undefined) // trimming from both ends, group tools per side
    {
        toolsPerSide = groupEntitiesBySide(context, capFaces, toolBodiesQuery);
    }
    else if (capFaces.start == undefined)
    {
        toolsPerSide.end = toolBodiesQuery;
    }
    else if (capFaces.end == undefined)
    {
        toolsPerSide.start = toolBodiesQuery;
    }

    return toolsPerSide;
}

function getTrimCandidates(context is Context, trimPlanes is Query, trimBodies is Query, trimEnds is array, sweepBodies is array) returns array
{
    // first get end piece collisions
    const trimEndBodies = qOwnerBody(qUnion(trimEnds));
    const collisions = computeCollisionsAgainstTools(context, trimPlanes, trimBodies, trimEndBodies);

    // for frames of size>1, only the starting face of the starting beam and the end face of the end beam are eligible.
    // so we make sure our collisions dont clip any 'inner faces' (any face that isnt the start or end face)
    for (var collision in collisions)
    {
        verify(!isInternalCapFace(context, collision.target), ErrorStringEnum.FRAME_NO_INTERNAL_TRIM, {
                    "entities" : collision.target });
    }

    // for size >= 3, we must also check that our trims aren't also trimming inner beams
    // check inner collisions and error if found
    const innerBeams = qSubtraction(qUnion(sweepBodies), trimEndBodies);
    const innerBeamCollisions = computeCollisionsAgainstTools(context, trimPlanes, trimBodies, innerBeams);

    if (innerBeamCollisions != [])
    {
        const collidingInnerBeamQuery = mapArray(innerBeamCollisions, function(c)
            {
                return qOwnerBody(c.target);
            });
        throw regenError(ErrorStringEnum.FRAME_NO_INTERNAL_TRIM, qUnion(collidingInnerBeamQuery));
    }

    return collisions;
}

function copeButtCorner(context is Context, topLevelId is Id, cornerId is Id, corner is map, prevFace is Query,
    face is Query, bodiesToDelete is box)
{
    const faceOnTarget = corner.cornerFlip ? prevFace : face;
    const targetBody = evaluateQuery(context, qOwnerBody(faceOnTarget))[0];
    const toolBody = qOwnerBody(corner.cornerFlip ? face : prevFace);
    const isStart = isStartFace(context, faceOnTarget);
    const newTarget = doOneEndTrim(context, topLevelId, cornerId, targetBody, toolBody, isStart, bodiesToDelete);
}

function createMiterCorner(context is Context, corner is map, cornerId is Id, prevFace is Query, prevFacePlane is Plane,
    face is Query, facePlane is Plane, bodiesToDelete is box)
{
    const firstMiterPlane = flipPlaneIfNeeded(prevFacePlane, corner.miterPlane);
    const secondMiterPlane = flipPlaneIfNeeded(facePlane, firstMiterPlane);
    createOneCorner(context, cornerId, prevFace, firstMiterPlane, face, secondMiterPlane, bodiesToDelete);
}

function createButtCorner(context is Context, corner is map, cornerId is Id, prevFace is Query, prevFacePlane is Plane,
    face is Query, facePlane is Plane, bodiesToDelete is box)
{
    const firstButtPlane = flipPlaneIfNeeded(prevFacePlane, corner.prevButtPlane);
    const secondButtPlane = flipPlaneIfNeeded(facePlane, corner.buttPlane);
    createOneCorner(context, cornerId, prevFace, firstButtPlane, face, secondButtPlane, bodiesToDelete);
}

function createOneCorner(context is Context, cornerId is Id, prevFace is Query, firstPlane is Plane,
    face is Query, secondPlane is Plane, bodiesToDelete is box)
{
    const idPrevPlane = cornerId + "plane1";
    const idPlane = cornerId + "plane2";
    opPlane(context, idPrevPlane, { "plane" : firstPlane, "width" : 1 * meter, "height" : 1 * meter });
    opPlane(context, idPlane, { "plane" : secondPlane, "width" : 1 * meter, "height" : 1 * meter });
    // By using replaceFace we are preserving attributes
    opReplaceFace(context, cornerId + "replaceFace1", {
                "replaceFaces" : prevFace,
                "templateFace" : qCreatedBy(idPrevPlane, EntityType.FACE)
            });
    opReplaceFace(context, cornerId + "replaceFace2", {
                "replaceFaces" : face,
                "templateFace" : qCreatedBy(idPlane, EntityType.FACE)
            });
    cleanUpAtEndOfFeature(bodiesToDelete, qUnion([qCreatedBy(idPlane), qCreatedBy(idPrevPlane)]));
}

function getMiterCornerData(prevEdgeEnd is map, edgeStart is map, position is Vector) returns map
{
    const corner = {
            "cornerType" : FrameCornerType.MITER,
            "miterPlane" : plane(prevEdgeEnd.origin, .5 * (prevEdgeEnd.direction + edgeStart.direction))
        };
    return corner;
}

function getButtCornerData(context is Context, definition is map, prevEdgeEnd is map, edgeStart is map,
    cornerOverride is map, face is Query, prevFace is Query, buttCornerType is FrameCornerType) returns map
{
    const position = prevEdgeEnd.origin;
    const normal = cross(prevEdgeEnd.direction, edgeStart.direction);
    const prevTargetNormal = cross(normal, edgeStart.direction);
    const targetNormal = cross(normal, prevEdgeEnd.direction);

    var facePlane = plane(edgeStart.origin, -edgeStart.direction);
    var prevFacePlane = plane(prevEdgeEnd.origin, prevEdgeEnd.direction);
    facePlane.x = normalize(cross(normal, facePlane.normal));
    prevFacePlane.x = normalize(cross(prevFacePlane.normal, normal));

    const faceBox = evBox3d(context, {
                "topology" : face,
                "tight" : true,
                "cSys" : coordSystem(facePlane)
            });

    const previousFaceBox = evBox3d(context, {
                "topology" : prevFace,
                "tight" : true,
                "cSys" : coordSystem(prevFacePlane)
            });

    const cornerFlip = cornerOverride.found ? cornerOverride.cornerButtFlip : definition.defaultButtFlip;
    const isCoped = buttCornerType == FrameCornerType.COPED_BUTT;
    const offset = getButtPlaneOffsets(faceBox, previousFaceBox, isCoped, cornerFlip);
    const prevButtPlane = plane(facePlane.origin + offset.distance * facePlane.x, prevTargetNormal);
    const buttPlane = plane(prevFacePlane.origin + offset.prevDistance * prevFacePlane.x, targetNormal);

    var manipulator = undefined;
    if (cornerOverride.found)
    {
        const flipManipulator = flipManipulator({
                    "base" : position,
                    "direction" : prevEdgeEnd.direction,
                    "otherDirection" : -edgeStart.direction,
                    "flipped" : cornerFlip
                });

        manipulator = { "flip" ~ cornerOverride.index : flipManipulator };
    }

    const corner = {
            "cornerType" : buttCornerType,
            "cornerFlip" : cornerFlip,
            "prevButtPlane" : prevButtPlane,
            "buttPlane" : buttPlane,
            "manipulator" : manipulator
        };
    return corner;
}

function getFrameData(context is Context, sweepId is Id, sweepData is map) returns map
{
    const frameData = {
            "sweptEdges" : makeQuery(sweepId, "SWEPT_EDGE", EntityType.EDGE, {}),
            "sweptFaces" : makeQuery(sweepId, "SWEPT_FACE", EntityType.FACE, {}),
            "startFace" : sweepData.startFace,
            "endFace" : sweepData.endFace
        };
    return frameData;
}

function isButtCorner(cornerType is FrameCornerType) returns boolean
{
    return (cornerType == FrameCornerType.BUTT || cornerType == FrameCornerType.COPED_BUTT);
}

function getSweepBodies(pathData is map) returns array
{
    const bodies = mapArray(pathData.sweepData, function(sweepData)
        {
            return sweepData.body;
        });
    return bodies;
}

function setAttributesOnPath(context is Context, profileData is map, pathSweepData is array)
{
    for (var sweepData in pathSweepData)
    {
        const frameData = getFrameData(context, sweepData.id, sweepData);
        setFrameAttributes(context, qCreatedBy(sweepData.id, EntityType.BODY), profileData, frameData);
    }
    setFrameTerminusAttributes(context, pathSweepData[0].startFace, last(pathSweepData).endFace);
}

// NB: Functions below are used in downversion frames operations only.
function sweepFrames_PRE_V1742(context is Context, topLevelId is Id, definition is map, profileData is map, bodiesToDelete is box) returns map
{
    verify(!isQueryEmpty(context, definition.selections), ErrorStringEnum.FRAME_SELECT_PATH, { "faultyParameters" : ["selections"] });

    var trimEnds = [];
    var sweepBodies = [];
    var manipulators = {};

    const cornerOverrides = gatherCornerOverrides(context, definition.cornerOverrides);
    const pathData = createPathsFromSelections(context, topLevelId, definition.selections, bodiesToDelete);
    const paths = pathData.paths;

    for (var pathIndex = 0; pathIndex < size(paths); pathIndex += 1)
    {
        const pathId = topLevelId + unstableIdComponent(pathIndex);
        const edgePath = paths[pathIndex];
        var pathSweepData = [];
        var pathCornerData = [];
        var pathFrameData = [];

        const numPathSegments = size(edgePath.edges);
        for (var edgeIndex = 0; edgeIndex < numPathSegments; edgeIndex += 1)
        {
            const edgeId = pathId + unstableIdComponent(edgeIndex);
            setExternalDisambiguation(context, edgeId, edgePath.edges[edgeIndex]);
            const profileId = edgeId + "profile";
            const edgeResult = (edgeIndex == 0) ?
                handleStartingEdge(context, definition, edgeId, edgePath, profileData) :
                handleContinuingEdge(context, definition, pathSweepData, edgeId, edgePath, edgeIndex, cornerOverrides, bodiesToDelete);

            manipulators = updateManipulators(manipulators, edgeResult.manipulators);
            pathCornerData = updateCornerData(pathCornerData, edgeResult.cornerData);
            const sweepId = edgeId + "sweep";
            const sweepData = sweepOneEdge(context, definition, sweepId, edgeResult.faceToSweep, edgeResult.edge);
            sweepBodies = append(sweepBodies, sweepData.body);
            pathSweepData = append(pathSweepData, sweepData);
            // make additional corner for closed loop
            const closedLoopCornerData = handleCornerForClosedLoop(context, definition, cornerOverrides, pathSweepData, edgePath, edgeIndex);
            manipulators = updateManipulators(manipulators, closedLoopCornerData.manipulator);
            pathCornerData = updateCornerData(pathCornerData, closedLoopCornerData);
            cleanUpAtEndOfFeature(bodiesToDelete, qCreatedBy(profileId, EntityType.BODY));
            const frameData = getFrameData(context, sweepId, sweepData);

            const frameQ = qCreatedBy(sweepId, EntityType.BODY);
            setFrameAttributes(context, frameQ, profileData, frameData);
            pathFrameData = append(pathFrameData, frameData);
        }
        // Only allow trimming of first segments's startFace and last segment's endFace
        // for paths of only one segment, this will be both ends of that single beam
        const lastPathSegment = last(pathFrameData);
        setFrameTerminusAttributes(context, pathFrameData[0].startFace, lastPathSegment.endFace);
        trimEnds = concatenateArrays([trimEnds, [pathFrameData[0].startFace, lastPathSegment.endFace]]);

        const createCornerId = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2149_DEPRECATE_INCREMENTING_ID_GENERATOR)
            ? getUnstableIncrementingId(pathId)
            : getIncrementingId(pathId);
        createCorners(context, topLevelId, createCornerId, pathSweepData, pathCornerData, bodiesToDelete);
    }
    // Always attach the frame manipulator to the first edge of the first path
    // this keeps the manipulator oriented correctly to the frame orientation (even though this orientation may change based on edge selection)
    const manipulatorEdge = paths[0].edges[0];
    const manipulatorFlipped = paths[0].flipped[0];
    const manipulatorLine = evaluatePathEdge(context, manipulatorEdge, manipulatorFlipped, 0.5);
    const frameManipulators = createFrameManipulators(context, topLevelId, definition, manipulatorLine, profileData);

    manipulators = updateManipulators(frameManipulators, manipulators);
    return {
            "trimEnds" : trimEnds,
            "manipulators" : manipulators,
            "sweepBodies" : sweepBodies,
            "compositeGroups" : []
        };
}

function handleStartingEdge(context is Context, definition is map, edgeId is Id,
    edgePath is Path, profileData is map) returns map
{
    const profileId = edgeId + "profile";
    // by definition the starting edge has index 0
    const edge = edgePath.edges[0];
    const edgeLine = evaluatePathEdge(context, edge, edgePath.flipped[0], 0);
    const planeAtEdgeStart = getPlaneAtLineStart(context, definition, edgeLine);
    const profilePlane = getProfilePlane(profileData, definition);

    const mirrorAndAngleTransform = getMirrorAndAngleTransform(definition.mirrorProfile, edgeLine, planeAtEdgeStart, definition.angle);
    const profileTransform = mirrorAndAngleTransform * transform(profilePlane, planeAtEdgeStart);
    opPattern(context, profileId, {
                "entities" : profileData.profileBody,
                "transforms" : [profileTransform],
                "instanceNames" : ["1"]
            });

    return {
            "edge" : edge,
            "faceToSweep" : qCreatedBy(profileId, EntityType.FACE),
            "manipulators" : {},
            "cornerData" : {}
        };
}

function handleContinuingEdge(context is Context, definition is map, sweepData is array, edgeId is Id,
    edgePath is Path, edgeIndex is number, cornerOverrides is array, bodiesToDelete is box) returns map
{
    const profileId = edgeId + "profile";
    // We transform the end face of the previous sweep to the start of new edge
    // rather than transforming from original plane to start of new edge.
    // BEL-168513: Fixes geometric problems for spline paths but causes tracking problems.
    const edge = edgePath.edges[edgeIndex];
    const edgeLine = evaluatePathEdge(context, edge, edgePath.flipped[edgeIndex], 0);

    const endFace = sweepData[edgeIndex - 1].endFace;
    opExtractSurface(context, edgeId + "extract", { "faces" : endFace });
    const extractedBody = qCreatedBy(edgeId + "extract", EntityType.BODY);
    cleanUpAtEndOfFeature(bodiesToDelete, extractedBody);
    var prevEdgeEnd = evaluatePathEdgeFromIndex(context, edgePath, edgeIndex - 1, 1);
    const sweepProfileTransform = transform(line(prevEdgeEnd.origin, prevEdgeEnd.direction), line(edgeLine.origin, edgeLine.direction));
    opTransform(context, profileId, {
                "bodies" : extractedBody,
                "transform" : sweepProfileTransform
            });
    const faceToSweep = qOwnedByBody(extractedBody, EntityType.FACE);
    const cornerData = getCurrentCornerData(context, prevEdgeEnd, edgeLine, cornerOverrides, definition, faceToSweep,
        endFace);
    return ({
                "edge" : edge,
                "faceToSweep" : faceToSweep,
                "manipulators" : cornerData.manipulator,
                "cornerData" : cornerData
            });
}

function groupCollisionResults_PRE_2057(context is Context, collisions is array) returns map
{
    var capFaceToToolBodies = {};
    var framesToBodies = {};

    for (var collision in collisions)
    {
        // if capFaces are trimmed, we will extend their beam end to improve trimming
        if (isFrameCapFace(context, collision.target))
        {
            capFaceToToolBodies = insertIntoMapOfArrays(capFaceToToolBodies, collision.target, collision.toolBody);
        }

        // Collect only collisions with overlapping volume and ignore adjacency or containment collisions
        if (collision["type"] == ClashType.INTERFERE)
        {
            framesToBodies = insertIntoMapOfArrays(framesToBodies, collision.targetBody, collision.toolBody);
        }
    }

    return
    {
            "capFaceToToolBodies" : capFaceToToolBodies,
            "framesToBodies" : framesToBodies
        };
}

function getIncrementingId(prefix is Id) returns function
{
    var index = new box(0);
    return function()
        {
            const newId = prefix + index[];
            index[] += 1;
            return newId;
        };
}

