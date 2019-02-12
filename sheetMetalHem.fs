FeatureScript 1010; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "1010.0");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "1010.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "1010.0");
import(path : "onshape/std/containers.fs", version : "1010.0");
import(path : "onshape/std/context.fs", version : "1010.0");
import(path : "onshape/std/coordSystem.fs", version : "1010.0");
import(path : "onshape/std/curveGeometry.fs", version : "1010.0");
import(path : "onshape/std/error.fs", version : "1010.0");
import(path : "onshape/std/evaluate.fs", version : "1010.0");
import(path : "onshape/std/feature.fs", version : "1010.0");
import(path : "onshape/std/math.fs", version : "1010.0");
import(path : "onshape/std/query.fs", version : "1010.0");
import(path : "onshape/std/rollSurface.fs", version : "1010.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "1010.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "1010.0");
import(path : "onshape/std/sketch.fs", version : "1010.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1010.0");
import(path : "onshape/std/topologyUtils.fs", version : "1010.0");
import(path : "onshape/std/uihint.gen.fs", version : "1010.0");
import(path : "onshape/std/valueBounds.fs", version : "1010.0");
import(path : "onshape/std/vector.fs", version : "1010.0");

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
 * A `LengthBoundSpec` for hem inner radius. Defined as NONNEGATIVE_LENGTH_BOUNDS / 10.
 */
export const SM_HEM_INNER_RADIUS_BOUNDS =
{
    (meter)      : [1e-5, 0.0025, 500],
    (centimeter) : 0.25,
    (millimeter) : 2.5,
    (inch)       : 0.1,
    (foot)       : 0.01,
    (yard)       : 0.0025
} as LengthBoundSpec;

/**
 * @internal
 * A `LengthBoundSpec` for hem gap. Defined as SM_HEM_INNER_RADIUS_BOUNDS * 2.
 */
export const SM_HEM_GAP_BOUNDS =
{
    (meter)      : [1e-5, 0.005, 500],
    (centimeter) : 0.5,
    (millimeter) : 5.0,
    (inch)       : 0.2,
    (foot)       : 0.02,
    (yard)       : 0.005
} as LengthBoundSpec;

/**
 * @internal
 * Create sheet metal hems on selected edges of sheet metal parts.
 */
annotation { "Feature Type Name" : "Hem", "Editing Logic Function" : "hemEditLogic" }
export const sheetMetalHem = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Edges or side faces to hem",
                     "Filter" : (SheetMetalDefinitionEntityType.EDGE && (GeometryType.LINE || GeometryType.PLANE))
                        && AllowFlattenedGeometry.YES && ModifiableEntityOnly.YES }
        definition.edges is Query;

        annotation { "Name" : "Hem type" }
        definition.hemType is SMHemType;

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;

        if (definition.hemType == SMHemType.STRAIGHT)
        {
            annotation { "Name" : "Flattened", "Default" : true, "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            definition.useMinimalGapForStraight is boolean;
        }

        if (definition.hemType == SMHemType.ROLLED || definition.hemType == SMHemType.TEAR_DROP
            || (definition.hemType == SMHemType.STRAIGHT && !definition.useMinimalGapForStraight))
        {
            annotation { "Name" : "Inner radius" }
            isLength(definition.innerRadius, SM_HEM_INNER_RADIUS_BOUNDS);
        }

        if (definition.hemType == SMHemType.TEAR_DROP)
        {
            annotation { "Name" : "Use minimal gap", "Default" : true, "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            definition.useMinimalGapForTipGap is boolean;

            if (!definition.useMinimalGapForTipGap)
            {
                annotation { "Name" : "Gap" }
                isLength(definition.tipGap, SM_HEM_INNER_RADIUS_BOUNDS);
            }
        }

        if (definition.hemType == SMHemType.ROLLED)
        {
            annotation { "Name" : "Angle" }
            isAngle(definition.angle, SM_HEM_ANGLE_BOUNDS);
        }

        if (definition.hemType == SMHemType.TEAR_DROP || definition.hemType == SMHemType.STRAIGHT)
        {
            annotation { "Name" : "Total length" }
            isLength(definition.length, NONNEGATIVE_LENGTH_BOUNDS);
        }

        annotation { "Name" : "Hem alignment", "UIHint" : [ UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE ] }
        definition.hemAlignment is SMHemAlignment;

        annotation { "Name" : "Corner type", "UIHint" : [ UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE ] }
        definition.hemCornerType is SMHemCornerType;
    }
    {
        // this is not necessary but helps with correct error reporting in feature pattern
        checkNotInFeaturePattern(context, definition.edges, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        if (size(evaluateQuery(context, definition.edges)) == 0)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_HEM_NO_EDGES, ["edges"]);
        }

        var edges = qUnion(getSMDefinitionEntities(context, definition.edges));
        var nonLineEdgeQ = qSubtraction(qEntityFilter(edges, EntityType.EDGE), qGeometry(edges, GeometryType.LINE));
        if (size(evaluateQuery(context, nonLineEdgeQ)) != 0)
        {
            setErrorEntities(context, id, { "entities" : nonLineEdgeQ });
            edges = qGeometry(edges, GeometryType.LINE);
            if (size(evaluateQuery(context, edges)) != 0)
            {
                reportFeatureWarning(context, id, ErrorStringEnum.SHEET_METAL_HEM_NON_LINEAR_EDGES);
            }
            else
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_HEM_NON_LINEAR_EDGES, ["edges"]);
            }
        }
        var evaluatedEdgeQuery = evaluateQuery(context, edges);
        if (size(evaluatedEdgeQuery) == 0)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_EDGE_NEEDED, ["edges"]);
        }

        // Get originals before any changes
        var smBodies = evaluateQuery(context, qOwnerBody(edges));
        var smBodiesQ = qUnion(smBodies);
        const initialData = getInitialEntitiesAndAttributes(context, smBodiesQ);
        const robustSMBodiesQ = qUnion([smBodiesQ, startTracking(context, smBodiesQ)]);

        const bodyToEdges = groupEntitiesByBody(context, qUnion(evaluatedEdgeQuery));

        var bodyIndex = 0;
        var bodiesToDelete = new box([]);
        var attributeIdCounter = new box(0);
        for (var entry in bodyToEdges)
        {
            addHemsToSheetBody(context, id, qUnion(entry.value), entry.key, bodyIndex, bodiesToDelete, attributeIdCounter, definition);
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
    }, {});

/**
 * Build and attach hems for each of the specified edges.  Edges should all belong to the same underlying sheet body.
 */
function addHemsToSheetBody(context is Context, topLevelId is Id, edges is Query, body is Query, bodyIndex is number,
    bodiesToDelete is box, attributeIdCounter is box, definition is map)
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
    }

    edgeIndex = 0;
    var matches = [];
    var bodiesToBoolean = [];
    const sharedVertexToMiteredBoundingData = new box({});
    for (var edge in evaluateQuery(context, edges))
    {
        // TODO: Investiage whether this disambiguation is useful, and successfully disambiguates the planes
        var edgeId = bodyId + unstableIdComponent("edge" ~ edgeIndex);
        edgeIndex += 1;
        setExternalDisambiguation(context, edgeId, edge);

        const hemData = edgeToHemData[edge];

        const helperToolsId = edgeId + "helperTools";
        const helperTools = constructHemSheetHelperTools(context, helperToolsId, edge, edgeToHemData, definition,
                modelParameters, sharedVertexToMiteredBoundingData);

        const hemSheetId = edgeId + "hemSheet";
        const sheetMap = constructHemSheets(context, hemSheetId, helperTools, hemData, definition);

        annotateHemSheets(context, topLevelId, sheetMap.arcSheet, sheetMap.otherSheet, attributeIdCounter, hemData, modelParameters, definition);

        matches = concatenateArrays([matches, createMatches(context, edge, sheetMap.arcSheet, sheetMap.otherSheet, hemData)]);

        bodiesToBoolean = append(bodiesToBoolean, qCreatedBy(hemSheetId, EntityType.BODY));
        bodiesToDelete[] = append(bodiesToDelete[], qUnion([qCreatedBy(hemData.profileSketchId, EntityType.BODY), qCreatedBy(helperToolsId, EntityType.BODY)]));
    }

    const booleanId = bodyId + "boolean";
    try
    {
        opBoolean(context, booleanId, {
                    "allowSheets" : true,
                    "tools" : qUnion([robustBodyQuery, qUnion(bodiesToBoolean)]),
                    "operationType" : BooleanOperationType.UNION,
                    "matches" : matches
                });
    }
    catch (error)
    {
        processSubfeatureStatus(context, topLevelId, {
                    "subfeatureId" : booleanId,
                    "propagateErrorDisplay" : true
                });
        throw error;
    }
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
 * }}
 */
function getHemData(context is Context, edge is Query) returns map
{
    const faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
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

    const faceTangentPlaneAtCenter = evFaceTangentPlaneAtEdge(context, {
                "edge" : edge,
                "face" : face,
                "parameter" : 0.5,
                "usingFaceOrientation" : true
            });
    const outFromFace = cross(edgeTangentLineAtCenter.direction, faceTangentPlaneAtCenter.normal);

    const bothVertices = qVertexAdjacent(edge, EntityType.VERTEX);
    const startResult = evaluateQuery(context, qClosestTo(bothVertices, edgeTangentLineAtStart.origin));
    if (size(startResult) != 1)
        throw "Unexpected number of vertices";
    const startVertex = startResult[0];
    const endResult = evaluateQuery(context, qClosestTo(bothVertices, edgeTangentLineAtEnd.origin));
    if (size(endResult) != 1)
        throw "Unexpected number of vertices";
    const endVertex = endResult[0];

    return {
            "edgeDirection" : edgeTangentLineAtCenter.direction,
            "edgeStartPosition" : edgeTangentLineAtStart.origin,
            "edgeCenterPosition" : edgeTangentLineAtCenter.origin,
            "edgeEndPosition" : edgeTangentLineAtEnd.origin,
            "face" : face,
            "faceNormal" : faceTangentPlaneAtCenter.normal,
            "outFromFace" : outFromFace,
            "startVertex" : startVertex,
            "endVertex" : endVertex
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
    const innerRadius = getInnerRadius(modelParameters, definition);
    const hemExtent = innerRadius + modelParameters.backThickness + modelParameters.frontThickness;

    var edgeChangeOptions = [];
    for (var edge in evaluateQuery(context, hemEdges))
    {
        edgeChangeOptions = append(edgeChangeOptions, {
                    "edge" : edge,
                    "face" : qEdgeAdjacent(edge, EntityType.FACE),
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
 *     @field arcEndLine {Vector} : a line whose origin is the position of the end of the initial arc, and whose direction is the tangent line at the end of the initial arc.
 * }}
 */
function sketchHemProfile(context is Context, id is Id, hemData is map, modelParameters is map, definition is map) returns map
{
    // Construct the sketch such that the origin is at the center of the edge, +X is tangent to the face and pointing away from the face,
    // and +Y is the direction of back thickness of the sheet (a.k.a. the anti-normal of the face).
    const sketchPlane = plane(hemData.edgeCenterPosition, hemData.edgeDirection, hemData.outFromFace);
    const sketch = newSketchOnPlane(context, id, {
                "sketchPlane" : sketchPlane
            });
    // Opposite direction should wrap around the front of the sheet.  Non-opposite direction should wrap around the back of the sheet.
    const wrapAroundFront = getWrapAroundFront(definition);

    const innerRadius = getInnerRadius(modelParameters, definition);

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
    var arcEndLine = evEdgeTangentLine(context, {
            "edge" : arcEdge,
            "parameter" : arcInfo.arcEndParameter
        });
    if (arcInfo.arcEndParameter == 0)
    {
        // Flip the arc end line direction if it is facing into the arc rather than out of the arc
        arcEndLine.direction *= -1;
    }

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
            "arcEndLine" : arcEndLine
        };
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

// -------------------------------------------------- Hem sheet tools --------------------------------------------------

/**
 * Construct bounding tools for the extremes of the hem. `edgeToHemData` should be in the format outputted by
 * `getEdgeToHemData(...)`, with the addition of augmentation by `augmentHemDataWithSketchInformation(...)`
 * @returns {{
 *      @field startBoundingPlane {Query} : a face of a construction plane to bound the hem at the start vertex
 *      @field endBoundingPlane {Query} : a face of a construction plane to bound the hem at the end vertex
 *      @field flatArcSketch {Query} : see [constructFlatArcSketchAndRollParameters] documentation
 *      @field rollParameters {map} : see [constructFlatArcSketchAndRollParameters] documentation
 * }}
 */
function constructHemSheetHelperTools(context is Context, id is Id, edge is Query, edgeToHemData is map,
    definition is map, modelParameters is map, sharedVertexToMiteredBoundingData is box) returns map
{
    const startBoundingData = getBoundingDataAtVertex(context, edge, true, edgeToHemData, definition, modelParameters, sharedVertexToMiteredBoundingData);
    const endBoundingData = getBoundingDataAtVertex(context, edge, false, edgeToHemData, definition, modelParameters, sharedVertexToMiteredBoundingData);

    const preparedFlatArcSketch = prepareFlatArcSketch(context, id + "flatArcSketch", edgeToHemData[edge],
            startBoundingData, endBoundingData, definition, modelParameters);

    // Optimization: for ROLLED hems, we do not need a bounding plane.
    var startBoundingPlane = qNothing();
    var endBoundingPlane = qNothing();
    if (definition.hemType != SMHemType.ROLLED)
    {
        startBoundingPlane = constructHemBoundingPlane(context, id + "startBoundingPlane", preparedFlatArcSketch.startFinalPoint,
                startBoundingData, edgeToHemData[edge]);
        endBoundingPlane = constructHemBoundingPlane(context, id + "endBoundingPlane", preparedFlatArcSketch.endFinalPoint,
                endBoundingData, edgeToHemData[edge]);
    }

    return {
            "startBoundingPlane" : startBoundingPlane,
            "endBoundingPlane" : endBoundingPlane,
            "flatArcSketch" : preparedFlatArcSketch.flatArcSketch,
            "rollParameters" : preparedFlatArcSketch.rollParameters
        };
}

/**
 * Construct a bounding plane for the given `boundingData`. Return a [Query] for the face of the plane.
 */
function constructHemBoundingPlane(context is Context, id is Id, relevantFinalPoint is Vector, boundingData is map, hemData is map) returns Query
{
    const acrossArcEndLine = line(hemData.arcEndLine.origin, hemData.edgeDirection);
    const finalPointRolledPosition = project(acrossArcEndLine, relevantFinalPoint);
    const hemSheetBoundingPlane = plane(finalPointRolledPosition, boundingData.minimalClearanceBoundingPlane.normal);

    const planeId = id + "plane";
    opPlane(context, planeId, {
                "plane" : hemSheetBoundingPlane
            });
    return qCreatedBy(planeId, EntityType.FACE);
}

// -------------------------------------------------- Bounding data calculation --------------------------------------------------

/**
 * Find the definition of the bounding plane for the given `edge`, at the given vertex (as defined by `isStart`)
 * @returns {{
 *      @field basePoint {Vector} : the base point at which the arc piece of the hem should start for the given vertex
 *      @field minimalClearanceBoundingPlane {Plane} : The boundary of the the thickened hem, including the necessary minimal clearance
 * }}
 */
function getBoundingDataAtVertex(context is Context, edge is Query, isStart is boolean, edgeToHemData is map, definition is map,
    modelParameters is map, sharedVertexToMiteredBoundingData is box) returns map
{
    const hemData = edgeToHemData[edge];
    const vertexInQuestion = isStart ? hemData.startVertex : hemData.endVertex;
    const vertexInQuestionPosition = isStart ? hemData.edgeStartPosition : hemData.edgeEndPosition;
    const tangentLineInQuestion = line(isStart ? hemData.edgeStartPosition : hemData.edgeEndPosition, hemData.edgeDirection);

    // ----- Find the base point of the hem arc -----
    // TODO: just put this on the hem data
    const associationAttribute = getAttributes(context, {
                    "entities" : edge,
                    "attributePattern" : {} as SMAssociationAttribute
                })[0];
    const baseVertex = qClosestTo(qVertexAdjacent(qSMFlatFilter(qBodyType(qAttributeQuery(associationAttribute), BodyType.SOLID), SMFlatType.NO), EntityType.VERTEX), vertexInQuestionPosition);
    var basePoint = project(line(hemData.edgeCenterPosition, hemData.edgeDirection), evVertexPoint(context, { "vertex" : baseVertex }));
    const basePointParameterPastVertex = dot((basePoint - vertexInQuestionPosition), hemData.edgeDirection);
    if ((isStart && basePointParameterPastVertex < 0) || (!isStart && basePointParameterPastVertex > 0))
    {
        // If the base point based on selection extends further than the base point based on underlying definition, than the edge
        // has been affected by alignment push-back, and the base point based on underlying definition should be used.
        basePoint = vertexInQuestionPosition;
    }

    // ----- Check if we already have the bounding plane for this vertex -----
    if (sharedVertexToMiteredBoundingData[][vertexInQuestion] != undefined)
    {
        // We have already constructed a bounding plane for this vertex when processing its neighbor.
        return miteredBoundingDataToBoundingData(isStart, sharedVertexToMiteredBoundingData[][vertexInQuestion], basePoint);
    }

    // ----- Next, find the end plane assuming we do not have a neighboring hem -----
    const dataByGeometry = getBoundingDataBySurroundingGeometry(context, isStart, basePoint, vertexInQuestion, tangentLineInQuestion, hemData, definition, modelParameters);
    const initialBoundingData = {
            "basePoint" : basePoint,
            "minimalClearanceBoundingPlane" : dataByGeometry.minimalClearanceBoundingPlane
        };

    if (!dataByGeometry.checkForMiter)
    {
        return initialBoundingData;
    }

    // ----- Next, check if there is a neighboring hem to miter with -----
    const otherHemEdges = qSubtraction(qUnion(keys(edgeToHemData)), edge);
    const edgesOfVertexInQuestion = qVertexAdjacent(vertexInQuestion, EntityType.EDGE);
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
function getBoundingDataBySurroundingGeometry(context is Context, isStart is boolean, basePoint is Vector, vertexInQuestion is Query,
    tangentLineInQuestion is Line, hemData is map, definition is map, modelParameters is map)
{
    // The canonical end plane is just the plane at the end of the edge
    const endPlane = plane(basePoint, tangentLineInQuestion.direction);

    // -- Check if there are any neighboring joints --
    const neighboringJoint = qEdgeTopologyFilter(qVertexAdjacent(vertexInQuestion, EntityType.EDGE), EdgeTopology.TWO_SIDED);
    if (evaluateQuery(context, neighboringJoint) == [])
    {
        // The hem does not have any neighboring joints.  Use the canonical end plane, but check for neighboring hems.
        return {
                "minimalClearanceBoundingPlane" : endPlane,
                "checkForMiter" : true
            };
    }

    // -- Check if the neighboring face (over the joint) infringes on the space for this hem --
    const neighborFace = qSubtraction(qEdgeAdjacent(neighboringJoint, EntityType.FACE), hemData.face);
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
                "minimalClearanceBoundingPlane" : plane(basePoint, boundingPlaneDirection),
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
            "minimalClearanceBoundingPlane" : minimalClearanceBoundingPlane
        };
}

// -------------------------------------------------- Flat arc sketch --------------------------------------------------

/**
 * Prepare a flat sketch which can be rolled onto a cylinder to become the hem arc. For SIMPLE corners, formulate this flat sketch such that
 * the side edges are linear, so that the side edges of the hem arc sheet are helices.  For CLOSED corners, formulate this flat sketch such
 * that the side edges are sinusoidal, so that the edge edges of the hem arc close up with their neighbors.
 * @returns {{
 *      @field flatArcSketch {Query} : the sketch region that will be rolled to form the hem arc sheet
 *      @field rollParameters {map} : {{
 *                                        @field rollPlane {RollSurface} : the `source` for the `opRoll` of `flatArcSketch`
 *                                        @field rollCylinder {RollSurface} : the `destination` for the `opRoll` of `flatArcSketch`
 *                                    }}
 *      @field startFinalPoint {Vector} : the location of the final sketch point on the start side of the flat arc sketch, in world coordinates
 *      @field endFinalPoint {Vector} : the locatoion of the final sketch point on the end side of the flat arc sketch, in world coordinates
 * }}
 */
function prepareFlatArcSketch(context is Context, id is Id, hemData is map,
    startBoundingData is map, endBoundingData is map, definition is map, modelParameters is map) returns map
{
    // Sketch on a plane whose normal is the normal of the attached sheet face, whose x direction is pointing outward from
    // the face, and whose y direction is, therefore, the coedge direction of the hem edge.
    const sheetFaceNormal = cross(hemData.outFromFace, hemData.edgeDirection);
    const sketchPlane = plane(hemData.edgeCenterPosition, sheetFaceNormal, hemData.outFromFace);

    const startDefinedSketch = defineSketchForSide(context, id + "startDefineSketch", true, hemData, startBoundingData, definition, modelParameters);
    const endDefinedSketch = defineSketchForSide(context, id + "endDefineSketch", false, hemData, endBoundingData, definition, modelParameters);

    const flatArcSketchId = id + "flatArcSketch";
    const flatArcSketch = newSketchOnPlane(context, flatArcSketchId, { "sketchPlane" : sketchPlane });

    const sketchMapper = function(worldPoint)
        {
            return worldToPlane(sketchPlane, worldPoint);
        };

    // sketch the flattened corners
    const startFinalPoint = sketchDefinedSketch(flatArcSketch, "start", startDefinedSketch, sketchMapper);
    const endFinalPoint = sketchDefinedSketch(flatArcSketch, "end", endDefinedSketch, sketchMapper);

    // sketch the far and near line to close the sketch
    skLineSegment(flatArcSketch, "startToEndBase", {
                "start" : worldToPlane(sketchPlane, startBoundingData.basePoint),
                "end" : worldToPlane(sketchPlane, endBoundingData.basePoint)
            });
    skLineSegment(flatArcSketch, "startToEnd", {
                "start" : startFinalPoint,
                "end" : endFinalPoint
            });

    skSolve(flatArcSketch);

    const planeForRoll = plane(sketchPlane.origin, (getWrapAroundFront(definition) ? -1 : 1) * sketchPlane.normal);
    const rollPlane = makeRollPlane(planeForRoll, hemData.edgeCenterPosition, hemData.outFromFace);
    const rollCylinder = makeRollCylinder(cylinder(coordSystem(plane(hemData.arcCenter, yAxis(sketchPlane))), hemData.arcRadius), hemData.edgeCenterPosition, hemData.outFromFace);

    return {
            "flatArcSketch" : qSketchRegion(flatArcSketchId),
            "rollParameters" : {
                    "rollPlane" : rollPlane,
                    "rollCylinder" : rollCylinder
                },
            "startFinalPoint" : planeToWorld(sketchPlane, startFinalPoint),
            "endFinalPoint" : planeToWorld(sketchPlane, endFinalPoint)
        };
}

/**
 * Sketch out a `definedSketch` as returned from `defineSketchForSide`.  Return its final point (which should lie on the end line
 * of the flattened arc).
 */
function sketchDefinedSketch(flatArcSketch is Sketch, id is string, definedSketch is map, sketchMapper is function) returns Vector
{
    // Written in such a way that we can start with a linearPortion and then extend beyond that with splinePortion, and return
    // the correct finalPoint.
    var finalPoint;
    if (definedSketch.linearPortion != undefined)
    {
        finalPoint = sketchLinearPortion(flatArcSketch, id, definedSketch.linearPortion, sketchMapper);
    }
    if (definedSketch.splinePortion != undefined)
    {
        finalPoint = sketchSplinePortion(flatArcSketch, id, definedSketch.splinePortion, sketchMapper);
    }
    return finalPoint;
}

/**
 * Sketch the linear portion and return its final point
 */
function sketchLinearPortion(flatArcSketch is Sketch, id is string, linearPortion is array, sketchMapper is function) returns Vector
{
    if (size(linearPortion) != 2)
    {
        throw "Incorrectly sized linear portion";
    }
    skLineSegment(flatArcSketch, id ~ "LinearPortion", {
                "start" : sketchMapper(linearPortion[0]),
                "end" : sketchMapper(linearPortion[1])
            });

    return sketchMapper(linearPortion[1]);
}

/**
 * Sketch the spline portion and return its final point
 */
function sketchSplinePortion(flatArcSketch is Sketch, id is string, splinePortion is array, sketchMapper is function) returns Vector
{
    var finalPoint;
    for (var i = 0; i < size(splinePortion); i += 1)
    {
        const splineSubportionInSketchParameters = mapArray(splinePortion[i], sketchMapper);
        finalPoint = splineSubportionInSketchParameters[size(splineSubportionInSketchParameters) - 1];
        skFitSpline(flatArcSketch, id ~ "SplinePortion" ~ i, { "points" : splineSubportionInSketchParameters });
    }
    return finalPoint;
}

/**
 * Return a map containing a `linearPortion` which should be sketched first, and then a `splinePortion` which should be sketched after.
 * See `prepareFlatArcSketch(...)` documentation for description of intent of different corner types.
 */
function defineSketchForSide(context is Context, id is Id, isStart is boolean, hemData is map, boundingData is map, definition is map, modelParameters is map)
{
    const intersectionPlane = boundingData.minimalClearanceBoundingPlane;

    // Devise a new x-axis where the origin is the corner in question, and the direction is the co-edge direction of the edge.
    // The y-axis will be away from the face (on the plane of the face), and the z direction is, therefore, the anti-normal of the face.
    const xAxis = hemData.edgeDirection;
    const yAxis = hemData.outFromFace;
    const zAxis = cross(xAxis, yAxis);

    // If the intersection plane caps the hem with no lean, than the flattened hem corner can just be a straight line
    const noLean = parallelVectors(intersectionPlane.normal, xAxis);
    if (noLean)
    {
        return { "linearPortion" : [boundingData.basePoint, boundingData.basePoint + (hemData.outFromFace * getTotalArcLength(hemData))] };
    }

    const basePointCSys = coordSystem(boundingData.basePoint, xAxis, zAxis);
    const outerSinusoidData = constructUnrolledSinusoidData(basePointCSys, intersectionPlane, false, isStart, hemData, definition, modelParameters);

    var linearPortion;
    var splinePortion;
    if (definition.hemCornerType == SMHemCornerType.SIMPLE)
    {
        var angleToUse;
        if (hemData.arcAngle < (outerSinusoidData.closestApproachRange[0] + (TOLERANCE.zeroAngle * radian)))
        {
            // Arc angle has no chance of being larger than the closest approach, do not bother calculating the closest approach
            angleToUse = hemData.arcAngle;
        }
        else
        {
            const closestApproachAngle = findClosestApproachAngle(outerSinusoidData, isStart);
            if (hemData.arcAngle < (closestApproachAngle + (TOLERANCE.zeroAngle * radian)))
            {
                angleToUse = hemData.arcAngle;
            }
            else
            {
                angleToUse = closestApproachAngle;
            }
        }

        var point = outerSinusoidData.getPoint(angleToUse);
        point[1] = (angleToUse / radian) * hemData.arcRadius;

        if (!tolerantEquals(angleToUse, hemData.arcAngle))
        {
            // Extend the point to the full length of the flattened hem arc if the closest approach occurs earlier than the full arc angle
            point *= (hemData.arcAngle / angleToUse);
        }

        linearPortion = [vector(0, 0) * meter, point];
    }
    else if (definition.hemCornerType == SMHemCornerType.CLOSED)
    {
        const innerSinusoidData = constructUnrolledSinusoidData(basePointCSys, intersectionPlane, true, isStart, hemData, definition, modelParameters);
        splinePortion = constructSplinePortion(0 * radian, hemData.arcAngle, innerSinusoidData, outerSinusoidData, isStart, hemData);
    }
    else
    {
        throw "Unrecognized corner type " ~ definition.hemCornerType;
    }

    return {
            "linearPortion" : portionToWorld(linearPortion, boundingData.basePoint, xAxis, yAxis),
            "splinePortion" : portionToWorld(splinePortion, boundingData.basePoint, xAxis, yAxis)
        };
}

/**
 * Transform a sketch portion constructed in defineSketchForSide into world coordinates.
 */
function portionToWorld(portion, basePoint is Vector, xAxis is Vector, yAxis is Vector)
{
    if (portion == undefined)
    {
        return undefined;
    }

    // Type checking is tricky here because a vector is backed by an array
    const isVector = is2dPoint(portion[0]);
    if (portion[0] is array && !isVector)
    {
        // splinePortion is an array of arrays.  Recur here on the internal arrays.
        portion = mapArray(portion, function(subPortion)
            {
                return portionToWorld(subPortion, basePoint, xAxis, yAxis);
            });
    }
    else if (isVector)
    {
        portion = mapArray(portion, function(point)
            {
                return basePoint + (xAxis * point[0]) + (yAxis * point[1]);
            });
    }
    else
    {
        throw "Unexpected type of sketch portion " ~ portion[0];
    }

    return portion;
}

/**
 * The intersection of the bounding plane and the hem cylinder is an ellipse.  If this ellipse is unrolled away from the face attached
 * to the hem edge, it forms a sinusoid on the hem face plane. Return a set of functions and information that can be used to
 * calculate properties of the sinusoid.  The `cSys` passed into the function is described in `defineSketchForSide(...)`,
 * and the return values of the lambdas returned by this function will be framed in that coordinate system.
 * @returns {{
 *      @field getX {function} :     Given a desired angle, return the x value of the sinusoid at that angle
 *      @field getPoint {function} : Given a desired angle, return the (x, y) value of the sinusoid at that angle
 *      @field dXdY {function} :     Given a desired angle, return the (dX/dY) derivative of the sinusoid at that angle
 *      @field closestApproachRange {array} : the lower and upper bound for `findClosestApproachAngle(...)` for this specific sinusoid
 * }}
 */
function constructUnrolledSinusoidData(cSys is CoordSystem, intersectionPlane is Plane, isInner is boolean, isStart is boolean, hemData is map, definition is map, modelParameters is map) returns map
{
    const thicknessSign = isInner ? -1 : 1;
    const relevantThickness = isInner ? getInnerThickness(definition, modelParameters) : getOuterThickness(definition, modelParameters);
    const relevantRadius = hemData.arcRadius + (thicknessSign * relevantThickness);

    // zAxis is face-antiAligned, so if we are looking at inner thickness and wrapping around the front OR outer thickness and wrapping around the back,
    // we push in -Z direction (along face normal) to get the origin. In the other two cases, push in +Z direction (against face normal).
    const originPushSign = isInner == getWrapAroundFront(definition) ? -1 : 1;
    cSys.origin += cSys.zAxis * originPushSign * relevantThickness;

    // Intersect the bounding plane with the new x-axis to find where it intersects the new x-axis.
    const boundingPlaneIntersection = findIntersectionPoint(intersectionPlane, line(cSys.origin, cSys.xAxis));
    const boundingPlaneXOffset = dot(boundingPlaneIntersection - cSys.origin, cSys.xAxis);

    // The intersection of the the hem cylinder and the bounding plane will be an ellipse. Find the length of the projection of the major axis of
    // this ellipse onto the x-axis.  This is the amplitude of the sinusoid formed by unrolling the ellispe.
    // norm(cross(a1, a2)) / dot(a1, a2) = sin(angle between a1, a2) / cos(angle between a1, a2) = tan(angle between a1, a2)
    const xCrossPlaneNormal = cross(cSys.xAxis, intersectionPlane.normal);
    const xAxisProjectionLength = relevantRadius * (norm(xCrossPlaneNormal) / dot(cSys.xAxis, intersectionPlane.normal));

    // The sinusoid should go outwards first, and then pull back inwards.  The xAxis is always oriented from start to end, so for start, we need to reverse the sinusoid
    const sinusoidSign = isStart ? -1 : 1;

    const twoPi = 2 * PI;
    const yTotalLength = twoPi * relevantRadius;

    // True if the two neighboring hems are on a plate, and the bounding plane normal leans away from the X axis towards the + or - Y axis.
    const leansInY = parallelVectors(xCrossPlaneNormal, cSys.zAxis);
    // True if the two neighboring hems are connected through a joint, and the bounding plane normal leans away from the X axis towards the + or - Z axis.
    const leansInZ = parallelVectors(xCrossPlaneNormal, yAxis(cSys));

    // Optimization: precalculate these coefficients for the lambdas and capture them
    const amplitude = xAxisProjectionLength * sinusoidSign;
    const yScale = yTotalLength / (twoPi * radian);
    const dAngledY = twoPi / yTotalLength;

    if (leansInY)
    {
        const getX = function(angle)
            {
                return (amplitude * sin(angle)) + boundingPlaneXOffset;
            };

        return {
                "getX" : getX,
                "getPoint" : function(angle)
                    {
                        return vector(getX(angle), angle * yScale);
                    },
                "dXdY" : function(angle)
                    {
                        return (amplitude * cos(angle)) * dAngledY;
                    },
                "closestApproachRange" : [180 * degree, 270 * degree]
            };
    }
    else if (leansInZ)
    {
        const getX = function(angle)
            {
                return (amplitude * (cos(angle) - 1)) + boundingPlaneXOffset;
            };

        return {
                "getX" : getX,
                "getPoint" : function(angle)
                    {
                        return vector(getX(angle), angle * yScale);
                    },
                "dXdY" : function(angle)
                    {
                        return (amplitude * -sin(angle)) * dAngledY;
                    },
                "closestApproachRange" : [90 * degree, 180 * degree]
            };
    }
    else
    {
        throw "Hem did not lean an expected way";
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
function findClosestApproachAngle(unrolledSinusoidData is map, isStart is boolean) returns ValueWithUnits
{
    const absoluteTop = unrolledSinusoidData.closestApproachRange[1];
    const absoluteBottom = unrolledSinusoidData.closestApproachRange[0];

    var aboveAngle = absoluteTop;
    var currAngle = (absoluteTop + absoluteBottom) / 2;
    var belowAngle = absoluteBottom;
    while (true) // TODO: either adjust tolerance or number of explicit steps
    {
        const currPoint = unrolledSinusoidData.getPoint(currAngle);
        const currdXdY = unrolledSinusoidData.dXdY(currAngle);

        const xIntercept = currPoint[0] - (currdXdY * currPoint[1]);
        if (abs(xIntercept) < (TOLERANCE.zeroLength * meter))
        {
            break;
        }

        if ((xIntercept > 0) == isStart)
        {
            aboveAngle = currAngle;
            currAngle = (currAngle + belowAngle) / 2;
        }
        else
        {
            belowAngle = currAngle;
            currAngle = (currAngle + aboveAngle) / 2;
        }

        if (currAngle - absoluteBottom < 0.00001 * degree)
        {
            currAngle = absoluteBottom;
            break;
        }
        else if (absoluteTop - currAngle < 0.00001 * degree)
        {
            currAngle = absoluteTop;
            break;
        }
    }
    @report("Optimal helix angle: " ~ roundToPrecision((currAngle - absoluteBottom) / degree, 3) ~ " degrees past sinusiod inflection");
    return currAngle;
}

const STEPS_PER_DEGREE = 2.5;
/**
 * Define a spline which traverses the boundary defined by innerSinusoidData and outerSinusoidData.  Returns an array of arrays;
 * each of the inner arrays can be sketched as as a spline to form a a contiguous boundary.
 *
 * When sketched and thickened, this boundary will ensure that the entire arc of thickened metal is always coincident
 * with the minimal clearance plane in question.  To maintain this condition, the thickened arc sheet may need to switch
 * whether its inner or outer lip is coincident to the plane.  The switch from one array to the next in the returned array
 * represents this switch.
 */
function constructSplinePortion(lowBound is ValueWithUnits, highBound is ValueWithUnits, innerSinusoidData is map, outerSinusoidData is map, isStart is boolean, hemData is map) returns array
{
    const totalAngle = highBound - lowBound;
    const splineSteps = round((totalAngle / degree) * STEPS_PER_DEGREE);

    const zeroDegreeSplineValues = [innerSinusoidData.getX(0 * degree), outerSinusoidData.getX(0 * degree)];
    const zeroDegreeXDiff = zeroDegreeSplineValues[getSplineIndexToUse(isStart, zeroDegreeSplineValues)];
    const ninetyDegrees = 90 * degree;

    var currSplinePortionIndex = -1;
    var splinePortion = [];
    var prevSplineIndexUsed = undefined;
    for (var i = 0; i <= splineSteps; i += 1)
    {
        const angle = lowBound + (totalAngle * (i / splineSteps));
        const splineValues = [innerSinusoidData.getX(angle), outerSinusoidData.getX(angle)];

        var splineIndexToUse = getSplineIndexToUse(isStart, splineValues);
        if (splineIndexToUse != prevSplineIndexUsed)
        {
            // Start a new spline.  If we do not use a new spline when switching, straight flattened hems fail due to necessary
            // waving in their splines.
            prevSplineIndexUsed = splineIndexToUse;
            currSplinePortionIndex += 1;
            if (currSplinePortionIndex == 0)
            {
                splinePortion = append(splinePortion, []);
            }
            else
            {
                // Start this spline with the final point of the last spline
                const previousSize = size(splinePortion[currSplinePortionIndex - 1]);
                const previousFinalValue = splinePortion[currSplinePortionIndex - 1][previousSize - 1];
                splinePortion = append(splinePortion, [previousFinalValue]);
            }
        }

        var x = splineValues[splineIndexToUse];
        // Blend the initial difference smoothly into the boundary curve over the course of 0 -> 45 degrees (absolute,
        // i.e. does not apply at all if lowBound > 45 * degree)
        const xBlend = 1 - sin(min(angle * 2, ninetyDegrees));
        x -= (xBlend * zeroDegreeXDiff);

        // Arc length = (arcAngle / (2 * pi)) * (2 * pi * r)
        //            = arcAngle * r
        const y = (angle / radian) * hemData.arcRadius;

        splinePortion[currSplinePortionIndex] = append(splinePortion[currSplinePortionIndex], vector(x, y));
    }

    return splinePortion;
}

/**
 * Determine which of index 0 and 1 of `splineValues` imposes a the stricter boundary, and must be followed for the current angle.
 */
function getSplineIndexToUse(isStart is boolean, splineValues is array) returns number
{
    var splineIndexToUse;
    if (isStart)
    {
        // if isStart, we always want the larger of the two values (favoring index 0 if within tolerance)
        splineIndexToUse = splineValues[0] > (splineValues[1] - (TOLERANCE.zeroLength * meter)) ? 0 : 1;
    }
    else
    {
        // if !isStart, we always want the smaller of the two values (favoring index 0 if within tolerance)
        splineIndexToUse = splineValues[0] < (splineValues[1] + (TOLERANCE.zeroLength * meter)) ? 0 : 1;
    }
    return splineIndexToUse;
}

// -------------------------------------------------- Sheet creation --------------------------------------------------

/**
 * Construct the hem sheets. Return the created hem sheets as a map.
 * @returns {{
 *      @field arcSheet {Query} : a query for the arc sheet
 *      @field otherSheet {Query} : a query for the sheet representing the rest of the hem
 * }}
 */
function constructHemSheets(context is Context, id is Id, helperTools is map, hemData is map, definition is map) returns map
{
    // Roll the provided sketch to form the hem arc
    const arcRollId = id + "roll";
    opRoll(context, arcRollId, {
                "entities" : helperTools.flatArcSketch,
                "source" : helperTools.rollParameters.rollPlane,
                "destination" : helperTools.rollParameters.rollCylinder
            });

    // Extrude the rest of the hem besides the initial arc.  This may be nothing for a ROLLED hem, or a planar section for
    // a STRAIGHT or TEAR_DROP hem.
    const extrudeId = id + "extrude";
    const sketchEdges = qConstructionFilter(qCreatedBy(hemData.profileSketchId, EntityType.EDGE), ConstructionObject.NO);
    const otherSketchEdges = qSubtraction(sketchEdges, hemData.arcEdge);
    if (evaluateQuery(context, otherSketchEdges) != [])
    {
        opExtrude(context, extrudeId, {
                    "entities" : otherSketchEdges,
                    "direction" : hemData.edgeDirection,
                    "startBound" : BoundingType.UP_TO_SURFACE,
                    "startBoundEntity" : helperTools.startBoundingPlane,
                    "endBound" : BoundingType.UP_TO_SURFACE,
                    "endBoundEntity" : helperTools.endBoundingPlane
                });
    }

    return {
            "arcSheet" : qCreatedBy(arcRollId, EntityType.BODY),
            "otherSheet" : qCreatedBy(extrudeId, EntityType.BODY)
        };
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
    bendAttribute.radius = { "value" : getInnerRadius(modelParameters, definition), "canBeEdited" : false };
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
        if (evaluateQuery(context, qOwnedByBody(otherSheet, EntityType.FACE)) != [])
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

    if (evaluateQuery(context, otherSheet) == [])
    {
        // If there is no otherSheet, we are done
        return matches;
    }

    const otherSheetEdges = qOwnedByBody(otherSheet, EntityType.EDGE);
    const matchedBetweenArcAndOther = evaluateQuery(context, qContainsPoint(qUnion([arcSheetEdges, otherSheetEdges]), hemData.arcEndLine.origin));
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

// -------------------------------------------------- Utilities --------------------------------------------------

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

function getInnerRadius(modelParameters is map, definition is map) returns ValueWithUnits
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
    // Lock tipGap to match innerRadius until the user first opens that part of the dialog.  That way the first time they open it, it is a valid number
    if (isCreating && oldDefinition.innerRadius != definition.innerRadius && specifiedParameters.innerRadius && !specifiedParameters.useMinimalGapForTipGap)
    {
        definition.tipGap = definition.innerRadius;
    }

    return definition;
}

