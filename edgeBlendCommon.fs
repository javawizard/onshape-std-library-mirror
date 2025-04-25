FeatureScript 2641; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/blendcontroltype.gen.fs", version : "2641.0");
import(path : "onshape/std/chamfermethod.gen.fs", version : "2641.0");
import(path : "onshape/std/chamfertype.gen.fs", version : "2641.0");
import(path : "onshape/std/containers.fs", version : "2641.0");
import(path : "onshape/std/edgeconvexitytype.gen.fs", version : "2641.0");
import(path : "onshape/std/evaluate.fs", version : "2641.0");
import(path : "onshape/std/feature.fs", version : "2641.0");
import(path : "onshape/std/filletcrosssection.gen.fs", version : "2641.0");
import(path : "onshape/std/manipulator.fs", version : "2641.0");
import(path : "onshape/std/math.fs", version : "2641.0");
import(path : "onshape/std/valueBounds.fs", version : "2641.0");
import(path : "onshape/std/vector.fs", version : "2641.0");

/**
*   @internal
*  Used by chamfer and sheetMetalCornerBreak
*/
export const CHAMFER_ANGLE_BOUNDS =
{
    (degree) : [0.1, 45, 179.9],
    (radian) : 0.25 * PI
} as AngleBoundSpec;

/**
* @internal
* part of chamfer predicate shared with sheetMetalCornerBreak
*/
export predicate chamferCommonOptions(definition is map)
{
    annotation { "Name" : "Measurement", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE] }
    definition.chamferMethod is ChamferMethod;

    if (definition.chamferType != undefined)
    {
        annotation { "Name" : "Chamfer type", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE] }
        definition.chamferType is ChamferType;
    }

    //first quantity input (length)
    if (definition.chamferType != ChamferType.TWO_OFFSETS)
    {
        annotation { "Name" : "Distance", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        isLength(definition.width, BLEND_BOUNDS);
    }
    else
    {
        annotation { "Name" : "Distance 1" }
        isLength(definition.width1, BLEND_BOUNDS);
    }

    //opposite direction button
    if (definition.chamferType == ChamferType.OFFSET_ANGLE ||
        definition.chamferType == ChamferType.TWO_OFFSETS)
    {
        annotation { "Name" : "Opposite direction", "Default" : false,  "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.oppositeDirection is boolean;
    }

    //second quantity input (length or angle depending on type)
    if (definition.chamferType == ChamferType.TWO_OFFSETS)
    {
        annotation { "Name" : "Distance 2" }
        isLength(definition.width2, BLEND_BOUNDS);
    }
    else if (definition.chamferType == ChamferType.OFFSET_ANGLE)
    {
        annotation { "Name" : "Angle" }
        isAngle(definition.angle, CHAMFER_ANGLE_BOUNDS);
    }
}
/**
* @internal
* part of fillet predicate shared with sheetMetalCornerBreak
*/
export predicate edgeFilletCommonOptions(definition is map, widthFieldName is string)
{
    annotation { "Name" : "Measurement", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE] }
    definition.blendControlType is BlendControlType;

    annotation { "Name" : "Control", "Description" : "Cross sectional control", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE] }
    definition.crossSection is FilletCrossSection;

    if (definition.blendControlType == BlendControlType.RADIUS)
    {
        annotation { "Name" : "Radius", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        isLength(definition.radius, BLEND_BOUNDS);
    }
    else
    {
        annotation { "Name" : "Width", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        isLength(definition[widthFieldName], BLEND_BOUNDS);
    }

    if (definition.crossSection == FilletCrossSection.CONIC)
    {
        annotation { "Name" : "Rho" }
        isReal(definition.rho, FILLET_RHO_BOUNDS);
    }
    else if (definition.crossSection == FilletCrossSection.CURVATURE)
    {
        annotation { "Name" : "Magnitude" }
        isReal(definition.magnitude, FILLET_RHO_BOUNDS);
    }
}

/**
*   @internal
*/
export predicate asymmetricFilletOption(definition is map)
{
    if (definition.blendControlType == BlendControlType.RADIUS)
    {
        annotation { "Name" : "Asymmetric" }
        definition.isAsymmetric is boolean;

        if (definition.isAsymmetric)
        {
            annotation { "Name" : "Second radius", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            isLength(definition.otherRadius, BLEND_BOUNDS);

            annotation { "Name" : "Flip asymmetric", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.flipAsymmetric is boolean;
        }
    }
}


const FILLET_RADIUS_MANIPULATOR = "filletRadiusManipulator";
const FILLET_WIDTH_MANIPULATOR = "filletWidthManipulator";

/**
*   @internal
*/
export function getFilletControlManipulatorId(definition is map) returns string
{
    return definition.blendControlType == BlendControlType.RADIUS ? FILLET_RADIUS_MANIPULATOR : FILLET_WIDTH_MANIPULATOR;
}


/**
 * Create a linear manipulator for radius or width parameter
 */
export function addFilletControlManipulator(context is Context, id is Id, definition is map, manipulatorEntity is Query)
{
    // convert given radius and edge topology into origin, direction, and offset
    const origin = evEdgeTangentLine(context, { "edge" : manipulatorEntity, "parameter" : 0.5 }).origin;
    const normals = try(findSurfaceNormalsAtEdge(context, manipulatorEntity, origin));
    if (normals != undefined && !parallelVectors(normals[0], normals[1]))
    {
        const direction = normalize(normals[0] + normals[1]);

        var convexity = 1.0;
        const bounds = boundsRange(BLEND_BOUNDS);
        var minDragValue = bounds[0];
        var maxDragValue = bounds[1];
        if (isEdgeConvex(context, manipulatorEntity))
        {
            convexity = -1.0;
            const tempMin = minDragValue;
            minDragValue = -maxDragValue;
            maxDragValue = -tempMin;
        }

        var offset;
        if (definition.blendControlType == BlendControlType.RADIUS)
        {
            offset = convexity * definition.radius * findRadiusToOffsetRatio(normals);
        }
        else
        {
            offset = convexity * definition.width * findRadiusToOffsetRatio(normals) / (normals[0] - normals[1])->norm();
        }

        const primaryParameterId = definition.blendControlType == BlendControlType.RADIUS ? "radius" : "width";
        // The undo stack entry is dependent on the manipulator id, so alter it based on the quantity being edited.
        const manipulatorId = getFilletControlManipulatorId(definition);
        addManipulators(context, id, {
                    (manipulatorId) : linearManipulator({
                            "base" : origin,
                            "direction" : direction,
                            "offset" : offset,
                            "minValue" : minDragValue,
                            "maxValue" : maxDragValue,
                            "primaryParameterId" : primaryParameterId
                        })
                });
    }
}
/**
 *  fillet manipulator change function
 */
export function onFilletControlManipulatorChange(context is Context, definition is map, newManipulators is map, manipulatorEntity is Query, widthFieldName is string) returns map
{
    try
    {
        const manipulatorId = getFilletControlManipulatorId(definition);
        if (newManipulators[manipulatorId] is map)
        {
            // convert given offset and edge topology into new radius
            const origin = evEdgeTangentLine(context, { "edge" : manipulatorEntity, "parameter" : 0.5 }).origin;
            const normals = findSurfaceNormalsAtEdge(context, manipulatorEntity, origin);
            const convexity = isEdgeConvex(context, manipulatorEntity) ? -1.0 : 1.0;

            const radius = convexity * newManipulators[manipulatorId].offset / findRadiusToOffsetRatio(normals);
            if (definition.blendControlType == BlendControlType.RADIUS)
            {
                definition.radius = radius;
            }
            else
            {
                definition[widthFieldName] = radius * (normals[0] - normals[1])->norm();
            }
        }
    }
    return definition;
}

/*
 * Find surface normals at the point closest to edgePoint on the two faces attached to the given edge.
 * Returns undefined if the edge does not have two faces adjacent to it.
 */
function findSurfaceNormalsAtEdge(context is Context, edge is Query, edgePoint is Vector)
{
    const faces = evaluateQuery(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE));
    if (size(faces) < 2)
        return undefined;

    var normals = makeArray(2);
    for (var i = 0; i < 2; i += 1)
    {
        const param = evDistance(context, { "side0" : faces[i], "side1" : edgePoint }).sides[0].parameter;
        const plane = evFaceTangentPlane(context, {
                    "face" : faces[i],
                    "parameter" : param
                });

        normals[i] = plane.normal;
    }
    return normals;
}

function isEdgeConvex(context is Context, edge is Query) returns boolean
{
    return evEdgeConvexity(context, { "edge" : edge }) == EdgeConvexityType.CONVEX;
}

/*
 * The distance from the center of a corner-inscribed circle to the corner itself is:
 * radius / cos(0.5 * angle between surface normals)
 * Therefore, the distance between the outer ege of the circle and the corner (the offset of the manipulator) is:
 * (radius / cos(0.5 * angle between surface normals)) - radius
 * So:
 * offset = radius * ((1.0 / cos(0.5 * angle between surface normals)) - 1.0)
 */
function findRadiusToOffsetRatio(normalArray is array) returns number
{
    return (1.0 / cos(0.5 * angleBetween(normalArray[0], normalArray[1]))) - 1.0;
}

