FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "✨");
export import(path : "onshape/std/tool.fs", version : "✨");
export import(path : "onshape/std/patternUtils.fs", version : "✨");

// Imports used internally
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");

/**
 * Performs a body, face, or feature circular pattern. Internally, performs
 * an `applyPattern`, which in turn performs an `opPattern` or, for a feature
 * pattern, calls the feature function.
 */
annotation { "Feature Type Name" : "Circular pattern", "Filter Selector" : "allparts" }
export const circularPattern = defineFeature(function(context is Context, id is Id, definition is map)
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

        annotation { "Name" : "Axis of pattern", "Filter" : QueryFilterCompound.ALLOWS_AXIS, "MaxNumberOfPicks" : 1 }
        definition.axis is Query;

        annotation { "Name" : "Angle" }
        isAngle(definition.angle, ANGLE_360_BOUNDS);

        annotation { "Name" : "Instance count" }
        isInteger(definition.instanceCount, PRIMARY_PATTERN_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Equal spacing" }
        definition.equalSpace is boolean;

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

        var transforms = [];
        var instanceNames = [];

        verifyPatternSize(context, id, definition.instanceCount);

        var angle = definition.angle;
        if (definition.oppositeDirection == true)
            angle = -angle;

        var remainingTransform = getRemainderPatternTransform(context, { "references" : definition.entities });
        var direction = computePatternAxis(context, definition.axis, isFeaturePattern(definition.patternType), remainingTransform);
        if (direction == undefined)
            throw regenError(ErrorStringEnum.PATTERN_CIRCULAR_NO_AXIS, ["axis"]);

        if (definition.equalSpace)
        {
            if (definition.instanceCount < 2)
                throw regenError(ErrorStringEnum.PATTERN_INPUT_TOO_FEW_INSTANCES, ["instanceCount"]);

            const isFull = abs(abs(stripUnits(angle)) - (2 * PI)) < TOLERANCE.zeroAngle;
            const instCt = isFull ? definition.instanceCount : definition.instanceCount - 1;
            angle = angle / instCt; //with error check above, no chance of instCt < 1
        }

        for (var i = 1; i < definition.instanceCount; i += 1)
        {
            var instanceTransform = rotationAround(direction, i * angle);
            transforms = append(transforms, instanceTransform);
            instanceNames = append(instanceNames, "" ~ i);
        }

        definition.transforms = transforms;
        definition.instanceNames = instanceNames;
        definition.seed = definition.entities;

        applyPattern(context, id, definition, remainingTransform);
    }, { patternType : PatternType.PART, operationType : NewBodyOperationType.NEW,
         oppositeDirection : false, equalSpace : false });

