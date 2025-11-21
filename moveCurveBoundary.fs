FeatureScript 2815; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/common.fs", version : "2815.0");
export import(path : "onshape/std/movecurveboundarytype.gen.fs", version : "2815.0");
export import(path : "onshape/std/curveextensionendcondition.gen.fs", version : "2815.0");
export import(path : "onshape/std/curveextensionshape.gen.fs", version : "2815.0");

//manipulator related:
const EXTEND_MANIPULATOR = "extendManipulator";
const MIN_EXTEND_VALUE = NONNEGATIVE_LENGTH_BOUNDS[meter][0] * meter;

/**
 * Extend or trim a curve. This is a thin wrapper around [opMoveCurveBoundary].
 */
annotation { "Feature Type Name" : "Trim curve",
        "UIHint" : UIHint.NO_PREVIEW_PROVIDED,
        "Manipulator Change Function" : "extendCurveManipulatorChange" }
export const trimCurve = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Move curve boundary type", "UIHint" : [UIHint.HORIZONTAL_ENUM, UIHint.REMEMBER_PREVIOUS_VALUE] }
        definition.moveBoundaryType is MoveCurveBoundaryType;

        annotation { "Name" : "Curves to adjust", "Filter" : EntityType.BODY && BodyType.WIRE && SketchObject.NO }
        definition.wires is Query;

        if (definition.moveBoundaryType == MoveCurveBoundaryType.EXTEND)
        {
            annotation { "Name" : "End condition", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE] }
            definition.endCondition is CurveExtensionEndCondition;

            if (definition.endCondition == CurveExtensionEndCondition.BLIND)
            {
                annotation { "Name" : "Distance" }
                isLength(definition.extensionDistance, NONNEGATIVE_LENGTH_BOUNDS);
            }
            else
            {
                annotation { "Name" : "Up to entity", "Filter" : (EntityType.BODY && SketchObject.NO) || EntityType.FACE || EntityType.EDGE || EntityType.VERTEX || BodyType.MATE_CONNECTOR }
                definition.extendTo is Query;
            }

            annotation { "Name" : "Extension shape", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE] }
            definition.extensionShape is CurveExtensionShape;
        }
        else
        {
            annotation { "Name" : "Up to entity", "Filter" : (EntityType.BODY && SketchObject.NO) || EntityType.FACE || EntityType.EDGE || EntityType.VERTEX || BodyType.MATE_CONNECTOR }
            definition.trimTo is Query;
        }

        annotation { "Name" : "Help point", "UIHint" : UIHint.PREVENT_CREATING_NEW_MATE_CONNECTORS,
         "Filter" : EntityType.VERTEX || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
        definition.helpPoint is Query;

        annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.flipHeuristics is boolean;
    }
    {
        if (isQueryEmpty(context, definition.wires))
        {
            throw regenError(ErrorStringEnum.MOVE_CURVE_BOUNDARY_SELECT_CURVE, ["wires"]);
        }
        if (definition.moveBoundaryType != MoveCurveBoundaryType.EXTEND && isQueryEmpty(context, definition.trimTo))
        {
            throw regenError(ErrorStringEnum.MOVE_CURVE_BOUNDARY_SELECT_TRIM_BOUNDARY, ["trimTo"]);
        }

        const helpPoint = try silent(evVertexPoint(context, { "vertex" : definition.helpPoint }));
        var endVertices = [];
        try silent
        {
            for (var vertex in findWireEndPoints(context, evaluateQuery(context, definition.wires)[0])) // Set up manipulator for the first wire always
            {
                endVertices = append(endVertices, evVertexPoint(context, { "vertex" : vertex }));
            }
        }

        try silent
        {
            // To support non-linear extension in curvature mode, perform the extend operation first, then anchor the manipulator at the end of the new curve
            opMoveCurveBoundary(context, id, definition);
            if (definition.moveBoundaryType == MoveCurveBoundaryType.EXTEND && helpPoint != undefined)
            {
                addExtendCurveManipulator(context, id, definition, helpPoint, endVertices, true);
            }
        }
        catch (e)
        {
            if (definition.moveBoundaryType == MoveCurveBoundaryType.EXTEND && helpPoint != undefined)
            {
                addExtendCurveManipulator(context, id, definition, helpPoint, endVertices, false);
            }
            throw e;
        }
    });

function addExtendCurveManipulator(context is Context, id is Id, definition is map, helpPoint is Vector, originalWireEnds is array, onSuccess is boolean)
{
    try silent
    {
        if (definition.endCondition == CurveExtensionEndCondition.BLIND)
        {
            const endVertices = findWireEndPoints(context, evaluateQuery(context, definition.wires)[0]);
            if (size(endVertices) == 2) // Wire has only two ends
            {
                const extendOrigin = findExtendOrigin(context, endVertices, helpPoint, originalWireEnds, definition.flipHeuristics, onSuccess);
                if (extendOrigin != undefined)
                {
                    const extendDirection = evaluateTangent(context, extendOrigin);
                    const originPoint = evVertexPoint(context, { "vertex" : extendOrigin });
                    // Use the endpoint of the extended curve or a linearly offset point from the original endpoint
                    const manipulatorOrigin = onSuccess ? originPoint : originPoint + extendDirection * definition.extensionDistance;

                    addManipulators(context, id, { (EXTEND_MANIPULATOR) :
                                linearManipulator({ "base" : manipulatorOrigin,
                                        "direction" : extendDirection,
                                        "offset" : MIN_EXTEND_VALUE,
                                        "primaryParameterId" : "extensionDistance" }) });
                }
            }
        }
    }
}

// Returns a direction vector that extends outward from the wire
function evaluateTangent(context is Context, endPoint is Query)
{
    const edge = evaluateQuery(context, qAdjacent(endPoint, AdjacencyType.VERTEX, EntityType.EDGE))[0];
    const tangentLine = evEdgeTangentLine(context, { "edge" : edge, "parameter" : 0. });
    const tangentLine2 = evEdgeTangentLine(context, { "edge" : edge, "parameter" : 1. });
    // Checks whether the tangent direction extends outward from a multi-edge wire
    const dirSign = !tolerantEquals(evVertexPoint(context, { "vertex" : endPoint }), tangentLine.origin) ? 1.0 : -1.0;

    return (tolerantEquals(evVertexPoint(context, { "vertex" : endPoint }), tangentLine.origin) ?
                tangentLine.direction : tangentLine2.direction) * dirSign;
}

// Returns endpoints of a wire, supports multi-edge as well
function findWireEndPoints(context is Context, wire is Query)
{
    var endVertices = [];

    const vertices = evaluateQuery(context, qOwnedByBody(wire, EntityType.VERTEX));
    for (var vertex in vertices)
    {
        // Indicates that the vertex is adjacent to only one edge of the wire (ends of wire)
        if (size(evaluateQuery(context, qAdjacent(vertex, AdjacencyType.VERTEX))) == 1)
        {
            endVertices = append(endVertices, vertex);
        }
    }

    return endVertices;
}

function findExtendOrigin(context is Context, endVertices is array, helpPoint is Vector, originalWireEnds is array, flipHeuristics is boolean, onSuccess is boolean)
{
    if (onSuccess)
    {
        const ends = qUnion(endVertices);
        // Identify the extended end of the wire by filtering the endpoints of the original wire
        return evaluateQuery(context, ends->qSubtraction(
                    qUnion(
                        [qContainsPoint(ends, originalWireEnds[0]), qContainsPoint(ends, originalWireEnds[1])]
                        )))[0];
    }
    else
    {
        const query = qUnion(endVertices);
        const closest = qClosestTo(query, helpPoint);
        // Find extended end of wire by calculating closest/farthest end to helpPoint
        return !flipHeuristics ? closest : qSubtraction(query, closest);
    }
}

/**
 * @internal
 * Manipulator change function for `Extend Curve Boundaries`.
 */
export function extendCurveManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    try silent
    {
        if (newManipulators[EXTEND_MANIPULATOR] is map && newManipulators[EXTEND_MANIPULATOR].offset != undefined)
        {
            const newValue = newManipulators[EXTEND_MANIPULATOR].offset;
            if (tolerantLessThanOrEqual(definition.extensionDistance + newValue.value * meter, 0. * meter))
            {
                definition.extensionDistance = MIN_EXTEND_VALUE;
            }
            else
            {
                definition.extensionDistance += newValue;
            }
        }
    }
    return definition;
}

