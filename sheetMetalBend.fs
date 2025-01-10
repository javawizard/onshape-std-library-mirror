FeatureScript 2559; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/common.fs", version : "2559.0");
import(path : "onshape/std/moveFace.fs", version : "2559.0");
import(path : "onshape/std/sheetMetalFlange.fs", version : "2559.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2559.0");
import(path : "onshape/std/projectiontype.gen.fs", version : "2559.0");

/**
 * @internal
 * An `AngleBoundSpec` for bends.  Bends can theoretically go full circle, but 90 degrees will be common.
 */
export const SM_BEND_ANGLE_BOUNDS =
{
            (degree) : [1, 90, 359.9]
        } as AngleBoundSpec;

const SM_BEND_DIRECTION_ANGLE_BOUNDS =
{
            (degree) : [0, 90, 180]
        } as AngleBoundSpec;


/**
 * Values specifying the alignment of sheet metal after it is bent, relative to the line
 */
export enum BendAlignment
{
    annotation { "Name" : "Bend line" }
    BEND_LINE,
    annotation { "Name" : "Hold line" }
    HELD_EDGE,
    annotation { "Name" : "Hold other line" }
    BENT_EDGE,
    annotation { "Name" : "Inner" }
    BENT_INSIDE,
    annotation { "Name" : "Outer" }
    BENT_OUTSIDE,
    annotation { "Name" : "Middle" }
    BENT_MIDPLANE
}

/**
 * Angle types for the bend
 */
export enum BendAngleControlType
{
    annotation { "Name" : "Bend angle" }
    BEND_ANGLE,
    annotation { "Name" : "Align to geometry" }
    ALIGN_GEOMETRY,
    annotation { "Name" : "Angle from direction" }
    ANGLE_FROM_DIRECTION
}

/**
 * Bend a sheet metal model along a reference line, with additional bend control options.
 */
annotation { "Feature Type Name" : "Bend", "Editing Logic Function" : "onBendChange" }
export const sheetMetalBend = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Bend line", "Filter" : GeometryType.LINE, "MaxNumberOfPicks" : 1 }
        definition.bendReference is Query;
        annotation { "Name" : "Hold opposite side", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.holdOtherSide is boolean;
        annotation { "Name" : "Sheet metal face to bend", "Filter" : EntityType.FACE && SheetMetalDefinitionEntityType.FACE && GeometryType.PLANE && ModifiableEntityOnly.YES, "MaxNumberOfPicks" : 1 }
        definition.face is Query;

        annotation { "Name" : "Bend alignment", "UIHint" : UIHint.SHOW_LABEL }
        definition.bendAlignment is BendAlignment;

        annotation { "Name" : "Angle control type" }
        definition.angleControlType is BendAngleControlType;
        annotation { "Name" : "Opposite angle", "UIHint" : UIHint.OPPOSITE_DIRECTION_CIRCULAR }
        definition.oppositeAngle is boolean;
        if (definition.angleControlType == BendAngleControlType.BEND_ANGLE)
        {
            annotation { "Name" : "Bend angle" }
            isAngle(definition.bendAngle, SM_BEND_ANGLE_BOUNDS);
        }
        else if (definition.angleControlType == BendAngleControlType.ALIGN_GEOMETRY)
        {
            annotation { "Name" : "Parallel to", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION && !BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
            definition.parallelEntity is Query;
        }
        else if (definition.angleControlType == BendAngleControlType.ANGLE_FROM_DIRECTION)
        {
            annotation { "Name" : "Direction", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION && !BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
            definition.directionEntity is Query;
            annotation { "Name" : "Angle" }
            isAngle(definition.angleFromDirection, SM_BEND_DIRECTION_ANGLE_BOUNDS);
            annotation { "Name" : "Opposite angle", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.oppositeAngleFromDirection is boolean;
        }
        annotation { "Name" : "Use model bend radius", "Default" : true }
        definition.useDefaultRadius is boolean;
        if (!definition.useDefaultRadius)
        {
            annotation { "Name" : "Bend radius" }
            isLength(definition.bendRadius, SM_BEND_RADIUS_BOUNDS);
        }
        annotation { "Name" : "Use model K Factor", "Default" : true }
        definition.useDefaultKFactor is boolean;
        if (!definition.useDefaultKFactor)
        {
            annotation { "Name" : "K Factor" }
            isReal(definition.kFactor, K_FACTOR_BOUNDS);
        }
    }
    {
        checkNotInFeaturePattern(context, definition.face, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        const modelFaceQ = checkInputQueries(context, definition);
        const modelPlane = evPlane(context, {
                    "face" : modelFaceQ
                });
        const modelBodyQ = qOwnerBody(modelFaceQ);
        const initialData = getInitialEntitiesAndAttributes(context, modelBodyQ);
        const modelParameters = getParameters(context, definition, getModelParameters(context, qOwnerBody(modelFaceQ)), modelFaceQ, definition.bendReference);
        const bendBoundaries = generateBendBoundaries(context, id + "boundaries", definition, modelFaceQ, modelParameters);
        const imprintResult = imprintBendBoundaries(context, id + "imprint", modelFaceQ, bendBoundaries);
        const surfacePieces = decomposeModelSurface(context, id + "decompose", imprintResult);
        const wrappedSheetQ = wrapBendSurface(context, id + "wrapBend", imprintResult, surfacePieces, modelParameters.midSurfaceRadius, modelParameters.modelRadius, definition.oppositeAngle);
        const signedAngle = definition.oppositeAngle ? -modelParameters.angle : modelParameters.angle;
        transformMovingSurface(context, id + "transform", surfacePieces.movingSurface, imprintResult, modelPlane, wrappedSheetQ, signedAngle);
        annotateBendSurface(context, id, wrappedSheetQ, modelParameters.bendRadius, modelParameters.angle, modelParameters.kFactor);
        composeModelSurfaces(context, id + "compose", [surfacePieces.fixedSurface, wrappedSheetQ, surfacePieces.movingSurface]);

        // Add association attributes where needed and compute deleted attributes
        var toUpdate = assignSMAttributesToNewOrSplitEntities(context, surfacePieces.fixedSurface, initialData, id);
        updateSheetMetalGeometry(context, id, { "entities" : toUpdate.modifiedEntities,
                    "deletedAttributes" : toUpdate.deletedAttributes });
    },
    {
            oppositeAngle : false,
            holdOtherSide : false,
            useDefaultRadius : true,
            useDefaultKFactor : true,
            bendAlignment : BendAlignment.BEND_LINE,
            angleControlType : BendAngleControlType.BEND_ANGLE
        });


/**
 * @internal
 * The editing logic for bend.
 * If the user has not touched `holdOtherSide` then we are allowed to do so, and will to minimize the number of faces moved
 */
export function onBendChange(context is Context, id is Id, oldDefinition is map, definition is map, specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (specifiedParameters.face != true && oldDefinition.bendReference != definition.bendReference)
    {
        try silent
        {
            definition = applyEditLogicForFace(context, id, oldDefinition, definition, specifiedParameters, hiddenBodies);
        }
    }

    // If we didn't modify 'hold other' and just changed a selection then re-evaluate hold other
    if (specifiedParameters.holdOtherSide != true && (oldDefinition.face != definition.face || oldDefinition.bendReference != definition.bendReference))
    {
        try silent
        {
            definition = applyEditLogicForHoldOther(context, id, oldDefinition, definition, specifiedParameters);
        }
    }
    return definition;
}

function applyEditLogicForHoldOther(context is Context, id is Id, oldDefinition is map, definition is map, specifiedParameters is map) returns map
{
    var modelFaceQ;
    try silent
    {
        modelFaceQ = checkInputQueries(context, definition);
    }
    catch
    {
        // Not going to do any logic if the selections are incorrect
        return definition;
    }

    const modelBodyQ = qOwnerBody(modelFaceQ);
    const modelParameters = getParameters(context, definition, getModelParameters(context, qOwnerBody(modelFaceQ)), modelFaceQ, definition.bendReference);

    // like regen but with `holdOtherSide` always false
    var modifiedDefinition = definition;
    modifiedDefinition.holdOtherSide = false;
    const bendBoundaries = generateBendBoundaries(context, id + "boundaries", modifiedDefinition, modelFaceQ, modelParameters);
    const imprintResult = imprintBendBoundaries(context, id + "imprint", modelFaceQ, bendBoundaries);
    const surfacePieces = decomposeModelSurface(context, id + "decompose", imprintResult);
    if (size(evaluateQuery(context, surfacePieces.fixedSurface)) > 1)
    {
        definition.holdOtherSide = true;
        return definition;
    }
    const faceCountDiff = size(evaluateQuery(context, qOwnedByBody(surfacePieces.fixedSurface, EntityType.FACE))) - size(evaluateQuery(context, qOwnedByBody(surfacePieces.movingSurface, EntityType.FACE)));
    if (faceCountDiff < 0)
    {
        definition.holdOtherSide = true;
    }
    else if (faceCountDiff > 0)
    {
        definition.holdOtherSide = false;
    }
    else
    {
        const edgeCountDiff = size(evaluateQuery(context, qOwnedByBody(surfacePieces.fixedSurface, EntityType.EDGE))) - size(evaluateQuery(context, qOwnedByBody(surfacePieces.movingSurface, EntityType.EDGE)));
        if (edgeCountDiff < 0)
        {
            definition.holdOtherSide = true;
        }
        else if (edgeCountDiff > 0)
        {
            definition.holdOtherSide = false;
        }
        else
        {
            const fBox = evBox3d(context, {
                        "topology" : surfacePieces.fixedSurface,
                        "tight" : false
                    });
            const mBox = evBox3d(context, {
                        "topology" : surfacePieces.movingSurface,
                        "tight" : false
                    });
            const fSize = box3dDiagonalLength(fBox);
            const mSize = box3dDiagonalLength(mBox);
            if (fSize < mSize)
            {
                definition.holdOtherSide = true;
            }
            else
            {
                definition.holdOtherSide = false;
            }
        }
    }
    return definition;
}

function applyEditLogicForFace(context is Context, id is Id, oldDefinition is map, definition is map, specifiedParameters is map, hiddenBodies is Query) returns map
{
    // We will use a silent try here because its ok if the entity is not from a sketch, we just don't bother going any further
    var thePlane;
    try silent
    {
        thePlane = evOwnerSketchPlane(context, { "entity" : definition.bendReference });
    }
    catch
    {
        // Not a sketch line
        return definition;
    }

    const facesQ = qAllSolidBodies()->qSubtraction(hiddenBodies)->qActiveSheetMetalFilter(ActiveSheetMetal.YES)->qOwnedByBody(EntityType.FACE)->qCoincidesWithPlane(thePlane);
    const faces = evaluateQuery(context, facesQ);
    // If only one face matches then use that
    if (size(faces) == 1)
    {
        definition.face = faces[0];
    }
    return definition;
}

type BendParameters typecheck canBeBendParameters;

predicate canBeBendParameters(value)
{
    isAngle(value.angle);
    isLength(value.bendRadius); // The radius of the bend specified in the feature
    isLength(value.modelRadius); // The radius of the cylinder in the sheet metal model
    isLength(value.midSurfaceRadius); // The radius of the mid-surface
    isLength(value.thickness);
    value.kFactor is number;
    isLength(value.bendAllowance);
}

function extractParallelNormal(context is Context, referenceQ is Query, bendDirection, referenceId is string) returns Vector
precondition
{
    is3dDirection(bendDirection);
}
{
    // We handle different types of geometry in different ways. For almost every type passed in here
    // the extracted direction is the normal to the plane of the entity but for lines it is the line's direction NOT the plane of the line.
    // From a user perspective the expectation is that it is parallel to the entity so we need different processing for lines.
    var direction = undefined;
    try silent
    {
        direction = evLine(context, { "edge" : referenceQ }).direction;
    }
    if (direction != undefined)
    {
        // There is no single plane in which a line sits, so we use the bend line to get a plane parallel to both lines, i.e.
        // the parallel normal is the cross product of the two vectors
        // If the direction is parallel to the bend direction then there is no unique answer, no unique way to define the angle so we fail
        if (parallelVectors(direction, bendDirection))
            throw regenError(ErrorStringEnum.ANGLE_CONTROL_PARALLEL_TO_BEND, [referenceId]);
        return normalize(cross(bendDirection, direction));
    }
    else
    {
        // Not a line. This doesn't mean that every direction is sensible. If it is not perpendicular to the bend direction then
        // it doesn't make sense
        const normal = extractDirection(context, referenceQ);
        if (!perpendicularVectors(normal, bendDirection))
            throw regenError(ErrorStringEnum.ANGLE_CONTROL_PARALLEL_TO_BEND, [referenceId]);
        else
            return normal;
    }
}


function calculateAngle(context is Context, definition is map, faceQ is Query, bendReferenceQ is Query) returns ValueWithUnits
precondition
{
    definition.angleControlType != BendAngleControlType.BEND_ANGLE || isAngle(definition.bendAngle);
    definition.angleControlType != BendAngleControlType.ALIGN_GEOMETRY || definition.parallelEntity is Query;
    definition.angleControlType != BendAngleControlType.ANGLE_FROM_DIRECTION || (definition.directionEntity is Query && isAngle(definition.angleFromDirection));
}
{
    if (definition.angleControlType == BendAngleControlType.BEND_ANGLE)
    {
        return definition.bendAngle;
    }
    else if (definition.angleControlType == BendAngleControlType.ALIGN_GEOMETRY ||
        definition.angleControlType == BendAngleControlType.ANGLE_FROM_DIRECTION)
    {
        const smPlane = evPlane(context, { "face" : faceQ });
        const refLine = evLine(context, { "edge" : bendReferenceQ });
        // If we are doing "hold other" then we need to reverse the line direction for the calculation for consistency with regeneration
        const referenceDirection = refLine.direction * (definition.holdOtherSide ? -1 : 1);
        var direction;
        if (definition.angleControlType == BendAngleControlType.ALIGN_GEOMETRY)
            direction = extractParallelNormal(context, definition.parallelEntity, referenceDirection, "parallelEntity");
        else
            direction = extractParallelNormal(context, definition.directionEntity, referenceDirection, "directionEntity");
        // The direction vector and the sheet metal need not be perpendicular, as with alignment, it is all about the final angles
        var angle = angleBetween(smPlane.normal, direction, -referenceDirection);
        if (definition.angleControlType == BendAngleControlType.ANGLE_FROM_DIRECTION)
        {
            if (definition.oppositeAngleFromDirection != definition.oppositeAngle)
                angle -= definition.angleFromDirection;
            else
                angle += definition.angleFromDirection;
        }
        const halfCircle = PI * radian;
        if (definition.oppositeAngle)
        {
            // We have calculated the angle based on the "positive side". If we have opposite angle set then we don't now negate that angle, we subtract it from 180 degrees
            angle = halfCircle - angle;
        }
        // Angles less than zero or more than a half circle need to be adjusted
        if (angle < 0)
            angle += halfCircle;
        else if (angle > halfCircle)
            angle -= halfCircle;
        return angle;
    }
    else
    {
        throw regenError(ErrorStringEnum.INVALID_INPUT, ["angleControlType"]);
    }
}

function getParameters(context is Context, definition is map, modelParameters is map, sheetMetalFaceQ is Query, bendReferenceQ is Query) returns BendParameters
precondition
{
    isLength(modelParameters.frontThickness);
    isLength(modelParameters.backThickness);
    isLength(modelParameters.defaultBendRadius);
    definition.useDefaultRadius || isLength(definition.bendRadius);
}
{
    const radius = definition.useDefaultRadius ? modelParameters.defaultBendRadius : definition.bendRadius;
    const thickness = modelParameters.frontThickness + modelParameters.backThickness;
    const kFactor = definition.useDefaultKFactor ? modelParameters['k-factor'] : definition.kFactor;
    const angle = calculateAngle(context, definition, sheetMetalFaceQ, bendReferenceQ);
    return {
                "angle" : angle,
                "thickness" : thickness,
                "kFactor" : kFactor,
                "bendAllowance" : calculateBendAllowance(radius, angle, thickness, kFactor),
                "midSurfaceRadius" : radius + (kFactor * thickness),
                "modelRadius" : radius + (definition.oppositeAngle ? modelParameters.frontThickness : modelParameters.backThickness),
                "bendRadius" : radius

            } as BendParameters;
}


function calculateBendAllowance(radius, angle, thickness, kFactor is number) returns ValueWithUnits
precondition
{
    isLength(radius);
    isAngle(angle);
    isLength(thickness);
}
{
    return (angle / radian) * (radius + (kFactor * thickness));
}

function checkInputQueries(context is Context, definition is map) returns Query
{
    // Checking in the order listed in the feature dialog
    try silent
    {
        evLine(context, { "edge" : definition.bendReference });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_NO_BEND_LINE, ["bendReference"]);
    }
    const modelFaceQ = getSheetMetalModelFace(context, definition.face);
    try silent
    {
        evPlane(context, { "face" : modelFaceQ });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_NO_FACE, ["face"]);
    }
    if (definition.angleControlType == BendAngleControlType.ALIGN_GEOMETRY)
    {
        var direction;
        try silent
        {
            direction = extractDirection(context, definition.parallelEntity);
        }
        if (direction == undefined)
            throw regenError(ErrorStringEnum.SHEET_METAL_BEND_NO_PARALLEL, ["parallelEntity"]);
    }
    else if (definition.angleControlType == BendAngleControlType.ANGLE_FROM_DIRECTION)
    {
        var direction;
        try silent
        {
            direction = extractDirection(context, definition.directionEntity);
        }
        if (direction == undefined)
            throw regenError(ErrorStringEnum.SHEET_METAL_BEND_NO_DIRECTION, ["directionEntity"]);
    }
    return modelFaceQ;
}

function getSheetMetalModelFace(context is Context, partFaceQ is Query) returns Query
{
    const definitionFaces = getSMDefinitionEntities(context, partFaceQ);
    if (size(definitionFaces) != 1)
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_NO_FACE, ["face"]);
    else
        return definitionFaces[0];
}

type BendBoundaries typecheck canBeBendBoundaries;

predicate canBeBendBoundaries(value)
{
    value.fixedBoundaryPlane is Query;
    value.movingBoundaryPlane is Query;
}

function isFlatAlignment(alignment is BendAlignment) returns boolean
{
    return alignment != BendAlignment.BENT_MIDPLANE && alignment != BendAlignment.BENT_OUTSIDE && alignment != BendAlignment.BENT_INSIDE;
}

function forkSeedSurface(context is Context, id is Id, seedSurfaceQ is Query, fixedOffset, movingOffset) returns BendBoundaries
precondition
{
    is3dLengthVector(fixedOffset);
    is3dLengthVector(movingOffset);
}
{
    opPattern(context, id, {
                "entities" : qOwnerBody(seedSurfaceQ),
                "transforms" : [transform(fixedOffset), transform(movingOffset)],
                "instanceNames" : ["fixed", "moving"]
            });
    return { "fixedBoundaryPlane" : qPatternInstances(id, "fixed", EntityType.FACE), "movingBoundaryPlane" : qPatternInstances(id, "moving", EntityType.FACE) } as BendBoundaries;
}

function addExtrudeBounds(context is Context, extrudeDefinition is map, faceQ is Query) returns map
{
    // We need to extrude far enough to cut through the face.
    // We have the direction, we also have the line that is being extruded.
    // It may be that the line is not perpendicular to the direction of extrusion so we need to account for that
    const direction = extrudeDefinition.direction;

    const ends = mapArray(evEdgeTangentLines(context, {
                    "edge" : extrudeDefinition.entities,
                    "parameters" : [0, 1]
                }), function(lineIn)
        {
            return lineIn.origin;
        });

    const faceBox = evBox3d(context, {
                "topology" : faceQ,
                "tight" : false,
                "cSys" : coordSystem(ends[0], perpendicularVector(direction), direction)
            });

    const lineSize = abs(dot(ends[1] - ends[0], direction));
    const padding = 0.01 * meter;

    const high = faceBox.maxCorner[2] + (lineSize + padding);

    extrudeDefinition.startBound = BoundingType.BLIND;
    extrudeDefinition.startDepth = -(faceBox.minCorner[2] - (lineSize + padding));
    extrudeDefinition.endBound = BoundingType.BLIND;
    extrudeDefinition.endDepth = faceBox.maxCorner[2] + (lineSize + padding);
    return extrudeDefinition;
}

function generateFlatAlignedBoundarySheets(context is Context, id is Id, sheetMetalModelFaceQ is Query, bendLineQ is Query, parameters is BendParameters, holdOtherSide is boolean, alignment is BendAlignment) returns BendBoundaries
{
    var modelPlane = evPlane(context, { "face" : sheetMetalModelFaceQ });
    const dropId = id + "drop";
    const bendLine = evLine(context, { "edge" : bendLineQ });
    if (parallelVectors(bendLine.direction, modelPlane.normal))
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_LINE_PERPENDICULAR_TO_FACE, ["bendReference"]);
    try
    {
        const extrudeDefinition = {
                "entities" : bendLineQ,
                "direction" : holdOtherSide ? -modelPlane.normal : modelPlane.normal
            };
        opExtrude(context, dropId + "face", addExtrudeBounds(context, extrudeDefinition, sheetMetalModelFaceQ));

        const direction = project(modelPlane, bendLine).direction;
        var transverse = cross(modelPlane.normal, direction);
        if (holdOtherSide)
        {
            transverse *= -1;
        }

        var forward;
        var backward;
        if (alignment == BendAlignment.HELD_EDGE)
        {
            forward = 1;
            backward = 0;
        }
        else if (alignment == BendAlignment.BENT_EDGE)
        {
            forward = 0;
            backward = -1;
        }
        else if (alignment == BendAlignment.BEND_LINE)
        {
            forward = 0.5;
            backward = -0.5;
        }
        else
        {
            throw regenError(ErrorStringEnum.INVALID_INPUT);
        }

        const boundaryBodyQ = qCreatedBy(dropId, EntityType.BODY)->qBodyType(BodyType.SHEET);
        const results = forkSeedSurface(context, id, boundaryBodyQ, backward * transverse * parameters.bendAllowance, forward * transverse * parameters.bendAllowance);
        opDeleteBodies(context, id + "deleteDrop", { "entities" : qCreatedBy(dropId, EntityType.BODY) });
        return results;
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_BAD_BEND_LINE, ["bendReference"]);
    }
}

function generateModelAlignedBoundarySheets(context is Context, id is Id, sheetMetalModelFaceQ is Query, bendLineQ is Query, parameters is BendParameters, holdOtherSide is boolean, alignment is BendAlignment, oppositeAngle is boolean) returns BendBoundaries
{
    // Given the 'bend line' we will find the plane that goes through that line, at the requisite angle.
    var modelPlane = evPlane(context, { "face" : sheetMetalModelFaceQ });
    var bendLine = evLine(context, { "edge" : bendLineQ });
    if (!perpendicularVectors(bendLine.direction, modelPlane.normal))
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_LINE_PERPENDICULAR_TO_FACE, ["bendReference"]);

    const rotation = rotationAround(bendLine, (parameters.angle - (90 * degree)) * (holdOtherSide ? 1 : -1) * (oppositeAngle ? -1 : 1));
    const extrusionDirection = rotation.linear * modelPlane.normal * (holdOtherSide ? -1 : 1);

    try
    {
        const dropId = id + "drop";
        const extrudeDefinition = {
                "entities" : bendLineQ,
                "direction" : extrusionDirection
            };
        opExtrude(context, dropId + "face", addExtrudeBounds(context, extrudeDefinition, sheetMetalModelFaceQ));

        // Now we need to work out the offset, from the projected line to yield the result at that location
        var transverse = cross(modelPlane.normal, bendLine.direction);
        if (holdOtherSide)
        {
            transverse *= -1;
        }

        var thicknessRatio;
        if (alignment == BendAlignment.BENT_MIDPLANE)
        {
            thicknessRatio = 0.5;
        }
        else if (alignment == BendAlignment.BENT_OUTSIDE)
        {
            thicknessRatio = 1.0;
        }
        else if (alignment == BendAlignment.BENT_INSIDE)
        {
            thicknessRatio = 0.0;
        }
        else
        {
            throw regenError(ErrorStringEnum.INVALID_INPUT);
        }
        if (oppositeAngle)
            thicknessRatio -= 1.0;
        const offset = parameters.modelRadius * tan(parameters.angle * 0.5);
        const adjustment = parameters.thickness * thicknessRatio / sin(parameters.angle);
        var backward = -(offset + adjustment);
        var forward = backward + parameters.bendAllowance;

        const boundaryBodyQ = qCreatedBy(dropId, EntityType.BODY)->qBodyType(BodyType.SHEET);
        const results = forkSeedSurface(context, id, boundaryBodyQ, backward * transverse, forward * transverse);
        opDeleteBodies(context, id + "deleteDrop", { "entities" : qCreatedBy(dropId, EntityType.BODY) });
        return results;
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_BAD_BEND_LINE, ["bendReference"]);
    }
}


function generateBendBoundaries(context is Context, id is Id, definition is map, sheetMetalModelFaceQ is Query, parameters is BendParameters) returns BendBoundaries
precondition
{
    definition.bendReference is Query;
    definition.holdOtherSide is boolean;
    definition.bendAlignment is BendAlignment;
    definition.oppositeAngle is boolean;
}
{
    if (isFlatAlignment(definition.bendAlignment))
    {
        return generateFlatAlignedBoundarySheets(context, id, sheetMetalModelFaceQ, definition.bendReference, parameters, definition.holdOtherSide, definition.bendAlignment);
    }
    else
    {
        return generateModelAlignedBoundarySheets(context, id, sheetMetalModelFaceQ, definition.bendReference, parameters, definition.holdOtherSide, definition.bendAlignment, definition.oppositeAngle);
    }
}

type ImprintResult typecheck canBeImprintResult;

predicate canBeImprintResult(value)
{
    value.fixedBoundary is Query; // This is a tracking query and resolves to the relevant edge on various bodies through the process
    value.movingBoundary is Query; // This is a tracking query and resolves to the relevant edge on various bodies through the process
    value.bendFaces is Query;
}


function imprintBendBoundaries(context is Context, id is Id, modelFaceQ is Query, boundaries is BendBoundaries) returns ImprintResult
{
    const splitByFixedLineQ = startTracking(context, { "subquery" : boundaries.fixedBoundaryPlane, "trackPartialDependency" : true });
    const splitByMovingLineQ = startTracking(context, { "subquery" : boundaries.movingBoundaryPlane, "trackPartialDependency" : true });
    try
    {
        opSplitFace(context, id, {
                    "faceTargets" : modelFaceQ,
                    "faceTools" : qUnion(boundaries.fixedBoundaryPlane, boundaries.movingBoundaryPlane),
                    "extendToCompletion" : true
                });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_IMPRINT_FAILED, ["face", "bendReference"]);
    }
    if (size(evaluateQuery(context, qUnion([qSplitBy(id, EntityType.FACE, true), qSplitBy(id, EntityType.FACE, false)]))) < 3)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_BAD_DECOMPOSITION, ["bendReference"]);
    }

    opDeleteBodies(context, id + "cleanup", {
                "entities" : qUnion([boundaries.fixedBoundaryPlane, boundaries.movingBoundaryPlane])
            });

    const fixedEdgesQ = qEntityFilter(splitByFixedLineQ, EntityType.EDGE);
    const movingEdgesQ = qEntityFilter(splitByMovingLineQ, EntityType.EDGE);
    const fixedFacesQ = qAdjacent(fixedEdgesQ, AdjacencyType.EDGE, EntityType.FACE);
    const movingFacesQ = qAdjacent(movingEdgesQ, AdjacencyType.EDGE, EntityType.FACE);
    return {
                "fixedBoundary" : fixedEdgesQ,
                "movingBoundary" : movingEdgesQ,
                "bendFaces" : qIntersection(fixedFacesQ, movingFacesQ)
            } as ImprintResult;
}

type SurfacePieces typecheck canBeSurfacePieces;

predicate canBeSurfacePieces(value)
{
    value.fixedSurface is Query;
    value.movingSurface is Query;
    value.flatBendSurface is Query;
}

enum BreakRipResult
{
    BREAK_RIP_SUCCEEDED,
    BREAK_RIP_NOT_ATTEMPTED,
    BREAK_RIP_CANT_BREAK_BUTTS
}

function breakRips(context is Context, id is Id, boundaryEdgeQ is Query) returns BreakRipResult
{
    // For every face, if ALL the edges (except the boundary) are rips then we can break all the rips
    // otherwise we can't.
    // Note that this is only handling simple cases, the likely more common ones.
    // If there is a chain of rips that spans multiple faces then this will not work though a more complex algorithm
    // could be written to do that.
    // There are often issues with de-ripping butt style rips and, for now, we will simply exclude them, with a different error.
    const facesQ = qAdjacent(boundaryEdgeQ, AdjacencyType.EDGE, EntityType.FACE);
    // We can ignore laminar edges. If we have some then they are not contributing to the problem and they can't fix it
    const edgesQ = qAdjacent(facesQ, AdjacencyType.EDGE, EntityType.EDGE)->qSubtraction(boundaryEdgeQ)->qEdgeTopologyFilter(EdgeTopology.TWO_SIDED);
    const edges = evaluateQuery(context, edgesQ);
    if (edges == [])
    {
        return BreakRipResult.BREAK_RIP_NOT_ATTEMPTED; // No edges, can't do anything
    }
    const edgeCount = size(edges);
    var foundNonEdgeRips = false;
    for (var j = 0; j < edgeCount; j += 1)
    {
        const jointAttribute = try silent(getJointAttribute(context, edges[j]));
        if (jointAttribute == undefined || jointAttribute.jointType == undefined || jointAttribute.jointStyle == undefined)
        {
            return BreakRipResult.BREAK_RIP_NOT_ATTEMPTED;
        }
        if (jointAttribute.jointType.value != SMJointType.RIP)
        {
            return BreakRipResult.BREAK_RIP_NOT_ATTEMPTED;
        }
        if (jointAttribute.jointStyle.value != SMJointStyle.EDGE)
        {
            foundNonEdgeRips = true;
        }
    }

    if (foundNonEdgeRips)
    {
        return BreakRipResult.BREAK_RIP_CANT_BREAK_BUTTS;
    }

    try
    {
        // boolean failure means we didn't try to de-rip
        return deripEdges(context, id, edgesQ) ? BreakRipResult.BREAK_RIP_SUCCEEDED : BreakRipResult.BREAK_RIP_NOT_ATTEMPTED;
    }
    catch
    {
        // If we fail to derip exceptionally then we do not return, we throw.
        throw regenError(ErrorStringEnum.SHEET_METAL_BOTH_SIDES_CONNECTED);
    }
}

function decomposeModelSurface(context is Context, id is Id, imprintResult is ImprintResult)
{
    const surfaceCloneId = id + "copyBend";
    opExtractSurface(context, surfaceCloneId, {
                "faces" : imprintResult.bendFaces,
                "offset" : 0 * meter,
                "tangentPropagation" : false
            });
    const bendSurfaceQ = qCreatedBy(surfaceCloneId, EntityType.BODY);
    const splitFixedEdgesQ = startTracking(context, imprintResult.fixedBoundary);
    const splitMovingEdgesQ = startTracking(context, imprintResult.movingBoundary);
    // When we do delete face we will find that the bendFaces query also resolves to the face we just copied! So filter it out
    opDeleteFace(context, id + "split", {
                "deleteFaces" : qSubtraction(imprintResult.bendFaces, qOwnedByBody(bendSurfaceQ, EntityType.FACE)),
                "includeFillet" : false,
                "capVoid" : false,
                "leaveOpen" : true
            });


    const fixedBodiesQ = qOwnerBody(splitFixedEdgesQ);
    const movingBodiesQ = qOwnerBody(splitMovingEdgesQ);
    const commonBodiesQ = qIntersection([fixedBodiesQ, movingBodiesQ]);
    if (!isQueryEmpty(context, commonBodiesQ))
    {
        // If we can break the rips in the moving pieces we are good to go,
        // otherwise we try the fixed ones, and if we can't break anything we're doomed to failure
        const movingAttempt = breakRips(context, id + "unripMoving", splitMovingEdgesQ);
        if (movingAttempt != BreakRipResult.BREAK_RIP_SUCCEEDED)
        {
            const fixedAttempt = breakRips(context, id + "unripFixed", splitFixedEdgesQ);
            if (fixedAttempt == BreakRipResult.BREAK_RIP_SUCCEEDED)
            {
                // Nothing to do
            }
            else if (fixedAttempt == BreakRipResult.BREAK_RIP_CANT_BREAK_BUTTS || movingAttempt == BreakRipResult.BREAK_RIP_CANT_BREAK_BUTTS)
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_CANNOT_BEND_BUTTS, qUnion([splitFixedEdgesQ, splitMovingEdgesQ]));
            }
            else
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_BOTH_SIDES_CONNECTED, qUnion([splitFixedEdgesQ, splitMovingEdgesQ]));
            }
        }
        else if (!isQueryEmpty(context, commonBodiesQ))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_BOTH_SIDES_CONNECTED, qUnion([splitFixedEdgesQ, splitMovingEdgesQ]));
        }
    }

    return {
                "fixedSurface" : qUnion(evaluateQuery(context, fixedBodiesQ)),
                "movingSurface" : qUnion(evaluateQuery(context, movingBodiesQ)),
                "flatBendSurface" : bendSurfaceQ
            } as SurfacePieces;
}

function wrapBendSurface(context is Context, id is Id, imprints is ImprintResult, pieces is SurfacePieces, wrapRadius, finalRadius, oppositeAngle is boolean) returns Query
precondition
{
    isLength(wrapRadius);
    isLength(finalRadius);
}
{
    // The fixed edge stays where it is and is tangent to the original plane, the moving edge moves.
    const flatBendFaceQ = qOwnedByBody(pieces.flatBendSurface, EntityType.FACE);
    const lineDirection = evLine(context, { "edge" : imprints.fixedBoundary }).direction;
    const anchorPoint = evVertexPoint(context, { "vertex" : qEdgeVertex(imprints.fixedBoundary, true) });
    const oppositeFactor = oppositeAngle ? -1 : 1;
    var flatPlane = evPlane(context, { "face" : flatBendFaceQ });
    flatPlane.normal *= oppositeFactor;
    flatPlane.x *= oppositeFactor;
    const planeDefinition = {
                "anchorPoint" : anchorPoint,
                "anchorDirection" : lineDirection,
                "plane" : flatPlane
            } as WrapSurface;

    const planeNormal = planeDefinition.plane.normal;
    const cylinderDefinition = {
                "anchorPoint" : anchorPoint,
                "anchorDirection" : lineDirection,
                "cylinder" : {
                    "coordSystem" : {
                        "zAxis" : lineDirection,
                        "xAxis" : planeNormal,
                        "origin" : anchorPoint - planeNormal * wrapRadius
                    },
                    "radius" : wrapRadius
                }
            } as WrapSurface;

    const wrapId = id + "wrap";
    const wrappedQ = qCreatedBy(wrapId, EntityType.BODY);
    try
    {
        opWrap(context, wrapId, {
                    "wrapType" : WrapType.SIMPLE,
                    "entities" : flatBendFaceQ,
                    "source" : planeDefinition,
                    "destination" : cylinderDefinition
                });
        // Wrap creates a new body
        opDeleteBodies(context, id + "cleanup", {
                    "entities" : pieces.flatBendSurface
                });
        // Scale around the anchorPoint with non-uniform scale, scaling the radius from wrapRadius to finalRadius
        opTransform(context, id + "transform", {
                    "bodies" : wrappedQ,
                    "transform" : scaleNonuniformly(finalRadius / wrapRadius, finalRadius / wrapRadius, 1.0,
                    coordSystem(anchorPoint, planeNormal, lineDirection))
                });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_ROLL_FAILED, qUnion([imprints.fixedBoundary, imprints.movingBoundary]));
    }

    return wrappedQ;
}

function transformMovingSurface(context is Context, id is Id, movingSurfaceQ is Query, imprints is ImprintResult, modelPlane is Plane, bendSurfaceQ is Query, angle)
precondition
{
    isAngle(angle);
}
{
    // This is actually really conceptually simple if you think about the preceding steps
    // We split a body into two. Now to put the moving part where it needs to be we translate it in the plane of the split face so that the moving piece is adjacent to the fixed piece
    // i.e. collapsing the gap we cut out for the bend. Then we rotate it around the bend cylinder to get to the other side of that.
    // It may be tempting to say you transform from the basis of the original edge of the moving surface to the basis of the bend edge but we don't
    // have any guarantee that the origins are compatible. Better to apply simpler transforms.

    // The queries in imprints are tracking queries and represent the edges on both the bend body and the moving body, which is great.
    const fixedEdgeQ = qOwnedByBody(imprints.fixedBoundary, bendSurfaceQ);
    const movingEdgeQ = qOwnedByBody(imprints.movingBoundary, movingSurfaceQ);

    // These should be parallel lines. We want a vector from the moving edge to the fixed edge
    const fixedLine = evLine(context, { "edge" : fixedEdgeQ });
    const movingLine = evLine(context, { "edge" : movingEdgeQ });
    const betweenOrigins = (fixedLine.origin - movingLine.origin);
    const translation = transform(betweenOrigins - (dot(betweenOrigins, fixedLine.direction) * fixedLine.direction));

    const cylinderInfo = evSurfaceDefinition(context, {
                "face" : qOwnedByBody(bendSurfaceQ, EntityType.FACE)
            });

    // The line direction is key, It needs to be consistent with the cross product of 'betweenOrigins' with the model face normal to make it a consistent direction
    var rotationDirection = cylinderInfo.coordSystem.zAxis;
    const positiveDirection = cross(normalize(betweenOrigins), modelPlane.normal);
    if (dot(positiveDirection, rotationDirection) < 0)
        rotationDirection *= -1;
    const rotationLine = line(cylinderInfo.coordSystem.origin, rotationDirection);
    const rotation = rotationAround(rotationLine, angle);

    opTransform(context, id + "transform", {
                "bodies" : movingSurfaceQ,
                "transform" : rotation * translation
            });
}

function composeModelSurfaces(context is Context, id is Id, sheets)
precondition
{
    sheets is array;
    for (var s in sheets)
    {
        s is Query;
    }
}
{
    try
    {
        opBoolean(context, id, {
                    "tools" : qUnion(sheets),
                    "operationType" : BooleanOperationType.UNION
                });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_COLLISION, qUnion(sheets));
    }
}

function annotateBendSurface(context is Context, id is Id, bendSheetQ is Query, radius, angle, kFactor is number)
precondition
{
    isLength(radius);
    isAngle(angle);
}
{
    const bendFaceQ = qOwnedByBody(bendSheetQ, EntityType.FACE);
    var attributeId = toAttributeId(id);
    var bendAttribute = makeSMJointAttribute(attributeId);
    bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited" : false };
    bendAttribute.bendType = { "value" : SMBendType.STANDARD, "canBeEdited" : false };
    bendAttribute.radius = {
            "value" : radius,
            "canBeEdited" : true,
            "controllingFeatureId" : attributeId,
            "defaultIdInFeature" : "useDefaultRadius",
            "parameterIdInFeature" : "bendRadius"
        };
    bendAttribute.angle = {
            "value" : angle,
            "canBeEdited" : false
        };
    bendAttribute['k-factor'] = {
            "value" : kFactor,
            "canBeEdited" : true,
            "controllingFeatureId" : attributeId,
            "defaultIdInFeature" : "useDefaultKFactor",
            "parameterIdInFeature" : "kFactor"
        };
    setAttribute(context, { "entities" : bendFaceQ, "attribute" : bendAttribute });
}

