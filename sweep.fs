export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/boolean.fs", version : "");
export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/feature.fs", version : "");





annotation {"Feature Type Name" : "Sweep"}
export function sweep(context is Context, id is Id, sweepDefinition is map)
precondition
{

    if(sweepDefinition.bodyType != undefined)
    {
        annotation {"Name" : "Creation type"}
        sweepDefinition.bodyType is ToolBodyType;
    }

    if (sweepDefinition.bodyType != ToolBodyType.SURFACE)
    {
        booleanStepTypePredicate(sweepDefinition);
    }

    if (sweepDefinition.bodyType != ToolBodyType.SURFACE)
    {
        annotation {"Name" : "Faces and sketch regions to sweep",
                    "Filter" : (EntityType.FACE && GeometryType.PLANE)
                               && ConstructionObject.NO}
        sweepDefinition.profiles is Query;
    }
    else
    {
        annotation {"Name" : "Edges and sketch curves to sweep",
                    "Filter" : (EntityType.EDGE)}
        sweepDefinition.surfaceProfiles is Query;
    }

    annotation {"Name" : "Sweep path", "Filter" : EntityType.EDGE }
    sweepDefinition.path is Query;


    if (sweepDefinition.bodyType != ToolBodyType.SURFACE)
    {
        booleanStepScopePredicate(sweepDefinition);
    }
}

{
    startFeature(context, id, sweepDefinition);

     if(sweepDefinition.bodyType == undefined)
        sweepDefinition.bodyType = ToolBodyType.SOLID;

    if(sweepDefinition.operationType == undefined)
       sweepDefinition.operationType = NewBodyOperationType.NEW;

    opSweep(context, id, sweepDefinition);

    if (sweepDefinition.bodyType == ToolBodyType.SOLID)
    {
        if (!processNewBodyIfNeeded(context, id, sweepDefinition))
        {
            var statusToolId = id + ".statusTools";
            opSweep(context, statusToolId, sweepDefinition);
            setBooleanErrorEntities(context, id, statusToolId);
        }
    }

    endFeature(context, id);
}



