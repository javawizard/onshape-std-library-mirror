FeatureScript 189; /* Automatically generated version */
export import(path : "onshape/std/geomOperations.fs", version : "");

annotation { "Feature Type Name" : "Shell", "Filter Selector" : "allparts" }
export const shell = defineFeature(function(context is Context, id is Id, shellDefinition is map)
    precondition
    {
        annotation { "Name" : "Hollow", "Default" : false }
        shellDefinition.isHollow is boolean;

        if (!shellDefinition.isHollow)
        {
            annotation { "Name" : "Faces to remove",
                "Filter" : EntityType.FACE && BodyType.SOLID }
            shellDefinition.entities is Query;
        }
        else
        {
            annotation { "Name" : "Parts to hollow",
                "Filter" : EntityType.BODY && BodyType.SOLID }
            shellDefinition.parts is Query;
        }

        annotation { "Name" : "Shell thickness" }
        isLength(shellDefinition.thickness, SHELL_OFFSET_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        shellDefinition.oppositeDirection is boolean;
    }
    {
        if (shellDefinition.isHollow)
            shellDefinition.entities = shellDefinition.parts;

        if (!checkInput(context, id, shellDefinition))
            return;

        if (!shellDefinition.oppositeDirection)
            shellDefinition.thickness = -shellDefinition.thickness;
        opShell(context, id, shellDefinition);
    }, { oppositeDirection : false, isHollow : false });

function checkInput(context is Context, id is Id, shellDefinition is map) returns boolean
{
    if (shellDefinition.isHollow)
        shellDefinition.entities = qEntityFilter(shellDefinition.entities, EntityType.BODY);
    else
        shellDefinition.entities = qEntityFilter(shellDefinition.entities, EntityType.FACE);

    if (size(evaluateQuery(context, shellDefinition.entities)) == 0)
    {
        if (shellDefinition.isHollow)
            reportFeatureError(context, id, ErrorStringEnum.SHELL_SELECT_PARTS, ["parts"]);
        else
            reportFeatureError(context, id, ErrorStringEnum.SHELL_SELECT_FACES, ["entities"]);
        return false;
    }
    return true;
}

