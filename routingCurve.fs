FeatureScript 2625; /* Automatically generated version */
import(path : "onshape/std/coordSystem.fs", version : "2625.0");
import(path : "onshape/std/debug.fs", version : "2625.0");
import(path : "onshape/std/evaluate.fs", version : "2625.0");
import(path : "onshape/std/feature.fs", version : "2625.0");
import(path : "onshape/std/manipulator.fs", version : "2625.0");
import(path : "onshape/std/matrix.fs", version : "2625.0");
import(path : "onshape/std/path.fs", version : "2625.0");
import(path : "onshape/std/query.fs", version : "2625.0");
import(path : "onshape/std/table.fs", version : "2625.0");
import(path : "onshape/std/tabReferences.fs", version : "2625.0");
import(path : "onshape/std/topologyUtils.fs", version : "2625.0");
import(path : "onshape/std/transform.fs", version : "2625.0");
import(path : "onshape/std/units.fs", version : "2625.0");
import(path : "onshape/std/valueBounds.fs", version : "2625.0");
import(path : "onshape/std/vector.fs", version : "2625.0");

export import(path : "onshape/std/importForeign.fs", version : "2625.0");

//==================================================================
//============================= Enums ==============================
//==================================================================

/**
 * Curve types.
 */
export enum RoutingCurveType
{
    annotation { "Name" : "Interpolated spline" }
    INTERPOLATED_SPLINE,
    annotation { "Name" : "Polyline" }
    POLYLINE
}

/**
 * Input types.
 */
export enum InputType
{
    annotation { "Name" : "Vertex" }
    VERTEX,
    annotation { "Name" : "Curve" }
    CURVE,
    annotation { "Name" : "CSV file" }
    CSV
}

/**
 * Curve edit modes.
 */
export enum CurveStep
{
    annotation { "Name" : "Points" }
    POINTS,
    annotation { "Name" : "Segments" }
    SEGMENTS
}

/**
 * Interpolated spline derivative alignments.
 */
export enum DerivativeAlignment
{
    annotation { "Name" : "Manipulator X axis" }
    X,
    annotation { "Name" : "Manipulator Y axis" }
    Y,
    annotation { "Name" : "Manipulator Z axis" }
    Z,
    annotation { "Name" : "Direction" }
    DIRECTION
}

/**
 * Point reference types.
 */
export enum ReferenceType
{
    annotation { "Name" : "Origin" }
    ORIGIN,
    annotation { "Name" : "Vertex" }
    VERTEX,
    annotation { "Name" : "Curve" }
    CURVE,
    annotation { "Name" : "Relative" }
    RELATIVE
}

/**
 * Segment edit modes.
 */
export enum SegmentEditType
{
    annotation { "Name" : "Add points" }
    ADD,
    annotation { "Name" : "Orthogonal path" }
    ORTHOGONAL
}

/**
 * Coordinate system options for the othogonal path segment edit mode.
 */
export enum OrthoCoordSystem
{
    annotation { "Name" : "World" }
    WORLD,
    annotation { "Name" : "Curve" }
    CURVE,
    annotation { "Name" : "Other" }
    OTHER
}

//==================================================================
//============================= Bounds =============================
//==================================================================

/**
 * Integer bound for segment index in segment edit mode.
 */
export const POINT_INDEX_BOUNDS =
{
            (unitless) : [0, 0, 10000]
        } as IntegerBoundSpec;

/**
 * Integer bound for segment index in segment edit mode.
 */
export const SEGMENT_INDEX_BOUNDS =
{
            (unitless) : [-1, -1, 10000]
        } as IntegerBoundSpec;

/**
 * Integer bound for CSV column index.
 */
export const COLUMN_BOUNDS =
{
            (unitless) : [0, 0, 1e5]
        } as IntegerBoundSpec;

/**
 * Integer bound for curve sampling.
 */
export const CURVE_SAMPLE_BOUNDS =
{
            (unitless) : [2, 5, 50]
        } as IntegerBoundSpec;

//==================================================================
//============================= Consts =============================
//==================================================================

const emptyPoint = {
        "index" : 0,
        "referenceType" : ReferenceType.ORIGIN,
        "vertex" : qNothing(),
        "curve" : qNothing(),
        "curveParameter" : 0,
        "alignCSys" : false,
        "flipCSys" : false,
        "dx" : 0 * inch,
        "dy" : 0 * inch,
        "dz" : 0 * inch,
        "rotationMatrix" : identityMatrix(3),
        "overrideBendRadius" : false,
        "pointBendRadius" : 0 * inch,
        "hasDerivative" : false,
        "derivativeAlignment" : DerivativeAlignment.X,
        "magnitude" : 1,
        "oppositeDirection" : false,
        "derivativeDirection" : qNothing(),
        "arrayAdded" : false
    };

const POINT_MANIPULATOR = "pointManipulator";
const TRIAD_MANIPULATOR = "triadManipulator";
const SEGMENT_MANIPULATOR = "segmentManipulator";
// These are the parameter ids in the points map that can accept variables.
const PARAMETERS_TO_COPY = ["dx", "dy", "dz", "pointBendRadius", "magnitude"];

//==================================================================
//=========================== Predicates ===========================
//==================================================================

predicate pointsStepPredicate(definition is map)
{
    inputsPredicate(definition);

    annotation { "Name" : "Closed" }
    definition.isPeriodic is boolean;

    annotation { "Name" : "Curve reference", "Filter" : EntityType.VERTEX || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1,
                "Description" : "Point or mate connector used as reference for the curve. Defaults to origin." }
    definition.curveBaseCSys is Query;

    annotation { "Name" : "Selected Point", "UIHint" : [UIHint.ALWAYS_HIDDEN, UIHint.UNCONFIGURABLE] }
    isInteger(definition.pointIndex, POINT_INDEX_BOUNDS);

    pointsPredicate(definition);

    annotation { "Name" : "Reset triad" }
    isButton(definition.resetTriad);

    annotation { "Name" : "Add point on axis drag", "UIHint" : UIHint.UNCONFIGURABLE }
    definition.addPointOnAxisDrag is boolean;

    annotation { "Name" : "Show reference coordinate system", "Description" : "Show coordinate system of the reference used by the current vertex.", "Default" : true }
    definition.showCSys is boolean;

    annotation { "Name" : "Keep points", "Description" : "Create points in addition to the curve." }
    definition.keepPoints is boolean;
}

predicate inputsPredicate(definition is map)
{
    annotation { "Group Name" : "Inputs", "Collapsed By Default" : false }
    {

        annotation { "Name" : "Input type", "UIHint" : UIHint.UNCONFIGURABLE }
        definition.inputType is InputType;

        if (definition.inputType == InputType.VERTEX)
        {
            annotation { "Name" : "Vertices", "Filter" : EntityType.VERTEX || BodyType.MATE_CONNECTOR, "UIHint" : UIHint.UNCONFIGURABLE }
            definition.inputVertices is Query;
        }
        else
        {
            if (definition.inputType == InputType.CURVE)
            {
                annotation { "Name" : "Edges", "Filter" : EntityType.EDGE || (EntityType.BODY && BodyType.WIRE && SketchObject.NO), "UIHint" : UIHint.UNCONFIGURABLE }
                definition.inputCurve is Query;

                annotation { "Name" : "Number of samples", "UIHint" : UIHint.UNCONFIGURABLE }
                isInteger(definition.samples, CURVE_SAMPLE_BOUNDS);
            }
            else
            {
                annotation { "Name" : "Table", "UIHint" : UIHint.UNCONFIGURABLE }
                definition.inputData is TableData;

                annotation { "Name" : "Data start column (0,1,2,.)", "UIHint" : UIHint.UNCONFIGURABLE }
                isInteger(definition.startColumn, COLUMN_BOUNDS);

                annotation { "Name" : "Unit", "Default" : LengthUnitNames.Meter, "UIHint" : UIHint.UNCONFIGURABLE }
                definition.csvUnit is LengthUnitNames;
            }

            annotation { "Name" : "Overwrite points", "Default" : true, "UIHint" : UIHint.UNCONFIGURABLE }
            definition.overwritePoints is boolean;

            annotation { "Name" : "Process" }
            isButton(definition.processInputs);
        }
    }
}

predicate pointsPredicate(definition is map)
{
    annotation { "Name" : "Points", "Item name" : "point", "Item label template" : "Point #index", "UIHint" : UIHint.PREVENT_ARRAY_REORDER }
    definition.points is array;
    for (var point in definition.points)
    {
        annotation { "Name" : "Point index", "UIHint" : [UIHint.ALWAYS_HIDDEN] }
        isInteger(point.index, POINT_INDEX_BOUNDS);

        annotation { "Name" : "Reference", "UIHint" : UIHint.SHOW_LABEL }
        point.referenceType is ReferenceType;

        if (point.referenceType == ReferenceType.VERTEX)
        {
            annotation { "Name" : "Vertex", "Filter" : EntityType.VERTEX || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
            point.vertex is Query;
        }
        else if (point.referenceType == ReferenceType.CURVE)
        {
            annotation { "Name" : "Edges", "Filter" : EntityType.EDGE || (EntityType.BODY && BodyType.WIRE) }
            point.curve is Query;
            annotation { "Name" : "Parameter" }
            isReal(point.curveParameter, EDGE_PARAMETER_BOUNDS);

            annotation { "Name" : "Align coordinate system", "Default" : false }
            point.alignCSys is boolean;
            if (point.alignCSys)
            {
                annotation { "Name" : "Flip coordinate system", "Default" : false, "UIHint" : UIHint.OPPOSITE_DIRECTION }
                point.flipCSys is boolean;
            }
        }

        annotation { "Name" : "X offset" }
        isLength(point.dx, ZERO_DEFAULT_LENGTH_BOUNDS);
        annotation { "Name" : "Y offset" }
        isLength(point.dy, ZERO_DEFAULT_LENGTH_BOUNDS);
        annotation { "Name" : "Z offset" }
        isLength(point.dz, ZERO_DEFAULT_LENGTH_BOUNDS);

        // Basis for triad manipulator
        annotation { "Name" : "Rotation matrix", "UIHint" : UIHint.ALWAYS_HIDDEN }
        isAnything(point.rotationMatrix);

        if (definition.curveType == RoutingCurveType.POLYLINE)
        {
            annotation { "Name" : "Override bend radius" }
            point.overrideBendRadius is boolean;
            if (point.overrideBendRadius)
            {
                annotation { "Name" : "Bend radius" }
                isLength(point.pointBendRadius, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
            }
        }
        else
        {
            annotation { "Name" : "Derivative" }
            point.hasDerivative is boolean;
            if (point.hasDerivative)
            {
                annotation { "Name" : "Derivative alignment" }
                point.derivativeAlignment is DerivativeAlignment;

                if (point.derivativeAlignment == DerivativeAlignment.DIRECTION)
                {
                    annotation { "Name" : "Derivative direction", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
                    point.derivativeDirection is Query;
                }

                annotation { "Name" : "Magnitude" }
                isReal(point.magnitude, CLAMP_MAGNITUDE_REAL_BOUNDS);

                annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                point.oppositeDirection is boolean;
            }
        }

        annotation { "Name" : "Added by the array", "Default" : true, "UIHint" : UIHint.ALWAYS_HIDDEN }
        point.arrayAdded is boolean;
    }
}

predicate segmentsStepPredicate(definition is map)
{
    annotation { "Name" : "Segment index", "UIHint" : UIHint.ALWAYS_HIDDEN }
    isInteger(definition.segmentIndex, SEGMENT_INDEX_BOUNDS);

    annotation { "Name" : "Segment edit type", "UIHint" : UIHint.UNCONFIGURABLE }
    definition.segmentEditType is SegmentEditType;

    if (definition.segmentEditType == SegmentEditType.ADD)
    {
        annotation { "Name" : "Number of points to add", "UIHint" : UIHint.UNCONFIGURABLE }
        isInteger(definition.numPointsToAdd, { (unitless) : [0, 1, 10000] } as IntegerBoundSpec);
    }
    else if (definition.segmentEditType == SegmentEditType.ORTHOGONAL)
    {
        orthoPathPredicate(definition);
    }
    annotation { "Name" : "Hidden references", "UIHint" : [UIHint.ALWAYS_HIDDEN, UIHint.ALWAYS_USE_DEPENDENCIES],
                 "Filter" : EntityType.EDGE || (EntityType.BODY && BodyType.WIRE) || EntityType.VERTEX || BodyType.MATE_CONNECTOR }
    definition.hiddenReferences is Query;
}

predicate orthoPathPredicate(definition is map)
{
    annotation { "Name" : "Coordinate system", "UIHint" : [UIHint.SHOW_LABEL, UIHint.UNCONFIGURABLE] }
    definition.orthoCsys is OrthoCoordSystem;

    if (definition.orthoCsys == OrthoCoordSystem.OTHER)
    {
        annotation { "Name" : "Orthogonal path coordinate system", "Filter" : EntityType.VERTEX || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1, "UIHint" : UIHint.UNCONFIGURABLE }
        definition.orthoCsysMate is Query;
    }

    // We have at most 6 options (x,y,z), (x,z,y), (y,x,z), (y,z,x), (z,x,y), (z,y,x).
    // We do have to do a manual bounds check in case the selected points are aligned on one axis, which reduces the options to 2.
    annotation { "Name" : "Solution index", "UIHint" : UIHint.ALWAYS_HIDDEN }
    isInteger(definition.solutionIndex, { (unitless) : [0, 0, 5] } as IntegerBoundSpec);

    annotation { "Name" : "Previous", "UIHint" : UIHint.DISPLAY_SHORT }
    isButton(definition.orthoPrevious);

    annotation { "Name" : "Next", "UIHint" : UIHint.DISPLAY_SHORT }
    isButton(definition.orthoNext);

    annotation { "Name" : "Confirm" }
    isButton(definition.confirmOrthoPath);
}

//==================================================================
//============================ Feature =============================
//==================================================================

/**
 * Creates a routing curve feature.
 */
annotation { "Feature Type Name" : "Routing curve",
        "Feature Name Template" : "Routing curve (#curveLength)",
        "Manipulator Change Function" : "routingCurveManipulator",
        "Editing Logic Function" : "routingCurveEditLogic" }
export const routingCurve = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Preselection", "UIHint" : UIHint.ALWAYS_HIDDEN, "Filter" : EntityType.EDGE || (EntityType.BODY && BodyType.WIRE && SketchObject.NO) || EntityType.VERTEX || BodyType.MATE_CONNECTOR }
        definition.preselectedEntities is Query;

        annotation { "Name" : "Curve type" }
        definition.curveType is RoutingCurveType;

        if (definition.curveType == RoutingCurveType.POLYLINE)
        {
            annotation { "Name" : "Bend radius" }
            isLength(definition.bendRadius, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
        }

        annotation { "Name" : "Step", "UIHint" : [UIHint.HORIZONTAL_ENUM, UIHint.UNCONFIGURABLE] }
        definition.step is CurveStep;

        if (definition.step == CurveStep.POINTS)
        {
            pointsStepPredicate(definition);
        }
        else
        {
            segmentsStepPredicate(definition);
        }
        annotation { "Name" : "Curve length", "UIHint" : UIHint.ALWAYS_HIDDEN }
        isLength(definition.curveLength, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
    }
    {
        checkInputs(context, definition);

        const numPoints = size(definition.points);
        const currentIndicesOrder = mapArray(definition.points, point => point.index);
        const newIndicesOrder = shiftIndices(deduplicateIndices(currentIndicesOrder));
        if (currentIndicesOrder != newIndicesOrder)
        {
            // If there was an addition/deletion as a result of a merge, edit logic has not yet run so we need to fix it manually until it runs.
            for (var i = 0; i < numPoints; i += 1)
            {
                definition.points[i].index = newIndicesOrder[i];
                // This is for the array item label
                setFeatureComputedParameter(context, id, {
                            "name" : "points[" ~ toString(i) ~ "].index",
                            "value" : newIndicesOrder[i]
                        });
            }
        }

        // Hide all the points in the array but the currently selected one.
        var ids = [];
        for (var i = 0; i < numPoints; i += 1)
        {
            if (definition.points[i].index != definition.pointIndex)
            {
                ids = append(ids, "points[" ~ toString(i) ~ "]");
            }
        }
        setFeatureHiddenParameters(context, id, ids);

        // Order the points and compute their positions
        const processedData = processRawPoints(context, definition);
        var positions = extractPositions(processedData.orderedPoints);

        // Show manipulators and polylines if necessary
        addManipulatorsAsNeeded(context, id, definition, processedData, positions);

        if (definition.step == CurveStep.SEGMENTS)
        {
            showControlPolyline(context, positions, definition.segmentIndex);

            if (definition.segmentEditType == SegmentEditType.ORTHOGONAL)
            {
                showCurrentOrthoSolution(context, id, positions, definition, processedData.baseCSys);
            }
        }
        else if (definition.showCSys)
        {
            showCSys(context, processedData.orderedPoints[definition.pointIndex].cSys);
        }

        // Remove coincident points to actually make the curve
        const cleanPoints = cleanupPoints(processedData.orderedPoints);

        if (size(cleanPoints) < 2 && !definition.isPeriodic)
        {
            throw regenError(ErrorStringEnum.ROUTING_CURVE_AT_LEAST_TWO_DISTINCT_POINTS, ["points"]);
        }
        if (size(cleanPoints) < 3 && definition.isPeriodic)
        {
            throw regenError(ErrorStringEnum.ROUTING_CURVE_AT_LEAST_THREE_DISTINCT_POINTS, ["points"]);
        }

        // Compute final positions, derivatives/radii
        positions = extractPositions(cleanPoints);
        var bendRadii = extractBendRadii(cleanPoints, definition.bendRadius);
        var derivatives = extractDerivatives(context, cleanPoints);

        if (definition.isPeriodic && !tolerantEquals(positions[0], positions[size(positions) - 1]))
        {
            positions = append(positions, positions[0]);
            if (definition.curveType == RoutingCurveType.POLYLINE)
            {
                const numPoints = size(processedData.orderedPoints);
                bendRadii = append(bendRadii, processedData.orderedPoints[numPoints - 1].overrideBendRadius ? processedData.orderedPoints[numPoints - 1].pointBendRadius : definition.bendRadius);
                bendRadii = append(bendRadii, processedData.orderedPoints[0].overrideBendRadius ? processedData.orderedPoints[0].pointBendRadius : definition.bendRadius);
            }
        }
        // Make the points and curve
        if (definition.keepPoints)
        {
            createPoints(context, id, positions);
        }

        if (definition.curveType == RoutingCurveType.INTERPOLATED_SPLINE)
        {
            if (derivatives != {})
            {
                const boundingBox = box3d(positions);
                const totalSpan = box3dDiagonalLength(boundingBox);
                for (var index, derivative in derivatives)
                {
                    derivatives[index] = derivative * totalSpan;
                }
                showInterpolatedSplineDerivatives(context, positions, derivatives);
            }
            makeInterpolatedSpline(context, id, positions, derivatives);
        }
        else if (definition.curveType == RoutingCurveType.POLYLINE)
        {
            makePolyline(context, id, positions, bendRadii);
        }
        setCurveLength(context, id);
    }, { "hiddenReferences" : qNothing()});

//==================================================================
//======================== Input processing ========================
//==================================================================

function checkInputs(context is Context, definition is map)
{
    if (definition.inputType == InputType.CURVE && !isQueryEmpty(context, definition.inputCurve))
    {
        const edgesQuery = qUnion([definition.inputCurve->qEntityFilter(EntityType.BODY)->qOwnedByBody(EntityType.EDGE), definition.inputCurve->qEntityFilter(EntityType.EDGE)]);
        try silent
        {
            constructPath(context, edgesQuery);
        }
        catch (error)
        {
            throw regenError(error, ["inputCurve"]);
        }
    }
    if (definition.inputType == InputType.CSV && definition.inputData != {} && definition.inputData.csvData != undefined)
    {
        const importedData = definition.inputData.csvData;
        for (var row in importedData)
        {
            checkCSVRow(row, definition.startColumn);
        }
    }
    if (definition.points == [])
    {
        throw regenError(ErrorStringEnum.ROUTING_CURVE_AT_LEAST_TWO_DISTINCT_POINTS, ["points"]);
    }
}

function checkCSVRow(row is array, startColumn is number)
{
    if (size(row) < startColumn + 3)
    {
        throw regenError(ErrorStringEnum.ROUTING_CURVE_CSV_NOT_ENOUGH_COLUMNS, ["inputData", "startColumn"]);
    }

    if (!(row[startColumn] is number) ||
        !(row[startColumn + 1] is number) ||
        !(row[startColumn + 2] is number))
    {
        throw regenError(ErrorStringEnum.ROUTING_CURVE_CSV_INVALID_DATA, ["inputData", "startColumn"]);
    }
}

function processNewVertices(context is Context, definition is map) returns map
{
    const vertices = evaluateQuery(context, definition.inputVertices);
    const points = mapArray(vertices, vertex => evVertexPoint(context, { 'vertex' : vertex }));
    const clusters = clusterPoints(points, TOLERANCE.zeroLength * meter);
    var filteredIndices = mapArray(clusters, cluster => cluster[0]);
    filteredIndices = sort(filteredIndices, function(a, b) { return a - b; });
    const filteredVertices = mapArray(filteredIndices, index => vertices[index]);
    const numVertices = size(filteredVertices);
    var newPoints = makeArray(numVertices, emptyPoint);
    for (var i = 0; i < numVertices; i += 1)
    {
        newPoints[i].referenceType = ReferenceType.VERTEX;
        newPoints[i].vertex = filteredVertices[i];
    }
    definition = addNewPoints(context, definition, newPoints, false);

    definition.inputVertices = qNothing();

    return definition;
}

function processNonVertexInputs(context is Context, definition is map) returns map
{
    if (definition.inputType == InputType.CURVE)
    {
        return processEdgeInput(context, definition);
    }
    else if (definition.inputType == InputType.CSV)
    {
        return processCSVData(context, definition, definition.overwritePoints);
    }
    return definition;
}

function processEdgeInput(context is Context, definition is map) returns map
{
    if (isQueryEmpty(context, definition.inputCurve))
    {
        return definition;
    }

    const numPoints = definition.samples;

    try
    {
        // If the path is closed, we need to add one less point and to make the curve closed.
        // We do this in a try block because this happens in edit logic, so if it fails, it won't populate the array otherwise.
        // By allowing the point array to be populated, we ensure the feature itself will fail with the correct error when evaluating these points.
        const edgesQuery = qUnion([definition.inputCurve->qEntityFilter(EntityType.BODY)->qOwnedByBody(EntityType.EDGE), definition.inputCurve->qEntityFilter(EntityType.EDGE)]);
        const path = constructPath(context, edgesQuery);
        definition.isPeriodic = path.closed;
    }

    const step = 1 / (definition.samples - (definition.isPeriodic ? 0 : 1));

    var newPoints = makeArray(numPoints, emptyPoint);
    for (var i = 0; i < numPoints; i += 1)
    {
        newPoints[i].referenceType = ReferenceType.CURVE;
        newPoints[i].curve = definition.inputCurve;
        newPoints[i].curveParameter = i * step;
    }
    definition = addNewPoints(context, definition, newPoints, definition.overwritePoints);
    definition.inputCurve = qNothing();
    return definition;
}

function processCSVData(context is Context, definition is map, overwritePoints is boolean) returns map
{
    if (definition.inputData == {} || definition.inputData.csvData == undefined)
    {
        return definition;
    }
    const newPoints = getInputCSVPoints(definition);
    if (newPoints != [])
    {
        definition = addNewPoints(context, definition, newPoints, overwritePoints);
    }
    return definition;
}

function getInputCSVPoints(definition is map) returns array
{
    const importedData = definition.inputData.csvData;
    const unit = getUnit(definition.csvUnit);
    var points = [];
    var processedRows = {};
    for (var row in importedData)
    {
        checkCSVRow(row, definition.startColumn);
        const croppedRow = [row[definition.startColumn], row[definition.startColumn + 1], row[definition.startColumn + 2]];
        // We're only checking for fully duplicated rows, we're not tolerance checking the actual points.
        if (processedRows[croppedRow] != undefined)
        {
            continue;
        }
        processedRows[croppedRow] = true;
        var point = emptyPoint;
        point.dx = croppedRow[0] * unit;
        point.dy = croppedRow[1] * unit;
        point.dz = croppedRow[2] * unit;

        points = append(points, point);
    }
    return points;
}

//==================================================================
//======================== Point processing ========================
//==================================================================

function addNewPoints(context is Context, definition is map, points is array, overwrite is boolean) returns map
{
    return addNewPoints(context, definition, points, overwrite, definition.pointIndex);
}

function addNewPoints(context is Context, definition is map, points is array, overwrite is boolean, index is number) returns map
{
    const numPoints = size(points);
    const onlyEmptyPoint = size(definition.points) == 1 &&
        isQueryEmpty(context, definition.points[0].vertex) &&
        definition.points[0].dx == 0 * meter &&
        definition.points[0].dy == 0 * meter &&
        definition.points[0].dz == 0 * meter;
    if (overwrite || size(definition.points) == 0 || onlyEmptyPoint)
    {
        for (var i = 0; i < numPoints; i += 1)
        {
            points[i].index = i;
        }
        definition.points = points;
        definition.pointIndex = numPoints - 1;
    }
    else
    {
        // If we don't override and there are existing points,
        // we shift the indices that are after the currently selected points.
        for (var i = 0; i < size(definition.points); i += 1)
        {
            if (definition.points[i].index > index)
            {
                definition.points[i].index += numPoints;
            }
        }
        for (var i = 0; i < numPoints; i += 1)
        {
            points[i].index = i + index + 1;
        }
        definition.points = concatenateArrays([definition.points, points]);
        if (definition.pointIndex >= index)
        {
            definition.pointIndex += numPoints;
        }
    }
    return definition;
}

//==================================================================
//============================== CSys ==============================
//==================================================================

function processCSys(context is Context, vertex is Query) returns CoordSystem
{
    return processCSys(context, vertex, WORLD_COORD_SYSTEM);
}

function processCSys(context is Context, vertex is Query, base is CoordSystem) returns CoordSystem
{
    if (!isQueryEmpty(context, qBodyType(vertex, BodyType.MATE_CONNECTOR)))
    {
        base = evMateConnector(context, {
                    "mateConnector" : vertex
                });
    }
    else if (!isQueryEmpty(context, vertex))
    {
        base.origin = evVertexPoint(context, {
                    "vertex" : vertex
                });
    }
    return base;
}

//==================================================================
//========================== Manipulators ==========================
//==================================================================

function addManipulatorsAsNeeded(context is Context, id is Id, definition is map, processedData is map, points is array)
{
    if (definition.step == CurveStep.POINTS)
    {
        addPointsAndTriadManipulators(context, id, definition, processedData, points);
    }
    else
    {
        addSegmentsManipulator(context, id, points);
    }
}

function addPointsAndTriadManipulators(context is Context, id is Id, definition is map, processedData is map, points is array)
{
    if (definition.points == [])
    {
        return;
    }
    addPointsManipulator(context, id, points, definition.pointIndex);
    addTriadManipulator(context, id, processedData.orderedPoints[definition.pointIndex]);
}

function addPointsManipulator(context is Context, id is Id, points is array, index is number)
{
    if (size(points) < 2)
    {
        return;
    }
    // We remove the position of the currently selected point because the point manipulator would interfere with the triad manipulator
    const pointsWithoutIndex = removeElementAt(points, index);
    const pointManip = pointsManipulator({
                "points" : pointsWithoutIndex,
                "index" : -1
            });
    addManipulators(context, id, { (POINT_MANIPULATOR) : pointManip });
}

function addTriadManipulator(context is Context, id is Id, point is map)
{
    const triadTransform = transform(matrix(point.rotationMatrix),
        vector(point.dx, point.dy, point.dz));

    const triadManip = fullTriadManipulator({
                "base" : point.cSys,
                "transform" : triadTransform,
                "displayEditView" : true
            });
    addManipulators(context, id, { (TRIAD_MANIPULATOR) : triadManip });
}

function addSegmentsManipulator(context is Context, id is Id, points is array)
{
    var positions = [];
    for (var i = 0; i < size(points) - 1; i += 1)
    {
        positions = append(positions, points[i] + (points[i + 1] - points[i]) / 2);
    }
    const pointManip = pointsManipulator({
                "points" : positions,
                "index" : -1
            });
    addManipulators(context, id, { (SEGMENT_MANIPULATOR) : pointManip });
}

/**
 * Manipulator handler for routing curve feature.
 */
export function routingCurveManipulator(context is Context, definition is map, newManipulators is map) returns map
{
    if (newManipulators[POINT_MANIPULATOR] is map)
    {
        definition = pointsManipulatorHandler(definition, newManipulators[POINT_MANIPULATOR].index);
    }
    if (newManipulators[TRIAD_MANIPULATOR] is map)
    {
        definition = triadManipulatorHandler(context, definition, newManipulators[TRIAD_MANIPULATOR].transform, newManipulators[TRIAD_MANIPULATOR].dragType);
    }
    if (newManipulators[SEGMENT_MANIPULATOR] is map)
    {
        definition = segmentManipulatorHandler(context, newManipulators[SEGMENT_MANIPULATOR].index, definition);
    }

    return definition;
}

function pointsManipulatorHandler(definition is map, newIndex is number) returns map
{
    const oldIndex = definition.pointIndex;
    definition.pointIndex = newIndex;
    // This is needed to account for the point at oldIndex not being present in the array we gave the manipulator.
    if (definition.pointIndex >= oldIndex)
    {
        definition.pointIndex = definition.pointIndex + 1;
    }
    return definition;
}

function isLinearDrag(dragType is ManipulatorDragTypeEnum)
{
    return dragType == ManipulatorDragTypeEnum.LINEAR_X || dragType == ManipulatorDragTypeEnum.LINEAR_Y || dragType == ManipulatorDragTypeEnum.LINEAR_Z;
}

function triadManipulatorHandler(context is Context, definition is map, triadTransform is Transform, dragType is ManipulatorDragTypeEnum) returns map
{
    const arrayParameterIndex = getArrayIndexOfPointWithIndex(definition.points, definition.pointIndex);
    var newPoint = definition.points[arrayParameterIndex];
    const transposedLinear = transpose(triadTransform.linear);
    newPoint.rotationMatrix = transposedLinear;

    newPoint.dx = triadTransform.translation[0];
    newPoint.dy = triadTransform.translation[1];
    newPoint.dz = triadTransform.translation[2];

    if (definition.addPointOnAxisDrag && isLinearDrag(dragType) && shouldAddPointOnAxisDrag(context, definition, newPoint))
    {
        newPoint.index = definition.pointIndex + 1;
        for (var i = 0; i < size(definition.points); i += 1)
        {
            if (definition.points[i].index > definition.pointIndex)
            {
                definition.points[i].index += 1;
            }
        }
        if (newPoint.referenceType == ReferenceType.RELATIVE)
        {
            newPoint.dx -= definition.points[arrayParameterIndex].dx;
            newPoint.dy -= definition.points[arrayParameterIndex].dy;
            newPoint.dz -= definition.points[arrayParameterIndex].dz;
        }
        definition.pointIndex = definition.pointIndex + 1;
        definition.points = append(definition.points, newPoint);
    }
    else
    {
        definition.points[arrayParameterIndex] = newPoint;
    }
    return definition;
}

// We only want to add a point if the previous, current and new points are not aligned to avoid adding points everytime we receive a manipulator update.
function shouldAddPointOnAxisDrag(context is Context, definition is map, newPoint is map) returns boolean
{
    const processedData = processRawPoints(context, definition);

    const currentPointLocation = processedData.orderedPoints[definition.pointIndex].position;
    const newPointOffset = vector(newPoint.dx, newPoint.dy, newPoint.dz);
    const newPointLocation = toWorld(processedData.orderedPoints[definition.pointIndex].cSys, newPointOffset);
    if (tolerantEquals(newPointLocation, currentPointLocation))
    {
        return false;
    }
    // There is no previous point to check against.
    if (definition.pointIndex == 0)
    {
        return true;
    }
    const previousPointLocation = processedData.orderedPoints[definition.pointIndex - 1].position;
    return !parallelVectors(previousPointLocation - currentPointLocation, currentPointLocation - newPointLocation);
}

function segmentManipulatorHandler(context is Context, index is number, definition is map) returns map
{
    const previousSegmentIndex = definition.segmentIndex;
    definition.segmentIndex = index;

    if (definition.segmentEditType == SegmentEditType.ADD)
    {
        definition = addPointsToSegment(context, index, definition);
    }
    else if (definition.segmentEditType == SegmentEditType.ORTHOGONAL)
    {
        if (previousSegmentIndex == definition.segmentIndex)
        {
            definition.segmentIndex = -1;
        }
        else
        {
            definition.solutionIndex = 0;
        }
    }
    return definition;
}

function changeRelativeEndPointOffset(definition is map, processedData is map, position is Vector, newOrigin is Vector) returns map
{
    if (processedData.orderedPoints[definition.segmentIndex + 1].referenceType != ReferenceType.RELATIVE)
    {
        return definition;
    }
    var newCSys = processedData.orderedPoints[definition.segmentIndex + 1].cSys;
    newCSys.origin = newOrigin;
    const oldPointInNewCSys = fromWorld(newCSys, position);

    const nextPointIndex = getArrayIndexOfPointWithIndex(definition.points, definition.segmentIndex + 1);
    definition.points[nextPointIndex].dx = oldPointInNewCSys[0];
    definition.points[nextPointIndex].dy = oldPointInNewCSys[1];
    definition.points[nextPointIndex].dz = oldPointInNewCSys[2];
    return definition;
}

function addPointsToSegment(context is Context, index is number, definition is map) returns map
{
    var processedData = processRawPoints(context, definition);
    const points = extractPositions(processedData.orderedPoints);
    const start = points[definition.segmentIndex];
    const end = points[definition.segmentIndex + 1];
    var startPoint = processedData.orderedPoints[definition.segmentIndex];
    const step = (fromWorld(startPoint.cSys, end) - fromWorld(startPoint.cSys, start)) / (definition.numPointsToAdd + 1);
    var newPoints = makeArray(definition.numPointsToAdd, startPoint);
    const isStartPointRelative = startPoint.referenceType == ReferenceType.RELATIVE;
    for (var i = 0; i < definition.numPointsToAdd; i += 1)
    {
        // If the start point is relative, all new points are relative, so they just need their offset to be step, not (i + 1) * step.
        if (isStartPointRelative)
        {
            newPoints[i].dx = step[0];
            newPoints[i].dy = step[1];
            newPoints[i].dz = step[2];
        }
        else
        {
            const currentPoint = (i + 1) * step;
            newPoints[i].dx += currentPoint[0];
            newPoints[i].dy += currentPoint[1];
            newPoints[i].dz += currentPoint[2];
        }
    }

    definition = changeRelativeEndPointOffset(definition, processedData, end, start + definition.numPointsToAdd * (end - start) / (definition.numPointsToAdd + 1));

    definition = addNewPoints(context, definition, newPoints, false, definition.segmentIndex);

    definition.segmentIndex = -1;

    return definition;
}

//==================================================================
//=========================== Ortho path ===========================
//==================================================================

function getOrthoSolutionNumber(context is Context, definition is map) returns number
{
    if (definition.segmentIndex < 0)
    {
        return 0;
    }
    const processedData = processRawPoints(context, definition);
    const cSys = getOrthoCSys(context, definition, processedData.baseCSys);
    const positions = extractPositions(processedData.orderedPoints);
    const point1 = fromWorld(cSys, positions[definition.segmentIndex]);
    const point2 = fromWorld(cSys, positions[definition.segmentIndex + 1]);
    var numEquals = 0;
    for (var i = 0; i < 3; i += 1)
    {
        if (tolerantEquals(point1[i], point2[i]))
        {
            numEquals += 1;
        }
    }
    return switch (numEquals)
        {
                0 : 6, // If the points are not aligned on any axis, there are 3! = 6 solutions
                1 : 2, // If the points are aligned on one axis, there are 2! = 2 solutions
                2 : 1,
                3 : 0
            };
}


function showCurrentOrthoSolution(context is Context, id is Id, pointLocations is array, definition is map, baseCSys is CoordSystem)
{
    const cSys = getOrthoCSys(context, definition, baseCSys);
    showCSys(context, cSys);
    if (definition.segmentIndex < 0)
    {
        return;
    }
    const point1 = pointLocations[definition.segmentIndex];
    const point2 = pointLocations[definition.segmentIndex + 1];
    const solution = getCurrentOrthoSolution(context, id, fromWorld(cSys, point1), fromWorld(cSys, point2), definition.solutionIndex);

    const solutionSize = size(solution);

    if (solutionSize == 0)
    {
        return;
    }

    const solutionPoints = concatenateArrays([[point1], mapArray(solution, function(point)
                {
                    return toWorld(cSys, point);
                }), [point2]]);
    showPolyline(context, solutionPoints, DebugColor.GREEN);
}

function getCurrentOrthoSolution(context is Context, id is Id, p1 is Vector, p2 is Vector, solution is number) returns array
{
    const p1ToP2 = p2 - p1;
    var zeros = [tolerantEqualsZero(p1ToP2[0]), tolerantEqualsZero(p1ToP2[1]), tolerantEqualsZero(p1ToP2[2])];

    if ((zeros[0] && zeros[1]) || (zeros[0] && zeros[2]) || (zeros[1] && zeros[2]))
    {
        // The points are already aligned on two axis in the given csys.
        throw regenError(ErrorStringEnum.ROUTING_CURVE_ORTHO_PATH_ALREADY_AXIS_ALIGNED, ["orthoCsys", "solutionIndex"]);
        return [];
    }

    const deltas = [
            vector(p1ToP2[0], 0 * meter, 0 * meter),
            vector(0 * meter, p1ToP2[1], 0 * meter),
            vector(0 * meter, 0 * meter, p1ToP2[2])
        ];

    if (indexOf(zeros, true) != -1)
    {
        if (solution > 1)
        {
            // If the points are aligned on 1 axis, there are only 2 solutions.
            // We shouldn't be able to get here but return nothing just in case.
            return [];
        }
        const deltaIndex = getSecondSolutionDeltaIndex(zeros, solution);
        return [p1 + deltas[deltaIndex]];
    }
    else
    {
        const firstDeltaIndex = floor(solution / 2);
        const firstPoint = p1 + deltas[firstDeltaIndex];
        zeros[firstDeltaIndex] = true;
        const secondDeltaIndex = getSecondSolutionDeltaIndex(zeros, solution % 2);
        return [firstPoint, firstPoint + deltas[secondDeltaIndex]];
    }
}

// This assumes solutionIndex is 0 or 1 and that zeros has one true and two false.
function getSecondSolutionDeltaIndex(zeros is array, solutionIndex is number) returns number
{
    var j = 0;
    for (var i = 0; i < 3; i += 1)
    {
        if (zeros[i])
        {
            continue;
        }
        if (j == solutionIndex)
        {
            return i;
        }
        j += 1;
    }
    // Should never happen.
    return -1;
}

function getOrthoCSys(context is Context, definition is map, baseCSys is CoordSystem) returns CoordSystem
{
    return switch (definition.orthoCsys)
        {
                OrthoCoordSystem.WORLD : WORLD_COORD_SYSTEM,
                OrthoCoordSystem.CURVE : baseCSys,
                OrthoCoordSystem.OTHER : processCSys(context, definition.orthoCsysMate)
            };
}

function addOrthoPathPoints(context is Context, id is Id, definition is map) returns map
{
    if (size(definition.points) < 2 || definition.segmentIndex < 0)
    {
        return definition;
    }

    var processedData = processRawPoints(context, definition);
    const cSys = getOrthoCSys(context, definition, processedData.baseCSys);
    const startPoint = processedData.orderedPoints[definition.segmentIndex];
    const point1 = startPoint.position;
    const point2 = processedData.orderedPoints[definition.segmentIndex + 1].position;
    var solution = getCurrentOrthoSolution(context, id, fromWorld(cSys, point1), fromWorld(cSys, point2), definition.solutionIndex);

    const solutionSize = size(solution);
    if (solutionSize == 0)
    {
        return definition;
    }
    solution = mapArray(solution, function(solutionPoint)
        {
            return toWorld(cSys, solutionPoint);
        });
    const isStartPointRelative = startPoint.referenceType == ReferenceType.RELATIVE;
    var newPoints = makeArray(solutionSize, startPoint);
    var solutionCSys = startPoint.cSys;
    if (isStartPointRelative)
    {
        solutionCSys.origin = point1;
    }
    for (var i = 0; i < solutionSize; i += 1)
    {
        if (isStartPointRelative && i > 0)
        {
            solutionCSys.origin = solution[i - 1];
        }
        const solutionPointInCSys = fromWorld(solutionCSys, solution[i]);
        newPoints[i].dx = solutionPointInCSys[0];
        newPoints[i].dy = solutionPointInCSys[1];
        newPoints[i].dz = solutionPointInCSys[2];
    }

    definition = changeRelativeEndPointOffset(definition, processedData, point2, solution[solutionSize - 1]);

    definition = addNewPoints(context, definition, newPoints, false, definition.segmentIndex);

    definition.solutionIndex = 0;
    definition.segmentIndex = -1;

    return definition;
}

//==================================================================
//======================== Data gathering ==========================
//==================================================================

// Takes the raw unprocessed definition, returns a map with an orderedPoints array and a baseCSys field.
// orderedPoints is the same as points, but ordered by point index, with each point having the following additional fields:
// - cSys: the coordinate system used to process the point's offset.
// - position: the position of the point in the world coordinate system.
function processRawPoints(context is Context, definition is map) returns map
{
    const numPoints = size(definition.points);
    var orderedPoints = makeArray(numPoints);
    for (var point in definition.points)
    {
        orderedPoints[point.index] = point;
    }
    const baseCSys = processCSys(context, definition.curveBaseCSys);
    var queriesToComputedPaths = {};
    var invalidPathsParameters = [];
    const invalidPathPrefix = "points[";
    const invalidPathSuffix = "].curve";
    for (var i = 0; i < numPoints; i += 1)
    {
        var point = orderedPoints[i];
        point.cSys = baseCSys;
        if (point.referenceType == ReferenceType.RELATIVE && i > 0)
        {
            point.cSys = orderedPoints[i - 1].cSys;
            point.cSys.origin = orderedPoints[i - 1].position;
        }
        else if (point.referenceType == ReferenceType.CURVE)
        {
            const edgesQuery = qUnion([point.curve->qEntityFilter(EntityType.BODY)->qOwnedByBody(EntityType.EDGE), point.curve->qEntityFilter(EntityType.EDGE)]);
            if (isQueryEmpty(context, edgesQuery))
            {
                throw regenError(ErrorStringEnum.ROUTING_CURVE_SELECT_CURVE, [invalidPathPrefix ~ toString(i) ~ invalidPathSuffix]);
            }
            var path = queriesToComputedPaths[edgesQuery];
            if (path == undefined)
            {
                try silent
                {
                    path = constructPath(context, edgesQuery);
                }
                catch
                {
                    invalidPathsParameters = append(invalidPathsParameters, invalidPathPrefix ~ toString(i) ~ invalidPathSuffix);
                    point.position = WORLD_ORIGIN; // In case there's a relative point afterwards
                    orderedPoints[i] = point;
                    continue;
                }
                queriesToComputedPaths[edgesQuery] = path;
            }
            const tangentResult = evPathTangentLines(context, path, [point.curveParameter]);
            if (point.alignCSys)
            {
                const edgeQuery = path.edges[tangentResult.edgeIndices[0]];
                const edgeParameter = evDistance(context, {
                                "side0" : edgeQuery,
                                "side1" : tangentResult.tangentLines[0].origin
                            }).sides[0].parameter;
                point.cSys = evEdgeCurvature(context, {
                                "edge" : edgeQuery,
                                "parameter" : edgeParameter
                            }).frame;
                if (path.flipped[tangentResult.edgeIndices[0]] != point.flipCSys) // Only flip if only one is true
                {
                    point.cSys.zAxis *= -1;
                }
            }
            else
            {
                point.cSys.origin = tangentResult.tangentLines[0].origin;
            }
        }
        else if (point.referenceType == ReferenceType.VERTEX)
        {
            point.cSys = processCSys(context, point.vertex, baseCSys);
        }

        point.position = toWorld(point.cSys, vector(point.dx, point.dy, point.dz));

        orderedPoints[i] = point;
    }
    if (invalidPathsParameters != [])
    {
        throw regenError(ErrorStringEnum.ROUTING_CURVE_INVALID_PATH, invalidPathsParameters);
    }
    return { "baseCSys" : baseCSys, "orderedPoints" : orderedPoints };
}

function extractPositions(points is array) returns array
{
    return mapArray(points, function(point)
        {
            return point.position;
        });
}

function extractBendRadii(points is array, globalRadius is ValueWithUnits) returns array
{
    var radii = mapArray(subArray(points, 1, size(points) - 1), function(point)
    {
        return point.overrideBendRadius ? point.pointBendRadius : globalRadius;
    });
    const lastPointIndex = size(points) - 1;
    if (tolerantEquals(points[0].position, points[lastPointIndex].position))
    {
        radii = append(radii, points[lastPointIndex].overrideBendRadius ? points[lastPointIndex].pointBendRadius : globalRadius);
    }
    return radii;
}

function extractDerivatives(context is Context, points is array) returns map
{
    var derivatives = {};
    for (var i = 0; i < size(points); i += 1)
    {
        const point = points[i];
        if (!point.hasDerivative)
        {
            continue;
        }
        const transformLinear = toWorld(point.cSys).linear;
        const axis = switch (point.derivativeAlignment)
            {
                    DerivativeAlignment.X : transformLinear * vector(transpose(matrix(point.rotationMatrix))[0]),
                    DerivativeAlignment.Y : transformLinear * vector(transpose(matrix(point.rotationMatrix))[1]),
                    DerivativeAlignment.Z : transformLinear * vector(transpose(matrix(point.rotationMatrix))[2]),
                    DerivativeAlignment.DIRECTION : extractDerivativeDirection(context, point.derivativeDirection)
                };
        const derivative = axis * point.magnitude * (point.oppositeDirection ? -1 : 1);
        if (!tolerantEqualsZero(derivative))
        {
            derivatives[i] = derivative;
        }
    }
    return derivatives;
}

function extractDerivativeDirection(context is Context, direction is Query) returns Vector
{
    if (isQueryEmpty(context, direction))
    {
        return vector(0, 0, 0) * meter;
    }
    const derivativeDirection = extractDirection(context, direction);
    if (derivativeDirection == undefined)
    {
        return vector(0, 0, 0) * meter;
    }
    return derivativeDirection;
}

function cleanupPoints(points is array) returns array
{
    var removedPoints = {};
    const numPoints = size(points);
    for (var i = 1; i < numPoints; i += 1)
    {
        if (!tolerantEquals(points[i - 1].position, points[i].position))
        {
            continue;
        }
        removedPoints[i] = true;
    }
    if (removedPoints == {})
    {
        return points;
    }
    var cleanIndex = 0;
    var cleanPoints = makeArray(numPoints - size(removedPoints));
    for (var i = 0; i < numPoints; i += 1)
    {
        if (removedPoints[i] != undefined)
        {
            continue;
        }
        cleanPoints[cleanIndex] = points[i];
        cleanIndex += 1;
    }
    return cleanPoints;
}

//==================================================================
//=========================== Edit logic ===========================
//==================================================================

/**
 * Edit logic function for routing curve.
 */
export function routingCurveEditLogic(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean, specifiedParameters is map, hiddenQueries is Query, clickedButton is string) returns map
{
    if (oldDefinition == {})
    {
        return processPreselections(context, definition);
    }

    if (clickedButton == "resetTriad")
    {
        return resetTriad(definition, getArrayIndexOfPointWithIndex(definition.points, definition.pointIndex));
    }

    if (clickedButton == "confirmOrthoPath")
    {
        return addOrthoPathPoints(context, id, definition);
    }
    if (clickedButton == "orthoPrevious" || clickedButton == "orthoNext")
    {
        const numSolutions = getOrthoSolutionNumber(context, definition);
        if (numSolutions > 1)
        {
            const offset = clickedButton == "orthoPrevious" ? -1 : 1;
            definition.solutionIndex = (definition.solutionIndex + offset) % numSolutions;
        }
        return definition;
    }

    const segmentToPoint = oldDefinition.step == CurveStep.SEGMENTS && definition.step == CurveStep.POINTS;
    const pointToSegment = oldDefinition.step == CurveStep.POINTS && definition.step == CurveStep.SEGMENTS;
    if (pointToSegment)
    {
        var references = [definition.curveBaseCSys];
        for (var point in definition.points)
        {
            if (point.referenceType == ReferenceType.VERTEX)
            {
                references = append(references, point.vertex);
            }
            else if (point.referenceType == ReferenceType.CURVE)
            {
                references = append(references, point.curve);
                if (definition.curveType == RoutingCurveType.INTERPOLATED_SPLINE && point.hasDerivative && point.derivativeAlignment == DerivativeAlignment.DIRECTION)
                {
                    references = append(references, point.derivativeDirection);
                }
            }
        }
        definition.hiddenReferences = qUnion(references);
    }
    if (segmentToPoint)
    {
        definition.hiddenReferences = qNothing();
    }
    if (segmentToPoint || (oldDefinition.segmentEditType == SegmentEditType.ORTHOGONAL && definition.segmentEditType != SegmentEditType.ORTHOGONAL))
    {
        // If the user was in the segment step and was doing an Ortho segment edit, and changed to an ADD segment edit or to the points step,
        // then we reset the segment index.
        definition.segmentIndex = -1;
        return definition;
    }

    return pointStepEditLogic(context, id, oldDefinition, definition, clickedButton);
}

function processPreselections(context is Context, definition is map) returns map
{
    const vertices = definition.preselectedEntities->qEntityFilter(EntityType.VERTEX);
    const wires = definition.preselectedEntities->qEntityFilter(EntityType.BODY)->qBodyType(BodyType.WIRE);
    const edges = definition.preselectedEntities->qEntityFilter(EntityType.EDGE);
    if (!isQueryEmpty(context, vertices))
    {
        definition.inputVertices = vertices;
        return processNewVertices(context, definition);
    }
    else if (!isQueryEmpty(context, wires) || !isQueryEmpty(context, edges))
    {
        definition.inputCurve = qUnion([wires, edges]);
        definition.inputType = InputType.CURVE;
    }
    definition.points = [emptyPoint];
    return definition;
}

function resetTriad(definition is map, index is number) returns map
{
    definition.points[index].rotationMatrix = identityMatrix(3);
    return definition;
}

// We assume that indices contains only unique values (see deduplicateIndices)
function shiftIndices(indices is array) returns array
{
    const numIndices = size(indices);
    const sortedIndices = sort(indices, function(a, b)
        {
            return a - b;
        });
    var missingIndices = [];
    var expectedIndex = 0;
    for (var index in sortedIndices)
    {
        while (index > expectedIndex)
        {
            missingIndices = append(missingIndices, expectedIndex);
            expectedIndex += 1;
        }
        expectedIndex += 1;
    }
    const numMissingIndicies = size(missingIndices);
    for (var i = 0; i < numIndices; i += 1)
    {
        var diff = 0;
        while (diff < numMissingIndicies && missingIndices[diff] < indices[i])
        {
            diff += 1;
        }
        indices[i] -= diff;
    }
    return indices;
}

// The heuristic for deduplication is: the first time we find an index,
// it has priority and subsequent instances of the same index will be changed.
function deduplicateIndices(indices is array) returns array
{
    var seenIndices = {};
    const numIndices = size(indices);
    for (var i = 0; i < numIndices; i += 1)
    {
        const index = indices[i];
        if (seenIndices[index] == undefined)
        {
            seenIndices[index] = true;
            continue;
        }
        for (var j = 0; j < numIndices; j += 1)
        {
            // Before i, there is a single instance of indices[i], we don't want to change it.
            // If there are more instances of indices[i] after i, we do want to change them.
            if (indices[j] > index || (indices[j] == index && j >= i))
            {
                indices[j] += 1;
            }
        }
        seenIndices[indices[i]] = true;
    }
    return indices;
}

function processAddedOrRemovedPoints(oldDefinition is map, definition is map) returns map
{
    // In case the current point has been deleted, we use the previous current point
    const index = getArrayIndexOfPointWithIndex(oldDefinition.points, oldDefinition.pointIndex);
    const currentPoint = oldDefinition.points[index];
    // If there are points added by the array button, we need to process them: we copy the currently selected point to the new points.
    const newNumPoints = size(definition.points);
    const indexString = toString(index);
    var numAddedPoints = 0;
    for (var i = 0; i < newNumPoints; i += 1)
    {
        if (!definition.points[i].arrayAdded)
        {
            continue;
        }
        numAddedPoints += 1;
        // currentPoint comes from the previous state, so its arrayAdded is already false.
        definition.points[i] = currentPoint;
        // This allows us to copy the variables into the new point.
        for (var paramToCopy in PARAMETERS_TO_COPY)
        {
            definition.points[i][paramToCopy] = copyParameter("points[" ~ indexString ~ "]." ~ paramToCopy);
        }
        if (currentPoint.referenceType == ReferenceType.RELATIVE)
        {
            definition.points[i].dx = 0 * meter;
            definition.points[i].dy = 0 * meter;
            definition.points[i].dz = 0 * meter;
        }
    }
    definition.pointIndex += numAddedPoints;
    const currentIndicesOrder = mapArray(definition.points, point => point.index);
    const newIndicesOrder = shiftIndices(deduplicateIndices(currentIndicesOrder));
    if (currentIndicesOrder != newIndicesOrder)
    {
        // There was some deletion or addition or both, we reorder the points and return.
        for (var i = 0; i < newNumPoints; i += 1)
        {
            definition.points[i].index = newIndicesOrder[i];
        }
    }
    if (definition.pointIndex >= newNumPoints)
    {
        definition.pointIndex = newNumPoints - 1;
    }
    return definition;
}

function pointStepEditLogic(context is Context, id is Id, oldDefinition is map, definition is map, clickedButton is string) returns map
{
    if (definition.step != CurveStep.POINTS || oldDefinition.step != CurveStep.POINTS)
    {
        return definition;
    }
    // First we check if the points array has changed in size or order
    if (definition.points == [])
    {
        definition.pointIndex = 0;
        definition.points = [emptyPoint];
        return definition;
    }
    const currentIndicesOrder = mapArray(definition.points, point => point.index);
    const newIndicesOrder = shiftIndices(deduplicateIndices(currentIndicesOrder));
    if (mapArray(oldDefinition.points, point => point.index) != currentIndicesOrder || currentIndicesOrder != newIndicesOrder)
    {
        return processAddedOrRemovedPoints(oldDefinition, definition);
    }
    const index = getArrayIndexOfPointWithIndex(definition.points, definition.pointIndex);
    // If we're still here, the points order hasn't changed so something else has.
    // There's a new input vertex: process it and clear the input field.
    if (!isQueryEmpty(context, definition.inputVertices))
    {
        return processNewVertices(context, definition);
    }
    // The processInputs button was clicked: process the edge/csv input and clear the input field.
    if (clickedButton == "processInputs")
    {
        return processNonVertexInputs(context, definition);
    }
    // The current point's reference type has changed: change the offset so that the point doesn't move.
    if (definition.points[index].referenceType != oldDefinition.points[index].referenceType)
    {
        if (definition.points[index].referenceType == ReferenceType.RELATIVE && definition.pointIndex == 0)
        {
            return definition;
        }
        return matchNewPointToOldPointInNewCSys(context, oldDefinition, definition, index);
    }
    // The point's vertex has changed: if there is a vertex, reset the offset, otherwise change the offset so the point doesn't move.
    if (definition.points[index].referenceType == ReferenceType.VERTEX && (!areQueriesEquivalent(context, definition.points[index].vertex, oldDefinition.points[index].vertex)))
    {
        if (!isQueryEmpty(context, definition.points[index].vertex))
        {
            definition.points[index].dx = 0 * meter;
            definition.points[index].dy = 0 * meter;
            definition.points[index].dz = 0 * meter;
        }
        else
        {
            definition = matchNewPointToOldPointInNewCSys(context, oldDefinition, definition, index);
        }
        return definition;
    }
    // If the current point has a curve reference type and its alignment has changed, we need to match the new offset to the old point.
    if (definition.points[index].alignCSys != oldDefinition.points[index].alignCSys || definition.points[index].flipCSys != oldDefinition.points[index].flipCSys)
    {
        return matchNewPointToOldPointInNewCSys(context, oldDefinition, definition, index);
    }
    return definition;
}

function matchNewPointToOldPointInNewCSys(context is Context, oldDefinition is map, definition is map, arrayParameterIndex is number) returns map
{
    var oldProcessedData;
    var newProcessedData;
    try silent {
        oldProcessedData = processRawPoints(context, oldDefinition);
        newProcessedData = processRawPoints(context, definition);
    }
    catch
    {
        definition.points[arrayParameterIndex].dx = 0 * meter;
        definition.points[arrayParameterIndex].dy = 0 * meter;
        definition.points[arrayParameterIndex].dz = 0 * meter;
        return definition;
    }
    // The goal is to keep the position we had in oldDefinition
    const oldPointInNewCSys = fromWorld(newProcessedData.orderedPoints[definition.pointIndex].cSys, oldProcessedData.orderedPoints[definition.pointIndex].position);
    definition.points[arrayParameterIndex].dx = oldPointInNewCSys[0];
    definition.points[arrayParameterIndex].dy = oldPointInNewCSys[1];
    definition.points[arrayParameterIndex].dz = oldPointInNewCSys[2];
    return definition;
}

//==================================================================
//======================= Geometry creation ========================
//==================================================================

function createPoints(context is Context, id is Id, points is array)
{
    for (var i = 0; i < size(points); i += 1)
    {
        opPoint(context, id + "point" + i, { "point" : points[i] });
    }
}

function makePolyline(context is Context, id is Id, points is array, bendRadii is array)
{
    @opPolyline(context, id + "polyline", { "points" : points, "bendRadii" : bendRadii, "showError" : true });
}

function makeInterpolatedSpline(context is Context, id is Id, points is array, derivatives is map)
{
    var splineDefinition = {
        "points" : points
    };
    if (derivatives != {})
    {

        if (derivatives[0] != undefined)
        {
            splineDefinition.startDerivative = derivatives[0];
            derivatives[0] = undefined;
        }
        const lastPointIndex = size(points) - 1;
        if (derivatives[lastPointIndex] != undefined)
        {
            splineDefinition.endDerivative = derivatives[lastPointIndex];
            derivatives[lastPointIndex] = undefined;
        }
        splineDefinition.derivatives = derivatives;
    }

    opFitSpline(context, id + "fitSpline", splineDefinition);
}

//==================================================================
//======================== Display utlities ========================
//==================================================================

function showCSys(context is Context, cSys is CoordSystem)
{
    const ARROW_LENGTH = 0.05 * meter;
    const ARROW_RADIUS = 0.05 * ARROW_LENGTH;
    addDebugArrow(context, cSys.origin, cSys.origin + cSys.xAxis * ARROW_LENGTH, ARROW_RADIUS, DebugColor.RED);
    addDebugArrow(context, cSys.origin, cSys.origin + yAxis(cSys) * ARROW_LENGTH, ARROW_RADIUS * (2 / 3), DebugColor.GREEN);
    addDebugArrow(context, cSys.origin, cSys.origin + cSys.zAxis * ARROW_LENGTH, ARROW_RADIUS * 0.5, DebugColor.BLUE);
}

function showControlPolyline(context is Context, points is array, highlightSegment is number)
{
    if (highlightSegment < 0)
    {
        showPolyline(context, points, DebugColor.MAGENTA);
    }
    else
    {
        showPolyline(context, subArray(points, 0, highlightSegment + 1), DebugColor.MAGENTA);
        addDebugLine(context, points[highlightSegment], points[highlightSegment + 1], DebugColor.CYAN);
        showPolyline(context, subArray(points, highlightSegment + 1), DebugColor.MAGENTA);
    }
}

function showPolyline(context is Context, points is array, color is DebugColor)
{
    for (var i = 0; i < size(points) - 1; i += 1)
    {
        if (tolerantEquals(points[i], points[i + 1]))
        {
            continue;
        }
        addDebugLine(context, points[i], points[i + 1], color);
        addDebugPoint(context, points[i], color);
    }
    addDebugPoint(context, points[size(points) - 1], color);
}

function showInterpolatedSplineDerivatives(context is Context, points is array, derivatives is map)
{
    for (var index, derivative in derivatives)
    {
        // We divide by 3 to get the same magnitude as 3d fit spline manipulators.
        addDebugArrow(context, points[index], points[index] + derivative / 3, .25 * centimeter, DebugColor.MAGENTA);
    }
}

//==================================================================
//========================= Other utlities =========================
//==================================================================

function getArrayIndexOfPointWithIndex(points is array, index is number) returns number
{
    for (var i = 0; i < size(points); i += 1)
    {
        if (index == points[i].index)
        {
            return i;
        }
    }
    return -1;
}

function getUnit(unit is LengthUnitNames) returns ValueWithUnits
{
    return switch (unit)
        {
                LengthUnitNames.Centimeter : centimeter,
                LengthUnitNames.Foot : foot,
                LengthUnitNames.Inch : inch,
                LengthUnitNames.Millimeter : millimeter,
                LengthUnitNames.Meter : meter,
                LengthUnitNames.Yard : yard
            };
}

function setCurveLength(context is Context, id is Id)
{
    const wireQuery = qCreatedBy(id, EntityType.BODY)->qBodyType(BodyType.WIRE);
    if (size(evaluateQuery(context, wireQuery)) != 1)
    {
        return;
    }
    const wireLength = evLength(context, {
                "entities" : wireQuery
            });
    setFeatureComputedParameter(context, id, {
                "name" : "curveLength",
                "value" : wireLength
            });
}

