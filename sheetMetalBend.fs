FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */


export import(path : "onshape/std/query.fs", version : "✨");

import(path : "onshape/std/boundingtype.gen.fs", version : "✨");
import(path : "onshape/std/coordSystem.fs", version : "✨");
import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");
import(path : "onshape/std/sketch.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/tool.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");

const STARTING_THICKNESS = 0.25 * inch;

const K_FACTOR_BOUNDS =
{
    (unitless) : [0.0, 0.45, 0.5]
} as RealBoundSpec;

/**
* @internal
*/
export enum SMDeformationForm
{
    annotation { "Name" : "K Factor" }
    KFACTOR,
    annotation { "Name" : "Deduction" }
    DEDUCTION,
    annotation { "Name" : "Allowance" }
    ALLLOWANCE
}

/**
* @internal
*  This feature bends a solid flat sheet starting at a break/bend edge
*/
annotation { "Feature Type Name" : "smBend" }
export const smBend = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Deformation form", "UIHint" : ["REMEMBER_PREVIOUS_VALUE"] }
        definition.deformationForm is SMDeformationForm;

        if (definition.deformationForm == SMDeformationForm.KFACTOR)
        {
            annotation { "Name" : "K Factor", "UIHint" : ["REMEMBER_PREVIOUS_VALUE"] }
            isReal(definition.kFactor, K_FACTOR_BOUNDS);
        }
        else if (definition.deformationForm == SMDeformationForm.DEDUCTION)
        {
            annotation { "Name" : "Deduction", "UIHint" : ["REMEMBER_PREVIOUS_VALUE"] }
            isLength(definition.deduction, NONNEGATIVE_LENGTH_BOUNDS);
        }
        else
        {
            annotation { "Name" : "Allowance", "UIHint" : ["REMEMBER_PREVIOUS_VALUE"] }
            isLength(definition.allowance, NONNEGATIVE_LENGTH_BOUNDS);
        }

        annotation { "Name" : "Radius", "UIHint" : ["REMEMBER_PREVIOUS_VALUE"] }
        isLength(definition.radius, NONNEGATIVE_LENGTH_BOUNDS);

        annotation { "Name" : "Angle", "UIHint" : ["REMEMBER_PREVIOUS_VALUE"] }
        isAngle(definition.angle, ANGLE_360_BOUNDS);

        annotation { "Name" : "Bend edges",
                    "Filter" : EntityType.EDGE && ConstructionObject.NO && GeometryType.LINE }
        definition.bendEdge is Query;

        annotation { "Name" : "Seed entity",
                    "Filter" : SketchObject.NO && ConstructionObject.NO && BodyType.SOLID }
        definition.seedEntity is Query;
    }
    {
        definition.bendOptions = {
            "useKFactor" : definition.useKFactor,
            "kFactor" : definition.kFactor,
            "deduction" : definition.deduction
            };
        var bendFaceData = findStartEdgeSplitInfo(context, definition);
        const thickness = bendFaceData.thickness;

        var seedBox = evBox3d(context, { "topology" : definition.seedEntity });
        var center = (seedBox.minCorner + seedBox.maxCorner) * 0.5;

        var tanFace = bendFaceData.bendFace;

        var faceTangent = evFaceTangentPlane(context, { "face" : tanFace, "parameter" : [0.5, 0.5] as Vector });

        const growTol = 1 * meter;

        opExtrude(context, id + "cut1", {
                    "bodyType" : ToolBodyType.SURFACE,
                    "direction" : faceTangent.normal,
                    "entities" : definition.bendEdge,
                    "startBound" : BoundingType.BLIND,
                    "endBound" : BoundingType.BLIND,
                    "startDepth" : thickness + growTol,
                    "endDepth" : growTol
                });

        opExtendSheetBody(context, id + "extend1", { "entities" : qCreatedBy(id + "cut1", EntityType.BODY), "distance" : growTol });
        var changedFaces = startTracking(context, bendFaceData.splitFaces);
        opSplitFace(context, id + "split1", { "faceTargets" : bendFaceData.splitFaces, "bodyTools" : qCreatedBy(id + "cut1", EntityType.BODY) });

        var bendFaceData2 = findFinishEdgeBendSplitInfo(context, bendFaceData.cuttingPlane, definition);

        var faceTangent2 = evFaceTangentPlane(context, { "face" : qCreatedBy(id + "cut1", EntityType.FACE), "parameter" : [0.5, 0.5] as Vector });
        var v = center - faceTangent2.origin;
        var sign = dot(v, faceTangent2.normal) < 0 ? -1 : 1;

        var allowance = calculateAllowance(definition, thickness);
        opOffsetFace(context, id + "offset", {
                    "moveFaces" : qCreatedBy(id + "cut1", EntityType.FACE),
                    "offsetDistance" : sign * allowance });

        opSplitFace(context, id + "split2", { "faceTargets" : bendFaceData2.splitFaces, "bodyTools" : qCreatedBy(id + "cut1", EntityType.BODY) });

        opDeleteBodies(context, id + "delete_cuts", { "entities" : qCreatedBy(id + "cut1", EntityType.BODY) });

        opBendAtEdgeInternal(context, id + "bend", definition);

        var bendFaces = qGeometry(changedFaces, GeometryType.CYLINDER);
        setBendAttributes(context, id, bendFaces, faceTangent, definition);
    }, { "pickEdges" : false, "useKFactor" : true, "kFactor" : 0.45 });

function setBendAttributes(context is Context, id is Id, bendFaceQuery is Query, topFacePlane is Plane, definition is map)
{
    var attributeId = toAttributeId(id);
    var wallAttrib = makeSMWallAttribute(attributeId);
    var bendAttribute = makeSMJointAttribute(attributeId);
    bendAttribute.jointType = { "value" : "BEND", "canBeEdited": true };
    bendAttribute.radius = {
        "value" : definition.radius,
        "controllingFeatureId" : toAttributeId(id),
        "parameterIdInFeature" : "radius",
        "canBeEdited" : true
    };
    clearSmAttributes(context, bendFaceQuery);
    var bendFaces = evaluateQuery(context, bendFaceQuery);
    for (var bendFace in bendFaces)
    {
        var cylinder = evSurfaceDefinition(context, {
            "face" : bendFace
        });
        if (cylinder is Cylinder)
        {
            setAttribute(context, {
                "entities" : bendFace,
                "attribute" : bendAttribute
            });
            var adjLinearEdges = qGeometry(qEdgeAdjacent(bendFace, EntityType.EDGE), GeometryType.LINE);
            var adjPlanarFaces = evaluateQuery(context, qGeometry(qEdgeAdjacent(adjLinearEdges, EntityType.FACE), GeometryType.PLANE));
            for (var wallFace in adjPlanarFaces)
            {
                var plane = evPlane(context, {
                    "face" : wallFace
                });
                if (squaredNorm(cross(plane.normal, topFacePlane.normal)) > TOLERANCE.zeroLength * TOLERANCE.zeroLength)
                {
                    clearSmAttributes(context, wallFace);
                    setAttribute(context, {
                        "entities" : wallFace,
                        "attribute" : wallAttrib
                    });
                }
            }
        }
    }
}

function findStartEdgeSplitInfo(context is Context, definition is map) returns map
{
    var seedBox = evBox3d(context, { "topology" : definition.seedEntity });
    var center = (seedBox.minCorner + seedBox.maxCorner) * 0.5;

    var tangentFace = findSeedFace(context, center, definition);
    var faceTangent = evFaceTangentPlane(context, { "face" : tangentFace, "parameter" : [0.5, 0.5] as Vector });
    var edgeTangent = evEdgeTangentLine(context, {
            "edge" : definition.bendEdge,
            "parameter" : 0.5
        });

    // Find seed vector in plane of tangentFace and perpendicular to the edge
    var origin = edgeTangent.origin;
    var normal = center - origin;
    normal = normalize(normal - edgeTangent.direction * dot(edgeTangent.direction, normal));
    normal = normalize(normal - faceTangent.normal * dot(faceTangent.normal, normal));

    const boundPlane = plane(origin, normal);

    var splitFaces = [];
    var testedFaces = {};
    var done = false;
    const maxIterations = 50;
    var count = 0;
    while (!done)
    {
        done = true;
        var adjFaceQuery = size(splitFaces) == 0 ? definition.seedEntity : qUnion(splitFaces);
        var faces = evaluateQuery(context, qEdgeAdjacent(adjFaceQuery, EntityType.FACE));
        for (var face in faces)
        {
            if (testedFaces[face] == true)
            {
                continue;
            }
            testedFaces[face] = true;
            var cSys = coordSystem(origin, edgeTangent.direction, normal);
            var bbox = evBox3d(context, {
                    "topology" : face,
                    "cSys" : cSys
                });
            if (bbox.maxCorner[2] > TOLERANCE.zeroLength * meter)
            {
                splitFaces = append(splitFaces, face);
                done = false;
            }
        }
        if (count >= maxIterations)
        {
            throw regenError("Cannot find starting split edge info");
        }
        count += 1;
    }

    var thickness = findThickness(context, tangentFace);

    return {
            "bendFace" : tangentFace,           // Face which will be bent
            "cuttingPlane" : boundPlane,        // plane to be used for next split operation
            "splitFaces" : qUnion(splitFaces),  // Faces to be split
            "thickness" : thickness             // thickness of the slab
        };
}

function calculateAllowance(definition is map, thickness is ValueWithUnits) returns ValueWithUnits
{
    var result;

    if (definition.deformationForm == SMDeformationForm.KFACTOR)
    {
        result = definition.angle / radian * (definition.radius + definition.bendOptions.kFactor * thickness);
    }
    else if (definition.deformationForm == SMDeformationForm.DEDUCTION)
    {
        result = 2 * (tan(definition.angle) + definition.radius + thickness) - definition.bendOptions.deduction;
    }
    else
    {
        result = definition.allowance;
    }
    return result;
}

function findThickness(context is Context, face is Query) returns ValueWithUnits
{
    var attr = getAttributes(context, {
            "entities" : qOwnerBody(face)
    });
    if (size(attr) != 1 || attr[0].thickness == undefined || attr[0].thickness.value == undefined)
        throw regenError("Bad sheet metal attribute");
    var thickness = attr[0].thickness.value;
    if (thickness is number)
        thickness *= meter;
    return thickness;
}

function findFinishEdgeBendSplitInfo(context is Context, cuttingPlane is Plane, definition is map) returns map
{
    var splitFaces = [];
    var testedFaces = {};
    var done = false;
    var seed = definition.seedEntity;
    var count = 0;
    const lengthTolerance = TOLERANCE.zeroLength * meter;
    while (!done)
    {
        done = true;
        var nextSeed = [];
        var faces = evaluateQuery(context, qEdgeAdjacent(seed, EntityType.FACE));
        for (var face in faces)
        {
            if (testedFaces[face] == true)
            {
                continue;
            }
            testedFaces[face] = true;
            var bbox = evBox3d(context, {
                    "topology" : face,
                    "cSys" : coordSystem(cuttingPlane.origin, cuttingPlane.x, cuttingPlane.normal)
                });
            if (bbox.minCorner[2] > -lengthTolerance && (bbox.maxCorner[2] - bbox.minCorner[2]) > lengthTolerance)
            {
                splitFaces = append(splitFaces, face);
                nextSeed = append(nextSeed, face);
                done = false;
            }
        }
        seed = qUnion(nextSeed);
        if (count > 100)
        {
            break;
        }
        count += 1;
    }

    return {
            "splitFaces" : qUnion(splitFaces)
        };
}

function findSeedFace(context is Context, center is Vector, definition is map) returns Query
{
    var body = qOwnerBody(definition.seedEntity);
    var edgeBody = qOwnerBody(definition.bendEdge);
    const evaluatedSeedBody = evaluateQuery(context, body);
    const evaluatedEdgeBody = evaluateQuery(context, edgeBody);
    if (size(evaluatedSeedBody) > 0 && size(evaluatedEdgeBody) > 0 && evaluatedSeedBody[0].transientId == evaluatedEdgeBody[0].transientId)
    {
        return qNthElement(qEdgeAdjacent(definition.bendEdge, EntityType.FACE), 0);
    }

    var edgeTangent = evEdgeTangentLine(context, {
            "edge" : definition.bendEdge,
            "parameter" : 0.5
        });
    var seedVector = center - edgeTangent.origin;
    seedVector = normalize(seedVector - edgeTangent.direction * dot(edgeTangent.direction, seedVector));
    var faces = evaluateQuery(context, qOwnedByBody(body, EntityType.FACE));
    var tangentFace;
    var dMin = 1.0e20 * meter;
    for (var face in faces)
    {
        var plane = evSurfaceDefinition(context, {
                "face" : face
            });
        if (plane is Plane && abs(dot(edgeTangent.direction, plane.normal)) < TOLERANCE.zeroLength && abs(dot(plane.normal, seedVector)) < 0.25)
        {
            var distResult = evDistance(context, {
                    "side0" : definition.bendEdge,
                    "side1" : face
                });

            var distance = distResult.distance;
            if (distance < dMin)
            {
                dMin = distance;
                tangentFace = face;
            }
        }
    }
    if (tangentFace == undefined)
    {
        throw regenError("Unable to find seed face for selected edge.");
    }
    return tangentFace;
}

