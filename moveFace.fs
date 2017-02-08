FeatureScript 505; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "505.0");
export import(path : "onshape/std/tool.fs", version : "505.0");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "505.0");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "505.0");
import(path : "onshape/std/box.fs", version : "505.0");
import(path : "onshape/std/containers.fs", version : "505.0");
import(path : "onshape/std/curveGeometry.fs", version : "505.0");
import(path : "onshape/std/evaluate.fs", version : "505.0");
import(path : "onshape/std/feature.fs", version : "505.0");
import(path : "onshape/std/mathUtils.fs", version : "505.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "505.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "505.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "505.0");
import(path : "onshape/std/valueBounds.fs", version : "505.0");

/** @internal */
export const MOVE_FACE_OFFSET_BOUNDS = NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS;
/** @internal */
export const MOVE_FACE_TRANSLATE_BOUNDS = NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS;
/** @internal */
export const MOVE_FACE_ROTATION_BOUNDS = ANGLE_360_ZERO_DEFAULT_BOUNDS;

/**
 * Feature performing an [opMoveFace].
 */
annotation { "Feature Type Name" : "Move face",
             "Manipulator Change Function" : "moveFaceManipulatorChange",
             "Filter Selector" : "allparts" }
export const moveFace = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Faces",
                     "UIHint" : "SHOW_CREATE_SELECTION",
                     "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES }
        definition.moveFaces is Query;

        annotation { "Name" : "Move face type" }
        definition.moveFaceType is MoveFaceType;

        if (definition.moveFaceType == MoveFaceType.TRANSLATE)
        {
            annotation { "Name" : "Direction",
                         "Filter" : QueryFilterCompound.ALLOWS_AXIS || GeometryType.PLANE,
                         "MaxNumberOfPicks" : 1 }
            definition.direction is Query;

            annotation { "Name" : "Distance" }
            isLength(definition.translationDistance, MOVE_FACE_TRANSLATE_BOUNDS);
        }

        if (definition.moveFaceType == MoveFaceType.ROTATE)
        {
            annotation { "Name" : "Axis",
                         "Filter" : QueryFilterCompound.ALLOWS_AXIS,
                         "MaxNumberOfPicks" : 1 }
            definition.axis is Query;
            annotation { "Name" : "Rotation angle" }
            isAngle(definition.angle, MOVE_FACE_ROTATION_BOUNDS);
        }

        if (definition.moveFaceType == MoveFaceType.OFFSET)
        {
            annotation { "Name" : "Distance" }
            isLength(definition.offsetDistance, MOVE_FACE_OFFSET_BOUNDS);
        }

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Reapply fillet", "Default" : true }
        definition.reFillet is boolean;
    }
    //============================ Body =============================
    {
        const resolvedEntities = evaluateQuery(context, definition.moveFaces);
        if (size(resolvedEntities) == 0)
            throw regenError(ErrorStringEnum.DIRECT_EDIT_MOVE_FACE_SELECT, ["moveFaces"]);

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

        // Extract an axis defined by the moved face for use in the manipulators.
        var facePlane = try(evFaceTangentPlane(context, { "face" : resolvedEntities[0], "parameter" : vector(0.5, 0.5) }));
        if (facePlane == undefined)
            throw regenError(ErrorStringEnum.NO_TANGENT_PLANE, ["moveFaces"]);
        // Extract an axis defined by the moved face for use in the manipulators.
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
            definition.offsetDistance = definition.offsetDistance * directionSign;

            addOffsetManipulator(context, id, definition, facePlane);

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
                var translation;
                const directionResult = try(evAxis(context, { "axis" : definition.direction }));
                var translationDirection;
                if (directionResult == undefined)
                {
                    const planeResult = try(evPlane(context, { "face" : definition.direction }));
                    if (planeResult == undefined)
                        throw regenError(ErrorStringEnum.NO_TRANSLATION_DIRECTION, ["direction"]);
                    translation = planeResult.normal * definition.translationDistance * directionSign;
                    translationDirection = planeResult.normal;
                }
                else
                {
                    translation = directionResult.direction * definition.translationDistance * directionSign;
                    translationDirection = directionResult.direction;
                }

                addTranslateManipulator(context, id, facePlane.origin, translationDirection, definition.translationDistance * directionSign);

                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V484_MOVE_FACE_0_DISTANCE))
                {
                    if (tolerantEquals(definition.translationDistance, 0 * meter))
                    {
                        return;
                    }
                }
                definition.transform = transform(translation);
            }
            if (definition.moveFaceType == MoveFaceType.ROTATE)
            {
                const axisResult = evAxis(context, { "axis" : getAssociatedAxis(context, definition) });

                addRotateManipulator(context, id, axisResult, facePlane, definition.angle * directionSign, definition.moveFaces);

                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V484_MOVE_FACE_0_DISTANCE))
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
    }, { oppositeDirection : false, reFillet : false });

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
function createEdgeLimitOption(context is Context, definition is map, smEntity is Query, faceToMove is Query, operationInfo is map) returns map
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
            throw regenError(ErrorStringEnum.SHEET_METAL_MOVE_NOT_PLANAR, ["moveFaces"], faceToMove);
        }
        if (definition.moveFaceType == MoveFaceType.OFFSET)
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

function createToolBodies(context is Context, entities is Query, definition is map) returns map
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
        "edgesToExtend" : [] };
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
        operationInfo = createEdgeLimitOption(context, definition, smEntity, evaluatedFace, operationInfo);
    }
    operationInfo.sheetMetalModels = qUnion(evaluateQuery(context, qUnion(operationInfo.sheetMetalModels)));
    operationInfo.modifiedFaces = qEdgeAdjacent(qUnion(concatenateArrays([operationInfo.alignedSMFaces, operationInfo.antiAlignedSMFaces, operationInfo.edgesToExtend])), EntityType.FACE);

    return operationInfo;
}

// Use association entity for axis if it is adjacent to a sheet metal face being moved.
function getAssociatedAxis(context is Context, definition) returns Query
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

const offsetSheetMetalFaces = defineSheetMetalFeature(function(context is Context, id is Id, definition)
    {
        const operationInfo = createToolBodies(context, definition.moveFaces, definition);

        // Find the faces that will be modified by deripping that haven't otherwise been moved and make sure they stay in place.
        var amendedFaces = qSubtraction(qUnion(operationInfo.derippedFaces), definition.moveFaces);
        var copiedDefinition = definition;
        copiedDefinition.moveFaceType = MoveFaceType.OFFSET;
        copiedDefinition.offsetDistance = 0 * meter;
        const derippingOperationInfo = createToolBodies(context, amendedFaces, copiedDefinition);

        const originalEntities = evaluateQuery(context, qOwnedByBody(operationInfo.sheetMetalModels));
        const initialAssociationAttributes = getAttributes(context, {
                    "entities" : qOwnedByBody(operationInfo.sheetMetalModels),
                    "attributePattern" : {} as SMAssociationAttribute });

        const edgeLimitOptions = concatenateArrays([derippingOperationInfo.edgeLimitOptions, operationInfo.edgeLimitOptions]);
        const modifiedFaces = operationInfo.modifiedFaces;
        const smEdges = operationInfo.edgesToExtend;
        const trackingSMModel = startTracking(context, operationInfo.sheetMetalModels);
        const allFaces = qUnion(concatenateArrays([operationInfo.alignedSMFaces, operationInfo.antiAlignedSMFaces]));
        const modifiedEdges = startTracking(context, qUnion(smEdges));
        const associateChanges = qUnion([startTracking(context, allFaces), modifiedEdges]);
        if (definition.moveFaceType != MoveFaceType.OFFSET)
        {
            if (size(evaluateQuery(context, allFaces)) > 0)
            {
                opMoveFace(context, id + "offset", mergeMaps(definition, { "moveFaces" : allFaces, "mergeFaces" : false }));
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
                opOffsetFace(context, id + "offset" + unstableIdComponent(1), { "moveFaces" : qUnion(operationInfo.alignedSMFaces), "offsetDistance" : definition.offsetDistance, "mergeFaces" : false });
            }
            if (size(operationInfo.antiAlignedSMFaces) > 0)
            {
                opOffsetFace(context, id + "offset" + unstableIdComponent(2), { "moveFaces" : qUnion(operationInfo.antiAlignedSMFaces), "offsetDistance" : -definition.offsetDistance, "mergeFaces" : false });
            }
        }
        if (size(edgeLimitOptions) > 0)
        {
            opExtendSheetBody(context, id + "extend", { "entities" : qUnion(smEdges), "extendMethod" : ExtendSheetBoundingType.EXTEND_TO_SURFACE, "edgeLimitOptions" : edgeLimitOptions });
            for (var edge in evaluateQuery(context, modifiedEdges))
            {
                const adjacentFaces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
                if (size(adjacentFaces) != 1)
                {
                    throw regenError(ErrorStringEnum.SHEET_METAL_SELF_INTERSECTING_MODEL, ["moveFaces"], edge);
                }
            }
        }
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V483_FLAT_QUERY_EVAL_FIX))
        {
            addRipsForNewEdges(context, id);
        }
        const toUpdate = assignSMAttributesToNewOrSplitEntities(context, qUnion([trackingSMModel, operationInfo.sheetMetalModels]),
                originalEntities, initialAssociationAttributes);


        try(updateSheetMetalGeometry(context, id + "smUpdate", {
                    "entities" : qUnion([toUpdate.modifiedEntities, modifiedFaces]),
                    "deletedAttributes" : toUpdate.deletedAttributes,
                    "associatedChanges" : associateChanges }));
        processSubfeatureStatus(context, id, { "subfeatureId" : id + "smUpdate", "propagateErrorDisplay" : true });
    }, {});

// Manipulator functions

const OFFSET_MANIPULATOR = "offsetManipulator";
const TRANSLATE_MANIPULATOR = "translateManipulator";
const ROTATE_MANIPULATOR = "rotateManipulator";

function addOffsetManipulator(context is Context, id is Id, moveFaceDefinition is map, facePlane is Plane)
{
    addManipulators(context, id, { (OFFSET_MANIPULATOR) :
                    linearManipulator(facePlane.origin, facePlane.normal, moveFaceDefinition.offsetDistance) });
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
 * A function for getting associated sheet metal entities outside of a sheet metal feature.
 */
function getSMDefinitionEntities(context is Context, selection is Query, entityType is EntityType) returns array
{
    var entityAssociations = try silent(getAttributes(context, {
                "entities" : qBodyType(selection, BodyType.SOLID),
                "attributePattern" : {} as SMAssociationAttribute
            }));
    var out = [];
    if (entityAssociations != undefined)
    {
        for (var attribute in entityAssociations)
        {
            const modelQuery = qAttributeQuery(asSMAttribute({ "objectType" : SMObjectType.MODEL }));
            const inModelQuery = qOwnedByBody(modelQuery, entityType);
            const isActive = isSheetMetalModelActive(context, modelQuery);
            const returnInactive = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V495_MOVE_FACE_ROTATION_AXIS);
            if (isActive || returnInactive)
            {
                var associatedEntities = evaluateQuery(context, qIntersection([qAttributeQuery(attribute), inModelQuery]));
                out = concatenateArrays([out, associatedEntities]);
            }
        }
    }
    return out;
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
        moveFaceDefinition.offsetDistance = abs(newValue);
    }
    else if (moveFaceDefinition.moveFaceType == MoveFaceType.TRANSLATE)
    {
        newValue = newManipulators[TRANSLATE_MANIPULATOR].offset;
        moveFaceDefinition.translationDistance = abs(newValue);
    }
    else if (moveFaceDefinition.moveFaceType == MoveFaceType.ROTATE)
    {
        newValue = newManipulators[ROTATE_MANIPULATOR].angle;
        moveFaceDefinition.angle = abs(newValue);
    }

    moveFaceDefinition.oppositeDirection = newValue.value < 0;

    return moveFaceDefinition;
}

function addRipsForNewEdges(context is Context, id is Id)
{
    const edges = qCreatedBy(id, EntityType.EDGE);
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
            createRipAttribute(context, edge, jointAttributeId, SMJointStyle.EDGE, {});
        }
        index += 1;
    }
}

