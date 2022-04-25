FeatureScript 1746; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "1746.0");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/blendcontroltype.gen.fs", version : "1746.0");
export import(path : "onshape/std/filletcrosssection.gen.fs", version : "1746.0");
export import(path : "onshape/std/manipulator.fs", version : "1746.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "1746.0");
import(path : "onshape/std/edgeconvexitytype.gen.fs", version : "1746.0");
import(path : "onshape/std/evaluate.fs", version : "1746.0");
import(path : "onshape/std/feature.fs", version : "1746.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "1746.0");
import(path : "onshape/std/sheetMetalCornerBreak.fs", version : "1746.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "1746.0");
import(path : "onshape/std/string.fs", version : "1746.0");
import(path : "onshape/std/tool.fs", version : "1746.0");
import(path : "onshape/std/valueBounds.fs", version : "1746.0");
import(path : "onshape/std/vector.fs", version : "1746.0");

const FILLET_RHO_BOUNDS =
{
    (unitless) : [0.0, 0.5, 0.99999]
} as RealBoundSpec;

const VR_BLEND_BOUNDS =
{
    (meter)      : [0, 0.005, 500], //allows zero
    (centimeter) : 0.5,
    (millimeter) : 5.0,
    (inch)       : 0.2,
    (foot)       : 0.015,
    (yard)       : 0.005
} as LengthBoundSpec;

/** @internal */
export enum FilletType
{
    annotation { "Name" : "Edge" }
    EDGE,
    annotation { "Name" : "Full round" }
    FULL_ROUND
}

/* @internal */
const EDGE_INTERIOR_PARAMETER_BOUNDS = [0.001, 0.5, 0.999];

/* @internal */
const EDGE_INTERIOR_PARAMETER_BOUNDS_SPEC =
{
    (unitless) : EDGE_INTERIOR_PARAMETER_BOUNDS
} as RealBoundSpec;

/* @internal */
const FS_VARIABLE_RADIUS_ARC_LENGTH_PARAMETERIZATION = false; // For better performance. Must use name different from definition map keys
// [evDistance], [evEdgeTangentLine] etc. called in conjunction with variable radius fillet should use this as value of arcLengthParameterization

/**
 * Feature performing an [opFillet] or [opFullRoundFillet].
 */
annotation { "Feature Type Name" : "Fillet", "Manipulator Change Function" : "filletManipulatorChange",
        "Filter Selector" : "allparts", "Editing Logic Function" : "filletEditLogic" }
export const fillet = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Fillet type", "UIHint" : UIHint.HORIZONTAL_ENUM }
        definition.filletType is FilletType;

        if (definition.filletType == FilletType.EDGE)
        {
            annotation { "Name" : "Entities to fillet",
                        "Filter" : ((ActiveSheetMetal.NO && ((EntityType.EDGE && EdgeTopology.TWO_SIDED) || EntityType.FACE))
                                    || (EntityType.EDGE && SheetMetalDefinitionEntityType.VERTEX))
                            && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES,
                        "AdditionalBoxSelectFilter" : EntityType.EDGE }
            definition.entities is Query;
        }
        else if (definition.filletType == FilletType.FULL_ROUND)
        {
            annotation { "Name" : "First side face",
                        "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES && ActiveSheetMetal.NO, "MaxNumberOfPicks" : 1 }
            definition.side1Face is Query;

            annotation { "Name" : "Second side face",
                        "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES && ActiveSheetMetal.NO, "MaxNumberOfPicks" : 1 }
            definition.side2Face is Query;

            annotation { "Name" : "Faces to round",
                        "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES && ActiveSheetMetal.NO }
            definition.centerFaces is Query;
        }

        annotation { "Name" : "Tangent propagation", "Default" : true }
        definition.tangentPropagation is boolean;

        if (definition.filletType == FilletType.EDGE)
        {
            annotation { "Name" : "Measurement", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE] }
            definition.blendControlType is BlendControlType;

            annotation { "Name" : "Cross section", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE] }
            definition.crossSection is FilletCrossSection;

            if (definition.blendControlType == BlendControlType.RADIUS)
            {
                annotation { "Name" : "Radius", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition.radius, BLEND_BOUNDS);
            }
            else
            {
                annotation { "Name" : "Width", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition.width, BLEND_BOUNDS);
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

            //to show an info only when certain parameters are changed
            annotation { "Name" : "Defaults changed", "UIHint" : UIHint.ALWAYS_HIDDEN }
            definition.defaultsChanged is boolean;

            if (definition.blendControlType == BlendControlType.RADIUS)
            {
                annotation { "Name" : "Variable fillet" }
                definition.isVariable is boolean;

                if (definition.isVariable)
                {
                    annotation { "Group Name" : "Variable fillet", "Driving Parameter" : "isVariable", "Collapsed By Default" : false }
                    {
                        annotation { "Name" : "Vertices", "Item name" : "vertex",
                                    "Driven query" : "vertex", "Item label template" : "[#vertexRadius] #vertex",
                                    "UIHint" : UIHint.PREVENT_ARRAY_REORDER }
                        definition.vertexSettings is array;
                        for (var setting in definition.vertexSettings)
                        {
                            annotation { "Name" : "Vertex", "Filter" : ModifiableEntityOnly.YES && EntityType.VERTEX,
                                        "MaxNumberOfPicks" : 1,
                                        "UIHint" : UIHint.ALWAYS_HIDDEN }
                            setting.vertex is Query;

                            annotation { "Name" : "Radius", "UIHint" : UIHint.MATCH_LAST_ARRAY_ITEM }
                            isLength(setting.vertexRadius, VR_BLEND_BOUNDS);

                            if (definition.crossSection == FilletCrossSection.CONIC)
                            {
                                annotation { "Name" : "Rho", "UIHint" : UIHint.MATCH_LAST_ARRAY_ITEM }
                                isReal(setting.variableRho, FILLET_RHO_BOUNDS);
                            }
                            else if (definition.crossSection == FilletCrossSection.CURVATURE)
                            {
                                annotation { "Name" : "Magnitude", "UIHint" : UIHint.MATCH_LAST_ARRAY_ITEM }
                                isReal(setting.variableMagnitude, FILLET_RHO_BOUNDS);
                            }
                        }

                        annotation { "Name" : "Points on edges", "Item name" : "point on edge",
                                    "Item label template" : "[#pointOnEdgeRadius] #edge",
                                    "UIHint" : UIHint.PREVENT_ARRAY_REORDER }
                        definition.pointOnEdgeSettings is array;
                        for (var setting in definition.pointOnEdgeSettings)
                        {
                            annotation { "Name" : "Edge",
                                        "Filter" : ModifiableEntityOnly.YES && EntityType.EDGE && ConstructionObject.NO,
                                        "MaxNumberOfPicks" : 1 }
                            setting.edge is Query;

                            // Edge parameter (number) defined in accordance with FS_VARIABLE_RADIUS_ARC_LENGTH_PARAMETERIZATION
                            annotation { "Name" : "Location" }
                            isReal(setting.edgeParameter, EDGE_INTERIOR_PARAMETER_BOUNDS_SPEC);

                            annotation { "Name" : "Radius", "UIHint" : UIHint.MATCH_LAST_ARRAY_ITEM }
                            isLength(setting.pointOnEdgeRadius, VR_BLEND_BOUNDS);

                            if (definition.crossSection == FilletCrossSection.CONIC)
                            {
                                annotation { "Name" : "Rho", "UIHint" : UIHint.MATCH_LAST_ARRAY_ITEM }
                                isReal(setting.pointOnEdgeVariableRho, FILLET_RHO_BOUNDS);
                            }
                            else if (definition.crossSection == FilletCrossSection.CURVATURE)
                            {
                                annotation { "Name" : "Magnitude", "UIHint" : UIHint.MATCH_LAST_ARRAY_ITEM }
                                isReal(setting.pointOnEdgeVariableMagnitude, FILLET_RHO_BOUNDS);
                            }
                        }
                        annotation { "Name" : "Smooth transition" }
                        definition.smoothTransition is boolean;
                    }
                }

                if (definition.crossSection != FilletCrossSection.CURVATURE)
                {
                    annotation { "Name" : "Allow edge overflow", "Default" : true }
                    definition.allowEdgeOverflow is boolean;

                    if (definition.allowEdgeOverflow)
                    {
                       annotation { "Group Name" : "Edge overflow", "Driving Parameter" : "allowEdgeOverflow" }
                       {
                           annotation { "Name" : "Edges to keep",
                                        "Filter" : EntityType.EDGE && EdgeTopology.TWO_SIDED && ConstructionObject.NO &&
                                                   SketchObject.NO && ModifiableEntityOnly.YES }
                           definition.keepEdges is Query;
                       }
                    }

                    annotation { "Name" : "Smooth fillet corners" }
                    definition.smoothCorners is boolean;

                    if (definition.smoothCorners)
                    {
                        annotation { "Group Name" : "Smooth fillet corners", "Driving Parameter" : "smoothCorners", "Collapsed By Default" : false }
                        {
                            annotation { "Name" : "Corners to exclude",
                                         "Filter" : EntityType.VERTEX && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES }
                            definition.smoothCornerExceptions is Query;
                        }
                    }
                }
            }
        }
    }
    {
        verifyNoMesh(context, definition, "entities");

        if (definition.filletType == FilletType.EDGE)
        {
            performEdgeFillet(context, id, definition);
        }
        else if (definition.filletType == FilletType.FULL_ROUND)
        {
            performFullRoundFillet(context, id, definition);
        }
    },
    {
        tangentPropagation : false,
        crossSection : FilletCrossSection.CIRCULAR,
        isVariable : false,
        smoothTransition : false,
        defaultsChanged : false,
        allowEdgeOverflow : true,
        keepEdges : qNothing(),
        filletType : FilletType.EDGE,
        blendControlType : BlendControlType.RADIUS,
        vertexSettings : [],
        pointOnEdgeSettings : [],
        smoothCorners : false,
        smoothCornerExceptions : qNothing()
    });


function performFullRoundFillet(context is Context, topLevelId is Id, definition is map)
{
    const allSelections =  qUnion([definition.side1Face, definition.side2Face, definition.centerFaces]);
    if (queryContainsActiveSheetMetal(context, allSelections))
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_PARTS_PROHIBITED, allSelections);
    }
    opFullRoundFillet(context, topLevelId, definition);
}

function performEdgeFillet(context is Context, topLevelId is Id, definition is map)
{
    definition.allowEdgeOverflow = (definition.crossSection == FilletCrossSection.CURVATURE) ? true : definition.allowEdgeOverflow;

    if (definition.blendControlType != BlendControlType.RADIUS || !definition.isVariable)
    {
        try(addFilletManipulator(context, topLevelId, definition));
    }
    else
    {
        // variableFilletArcLengthParameterization is not a feature parameter, but we pass it to server using definition
        definition.variableFilletArcLengthParameterization = FS_VARIABLE_RADIUS_ARC_LENGTH_PARAMETERIZATION;
        if (!isInFeaturePattern(context))
        {
            try(addPointOnEdgeManipulators(context, topLevelId, definition));
        }
    }
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V575_SHEET_METAL_FILLET_CHAMFER))
    {
        sheetMetalAwareFillet(context, topLevelId, definition);
    }
    else
    {
        opFillet(context, topLevelId, definition);
    }

    if (!definition.defaultsChanged) //defaults did not change, suppress info if needed
    {
        var result = getFeatureStatus(context, topLevelId);
        if (result != undefined && result.statusEnum == ErrorStringEnum.VRFILLET_NO_EFFECT)
        {
            clearFeatureStatus(context, topLevelId, { "withDisplayData" : false }); //keep supplemental graphics
        }
    }
}

/**
 * @internal
 * Edit logic to check if a default value has been changed. So that we only display the info when needed
 */
export function filletEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    definition.defaultsChanged = false;
    if (specifiedParameters.radius || specifiedParameters.rho || specifiedParameters.magnitude)
    {
        if (definition.radius != oldDefinition.radius ||
            definition.rho != oldDefinition.rho ||
            definition.magnitude != oldDefinition.magnitude)
        {
            definition.defaultsChanged = true; //default values are being changed, server decides if they're being used anywhere
        }
    }
    return definition;
}


/*
 * Call sheetMetalCornerBreak on active sheet metal entities and opFillet on the remaining entities
 */
function sheetMetalAwareFillet(context is Context, id is Id, definition is map)
{
    var separatedQueries = separateSheetMetalQueries(context, definition.entities);
    var hasSheetMetalQueries = !isQueryEmpty(context, separatedQueries.sheetMetalQueries);
    var hasNonSheetMetalQueries = !isQueryEmpty(context, separatedQueries.nonSheetMetalQueries);

    if (!hasSheetMetalQueries && !hasNonSheetMetalQueries)
    {
        throw regenError(ErrorStringEnum.FILLET_SELECT_EDGES, ["entities"]);
    }

    if (hasSheetMetalQueries)
    {
        if (definition.blendControlType == BlendControlType.WIDTH)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_FILLET_NO_WIDTH, ["blendControlType"]);
        }
        if (definition.crossSection != FilletCrossSection.CIRCULAR)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_FILLET_NO_CONIC, ["crossSection"]);
        }
        if (definition.isVariable)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_FILLET_NO_CONIC, ["isVariable"]);
        }
        var cornerBreakDefinition = {
            "entities" : separatedQueries.sheetMetalQueries,
            "cornerBreakStyle" : SMCornerBreakStyle.FILLET,
            "range" : definition.radius
        };
        callSubfeatureAndProcessStatus(id, sheetMetalCornerBreak, context, id + "smFillet", cornerBreakDefinition);
    }

    if (hasNonSheetMetalQueries)
    {
        definition.entities = separatedQueries.nonSheetMetalQueries;
        opFillet(context, id, definition);
    }
}

const FILLET_RADIUS_MANIPULATOR = "filletRadiusManipulator";
const FILLET_WIDTH_MANIPULATOR = "filletWidthManipulator";
const POINT_ON_EDGE_MANIPULATOR = "pointOnEdgeManipulator";

function getManipulatorId(definition is map) returns string
{
    return definition.blendControlType == BlendControlType.RADIUS ? FILLET_RADIUS_MANIPULATOR : FILLET_WIDTH_MANIPULATOR;
}

/*
 * Create a linear manipulator for the fillet
 */
function addFilletManipulator(context is Context, id is Id, definition is map)
{
    // get last last edge (or arbitrary edge of the last face) from the qlv
    const operativeEntity = try(findManipulationEntity(context, definition));
    if (operativeEntity != undefined)
    {
        // convert given radius and edge topology into origin, direction, and offset
        const origin = evEdgeTangentLine(context, { "edge" : operativeEntity, "parameter" : 0.5 }).origin;
        const normals = try(findSurfaceNormalsAtEdge(context, operativeEntity, origin));
        if (normals != undefined && !parallelVectors(normals[0], normals[1]))
        {
            const direction = normalize(normals[0] + normals[1]);

            var convexity = 1.0;
            const bounds = boundsRange(BLEND_BOUNDS);
            var minDragValue = bounds[0];
            var maxDragValue = bounds[1];
            if (isEdgeConvex(context, operativeEntity))
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
            const manipulatorId = getManipulatorId(definition);
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
}

/*
 * Create linear tangtential manipulators for points on edges with the variable radii
 */
function addPointOnEdgeManipulators(context is Context, id is Id, definition is map)
{
    if (definition.blendControlType != BlendControlType.RADIUS || !definition.isVariable)
    {
        return;
    }
    for (var ii = 0; ii < size(definition.pointOnEdgeSettings); ii += 1)
    {
        var setting = definition.pointOnEdgeSettings[ii];
        var qEdges = evaluateQuery(context, qEntityFilter(setting.edge, EntityType.EDGE));
        var nEdges = size(qEdges);

        if (nEdges > 1)
        {
            throw regenError("Variable radius fillet has more than one edge selected in a particular setting");
        }
        else if (nEdges == 1)
        {
            var lines = evEdgeTangentLines(context, { "edge" : qEdges[0],
                    "parameters" : [EDGE_INTERIOR_PARAMETER_BOUNDS[0], setting.edgeParameter, EDGE_INTERIOR_PARAMETER_BOUNDS[2]],
                    "arcLengthParameterization" : FS_VARIABLE_RADIUS_ARC_LENGTH_PARAMETERIZATION });
            addManipulators(context, id, { POINT_ON_EDGE_MANIPULATOR ~ '.' ~ toString(ii) :
                        linearManipulator({
                                "base" : lines[1].origin,
                                "direction" : lines[1].direction,
                                "offset" : 0 * inch,
                                "minValue" : -norm(lines[0].origin - lines[1].origin),
                                "maxValue" : norm(lines[2].origin - lines[1].origin),
                                "style" : ManipulatorStyleEnum.TANGENTIAL }) });
        }
    }
}

/**
 * @internal
 * Manipulator change function for `fillet`.
 */
export function filletManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    try
    {
        const manipulatorId = getManipulatorId(definition);
        if (newManipulators[manipulatorId] is map)
        {
            // convert given offset and edge topology into new radius
            const operativeEntity = findManipulationEntity(context, definition);
            const origin = evEdgeTangentLine(context, { "edge" : operativeEntity, "parameter" : 0.5 }).origin;
            const normals = findSurfaceNormalsAtEdge(context, operativeEntity, origin);
            const convexity = isEdgeConvex(context, operativeEntity) ? -1.0 : 1.0;

            const radius = convexity * newManipulators[manipulatorId].offset / findRadiusToOffsetRatio(normals);
            if (definition.blendControlType == BlendControlType.RADIUS)
            {
                definition.radius = radius;
            }
            else
            {
                definition.width = radius * (normals[0] - normals[1])->norm();
            }
        }
        else // points on edges manipulators
        {
            var manipulatorKeys = keys(newManipulators);
            for (var ii = 0; ii < size(manipulatorKeys); ii += 1)
            {
                var manipulator = newManipulators[manipulatorKeys[ii]];
                if (abs(manipulator.offset) < TOLERANCE.zeroLength * meter)
                    continue;
                var settingIndex = stringToNumber(replace(manipulatorKeys[ii], POINT_ON_EDGE_MANIPULATOR ~ '.', ""));
                var pos = manipulator.base + manipulator.direction * manipulator.offset;
                var qEdges = evaluateQuery(context, qEntityFilter(definition.pointOnEdgeSettings[settingIndex].edge, EntityType.EDGE));
                var distanceResult is DistanceResult = evDistance(context, { "side0" : pos, "side1" : qEdges[0],
                        "arcLengthParameterization" : FS_VARIABLE_RADIUS_ARC_LENGTH_PARAMETERIZATION });
                var parameter = distanceResult.sides[1].parameter;
                if (parameter < EDGE_INTERIOR_PARAMETER_BOUNDS[0])
                {
                    parameter = EDGE_INTERIOR_PARAMETER_BOUNDS[0];
                }
                else if (parameter > EDGE_INTERIOR_PARAMETER_BOUNDS[2])
                {
                    parameter = EDGE_INTERIOR_PARAMETER_BOUNDS[2];
                }
                definition.pointOnEdgeSettings[settingIndex].edgeParameter = parameter;
            }
        }
    }

    return definition;
}

/*
 * Find the final element in the qlv.
 * If it is an edge, return it.
 * If it is a face, return one of its edges arbitrarily
 */
function findManipulationEntity(context is Context, definition is map) returns Query
{
    const resolvedEntities = evaluateQuery(context, definition.entities);
    if (@size(resolvedEntities) > 0)
    {
        var operativeEntity = resolvedEntities[@size(resolvedEntities) - 1];
        if (!isQueryEmpty(context, qEntityFilter(operativeEntity, EntityType.FACE)))
        {
            operativeEntity = evaluateQuery(context, qAdjacent(operativeEntity, AdjacencyType.EDGE, EntityType.EDGE))[0];
        }
        return operativeEntity;
    }
    throw {};
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

