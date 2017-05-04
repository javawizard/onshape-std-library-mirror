FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/booleanoperationtype.gen.fs", version : "✨");
export import(path : "onshape/std/query.fs", version : "✨");
export import(path : "onshape/std/tool.fs", version : "✨");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/box.fs", version : "✨");
import(path : "onshape/std/boundingtype.gen.fs", version : "✨");
import(path : "onshape/std/clashtype.gen.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/primitives.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");
import(path : "onshape/std/transform.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");

/**
 * The boolean feature.  Performs an [opBoolean] after a possible [opOffsetFace] if the operation is subtraction.
 */
annotation { "Feature Type Name" : "Boolean", "Filter Selector" : "allparts" }
export const booleanBodies = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Operation type", "UIHint" : "HORIZONTAL_ENUM" }
        definition.operationType is BooleanOperationType;
        annotation { "Name" : "Tools", "Filter" : EntityType.BODY && BodyType.SOLID }
        definition.tools is Query;

        if (definition.operationType == BooleanOperationType.SUBTRACTION)
        {
            annotation { "Name" : "Targets", "Filter" : EntityType.BODY && BodyType.SOLID && ModifiableEntityOnly.YES }
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
                isLength(definition.offsetDistance, SHELL_OFFSET_BOUNDS);

                annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
                definition.oppositeDirection is boolean;

                annotation { "Name" : "Reapply fillet" }
                definition.reFillet is boolean;
            }
        }
        if (definition.operationType == BooleanOperationType.SUBTRACTION || definition.operationType == BooleanOperationType.INTERSECTION)
        {
            annotation { "Name" : "Keep tools" }
            definition.keepTools is boolean;
        }
    }
    {
        if (definition.offset && definition.operationType == BooleanOperationType.SUBTRACTION)
        {
            if (definition.oppositeDirection)
            {
                definition.offsetDistance = -definition.offsetDistance;
            }
            const suffix = "offsetTempBody";
            const transformMatrix = identityTransform();
            opPattern(context, id + suffix,
                    { "entities" : definition.tools,
                        "transforms" : [transformMatrix],
                        "instanceNames" : ["1"] });

            var faceQuery;
            if (definition.offsetAll)
            {
                faceQuery = qCreatedBy(id + suffix, EntityType.FACE);
            }
            else
            {
                faceQuery = wrapFaceQueryInCopy(definition.entitiesToOffset, id + suffix);
                if (size(evaluateQuery(context, faceQuery)) == 0)
                    throw regenError(ErrorStringEnum.BOOLEAN_OFFSET_NO_FACES, ["entitiesToOffset"]);
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
            var isSubtractComplement = false;
            if (definition.operationType == BooleanOperationType.SUBTRACT_COMPLEMENT &&
                isAtVersionOrLater(context, FeatureScriptVersionNumber.V179_SUBTRACT_COMPLEMENT_HANDLED_IN_FS))
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
        return qUnion(mapArray(query.subqueries, function(q)
                {
                    return wrapFaceQueryInCopy(q, id);
                }));
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
    annotation { "Name" : "Result body operation type", "UIHint" : "HORIZONTAL_ENUM" }
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
                annotation { "Name" : "Merge scope", "Filter" : EntityType.BODY && BodyType.SOLID && ModifiableEntityOnly.YES }
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
    output.targets = (definition.defaultScope != false) ? qAllModifiableSolidBodies() : definition.booleanScope;
    output.targets = qSubtraction(output.targets, defaultTools);

    // We treat boolean slightly differently, as tools/targets are in select cases interchangeable.
    // (This logic comes from the fact that grouping of tools/targets has a significant effect on output.)
    if (definition.operationType == NewBodyOperationType.ADD &&
        size(evaluateQuery(context, output.targets)) <= 0)
    {
        output.tools = resultQuery;
        output.targets = definition.seed;
    }

    return output;
}

/**
 * Performs a boolean operation (optionally). Used by body-creating features (like [extrude]) as the boolean step.
 * On top of the regular boolean feature, converts the `operationType` and creates error bodies on failure.
 *
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
 * }}
 * @param reconstructOp {function}: A function which takes in an Id, and reconstructs the input to show to the user as error geometry
 *      in case the input is problematic or the boolean itself fails.
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

    booleanDefinition.operationType = convertNewBodyOpToBoolOp(definition.operationType);

    if (size(evaluateQuery(context, booleanDefinition.tools)) == 0)
    {
        var errorEnum = ErrorStringEnum.BOOLEAN_NEED_ONE_SOLID;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V263_SURFACE_PATTERN_BOOLEAN))
        {
            errorEnum = ErrorStringEnum.FEATURE_NO_SOLIDS;
        }
        throw regenError(errorEnum, solidsQuery);
    }
    if (size(evaluateQuery(context, booleanDefinition.targets)) == 0)
        throw regenError(ErrorStringEnum.BOOLEAN_NEED_ONE_SOLID, ["booleanScope"], solidsQuery);

    booleanDefinition.targetsAndToolsNeedGrouping = true;
    const boolId = id + "boolean";
    try(booleanBodies(context, boolId, booleanDefinition));
    processSubfeatureStatus(context, id, { "subfeatureId" : boolId, "propagateErrorDisplay" : true });
    if (getFeatureWarning(context, boolId) != undefined || getFeatureInfo(context, boolId) != undefined)
    {
        const errorId = id + "errorEntities";
        reconstructOp(errorId);
        setErrorEntities(context, id, { "entities" : qCreatedBy(errorId, EntityType.BODY) });
        opDeleteBodies(context, id + "delete", { "entities" : qCreatedBy(errorId, EntityType.BODY) });
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
    annotation { "Name" : "Result body operation type", "UIHint" : "HORIZONTAL_ENUM" }
    surfaceDefinition.surfaceOperationType is NewSurfaceOperationType;
}


/**
 * @internal
 * Used by features using surface boolean heuristics
 */
export function filterJoinableSurfaceEdges(edges is Query) returns Query
{
    return qEdgeTopologyFilter(qSketchFilter(edges, SketchObject.NO), EdgeTopology.LAMINAR);
}

/**
 * @internal
 */
function getJoinableSurfaceEdgeFromParentEdge(context is Context, id is Id, parentEdge is Query, transform is Transform) returns Query
{
    var midPoint = evEdgeTangentLine(context, {
        "edge" : parentEdge,
        "parameter" : 0.5
    }).origin;

    var track = qContainsPoint(filterJoinableSurfaceEdges(startTracking(context,
                {"subquery" : parentEdge, "trackPartialDependency" : true, "lastOperationId" : lastModifyingOperationId(context, parentEdge) })), transform * midPoint);
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
export function surfaceOperationTypeEditLogic (context is Context, id is Id, definition is map,
                                           specifiedParameters is map, inputEdges is Query)
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
                if (size(evaluateQuery(context, getJoinableSurfaceEdgeFromParentEdge(context, id, otherEdges[i], identityTransform()))) > 0)
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

/**
 * @internal
 * Used by features using surface boolean
 */
export function createTopologyMatchesForSurfaceJoin(context is Context, id is Id, created is Query, originating is Query, transform is Transform) returns array
{
    var createdEdges = evaluateQuery(context, qEdgeTopologyFilter(created, EdgeTopology.LAMINAR));
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
    var nCreatedEdges = size(createdEdges);

    var matches = makeArray(nCreatedEdges);
    var nMatches = 0;
    for (var i = 0; i < nCreatedEdges; i += 1)
    {
        var midPoint = evEdgeTangentLine(context, {
            "edge" : createdEdges[i],
            "parameter" : 0.5
        }).origin;

        var originals = evaluateQuery(context, qContainsPoint(qIntersection([originatingEdges, qDependency(createdEdges[i])]), midPoint));
        var nOriginalMatches = size(originals);
        if (nOriginalMatches == 1)
        {
            matches[nMatches] = createJoinMatch(originals[0], createdEdges[i]);
            nMatches += 1;
        }
        else if (nOriginalMatches == 0)
        {
            var originalEdge = evaluateQuery(context, qContainsPoint(qIntersection([nonMatchedOriginatingEdges, qDependency(createdEdges[i])]), inverse(transform) * midPoint));
            if (size(originalEdge) == 1)
            {
                var edges = evaluateQuery(context, getJoinableSurfaceEdgeFromParentEdge(context, id, originalEdge[0], transform));
                if (size(edges) == 1)
                {
                    matches[nMatches] =  createJoinMatch(edges[0], createdEdges[i]);
                    nMatches += 1;
                }
            }
        }
    }

    return resize(matches, nMatches);
}
/**
 * Joins surface bodies at the matching edges.
 * @param context {Context}
 * @param id {Id}: identifier of the feature
 * @param matches {array}: Matching edges of the sheet bodies. Each matching element is a map with fields `topology1`, `topology2`
 *      and `matchType`; where `topology1` and `topology2` are a pair of matching edges of two sheet bodies and
 *      `matchType` is the type of match [TopologyMatchType] between them. Owner body of `matches[0].topology1` survives in the join operation.
 * @param reconstructOp {function}: A function which takes in an Id, and reconstructs the input to show to the user as error geometry
 *      in case the input is problematic or the join itself fails.
 */
export function joinSurfaceBodies(context is Context, id is Id, matches is array, reconstructOp is function)
{
    var nMatches = size(matches);
    if (nMatches == 0)
        throw regenError(ErrorStringEnum.BOOLEAN_NO_TARGET_SURFACE);

    var tools = makeArray(nMatches * 2);
    for (var i = 0; i < nMatches; i += 1)
    {
        tools[i] = qOwnerBody(matches[i].topology1);
        tools[nMatches + i] = qOwnerBody(matches[i].topology2);
    }
    const joinId = id + "join";
    try
    (
     opBoolean(context, joinId, {
        "allowSheets" : true,
        "tools" : qUnion(tools),
        "operationType" : BooleanOperationType.UNION,
        "makeSolid" : false,
        "eraseImprintedEdges" : true,
        "matches" : matches
    }));

    processSubfeatureStatus(context, id, { "subfeatureId" : joinId, "propagateErrorDisplay" : true });
    if (getFeatureWarning(context, joinId) != undefined || getFeatureInfo(context, joinId) != undefined)
    {
        const errorId = id + "errorEntities";
        reconstructOp(errorId);
        setErrorEntities(context, id, { "entities" : qCreatedBy(errorId, EntityType.BODY) });
        opDeleteBodies(context, id + "delete", { "entities" : qCreatedBy(errorId, EntityType.BODY) });
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
        const deleteToolsAtEnd = !definition.keepTools;
        definition.keepTools = true;
        var evaluatedOriginalTools = qUnion(evaluateQuery(context, definition.tools));
        if (size(evaluateQuery(context, parts.nonSheetMetalPartsQuery)) > 0)
        {
            definition.targets = parts.nonSheetMetalPartsQuery;
            performRegularBoolean(context, id, definition);
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

            try (defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
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
                    const originalEntities = evaluateQuery(context, qOwnedByBody(sheetMetalModel));
                    const initialAssociationAttributes = getAttributes(context, {
                        "entities" : qOwnedByBody(sheetMetalModel),
                        "attributePattern" : {} as SMAssociationAttribute});

                    definition.targets = sheetMetalModel;
                    const trackingSMModel = startTracking(context, sheetMetalModel);
                    const modifiedFaces = performSheetMetalBoolean(context, id, definition);

                    const toUpdate = assignSMAttributesToNewOrSplitEntities(context, qUnion([trackingSMModel, sheetMetalModel]),
                            originalEntities, initialAssociationAttributes);

                   updateSheetMetalGeometry(context, id + "smUpdate", {
                        "entities" : qUnion([toUpdate.modifiedEntities, modifiedFaces]),
                        "deletedAttributes" :  toUpdate.deletedAttributes});
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
            processSubfeatureStatus(context, id, { "subfeatureId" : booleanId, "propagateErrorDisplay" : true });
        }
        if (deleteToolsAtEnd)
        {
            opDeleteBodies(context, id + "deleteTools", { "entities" : evaluatedOriginalTools });
        }
    }
}

function thickenFace(context is Context, id is Id, thickness is ValueWithUnits, planarFace is Query) returns Query
{
    const thickenId = id + "thicken";
    opThicken(context, thickenId, {
                "entities" : planarFace,
                "thickness1" : thickness * 0.5,
                "thickness2" : thickness * 0.5
            });
    return qOwnerBody(qCreatedBy(thickenId));
}

function trimTool(context is Context, id is Id, thickened is Query, tool is Query) returns Query
{
    const intersectionId = id + "intersect";
    opBoolean(context, intersectionId, {
                "tools" : qUnion([tool, thickened]),
                "operationType" : BooleanOperationType.INTERSECTION,
                "keepTools" : true
            });
    return qOwnerBody(qCreatedBy(intersectionId));
}

function createOutline(context is Context, id is Id, trimmed is Query, planarFace is Query)
{
    const outlineId = id + "outline";
    opCreateOutline(context, outlineId, {
                "tools" : trimmed,
                "plane" : planarFace
            });
    return qCreatedBy(outlineId, EntityType.FACE);
}

function createBooleanToolsForFace(context is Context, id is Id, planarFace is Query, tool is Query, thickness is ValueWithUnits)
{
    const thickened = thickenFace(context, id, thickness, planarFace);
    const toolCount = size(evaluateQuery(context, tool));
    var tools = [];
    var allTrimmed = [];
    for (var index = 0; index < toolCount; index += 1)
    {
        const subId = id + unstableIdComponent(index);
        const trimmed = trimTool(context, subId, thickened, qNthElement(tool, index));
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
            const outline = createOutline(context, subId, trimmed, planarFace);
            if (outline != undefined)
            {
                tools = append(tools, outline);
            }
        }
    }
    opDeleteBodies(context, id + "deleteThickened", { "entities" : qUnion(append(allTrimmed, thickened)) });
    if (size(tools) > 0)
    {
        return qOwnerBody(qUnion(tools));
    }
    else
    {
        return undefined;
    }
}

function performSheetMetalSurfaceBoolean(context is Context, id is Id, definition is map, targetFaces is Query, toolFaces is Query, thickness is ValueWithUnits) returns boolean
{
    definition.allowSheets = true;
    definition.tools = toolFaces;
    definition.targets = targetFaces;
    definition.keepTools = false;
    try(opBoolean(context, id, definition));
    return true;
}

function clashBoxes(a is Box3d, b is Box3d) returns boolean {
    for (var dim in [0, 1, 2])
    {
        if (a.minCorner[dim] > b.maxCorner[dim] || b.minCorner[dim] > a.maxCorner[dim])
        {
            return false;
        }
    }
    return true;
}

function faceBoxClashesWithBox(context is Context, face is Query, toolBox is Box3d) returns boolean
{
    const faceBox = evBox3d(context, {
            "topology" : face,
            "tight" : true
    });
    if (faceBox == undefined) {
        return false;
    }
    else
    {
        return clashBoxes(toolBox, faceBox);
    }
}

function performSheetMetalBoolean(context is Context, id is Id, definition is map) returns Query
{
    const attributes = getSmObjectTypeAttributes(context, definition.targets, SMObjectType.MODEL);
    if (size(attributes) != 1 || attributes[0].thickness == undefined || attributes[0].thickness.value == undefined)
    {
        throw regenError(ErrorStringEnum.REGEN_ERROR);
    }
    var thickness = attributes[0].thickness.value;
    // We get the faces not from the targets but from the faces of the source part that have associations
    const definitionEntities = qUnion(getSMDefinitionEntities(context, qOwnedByBody(definition.sheetMetalPart, EntityType.FACE)));
    const facesQ = qEntityFilter(definitionEntities, EntityType.FACE);

    var planarFacesQ = qGeometry(facesQ, GeometryType.PLANE);
    var planarFaceArray = evaluateQuery(context, planarFacesQ);
    var index = 0;
    var allToolBodies = [];
    var modifiedFaces = [];
    const toolBox = try(evBox3d(context, {
            "topology" : definition.tools,
            "tight" : true
    }));
    if (toolBox == undefined) {
        throw regenError(ErrorStringEnum.REGEN_ERROR);
    }
    // Doesn't matter which box we extend. Extending the tool box reduces what we do
    const thickenedBox = try(extendBox3d(toolBox, thickness * 0.5, 0));
    for (var planarFace in planarFaceArray)
    {
        index += 1;
        if (!faceBoxClashesWithBox(context, planarFace, thickenedBox)) {
            continue;
        }

        const toolBodies = createBooleanToolsForFace(context, id + unstableIdComponent(index), planarFace, definition.tools, thickness);
        if (toolBodies != undefined)
        {
            allToolBodies = append(allToolBodies, toolBodies);
            modifiedFaces = append(modifiedFaces, planarFace);
        }
    }
    performSheetMetalSurfaceBoolean(context, id, definition, definition.targets, qUnion(allToolBodies), thickness);
    return qUnion(modifiedFaces);
}

