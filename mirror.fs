export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path: "onshape/std/evaluate.fs", version : "");
export import(path: "onshape/std/transform.fs", version : "");

annotation {"Feature Type Name" : "Mirror"}
export const mirror = defineFeature(function(context is Context, id is Id, mirrorDefinition is map)
    precondition
    {
        annotation {"Name" : "Face mirror", "Default" : false}
        mirrorDefinition.isFaceMirror is boolean;

        if (!mirrorDefinition.isFaceMirror)
        {
            annotation {"Name" : "Entities to mirror", "Filter" : EntityType.BODY }
            mirrorDefinition.entities is Query;
        }
        else
        {
            annotation {"Name" : "Faces to mirror", "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
            mirrorDefinition.faces is Query;
        }

        annotation {"Name" : "Mirror plane", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1}
        mirrorDefinition.mirrorPlane is Query;
    }
    {
        const isFaceMirror = mirrorDefinition.isFaceMirror;

        if(isFaceMirror)
            mirrorDefinition.entities = mirrorDefinition.faces;

        if(size(evaluateQuery(context, mirrorDefinition.entities)) == 0)
        {
            if (isFaceMirror)
                reportFeatureError(context, id, ErrorStringEnum.MIRROR_SELECT_FACES, ["faces"]);
            else
                reportFeatureError(context, id, ErrorStringEnum.MIRROR_SELECT_PARTS, ["entities"]);
            return;
        }

        mirrorDefinition.mirrorPlane = qGeometry(mirrorDefinition.mirrorPlane, GeometryType.PLANE);
        var planeResult = evPlane(context, {"face" : mirrorDefinition.mirrorPlane});
        if (planeResult.error != undefined)
        {
            reportFeatureError(context, id, ErrorStringEnum.MIRROR_NO_PLANE, ["mirrorPlane"]);
            return;
        }

        var transform = mirrorAcross(planeResult.result);
        opPattern(context, id, {"entities" : mirrorDefinition.entities, "transforms" : [transform], "instanceNames" : ["1"],
                                notFoundErrorKey("entities") :  ErrorStringEnum.MIRROR_SELECT_PARTS });
        if(getFeatureError(context, id).result != undefined)
            reportFeatureError(context, id, isFaceMirror ? ErrorStringEnum.MIRROR_FACE_FAILED : ErrorStringEnum.MIRROR_BODY_FAILED);
    }, { isFaceMirror : false });

