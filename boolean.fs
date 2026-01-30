FeatureScript 2878; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/booleanoperationtype.gen.fs", version : "2878.0");
export import(path : "onshape/std/query.fs", version : "2878.0");
export import(path : "onshape/std/tool.fs", version : "2878.0");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "2878.0");
import(path : "onshape/std/box.fs", version : "2878.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "2878.0");
import(path : "onshape/std/clashtype.gen.fs", version : "2878.0");
import(path : "onshape/std/containers.fs", version : "2878.0");
import(path : "onshape/std/evaluate.fs", version : "2878.0");
import(path : "onshape/std/feature.fs", version : "2878.0");
import(path : "onshape/std/math.fs", version : "2878.0");
import(path : "onshape/std/patternCommon.fs", version : "2878.0");
import(path : "onshape/std/primitives.fs", version : "2878.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2878.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2878.0");
import(path : "onshape/std/string.fs", version : "2878.0");
import(path : "onshape/std/topologyUtils.fs", version : "2878.0");
import(path : "onshape/std/transform.fs", version : "2878.0");
import(path : "onshape/std/valueBounds.fs", version : "2878.0");
import(path : "onshape/std/vector.fs", version : "2878.0");

/**
 * The boolean feature.  Performs an [opBoolean] after a possible [opOffsetFace] if the operation is subtraction.
 */
annotation { "Feature Type Name" : "Boolean", "Filter Selector" : "allparts" }
export const booleanBodies = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Operation type", "UIHint" : UIHint.HORIZONTAL_ENUM }
        definition.operationType is BooleanOperationType;
        annotation { "Name" : "Tools", "Filter" : EntityType.BODY &&
                    (BodyType.SOLID || (BodyType.SHEET && ConstructionObject.NO && SketchObject.NO)) && AllowMeshGeometry.YES,
                    "UIHint" : UIHint.ALLOW_QUERY_ORDER }
        definition.tools is Query;

        if (definition.operationType == BooleanOperationType.SUBTRACTION)
        {
            annotation { "Name" : "Targets", "Filter" : EntityType.BODY && ModifiableEntityOnly.YES &&
                        (BodyType.SOLID || (BodyType.SHEET && ConstructionObject.NO && SketchObject.NO)) && AllowMeshGeometry.YES }
            definition.targets is Query;

            annotation { "Name" : "Offset" }
            definition.offset is boolean;

            if (definition.offset)
            {
                annotation { "Name" : "Offset all" }
                definition.offsetAll is boolean;

                if (!definition.offsetAll)
                {
                    annotation { "Name" : "Faces to offset",
                                "Filter" : (EntityType.FACE && BodyType.SOLID) }
                    definition.entitiesToOffset is Query;
                }

                annotation { "Name" : "Offset distance" }
                isLength(definition.offsetDistance, ZERO_INCLUSIVE_OFFSET_BOUNDS);

                annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                definition.oppositeDirection is boolean;

                annotation { "Name" : "Reapply fillet" }
                definition.reFillet is boolean;
            }
        }

        annotation { "Name" : "Keep tools", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        definition.keepTools is boolean;
    }
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1197_DETECT_SURFACE_JOIN_CPP))
        {
            const hasSheetsAsTools = !isQueryEmpty(context, qModifiableSurface(definition.tools));

            if (definition.operationType != BooleanOperationType.UNION)
            {
                if (hasSheetsAsTools)
                {
                    throw regenError(ErrorStringEnum.BOOLEAN_TOOL_INPUTS_NOT_SOLID, ["tools"]);
                }
            }
            else if (hasSheetsAsTools)
            {
                if (!isQueryEmpty(context, qBodyType(definition.tools, BodyType.SOLID)))
                {
                    throw regenError(ErrorStringEnum.BOOLEAN_CANNOT_MIX_SOLIDS_AND_SURFACES, ["tools"]);
                }
                try
                {
                    const noImpliedDetection =
                    !isAtVersionOrLater(context, FeatureScriptVersionNumber.V1417_IMPLIED_DETECT_ADJACENCY);
                    opBoolean(context, id, {
                                "operationType" : BooleanOperationType.UNION,
                                "makeSolid" : true,
                                "eraseImprintedEdges" : true,
                                "detectAdjacencyForSheets" : noImpliedDetection,
                                "recomputeMatches" : true,
                                "tools" : definition.tools,
                                "keepTools" : definition.keepTools
                            });
                }
                return;
            }
        }

        var doOffset = definition.offset && definition.operationType == BooleanOperationType.SUBTRACTION;
        if (doOffset && tolerantEquals(definition.offsetDistance, 0 * meter))
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1700_BOOLEAN_ZERO_OFFSET))
            {
                doOffset = false;
            }
            else
            {
                throw regenError(ErrorStringEnum.DIRECT_EDIT_NO_OFFSET, ["offsetDistance"]);
            }
        }

        if (doOffset)
        {
            if (definition.oppositeDirection)
            {
                definition.offsetDistance = -definition.offsetDistance;
            }

            var faceQuery;
            const useTrackingQuery = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2704_QLV_EXEC_FILTER);
            if (useTrackingQuery)
            {
                faceQuery = startTracking(context, definition.offsetAll ? qOwnedByBody(definition.tools, EntityType.FACE) : definition.entitiesToOffset);
            }
            const suffix = "offsetTempBody";
            const transformMatrix = identityTransform();
            opPattern(context, id + suffix,
                { "entities" : definition.tools,
                        "transforms" : [transformMatrix],
                        "instanceNames" : ["1"] });

            if (!useTrackingQuery)
            {
                if (definition.offsetAll)
                {
                    faceQuery = qCreatedBy(id + suffix, EntityType.FACE);
                }
                else
                {
                    faceQuery = wrapFaceQueryInCopy(definition.entitiesToOffset, id + suffix);
                    if (isQueryEmpty(context, faceQuery))
                        throw regenError(ErrorStringEnum.BOOLEAN_OFFSET_NO_FACES, ["entitiesToOffset"]);
                }
            }

            const tempMoveFaceSuffix = "offsetMoveFace";
            const moveFaceDefinition = {
                    "moveFaces" : faceQuery,
                    "moveFaceType" : MoveFaceType.OFFSET,
                    "offsetDistance" : definition.offsetDistance,
                    "reFillet" : definition.reFillet };

            opOffsetFace(context, id + tempMoveFaceSuffix, moveFaceDefinition);

            const doSheetMetalBooleans = shouldPerformSheetMetalAwareBooleans(context, definition);

            const toolBodies = qCreatedBy(id + suffix, EntityType.BODY);
            const tempBooleanDefinition = {
                    "operationType" : definition.operationType,
                    "tools" : toolBodies,
                    "targets" : definition.targets,
                    "keepTools" : doSheetMetalBooleans };

            const tempBooleanSuffix = "tempBoolean";
            if (doSheetMetalBooleans)
            {
                try(sheetMetalAwareBoolean(context, id + tempBooleanSuffix, tempBooleanDefinition));
            }
            else
            {
                try(opBoolean(context, id + tempBooleanSuffix, tempBooleanDefinition));
            }
            processSubfeatureStatus(context, id, { "subfeatureId" : id + tempBooleanSuffix, "propagateErrorDisplay" : true });
            if (doSheetMetalBooleans)
            {
                opDeleteBodies(context, id + "deleteTemp", { "entities" : toolBodies });
            }
            if (!definition.keepTools)
            {
                opDeleteBodies(context, id + "delete", { "entities" : definition.tools });
            }
        }
        else
        {
            //Between versions 179 and 1017 SUBTRACT_COMPLEMENT processing was handled on FS side
            var isSubtractComplement = false;
            if (definition.operationType == BooleanOperationType.SUBTRACT_COMPLEMENT &&
                isAtVersionOrLater(context, FeatureScriptVersionNumber.V179_SUBTRACT_COMPLEMENT_HANDLED_IN_FS) &&
                !isAtVersionOrLater(context, FeatureScriptVersionNumber.V1017_SUBTRACT_COMPLEMENT))
            {
                isSubtractComplement = true;
                definition.tools = constructToolsComplement(context, id, definition);
                definition.operationType = BooleanOperationType.SUBTRACTION;
                definition.keepTools = false;
            }
            if (shouldPerformSheetMetalAwareBooleans(context, definition))
            {
                sheetMetalAwareBoolean(context, id, definition);
            }
            else
            {
                opBoolean(context, id, definition);
            }
            if (isSubtractComplement)
            {
                var errorMessage = getFeatureInfo(context, id);
                if (errorMessage == ErrorStringEnum.BOOLEAN_SUBTRACT_NO_OP)
                {
                    reportFeatureInfo(context, id, ErrorStringEnum.BOOLEAN_INTERSECT_NO_OP);
                }
            }
        }
    }, { keepTools : false, offset : false, oppositeDirection : false, offsetAll : false, reFillet : false });

function shouldPerformSheetMetalAwareBooleans(context is Context, definition is map) returns boolean
{
    return definition.targets != undefined && isAtVersionOrLater(context, FeatureScriptVersionNumber.V440_SYNTAX_ERRORS);
}

function wrapFaceQueryInCopy(query is Query, id is Id) returns Query
{
    if (query.queryType == QueryType.UNION)
    {
        return qUnion(mapArray(query.subqueries, q => wrapFaceQueryInCopy(q, id)));
    }
    return makeQuery(id, "COPY", EntityType.FACE, { "derivedFrom" : query, "instanceName" : "1" });
}

/**
 * Build a block large enough to contain all tools and targets. Subtract tools from it.
 */
function constructToolsComplement(context is Context, id is Id, booleanDefinition is map) returns Query
{
    const inputTools = evaluateQuery(context, booleanDefinition.tools); // save tools here to avoid qCreatedBy confusion

    const boxResult = evBox3d(context, { "topology" : qUnion([booleanDefinition.tools, booleanDefinition.targets]) });
    const extendedBox is Box3d = extendBox3d(boxResult, 0. * meter, 0.1);
    const boxId is Id = id + "containingBox";
    fCuboid(context, boxId, { "corner1" : extendedBox.minCorner, "corner2" : extendedBox.maxCorner });

    const complementId = id + "toolComplement";
    const complementDefinition = {
            "operationType" : BooleanOperationType.SUBTRACTION,
            "tools" : qUnion(inputTools),
            "targets" : qCreatedBy(boxId, EntityType.BODY),
            "keepTools" : booleanDefinition.keepTools };
    opBoolean(context, complementId, complementDefinition);
    return qCreatedBy(boxId, EntityType.BODY); // Subtraction modifies target tool
}

/**
 * Maps a [NewBodyOperationType] (used in features like [extrude]) to its corresponding [BooleanOperationType].
 */
export function convertNewBodyOpToBoolOp(operationType is NewBodyOperationType) returns BooleanOperationType
{
    return {
                NewBodyOperationType.ADD : BooleanOperationType.UNION,
                NewBodyOperationType.REMOVE : BooleanOperationType.SUBTRACTION,
                NewBodyOperationType.INTERSECT : BooleanOperationType.SUBTRACT_COMPLEMENT
            }[operationType];
}

/**
 * Predicate which specifies a field `operationType` of type [NewBodyOperationType].
 * Used by body-creating feature preconditions such as extrude, revolve, sweep or loft.
 *
 * When used in a precondition, [NewBodyOperationType] creates UI like the extrude
 * feature, with a horizontal list of the words "New", "Add", etc. When using this
 * predicate in features, make sure to export an import of `tool.fs` so that [NewBodyOperationType]
 * is visible to the Part Studios:
 * ```
 * export import(path : "onshape/std/tool.fs", version : "");
 * ```
 *
 * @param booleanDefinition : @autocomplete `definition`
 */
export predicate booleanStepTypePredicate(booleanDefinition is map)
{
    annotation { "Name" : "Result body operation type", "UIHint" : UIHint.HORIZONTAL_ENUM }
    booleanDefinition.operationType is NewBodyOperationType;
}

/**
 * Used by body-creating feature preconditions to allow post-creation booleans,
 * specifying the merge scope (or "Merge with all") for that boolean.
 *
 * Designed to be used together with [booleanStepTypePredicate].
 *
 * @param booleanDefinition : @autocomplete `definition`
 */
export predicate booleanStepScopePredicate(booleanDefinition is map)
{
    if (booleanDefinition.operationType != NewBodyOperationType.NEW)
    {
        if (booleanDefinition.defaultScope != undefined)
        {
            annotation { "Name" : "Merge with all", "Default" : false }
            booleanDefinition.defaultScope is boolean;
            if (booleanDefinition.defaultScope != true)
            {
                annotation { "Name" : "Merge scope", "Filter" : EntityType.BODY && BodyType.SOLID && ModifiableEntityOnly.YES && AllowMeshGeometry.YES }
                booleanDefinition.booleanScope is Query;
            }
        }
    }
}

/**
 * Used by body-creating pattern feature preconditions to allow post-creation booleans with surfaces or solids,
 * specifying the merge scope (or "Merge with all") for that boolean.
 *
 * @param booleanDefinition : @autocomplete `definition`
 */
export predicate booleanPatternScopePredicate(booleanDefinition is map)
{
    if (booleanDefinition.operationType != NewBodyOperationType.NEW)
    {
        if (booleanDefinition.defaultScope != undefined)
        {
            annotation { "Name" : "Merge with all", "Default" : false }
            booleanDefinition.defaultScope is boolean;
            if (booleanDefinition.defaultScope != true)
            {
                // In reality surfaces are allowed as targets only in
                // surface + surfaces and surface - solid.
                // Unfortunately, we can't check for that in precondition
                // It will be enforced during execution
                annotation { "Name" : "Merge scope", "Filter" : (EntityType.BODY && AllowMeshGeometry.YES) &&
                            (BodyType.SOLID || (BodyType.SHEET && ConstructionObject.NO && SketchObject.NO))
                            && ModifiableEntityOnly.YES }
                booleanDefinition.booleanScope is Query;
            }
        }
    }
}

/**
 * Constructs a map with tools and targets queries for boolean operations. For operations where
 * seed needs to be part of the tools for the boolean, set the "seed" parameter in the definition.
 *
 * @param context {Context}
 * @param id {Id}: identifier of the tools feature
 * @param definition {map} : See `definition` of [preocessNewBodyIfNeeded] for details.
 * @returns {{
 *    @field targets {Query}: targets to use
 *    @field tools {Query}: tools to use
 *    @field targetsAndToolsNeedGrouping {boolean}: target and tool grouping to use in [opBoolean]
 * }}
 */
function subfeatureToolsTargets(context is Context, id is Id, definition is map) returns map
{
    // Fill defaults
    definition = mergeMaps({ "seed" : qNothing(), "defaultScope" : true }, definition);
    const resultQuery = qBodyType(qCreatedBy(id, EntityType.BODY), BodyType.SOLID);

    var seedQuery = definition.seed;
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V263_SURFACE_PATTERN_BOOLEAN))
        seedQuery = qBodyType(definition.seed, BodyType.SOLID);

    const defaultTools = qUnion([seedQuery, resultQuery]);

    var output = {};
    output.tools = defaultTools;

    if (definition.defaultScope != false)
    {
        if (isAtInitialMixedModelingReleaseVersionOrLater(context))
        {
            output.targets = qAllModifiableSolidBodies();
        }
        else
        {
            output.targets = qAllModifiableSolidBodiesNoMesh();
        }
    }
    else
    {
        output.targets = definition.booleanScope;
    }
    output.targets = qSubtraction(output.targets, defaultTools);

    if (definition.mergeScopeExclusion != undefined)
    {
        output.targets = qSubtraction(output.targets, definition.mergeScopeExclusion);
    }
    output.targetsAndToolsNeedGrouping = true;

    // We treat boolean slightly differently, as tools/targets are in select cases interchangeable.
    // (This logic comes from the fact that grouping of tools/targets has a significant effect on output.)
    if (definition.operationType == NewBodyOperationType.ADD &&
        isQueryEmpty(context, output.targets))
    {
        if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V712_SKIP_TARGET_BOOLEAN))
        {
            // BEL-37474 made this behave as if we were just using all tools and targets as tools with no grouping
            output.tools = resultQuery;
            output.targets = definition.seed;
        }
        else
        {
            // Always keep the seed in the tools.  Do not group if we have seeds.
            if (!isQueryEmpty(context, seedQuery))
            {
                output.targetsAndToolsNeedGrouping = false;
            }
        }
    }

    return output;
}

/**
 * This function is designed to be used by body-creating features (like [extrude]) as a boolean post-processing
 * step with options from [booleanStepTypePredicate] and [booleanStepScopePredicate] in the case where the preceding
 * operations of the feature have created new solid or surface bodies.
 * On top of the regular boolean operation, converts the `operationType` and creates error bodies on failure.
 * @param id : identifier of the main feature
 * @param definition {{
 *      @field operationType {NewBodyOperationType}:
 *              @eg `NewBodyOperationType.ADD` performs a boolean union
 *              @eg `NewBodyOperationType.NEW` does nothing
 *      @field defaultScope {boolean}: @optional
 *              @eg `true`  indicates merge scope of "everything else" (default)
 *              @eg `false` indicates merge scope is specified in `booleanScope`
 *      @field booleanScope {Query}: targets to use if `defaultScope` is false
 *      @field seed {Query}: @optional
 *              If set, will be included in the tools section of the boolean.
 *      @field mergeScopeExclusion {Query}: @optional
 *              If set, will be excluded from the targets section of the boolean.
 * }}
 * @param reconstructOp {function}: A function which takes in an Id, and reconstructs the input to show to the user
 *      as error geometry in case the input is problematic or the boolean itself fails.
 *      @eg `function() {}`. For a more elaborate example see the source code of revolve feature in the Standard Library.
 */
export function processNewBodyIfNeeded(context is Context, id is Id, definition is map, reconstructOp is function)
{
    if (definition.operationType == NewBodyOperationType.NEW)
        return;

    const solidsQuery = qModifiableEntityFilter(qBodyType(qCreatedBy(id, EntityType.BODY), BodyType.SOLID));

    var booleanDefinition = subfeatureToolsTargets(context, id, definition);
    if (definition.operationType != NewBodyOperationType.REMOVE && queryContainsActiveSheetMetal(context, booleanDefinition.targets))
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_CAN_ONLY_REMOVE, [], booleanDefinition.targets);
    }

    booleanDefinition.eraseImprintedEdges = definition.eraseImprintedEdges;
    booleanDefinition.operationType = convertNewBodyOpToBoolOp(definition.operationType);
    booleanDefinition.allowSheets = definition.allowSheets;

    if (isQueryEmpty(context, booleanDefinition.tools))
    {
        var errorEnum = ErrorStringEnum.BOOLEAN_NEED_ONE_SOLID;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V263_SURFACE_PATTERN_BOOLEAN))
        {
            errorEnum = ErrorStringEnum.FEATURE_NO_SOLIDS;
        }
        throw regenError(errorEnum, solidsQuery);
    }
    if (booleanDefinition.targetsAndToolsNeedGrouping && isQueryEmpty(context, booleanDefinition.targets))
        throw regenError(ErrorStringEnum.BOOLEAN_NEED_ONE_SOLID, ["booleanScope"], solidsQuery);

    const boolId = id + "boolean";
    try(booleanBodies(context, boolId, booleanDefinition));
    processSubfeatureStatus(context, id, { "subfeatureId" : boolId, "propagateErrorDisplay" : true });
    if (featureHasNonTrivialStatus(context, boolId))
    {
        const errorId = id + "errorEntities";
        try silent
        {
            reconstructOp(errorId);
            var qError = qCreatedBy(errorId, EntityType.BODY);
            // For the needs of pattern processPatternBooleansIfNeeded we need to highlight just solids
            // in case of info and warning but everything in case of true error
            if (getFeatureError(context, boolId) == undefined &&
                definition.operationType == NewBodyOperationType.ADD)
            {
                qError = qBodyType(qError, BodyType.SOLID);
            }
            setErrorEntities(context, id, { "entities" : qError });
            opDeleteBodies(context, errorId + "delete", { "entities" : qCreatedBy(errorId, EntityType.BODY) });
        }
        catch (e)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2540_APPX_CURVE_ENCLOSE_FIXES))
            {
                const reconstructedEntities = qCreatedBy(errorId, EntityType.BODY);
                if (!isQueryEmpty(context, reconstructedEntities))
                {
                    opDeleteBodies(context, errorId + "delete", { "entities" : reconstructedEntities });
                }

            }
            if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V736_SM_74))
                throw e;
        }
    }
}

/**
 * Predicate which specifies a field `surfaceOperationType` of type [NewSurfaceOperationType].
 * Used by surface-creating feature preconditions such as revolve, sweep or loft.
 *
 * When used in a precondition, [NewSurfaceOperationType] creates UI like the sweep
 * feature, with a horizontal list of the words "New" and "Add". When using this
 * predicate in features, make sure to export an import of `tool.fs` so that [NewSurfaceOperationType]
 * is visible to the Part Studios:
 * ```
 * export import(path : "onshape/std/tool.fs", version : "");
 * ```
 *
 * @param surfaceDefinition : @autocomplete `definition`
 */
export predicate surfaceOperationTypePredicate(surfaceDefinition is map)
{
    annotation { "Name" : "Result body operation type", "UIHint" : UIHint.HORIZONTAL_ENUM }
    surfaceDefinition.surfaceOperationType is NewSurfaceOperationType;
}

/**
 * Used by surface-creating feature preconditions to allow post-creation booleans,
 * specifying the merge scope (or "Merge with all") for that boolean.
 *
 * Designed to be used together with [surfaceOperationTypePredicate].
 *
 * @param definition : @autocomplete `definition`
 */

export predicate surfaceJoinStepScopePredicate(definition is map)
{
    if (definition.surfaceOperationType != NewSurfaceOperationType.NEW)
    {
        if (definition.defaultSurfaceScope != undefined)
        {
            annotation { "Name" : "Merge with all", "Default" : true }
            definition.defaultSurfaceScope is boolean;
            if (definition.defaultSurfaceScope != true)
            {
                annotation { "Name" : "Merge scope", "Filter" : EntityType.BODY && BodyType.SHEET && ModifiableEntityOnly.YES &&
                            AllowMeshGeometry.YES && SketchObject.NO }
                definition.booleanSurfaceScope is Query;
            }
        }
    }
}

/**
 * @internal
 * Used by features using surface boolean heuristics
 */
export function filterJoinableSurfaceEdges(edges is Query) returns Query
{
    return qEdgeTopologyFilter(qSketchFilter(edges, SketchObject.NO), EdgeTopology.ONE_SIDED);
}

/**
 * @internal
 */
function filterOverlappingEdges(context is Context, targetEdge is Query, edges is Query, transform is Transform) returns Query
{
    var useTolerantCheck = isAtVersionOrLater(context, FeatureScriptVersionNumber.V607_HOLE_FEATURE_FIT_UPDATE);

    var midPoint = transform * evEdgeTangentLine(context, {
                    "edge" : targetEdge,
                    "parameter" : 0.5,
                    "arcLengthParameterization" : !useTolerantCheck
                }).origin;

    if (useTolerantCheck)
    {
        return qWithinRadius(edges, midPoint, TOLERANCE.booleanDefaultTolerance * meter);
    }
    else
    {
        return qContainsPoint(edges, midPoint);
    }
}

/**
 * @internal
 */
function getJoinableSurfaceEdgeFromParentEdge(context is Context, id is Id, parentEdge is Query, transform is Transform) returns Query
{
    var track = filterOverlappingEdges(context, parentEdge, filterJoinableSurfaceEdges(startTracking(context,
            { "subquery" : parentEdge, "trackPartialDependency" : true, "lastOperationId" : lastModifyingOperationId(context, parentEdge) })), transform);

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V776_SURFACE_JOIN_BUG_FIX))
    {
        return qSubtraction(track, qCreatedBy(id));
    }

    var trackedEdges = evaluateQuery(context, qSubtraction(track, qCreatedBy(id)));

    if (size(trackedEdges) == 1)
    {
        return trackedEdges[0];
    }

    return qNothing();
}

/**
 * @internal
 */
function createJoinMatch(topology1 is Query, topology2 is Query) returns map
{
    return { "topology1" : topology1, "topology2" : topology2, "matchType" : TopologyMatchType.COINCIDENT };
}

/**
 * @internal
 * Used by features using surface boolean heuristics
 */
export function surfaceOperationTypeEditLogic(context is Context, id is Id, definition is map,
    specifiedParameters is map, inputEdges is Query, hiddenBodies is Query)
{
    if (!specifiedParameters.surfaceOperationType)
    {
        var joinableEdges = evaluateQuery(context, filterJoinableSurfaceEdges(inputEdges));
        var anyJoinable = size(joinableEdges) > 0;
        if (!anyJoinable)
        {
            var otherEdges;
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V576_GET_WIRE_LAMINAR_DEPENDENCIES))
            {
                otherEdges = evaluateQuery(context, qEntityFilter(inputEdges, EntityType.EDGE));
            }
            else
            {
                otherEdges = evaluateQuery(context, qSketchFilter(inputEdges, SketchObject.YES));
            }

            for (var i = 0; i < size(otherEdges); i += 1)
            {
                var siblingEdges = getJoinableSurfaceEdgeFromParentEdge(context, id, otherEdges[i], identityTransform());
                if (!isQueryEmpty(context, qSubtraction(siblingEdges, qOwnedByBody(hiddenBodies, EntityType.EDGE))))
                {
                    anyJoinable = true;
                    break;
                }
            }
        }
        definition.surfaceOperationType = anyJoinable ? NewSurfaceOperationType.ADD : NewSurfaceOperationType.NEW;
    }
    return definition;
}

function filterByOwnerBody(context is Context, edges is Query, bodies is Query) returns Query
{
    var allEdges = evaluateQuery(context, edges);
    var filteredEdges = [];
    for (var edge in allEdges)
    {
        if (isQueryEmpty(context, qSubtraction(qOwnerBody(edge), bodies)))
        {
            filteredEdges = append(filteredEdges, edge);
        }
    }

    return qUnion(filteredEdges);
}

/**
 * @internal
 * Used by features using surface boolean.
 * Designed to be used together with [surfaceJoinStepScopePredicate].
 * @param context {Context}
 * @param id {Id}: Identifier of the feature
 * @param definition {{
 *      @field defaultSurfaceScope {boolean}: @optional
 *              @eg `true`  indicates merge scope of all the original and related surfaces used as input to create this surface (default)
 *              @eg `false` indicates merge scope is specified in `booleanSurfaceScope`
 *      @field booleanSurfaceScope {Query}: targets to use if `defaultSurfaceScope` is false
 * }}
 * @param created {Query}: All newly created edges to be considered in matching.
 * @param originating {Query} : All original input edges that were used to create the edges.
 * @param transform {Transform} : Remaining feature pattern transform
 */
export function createTopologyMatchesForSurfaceJoin(context is Context, id is Id, definition is map, created is Query, originating is Query, transform is Transform) returns array
{
    var createdEdges = evaluateQuery(context, qEdgeTopologyFilter(created, EdgeTopology.ONE_SIDED));
    var originatingEdges = filterJoinableSurfaceEdges(originating);
    var nonMatchedOriginatingEdges;
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V576_GET_WIRE_LAMINAR_DEPENDENCIES))
    {
        // Any edge that we didn't already match is a candidate
        nonMatchedOriginatingEdges = qSubtraction(qEntityFilter(originating, EntityType.EDGE), originatingEdges);
    }
    else
    {
        nonMatchedOriginatingEdges = qSketchFilter(originating, SketchObject.YES);
    }

    if (definition.defaultSurfaceScope == false)
    {
        if (isQueryEmpty(context, definition.booleanSurfaceScope))
        {
            throw regenError(ErrorStringEnum.BOOLEAN_NO_SURFACE_IN_MERGE_SCOPE, ["booleanSurfaceScope"], qCreatedBy(id, EntityType.BODY));
        }
        originatingEdges = filterByOwnerBody(context, originatingEdges, definition.booleanSurfaceScope);
    }

    var nCreatedEdges = size(createdEdges);
    const filterMatchesByOverlap = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1031_BODY_NET_IN_LOFT);

    var matches = makeArray(nCreatedEdges);
    var nMatches = 0;
    for (var i = 0; i < nCreatedEdges; i += 1)
    {
        var dependencies = qDependency(createdEdges[i]);
        var originals = evaluateQuery(context, filterOverlappingEdges(context, createdEdges[i], qIntersection([originatingEdges, dependencies]), identityTransform()));
        var nOriginalMatches = size(originals);
        var matchedEdge = undefined;
        if (nOriginalMatches == 1)
        {
            matchedEdge = originals[0];
        }
        else if (nOriginalMatches == 0)
        {
            var originalEdges = evaluateQuery(context, filterOverlappingEdges(context, createdEdges[i], qIntersection([nonMatchedOriginatingEdges, dependencies]), inverse(transform)));
            if (size(originalEdges) == 1)
            {
                var siblingEdges = getJoinableSurfaceEdgeFromParentEdge(context, id, originalEdges[0], transform);
                if (definition.defaultSurfaceScope == false)
                {
                    siblingEdges = filterByOwnerBody(context, siblingEdges, definition.booleanSurfaceScope);
                }
                if (filterMatchesByOverlap)
                {
                    siblingEdges = filterOverlappingEdges(context, createdEdges[i], siblingEdges, identityTransform());
                }
                const edges = evaluateQuery(context, siblingEdges);
                const nEdges = size(edges);
                const nBodies = size(evaluateQuery(context, qOwnerBody(siblingEdges)));
                if (nEdges == 1 ||
                    (nEdges > 0 && nBodies == 1 && filterMatchesByOverlap))
                {
                    matchedEdge = edges[0];
                }
            }
        }

        if (matchedEdge != undefined)
        {
            matches[nMatches] = createJoinMatch(matchedEdge, createdEdges[i]);
            nMatches += 1;
        }
    }

    return resize(matches, nMatches);
}

/**
 * @internal
 * Throws error if booleanSurfaceScope contains a surface that is not present in matches
 * Used by features using surface boolean.
 * Designed to be used together with [createTopologyMatchesForSurfaceJoin].
 */
export function checkForNotJoinableSurfacesInScope(context is Context, id is Id, definition is map, matches is array)
{
    if (definition.defaultSurfaceScope == false)
    {
        var allMatchTargets = [];
        for (var i = 0; i < size(matches); i += 1)
        {
            allMatchTargets = append(allMatchTargets, qOwnerBody(matches[i].topology1));
        }
        var notJoinableSurfacesInScope = qSubtraction(definition.booleanSurfaceScope, qUnion(allMatchTargets));
        if (!isQueryEmpty(context, notJoinableSurfacesInScope))
        {
            setErrorEntities(context, id, { "entities" : notJoinableSurfacesInScope });
            reportFeatureWarning(context, id, ErrorStringEnum.BOOLEAN_NO_SHARED_EDGE_WITH_SURFACE_IN_MERGE_SCOPE);
        }
    }
}

/**
 * @internal
 * Joins surface bodies at the matching edges.
 * @param context {Context}
 * @param id {Id}: identifier of the feature
 * @param matches {array}: Matching edges of the sheet bodies. Each matching element is a map with fields `topology1`, `topology2`
 *      and `matchType`; where `topology1` and `topology2` are a pair of matching edges of two sheet bodies and
 *      `matchType` is the type of match [TopologyMatchType] between them. Owner body of `matches[0].topology1` survives in the join operation.
 * @param reconstructOp {function}: A function which takes in an Id, and reconstructs the input to show to the user as error geometry
 *      in case the input is problematic or the join itself fails.
 * @param makeSolid {boolean}: Tries to join the surfaces into a solid
 */
export function joinSurfaceBodies(context is Context, id is Id, matches is array, makeSolid is boolean, reconstructOp is function)
{
    const joinId = id + "join";

    var nMatches = size(matches);
    if (nMatches == 0)
    {
        if (!featureHasNonTrivialStatus(context, id))
        {
            reportFeatureWarning(context, id, ErrorStringEnum.BOOLEAN_NO_TARGET_SURFACE);
        }
    }
    else
    {
        var tools = makeArray(nMatches * 2);
        for (var i = 0; i < nMatches; i += 1)
        {
            tools[i] = qOwnerBody(matches[i].topology1);
            tools[nMatches + i] = qOwnerBody(matches[i].topology2);
        }
        try
(
opBoolean(context, joinId, {
                        "allowSheets" : true,
                        "tools" : qUnion(tools),
                        "operationType" : BooleanOperationType.UNION,
                        "makeSolid" : makeSolid,
                        "eraseImprintedEdges" : true,
                        "matches" : matches,
                        "recomputeMatches" : true
                    }));
        processSubfeatureStatus(context, id, { "subfeatureId" : joinId, "propagateErrorDisplay" : true });
    }
    if (nMatches == 0 || featureHasNonTrivialStatus(context, joinId))
    {
        const errorId = id + "errorEntities";
        reconstructOp(errorId);
        setErrorEntities(context, id, { "entities" : qCreatedBy(errorId, EntityType.BODY) });
        opDeleteBodies(context, id + "delete", { "entities" : qCreatedBy(errorId, EntityType.BODY) });
    }
}

/**
 * @internal
 * A query that filters out non-modifiable or non-surface entities in subquery
 */
export function qModifiableSurface(subquery is Query) returns Query
{
    return qModifiableEntityFilter(
        qSketchFilter(
            qConstructionFilter(
                qBodyType(
                    qEntityFilter(subquery, EntityType.BODY),
                    BodyType.SHEET),
                ConstructionObject.NO),
            SketchObject.NO));
}

/**
 *  This function is designed to be used by surface-body-creating features (like [extrude]) as a boolean post-processing
 * step with options from [surfaceOperationTypePredicate ] and [surfaceJoinStepScopePredicate]. It detects matching edges of adjacent
 * bodies and joins surface bodies at these  edges.
 * @param context {Context}
 * @param id {Id}: identifier of the feature
 * @param definition {{
 *      @field defaultSurfaceScope {boolean}: @optional
 *              @eg `true`  indicates merge scope of all the original and related surfaces used as input to create this surface (default)
 *              @eg `false` indicates merge scope is specified in `booleanSurfaceScope`
 *      @field booleanSurfaceScope {Query}: @optional targets to use if `defaultSurfaceScope` is false
 *              Default is `qNothing()`
 *      @field seed {Query}: @optional
 *              Default is `qNothing()` If set, will be included in the tools section of the boolean.
 * }}
 * @param makeSolid {boolean}: Tries to join the surfaces into a solid
 * @param reconstructOp {function}: A function which takes in an Id, and reconstructs the input to show to the user as error geometry
 *      in case the input is problematic or the join itself fails.
 *      @eg `function() {}`. For a more elaborate example see the source code of revolve feature in the Standard Library.
 */
export function joinSurfaceBodiesWithAutoMatching(context is Context, id is Id, definition is map, makeSolid is boolean, reconstructOp is function)
{
    joinSurfaceBodiesWithAutoMatching(context, id, mergeMaps(definition, { "makeSolid" : makeSolid }), reconstructOp);
}

/**
 *  This function is designed to be used by surface-body-creating features (like [extrude]) as a boolean post-processing
 * step with options from [surfaceOperationTypePredicate ] and [surfaceJoinStepScopePredicate]. It detects matching edges of adjacent
 * bodies and joins surface bodies at these  edges.
 * @param context {Context}
 * @param id {Id}: identifier of the feature
 * @param definition {{
 *      @field defaultSurfaceScope {boolean}: @optional
 *              @eg `true`  indicates merge scope of all the original and related surfaces used as input to create this surface (default)
 *              @eg `false` indicates merge scope is specified in `booleanSurfaceScope`
 *      @field booleanSurfaceScope {Query}: @optional targets to use if `defaultSurfaceScope` is false
 *              Default is `qNothing()`
 *      @field seed {Query}: @optional
 *              Default is `qNothing()` If set, will be included in the tools section of the boolean.
 *      @field makeSolid {boolean}: @optional Tries to join the surfaces into a solid
 *              Default is false
 *      @field eraseImprintedEdges {boolean}: @optional Merge all mergeable imprinted edges created by the boolean operation
 *              Default is true
 * }}
 * @param reconstructOp {function}: A function which takes in an Id, and reconstructs the input to show to the user as error geometry
 *      in case the input is problematic or the join itself fails.
 *      @eg `function() {}`. For a more elaborate example see the source code of revolve feature in the Standard Library.
 */
export function joinSurfaceBodiesWithAutoMatching(context is Context, id is Id, definition is map, reconstructOp is function)
{
    const seeded = definition.seed != undefined;
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V1215_BOOLEANS_OF_SURFACES) &&
        definition.defaultSurfaceScope == undefined && !seeded)
    {
        return;
    }

    const joinId = id + "join";
    // Need to add seed surfaces if defined.
    const entities = seeded ? qUnion([definition.seed, qCreatedBy(id)]) : qCreatedBy(id);
    const tools = qModifiableSurface(entities);
    const contextTargets = qSubtraction(qModifiableSurface(qEverything()), tools);

    var targets = undefined;
    if (definition.defaultSurfaceScope != false)
    {
        if (!isQueryEmpty(context, contextTargets))
        {
            targets = contextTargets;
        }
    }
    else if (definition.booleanSurfaceScope != undefined &&
        !isQueryEmpty(context, qModifiableSurface(definition.booleanSurfaceScope)))
    {
        targets = qModifiableSurface(definition.booleanSurfaceScope);
    }
    // otherwise join feature surfaces between themselves but not to merge scope.

    if (!seeded)
    {
        if (definition.defaultSurfaceScope == true)
        {
            if (targets == undefined)
            {
                throw regenError(ErrorStringEnum.BOOLEAN_NO_SURFACE_TO_MERGE_WITH, qCreatedBy(id, EntityType.BODY));
            }
        }
        else if (targets == undefined)
        {
            throw regenError(ErrorStringEnum.BOOLEAN_NO_SURFACE_IN_MERGE_SCOPE,
                ["booleanSurfaceScope"], qCreatedBy(id, EntityType.BODY));
        }
    }

    try
    {
        const noImpliedDetection =
        !isAtVersionOrLater(context, FeatureScriptVersionNumber.V1417_IMPLIED_DETECT_ADJACENCY);
        opBoolean(context, joinId, {
                    "operationType" : BooleanOperationType.UNION,
                    "makeSolid" : definition.makeSolid,
                    "eraseImprintedEdges" : (definition.eraseImprintedEdges == undefined) || definition.eraseImprintedEdges,
                    "detectAdjacencyForSheets" : noImpliedDetection,
                    "recomputeMatches" : true,
                    "tools" : tools,
                    "targets" : targets,
                    "targetsAndToolsNeedGrouping" : targets != undefined
                });
    }
    processSubfeatureStatus(context, id, { "subfeatureId" : joinId, "propagateErrorDisplay" : true });

    if (featureHasNonTrivialStatus(context, joinId))
    {
        const errorId = id + "errorSurfaces";
        reconstructOp(errorId);
        // For the needs of pattern processPatternBooleansIfNeeded we need to highlight just surfaces
        // in case of info and warning but everything in case of true error
        var qError = qCreatedBy(errorId, EntityType.BODY);
        if (getFeatureError(context, joinId) == undefined) // no need to version display data
        {

            qError = qModifiableSurface(qError);
        }
        setErrorEntities(context, id, { "entities" : qError });
        opDeleteBodies(context, id + "deleteSurfaces", { "entities" : qCreatedBy(errorId, EntityType.BODY) });
    }
}

function performRegularBoolean(context is Context, id is Id, definition is map)
{
    try(opBoolean(context, id, definition));
}

function copyBodies(context is Context, id is Id, bodies is Query) returns Query
{
    const copyId = id + "bodyCopy";
    opPattern(context, copyId, {
                "entities" : bodies,
                "transforms" : [identityTransform()],
                "instanceNames" : ["copy"]
            });
    return qCreatedBy(copyId, EntityType.BODY);
}

function sheetMetalAwareBoolean(context is Context, id is Id, definition is map)
{
    const parts = partitionSheetMetalParts(context, definition.targets);
    if (size(parts.sheetMetalPartsMap) == 0)
    {
        performRegularBoolean(context, id, definition);
    }
    else if (definition.operationType != BooleanOperationType.SUBTRACTION)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_CAN_ONLY_SUBTRACT);
    }
    else
    {
        const handleNoOpResults = isAtVersionOrLater(context, FeatureScriptVersionNumber.V630_SM_BOOLEAN_NOOP_HANDLING);
        const deleteToolsAtEnd = !definition.keepTools;
        definition.keepTools = true;
        var evaluatedOriginalTools = qUnion(evaluateQuery(context, definition.tools));
        var booleanWasNoOp = true;
        if (!isQueryEmpty(context, parts.nonSheetMetalPartsQuery))
        {
            definition.targets = parts.nonSheetMetalPartsQuery;
            performRegularBoolean(context, id, definition);
            if (!statusIsNoOp(context, id))
            {
                booleanWasNoOp = false;
            }
        }
        // The query for the tools could change if it uses qCreatedBy(top-level-id), for example
        // because subsequent operations are adding more bodies with different IDs
        // so substitute the evaluated original tools
        definition.tools = evaluatedOriginalTools;

        checkNotInFeaturePattern(context, definition.targets, ErrorStringEnum.SHEET_METAL_BLOCKED_PATTERN);
        var index = 0;
        for (var idAndParts in parts.sheetMetalPartsMap)
        {
            const subId = id + unstableIdComponent(index);
            index += 1;
            const booleanId = subId + "tempSMBoolean";

            definition.sheetMetalPart = qUnion(idAndParts.value);

            try(defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
                        {
                            var reportAnError = false;
                            try
                            {
                                const sheetMetalModel = try(getSheetMetalModelForPart(context, definition.sheetMetalPart));
                                if (sheetMetalModel == undefined)
                                {
                                    // Currently qEverything will return flat sheet metal so when booleaning with everything
                                    // We won't find the sheet metal model
                                    // That's fine, we'll handle it for now by skipping out in this case.
                                    // When we have fixed up qEverything we will want to revert this so that we catch genuine
                                    // unexpected issues
                                    // See BEL-54458
                                    return;
                                }
                                const initialData = getInitialEntitiesAndAttributes(context, sheetMetalModel);
                                const trackedSheets = trackModelBySheet(context, sheetMetalModel);
                                const trackedTwoSidedEdges = trackTwoSidedEdges(context, sheetMetalModel);

                                const robustSMModel = qUnion([startTracking(context, sheetMetalModel), sheetMetalModel]);

                                const useRobustForTargets = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1009_SM_BOOLEAN_TRACK);
                                definition.targets = useRobustForTargets ? robustSMModel : sheetMetalModel;

                                const modifiedFaceArray = performSheetMetalBoolean(context, id, definition);

                                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V748_SM_FAIL_SHEET_DELETION)
                                    && !eachSheetStillExists(context, trackedSheets))
                                {
                                    reportFeatureError(context, id, ErrorStringEnum.SHEET_METAL_SUBTRACT_DESTROYS_SHEET);
                                    return;
                                }

                                var modifiedEntityArray = modifiedFaceArray;
                                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V736_SM_74))
                                {
                                    const modifiedEdgeArray = removeJointAttributesFromOneSidedEdges(context, robustSMModel);
                                    modifiedEntityArray = concatenateArrays([modifiedEntityArray, modifiedEdgeArray]);
                                }

                                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1050_HEM_REATED_FIXES))
                                {
                                    const newlyLaminarEdges = evaluateQuery(context, qEdgeTopologyFilter(trackedTwoSidedEdges, EdgeTopology.ONE_SIDED));
                                    modifiedEntityArray = concatenateArrays([modifiedEntityArray, newlyLaminarEdges]);
                                }

                                if (modifiedEntityArray != [] || !isAtVersionOrLater(context, FeatureScriptVersionNumber.V630_SM_BOOLEAN_NOOP_HANDLING))
                                {
                                    const modifiedEntities = qUnion(modifiedEntityArray);
                                    const toUpdate = assignSMAttributesToNewOrSplitEntities(context, robustSMModel, initialData, id);

                                    updateSheetMetalGeometry(context, id + "smUpdate", {
                                                "entities" : qUnion([toUpdate.modifiedEntities, modifiedEntities]),
                                                "deletedAttributes" : toUpdate.deletedAttributes });
                                }
                            }
                            catch
                            {
                                reportAnError = true;
                            }
                            processSubfeatureStatus(context, id, { "subfeatureId" : id + "smUpdate", "propagateErrorDisplay" : true });
                            if (reportAnError && (getFeatureError(context, id) == undefined))
                            {
                                reportFeatureError(context, id, ErrorStringEnum.REGEN_ERROR);
                            }
                        }, {})(context, booleanId, definition));
            var propagateStatus = true;
            if (statusIsNoOp(context, booleanId))
            {
                if (handleNoOpResults)
                {
                    propagateStatus = false;
                }
            }
            else
            {
                booleanWasNoOp = false;
            }
            if (propagateStatus)
            {
                processSubfeatureStatus(context, id, { "subfeatureId" : booleanId, "propagateErrorDisplay" : true });
            }
        }
        if (deleteToolsAtEnd)
        {
            opDeleteBodies(context, id + "deleteTools", { "entities" : evaluatedOriginalTools });
        }
        if (handleNoOpResults && booleanWasNoOp && getFeatureError(context, id) == undefined && getFeatureWarning(context, id) == undefined)
        {
            reportBooleanNoOpWarning(context, id, definition);
        }
    }
}

function trackModelBySheet(context is Context, sheetMetalModel is Query) returns array
{
    return mapArray(evaluateQuery(context, sheetMetalModel),
                    sheetOfModel => qUnion([sheetOfModel, startTracking(context, sheetOfModel)]));
}

function trackTwoSidedEdges(context is Context, sheetMetalModel is Query) returns Query
{
    const twoSidedEdges = qEdgeTopologyFilter(qOwnedByBody(sheetMetalModel, EntityType.EDGE), EdgeTopology.TWO_SIDED);
    return qUnion([qUnion(evaluateQuery(context, twoSidedEdges)), startTracking(context, twoSidedEdges)]);
}

function eachSheetStillExists(context is Context, sheetTracking is array) returns boolean
{
    const checkForWalls = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1050_HEM_REATED_FIXES);
    const wallAttributePattern = asSMAttribute({ "objectType" : SMObjectType.WALL });
    for (var sheet in sheetTracking)
    {
        if (isQueryEmpty(context, sheet))
        {
            return false;
        }
        const wallQ = qAttributeFilter(qOwnedByBody(sheet, EntityType.FACE), wallAttributePattern);
        if (checkForWalls && isQueryEmpty(context, wallQ))
        {
            return false;
        }
    }
    return true;
}

function thickenFaces(context is Context, id is Id, modelParameters is map, faces is Query) returns Query
{
    opThicken(context, id, {
                "entities" : faces,
                "thickness1" : modelParameters.frontThickness,
                "thickness2" : modelParameters.backThickness
            });
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V708_SM_BOOLEAN))
    {
        return qCreatedBy(id, EntityType.BODY);
    }
    else
    {
        return qOwnerBody(qCreatedBy(id));
    }
}

function trimTool(context is Context, id is Id, thickened is Query, tool is Query) returns Query
{
    const intersectionId = id + "intersect";
    var toolsQ = isAtVersionOrLater(context, FeatureScriptVersionNumber.V779_BOOLEAN_TRACK_MERGE) ? qUnion([thickened, tool]) : qUnion([tool, thickened]);
    opBoolean(context, intersectionId, {
                "tools" : toolsQ,
                "operationType" : BooleanOperationType.INTERSECTION,
                "keepTools" : true
            });
    return qOwnerBody(qCreatedBy(intersectionId));
}

function createOutline(context is Context, id is Id, parentId, trimmed is Query, face is Query, faceIsPlanar is boolean, capFacesTracking is Query) returns Query
{
    const offsetFaces = qIntersection([capFacesTracking, qOwnedByBody(trimmed, EntityType.FACE)]);
    if (!faceIsPlanar && isAtVersionOrLater(context, FeatureScriptVersionNumber.V1170_ROLLED_OUTLINE_REVERT))
    {
        for (var oneTrimmed in evaluateQuery(context, trimmed))
        {
            if (size(evaluateQuery(context, qIntersection([qOwnedByBody(oneTrimmed, EntityType.FACE), offsetFaces]))) != 2)
            {
                if (parentId != undefined)
                {
                    reportFeatureError(context, parentId, ErrorStringEnum.SHEET_METAL_TOOL_DOES_NOT_CUT_THROUGH);
                }
                throw regenError(ErrorStringEnum.SHEET_METAL_TOOL_DOES_NOT_CUT_THROUGH);
            }
        }
    }

    const outlineId = id + "outline";
    opCreateOutline(context, outlineId, {
                "tools" : trimmed,
                "target" : face,
                "offsetFaces" : offsetFaces
            });
    return qCreatedBy(outlineId, EntityType.FACE);
}

function toolsSet(context, tools is Query) returns box
{
    return new box(evaluateQuery(context, tools)->foldArray({}, (soFar, next) => soFar->mergeMaps([next], true)));
}


/**
 * @internal
 * returns undefined if no tools were created or a Query for tool bodies created
 */
export function createBooleanToolsForFace(context is Context, id is Id, face is Query, tools is Query, modelParameters is map)
{
    const toolsToCopy = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1953_SM_BOOLEAN_COPY_ADJACENT_TOOLS_FIX) ? toolsSet(context, tools) : new box({});
    const faceSweptDataCache = makeFaceSweptDataCache(context);
    const outlineBodiesQ = createOutlineBooleanToolsForFace(context, id, undefined, face, undefined, evaluateQuery(context, tools), undefined,
        modelParameters, toolsToCopy, faceSweptDataCache, undefined);
    if (outlineBodiesQ == undefined) //outlineBodiesQ will be qNothing if no outline bodies were created, but a toolCopy is needed.
        return undefined;

    if (toolsToCopy[] != {})
    {
        const toolsToCopyQ = qUnion(keys(toolsToCopy[]));
        if (!isQueryEmpty(context, toolsToCopyQ))
        {
            const copies = copyBodies(context, id + "copyTools", toolsToCopyQ);
            return qUnion([outlineBodiesQ, copies]);
        }
    }
    return outlineBodiesQ;
}

//No longer used - turned out to not be sufficiently thin
const SM_THIN_EXTENSION_LEGACY = 1.e-4 * meter;

/**
 * @internal
 * If provided, faceBox should be a Box3d.  If provided, toolToThickenedToolBox should be a map from transient queries of
 * `tools` to their thickened Box3ds.  If either is not provided, bounding box testing will not be executed.
 * toolsToCopy is a boxed set of transient queries of tools whose copy can be used instead of an outline.  Because FS does not provide a set structure,
 * this is implemented as a map from transiet queries to `true`.
 * faceSweptDataCache is a cache of face transient queries to a map with surface characteristics as collected in sweptAlong.
 * `copyToolToFaceData` should be a persistent box (initially set to an empty map) that is used to internally track which faces each copy tool is responsible
 * for. If set to `undefined`, the assumption is that the tool will not intersect another face.
 * When `createOutlineBooleanToolsForFace` encounters a face for which the copy tool doesn't work, `copyToolToFaceData` is referenced to generate all the
 * outlines for faces intersecting the to-be-removed copy tool.
 * Returns undefined if no tool is necessary (tools don't intersect thickened body), or a query for outline bodies created. It might be a qNothing
 * if all tools were added to toolsToCopy.
 */
function createOutlineBooleanToolsForFace(context is Context, id is Id, parentId, face is Query, faceBox, toolsArray is array,
    toolToThickenedToolBox, modelParameters is map, toolsToCopy, faceSweptDataCache is function, copyToolToFaceData)
{
    var outlines = [];
    var allTrimmed = [];
    var thickened = undefined;
    var capFacesQ = undefined;
    var skippedAll = true;
    const clashInfoProvided = (faceBox != undefined && toolToThickenedToolBox != undefined);
    const useFineClashing = isAtVersionOrLater(context, FeatureScriptVersionNumber.V913_TOOL_CLASH_FINE) && clashInfoProvided;
    var toolsOut;
    const toolsOptOutFromCopy = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1953_SM_BOOLEAN_COPY_ADJACENT_TOOLS_FIX);
    const computePreviousFaceOutlinesWhenCopyHasOptedOut = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1977_SM_BOOLEAN_FIX) && copyToolToFaceData != undefined;
    const removeFaceFromToolCopyData = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1985_SM_BOOLEAN_FIX);
    const faceData = faceSweptDataCache(face);
    const planarFace = (faceData.planeNormal != undefined);
    for (var index = 0; index < size(toolsArray); index += 1)
    {
        const tool = toolsArray[index];
        if (useFineClashing)
        {
            if (!clashBoxes(faceBox, toolToThickenedToolBox[tool]))
            {
                continue;
            }
        }
        const copyCanBeUsed = (toolsToCopy[][tool] == true || !toolsOptOutFromCopy) && planarFace &&
            (determineToolUsage(context, face, tool, faceSweptDataCache) == ToolUsage.USE_COPY);
        if (copyCanBeUsed)
        {
            if (!toolsOptOutFromCopy)
            {
                toolsToCopy[][tool] = true;
            }
            toolsOut = qNothing(); // the face is counted as modified
            if (computePreviousFaceOutlinesWhenCopyHasOptedOut)
            {
                copyToolToFaceData[] = insertIntoMapOfArrays(copyToolToFaceData[], tool, { 'face' : face, 'faceBox' : faceBox, 'id' : id });
            }
            continue;
        }
        else if (toolsOptOutFromCopy)
        {
            toolsToCopy[][tool] = undefined;
            if (computePreviousFaceOutlinesWhenCopyHasOptedOut && copyToolToFaceData[][tool] != undefined)
            {
                // Create all surface tools that were assumed to be acounted for in the copy.
                for (var faceDetails in copyToolToFaceData[][tool])
                {
                    outlines = append(outlines, createOutlineBooleanToolsForFace(context, faceDetails.id, parentId, faceDetails.face, faceDetails.faceBox, toolsArray,
                            toolToThickenedToolBox, modelParameters, toolsToCopy, faceSweptDataCache, undefined));
                }
                if (removeFaceFromToolCopyData)
                {
                    copyToolToFaceData[][tool] = undefined;
                }
            }
        }

        // Lazy creation of thickened face only when we need it.
        if (skippedAll)
        {
            thickened = thickenFaces(context, id + "thicken", modelParameters, face);
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V646_SM_MULTI_TOOL_FIX))
            {
                // The intersection of the thickened body with the tool creates multiple bits and
                // they get evaluated by the original 'thickened' query in subsequent passes through the loop.
                // We don't want that and so we will evaluate the thickened body first
                thickened = qUnion(evaluateQuery(context, thickened));
            }
            capFacesQ = qCapEntity(id + "thicken", CapType.EITHER, EntityType.FACE);
            skippedAll = false;
        }

        const subId = id + unstableIdComponent(index);
        var trackingCapFaces = startTracking(context, capFacesQ);
        const trimmed = trimTool(context, subId, thickened, tool);
        var trimResultIsValid = false;
        if (trimmed != undefined)
        {
            const pieceCount = size(evaluateQuery(context, trimmed));
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V492_MULTIPLE_OUTLINE_PIECES))
            {
                trimResultIsValid = pieceCount > 0;
            }
            else
            {
                trimResultIsValid = pieceCount == 1;
            }
        }
        if (trimResultIsValid)
        {
            allTrimmed = append(allTrimmed, trimmed);
            const outline = createOutline(context, subId, parentId, trimmed, face, planarFace, trackingCapFaces);
            if (outline != undefined)
            {
                outlines = append(outlines, outline);
            }
        }
    }

    if (!skippedAll)
    {
        var thin = SM_THIN_EXTENSION_LEGACY;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1050_HEM_REATED_FIXES))
        {
            thin = min(thin, 0.2 * modelParameters.minimalClearance);
        }
        var toDeleteArray = append(allTrimmed, thickened);
        if (size(outlines) > 0)
        {
            toolsOut = qOwnerBody(qUnion(outlines));
            if (!planarFace)
            {
                toDeleteArray = append(toDeleteArray, toolsOut);
                toolsOut = thickenFaces(context, id + "thickenTools",
                    { "frontThickness" : thin, "backThickness" : thin }, qUnion(outlines));
            }
        }
        opDeleteBodies(context, id + "deleteThickened", { "entities" : qUnion(toDeleteArray) });
    }
    return toolsOut;
}

function performOneSheetMetalSurfaceBoolean(context is Context, topLevelId is Id, id is Id, definition is map, handleErrors is boolean)
{
    try(opBoolean(context, id, definition));
    if (handleErrors)
    {
        if (id != topLevelId)
        {
            processSubfeatureStatus(context, topLevelId, { "subfeatureId" : id, "propagateErrorDisplay" : true });
        }
        const error = getFeatureError(context, topLevelId);
        if (error != undefined)
        {
            throw error;
        }
    }
}

function performSheetMetalSurfaceBoolean(context is Context, id is Id, definition is map, targets is Query, tools is Query, matches is array)
{
    const handleErrors = isAtVersionOrLater(context, FeatureScriptVersionNumber.V951_FAIL_SURFACE_BOOLEAN);
    const useAutoMatching = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2668_SM_CONE);

    definition.allowSheets = true;
    definition.targets = targets;
    definition.keepTools = false;
    const sheetTools = evaluateQuery(context, qBodyType(tools, BodyType.SHEET));
    if (size(sheetTools) > 0)
    {
        definition.tools = qUnion(sheetTools);
        if (useAutoMatching)
            definition.recomputeMatches = true;
        else
            definition.matches = matches;
        performOneSheetMetalSurfaceBoolean(context, id, id, definition, handleErrors);
    }
    const solidTools = evaluateQuery(context, qBodyType(tools, BodyType.SOLID));
    if (size(solidTools) > 0)
    {
        definition.tools = qUnion(solidTools);
        // Matches are only provided for overlapping regions of sheet targets.
        definition.matches = [];
        performOneSheetMetalSurfaceBoolean(context, id, id + "solid", definition, handleErrors);
    }
}

function clashBoxes(a is Box3d, b is Box3d) returns boolean
{
    for (var dim in [0, 1, 2])
    {
        if (a.minCorner[dim] > b.maxCorner[dim] || b.minCorner[dim] > a.maxCorner[dim])
        {
            return false;
        }
    }
    return true;
}

function getThickenedBox(context is Context, topology is Query, thickness is ValueWithUnits)
{
    const topologyBox = try(evBox3d(context, {
                    "topology" : topology,
                    "tight" : true
                }));
    if (topologyBox == undefined)
    {
        return undefined;
    }
    return try(extendBox3d(topologyBox, thickness, 0));
}

function performSheetMetalBoolean(context is Context, id is Id, definition is map) returns array
{
    const modelParameters = try(getModelParameters(context, definition.targets));
    if (modelParameters == undefined)
    {
        throw regenError(ErrorStringEnum.REGEN_ERROR);
    }
    var thickness = modelParameters.frontThickness + modelParameters.backThickness;
    if (modelParameters.frontThickness > 0 && modelParameters.backThickness > 0)
    {
        thickness = thickness * 0.5;
    }
    // We get the faces not from the targets but from the faces of the source part that have associations
    const definitionEntities = qUnion(getSMDefinitionEntities(context, qOwnedByBody(definition.sheetMetalPart, EntityType.FACE)));
    const facesQ = qEntityFilter(definitionEntities, EntityType.FACE);
    var faceArray = evaluateQuery(context, facesQ);

    // Create one bounding boxes encompassing all of the tools, and one bounding box for each tool.
    // Doesn't matter which box we extend (face vs. tools). Extending the tool box reduces what we do.
    const thickenedToolsBox = getThickenedBox(context, definition.tools, thickness);
    if (thickenedToolsBox == undefined)
    {
        throw regenError(ErrorStringEnum.REGEN_ERROR);
    }
    var toolToThickenedToolBox = {};
    const toolsArray = evaluateQuery(context, definition.tools);
    // Before this version, toolToThickenedToolBox is not used, so we do not need to fill it.
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V913_TOOL_CLASH_FINE))
    {
        for (var tool in toolsArray)
        {
            toolToThickenedToolBox[tool] = getThickenedBox(context, tool, thickness);
            if (toolToThickenedToolBox[tool] == undefined)
            {
                throw regenError(ErrorStringEnum.REGEN_ERROR);
            }
        }
    }

    const faceSweptDataCache = makeFaceSweptDataCache(context);
    var allToolBodies = [];
    var modifiedFaces = [];
    var matches = [];
    const toolsToCopy = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1953_SM_BOOLEAN_COPY_ADJACENT_TOOLS_FIX) ? toolsSet(context, definition.tools) : new box({});
    const provideBooleanMatches = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2423_SM_BOOLEAN_USE_SHEET_TOOL);
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1990_SM_BOOLEAN_SIMPLIFIED))
    {
        const toolsPerFace = checkCanCopyTools(context, faceArray, toolsArray, thickenedToolsBox, toolToThickenedToolBox, faceSweptDataCache, toolsToCopy);

        if (toolsPerFace == {})
        {
            reportFeatureInfo(context, id, ErrorStringEnum.BOOLEAN_SUBTRACT_NO_OP);
            return [];
        }

        var index = 0;
        for (var face in faceArray)
        {
            if (toolsPerFace[face] == undefined)
                continue;
            const toBuildOutlineQ = qSubtraction(qUnion(toolsPerFace[face]), qUnion(keys(toolsToCopy[])));
            if (isQueryEmpty(context, toBuildOutlineQ))
                continue;
            const planarFace = faceSweptDataCache(face).planeNormal != undefined;
            const faceTools = createOutlineBooleanToolsForFaceNoChecks(context, id + unstableIdComponent(index), id, face, planarFace,
                toBuildOutlineQ, modelParameters);
            if (!isQueryEmpty(context, faceTools))
            {
                allToolBodies = append(allToolBodies, faceTools);
                if (provideBooleanMatches)
                {
                    for (var toolFace in evaluateQuery(context, qOwnedByBody(faceTools, EntityType.FACE)))
                    {
                        matches = matches->append({ "topology1" : toolFace, "topology2" : face, "matchType" : TopologyMatchType.OVERLAPING });
                    }
                }
            }
            index += 1;
        }
        modifiedFaces = keys(toolsPerFace);
    }
    else
    {
        var index = 0;
        // A box that contains a map from copy tool to faces intersected, so that surface tools may be built in the event of copy tools being subsequently discarded.
        const copyToolToFaceData = new box({});
        for (var face in faceArray)
        {
            index += 1;

            const faceBox = evBox3d(context, {
                        "topology" : face,
                        "tight" : true
                    });
            if (faceBox == undefined)
            {
                continue;
            }

            if (!clashBoxes(faceBox, thickenedToolsBox))
            {
                continue;
            }
            const toolBodies = createOutlineBooleanToolsForFace(context, id + unstableIdComponent(index), id, face, faceBox,
                toolsArray, toolToThickenedToolBox, modelParameters, toolsToCopy, faceSweptDataCache, copyToolToFaceData);
            if (toolBodies != undefined)
            {
                allToolBodies = append(allToolBodies, toolBodies);
                modifiedFaces = append(modifiedFaces, face);
            }
        }
    }
    const toolsToCopyQ = qUnion(keys(toolsToCopy[]));
    if (!isQueryEmpty(context, toolsToCopyQ))
    {
        const copies = copyBodies(context, id + "copyTools", toolsToCopyQ);
        allToolBodies = append(allToolBodies, copies);
    }

    if (size(allToolBodies) == 0 && isAtVersionOrLater(context, FeatureScriptVersionNumber.V630_SM_BOOLEAN_NOOP_HANDLING))
    {
        reportBooleanNoOpWarning(context, id, definition);
    }
    else
    {
        performSheetMetalSurfaceBoolean(context, id, definition, definition.targets, qUnion(allToolBodies), matches);
    }
    return modifiedFaces;
}

/**
 * removes tools which require outline from toolsToCopy, returns a map of face to array of tools clashing with it.
 **/
function checkCanCopyTools(context is Context, faceArray is array, toolsArray is array, thickenedToolsBox is Box3d,
    toolToThickenedToolBox is map, faceSweptDataCache is function, toolsToCopy is box) returns map
{
    var faceToFaceBox = {};
    var considerFaces = [];
    for (var face in faceArray)
    {
        const faceBox = evBox3d(context, {
                    "topology" : face,
                    "tight" : true
                });
        if (faceBox == undefined || !clashBoxes(faceBox, thickenedToolsBox))
        {
            continue;
        }
        faceToFaceBox[face] = faceBox;
        considerFaces = append(considerFaces, face);
    }

    if (considerFaces == [])
    {
        return {};
    }

    const outOfLimitFacesQ = qSubtraction(qUnion(faceArray)->qOwnerBody()->qOwnedByBody(EntityType.FACE), qUnion(faceArray));
    var toolsPerFace = {};
    for (var tool in toolsArray)
    {
        if (!isQueryEmpty(context, outOfLimitFacesQ))
        {
            const collisionWithOthers = evCollision(context, {
                        "tools" : qOwnedByBody(tool, EntityType.FACE),
                        "targets" : outOfLimitFacesQ
                    });

            if (collisionWithOthers != [])
                toolsToCopy[][tool] = undefined;
        }
        const toolBox = toolToThickenedToolBox[tool];
        for (var face in considerFaces)
        {
            const faceBox = faceToFaceBox[face];
            if (!clashBoxes(faceBox, toolBox))
            {
                continue;
            }
            if (toolsToCopy[][tool] != undefined)
            {
                const faceData = faceSweptDataCache(face);
                const planarFace = (faceData.planeNormal != undefined);
                const toolUsage = ((planarFace) ? determineToolUsage(context, face, tool, faceSweptDataCache) : ToolUsage.MAKE_OUTLINE);
                if (toolUsage == ToolUsage.NO_CLASH) // the tool will not be added to toolsPerFace for this face
                    continue;
                else if (toolUsage == ToolUsage.MAKE_OUTLINE)
                    toolsToCopy[][tool] = undefined;
            }
            toolsPerFace = insertIntoMapOfArrays(toolsPerFace, face, tool);
        }
    }
    return toolsPerFace;
}

function createOutlineBooleanToolsForFaceNoChecks(context is Context, id is Id, parentId is Id, face is Query, planarFace is boolean, toolsQ is Query, modelParameters is map) returns Query
{
    var faceTools = qNothing();
    var thickened = thickenFaces(context, id + "thicken", modelParameters, face);
    thickened = qUnion(evaluateQuery(context, thickened));
    var capFacesQ = qCapEntity(id + "thicken", CapType.EITHER, EntityType.FACE);

    const tools = evaluateQuery(context, toolsQ);
    var allTrimmed = [];
    var outlines = [];
    for (var toolIdx = 0; toolIdx < size(tools); toolIdx += 1)
    {
        const subId = id + unstableIdComponent(toolIdx);
        const trackingCapFaces = startTracking(context, capFacesQ);
        const trimmed = trimTool(context, subId, thickened, tools[toolIdx]);
        if (trimmed != undefined && !isQueryEmpty(context, trimmed))
        {
            allTrimmed = append(allTrimmed, trimmed);
            const outline = createOutline(context, subId, parentId, trimmed, face, planarFace, trackingCapFaces);
            if (outline != undefined)
            {
                outlines = append(outlines, outline);
            }
        }
    }

    var toDeleteArray = append(allTrimmed, thickened);
    if (outlines != [])
    {
        faceTools = qOwnerBody(qUnion(outlines));
        if (!planarFace && !isAtVersionOrLater(context, FeatureScriptVersionNumber.V2423_SM_BOOLEAN_USE_SHEET_TOOL))
        {
            const thin = min(SM_THIN_EXTENSION_LEGACY, 0.2 * modelParameters.minimalClearance);
            toDeleteArray = append(toDeleteArray, faceTools);
            faceTools = thickenFaces(context, id + "thickenTools",
                { "frontThickness" : thin, "backThickness" : thin }, qUnion(outlines));
        }
    }
    opDeleteBodies(context, id + "deleteThickened", { "entities" : qUnion(toDeleteArray) });
    return faceTools;
}

function reportBooleanNoOpWarning(context is Context, id is Id, definition is map)
{
    if (getFeatureWarning(context, id) == undefined && getFeatureError(context, id) == undefined)
    {
        if (definition.operationType == BooleanOperationType.SUBTRACTION)
        {
            reportFeatureInfo(context, id, ErrorStringEnum.BOOLEAN_SUBTRACT_NO_OP);
        }
        else if (definition.operationType == BooleanOperationType.INTERSECTION)
        {
            reportFeatureInfo(context, id, ErrorStringEnum.BOOLEAN_INTERSECT_NO_OP);
        }
        else if (definition.operationType == BooleanOperationType.UNION)
        {
            reportFeatureInfo(context, id, ErrorStringEnum.BOOLEAN_UNION_NO_OP);
        }
    }
}

function statusIsNoOp(context is Context, id is Id) returns boolean
{
    const info = getFeatureInfo(context, id);
    return info == ErrorStringEnum.BOOLEAN_SUBTRACT_NO_OP ||
        info == ErrorStringEnum.BOOLEAN_INTERSECT_NO_OP ||
        info == ErrorStringEnum.BOOLEAN_UNION_NO_OP;
}

enum ToolUsage
{
    USE_COPY,
    MAKE_OUTLINE,
    NO_CLASH
}

function determineToolUsage(context is Context, smFace is Query, tool is Query, faceSweptDataCache is function) returns ToolUsage
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V918_SM_BOOLEAN_TOOLS))
    {
        return ToolUsage.MAKE_OUTLINE;
    }
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1482_DONT_COPY_SM_TOOLS) && queryContainsActiveSheetMetal(context, tool))
    {
        return ToolUsage.MAKE_OUTLINE; // Cannot copy sheet metal parts.
    }
    const faceData = faceSweptDataCache(smFace);

    //BEL-105231. Starting V948 we check that tool clashes with definition face and both associated model faces.
    const collideWithModelFaces = isAtVersionOrLater(context, FeatureScriptVersionNumber.V948_BOOLEAN_TOOLS_STRICTER);
    const targetFaces = (collideWithModelFaces) ? addAssociatedFaces(context, smFace) : [smFace];
    const targetQ = qUnion(targetFaces);
    const collisionData = evCollision(context, {
                "tools" : qOwnedByBody(tool, EntityType.FACE),
                "targets" : targetQ
            });

    if (collisionData == [])
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2679_SM_BOOLEAN_CONTAINED))
        {
            // There might be no face collision, but containment in tool body
            const collisionWToolBody = evCollision(context, {
                    "tools" : tool,
                    "targets" : targetQ
                });
            for (var collision in collisionWToolBody)
            {
                const collisionType = collision['type'];
                if (collisionType == ClashType.TARGET_IN_TOOL && isQueryEmpty(context, qSubtraction(collision.target, smFace)))
                {
                    return ToolUsage.USE_COPY;
                }
            }
            if (collisionWToolBody != [])
                return ToolUsage.MAKE_OUTLINE;
        }
        return ToolUsage.NO_CLASH;
    }

    // facesToCheck collects intersecting faces in tools that either INTERFERE or ABUT target faces or are adjacent to abutting edges
    var facesToCheck = [];
    // intersectingFacesToTargetFaces collects target faces with which intersecting tool faces collide,
    // key is transient id of an intersecting tool face, value is a map of target face transient id to true ( used in lieu of set)
    var intersectingFacesToTargetFaces = {};
    for (var collision in collisionData)
    {
        // collision.target is either one of targetFaces or an edge adjacent to them - recover corresponding target face.
        const targetFaceAdjacentToEdgeInCollisionQ = [qEntityFilter(collision.target, EntityType.EDGE)->qAdjacent(AdjacencyType.EDGE, EntityType.FACE), targetQ]->qIntersection();
        const targetFaceQ = qUnion([qEntityFilter(collision.target, EntityType.FACE), targetFaceAdjacentToEdgeInCollisionQ]);
        const targetFaces = evaluateQuery(context, targetFaceQ);
        if (size(targetFaces) != 1)
        {
            return ToolUsage.MAKE_OUTLINE;
        }

        const collisionType = collision['type'];
        if (collisionType == ClashType.TARGET_IN_TOOL)
        {
            if (collideWithModelFaces) // having more target faces we have to keep checking
                continue;
            else
                return ToolUsage.USE_COPY;
        }
        if (collisionType == ClashType.TOOL_IN_TARGET)
            continue;

        var faceToCheckQ;
        if (collisionType == ClashType.INTERFERE)
        {
            faceToCheckQ = qEntityFilter(collision.tool, EntityType.FACE); // We don't care for edge interference
        }
        else // some sort of Abutting, I've seen only ABUT_NO_CLASS
        {
            const edgeAdjacentQ = qAdjacent(qEntityFilter(collision.tool, EntityType.EDGE), AdjacencyType.EDGE, EntityType.FACE);
            const parallelToPlaneQ = qParallelPlanes(edgeAdjacentQ, faceData.planeNormal);
            faceToCheckQ = qUnion([qEntityFilter(collision.tool, EntityType.FACE), qSubtraction(edgeAdjacentQ, parallelToPlaneQ)]);
        }
        const toolFaces = evaluateQuery(context, faceToCheckQ);
        if (toolFaces == [])
            continue;
        facesToCheck = append(facesToCheck, faceToCheckQ);
        for (var toolFace in toolFaces)
        {
            if (intersectingFacesToTargetFaces[toolFace] == undefined)
            {
                intersectingFacesToTargetFaces[toolFace] = { targetFaces[0] : true };
            }
            else
            {
                intersectingFacesToTargetFaces[toolFace][targetFaces[0]] = true;
            }
        }
    }
    // faceToCheck has to collide with all target faces and be orthogonal to smFace plane
    for (var faceToCheck in evaluateQuery(context, qUnion(facesToCheck)))
    {
        for (var targetFace in targetFaces)
        {
            if (intersectingFacesToTargetFaces[faceToCheck][targetFace] != true)
            {
                return ToolUsage.MAKE_OUTLINE;
            }
        }
        if (!sweptAlong(context, faceToCheck, faceData.planeNormal, faceSweptDataCache))
        {
            return ToolUsage.MAKE_OUTLINE;
        }
    }
    return ToolUsage.USE_COPY;
}

function addAssociatedFaces(context is Context, face is Query) returns array
{
    const associatedFacesQ = getSMCorrespondingInPart(context, face, EntityType.FACE);
    return append(evaluateQuery(context, associatedFacesQ), face);
}

