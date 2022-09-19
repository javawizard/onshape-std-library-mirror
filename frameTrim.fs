FeatureScript 1847; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/booleanoperationtype.gen.fs", version : "1847.0");
import(path : "onshape/std/containers.fs", version : "1847.0");
import(path : "onshape/std/error.fs", version : "1847.0");
import(path : "onshape/std/evaluate.fs", version : "1847.0");
import(path : "onshape/std/feature.fs", version : "1847.0");
import(path : "onshape/std/frameAttributes.fs", version : "1847.0");
import(path : "onshape/std/frameUtils.fs", version : "1847.0");
import(path : "onshape/std/manipulator.fs", version : "1847.0");

/** @internal */
export enum FrameTrimType
{
    annotation { "Name" : "Ordered groups" }
    GROUP,
    annotation { "Name" : "Face" }
    FACE
}

/**
 * Trim frames against faces, or perform an ordered trim of frame groups.
 */
annotation {
        "Feature Type Name" : "Frame trim",
        "Manipulator Change Function" : "frameTrimManipulatorChange"
    }
export const frameTrim = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation {
                    "Name" : "Trim Type",
                    "UIHint" : [UIHint.HORIZONTAL_ENUM, UIHint.REMEMBER_PREVIOUS_VALUE]
                }
        definition.trimType is FrameTrimType;

        if (definition.trimType == FrameTrimType.GROUP)
        {
            annotation {
                        "Name" : "Frame groups",
                        "Description" : "Frame segments in earlier groups are used as tools for later groups",
                        "Item name" : "group",
                        "Driven query" : "frames",
                        "Item label template" : "#frames",
                        "UIHint" : UIHint.COLLAPSE_ARRAY_ITEMS
                    }
            definition.frameGroups is array;

            for (var profile in definition.frameGroups)
            {
                annotation {
                            "Name" : "Frames",
                            "Filter" : EntityType.BODY && BodyType.SOLID,
                            "Description" : "Frame segments are trimmed against preceding groups and are used as tools for later groups"
                        }
                profile.frames is Query;
            }
        }
        else
        {
            annotation {
                        "Name" : "Parts to trim",
                        "Filter" : EntityType.BODY && BodyType.SOLID,
                        "Description" : "Parts to trim using trim tool"
                    }
            definition.targets is Query;

            annotation {

                        "Name" : "Face to trim to",
                        "Filter" : EntityType.FACE || (EntityType.BODY && BodyType.SHEET && ModifiableEntityOnly.NO) ||
                        (ConstructionObject.YES && GeometryType.PLANE),
                        "Description" : "Face, plane, or planar face to use as trim tool",
                        "MaxNumberOfPicks" : 1
                    }
            definition.tool is Query;

            annotation {
                        "Name" : "Opposite side",
                        "UIHint" : UIHint.OPPOSITE_DIRECTION,
                        "Default" : false
                    }
            definition.flipTrim is boolean;

            annotation {
                        "Name" : "Trim to face boundaries",
                        "Default" : false
                    }
            definition.useToolExtents is boolean;
        }
    }
    {
        switch (definition.trimType)
            {
                    FrameTrimType.GROUP : groupTrim(context, id, definition),
                    FrameTrimType.FACE : faceTrim(context, id, definition)
                };
    },
    {
            trimType : FrameTrimType.GROUP,
            useToolExtents : false,
            flipTrim : false
        });

function groupTrim(context is Context, topLevelId is Id, definition is map)
{
    const frameGroups = definition.frameGroups;
    validateGroups(context, frameGroups);
    var bodiesToDelete = new box([]);
    doAllGroupTrims(context, topLevelId, frameGroups, bodiesToDelete);
    deleteBodies(context, topLevelId, bodiesToDelete);
}

function doAllGroupTrims(context is Context, topLevelId is Id, frameGroups is array, bodiesToDelete is box)
{
    // The groups are set by the user in decreasing priority.
    // First group does not get modified. Last group is trimmed by all other groups.
    // Ex: There are three groups [a,b], [c], [d,e].
    // This operation will therefore perform two booleans.
    // The first boolean is: [d,e] - [a,b,c]
    // The second boolean is: [c] - [a,b]
    const allTools = aggregateTools(frameGroups);
    const numBooleans = size(frameGroups) - 1;
    const trimId = getUnstableIncrementingId(topLevelId + "groupTrim");
    for (var i = numBooleans; i > 0; i -= 1)
    {
        const targets = frameGroups[i].frames;
        const tools = qUnion(allTools[i - 1]);
        doOneBooleanTrim(context, topLevelId, trimId(), targets, tools, bodiesToDelete);
    }
}

function applyKeptBodyHeuristic(context is Context, allBodies is Query) returns Query
{
    //By convention, Frame trim always keeps the single largest body per input beam.
    const largestBody = qLargest(allBodies);
    verify(size(evaluateQuery(context, largestBody)) == 1, ErrorStringEnum.FRAME_MULTIPLE_EQUAL_SEGMENTS_AFTER_SPLIT, {
                "entities" : allBodies
                });
    return largestBody;
}

function doOneBoolean(context is Context, topLevelId is Id, trimId is Id, targets is Query, tools is Query)
{
    callSubfeatureAndProcessStatus(topLevelId, opBoolean, context, trimId, {
                "tools" : tools,
                "targets" : targets,
                "keepTools" : true,
                "operationType" : BooleanOperationType.SUBTRACTION
            }, {
                "overrideStatus" : ErrorStringEnum.FRAME_TRIM_FAILED,
                "propagateErrorDisplay" : true,
                "additionalErrorEntities" : qUnion([tools, targets])
            });
}

function faceTrim(context is Context, topLevelId is Id, definition is map)
{
    verify(!isQueryEmpty(context, definition.targets), ErrorStringEnum.FRAME_TRIM_SELECT_TARGETS, { "faultyParameters" : ["targets"] });
    verify(!isQueryEmpty(context, definition.tool), ErrorStringEnum.FRAME_TRIM_SELECT_TOOL, { "faultyParameters" : ["tool"] });

    addKeepSideManipulator(context, topLevelId, definition);
    var bodiesToDelete = new box([]);
    const keepType = definition.flipTrim ? SplitOperationKeepType.KEEP_BACK : SplitOperationKeepType.KEEP_FRONT;
    doOneFaceTrim(context, topLevelId, topLevelId + "splitByFace", definition.targets, definition.tool, definition.useToolExtents, keepType, bodiesToDelete);
    deleteBodies(context, topLevelId, bodiesToDelete);
}

function getBeamEndAttributeData(context is Context, target is Query) returns map
{
    const startFaces = evaluateQuery(context, qFrameStartFace(target));
    const endFaces = evaluateQuery(context, qFrameEndFace(target));
    verify(startFaces != [], ErrorStringEnum.FRAME_MISSING_CAP_FACES);
    verify(endFaces != [], ErrorStringEnum.FRAME_MISSING_CAP_FACES);
    const startAttribute = getFrameTopologyAttribute(context, startFaces[0]);
    const endAttribute = getFrameTopologyAttribute(context, endFaces[0]);
    //in case all end cap information is removed, first compute the bb center for later distance applyNearest heuristic
    const startBB = evBox3d(context, {
                "topology" : qFrameStartFace(target),
                "tight" : true
            });

    const endBB = evBox3d(context, {
                "topology" : qFrameEndFace(target),
                "tight" : true
            });

    return {
            "startAttribute" : startAttribute,
            "endAttribute" : endAttribute,
            "startBB" : startBB,
            "endBB" : endBB
        };
}

function doOneBooleanTrim(context is Context, topLevelId is Id, trimId is Id, targets is Query, tools is Query, bodiesToDelete is box)
{
    //Each operation takes "M tools, N targets"
    //but track and reattribute each beam individually
    const allTargets = evaluateQuery(context, targets);
    const beamEndAttributes = mapArray(allTargets, function(target)
        {
            return getBeamEndAttributeData(context, target);
        });
    const trackingTargets = mapArray(allTargets, function(target)
        {
            return qUnion(target, startTracking(context, target));
        });
    doOneBoolean(context, topLevelId, trimId, targets, tools);
    const keptBodies = mapArray(trackingTargets, function(trackedTarget)
        {
            return applyKeptBodyHeuristic(context, trackedTarget);
        });
    for (var i = 0; i < size(keptBodies); i += 1)
    {
        handleOneTrimResult(context, trackingTargets[i], keptBodies[i], trimId, beamEndAttributes[i], bodiesToDelete);
    }
}

function handleOneTrimResult(context is Context, trackedTarget is Query, keptBody is Query, trimId is Id,
    beamEndAttributes is map, bodiesToDelete is box)
{
    gatherBodiesForDeletion(context, trackedTarget, keptBody, bodiesToDelete);
    const createdFaces = qIntersection([qOwnedByBody(keptBody, EntityType.FACE), qCreatedBy(trimId, EntityType.FACE)]);
    reapplyAttributes(context, keptBody, createdFaces, beamEndAttributes);
    validateBeamAttributes(context, createdFaces, keptBody);
}

function doOneFaceTrim(context is Context, topLevelId is Id, trimId is Id, targets is Query, tool is Query, useToolExtents is boolean, keepType is SplitOperationKeepType, bodiesToDelete is box)
{
    const allTargets = evaluateQuery(context, targets);
    const beamEndAttributes = mapArray(allTargets, function(target)
        {
            return getBeamEndAttributeData(context, target);
        });
    //The split may or may not affect the body, so consider the original body as well.
    const trackingTargets = mapArray(allTargets, function(target)
        {
            return qUnion(target, startTracking(context, target));
        });

    //always treat construction planes as infinite
    const constructionPlaneQuery = qConstructionFilter(tool, ConstructionObject.YES);
    if (!isQueryEmpty(context, constructionPlaneQuery))
    {
        useToolExtents = false;
    }

    doOneSplit(context, topLevelId, trimId, targets, tool, useToolExtents, keepType);
    const keptBodies = mapArray(trackingTargets, function(trackedTarget)
        {
            return applyKeptBodyHeuristic(context, trackedTarget);
        });
    for (var i = 0; i < size(keptBodies); i += 1)
    {
        handleOneTrimResult(context, trackingTargets[i], keptBodies[i], trimId, beamEndAttributes[i], bodiesToDelete);
    }
}

function reapplyAttributes(context is Context, beam is Query, createdFacesQuery is Query, beamAttributes is map)
{
    //Only reapply attributes to created faces.
    //A note about trimming operations:
    //A beam boolean or beam split can produce 0 or more trim sites (disjoint set of trim faces).
    //Allow 0, 1, or 2 sites: No trim, one end, or both ends. Detect and fail the case of >2 trim sites.
    //A beam end after a trim operation can be either untrimmed, a partial trim, or a complete trim.
    //An untrimmed beam end retains its old attributes and faces. No work is needed.
    //A partial trim splits a cap face, leaving some face on the keptBody with attribute information.
    //A complete trim sites split only swept faces, cutting off the cap face, leaving a new face with no attribute
    //information.
    //The exhaustive list of cases to handle:
    //[partial, no trim]: reapplyFlood will reattribute partial
    //[complete, no trim]: reapplyFlood does nothing, then reapplyMissing reattributes complete
    //[partial, partial]: reapplyFlood will reattribute both
    //[partial, complete]: reapplyFlood will reattribute partial, then reapplyMissing reattributes complete
    //[complete, compete]: arbitrary reassignment will reattribute the two new faces (or error)
    //Some additional topological observation:
    //A split trim operation produces a single face per trim site.
    //It is NOT guaranteed that in a partial split trim a created face will be adjacent to an attributed end face.
    //A boolean trim operation may produce multiple faces.
    //In a partial boolean trim, these faces may NOT all be adjacent to an attributed face.
    //Thus in some cases the floodFill will not be able to 'reach' new faces without traversing some attributed face.
    //However, floodfill uses proximity to fill newly created faces to see if they form a cluster (every face shares
    //an edge with at least one other face in the cluster).
    //Thus a "boolean" trim is a more general case of a "split" trim, with the more general attribution algorithm
    //correct for both.

    //Early exit when there are no new faces
    if (isQueryEmpty(context, createdFacesQuery))
    {
        return;
    }

    const startFaceQuery = qFrameStartFace(beam);
    const startFaces = evaluateQuery(context, startFaceQuery);
    const hasStartFaces = startFaces != [];

    if (hasStartFaces)
    {
        //For our flood process, use a single old start face.
        //Allow flood to fill createdFaces and unattributed faces.
        //Allow our flood to cross preexisting start faces.
        const allAttributedFacesQuery = qFrameAllFaces(beam);
        //NB: unattributed faces includes the created faces
        const allUnattributedFacesQuery = qSubtraction(qOwnedByBody(beam, EntityType.FACE), allAttributedFacesQuery);
        const candidateFaceQuery = qUnion([startFaceQuery, allUnattributedFacesQuery]);
        reapplyFlood(context, startFaces[0], candidateFaceQuery, beamAttributes.startAttribute);
    }

    const endFaceQuery = qFrameEndFace(beam);
    const endFaces = evaluateQuery(context, endFaceQuery);
    const hasEndFaces = endFaces != [];

    if (hasEndFaces)
    {
        //NB: create new queries because attribution may have changed model state
        const allAttributedFacesQuery = qFrameAllFaces(beam);
        const allUnattributedFacesQuery = qSubtraction(qOwnedByBody(beam, EntityType.FACE), allAttributedFacesQuery);
        const candidateFaceQuery = qUnion([endFaceQuery, allUnattributedFacesQuery]);
        reapplyFlood(context, endFaces[0], candidateFaceQuery, beamAttributes.endAttribute);
    }

    if (hasStartFaces != hasEndFaces)
    {
        //this means one and only one side exists so reapply the missing
        //one of these reapplyMissing calls will early exit
        //the other will apply the 'opposite' attributes to all new faces
        //NB: create new queries because attribution may have changed model state
        const startFaceQuery = qFrameStartFace(beam);
        const endFaceQuery = qFrameEndFace(beam);
        const allAttributedFacesQuery = qFrameAllFaces(beam);
        const candidateFaceQuery = qSubtraction(createdFacesQuery, allAttributedFacesQuery);
        reapplyMissing(context, beam, startFaceQuery, candidateFaceQuery, beamAttributes.startAttribute);
        reapplyMissing(context, beam, endFaceQuery, candidateFaceQuery, beamAttributes.endAttribute);
    }

    if (!hasStartFaces && !hasEndFaces)
    {
        //there is no preexisting cap information to work from at either end.
        //the final heuristic is to reapply the attributes based on proximity to previously attributed faces.
        const allAttributedFacesQuery = qFrameAllFaces(beam);
        const allUnattributedFacesQuery = qSubtraction(qOwnedByBody(beam, EntityType.FACE), allAttributedFacesQuery);
        const createdUnattributedFacesQuery = qSubtraction(createdFacesQuery, allAttributedFacesQuery);
        const candidateFacesQuery = qIntersection([createdFacesQuery, allUnattributedFacesQuery]);
        reapplyNearest(context, createdUnattributedFacesQuery, allUnattributedFacesQuery, beamAttributes);
    }
}

function reapplyFlood(context is Context, seedFace is Query, candidateFaces is Query, attributesToReapply is FrameTopologyAttribute)
{
    const clusteredFaces = qUnion(clusterByAdjacency(context, seedFace, [], candidateFaces));
    if (!isQueryEmpty(context, clusteredFaces))
    {
        setFrameTopologyAttribute(context, clusteredFaces, attributesToReapply);
    }
}

function reapplyMissing(context is Context, beam is Query, attributedFaces is Query, createdFaces is Query, attributesToReapply is FrameTopologyAttribute)
{
    //early exit if attributed faces exist
    if (!isQueryEmpty(context, attributedFaces))
    {
        return;
    }
    //NB: All faces created by this operation as needing re-attribution. There is no partial set to floodfill from.
    setFrameTopologyAttribute(context, createdFaces, attributesToReapply);
}

function clusterByAdjacency(context is Context, seedFace is Query, clusterFaces is array, candidateFaces is Query) returns array
{
    //Gather all faces adjacent to seedFace
    //Only consider the faces in the candidateFaces set
    //Remove faces already in the cluster (no backtracking)
    var childFacesQuery = qAdjacent(seedFace, AdjacencyType.EDGE, EntityType.FACE);
    childFacesQuery = qIntersection(childFacesQuery, candidateFaces);
    childFacesQuery = qSubtraction(childFacesQuery, qUnion(clusterFaces));
    const childFaces = evaluateQuery(context, childFacesQuery);
    //add faces to cluster to prevent backtracking
    clusterFaces = concatenateArrays([clusterFaces, childFaces]);
    var returnedFaces = childFaces;
    for (var childFace in childFaces)
    {
        //recursive call (depth first search)
        const childClusterFaces = clusterByAdjacency(context, childFace, clusterFaces, candidateFaces);
        //prevent backtracking
        clusterFaces = concatenateArrays([clusterFaces, childClusterFaces]);
        //aggregate all faces reached from this face
        returnedFaces = concatenateArrays([returnedFaces, childClusterFaces]);
    }
    return returnedFaces;
}

function reapplyNearest(context is Context, createdFacesQuery is Query, candidateFaceQuery is Query, beamAttributes is map)
{
    //BEL-174084: evDistance is unstable.
    //Handle the case of two 'complete' trims.
    //With splits, this would result in two faces that are not adjacent.
    //With booleans, this results in two clusters that are not adjacent.
    verify(!isQueryEmpty(context, createdFacesQuery), ErrorStringEnum.FRAME_CANDIDATE_FACES);
    const allCreatedFaces = evaluateQuery(context, createdFacesQuery);
    const firstFace = allCreatedFaces[0];
    const firstCluster = append(clusterByAdjacency(context, firstFace, [firstFace], candidateFaceQuery), firstFace);
    const firstClusterQuery = qUnion(firstCluster);
    //determine remaining unclustered faces
    const remainingFaceQuery = qSubtraction(candidateFaceQuery, firstClusterQuery);
    verify(!isQueryEmpty(context, remainingFaceQuery), ErrorStringEnum.FRAME_CANDIDATE_FACES);
    const allRemainingFaces = evaluateQuery(context, remainingFaceQuery);
    const secondFace = allRemainingFaces[0];
    const secondCluster = append(clusterByAdjacency(context, secondFace, [secondFace], remainingFaceQuery), secondFace);
    const secondClusterQuery = qUnion(secondCluster);
    //use distance to old faces. Whichever face-bbcorner pair is smallest sets the attribution
    const firstAttribute = getCloserAttribute(context, firstClusterQuery, beamAttributes);
    const secondAttribute = getCloserAttribute(context, secondClusterQuery, beamAttributes);
    //the smallest distance is the most likely correct choice.
    if (firstAttribute.distance != undefined &&
        secondAttribute.distance != undefined &&
        firstAttribute.distance < secondAttribute.distance)
    {
        setFrameTopologyAttribute(context, firstClusterQuery, firstAttribute.attribute);
        setFrameTopologyAttribute(context, secondClusterQuery, firstAttribute.other);
    }
    else
    {
        //also handles undefined distance cases
        setFrameTopologyAttribute(context, firstClusterQuery, secondAttribute.other);
        setFrameTopologyAttribute(context, secondClusterQuery, secondAttribute.attribute);
    }
}

function getCloserAttribute(context is Context, faceQuery is Query, beamAttributes is map) returns map
{
    const minDistanceToStart = getMinDistanceToBBExtents(context, faceQuery, beamAttributes.startBB);
    const minDistanceToEnd = getMinDistanceToBBExtents(context, faceQuery, beamAttributes.endBB);
    if ((minDistanceToStart != undefined) && (minDistanceToEnd != undefined) &&
        (minDistanceToStart < minDistanceToEnd))
    {
        return {
                "distance" : minDistanceToStart,
                "attribute" : beamAttributes.startAttribute,
                "other" : beamAttributes.endAttribute
            };
    }
    else
    {
        //handles undefined and minDistanceToEnd cases
        //if the min distances are undefined, then there is insufficient information so either choice is valid
        return {
                "distance" : minDistanceToEnd,
                "attribute" : beamAttributes.endAttribute,
                "other" : beamAttributes.startAttribute
            };
    }
}

function getMinDistanceToBBExtents(context is Context, faceQuery is Query, bb is Box3d)
{
    try silent
    {
        const minCornerDist = evDistance(context, {
                    "side0" : faceQuery,
                    "side1" : bb.minCorner
                });
        const maxCornerDist = evDistance(context, {
                    "side0" : faceQuery,
                    "side1" : bb.maxCorner
                });
        return (minCornerDist.distance < maxCornerDist.distance) ? minCornerDist.distance : maxCornerDist.distance;
    }
    return undefined;
}

function validateBeamAttributes(context is Context, createdFaces is Query, beam is Query)
{
    //a well-formed beam cannot have end caps adjacent to one another.
    //a "lengthwise split" would create a face that touches both start and end cap faces.
    //an incorrect floodfill would also do it.
    //This forces a bad frametrim to error immediately which is easier to find and fix.
    {
        const startFaces = qFrameStartFace(beam);
        const endFaces = qFrameEndFace(beam);
        const startAdjacentFaces = qAdjacent(startFaces, AdjacencyType.EDGE, EntityType.FACE);
        const incorrectAttributeFaces = qIntersection([endFaces, startAdjacentFaces]);
        verify(isQueryEmpty(context, incorrectAttributeFaces), ErrorStringEnum.FRAME_LENGTHWISE_TRIM);
    }
}

function doOneSplit(context is Context, topLevelId is Id, trimId is Id, targets is Query, tool is Query, useToolExtents is boolean, keepType is SplitOperationKeepType)
{
    callSubfeatureAndProcessStatus(topLevelId, opSplitPart, context, trimId, {
                "targets" : targets,
                "tool" : tool,
                "useTrimmed" : useToolExtents,
                "keepType" : keepType
            }, {
                "overrideStatus" : ErrorStringEnum.FRAME_TRIM_FAILED,
                "propagateErrorDisplay" : true,
                "additionalErrorEntities" : targets
            });
}

function deleteBodies(context is Context, topLevelId is Id, bodiesToDelete is box)
{
    if (bodiesToDelete[] != [])
    {
        opDeleteBodies(context, topLevelId + "cleanup", {
                    "entities" : qUnion(bodiesToDelete[])
                });
    }
}

function gatherBodiesForDeletion(context is Context, allBodies is Query, keptBody is Query, bodiesToDelete is box)
{
    const leftOverBodies = evaluateQuery(context, qSubtraction(allBodies, keptBody));
    bodiesToDelete[] = concatenateArrays([bodiesToDelete[], leftOverBodies]);
}

function validateGroups(context is Context, frameGroups is array)
{
    const numFrameGroups = size(frameGroups);
    verify(numFrameGroups >= 2, ErrorStringEnum.FRAME_TRIM_GROUPS);

    //error out if there's overlap between groups
    for (var i = 0; i < numFrameGroups; i += 1)
    {
        for (var j = i + 1; j < numFrameGroups; j += 1)
        {
            const intersectionQuery = qIntersection([frameGroups[i].frames, frameGroups[j].frames]);
            verify(isQueryEmpty(context, intersectionQuery), ErrorStringEnum.FRAME_DISJOINT_GROUPS, {
                        "entities" : intersectionQuery
                    });
        }
    }
}

function aggregateTools(frameGroups is array) returns array
{
    const numBooleans = size(frameGroups) - 1;
    var toolGroups = makeArray(numBooleans);
    toolGroups[0] = [frameGroups[0].frames];
    for (var i = 1; i < numBooleans; i += 1)
    {
        toolGroups[i] = append(toolGroups[i - 1], frameGroups[i].frames);
    }
    return toolGroups;
}

/**
 * @internal
 * Manipulator to keep front/back side of split
 */
function addKeepSideManipulator(context is Context, id is Id, definition is map)
{
    if (isQueryEmpty(context, definition.targets) || (isQueryEmpty(context, definition.tool)))
    {
        return;
    }

    var toolFaces;
    if (!isQueryEmpty(context, qOwnedByBody(definition.tool, EntityType.FACE)))
    {
        toolFaces = qOwnedByBody(definition.tool, EntityType.FACE);
    }
    else if (!isQueryEmpty(context, qEntityFilter(definition.tool, EntityType.FACE)))
    {
        toolFaces = qEntityFilter(definition.tool, EntityType.FACE);
    }
    else
    {
        return;
    }

    const distResult = try silent(evDistance(context, {
                    "side0" : toolFaces,
                    "side1" : definition.targets
                }));
    if (distResult == undefined)
    {
        return;
    }
    const tangentPlane = evFaceTangentPlane(context, {
                "face" : qNthElement(toolFaces, distResult.sides[0].index),
                "parameter" : distResult.sides[0].parameter
            });
    var manipulator is Manipulator = flipManipulator({
            "base" : tangentPlane.origin,
            "direction" : tangentPlane.normal,
            "flipped" : definition.flipTrim
        });
    addManipulators(context, id, {
                "flipManipulator" : manipulator
            });
}

/**
 * @internal
 */
export function frameTrimManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    for (var manipulator in newManipulators)
    {
        if (manipulator.key == "flipManipulator")
        {
            definition.flipTrim = manipulator.value.flipped;
            return definition;
        }
    }
}

