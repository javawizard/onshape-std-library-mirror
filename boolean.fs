FeatureScript 392; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/booleanoperationtype.gen.fs", version : "392.0");
export import(path : "onshape/std/query.fs", version : "392.0");
export import(path : "onshape/std/tool.fs", version : "392.0");

// Imports used internally
import(path : "onshape/std/box.fs", version : "392.0");
import(path : "onshape/std/clashtype.gen.fs", version : "392.0");
import(path : "onshape/std/containers.fs", version : "392.0");
import(path : "onshape/std/evaluate.fs", version : "392.0");
import(path : "onshape/std/feature.fs", version : "392.0");
import(path : "onshape/std/primitives.fs", version : "392.0");
import(path : "onshape/std/transform.fs", version : "392.0");
import(path : "onshape/std/valueBounds.fs", version : "392.0");

/**
 * The boolean feature.  Performs an `opBoolean` after a possible `opOffsetFaces` if the operation is subtraction.
 */
annotation { "Feature Type Name" : "Boolean", "Filter Selector" : "allparts" }
export const booleanBodies = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Operation type" }
        definition.operationType is BooleanOperationType;
        annotation { "Name" : "Tools", "Filter" : EntityType.BODY && BodyType.SOLID }
        definition.tools is Query;

        if (definition.operationType == BooleanOperationType.SUBTRACTION)
        {
            annotation { "Name" : "Targets", "Filter" : EntityType.BODY && BodyType.SOLID }
            definition.targets is Query;

            annotation { "Name" : "Offset" }
            definition.offset is boolean;

            if (definition.offset)
            {
                annotation { "Name" : "Offset all" }
                definition.offsetAll is boolean;

                if (!definition.offsetAll)
                {
                    annotation { "Name" : "Faces to offset",
                                 "Filter" : (EntityType.FACE && BodyType.SOLID) }
                    definition.entitiesToOffset is Query;
                }

                annotation { "Name" : "Offset distance" }
                isLength(definition.offsetDistance, SHELL_OFFSET_BOUNDS);

                annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
                definition.oppositeDirection is boolean;

                annotation { "Name" : "Reapply fillet" }
                definition.reFillet is boolean;
            }
        }
        if (definition.operationType == BooleanOperationType.SUBTRACTION || definition.operationType == BooleanOperationType.INTERSECTION)
        {
            annotation { "Name" : "Keep tools" }
            definition.keepTools is boolean;
        }
    }
    {
        if (definition.offset && definition.operationType == BooleanOperationType.SUBTRACTION)
        {
            if (definition.oppositeDirection)
            {
                definition.offsetDistance = -definition.offsetDistance;
            }
            const suffix = "offsetTempBody";
            const transformMatrix = identityTransform();
            opPattern(context, id + suffix,
                      { "entities" : definition.tools,
                        "transforms" : [transformMatrix],
                        "instanceNames" : ["1"] });

            var faceQuery;
            if (definition.offsetAll)
            {
                faceQuery = qCreatedBy(id + suffix, EntityType.FACE);
            }
            else
            {
                faceQuery = wrapFaceQueryInCopy(definition.entitiesToOffset, id + suffix);
                if (size(evaluateQuery(context, faceQuery)) == 0)
                    throw regenError(ErrorStringEnum.BOOLEAN_OFFSET_NO_FACES, ["entitiesToOffset"]);
            }

            const tempMoveFaceSuffix = "offsetMoveFace";
            const moveFaceDefinition = {
                "moveFaces" : faceQuery,
                "moveFaceType" : MoveFaceType.OFFSET,
                "offsetDistance" : definition.offsetDistance,
                "reFillet" : definition.reFillet };

            opOffsetFace(context, id + tempMoveFaceSuffix, moveFaceDefinition);

            const tempBooleanDefinition = {
                "operationType" : definition.operationType,
                "tools" : qCreatedBy(id + suffix, EntityType.BODY),
                "targets" : definition.targets,
                "keepTools" : false };

            const tempBooleanSuffix = "tempBoolean";
            opBoolean(context, id + tempBooleanSuffix, tempBooleanDefinition);
            processSubfeatureStatus(context, id + tempBooleanSuffix, id);

            if (!definition.keepTools)
            {
                opDeleteBodies(context, id + "delete", { "entities" : definition.tools });
            }
        }
        else
        {
            var isSubtractComplement = false;
            if (definition.operationType == BooleanOperationType.SUBTRACT_COMPLEMENT &&
                isAtVersionOrLater(context, FeatureScriptVersionNumber.V179_SUBTRACT_COMPLEMENT_HANDLED_IN_FS))
            {
                isSubtractComplement = true;
                definition.tools = constructToolsComplement(context, id, definition);
                definition.operationType = BooleanOperationType.SUBTRACTION;
                definition.keepTools = false;
            }
            opBoolean(context, id, definition);
            if (isSubtractComplement)
            {
                var errorMessage = getFeatureInfo(context, id);
                if (errorMessage == ErrorStringEnum.BOOLEAN_SUBTRACT_NO_OP)
                {
                    reportFeatureInfo(context, id,  ErrorStringEnum.BOOLEAN_INTERSECT_NO_OP);
                }
            }
        }
    }, { keepTools : false, offset : false, oppositeDirection : false, offsetAll : false, reFillet : false });

function wrapFaceQueryInCopy(query is Query, id is Id) returns Query
{
    if (query.queryType == QueryType.UNION)
    {
        return qUnion(mapArray(query.subqueries, function(q) { return wrapFaceQueryInCopy(q, id); }));
    }
    return makeQuery(id, "COPY", EntityType.FACE, { "derivedFrom" : query, "instanceName" : "1" });
}

/**
 * Build a block large enough to contain all tools and targets. Subtract tools from it.
 */
function constructToolsComplement(context is Context, id is Id, booleanDefinition is map) returns Query
{
    const inputTools = evaluateQuery(context, booleanDefinition.tools); // save tools here to avoid qCreatedBy confusion

    const boxResult = evBox3d(context, {"topology" : qUnion([booleanDefinition.tools, booleanDefinition.targets])});
    const extendedBox is Box3d = extendBox3d(boxResult, 0. * meter, 0.1);
    const boxId is Id = id + "containingBox";
    fCuboid(context, boxId, {"corner1" : extendedBox.minCorner, "corner2" : extendedBox.maxCorner});

    const complementId = id + "toolComplement";
    const complementDefinition = {
                "operationType" : BooleanOperationType.SUBTRACTION,
                "tools" : qUnion(inputTools),
                "targets" : qCreatedBy(boxId, EntityType.BODY),
                "keepTools" : booleanDefinition.keepTools };
    opBoolean(context, complementId, complementDefinition);
    return qCreatedBy(boxId, EntityType.BODY); // Subtraction modifies target tool
}

/**
 * Maps a `NewBodyOperationType` (used in features like `extrude`) to its corresponding `BooleanOperationType`.
 */
export function convertNewBodyOpToBoolOp(operationType is NewBodyOperationType) returns BooleanOperationType
{
    return {
        NewBodyOperationType.ADD :       BooleanOperationType.UNION,
        NewBodyOperationType.REMOVE :    BooleanOperationType.SUBTRACTION,
        NewBodyOperationType.INTERSECT : BooleanOperationType.SUBTRACT_COMPLEMENT
    }[operationType];
}

/**
 * Predicate which specifies a field `operationType` of type `NewBodyOperationType`.
 * Used by body-creating feature preconditions such as extrude, revolve, sweep or loft.
 *
 * When used in a precondition, `NewBodyOperationType` creates UI like the extrude
 * feature, with a horizontal list of the words "New", "Add", etc. When using this
 * predicate in features, make sure to export an import of `tool.fs` so that `NewBodyOperationType`
 * is visible to the Part Studios:
 * ```
 * export import(path : "onshape/std/tool.fs", version : "");
 * ```
 *
 * @param booleanDefinition : @autocomplete `definition`
 */
export predicate booleanStepTypePredicate(booleanDefinition is map)
{
    annotation { "Name" : "Result body operation type" }
    booleanDefinition.operationType is NewBodyOperationType;
}

/**
 * Used by body-creating feature preconditions to allow post-creation booleans,
 * specifying the merge scope (or "Merge with all") for that boolean.
 *
 * Designed to be used together with `booleanStepTypePredicate`.
 *
 * @param booleanDefinition : @autocomplete `definition`
 */
export predicate booleanStepScopePredicate(booleanDefinition is map)
{
    if (booleanDefinition.operationType != NewBodyOperationType.NEW)
    {
        if (booleanDefinition.defaultScope != undefined)
        {
            annotation { "Name" : "Merge with all", "Default" : false }
            booleanDefinition.defaultScope is boolean;
            if (booleanDefinition.defaultScope != true)
            {
                annotation { "Name" : "Merge scope", "Filter" : EntityType.BODY && BodyType.SOLID }
                booleanDefinition.booleanScope is Query;
            }
        }
    }
}

/**
 * Constructs a map with tools and targets queries for boolean operations. For operations where
 * seed needs to be part of the tools for the boolean, set the "seed" parameter in the definition.
 *
 * @param context {Context}
 * @param id {Id}: identifier of the tools feature
 * @param definition {map} : See `definition` of `preocessNewBodyIfNeeded` for details.
 * @returns {{
 *    @field targets {Query}: targets to use
 *    @field tools {Query}: tools to use
 * }}
 */
function subfeatureToolsTargets(context is Context, id is Id, definition is map) returns map
{
    // Fill defaults
    definition = mergeMaps({ "seed" : qNothing(), "defaultScope" : true }, definition);
    const resultQuery = qBodyType(qCreatedBy(id, EntityType.BODY), BodyType.SOLID);

    var seedQuery = definition.seed;
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V263_SURFACE_PATTERN_BOOLEAN))
        seedQuery = qBodyType(definition.seed, BodyType.SOLID);

    const defaultTools = qUnion([seedQuery, resultQuery]);

    var output = {};
    output.tools = defaultTools;
    output.targets = (definition.defaultScope != false) ? qBodyType(qEverything(EntityType.BODY), BodyType.SOLID) : definition.booleanScope;
    output.targets = qSubtraction(output.targets, defaultTools);

    // We treat boolean slightly differently, as tools/targets are in select cases interchangeable.
    // (This logic comes from the fact that grouping of tools/targets has a significant effect on output.)
    if (definition.operationType == NewBodyOperationType.ADD &&
        size(evaluateQuery(context, output.targets)) <= 0)
    {
        output.tools = resultQuery;
        output.targets = definition.seed;
    }

    return output;
}

/**
 * Performs a boolean operation (optionally). Used by body-creating features (like `extrude`) as the boolean step.
 * On top of the regular boolean feature, converts the `operationType` and creates error bodies on failure.
 *
 * @param id : identifier of the main feature
 * @param definition {{
 *      @field operationType {NewBodyOperationType}:
 *              @eg `NewBodyOperationType.ADD` performs a boolean union
 *              @eg `NewBodyOperationType.NEW` does nothing
 *      @field defaultScope {boolean}: @optional
 *              @eg `true`  indicates merge scope of "everything else" (default)
 *              @eg `false` indicates merge scope is specified in `booleanScope`
 *      @field booleanScope {Query}: targets to use if `defaultScope` is false
 *      @field seed {Query}: @optional
 *              If set, will be included in the tools section of the boolean.
 * }}
 * @param reconstructOp {function}: A function which takes in an Id, and reconstructs the input to show to the user as error geometry
 *          in case the input is problematic or the boolean itself fails.
 */
export function processNewBodyIfNeeded(context is Context, id is Id, definition is map, reconstructOp is function)
{
    if (definition.operationType == NewBodyOperationType.NEW)
        return;

    const solidsQuery = qCreatedBy(id, EntityType.BODY);

    var booleanDefinition = subfeatureToolsTargets(context, id, definition);
    booleanDefinition.operationType = convertNewBodyOpToBoolOp(definition.operationType);

    if (size(evaluateQuery(context, booleanDefinition.tools)) == 0)
    {
        var errorEnum = ErrorStringEnum.BOOLEAN_NEED_ONE_SOLID;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V263_SURFACE_PATTERN_BOOLEAN))
        {
            errorEnum = ErrorStringEnum.FEATURE_NO_SOLIDS;
        }
        throw regenError(errorEnum, solidsQuery);
    }
    if (size(evaluateQuery(context, booleanDefinition.targets)) == 0)
        throw regenError(ErrorStringEnum.BOOLEAN_NEED_ONE_SOLID, ["booleanScope"], solidsQuery);

    booleanDefinition.targetsAndToolsNeedGrouping = true;
    const boolId = id + "boolean";
    booleanBodies(context, boolId, booleanDefinition);
    if (getFeatureWarning(context, boolId) != undefined || getFeatureInfo(context, boolId) != undefined)
    {
        processSubfeatureStatus(context, boolId, id);

        const errorId = id + "errorEntities";
        reconstructOp(errorId);
        setErrorEntities(context, id, { "entities" : qCreatedBy(errorId, EntityType.BODY) });
        opDeleteBodies(context, id + "delete", { "entities" : qCreatedBy(errorId, EntityType.BODY) });
    }
}

