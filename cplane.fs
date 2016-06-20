FeatureScript 370; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "370.0");

// Imports used internally
import(path : "onshape/std/box.fs", version : "370.0");
import(path : "onshape/std/containers.fs", version : "370.0");
import(path : "onshape/std/evaluate.fs", version : "370.0");
import(path : "onshape/std/feature.fs", version : "370.0");
import(path : "onshape/std/mathUtils.fs", version : "370.0");
import(path : "onshape/std/curveGeometry.fs", version : "370.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "370.0");
import(path : "onshape/std/valueBounds.fs", version : "370.0");

/**
 * The method of defining a construction plane.
 */
export enum CPlaneType
{
    annotation { "Name" : "Offset" }
    OFFSET,
    annotation { "Name" : "Plane Point" }
    PLANE_POINT,
    annotation { "Name" : "Line Angle" }
    LINE_ANGLE,
    annotation { "Name" : "Point Normal" }
    LINE_POINT,
    annotation { "Name" : "Three Point" }
    THREE_POINT,
    annotation { "Name" : "Mid Plane" }
    MID_PLANE,
    annotation { "Name" : "Curve Point" }
    CURVE_POINT
}

// Messages
const midPlaneDefaultErrorMessage    = ErrorStringEnum.CPLANE_INPUT_MIDPLANE;
const requiresPlaneToOffsetMessage   = ErrorStringEnum.CPLANE_INPUT_OFFSET_PLANE;
const requiresPointPlaneMessage      = ErrorStringEnum.CPLANE_INPUT_POINT_PLANE;
const requiresLineAngleSelectMessage = ErrorStringEnum.CPLANE_SELECT_LINE_ANGLE_REFERENCE;
const requiresLineAxisMessage        = ErrorStringEnum.CPLANE_INPUT_LINE_ANGLE2;
const requiresLinePointMessage       = ErrorStringEnum.CPLANE_INPUT_POINT_LINE;
const degenerateSelectionMessage     = ErrorStringEnum.CPLANE_DEGENERATE_SELECTION;
const tooManyEntitiesMessage         = ErrorStringEnum.TOO_MANY_ENTITIES_SELECTED;
const requiresThreePointsMessage     = ErrorStringEnum.CPLANE_INPUT_THREE_POINT;
const degeneratePointsMessage        = ErrorStringEnum.POINTS_COINCIDENT;
const coincidentPointsMessage        = ErrorStringEnum.POINTS_COINCIDENT;
const edgeIsClosedLoopMessage        = ErrorStringEnum.CPLANE_INPUT_MIDPLANE;
const requiresCurvePointMessage      = ErrorStringEnum.CPLANE_INPUT_CURVE_POINT;

// Factor by which to extend default plane size
const PLANE_SIZE_EXTENSION_FACTOR = 0.2;

const PLANE_OFFSET_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0, 0.025, 500],
    (centimeter) : 2.5,
    (millimeter) : 25,
    (inch)       : 1,
    (foot)       : 0.0833,
    (yard)       : 0.0277
} as LengthBoundSpec;

/**
 * Creates a construction plane feature by calling `opPlane`.
 */
annotation { "Feature Type Name" : "Plane", "UIHint" : "CONTROL_VISIBILITY", "Editing Logic Function" : "cPlaneLogic"}
export const cPlane = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities",
                    "Filter" : GeometryType.PLANE || EntityType.VERTEX || QueryFilterCompound.ALLOWS_AXIS || EntityType.EDGE }
        definition.entities is Query;

        annotation { "Name" : "Plane type" }
        definition.cplaneType is CPlaneType;

        if (definition.cplaneType == CPlaneType.OFFSET)
        {
            annotation { "Name" : "Offset distance" }
            isLength(definition.offset, PLANE_OFFSET_BOUNDS);

            annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
            definition.oppositeDirection is boolean;
        }

        if (definition.cplaneType == CPlaneType.LINE_ANGLE)
        {
            annotation { "Name" : "Angle" }
            isAngle(definition.angle, ANGLE_360_ZERO_DEFAULT_BOUNDS);
        }

        if (definition.cplaneType == CPlaneType.MID_PLANE)
        {
            annotation { "Name" : "Flip alignment" }
            definition.flipAlignment is boolean;
        }

        annotation { "Name" : "Starting width", "UIHint" : "ALWAYS_HIDDEN" }
        isLength(definition.width, PLANE_SIZE_BOUNDS);

        annotation { "Name" : "Starting height", "UIHint" : "ALWAYS_HIDDEN" }
        isLength(definition.height, PLANE_SIZE_BOUNDS);
    }
    //============================ Body =============================
    {
        const entities = evaluateQuery(context, definition.entities);
        const numEntities = @size(entities);

        var remainingTransform = getRemainderPatternTransform(context,
            {"references" : definition.entities});

        if (definition.cplaneType == CPlaneType.OFFSET)
        {
            if (numEntities < 1)
                throw regenError(requiresPlaneToOffsetMessage, ["entities"]);
            if (numEntities > 1)
                throw regenError(tooManyEntitiesMessage, ["entities"]);
            definition.plane = evPlane(context, { "face" : definition.entities });
            if (definition.oppositeDirection)
                definition.offset = -definition.offset;
            definition.plane.origin += definition.plane.normal * definition.offset;
        }

        if (definition.cplaneType == CPlaneType.PLANE_POINT)
        {
            if (numEntities < 2)
                throw regenError(requiresPointPlaneMessage, ["entities"]);
            if (numEntities > 2)
                throw regenError(tooManyEntitiesMessage, ["entities"]);
            definition.plane = evPlane(context, { "face" : qEntityFilter(definition.entities, EntityType.FACE) });
            definition.plane.origin = evVertexPoint(context, { "vertex" : qEntityFilter(definition.entities, EntityType.VERTEX) });
        }

        if (definition.cplaneType == CPlaneType.LINE_ANGLE)
        {
            definition.plane = lineAnglePlane(context, id, entities, definition.angle);
        }

        if (definition.cplaneType == CPlaneType.LINE_POINT) // Point normal
        {
            if (numEntities < 2)
                throw regenError(requiresLinePointMessage, ["entities"]);
            if (numEntities > 2)
                throw regenError(tooManyEntitiesMessage, ["entities"]);
            const axisResult = evAxis(context, { "axis" : qUnion([qEntityFilter(definition.entities, EntityType.EDGE),
                                    qEntityFilter(definition.entities, EntityType.FACE)]) });
            const pointResult = evVertexPoint(context, { "vertex" : qEntityFilter(definition.entities, EntityType.VERTEX) });
            definition.plane = plane(pointResult, axisResult.direction);
        }

        if (definition.cplaneType == CPlaneType.THREE_POINT)
        {
            const vertexQueries = evaluateQuery(context, qEntityFilter(definition.entities, EntityType.VERTEX));
            if (size(vertexQueries) < 3)
                throw regenError(requiresThreePointsMessage, ["entities"]);
            if (numEntities > 3)
                throw regenError(tooManyEntitiesMessage, ["entities"]);
            var points = makeArray(3);
            for (var i = 0; i < 3; i += 1)
            {
                points[i] = evVertexPoint(context, { "vertex" : vertexQueries[i] });
            }
            const normal = cross(points[2] - points[0], points[1] - points[0]);
            if (norm(normal).value < TOLERANCE.zeroLength)
                throw regenError(degeneratePointsMessage, ["entities"]);

            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V316_PLANE_DEFAULT_SIZE_FIX))
            {
                var center = (points[0] + points[1] + points[2]) / 3;
                definition.plane = plane(center, normalize(normal), normalize(points[1] - points[0]));

                // Evaluate size here to avoid re-evaluating queries in the plane size function
                var maxLength = max(max(norm(definition.plane.origin - points[0]), norm(definition.plane.origin - points[1])), norm(definition.plane.origin - points[2]));
                var newSize = maxLength * 2.0;
                var sizeOffset = newSize * PLANE_SIZE_EXTENSION_FACTOR;
                definition.width = newSize + sizeOffset;
                definition.height = newSize + sizeOffset;
            }
            else
            {
                definition.plane = plane(points[0], normalize(normal), normalize(points[1] - points[0]));
            }
        }

        if (definition.cplaneType == CPlaneType.MID_PLANE)
        {

            const vertexQueries = evaluateQuery(context, qEntityFilter(definition.entities, EntityType.VERTEX));
            const edgeQueries = evaluateQuery(context, qEntityFilter(definition.entities, EntityType.EDGE));
            const faceQueries = evaluateQuery(context, qEntityFilter(definition.entities, EntityType.FACE));

            // attempt from two points
            if (@size(vertexQueries) == 2)
            {
                // Check for extra entities, not vertices
                if (numEntities > 2)
                    throw regenError(tooManyEntitiesMessage, ["entities"]);
                definition.plane = createMidPlaneFromTwoPoints(context, id, vertexQueries);

            }

            // attempt from a edge
            else if (@size(edgeQueries) == 1)

            {
                // Check for extra entities, not edges
                if (numEntities > 1)
                    throw regenError(tooManyEntitiesMessage, ["entities"]);
                definition.plane = createMidPlaneFromEdge(context, id, edgeQueries);

            }

            // attempt from 2 planes
            else if (@size(faceQueries) == 2)

            {
                // Check for extra entities, not faces
                if (numEntities > 2)
                    throw regenError(tooManyEntitiesMessage, ["entities"]);
                definition.plane = createMidPlaneFromTwoPlanes(context, id, definition);

            }

            // error if our plane definition hasn't been created yet
            if (definition.plane == undefined)
            {
                throw regenError(midPlaneDefaultErrorMessage, ["entities"]);
            }
        }

        if (definition.cplaneType == CPlaneType.CURVE_POINT)
        {
            if (numEntities < 2)
                throw regenError(requiresCurvePointMessage, ["entities"]);
            if (numEntities > 2)
                throw regenError(tooManyEntitiesMessage, ["entities"]);

            var param;
            try
            {
                param = evDistance(context, {
                                    "side0" : qEntityFilter(definition.entities, EntityType.VERTEX),
                                    "side1" : qEntityFilter(definition.entities, EntityType.EDGE),
                                    "extendSide1" : true
                                }).sides[1].parameter;
            }
            catch (error)
            {
                if (try(error.message as string) == "CANNOT_RESOLVE_ENTITIES")
                    throw regenError(requiresCurvePointMessage, ["entities"]);
                error.faultyParameters = ["entities"];
                throw error;
            }

            const lineResult = evEdgeTangentLine(context, { "edge" : qEntityFilter(definition.entities, EntityType.EDGE),
                        "parameter" : param, "arcLengthParameterization" : false });

            definition.plane = plane(lineResult.origin, lineResult.direction);
        }

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V316_PLANE_DEFAULT_SIZE_FIX))
        {
            var planeSize = getPlaneDefaultSize(context, definition);
            definition.width = planeSize[0];
            definition.height = planeSize[1];
        }

        opPlane(context, id, definition);
        transformResultIfNecessary(context, id, remainingTransform);

    }, { oppositeDirection : false, flipAlignment : false, width : 1 * meter, height : 1 * meter });

function lineAnglePlane(context is Context, id is Id, entities is array, angle is ValueWithUnits) returns Plane
{
    if (size(entities) == 1)
    {
        reportFeatureInfo(context, id, requiresLineAngleSelectMessage);
        const lineResult = evAxis(context, { "axis" : entities[0] });
        var normal = perpendicularVector(lineResult.direction);
        normal = rotationMatrix3d(lineResult.direction, angle) * normal;
        return plane(lineResult.origin, normal, lineResult.direction);
    }

    if (size(entities) < 2)
        throw regenError(requiresLineAxisMessage, ["entities"]);
    if (size(entities) > 2)
        throw regenError(tooManyEntitiesMessage, ["entities"]);
    var axis1 = try(evAxis(context, { "axis" : entities[0] }));
    var axis2 = try(evAxis(context, { "axis" : entities[1] }));

    if (axis1 == undefined) // If the plane or point is selected first, swap.
    {
        if (axis2 == undefined)
            throw regenError(requiresLineAxisMessage, ["entities"]);
        axis1 = axis2;
        axis2 = undefined;
        entities[1] = entities[0];
    }

    // The second entity can be an axis, a plane, or a point
    var secondInPlaneDirection;
    if (axis2 != undefined)
    {
        if (parallelVectors(axis1.direction, axis2.direction))
            secondInPlaneDirection = axis2.origin - axis1.origin;
        else
            secondInPlaneDirection = axis2.direction;
    }
    else
    {
        var plane = try(evPlane(context, { "face" : entities[1] }));
        if (plane != undefined)
        {
            secondInPlaneDirection = cross(axis1.direction, plane.normal);
        }
        else
        {
            var point = try(evVertexPoint(context, { "vertex" : entities[1] }));
            if (point != undefined)
                secondInPlaneDirection = point - axis1.origin;
        }
    }
    if (secondInPlaneDirection == undefined)
        throw regenError(requiresLineAxisMessage, ["entities"]);

    var normal = cross(axis1.direction, secondInPlaneDirection);
    if (stripUnits(squaredNorm(normal)) < TOLERANCE.zeroLength * TOLERANCE.zeroLength)
        throw regenError(degenerateSelectionMessage, ["entities"]);

    normal = rotationMatrix3d(axis1.direction, angle) * normal;
    return plane(axis1.origin, normal, axis1.direction);
}

function getPlaneDefaultSize(context is Context, definition is map) returns array
{
    var planeType = definition.cplaneType;
    var planeBounds = [definition.width, definition.height];
    if (planeType == CPlaneType.OFFSET)
    {
        var isConstruction = false;
        const cSys = planeToCSys(definition.plane);
        var bounds = evBox3d(context, { 'topology' : definition.entities, 'cSys' : cSys });

        if (planeType == CPlaneType.OFFSET)
        {
            var constructionFilteredEntities = evaluateQuery(context, qConstructionFilter(definition.entities, ConstructionObject.NO));
            if (@size(constructionFilteredEntities) == 0)
            {
                isConstruction = true;
            }
        }

        planeBounds = getExpandedPlaneBounds(bounds, isConstruction);
    }
    else if (planeType == CPlaneType.LINE_ANGLE)
    {
        const edgeQueries = evaluateQuery(context, qEntityFilter(definition.entities, EntityType.EDGE));
        if (edgeQueries != undefined && @size(edgeQueries) > 0)
        {
            const edgeEndPoints = evEdgeTangentLines(context, { "edge" : edgeQueries[0], "parameters" : [0, 1] });
            if (edgeEndPoints != undefined && @size(edgeEndPoints) > 1)
            {
                var normal = edgeEndPoints[0].origin - edgeEndPoints[1].origin;
                var size = norm(normal) * (1 + PLANE_SIZE_EXTENSION_FACTOR);
                planeBounds = [size, size];
            }
        }
    }
    else if (planeType == CPlaneType.PLANE_POINT)
    {
        const cSys = planeToCSys(definition.plane);
        var bounds = evBox3d(context, { 'topology' :  qEntityFilter(definition.entities, EntityType.FACE), 'cSys' : cSys });

        var isConstruction = false;
        var constructionFilteredEntities = evaluateQuery(context, qConstructionFilter(qEntityFilter(definition.entities, EntityType.FACE), ConstructionObject.NO));
        if (@size(constructionFilteredEntities) == 0)
        {
            isConstruction = true;
        }

        planeBounds = getExpandedPlaneBounds(bounds, isConstruction);
    }
    else if (planeType == CPlaneType.MID_PLANE)
    {
        var isConstruction = false;
        var faceFilteredEntities = evaluateQuery(context, qEntityFilter(definition.entities, EntityType.FACE));
        // If this mid plane is not based on a face, return the default plane bounds
        if (@size(faceFilteredEntities) == 0)
        {
            return planeBounds;
        }

        const cSys = planeToCSys(definition.plane);
        var bounds = evBox3d(context, { 'topology' : qEntityFilter(definition.entities, EntityType.FACE), 'cSys' : cSys });

        if (bounds == undefined)
        {
            return planeBounds;
        }

        var constructionFilteredEntities = evaluateQuery(context, qConstructionFilter(definition.entities, ConstructionObject.NO));
        if (@size(constructionFilteredEntities) == 0)
        {
            isConstruction = true;
        }

        planeBounds = getExpandedPlaneBounds(bounds, isConstruction);
    }

    return planeBounds;
}

function getExpandedPlaneBounds(bounds is Box3d, isConstruction is boolean) returns array
{
    var planeExpansionFactor = (isConstruction ? 0.0 : PLANE_SIZE_EXTENSION_FACTOR);
    var minPlaneCorner = bounds.minCorner;
    var maxPlaneCorner = bounds.maxCorner;

    var height = abs(maxPlaneCorner[1] - minPlaneCorner[1]);
    var width = abs(maxPlaneCorner[0] - minPlaneCorner[0]);
    var offset = max(width, height) * planeExpansionFactor;
    return [width + offset, height + offset];
}

function createMidPlaneFromTwoPoints(context is Context, id is Id, vertexQueries is array) returns Plane
{
    var points = makeArray(2);
    for (var i = 0; i < 2; i += 1)
        points[i] = evVertexPoint(context, { "vertex" : vertexQueries[i] });

    var normal = points[1] - points[0];
    if (norm(normal) <= TOLERANCE.zeroLength * meter)
        throw regenError(coincidentPointsMessage, ["entities"]);

    normal = normalize(normal);
    const midOrigin = 0.5 * (points[0] + points[1]);

    return plane(midOrigin, normal);
}

function createMidPlaneFromEdge(context is Context, id is Id, edgeQueries is array) returns Plane
{
    var points = makeArray(2);
    const edgeEndPoints = evEdgeTangentLines(context, { "edge" : edgeQueries[0], "parameters" : [0, 1] });
    points[0] = edgeEndPoints[0].origin;
    points[1] = edgeEndPoints[1].origin;
    var normal = points[1] - points[0];
    if (norm(normal) <= TOLERANCE.zeroLength * meter)
        throw regenError(edgeIsClosedLoopMessage, ["entities"]);

    normal = normalize(normal);
    const midOrigin = 0.5 * (points[0] + points[1]);

    return plane(midOrigin, normal);
}

function createMidPlaneFromTwoPlanes(context is Context, id is Id, cplaneDefinition is map)
{
    var p1 = evPlane(context, { "face" : qNthElement(cplaneDefinition.entities, 0) });
    var p2 = evPlane(context, { "face" : qNthElement(cplaneDefinition.entities, 1) });

    // By default, we want a plane to be the bisector of two adjacent plane of a solid, like bisecting a wedge of a cake.
    // Negate plane2's normal to get that. The other solution is a plane perpendicular to the bisecting plane
    if (!cplaneDefinition.flipAlignment)
        p2.normal = -p2.normal;

    const midOrigin = 0.5 * (p1.origin + p2.origin);
    const intersection = intersection(p1, p2);

    // Check for parallel case, when two planes are parallel there are no other solution we just take the normal of plane1
    if (intersection == undefined)
    {
        cplaneDefinition.plane = plane(midOrigin, p1.normal, p1.x);
    }
    else
    {
        const normal = normalize(p1.normal + p2.normal);
        const x = rotationMatrix3d(p1.normal, normal) * p1.x;
        cplaneDefinition.plane = plane(intersection.origin, normal, x);
        cplaneDefinition.plane.origin = project(cplaneDefinition.plane, midOrigin);
    }

    return cplaneDefinition.plane;
}

/**
 * Heuristics to determine the type of plane to be constructed, based on user preselection.
 */
export function cPlaneLogic(context is Context, id is Id, oldDefinition is map, definition is map) returns map
{
    if (oldDefinition != {}) // Only do anything on preselection
        return definition;

    const entities = definition.entities;

    const total is number = size(evaluateQuery(context, entities));
    const vertices is number = size(evaluateQuery(context, qEntityFilter(entities, EntityType.VERTEX)));
    const lines is number = size(evaluateQuery(context, qGeometry(entities, GeometryType.LINE)));
    const planes is number = size(evaluateQuery(context, qGeometry(entities, GeometryType.PLANE)));
    const curves is number = size(evaluateQuery(context, qSubtraction(qEntityFilter(entities, EntityType.EDGE),
                    qGeometry(entities, GeometryType.LINE))));

    if (total == 1)
    {
        if (planes == 1)
            definition.cplaneType = CPlaneType.OFFSET;
        else if ((lines + curves) == 1 && size(evaluateQuery(context, qVertexAdjacent(entities, EntityType.VERTEX))) == 2)
            definition.cplaneType = CPlaneType.MID_PLANE;
    }
    if (total == 2)
    {
        if (planes == 1 && vertices == 1)
            definition.cplaneType = CPlaneType.PLANE_POINT;
        else if (planes == 2 || vertices == 2)
            definition.cplaneType = CPlaneType.MID_PLANE;
        else if (curves == 1 && vertices == 1)
            definition.cplaneType = CPlaneType.CURVE_POINT;
        else if (vertices == 1) // The other thing must be a plane or an axis
            definition.cplaneType = CPlaneType.LINE_POINT; // Point and normal
        else if (lines >= 1 && (lines + vertices + planes) == 2)
            definition.cplaneType = CPlaneType.LINE_ANGLE;
    }
    if (total == 3 && vertices == 3)
        definition.cplaneType = CPlaneType.THREE_POINT;

    return definition;
}
