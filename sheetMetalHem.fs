FeatureScript 1746; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "1746.0");
import(path : "onshape/std/booleanaccuracy.gen.fs", version : "1746.0");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "1746.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "1746.0");
import(path : "onshape/std/containers.fs", version : "1746.0");
import(path : "onshape/std/context.fs", version : "1746.0");
import(path : "onshape/std/coordSystem.fs", version : "1746.0");
import(path : "onshape/std/curveGeometry.fs", version : "1746.0");
import(path : "onshape/std/error.fs", version : "1746.0");
import(path : "onshape/std/evaluate.fs", version : "1746.0");
import(path : "onshape/std/feature.fs", version : "1746.0");
import(path : "onshape/std/math.fs", version : "1746.0");
import(path : "onshape/std/query.fs", version : "1746.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "1746.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "1746.0");
import(path : "onshape/std/sketch.fs", version : "1746.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1746.0");
import(path : "onshape/std/topologyUtils.fs", version : "1746.0");
import(path : "onshape/std/transform.fs", version : "1746.0");
import(path : "onshape/std/valueBounds.fs", version : "1746.0");
import(path : "onshape/std/vector.fs", version : "1746.0");

/**
 * @internal
 * Specifies the type of hem to create.
 *
 * @value ROLLED    : A circular hem defined by an angle and inner radius.
 * @value STRAIGHT  : A hem with an 180 degree arc and a linear extension defined by a gap and total hem width.
 * @value TEAR_DROP : A hem in the shape of a tear drop defined by an inner radius and total hem width.
 */
export enum SMHemType
{
    annotation { "Name" : "Straight" }
    STRAIGHT,
    annotation { "Name" : "Rolled" }
    ROLLED,
    annotation { "Name" : "Tear drop" }
    TEAR_DROP
}

/**
 * @internal
 * Specifies the type of alignment of the hem
 *
 * @value OUTER    : The outside of the hem aligns with the selected edge of the sheet metal model.
 * @value IN_PLACE : The hem is built directly off of the selected edge of the sheet metal model, without moving the edge first.
 */
export enum SMHemAlignment
{
    annotation { "Name" : "Outer" }
    OUTER,
    annotation { "Name" : "In place" }
    IN_PLACE
}

/**
 * @internal
 * Specifies the type of corners to use for the hem.
 *
 * @value SIMPLE : Hem corners are linear in the flat, and helical when folded.  Hem sheets are pushed back to meet the end of the helix.
 * @value CLOSED: Hem corners are splines in the flat, and lie at the defined sheet metal minimal clearance away from any neighbors in the folded.
 */
export enum SMHemCornerType
{
    annotation { "Name" : "Simple" }
    SIMPLE,
    annotation { "Name" : "Closed" }
    CLOSED
}

/**
 * @internal
 * An `AngleBoundSpec` for hems.  Defaults to 270 (or 3pi/2), and is limited to (0 degrees, 360 degrees) exclusive.
 */
export const SM_HEM_ANGLE_BOUNDS =
{
    (degree) : [180, 270, 359.9]
} as AngleBoundSpec;

/**
 * @internal
 * A `LengthBoundSpec` for hem lengths. Defaults defined as NONNEGATIVE_LENGTH_BOUNDS / 2.
 */
export const SM_HEM_LENGTH_BOUNDS =
{
    (meter)      : [1e-5, 0.0125, 500],
    (centimeter) : 1.25,
    (millimeter) : 12.5,
    (inch)       : 0.5,
    (foot)       : 0.05,
    (yard)       : 0.0125
} as LengthBoundSpec;

/**
 * @internal
 * Create sheet metal hems on selected edges of sheet metal parts.
 */
annotation { "Feature Type Name" : "Hem", "Editing Logic Function" : "hemEditLogic" }
export const sheetMetalHem = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        // This should match sheetMetalFlange
        annotation { "Name" : "Edges or side faces to hem",
                     "Filter" : ModifiableEntityOnly.YES && SheetMetalDefinitionEntityType.EDGE
                        && ((GeometryType.LINE && AllowFlattenedGeometry.YES) || (GeometryType.PLANE && AllowFlattenedGeometry.NO)) }
        definition.edges is Query;

        annotation { "Name" : "Hem type", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        definition.hemType is SMHemType;

        annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.oppositeDirection is boolean;

        if (definition.hemType == SMHemType.STRAIGHT)
        {
            annotation { "Name" : "Flattened", "Default" : true, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            definition.useMinimalGapForStraight is boolean;
        }

        if (definition.hemType == SMHemType.ROLLED || definition.hemType == SMHemType.TEAR_DROP
            || (definition.hemType == SMHemType.STRAIGHT && !definition.useMinimalGapForStraight))
        {
            annotation { "Name" : "Inner radius", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            isLength(definition.innerRadius, SM_BEND_RADIUS_BOUNDS);
        }

        if (definition.hemType == SMHemType.TEAR_DROP)
        {
            annotation { "Name" : "Use minimal gap", "Default" : true, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            definition.useMinimalGapForTipGap is boolean;

            if (!definition.useMinimalGapForTipGap)
            {
                annotation { "Name" : "Gap", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition.tipGap, SM_MINIMAL_CLEARANCE_BOUNDS);
            }
        }

        if (definition.hemType == SMHemType.ROLLED)
        {
            annotation { "Name" : "Angle", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            isAngle(definition.angle, SM_HEM_ANGLE_BOUNDS);
        }

        if (definition.hemType == SMHemType.TEAR_DROP || definition.hemType == SMHemType.STRAIGHT)
        {
            annotation { "Name" : "Total length", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            isLength(definition.length, SM_HEM_LENGTH_BOUNDS);
        }

        annotation { "Name" : "Hem alignment", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE] }
        definition.hemAlignment is SMHemAlignment;

        annotation { "Name" : "Corner type", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE] }
        definition.hemCornerType is SMHemCornerType;
    }
    {
        // this is not necessary but helps with correct error reporting in feature pattern
        checkNotInFeaturePattern(context, definition.edges, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        const edgesArr = getSMDefinitionEdgesForFlangeTypeFeature(context, id, {
                    "edges" : definition.edges,
                    "errorForNoEdges" : ErrorStringEnum.SHEET_METAL_HEM_NO_EDGES,
                    "errorForInternal" : ErrorStringEnum.SHEET_METAL_HEM_INTERNAL,
                    "errorForNonLinearEdges" : ErrorStringEnum.SHEET_METAL_HEM_NON_LINEAR_EDGES,
                    "errorForEdgesNextToCylinderBend" : ErrorStringEnum.SHEET_METAL_HEM_NEXT_TO_CYLINDER_BEND
                });
        const edges = qUnion(edgesArr);

        // Remove fillets and chamfers
        removeCornerBreaksAtEdgeVertices(context, edges);

        // Get originals before any changes
        var smBodies = evaluateQuery(context, qOwnerBody(edges));
        var smBodiesQ = qUnion(smBodies);
        const initialData = getInitialEntitiesAndAttributes(context, smBodiesQ);
        const robustSMBodiesQ = qUnion([smBodiesQ, startTracking(context, smBodiesQ)]);

        const bodyToEdges = groupEntitiesByBody(context, edges);

        var bodyIndex = 0;
        var bodiesToDelete = new box([]);
        var attributeIdCounter = new box(0);
        for (var entry in bodyToEdges)
        {
            addHemsToSheetBody(context, id, qUnion(entry.value), entry.key, bodyIndex, definition, bodiesToDelete, attributeIdCounter);
            bodyIndex += 1;
        }

        // Delete the helper bodies that are no longer needed
        opDeleteBodies(context, id + "deleteBodies", {
                    "entities" : qUnion(bodiesToDelete[])
                });

        // Add association attributes where needed and compute deleted attributes
        var toUpdate = assignSMAttributesToNewOrSplitEntities(context, robustSMBodiesQ, initialData, id);
        updateSheetMetalGeometry(context, id, {
                    "entities" : toUpdate.modifiedEntities,
                    "deletedAttributes" : toUpdate.deletedAttributes
                });
    }, { "oppositeDirection" : false });

/**
 * Build and attach hems for each of the specified edges.  Edges should all belong to the same underlying sheet body.
 */
function addHemsToSheetBody(context is Context, topLevelId is Id, edges is Query, body is Query, bodyIndex is number,
    definition is map, bodiesToDelete is box, attributeIdCounter is box)
{
    const bodyId = topLevelId + unstableIdComponent("body" ~ bodyIndex);
    const modelParameters = getModelParameters(context, body);
    const robustBodyQuery = qUnion([body, startTracking(context, body)]);

    edges = changeUnderlyingSheetForAlignment(context, topLevelId, bodyId + "alignment", edges, modelParameters, definition);
    const edgeArray = evaluateQuery(context, edges);

    var edgeToHemData = getEdgeToHemData(context, edgeArray);

    var edgeIndex = 0;
    for (var edge in edgeArray)
    {
        var sketchId = bodyId + "sketch" + unstableIdComponent("edge" ~ edgeIndex);
        edgeIndex += 1;
        setExternalDisambiguation(context, sketchId, edge);

        const sketchHemProfileReturn = sketchHemProfile(context, sketchId, edgeToHemData[edge], modelParameters, definition);
        edgeToHemData[edge] = augmentHemDataWithSketchInformation(edgeToHemData[edge], sketchId, sketchHemProfileReturn);
        addBodiesToDelete(bodiesToDelete, qCreatedBy(sketchId, EntityType.BODY));
    }

    edgeIndex = 0;
    var matches = [];
    var bodiesToBoolean = [];
    const sharedVertexToMiteredBoundingData = new box({});
    for (var edge in edgeArray)
    {
        var edgeId = bodyId + unstableIdComponent("edge" ~ edgeIndex);
        edgeIndex += 1;
        setExternalDisambiguation(context, edgeId, edge);

        const hemData = edgeToHemData[edge];

        const startAndEndBoundingData = getStartAndEndBoundingData(context, edge, edgeToHemData, definition, modelParameters, sharedVertexToMiteredBoundingData);

        const hemSheetId = edgeId + "hemSheet";
        const sheetMap = constructHemSheets(context, hemSheetId, hemData, startAndEndBoundingData, definition, modelParameters, bodiesToDelete);

        annotateHemSheets(context, topLevelId, sheetMap.arcSheet, sheetMap.otherSheet, attributeIdCounter, hemData, modelParameters, definition);

        matches = concatenateArrays([matches, createMatches(context, edge, sheetMap.arcSheet, sheetMap.otherSheet, hemData)]);

        bodiesToBoolean = append(bodiesToBoolean, qUnion([sheetMap.arcSheet, sheetMap.otherSheet]));
    }

    mergeSheetMetal(context, bodyId + "boolean", {
                "topLevelId" : topLevelId,
                "surfacesToAdd" : qUnion(bodiesToBoolean),
                "originalSurfaces" : robustBodyQuery,
                "matches" : matches,
                "accuracy" : BooleanAccuracy.HIGH,
                "attributeIdCounter" : attributeIdCounter,
                "error" : ErrorStringEnum.SHEET_METAL_HEM_FAILED,
                "errorParameters" : ["edges"]
            });
}

// -------------------------------------------------- Hem data --------------------------------------------------

/**
 * Fill a map with the results of `getHemData` for each edge of `edgeArray`
 */
function getEdgeToHemData(context is Context, edgeArray is array) returns map
{
    var edgeToHemData = {};
    for (var edge in edgeArray)
    {
        edgeToHemData[edge] = getHemData(context, edge);
    }
    return edgeToHemData;
}

/**
 * Must be called after adjusting edges for alignment, or base point calculation will be incorrect.
 * @returns {{
 *      @field edgeDirection {Vector} : the direction of the edge (with edge orientation determined by the adjacent face)
 *      @field edgeStartPosition {Line}  : the position of the start of the edge (with edge orientation determined by the adjacent face)
 *      @field edgeCenterPosition {Line} : the position of the center of the edge (with edge orientation determined by the adjacent face)
 *      @field edgeEndPosition {Line}    : the position of the end of the edge (with edge orientation determined by the adjacent face)
 *      @field face {Query} : the face that the hem is growing off of
 *      @field faceNormal {Query} : the normal of `face`
 *      @field outFromFace {Vector} : the direction that lies on the face plane and pointing directly away from the face, evaluated at the center of the edge
 *      @field startVertex {Query} : the start vertex of the edge (with edge orientation determined by the adjacent face)
 *      @field endVertex {Query}   : the end vertex of the edge (with edge orientation determined by the adjacent face)
 *      @field startAndEndSurroundingGeometryData {map} : surrounding geometry data for `start` and `end`.  See [getSurroundingGeometryDataForSide].
 * }}
 */
function getHemData(context is Context, edge is Query) returns map
{
    const faces = evaluateQuery(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE));
    if (size(faces) != 1)
        throw regenError(ErrorStringEnum.SHEET_METAL_HEM_INTERNAL, ["edges"]);
    const face = faces[0];

    const edgeTangentLines = evEdgeTangentLines(context, {
                "edge" : edge,
                "parameters" : [0, 0.5, 1],
                "face" : face
            });
    const edgeTangentLineAtStart = edgeTangentLines[0];
    const edgeTangentLineAtCenter = edgeTangentLines[1];
    const edgeTangentLineAtEnd = edgeTangentLines[2];
    const edgeDirection = edgeTangentLineAtCenter.direction;

    const faceTangentPlaneAtEdge = evFaceTangentPlaneAtEdge(context, {
                "edge" : edge,
                "face" : face,
                "parameter" : 0.5,
                "usingFaceOrientation" : true
            });
    const faceNormal = faceTangentPlaneAtEdge.normal;
    const outFromFace = cross(edgeDirection, faceNormal);


    const bothVertices = qAdjacent(edge, AdjacencyType.VERTEX, EntityType.VERTEX);
    const startResult = evaluateQuery(context, qClosestTo(bothVertices, edgeTangentLineAtStart.origin));
    if (size(startResult) != 1)
        throw "Unexpected number of vertices";
    const startVertex = startResult[0];
    const endResult = evaluateQuery(context, qClosestTo(bothVertices, edgeTangentLineAtEnd.origin));
    if (size(endResult) != 1)
        throw "Unexpected number of vertices";
    const endVertex = endResult[0];

    return {
            "edgeDirection" : edgeDirection,
            "edgeStartPosition" : edgeTangentLineAtStart.origin,
            "edgeCenterPosition" : edgeTangentLineAtCenter.origin,
            "edgeEndPosition" : edgeTangentLineAtEnd.origin,
            "face" : face,
            "faceNormal" : faceNormal,
            "outFromFace" : outFromFace,
            "startVertex" : startVertex,
            "endVertex" : endVertex,
            "startAndEndSurroundingGeometryData" : {
                    "start" : getSurroundingGeometryDataForSide(context, true, edge, edgeDirection, startVertex, edgeTangentLineAtStart.origin, face, faceNormal),
                    "end" : getSurroundingGeometryDataForSide(context, false, edge, edgeDirection, endVertex, edgeTangentLineAtEnd.origin, face, faceNormal)
                }
        };
}

/**
 * Adds the profile sketch id and data returned from sketchHemProfile(...) into the given hemData.
 */
function augmentHemDataWithSketchInformation(hemData is map, profileSketchId is Id, sketchHemProfileReturn is map) returns map
{
    hemData.profileSketchId = profileSketchId;
    hemData = mergeMaps(hemData, sketchHemProfileReturn);
    return hemData;
}

// -------------------------------------------------- Surrounding geometry --------------------------------------------------

/**
 * @returns {{
 *     @field sideEdge {Query} : The edge that is adjacent to the hem face, and vertex-adjacent to the hem edge for the given side.
 *     @field sideEdgeIsTwoSided {boolean}
 *     @field basePoint {Vector} : The base point that the hem arc should extend from for the given side.
 *     @field endPlane {Plane} : See [getEndPlane]
 * }}
 */
function getSurroundingGeometryDataForSide(context is Context, isStart is boolean, edge is Query, edgeDirection is Vector,
    vertexInQuestion is Query, vertexInQuestionPosition is Vector, hemFace is Query, hemFaceNormal is Vector) returns map
{
    const sideEdgeData = getSideEdgeData(context, edge, vertexInQuestion, hemFace);
    const sideEdge = sideEdgeData.sideEdge;
    const sideEdgeIsTwoSided = sideEdgeData.sideEdgeIsTwoSided;

    var basePoint = vertexInQuestionPosition;

    // We cannot get a useful result out of evDistance if the side geometry is a line parallel to the hem edge
    const canRefineWithSideGeometry = (sideEdgeData.sideEdgeLine == undefined) || (!parallelVectors(sideEdgeData.sideEdgeLine.direction, edgeDirection));
    if (canRefineWithSideGeometry)
    {
        const relevantSideGeometry = getRelevantSolidFoldedSideGeometry(context, edge, sideEdge, sideEdgeIsTwoSided, vertexInQuestionPosition);
        if (relevantSideGeometry != undefined)
        {
            basePoint = evDistance(context, {
                            "side0" : relevantSideGeometry,
                            "side1" : edge,
                            // BEL-115798: Make our best attempt to find the real intersection between side geometry and edge line.
                            // If there is a fillet or chamfer pulling back the current built-out geometry on the corner, we have to
                            // rely on the extension to potentially find the right intersection. In the case of linear side edges
                            // or planar side faces this is sufficient, but in the case of non-analytic geometry we may just get it wrong.
                            "extendSide0" : true
                            // Do not extend side1.  This ensures that the base point does not extend past the end of the edge.
                            // (ex: in the case of neighboring hems on a plate that are both being pulled back by OUTER alignment)
                        }).sides[1].point;
        }
    }

    return {
            "sideEdge" : sideEdge,
            "sideEdgeIsTwoSided" : sideEdgeIsTwoSided,
            "basePoint" : basePoint,
            "endPlane" : getEndPlane(context, isStart, basePoint, edgeDirection, sideEdge, hemFace, hemFaceNormal)
        };
}

/**
 * @returns {{
 *     @field sideEdge {Query} : The edge that is adjacent to the hem face, and vertex-adjacent to the hem edge for the given side.
 *     @field sideEdgeIsTwoSided {boolean}
 *     @field sideEdgeLine {Line} : The [Line] representing sideEdge, if sideEdge is linear.  Otherwise, undefined.
 * }}
 */
function getSideEdgeData(context is Context, edge is Query, vertexInQuestion is Query, hemFace is Query) returns map
{
    const outgoingEdges = qSubtraction(qAdjacent(vertexInQuestion, AdjacencyType.VERTEX, EntityType.EDGE), edge);
    const jointToNextFace = qIntersection([outgoingEdges, qEdgeTopologyFilter(qAdjacent(hemFace, AdjacencyType.EDGE, EntityType.EDGE), EdgeTopology.TWO_SIDED)]);

    // Find the side edge that we care about.  This should be the closest outgoing two sided edge, or the one-sided edge if a two-sided one
    // does not exist.
    // TODO: BEL-113962 trace over vertical edge to next intersection
    var sideEdge;
    var sideEdgeIsTwoSided;
    const jointToNextFaceArr = evaluateQuery(context, jointToNextFace);
    if (jointToNextFaceArr == [])
    {
        const outgoingEdgesArr = evaluateQuery(context, outgoingEdges);
        if (size(outgoingEdgesArr) != 1)
        {
            // This should not be topologically possible
            throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
        }

        sideEdge = outgoingEdgesArr[0];
        sideEdgeIsTwoSided = false;
    }
    else if (size(jointToNextFaceArr) == 1)
    {
        sideEdge = jointToNextFaceArr[0];
        sideEdgeIsTwoSided = true;
    }
    else
    {
        // This should not be topologically possible
        throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
    }

    return {
            "sideEdge" : sideEdge,
            "sideEdgeIsTwoSided" : sideEdgeIsTwoSided,
            "sideEdgeLine" : try silent(evLine(context, { "edge" : sideEdge }))
        };
}

/**
 * Get the most relevant edge or face which sideEdge refers to in the solid folded sheet metal model.  In rare cases we
 * may not be able to find this, and will return undefined.
 */
function getRelevantSolidFoldedSideGeometry(context is Context, edge is Query, sideEdge is Query, sideEdgeIsTwoSided is boolean, vertexInQuestionPosition is Vector)
{
    // Find the faces associated with the wall attached to the hem
    const hemWallAssociatedFaces = qAssociatedInSolidFolded(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE));

    // Find the built-out side geometry which will define the base point and end plane
    var sideEdgeAssociations;
    try silent
    {
        sideEdgeAssociations = qAssociatedInSolidFolded(context, sideEdge);
    }
    catch
    {
        // BEL-115533: We may have created a step edge with no association.  For now, just return undefined.
        return undefined;
    }

    var sideEdgeJointType = undefined;
    if (sideEdgeIsTwoSided)
    {
        const sideEdgeJointAttribute = getJointAttribute(context, sideEdge);
        if (sideEdgeJointAttribute != undefined)
        {
            sideEdgeJointType = sideEdgeJointAttribute.jointType.value;
        }
        else
        {
            // If it is two sided with no attribute, it is an edge between a wall and a cylinder bend.  Treat in the same
            // way as a TANGENT joint.
            sideEdgeJointType = SMJointType.TANGENT;
        }
    }

    var relevantSideGeometry;
    if (sideEdgeJointType == SMJointType.RIP || sideEdgeJointType == undefined)
    {
        // RIPs and laminar edges have a side face. RIP edges will have associated side faces for both the wall we care
        // about and the wall on the other side of the rip. Laminar edges are simpler and only have an associated side
        // face for the wall we care about.  In both cases, it is possible to find the correct side face by intersecting
        // all the side associations with the faces adjacent to our wall's faces.
        relevantSideGeometry = qIntersection([qAdjacent(hemWallAssociatedFaces, AdjacencyType.EDGE, EntityType.FACE), sideEdgeAssociations]);
    }
    else if (sideEdgeJointType == SMJointType.BEND || sideEdgeJointType == SMJointType.TANGENT)
    {
        // BENDs and TANGENTs do not have a side face, so we need to use a side edge that borders the wall we care about.
        // BEND edges will have associated side edges for both the wall we care about, and the wall on the other side of
        // the bend. TANGENT edges are simpler, and only have associated side edges for the wall we care about.  In both
        // cases, it is possible to find the correct side edge by intersecting all the side associations with the edges
        // bordering our wall's faces. Then use the edge closest to the vertexInQuestion.
        const potentialSideEdges = qIntersection([qAdjacent(hemWallAssociatedFaces, AdjacencyType.EDGE, EntityType.EDGE), sideEdgeAssociations]);
        relevantSideGeometry = qClosestTo(potentialSideEdges, vertexInQuestionPosition);
    }
    else
    {
        throw "Unrecognized joint type";
    }

    const relevantSideGeometryArr = evaluateQuery(context, relevantSideGeometry);
    if (size(relevantSideGeometryArr) != 1)
    {
        // This should not be topologically possible
        throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
    }
    return relevantSideGeometryArr[0];
}

/**
 * Find the canonical end plane for the start or end of a hem edge. This plane has its origin at the hem base point,
 * and its normal is such that the plane is coincident with the side geometry next to the hem.
 */
function getEndPlane(context is Context, isStart is boolean, basePoint is Vector, edgeDirection is Vector, sideEdge is Query,
    hemFace is Query, hemFaceNormal is Vector) returns Plane
{
    const sideEdgeDirectionAtVertex = evEdgeTangentLine(context, {
                    "edge" : sideEdge,
                    "parameter" : isStart ? 1.0 : 0.0,
                    "face" : hemFace
                }).direction;

    var planeNormal;
    if (!parallelVectors(edgeDirection, sideEdgeDirectionAtVertex))
    {
        // The normal should point 'along' the `edgeDirection`, rather than 'away' from it. See
        // `getBoundingDataAtVertex(...)` documentation.
        const normalSign = isStart ? 1 : -1;
        planeNormal = normalSign * cross(hemFaceNormal, sideEdgeDirectionAtVertex);
    }
    else
    {
        // BEL-116134: Make sure we do not fail if sideEdge is tangent.
        planeNormal = edgeDirection;
    }

    return plane(basePoint, planeNormal);
}

// -------------------------------------------------- Hem alignment --------------------------------------------------

/**
 * Push back the hem edges if necessary, and return the resulting edges
 */
function changeUnderlyingSheetForAlignment(context is Context, topLevelId is Id, id is Id, hemEdges is Query, modelParameters is map,
    definition is map) returns Query
{
    if (definition.hemAlignment == SMHemAlignment.IN_PLACE)
    {
        // Do nothing and return the same edges
        return hemEdges;
    }
    if (definition.hemAlignment != SMHemAlignment.OUTER)
    {
        // UI user cannot hit this
        throw "Unrecognized hem alignment type: " ~ definition.hemAlignment;
    }

    // OUTER
    const innerRadius = getInnerRadius(definition, modelParameters);
    const hemExtent = innerRadius + modelParameters.backThickness + modelParameters.frontThickness;

    var edgeChangeOptions = [];
    for (var edge in evaluateQuery(context, hemEdges))
    {
        edgeChangeOptions = append(edgeChangeOptions, {
                    "edge" : edge,
                    "face" : qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE),
                    "offset" : -hemExtent
                });
    }
    const resultEdges = qUnion([hemEdges, startTracking(context, hemEdges)]);

    const edgeChangeId = id + "edgeChange";
    try
    {
        // TODO: Try to do this more precisely for rolled surfaces if possible, using a detached fillet.
        sheetMetalEdgeChangeCall(context, edgeChangeId, hemEdges, {
                    "edgeChangeOptions" : edgeChangeOptions
                });
    }
    processSubfeatureStatus(context, topLevelId, { "subfeatureId" : edgeChangeId, "propagateErrorDisplay" : true });
    if (featureHasError(context, topLevelId))
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAIL_ALIGNMENT, ["hemAlignment"]);
    }

    if (size(evaluateQuery(context, resultEdges)) != size(edgeChangeOptions))
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAIL_ALIGNMENT, ["hemAlignment"]);
    }

    return resultEdges;
}

// -------------------------------------------------- Hem sketch --------------------------------------------------

/**
 * Sketch the hem profile for a given edge. `hemData` should be in the format outputted by `getHemData`.
 * @returns {{
 *     @field arcEdge {Query} : the arc of the hem profile.
 *     @field arcAngle {ValueWithUnits} : the central angle of the initial arc.
 *     @field arcRadius {ValueWithUnits} : the radius of the initial arc.
 *     @field arcCenter {Vector} : the center of the initial arc.
 *     @field arcEndPosition {Vector} : The position of the end of the initial arc.
 * }}
 */
function sketchHemProfile(context is Context, id is Id, hemData is map, modelParameters is map, definition is map) returns map
{
    // See newHemProfileSketch(...) documentation for information about sketch plane alignment.
    const sketch = newHemProfileSketch(context, id, hemData);

    // Opposite direction should wrap around the front of the sheet.  Non-opposite direction should wrap around the back of the sheet.
    const wrapAroundFront = getWrapAroundFront(definition);

    const innerRadius = getInnerRadius(definition, modelParameters);

    var arcInfo;
    var arcAngle;
    if (definition.hemType == SMHemType.ROLLED)
    {
        arcAngle = definition.angle;
        arcInfo = sketchInitialArc(sketch, wrapAroundFront, modelParameters, innerRadius, arcAngle);
    }
    else if (definition.hemType == SMHemType.TEAR_DROP)
    {
        const tipGap = definition.useMinimalGapForTipGap ? modelParameters.minimalClearance : definition.tipGap;

        if (definition.length < 2 * (innerRadius + modelParameters.frontThickness + modelParameters.backThickness) + (TOLERANCE.zeroLength * meter))
            throw regenError(ErrorStringEnum.SHEET_METAL_HEM_TOO_SHORT, ["length"]);
        if (tipGap > 2 * innerRadius - TOLERANCE.zeroLength * meter)
            throw regenError(ErrorStringEnum.SHEET_METAL_HEM_TEAR_DROP_GAP_TOO_LARGE, ["tipGap"]);

        arcInfo = sketchInitialArc(sketch, wrapAroundFront, modelParameters, innerRadius, 180 * degree);
        const lineInfo = sketchHorizontalLineAfterInitialArc(sketch, modelParameters, definition.length, arcInfo.arcEnd, innerRadius);
        const helperCapInfo = sketchTearDropHelperCap(sketch, modelParameters, lineInfo.lineEnd);
        applyTearDropConstraints(sketch, wrapAroundFront, modelParameters, arcInfo, lineInfo, helperCapInfo, definition.length, innerRadius, tipGap);
    }
    else if (definition.hemType == SMHemType.STRAIGHT)
    {
        arcAngle = 180 * degree;
        arcInfo = sketchInitialArc(sketch, wrapAroundFront, modelParameters, innerRadius, arcAngle);
        sketchHorizontalLineAfterInitialArc(sketch, modelParameters, definition.length, arcInfo.arcEnd, innerRadius);
    }
    else
    {
        // This cannot be hit by a UI user.
        throwHemTypeError(definition.hemType);
    }
    skSolve(sketch);

    const arcEdge = sketchEntityQuery(id, EntityType.EDGE, arcInfo.arcId);
    var arcEndPosition = evEdgeTangentLine(context, {
                "edge" : arcEdge,
                "parameter" : arcInfo.arcEndParameter
            }).origin;

    if (arcInfo.arcRadius == undefined)
    {
        throw "arc radius not found";
    }
    const arcRadius = arcInfo.arcRadius;

    if (arcAngle == undefined)
    {
        if (definition.hemType != SMHemType.TEAR_DROP)
        {
            throw "arcAngle should have been set"; // User will not hit this
        }
        // Must evaluate arc angle for tear drop after the solve since tear drop relies on constraints for positioning.
        const arcLength = evLength(context, { "entities" : arcEdge });
        // arc percent of circle = arc length / (2 * pi * r)
        // arc angle = 2 * pi * arc percent of circle
        // arc angle = arc length / r
        arcAngle = (arcLength / arcRadius) * radian;
    }

    const arcCenter = hemData.edgeCenterPosition + ((wrapAroundFront ? 1 : -1) * arcRadius * cross(hemData.outFromFace, hemData.edgeDirection));

    return {
            "arcEdge" : arcEdge,
            "arcAngle" : arcAngle,
            "arcRadius" : arcRadius,
            "arcCenter" : arcCenter,
            "arcEndPosition" : arcEndPosition
        };
}

/**
 * Construct the sketch such that the origin is at the center of the edge, +X is tangent to the face and pointing away from the face,
 * and +Y is the direction of back thickness of the sheet (a.k.a. the anti-normal of the face).
 */
function newHemProfileSketch(context is Context, id is Id, hemData is map) returns Sketch
{
    const sketchPlane = plane(hemData.edgeCenterPosition, hemData.edgeDirection, hemData.outFromFace);
    return newSketchOnPlane(context, id, {
                "sketchPlane" : sketchPlane
            });
}

/**
 * Sketch the initial arc of the hem, starting at the selected edge and wrapping either around the front of the back of
 * the sheet. Returns information about arc ids and arc end position.
 */
function sketchInitialArc(sketch is Sketch, wrapAroundFront is boolean, modelParameters is map,
    innerRadius is ValueWithUnits, angle is ValueWithUnits) returns map
{
    // "Front" of the sheet is -Y direction. "Back" of the sheet is +Y direction.
    const ySign = wrapAroundFront ? -1 : 1;
    const addition = wrapAroundFront ? modelParameters.frontThickness : modelParameters.backThickness;

    const arcRadius = innerRadius + addition;
    const circleCenter = vector(0 * inch, ySign * arcRadius);

    const arcMid = circleCenter + (arcRadius * vector(sin(angle / 2), ySign * -cos(angle / 2)));
    const arcEnd = circleCenter + (arcRadius * vector(sin(angle), ySign * -cos(angle)));

    const arcId = "initialHemArc";
    skArc(sketch, arcId, {
                "start" : vector(0, 0) * inch,
                "mid" : arcMid,
                "end" : arcEnd
            });

    return {
            "arcId" : arcId,
            // Internally, arc start and end are adjusted to be counter-clockwise. What we refer to as "start" (the root
            // point attached to the hem edge), may actually be the "end" internally.
            "arcStartId" : arcId ~ "." ~ (wrapAroundFront ? "end" : "start"),
            "arcEndId" : arcId ~ "." ~ (wrapAroundFront ? "start" : "end"),
            "arcEndParameter" : wrapAroundFront ? 0 : 1,
            "arcEnd" : arcEnd,
            "arcRadius" : arcRadius
        };
}

/**
 * Sketch the line segment extending from the initial arc of the hem for open and flattened hems. Returns information
 * about line ids and line end position.
 */
function sketchHorizontalLineAfterInitialArc(sketch is Sketch, modelParameters is map, totalHemLength is ValueWithUnits,
    arcEnd is Vector, innerRadius is ValueWithUnits) returns map
{
    const lineSegmentLength = totalHemLength - (innerRadius + modelParameters.frontThickness + modelParameters.backThickness);
    if (lineSegmentLength < (TOLERANCE.zeroLength * meter))
        throw regenError(ErrorStringEnum.SHEET_METAL_HEM_TOO_SHORT, ["length"]);
    const lineEnd = arcEnd - vector(lineSegmentLength, 0 * inch);

    const lineId = "horizontalLineAfterInitialArc";
    skLineSegment(sketch, lineId, {
                "start" : arcEnd,
                "end" : lineEnd
            });

    return {
            "lineId" : lineId,
            "lineStartId" : lineId ~ ".start",
            "lineEndId" : lineId ~ ".end",
            "lineEnd" : lineEnd
        };
}

/**
 * Sketch a cap at the end of a tear drop hem.  Once constrained, this will represent the thickened end of the tear drop
 * hem. Returns information about helper cap ids.
 */
function sketchTearDropHelperCap(sketch is Sketch, modelParameters is map, horizontalLineEnd is Vector) returns map
{
    // No matter whether we wrap around the front or the back, after wrapping the front thickness is now on top and the
    // back thickness is on bottom.
    const helperCapId = "helperCap";
    skLineSegment(sketch, helperCapId, {
                "start" : horizontalLineEnd - (vector(0 * inch, modelParameters.backThickness)),
                "end" : horizontalLineEnd + (vector(0 * inch, modelParameters.frontThickness)),
                "construction" : true
            });

    return {
            "helperCapId" : helperCapId,
            "helperCapFrontId" : helperCapId ~ ".end",
            "helperCapBackId" : helperCapId ~ ".start"
        };
}

/**
 * Apply constraints to turn an arc, horizontal line, and helper cap into a tear drop hem upon sketch solve.  After
 * searching for an analytical solution to the question "What should the attack angle of the initial arc be such that a
 * linear extension of the arc to a distance of minimal clearance from the sheet metal part creates a hem that covers
 * the total horizontal distance specified by the user?" It was found that any solution was too complicated to simplify,
 * and that employing the sketch solver was easier to maintain in the long run.
 */
function applyTearDropConstraints(sketch is Sketch, wrapAroundFront is boolean, modelParameters is map, arcInfo is map,
    lineInfo is map, helperCapInfo is map, totalHemLength is ValueWithUnits, innerRadius is ValueWithUnits, tipGap is ValueWithUnits)
{
    // Fix the start of the hem arc, it is already in the right place
    skConstraint(sketch, "fixRoot", {
                "constraintType" : ConstraintType.FIX,
                "localFirst" : arcInfo.arcStartId
            });
    // Fix the center and radius of the arc, these are well-defined.  This does not fix the end position of the arc
    skConstraint(sketch, "fixArcPos", {
                "constraintType" : ConstraintType.FIX,
                "localFirst" : arcInfo.arcId
            });
    // Attach the end of the arc to the beginning of the horizontal line
    skConstraint(sketch, "arcEndToLineStart", {
                "constraintType" : ConstraintType.COINCIDENT,
                "localFirst" : arcInfo.arcEndId,
                "localSecond" : lineInfo.lineStartId
            });
    // Ensure tangency between the line segment and the arc
    skConstraint(sketch, "arcTangentToLine", {
                "constraintType" : ConstraintType.TANGENT,
                "localFirst" : arcInfo.arcId,
                "localSecond" : lineInfo.lineId
            });
    // Attach the end of the horizontal line to the helper cap
    skConstraint(sketch, "lineEndToHelperCap", {
                "constraintType" : ConstraintType.COINCIDENT,
                "localFirst" : lineInfo.lineEndId,
                "localSecond" : helperCapInfo.helperCapId
            });
    // Make sure the helper cap stays perpendicular with the horizontal line
    skConstraint(sketch, "linePerpendicularToHelperCap", {
                "constraintType" : ConstraintType.PERPENDICULAR,
                "localFirst" : lineInfo.lineId,
                "localSecond" : helperCapInfo.helperCapId
            });
    // Make sure the helper cap points stay the correct distance from the end of the line
    skConstraint(sketch, "frontThicknessOnHelperFront", {
                "constraintType" : ConstraintType.DISTANCE,
                "localFirst" : lineInfo.lineEndId,
                "localSecond" : helperCapInfo.helperCapFrontId,
                "direction" : DimensionDirection.MINIMUM,
                "length" : modelParameters.frontThickness
            });
    skConstraint(sketch, "backThicknessOnHelperBack", {
                "constraintType" : ConstraintType.DISTANCE,
                "localFirst" : lineInfo.lineEndId,
                "localSecond" : helperCapInfo.helperCapBackId,
                "direction" : DimensionDirection.MINIMUM,
                "length" : modelParameters.backThickness
            });
    // Pull the end of the horizontal line down by pulling the inner helper cap endpoint to be at a distance of minimal clearance from the sheet metal part
    const distanceFromSheet = tipGap + (wrapAroundFront ? modelParameters.frontThickness : modelParameters.backThickness);
    const pointNearSheet = wrapAroundFront ? helperCapInfo.helperCapFrontId : helperCapInfo.helperCapBackId;
    // Make sure to use ANTI_ALIGNED when we specify a localFirst that is vertically "higher" than localSecond
    const alignment = wrapAroundFront ? DimensionAlignment.ANTI_ALIGNED : DimensionAlignment.ALIGNED;
    skConstraint(sketch, "minimumThicknessFromInnerHelperCapPoint", {
                "constraintType" : ConstraintType.DISTANCE,
                "localFirst" : arcInfo.arcStartId,
                "localSecond" : pointNearSheet,
                "direction" : DimensionDirection.VERTICAL,
                "length" : distanceFromSheet,
                "alignment" : alignment
            });
    // Constrain the width of the entire hem to the user specified value
    const distanceFromArcStartToFarHelperCap = totalHemLength - (innerRadius + modelParameters.frontThickness + modelParameters.backThickness);
    const farHelperPoint = wrapAroundFront ? helperCapInfo.helperCapBackId : helperCapInfo.helperCapFrontId;
    skConstraint(sketch, "UserDefinedWidth", {
                "constraintType" : ConstraintType.DISTANCE,
                "localFirst" : arcInfo.arcStartId,
                "localSecond" : farHelperPoint,
                "direction" : DimensionDirection.HORIZONTAL,
                "length" : distanceFromArcStartToFarHelperCap,
                "alignment" : DimensionAlignment.ANTI_ALIGNED // localFirst is to the right of localSecond
            });
}

// -------------------------------------------------- Bounding data calculation --------------------------------------------------

/**
 * Get the bounding data for the start and end of the hem. `edgeToHemData` should be in the format outputted by
 * [getEdgeToHemData], with the addition of augmentation by [augmentHemDataWithSketchInformation]
 * @returns {{
 *      @field startBoundingData {map} : Bounding data for the start of the edge, in the form returned by [getBoundingDataAtVertex]
 *      @field endBoundingData {map} : Bounding data for the end of the edge, in the form returned by [getBoundingDataAtVertex]
 * }}
 */
function getStartAndEndBoundingData(context is Context, edge is Query, edgeToHemData is map, definition is map, modelParameters is map, sharedVertexToMiteredBoundingData is box) returns map
{
    return {
            "start" : getBoundingDataAtVertex(context, edge, true, edgeToHemData, definition, modelParameters, sharedVertexToMiteredBoundingData),
            "end" : getBoundingDataAtVertex(context, edge, false, edgeToHemData, definition, modelParameters, sharedVertexToMiteredBoundingData)
        };
}

/**
 * Find the definition of the bounding plane for the given `edge`, at the given vertex (as defined by `isStart`)
 * @returns {{
 *      @field basePoint {Vector} : the base point at which the arc piece of the hem should start for the given vertex
 *      @field minimalClearanceBoundingPlane {Plane} : The boundary of the the thickened hem, including the necessary minimal clearance.
 *                                                     minimalClearanceBoundingPlane should be constructed such that its normal points
 *                                                     forward in the hemisphere defined by hemData.edgeDirection, regardless of isStart.
 *                                                     e.g. dot(minimalClearanceBoundingPlane.normal, hemData.edgeDirection) > 0
 *      @field isMiteredWithNeighborHem {boolean} : `true` if the minimalClearanceBoundingPlane is the result of a miter with a neighboring hem
 * }}
 */
function getBoundingDataAtVertex(context is Context, edge is Query, isStart is boolean, edgeToHemData is map, definition is map,
    modelParameters is map, sharedVertexToMiteredBoundingData is box) returns map
{
    const hemData = edgeToHemData[edge];
    const basePoint = hemData.startAndEndSurroundingGeometryData[isStart ? "start" : "end"].basePoint;
    const vertexInQuestion = isStart ? hemData.startVertex : hemData.endVertex;
    const vertexInQuestionPosition = isStart ? hemData.edgeStartPosition : hemData.edgeEndPosition;
    const tangentLineInQuestion = line(isStart ? hemData.edgeStartPosition : hemData.edgeEndPosition, hemData.edgeDirection);

    // ----- Check if we already have the bounding plane for this vertex -----
    if (sharedVertexToMiteredBoundingData[][vertexInQuestion] != undefined)
    {
        // We have already constructed a bounding plane for this vertex when processing its neighbor.
        return miteredBoundingDataToBoundingData(isStart, sharedVertexToMiteredBoundingData[][vertexInQuestion], basePoint);
    }

    // ----- Next, find the end plane assuming we do not have a neighboring hem -----
    const dataByGeometry = getBoundingDataBySurroundingGeometry(context, edge, isStart, vertexInQuestion, tangentLineInQuestion, hemData, definition, modelParameters);
    const initialBoundingData = {
            "basePoint" : basePoint,
            "minimalClearanceBoundingPlane" : dataByGeometry.minimalClearanceBoundingPlane,
            "isMiteredWithNeighborHem" : false
        };

    if (!dataByGeometry.checkForMiter)
    {
        return initialBoundingData;
    }

    // ----- Next, check if there is a neighboring hem to miter with -----
    const otherHemEdges = qSubtraction(qUnion(keys(edgeToHemData)), edge);
    const edgesOfVertexInQuestion = qAdjacent(vertexInQuestion, AdjacencyType.VERTEX, EntityType.EDGE);
    const neighboringHemEdges = evaluateQuery(context, qIntersection([otherHemEdges, edgesOfVertexInQuestion]));
    if (neighboringHemEdges == [])
    {
        // There is no neighboring hem, construct a simple bounding plane.
        return initialBoundingData;
    }

    // ----- There is a neighbor, do the appropriate work to form a miter plane -----
    if (size(neighboringHemEdges) != 1)
    {
        throw "Should not have found multiple neighbor hem edges";
    }
    const neighboringHemData = edgeToHemData[neighboringHemEdges[0]];

    const miteredBoundingData = getMiteredBoundingDataAtVertex(isStart, vertexInQuestion, tangentLineInQuestion, hemData,
            neighboringHemData, definition, modelParameters, sharedVertexToMiteredBoundingData);
    return miteredBoundingDataToBoundingData(isStart, miteredBoundingData, basePoint);
}

/**
 * Helper for creating a miter plane in defineHemBoundingPlaneAtVertex based on the sheet metal geometry surrounding the hem edge.
 * Does not account for any neighboring hems, only neighboring existing sheet metal geometry.
 * @returns {{
 *      @field minimalClearanceBoundingPlane {Plane}: See `getBoundingDataAtVertex(...)` documentation
 *      @field checkForMiter {boolean} : If true, the minimalClearanceBoundingPlane is safe to use as a finalized bounding plane.
 *                                       If false, we still need to check for neighboring hems to miter with before continuing
 * }}
 */
function getBoundingDataBySurroundingGeometry(context is Context, edge is Query, isStart is boolean, vertexInQuestion is Query,
    tangentLineInQuestion is Line, hemData is map, definition is map, modelParameters is map)
{
    const surroundingGeometryData = hemData.startAndEndSurroundingGeometryData[isStart ? "start" : "end"];
    const endPlane = surroundingGeometryData.endPlane;
    if (!surroundingGeometryData.sideEdgeIsTwoSided)
    {
        // The hem does not have any neighboring joints.  Use the canonical end plane, but check for neighboring hems (such
        // as two neighbors hems on a plate).
        return {
                "minimalClearanceBoundingPlane" : endPlane,
                "checkForMiter" : true
            };
    }

    // -- Check if the neighboring face (over the joint) infringes on the space for this hem --
    const neighboringJoint = surroundingGeometryData.sideEdge;
    const neighborFace = qSubtraction(qAdjacent(neighboringJoint, AdjacencyType.EDGE, EntityType.FACE), hemData.face);
    const neighborTangentPlane = evFaceTangentPlaneAtEdge(context, {
                "edge" : neighboringJoint,
                "face" : neighborFace,
                "parameter" : 0.5
            });

    const wrapAroundFront = getWrapAroundFront(definition);
    const hemForwardDirection = wrapAroundFront ? hemData.faceNormal : -hemData.faceNormal;
    const neighborForwardDirection = wrapAroundFront ? neighborTangentPlane.normal : -(neighborTangentPlane.normal);

    const intoEdge = isStart ? hemData.edgeDirection : -hemData.edgeDirection;
    if (dot(intoEdge, neighborForwardDirection) < TOLERANCE.zeroLength)
    {
        // Hem neighbors a joint, but the hem is going towards the "outside" of the joint.  No additional push back is needed,
        // and no miter should be made with a neighboring hem.
        return {
                "minimalClearanceBoundingPlane" : endPlane,
                "checkForMiter" : false
            };
    }

    if (dot(hemForwardDirection, neighborForwardDirection) > TOLERANCE.zeroLength)
    {
        // Hem neighbors a joint, and the hem is going towards the "inside" of the joint, but the joint is obtuse. No additional
        // push back is needed if this hem is alone, but it may need to be mitered with a neighbor.
        return {
                "minimalClearanceBoundingPlane" : endPlane,
                "checkForMiter" : true
            };
    }

    // -- The hem neighbors a joint, the hem is going towards the "inside" of the joint, and the joint is acute --
    // -- The neighboring face infringes on the hem's space; calculate and return the appropriate pushed back planes --
    // -- Do not skip the neighboring hem check, as a neighboring hem will infringe even further on this hem's space --
    const boundingPlaneDirection = isStart ? -neighborTangentPlane.normal : neighborTangentPlane.normal;

    const objectAttribute = getSmObjectTypeAttributes(context, neighboringJoint, SMObjectType.JOINT);
    if (size(objectAttribute) != 1)
    {
        throw "Did not find one joint attribute on two sided edge";
    }
    const jointType = objectAttribute[0].jointType.value;
    if (jointType == SMJointType.RIP)
    {
        // The neighbor may be thickened towards us.  Because of the nature of hem, the thickness of that sheet towards us is the inner thickness of the hem.
        const neighborForwardThickness = getInnerThickness(definition, modelParameters);
        const minimalClearancePushBack = modelParameters.minimalClearance + neighborForwardThickness;
        return {
                "minimalClearanceBoundingPlane" : plane(neighborTangentPlane.origin + (minimalClearancePushBack * neighborForwardDirection), boundingPlaneDirection),
                "checkForMiter" : true
            };
    }
    else if (jointType == SMJointType.BEND)
    {
        // Pull hems neighboring bends or tangents all the way back to the base point, but follow the neighboring face tangent plane
        return {
                "minimalClearanceBoundingPlane" : plane(surroundingGeometryData.basePoint, boundingPlaneDirection),
                "checkForMiter" : true
            };
    }
    else if (jointType == SMJointType.TANGENT)
    {
        throw "Tangent should have been caught by earlier checks";
    }
    else
    {
        throw "Unrecognized joint type";
    }
}

/**
 * Helper for creating a miter plane in defineHemBoundingPlaneAtVertex based on a miter between two neighboring hems.  The data returned by
 * this function can be transformed into `boundingData` using `miteredBoundingDataToBoundingData(...)`
 * @returns {{
 *      @field bisectorPlane {Plane}: the bisector plane that divides this hem and the neighboring hem
 *      @field minimalClearancePushBack {ValueWithUnits} : the adjustment that needs to be made to the bisector plane to represent the boundary of the thickened hem
 * }}
 */
function getMiteredBoundingDataAtVertex(isStart is boolean, vertexInQuestion is Query, tangentLineInQuestion is Line, hemData is map,
    neighboringHemData is map, definition is map, modelParameters is map, sharedVertexToMiteredBoundingData is box) returns map
{
    // Because we are using coEdge normals, the neighboring vertex will be at the end if isStart, or the start if !isStart.
    const neighboringVertexIsStart = !isStart;
    const neighboringTangentLineInQuestion = line(neighboringVertexIsStart ? neighboringHemData.edgeStartPosition : neighboringHemData.edgeEndPosition, neighboringHemData.edgeDirection);

    const bisector = normalize(tangentLineInQuestion.direction + neighboringTangentLineInQuestion.direction);
    var boundingPlane = plane(tangentLineInQuestion.origin, bisector);

    // Hems should be `minimalClearance` distance apart from each other
    const minimalClearancePushBack = 0.5 * modelParameters.minimalClearance;
    const returnMap = {
            "bisectorPlane" : boundingPlane,
            "minimalClearancePushBack" : minimalClearancePushBack
        };

    // Store the shared plane information in the box for later use
    sharedVertexToMiteredBoundingData[][vertexInQuestion] = returnMap;

    return returnMap;
}

/**
 *  Transform the data obtained from `getMiteredBoundingDataAtVertex(...)` into `boundingData`, suitable to be returned from `getBoundingDataAtVertex(...)`
 */
function miteredBoundingDataToBoundingData(isStart is boolean, miteredBoundingData is map, basePoint is Vector) returns map
{
    var minimalClearanceBoundingPlane = miteredBoundingData.bisectorPlane;
    minimalClearanceBoundingPlane.origin += (minimalClearanceBoundingPlane.normal * ((isStart ? 1.0 : -1.0) * miteredBoundingData.minimalClearancePushBack));
    return {
            "basePoint" : basePoint,
            "minimalClearanceBoundingPlane" : minimalClearanceBoundingPlane,
            "isMiteredWithNeighborHem" : true
        };
}

// -------------------------------------------------- Flattened arc calculations --------------------------------------------------

const CLOSEST_APPROACH_LOW_BOUND = 0.1 * degree;

/**
 * The intersection of the bounding plane and the hem cylinder is an ellipse.  If this ellipse is unrolled away from the face attached
 * to the hem edge, it forms a sinusoid on the hem face plane. Return a set of functions and information that can be used to
 * calculate properties of the sinusoid.  In the following descriptions, the "x" axis is the hem edge.
 * @returns {{
 *      @field getX {function} : Given a desired angle, return the x value of the sinusoid at that angle
 *      @field getInterceptData {function} : Given a desired angle, return a map containing:
 *                                               "xIntercept" : the x intercept of the tangent line of the sinusoid for the given angle.
 *                                               "newtonStep" : the value of the xIntersect / xIntersectPrime, where xIntersectPrime is
 *                                                              derivative of the x intercept function with respect to the angle (in
 *                                                              other words, the rate of change of the x intercept with respect to the
 *                                                              angle), at the given angle. This value is used for the newton's method
 *                                                              calculation in `findClosestApproachAngle(...)`.
 *      @field closestApproachRange {array} : the lower and upper bound for `findClosestApproachAngle(...)` for this specific sinusoid
 * }}
 */
function constructUnrolledSinusoidData(context is Context, basePoint is Vector, boundingPlane is Plane, isInner is boolean, isStart is boolean,
    hemData is map, definition is map, modelParameters is map) returns map
{
    // Devise a new coordinate system where the origin is the corner in question, the x-axis is direction is the direction of the coedge,
    // the y-axis will be away from the face (on the plane of the face), and the z direction is, therefore, the anti-normal of the face.
    const xAxis = hemData.edgeDirection;
    const yAxis = hemData.outFromFace;
    const zAxis = -hemData.faceNormal;

    // zAxis is face-antiAligned, so if we are looking at inner thickness and wrapping around the front OR outer thickness and wrapping around the back,
    // we push in -Z direction (along face normal) to get the origin. In the other two cases, push in +Z direction (against face normal).
    const thicknessSign = isInner ? -1 : 1;
    const relevantThickness = isInner ? getInnerThickness(definition, modelParameters) : getOuterThickness(definition, modelParameters);
    const relevantRadius = hemData.arcRadius + (thicknessSign * relevantThickness);

    const wrapAroundFront = getWrapAroundFront(definition);
    const basePointPushSign = isInner == wrapAroundFront ? -1 : 1;
    const origin = basePoint + zAxis * (basePointPushSign * relevantThickness);

    // Intersect the bounding plane with the new x-axis to find where it intersects the new x-axis.
    const boundingPlaneIntersection = findIntersectionPoint(boundingPlane, line(origin, xAxis));
    const boundingPlaneXOffset = dot(boundingPlaneIntersection - origin, xAxis);

    // The intersection of the the hem cylinder and the bounding plane will be an ellipse. Find the length of the projection of the major axis of
    // this ellipse onto the x-axis.  This is the amplitude of the sinusoid formed by unrolling the ellipse.
    // See diagram at: https://cad.onshape.com/documents/4a84a33eda062b3ab7b689d2/w/43445770d0bc13b0fcaa0b1d/e/16542f437559e1bcce6f874e
    // We are looking for xAxisProjectionLength = relevantRadius * tan(angle between xAxis and boundingPlane.normal)
    // decomposing: tan(angle between a1, a2) = sin(angle between a1, a2) / cos(angle between a1, a2) = norm(cross(a1, a2)) / dot(a1, a2)
    const xAxisProjectionLength = relevantRadius * (norm(cross(xAxis, boundingPlane.normal)) / dot(xAxis, boundingPlane.normal));

    // Calculate the vector pointing from the hem arc center to the point on the hem arc where the sinusioid is at its farthest "bump in"
    const boundingPlaneNormalOutwards = (isStart ? -1 : 1) * boundingPlane.normal;
    const hemCenterToLargestBumpInwards = boundingPlaneNormalOutwards - (dot(xAxis, boundingPlaneNormalOutwards) * xAxis);

    // Considering the start of the arc as 0, and having the angle become more positive as we move forward along the hem arc,
    // find the angle of the point on the hem arc where the intersection of the hem cyliner and the hem plane makes its farthest "bump in"
    const twoPi = 2 * PI;
    const arcCenterToHemEdge = wrapAroundFront ? zAxis : -zAxis;
    var angleOfLargestBumpInwards = atan2(dot(hemCenterToLargestBumpInwards, yAxis), dot(hemCenterToLargestBumpInwards, arcCenterToHemEdge));
    angleOfLargestBumpInwards = angleOfLargestBumpInwards % (twoPi * radian);

    // ----- Optimization: precalculate coefficients for the lambdas and capture them -----
    const amplitude = xAxisProjectionLength * (isStart ? 1 : -1);
    const initialCosValue = cos(angleOfLargestBumpInwards);
    const combinedConstants = (boundingPlaneXOffset / amplitude) - initialCosValue;

    return
    {
            "getInterceptData" : function(angle)
                {
                    // ----- Optimization: precalculate shared coefficients that depend on the input angle -----
                    const strippedAngle = stripUnits(angle);
                    const angleDiff = angle - angleOfLargestBumpInwards;
                    const cosAngleDiff = cos(angleDiff);
                    const sinTimesAnglePlusConstants = sin(angleDiff) * strippedAngle + combinedConstants;

                    // The following notes use abbreviations:
                    //     A = amplitude
                    //     angle0 = angleOfLargestBumpInwards
                    //     x0 = boundingPlaneXOffset
                    //     R = relevantRadius

                    // ----- Important notes about x -----
                    // -- the cosinusoid intercepts xAxis at x0, and is at angle0 into its period at that point --
                    // getX(angle) = A * (cos(angle - angle0) - cos(angle0)) + x0
                    //             = A * (cos(angle - angle0) + (x0 / A) - cos(angle0))
                    // -- derivative --
                    // dXdAngle(angle) = A * -sin(angle - angle0)

                    // ----- Important notes about y -----
                    // -- y travels two full radii from [0, 2pi]
                    // getY(angle) = R * (angle / radian)
                    // -- In math terms, (angle / radian) is equivalent to 'angle in radians' so we can treat it as one term for the derivative.
                    // (angle / radian) = Y / R
                    // dAngledY(angle) = 1 / R

                    return {
                            // xIntercept =
                            // = getX(angle) - dXdY(angle) * getY(angle)
                            // = getX(angle) - dXdAngle(angle) * dAngledY(angle) * getY(angle)
                            // = getX(angle) - dXdAngle(angle) * (1 / R) * (R * (angle / radian))
                            // = getX(angle) - dXdAngle(angle) * (angle / radian)
                            // = ( A * (cos(angle - angle0) + (x0 / A) - cos(angle0)) ) - ( A * -sin(angle - angleOfLargestBumpInwards) ) * (angle / radian)
                            // = A * (cos(angle - angle0) + sin(angle - angle0) * (angle / radian) + (x0 / A) - cos(angle0))
                            "xIntercept" : amplitude * (cosAngleDiff + sinTimesAnglePlusConstants),

                            // xInterceptPrime =
                            // = d(xIntercept)dAngle
                            // = d( A * (cos(angle - angle0) - cos(angle0) + sin(angle - angle0) * (angle / radian)) + x0 )dAngle
                            // = A * d( cos(angle - angle0) + sin(angle - angle0) * (angle / radian) )dAngle
                            // = A * (-sin(angle - angle0) + d( sin(angle - angle0) * (angle / radian))dAngle )
                            // = A * (-sin(angle - angle0) + sin(angle - angle0) + cos(angle - angle0) * (angle / radian))
                            // = A * cos(angle - angle0) * (angle / radian)
                            // We never actually use this value, besides to calculate the next one
                            // "xInterceptPrime" : amplitude * cosAngleDiff * strippedAngle,

                            // newtonStep =
                            // = xIntercept / xInterceptPrime
                            // = (A * (cos(angle - angle0) + sin(angle - angle0) * (angle / radian) + (x0 / A) - cos(angle0))) / (A * cos(angle - angle0) * (angle / radian))
                            // = ((cos(angle - angle0) + sin(angle - angle0) * (angle / radian) + (x0 / A) - cos(angle0))) / (cos(angle - angle0) * (angle / radian))
                            "newtonStep" : (cosAngleDiff + sinTimesAnglePlusConstants) / (cosAngleDiff * strippedAngle)
                        };
                },
            "getX" : function(angle)
                {
                    // See notes on x above.
                    return amplitude * (cos(angle - angleOfLargestBumpInwards) + combinedConstants);
                },
            // Always use a lower boundary greater than 0 to avoid divide/multiply by 0 issues down the line.
            "closestApproachRange" : [max(CLOSEST_APPROACH_LOW_BOUND, angleOfLargestBumpInwards - 90 * degree), angleOfLargestBumpInwards]
        };
}

enum PreviousClamp
{
    NEITHER,
    TOP,
    BOTTOM
}

/**
 * `unrolledSinusoidData` represents the boundary ellipse which we must not cross, or risk intersecting with neighboring
 * geometry.  Fit a line that starts at the (0, 0) (which has been constructed as the base point where the hem corner
 * MUST start), and is tangent with the sinusoid in question.  When rolled, this line will form a helix.  By seeking out
 * tangency, we guarantee that 1) the the helix does not infringe into neighboring space and 2) the helix stays as close
 * to neighboring space as possible.  In other words, ensure that the helix formed by the line has one point of contact
 * with the minimal clearance plane in question.
 *
 * Return the angle of the sinusoid that the line must pass through to achieve closest approach.
 */
function findClosestApproachAngle(unrolledSinusoidData is map) returns ValueWithUnits
{
    const absoluteTop = unrolledSinusoidData.closestApproachRange[1];
    const absoluteTopRaw = stripUnits(absoluteTop);
    const absoluteBottom = unrolledSinusoidData.closestApproachRange[0];
    const absoluteBottomRaw = stripUnits(absoluteBottom);

    var steps = 0;
    var previousClamp = PreviousClamp.NEITHER;
    var currAngle = (absoluteTop + absoluteBottom) / 2;
    while (true)
    {
        steps += 1;
        if (steps > 30)
        {
            // Newton's method should never need to go this far.
            throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
        }

        const interceptData = unrolledSinusoidData.getInterceptData(currAngle);

        // Use two comparisons on a stipped value to avoid performance cost of `abs()` and ValueWithUnits comparison.
        const xInterceptStripped = stripUnits(interceptData.xIntercept);
        if (xInterceptStripped > -TOLERANCE.zeroLength && xInterceptStripped < TOLERANCE.zeroLength)
        {
            // Found an exact match
            break;
        }

        // Newton's Method
        const currAngleRaw = stripUnits(currAngle) - interceptData.newtonStep;

        // Clamp currAngle, and return if the desired value is outside of the range.
        if (currAngleRaw < absoluteBottomRaw)
        {
            currAngle = absoluteBottom;
            if (previousClamp == PreviousClamp.BOTTOM)
            {
                // We clamped last step, but are still trying to move past the clamp
                break;
            }
            previousClamp = PreviousClamp.BOTTOM;
        }
        else if (currAngleRaw > absoluteTopRaw)
        {
            currAngle = absoluteTop;
            if (previousClamp == PreviousClamp.TOP)
            {
                // We clamped last step, but are still trying to move past the clamp
                break;
            }
            previousClamp = PreviousClamp.TOP;
        }
        else
        {
            currAngle = currAngleRaw * radian;
            previousClamp = PreviousClamp.NEITHER;
        }
    }

    return currAngle;
}

// -------------------------------------------------- Sheet creation --------------------------------------------------

/**
 * Construct the hem sheets. Return the created hem sheets as a map.
 * @returns {{
 *      @field arcSheet {Query} : a query for the arc sheet
 *      @field otherSheet {Query} : a query for the sheet representing the rest of the hem
 * }}
 */
function constructHemSheets(context is Context, id is Id, hemData is map, startAndEndBoundingData is map, definition is map,
    modelParameters is map, bodiesToDelete is box) returns map
{
    // For historical parity, it is important that the same id is used for hem sheet extrusion.
    const arcSheetId = id + "arcSheet";
    const arcSheetExtrudeId = arcSheetId + "extrude";

    var arcSheet;
    if (definition.hemCornerType == SMHemCornerType.SIMPLE)
    {
        arcSheet = constructSimpleHemArcSheet(context, arcSheetId, arcSheetExtrudeId, hemData, startAndEndBoundingData, definition,
                modelParameters, bodiesToDelete);
    }
    else if (definition.hemCornerType == SMHemCornerType.CLOSED)
    {
        arcSheet = constructClosedHemArcSheet(context, arcSheetId, arcSheetExtrudeId, hemData, startAndEndBoundingData, definition,
                modelParameters, bodiesToDelete);
    }
    else
    {
        throw "Unrecognized hem corner type";
    }

    const otherSheet = constructOtherSheet(context, id + "otherSheet", arcSheet, hemData, startAndEndBoundingData, bodiesToDelete);

    return {
            "arcSheet" : arcSheet,
            "otherSheet" : otherSheet
        };
}

// -------------------------------------------------- Sheet creation - simple hem arc --------------------------------------------------

/**
 * For SIMPLE, construct bounding helices based on the unrolled sinusoid data, and extrude up to them using extrude up-to-wire.
 * This geometry will unfold as a straight line in the flat.
 */
function constructSimpleHemArcSheet(context is Context, id is Id, extrudeId is Id, hemData is map, startAndEndBoundingData is map,
    definition is map, modelParameters is map, bodiesToDelete is box) returns Query
{
    const boundingGeometryId = id + "simpleBoundingGeometry";
    const startBoundingMap = constructSimpleBoundingGeometryForSideIfNecessary(context, boundingGeometryId + "start", true, hemData,
            startAndEndBoundingData.start, definition, modelParameters);
    const endBoundingMap = constructSimpleBoundingGeometryForSideIfNecessary(context, boundingGeometryId + "end", false, hemData,
            startAndEndBoundingData.end, definition, modelParameters);
    addBodiesToDelete(bodiesToDelete, qCreatedBy(boundingGeometryId, EntityType.BODY));

    extrudeWithBoundingMaps(context, extrudeId, hemData.arcEdge, startBoundingMap, endBoundingMap, hemData, { "allowUpToWire" : true });
    return qCreatedBy(extrudeId, EntityType.BODY);
}

// A little less than a degree (in terms of percent of a full revolution)
const HELIX_NUDGE = 0.0025;

/**
 * Build a bounding helix for a SIMPLE hem corner, if necessary. In certain cases, it will be possible to just use a blind extrude. Return a bounding
 * map consumable by `extrudeWithBoundingMaps(...)`.
 */
function constructSimpleBoundingGeometryForSideIfNecessary(context is Context, id is Id, isStart is boolean, hemData is map, boundingData is map,
    definition is map, modelParameters is map) returns map
{
    // Build a helix (representing a line in the flat) to act as the bounding geometry for the side. Control the helix by finding
    // the angle of closest approach, and the lateral distance of the helix at that angle.
    const closestApproachData = getClosestApproachData(context, isStart, hemData, boundingData, definition, modelParameters);

    if (closestApproachData.useBlindExtrude)
    {
        // Helix is actually just a planar arc.  opHelix will fail, use blind extrude.
        // (Note: we used to go up to a plane here, but using blind is a nice optimization)
        return makeBlindBoundingMap(isStart, boundingData.basePoint, hemData);
    }
    else
    {
        // Construct the appropriate helix.
        const helixId = id + "helix";
        try
        {
            opHelix(context, helixId, {
                        "direction" : (isStart ? 1 : -1) * hemData.edgeDirection,
                        "axisStart" : hemData.arcCenter,
                        "startPoint" : boundingData.basePoint,
                        "interval" : [0., (hemData.arcAngle / (2 * PI * radian)) + HELIX_NUDGE], // Add a a bit to smooth out issues
                        "clockwise" : getWrapAroundFront(definition) != isStart,
                        "helicalPitch" : closestApproachData.xDiffInward * ((2 * PI) / closestApproachData.closestApproachAngle),
                        "spiralPitch" : 0 * inch
                    });
        }
        catch
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
        }
        return makeUpToBoundingMap(isStart, BoundingType.UP_TO_BODY, qCreatedBy(helixId, EntityType.BODY));
    }
}

/**
 * Find the angle of closest approach for the helix, and how far the helix should push in at that angle.
 * @returns {{
 *     @field useBlindExtrude {boolean} : `true` if the helix is unecessary, and a blind extrude up to the hem base point can be used instead.
 *     @field closestApproachAngle {ValueWithUnits} : the closest approach for the helix.  `undefined` if `blindExtrude` is `true`.
 *     @field xDiffInward {ValueWithUnits} : how far inward the helix should push at `closestApproachAngle`.  `undefined` if `blindExtrude` is `true`.
 * }}
 */
function getClosestApproachData(context is Context, isStart is boolean, hemData is map, boundingData is map,
    definition is map, modelParameters is map) returns map
{
    const boundingPlane = boundingData.minimalClearanceBoundingPlane;

    var closestApproachAngle;
    var xDiffAtClosestApproachAngle;
    if (parallelVectors(hemData.edgeDirection, boundingPlane.normal))
    {
        // Edge and bounding plane normal are parallel.  The intersection of the hem arc sheet and the bounding plane is a circle, and
        // the lateral distance of the intersection from the base point is the same for every angle.
        closestApproachAngle = hemData.arcAngle;

        // The sign here is important.  The number should represent the distance from the base point to the boundingPlane, with
        // edge direction as positive x.  Note that the boundingPlane does not necessarily pass through the base point, so we may still
        // be building a helix here to make the base point "spin up" to the boundingPlane.
        xDiffAtClosestApproachAngle = dot(boundingPlane.origin - boundingData.basePoint, hemData.edgeDirection);
    }
    else
    {
        // Edge and bouding plane normal are not parallel.  The intersection of the hem arc sheet and the bounding plane is an ellipse,
        // which unrolls as a sinusioid.  Use the sinusoid to find the closest approach angle and its lateral distance.
        const unrolledSinusoidData = constructUnrolledSinusoidData(context, boundingData.basePoint, boundingPlane, false, isStart,
                hemData, definition, modelParameters);
        if (hemData.arcAngle < (unrolledSinusoidData.closestApproachRange[0] + (TOLERANCE.zeroAngle * radian)))
        {
            // Angle which we are going to build does not enter the range of possible closestApproaches.  Use the angle
            // we are going to build as the closest approach so that the helix "spins up" and ends piercing the bounding plane
            closestApproachAngle = hemData.arcAngle;
        }
        else
        {
            // -- Optimization: If there is absolutely no chance that the hem helix will pull inward, we can
            //    skip the search due to BEL-115314 (SIMPLE hem helix should never push outward), and make the
            //    hem corner go straight forward --
            const xDiffAtFurthestBumpIn = unrolledSinusoidData.getX(unrolledSinusoidData.closestApproachRange[1]);
            if (xDiffZeroOrOutward(isStart, xDiffAtFurthestBumpIn))
            {
                return { "useBlindExtrude" : true };
            }

            // Angle which we are going to build may be larger than the closest approach.  We must search to find the closest approach.
            closestApproachAngle = min(hemData.arcAngle, findClosestApproachAngle(unrolledSinusoidData));
        }
        xDiffAtClosestApproachAngle = unrolledSinusoidData.getX(closestApproachAngle);
    }

    // BEL-115314: SIMPLE hem helix should never push outward
    if (xDiffZeroOrOutward(isStart, xDiffAtClosestApproachAngle))
    {
        return { "useBlindExtrude" : true };
    }
    return {
        "useBlindExtrude" : false,
        "closestApproachAngle" : closestApproachAngle,
        "xDiffInward" : abs(xDiffAtClosestApproachAngle)
    };
}

function xDiffZeroOrOutward(isStart is boolean, xDiff is ValueWithUnits) returns boolean
{
    const zeroLengthMeters = TOLERANCE.zeroLength * meter;
    return (isStart && xDiff < zeroLengthMeters || !isStart && xDiff > -zeroLengthMeters);
}

// -------------------------------------------------- Sheet creation - closed hem arc --------------------------------------------------

/**
 * For CLOSED, create a sheet bounded by the minimum clearance bounding plane on both the inner and outer arc of the hem.
 * Offset the one that does not match the definition body, then intersect the two sheets. This creates geometry that is bounded
 * exactly at the minimum clearance bounding plane.
 *
 * This may leave geometry which would create a step edge in the built-out model, so correct the step edge by building
 * sliver bodies that connect or subtract to ensure the hem arc touches the base point.
 * }}
 */
function constructClosedHemArcSheet(context is Context, id is Id, extrudeId is Id, hemData is map, startAndEndBoundingData is map,
    definition is map, modelParameters is map, bodiesToDelete is box) returns Query
{
    const planesId = id + "closedBoundingPlanes";
    const boundingMaps = {
            "start" : constructExtrudeBoundingPlaneIfNecessary(context, planesId + "start", true,
                    startAndEndBoundingData.start.minimalClearanceBoundingPlane, hemData, bodiesToDelete),
            "end" : constructExtrudeBoundingPlaneIfNecessary(context, planesId + "end", false,
                    startAndEndBoundingData.end.minimalClearanceBoundingPlane, hemData, bodiesToDelete)
        };
    const bothBoundsBlind = boundingMaps.start.startBound == BoundingType.BLIND && boundingMaps.end.endBound == BoundingType.BLIND;

    // Construct the inner and outer sheets, bounded by the same bounding planes, and offset their size so that they both
    // match the underlying sheet metal definition body. Then, intersect the two sheets to  get the final shape of the hem
    // arc sheet tool.
    const arcSheet = constructBaseClosedHemArcSheet(context, id, extrudeId, boundingMaps, hemData,
            definition, modelParameters, bodiesToDelete);

    // -- Optimization : offset and intersect will have no effect if the bounds are trivial blinds --
    if (!bothBoundsBlind)
    {
        const intersectionTool = constructClosedHemArcSheetIntersectTool(context, id + "offsetExtrude", boundingMaps, hemData,
                definition, modelParameters, bodiesToDelete);

        try
        {
            opBoolean(context, id + "intersect", {
                        "targets" : arcSheet,
                        "tools" : intersectionTool,
                        "operationType" : BooleanOperationType.SUBTRACT_COMPLEMENT,
                        "allowSheets" : true,
                        "eraseImprintedEdges" : false
                    });
        }
        catch
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
        }
    }

    // Make an adjustment to remove any step faces between the base point and the arc sheet
    adjustToAvoidStepFaces(context, id + "avoidStepFaces", arcSheet, hemData, startAndEndBoundingData, definition,
            modelParameters, bodiesToDelete);

    return arcSheet;
}

/**
 * Create a hem arc which matches the definition sheet.
 */
function constructBaseClosedHemArcSheet(context is Context, id is Id, extrudeId is Id, boundingMaps is map, hemData is map, definition is map,
    modelParameters is map, bodiesToDelete is box) returns Query
{
    return constructClosedHemArcSheetForRadius(context, id, extrudeId, hemData.arcRadius, boundingMaps, hemData, definition,
            modelParameters, bodiesToDelete);
}

/**
 * Create a hem arc sheet for the inner or outer arc, whichever does not match the definition sheet, then thicken that sheet up to the location
 * of the hem arc sheet.
 */
function constructClosedHemArcSheetIntersectTool(context is Context, id is Id, boundingMaps is map, hemData is map, definition is map,
    modelParameters is map, bodiesToDelete is box) returns Query
{
    const innerThickness = -getInnerThickness(definition, modelParameters);
    const hasInnerThickness = !tolerantEquals(innerThickness, 0 * meter);

    const outerThickness = getOuterThickness(definition, modelParameters);
    const hasOuterThickness = !tolerantEquals(outerThickness, 0 * meter);

    if (hasInnerThickness == hasOuterThickness)
    {
        // There should only be one thickness, sheet metal is too old for hem.
        throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
    }

    return constructClosedHemArcSheetForRadius(context, id, id + "extrude", hemData.arcRadius + innerThickness + outerThickness,
            boundingMaps, hemData, definition, modelParameters, bodiesToDelete);
}

/**
 * Construct a hem arc sheet of the given radius with closed boundaries at either end.
 *
 * Once the hem sheet is created, if the radius of the sheet does not match `hemData.arcRadius`, transform the sheet so that
 * the sheet's radius matches `hemData.arcRadius`.
 */
function constructClosedHemArcSheetForRadius(context is Context, id is Id, extrudeId is Id, arcRadius is ValueWithUnits,
    boundingMaps is map, hemData is map, definition is map, modelParameters is map, bodiesToDelete is box) returns Query
{
    const sketchedArcRadius = hemData.arcRadius;

    var needsTransform = false;
    var arcToExtrude = hemData.arcEdge;
    if (!tolerantEquals(arcRadius, sketchedArcRadius))
    {
        const arcCopyId = id + "arcCopy";
        try
        {
            opPattern(context, arcCopyId, {
                        "entities" : qOwnerBody(hemData.arcEdge),
                        "transforms" : [scaleUniformly(arcRadius / sketchedArcRadius, hemData.arcCenter)],
                        "instanceNames" : ["arcCopy"]
                    });
        }
        catch
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
        }
        addBodiesToDelete(bodiesToDelete, qCreatedBy(arcCopyId, EntityType.BODY));
        arcToExtrude = qCreatedBy(arcCopyId, EntityType.EDGE);
        needsTransform = true;
    }

    extrudeWithBoundingMaps(context, extrudeId, arcToExtrude, boundingMaps.start, boundingMaps.end, hemData);
    const arcSheet = qCreatedBy(extrudeId, EntityType.BODY);

    if (needsTransform)
    {
        const scaleFactor = sketchedArcRadius / arcRadius;
        const cylinderCsys = coordSystem(hemData.arcCenter, hemData.edgeDirection, hemData.outFromFace);
        try
        {
            opTransform(context, id + "transform", {
                        "bodies" : arcSheet,
                        "transform" : toWorld(cylinderCsys) * scaleNonuniformly(1, scaleFactor, scaleFactor) * fromWorld(cylinderCsys)
                    });
        }
        catch
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
        }
    }
    return arcSheet;
}

// -------------------------------------------------- Sheet creation - closed hem arc - step face adjustment --------------------------------------------------

// TODO: test more cases of this.  Right now flattened comes out with an ugly lip when executed on a box.
// It seems like the correct thing to do might be to scale this by ratio of vertexToBasePointDistance -> innerRadius
const ANGLE_TO_COINCIDE_AT = 45 * degree;

enum StepFaceType
{
    NO_STEP,
    STEP_INSIDE_BASE_POINT,
    STEP_OUTSIDE_BASE_POINT
}

/**
 * Add or remove a small piece of material on each side of the arcSheet to make sure the arcSheet touches the base points defined in
 * startAndEndBoundingData.  The piece of material will start at the base point and meet the existing arc sheet side edge ANGLE_TO_COINCIDE_AT
 * degrees into the arc.
 */
function adjustToAvoidStepFaces(context is Context, id is Id, arcSheet is Query, hemData is map,
    startAndEndBoundingData is map, definition is map, modelParameters is map, bodiesToDelete is box)
{
    const startAndEndStepFaceData = getStartAndEndStepFaceData(context, arcSheet, hemData, startAndEndBoundingData);
    if (startAndEndStepFaceData.start.stepFaceType == StepFaceType.NO_STEP && startAndEndStepFaceData.end.stepFaceType == StepFaceType.NO_STEP)
    {
        // Nothing to adjust
        return;
    }

    // Sketch a small arc segment covering ANGLE_TO_COINCIDE_AT degrees
    const sketchData = sketchStepFaceAvoidanceArcSegment(context, id + "sketchArcSegment", hemData, definition, modelParameters, bodiesToDelete);

    if (startAndEndStepFaceData.start.stepFaceType != StepFaceType.NO_STEP)
    {
        const adjustReturn = adjustToAvoidStepFaceForSide(context, id + "startStepFaceAdjust", true, arcSheet,
                startAndEndStepFaceData.start, sketchData, hemData, startAndEndBoundingData.start, bodiesToDelete);
    }
    if (startAndEndStepFaceData.end.stepFaceType != StepFaceType.NO_STEP)
    {
        const adjustReturn = adjustToAvoidStepFaceForSide(context, id + "endStepFaceAdjust", false, arcSheet,
                startAndEndStepFaceData.end, sketchData, hemData, startAndEndBoundingData.end, bodiesToDelete);
    }
}

/**
 * Get information about the existing boundary for start and end.  See [getStepFaceDataForSide] for the contents of the data.
 */
function getStartAndEndStepFaceData(context is Context, arcSheet is Query, hemData is map, startAndEndBoundingData is map) returns map
{
    const rootEdge = qContainsPoint(qOwnedByBody(arcSheet, EntityType.EDGE), hemData.edgeCenterPosition);
    const bothVertices = qAdjacent(rootEdge, AdjacencyType.VERTEX, EntityType.VERTEX);
    const startVertex = qFarthestAlong(bothVertices, -hemData.edgeDirection);
    const endVertex = qSubtraction(bothVertices, startVertex);

    return {
            "start" : getStepFaceDataForSide(context, true, rootEdge, startVertex, hemData, startAndEndBoundingData.start),
            "end" : getStepFaceDataForSide(context, false, rootEdge, endVertex, hemData, startAndEndBoundingData.end)
        };
}

/**
 * @returns {{
 *     @field stepFaceType {StepFaceType} : The type of step that currently exists for the given side.
 *     @field existingEdge {Query} : The first bounding edge off of rootEdge for the given side. `undefined` if stepFaceType is NO_STEP.
 * }}
 */
function getStepFaceDataForSide(context is Context, isStart is boolean, rootEdge is Query, vertexInQuestion is Query, hemData is map, boundingData is map) returns map
{
    const outFromCenter = isStart ? -hemData.edgeDirection : hemData.edgeDirection;
    const vertexPosition = evVertexPoint(context, { "vertex" : vertexInQuestion });
    const vertexToBasePointDistance = dot(boundingData.basePoint - vertexPosition, outFromCenter);

    if (tolerantEquals(vertexToBasePointDistance, 0 * meter))
    {
        return {
                "stepFaceType" : StepFaceType.NO_STEP
            };
    }
    else
    {
        return {
                "stepFaceType" : vertexToBasePointDistance > 0 * meter ? StepFaceType.STEP_INSIDE_BASE_POINT : StepFaceType.STEP_OUTSIDE_BASE_POINT,
                "existingEdge" : qSubtraction(qAdjacent(vertexInQuestion, AdjacencyType.VERTEX, EntityType.EDGE), rootEdge)
            };
    }
}

/**
 * Sketch an arc covering ANGLE_TO_COINCIDE_AT degrees. This arc will be extruded to created step face avoidance tools.
 * @returns {{
 *     @field arcEdge {Query} : The sketched arc segment
 *     @field farArcVertexPosition {Vector} : The position of the vertex at the far end of the arc segment
 * }}
 */
function sketchStepFaceAvoidanceArcSegment(context is Context, id is Id, hemData is map, definition is map, modelParameters is map,
    bodiesToDelete is box) returns map
{
    const sketch = newHemProfileSketch(context, id, hemData);
    sketchInitialArc(sketch, getWrapAroundFront(definition), modelParameters, getInnerRadius(definition, modelParameters), ANGLE_TO_COINCIDE_AT);
    skSolve(sketch);

    addBodiesToDelete(bodiesToDelete, qCreatedBy(id, EntityType.BODY));

    const arcEdge = qCreatedBy(id, EntityType.EDGE);
    const bothArcVertices = qAdjacent(arcEdge, AdjacencyType.VERTEX, EntityType.VERTEX);
    const farArcVertex = qSubtraction(bothArcVertices, qContainsPoint(bothArcVertices, hemData.edgeCenterPosition));
    const farArcVertexPosition = evVertexPoint(context, { "vertex" : farArcVertex });

    return {
            "arcEdge" : arcEdge,
            "farArcVertexPosition" : farArcVertexPosition
        };
}

/**
 * Adjust the side of the sheet by extruding an additive or subtractive tool, for STEP_INSIDE_BASE_POINT and STEP_OUTSIDE_BASE_POINT
 * respectively.
 */
function adjustToAvoidStepFaceForSide(context is Context, id is Id, isStart is boolean, arcSheet is Query, stepFaceData is map,
    sketchData is map, hemData is map, boundingData is map, bodiesToDelete is box)
{
    // Create a plane that passes through both base point and the intersection of the existing edge with a line along the end of the
    // sketch arc. In rare cases, the existing edge may not cover the full ANGLE_TO_COINCIDE_AT, but evDistanceShould handle this nicely,
    // and we will just use the endpoint of the existing edge instead.
    const lineAlongArcSegmentEnd = line(sketchData.farArcVertexPosition, hemData.edgeDirection);
    const distanceReturn = evDistance(context, {
                "side0" : stepFaceData.existingEdge,
                "side1" : lineAlongArcSegmentEnd
            });
    const pointOnExistingEdge = distanceReturn.sides[0].point;

    const definingDirection = pointOnExistingEdge - boundingData.basePoint;
    const boundingPlaneDef = plane(boundingData.basePoint, normalize(cross(hemData.outFromFace, definingDirection)));
    const boundingPlane = constructPlane(context, id + "plane", boundingPlaneDef);
    addBodiesToDelete(bodiesToDelete, qOwnerBody(boundingPlane));

    // Extrude hasn't been executed yet, but set up some common queries before the branch
    const extrudeId = id + "extrude";
    const createdSheet = qCreatedBy(extrudeId, EntityType.BODY);

    const booleanId = id + "boolean";
    const booleanMatches = [{
                "topology1" : qOwnedByBody(arcSheet, EntityType.FACE),
                "topology2" : qOwnedByBody(createdSheet, EntityType.FACE),
                "matchType" : TopologyMatchType.OVERLAPING
            }];

    if (stepFaceData.stepFaceType == StepFaceType.STEP_INSIDE_BASE_POINT)
    {
        try
        {
            // Extrude an additive tool from the center of the hem arc to the boundingPlane, and add it to the arc sheet.
            // A simple example of reaching this case is two neighboring hems on a plate.
            opExtrude(context, extrudeId, {
                        "entities" : sketchData.arcEdge,
                        "direction" : isStart ? -hemData.edgeDirection : hemData.edgeDirection,
                        "endBound" : BoundingType.UP_TO_SURFACE,
                        "endBoundEntity" : boundingPlane
                    });

            opBoolean(context, booleanId, {
                        "tools" : qUnion([arcSheet, createdSheet]),
                        "operationType" : BooleanOperationType.UNION,
                        "matches" : booleanMatches,
                        "allowSheets" : true
                    });
        }
        catch
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
        }
    }
    else if (stepFaceData.stepFaceType == StepFaceType.STEP_OUTSIDE_BASE_POINT)
    {
        try
        {
            // Extrude a subtractive tool from the bounding plane through all, and remove it from the arc sheet.
            // A simple example of reaching this case is two neighboring hems on the top edges of a box, where the selected edges have a
            // bend between them.
            opExtrude(context, extrudeId, {
                        "entities" : sketchData.arcEdge,
                        "direction" : isStart ? -hemData.edgeDirection : hemData.edgeDirection,
                        "startBound" : BoundingType.UP_TO_SURFACE,
                        "startBoundEntity" : boundingPlane,
                        "isStartBoundOpposite" : false,
                        "endBound" : BoundingType.THROUGH_ALL
                    });

            opBoolean(context, booleanId, {
                        "targets" : arcSheet,
                        "tools" : createdSheet,
                        "operationType" : BooleanOperationType.SUBTRACTION,
                        "matches" : booleanMatches,
                        "allowSheets" : true
                    });
        }
        catch
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
        }
    }
    else
    {
        throw "Unrecognized StepFaceType";
    }
}

// -------------------------------------------------- Sheet creation - other sheet --------------------------------------------------

/**
 * Extrude the rest of the hem besides the initial arc.  This may be nothing for a ROLLED hem, or a planar section for
 * a STRAIGHT or TEAR_DROP hem.
 */
function constructOtherSheet(context is Context, id is Id, arcSheet is Query, hemData is map, startAndEndBoundingData is map,
    bodiesToDelete is box) returns Query
{
    const sketchEdges = qConstructionFilter(qCreatedBy(hemData.profileSketchId, EntityType.EDGE), ConstructionObject.NO);
    const otherSketchEdges = qSubtraction(sketchEdges, hemData.arcEdge);
    if (isQueryEmpty(context, otherSketchEdges))
    {
        return qNothing();
    }

    // Bound each sheet by a plane rooted wherever the hem arc ends (to avoid step faces between
    // the arc sheet and the other sheet).
    const farEdgeVertices = qAdjacent(qContainsPoint(qOwnedByBody(arcSheet, EntityType.EDGE), hemData.arcEndPosition), AdjacencyType.VERTEX, EntityType.VERTEX);
    const startFinalPoint = evVertexPoint(context, { "vertex" : qClosestTo(farEdgeVertices, startAndEndBoundingData.start.basePoint) });
    const endFinalPoint = evVertexPoint(context, { "vertex" : qClosestTo(farEdgeVertices, startAndEndBoundingData.end.basePoint) });

    const planeId = id + "planes";
    const startBoundingMap = constructBoundingPlaneForOtherSheetIfNecessary(context, planeId + "startBoundingPlane", true, startFinalPoint,
            startAndEndBoundingData.start, hemData, bodiesToDelete);
    const endBoundingMap = constructBoundingPlaneForOtherSheetIfNecessary(context, planeId + "endBoundingPlane", false, endFinalPoint,
            startAndEndBoundingData.end, hemData, bodiesToDelete);

    const extrudeId = id + "extrude";
    extrudeWithBoundingMaps(context, extrudeId, otherSketchEdges, startBoundingMap, endBoundingMap, hemData);

    return qCreatedBy(extrudeId, EntityType.BODY);
}

/**
 * Construct a bounding plane if a blind extrude is not sufficient.  Return a bounding map consumable by `extrudeWithBoundingMaps(...)`.
 */
function constructBoundingPlaneForOtherSheetIfNecessary(context is Context, id is Id, isStart is boolean, planeOrigin is Vector,
    boundingData is map, hemData is map, bodiesToDelete is box) returns map
{
    var boundingPlaneNormal = boundingData.minimalClearanceBoundingPlane.normal;

    // BEL-115669: Only execute BEL-115206 adjustment for non-neighbor-mitered hems.
    if (!boundingData.isMiteredWithNeighborHem)
    {
        // BEL-115206: Ensuring the bounding plane pulls inward here makes sure that hem sides do not push outwards. This
        // only affects the sides of the hem other sheet.  It does not affect hem corners.
        const endPlaneNormalDotOutFromFace = dot(boundingPlaneNormal, hemData.outFromFace);

        // The bounding plane normal always points in the same hemisphere as the edge direction (See getBoundingDataAtVertex(...)
        // documentation for more detail).  Consider hemData.outFromFace as 'up' (see getHemData(...) documentation for description
        // of this direction). If the start bounding plane normal points downward, or the end bounding plane points upward, then
        // that bounding plane allows the hem side to push outward.
        const hemPushesOut = (isStart == endPlaneNormalDotOutFromFace < 0);
        if (hemPushesOut)
        {
            // Equivalent of setting `boundingPlaneNormal = hemData.edgeDirection`, but since we know what result will
            // come out of `constructExtrudeBoundingPlaneIfNecessary(...)` by doing that, we may as well not waste the cycles.
            return makeBlindBoundingMap(isStart, planeOrigin, hemData);
        }
    }

    return constructExtrudeBoundingPlaneIfNecessary(context, id, isStart, plane(planeOrigin, boundingPlaneNormal), hemData, bodiesToDelete);
}

// -------------------------------------------------- Sheet annotation --------------------------------------------------

/**
 * Add the appropriate sheet metal attributes to the hem sheet. `coincidentEdge` is the edge at which the hem sheet will attach to the master
 * sheet body.
 */
function annotateHemSheets(context is Context, topLevelId is Id, arcSheet is Query, otherSheet is Query, attributeIdCounter is box,
    hemData is map, modelParameters is map, definition is map)
{
    // Face of arc sheet needs bend attribute
    var bendAttribute = makeSMJointAttribute(toAttributeId(topLevelId + attributeIdCounter[]));
    bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited" : false };
    bendAttribute.bendType = { "value" : SMBendType.HEM, "canBeEdited" : false };
    bendAttribute.radius = { "value" : getInnerRadius(definition, modelParameters), "canBeEdited" : false };
    bendAttribute.angle = { "value" : hemData.arcAngle, "canBeEdited" : false };
    setAttribute(context, {
                "entities" : qOwnedByBody(arcSheet, EntityType.FACE),
                "attribute" : bendAttribute
            });
    attributeIdCounter[] += 1;

    if (definition.hemType == SMHemType.STRAIGHT || definition.hemType == SMHemType.TEAR_DROP)
    {
        const otherFaces = evaluateQuery(context, qOwnedByBody(otherSheet, EntityType.FACE));
        if (size(otherFaces) != 1)
        {
            // Expected straight or tear drop hem to have one other face
            throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
        }
        setAttribute(context, {
                    "entities" : otherFaces[0],
                    "attribute" : makeSMWallAttribute(toAttributeId(topLevelId + attributeIdCounter[]))
                });
        attributeIdCounter[] += 1;
    }
    else if (definition.hemType == SMHemType.ROLLED)
    {
        if (!isQueryEmpty(context, qOwnedByBody(otherSheet, EntityType.FACE)))
        {
            // Did not expect rolled to have additional sheet
            throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
        }
    }
    else
    {
        throwHemTypeError(definition.hemType);
    }
}

// -------------------------------------------------- Match finding --------------------------------------------------

function createMatches(context is Context, edge is Query, arcSheet is Query, otherSheet is Query, hemData is map) returns array
{
    const arcSheetEdges = qOwnedByBody(arcSheet, EntityType.EDGE);
    const coincidentToEdge = evaluateQuery(context, qContainsPoint(arcSheetEdges, hemData.edgeCenterPosition));
    if (size(coincidentToEdge) != 1)
    {
        // Hem extrusion did not result in expected edge
        throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
    }

    var matches = [{
            "topology1" : edge,
            "topology2" : coincidentToEdge[0],
            "matchType" : TopologyMatchType.COINCIDENT
        }];

    if (isQueryEmpty(context, otherSheet))
    {
        // If there is no otherSheet, we are done
        return matches;
    }

    const otherSheetEdges = qOwnedByBody(otherSheet, EntityType.EDGE);
    const matchedBetweenArcAndOther = evaluateQuery(context, qContainsPoint(qUnion([arcSheetEdges, otherSheetEdges]), hemData.arcEndPosition));
    if (size(matchedBetweenArcAndOther) != 2)
    {
        // Hem extrusion did not result in expected edges
        throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
    }

    matches = append(matches, {
                "topology1" : matchedBetweenArcAndOther[0],
                "topology2" : matchedBetweenArcAndOther[1],
                "matchType" : TopologyMatchType.COINCIDENT
            });
    return matches;
}

// -------------------------------------------------- Utilities - extrudeWithBoundingMaps --------------------------------------------------

/**
 * Construct an extrude bounding plane if the result would be different than a blind extrude, otherwise skip the construction.
 * Return a bounding map consumable by `extrudeWithBoundingMaps(...)`.
 */
function constructExtrudeBoundingPlaneIfNecessary(context is Context, id is Id, isStart is boolean, plane is Plane, hemData is map, bodiesToDelete is box) returns map
{
    if (!parallelVectors(plane.normal, hemData.edgeDirection))
    {
        // Bounding plane makes a different bound than blind extrude would
        addBodiesToDelete(bodiesToDelete, qCreatedBy(id, EntityType.BODY));
        return makeUpToBoundingMap(isStart, BoundingType.UP_TO_SURFACE, constructPlane(context, id, plane));
    }
    else
    {
        // Bounding plane is equivalent to blind extrude
        return makeBlindBoundingMap(isStart, plane.origin, hemData);
    }
}

function getBoundField(isStart is boolean) returns string
{
    return isStart ? "startBound" : "endBound";
}

function makeBlindBoundingMap(isStart is boolean, upToPoint is Vector, hemData is map) returns map
{
    const depthField = isStart ? "startDepth" : "endDepth";
    return {
            getBoundField(isStart) : BoundingType.BLIND,
            (depthField) : dot(upToPoint - hemData.edgeCenterPosition, (isStart ? -1 : 1) * hemData.edgeDirection)
        };
}

function makeUpToBoundingMap(isStart is boolean, boundingType is BoundingType, entity is Query)
{
    const entityField = isStart ? "startBoundEntity" : "endBoundEntity";
    return {
            getBoundField(isStart) : boundingType,
            (entityField) : entity
        };
}

/**
 * Utility for extruding with bounding maps constucted by `makeBlindBoundingMap(...)` or `makeUpToBoundingMap(...)`
 */
function extrudeWithBoundingMaps(context is Context, id is Id, entities is Query, startBoundingMap is map, endBoundingMap is map, hemData is map, overrides is map)
{
    var definition = {
        "entities" : entities,
        "direction" : hemData.edgeDirection
    };
    definition = mergeMaps(definition, startBoundingMap);
    definition = mergeMaps(definition, endBoundingMap);
    definition = mergeMaps(definition, overrides);

    try
    {
        opExtrude(context, id, definition);
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
    }
}

function extrudeWithBoundingMaps(context is Context, id is Id, entities is Query, startBoundingMap is map, endBoundingMap is map, hemData is map)
{
    extrudeWithBoundingMaps(context, id, entities, startBoundingMap, endBoundingMap, hemData, {});
}

// -------------------------------------------------- Utilities - General --------------------------------------------------

function constructPlane(context is Context, id is Id, plane is Plane) returns Query
{
    opPlane(context, id, { "plane" : plane });
    return qCreatedBy(id, EntityType.FACE);
}

/**
 * Add additional bodies to delete into the array of queries held by bodiesToDeleteBox
 */
function addBodiesToDelete(bodiesToDeleteBox is box, bodiesToDeleteQuery is Query)
{
    bodiesToDeleteBox[] = append(bodiesToDeleteBox[], bodiesToDeleteQuery);
}

function qAssociatedInSolidFolded(context is Context, entity is Query) returns Query
{
    const associationAttribute = getSMAssociationAttributes(context, entity);
    if (size(associationAttribute) != 1)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_HEM_FAILED);
    }
    return qSheetMetalFlatFilter(qBodyType(qAttributeQuery(associationAttribute[0]), BodyType.SOLID), SMFlatType.NO);
}

function getWrapAroundFront(definition is map) returns boolean
{
    return definition.oppositeDirection;
}

// get thickness that should be added to hemData.arcRadius to find the outer radius
function getOuterThickness(definition is map, modelParameters is map) returns ValueWithUnits
{
    return getWrapAroundFront(definition) ? modelParameters.backThickness : modelParameters.frontThickness;
}

// get thickness that should be subtracted from hemData.arcRadius to find the inner radius
function getInnerThickness(definition is map, modelParameters is map) returns ValueWithUnits
{
    return getWrapAroundFront(definition) ? modelParameters.frontThickness : modelParameters.backThickness;
}

function getInnerRadius(definition is map, modelParameters is map) returns ValueWithUnits
{
    if (definition.hemType == SMHemType.ROLLED || definition.hemType == SMHemType.TEAR_DROP)
    {
        return definition.innerRadius;
    }
    else if (definition.hemType == SMHemType.STRAIGHT)
    {
        return definition.useMinimalGapForStraight ? (modelParameters.minimalClearance / 2) : definition.innerRadius;
    }
    else
    {
        // This cannot be hit by a UI user.
        throwHemTypeError(definition.hemType);
    }
}

/**
 * Intersect a plane and a line.  Error If they are parallel.
 */
function findIntersectionPoint(plane is Plane, line is Line) returns Vector
{
    const intersectionResult = intersection(plane, line);
    if (intersectionResult.dim != 0)
    {
        throw "Expected plane and line to intersect at a point";
    }
    return intersectionResult.intersection;
}

function getTotalArcLength(hemData is map) returns ValueWithUnits
{
    // Arc length = (arcAngle / (2 * pi)) * (2 * pi * r)
    //            = arcAngle * r
    return (hemData.arcAngle / radian) * hemData.arcRadius;
}

function throwHemTypeError(hemType is SMHemType)
{
    throw "Unrecognized hem type: " ~ hemType;
}

// -------------------------------------------------- Editing Logic --------------------------------------------------

/**
 * @internal
 * Editing logic for the hem feature.
 */
export function hemEditLogic(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean, specifiedParameters is map) returns map
{
    // Lock tipGap to match innerRadius until the user first opens that part of the dialog.  That way the first time they open it, it is a valid number.
    // Because we use `REMEMBER_PREVIOUS_VALUE` on most of the parameters, the dialog could start with `useMinimalGapForTipGap` as true or false,
    // so only make this sort of edit while `tipGap` is hidden, and if neither the checkbox or the value have been touched.
    const tipGapValueShowing = definition.hemType == SMHemType.TEAR_DROP && !definition.useMinimalGapForTipGap;
    const tipGapCheckboxOrValueTouched = specifiedParameters.useMinimalGapForTipGap || specifiedParameters.tipGap;
    if (isCreating && oldDefinition.innerRadius != definition.innerRadius && !tipGapValueShowing && !tipGapCheckboxOrValueTouched)
    {
        definition.tipGap = definition.innerRadius;
    }

    return definition;
}

