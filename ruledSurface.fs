FeatureScript 1930; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "1930.0");
export import(path : "onshape/std/ruledsurfacecornertype.gen.fs", version : "1930.0");
export import(path : "onshape/std/ruledsurfacetype.gen.fs", version : "1930.0");
export import(path : "onshape/std/tool.fs", version : "1930.0");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "1930.0");

import(path : "onshape/std/boolean.fs", version : "1930.0");
import(path : "onshape/std/containers.fs", version : "1930.0");
import(path : "onshape/std/error.fs", version : "1930.0");
import(path : "onshape/std/evaluate.fs", version : "1930.0");
import(path : "onshape/std/feature.fs", version : "1930.0");
import(path : "onshape/std/path.fs", version : "1930.0");
import(path : "onshape/std/string.fs", version : "1930.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1930.0");
import(path : "onshape/std/topologyUtils.fs", version : "1930.0");
import(path : "onshape/std/transform.fs", version : "1930.0");
import(path : "onshape/std/valueBounds.fs", version : "1930.0");
import(path : "onshape/std/vector.fs", version : "1930.0");

/**
 * The type of ruled surface to apply at a specific vertex.
 */
export enum VertexOverrideType
{
    annotation { "Name" : "Normal" }
    NORMAL,
    annotation { "Name" : "Tangent" }
    TANGENT,
    annotation { "Name" : "Aligned with direction" }
    ALIGNED_WITH_VECTOR,
    annotation { "Name" : "Up to vertex" }
    UP_TO_VERTEX
}

/**
 * The type of ruled surface to apply for the overall operation.
 */
export enum RuledSurfaceInterfaceType
{
    annotation { "Name" : "Normal" }
    NORMAL,
    annotation { "Name" : "Tangent" }
    TANGENT,
    annotation { "Name" : "Aligned with direction" }
    ALIGNED_WITH_VECTOR,
    annotation { "Name" : "Angle from direction" }
    ANGLE_FROM_VECTOR
}

/**
 * @internal
 * The bounds for the number of ruled lines shown per path.
 */
export const RULED_LINE_COUNT_BOUNDS =
{
    (unitless) : [1, 40, 100]
} as IntegerBoundSpec;


// Strings necessary for mapping undo stack entries.
const ANGLE_MANIPULATOR = "angleManipulator.";
const DISTANCE_MANIPULATOR = "depthManipulator.";

const OVERRIDE_MANIPULATOR = "O.";
const TOP_LEVEL_MANIPULATOR = "T.";

/**
 * Feature creating a ruled surface.
 */
annotation { "Feature Type Name" : "Ruled surface",
        "Manipulator Change Function" : "ruledSurfaceManipulator",
        "Editing Logic Function" : "ruledSurfaceEditingLogic" }
export const ruledSurface = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        surfaceOperationTypePredicate(definition);

        annotation { "Name" : "Edges for ruled surface path", "Filter" : EntityType.EDGE && ConstructionObject.NO }
        definition.edges is Query;

        annotation { "Name" : "Ruled surface type", "UIHint" : UIHint.SHOW_LABEL }
        definition.ruledType is RuledSurfaceInterfaceType;

        if (isAlignedType(definition))
        {
            annotation { "Name" : "Direction", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION, "MaxNumberOfPicks" : 1 }
            definition.axis is Query;
        }

        // Separate angle fields to provide two different defaults.
        if (definition.ruledType == RuledSurfaceInterfaceType.ANGLE_FROM_VECTOR)
        {
            annotation { "Name" : "Angle" }
            isAngle(definition.angleFromVector, ANGLE_360_90_DEFAULT_BOUNDS);
            annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION_CIRCULAR }
            definition.oppositeAngleFromVector is boolean;
        }
        else if (definition.ruledType == RuledSurfaceInterfaceType.TANGENT ||
                 definition.ruledType == RuledSurfaceInterfaceType.NORMAL)
        {
            annotation { "Name" : "Angle" }
            isAngle(definition.angle, ANGLE_360_ZERO_DEFAULT_BOUNDS);
            annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION_CIRCULAR }
            definition.oppositeAngle is boolean;
        }

        annotation { "Name" : "Distance" }
        isLength(definition.distance, LENGTH_BOUNDS);
        annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.oppositeDirection is boolean;

        if (!isAlignedType(definition))
        {
            annotation { "Name" : "Reference faces", "Filter" : EntityType.FACE && SketchObject.NO && ConstructionObject.NO }
            definition.referenceFaces is Query;
        }

        annotation { "Name" : "Vertex overrides", "Item name" : "arrayItem",
                    "Driven query" : "vertex", "Item label template" : "#vertex" }
        definition.vertexOverrides is array;
        for (var arrayItem in definition.vertexOverrides)
        {
            annotation { "Name" : "Vertex", "Filter" : EntityType.VERTEX && ConstructionObject.NO, "MaxNumberOfPicks" : 1, "UIHint" : UIHint.SHOW_LABEL }
            arrayItem.vertex is Query;

            annotation { "Name" : "Ruled surface type", "UIHint" : [UIHint.SHOW_LABEL, UIHint.MATCH_LAST_ARRAY_ITEM] }
            arrayItem.overrideType is VertexOverrideType;

            if (arrayItem.overrideType == VertexOverrideType.NORMAL || arrayItem.overrideType == VertexOverrideType.TANGENT)
            {
                annotation { "Name" : "Angle" }
                isAngle(arrayItem.angleOverride, ANGLE_360_ZERO_DEFAULT_BOUNDS);
                annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION_CIRCULAR }
                arrayItem.oppositeAngleOverride is boolean;
            }
            else if (arrayItem.overrideType == VertexOverrideType.ALIGNED_WITH_VECTOR)
            {
                annotation { "Name" : "Direction", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION, "MaxNumberOfPicks" : 1 }
                arrayItem.axisOverride is Query;
            }
            else if (arrayItem.overrideType == VertexOverrideType.UP_TO_VERTEX)
            {
                annotation { "Name" : "Up to vertex or mate connector", "Filter" : QueryFilterCompound.ALLOWS_VERTEX, "MaxNumberOfPicks" : 1 }
                arrayItem.boundaryOverride is Query;
            }

            if (arrayItem.overrideType != VertexOverrideType.UP_TO_VERTEX)
            {
                annotation { "Name" : "Distance" }
                isLength(arrayItem.distanceOverride, LENGTH_BOUNDS);
                annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                arrayItem.oppositeDirectionOverride is boolean;
            }
        }

        annotation { "Name" : "Use cubic interpolation" }
        definition.useCubicInterpolation is boolean;
        annotation { "Name" : "Show ruled lines" }
        definition.showRuledLines is boolean;

        if (definition.showRuledLines)
        {
            annotation { "Name" : "Count" }
            isInteger(definition.ruledLineCount, RULED_LINE_COUNT_BOUNDS);
        }

        annotation { "Name" : "Corner type", "UIHint" : UIHint.SHOW_LABEL }
        definition.cornerType is RuledSurfaceCornerType;

        surfaceJoinStepScopePredicate(definition);
    }
    {
        createRuledSurface(context, id, definition);

        var remainingTransform = getRemainderPatternTransform(context,
            { "references" : collectAllReferences(definition) });
        transformResultIfNecessary(context, id, remainingTransform);

        if (definition.surfaceOperationType == NewSurfaceOperationType.ADD)
        {
            const reconstructOp = function(innerId)
                {
                    createRuledSurface(context, innerId, definition);
                    transformResultIfNecessary(context, id, remainingTransform);
                };
            const makeSolid = false;
            joinSurfaceBodiesWithAutoMatching(context, id, definition, makeSolid, reconstructOp);
        }
    }, { oppositeDirection : false, oppositeAngle : false, angle : 0 * radian, surfaceOperationType : NewSurfaceOperationType.NEW,
            useCubicInterpolation : true, showRuledLines : false, ruledLineCount : 40, vertexOverrides: [], cornerType : RuledSurfaceCornerType.SPUN,
            angleFromVector: 0 * radian, oppositeAngleFromVector: false });

predicate isAlignedType(definition)
{
    definition.ruledType == RuledSurfaceInterfaceType.ALIGNED_WITH_VECTOR || definition.ruledType == RuledSurfaceInterfaceType.ANGLE_FROM_VECTOR;
}

function collectAllReferences(definition is map) returns Query
{
    var queryArray = [definition.edges];
    if (definition.ruledType != RuledSurfaceInterfaceType.ALIGNED_WITH_VECTOR)
    {
        queryArray = queryArray->append(definition.referenceFaces);
    }
    else
    {
        queryArray = queryArray->append(definition.axis);
    }
    for (var vertexOverride in definition.vertexOverrides)
    {
        queryArray = queryArray->append(vertexOverride.vertex);
        if (vertexOverride.overrideType == VertexOverrideType.UP_TO_VERTEX)
        {
            queryArray = queryArray->append(vertexOverride.boundaryOverride);
        }
        else if (vertexOverride.overrideType == VertexOverrideType.ALIGNED_WITH_VECTOR)
        {
            queryArray = queryArray->append(vertexOverride.axisOverride);
        }
    }

    return qUnion(queryArray);
}

function createRuledSurface(context is Context, id is Id, definition is map)
{
    verifyNonemptyQuery(context, definition, "edges", ErrorStringEnum.RULED_SURFACE_SELECT_EDGES);
    definition.edges = followWireEdgesToLaminarSource(context, definition.edges);
    const vertexOverrides = unpackVertexOverrides(context, definition);
    var referenceFaces = qNothing();
    if (!isAlignedType(definition) || !isAtVersionOrLater(context, FeatureScriptVersionNumber.V1714_RULED_SURFACE_IGNORE_UNUSED))
    {
        // Before V1714 reference faces were being tested for validity even when they were unused.
        referenceFaces = qUnion([definition.referenceFaces, qAdjacent(qEdgeTopologyFilter(definition.edges, EdgeTopology.ONE_SIDED), AdjacencyType.EDGE, EntityType.FACE)]);
    }

    if (isAlignedType(definition))
    {
        const direction = getAxisDirection(context, definition.axis, "axis", qNothing());

        const paths = constructPathsAndConvertError(context, definition.edges, referenceFaces);
        const edgeToOrientationMap = createEdgeToOrientationMap(context, paths);
        if (paths != undefined)
        {
            addTopLevelManipulator(context, id, definition, paths, edgeToOrientationMap);
        }
        addVertexOverrideManipulators(context, id, definition, qNothing(), edgeToOrientationMap);

        var angle;
        var ruledType;
        if (definition.ruledType == RuledSurfaceInterfaceType.ALIGNED_WITH_VECTOR)
        {
            ruledType = RuledSurfaceType.ALIGNED_WITH_VECTOR;
            angle = 0;
        }
        else
        {
            ruledType = RuledSurfaceType.ANGLE_FROM_VECTOR;
            angle = definition.oppositeAngleFromVector ? -definition.angleFromVector : definition.angleFromVector;
        }
        opRuledSurface(context, id, {
                    "cornerType" : definition.cornerType,
                    "edgeAlignmentType" : definition.edgeAlignmentType,
                    "path" : definition.edges,
                    "width" : definition.distance * (definition.oppositeDirection ? -1 : 1),
                    "ruledDirection" : direction,
                    "vertexOverrides" : vertexOverrides,
                    "useCubicInterpolation" : definition.useCubicInterpolation,
                    "showRuledLines" : definition.showRuledLines,
                    "ruledLineCount" : definition.ruledLineCount,
                    "ruledSurfaceType" : ruledType,
                    "angle" : angle
                });
    }
    else
    {
        var adjacentFaces = [];
        const paths = constructPathsAndConvertError(context, definition.edges, referenceFaces);
        const edgeToOrientationMap = createEdgeToOrientationMap(context, paths);
        if (paths != undefined)
        {
            addTopLevelManipulator(context, id, definition, paths, edgeToOrientationMap);
            for (var path in paths)
            {
                if (path.adjacentFaces != undefined)
                {
                    adjacentFaces = adjacentFaces->append(path.adjacentFaces);
                }
            }
        }
        addVertexOverrideManipulators(context, id, definition, qUnion(adjacentFaces), edgeToOrientationMap);

        const angle = adjustAngleForTangent(definition.angle, definition.ruledType == RuledSurfaceInterfaceType.TANGENT, definition.oppositeAngle);
        opRuledSurface(context, id, {
                    "cornerType" : definition.cornerType,
                    "edgeAlignmentType" : definition.edgeAlignmentType,
                    "path" : definition.edges,
                    "width" : definition.distance * (definition.oppositeDirection ? -1 : 1),
                    "referenceFaces" : qUnion(adjacentFaces),
                    "angle" : angle,
                    "vertexOverrides" : vertexOverrides,
                    "useCubicInterpolation" : definition.useCubicInterpolation,
                    "showRuledLines" : definition.showRuledLines,
                    "ruledLineCount" : definition.ruledLineCount,
                    "ruledSurfaceType" : RuledSurfaceType.ANGLE_FROM_FACE
                });
    }
}

function constructPathsAndConvertError(context is Context, edges is Query, referenceFaces is Query)
{
    try
    {
        return constructPaths(context, edges, { "adjacentSeedFaces" : referenceFaces });
    }
    catch (error)
    {
        const message = try(error.message as ErrorStringEnum);
        if (message == ErrorStringEnum.CONSTRUCT_PATH_NOT_MANIFOLD)
        {
            throw regenError(ErrorStringEnum.RULED_SURFACE_EDGES_NOT_MANIFOLD, ["edges"], edges);
        }
        else if (message == ErrorStringEnum.CONSTRUCT_PATH_EDGES_OVERLAP)
        {
            throw regenError(ErrorStringEnum.RULED_SURFACE_OVERLAPPING_SKETCH_EDGES, ["edges"], edges);
        }
        else if (message == ErrorStringEnum.CONSTRUCT_PATH_FACES_OPPOSITE_SIDES)
        {
            throw regenError(ErrorStringEnum.RULED_SURFACE_REFERENCE_FACES_BOTH_SIDES, ["referenceFaces"], referenceFaces);
        }
        else
        {
            throw ErrorStringEnum.RULED_SURFACE_FAILED;
        }
    }
}

function adjustAngleForTangent(angle is ValueWithUnits, isTangent is boolean, isOppositeAngle is boolean) returns ValueWithUnits
{
    return angle * (isOppositeAngle ? -1 : 1) + (isTangent ? 0 : -PI / 2) * radian;
}

function getAxisDirection(context is Context, axis is Query, parameterId is string, errorQuery is Query)
{
    const direction = extractDirection(context, axis);
    if (direction == undefined)
    {
        if (isQueryEmpty(context, errorQuery))
        {
            throw regenError(ErrorStringEnum.RULED_SURFACE_SELECT_DIRECTION, [parameterId]);
        }
        else
        {
            throw regenError(ErrorStringEnum.RULED_SURFACE_SELECT_DIRECTION, [parameterId], errorQuery);
        }
    }
    return direction;
}

function unpackVertexOverrides(context is Context, definition is map) returns array
{
    var vertexOverrides = [];
    for (var index, override in definition.vertexOverrides)
    {
        var unpacked = {};
        unpacked.vertex = override.vertex;

        if (definition.ruledType == RuledSurfaceInterfaceType.ALIGNED_WITH_VECTOR &&
            (override.overrideType == VertexOverrideType.NORMAL || override.overrideType == VertexOverrideType.TANGENT))
        {
            throw regenError(ErrorStringEnum.RULED_SURFACE_GLOBAL_NORMAL_OVERRIDE, [faultyArrayParameterId("vertexOverrides", index, "overrideType")]);
        }

        if (override.overrideType == VertexOverrideType.UP_TO_VERTEX)
        {
            if (isQueryEmpty(context, override.boundaryOverride))
            {
                throw regenError(ErrorStringEnum.RULED_SURFACE_SELECT_UP_TO_ENTITY, [faultyArrayParameterId("vertexOverrides", index, "boundaryOverride")]);
            }
            unpacked.upToEntity = override.boundaryOverride;
        }
        else if (override.overrideType == VertexOverrideType.TANGENT)
        {
            unpacked.width = override.oppositeDirectionOverride ? -override.distanceOverride : override.distanceOverride;
            unpacked.angle = adjustAngleForTangent(override.angleOverride, true, override.oppositeAngleOverride);
        }
        else if (override.overrideType == VertexOverrideType.NORMAL)
        {
            unpacked.width = override.oppositeDirectionOverride ? -override.distanceOverride : override.distanceOverride;
            unpacked.angle = adjustAngleForTangent(override.angleOverride, false, override.oppositeAngleOverride);
        }
        else
        {
            unpacked.width = override.oppositeDirectionOverride ? -override.distanceOverride : override.distanceOverride;
            unpacked.ruledDirection = getAxisDirection(context, override.axisOverride, faultyArrayParameterId("vertexOverrides", index, "axisOverride"), override.vertex);
        }
        vertexOverrides = vertexOverrides->append(unpacked);
    }
    return vertexOverrides;
}

function addTopLevelManipulator(context is Context, id is Id, definition is map, paths is array, edgeToOrientationMap is map)
{
    if (paths == [])
    {
        return;
    }

    var vertexQueryArray = [];
    for (var vertexOverride in definition.vertexOverrides)
    {
        vertexQueryArray = vertexQueryArray->append(vertexOverride.vertex);
    }
    // Negative index differentiates from override manipulators.
    const manipulatorIndex = -1;

    const firstPath = paths[0];
    const freeVertex = findFreeVertex(context, firstPath, qUnion(vertexQueryArray));
    if (freeVertex != undefined)
    {
        const edge = getOneAdjacentEdgeForVertex(context, qUnion(firstPath.edges), freeVertex);
        const manipulatorPosition = manipulatorPosition(context, edge, freeVertex);
        addTopLevelManipulatorAtPosition(context, id, definition, manipulatorPosition, firstPath, manipulatorIndex, edgeToOrientationMap);
    }
    else if (firstPath.edges->size() == 1 && firstPath.closed)
    {
        const edge = firstPath.edges[0];
        const manipulatorPosition = manipulatorPosition(context, edge, 0);
        addTopLevelManipulatorAtPosition(context, id, definition, manipulatorPosition, firstPath, manipulatorIndex, edgeToOrientationMap);
    }
}

function findFreeVertex(context is Context, path is Path, boundVertices is Query)
{
    if (!path.closed)
    {
        // Open path => return at an unbound end vertex only.
        const freeVertices = evaluateQuery(context, qSubtraction(getPathEndVertices(context, path), boundVertices));
        if (freeVertices != [])
        {
            return freeVertices[0];
        }
    }
    else
    {
        // Closed path, => if any vertices have overrides, then there are no free vertices
        const pathVertices = qAdjacent(qUnion(path.edges), AdjacencyType.VERTEX, EntityType.VERTEX);
        if (isQueryEmpty(context, qIntersection([boundVertices, pathVertices])))
        {
            const evaluatedPathVertices = evaluateQuery(context, pathVertices);
            if (evaluatedPathVertices != [])
            {
                return evaluatedPathVertices[0];
            }
        }
    }
    return undefined;
}

function getOneAdjacentEdgeForVertex(context is Context, edges is Query, vertex is Query) returns Query
{
    return qNthElement(qIntersection([qAdjacent(vertex, AdjacencyType.VERTEX, EntityType.EDGE), edges]), 0);
}

function addAngleFromManipulators(context is Context, id is Id, definition is map, manipulatorOptions is map)
{
    const adjacentEdge = manipulatorOptions.position.edge;
    var position = manipulatorOptions.position->getPosition();

    // Fail silently when making manipulators. Let the actual ruled surface code inform about what action needs to be taken.
    var directions = {};
    if (definition.ruledType == RuledSurfaceInterfaceType.ANGLE_FROM_VECTOR)
    {
        const direction = getAxisDirection(context, definition.axis, "axis", qNothing());
        directions = try silent(getRuledDirectionsReferenceDirection(context, adjacentEdge, direction, manipulatorOptions));
    }
    else
    {
        var adjacentFaces = [];
        if (manipulatorOptions.faces != undefined)
        {
            adjacentFaces = evaluateQuery(context, qIntersection([qAdjacent(adjacentEdge, AdjacencyType.EDGE, EntityType.FACE), manipulatorOptions.faces]));
        }

        if (adjacentFaces == [])
        {
            directions = try silent(getRuledDirectionsSketchEdge(context, adjacentEdge, manipulatorOptions));
        }
        else if (adjacentFaces->size() == 1)
        {
            directions = try silent(getRuledDirectionsFace(context, adjacentEdge, adjacentFaces[0], manipulatorOptions));
        }
    }

    if (directions == undefined)
    {
        return;
    }
    const distanceOffset = manipulatorOptions.oppositeDirection ? -manipulatorOptions.distance : manipulatorOptions.distance;
    addDistanceManipulator(context, id, manipulatorOptions.manipulatorBaseId, position, directions.ruledDirection, distanceOffset, manipulatorOptions.distanceManipulatorParameter);
    const angleOffset = manipulatorOptions.oppositeAngle ? -manipulatorOptions.angle : manipulatorOptions.angle;
    addAngularManipulator(context, id, manipulatorOptions.manipulatorBaseId, position, directions.baseDirection, distanceOffset, angleOffset, directions.tangent, manipulatorOptions.angleManipulatorParameter);
}

function addAlignedWithVectorManipulator(context is Context, id is Id, manipulatorId is string, position is ManipulatorPosition, direction is Query, distance is ValueWithUnits, oppositeDirection is boolean, parameterId)
{
    const manipulatorCoordinates = position->getPosition();
    var directionVector = extractDirection(context, direction);
    if (directionVector != undefined)
    {
        // Flipping the direction creates jumpiness around cursor position, but flipping the distance does not.
        const manipulatorOffset = oppositeDirection ? -distance : distance;
        addDistanceManipulator(context, id, manipulatorId, manipulatorCoordinates, directionVector, manipulatorOffset, parameterId);
    }
}

/**
 * A `ManipulatorPosition` represents a place where a ruled surface manipulator is anchored, either on a vertex, or on
 *      an edge.
 * @type {{
 *      @field vertex {Query
 *      @field parameter {number}
 *      @field edge {Query}
 * }}
 */
type ManipulatorPosition typecheck canBeManipulatorPosition;

predicate canBeManipulatorPosition(value)
{
    value.edge is Query;
    value.position is Vector;
    value.parameter is number;
}

function manipulatorPosition(context is Context, edge is Query, vertex is Query) returns ManipulatorPosition
{
    const position = evVertexPoint(context, {
                "vertex" : vertex
            });

    const parameter = evDistance(context, {
                    "side0" : edge,
                    "side1" : vertex
                }).sides[0].parameter;

    return {
                "edge" : edge,
                "parameter" : parameter,
                "position" : position
            } as ManipulatorPosition;
}

function manipulatorPosition(context is Context, edge is Query, parameter is number) returns ManipulatorPosition
{
    const position = evEdgeTangentLine(context, {
                    "edge" : edge,
                    "parameter" : parameter
                }).origin;

    return {
                "edge" : edge,
                "parameter" : parameter,
                "position" : position
            } as ManipulatorPosition;
}

function getPosition(value is ManipulatorPosition) returns Vector
{
    return value.position;
}

function getParameter(value is ManipulatorPosition) returns number
{
    return value.parameter;
}

function addTopLevelManipulatorAtPosition(context is Context, id is Id, definition is map, position is ManipulatorPosition, path is Path, manipulatorIndex is number, edgeToOrientationMap is map)
{
    const manipulatorBaseId = TOP_LEVEL_MANIPULATOR ~ manipulatorIndex;
    if (definition.ruledType == RuledSurfaceInterfaceType.ALIGNED_WITH_VECTOR)
    {
        const parameterId = "distance";
        addAlignedWithVectorManipulator(context, id, manipulatorBaseId, position, definition.axis, definition.distance, definition.oppositeDirection, parameterId);
    }
    else if (definition.ruledType == RuledSurfaceInterfaceType.ANGLE_FROM_VECTOR)
    {
        const manipulatorCoordinates = position->getPosition();
        var directionVector = extractDirection(context, definition.axis);
        if (directionVector != undefined)
        {
            // Flipping the direction creates jumpiness around cursor position, but flipping the distance does not.
            const manipulatorOffset = definition.oppositeDirection ? -definition.distance : definition.distance;
            const directions = getRuledDirectionsReferenceDirection(context, position.edge, directionVector, {
                        "edgeToOrientationMap" : edgeToOrientationMap,
                        "oppositeAngle" : definition.oppositeAngleFromVector,
                        "angle" : definition.angleFromVector,
                        "isTangent" : false,
                        "position" : position });
            const angleOffset = definition.oppositeAngleFromVector ? -definition.angleFromVector : definition.angleFromVector;
            const distanceParameter = "distance";
            const angleParameter = "angleFromVector";
            addDistanceManipulator(context, id, manipulatorBaseId, manipulatorCoordinates, directions.ruledDirection, manipulatorOffset, distanceParameter);
            addAngularManipulator(context, id, manipulatorBaseId, manipulatorCoordinates, directions.baseDirection, manipulatorOffset, angleOffset, directions.tangent, angleParameter);
        }
    }
    else
    {
        addAngleFromManipulators(context, id, definition, {
                    "position" : position,
                    "faces" : path.adjacentFaces,
                    "angle" : definition.angle,
                    "distance" : definition.distance,
                    "isTangent" : definition.ruledType == RuledSurfaceInterfaceType.TANGENT,
                    "oppositeDirection" : definition.oppositeDirection,
                    "oppositeAngle" : definition.oppositeAngle,
                    "manipulatorBaseId" : manipulatorBaseId,
                    "edgeToOrientationMap" : edgeToOrientationMap,
                    "distanceManipulatorParameter" : "distance",
                    "angleManipulatorParameter" : "angle"
                });
    }
}

function addVertexOverrideManipulators(context is Context, id is Id, definition is map, faces is Query, edgeToOrientationMap is map)
{
    for (var index, vertexOverride in definition.vertexOverrides)
    {
        if (vertexOverride.overrideType == VertexOverrideType.UP_TO_VERTEX)
        {
            continue;
        }

        // Vertex override manipulators use this index on change to identify the appropriate definition, so match it
        // with the array item indexing.
        const manipulatorBaseId = OVERRIDE_MANIPULATOR ~ toString(index);
        const edge = getOneAdjacentEdgeForVertex(context, definition.edges, vertexOverride.vertex);
        if (isQueryEmpty(context, edge))
        {
            continue; // Vertex override does not belong to this path.
        }

        const position = manipulatorPosition(context, edge, vertexOverride.vertex);
        if (vertexOverride.overrideType == VertexOverrideType.ALIGNED_WITH_VECTOR)
        {
            const parameterId = undefined; // Parameter highlights are not currently working for array parameters.
            addAlignedWithVectorManipulator(context, id, manipulatorBaseId, position, vertexOverride.axisOverride, vertexOverride.distanceOverride, vertexOverride.oppositeDirectionOverride, parameterId);
        }
        else
        {
            addAngleFromManipulators(context, id, definition, {
                        "position" : position,
                        "faces" : faces,
                        "angle" : vertexOverride.angleOverride,
                        "distance" : vertexOverride.distanceOverride,
                        "isTangent" : vertexOverride.overrideType == VertexOverrideType.TANGENT,
                        "oppositeDirection" : vertexOverride.oppositeDirectionOverride,
                        "oppositeAngle" : vertexOverride.oppositeAngleOverride,
                        "manipulatorBaseId" : manipulatorBaseId,
                        "edgeToOrientationMap" : edgeToOrientationMap
                    });
        }
    }
}

function isEdgeAlignedWithFace(context is Context, edge is Query, face is Query) returns boolean
{
    const coEdgeTangentLine = evEdgeTangentLine(context, {
                "edge" : edge,
                "parameter" : 0.5,
                "face" : face
            });
    const edgeTangentLine = evEdgeTangentLine(context, {
                "edge" : edge,
                "parameter" : 0.5
            });

    return dot(coEdgeTangentLine.direction, edgeTangentLine.direction) > 0;
}

function getRuledDirectionsSketchEdge(context is Context, edge is Query, manipulatorOptions is map)
{
    const transientQuery = evaluateQuery(context, edge)[0];
    const isFlipped = manipulatorOptions.edgeToOrientationMap[transientQuery];

    const sketchPlane = evOwnerSketchPlane(context, {
                "entity" : transientQuery
            });

    return getRuledDirectionsReferenceDirection(context, edge, sketchPlane.normal, manipulatorOptions);
}

function getRuledDirectionsReferenceDirection(context is Context, edge is Query, referenceDirection is Vector, manipulatorOptions is map) returns map
{
    const transientQuery = evaluateQuery(context, edge)[0];
    const isFlipped = manipulatorOptions.edgeToOrientationMap[transientQuery];

    var parameter = manipulatorOptions.position->getParameter();

    var tangent = evEdgeTangentLine(context, {
                "edge" : transientQuery,
                "parameter" : parameter
            }).direction;

    if (isFlipped)
    {
        tangent *= -1;
    }

    if (parallelVectors(tangent, referenceDirection))
    {
        throw regenError(ErrorStringEnum.RULED_SURFACE_EDGE_PARALLEL_REFERENCE, ["axis"], edge);
    }

    const xDirection = normalize(cross(tangent, referenceDirection));
    const rotationAxis = cross(referenceDirection, xDirection);

    var baseDirection = referenceDirection;
    if (manipulatorOptions.isTangent)
    {
        baseDirection = rotationMatrix3d(rotationAxis, PI / 2 * radian) * baseDirection;
    }
    const ruledDirection = rotationMatrix3d(rotationAxis, manipulatorOptions.oppositeAngle ? -manipulatorOptions.angle : manipulatorOptions.angle) * baseDirection;

    return {
            "ruledDirection" : ruledDirection,
            "tangent" : rotationAxis,
            "baseDirection" : baseDirection
        };
}

function getRuledDirectionsFace(context is Context, edge is Query, face is Query, manipulatorOptions is map) returns map
{
    var parameterOnEdge = manipulatorOptions.position->getParameter();

    if (!isEdgeAlignedWithFace(context, edge, face))
    {
        parameterOnEdge = 1 - parameterOnEdge;
    }

    var baseDirection = evFaceNormalAtEdge(context, {
            "edge" : edge,
            "face" : face,
            "parameter" : parameterOnEdge,
            "usingFaceOrientation" : true
        });

    const tangentLine = evEdgeTangentLine(context, {
                "edge" : edge,
                "parameter" : parameterOnEdge,
                "face" : face
            });

    if (manipulatorOptions.isTangent)
    {
        baseDirection = rotationMatrix3d(tangentLine.direction, PI / 2 * radian) * baseDirection;
    }
    var ruledDirection = rotationMatrix3d(tangentLine.direction, manipulatorOptions.oppositeAngle ? -manipulatorOptions.angle : manipulatorOptions.angle) * baseDirection;
    return {
            "ruledDirection" : ruledDirection,
            "tangent" : tangentLine.direction,
            "baseDirection" : baseDirection
        };
}

function addDistanceManipulator(context is Context, id is Id, manipulatorId is string, origin is Vector, direction is Vector, distance is ValueWithUnits, parameterId)
{
    addManipulators(context, id, { (DISTANCE_MANIPULATOR ~ manipulatorId) : linearManipulator({
                        "base" : origin,
                        "direction" : direction,
                        "offset" : distance,
                        "primaryParameterId" : parameterId
                    }) });
}

function addAngularManipulator(context is Context, id is Id, manipulatorId is string, origin is Vector, baseDirection is Vector,
    distance is ValueWithUnits, angle is ValueWithUnits, tangentDirection is Vector, parameterId)
{
    // The sign of the angle is used for the direction of the manipulator arrow, so make sure it is preserved when bringing the angle into the bounds.
    var adjustedAngle = angle % (2 * PI * radian);
    if (angle < 0)
    {
        adjustedAngle = adjustedAngle - 2 * PI * radian;
    }
    addManipulators(context, id, { (ANGLE_MANIPULATOR ~ manipulatorId) : angularManipulator({
                        "axisOrigin" : origin,
                        "axisDirection" : tangentDirection,
                        "rotationOrigin" : origin + baseDirection * distance,
                        "angle" : adjustedAngle,
                        "style" : ManipulatorStyleEnum.DEFAULT,
                        "minValue" : -2 * PI * radian,
                        "maxValue" : 2 * PI * radian,
                        "disableMinimumOffset" : true,
                        "primaryParameterId" : parameterId
                    }) });
}

/** @internal */
export function ruledSurfaceManipulator(context is Context, definition is map, newManipulators is map) returns map
{
    var manipulatorKeys = keys(newManipulators);
    const regexStr = "(?:" ~ DISTANCE_MANIPULATOR ~ "|" ~ ANGLE_MANIPULATOR ~ ")(" ~ TOP_LEVEL_MANIPULATOR ~ "|" ~ OVERRIDE_MANIPULATOR ~ ")(-?\\d+)";
    for (var key in manipulatorKeys)
    {
        var manipulator = newManipulators[key];
        var parsed = match(key, regexStr);
        if (!parsed.hasMatch)
        {
            return;
        }
        const vertexIndex = stringToNumber(parsed.captures[2]);
        const relativeType = parsed.captures[1];
        if (manipulator.manipulatorType == ManipulatorType.LINEAR_1D)
        {
            definition = processLinearManipulator(definition, manipulator, vertexIndex, relativeType);
        }
        else if (manipulator.manipulatorType == ManipulatorType.ANGULAR)
        {
            definition = processAngularManipulator(definition, manipulator, vertexIndex, relativeType);
        }
    }
    return definition;
}

function processLinearManipulator(definition is map, manipulator is Manipulator, index is number, relativeType is string) returns map
{
    if (relativeType == TOP_LEVEL_MANIPULATOR)
    {
        definition.distance = abs(manipulator.offset);
        definition.oppositeDirection = manipulator.offset < 0;
    }
    else
    {
        var vertexOverride = definition.vertexOverrides[index];
        vertexOverride.distanceOverride = abs(manipulator.offset);
        vertexOverride.oppositeDirectionOverride = manipulator.offset < 0;
        definition.vertexOverrides[index] = vertexOverride;
    }

    return definition;
}

function processAngularManipulator(definition is map, manipulator is Manipulator, index is number, relativeType is string) returns map
{
    if (relativeType == TOP_LEVEL_MANIPULATOR)
    {
        if (definition.ruledType == RuledSurfaceInterfaceType.ANGLE_FROM_VECTOR)
        {
            definition.angleFromVector = abs(manipulator.angle);
            definition.oppositeAngleFromVector = manipulator.angle < 0;
        }
        else
        {
            definition.angle = abs(manipulator.angle);
            definition.oppositeAngle = manipulator.angle < 0;
        }
    }
    else
    {
        var vertexOverride = definition.vertexOverrides[index];
        vertexOverride.angleOverride = abs(manipulator.angle);
        vertexOverride.oppositeAngleOverride = manipulator.angle < 0;
        definition.vertexOverrides[index] = vertexOverride;
    }

    return definition;
}

/**
 * @internal
 */
export function ruledSurfaceEditingLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    var modifiedDefinition = definition;
    if (!specifiedParameters.referenceFaces && oldDefinition.edges != definition.edges)
    {
        const paths = try silent(constructPaths(context, definition.edges, { "adjacentSeedFaces" : definition.referenceFaces }));
        var newReferenceFacesArray = [];
        if (paths != undefined)
        {
            for (var path in paths)
            {
                // If constructPaths is able to infer adjacent faces on one side of the path from adjacentSeedFaces, it will return them.
                // If this property is undefined, insert an adjacent face of these edges into the definition.
                if (path.adjacentFaces == undefined)
                {
                    const firstAdjacentFace = qNthElement(qAdjacent(qUnion(path.edges), AdjacencyType.EDGE, EntityType.FACE), 0);
                    const newAdjacentFace = evaluateQuery(context, firstAdjacentFace);
                    if (newAdjacentFace != [])
                    {
                        newReferenceFacesArray = newReferenceFacesArray->append(newAdjacentFace[0]);
                    }
                }
            }
        }

        if (newReferenceFacesArray != [])
        {
            modifiedDefinition.referenceFaces = qUnion([definition.referenceFaces, qUnion(newReferenceFacesArray)]);
        }
    }

    if (definition.ruledType == RuledSurfaceInterfaceType.ALIGNED_WITH_VECTOR &&
        definition.vertexOverrides->size() > oldDefinition.vertexOverrides->size())
    {
        const index = definition.vertexOverrides->size() - 1;
        var vertexOverride = definition.vertexOverrides[index];
        vertexOverride.overrideType = VertexOverrideType.ALIGNED_WITH_VECTOR;
        modifiedDefinition.vertexOverrides[index] = vertexOverride;
    }
    return surfaceOperationTypeEditLogic(context, id, modifiedDefinition, specifiedParameters, modifiedDefinition.edges, hiddenBodies);
}

function createEdgeToOrientationMap(context is Context, paths is array) returns map
{
    var edgeToOrientationMap = {};
    for (var path in paths)
    {
        for (var index, edge in path.edges)
        {
            edgeToOrientationMap[edge] = path.flipped[index];
        }
    }
    return edgeToOrientationMap;
}

