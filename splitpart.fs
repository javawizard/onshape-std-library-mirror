export import(path : "onshape/std/geomUtils.fs", version : "");

//Split Part Feature
annotation {"Feature Type Name" : "Split part"}
export function splitPart(context is Context, id is Id, splitPartDefinition is map)
precondition
{
    annotation {"Name" : "Parts to split", "Filter" : EntityType.BODY && BodyType.SOLID }
    splitPartDefinition.targets is Query;
    annotation {"Name" : "Entity to split with", "Filter" : (EntityType.BODY && BodyType.SHEET) ||
                                                             (GeometryType.PLANE && ConstructionObject.YES), "MaxNumberOfPicks" : 1}
    splitPartDefinition.tool is Query;
    if( splitPartDefinition.keepTools != undefined )
    {
        annotation {"Name" : "Keep tools"}
        splitPartDefinition.keepTools is boolean;
    }
}
{
    startFeature(context, id, splitPartDefinition);
    splitPartDefinition.tool = qOwnerPart(splitPartDefinition.tool);

    if(splitPartDefinition.keepTools == undefined)
        splitPartDefinition.keepTools = false;

    opSplitPart(context, id, splitPartDefinition);
    endFeature(context, id);
}


