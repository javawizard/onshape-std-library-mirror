FeatureScript 244; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/boundingtype.gen.fs", version : "");
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/tool.fs", version : "");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "");
import(path : "onshape/std/box.fs", version : "");
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/curveGeometry.fs", version : "");
import(path : "onshape/std/draft.fs", version : "");
import(path : "onshape/std/evaluate.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/mathUtils.fs", version : "");
import(path : "onshape/std/surfaceGeometry.fs", version : "");
import(path : "onshape/std/valueBounds.fs", version : "");

/**
 * TODO: description
 */
export enum SecondDirectionBoundingType
{
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Up to next" }
    UP_TO_NEXT,
    annotation { "Name" : "Up to face" }
    UP_TO_SURFACE,
    annotation { "Name" : "Up to part" }
    UP_TO_BODY,
    annotation { "Name" : "Through all" }
    THROUGH_ALL
}

/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
annotation { "Feature Type Name" : "Extrude", "Manipulator Change Function" : "extrudeManipulatorChange", "Filter Selector" : "allparts" }
export const extrude = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Creation type" }
        definition.bodyType is ToolBodyType;

        if (definition.bodyType != ToolBodyType.SURFACE)
        {
            booleanStepTypePredicate(definition);
        }

        if (definition.bodyType != ToolBodyType.SURFACE)
        {
            annotation { "Name" : "Faces and sketch regions to extrude",
                         "Filter" : (EntityType.FACE && GeometryType.PLANE)
                            && ConstructionObject.NO }
            definition.entities is Query;
        }
        else
        {
            annotation { "Name" : "Sketch curves to extrude",
                         "Filter" : (EntityType.EDGE && SketchObject.YES && ConstructionObject.NO ) }
            definition.surfaceEntities is Query;
        }

        annotation { "Name" : "End type" }
        definition.endBound is BoundingType;

        if (definition.endBound != BoundingType.SYMMETRIC)
        {
            annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
            definition.oppositeDirection is boolean;
        }

        if (definition.endBound == BoundingType.UP_TO_SURFACE)
        {
            annotation { "Name" : "Up to face",
                "Filter" : EntityType.FACE && SketchObject.NO,
                "MaxNumberOfPicks" : 1 }
            definition.endBoundEntityFace is Query;
        }
        else if (definition.endBound == BoundingType.UP_TO_BODY)
        {
            annotation { "Name" : "Up to surface or part",
                         "Filter" : EntityType.BODY && SketchObject.NO,
                         "MaxNumberOfPicks" : 1 }
            definition.endBoundEntityBody is Query;
        }

        if (definition.endBound == BoundingType.BLIND ||
            definition.endBound == BoundingType.SYMMETRIC)
        {
            annotation { "Name" : "Depth" }
            isLength(definition.depth, LENGTH_BOUNDS);
        }

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            annotation { "Name" : "Draft", "UIHint" : "DISPLAY_SHORT" }
            definition.hasDraft is boolean;

            if (definition.hasDraft == true)
            {
                annotation { "Name" : "Draft angle", "UIHint" : "DISPLAY_SHORT" }
                isAngle(definition.draftAngle, ANGLE_STRICT_90_BOUNDS);

                annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
                definition.draftPullDirection is boolean;
            }
        }

        if (definition.endBound != BoundingType.SYMMETRIC)
        {
            annotation { "Name" : "Second end position" }
            definition.hasSecondDirection is boolean;

            if (definition.hasSecondDirection)
            {
                annotation { "Name" : "End type" }
                definition.secondDirectionBound is SecondDirectionBoundingType;

                annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION", "Default" : true }
                definition.secondDirectionOppositeDirection is boolean;

                if (definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_SURFACE)
                {
                    annotation { "Name" : "Up to face",
                        "Filter" : EntityType.FACE && SketchObject.NO,
                        "MaxNumberOfPicks" : 1 }
                    definition.secondDirectionBoundEntityFace is Query;
                }
                else if (definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_BODY)
                {
                    annotation { "Name" : "Up to surface or part",
                                 "Filter" : EntityType.BODY && SketchObject.NO,
                                 "MaxNumberOfPicks" : 1 }
                    definition.secondDirectionBoundEntityBody is Query;
                }

                if (definition.secondDirectionBound == SecondDirectionBoundingType.BLIND)
                {
                    annotation { "Name" : "Depth" }
                    isLength(definition.secondDirectionDepth, LENGTH_BOUNDS);
                }

                if (definition.bodyType == ToolBodyType.SOLID &&
                    ((definition.secondDirectionOppositeDirection && !definition.oppositeDirection) ||
                     (!definition.secondDirectionOppositeDirection && definition.oppositeDirection)))
                {
                    annotation { "Name" : "Draft", "UIHint" : "DISPLAY_SHORT" }
                    definition.hasSecondDirectionDraft is boolean;

                    if (definition.hasSecondDirectionDraft)
                    {
                        annotation { "Name" : "Draft angle", "UIHint" : "DISPLAY_SHORT" }
                        isAngle(definition.secondDirectionDraftAngle, ANGLE_STRICT_90_BOUNDS);

                        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
                        definition.secondDirectionDraftPullDirection is boolean;
                    }
                }
            }
        }
        if (definition.bodyType != ToolBodyType.SURFACE)
        {
            booleanStepScopePredicate(definition);
        }
    }
    {
        definition.entities = getEntitiesToUse(context, definition);

        // compute the draftCondition before definition gets changed.
        const draftCondition is map = getDraftConditions(definition);

        const resolvedEntities = evaluateQuery(context, definition.entities);
        if (size(resolvedEntities) == 0)
        {
            if (definition.bodyType == ToolBodyType.SOLID)
                throw regenError(ErrorStringEnum.EXTRUDE_NO_SELECTED_REGION, ["entities"]);
            else
                throw regenError(ErrorStringEnum.EXTRUDE_SURF_NO_CURVE, ["surfaceEntities"]);
        }

        // ------------- Get the extrude axis ---------------
        const extrudeAxis = try(computeExtrudeAxis(context, resolvedEntities[0]));
        if (extrudeAxis == undefined)
            throw regenError(ErrorStringEnum.EXTRUDE_NO_DIRECTION);

        // Add manipulator
        addExtrudeManipulator(context, id, definition, extrudeAxis);

        // ------------- Determine the direction ---------------

        definition.direction = extrudeAxis.direction;

        if (definition.oppositeDirection)
            definition.direction *= -1;

        if (definition.depth != undefined && definition.depth < 0)
        {
            definition.depth *= -1;
            if (definition.endBound == BoundingType.BLIND)
                definition.direction *= -1;
        }

        definition.isStartBoundOpposite = false;

        // ------------- Determine the bounds ---------------

        if (definition.endBound == BoundingType.UP_TO_SURFACE)
        {
            definition.endBoundEntity = definition.endBoundEntityFace;
        }
        else if (definition.endBound == BoundingType.UP_TO_BODY)
        {
            definition.endBoundEntity = definition.endBoundEntityBody;
        }

        definition.startBound = BoundingType.BLIND;
        definition.startDepth = 0;
        definition.endDepth = definition.depth;

        if (definition.endBound == BoundingType.SYMMETRIC)
        {
            definition.endBound = BoundingType.BLIND;
            definition.startDepth = definition.depth * -0.5;
            definition.endDepth = definition.depth * 0.5;
        }
        else if (definition.hasSecondDirection)
        {
            // ------------- Check the second direction ---------------

            if (definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_SURFACE)
            {
                definition.secondDirectionBoundEntity = definition.secondDirectionBoundEntityFace;
            }
            else if (definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_BODY)
            {
                definition.secondDirectionBoundEntity = definition.secondDirectionBoundEntityBody;
            }

            definition.startBound = definition.secondDirectionBound as BoundingType;
            definition.startBoundEntity = definition.secondDirectionBoundEntity;

            if (definition.secondDirectionDepth != undefined &&
                definition.secondDirectionDepth < 0)
            {
                definition.secondDirectionDepth *= -1;
            }

            definition.startDepth = definition.secondDirectionDepth;
            if (definition.secondDirectionOppositeDirection != definition.oppositeDirection)
                definition.isStartBoundOpposite = true;
        }

        // ------------- Perform the operation ---------------

        extrudeWithDraft(context, id, definition, draftCondition);

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            const reconstructOp = function(id) { extrudeWithDraft(context, id, definition, draftCondition); };
            processNewBodyIfNeeded(context, id, definition, reconstructOp);
        }

    }, { endBound : BoundingType.BLIND, oppositeDirection : false,
            bodyType : ToolBodyType.SOLID, operationType : NewBodyOperationType.NEW,
            secondDirectionBound : SecondDirectionBoundingType.BLIND,
            secondDirectionOppositeDirection : true, hasSecondDirection : false,
            hasDraft: false, hasSecondDirectionDraft: false,
            draftPullDirection : false, secondDirectionDraftPullDirection : false });

predicate supportsDraft(definition is map)
{
    definition.bodyType == ToolBodyType.SOLID;
}

predicate firstDirectionNeedsDraft(definition is map)
{
    supportsDraft(definition);
    definition.hasDraft;
}

predicate symmetricNeedsDraft(definition is map)
{
    firstDirectionNeedsDraft(definition);
    definition.endBound == BoundingType.SYMMETRIC;
}

predicate hasSecondDirectionExtrude(definition is map)
{
    definition.endBound != BoundingType.SYMMETRIC;
    definition.hasSecondDirection;
}

predicate secondDirectionNeedsDraft(definition is map)
{
    supportsDraft(definition);
    hasSecondDirectionExtrude(definition);
    definition.secondDirectionOppositeDirection != definition.oppositeDirection;
    definition.hasSecondDirectionDraft;
}

predicate needsSplit(definition is map)
{
    definition.endBound == BoundingType.SYMMETRIC || (hasSecondDirectionExtrude(definition) && definition.secondDirectionOppositeDirection != definition.oppositeDirection);
}

function extrudeWithDraft(context is Context, id is Id, definition is map, draftCondition is map)
{
    opExtrude(context, id, definition);

    if (draftCondition.firstDirectionNeedsDraft || draftCondition.secondDirectionNeedsDraft)
    {
        const extrudeBodies = qCreatedBy(id, EntityType.BODY);
        const extrudeBodyArray = evaluateQuery(context, extrudeBodies);
        for (var i = 0; i < size(extrudeBodyArray); i += 1)
        {
            draftExtrudeBody(context, id, i, definition, extrudeBodyArray[i], draftCondition);
        }
    }
}

function getDraftConditions(definition is map)
{
    return
    {
        "secondDirectionNeedsDraft" : secondDirectionNeedsDraft(definition),
        "symmetricNeedsDraft" : symmetricNeedsDraft(definition),
        "firstDirectionNeedsDraft" : firstDirectionNeedsDraft(definition),
        "needsSplit" : needsSplit(definition)
    };
}

function applyDraft(context is Context, draftId is Id, draftFaces is Query,
                    draftDefinition is map, referenceFace is Query, neutralPlane is Plane)
{
    draftDefinition.tangentPropagation = false;
    draftDefinition.reFillet = false;
    draftDefinition.draftFaces = draftFaces;
    draftDefinition.neutralPlane = referenceFace;
    draftDefinition.pullVec = neutralPlane.normal;
    if (!draftDefinition.pullDirection)
    {
        draftDefinition.pullVec = -draftDefinition.pullVec;
    }

    opDraft(context, draftId, draftDefinition);
}

function createNeutralPlane(context is Context, id is Id, extrudeBody is Query, direction is Vector) returns Plane
{
    const dependencies = evaluateQuery(context, qDependency(extrudeBody));
    if (size(dependencies) == 0)
        throw "Cannot find any dependency for the extruded body";
    var neutralPlane;
    try
    {
        const line = evEdgeTangentLine(context, { "edge" : dependencies[0], "parameter" : 0.5 });
        neutralPlane = plane(line.origin, direction);
    }
    catch
    {
        const facePlane = evFaceTangentPlane(context, { "face" : dependencies[0], "parameter" : vector(0.5, 0.5) });
        neutralPlane = plane(facePlane.origin, direction);
    }

    // reference face
    const splitPlaneDefinition = { "plane" : neutralPlane, "size" : 1 * meter };
    opPlane(context, id, splitPlaneDefinition);
    return neutralPlane;
}

function draftExtrudeBody(context is Context, id is Id, suffix is number, definition is map, extrudeBody is Query, conditions is map)
{
    const neutralPlaneId = id + ("neutralPlane" ~ suffix);
    const neutralPlane = try(createNeutralPlane(context, neutralPlaneId, extrudeBody, definition.direction));

    if (neutralPlane == undefined)
        throw regenError(ErrorStringEnum.DRAFT_SELECT_NEUTRAL, ["neutralPlane" ~ suffix]);

    const neutralPlaneFace = qCreatedBy(neutralPlaneId, EntityType.FACE);
    const neutralPlaneQuery = qOwnerBody(qCreatedBy(neutralPlaneId));

    if (conditions.needsSplit)
    {
        // Split the body
        const splitId = id + ("split" ~ suffix);
        opSplitPart(context, splitId, { targets: extrudeBody, tool: neutralPlaneQuery, keepTools: false });

        const firstBodies = qSplitBy(splitId, EntityType.BODY, false);
        const secondBodies = qSplitBy(splitId, EntityType.BODY, true);
        const firstBodyArray = evaluateQuery(context, firstBodies);
        const secondBodyArray = evaluateQuery(context, secondBodies);
        if (size(firstBodyArray) != size(secondBodyArray))
            throw regenError(ErrorStringEnum.SPLIT_INVALID_INPUT, ["split" ~ suffix]);

        const draftFirstId = id + ("draftFirst" ~ suffix);
        const draftSecondId = id + ("draftSecond" ~ suffix);

        const firstFaces = qSplitBy(splitId, EntityType.FACE, false);
        const secondFaces = qSplitBy(splitId, EntityType.FACE, true);

        var draftFirstDefinition = undefined;
        var draftSecondDefinition = undefined;
        // Apply draft
        if (conditions.symmetricNeedsDraft)
        {
            draftFirstDefinition = { "angle" : definition.draftAngle,
                "pullDirection" : definition.draftPullDirection };
            draftSecondDefinition = { "angle" : definition.draftAngle,
                "pullDirection" : !definition.draftPullDirection };
        }
        else
        {
            if (conditions.firstDirectionNeedsDraft)
            {
                draftFirstDefinition = { "angle" : definition.draftAngle,
                    "pullDirection" : definition.draftPullDirection };
            }
            if (conditions.secondDirectionNeedsDraft)
            {
                draftSecondDefinition = { "angle" : definition.secondDirectionDraftAngle,
                    "pullDirection" : !definition.secondDirectionDraftPullDirection };
            }
        }

        if (draftFirstDefinition != undefined)
            applyDraft(context, draftFirstId, firstFaces, draftFirstDefinition, neutralPlaneFace, neutralPlane);
        if (draftSecondDefinition != undefined)
            applyDraft(context, draftSecondId, secondFaces, draftSecondDefinition, neutralPlaneFace, neutralPlane);

        const toolsQ = qUnion([firstBodies, secondBodies]);
        booleanBodies(context, id + ("booleanUnion" ~ suffix),
                      { "operationType" : BooleanOperationType.UNION, "tools" : toolsQ });
    }
    else
    {
        const draftDefinition = { "angle" : definition.draftAngle,
                                  "pullDirection" : definition.draftPullDirection };
        // TODO: replace this with the merged query enum
        const draftFaces = qOwnedByBody(makeQuery(id, "SWEPT_FACE", EntityType.FACE, {}), extrudeBody);
        applyDraft(context, id + ("draft" ~ suffix), draftFaces, draftDefinition, neutralPlaneFace, neutralPlane);
    }

    opDeleteBodies(context, id + ("deleteNeutralPlane" ~ suffix), { "entities" : qCreatedBy(neutralPlaneId) });
}

function getEntitiesToUse(context is Context, definition is map)
{
    if (definition.bodyType == ToolBodyType.SOLID)
    {
        return definition.entities;
    }
    else
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V177_CONSTRUCTION_OBJECT_FILTER))
        {
            return qConstructionFilter(definition.surfaceEntities, ConstructionObject.NO);
        }
        else
        {
            return definition.surfaceEntities;
        }
    }
}

function computeExtrudeAxis(context is Context, entity is Query)
{
    var planes = evaluateQuery(context, qGeometry(entity, GeometryType.PLANE));
    if (size(planes) == 1)
    {
        const entityPlane = evPlane(context, { "face" : entity });
        return line(entityPlane.origin, entityPlane.normal);
    }
    //The extrude axis should start in the middle of the edge and point in the sketch plane normal
    const tangentAtEdge = evEdgeTangentLine(context, { "edge" : entity, "parameter" : 0.5 });
    const entityPlane = evOwnerSketchPlane(context, { "entity" : entity });
    return line(tangentAtEdge.origin, entityPlane.normal);
}

// Manipulator functions

const DEPTH_MANIPULATOR = "depthManipulator";
const FLIP_MANIPULATOR = "flipManipulator";
const SECOND_DEPTH_MANIPULATOR = "secondDirectionDepthManipulator";
const SECOND_FLIP_MANIPULATOR = "secondDirectionFlipManipulator";

function addExtrudeManipulator(context is Context, id is Id, definition is map, extrudeAxis is Line)
{
    const usedEntities = getEntitiesToUse(context, definition);

    if (definition.endBound != BoundingType.BLIND && definition.endBound != BoundingType.SYMMETRIC)
    {
        addManipulators(context, id, { (FLIP_MANIPULATOR) :
                        flipManipulator(extrudeAxis.origin,
                                        extrudeAxis.direction,
                                        definition.oppositeDirection,
                                        usedEntities) });
    }
    else
    {
        var offset = definition.depth;
        if (definition.endBound == BoundingType.SYMMETRIC)
            offset *= 0.5;
        if (definition.oppositeDirection)
            offset *= -1;
        addManipulators(context, id, { (DEPTH_MANIPULATOR) :
                        linearManipulator(extrudeAxis.origin,
                            extrudeAxis.direction,
                            offset,
                            usedEntities) });
    }

    if (definition.hasSecondDirection && definition.endBound != BoundingType.SYMMETRIC)
    {
        if (definition.secondDirectionBound != SecondDirectionBoundingType.BLIND)
        {
            addManipulators(context, id, { (SECOND_FLIP_MANIPULATOR) :
                            flipManipulator(extrudeAxis.origin,
                                            extrudeAxis.direction,
                                            definition.secondDirectionOppositeDirection,
                                            usedEntities,
                                            ManipulatorStyleEnum.SECONDARY) });
        }
        else
        {
            var secondDirectionOffset = definition.secondDirectionDepth;
            if (definition.secondDirectionOppositeDirection == true)
                secondDirectionOffset *= -1;
            addManipulators(context, id, { (SECOND_DEPTH_MANIPULATOR) :
                            linearManipulator(extrudeAxis.origin,
                                              extrudeAxis.direction,
                                              secondDirectionOffset,
                                              usedEntities,
                                              ManipulatorStyleEnum.SECONDARY) });
        }
    }
}

/**
 * TODO: description
 * @param context
 * @param definition {{
 *      @field TODO
 * }}
 * @param newManipulators {{
 *      @field TODO
 * }}
 */
export function extrudeManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    if (newManipulators[DEPTH_MANIPULATOR] is map &&
        (definition.endBound == BoundingType.BLIND ||
         definition.endBound == BoundingType.SYMMETRIC))
    {
        var newOffset = newManipulators[DEPTH_MANIPULATOR].offset;
        if (definition.endBound == BoundingType.SYMMETRIC)
            newOffset *= 2;
        definition.oppositeDirection = newOffset < 0 * meter;
        definition.depth = abs(newOffset);
    }
    else if (newManipulators[FLIP_MANIPULATOR] is map &&
             definition.endBound != BoundingType.BLIND &&
             definition.endBound != BoundingType.SYMMETRIC)
    {
        definition.oppositeDirection = newManipulators[FLIP_MANIPULATOR].flipped;
    }

    if (definition.hasSecondDirection && definition.endBound != BoundingType.SYMMETRIC)
    {
        if (newManipulators[SECOND_DEPTH_MANIPULATOR] is map &&
            definition.secondDirectionBound == SecondDirectionBoundingType.BLIND)
        {
            const newSecondDirectionOffset = newManipulators[SECOND_DEPTH_MANIPULATOR].offset;
            definition.secondDirectionOppositeDirection = newSecondDirectionOffset < 0 * meter;
            definition.secondDirectionDepth = abs(newSecondDirectionOffset);
        }
        else if (newManipulators[SECOND_FLIP_MANIPULATOR] is map &&
            definition.secondDirectionBound != SecondDirectionBoundingType.BLIND)
        {
            definition.secondDirectionOppositeDirection = newManipulators[SECOND_FLIP_MANIPULATOR].flipped;
        }
    }

    return definition;
}

function shouldFlipExtrudeDirection(context is Context, endBound is BoundingType,
    endBoundEntity is Query,
    extrudeAxis is Line)
{
    if (endBound != BoundingType.UP_TO_SURFACE &&
        endBound != BoundingType.UP_TO_BODY)
        return false;

    if (endBound == BoundingType.UP_TO_SURFACE)
    {
        const refPlane = try(evPlane(context, { "face" : endBoundEntity }));
        if (refPlane == undefined)
            return false; //err on side of not flipping, TODO: surfaceXline
        const isecResult = intersection(refPlane, extrudeAxis);
        if (isecResult == undefined || isecResult.dim != 0)
            return false;

        const dotPr = stripUnits(dot(isecResult.intersection - extrudeAxis.origin, extrudeAxis.direction));
        return dotPr < -TOLERANCE.zeroLength;
    }
    const pln = plane(extrudeAxis.origin, extrudeAxis.direction);
    const boxResult = try(evBox3d(context, { 'topology' : endBoundEntity, 'cSys' : planeToCSys(pln) }));
    if (boxResult == undefined)
        return false;

    return (stripUnits(boxResult.minCorner[2]) < -TOLERANCE.zeroLength &&
            stripUnits(boxResult.maxCorner[2]) < TOLERANCE.zeroLength);
}


/**
 * TODO: description
 * @param context
 * @param featureDefinition {{
 *      @field TODO
 * }}
 * @param featureInfo {{
 *      @field TODO
 * }}
 */
export function upToBoundaryFlip(context is Context, featureDefinition is map, featureInfo is map) returns map
{
    const usedEntities = getEntitiesToUse(context, featureDefinition);
    const resolvedEntities = evaluateQuery(context, usedEntities);
    if (@size(resolvedEntities) == 0)
    {
        return featureDefinition;
    }
    const extrudeAxis = computeExtrudeAxis(context, resolvedEntities[0]);
    if (extrudeAxis == undefined)
    {
        return featureDefinition;
    }
    var direction = extrudeAxis.direction;
    if (featureDefinition.oppositeDirection == true)
        direction *= -1;
    var endBoundEntity;
    if (featureDefinition.endBound == BoundingType.UP_TO_SURFACE)
    {
        endBoundEntity = featureDefinition.endBoundEntityFace;
    }
    else if (featureDefinition.endBound == BoundingType.UP_TO_BODY)
    {
        endBoundEntity = featureDefinition.endBoundEntityBody;
    }
    if (endBoundEntity is Query &&
        shouldFlipExtrudeDirection(context,
                                   featureDefinition.endBound,
                                   endBoundEntity,
                                   line(extrudeAxis.origin, direction)))
    {
        featureDefinition.oppositeDirection = (featureDefinition.oppositeDirection == true) ? false : true;
    }
    return featureDefinition;
}

/**
 * TODO: description
 * @param context
 * @param featureDefinition {{
 *      @field TODO
 * }}
 * @param featureInfo {{
 *      @field TODO
 * }}
 */
export function performTypeFlip(context is Context, featureDefinition is map, featureInfo is map) returns map
{
    featureDefinition.oppositeDirection = (featureDefinition.oppositeDirection == true) ? false : true;
    return featureDefinition;
}

