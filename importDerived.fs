FeatureScript 1135; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "1135.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "1135.0");
import(path : "onshape/std/feature.fs", version : "1135.0");
import(path : "onshape/std/tool.fs", version : "1135.0");
import(path : "onshape/std/transform.fs", version : "1135.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "1135.0");

/**
 * A special type for functions defined as the `build` function for a Part
 * Studio, which return a context containing parts.
 */
export type BuildFunction typecheck canBeBuildFunction;

/** Typecheck for [BuildFunction] */
export predicate canBeBuildFunction(value)
{
    value is function;
}

/**
 * Feature performing an [opMergeContexts], used for including parts in one
 * Part Studio that were designed in another.
 *
 * When a derived part from Part Studio 1 is created in a Part Studio 2, code
 * is generated in Part Studio 2 which imports Part Studio 1 into a namespace.
 * The `build` function from that namespace is passed into this feature,
 * where the `build` function is called and run in its entirety.
 *
 * If not all bodies from the derived Part Studio are included, the missing
 * bodies are deleted after building the Part Studio, but before merging in
 * its context.
 */
annotation { "Feature Type Name" : "Derived" }
export const importDerived = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Parts to import", "UIHint" : "ALWAYS_HIDDEN" }
        definition.parts is Query;
        annotation { "UIHint" : "ALWAYS_HIDDEN" }
        definition.buildFunction is BuildFunction;
    }
    {

        var remainingTransform = getRemainderPatternTransform(context, {"references" : qNothing()});
        if (remainingTransform != identityTransform())
        {
            opPattern(context, id,
                      { "entities" : qCreatedBy(id, EntityType.BODY),
                        "transforms" : [remainingTransform],
                        "instanceNames" : ["1"] });
            return;
        }

        const otherContext = @convert(definition.buildFunction(), undefined);
        if (otherContext != undefined)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V993_CLAMP_BASE_CONTEXT_VERSION))
            {
                @clampContextVersion(context, {"loadedContext" : otherContext});
            }

            if (size(evaluateQuery(otherContext, definition.parts)) == 0)
                throw regenError(ErrorStringEnum.IMPORT_DERIVED_NO_PARTS, ["parts"]);

            // Record the parts query in the old context -- the record will be merged into the new context
            recordParameters(otherContext, id, definition);

            const otherContextId is Id = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1018_DERIVED) ?
                                                    makeId(id[0] ~ "_inBase") : id;

            // remove sheet metal attributes and helper bodies
            var smPartsQ = clearSheetMetalData(otherContext, otherContextId + "sheetMetal", undefined);

            //don't want to merge default bodies
            const defaultBodies = qUnion([qCreatedBy(makeId("Origin"), EntityType.BODY),
                                          qCreatedBy(makeId("Front"), EntityType.BODY),
                                          qCreatedBy(makeId("Top"), EntityType.BODY),
                                          qCreatedBy(makeId("Right"), EntityType.BODY)]);

            var bodiesToKeep = qSubtraction(qUnion([definition.parts, qMateConnectorsOfParts(definition.parts), qContainedInCompositeParts(definition.parts)]), defaultBodies);
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V566_MODIFIABLE_ONLY_IN_DERIVED))
            {
                bodiesToKeep = qModifiableEntityFilter(bodiesToKeep);
            }

            const allBodies = qEverything(EntityType.BODY);

            const deleteDefinition = {
                "entities" : qSubtraction(qUnion([allBodies, smPartsQ]) , bodiesToKeep)
            };
            opDeleteBodies(otherContext, otherContextId + "delete", deleteDefinition);

            var mergeDefinition = definition; // to pass such general parameters as asVersion
            mergeDefinition.contextFrom = otherContext;
            opMergeContexts(context, id + "merge", mergeDefinition);
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V468_PROPAGATE_MERGE_ERROR))
            {
                processSubfeatureStatus(context, id, {"subfeatureId" : id + "merge"});
            }
        }
    });

