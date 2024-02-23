FeatureScript 2279; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

export import(path : "onshape/std/chamfertype.gen.fs", version : "2279.0");
export import(path : "onshape/std/hole.fs", version : "2279.0");
export import(path : "onshape/std/holeAttribute.fs", version : "2279.0");
export import(path : "onshape/std/holesectionfacetype.gen.fs", version : "2279.0");
export import(path : "onshape/std/moveFace.fs", version : "2279.0");
export import(path : "onshape/std/query.fs", version : "2279.0");
export import(path : "onshape/std/tool.fs", version : "2279.0");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "2279.0");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "2279.0");
import(path : "onshape/std/containers.fs", version : "2279.0");
import(path : "onshape/std/string.fs", version : "2279.0");
import(path : "onshape/std/debug.fs", version : "2279.0");
import(path : "onshape/std/coordSystem.fs", version : "2279.0");
import(path : "onshape/std/curveGeometry.fs", version : "2279.0");
import(path : "onshape/std/evaluate.fs", version : "2279.0");
import(path : "onshape/std/feature.fs", version : "2279.0");
import(path : "onshape/std/lookupTablePath.fs", version : "2279.0");
import(path : "onshape/std/holetables.gen.fs", version : "2279.0");
import(path : "onshape/std/primitives.fs", version : "2279.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2279.0");
import(path : "onshape/std/splitpart.fs", version : "2279.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2279.0");
import(path : "onshape/std/valueBounds.fs", version : "2279.0");
import(path : "onshape/std/vector.fs", version : "2279.0");



/** @internal */
export enum DepthType
{
    annotation { "Name" : "Blind" }
    Blind,
    annotation { "Name" : "Up to next" }
    UpToNext
}

const UNDERCUT_DEPTH_OFFSET = .015 * inch;
const CHAMFER_LENGTH_OFFSET = .0075 * inch;
const CHAMFER_ANGLE_BOUNDS_EXTERNAL_THREAD =
{
    (degree) : [0.1, 30, 89],
    (radian) : PI / 6
} as AngleBoundSpec;

const EXTERNAL_THREAD_DEPTH_BOUNDS =
{
            (millimeter) : [0.1, 10.0, 200000],
            (centimeter) : 1.0,
            (meter) : 0.01,
            (inch) : 0.5,
            (foot) : 0.04,
            (yard) : 0.014
        } as LengthBoundSpec;

/**
 * All of the external thread standards
 */
const localExternalThreadTable =
{
        "name" : "standard",
        "displayName" : "Standard",
        "entries" : {
            "ANSI" : ANSI_ExternalThreadTable,
            "ISO" : ISO_ExternalThreadTable
        }
    };

const MINIMUM_ARC_SWEEP = 182;
/**
 * Given a list of edge or selections and an optional offset, find the cylindrical faces corresponding to them
 * split the face at the offset, and add external thread size data.
 */
annotation { "Feature Type Name" : "External thread", "Editing Logic Function" : "editFeatureLogicExternalThread" }
export const externalThread = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Cylinder edges", "UIHint" : UIHint.UNCONFIGURABLE, "Filter" : ((GeometryType.CIRCLE || GeometryType.ARC) && SketchObject.NO && EdgeTopology.TWO_SIDED && ConstructionObject.NO && ModifiableEntityOnly.YES && AllowMeshGeometry.NO && ActiveSheetMetal.NO) }
        definition.entities is Query;

        annotation { "Name" : "Standard", "Lookup Table" : localExternalThreadTable, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "UNCONFIGURABLE"] }
        definition.branchOfStandard is LookupTablePath;

        annotation { "Name" : "End type", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE] }
        definition.depthType is DepthType;
        if (definition.depthType == DepthType.Blind)
        {
            annotation { "Name" : "Length", "Column Name" : "Thread length", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            isLength(definition.threadDepth, EXTERNAL_THREAD_DEPTH_BOUNDS);

        }

        annotation { "Name" : "Add chamfer", "Default" : false }
        definition.addChamfer is boolean;

        if (definition.addChamfer)
        {
            annotation { "Group Name" : "Chamfer", "Driving Parameter" : "addChamfer", "Collapsed By Default" : false }
            {
                annotation { "Name" : "Length", "Column Name" : "Chamfer length", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition.chamferWidth, BLEND_BOUNDS);

                annotation { "Name" : "Angle", "Column Name" : "Chamfer angle", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isAngle(definition.chamferAngle, CHAMFER_ANGLE_BOUNDS_EXTERNAL_THREAD);
            }
        }

        annotation { "Name" : "Add undercut", "Default" : false }
        definition.addUndercut is boolean;
        if (definition.addUndercut)
        {
            annotation { "Group Name" : "Undercut", "Driving Parameter" : "addUndercut", "Collapsed By Default" : false }
            {
                annotation { "Name" : "Diameter", "Column Name" : "Undercut diameter", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE] }
                isLength(definition.undercutDiameter, NONNEGATIVE_LENGTH_BOUNDS);

                annotation { "Name" : "Length", "Column Name" : "Undercut length", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE] }
                isLength(definition.undercutLength, NONNEGATIVE_LENGTH_BOUNDS);
            }
        }
        if (!definition.addUndercut && (definition.depthType == DepthType.Blind))
        {
            annotation { "Name" : "Split face", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE, "Default" : true }
            definition.splitFace is boolean;
        }

    }
    {
        if (isQueryEmpty(context, definition.entities))
        {
            throw regenError(ErrorStringEnum.SELECT_CYLINDER_EDGES, ["entities"]);
        }
        checkExistingExternalThread(context, definition.entities);
        const entityList = checkAndSplitAllShaftFaces(context, id, definition);
        checkNonMatchingSize(context, id, definition, entityList[0].diameter);
        const attributes = addExternalThreadAttributes(context, id, definition, entityList);
        const chamfers = addChamfers(context, id + "Chamfers", definition, entityList);
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1904_ADD_HOLE_ATTRIBUTES_TO_CHAMFER_FEATURES))
        {
            for (var chamferIndex, chamfer in chamfers) // Add attributes to newly created chamfer faces as well
            {
                checkExistingExternalThread(context, chamfer);
                var updatedAttribute = attributes[chamferIndex];
                updatedAttribute.sectionFace = { "type" : HoleSectionFaceType.EXTERNAL_THREAD_CHAMFER_FACE };
                setAttribute(context, { "entities" : chamfer, "attribute" : updatedAttribute});
            }
        }
        if (definition.addUndercut)
        {
            const minorDiameter = getMinorDiameter(definition);
            const undercutDiameterTooLarge = definition.undercutDiameter > minorDiameter - TOLERANCE.zeroLength * meter;
            if (undercutDiameterTooLarge)
            {
                throw regenError(ErrorStringEnum.UNDERCUT_DIAMETER_TOO_LARGE, ["undercutDiameter"]);
            }
        }

        const perEdgeFunctionHighlight = function(endEdgeQuery is Query, innerId is Id) {
            const qAdjacentCylinders = qAdjacent(endEdgeQuery, AdjacencyType.EDGE, EntityType.FACE)->qGeometry(GeometryType.CYLINDER);
            setHighlightedEntities(context, {"entities": qAdjacentCylinders});
        };
         forEachEntity(context, id + "highlight", definition.entities, perEdgeFunctionHighlight);

    }, { keepTools : false, splitType : SplitType.PART, useTrimmed : false, keepBothSides : true, keepFront : true, keepToolSurfaces : true });



function shouldTapThrough(context is Context, shaftRadius is ValueWithUnits, facesAdjacentToSelectedEdge is Query, shaftFace is Query, minorThreadDiameter is ValueWithUnits) returns boolean
{
     if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V1918_THREADS_TAPPED_THRU_FOR_CHAMFER_CASE_v2))
     {
        return false;
     }
    // 3 special cases to determine if an up to next thread should tap thorugh all or not

    const shaftAdjacentFaces = qAdjacent(shaftFace, AdjacencyType.EDGE, EntityType.FACE);
    const farEndFace = qSubtraction(shaftAdjacentFaces, facesAdjacentToSelectedEdge);
    if (size(evaluateQuery(context, farEndFace)) != 1)
    {
        return false;
    }
    const farEndFaceSurface = evSurfaceDefinition(context, {"face" : farEndFace});
    // 1) Test if face on far end of shaft is a chamfer
    const farEndFaceIsConical = (farEndFaceSurface is Cone);
    if (!farEndFaceIsConical)
    {
        return false;
    }
    const shaftFarEndEdges = qIntersection(
        qAdjacent(farEndFace, AdjacencyType.EDGE, EntityType.EDGE),
        qAdjacent(shaftFace,AdjacencyType.EDGE, EntityType.EDGE ));
    const shaftFarEndEdge = qUnion(qGeometry(shaftFarEndEdges, GeometryType.ARC), qGeometry(shaftFarEndEdges, GeometryType.CIRCLE));

    if (size(evaluateQuery(context, shaftFarEndEdge)) != 1)
    {
        return false;
    }

    // 2) Test the edge convexity of of the "far" end edge of the shaft
    const shaftFarEndEdgeIsConvex = evEdgeConvexity(context, { "edge" : shaftFarEndEdge }) == EdgeConvexityType.CONVEX;
    if (shaftFarEndEdgeIsConvex == false)
    {
        return false;
    }

    // 3) Check the diameter of the far ending face not shared with the shaft.
    const farEndEdges = qAdjacent(farEndFace, AdjacencyType.EDGE, EntityType.EDGE);
    const nonShaftFarEndEdge = qSubtraction(farEndEdges, shaftFarEndEdge);
    if (size(evaluateQuery(context, nonShaftFarEndEdge)) != 1)
    {
        return false;
    }
    const nonShaftFarEndEdgeRadius = evCurveDefinition(context, {
            "edge" : nonShaftFarEndEdge,
            "returnBSplinesAsOther" : true
        }).radius;
    if (nonShaftFarEndEdgeRadius == undefined)
    {
        return false;
    }
    return (nonShaftFarEndEdgeRadius * 2) < minorThreadDiameter - (TOLERANCE.zeroLength * meter);
}

/**
 * From a starting edge selection, get data about a valid cylindrical face to annotation and/or cut and data about the direction to
 * cut.
 */
export function getSplitData(context is Context, topLevelId is Id, endEdge is Query, minorDiameter is ValueWithUnits) returns map
{
    const facesAdjacentToSelectedEdge = qAdjacent(endEdge, AdjacencyType.EDGE, EntityType.FACE);
    var endcapSurface;
    var shaftQuery;
    var chamferSurface;
    for (var face in evaluateQuery(context, facesAdjacentToSelectedEdge))
    {
        const currentSurface = evSurfaceDefinition(context, { "face" : face });
        const isCylinder = !isQueryEmpty(context, qGeometry(face, GeometryType.CYLINDER));
        const isPlanar = !isQueryEmpty(context, qGeometry(face, GeometryType.PLANE));
        const isCone = !isQueryEmpty(context, qGeometry(face, GeometryType.CONE));
        if (isCylinder)
        {
            shaftQuery = face;
        }
        else if (isPlanar)
        {
            endcapSurface = currentSurface;
        }
        else if (isCone)
        {
            chamferSurface = face;
        }
        else
        { // Existing filters should prevent this, but just to safeguard against corner cases that are not supported
            throw regenError(ErrorStringEnum.UNABLE_TO_FIND_THREAD_BOUNDARY);
        }
    }

    if ((shaftQuery == undefined) || ((endcapSurface == undefined) && (chamferSurface == undefined)))
    {
        throw regenError(ErrorStringEnum.UNABLE_TO_FIND_THREAD_BOUNDARY);
    }

    const cylinderDirection = computeCylinderAxis(context, shaftQuery);
    const edgeDirectionInfo = getEdgeDirectionInfo(context, shaftQuery, endEdge);

    if (!parallelVectors(cylinderDirection.direction, edgeDirectionInfo.edgeCoordSys.zAxis))
    {
        throw regenError(ErrorStringEnum.UNABLE_TO_FIND_THREAD_BOUNDARY);
    }

    const isConvexCylinderFace = isConvexCylinder(context, shaftQuery);
    const shaftRadius = evCurveDefinition(context, { "edge" :endEdge, "returnBSplinesAsOther" : true }).radius;

    const shouldTapThrough = shouldTapThrough(context, shaftRadius, facesAdjacentToSelectedEdge, shaftQuery, minorDiameter);

    const arcLength = evLength(context, { "entities" : endEdge });
    const arcAngle = (arcLength /  shaftRadius) * radian;
    const minRadians = (MINIMUM_ARC_SWEEP / 180.0) * PI * radian;
    const isValidArc = (arcAngle >= minRadians);

    var chamferDistance = 0 * meter;
    if (chamferSurface != undefined)
    {
        chamferDistance = calculateChamferLength(context, chamferSurface as Query);
    }

    const dimensions = queryDimensions(context, shaftQuery);
    const edgeCoordZ = edgeDirectionInfo.edgeCoordSys.zAxis * (edgeDirectionInfo.needsFlip? -1 : 1);
    const cylSurface = evSurfaceDefinition(context, { "face" : shaftQuery });
    const cylinderZ = cylSurface.coordSystem.zAxis;
    const dotPositive = dot(edgeCoordZ, cylinderZ) > 0;

    return {
            "isConvexCylinderFace" : isConvexCylinderFace,
            "isValidArc" : isValidArc,
            "endcapSurface" : endcapSurface,
            "chamferSurface" : chamferSurface,
            "edgeQuery" : endEdge,
            "shaftQuery" : shaftQuery,
            "length" : edgeDirectionInfo.length,
            "needsFlip" : edgeDirectionInfo.needsFlip,
            "edgeCoordSys" : edgeDirectionInfo.edgeCoordSys,
            "diameter" : dimensions.radius * 2,
            "chamferDistance" : chamferDistance,
            "cylinderAlignedWithThreadDirection": dotPositive,
            "shouldTapThrough" : shouldTapThrough
        };
}


/**
 * Find an selected edge's coordinate system and whether the direction to cut needs to be inverted.
 */
function getEdgeDirectionInfo(context is Context, shaft is Query, endEdge is Query)
{
    const coordSys = evCurveDefinition(context, { "edge" : endEdge, "returnBSplinesAsOther" : true }).coordSystem;
    const cylinderBox = calculateCylinderBoundingBox(context, shaft, coordSys);
    const needsFlip = (cylinderBox.maxCorner[2] <= TOLERANCE.zeroLength * meter);
    const length = abs(cylinderBox.maxCorner[2]) > abs(cylinderBox.minCorner[2]) ? abs(cylinderBox.maxCorner[2]) : abs(cylinderBox.minCorner[2]);
    return { "needsFlip" : needsFlip, "edgeCoordSys" : coordSys, "length" : length };
}

/**
 * Get axis of cylinder (taken from hole tag feature)
 */
function computeCylinderAxis(context is Context, faces is Query) returns Line
{
    return evAxis(context, { "axis" : faces });
}

/**
 * Calculate cylinder bounding box
 */
function calculateCylinderBoundingBox(context is Context, cylinderEntities is Query, coordSys is CoordSystem)
{
    const cylinderBox is Box3d = evBox3d(context, {
                "topology" : cylinderEntities,
                "cSys" : coordSystem(coordSys.origin, perpendicularVector(coordSys.zAxis), coordSys.zAxis),
                "tight" : true
            });
    return cylinderBox;
}

/**
 * Calculate chamfer length
 */
function calculateChamferLength(context is Context, chamferQ is Query)
{
    const chamferEdges = qUnion(
        qGeometry(qAdjacent(chamferQ, AdjacencyType.EDGE, EntityType.EDGE), GeometryType.ARC),
        qGeometry(qAdjacent(chamferQ, AdjacencyType.EDGE, EntityType.EDGE), GeometryType.CIRCLE)
    );

    const evChamferEdges = evaluateQuery(context, chamferEdges);
    if (size(evChamferEdges) != 2)
    {
        throw regenError(ErrorStringEnum.UNABLE_TO_FIND_THREAD_BOUNDARY);
    }

    const curve1 = evCurveDefinition(context, { "edge" : evChamferEdges[0], "returnBSplinesAsOther" : true });
    const curve2 = evCurveDefinition(context, { "edge" : evChamferEdges[1], "returnBSplinesAsOther" : true });

    if (!(curve1 is Circle) || !(curve2 is Circle))
    {
        throw regenError(ErrorStringEnum.UNABLE_TO_FIND_THREAD_BOUNDARY);
    }

    return norm(curve1.coordSystem.origin - curve2.coordSystem.origin);
}

/**
 * Get cylinder radius and length
 */
function queryDimensions(context is Context, aSurface is Query) returns map
{
    const surface = evSurfaceDefinition(context, {
                "face" : aSurface
            });
    return { "radius" : surface.radius };
}

/**
 * Check if a cylinder is convex
 */
function isConvexCylinder(context is Context, aSurface is Query)
{
    const surface = evSurfaceDefinition(context, {
                "face" : aSurface
            });

    if (!(surface is Cylinder || surface is Cone))
    { // The selection filter should take care of this, but just to be safe
        throw regenError(ErrorStringEnum.NOT_CYLINDER_OR_CONE);
    }
    const surfaceCoords = surface.coordSystem;
    const testTangentPlane = evFaceTangentPlane(context, {
                "face" : aSurface,
                "parameter" : vector(1, 1)
            });
    const normalOnCylinder = testTangentPlane.normal;
    const pointOnCylinder = testTangentPlane.origin;

    const cylAxis = computeCylinderAxis(context, aSurface);
    const pointOnAxis = project(cylAxis, pointOnCylinder);
    const vecProjectionToPoint = vector(pointOnAxis - pointOnCylinder);
    return dot(vecProjectionToPoint, normalOnCylinder) < 0;
}

/**
 * Main implementation -- gets info about each selection, performs split or undercut, and deletes temporary plane used as splitting tool
 */
function checkAndSplitAllShaftFaces(context is Context, topLevelId is Id, definition is map)
{
    const performSplit = ((definition.depthType == DepthType.Blind) && definition.splitFace);
    var firstDiameter is box = new box(undefined);
    var entityMapList is box = new box([]);
    const perEdgeFunction = function(endEdgeQuery is Query, innerId is Id)
        {
            if (queryContainsActiveSheetMetal(context, endEdgeQuery))
            {
                throw regenError(ErrorStringEnum.EXTERNAL_THREADS_UNSUPPORTED_ON_SHEET_METAL);
            }

            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1916_CHECK_INVALID_CYLINDER_END_SELECTION))
            {
                // Needed if the user selects the "wrong" edge of a cylinder that is bound by a face making the edge non-convex (not typical)
                const shaftNearEndEdgeIsConvex = evEdgeConvexity(context, { "edge" : endEdgeQuery }) == EdgeConvexityType.CONVEX;
                if (!shaftNearEndEdgeIsConvex)
                {
                    throw regenError(ErrorStringEnum.WRONG_CYLINDER_EDGE_SELECTED, ["entities"], endEdgeQuery);
                }
            }

            const minorThreadDiameter = getMinorDiameter(definition);
            const splitData = getSplitData(context, innerId, endEdgeQuery, minorThreadDiameter);
            entityMapList[] = append(entityMapList[], splitData);
            if (firstDiameter[] == undefined)
            {
                firstDiameter[] = splitData.diameter;
            }
            if (!tolerantEquals(splitData.diameter, firstDiameter[]))
            {
                throw regenError(ErrorStringEnum.DIAMETERS_MUST_BE_EQUAL);
            }

            if (!splitData.isConvexCylinderFace)
            {
                throw regenError(ErrorStringEnum.NOT_CONVEX);
            }
            if (!splitData.isValidArc)
            {
                throw regenError(ErrorStringEnum.INVALID_ARC_LENGTH);
            }
            var threadLengthTooLong = (definition.threadDepth >= splitData.length - (TOLERANCE.zeroLength * meter));
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1901_MAX_THREAD_LENGTH_COMPARISON_FIX))
            {
                threadLengthTooLong = (definition.threadDepth > splitData.length + (TOLERANCE.zeroLength * meter));
            }
            if ((definition.depthType == DepthType.Blind) && threadLengthTooLong)
            {
                throw regenError(ErrorStringEnum.THREAD_DEPTH_BEYOND_CYLINDER, ["threadDepth"]);
            }
            if (performSplit && !definition.addUndercut)
            {
                var adjustedDepthValue = definition.threadDepth - splitData.chamferDistance;
                if (splitData.needsFlip)
                {
                    adjustedDepthValue *= -1; // The normal is going the other way, so we reverse direction
                }

                const planeId = innerId + "tempToolPlane";
                const splitFaceDefinition = {
                        "faceTargets" : splitData.shaftQuery,
                        "planeTools" : qCreatedBy(planeId, EntityType.FACE),
                        "keepToolSurfaces" : false
                    };

                const adjustedOrigin = splitData.edgeCoordSys.origin + (splitData.edgeCoordSys.zAxis * adjustedDepthValue);
                const tempSplitToolPlane = plane(adjustedOrigin, splitData.edgeCoordSys.zAxis); // The cutting tool is a plane offset from the planar face of the endcap.
                opPlane(context, planeId, { "plane" : tempSplitToolPlane });
                opSplitFace(context, innerId + "Split", splitFaceDefinition);
                opDeleteBodies(context, innerId + "Delete", {
                            "entities" : qCreatedBy(planeId, EntityType.BODY) });
            }
            else if (definition.addUndercut)
            {
                var adjustedDepthValue = definition.threadDepth - splitData.chamferDistance;
                var undercutLength = definition.undercutLength;
                var cylinderLength = splitData.length;
                if (splitData.needsFlip)
                {
                    adjustedDepthValue *= -1; // The normal is going the other way, so we reverse direction
                    undercutLength *= -1;
                    cylinderLength *= -1;
                }

                const planeIdUpToNext = innerId + "tempToolPlaneFarUpToNext";
                const planeIdBlindNear = innerId + "tempToolPlaneBlindNear";
                const planeIdBlindFar = innerId + "tempToolPlaneBlindFar";
                var splitFaceDefinitionUndercut = {
                    "faceTargets" : splitData.shaftQuery,
                    "keepToolSurfaces" : false
                    };
                if (definition.depthType == DepthType.Blind)
                {
                    splitFaceDefinitionUndercut["planeTools"] = qUnion(qCreatedBy(planeIdBlindNear, EntityType.FACE), qCreatedBy(planeIdBlindFar, EntityType.FACE));
                    const adjustedOriginBlindNear = splitData.edgeCoordSys.origin + (splitData.edgeCoordSys.zAxis * adjustedDepthValue);
                    const adjustedOriginBlindFar = splitData.edgeCoordSys.origin + (splitData.edgeCoordSys.zAxis * (adjustedDepthValue + undercutLength));
                    const tempSplitToolPlaneBlindNear = plane(adjustedOriginBlindNear, splitData.edgeCoordSys.zAxis);
                    const tempSplitToolPlaneBlindFar = plane(adjustedOriginBlindFar, splitData.edgeCoordSys.zAxis);
                    opPlane(context, planeIdBlindNear, { "plane" : tempSplitToolPlaneBlindNear });
                    opPlane(context, planeIdBlindFar, { "plane" : tempSplitToolPlaneBlindFar });
                }
                else
                {
                    splitFaceDefinitionUndercut["planeTools"] = qCreatedBy(planeIdUpToNext, EntityType.FACE);
                    const adjustedOriginUpToNext = splitData.edgeCoordSys.origin + (splitData.edgeCoordSys.zAxis * (cylinderLength - undercutLength));
                    const tempSplitToolPlaneUpToNext = plane(adjustedOriginUpToNext, splitData.edgeCoordSys.zAxis);
                    opPlane(context, planeIdUpToNext, { "plane" : tempSplitToolPlaneUpToNext });
                }
                opSplitFace(context, innerId + "Split", splitFaceDefinitionUndercut);
                var edgeAdjacentCylinder = qGeometry(qAdjacent(definition.entities, AdjacencyType.EDGE, EntityType.FACE), GeometryType.CYLINDER);
                var undercutCylinder = qGeometry(qAdjacent(edgeAdjacentCylinder, AdjacencyType.EDGE, EntityType.FACE), GeometryType.CYLINDER);
                if (size(evaluateQuery(context, undercutCylinder)) == 0) {
                    // No edges after the split most likely means that the undercut length exceeds the shaft length in an up-to-face case
                    throw regenError(ErrorStringEnum.UNDERCUT_OFF_FACE, ["undercutLength"]);
                }
                const undercutDef = {
                    "offsetDistance" : getUndercutOffset(definition) * -1,
                    "moveFaceType" : MoveFaceType.OFFSET,
                    "moveFaces": undercutCylinder,
                    "outputType":  MoveFaceOutputType.MOVE,
                    "limitType": MoveFaceBoundingType.BLIND
                };
                var threadPlusUndercutTooLong = ((definition.threadDepth + definition.undercutLength >= splitData.length - (TOLERANCE.zeroLength * meter)));
                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1901_MAX_THREAD_LENGTH_COMPARISON_FIX))
                {
                    threadPlusUndercutTooLong = ((definition.threadDepth + definition.undercutLength > splitData.length + (TOLERANCE.zeroLength * meter)));
                }
                if ((definition.depthType == DepthType.Blind) && threadPlusUndercutTooLong)
                {
                    throw regenError(ErrorStringEnum.UNDERCUT_OFF_FACE, ["threadDepth", "undercutLength"]);
                }
                const trackUndercutCylinderFace = startTracking(context, undercutCylinder);
                opOffsetFace(context, innerId + "Offset", undercutDef);
                const qUndercutPlanarFaces = qCreatedBy(innerId + "Offset", EntityType.FACE);
                setHighlightedEntities(context, {"entities": qUnion(trackUndercutCylinderFace, qUndercutPlanarFaces)});

                opDeleteBodies(context, innerId + "Delete", {
                    "entities" : qUnion( qCreatedBy(planeIdBlindNear, EntityType.BODY), qCreatedBy(planeIdBlindFar, EntityType.BODY),  qCreatedBy(planeIdUpToNext, EntityType.BODY)) });
            }

        };
    forEachEntity(context, topLevelId, definition.entities, perEdgeFunction);


    return entityMapList[];
}

/**
 * Add chamfers to threaded cylinder ends
 */
function addChamfers(context, topLevelId, definition, entityList is array) returns array
{
    var chamferFaces is box = new box([]);
    var index = new box(0);
    if (definition.addChamfer)
    {
        const perEdgeFunctionChamfer = function(endEdgeQuery is Query, innerId is Id) {
            const chamferParams = {
                "entities": endEdgeQuery,
                "angle": (90 * degree) - definition.chamferAngle,
                "width": definition.chamferWidth,
                "chamferType": ChamferType.OFFSET_ANGLE,
                "oppositeDirection": !entityList[index[]].needsFlip // similar to calling isEdgeAlignedWithFace, but we already computed the value
            };
            try
            {
                opChamfer(context, innerId + "Chamfer", chamferParams);
                const qChamferFace = qCreatedBy(innerId + "Chamfer", EntityType.FACE);
                chamferFaces[] = append(chamferFaces[], qChamferFace);
                const qAdjacentCylinder = qChamferFace->qAdjacent(AdjacencyType.EDGE, EntityType.FACE)->qGeometry(GeometryType.CYLINDER);
                setHighlightedEntities(context, {"entities": qUnion(qChamferFace, qAdjacentCylinder)});
            }
            catch
            {
                throw regenError(ErrorStringEnum.CHAMFER_FAILED, ["entities"], endEdgeQuery);
            }
            index[] += 1;
        };
        forEachEntity(context, topLevelId, definition.entities, perEdgeFunctionChamfer);
    }
    return chamferFaces[];
}



/**
 * Helper function to get a portion of a lookup table
 */
function getTable(table is map, path is LookupTablePath)
{
    while (table != undefined && table.entries != undefined && table.name != undefined)
    {
        var pathKey = path[table.name];
        var nextEntry = table.entries[pathKey];
        if (nextEntry == undefined)
        {
            return table;
        }
        if (!(nextEntry is map))
        {
            return nextEntry;
        }
        table = nextEntry;
    }
    return table;
}

/**
 * Helper function to determine if a diameter is between to standard diameter sizes in a list, excluding 0 index.
 */
function diameterMatch(index is number, pathArray is array, diameter is ValueWithUnits)
{
    return (diameter <= (pathArray[index].diameter + (TOLERANCE.zeroLength * meter)) && diameter > (pathArray[index - 1].diameter + (TOLERANCE.zeroLength * meter)));
}

/**
 * Simple binary search adjusted to search for a standard size
 */
function binarySearch(context, standardSizes is array, diameter is ValueWithUnits)
{
    var lowerBound = 0;
    var upperBound = size(standardSizes) - 1;
    var midPoint;
    while (lowerBound != upperBound)
    {
        midPoint = ceil((lowerBound + upperBound) / 2);
        if (diameterMatch(midPoint, standardSizes, diameter))
        {
            return midPoint;
        }
        else if (diameter > (standardSizes[midPoint].diameter - (TOLERANCE.zeroLength * meter)))
        {
            lowerBound = midPoint;
        }
        else
        {
            upperBound = midPoint;
        }
    }
    throw "Thread size not found.";
}

/**
 * Given a diameter and a lookup table, search for a thread size equal to or close to that diameter
 */
function searchExternalThreads(context is Context, definition is map, diameter is ValueWithUnits)
{
    const standard = { 'standard' : definition.branchOfStandard.standard };

    var nearestSize = 0.01 * meter;
    var resultPath = standard;
    var table = getTable(localExternalThreadTable, lookupTablePath(resultPath));
    if (table.entries != undefined)
    {
        var pathArray = makeArray(size(table.entries));
        var k = 0;
        for (var entry in table.entries)
        {
            pathArray[k] = { 'path' : { 'size' : entry.key, 'pitch': first(entry["value"]) }, 'diameter' : lookupTableEvaluate(first(entry["value"]["entries"]).holeDiameter) };
            k += 1;
        }
        pathArray = sort(pathArray, function(a, b)
            {
                return a.diameter - b.diameter;
            });
        if (diameter < pathArray[0].diameter + (TOLERANCE.zeroLength * meter))
        {
            resultPath.size = pathArray[0].path.size;
            resultPath.pitch = pathArray[0].path.pitch;
            nearestSize = pathArray[0].diameter;
        }
        else if (diameter > last(pathArray).diameter - (TOLERANCE.zeroLength * meter))
        {
            resultPath.size = last(pathArray).path.size;
            resultPath.pitch = last(pathArray).path.pitch;
            nearestSize = last(pathArray).diameter;
        }
        else
        {
            const foundMatchIndex = binarySearch(context, pathArray, diameter);
            const firstDiff = abs(diameter - pathArray[foundMatchIndex - 1].diameter);
            const secondDiff = abs(diameter - pathArray[foundMatchIndex].diameter);
            var sizeIndex;
            if (tolerantEquals(firstDiff, secondDiff))
            {
                sizeIndex = foundMatchIndex;
            }
            else
            {
                sizeIndex = firstDiff > secondDiff ? foundMatchIndex : foundMatchIndex - 1;
            }
            nearestSize = pathArray[sizeIndex].diameter;
            resultPath.size = pathArray[sizeIndex].path.size;
            resultPath.pitch = pathArray[sizeIndex].path.pitch;
        }
    }
    resultPath = lookupTablePath(resultPath);
    return { "path" : resultPath, "size" : nearestSize };
}

/**
 * Returns true if the external thread standard has changed (ANSI to ISO, DIN, etc..)
 */
function standardChanged(oldDefinition is map, definition is map)
{
    if (oldDefinition.branchOfStandard == undefined)
    {
        return true;
    }
    return (oldDefinition.branchOfStandard.standard != definition.branchOfStandard.standard);
}

/**
 * Returns true if the external thread size has changed
 */
function sizeChanged(oldDefinition is map, definition is map)
{
    // Upon load of the feature, we go from a blank definition to a default-populated definition
    if (oldDefinition == undefined || oldDefinition.branchOfStandard == undefined)
    {
        return true;
    }
    return (oldDefinition.branchOfStandard.standard != definition.branchOfStandard.standard ||
       oldDefinition.branchOfStandard.size != definition.branchOfStandard.size ||
       oldDefinition.branchOfStandard.pitch != definition.branchOfStandard.pitch);
}

function getRadialDifference(definition is map)
{
    const table = getTable(localExternalThreadTable, lookupTablePath(definition.branchOfStandard));

    if (table.minorDiameter == undefined || table.majorDiameter == undefined)
    {
        throw regenError(ErrorStringEnum.PARAMETER_VALUE_INVALID, ["branchOfStandard"]);
    }

    const minorDiameter = lookupTableEvaluate(table.minorDiameter);
    const majorDiameter = lookupTableEvaluate(table.majorDiameter);
    const majorMinorDiff = majorDiameter - minorDiameter;
    return majorMinorDiff * 0.5;
}

function getUndercutOffset(definition is map)
{
    const table = getTable(localExternalThreadTable, lookupTablePath(definition.branchOfStandard));

    if (table.majorDiameter == undefined)
    {
        throw regenError(ErrorStringEnum.PARAMETER_VALUE_INVALID, ["branchOfStandard"]);
    }

    const majorDiameter = lookupTableEvaluate(table.majorDiameter);
    return (majorDiameter - definition.undercutDiameter) * 0.5;
}

function getMinorDiameter(definition is map)
{
    const table = getTable(localExternalThreadTable, lookupTablePath(definition.branchOfStandard));

    if (table.minorDiameter == undefined)
    {
        throw regenError(ErrorStringEnum.PARAMETER_VALUE_INVALID, ["branchOfStandard"]);
    }

    return lookupTableEvaluate(table.minorDiameter);
}



/** @internal */
export function editFeatureLogicExternalThread(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean, specified is map) returns map
{
    const endEdges = evaluateQuery(context, definition.entities);
    if (size(endEdges) <= 0)
    {
        return definition;
    }
    const minorThreadDiameter = getMinorDiameter(definition);
    const splitData = getSplitData(context, id, endEdges[0], minorThreadDiameter);
    if (!splitData.isConvexCylinderFace || !splitData.isValidArc)
    {
        return definition;
    }

    // Only update size if an edge is selected AND either the edge selection has changed or the standard (ANSI/ISO, etc) has changed.
    if (!isQueryEmpty(context, definition.entities) && (standardChanged(oldDefinition, definition) || isQueryEmpty(context, oldDefinition.entities)))
    {
        const foundStandard = searchExternalThreads(context, definition, splitData.diameter);
        definition.branchOfStandard = foundStandard.path;
    }

    if (sizeChanged(oldDefinition, definition) || (oldDefinition.addChamfer && !definition.addChamfer) )
    {
        const radialDiff = getRadialDifference(definition);
        definition.chamferWidth = radialDiff + CHAMFER_LENGTH_OFFSET;
    }

    if (sizeChanged(oldDefinition, definition) || (oldDefinition.addUndercut && !definition.addUndercut))
    {
        const minorDiameter = getMinorDiameter(definition);
        definition.undercutDiameter = minorDiameter - UNDERCUT_DEPTH_OFFSET;
        var pitchLength = computePitchValue(definition.branchOfStandard.pitch);
        if (pitchLength != undefined)
        {
            definition.undercutLength = pitchLength * 2;
        }
        else
        {
            definition.undercutLength = 1 * millimeter;
        }
    }
    return definition;
}

/**
 * Report a notification banner if no exact external thread size is found or the user changes the size from the recommended one
 */
function checkNonMatchingSize(context is Context, id is Id, definition is map, latestDiameter is ValueWithUnits)
{
    if (!isQueryEmpty(context, definition.entities))
    {
        const threadTable = getTable(localExternalThreadTable, lookupTablePath(definition.branchOfStandard));
        const latestMinorDiameter = lookupTableEvaluate(threadTable.minorDiameter);

        const foundStandard = searchExternalThreads(context, definition, latestDiameter);
        const foundSize = foundStandard.size;
        const foundSizeName = foundStandard.path;

        if (foundSize != undefined)
        {
            // Case for user-selected another size
            if (definition.branchOfStandard['size'] != foundSizeName['size'])
            {
                if (latestDiameter < latestMinorDiameter)
                {
                    reportFeatureError(context, id, ErrorStringEnum.NON_MATCHING_SIZE_ERROR);
                }
                else
                {
                    reportFeatureInfo(context, id, ErrorStringEnum.SELECTED_NON_MATCHING_SIZE);
                }
            }
            // Case for size not found
            else if (!tolerantEquals(foundSize, latestDiameter))
            {
                if (latestDiameter < latestMinorDiameter)
                {
                    reportFeatureError(context, id, ErrorStringEnum.NON_MATCHING_SIZE_ERROR);
                }
                else
                {
                    reportFeatureInfo(context, id, ErrorStringEnum.NEAREST_MATCHING_THREAD_SIZE);
                }
            }
        }
    }
}


/**
 * Check if external thread attributes already exist on selected entities
 */
function checkExistingExternalThread(context is Context, entities is Query)
{
    const holeAttributes = getAttributes(context, {
        "entities" : entities,
        "attributePattern" : {} as HoleAttribute}
    );
    for (var holeAttribute in holeAttributes) {
        if (holeAttribute.isExternalThread) {
                throw regenError(ErrorStringEnum.CANNOT_ADD_MORE_THAN_ONE_THREAD_TO_UNSPLIT_CYLINDER, ["entities"]);
        }
    }
}

/**
 * Add external thread attribute to each selected entity
 */
function addExternalThreadAttributes(context is Context, id is Id, definition is map, entityList is array)
{
    var attributes = [];
    var table = getTable(localExternalThreadTable, lookupTablePath(definition.branchOfStandard));
    const minorDiameter = lookupTableEvaluate(table.minorDiameter);
    const majorDiameter = lookupTableEvaluate(table.majorDiameter);
    const holeDiameter = lookupTableEvaluate(table.holeDiameter);
    const pitchAnnotation = buildPitchAnnotation(definition.branchOfStandard.pitch);
    const nominalSize = definition.branchOfStandard.size ~ pitchAnnotation;

    var threadDepth = 1 * inch;
    const isBlind = (definition.depthType == DepthType.Blind);
    if (isBlind)
    {
        threadDepth = definition.threadDepth;
    }

    var i = 1;
    for (var entityMap in entityList)
    {
        const newId = id ~ ".idx." ~ i;
        if (!isBlind)
        {
            threadDepth = entityMap.length;
        }
        var relatedEntities = qAdjacent(entityMap.edgeQuery, AdjacencyType.EDGE, EntityType.FACE);
        var cylinderHighlight = qGeometry(relatedEntities, GeometryType.CYLINDER);
        const tapThrough = !isBlind && entityMap.shouldTapThrough;
        const attribute = createExternalThreadAttribute(newId, minorDiameter, majorDiameter, holeDiameter, threadDepth, isBlind, nominalSize, entityMap.length, entityMap.cylinderAlignedWithThreadDirection, tapThrough );
        const onlyCylinder = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2243_EXT_THREAD_ATTRIBUTE_FIX);
        const entitiesToMark = qUnion(onlyCylinder ? cylinderHighlight : relatedEntities, entityMap.edgeQuery);
        attributes = append(attributes, attribute);
        checkExistingExternalThread(context, entitiesToMark);
        setAttribute(context, { "entities" : entitiesToMark, "attribute" : attribute });
        addDebugEntities(context, cylinderHighlight, DebugColor.ORANGE);
        i += 1;
    }
    return attributes;
}

/**
 * Create an attribute for an external thread on a face
 */
function createExternalThreadAttribute(id is string, minorDiameter is ValueWithUnits, majorDiameter is ValueWithUnits, holeDiameter is ValueWithUnits, threadDepth is ValueWithUnits, isBlind is boolean, nominalSize is string, shaftLength is ValueWithUnits, cylinderAlignedWithThreadDirection is boolean, tapThrough is boolean)
{
    var threadAttribute = makeHoleAttribute(true, id, HoleStyle.SIMPLE);
    threadAttribute.isTappedHole = true;
    threadAttribute.sectionFace = { "type" : HoleSectionFaceType.THROUGH_FACE };
    threadAttribute.isExternalThread = true;
    threadAttribute.minorDiameter = { "value" : minorDiameter };
    threadAttribute.majorDiameter = { "value" : majorDiameter };
    threadAttribute.tappedDepth = { "value" : threadDepth };
    threadAttribute.holeDepth = { "value" : shaftLength };
    threadAttribute.holeDiameter = { "value" : holeDiameter };
    threadAttribute.endType = isBlind ? HoleEndStyle.BLIND : HoleEndStyle.THROUGH;
    threadAttribute.tapSize = nominalSize;
    threadAttribute.isTappedThrough = tapThrough;
    threadAttribute.isTaperedPipeTapHole = false;
    threadAttribute.tapClearance = 0.0;
    threadAttribute.cylinderAlignedWithThreadDirection = cylinderAlignedWithThreadDirection;
    return threadAttribute;
}

