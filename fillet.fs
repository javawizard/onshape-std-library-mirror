FeatureScript 2796; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2796.0");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/blendcontroltype.gen.fs", version : "2796.0");
export import(path : "onshape/std/edgeBlendCommon.fs", version : "2796.0");
export import(path : "onshape/std/filletcrosssection.gen.fs", version : "2796.0");
export import(path : "onshape/std/manipulator.fs", version : "2796.0");
export import(path : "onshape/std/surfacetype.gen.fs", version : "2796.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "2796.0");
import(path : "onshape/std/edgeconvexitytype.gen.fs", version : "2796.0");
import(path : "onshape/std/evaluate.fs", version : "2796.0");
import(path : "onshape/std/feature.fs", version : "2796.0");
import(path : "onshape/std/path.fs", version : "2796.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2796.0");
import(path : "onshape/std/sheetMetalCornerBreakAttributeBased.fs", version : "2796.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2796.0");
import(path : "onshape/std/string.fs", version : "2796.0");
import(path : "onshape/std/tool.fs", version : "2796.0");
import(path : "onshape/std/valueBounds.fs", version : "2796.0");
import(path : "onshape/std/vector.fs", version : "2796.0");
import(path : "onshape/std/offsetSurface.fs", version : "2796.0");
import(path : "onshape/std/curveGeometry.fs", version : "2796.0");
import(path : "onshape/std/topologyUtils.fs", version : "2796.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2796.0");

const VR_BLEND_BOUNDS = {
            (meter) : [0, 0.005, 500], //allows zero
            (centimeter) : 0.5,
            (millimeter) : 5.0,
            (inch) : 0.2,
            (foot) : 0.015,
            (yard) : 0.005
        } as LengthBoundSpec;

/** @internal */
export enum FilletType
{
    annotation { "Name" : "Edge" }
    EDGE,
    annotation { "Name" : "Full round" }
    FULL_ROUND
}

/** @internal */
export enum EndTypePartialFillet
{
    annotation { "Name" : "Position" }
    PERCENTAGE,
    annotation { "Name" : "Entity" }
    ENTITY,
    annotation { "Name" : "Offset" }
    OFFSET
}

/* @internal */
const EDGE_INTERIOR_PARAMETER_BOUNDS = [0.001, 0.5, 0.999];

/* @internal */
const EDGE_INTERIOR_PARAMETER_BOUNDS_SPEC =
{
    (unitless) : EDGE_INTERIOR_PARAMETER_BOUNDS
} as RealBoundSpec;

/* @internal */
const PARTIAL_EDGE_INTERIOR_PARAMETER_BOUNDS = [0.0, 0.01, 1.0];

/* @internal */
const PARTIAL_EDGE_INTERIOR_PARAMETER_BOUNDS_SPEC =
{
    (unitless) : PARTIAL_EDGE_INTERIOR_PARAMETER_BOUNDS
} as RealBoundSpec;

/* @internal */
const PARTIAL_EDGE_INTERIOR_PARAMETER_BOUNDS2 = [0.0, 0.99, 1.0];

/* @internal */
const PARTIAL_EDGE_INTERIOR_PARAMETER_BOUNDS_SPEC2 =
{
    (unitless) : PARTIAL_EDGE_INTERIOR_PARAMETER_BOUNDS2
} as RealBoundSpec;

/* @internal */
const FS_VARIABLE_RADIUS_ARC_LENGTH_PARAMETERIZATION = false; // For better performance. Must use name different from definition map keys
// [evDistance], [evEdgeTangentLine] etc. called in conjunction with variable radius fillet should use this as value of arcLengthParameterization

/* @internal */
const FS_PARTIAL_RADIUS_ARC_LENGTH_PARAMETERIZATION = false;

/* @internal */
const PARAMETER_PRECISION = 3;

/* @internal */
const VARIABLE_FILLET_VERTEX_MAP = {
        "parameter" : "vertexSettings",
        "radius" : "vertexRadius",
        "otherRadius" : "vertexOtherRadius",
        "flipAsymmetric" : "vertexFlipAsymmetric",
        "rho" : "variableRho",
        "magnitude" : "variableMagnitude"
    };

/* @internal */
const VARIABLE_FILLET_EDGE_MAP = {
        "parameter" : "pointOnEdgeSettings",
        "radius" : "pointOnEdgeRadius",
        "otherRadius" : "pointOnEdgeOtherRadius",
        "flipAsymmetric" : "pointOnEdgeFlipAsymmetric",
        "rho" : "pointOnEdgeVariableRho",
        "magnitude" : "pointOnEdgeVariableMagnitude"
    };

const FILLET_WIDTH = "width";

/**
*   Feature performing an [opFillet] or [opFullRoundFillet].
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
            edgeFilletCommonOptions(definition, FILLET_WIDTH);

            //to show an info only when certain parameters are changed
            annotation { "Name" : "Defaults changed", "UIHint" : UIHint.ALWAYS_HIDDEN }
            definition.defaultsChanged is boolean;

            asymmetricFilletOption(definition);

            if (definition.blendControlType == BlendControlType.RADIUS)
            {
                annotation { "Name" : "Partial fillet", "Default" : false }
                definition.isPartial is boolean;

                if (definition.isPartial)
                {
                    annotation { "Group Name" : "Partial fillet", "Driving Parameter" : "isPartial", "Collapsed By Default" : false }
                    {
                        annotation { "Name" : "End type", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE] }
                        definition.startPartialType is EndTypePartialFillet;

                        if (definition.startPartialType == EndTypePartialFillet.OFFSET)
                        {
                            annotation { "Name" : "Start offset" }
                            isLength(definition.startPartialOffset, VR_BLEND_BOUNDS);
                        }
                        else if (definition.startPartialType == EndTypePartialFillet.ENTITY)
                        {
                            annotation {"Name" : "Select an entity or mate connector",
                                "Filter" : (EntityType.FACE && AllowMeshGeometry.YES) || QueryFilterCompound.ALLOWS_VERTEX,
                                "MaxNumberOfPicks" : 1 }
                            definition.startPartialEntity is Query;
                        }
                        else if (definition.startPartialType == EndTypePartialFillet.PERCENTAGE)
                        {
                            annotation { "Name" : "Start position" }
                            isReal(definition.partialFirstEdgeTotalParameter, PARTIAL_EDGE_INTERIOR_PARAMETER_BOUNDS_SPEC);
                        }

                        annotation { "Name" : "Opposite direction", "Default" : true, "UIHint" : [UIHint.OPPOSITE_DIRECTION, UIHint.DISPLAY_SHORT] }
                        definition.partialOppositeParameter is boolean;

                        if (definition.startPartialType == EndTypePartialFillet.ENTITY)
                        {
                            annotation { "Name" : "Trim to face boundaries", "Default" : false }
                            definition.useTrimmedFirstBound is boolean;
                        }

                        annotation { "Name" : "Second bound" }
                        definition.secondBound is boolean;

                        if (definition.secondBound)
                        {
                            annotation { "Name" : "End type", "Column Name" : "Second end type", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE] }
                            definition.endPartialType is EndTypePartialFillet;

                            if (definition.endPartialType == EndTypePartialFillet.OFFSET)
                            {
                                annotation { "Name" : "End offset" }
                                isLength(definition.endPartialOffset, VR_BLEND_BOUNDS);
                            }
                            else if (definition.endPartialType == EndTypePartialFillet.ENTITY)
                            {
                                annotation {"Name" : "Select an entity or mate connector",
                                    "Filter" : (EntityType.FACE && AllowMeshGeometry.YES) || QueryFilterCompound.ALLOWS_VERTEX,
                                    "MaxNumberOfPicks" : 1 }
                                definition.endPartialEntity is Query;

                                annotation { "Name" : "Trim to face boundaries", "Default" : false, "Column Name" : "Second trim to face boundaries" }
                                definition.useTrimmedSecondBound is boolean;
                            }
                            else if (definition.endPartialType == EndTypePartialFillet.PERCENTAGE)
                            {
                                annotation { "Name" : "End position" }
                                isReal(definition.partialSecondEdgeTotalParameter, PARTIAL_EDGE_INTERIOR_PARAMETER_BOUNDS_SPEC2);
                            }
                        }
                    }
                }

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

                            if (definition.isAsymmetric) {
                                annotation { "Name" : "Second radius", "UIHint" : UIHint.MATCH_LAST_ARRAY_ITEM }
                                isLength(setting.vertexOtherRadius, BLEND_BOUNDS);
                                annotation { "Name" : "Flip asymmetric", "UIHint" : [UIHint.OPPOSITE_DIRECTION, UIHint.MATCH_LAST_ARRAY_ITEM] }
                                setting.vertexFlipAsymmetric is boolean;
                            }

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
                                    "UIHint" : [UIHint.PREVENT_ARRAY_REORDER, UIHint.FOCUS_INNER_QUERY] }
                        definition.pointOnEdgeSettings is array;
                        for (var setting in definition.pointOnEdgeSettings)
                        {
                            annotation { "Name" : "Edge",
                                        "Filter" :  SketchObject.NO && ModifiableEntityOnly.YES && EntityType.EDGE && ConstructionObject.NO,
                                        "MaxNumberOfPicks" : 1 }
                            setting.edge is Query;

                            // Edge parameter (number) defined in accordance with FS_VARIABLE_RADIUS_ARC_LENGTH_PARAMETERIZATION
                            annotation { "Name" : "Location" }
                            isReal(setting.edgeParameter, EDGE_INTERIOR_PARAMETER_BOUNDS_SPEC);

                            annotation { "Name" : "Radius", "UIHint" : UIHint.MATCH_LAST_ARRAY_ITEM }
                            isLength(setting.pointOnEdgeRadius, VR_BLEND_BOUNDS);

                            if (definition.isAsymmetric) {
                                annotation { "Name" : "Second radius", "UIHint" : UIHint.MATCH_LAST_ARRAY_ITEM }
                                isLength(setting.pointOnEdgeOtherRadius, BLEND_BOUNDS);
                                annotation { "Name" : "Flip asymmetric", "UIHint" : [UIHint.OPPOSITE_DIRECTION, UIHint.MATCH_LAST_ARRAY_ITEM] }
                                setting.pointOnEdgeFlipAsymmetric is boolean;
                            }

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
        smoothCornerExceptions : qNothing(),
        isAsymmetric : false,
        isPartial : false,
        startPartialType: EndTypePartialFillet.PERCENTAGE,
        endPartialType: EndTypePartialFillet.PERCENTAGE,
        useTrimmedFirstBound : false,
        useTrimmedSecondBound : false
    });


function performFullRoundFillet(context is Context, topLevelId is Id, definition is map)
{
    const allSelections = qUnion([definition.side1Face, definition.side2Face, definition.centerFaces]);
    if (queryContainsActiveSheetMetal(context, allSelections))
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_PARTS_PROHIBITED, allSelections);
    }
    opFullRoundFillet(context, topLevelId, definition);
}

function performEdgeFillet(context is Context, topLevelId is Id, definition is map)
{
    definition.allowEdgeOverflow = (definition.crossSection == FilletCrossSection.CURVATURE) ? true : definition.allowEdgeOverflow;

    var facesToDelete = qNothing();
    if (definition.isPartial && definition.blendControlType == BlendControlType.RADIUS)
    {
        definition.partialArcLengthParameterization = FS_PARTIAL_RADIUS_ARC_LENGTH_PARAMETERIZATION;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2479_PF_ADDED_ENTITY_END_CONDITION))
        {
            const partialFilletData = generatePartialFilletData(context, topLevelId, definition);
            definition.partialFilletBounds = partialFilletData.partialFilletBounds;
            definition.partialFilletCapBounds = partialFilletData.partialFilletCapBounds;
            facesToDelete = partialFilletData.facesToDelete;
            definition.entities = partialFilletData.filterEntities;
        }
        else
        {
            const partialFilletData = generatePartialFilletDataPreV2464(context, topLevelId, definition);
            definition.partialFilletBounds = partialFilletData.partialFilletBounds;
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1917_PARTIAL_FILLET_FIX_CHAIN_OF_EDGES))
            {
                definition.entities = partialFilletData.filterEntities;
            }
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1968_PARTIAL_FILLET_CHECK_INVALID_BOUNDS) && definition.secondBound && abs(definition.partialFirstEdgeTotalParameter - definition.partialSecondEdgeTotalParameter) < TOLERANCE.zeroLength)
            {
                throw regenError(ErrorStringEnum.PARTIAL_FILLET_INVALID_BOUNDS_ERROR);
            }
        }
    }

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

    if (!isQueryEmpty(context, facesToDelete))
    {
        opDeleteFace(context, topLevelId + "deleteFace1", {
            "deleteFaces" : facesToDelete,
            "includeFillet" : false,
            "capVoid" : false,
            "leaveOpen" : false
        });
    }
}

/** @internal */
function getPath(context is Context, definition is map, edges is Query) returns Path
{
    const separatedQueries = separateSheetMetalQueries(context, definition.entities);

    if (!isQueryEmpty(context, separatedQueries.sheetMetalQueries))
    {
        throw regenError(ErrorStringEnum.CANNOT_USE_PARTIAL_FILLET_IN_SHEET_METAL, ["entities"], separatedQueries.sheetMetalQueries);
    }

    var paths;
    try
    {
        const tangentEdges = definition.tangentPropagation ? qTangentConnectedEdges(edges) : edges;
        paths = constructPaths(context, tangentEdges, {});
    }
    catch
    {
        throw regenError(ErrorStringEnum.PATH_EDGES_NOT_CONTINUOUS, ["edges"], edges);
    }

    if (size(paths) != 1)
    {
        throw regenError(ErrorStringEnum.PARTIAL_FILLET_BAD_INPUT_ERROR, ["edges"], edges);
    }

    if (paths[0].closed && !definition.secondBound)
    {
        throw regenError(ErrorStringEnum.PARTIAL_FILLET_CLOSED_PATH_ERROR, ["edges"], edges);
    }

    return paths[0];
}

/** @internal */
function generatePartialFilletDataPreV2464(context is Context, topLevelId is Id, definition is map) returns map
{
    const edges = qEntityFilter(definition.entities, EntityType.EDGE);
    const path = getPath(context, definition, edges);
    var params = [definition.partialFirstEdgeTotalParameter] as array;
    if (definition.secondBound)
    {
        params = append(params, definition.partialSecondEdgeTotalParameter);
    }

    var partialFilletBounds = [];
    var flipDirection = definition.partialOppositeParameter;

    const lines = evPathTangentLines(context, path, params);
    const tangentLinesCount = size(lines.tangentLines);
    const pathLength = evPathLength(context, path);
    for (var i = 0; i < tangentLinesCount; i += 1)
    {
        if (!isInFeaturePattern(context))
        {
            var param = i == 0 ? definition.partialFirstEdgeTotalParameter : definition.partialSecondEdgeTotalParameter;
            param = flipDirection ? param : 1 - param;
            const direction = flipDirection ? -lines.tangentLines[i].direction : lines.tangentLines[i].direction;
            const offset = pathLength * param;
            const position = lines.tangentLines[i].origin + (direction * offset);
            const primaryParameterId = i == 0 ? "partialFirstEdgeTotalParameter" : "partialSecondEdgeTotalParameter";

            try(addPartialFilletManipulators(context, topLevelId, position, direction, offset, pathLength, i, primaryParameterId));
        }

        const boundaryEdge = path.edges[lines.edgeIndices[i]];
        const qEdges = evaluateQuery(context, qEntityFilter(boundaryEdge, EntityType.EDGE));
        const boundaryParameter = evDistance(context, { "side0" : lines.tangentLines[i].origin, "side1" : qEdges[0],
                    "arcLengthParameterization" : FS_PARTIAL_RADIUS_ARC_LENGTH_PARAMETERIZATION }).sides[1].parameter;

        partialFilletBounds = append(partialFilletBounds, { "boundaryEdge" : boundaryEdge, "boundaryParameter" : boundaryParameter, "isFlipped" : path.flipped[lines.edgeIndices[i]] != flipDirection });

        flipDirection = !flipDirection;
    }

    var filterEntities = qNothing();
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1968_PARTIAL_FILLET_CHECK_INVALID_BOUNDS))
    {
        const inverseBounds = definition.secondBound ? definition.partialFirstEdgeTotalParameter > definition.partialSecondEdgeTotalParameter : false;
        if (!inverseBounds)
        {
            filterEntities = qUnion(subArray(path.edges, lines.edgeIndices[0], definition.secondBound ? lines.edgeIndices[1] : size(path.edges)));
        }
        else
        {
            filterEntities = qUnion(subArray(path.edges, lines.edgeIndices[1], lines.edgeIndices[0]));
        }
        if (inverseBounds == definition.partialOppositeParameter)
        {
            filterEntities = qSubtraction(edges, filterEntities);
        }
    }
    else
    {
        filterEntities = qUnion(subArray(path.edges, lines.edgeIndices[0], definition.secondBound ? lines.edgeIndices[1] : size(path.edges)));
        if (!definition.partialOppositeParameter)
        {
            filterEntities = qSubtraction(edges, filterEntities);
        }
    }

    for (var i = 0; i < size(lines.edgeIndices); i += 1)
    {
         filterEntities = qUnion([filterEntities, path.edges[lines.edgeIndices[i]]]);
    }

    return {"filterEntities": filterEntities, "partialFilletBounds": partialFilletBounds};
}

/** @internal */
function generatePartialFilletData(context is Context, topLevelId is Id, definition is map) returns map
{
    const edges = qEntityFilter(definition.entities, EntityType.EDGE);
    const path = getPath(context, definition, edges);

    var flipDirection = definition.partialOppositeParameter;

    const partialFilletData = generatePartialFilletDataForBound(context, definition, topLevelId, path, edges, flipDirection, false);

    var partialFilletBounds = partialFilletData.partialFilletBounds;
    var partialFilletCapBounds = partialFilletData.partialFilletCapBounds;
    var facesToDelete = partialFilletData.facesToDelete;
    var filterEntities = partialFilletData.filterEntities;

    if (definition.secondBound)
    {
        flipDirection = !flipDirection;
        const partialFilletSecondData = generatePartialFilletDataForBound(context, definition, topLevelId, path, edges, flipDirection, true);

        if (partialFilletData.edgeIndex == partialFilletSecondData.edgeIndex &&
            abs(partialFilletData.boundaryParameter - partialFilletSecondData.boundaryParameter) < TOLERANCE.zeroLength)
        {
            throw regenError(ErrorStringEnum.PARTIAL_FILLET_INVALID_BOUNDS_ERROR);
        }

        partialFilletBounds = concatenateArrays([partialFilletBounds, partialFilletSecondData.partialFilletBounds]);
        partialFilletCapBounds = concatenateArrays([partialFilletCapBounds, partialFilletSecondData.partialFilletCapBounds]);
        facesToDelete = qUnion([facesToDelete, partialFilletSecondData.facesToDelete]);

        if (partialFilletData.edgeIndex > partialFilletSecondData.edgeIndex ||
            (partialFilletData.edgeIndex == partialFilletSecondData.edgeIndex &&
            partialFilletData.boundaryParameter > partialFilletSecondData.boundaryParameter))
        {
            flipDirection = !flipDirection;
        }

        if (flipDirection)
        {
            filterEntities = qUnion(filterEntities, partialFilletSecondData.filterEntities);
        }
        else
        {
            filterEntities = qIntersection([filterEntities, partialFilletSecondData.filterEntities]);
        }
        filterEntities = qUnion([filterEntities, path.edges[partialFilletSecondData.edgeIndex]]);
    }

    filterEntities = qUnion([filterEntities, path.edges[partialFilletData.edgeIndex]]);

    return {
        "filterEntities": filterEntities,
        "partialFilletBounds": partialFilletBounds,
        "partialFilletCapBounds": partialFilletCapBounds,
        "facesToDelete": facesToDelete
    };
}

/** @internal */
function getNumberOfIntersections(context is Context, id is Id, edges is Query, surface is Query, useTrimmed is boolean) returns number
{
    var numberOfIntersections = 0;
    try
    {
        startFeature(context, id, {});
        // Create temporary wire body(s) to copy edges
        opExtractWires(context, id + "opExtractWires1", {
            "edges" : edges
        });

        // Get edges from created wire body(s)
        const edgesFromWireBody = qCreatedBy(id + "opExtractWires1", EntityType.BODY);
        const numberOfEdgesBeforeSplit = size(evaluateQuery(context, qOwnedByBody(edgesFromWireBody, EntityType.EDGE)));

        // Split edges by inputed surface
        try silent(opSplitPart(context, id + "splitPart1", {
            "targets" : edgesFromWireBody,
            "tool" : surface,
            "useTrimmed" : useTrimmed
        }));

        var closeEdgeModificator is number = 0;
        if (size(evaluateQuery(context, edges)) == 1 && isClosed(context, edges) != isClosed(context, edgesFromWireBody))
        {
            closeEdgeModificator += 1;
        }

        // Return difference in edges before and after split
        const numberOfEdgesAfterSplit = size(evaluateQuery(context, qOwnedByBody(edgesFromWireBody, EntityType.EDGE))) + closeEdgeModificator;
        numberOfIntersections = numberOfEdgesAfterSplit - numberOfEdgesBeforeSplit;
    }
    catch {} // Do not throw an error to allow the server side to execute the feature

    abortFeature(context, id);

    return numberOfIntersections;
}

/** @internal */
function generatePartialFilletDataForBound(context is Context,
    definition is map,
    topLevelId is Id,
    path is Path,
    edges is Query,
    partialDirection is boolean,
    isSecondBound is boolean) returns map
{
    const id = topLevelId + (isSecondBound ? "filletSecondBound" : "filletFirstBound");
    const partialType = isSecondBound ? definition.endPartialType : definition.startPartialType;
    const partialCapEntity = isSecondBound ? definition.endPartialEntity : definition.startPartialEntity;
    var useTrimmedBound = isSecondBound ? definition.useTrimmedSecondBound : definition.useTrimmedFirstBound;

    var flipDirection = definition.partialOppositeParameter;

    var facesToDelete = qNothing();
    var filterEntitiesPerBound = qNothing();

    var partialFilletBounds = [];
    var partialFilletCapBounds = [];

    var edgeIndex = -1;
    var boundaryParameter = 0;
    if (partialType == EndTypePartialFillet.ENTITY)
    {
        if (isQueryEmpty(context, partialCapEntity))
        {
            throw regenError(ErrorStringEnum.PARTIAL_FILLET_INVALID_BOUND_ENTITY);
        }

        const mateConnectorCSys = try silent(evMateConnector(context, { "mateConnector" : partialCapEntity }));
        const point = try silent(evVertexPoint(context, { "vertex" : partialCapEntity}));
        if (point != undefined && mateConnectorCSys == undefined)
        {
            const distanceResult = evDistance(context, {
                "side0" : partialCapEntity,
                "side1" : qUnion(path.edges),
                "arcLengthParameterization" : FS_PARTIAL_RADIUS_ARC_LENGTH_PARAMETERIZATION
            });

            edgeIndex = distanceResult.sides[1].index;
            boundaryParameter = distanceResult.sides[1].parameter;

            const boundaryEdge = path.edges[edgeIndex];
            partialFilletBounds = append(partialFilletBounds, { "boundaryEdge" : boundaryEdge, "boundaryParameter" : boundaryParameter, "isFlipped" : path.flipped[edgeIndex] != partialDirection });
        }
        else
        {
            var distanceResult;
            var capEntity = qNothing();
            var facePlane;
            if (mateConnectorCSys != undefined)
            {
                facePlane = plane(mateConnectorCSys);
                opPlane(context, id + "tempFace", {
                    "plane" : facePlane
                });

                distanceResult = evDistance(context, {
                    "side0" : facePlane,
                    "side1" : qUnion(path.edges),
                    "arcLengthParameterization" : FS_PARTIAL_RADIUS_ARC_LENGTH_PARAMETERIZATION
                });
                useTrimmedBound = false;
            }
            else
            {
                offsetSurface(context, id + "tempFace", {
                    "surfacesAndFaces" : partialCapEntity,
                    "offset" : 0 * meter
                });

                // If surface is type of plane, we need useTrimmed to be false
                const evaluatedSurface = evSurfaceDefinition(context, {
                    "face" : partialCapEntity
                });

                // Do not use trimmed representation for a construction plane
                const isCounstruction is boolean = isQueryEmpty(context, qConstructionFilter(partialCapEntity, ConstructionObject.NO));
                if (isCounstruction)
                    useTrimmedBound = false;

                useTrimmedBound = evaluatedSurface["surfaceType"] == SurfaceType.PLANE ? useTrimmedBound : true;
                distanceResult = evDistance(context, {
                    "side0" : partialCapEntity,
                    "side1" : qUnion(path.edges),
                    "extendSide0" : !useTrimmedBound,
                    "arcLengthParameterization" : FS_PARTIAL_RADIUS_ARC_LENGTH_PARAMETERIZATION
                });

                facePlane = evFaceTangentPlane(context, {
                    "face": partialCapEntity,
                    "parameter": distanceResult.sides[0].parameter
                });
            }

            edgeIndex = distanceResult.sides[1].index;
            boundaryParameter = distanceResult.sides[1].parameter;

            var tempFace = qCreatedBy(id + "tempFace", EntityType.FACE);
            facesToDelete = qUnion([facesToDelete, tempFace]);
            capEntity = tempFace;

            if (getNumberOfIntersections(context, id, qUnion(path.edges), capEntity, useTrimmedBound) > 1)
            {
                throw regenError(ErrorStringEnum.FILLET_ILLEGAL_END_BOUNDARY, isSecondBound ? ["endPartialEntity"] : ["startPartialEntity"], partialCapEntity);
            }

            const edgeTangent = evEdgeTangentLine(context, {
                "edge": path.edges[edgeIndex],
                "parameter": distanceResult.sides[1].parameter
            });

            // fromStart is a flag that specifies which side of the cap will be the exterior.
            // The algorithm trims away the portion of the blend on the side toward which the cap's normal points,
            // if the edge direction and cap normal are co-directed.
            var fromStart = dot(facePlane.normal, edgeTangent.direction) > 0;
            // The direction of the edge itself might be flipped relative to the path's direction,
            // so we flip the 'fromStart' flag accordingly.
            if (path.flipped[edgeIndex])
            {
                fromStart = !fromStart;
                boundaryParameter = 1 - boundaryParameter;
            }
            // If flipDirection is true (as set by the user input), it indicates that the flag must be flipped
            // to ensure consistency with the overall direction logic.
            if (flipDirection) fromStart = !fromStart;
            // If the partialDirection matches the current 'fromStart' setting, we need to adjust the logic
            // to ensure that the flipDirection correctly represents the intended direction for the blend.
            if (partialDirection == fromStart) {
                fromStart = !fromStart;
                flipDirection = !flipDirection;
            }
            partialFilletCapBounds = append(partialFilletCapBounds, {
                boundaryCap: capEntity,
                isFlipped: flipDirection,
                useTrimmed: useTrimmedBound
            });

            // Depending on the 'fromStart' flag, determine which subset of edges should be blended.
            // This subset can either be from the first edge to the intersected edge, or from the intersected edge to the last edge.
            filterEntitiesPerBound = fromStart
                ? qUnion(subArray(path.edges, 0, edgeIndex))
                : qUnion(subArray(path.edges, edgeIndex, size(path.edges)));
        }
    }
    else
    {
        const pathLength = evPathLength(context, path);
        var parameter = partialType == EndTypePartialFillet.PERCENTAGE
            ? (isSecondBound ? definition.partialSecondEdgeTotalParameter : definition.partialFirstEdgeTotalParameter)
            : (isSecondBound ? (pathLength - definition.endPartialOffset) : definition.startPartialOffset) / pathLength;

        if (parameter > 1 || (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2502_PF_INVALID_OFFSET_FIX) && parameter < 0))
        {
            throw regenError(ErrorStringEnum.PARTIAL_FILLET_OFFSET_BOUNDARY_TOO_LARGE, isSecondBound ? ["endPartialOffset"] : ["startPartialOffset"]);
        }

        const lines = evPathTangentLines(context, path, [parameter]);
        const tangentLine = lines.tangentLines[0];
        edgeIndex = lines.edgeIndices[0];
        boundaryParameter = parameter;

        if (!isInFeaturePattern(context))
        {
            parameter = partialDirection ? parameter : 1 - parameter;
            const direction = partialDirection ? -tangentLine.direction : tangentLine.direction;
            const offset = pathLength * parameter;
            const position = tangentLine.origin + (direction * offset);

            const primaryParameterId = partialType == EndTypePartialFillet.OFFSET
                ? (isSecondBound ? "endPartialOffset" : "startPartialOffset")
                : (isSecondBound ? "partialSecondEdgeTotalParameter" : "partialFirstEdgeTotalParameter");
            const index = isSecondBound ? 1 : 0;

            try(addPartialFilletManipulators(context, topLevelId, position, direction, offset, pathLength, index, primaryParameterId));
        }

        const boundaryEdge = path.edges[edgeIndex];
        const qEdges = evaluateQuery(context, qEntityFilter(boundaryEdge, EntityType.EDGE));
        const boundaryParameter = evDistance(context, { "side0" : tangentLine.origin, "side1" : qEdges[0],
                    "arcLengthParameterization" : FS_PARTIAL_RADIUS_ARC_LENGTH_PARAMETERIZATION }).sides[1].parameter;

        partialFilletBounds = append(partialFilletBounds, { "boundaryEdge" : boundaryEdge, "boundaryParameter" : boundaryParameter, "isFlipped" : path.flipped[edgeIndex] != partialDirection });
    }

    if (isQueryEmpty(context, filterEntitiesPerBound))
    {
        filterEntitiesPerBound = qUnion(subArray(path.edges, edgeIndex, size(path.edges)));
        if (!partialDirection)
        {
            filterEntitiesPerBound = qSubtraction(qUnion(path.edges), filterEntitiesPerBound);
        }
    }

    return {
        "filterEntities": filterEntitiesPerBound,
        "partialFilletBounds": partialFilletBounds,
        "partialFilletCapBounds": partialFilletCapBounds,
        "facesToDelete": facesToDelete,
        "edgeIndex": edgeIndex,
        "boundaryParameter": boundaryParameter
    };
}

/*
 * Create linear tangtential manipulators for partial fillet
 */
function addPartialFilletManipulators(context is Context, topLevelId is Id, position is Vector, direction is Vector,
    offset is ValueWithUnits, maxValue is ValueWithUnits, index is number, primaryParameterId is string)
{
    addManipulators(context, topLevelId, {
        PARTIAL_POINT_ON_EDGE_MANIPULATOR ~ '.' ~ toString(index): linearManipulator({
            "base" : position,
            "direction" : -direction,
            "offset" : offset,
            "minValue" : 0 * meter,
            "maxValue" : maxValue,
            "primaryParameterId" : primaryParameterId
        })
    });
}

/**
 * @internal
 * Edit logic to check if a default value has been changed. So that we only display the info when needed
 * Additionally, updates the variable fillet default values to match the current global defaults.
 */
export function filletEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    definition.defaultsChanged = false;
    if (specifiedParameters.radius || specifiedParameters.rho || specifiedParameters.magnitude || specifiedParameters.otherRadius)
    {
        if (definition.radius != oldDefinition.radius ||
            definition.rho != oldDefinition.rho ||
            definition.magnitude != oldDefinition.magnitude ||
            definition.otherRadius != oldDefinition.otherRadius)
        {
            definition.defaultsChanged = true; //default values are being changed, server decides if they're being used anywhere
        }
    }

    if (definition.isVariable)
    {
        if (isFirstEntryAdded(oldDefinition, definition, "vertexSettings"))
        {
            definition = updateVariableFilletParameters(definition, VARIABLE_FILLET_VERTEX_MAP);
        }
        if (isFirstEntryAdded(oldDefinition, definition, "pointOnEdgeSettings"))
        {
            definition = updateVariableFilletParameters(definition, VARIABLE_FILLET_EDGE_MAP);
        }
    }

    return definition;
}

function updateVariableFilletParameters(definition is map, parametersMap is map) returns map
{
    const parameter = parametersMap["parameter"];
    // Unfortunately, it is not possible to just iterate through the map: some values (like flipAsymmetric) must be adjusted
    definition[parameter][0][parametersMap["radius"]] = definition.radius;
    if (definition.isAsymmetric)
    {
        definition[parameter][0][parametersMap["otherRadius"]] = definition.otherRadius;
        definition[parameter][0][parametersMap["flipAsymmetric"]] = !definition.flipAsymmetric;
    }
    if (definition.crossSection == FilletCrossSection.CONIC)
    {
        definition[parameter][0][parametersMap["rho"]] = definition.rho;
    }
    else if (definition.crossSection == FilletCrossSection.CURVATURE)
    {
        definition[parameter][0][parametersMap["magnitude"]] = definition.magnitude;
    }
    return definition;
}

function isFirstEntryAdded(oldDefinition is map, definition is map, arrayField is string) returns boolean
{
    return oldDefinition[arrayField] == [] && definition[arrayField] != [];
}

/*
 * Call sheetMetalCornerBreakAttributeBased on active sheet metal entities and opFillet on the remaining entities
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
        verify(definition.blendControlType != BlendControlType.WIDTH, ErrorStringEnum.SHEET_METAL_FILLET_OPTIONS_USE_CORNER_BREAK, {"faultyParameters" : ["blendControlType"]});
        verify(definition.crossSection == FilletCrossSection.CIRCULAR, ErrorStringEnum.SHEET_METAL_FILLET_OPTIONS_USE_CORNER_BREAK, {"faultyParameters" : ["crossSection"]});
        verify(!definition.isVariable, ErrorStringEnum.SHEET_METAL_FILLET_NO_CONIC, {"faultyParameters" : ["isVariable"]});
        verify(!definition.isAsymmetric, ErrorStringEnum.SHEET_METAL_FILLET_OPTIONS_USE_CORNER_BREAK, {"faultyParameters" : ["isAsymmetric"]});
        verify(!definition.isPartial, ErrorStringEnum.CANNOT_USE_PARTIAL_FILLET_IN_SHEET_METAL, {"faultyParameters" : ["isPartial"]});

        reportFeatureInfo(context, id, ErrorStringEnum.SHEET_METAL_USE_CORNER_BREAK_INFO);

        var cornerBreakDefinition = {
            "entities" : separatedQueries.sheetMetalQueries,
            "cornerBreakStyle" : SMCornerBreakStyle.FILLET,
            "range" : definition.radius
        };
        callSubfeatureAndProcessStatus(id, sheetMetalCornerBreakAttributeBased, context, id + "smFillet", cornerBreakDefinition);
    }

    if (hasNonSheetMetalQueries)
    {
        definition.entities = separatedQueries.nonSheetMetalQueries;
        opFillet(context, id, definition);
    }
}

const VARIABLE_POINT_ON_EDGE_MANIPULATOR = "variablePointOnEdgeManipulator";
const PARTIAL_POINT_ON_EDGE_MANIPULATOR = "partialPointOnEdgeManipulator";

/*
 * Create a linear manipulator for the fillet
 */
function addFilletManipulator(context is Context, id is Id, definition is map)
{
    // get last last edge (or arbitrary edge of the last face) from the qlv
    const operativeEntity = try(findManipulationEntity(context, definition));
    if (operativeEntity != undefined)
    {
        addFilletControlManipulator(context, id, definition, operativeEntity);
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
            addManipulators(context, id, { VARIABLE_POINT_ON_EDGE_MANIPULATOR ~ '.' ~ toString(ii) :
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
        const manipulatorId = getFilletControlManipulatorId(definition);
        if (newManipulators[manipulatorId] is map)
        {
            // convert given offset and edge topology into new radius
            const operativeEntity = findManipulationEntity(context, definition);
            return onFilletControlManipulatorChange(context, definition, newManipulators, operativeEntity, FILLET_WIDTH);
        }
        else // points on edges manipulators
        {
            for (var key, manipulator in newManipulators)
            {
                if (match(key, PARTIAL_POINT_ON_EDGE_MANIPULATOR ~ '.*').hasMatch)
                {
                    const index = stringToNumber(replace(key, PARTIAL_POINT_ON_EDGE_MANIPULATOR ~ '.', ""));
                    const edges = qEntityFilter(definition.entities, EntityType.EDGE);
                    try
                    {
                        const tangentEdges = definition.tangentPropagation ? qTangentConnectedEdges(edges) : edges;
                        const totalLength = evLength(context, { entities: tangentEdges });

                        var offset = manipulator.offset;
                        var totalParameter = clamp(offset / totalLength, PARTIAL_EDGE_INTERIOR_PARAMETER_BOUNDS[0], PARTIAL_EDGE_INTERIOR_PARAMETER_BOUNDS[2]);

                        if (!definition.partialOppositeParameter) {
                            totalParameter = 1 - totalParameter;
                            offset = totalLength - offset;
                        }

                        const isStartPartial = index == 0;
                        const isOffsetType = isStartPartial
                            ? definition.startPartialType == EndTypePartialFillet.OFFSET
                            : definition.endPartialType == EndTypePartialFillet.OFFSET;

                        if (isOffsetType) {
                            offset = clamp(offset.value, VR_BLEND_BOUNDS[meter][0], totalLength / meter) * meter;
                            if (isStartPartial) {
                                definition.startPartialOffset = offset;
                            } else {
                                definition.endPartialOffset = offset;
                            }
                        } else {
                            const parameter = isStartPartial ? totalParameter : 1 - totalParameter;
                            const parameterKey = isStartPartial
                                ? 'partialFirstEdgeTotalParameter'
                                : 'partialSecondEdgeTotalParameter';

                            definition[parameterKey] = roundToPrecision(parameter, PARAMETER_PRECISION);
                        }
                    }
                }
                else
                {
                    if (abs(manipulator.offset) < TOLERANCE.zeroLength * meter)
                        continue;

                    var settingIndex = stringToNumber(replace(key, VARIABLE_POINT_ON_EDGE_MANIPULATOR ~ '.', ""));
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
    }

    return definition;
}

/*
 * Start with the final element in the qlv.
 * If it is an edge, return it.
 * If it is a face and it has edges, return one arbitrarily.
 * Continue through the list in reverse order until an edge can be found.
 */
function findManipulationEntity(context is Context, definition is map) returns Query
{
    const resolvedEntities = evaluateQuery(context, definition.entities);
    const nResolved = size(resolvedEntities);

    for (var i = nResolved - 1; i >= 0; i -= 1)
    {
        const entity = resolvedEntities[i];

        if (!isQueryEmpty(context, qEntityFilter(entity, EntityType.FACE)))
        {
            const edges = evaluateQuery(context, qAdjacent(entity, AdjacencyType.EDGE, EntityType.EDGE));

            if (edges != [])
            {
                return edges[0];
            }
        }
        else
        {
            return entity;
        }
    }

    throw {};
}

