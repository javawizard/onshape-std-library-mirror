FeatureScript 336; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "336.0");
export import(path : "onshape/std/tool.fs", version : "336.0");
export import(path : "onshape/std/patternUtils.fs", version : "336.0");

// Imports used internally
import(path : "onshape/std/mathUtils.fs", version : "336.0");
import(path : "onshape/std/units.fs", version : "336.0");

/**
 * Performs a body, face, or feature linear pattern. Internally, performs
 * an `applyPattern`, which in turn performs an `opPattern` or, for a feature
 * pattern, calls the feature function.
 */
annotation { "Feature Type Name" : "Linear pattern", "Filter Selector" : "allparts" }
export const linearPattern = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Pattern type" }
        definition.patternType is PatternType;

        if (definition.patternType == PatternType.PART)
        {
            booleanStepTypePredicate(definition);

            annotation { "Name" : "Entities to pattern", "Filter" : EntityType.BODY }
            definition.entities is Query;
        }
        else if (definition.patternType == PatternType.FACE)
        {
            annotation { "Name" : "Faces to pattern",
                         "UIHint" : "ALLOW_FEATURE_SELECTION",
                         "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
            definition.faces is Query;
        }
        else if (definition.patternType == PatternType.FEATURE)
        {
            annotation { "Name" : "Features to pattern" }
            definition.instanceFunction is FeatureList;
        }

        annotation { "Name" : "Direction",
                     "Filter" : QueryFilterCompound.ALLOWS_AXIS || GeometryType.PLANE,
                     "MaxNumberOfPicks" : 1 }
        definition.directionOne is Query;

        annotation { "Name" : "Distance" }
        isLength(definition.distance, PATTERN_OFFSET_BOUND);

        annotation { "Name" : "Instance count" }
        isInteger(definition.instanceCount, PRIMARY_PATTERN_BOUNDS);

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
            isInteger(definition.instanceCountTwo, SECONDARY_PATTERN_BOUNDS);

            annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
            definition.oppositeDirectionTwo is boolean;
        }
        if (definition.patternType == PatternType.PART)
        {
            booleanStepScopePredicate(definition);
        }
    }
    {
        if (definition.patternType == PatternType.FACE)
            definition.entities = definition.faces;

        checkInput(context, id, definition, false);

        if (definition.patternType == PatternType.FEATURE)
            definition.instanceFunction = valuesSortedById(context, definition.instanceFunction);

        var remainingTransform = getRemainderPatternTransform(context,
            { "references" : definition.entities});

        // Compute a vector of transforms
        var transforms = [];
        var instanceNames = [];

        //Dir 1
        const result = try(computePatternOffset(context, definition.directionOne,
                    definition.oppositeDirection, definition.distance, isFeaturePattern(definition.patternType), remainingTransform));

        if (result == undefined)
            throw regenError(ErrorStringEnum.PATTERN_LINEAR_NO_DIR, ["directionOne"]);
        const offset1 = result.offset;
        const count1 = definition.instanceCount;

        //Dir2, if any
        var offset2 = zeroVector(3) * meter;
        var count2 = 1;
        if (definition.hasSecondDir == true)
        {
            count2 = definition.instanceCountTwo;

            const result = try(computePatternOffset(context, definition.directionTwo,
                        definition.oppositeDirectionTwo, definition.distanceTwo, isFeaturePattern(definition.patternType), remainingTransform));
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

                var instanceTransform = transform(identityMatrix(3), offset1 * i + offset2 * j);
                transforms = append(transforms, instanceTransform);
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
        definition.seed = definition.entities;

        applyPattern(context, id, definition, remainingTransform);
    }, { patternType : PatternType.PART, operationType : NewBodyOperationType.NEW,
         hasSecondDir : false, oppositeDirection : false, oppositeDirectionTwo : false });
