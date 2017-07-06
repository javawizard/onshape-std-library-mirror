FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Most patterns use these
export import(path : "onshape/std/boolean.fs", version : "✨");
export import(path : "onshape/std/containers.fs", version : "✨");
export import(path : "onshape/std/evaluate.fs", version : "✨");
export import(path : "onshape/std/feature.fs", version : "✨");
export import(path : "onshape/std/featureList.fs", version : "✨");
export import(path : "onshape/std/valueBounds.fs", version : "✨");

import(path : "onshape/std/mathUtils.fs", version : "✨");
import(path : "onshape/std/topologyUtils.fs", version : "✨");

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
 * @seealso [PatternType]
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
    withTransform is boolean, remainingTransform is Transform) returns map
{
    if (oppositeDir)
        distance = -distance;

    var direction = extractDirection(context, entity);
    if (direction == undefined)
        throw "Offset direction could not be computed";
    var offset = direction * distance;

    if (withTransform)
    {
        var remainingTransformForAxis = getRemainderPatternTransform(context, { "references" : entity });
        return { "offset" : (inverse(remainingTransform) * remainingTransformForAxis).linear * offset };
    }
    return { "offset" : offset };
}

/**
 * @internal
 * TODO: Is this worth exposing?
 */
export function computePatternAxis(context is Context, axisQuery is Query, withTransform is boolean, remainingTransform is Transform)
{
    const rawDirectionResult = try(evAxis(context, { "axis" : axisQuery }));
    if (rawDirectionResult != undefined && withTransform)
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
        if (undefined != definition.surfaceJoinMatches && size(definition.surfaceJoinMatches) > 0)
        {
            joinSurfaceBodies(context, id, definition.surfaceJoinMatches, false, reconstructOp);
            if (size(evaluateQuery(context, qBodyType(qCreatedBy(id, EntityType.BODY), BodyType.SOLID))) == 0)
            {
                return;
            }
        }
        processNewBodyIfNeeded(context, id, definition, reconstructOp);
    }
}

/**
 * Applies the body, face, or feature pattern, given just transforms and instance names
 * @param definition {{
 *      @field patternType {PatternType}
 *      @field entities {Query} : @requiredif{`patternType` is not `FEATURE`} The faces or parts to pattern.
 *      @field instanceFunction {FeatureList} : @requiredif{`patternType` is `FEATURE`} The features to pattern.
 *      @field transforms {array} : An `array` of [Transform]s in which to place
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
                catch (e)
                {
                    if (e is map && e.message == ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN)
                    {
                        throw regenError(e.message, ["instanceFunction"]);
                    }
                }
            }
            unsetFeaturePatternInstanceData(context, instanceId);
        }

        if (featureSuccessCount == 0) // TODO: better error
            throw regenError(ErrorStringEnum.PATTERN_FEATURE_FAILED, ["instanceFunction"]);
    }
}

