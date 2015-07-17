FeatureScript 172; /* Automatically generated version */
export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/transform.fs", version : "");
export import(path : "onshape/std/manipulator.fs", version : "");
export import(path : "onshape/std/print.fs", version : "");

export enum TransformType
{
    annotation { "Name" : "Translate by line" }
    TRANSLATION_ENTITY,
    annotation { "Name" : "Translate by distance" }
    TRANSLATION_DISTANCE,
    annotation { "Name" : "Translate by XYZ" }
    TRANSLATION_3D,
    annotation { "Name" : "Rotate" }
    ROTATION,
    annotation { "Name" : "Copy in place" }
    COPY,
    annotation { "Name" : "Scale uniformly" }
    SCALE_UNIFORMLY
}

/* Manipulator names */
const ROTATE = "rotate";
const OFFSET_LINE = "offsetLine";
const TRANSLATION = "translation";

/* Reduce from [-2 pi, +2 pi] to [-pi, +pi] radians */
function reduceAngle(angle is ValueWithUnits) returns ValueWithUnits
precondition
{
    isAngle(angle);
}
{
    const CIRCLE = 2 * PI * radian;
    const REDUCE_ADD = 3 * PI * radian;
    const REDUCE_SUB = PI * radian;
    return (angle + REDUCE_ADD) % CIRCLE - REDUCE_SUB;
}

/* This feature is deprecated but still used by the UI.  See BEL-15681. */
annotation { "Feature Type Name" : "Copy part", "Filter Selector" : "allparts" }
export const copyPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities to copy", "Filter" : EntityType.BODY }
        definition.entities is Query;
    }
    {
        var transform = identityTransform();
        opPattern(context, id, { "entities" : definition.entities,
                                 "transforms" : [transform],
                                 "instanceNames" : ["1"],
                                 notFoundErrorKey("entities") : ErrorStringEnum.COPY_SELECT_PARTS });
    });

/* Find a point to attach the manipulator bar.
   Returns Vector or undefined.  */
function findCenter(context is Context, id is Id, entities is Query)
{
    var boxResult = evBox3d(context, { topology : entities });
    if (boxResult.result is Box3d)
        return (boxResult.result.minCorner + boxResult.result.maxCorner) / 2;
    reportFeatureError(context, id, ErrorStringEnum.REGEN_ERROR);
    return undefined;
}

function reportCoincident(context is Context, id is Id, distance is Vector) returns boolean
{
    if (norm(distance).value > TOLERANCE.zeroLength)
        return false;
    reportFeatureError(context, id, ErrorStringEnum.POINTS_COINCIDENT);
    return true;
}

// Note that transform() is also defined in transform.fs
// with different signatures.  This is written as a wrapper around defineFeature to keep overloads working.
annotation { "Feature Type Name" : "Transform",
             "Manipulator Change Function" : "transformManipulatorChange",
             "Filter Selector" : "allparts" }
export function transform(context is Context, id is Id, definition is map)
precondition
{
    annotation { "Name" : "Parts to transform or copy",
                 "Filter" : EntityType.BODY }
    definition.entities is Query;

    annotation { "Name" : "Transform type" }
    definition.transformType is TransformType;

    if (definition.transformType == TransformType.TRANSLATION_ENTITY)
    {
        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirectionEntity is boolean;
    }

    if (definition.transformType == TransformType.TRANSLATION_ENTITY)
    {
        annotation { "Name" : "Line or points",
                     "Filter" : EntityType.VERTEX || EntityType.EDGE,
                     "MaxNumberOfPicks" : 2 }
        definition.transformLine is Query;
    }
    else if (definition.transformType == TransformType.ROTATION)
    {
        annotation { "Name" : "Axis",
                     "Filter" : QueryFilterCompound.ALLOWS_AXIS,
                     "MaxNumberOfPicks" : 1 }
        definition.transformAxis is Query;
    }
    else if (definition.transformType == TransformType.TRANSLATION_DISTANCE)
    {
        annotation { "Name" : "Direction",
                     "Filter" : QueryFilterCompound.ALLOWS_AXIS || GeometryType.PLANE || EntityType.VERTEX,
                     "MaxNumberOfPicks" : 2 }
        definition.transformDirection is Query;
        annotation { "Name" : "Distance" }
        isLength(definition.distance, NONNEGATIVE_LENGTH_BOUNDS);
    }
    else if (definition.transformType == TransformType.SCALE_UNIFORMLY)
    {
        annotation { "Name" : "Scale" }
        isReal(definition.scale, SCALE_BOUNDS);
    }

    if (definition.transformType == TransformType.ROTATION)
    {
        annotation { "Name" : "Angle" }
        isAngle(definition.angle, ANGLE_360_BOUNDS);
    }

    if (definition.transformType == TransformType.ROTATION ||
        definition.transformType == TransformType.TRANSLATION_DISTANCE)
    {
        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;
    }

    if (definition.transformType == TransformType.TRANSLATION_3D)
    {
        annotation { "Name" : "X translation" }
        isLength(definition.dx, ZERO_DEFAULT_LENGTH_BOUNDS);

        annotation { "Name" : "Y translation" }
        isLength(definition.dy, ZERO_DEFAULT_LENGTH_BOUNDS);

        annotation { "Name" : "Z translation" }
        isLength(definition.dz, ZERO_DEFAULT_LENGTH_BOUNDS);
    }

    if (definition.transformType != TransformType.COPY)
    {
        annotation { "Name" : "Copy part" }
        definition.makeCopy is boolean;
    }

    if (definition.transformType == TransformType.SCALE_UNIFORMLY)
    {
        annotation { "Name" : "Point",
                     "Filter" : EntityType.VERTEX,
                     "MaxNumberOfPicks" : 1 }
        definition.scalePoint is Query;
    }
}
{
    fTransform(context, id, definition);
}

const fTransform = defineFeature(function(context is Context, id is Id, definition is map)
    {
        //Start by figuring out the transform
        var transformMatrix = identityTransform();
        var transformType = definition.transformType;

        /* Prior to V74
           1. Transform of no part was not an error (bug compatibility)
           2. Null transform was not an error (behavior change)
         */
        var validateInputs = isAtVersionOrLater(context, FeatureScriptVersionNumber.V74_TRANSFORM_CHECKING);

        if (validateInputs && size(evaluateQuery(context, definition.entities)) == 0)
        {
            reportFeatureError(context, id, ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["entities"]);
            return;
        }

        if (transformType == TransformType.TRANSLATION_ENTITY ||
            transformType == TransformType.TRANSLATION_DISTANCE)
        {
            var selection = (transformType == TransformType.TRANSLATION_ENTITY ?
                definition.transformLine : definition.transformDirection);
            //For a translation / translation by distance, figure out which entities are used to specify it
            var distanceSpecified = transformType == TransformType.TRANSLATION_DISTANCE;
            var translation;
            var vertices = evaluateQuery(context, qEntityFilter(selection, EntityType.VERTEX));
            var edges = evaluateQuery(context, qEntityFilter(selection, EntityType.EDGE));
            var nedges = @size(edges);
            var faces = evaluateQuery(context, qEntityFilter(selection, EntityType.FACE));

            if (@size(vertices) >= 2)
            {
                var v0 = evVertexPoint(context, { "vertex" : vertices[0] }).result;
                var v1 = evVertexPoint(context, { "vertex" : vertices[1] }).result;
                translation = v1 - v0;
                if (validateInputs && reportCoincident(context, id, translation))
                    return;
            }
            else if (validateInputs && nedges > 1)
            {
                reportFeatureError(context, id, ErrorStringEnum.TOO_MANY_ENTITIES_SELECTED,
                        [distanceSpecified ? "transformDirection" : "transformLine"]);
                return;
            }
            else if (nedges >= 1)
            {
                var evalResult = evEdgeTangentLines(context, { "edge" : edges[0], "parameters" : [0, 1] });
                if (reportFeatureError(context, id, evalResult.error))
                    return;
                translation = evalResult.result[1].origin - evalResult.result[0].origin;
                if (validateInputs && reportCoincident(context, id, translation))
                    return;
            }
            else if (distanceSpecified && @size(faces) >= 1) // A plane only provides direction
            {
                var planeResult = evPlane(context, { "face" : faces[0] });
                var axisResult = evAxis(context, { "axis" : faces[0] });
                if (planeResult.result is Plane)
                    translation = planeResult.result.normal * meter;
                else if (axisResult.result is Line)
                    translation = axisResult.result.direction * meter;
                else if (reportFeatureError(context, id, planeResult.error, ["transformDirection"]))
                    return;
                else if (reportFeatureError(context, id, axisResult.error, ["transformDirection"]))
                    return;
                /* else "can't happen" and norm(undefined) will fail later */
            }
            else
            {
                if (distanceSpecified)
                    reportFeatureError(context, id, ErrorStringEnum.TRANSFORM_TRANSLATE_BY_DISTANCE_INPUT, ["transformDirection"]);
                else
                    reportFeatureError(context, id, ErrorStringEnum.TRANSFORM_TRANSLATE_INPUT, ["transformLine"]);
                return;
            }

            if (distanceSpecified)
            {
                if (norm(translation).value < TOLERANCE.zeroLength)
                {
                    reportFeatureError(context, id, ErrorStringEnum.NO_TRANSLATION_DIRECTION, ["transformDirection"]);
                    return;
                }
                var target = definition.entities;
                var origin = findCenter(context, id, target);
                if (origin is undefined)
                    return;
                var direction = normalize(translation);
                var distance = definition.distance;
                if (definition.oppositeDirection)
                    distance = -distance;
                addManipulators(context, id, {
                            (OFFSET_LINE) : linearManipulator(origin, direction, distance, target) });
                translation = direction * definition.distance;
            }
            transformMatrix = transform(translation);
        }

        if (transformType == TransformType.ROTATION)
        {
            var axisResult = evAxis(context, { "axis" : definition.transformAxis });
            if (reportFeatureError(context, id, axisResult.error, ["transformAxis"]))
                return;
            var target = definition.entities;
            var origin = findCenter(context, id, target);
            if (origin is undefined)
                return;
            var angle = reduceAngle(definition.angle);
            if (definition.oppositeDirection)
                angle = -angle;
            var axis = axisResult.result;
            addManipulators(context, id,
                    { (ROTATE) :
                      angularManipulator({ "axisOrigin" : project(axis, origin),
                                           "axisDirection" : axis.direction,
                                           "rotationOrigin" : origin,
                                           "angle" : angle,
                                           "sources" : target }) });
            transformMatrix = rotationAround(axis, angle);
        }

        if (transformType == TransformType.TRANSLATION_3D)
        {
            var dx = definition.dx;
            var dy = definition.dy;
            var dz = definition.dz;
            var transformVector = vector(dx, dy, dz);
            transformMatrix = transform(transformVector);
            var target = definition.entities;
            var origin = findCenter(context, id, target);
            if (origin is undefined)
                return;
            addManipulators(context, id, { (TRANSLATION) : triadManipulator(origin, transformVector, target) });
        }

        if (transformType == TransformType.SCALE_UNIFORMLY)
        {

            var matrix = identityMatrix(3) * definition.scale;
            var target = definition.entities;

            var centerPointResult = evVertexPoint(context, { "vertex" : definition.scalePoint });
            if (reportFeatureError(context, id, centerPointResult.error, ["scalePoint"]))
                return;

            var centerPoint = centerPointResult.result;
            transformMatrix = transform(matrix, centerPoint - matrix * centerPoint);
        }
        /* Reversal for rotation and 3D translation is handled above.
           Reversal is not applicable to copy. */
        if ((transformType == TransformType.TRANSLATION_ENTITY &&
             definition.oppositeDirectionEntity) ||
            (transformType == TransformType.TRANSLATION_DISTANCE &&
             definition.oppositeDirection))
            transformMatrix = inverse(transformMatrix);

        if (definition.makeCopy || transformType == TransformType.COPY)
        {
            opPattern(context, id,
                      { "entities" : definition.entities,
                        "transforms" : [transformMatrix],
                        "instanceNames" : ["1"] });
        }
        else
        {
            var subId = validateInputs ? id : id + "transform";
            opTransform(context, subId,
                        { "bodies" : definition.entities,
                          "transform" : transformMatrix });
        }
    }, { oppositeDirection : false, scale : 1.0 });

function extractOffset(input is map, axis is string)
{
    var submap = input[axis];
    if (submap is undefined)
        return undefined;
    return submap.offset;
}

export function transformManipulatorChange(context is Context, output is map, input is map) returns map
{
    if (input[ROTATE] is map)
    {
        var angle = reduceAngle(input[ROTATE].angle);
        output.angle = abs(angle);
        output.oppositeDirection = angle < 0 * radian;
    }
    var distance = extractOffset(input, OFFSET_LINE);
    if (distance is ValueWithUnits)
    {
        output.distance = abs(distance);
        output.oppositeDirection = distance.value < 0;
    }
    if (input[TRANSLATION] is map)
    {
        var offset = input[TRANSLATION].offset;
        output.dx = offset[0];
        output.dy = offset[1];
        output.dz = offset[2];
    }
    return output;
}

