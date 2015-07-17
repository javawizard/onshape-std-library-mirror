FeatureScript ✨; /* Automatically generated version */
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/boolean.fs", version : "");
export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/feature.fs", version : "");

annotation { "Feature Type Name" : "Sweep", "Filter Selector" : "allparts" }
export const sweep = defineFeature(function(context is Context, id is Id, sweepDefinition is map)
    precondition
    {
        annotation { "Name" : "Creation type" }
        sweepDefinition.bodyType is ToolBodyType;

        if (sweepDefinition.bodyType == ToolBodyType.SOLID)
        {
            booleanStepTypePredicate(sweepDefinition);

            annotation { "Name" : "Faces and sketch regions to sweep",
                         "Filter" : (EntityType.FACE && GeometryType.PLANE) && ConstructionObject.NO }
            sweepDefinition.profiles is Query;
        }
        else
        {
            annotation { "Name" : "Edges and sketch curves to sweep",
                         "Filter" : (EntityType.EDGE) }
            sweepDefinition.surfaceProfiles is Query;
        }

        annotation { "Name" : "Sweep path", "Filter" : EntityType.EDGE }
        sweepDefinition.path is Query;

        annotation { "Name" : "Keep profile orientation" }
        sweepDefinition.keepProfileOrientation is boolean;

        if (sweepDefinition.bodyType == ToolBodyType.SOLID)
        {
            booleanStepScopePredicate(sweepDefinition);
        }
    }
    {
        if (sweepDefinition.bodyType == ToolBodyType.SURFACE)
            sweepDefinition.profiles = sweepDefinition.surfaceProfiles;

        opSweep(context, id, sweepDefinition);

        if (sweepDefinition.bodyType == ToolBodyType.SOLID)
        {
            if (!processNewBodyIfNeeded(context, id, sweepDefinition))
            {
                var statusToolId = id + "statusTools";
                startFeature(context, statusToolId, sweepDefinition);
                opSweep(context, statusToolId, sweepDefinition);
                setBooleanErrorEntities(context, id, statusToolId);
                endFeature(context, statusToolId);
            }
        }
    }, { bodyType : ToolBodyType.SOLID, operationType : NewBodyOperationType.NEW, keepProfileOrientation : false });

