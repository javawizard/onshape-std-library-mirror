FeatureScript 225; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/evaluate.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/mathUtils.fs", version : "");
import(path : "onshape/std/surfaceGeometry.fs", version : "");
import(path : "onshape/std/valueBounds.fs", version : "");

/**
 * TODO: description
 */
export enum CPlaneType
{
    annotation { "Name" : "Offset" }
    OFFSET,
    annotation { "Name" : "Plane Point" }
    PLANE_POINT,
    annotation { "Name" : "Line Angle" }
    LINE_ANGLE,
    annotation { "Name" : "Line Point" }
    LINE_POINT,
    annotation { "Name" : "Three Point" }
    THREE_POINT,
    annotation { "Name" : "Mid Plane" }
    MID_PLANE,
    annotation { "Name" : "Curve Point" }
    CURVE_POINT
}

// Messages
const midPlaneDefaultErrorMessage  = ErrorStringEnum.CPLANE_INPUT_MIDPLANE;
const requiresPlaneToOffsetMessage = ErrorStringEnum.CPLANE_INPUT_OFFSET_PLANE;
const requiresPointPlaneMessage    = ErrorStringEnum.CPLANE_INPUT_POINT_PLANE;
const requiresLineMessage          = ErrorStringEnum.CPLANE_INPUT_LINE_ANGLE;
const requiresLinePointMessage     = ErrorStringEnum.CPLANE_INPUT_POINT_LINE;
const tooManyEntitiesMessage       = ErrorStringEnum.TOO_MANY_ENTITIES_SELECTED;
const requiresThreePointsMessage   = ErrorStringEnum.CPLANE_INPUT_THREE_POINT;
const degeneratePointsMessage      = ErrorStringEnum.POINTS_COINCIDENT;
const coincidentPointsMessage      = ErrorStringEnum.POINTS_COINCIDENT;
const edgeIsClosedLoopMessage      = ErrorStringEnum.CPLANE_INPUT_MIDPLANE;
const requiresCurvePointMessage    = ErrorStringEnum.CPLANE_INPUT_CURVE_POINT;

/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
annotation { "Feature Type Name" : "Plane", "UIHint" : "CONTROL_VISIBILITY" }
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
            isAngle(definition.angle, ANGLE_360_BOUNDS);
        }

        if (definition.cplaneType == CPlaneType.MID_PLANE)
        {
            annotation { "Name" : "Flip alignment" }
            definition.flipAlignment is boolean;
        }

        annotation { "Name" : "Display size", "UIHint" : "ALWAYS_HIDDEN" }
        isLength(definition.size, PLANE_SIZE_BOUNDS);
    }
    //============================ Body =============================
    {
        const entities = evaluateQuery(context, definition.entities);
        const numEntities = @size(entities);

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
            if (numEntities < 1)
                throw regenError(requiresLineMessage, ["entities"]);
            if (numEntities > 1)
                throw regenError(tooManyEntitiesMessage, ["entities"]);
            const lineResult = evAxis(context, { "axis" : definition.entities });
            var normal = perpendicularVector(lineResult.direction);
            normal = rotationMatrix3d(lineResult.direction, definition.angle) * normal;
            definition.plane = plane(lineResult.origin, normal, lineResult.direction);
        }

        if (definition.cplaneType == CPlaneType.LINE_POINT)
        {
            if (numEntities < 2)
                throw regenError(requiresLinePointMessage, ["entities"]);
            if (numEntities > 2)
                throw regenError(tooManyEntitiesMessage, ["entities"]);
            const lineResult = evAxis(context, { "axis" : qUnion([qEntityFilter(definition.entities, EntityType.EDGE),
                                      qEntityFilter(definition.entities, EntityType.FACE)]) });
            const pointResult = evVertexPoint(context, { "vertex" : qEntityFilter(definition.entities, EntityType.VERTEX) });
            definition.plane = plane(pointResult, lineResult.direction);
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
            definition.plane = plane(points[0], normalize(normal), normalize(points[1] - points[0]));
        }

        if (definition.cplaneType == CPlaneType.MID_PLANE)
        {
            // attempt from two points
            const vertexQueries = evaluateQuery(context, qEntityFilter(definition.entities, EntityType.VERTEX));
            if (@size(vertexQueries) == 2)
            {
                // Check for extra entities, not vertices
                if (numEntities > 2)
                    throw regenError(tooManyEntitiesMessage, ["entities"]);
                createMidPlaneFromTwoPoints(context, id, vertexQueries, definition.size);
                return;
            }

            // attempt from a edge
            const edgeQueries = evaluateQuery(context, qEntityFilter(definition.entities, EntityType.EDGE));
            if (@size(edgeQueries) == 1)
            {
                // Check for extra entities, not edges
                if (numEntities > 1)
                    throw regenError(tooManyEntitiesMessage, ["entities"]);
                createMidPlaneFromEdge(context, id, edgeQueries, definition.size);
                return;
            }

            // attempt from 2 planes
            const faceQueries = evaluateQuery(context, qEntityFilter(definition.entities, EntityType.FACE));
            if (@size(faceQueries) == 2)
            {
                // Check for extra entities, not faces
                if (numEntities > 2)
                    throw regenError(tooManyEntitiesMessage, ["entities"]);
                createMidPlaneFromTwoPlanes(context, id, definition);
                return;
            }

            // fall through to failure
            throw regenError(midPlaneDefaultErrorMessage, ["entities"]);
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
                param = evProjectPointOnCurve(context, { "edge" : qEntityFilter(definition.entities, EntityType.EDGE),
                                                             "vertex" : qEntityFilter(definition.entities, EntityType.VERTEX) });
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

        opPlane(context, id, definition);
    }, { oppositeDirection : false, flipAlignment : false });

function createMidPlaneFromTwoPoints(context is Context, id is Id, vertexQueries is array, size is ValueWithUnits)
{
    var points = makeArray(2);
    for (var i = 0; i < 2; i += 1)
        points[i] = evVertexPoint(context, { "vertex" : vertexQueries[i] });

    var normal = points[1] - points[0];
    if (norm(normal) <= TOLERANCE.zeroLength * meter)
        throw regenError(coincidentPointsMessage, ["entities"]);

    normal = normalize(normal);
    const midOrigin = 0.5 * (points[0] + points[1]);
    opPlane(context, id, { "plane" : plane(midOrigin, normal), "size" : size });
}

function createMidPlaneFromEdge(context is Context, id is Id, edgeQueries is array, size is ValueWithUnits)
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
    opPlane(context, id, { "plane" : plane(midOrigin, normal), "size" : size });
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

    opPlane(context, id, cplaneDefinition);
}

