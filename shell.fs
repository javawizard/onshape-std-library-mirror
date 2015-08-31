FeatureScript 213; /* Automatically generated version */
export import(path : "onshape/std/geomOperations.fs", version : "");

annotation { "Feature Type Name" : "Shell", "Filter Selector" : "allparts" }
export const shell = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Hollow", "Default" : false }
        definition.isHollow is boolean;

        if (!definition.isHollow)
        {
            annotation { "Name" : "Faces to remove",
                "Filter" : EntityType.FACE && BodyType.SOLID }
            definition.entities is Query;
        }
        else
        {
            annotation { "Name" : "Parts to hollow",
                "Filter" : EntityType.BODY && BodyType.SOLID }
            definition.parts is Query;
        }

        annotation { "Name" : "Shell thickness" }
        isLength(definition.thickness, SHELL_OFFSET_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;
    }
    {
        if (definition.isHollow)
            definition.entities = qEntityFilter(definition.parts, EntityType.BODY);
        else
            definition.entities = qEntityFilter(definition.entities, EntityType.FACE);

        if (size(evaluateQuery(context, definition.entities)) == 0)
        {
            if (definition.isHollow)
                throw regenError(ErrorStringEnum.SHELL_SELECT_PARTS, ["parts"]);
            else
                throw regenError(ErrorStringEnum.SHELL_SELECT_FACES, ["entities"]);
        }

        if (!definition.oppositeDirection)
            definition.thickness = -definition.thickness;
        opShell(context, id, definition);
    }, { oppositeDirection : false, isHollow : false });

