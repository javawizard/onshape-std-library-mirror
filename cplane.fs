FeatureScript 2491; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2491.0");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "2491.0");

// Imports used internally
import(path : "onshape/std/box.fs", version : "2491.0");
import(path : "onshape/std/containers.fs", version : "2491.0");
import(path : "onshape/std/evaluate.fs", version : "2491.0");
import(path : "onshape/std/feature.fs", version : "2491.0");
import(path : "onshape/std/mathUtils.fs", version : "2491.0");
import(path : "onshape/std/curveGeometry.fs", version : "2491.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2491.0");
import(path : "onshape/std/valueBounds.fs", version : "2491.0");

/**
 * The method of defining a construction plane.
 */
export enum CPlaneType
{
    annotation { "Name" : "Offset" }
    OFFSET,
    annotation { "Name" : "Plane point" }
    PLANE_POINT,
    annotation { "Name" : "Line angle" }
    LINE_ANGLE,
    annotation { "Name" : "Point normal" }
    LINE_POINT,
    annotation { "Name" : "Three point" }
    THREE_POINT,
    annotation { "Name" : "Mid plane" }
    MID_PLANE,
    annotation { "Name" : "Curve point" }
    CURVE_POINT,
    annotation { "Name" : "Tangent" }
    TANGENT_PLANE
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
const noSMInFlatReferences           = ErrorStringEnum.FLATTENED_SHEET_METAL_SKETCH_PROHIBTED;
const requiresTangentMessage         = ErrorStringEnum.CPLANE_TANGENT_INPUT;
const requiresTangentSelectMessage   = ErrorStringEnum.CPLANE_TANGENT_SELECT_REFERENCE;
const tangentInvalidPlaneMessage     = ErrorStringEnum.CPLANE_TANGENT_PLANE_INVALID;
const tangentInvalidPointMessage     = ErrorStringEnum.CPLANE_TANGENT_POINT_INVALID;

// Factor by which to extend default plane size
const PLANE_SIZE_EXTENSION_FACTOR = 0.2;

const PLANE_OFFSET_BOUNDS =
{
    (meter)      : [0.0, 0.025, 500],
    (centimeter) : 2.5,
    (millimeter) : 25,
    (inch)       : 1,
    (foot)       : 0.0833,
    (yard)       : 0.0277
} as LengthBoundSpec;

/**
 * Creates a construction plane feature by calling [opPlane].
 */
annotation { "Feature Type Name" : "Plane", "Manipulator Change Function" : "cplaneManipulatorChange", "UIHint" : UIHint.CONTROL_VISIBILITY, "Editing Logic Function" : "cPlaneLogic"}
export const cPlane = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities",
                    "Filter" : GeometryType.PLANE || EntityType.VERTEX || QueryFilterCompound.ALLOWS_AXIS || EntityType.EDGE || BodyType.MATE_CONNECTOR,
                    "UIHint" : UIHint.PREVENT_CREATING_NEW_MATE_CONNECTORS }
        definition.entities is Query;

        annotation { "Name" : "Plane type", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        definition.cplaneType is CPlaneType;

                if (definition.cplaneType == CPlaneType.OFFSET)
        {
            annotation { "Name" : "Offset distance" }
            isLength(definition.offset, PLANE_OFFSET_BOUNDS);
        }

        if (definition.cplaneType == CPlaneType.LINE_ANGLE)
        {
            annotation { "Name" : "Angle" }
            isAngle(definition.angle, ANGLE_360_ZERO_DEFAULT_BOUNDS);
        }

        if (definition.cplaneType == CPlaneType.OFFSET || definition.cplaneType == CPlaneType.LINE_ANGLE)
        {
            annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.oppositeDirection is boolean;
        }

        if (definition.cplaneType == CPlaneType.MID_PLANE || definition.cplaneType == CPlaneType.TANGENT_PLANE)
        {
            annotation { "Name" : "Flip alignment" }
            definition.flipAlignment is boolean;
        }

        annotation { "Name" : "Flip normal" }
        definition.flipNormal is boolean;

        annotation { "Name" : "Starting width", "UIHint" : UIHint.ALWAYS_HIDDEN }
        isLength(definition.width, PLANE_SIZE_BOUNDS);

        annotation { "Name" : "Starting height", "UIHint" : UIHint.ALWAYS_HIDDEN }
        isLength(definition.height, PLANE_SIZE_BOUNDS);
    }
    //============================ Body =============================
    {
        verifyNoMesh(context, definition, "entities");

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V858_SM_FLAT_BUG_FIXES))
        {
            verifyNoSheetMetalFlatQuery(context, definition.entities, "entities", noSMInFlatReferences);
        }
        if (definition.cplaneType == CPlaneType.LINE_ANGLE)
            definition.angle = adjustAngle(context, definition.angle);

        const entities = evaluateQuery(context, definition.entities);
        const numEntities = @size(entities);

        var remainingTransform = getRemainderPatternTransform(context,
            {"references" : definition.entities});

        var planeBounds = new box(undefined);
        var directionSign = 1;
        if (definition.oppositeDirection)
            directionSign = -1;

        if (definition.cplaneType == CPlaneType.OFFSET)
        {
            if (numEntities < 1)
                throw regenError(requiresPlaneToOffsetMessage, ["entities"]);
            if (numEntities > 1)
                throw regenError(tooManyEntitiesMessage, ["entities"]);
            var referencePlane = evPlane(context, { "face" : definition.entities });
            definition.plane = referencePlane;
            definition.offset = definition.offset * directionSign;
            definition.plane.origin += definition.plane.normal * definition.offset;
            addOffsetManipulator(context, id, definition, referencePlane.origin);
        }

        if (definition.cplaneType == CPlaneType.PLANE_POINT)
        {
            if (numEntities < 2)
                throw regenError(requiresPointPlaneMessage, ["entities"]);
            if (numEntities > 2)
                throw regenError(tooManyEntitiesMessage, ["entities"]);

            var planes = qEntityFilter(definition.entities, EntityType.FACE);
            var vertices = qEntityFilter(definition.entities, EntityType.VERTEX);

            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1004_MATE_CONNECTOR_AS_PLANE))
            {
                const mateConnectors = qBodyType(definition.entities, BodyType.MATE_CONNECTOR);
                planes = qUnion([planes, mateConnectors]);
                vertices = qSubtraction(vertices, mateConnectors);
            }

            definition.plane = evPlane(context, { "face" : planes });
            definition.plane.origin = evVertexPoint(context, { "vertex" : vertices });
        }

        if (definition.cplaneType == CPlaneType.LINE_ANGLE)
        {
            definition.plane = lineAnglePlane(context, id, definition, entities, definition.angle * directionSign, planeBounds);
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
            var vertices = qEntityFilter(definition.entities, EntityType.VERTEX);
            var edges = qEntityFilter(definition.entities, EntityType.EDGE);
            var planes = qEntityFilter(definition.entities, EntityType.FACE);

            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1004_MATE_CONNECTOR_AS_PLANE))
            {
                const mateConnectors = qBodyType(definition.entities, BodyType.MATE_CONNECTOR);
                vertices = qSubtraction(vertices, mateConnectors);
                planes = qUnion([planes, mateConnectors]);
            }

            const vertexQueries = evaluateQuery(context, vertices);
            const edgeQueries = evaluateQuery(context, edges);
            const planeQueries = evaluateQuery(context, planes);

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
            else if (@size(planeQueries) == 2)

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
                                    "extendSide1" : true,
                                    "arcLengthParameterization" : false
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

        if (definition.cplaneType == CPlaneType.TANGENT_PLANE)
        {
            // Check the number of entities
            if (numEntities == 0)
            {
                throw regenError(requiresTangentMessage, ["entities"]);
            }
            if (numEntities > 2)
            {
                throw regenError(tooManyEntitiesMessage, ["entities"]);
            }

            // Get the reference surface (assumed to be a cylinder)
            var refSurface = qGeometry(definition.entities, GeometryType.CYLINDER);
            if (@size(evaluateQuery(context, refSurface)) != 1)
            {
                throw regenError(requiresTangentMessage, ["entities"]);
            }

            // Initialize parameters and flags
            const cyl is Cylinder = evSurfaceDefinition(context, {
                "face" : refSurface
            });
            var flipAlignment = definition.flipAlignment;
            var planeNormal = perpendicularVector(cyl.coordSystem.zAxis);

            if (numEntities == 1)
            {
                // Show blue bubble message that an additional entity is required
                reportFeatureInfo(context, id, requiresTangentSelectMessage);
            }
            else
            {
                // Get the reference entity (either a vertex or a plane)
                const refentity = qUnion([qEntityFilter(definition.entities, EntityType.VERTEX), qGeometry(definition.entities, GeometryType.PLANE)]);
                if (@size(evaluateQuery(context, refentity)) != 1)
                {
                    throw regenError(requiresTangentMessage, ["entities"]);
                }

                // Check if the reference entity is a plane or a mate connector
                // Consider Mate Connector as a Plane
                 if (@size(evaluateQuery(context, qGeometry(refentity, GeometryType.PLANE))) == 1 ||
                    @size(evaluateQuery(context, qBodyType(refentity, BodyType.MATE_CONNECTOR))) == 1)
                {
                    planeNormal = evPlane(context, {
                        "face" : refentity
                    }).normal;
                }
                else
                {
                    // Reference entity is a vertex
                    // Calculate the rotation angle to adjust the reference entity
                    var point = evVertexPoint(context, {
                            "vertex" : refentity
                    });

                    point = project(plane(cyl.coordSystem), point) - cyl.coordSystem.origin;
                    const len = norm(point);
                    if (len < cyl.radius - TOLERANCE.zeroLength * meter)
                    {
                       throw regenError(tangentInvalidPointMessage, ["entities"]);
                    }

                    var angle = 0 * radian;
                    if (len > cyl.radius + TOLERANCE.zeroLength * meter)
                    {
                        angle = acos(cyl.radius / len);
                    }

                    if (flipAlignment)
                    {
                        // Adjust the angle if alignment is flipped,
                        // to place a tangent plane on the another side of the Cylinder
                        angle = 360 * degree - angle;
                        flipAlignment = false;
                    }
                    planeNormal = rotationMatrix3d(cyl.coordSystem.zAxis, angle) * (point / len);
                }

                if (!perpendicularVectors(cyl.coordSystem.zAxis, planeNormal))
                {
                    throw regenError(tangentInvalidPlaneMessage, ["entities"]);
                }
            }

            const planeOrigin = cyl.coordSystem.origin + cyl.radius * planeNormal;
            definition.plane = plane(planeOrigin, planeNormal, cyl.coordSystem.zAxis);

            if (flipAlignment)
            {
                // Adjust the plane origin if alignment is flipped
                // To place a tangent plane on the opposite side of the Cylinder
                definition.plane.origin = definition.plane.origin - definition.plane.normal * cyl.radius * 2;
                definition.plane.normal *= -1;
            }
        }

        if (definition.flipNormal)
        {
            definition.plane.normal *= -1;
        }

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V316_PLANE_DEFAULT_SIZE_FIX))
        {
            var planeSize = planeBounds[] == undefined ? getPlaneDefaultSize(context, definition) : planeBounds[];
            definition.width = planeSize[0];
            definition.height = planeSize[1];
        }
        opPlane(context, id, definition);
        transformResultIfNecessary(context, id, remainingTransform);
    }, { oppositeDirection : false, flipAlignment : false, flipNormal : false, width : 1 * meter, height : 1 * meter });

function lineAnglePlane(context is Context, id is Id, definition is map, entities is array, angle is ValueWithUnits, planeBounds is box) returns Plane
{
    if (size(entities) == 1)
    {
        reportFeatureInfo(context, id, requiresLineAngleSelectMessage);
        const lineResult = evAxis(context, { "axis" : entities[0] });
        definition.axis = lineResult;

        var baseNormal = perpendicularVector(lineResult.direction);
        var normal = rotationMatrix3d(lineResult.direction, angle) * baseNormal;

        addAngleManipulator(context, id, definition, angle, cross(baseNormal, lineResult.direction) * meter, planeBounds);
        return plane(lineResult.origin, normal, lineResult.direction);
    }

    if (size(entities) < 2)
        throw regenError(requiresLineAxisMessage, ["entities"]);
    if (size(entities) > 2)
        throw regenError(tooManyEntitiesMessage, ["entities"]);
    var axis1 = try silent(evAxis(context, { "axis" : entities[0] }));
    var axis2 = try silent(evAxis(context, { "axis" : entities[1] }));

    if (axis1 == undefined) // If the plane or point is selected first, swap.
    {
        if (axis2 == undefined)
            throw regenError(requiresLineAxisMessage, ["entities"]);
        axis1 = axis2;
        axis2 = undefined;
        entities[1] = entities[0];
    }

    var secondInPlaneDirection;
    definition.axis = axis1;

    // The second entity can be an axis, a plane, or a point
    if (axis2 != undefined)
    {
        if (parallelVectors(axis1.direction, axis2.direction))
            secondInPlaneDirection = axis2.origin - axis1.origin;
        else
            secondInPlaneDirection = axis2.direction * meter;
    }
    else
    {
        var plane = try silent(evPlane(context, { "face" : entities[1] }));
        if (plane != undefined)
        {
            secondInPlaneDirection = cross(axis1.direction, plane.normal) * meter;
        }
        else
        {
            var point = try silent(evVertexPoint(context, { "vertex" : entities[1] }));
            if (point != undefined)
                secondInPlaneDirection = point - axis1.origin;
        }
    }

    // Above should be parsed by parseEntities
    if (secondInPlaneDirection == undefined)
        throw regenError(requiresLineAxisMessage, ["entities"]);

    var normal = cross(axis1.direction, secondInPlaneDirection);
    if (stripUnits(squaredNorm(normal)) < TOLERANCE.zeroLength * TOLERANCE.zeroLength)
        throw regenError(degenerateSelectionMessage, ["entities"]);

    normal = rotationMatrix3d(axis1.direction, angle) * normal;

    addAngleManipulator(context, id, definition, angle, secondInPlaneDirection, planeBounds);
    return plane(axis1.origin, normal, axis1.direction);
}

function getPlaneDefaultSize(context is Context, definition is map) returns array
{
    var planeType = definition.cplaneType;
    var planeBounds = [definition.width, definition.height];
    if (planeType == CPlaneType.OFFSET)
    {
        const filterFaces = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1004_MATE_CONNECTOR_AS_PLANE);
        // If after V1004, and this offset plane is not based on a face, return the default plane bounds
        if (filterFaces && isQueryEmpty(context, qEntityFilter(definition.entities, EntityType.FACE)))
        {
            return planeBounds;
        }

        const cSys = planeToCSys(definition.plane);
        var bounds = evBox3d(context, { 'topology' : definition.entities, 'cSys' : cSys, 'tight' : false });

        var isConstruction = false;
        var constructionFilteredEntities = evaluateQuery(context, qConstructionFilter(definition.entities, ConstructionObject.NO));
        if (@size(constructionFilteredEntities) == 0)
        {
            isConstruction = true;
        }

        planeBounds = getExpandedPlaneBounds(bounds, isConstruction);
    }
    else if (planeType == CPlaneType.LINE_ANGLE)
    {
        const edgeQueries = evaluateQuery(context, qEntityFilter(definition.entities, EntityType.EDGE));
        if (edgeQueries != undefined && @size(edgeQueries) > 0)
        {
            var curveDefinition = evCurveDefinition(context, { "edge" : edgeQueries[0], "returnBSplinesAsOther" : true });
            var size;
            if (curveDefinition is Circle)
                size = curveDefinition.radius * 2;
            else
                size = evLength(context, { "entities" : edgeQueries[0] });

            size *= 1 + PLANE_SIZE_EXTENSION_FACTOR;
            planeBounds = [size, size];
        }
    }
    else if (planeType == CPlaneType.PLANE_POINT)
    {
        const filterFaces = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1004_MATE_CONNECTOR_AS_PLANE);
        // If after V1004, and this point-plane is not based on a face, return the default plane bounds
        if (filterFaces && isQueryEmpty(context, qEntityFilter(definition.entities, EntityType.FACE)))
        {
            return planeBounds;
        }

        const cSys = planeToCSys(definition.plane);
        var bounds = evBox3d(context, { 'topology' :  qEntityFilter(definition.entities, EntityType.FACE), 'cSys' : cSys, 'tight' : false });

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
        var faceFilteredEntities = evaluateQuery(context, qEntityFilter(definition.entities, EntityType.FACE));
        // If this mid plane is not based on a face, return the default plane bounds
        if (@size(faceFilteredEntities) == 0)
        {
            return planeBounds;
        }

        const cSys = planeToCSys(definition.plane);
        var bounds = evBox3d(context, { 'topology' : qEntityFilter(definition.entities, EntityType.FACE), 'cSys' : cSys, 'tight' : false });

        if (bounds == undefined)
        {
            return planeBounds;
        }

        var isConstruction = false;
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

// Manipulator functions

const DEPTH_MANIPULATOR = "depthManipulator";
const ROTATE_MANIPULATOR = "rotateManipulator";

function addOffsetManipulator(context is Context, id is Id, definition is map, referencePoint is Vector)
{

    addManipulators(context, id, {
                (DEPTH_MANIPULATOR) : linearManipulator({
                            "base" : referencePoint,
                            "direction" : definition.plane.normal,
                            "offset" : definition.offset,
                            "primaryParameterId" : "offset"
                        })
            });
}

function addAngleManipulator(context is Context, id is Id, definition is map, angle is ValueWithUnits, revolvePoint is Vector, planeBounds is box)
{
    planeBounds[] = getPlaneDefaultSize(context, definition);
    const size = planeBounds[][0];
    const minValue = -2 * PI * radian;
    const maxValue = 2 * PI * radian;
    const axisOrigin = definition.axis.origin;
    const axisDirection = definition.axis.direction;

    revolvePoint = project(plane(axisOrigin, axisDirection), revolvePoint); // Project revolvePoint to be on the plane normal to the rotation axis
    revolvePoint = project(plane(axisOrigin, cross(revolvePoint, axisDirection)), revolvePoint); // Project revolvePoint onto the plane at 0-degrees
    revolvePoint = axisOrigin + normalize(revolvePoint - axisOrigin) * size / 4; // Scale manipulator to reach halfway across the plane

    addManipulators(context, id, {
                (ROTATE_MANIPULATOR) : angularManipulator({
                            "axisOrigin" : axisOrigin,
                            "axisDirection" : axisDirection,
                            "rotationOrigin" : revolvePoint,
                            "angle": angle,
                            "minValue": minValue,
                            "maxValue": maxValue,
                            "primaryParameterId" : "angle"
                        })
            });
}

/**
 * @internal
 * Manipulator change function for `cPlane`.
 */
export function cplaneManipulatorChange(context is Context, cplaneDefinition is map, newManipulators is map) returns map
{
    var newValue = 0 * meter;
    if (newManipulators[DEPTH_MANIPULATOR] is map && cplaneDefinition.cplaneType == CPlaneType.OFFSET)
    {
        newValue = newManipulators[DEPTH_MANIPULATOR].offset;
        cplaneDefinition.offset = abs(newValue);
    }
    else if (newManipulators[ROTATE_MANIPULATOR] is map && cplaneDefinition.cplaneType == CPlaneType.LINE_ANGLE)
    {
        newValue = newManipulators[ROTATE_MANIPULATOR].angle;
        cplaneDefinition.angle = abs(newValue);
    }

    cplaneDefinition.oppositeDirection = newValue < 0;

    return cplaneDefinition;
}

/**
 * @internal
 * Heuristics to determine the type of plane to be constructed, based on user preselection.
 */
export function cPlaneLogic(context is Context, id is Id, oldDefinition is map, definition is map) returns map
{
    if (oldDefinition != {}) // Only do anything on preselection
        return definition;

    const entities = definition.entities;

    const mateConnectorQ is Query = qBodyType(entities, BodyType.MATE_CONNECTOR);

    const total is number = size(evaluateQuery(context, entities));
    const vertices is number = size(evaluateQuery(context, qSubtraction(qEntityFilter(entities, EntityType.VERTEX), mateConnectorQ)));
    const lines is number = size(evaluateQuery(context, qGeometry(entities, GeometryType.LINE)));
    const planes is number = size(evaluateQuery(context, qUnion([qGeometry(entities, GeometryType.PLANE), mateConnectorQ])));
    const curves is number = size(evaluateQuery(context, qSubtraction(qEntityFilter(entities, EntityType.EDGE),
                                                                      qGeometry(entities, GeometryType.LINE))));
    const mateConnectors is number = size(evaluateQuery(context, mateConnectorQ));
    const cylinders is number = size(evaluateQuery(context, qGeometry(entities, GeometryType.CYLINDER)));

    if (total == 1)
    {
        if (planes == 1)
            definition.cplaneType = CPlaneType.OFFSET;
        else if ((lines + curves) == 1 && size(evaluateQuery(context, qAdjacent(entities, AdjacencyType.VERTEX, EntityType.VERTEX))) == 2)
            definition.cplaneType = CPlaneType.MID_PLANE;
        else if (try silent(evAxis(context, { "axis" : entities })) != undefined)
            definition.cplaneType = CPlaneType.LINE_ANGLE;
    }
    else if (total == 2)
    {
        if (planes == 1 && vertices == 1)
            definition.cplaneType = CPlaneType.PLANE_POINT;
        else if (planes == 2 || vertices == 2)
            definition.cplaneType = CPlaneType.MID_PLANE;
        else if (curves == 1 && vertices == 1)
            definition.cplaneType = CPlaneType.CURVE_POINT;
        else if (cylinders == 1 && (vertices + planes) == 1)
            definition.cplaneType = CPlaneType.TANGENT_PLANE;
        else if (vertices == 1) // The other thing must be a plane or an axis
            definition.cplaneType = CPlaneType.LINE_POINT; // Point and normal
        else if (lines >= 1 && (lines + vertices + planes) == 2)
            definition.cplaneType = CPlaneType.LINE_ANGLE;
    }
    else if (total == 3)
    {
        if ((vertices + mateConnectors) == 3)
            definition.cplaneType = CPlaneType.THREE_POINT;
    }

    return definition;
}

