export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/transform.fs", version : "");
export import(path : "onshape/std/print.fs", version : "");
export import(path : "onshape/std/errorstringenum.gen.fs", version : "");

export enum TransformType
{
    annotation {"Name" : "Translate by entity"}
    TRANSLATION_ENTITY,
    annotation {"Name" : "Translate by distance"}
    TRANSLATION_DISTANCE,
    annotation {"Name" : "Translate by XYZ"}
    TRANSLATION_3D,
    annotation {"Name" : "Rotate about entity"}
    ROTATION,
    annotation {"Name" : "Copy in place"}
    COPY
}


annotation {"Feature Type Name" : "Copy part"}
export function copyPart(context is Context, id is Id, definition is map)
precondition
{
    annotation {"Name" : "Entities to copy", "Filter" : EntityType.BODY}
    definition.entities is Query;
}
{
    startFeature(context, id, definition);
    var transform = identityTransform();
    opPattern(context, id, {"entities" : definition.entities, "transforms" : [transform], "instanceNames" : ["1"],
                            notFoundErrorKey("entities") :  ErrorStringEnum.COPY_SELECT_PARTS });
    endFeature(context, id);
}

// Note that transform() is also defined in transform.fs
// with different signatures.

annotation {"Feature Type Name" : "Transform"}
export function transform(context is Context, id is Id, definition is map)
precondition
{
    annotation {"Name" : "Parts to transform or copy",
                "Filter" : EntityType.BODY}
    definition.entities is Query;

    annotation {"Name" : "Transform type"}
    definition.transformType is TransformType;

    if(definition.transformType == TransformType.TRANSLATION_ENTITY)
    {
        annotation {"Name" : "Opposite direction", "UIHint" : "OppositeDirection"}
        definition.oppositeDirectionEntity is boolean;
    }

    if(definition.transformType == TransformType.TRANSLATION_ENTITY)
    {
        annotation {"Name" : "Transform definition",
                    "Filter": EntityType.VERTEX || EntityType.EDGE,
                    "MaxNumberOfPicks" : 2}
        definition.transformLine is Query;
    }
    else if(definition.transformType == TransformType.ROTATION)
    {
        annotation {"Name" : "Axis of rotation",
                    "Filter": QueryFilterCompound.ALLOWS_AXIS,
                    "MaxNumberOfPicks" : 1}
        definition.transformAxis is Query;
    }
    else if(definition.transformType == TransformType.TRANSLATION_DISTANCE)
    {
        annotation {"Name" : "Direction",
                    "Filter": QueryFilterCompound.ALLOWS_AXIS || GeometryType.PLANE || EntityType.VERTEX,
                    "MaxNumberOfPicks" : 2}
        definition.transformDirection is Query;
        annotation {"Name" : "Distance"}
        isLength(definition.distance, NONNEGATIVE_LENGTH_BOUNDS);
    }

    if(definition.transformType == TransformType.ROTATION)
    {
        annotation {"Name" : "Angle"}
        isAngle(definition.angle, ANGLE_360_BOUNDS);
    }

    if(definition.transformType == TransformType.ROTATION ||
       definition.transformType == TransformType.TRANSLATION_DISTANCE)
    {
        annotation {"Name" : "Opposite direction", "UIHint" : "OppositeDirection"}
        definition.oppositeDirection is boolean;
    }

    if(definition.transformType == TransformType.TRANSLATION_3D)
    {
        annotation {"Name": "X translation"}
        isLength(definition.dx, ZERO_DEFAULT_LENGTH_BOUNDS);
        annotation {"Name" : "Opposite direction",
                    "UIHint" : "OppositeDirection"}
        definition.oppositeX is boolean;
        annotation {"Name": "Y translation"}
        isLength(definition.dy, ZERO_DEFAULT_LENGTH_BOUNDS);
        annotation {"Name" : "Opposite direction",
                    "UIHint" : "OppositeDirection"}
        definition.oppositeY is boolean;
        annotation {"Name": "Z translation"}
        isLength(definition.dz, ZERO_DEFAULT_LENGTH_BOUNDS);
        annotation {"Name" : "Opposite direction",
                    "UIHint" : "OppositeDirection"}
        definition.oppositeZ is boolean;
    }

    if(definition.transformType != TransformType.COPY)
    {
        annotation {"Name" : "Copy part"}
        definition.makeCopy is boolean;
    }
}
//============================ Body =============================
{
    startFeature(context, id, definition);
    //Start by figuring out the transform
    var transform = identityTransform();
    var transformType = definition.transformType;

    if(transformType == TransformType.TRANSLATION_ENTITY ||
        transformType == TransformType.TRANSLATION_DISTANCE)
    {
        var selection = (transformType == TransformType.TRANSLATION_ENTITY ?
                         definition.transformLine : definition.transformDirection);
        //For a translation / translation by distance, figure out which entities are used to specify it
        var distanceSpecified = transformType == TransformType.TRANSLATION_DISTANCE;
        var translation;
        var vertices = evaluateQuery(context, qEntityFilter(selection, EntityType.VERTEX));
        var edges = evaluateQuery(context, qEntityFilter(selection, EntityType.EDGE));
        var faces = evaluateQuery(context, qEntityFilter(selection, EntityType.FACE));

        if(@size(vertices) >= 2)
        {
            var v0 = evVertexPoint(context, { "vertex" : vertices[0] }).result;
            var v1 = evVertexPoint(context, { "vertex" : vertices[1] }).result;
            translation = v1 - v0;
        }
        else if(@size(edges) >= 1)
        {
            var evalResult = evEdgeTangentLines(context, { "edge" : edges[0], "parameters" : [0, 1] });
            if(reportFeatureError(context, id, evalResult.error))
                return;
            translation = evalResult.result[1].origin - evalResult.result[0].origin;
        }
        else if(distanceSpecified && @size(faces) >= 1) // A plane only provides direction
        {
            var planeResult = evPlane(context, { "face" : faces[0] });
            var axisResult = evAxis(context, { "axis" : faces[0] });
            if (planeResult.result is Plane)
                translation = planeResult.result.normal * meter;
            else if (axisResult.result is Line)
                translation = axisResult.result.direction * meter;
            else if(reportFeatureError(context, id, planeResult.error))
                return;
            else if(reportFeatureError(context, id, axisResult.error))
                return;
            /* else "can't happen" and norm(undefined) will fail later */
        }
        else
        {
            if(distanceSpecified)
                reportFeatureError(context, id, ErrorStringEnum.TRANSFORM_TRANSLATE_BY_DISTANCE_INPUT);
            else
                reportFeatureError(context, id, ErrorStringEnum.TRANSFORM_TRANSLATE_INPUT);
            return;
        }

        if(distanceSpecified)
        {
            if(norm(translation).value < TOLERANCE.zeroLength)
            {
                reportFeatureError(context, id, ErrorStringEnum.NO_TRANSLATION_DIRECTION);
                return;
            }
            translation = normalize(translation) * definition.distance;
        }
        transform = transform(translation);
    }

    if(transformType == TransformType.ROTATION)
    {
        var axisResult = evAxis(context, { "axis" : definition.transformAxis });
        if(reportFeatureError(context, id, axisResult.error))
            return;
        var angle = definition.angle;
        transform = rotationAround(axisResult.result, angle);
    }

    if(transformType == TransformType.TRANSLATION_3D)
    {
        var dx = definition.oppositeX ? -definition.dx : definition.dx;
        var dy = definition.oppositeY ? -definition.dy : definition.dy;
        var dz = definition.oppositeZ ? -definition.dz : definition.dz;
        transform = transform(vector(dx, dy, dz));
    }

    var oppositeDirection = false;
    if(transformType == TransformType.TRANSLATION_ENTITY)
    {
        oppositeDirection = definition.oppositeDirectionEntity;
    }
    else if(transformType != TransformType.TRANSLATION_3D)
    {
        oppositeDirection = definition.oppositeDirection;
    }

    if(oppositeDirection == true)
        transform = inverse(transform);

    if(definition.makeCopy || transformType == TransformType.COPY)
    {
        opPattern(context, id,
                  { "entities" : definition.entities,
                    "transforms" : [transform] ,
                    "instanceNames" : ["1"]});
    }
    else
    {
        opTransform(context, id + "transform",
                    { "bodies" : definition.entities,
                      "transform" : transform});
    }

    endFeature(context, id);
}

