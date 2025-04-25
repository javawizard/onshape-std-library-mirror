FeatureScript 2641; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.


import(path : "onshape/std/attributes.fs", version : "2641.0");
import(path : "onshape/std/containers.fs", version : "2641.0");
import(path : "onshape/std/debug.fs", version : "2641.0");
import(path : "onshape/std/evaluate.fs", version : "2641.0");
import(path : "onshape/std/error.fs", version : "2641.0");
import(path : "onshape/std/feature.fs", version : "2641.0");
import(path : "onshape/std/string.fs", version : "2641.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2641.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2641.0");

/**
 * Deactivate the sheet metal model of selected parts.
 * Continued modeling on deactivated sheet metal parts will not be represented in the flat pattern or the table.
 */
annotation { "Feature Type Name" : "Finish sheet metal model" }
export const sheetMetalEnd = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Sheet metal parts",
                     "Filter" : EntityType.BODY && ActiveSheetMetal.YES && ModifiableEntityOnly.YES }
        definition.sheetMetalParts is Query;
    }
    {
        checkNotInFeaturePattern(context, definition.sheetMetalParts, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2543_FINISH_SM_PARTS_BUGS))
        {
            verifyNonemptyQuery(context, definition, "sheetMetalParts", ErrorStringEnum.SHEET_METAL_SELECT_PARTS);
        }
        else
        {
            verifyNonemptyQuery(context, definition, "sheetMetalParts", ErrorStringEnum.SHEET_METAL_SELECT_PART);
        }

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2532_END_SM_UPDATE))
        {
            const result = anyPartIsActiveNotActive(context, definition.sheetMetalParts);
            if (!result.anyPartIsActive)
            {
                // Error if no parts are active sheet metal
                throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_MODEL_NEEDED, ["sheetMetalParts"]);
            }
            else if (result.anyPartIsNotActive)
            {
                // Warning if any part is not active sheet metal
                reportFeatureWarning(context, id, ErrorStringEnum.SHEET_METAL_INACTIVE_MODEL_SELECTED, ["sheetMetalParts"]);
            }
        }
        else
        {
            if (!areEntitiesFromSingleActiveSheetMetalModel(context, definition.sheetMetalParts))
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_SINGLE_MODEL_NEEDED, ["sheetMetalParts"]);
            }
        }

        var smModelsQ = qEntityFilter(qUnion(getSMDefinitionEntities(context, definition.sheetMetalParts)), EntityType.BODY);

        var smModels = evaluateQuery(context, smModelsQ);

        var allEdges = [];

        // highlight all SM parts affected by this operation when the feature is selected
        for (var smModel in smModels)
        {
            const modelId = getActiveSheetMetalId(context, smModel);
            if (!(modelId is undefined))
            {
                const smBodies = qAttributeQuery(asSMAttribute({
                                "objectType" : SMObjectType.MODEL, "attributeId" : modelId }));
                const parts = getSMCorrespondingInPart(context, smBodies, EntityType.BODY);
                setHighlightedEntities(context, { "entities" : parts });
                var query = qEntityFilter(qOwnedByBody(qSubtraction(parts, definition.sheetMetalParts)), EntityType.EDGE);
                allEdges = append(allEdges, query);
            }
        }

        // Compute tan edges to exclude from debug..
        var tanEdges is box = new box([]);

        forEachEntity(context, id + "operation", qUnion(allEdges), function(entity is Query, id is Id)
            {
                // perform operations with the entity
                const convex = evEdgeConvexity(context, {
                            "edge" : entity
                        });
                if (convex == EdgeConvexityType.SMOOTH)
                {
                    tanEdges[] = append(tanEdges[], entity);
                }
            });
        // Add subtract filter
        var otherpartsQ = qSubtraction(qUnion(allEdges), qUnion(tanEdges[]));
        if (!isQueryEmpty(context,otherpartsQ))
        {
            addDebugEntities(context, otherpartsQ, DebugColor.YELLOW);
        }

        markSMModelsInactive(context, id, smModels, ["sheetMetalParts"]);
        updateSheetMetalGeometry(context, id, {"entities" : smModelsQ});
        if (!featureHasNonTrivialStatus(context, id))
        {
            reportFeatureInfo(context, id, ErrorStringEnum.SHEET_METAL_END_DONE);
        }
    }, {});

function anyPartIsActiveNotActive(context is Context, bodies is Query) returns map
{
    var anyPartIsActive = false;
    var anyPartIsNotActive = false;
    var smDefinitionBodies = [];
    for (var body in evaluateQuery(context, bodies))
    {
        smDefinitionBodies = getSMDefinitionEntities(context, body);
        if (smDefinitionBodies == [])
        {
            anyPartIsNotActive = true;
        }
        else
        {
            for (var smDefinitionBody in smDefinitionBodies)
            {
                if (isSheetMetalModelActive(context, smDefinitionBody))
                {
                    anyPartIsActive = true;
                }
                else
                {
                    anyPartIsNotActive = true;
                }
            }
        }
        if (anyPartIsActive && anyPartIsNotActive)
        {
            break;
        }
    }
    return { "anyPartIsActive" : anyPartIsActive, "anyPartIsNotActive" : anyPartIsNotActive };
}


