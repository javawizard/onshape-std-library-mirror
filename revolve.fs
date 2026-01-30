FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/tool.fs", version : "✨");

// Features using manipulators must export manipulator.fs
export import(path : "onshape/std/manipulator.fs", version : "✨");
export import(path : "onshape/std/sidegeometryrule.gen.fs", version : "✨");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "✨");
import(path : "onshape/std/booleanHeuristics.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/mathUtils.fs", version : "✨");
import(path : "onshape/std/offsetSurface.fs", version : "✨");
import(path : "onshape/std/sketch.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/tolerance.fs", version : "✨");
import(path : "onshape/std/topologyUtils.fs", version : "✨");
import(path : "onshape/std/transform.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");

/**
 * Types of bounds allowed in revolve operation.
 */
export enum RevolveBoundingType
{
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Up to next" }
    UP_TO_NEXT,
    annotation { "Name" : "Up to face" }
    UP_TO_SURFACE,
    annotation { "Name" : "Up to part" }
    UP_TO_BODY,
    annotation { "Name" : "Up to vertex" }
    UP_TO_VERTEX
}

const THICKNESS_PARAMETERS = { "thickness" : {}, "thickness1" : {}, "thickness2" : {} };

/**
 * Feature performing an [opRevolve], followed by an [opBoolean]. For simple revolves, prefer using
 * [opRevolve] directly.
 */
annotation { "Feature Type Name" : "Revolve",
        "Manipulator Change Function" : "revolveManipulatorChange",
        "Filter Selector" : "allparts",
        "Editing Logic Function" : "revolveEditLogic" }
export const revolve = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Creation type", "UIHint" : [UIHint.HORIZONTAL_ENUM, UIHint.REMEMBER_PREVIOUS_VALUE] }
        definition.bodyType is ExtendedToolBodyType;

        if (definition.bodyType != ExtendedToolBodyType.SURFACE)
        {
            booleanStepTypePredicate(definition);
        }
        else
        {
            surfaceOperationTypePredicate(definition);
        }

        if (definition.bodyType == ExtendedToolBodyType.SOLID)
        {
            annotation { "Name" : "Faces and sketch regions to revolve",
                        "Filter" : (EntityType.FACE && GeometryType.PLANE) && ConstructionObject.NO }
            definition.entities is Query;
        }
        else if (definition.bodyType == ExtendedToolBodyType.SURFACE)
        {
            annotation { "Name" : "Edges and sketch curves to revolve",
                        "Filter" : (EntityType.EDGE && ConstructionObject.NO) || (EntityType.BODY && BodyType.WIRE && SketchObject.NO) }
            definition.surfaceEntities is Query;
        }
        else
        {
            annotation { "Name" : "Faces and sketch regions to revolve",
                        "Filter" : (EntityType.FACE && GeometryType.PLANE && ConstructionObject.NO && ModifiableEntityOnly.NO) ||
                        (EntityType.EDGE && SketchObject.YES && ConstructionObject.NO && ModifiableEntityOnly.YES) }
            definition.wallShape is Query;

            annotation { "Name" : "Mid plane", "Default" : false }
            definition.midplane is boolean;

            if (!definition.midplane)
            {
                annotation { "Name" : "Thickness 1", "UIHint" : UIHint.CAN_BE_TOLERANT }
                isLength(definition.thickness1, ZERO_INCLUSIVE_OFFSET_BOUNDS);

                annotation { "Name" : "Flip wall", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                definition.flipWall is boolean;

                annotation { "Name" : "Thickness 2", "UIHint" : UIHint.CAN_BE_TOLERANT }
                isLength(definition.thickness2, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
            }
            else
            {
                annotation { "Name" : "Thickness", "UIHint" : UIHint.CAN_BE_TOLERANT }
                isLength(definition.thickness, ZERO_INCLUSIVE_OFFSET_BOUNDS);
            }
        }

        annotation { "Name" : "Revolve axis", "Filter" : QueryFilterCompound.ALLOWS_AXIS, "MaxNumberOfPicks" : 1 }
        definition.axis is Query;

        annotation { "Name" : "Full revolve", "Default" : true }
        definition.fullRevolve is boolean;

        if (!definition.fullRevolve)
        {
            annotation { "Name" : "End type" }
            definition.endBound is RevolveBoundingType;
            annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION_CIRCULAR }
            definition.oppositeDirection is boolean;

            if (definition.endBound == RevolveBoundingType.BLIND)
            {
                annotation { "Name" : "Revolve angle", "UIHint" : UIHint.CAN_BE_TOLERANT }
                isAngle(definition.angle, ANGLE_360_BOUNDS);

                annotation { "Name" : "Symmetric" }
                definition.symmetric is boolean;
            }
            else if (definition.endBound == RevolveBoundingType.UP_TO_SURFACE)
            {
                annotation { "Name" : "Up to face",
                            "Filter" : (EntityType.FACE && SketchObject.NO && AllowMeshGeometry.YES) || BodyType.MATE_CONNECTOR,
                            "MaxNumberOfPicks" : 1 }
                definition.endBoundEntityFace is Query;

            }
            else if (definition.endBound == RevolveBoundingType.UP_TO_BODY)
            {
                annotation { "Name" : "Up to surface or part",
                            "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO && AllowMeshGeometry.YES,
                            "MaxNumberOfPicks" : 1 }
                definition.endBoundEntityBody is Query;
            }
            else if (definition.endBound == RevolveBoundingType.UP_TO_VERTEX)
            {
                annotation { "Name" : "Up to vertex or mate connector",
                            "Filter" : QueryFilterCompound.ALLOWS_VERTEX,
                            "MaxNumberOfPicks" : 1 }
                definition.endBoundEntityVertex is Query;
            }
            if (definition.endBound != RevolveBoundingType.BLIND)
            {
                annotation { "Name" : "Offset", "Column Name" : "Has offset", "UIHint" : ["DISPLAY_SHORT", "FIRST_IN_ROW"] }
                definition.endBoundHasOffset is boolean;
                if (definition.endBoundHasOffset)
                {
                    annotation { "Name" : "Offset", "Column Name" : "Offset angle", "UIHint" : ["DISPLAY_SHORT"] }
                    isAngle(definition.endBoundOffset, ANGLE_360_ZERO_DEFAULT_BOUNDS);
                    annotation { "Name" : "Flip offset", "Column Name" : "Offset opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION_CIRCULAR }
                    definition.endBoundOffsetFlip is boolean;
                }
            }

            if (definition.endBound != RevolveBoundingType.BLIND || !definition.symmetric)
            {
                annotation { "Name" : "Second end position" }
                definition.hasStartBound is boolean;

                if (definition.hasStartBound)
                {
                    annotation { "Group Name" : "Second direction", "Driving Parameter" : "hasStartBound", "Collapsed By Default" : false }
                    {
                        annotation { "Name" : "Start type" }
                        definition.startBound is RevolveBoundingType;

                        if (definition.startBound == RevolveBoundingType.BLIND)
                        {
                            annotation { "Name" : "Start angle" }
                            isAngle(definition.angleBack, ANGLE_360_REVERSE_DEFAULT_BOUNDS);
                        }
                        else
                        {
                            annotation { "Name" : "Opposite direction", "Column Name" : "Start opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION_CIRCULAR }
                            definition.startOppositeDirection is boolean;

                            if (definition.startBound == RevolveBoundingType.UP_TO_SURFACE)
                            {
                                annotation { "Name" : "Up to face", "Column Name" : "Start up to face",
                                            "Filter" : (EntityType.FACE && SketchObject.NO && AllowMeshGeometry.YES) || BodyType.MATE_CONNECTOR,
                                            "MaxNumberOfPicks" : 1 }
                                definition.startBoundEntityFace is Query;
                            }
                            else if (definition.startBound == RevolveBoundingType.UP_TO_BODY)
                            {
                                annotation { "Name" : "Up to surface or part", "Column Name" : "Start up to surface or part",
                                            "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO && AllowMeshGeometry.YES,
                                            "MaxNumberOfPicks" : 1 }
                                definition.startBoundEntityBody is Query;
                            }
                            else if (definition.startBound == RevolveBoundingType.UP_TO_VERTEX)
                            {
                                annotation { "Name" : "Up to vertex or mate connector", "Column Name" : "Start up to vertex or mate connector",
                                            "Filter" : QueryFilterCompound.ALLOWS_VERTEX,
                                            "MaxNumberOfPicks" : 1 }
                                definition.startBoundEntityVertex is Query;
                            }
                            annotation { "Name" : "Start offset", "Column Name" : "Start direction has offset", "UIHint" : ["DISPLAY_SHORT", "FIRST_IN_ROW"] }
                            definition.startBoundHasOffset is boolean;
                            if (definition.startBoundHasOffset)
                            {
                                annotation { "Name" : "Start offset", "Column Name" : "Start direction offset angle", "UIHint" : ["DISPLAY_SHORT"] }
                                isAngle(definition.startBoundOffset, ANGLE_360_ZERO_DEFAULT_BOUNDS);
                                annotation { "Name" : "Flip offset", "Column Name" : "Start offset opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION_CIRCULAR }
                                definition.startBoundOffsetFlip is boolean;
                            }
                        }
                    }

                }
            }
        }

        if (definition.bodyType == ExtendedToolBodyType.SOLID || definition.bodyType == ExtendedToolBodyType.THIN)
        {
            booleanStepScopePredicate(definition);
        }
        else
        {
            surfaceJoinStepScopePredicate(definition);
        }
    }
    {
        const originalDefinition = definition; // we need to refer to the original definition later on, not the modified one

        if (hasFirstBlindDirection(definition))
            definition.endBoundAngle = adjustAngle(context, definition.angle);

        if (revolveHasStartBound(definition) && definition.startBound == RevolveBoundingType.BLIND)
            definition.startBoundAngle = adjustAngle(context, definition.angleBack);
        // opRevolve uses angleBack to identify pre-bounds revolves, so we mark it as undefined after (possibly) consuming its value
        definition.angleBack = undefined;

        definition.entities = getEntitiesToUse(context, definition);

        verifyNoMesh(context, definition, "entities");
        verifyNoMesh(context, definition, "axis");

        const resolvedEntities = evaluateQuery(context, definition.entities);
        if (resolvedEntities == [])
        {
            if (definition.bodyType == ExtendedToolBodyType.SOLID)
                throw regenError(ErrorStringEnum.REVOLVE_SELECT_FACES, ["entities"]);
            else
                throw regenError(ErrorStringEnum.REVOLVE_SURF_NO_CURVE, ["surfaceEntities"]);
        }

        var remainingTransform = getRemainderPatternTransform(context,
        { "references" : qUnion([definition.entities, definition.axis]) });

        definition.axis = try(evAxis(context, definition));
        if (definition.axis == undefined)
            throw regenError(ErrorStringEnum.REVOLVE_SELECT_AXIS, ["axis"]);

        if (definition.bodyType == ExtendedToolBodyType.THIN)
        {
            //For Thin wall creation - prepare thickness value
            definition = setWallThickness(definition);
        }

        addRevolveManipulator(context, id, definition);

        if (definition.fullRevolve)
        {
            definition.endBound = RevolveBoundingType.BLIND;
            definition.endBoundAngle = 2 * PI * radian;
            definition.startBound = RevolveBoundingType.BLIND;
            definition.startBoundAngle = 0 * radian;
        }
        else if (definition.endBound == RevolveBoundingType.BLIND && definition.symmetric)
        {
            definition.endBoundAngle = definition.endBoundAngle / 2;
            definition.startBound = RevolveBoundingType.BLIND;
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V234_REVOLVE_TWO_DIRECTION))
            {
                definition.startBoundAngle = 2 * PI * radian - definition.endBoundAngle;
            }
            else
            {
                // older versions use opposite direction
                definition.startBoundAngle = definition.endBoundAngle;
            }
        }
        else if (!revolveHasStartBound(definition))
        {
            definition.startBound = RevolveBoundingType.BLIND;
            definition.startBoundAngle = 0 * radian;
        }
        else if (definition.startBound != RevolveBoundingType.BLIND)
        {
            // We want the start direction to be independent of the end direction, and we flip the axis depending on definition.oppositeDirection,
            // so we need to make sure we have the correct "opposite"
            definition.startOppositeDirection = definition.startOppositeDirection != definition.oppositeDirection;
        }
        if (!definition.endBoundHasOffset)
        {
            definition.endBoundOffset = 0 * radian;
        }
        if (!definition.startBoundHasOffset)
        {
            definition.startBoundOffset = 0 * radian;
        }
        if (definition.oppositeDirection)
        {
            definition.axis.direction *= -1; // To be consistent with extrude
        }

        var tolerantParameters;
        try
        {
            tolerantParameters = getTolerantParameterIds(context, {});
        }
        var thinOpposingPairTracking = [];
        const hasTolerantThickness = tolerantParameters != undefined && size(intersectMaps([tolerantParameters, THICKNESS_PARAMETERS])) > 0;
        if (hasTolerantThickness)
        {
            // we do tracking if there is ANY sort of tolerance, not just thin because we need the end-face tracking
            thinOpposingPairTracking = createOpposingPairTracking(context, id, definition);
        }

        opRevolve(context, id, definition);
        transformResultIfNecessary(context, id, remainingTransform);

        //In case of Thin wall creation - apply thicken to the every new surface if the revolve has two blind bounds.
        // If either bound is not blind, this happens during opRevolve.
        if (definition.bodyType == ExtendedToolBodyType.THIN && isDoubleBlind(definition))
        {
            applyThickenToCreatedSurfaces(context, id, definition);
        }

        const reconstructOp = function(id)
            {
                opRevolve(context, id, definition);
                transformResultIfNecessary(context, id, remainingTransform);

                if (definition.bodyType == ExtendedToolBodyType.THIN && isDoubleBlind(definition))
                {
                    applyThickenToCreatedSurfaces(context, id, definition);
                }
            };

        if (definition.bodyType == ExtendedToolBodyType.SURFACE && tolerantParameters != undefined && size(tolerantParameters) > 0)
        {
            considerSurfaceToleranceWarning(context, id, originalDefinition, tolerantParameters);
        }

        if (definition.bodyType == ExtendedToolBodyType.SOLID || definition.bodyType == ExtendedToolBodyType.THIN)
        {
            var dimensioningData;
            if (tolerantParameters != undefined && size(tolerantParameters) > 0)
            {
                dimensioningData = generateDimensioningData(context, id, originalDefinition, definition.axis.direction, tolerantParameters);
            }

            processNewBodyIfNeeded(context, id, definition, reconstructOp);

            if (dimensioningData != undefined)
            {
                const thinOpposingPairs = resolveOpposingPairTracking(context, id, thinOpposingPairTracking, isDoubleBlind(definition));
                registerDimensionedEntities(context, id, originalDefinition, definition.axis.direction, dimensioningData, tolerantParameters, thinOpposingPairs);
            }
        }
        else if (definition.surfaceOperationType == NewSurfaceOperationType.ADD)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1197_DETECT_SURFACE_JOIN_CPP))
            {
                joinSurfaceBodiesWithAutoMatching(context, id, definition, false, reconstructOp);
            }
            else
            {
                var matches = createTopologyMatchesForSurfaceJoin(context, id, definition, qCapEntity(id, CapType.START), definition.entities, remainingTransform);
                checkForNotJoinableSurfacesInScope(context, id, definition, matches);
                joinSurfaceBodies(context, id, matches, false, reconstructOp);
            }
        }
    }, { bodyType : ExtendedToolBodyType.SOLID, oppositeDirection : false, operationType : NewBodyOperationType.NEW, surfaceOperationType : NewSurfaceOperationType.NEW, defaultSurfaceScope : true,
            fullRevolve : true, endBound : RevolveBoundingType.BLIND, symmetric : false, hasStartBound : false, endBoundHasOffset : false, startBoundHasOffset : false, startOppositeDirection : false });


function hasFirstBlindDirection(definition is map)
{
    return !definition.fullRevolve && definition.endBound == RevolveBoundingType.BLIND;
}

function revolveHasStartBound(definition is map)
{
    return !definition.fullRevolve && !(definition.endBound == RevolveBoundingType.BLIND && definition.symmetric) && definition.hasStartBound;
}

// We have a double blind (i.e. no up to bounds) if:
// - it's a full revolve, or
// - the end bound is blind and:
//      - it's symmetric, or
//      - it doesn't have a start bound, or
//      - the start bound is blind.
function isDoubleBlind(definition is map)
{
    return definition.fullRevolve || (definition.endBound == RevolveBoundingType.BLIND && (definition.symmetric || !definition.hasStartBound || definition.startBound == RevolveBoundingType.BLIND));
}

//Manipulator functions

const ANGLE_MANIPULATOR = "angleManipulator";
const SECOND_ANGLE_MANIPULATOR = "secondAngleManipulator";
const FLIP_MANIPULATOR = "flipManipulator";
const SECOND_FLIP_MANIPULATOR = "secondFlipManipulator";

function getEntitiesToUse(context is Context, revolveDefinition is map)
{
    if (revolveDefinition.bodyType == ExtendedToolBodyType.SOLID)
    {
        return revolveDefinition.entities;
    }
    else
    {
        if (revolveDefinition.bodyType == ExtendedToolBodyType.THIN)
        {
            //planar faces may be selected, only their edges should be taken
            const faces = qEntityFilter(revolveDefinition.wallShape, EntityType.FACE);
            if (!isQueryEmpty(context, faces))
            {
                const extractedEdges = qAdjacent(faces, AdjacencyType.EDGE, EntityType.EDGE);
                revolveDefinition.wallShape = qSubtraction(revolveDefinition.wallShape, faces);
                revolveDefinition.wallShape = qUnion([revolveDefinition.wallShape, extractedEdges]);
            }
        }
        const qlvInput = revolveDefinition.bodyType == ExtendedToolBodyType.SURFACE ? revolveDefinition.surfaceEntities : revolveDefinition.wallShape;
        var surfaceEntities;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V177_CONSTRUCTION_OBJECT_FILTER))
        {
            surfaceEntities = qConstructionFilter(qlvInput, ConstructionObject.NO);
        }
        else
        {
            surfaceEntities = qlvInput;
        }

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V576_GET_WIRE_LAMINAR_DEPENDENCIES))
        {
            surfaceEntities = followWireEdgesToLaminarSource(context, surfaceEntities);
        }
        return surfaceEntities;
    }
}

function disableTwoDirectionManipulator(context is Context, revolveDefinition is map)
{
    return revolveHasStartBound(revolveDefinition) &&
        !isAtVersionOrLater(context, FeatureScriptVersionNumber.V234_REVOLVE_TWO_DIRECTION);
}

function addRevolveManipulator(context is Context, id is Id, revolveDefinition is map)
{
    if (revolveDefinition.fullRevolve || disableTwoDirectionManipulator(context, revolveDefinition))
    {
        return;
    }
    const isEndBlind = hasFirstBlindDirection(revolveDefinition);
    const hasStartBound = revolveHasStartBound(revolveDefinition);
    const isStartBlind = revolveDefinition.startBound == RevolveBoundingType.BLIND;

    const entities = qSheetMetalFlatFilter(getEntitiesToUse(context, revolveDefinition), SMFlatType.NO);

    //Compute manipulator parameters
    var revolvePoint;
    const faceResult = try silent(evFaceTangentPlane(context, { "face" : qNthElement(entities, 0), "parameter" : vector(0.5, 0.5) }));
    if (faceResult != undefined)
    {
        revolvePoint = faceResult.origin;
    }
    else
    {
        const edgeResult = try silent(evEdgeTangentLine(context, { "edge" : qNthElement(entities, 0), "parameter" : 0.5 }));
        if (edgeResult != undefined)
            revolvePoint = edgeResult.origin;
        else
            return;
    }
    const axisOrigin = project(revolveDefinition.axis, revolvePoint);

    var minValue = -2 * PI * radian;
    var maxValue = 2 * PI * radian;

    //Compute value
    var angle = revolveDefinition.endBoundAngle;
    if (hasStartBound && isStartBlind)
    {
        if (!revolveDefinition.oppositeDirection)
        {
            minValue = 0 * radian;
            maxValue = 2 * PI * radian;
        }
        else
        {
            minValue = -2 * PI * radian;
            maxValue = 0 * radian;
        }
    }

    if (isEndBlind)
    {
        if (revolveDefinition.oppositeDirection == true)
            angle *= -1;
        if (revolveDefinition.symmetric)
        {
            angle *= .5;
            minValue = -PI * radian;
            maxValue = PI * radian;
        }
        addManipulators(context, id, {
                    (ANGLE_MANIPULATOR) : angularManipulator({
                            "axisOrigin" : axisOrigin,
                            "axisDirection" : revolveDefinition.axis.direction,
                            "rotationOrigin" : revolvePoint,
                            "angle" : angle,
                            "sources" : entities,
                            "minValue" : minValue,
                            "maxValue" : maxValue,
                            "primaryParameterId" : "angle"
                        })
                });
    }
    else
    {
        const direction = normalize(cross(revolveDefinition.axis.direction, revolvePoint - axisOrigin));
        addManipulators(context, id, {
                    (FLIP_MANIPULATOR) : flipManipulator({
                            "base" : revolvePoint,
                            "direction" : direction,
                            "flipped" : revolveDefinition.oppositeDirection,
                            "style" : ManipulatorStyleEnum.DEFAULT
                        })
                });
    }

    if (hasStartBound)
    {
        if (isStartBlind)
        {
            var angleBack = revolveDefinition.startBoundAngle;
            if (revolveDefinition.oppositeDirection)
                angleBack *= -1;
            addManipulators(context, id, {
                        (SECOND_ANGLE_MANIPULATOR) : angularManipulator({
                                "axisOrigin" : axisOrigin,
                                "axisDirection" : revolveDefinition.axis.direction,
                                "rotationOrigin" : revolvePoint,
                                "angle" : angleBack,
                                "sources" : entities,
                                "minValue" : minValue,
                                "maxValue" : maxValue,
                                "style" : ManipulatorStyleEnum.SECONDARY,
                                "primaryParameterId" : "angleBack"
                            })
                    });
        }
        else
        {
            const direction = normalize(cross(revolveDefinition.axis.direction, revolvePoint - axisOrigin));
            addManipulators(context, id, {
                        (SECOND_FLIP_MANIPULATOR) : flipManipulator({
                                "base" : revolvePoint,
                                "direction" : direction,
                                "flipped" : revolveDefinition.startOppositeDirection,
                                "style" : ManipulatorStyleEnum.SECONDARY
                            })
                });
        }
    }
}

/**
 * @internal
 * Manipulator change function for `revolve` feature.
 */
export function revolveManipulatorChange(context is Context, revolveDefinition is map, newManipulators is map) returns map
{
    if (newManipulators[ANGLE_MANIPULATOR] is Manipulator)
    {
        const newAngle = newManipulators[ANGLE_MANIPULATOR].angle;

        revolveDefinition.oppositeDirection = newAngle < 0 * radian;
        revolveDefinition.angle = abs(newAngle);

        if (revolveDefinition.symmetric)
        {
            revolveDefinition.angle *= 2;
            if (revolveDefinition.angle > 2 * PI * radian)
            {
                // for the effect of one-directional manip loop
                revolveDefinition.angle = 4 * PI * radian - revolveDefinition.angle;
            }
        }
    }
    if (newManipulators[SECOND_ANGLE_MANIPULATOR] is Manipulator)
    {
        revolveDefinition.angleBack = abs(newManipulators[SECOND_ANGLE_MANIPULATOR].angle);
    }
    if (newManipulators[FLIP_MANIPULATOR] is Manipulator)
    {
        revolveDefinition.oppositeDirection = newManipulators[FLIP_MANIPULATOR].flipped;
    }
    if (newManipulators[SECOND_FLIP_MANIPULATOR] is Manipulator)
    {
        revolveDefinition.startOppositeDirection = newManipulators[SECOND_FLIP_MANIPULATOR].flipped;
    }
    return revolveDefinition;
}


/**
 * @internal
 * Editing logic function for `revolve` feature.
 */
export function revolveEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    var retestDirectionFlip = false;
    // If flip has not been specified and there is no second direction we can adjust flip based on boolean operation
    if (!specifiedParameters.oppositeDirection && hasFirstBlindDirection(definition) && !definition.symmetric && !revolveHasStartBound(definition))
    {
        if (canSetBooleanFlip(oldDefinition, definition, specifiedParameters))
        {
            definition.oppositeDirection = !definition.oppositeDirection;
        }
        else
        {
            retestDirectionFlip = true;
        }
    }

    if (definition.bodyType == ExtendedToolBodyType.SOLID || definition.bodyType == ExtendedToolBodyType.THIN)
    {
        var newDefinition = definition;
        if (retestDirectionFlip)
        {
            // Always retest forward to stabilize the case where the forward and backward test both result in a flip to
            // each other.  Example: BEL-106989
            newDefinition.oppositeDirection = false;
        }

        newDefinition = booleanStepEditLogic(context, id, oldDefinition, newDefinition, specifiedParameters, hiddenBodies, revolve);

        // booleanStepEditLogic might change boolean operation type,
        // if flip was not adjusted above, re-test it
        if (retestDirectionFlip)
        {
            if (canSetBooleanFlip(definition, newDefinition, specifiedParameters))
            {
                newDefinition.oppositeDirection = true;
            }
            else
            {
                newDefinition.oppositeDirection = definition.oppositeDirection;
            }
        }

        definition = newDefinition;
    }
    else if (definition.bodyType == ExtendedToolBodyType.SURFACE && !specifiedParameters.surfaceOperationType)
    {
        if (definition.fullRevolve || revolveHasStartBound(definition) || definition.symmetric)
        {
            definition.surfaceOperationType = NewSurfaceOperationType.NEW;
        }
        else
        {
            definition = surfaceOperationTypeEditLogic(context, id, definition, specifiedParameters, definition.surfaceEntities, hiddenBodies);
        }
    }

    return definition;
}

//Thin wall - thickess value definition
function setWallThickness(definition is map) returns map
{
    definition.wallThickness_1 = definition.thickness1;
    definition.wallThickness_2 = definition.thickness2;

    if (definition.midplane)
    {
        definition.wallThickness_1 = definition.thickness / 2;
        definition.wallThickness_2 = definition.wallThickness_1;
        return definition;
    }

    const flipThickness = definition.flipWall != definition.oppositeDirection;
    if (flipThickness)
    {
        definition.wallThickness_1 = definition.thickness2;
        definition.wallThickness_2 = definition.thickness1;
    }
    return definition;
}

function applyThickenToCreatedSurfaces(context is Context, id is Id, definition is map)
{
    var fullRevolveWasPerformed = false;
    if (revolveHasStartBound(definition))
    {
        const isForwardAngleFullCircle = tolerantEquals(definition.endBoundAngle, 0 * degree) || tolerantEquals(360 * degree, definition.endBoundAngle);
        const isBackAngleFullCircle = tolerantEquals(definition.startBoundAngle, 0 * degree) || tolerantEquals(360 * degree, definition.startBoundAngle);
        fullRevolveWasPerformed = (isForwardAngleFullCircle && isBackAngleFullCircle) || tolerantEquals(definition.endBoundAngle, definition.startBoundAngle);
    }
    else
    {
        const isRevolveAngleFullCircle = tolerantEquals(definition.endBoundAngle, 0 * degree) || tolerantEquals(360 * degree, definition.endBoundAngle);
        fullRevolveWasPerformed = definition.fullRevolve || isRevolveAngleFullCircle;
    }

    const surfaceBodies = qBodyType(qCreatedBy(id, EntityType.BODY), BodyType.SHEET);
    if (fullRevolveWasPerformed)
    {
        opThicken(context, id + "defaultSegmentThicken", {
                    "entities" : surfaceBodies,
                    "thickness1" : definition.wallThickness_1,
                    "thickness2" : definition.wallThickness_2
                });
    }
    else
    {
        opThicken(context, id + "customSegmentThicken", {
                    "entities" : surfaceBodies,
                    "thickness1" : definition.wallThickness_1,
                    "thickness2" : definition.wallThickness_2,
                    "sideGeometryRule" : { "type" : SideGeometryRule.REVOLVED, "axis" : definition.axis }
                });
    }
    opDeleteBodies(context, id + "deleteWallShape", {
                "entities" : surfaceBodies });
}

// Tolerant dimension functions

type DimensioningData typecheck canBeDimensioningData;

predicate canBeDimensioningData(value)
{
    value.startPlane == undefined || value.startPlane is Plane;
    value.endPlane == undefined || value.endPlane is Plane;
}

function getQueriesForStartAndEndCaps(context is Context, id is Id, thin is boolean, axis)
precondition
{
    is3dDirection(axis);
}
{
    var startQ;
    var endQ;
    if (thin)
    {
        const planarNonCapFacesQ = qNonCapEntity(id, EntityType.FACE)->qGeometry(GeometryType.PLANE);
        // We choose the first of the planar faces to be "side 1". We need to use the results, not the inputs to the feature,
        // and there may be multiple faces in that plane that we want to return (a revolve can include multiple disjoint entities)
        // To complicate matters further we can have non-cap entities that are side faces, so we need to exclude any that are
        // perpendicular to the axis
        const endFacesQ = qSubtraction(planarNonCapFacesQ, qParallelPlanes(planarNonCapFacesQ, axis));
        if (!isQueryEmpty(context, endFacesQ))
        {
            const plane1 = evPlane(context,
                {
                        face : qNthElement(endFacesQ, 0)
                    });
            startQ = endFacesQ->qCoincidesWithPlane(plane1);
            endQ = qSubtraction(endFacesQ, startQ);
        }
        else
        {
            startQ = qNothing();
            endQ = qNothing();
        }
    }
    else
    {
        startQ = qCapEntity(id, CapType.START, EntityType.FACE);
        endQ = qCapEntity(id, CapType.END, EntityType.FACE);
    }
    return [startQ, endQ];
}

function generateDimensioningData(context is Context, id is Id, definition is map, axis, tolerantParameters is map) returns DimensioningData
precondition
{
    is3dDirection(axis);
}
{
    var result = {} as DimensioningData;
    if (tolerantParameters.angle != undefined && hasFirstBlindDirection(definition))
    {
        const ends = getQueriesForStartAndEndCaps(context, id, definition.bodyType == ExtendedToolBodyType.THIN, axis);
        const startQ = ends[0];
        if (!isQueryEmpty(context, startQ))
        {
            result.startPlane = evPlane(context,
                {
                        "face" : startQ
                    });
        }
        const endQ = ends[1];
        if (!isQueryEmpty(context, endQ))
        {
            result.endPlane = evPlane(context,
                {
                        "face" : endQ
                    });
        }
    }
    return result;
}

function resolveEndCaps(context is Context, id is Id, thin is boolean, axis, dimensioningData is DimensioningData) returns map
precondition
{
    is3dDirection(axis);
}
{
    var ends = getQueriesForStartAndEndCaps(context, id, thin, axis);
    var startWasReplaced = false;
    var startQ = ends[0];
    var endQ = ends[1];

    if (thin && !isQueryEmpty(context, startQ) && isQueryEmpty(context, endQ))
    {
        // With a thin feature we don't know if the start or end is missing, if only one is found then it is returned as the start
        // So, in this case we check if there is coincidence with the start plane, and if not we swap
        if (dimensioningData.startPlane != undefined && isQueryEmpty(context, qCoincidesWithPlane(startQ, dimensioningData.startPlane)))
        {
            // The start isn't, so we assume it is the end
            const swapping = endQ;
            endQ = startQ;
            startQ = swapping;
        }
    }
    if (isQueryEmpty(context, startQ) && dimensioningData.startPlane != undefined)
    {
        startQ = qCreatedBy(id, EntityType.FACE)->qOwnerBody()->qOwnedByBody(EntityType.FACE)->qCoincidesWithPlane(dimensioningData.startPlane);
        startWasReplaced = true;
    }
    if (isQueryEmpty(context, endQ) && dimensioningData.endPlane != undefined)
    {
        endQ = qCreatedBy(id, EntityType.FACE)->qOwnerBody()->qOwnedByBody(EntityType.FACE)->qCoincidesWithPlane(dimensioningData.endPlane);
    }
    return
    {
            "start" : startQ,
            "end" : endQ,
            "replacedStart" : startWasReplaced
        };
}

function registerDimensionedEntities(context is Context, id is Id, definition is map, axis, dimensioningData is DimensioningData, tolerantParameters is map, thinOpposingPairs is array)
precondition
{
    is3dDirection(axis);
}
{
    if (tolerantParameters.angle != undefined && hasFirstBlindDirection(definition))
    {
        const endData = resolveEndCaps(context, id, definition.bodyType == ExtendedToolBodyType.THIN, axis, dimensioningData);
        var startQ = endData.start;
        var endQ = endData.end;
        var startWasReplaced = endData.replacedStart;

        if (revolveHasStartBound(definition))
        {
            reportFeatureWarning(context, id, ErrorStringEnum.TOLERANT_ANGLE_NO_SECOND);
        }
        else if (isQueryEmpty(context, startQ) || isQueryEmpty(context, endQ))
        {
            reportFeatureWarning(context, id, ErrorStringEnum.TOLERANT_ANGLE_END_CONSUMED);
        }
        else
        {
            // With an angle we need to store whether the angle 'measures solid' or not, starting from the start face.
            // If its a remove it doesn't, otherwise it does BUT if we are revolving from a face then, the face that got
            // selected just above here has the opposite sense.
            setDimensionedEntities(context,
                {
                        parameterId : 'angle',
                        queries : [startQ, endQ],
                        dimensionType : FeatureDimensionType.ANGLE,
                        measuresSolidAngle : (definition.operationType != NewBodyOperationType.REMOVE) != startWasReplaced
                    });
        }
    }

    if (definition.bodyType == ExtendedToolBodyType.THIN)
    {
        registerEntitiesForThinFeature(context, id, definition, tolerantParameters, thinOpposingPairs);
    }
}

function createOpposingPairTracking(context is Context, id is Id, definition is map) returns array
{
    const sourceLinesQ = qGeometry(definition.entities, GeometryType.LINE);
    var validQ = sourceLinesQ;
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2687_LOOSEN_THIN_RESTRICTIONS))
    {
        // Include circles (full and arcs) in the list of valid edges
        validQ = qUnion([sourceLinesQ, qGeometry(definition.entities, GeometryType.CIRCLE), qGeometry(definition.entities, GeometryType.ARC)]);
    }
    if (!isQueryEmpty(context, validQ))
    {
        return mapArray(evaluateQuery(context, validQ), edgeQ
            =>startTracking(context, edgeQ));
    }
    else
    {
        return [];
    }
}

function resolveOpposingPairTracking(context is Context, id is Id, tracking is array, isDoubleBlind is boolean) returns array
{
    if (size(tracking) > 0)
    {
        const restrictTypes = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V2687_LOOSEN_THIN_RESTRICTIONS);
        const allResults = mapArray(tracking, trackingQ
            =>{
                const nonCapQ = isDoubleBlind ? qSubtraction(trackingQ, qNonCapEntity(id, EntityType.FACE)) : qIntersection(trackingQ, qNonCapEntity(id, EntityType.FACE));
                const planeQ = qGeometry(nonCapQ, GeometryType.PLANE);
                var validQ = planeQ;
                if (!restrictTypes)
                {
                    const coneQ = qGeometry(nonCapQ, GeometryType.CONE);
                    const torusQ = qGeometry(nonCapQ, GeometryType.TORUS);
                    const cylinderQ = qGeometry(nonCapQ, GeometryType.CYLINDER);
                    validQ = qUnion([planeQ, coneQ, torusQ, cylinderQ]);
                }
                return evaluateQuery(context, validQ);
            });
        return filter(allResults, list
            =>size(list) == 2);
    }
    else
    {
        return [];
    }
}

function considerSurfaceToleranceWarning(context is Context, id is Id, definition is map, tolerantParameters is map)
{
    if (!definition.fullRevolve && tolerantParameters.angle != undefined)
    {
        reportFeatureWarning(context, id, ErrorStringEnum.TOLERANT_SOLID_ONLY, ['angle']);
    }
}

