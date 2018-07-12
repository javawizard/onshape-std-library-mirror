FeatureScript 860; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "860.0");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "860.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "860.0");
import(path : "onshape/std/containers.fs", version : "860.0");
import(path : "onshape/std/coordSystem.fs", version : "860.0");
import(path : "onshape/std/curveGeometry.fs", version : "860.0");
import(path : "onshape/std/evaluate.fs", version : "860.0");
import(path : "onshape/std/feature.fs", version : "860.0");
import(path : "onshape/std/math.fs", version : "860.0");
import(path : "onshape/std/manipulator.fs", version : "860.0");
import(path : "onshape/std/query.fs", version : "860.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "860.0");
import(path : "onshape/std/smobjecttype.gen.fs", version : "860.0");
import(path : "onshape/std/string.fs", version : "860.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "860.0");
import(path : "onshape/std/tool.fs", version : "860.0");
import(path : "onshape/std/valueBounds.fs", version : "860.0");
import(path : "onshape/std/vector.fs", version : "860.0");
import(path : "onshape/std/topologyUtils.fs", version : "860.0");
import(path : "onshape/std/transform.fs", version : "860.0");



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
 export function updateSheetMetalGeometry(context is Context, id is Id, args is map)
 {
    adjustCornerBreakAttributes(context, args.entities);
    @updateSheetMetalGeometry(context, id, args);
 }

/**
 * @internal
 * Used in boolean and sheet metal cut pattern to extend through the sheet body at a distance greater than boolean
 * tolerance.
 */
export const SM_THIN_EXTENSION = 1.e-4 * meter;

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
    bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited": false };
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
        if (!cylinderCanBeBend(context, face))
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
    assignSmAssociationAttributes(context, qUnion([args.surfaceBodies, facesQ, edgesQ, verticesQ]));
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

function cylinderCanBeBend(context is Context, face is Query) returns boolean
{
    var lineEdgesQ = qGeometry(qEdgeAdjacent(face, EntityType.EDGE), GeometryType.LINE);
    var lineEdges = evaluateQuery(context, lineEdgesQ);
    if (size(lineEdges) < 2)
    {
        return false;
    }
    var countOtherFaces = 0;
    var allowCutBends = isAtVersionOrLater(context, FeatureScriptVersionNumber.V775_DETACHED_FILLET);
    for (var edge in lineEdges)
    {
        var otherFaceQ = qSubtraction(qEdgeAdjacent(edge, EntityType.FACE), face);
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
    return (allowCutBends) ? (countOtherFaces >= 2) : true;
}

/**
 * Compute angle between face normals at edge mid point.
 */
export function edgeAngle(context is Context, edge is Query) returns ValueWithUnits
{
    var faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
    if (size(faces) != 2)
    {
        throw "Expects 2-sided edge";
    }
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V684_SM_SWEPT_SUPPORT))
    {
        var normal0 = evFaceNormalAtEdge(context, { "edge" : edge, "face" : faces[0], "parameter" : 0.5 });
        var normal1 = evFaceNormalAtEdge(context, { "edge" : edge, "face" : faces[1], "parameter" : 0.5 });
        return angleBetween(normal0, normal1);
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
    var faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
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
export function updateJointAngle(context is Context, id is Id, edges is Query)
{
    var moreFlexibleUpdate = isAtVersionOrLater(context, FeatureScriptVersionNumber.V695_SM_SWEPT_SUPPORT);
    var insertNewAttributeIfNecessary = isAtVersionOrLater(context, FeatureScriptVersionNumber.V704_MOVE_FACE_ROLLED_SM);

    var newAttributeCounter = 0;
    for (var edge in evaluateQuery(context, edges))
    {
        const jointAttribute = try silent(getJointAttribute(context, edge));
        if (jointAttribute == undefined)
        {
            if (insertNewAttributeIfNecessary)
            {
                const newJointAttribute = makeNewJointAttributeIfNeeded(context, edge, toAttributeId(id + newAttributeCounter));
                if (newJointAttribute != undefined)
                {
                    setAttribute(context, {"entities" : edge, "attribute" : newJointAttribute});
                    newAttributeCounter += 1;
                }
            }
            continue;
        }
        var replacementAttribute = (moreFlexibleUpdate) ? computeReplacementAttribute(context, id, edge, jointAttribute) :
                                                legacyComputeReplacementAttribute(context, edge, jointAttribute);

        if (replacementAttribute == undefined)
        {
            continue;
        }
        replaceSMAttribute(context, jointAttribute, replacementAttribute);
    }
}

function computeReplacementAttribute(context is Context, id is Id, edge is Query, jointAttribute is map)
{
    if (!edgeIsTwoSided(context, edge)) // can not continue as joint
    {
        clearSmAttributes(context, edge);
        return undefined;
    }

    var angleVal = try silent(edgeAngle(context, edge));
    if (jointAttribute.jointType.value == SMJointType.BEND &&
        jointAttribute.bendRadius != undefined)
    {
       angleVal = try silent(bendAngle(context, id, edge, jointAttribute.bendRadius.value));
    }

    if ((angleVal == undefined && jointAttribute.angle == undefined) ||
         try silent(tolerantEquals(angleVal, jointAttribute.angle.value)) == true) // nothing changed
    {
        return undefined;
    }

    var replacementAttribute = jointAttribute;

    if (angleVal == undefined || abs(angleVal) < TOLERANCE.zeroAngle * radian)
    {
        replacementAttribute.jointStyle = undefined;
        replacementAttribute.jointType = { "value" : SMJointType.RIP, "canBeEdited" : false };
    }
    else if (replacementAttribute.jointType != undefined && replacementAttribute.jointType.value == SMJointType.RIP)
    {
        replacementAttribute.jointStyle = { "value" : SMJointStyle.EDGE, "canBeEdited" : true };
        replacementAttribute.jointStyle.canBeEdited = true;
    }

    replacementAttribute.angle = { "value" : angleVal, "canBeEdited" :
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

/**
 * A `LengthBoundSpec` for thickness in sheet metal features. default to `(1/16)"` (i.e. steel)
 */
export const SM_THICKNESS_BOUNDS =
{
    (meter)      : [1e-5, 0.0016, 500],
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
    (meter)      : [1e-5, 0.005 , 500],
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
    if (size(evaluateQuery(context, partQuery)) == 0)
    {
        return undefined;
    }
    const attributes = getAttributes(context, {
                                    "entities" : partQuery,
                                    "attributePattern" : {} as SMAssociationAttribute
                                    });
    if (size(attributes) == 0)
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
    var nonSheetMetal = separateSheetMetalQueries(context, query).nonSheetMetalQueries;
    return size(evaluateQuery(context, nonSheetMetal)) > 0;
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
    var entityAssociations = getAttributes(context, {
            "entities" : partQuery,
            "attributePattern" : {} as SMAssociationAttribute
        });
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
 * @internal
 * initialData is computed by a call to getInitialEntitiesAndAttributes at the beginning of the feature
 */
export function assignSMAttributesToNewOrSplitEntities(context is Context, sheetMetalModels is Query,
                                            initialData is map) returns map
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

    const attributes = getAttributes(context, { "entities" : entitiesToAddAssociationsQ,
                "attributePattern" : {} as SMAssociationAttribute });
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
                            definitionAttributes[0].jointType.value == SMJointType.BEND &&
                            try silent(edgeIsTwoSided(context, edgeQ)) == true)
                        {
                            const jointAttributeId = definitionAttributes[0].attribute_id ~ ".rip";
                            const ripAttribute = createRipAttribute(context, edgeQ, jointAttributeId, SMJointStyle.EDGE, {});
                            setAttribute(context, {"entities" : edgeQ, "attribute" : ripAttribute});
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
    var entitiesWithExistingAttributesQ = qAttributeFilter(entitiesToAddAssociationsQ, {} as SMAssociationAttribute);

    var entitiesThatNeedAssociationQ = qSubtraction(entitiesToAddAssociationsQ, entitiesWithExistingAttributesQ);
    assignSmAssociationAttributes(context, entitiesThatNeedAssociationQ);

    var finalAssociationAttributes = getAttributes(context, {
            "entities" : qOwnedByBody(sheetMetalModels), // counting on body transient query surviving
            "attributePattern" : {} as SMAssociationAttribute
    });
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
            return cylinderCanBeBend(context, qGeometry(entity, GeometryType.CYLINDER));
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
                size(evaluateQuery(context, qGeometry(qEdgeAdjacent(entity, EntityType.FACE), GeometryType.PLANE))) == 2)
            {
                return false;
            }
            return zeroAngle;
        }
        if (attribute.jointType.value == SMJointType.RIP && zeroAngle && nLines == 1)
        {
            const faceBendAttributes = getSmObjectTypeAttributes(context, qEdgeAdjacent(entity, EntityType.FACE), SMObjectType.JOINT);
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
 *
 * @return {{
 *      @field sheetMetalQueries {Query} : `targets` which are part of an active sheet metal model
 *      @field nonSheetMetalQueries {Query} : `targets` which are not part of an active sheet metal model
 * }}
 */
export function separateSheetMetalQueries(context is Context, targets is Query) returns map
{
    var sheetMetalEntities = [];
    var nonSheetMetalEntities = [];
    for (var entity in evaluateQuery(context, targets))
    {
        if (queryContainsActiveSheetMetal(context, entity))
        {
            sheetMetalEntities = append(sheetMetalEntities, entity);
        }
        else
        {
            nonSheetMetalEntities = append(nonSheetMetalEntities, entity);
        }
    }
    return { "sheetMetalQueries" : qUnion(sheetMetalEntities), "nonSheetMetalQueries" : qUnion(nonSheetMetalEntities) };
}

/**
 * @internal
 */
export function checkNotInFeaturePattern(context is Context, references is Query, error is ErrorStringEnum)
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V675_MORE_TAB_FIXES))
    {
        if (@isInFeaturePattern(context))
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
 * Used in importDerived to strip sheet metal related data off the imported context
 * returns query of all sheet metal parts (3d and flattened)
 */
export function clearSheetMetalData(context, id) returns Query
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

    // Solid bodies 3d and Flat and only they are associated with sheet bodies
    var associationAttributes = getAttributes(context, {"entities" : smModelsQ, "attributePattern" : {} as SMAssociationAttribute});
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

        associationAttributes = getAttributes(context, {"entities" : qUnion([smEdgeQ, smCylinderQ]), "attributePattern" : {} as SMAssociationAttribute});
        for (var attribute in associationAttributes)
        {
            smPartNBendLineQArr = append(smPartNBendLineQArr, qEntityFilter(qBodyType(qAttributeQuery(attribute), BodyType.WIRE), EntityType.BODY));
        }
    }


    var smPartNBendLineQEvaluated = evaluateQuery(context, qUnion(smPartNBendLineQArr));

    // remove all SMAttributes
    removeAttributes(context, {
        "attributePattern" : asSMAttribute({})
    });

    // Deactivating active sheet metal models
    if (size(smModelsActiveEvaluated) > 0)
    {
        updateSheetMetalGeometry(context, id, { "entities" : qUnion(smModelsActiveEvaluated) });
    }

    // remove all SMAssociationAttribute
    removeAttributes(context, {
        "attributePattern" : {} as SMAssociationAttribute
    });

    // Deleting all sheet bodies
    opDeleteBodies(context, id + "deleteSheetBodies", {
            "entities" : qUnion(smModelsEvaluated)
    });

   return qUnion(smPartNBendLineQEvaluated);
}

/**
 * @internal
 */
export function makeNewJointAttributeIfNeeded(context is Context, edge is Query, attributeId is string)
{
    var faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
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
        sheetVertices = qUnion(getSMDefinitionEntities(context, qVertexAdjacent(qEntityFilter(entity, EntityType.EDGE), EntityType.VERTEX)));
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
    var adjacentFaces = qEdgeAdjacent(edgeQ, EntityType.FACE);
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V589_STABLE_BREAK_REMOVAL))
    {
        if (size(evaluateQuery(context, adjacentFaces)) == 0)
        {
            return;
        }
    }

    var wallIds = [];
    for (var wallAttribute in getSmObjectTypeAttributes(context, adjacentFaces, SMObjectType.WALL))
    {
        wallIds = append(wallIds, wallAttribute.attributeId);
    }
    for (var vertexQ in evaluateQuery(context, qVertexAdjacent(edgeQ, EntityType.VERTEX)))
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
export function sheetMetalExtendSheetBodyCall(context is Context, id is Id, definition is map)
{
    var vertexToTrackingAndAttribute = collectAttributesOfAdjacentVertices(context, definition.entities);
    var edgeToTrackingAndAssociation = removeAssociationsFromFreeEdges(context, definition.entities);
    opExtendSheetBody(context, id, definition);
    restoreAssociations(context, edgeToTrackingAndAssociation);
    adjustCornerBreakAttributes(context, id, vertexToTrackingAndAttribute);
}

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
    var adjacentEdgesQ = qVertexAdjacent(edgesIn, EntityType.EDGE);
    for (var edge in evaluateQuery(context, adjacentEdgesQ))
    {
        if (edgeIsTwoSided(context, edge) || size(getAttributes(context, {
                "entities" : edge,
                "attributePattern" : asSMAttribute({})})) > 0)
        {
            continue;
        }
        var associations = getAttributes(context, {
                "entities" : edge,
                "attributePattern" : {} as SMAssociationAttribute});
        if (size(associations) == 1)
        {
            edgeToTrackingAndAssociation[edge] = {
                "tracking" : startTracking(context, { 'subquery' : edge,
                                                      'trackPartialDependency' : true}),
                "association" : associations[0]};
            removeAttributes(context, {
                "entities" : edge,
                "attributePattern" : {} as SMAssociationAttribute});
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
            var associations = getAttributes(context, {
                "entities" : edge,
                "attributePattern" : {} as SMAssociationAttribute});
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
    for (var vertexQ in evaluateQuery(context, qVertexAdjacent(edgesIn, EntityType.VERTEX)))
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
            var wallsAroundVertex = getSmObjectTypeAttributes(context, qVertexAdjacent(vert, EntityType.FACE), SMObjectType.WALL);
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
 * A function for getting associated sheet metal entities outside of a sheet metal feature.
 */
export function getSMDefinitionEntities(context is Context, selection is Query, entityType is EntityType) returns array
{
    var entityAssociations = try silent(getAttributes(context, {
                "entities" : qBodyType(selection, BodyType.SOLID),
                "attributePattern" : {} as SMAssociationAttribute
            }));
    var out = [];
    if (entityAssociations != undefined)
    {
        for (var attribute in entityAssociations)
        {
            const modelQuery = qAttributeQuery(asSMAttribute({ "objectType" : SMObjectType.MODEL }));
            const associatedEntities = evaluateQuery(context, qIntersection([qAttributeQuery(attribute), qOwnedByBody(modelQuery, entityType)]));
            const ownerBody = qOwnerBody(qUnion(associatedEntities));
            const isActive = try silent(isAtVersionOrLater(context, FeatureScriptVersionNumber.V522_MOVE_FACE_NONPLANAR) ?
                    isSheetMetalModelActive(context, ownerBody) : isSheetMetalModelActive(context, modelQuery));
            const returnInactive = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V495_MOVE_FACE_ROTATION_AXIS);
            if ((isActive != undefined && isActive) || returnInactive)
            {

                out = concatenateArrays([out, associatedEntities]);
            }
        }
    }
    return out;
}

/**
 * Returns an array of sm models associated with selection in a way that works outside of sheet metal features.
 */
export function getOwnerSMModel(context is Context, selection is Query) returns array
{
    var entityAssociations = try silent(getAttributes(context, {
                "entities" : qBodyType(selection, BodyType.SOLID),
                "attributePattern" : {} as SMAssociationAttribute
            }));
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
    return out;
}

/**
 * Returns a query for all sheet metal part entities of entityType related to the input sheet metal model entities.
 */
export function getSMCorrespondingInPart(context is Context, selection is Query, entityType is EntityType) returns Query
{
    var entityAssociations = getAttributes(context, {
            "entities" : selection,
            "attributePattern" : {} as SMAssociationAttribute
        });
    var out = [];
    for (var attribute in entityAssociations)
    {
        var associatedEntities = evaluateQuery(context, qBodyType(qAttributeQuery(attribute), BodyType.SOLID));
        out = concatenateArrays([out, associatedEntities]);
    }

    var corresponding = qEntityFilter(qUnion(out), entityType);
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
        for (var vertex in evaluateQuery(context, qVertexAdjacent(wall, EntityType.VERTEX)))
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
    for (var edge in evaluateQuery(context, qEdgeTopologyFilter(qOwnedByBody(sheetMetalModels, EntityType.EDGE), EdgeTopology.LAMINAR)))
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
        for (var ent in evaluateQuery(context, qOwnedByBody(affectedSmBodies)))
        {
            if (size(getAttributes(context, {
                    "entities" : ent,
                    "attributePattern" : asSMAttribute({})
                    })) == 0)
            {
                trackingOriginalEntities = append(trackingOriginalEntities, startTrackingIdentity(context, ent));
            }
        }
    }
    const initialAssociationAttributes = getAttributes(context, {
                "entities" : qUnion(originalEntities),
                "attributePattern" : {} as SMAssociationAttribute });

    return { "originalEntities" : originalEntities,
             "initialAssociationAttributes" : initialAssociationAttributes,
             "originalEntitiesTracking" : trackingOriginalEntities};
 }

