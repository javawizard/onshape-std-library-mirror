FeatureScript 2878; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/containers.fs", version : "2878.0");
import(path : "onshape/std/derive.fs", version : "2878.0");
import(path : "onshape/std/feature.fs", version : "2878.0");
import(path : "onshape/std/importDerived.fs", version : "2878.0");
import(path : "onshape/std/instantiator.fs", version : "2878.0");
import(path : "onshape/std/tabReferences.fs", version : "2878.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2878.0");
import(path : "onshape/std/evaluate.fs", version : "2878.0");
import(path : "onshape/std/coordSystem.fs", version : "2878.0");
import(path : "onshape/std/query.fs", version : "2878.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2878.0");
import(path : "onshape/std/transform.fs", version : "2878.0");
import(path: "onshape/std/vector.fs", version : "2878.0");


/**
 * @internal
 * Internal feature for assembly mirror.
 *
 * @param id : @autocomplete `id + "derivedMirror"`
 */
annotation { "Feature Type Name" : "Derived mirror", "Feature Type Description" : "" }
export const derivedMirror = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Part Studio",
                     "UIHint" : [UIHint.READ_ONLY, UIHint.UNCONFIGURABLE] }
        definition.partStudio is PartStudioData;
    }
    {
        if (definition.partStudio.buildFunction == undefined)
        {
            throw regenError(ErrorStringEnum.IMPORT_DERIVED_NO_PARTS, ["partStudio"]);
        }

        const allBodies = qEverything(EntityType.BODY);
        var otherContext = @convert(definition.partStudio.buildFunction(definition.partStudio.configuration), undefined);
        var partsToInstantiate = definition.partStudio.partQuery;
        var sheetMetalPartQueries = qNothing();

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2848_ASM_MIRROR_SUPPORT_FLAT_PARTS))
        {
            if (isQueryEmpty(otherContext, definition.partStudio.partQuery))
            {
                throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["partStudio"]);
            }
            // If there are closed composite part queries present fall back onto old behavior and do not derive anything as sheet metal.
            const hasClosedCompositesWithActiveSM = !isQueryEmpty(otherContext, qContainedInCompositeParts(definition.partStudio.partQuery)->
                                                                        qCompositePartTypeFilter(CompositePartType.CLOSED)->
                                                                        qActiveSheetMetalFilter(ActiveSheetMetal.YES));
            if (!hasClosedCompositesWithActiveSM)
            {
                const queries = separateSheetMetalQueries(otherContext, definition.partStudio.partQuery);
                partsToInstantiate = queries.nonSheetMetalQueries;
                sheetMetalPartQueries = getRelevantSheetMetalParts(otherContext, queries.sheetMetalQueries);
                // Skip deriving sketches on flat for now. Form wires do come through
                const sketchesOnFlat = sheetMetalPartQueries->qSketchFilter(SketchObject.YES)->qSheetMetalFlatFilter(SMFlatType.YES);
                sheetMetalPartQueries = qSubtraction(sheetMetalPartQueries, sketchesOnFlat);
                if (!isQueryEmpty(otherContext, sheetMetalPartQueries->qCompositePartsContaining(CompositePartType.CLOSED)))
                {
                    // some relevant sheet metal parts happened to be in closed composites, revert to old behavior
                    sheetMetalPartQueries = qNothing();
                    partsToInstantiate = definition.partStudio.partQuery;
                    reportFeatureInfo(context, id, ErrorStringEnum.ASSEMBLY_MIRROR_NO_ACTIVE_SM_COMPOSITE);
                }
                else
                {
                    // we can handle open composites, just go through derive workflow instead of instantiate to avoid
                    // double derivation
                    const openCompositesWithSM = qCompositePartsContaining(sheetMetalPartQueries, CompositePartType.OPEN);
                    partsToInstantiate = qSubtraction(partsToInstantiate, qUnion([openCompositesWithSM,
                                        qIntersection([qFlattenedCompositeParts(openCompositesWithSM), partsToInstantiate])]));
                    sheetMetalPartQueries = qUnion(sheetMetalPartQueries, openCompositesWithSM);
                }
            }
            else
            {
                reportFeatureInfo(context, id, ErrorStringEnum.ASSEMBLY_MIRROR_NO_ACTIVE_SM_COMPOSITE);
            }
        }

        const transform = mirrorAcross(XY_PLANE);
        const haveSMParts = !isQueryEmpty(otherContext, sheetMetalPartQueries);
        if (!isQueryEmpty(otherContext, partsToInstantiate))
        {
            const instantiator = newInstantiator(id, { "clearCustomProperties" : true, "nameSuffix" : "-Mirrored" });
            definition.partStudio.partQuery = partsToInstantiate;
            addInstance(instantiator, definition.partStudio, {});
            try silent
            {
                instantiate(context, instantiator);
            }
            catch(error)
            {
                if (!haveSMParts)
                {
                    throw error;
                }
            }
            const allParts = qUnion(
                allBodies->qBodyType(BodyType.SOLID),
                allBodies->qBodyType(BodyType.SHEET),
                allBodies->qBodyType(BodyType.COMPOSITE)
            )->qConstructionFilter(ConstructionObject.NO);

            if (!isQueryEmpty(context, allParts))
            {
                opTransform(context, id + "mirrorTransform", {
                    "bodies" : allParts,
                    "transform" : transform
                });
            }
            if (haveSMParts)
            {
                otherContext = @convert(definition.partStudio.buildFunction(definition.partStudio.configuration), undefined);
            }
        }
        if (haveSMParts)
        {
            definition.partStudio.partQuery = sheetMetalPartQueries;
            deriveSheetMetalMirror(context, otherContext, definition, id + "deriveSM", transform);
        }

        const allMateConnectors = qBodyType(allBodies, BodyType.MATE_CONNECTOR);
        for (var index, mateConnectorQuery in evaluateQuery(context, allMateConnectors))
        {
            var mateConnector = evMateConnector(context, { "mateConnector" : mateConnectorQuery });
            var mirroredCS = coordSystem(vector(mateConnector.origin[0], mateConnector.origin[1], -1 * mateConnector.origin[2]),
                                         vector(mateConnector.xAxis[0], mateConnector.xAxis[1], -1 * mateConnector.xAxis[2]),
                                         vector(mateConnector.zAxis[0], mateConnector.zAxis[1], -1 * mateConnector.zAxis[2]));
            opTransform(context, id + index, {
                    "bodies" : mateConnectorQuery,
                    "transform" :
                    toWorld(mirroredCS) *
                    fromWorld(mateConnector)});
        }
    });

function deriveSheetMetalMirror(context is Context, otherContext is Context, definition is map, idToRecord is Id, transform is Transform)
{
    const partStudio = definition.partStudio;
    var mergedParts = {};
    mergedParts[partStudio.partQuery] = true;
    const derivedResult = derive(context, idToRecord + "derive", partStudio.buildFunction, {
                "parts" : partStudio.partQuery,
                "queriesToTrack" : mergedParts,
                "idToRecord" : idToRecord,
                "clearSMDataFromAll" : false,
                "loadedContext" : otherContext,
                "nameSuffix" : "-Mirrored"
            });

    mergedParts = derivedResult.trackingResults;
    var derivedParts = mergedParts[partStudio.partQuery];
    if (derivedParts == undefined)
    {
        throw regenError("Error tracking parts in sheet metal derive");
    }

    if (transform != identityTransform())
    {
        //necessary to flatten as we might have open composites here
        const partsToTransform = evaluateQuery(context, qUnion(derivedParts)->qFlattenedCompositeParts());
        sheetMetalTransform(context, idToRecord + "mirrorTransform", {"derivedParts" : partsToTransform, "transform" : transform});
    }
}

