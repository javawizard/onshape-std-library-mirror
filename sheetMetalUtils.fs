FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/booleanaccuracy.gen.fs", version : "✨");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "✨");
import(path : "onshape/std/boundingtype.gen.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/coordSystem.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");
import(path : "onshape/std/manipulator.fs", version : "✨");
import(path : "onshape/std/query.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/smobjecttype.gen.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/tool.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");
import(path : "onshape/std/topologyUtils.fs", version : "✨");
import(path : "onshape/std/transform.fs", version : "✨");



/**
 * Exposes sheet metal definition sheet body to the queries within feature.
 */
export function defineSheetMetalFeature(feature is function, defaults is map) returns function
{
    defaults.isSheetMetal = true;
    return defineFeature(feature, defaults);
}

/**
 * Based on current state of sheet metal definition sheet body update solid bodies
 * @param args {{
 *      @field entities{Query} : sheet metal definition entities changed (or attributes changed) in this feature
 *      @field deletedAttributes{array} : associated attributes of deleted sheet metal definition entities
 *      @field associatedChanges{Query} : sheet metal definition entities representing the change of this feature
 * }}
 */
export const updateSheetMetalGeometry = function(context is Context, id is Id, args is map)
{
    adjustCornerBreakAttributes(context, args.entities);
    @updateSheetMetalGeometry(context, id, args);
};

/**
* Direction of material from definition body. For old models (before V629_SM_MODEL_FRONT_N_BACK)
* It is BOTH, for new models FRONT/BACK depends on oppositeDirection in sheetMetalStart
*/
export enum SMThicknessDirection
{
    BOTH,
    FRONT,
    BACK
}

/**
 * Set a bend attribute on a cylindrical face
 * @param face {Query} : face to set bend attribute on
 * @param frontThickness {ValueWithUnits}
 * @param backThickness {ValueWithUnits}
 * @param attributeId {string} : id of new bend attribute
 */
export function setCylindricalBendAttribute(context is Context, face is Query, frontThickness, backThickness, attributeId is string)
{
    var surface = evSurfaceDefinition(context, {
            "face" : face
    });
    var bendAttribute = makeSMJointAttribute(attributeId);
    bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited" : false };
    bendAttribute.bendType = { "value" : SMBendType.STANDARD, "canBeEdited" : false };
    const angleVal = cylinderAngle(context, face);
    bendAttribute.angle = {"value" : angleVal, "canBeEdited" : false};

    const tanPlane = evFaceTangentPlane(context, { "face" : face, "parameter" : vector(0.5, 0.5)});
    const convex = (dot(tanPlane.origin - surface.coordSystem.origin, tanPlane.normal) > 0);
    var thicknessData = (convex) ? backThickness : frontThickness;
    var bendRadius = (thicknessData == undefined) ? surface.radius : surface.radius - thicknessData;
    bendAttribute.radius = { "value" : bendRadius, "canBeEdited" : false, "isDefault" : false};
    setAttribute(context, {
            "entities" : face,
            "attribute" : bendAttribute
    });
}

/**
* Assign SMAttributes to topology of sheet metal definition sheet body
* @param args {{
*       @field surfaceBodies{Query}
*       @field bendEdgesAndFaces{Query}
*       @field specialRadiiBends{array} : array of pairs "(edge, bendRadius)"
*       @field defaultRadius{ValueWithUnits} : bend radius to be applied to edges in bendEdgesAndFaces
*       @field controlsThickness{boolean}
*       @field thickness{ValueWithUnits}
*       @field defaultCornerReliefScale{number}
*       @field defaultRoundReliefDiameter{ValueWithUnits}
*       @field defaultSquareReliefWidth{ValueWithUnits}
*       @field defaultBendReliefScale{number}
* }}
*/
export function annotateSmSurfaceBodies(context is Context, id is Id, args is map, objectCount is number) returns number
{
    var surfaceBodies = evaluateQuery(context, args.surfaceBodies);
    if (size(surfaceBodies) == 0)
    {
        return 0;
    }
    var featureIdString = toAttributeId(id);
    var thicknessData = {"value" : (args.thicknessDirection == SMThicknessDirection.BOTH) ? 0.5 * args.thickness : args.thickness,
                            "canBeEdited" : args.controlsThickness};
    if (args.controlsThickness)
    {
        thicknessData.controllingFeatureId = featureIdString;
        thicknessData.parameterIdInFeature = "thickness";
    }
    var kFactorData = {"value" : args.kFactor,
        "canBeEdited" : true,
        "controllingFeatureId" : featureIdString,
        "parameterIdInFeature" : "kFactor"
        };
    var minimalClearanceData = {"value" : args.minimalClearance,
        "canBeEdited" : true,
        "controllingFeatureId" : featureIdString,
        "parameterIdInFeature" : "minimalClearance"
        };
    var defaultCornerReliefScale = { "value" : args.defaultCornerReliefScale,
        "canBeEdited" : true,
        "controllingFeatureId" : featureIdString,
        "parameterIdInFeature" : "defaultCornerReliefScale"
        };
    var defaultRoundReliefDiameter = { "value" : args.defaultRoundReliefDiameter,
        "canBeEdited" : true,
        "controllingFeatureId" : featureIdString,
        "parameterIdInFeature" : "defaultRoundReliefDiameter"
        };
    var defaultSquareReliefWidth = { "value" : args.defaultSquareReliefWidth,
        "canBeEdited" : true,
        "controllingFeatureId" : featureIdString,
        "parameterIdInFeature" : "defaultSquareReliefWidth"
        };
    var defaultBendReliefScale = { "value" : args.defaultBendReliefScale,
        "canBeEdited" : true,
        "controllingFeatureId" : featureIdString,
        "parameterIdInFeature" : "defaultBendReliefScale"
        };
    var defaultBendReliefDepthScale = { "value" : args.defaultBendReliefDepthScale,
        "canBeEdited" : true,
        "controllingFeatureId" : featureIdString,
        "parameterIdInFeature" : "defaultBendReliefDepthScale"
        };

    var modelAttribute = asSMAttribute({"attributeId" : featureIdString,
                    "objectType" : SMObjectType.MODEL,
                    "active" : true,
                    "k-factor" : kFactorData,
                    "minimalClearance" : minimalClearanceData,
                    "flipDirectionUp" : args.flipDirectionUp,
                    "defaultBendRadius" : {"value" : args.defaultRadius},
                    "defaultCornerReliefScale" : defaultCornerReliefScale,
                    "defaultRoundReliefDiameter" : defaultRoundReliefDiameter,
                    "defaultSquareReliefWidth" : defaultSquareReliefWidth,
                    "defaultBendReliefDepthScale" : defaultBendReliefDepthScale,
                    "defaultBendReliefScale" : defaultBendReliefScale,
                    "fsVersion" : getCurrentVersion(context)});
    if (args.thicknessDirection == SMThicknessDirection.FRONT ||
        args.thicknessDirection == SMThicknessDirection.BOTH)
    {
        modelAttribute.frontThickness = thicknessData;
    }
    if (args.thicknessDirection == SMThicknessDirection.BACK ||
        args.thicknessDirection == SMThicknessDirection.BOTH)
    {
        modelAttribute.backThickness = thicknessData;
    }

    if (args.defaultTwoCornerStyle != undefined)
    {
        modelAttribute.defaultTwoCornerStyle = args.defaultTwoCornerStyle;
    }
    if (args.defaultThreeCornerStyle != undefined)
    {
        modelAttribute.defaultThreeCornerStyle = args.defaultThreeCornerStyle;
    }
    if (args.defaultBendReliefStyle != undefined)
    {
        modelAttribute.defaultBendReliefStyle = args.defaultBendReliefStyle;
    }
    if (args.kFactorRolled != undefined)
    {
           modelAttribute.kFactorRolled = { "value" : args.kFactorRolled,
                "canBeEdited" : true,
                "controllingFeatureId" : featureIdString,
                "parameterIdInFeature" : "kFactorRolled"
                };
    }

    for (var body in surfaceBodies)
    {
        setAttribute(context, {
                "entities" : body,
                "attribute" : modelAttribute
        });
    }

    // bend entity to either true ( use default radius in case of edge, figure it out from geometry in case of face) or radius value
    var bendMap = {};
    for (var entity in evaluateQuery(context, args.bendEdgesAndFaces))
    {
        bendMap[entity] = true;
    }
    for (var edgeAndRadius in args.specialRadiiBends)
    {
        bendMap[edgeAndRadius[0]] = edgeAndRadius[1];
    }

    var facesQ =  qOwnedByBody(args.surfaceBodies, EntityType.FACE);
    var count = objectCount;
    var cylinderBends = [];
    var containsRolledFaces = false;
    for (var face in evaluateQuery(context, facesQ))
    {
        var surface = evSurfaceDefinition(context, {
                "face" : face
        });
        if (surface is Plane ||
            surface.surfaceType == SurfaceType.EXTRUDED ||
            (surface is Cylinder && bendMap[face] != true))
        {
            setAttribute(context, {
                    "entities" : face,
                    "attribute" : makeSMWallAttribute(toAttributeId(id + count))
            });
            count += 1;
        }
        else if (surface is Cylinder)
        {
            cylinderBends = append(cylinderBends, face);
        }
        else
        {
           setErrorEntities(context, id, { "entities" : face });
           reportFeatureError(context, id, ErrorStringEnum.SHEET_METAL_INVALID_FACE);
           return 0;
        }
        if (!(surface is Plane))
              containsRolledFaces = true;
    }
    for (var face in cylinderBends)
    {
        if (!cylinderCanBeBend(context, face, SMBendType.STANDARD))
        {
           setErrorEntities(context, id, { "entities" : face });
           reportFeatureError(context, id, ErrorStringEnum.SHEET_METAL_CYLINDER_BEND);
           return 0;
        }
        const frontThickness = (modelAttribute.frontThickness == undefined) ? undefined : modelAttribute.frontThickness.value;
        const backThickness = (modelAttribute.backThickness == undefined) ? undefined : modelAttribute.backThickness.value;
        setCylindricalBendAttribute(context, face, frontThickness, backThickness, toAttributeId(id + count));
        count += 1;
    }
    var edgesQ = qOwnedByBody(args.surfaceBodies, EntityType.EDGE);
    for (var edge in evaluateQuery(context, edgesQ))
    {
        var faces = evaluateQuery(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE));
        if (size(faces) != 2)
        {
            continue; // TODO : warning if edge is selected as a bend
        }
        if (size(getSmObjectTypeAttributes(context, qUnion(faces), SMObjectType.WALL)) != 2)
        {
            continue;
        }
        var bendRadius = bendMap[edge];
        var attributeId = toAttributeId(id + count);
        count += 1;
        if (bendRadius != undefined)
        {
            if (bendRadius == true)
            {
                bendRadius = args.defaultRadius;
            }
            var bendAttribute = createBendAttribute(context, id, edge, attributeId, bendRadius, false);
            if (bendAttribute == undefined)
            {
                setErrorEntities(context, id, {"entities" : edge});
                reportFeatureError(context, id, ErrorStringEnum.SHEET_METAL_NO_0_ANGLE_BEND);
                return 0;
            }
            setAttribute(context, {
                    "entities" : edge,
                    "attribute" : bendAttribute
            });
        }
        else
        {
            var angleVal = try silent(edgeAngle(context, edge));
            var zeroAngle = angleVal == undefined || angleVal < TOLERANCE.zeroAngle * radian;
            var jointAttribute = makeSMJointAttribute(attributeId);
            if (zeroAngle && !treatTangentEdgeAsRip(context, faces[0], faces[1]))
            {
                jointAttribute.jointType = { "value" : SMJointType.TANGENT, "canBeEdited": true };
            }
            else
            {
                jointAttribute.jointType = { "value" : SMJointType.RIP, "canBeEdited": true };
                if (angleVal != undefined)
                {
                    jointAttribute.angle = {"value" : angleVal, "canBeEdited" : false};
                }
                if (!zeroAngle)
                {
                    jointAttribute.jointStyle = { "value" : SMJointStyle.EDGE, "canBeEdited": true };
                }
            }
            setAttribute(context, {
                    "entities" : edge,
                    "attribute" : jointAttribute
            });
        }
    }
    // Non-trivial corner relief does not work on rolled walls
    if (containsRolledFaces &&
        (args.defaultTwoCornerStyle != SMReliefStyle.SIMPLE || args.defaultThreeCornerStyle != SMReliefStyle.SIMPLE))
    {
        reportFeatureInfo(context, id, ErrorStringEnum.SHEET_METAL_ROLLED_CORNER_RELIF, ["defaultCornerStyle"]);
    }
    var verticesQ = qOwnedByBody(args.surfaceBodies, EntityType.VERTEX);
    assignSMAssociationAttributes(context, qUnion([args.surfaceBodies, facesQ, edgesQ, verticesQ]));
    return count;
}

// Redundant edge between planes should be treated as a rip.
function treatTangentEdgeAsRip(context, face0 is Query, face1 is Query) returns boolean
{
    var surface0 = evSurfaceDefinition(context, {
                "face" : face0
        });
    var surface1 = evSurfaceDefinition(context, {
                "face" : face1
        });
    if (surface0 is Plane && surface1 is Plane)
    {
        return true;
    }
    return false;
}

function cylinderCanBeBend(context is Context, face is Query, bendType is SMBendType) returns boolean
{
    var lineEdgesQ = qGeometry(qAdjacent(face, AdjacencyType.EDGE, EntityType.EDGE), GeometryType.LINE);
    var lineEdges = evaluateQuery(context, lineEdgesQ);
    const requiredLineMinimum = (bendType == SMBendType.HEM) ? 1 : 2;
    if (size(lineEdges) < requiredLineMinimum)
    {
        return false;
    }
    var countOtherFaces = 0;
    var allowCutBends = isAtVersionOrLater(context, FeatureScriptVersionNumber.V775_DETACHED_FILLET);
    for (var edge in lineEdges)
    {
        var otherFaceQ = qSubtraction(qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE), face);
        var otherFaces = evaluateQuery(context, otherFaceQ);
        if (allowCutBends)
        {
            if (size(otherFaces) == 0)
            {
                continue;
            }
        }
        else
        {
             if (size(otherFaces) != 1)
            {
                return false;
            }
        }
        countOtherFaces += 1;
        var wallAttributes = getSmObjectTypeAttributes(context, otherFaces[0], SMObjectType.WALL);
        if (size(wallAttributes) != 1)
        {
            return false;
        }

        if (edgeAngle(context, edge) > TOLERANCE.zeroAngle * radian)
        {
            return false;
        }
    }
    return (allowCutBends) ? (countOtherFaces >= requiredLineMinimum) : true;
}

/**
 * Calculate the largest angle between face normals of an edge that is the result of the intersection between two cylinders
 * with the same radius.
 */
function edgeAngleBetweenIntersectingEquivalentCylinders(context is Context, edge is Query, faces is array, cylinders is array) returns ValueWithUnits
{
    const ellipse = evCurveDefinition(context, {
                "edge" : edge
            });
    const majorPoints = [
            ellipse.coordSystem.origin - (ellipse.coordSystem.xAxis * ellipse.majorRadius),
            ellipse.coordSystem.origin + (ellipse.coordSystem.xAxis * ellipse.majorRadius)
        ];

    if ((!isQueryEmpty(context, qContainsPoint(edge, majorPoints[0]))) || (!isQueryEmpty(context, qContainsPoint(edge, majorPoints[1]))))
    {
        // If the range of the ellipse captures either of the major points, we can use the analytical solution
        return angleBetween(cylinders[0].coordSystem.zAxis, cylinders[1].coordSystem.zAxis);
    }
    else
    {
        // Otherwise the point on the ellipse furthest from the center will have the largest angle
        const distanceResult = evDistance(context, {
                    "side0" : ellipse.coordSystem.origin,
                    "side1" : edge,
                    "maximum" : true,
                    "arcLengthParameterization" : false
                });
        // Before V947, we were accidentally sending non-arclength parameters into arc length calculation of evFaceNormalAtEdge
        const arcLengthForFaceNormal = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V947_EVDISTANCE_ARCLENGTH);
        var normal0 = evFaceNormalAtEdge(context, { "edge" : edge, "face" : faces[0], "parameter" : distanceResult.sides[1].parameter, "arcLengthParameterization" : arcLengthForFaceNormal });
        var normal1 = evFaceNormalAtEdge(context, { "edge" : edge, "face" : faces[1], "parameter" : distanceResult.sides[1].parameter, "arcLengthParameterization" : arcLengthForFaceNormal });
        return angleBetween(normal0, normal1);
    }
}

/**
 * Calculate angle between face normals at edge midpoint
 */
function edgeAngleAtMidpoint(context is Context, edge is Query, faces is array) returns ValueWithUnits
{
    var normal0 = evFaceNormalAtEdge(context, { "edge" : edge, "face" : faces[0], "parameter" : 0.5 });
    var normal1 = evFaceNormalAtEdge(context, { "edge" : edge, "face" : faces[1], "parameter" : 0.5 });
    return angleBetween(normal0, normal1);
}

/**
 * @internal
 * Compute angle between face normals at edge.  For most edges, use edge midpoint.  For special case of intersecting
 * cylinders, find the maximal angle.
 *
 * TODO: For correctness (especially in sheet metal rip push-back), this should always calculate maximum edge angle.
 */
export function edgeAngle(context is Context, edge is Query) returns ValueWithUnits
{
    var faces = evaluateQuery(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE));
    if (size(faces) != 2)
    {
        throw "Expects 2-sided edge";
    }
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V929_SM_ALIGN_UNROLLED))
    {
        const edgeIsLinear = !isQueryEmpty(context, qGeometry(edge, GeometryType.LINE));
        if (edgeIsLinear)
        {
            // Edge angle will be the same all the way along the edge (as long as both faces are developable surfaces).
            return edgeAngleAtMidpoint(context, edge, faces);
        }
        // Edge is not linear, so at least one of faces is not planar.  If one of the faces is planar, and the other face
        // is swept along the normal of that plane, the edge angle is the same all the way along the edge.
        const planarFaces = evaluateQuery(context, qGeometry(qUnion(faces), GeometryType.PLANE));
        if (planarFaces != [])
        {
            const planarFace = planarFaces[0];
            const nonPlanarFace = (planarFace == faces[0]) ? faces[1] : faces[0];
            const planeNormal = evPlane(context, { "face" : planarFace }).normal;
            // In either case we return the edge angle at midpoint, but in the case where the non-planar face is not swept
            // along the normal of the planar face, there may be a solution with a larger angle.
            if (!sweptAlong(context, nonPlanarFace, planeNormal))
            {
                // TODO: handle more cases
                @report("sheetMetalUtils.fs::edgeAngle(...): planar face intersecting face which is not swept along its normal.");
            }
            return edgeAngleAtMidpoint(context, edge, faces);
        }
        const cylindricalFaces = evaluateQuery(context, qGeometry(qUnion(faces), GeometryType.CYLINDER));
        if (size(cylindricalFaces) == 2)
        {
            const cylinder0 = evSurfaceDefinition(context, { "face" : faces[0] });
            const cylinder1 = evSurfaceDefinition(context, { "face" : faces[1] });
            if (tolerantEquals(cylinder0.radius, cylinder1.radius))
            {
                return edgeAngleBetweenIntersectingEquivalentCylinders(context, edge, faces, [cylinder0, cylinder1]);
            }
            else
            {
                // TODO: handle more cases
                @report("sheetMetalUtils.fs::edgeAngle(...): two intersecting cylindrical surfaces have different radii.");
                return edgeAngleAtMidpoint(context, edge, faces);
            }
        }
        // TODO: handle more cases
        @report("sheetMetalUtils.fs::edgeAngle(...): neither face is planar and " ~ size(cylindricalFaces) ~ " face(s) is (are) cylindrical.");
        return edgeAngleAtMidpoint(context, edge, faces);
    }
    else if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V684_SM_SWEPT_SUPPORT))
    {
        return edgeAngleAtMidpoint(context, edge, faces);
    }
    else
    {
        var plane0 = evPlane(context, {
            "face" : faces[0]
        });
        var plane1 = evPlane(context, {
            "face" : faces[1]
        });
        return angleBetween(plane0.normal, plane1.normal);
    }
}

/**
*   @internal
*   Compute angle for sheet metal bend of given radius built on edge of sheet metal definition model
*/
export function bendAngle(context is Context, id is Id, edge is Query, bendRadius is ValueWithUnits) returns ValueWithUnits
{
    var faces = evaluateQuery(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE));
    if (size(faces) != 2)
    {
        throw "Expects 2-sided edge";
    }
    const plane0 = try silent(evPlane(context, {"face" : faces[0]}));
    const plane1 = try silent(evPlane(context, {"face" : faces[1]}));

    if (plane0 != undefined && plane1 != undefined)
        return angleBetween(plane0.normal, plane1.normal);
    else
    {
       const modelParams = getModelParameters(context, qOwnerBody(edge));
       var thickness = 0;
       var convexity = evEdgeConvexity(context, {"edge" : edge});
       if (convexity == EdgeConvexityType.CONVEX)
          thickness = modelParams.backThickness;
       else if (convexity == EdgeConvexityType.CONCAVE)
          thickness = modelParams.frontThickness;
       return computeFilletAngle(context, id, edge, bendRadius + thickness);
    }
}

function computeFilletAngle(context is Context, id is Id, edge is Query, radius) returns ValueWithUnits
{
    var tempFilletId = id + "tempFillet";
    startFeature(context, tempFilletId);
    var angle;
    try
    {
        opFillet(context, tempFilletId, {
                "entities" : edge,
                "radius" : radius,
                "createDetachedSurface" : true
        });

        var filletFaces = qCreatedBy(tempFilletId, EntityType.FACE);
        angle = cylinderAngle(context, filletFaces);
    } catch (error)
    {
       // making sure the operation is rolled back
        abortFeature(context, tempFilletId);
        throw error;
    }
    abortFeature(context, tempFilletId);
    return angle;
}

function cylinderAngle(context, face is Query) returns ValueWithUnits
{
    const plane0 = evFaceTangentPlane(context, {
            "face" : qNthElement(face, 0),
            "parameter" : vector(0., 0.5)
    });

    const plane1 = evFaceTangentPlane(context, {
            "face" : qNthElement(face, 0),
            "parameter" : vector(0.5, 0.5)
    });

    return 2 * angleBetween(plane0.normal, plane1.normal);
}

/**
 * @internal
 * id is required for bendAngle() to create temporary fillet if needed
 */
export function updateJointAngle(context is Context, id is Id, jointEntities is Query)
{
    // We stopped creating new attributes here to allow addRipsForNewEdges to handle newly 2-sided edges.
    // Creating Tangent joint here was adding connectivity and causing unfold failure.
    var insertNewAttributeIfNecessary = isAtVersionOrLater(context, FeatureScriptVersionNumber.V704_MOVE_FACE_ROLLED_SM) &&
                                        !isAtVersionOrLater(context, FeatureScriptVersionNumber.V1047_MOVE_FACE_JOINTS);

    var newAttributeCounter = 0;
    for (var edgeOrFace in evaluateQuery(context, jointEntities))
    {
        const jointAttribute = try silent(getJointAttribute(context, edgeOrFace));
        if (jointAttribute == undefined)
        {
            if (insertNewAttributeIfNecessary)
            {
                const newJointAttribute = makeNewJointAttributeIfNeeded(context, edgeOrFace, toAttributeId(id + newAttributeCounter));
                if (newJointAttribute != undefined)
                {
                    setAttribute(context, {"entities" : edgeOrFace, "attribute" : newJointAttribute});
                    newAttributeCounter += 1;
                }
            }
            continue;
        }

        var replacementAttribute = computeReplacementAttribute(context, id, edgeOrFace, jointAttribute);

        if (replacementAttribute == undefined)
        {
            continue;
        }
        replaceSMAttribute(context, jointAttribute, replacementAttribute);
    }
}

function computeReplacementAttribute(context is Context, id is Id, edgeOrFace is Query, jointAttribute is map)
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V695_SM_SWEPT_SUPPORT))
    {
        return legacyComputeReplacementAttribute(context, edgeOrFace, jointAttribute);
    }

    const recomputeForFaces = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1047_MOVE_FACE_JOINTS);
    const edge = qEntityFilter(edgeOrFace, EntityType.EDGE);
    const cylinder = qGeometry(edgeOrFace, GeometryType.CYLINDER);
    // before V1047 we assumed an edge coming here
    var isEdge = !recomputeForFaces || (!isQueryEmpty(context, edge));
    if (isEdge && !edgeIsTwoSided(context, edge)) // can not continue as joint
    {
        clearSmAttributes(context, edge);
        return undefined;
    }

    var angleVal = 0.;
    if (isEdge)
    {
        if (recomputeForFaces && jointAttribute.jointType.value == SMJointType.BEND)
        {
           angleVal = try silent(bendAngle(context, id, edgeOrFace, jointAttribute.radius.value));
        }
        else
        {
            angleVal = try silent(edgeAngle(context, edge));
        }
    }
    else if (!isQueryEmpty(context, cylinder))
    {
        angleVal = cylinderAngle(context, cylinder);
    }
    else
    {
         throw "computeReplacementAttribute can be called for 2 sided edge or cylinder face";
    }

    if ((angleVal == undefined && jointAttribute.angle == undefined) ||
         try silent(tolerantEquals(angleVal, jointAttribute.angle.value)) == true) // nothing changed
    {
        return undefined;
    }

    var replacementAttribute = jointAttribute;
    var isTangentJoint = false;
    if (angleVal == undefined || abs(angleVal) < TOLERANCE.zeroAngle * radian)
    {
        replacementAttribute.jointStyle = undefined;
        if (isEdge && recomputeForFaces)
        {
            var faces = evaluateQuery(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE));
            if (size(faces) != 2)
            {
                throw "2 adjacent faces expected";
            }
            if (!treatTangentEdgeAsRip(context, faces[0], faces[1]))
            {
                replacementAttribute.jointType = { "value" : SMJointType.TANGENT, "canBeEdited": true };
                isTangentJoint = true;
            }
        }
        if (!isTangentJoint)
        {
            replacementAttribute.jointType = { "value" : SMJointType.RIP, "canBeEdited" : false };
        }
    }
    else if (replacementAttribute.jointType != undefined && replacementAttribute.jointType.value == SMJointType.RIP)
    {
        replacementAttribute.jointStyle = { "value" : SMJointStyle.EDGE, "canBeEdited" : true };
        replacementAttribute.jointStyle.canBeEdited = true;
    }

    replacementAttribute.angle = (isTangentJoint) ? undefined : { "value" : angleVal, "canBeEdited" :
                            jointAttribute.angle != undefined && jointAttribute.angle.canBeEdited };
    return replacementAttribute;
}

function legacyComputeReplacementAttribute(context is Context, edge is Query, jointAttribute is map)
{
    if (jointAttribute.angle == undefined)
    {
        return undefined;
    }

    var angleVal = try silent(edgeAngle(context, edge));
    if (angleVal == undefined || tolerantEquals(angleVal, jointAttribute.angle.value))
    {
        return undefined;
    }

    var replacementAttribute = jointAttribute;

    if (abs(angleVal) < TOLERANCE.zeroAngle * radian)
    {
        replacementAttribute.jointStyle = undefined;
        replacementAttribute.jointType = { "value" : SMJointType.RIP, "canBeEdited" : false };
    }
    else if (replacementAttribute.jointType != undefined && replacementAttribute.jointType.value == SMJointType.RIP)
    {
        replacementAttribute.jointStyle = { "value" : SMJointStyle.EDGE, "canBeEdited" : true };
        replacementAttribute.jointType.canBeEdited = true;
    }

    replacementAttribute.angle = { "value" : angleVal, "canBeEdited" : jointAttribute.angle.canBeEdited };
    return replacementAttribute;
}


/**
 * A `RealBoundSpec` for sheet metal K-factor between 0. and 1., defaulting to `.45`.
 */
export const K_FACTOR_BOUNDS =
{
    (unitless) : [0, 0.45, 1]
} as RealBoundSpec;

/**
 * A `RealBoundSpec` for rolled sheet metal K-factor between 0. and 1., defaulting to `.5`.
 */
export const ROLLED_K_FACTOR_BOUNDS =
{
    (unitless) : [0, 0.5, 1]
} as RealBoundSpec;

/**
 * A `LengthBoundSpec` for minimal clearance of sheet  metal rips
 */
export const SM_MINIMAL_CLEARANCE_BOUNDS =
{
    (meter)      : [2e-5, 2e-5, 1],
    (centimeter) : 2e-3,
    (millimeter) : 0.02,
    (inch)       : 1e-3,
    (foot)       : 1e-4,
    (yard)       : 2.2e-5
} as LengthBoundSpec;


/**
 * A `LengthBoundSpec` for bend radius in sheet metal features
 */
export const SM_BEND_RADIUS_BOUNDS =
{
    (meter)      : [1e-5, 0.0023 , 500],
    (centimeter) : 0.23,
    (millimeter) : 2.3,
    (inch)       : 0.09,
    (foot)       : 0.0075,
    (yard)       : 0.0025
} as LengthBoundSpec;
// ^ Adjustments to this also require adjustments to SM_HEM_GAP_BOUNDS.

/**
 * A `LengthBoundSpec` for thickness in sheet metal features. default to `(1/16)"` (i.e. steel)
 */
export const SM_THICKNESS_BOUNDS =
{
    (meter)      : [2e-5, 0.0016, 500],
    (centimeter) : 0.16,
    (millimeter) : 1.6,
    (inch)       : 0.0625,
    (foot)       : 0.005,
    (yard)       : 0.002
} as LengthBoundSpec;

/**
 * A `LengthBoundSpec` for relief size, corners or bend relief, in sheet metal features.
 */
export const SM_RELIEF_SIZE_BOUNDS =
{
    (meter)      : [2e-5, 0.005 , 500],
    (centimeter) : 0.5,
    (millimeter) : 5,
    (inch)       : 0.25,
    (foot)       : 0.015,
    (yard)       : 0.005
} as LengthBoundSpec;

/**
 * Partitions allParts into non-sheet metal parts and sheet metal parts.
 * To preserve existing behavior of code the returned non-sm query is exactly the same as what is passed in
 * for non-sm cases and a query is returned for them.
 * The sheet metal results will usually be iterated through and so are returned as a map with
 * the keys being the sheet metal ID and the values being the parts associated with that model.
 *
 * @seealso [separateSheetMetalQueries]
 */
export function partitionSheetMetalParts(context is Context, allParts is Query)
{
    if (!queryContainsActiveSheetMetal(context, allParts))
    {
        // Don't mess with the query, for performance and legacy reasons
        return { "nonSheetMetalPartsQuery" : allParts, "sheetMetalPartsMap" : {} };
    }
    var parts = evaluateQuery(context, allParts);
    var nonSheetMetal = [];
    var sheetMetal = {};
    for (var part in parts)
    {
        if (isActiveSheetMetalPart(context, part))
        {
            const sheetMetalId = getActiveSheetMetalId(context, part);
            var parts = try(sheetMetal[sheetMetalId]);
            if (parts == undefined) {
                sheetMetal[sheetMetalId] = [part];
            } else {
                sheetMetal[sheetMetalId] = append(parts, part);
            }
        }
        else
        {
            nonSheetMetal = append(nonSheetMetal, part);
        }
    }
    return { "nonSheetMetalPartsQuery" : qUnion(nonSheetMetal), "sheetMetalPartsMap" : sheetMetal };
}

/**
 * Get the first id of active sheet metal model the entities of query belong to.
 */
export function getActiveSheetMetalId(context is Context, query is Query)
{
    const partQuery = qOwnerBody(query);
    if (isQueryEmpty(context, partQuery))
    {
        return undefined;
    }
    const attributes = getSMAssociationAttributes(context, partQuery);
    if (attributes == [])
    {
        return undefined;
    }
    for (var attribute in attributes)
    {
        const modelAttributes = getSmObjectTypeAttributes(context, qAttributeQuery(attribute), SMObjectType.MODEL);
        if (size(modelAttributes) == 1 && modelAttributes[0].active == true)
        {
            return modelAttributes[0].attributeId;
        }
    }
    return undefined;
}

/**
 * @internal
 */
export function queryContainsActiveSheetMetal(context is Context, query is Query) returns boolean
{
    return getActiveSheetMetalId(context, query) != undefined;
}

/**
 * @internal
 */
export function queryContainsNonSheetMetal(context is Context, query is Query) returns boolean
{
    return !isQueryEmpty(context, qActiveSheetMetalFilter(query, ActiveSheetMetal.NO));
}

/**
 * @internal
 */
export function getSheetMetalModelForPart(context is Context, partQuery is Query) returns Query
{
    const definitionEntities = getSMDefinitionEntities(context, partQuery);
    return qUnion(definitionEntities);
}

/**
 * @internal
 */
export function isActiveSheetMetalPart(context is Context, partQuery is Query) returns boolean
{
    var entityAssociations = getSMAssociationAttributes(context, partQuery);
    if (size(entityAssociations) != 1)
    {
        return false;
    }
    // The intermediate attribute query will get back more entities than just the model but the NEXT query will only
    // get back the model attributes
    const attributes = getSmObjectTypeAttributes(context, qAttributeQuery(entityAssociations[0]), SMObjectType.MODEL);
    if (size(attributes) != 1)
    {
        return false;
    }
    else
    {
        return attributes[0].active == true;
    }
}

/**
 * Set a wall attribute on a face and add tangent joint attributes to the nonlaminar edges.
 * @internal
 */
export function addWallAttributeToPreviouslyBendFace(context is Context, face is Query, idBase is string)
{
    const faceEdges = evaluateQuery(context, qAdjacent(face, AdjacencyType.EDGE, EntityType.EDGE));
    var wallAttribute = makeSMWallAttribute(idBase);
    setAttribute(context, { "entities" : face, "attribute" : wallAttribute });

    var index = 0;
    for (var edge in faceEdges)
    {
        if (edgeIsTwoSided(context, edge))
        {
            var jointAttribute = makeSMJointAttribute(idBase ~ index);
            jointAttribute.jointType = { "value" : SMJointType.TANGENT, "canBeEdited" : false };
            setAttribute(context, { "entities" : edge, "attribute" : jointAttribute });
            index += 1;
        }
    }
}

/**
 * @internal
 * initialData is computed by a call to getInitialEntitiesAndAttributes at the beginning of the feature
 */
export function assignSMAttributesToNewOrSplitEntities(context is Context, sheetMetalModels is Query,
                                            initialData is map, operationId is Id) returns map
{
    var originalOrModifiedEntitiesMap = {};
    for ( var entity in initialData.originalEntities)
    {
        originalOrModifiedEntitiesMap[entity.transientId] = true;
    }
    for (var entityQ in initialData.originalEntitiesTracking)
    {
        const ents = evaluateQuery(context, entityQ);
        if (size(ents) == 1)
        {
            originalOrModifiedEntitiesMap[ents[0].transientId] = true;
        }
    }

    // Transient queries new to sheet metal body
    var entitiesToAddAssociations = filter(evaluateQuery(context, qOwnedByBody(sheetMetalModels)),
                    function(entry)
                    {
                        return originalOrModifiedEntitiesMap[entry.transientId] != true;
                    });
    var entitiesToAddAssociationsQ = qUnion(entitiesToAddAssociations);

    const attributes = getSMAssociationAttributes(context, entitiesToAddAssociationsQ);
    for (var attribute in attributes)
    {
        // If we find now that we have multiple entities with an association attribute then something like a wall
        // or bend has been split into multiple
        // Those entities need to get a new association attribute.
        // However, we need to propagate any bend/wall information and so we will now take a look to
        // see which attributes need copying and copy all the data, but with a new ID
        const entitiesWithAttribute = qAttributeFilter(qOwnedByBody(sheetMetalModels), attribute);
        var existingEntitiesWithAttribute = [];
        var newEntitiesWithAttribute = [];
        for (var entity in evaluateQuery(context, entitiesWithAttribute))
        {
            if (originalOrModifiedEntitiesMap[entity.transientId] == true)
                existingEntitiesWithAttribute = append(existingEntitiesWithAttribute, entity);
            else
                newEntitiesWithAttribute = append(newEntitiesWithAttribute, entity);
        }

        // First case: There is an existing entity that still has the attribute (masterEntities).
        var masterEntities = qUnion(existingEntitiesWithAttribute);
        var entitiesToModify = qUnion(newEntitiesWithAttribute);
        var nMaster = size(existingEntitiesWithAttribute);
        if (nMaster == 0)
        {
            // Second case: There are no existing entities that have the attribute
            // But there may be multiple new entities with the same attribute
            // Let the 'master' be the first of the new entities. It might be
            // reassigned below to the first one keeping the definition attribute
            masterEntities = newEntitiesWithAttribute[0];
            entitiesToModify = qSubtraction(entitiesToModify, masterEntities);
        }
        else if (nMaster > 1)
        {
            throw "Can't have multiple master entities";
        }

        const definitionAttributes = getAttributes(context, { "entities" : masterEntities, "attributePattern" : {} as SMAttribute });
        var nDefAttributes = size(definitionAttributes);
        if (nDefAttributes > 1)
        {
            throw "Entities with same association attribute can't have different definition attributes";
        }

        if (nDefAttributes == 1)
        {
            // qUnion does not re-order, so master entity will be considered first
            var evaluatedEntitiesToUpdateSmAttribute = evaluateQuery(context, qUnion([masterEntities, entitiesToModify]));
            var attributeSurvived = false;
            for (var count = 0; count < size(evaluatedEntitiesToUpdateSmAttribute); count += 1)
            {
                var entity = evaluatedEntitiesToUpdateSmAttribute[count];
                // Remove definition attribute from entities which are not fit to have it
                if (!isEntityAppropriateForAttribute(context, entity, definitionAttributes[0]))
                {
                    if (count == 0 && nMaster > 0)
                    {
                        throw "Existing entity not fit for definition attribute";
                    }
                    removeAttributes(context, { "entities" : entity, "attributePattern" : {} as SMAttribute });
                    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V664_FLAT_JOINT_TO_RIP))
                    {
                        var edgeQ = qEntityFilter(entity, EntityType.EDGE);
                        if (definitionAttributes[0].jointType != undefined &&
                            definitionAttributes[0].jointType.value == SMJointType.BEND)
                        {
                            if (try silent(edgeIsTwoSided(context, edgeQ)) == true)
                            {
                                const jointAttributeId = definitionAttributes[0].attribute_id ~ ".rip";
                                const ripAttribute = createRipAttribute(context, edgeQ, jointAttributeId, SMJointStyle.EDGE, {});
                                setAttribute(context, { "entities" : edgeQ, "attribute" : ripAttribute });
                            }
                            else if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V914_SM_JOINT_TO_WALL) &&
                                size(evaluateQuery(context, qEntityFilter(entity, EntityType.FACE))) == 1)
                            {
                                addWallAttributeToPreviouslyBendFace(context, qEntityFilter(entity, EntityType.FACE), toAttributeId(operationId + ("wall" ~ count)));
                            }
                        }
                    }
                }
                else if (!attributeSurvived) // favor one of the entities inheriting the attribute to be a masterEntity
                {
                    masterEntities = entity;
                    attributeSurvived = true;
                }
            }
            entitiesToModify = qSubtraction(qUnion(newEntitiesWithAttribute), masterEntities);
        }

        // Clean up association attributes off of the entities that need new attributes
        removeAttributes(context, { "entities" : entitiesToModify, "attributePattern" : attribute });
    }

    //After propagating attributes with split entities we can look to see what is left, i.e. entities
    // with NO attribute and add completely new attributes to those
    var entitiesWithExistingAttributesQ = qAttributeFilter(entitiesToAddAssociationsQ, smAssociationAttributePattern);

    var entitiesThatNeedAssociationQ = qSubtraction(entitiesToAddAssociationsQ, entitiesWithExistingAttributesQ);
    assignSMAssociationAttributes(context, entitiesThatNeedAssociationQ);

    const allEntities = qOwnedByBody(sheetMetalModels); // counting on body transient query surviving
    var finalAssociationAttributes = getSMAssociationAttributes(context, allEntities);
    var finalAssociationAttributesMap = {};
    for (var attribute in finalAssociationAttributes)
        finalAssociationAttributesMap[attribute.attributeId] = true;
    var deletedAttributes = filter(initialData.initialAssociationAttributes,
                    function(attribute)
                    {
                        return finalAssociationAttributesMap[attribute.attributeId] != true;
                    });
    return { "modifiedEntities" : entitiesToAddAssociationsQ, "deletedAttributes" : deletedAttributes};
}

/**
 * @internal
 */
export function isEntityAppropriateForAttribute(context is Context, entity is Query, attribute is SMAttribute) returns boolean
{
    var filteredQ;
    if (attribute.objectType == SMObjectType.MODEL)
        filteredQ = qEntityFilter(entity, EntityType.BODY);
    else if (attribute.objectType == SMObjectType.JOINT)
    {
        if (attribute.jointType.value == SMJointType.BEND &&
            size(evaluateQuery(context, qGeometry(entity, GeometryType.CYLINDER))) == 1)
        {
            const bendType = (attribute.bendType != undefined) ? attribute.bendType.value : SMBendType.STANDARD;
            return cylinderCanBeBend(context, qGeometry(entity, GeometryType.CYLINDER), bendType);
        }

        filteredQ = qEntityFilter(entity, EntityType.EDGE);
        if (try silent(edgeIsTwoSided(context, filteredQ)) != true)
        {
            return false;
        }
        var nLines = size(evaluateQuery(context, qGeometry(entity, GeometryType.LINE)));
        var zeroAngle = false;
        if (nLines == 1)
        {
            var angleVal = try silent(edgeAngle(context, filteredQ));
            zeroAngle = angleVal == undefined || angleVal < TOLERANCE.zeroAngle * radian;
        }
        if (attribute.jointType.value == SMJointType.BEND)
        {
            if (nLines != 1)
            {
                return false;
            }
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V664_FLAT_JOINT_TO_RIP))
            {
                return !zeroAngle;
            }
        }
        if (attribute.jointType.value == SMJointType.TANGENT)
        {
            if (nLines != 1 ||
                size(evaluateQuery(context, qGeometry(qAdjacent(entity, AdjacencyType.EDGE, EntityType.FACE), GeometryType.PLANE))) == 2)
            {
                return false;
            }
            return zeroAngle;
        }
        if (attribute.jointType.value == SMJointType.RIP && zeroAngle && nLines == 1)
        {
            const faceBendAttributes = getSmObjectTypeAttributes(context, qAdjacent(entity, AdjacencyType.EDGE, EntityType.FACE), SMObjectType.JOINT);
            return (size(faceBendAttributes) == 0);
        }
        return true;
    }
    else if (attribute.objectType == SMObjectType.WALL)
    {
        filteredQ = qEntityFilter(entity, EntityType.FACE);
    }
    else if (attribute.objectType == SMObjectType.CORNER)
    {
        filteredQ = qEntityFilter(entity, EntityType.VERTEX);
    }
    else
    {
        throw ("Unhandled SMObjectType" ~ attribute.objectType);
    }
    if (size(evaluateQuery(context, filteredQ)) != 1)
    {
        return false;
    }
    return true;
}

/**
 * Extract sheet metal model parameters in a convenient form
 */
export function getModelParameters(context is Context, model is Query) returns map
{
    var attr = getAttributes(context, {"entities" : model, "attributePattern" : asSMAttribute({})});

    if (size(attr) != 1 || (
        (attr[0].frontThickness == undefined || attr[0].frontThickness.value == undefined)  &&
        (attr[0].backThickness == undefined || attr[0].backThickness.value == undefined)) ||
        attr[0].minimalClearance == undefined || attr[0].minimalClearance.value == undefined ||
        attr[0].defaultBendRadius == undefined || attr[0].defaultBendRadius.value == undefined)
    {
        throw "Could not get sheet metal attribute.";
    }
    var frontThickness = (attr[0].frontThickness == undefined) ? 0 * meter : attr[0].frontThickness.value;
    var backThickness = (attr[0].backThickness == undefined) ? 0 * meter : attr[0].backThickness.value;
    return {"frontThickness" : frontThickness,
            "backThickness" : backThickness,
            "minimalClearance" : attr[0].minimalClearance.value,
            "defaultBendRadius" : attr[0].defaultBendRadius.value};
}

/**
 * Separates queries which are part of an active sheet metal model (either in the folded model or
 * the flat pattern).
 * @seealso [partitionSheetMetalParts]
 *
 * @return {{
 *      @field sheetMetalQueries {Query} : `targets` which are part of an active sheet metal model
 *      @field nonSheetMetalQueries {Query} : `targets` which are not part of an active sheet metal model
 * }}
 */
export function separateSheetMetalQueries(context is Context, targets is Query) returns map
{
    return {
            // Return a union of transient queries for legacy purposes
            "sheetMetalQueries" : qUnion(evaluateQuery(context, qActiveSheetMetalFilter(targets, ActiveSheetMetal.YES))),
            "nonSheetMetalQueries" : qUnion(evaluateQuery(context, qActiveSheetMetalFilter(targets, ActiveSheetMetal.NO)))
        };
}

/**
 * @internal
 */
export function checkNotInFeaturePattern(context is Context, references is Query, error is ErrorStringEnum)
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V675_MORE_TAB_FIXES))
    {
        if (isInFeaturePattern(context))
        {
            throw regenError(error);
        }
    }
    else
    {
        var remainingTransform = getRemainderPatternTransform(context, {"references" : references});
        if (remainingTransform != identityTransform())
        {
            throw regenError(error);
        }
    }
}

/**
 * @internal
 * Used in derive to strip sheet metal related data off the imported context
 * returns query of all sheet metal parts (3d, flattened and bend lines ), except for
 * sheet metal models in partsToKeep.
 */
export function clearSheetMetalData(context is Context, id is Id, partsToKeep, keepDefinitionBodies is boolean) returns Query
precondition
{
    partsToKeep == undefined || partsToKeep is Query;
}
{
    // All the attribute queries are evaluated immediately because this function
    // removes all SMAttributes and SMAssociationAttribute
    var smModelsQ = qAttributeQuery(asSMAttribute({objectType : SMObjectType.MODEL}));
    var smModelsEvaluated = evaluateQuery(context, smModelsQ);

    if (size(smModelsEvaluated) == 0)
        return qNothing();

    var smModelsActiveQ = qAttributeQuery(asSMAttribute({objectType : SMObjectType.MODEL,
                                                  active : true}));
    var smModelsActiveEvaluated = evaluateQuery(context, smModelsActiveQ);

    const smModelsToKeep = getSmModelsToKeep(context, partsToKeep);
    var keepingSomeModels = false;
    if (smModelsToKeep != [])
    {
        const smModelsToKeepQ = qUnion(smModelsToKeep);
        smModelsQ = qSubtraction(smModelsQ, smModelsToKeepQ);
        smModelsEvaluated = evaluateQuery(context, smModelsQ);
        smModelsActiveQ = qSubtraction(smModelsActiveQ, smModelsToKeepQ);
        smModelsActiveEvaluated = evaluateQuery(context, smModelsActiveQ);
        keepingSomeModels = true;
    }

    // Solid bodies 3d and Flat and only they are associated with sheet bodies
    var associationAttributes = getSMAssociationAttributes(context, smModelsQ);
    var smPartNBendLineQArr = [];
    for (var attribute in associationAttributes)
    {
        smPartNBendLineQArr = append(smPartNBendLineQArr, qAttributeQuery(attribute));
    }

    // Bend centerlines are wire bodies associated with edges or cylinder faces of sheet bodies.
    // There is more topology associated with edges that  why we have to apply filters to qAttributeQuery
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V557_NO_BEND_CENTERLINE_IN_DERIVED))
    {
        var smEdgeQ = qOwnedByBody(smModelsQ, EntityType.EDGE);
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V724_SM_MAKE_JOINT_TYPE))
        { // As to avoid too many iterations on FS side filter out laminar and non-linear edges which can't be bends .
            smEdgeQ = qGeometry(qEdgeTopologyFilter(smEdgeQ, EdgeTopology.TWO_SIDED), GeometryType.LINE);
        }
        const smCylinderQ = qGeometry(qOwnedByBody(smModelsQ, EntityType.FACE), GeometryType.CYLINDER);

        associationAttributes = getSMAssociationAttributes(context, qUnion([smEdgeQ, smCylinderQ]));
        for (var attribute in associationAttributes)
        {
            smPartNBendLineQArr = append(smPartNBendLineQArr, qEntityFilter(qBodyType(qAttributeQuery(attribute), BodyType.WIRE), EntityType.BODY));
        }
    }


    var smPartNBendLineQEvaluated = evaluateQuery(context, qUnion(smPartNBendLineQArr));

    // remove all SMAttributes
    removeAttributes(context, {
        "entities" : (keepingSomeModels) ? qOwnedByBody(smModelsQ) : undefined,
        "attributePattern" : asSMAttribute({})
    });

    // Deactivating active sheet metal models
    if (size(smModelsActiveEvaluated) > 0)
    {
        updateSheetMetalGeometry(context, id, { "entities" : qUnion(smModelsActiveEvaluated) });
    }

    // remove all SMAssociationAttribute
    removeAttributes(context, {
        "entities" : (keepingSomeModels) ? qOwnedByBody(smModelsQ) : undefined,
        "attributePattern" : smAssociationAttributePattern
    });

    if (!keepDefinitionBodies) {
        // Deleting all sheet bodies
        opDeleteBodies(context, id + "deleteSheetBodies", {
                "entities" : qUnion(smModelsEvaluated)
        });
    }

   return qUnion(smPartNBendLineQEvaluated);
}

function getSmModelsToKeep(context is Context, parts) returns array
{
    if (parts == undefined)
    {
        return [];
    }
    const associationAttributes = getSMAssociationAttributes(context, parts);
    var out = [];
    for (var attribute in associationAttributes)
    {
        const smModelQ = qAttributeFilter(qAttributeQuery(attribute), asSMAttribute({objectType : SMObjectType.MODEL}));
        out = concatenateArrays([out, evaluateQuery(context, smModelQ)]);
    }
    return out;
}

/**
 * @internal
 */
function makeNewJointAttributeIfNeeded(context is Context, edge is Query, attributeId is string)
{
    var faces = evaluateQuery(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE));
    if (size(faces) != 2)
    {
        return undefined;
    }

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V695_SM_SWEPT_SUPPORT))
    {
        //2-sided edge without attribute might be an edge of cylindrical bend or a tangent joint
        if (size(getSmObjectTypeAttributes(context, qUnion(faces), SMObjectType.WALL)) != 2)
        {
            return undefined;
        }
        var angleVal = try silent(edgeAngle(context, edge));
        var zeroAngle = angleVal == undefined || angleVal < TOLERANCE.zeroAngle * radian;
        if (zeroAngle && !treatTangentEdgeAsRip(context, faces[0], faces[1]))
        {
            var jointAttribute = makeSMJointAttribute(attributeId);
            jointAttribute.jointType = { "value" : SMJointType.TANGENT, "canBeEdited": true };
            return jointAttribute;
        }
    }
    //default to RIP
    return createRipAttribute(context, edge, attributeId, SMJointStyle.EDGE, undefined);
}

/**
 * @internal
 */
export function createRipAttribute(context is Context, entity is Query, ripId is string, ripStyle is SMJointStyle, jointAttributes)
{
    var ripAttribute = makeSMJointAttribute(ripId);
    ripAttribute.jointType = { "value" : SMJointType.RIP, "canBeEdited": true };

    var angle = try silent(edgeAngle(context, entity));
    if (angle != undefined)
    {
        ripAttribute.angle = {"value" : angle, "canBeEdited" : false};
    }
    // If the angle is zero then this rip should not have a style
    if (angle != undefined && abs(angle) >= TOLERANCE.zeroAngle * radian) {
        ripAttribute.jointStyle = { "value" : ripStyle, "canBeEdited": true };
    }

    if (jointAttributes != undefined && jointAttributes.minimalClearance != undefined)
    {
        ripAttribute.minimalClearance = jointAttributes.minimalClearance;
    }
    return ripAttribute;
}

/**
 * @internal
 */
export function createBendAttribute(context is Context, id is Id, edge is Query, bendId is string,
                                    bendRadius is ValueWithUnits, allowNoAngle is boolean)
{
    var bendAttribute = makeSMJointAttribute(bendId);
    bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited": true };
    bendAttribute.bendType = { "value" : SMBendType.STANDARD, "canBeEdited" : false };
    bendAttribute.radius = {
        "value" : bendRadius,
        "canBeEdited" : true
    };
    const angle = try silent(bendAngle(context, id, edge, bendRadius));
    if (!allowNoAngle &&
         (angle == undefined || abs(angle) < TOLERANCE.zeroAngle * radian))
        return undefined;

    if (angle != undefined)
    {
        bendAttribute.angle = {"value" : angle, "canBeEdited" : false};
    }
    return bendAttribute;
}

/**
 * @internal
 */
export function findCornerDefinitionVertex(context is Context, entity is Query) returns Query
{
    var definitionEntities = qUnion(getSMDefinitionEntities(context, entity));
    var sheetVertices = qEntityFilter(definitionEntities, EntityType.VERTEX);
    var found = size(evaluateQuery(context, sheetVertices));
    if (found == 0)
    {
        // Look for the vertices adjacent to any edge to get to it from them
        sheetVertices = qUnion(getSMDefinitionEntities(context, qAdjacent(qEntityFilter(entity, EntityType.EDGE), AdjacencyType.VERTEX, EntityType.VERTEX)));
        found = size(evaluateQuery(context, sheetVertices));
    }
    if (found != 1)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_RIP_NO_CORNER);
    }
    return sheetVertices;
}

/**
* @internal
* Remove corner breaks at the bend ends
*/
function adjustCornerBreakAttributes(context is Context, modifiedEntities is Query)
{
    for (var edgeQ in evaluateQuery(context, qEntityFilter(modifiedEntities, EntityType.EDGE)))
    {
        var attributes = getSmObjectTypeAttributes(context, edgeQ, SMObjectType.JOINT);
        if (size(attributes) != 1 ||
            attributes[0].jointType == undefined ||
            attributes[0].jointType.value != SMJointType.BEND)
        {
            continue;
        }
        removeCornerBreaksAtEnds(context, edgeQ);
    }
}

/**
* @internal
*/
export function removeCornerBreaksAtEdgeVertices(context is Context, edges is Query)
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V589_STABLE_BREAK_REMOVAL))
    {
        edges = qEntityFilter(edges, EntityType.EDGE);
    }

    for (var edge in evaluateQuery(context, edges))
    {
        removeCornerBreaksAtEnds(context, edge);
    }
}

function removeCornerBreaksAtEnds(context is Context, edgeQ is Query)
{
    var adjacentFaces = qAdjacent(edgeQ, AdjacencyType.EDGE, EntityType.FACE);
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V589_STABLE_BREAK_REMOVAL))
    {
        if (isQueryEmpty(context, adjacentFaces))
        {
            return;
        }
    }

    var wallIds = [];
    for (var wallAttribute in getSmObjectTypeAttributes(context, adjacentFaces, SMObjectType.WALL))
    {
        wallIds = append(wallIds, wallAttribute.attributeId);
    }
    for (var vertexQ in evaluateQuery(context, qAdjacent(edgeQ, AdjacencyType.VERTEX, EntityType.VERTEX)))
    {
        var cornerAttributes = getSmObjectTypeAttributes(context, vertexQ, SMObjectType.CORNER);
        if (size(cornerAttributes) != 1 ||
            cornerAttributes[0].cornerBreaks == undefined ||
            size(cornerAttributes[0].cornerBreaks) == 0)
        {
            continue;
        }
        var updatedAttribute = cornerAttributes[0];
        var nCornerBreaksBefore = size(updatedAttribute.cornerBreaks);
        updatedAttribute.cornerBreaks = filter(updatedAttribute.cornerBreaks,
                      function(cornerBreak) returns boolean
                      {
                          return (cornerBreak.value != undefined &&
                            !isIn(cornerBreak.value.wallId, wallIds));
                      } );
        var nCornerBreaksAfter = size(updatedAttribute.cornerBreaks);
        if (nCornerBreaksAfter !=  nCornerBreaksBefore)
        {
            updateCornerAttribute(context, vertexQ, updatedAttribute);
        }
    }
}

/**
 *  Wrapper around opExtendSheetBody used in sheet metal operations to handle remapping of cornerBreak data
 */
export const sheetMetalExtendSheetBodyCall = function(context is Context, id is Id, definition is map)
{
    var vertexToTrackingAndAttribute = collectAttributesOfAdjacentVertices(context, definition.entities);
    var edgeToTrackingAndAssociation = removeAssociationsFromFreeEdges(context, definition.entities);
    opExtendSheetBody(context, id, definition);
    restoreAssociations(context, edgeToTrackingAndAssociation);
    adjustCornerBreakAttributes(context, id, vertexToTrackingAndAttribute);
};

/**
*  Wrapper around opEdgeChange used in sheet metal operations to handle remapping of cornerBreak data
*/
export function sheetMetalEdgeChangeCall(context is Context, id is Id, edges is Query, definition is map)
{
    var vertexToTrackingAndAttribute = collectAttributesOfAdjacentVertices(context, edges);
    var edgeToTrackingAndAssociation = removeAssociationsFromFreeEdges(context, edges);
    opEdgeChange(context, id, definition);
    restoreAssociations(context, edgeToTrackingAndAssociation);
    adjustCornerBreakAttributes(context, id, vertexToTrackingAndAttribute);
}

function removeAssociationsFromFreeEdges(context is Context, edgesIn is Query) returns map
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V599_SM_ASSOCIATION_FIX))
    {
        return {};
    }
    var edgeToTrackingAndAssociation = {};
    var adjacentEdgesQ = qAdjacent(edgesIn, AdjacencyType.VERTEX, EntityType.EDGE);
    for (var edge in evaluateQuery(context, adjacentEdgesQ))
    {
        if (edgeIsTwoSided(context, edge) || size(getAttributes(context, {
                "entities" : edge,
                "attributePattern" : asSMAttribute({})})) > 0)
        {
            continue;
        }
        var associations = getSMAssociationAttributes(context, edge);
        if (size(associations) == 1)
        {
            edgeToTrackingAndAssociation[edge] = {
                "tracking" : startTracking(context, { 'subquery' : edge,
                                                      'trackPartialDependency' : true}),
                "association" : associations[0]};
            removeAttributes(context, {
                "entities" : edge,
                "attributePattern" : smAssociationAttributePattern});
        }
    }
    return edgeToTrackingAndAssociation;
}

function restoreAssociations(context is Context, edgeToTrackingAndAssociation is map)
{
    for (var edgeData in edgeToTrackingAndAssociation)
    {
        var edgesAfter = evaluateQuery(context, qUnion([edgeData.key, edgeData.value.tracking]));
        for (var edge in edgesAfter)
        {
            var associations = getSMAssociationAttributes(context, edge);
            if (size(associations) == 0)
            {
                setAttribute(context, {
                        "entities" : edge,
                        "attribute" : edgeData.value.association
                });
            }
        }
    }
}

function collectAttributesOfAdjacentVertices(context is Context, edgesIn is Query) returns map
{
    var vertexToTrackingAndAttribute = {};
    for (var vertexQ in evaluateQuery(context, qAdjacent(edgesIn, AdjacencyType.VERTEX, EntityType.VERTEX)))
    {
        var attribute = getCornerAttribute(context, vertexQ);
        if (attribute != undefined &&
            attribute.cornerBreaks != undefined &&
            size(attribute.cornerBreaks) > 0)
        {
            vertexToTrackingAndAttribute[vertexQ] = { "tracking" : startTracking(context, vertexQ), "attribute" : attribute};
        }
    }
    return vertexToTrackingAndAttribute;
}

function adjustCornerBreakAttributes(context is Context, id is Id, vertexToTrackingAndAttribute is map)
{
    var counter = 0;
    for (var vertexData in vertexToTrackingAndAttribute)
    {
        var afterVertices = evaluateQuery(context, qUnion([vertexData.key, vertexData.value.tracking]));
        if (size(afterVertices) == 0)
           continue;
        for (var vert in afterVertices)
        {
            var survivingAttribute = getCornerAttribute(context, vert);
            var wallsAroundVertex = getSmObjectTypeAttributes(context, qAdjacent(vert, AdjacencyType.VERTEX, EntityType.FACE), SMObjectType.WALL);
            var cornerBreaks = [];
            for (var wall in wallsAroundVertex)
            {
                var cBreak = findCornerBreak(vertexData.value.attribute, wall.attributeId);
                if (cBreak == undefined && survivingAttribute != undefined)
                {
                   //When vertices merge (e.g. makeJoint) surviving attribute is an attribute that belonged
                   // to one of the original vertices. Here we are merging data from all originals.
                    cBreak = findCornerBreak(survivingAttribute, wall.attributeId);
                }
                if (cBreak != undefined)
                {
                    cornerBreaks = append(cornerBreaks, cBreak);
                }
            }
            if (survivingAttribute != undefined)
            {
                var newAttribute = survivingAttribute;
                newAttribute.cornerBreaks = cornerBreaks;
                updateCornerAttribute(context, vert, newAttribute);
            }
            else if (size(cornerBreaks) > 0)
            {
                var newAttribute = makeSMCornerAttribute(toAttributeId(id + ("breakReshuffle" ~ counter)));
                counter += 1;
                newAttribute.cornerBreaks = cornerBreaks;
                setAttribute(context, {
                        "entities" : vert,
                        "attribute" : newAttribute
                });
            }
        }
    }
}

/**
 * Returns an array of sm models associated with selection in a way that works outside of sheet metal features.
 */
export function getOwnerSMModel(context is Context, selection is Query) returns array
{
    var entityAssociations = try silent(getSMAssociationAttributes(context, qBodyType(selection, BodyType.SOLID)));
    var out = [];
    if (entityAssociations != undefined)
    {
        for (var attribute in entityAssociations)
        {
            const modelQuery = qAttributeQuery(asSMAttribute({ "objectType" : SMObjectType.MODEL }));
            const associatedEntities = evaluateQuery(context, qIntersection([qAttributeQuery(attribute), qOwnedByBody(modelQuery)]));
            const ownerBody = qOwnerBody(qUnion(associatedEntities));
            const isActive = isSheetMetalModelActive(context, ownerBody);
            if (isActive != undefined && isActive)
            {
                out = append(out, ownerBody);
            }
        }
    }

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1076_TRANSIENT_QUERY))
        return evaluateQuery(context, qUnion(out));
    else
        return out;
}

/**
 * Returns a query for all sheet metal part entities of entityType related to the input sheet metal model entities.
 */
export function getSMCorrespondingInPart(context is Context, selection is Query, entityType is EntityType) returns Query
{
    var entityAssociations = getSMAssociationAttributes(context, selection);
    var out = [];
    for (var attribute in entityAssociations)
    {
        var associatedEntities = evaluateQuery(context, qBodyType(qAttributeQuery(attribute), BodyType.SOLID));
        out = concatenateArrays([out, associatedEntities]);
    }

    var corresponding = qEntityFilter(qUnion(out), entityType);
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V948_BOOLEAN_TOOLS_STRICTER))
    {
        return qSheetMetalFlatFilter(corresponding, SMFlatType.NO);
    }
    return qSubtraction(corresponding, qCorrespondingInFlat(corresponding));
}

/**
 * Collect information before adding material which may merge sheet metal walls. The map returned by this function should
 * be passed directly into [remapCornerBreaks] after making the desired geometry changes.
 *
 * Only walls which have broken corners are tracked.
 * @returns {{
 *     @field wallIdToPersistentWall {map} : a map from wall id to a tracked query for the wall
 *     @field wallIdToVerticesWithBreaks {array} : a map from wall id to an array of tracked vertices which break the wall
 * }}
 */
export function collectCornerBreakTracking(context is Context, bodies is Query) returns map
{
    const walls = qOwnedByBody(bodies, EntityType.FACE);

    var wallIdToPersistentWall = {};
    var wallIdToVerticesWithBreaks = {};
    for (var wall in evaluateQuery(context, walls))
    {
        const wallId = try silent(getWallAttribute(context, wall).attributeId);
        if (wallId == undefined)
        {
            // This should not happen, but we should be tolerant to it.
            continue;
        }
        for (var vertex in evaluateQuery(context, qAdjacent(wall, AdjacencyType.VERTEX, EntityType.VERTEX)))
        {
            const cornerBreaks = try silent(getCornerAttribute(context, vertex).cornerBreaks);
            if (cornerBreaks == undefined)
                continue;
            for (var cornerBreak in cornerBreaks)
            {
                // If the corner break breaks the wall in question
                if (cornerBreak.value.wallId == wallId)
                {
                    // Add the wall to the persistent wall map
                    if (wallIdToPersistentWall[wallId] == undefined)
                    {
                        var wallTracking = startTracking(context, { "subquery" : wall, "trackPartialDependency" : true });
                        wallTracking = qEntityFilter(wallTracking, EntityType.FACE);
                        wallIdToPersistentWall[wallId] = qUnion([wall, wallTracking]);
                    }

                    // Add the vertex to the list of vertices breaking the wall
                    if (wallIdToVerticesWithBreaks[wallId] == undefined)
                    {
                        wallIdToVerticesWithBreaks[wallId] = [];
                    }
                    const persistentVertexQuery = qUnion([vertex, startTracking(context, vertex)]);
                    wallIdToVerticesWithBreaks[wallId] = append(wallIdToVerticesWithBreaks[wallId], persistentVertexQuery);
                }
            }
        }
    }

    return {
        "wallIdToPersistentWall" : wallIdToPersistentWall,
        "wallIdToVerticesWithBreaks" : wallIdToVerticesWithBreaks
    };
}

/**
 * Remap corner breaks on walls whose wall id has changed.  Takes the output of [collectCornerBreakTracking] as
 * `cornerBreakTracking`.
 */
export function remapCornerBreaks(context is Context, cornerBreakTracking is map)
{
    var oldWallIdToNewWallId = {};
    for (var wallIdToWallQuery in cornerBreakTracking.wallIdToPersistentWall)
    {
        const newWalls = evaluateQuery(context, wallIdToWallQuery.value);
        var newWallId;
        // Make sure all the walls that came from the wall in question have the same wall id.
        for (var wall in newWalls)
        {
            const wallId = try silent(getWallAttribute(context, wall).attributeId);
            if (wallId == undefined)
            {
                // All walls should have wall ids.
                newWallId = undefined;
                break;
            }

            if (newWallId == undefined)
            {
                newWallId = wallId;
            }
            if (wallId != newWallId)
            {
                // Wall split into multiple wall ids.  There is nothing we can do.
                newWallId = undefined;
                break;
            }
        }
        if (newWallId == undefined)
            continue;

        // Add a mapping if a remap is required
        if (newWallId != wallIdToWallQuery.key)
        {
            oldWallIdToNewWallId[wallIdToWallQuery.key] = newWallId;
        }
    }

    var vertexToOriginalAndNewAttribute = {};
    for (var oldAndNew in oldWallIdToNewWallId)
    {
        const oldWallId = oldAndNew.key;
        const newWallId = oldAndNew.value;
        for (var vertex in evaluateQuery(context, qUnion(cornerBreakTracking.wallIdToVerticesWithBreaks[oldWallId])))
        {
            var originalAndNewAttribute = vertexToOriginalAndNewAttribute[vertex];
            var attributeToModify;
            if (originalAndNewAttribute == undefined)
            {
                const originalAttribute = getCornerAttribute(context, vertex);
                if (originalAttribute == undefined)
                {
                    // This should not happen, but we should be tolerant to it.
                    continue;
                }
                vertexToOriginalAndNewAttribute[vertex] = {
                        "originalAttribute" : originalAttribute,
                        "newAttribute" : originalAttribute
                };
                attributeToModify = originalAttribute;
            }
            else
            {
                attributeToModify = vertexToOriginalAndNewAttribute[vertex].newAttribute;
            }

            // Remap the ids in question and store the new attribute for batch update.
            for (var i = 0; i < size(attributeToModify.cornerBreaks); i += 1)
            {
                if (attributeToModify.cornerBreaks[i].value.wallId == oldWallId)
                {
                    attributeToModify.cornerBreaks[i].value.wallId = newWallId;
                    vertexToOriginalAndNewAttribute[vertex].newAttribute = attributeToModify;
                    break;
                }
            }
        }
    }

    // Update the attributes which have recieved changes.
    for (var originalAndNewAttribute in values(vertexToOriginalAndNewAttribute))
    {
        replaceSMAttribute(context, originalAndNewAttribute.originalAttribute, originalAndNewAttribute.newAttribute);
    }
}

/**
 * @internal
 */
export function removeJointAttributesFromOneSidedEdges(context is Context, sheetMetalModels is Query) returns array
{
    var modifiedEdges = [];
    for (var edge in evaluateQuery(context, qEdgeTopologyFilter(qOwnedByBody(sheetMetalModels, EntityType.EDGE), EdgeTopology.ONE_SIDED)))
    {
        if (size(getSmObjectTypeAttributes(context, edge, SMObjectType.JOINT)) > 0)
        {
            modifiedEdges = append(modifiedEdges, edge);
        }
    }
    removeAttributes(context, {
                "entities" : qUnion(modifiedEdges),
                "attributePattern" : { "objectType" : SMObjectType.JOINT } as SMAttribute
            });
    return modifiedEdges;
}

/**
 * @internal
 */
 export function getInitialEntitiesAndAttributes(context is Context, affectedSmBodies is Query) returns map
 {
    const originalEntities = evaluateQuery(context, qOwnedByBody(affectedSmBodies));
    var trackingOriginalEntities = [];
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V817_SM_OPTIMIZATION))
    {
        //Track non-object entities (e.g. laminar edges), no need to process their attributes if tracking
        //resolves to a single entity.
        const allEntities = qOwnedByBody(affectedSmBodies);
        const allIdentitiesWithSMAttributes = qAttributeFilter(allEntities, asSMAttribute({}));
        trackingOriginalEntities = startTrackingIdentityBatched(context, qSubtraction(allEntities, allIdentitiesWithSMAttributes));
    }
    const initialAssociationAttributes = getSMAssociationAttributes(context, qUnion(originalEntities));

    return { "originalEntities" : originalEntities,
             "initialAssociationAttributes" : initialAssociationAttributes,
             "originalEntitiesTracking" : trackingOriginalEntities};
 }

 /**
 * @internal
 */
export function mergeSheetMetal(context is Context, booleanId is Id, args is map)
precondition
{
    args.topLevelId is Id;
    args.surfacesToAdd is Query;
    args.originalSurfaces is Query;
    args.matches == undefined || args.matches is array;
    args.accuracy == undefined || args.accuracy is BooleanAccuracy;
    args.trackingBendEdges == undefined || args.trackingBendEdges is array;
    args.bendRadius == undefined || isLength(args.bendRadius);
    args.attributeIdCounter is box;
    args.error is ErrorStringEnum;
    args.errorParameters is array;
    args.legacyId == undefined || args.legacyId is Id;
}
{
    const trackedFaces = trackAllFaces(context, args.surfacesToAdd, args.originalSurfaces);
    const nOriginalBodies = size(evaluateQuery(context, args.originalSurfaces));
    const allSurfaces = qUnion(evaluateQuery(context, qUnion([args.originalSurfaces, args.surfacesToAdd])));
    try
    {
        opBoolean(context, booleanId, {
            "allowSheets" : true,
            "tools" : allSurfaces,
            "operationType" : BooleanOperationType.UNION,
            "makeSolid" : false,
            "eraseImprintedEdges" : false,
            "matches" : args.matches,
            "accuracy" : args.accuracy
        });
    }
    catch
    {
        // Error display
        processSubfeatureStatus(context, args.topLevelId, {"subfeatureId" : booleanId, "propagateErrorDisplay" : true});
        setErrorEntities(context, args.topLevelId, { "entities" : args.surfacesToAdd });
        //cleanup of all new surfaces should happen in abortFeature
        throw regenError(args.error, args.errorParameters);
    }

    // we could check for no-op info here but it won't catch cases where some surfaces could be joined and others couldnt
    // also check if boolean created an extra body trying to avoid non-manifold geometry
    if (size(evaluateQuery(context, allSurfaces)) != nOriginalBodies || !isQueryEmpty(context, qCreatedBy(booleanId, EntityType.BODY)))
    {
        const useId = (args.legacyId == undefined) ? args.topLevelId : args.legacyId;
        setErrorEntities(context, useId, { "entities" : allSurfaces });
        //cleanup of all new surfaces should happen in abortFeature
        throw regenError(args.error, args.errorParameters);
    }

    //check that none got split
    for (var trackedFace in trackedFaces.allFaces)
    {
        var evaluatedFaces = evaluateQuery(context, qEntityFilter(trackedFace, EntityType.FACE));
        if (size(evaluatedFaces) > 1)
        {
            var newFaces = qEntityFilter(qUnion(trackedFaces.newFaces), EntityType.FACE);
            var newEdges = qAdjacent(qUnion(trackedFaces.newFaces), AdjacencyType.EDGE, EntityType.EDGE);
            setErrorEntities(context, args.topLevelId, { "entities" : qUnion([newFaces, newEdges])});
            throw regenError(ErrorStringEnum.SHEET_METAL_SELF_INTERSECTING_MODEL, args.errorParameters);
        }
    }

    if (args.trackingBendEdges != undefined)
    {
        for (var bendEdge in args.trackingBendEdges)
        {
            var bendAttribute = createBendAttribute(context, args.topLevelId, bendEdge,
                                                    toAttributeId(args.topLevelId + args.attributeIdCounter[]), args.bendRadius, false);
            if (bendAttribute != undefined)
            {
                setAttribute(context, {"entities" : bendEdge, "attribute" : bendAttribute});
                args.attributeIdCounter[] += 1;
            }
        }
    }

    //add rips to new interior edges
    for (var entity in evaluateQuery(context, qOwnedByBody(allSurfaces, EntityType.EDGE)))
    {
        var attributes = getAttributes(context, {"entities" : entity, "attributePattern" : asSMAttribute({})});
        if (size(attributes) == 0)
        {
            var jointAttribute = makeNewJointAttributeIfNeeded(context, entity,  toAttributeId(args.topLevelId + args.attributeIdCounter[]));
            if (jointAttribute != undefined)
            {
                setAttribute(context, {"entities" : entity, "attribute" : jointAttribute});
                args.attributeIdCounter[] += 1;
            }
        }
    }
}


function trackAllFaces(context is Context, newSurfaces is Query, originals is Query)
{
    var trackedFacesNew = [];
    for (var face in evaluateQuery(context, qOwnedByBody(newSurfaces, EntityType.FACE)))
    {
        trackedFacesNew = append(trackedFacesNew, qUnion([face, startTracking(context, face)]));
    }
    var trackedFaces = trackedFacesNew;
    for (var face in evaluateQuery(context, qOwnedByBody(originals, EntityType.FACE)))
    {
        trackedFaces = append(trackedFaces, qUnion([face, startTracking(context, face)]));
    }
    return {"allFaces" : trackedFaces, "newFaces" : trackedFacesNew};
}

/**
 * @internal
 * For flange and hem, take initial input selections and find the associated definition edges. Filter out non-flangeable
 * or non-hemmable edges, and highlight those selections in red. If some edges were filtered out, but some remain,
 * display a feature warning.  If all the edges were filtered out, throw a feature error. Return the remaining edges.
 */
export function getSMDefinitionEdgesForFlangeTypeFeature(context is Context, topLevelId is Id, args is map) returns array
precondition
{
    args.edges is Query;
    args.errorForNoEdges is ErrorStringEnum;
    args.errorForInternal is ErrorStringEnum;
    args.errorForNonLinearEdges is ErrorStringEnum;
    args.errorForEdgesNextToCylinderBend is ErrorStringEnum;
    args.improveConsistency == undefined || args.improveConsistency is boolean; // version flag for flange.  undefined means true.
}
{
    args.improveConsistency = (args.improveConsistency == undefined) ? true : args.improveConsistency;

    if (isQueryEmpty(context, args.edges))
    {
        throw regenError(args.errorForNoEdges, ["edges"]);
    }

    var definitionEntities = qUnion(getSMDefinitionEntities(context, args.edges));

    // ----- Filter out any edges that cannot be used and display them with a warning -----
    var errorEdges = new box([]);
    var errorMessages = new box([]);

    // Set up a lambda function which augments errorEdges and errorMessages if the input errorEdgeQ parameter
    // resolves to anything.
    const addToErrorArraysIfNecessary = function(errorEdgeQ is Query, errorMessage is ErrorStringEnum)
                                        {
                                            if (!isQueryEmpty(context, errorEdgeQ))
                                            {
                                                errorEdges[] = append(errorEdges[], errorEdgeQ);
                                                errorMessages[] = append(errorMessages[], errorMessage);
                                            }
                                        };

    // -- Non-edges --
    // This should never be hit by a UI user, as flange and hem filter on SheetMetalDefinitionEntityType.EDGE, but just
    // in case, we should catch it.
    const nonEdges = qSubtraction(definitionEntities, qEntityFilter(definitionEntities, EntityType.EDGE));
    if (args.improveConsistency)
    {
        addToErrorArraysIfNecessary(nonEdges, args.errorForInternal);
    }
    const definitionEdges = qSubtraction(definitionEntities, nonEdges);

    // -- Non-linear edges --
    const nonLinearEdges = qSubtraction(definitionEdges, qGeometry(definitionEdges, GeometryType.LINE));
    addToErrorArraysIfNecessary(nonLinearEdges, args.errorForNonLinearEdges);

    if (args.improveConsistency)
    {
        // -- Two-sided edges --
        var twoSidedEdges = qEdgeTopologyFilter(definitionEdges, EdgeTopology.TWO_SIDED);
        addToErrorArraysIfNecessary(twoSidedEdges, args.errorForInternal);

        // -- Edges next to a cylinder bend --
        // It is possible to select these because of rolled hem and other operations; we do not currently support
        // bend-on-bend, but this may become available in the future.
        const adjacentCylinderFaces = qGeometry(qAdjacent(definitionEdges, AdjacencyType.EDGE, EntityType.FACE), GeometryType.CYLINDER);
        const adjacentCylinderBends = qAttributeFilter(adjacentCylinderFaces, asSMAttribute({ 'objectType' : SMObjectType.JOINT }));
        const edgesTouchingCylinderBend = qIntersection([definitionEdges, qAdjacent(adjacentCylinderBends, AdjacencyType.EDGE, EntityType.EDGE)]);
        addToErrorArraysIfNecessary(edgesTouchingCylinderBend, args.errorForEdgesNextToCylinderBend);
    }

    // Filter the error edges out from the selection, and display the corresponding selection
    const foundErrorEdges = (errorEdges[] != []);
    if (foundErrorEdges)
    {
        const originalSelections = getSelectionsForSMDefinitionEntities(context, qUnion(errorEdges[]), args.edges);
        setErrorEntities(context, topLevelId, { "entities" : originalSelections });
        definitionEntities = qSubtraction(definitionEntities, qUnion(errorEdges[]));
    }

    const firstError = foundErrorEdges ? errorMessages[][0] : undefined;

    const remainingDefinitionEntitiesArr = evaluateQuery(context, definitionEntities);
    if (remainingDefinitionEntitiesArr == [])
    {
        // If there are no edges left (or there were no active edges to begin with), fail.
        const message = foundErrorEdges ? firstError : ErrorStringEnum.SHEET_METAL_ACTIVE_EDGE_NEEDED;
        throw regenError(message, ["edges"]);
    }
    else if (foundErrorEdges)
    {
        // If there are some edges left, but we had to filter some out, display a warning
        // This should be warning, not info, as it can be caused by an upstream change (e.x. using make joint to change
        // a free edge to a rip)
        reportFeatureWarning(context, topLevelId, firstError);
    }

    return remainingDefinitionEntitiesArr;
}

/**
 * Map a group of sheet metal definition entities back to the original entities selected by the user.
 */
export function getSelectionsForSMDefinitionEntities(context is Context, definitionEntities is Query, originalSelections is Query)
{
    var associationAttributes = getSMAssociationAttributes(context, definitionEntities);
    const associatedEntities = mapArray(associationAttributes, function(attribute) { return qAttributeQuery(attribute); });
    return qIntersection([qUnion(associatedEntities), originalSelections]);
}

