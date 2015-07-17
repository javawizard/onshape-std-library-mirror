FeatureScript 172; /* Automatically generated version */
export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/feature.fs", version : "");
export import(path : "onshape/std/boolean.fs", version : "");
export import(path : "onshape/std/manipulator.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");

//Thicken Feature
annotation { "Feature Type Name" : "Thicken", "Filter Selector" : "allparts" }
export const thicken = defineFeature(function(context is Context, id is Id, thickenDefinition is map)
    precondition
    {
        booleanStepTypePredicate(thickenDefinition);

        annotation { "Name" : "Faces and surfaces to thicken",
                    "Filter" : (EntityType.FACE || (BodyType.SHEET && EntityType.BODY))
                        && ConstructionObject.NO }
        thickenDefinition.entities is Query;

        annotation { "Name" : "Direction 1" }
        isLength(thickenDefinition.thickness1, THICKEN_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        thickenDefinition.oppositeDirection is boolean;

        annotation { "Name" : "Direction 2" }
        isLength(thickenDefinition.thickness2, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

        booleanStepScopePredicate(thickenDefinition);
    }
    {
        // ------------- Determine the direction ---------------
        if (thickenDefinition.oppositeDirection)
        {
            var temp = thickenDefinition.thickness2;
            thickenDefinition.thickness2 = thickenDefinition.thickness1;
            thickenDefinition.thickness1 = temp;
        }

        // ------------- Perform the operation ---------------
        opThicken(context, id, thickenDefinition);

        if (!processNewBodyIfNeeded(context, id, thickenDefinition))
        {
            var statusToolId = id + "statusTools";
            startFeature(context, statusToolId, thickenDefinition);
            opThicken(context, statusToolId, thickenDefinition);
            setBooleanErrorEntities(context, id, statusToolId);
            endFeature(context, statusToolId);
        }
    }, { oppositeDirection : false, operationType : NewBodyOperationType.NEW });

