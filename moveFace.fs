FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "✨");
export import(path : "onshape/std/tool.fs", version : "✨");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "✨");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/box.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/mathUtils.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");

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

        annotation { "Name" : "Move Face Type" }
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
        const facePlane = try(evFaceTangentPlane(context, { "face" : resolvedEntities[0], "parameter" : vector(0.5, 0.5) }));
        if (facePlane == undefined)
            throw regenError(ErrorStringEnum.NO_TANGENT_PLANE, ["moveFaces"]);

        if (definition.moveFaceType == MoveFaceType.OFFSET)
        {
            definition.offsetDistance = definition.offsetDistance * directionSign;

            addOffsetManipulator(context, id, definition, facePlane);

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

                definition.transform = transform(translation);
            }
            if (definition.moveFaceType == MoveFaceType.ROTATE)
            {
                const axisResult = evAxis(context, { "axis" : definition.axis });

                addRotateManipulator(context, id, axisResult, facePlane, definition.angle * directionSign, definition.moveFaces);

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

function sheetMetalAwareMoveFace(context is Context, id is Id, definition is map)
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V450_SPLIT_TRACKING_MERGED_EDGES))
    {
        var queries = separateSheetMetalQueries(context, id, definition.moveFaces);
        const nonSheetMetalQueryCount = size(evaluateQuery(context, queries.nonSheetMetalQueries));
        const sheetMetalQueryCount = size(evaluateQuery(context, queries.sheetMetalQueries));
        if (sheetMetalQueryCount > 0)
        {
            if (definition.moveFaceType != MoveFaceType.OFFSET)
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_CAN_ONLY_OFFSET);
            }
            offsetSheetMetalFaces(context, id + "smOffset", mergeMaps(definition, { "moveFaces" : queries.sheetMetalQueries }));
            processSubfeatureStatus(context, id, { "subfeatureId" : id + "smOffset", "propagateErrorDisplay" : true });
        }
        if (nonSheetMetalQueryCount > 0)
        {
            if (definition.moveFaceType == MoveFaceType.OFFSET)
            {
                opOffsetFace(context, id, mergeMaps(definition, { "moveFaces" : queries.nonSheetMetalQueries }));
            }
            else
            {
            opMoveFace(context, id, definition);
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

const offsetSheetMetalFaces = defineSheetMetalFeature(function(context is Context, id is Id, definition)
    {
        var smEdgePartFacePairs = [];
        var alignedSMFaces = [];
        var antiAlignedSMFaces = [];
        var smEdges = [];
        var sheetMetalModels = [];
        for (var evaluatedFace in evaluateQuery(context, definition.moveFaces))
        {
            const smEntity = qUnion(getSMDefinitionEntities(context, evaluatedFace));
            const smFace = qEntityFilter(smEntity, EntityType.FACE);
            const smEdge = qEntityFilter(smEntity, EntityType.EDGE);
            if (size(evaluateQuery(context, smFace)) == 1)
            {
                const partFaceNormal = evPlane(context, { "face" : evaluatedFace }).normal;
                const smModelFaceNormal = evPlane(context, { "face" : smFace }).normal;
                if (dot(partFaceNormal, smModelFaceNormal) > 0)
                {
                    alignedSMFaces = append(alignedSMFaces, smFace);
                }
                else
                {
                    antiAlignedSMFaces = append(antiAlignedSMFaces, smFace);
                }
                sheetMetalModels = append(sheetMetalModels, qOwnerBody(smFace));
            }
            else if (size(evaluateQuery(context, smEdge)) == 1)
            {
                smEdgePartFacePairs = append(smEdgePartFacePairs, { "edge" : smEdge, "limitEntity" : evaluatedFace });
                sheetMetalModels = append(sheetMetalModels, qOwnerBody(smEdge));
                smEdges = append(smEdges, smEdge);
            }
        }
        sheetMetalModels = qUnion(evaluateQuery(context, qUnion(sheetMetalModels)));
        const modifiedFaces = qEdgeAdjacent(qUnion(concatenateArrays([smEdges, alignedSMFaces, antiAlignedSMFaces])), EntityType.FACE);

        const originalEntities = evaluateQuery(context, qOwnedByBody(sheetMetalModels));
        const initialAssociationAttributes = getAttributes(context, {
                    "entities" : qOwnedByBody(sheetMetalModels),
                    "attributePattern" : {} as SMAssociationAttribute });

        if (size(smEdgePartFacePairs) > 0)
        {
            opExtendSheetBody(context, id + "extend", { "entities" : qUnion(smEdges), "extendMethod" : ExtendSheetBoundingType.EXTEND_TO_SURFACE, "offset" : definition.offsetDistance, "edgeLimitOptions" : smEdgePartFacePairs });
        }
        if (size(alignedSMFaces) > 0)
        {
            opOffsetFace(context, id + "offset" + unstableIdComponent(1), { "moveFaces" : qUnion(alignedSMFaces), "offsetDistance" : definition.offsetDistance });
        }
        if (size(antiAlignedSMFaces) > 0)
        {
            opOffsetFace(context, id + "offset" + unstableIdComponent(2), { "moveFaces" : qUnion(antiAlignedSMFaces), "offsetDistance" : -definition.offsetDistance });
        }

        const toUpdate = assignSMAttributesToNewOrSplitEntities(context, sheetMetalModels,
                originalEntities, initialAssociationAttributes);

        updateSheetMetalGeometry(context, id + "smUpdate", {
                    "entities" : qUnion([toUpdate.modifiedEntities, modifiedFaces]),
                    "deletedAttributes" : toUpdate.deletedAttributes });
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

