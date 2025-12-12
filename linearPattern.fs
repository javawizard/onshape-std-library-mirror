FeatureScript 2837; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2837.0");
export import(path : "onshape/std/tool.fs", version : "2837.0");
export import(path : "onshape/std/patternUtils.fs", version : "2837.0");

// Imports used internally
import(path : "onshape/std/manipulator.fs", version : "2837.0");
import(path : "onshape/std/mathUtils.fs", version : "2837.0");
import(path : "onshape/std/units.fs", version : "2837.0");
import(path : "onshape/std/recordpatterntype.gen.fs", version : "2837.0");

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
 *
 *      @field skipInstances {boolean}: @optional
 *              Whether to exclude certain instances of the pattern.
 *      @field skippedInstances {array}: @requiredif {`skipInstances` is `true`}
 *              Which instances of the pattern to skip. Each is denoted by one index for each direction,
 *              either of which may be negative if `isCentered` or `isCenteredTwo` respectively is `true`.
 *              @ex `[{ index1: -3, index2: 1 }, { index1: 0, index2: -2 }, { index1: 5, index2: 0 }]`
 * }}
 */
annotation { "Feature Type Name" : "Linear pattern", "Filter Selector" : "allparts", "Manipulator Change Function" : "linearPatternPointChange" }
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

        annotation { "Name" : "Centered" }
        definition.isCentered is boolean;

        annotation { "Name" : "Second direction", "Column Name" : "Has second direction" }
        definition.hasSecondDir is boolean;

        annotation { "Group Name" : "Second direction", "Driving Parameter" : "hasSecondDir", "Collapsed By Default" : false }
        {
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

                annotation { "Name" : "Centered", "Column Name" : "Second centered" }
                definition.isCenteredTwo is boolean;
            }
        }

        if (definition.patternType == PatternType.PART)
        {
            booleanPatternScopePredicate(definition);
        }

        if (definition.patternType == PatternType.FEATURE)
        {
            annotation { "Name" : "Reapply features" }
            definition.fullFeaturePattern is boolean;
        }

        annotation { "Name" : "Skip instances" }
        definition.skipInstances is boolean;

        annotation { "Group Name" : "Skip instances", "Driving Parameter" : "skipInstances", "Collapsed By Default" : false }
        {
            if (definition.skipInstances)
            {
                annotation { "Name" : "Instances to skip", "Item name" : "instance", "Item label template" : "(#index1, #index2)", "Show labels only" : true, "UIHint" : [UIHint.INITIAL_FOCUS, UIHint.PREVENT_ARRAY_REORDER, UIHint.ALLOW_ARRAY_FOCUS] }
                definition.skippedInstances is array;

                for (var instance in definition.skippedInstances)
                {
                    annotation { "Name" : "First direction" }
                    isInteger(instance.index1, { (unitless) : [-1e5, 1, 1e5] } as IntegerBoundSpec);

                    annotation { "Name" : "Second direction" }
                    isInteger(instance.index2, { (unitless) : [-1e5, 0, 1e5] } as IntegerBoundSpec);
                }
            }
        }
    }
    {
        verifyNoMesh(context, definition, "directionOne");
        if (definition.hasSecondDir)
        {
            verifyNoMesh(context, definition, "directionTwo");
        }

        definition = adjustPatternDefinitionEntities(context, definition, false);

        const remainingTransform = getRemainderPatternTransform(context, { "references" : getReferencesForRemainderTransform(definition) });
        const withDirectionTransform = isFeaturePattern(definition.patternType) || isAtVersionOrLater(context, FeatureScriptVersionNumber.V518_MIRRORING_LIN_PATTERNS);

        //Dir 1
        const result = try(computePatternOffset(context, definition.directionOne,
                definition.oppositeDirection, definition.distance, withDirectionTransform, remainingTransform));

        if (result == undefined)
            throw regenError(ErrorStringEnum.PATTERN_LINEAR_NO_DIR, ["directionOne"]);
        const offset1 = result.offset;

        //Dir2, if any
        var offset2 = zeroVector(3) * meter;
        if (definition.hasSecondDir)
        {
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
            else if (definition.instanceCountTwo > 1)
            {
                //if definition.instanceCountTwo == 1, we don't need a direction (i.e. we keep the 1-directional solution),
                //so only complain about direction if the count for second direction is > 1.
                throw regenError(ErrorStringEnum.PATTERN_LINEAR_NO_DIR, ["directionTwo"]);
            }
        }
        else
        {
            definition.instanceCountTwo = 1;
        }

        verifyPatternSize(context, id, definition.instanceCount * definition.instanceCountTwo);

        const linearPatternTransforms = computeLinearPatternTransforms(context, definition, offset1, offset2);

        if (definition.skipInstances)
        {
            reportAnyInvalidEntries(context, id, definition);

            const instanceToIndex = function(instance)
                {
                    return gridCoordinatesToIndex(instance.index1, instance.index2, definition.instanceCount, definition.hasSecondDir ? definition.instanceCountTwo : 1, definition.isCentered, definition.hasSecondDir && definition.isCenteredTwo);
                };
            const isInstanceWithinRange = function(instance)
                {
                    return !isIndexOutsideRange(instance.index1, instance.index2, definition.instanceCount, definition.hasSecondDir ? definition.instanceCountTwo : 1, definition.isCentered, definition.hasSecondDir && definition.isCenteredTwo);
                };
            addManipulators(context, id, { "points" : {
                                "points" : linearPatternTransforms.manipulatorPoints,
                                "selectedIndices" : mapArray(filter(definition.skippedInstances, isInstanceWithinRange), instanceToIndex),
                                "suppressedIndices" : [instanceToIndex({ "index1" : 0, "index2" : 0 })],
                                "manipulatorType" : ManipulatorType.TOGGLE_POINTS } as Manipulator });
        }

        definition.transforms = linearPatternTransforms.transforms;
        definition.instanceNames = linearPatternTransforms.instanceNames;
        definition.seed = definition.entities;

        definition.sketchPatternInfo = ErrorStringEnum.LINEAR_PATTERN_SKETCH_REAPPLY_INFO;

        applyPattern(context, id, definition, remainingTransform);

        const patternDirections = definition.instanceCountTwo > 1 ? [offset1, offset2] : [offset1];
        setPatternData(context, id, RecordPatternType.LINEAR, patternDirections);
    }, {
            patternType : PatternType.PART,
            operationType : NewBodyOperationType.NEW,
            hasSecondDir : false,
            oppositeDirection : false,
            oppositeDirectionTwo : false,
            isCentered : false,
            isCenteredTwo : false,
            fullFeaturePattern : false,
            skipInstances : false,
            skippedInstances : []
        });

function reportAnyInvalidEntries(context is Context, id is Id, definition is map)
{
    var hasSeedIndex = false;
    var hasOutsideRangeIndex = false;

    for (var instance in definition.skippedInstances)
    {
        if (instance.index1 == 0 && instance.index2 == 0)
        {
            hasSeedIndex = true;
        }

        if (isIndexOutsideRange(instance.index1, instance.index2, definition.instanceCount, definition.hasSecondDir ? definition.instanceCountTwo : 1, definition.isCentered, definition.hasSecondDir && definition.isCenteredTwo))
        {
            hasOutsideRangeIndex = true;
        }
    }

    if (hasSeedIndex)
    {
        reportFeatureInfo(context, id, ErrorStringEnum.PATTERN_SKIPPED_INSTANCES_SEED_INDEX);
    }
    else if (hasOutsideRangeIndex)
    {
        reportFeatureInfo(context, id, ErrorStringEnum.PATTERN_SKIPPED_INSTANCES_OUT_OF_RANGE_INDEX);
    }
}

function computeLinearPatternTransforms(context is Context, definition is map, offset1 is Vector, offset2 is Vector) returns PatternTransforms
{
    // Features held back from before the @computeLinearPatternTransforms builtin was introduced should run the previous code for computing the list of transforms.
    // definition.computeTransformsWithoutBuiltin will be true for such features until the next time the feature is opened, after which it will be undefined.
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2338_PATTERN_SKIP_INSTANCES) || definition.computeTransformsWithoutBuiltin != true)
    {
        definition.offset1 = offset1;
        definition.offset2 = offset2;

        if (definition.skipInstances)
        {
            definition.startPoint = try silent (getStartPoint(context, getReferencesForStartPoint(definition)));
        }

        return @computeLinearPatternTransforms(context, definition) as PatternTransforms;
    }

    // Compute a vector of transforms
    // Adding just the values and mutating the transform rather than creating the translation from scratch on each iteration
    // is necessary for performance since it is in an inner loop bottleneck.
    var transforms = [];
    var instanceNames = [];
    const identity = identityMatrix(3);
    var instanceTransform = transform(identity, zeroVector(3) * meter);

    const count1 = definition.instanceCount;
    const count2 = definition.instanceCountTwo;

    // If centered, create (count - 1) number of new instances on either side of the seed.
    const startIndex1 = definition.isCentered ? 1 - count1 : 0;
    const startIndex2 = definition.isCenteredTwo ? 1 - count2 : 0;

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

    return { "transforms" : transforms, "instanceNames" : instanceNames, "manipulatorPoints" : [] } as PatternTransforms;
}

function indexToGridCoordinates(index is number, instanceCount1 is number, instanceCount2 is number, isCentered1 is boolean, isCentered2 is boolean) returns map
{
    const index1Max = isCentered1 ? 2 * instanceCount1 - 1 : instanceCount1;
    return {
            "index1" : isCentered1 ? index % index1Max - instanceCount1 + 1 : index % index1Max,
            "index2" : isCentered2 ? floor(index / index1Max) - instanceCount2 + 1 : floor(index / index1Max)
        };
}

function gridCoordinatesToIndex(index1 is number, index2 is number, instanceCount1 is number, instanceCount2 is number, isCentered1 is boolean, isCentered2 is boolean) returns number
{
    const index1Max = isCentered1 ? 2 * instanceCount1 - 1 : instanceCount1;

    const normalizedIndex1 = isCentered1 ? index1 + instanceCount1 - 1 : index1;
    const normalizedIndex2 = isCentered2 ? index2 + instanceCount2 - 1 : index2;

    return normalizedIndex1 + normalizedIndex2 * index1Max;
}

function isIndexOutsideRange(index1 is number, index2 is number, instanceCount1 is number, instanceCount2 is number, isCentered1 is boolean, isCentered2 is boolean) returns boolean
{
    return index1 >= instanceCount1 || index1 < (isCentered1 ? -instanceCount1 + 1 : 0)
        || index2 >= instanceCount2 || index2 < (isCentered2 ? -instanceCount2 + 1 : 0);
}

/**
 * @internal
 * The manipulator change function used in the `linearPattern` feature.
 */
export function linearPatternPointChange(context is Context, definition is map, newManipulators is map) returns map
{
    const indexToInstance = function(index)
        {
            return indexToGridCoordinates(index, definition.instanceCount, definition.hasSecondDir ? definition.instanceCountTwo : 1, definition.isCentered, definition.hasSecondDir && definition.isCenteredTwo);
        };
    const isInstanceOutsideRange = function(instance)
        {
            return isIndexOutsideRange(instance.index1, instance.index2, definition.instanceCount, definition.hasSecondDir ? definition.instanceCountTwo : 1, definition.isCentered, definition.hasSecondDir && definition.isCenteredTwo);
        };

    const newInstances = mapArray(newManipulators["points"].selectedIndices, indexToInstance);
    const outInstances = filter(definition.skippedInstances, isInstanceOutsideRange);

    if (size(outInstances) == 0)
    {
        definition.skippedInstances = newInstances;
        return definition;
    }

    definition.skippedInstances = makeArray(size(newInstances) + size(outInstances));
    var newIndex = 0;
    var outIndex = 0;

    for (var i = 0; i < size(definition.skippedInstances); i += 1)
    {
        if (newIndex >= size(newInstances))
        {
            definition.skippedInstances[i] = outInstances[outIndex];
            outIndex += 1;
        }
        else if (outIndex >= size(outInstances))
        {
            definition.skippedInstances[i] = newInstances[newIndex];
            newIndex += 1;
        }
        else if (newInstances[newIndex].index2 < outInstances[outIndex].index2 || (newInstances[newIndex].index2 == outInstances[outIndex].index2 && newInstances[newIndex].index1 < outInstances[outIndex].index1))
        {
            definition.skippedInstances[i] = newInstances[newIndex];
            newIndex += 1;
        }
        else
        {
            definition.skippedInstances[i] = outInstances[outIndex];
            outIndex += 1;
        }
    }

    return definition;
}

