FeatureScript 2522; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.


import(path : "onshape/std/attributes.fs", version : "2522.0");
import(path : "onshape/std/containers.fs", version : "2522.0");
import(path : "onshape/std/error.fs", version : "2522.0");
import(path : "onshape/std/feature.fs", version : "2522.0");
import(path : "onshape/std/string.fs", version : "2522.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2522.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2522.0");

/**
 * Deactivate the sheet metal model of selected parts.
 * Continued modeling on deactivated sheet metal parts will not be represented in the flat pattern or the table.
 */
annotation { "Feature Type Name" : "Finish sheet metal model" }
export const sheetMetalEnd = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Sheet metal part", "MaxNumberOfPicks" : 1,
                     "Filter" : EntityType.BODY && ActiveSheetMetal.YES && ModifiableEntityOnly.YES }
        definition.sheetMetalParts is Query;
    }
    {
        checkNotInFeaturePattern(context, definition.sheetMetalParts, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        verifyNonemptyQuery(context, definition, "sheetMetalParts", ErrorStringEnum.SHEET_METAL_SELECT_PART);

        if (!areEntitiesFromSingleActiveSheetMetalModel(context, definition.sheetMetalParts))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_SINGLE_MODEL_NEEDED, ["sheetMetalParts"]);
        }

        var smModels = qEntityFilter(qUnion(getSMDefinitionEntities(context, definition.sheetMetalParts)), EntityType.BODY);

        // highlight all SM parts affected by this operation when the feature is selected
        const modelId = getActiveSheetMetalId(context, smModels);
        if (!(modelId is undefined)) {
            const smBodies = qAttributeQuery(asSMAttribute({
            "objectType" : SMObjectType.MODEL, "attributeId" : modelId}));
            const parts = getSMCorrespondingInPart(context, smBodies, EntityType.BODY);
            setHighlightedEntities(context, {"entities": parts});
        }

        markSMModelsInactive(context, id, [smModels], ["sheetMetalParts"]);
        updateSheetMetalGeometry(context, id, {"entities" : smModels});
        if (!featureHasNonTrivialStatus(context, id))
        {
            reportFeatureInfo(context, id, ErrorStringEnum.SHEET_METAL_END_DONE);
        }
    }, {});

