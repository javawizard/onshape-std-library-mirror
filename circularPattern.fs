FeatureScript 686; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "686.0");
export import(path : "onshape/std/tool.fs", version : "686.0");
export import(path : "onshape/std/patternUtils.fs", version : "686.0");

// Imports used internally
import(path : "onshape/std/curveGeometry.fs", version : "686.0");
import(path : "onshape/std/math.fs", version : "686.0");

/**
 * Performs a body, face, or feature circular pattern. Internally, performs
 * an [applyPattern], which in turn performs an [opPattern] or, for a feature
 * pattern, calls the feature function.
 *
 * @param id : @autocomplete `id + "circularPattern1"`
 * @param definition {{
 *      @field patternType {PatternType}: @optional
 *              Specifies a `PART`, `FEATURE`, or `FACE` pattern. Default is `PART`.
 *              @autocomplete `PatternType.PART`
 *      @field entities {Query}: @requiredif{`patternType` is `PART`}
 *              The parts to pattern.
 *              @eg `qCreatedBy(id + "extrude1", EntityType.BODY)`
 *      @field faces {Query}: @requiredif{`patternType` is `FACE`}
 *              The faces to pattern.
 *      @field instanceFunction {FeatureList}: @requiredif{`patternType` is `FEATURE`}
 *              The [FeatureList] of the features to pattern.
 *
 *      @field axis {Query}:
 *              The axis of the pattern.
 *      @field angle {ValueWithUnits}:
 *              The angle between each pattern instance, or the total angle spanned by the pattern if `equalSpace` is `true`.
 *              @eg `360 * degree`
 *      @field instanceCount {number}:
 *              The resulting number of pattern entities, unless `isCentered` is `true`.
 *              @eg `4` to have 4 resulting pattern entities (including the seed).
 *      @field oppositeDirection {boolean}: @optional
 *              @ex `true` to switch the direction of the pattern around the axis.
 *      @field equalSpace {boolean}: @optional
 *              @ex `true` for the entire pattern to lie within `angle`
 *              @ex `false` for there to be `angle` between each pattern instance (default)
 *      @field isCentered {boolean}: @optional
 *              Whether to center the pattern on the seed. When set to `true`, `instanceCount - 1` pattern entities are
 *              created in each direction around the axis. Default is `false`.
 *
 *      @field operationType {NewBodyOperationType} : @optional
 *              Specifies how the newly created body will be merged with existing bodies.
 *      @field defaultScope {boolean} : @optional
 *              @ex `true` to merge with all other bodies
 *              @ex `false` to merge with `booleanScope`
 *      @field booleanScope {Query} : @requiredif {`defaultScope` is `false`}
 *              The specified bodies to merge with.
 * }}
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
                         "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES }
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
        isAngle(definition.angle, ANGLE_360_FULL_DEFAULT_BOUNDS);

        annotation { "Name" : "Instance count" }
        isInteger(definition.instanceCount, CIRCULAR_PATTERN_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION_CIRCULAR" }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Equal spacing", "Default" : true }
        definition.equalSpace is boolean;

        annotation { "Name" : "Centered"}
        definition.isCentered is boolean;

        if (definition.patternType == PatternType.PART)
        {
            booleanStepScopePredicate(definition);
        }
    }
    {
        definition.angle = adjustAngle(context, definition.angle);

        definition = adjustPatternDefinitionEntities(context, definition, false);

        if (definition.patternType == PatternType.FEATURE)
            definition.instanceFunction = valuesSortedById(context, definition.instanceFunction);

        var transforms = [];
        var instanceNames = [];

        verifyPatternSize(context, id, definition.instanceCount);

        var angle = definition.angle;
        if (definition.oppositeDirection == true)
            angle = -angle;

        var remainingTransform = getRemainderPatternTransform(context, { "references" : definition.entities });

        var withAxisTransform = isFeaturePattern(definition.patternType) || isAtVersionOrLater(context, FeatureScriptVersionNumber.V518_MIRRORING_LIN_PATTERNS);
        var direction = computePatternAxis(context, definition.axis, withAxisTransform, remainingTransform);
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

        // If centered, create (instanceCount - 1) number of new instances on either side of the seed.
        var startIndex = definition.isCentered ? 1 - definition.instanceCount : 0;
        for (var i = startIndex; i < definition.instanceCount; i += 1)
        {
            if (i != 0)
            {
                var instanceTransform = rotationAround(direction, i * angle);
                transforms = append(transforms, instanceTransform);
                instanceNames = append(instanceNames, "" ~ i);
            }
        }

        definition.transforms = transforms;
        definition.instanceNames = instanceNames;
        definition.seed = definition.entities;

        applyPattern(context, id, definition, remainingTransform);
    }, { patternType : PatternType.PART, operationType : NewBodyOperationType.NEW,
         oppositeDirection : false, equalSpace : false, isCentered : false });

