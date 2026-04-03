FeatureScript 2931; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/containers.fs", version : "2931.0");
import(path : "onshape/std/coordSystem.fs", version : "2931.0");
import(path : "onshape/std/curveGeometry.fs", version : "2931.0");
import(path : "onshape/std/debug.fs", version : "2931.0");
import(path : "onshape/std/evaluate.fs", version : "2931.0");
import(path : "onshape/std/feature.fs", version : "2931.0");
import(path : "onshape/std/manipulator.fs", version : "2931.0");
import(path : "onshape/std/math.fs", version : "2931.0");
import(path : "onshape/std/matrix.fs", version : "2931.0");
import(path : "onshape/std/path.fs", version : "2931.0");
import(path : "onshape/std/splineUtils.fs", version : "2931.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2931.0");
import(path : "onshape/std/valueBounds.fs", version : "2931.0");
import(path : "onshape/std/vector.fs", version : "2931.0");
import(path : "onshape/std/nurbsUtils.fs", version : "2931.0");
import(path : "onshape/std/approximationUtils.fs", version : "2931.0");

/**
 * An `IntegerBoundSpec` for control point indices.
 */
export const CONTROL_POINT_INDEX_BOUND =
{
            (unitless) : [0, 0, MAX_CONTROL_POINTS - 1]
        } as IntegerBoundSpec;

/**
 * Reference plane enum for planarization
 */
export enum PlaneReference
{
    annotation { "Name" : "Best fit" }
    BEST,
    annotation { "Name" : "YZ plane" }
    YZPLANE,
    annotation { "Name" : "XZ plane" }
    XZPLANE,
    annotation { "Name" : "XY plane" }
    XYPLANE,
    annotation { "Name" : "Custom" }
    CUSTOM
}

/**
 * Edit point mode
 */
export enum EditPointMode
{
    annotation { "Name" : "XYZ" }
    XYZ,
    annotation { "Name" : "UVN (Tangent)" }
    UVN_TANGENT,
    annotation { "Name" : "UVN (Control polygon)" }
    UVN_CONTROL_POLYGON
}

const INDEX_MANIPULATOR = "indexManipulator";
const INDICES_MANIPULATOR = "indicesManipulator";
const OFFSET_MANIPULATOR = "offsetManipulator";

const U_TANGENT_MANIPULATOR = "UTangentManipulator";
const V_TANGENT_MANIPULATOR = "VTangentManipulator";
const N_MANIPULATOR = "NTangentManipulator";
const U1_CONTROL_POLYGON_MANIPULATOR = "U1ControlPolygonManipulator";
const U2_CONTROL_POLYGON_MANIPULATOR = "U2ControlPolygonManipulator";
const V_CONTROL_POLYGON_MANIPULATOR = "VControlPolygonManipulator";
const LINEAR_MANIPULATORS = [U_TANGENT_MANIPULATOR, V_TANGENT_MANIPULATOR, N_MANIPULATOR, U1_CONTROL_POLYGON_MANIPULATOR, U2_CONTROL_POLYGON_MANIPULATOR, V_CONTROL_POLYGON_MANIPULATOR];

/**
 * A curve editing feature.
 */
annotation { "Feature Type Name" : "Edit curve",
        "Feature Type Description" : "Edits a curve directly if possible or a composite curve of the selection otherwise.",
        "Editing Logic Function" : "editCurveEditLogic",
        "Manipulator Change Function" : "onEditCurveManipulatorChange" }
export const editCurve = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Curves", "Filter" : EntityType.EDGE || (EntityType.BODY && BodyType.WIRE && SketchObject.NO) }
        definition.wire is Query;

        annotation { "Name" : "Approximate" }
        definition.approximate is boolean;
        annotation { "Group Name" : "Approximation parameters", "Driving Parameter" : "approximate", "Collapsed By Default" : false }
        {
            if (definition.approximate)
            {
                annotation { "Name" : "Target degree", "Column Name" : "Approximation target degree" }
                isInteger(definition.approximationDegree, DEGREE_BOUND);

                annotation { "Name" : "Maximum control points" }
                isInteger(definition.approximationMaxCPs, { (unitless) : [4, 15, MAX_CONTROL_POINTS] } as IntegerBoundSpec);

                annotation { "Name" : "Tolerance" }
                isLength(definition.approximationTolerance, TOLERANCE_BOUND);

                annotation { "Name" : "Keep start derivative" }
                definition.keepStartDerivative is boolean;

                annotation { "Name" : "Keep end derivative" }
                definition.keepEndDerivative is boolean;
            }
        }

        annotation { "Name" : "Elevate" }
        definition.elevate is boolean;
        annotation { "Group Name" : "Elevation parameters", "Driving Parameter" : "elevate", "Collapsed By Default" : false }
        {
            if (definition.elevate)
            {
                annotation { "Name" : "Target degree", "Column Name" : "Elevation target degree" }
                isInteger(definition.elevationDegree, DEGREE_BOUND);
            }
        }

        annotation { "Name" : "Planarize" }
        definition.planarize is boolean;
        annotation { "Group Name" : "Planarization parameters", "Driving Parameter" : "planarize", "Collapsed By Default" : false }
        {
            if (definition.planarize)
            {
                annotation { "Name" : "Reference plane", "Default" : PlaneReference.BEST }
                definition.planeReference is PlaneReference;

                if (definition.planeReference == PlaneReference.CUSTOM)
                {
                    annotation { "Name" : "Entity or mate connector", "Filter" : QueryFilterCompound.ALLOWS_PLANE, "MaxNumberOfPicks" : 1 }
                    definition.referencePlane is Query;
                }
                else if (definition.planeReference == PlaneReference.BEST)
                {
                    annotation { "Name" : "Lock ends" }
                    definition.lockEnds is boolean;
                }
            }
        }

        annotation { "Name" : "Edit control points" }
        definition.editControlPoints is boolean;
        annotation { "Group Name" : "Edit control points", "Driving Parameter" : "editControlPoints", "Collapsed By Default" : false }
        {
            if (definition.editControlPoints)
            {
                annotation { "Name" : "Edit mode" }
                definition.editPointMode is EditPointMode;

                if (definition.editPointMode == EditPointMode.XYZ)
                {
                    annotation { "Name" : "Control point indices", "Item name" : "index", "Item label template" : "Control point index #indexValue", "Show labels only" : true, "UIHint" : [UIHint.INITIAL_FOCUS, UIHint.PREVENT_ARRAY_REORDER, UIHint.ALLOW_ARRAY_FOCUS] }
                    definition.selectedIndices is array;
                    for (var selectedIndex in definition.selectedIndices)
                    {
                        annotation { "Name" : "Control point index" }
                        isInteger(selectedIndex.indexValue, CONTROL_POINT_INDEX_BOUND);
                    }
                }
                else
                {
                    annotation { "Name" : "Control point index" }
                    isInteger(definition.selectedIndex, CONTROL_POINT_INDEX_BOUND);
                }

                annotation { "Name" : "Points overrides", "Item name" : "point override", "Item label template" : "#index: #x;#y;#z", "UIHint" : [UIHint.PREVENT_ARRAY_REORDER, UIHint.COLLAPSE_ARRAY_ITEMS] }
                definition.controlPointEdits is array;
                for (var controlPointEdit in definition.controlPointEdits)
                {
                    annotation { "Name" : "Control point index" }
                    isInteger(controlPointEdit.index, CONTROL_POINT_INDEX_BOUND);

                    annotation { "Name" : "Reference", "Filter" : QueryFilterCompound.ALLOWS_VERTEX, "MaxNumberOfPicks" : 1 }
                    controlPointEdit.reference is Query;

                    annotation { "Name" : "X offset" }
                    isLength(controlPointEdit.x, ZERO_DEFAULT_LENGTH_BOUNDS);
                    annotation { "Name" : "Y offset" }
                    isLength(controlPointEdit.y, ZERO_DEFAULT_LENGTH_BOUNDS);
                    annotation { "Name" : "Z offset" }
                    isLength(controlPointEdit.z, ZERO_DEFAULT_LENGTH_BOUNDS);

                    annotation { "Name" : "Weight" }
                    isReal(controlPointEdit.weight, SCALE_BOUNDS);
                }
            }
        }

        annotation { "Name" : "Show details", "Default" : true }
        definition.showDetails is boolean;
        annotation { "Group Name" : "Show details", "Driving Parameter" : "showDetails", "Collapsed By Default" : false }
        {
            if (definition.showDetails)
            {
                annotation { "Name" : "Degree", "UIHint" : UIHint.READ_ONLY }
                isInteger(definition.curveDegree, { (unitless) : [0, 0, MAX_DEGREE] } as IntegerBoundSpec);

                annotation { "Name" : "Control points", "UIHint" : UIHint.READ_ONLY }
                isInteger(definition.curveNumCPs, CONTROL_POINT_INDEX_BOUND);

                annotation { "Name" : "Spans", "UIHint" : UIHint.READ_ONLY }
                isInteger(definition.curveNumSpans, CONTROL_POINT_INDEX_BOUND);
            }
        }

        annotation { "Name" : "Show deviation" }
        definition.approximationShowDeviation is boolean;
        annotation { "Group Name" : "Show deviation", "Driving Parameter" : "approximationShowDeviation", "Collapsed By Default" : false }
        {
            if (definition.approximationShowDeviation)
            {
                annotation { "Name" : "Maximum deviation", "UIHint" : UIHint.READ_ONLY }
                isLength(definition.maxDeviation, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
            }
        }
    }
    {
        // BEL-234151: This is necessary to check for self-intersecting sketch curves.
        // The issue is that evaluating a query with a self intersecting curve in FS simply removes it.
        // So in order to detect those, we have to make a call to the server (which here is done with opExtractWires)
        // and see if that fails. If the error is EXTRACT_WIRES_NEEDS_EDGES, it means we have no selection at all, so
        // we fail with the Edit curve-specific error, otherwise we just bubble up the opExtractWires error.
        var queryToReplace;
        try silent
        {
            queryToReplace = getQueryToReplace(context, id, definition);
        }
        catch (error)
        {
            const message = try(error.message as ErrorStringEnum);
            if (message == ErrorStringEnum.EXTRACT_WIRES_NEEDS_EDGES)
            {
                throw regenError(ErrorStringEnum.EDIT_CURVE_SELECT_WIRE, ["wire"]);
            }
            throw error;
        }

        var bspline = getBSplineFromInput(context, definition);

        if (definition.elevate)
        {
            bspline = elevate(context, id, bspline, definition.elevationDegree);
        }
        if (definition.planarize && !(definition.planeReference == PlaneReference.BEST && isPlanar(context, id, bspline)))
        {
            const plane = findPlane(context, definition, bspline);
            bspline = planarize(bspline, plane);
        }

        if (definition.editControlPoints)
        {
            const bsplineBeforeEdits = bspline;
            bspline = editControlPoints(context, id, bspline, definition.controlPointEdits);
            if (definition.editPointMode == EditPointMode.XYZ)
            {
                showXYZEditManipulator(context, id, definition, bspline.controlPoints, bspline.editToIndex);
            }
            else if (definition.editPointMode == EditPointMode.UVN_TANGENT)
            {
                showUVNTangentEditManipulators(context, id, bsplineBeforeEdits, bspline.controlPoints, definition.selectedIndex);
            }
            else
            {
                showUVNControlPolygonEditManipulators(context, id, bsplineBeforeEdits, bspline.controlPoints, definition.selectedIndex);
            }
        }
        showIndexManipulators(context, id, bspline.controlPoints, definition);

        showPolyline(context, bspline);
        updateCurveData(context, id, bspline);

        // This is necessary to add control point overlaps in case of knot overlaps
        bspline = bSplineCurve({
                    "degree" : bspline.degree,
                    "isPeriodic" : bspline.isPeriodic,
                    "controlPoints" : bspline.controlPoints,
                    "knots" : bspline.knots,
                    "weights" : bspline.weights });

        opCreateBSplineCurve(context, id + "bSplineCurve", {
                    "bSplineCurve" : bspline
                });
        if (definition.approximationShowDeviation)
        {
            const maxDeviationResult = evMaxPathDeviation(context, {
                        "side1" : qCreatedBy(id + "bSplineCurve", EntityType.EDGE),
                        "side2" : queryToReplace,
                        "showDeviation" : true });

            setFeatureComputedParameter(context, id, { "name" : "maxDeviation", "value" : maxDeviationResult.deviation });
        }
        opEditCurve(context, id + "editCurve", {
                    "wire" : queryToReplace,
                    "edge" : qCreatedBy(id + "bSplineCurve", EntityType.EDGE),
                    "showCurves" : true
                });
        opDeleteBodies(context, id + "deleteBVSplineCurve", {
                    "entities" : qCreatedBy(id + "bSplineCurve", EntityType.BODY)
                });
    }, { "approximate" : false, "elevate" : false, "planarize" : false,
            "editControlPoints" : false, "showDetails" : false, "approximationShowDeviation" : false,
            "editPointMode" : EditPointMode.XYZ, "selectedIndices" : [] });

//==================================================================
//======================== Input Processing ========================
//==================================================================

function inputCanBeModified(context is Context, wire is Query) returns boolean
{
    const allBodiesQuery = qOwnerBody(wire);
    const allBodiesEdgesQuery = qOwnedByBody(allBodiesQuery, EntityType.EDGE);
    const edgesQuery = qEntityFilter(wire, EntityType.EDGE);
    const bodiesQuery = qEntityFilter(wire, EntityType.BODY);
    const bodiesEdgesQuery = qOwnedByBody(bodiesQuery, EntityType.EDGE);
    const allEdgesQuery = qUnion([edgesQuery, bodiesEdgesQuery]);
    const nonModifiableSelections = qSubtraction(wire, qModifiableEntityFilter(wire));

    // We can only use the raw input if:
    // - all the inputs come from a single body,
    // - all the edges of said body have been selected,
    // - the body is a wire,
    // - the body is not a sketch body.
    // - all the selections are not from in-context entities

    const singleBody = size(evaluateQuery(context, allBodiesQuery)) == 1;
    const allEdgesSelected = size(evaluateQuery(context, allBodiesEdgesQuery)) == size(evaluateQuery(context, allEdgesQuery));
    const isWireBody = !isQueryEmpty(context, qBodyType(allBodiesQuery, BodyType.WIRE));
    const isNotSketchBody = isQueryEmpty(context, qSketchFilter(allBodiesQuery, SketchObject.YES));
    const allEdgesNotInContext = isQueryEmpty(context, nonModifiableSelections);

    return singleBody && allEdgesSelected && isWireBody && isNotSketchBody && allEdgesNotInContext;
}

function getQueryToReplace(context is Context, id is Id, definition is map) returns Query
{
    if (inputCanBeModified(context, definition.wire))
    {
        return definition.wire;
    }

    opExtractWires(context, id + "opExtractWires", {
                "edges" : getAllEdgesQuery(definition.wire)
            });

    return qCreatedBy(id + "opExtractWires", EntityType.BODY);
}

function checkBSpline(bspline is BSplineCurve, wire is Query)
{
    if (bspline.degree > MAX_DEGREE)
    {
        throw regenError(ErrorStringEnum.EDIT_CURVE_DEGREE_TOO_HIGH, ["wire"], wire);
    }
    if (size(bspline.controlPoints) > MAX_CONTROL_POINTS)
    {
        throw regenError(ErrorStringEnum.EDIT_CURVE_TOO_MANY_CONTROL_POINTS, ["wire"], wire);
    }
}

function getBSplineFromInput(context is Context, definition is map) returns map
{
    var bspline;
    const edgesQuery = getAllEdgesQuery(definition.wire);
    if (definition.approximate)
    {
        var path;
        try silent
        {
            path = constructPath(context, edgesQuery, { "tolerance" : 1e-5 * meter }).path;
        }
        catch (error)
        {
            throw regenError(error, ["wire"], definition.wire);
        }

        checkApproximationParameters(definition, path);

        const approximationTarget = makeApproximationTarget(context, path, definition.keepStartDerivative, definition.keepEndDerivative);

        bspline = approximateSpline(context, {
                        "degree" : definition.approximationDegree,
                        "tolerance" : definition.approximationTolerance,
                        "isPeriodic" : path.closed,
                        "targets" : [approximationTarget],
                        "maxControlPoints" : definition.approximationMaxCPs
                    })[0];
    }
    else
    {
        const edges = evaluateQuery(context, edgesQuery);
        if (size(edges) > 1)
        {
            throw regenError(ErrorStringEnum.EDIT_CURVE_MULTIPLE_EDGES, ["wire"], definition.wire);
        }
        const edge = edges[0];
        const curveDef = evCurveDefinition(context, {
                    "edge" : edge,
                    "simplify" : true
                });
        if (curveDef is Line)
        {
            const edgeVertices = evaluateQuery(context, qAdjacent(edge, AdjacencyType.VERTEX, EntityType.VERTEX));
            bspline = bSplineCurve({
                        "degree" : 1,
                        "isPeriodic" : false,
                        "controlPoints" : [evVertexPoint(context, { "vertex" : edgeVertices[0] }), evVertexPoint(context, { "vertex" : edgeVertices[1] })]
                    });
        }
        else if (curveDef is BSplineCurve)
        {
            bspline = curveDef;
            checkBSpline(bspline, definition.wire);
        }
        else
        {
            bspline = evApproximateBSplineCurve(context, {
                        "edge" : edge
                    });
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2554_EDIT_CURVE_CHECK_BSPLINE_APPROXIMATION))
            {
                checkBSpline(bspline, definition.wire);
            }
        }
    }
    // Since weights can be modified, it's either to make every curve rational and default the weights to all 1s.
    if (!bspline.isRational)
    {
        bspline.weights = makeArray(size(bspline.controlPoints), 1);
        bspline.isRational = true;
    }
    return cleanUpPeriodicBSplineDefinition(bspline);
}

// There are a few ways that periodic NURBS are handled, the can have no overlaps, overlapping knots or onverlapping knots and control points.
// For our purposes, if a curve has overlapping knots and overlapping control points, we remove the overlapping control points.
function cleanUpPeriodicBSplineDefinition(bspline is map) returns map
{
    if (!bspline.isPeriodic || bspline.knots[0] == 0 || size(bspline.controlPoints) + 2 * bspline.degree + 1 == size(bspline.knots))
    {
        return bspline;
    }
    const numOverlappingKnots = indexOf(bspline.knots, 0);
    // In certain cases, we get periodic curves with only the first control point overlapping and a single knot overlap.
    // For our bspline creation code this is the same as having no overlapping knots so we clamp the knot vector to [0;1] to avoid issues with elevation.
    if (numOverlappingKnots == 1)
    {
        bspline.knots[0] = 0;
        bspline.knots[size(bspline.knots) - 1] = 1;
        return bspline;
    }
    // If we're here, we have repeated knots AND repeated control points. We only want the knots.
    const lastIndex = size(bspline.controlPoints) - bspline.degree;
    bspline.controlPoints = subArray(bspline.controlPoints, 0, lastIndex);
    if (bspline.weights != undefined)
    {
        bspline.weights = subArray(bspline.weights, 0, lastIndex);
    }
    return bspline;
}

//==================================================================
//=========================== Elevation ============================
//==================================================================

function elevate(context is Context, id is Id, bspline is map, targetDegree is number) returns map
{
    if (bspline.degree >= targetDegree)
    {
        reportFeatureInfo(context, id, "Curve degree is already equal or above elevation target degree.");
    }
    else
    {
        bspline = elevateDegree(bspline, targetDegree);
    }
    return bspline;
}

function subdivideIntoBeziers(points is array, knots is array, curveDegree is number) returns array
{
    var numSplits = 0;
    for (var i = curveDegree + 1; i < size(knots) - curveDegree - 1; i += 1)
    {
        if (knots[i] != knots[i + 1])
        {
            numSplits += 1;
        }
    }
    const overlappingKnots = knots[0] < 0;
    if (overlappingKnots)
    {
        numSplits += 2;
        for (var i = 0; i < curveDegree + 2; i += 1)
        {
            points = append(points, points[i]);
        }
    }
    var currentKnots = knots;
    var currentPoints = points;
    var beziers = makeArray(numSplits + 1);

    for (var i = 0; i < numSplits; i += 1)
    {
        const bezierAndSpline = splitAtFirstKnot(currentPoints, currentKnots, curveDegree);
        beziers[i] = bezierAndSpline.bezier;
        currentPoints = bezierAndSpline.bspline;
        currentKnots = bezierAndSpline.knots;
    }
    beziers[numSplits] = currentPoints;
    if (overlappingKnots)
    {
        beziers = subArray(beziers, 1, size(beziers) - 1);
    }
    return beziers;
}

// Returns the first bezier subdivision and the rest of the curve.
// We apply DeBoor's algorithm, which gives the segment subdivision of the bspline.
// The Bezier points are the first points of each level of segmentation.
// The last points of each level of segmentation are prepended to the bspline.
// See https://doi.org/10.1007/978-3-642-59223-2
function splitAtFirstKnot(points is array, knots is array, curveDegree is number) returns map
{
    if (size(points) == curveDegree + 1)
    {
        // This is a Bezier curve, no need to do anything.
        return {};
    }
    // first knot's index (k) is d + 1
    var k = curveDegree + 1;
    // Value of first knot
    const u = knots[k];
    // multiplicity of the knot
    var s = 1;
    for (var i = k + 1; i < size(knots); i += 1)
    {
        if (knots[i] != knots[k])
        {
            break;
        }
        s += 1;
        k += 1;
    }
    // De boor
    const h = curveDegree - s;
    var result = makeArray(h + 1);
    result[0] = subArray(points, k - curveDegree, k - s + 1);
    for (var r = 1; r <= h; r += 1)
    {
        result[r] = makeArray(curveDegree - s - r + 1);
        for (var i = 0; i <= curveDegree - s - r; i += 1)
        {
            const knotIndex = i + k - curveDegree + r;
            const alpha = (u - knots[knotIndex]) / (knots[knotIndex + curveDegree - r + 1] - knots[knotIndex]);
            result[r][i] = (1 - alpha) * result[r - 1][i] + alpha * result[r - 1][i + 1];
        }
    }
    // Extracting the bezier points from the segmentation
    const bezierPointsBeforeDeBoor = subArray(points, 0, k - curveDegree);
    const bsplinePointsAfterDeBoor = subArray(points, k - s + 1, size(points));
    var bezierPointsInDeBoor = makeArray(h + 1);
    var bsplinePontsInDeBoor = makeArray(h + 1);
    for (var r = 0; r <= h; r += 1)
    {
        bezierPointsInDeBoor[r] = result[r][0];
        bsplinePontsInDeBoor[r] = result[h - r][size(result[h - r]) - 1];
    }
    const bezier = concatenateArrays([bezierPointsBeforeDeBoor, bezierPointsInDeBoor]);
    const bspline = concatenateArrays([bsplinePontsInDeBoor, bsplinePointsAfterDeBoor]);
    const newKnots = concatenateArrays([makeArray(curveDegree + 1, u), subArray(knots, k + 1, size(knots))]);
    return { "bezier" : bezier, "bspline" : bspline, "knots" : newKnots };
}

function elevateDegree(bspline is map, newDegree is number) returns map
{
    const weightedPoints = combinePointsAndWeights(bspline.controlPoints, bspline.weights);

    var newPoints;
    if (isBezier(bspline.controlPoints, bspline.degree, bspline.knots))
    {
        newPoints = elevateBezierDegree(weightedPoints, newDegree);
        bspline.knots = makeUniformKnotArray(newDegree, size(newPoints), false);
    }
    else
    {
        const pointsAndKnots = elevateBSpline(weightedPoints, bspline.knots, bspline.degree, newDegree);
        newPoints = pointsAndKnots.points;
        bspline.knots = pointsAndKnots.knots as KnotArray;
    }

    const pointsAndWeights = separatePointsAndWeights(newPoints);
    bspline.controlPoints = pointsAndWeights.points;
    bspline.weights = pointsAndWeights.weights;
    bspline.degree = newDegree;

    return bspline;
}

function elevateBSpline(originalPoints is array, originalKnots is array, originalDegree is number, newDegree is number) returns map
{
    // First we subdivide the bspline into bezier curves
    var beziers = subdivideIntoBeziers(originalPoints, originalKnots, originalDegree);

    // Then we elevate each bezier curve separately
    for (var i = 0; i < size(beziers); i += 1)
    {
        beziers[i] = elevateBezierDegree(beziers[i], newDegree);
    }
    // Then we combine the beziers into one bspline
    var points = [beziers[0][0]];
    for (var i = 0; i < size(beziers); i += 1)
    {
        for (var j = 1; j < size(beziers[i]); j += 1)
        {
            points = append(points, beziers[i][j]);
        }
    }
    // We make the corresponding knot vector, which is the same knot vector but with added multiplicity
    const lastKnot = originalKnots[size(originalKnots) - originalDegree - 1];
    var i = originalDegree + 1;
    var currentKnot = originalKnots[i];
    var newKnots = makeArray(newDegree + 1, originalKnots[i - 1]);
    while (currentKnot != lastKnot)
    {
        newKnots = concatenateArrays([newKnots, makeArray(newDegree, currentKnot)]);
        // We skip identical knots
        while (originalKnots[i] == currentKnot)
        {
            i += 1;
        }
        currentKnot = originalKnots[i];
    }
    newKnots = concatenateArrays([newKnots, makeArray(newDegree + 1, lastKnot)]);
    // Then we simplify
    return removeKnots(points, newKnots, newDegree);
}

//==================================================================
//========================= Planarization ==========================
//==================================================================

function planarize(bspline is map, plane is Plane) returns map
{
    for (var i = 0; i < size(bspline.controlPoints); i += 1)
    {
        bspline.controlPoints[i] = project(plane, bspline.controlPoints[i]);
    }
    return bspline;
}

function findPlane(context is Context, definition is map, bspline is map) returns Plane
{
    var planeResult;
    if (definition.planeReference == PlaneReference.XYPLANE)
    {
        planeResult = XY_PLANE;
    }
    else if (definition.planeReference == PlaneReference.YZPLANE)
    {
        planeResult = YZ_PLANE;
    }
    else if (definition.planeReference == PlaneReference.XZPLANE)
    {
        planeResult = XZ_PLANE;
    }
    else if (definition.planeReference == PlaneReference.CUSTOM)
    {
        if (isQueryEmpty(context, definition.referencePlane))
        {
            throw regenError(ErrorStringEnum.EDIT_CURVE_SELECT_PLANE, ["referencePlane"]);
        }
        planeResult = evPlane(context, {
                    "face" : definition.referencePlane
                });
    }
    else if (definition.planeReference == PlaneReference.BEST)
    {
        if (!definition.lockEnds)
        {
            planeResult = fitPlane(bspline.controlPoints);
        }
        else
        {
            if (bspline.isPeriodic)
            {
                throw regenError(ErrorStringEnum.EDIT_CURVE_LOCK_ENDS_PERIODIC, ["lockEnds", "wire"]);
            }
            planeResult = fitPlaneKeepEnds(bspline.controlPoints);
        }
        if (tolerantEqualsZero(norm(planeResult.normal)))
        {
            throw regenError(ErrorStringEnum.EDIT_CURVE_NO_BEST_FIT, ["wire"]);
        }
    }
    return planeResult;
}

function isPlanar(context is Context, id is Id, bspline is map) returns boolean
{
    var planar = false;
    const planarId = id + "PlanarCheck";
    startFeature(context, planarId, {});
    opCreateBSplineCurve(context, planarId + "bSplineCurve", {
                "bSplineCurve" : bSplineCurve(bspline)
            });
    try silent
    {
        evPlanarEdge(context, {
                    "edge" : qCreatedBy(planarId + "bSplineCurve", EntityType.EDGE)
                });
        // evPlanarEdge throws if the edge is not planar so if we reach this point the edge is planar.
        planar = true;
    }
    abortFeature(context, planarId);
    return planar;
}

// Many thanks to Michael Lauer for fitPlaneKeepEnds and fitPlane
function fitPlaneKeepEnds(points is array) returns Plane
{
    /**
     * Let A be the unit vector from the first to the last pointsand O be the plane's origin (i.e. any point on the line).
     * Let the set of points you're trying to fit be { P_i }, and V_i = P_i - O
     *
     * We want to find a unit vector N perpendicular to A that minimizes \Sum_i (N · V_i) ^ 2
     * So pick unit vectors B and C perpendicular to A and to each other. You can write N as n_0 B + n_1 C for a two-dimensional unit vector (n_0, n_1).
     *
     * Plugging that in to the thing you're minimizing you get: \Sum_i (B·V_i n_0 + C·V_i n_1)^2 = n^t M n
     *      where M is the two dimensional matrix:
     *          \Sum (B·V_i)^2         \Sum (B·V_i) (C·V_i)
     *          \Sum (B·V_i) (C·V_i)   \Sum (C·V_i)^2
     *
     * Since the 2d vector n is constrained n·n = 1, we can do the constrained minimization with lagrange multipliers, and find
     *    M n = λ n
     * so n is an eigenvector of M - the one with the smallest eigenvalue.
     */
    const numPoints = size(points);
    if (numPoints == 0)
    {
        throw regenError(ErrorStringEnum.CANNOT_RESOLVE_PLANE);
    }
    const firstPoint = points[0];
    var Vis = makeArray(numPoints - 2);
    for (var i = 1; i < numPoints - 1; i += 1)
    {
        Vis[i - 1] = (points[i] - firstPoint) / meter;
    }

    const A = normalize(points[numPoints - 1] - firstPoint);
    var B = zeroVector(3);
    for (var Vi in Vis)
    {
        B = cross(A, Vi);
        if (norm(B) != 0)
        {
            B = normalize(B);
            break;
        }
    }
    if (tolerantEqualsZero(norm(B)))
    {
        throw regenError(ErrorStringEnum.EDIT_CURVE_NO_BEST_FIT, ["wire"]);
    }
    const C = cross(A, B);
    var m00 = 0;
    var m10 = 0;
    var m11 = 0;
    for (var Vi in Vis)
    {
        const BVi = dot(B, Vi);
        const CVi = dot(C, Vi);
        m00 += BVi * BVi;
        m10 += BVi * CVi;
        m11 += CVi * CVi;
    }
    const M = matrix([[m00, m10], [m10, m11]]);
    const svdResult = svd(M);
    const n = B * svdResult.u[1][0] + C * svdResult.u[1][1];
    return plane(firstPoint, n);
}

function fitPlane(points is array) returns Plane
{
    /**
     * We want to find a plane that minimizes the sum of the squares of the distances of
     * the points to the plane. That is, if n is the plane's normal and o is its origin, we're minimizing
     *
     * \sum_p (n · (p - o))^2
     *
     * This is minimized w.r.t. o when o is the center of mass of the points
     *
     * To minimize w.r.t. to n, remember that n · n = 1, and use Lagrange multipliers
     *
     * \sum_p ( 2 (p - o) (p - o) · n = λ 2 n
     *
     * Which is to say, that the solution is an eigenvector of the 3x3 matrix
     *
     * \sum_p (p - o) (p - o)^t
     *
     * In fact it's the eigenvector with the smallest eigenvalue.
     * We can find this directly from the svd of the matrix.
     */
    const numPoints = size(points);
    if (numPoints == 0)
    {
        throw regenError(ErrorStringEnum.CANNOT_RESOLVE_PLANE);
    }
    // First find the center of mass
    var center = vector(0, 0, 0) * meter;
    for (var pt in points)
    {
        center = center + pt;
    }
    center = center / numPoints;

    // Now accumulate the matrix
    var m = zeroMatrix(3, 3);
    for (var pt in points)
    {
        var unitless = (pt - center) / meter;
        var oneDMatrix = matrix([unitless]);
        var matrixComponent = transpose(oneDMatrix) * oneDMatrix;
        m = m + matrixComponent;
    }
    var svdResult = svd(m);
    // Assuming the singular values are in order with the smallest one last
    var ut = transpose(svdResult.u);
    return plane(center, ut[2] as Vector, ut[1] as Vector);
}

//==================================================================
//========================== Control point edit ===========================
//==================================================================

function editControlPoints(context is Context, id is Id, bspline is map, controlPointEdits is array) returns map
{
    const numCPs = size(bspline.controlPoints);
    var editedControlPoints = {};
    // Use for the XYZ manipulator to avoid going through the edits array multiple times.
    bspline.editToIndex = {};
    for (var i = 0; i < size(controlPointEdits); i += 1)
    {
        const controlPointEdit = controlPointEdits[i];
        if (controlPointEdit.index >= numCPs)
        {
            throw regenError("Index " ~ controlPointEdit.index ~ " of control point edit is out of bounds.", ["controlPointEdits"]);
        }
        if (editedControlPoints[controlPointEdit.index] == true)
        {
            throw regenError("Multiple edits targeting control point " ~ controlPointEdit.index, ["controlPointEdits"]);
        }
        editedControlPoints[controlPointEdit.index] = true;
        var point = bspline.controlPoints[controlPointEdit.index];
        if (!isQueryEmpty(context, controlPointEdit.reference))
        {
            point = evVertexPoint(context, {
                        "vertex" : controlPointEdit.reference
                    });
        }
        const offset = [controlPointEdit.x, controlPointEdit.y, controlPointEdit.z] as Vector;
        bspline.controlPoints[controlPointEdit.index] = point + offset;
        bspline.weights[controlPointEdit.index] = controlPointEdit.weight;
        bspline.editToIndex[controlPointEdit.index] = i;
    }
    return bspline;
}

//==================================================================
//========================== Manipulators ==========================
//==================================================================

function showXYZEditManipulator(context is Context, id is Id, definition is map, controlPoints is array, editToIndex is map)
{
    if (!indicesAreValid(context, id, definition.selectedIndices, controlPoints))
    {
        return;
    }
    var base = WORLD_ORIGIN;
    var offset = WORLD_ORIGIN;
    const numCPs = size(controlPoints);

    const numSelections = size(definition.selectedIndices);
    if (numSelections == 0)
    {
        return;
    }
    var seenIndices = {};
    for (var i = 0; i < numSelections; i += 1)
    {
        const selectedIndex = definition.selectedIndices[i].indexValue;
        if (seenIndices[selectedIndex] == true)
        {
            reportFeatureWarning(context, id, "Control point " ~ selectedIndex ~ " selected mulitple times", ["selectedIndices"]);
            return;
        }
        seenIndices[selectedIndex] = true;
        var currentOffset = WORLD_ORIGIN;
        if (editToIndex[selectedIndex] != undefined)
        {
            const edit = definition.controlPointEdits[editToIndex[selectedIndex]];
            currentOffset = [edit.x, edit.y, edit.z] as Vector;
        }
        offset += currentOffset;
        base += controlPoints[selectedIndex] - currentOffset;
    }

    base /= numSelections;
    offset /= numSelections;

    const triadManipulator = triadManipulator({
                "base" : base,
                "offset" : offset
            });
    addManipulators(context, id, {
                (OFFSET_MANIPULATOR) : triadManipulator
            });
}

function showUVNTangentEditManipulators(context is Context, id is Id, bsplineBeforeEdit is map, points is array, selectedIndex is number)
{
    if (!indexIsValid(context, id, selectedIndex, points))
    {
        return;
    }
    // The UVN tangent frame is defined as such:
    // U: tangent vector to the bspline curve at the point where the selected control point has the greatest influence.
    // V: normal vector at the same point.
    // N: binormal vector at the same point.
    // The frame is computed on the bspline before any per-point edit is made to keep the frame consistent when editing.
    const frame = getCurvatureFrameAtControlPointIndex(context, id, selectedIndex, bsplineBeforeEdit);

    const base = points[selectedIndex];

    const uManipulator = linearManipulator({
                "base" : base,
                "direction" : frame.zAxis,
                "offset" : 0 * meter
            });
    addManipulators(context, id, {
                (U_TANGENT_MANIPULATOR) : uManipulator
            });

    const vManipulator = linearManipulator({
                "base" : base,
                "direction" : frame.xAxis,
                "offset" : 0 * meter
            });
    addManipulators(context, id, {
                (V_TANGENT_MANIPULATOR) : vManipulator
            });

    const nManipulator = linearManipulator({
                "base" : base,
                "direction" : cross(frame.xAxis, frame.zAxis),
                "offset" : 0 * meter
            });
    addManipulators(context, id, {
                (N_MANIPULATOR) : nManipulator
            });
}

function showUVNControlPolygonEditManipulators(context is Context, id is Id, bsplineBeforeEdit is map, points is array, selectedIndex is number)
{
    if (!indexIsValid(context, id, selectedIndex, points))
    {
        return;
    }
    // The UVN control polygon frame is defined as such:
    // U1: the direction between the current control point and the previous control point, if available.
    // U2: the direction between the current control point and the next control point, if available.
    // N: the normal direction the curve as defined in tangent frame.
    // V: the direction orthogonal to the normal and the direction between the previous and next control points if they are available,
    //    or between the current control point and the previous/next one for the ends.
    const base = points[selectedIndex];

    // We use the same N for tangent and control polygon
    const frame = getCurvatureFrameAtControlPointIndex(context, id, selectedIndex, bsplineBeforeEdit);

    const nDirection = cross(frame.xAxis, frame.zAxis);
    const nManipulator = linearManipulator({
                "base" : base,
                "direction" : nDirection,
                "offset" : 0 * meter
            });
    addManipulators(context, id, {
                (N_MANIPULATOR) : nManipulator
            });

    var numCP = size(points);
    // If the bspline is periodic, we prepend and append the endpoints for ease of computation.
    // This only makes sense if we have overlaps built into the knot vector, i.e. if the knot vector goes past [0,1], so we also check for this.
    if (bsplineBeforeEdit.isPeriodic && bsplineBeforeEdit.knots[0] < 0)
    {
        points = concatenateArrays([[points[numCP - 1]], points, [points[0]]]);
        numCP = size(points);
        selectedIndex += 1;
    }
    // U1
    if (selectedIndex != 0)
    {
        const u1Manipulator = linearManipulator({
                    "base" : base,
                    "direction" : normalize(points[selectedIndex - 1] - points[selectedIndex]),
                    "offset" : 0 * meter
                });
        addManipulators(context, id, {
                    (U1_CONTROL_POLYGON_MANIPULATOR) : u1Manipulator
                });
    }
    // U2
    if (selectedIndex != numCP - 1)
    {
        const u2Manipulator = linearManipulator({
                    "base" : base,
                    "direction" : normalize(points[selectedIndex + 1] - points[selectedIndex]),
                    "offset" : 0 * meter
                });
        addManipulators(context, id, {
                    (U2_CONTROL_POLYGON_MANIPULATOR) : u2Manipulator
                });
    }
    // V
    const chord = selectedIndex == 0 ? points[1] - points[0] : (selectedIndex == numCP - 1 ? points[numCP - 1] - points[numCP - 2] : points[selectedIndex + 1] - points[selectedIndex - 1]);
    var direction = cross(nDirection, chord);
    if (tolerantEqualsZero(norm(direction)))
    {
        // If we can't find a good V from U's and N, we default to the curvature frame's V.
        direction = frame.xAxis;
    }
    else
    {
        direction = normalize(direction);
    }
    const vManipulator = linearManipulator({
                "base" : base,
                "direction" : direction,
                "offset" : 0 * meter
            });
    addManipulators(context, id, {
                (V_CONTROL_POLYGON_MANIPULATOR) : vManipulator
            });
}

function showIndexManipulators(context is Context, id is Id, points is array, definition is map)
{
    if (!definition.editControlPoints)
    {
        showIndexManipulators(context, id, points, -1);
    }
    else if (definition.editPointMode == EditPointMode.XYZ)
    {
        const selectedIndices = mapArray(definition.selectedIndices, selectedIndex => selectedIndex.indexValue);
        const indicesManipulator = togglePointsManipulator({
                    "points" : points,
                    "selectedIndices" : selectedIndices,
                    "suppressedIndices" : []
                });

        addManipulators(context, id, {
                    (INDICES_MANIPULATOR) : indicesManipulator
                });
    }
    else
    {
        showIndexManipulators(context, id, points, definition.selectedIndex);
    }
}

function showIndexManipulators(context is Context, id is Id, points is array, selectedIndex is number)
{
    const indexManipulator = pointsManipulator({
                "points" : points,
                "index" : selectedIndex
            });

    addManipulators(context, id, {
                (INDEX_MANIPULATOR) : indexManipulator
            });
}

/**
 * Manipulator change handling for curve editing
 */
export function onEditCurveManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    if (newManipulators[INDEX_MANIPULATOR] is map)
    {
        // If the user deliberately selects a control point, we turn on CP editing
        definition.editControlPoints = true;
        if (definition.editPointMode == EditPointMode.XYZ)
        {
            definition.selectedIndices = [{ "indexValue" : newManipulators[INDEX_MANIPULATOR].index }];
        }
        else
        {
            definition.selectedIndex = newManipulators[INDEX_MANIPULATOR].index;
        }
    }
    if (newManipulators[INDICES_MANIPULATOR] is map)
    {
        definition.selectedIndices = mapArray(newManipulators[INDICES_MANIPULATOR].selectedIndices, index => { "indexValue" : index });
    }
    if (newManipulators[OFFSET_MANIPULATOR] is map)
    {
        definition = multiEditProcessing(context, definition, newManipulators[OFFSET_MANIPULATOR]);
    }
    for (var linearManipulator in LINEAR_MANIPULATORS)
    {
        if (newManipulators[linearManipulator] is map)
        {
            definition = processSingleDirectionEdit(context, definition, newManipulators[linearManipulator]);
        }
    }
    return definition;
}

function processSingleDirectionEdit(context is Context, definition is map, manip is map) returns map
{
    const offset = manip.direction * manip.offset;
    for (var i = 0; i < size(definition.controlPointEdits); i += 1)
    {
        var edit = definition.controlPointEdits[i];
        if (edit.index != definition.selectedIndex)
        {
            continue;
        }
        edit.x += offset[0];
        edit.y += offset[1];
        edit.z += offset[2];
        definition.controlPointEdits[i] = edit;
        return definition;
    }
    // We haven't found an existing edit, we add a new one.
    const bsplineBeforeEdit = computeBSplineBeforeEdit(context, definition);
    var weight = 1;
    if (bsplineBeforeEdit.weights != undefined && definition.selectedIndex < size(bsplineBeforeEdit.weights))
    {
        weight = bsplineBeforeEdit.weights[definition.selectedIndex];
    }
    var newEdit = {
        "index" : definition.selectedIndex,
        "reference" : qNothing(),
        "x" : offset[0],
        "y" : offset[1],
        "z" : offset[2],
        "weight" : weight
    };
    definition.controlPointEdits = append(definition.controlPointEdits, newEdit);
    return definition;
}


function multiEditProcessing(context is Context, definition is map, manipulator is map) returns map
{
    // First we need to average all the currently selected offsets.
    var baseOffset = WORLD_ORIGIN;
    var selectedIndices = {};
    for (var selectedIndex in definition.selectedIndices)
    {
        selectedIndices[selectedIndex.indexValue] = true;
    }
    var existingEditsIndices = [];
    for (var i = 0; i < size(definition.controlPointEdits); i += 1)
    {
        const edit = definition.controlPointEdits[i];
        if (selectedIndices[edit.index] == true)
        {
            selectedIndices[edit.index] = undefined; // useful for when we need to create the edits.
            existingEditsIndices = append(existingEditsIndices, i);
            baseOffset += [edit.x, edit.y, edit.z] as Vector;
        }
    }
    baseOffset /= size(definition.selectedIndices);

    const manipulatorOffset = [manipulator.offset[0], manipulator.offset[1], manipulator.offset[2]] as Vector;
    // We remove the average offsets from the new offset, this will give us how much we need to add to each edit
    const offsetToAdd = manipulatorOffset - baseOffset;
    // First we go through the edits that we did find and add the new offset.
    for (var existingEditIndex in existingEditsIndices)
    {
        definition.controlPointEdits[existingEditIndex].x += offsetToAdd[0];
        definition.controlPointEdits[existingEditIndex].y += offsetToAdd[1];
        definition.controlPointEdits[existingEditIndex].z += offsetToAdd[2];
    }
    // Then we go through the ones we didn't find and add new edits.
    if (selectedIndices == {})
    {
        return definition;
    }
    const bsplineBeforeEdit = computeBSplineBeforeEdit(context, definition);
    for (var nonExistingEditIndex in keys(selectedIndices))
    {
        var weight = 1;
        if (bsplineBeforeEdit.weights != undefined && nonExistingEditIndex < size(bsplineBeforeEdit.weights))
        {
            weight = bsplineBeforeEdit.weights[nonExistingEditIndex];
        }
        var newEdit = {
            "index" : nonExistingEditIndex,
            "reference" : qNothing(),
            "x" : offsetToAdd[0],
            "y" : offsetToAdd[1],
            "z" : offsetToAdd[2],
            "weight" : weight
        };
        definition.controlPointEdits = append(definition.controlPointEdits, newEdit);
    }
    return definition;
}

//==================================================================
//=========================== Edit Logic ===========================
//==================================================================


/**
 * Edit logic function for curve editing
 */
export function editCurveEditLogic(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean) returns map
{
    if (oldDefinition == {})
    {
        return definition;
    }
    if (definition.editControlPoints)
    {
        const controlPointEditSize = size(definition.controlPointEdits);
        const oldControlPointEditSize = size(oldDefinition.controlPointEdits);
        if (controlPointEditSize == oldControlPointEditSize)
        {
            for (var i = 0; i < controlPointEditSize; i += 1)
            {
                // If the user changes the reference in a control point edit, we reset the offset.
                // This is the reason why array reordering is turned off for definition.controlPointEdits.
                if (!areQueriesEquivalent(context, definition.controlPointEdits[i].reference, oldDefinition.controlPointEdits[i].reference))
                {
                    definition.controlPointEdits[i].x = 0 * meter;
                    definition.controlPointEdits[i].y = 0 * meter;
                    definition.controlPointEdits[i].z = 0 * meter;
                    return definition;
                }
            }
        }
    }
    return definition;
}

//==================================================================
//=========================== Utilities ============================
//==================================================================

// Returns {} if there's an issue, otherwise the bspline
function computeBSplineBeforeEdit(context is Context, definition is map) returns map
{
    if (isQueryEmpty(context, definition.wire))
    {
        return {};
    }
    // If getBSplineFromInput throws, we it means that either we need to reapproximate the curve,
    // or the approximation parameters are wrong.
    // In both cases, it's fine to just set the new weight to 1.
    var bspline;
    try
    {
        bspline = getBSplineFromInput(context, definition);
    }
    catch
    {
        return {};
    }
    if (definition.elevate && bspline.degree < definition.elevationDegree)
    {
        bspline = elevateDegree(bspline, definition.elevationDegree);
    }
    return bspline;
}

function isBezier(points is array, curveDegree is number, knots is array) returns boolean
{
    return size(points) == curveDegree + 1 && knots[0] == 0;
}

function computeSpans(bspline is map) returns number
{
    var spans = 1;
    for (var i = bspline.degree + 1; i < size(bspline.knots) - (bspline.degree + 1); i += 1)
    {
        if (bspline.knots[i] != bspline.knots[i - 1])
        {
            spans += 1;
        }
    }
    return spans;
}

function showPolyline(context is Context, bspline is map)
{
    for (var i = 0; i < size(bspline.controlPoints) - 1; i += 1)
    {
        if (tolerantEquals(bspline.controlPoints[i], bspline.controlPoints[i + 1]))
        {
            continue;
        }
        addDebugLine(context, bspline.controlPoints[i], bspline.controlPoints[i + 1], DebugColor.MAGENTA);
    }
    if (bspline.isPeriodic && !firstAndLastCPShouldOverlap(bspline))
    {
        addDebugLine(context, bspline.controlPoints[size(bspline.controlPoints) - 1], bspline.controlPoints[0], DebugColor.MAGENTA);
    }
}

function updateCurveData(context is Context, id is Id, bspline is map)
{
    const spans = computeSpans(bspline);
    const numCP = size(bspline.controlPoints);

    setFeatureComputedParameter(context, id, { "name" : "curveDegree", "value" : bspline.degree });
    setFeatureComputedParameter(context, id, { "name" : "curveNumCPs", "value" : numCP });
    setFeatureComputedParameter(context, id, { "name" : "curveNumSpans", "value" : spans });
}

function getAllEdgesQuery(query is Query) returns Query
{
    return qUnion([qEntityFilter(query, EntityType.EDGE), qEntityFilter(query, EntityType.BODY)->qOwnedByBody(EntityType.EDGE)]);
}

function firstAndLastCPShouldOverlap(bspline is map) returns boolean
{
    return bspline.isPeriodic && bspline.knots[0] == 0;
}

function grevilleParameter(controlPointIndex is number, degree is number, knots is array) returns number
{
    // The knot vector already contains the extra knots for periodic bsplines, so we don't need to do any special handling for periodicity here.
    var sum = 0;
    for (var j = 1; j <= degree; j += 1)
    {
        sum += knots[controlPointIndex + j];
    }
    return sum / degree;
}

function getCurvatureFrameAtControlPointIndex(context is Context, id is Id, index is number, bspline is map) returns CoordSystem
{
    const edgeParameter = grevilleParameter(index, bspline.degree, bspline.knots);
    const uvnId = id + "uvn";
    startFeature(context, uvnId, {});
    opCreateBSplineCurve(context, uvnId + "bSplineCurve", {
                "bSplineCurve" : bSplineCurve(bspline)
            });
    const result = evEdgeCurvature(context, {
                "edge" : qCreatedBy(uvnId + "bSplineCurve", EntityType.EDGE),
                "parameter" : edgeParameter
            });
    abortFeature(context, uvnId);
    return result.frame;
}

function indexIsValid(context is Context, id is Id, index is number, points is array) returns boolean
{
    if (index < 0 || index > size(points) - 1)
    {
        reportFeatureWarning(context, id, ErrorStringEnum.EDIT_CURVE_INDEX_TOO_LARGE, ["selectedIndex"]);
        return false;
    }
    return true;
}

function indicesAreValid(context is Context, id is Id, indices is array, points is array) returns boolean
{
    const numControlPoints = size(points);
    for (var index in indices)
    {
        if (index.indexValue < 0 || index.indexValue > numControlPoints - 1)
        {
            reportFeatureWarning(context, id, ErrorStringEnum.EDIT_CURVE_INDEX_TOO_LARGE, ["selectedIndices"]);
            return false;
        }
    }
    return true;
}

