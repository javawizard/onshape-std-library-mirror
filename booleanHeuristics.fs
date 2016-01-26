FeatureScript 293; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

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
 * For Onshape internal use.
 *
 * If the number of parts hit by the tools is equal to 1 then we default to add.
 * If the number of parts hit by the tools is other than 1 then we default to new.
 */
 //TODO - remove in rel-1.42
export function autoSelectionForBooleanStep2(context is Context, featureDefinition is map, featureInfo is map) returns map
{
     return autoSelectionForBooleanStep(context, makeId(featureInfo.featureId), featureDefinition, featureInfo.excludeBodies);
}

export function booleanStepEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query, toolBodiesOp is function) returns map
{
    if (specifiedParameters.defaultScope || specifiedParameters.booleanScope)
    {
        return definition;
    }
    var scopeOnly = (specifiedParameters.operationType == true);
    if (!scopeOnly && (specifiedParameters.oppositeDirection == true) &&
        definition.oppositeDirection != oldDefinition.oppositeDirection)
    {
        scopeOnly = true;
    }
    if (scopeOnly && definition.operationType == NewBodyOperationType.NEW)
    {
        return definition;
    }
    if (definition.booleanScope is Query)
    {
        var scopeValue = evaluateQuery(context, definition.booleanScope);
        // Something is in the scope from previous heuristics
        if (scopeOnly && size(scopeValue) > 0)
        {
            return definition;
        }
    }
    var newOpDefinition = definition;
    newOpDefinition.operationType = NewBodyOperationType.NEW;
    var heuristicsId = id  + "heuristics";
    startFeature(context, heuristicsId, newOpDefinition);
    try
    {
        toolBodiesOp(context, heuristicsId + "op", newOpDefinition);
        newOpDefinition = autoSelectionForBooleanStep(context, heuristicsId, newOpDefinition, hiddenBodies);
        if (scopeOnly)
        {
            newOpDefinition.operationType = definition.operationType;
        }
    }
    catch
    {
        newOpDefinition = definition;
    }
    abortFeature(context, heuristicsId);
    return newOpDefinition;
}

/**
* Used by features using boolean heuristics
*/

export function canSetBooleanFlip (oldDefinition is map, definition is map, specifiedParameters is map) returns boolean
{
    if (specifiedParameters.booleanScope || oldDefinition.operationType == definition.operationType)
    {
        return false;
    }
    var existingTypeIsNegative = (oldDefinition.operationType == NewBodyOperationType.REMOVE ||
                    definition.operationType == NewBodyOperationType.INTERSECT);
    var newTypeIsNegative = (definition.operationType == NewBodyOperationType.REMOVE ||
                    definition.operationType == NewBodyOperationType.INTERSECT);
    return existingTypeIsNegative != newTypeIsNegative;
}

//TODO - make excludeBodies Query in rel-1.42
function autoSelectionForBooleanStep(context is Context, toolFeatureId is Id, featureDefinition is map, excludeBodies)
{
    const toolQ = qBodyType(qCreatedBy(toolFeatureId, EntityType.BODY), BodyType.SOLID);
    var excludeQ;
    if (excludeBodies is Query)
        excludeQ = qUnion([toolQ, excludeBodies]);
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

