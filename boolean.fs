FeatureScript 225; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/booleanoperationtype.gen.fs", version : "");
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/tool.fs", version : "");

// Imports used internally
import(path : "onshape/std/box.fs", version : "");
import(path : "onshape/std/clashtype.gen.fs", version : "");
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/evaluate.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/primitives.fs", version : "");
import(path : "onshape/std/transform.fs", version : "");
import(path : "onshape/std/valueBounds.fs", version : "");

/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
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
            if (definition.operationType == BooleanOperationType.SUBTRACT_COMPLEMENT &&
                isAtVersionOrLater(context, FeatureScriptVersionNumber.V179_SUBTRACT_COMPLEMENT_HANDLED_IN_FS))
            {
               definition.tools = constructToolsComplement(context, id, definition);
               definition.operationType = BooleanOperationType.SUBTRACTION;
               definition.keepTools = false;
            }
            opBoolean(context, id, definition);
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

/**  Build a block large enough to contain all tools and targets. Subtract tools from it
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
 * TODO: description
 * @param operationType
 */
export function convertNewBodyOpToBoolOp(operationType is NewBodyOperationType) returns BooleanOperationType
{
    return {
        NewBodyOperationType.ADD :       BooleanOperationType.UNION,
        NewBodyOperationType.REMOVE :    BooleanOperationType.SUBTRACTION,
        NewBodyOperationType.INTERSECT : BooleanOperationType.SUBTRACT_COMPLEMENT
    }[operationType];
}

export predicate booleanStepTypePredicate(booleanDefinition is map)
{
    annotation { "Name" : "Result body operation type" }
    booleanDefinition.operationType is NewBodyOperationType;
}

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

export predicate booleanStepPredicate(booleanDefinition is map)
{
    booleanStepTypePredicate(booleanDefinition);
    booleanStepScopePredicate(booleanDefinition);
}

/**
 * Constructs a map with tools and targets queries for boolean operations. For operations where
 * seed needs to be part of the tools for the boolean, set the "seed" parameter in the definition.
 *
 * @param context {Context}
 * @param id {Id}: identifier of the tools feature
 * @param definition {{
 *      @field operationType {NewBodyOperationType}:
 *          @eg `NewBodyOperationType.ADD` performs a boolean union
 *          @eg `NewBodyOperationType.NEW` does nothing
 *      @field defaultScope {boolean}: @optional
 *          @eg `true`  indicates merge scope of "everything else" (default)
 *          @eg `false` indicates merge scope is specified in `booleanScope`
 *      @field booleanScope {Query}: targets to use if `defaultScope` is false
 *      @field seed {Query}: if set, will be included in the tools section of the boolean
 *          @optional
 * }}
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
    const defaultTools = qUnion([definition.seed, resultQuery]);

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
 * Performs a boolean operation (optionally).
 *
 * Arguments:
 * @param id : identifier of the main feature
 * @param tools : query to be used for the tools
 * @param definition {map} : @see `subfeatureToolsTargets#definition` definition specifying how to contruct tools and targets
 *                           on subfeatureToolsTargets.
 * @param reconstructOp {function(Id)}: reconstructs the input to show to the user as error geometry
 *                                      in case the input is problematic or the boolean itself fails.
 */
export function processNewBodyIfNeeded(context is Context, id is Id, definition is map, reconstructOp is function)
{
    if (definition.operationType == NewBodyOperationType.NEW)
        return;

    const solidsQuery = qCreatedBy(id, EntityType.BODY);

    var booleanDefinition = subfeatureToolsTargets(context, id, definition);
    booleanDefinition.operationType = convertNewBodyOpToBoolOp(definition.operationType);

    if (size(evaluateQuery(context, booleanDefinition.tools)) == 0)
        throw regenError(ErrorStringEnum.BOOLEAN_NEED_ONE_SOLID, solidsQuery);

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

function classifyCollisions(context is Context, collisions is array) returns map
{
    var targetBodyToCollisionType = {};
    for (var c in collisions)
    {
        if (targetBodyToCollisionType[c.toolBody] == undefined)
            targetBodyToCollisionType[c.toolBody] = {};
        if (c.targetBody is Query)
        {
            if (targetBodyToCollisionType[c.toolBody][c.targetBody] == undefined)
            {
                targetBodyToCollisionType[c.toolBody][c.targetBody] = {};
                targetBodyToCollisionType[c.toolBody][c.targetBody]['collisions'] = [];
            }
            targetBodyToCollisionType[c.toolBody][c.targetBody]['type'] =
                combineCollisionType(targetBodyToCollisionType[c.toolBody][c.targetBody]['type'], c['type']);
            targetBodyToCollisionType[c.toolBody][c.targetBody]['collisions'] = append(targetBodyToCollisionType[c.toolBody][c.targetBody]['collisions'], c);
        }
    }

    var classifyCollisions = {};
    for (var perTool in targetBodyToCollisionType)
    {
        var toolCollisions = { 'abutting' : [], 'intersection' : [] };
        for (var entry in perTool.value)
        {
            const clash is ClashType = entry.value['type'];
            if (clash == ClashType.INTERFERE ||
                clash == ClashType.TARGET_IN_TOOL ||
                clash == ClashType.TOOL_IN_TARGET)
                toolCollisions.intersection = append(toolCollisions.intersection, entry.key);
            else if ((clash == ClashType.ABUT_NO_CLASS ||
                      clash == ClashType.ABUT_TOOL_OUT_TARGET) &&
                    faceToFaceCollisionsContainInterferences(context, entry.value['collisions']))
                toolCollisions.abutting = append(toolCollisions.abutting, entry.key);
        }
        if (size(toolCollisions.intersection) > 0 || size(toolCollisions.abutting) > 0)
            classifyCollisions[perTool.key] = toolCollisions;
    }
    return classifyCollisions;
}

function faceToFaceCollisionsContainInterferences(context is Context, collisions) returns boolean
{
    for (var c in collisions)
    {
        if (c.tool is Query && c.target is Query)
        {
            var collisionResult = try(evCollision(context, { 'tools' : c.tool, 'targets' : c.target }));
            if (collisionResult == undefined)
                return false;

            for (var col1 in collisionResult)
            {
                const clash is ClashType = col1['type'];
                if (clash == ClashType.INTERFERE ||
                    clash == ClashType.TARGET_IN_TOOL ||
                    clash == ClashType.TOOL_IN_TARGET)
                {
                    return true;
                }
            }
        }
    }
    return false;
}

// This function implements autoSelection rules. Is probably a subject to further changes.
// Current rule: If every tool abuts one and only one target Boolean operation is set to "ADD" and abutting targets go into booleanScope list
// If every tool intersects one and only one target Boolean operation is set to "REMOVE" and intersecting targets go into booleanScope list
// Otherwise Boolean operation is set to "NEW" booleanScope list is cleared out
/**
 * TODO: description
 * @param context
 * @param featureDefinition {{
 *      @field TODO
 * }}
 * @param featureInfo {{
 *      @field TODO
 * }}
 */
export function autoSelectionForBooleanStep(context is Context, featureDefinition is map, featureInfo is map) returns map
{
    const id = newId();
    const toolQ = qBodyType(qCreatedBy(id + featureInfo.featureId, EntityType.BODY), BodyType.SOLID);
    var excludeQ;
    if (featureInfo.excludeBodies is Query)
        excludeQ = qUnion([toolQ, featureInfo.excludeBodies]);
    else
        excludeQ = toolQ;
    const targetQ = qSubtraction(qBodyType(qEverything(EntityType.BODY), BodyType.SOLID), excludeQ);
    const collisions = try(evCollision(context, { tools : toolQ, targets : targetQ }));
    if (collisions == undefined)
        return setOperationType(featureDefinition, NewBodyOperationType.NEW, []);

    const collisionClasses is map = classifyCollisions(context, collisions);
    var conditionsToAdd;    //undefined|boolean
    var conditionsToRemove; //undefined|boolean
    var target = [];
    for (var entry in collisionClasses)
    {
        const nIntersections = size(entry.value.intersection);
        const nAbutting = size(entry.value.abutting);
        conditionsToRemove = (conditionsToRemove != false && nAbutting == 0 && nIntersections == 1);
        conditionsToAdd = (conditionsToAdd != false && nAbutting == 1 && nIntersections == 0);
        if (conditionsToRemove == conditionsToAdd)
            break;
        if (conditionsToRemove && !isIn(entry.value.intersection[0], target))
            target = append(target, entry.value.intersection[0]);
        if (conditionsToAdd && !isIn(entry.value.abutting[0], target))
            target = append(target, entry.value.abutting[0]);
    }
    if (conditionsToRemove == true && conditionsToAdd != true)
        return setOperationType(featureDefinition, NewBodyOperationType.REMOVE, target);
    else if (conditionsToAdd == true && conditionsToRemove != true)
        return setOperationType(featureDefinition, NewBodyOperationType.ADD, target);
    return setOperationType(featureDefinition, NewBodyOperationType.NEW, []);
}

// The latest implementation is simpler. If the number of parts hit by the tools is equal to 1 then we default to add.
// If the number of parts hit by the tools is other than 1 then we default to new.
/**
 * TODO: description
 * @param context
 * @param featureDefinition {{
 *      @field TODO
 * }}
 * @param featureInfo {{
 *      @field TODO
 * }}
 */
export function autoSelectionForBooleanStep2(context is Context, featureDefinition is map, featureInfo is map) returns map
{
    const toolQ = qBodyType(qCreatedBy(makeId(featureInfo.featureId), EntityType.BODY), BodyType.SOLID);
    var excludeQ;
    if (featureInfo.excludeBodies is Query)
        excludeQ = qUnion([toolQ, featureInfo.excludeBodies]);
    else
        excludeQ = toolQ;
    const targetQ = qSubtraction(qBodyType(qEverything(EntityType.BODY), BodyType.SOLID), excludeQ);
    const collisions = try(evCollision(context, { tools : toolQ, targets : targetQ }));
    if (collisions == undefined)
        return setOperationType(featureDefinition, NewBodyOperationType.NEW, []);

    if (size(collisions) > 0)
    {
        const collisionClasses = classifyCollisions(context, collisions);
        var target = undefined;
        for (var entry in collisionClasses)
        {
            const collisions = concatenateArrays([entry.value.intersection, entry.value.abutting]);
            const nCollisions = size(collisions);
            if (nCollisions > 1)
            {
                // Hits more than one thing. Use NEW.
                return setOperationType(featureDefinition, NewBodyOperationType.NEW, []);
            }
            else if (nCollisions == 1)
            {
                if (target != undefined && target != collisions[0])
                {
                    // Hits more than one thing. Use NEW.
                    return setOperationType(featureDefinition, NewBodyOperationType.NEW, []);
                }
                else
                {
                    target = collisions[0];
                }
            }
        }
        // classifyCollisions filters out abutting along edge, so we might come empty here
        if (target != undefined)
        {
            return setOperationType(featureDefinition, NewBodyOperationType.ADD, [target]);
        }
    }
    // No collisions, use NEW
    return setOperationType(featureDefinition, NewBodyOperationType.NEW, []);
}


function combineCollisionType(oldType, newType is ClashType) returns ClashType
precondition
{
    oldType is undefined || oldType is ClashType;
}
{
    if (oldType == undefined || oldType == newType)
        return newType;
    if (oldType == ClashType.INTERFERE || newType == ClashType.INTERFERE)
        return ClashType.INTERFERE;
    if (oldType == ClashType.TARGET_IN_TOOL || oldType == ClashType.TOOL_IN_TARGET)
        return ClashType.INTERFERE;
    return oldType;
}

function setOperationType(featureDef is map, opType is NewBodyOperationType, targets is array)
{
    featureDef.operationType = opType;
    if (opType != NewBodyOperationType.NEW)
    {
        featureDef.defaultScope = false;
        featureDef.booleanScope = qUnion(targets);
    }
    else
    {
        featureDef.defaultScope = false;
        featureDef.booleanScope = qUnion([]);
    }
    return featureDef;
}

//This function implements flip on Remove heuristics.
//If tool bodies don't intersect with targets but there are abuttings, flip the direction
/**
 * TODO: description
 * @param context
 * @param featureDefinition {{
 *      @field TODO
 * }}
 * @param featureInfo {{
 *      @field TODO
 * }}
 */
export function flipCorrectionForRemove(context is Context, featureDefinition is map, featureInfo is map) returns map
{
    const toolQ = qBodyType(qCreatedBy(newId() + featureInfo.featureId, EntityType.BODY), BodyType.SOLID);
    var excludeQ;
    if (featureInfo.excludeBodies is Query)
        excludeQ = qUnion([toolQ, featureInfo.excludeBodies]);
    else
        excludeQ = toolQ;
    const targetQ = qSubtraction(qBodyType(qEverything(EntityType.BODY), BodyType.SOLID), excludeQ);
    const collisions = try(evCollision(context, { tools : toolQ, targets : targetQ }));
    if (collisions == undefined)
    {
        return featureDefinition;
    }
    const collisionClasses = classifyCollisions(context, collisions);
    var haveAbutting = false; //boolean
    for (var entry in collisionClasses)
    {
        const nIntersections = size(entry.value.intersection);
        if (nIntersections > 0)
            return featureDefinition;
        const nAbutting = size(entry.value.abutting);
        if (nAbutting > 0)
            haveAbutting = true;
    }
    if (haveAbutting) // if we made it here there are no intersections
        featureDefinition.oppositeDirection = (featureDefinition.oppositeDirection == true) ? false : true;

    return featureDefinition;
}

/**
 * TODO: description
 * @param context
 * @param id
 * @param statusToolId
 */
export function setBooleanErrorEntities(context is Context, id is Id, statusToolId is Id)
{
    const statusToolsQ = qBodyType(qCreatedBy(statusToolId, EntityType.BODY), BodyType.SOLID);
    if (size(evaluateQuery(context, statusToolsQ)) > 0)
    {
        const errorDefinition = { entities : statusToolsQ };
        setErrorEntities(context, id, errorDefinition);
        const deletionData = { entities : statusToolsQ };
        opDeleteBodies(context, statusToolId + "delete", deletionData);
    }
}

