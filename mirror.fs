FeatureScript ✨; /* Automatically generated version */
export import(path : "onshape/std/geomOperations.fs", version : "");
export import(path : "onshape/std/boolean.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/transform.fs", version : "");

annotation { "Feature Type Name" : "Mirror", "Filter Selector" : "allparts" }
export const mirror = defineFeature(function(context is Context, id is Id, mirrorDefinition is map)
    precondition
    {
        annotation { "Name" : "Face mirror", "Default" : false }
        mirrorDefinition.isFaceMirror is boolean;

        if (!mirrorDefinition.isFaceMirror)
        {
            booleanStepTypePredicate(mirrorDefinition);

            annotation { "Name" : "Entities to mirror", "Filter" : EntityType.BODY }
            mirrorDefinition.entities is Query;
        }
        else
        {
            annotation { "Name" : "Faces to mirror", "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
            mirrorDefinition.faces is Query;
        }

        annotation { "Name" : "Mirror plane", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1 }
        mirrorDefinition.mirrorPlane is Query;

        if (!mirrorDefinition.isFaceMirror)
        {
            booleanStepScopePredicate(mirrorDefinition);
        }
    }
    {
        const isFaceMirror = mirrorDefinition.isFaceMirror;

        if (isFaceMirror)
            mirrorDefinition.entities = mirrorDefinition.faces;

        if (size(evaluateQuery(context, mirrorDefinition.entities)) == 0)
        {
            if (isFaceMirror)
                reportFeatureError(context, id, ErrorStringEnum.MIRROR_SELECT_FACES, ["faces"]);
            else
                reportFeatureError(context, id, ErrorStringEnum.MIRROR_SELECT_PARTS, ["entities"]);
            return;
        }

        mirrorDefinition.mirrorPlane = qGeometry(mirrorDefinition.mirrorPlane, GeometryType.PLANE);
        var planeResult = evPlane(context, { "face" : mirrorDefinition.mirrorPlane });
        if (planeResult.error != undefined)
        {
            reportFeatureError(context, id, ErrorStringEnum.MIRROR_NO_PLANE, ["mirrorPlane"]);
            return;
        }

        var transform = mirrorAcross(planeResult.result);
        var patternDefinition = {
            "entities" : mirrorDefinition.entities,
            "transforms" : [transform],
            "instanceNames" : ["1"],
            notFoundErrorKey("entities") : ErrorStringEnum.MIRROR_SELECT_PARTS };
        opPattern(context, id, patternDefinition);

        if (getFeatureError(context, id).result != undefined)
        {
            reportFeatureError(context, id, mirrorDefinition.isFaceMirror ? ErrorStringEnum.MIRROR_FACE_FAILED : ErrorStringEnum.MIRROR_BODY_FAILED);
            return;
        }

        // Perform any booleans, if required
        if (!mirrorDefinition.isFaceMirror)
        {
            // We only include original body in the tools if the operation is UNION
            var additionalParmeters = (mirrorDefinition.operationType == NewBodyOperationType.ADD) ?
                { "seed" : mirrorDefinition.entities } : {};
            if (!processNewBodyIfNeeded(context, id, mergeMaps(mirrorDefinition, additionalParmeters)))
            {
                var errorId = id + "boolError";
                opPattern(context, errorId, patternDefinition);
                setBooleanErrorEntities(context, id, errorId);
            }
        }
    }, { isFaceMirror : false, operationType : NewBodyOperationType.NEW });

