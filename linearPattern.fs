FeatureScript 1711; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "1711.0");
export import(path : "onshape/std/tool.fs", version : "1711.0");
export import(path : "onshape/std/patternUtils.fs", version : "1711.0");

// Imports used internally
import(path : "onshape/std/mathUtils.fs", version : "1711.0");
import(path : "onshape/std/units.fs", version : "1711.0");

/**
 * Performs a body, face, or feature linear pattern. Internally, performs
 * an [applyPattern], which in turn performs an [opPattern] or, for a feature
 * pattern, calls the feature function.
 *
 * @param id : @autocomplete `id + "linearPattern1"`
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
 *      @field directionOne {Query}:
 *              The direction of the pattern.
 *              @eg `qCreatedBy(newId() + "Right", EntityType.FACE)`
 *      @field distance {ValueWithUnits}:
 *              The distance between each pattern entity.
 *              @eg `1.0 * inch` to space the pattern entities 1 inch apart.
 *      @field instanceCount {number}:
 *              The resulting number of pattern entities, unless `isCentered` is `true`.
 *              @eg `2` to have 2 resulting pattern entities (including the seed).
 *      @field oppositeDirection {boolean}: @optional
 *              @ex `true` to switch the direction of the pattern along `directionOne`.
 *      @field isCentered {boolean}: @optional
 *              Whether to center the pattern on the seed. When set to `true`, `instanceCount - 1` pattern entities are
 *              created along each direction of `directionOne`. Default is `false`.
 *
 *      @field hasSecondDir {boolean}: @optional
 *              @ex `true` if the pattern should extend in two directions rather than one, creating a grid of pattern entities.
 *      @field directionTwo {Query}: @requiredif{`hasSecondDir` is `true`}
 *              The second direction of the pattern.
 *      @field distanceTwo {ValueWithUnits}: @requiredif{`hasSecondDir` is `true`}
 *              The distance between each pattern entity in the second direction.
 *      @field instanceCountTwo {number}: @requiredif{`hasSecondDir` is `true`}
 *              The resulting number of pattern entities in the second direction, unless `isCentered` is `true`.
 *      @field oppositeDirectionTwo {boolean}: @optional
 *              @ex `true` to switch the direction of the pattern along `directionTwo`.
 *      @field isCenteredTwo {boolean}: @optional
 *              Whether to center the second direction of the pattern on the seed. When set to `true`, `instanceCount - 1`
 *              pattern entities are created along each direction of `directionTwo`. Default is `false`.
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
annotation { "Feature Type Name" : "Linear pattern", "Filter Selector" : "allparts" }
export const linearPattern = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        patternTypePredicate(definition);

        annotation { "Name" : "Direction",
                     "Filter" : QueryFilterCompound.ALLOWS_DIRECTION,
                     "MaxNumberOfPicks" : 1 }
        definition.directionOne is Query;

        annotation { "Name" : "Distance" }
        isLength(definition.distance, PATTERN_OFFSET_BOUND);

        annotation { "Name" : "Instance count" }
        isInteger(definition.instanceCount, PRIMARY_PATTERN_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Centered"}
        definition.isCentered is boolean;

        annotation { "Name" : "Second direction", "Column Name" : "Has second direction" }
        definition.hasSecondDir is boolean;

        if (definition.hasSecondDir)
        {
            annotation { "Name" : "Direction", "Column Name" : "Second direction",
                         "Filter" : QueryFilterCompound.ALLOWS_DIRECTION,
                         "MaxNumberOfPicks" : 1 }
            definition.directionTwo is Query;

            annotation { "Name" : "Distance", "Column Name" : "Second distance" }
            isLength(definition.distanceTwo, PATTERN_OFFSET_BOUND);

            annotation { "Name" : "Instance count", "Column Name" : "Second instance count" }
            isInteger(definition.instanceCountTwo, SECONDARY_PATTERN_BOUNDS);

            annotation { "Name" : "Opposite direction", "Column Name" : "Second opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.oppositeDirectionTwo is boolean;

            annotation { "Name" : "Centered", "Column Name" : "Second centered"}
            definition.isCenteredTwo is boolean;
        }
        if (definition.patternType == PatternType.PART)
        {
            booleanPatternScopePredicate(definition);
        }

        if (definition.patternType == PatternType.FEATURE)
        {
            annotation { "Name" : "Apply per instance" }
            definition.fullFeaturePattern is boolean;
        }
    }
    {
        definition = adjustPatternDefinitionEntities(context, definition, false);

        var remainingTransform = getRemainderPatternTransform(context, { "references" : getReferencesForRemainderTransform(definition)});

        var withDirectionTransform = isFeaturePattern(definition.patternType) || isAtVersionOrLater(context, FeatureScriptVersionNumber.V518_MIRRORING_LIN_PATTERNS);
        //Dir 1
        const result = try(computePatternOffset(context, definition.directionOne,
                    definition.oppositeDirection, definition.distance, withDirectionTransform, remainingTransform));

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
                        definition.oppositeDirectionTwo, definition.distanceTwo, withDirectionTransform, remainingTransform));
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

        // Compute a vector of transforms
        // Adding just the values and mutating the transform rather than creating the translation from scratch on each iteration
        // is necessary for performance since it is in an inner loop bottleneck.
        var transforms = [];
        var instanceNames = [];
        const identity = identityMatrix(3);
        var instanceTransform = transform(identity, zeroVector(3) * meter);

        // If centered, create (count - 1) number of new instances on either side of the seed.
        var startIndex1 = definition.isCentered ? 1 - count1 : 0;
        var startIndex2 = definition.isCenteredTwo ? 1 - count2 : 0;

        for (var j = startIndex2; j < count2; j += 1)
        {
            const instName = j == 0 ? "" : ("_" ~ j);
            instanceTransform.translation = offset2 * j + offset1 * startIndex1;
            for (var i = startIndex1; i < count1; i += 1)
            {
                // skip recreating original
                if (j != 0 || i != 0)
                {
                    transforms = append(transforms, instanceTransform);
                    instanceNames = append(instanceNames, i ~ instName);
                }
                instanceTransform.translation[0].value += offset1[0].value;
                instanceTransform.translation[1].value += offset1[1].value;
                instanceTransform.translation[2].value += offset1[2].value;
            }
        }

        definition.transforms = transforms;
        definition.instanceNames = instanceNames;
        definition.seed = definition.entities;

        applyPattern(context, id, definition, remainingTransform);
    }, { patternType : PatternType.PART, operationType : NewBodyOperationType.NEW, hasSecondDir : false,
         oppositeDirection : false, oppositeDirectionTwo : false, isCentered : false, isCenteredTwo : false, fullFeaturePattern : false });

