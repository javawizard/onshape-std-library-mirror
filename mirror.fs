FeatureScript ✨; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/tool.fs", version : "");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "");
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/evaluate.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/transform.fs", version : "");

/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
annotation { "Feature Type Name" : "Mirror", "Filter Selector" : "allparts" }
export const mirror = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Face mirror", "Default" : false }
        definition.isFaceMirror is boolean;

        if (!definition.isFaceMirror)
        {
            booleanStepTypePredicate(definition);

            annotation { "Name" : "Entities to mirror", "Filter" : EntityType.BODY }
            definition.entities is Query;
        }
        else
        {
            annotation { "Name" : "Faces to mirror", "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
            definition.faces is Query;
        }

        annotation { "Name" : "Mirror plane", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1 }
        definition.mirrorPlane is Query;

        if (!definition.isFaceMirror)
        {
            booleanStepScopePredicate(definition);
        }
    }
    {
        const isFaceMirror = definition.isFaceMirror;

        if (isFaceMirror)
            definition.entities = definition.faces;

        if (size(evaluateQuery(context, definition.entities)) == 0)
        {
            if (isFaceMirror)
                throw regenError(ErrorStringEnum.MIRROR_SELECT_FACES, ["faces"]);
            else
                throw regenError(ErrorStringEnum.MIRROR_SELECT_PARTS, ["entities"]);
            return;
        }

        definition.mirrorPlane = qGeometry(definition.mirrorPlane, GeometryType.PLANE);
        const planeResult = try(evPlane(context, { "face" : definition.mirrorPlane }));
        if (planeResult == undefined)
            throw regenError(ErrorStringEnum.MIRROR_NO_PLANE, ["mirrorPlane"]);

        const transform = mirrorAcross(planeResult);
        const patternDefinition = {
            "entities" : definition.entities,
            "transforms" : [transform],
            "instanceNames" : ["1"],
            notFoundErrorKey("entities") : ErrorStringEnum.MIRROR_SELECT_PARTS };

        try
        {
            opPattern(context, id, patternDefinition);
        }
        catch
        {
            throw regenError(definition.isFaceMirror ? ErrorStringEnum.MIRROR_FACE_FAILED : ErrorStringEnum.MIRROR_BODY_FAILED);
        }

        // Perform any booleans, if required
        if (!definition.isFaceMirror)
        {
            // We only include original body in the tools if the operation is UNION
            const additionalParmeters = (definition.operationType == NewBodyOperationType.ADD) ?
                { "seed" : definition.entities } : {};
            const reconstructOp = function(id) { opPattern(context, id, patternDefinition); };
            processNewBodyIfNeeded(context, id, mergeMaps(definition, additionalParmeters), reconstructOp);
        }
    }, { isFaceMirror : false, operationType : NewBodyOperationType.NEW });

