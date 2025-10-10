FeatureScript 2780; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "2780.0");
import(path : "onshape/std/containers.fs", version : "2780.0");
import(path : "onshape/std/context.fs", version : "2780.0");
import(path : "onshape/std/defaultFeatures.fs", version : "2780.0");
import(path : "onshape/std/query.fs", version : "2780.0");
import(path : "onshape/std/feature.fs", version : "2780.0");
import(path : "onshape/std/formedUtils.fs", version : "2780.0");
import(path : "onshape/std/evaluate.fs", version : "2780.0");
import(path : "onshape/std/coordSystem.fs", version : "2780.0");
import(path : "onshape/std/geomOperations.fs", version : "2780.0");
import(path : "onshape/std/properties.fs", version : "2780.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2780.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2780.0");
import(path : "onshape/std/string.fs", version : "2780.0");
import(path : "onshape/std/transform.fs", version : "2780.0");

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
 *              @field mateConnectorIds {array} : @optional Array of creating feature ids for mate connectors.
 *              @field mateConnectorFeatureIndices {array} : @optional Array of indices into mate connectors created by feature.
 *              @field loadedContext {Context} :  @optional Preloaded context, if available, of the reference.
 *              @field clearCustomProperties {boolean} : @optional Default is `false`.
 *              @field nameSuffix {string} : @optional A suffix to append to names of parts coming from base part studio.
 * }}
 * @return {{
 *              @field mateConnectors {map} : Map from mate connector query to `Transform` to that mate connector
 *              @field trackingResults {map} : Map from `Query` keys of `queriesToTrack` to each query's value in the new context (given
 *                      as an array of transient queries)
 * }}
 * */
export function derive(context is Context, id is Id, buildFunction is function, options is map) returns map
{
    const otherContext = options.loadedContext != undefined ? options.loadedContext : @convert(buildFunction(options.configuration), undefined);

    if (otherContext != undefined &&
        isAtVersionOrLater(context, FeatureScriptVersionNumber.V993_CLAMP_BASE_CONTEXT_VERSION))
    {
        @clampContextVersion(context, {"loadedContext" : otherContext});
    }

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1186_COMPOSITE_QUERY))
        options.parts = qConsumed(options.parts, Consumed.NO);

    if (options.idToRecord != undefined && options.parameterNameToRecord != undefined && options.parameterToRecord != undefined)
        @recordQuery(otherContext, options.idToRecord, { (options.parameterNameToRecord) : options.parameterToRecord });

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
        for (var i, query in options.mateConnectors)
        {
            const mateConnectorArray = evaluateQuery(otherContext, query);
            if (size(mateConnectorArray) == 0)
            {
                throw regenError(ErrorStringEnum.DERIVED_NO_MATE_CONNECTORS);
            }
            if (options.mateConnectorIds[i] == undefined ) //maintain current behavior
            {
                out.mateConnectors[query] = fromWorld(evMateConnector(otherContext, { "mateConnector" : query }));
            }
            else
            {
                const baseMC = evaluateQuery(otherContext, qCreatedBy(options.mateConnectorIds[i] as Id, EntityType.BODY));
                if (baseMC == [] || options.mateConnectorIndices[i] >= size(baseMC))
                {
                    out.msg = ErrorStringEnum.DERIVED_MATE_CONNECTOR_RESET;
                    out.mateConnectors[query] = fromWorld(evMateConnector(otherContext, { "mateConnector" : query }));
                }
                else
                {
                    out.mateConnectors[query] = fromWorld(evMateConnector(otherContext, { "mateConnector" : baseMC[options.mateConnectorIndices[i]] }));
                }
            }
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
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2473_SM_DERIVED_ERROR_CASE))
        bodiesToKeep = qUnion([bodiesToKeep, getSheetMetalModelForPart(otherContext, bodiesToKeep)]);

    var toDelete = qSubtraction(qUnion([ALL_BODIES, smPartsQ]), bodiesToKeep);
    //adjust for form sketches and mate connectors attached to flats we will delete
    const canNotKeepQ = computeFormArtifactsToDelete(otherContext, bodiesToKeep, toDelete);
    bodiesToKeep = qSubtraction(bodiesToKeep, canNotKeepQ);
    toDelete = qUnion(toDelete, canNotKeepQ);

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2466_SM_DERIVED))
    {
        //need to do this to prevent `Expected all associated sheet metal topologies to be bodies` warnings
        removeAttributes(otherContext, {
                "entities" : toDelete,
                "attributePattern" : smAssociationAttributePattern
            });
    }

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

    var names = [];
    if (isAtVersionOrLater(otherContext, FeatureScriptVersionNumber.V2769_ASM_MIRROR_FS_VERSION) && !isUndefinedOrEmptyString(options.nameSuffix))
    {
        names = collectNamesFromQueries(otherContext, queriesToTrack);
    }

    const clearCustomProperties = options.clearCustomProperties ?? false; //we keep custom properties by default
    const trackingResults = opMergeContexts(context, id + "merge", { "contextFrom" : otherContext,
                                                                    "trackThroughMerge" : queriesToTrack,
                                                                    "clearCustomProperties" : clearCustomProperties });

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

        if (!isUndefinedOrEmptyString(options.nameSuffix) && names != [])
        {
            updateNamesWithSuffix(context, trackingResults, names, options.nameSuffix);
        }
    }
    return out;
}

function collectNamesFromQueries(context is Context, queriesToTrack is array)
{
    var names = [];
    for (var q in queriesToTrack)
    {
        var qNames = [];
        for (var entity in evaluateQuery(context, q))
        {
            var name = getProperty(context, { "entity" : entity, "propertyType" : PropertyType.NAME } );
            qNames = append(qNames, name);
        }
        names = append(names, qNames);
    }
    return names;
}

function updateNamesWithSuffix(context is Context, trackingResults is array, names is array, suffix is string)
{
    for (var i = 0; i < size(trackingResults); i += 1)
    {
        if (trackingResults[i] != [] && size(trackingResults[i]) != size(names[i]))
            throw regenError("Size mismatch while updating names");

        if (names[i] == [])
            continue;
        for (var j = 0; j < size(trackingResults[i]); j += 1)
        {
            if (names[i][j] == undefined)
                continue;
            setProperty(context, {
                    "entities" : trackingResults[i][j],
                    "propertyType" : PropertyType.NAME,
                    "value" : names[i][j] ~ suffix
            });
        }
    }
}

