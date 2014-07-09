export import(path: "onshape/std/evaluate.fs", version : "");
export import(path: "onshape/std/geomUtils.fs", version : "");


annotation {"Feature Type Name" : "Sweep"}
export function sweep(context is Context, id is Id, sweepDefinition is map)
precondition
{
    annotation {"Name" : "Faces and edges to sweep", "Filter" : (GeometryType.PLANE || EntityType.EDGE || EntityType.FACE ) && ConstructionObject.NO}
    sweepDefinition.profiles is Query;
    annotation {"Name" : "Sweep path", "Filter" : EntityType.EDGE }
    sweepDefinition.path is Query;
}
{
    startFeature(context, id, sweepDefinition);
    opSweep(context, id, sweepDefinition);
    endFeature(context, id);
}

