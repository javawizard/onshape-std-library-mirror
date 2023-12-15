FeatureScript 2221; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/containers.fs", version : "2221.0");
import(path : "onshape/std/context.fs", version : "2221.0");
import(path : "onshape/std/defaultFeatures.fs", version : "2221.0");
import(path : "onshape/std/query.fs", version : "2221.0");
import(path : "onshape/std/feature.fs", version : "2221.0");
import(path : "onshape/std/evaluate.fs", version : "2221.0");
import(path : "onshape/std/coordSystem.fs", version : "2221.0");
import(path : "onshape/std/geomOperations.fs", version : "2221.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2221.0");
import(path : "onshape/std/transform.fs", version : "2221.0");

const NEVER_KEEP = qDefaultBodies();
const ALL_BODIES = qEverything(EntityType.BODY);

/**
 *  Merges context returned by buildFunction(options.configuration) into context.
 *
 * @param id : @autocomplete `id + "derive1"`
 * @param options {{
 *              @field parts {Query} : Queries resolving to bodies in base context to be preserved.
 *              @field configuration {map} : The configuration of the part studio. @autocomplete `{}`
 *              @field clearSMDataFromAll {boolean} : @optional Default is `true`.
 *                     If set to `false`, for every part in options.parts belonging to an active sheet metal model all 3d parts and flats
 *                     of that sheet metal model survive and remain active.
 *              @field filterOutNonModifiable {boolean} : @optional Default is `true`.
 *              @field propagateMergeStatus {boolean} : @optional Default is `true`.
 *              @field noPartsError {ErrorStringEnum} : @optional Error to be reported if options.parts resolves to empty array.
 *                     If field is not specified ErrorStringEnum.IMPORT_DERIVED_NO_PARTS is used.
 *              @field noPartsErrorParams {array} : @optional
 *              @field queriesToTrack {map} : @optional Map whose keys are `Query`s which resolve in the original derived context (that is, the context
 *                     resulting from `buildFunction`). If set, the output field `trackingResults` will contain values which resolve to each query's equivalent
 *                     entities in the current `context`.
 *              @field mateConnectors {array} : @optional Array of queries for mate connectors, to evaluate in the new context.
 *                     If set, the output field `mateConnectors` will be a map from each query to its resulting transform.
 * }}
 * @return {{
 *              @field mateConnectors {map} : Map from mate connector query to `Transform` to that mate connector
 *              @field trackingResults {map} : Map from `Query` keys of `queriesToTrack` to each query's value in the new context (given
 *                      as an array of transient queries)
 * }}
 * */
export function derive(context is Context, id is Id, buildFunction is function, options is map) returns map
{
    const otherContext = @convert(buildFunction(options.configuration), undefined);

    if (otherContext != undefined &&
        isAtVersionOrLater(context, FeatureScriptVersionNumber.V993_CLAMP_BASE_CONTEXT_VERSION))
    {
        @clampContextVersion(context, {"loadedContext" : otherContext});
    }

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1186_COMPOSITE_QUERY))
        options.parts = qConsumed(options.parts, Consumed.NO);

    if (size(evaluateQuery(otherContext, options.parts)) == 0)
    {
        const noPartsError = (options.noPartsError != undefined) ? options.noPartsError : ErrorStringEnum.IMPORT_DERIVED_NO_PARTS;
        if (options.noPartsErrorParams != undefined)
            throw regenError(noPartsError, options.noPartsErrorParams);
        else
            throw regenError(noPartsError);
    }

    var out = {};
    // Fill in fromWorld transformations corresponding to mateConnectors
    if (options.mateConnectors != undefined)
    {
        out.mateConnectors = {};
        for (var query in options.mateConnectors)
        {
            out.mateConnectors[query] = fromWorld(evMateConnector(otherContext, { "mateConnector" : query }));
        }
    }
    const otherContextId is Id = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1018_DERIVED) ?
                                                    makeId(id[0] ~ "_inBase") : id;

    const activeSmHandling = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2195_ACTIVE_SM_IN_DERIVE);
    // remove sheet metal attributes and helper bodies
    const smPartsQ = clearSheetMetalData(otherContext, otherContextId + "sheetMetal", (options.clearSMDataFromAll == false) ? options.parts : undefined, false);
    const survivingSMParts is Query = ALL_BODIES -> qBodyType(BodyType.SOLID) -> qActiveSheetMetalFilter(ActiveSheetMetal.YES);
    const flatParts is Query = qCorrespondingInFlat(survivingSMParts);

    var bodiesToKeep = (activeSmHandling) ? qUnion([options.parts, survivingSMParts, flatParts]) : options.parts;
    bodiesToKeep = qSubtraction(bodiesToKeep , NEVER_KEEP) ;
    // don't want to merge default bodies or unmodifiable bodies
    if (options.filterOutNonModifiable != false)
        bodiesToKeep = qModifiableEntityFilter(bodiesToKeep);
    bodiesToKeep = qUnion([bodiesToKeep, qContainedInCompositeParts(bodiesToKeep)]);

    const toDelete = qSubtraction(qUnion([ALL_BODIES, smPartsQ]), bodiesToKeep);

    opDeleteBodies(otherContext, otherContextId + "delete", { "entities" : toDelete });
    var queriesToTrack;
    if (options.queriesToTrack != undefined)
    {
        queriesToTrack = [];
        for (var query in options.queriesToTrack)
            queriesToTrack = append(queriesToTrack, query.key);
        if (activeSmHandling)
            queriesToTrack = append(queriesToTrack, qContainedInCompositeParts(qUnion(queriesToTrack)));
    }
    // before V2195 this call was here. It would barf if queriesToTrack is undefined
    if (!activeSmHandling)
       queriesToTrack = append(queriesToTrack, qContainedInCompositeParts(qUnion(queriesToTrack)));

    const trackingResults = opMergeContexts(context, id + "merge", { "contextFrom" : otherContext, "trackThroughMerge" : queriesToTrack });
    if (options.propagateMergeStatus != false)
        processSubfeatureStatus(context, id, { "subfeatureId" : id + "merge" });
    if (queriesToTrack != undefined)
    {
        out.trackingResults = {};
        if (size(queriesToTrack) != size(trackingResults))
            throw regenError("Wrong output from opMergeContexts");

        for (var i = 0; i < size(queriesToTrack); i += 1)
        {
            out.trackingResults[queriesToTrack[i]] = trackingResults[i];
        }
    }
    return out;
}


