export import(path: "onshape/std/geomUtils.fs", version : "");
export import(path: "onshape/std/evaluate.fs", version : "");
export import(path:"onshape/std/transform.fs", version : "");
export import(path:"onshape/std/print.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");
export import(path : "onshape/std/manipulator.fs", version : "");
export import(path : "onshape/std/errorstringenum.gen.fs", version : "");

export const MOVE_FACE_OFFSET_BOUNDS = NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS;
export const MOVE_FACE_TRANSLATE_BOUNDS = NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS;
export const MOVE_FACE_ROTATION_BOUNDS = ANGLE_360_ZERO_DEFAULT_BOUNDS;

export enum MoveFaceType
{
    annotation {"Name" : "Translate"}
    TRANSLATE,
    annotation {"Name" : "Rotate"}
    ROTATE,
    annotation {"Name" : "Offset"}
    OFFSET
}

annotation {"Feature Type Name" : "Move face", "Manipulator Change Function" : "moveFaceManipulatorChange" }
export function moveFace(context is Context, id is Id, definition is map)
precondition
{
    annotation {"Name" : "Faces",
                "UIHint" : "ShowCreateSelection",
                "Filter": EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
    definition.moveFaces is Query;

    annotation {"Name" : "Move Face Type"}
    definition.moveFaceType is MoveFaceType;

    if(definition.moveFaceType == MoveFaceType.TRANSLATE)
    {
        annotation {"Name" : "Direction",
                "Filter" : QueryFilterCompound.ALLOWS_AXIS || GeometryType.PLANE,
                "MaxNumberOfPicks" : 1}
        definition.direction is Query;

        annotation {"Name" : "Distance"}
        isLength(definition.translationDistance, MOVE_FACE_TRANSLATE_BOUNDS);
    }

    if(definition.moveFaceType == MoveFaceType.ROTATE)
    {
        annotation {"Name" : "Axis",
                "Filter" : QueryFilterCompound.ALLOWS_AXIS,
                "MaxNumberOfPicks" : 1}
        definition.axis is Query;
        annotation {"Name" : "Rotation Angle"}
        isAngle(definition.angle, MOVE_FACE_ROTATION_BOUNDS);
    }

    if(definition.moveFaceType == MoveFaceType.OFFSET)
    {
        annotation {"Name" : "Distance"}
        isLength(definition.offsetDistance, MOVE_FACE_OFFSET_BOUNDS);
    }

    if(definition.oppositeDirection != undefined)
    {
        annotation {"Name" : "Opposite direction", "UIHint" : "OppositeDirection"}
        definition.oppositeDirection is boolean;
    }

    if(definition.reFillet != undefined)
    {
        annotation {"Name" : "Reapply fillet", "Default" : true}
        definition.reFillet is boolean;
    }

}
//============================ Body =============================
{
    startFeature(context, id, definition);
    var directionSign = 1;
    if(definition.oppositeDirection == true)
        directionSign = -1;

    // Extract an axis defined by the moved face for use in the manipulators.
    var resolvedEntities = evaluateQuery(context, definition.moveFaces);
    var facePlane = undefined;
    if(@size(resolvedEntities) > 0)
    {
        facePlane = computeFacePlane(context, id, resolvedEntities[0]);
        if(facePlane == undefined)
        {
            reportFeatureError(context, id, ErrorStringEnum.NO_TANGENT_PLANE);
            return;
        }
    }

    if(definition.moveFaceType == MoveFaceType.OFFSET)
    {
        definition.offsetDistance = definition.offsetDistance * directionSign;

        if(facePlane != undefined)
        {
            addOffsetManipulator(context, id, definition, facePlane);
        }

        opOffsetFace(context, id, definition);
    }
    else
    {
        var transform = identityTransform();
        if(definition.moveFaceType == MoveFaceType.TRANSLATE)
        {
            // If the user specified an axis for the direction, we will use that for the translation.  If they,
            // specified a face, we will use the face's normal, if it is planar.
            var translation;
            var directionResult = evAxis(context, {"axis" : definition.direction});
            var translationDirection;
            if(directionResult.error != undefined)
            {
                var planeResult = evPlane(context, {"face" : definition.direction});
                if(planeResult.error != undefined)
                {
                    reportFeatureError(context, id, ErrorStringEnum.NO_TRANSLATION_DIRECTION);
                    return;
                }
                translation = planeResult.result.normal * definition.translationDistance * directionSign;
                translationDirection = planeResult.result.normal;
            }
            else
            {
                translation = directionResult.result.direction * definition.translationDistance * directionSign;
                translationDirection = directionResult.result.direction;
            }

            if(facePlane != undefined)
            {
                addTranslateManipulator(context, id, facePlane.origin, translationDirection, definition.translationDistance * directionSign);
            }

            definition.transform = transform(translation);
        }
        if(definition.moveFaceType == MoveFaceType.ROTATE)
        {
            var axisResult = evAxis(context, { "axis" : definition.axis });
            if(reportFeatureError(context, id, axisResult.error))
                return;

            if(facePlane != undefined)
            {
                addRotateManipulator(context, id, axisResult.result, facePlane, definition.angle * directionSign);
            }

            definition.transform = rotationAround(axisResult.result, definition.angle * directionSign);
        }
        opMoveFace(context, id, definition);
    }
    endFeature(context, id);
}


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

function addRotateManipulator(context is Context, id is Id, axis is Line, facePlane is Plane, angle is ValueWithUnits)
{
    // Project the center of the plane onto the axis
    var rotateOrigin = axis.origin + dotProduct(facePlane.origin - axis.origin, axis.direction) * axis.direction;
    if (samePoint(rotateOrigin, facePlane.origin))
    {
        return;
    }

    addManipulators(context, id, { (ROTATE_MANIPULATOR) :
                                   angularManipulator(rotateOrigin, axis.direction, facePlane.origin, angle) });
}

function computeFacePlane(context is Context, id is Id, faceQuery is Query)
{
    var plane = evFaceTangentPlane(context, {"face" : faceQuery, "parameter" : vector(0.5, 0.5)});
    if(reportFeatureError(context, id, plane.error))
       return undefined;

    return plane.result;
}

export function moveFaceManipulatorChange(context is Context, moveFaceDefinition is map, newManipulators is map) returns map
precondition
{
}
{
    var newValue = 0 * meter;
    if(moveFaceDefinition.moveFaceType == MoveFaceType.OFFSET) {
        newValue = newManipulators[OFFSET_MANIPULATOR].offset;
        moveFaceDefinition.offsetDistance = abs(newValue);
    }
    else if(moveFaceDefinition.moveFaceType == MoveFaceType.TRANSLATE) {
        newValue = newManipulators[TRANSLATE_MANIPULATOR].offset;
        moveFaceDefinition.translationDistance = abs(newValue);
    }
    else if(moveFaceDefinition.moveFaceType == MoveFaceType.ROTATE) {
        newValue = newManipulators[ROTATE_MANIPULATOR].angle;
        moveFaceDefinition.angle = abs(newValue);
    }

    moveFaceDefinition.oppositeDirection = newValue < 0 * meter;

    return moveFaceDefinition;
}
