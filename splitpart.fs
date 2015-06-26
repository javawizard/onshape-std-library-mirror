FeatureScript 156; /* Automatically generated version */
export import(path : "onshape/std/geomUtils.fs", version : "");

//Split Part Feature
annotation {"Feature Type Name" : "Split part"}
export const splitPart = defineFeature(function(context is Context, id is Id, splitPartDefinition is map)
    precondition
    {
        annotation {"Name" : "Parts to split", "Filter" : EntityType.BODY && BodyType.SOLID }
        splitPartDefinition.targets is Query;

        annotation {"Name" : "Entity to split with", "Filter" : (EntityType.BODY && BodyType.SHEET) ||
                                                                 (GeometryType.PLANE && ConstructionObject.YES), "MaxNumberOfPicks" : 1}
        splitPartDefinition.tool is Query;

        annotation {"Name" : "Keep tools"}
        splitPartDefinition.keepTools is boolean;
    }
    {
        splitPartDefinition.tool = qOwnerPart(splitPartDefinition.tool);
        opSplitPart(context, id, splitPartDefinition);
    }, { keepTools : false });


