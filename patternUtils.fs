FeatureScript ✨; /* Automatically generated version */
// Most patterns use these
export import(path : "onshape/std/boolean.fs", version : "");
export import(path : "onshape/std/containers.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/feature.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");

export const PATTERN_OFFSET_BOUND = NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS;

export function computePatternOffset(context is Context, entity is Query, oppositeDir is boolean, distance is ValueWithUnits) returns map
{
    if (oppositeDir)
        distance = -distance;

    const rawDirectionResult = try(evAxis(context, { "axis" : entity }));
    if (rawDirectionResult == undefined)
        return { "offset" : evPlane(context, { "face" : entity }).normal * distance };
    else
        return { "offset" : rawDirectionResult.direction * distance };
}

export function verifyPatternSize(context is Context, id is Id, instances is number)
{
    if (instances <= 2500)
        return; //Fine
    throw regenError(ErrorStringEnum.PATTERN_INPUT_TOO_MANY_INSTANCES);
}

export function checkInput(context is Context, id is Id, definition is map)
{
    if (size(evaluateQuery(context, definition.entities)) == 0)
    {
        if (definition.isFacePattern)
            throw regenError(ErrorStringEnum.PATTERN_SELECT_FACES, ["faces"]);
        else
            throw regenError(ErrorStringEnum.PATTERN_SELECT_PARTS, ["entities"]);
    }
}

export function processPatternBooleansIfNeeded(context is Context, id is Id, definition is map)
{
    if (!definition.isFacePattern)
    {
        const reconstructOp = function(id) { opPattern(context, id, definition); };
        processNewBodyIfNeeded(context, id, mergeMaps(definition, { "seed" : definition.entities }), reconstructOp);
    }
}

