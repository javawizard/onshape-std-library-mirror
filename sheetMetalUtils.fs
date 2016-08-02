FeatureScript 392; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "392.0");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "392.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "392.0");
import(path : "onshape/std/containers.fs", version : "392.0");
import(path : "onshape/std/coordSystem.fs", version : "392.0");
import(path : "onshape/std/curveGeometry.fs", version : "392.0");
import(path : "onshape/std/evaluate.fs", version : "392.0");
import(path : "onshape/std/feature.fs", version : "392.0");
import(path : "onshape/std/math.fs", version : "392.0");
import(path : "onshape/std/manipulator.fs", version : "392.0");
import(path : "onshape/std/query.fs", version : "392.0");
import(path : "onshape/std/sketch.fs", version : "392.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "392.0");
import(path : "onshape/std/smobjecttype.gen.fs", version : "392.0");
import(path : "onshape/std/string.fs", version : "392.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "392.0");
import(path : "onshape/std/tool.fs", version : "392.0");
import(path : "onshape/std/valueBounds.fs", version : "392.0");
import(path : "onshape/std/vector.fs", version : "392.0");

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */


/**
 * @internal
 */
export enum ToleranceLevel
{
    annotation { "Name" : "Very tight" }
    VERY_TIGHT,
    annotation { "Name" : "Tight" }
    TIGHT,
    annotation { "Name" : "Medium" }
    MEDIUM,
    annotation { "Name" : "Loose" }
    LOOSE
}

/**
 * @internal
 */
export function getEdgeCSys(context is Context, edge is Query) returns CoordSystem
{
    var faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
    if (size(faces) != 2)
        throw regenError("Bad edge");
    var offsetData = findOffsetEdgeData(context, edge);

    var topPlane = evPlane(context, {
            "face" : offsetData.topFace
    });

    var sidePlane = evPlane(context, {
            "face" : offsetData.sideFace
    });

    var edgeLine = evEdgeTangentLine(context, {
            "edge" : edge,
            "parameter" : 0.5
    });

    var normal = cross(sidePlane.normal, edgeLine.direction);
    if (dot(normal, topPlane.normal) < 0)
    {
        edgeLine.direction = -edgeLine.direction;
    }
    return coordSystem(edgeLine.origin, edgeLine.direction, topPlane.normal);
}

/**
 * @internal
 */
// TODO BRT - Change this to use sheet metal attributes instead of geometry.
export function findOffsetEdgeData(context is Context, edge is Query) returns map
{
    const BIG_NUMBER = 1.0e20 * meter;
    var attr = getAttributes(context, {
            "entities" : qOwnerBody(edge)
    });
    if (size(attr) != 1 || attr[0].thickness == undefined || attr[0].thickness.value == undefined)
        throw regenError("Bad sheet metal attribute");
    var offsetDistance = attr[0].thickness.value;
    if (offsetDistance is number)
        offsetDistance *= meter;
    var edgeLine = evEdgeTangentLine(context, {
            "edge" : edge,
            "parameter" : 0.0
    });
    var edgeLength = evLength(context, {
            "entities" : edge
    });
    var minDistance = BIG_NUMBER;
    var offsetEdge;
    var faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
    if (size(faces) != 2)
        throw regenError("Could not find offset data");

    var topFace;
    var sideFace;
    if (hasSheetMetalAttribute(context, faces[0], SMObjectType.WALL))
    {
        topFace = faces[0];
        sideFace = faces[1];
    }
    else
    {
        topFace = faces[1];
        sideFace = faces[0];
    }
    var edges = evaluateQuery(context, qEdgeAdjacent(sideFace, EntityType.EDGE));
    for (var testEdge in edges)
    {
        if (testEdge == edge)
            continue;
        var testEdgeLine = evEdgeTangentLine(context, {
                "edge" : testEdge,
                "parameter" : 0.0
        });
        if (!tolerantParallel(edgeLine, testEdgeLine) || tolerantCoLinear(edgeLine, testEdgeLine))
            continue;
        var testEdgeLength = evLength(context, {
                "entities" : testEdge
        });
        if (abs(testEdgeLength - edgeLength) > TOLERANCE.zeroLength * meter)
            continue;
        var v = testEdgeLine.origin - edgeLine.origin;
        v = v - edgeLine.direction * dot(edgeLine.direction, v);
        var distance = norm(v);
        if (distance < minDistance)
        {
            minDistance = distance;
            offsetEdge = testEdge;
        }
    }

    if (minDistance == BIG_NUMBER)
        throw regenError("Could not find offset edge");

    return {
        "offsetEdge" : offsetEdge,
        "offsetDistance" : minDistance,
        "sideFace" : sideFace,
        "topFace" : topFace
    };
}

/**
 * @internal
 */
export function tolerantParallel(line0 is Line, line1 is Line) returns boolean
{
    return squaredNorm(cross(line0.direction, line1.direction)) < TOLERANCE.zeroAngle * TOLERANCE.zeroAngle;
}

/**
 * @internal
 */
export function tolerantCoLinear(line0 is Line, line1 is Line) returns boolean
{
    if (tolerantParallel(line0, line1)) {
        var v = line1.origin - line0.origin;
        v = v - line0.direction * dot(v, line0.direction);
        var lengthTolerance = TOLERANCE.zeroLength * meter;
        return squaredNorm(v) < lengthTolerance * lengthTolerance;
    }
    return false;
}

/**
 * @internal
 */
export function hasSheetMetalAttribute(context is Context, entities is Query, objectType is SMObjectType) returns boolean
{
    return size(getSmObjectTypeAttributes(context, entities, objectType)) != 0;
}

/**
 * @internal
 */
export function startSheetMetalFeature(context is Context, id is Id)
{
    @startSheetMetalFeature(context, id);
}


/**
 * @internal
 */
 export function endSheetMetalFeature(context is Context, id is Id, args is map)
 {
    @endSheetMetalFeature(context, id, args);
 }


/**
* @internal
* @param args {{
*       @field surfaceBodies{Query}
*       @field bendEdges{Query}
*       @field specialRadiiBends{array} : array of pairs "(edge, bendRadius)"
*       @field defaultRadius{ValueWithUnits} : bend radius to be applied to bendEdges
*       @field controllsThickness{boolean}
*       @field thickness{ValueWithUnits}
* }}
*/
export function annotateSmSurfaceBodies(context is Context, id is Id, args is map, objectCount is number) returns number
{
    var surfaceBodies = evaluateQuery(context, args.surfaceBodies);
    if (size(surfaceBodies) == 0)
    {
        return;
    }
    var featureIdString = toAttributeId(id);
    var thicknessData = {"value" : args.thickness, "canBeEdited" : args.controllsThickness};
    if (args.controllsThickness)
    {
        thicknessData.controllingFeatureId = featureIdString;
        thicknessData.parameterIdInFeature = "thickness";
    }
    var modelAttribute = asSMAttribute({"attributeId" : featureIdString,
                    "objectType" : SMObjectType.MODEL,
                    "active" : true,
                    "thickness" : thicknessData,
                    "defaultBendRadius" : {"value" : args.defaultRadius}});

    var facesQ =  qOwnedByBody(args.surfaceBodies, EntityType.FACE);
    var count = objectCount;
    for (var face in evaluateQuery(context, facesQ))
    {
        var surface = evSurfaceDefinition(context, {
                "face" : face
        });
        if (surface is Plane)
        {
            setAttribute(context, {
                    "entities" : face,
                    "attribute" : makeSMWallAttribute(toAttributeId(id + count))
            });
            count += 1;
        }
        else if (surface is Cylinder)
        {
            var bendAttribute = makeSMJointAttribute(toAttributeId(id + count));
            bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited": false };

            var bendRadius = surface.radius - 0.5 * args.thickness;
            bendAttribute.radius = { "value" : bendRadius, "canBeEdited" : false, "isDefault" : false};
            setAttribute(context, {
                    "entities" : face,
                    "attribute" : bendAttribute
            });
            count += 1;
        }
        else
        {
            regenError("Only planar walls are supported");
        }
    }
    var bendMap = {};
    for (var edge in evaluateQuery(context, args.bendEdges))
    {
        bendMap[edge] = true;
    }
    for (var edgeAndRadius in args.specialRadiiBends)
    {
        bendMap[edgeAndRadius[0]] = edgeAndRadius[1];
    }
    var edgesQ = qOwnedByBody(args.surfaceBodies, EntityType.EDGE);
    for (var edge in evaluateQuery(context, edgesQ))
    {
        var faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
        if (size(faces) != 2)
        {
            continue; // TODO : warning if edge is selected as a bend
        }
        if (size(getSmObjectTypeAttributes(context, qUnion(faces), SMObjectType.WALL)) != 2)
        {
            continue;
        }
        var bendRadius = bendMap[edge];
        if (bendRadius != undefined)
        {
            var bendAttribute = makeSMJointAttribute(toAttributeId(id + count));
            bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited": true };
            if (bendRadius == true)
            {
                bendRadius = args.defaultRadius;
            }
            bendAttribute.radius = { "value" : bendRadius, "canBeEdited" : true, "isDefault" : true};
            bendAttribute.angle = {"value" : edgeAngle(context, edge), "canBeEdited" : false};
            setAttribute(context, {
                    "entities" : edge,
                    "attribute" : bendAttribute
            });
            count += 1;
        }
        else
        {
            var ripAttribute = makeSMJointAttribute(toAttributeId(id + count));
            ripAttribute.jointType = { "value" : SMJointType.RIP, "canBeEdited": true };
            var angle = try(edgeAngle(context, edge));
            if (angle != undefined)
            {
                ripAttribute.angle = {"value" : angle, "canBeEdited" : false};
            }
            setAttribute(context, {
                    "entities" : edge,
                    "attribute" : ripAttribute
            });
        }
        count += 1;
    }
    for (var body in surfaceBodies)
    {
        setAttribute(context, {
                "entities" : body,
                "attribute" : modelAttribute
        });
    }
    var verticesQ = qOwnedByBody(args.surfaceBodies, EntityType.VERTEX);
    assignSmAssociationAttributes(context, qUnion([args.surfaceBodies, facesQ, edgesQ, verticesQ]));
    return count;
}

/**
 * @internal
 * For an edge between two planes computes angle between plane normals
 */
export function edgeAngle(context is Context, edge is Query) returns ValueWithUnits
{
    var faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
    if (size(faces) != 2)
    {
        throw "Expects 2-sided faces";
    }
    var plane0 = evPlane(context, {
            "face" : faces[0]
    });
    var plane1 = evPlane(context, {
            "face" : faces[1]
    });
    return angleBetween(plane0.normal, plane1.normal);
}

