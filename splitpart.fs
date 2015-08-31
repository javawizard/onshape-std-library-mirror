FeatureScript ✨; /* Automatically generated version */
export import(path : "onshape/std/geomOperations.fs", version : "");

//Split Part Feature
annotation { "Feature Type Name" : "Split part", "Filter Selector" : "allparts" }
export const splitPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Parts to split", "Filter" : EntityType.BODY && BodyType.SOLID }
        definition.targets is Query;

        annotation { "Name" : "Entity to split with",
                     "Filter" : (EntityType.BODY && BodyType.SHEET) || (GeometryType.PLANE && ConstructionObject.YES),
                     "MaxNumberOfPicks" : 1 }
        definition.tool is Query;

        annotation { "Name" : "Keep tools" }
        definition.keepTools is boolean;
    }
    {
        definition.tool = qOwnerPart(definition.tool);
        opSplitPart(context, id, definition);
    }, { keepTools : false });


