FeatureScript 477; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "477.0");
export import(path : "onshape/std/tool.fs", version : "477.0");

// Features using manipulators must export these.
export import(path : "onshape/std/manipulator.fs", version : "477.0");
export import(path : "onshape/std/tool.fs", version : "477.0");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "477.0");
import(path : "onshape/std/booleanHeuristics.fs", version : "477.0");
import(path : "onshape/std/evaluate.fs", version : "477.0");
import(path : "onshape/std/feature.fs", version : "477.0");
import(path : "onshape/std/valueBounds.fs", version : "477.0");

const THICKEN_BOUNDS =
{
    (meter)      : [0.0, 0.005, 500],
    (centimeter) : 0.5,
    (millimeter) : 5.0,
    (inch)       : 0.25,
    (foot)       : 0.025,
    (yard)       : 0.01
} as LengthBoundSpec;

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
        isLength(definition.thickness1, THICKEN_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
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
 * implements heuristics for thicken feature
 */
export function thickenEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    // If flip has not been specified and there is no second direction we can adjust flip based on boolean operation
    if (!specifiedParameters.oppositeDirection && definition.thickness2 > 0)
    {
        if (canSetBooleanFlip(oldDefinition, definition, specifiedParameters))
        {
            definition.oppositeDirection = !definition.oppositeDirection;
        }
    }
    return booleanStepEditLogic(context, id, oldDefinition, definition,
                                specifiedParameters, hiddenBodies, thicken);
}

