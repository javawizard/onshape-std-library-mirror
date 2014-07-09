export import(path: "onshape/std/geomUtils.fs", version : "");
export import(path: "onshape/std/boolean.fs", version : "");
export import(path: "onshape/std/evaluate.fs", version : "");
export import(path: "onshape/std/transform.fs", version : "");
export import(path: "onshape/std/print.fs", version : "");
export import(path: "onshape/std/errorstringenum.gen.fs", version : "");

export const PATTERN_OFFSET_BOUND = NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS;


function computePatternOffset(context is Context, entity is Query, oppositeDir is boolean, distance is ValueWithUnits) returns map
{
    if(oppositeDir)
        distance = -distance;

    var rawDirectionResult = evAxis(context, {"axis" : entity});
    if(rawDirectionResult.error != undefined)
    {
        var rawPlaneResult = evPlane(context, {"face" : entity});
        if(rawPlaneResult.error != undefined)
            return rawPlaneResult;

        return {"offset" : rawPlaneResult.result.normal * distance};
    }
    else
        return {"offset" : rawDirectionResult.result.direction * distance};
}

function verifyPatternSize(context is Context, id is Id, instances is number) returns boolean
{
    if(instances <= 2500)
        return false; //Fine
    reportFeatureError(context, id, ErrorStringEnum.PATTERN_INPUT_TOO_MANY_INSTANCES);
    return true;
}

//LinearPattern Feature
annotation {"Feature Type Name" : "Linear pattern"}
export function linearPattern(context is Context, id is Id, patternDefinition is map)
precondition
{
    if ( patternDefinition.isFacePattern != undefined )
    {
        annotation {"Name" : "Face pattern", "Default" : false}
        patternDefinition.isFacePattern is boolean;
    }

    if ( patternDefinition.isFacePattern == false)
    {
        annotation {"Name" : "Entities to pattern",
        "Filter" : EntityType.BODY }
        patternDefinition.entities is Query;
    }
    else
    {
        annotation {"Name" : "Faces to pattern", "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
        patternDefinition.faces is Query;
    }

    annotation {"Name" : "Direction",
                "Filter" : QueryFilterCompound.ALLOWS_AXIS || GeometryType.PLANE,
                "MaxNumberOfPicks" : 1}
    patternDefinition.directionOne is Query;
    annotation {"Name" : "Distance"}
    isLength(patternDefinition.distance, PATTERN_OFFSET_BOUND);
    annotation {"Name" : "Instance count"}
    isInteger(patternDefinition.instanceCount, POSITIVE_COUNT_BOUNDS);
    annotation {"Name" : "Opposite direction", "UIHint" : "OppositeDirection"}
    patternDefinition.oppositeDirection is boolean;

    if (patternDefinition.hasSecondDir != undefined)
    {
        annotation {"Name" : "Second direction"}
        patternDefinition.hasSecondDir is boolean;
    }
    if(patternDefinition.hasSecondDir == true)
    {
        annotation {"Name" : "Direction2",
                    "Filter" : QueryFilterCompound.ALLOWS_AXIS || GeometryType.PLANE,
                    "MaxNumberOfPicks" : 1}
        patternDefinition.directionTwo is Query;
        annotation {"Name" : "Distance2"}
        isLength(patternDefinition.distanceTwo, PATTERN_OFFSET_BOUND);
        annotation {"Name" : "Instance count2"}
        isInteger(patternDefinition.instanceCountTwo, POSITIVE_COUNT_BOUNDS);
        annotation {"Name" : "Opposite direction2", "UIHint" : "OppositeDirection"}
        patternDefinition.oppositeDirectionTwo is boolean;
    }

}
{
    startFeature(context, id, patternDefinition);

    // Compute a vector of transforms
    var transforms = [];
    var instanceNames = [];

    //Dir 1
    var result = computePatternOffset(context, patternDefinition.directionOne,
        patternDefinition.oppositeDirection, patternDefinition.distance);
    if(reportFeatureError(context, id, result.error))
        return;

    var offset1 = result.offset;
    var count1 = patternDefinition.instanceCount;

    //Dir2, if any
    var offset2 = zeroVector(3) * meter;
    var count2 = 1;
    if(patternDefinition.hasSecondDir == true)
    {
        var result = computePatternOffset(context, patternDefinition.directionTwo,
            patternDefinition.oppositeDirectionTwo, patternDefinition.distanceTwo);
        if(reportFeatureError(context, id, result.error))
            return;
        offset2 = result.offset;
        if(parallelVectors(offset1, offset2))
        {   //notify user that parallel directions are selected for dir1 and dir2
            reportFeatureInfo(context, id, ErrorStringEnum.PATTERN_DIRECTIONS_PARALLEL);
        }
        count2 = patternDefinition.instanceCountTwo;
    }

    if(verifyPatternSize(context, id, count1 * count2))
        return;

    //Create the transforms and instance names, create along the first direction first then the second direction
    for(var j = 0; j < count2; j += 1)
    {
        for(var i = 0; i < count1; i += 1)
        {
            if ( j == 0 && i == 0 ) //skip recreating original
                continue;

            transforms = append(transforms, transform(identityMatrix(3), offset1 * i + offset2 * j));
            var instName = "" ~ i;
            if(j > 0)
            {
                instName ~= "_" ~ j;
            }
            instanceNames = append(instanceNames, instName);
        }
    }

    if (patternDefinition.isFacePattern == true)
    {
        opPattern(context, id, { "entities" : patternDefinition.faces,"transforms" : transforms , "instanceNames" : instanceNames  });
    }
    else
    {
        opPattern(context, id, { "entities" : patternDefinition.entities, "transforms" : transforms , "instanceNames" : instanceNames});
    }
    endFeature(context, id);
}

//======================================================================================
//CircularPattern Feature
annotation {"Feature Type Name" : "Circular pattern"}
export function circularPattern(context is Context, id is Id, patternDefinition is map)
precondition
{
    if ( patternDefinition.isFacePattern != undefined )
    {
        annotation {"Name" : "Face pattern", "Default" : false}
        patternDefinition.isFacePattern is boolean;
    }


    if ( patternDefinition.isFacePattern == false)
    {
        annotation {"Name" : "Entities to pattern",
                    "Filter" : EntityType.BODY}
        patternDefinition.entities is Query;
    }
    else
    {
        annotation {"Name" : "Faces to pattern",
                    "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO}
        patternDefinition.faces is Query;
    }

    annotation {"Name" : "Axis of pattern", "Filter" : QueryFilterCompound.ALLOWS_AXIS, "MaxNumberOfPicks" : 1}
    patternDefinition.axis is Query;
    annotation {"Name" : "Angle"}
    isAngle(patternDefinition.angle, ANGLE_360_BOUNDS);
    annotation {"Name" : "Instance count"}
    isInteger(patternDefinition.instanceCount, POSITIVE_COUNT_BOUNDS);
    if(patternDefinition.oppositeDirection != undefined)
    {
        annotation {"Name" : "Opposite direction", "UIHint" : "OppositeDirection"}
        patternDefinition.oppositeDirection is boolean;
    }
    if(patternDefinition.equalSpace != undefined)
    {
        annotation {"Name" : "Equal spacing"}
        patternDefinition.equalSpace is boolean;
    }

}
{
    startFeature(context, id, patternDefinition);
    var transforms = [];
    var instanceNames = [];

    if(verifyPatternSize(context, id, patternDefinition.instanceCount))
      return;

    var angle = patternDefinition.angle;
    if(patternDefinition.oppositeDirection == true)
        angle = -angle;

    var rawDirectionResult = evAxis(context, {"axis" : patternDefinition.axis});
    if (reportFeatureError(context, id, rawDirectionResult.error))
        return;

    if(patternDefinition.equalSpace == true)
    {
        if( patternDefinition.instanceCount < 2 )
        {
            reportFeatureError(context, id, ErrorStringEnum.PATTERN_INPUT_TOO_FEW_INSTANCES);
            return;
        }
        var isFull = abs(abs(stripUnits(angle)) - (2 * PI)) < TOLERANCE.zeroAngle;
        var instCt = isFull ? patternDefinition.instanceCount : patternDefinition.instanceCount - 1;
        angle = angle / instCt; //with error check above, no chance of instCt < 1
    }

    for(var i = 1; i < patternDefinition.instanceCount; i += 1)
    {
        transforms = append(transforms, rotationAround(rawDirectionResult.result, i * angle));
        instanceNames = append(instanceNames, "" ~ i);
    }

    if (patternDefinition.isFacePattern == true)
    {
        opPattern(context, id, { "entities" : patternDefinition.faces, "transforms" : transforms , "instanceNames" : instanceNames  });
    }
    else
    {
        opPattern(context, id, { "entities" : patternDefinition.entities, "transforms" : transforms , "instanceNames" : instanceNames});
    }
    endFeature(context, id);
}



//======================================================================================
//CurvePattern Feature
annotation { "Feature Type Name" : "Curve pattern" }
export function curvePattern(context is Context, id is Id, patternDefinition is map)
precondition
{
    annotation {"Name" : "Entities to pattern", "Filter" : EntityType.BODY}
    patternDefinition.entities is Query;
    annotation {"Name" : "Curve", "Filter" : EntityType.EDGE}
    patternDefinition.curve is Query;
    annotation {"Name" : "Instance count"}
    isInteger(patternDefinition.instanceCount, POSITIVE_COUNT_BOUNDS);
    annotation {"Name" : "Opposite direction", "UIHint" : "OppositeDirection"}
    patternDefinition.oppositeDirection is boolean;
    annotation {"Name" : "Follow curve"}
    patternDefinition.followCurve is boolean;
    booleanStepPredicate(patternDefinition);
}
{

    startFeature(context, id, patternDefinition);

    // Compute a vector of transforms
    var transforms = [];
    var parameters = [];
    var instNames =[];
    // Equally spaced
    var count = patternDefinition.instanceCount;
    var isClosed = size(evaluateQuery(context, qVertexAdjacent(patternDefinition.curve, EntityType.VERTEX))) < 2;
    var parameterStep = 1 / (count - (isClosed ? 0 : 1));
    for (var i = 0; i < count; i += 1)
    {
        parameters = append(parameters, i * parameterStep);
        instNames = append(instNames, "" ~ i);
    }

    if(patternDefinition.oppositeDirection)
    {
        parameters = reverse(parameters);
    }
    var evaluatedResult = evEdgeTangentLines(context, { "edge" : patternDefinition.curve, "parameters" : parameters });
    if(reportFeatureError(context, id, evaluatedResult.error))
        return;

    var evaluated = evaluatedResult.result;
    var lastPoint;
    var lastTan;
    var transform;
    for (var positionAndTangent in evaluated)
    {
        var pos = positionAndTangent.origin;
        var tangent = positionAndTangent.direction;

        if(transform == undefined)
        {
            transform = identityTransform();
        }
        else
        {
            if (patternDefinition.followCurve)
            {
                // Compute a rotation from the old to the new
                var rotation = rotationMatrix3d(lastTan, tangent);
                transform = transform(rotation, pos) * transform(-lastPoint) * transform;
            }
            else
            {
                transform = transform(pos - lastPoint) * transform;
            }
        }
        lastTan = tangent;
        lastPoint = pos;
        transforms = append(transforms, transform);
    }

    opPattern(context, id, { "entities" : patternDefinition.entities, "transforms" : transforms , "instanceNames" : instNames});
    opDeleteBodies(context, id + "delete", { "entities" : patternDefinition.entities });
    processNewBodyIfNeeded(context, id, patternDefinition);
    endFeature(context, id);
}

