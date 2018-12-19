FeatureScript 975; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "975.0");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "975.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "975.0");
import(path : "onshape/std/containers.fs", version : "975.0");
import(path : "onshape/std/context.fs", version : "975.0");
import(path : "onshape/std/curveGeometry.fs", version : "975.0");
import(path : "onshape/std/error.fs", version : "975.0");
import(path : "onshape/std/evaluate.fs", version : "975.0");
import(path : "onshape/std/feature.fs", version : "975.0");
import(path : "onshape/std/math.fs", version : "975.0");
import(path : "onshape/std/query.fs", version : "975.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "975.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "975.0");
import(path : "onshape/std/sketch.fs", version : "975.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "975.0");
import(path : "onshape/std/topologyUtils.fs", version : "975.0");
import(path : "onshape/std/valueBounds.fs", version : "975.0");
import(path : "onshape/std/vector.fs", version : "975.0");

/**
 * Specifies the type of alignment of the hem
 * TODO: Discuss names with UX
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
 * TODO: Hem documentation
 */
annotation { "Feature Type Name" : "Hem", "Editing Logic Function" : "hemEditLogic" }
export const sheetMetalHem = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Edges or side faces to hem",
                     "Filter" : (SheetMetalDefinitionEntityType.EDGE && (GeometryType.LINE || GeometryType.PLANE))
                        && AllowFlattenedGeometry.YES && ModifiableEntityOnly.YES }
        definition.edges is Query;

        annotation { "Name" : "Hem alignment" }
        definition.hemAlignment is SMHemAlignment;

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
            annotation { "Name" : "Total length" } // TODO: wording? "Total hem length"? just "length"?
            isLength(definition.length, NONNEGATIVE_LENGTH_BOUNDS);
        }
    }
    {
        // ---------- //
        // TODO: COPIED FROM FLANGE, NEEDS NEW ERROR MESSAGES
        // this is not necessary but helps with correct error reporting in feature pattern
        checkNotInFeaturePattern(context, definition.edges, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        if (size(evaluateQuery(context, definition.edges)) == 0)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_NO_EDGES, ["edges"]);
        }

        var edges = qUnion(getSMDefinitionEntities(context, definition.edges));
        var nonLineEdgeQ = qSubtraction(qEntityFilter(edges, EntityType.EDGE), qGeometry(edges, GeometryType.LINE));
        if (size(evaluateQuery(context, nonLineEdgeQ)) != 0)
        {
            setErrorEntities(context, id, { "entities" : nonLineEdgeQ });
            edges = qGeometry(edges, GeometryType.LINE);
            if (size(evaluateQuery(context, edges)) != 0)
            {
                reportFeatureWarning(context, id, ErrorStringEnum.SHEET_METAL_FLANGE_NON_LINEAR_EDGES);
            }
            else
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_NON_LINEAR_EDGES, ["edges"]);
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
        // ---------- //

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

    var edgeToHemData = {};
    for (var edge in edgeArray)
    {
        edgeToHemData[edge] = getHemData(context, edge);
    }

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
    const sharedVertexToBoundingPlane = new box({});
    for (var edge in evaluateQuery(context, edges))
    {
        // TODO: Investiage whether this disambiguation is useful, and successfully disambiguates the planes
        var edgeId = bodyId + unstableIdComponent("edge" ~ edgeIndex);
        edgeIndex += 1;
        setExternalDisambiguation(context, edgeId, edge);

        const hemData = edgeToHemData[edge];

        const planeId = edgeId + "plane";
        const boundingPlanes = constructHemBoundingPlanes(context, planeId, edge, edgeToHemData, definition,
                modelParameters, sharedVertexToBoundingPlane);

        const hemSheetId = edgeId + "hemSheet";
        const sheetMap = constructHemSheet(context, hemSheetId, hemData.profileSketchId, hemData.arcEdge, boundingPlanes, edgeToHemData[edge]);

        annotateHemSheet(context, topLevelId, sheetMap.arcSheet, sheetMap.otherSheet, attributeIdCounter, hemData, modelParameters, definition);

        const coincidentVertexResult = evaluateQuery(context, hemData.coincidentVertexTracking);
        if (size(coincidentVertexResult) != 1)
            throw "Hem extrusion did not result in expected edge"; // TODO: generic ErrorStringEnum
        matches = append(matches, {
                    "topology1" : edge,
                    "topology2" : coincidentVertexResult[0],
                    "matchType" : TopologyMatchType.COINCIDENT
                });

        bodiesToBoolean = append(bodiesToBoolean, qCreatedBy(hemSheetId, EntityType.BODY));
        bodiesToDelete[] = append(bodiesToDelete[], qUnion([qCreatedBy(hemData.profileSketchId, EntityType.BODY), qCreatedBy(planeId, EntityType.BODY)]));
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

// ---------- Hem data ----------

/**
 * @return {{
 *      @field edgeDirection {Vector} : the direction of the edge (with edge orientation determined by the adjacent face)
 *      @field edgeStartPosition {Line}  : the position of the start of the edge (with edge orientation determined by the adjacent face)
 *      @field edgeCenterPosition {Line} : the position of the center of the edge (with edge orientation determined by the adjacent face)
 *      @field edgeEndPosition {Line}    : the position of the end of the edge (with edge orientation determined by the adjacent face)
 *      @field outFromFace {Vector} : the direction that is tangent to the face and pointing directly away from the face, evaluated at the center of the edge.
 *      @field startVertex {Query} : the start vertex of the edge (with edge orientation determined by the adjacent face)
 *      @field endVertex {Query}   : the end vertex of the edge (with edge orientation determined by the adjacent face)
 * }}
 */
function getHemData(context is Context, edge is Query) returns map
{
    const faces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
    if (size(faces) != 1)
        throw "Too many faces"; //TODO: This is where we fail out if a nonlaminar edge is selected.  Either fail earlier or replace with an appropriate error.
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

// ---------- Hem alignment ----------

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
        throw regenError(ErrorStringEnum.SHEET_METAL_FLANGE_FAIL_ALIGNMENT, ["flangeAlignment"]); // TODO: Hem alignment error
    }

    return resultEdges;
}

// ---------- Hem sketch ----------

/**
 * Sketch the hem profile for a given edge. `hemData` should be in the format outputted by `getHemData`.
 * @returns {{
 *     @field coincidentVertexTracking {Query} : a tracking query for the point on the sketch that is coincident with the hem edge.
 *     @field arcEdge {Query} : the arc of the hem profile.
 *     @field arcAngle {ValueWithUnits} : the central angle of the initial arc.
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
            throw "hem too short for teardrop"; // TODO: Error message
        if (tipGap > 2 * innerRadius - TOLERANCE.zeroLength * meter)
            throw "Tip gap too large for teardrop"; // TODO: Error message

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

    if (arcAngle == undefined)
    {
        if (definition.hemType != SMHemType.TEAR_DROP)
        {
            throw "arcAngle should have been set"; // User will not hit this
        }
        // Must evaluate arc angle for tear drop after the solve since tear drop relies on constraints for positioning.
        const arcLength = evLength(context, { "entities" : arcEdge });
        const arcRadius = evCurveDefinition(context, { "edge" : arcEdge }).radius;
        // arc percent of circle = arc length / (2 * pi * r)
        // arc angle = 2 * pi * arc percent of circle
        // arc angle = arc length / r
        arcAngle = (arcLength / arcRadius) * radian;
    }

    return {
            "coincidentVertexTracking" : startTracking(context, id, arcInfo.arcStartId),
            "arcEdge" : arcEdge,
            "arcAngle" : arcAngle,
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
            "arcEnd" : arcEnd
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
        throw "Hem too short"; // TODO: Error message
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

// ---------- Extrusion, and bounding entities for extrusion ----------

/**
 * Construct bounding planes for the extremes of the hem. `hemData` should be in the format outputted by `getHemData`.
 * @return {{
 *      @field startBoundingPlane {Query} : a construction plane to bound the hem at the start vertex
 *      @field endBoundingPlane {Query} : a construction plane to bound the hem at the end vertex
 *      @field startBoundingPlaneForArc {Query} : a construction plane to bound the arc portion of the hem at the start vertex
 *      @field endBoundingPlaneForArc {Query} : a construction plane to bound the arc portion of the hem at the end vertex
 * }}
 */
function constructHemBoundingPlanes(context is Context, id is Id, edge is Query, edgeToHemData is map,
        definition is map, modelParameters is map, sharedVertexToBoundingPlane is box) returns map
{
    const startBoundingPlanesReturn = constructHemBoundingPlanesForSide(context, id + "start", edge, true, edgeToHemData,
        definition, modelParameters, sharedVertexToBoundingPlane);

    const endBoundingPlanesReturn = constructHemBoundingPlanesForSide(context, id + "end", edge, false, edgeToHemData,
        definition, modelParameters, sharedVertexToBoundingPlane);

    return {
            "startBoundingPlane" : startBoundingPlanesReturn.boundingPlane,
            "endBoundingPlane" : endBoundingPlanesReturn.boundingPlane,
            "startBoundingPlaneForArc" : startBoundingPlanesReturn.boundingPlaneForArc,
            "endBoundingPlaneForArc" : endBoundingPlanesReturn.boundingPlaneForArc
        };
}

/**
 * Construct the hem bounding planes for the start or end of an edge
 * @return {{
 *      @field boundingPlane {Query} : a construction plane to bound the hem at the given side
 *      @field boundingPlaneForArc {Query} : a construction plane to bound the arc portion of the hem at the given side
 * }}
 */
function constructHemBoundingPlanesForSide(context is Context, id is Id, edge is Query, isStart is boolean, edgeToHemData is map,
        definition is map, modelParameters is map, sharedVertexToBoundingPlane is box) returns map
{
    const boundingPlaneReturn = constructHemBoundingPlaneAtVertex(context, id + "boundingPlane", edge, isStart, edgeToHemData,
            definition, modelParameters, sharedVertexToBoundingPlane);

    var boundingPlaneForArc;
    if (boundingPlaneReturn.useBoundingPlaneForArcPlane)
    {
        boundingPlaneForArc = boundingPlaneReturn.planeQuery;
    }
    else
    {
        boundingPlaneForArc = constructHemBoundingPlaneForArc(context, id + "arcBoundingPlane", isStart, boundingPlaneReturn.planeDefinition,
                edgeToHemData[edge], definition, modelParameters);
    }

    return {
            "boundingPlane" : boundingPlaneReturn.planeQuery,
            "boundingPlaneForArc" : boundingPlaneForArc
        };
}

/**
 * Construct a bounding plane for the given `edge`, at the given vertex (as defined by `isStart`)
 * @returns {{
 *      @field planeDefinition {Plane}: the definition of the bounding plane for the given vertex
 *      @field planeQuery {Plane} : a query for the bounding plane for the given vertex
 *      @field useBoundingPlaneForArcPlane {boolean} : whether the constructed bounding plane is suitable for use as the arc bounding plane
 * }}
 */
function constructHemBoundingPlaneAtVertex(context is Context, id is Id, edge is Query, isStart is boolean, edgeToHemData is map,
        definition is map, modelParameters is map, sharedVertexToBoundingPlane is box) returns map
{
    const defineHemBoundingPlaneReturn = defineHemBoundingPlaneAtVertex(context, edge, isStart, edgeToHemData, definition,
            modelParameters, sharedVertexToBoundingPlane);

    // Push back the plane by the appropriate amount
    var adjustedBoundingPlane = defineHemBoundingPlaneReturn.boundingPlane;
    adjustedBoundingPlane.origin += (adjustedBoundingPlane.normal * ((isStart ? 1.0 : -1.0) * defineHemBoundingPlaneReturn.pushBack));

    // Optimization: for ROLLED hems, we will not have a use for this plane unless we also need to use it as the arc plane.
    const skipPlaneCreation = (definition.hemType == SMHemType.ROLLED) && !defineHemBoundingPlaneReturn.useBoundingPlaneForArcPlane;

    var planeQuery;
    if (!skipPlaneCreation)
    {
        const planeId = id + "plane";
        opPlane(context, planeId, {
                    "plane" : adjustedBoundingPlane
                });
        planeQuery = qCreatedBy(planeId, EntityType.FACE);
    }
    else
    {
        planeQuery = qNothing();
    }

    return {
            "planeDefinition" : adjustedBoundingPlane,
            "planeQuery" : planeQuery,
            "useBoundingPlaneForArcPlane" : defineHemBoundingPlaneReturn.useBoundingPlaneForArcPlane
        };
}

/**
 * Find the definition of the bounding plane for the given `edge`, at the given vertex (as defined by `isStart`)
 * @returns {{
 *      @field boundingPlane {Plane}: the definition of the bounding plane to use at the given vertex
 *      @field pushBack {ValueWithUnits} : the adjustment that needs to be made to the bounding plane before construction
 *      @field useBoundingPlaneForArcPlane {boolean} : whether the constructed bounding plane is suitable for use as the arc bounding plane.
 *                                                     If we are not mitering, the same plane should be suitable. If we are mitering, the
 *                                                     arc portion of the hem needs to be treated differently than the rest of the hem.
 * }}
 */
function defineHemBoundingPlaneAtVertex(context is Context, edge is Query, isStart is boolean, edgeToHemData is map, definition is map,
        modelParameters is map, sharedVertexToBoundingPlane is box) returns map
{
    const hemData = edgeToHemData[edge];
    const vertexInQuestion = isStart ? hemData.startVertex : hemData.endVertex;

    // ----- First check if we already have the bounding plane for this vertex -----
    if (sharedVertexToBoundingPlane[][vertexInQuestion] != undefined)
    {
        // We have already constructed a bounding plane for this vertex when processing its neighbor.
        return {
                "boundingPlane" : sharedVertexToBoundingPlane[][vertexInQuestion].boundingPlane,
                "pushBack" : sharedVertexToBoundingPlane[][vertexInQuestion].pushBack,
                "useBoundingPlaneForArcPlane" : false
            };
    }

    // ----- Next, check if there is a neighbor to account for -----
    const tangentLineInQuestion = line(isStart ? hemData.edgeStartPosition : hemData.edgeEndPosition, hemData.edgeDirection);

    const otherHemEdges = qSubtraction(qUnion(keys(edgeToHemData)), edge);
    const edgesOfVertexInQuestion = qVertexAdjacent(vertexInQuestion, EntityType.EDGE);
    const neighboringHemEdges = evaluateQuery(context, qIntersection([otherHemEdges, edgesOfVertexInQuestion]));
    if (neighboringHemEdges == [])
    {
        // There is no neighboring hem, construct a simple bounding plane.
        return {
                "boundingPlane" : plane(tangentLineInQuestion.origin, tangentLineInQuestion.direction),
                "pushBack" : 0 * meter,
                "useBoundingPlaneForArcPlane" : true
            };
    }

    // ----- There is a neighbor, do the appropriate work to form a miter plane -----
    if (size(neighboringHemEdges) != 1)
    {
        throw "Should not have found multiple neighbor hem edges";
    }
    const neighboringHemData = edgeToHemData[neighboringHemEdges[0]];

    var miterPlaneDefinition = defineHemMiterPlaneAtVertex(isStart, vertexInQuestion, tangentLineInQuestion, hemData,
            neighboringHemData, definition, modelParameters, sharedVertexToBoundingPlane);
    miterPlaneDefinition.useBoundingPlaneForArcPlane = false;
    return miterPlaneDefinition;
}

/**
 * Helper for creating a miter plane in defineHemBoundingPlaneAtVertex.
 * @returns {{
 *      @field boundingPlane {Plane}: the definition of the bounding plane to use at the given vertex
 *      @field pushBack {ValueWithUnits} : the adjustment that needs to be made to the bounding plane before construction
 * }}
 */
function defineHemMiterPlaneAtVertex(isStart is boolean, vertexInQuestion is Query, tangentLineInQuestion is Line, hemData is map,
        neighboringHemData is map, definition is map, modelParameters is map, sharedVertexToBoundingPlane is box) returns map
{
    // Because we are using coEdge normals, the neighboring vertex will be at the end if isStart, or the start if !isStart.
    const neighboringVertexIsStart = !isStart;
    const neighboringTangentLineInQuestion = line(neighboringVertexIsStart ? neighboringHemData.edgeStartPosition : neighboringHemData.edgeEndPosition, neighboringHemData.edgeDirection);

    const bisector = normalize(tangentLineInQuestion.direction + neighboringTangentLineInQuestion.direction);

    // TODO: Handle outward case
    var boundingPlane = plane(tangentLineInQuestion.origin, bisector);

    // Hems should be `minimalClearance` distance apart from each other
    var pushBack = 0.5 * modelParameters.minimalClearance;

    // If some thickness extends outside the hem, push back an addition distance so that the closest approach of the material is still `minimalClearance` apart.
    const wrapAroundFront = getWrapAroundFront(definition);
    const relevantThickness = wrapAroundFront ? modelParameters.backThickness : modelParameters.frontThickness;
    if (!tolerantEquals(relevantThickness, 0 * meter))
    {
        const hemPlaneNormal = cross(tangentLineInQuestion.direction, hemData.arcEndLine.direction);
        const neighboringHemPlaneNormal = cross(neighboringTangentLineInQuestion.direction, neighboringHemData.arcEndLine.direction);
        const angleBetweenNormals = angleBetween(hemPlaneNormal, neighboringHemPlaneNormal);
        const angleBetweenHemPlanes = (180 * degree) - angleBetweenNormals;

        pushBack += (modelParameters.frontThickness + modelParameters.backThickness) * cos(0.5 * angleBetweenHemPlanes);
    }

    const returnMap = {
            "boundingPlane" : boundingPlane,
            "pushBack" : pushBack
        };

    // Store the shared plane information in the box for later use
    sharedVertexToBoundingPlane[][vertexInQuestion] = returnMap;

    return returnMap;
}

/**
 * Construct a bounding plane for the start or end of the arc portion of the hem specified by `hemData`.  `hemBoundingPlane`
 * should be the result of `constructHemBoundingPlaneAtVertex(...)`.  Returns a [Query] for the constructed bounding plane.
 */
function constructHemBoundingPlaneForArc(context is Context, id is Id, isStart is boolean, hemBoundingPlane is Plane,
        hemData is map, definition is map, modelParameters is map) returns Query
{
    const arcEndPosition = hemData.arcEndLine.origin;
    const edgeDirection = hemData.edgeDirection;

    if (tolerantEquals(hemBoundingPlane.normal, edgeDirection))
    {
        throw "Plane already exists, should have been skipped";
    }

    // Create a helix whose axis is the center of the arc, which curls up from the vertex of the hem edge to the pushed
    // back vertex of the arc end edge.

    // Intersection of arc end line and hem bounding plane will be the location of the vertex on the arc end edge.
    const lineAlongArcEndEdge = line(arcEndPosition, edgeDirection);
    const intersectionResult = intersection(hemBoundingPlane, lineAlongArcEndEdge);
    if (intersectionResult.dim != 0)
        throw "Expected arc end line and bounding plane to intersect at a point";

    // Vertex position of the start of the helix
    const vertexPosition = isStart ? hemData.edgeStartPosition : hemData.edgeEndPosition;

    const hemCircle = evCurveDefinition(context, {
                "edge" : hemData.arcEdge
            });
    const hemCircleCenter = hemCircle.coordSystem.origin;

    // Project the center of the hem arc onto the plane at the vertex of the hem edge.
    const edgeEndPlane = plane(vertexPosition, edgeDirection);
    const projectedCenter = project(edgeEndPlane, hemCircleCenter);

    // Project the vector from the start of the helix axis to the end of the helix onto the helix axis.  This is the total
    // height of the helix.  Use this to find the helical pitch (a.k.a. the height of one turn).
    const projectedCenterToIntersection = intersectionResult.intersection - projectedCenter;
    const lengthAlongEdgeToIntersection = dot((isStart ? 1.0 : -1.0) * edgeDirection, projectedCenterToIntersection);

    const arcPercentOfCircle = hemData.arcAngle / (360 * degree);
    const heightOfOneTurn = lengthAlongEdgeToIntersection / arcPercentOfCircle;

    const wrapAroundFront = getWrapAroundFront(definition);
    opHelix(context, id + "helix1", {
                "direction" : (isStart ? 1.0 : -1.0) * edgeDirection,
                "axisStart" : projectedCenter,
                "startPoint" : vertexPosition,
                "interval" : [0, arcPercentOfCircle],
                "clockwise" : isStart != wrapAroundFront,
                "helicalPitch" : heightOfOneTurn,
                "spiralPitch" : 0 * inch
            });

    // TODO: actually use the helix correctly
    const helixPoints = evEdgeTangentLines(context, {
                "edge" : qCreatedBy(id + "helix1", EntityType.EDGE),
                "parameters" : [0, 0.5, 1]
            });
    const boundingPlaneNormal = cross(normalize(helixPoints[2].origin - helixPoints[1].origin), normalize(helixPoints[1].origin - helixPoints[0].origin));
    const boundingPlaneForArc = plane(helixPoints[0].origin, boundingPlaneNormal);

    const planeId = id + "plane";
    opPlane(context, planeId, {
                "plane" : boundingPlaneForArc
            });
    return qCreatedBy(planeId, EntityType.FACE);
}

/**
 * Extrude the hem profile to become the hem sheet. Return the edge at which the hem sheet will attach to the master sheet body.
 * @return {{
 *      @field arcSheet {Query} : a query for the arc sheet
 *      @field otherSheet {Query} : a query for the sheet representing the rest of the hem
 * }}
 */
function constructHemSheet(context is Context, id is Id, sketchId is Id, arcEdge is Query, boundingPlanes is map, hemData is map) returns map
{
    const arcExtrudeId = id + "extrudeArc";
    opExtrude(context, arcExtrudeId, {
                "entities" : arcEdge,
                "direction" : hemData.edgeDirection,
                "startBound" : BoundingType.UP_TO_SURFACE,
                "startBoundEntity" : boundingPlanes.startBoundingPlaneForArc,
                "endBound" : BoundingType.UP_TO_SURFACE,
                "endBoundEntity" : boundingPlanes.endBoundingPlaneForArc
            });

    // Extrude the rest of the hem besides the initial arc.  This may be nothing for a ROLLED hem, or a planar section for
    // a STRAIGHT or TEAR_DROP hem.
    const otherExtrudeId = id + "extrudeOther";
    const sketchEdges = qConstructionFilter(qCreatedBy(sketchId, EntityType.EDGE), ConstructionObject.NO);
    const otherSketchEdges = qSubtraction(sketchEdges, arcEdge);
    if (evaluateQuery(context, otherSketchEdges) != [])
    {
        opExtrude(context, otherExtrudeId, {
                    "entities" : otherSketchEdges,
                    "direction" : hemData.edgeDirection,
                    "startBound" : BoundingType.UP_TO_SURFACE,
                    "startBoundEntity" : boundingPlanes.startBoundingPlane,
                    "endBound" : BoundingType.UP_TO_SURFACE,
                    "endBoundEntity" : boundingPlanes.endBoundingPlane
                });
    }
    return {
            "arcSheet" : qCreatedBy(arcExtrudeId, EntityType.BODY),
            "otherSheet" : qCreatedBy(otherExtrudeId, EntityType.BODY)
        };
}

// ---------- Sheet annotation ----------

/**
 * Add the appropriate sheet metal attributes to the hem sheet. `coincidentEdge` is the edge at which the hem sheet will attach to the master
 * sheet body.
 */
function annotateHemSheet(context is Context, topLevelId is Id, arcSheet is Query, otherSheet is Query, attributeIdCounter is box,
        hemData is map, modelParameters is map, definition is map)
{
    // Face of arc sheet needs bend attribute
    var bendAttribute = makeSMJointAttribute(toAttributeId(topLevelId + attributeIdCounter[]));
    bendAttribute.jointType = { "value" : SMJointType.BEND, "canBeEdited" : false };
    bendAttribute.bendType = { "value" : SMBendType.HEM, "canBeEdited" : false };
    bendAttribute.radius = { "value" : getInnerRadius(modelParameters, definition), "canBeEdited" : false };
    bendAttribute.angle = { "value" :  hemData.arcAngle, "canBeEdited" : false};
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
            throw "Expected straight or tear drop hem to have one other face"; // TODO: generic ErrorStringEnum
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
            throw "Did not expect rolled to have additional sheet"; // TODO: generic ErrorStringEnum
        }
    }
    else
    {
        throwHemTypeError(definition.hemType);
    }
}

// ---------- Utilities ----------

function getWrapAroundFront(definition is map)
{
    return definition.oppositeDirection;
}

function getInnerRadius(modelParameters is map, definition is map) returns ValueWithUnits
{
    if (definition.hemType == SMHemType.ROLLED || definition.hemType == SMHemType.TEAR_DROP)
    {
        return definition.innerRadius;
    }
    else if (definition.hemType == SMHemType.STRAIGHT)
    {
        return definition.useMinimalGapForStraight ? modelParameters.minimalClearance : definition.innerRadius;
    }
    else
    {
        // This cannot be hit by a UI user.
        throwHemTypeError(definition.hemType);
    }
}

function throwHemTypeError(hemType is SMHemType)
{
    throw "Unrecognized hem type: " ~ hemType;
}

// ---------- Editing Logic ----------

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

