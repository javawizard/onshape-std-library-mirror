FeatureScript 213; /* Automatically generated version */
export import(path : "onshape/std/geomOperations.fs", version : "");
export import(path : "onshape/std/feature.fs", version : "");
export import(path : "onshape/std/boolean.fs", version : "");
export import(path : "onshape/std/manipulator.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");

//Thicken Feature
annotation { "Feature Type Name" : "Thicken", "Filter Selector" : "allparts" }
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
            var temp = definition.thickness2;
            definition.thickness2 = definition.thickness1;
            definition.thickness1 = temp;
        }

        // ------------- Perform the operation ---------------
        opThicken(context, id, definition);

        const reconstructOp = function(id) { opThicken(context, id, definition); };
        processNewBodyIfNeeded(context, id, definition, reconstructOp);
    }, { oppositeDirection : false, operationType : NewBodyOperationType.NEW });

