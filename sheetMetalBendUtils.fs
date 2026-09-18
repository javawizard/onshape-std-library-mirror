FeatureScript 3083; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "3083.0");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "3083.0");
import(path : "onshape/std/containers.fs", version : "3083.0");
import(path : "onshape/std/curveGeometry.fs", version : "3083.0");
import(path : "onshape/std/evaluate.fs", version : "3083.0");
import(path : "onshape/std/feature.fs", version : "3083.0");
import(path : "onshape/std/moveFace.fs", version : "3083.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "3083.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "3083.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "3083.0");
import(path : "onshape/std/topologyUtils.fs", version : "3083.0");
import(path : "onshape/std/transform.fs", version : "3083.0");
import(path : "onshape/std/valueBounds.fs", version : "3083.0");
import(path : "onshape/std/vector.fs", version : "3083.0");
import(path : "onshape/std/wrapSurface.fs", version : "3083.0");

/**
 * @internal
 * An `AngleBoundSpec` for bends.  Bends can theoretically go full circle, but 90 degrees will be common.
 */
export const SM_BEND_ANGLE_BOUNDS =
{
            (degree) : [1, 90, 359.9]
        } as AngleBoundSpec;

/**
 * @internal
 */
export const SM_BEND_DIRECTION_ANGLE_BOUNDS =
{
            (degree) : [0, 90, 180]
        } as AngleBoundSpec;

/**
 * @internal
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
 * @internal
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
 * @internal
 */
export enum JogOffsetAnchor
{
    annotation { "Name" : "Inside" }
    INSIDE,
    annotation { "Name" : "Nominal" }
    NOMINAL,
    annotation { "Name" : "Outside" }
    OUTSIDE
}

/**
 * @internal
 */
export enum JogOffsetBoundingType
{
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Up to entity" }
    UP_TO_ENTITY,
    annotation { "Name" : "Thickness" }
    THICKNESS
}

/** @internal */
export predicate sheetMetalBendTopPredicate(definition is map)
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
}

/** @internal */
export predicate sheetMetalBendBottomPredicate(definition is map)
{
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

/** @internal */
export function doSheetMetalBend(context is Context, id is Id, definition is map, modelFaceQ is Query, createJog is boolean, firstBendOfJog is boolean)
{
    const modelPlane = evPlane(context, {
                "face" : modelFaceQ
            });
    const bendParameters = getBendParameters(context, definition, getModelParameters(context, qOwnerBody(modelFaceQ)), modelFaceQ, definition.bendReference);
    const bendBoundaries = generateBendBoundaries(context, id + "boundaries", definition, modelFaceQ, bendParameters);
    var clippedReturn;
    if (firstBendOfJog)
    {
        modelFaceQ = qUnion(modelFaceQ, startTracking(context, modelFaceQ));
        try
        {
            clippedReturn = computeJogSurfaces(context, id, definition, modelFaceQ, bendBoundaries, bendParameters);
            modelFaceQ = qEntityFilter(qOwnedByBody(modelFaceQ, clippedReturn.clippedSurface), EntityType.FACE);
        }
        catch (error)
        {
            if ((error is map) && (error.message != undefined))
            {
                throw error;
            }
            else
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_JOG_STRETCH_CLIPPED);
            }
        }
    }
    const imprintResult = imprintBendBoundaries(context, id + "imprint", modelFaceQ, bendBoundaries, createJog, firstBendOfJog);
    var surfacePieces = decomposeModelSurface(context, id + "decompose", imprintResult);
    if (firstBendOfJog)
    {
        surfacePieces.fixedSurface = clippedReturn.fixedSurface;
    }
    const wrappedSheetQ = wrapBendSurface(context, id + "wrapBend", imprintResult, surfacePieces, bendParameters.midSurfaceRadius, bendParameters.modelRadius, definition.oppositeAngle);
    const signedAngle = definition.oppositeAngle ? -bendParameters.angle : bendParameters.angle;
    const bendIsTowardsModelPlaneNormal = transformMovingSurface(context, id + "transform", surfacePieces, imprintResult, modelPlane, wrappedSheetQ, signedAngle);
    annotateBendSurface(context, id, wrappedSheetQ, bendParameters.bendRadius, bendParameters.angle, bendParameters.kFactor, createJog);
    var trackedMovingSurface = undefined;
    if (firstBendOfJog)
    {
        // Since the modelFace gets split in imprintBendBoundaries and since surfacePieces.movingSurface can have multiple faces, find the common face
        const movingFace = qIntersection(modelFaceQ, surfacePieces.movingSurface->qOwnedByBody(EntityType.FACE));
        trackedMovingSurface = qUnion(movingFace, startTracking(context, movingFace));
    }
    composeModelSurfaces(context, id + "compose", [surfacePieces.fixedSurface, wrappedSheetQ, surfacePieces.movingSurface], createJog);
    if (firstBendOfJog)
    {
        return { "fixedSurface" : surfacePieces.fixedSurface,
                "fixedPlane" : modelPlane,
                "fixedEdge" : imprintResult.fixedBoundary,
                "movingSurface" : trackedMovingSurface,
                "angle" : bendParameters.angle,
                "bendIsTowardsModelPlaneNormal" : bendIsTowardsModelPlaneNormal };
    }
    else
    {
        return { "fixedSurface" : surfacePieces.fixedSurface };
    }
}

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

/**
 * @internal
 * The editing logic for jog.
 * Other than the editing logic for bend, if the bendAngle is too big for the jog offset, follow the offset and compute the bendAngle
 */
export function onJogChange(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (isCreating && specifiedParameters.face != true && oldDefinition.bendReference != definition.bendReference)
    {
        try silent
        {
            definition = applyEditLogicForFace(context, id, oldDefinition, definition, specifiedParameters, hiddenBodies);
        }
    }

    // If the user has not specified the direction, we flip jog direction for up-to-entity if needed
    if (definition.offsetType == JogOffsetBoundingType.UP_TO_ENTITY && specifiedParameters.oppositeAngle != true)
    {
        try silent
        {
            definition = applyEditLogicForOppositeAngle(context, id, definition);
        }
    }

    // If the bendAngle is too big for the offset, follow the offset and compute the bendAngle
    definition.bendAngleSetProgrammatically = false;
    if (definition.angleControlType == BendAngleControlType.BEND_ANGLE)
    {
        try silent
        {
            definition = applyEditLogicForBendAngle(context, id, definition);
        }
    }

    // If we didn't modify 'hold other' and just changed a selection then re-evaluate hold other
    if (isCreating && specifiedParameters.holdOtherSide != true && (oldDefinition.face != definition.face || oldDefinition.bendReference != definition.bendReference))
    {
        try silent
        {
            definition = applyEditLogicForHoldOther(context, id, oldDefinition, definition, specifiedParameters);
        }
    }
    return definition;
}

function applyEditLogicForOppositeAngle(context is Context, id is Id, definition is map)
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

    const modParams = getModelParameters(context, qOwnerBody(modelFaceQ));
    const facePlane = evPlane(context, { "face" : modelFaceQ });
    const offsetDirection = definition.oppositeAngle ? facePlane.normal : facePlane.normal * -1;
    try silent
    {
        getOffsetToEntity(context, modParams, facePlane, definition, offsetDirection);
    }
    catch (error)
    {
        if (error is map && error.message == ErrorStringEnum.SHEET_METAL_JOG_OPPOSITE_DIRECTION)
        {
            definition.oppositeAngle = !definition.oppositeAngle;
        }
    }

    return definition;
}

function applyEditLogicForBendAngle(context is Context, id is Id, definition is map)
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

    const modParams = getModelParameters(context, qOwnerBody(modelFaceQ));
    const specifiedNominalOffset = computeJogOffset(context, modParams, definition, evPlane(context, { "face" : modelFaceQ }), undefined);
    const t = modParams.frontThickness + modParams.backThickness;
    const R = definition.useDefaultRadius ? modParams.defaultBendRadius : definition.bendRadius;
    const cosAngleAdjacentBends = 1 - (specifiedNominalOffset / (2 * R + t));
    // Check upfront if there is a bend angle that satisfies the user specified jog offset when the bends are adjacent to each other
    if (abs(cosAngleAdjacentBends) < 1)
    {
        const nominalOffsetForAngleAdjacentBends = computeNominalOffsetForAngleAdjacentBends(definition.bendAngle, R, t);

        if ((definition.bendAngle - PI * radian <= -TOLERANCE.zeroAngle * radian) &&
            (specifiedNominalOffset - nominalOffsetForAngleAdjacentBends <= TOLERANCE.zeroLength * meter))
        {
            definition.bendAngleReadOnly = acos(cosAngleAdjacentBends);
            definition.bendAngleSetProgrammatically = true;
        }
        else if ((definition.bendAngle - PI * radian >= TOLERANCE.zeroAngle * radian) &&
            (specifiedNominalOffset - nominalOffsetForAngleAdjacentBends >= TOLERANCE.zeroLength * meter))
        {
            definition.bendAngleReadOnly = 2 * PI * radian - acos(cosAngleAdjacentBends);
            definition.bendAngleSetProgrammatically = true;
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
    const bendParameters = getBendParameters(context, definition, getModelParameters(context, qOwnerBody(modelFaceQ)), modelFaceQ, definition.bendReference);

    // like regen but with `holdOtherSide` always false
    var modifiedDefinition = definition;
    modifiedDefinition.holdOtherSide = false;
    const bendBoundaries = generateBendBoundaries(context, id + "boundaries", modifiedDefinition, modelFaceQ, bendParameters);
    const imprintResult = imprintBendBoundaries(context, id + "imprint", modelFaceQ, bendBoundaries, false, false);
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
    // Make sure non-SheetMetalDefinitionEntityType.FACE faces do not sneak in
    var nFaces = 0;
    var theFace = qNothing();
    for (var face in evaluateQuery(context, facesQ))
    {
        const definitionEntities = getSMDefinitionEntities(context, face);
        if (size(definitionEntities) == 1 && !isQueryEmpty(context, definitionEntities[0]->qGeometry(GeometryType.PLANE)))
        {
            nFaces += 1;
            if (nFaces > 1)
            {
                return definition;
            }
            theFace = face;
        }
    }
    // If only one face matches then use that
    if (nFaces == 1)
    {
        definition.face = theFace;
    }
    return definition;
}

/**
 * Every fixedEdge/movingEdge should share a face with a movingEdge/fixedEdge i.e.
 * every fixedEdge should have a movingFace adjacent to it and every movingEdge should have a fixedFace adjacent to it.
 */
function checkSplitResults(context is Context, fixedEdgesQ is Query, movingEdgesQ is Query)
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V3081_CHECK_SPLITS_RESULTS))
    {
        return;
    }

    if (isQueryEmpty(context, fixedEdgesQ) || isQueryEmpty(context, movingEdgesQ))
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_BAD_DECOMPOSITION, ["bendReference"]);
    }

    const fixedFacesQ = qAdjacent(fixedEdgesQ, AdjacencyType.EDGE, EntityType.FACE);
    const movingFacesQ = qAdjacent(movingEdgesQ, AdjacencyType.EDGE, EntityType.FACE);

    for (var fixedEdge in evaluateQuery(context, fixedEdgesQ->qEdgeTopologyFilter(EdgeTopology.TWO_SIDED)))
    {
        if (isQueryEmpty(context, qIntersection(qAdjacent(fixedEdge, AdjacencyType.EDGE, EntityType.FACE), movingFacesQ)))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_BEND_BAD_DECOMPOSITION, ["bendReference"], fixedEdge);
        }
    }

    for (var movingEdge in evaluateQuery(context, movingEdgesQ->qEdgeTopologyFilter(EdgeTopology.TWO_SIDED)))
    {
        if (isQueryEmpty(context, qIntersection(qAdjacent(movingEdge, AdjacencyType.EDGE, EntityType.FACE), fixedFacesQ)))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_BEND_BAD_DECOMPOSITION, ["bendReference"], movingEdge);
        }
    }
}

function markCollapsedWalls(context is Context, fixedEdgesQ is Query, toBendFacesQ is Query)
{
    for (var fixedEdge in evaluateQuery(context, fixedEdgesQ))
    {
        const fixedFaces = qAdjacent(fixedEdge, AdjacencyType.EDGE, EntityType.FACE);
        const jointAttribute = getJointAttribute(context, fixedFaces);
        // If one face adjacent to this fixedEdge is already a bend and if the other face is going to be a bend, mark the edge as a collapsed wall
        if (jointAttribute?.jointType?.value == SMJointType.BEND &&
            !isQueryEmpty(context, qIntersection(toBendFacesQ, fixedFaces)))
        {
            setAttribute(context, { "entities" : fixedEdge, "attribute" : asSMAttribute({ "objectType" : SMObjectType.COLLAPSED_WALL }) });
        }
    }
}

function computeJogSurfaces(context is Context, id is Id, definition is map, modelFaceQ is Query, bendBoundaries is BendBoundaries, bendParameters is BendParameters)
{
    const modelPlane = evPlane(context, { "face" : modelFaceQ });
    const bendDirection = project(modelPlane, evLine(context, { "edge" : definition.bendReference })).direction;
    var clippingDirection = cross(modelPlane.normal, bendDirection);
    if (definition.holdOtherSide)
    {
        clippingDirection *= -1;
    }

    const thickness = bendParameters.frontThickness + bendParameters.backThickness;
    const bendRadius = bendParameters.bendRadius;
    const angle = bendParameters.angle;
    const lengthOfBend = (bendRadius + bendParameters.kFactor * thickness) * (angle / radian);
    const lengthOfWallBetweenBends = computeLengthOfWallBetweenBends(context, definition, bendParameters, bendRadius, modelPlane, undefined, angle);
    const lengthOfBendsAndWallBetween = 2 * lengthOfBend + lengthOfWallBetweenBends;
    const lengthOfClippedPart = definition.preserveMaterial ? lengthOfBendsAndWallBetween : (2 * bendRadius + thickness) * sin(angle) + lengthOfWallBetweenBends * cos(angle);
    try
    {
        opPattern(context, id + "generateSplitPlanes", {
                    "entities" : bendBoundaries.fixedBoundaryPlane,
                    "transforms" : [transform(clippingDirection * (lengthOfBend + lengthOfWallBetweenBends)), transform(clippingDirection * lengthOfClippedPart)],
                    "instanceNames" : ["fixedOfBend2", "clipping"]
                });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_BAD_BEND_LINE, ["bendReference"]);
    }
    const clippingBoundaryPlane = qPatternInstances(id + "generateSplitPlanes", "clipping", EntityType.FACE);
    const splitByFixedLineQ = startTracking(context, { "subquery" : bendBoundaries.fixedBoundaryPlane, "trackPartialDependency" : true });
    const splitByClippingLineQ = startTracking(context, { "subquery" : clippingBoundaryPlane, "trackPartialDependency" : true });
    try
    {
        opSplitFace(context, id + "splitClipping", {
                    "faceTargets" : modelFaceQ,
                    "faceTools" : qUnion(bendBoundaries.fixedBoundaryPlane, clippingBoundaryPlane),
                    "ownExistingImprints" : true,
                    "extendToCompletion" : true
                });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_IMPRINT_FAILED, ["face", "bendReference"]);
    }
    const fixedEdgesQ = qEntityFilter(splitByFixedLineQ, EntityType.EDGE);
    const clippingEdgesQ = qEntityFilter(splitByClippingLineQ, EntityType.EDGE);
    checkSplitResults(context, fixedEdgesQ, clippingEdgesQ);
    const fixedFacesQ = qAdjacent(fixedEdgesQ, AdjacencyType.EDGE, EntityType.FACE);
    const clippingFacesQ = qAdjacent(clippingEdgesQ, AdjacencyType.EDGE, EntityType.FACE);
    markCollapsedWalls(context, fixedEdgesQ, clippingFacesQ);

    const fixedBodiesQ = qOwnerBody(fixedEdgesQ);
    const movingBodiesQ = qOwnerBody(clippingEdgesQ);
    const commonBodiesQ = qIntersection([fixedBodiesQ, movingBodiesQ]);
    if (!isQueryEmpty(context, commonBodiesQ))
    {
        // If we can break the rips in the moving pieces we are good to go,
        // otherwise we try the fixed ones.
        const movingAttempt = breakRips(context, id + "unripMoving", clippingEdgesQ, fixedEdgesQ);
        if (movingAttempt != BreakRipResult.BREAK_RIP_SUCCEEDED)
        {
            breakRips(context, id + "unripFixed", fixedEdgesQ, clippingEdgesQ);
        }
    }

    try
    {
        opExtractSurface(context, id + "copyClipped", {
                    "faces" : qIntersection(fixedFacesQ, clippingFacesQ),
                    "tangentPropagation" : false
                });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_JOG_STRETCH_CLIPPED);
    }
    const clippedSurfaceQ = qCreatedBy(id + "copyClipped", EntityType.BODY);
    const splitFixedEdgesQ = startTracking(context, fixedEdgesQ);
    const splitClippingEdgesQ = startTracking(context, clippingEdgesQ);
    try
    {
        // When we do delete face we will find that the clippingFacesQ query also resolves to the face we just copied! So filter it out
        opDeleteFace(context, id + "split", {
                    "deleteFaces" : qSubtraction(qIntersection(fixedFacesQ, clippingFacesQ), qOwnedByBody(clippedSurfaceQ, EntityType.FACE)),
                    "includeFillet" : false,
                    "capVoid" : false,
                    "leaveOpen" : true
                });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_JOG_STRETCH_CLIPPED);
    }

    const fixedSurface = qUnion(evaluateQuery(context, qOwnerBody(splitFixedEdgesQ)));
    const endSurface = qUnion(evaluateQuery(context, qOwnerBody(splitClippingEdgesQ)));

    if (!definition.preserveMaterial)
    {
        // Stretch the clipped surfaces to accommodate the two bends and the length that will be used for the wall between the bends
        const fixedEdgeLine = evLine(context, { "edge" : qOwnedByBody(fixedEdgesQ, clippedSurfaceQ) });
        opTransform(context, id + "stretchClipped", {
                    "bodies" : clippedSurfaceQ,
                    "transform" : scaleNonuniformly(1, 1, lengthOfBendsAndWallBetween / lengthOfClippedPart, coordSystem(fixedEdgeLine.origin, bendDirection, clippingDirection))
                });
        opTransform(context, id + "translateEndPart", {
                    "bodies" : endSurface,
                    "transform" : transform((lengthOfBendsAndWallBetween - lengthOfClippedPart) * clippingDirection)
                });
    }
    const clippedSurfaceFacesQ = qOwnedByBody(clippedSurfaceQ, EntityType.FACE);
    const clippedSurfaceFaces = evaluateQuery(context, clippedSurfaceFacesQ);
    const nClippedSurfaceFaces = size(clippedSurfaceFaces);
    for (var i = 0; i < nClippedSurfaceFaces; i = i + 1)
    {
        setAttribute(context, { "entities" : clippedSurfaceFaces[i], "attribute" : makeSMWallAttribute(toAttributeId(id + i)) });
    }

    const fixedPlaneOfBend2 = qPatternInstances(id + "generateSplitPlanes", "fixedOfBend2", EntityType.FACE);
    try
    {
        opSplitFace(context, id + "splitBendBoundaries", {
                    "faceTargets" : clippedSurfaceFacesQ,
                    "faceTools" : qUnion(bendBoundaries.movingBoundaryPlane, fixedPlaneOfBend2),
                    "ownExistingImprints" : true,
                    "extendToCompletion" : true
                });
        opDeleteBodies(context, id + "deleteClippingBoundaryPlane", {
                    "entities" : qUnion(fixedPlaneOfBend2, clippingBoundaryPlane)
                });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_IMPRINT_FAILED, ["face", "bendReference"]);
    }

    try
    {
        opBoolean(context, id + "mergeStretched", {
                    "tools" : qUnion(clippedSurfaceQ, endSurface),
                    "eraseImprintedEdges" : false,
                    "operationType" : BooleanOperationType.UNION
                });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_COLLISION, qUnion(clippedSurfaceQ, endSurface));
    }

    return { "fixedSurface" : fixedSurface,
            "clippedSurface" : qUnion(clippedSurfaceQ, endSurface) };
}

type BendParameters typecheck canBeBendParameters;

predicate canBeBendParameters(value)
{
    isAngle(value.angle);
    isLength(value.bendRadius); // The radius of the bend specified in the feature
    isLength(value.modelRadius); // The radius of the cylinder in the sheet metal model
    isLength(value.midSurfaceRadius); // The radius of the mid-surface
    isLength(value.frontThickness);
    isLength(value.backThickness);
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

function getBendParameters(context is Context, definition is map, modelParameters is map, sheetMetalFaceQ is Query, bendReferenceQ is Query) returns BendParameters
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
                "frontThickness" : modelParameters.frontThickness,
                "backThickness" : modelParameters.backThickness,
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

/** @internal */
export function checkInputs(context is Context, id is Id, definition is map, createJog is boolean) returns map
{
    const modelFaceQ = checkInputQueries(context, definition);

    if (createJog)
    {
        const modelBodyQ = qOwnerBody(modelFaceQ);
        const modParams = getModelParameters(context, modelBodyQ);

        const specifiedNominalOffset = computeJogOffset(context, modParams, definition, evPlane(context, { "face" : modelFaceQ }), undefined);
        if (specifiedNominalOffset < TOLERANCE.zeroLength * meter)
        {
            if (definition.offsetType == JogOffsetBoundingType.BLIND)
            {
                if (definition.bendOffsetAnchor == JogOffsetAnchor.OUTSIDE)
                {
                    throw regenError(ErrorStringEnum.SHEET_METAL_JOG_BLIND_OUTSIDE, ["bendOffset"]);
                }
                else if (definition.bendOffsetAnchor == JogOffsetAnchor.NOMINAL)
                {
                    throw regenError(ErrorStringEnum.SHEET_METAL_JOG_BLIND_NOMINAL, ["bendOffset"]);
                }
            }
            else if (definition.offsetType == JogOffsetBoundingType.THICKNESS)
            {
                if (definition.bendOffsetAnchor == JogOffsetAnchor.OUTSIDE)
                {
                    throw regenError(ErrorStringEnum.SHEET_METAL_JOG_THICKNESS_OUTSIDE, ["thicknessFactor"]);
                }
                else if (definition.bendOffsetAnchor == JogOffsetAnchor.NOMINAL)
                {
                    throw regenError(ErrorStringEnum.SHEET_METAL_JOG_THICKNESS_NOMINAL, ["thicknessFactor"]);
                }
            }
            else // definition.offsetType == JogOffsetBoundingType.UP_TO_ENTITY
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_JOG_UP_TO_ENTITY, ["jogLimitDistance"]);
            }
        }

        // If the bendAngle is too big for the offset, follow the offset and compute the bendAngle
        definition.bendAngleSetProgrammatically = false;
        if (definition.angleControlType == BendAngleControlType.BEND_ANGLE)
        {
            definition = applyEditLogicForBendAngle(context, id, definition);
            if (definition.bendAngleSetProgrammatically)
            {
                reportFeatureInfo(context, id, ErrorStringEnum.SHEET_METAL_JOG_BEND_ANGLE_UPDATED);
                definition.bendAngle = definition.bendAngleReadOnly;
                setFeatureComputedParameter(context, id, { "name" : "bendAngleReadOnly", "value" : definition.bendAngleReadOnly });
            }
            setFeatureComputedParameter(context, id, { "name" : "bendAngleSetProgrammatically", "value" : definition.bendAngleSetProgrammatically });
        }
        else
        {
            const angle = calculateAngle(context, definition, modelFaceQ, definition.bendReference);
            try
            {
                isAngle(angle, SM_BEND_ANGLE_BOUNDS);
            }
            catch
            {
                throw regenError(ErrorStringEnum.ANGLE_CONTROL_PARALLEL_TO_BEND, ["parallelEntity", "directionEntity", "angleFromDirection"]);
            }
        }
    }

    return { "modelFaceQ" : modelFaceQ,
            "definition" : definition };
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
        const offset = parameters.modelRadius * tan(parameters.angle * 0.5);
        const frontThicknessRatio = oppositeAngle ? thicknessRatio - 1 : thicknessRatio;
        var adjustment = 0 * meter;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2780_BEND_BACK_THICKNESS))
        {
            const backThicknessRatio = oppositeAngle ? thicknessRatio : thicknessRatio - 1;
            adjustment = (parameters.backThickness * backThicknessRatio + parameters.frontThickness * frontThicknessRatio) / sin(parameters.angle);
        }
        else
        {
            adjustment = frontThicknessRatio * (parameters.backThickness + parameters.frontThickness) / sin(parameters.angle);
        }
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

function imprintBendBoundaries(context is Context, id is Id, modelFaceQ is Query, boundaries is BendBoundaries, createJog is boolean, firstBendOfJog is boolean) returns ImprintResult
{
    const splitByFixedLineQ = startTracking(context, { "subquery" : boundaries.fixedBoundaryPlane, "trackPartialDependency" : true });
    const splitByMovingLineQ = startTracking(context, { "subquery" : boundaries.movingBoundaryPlane, "trackPartialDependency" : true });
    try
    {
        opSplitFace(context, id, {
                    "faceTargets" : modelFaceQ,
                    "faceTools" : qUnion(boundaries.fixedBoundaryPlane, boundaries.movingBoundaryPlane),
                    "ownExistingImprints" : createJog,
                    "extendToCompletion" : true
                });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_IMPRINT_FAILED, ["face", "bendReference"]);
    }

    const fixedEdgesQ = qEntityFilter(splitByFixedLineQ, EntityType.EDGE);
    const movingEdgesQ = qEntityFilter(splitByMovingLineQ, EntityType.EDGE);
    checkSplitResults(context, fixedEdgesQ, movingEdgesQ);
    const fixedFacesQ = qAdjacent(fixedEdgesQ, AdjacencyType.EDGE, EntityType.FACE);
    const movingFacesQ = qAdjacent(movingEdgesQ, AdjacencyType.EDGE, EntityType.FACE);
    const toBendFacesQ = qIntersection(fixedFacesQ, movingFacesQ);

    if (createJog)
    {
        if (isQueryEmpty(context, fixedEdgesQ))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_BEND_BAD_DECOMPOSITION, ["bendReference"]);
        }
        else
        {
            markCollapsedWalls(context, fixedEdgesQ, toBendFacesQ);
        }
        // For jogs, all the splits are done up-front, in computeJogSurfaces, so that the extrusions to generate the boundary planes are all done in the same direction.
        // This ensures that the query resolution for the faces between the bends is stable, irrespective of the bend angle of the jog.
        if (size(evaluateQuery(context, qUnion([qSplitBy(id, EntityType.FACE, true), qSplitBy(id, EntityType.FACE, false)]))) > 0)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_BEND_BAD_DECOMPOSITION, ["bendReference"]);
        }
    }
    else if (size(evaluateQuery(context, qUnion([qSplitBy(id, EntityType.FACE, true), qSplitBy(id, EntityType.FACE, false)]))) < 3)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_BAD_DECOMPOSITION, ["bendReference"]);
    }

    opDeleteBodies(context, id + "cleanup", {
                "entities" : qUnion([boundaries.fixedBoundaryPlane, boundaries.movingBoundaryPlane])
            });

    return {
                "fixedBoundary" : fixedEdgesQ,
                "movingBoundary" : movingEdgesQ,
                "bendFaces" : toBendFacesQ
            } as ImprintResult;
}

type SurfacePieces typecheck canBeSurfacePieces;

predicate canBeSurfacePieces(value)
{
    value.fixedSurface is Query;
    value.movingEdges is undefined || value.movingEdges is Query;
    value.movingSurface is Query;
    value.flatBendSurface is Query;
}

enum BreakRipResult
{
    BREAK_RIP_SUCCEEDED,
    BREAK_RIP_NOT_ATTEMPTED,
    BREAK_RIP_CANT_BREAK_BUTTS
}

function breakRips(context is Context, id is Id, boundaryEdgeQ is Query, otherBoundaryEdgeQ is Query) returns BreakRipResult
{
    // For every face, if ALL the edges (except the boundary) are rips then we can break all the rips
    // otherwise we can't.
    // Note that this is only handling simple cases, the likely more common ones.
    // If there is a chain of rips that spans multiple faces then this will not work though a more complex algorithm
    // could be written to do that.
    // There are often issues with de-ripping butt style rips and, for now, we will simply exclude them, with a different error.
    const facesQ = qAdjacent(boundaryEdgeQ, AdjacencyType.EDGE, EntityType.FACE);
    // We can ignore laminar edges. If we have some then they are not contributing to the problem and they can't fix it
    const edgesQ = qAdjacent(facesQ, AdjacencyType.EDGE, EntityType.EDGE)->qSubtraction(boundaryEdgeQ)->qSubtraction(otherBoundaryEdgeQ)->qEdgeTopologyFilter(EdgeTopology.TWO_SIDED);
    const edges = evaluateQuery(context, edgesQ);
    if (edges == [])
    {
        return BreakRipResult.BREAK_RIP_NOT_ATTEMPTED; // No edges, can't do anything
    }
    const canBreakButts = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2860_HANDLE_SPLIT_IN_DERIP);
    const edgeCount = size(edges);
    var foundNonEdgeRips = false;
    if (!canBreakButts)
    {
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

function decomposeModelSurfaceLegacy(context is Context, id is Id, imprintResult is ImprintResult)
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
        const movingAttempt = breakRips(context, id + "unripMoving", splitMovingEdgesQ, qNothing());
        if (movingAttempt != BreakRipResult.BREAK_RIP_SUCCEEDED)
        {
            const fixedAttempt = breakRips(context, id + "unripFixed", splitFixedEdgesQ, qNothing());
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

function decomposeModelSurface(context is Context, id is Id, imprintResult is ImprintResult)
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V3062_EXTRACT_SURFACE_AFTER_BREAK_RIPS))
    {
        return decomposeModelSurfaceLegacy(context, id, imprintResult);
    }

    if (!isQueryEmpty(context, qIntersection([qOwnerBody(imprintResult.fixedBoundary), qOwnerBody(imprintResult.movingBoundary)])))
    {
        // If we can break the rips in the moving pieces we are good to go,
        // otherwise we try the fixed ones, and if we can't break anything we're doomed to failure
        const movingAttempt = breakRips(context, id + "unripMoving", imprintResult.movingBoundary, imprintResult.fixedBoundary);
        if (movingAttempt != BreakRipResult.BREAK_RIP_SUCCEEDED)
        {
            const fixedAttempt = breakRips(context, id + "unripFixed", imprintResult.fixedBoundary, imprintResult.movingBoundary);
            if (fixedAttempt == BreakRipResult.BREAK_RIP_SUCCEEDED)
            {
                // Nothing to do
            }
            else if (fixedAttempt == BreakRipResult.BREAK_RIP_CANT_BREAK_BUTTS || movingAttempt == BreakRipResult.BREAK_RIP_CANT_BREAK_BUTTS)
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_CANNOT_BEND_BUTTS, qUnion([imprintResult.fixedBoundary, imprintResult.movingBoundary]));
            }
        }
    }

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

    if (!isQueryEmpty(context, qIntersection([fixedBodiesQ, movingBodiesQ])))
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BOTH_SIDES_CONNECTED, qUnion([splitFixedEdgesQ, splitMovingEdgesQ]));
    }

    return {
                "fixedSurface" : qUnion(evaluateQuery(context, fixedBodiesQ)),
                "movingEdges" : splitMovingEdgesQ,
                "movingSurface" : qUnion(evaluateQuery(context, movingBodiesQ)),
                "flatBendSurface" : bendSurfaceQ
            } as SurfacePieces;
}

function wrapBendSurface(context is Context, id is Id, imprints is ImprintResult, pieces is SurfacePieces, midSurfaceRadius, finalRadius, oppositeAngle is boolean) returns Query
precondition
{
    isLength(midSurfaceRadius);
    isLength(finalRadius);
}
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V3062_EXTRACT_SURFACE_AFTER_BREAK_RIPS))
    {
        imprints.fixedBoundary = qOwnedByBody(imprints.fixedBoundary, pieces.flatBendSurface);
    }
    // The fixed edge stays where it is and is tangent to the original plane, the moving edge moves.
    const flatBendFaceQ = qOwnedByBody(pieces.flatBendSurface, EntityType.FACE);
    //imprint might produce tolerant vertices
    //In such a case mismatch between edge location and anchor point causes
    // non-tangent edges around bend. Mid-edge will give us a better precision.
    const anchorFromEdge = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2974_BEND_PRESERVE_ARC);
    const fixedLine = (anchorFromEdge) ? evEdgeTangentLine(context, { "edge" : imprints.fixedBoundary, "parameter" : 0.5 }) : undefined;
    const lineDirection = (anchorFromEdge) ? fixedLine.direction : evLine(context, { "edge" : imprints.fixedBoundary }).direction;
    const anchorPoint = (anchorFromEdge) ? fixedLine.origin : evVertexPoint(context, { "vertex" : qEdgeVertex(imprints.fixedBoundary, true) });
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
    var wrapRadius = midSurfaceRadius;
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2903_SM_BEND_FIX))
    {
        // We are going to transform the flat bend surface, then wrap. Before this fix we would
        // wrap, then transform the cylinder. This produces better results than transforming the cylinder
        // in certain cases.
        wrapRadius = finalRadius;
    }
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
                },
                //instead of applying scale transform we will use allowanceRadius for scaling inside wrap
                "allowanceRadius" : (anchorFromEdge) ? midSurfaceRadius : undefined
            } as WrapSurface;

    const wrapId = id + "wrap";
    const wrappedQ = qCreatedBy(wrapId, EntityType.BODY);
    try
    {
        if (!anchorFromEdge && isAtVersionOrLater(context, FeatureScriptVersionNumber.V2903_SM_BEND_FIX))
        {
            const movingPoint = evVertexPoint(context, { "vertex" : qEdgeVertex(imprints.movingBoundary, true) });
            const inBendPlane = movingPoint - anchorPoint;
            const transverseDir = inBendPlane - (dot(inBendPlane, lineDirection) * lineDirection);

            opTransform(context, id + "transform", {
                        "bodies" : pieces.flatBendSurface,
                        "transform" : scaleNonuniformly(finalRadius / midSurfaceRadius, 1.0, 1.0, coordSystem(anchorPoint, transverseDir, planeNormal))
                    });
        }

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

        if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V2903_SM_BEND_FIX))
        {
            // Scale around the anchorPoint with non-uniform scale, scaling the radius from wrapRadius to finalRadius
            opTransform(context, id + "transform", {
                        "bodies" : wrappedQ,
                        "transform" : scaleNonuniformly(finalRadius / wrapRadius, finalRadius / wrapRadius, 1.0,
                        coordSystem(anchorPoint, planeNormal, lineDirection))
                    });
        }
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_ROLL_FAILED, qUnion([imprints.fixedBoundary, imprints.movingBoundary]));
    }

    return wrappedQ;
}

function transformMovingSurface(context is Context, id is Id, surfacePieces is SurfacePieces, imprints is ImprintResult, modelPlane is Plane, bendSurfaceQ is Query, angle) returns boolean
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
    const movingSurfaceQ = surfacePieces.movingSurface;
    const movingEdgeQ = qOwnedByBody(isAtVersionOrLater(context, FeatureScriptVersionNumber.V3082_MOVING_EDGE_OF_MOVING_SURFACE) ? surfacePieces.movingEdges : imprints.movingBoundary, movingSurfaceQ);

    // These should be parallel lines. We want a vector from the moving edge to the fixed edge
    const fixedLine = evLine(context, { "edge" : fixedEdgeQ });
    const movingLine = evLine(context, { "edge" : movingEdgeQ });
    const betweenOrigins = (fixedLine.origin - movingLine.origin);
    const translationVector = betweenOrigins - (dot(betweenOrigins, fixedLine.direction) * fixedLine.direction);
    const translation = transform(translationVector);

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

    // If the rotation direction doesn't cause the flatBend(-translationVector) vector to rotate towards modelPlane.normal, exclusive or,
    // if the rotation angle is negative, let jog know that the offset direction to move the bend reference needs to be flipped
    return ((dot(cross(-translationVector, modelPlane.normal), rotationDirection) > 0) == (angle > TOLERANCE.zeroAngle * radian));
}

function composeModelSurfaces(context is Context, id is Id, sheets, createJog is boolean)
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
                    "eraseImprintedEdges" : !createJog,
                    "operationType" : BooleanOperationType.UNION
                });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_BEND_COLLISION, qUnion(sheets));
    }
}

function annotateBendSurface(context is Context, id is Id, bendSheetQ is Query, radius, angle, kFactor is number, createJog is boolean)
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
            "canBeEdited" : !createJog,
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

/** @internal */
export function transformBendReference(context is Context, id is Id, modParams is map, definition is map, smReturn1 is map)
{
    var offsetDirection = smReturn1.fixedPlane.normal;
    if (!smReturn1.bendIsTowardsModelPlaneNormal)
    {
        offsetDirection *= -1;
    }

    const t = modParams.frontThickness + modParams.backThickness;
    const R = definition.useDefaultRadius ? modParams.defaultBendRadius : definition.bendRadius;
    const lengthOfWallBetweenBends = computeLengthOfWallBetweenBends(context, definition, modParams, R, smReturn1.fixedPlane, offsetDirection, smReturn1.angle);
    const offset = (R + 0.5 * t) + (lengthOfWallBetweenBends / sin(smReturn1.angle));

    const adjustForThickness = 0.5 * (modParams.frontThickness - modParams.backThickness) * dot(offsetDirection, smReturn1.fixedPlane.normal);
    const offsetVec = (offset + adjustForThickness) * offsetDirection;

    const fixedEdges = evaluateQuery(context, smReturn1.fixedEdge);
    const nFixedEdges = size(fixedEdges);
    if (nFixedEdges == 0)
    {
        // Shouldn't reach here
        throw "fixedEdge is empty";
    }
    for (var iFixedEdge = 0; iFixedEdge < nFixedEdges; iFixedEdge += 1)
    {
        const lines = evEdgeTangentLines(context, {
                    "edge" : fixedEdges[iFixedEdge],
                    "parameters" : [0., 1.],
                    "arcLengthParameterization" : false
                });
        var points = [lines[0].origin, lines[1].origin];
        if (dot(lines[0].direction, evLine(context, { "edge" : definition.bendReference }).direction) < 0)
        {
            points = [lines[1].origin, lines[0].origin];
        }

        opFitSpline(context, id + "fitWire" + iFixedEdge, { "points" : [points[0] + offsetVec, points[1] + offsetVec] });
    }
}

function computeJogOffset(context is Context, modParams is map, definition is map, facePlane is Plane, offsetDirection)
{
    var offset;
    const thickness = modParams.backThickness + modParams.frontThickness;

    if (definition.offsetType == JogOffsetBoundingType.BLIND ||
        definition.offsetType == JogOffsetBoundingType.THICKNESS)
    {
        offset = (definition.offsetType == JogOffsetBoundingType.BLIND) ? definition.bendOffset : thickness * definition.thicknessFactor;
        if (definition.bendOffsetAnchor == JogOffsetAnchor.INSIDE)
        {
            offset += thickness;
        }
        else if (definition.bendOffsetAnchor == JogOffsetAnchor.OUTSIDE)
        {
            offset -= thickness;
        }
    }
    else // JogOffsetBoundingType.UP_TO_ENTITY. The inner, outer or nominal face needs to end up on the chosen entity.
    {
        offset = getOffsetToEntity(context, modParams, facePlane, definition, offsetDirection);
        if (definition.jogLimitOffset)
        {
            offset += (definition.jogLimitOppositeDirection ? -1 : 1) * definition.jogLimitDistance;
        }
        if (definition.bendOffsetAnchor == JogOffsetAnchor.INSIDE)
        {
            offset += 0.5 * thickness;
        }
        else if (definition.bendOffsetAnchor == JogOffsetAnchor.OUTSIDE)
        {
            offset -= 0.5 * thickness;
        }
    }

    return offset;
}

function getOffsetToEntity(context is Context, modParams is map, facePlane is Plane, definition is map, offsetDirection)
{
    // If moving up to a construction plane, treat it as an infinite plane rather than a ~6 in square.
    const limitPlaneEntity = evaluateQuery(context, qGeometry(qConstructionFilter(definition.jogLimit, ConstructionObject.YES), GeometryType.PLANE));

    const distanceResult = try silent(evDistance(context, {
                    "side0" : facePlane,
                    "side1" : definition.jogLimit,
                    "extendSide0" : true,
                    "extendSide1" : size(limitPlaneEntity) == 1,
                    "arcLengthParameterization" : false
                }));

    if (distanceResult == undefined)
    {
        throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["jogLimit"]);
    }

    const facePoint = distanceResult.sides[0].point;
    const limitPoint = distanceResult.sides[1].point;

    if (offsetDirection != undefined && dot(offsetDirection, limitPoint - facePoint) < -TOLERANCE.zeroLength * meter)
    {
        if (parallelVectors(offsetDirection, limitPoint - facePoint))
        {
            // If we detect that jog direction and target entity are in conflict,
            // show a message explaining it and some error graphics
            throw regenError(ErrorStringEnum.SHEET_METAL_JOG_OPPOSITE_DIRECTION, definition.jogLimit);
        }
        else
        {
            // If we calculated everything correctly we shouldn't come here
            throw "Wrong direction";
        }
    }

    const adjustForThickness = 0.5 * (modParams.frontThickness - modParams.backThickness) * facePlane.normal;
    const nominalPoint = facePoint + adjustForThickness;
    const offsetDistance = abs(dot(facePlane.normal, limitPoint - nominalPoint));

    return offsetDistance;
}

/**
 * Calculate the resultant nominal jog offset for the given bend angle if the bends of the jog are adjacent to each other, i.e. if there is no wall between the bends.
 */
function computeNominalOffsetForAngleAdjacentBends(bendAngle is ValueWithUnits, bendRadius is ValueWithUnits, modelThickness is ValueWithUnits) returns ValueWithUnits
{
    const cA = cos(bendAngle);
    const topToInflection = bendRadius * (1 - cA);
    const inflectionToTop = (bendRadius + modelThickness) * (1 - cA);

    return (topToInflection + inflectionToTop);
}

function computeLengthOfWallBetweenBends(context is Context, definition is map, modParams is map, bendRadius is ValueWithUnits, facePlane is Plane, offsetDirection, angle is ValueWithUnits) returns ValueWithUnits
{
    if (definition.bendAngleSetProgrammatically)
    {
        return 0.0 * meter;
    }
    const thickness = modParams.backThickness + modParams.frontThickness;
    const specifiedNominalOffset = computeJogOffset(context, modParams, definition, facePlane, offsetDirection);
    const nominalOffsetForAngleAdjacentBends = computeNominalOffsetForAngleAdjacentBends(angle, bendRadius, thickness);

    return (specifiedNominalOffset - nominalOffsetForAngleAdjacentBends) / sin(angle);
}

