FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "✨");
export import(path : "onshape/std/tool.fs", version : "✨");
export import(path : "onshape/std/patternUtils.fs", version : "✨");

// Imports used internally
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/manipulator.fs", version : "✨");
import(path : "onshape/std/mathUtils.fs", version : "✨");
import(path : "onshape/std/recordpatterntype.gen.fs", version : "✨");

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
 *
 *      @field skipInstances {boolean}: @optional
 *              Whether to exclude certain instances of the pattern.
 *      @field skippedInstances {array}: @requiredif {`skipInstances` is `true`}
 *              Which instances of the pattern to skip. Each is denoted by a single index, which may be negative if `isCentered` is `true`.
 *              @ex `[{ index: -3 }, { index: 2 }, { index: 5 }]`
 * }}
 */
annotation { "Feature Type Name" : "Circular pattern", "Filter Selector" : "allparts", "Manipulator Change Function" : "circularPatternPointChange" }
export const circularPattern = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        patternTypePredicate(definition);

        annotation { "Name" : "Axis of pattern", "Filter" : QueryFilterCompound.ALLOWS_AXIS, "MaxNumberOfPicks" : 1 }
        definition.axis is Query;

        annotation { "Name" : "Angle" }
        isAngle(definition.angle, ANGLE_360_FULL_DEFAULT_BOUNDS);

        annotation { "Name" : "Instance count" }
        isInteger(definition.instanceCount, CIRCULAR_PATTERN_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION_CIRCULAR }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Equal spacing", "Default" : true }
        definition.equalSpace is boolean;

        annotation { "Name" : "Centered" }
        definition.isCentered is boolean;

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
                annotation { "Name" : "Instances to skip", "Item name" : "instance", "Item label template" : "#index", "Show labels only" : true, "UIHint" : [UIHint.INITIAL_FOCUS, UIHint.PREVENT_ARRAY_REORDER, UIHint.ALLOW_ARRAY_FOCUS] }
                definition.skippedInstances is array;

                for (var instance in definition.skippedInstances)
                {
                    annotation { "Name" : "Index" }
                    isInteger(instance.index, { (unitless) : [-1e5, 1, 1e5] } as IntegerBoundSpec);
                }
            }
        }
    }
    {
        verifyNoMesh(context, definition, "axis");

        definition = adjustPatternDefinitionEntities(context, definition, false);

        verifyPatternSize(context, id, definition.instanceCount);

        var angle = adjustAngle(context, definition.angle);
        if (definition.oppositeDirection == true)
            angle = -angle;

        const remainingTransform = getRemainderPatternTransform(context, { "references" : getReferencesForRemainderTransform(definition) });

        const withAxisTransform = isFeaturePattern(definition.patternType) || isAtVersionOrLater(context, FeatureScriptVersionNumber.V518_MIRRORING_LIN_PATTERNS);
        const axis = computePatternAxis(context, definition.axis, withAxisTransform, remainingTransform);
        if (axis == undefined)
            throw regenError(ErrorStringEnum.PATTERN_CIRCULAR_NO_AXIS, ["axis"]);

        if (definition.equalSpace)
        {
            if (tooFewPatternInstances(context, definition.instanceCount))
                throw regenError(ErrorStringEnum.PATTERN_INPUT_TOO_FEW_INSTANCES, ["instanceCount"]);

            const isFull = abs(abs(stripUnits(angle)) - (2 * PI)) < TOLERANCE.zeroAngle;
            const instCt = isFull ? definition.instanceCount : definition.instanceCount - 1;
            angle = instCt <= 1 ? angle : angle / instCt;
        }

        definition.startPoint = try silent (getStartPoint(context, getReferencesForStartPoint(definition)));
        const circularPatternTransforms = computeCircularPatternTransforms(context, definition, axis, angle);

        if (definition.skipInstances)
        {
            reportAnyInvalidEntries(context, id, definition);

            const instanceToIndex = function(instance) { return definition.isCentered ? instance.index + definition.instanceCount - 1 : instance.index; };
            addManipulators(context, id, { "points" : {
                                "points" : circularPatternTransforms.manipulatorPoints,
                                "selectedIndices" : mapArray(definition.skippedInstances, instanceToIndex),
                                "suppressedIndices" : [definition.isCentered ? definition.instanceCount - 1 : 0],
                                "manipulatorType" : ManipulatorType.TOGGLE_POINTS } as Manipulator });
        }

        definition.transforms = circularPatternTransforms.transforms;
        definition.instanceNames = circularPatternTransforms.instanceNames;
        definition.seed = definition.entities;

        definition.sketchPatternInfo = ErrorStringEnum.CIRCULAR_PATTERN_SKETCH_REAPPLY_INFO;

        applyPattern(context, id, definition, remainingTransform);

        setPatternData(context, id, RecordPatternType.CIRCULAR, [axis.direction]);
    }, {
            patternType : PatternType.PART,
            operationType : NewBodyOperationType.NEW,
            oppositeDirection : false,
            equalSpace : false,
            isCentered : false,
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
        if (instance.index == 0)
        {
            hasSeedIndex = true;
        }

        if (instance.index >= definition.instanceCount || instance.index < (definition.isCentered ? -definition.instanceCount + 1 : 0))
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

function computeCircularPatternTransforms(context is Context, definition is map, axis is Line, angle is ValueWithUnits) returns PatternTransforms
{
    // Features held back from before the @computeCircularPatternTransforms builtin was introduced should run the previous code for computing the list of transforms.
    // definition.computeTransformsWithoutBuiltin will be true for such features until the next time the feature is opened, after which it will be undefined.
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2338_PATTERN_SKIP_INSTANCES) || definition.computeTransformsWithoutBuiltin != true)
    {
        definition.axis = axis;
        definition.angle = angle;

        return @computeCircularPatternTransforms(context, definition) as PatternTransforms;
    }

    var transforms = [];
    var instanceNames = [];

    // If centered, create (instanceCount - 1) number of new instances on either side of the seed.
    var startIndex = definition.isCentered ? 1 - definition.instanceCount : 0;
    for (var i = startIndex; i < definition.instanceCount; i += 1)
    {
        if (i != 0)
        {
            const instanceTransform = rotationAround(axis, i * angle);
            transforms = append(transforms, instanceTransform);
            instanceNames = append(instanceNames, "" ~ i);
        }
    }

    return { "transforms" : transforms, "instanceNames" : instanceNames, "manipulatorPoints" : [] } as PatternTransforms;
}

/**
 * @internal
 * The manipulator change function used in the `circularPattern` feature.
 */
export function circularPatternPointChange(context is Context, definition is map, newManipulators is map) returns map
{
    const indexToInstance = function(index) { return { "index" : definition.isCentered ? index - definition.instanceCount + 1 : index }; };
    definition.skippedInstances = mapArray(newManipulators["points"].selectedIndices, indexToInstance);

    return definition;
}

