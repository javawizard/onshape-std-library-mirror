FeatureScript 316; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/tool.fs", version : "");
export import(path : "onshape/std/patternUtils.fs", version : "");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "");
import(path : "onshape/std/booleanHeuristics.fs", version : "");
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
annotation { "Feature Type Name" : "Mirror",
             "Filter Selector" : "allparts",
             "Editing Logic Function" : "mirrorEditLogic" }
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

        var remainingTransform = getRemainderPatternTransform(context,
                {"references" : qUnion([definition.entities, definition.mirrorPlane])});

        definition.mirrorPlane = qGeometry(definition.mirrorPlane, GeometryType.PLANE);
        const planeResult = try(evPlane(context, { "face" : definition.mirrorPlane }));
        if (planeResult == undefined)
            throw regenError(ErrorStringEnum.MIRROR_NO_PLANE, ["mirrorPlane"]);

        const transform = mirrorAcross(planeResult);

        definition.transforms = [transform];
        definition.instanceNames = ["1"];
        definition[notFoundErrorKey("entities")] = ErrorStringEnum.MIRROR_SELECT_PARTS;
        // We only include original body in the tools if the operation is UNION
        if (!definition.isFaceMirror && definition.operationType == NewBodyOperationType.ADD)
            definition.seed = definition.entities;
        applyPattern(context, id, definition, remainingTransform);
    }, { isFaceMirror : false, operationType : NewBodyOperationType.NEW });

 /**
 * implements heuristics for mirror feature
 */
export function mirrorEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    return booleanStepEditLogic(context, id, oldDefinition, definition,
                                specifiedParameters, hiddenBodies, mirror);
}

