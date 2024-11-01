FeatureScript 2506; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

export import(path : "onshape/std/extrudeCommon.fs", version : "2506.0");
export import(path : "onshape/std/query.fs", version : "2506.0");

import(path : "onshape/std/attributes.fs", version : "2506.0");
import(path : "onshape/std/box.fs", version : "2506.0");
import(path : "onshape/std/containers.fs", version : "2506.0");
import(path : "onshape/std/coordSystem.fs", version : "2506.0");
import(path : "onshape/std/curveGeometry.fs", version : "2506.0");
import(path : "onshape/std/error.fs", version : "2506.0");
import(path : "onshape/std/evaluate.fs", version : "2506.0");
import(path : "onshape/std/feature.fs", version : "2506.0");
import(path : "onshape/std/geomOperations.fs", version : "2506.0");
import(path : "onshape/std/manipulator.fs", version : "2506.0");
import(path : "onshape/std/math.fs", version : "2506.0");
import(path : "onshape/std/modifyFillet.fs", version : "2506.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2506.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2506.0");
import(path : "onshape/std/sketch.fs", version : "2506.0");
import(path : "onshape/std/smreliefstyle.gen.fs", version : "2506.0");
import(path : "onshape/std/string.fs", version : "2506.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2506.0");
import(path : "onshape/std/tool.fs", version : "2506.0");
import(path : "onshape/std/topologyUtils.fs", version : "2506.0");
import(path : "onshape/std/valueBounds.fs", version : "2506.0");
import(path : "onshape/std/vector.fs", version : "2506.0");

/**
 * Method of initializing sheet metal model
 */
export enum SMProcessType
{
    annotation { "Name" : "Convert" }
    CONVERT,
    annotation { "Name" : "Extrude" }
    EXTRUDE,
    annotation { "Name" : "Thicken" }
    THICKEN
}

/**
 * Default corner relief style setting
 */
export enum SMCornerStrategyType
{
    annotation { "Name" : "Square - Sized" }
    SIZED_RECTANGLE,
    annotation { "Name" : "Rectangle - Scaled" }
    RECTANGLE,
    annotation { "Name" : "Round - Sized" }
    SIZED_ROUND,
    annotation { "Name" : "Round - Scaled" }
    ROUND,
    annotation { "Name" : "Closed" }
    CLOSED,
    annotation { "Name" : "Simple" }
    SIMPLE
}

/**
 * Default bend relief style setting
 */
export enum SMBendStrategyType
{
    annotation { "Name" : "Rectangle - Scaled" }
    RECTANGLE,
    annotation { "Name" : "Obround - Scaled" }
    OBROUND,
    annotation { "Name" : "Tear" }
    TEAR
}

/**
 * Bend calculation setting
 */
export enum SMBendCalculationType
{
    annotation { "Name" : "K Factor" }
    K_FACTOR,
    annotation { "Name" : "Bend allowance" }
    BEND_ALLOWANCE,
    annotation { "Name" : "Bend deduction" }
    BEND_DEDUCTION
}

/**
 * Corner relief scale bounds
 */
export const CORNER_RELIEF_SCALE_BOUNDS =
{
    (unitless) : [1.0, 1.5, 2.0]
} as RealBoundSpec;

/**
 * Bend relief depth scale bounds
 */
export const BEND_RELIEF_DEPTH_SCALE_BOUNDS =
{
    (unitless) : [1.0, 2.0, 5.0]
} as RealBoundSpec;

/**
 * Bend relief width scale bounds
 */
export const BEND_RELIEF_WIDTH_SCALE_BOUNDS =
{
    (unitless) : [0.0625, 1.0625, 2.0]
} as RealBoundSpec;

/**
 * Manipulator name for the "flip direction up" manipulator
 */
export const FLIP_DIRECTION_UP_MANIPULATOR_NAME = "flipDirectionUpManipulator";

/**
 * Create and activate a sheet metal model by converting existing parts, extruding sketch curves or thickening.
 * All operations on an active sheet metal model will automatically be represented in the flat pattern and the table.
 * Sheet metal models may consist of multiple parts. Multiple sheet metal models can be active.
 */
annotation { "Feature Type Name" : "Sheet metal model",
             "Manipulator Change Function" : "sheetMetalStartManipulatorChange",
             "Filter Selector" : "allparts",
             "Editing Logic Function" : "sheetMetalStartEditLogic" }
export const sheetMetalStart = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
     precondition
    {
        annotation { "Name" : "Entities", "UIHint" : UIHint.ALWAYS_HIDDEN, "Filter" : (EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO) ||
            ((EntityType.FACE || EntityType.EDGE) && SketchObject.YES && ConstructionObject.NO) }
        definition.initEntities is Query;

        annotation { "Name" : "Process", "UIHint" : UIHint.HORIZONTAL_ENUM }
        definition.process is SMProcessType;

        // First the entities
        annotation { "Group Name" : "Selections", "Collapsed By Default" : false}
        {
            if (definition.process == SMProcessType.CONVERT)
            {
                annotation { "Name" : "Parts and surfaces to convert",
                            "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO && ConstructionObject.NO && AllowMeshGeometry.YES }
                definition.partToConvert is Query;

                annotation { "Name" : "Faces to exclude", "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO && AllowMeshGeometry.YES }
                definition.facesToExclude is Query;
            }
            else if (definition.process == SMProcessType.EXTRUDE)
            {
                annotation { "Name" : "Sketch curves to extrude",
                            "Filter" : SketchObject.YES && ConstructionObject.NO && ModifiableEntityOnly.YES && EntityType.EDGE }
                definition.sketchCurves is Query;

                annotation { "Name" : "Arcs to extrude as bends",
                            "Filter" : SketchObject.YES && ConstructionObject.NO && ModifiableEntityOnly.YES && (EntityType.EDGE && GeometryType.ARC) }
                definition.bendArcs is Query;

                annotation { "Name" : "End type" }
                definition.endBound is SMExtrudeBoundingType;

                annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                definition.oppositeExtrudeDirection is boolean;

                extrudeBoundParametersPredicate(definition);

                if (definition.endBound == SMExtrudeBoundingType.BLIND)
                {
                    annotation { "Name" : "Symmetric" }
                    definition.symmetric is boolean;
                }

                if (!isSymmetricExtrude(definition))
                {
                    annotation { "Name" : "Second end position" }
                    definition.hasSecondDirection is boolean;

                    if (definition.hasSecondDirection)
                    {
                        annotation { "Name" : "End type", "Column Name" : "Second end type" }
                        definition.secondDirectionBound is SMExtrudeBoundingType;

                        annotation { "Name" : "Opposite direction", "Column Name" : "Second opposite direction",
                                     "UIHint" : UIHint.OPPOSITE_DIRECTION, "Default" : true }
                        definition.secondDirectionOppositeExtrudeDirection is boolean;

                        extrudeSecondDirectionBoundParametersPredicate(definition);
                    }
                }
            }
            else if (definition.process == SMProcessType.THICKEN)
            {
                annotation { "Name" : "Faces or sketch regions to thicken",
                            "Filter" : ConstructionObject.NO && (GeometryType.PLANE || GeometryType.CYLINDER || GeometryType.EXTRUDED) }
                definition.regions is Query;

                annotation { "Name" : "Tangent propagation", "Default" : false }
                definition.tangentPropagation is boolean;
            }

            if (definition.process == SMProcessType.THICKEN || definition.process == SMProcessType.CONVERT)
            {
                annotation { "Name" : "Edges or cylinders to bend",
                             "Filter" : ((EntityType.EDGE && EdgeTopology.TWO_SIDED && GeometryType.LINE) ||
                                         (EntityType.FACE && GeometryType.CYLINDER)) && SketchObject.NO }
                definition.bends is Query;

                annotation { "Name" : "Clearance from input" }
                isLength(definition.clearance, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

                annotation { "Name" : "Include bends", "Description" : "Check to include the clearance for bends" }
                definition.bendsIncluded is boolean;
            }

            if (definition.process == SMProcessType.CONVERT)
            {
                annotation { "Name" : "Keep input parts" }
                definition.keepInputParts is boolean;
            }
        }

        // Then some common parameters
        annotation { "Group Name" : "General", "Collapsed By Default" : false}
        {
            annotation { "Name" : "Thickness", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            isLength(definition.thickness, SM_THICKNESS_BOUNDS);

            annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.oppositeDirection is boolean;

            annotation { "Name" : "Bend radius", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            isLength(definition.radius, SM_BEND_RADIUS_BOUNDS);

            annotation { "Name" : "Flip direction up", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            definition.flipDirectionUp is boolean;
        }

        annotation { "Group Name" : "Material", "Collapsed By Default" : true}
        {
            annotation { "Name" : "Bend calculation",
                         "Default" : SMBendCalculationType.K_FACTOR,
                         "UIHint" : ["SHOW_LABEL", "REMEMBER_PREVIOUS_VALUE"] }
            definition.bendCalculationType is SMBendCalculationType;

            annotation { "Name" : "Default bend K Factor", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            isReal(definition.kFactor, K_FACTOR_BOUNDS);

            annotation { "Name" : "Rolled K Factor", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            isReal(definition.kFactorRolled, ROLLED_K_FACTOR_BOUNDS);
        }

        annotation { "Group Name" : "Relief", "Collapsed By Default" : true}
        {
            annotation { "Name" : "Minimal gap", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            isLength(definition.minimalClearance, SM_MINIMAL_CLEARANCE_BOUNDS);

            annotation { "Name" : "Corner relief type",
                         "Default" : SMCornerStrategyType.SIMPLE,
                         "UIHint" : ["SHOW_LABEL", "REMEMBER_PREVIOUS_VALUE"] }
            definition.defaultCornerStyle is SMCornerStrategyType;

            if (definition.defaultCornerStyle == SMCornerStrategyType.RECTANGLE ||
                definition.defaultCornerStyle == SMCornerStrategyType.ROUND)
            {
                annotation { "Name" : "Corner relief scale", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isReal(definition.defaultCornerReliefScale, CORNER_RELIEF_SCALE_BOUNDS);
            }

            if (definition.defaultCornerStyle == SMCornerStrategyType.SIZED_ROUND)
            {
                annotation { "Name" : "Corner relief diameter", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition.defaultRoundReliefDiameter, SM_RELIEF_SIZE_BOUNDS);
            }

            if (definition.defaultCornerStyle == SMCornerStrategyType.SIZED_RECTANGLE)
            {
                annotation { "Name" : "Corner relief width", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition.defaultSquareReliefWidth, SM_RELIEF_SIZE_BOUNDS);
            }

            annotation { "Name" : "Bend relief type",
                         "Default" : SMBendStrategyType.OBROUND,
                         "UIHint" : ["SHOW_LABEL", "REMEMBER_PREVIOUS_VALUE"] }
            definition.defaultBendReliefStyle is SMBendStrategyType;

            if (definition.defaultBendReliefStyle == SMBendStrategyType.OBROUND ||
                definition.defaultBendReliefStyle == SMBendStrategyType.RECTANGLE)
            {
                annotation { "Name" : "Bend relief depth scale", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isReal(definition.defaultBendReliefDepthScale, BEND_RELIEF_DEPTH_SCALE_BOUNDS);
                annotation { "Name" : "Bend relief width scale", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isReal(definition.defaultBendReliefScale, BEND_RELIEF_WIDTH_SCALE_BOUNDS);
            }
        }
    }
    {
        verifyNoMeshSheetMetalStart(context, definition);

        var resultSheetBodies = undefined;
        definition.supportRolled = isAtVersionOrLater(context, FeatureScriptVersionNumber.V727_SM_SUPPORT_ROLLED);
        // tangentPropagation is meaningful only for Thicken option
        definition.tangentPropagation = (definition.tangentPropagation && definition.process == SMProcessType.THICKEN);
        if (definition.process == SMProcessType.CONVERT)
        {
            checkNotInFeaturePattern(context, definition.partToConvert, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);
            resultSheetBodies = convertExistingPart(context, id, definition);
        }
        else if (definition.process == SMProcessType.EXTRUDE)
        {
            checkNotInFeaturePattern(context, definition.sketchCurves, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);
            resultSheetBodies = extrudeSheetMetal(context, id, definition);
        }
        else if (definition.process == SMProcessType.THICKEN)
        {
            checkNotInFeaturePattern(context, definition.regions, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);
            resultSheetBodies = thickenToSheetMetal(context, id, definition);
        }

        addFlipDirectionUpManipulator(resultSheetBodies, FLIP_DIRECTION_UP_MANIPULATOR_NAME, id, context, definition);

    }, { "kFactor" : 0.45,
      "kFactorRolled" : 0.5,
      "minimalClearance" : 2e-5 * meter,
      "oppositeDirection" : false,
      "initEntities" : qNothing(),
      "bendArcs" : qNothing(),
      "defaultCornerStyle" :  SMCornerStrategyType.SIMPLE,
      "defaultCornerReliefScale" : 1.5,
      "defaultRoundReliefDiameter" : 0 * meter,
      "defaultSquareReliefWidth" : 0 * meter,
      "defaultBendReliefStyle" :  SMBendStrategyType.OBROUND,
      "defaultBendReliefDepthScale" : 1.5,
      "defaultBendReliefScale" : 1.0625,
      "bendsIncluded" : false,
      "clearance" : 0 * meter,
      "keepInputParts" : false,
      "tangentPropagation" : false,
      "hasSecondDirection" : false, // option for extrude second direction
      "oppositeExtrudeDirection" : false,
      "secondDirectionOppositeExtrudeDirection" : false,
      "hasOffset" : false,
      "hasSecondDirectionOffset" : false,
      "offsetOppositeDirection" : false,
      "secondDirectionOffsetOppositeDirection" : false,
      "symmetric" : false,
      "flipDirectionUp" : false,
      "bendCalculationType" : SMBendCalculationType.K_FACTOR
    });

function verifyNoMeshSheetMetalStart(context is Context, definition is map)
{
    if (definition.process == SMProcessType.CONVERT)
    {
        // A model that contains mesh faces can still be converted if those mesh faces are excluded.
        verifyNoMesh(context, { "partToConvert" : qSubtraction(qOwnedByBody(definition.partToConvert, EntityType.FACE), definition.facesToExclude)}, "partToConvert");
    }
    else if (definition.process == SMProcessType.EXTRUDE)
    {
        verifyNoMesh(context, definition, "sketchCurves");
        verifyNoMesh(context, definition, "bendArcs");
    }
    else if (definition.process == SMProcessType.THICKEN)
    {
        verifyNoMesh(context, definition, "regions");
    }
    if (definition.process == SMProcessType.THICKEN || definition.process == SMProcessType.CONVERT)
    {
        verifyNoMesh(context, definition, "bends");
    }
}

function finalizeSheetMetalGeometry(context is Context, id is Id, entities is Query)
{
    try
    {
        updateSheetMetalGeometry(context, id, { "entities" : entities });
    }
    catch (e)
    {
        var messageAsEnum = try silent(e.message as ErrorStringEnum);
        if (messageAsEnum == ErrorStringEnum.BOOLEAN_INVALID)
        {
            // I can't think of anything more useful to tell the user right now. Analyzing such cases
            // may make it clearer when it can happen
            throw regenError(ErrorStringEnum.SHEET_METAL_REBUILD_ERROR);
        }
        else if (messageAsEnum == ErrorStringEnum.BAD_GEOMETRY ||
                messageAsEnum == ErrorStringEnum.THICKEN_FAILED)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_CANNOT_THICKEN);
        }
        else if (messageAsEnum == ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);
        }
        else
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_REBUILD_ERROR);
        }
    }
}

function addFlipDirectionUpManipulator(sheetBodies is Query, manipulatorName is string, id is Id, context is Context, definition is map) {
    if (!isQueryEmpty(context, qOwnedByBody(sheetBodies, EntityType.FACE)))
    {
        var tangentPlane = evFaceTangentPlane(context, {
                "face" : qNthElement(qOwnedByBody(sheetBodies, EntityType.FACE), 0),
                "parameter" : vector(0.5, 0.5)
        });
        var manipulator is Manipulator = flipManipulator({
            "base" : tangentPlane.origin,
            "direction" : -tangentPlane.normal,
            "flipped" : definition.flipDirectionUp
        });
        addManipulators(context, id, {
            (manipulatorName) : manipulator
        });
    }
}

/*
 * Methods for CONVERT
 */

function convertExistingPart(context is Context, id is Id, definition is map)
{
    verifyNonemptyQuery(context, definition, "partToConvert", ErrorStringEnum.CANNOT_RESOLVE_ENTITIES);

    var associationAttributes = getSMAssociationAttributes(context, definition.partToConvert);
    if (associationAttributes != [])
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_INPUT_BODY_SHOULD_NOT_BE_SHEET_METAL, ["partToConvert"]);
    }

    var facesOnUnknownBodies = evaluateQuery(context, qSubtraction(qOwnerBody(definition.facesToExclude), definition.partToConvert));
    if (facesOnUnknownBodies != [])
    {
        reportFeatureWarning(context, id, ErrorStringEnum.FACES_NOT_OWNED_BY_PARTS);
    }
    var edgesOnUnknownBodies = evaluateQuery(context, qSubtraction(qOwnerBody(definition.bends), definition.partToConvert));
    if (edgesOnUnknownBodies != [])
    {
        reportFeatureWarning(context, id, ErrorStringEnum.EDGES_NOT_OWNED_BY_PARTS);
    }

    var complimentFacesQ = qSubtraction(qOwnedByBody(definition.partToConvert, EntityType.FACE), definition.facesToExclude);

    var nFacesToExclude = size(evaluateQuery(context, definition.facesToExclude));
    var nComplimentFaces = size(evaluateQuery(context, complimentFacesQ));
    var nBends = size(evaluateQuery(context, definition.bends));

    if (definition.supportRolled == true)
    {
        throwOnUnsupportedFaces(context, complimentFacesQ, ["partToConvert", "facesToExclude"]);
    }
    else
    {
        // Let's be careful to screen out unwanted faces here, i.e. anything that isn't planar
        var planarFaces = qGeometry(complimentFacesQ, GeometryType.PLANE);
        var badFaces = qSubtraction(complimentFacesQ, planarFaces);
        if (!isQueryEmpty(context, badFaces))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_CONVERT_PLANE, ["partToConvert", "facesToExclude"], badFaces);
        }
    }

    var bendsQ = convertFaces(context, id, definition, complimentFacesQ, true);
    definition.remindToSelectBends = (nBends == 0 && nFacesToExclude > 0 && nComplimentFaces > 1);
    annotateConvertedFaces(context, id, definition, bendsQ);

    return qCreatedBy(id, EntityType.BODY);
}

function convertFaces(context is Context, id is Id, definition, faces is Query, trimWithFacesAround is boolean) returns Query
{
    var surfaceId = id + "extractSurface";
    var bendsQ = startTracking(context, { "subquery" : definition.bends });
    var offset = computeSurfaceOffset(context, definition);

    try
    {
        opExtractSurface(context, surfaceId, {
                    "faces" : faces,
                    "offset" : offset,
                    "useFacesAroundToTrimOffset" : trimWithFacesAround,
                    "tangentPropagation" : definition.tangentPropagation });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_CANNOT_THICKEN, ["partToConvert", "facesToExclude", "regions"]);
    }

    return bendsQ;
}

function annotateConvertedFaces(context is Context, id is Id, definition, bendsQuery is Query)
{
    try
    {
        var thicknessDirection = getThicknessDirection(context, definition);
        annotateSmSurfaceBodies(context, id, {
                    "surfaceBodies" : qCreatedBy(id, EntityType.BODY),
                    "bendEdgesAndFaces" : bendsQuery,
                    "specialRadiiBends" : [],
                    "defaultRadius" : definition.radius,
                    "controlsThickness" : true,
                    "thickness" : definition.thickness,
                    "thicknessDirection" : thicknessDirection,
                    "minimalClearance" : definition.minimalClearance,
                    "kFactor" : definition.kFactor,
                    "kFactorRolled" : definition.kFactorRolled,
                    "flipDirectionUp" : definition.flipDirectionUp,
                    "defaultTwoCornerStyle" : getDefaultTwoCornerStyle(definition),
                    "defaultThreeCornerStyle" : getDefaultThreeCornerStyle(context, definition),
                    "defaultBendReliefStyle" : getDefaultBendReliefStyle(definition),
                    "defaultCornerReliefScale" : definition.defaultCornerReliefScale,
                    "defaultRoundReliefDiameter" : definition.defaultRoundReliefDiameter,
                    "defaultSquareReliefWidth" : definition.defaultSquareReliefWidth,
                    "defaultBendReliefDepthScale" : definition.defaultBendReliefDepthScale,
                    "defaultBendReliefScale" : definition.defaultBendReliefScale,
                    "bendCalculationType" : definition.bendCalculationType}, 0);
        if (getFeatureError(context, id) != undefined)
        {
            return;
        }
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_REBUILD_ERROR);
    }

    if (!definition.keepInputParts)
    {
        try
        {
            opDeleteBodies(context, id + "deleteBodies", {
                        "entities" : definition.partToConvert
                    });
        }
        catch
        {
            throw regenError(ErrorStringEnum.REGEN_ERROR);
        }
    }

    finalizeSheetMetalGeometry(context, id, qUnion([qCreatedBy(id, EntityType.FACE), qCreatedBy(id, EntityType.EDGE)]));
    if (definition.remindToSelectBends)
    {
        if (!featureHasNonTrivialStatus(context, id))
        {
            reportFeatureInfo(context, id, ErrorStringEnum.SHEET_METAL_START_SELECT_BENDS, ["bends"]);
        }
    }
}

function computeSurfaceOffset(context is Context, definition is map) returns ValueWithUnits
{
    var wallClearance = definition.clearance;
    if (definition.bendsIncluded)
    {
        var edges = evaluateQuery(context, qEntityFilter(definition.bends, EntityType.EDGE));
        if (size(edges) > 0)
        {
            for (var edge in edges)
            {
                var adjacentWalls = qSubtraction(qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE), definition.facesToExclude);
                if (isQueryEmpty(context, adjacentWalls))
                {
                    continue;
                }
                var convexity = evEdgeConvexity(context, { "edge" : edge });
                if (definition.oppositeDirection)
                {
                    if (convexity != EdgeConvexityType.CONCAVE)
                    {
                        continue;
                    }
                }
                else
                {
                    if (convexity != EdgeConvexityType.CONVEX)
                    {
                        continue;
                    }
                }
                var eAngle = edgeAngle(context, edge);
                var cHalfAngle = cos(eAngle * 0.5);
                var clearance = definition.radius * (1 - cHalfAngle) + definition.clearance * cHalfAngle;
                if (clearance > wallClearance)
                {
                    wallClearance = clearance;
                }
            }
        }
    }
    var offset = wallClearance;
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V629_SM_MODEL_FRONT_N_BACK))
    {
       offset += 0.5 * definition.thickness;
    }
    if (definition.oppositeDirection)
    {
        offset = -offset;
    }
    return offset;
}

/*
 * Methods for EXTRUDE
 */

function extrudeSheetMetal(context is Context, id is Id, definition is map)
{
    definition.trackingBendArcs = (definition.supportRolled == true) ? startTracking(context, definition.bendArcs) : qNothing();
    const sheetQuery = extrudeSketchCurves(context, id, definition);
    const createdSheetBodies = evaluateQuery(context, sheetQuery);

    if (createdSheetBodies == [])
    {
        throw regenError(ErrorStringEnum.EXTRUDE_SURF_NO_CURVE, ["sketchCurves"]);
    }

    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V629_SM_MODEL_FRONT_N_BACK))
    {
        // Regardless of whether the sheets were created by curves or regions
        // we want to offset the sheet by half the thickness
        var oppositeOffset = definition.oppositeDirection;
        if (definition.oppositeExtrudeDirection == true)
        {
            oppositeOffset = !oppositeOffset;
        }
       offsetSheets(context, id, sheetQuery, definition.thickness, oppositeOffset);
    }
    const facesAndEdges = addSheetMetalDataToSheet(context, id, sheetQuery, definition);
    finalizeSheetMetalGeometry(context, id, facesAndEdges);

    return sheetQuery;
}

function offsetSheets(context is Context, id is Id, sheetQuery is Query, thickness is ValueWithUnits, oppositeDirection is boolean)
{
    try
    {
        opOffsetFace(context, id + "offsetFaces", {
                    "moveFaces" : qOwnedByBody(sheetQuery, EntityType.FACE),
                    "offsetDistance" : thickness * 0.5 * (oppositeDirection ? -1 : 1)
                });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_TOO_THICK, ["thickness"]);
    }
}

function thickenToSheetMetal(context is Context, id is Id, definition is map)
{
    const evaluatedFaceQueries = evaluateQuery(context, definition.regions);
    const faceQueryCount = size(evaluatedFaceQueries);
    if (faceQueryCount == 0)
    {
        throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["regions"]);
    }

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V534_INFORM_IN_CONTEXT_SM_THICKEN))
    {
        if (faceQueryCount > 1)
        {
          const modifiable = size(evaluateQuery(context, qModifiableEntityFilter(definition.regions)));
          if (modifiable != faceQueryCount)
          {
            reportFeatureInfo(context, id, ErrorStringEnum.SHEET_METAL_THICKEN_IN_CONTEXT_INFO, ["regions"]);
          }
        }
    }

    var sketchPlaneToFacesMap = {};
    var facesToConvert = [];
    var index = 0;
    for (var evaluatedFace in evaluatedFaceQueries)
    {
        var key = try silent(evOwnerSketchPlane(context, { "entity" : evaluatedFace }));
        if (key == undefined)
        {
            facesToConvert = append(facesToConvert, evaluatedFace);
        }
        else
        {
            if (sketchPlaneToFacesMap[key] == undefined)
            {
                sketchPlaneToFacesMap[key] = [evaluatedFace];
            }
            else
            {
                sketchPlaneToFacesMap[key] = append(sketchPlaneToFacesMap[key], evaluatedFace);
            }
        }
    }

    index = 0;
    for (var entry in sketchPlaneToFacesMap)
    {
        var faceQueryArray = entry.value;

        definition.regions = qUnion(faceQueryArray);
        convertRegion(context, id + unstableIdComponent(index), definition);
        index += 1;
    }

    if (definition.supportRolled == true)
    {
        throwOnUnsupportedFaces(context, qUnion(facesToConvert), ["regions"]);
    }
    var bendsQ = qNothing();
    var nFaces = size(facesToConvert);
    var nBends = size(evaluateQuery(context, definition.bends));
    if (nFaces != 0)
    {
        var useFacesAround = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V525_SM_THICKEN_NO_NEIGHBORS);
        facesToConvert = append(facesToConvert, qEntityFilter(definition.bends, EntityType.FACE));
        bendsQ = convertFaces(context, id, definition, qUnion(facesToConvert), useFacesAround);
    }
    definition.keepInputParts = true;
    definition.remindToSelectBends = (nFaces > 1 && nBends == 0);
    annotateConvertedFaces(context, id, definition, bendsQ);

    return qCreatedBy(id, EntityType.BODY);
}

function convertRegion(context is Context, id is Id, definition is map)
{
    const extrudeId = id + "extrude";
    const sign = definition.oppositeDirection ? -1 : 1;
    var startDepth = definition.clearance;
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V629_SM_MODEL_FRONT_N_BACK))
    {
        if (!definition.oppositeDirection)
            startDepth += definition.thickness;
    }
    else
    {
        startDepth += 0.5 * definition.thickness;
    }

    try
    {
        opExtrude(context, extrudeId, {
                    "entities" : definition.regions,
                    "direction" : sign * evPlane(context, { "face" : definition.regions }).normal,
                    "endBound" : BoundingType.BLIND,
                    "endDepth" : startDepth + definition.thickness,
                    "startBound" : BoundingType.BLIND,
                    "startDepth" : -startDepth
                });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_CANNOT_THICKEN, ["regions"]);
    }
    try
    {
        var createdQuery = qCreatedBy(extrudeId, EntityType.BODY);
        opExtractSurface(context, id + "extract", { "faces" : qCapEntity(extrudeId, CapType.START, EntityType.FACE) });
        opDeleteBodies(context, id + "deleteBodies", {
                    "entities" : createdQuery
                });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_REBUILD_ERROR);
    }
}

function getSketchCurvesToExtrude(definition is map)
{
    var sketchCurves = (definition.supportRolled == true) ?
            qUnion([definition.bendArcs, definition.sketchCurves]) :
            qGeometry(definition.sketchCurves, GeometryType.LINE);
    return qConstructionFilter(sketchCurves, ConstructionObject.NO);
}

function extrudeSketchCurves(context is Context, id is Id, definition is map) returns Query
{
    const sketchCurves = getSketchCurvesToExtrude(definition);
    const resolvedEntities = evaluateQuery(context, sketchCurves);
    if (size(resolvedEntities) > 0)
    {
        // Handle negative inputs
        definition = adjustExtrudeDirectionForBlind(definition);

        const extrudeAxis = getExtrudeDirection(context, resolvedEntities[0]);
        addExtrudeManipulator(context, id, definition, sketchCurves, extrudeAxis, false);

        definition = transformExtrudeDefinitionForOpExtrude(context, id, sketchCurves, extrudeAxis.direction, definition);

        const extrudeId = id + "extrude";
        callSubfeatureAndProcessStatus(id, opExtrude, context, extrudeId, definition, {
                    "propagateErrorDisplay" : true,
                    "featureParameterMap" : { "entities" : "sketchCurves" }
                });
        cleanupTemporaryBoundaryPlanes(context, id, definition);

        return qCreatedBy(extrudeId, EntityType.BODY);
    }
    return qNothing();
}

function getExtrudeDirection(context is Context, entity is Query)
{
    const tangentAtEdge = evEdgeTangentLine(context, { "edge" : entity, "parameter" : 0.5 });
    const entityPlane = evOwnerSketchPlane(context, { "entity" : entity });
    var direction = entityPlane.normal;
    return line(tangentAtEdge.origin, direction);
}

function extrudeUpToBoundaryFlip(context is Context, definition is map) returns map
{
    const sketchCurves = getSketchCurvesToExtrude(definition);
    const resolvedEntities = evaluateQuery(context, sketchCurves);
    if (size(resolvedEntities) == 0)
    {
        return definition;
    }
    const extrudeAxis = try(getExtrudeDirection(context, resolvedEntities[0]));
    if (extrudeAxis == undefined)
    {
        return definition;
    }
    return extrudeUpToBoundaryFlipCommon(context, extrudeAxis, definition);
}

function addSheetMetalDataToSheet(context is Context, id is Id, surfaceBodies is Query, definition is map) returns Query
{
    var sharpEdges = [];
    const twoSidedEdges = qOwnedByBody(surfaceBodies, EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.TWO_SIDED);
    for (var edge in evaluateQuery(context, twoSidedEdges))
    {
        var convexity = evEdgeConvexity(context, { "edge" : edge });
        if (convexity == EdgeConvexityType.CONVEX || convexity == EdgeConvexityType.CONCAVE)
        {
            sharpEdges = append(sharpEdges, edge);
        }
    }

    var thicknessDirection = getThicknessDirection(context, definition);
    var surfaceData =
    {
        "defaultRadius" : definition.radius,
        "surfaceBodies" : surfaceBodies,
        "bendEdgesAndFaces" : qUnion([qUnion(sharpEdges), definition.trackingBendArcs]),
        "specialRadiiBends" : [],
        "thickness" : definition.thickness,
        "thicknessDirection" : thicknessDirection,
        "controlsThickness" : true,
        "minimalClearance" : definition.minimalClearance,
        "kFactor" : definition.kFactor,
        "kFactorRolled" : definition.kFactorRolled,
        "flipDirectionUp" : definition.flipDirectionUp,
        "defaultTwoCornerStyle" : getDefaultTwoCornerStyle(definition),
        "defaultThreeCornerStyle" : getDefaultThreeCornerStyle(context, definition),
        "defaultBendReliefStyle" : getDefaultBendReliefStyle(definition),
        "defaultCornerReliefScale" : definition.defaultCornerReliefScale,
        "defaultRoundReliefDiameter" : definition.defaultRoundReliefDiameter,
        "defaultSquareReliefWidth" : definition.defaultSquareReliefWidth,
        "defaultBendReliefDepthScale" : definition.defaultBendReliefDepthScale,
        "defaultBendReliefScale" : definition.defaultBendReliefScale,
        "bendCalculationType" : definition.bendCalculationType
    };

    try
    {
        annotateSmSurfaceBodies(context, id, surfaceData, 0);
        if (getFeatureError(context, id) != undefined)
        {
            return qNothing();
        }
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_CANNOT_THICKEN);
    }

    return qUnion([qOwnedByBody(surfaceData.surfaceBodies, EntityType.FACE), qUnion(sharpEdges)]);
}

function getDefaultTwoCornerStyle(definition is map) returns SMReliefStyle
{
    if (definition.defaultCornerStyle == SMCornerStrategyType.RECTANGLE)
    {
        return SMReliefStyle.RECTANGLE;
    }
    else if (definition.defaultCornerStyle == SMCornerStrategyType.ROUND)
    {
        return SMReliefStyle.ROUND;
    }
    else if (definition.defaultCornerStyle == SMCornerStrategyType.SIZED_RECTANGLE)
    {
        return SMReliefStyle.SIZED_RECTANGLE;
    }
    else if (definition.defaultCornerStyle == SMCornerStrategyType.SIZED_ROUND)
    {
        return SMReliefStyle.SIZED_ROUND;
    }
    else if (definition.defaultCornerStyle == SMCornerStrategyType.CLOSED)
    {
        return SMReliefStyle.CLOSED;
    }
    else if (definition.defaultCornerStyle == SMCornerStrategyType.SIMPLE)
    {
        return SMReliefStyle.SIMPLE;
    }
    else
    {
        return SMReliefStyle.RECTANGLE;
    }
}

function getDefaultThreeCornerStyle(context is Context, definition is map) returns SMReliefStyle
{
    const includeSized = isAtVersionOrLater(context, FeatureScriptVersionNumber.V781_THREE_BEND_SIZED);
    const fallbackToSimple = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1457_SM_CLOSED_RELIEF_FALLBACK);

    if (definition.defaultCornerStyle == SMCornerStrategyType.RECTANGLE)
    {
        return SMReliefStyle.RECTANGLE;
    }
    else if (definition.defaultCornerStyle == SMCornerStrategyType.ROUND)
    {
        return SMReliefStyle.ROUND;
    }
    else if (includeSized && definition.defaultCornerStyle == SMCornerStrategyType.SIZED_RECTANGLE)
    {
        return SMReliefStyle.SIZED_RECTANGLE;
    }
    else if (includeSized && definition.defaultCornerStyle == SMCornerStrategyType.SIZED_ROUND)
    {
        return SMReliefStyle.SIZED_ROUND;
    }
    else if (definition.defaultCornerStyle == SMCornerStrategyType.SIMPLE)
    {
        return SMReliefStyle.SIMPLE;
    }
    else if (fallbackToSimple)
    {
        return SMReliefStyle.SIMPLE;
    }
    else
    {
        return SMReliefStyle.RECTANGLE;
    }
}

function getDefaultBendReliefStyle(definition is map) returns SMReliefStyle
{
    if (definition.defaultBendReliefStyle == SMBendStrategyType.RECTANGLE)
    {
        return SMReliefStyle.RECTANGLE;
    }
    else if (definition.defaultBendReliefStyle == SMBendStrategyType.OBROUND)
    {
        return SMReliefStyle.OBROUND;
    }
    else if (definition.defaultBendReliefStyle == SMBendStrategyType.TEAR)
    {
        return SMReliefStyle.TEAR;
    }
    else
    {
        return SMReliefStyle.OBROUND;
    }
}

function throwOnUnsupportedFaces(context is Context, faceQ is Query, parameterIds is array)
{
    const supportedGeomTypes = [GeometryType.PLANE, GeometryType.CYLINDER, GeometryType.EXTRUDED];
    var allowedQs = [];
    for (var geom in supportedGeomTypes)
    {
        allowedQs = append(allowedQs, qGeometry(faceQ, geom));
    }
    const unsupportedQ = qSubtraction(faceQ, qUnion(allowedQs));
    if (!isQueryEmpty(context, unsupportedQ))
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_INVALID_FACE, parameterIds, unsupportedQ);
    }
}

/*
 * Methods for RECOGNIZE
 */

/**
 *  @internal
 *  This function uses evOffsetDetection functionality to recognize sheet metal body,
 *  extracts definition sheet surface, replaces cylinders with sharp edges, when possible.
 *  Sheet body is annotated as Model, planar faces are annotated as Walls,
 *  cylinders or sharp edges replacing them are annotated as Bends preserving original radius,
 *  Original sharp edges are annotated as Bends of input radius. TODO : recognize Rips.
 */
annotation { "Feature Type Name" : "Recognize"}
export const sheetMetalRecognize = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
precondition
{
    annotation { "Name" : "Bodies", "Filter" : EntityType.BODY }
    definition.bodies is Query;

    annotation { "Name" : "Radius" }
    isLength(definition.radius, NONNEGATIVE_LENGTH_BOUNDS);

    annotation { "Name" : "Change thickness" }
    definition.changeThickness is boolean;
    if (definition.changeThickness)
    {
        annotation { "Name" : "Thickness" }
        isLength(definition.surfaceThickness, NONNEGATIVE_LENGTH_BOUNDS);
    }

    annotation { "Name" : "Keep input parts" }
    definition.keepInputParts is boolean;

    annotation { "Name" : "K Factor" }
    isReal(definition.kFactor, POSITIVE_REAL_BOUNDS);

    annotation { "Name" : "Minimal gap" }
    isLength(definition.minimalClearance, NONNEGATIVE_LENGTH_BOUNDS);
}
{
    var associationAttributes = getSMAssociationAttributes(context, definition.bodies);
    if (associationAttributes != [])
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_INPUT_BODY_SHOULD_NOT_BE_SHEET_METAL, ["bodies"]);
    }
    var offsetGroups = evOffsetDetection(context, definition);

    if (size(offsetGroups) != size(evaluateQuery(context, definition.bodies)))
    {
        //TODO: - actually a group per body check
        throw regenError(ErrorStringEnum.SHEET_METAL_CANNOT_RECOGNIZE_PARTS, ["bodies"]);
    }

    var objectCount = 0;
    var groupCount = 0;
    var smFacesAndEdgesQ = qNothing();
    for (var group in offsetGroups)
    {
        var surfaceId = id + ("surface_" ~ groupCount);
        var surfaceData = makeSurfaceBody(context, surfaceId, group);
        surfaceData.defaultRadius = definition.radius;
        surfaceData.controlsThickness = definition.changeThickness;
        if (definition.changeThickness)
        {
            surfaceData.thickness = definition.thickness;
        }
        surfaceData.kFactor = definition.kFactor;
        surfaceData.minimalClearance = definition.minimalClearance;
        groupCount += 1;
        smFacesAndEdgesQ = qUnion([smFacesAndEdgesQ, qCreatedBy(surfaceId, EntityType.FACE), qCreatedBy(surfaceId, EntityType.EDGE)]);
        objectCount = annotateSmSurfaceBodies(context, id, surfaceData, objectCount);
        if (getFeatureError(context, id) != undefined)
        {
            return;
        }
    }
    if (!definition.keepInputParts)
    {
        try
        {
            opDeleteBodies(context, id + "deleteBodies", {
                        "entities" : definition.bodies
                    });
        }
        catch
        {
            throw regenError(ErrorStringEnum.REGEN_ERROR);
        }
    }

    finalizeSheetMetalGeometry(context, id, smFacesAndEdgesQ);
}, {"kFactor" : 0.45, "minimalClearance" : 2e-5 * meter, "changeThickness" : false, "keepInputParts" : false});

function makeSurfaceBody(context is Context, id is Id, group is map)
{
    var out = { "thickness" : 0.5 * (group.offsetLow + group.offsetHigh),
                "thicknessDirection" : SMThicknessDirection.BACK };
    try
    {
        opExtractSurface(context, id, {
                    "faces" : qUnion(group.side0),
                    "offset" : 0.0,
                    "useFacesAroundToTrimOffset" : true
                });
        var srfBodies = evaluateQuery(context, qCreatedBy(id, EntityType.BODY));
        if (size(srfBodies) != 1)
        {
            throw regenError("Unexpected number of surfaces extracted");
        }
        out.surfaceBodies = srfBodies[0];
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_CANNOT_THICKEN, ["bodies"]);
    }

    //Collect sharp edges to mark them as default radius bends
    var sharpEdges = [];
    const twoSidedEdges = qOwnedByBody(out.surfaceBodies, EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.TWO_SIDED);
    for (var edge in evaluateQuery(context, twoSidedEdges))
    {
        var convexity = evEdgeConvexity(context, { "edge" : edge });
        if (convexity == EdgeConvexityType.CONVEX || convexity == EdgeConvexityType.CONCAVE)
        {
            sharpEdges = append(sharpEdges, edge);
        }
    }
    out.bendEdgesAndFaces = qUnion(sharpEdges);

    // remove cylindrical faces where possible and collect replacement edges with radius data
    // TODO: when moveEdge functionality is available try extract planar faces,
    // extend to other side of bend or rip and merge
    out.specialRadiiBends = [];
    var cylFaces = evaluateQuery(context, qGeometry(qOwnedByBody(out.surfaceBodies, EntityType.FACE), GeometryType.CYLINDER));
    for (var i = 0; i < size(cylFaces); i += 1)
    {
        var cylSurface = evSurfaceDefinition(context, {
                "face" : cylFaces[i]
            });
        var boundingFaces = evaluateQuery(context, qAdjacent(cylFaces[i], AdjacencyType.EDGE, EntityType.FACE));
        if (size(boundingFaces) != 2)
        {
            continue;
        }
        try
        {
            var removeFilletId = id + ("removeFillet_" ~ i);
            opModifyFillet(context, removeFilletId, {
                        "faces" : cylFaces[i],
                        "modifyFilletType" : ModifyFilletType.REMOVE_FILLET
                    });

            var edges = evaluateQuery(context, qIntersection([qAdjacent(boundingFaces[0], AdjacencyType.EDGE, EntityType.EDGE),
                        qAdjacent(boundingFaces[1], AdjacencyType.EDGE, EntityType.EDGE)]));
            for (var edge in edges)
            {
                var convexity = evEdgeConvexity(context, { "edge" : edge });
                var bendRadius = (convexity == EdgeConvexityType.CONVEX) ? cylSurface.radius - out.thickness : cylSurface.radius;
                out.specialRadiiBends = append(out.specialRadiiBends, [edge, bendRadius]);
            }
        }
        catch
        {
        }
    }
    return out;
}

function getThicknessDirection(context is Context, startModelDefinition is map) returns SMThicknessDirection
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V629_SM_MODEL_FRONT_N_BACK))
    {
        return SMThicknessDirection.BOTH;
    }
    var oppositeDirection = startModelDefinition.oppositeDirection;
    if (startModelDefinition.process == SMProcessType.EXTRUDE &&
        startModelDefinition.oppositeExtrudeDirection == true)
    {
        //Flipping direction of extrude flips surface orientation
        // Flip material side here so that result is symmetric
        // with respect to the sketch plane.
        oppositeDirection = !oppositeDirection;
    }
    return (oppositeDirection) ? SMThicknessDirection.BACK : SMThicknessDirection.FRONT;
}

/**
 * @internal
 */
export function sheetMetalStartManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    for (var manipulator in newManipulators)
    {
        if (manipulator.key == "flipDirectionUpManipulator")
        {
            definition.flipDirectionUp = manipulator.value.flipped;
            return definition;
        }
        else
        {
            return extrudeManipulatorChange(context, definition, newManipulators);
        }
    }
}

/**
 * @internal
 */
export function sheetMetalStartEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    // Preselection processing
    if (oldDefinition == {})
    {
        const bodies = qEntityFilter(definition.initEntities, EntityType.BODY);
        const faces = qEntityFilter(definition.initEntities, EntityType.FACE);
        const edges = qModifiableEntityFilter(qEntityFilter(definition.initEntities, EntityType.EDGE));
        definition.process = SMProcessType.CONVERT;
        if (!isQueryEmpty(context, bodies))
        {
            definition.partToConvert = bodies;
        }
        else if (!isQueryEmpty(context, faces))
        {
            definition.regions = faces;
            definition.process = SMProcessType.THICKEN;
        }
        else if (!isQueryEmpty(context, edges))
        {
            definition.sketchCurves = edges;
            definition.process = SMProcessType.EXTRUDE;
        }
        // Clear out the pre-selection data: this is especially important if the query is to imported data
        definition.initEntities = qNothing();
    }

    // Extrude flips
    // If this is changed, make sure to reflect the change in extrude::extrudeEditLogic.
    if (canSetExtrudeFlips(definition, specifiedParameters))
    {
        if (canSetExtrudeUpToFlip(definition, specifiedParameters))
        {
            definition = extrudeUpToBoundaryFlip(context, definition);
        }
    }
    definition = setExtrudeSecondDirectionFlip(definition, specifiedParameters);

    return definition;
}

