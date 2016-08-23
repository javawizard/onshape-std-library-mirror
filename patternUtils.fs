FeatureScript 408; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Most patterns use these
export import(path : "onshape/std/boolean.fs", version : "408.0");
export import(path : "onshape/std/containers.fs", version : "408.0");
export import(path : "onshape/std/evaluate.fs", version : "408.0");
export import(path : "onshape/std/feature.fs", version : "408.0");
export import(path : "onshape/std/featureList.fs", version : "408.0");
export import(path : "onshape/std/valueBounds.fs", version : "408.0");

import(path : "onshape/std/mathUtils.fs", version : "408.0");

/** @internal */
export const PATTERN_OFFSET_BOUND = NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS;

/**
 * The type of pattern.
 * @value PART : Creates copies of bodies.
 * @value FEATURE : Calls a feature function multiple times, first informing the
 *          `context` of the transform to be applied.
 * @value FACE : Creates copies of faces and attempts to merge them with
 *          existing bodies.
 */
export enum PatternType
{
    annotation { "Name" : "Part pattern" }
    PART,
    annotation { "Name" : "Feature pattern" }
    FEATURE,
    annotation { "Name" : "Face pattern" }
    FACE
}

/**
 * The type of mirror.
 * @see `PatternType`
 */
export enum MirrorType
{
    annotation { "Name" : "Part mirror" }
    PART,
    annotation { "Name" : "Feature mirror" }
    FEATURE,
    annotation { "Name" : "Face mirror" }
    FACE
}

/**
 * @internal
 * TODO: Is this worth exposing?
 */
export function computePatternOffset(context is Context, entity is Query, oppositeDir is boolean, distance is ValueWithUnits,
    isFeaturePattern is boolean, remainingTransform is Transform) returns map
{
    if (oppositeDir)
        distance = -distance;

    var direction;
    const rawDirectionResult = try silent(evAxis(context, { "axis" : entity }));
    if (rawDirectionResult == undefined)
        direction = evPlane(context, { "face" : entity }).normal * distance;
    else
        direction = rawDirectionResult.direction * distance;

    if (isFeaturePattern)
    {
        var remainingTransformForAxis = getRemainderPatternTransform(context, { "references" : entity });
        return { "offset" : (inverse(remainingTransform) * remainingTransformForAxis).linear * direction };
    }
    return { "offset" : direction };
}

/**
 * @internal
 * TODO: Is this worth exposing?
 */
export function computePatternAxis(context is Context, axisQuery is Query, isFeaturePattern is boolean, remainingTransform is Transform)
{
    const rawDirectionResult = try(evAxis(context, { "axis" : axisQuery }));
    if (rawDirectionResult != undefined && isFeaturePattern)
    {
        var remainingTransformForAxis = getRemainderPatternTransform(context, { "references" : axisQuery });
        return inverse(remainingTransform) * remainingTransformForAxis * rawDirectionResult;
    }
    else
        return rawDirectionResult;
}

/** @internal */
export function verifyPatternSize(context is Context, id is Id, instances is number)
{
    if (instances <= 2500)
        return; //Fine
    throw regenError(ErrorStringEnum.PATTERN_INPUT_TOO_MANY_INSTANCES);
}

/**
 * @internal
 * @param patternType : Either a `PatternType` or a `FeatureType`
 * @return {boolean} : `true` if the given enum value represents a feature pattern.
 */
export function isFeaturePattern(patternType)
{
    return (patternType == PatternType.FEATURE || patternType == MirrorType.FEATURE);
}

function isPartPattern(patternType)
{
    return (patternType == PatternType.PART || patternType == MirrorType.PART);
}

function isFacePattern(patternType)
{
    return (patternType == PatternType.FACE || patternType == MirrorType.FACE);
}

/** @internal */
export function checkInput(context is Context, id is Id, definition is map, isMirror is boolean)
{
    if (isFeaturePattern(definition.patternType))
    {
        if (size(definition.instanceFunction) == 0)
            throw regenError(isMirror ? ErrorStringEnum.MIRROR_SELECT_FEATURES : ErrorStringEnum.PATTERN_SELECT_FEATURES, ["instanceFunction"]);
    }
    else if (size(evaluateQuery(context, definition.entities)) == 0)
    {
        if (isFacePattern(definition.patternType))
            throw regenError(isMirror ? ErrorStringEnum.MIRROR_SELECT_FACES : ErrorStringEnum.PATTERN_SELECT_FACES, ["faces"]);
        else
            throw regenError(isMirror ? ErrorStringEnum.MIRROR_SELECT_PARTS : ErrorStringEnum.PATTERN_SELECT_PARTS, ["entities"]);
    }
}

/** @internal */
export function processPatternBooleansIfNeeded(context is Context, id is Id, definition is map)
{
    if (isPartPattern(definition.patternType))
    {
        const reconstructOp = function(id) { opPattern(context, id, definition); };
        processNewBodyIfNeeded(context, id, definition, reconstructOp);
    }
}

/**
 * Applies the body, face, or feature pattern, given just transforms and instance names
 * @param definition {{
 *      @field patternType {PatternType}
 *      @field transforms {array} : An `array` of `Transform`s in which to place
 *              new instances.
 *      @field instanceNames {array} : An `array` of the same size as
 *              `transforms` with a `string` for each transform, used in later
 *              features to identify the entities created.
 * }}
 */
export function applyPattern(context is Context, id is Id, definition is map, remainingTransform is Transform)
{
    if (!isFeaturePattern(definition.patternType))
    {
        opPattern(context, id, definition);
        transformResultIfNecessary(context, id, remainingTransform);

        processPatternBooleansIfNeeded(context, id, definition);
    }
    else
    {
        var featureSuccessCount = 0;
        for (var i = 0; i < size(definition.transforms); i += 1)
        {
            var instanceId = id + definition.instanceNames[i];
            setFeaturePatternInstanceData(context, instanceId, {"transform" : definition.transforms[i]});
            for (var func in definition.instanceFunction)
            {
                try
                {
                    func(instanceId);
                    featureSuccessCount += 1;
                }
            }
            unsetFeaturePatternInstanceData(context, instanceId);
        }

        if (featureSuccessCount == 0) // TODO: better error
            throw regenError(ErrorStringEnum.PATTERN_FEATURE_FAILED, ["instanceFunction"]);
    }
}

