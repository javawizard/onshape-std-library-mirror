FeatureScript 718; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

export import(path: "onshape/std/patternCommon.fs", version : "718.0");

// Most patterns use these
export import(path : "onshape/std/boolean.fs", version : "718.0");
export import(path : "onshape/std/containers.fs", version : "718.0");
export import(path : "onshape/std/evaluate.fs", version : "718.0");
export import(path : "onshape/std/feature.fs", version : "718.0");
export import(path : "onshape/std/featureList.fs", version : "718.0");
export import(path : "onshape/std/valueBounds.fs", version : "718.0");

import(path : "onshape/std/mathUtils.fs", version : "718.0");
import(path : "onshape/std/sheetMetalPattern.fs", version : "718.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "718.0");
import(path : "onshape/std/topologyUtils.fs", version : "718.0");

/** @internal */
export const PATTERN_OFFSET_BOUND = NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS;

/**
 * @internal
 * Preprocess the entities and instance function for pattern
 */
export function adjustPatternDefinitionEntities(context is Context, definition is map, isMirror is boolean) returns map
{
    if (isFacePattern(definition.patternType))
        definition.entities = definition.faces;
    else if (isFeaturePattern(definition.patternType) && isAtVersionOrLater(context, FeatureScriptVersionNumber.V666_FEATURE_PATTERN_ENTITIES))
        definition.entities = qNothing();

    checkPatternInput(context, definition, isMirror);

    return definition;
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

/** @internal */
function checkPatternInput(context is Context, definition is map, isMirror is boolean)
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
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V693_SM_PATTERN))
        {
            sheetMetalAwareGeometryPattern(context, id, definition, remainingTransform);
        }
        else
        {
            geometryPattern(context, id, definition, remainingTransform);
        }
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
                    if (e is map && try silent(e.message as ErrorStringEnum) == ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN)
                    {
                        throw regenError(ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN, ["instanceFunction"]);
                    }
                }
            }
            unsetFeaturePatternInstanceData(context, instanceId);
        }

        if (featureSuccessCount == 0) // TODO: better error
            throw regenError(ErrorStringEnum.PATTERN_FEATURE_FAILED, ["instanceFunction"]);
    }
}

/**
 * Perform a face or body pattern as described by the definition, then transform and boolean the result if necessary.
 */
function geometryPattern(context is Context, id is Id, definition is map, remainingTransform is Transform)
{
    opPattern(context, id, definition);
    transformResultIfNecessary(context, id, remainingTransform);

    processPatternBooleansIfNeeded(context, id, definition);
}

/**
 * Split the input entities of the pattern and perform face and body patterns for standard entities and sheet metal entities.
 */
function sheetMetalAwareGeometryPattern(context is Context, id is Id, definition is map, remainingTransform is Transform)
{
    var separatedQueries = separateSheetMetalQueries(context, definition.entities);
    var hasNonSheetMetalQueries = size(evaluateQuery(context, separatedQueries.nonSheetMetalQueries)) > 0;
    var hasSheetMetalQueries = size(evaluateQuery(context, separatedQueries.sheetMetalQueries)) > 0;

    if (hasNonSheetMetalQueries)
    {
        definition.entities = separatedQueries.nonSheetMetalQueries;
        geometryPattern(context, id, definition, remainingTransform);
    }
    if (hasSheetMetalQueries)
    {
        definition.entities = separatedQueries.sheetMetalQueries;
        definition.topLevelId = id;
        sheetMetalGeometryPattern(context, id + "smPattern", definition);
    }
}

