FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "✨");
import(path : "onshape/std/boundingtype.gen.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/sketch.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/topologyUtils.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");

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
    annotation { "Name" : "Rolled" }
    ROLLED,
    annotation { "Name" : "Straight" }
    STRAIGHT,
    annotation { "Name" : "Tear drop" }
    TEAR_DROP
}

/**
 * @internal
 * An `AngleBoundSpec` for hems.  Defaults to 270 (or 3pi/2), and is limited to (0 degrees, 360 degrees) exclusive.
 */
export const SM_HEM_ANGLE_BOUNDS =
{
    (degree) : [0.1, 270, 359.9]
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

        if (definition.hemType == SMHemType.ROLLED || definition.hemType == SMHemType.TEAR_DROP)
        {
            annotation { "Name" : "Inner radius" }
            isLength(definition.innerRadius, SM_HEM_INNER_RADIUS_BOUNDS);
        }

        if (definition.hemType == SMHemType.STRAIGHT || definition.hemType == SMHemType.TEAR_DROP)
        {
            // TODO: Ask UX if REMEMBER_PREVIOUS_VALUE is appropriate when sharing this variable between tip gap and straight gap
            annotation { "Name" : "Use minimal gap", "Default" : true, "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            definition.useMinimalGap is boolean;

            if (!definition.useMinimalGap)
            {
                // innerDiameter and tipGap are both presented as "Gap", but are kept separately so that innerDiameter can be
                // kept in sync with innerRadius. See hemEditLogic.
                if (definition.hemType == SMHemType.STRAIGHT)
                {
                    annotation { "Name" : "Gap" }
                    isLength(definition.innerDiameter, SM_HEM_GAP_BOUNDS);
                }
                else if (definition.hemType == SMHemType.TEAR_DROP)
                {
                    annotation { "Name" : "Gap" }
                    isLength(definition.tipGap, SM_HEM_INNER_RADIUS_BOUNDS);
                }
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

    var edgeToHemData = {};
    for (var edge in evaluateQuery(context, edges))
    {
        edgeToHemData[edge] = getHemData(context, edge);
    }

    var edgeIndex = 0;
    var matches = [];
    var bodiesToBoolean = [];
    for (var edge in evaluateQuery(context, edges))
    {
        var edgeId = bodyId + unstableIdComponent("edge" ~ edgeIndex);
        edgeIndex += 1;
        setExternalDisambiguation(context, edgeId, edge);

        const sketchId = edgeId + "sketch";
        const coincidentVertexTracking = sketchHemProfile(context, sketchId, edgeToHemData[edge], modelParameters, definition);

        const planeId = edgeId + "plane";
        const boundingPlanes = constructHemBoundingPlanes(context, planeId, edgeToHemData[edge]);

        const hemSheetId = edgeId + "hemSheet";
        constructHemSheet(context, hemSheetId, sketchId, boundingPlanes, edgeToHemData[edge]);

        const coincidentVertexResult = evaluateQuery(context, coincidentVertexTracking);
        if (size(coincidentVertexResult) != 1)
            throw "Hem extrusion did not result in expected edge"; // TODO: Maybe replace with generic ErrorStringEnum? This should not be hit, but if it is we should translate it.
        const coincidentEdge = coincidentVertexResult[0];
        annotateHemSheet(context, topLevelId, coincidentEdge, qCreatedBy(hemSheetId, EntityType.BODY), attributeIdCounter);

        matches = append(matches, {
                    "topology1" : edge,
                    "topology2" : coincidentEdge,
                    "matchType" : TopologyMatchType.COINCIDENT
                });

        bodiesToBoolean = append(bodiesToBoolean, qCreatedBy(hemSheetId, EntityType.BODY));
        bodiesToDelete[] = append(bodiesToDelete[], qUnion([qCreatedBy(sketchId, EntityType.BODY), qCreatedBy(planeId, EntityType.BODY)]));
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

function getInnerRadius(modelParameters is map, definition is map) returns ValueWithUnits
{
    if (definition.hemType == SMHemType.ROLLED || definition.hemType == SMHemType.TEAR_DROP)
    {
        return definition.innerRadius;
    }
    else if (definition.hemType == SMHemType.STRAIGHT)
    {
        var innerDiameter = definition.useMinimalGap ? modelParameters.minimalClearance : definition.innerDiameter;
        return innerDiameter / 2;
    }
    else
    {
        // This cannot be hit by a UI user.
        throwHemTypeError(definition.hemType);
    }
}

/**
 * @return {{
 *      @field adjacentFace {Query} : the face adjacent to the given edge
 *      @field edgeTangentLineAtStart {Line}  : the tangent line at the start of the edge (with edge orientation determined by face)
 *      @field edgeTangentLineAtCenter {Line} : the tangent line at the center of the edge (with edge orientation determined by face)
 *      @field edgeTangentLineAtEnd {Line}    : the tangent line at the end of the edge (with edge orientation determined by face)
 *      @field outFromFace {Vector} : the direction that is tangent to the face and pointing directly away from the face, evaluated at the center of the edge.
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
    const edgeTangentLineAtCenter = edgeTangentLines[1];
    const faceTangentPlaneAtCenter = evFaceTangentPlaneAtEdge(context, {
                "edge" : edge,
                "face" : face,
                "parameter" : 0.5,
                "usingFaceOrientation" : true
            });
    const outFromFace = cross(edgeTangentLineAtCenter.direction, faceTangentPlaneAtCenter.normal);

    return {
            "adjacentFace" : face,
            "edgeTangentLineAtStart" : edgeTangentLines[0],
            "edgeTangentLineAtCenter" : edgeTangentLineAtCenter,
            "edgeTangentLineAtEnd" : edgeTangentLines[2],
            "outFromFace" : outFromFace
        };
}

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

/**
 * Sketch the hem profile for a given edge. `hemData` should be in the format outputted by `getHemData`.
 * Return a tracking query for the point on the sketch that is coincident with the hem edge.
 */
function sketchHemProfile(context is Context, id is Id, hemData is map, modelParameters is map, definition is map) returns Query
{
    // Construct the sketch such that the origin is at the center of the edge, +X is tangent to the face and pointing away from the face,
    // and +Y is the direction of back thickness of the sheet (a.k.a. the anti-normal of the face).
    const sketchPlane = plane(hemData.edgeTangentLineAtCenter.origin, hemData.edgeTangentLineAtCenter.direction, hemData.outFromFace);
    const sketch = newSketchOnPlane(context, id, {
                "sketchPlane" : sketchPlane
            });
    // Opposite direction should wrap around the front of the sheet.  Non-opposite direction should wrap around the back of the sheet.
    const wrapAroundFront = definition.oppositeDirection;

    const innerRadius = getInnerRadius(modelParameters, definition);

    var coincidentPointId;
    if (definition.hemType == SMHemType.ROLLED)
    {
        const arcInfo = sketchInitialArc(sketch, wrapAroundFront, modelParameters, innerRadius, definition.angle);
        coincidentPointId = arcInfo.arcStartId;
    }
    else if (definition.hemType == SMHemType.TEAR_DROP)
    {
        const tipGap = definition.useMinimalGap ? modelParameters.minimalClearance : definition.tipGap;

        if (definition.length < 2 * (innerRadius + modelParameters.frontThickness + modelParameters.backThickness) + (TOLERANCE.zeroLength * meter))
            throw "hem too short for teardrop"; // TODO: Error message
        if (tipGap > 2 * innerRadius - TOLERANCE.zeroLength * meter)
            throw "Tip gap too large for teardrop"; // TODO: Error message

        const arcInfo = sketchInitialArc(sketch, wrapAroundFront, modelParameters, innerRadius, 180 * degree);
        const lineInfo = sketchHorizontalLineAfterInitialArc(sketch, modelParameters, definition.length, arcInfo.arcEnd, innerRadius);
        const helperCapInfo = sketchTearDropHelperCap(sketch, modelParameters, lineInfo.lineEnd);
        applyTearDropConstraints(sketch, wrapAroundFront, modelParameters, arcInfo, lineInfo, helperCapInfo, definition.length, innerRadius, tipGap);

        coincidentPointId = arcInfo.arcStartId;
    }
    else if (definition.hemType == SMHemType.STRAIGHT)
    {
        const arcInfo = sketchInitialArc(sketch, wrapAroundFront, modelParameters, innerRadius, 180 * degree);
        sketchHorizontalLineAfterInitialArc(sketch, modelParameters, definition.length, arcInfo.arcEnd, innerRadius);

        coincidentPointId = arcInfo.arcStartId;
    }
    else
    {
        // This cannot be hit by a UI user.
        throwHemTypeError(definition.hemType);
    }
    skSolve(sketch);

    return startTracking(context, sketchEntityQuery(id, EntityType.VERTEX, coincidentPointId));
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

/**
 * Construct bounding planes for the extremes of the hem. `hemData` should be in the format outputted by `getHemData`.
 * @return {{
 *      @field startBoundingPlane {Query} : a construction plane to bound the hem at the start vertex
 *      @field endBoundingPlane {Query} : a construction plane to bound the hem at the end vertex
 */
function constructHemBoundingPlanes(context is Context, id is Id, hemData is map) returns map
{
    // TODO: MITER!
    const startPlaneId = id + "start";
    opPlane(context, startPlaneId, {
                "plane" : plane(hemData.edgeTangentLineAtStart.origin, hemData.edgeTangentLineAtStart.direction)
            });
    const endPlaneId = id + "end";
    opPlane(context, endPlaneId, {
                "plane" : plane(hemData.edgeTangentLineAtEnd.origin, hemData.edgeTangentLineAtEnd.direction)
            });

    return {
            "startBoundingPlane" : qCreatedBy(startPlaneId, EntityType.FACE),
            "endBoundingPlane" : qCreatedBy(endPlaneId, EntityType.FACE)
        };
}

/**
 * Extrude the hem profile to become the hem sheet. Return the edge at which the hem sheet will attach to the master sheet body.
 */
function constructHemSheet(context is Context, id is Id, sketchId is Id, boundingPlanes is map, hemData is map) returns Query
{
    opExtrude(context, id, {
                "entities" : qConstructionFilter(qCreatedBy(sketchId, EntityType.EDGE), ConstructionObject.NO),
                "direction" : hemData.edgeTangentLineAtCenter.direction,
                "startBound" : BoundingType.UP_TO_SURFACE,
                "startBoundEntity" : boundingPlanes.startBoundingPlane,
                "endBound" : BoundingType.UP_TO_SURFACE,
                "endBoundEntity" : boundingPlanes.endBoundingPlane
            });
    return qClosestTo(qNonCapEntity(id, EntityType.EDGE), hemData.edgeTangentLineAtCenter.origin);
}

/**
 * Add the appropriate sheet metal attributes to the hem sheet. `coincidentEdge` is the edge at which the hem sheet will attach to the master
 * sheet body.
 */
function annotateHemSheet(context is Context, topLevelId is Id, coincidentEdge is Query, hemSheetBody is Query, attributeIdCounter is box)
{
    // Each face created by the extrude needs a unique wall id
    for (var face in evaluateQuery(context, qOwnedByBody(hemSheetBody, EntityType.FACE)))
    {
        setAttribute(context, {
                    "entities" : face,
                    "attribute" : makeSMWallAttribute(toAttributeId(topLevelId + attributeIdCounter[]))
                });
        attributeIdCounter[] += 1;
    }

    // The edge that will attach to the master sheet needs a tangent joint
    // TODO: Use dummy joints to hide tangents from the table
    var tangentAttribute = makeSMJointAttribute(toAttributeId(topLevelId + attributeIdCounter[]));
    tangentAttribute.jointType = { "value" : SMJointType.TANGENT, "canBeEdited" : false };
    setAttribute(context, {
                "entities" : coincidentEdge,
                "attribute" : tangentAttribute
            });
    attributeIdCounter[] += 1;

    // Each two sided edge within the hem needs a tangent joint
    for (var edge in evaluateQuery(context, qEdgeTopologyFilter(qOwnedByBody(hemSheetBody, EntityType.EDGE), EdgeTopology.TWO_SIDED)))
    {
        var tangentAttribute = makeSMJointAttribute(toAttributeId(topLevelId + attributeIdCounter[]));
        tangentAttribute.jointType = { "value" : SMJointType.TANGENT, "canBeEdited" : false };
        setAttribute(context, {
                    "entities" : edge,
                    "attribute" : tangentAttribute
                });
        attributeIdCounter[] += 1;
    }

    // TODO: Add appropriate rips once mitering code is written
}

function throwHemTypeError(hemType is SMHemType)
{
    throw "Unrecognized hem type: " ~ hemType;
}

/**
 * @internal
 * Editing logic for the hem feature.
 */
export function hemEditLogic(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean, specifiedParameters is map) returns map
{
    // Keep innerRadius and innerDiameter in sync
    // TODO: Speak with UX about whether this is useful or "too smart"
    if (oldDefinition.innerRadius != definition.innerRadius && specifiedParameters.innerRadius)
    {
        definition.innerDiameter = 2 * definition.innerRadius;
    }
    if (oldDefinition.innerDiameter != definition.innerDiameter && specifiedParameters.innerDiameter)
    {
        definition.innerRadius = definition.innerDiameter / 2;
    }

    // Lock tipGap to match innerRadius until the user specifies a tip gap
    if (isCreating && oldDefinition.innerRadius != definition.innerRadius && specifiedParameters.innerRadius && !specifiedParameters.tipGap)
    {
        definition.tipGap = definition.innerRadius;
    }

    return definition;
}

