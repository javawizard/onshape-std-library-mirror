FeatureScript 675; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "675.0");
export import(path : "onshape/std/tool.fs", version : "675.0");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "675.0");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "675.0");
import(path : "onshape/std/box.fs", version : "675.0");
import(path : "onshape/std/containers.fs", version : "675.0");
import(path : "onshape/std/curveGeometry.fs", version : "675.0");
import(path : "onshape/std/evaluate.fs", version : "675.0");
import(path : "onshape/std/feature.fs", version : "675.0");
import(path : "onshape/std/mathUtils.fs", version : "675.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "675.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "675.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "675.0");
import(path : "onshape/std/topologyUtils.fs", version : "675.0");
import(path : "onshape/std/valueBounds.fs", version : "675.0");


/**
 * @internal
 * Output types of move face
 */
export enum MoveFaceOutputType
{
    annotation { "Name" : "Move" }
    MOVE,
    annotation { "Name" : "Create" }
    CREATE
}

/**
 * @internal
 * Bounding types of move face
 */
export enum MoveFaceBoundingType
{
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Up to entity" }
    UP_TO_ENTITY
}

/** @internal */
export const MOVE_FACE_OFFSET_BOUNDS = ZERO_INCLUSIVE_OFFSET_BOUNDS;
/** @internal */
export const MOVE_FACE_TRANSLATE_BOUNDS = ZERO_INCLUSIVE_OFFSET_BOUNDS;
/** @internal */
export const MOVE_FACE_ROTATION_BOUNDS = ANGLE_360_ZERO_DEFAULT_BOUNDS;

/**
 * Feature performing an [opMoveFace].
 */
annotation { "Feature Type Name" : "Move face",
             "Manipulator Change Function" : "moveFaceManipulatorChange",
             "Filter Selector" : "allparts",
             "Editing Logic Function" : "moveFaceEditingLogic"}
export const moveFace = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Creation type", "UIHint" : ["HORIZONTAL_ENUM", "ALWAYS_HIDDEN"] } //Legacy
        definition.outputType is MoveFaceOutputType;
        if (definition.outputType == MoveFaceOutputType.MOVE)
        {
            annotation { "Name" : "Faces to move",
                         "UIHint" : "SHOW_CREATE_SELECTION",
                         "Filter" : (EntityType.FACE && (ActiveSheetMetal.NO || SheetMetalDefinitionEntityType.EDGE || SheetMetalDefinitionEntityType.FACE))
                                    && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES }
            definition.moveFaces is Query;
        }
        else
        {
            annotation { "Name" : "Faces, surfaces, and sketch regions",
                         "UIHint" : "SHOW_CREATE_SELECTION",
                         "Filter" : (EntityType.FACE || (BodyType.SHEET && EntityType.BODY))
                            && ConstructionObject.NO }
            definition.surfacesAndFaces is Query;
        }

        annotation { "Name" : "Move face type" }
        definition.moveFaceType is MoveFaceType;

        if (definition.moveFaceType == MoveFaceType.TRANSLATE)
        {
            annotation { "Name" : "Direction",
                        "Filter" : QueryFilterCompound.ALLOWS_DIRECTION,
                        "MaxNumberOfPicks" : 1 }
            definition.direction is Query;
        }
        else if (definition.moveFaceType == MoveFaceType.ROTATE)
        {
            annotation { "Name" : "Axis",
                        "Filter" : QueryFilterCompound.ALLOWS_AXIS,
                        "MaxNumberOfPicks" : 1 }
            definition.axis is Query;
        }

        if (definition.moveFaceType != MoveFaceType.ROTATE)
        {
            annotation { "Name" : "End type", "UIHint" : "SHOW_LABEL" }
            definition.limitType is MoveFaceBoundingType;
        }

        if (definition.limitType == MoveFaceBoundingType.BLIND || definition.moveFaceType == MoveFaceType.ROTATE)
        {
            if (definition.moveFaceType == MoveFaceType.TRANSLATE)
            {
                annotation { "Name" : "Distance" }
                isLength(definition.translationDistance, MOVE_FACE_TRANSLATE_BOUNDS);
            }
            else if (definition.moveFaceType == MoveFaceType.ROTATE)
            {
                annotation { "Name" : "Rotation angle" }
                isAngle(definition.angle, MOVE_FACE_ROTATION_BOUNDS);
            }
            else
            {
                annotation { "Name" : "Distance" }
                isLength(definition.offsetDistance, MOVE_FACE_OFFSET_BOUNDS);
            }

            annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
            definition.oppositeDirection is boolean;
        }
        else
        {
            if (definition.moveFaceType == MoveFaceType.TRANSLATE)
            {
                annotation { "Name" : "Up to entity", "Filter" : (EntityType.FACE && GeometryType.PLANE) || EntityType.VERTEX, "MaxNumberOfPicks" : 1 }
                definition.limitEntity is Query;
            }
            else
            {
                // Can accept anything that evDistance will take.
                annotation { "Name" : "Up to entity", "Filter" : EntityType.VERTEX || EntityType.EDGE || EntityType.FACE || EntityType.BODY, "MaxNumberOfPicks" : 1 }
                definition.limitQuery is Query;
            }

            if (definition.limitType == MoveFaceBoundingType.UP_TO_ENTITY)
            {
                annotation { "Name" : "Offset distance", "UIHint" : "DISPLAY_SHORT" }
                definition.hasOffset is boolean;

                if (definition.hasOffset)
                {
                    annotation { "Name" : "Offset", "UIHint" : "DISPLAY_SHORT" }
                    isLength(definition.offset, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

                    annotation { "Name" : "Opposite offset direction", "Default" : true, "UIHint" : "OPPOSITE_DIRECTION" }
                    definition.oppositeOffsetDirection is boolean;
                }
            }
        }

        if (definition.outputType == MoveFaceOutputType.MOVE)
        {
            annotation { "Name" : "Reapply fillet", "Default" : true }
            definition.reFillet is boolean;
        }
    }
    //============================ Body =============================
    {
        var qSurfacesAndFaces;
        if (definition.outputType == MoveFaceOutputType.MOVE)
        {
            definition.queryParameter = "moveFaces";
            definition.selectionErrorStringEnum = ErrorStringEnum.DIRECT_EDIT_MOVE_FACE_SELECT;
            qSurfacesAndFaces = definition.moveFaces;
        }
        else
        {
            definition.queryParameter = "surfacesAndFaces";
            definition.selectionErrorStringEnum = ErrorStringEnum.DIRECT_EDIT_MOVE_FACE_CREATE_SELECT;
            qSurfacesAndFaces = qUnion([qEntityFilter(definition.surfacesAndFaces, EntityType.FACE), qOwnedByBody(qEntityFilter(definition.surfacesAndFaces, EntityType.BODY), EntityType.FACE)]);

            return moveFaceCreateLegacy(context, id, definition, qSurfacesAndFaces);
        }

        const resolvedEntities = evaluateQuery(context, qSurfacesAndFaces);
        if (size(resolvedEntities) == 0)
            throw regenError(definition.selectionErrorStringEnum, [definition.queryParameter]);

        var directionSign = 1;
        if (definition.oppositeDirection)
            directionSign = -1;

        if (definition.moveFaceType != MoveFaceType.OFFSET &&
            isAtVersionOrLater(context, FeatureScriptVersionNumber.V426_MOVE_FACE_IN_MIRROR))
        {
            var fullTransform = getFullPatternTransform(context);
            if (abs(determinant(fullTransform.linear) + 1) < TOLERANCE.zeroLength) //det == -1
            {
                //we have a reflection on the input body, flip direction
                directionSign = -directionSign;
            }
        }


        if (definition.limitType == MoveFaceBoundingType.UP_TO_ENTITY && definition.hasOffset)
        {
            definition.offset = definition.offset * (definition.oppositeOffsetDirection ? -1 : 1);
        }
        else
        {
            definition.offset = 0 * meter;
        }

        // Extract an axis defined by the moved face for use in the manipulators.
        var facePlane = try(evFaceTangentPlane(context, { "face" : resolvedEntities[0], "parameter" : vector(0.5, 0.5) }));
        if (facePlane == undefined)
            throw regenError(ErrorStringEnum.NO_TANGENT_PLANE, [definition.queryParameter]);
        const associatedSMEntities = getSMDefinitionEntities(context, resolvedEntities[0], EntityType.FACE);
        if (size(associatedSMEntities) == 1)
        {
            const sheetMetalModelPlane = try silent(evFaceTangentPlane(context, { "face" : associatedSMEntities[0], "parameter" : vector(0.5, 0.5) }));
            if (sheetMetalModelPlane != undefined)
            {
                facePlane.origin = sheetMetalModelPlane.origin;
            }
        }

        if (definition.moveFaceType == MoveFaceType.OFFSET)
        {
            if (definition.limitType != MoveFaceBoundingType.BLIND)
            {
                definition.offsetDistance = getOffsetToEntity(context, resolvedEntities[0], definition, id, facePlane);
            }
            else
            {
                definition.offsetDistance = definition.offsetDistance * directionSign;
                addOffsetManipulator(context, id, definition.offsetDistance, facePlane);
            }

            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V484_MOVE_FACE_0_DISTANCE))
            {
                if (tolerantEquals(definition.offsetDistance, 0 * meter))
                {
                  return;
                }
            }
            sheetMetalAwareMoveFace(context, id, definition);
        }
        else
        {
            if (definition.moveFaceType == MoveFaceType.TRANSLATE)
            {
                // If the user specified an axis for the direction, we will use that for the translation.  If they,
                // specified a face, we will use the face's normal, if it is planar.
                var translationDirection = extractDirection(context, definition.direction);
                if (translationDirection == undefined)
                {
                    throw regenError(ErrorStringEnum.NO_TRANSLATION_DIRECTION, ["direction"]);
                }

                if (definition.limitType != MoveFaceBoundingType.BLIND)
                {
                    definition.transform = getTranslationTransformToEntity(context, resolvedEntities[0], definition, id, translationDirection, facePlane);
                }
                else
                {
                    var translation = translationDirection * definition.translationDistance * directionSign;
                    definition.transform = transform(translation);
                    addTranslateManipulator(context, id, facePlane.origin, translationDirection, definition.translationDistance * directionSign);

                    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V484_MOVE_FACE_0_DISTANCE))
                    {
                        if (tolerantEquals(definition.translationDistance, 0 * meter))
                        {
                            return;
                        }
                    }
                }
                definition.direction = translationDirection;
            }
            if (definition.moveFaceType == MoveFaceType.ROTATE)
            {
                const axisResult = evAxis(context, { "axis" : getAssociatedAxis(context, definition) });
                addRotateManipulator(context, id, axisResult, facePlane, definition.angle * directionSign, definition.moveFaces);
                if (definition.outputType == MoveFaceOutputType.MOVE && isAtVersionOrLater(context, FeatureScriptVersionNumber.V484_MOVE_FACE_0_DISTANCE))
                {
                    if (tolerantEquals(definition.angle, 0 * radian))
                    {
                        return;
                    }
                }
                // Since parasolid works off the transform only, it will try construct faces along the shortest path
                // to the transformed face(s). For angles >= PI this means it will try to construct in the wrong direction.
                // Therefore we split the rotation into two steps.
                if (definition.angle >= (PI - TOLERANCE.zeroAngle) * radian && isAtVersionOrLater(context, FeatureScriptVersionNumber.V309_MOVE_FACE_SUPPORT_360_DEG_ROTATION))
                {
                    const transform = rotationAround(axisResult, definition.angle / 2 * directionSign);
                    definition.transformList = [transform, transform];
                }
                else
                {
                    definition.transform = rotationAround(axisResult, definition.angle * directionSign);
                }
            }
            sheetMetalAwareMoveFace(context, id, definition);
        }
    }, {outputType : MoveFaceOutputType.MOVE, oppositeDirection : false, reFillet : false, limitType : MoveFaceBoundingType.BLIND, hasOffset : false});

function getOffsetToEntity(context is Context, face is Query, definition is map, id is Id, faceTangentPlane is Plane) returns ValueWithUnits
{
    // If moving up to a construction plane, treat it as an infinite plane rather than a ~6 in square.
    const limitPlaneEntity = evaluateQuery(context, qGeometry(qConstructionFilter(definition.limitQuery, ConstructionObject.YES), GeometryType.PLANE));

    const distanceResult = try silent(evDistance(context, {
                    "side0" : face,
                    "side1" : definition.limitQuery,
                    "extendSide0" : true,
                    "extendSide1" : size(limitPlaneEntity) == 1
                }));

    if (distanceResult == undefined)
    {
        throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["limitEntity"]);
    }

    // faceTangentPlane is at the parametric center of the face and is used for placing the manipulator.
    // tangentPlane could be anywhere on the surface and is used to very that the offset is correct.
    const tangentPlane = evFaceTangentPlane(context, {
                "face" : face,
                "parameter" : distanceResult.sides[0].parameter
            });

    const facePoint = distanceResult.sides[0].point;
    const limitPoint = distanceResult.sides[1].point;

    // If the face normal is not parallel to the line from the evDistance call, then a boundary condition was found rather
    // than a minimum/maximum, and offsetting the face will not reach the entity. If the distance is 0, the operation
    // can succeed as a no-op anyway.
    if (!parallelVectors(tangentPlane.normal, facePoint - limitPoint) && !tolerantEquals(distanceResult.distance, 0 * meter))
    {
        throw regenError(ErrorStringEnum.MOVE_FACE_NO_INTERSECTION, ["limitEntity"], definition.limitQuery);
    }

    const offsetDistance = dot(tangentPlane.normal, limitPoint - facePoint);
    if ((definition.limitType == MoveFaceBoundingType.UP_TO_ENTITY) && definition.hasOffset)
    {
        // Move the manipulator plane by the offset distance so it will line up with the preview.
        faceTangentPlane.origin = faceTangentPlane.origin + offsetDistance * faceTangentPlane.normal;
        addOffsetManipulator(context, id, definition.offset, faceTangentPlane);
    }

    return offsetDistance + definition.offset;
}

function getTranslationTransformToEntity(context is Context, face is Query, definition is map, id is Id, translationDirection is Vector, faceTangentPlane is Plane) returns Transform
{
    var limitPoint = try silent(evVertexPoint(context, { "vertex" : definition.limitEntity }));
    if (limitPoint == undefined)
    {
        const limitPlane = try silent(evPlane(context, { "face" : definition.limitEntity }));
        if (limitPlane == undefined)
        {
            throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["limitEntity"]);
        }
        const facePlane = try silent(evPlane(context, {
                        "face" : face
                    }));
        if (facePlane == undefined)
        {
            throw regenError(ErrorStringEnum.TRANSLATION_FACE_NOT_PLANAR, definition.queryParameter, face);
        }
        if (!parallelVectors(limitPlane.normal, facePlane.normal))
        {
            throw regenError(ErrorStringEnum.UP_TO_FACE_NOT_PARALLEL);
        }
        limitPoint = limitPlane.origin;
    }
    const boxSys = coordSystem(plane(limitPoint, translationDirection));
    const boxResult = evBox3d(context, {
                "topology" : face,
                "tight" : true,
                "cSys" : boxSys
            });

    // This is more used to establish the direction of the line because it doesn't have an end.
    // Projecting the z value of this corner of the bounding box will cause the solution found by evDistance
    // to be the closest one in the direction of translationDirection if the limitEntity is between two or more possible solutions.
    const lineEnd = limitPoint + (translationDirection * boxResult.minCorner[2]);
    var lineDirection = lineEnd - limitPoint;
    if (tolerantEquals(lineDirection, vector(0, 0, 0) * meter))
    {
        lineDirection = translationDirection;
    }
    else
    {
        lineDirection = normalize(lineDirection);
    }

    const intersectionLine = line(limitPoint, lineDirection);
    const distanceResult = evDistance(context, {
                "side0" : face,
                "side1" : intersectionLine,
                "extendSide0" : true
            });
    if (!tolerantEquals(distanceResult.distance, 0 * meter))
    {
        throw regenError(ErrorStringEnum.MOVE_FACE_NO_INTERSECTION, definition.queryParameter, face);
    }


    const intersectionPoint = distanceResult.sides[0].point;
    const translation = translationDirection * definition.offset + (limitPoint - intersectionPoint);
    if (definition.limitType == MoveFaceBoundingType.UP_TO_ENTITY && definition.hasOffset)
    {
        addTranslateManipulator(context, id, faceTangentPlane.origin + (limitPoint - intersectionPoint), translationDirection, definition.offset);
    }
    return transform(translation);
}

/**
 * sheetMetalAwareMoveFace effectively has two different modes. For faces that associate to a sheet metal model face,
 * it does a normal move face on the sheet metal model face. For faces that associate to a sheet metal model edge, it
 * uses opExtendSheetBody to move that edge. If that edge is a rip, it does an opExtendSheetBody for both adjacent faces
 * because removing the rip would otherwise change some geometry that was not selected for the operation.
 */
function sheetMetalAwareMoveFace(context is Context, id is Id, definition is map)
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V450_SPLIT_TRACKING_MERGED_EDGES))
    {
        var queries = separateSheetMetalQueries(context, definition.moveFaces);
        const nonSheetMetalQueryCount = size(evaluateQuery(context, queries.nonSheetMetalQueries));
        const sheetMetalQueryCount = size(evaluateQuery(context, queries.sheetMetalQueries));
        if (sheetMetalQueryCount > 0)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V486_MOVE_FACE_PROPAGATE_INFO))
            {
                try
                {
                    offsetSheetMetalFaces(context, id + "smOffset", mergeMaps(definition, { "moveFaces" : queries.sheetMetalQueries}));
                }
                processSubfeatureStatus(context, id, { "subfeatureId" : id + "smOffset", "propagateErrorDisplay" : true, "featureParameterMap" : { "moveFaces" : "moveFaces" } });
            }
            else
            {
                try
                {
                    offsetSheetMetalFaces(context, id + "smOffset", mergeMaps(definition, { "moveFaces" : queries.sheetMetalQueries}));
                }
                catch
                {
                    processSubfeatureStatus(context, id, { "subfeatureId" : id + "smOffset", "propagateErrorDisplay" : true, "featureParameterMap" : { "moveFaces" : "moveFaces" } });
                }
            }
        }
        if (nonSheetMetalQueryCount > 0)
        {
            if (definition.moveFaceType == MoveFaceType.OFFSET)
            {
                opOffsetFace(context, id, mergeMaps(definition, { "moveFaces" : queries.nonSheetMetalQueries }));
            }
            else
            {
                opMoveFace(context, id, mergeMaps(definition, { "moveFaces" : queries.nonSheetMetalQueries }));
            }
        }
    }
    else
    {
        if (definition.moveFaceType == MoveFaceType.OFFSET)
        {
            opOffsetFace(context, id, definition);
        }
        else
        {
            opMoveFace(context, id, definition);
        }
    }
}

/**
 * @param smEntity : A query for a sheet metal edge to move.
 * @param faceToMove : A query for the sheet metal part face that the operation is moving.
 * @param operationInfo : A map of arrays that this function appends information to and then returns.
 */
function createEdgeLimitOption(context is Context, id is Id, definition is map, smEntity is Query, faceToMove is Query, operationInfo is map) returns map
{
    const smEdge = qEntityFilter(smEntity, EntityType.EDGE);
    if (size(evaluateQuery(context, smEdge)) == 1)
    {
        var faceToExtend;
        const adjacentFaces = evaluateQuery(context, qEdgeAdjacent(smEdge, EntityType.FACE));
        if (size(adjacentFaces) == 2)
        {
            // If there are two faces adjacent to the edge, then there is a rip that needs to be removed.
            // In this case, the face that will be modified when the sheet metal edge is moved needs to be specified
            // In edgeLimitOption since opExtendSheetBody cannot infer it.
            const adjacentFaceQuery = qEdgeAdjacent(faceToMove, EntityType.FACE);
            // Find that sheet metal face that needs to be modified in order to move faceToMove.
            const adjacentFaceSMFace = evaluateQuery(context, qEntityFilter(qUnion(getSMDefinitionEntities(context, adjacentFaceQuery)), EntityType.FACE));
            if (size(adjacentFaceSMFace) == 1)
            {
                faceToExtend = adjacentFaceSMFace[0];
                clearSmAttributes(context, smEdge);
            }
            else
            {
                return operationInfo;
            }
            // In the case of a rip, keep track of the sheet metal part faces for later user. If the other one is not
            // Already being moved, the underlying edge will need to be adjusted to keep it in place.
            const attributes = getAttributes(context, { "entities" : smEdge, "attributePattern" : {} as SMAssociationAttribute });
            if (size(attributes) != 1)
            {
                throw regenError(ErrorStringEnum.REGEN_ERROR);
            }
            operationInfo.derippedFaces = append(operationInfo.derippedFaces, qAttributeFilter(qEverything(EntityType.FACE), attributes[0]));
        }

        // Create a plane to use as a limit for opExtendSheetBody for extending faceToMove.
        var limitPlane = try silent(evPlane(context, { "face" : faceToMove }));
        if (limitPlane == undefined)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V522_MOVE_FACE_NONPLANAR))
            {
                // If moving a nonplanar face or up to an entity, create a tool body and use opMoveFace for positioning.
                opExtractSurface(context, id, {"faces" : faceToMove});
                limitPlane = qCreatedBy(id, EntityType.FACE);
                opMoveFace(context, id + "moveFace", mergeMaps(definition, {"moveFaces" : limitPlane}));
            }
            else
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_MOVE_NOT_PLANAR, ["moveFaces"], faceToMove);
            }
        }
        else if (definition.moveFaceType == MoveFaceType.OFFSET)
        {
            limitPlane.origin += limitPlane.normal * definition.offsetDistance;
        }
        else
        {
            limitPlane = definition.transform * limitPlane;
        }
        const edgeLimitOption = { "edge" : smEdge, "limitEntity" : limitPlane, "faceToExtend" : faceToExtend };
        const sheetMetalModel = qOwnerBody(smEdge);
        operationInfo.edgeLimitOptions = append(operationInfo.edgeLimitOptions, edgeLimitOption);
        operationInfo.edgesToExtend = append(operationInfo.edgesToExtend, smEdge);
        operationInfo.sheetMetalModels = append(operationInfo.sheetMetalModels, sheetMetalModel);
    }
    return operationInfo;
}

function createToolBodies(context is Context, id is Id, entities is Query, definition is map) returns map
{
    // A map to gather queries across multiple calls to createEdgeLimitOptions.
    // edgeLimitOptions is an array of overrides for specific edges for opExtendSheetBody.
    // alignedSMFaces is all sheet metal model faces being moved that have a normal aligned with the selected sheet metal part normal.
    // modifiedFaces are a superset of the faces that will be modified by the operation.
    // sheetMetalModels is all sheet metal models that will be altered by this operation.
    // derippedFaces are side faces corresponding to rips that will be removed.
    // edgesToExtend is the set of edges to be replaced by the opExtendSheetBody operation.
    var operationInfo = { "edgeLimitOptions" : [],
        "alignedSMFaces" : [],
        "antiAlignedSMFaces" : [],
        "modifiedFaces" : [],
        "sheetMetalModels" : [],
        "derippedFaces" : [],
        "edgesToExtend" : []};
    var index = 0;
    for (var evaluatedFace in evaluateQuery(context, entities))
    {
        // Separate selections into two categories: sheet metal model faces that the normal moveFace process will work on (aligned and antiAlignedFaces),
        // and edges that need to be moved with opExtendSheetBody.
        const smEntity = qUnion(getSMDefinitionEntities(context, evaluatedFace));
        const smFace = qEntityFilter(smEntity, EntityType.FACE);
        if (size(evaluateQuery(context, smFace)) == 1)
        {
            const partFaceNormal = evPlane(context, { "face" : evaluatedFace }).normal;
            const smModelFaceNormal = evPlane(context, { "face" : smFace }).normal;
            if (dot(partFaceNormal, smModelFaceNormal) > 0)
            {
                operationInfo.alignedSMFaces = append(operationInfo.alignedSMFaces, smFace);
            }
            else
            {
                operationInfo.antiAlignedSMFaces = append(operationInfo.antiAlignedSMFaces, smFace);
            }
            operationInfo.sheetMetalModels = append(operationInfo.sheetMetalModels, qOwnerBody(smFace));
        }

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V517_MOVE_FACE_CHECK_BEND_EDGE))
        {
            // Check that the entity is not a bend edge. Checking here so that the picked face can be highlighted.
            const jointAttribute = try silent(getJointAttribute(context, smEntity));
            if (jointAttribute != undefined && jointAttribute.jointType != undefined && jointAttribute.jointType.value == SMJointType.BEND)
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_CANNOT_MOVE_BEND_EDGE, ["moveFaces"], evaluatedFace);
            }
        }

        operationInfo = createEdgeLimitOption(context, id + unstableIdComponent(index), definition, smEntity, evaluatedFace, operationInfo);
        index += 1;
    }
    operationInfo.sheetMetalModels = qUnion(evaluateQuery(context, qUnion(operationInfo.sheetMetalModels)));
    operationInfo.modifiedFaces = qEdgeAdjacent(qUnion(concatenateArrays([operationInfo.alignedSMFaces, operationInfo.antiAlignedSMFaces, operationInfo.edgesToExtend])), EntityType.FACE);

    return operationInfo;
}

// Use association entity for axis if it is adjacent to a sheet metal face being moved.
function getAssociatedAxis(context is Context, definition) returns Query
{
    if (definition.outputType == MoveFaceOutputType.MOVE)
    {
        const axisSMEntities = qUnion(getSMDefinitionEntities(context, definition.axis, EntityType.EDGE));
        const moveFaceSMEntities = qEdgeAdjacent(qUnion(getSMDefinitionEntities(context, definition.moveFaces, EntityType.FACE)), EntityType.EDGE);
        const axisCandidates = evaluateQuery(context, qIntersection([axisSMEntities, moveFaceSMEntities]));
        if (size(axisCandidates) != 1)
        {
            return definition.axis;
        }
        else
        {
            return axisCandidates[0];
        }
    }
    else
    {
        return definition.axis;
    }
}

const offsetSheetMetalFaces = defineSheetMetalFeature(function(context is Context, id is Id, definition)
    {
        const toolId = id + "tool";
        const operationInfo = createToolBodies(context, toolId + unstableIdComponent("open"), definition.moveFaces, definition);

        // Find the faces that will be modified by deripping that haven't otherwise been moved and make sure they stay in place.
        var amendedFaces = qSubtraction(qUnion(operationInfo.derippedFaces), definition.moveFaces);
        var copiedDefinition = definition;
        copiedDefinition.moveFaceType = MoveFaceType.OFFSET;
        copiedDefinition.offsetDistance = 0 * meter;
        const derippingOperationInfo = createToolBodies(context, toolId + unstableIdComponent("derip"), amendedFaces, copiedDefinition);

        const originalEntities = evaluateQuery(context, qOwnedByBody(operationInfo.sheetMetalModels));
        const initialAssociationAttributes = getAttributes(context, {
                    "entities" : qOwnedByBody(operationInfo.sheetMetalModels),
                    "attributePattern" : {} as SMAssociationAttribute });

        const edgeLimitOptions = concatenateArrays([derippingOperationInfo.edgeLimitOptions, operationInfo.edgeLimitOptions]);
        const modifiedFaces = operationInfo.modifiedFaces;
        const smEdges = operationInfo.edgesToExtend;
        const trackingSMModel = startTracking(context, operationInfo.sheetMetalModels);
        const allFaces = qUnion(concatenateArrays([operationInfo.alignedSMFaces, operationInfo.antiAlignedSMFaces]));
        const newly2SidedAsRips = isAtVersionOrLater(context, FeatureScriptVersionNumber.V664_FLAT_JOINT_TO_RIP);
        var edgesToTrack = qUnion(smEdges);
        if (newly2SidedAsRips)
        {
            edgesToTrack = qEdgeAdjacent(qEdgeAdjacent(edgesToTrack, EntityType.FACE), EntityType.EDGE);
        }
        var modifiedEdges = startTracking(context, edgesToTrack);
        const associateChanges = qUnion([startTracking(context, allFaces), modifiedEdges]);
        const mergeFaces = (definition.moveFaceType != MoveFaceType.ROTATE) && isAtVersionOrLater(context, FeatureScriptVersionNumber.V528_MOVE_FACE_MERGE);
        if (definition.moveFaceType != MoveFaceType.OFFSET)
        {
            if (size(evaluateQuery(context, allFaces)) > 0)
            {
                opMoveFace(context, id + "offset", mergeMaps(definition, { "moveFaces" : allFaces, "mergeFaces" : mergeFaces }));
            }
            if (definition.moveFaceType == MoveFaceType.ROTATE)
            {
                updateJointAngle(context, qEdgeAdjacent(allFaces, EntityType.EDGE));
            }
        }
        else
        {
            if (size(operationInfo.alignedSMFaces) > 0)
            {
                opOffsetFace(context, id + "offset" + unstableIdComponent(1), { "moveFaces" : qUnion(operationInfo.alignedSMFaces), "offsetDistance" : definition.offsetDistance, "mergeFaces" : mergeFaces });
            }
            if (size(operationInfo.antiAlignedSMFaces) > 0)
            {
                opOffsetFace(context, id + "offset" + unstableIdComponent(2), { "moveFaces" : qUnion(operationInfo.antiAlignedSMFaces), "offsetDistance" : -definition.offsetDistance, "mergeFaces" : mergeFaces });
            }
        }
        if (size(edgeLimitOptions) > 0)
        {
            sheetMetalExtendSheetBodyCall(context, id + "extend", {
                        "entities" : qUnion(smEdges),
                        "extendMethod" : ExtendSheetBoundingType.EXTEND_TO_SURFACE,
                        "edgeLimitOptions" : edgeLimitOptions });
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V512_MOVE_FACE_OVERLAP))
            {
                if (size(evaluateQuery(context, qCreatedBy(id + "extend", EntityType.FACE))) != 0)
                {
                    throw regenError(ErrorStringEnum.SHEET_METAL_SELF_INTERSECTING_MODEL);
                }
                modifiedEdges = qUnion([modifiedEdges, qCreatedBy(id + "extend", EntityType.EDGE)]);
            }
            if (!newly2SidedAsRips)
            {
                for (var edge in evaluateQuery(context, modifiedEdges))
                {
                    const adjacentFaces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
                    if (size(adjacentFaces) != 1)
                    {
                        throw regenError(ErrorStringEnum.SHEET_METAL_SELF_INTERSECTING_MODEL, ["moveFaces"], edge);
                    }
                }
            }
        }
        if (newly2SidedAsRips)
        {
            addRipsForNewEdges(context, id,  modifiedEdges);
        }
        const toUpdate = assignSMAttributesToNewOrSplitEntities(context, qUnion([trackingSMModel, operationInfo.sheetMetalModels]),
                originalEntities, initialAssociationAttributes);


        try(updateSheetMetalGeometry(context, id + "smUpdate", {
                        "entities" : qUnion([toUpdate.modifiedEntities, modifiedFaces]),
                        "deletedAttributes" : toUpdate.deletedAttributes,
                        "associatedChanges" : associateChanges }));
        processSubfeatureStatus(context, id, { "subfeatureId" : id + "smUpdate", "propagateErrorDisplay" : true });
        try silent(opDeleteBodies(context, id + "delete", {
                "entities" : qCreatedBy(toolId, EntityType.BODY)
        }));
    }, {});

// Manipulator functions

const OFFSET_MANIPULATOR = "offsetManipulator";
const TRANSLATE_MANIPULATOR = "translateManipulator";
const ROTATE_MANIPULATOR = "rotateManipulator";

function addOffsetManipulator(context is Context, id is Id, offsetDistance is ValueWithUnits, facePlane is Plane)
{
    addManipulators(context, id, { (OFFSET_MANIPULATOR) :
                    linearManipulator(facePlane.origin, facePlane.normal, offsetDistance) });
}

function addTranslateManipulator(context is Context, id is Id, origin is Vector, direction is Vector, magnitude is ValueWithUnits)
{
    addManipulators(context, id, { (TRANSLATE_MANIPULATOR) :
                    linearManipulator(origin, direction, magnitude) });
}

function addRotateManipulator(context is Context, id is Id, axis is Line, facePlane is Plane, angle is ValueWithUnits, faceQuery is Query)
{
    // Project the center of the plane onto the axis
    var refPoint = facePlane.origin;
    const rotateOrigin = axis.origin + dot(refPoint - axis.origin, axis.direction) * axis.direction;
    if (tolerantEquals(rotateOrigin, refPoint))
    {
        // refPoint lies on the axis, so construct a different refPoint
        var orthoVec = cross(axis.direction, facePlane.normal);
        const orthoVecNorm = norm(orthoVec);
        if (abs(orthoVecNorm) > TOLERANCE.zeroLength)
        {
            orthoVec = orthoVec / orthoVecNorm;
        }
        else
        {
            // The plane normal is parallel to the axis, so choose an arbitrary orthogonal vector
            orthoVec = perpendicularVector(axis.direction);
        }
        // Calculate a manipulator radius if we have to use an arbitrary face point.
        const faceBox = try(evBox3d(context, { topology : qNthElement(faceQuery, 0) }));
        var manipulatorRadius = 0.001 * meter; // default of 1 mm if we fail to get the box
        if (faceBox != undefined)
            manipulatorRadius = norm(faceBox.maxCorner - faceBox.minCorner) * 0.5;
        refPoint = rotateOrigin + orthoVec * manipulatorRadius;
    }
    var minValue = -2 * PI * radian;
    var maxValue = 2 * PI * radian;

    addManipulators(context, id, { (ROTATE_MANIPULATOR) : angularManipulator({ "axisOrigin" : rotateOrigin,
                            "axisDirection" : axis.direction,
                            "rotationOrigin" : refPoint,
                            "angle" : angle,
                            "minValue" : minValue,
                            "maxValue" : maxValue }) });
}

/**
 * @internal
 * Manipulator change function for `moveFace`.
 */
export function moveFaceManipulatorChange(context is Context, moveFaceDefinition is map, newManipulators is map) returns map
precondition
{
}
{
    var newValue = 0 * meter;
    if (moveFaceDefinition.moveFaceType == MoveFaceType.OFFSET)
    {
        newValue = newManipulators[OFFSET_MANIPULATOR].offset;
        if (moveFaceDefinition.limitType == MoveFaceBoundingType.UP_TO_ENTITY && moveFaceDefinition.hasOffset)
        {
            moveFaceDefinition.offset = abs(newValue);
        }
        else
        {
            moveFaceDefinition.offsetDistance = abs(newValue);
        }
    }
    else if (moveFaceDefinition.moveFaceType == MoveFaceType.TRANSLATE)
    {
        newValue = newManipulators[TRANSLATE_MANIPULATOR].offset;
        if (moveFaceDefinition.limitType == MoveFaceBoundingType.UP_TO_ENTITY && moveFaceDefinition.hasOffset)
        {
            moveFaceDefinition.offset = abs(newValue);
        }
        else
        {
            moveFaceDefinition.translationDistance = abs(newValue);
        }
    }
    else if (moveFaceDefinition.moveFaceType == MoveFaceType.ROTATE)
    {
        newValue = newManipulators[ROTATE_MANIPULATOR].angle;
        moveFaceDefinition.angle = abs(newValue);
    }

    if (moveFaceDefinition.limitType == MoveFaceBoundingType.UP_TO_ENTITY && moveFaceDefinition.hasOffset)
    {
        moveFaceDefinition.oppositeOffsetDirection = newValue.value < 0;
    }
    else
    {
        moveFaceDefinition.oppositeDirection = newValue.value < 0;
    }

    return moveFaceDefinition;
}

/**
 * Editing logic. Fills in translation direction. Fills in offset distance as minimal clearance.
 */
export function moveFaceEditingLogic(context is Context, id is Id, oldDefinition is map, definition is map, specifiedParameters is map) returns map
{
    if (definition.moveFaceType == MoveFaceType.TRANSLATE && !specifiedParameters.direction)
    {
        const moveFaces = evaluateQuery(context, qGeometry(definition.moveFaces, GeometryType.PLANE));
        if (size(moveFaces) != 0)
        {
            definition.direction = moveFaces[0];
        }
    }

    if (specifiedParameters.offset || specifiedParameters.oppositeOffsetDirection)
        return definition;

    const limitEntity = definition.moveFaceType == MoveFaceType.TRANSLATE ? definition.limitEntity : definition.limitQuery;
    const smLimitEntityBodies = try silent(getOwnerSMModel(context, limitEntity));

    if (smLimitEntityBodies is undefined || size(smLimitEntityBodies) != 1)
        return definition;

    var smMoveFacesBodies = try silent(getOwnerSMModel(context, definition.moveFaces));
    if (smMoveFacesBodies is undefined || size(smMoveFacesBodies) != 1)
        return definition;

    const evaluatedBodies = evaluateQuery(context, qOwnerBody(qUnion(concatenateArrays([smLimitEntityBodies, smMoveFacesBodies]))));
    if (size(evaluatedBodies) != 1)
        return definition;

    const modelParameters = try silent(getModelParameters(context, evaluatedBodies[0]));
    if (modelParameters is undefined)
        return definition;

    definition.offset = modelParameters.minimalClearance;

    return definition;
}

function addRipsForNewEdges(context is Context, id is Id, edges is Query)
{
    var index = 0;
    for (var edge in evaluateQuery(context, edges))
    {
        const jointAttribute = try silent(getJointAttribute(context, edge));
        if (jointAttribute != undefined)
        {
            continue;
        }
        var adjacentFaces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
        if (size(adjacentFaces) == 2)
        {
            const jointAttributeId = toAttributeId(id + "joint" + index);
            const ripAttribute = createRipAttribute(context, edge, jointAttributeId, SMJointStyle.EDGE, {});
            setAttribute(context, {"entities" : edge, "attribute" : ripAttribute});
        }
        index += 1;
    }
}

/*
 * @internal
 * Code that creates surfaces, which is now reverted. Only kept around for legacy features/upgrades purposes.
 */
function moveFaceCreateLegacy(context is Context, id is Id, definition is map, qSurfacesAndFaces is Query)
{
    const resolvedEntities = evaluateQuery(context, qSurfacesAndFaces);
    if (size(resolvedEntities) == 0)
        throw regenError(definition.selectionErrorStringEnum, [definition.queryParameter]);

    var directionSign = 1;
    if (definition.oppositeDirection)
        directionSign = -1;

    if (definition.moveFaceType != MoveFaceType.OFFSET &&
        isAtVersionOrLater(context, FeatureScriptVersionNumber.V426_MOVE_FACE_IN_MIRROR))
    {
        var fullTransform = getFullPatternTransform(context);
        if (abs(determinant(fullTransform.linear) + 1) < TOLERANCE.zeroLength) //det == -1
        {
            //we have a reflection on the input body, flip direction
            directionSign = -directionSign;
        }
    }


    if (definition.limitType == MoveFaceBoundingType.UP_TO_ENTITY && definition.hasOffset)
    {
        definition.offset = definition.offset * (definition.oppositeOffsetDirection ? -1 : 1);
    }
    else
    {
        definition.offset = 0 * meter;
    }

    // Extract an axis defined by the moved face for use in the manipulators.
    var facePlane = try(evFaceTangentPlane(context, { "face" : resolvedEntities[0], "parameter" : vector(0.5, 0.5) }));
    if (facePlane == undefined)
        throw regenError(ErrorStringEnum.NO_TANGENT_PLANE, [definition.queryParameter]);

    if (definition.moveFaceType == MoveFaceType.OFFSET)
    {
        if (definition.limitType != MoveFaceBoundingType.BLIND)
        {
            definition.offsetDistance = getOffsetToEntity(context, resolvedEntities[0], definition, id, facePlane);
        }
        else
        {
            definition.offsetDistance = definition.offsetDistance * directionSign;
            addOffsetManipulator(context, id, definition.offsetDistance, facePlane);
        }

        opExtractSurface(context, id, {"faces" : qSurfacesAndFaces,
                "offset" : definition.offsetDistance,
                "useFacesAroundToTrimOffset" : false
            });
    }
    else
    {
        if (definition.moveFaceType == MoveFaceType.TRANSLATE)
        {
            // If the user specified an axis for the direction, we will use that for the translation.  If they,
            // specified a face, we will use the face's normal, if it is planar.
            var translation;
            const directionResult = try silent(evAxis(context, { "axis" : definition.direction }));
            var translationDirection;
            if (directionResult == undefined)
            {
                const planeResult = try silent(evPlane(context, { "face" : definition.direction }));
                if (planeResult == undefined)
                    throw regenError(ErrorStringEnum.NO_TRANSLATION_DIRECTION, ["direction"]);
                translationDirection = planeResult.normal;
            }
            else
            {
                translationDirection = directionResult.direction;
            }

            if (definition.limitType != MoveFaceBoundingType.BLIND)
            {
                definition.transform = getTranslationTransformToEntity(context, resolvedEntities[0], definition, id, translationDirection, facePlane);
            }
            else
            {
                translation = translationDirection * definition.translationDistance * directionSign;
                definition.transform = transform(translation);
                addTranslateManipulator(context, id, facePlane.origin, translationDirection, definition.translationDistance * directionSign);
            }
            definition.direction = translationDirection;
        }
        if (definition.moveFaceType == MoveFaceType.ROTATE)
        {
            const axisResult = evAxis(context, { "axis" : getAssociatedAxis(context, definition) });
            addRotateManipulator(context, id, axisResult, facePlane, definition.angle * directionSign, definition.moveFaces);

            definition.transform = rotationAround(axisResult, definition.angle * directionSign);
        }

        opExtractSurface(context, id + "extractSurface", {
            "faces" : qSurfacesAndFaces,
            "offset" : 0 * meter
        });

        opTransform(context, id + "transform", {
            "bodies" : qCreatedBy(id + "extractSurface", EntityType.BODY),
            "transform" : definition.transform
        });
    }
}

/**
 * Splits input sheet metal edges and adjusts them to lie on corresponding sheet metal part faces.
 */
export function deripEdges(context is Context, id is Id, edges is Query) returns boolean
{
    var facesToMove = [];
    for (var edge in evaluateQuery(context, edges))
    {
        const adjacentFaces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
        if (size(adjacentFaces) != 2)
        {
            continue;
        }

        const jointAttribute = try silent(getJointAttribute(context, edge));
        if (jointAttribute == undefined || jointAttribute.jointType == undefined || jointAttribute.jointType.value != SMJointType.RIP)
        {
            return false;
        }

        const attributes = getAttributes(context, { "entities" : edge, "attributePattern" : {} as SMAssociationAttribute });
        if (size(attributes) != 1)
        {
            return false;
        }
        facesToMove = append(facesToMove, qAttributeFilter(qEverything(EntityType.FACE), attributes[0]));
    }

    if (size(facesToMove) != 0)
    {
        var definition = { "moveFaces" : qUnion(facesToMove),
            "moveFaceType" : MoveFaceType.OFFSET,
            "offsetDistance" : 0 * meter };

        const toolId = id + "tool";
        const operationInfo = createToolBodies(context, toolId, definition.moveFaces, definition);
        if (size(operationInfo.edgeLimitOptions) > 0)
        {
            sheetMetalExtendSheetBodyCall(context, id + "extend", {
                        "entities" : qUnion(operationInfo.edgesToExtend),
                        "extendMethod" : ExtendSheetBoundingType.EXTEND_TO_SURFACE,
                        "edgeLimitOptions" : operationInfo.edgeLimitOptions });
        }
        try silent(opDeleteBodies(context, id + "deleteBodies", {
                    "entities" : qCreatedBy(toolId, EntityType.BODY)
                }));
    }

    return true;
}

