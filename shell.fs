export import(path : "onshape/std/geomUtils.fs", version : "");

annotation {"Feature Type Name" : "Shell"}
export function shell(context is Context, id is Id, shellDefinition is map)
precondition
{
    annotation {"Name" : "Faces to shell",
                "Filter": EntityType.FACE && BodyType.SOLID }
    shellDefinition.entities is Query;
    annotation {"Name" : "Shell thickness"}
    isLength(shellDefinition.thickness, SHELL_OFFSET_BOUNDS);
    annotation {"Name" : "Opposite direction", "UIHint" : "OppositeDirection"}
    shellDefinition.oppositeDirection is boolean;
}
{
    startFeature(context, id, shellDefinition);
    if(!shellDefinition.oppositeDirection)
        shellDefinition.thickness = -shellDefinition.thickness;
    opShell(context, id, shellDefinition);
    endFeature(context, id);
}

