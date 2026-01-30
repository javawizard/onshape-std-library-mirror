FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/sheetMetalStart.fs", version : "✨");
export import(path : "onshape/std/tool.fs", version : "✨");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/boolean.fs", version : "✨");
import(path : "onshape/std/compositeCurve.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/debug.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/loft.fs", version : "✨");
import(path : "onshape/std/moveCurveBoundary.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/topologyUtils.fs", version : "✨");
import(path : "onshape/std/transform.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");

const CHORDAL_BOUNDS = {
            (meter) : [0.00001, 0.001, 0.1],
            (centimeter) : 0.1,
            (millimeter) : 1.0,
            (inch) : 0.05,
            (foot) : 0.005,
            (yard) : 0.001
        } as LengthBoundSpec;

/**
 * Create or add to existing sheet metal parts by selecting two profiles to loft.
 * Adjust chordal tolerance to change loft resolution. Rips are automatically added to closed profiles.
 * Operations on sheet metal models are automatically represented as a flat pattern, and joints and bends are listed in a table.
 */
annotation { "Feature Type Name" : "Sheet metal loft",
        "Editing Logic Function" : "tessLoftEditLogic",
        "Manipulator Change Function" : "tessLoftManipulator" }
export const sheetMetalLoft = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        surfaceOperationTypePredicate(definition);
        annotation { "Name" : "Profile 1",
                    "Filter" : ((EntityType.EDGE || EntityType.FACE || (EntityType.BODY && (BodyType.WIRE || BodyType.SHEET) && SketchObject.NO)) && ConstructionObject.NO)
                    || EntityType.VERTEX,
                    "AdditionalBoxSelectFilter" : (EntityType.EDGE && !EntityType.BODY) }
        definition.profile1 is Query;
        annotation { "Name" : "Profile 2",
                    "Filter" : ((EntityType.EDGE || EntityType.FACE || (EntityType.BODY && (BodyType.WIRE || BodyType.SHEET) && SketchObject.NO)) && ConstructionObject.NO)
                    || EntityType.VERTEX,
                    "AdditionalBoxSelectFilter" : (EntityType.EDGE && !EntityType.BODY) }
        definition.profile2 is Query;
        annotation { "Name" : "Connections" }
        definition.matchConnections is boolean;
        if (definition.matchConnections)
        {
            annotation { "Group Name" : "Connections", "Driving Parameter" : "matchConnections", "Collapsed By Default" : false }
            {
                annotation { "Name" : "Match connections", "Item name" : "connection", "UIHint" : UIHint.FOCUS_INNER_QUERY,
                            "Driven query" : "connectionEntities", "Item label template" : "#connectionEntities" }
                definition.connections is array;
                for (var connection in definition.connections)
                {
                    annotation { "Name" : "Vertices or edges",
                                "Filter" : (EntityType.EDGE && ConstructionObject.NO) || (EntityType.VERTEX) }
                    connection.connectionEntities is Query;
                    annotation { "Name" : "Rip", "Default" : false }
                    connection.isRip is boolean;
                    annotation { "Name" : "Edge queries", "UIHint" : UIHint.ALWAYS_HIDDEN }
                    connection.connectionEdgeQueries is Query; // Unioned array of individual edge queries synchronized with connectionEdgeParameters
                    // Synced array of edge parameters (numbers) defined in accordance with fsConnectionsArcLengthParameterization
                    annotation { "Name" : "Edge parameters", "UIHint" : UIHint.ALWAYS_HIDDEN }
                    isAnything(connection.connectionEdgeParameters);
                }
            }
        }

        annotation { "Name" : "Chordal tolerance" }
        isLength(definition.chordalTolerance, CHORDAL_BOUNDS);

        if (definition.surfaceOperationType == NewSurfaceOperationType.ADD)
        {
            annotation { "Name" : "Merge scope", "Filter" : EntityType.BODY && BodyType.SOLID && ModifiableEntityOnly.YES && ActiveSheetMetal.YES }
            definition.booleanScope is Query;
        }
        else
        {
            sheetMetalModelParameters(definition);
        }
    }
    {
        if (definition.surfaceOperationType == NewSurfaceOperationType.NEW)
        {
            checkNotInFeaturePattern(context, qUnion(definition.profile1, definition.profile2), ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);
        }
        else if (definition.surfaceOperationType == NewSurfaceOperationType.ADD)
        {
            checkNotInFeaturePattern(context, qUnion(definition.profile1, definition.profile2, definition.booleanScope), ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);
        }
        if (isQueryEmpty(context, definition.profile1))
        {
            throw regenError(ErrorStringEnum.TL_SELECT_PROFILES, ["profile1"]);
        }
        if (isQueryEmpty(context, definition.profile2))
        {
            throw regenError(ErrorStringEnum.TL_SELECT_PROFILES, ["profile2"]);
        }
        const distanceBetweenProfiles = evDistance(context, { "side0" : definition.profile1, "side1" : definition.profile2,
                    "arcLengthParameterization" : false });
        if (distanceBetweenProfiles.distance < TOLERANCE.zeroLength * meter)
        {
            addAuxiliaryPoint(context, id, distanceBetweenProfiles.sides[0].point, DebugColor.RED);
            throw regenError(ErrorStringEnum.TL_NO_INTERSECTING_PROFILES, ["profile1", "profile2"]);
        }
        if (definition.surfaceOperationType == NewSurfaceOperationType.ADD)
        {
            if (isQueryEmpty(context, definition.booleanScope))
            {
                throw regenError(ErrorStringEnum.BOOLEAN_NEED_ONE_SOLID, ["booleanScope"]);
            }
            if (!isQueryEmpty(context, definition.booleanScope->qActiveSheetMetalFilter(ActiveSheetMetal.NO)))
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_INACTIVE_MODEL_SELECTED, ["booleanScope"]);
            }
            if (!areEntitiesFromSingleActiveSheetMetalModel(context, definition.booleanScope))
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_SINGLE_MODEL_NEEDED, ["booleanScope"]);
            }
        }

        var ripPositions = addConnectionManipulators(context, id, definition);
        definition.profileSubqueries = [getProfileEdgesAndVertices(definition.profile1), getProfileEdgesAndVertices(definition.profile2)];
        if (definition.surfaceOperationType == NewSurfaceOperationType.ADD)
        {
            definition.booleanSurfaceScope = qUnion(getSMDefinitionEntities(context, definition.booleanScope));
            definition = transformProfileIfNeeded(context, id, definition, 0);
            definition = transformProfileIfNeeded(context, id, definition, 1);
            ripPositions = getRipPositions(context, definition);
        }
        definition = packDefinition(context, definition);
        const matchOutput = evTessellatedLoftMatches(context, definition);
        definition.connections = convertMatchesToConnections(matchOutput.matches);
        callSubfeatureAndProcessStatus(id, opTessellatedLoft, context, id + "loft", definition);
        try
        {
            var assignAttribsToLoft = true;
            if (definition.surfaceOperationType == NewSurfaceOperationType.ADD)
            {
                assignAttribsToLoft = false;
                const initialData = getInitialEntitiesAndAttributes(context, definition.booleanSurfaceScope);
                const definitionBodiesEdges = getTrackingQueriesForOneSidedDefinitionEdgesPerBodyBeingMerged(context, definition.booleanScope);
                var orientationCheckData;
                if (evaluateQueryCount(context, definition.booleanSurfaceScope) > 1)
                {
                    const definitionBodiesQ = definition.booleanSurfaceScope->qSubtraction(definition.booleanSurfaceScope->qNthElement(0));
                    orientationCheckData = getOrientationCheckData(context, definition.booleanScope, definitionBodiesQ);
                }
                joinSMDefinitionSurfaceBodiesWithAutoMatching(context, id, definition);
                opDeleteBodies(context, id + "deleteTempCurves", { "entities" : qCreatedBy(id + "transform", EntityType.BODY) });
                const booleanResult = getFeatureStatus(context, id + "join");
                if (booleanResult != undefined && booleanResult.statusEnum == ErrorStringEnum.BOOLEAN_UNION_NO_OP)
                {
                    assignAttribsToLoft = true;
                }
                else
                {
                    if (orientationCheckData != undefined)
                    {
                        checkOrientations(context, id, orientationCheckData);
                    }
                    var annotatedEntities = annotateNewSheetFaces(context, id);
                    var newEdges = getNewTwoSidedEdgesAndRipEdges(context, id, matchOutput.isClosed, ripPositions, matchOutput.matches, definitionBodiesEdges);
                    newEdges.ripEdges = newEdges.ripEdges->qUnion(getRipEdgesNeededForUnfold(context, newEdges.twoSidedEdges, definitionBodiesEdges));
                    annotatedEntities = annotatedEntities->concatenateArrays(annotateNewSheetEdges(context, id, newEdges, definition.modelParameters.bendRadius,
                            size(annotatedEntities)));
                    const toUpdate = assignSMAttributesToNewOrSplitEntities(context, definition.booleanSurfaceScope, initialData, id);
                    updateSheetMetalGeometry(context, id, {
                                "entities" : qUnion(toUpdate.modifiedEntities, qUnion(annotatedEntities)),
                                "deletedAttributes" : toUpdate.deletedAttributes
                            });
                }
            }

            if (assignAttribsToLoft)
            {
                annotateSheetBody(context, id, definition, matchOutput.isClosed, ripPositions, matchOutput.matches);
                assignSMAssociationAttributes(context, qCreatedBy(id + "loft"));
                updateSheetMetalGeometry(context, id, {
                            "entities" : qCreatedBy(id + "loft", EntityType.FACE)->qUnion(qCreatedBy(id + "loft", EntityType.EDGE))
                        });
            }
        }
        catch (error)
        {
            setErrorEntities(context, id, {
                        "entities" : qCreatedBy(id + "loft", EntityType.FACE),
                        "color" : DebugColor.YELLOW
                    });
            throw error;
        }
        if (definition.surfaceOperationType != NewSurfaceOperationType.ADD)
        {
            addFlipDirectionUpManipulator(qCreatedBy(id + "loft", EntityType.BODY), FLIP_DIRECTION_UP_MANIPULATOR_NAME, id, context, definition);
        }
    },
    { "matchConnections" : false,
            "connections" : [],
            "surfaceOperationType" : NewSurfaceOperationType.NEW,
            "kFactor" : 0.45,
            "kFactorRolled" : 0.5,
            "minimalClearance" : 2e-5 * meter,
            "oppositeDirection" : false,
            "defaultCornerStyle" : SMCornerStrategyType.RECTANGLE,
            "defaultCornerReliefScale" : 1.2,
            "defaultRoundReliefDiameter" : 0 * meter,
            "defaultSquareReliefWidth" : 0 * meter,
            "defaultBendReliefStyle" : SMBendStrategyType.OBROUND,
            "defaultBendReliefDepthScale" : 1.5,
            "defaultBendReliefScale" : 1.0625,
            "flipDirectionUp" : false,
            "bendCalculationType" : SMBendCalculationType.K_FACTOR
        });

const joinSMDefinitionSurfaceBodiesWithAutoMatching = function(context is Context, id is Id, definition is map)
    {
        const reconstructOp = function(id)
            {
                opTessellatedLoft(context, id, definition);
            };

        var joinDefinition = { "defaultSurfaceScope" : false };
        joinDefinition.booleanSurfaceScope = definition.booleanSurfaceScope;
        joinSurfaceBodiesWithAutoMatching(context, id, joinDefinition, reconstructOp);
    };

function convertMatchesToConnections(matches)
{
    var connections = [];

    for (var match in matches)
    {
        connections = connections->append(createConnectionFromMatch(match));
    }

    return connections;
}

function getLoftModelParameters(context is Context, definition is map) returns map
{
    if (definition.surfaceOperationType == NewSurfaceOperationType.ADD)
    {
        definition.booleanSurfaceScope = qUnion(getSMDefinitionEntities(context, definition.booleanScope));
        const params = getModelParameters(context, definition.booleanSurfaceScope);
        return {
                "bendRadius" : params.defaultBendRadius,
                "frontThickness" : params.frontThickness,
                "backThickness" : params.backThickness
            };
    }
    else
    {
        return {
                "bendRadius" : definition.radius,
                "frontThickness" : definition.oppositeDirection ? 0 * meter : definition.thickness,
                "backThickness" : definition.oppositeDirection ? definition.thickness : 0 * meter
            };
    }
}

/**
 * @internal
 */
export function getProfileEdgesAndVertices(profile is Query) returns Query
{
    return qUnion([qEntityFilter(profile, EntityType.EDGE),
                qEntityFilter(profile, EntityType.VERTEX),
                qEntityFilter(profile, EntityType.FACE)->qAdjacent(AdjacencyType.EDGE, EntityType.EDGE),
                qEntityFilter(profile, EntityType.BODY)->qBodyType(BodyType.WIRE)->qOwnedByBody(EntityType.EDGE),
                qEntityFilter(profile, EntityType.BODY)->qBodyType(BodyType.SHEET)->qOwnedByBody(EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.ONE_SIDED),
                qEntityFilter(profile, EntityType.BODY)->qBodyType(BodyType.POINT)->qOwnedByBody(EntityType.VERTEX)]);
}

function packDefinition(context is Context, definition is map) returns map
{
    definition.modelParameters = getLoftModelParameters(context, definition);

    if (!definition.matchConnections)
    {
        definition.connections = [];
        return definition;
    }
    else
    {
        for (var ii = 0; ii < size(definition.connections); ii += 1)
        {
            // opLoft expects an array of individual connection edge queries
            definition.connections[ii].connectionEdges =
                evaluateQuery(context, qEntityFilter(definition.connections[ii].connectionEdgeQueries, EntityType.EDGE));

            // Can happen if modeling changes happen upstream such that query evaluates to more or less entities, but
            // editing logic does not have a chance to run
            const hasEdgeParameterMismatch = size(definition.connections[ii].connectionEdges)
                != size(definition.connections[ii].connectionEdgeParameters);
            if (hasEdgeParameterMismatch)
            {
                throw regenError(ErrorStringEnum.LOFT_CONNECTION_MATCHING, ["connections[" ~ ii ~ "].connectionEntities"]);
            }
            definition.connections[ii].removeRedundantEdge = !definition.connections[ii].isRip;
        }
    }
    return definition;
}

function annotateNewSheetFaces(context is Context, id is Id) returns array
{
    var annotatedFaces = [];
    var count = 0;
    for (var face in evaluateQuery(context, qCreatedBy(id + "loft", EntityType.FACE)))
    {
        if (getAttributes(context, { "entities" : face,
                        "attributePattern" : asSMAttribute({}) }) == [])
        {
            setAttribute(context, {
                        "entities" : face,
                        "attribute" : makeSMWallAttribute(toAttributeId(id + count))
                    });
            count += 1;
        }
        annotatedFaces = append(annotatedFaces, face);
    }
    return annotatedFaces;
}

function annotateNewSheetEdges(context is Context, id is Id, newEdges is map, bendRadius is ValueWithUnits, count is number) returns array
{
    var annotatedEdges = [];
    const evaluatedRipEdges = evaluateQuery(context, newEdges.ripEdges);
    for (var edge in evaluateQuery(context, newEdges.twoSidedEdges))
    {
        var faces = evaluateQuery(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE));
        if (size(faces) != 2)
        {
            continue; // laminar edges cannot be rips, bends or tangent joints
        }
        const isRip = isIn(edge, evaluatedRipEdges);
        if (!isRip && size(getSmObjectTypeAttributes(context, qUnion(faces), SMObjectType.WALL)) != 2)
        {
            continue;
        }
        var jointAttribute = undefined;
        var angleVal = try silent(edgeAngle(context, edge));
        var zeroAngle = angleVal == undefined || angleVal < TOLERANCE.zeroAngle * radian;
        if (isRip)
        {
            jointAttribute = makeSMJointAttribute(toAttributeId(id + count));
            jointAttribute.jointType = { "value" : SMJointType.RIP, "canBeEdited" : true };
            if (angleVal != undefined)
            {
                jointAttribute.angle = { "value" : angleVal, "canBeEdited" : false };
            }
            if (!zeroAngle)
            {
                jointAttribute.jointStyle = { "value" : SMJointStyle.EDGE, "canBeEdited" : true };
            }
        }
        else if (zeroAngle)
        {
            jointAttribute = makeSMJointAttribute(toAttributeId(id + count));
            jointAttribute.jointType = { "value" : SMJointType.TANGENT, "canBeEdited" : true };
        }
        else
        {
            if ((evSurfaceDefinition(context, { "face" : faces[0] }) is Cone) ||
                (evSurfaceDefinition(context, { "face" : faces[1] }) is Cone))
            {
                throw regenError(ErrorStringEnum.TL_CONE_NEEDS_TANGENT_POLYLINES, ["profile1", "profile2"]);
            }
            jointAttribute = createBendAttribute(context, id, edge, toAttributeId(id + count), bendRadius, false, false);
        }
        //there may already be a joint attribute from before the merge, if the newly created edge is joined to an
        //existing joint edge.
        const possibleJointAttribute = getAttributes(context, { "entities" : edge,
                    "attributePattern" : asSMAttribute({ "objectType" : SMObjectType.JOINT }) });
        if (jointAttribute != undefined && possibleJointAttribute == [])
        {
            setAttribute(context, {
                        "entities" : edge,
                        "attribute" : jointAttribute
                    });
            annotatedEdges = append(annotatedEdges, edge);
            count += 1;
        }
    }
    return annotatedEdges;
}

function findTwoSidedEdgeFromMatches(context is Context, matches is array, twoSidedEdges is Query)
{
    for (var match in matches)
    {
        // If there is a rip connection, this code will not have been called, so skip all matches that came from user connections.
        if (match.hideMatchFromUI)
        {
            continue;
        }
        const midPoint = (match.match[0].position + match.match[1].position) / 2;
        var correspondingTwoSidedEdge = qContainsPoint(twoSidedEdges, midPoint);
        // Skip merged out matches.
        if (!isQueryEmpty(context, correspondingTwoSidedEdge))
        {
            return [correspondingTwoSidedEdge];
        }
    }
    return [];
}

function getNewTwoSidedEdgesAndRipEdges(context is Context, id is Id, isClosed is boolean, ripPositions is array, matches is array, definitionBodiesEdges is array) returns map
{
    const twoSidedEdges = qCreatedBy(id + "loft", EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.TWO_SIDED);
    const loftTwoSidedEdges = twoSidedEdges->qSubtraction(qUnion(definitionBodiesEdges));
    var ripEdges = [];
    if (!isQueryEmpty(context, loftTwoSidedEdges))
    {
        if (ripPositions != [])
        {
            for (var position in ripPositions)
            {
                ripEdges = ripEdges->append(qContainsPoint(loftTwoSidedEdges, position));
            }
        }
        else
        {
            if (isClosed)
            {
                ripEdges = findTwoSidedEdgeFromMatches(context, matches, loftTwoSidedEdges);
                if (ripEdges == [])
                {
                    ripEdges = [loftTwoSidedEdges->qNthElement(0)];
                }
            }
        }
    }

    return { "twoSidedEdges" : twoSidedEdges,
            "ripEdges" : qUnion(ripEdges) };
}

function getTrackingQueriesForOneSidedDefinitionEdgesPerBodyBeingMerged(context is Context, booleanScope is Query) returns array
{
    var definitionBodiesEdges = [];
    for (var body in evaluateQuery(context, booleanScope))
    {
        const definitionFaces = qUnion(getSMDefinitionEntities(context, body->qOwnedByBody(EntityType.FACE)))->qEntityFilter(EntityType.FACE);
        const edgesToTrack = definitionFaces->qAdjacent(AdjacencyType.EDGE, EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.ONE_SIDED);
        definitionBodiesEdges = definitionBodiesEdges->append(startTracking(context, edgesToTrack));
    }
    return definitionBodiesEdges;
}

function getRipEdgesNeededForUnfold(context is Context, twoSidedEdgesCreatedByLoft is Query, definitionBodiesEdges is array) returns Query
{
    var ripEdges = [];
    for (var definitionBodyEdges in definitionBodiesEdges)
    {
        const definitionBodyMergedEdges = evaluateQuery(context, qIntersection(twoSidedEdgesCreatedByLoft, definitionBodyEdges));
        const nDefinitionBodyMergedEdges = size(definitionBodyMergedEdges);
        var bendFound = false;
        for (var i = 0; i < nDefinitionBodyMergedEdges && !bendFound; i = i + 1)
        {
            const edge = definitionBodyMergedEdges[i];
            if (size(getSmObjectTypeAttributes(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE), SMObjectType.WALL)) == 2)
            {
                bendFound = true;
                if (nDefinitionBodyMergedEdges > i + 1)
                {
                    // Only one of the merged edges of body can be a bend, others need to be rips, in order for the unfold to succeed
                    ripEdges = ripEdges->concatenateArrays(subArray(definitionBodyMergedEdges, i + 1));
                }
            }
            else
            {
                ripEdges = ripEdges->append(edge);
            }
        }
    }
    return qUnion(ripEdges);
}

function annotateSheetBody(context, id, definition, isClosed is boolean, ripPositions is array, matches is array)
{
    const sheetMetalModelAttributeArgs = getSheetMetalModelAttributeArgsFromDialogParams(context, definition);
    const modelAttribute = getSheetMetalModelAttributeFromParams(context, id, sheetMetalModelAttributeArgs);

    for (var body in evaluateQuery(context, qCreatedBy(id + "loft", EntityType.BODY)))
    {
        setAttribute(context, {
                    "entities" : body,
                    "attribute" : modelAttribute
                });
    }

    const newEdges = getNewTwoSidedEdgesAndRipEdges(context, id, isClosed, ripPositions, matches, []);
    const annotatedFaces = annotateNewSheetFaces(context, id);
    annotateNewSheetEdges(context, id, newEdges, definition.radius, size(annotatedFaces));
}

function collectMatchItems(connection is map, matchItem is map) returns map
{
    if (matchItem.vertex != undefined)
    {
        connection.connectionEntities = connection.connectionEntities->append(matchItem.vertex);
    }
    else
    {
        connection.connectionEntities = connection.connectionEntities->append(matchItem.edge);
        connection.connectionEdgeQueries = connection.connectionEdgeQueries->append(matchItem.edge);
        connection.connectionEdgeParameters = connection.connectionEdgeParameters->append(clamp(matchItem.parameter, 0, 1));
    }

    return connection;
}

function createConnectionFromMatch(match is map) returns map
{
    var collectedMatchItems = { "connectionEntities" : [], "connectionEdgeQueries" : [], "connectionEdgeParameters" : [] };
    collectedMatchItems = collectedMatchItems->collectMatchItems(match.match[0]);
    collectedMatchItems = collectedMatchItems->collectMatchItems(match.match[1]);
    return {
            "isRip" : false,
            "connectionEntities" : qUnion(collectedMatchItems.connectionEntities),
            "connectionEdgeQueries" : qUnion(collectedMatchItems.connectionEdgeQueries),
            "connectionEdgeParameters" : collectedMatchItems.connectionEdgeParameters,
            "connectionEdges" : collectedMatchItems.connectionEdgeQueries,
            "removeRedundantEdge" : match.removeRedundantEdge
        };
}

/**
 * @internal
 * Manipulator function for loft feature.
 */
export function tessLoftManipulator(context is Context, definition is map, newManipulators is map) returns map
{
    var manipulatorKeys = keys(newManipulators);
    for (var ii = 0; ii < size(manipulatorKeys); ii += 1)
    {
        var manipulator = newManipulators[manipulatorKeys[ii]];
        if (manipulatorKeys[ii] == FLIP_DIRECTION_UP_MANIPULATOR_NAME)
        {
            definition.flipDirectionUp = manipulator.flipped;
        }
        else
        {
            definition = loftLinearManipulator(context, definition, manipulatorKeys[ii], manipulator, 0, 1);
        }
    }
    return definition;
}

/**
 * @internal
 * Editing logic function for loft feature.
 */
export function tessLoftEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (definition.matchConnections)
    {
        definition.connections = updateConnections(context, definition.connections);
        const iConnection = connectionIndexToAutoComplete(context, oldDefinition, definition);
        if (iConnection != -1)
        {
            definition.connections[iConnection] = autoCompleteConnection(context, definition, iConnection);
        }
    }
    if (isCreating && !specifiedParameters.defaultCornerStyle)
    {
        definition.defaultCornerStyle = SMCornerStrategyType.RECTANGLE;
        if (!specifiedParameters.defaultCornerReliefScale)
        {
            definition.defaultCornerReliefScale = 1.2;
        }
    }
    // Populate the booleanScope QLV based on the profiles (de)selected
    if (definition.surfaceOperationType == NewSurfaceOperationType.ADD &&
        specifiedParameters.booleanScope != true &&
        (definition.profile1 != oldDefinition.profile1 || definition.profile2 != oldDefinition.profile2))
    {
        const profileVerticesQ = qUnion([qEntityFilter(definition.profile1, EntityType.EDGE)->qOwnerBody(),
                        qEntityFilter(definition.profile1, EntityType.BODY),
                        qEntityFilter(definition.profile2, EntityType.EDGE)->qOwnerBody(),
                        qEntityFilter(definition.profile2, EntityType.BODY)])->qOwnedByBody(EntityType.VERTEX);
        const smSolidVerticesQ = qAllSolidBodies()->qActiveSheetMetalFilter(ActiveSheetMetal.YES)->qSubtraction(hiddenBodies)->qOwnedByBody(EntityType.VERTEX);
        var commonVertices = [];
        for (var vertex in evaluateQuery(context, profileVerticesQ))
        {
            var smSolidVertices = evaluateQuery(context, qContainsPoint(smSolidVerticesQ, evVertexPoint(context, { "vertex" : vertex })));
            // Append only if there is one vertex. If there is any ambiguity - let the user select manually.
            if (size(smSolidVertices) == 1)
            {
                commonVertices = append(commonVertices, smSolidVertices[0]);
            }
        }
        definition.booleanScope = qUnion(qUnion(commonVertices)->qOwnerBody(),
                    definition.profile1->qOwnerBody()->qBodyType(BodyType.SOLID),
                    definition.profile2->qOwnerBody()->qBodyType(BodyType.SOLID))->qActiveSheetMetalFilter(ActiveSheetMetal.YES)->qModifiableEntityFilter();
    }
    return definition;
}

function connectionIndexToAutoComplete(context is Context, oldDefinition is map, definition is map) returns number
{
    const nConnections = size(definition.connections);
    const nOldConnections = size(oldDefinition.connections);

    if (nConnections > nOldConnections)
    {
        // brand new connection has been started
        return (nConnections - 1);
    }

    if (nConnections < nOldConnections)
    {
        // connection has been deleted
        return -1;
    }

    // nConnections == nOldConnections
    for (var iConnection = 0; iConnection < nConnections; iConnection += 1)
    {
        if (evaluateQueryCount(context, definition.connections[iConnection].connectionEntities) == 1 &&
            evaluateQueryCount(context, oldDefinition.connections[iConnection].connectionEntities) == 0)
        {
            // first connection entity has been selected for an existing connection
            return iConnection;
        }
    }

    return -1;
}

/**
 * For the purposes of completing connections. It's desirable to get the sketch edge that formed the imprint because it's consistent with what users can pick from the UI.
 * Otherwise, selection highlights don't work, and the entity cannot be deselected from the UI.
 */
function getProfileEdgesForConnectionCompletion(profile is Query) returns Query
{
    return qUnion([qEntityFilter(profile, EntityType.EDGE),
                qEntityFilter(profile, EntityType.FACE)->qSketchFilter(SketchObject.NO)->qAdjacent(AdjacencyType.EDGE, EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.ONE_SIDED),
                qEntityFilter(profile, EntityType.FACE)->qSketchFilter(SketchObject.YES)->qAdjacent(AdjacencyType.EDGE, EntityType.EDGE)->qDependency()->qBodyType(BodyType.WIRE)->qEntityFilter(EntityType.EDGE),
                qEntityFilter(profile, EntityType.BODY)->qBodyType(BodyType.WIRE)->qOwnedByBody(EntityType.EDGE),
                qEntityFilter(profile, EntityType.BODY)->qBodyType(BodyType.SHEET)->qOwnedByBody(EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.ONE_SIDED)]);
}

function autoCompleteConnection(context is Context, definition is map, iConnection is number) returns map
{
    if (definition.connections == [])
    {
        return {};
    }
    var connection = definition.connections[iConnection];
    var connectionEntities = evaluateQuery(context, connection.connectionEntities);
    if (size(connectionEntities) == 1)
    {
        var profileToFindClosestPointOn = undefined;
        var connectionPosition = undefined;
        if (size(connection.connectionEdgeParameters) == 1)
        {
            // Point on edge
            if (isIn(connectionEntities[0], evaluateQuery(context, getProfileEdgesForConnectionCompletion(definition.profile1))))
            {
                profileToFindClosestPointOn = definition.profile2;
            }
            else if (isIn(connectionEntities[0], evaluateQuery(context, getProfileEdgesForConnectionCompletion(definition.profile2))))
            {
                profileToFindClosestPointOn = definition.profile1;
            }
            if (profileToFindClosestPointOn != undefined)
            {
                connectionPosition = evEdgeTangentLine(context, { "edge" : connectionEntities[0],
                                "parameter" : connection.connectionEdgeParameters[0],
                                "arcLengthParameterization" : false }).origin;
            }
        }
        else
        {
            // Vertex including edge point(midpoint)
            if (evDistance(context, { "side0" : connectionEntities[0], "side1" : definition.profile1,
                                "arcLengthParameterization" : false }).distance < TOLERANCE.zeroLength * meter)
            {
                profileToFindClosestPointOn = definition.profile2;
            }
            else if (evDistance(context, { "side0" : connectionEntities[0], "side1" : definition.profile2,
                                "arcLengthParameterization" : false }).distance < TOLERANCE.zeroLength * meter)
            {
                profileToFindClosestPointOn = definition.profile1;
            }
            if (profileToFindClosestPointOn != undefined)
            {
                connectionPosition = evVertexPoint(context, { "vertex" : connectionEntities[0] });
            }
        }
        if (connectionPosition != undefined)
        {
            if (!isQueryEmpty(context, profileToFindClosestPointOn->qEntityFilter(EntityType.FACE)->qSketchFilter(SketchObject.YES)) &&
                isQueryEmpty(context, getProfileEdgesForConnectionCompletion(profileToFindClosestPointOn->qEntityFilter(EntityType.FACE)->qSketchFilter(SketchObject.YES))))
            {
                // Cannot auto-complete a connection onto a profile that has a patterned sketch face
                return connection;
            }
            var newConnectionEntity = undefined;
            if (isQueryEmpty(context, getProfileEdgesForConnectionCompletion(profileToFindClosestPointOn)))
            {
                // profileToFindClosestPointOn is a vertex
                newConnectionEntity = profileToFindClosestPointOn;
            }
            else
            {
                // Take the distance from the other side as it would be input into the tessellated loft operation to avoid finding minimum distance
                // to overbuilt sketch edges corresponding to imprints.
                const distanceFromOtherProfile = evDistance(context, { "side0" : connectionPosition,
                            "side1" : getProfileEdgesAndVertices(profileToFindClosestPointOn),
                            "arcLengthParameterization" : false });
                // The resulting position needs to be projected back onto the user visible entities to get the correct parameter.
                const distanceToUserEntities = evDistance(context, {
                            "side0" : distanceFromOtherProfile.sides[1].point,
                            "side1" : getProfileEdgesForConnectionCompletion(profileToFindClosestPointOn),
                            "arcLengthParameterization" : false
                        });
                const edgesOnOtherProfile = evaluateQuery(context, getProfileEdgesForConnectionCompletion(profileToFindClosestPointOn));
                newConnectionEntity = edgesOnOtherProfile[distanceToUserEntities.sides[1].index];
                connection.connectionEdgeQueries = qUnion(evaluateQuery(context, connection.connectionEdgeQueries)->append(newConnectionEntity));
                connection.connectionEdgeParameters = connection.connectionEdgeParameters->append(clamp(distanceToUserEntities.sides[1].parameter, 0, 1));
            }
            connection.connectionEntities = qUnion(connectionEntities->append(newConnectionEntity));
        }
    }
    return connection;
}

function addConnectionManipulators(context is Context, id is Id, definition is map) returns array
{
    if (!definition.matchConnections)
    {
        return [];
    }
    var ripMidsOut = [];
    for (var ii = 0; ii < size(definition.connections); ii += 1)
    {
        var connection = definition.connections[ii];
        var positions = addLinearManipulatorForConnection(context, id, connection, ii);

        for (var vertex in evaluateQuery(context, qEntityFilter(connection.connectionEntities, EntityType.VERTEX)))
        {
            var position = evVertexPoint(context, { "vertex" : vertex });
            positions = positions->append(position);
        }

        if (positions->size() == 2)
        {
            if (connection.isRip)
            {
                ripMidsOut = ripMidsOut->append((positions[0] + positions[1]) / 2);
                addAuxiliaryLine(context, id, positions[0], positions[1], DebugColor.CYAN);
            }
            else
            {
                addAuxiliaryLine(context, id, positions[0], positions[1], DebugColor.MAGENTA);
            }
        }
    }
    return ripMidsOut;
}

function getRipPositions(context is Context, definition is map) returns array
{
    if (!definition.matchConnections)
    {
        return [];
    }
    var ripMidsOut = [];
    for (var ii = 0; ii < size(definition.connections); ii += 1)
    {
        var connection = definition.connections[ii];
        if (connection.isRip)
        {
            var positions = [];
            var edges = evaluateQuery(context, connection.connectionEdgeQueries);
            for (var jj = 0; jj < size(edges); jj += 1)
            {
                var line = evEdgeTangentLine(context, { "edge" : edges[jj], "parameter" : connection.connectionEdgeParameters[jj],
                        "arcLengthParameterization" : false });
                positions = positions->append(line.origin);
            }
            for (var vertex in evaluateQuery(context, qEntityFilter(connection.connectionEntities, EntityType.VERTEX)))
            {
                var position = evVertexPoint(context, { "vertex" : vertex });
                positions = positions->append(position);
            }
            if (positions->size() == 2)
            {
                ripMidsOut = ripMidsOut->append((positions[0] + positions[1]) / 2);
            }
        }
    }
    return ripMidsOut;
}

function transformProfileIfNeeded(context is Context, topLevelId is Id, definition is map, profileIndex is number) returns map
{
    const profileQuery = definition.profileSubqueries[profileIndex];
    if (isQueryEmpty(context, profileQuery->qEntityFilter(EntityType.EDGE)))
    {
        // Profile is a vertex
        const smDefinitionVerticesQ = definition.booleanSurfaceScope->qOwnedByBody(EntityType.VERTEX);
        const smSolidVerticesQ = definition.booleanScope->qOwnedByBody(EntityType.VERTEX);
        const vertexCurrentPoint = evVertexPoint(context, { "vertex" : profileQuery });
        if (isQueryEmpty(context, qContainsPoint(smDefinitionVerticesQ, vertexCurrentPoint)))
        {
            var smSolidVertices = evaluateQuery(context, qContainsPoint(smSolidVerticesQ, vertexCurrentPoint));
            if (size(smSolidVertices) == 1)
            {
                var smDefinitionVertices = getSMDefinitionEntities(context, smSolidVertices[0], EntityType.VERTEX);
                if (size(smDefinitionVertices) == 1)
                {
                    definition = transformConnectionsOnProfile(context, definition, profileQuery, identityTransform(), smDefinitionVertices[0], false);
                    addAuxiliaryEntities(context, topLevelId, smDefinitionVertices[0], DebugColor.GREEN);
                    reportFeatureInfo(context, topLevelId, ErrorStringEnum.TL_PROFILES_TRANSFORMED);
                    definition.profileSubqueries[profileIndex] = smDefinitionVertices[0];
                }
            }
        }
        return definition;
    }

    const profileEdgeSelections = profileQuery->qEntityFilter(EntityType.EDGE);
    var allDefinitionEdges = [];
    var foundNonDefEdge = false;
    for (var edge in evaluateQuery(context, profileEdgeSelections))
    {
        var smDefnVertex = getSMDefinitionEntities(context, edge, EntityType.VERTEX);
        var smDefnEdge = getSMDefinitionEntities(context, edge, EntityType.EDGE);
        if (smDefnEdge == [] && smDefnVertex == [])
        {
            // found edge that does not have a defn entity, possibly need ends transform
            foundNonDefEdge = true;
            break;
        }
        if (smDefnEdge != [])
            allDefinitionEdges = concatenateArrays([allDefinitionEdges, smDefnEdge]);
    }

    const id = topLevelId + "transform" + toString(profileIndex);
    callSubfeatureAndProcessStatus(id, compositeCurve, context, id + "composite", { "edges" : profileEdgeSelections });
    const curve = qCreatedBy(id + "composite", EntityType.BODY);
    const curveVertices = evaluateQuery(context, curve->qOwnedByBody(EntityType.VERTEX));
    if (size(curveVertices) < 2)
    {
        // Ring edge
        return definition;
    }
    var endVertices = [];
    for (var vertex in curveVertices)
    {
        if (evaluateQueryCount(context, vertex->qAdjacent(AdjacencyType.VERTEX, EntityType.EDGE)) == 1)
        {
            endVertices = append(endVertices, vertex);
        }
    }

    if (!foundNonDefEdge && allDefinitionEdges != [])
    {
        //all edges have a definition entity, use definition surface edges to create loft
        const id = topLevelId + "transform" + toString(profileIndex) + "def";
        callSubfeatureAndProcessStatus(id, compositeCurve, context, id + "composite", { "edges" : qUnion(allDefinitionEdges) });
        const defCurve = qCreatedBy(id + "composite", EntityType.BODY);

        if (size(endVertices) == 2) // open profile, trim ends to match profile edges if needed
        {
            trimCurveEndsIfNeeded(context, id, defCurve, endVertices);
        }

        definition = transformConnectionsOnProfile(context, definition, profileQuery, identityTransform(), defCurve, true);
        definition.profileSubqueries[profileIndex] = defCurve->qOwnedByBody(EntityType.EDGE);
        //dont need to display info or auxiliary graphics as this should be invisible to the user
        return definition;
    }

    if (size(endVertices) != 2)
    {
        return definition;
    }

    const smDefinitionVerticesQ = definition.booleanSurfaceScope->qOwnedByBody(EntityType.VERTEX);
    const smSolidVerticesQ = definition.booleanScope->qOwnedByBody(EntityType.VERTEX);
    var needTransform = false;

    const startVertexCurrentPoint = evVertexPoint(context, { "vertex" : endVertices[0] });
    var startVertexTargetPoint = startVertexCurrentPoint;
    if (isQueryEmpty(context, qContainsPoint(smDefinitionVerticesQ, startVertexCurrentPoint)))
    {
        var smSolidVertices = evaluateQuery(context, qContainsPoint(smSolidVerticesQ, startVertexCurrentPoint));
        if (size(smSolidVertices) == 1)
        {
            var smDefinitionVertices = getSMDefinitionEntities(context, smSolidVertices[0], EntityType.VERTEX);
            if (size(smDefinitionVertices) == 1)
            {
                startVertexTargetPoint = evVertexPoint(context, { "vertex" : smDefinitionVertices[0] });
                needTransform = true;
            }
        }
    }

    const endVertexCurrentPoint = evVertexPoint(context, { "vertex" : endVertices[1] });
    var endVertexTargetPoint = endVertexCurrentPoint;
    if (isQueryEmpty(context, qContainsPoint(smDefinitionVerticesQ, endVertexCurrentPoint)))
    {
        var smSolidVertices = evaluateQuery(context, qContainsPoint(smSolidVerticesQ, endVertexCurrentPoint));
        if (size(smSolidVertices) == 1)
        {
            var smDefinitionVertices = getSMDefinitionEntities(context, smSolidVertices[0], EntityType.VERTEX);
            if (size(smDefinitionVertices) == 1)
            {
                endVertexTargetPoint = evVertexPoint(context, { "vertex" : smDefinitionVertices[0] });
                needTransform = true;
            }
        }
    }

    if (tolerantEquals(startVertexTargetPoint, endVertexTargetPoint) || //collapsing onto a point
        tolerantEquals(startVertexCurrentPoint, endVertexCurrentPoint))
    {
        return definition;
    }

    if (needTransform)
    {
        var transform = transform(line(startVertexCurrentPoint, (endVertexCurrentPoint - startVertexCurrentPoint)),
        line(startVertexTargetPoint, (endVertexTargetPoint - startVertexTargetPoint)));
        const endVertexPointAfterTranslateRotate = transform * endVertexCurrentPoint;
        if (endVertexPointAfterTranslateRotate != endVertexTargetPoint)
        {
            const scale = norm(endVertexTargetPoint - startVertexTargetPoint) /
                norm(endVertexPointAfterTranslateRotate - startVertexTargetPoint);
            transform = scaleUniformly(scale, startVertexTargetPoint) * transform;
        }
        const patternId = id + "pattern" + toString(profileIndex);
        opPattern(context, patternId, {
                    "entities" : curve,
                    "transforms" : [transform],
                    "instanceNames" : ["transformedCurve"]
                });
        definition = transformConnectionsOnProfile(context, definition, profileQuery, transform, qCreatedBy(patternId, EntityType.BODY), false);
        addAuxiliaryEntities(context, topLevelId, qCreatedBy(patternId), DebugColor.GREEN);
        reportFeatureInfo(context, topLevelId, ErrorStringEnum.TL_PROFILES_TRANSFORMED);
        definition.profileSubqueries[profileIndex] = qCreatedBy(patternId, EntityType.EDGE);
        return definition;
    }

    return definition;
}

function trimCurveEndsIfNeeded(context is Context, id is Id, defCurve is Query, endVertices is array)
{
    var defCurveEnds = [];
    for (var vertex in evaluateQuery(context, defCurve->qOwnedByBody(EntityType.VERTEX)))
    {
        if (evaluateQueryCount(context, vertex->qAdjacent(AdjacencyType.VERTEX, EntityType.EDGE)) == 1)
        {
            defCurveEnds = append(defCurveEnds, evVertexPoint(context, { "vertex" : vertex }));
        }
    }

    if (size(defCurveEnds) != 2) //closed definition curve
        return;

    var pointsToMove = [];
    var distanceResult = evDistance(context, {
            "side0" : endVertices[0],
            "side1" : defCurve,
            "arcLengthParameterization" : false
        });
    if (!tolerantEquals(distanceResult.sides[1].point, defCurveEnds[0]) &&
        !tolerantEquals(distanceResult.sides[1].point, defCurveEnds[1]))
        pointsToMove = append(pointsToMove, distanceResult.sides[1].point);

    distanceResult = evDistance(context, {
                "side0" : endVertices[1],
                "side1" : defCurve,
                "arcLengthParameterization" : false
            });
    if (!tolerantEquals(distanceResult.sides[1].point, defCurveEnds[0]) &&
        !tolerantEquals(distanceResult.sides[1].point, defCurveEnds[1]))
        pointsToMove = append(pointsToMove, distanceResult.sides[1].point);

    var helpPoint;
    const nrEdges = evaluateQueryCount(context, qOwnedByBody(defCurve, EntityType.EDGE));
    if (nrEdges > 1)
    {
        // pick the other endpoint of the edge that has the endVertex
        helpPoint = evVertexPoint(context, {
                    "vertex" : qSubtraction(endVertices[0]->qAdjacent(AdjacencyType.VERTEX, EntityType.EDGE)->qAdjacent(AdjacencyType.VERTEX, EntityType.VERTEX), endVertices[0])
                });
    }
    else
    {
        //single edge, use midpoint
        helpPoint = evEdgeTangentLine(context, {
                        "edge" : endVertices[0]->qAdjacent(AdjacencyType.VERTEX, EntityType.EDGE),
                        "parameter" : .5
                    }).origin;
    }
    //opMoveCurveBoundary doesnt require helpPoint to be on the curve, but it's safer to put it there
    distanceResult = evDistance(context, {
                "side0" : helpPoint,
                "side1" : defCurve,
                "arcLengthParameterization" : false
            });
    helpPoint = distanceResult.sides[1].point;

    if (pointsToMove != [])
    {
        const moveBoundaryDef = {
                "wires" : defCurve,
                "moveBoundaryType" : MoveCurveBoundaryType.TRIM,
                "trimToPoints" : pointsToMove,
                "helpPointPosition" : helpPoint
            };
        opMoveCurveBoundary(context, id + "moveCurveBoundary", moveBoundaryDef);
    }
}

function transformConnectionsOnProfile(context is Context, definition is map, originalProfile is Query, transform is Transform, transformedProfile is Query, useDefinitionSurface is boolean) returns map
{
    if (!definition.matchConnections || definition.connections == [])
    {
        return definition;
    }
    const originalProfileEdges = evaluateQuery(context, originalProfile->qEntityFilter(EntityType.EDGE));
    const transformedProfileEdgesQ = qOwnedByBody(transformedProfile, EntityType.EDGE);
    const transformedProfileEdges = evaluateQuery(context, transformedProfileEdgesQ);
    const profileIsAVertex = (transformedProfileEdges == []) ? true : false;
    for (var iConnection = 0; iConnection < size(definition.connections); iConnection += 1)
    {
        var connectionEntities = evaluateQuery(context, definition.connections[iConnection].connectionEntities);
        var connectionEdges = evaluateQuery(context, definition.connections[iConnection].connectionEdgeQueries);
        for (var iConnectionEntity = 0; iConnectionEntity < size(connectionEntities); iConnectionEntity += 1)
        {
            const connectionEntity = connectionEntities[iConnectionEntity];
            var connectionPositionOnOriginalProfile = undefined;
            const iConnectionEdge = indexOf(connectionEdges, connectionEntity);
            if (iConnectionEdge != -1)
            {
                // Point on edge
                if (isIn(connectionEdges[iConnectionEdge], originalProfileEdges))
                {
                    connectionPositionOnOriginalProfile = evEdgeTangentLine(context, { "edge" : connectionEdges[iConnectionEdge],
                                    "parameter" : definition.connections[iConnection].connectionEdgeParameters[iConnectionEdge],
                                    "arcLengthParameterization" : false }).origin;
                }
            }
            else
            {
                // Vertex including edge point(midpoint)
                const distanceFromOriginalProfile = evDistance(context, { "side0" : connectionEntity, "side1" : originalProfile,
                                "arcLengthParameterization" : false }).distance;
                if (distanceFromOriginalProfile < TOLERANCE.zeroLength * meter)
                {
                    connectionPositionOnOriginalProfile = evVertexPoint(context, { "vertex" : connectionEntity });
                }
            }
            if (connectionPositionOnOriginalProfile != undefined)
            {
                // This connection entity is on this original profile.
                // Replace it with a corresponding entity on the transformed profile.
                if (profileIsAVertex)
                {
                    connectionEntities[iConnectionEntity] = transformedProfile;
                }
                else
                {
                    const distanceFromTransformedProfile = evDistance(context, { "side0" : useDefinitionSurface ? connectionPositionOnOriginalProfile
                                : transform * connectionPositionOnOriginalProfile,
                                "side1" : transformedProfileEdgesQ,
                                "arcLengthParameterization" : false });
                    // If useDefinitionSurface, use the closest point on the (trimmed) defintion edges.
                    // If not useDefinitionSurface, use the transformed point which should be on the transformed profile.
                    if (useDefinitionSurface || distanceFromTransformedProfile.distance < TOLERANCE.zeroLength * meter)
                    {
                        connectionEntities[iConnectionEntity] = transformedProfileEdges[distanceFromTransformedProfile.sides[1].index];
                        if (iConnectionEdge != -1)
                        {
                            connectionEdges[iConnectionEdge] = connectionEntities[iConnectionEntity];
                            definition.connections[iConnection].connectionEdgeParameters[iConnectionEdge] = clamp(distanceFromTransformedProfile.sides[1].parameter, 0, 1);
                        }
                        else
                        {
                            connectionEdges = connectionEdges->append(connectionEntities[iConnectionEntity]);
                            definition.connections[iConnection].connectionEdgeParameters =
                                definition.connections[iConnection].connectionEdgeParameters->append(clamp(distanceFromTransformedProfile.sides[1].parameter, 0, 1));
                        }
                    }
                }
            }
        }
        definition.connections[iConnection].connectionEntities = qUnion(connectionEntities);
        definition.connections[iConnection].connectionEdgeQueries = qUnion(connectionEdges);
    }
    return definition;
}

function getOrientationCheckData(context is Context, inputBodies is Query, definitionBodiesToCheck is Query) returns map
{
    var out = {};
    for (var body in evaluateQuery(context, inputBodies))
    {
        const definitionQ = qUnion(getSMDefinitionEntities(context, body));
        if (!isQueryEmpty(context, qIntersection(definitionBodiesToCheck, definitionQ)))
        {
            const faceQ = definitionQ->qOwnedByBody(EntityType.FACE)->qNthElement(0);
            const midTangent = try silent(evFaceTangentPlane(context, {
                    "face" : faceQ,
                    "parameter" : vector(0.5, 0.5)
            }));
            if (midTangent != undefined)
            {
                out[body] = {"face" : makeRobustQuery(context, faceQ), "normal" : midTangent.normal};
            }
        }
    }
    return out;
}

function checkOrientations(context is Context, featureId is Id, orientationCheckData is map)
{
    var bodiesWillChange = [];
    for (var body, checkData in orientationCheckData)
    {
         const midTangent = try silent(evFaceTangentPlane(context, {
                    "face" : checkData.face,
                    "parameter" : vector(0.5, 0.5)
            }));

        // normally this should be either 1 or -1 compare to -0.9 to avoid random mismatch
        if (midTangent != undefined && dot(midTangent.normal, checkData.normal) < -0.9)
        {
            bodiesWillChange = bodiesWillChange->append(body);
        }
    }
    if (bodiesWillChange != [])
    {
        addAuxiliaryEntities(context, featureId, qUnion(bodiesWillChange), DebugColor.RED);
        reportFeatureWarning(context, featureId, ErrorStringEnum.SHEET_METAL_LOFT_MERGE_SCOPE_SHIFT, ["booleanScope"]);
    }
}

