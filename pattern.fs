FeatureScript 213; /* Automatically generated version */
export import(path : "onshape/std/geomOperations.fs", version : "");
export import(path : "onshape/std/boolean.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/transform.fs", version : "");
export import(path : "onshape/std/print.fs", version : "");

export const PATTERN_OFFSET_BOUND = NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS;


function computePatternOffset(context is Context, entity is Query, oppositeDir is boolean, distance is ValueWithUnits) returns map
{
    if (oppositeDir)
        distance = -distance;

    var rawDirectionResult = try(evAxis(context, { "axis" : entity }));
    if (rawDirectionResult == undefined)
        return { "offset" : evPlane(context, { "face" : entity }).normal * distance };
    else
        return { "offset" : rawDirectionResult.direction * distance };
}

function verifyPatternSize(context is Context, id is Id, instances is number)
{
    if (instances <= 2500)
        return; //Fine
    throw regenError(ErrorStringEnum.PATTERN_INPUT_TOO_MANY_INSTANCES);
}

//LinearPattern Feature
annotation { "Feature Type Name" : "Linear pattern", "Filter Selector" : "allparts" }
export const linearPattern = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Face pattern", "Default" : false }
        definition.isFacePattern is boolean;

        if (!definition.isFacePattern)
        {
            booleanStepTypePredicate(definition);

            annotation { "Name" : "Entities to pattern", "Filter" : EntityType.BODY }
            definition.entities is Query;
        }
        else
        {
            annotation { "Name" : "Faces to pattern", "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
            definition.faces is Query;
        }

        annotation { "Name" : "Direction",
                     "Filter" : QueryFilterCompound.ALLOWS_AXIS || GeometryType.PLANE,
                     "MaxNumberOfPicks" : 1 }
        definition.directionOne is Query;

        annotation { "Name" : "Distance" }
        isLength(definition.distance, PATTERN_OFFSET_BOUND);

        annotation { "Name" : "Instance count" }
        isInteger(definition.instanceCount, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Second direction" }
        definition.hasSecondDir is boolean;

        if (definition.hasSecondDir)
        {
            annotation { "Name" : "Direction",
                         "Filter" : QueryFilterCompound.ALLOWS_AXIS || GeometryType.PLANE,
                         "MaxNumberOfPicks" : 1 }
            definition.directionTwo is Query;

            annotation { "Name" : "Distance" }
            isLength(definition.distanceTwo, PATTERN_OFFSET_BOUND);

            annotation { "Name" : "Instance count" }
            isInteger(definition.instanceCountTwo, POSITIVE_COUNT_BOUNDS_DEFAULT_1);

            annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
            definition.oppositeDirectionTwo is boolean;
        }
        if (!definition.isFacePattern)
        {
            booleanStepScopePredicate(definition);
        }
    }
    {
        if (definition.isFacePattern)
            definition.entities = definition.faces;

        checkInput(context, id, definition);

        // Compute a vector of transforms
        var transforms = [];
        var instanceNames = [];

        //Dir 1
        var result = try(computePatternOffset(context, definition.directionOne,
                         definition.oppositeDirection, definition.distance));
        if (result == undefined)
            throw regenError(ErrorStringEnum.PATTERN_LINEAR_NO_DIR, ["directionOne"]);
        var offset1 = result.offset;
        var count1 = definition.instanceCount;

        //Dir2, if any
        var offset2 = zeroVector(3) * meter;
        var count2 = 1;
        if (definition.hasSecondDir == true)
        {
            count2 = definition.instanceCountTwo;

            var result = try(computePatternOffset(context, definition.directionTwo,
                                    definition.oppositeDirectionTwo, definition.distanceTwo));
            if (result != undefined)
            {
                offset2 = result.offset;
                if (parallelVectors(offset1, offset2))
                { //notify user that parallel directions are selected for dir1 and dir2
                    reportFeatureInfo(context, id, ErrorStringEnum.PATTERN_DIRECTIONS_PARALLEL);
                }
            }
            else if (count2 > 1)
            {
                //if count2 = 1, we don't need a direction (i.e. we keep the 1-directional solution),
                //so only complain about direction if the count for second direction is > 1.
                throw regenError(ErrorStringEnum.PATTERN_LINEAR_NO_DIR, ["directionTwo"]);
            }
        }

        verifyPatternSize(context, id, count1 * count2);

        //Create the transforms and instance names, create along the first direction first then the second direction
        for (var j = 0; j < count2; j += 1)
        {
            for (var i = 0; i < count1; i += 1)
            {
                if (j == 0 && i == 0) //skip recreating original
                    continue;

                transforms = append(transforms, transform(identityMatrix(3), offset1 * i + offset2 * j));
                var instName = "" ~ i;
                if (j > 0)
                {
                    instName ~= "_" ~ j;
                }
                instanceNames = append(instanceNames, instName);
            }
        }

        definition.transforms = transforms;
        definition.instanceNames = instanceNames;

        opPattern(context, id, definition);

        processPatternBooleansIfNeeded(context, id, definition);
    }, { isFacePattern : true, operationType : NewBodyOperationType.NEW,
         hasSecondDir : false, oppositeDirection : false, oppositeDirectionTwo : false });

//======================================================================================
//CircularPattern Feature
annotation { "Feature Type Name" : "Circular pattern", "Filter Selector" : "allparts" }
export const circularPattern = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Face pattern", "Default" : false }
        definition.isFacePattern is boolean;

        if (!definition.isFacePattern)
        {
            booleanStepTypePredicate(definition);

            annotation { "Name" : "Entities to pattern", "Filter" : EntityType.BODY }
            definition.entities is Query;
        }
        else
        {
            annotation { "Name" : "Faces to pattern",
                         "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
            definition.faces is Query;
        }

        annotation { "Name" : "Axis of pattern", "Filter" : QueryFilterCompound.ALLOWS_AXIS, "MaxNumberOfPicks" : 1 }
        definition.axis is Query;

        annotation { "Name" : "Angle" }
        isAngle(definition.angle, ANGLE_360_BOUNDS);

        annotation { "Name" : "Instance count" }
        isInteger(definition.instanceCount, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Equal spacing" }
        definition.equalSpace is boolean;

        if (!definition.isFacePattern)
        {
            booleanStepScopePredicate(definition);
        }
    }
    {
        if (definition.isFacePattern)
            definition.entities = definition.faces;

        checkInput(context, id, definition);

        var transforms = [];
        var instanceNames = [];

        verifyPatternSize(context, id, definition.instanceCount);

        var angle = definition.angle;
        if (definition.oppositeDirection == true)
            angle = -angle;

        var rawDirectionResult = try(evAxis(context, { "axis" : definition.axis }));
        if (rawDirectionResult == undefined)
            throw regenError(ErrorStringEnum.PATTERN_CIRCULAR_NO_AXIS, ["axis"]);

        if (definition.equalSpace)
        {
            if (definition.instanceCount < 2)
                throw regenError(ErrorStringEnum.PATTERN_INPUT_TOO_FEW_INSTANCES, ["instanceCount"]);

            var isFull = abs(abs(stripUnits(angle)) - (2 * PI)) < TOLERANCE.zeroAngle;
            var instCt = isFull ? definition.instanceCount : definition.instanceCount - 1;
            angle = angle / instCt; //with error check above, no chance of instCt < 1
        }

        for (var i = 1; i < definition.instanceCount; i += 1)
        {
            transforms = append(transforms, rotationAround(rawDirectionResult, i * angle));
            instanceNames = append(instanceNames, "" ~ i);
        }

        definition.transforms = transforms;
        definition.instanceNames = instanceNames;

        opPattern(context, id, definition);

        processPatternBooleansIfNeeded(context, id, definition);
    }, { isFacePattern : true, operationType : NewBodyOperationType.NEW,
         oppositeDirection : false, equalSpace : false });


function processPatternBooleansIfNeeded(context is Context, id is Id, definition is map)
{
    if (!definition.isFacePattern)
    {
        const reconstructOp = function(id) { opPattern(context, id, definition); };
        processNewBodyIfNeeded(context, id, mergeMaps(definition, { "seed" : definition.entities }), reconstructOp);
    }
}

function checkInput(context is Context, id is Id, definition is map)
{
    if (size(evaluateQuery(context, definition.entities)) == 0)
    {
        if (definition.isFacePattern)
            throw regenError(ErrorStringEnum.PATTERN_SELECT_FACES, ["faces"]);
        else
            throw regenError(ErrorStringEnum.PATTERN_SELECT_PARTS, ["entities"]);
    }
}


//======================================================================================
//CurvePattern Feature
annotation { "Feature Type Name" : "Curve pattern", "Filter Selector" : "allparts" }
export const curvePattern = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities to pattern", "Filter" : EntityType.BODY }
        definition.entities is Query;

        annotation { "Name" : "Curve", "Filter" : EntityType.EDGE }
        definition.curve is Query;

        annotation { "Name" : "Instance count" }
        isInteger(definition.instanceCount, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Follow curve" }
        definition.followCurve is boolean;

        booleanStepPredicate(definition);
    }
    {
        // Compute a vector of transforms
        var transforms = [];
        var parameters = [];
        var instNames = [];
        // Equally spaced
        var count = definition.instanceCount;
        var isClosed = size(evaluateQuery(context, qVertexAdjacent(definition.curve, EntityType.VERTEX))) < 2;
        var parameterStep = 1 / (count - (isClosed ? 0 : 1));
        for (var i = 0; i < count; i += 1)
        {
            parameters = append(parameters, i * parameterStep);
            instNames = append(instNames, "" ~ i);
        }

        if (definition.oppositeDirection)
        {
            parameters = reverse(parameters);
        }
        var evaluated = evEdgeTangentLines(context, { "edge" : definition.curve, "parameters" : parameters });
        var lastPoint;
        var lastTan;
        var curTransform;
        for (var positionAndTangent in evaluated)
        {
            var pos = positionAndTangent.origin;
            var tangent = positionAndTangent.direction;

            if (curTransform == undefined)
            {
                curTransform = identityTransform();
            }
            else
            {
                if (definition.followCurve)
                {
                    // Compute a rotation from the old to the new
                    var rotation = rotationMatrix3d(lastTan, tangent);
                    curTransform = transform(rotation, pos) * transform(-lastPoint) * curTransform;
                }
                else
                {
                    curTransform = transform(pos - lastPoint) * curTransform;
                }
            }
            lastTan = tangent;
            lastPoint = pos;
            transforms = append(transforms, curTransform);
        }

        definition.transforms = transforms;
        definition.instanceNames = instNames;
        opPattern(context, id, definition);
        const reconstructOp = function(id) { opPattern(context, id, definition); };
        processNewBodyIfNeeded(context, id, definition, reconstructOp);
    });

