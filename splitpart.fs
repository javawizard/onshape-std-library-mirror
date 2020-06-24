FeatureScript 1311; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "1311.0");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "1311.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "1311.0");
import(path : "onshape/std/boolean.fs", version : "1311.0");
import(path : "onshape/std/containers.fs", version : "1311.0");
import(path : "onshape/std/evaluate.fs", version : "1311.0");
import(path : "onshape/std/feature.fs", version : "1311.0");
import(path : "onshape/std/math.fs", version : "1311.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "1311.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "1311.0");
import(path : "onshape/std/sketch.fs", version : "1311.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1311.0");
import(path : "onshape/std/tool.fs", version : "1311.0");
import(path : "onshape/std/vector.fs", version : "1311.0");

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
annotation { "Feature Type Name" : "Split", "Filter Selector" : "allparts",
             "Editing Logic Function" : "splitEditLogic" }
export const splitPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Split type", "UIHint" : UIHint.HORIZONTAL_ENUM }
        definition.splitType is SplitType;

        if (definition.splitType == SplitType.PART)
        {
            annotation { "Name" : "Parts or surfaces to split", "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && ModifiableEntityOnly.YES }
            definition.targets is Query;

            annotation { "Name" : "Entity to split with",
                        "Filter" : (EntityType.BODY && BodyType.SHEET) || (GeometryType.PLANE && ConstructionObject.YES) || EntityType.FACE,
                        "MaxNumberOfPicks" : 1 }
            definition.tool is Query;

            annotation { "Name" : "Keep tools" }
            definition.keepTools is boolean;

            annotation { "Name" : "Trim to face boundaries" }
            definition.useTrimmed is boolean;
        }
        else
        {
            annotation { "Name" : "Faces to split", "Filter" : (EntityType.FACE && SketchObject.NO && ConstructionObject.NO && ModifiableEntityOnly.YES) }
            definition.faceTargets is Query;

            annotation { "Name" : "Entities to split with",
                        "Filter" : (EntityType.EDGE && SketchObject.YES && ModifiableEntityOnly.YES && ConstructionObject.NO) ||
                            (EntityType.BODY && BodyType.SHEET && ModifiableEntityOnly.NO) ||
                            EntityType.FACE || (GeometryType.PLANE && ConstructionObject.YES) }
            definition.faceTools is Query;

            annotation {"Name" : "Keep tool surfaces", "Default" : true}
            definition.keepToolSurfaces is boolean;
        }
    }
    {
        performSplit(context, id, definition);
    }, { keepTools : false, splitType : SplitType.PART, useTrimmed : false, keepToolSurfaces : true});

function performSplit(context is Context, id is Id, definition is map)
{
    if (definition.splitType == SplitType.PART)
    {
        if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V747_SPLIT_ALLOW_FACES))
        {
            definition.tool = qOwnerBody(definition.tool);
        }
        else
        {
            const sizeF = size(evaluateQuery(context, qEntityFilter(definition.tool, EntityType.FACE)));
            if (sizeF == 1)
            {
                if (definition.keepTools == false)
                    reportFeatureInfo(context, id, ErrorStringEnum.SPLIT_KEEP_TOOLS_WITH_FACE);
            }
            else
            {
                const allFaces = evaluateQuery(context, qOwnedByBody(definition.tool, EntityType.FACE));
                if (size(allFaces) > 1 && definition.useTrimmed) //multi-face surface body as tool
                    reportFeatureInfo(context, id, ErrorStringEnum.SPLIT_TRIM_WITH_SINGLE_FACE);
            }
        }
        opSplitPart(context, id, definition);
    }
    else
    {
        const edgeTools = qConstructionFilter(qEntityFilter(definition.faceTools, EntityType.EDGE),
            ConstructionObject.NO);
        const bodyTools = qConstructionFilter(qEntityFilter(definition.faceTools, EntityType.BODY),
            ConstructionObject.NO);
        const planeTools = qConstructionFilter(qEntityFilter(definition.faceTools, EntityType.FACE),
            ConstructionObject.YES);
        const faceTools = qConstructionFilter(qEntityFilter(definition.faceTools, EntityType.FACE),
            ConstructionObject.NO);
        var splitFaceDefinition = {
            "faceTargets" : definition.faceTargets,
            "edgeTools" : edgeTools,
            "bodyTools" : bodyTools,
            "planeTools" : planeTools,
            "faceTools" : faceTools,
            "keepToolSurfaces" : definition.keepToolSurfaces
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

/**
 * @internal
 * Edit logic to set keepTools to true when a face is selected
 */
export function splitEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    const sizeF = size(evaluateQuery(context, qEntityFilter(definition.tool, EntityType.FACE)));
    if (sizeF == 1 && !specifiedParameters.keepTools)
    {
        definition.keepTools = true;
    }
    return definition;
}

