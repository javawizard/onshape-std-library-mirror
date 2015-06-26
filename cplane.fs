FeatureScript ✨; /* Automatically generated version */
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/boolean.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");

export enum CPlaneType
{
    annotation {"Name" : "Offset"}
    OFFSET,
    annotation {"Name" : "Plane Point"}
    PLANE_POINT,
    annotation {"Name" : "Line Angle"}
    LINE_ANGLE,
    annotation {"Name" : "Line Point"}
    LINE_POINT,
    annotation {"Name" : "Three Point"}
    THREE_POINT,
    annotation {"Name" : "Mid Plane"}
    MID_PLANE,
    annotation {"Name" : "Curve Point"}
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

annotation {"Feature Type Name" : "Plane", "UIHint" : "CONTROL_VISIBILITY"}
export const cPlane = defineFeature(function(context is Context, id is Id, cplaneDefinition is map)
    precondition
    {

        annotation {"Name" : "Entities",
                    "Filter" : GeometryType.PLANE || EntityType.VERTEX || QueryFilterCompound.ALLOWS_AXIS || EntityType.EDGE}
        cplaneDefinition.entities is Query;

        annotation {"Name" : "Plane type"}
        cplaneDefinition.cplaneType is CPlaneType;

        if(cplaneDefinition.cplaneType == CPlaneType.OFFSET)
        {
            annotation {"Name" : "Offset distance"}
            isLength(cplaneDefinition.offset, PLANE_OFFSET_BOUNDS);

            annotation {"Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION"}
            cplaneDefinition.oppositeDirection is boolean;
        }

        if(cplaneDefinition.cplaneType == CPlaneType.LINE_ANGLE)
        {
            annotation {"Name" : "Angle"}
            isAngle(cplaneDefinition.angle, ANGLE_360_BOUNDS);
        }

        if(cplaneDefinition.cplaneType == CPlaneType.MID_PLANE)
        {
            annotation {"Name" : "Flip alignment"}
            cplaneDefinition.flipAlignment is boolean;
        }

        annotation {"Name" : "Display size", "UIHint" : "ALWAYS_HIDDEN"}
        isLength(cplaneDefinition.size, PLANE_SIZE_BOUNDS);
    }
    //============================ Body =============================
    {
        var entities = evaluateQuery(context, cplaneDefinition.entities);
        var numEntities = @size(entities);

        if(cplaneDefinition.cplaneType == CPlaneType.OFFSET)
        {
            if(numEntities < 1) {
                reportFeatureError(context, id, requiresPlaneToOffsetMessage, ["entities"]);
                return;
            }
            if(numEntities > 1) {
                reportFeatureError(context, id, tooManyEntitiesMessage, ["entities"]);
                return;
            }
            var planeResult = evPlane(context, { "face" : cplaneDefinition.entities });
            if(reportFeatureError(context, id, planeResult.error, ["entities"]))
                return;
            cplaneDefinition.plane = planeResult.result;
            if(cplaneDefinition.oppositeDirection)
                cplaneDefinition.offset = -cplaneDefinition.offset;
            cplaneDefinition.plane.origin += cplaneDefinition.plane.normal * cplaneDefinition.offset;
        }

        if(cplaneDefinition.cplaneType == CPlaneType.PLANE_POINT)
        {
            if(numEntities < 2) {
                reportFeatureError(context, id, requiresPointPlaneMessage, ["entities"]);
                return;
            }
            if(numEntities > 2) {
                reportFeatureError(context, id, tooManyEntitiesMessage, ["entities"]);
                return;
            }
            var planeResult = evPlane(context, { "face" : qEntityFilter(cplaneDefinition.entities, EntityType.FACE) });
            if(reportFeatureError(context, id, planeResult.error, ["entities"]))
                return;
            var pointResult = evVertexPoint(context, { "vertex" : qEntityFilter(cplaneDefinition.entities, EntityType.VERTEX) });
            if (reportFeatureError(context, id, pointResult.error, ["entities"]))
                return;
            cplaneDefinition.plane = planeResult.result;
            cplaneDefinition.plane.origin = pointResult.result;
        }

        if(cplaneDefinition.cplaneType == CPlaneType.LINE_ANGLE)
        {
            if(numEntities < 1) {
                reportFeatureError(context, id, requiresLineMessage, ["entities"]);
                return;
            }
            if(numEntities > 1) {
                reportFeatureError(context, id, tooManyEntitiesMessage, ["entities"]);
                return;
            }
            var lineResult = evAxis(context, { "axis" : cplaneDefinition.entities });
            if(reportFeatureError(context, id, lineResult.error, ["entities"]))
                return;
            var normal = perpendicularVector(lineResult.result.direction);
            normal = rotationMatrix3d(lineResult.result.direction, cplaneDefinition.angle) *  normal;
            cplaneDefinition.plane = plane(lineResult.result.origin, normal, lineResult.result.direction);
        }

        if(cplaneDefinition.cplaneType == CPlaneType.LINE_POINT)
        {
            if(numEntities < 2) {
                reportFeatureError(context, id, requiresLinePointMessage, ["entities"]);
                return;
            }
            if(numEntities > 2) {
                reportFeatureError(context, id, tooManyEntitiesMessage, ["entities"]);
                return;
            }
            var lineResult = evAxis(context, { "axis" : qUnion([qEntityFilter(cplaneDefinition.entities, EntityType.EDGE),
                                                                qEntityFilter(cplaneDefinition.entities, EntityType.FACE)]) });
            if(reportFeatureError(context, id, lineResult.error, ["entities"]))
                return;
            var pointResult = evVertexPoint(context, { "vertex" : qEntityFilter(cplaneDefinition.entities, EntityType.VERTEX) });
            if(reportFeatureError(context, id, pointResult.error, ["entities"]))
               return;
            cplaneDefinition.plane = plane(pointResult.result, lineResult.result.direction);
        }

        if(cplaneDefinition.cplaneType == CPlaneType.THREE_POINT)
        {
            var vertexQueries = evaluateQuery(context, qEntityFilter(cplaneDefinition.entities, EntityType.VERTEX));
            if(@size(vertexQueries) < 3)
            {
                reportFeatureError(context, id, requiresThreePointsMessage, ["entities"]);
                return;
            }
            if(numEntities > 3) {
                reportFeatureError(context, id, tooManyEntitiesMessage, ["entities"]);
                return;
            }
            var points = makeArray(3);
            for(var i = 0; i < 3; i += 1)
            {
                var pointResult = evVertexPoint(context, { "vertex" : vertexQueries[i] });
                if(reportFeatureError(context, id, pointResult.error, ["entities"]))
                    return;
                points[i] = pointResult.result;
            }
            var normal = crossProduct(points[2] - points[0], points[1] - points[0]);
            if(norm(normal).value < TOLERANCE.zeroLength)
            {
                reportFeatureError(context, id, degeneratePointsMessage, ["entities"]);
                return;
            }
            cplaneDefinition.plane = plane(points[0], normalize(normal), normalize(points[1] - points[0]));
        }

        if(cplaneDefinition.cplaneType == CPlaneType.MID_PLANE)
        {
            // attempt from two points
            var vertexQueries = evaluateQuery(context, qEntityFilter(cplaneDefinition.entities, EntityType.VERTEX));
            if(@size(vertexQueries) == 2)
            {
                // Check for extra entities, not vertices
                if(numEntities > 2) {
                    reportFeatureError(context, id, tooManyEntitiesMessage, ["entities"]);
                    return;
                }
                createMidPlaneFromTwoPoints(context, id, vertexQueries, cplaneDefinition.size);
                return;
            }

            // attempt from a edge
            var edgeQueries = evaluateQuery(context, qEntityFilter(cplaneDefinition.entities, EntityType.EDGE));
            if(@size(edgeQueries) == 1)
            {
                // Check for extra entities, not edges
                if(numEntities > 1) {
                    reportFeatureError(context, id, tooManyEntitiesMessage, ["entities"]);
                    return;
                }
                createMidPlaneFromEdge(context, id, edgeQueries, cplaneDefinition.size);
                return;
            }

            // attempt from 2 planes
            var faceQueries = evaluateQuery(context, qEntityFilter(cplaneDefinition.entities, EntityType.FACE));
            if(@size(faceQueries) == 2)
            {
                // Check for extra entities, not faces
                if(numEntities > 2) {
                    reportFeatureError(context, id, tooManyEntitiesMessage, ["entities"]);
                    return;
                }
                createMidPlaneFromTwoPlanes(context, id, cplaneDefinition);
                return;
            }

            // fall through to failure
            reportFeatureError(context, id, midPlaneDefaultErrorMessage, ["entities"]);
            return;
        }

        if(cplaneDefinition.cplaneType == CPlaneType.CURVE_POINT)
        {
            if(numEntities < 2) {
                reportFeatureError(context, id, requiresCurvePointMessage, ["entities"]);
                return;
            }
            if(numEntities > 2) {
                reportFeatureError(context, id, tooManyEntitiesMessage, ["entities"]);
                return;
            }
            var param = evProjectPointOnCurve(context, { "edge" : qEntityFilter(cplaneDefinition.entities, EntityType.EDGE),
                                                       "vertex" : qEntityFilter(cplaneDefinition.entities, EntityType.VERTEX) });
            if ( param.error == "CANNOT_RESOLVE_ENTITIES")
            {
                reportFeatureError(context, id, requiresCurvePointMessage, ["entities"]);
                return;
            }
            else if (reportFeatureError(context, id, param.error, ["entities"]))
                return;

            var lineResult = evEdgeTangentLine(context, {"edge" : qEntityFilter(cplaneDefinition.entities, EntityType.EDGE),
                                        "parameter" : param.result, "arcLengthParameterization" : false });
            if(reportFeatureError(context, id, lineResult.error, ["entities"]))
                return;

            cplaneDefinition.plane = plane(lineResult.result.origin, lineResult.result.direction);
        }

        opPlane(context, id, cplaneDefinition);
    }, { oppositeDirection : false, flipAlignment : false });

function createMidPlaneFromTwoPoints(context is Context, id is Id, vertexQueries is array, size is ValueWithUnits) returns boolean
{
    var points = makeArray(2);
    for(var i = 0; i < 2; i += 1)
    {
        var pointResult = evVertexPoint(context, { "vertex" : vertexQueries[i] });
        if(reportFeatureError(context, id, pointResult.error, ["entities"]))
            return false;
        points[i] = pointResult.result;
    }
    var normal = points[1] - points[0];
    if (norm(normal) > TOLERANCE.zeroLength * meter)
    {
        normal = normalize(normal);
        var midOrigin = 0.5 * (points[0] + points[1]);
        opPlane(context, id, { "plane" : plane(midOrigin, normal), "size" : size });
        return true;
    }
    else
    {
        reportFeatureError(context, id, coincidentPointsMessage, ["entities"]);
        return false;
    }
}

function createMidPlaneFromEdge(context is Context, id is Id, edgeQueries is array, size is ValueWithUnits) returns boolean
{
    var points = makeArray(2);
    var edgeEndPoints = evEdgeTangentLines(context, { "edge" : edgeQueries[0], "parameters" : [0, 1] });
    points[0] = edgeEndPoints.result[0].origin;
    points[1] = edgeEndPoints.result[1].origin;
    var normal = points[1] - points[0];
    if (norm(normal) > TOLERANCE.zeroLength * meter)
    {
        normal = normalize(normal);
        var midOrigin = 0.5 * (points[0] + points[1]);
        opPlane(context, id, { "plane" : plane(midOrigin, normal), "size" : size });
        return true;
    }
    else
    {
        reportFeatureError(context, id, edgeIsClosedLoopMessage, ["entities"]);
        return false;
    }

    return false;
}

function createMidPlaneFromTwoPlanes(context is Context, id is Id, cplaneDefinition is map) returns boolean
{
    var plane1Result = evPlane(context, { "face" : qNthElement(cplaneDefinition.entities, 0) });
    if(reportFeatureError(context, id, plane1Result.error, ["entities"]))
        return false;
    var plane2Result = evPlane(context, { "face" : qNthElement(cplaneDefinition.entities, 1) });
    if(reportFeatureError(context, id, plane2Result.error, ["entities"]))
        return false;
    var p1 = plane1Result.result;
    var p2 = plane2Result.result;

    // By default, we want a plane to be the bisector of two adjacent plane of a solid, like bisecting a wedge of a cake.
    // Negate plane2's normal to get that. The other solution is a plane perpendicular to the bisecting plane
    if (!cplaneDefinition.flipAlignment)
        p2.normal = -p2.normal;

    var midOrigin = 0.5 * (p1.origin + p2.origin);
    var intersection = intersection(p1, p2);

    // Check for parallel case, when two planes are parallel there are no other solution we just take the normal of plane1
    if (intersection == undefined)
    {
        cplaneDefinition.plane = plane(midOrigin, p1.normal, p1.x);
    }
    else
    {
        var normal = normalize(p1.normal + p2.normal);
        var x = rotationMatrix3d(p1.normal, normal) * p1.x;
        cplaneDefinition.plane = plane(intersection.origin, normal, x);
        cplaneDefinition.plane.origin = project(cplaneDefinition.plane, midOrigin);
    }

    opPlane(context, id, cplaneDefinition);
    return true;
}

