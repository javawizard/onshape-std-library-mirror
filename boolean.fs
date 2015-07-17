FeatureScript 172; /* Automatically generated version */
export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/utils.fs", version : "");
export import(path : "onshape/std/moveFace.fs", version : "");

export enum BooleanOperationType
{
    annotation { "Name" : "Union" }
    UNION,
    annotation { "Name" : "Subtract" }
    SUBTRACTION,
    annotation { "Name" : "Intersect" }
    INTERSECTION,
    annotation { "Hidden" : true }
    SUBTRACT_COMPLEMENT
}

//Boolean Operation
annotation { "Feature Type Name" : "Boolean", "Filter Selector" : "allparts" }
export const booleanBodies = defineFeature(function(context is Context, id is Id, booleanDefinition is map)
    precondition
    {
        annotation { "Name" : "Operation type" }
        booleanDefinition.operationType is BooleanOperationType;
        annotation { "Name" : "Tools", "Filter" : EntityType.BODY && BodyType.SOLID }
        booleanDefinition.tools is Query;

        if (booleanDefinition.operationType == BooleanOperationType.SUBTRACTION)
        {
            annotation { "Name" : "Targets", "Filter" : EntityType.BODY && BodyType.SOLID }
            booleanDefinition.targets is Query;

            annotation { "Name" : "Offset" }
            booleanDefinition.offset is boolean;

            if (booleanDefinition.offset)
            {
                annotation { "Name" : "Offset all" }
                booleanDefinition.offsetAll is boolean;

                if (!booleanDefinition.offsetAll)
                {
                    annotation { "Name" : "Faces to offset",
                                 "Filter" : (EntityType.FACE && BodyType.SOLID) }
                    booleanDefinition.entitiesToOffset is Query;
                }

                annotation { "Name" : "Offset distance" }
                isLength(booleanDefinition.offsetDistance, SHELL_OFFSET_BOUNDS);

                annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
                booleanDefinition.oppositeDirection is boolean;

                annotation { "Name" : "Reapply fillet" }
                booleanDefinition.reFillet is boolean;
            }

            annotation { "Name" : "Keep tools" }
            booleanDefinition.keepTools is boolean;
        }
    }
    {
        if (booleanDefinition.offset && booleanDefinition.operationType == BooleanOperationType.SUBTRACTION)
        {
            if (booleanDefinition.oppositeDirection)
            {
                booleanDefinition.offsetDistance = -booleanDefinition.offsetDistance;
            }
            var suffix = "offsetTempBody";
            var transformMatrix = identityTransform();
            opPattern(context, id + suffix,
                      { "entities" : booleanDefinition.tools,
                        "transforms" : [transformMatrix],
                        "instanceNames" : ["1"] });

            var faceQuery;
            if (booleanDefinition.offsetAll)
            {
                faceQuery = qCreatedBy(id + suffix, EntityType.FACE);
            }
            else
            {
                faceQuery = wrapFaceQueryInCopy(booleanDefinition.entitiesToOffset, id + suffix);
                if (size(evaluateQuery(context, faceQuery)) == 0)
                {
                    reportFeatureError(context, id, ErrorStringEnum.BOOLEAN_OFFSET_NO_FACES, ["entitiesToOffset"]);
                    return;
                }
            }

            var tempMoveFaceSuffix = "offsetMoveFace";
            var moveFaceDefinition = {
                "moveFaces" : faceQuery,
                "moveFaceType" : MoveFaceType.OFFSET,
                "offsetDistance" : booleanDefinition.offsetDistance,
                "reFillet" : booleanDefinition.reFillet };

            opOffsetFace(context, id + tempMoveFaceSuffix, moveFaceDefinition);

            var tempBooleanDefinition = {
                "operationType" : booleanDefinition.operationType,
                "tools" : qCreatedBy(id + suffix, EntityType.BODY),
                "targets" : booleanDefinition.targets,
                "keepTools" : false };

            var tempBooleanSuffix = "tempBoolean";
            opBoolean(context, id + tempBooleanSuffix, tempBooleanDefinition);
            processSubfeatureStatus(context, id + tempBooleanSuffix, id);

            if (!booleanDefinition.keepTools)
            {
                opDeleteBodies(context, id + ".delete", { "entities" : booleanDefinition.tools });
            }
        }
        else
        {
            opBoolean(context, id, booleanDefinition);
        }
    }, { keepTools : false, offset : false, oppositeDirection : false, offsetAll : false, reFillet : false });

function wrapFaceQueryInCopy(query is Query, id is Id) returns Query
{
    if (query.queryType == QueryType.UNION)
    {
        var wrappedSubqueries = query.subqueries;
        for (var i = 0; i < @size(wrappedSubqueries); i += 1)
        {
            wrappedSubqueries[i] = wrapFaceQueryInCopy(wrappedSubqueries[i], id);
        }
        return qUnion(wrappedSubqueries);
    }
    return makeQuery(id, "COPY", EntityType.FACE, { "derivedFrom" : query, "instanceName" : "1" });
}

export enum NewBodyOperationType
{
    annotation { "Name" : "New" }
    NEW,
    annotation { "Name" : "Add" }
    ADD,
    annotation { "Name" : "Remove" }
    REMOVE,
    annotation { "Name" : "Intersect" }
    INTERSECT
}

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

/** Constructs a map with tools and targets queries for boolean operations. For operations where
 * seed needs to be part of the tools for the boolean, should set the "seed" parameter in the
 * definition.
 *
 * Arguments:
 * @param id: identifier of the tools feature
 * @param definition:
 *    .seed <Query>: original input to the seed function (optional)
 *    .operationType <NewBodyOperationType>: one of "ADD", "REMOVE", "INTERSECT"
 *    .defaultScope <boolean>: if true, targets are set to everything except the tools
 *    .booleanScope <Query>: is used as the targets if !defaultScope
 * @return map
 *    .targets <Query>: targets to use
 *    .tools <Query>: tools to use
 */
function subfeatureToolsTargets(context is Context, id is Id, definition is map) returns map
{
    // Fill defaults
    definition = mergeMaps({ "seed" : qNothing(), "defaultScope" : true }, definition);

    var output = {};
    var resultQuery = qBodyType(qCreatedBy(id, EntityType.BODY), BodyType.SOLID);
    var defaultTools = qUnion([definition.seed, resultQuery]);

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

/** Performs a boolean operation (optionally).
 *
 * Arguments:
 * @param id: identifier of the main feature
 * @param tools: query to be used for the tools
 * @param definition:
 *     .operationType <NewBodyOperationType>: if "New", nothing is done
 *     .defaultScope <boolean>: if true, indicates merge scope of "everything else"
 *     .booleanScope <Query>: is used as the targets if !defaultScope
 *     .seed <Query>: {if set, will be as a rule included in the tool section of the boolean,
 *                     (see subfeatureToolsTargets)}
 * @return {This function returns a logical boolean to indicate if, in the case of "false",
 *          the error geometry should be shown to the user. This can happen both when
 *          input is malformed and when the boolean itself fails.}
 */
export function processNewBodyIfNeeded(context is Context, id is Id, definition is map) returns boolean
{
    if (!featureHasError(context, id) && definition.operationType != NewBodyOperationType.NEW)
    {
        var booleanDefinition = subfeatureToolsTargets(context, id, definition);
        booleanDefinition.operationType = convertNewBodyOpToBoolOp(definition.operationType);
        if (size(evaluateQuery(context, booleanDefinition.tools)) == 0)
        {
            //TODO : this would probably be better to block on the UI, but that would be something
            //to do if/when we have a "Make surface" checkbox or similar indication.
            reportFeatureError(context, id, ErrorStringEnum.BOOLEAN_NEED_ONE_SOLID);
            return true;
        }

        if (size(evaluateQuery(context, booleanDefinition.targets)) > 0)
        {
            booleanDefinition.targetsAndToolsNeedGrouping = true;
            const boolId = id + "boolean";
            booleanBodies(context, boolId, booleanDefinition);
            return !processSubfeatureStatus(context, boolId, id);
        }
        else
        {
            reportFeatureError(context, id, ErrorStringEnum.BOOLEAN_NEED_ONE_SOLID, ["booleanScope"]);
            return false;
        }
    }
    else
    {
        return true;
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
            if (entry.value['type'] == "INTERFERE" ||
                entry.value['type'] == "TARGET_IN_TOOL" ||
                entry.value['type'] == "TOOL_IN_TARGET")
                toolCollisions.intersection = append(toolCollisions.intersection, entry.key);
            else if ((entry.value['type'] == "ABUT_NO_CLASS" ||
                      entry.value['type'] == "ABUT_TOOL_OUT_TARGET") &&
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
            var collisionResult = evCollision(context, { 'tools' : c.tool, 'targets' : c.target });
            if (collisionResult.error != undefined)
            {
                return false;
            }

            for (var col1 in collisionResult.result)
            {
                if (col1['type'] == "INTERFERE" ||
                    col1['type'] == "TARGET_IN_TOOL" ||
                    col1['type'] == "TOOL_IN_TARGET")
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
export function autoSelectionForBooleanStep(context is Context, featureDefinition is map, featureInfo is map) returns map
{
    var id = newId();
    var toolQ = qBodyType(qCreatedBy(id + featureInfo.featureId, EntityType.BODY), BodyType.SOLID);
    var excludeQ;
    if (featureInfo.excludeBodies is Query)
        excludeQ = qUnion([toolQ, featureInfo.excludeBodies]);
    else
        excludeQ = toolQ;
    var targetQ = qSubtraction(qBodyType(qEverything(EntityType.BODY), BodyType.SOLID), excludeQ);
    var collisionResult = evCollision(context, { tools : toolQ, targets : targetQ });
    if (collisionResult.error != undefined)
    {
        return setOperationType(featureDefinition, NewBodyOperationType.NEW, []);
    }
    var collisions = collisionResult.result;
    if (collisions is array)
    {
        var collisionClasses = classifyCollisions(context, collisions);
        var conditionsToAdd;    //undefined|boolean
        var conditionsToRemove; //undefined|boolean
        var target = [];
        for (var entry in collisionClasses)
        {
            var nIntersections = size(entry.value.intersection);
            var nAbutting = size(entry.value.abutting);
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
    }
    return setOperationType(featureDefinition, NewBodyOperationType.NEW, []);
}

// The latest implementation is simpler. If the number of parts hit by the tools is equal to 1 then we default to add.
// If the number of parts hit by the tools is other than 1 then we default to new.
export function autoSelectionForBooleanStep2(context is Context, featureDefinition is map, featureInfo is map) returns map
{
    var id = newId();
    var toolQ = qBodyType(qCreatedBy(id + featureInfo.featureId, EntityType.BODY), BodyType.SOLID);
    var excludeQ;
    if (featureInfo.excludeBodies is Query)
        excludeQ = qUnion([toolQ, featureInfo.excludeBodies]);
    else
        excludeQ = toolQ;
    var targetQ = qSubtraction(qBodyType(qEverything(EntityType.BODY), BodyType.SOLID), excludeQ);
    var collisionResult = evCollision(context, { tools : toolQ, targets : targetQ });
    if (collisionResult.error != undefined)
    {
        return setOperationType(featureDefinition, NewBodyOperationType.NEW, []);
    }
    var collisions = collisionResult.result;
    if (collisions is array && size(collisions) > 0)
    {
        var collisionClasses = classifyCollisions(context, collisions);
        var target = undefined;
        for (var entry in collisionClasses)
        {
            var collisions = concatenateArrays([entry.value.intersection, entry.value.abutting]);
            var nCollisions = size(collisions);
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
        // classifyCollisions filters out  abutting along edge, so we might come empty here
        if (target != undefined)
        {
            return setOperationType(featureDefinition, NewBodyOperationType.ADD, [target]);
        }
    }
    // No collisions, use NEW
    return setOperationType(featureDefinition, NewBodyOperationType.NEW, []);
}


function combineCollisionType(oldType, newType is string) returns string
{
    if (oldType == undefined || oldType == newType)
        return newType;
    if (oldType == "INTERFERE" || newType == "INTERFERE")
        return "INTERFERE";
    if (oldType == "TARGET_IN_TOOL" || oldType == "TOOL_IN_TARGET")
        return "INTERFERE";
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
export function flipCorrectionForRemove(context is Context, featureDefinition is map, featureInfo is map) returns map
{
    var id = newId();
    var toolQ = qBodyType(qCreatedBy(id + featureInfo.featureId, EntityType.BODY), BodyType.SOLID);
    var excludeQ;
    if (featureInfo.excludeBodies is Query)
        excludeQ = qUnion([toolQ, featureInfo.excludeBodies]);
    else
        excludeQ = toolQ;
    var targetQ = qSubtraction(qBodyType(qEverything(EntityType.BODY), BodyType.SOLID), excludeQ);
    var collisionResult = evCollision(context, { tools : toolQ, targets : targetQ });
    if (collisionResult.error != undefined)
    {
        return featureDefinition;
    }
    var collisions = collisionResult.result;
    if (collisions is array)
    {
        var collisionClasses = classifyCollisions(context, collisions);
        var haveAbutting = false; //boolean
        for (var entry in collisionClasses)
        {
            var nIntersections = size(entry.value.intersection);
            if (nIntersections > 0)
                return featureDefinition;
            var nAbutting = size(entry.value.abutting);
            if (nAbutting > 0)
                haveAbutting = true;
        }
        if (haveAbutting) // if we made it here there are no intersections
            featureDefinition.oppositeDirection = (featureDefinition.oppositeDirection == true) ? false : true;
    }
    return featureDefinition;
}

export function setBooleanErrorEntities(context is Context, id is Id, statusToolId is Id)
{
    var statusToolsQ = qBodyType(qCreatedBy(statusToolId, EntityType.BODY), BodyType.SOLID);
    if (size(evaluateQuery(context, statusToolsQ)) > 0)
    {
        var errorDefinition = {};
        errorDefinition.entities = statusToolsQ;
        setErrorEntities(context, id, errorDefinition);
        var deletionData = {};
        deletionData.entities = statusToolsQ;
        opDeleteBodies(context, statusToolId + ".delete", deletionData);
    }
}

