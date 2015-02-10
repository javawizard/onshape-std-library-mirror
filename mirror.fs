export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path: "onshape/std/evaluate.fs", version : "");
export import(path: "onshape/std/transform.fs", version : "");

annotation {"Feature Type Name" : "Mirror"}
export const mirror = defineFeature(function(context is Context, id is Id, mirrorDefinition is map)
    precondition
    {
        annotation {"Name" : "Entities to mirror",
                "Filter" : EntityType.BODY}
        mirrorDefinition.entities is Query;

        annotation {"Name" : "Mirror plane", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1}
        mirrorDefinition.mirrorPlane is Query;
    }
    {
        mirrorDefinition.mirrorPlane = qGeometry(mirrorDefinition.mirrorPlane, GeometryType.PLANE);
        var planeResult = evPlane(context, {"face" : mirrorDefinition.mirrorPlane});
        if (planeResult.error != undefined)
        {
            reportFeatureError(context, id, ErrorStringEnum.MIRROR_NO_PLANE);
            return;
        }

        var transform = mirrorAcross(planeResult.result);
        opPattern(context, id, {"entities" : mirrorDefinition.entities, "transforms" : [transform], "instanceNames" : ["1"],
                                notFoundErrorKey("entities") :  ErrorStringEnum.MIRROR_SELECT_PARTS });
    });

