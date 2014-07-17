export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/transform.fs", version : "");
export import(path : "onshape/std/print.fs", version : "");
export import(path : "onshape/std/errorstringenum.gen.fs", version : "");

export enum TransformType
{
    annotation {"Name" : "Translation"}
    TRANSLATION,
    annotation {"Name" : "Translation by Distance"}
    TRANSLATION_DISTANCE,
    annotation {"Name" : "Rotation"}
    ROTATION,
    annotation {"Name" : "Mirror"}
    MIRROR
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

annotation {"Feature Type Name" : "Transform / Copy"}
export function transformCopy(context is Context, id is Id, definition is map)
precondition
{
    annotation {"Name" : "Entities to transform",
            "Filter" : EntityType.BODY}
    definition.entities is Query;

    annotation {"Name" : "Make a copy"}
    definition.makeCopy is boolean;

    annotation {"Name" : "Transform type"}
    definition.transformType is TransformType;

    if(definition.transformType != TransformType.MIRROR)
    {
        if(definition.oppositeDirection != undefined)
        {
            annotation {"Name" : "Opposite direction", "UIHint" : "OppositeDirection"}
            definition.oppositeDirection is boolean;
        }
    }

    annotation {"Name" : "Transform definition",
                "Filter": QueryFilterCompound.ALLOWS_AXIS ||  GeometryType.PLANE || EntityType.VERTEX,
                "MaxNumberOfPicks" : 2}
    definition.transformDefinition is Query;

    if(definition.transformType == TransformType.TRANSLATION_DISTANCE)
    {
        annotation {"Name" : "Distance"}
        isLength(definition.distance, BLEND_BOUNDS);
    }

    if(definition.transformType == TransformType.ROTATION)
    {
        annotation {"Name" : "Angle"}
        isAngle(definition.angle, ANGLE_360_BOUNDS);
    }
}
//============================ Body =============================
{
    startFeature(context, id, definition);
    //Start by figuring out the transform
    var transform = identityTransform();

    if(definition.transformType == TransformType.TRANSLATION ||
        definition.transformType == TransformType.TRANSLATION_DISTANCE)
    {
        //For a translation / translation by distance, figure out which entities are used to specify it
        var distanceSpecified = definition.transformType == TransformType.TRANSLATION_DISTANCE;
        var translation;
        var vertices = evaluateQuery(context, qEntityFilter(definition.transformDefinition, EntityType.VERTEX));
        var edges = evaluateQuery(context, qEntityFilter(definition.transformDefinition, EntityType.EDGE));
        var faces = evaluateQuery(context, qEntityFilter(definition.transformDefinition, EntityType.FACE));

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
            if(reportFeatureError(context, id, planeResult.error))
                return;
            translation = planeResult.result.normal * meter;
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

    if(definition.transformType == TransformType.ROTATION)
    {
        var axisResult = evAxis(context, { "axis" : definition.transformDefinition });
        if(reportFeatureError(context, id, axisResult.error))
            return;
        transform = rotationAround(axisResult.result, definition.angle);
    }
    if(definition.transformType == TransformType.MIRROR)
    {
        var planeResult = evPlane(context, { "face" : definition.transformDefinition });
        if(reportFeatureError(context, id, planeResult.error))
            return;
        transform = mirrorAcross(planeResult.result);
    }

    if(definition.oppositeDirection == true)
        transform = inverse(transform);

    if(!definition.makeCopy)
        opTransform(context, id + "transform", { "bodies" : definition.entities, "transform" : transform});
    else
        opPattern(context, id, { "entities" : definition.entities, "transforms" : [transform] , "instanceNames" : ["1"]});
    endFeature(context, id);
}

