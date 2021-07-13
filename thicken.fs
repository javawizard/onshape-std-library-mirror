FeatureScript 1549; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "1549.0");
export import(path : "onshape/std/tool.fs", version : "1549.0");

// Features using manipulators must export these.
export import(path : "onshape/std/manipulator.fs", version : "1549.0");
export import(path : "onshape/std/tool.fs", version : "1549.0");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "1549.0");
import(path : "onshape/std/booleanHeuristics.fs", version : "1549.0");
import(path : "onshape/std/evaluate.fs", version : "1549.0");
import(path : "onshape/std/feature.fs", version : "1549.0");
import(path : "onshape/std/valueBounds.fs", version : "1549.0");


/**
 * Feature performing an [opThicken], followed by an [opBoolean]. For simple thickens, prefer using
 * [opThicken] directly.
 */
annotation { "Feature Type Name" : "Thicken",
             "Filter Selector" : "allparts",
             "Editing Logic Function" : "thickenEditLogic" }
export const thicken = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        booleanStepTypePredicate(definition);

        annotation { "Name" : "Faces and surfaces to thicken",
                    "Filter" : (EntityType.FACE || (BodyType.SHEET && EntityType.BODY))
                        && ConstructionObject.NO }
        definition.entities is Query;

        annotation { "Name" : "Direction 1" }
        isLength(definition.thickness1, ZERO_INCLUSIVE_OFFSET_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Direction 2" }
        isLength(definition.thickness2, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

        booleanStepScopePredicate(definition);
    }
    {
        // ------------- Determine the direction ---------------
        if (definition.oppositeDirection)
        {
            const temp = definition.thickness2;
            definition.thickness2 = definition.thickness1;
            definition.thickness1 = temp;
        }

        // ------------- Perform the operation ---------------
        var remainingTransform = getRemainderPatternTransform(context,
                {"references" : definition.entities});
        opThicken(context, id, definition);
        transformResultIfNecessary(context, id, remainingTransform);

        const reconstructOp = function(id) {
            opThicken(context, id, definition);
            transformResultIfNecessary(context, id, remainingTransform);
        };
        processNewBodyIfNeeded(context, id, definition, reconstructOp);
    }, { oppositeDirection : false, operationType : NewBodyOperationType.NEW });

/**
 * @internal
 * Implements heuristics for thicken feature.
 */
export function thickenEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map) returns map
{
    if (specifiedParameters.thickness2)
    {
        return definition;
    }

    // If flip has not been specified and there is no second direction we can adjust flip based on boolean operation
    if (!specifiedParameters.oppositeDirection)
    {
        if (canSetBooleanFlip(oldDefinition, definition, specifiedParameters))
        {
            definition.oppositeDirection = !definition.oppositeDirection;
        }
    }

    const logicMap = booleanStepEditLogicAnalysis(context, oldDefinition, definition, specifiedParameters);
    if (!logicMap.canDefineOperation && !logicMap.canDefineScope)
    {
        return definition;
    }
    const facesOfSolids = qBodyType(qEntityFilter(definition.entities, EntityType.FACE), BodyType.SOLID);
    const booleanScope = evaluateQuery(context, qOwnerBody(facesOfSolids));
    if (logicMap.canDefineOperation && booleanScope != [])
    {
        definition.operationType = (definition.oppositeDirection) ? NewBodyOperationType.REMOVE : NewBodyOperationType.ADD;
    }
    if (logicMap.canDefineScope)
    {
        definition.booleanScope = qUnion(booleanScope);
    }
    return definition;
}

