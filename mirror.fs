export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path: "onshape/std/evaluate.fs", version : "");
export import(path:"onshape/std/transform.fs", version : "");

annotation {"Feature Type Name" : "Mirror"}
export function mirror(context is Context, id is Id, mirrorDefinition is map)
precondition
{
    annotation {"Name" : "Entities to mirror",
            "Filter" : EntityType.BODY}
    mirrorDefinition.entities is Query;
    annotation {"Name" : "Mirror plane", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1}
    mirrorDefinition.mirrorPlane is Query;
}
{
    startFeature(context, id, mirrorDefinition);
    mirrorDefinition.mirrorPlane = qGeometry(mirrorDefinition.mirrorPlane, GeometryType.PLANE);
    var planeResult = evPlane(context, {"face" : mirrorDefinition.mirrorPlane});
    if(reportFeatureError(context, id, planeResult.error))
        return;

    var transform = mirrorAcross(planeResult.result);
    opPattern(context, id, {"entities" : mirrorDefinition.entities, "transforms" : [transform], "instanceNames" : ["1"]});
    endFeature(context, id);
}

