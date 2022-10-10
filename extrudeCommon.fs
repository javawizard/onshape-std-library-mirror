FeatureScript 1867; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

export import(path : "onshape/std/boundingtype.gen.fs", version : "1867.0");

import(path : "onshape/std/curveGeometry.fs", version : "1867.0");
import(path : "onshape/std/evaluate.fs", version : "1867.0");
import(path : "onshape/std/feature.fs", version : "1867.0");
import(path : "onshape/std/manipulator.fs", version : "1867.0");
import(path : "onshape/std/query.fs", version : "1867.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1867.0");
import(path : "onshape/std/valueBounds.fs", version : "1867.0");
import(path : "onshape/std/vector.fs", version : "1867.0");
import(path : "onshape/std/coordSystem.fs", version : "1867.0");
import(path : "onshape/std/containers.fs", version : "1867.0");

/**
 * Bounding type used with SMProcessType.EXTRUDE
 */
export enum SMExtrudeBoundingType
{
    annotation { "Name" : "Blind" }
    BLIND,
    annotation {"Name" : "Up to next"}
    UP_TO_NEXT,
    annotation {"Name" : "Up to face"}
    UP_TO_SURFACE,
    annotation {"Name" : "Up to part"}
    UP_TO_BODY,
    annotation {"Name" : "Up to vertex"}
    UP_TO_VERTEX
}

/**
 * @internal
 * Predicate which specifies fields depending on the [BoundingType] for an extrude-type feature.
 * The definition in question should specify an `endBound` of type [BoundingType] or [SMExtrudeBoundingType].
 *
 * When used in a precondition, this predicate creates the UI under the [BoundingType] dropdown, such as `Depth` for
 * `Blind` extrudes or `Up to surface or part` QLV for `Up to part` extrudes.
 */
export predicate extrudeBoundParametersPredicate(definition is map)
{
    if (definition.endBound == BoundingType.BLIND ||
        definition.endBound == SMExtrudeBoundingType.BLIND)
    {
        annotation { "Name" : "Depth" }
        isLength(definition.depth, LENGTH_BOUNDS);
    }
    else if (definition.endBound == BoundingType.UP_TO_SURFACE ||
             definition.endBound == SMExtrudeBoundingType.UP_TO_SURFACE)
    {
        annotation { "Name" : "Up to face",
            "Filter" : (EntityType.FACE && SketchObject.NO && AllowMeshGeometry.YES) || BodyType.MATE_CONNECTOR,
            "MaxNumberOfPicks" : 1 }
        definition.endBoundEntityFace is Query;
    }
    else if (definition.endBound == BoundingType.UP_TO_BODY ||
             definition.endBound == SMExtrudeBoundingType.UP_TO_BODY)
    {
        annotation { "Name" : "Up to surface or part",
                     "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO && AllowMeshGeometry.YES,
                     "MaxNumberOfPicks" : 1 }
        definition.endBoundEntityBody is Query;
    }
    else if (definition.endBound == BoundingType.UP_TO_VERTEX ||
             definition.endBound == SMExtrudeBoundingType.UP_TO_VERTEX)
    {
        annotation {"Name" : "Up to vertex or mate connector",
            "Filter" : QueryFilterCompound.ALLOWS_VERTEX,
            "MaxNumberOfPicks" : 1 }
        definition.endBoundEntityVertex is Query;
    }

    if (definition.endBound == BoundingType.UP_TO_NEXT ||
        definition.endBound == BoundingType.UP_TO_SURFACE ||
        definition.endBound == BoundingType.UP_TO_BODY ||
        definition.endBound == BoundingType.UP_TO_VERTEX ||
        definition.endBound == SMExtrudeBoundingType.UP_TO_NEXT ||
        definition.endBound == SMExtrudeBoundingType.UP_TO_SURFACE ||
        definition.endBound == SMExtrudeBoundingType.UP_TO_BODY ||
        definition.endBound == SMExtrudeBoundingType.UP_TO_VERTEX)
    {
        annotation {"Name" : "Offset distance", "Column Name" : "Has offset", "UIHint" : [ "DISPLAY_SHORT", "FIRST_IN_ROW" ] }
        definition.hasOffset is boolean;

        if (definition.hasOffset)
        {
            annotation {"Name" : "Offset distance", "UIHint" : UIHint.DISPLAY_SHORT }
            isLength(definition.offsetDistance, LENGTH_BOUNDS);

            annotation {"Name" : "Opposite direction", "Column Name" : "Offset opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION}
            definition.offsetOppositeDirection is boolean;
        }
    }
}

/**
 * @internal
 * Predicate which specifies fields depending on the [BoundingType] for an extrude-type feature.
 * The definition in question should specify a `secondDirectionBound` of type [BoundingType] or [SMExtrudeBoundingType].
 *
 * When used in a precondition, this predicate creates the UI under the [BoundingType] dropdown, such as `Depth` for
 * `Blind` extrudes or `Up to surface or part` QLV for `Up to part` extrudes.
 */
export predicate extrudeSecondDirectionBoundParametersPredicate(definition is map)
{
    if (definition.secondDirectionBound == BoundingType.BLIND ||
        definition.secondDirectionBound == SMExtrudeBoundingType.BLIND)
    {
        annotation { "Name" : "Depth", "Column Name" : "Second depth" }
        isLength(definition.secondDirectionDepth, LENGTH_BOUNDS);
    }
    else if (definition.secondDirectionBound == BoundingType.UP_TO_SURFACE ||
             definition.secondDirectionBound == SMExtrudeBoundingType.UP_TO_SURFACE)
    {
        annotation { "Name" : "Up to face", "Column Name" : "Second up to face",
            "Filter" : (EntityType.FACE && SketchObject.NO && AllowMeshGeometry.YES) || BodyType.MATE_CONNECTOR,
            "MaxNumberOfPicks" : 1 }
        definition.secondDirectionBoundEntityFace is Query;
    }
    else if (definition.secondDirectionBound == BoundingType.UP_TO_BODY ||
             definition.secondDirectionBound == SMExtrudeBoundingType.UP_TO_BODY)
    {
        annotation { "Name" : "Up to surface or part", "Column Name" : "Second up to surface or part",
                     "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO && AllowMeshGeometry.YES,
                     "MaxNumberOfPicks" : 1 }
        definition.secondDirectionBoundEntityBody is Query;
    }

    else if (definition.secondDirectionBound == BoundingType.UP_TO_VERTEX ||
             definition.secondDirectionBound == SMExtrudeBoundingType.UP_TO_VERTEX)
    {
        annotation { "Name" : "Up to vertex or mate connector", "Column Name" : "Second up to vertex or mate connector",
            "Filter" : QueryFilterCompound.ALLOWS_VERTEX,
            "MaxNumberOfPicks" : 1 }
        definition.secondDirectionBoundEntityVertex is Query;
    }

    if (definition.secondDirectionBound == BoundingType.UP_TO_NEXT ||
        definition.secondDirectionBound == BoundingType.UP_TO_SURFACE ||
        definition.secondDirectionBound == BoundingType.UP_TO_BODY ||
        definition.secondDirectionBound == BoundingType.UP_TO_VERTEX ||
        definition.secondDirectionBound == SMExtrudeBoundingType.UP_TO_NEXT ||
        definition.secondDirectionBound == SMExtrudeBoundingType.UP_TO_SURFACE ||
        definition.secondDirectionBound == SMExtrudeBoundingType.UP_TO_BODY ||
        definition.secondDirectionBound == SMExtrudeBoundingType.UP_TO_VERTEX)
    {
        annotation {"Name" : "Offset distance", "Column Name" : "Second direction has offset", "UIHint" : [ "DISPLAY_SHORT", "FIRST_IN_ROW" ] }
        definition.hasSecondDirectionOffset is boolean;

        if (definition.hasSecondDirectionOffset)
        {
            annotation {"Name" : "Offset distance", "Column Name" : "Second offset distance", "UIHint" : UIHint.DISPLAY_SHORT }
            isLength(definition.secondDirectionOffsetDistance, LENGTH_BOUNDS);

            annotation {"Name" : "Opposite direction", "Column Name" : "Second offset opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION}
            definition.secondDirectionOffsetOppositeDirection is boolean;
        }
    }
}

// ---------- SYMMETRIC ----------
/** @internal */
export predicate isSymmetricExtrude(definition is map)
{
    definition.symmetric;
    definition.endBound == BoundingType.BLIND || definition.endBound == BoundingType.THROUGH_ALL || definition.endBound == SMExtrudeBoundingType.BLIND;
}

// ---------- OPPOSITE_DIRECTION ----------

function oppositeDirectionField(definition is map) returns string
{
    // For sheet metal we use "oppositeExtrudeDirection".  For normal extrude we use "oppositeDirection"
    return (definition.oppositeExtrudeDirection != undefined) ? "oppositeExtrudeDirection" : "oppositeDirection";
}

function flipOppositeDirection(definition is map) returns map
{
    definition[oppositeDirectionField(definition)] =
            !definition[oppositeDirectionField(definition)];
    return definition;
}

predicate isOppositeDirection(definition is map)
{
    definition[oppositeDirectionField(definition)] == true;
}

function secondDirectionOppositeDirectionField(definition is map) returns string
{
    // For sheet metal we use "secondDirectionOppositeExtrudeDirection".  For normal extrude we use "secondDirectionOppositeDirection"
    return (definition.secondDirectionOppositeExtrudeDirection != undefined) ?
            "secondDirectionOppositeExtrudeDirection" : "secondDirectionOppositeDirection";
}

function flipSecondDirectionOppositeDirection(definition is map) returns map
{
    definition[secondDirectionOppositeDirectionField(definition)] =
            !definition[secondDirectionOppositeDirectionField(definition)];
    return definition;
}

predicate isSecondDirectionOppositeDirection(definition is map)
{
    definition[secondDirectionOppositeDirectionField(definition)] == true;
}

// ---------- BOUNDING TYPES ----------

predicate isFirstDirectionOfType(definition is map, boundingType is BoundingType)
{
    definition.endBound as BoundingType == boundingType;
}

predicate isSecondDirectionOfType(definition is map, boundingType is BoundingType)
{
    definition.secondDirectionBound as BoundingType == boundingType;
}

// ---------- Definition adjustments ----------

/**
 * @internal
 * Ensure that `depth` fields for both directions of `BLIND` extrudes are positive, and set the `oppositeDirection`
 * field appropriately to compensate.
 */
export function adjustExtrudeDirectionForBlind(definition is map) returns map
{
    if (isFirstDirectionOfType(definition, BoundingType.BLIND) && definition.depth < 0)
    {
        definition.depth *= -1;
        definition = flipOppositeDirection(definition);
    }

    if (definition.hasSecondDirection && isSecondDirectionOfType(definition, BoundingType.BLIND) && definition.secondDirectionDepth < 0)
    {
        definition.secondDirectionDepth *= -1;
        definition = flipSecondDirectionOppositeDirection(definition);
    }
    return definition;
}

/**
 * @internal
 * Remap the inputs of an extrude-type feature into appropriate input for `opExtrude`.
 */
export function transformExtrudeDefinitionForOpExtrude(context is Context, id is Id, entities is Query,
        direction is Vector, definition is map) returns map
{
    // Set the entities
    definition.entities = entities;

    // Set the direction
    definition.direction = direction;

    if (isOppositeDirection(definition))
        definition.direction *= -1;

    definition.isStartBoundOpposite = false;

    // Determine the bounds

    // All temporary planes are given the same id for history tracking. The UP_TO_VERTEX case used to be the only one
    // that created a temporary plane, so that's where the id comes from. Now the mate connector case of UP_TO_SURFACE
    // also creates a temporary plane.
    const tempPlaneId = id + "vertexPlane";

    if (isFirstDirectionOfType(definition, BoundingType.UP_TO_SURFACE))
    {
        verifyNonemptyQuery(context, definition, "endBoundEntityFace", ErrorStringEnum.EXTRUDE_SELECT_TERMINATING_SURFACE);
        const mateConnectorCSys = try silent(evMateConnector(context, { "mateConnector" : definition.endBoundEntityFace }));
        if (mateConnectorCSys != undefined)
        {
            definition.mateConnectorPlaneId = tempPlaneId;
            definition.endBoundEntity = createMateConnectorBoundaryPlane(context, definition.mateConnectorPlaneId, mateConnectorCSys);
        }
        else
        {
            definition.endBoundEntity = definition.endBoundEntityFace;
        }
    }
    else if (isFirstDirectionOfType(definition, BoundingType.UP_TO_BODY))
    {
        verifyNonemptyQuery(context, definition, "endBoundEntityBody", ErrorStringEnum.EXTRUDE_SELECT_TERMINATING_BODY);
        definition.endBoundEntity = definition.endBoundEntityBody;
    }
    else if (isFirstDirectionOfType(definition, BoundingType.UP_TO_VERTEX))
    {
        verifyNonemptyQuery(context, definition, "endBoundEntityVertex", ErrorStringEnum.EXTRUDE_SELECT_TERMINATING_VERTEX);
        definition.vertexPlaneId = tempPlaneId;
        definition.endBoundEntity = createVertexBoundaryPlane(context, definition.vertexPlaneId,
                definition.endBoundEntityVertex, definition.direction);
        definition.endBound = BoundingType.UP_TO_SURFACE;
    }

    definition.endTranslationalOffset = 0;

    if (definition.hasOffset &&
       (isFirstDirectionOfType(definition, BoundingType.UP_TO_NEXT) ||
        isFirstDirectionOfType(definition, BoundingType.UP_TO_SURFACE) ||
        isFirstDirectionOfType(definition, BoundingType.UP_TO_BODY)))
    {
        // @opExtrude takes endTranslationalOffset in terms of extrude direction, but positive, non-flipped offset
        // in extrude feature should pull away from the body.
        if (!definition.offsetOppositeDirection)
        {
            definition.offsetDistance *= -1;
        }
        definition.endTranslationalOffset = definition.offsetDistance;
    }

    definition.startBound = BoundingType.BLIND;
    definition.startDepth = 0;
    definition.startTranslationalOffset = 0;
    definition.endDepth = definition.depth;

    if (isSymmetricExtrude(definition))
    {
        if (isFirstDirectionOfType(definition, BoundingType.BLIND))
        {
            definition.startBound = BoundingType.BLIND;
            definition.endDepth = definition.depth * 0.5;
            definition.startDepth = -definition.depth * 0.5;
        }
        else if (isFirstDirectionOfType(definition, BoundingType.THROUGH_ALL))
        {

            definition.isStartBoundOpposite = true;
            definition.startBound = BoundingType.THROUGH_ALL;
        }
        else
        {
            throw "Unexpected bounding type for symmetric extrude: " ~ definition.endBound;
        }
    }
    else if (definition.hasSecondDirection)
    {
        // Check the second direction

        // See tempPlaneId above for more info.
        const secondTempPlaneId = id + "secondVertexPlane";

        if (isSecondDirectionOfType(definition, BoundingType.UP_TO_SURFACE))
        {
            verifyNonemptyQuery(context, definition, "secondDirectionBoundEntityFace", ErrorStringEnum.EXTRUDE_SELECT_TERMINATING_SURFACE);
            const mateConnectorCSys = try silent(evMateConnector(context, { "mateConnector" : definition.secondDirectionBoundEntityFace }));
            if (mateConnectorCSys != undefined)
            {
                definition.secondMateConnectorPlaneId = secondTempPlaneId;
                definition.secondDirectionBoundEntity = createMateConnectorBoundaryPlane(context,
                        definition.secondMateConnectorPlaneId, mateConnectorCSys);
            }
            else
            {
                definition.secondDirectionBoundEntity = definition.secondDirectionBoundEntityFace;
            }
        }
        else if (isSecondDirectionOfType(definition, BoundingType.UP_TO_BODY))
        {
            verifyNonemptyQuery(context, definition, "secondDirectionBoundEntityBody", ErrorStringEnum.EXTRUDE_SELECT_TERMINATING_BODY);
            definition.secondDirectionBoundEntity = definition.secondDirectionBoundEntityBody;
        }
        else if (isSecondDirectionOfType(definition, BoundingType.UP_TO_VERTEX))
        {
            verifyNonemptyQuery(context, definition, "secondDirectionBoundEntityVertex", ErrorStringEnum.EXTRUDE_SELECT_TERMINATING_VERTEX);
            definition.secondVertexPlaneId = secondTempPlaneId;
            definition.secondDirectionBoundEntity = createVertexBoundaryPlane(context, definition.secondVertexPlaneId,
                    definition.secondDirectionBoundEntityVertex, definition.direction);
            definition.secondDirectionBound = BoundingType.UP_TO_SURFACE;
        }

        definition.startBound = definition.secondDirectionBound as BoundingType;
        definition.startBoundEntity = definition.secondDirectionBoundEntity;

        definition.startDepth = definition.secondDirectionDepth;
        if (isSecondDirectionOppositeDirection(definition) != isOppositeDirection(definition))
            definition.isStartBoundOpposite = true;

        if (definition.hasSecondDirectionOffset &&
           (isSecondDirectionOfType(definition, BoundingType.UP_TO_NEXT) ||
            isSecondDirectionOfType(definition, BoundingType.UP_TO_SURFACE) ||
            isSecondDirectionOfType(definition, BoundingType.UP_TO_BODY)))
        {
            if ((definition.secondDirectionOffsetOppositeDirection && !definition.isStartBoundOpposite) ||
                (!definition.secondDirectionOffsetOppositeDirection && definition.isStartBoundOpposite))
            {
                definition.secondDirectionOffsetDistance *= -1;
            }
            definition.startTranslationalOffset = definition.secondDirectionOffsetDistance;
        }
    }

    return definition;
}

function createVertexBoundaryPlane(context is Context, id is Id, vertex is Query, direction is Vector) returns Query
{
    const vertexPoint = evVertexPoint(context, { "vertex" : vertex });
    opPlane(context, id, { "plane" : plane(vertexPoint, direction) });
    return qCreatedBy(id, EntityType.FACE);
}

function createMateConnectorBoundaryPlane(context is Context, id is Id, mateConnectorCSys is CoordSystem) returns Query
{
    opPlane(context, id, { "plane" : plane(mateConnectorCSys) });
    return qCreatedBy(id, EntityType.FACE);
}

/**
 * @internal
 * Clean up boundary planes created by `transformExtrudeDefinitionForOpExtrude` for `UP_TO_VERTEX` and
 * mate connector `UP_TO_SURFACE` extrudes.  Should be called after the [opExtrude] is executed.
 */
export function cleanupTemporaryBoundaryPlanes(context is Context, id is Id, definition is map)
{
    var tempBodies = [];
    const batchDelete = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1031_BODY_NET_IN_LOFT);

    if (definition.vertexPlaneId != undefined)
    {
        const tempBody = qCreatedBy(definition.vertexPlaneId, EntityType.BODY);
        if (batchDelete)
        {
            tempBodies = append(tempBodies, tempBody);
        }
        else
        {
            opDeleteBodies(context, id + "deleteVertexPlane", { "entities" : tempBody });
        }
    }

    if (definition.secondVertexPlaneId != undefined)
    {
        const tempBody = qCreatedBy(definition.secondVertexPlaneId, EntityType.BODY);
        if (batchDelete)
        {
            tempBodies = append(tempBodies, tempBody);
        }
        else
        {
            opDeleteBodies(context, id + "deleteSecondVertexPlane", { "entities" : tempBody });
        }
    }

    if (definition.mateConnectorPlaneId != undefined)
    {
        tempBodies = append(tempBodies, qCreatedBy(definition.mateConnectorPlaneId, EntityType.BODY));
    }

    if (definition.secondMateConnectorPlaneId != undefined)
    {
        tempBodies = append(tempBodies, qCreatedBy(definition.secondMateConnectorPlaneId, EntityType.BODY));
    }

    if (tempBodies != [])
    {
        opDeleteBodies(context, id + "deleteTempExtrudePlanes", { "entities" : qUnion(tempBodies) });
    }
}

// ---------- Manipulator functions ----------

const DEPTH_MANIPULATOR = "depthManipulator";
const FLIP_MANIPULATOR = "flipManipulator";
const SECOND_DEPTH_MANIPULATOR = "secondDirectionDepthManipulator";
const SECOND_FLIP_MANIPULATOR = "secondDirectionFlipManipulator";

/**
 * @internal
 * Add the appropriate depth and flip manipulators for [extrude] feature or [sheetMetalStart] feature using `Extrude` option.
 */
export function addExtrudeManipulator(context is Context, id is Id, definition is map, entities is Query, extrudeAxis is Line, showEntities is boolean)
{
    if (!isQueryEmpty(context, qSheetMetalFlatFilter(entities, SMFlatType.YES)))
    {
        return;
    }
    if (isFirstDirectionOfType(definition, BoundingType.THROUGH_ALL) && definition.symmetric)
    {
        return;
    }
    if (!isFirstDirectionOfType(definition, BoundingType.BLIND))
    {
        addManipulators(context, id, {
                    (FLIP_MANIPULATOR) : flipManipulator({
                                "base" : extrudeAxis.origin,
                                "direction" : extrudeAxis.direction,
                                "flipped" : isOppositeDirection(definition)
                            })
                });
    }
    else
    {
        var depthOffset = definition.depth;
        if (isSymmetricExtrude(definition))
            depthOffset *= 0.5;
        // BLIND relies on oppositeDirection to determine the flip of the manipulator
        if (isOppositeDirection(definition))
            depthOffset *= -1;
        addManipulators(context, id, {
                    (DEPTH_MANIPULATOR) : linearManipulator({
                                "base" : extrudeAxis.origin,
                                "direction" : extrudeAxis.direction,
                                "offset" : depthOffset,
                                "sources" : showEntities ? entities : undefined,
                                "primaryParameterId" : "depth"
                            })
                });
    }

    if (definition.hasSecondDirection && !isSymmetricExtrude(definition))
    {
        if (!isSecondDirectionOfType(definition, BoundingType.BLIND))
        {
            addManipulators(context, id, {
                        (SECOND_FLIP_MANIPULATOR) : flipManipulator({
                                    "base" : extrudeAxis.origin,
                                    "direction" : extrudeAxis.direction,
                                    "flipped" : isSecondDirectionOppositeDirection(definition),
                                    "style" : ManipulatorStyleEnum.SECONDARY
                                })
                    });
        }
        else
        {
            var secondDirectionDepthOffset = definition.secondDirectionDepth;
            if (isSecondDirectionOppositeDirection(definition))
                secondDirectionDepthOffset *= -1;
            addManipulators(context, id, {
                        (SECOND_DEPTH_MANIPULATOR) : linearManipulator({
                                    "base" : extrudeAxis.origin,
                                    "direction" : extrudeAxis.direction,
                                    "offset" : secondDirectionDepthOffset,
                                    "sources" : showEntities ? entities : undefined,
                                    "style" : ManipulatorStyleEnum.SECONDARY,
                                    "primaryParameterId" : "secondDirectionDepth"
                                })
                    });
        }
    }
}

/**
 * @internal
 * The manipulator change function used for extrude manipulators in the `extrude` and `sheetMetalStart` features.
 */
export function extrudeManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    if (newManipulators[DEPTH_MANIPULATOR] is map &&
        (isFirstDirectionOfType(definition, BoundingType.BLIND)))
    {
        var newDepthOffset = newManipulators[DEPTH_MANIPULATOR].offset;
        if (isSymmetricExtrude(definition))
            newDepthOffset *= 2;
        definition[oppositeDirectionField(definition)] = newDepthOffset < 0;
        definition.depth = abs(newDepthOffset);
    }
    else if (newManipulators[FLIP_MANIPULATOR] is map &&
             !isFirstDirectionOfType(definition, BoundingType.BLIND))
    {
        definition[oppositeDirectionField(definition)] = newManipulators[FLIP_MANIPULATOR].flipped;
    }

    if (definition.hasSecondDirection && !isSymmetricExtrude(definition))
    {
        if (newManipulators[SECOND_DEPTH_MANIPULATOR] is map &&
            isSecondDirectionOfType(definition, BoundingType.BLIND))
        {
            const newSecondDirectionDepthOffset = newManipulators[SECOND_DEPTH_MANIPULATOR].offset;
            definition[secondDirectionOppositeDirectionField(definition)] = newSecondDirectionDepthOffset < 0;
            definition.secondDirectionDepth = abs(newSecondDirectionDepthOffset);
        }
        else if (newManipulators[SECOND_FLIP_MANIPULATOR] is map &&
            !isSecondDirectionOfType(definition, BoundingType.BLIND))
        {
            definition[secondDirectionOppositeDirectionField(definition)] = newManipulators[SECOND_FLIP_MANIPULATOR].flipped;
        }
    }

    return definition;
}

// ---------- Editing logic functions ----------

/**
 * @internal
 * Do not attempt to set any primary direction flips in [extrude] if there is a second direction, if the [extrude] is
 * `symmetric`, or if the user has already toggled the primary direction flipper.
 */
export function canSetExtrudeFlips(definition is map, specifiedParameters is map) returns boolean
{
    return !definition.hasSecondDirection &&
            !isSymmetricExtrude(definition) &&
            !specifiedParameters[oppositeDirectionField(definition)];
}

/**
 * @internal
 * Attempt to reorient the primary direction of the extrude towards the `UP_TO` boundary if the user has explicitly
 * the bounding entity.
 */
export function canSetExtrudeUpToFlip(definition is map, specifiedParameters is map) returns boolean
{
    if (isFirstDirectionOfType(definition, BoundingType.UP_TO_SURFACE))
    {
        return specifiedParameters.endBoundEntityFace;
    }
    if (isFirstDirectionOfType(definition, BoundingType.UP_TO_BODY))
    {
        return specifiedParameters.endBoundEntityBody;
    }
    if (isFirstDirectionOfType(definition, BoundingType.UP_TO_VERTEX))
    {
        return specifiedParameters.endBoundEntityVertex;
    }
    return false;
}

// Test whether the extrude direction should be flipped based on the `UP_TO` parameters.
function shouldFlipExtrudeDirection(context is Context, endBound is BoundingType, endBoundEntity is Query, extrudeAxis is Line)
{
    if (endBound != BoundingType.UP_TO_SURFACE &&
        endBound != BoundingType.UP_TO_BODY &&
        endBound != BoundingType.UP_TO_VERTEX)
        return false;

    if (endBound == BoundingType.UP_TO_SURFACE)
    {
        const refPlane = try(evPlane(context, { "face" : endBoundEntity }));
        if (refPlane == undefined)
            return false; //err on side of not flipping, TODO: surfaceXline
        const isecResult = intersection(refPlane, extrudeAxis);
        if (isecResult == undefined || isecResult.dim != 0)
            return false;

        const dotProduct = stripUnits(dot(isecResult.intersection - extrudeAxis.origin, extrudeAxis.direction));
        return dotProduct < -TOLERANCE.zeroLength;
    }
    if (endBound == BoundingType.UP_TO_VERTEX)
    {
        const targetVertex = try(evVertexPoint(context, { "vertex" : endBoundEntity}));
        if (targetVertex == undefined)
            return false;
        const dotProduct = stripUnits(dot(targetVertex - extrudeAxis.origin, extrudeAxis.direction));
        return dotProduct < -TOLERANCE.zeroLength;
    }
    const pln = plane(extrudeAxis.origin, extrudeAxis.direction);
    const boxResult = try(evBox3d(context, { 'topology' : endBoundEntity, 'cSys' : planeToCSys(pln), 'tight' : false }));
    if (boxResult == undefined)
        return false;

    return (stripUnits(boxResult.minCorner[2]) < -TOLERANCE.zeroLength &&
            stripUnits(boxResult.maxCorner[2]) < TOLERANCE.zeroLength);
}

/**
 * @internal
 * Flip the extrude direction such that the primary direction is facing the `UP_TO` bounding entity.
 */
export function extrudeUpToBoundaryFlipCommon(context is Context, extrudeAxis is Line, definition is map) returns map
{
    if (isOppositeDirection(definition))
        extrudeAxis.direction *= -1;
    var endBoundEntity;
    if (isFirstDirectionOfType(definition, BoundingType.UP_TO_SURFACE))
    {
        endBoundEntity = definition.endBoundEntityFace;
    }
    else if (isFirstDirectionOfType(definition, BoundingType.UP_TO_BODY))
    {
        endBoundEntity = definition.endBoundEntityBody;
    }
    else if (isFirstDirectionOfType(definition, BoundingType.UP_TO_VERTEX))
    {
        endBoundEntity = definition.endBoundEntityVertex;
    }
    if (endBoundEntity is Query && shouldFlipExtrudeDirection(context, definition.endBound as BoundingType, endBoundEntity, extrudeAxis))
    {
        definition = flipOppositeDirection(definition);
    }

    return definition;
}

/**
 * @internal
 * Ensure that the second direction of the extrude is facing away from the first direction if the user has not already
 * explicitly toggled the second direction flipper.
 */
export function setExtrudeSecondDirectionFlip(definition is map, specifiedParameters is map) returns map
{
    if (specifiedParameters[secondDirectionOppositeDirectionField(definition)] is undefined ||
        specifiedParameters[secondDirectionOppositeDirectionField(definition)] ||
        !definition.hasSecondDirection)
    {
        return definition;
    }

    definition[secondDirectionOppositeDirectionField(definition)] = !isOppositeDirection(definition);
    return definition;
}

