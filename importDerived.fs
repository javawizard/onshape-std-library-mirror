FeatureScript 2737; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2737.0");
export import(path : "onshape/std/tool.fs", version : "2737.0");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "2737.0");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "2737.0");
import(path : "onshape/std/booleanHeuristics.fs", version : "2737.0");
import(path : "onshape/std/containers.fs", version : "2737.0");
import(path : "onshape/std/coordSystem.fs", version : "2737.0");
import(path : "onshape/std/defaultFeatures.fs", version : "2737.0");
import(path : "onshape/std/derive.fs", version : "2737.0");
import(path : "onshape/std/evaluate.fs", version : "2737.0");
import(path : "onshape/std/feature.fs", version : "2737.0");
import(path : "onshape/std/instantiator.fs", version : "2737.0");
import(path : "onshape/std/tool.fs", version : "2737.0");
import(path : "onshape/std/transform.fs", version : "2737.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2737.0");
import(path : "onshape/std/valueBounds.fs", version : "2737.0");

/**
 * Enum controlling the placement of derived entities in the target part studio.
 *
 */
export enum DerivedPlacementType
{
    annotation { "Name" : "Base origin" }
    AT_ORIGIN,
    annotation { "Name" : "Base mate connector" }
    AT_MATE_CONNECTOR
}

const DERIVED_MATE_CONNECTOR_COUNT = {
    (unitless) : [-1, -1, 50] // default to -1 so that we initially don't have any manipulators selected
} as IntegerBoundSpec;
const MATE_CONNECTOR_MANIPULATOR = "mateConnectorManipulator";

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
 * Feature using [PartStudioData] for including parts in one
 * Part Studio that were designed in another.
 *
 * Location query determines where in the current Part Studio the selections
 * will be located. Bringing in multiple instances using multiple location
 * queries is allowed but not recommended.
 *
 * One can use the origin or a mate connector from the base Part Studio for
 * placement in the current Part Studio.
 */
annotation { "Feature Type Name" : "Derived",
             "Manipulator Change Function" : "onManipulatorChange" }
export const importDerived = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation {"Default" : true, "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.newUI is boolean;

        if (definition.newUI)
        {
            annotation { "Name" : "Part Studio",
                         "UIHint" : UIHint.SHOW_INLINE_CONFIG_INPUTS }
            definition.partStudio is PartStudioData;

            annotation { "Name" : "Preserve active sheet metal models"}
            definition.preserveActiveSheetMetal is boolean;

            annotation { "Name" : "Locations",
                         "Description" : "Select or create mate connectors in this Part Studio to position derived instances",
                         "Filter" : BodyType.MATE_CONNECTOR }
            definition.location is Query;

            annotation { "Name" : "Placement", "UIHint" : UIHint.SHOW_LABEL }
            definition.placement is DerivedPlacementType;

            if (definition.placement == DerivedPlacementType.AT_MATE_CONNECTOR )
            {
                annotation { "Name" : "Mate connector index", "UIHint" : UIHint.ALWAYS_HIDDEN}
                isInteger(definition.mateConnectorIndex, DERIVED_MATE_CONNECTOR_COUNT);

                annotation { "Name" : "Base mate connector id", "UIHint" : UIHint.ALWAYS_HIDDEN}
                isAnything(definition.mateConnectorId);

                annotation { "Name" : "Base mate connector feature index", "UIHint" : UIHint.ALWAYS_HIDDEN}
                isInteger(definition.mateConnectorIndexInFeature, DERIVED_MATE_CONNECTOR_COUNT);
            }

            annotation { "Default" : true, "Name" : "Include mate connectors" }
            definition.includeMateConnectors is boolean;
        }
        else
        {
            annotation { "Name" : "Parts to import", "UIHint" : UIHint.ALWAYS_HIDDEN }
            definition.parts is Query;
            annotation { "UIHint" : UIHint.ALWAYS_HIDDEN }
            definition.buildFunction is BuildFunction;
        }
    }
    {
        if (!definition.newUI)
        {
            return oldUIDerived(context, id, definition);
        }

        checkDerivedFromSameSource(context, id, definition);
        const remainderTransform = getRemainderPatternTransform(context, { "references" : definition.location });

        const userSelections = definition.partStudio.partQuery;
        if (definition.partStudio.buildFunction == undefined)
        {
            throw regenError(ErrorStringEnum.DERIVED_NO_PARTS, ["partStudio"]);
        }
        var otherContext = @convert(definition.partStudio.buildFunction(definition.partStudio.configuration), undefined);

        var selectedParts = userSelections;
        if (selectedParts == undefined || selectedParts.subqueries == [])
        {
            throw regenError(ErrorStringEnum.DERIVED_NO_PARTS, ["partStudio"]);
        }
        else if (isQueryEmpty(otherContext, qSubtraction(selectedParts, qDefaultBodies())))
        {
            throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["partStudio"]);
        }
        var partsToInstantiate = selectedParts;
        var havePartsToInstantiate = true;
        if (definition.preserveActiveSheetMetal)
        {
            rejectCompositesWithActiveSheetMetal(otherContext, selectedParts);

            // get mate connectors before separating sheet metal queries from others, otherwise we derive them twice:
            // once with an instantiate and once more with sheet metal derive.
            const getMateConnectorsBeforeSeparate = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2498_FIDELITY_REPORTING) &&
                                                    !isAtVersionOrLater(context, FeatureScriptVersionNumber.V2572_DERIVEDSM_BUGFIX);
            var mateConnectorsOfDerivedParts = qNothing();
            if (getMateConnectorsBeforeSeparate)
            {
                // Gets mate connector queries from derived parts and composites handling ownerless/implicit ones as well
                mateConnectorsOfDerivedParts = getRelevantBaseMateConnectors(context, otherContext, selectedParts).query;
                selectedParts = qUnion(selectedParts, mateConnectorsOfDerivedParts);
            }

            const queries = separateSheetMetalQueries(otherContext, selectedParts);
            var partsToDerive = queries.sheetMetalQueries;
            partsToInstantiate = queries.nonSheetMetalQueries;
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2572_DERIVEDSM_BUGFIX))
            {
                partsToInstantiate = qSubtraction(partsToInstantiate, qMateConnectorsOfParts(qUnion([partsToDerive, qFlattenedCompositeParts(partsToDerive)])));
            }

            // flat sketches will be added to and brought in with partsToDerive and will be shown on the flat
            // view, don't bring them with partsToInstantiate.
            const flatSketches = partsToInstantiate->qSketchFilter(SketchObject.YES)->qSheetMetalFlatFilter(SMFlatType.YES);
            partsToInstantiate = qSubtraction(partsToInstantiate, flatSketches);
            // if we only have default bodies in nonSheetMetal queries dont try to instantiate
            partsToInstantiate = qSubtraction(partsToInstantiate, qDefaultBodies());

            havePartsToInstantiate = !isQueryEmpty(otherContext, partsToInstantiate);
            partsToDerive = getRelevantSheetMetalParts(otherContext, partsToDerive);

            if (!isQueryEmpty(otherContext, partsToDerive))
            {
                if (size(evaluateQuery(otherContext, partsToDerive)) > size(evaluateQuery(otherContext, queries.sheetMetalQueries)))
                {
                    reportFeatureInfo(context, id, ErrorStringEnum.DERIVED_SM_AUTO_INSERT);
                }

                if (!getMateConnectorsBeforeSeparate)
                {
                    // Gets mate connector queries from derived parts and composites handling ownerless/implicit ones as well
                    mateConnectorsOfDerivedParts = getRelevantBaseMateConnectors(context, otherContext, partsToDerive).query;
                    definition.partStudio.partQuery = qUnion(partsToDerive, mateConnectorsOfDerivedParts);
                }
                else
                {
                    definition.partStudio.partQuery  = partsToDerive;
                }

                const locations = evaluateQuery(context, definition.location);
                if (size(locations) > 1)
                    reportFeatureError(context, id, ErrorStringEnum.DERIVED_NO_INSTANCING_SM);

                var transform = identityTransform();
                if (locations != [])
                {
                    const location =  evMateConnector(context, { "mateConnector" : locations[0] });
                    transform = toWorld(location);
                }

                var baseMateConnectorData = {};
                if (definition.placement == DerivedPlacementType.AT_MATE_CONNECTOR) //use a base mate connector
                {
                    if (definition.mateConnectorId != 0) // a manipulator has not yet been selected, we don't want to move the entities, just show manipulators
                    {
                        baseMateConnectorData = { "mateConnector" : mateConnectorsOfDerivedParts,
                                              "mateConnectorId" : definition.mateConnectorId as Id,
                                              "mateConnectorIndex" : definition.mateConnectorIndexInFeature};
                    }
                }

                deriveSheetMetal(context, otherContext, definition.partStudio, id, userSelections, transform, baseMateConnectorData );
                if (havePartsToInstantiate)
                {
                    otherContext = @convert(definition.partStudio.buildFunction(definition.partStudio.configuration), undefined);
                }
            }
        }
        if (havePartsToInstantiate)
        {
            // Gets mate connector queries from derived parts and composites handling ownerless/implicit ones as well
            const mateConnectorsOfDerivedParts = getRelevantBaseMateConnectors(context, otherContext, partsToInstantiate).query;
            definition.partStudio.partQuery = qUnion(partsToInstantiate, mateConnectorsOfDerivedParts) -> qSheetMetalFormFilter(SMFormType.NO);
            const locations = evaluateQuery(context, definition.location);
            const instantiator = newInstantiator(id, {
                "idToRecord" : id,
                "parameterNameToRecord" : "partStudio.partQuery",
                "parameterToRecord" : userSelections
                });

            var instanceDefinition = {"loadedContext" : otherContext}; // use provided otherContext
            if (definition.placement == DerivedPlacementType.AT_MATE_CONNECTOR) // use a base mate connector instead of base origin
            {
                if (definition.mateConnectorId != 0) // a manipulator has not yet been selected, we don't want to move the entities, just show manipulators
                {
                    instanceDefinition = { "mateConnector" : mateConnectorsOfDerivedParts,
                                           "mateConnectorId" : definition.mateConnectorId as Id,
                                           "mateConnectorIndex" : definition.mateConnectorIndexInFeature};
                }
            }

            if (locations == []) //position at current part studio origin
            {
                instanceDefinition.identity = qOrigin(EntityType.BODY); // used for external disambiguation
                addInstance(instantiator, definition.partStudio, instanceDefinition);
            }
            else
            {
                if (size(locations) > 1)
                    reportFeatureInfo(context, id, ErrorStringEnum.DERIVED_NO_INSTANCING);

               for (var mateConnector in locations)
                {
                    const location = evMateConnector(context, { "mateConnector" : mateConnector });
                    instanceDefinition.transform = toWorld(location);
                    instanceDefinition.identity = mateConnector;
                    addInstance(instantiator, definition.partStudio, instanceDefinition);
                }
            }

            instantiate(context, instantiator);

            if (instantiator[].status == ErrorStringEnum.DERIVED_MATE_CONNECTOR_RESET)
                reportFeatureWarning(context, id, ErrorStringEnum.DERIVED_MATE_CONNECTOR_RESET);
        }

        transformResultIfNecessary(context, id, remainderTransform);

        const points = processMateConnectors(context, id, definition.includeMateConnectors);
        if (definition.placement == DerivedPlacementType.AT_MATE_CONNECTOR)
        {
            const pointsManipulator = pointsManipulator({ "points" : points, "index" : definition.mateConnectorIndex, "primaryParameterId" : "mateConnectorIndex" });
            addManipulators(context, id, {(MATE_CONNECTOR_MANIPULATOR) : pointsManipulator });
        }

    }, { location : qNothing(), placement : DerivedPlacementType.AT_ORIGIN, mateConnectorIndex : -1 , includeMateConnectors : true, newUI : true, mateConnectorId : 0, mateConnectorIndexInFeature : -1, preserveActiveSheetMetal : false});

function rejectCompositesWithActiveSheetMetal(context is Context, selectedParts is Query)
{
    const flattenedComposites = qContainedInCompositeParts(selectedParts);
    if (!isQueryEmpty(context, qActiveSheetMetalFilter(flattenedComposites, ActiveSheetMetal.YES)))
    {
        throw regenError(ErrorStringEnum.DERIVED_NO_ACTIVE_SM_COMPOSITE, ["partStudio"]);
    }
}

function deriveSheetMetal(context is Context, otherContext is Context, partStudio is PartStudioData, idToRecord is Id, userSelections is Query, transform is Transform, baseMateConnectorData is map)
{

    var mergedParts = {};
    mergedParts[partStudio.partQuery] = true;
    const derivedResult = derive(context, idToRecord + "derive", partStudio.buildFunction, {
                "parts" : partStudio.partQuery,
                "mateConnectors" : baseMateConnectorData != {} ? [baseMateConnectorData.mateConnector] : undefined,
                "mateConnectorIds" : baseMateConnectorData != {} ? [baseMateConnectorData.mateConnectorId] : undefined,
                "mateConnectorIndices" : baseMateConnectorData != {} ? [baseMateConnectorData.mateConnectorIndex] : undefined,
                "queriesToTrack" : mergedParts,
                "idToRecord" : idToRecord,
                "parameterNameToRecord" : "partStudio.partQuery",
                "parameterToRecord" : userSelections,
                "clearSMDataFromAll" : false,
                "loadedContext" : otherContext
            });

    if (derivedResult.msg == ErrorStringEnum.DERIVED_MATE_CONNECTOR_RESET)
        reportFeatureWarning(context, idToRecord, ErrorStringEnum.DERIVED_MATE_CONNECTOR_RESET);

    const mateConnectorQueries = derivedResult.mateConnectors;
    mergedParts = derivedResult.trackingResults;

    var resultTransform = transform;
    if (baseMateConnectorData != {})
    {
        var mateConnectorTransform = derivedResult.mateConnectors[baseMateConnectorData.mateConnector];
        if (mateConnectorTransform != undefined)
            resultTransform *= mateConnectorTransform;
    }

    var derivedParts = mergedParts[partStudio.partQuery];
    if (derivedParts == undefined)
        throw regenError("Error tracking parts in sheet metal derive");

    if (resultTransform != identityTransform())
    {
      sheetMetalTransform(context, idToRecord, {"derivedParts" : derivedParts, "transform" : resultTransform});
    }
}

const sheetMetalTransform = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
{
    const smModelsQ = getSheetMetalModelForPart(context, qUnion(definition.derivedParts));
    const smModels = evaluateQuery(context, smModelsQ);

    const nonSMParts = qUnion(definition.derivedParts)->qActiveSheetMetalFilter(ActiveSheetMetal.NO);
    const flatSketches = qUnion(definition.derivedParts)->qSketchFilter(SketchObject.YES)->qSheetMetalFlatFilter(SMFlatType.YES);
    opTransform(context, id + "transform",
        { "bodies" : qSubtraction(qUnion(smModelsQ, nonSMParts), flatSketches),
          "transform" : definition.transform
        });

    callSubfeatureAndProcessStatus(id, updateSheetMetalGeometry, context, id + "smUpdate", {
                      "entities" : smModelsQ->qOwnedByBody( EntityType.FACE)});
}, {});

// Get relevant mate connectors from base part studio. Include ownerless/implicit ones as well
// if the whole part studio is selected. Otherwise only return those owned by selected parts.
function getRelevantBaseMateConnectors(context is Context, otherContext is Context, partQuery is Query) returns map
{
    var allBaseMateConnectorsQ = partQuery->qEntityFilter(EntityType.BODY)->qBodyType(BodyType.MATE_CONNECTOR);
    var allBaseMateConnectors = evaluateQuery(otherContext, allBaseMateConnectorsQ);
    if (allBaseMateConnectors == [])  // there are no implicit or ownerless mate connectors
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2515_COMPOSITE_MC_DERIVED))
        {
            //make sure to bring mate connectors owned by derived composite parts, not just their constituents
            allBaseMateConnectorsQ = qMateConnectorsOfParts(qUnion([partQuery, qFlattenedCompositeParts(partQuery)]));
        }
        else
            allBaseMateConnectorsQ = qMateConnectorsOfParts(qFlattenedCompositeParts(partQuery));
        allBaseMateConnectors = evaluateQuery(otherContext, allBaseMateConnectorsQ);
    }

    return { "query" : allBaseMateConnectorsQ, "evaluated" : allBaseMateConnectors };
}

function getRelevantSheetMetalParts(context is Context, selectedParts is Query) returns Query
{
    //we want to filter if any flats were selected while preserveActiveSheetMetal was false
    var queryToReturn = qUnion([qSheetMetalFlatFilter(selectedParts, SMFlatType.NO), qSketchFilter(selectedParts, SketchObject.YES)]);
    const smDefQ = evaluateQuery(context, getSheetMetalModelForPart(context, queryToReturn));
    if (smDefQ != [])
    {
        var partsToAdd = [];
        for (var model in smDefQ)
        {
            partsToAdd = append(partsToAdd, collectAllSheetMetalModelParts(context, model));
        }
        queryToReturn = qUnion([queryToReturn, qUnion(partsToAdd)]);
    }
    return queryToReturn;
}

function collectAllSheetMetalModelParts(context is Context, model is Query) returns Query
{
    const smId = getActiveSheetMetalId(context, model);
    var collectedParts = [];
    //get all 3d parts associated with selected SMs
    const allActiveSMQuery = qEverything(EntityType.BODY)->qBodyType(BodyType.SOLID)->qActiveSheetMetalFilter(ActiveSheetMetal.YES);
    for (var aSM in evaluateQuery(context, allActiveSMQuery))
    {
        if (getActiveSheetMetalId(context, aSM) == smId)
        {
            collectedParts = append(collectedParts, aSM);
        }
    }

    //get all flat sketches and form centermark mate connectors associated with selected SMs
    const allFlatSketchesQ = qEverything(EntityType.BODY)->qSketchFilter(SketchObject.YES)->qSheetMetalFlatFilter(SMFlatType.YES);
    const allFormCenterMarkMCs = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2612_DERIVE_FORM_CENTERMARK) ?
            qEverything(EntityType.BODY)->qSheetMetalFormFilter(SMFormType.YES)->qBodyType(BodyType.MATE_CONNECTOR) : qNothing();
    const allFlatSketches = evaluateQuery(context, qUnion(allFlatSketchesQ, allFormCenterMarkMCs));
    for (var flatSk in allFlatSketches)
    {
        const allFlatBodies = evaluateQuery(context, qPartsAttachedTo(flatSk));
        for (var flat in allFlatBodies)
        {
            if (getActiveSheetMetalId(context, flat) == smId)
            {
                collectedParts = append(collectedParts, flatSk);
            }
        }
    }

    return qUnion(collectedParts);
}

/**
 * @internal
 * Manipulator change function for `importDerived`. Uses a points manipulator to select mate connectors from source part studio
 */
export function onManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    if (newManipulators[MATE_CONNECTOR_MANIPULATOR] is map)
    {
        definition.mateConnectorIndex = newManipulators[MATE_CONNECTOR_MANIPULATOR].index;

        const otherContext = @convert(definition.partStudio.buildFunction(definition.partStudio.configuration), undefined);

        var selectedParts = definition.partStudio.partQuery;
        if (definition.preserveActiveSheetMetal)
        {
            selectedParts = getRelevantSheetMetalParts(otherContext, selectedParts);
        }
        const result = getRelevantBaseMateConnectors(context, otherContext, selectedParts);
        const allBaseMateConnectorsQ = result.query;
        const allBaseMateConnectors = result.evaluated;
        const instances = size(evaluateQuery(context, definition.location));
        var index = 0;

        if ( instances > 1 )
        {
            // when we have multiple instances, the manipulator clicked may have index larger than the number
            // of mate connectors
            const sizeMC = size(allBaseMateConnectors);
            index = definition.mateConnectorIndex % sizeMC;
        }
        else
        {
            index = definition.mateConnectorIndex;
        }

        const entitySelected = allBaseMateConnectors[index];
        definition.mateConnectorId = lastModifyingOperationId(otherContext, qNthElement(allBaseMateConnectorsQ, index));
        const allCreated = evaluateQuery(otherContext, qCreatedBy(definition.mateConnectorId, EntityType.BODY));
        definition.mateConnectorIndexInFeature = @indexOf(allCreated, entitySelected);
    }
    return definition;
}

enum ReferenceType
{
    UNKNOWN,
    RELEASE,
    VERSION,
    WORKSPACE
}

const DERIVED_TRACKING_MAP_VARIABLE_NAME = "-derivedTrackingMap"; // Not a valid identifier, so it is not offered in autocomplete

// Store a context variable that is a map keyed by [buildFunction, configuration]. Throw if user
// attempts to insert a derived feature from same source
function checkDerivedFromSameSource(context is Context, id is Id, definition is map)
{
    const refType = (definition.partStudio.referenceType != undefined) ? definition.partStudio.referenceType as ReferenceType : ReferenceType.UNKNOWN;
    if (refType == ReferenceType.RELEASE)
    {
        return;
    }
    var trackingMap = getVariable(context, DERIVED_TRACKING_MAP_VARIABLE_NAME, {});
    if (definition.partStudio.buildFunction != undefined)
    {
        const refLetter = (refType == ReferenceType.VERSION) ? "v" : "w";
        const key = [refLetter, definition.partStudio.buildFunction, definition.partStudio.configuration];
        if (trackingMap[key] != undefined)
        {
            throw regenError(ErrorStringEnum.DERIVED_NO_SAME_SOURCE);
        }
        trackingMap[key] = id;
        setVariable(context, DERIVED_TRACKING_MAP_VARIABLE_NAME, trackingMap);
    }
}

function processMateConnectors(context is Context, id is Id, includeMateConnectors is boolean) returns array
{
    var mateConnectors = qCreatedBy(id, EntityType.BODY)->qBodyType(BodyType.MATE_CONNECTOR);
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2612_DERIVE_FORM_CENTERMARK))
    {
        mateConnectors = mateConnectors->qSheetMetalFormFilter(SMFormType.NO);
    }

    var manipulatorPoints = [];
    for (var mateConnector in evaluateQuery(context, mateConnectors))
    {
        const mc = evMateConnector(context, {"mateConnector" : mateConnector});
        manipulatorPoints = append(manipulatorPoints, mc.origin);
    }

    if (!includeMateConnectors)
    {
        if (!isQueryEmpty(context, mateConnectors))
        {
            opDeleteBodies(context, id + "deleteMateConnectors", {
                    "entities" : mateConnectors
                });
        }
    }

    return manipulatorPoints;
}

function oldUIDerived(context is Context, id is Id, definition is map)
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

        // Record the parts query in the old context -- the record will be merged into the new context
        recordParameters(otherContext, id, definition);

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1186_COMPOSITE_QUERY))
            definition.parts = qConsumed(definition.parts, Consumed.NO);

        verifyNonemptyQuery(otherContext, definition, "parts", ErrorStringEnum.IMPORT_DERIVED_NO_PARTS);

        const otherContextId is Id = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1018_DERIVED) ?
                                                    makeId(id[0] ~ "_inBase") : id;

        // remove sheet metal attributes and helper bodies
        var smPartsQ = clearSheetMetalData(otherContext, otherContextId + "sheetMetal", undefined, false);

        var flattenedParts = qUnion([definition.parts, qContainedInCompositeParts(definition.parts)]);
        var bodiesToKeep = qSubtraction(qUnion([flattenedParts, qMateConnectorsOfParts(flattenedParts)]), qDefaultBodies());
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
}

