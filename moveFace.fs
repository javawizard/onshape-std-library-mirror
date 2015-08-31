FeatureScript 213; /* Automatically generated version */
export import(path : "onshape/std/geomOperations.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/transform.fs", version : "");
export import(path : "onshape/std/print.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");
export import(path : "onshape/std/manipulator.fs", version : "");
export import(path : "onshape/std/box.fs", version : "");

export const MOVE_FACE_OFFSET_BOUNDS = NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS;
export const MOVE_FACE_TRANSLATE_BOUNDS = NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS;
export const MOVE_FACE_ROTATION_BOUNDS = ANGLE_360_ZERO_DEFAULT_BOUNDS;

annotation { "Feature Type Name" : "Move face",
             "Manipulator Change Function" : "moveFaceManipulatorChange",
             "Filter Selector" : "allparts" }
export const moveFace = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Faces",
                     "UIHint" : "SHOW_CREATE_SELECTION",
                     "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
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
        var resolvedEntities = evaluateQuery(context, definition.moveFaces);
        if (size(resolvedEntities) == 0)
            throw regenError(ErrorStringEnum.DIRECT_EDIT_MOVE_FACE_SELECT, ["moveFaces"]);

        var directionSign = 1;
        if (definition.oppositeDirection)
            directionSign = -1;

        // Extract an axis defined by the moved face for use in the manipulators.
        var facePlane = try(evFaceTangentPlane(context, { "face" : resolvedEntities[0], "parameter" : vector(0.5, 0.5) }));
        if (facePlane == undefined)
            throw regenError(ErrorStringEnum.NO_TANGENT_PLANE, ["moveFaces"]);

        if (definition.moveFaceType == MoveFaceType.OFFSET)
        {
            definition.offsetDistance = definition.offsetDistance * directionSign;

            addOffsetManipulator(context, id, definition, facePlane);

            opOffsetFace(context, id, definition);
        }
        else
        {
            if (definition.moveFaceType == MoveFaceType.TRANSLATE)
            {
                // If the user specified an axis for the direction, we will use that for the translation.  If they,
                // specified a face, we will use the face's normal, if it is planar.
                var translation;
                var directionResult = try(evAxis(context, { "axis" : definition.direction }));
                var translationDirection;
                if (directionResult == undefined)
                {
                    var planeResult = try(evPlane(context, { "face" : definition.direction }));
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
                var axisResult = evAxis(context, { "axis" : definition.axis });

                addRotateManipulator(context, id, axisResult, facePlane, definition.angle * directionSign, definition.moveFaces);

                definition.transform = rotationAround(axisResult, definition.angle * directionSign);
            }
            opMoveFace(context, id, definition);
        }
    }, { oppositeDirection : false, reFillet : false });


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
    var rotateOrigin = axis.origin + dot(refPoint - axis.origin, axis.direction) * axis.direction;
    if (samePoint(rotateOrigin, refPoint))
    {
        // refPoint lies on the axis, so construct a different refPoint
        var orthoVec = cross(axis.direction, facePlane.normal);
        var orthoVecNorm = norm(orthoVec);
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
        var faceBox = try(evBox3d(context, { topology : qNthElement(faceQuery, 0) }));
        var manipulatorRadius = 0.001 * meter; // default of 1 mm if we fail to get the box
        if (faceBox != undefined)
            manipulatorRadius = norm(faceBox.maxCorner - faceBox.minCorner) * 0.5;
        refPoint = rotateOrigin + orthoVec * manipulatorRadius;
    }

    addManipulators(context, id, { (ROTATE_MANIPULATOR) : angularManipulator({ "axisOrigin" : rotateOrigin,
                                   "axisDirection" : axis.direction,
                                   "rotationOrigin" : refPoint,
                                   "angle" : angle }) });
}

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

