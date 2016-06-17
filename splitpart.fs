FeatureScript 369; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "369.0");

// Imports used internally
import(path : "onshape/std/feature.fs", version : "369.0");
import(path : "onshape/std/tool.fs", version : "369.0");
import(path : "onshape/std/evaluate.fs", version : "369.0");

/**
 * Defines whether a `spit` should split whole parts, or just faces.
 */
export enum SplitType
{
    annotation { "Name" : "Part" }
    PART,
    annotation { "Name" : "Face" }
    FACE
}

/**
 * Feature performing an `opSplit`.
 */
annotation { "Feature Type Name" : "Split", "Filter Selector" : "allparts" }
export const splitPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Split type" }
        definition.splitType is SplitType;

        if (definition.splitType == SplitType.PART)
        {
            annotation { "Name" : "Parts or surfaces to split", "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) }
            definition.targets is Query;

            annotation { "Name" : "Entity to split with",
                "Filter" : (EntityType.BODY && BodyType.SHEET) || (GeometryType.PLANE && ConstructionObject.YES),
                "MaxNumberOfPicks" : 1 }
            definition.tool is Query;

            annotation { "Name" : "Keep tools" }
            definition.keepTools is boolean;
        }
        else
        {
            annotation { "Name" : "Faces to split", "Filter" : (EntityType.FACE && SketchObject.NO && ConstructionObject.NO) }
            definition.faceTargets is Query;

            annotation { "Name" : "Entities to split with",
                "Filter" : (EntityType.EDGE && SketchObject.YES && ConstructionObject.NO) ||
                           (EntityType.BODY && BodyType.SHEET) ||
                           (EntityType.FACE && GeometryType.PLANE && ConstructionObject.YES) }
            definition.faceTools is Query;
        }
    }
    {
        if (definition.splitType == SplitType.PART)
        {
            definition.tool = qOwnerBody(definition.tool);
            opSplitPart(context, id, definition);
        }
        else
        {
            var edgeTools = qConstructionFilter(qEntityFilter(definition.faceTools, EntityType.EDGE),
                                                ConstructionObject.NO);
            var bodyTools = qConstructionFilter(qEntityFilter(definition.faceTools, EntityType.BODY),
                                                ConstructionObject.NO);
            var planeTools = qEntityFilter(definition.faceTools, EntityType.FACE);

            var splitFaceDefinition = {
                "faceTargets" : definition.faceTargets,
                "edgeTools" : edgeTools,
                "bodyTools" : bodyTools,
                "planeTools" : planeTools
            };

            // edge tools could be empty
            const planeResult = try(evOwnerSketchPlane(context, { "entity" : edgeTools }));
            if (planeResult != undefined)
            {
                splitFaceDefinition.direction = planeResult.normal;
            }

            opSplitFace(context, id, splitFaceDefinition);
        }
    }, { keepTools : false, splitType : SplitType.PART });


