FeatureScript 2180; /* Automatically generated version */
import(path : "onshape/std/containers.fs", version : "2180.0");
import(path : "onshape/std/coordSystem.fs", version : "2180.0");
import(path : "onshape/std/curveGeometry.fs", version : "2180.0");
import(path : "onshape/std/evaluate.fs", version : "2180.0");
import(path : "onshape/std/feature.fs", version : "2180.0");
import(path : "onshape/std/manipulator.fs", version : "2180.0");
import(path : "onshape/std/math.fs", version : "2180.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2180.0");
import(path : "onshape/std/valueBounds.fs", version : "2180.0");
import(path : "onshape/std/vector.fs", version : "2180.0");
import(path : "onshape/std/debug.fs", version : "2180.0");

/**
 * Specifies how the bridging curve will match the vertex or edge at each side
 * @value POSITION : The bridging curve will end at the provided vertex. Direction of the curve is unspecified
 * @value TANGENCY : The bridging curve will end at the vertex and the curve will be tangent to the edge
 * @value CURVATURE : The bridging curve will end at the vertex and the curve will have same curvature as the edge at the vertex
 */
export enum BridgingCurveMatchType
{
    annotation { "Name" : "Match position" }
    POSITION,
    annotation { "Name" : "Match tangent" }
    TANGENCY,
    annotation { "Name" : "Match curvature" }
    CURVATURE
}

/** @internal */
export enum BridgingCurveMethod
{
    annotation { "Name" : "Magnitude/bias" }
    MAGNITUDE_BIAS, // The legacy method
    annotation { "Name" : "Control points" }
    CONTROL_POINTS
}

/**
 * A `RealBoundSpec` for bias of a tangency/tangency bridge, defaulting to `0.5`.
 */
export const BIAS_BOUNDS =
{
    (unitless) : [0.0001, 0.5, 0.9999]
} as RealBoundSpec;

const UI_SCALING = 0.5;
const ANGLE_RANGE = 30 * degree;
const SIDE_1_MANIPULATOR = "Side1Manip";
const SIDE_2_MANIPULATOR = "Side2Manip";
const ANGLE_1_MANIPULATOR = "Angle1Manip";
const ANGLE_2_MANIPULATOR = "Angle2Manip";
const MAGNITUDE_MANIPULATOR = "magnitudeManipulator";
const CENTRAL_MAGNITUDE_MANIPULATOR = "centralMagnitudeManipulator";
const BIAS_MANIPULATOR = "biasManipulator";
const DEFAULT_BIAS_MINIMUM = 0.25;
const DEFAULT_G1G1_SCALE = 2 / 3;

function magnitudeManipulatorId(side is number) returns string
{
    return (side == 1 ? "Start" : "End") ~ " magnitude";
}

function curvatureManipulatorId(side is number) returns string
{
    return (side == 1 ? "Start" : "End") ~ " curvature offset";
}

/**
 * Creates a curve between two points, optionally with matching of tangency or curvature to other curves at that point
 */
annotation { "Feature Type Name" : "Bridging curve",
        "Editing Logic Function" : "bridgingCurveEditingLogic",
        "Manipulator Change Function" : "bridgingCurveManipulator",
        "UIHint" : UIHint.NO_PREVIEW_PROVIDED }
export const bridgingCurve = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Preselection", "UIHint" : UIHint.ALWAYS_HIDDEN, "Filter" : EntityType.EDGE || EntityType.VERTEX || (EntityType.FACE && ConstructionObject.NO) || BodyType.MATE_CONNECTOR }
        definition.preselectedEntities is Query;
        annotation { "Name" : "Start", "Filter" : EntityType.EDGE || EntityType.VERTEX || (EntityType.FACE && ConstructionObject.NO) || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 2 }
        definition.side1 is Query;
        annotation { "Name" : "Match", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE, "Default" : BridgingCurveMatchType.TANGENCY }
        definition.match1 is BridgingCurveMatchType;
        if (definition.match1 != BridgingCurveMatchType.POSITION)
        {
            annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.flip1 is boolean;
        }
        annotation { "Name" : "End", "Filter" : EntityType.EDGE || EntityType.VERTEX || (EntityType.FACE && ConstructionObject.NO) || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 2 }
        definition.side2 is Query;
        annotation { "Name" : "Match", "Column Name" : "Second match", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE, "Default" : BridgingCurveMatchType.TANGENCY }
        definition.match2 is BridgingCurveMatchType;
        if (definition.match2 != BridgingCurveMatchType.POSITION)
        {
            annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.flip2 is boolean;
        }
        if (definition.match1 != BridgingCurveMatchType.POSITION || definition.match2 != BridgingCurveMatchType.POSITION)
        {
            annotation { "Name" : "Method", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE], "Default" : BridgingCurveMethod.CONTROL_POINTS }
            definition.method is BridgingCurveMethod;
            if (definition.method == BridgingCurveMethod.CONTROL_POINTS)
            {
                annotation { "Name" : "Edit control points", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                definition.editControlPoints is boolean;
                if (definition.editControlPoints)
                {
                    annotation { "Group Name" : "Edit control points", "Collapsed By Default" : false, "Driving Parameter" : "editControlPoints" }
                    {
                        if (definition.match1 != BridgingCurveMatchType.POSITION)
                        {
                            annotation { "Name" : "Start magnitude" }
                            isReal(definition.side1Magnitude, POSITIVE_REAL_BOUNDS);
                            if (definition.match1 == BridgingCurveMatchType.CURVATURE)
                            {
                                annotation { "Name" : "Start curvature offset" }
                                isReal(definition.side1CurvatureOffset, CLAMP_MAGNITUDE_REAL_BOUNDS);
                            }
                        }
                        if (definition.match2 != BridgingCurveMatchType.POSITION)
                        {
                            annotation { "Name" : "End magnitude" }
                            isReal(definition.side2Magnitude, POSITIVE_REAL_BOUNDS);
                            if (definition.match2 == BridgingCurveMatchType.CURVATURE)
                            {
                                annotation { "Name" : "End curvature offset" }
                                isReal(definition.side2CurvatureOffset, CLAMP_MAGNITUDE_REAL_BOUNDS);
                            }
                        }
                    }
                }
            }
            else // Legacy controls
            {
                if (definition.match1 != BridgingCurveMatchType.POSITION || definition.match2 != BridgingCurveMatchType.POSITION)
                {
                    annotation { "Name" : "Magnitude" }
                    isReal(definition.magnitude, POSITIVE_REAL_BOUNDS);
                }
                if (definition.match1 != BridgingCurveMatchType.POSITION && definition.match2 != BridgingCurveMatchType.POSITION)
                {
                    annotation { "Name" : "Bias" }
                    isReal(definition.bias, BIAS_BOUNDS);
                }
            }
        }

        annotation { "Name" : "side1HasVertex", "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.side1HasVertex is boolean;
        annotation { "Name" : "side2HasVertex", "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.side2HasVertex is boolean;

        if (!definition.side1HasVertex || !definition.side2HasVertex)
        {
            annotation { "Name" : "Edit edge positions", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            definition.editEdgePositions is boolean;
            if (definition.editEdgePositions)
            {
                annotation { "Group Name" : "Edge positions", "Collapsed By Default" : false, "Driving Parameter" : "editEdgePositions" }
                {
                    if (!definition.side1HasVertex)
                    {
                        annotation { "Name" : "Start edge position" }
                        isReal(definition.startEdgeParameter, EDGE_PARAMETER_BOUNDS);
                    }
                    if (!definition.side2HasVertex)
                    {
                        annotation { "Name" : "End edge position" }
                        isReal(definition.endEdgeParameter, EDGE_PARAMETER_BOUNDS);
                    }
                }
            }
        }
        annotation { "Name" : "side1HasFace", "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.side1HasFace is boolean;
        annotation { "Name" : "side2HasFace", "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.side2HasFace is boolean;

        if ((definition.side1HasFace || definition.side2HasFace) && (definition.match1 != BridgingCurveMatchType.POSITION || definition.match2 != BridgingCurveMatchType.POSITION))
        {
            annotation { "Name" : "Edit tangency angles", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            definition.editTangencyAngles is boolean;
            if (definition.editTangencyAngles)
            {
                annotation { "Group Name" : "Edit angles", "Collapsed By Default" : false, "Driving Parameter" : "editTangencyAngles" }
                {
                    if (definition.match1 != BridgingCurveMatchType.POSITION && definition.side1HasFace)
                    {
                        annotation { "Name" : "Start angle" }
                        isAngle(definition.startAngle, ANGLE_180_MINUS_180_BOUNDS);
                    }
                    if (definition.match2 != BridgingCurveMatchType.POSITION && definition.side2HasFace)
                    {
                        annotation { "Name" : "End angle" }
                        isAngle(definition.endAngle, ANGLE_180_MINUS_180_BOUNDS);
                    }
                }
            }
        }
    }
    {
        verifyNoMesh(context, definition, "side1");
        verifyNoMesh(context, definition, "side2");
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V858_SM_FLAT_BUG_FIXES))
        {
            verifyNoSheetMetalFlatQuery(context, definition.side1, "side1", ErrorStringEnum.FLATTENED_SHEET_METAL_SKETCH_PROHIBTED);
            verifyNoSheetMetalFlatQuery(context, definition.side2, "side2", ErrorStringEnum.FLATTENED_SHEET_METAL_SKETCH_PROHIBTED);
        }
        var remainingTransform = getRemainderPatternTransform(context,
            {
                "references" : qUnion([definition.side1, definition.side2])
            });

        const sideData = getSideDataAndAddManipulators(context, definition, true, {"id" : id});
        var side1Data = sideData.side1;
        var side2Data = sideData.side2;

        if (definition.method == BridgingCurveMethod.MAGNITUDE_BIAS)
        {
            legacyBridgingCurve(context, id, definition, side1Data, side2Data);
        }
        else // Current code path
        {
            var side1 is BridgingSideData = getBridgingSideData(side1Data, definition.match1);
            var side2 is BridgingSideData = getBridgingSideData(side2Data, definition.match2);
            side1.speedScaleName = "side1Magnitude";
            side2.speedScaleName = "side2Magnitude";
            side1.speedScale = definition.side1Magnitude;
            side2.speedScale = definition.side2Magnitude;
            side1.curvatureOffsetScale = definition.side1CurvatureOffset;
            side2.curvatureOffsetScale = definition.side2CurvatureOffset;

            const controlPoints = computeBridgingControlPoints(side1, side2);
            if (definition.editControlPoints &&
                (definition.match1 != BridgingCurveMatchType.POSITION || definition.match2 != BridgingCurveMatchType.POSITION))
            {
                addManipulatorsForSide(context, id, controlPoints, 1, side1.degree, definition.flip1);
                addManipulatorsForSide(context, id, controlPoints, 2, side2.degree, definition.flip2);
                showControlPoints(context, id, controlPoints);
            }

            createBezierCurve(context, id, controlPoints);
        }
        showWire(context, id, qCreatedBy(id, EntityType.BODY));
        transformResultIfNecessary(context, id, remainingTransform);
    }, {
            'match1' : BridgingCurveMatchType.TANGENCY,
            'match2' : BridgingCurveMatchType.TANGENCY,
            'flip1' : false,
            'flip2' : false,
            'method' : BridgingCurveMethod.MAGNITUDE_BIAS,
            'preselectedEntities' : qNothing(),
            'side1HasVertex' : false,
            'side2HasVertex' : false,
            'side1HasFace' : true,
            'side2HasFace' : true,
            'editEdgePositions' : false,
            'editTangencyAngles' : false
        }
    );

/**
 * Data type for side queries
 *
 *  @type {{
 *      @field vertex {Query} : The vertex element on a side query @optional
 *      @field edge {Query} : The edge element on a side query @optional
 *      @field face {Query} : The face element on a side query @optional
 * }}
 */
export type SideQueries typecheck canBeSideQueries;

predicate canBeSideQueries(value)
{
    value is map;
    value.vertex == undefined || value.vertex is Query;
    value.edge == undefined || value.edge is Query;
    value.face == undefined || value.face is Query;
    value.position == undefined || value.position is Vector;
    value.tangent == undefined || value.tangent is Vector;
}

/**
 * Data type for precomputed side data
 *
 *  @type {{
 *      @field point {Vector} : The position
 *      @field frame {EdgeCurvatureResult} : The coordSystem and curvature at the given position @optional
 * }}
 */
export type SideData typecheck canBeSideData;

predicate canBeSideData(value)
{
    value is map;
    isLengthVector(value.point);
    value.frame == undefined || value.frame is EdgeCurvatureResult;
}

/**
 * Aggregation of both sides SideData
 *
 *  @type {{
 *      @field side1 {SideData} : SideData map for start side
 *      @field side2 {SideData} : SideData map for end side
 * }}
 */
export type TwoSidesData typecheck canBeTwoSidesData;

predicate canBeTwoSidesData(value)
{
    value is map;
    value.side1 is SideData;
    value.side2 is SideData;
}

/**
 * Data type for unified control points computation
 *
 *  @type {{
 *      @field degree {number} : 0 for positional continuity (G0), 1 for tangent continuity (G1), 2 for curvature continuity (G2)
 *      @field position {Vector} : The position
 *      @field tangent {Vector} : @requiredif { `degree` >= 1 } The tangent direction vector
 *      @field speedScale {number} : How much to scale the default speed @optional
 *      @field curvatureDirection {Vector} : @requiredif { `degree` == 2 } The curvature direction vector
 *      @field curvature {ValueWithUnits} : @requiredif { `degree` == 2 } The curvature magnitude (inverse length units)
 *      @field curvatureOffsetScale {number} : How much to scale the default third control point offset @optional
 * }}
 */
export type BridgingSideData typecheck canBeBridgingSideData;

predicate canBeBridgingSideData(value)
{
    value is map;
    value.degree == 0 || value.degree == 1 || value.degree == 2;
    isLengthVector(value.position);
    if (value.degree >= 1) // G1 or higher
    {
        is3dDirection(value.tangent);
        value.speedScale == undefined || value.speedScale is number;
    }
    if (value.degree >= 2) // G2
    {
        is3dDirection(value.curvatureDirection);
        value.curvature is ValueWithUnits;
        value.curvatureOffsetScale == undefined || value.curvatureOffsetScale is number;
    }
}

function makeSideQueries(context is Context, side is Query) returns SideQueries
{
    const newVersion is boolean = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2150_BRIDGING_CURVE_MC_TANGENTS);
    var sideQueries is SideQueries = {} as SideQueries;
    const mateConnector = qBodyType(side, BodyType.MATE_CONNECTOR);
    if (size(evaluateQuery(context, mateConnector)) == 1)
    {
        const frame = evMateConnector(context, {
                "mateConnector" : mateConnector
        });
        sideQueries.position = frame.origin;
        if (newVersion)
            sideQueries.tangent = frame.zAxis;
    }
    const vertex = qEntityFilter(side, EntityType.VERTEX)->qSubtraction(newVersion ? mateConnector : qNothing());
    if (size(evaluateQuery(context, vertex)) == 1)
    {
        sideQueries.vertex = vertex;
        sideQueries.position = evVertexPoint(context, {
                "vertex" : vertex
        });
    }
    const edge = qEntityFilter(side, EntityType.EDGE);
    if (size(evaluateQuery(context, edge)) == 1)
    {
        sideQueries.edge = edge;
    }
    const face = qEntityFilter(side, EntityType.FACE);
    if (size(evaluateQuery(context, face)) == 1)
    {
        sideQueries.face = face;
    }
    return sideQueries;
}

function checkFaceContainsEdge(context is Context, face is Query, edge is Query) returns boolean
{
    var results = evEdgeCurvatures(context, {
                                    "edge" : edge,
                                    "parameters" : [0, 0.3, 0.7, 1]
    });
    for (var result in results)
    {
        if (isQueryEmpty(context, qContainsPoint(face, result.frame.origin)))
        {
            return false;
        }
    }
    return true;
}

function checkSideIsValid(context is Context, sideQueries is SideQueries, sideName is string, sideErrorMessage is string)
{
    if (sideQueries.position == undefined && sideQueries.edge == undefined && sideQueries.face == undefined)
    {
        throw regenError(sideErrorMessage, [sideName]);
    }
    // We need at least a vertex or an edge on each side.
    if (sideQueries.position == undefined && sideQueries.edge == undefined)
    {
        throw regenError(ErrorStringEnum.BRIDGING_CURVE_VERTEX_OR_EDGE_ON_SIDE, [sideName]);
    }
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1920_CONNECTING_CURVE_BETTER_INPUT_CHECKS) && sideQueries.face != undefined)
    {
        // We need to make sure the face contains the vertex or edge
        if (sideQueries.position != undefined)
        {
            if (isQueryEmpty(context, qContainsPoint(sideQueries.face, sideQueries.position)))
            {
                throw regenError(ErrorStringEnum.BRIDGING_CURVE_VERTEX_BELONG_TO_FACE, [sideName]);
            }
        }
        else if (sideQueries.edge != undefined)
        {
            // Either the edge is a face edge or it's coincident with the face.
            var adjacentEdges = qAdjacent(sideQueries.face, AdjacencyType.EDGE, EntityType.EDGE);
            if (isQueryEmpty(context, qIntersection([adjacentEdges, sideQueries.edge])))
            {
                if (!checkFaceContainsEdge(context, sideQueries.face, sideQueries.edge))
                {
                    throw regenError(ErrorStringEnum.BRIDGING_CURVE_EDGE_BELONG_TO_FACE, [sideName]);
                }
            }
        }
    }
}

// id should be {"id" : id} if addManip is true, {} otherwise.
// Returns { s}
function getSideDataAndAddManipulators(context is Context, definition is map, addManip is boolean, id is map) returns TwoSidesData
{
    var side1Data;
    var side2Data;

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1914_BRIDGING_CONNECTING_CURVE))
    {
        const sideQueries1 = makeSideQueries(context, definition.side1);
        const sideQueries2 = makeSideQueries(context, definition.side2);

        checkSideIsValid(context, sideQueries1, "side1", ErrorStringEnum.BRIDGING_CURVE_NO_START_SELECTION);
        checkSideIsValid(context, sideQueries2, "side2", ErrorStringEnum.BRIDGING_CURVE_NO_END_SELECTION);

        if (addManip && definition.editEdgePositions && sideQueries1.position == undefined && sideQueries1.edge != undefined)
        {
            addTangentManipulator(context, id.id, SIDE_1_MANIPULATOR, sideQueries1.edge, definition.startEdgeParameter, "startEdgeParameter");
        }
        if (addManip && definition.editEdgePositions && sideQueries2.position == undefined && sideQueries2.edge != undefined)
        {
            addTangentManipulator(context, id.id, SIDE_2_MANIPULATOR, sideQueries2.edge, definition.endEdgeParameter, "endEdgeParameter");
        }
        var startParam = definition.editEdgePositions ? definition.startEdgeParameter : 0.5;
        var endParam = definition.editEdgePositions ? definition.endEdgeParameter : 0.5;
        const points = getPointsLocations(context, sideQueries1, startParam, sideQueries2, endParam, definition.editEdgePositions);
        const point1 = points.point1;
        const point2 = points.point2;

        var startAngle = definition.editTangencyAngles ? definition.startAngle : 0 * degree;
        var endAngle = definition.editTangencyAngles ? definition.endAngle : 0 * degree;

        side1Data = getDataForSide(context, sideQueries1, definition.editEdgePositions, startParam, startAngle, definition.match1, definition.flip1, "side1", point1, point2);
        side2Data = getDataForSide(context, sideQueries2, definition.editEdgePositions, endParam, endAngle, definition.match2, definition.flip2, "side2", point2, point1);

        if (definition.editTangencyAngles)
        {
            const midLength = norm(side1Data.point - side2Data.point) / 2;
            if (definition.match1 != BridgingCurveMatchType.POSITION && side1Data.frame != undefined && !isQueryEmpty(context, qEntityFilter(definition.side1, EntityType.FACE)))
            {
                const frame1 = side1Data.frame.frame;
                if (addManip)
                {
                    // getDataForSide returns the frame rotated by the angle, so we need to unrotate the rotation origin
                    addManipulators(context, id.id, {
                                (ANGLE_1_MANIPULATOR) : angularManipulator({
                                        "axisOrigin" : frame1.origin,
                                        "axisDirection" : frame1.xAxis,
                                        "rotationOrigin" : frame1.origin + rotationMatrix3d(frame1.xAxis, - startAngle) * frame1.zAxis * midLength,
                                        "angle" : definition.startAngle,
                                        "primaryParameterId" : "startAngle"
                                    })
                                });
                }
            }
            if (definition.match2 != BridgingCurveMatchType.POSITION && side2Data.frame != undefined && !isQueryEmpty(context, qEntityFilter(definition.side2, EntityType.FACE)))
            {
                const frame2 = side2Data.frame.frame;
                if (addManip)
                {
                    // getDataForSide returns the frame rotated by the angle, so we need to unrotate the rotation origin
                    addManipulators(context, id.id, {
                                    (ANGLE_2_MANIPULATOR) : angularManipulator({
                                        "axisOrigin" : frame2.origin,
                                        "axisDirection" : frame2.xAxis,
                                        "rotationOrigin" : frame2.origin + rotationMatrix3d(frame2.xAxis, - endAngle) * frame2.zAxis * midLength,
                                        "angle" : definition.endAngle,
                                        "primaryParameterId" : "endAngle"
                                    })
                                });
                }
            }
        }
    }
    else
    {
        side1Data = getDataForSideDeprecated(context, definition.side1, definition.match1, definition.flip1, "side1", definition.side2);
        side2Data = getDataForSideDeprecated(context, definition.side2, definition.match2, definition.flip2, "side2", definition.side1);
    }
    return {
        "side1" : side1Data,
        "side2" : side2Data
    } as TwoSidesData;
}

function computeCurvature(curvatures is FaceCurvatureResult, v is Vector)
{
    var denom = dot(v, v);
    var c1 = dot(curvatures.minDirection, v) ^ 2;
    var c2 = dot(curvatures.maxDirection, v) ^ 2;
    var num = c1 * curvatures.minCurvature + c2 * curvatures.maxCurvature;
    return num / denom;
}

function getPointLocationFromVertexOrParameter(context is Context, sideQueries is SideQueries, parameter is number, useParameter is boolean)
{
    if (sideQueries.position != undefined)
    {
        return sideQueries.position;
    }
    const edge = sideQueries.edge;
    const face = sideQueries.face;

    if (edge != undefined && (useParameter || face != undefined))
    {
        return evEdgeTangentLine(context, {
                "edge" : edge,
                "parameter" : parameter
        }).origin;
    }
}

function getPointsLocations(context is Context, sideQueries1 is SideQueries, side1Parameter is number, sideQueries2 is SideQueries, side2Parameter is number, useParameter is boolean) returns map
{
    var point1 = getPointLocationFromVertexOrParameter(context, sideQueries1, side1Parameter, useParameter);
    var point2 = getPointLocationFromVertexOrParameter(context, sideQueries2, side2Parameter, useParameter);

    const side1Edge = sideQueries1.edge;
    const side2Edge = sideQueries2.edge;
    // If point1 is undefined, this means side1Edge is not undefined. Same for point2.
    if (point1 == undefined)
    {
        if (point2 == undefined)
        {
            // We only have an edge for each side
            point1 = inferVertexFromEdge(context, side1Edge, side2Edge);
        }
        else
        {
            point1 = inferVertexFromPosition(context, side1Edge, point2);
        }
        point1 = evVertexPoint(context, {
                        "vertex" : point1
                    });
    }
    if (point2 == undefined)
    {
        point2 = evVertexPoint(context, {
                        "vertex" : inferVertexFromPosition(context, side2Edge, point1)
                    });
    }

    return {
        "point1" : point1,
        "point2" : point2
    };
}

function getBridgingSideData(sideData is map, match is BridgingCurveMatchType) returns BridgingSideData
{
    var result = { "position" : sideData.point };
    result.degree = switch(match) { BridgingCurveMatchType.POSITION : 0, BridgingCurveMatchType.TANGENCY : 1, BridgingCurveMatchType.CURVATURE : 2 };
    if (result.degree >= 1)
    {
        result.tangent = curvatureFrameTangent(sideData.frame);
    }
    if (result.degree >= 2)
    {
        result.curvature = sideData.frame.curvature;
        result.curvatureDirection = curvatureFrameNormal(sideData.frame);
    }

    return result as BridgingSideData;
}

/** Returns an array of control points for a bridigng bezier curve given two side constraints */
export function computeBridgingControlPoints(side1 is BridgingSideData, side2 is BridgingSideData) returns array
{
    const degree = side1.degree + side2.degree + 1;

    if (tolerantEquals(side1.position, side2.position))
        throw regenError(ErrorStringEnum.BRIDGING_CURVE_POSITIONS_IDENTICAL);

    if (degree == 1)
        return [side1.position, side2.position];

    const positionDiff = side2.position - side1.position;
    const positionDistance = norm(positionDiff);
    const positionDiffNormalized = positionDiff / positionDistance;

    var targetSpeed;

    if (degree == 2) // G0 - G1 case
    {
        if (side1.degree == 0)
            targetSpeed = determineDefaultG0G1Speed(side1.position, side2.position, side2.tangent);
        else
            targetSpeed = determineDefaultG0G1Speed(side2.position, side1.position, side1.tangent);
    }
    else // All other cases
    {
        // Compute the tangent speeds using approachSpeed - how quickly two particles moving along the tangents
        // approach each other (maximum of 2 if they go towards each other, min of -2 if they go away from each other)
        var approachSpeed = 2;
        if (side1.degree > 0)
            approachSpeed += dot(side1.tangent, positionDiffNormalized) - 1;
        if (side2.degree > 0)
            approachSpeed += dot(side2.tangent, -positionDiffNormalized) - 1;
        approachSpeed = max(0, approachSpeed);

        // If approachSpeed is maximal, targetSpeed is positionDistance / degree (which gives the correct answer for a line),
        // and approachSpeed is 0, targetSpeed is 2 * positionDistance / degree (which gets you close to a semicircle for degree 3
        // with appropriate tangent constraints).
        targetSpeed = positionDistance * (2 - approachSpeed / 2) / degree;
    }
    var side1Control = computeSideControlPoints(side1, degree, targetSpeed, side2);
    var side2Control = computeSideControlPoints(side2, degree, targetSpeed, side1);

    return concatenateArrays([side1Control, reverse(side2Control)]);
}

function computeSideControlPoints(side is BridgingSideData, degree is number, targetSpeed is ValueWithUnits, otherSide is BridgingSideData) returns array
{
    if (side.degree == 0)
        return [side.position];

    if (side.speedScale == undefined)
        side.speedScale = 1;
    else if (abs(side.speedScale) < TOLERANCE.zeroLength)
        throw regenError(ErrorStringEnum.BRIDGING_CURVE_ZERO_SPEED_SCALE, [side.speedScaleName]);

    const defaultSpeed = side.degree == 2 ? speedForCurvature(targetSpeed, side.curvature, degree) : targetSpeed;
    const speed = defaultSpeed * side.speedScale;

    var control = [side.position, side.position + side.tangent * speed];
    if (side.degree == 1)
        return control;

    // Curvature case
    if (side.curvatureOffsetScale == undefined)
        side.curvatureOffsetScale = 1;

    // using k = ((degree - 1) / degree) * h / |t| ^ 2, where k is curvature, t is tangent vector, h is the minimum distance
    var distance = (degree / (degree - 1)) * side.curvature * speed * speed;
    var curvaturePoint = control[1] + distance * side.curvatureDirection;

    var curvatureSpeed = defaultSpeed * sqrt(side.speedScale);

    if (otherSide.degree == 0) // G0-G2 adjustment
    {
        const scale = 1.13041336; // Chosen so that a bridging curve between a line and a point 45 degrees off stays in the square
        var d = dot(side.tangent, normalize(otherSide.position - (control[1] + scale * side.tangent * curvatureSpeed)));
        d = clamp(d, -0.5, 0);
        curvatureSpeed *= 1 + d;
    }

    curvaturePoint += side.tangent * curvatureSpeed * side.curvatureOffsetScale;

    return append(control, curvaturePoint);
}

/**
 * Given a curvature and a target distance, and a spline degree, figure out how far to go for the second control point
 * so the third control point ends up 2 * targetDistance away from the first
 */
function speedForCurvature(targetDistance is ValueWithUnits, curvature is ValueWithUnits, degree is number) returns ValueWithUnits
{
    if (tolerantEquals(curvature, 0 / meter))
        return targetDistance;

    // Perpendicular distance for second point is d/(d-1) * curvature * speed^2.
    // Let x be the speed and assume the tangent spacing of first and second control points is the same.
    // Then the squared distance from point2 to the second control point is:
    // (2 * x)^2 + d^2/(d-1)^2 * k^2 x^4
    // And we would like that to be (2 * targetDistance)^2.
    // So, letting y = x^2, we have a quadratic equation in y with:
    const a = (curvature * degree / (degree - 1)) ^ 2;
    const b = 4;
    const c = -4 * targetDistance ^ 2;
    const y = solveQuadratic(a, b, c)[0];

    return sqrt(y);
}

function solveQuadratic(a, b, c) returns array
{
    const discriminant = b ^ 2 - 4 * a * c;
    if (discriminant < 0)
        return [];
    return [(-b + sqrt(discriminant)) / (2 * a), (-b - sqrt(discriminant)) / (2 * a)];
}

function addManipulatorsForSide(context is Context, id is Id, controlPoints is array, side is number, degree is number, flip is boolean)
{
    if (degree == 0)
        return;
    if (side == 2)
        controlPoints = reverse(controlPoints);

    const direction = normalize(controlPoints[1] - controlPoints[0]);

    var magnitudeManipulator is Manipulator = linearManipulator({
                "base" : controlPoints[0],
                "direction" : direction * (flip ? -1 : 1),
                "offset" : norm(controlPoints[1] - controlPoints[0]) * (flip ? -1 : 1),
                "primaryParameterId" : "side" ~ side ~ "Magnitude"
            });
    addManipulators(context, id, {
                magnitudeManipulatorId(side) : magnitudeManipulator
            });

    if (degree == 1)
        return;

    const base = controlPoints[2] - direction * dot(direction, controlPoints[2] - controlPoints[1]);

    var curvatureManipulator is Manipulator = linearManipulator({
                "base" : base,
                "direction" : direction,
                "offset" : dot(direction, controlPoints[2] - base),
                "style" : ManipulatorStyleEnum.SECONDARY,
                "primaryParameterId" : "side" ~ side ~ "CurvatureOffset"
            });
    addManipulators(context, id, {
                curvatureManipulatorId(side) : curvatureManipulator
            });

}

function showControlPoints(context is Context, id is Id, controlPoints is array)
{
    if (!isTopLevelId(id))
    {
        return;
    }
    const controlId = id + "controlPoints";
    startFeature(context, controlId, {});
    try
    {
        opPoint(context, controlId + 0 + "point", { "point" : controlPoints[0] });
        for (var i = 1; i < size(controlPoints); i += 1)
        {
            opPoint(context, controlId + i + "point", { "point" : controlPoints[i] });
            opFitSpline(context, controlId + i + "line", { "points" : [ controlPoints[i - 1], controlPoints[i] ] });
        }
        const edges = qCreatedBy(controlId, EntityType.EDGE);
        const vertices = qCreatedBy(controlId, EntityType.VERTEX)->qBodyType(BodyType.POINT);
        addDebugEntities(context, qUnion([vertices, edges]), DebugColor.MAGENTA);
    }
    abortFeature(context, controlId);
}

function showWire(context is Context, id is Id, wire is Query)
{
    if (!isTopLevelId(id))
    {
        return;
    }
    const showWireId = id + "showWire";
    startFeature(context, showWireId, {});
    try
    {
        addDebugEntities(context, wire, DebugColor.MAGENTA);
    }
    abortFeature(context, showWireId);
}

function checkSidesInPreselectionEditingLogic(context is Context, sides is array) returns boolean
{
    if (size(sides) > 2)
    {
        return false;
    }
    const sideCount = size(sides);
    for (var i = 0; i < sideCount; i += 1)
    {
        var side = sides[i];
        if (size(evaluateQuery(context, side)) > 2)
        {
            return false;
        }
        const vertices = evaluateQuery(context, qEntityFilter(side, EntityType.VERTEX));
        const edges = evaluateQuery(context, qEntityFilter(side, EntityType.EDGE));
        const faces = evaluateQuery(context, qEntityFilter(side, EntityType.FACE));

        const vertexCount = size(vertices);
        const edgeCount = size(edges);
        const faceCount = size(faces);

        if (vertexCount > 2 || edgeCount > 2 || faceCount > 2)
        {
            return false;
        }
        for (var j = i+1; j < sideCount; j += 1)
        {
            if (!isQueryEmpty(context, qIntersection([side, sides[j]])))
            {
                return false;
            }
        }
    }
    return true;
}

function bridgingCurvePreselectionEditingLogic(context is Context, oldDefinition is map, definition is map) returns map
{
    const vertices = evaluateQuery(context, qEntityFilter(definition.preselectedEntities, EntityType.VERTEX));
    const edges = evaluateQuery(context, qEntityFilter(definition.preselectedEntities, EntityType.EDGE));
    const faces = evaluateQuery(context, qEntityFilter(definition.preselectedEntities, EntityType.FACE));

    const vertexCount = size(vertices);
    const edgeCount = size(edges);
    const faceCount = size(faces);

    if (vertexCount > 2 || edgeCount > 2 || faceCount > 2 || vertexCount + edgeCount + faceCount == 0)
    {
        return definition;
    }

    var sides = faces;
    for (var edge in edges)
    {
        var hasBeenAdded = false;
        for (var i = 0; i < faceCount; i += 1)
        {
            if (checkFaceContainsEdge(context, faces[i], edge))
            {
                sides[i] = qUnion([sides[i], edge]);
                hasBeenAdded = true;
            }
        }
        if (!hasBeenAdded)
        {
            sides = append(sides, edge);
        }
    }

    var sideCount = size(sides);
    for (var vertex in vertices)
    {
        var hasBeenAdded = false;
        for (var i = 0; i < sideCount; i += 1)
        {
            if (!isQueryEmpty(context, qContainsPoint(sides[i], evVertexPoint(context, {
                    "vertex" : vertex
            }))))
            {
                sides[i] = qUnion([sides[i], vertex]);
                hasBeenAdded = true;
            }
        }
        if (!hasBeenAdded)
        {
            sides = append(sides, vertex);
        }
    }

    if (!checkSidesInPreselectionEditingLogic(context, sides))
    {
        return definition;
    }

    sideCount = size(sides);
    if (sideCount > 0)
    {
        definition.side1 = sides[0];
    }
    if (sideCount > 1)
    {
        definition.side2 = sides[1];
    }

    return definition;
}

function needToFlipSide(context is Context, edge is Query, point is Vector) returns boolean
{
    const frames = evEdgeCurvatures(context, {
                "edge" : edge,
                "parameters" : [0, 1]
            });
    const inferredFrame = findClosestEndFrame(point, frames[0], frames[1]);
    const midEdgeTangent = evEdgeTangentLine(context, {
            "edge" : edge,
            "parameter" : 0.5
    });
    return dot(inferredFrame.frame.zAxis, midEdgeTangent.direction) < 0;
}

function sideFlips(context is Context, oldDefinition is map, definition is map,
    specifiedParameters is map) returns map
{
    if (oldDefinition == {} || oldDefinition.editEdgePositions == definition.editEdgePositions)
    {
        return definition;
    }
    const sideQueries1 = makeSideQueries(context, definition.side1);
    const sideQueries2 = makeSideQueries(context, definition.side2);
    const side1IsOnlyEdge = sideQueries1.edge != undefined && sideQueries1.position == undefined && sideQueries1.face == undefined;
    const side2IsOnlyEdge = sideQueries2.edge != undefined && sideQueries2.position == undefined && sideQueries2.face == undefined;
    const canFlipSide1 = side1IsOnlyEdge && !specifiedParameters.flip1 && !specifiedParameters.startEdgeParameter;
    const canFlipSide2 = side2IsOnlyEdge && !specifiedParameters.flip2 && !specifiedParameters.endEdgeParameter;
    // We only want to flip if we only have an edge (ie we infer the edge point), the user has not manually flipped the tangency direction
    // and the user has not specified an edge parameter.
    if (!canFlipSide1 && !canFlipSide2)
    {
        return definition;
    }
    const points = getPointsLocations(context, sideQueries1, 0, sideQueries2, 0, false);
    if (canFlipSide1)
    {
        definition.flip1 = needToFlipSide(context, sideQueries1.edge, points.point1) ? !definition.flip1 : definition.flip1;
    }
    if (canFlipSide2)
    {
        definition.flip2 = needToFlipSide(context, sideQueries2.edge, points.point2) ? !definition.flip2 : definition.flip2;
    }
    return definition;
}

/**
 * @internal
 * The editing logic function used in the `bridgingCurve` feature.
 */
export function bridgingCurveEditingLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (oldDefinition == {})
    {
        definition = bridgingCurvePreselectionEditingLogic(context, oldDefinition, definition);
    }

    definition.side1HasVertex = !isQueryEmpty(context, qEntityFilter(definition.side1, EntityType.VERTEX)) || !isQueryEmpty(context, qBodyType(definition.side1, BodyType.MATE_CONNECTOR));
    definition.side2HasVertex = !isQueryEmpty(context, qEntityFilter(definition.side2, EntityType.VERTEX)) || !isQueryEmpty(context, qBodyType(definition.side2, BodyType.MATE_CONNECTOR));
    definition.side1HasFace = !isQueryEmpty(context, qEntityFilter(definition.side1, EntityType.FACE));
    definition.side2HasFace = !isQueryEmpty(context, qEntityFilter(definition.side2, EntityType.FACE));

    return sideFlips(context, oldDefinition, definition, specifiedParameters);
}

/**
 * @internal
 * The manipulator change function used in the `bridgingCurve` feature.
 */
export function bridgingCurveManipulator(context is Context, definition is map, newManipulators is map) returns map
{
    // These manipulators need to be before control points computation to avoid issues when edge positions are the same.
    if (newManipulators[SIDE_1_MANIPULATOR] is map)
    {
        const manipulator = newManipulators[SIDE_1_MANIPULATOR];
        var edge = qEntityFilter(definition.side1, EntityType.EDGE);
        if (!isQueryEmpty(context, edge))
        {
            var pos = manipulator.base + manipulator.direction * manipulator.offset;
            var distanceResult = evDistance(context, { "side0" : pos, "side1" : edge });
            definition["startEdgeParameter"] = distanceResult.sides[1].parameter;
        }
    }
    if (newManipulators[SIDE_2_MANIPULATOR] is map)
    {
        const manipulator = newManipulators[SIDE_2_MANIPULATOR];
        var edge = qEntityFilter(definition.side2, EntityType.EDGE);
        if (!isQueryEmpty(context, edge))
        {
            var pos = manipulator.base + manipulator.direction * manipulator.offset;
            var distanceResult = evDistance(context, { "side0" : pos, "side1" : edge });
            definition["endEdgeParameter"] = distanceResult.sides[1].parameter;
        }
    }
    if (newManipulators[ANGLE_1_MANIPULATOR] is map)
    {
        definition.startAngle = newManipulators[ANGLE_1_MANIPULATOR].angle;
    }
    if (newManipulators[ANGLE_2_MANIPULATOR] is map)
    {
        definition.endAngle = newManipulators[ANGLE_2_MANIPULATOR].angle;
    }

    const sideData = getSideDataAndAddManipulators(context, definition, false, {});
    var side1Data = sideData.side1;
    var side2Data = sideData.side2;

    var side1 is BridgingSideData = getBridgingSideData(side1Data, definition.match1);
    var side2 is BridgingSideData = getBridgingSideData(side2Data, definition.match2);

    if (newManipulators[curvatureManipulatorId(1)] is map || newManipulators[curvatureManipulatorId(2)] is map)
    {
        side1.speedScale = definition.side1Magnitude;
        side2.speedScale = definition.side2Magnitude;
    }
    var controlPoints = computeBridgingControlPoints(side1, side2);
    for (var side in [1, 2])
    {
        if (side == 2)
            controlPoints = reverse(controlPoints);

        const magnitudeManipulator = newManipulators[magnitudeManipulatorId(side)];
        if (magnitudeManipulator is map)
        {
            const defaultSpeed = norm(controlPoints[1] - controlPoints[0]);
            definition["side" ~ side ~ "Magnitude"] = abs(magnitudeManipulator.offset) / defaultSpeed;
            definition["flip" ~ side] = magnitudeManipulator.offset < 0;
        }
        const curvatureManipulator = newManipulators[curvatureManipulatorId(side)];
        if (curvatureManipulator is map)
        {
            const direction = normalize(controlPoints[1] - controlPoints[0]);
            const defaultSpeed = dot(direction, controlPoints[2] - controlPoints[1]);
            definition["side" ~ side ~ "CurvatureOffset"] = curvatureManipulator.offset / defaultSpeed;
        }
    }

    // ==== Legacy manipulator code ====
    if (newManipulators[MAGNITUDE_MANIPULATOR] is map)
    {
        const manipulator = newManipulators[MAGNITUDE_MANIPULATOR];
        var defaultSpeed;
        if (definition.match1 != BridgingCurveMatchType.POSITION)
        {
            defaultSpeed = determineDefaultG0G1Speed(side2Data.point, side1Data.point, curvatureFrameTangent(side1Data.frame));
        }
        else if (definition.match2 != BridgingCurveMatchType.POSITION)
        {
            defaultSpeed = determineDefaultG0G1Speed(side1Data.point, side2Data.point, curvatureFrameTangent(side2Data.frame));
        }
        definition.magnitude = manipulator.offset / (defaultSpeed * UI_SCALING);
    }
    if (newManipulators[CENTRAL_MAGNITUDE_MANIPULATOR] is map)
    {
        const manipulator = newManipulators[CENTRAL_MAGNITUDE_MANIPULATOR];
        const speeds = determineDefaultG1G1Speed(side1Data.frame, side2Data.frame);
        var scaling = UI_SCALING * (speeds[0] + speeds[1]) * 0.5;
        definition.magnitude = manipulator.offset / scaling;
    }
    if (newManipulators[BIAS_MANIPULATOR] is map)
    {
        const manipulator = newManipulators[BIAS_MANIPULATOR];
        definition.bias = (manipulator.angle / ANGLE_RANGE) + 0.5;
        definition.bias = max(0.01, min(0.99, definition.bias));
    }
    return definition;
}

function inferVertexFromEdge(context is Context, edge is Query, otherEdge is Query) returns Query
{
    var inferred = qNothing();
    // In this case we want to get the vertex closest to one of the vertices of the other edge
    const edgeVertices = qAdjacent(edge, AdjacencyType.VERTEX, EntityType.VERTEX);
    const otherEdgeVertices = qAdjacent(otherEdge, AdjacencyType.VERTEX, EntityType.VERTEX);
    var bestDistance = -2 * meter;
    for (var vertex in evaluateQuery(context, otherEdgeVertices))
    {
        const vertexPoint = evVertexPoint(context, { "vertex" : vertex });
        const found = qClosestTo(edgeVertices, vertexPoint);
        if (size(evaluateQuery(context, found)) == 1)
        {
            const distance = norm(evVertexPoint(context, { "vertex" : found }) - vertexPoint);
            if (bestDistance < -1 * meter || distance < bestDistance)
            {
                inferred = found;
                bestDistance = distance;
            }
        }
    }
    return inferred;
}

function inferVertexFromPosition(context is Context, edge is Query, otherPoint is Vector) returns Query
{
    const edgeVertices = qAdjacent(edge, AdjacencyType.VERTEX, EntityType.VERTEX);
    return qClosestTo(edgeVertices, otherPoint);
}

function inferVertexDeprecated(context is Context, edge is Query, otherSide is Query) returns Query
{
    var otherVertex = qEntityFilter(otherSide, EntityType.VERTEX);
    var otherEdge = qEntityFilter(otherSide, EntityType.EDGE);
    var inferred = qNothing();
    if (size(evaluateQuery(context, otherVertex)) == 1)
    {
        const edgeVertices = qAdjacent(edge, AdjacencyType.VERTEX, EntityType.VERTEX);
        inferred = inferVertexFromPosition(context, edge, evVertexPoint(context, {
                        "vertex" : otherVertex
                    }));
    }
    else if (size(evaluateQuery(context, otherEdge)) == 1)
    {
        inferred = inferVertexFromEdge(context, edge, otherEdge);
    }
    return inferred;
}

/**
 * This will find the frame that exactly matches the point passed in. It's not useful in general cases and is deprecated.
 * It will reverse the direction of the start frame to point out of the edge
 */
function findMatchingEndFrame(point is Vector, startFrame is EdgeCurvatureResult, endFrame is EdgeCurvatureResult) returns EdgeCurvatureResult
{
    if (tolerantEquals(startFrame.frame.origin, point))
    {
        // This is the frame at the start of the edge and we want a frame that points out of the edge
        // so we invert the zAxis (which is the tangent, see curvatureFrameTangent)
        var frame = startFrame;
        frame.frame.zAxis *= -1;
        return frame;
    }
    else if (tolerantEquals(endFrame.frame.origin, point))
    {
        return endFrame;
    }
    else
    {
        return undefined;
    }
}

/**
 * This will find the frame that is closest to the point passed in..
 * It will reverse the direction of the start frame to point out of the edge
 */
function findClosestEndFrame(point is Vector, startFrame is EdgeCurvatureResult, endFrame is EdgeCurvatureResult) returns EdgeCurvatureResult
{
    const startDistance = squaredNorm(startFrame.frame.origin - point);
    const endDistance = squaredNorm(endFrame.frame.origin - point);
    if (startDistance < endDistance)
    {
        var frame = startFrame;
        frame.frame.zAxis *= -1;
        return frame;
    }
    else
    {
        return endFrame;
    }
}

function getDataForSideDeprecated(context is Context, side is Query, match is BridgingCurveMatchType, flip is boolean, sideName is string, otherSide is Query) returns SideData
{
    var points = qEntityFilter(side, EntityType.VERTEX);
    var edges = qEntityFilter(side, EntityType.EDGE);
    var edgeCount = size(evaluateQuery(context, edges));
    var sideQueries is SideQueries = {
        "vertex" : undefined,
        "edge" : undefined,
        "face" : undefined
    } as SideQueries;

    if (edgeCount == 1)
    {
        sideQueries.edge = edges;
        if (isQueryEmpty(context, points))
        {
            // The user hasn't selected a vertex but if they selected an edge we may be able to work out what they want from the other side selections
            points = inferVertexDeprecated(context, edges, otherSide);
        }
    }
    if (size(evaluateQuery(context, points)) != 1)
    {
        throw regenError(ErrorStringEnum.BRIDGING_CURVE_VERTEX_BOTH_SIDES, [sideName]);
    }
    sideQueries.vertex = points;
    sideQueries.position = evVertexPoint(context, { "vertex" : points });

    return getDataForSideNoFace(context, sideQueries, match, flip, sideName, sideQueries.position, false, 0);
}

function getDataForSideNoFace(context is Context, sideQueries is SideQueries, match is BridgingCurveMatchType, flip is boolean, sideName is string, sidePoint is Vector, useParameter is boolean, parameter is number) returns SideData
{
    var vertex = sideQueries.vertex;
    var edges = sideQueries.edge;

    if (edges == undefined && match != BridgingCurveMatchType.POSITION)
    {
        if (sideQueries.tangent != undefined) // We have a mate connector
        {
            // Construct a frame just from the mate connector
            return
            {
                "point" : sidePoint,
                "frame" : {
                    "frame" : coordSystem(sidePoint, perpendicularVector(sideQueries.tangent), sideQueries.tangent * (flip ? -1 : 1)),
                    "curvature" : 0 / meter // Mate connectors have zero curvature
                } as EdgeCurvatureResult
            } as SideData;
        }
        // Try to get the edge from the vertex
        edges = qAdjacent(vertex, AdjacencyType.VERTEX, EntityType.EDGE);
        var edgeCount = size(evaluateQuery(context, edges));
        if (edgeCount != 1)
        {
            throw regenError(ErrorStringEnum.BRIDGING_CURVE_ONE_EDGE_EACH_SIDE, [sideName]);
        }
    }

    var frame;
    var position = sideQueries.position;
    if (match != BridgingCurveMatchType.POSITION)
    {
        if (useParameter && position == undefined)
        {
            frame = evEdgeCurvature(context, {
                    "edge" : edges,
                    "parameter" : parameter
            });
        }
        else
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1935_BRIDGING_CURVE_MATE_CONNECTOR))
            {
                const positionParameter = evDistance(context, {
                        "side0" : edges,
                        "side1" : sidePoint
                    }).sides[0].parameter;

                frame = evEdgeCurvature(context, {
                        "edge" : edges,
                        "parameter" : positionParameter
                });
                if (positionParameter < TOLERANCE.zeroLength)
                {
                    frame.frame.zAxis *= -1;
                }
            }
            else
            {
                // This code deliberately only considers the ends of the edge but we could just as easily match to an
                // edge that passes through the specified point but doesn't end there.
                const frames = evEdgeCurvatures(context, {
                            "edge" : edges,
                            "parameters" : [0, 1]
                        });
                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V606_TOLERANT_BRIDGING_CURVE))
                {
                    frame = findClosestEndFrame(sidePoint, frames[0], frames[1]);
                }
                else
                {
                    frame = findMatchingEndFrame(sidePoint, frames[0], frames[1]);
                }
            }
        }
        if (frame == undefined)
        {
            throw regenError(ErrorStringEnum.BRIDGING_CURVE_VERTEX_AT_END_OF_EDGE, [sideName]);
        }
        if (flip)
        {
            frame.frame.zAxis = -frame.frame.zAxis;
        }
    }

    return
    {
        "point" : sidePoint,
        "frame" : frame
    } as SideData;
}

function getDataForSideFace(context is Context, sideQueries is SideQueries, param is number, angle is ValueWithUnits, match is BridgingCurveMatchType, flip is boolean, sideName is string, point is Vector, otherPoint is Vector) returns SideData
{
    var tangent;
    var edgeTangent;
    if (sideQueries.edge != undefined)
    {
        const line = evEdgeTangentLine(context, { "edge" : sideQueries.edge, "parameter" : param });
        edgeTangent = line.direction;
    }

    const distanceResult = evDistance(context, { "side0" : point, "side1" : sideQueries.face });
    const facePlane = evFaceTangentPlane(context, { "face" : sideQueries.face, "parameter" : distanceResult.sides[1].parameter });

    if (edgeTangent == undefined)
    {
        const projectedPoint = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2173_BRIDGING_CURVE_PROJECTION_FIX) ? project(facePlane, point) : point;
        const projectedOtherPoint = project(facePlane, otherPoint);
        if (tolerantEquals(projectedPoint, projectedOtherPoint))
        {
            tangent = facePlane.x;
        }
        else
        {
            tangent = normalize(projectedOtherPoint - projectedPoint);
        }
    }
    else
    {
        tangent = cross(facePlane.normal, edgeTangent);
    }
    if (dot(tangent, otherPoint - point) < 0)
    {
        tangent = -tangent;
    }

    var rotatedTangent = rotationMatrix3d(facePlane.normal, angle) * tangent;
    var vectorForCurvature;
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2038_BRIDGING_CURVE_CURVATURE_FIX))
    {
        vectorForCurvature = rotatedTangent;
    }
    else
    {
        vectorForCurvature = tangent;
    }

    var curvatureResult = evFaceCurvature(context, {
            "face" : sideQueries.face,
            "parameter" : distanceResult.sides[1].parameter
    });

    return
    {
        "point" : point,
        "frame" : { "frame" : coordSystem(point, facePlane.normal, (flip ? -1 : 1) * rotatedTangent),
                    "curvature" : -computeCurvature(curvatureResult, vectorForCurvature) } as EdgeCurvatureResult
    } as SideData;
}

function getDataForSide(context is Context, sideQueries is SideQueries, useParameter is boolean, param is number, angle is ValueWithUnits, match is BridgingCurveMatchType, flip is boolean, sideName is string, sidePoint is Vector, otherPoint is Vector) returns SideData
{
    if (sideQueries.face == undefined)
    {
        return getDataForSideNoFace(context, sideQueries, match, flip, sideName, sidePoint, useParameter, param);
    }
    else
    {
        return getDataForSideFace(context, sideQueries, param, angle, match, flip, sideName, sidePoint, otherPoint);
    }
}

// ======================================= Legacy-only functions below =====================================================

function legacyBridgingCurve(context is Context, id is Id, definition is map, side1Data, side2Data)
{
    if (definition.match1 == BridgingCurveMatchType.POSITION)
    {
        if (definition.match2 == BridgingCurveMatchType.POSITION)
        {
            createG0G0BridgingCurve(context, id, side1Data.point, side2Data.point);
        }
        else if (definition.match2 == BridgingCurveMatchType.TANGENCY)
        {
            createG0G1BridgingCurve(context, id, side1Data.point, side2Data.frame, definition.magnitude, false);
        }
        else if (definition.match2 == BridgingCurveMatchType.CURVATURE)
        {
            createG0G2BridgingCurve(context, id, side1Data.point, side2Data.frame, definition.magnitude, false);
        }
    }
    else if (definition.match1 == BridgingCurveMatchType.TANGENCY)
    {
        if (definition.match2 == BridgingCurveMatchType.POSITION)
        {
            createG0G1BridgingCurve(context, id, side2Data.point, side1Data.frame, definition.magnitude, true);
        }
        else if (definition.match2 == BridgingCurveMatchType.TANGENCY)
        {
            createG1G1BridgingCurve(context, id, side1Data.frame, side2Data.frame, definition.magnitude, definition.bias);
        }
        else if (definition.match2 == BridgingCurveMatchType.CURVATURE)
        {
            createG1G2BridgingCurve(context, id, side1Data.frame, side2Data.frame, definition.magnitude, definition.bias, false);
        }
    }
    else if (definition.match1 == BridgingCurveMatchType.CURVATURE)
    {
        if (definition.match2 == BridgingCurveMatchType.POSITION)
        {
            createG0G2BridgingCurve(context, id, side2Data.point, side1Data.frame, definition.magnitude, true);
        }
        else if (definition.match2 == BridgingCurveMatchType.TANGENCY)
        {
            createG1G2BridgingCurve(context, id, side2Data.frame, side1Data.frame, definition.magnitude, definition.bias, true);
        }
        else if (definition.match2 == BridgingCurveMatchType.CURVATURE)
        {
            createG2G2BridgingCurve(context, id, side1Data.frame, side2Data.frame, definition.magnitude, definition.bias);
        }
    }
}

function createG0G0BridgingCurve(context is Context, id is Id, point1 is Vector, point2 is Vector)
{
    createBezierCurve(context, id, [point1, point2]);
}

function determineDefaultG0G1Speed(point1 is Vector, point2 is Vector, direction is Vector) returns ValueWithUnits
{
    // One argument I could make is that we want the distance between the middle control point and the other two to be equal
    // This would yield a symmetric curve. However, we may want to cap that based on distance between the two given points.
    // We'll start out with something simple.

    // 1. If the points are the same then there is nothing we can do
    if (tolerantEquals(point1, point2))
    {
        throw regenError(ErrorStringEnum.BRIDGING_CURVE_POSITIONS_IDENTICAL);
    }

    // 2. First look at the intersection of the tangent vector with the bisecting plane of the points.
    //    If we can find one then the curve will be symmetric which may be nice

    // At the same time we don't want discontinuous behavior as point1 falls behind point2.
    // So cap the maximum distance to be the distance between the source points
    const maximumDistance = norm(point2 - point1);

    const line = line(point2, direction);
    const plane = plane((point1 + point2) * 0.5, normalize(point2 - point1));
    const intersection = intersection(plane, line);
    var distance = maximumDistance;
    if (intersection.dim == 0)
    {
        const candidate = intersection.intersection;
        const calculatedDistance = dot(candidate - point2, direction);
        if (calculatedDistance > 0 && calculatedDistance < maximumDistance)
        {
            distance = calculatedDistance;
        }
    }
    return distance;
}

function createG0G1BridgingCurve(context is Context, id is Id,
    point is Vector, curvatureFrame is EdgeCurvatureResult, magnitude is number, flipDirection is boolean)
{

    const defaultSpeed = determineDefaultG0G1Speed(point, curvatureFrame.frame.origin, curvatureFrameTangent(curvatureFrame));
    var magnitudeManipulator is Manipulator = linearManipulator({
                "base" : curvatureFrame.frame.origin,
                "direction" : curvatureFrameTangent(curvatureFrame),
                "offset" : magnitude * UI_SCALING * defaultSpeed,
                "minValue" : TOLERANCE.zeroLength * meter,
                "primaryParameterId" : "magnitude"
            });
    addManipulators(context, id, {
                (MAGNITUDE_MANIPULATOR) : magnitudeManipulator
            });

    // OK. The tangent vector of the edge curvature result goes away from the edge
    // Very simple to begin with
    const speed = magnitude * defaultSpeed;
    var middlePoint = curvatureFrame.frame.origin + (curvatureFrameTangent(curvatureFrame) * speed);

    var controlPoints = [
        point,
        middlePoint,
        curvatureFrame.frame.origin
    ];

    return createBezierCurve(context, id, flipDirection ? reverse(controlPoints) : controlPoints);
}

function determineDefaultG1G1Speed(frame1 is EdgeCurvatureResult, frame2 is EdgeCurvatureResult) returns array
{
    const line1 is Line = line(frame1.frame.origin, curvatureFrameTangent(frame1));
    const line2 is Line = line(frame2.frame.origin, curvatureFrameTangent(frame2));

    // Given the two points and tangent vectors we want to first of all calculate the default bias
    // which we will do by getting the distance of the point from the other line
    const bias1 = norm(project(line2, line1.origin) - line1.origin);
    const bias2 = norm(project(line1, line2.origin) - line2.origin);

    const jointBias = bias1 + bias2;
    var bias = 0.5;
    if (jointBias > TOLERANCE.zeroLength * meter)
    {
        bias = clamp(bias2 / jointBias, DEFAULT_BIAS_MINIMUM, 1 - DEFAULT_BIAS_MINIMUM);
    }
    // With the bias in hand we can now factor it in to the initial speeds
    const baseDistance = norm(line1.origin - line2.origin);
    return [DEFAULT_G1G1_SCALE * (1 - bias) * baseDistance, DEFAULT_G1G1_SCALE * bias * baseDistance];
}

function createG1G1Manipulators(context is Context, id is Id,
    line1 is Line, line2 is Line,
    speed1 is ValueWithUnits, speed2 is ValueWithUnits,
    magnitude is number, bias is number)
{
    var averageTangent = (line1.direction + line2.direction) * 0.5;
    if (tolerantEquals(averageTangent, vector(0, 0, 0)))
    {
        averageTangent = line1.direction;
    }
    var normal = cross(averageTangent, line2.origin - line1.origin);
    if (tolerantEquals(normal, vector(0, 0, 0) * meter))
    {
        return;
    }
    const direction = normalize(cross(line2.origin - line1.origin, normal));
    const base = (line1.origin + line2.origin) * 0.5;
    const offset = magnitude * UI_SCALING * (speed1 + speed2) * 0.5;
    var magnitudeManipulator is Manipulator = linearManipulator({
                "base" : base,
                "direction" : direction,
                "offset" : offset,
                "minValue" : TOLERANCE.zeroLength * meter,
                "primaryParameterId" : "magnitude"
            });

    const halfWidth = norm(line1.origin - line2.origin) * 0.5;
    const radius = halfWidth / sin(ANGLE_RANGE * 0.5);
    var biasManipulator is Manipulator = angularManipulator({
            "axisOrigin" : base + ((offset - radius) * direction),
            "axisDirection" : -normalize(normal), // negated so that the bias arrow matches how the shape changes
            "rotationOrigin" : base + (offset * direction),
            "angle" : (bias - 0.5) * ANGLE_RANGE,
            "minValue" : -ANGLE_RANGE * 0.49,
            "maxValue" : ANGLE_RANGE * 0.49,
            "style" : ManipulatorStyleEnum.SIMPLE,
            "primaryParameterId" : "bias"
        });

    addManipulators(context, id, {
                (CENTRAL_MAGNITUDE_MANIPULATOR) : magnitudeManipulator,
                (BIAS_MANIPULATOR) : biasManipulator
            });
}

function createG1G1BridgingCurve(context is Context, id is Id,
    curvatureFrame1 is EdgeCurvatureResult, curvatureFrame2 is EdgeCurvatureResult, magnitude is number, bias is number)
{
    // OK. The tangent vector of the edge curvature result goes away from the edge
    // Very simple to begin with

    const speeds = determineDefaultG1G1Speed(curvatureFrame1, curvatureFrame2);

    const speed1 = speeds[0] * magnitude * 2 * (1 - bias);
    const speed2 = speeds[1] * magnitude * 2 * bias;

    const point1 = curvatureFrame1.frame.origin;
    const point2 = curvatureFrame2.frame.origin;
    const tangent1 = curvatureFrameTangent(curvatureFrame1);
    const tangent2 = curvatureFrameTangent(curvatureFrame2);

    createG1G1Manipulators(context, id, line(point1, tangent1), line(point2, tangent2),
            speeds[0], speeds[1],
            magnitude, bias);

    var inner1 = point1 + (speed1 * tangent1);
    var inner2 = point2 + (speed2 * tangent2);

    const controlPoints = [
            curvatureFrame1.frame.origin,
            inner1,
            inner2,
            curvatureFrame2.frame.origin
        ];

    return createBezierCurve(context, id, controlPoints);
}

function createG0G2BridgingCurve(context is Context, id is Id,
    point is Vector, curvatureFrame is EdgeCurvatureResult, magnitude is number, flipDirection is boolean)
{
    const defaultSpeed = determineDefaultG0G1Speed(point, curvatureFrame.frame.origin, curvatureFrameTangent(curvatureFrame));
    const speed = magnitude * defaultSpeed;

    var P2 = curvatureFrame.frame.origin + (curvatureFrameTangent(curvatureFrame) * speed);
    var P3 = curvatureFrame.frame.origin;
    var degree = 3;
    // using k = ((degree - 1) / degree) * h / |t| ^ 2, where k is curvature, t is tangent vector, h is the minimum distance
    // from the next control point to the extension of tangent direction
    var distance = (degree / (degree - 1)) * curvatureFrame.curvature * speed * speed;
    var P1 = P2 + distance * curvatureFrameNormal(curvatureFrame);

    var controlPoints = [
        point,
        P1,
        P2,
        P3
    ];

    var magnitudeManipulator is Manipulator = linearManipulator({
                "base" : curvatureFrame.frame.origin,
                "direction" : curvatureFrameTangent(curvatureFrame),
                "offset" : magnitude * UI_SCALING * defaultSpeed,
                "minValue" : TOLERANCE.zeroLength * meter,
                "primaryParameterId" : "magnitude"
            });
    addManipulators(context, id, {
                (MAGNITUDE_MANIPULATOR) : magnitudeManipulator
            });

    return createBezierCurve(context, id, flipDirection ? reverse(controlPoints) : controlPoints);
}

function createG1G2BridgingCurve(context is Context, id is Id,
    curvatureFrame1 is EdgeCurvatureResult, curvatureFrame2 is EdgeCurvatureResult, magnitude is number, bias is number, flipDirection is boolean)
{
    const speeds = determineDefaultG1G1Speed(curvatureFrame1, curvatureFrame2);

    const speed1 = speeds[0] * magnitude * 2 * (1 - bias);
    const speed2 = speeds[1] * magnitude * 2 * bias;

    const point1 = curvatureFrame1.frame.origin;
    const point2 = curvatureFrame2.frame.origin;
    const tangent1 = curvatureFrameTangent(curvatureFrame1);
    const tangent2 = curvatureFrameTangent(curvatureFrame2);

    createG1G1Manipulators(context, id, line(point1, tangent1), line(point2, tangent2),
            speeds[0], speeds[1],
            magnitude, bias);

    // using k = ((degree - 1) / degree) * h / |t| ^ 2, where k is curvature, t is tangent vector, h is the minimum distance
    // from the next control point to the extension of tangent direction
    var P1 = point1 + (speed1 * tangent1);
    var P3 = point2 + (speed2 * tangent2);
    var degree = 4;
    var distance = (degree / (degree - 1)) * curvatureFrame2.curvature * speed2 * speed2;
    var P2 = P3 + distance * curvatureFrameNormal(curvatureFrame2);

    var controlPoints = [
            curvatureFrame1.frame.origin,
            P1,
            P2,
            P3,
            curvatureFrame2.frame.origin
    ];
    return createBezierCurve(context, id, flipDirection ? reverse(controlPoints) : controlPoints);
}

function createG2G2BridgingCurve(context is Context, id is Id,
    curvatureFrame1 is EdgeCurvatureResult, curvatureFrame2 is EdgeCurvatureResult, magnitude is number, bias is number)
{
    const speeds = determineDefaultG1G1Speed(curvatureFrame1, curvatureFrame2);

    const speed1 = speeds[0] * magnitude * 2 * (1 - bias);
    const speed2 = speeds[1] * magnitude * 2 * bias;

    const point1 = curvatureFrame1.frame.origin;
    const point2 = curvatureFrame2.frame.origin;
    const tangent1 = curvatureFrameTangent(curvatureFrame1);
    const tangent2 = curvatureFrameTangent(curvatureFrame2);

    createG1G1Manipulators(context, id, line(point1, tangent1), line(point2, tangent2),
            speeds[0], speeds[1],
            magnitude, bias);

    // using k = ((degree - 1) / degree) * h / |t| ^ 2, where k is curvature, t is tangent vector, h is the minimum distance
    // from the next control point to the extension of tangent direction
    var degree = 5;
    var P1 = point1 + (speed1 * tangent1);
    var P4 = point2 + (speed2 * tangent2);
    var distance1 = (degree / (degree - 1)) * curvatureFrame1.curvature * speed1 * speed1;
    var P2 = P1 + distance1 * curvatureFrameNormal(curvatureFrame1);
    var distance2 = (degree / (degree - 1)) * curvatureFrame2.curvature * speed2 * speed2;
    var P3 = P4 + distance2 * curvatureFrameNormal(curvatureFrame2);

    var controlPoints = [
            curvatureFrame1.frame.origin,
            P1,
            P2,
            P3,
            P4,
            curvatureFrame2.frame.origin
    ];

    return createBezierCurve(context, id, controlPoints);
}

function createBezierCurve(context is Context, id is Id, controlPoints is array)
{
    const bCurve = bSplineCurve({ "degree" : size(controlPoints) - 1, "isPeriodic" : false, "controlPoints" : controlPoints });
    opCreateBSplineCurve(context, id, { "bSplineCurve" : bCurve });
}

function addTangentManipulator(context is Context, id is Id, key is string, edge is Query, parameter is number, parameterId is string)
{
    var line = evEdgeTangentLine(context, { "edge" : edge, "parameter" : parameter });
    addManipulators(context, id, { (key) :
                linearManipulator({
                        "base" : line.origin,
                        "direction" : line.direction,
                        "offset" : 0 * inch,
                        "style" : ManipulatorStyleEnum.TANGENTIAL,
                        "primaryParameterId" : parameterId
                    })
            });
}

