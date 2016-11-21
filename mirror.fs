FeatureScript 455; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "455.0");
export import(path : "onshape/std/tool.fs", version : "455.0");
export import(path : "onshape/std/patternUtils.fs", version : "455.0");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "455.0");
import(path : "onshape/std/booleanHeuristics.fs", version : "455.0");
import(path : "onshape/std/containers.fs", version : "455.0");
import(path : "onshape/std/evaluate.fs", version : "455.0");
import(path : "onshape/std/feature.fs", version : "455.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "455.0");
import(path : "onshape/std/transform.fs", version : "455.0");


/**
 * Feature creating a single copy of some features, bodies, or faces, mirrored
 * about a given entity. Internally, performs an `applyPattern`, which in turn
 * performs an [opPattern] or, for a feature mirror, calls the feature
 * function.
 */
annotation { "Feature Type Name" : "Mirror",
             "Filter Selector" : "allparts",
             "Editing Logic Function" : "mirrorEditLogic" }
export const mirror = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Mirror type" }
        definition.patternType is MirrorType;

        if (definition.patternType == MirrorType.PART)
        {
            booleanStepTypePredicate(definition);

            annotation { "Name" : "Entities to mirror", "Filter" : EntityType.BODY }
            definition.entities is Query;
        }
        else if (definition.patternType == MirrorType.FACE)
        {
            annotation { "Name" : "Faces to mirror", "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
            definition.faces is Query;
        }
        else if (definition.patternType == MirrorType.FEATURE)
        {
            annotation { "Name" : "Features to mirror" }
            definition.instanceFunction is FeatureList;
        }

        annotation { "Name" : "Mirror plane", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1 }
        definition.mirrorPlane is Query;

        if (definition.patternType == MirrorType.PART)
        {
            booleanStepScopePredicate(definition);
        }
    }
    {
       if (definition.patternType == MirrorType.FACE)
            definition.entities = definition.faces;

        checkInput(context, id, definition, true);

        if (definition.patternType == MirrorType.FEATURE)
            definition.instanceFunction = valuesSortedById(context, definition.instanceFunction);

        var remainingTransform;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V325_FEATURE_MIRROR))
            remainingTransform = getRemainderPatternTransform(context, {"references" : qUnion([definition.entities])});
        else
            remainingTransform = getRemainderPatternTransform(context, {"references" : qUnion([definition.entities, definition.mirrorPlane])});

        definition.mirrorPlane = qGeometry(definition.mirrorPlane, GeometryType.PLANE);
        var planeResult = try(evPlane(context, { "face" : definition.mirrorPlane}));
        if (planeResult != undefined && isFeaturePattern(definition.patternType))
            planeResult = inverse(remainingTransform) * planeResult; // we don't want to transform the mirror plane

        if (planeResult == undefined)
            throw regenError(ErrorStringEnum.MIRROR_NO_PLANE, ["mirrorPlane"]);

        const transform = mirrorAcross(planeResult);

        definition.transforms = [transform];
        definition.instanceNames = ["1"];
        definition[notFoundErrorKey("entities")] = ErrorStringEnum.MIRROR_SELECT_PARTS;
        // We only include original body in the tools if the operation is UNION
        if (definition.patternType == MirrorType.PART && definition.operationType == NewBodyOperationType.ADD)
            definition.seed = definition.entities;
        applyPattern(context, id, definition, remainingTransform);
    }, { patternType : MirrorType.PART, operationType : NewBodyOperationType.NEW });

 /**
 * implements heuristics for mirror feature
 */
export function mirrorEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    return booleanStepEditLogic(context, id, oldDefinition, definition,
                                specifiedParameters, hiddenBodies, mirror);
}

