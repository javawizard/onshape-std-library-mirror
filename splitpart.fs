FeatureScript 505; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "505.0");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "505.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "505.0");
import(path : "onshape/std/boolean.fs", version : "505.0");
import(path : "onshape/std/containers.fs", version : "505.0");
import(path : "onshape/std/evaluate.fs", version : "505.0");
import(path : "onshape/std/feature.fs", version : "505.0");
import(path : "onshape/std/math.fs", version : "505.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "505.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "505.0");
import(path : "onshape/std/sketch.fs", version : "505.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "505.0");
import(path : "onshape/std/tool.fs", version : "505.0");
import(path : "onshape/std/vector.fs", version : "505.0");

/**
 * Defines whether a `split` should split whole parts, or just faces.
 */

export enum SplitType
{
    annotation { "Name" : "Part" }
    PART,
    annotation { "Name" : "Face" }
    FACE
}

/**
 * Feature performing an [opSplitPart].
 */
annotation { "Feature Type Name" : "Split", "Filter Selector" : "allparts", "Editing Logic Function" : "splitPartEditLogic" }
export const splitPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Split type", "UIHint" : "HORIZONTAL_ENUM" }
        definition.splitType is SplitType;

        if (definition.splitType == SplitType.PART)
        {
            annotation { "Name" : "Parts or surfaces to split", "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && ModifiableEntityOnly.YES }
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
            annotation { "Name" : "Faces to split", "Filter" : (EntityType.FACE && SketchObject.NO && ConstructionObject.NO && ModifiableEntityOnly.YES) }
            definition.faceTargets is Query;

            annotation { "Name" : "Entities to split with",
                        "Filter" : (EntityType.EDGE && SketchObject.YES && ModifiableEntityOnly.YES && ConstructionObject.NO) ||
                            (EntityType.BODY && BodyType.SHEET) ||
                            (EntityType.FACE && GeometryType.PLANE && ConstructionObject.YES) }
            definition.faceTools is Query;
        }
    }
    {
        performSplit(context, id, definition);
    }, { keepTools : false, splitType : SplitType.PART });

/** @internal */
export function splitPartEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
                                 isCreating is boolean, specifiedParameters is map) returns map
{
    if (!definition.keepTools && !specifiedParameters.keepTools)
    {
        definition.keepTools = hasNewNonModifiableEntities(context, definition.tool);
    }
    return definition;
}

function performSplit(context is Context, id is Id, definition is map)
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
        const planeResult = try silent (evOwnerSketchPlane(context, { "entity" : edgeTools }));
        if (planeResult != undefined)
        {
            splitFaceDefinition.direction = planeResult.normal;
        }

        opSplitFace(context, id, splitFaceDefinition);
    }
}

