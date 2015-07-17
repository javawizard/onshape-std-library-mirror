FeatureScript 172; /* Automatically generated version */
export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/feature.fs", version : "");
export import(path : "onshape/std/boolean.fs", version : "");
export import(path : "onshape/std/manipulator.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/manipulatorstyleenum.gen.fs", version : "");

export enum BoundingType
{
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Symmetric" }
    SYMMETRIC,
    annotation { "Name" : "Up to next" }
    UP_TO_NEXT,
    annotation { "Name" : "Up to surface" }
    UP_TO_SURFACE,
    annotation { "Name" : "Up to part" }
    UP_TO_BODY,
    annotation { "Name" : "Through all" }
    THROUGH_ALL
}

export enum SecondDirectionBoundingType
{
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Up to next" }
    UP_TO_NEXT,
    annotation { "Name" : "Up to surface" }
    UP_TO_SURFACE,
    annotation { "Name" : "Up to part" }
    UP_TO_BODY,
    annotation { "Name" : "Through all" }
    THROUGH_ALL
}

//Extrude Feature
annotation { "Feature Type Name" : "Extrude", "Manipulator Change Function" : "extrudeManipulatorChange", "Filter Selector" : "allparts" }
export const extrude = defineFeature(function(context is Context, id is Id, extrudeDefinition is map)
    precondition
    {
        annotation { "Name" : "Creation type" }
        extrudeDefinition.bodyType is ToolBodyType;

        if (extrudeDefinition.bodyType != ToolBodyType.SURFACE)
        {
            booleanStepTypePredicate(extrudeDefinition);
        }

        if (extrudeDefinition.bodyType != ToolBodyType.SURFACE)
        {
            annotation { "Name" : "Faces and sketch regions to extrude",
                         "Filter" : (EntityType.FACE && GeometryType.PLANE)
                            && ConstructionObject.NO }
            extrudeDefinition.entities is Query;
        }
        else
        {
            annotation { "Name" : "Sketch curves to extrude",
                         "Filter" : (EntityType.EDGE && SketchObject.YES) }
            extrudeDefinition.surfaceEntities is Query;
        }

        annotation { "Name" : "End type" }
        extrudeDefinition.endBound is BoundingType;

        if (extrudeDefinition.endBound != BoundingType.SYMMETRIC)
        {
            annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
            extrudeDefinition.oppositeDirection is boolean;
        }

        if (extrudeDefinition.endBound == BoundingType.UP_TO_SURFACE ||
            extrudeDefinition.endBound == BoundingType.UP_TO_BODY)
        {
            annotation { "Name" : "Up to face, surface, or part",
                         "Filter" : (EntityType.FACE || EntityType.BODY) && SketchObject.NO,
                         "MaxNumberOfPicks" : 1 }
            extrudeDefinition.endBoundEntity is Query;
        }

        if (extrudeDefinition.endBound == BoundingType.BLIND ||
            extrudeDefinition.endBound == BoundingType.SYMMETRIC)
        {
            annotation { "Name" : "Depth" }
            isLength(extrudeDefinition.depth, LENGTH_BOUNDS);
        }

        if (extrudeDefinition.endBound != BoundingType.SYMMETRIC)
        {
            annotation { "Name" : "Second end position" }
            extrudeDefinition.hasSecondDirection is boolean;

            if (extrudeDefinition.hasSecondDirection)
            {
                annotation { "Name" : "End type" }
                extrudeDefinition.secondDirectionBound is SecondDirectionBoundingType;

                annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION", "Default" : true }
                extrudeDefinition.secondDirectionOppositeDirection is boolean;

                if (extrudeDefinition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_SURFACE ||
                    extrudeDefinition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_BODY)
                {
                    annotation { "Name" : "Up to face, surface, or part",
                                 "Filter" : (EntityType.FACE || EntityType.BODY) && SketchObject.NO,
                                 "MaxNumberOfPicks" : 1 }
                    extrudeDefinition.secondDirectionBoundEntity is Query;
                }

                if (extrudeDefinition.secondDirectionBound == SecondDirectionBoundingType.BLIND)
                {
                    annotation { "Name" : "Depth" }
                    isLength(extrudeDefinition.secondDirectionDepth, LENGTH_BOUNDS);
                }
            }
        }
        if (extrudeDefinition.bodyType != ToolBodyType.SURFACE)
        {
            booleanStepScopePredicate(extrudeDefinition);
        }
    }
    {
        extrudeDefinition.entities = getEntitiesToUse(extrudeDefinition);

        var resolvedEntities = evaluateQuery(context, extrudeDefinition.entities);
        if (@size(resolvedEntities) == 0)
        {
            if (extrudeDefinition.bodyType == ToolBodyType.SOLID)
                reportFeatureError(context, id, ErrorStringEnum.EXTRUDE_NO_SELECTED_REGION, ["entities"]);
            else
                reportFeatureError(context, id, ErrorStringEnum.EXTRUDE_SURF_NO_CURVE, ["surfaceEntities"]);
            return;
        }

        // ------------- Get the extrude axis ---------------
        var extrudeAxis = computeExtrudeAxis(context, resolvedEntities[0]);
        if (extrudeAxis == undefined)
        {
            reportFeatureError(context, id, ErrorStringEnum.EXTRUDE_NO_DIRECTION);
            return;
        }

        // Add manipulator
        addExtrudeManipulator(context, id, extrudeDefinition, extrudeAxis);

        // ------------- Determine the direction ---------------

        extrudeDefinition.direction = extrudeAxis.direction;

        if (extrudeDefinition.oppositeDirection)
            extrudeDefinition.direction *= -1;

        if (extrudeDefinition.depth != undefined && extrudeDefinition.depth < 0 * meter)
        {
            extrudeDefinition.depth *= -1;
            if (extrudeDefinition.endBound == BoundingType.BLIND)
                extrudeDefinition.direction *= -1;
        }

        extrudeDefinition.isStartBoundOpposite = false;

        // ------------- Determine the bounds ---------------
        extrudeDefinition.startBound = BoundingType.BLIND;
        extrudeDefinition.startDepth = 0;
        extrudeDefinition.endDepth = extrudeDefinition.depth;

        if (extrudeDefinition.endBound == BoundingType.SYMMETRIC)
        {
            extrudeDefinition.endBound = BoundingType.BLIND;
            extrudeDefinition.startDepth = extrudeDefinition.depth * -0.5;
            extrudeDefinition.endDepth = extrudeDefinition.depth * 0.5;
        }
        else if (extrudeDefinition.hasSecondDirection)
        {
            // ------------- Check the second direction ---------------
            extrudeDefinition.startBound = extrudeDefinition.secondDirectionBound as BoundingType;
            extrudeDefinition.startBoundEntity = extrudeDefinition.secondDirectionBoundEntity;

            if (extrudeDefinition.secondDirectionDepth != undefined &&
                extrudeDefinition.secondDirectionDepth < 0 * meter)
            {
                extrudeDefinition.secondDirectionDepth *= -1;
            }

            extrudeDefinition.startDepth = extrudeDefinition.secondDirectionDepth;
            if (extrudeDefinition.secondDirectionOppositeDirection != extrudeDefinition.oppositeDirection)
                extrudeDefinition.isStartBoundOpposite = true;
        }

        // ------------- Perform the operation ---------------

        opExtrude(context, id, extrudeDefinition);

        if (extrudeDefinition.bodyType == ToolBodyType.SOLID)
        {
            if (!processNewBodyIfNeeded(context, id, extrudeDefinition))
            {
                var statusToolId = id + "statusTools";
                startFeature(context, statusToolId, extrudeDefinition);
                opExtrude(context, statusToolId, extrudeDefinition);
                setBooleanErrorEntities(context, id, statusToolId);
                endFeature(context, statusToolId);
            }
        }
    }, { endBound : BoundingType.BLIND, oppositeDirection : false,
            bodyType : ToolBodyType.SOLID, operationType : NewBodyOperationType.NEW,
            secondDirectionBound : SecondDirectionBoundingType.BLIND,
            secondDirectionOppositeDirection : true, hasSecondDirection : false });

function getEntitiesToUse(extrudeDefinition is map)
{
    if (extrudeDefinition.bodyType == ToolBodyType.SOLID)
    {
        return extrudeDefinition.entities;
    }
    else
    {
        return extrudeDefinition.surfaceEntities;
    }
}

function computeExtrudeAxis(context is Context, entity is Query)
{
    var entityPlane = evPlane(context, { "face" : entity });
    if (entityPlane.result != undefined)
        return line(entityPlane.result.origin, entityPlane.result.normal);

    //The extrude axis should start in the middle of the edge and point in the sketch plane normal
    var tangentAtEdge = evEdgeTangentLine(context, { "edge" : entity, "parameter" : 0.5 });
    if (tangentAtEdge.result == undefined)
        return undefined;

    entityPlane = evOwnerSketchPlane(context, { "entity" : entity });
    if (entityPlane.result == undefined)
        return undefined;

    return line(tangentAtEdge.result.origin, entityPlane.result.normal);
}

// Manipulator functions

const DEPTH_MANIPULATOR = "depthManipulator";
const FLIP_MANIPULATOR = "flipManipulator";
const SECOND_DEPTH_MANIPULATOR = "secondDirectionDepthManipulator";
const SECOND_FLIP_MANIPULATOR = "secondDirectionFlipManipulator";

function addExtrudeManipulator(context is Context, id is Id, extrudeDefinition is map, extrudeAxis is Line)
{
    var usedEntities = getEntitiesToUse(extrudeDefinition);

    if (extrudeDefinition.endBound != BoundingType.BLIND && extrudeDefinition.endBound != BoundingType.SYMMETRIC)
    {
        addManipulators(context, id, { (FLIP_MANIPULATOR) :
                        flipManipulator(extrudeAxis.origin,
                                        extrudeAxis.direction,
                                        extrudeDefinition.oppositeDirection,
                                        usedEntities) });
    }
    else
    {
        var offset = extrudeDefinition.depth;
        if (extrudeDefinition.endBound == BoundingType.SYMMETRIC)
            offset *= 0.5;
        if (extrudeDefinition.oppositeDirection == true)
            offset *= -1;
        addManipulators(context, id, { (DEPTH_MANIPULATOR) :
                        linearManipulator(extrudeAxis.origin,
                            extrudeAxis.direction,
                            offset,
                            usedEntities) });
    }

    if (extrudeDefinition.hasSecondDirection && extrudeDefinition.endBound != BoundingType.SYMMETRIC)
    {
        if (extrudeDefinition.secondDirectionBound != SecondDirectionBoundingType.BLIND)
        {
            addManipulators(context, id, { (SECOND_FLIP_MANIPULATOR) :
                            flipManipulator(extrudeAxis.origin,
                                            extrudeAxis.direction,
                                            extrudeDefinition.secondDirectionOppositeDirection,
                                            usedEntities,
                                            ManipulatorStyleEnum.SECONDARY) });
        }
        else
        {
            var secondDirectionOffset = extrudeDefinition.secondDirectionDepth;
            if (extrudeDefinition.secondDirectionOppositeDirection == true)
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

export function extrudeManipulatorChange(context is Context, extrudeDefinition is map, newManipulators is map) returns map
{
    if (newManipulators[DEPTH_MANIPULATOR] is map &&
        (extrudeDefinition.endBound == BoundingType.BLIND ||
         extrudeDefinition.endBound == BoundingType.SYMMETRIC))
    {
        var newOffset = newManipulators[DEPTH_MANIPULATOR].offset;
        if (extrudeDefinition.endBound == BoundingType.SYMMETRIC)
            newOffset *= 2;
        extrudeDefinition.oppositeDirection = newOffset < 0 * meter;
        extrudeDefinition.depth = abs(newOffset);
    }
    else if (newManipulators[FLIP_MANIPULATOR] is map &&
             extrudeDefinition.endBound != BoundingType.BLIND &&
             extrudeDefinition.endBound != BoundingType.SYMMETRIC)
    {
        extrudeDefinition.oppositeDirection = newManipulators[FLIP_MANIPULATOR].flipped;
    }

    if (extrudeDefinition.hasSecondDirection && extrudeDefinition.endBound != BoundingType.SYMMETRIC)
    {
        if (newManipulators[SECOND_DEPTH_MANIPULATOR] is map &&
            extrudeDefinition.secondDirectionBound == SecondDirectionBoundingType.BLIND)
        {
            var newSecondDirectionOffset = newManipulators[SECOND_DEPTH_MANIPULATOR].offset;
            extrudeDefinition.secondDirectionOppositeDirection = newSecondDirectionOffset < 0 * meter;
            extrudeDefinition.secondDirectionDepth = abs(newSecondDirectionOffset);
        }
        else if (newManipulators[SECOND_FLIP_MANIPULATOR] is map &&
            extrudeDefinition.secondDirectionBound != SecondDirectionBoundingType.BLIND)
        {
            extrudeDefinition.secondDirectionOppositeDirection = newManipulators[SECOND_FLIP_MANIPULATOR].flipped;
        }
    }

    return extrudeDefinition;
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
        var refPlane = evPlane(context, { "face" : endBoundEntity });
        if (refPlane.result == undefined)
            return false; //err on side of not flipping, TODO: surfaceXline
        var isecResult = intersection(refPlane.result, extrudeAxis);
        if (isecResult == undefined || isecResult.dim != 0)
            return false;

        var dotPr = stripUnits(dotProduct(isecResult.intersection - extrudeAxis.origin, extrudeAxis.direction));
        return dotPr < -TOLERANCE.zeroLength;
    }
    var pln = plane(extrudeAxis.origin, extrudeAxis.direction);
    var boxResult = evBox3d(context, { 'topology' : endBoundEntity, 'cSys' : planeToWorld(pln) });
    if (boxResult.error != undefined)
        return false;

    return (stripUnits(boxResult.result.minCorner[2]) < -TOLERANCE.zeroLength &&
            stripUnits(boxResult.result.maxCorner[2]) < TOLERANCE.zeroLength);
}


export function upToBoundaryFlip(context is Context, featureDefinition is map, featureInfo is map) returns map
{
    var usedEntities = getEntitiesToUse(featureDefinition);
    var resolvedEntities = evaluateQuery(context, usedEntities);
    if (@size(resolvedEntities) == 0)
    {
        return featureDefinition;
    }
    var extrudeAxis = computeExtrudeAxis(context, resolvedEntities[0]);
    if (extrudeAxis == undefined)
    {
        return featureDefinition;
    }
    var direction = extrudeAxis.direction;
    if (featureDefinition.oppositeDirection == true)
        direction *= -1;
    if (featureDefinition.endBoundEntity is Query &&
        shouldFlipExtrudeDirection(context,
                                   featureDefinition.endBound,
                                   featureDefinition.endBoundEntity,
                                   line(extrudeAxis.origin, direction)))
    {
        featureDefinition.oppositeDirection = (featureDefinition.oppositeDirection == true) ? false : true;
    }
    return featureDefinition;
}

export function performTypeFlip(context is Context, featureDefinition is map, featureInfo is map) returns map
{
    featureDefinition.oppositeDirection = (featureDefinition.oppositeDirection == true) ? false : true;
    return featureDefinition;
}

