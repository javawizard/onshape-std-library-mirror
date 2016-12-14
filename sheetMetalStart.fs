FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */

export import(path : "onshape/std/query.fs", version : "✨");
export import(path : "onshape/std/smcornerstyle.gen.fs", version : "✨");

import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/boundingtype.gen.fs", version : "✨");
import(path : "onshape/std/box.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/coordSystem.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/error.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/geomOperations.fs", version : "✨");
import(path : "onshape/std/manipulator.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");
import(path : "onshape/std/modifyFillet.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/sketch.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/tool.fs", version : "✨");
import(path : "onshape/std/topologyUtils.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");

/**
 * @internal
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
 * @internal
 */
export enum SMExtrudeBoundingType
{
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Symmetric" }
    SYMMETRIC
}

/**
 * @internal
 */
export enum SMCornerStrategyType
{
    annotation { "Name" : "Rectangle" }
    RECTANGLE,
    annotation { "Name" : "Round" }
    ROUND,
    annotation { "Name" : "Closed" }
    CLOSED,
    annotation { "Name" : "Simple" }
    SIMPLE
}

/**
 * @internal
 */
export enum SMBendStrategyType
{
    annotation { "Name" : "Rectangle" }
    RECTANGLE,
    annotation { "Name" : "Obround" }
    OBROUND,
    annotation { "Name" : "Tear" }
    TEAR
}

/**
 * @internal
 */
export const CORNER_RELIEF_SCALE_BOUNDS =
{
    (unitless) : [1.0, 1.5, 2.0]
} as RealBoundSpec;

/**
 * @internal
 */
export const BEND_RELIEF_SCALE_BOUNDS =
{
    (unitless) : [0.0625, 1.0625, 2.0]
} as RealBoundSpec;

/**
 * @internal
 */
annotation { "Feature Type Name" : "Start Sheet Metal",
             "Manipulator Change Function" : "sheetMetalStartManipulatorChange",
             "Filter Selector" : "allparts",
             "Editing Logic Function" : "sheetMetalStartEditLogic" }
export const sheetMetalStart = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities", "UIHint" : "ALWAYS_HIDDEN", "Filter" : EntityType.BODY || ((EntityType.FACE || EntityType.EDGE) && SketchObject.YES && ConstructionObject.NO) }
        definition.initEntities is Query;

        annotation { "Name" : "Process", "UIHint" : "HORIZONTAL_ENUM" }
        definition.process is SMProcessType;

        // First the entities
        if (definition.process == SMProcessType.CONVERT)
        {
            annotation { "Name" : "Parts and surfaces to convert",
                        "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO && ConstructionObject.NO }
            definition.partToConvert is Query;

            annotation { "Name" : "Faces to Exclude", "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
            definition.facesToExclude is Query;
        }
        else if (definition.process == SMProcessType.EXTRUDE)
        {
            annotation { "Name" : "Sketch curves to extrude",
                        "Filter" : SketchObject.YES && ConstructionObject.NO &&
                            (EntityType.EDGE && GeometryType.LINE) }
            definition.sketchCurves is Query;

            annotation { "Name" : "End type" }
            definition.endBound is SMExtrudeBoundingType;
            annotation { "Name" : "Depth" }
            isLength(definition.depth, NONNEGATIVE_LENGTH_BOUNDS);
            if (definition.endBound == SMExtrudeBoundingType.BLIND)
            {
                annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION", "Default" : false }
                definition.oppositeExtrudeDirection is boolean;
            }
        }
        else if (definition.process == SMProcessType.THICKEN)
        {
            annotation { "Name" : "Faces or sketch regions to thicken",
                        "Filter" : EntityType.FACE && GeometryType.PLANE && ConstructionObject.NO }
            definition.regions is Query;
        }

        if (definition.process == SMProcessType.THICKEN || definition.process == SMProcessType.CONVERT)
        {
            annotation { "Name" : "Bends", "Filter" : EntityType.EDGE && EdgeTopology.TWO_SIDED && GeometryType.LINE && SketchObject.NO }
            definition.bends is Query;

            annotation { "Name" : "Clearance from input" }
            isLength(definition.clearance, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

            annotation { "Name" : "Clearance includes bends" }
            definition.bendsIncluded is boolean;
        }

        if (definition.process == SMProcessType.CONVERT)
        {
            annotation { "Name" : "Keep Input Parts" }
            definition.keepInputParts is boolean;
        }

        // Then some common parameters
        annotation { "Name" : "Thickness", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        isLength(definition.thickness, SM_THICKNESS_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Bend Radius", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        isLength(definition.radius, SM_BEND_RADIUS_BOUNDS);

        annotation { "Name" : "K Factor", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        isReal(definition.kFactor, K_FACTOR_BOUNDS);

        annotation { "Name" : "Minimal clearance", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        isLength(definition.minimalClearance, SM_MINIMAL_CLEARANCE_BOUNDS);

        annotation { "Name" : "Corner relief type",
                     "Default" : SMCornerStrategyType.RECTANGLE,
                     "UIHint" : ["SHOW_LABEL", "REMEMBER_PREVIOUS_VALUE"] }
        definition.defaultCornerStyle is SMCornerStrategyType;

        annotation { "Name" : "Corner relief scale", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        isReal(definition.defaultCornerReliefScale, CORNER_RELIEF_SCALE_BOUNDS);

        annotation { "Name" : "Bend relief type",
                     "Default" : SMBendStrategyType.OBROUND,
                     "UIHint" : ["SHOW_LABEL", "REMEMBER_PREVIOUS_VALUE"] }
        definition.defaultBendReliefStyle is SMBendStrategyType;

        annotation { "Name" : "Bend relief scale", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        isReal(definition.defaultBendReliefScale, BEND_RELIEF_SCALE_BOUNDS);
    }
    {
        if (definition.process == SMProcessType.CONVERT)
        {
            checkNotInFeaturePattern(context, definition.partToConvert);
            convertExistingPart(context, id, definition);
        }
        else if (definition.process == SMProcessType.EXTRUDE)
        {
            checkNotInFeaturePattern(context, definition.sketchCurves);
            extrudeSheetMetal(context, id, definition);
        }
        else if (definition.process == SMProcessType.THICKEN)
        {
            checkNotInFeaturePattern(context, definition.regions);
            thickenToSheetMetal(context, id, definition);
        }
    }, { "kFactor" : 0.45,
     "minimalClearance" : 2e-5 * meter,
      "oppositeDirection" : false,
      "initEntities" : qNothing(),
      "defaultCornerStyle" :  SMCornerStrategyType.RECTANGLE,
      "defaultCornerReliefScale" : 1.5,
      "defaultBendReliefStyle" :  SMBendStrategyType.OBROUND,
      "defaultBendReliefScale" : 1.0625
    });

function finalizeSheetMetalGeometry(context is Context, id is Id, entities is Query)
{
    try
    {
        updateSheetMetalGeometry(context, id, { "entities" : entities });
    }
    catch (e)
    {
        if (e.message == ErrorStringEnum.BOOLEAN_INVALID)
        {
            // I can't think of anything more useful to tell the user right now. Analyzing such cases
            // may make it clearer when it can happen
            throw regenError(ErrorStringEnum.SHEET_METAL_REBUILD_ERROR);
        }
        else if (e.message == ErrorStringEnum.BAD_GEOMETRY)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_CANNOT_THICKEN);
        }
        else
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_REBUILD_ERROR);
        }
    }
}

/*
 * Methods for CONVERT
 */

function convertExistingPart(context is Context, id is Id, definition is map)
{
    if (size(evaluateQuery(context, definition.partToConvert)) < 1)
    {
        throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["partToConvert"]);
    }

    var associationAttributes = getAttributes(context, {
            "entities" : definition.partToConvert,
            "attributePattern" : {} as SMAssociationAttribute
        });
    if (size(associationAttributes) != 0)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_INPUT_BODY_SHOULD_NOT_BE_SHEET_METAL, ["partToConvert"]);
    }

    var facesOnUnknownBodies = evaluateQuery(context, qSubtraction(qOwnerBody(definition.facesToExclude), definition.partToConvert));
    if (size(facesOnUnknownBodies) > 0)
    {
        reportFeatureWarning(context, id, ErrorStringEnum.FACES_NOT_OWNED_BY_PARTS);
    }
    var edgesOnUnknownBodies = evaluateQuery(context, qSubtraction(qOwnerBody(definition.bends), definition.partToConvert));
    if (size(edgesOnUnknownBodies) > 0)
    {
        reportFeatureWarning(context, id, ErrorStringEnum.EDGES_NOT_OWNED_BY_PARTS);
    }

    var complimentFacesQ = qSubtraction(qOwnedByBody(definition.partToConvert, EntityType.FACE), definition.facesToExclude);

    var nFacesToExclude = size(evaluateQuery(context, definition.facesToExclude));
    var nComplimentFaces = size(evaluateQuery(context, complimentFacesQ));
    var nBends = size(evaluateQuery(context, definition.bends));

    // Let's be careful to screen out unwanted faces here, i.e. anything that isn't planar
    var planarFaces = qGeometry(complimentFacesQ, GeometryType.PLANE);
    var badFaces = qSubtraction(complimentFacesQ, planarFaces);
    if (size(evaluateQuery(context, badFaces)) > 0)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_CONVERT_PLANE, ["partToConvert", "facesToExclude"], badFaces);
    }

    var bendEdgesQ = convertFaces(context, id, definition, complimentFacesQ);
    definition.remindToSelectBends = (nBends == 0 && nFacesToExclude > 0 && nComplimentFaces > 1);
    annotateConvertedFaces(context, id, definition, bendEdgesQ);
}

function convertFaces(context is Context, id is Id, definition, faces is Query) returns Query
{
    var surfaceId = id + "extractSurface";
    var bendEdgesQ = startTracking(context, { "subquery" : definition.bends });
    var offset = computeSurfaceOffset(context, definition);

    try
    {
        opExtractSurface(context, surfaceId, {
                    "faces" : faces,
                    "offset" : offset });
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_CANNOT_THICKEN, ["partToConvert", "facesToExclude", "regions"]);
    }

    return bendEdgesQ;
}

function annotateConvertedFaces(context is Context, id is Id, definition, bendEdgesQuery is Query)
{
    try
    {
        annotateSmSurfaceBodies(context, id, {
                    "surfaceBodies" : qCreatedBy(id, EntityType.BODY),
                    "bendEdges" : bendEdgesQuery,
                    "specialRadiiBends" : [],
                    "defaultRadius" : definition.radius,
                    "controlsThickness" : true,
                    "thickness" : definition.thickness,
                    "minimalClearance" : definition.minimalClearance,
                    "kFactor" : definition.kFactor,
                    "defaultTwoCornerStyle" : getDefaultTwoCornerStyle(definition),
                    "defaultThreeCornerStyle" : getDefaultThreeCornerStyle(definition),
                    "defaultBendReliefStyle" : getDefaultBendReliefStyle(definition),
                    "defaultCornerReliefScale" : definition.defaultCornerReliefScale,
                    "defaultBendReliefScale" : definition.defaultBendReliefScale}, 0);
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
        var edges = evaluateQuery(context, definition.bends);
        if (size(edges) > 0)
        {
            for (var edge in edges)
            {
                var adjacentWalls = qSubtraction(qEdgeAdjacent(edge, EntityType.FACE), definition.facesToExclude);
                if (size(evaluateQuery(context, adjacentWalls)) == 0)
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
    var offset = 0.5 * definition.thickness + wallClearance;
    if (definition.oppositeDirection)
    {
        offset = -offset;
    }
    return offset;
}

/*
 * Methods for EXTRUDE
 */

const DEPTH_MANIPULATOR = "depthManipulator";

function extrudeSheetMetal(context is Context, id is Id, definition is map)
{
    const sheetQuery = extrudeSketchCurves(context, id, definition);
    const createdSheetBodies = evaluateQuery(context, sheetQuery);
    if (size(createdSheetBodies) == 0)
    {
        throw regenError(ErrorStringEnum.EXTRUDE_SURF_NO_CURVE);
    }

    // Regardless of whether the sheets were created by curves or regions
    // we want to offset the sheet by half the thickness
    var oppositeOffset = definition.oppositeDirection;
    if (definition.oppositeExtrudeDirection == true)
    {
        oppositeOffset = !oppositeOffset;
    }
    offsetSheets(context, id, sheetQuery, definition.thickness, oppositeOffset);

    const facesAndEdges = addSheetMetalDataToSheet(context, id, sheetQuery, definition);
    finalizeSheetMetalGeometry(context, id, facesAndEdges);
}

function offsetSheets(context is Context, id is Id, sheetQuery is Query, thickness is ValueWithUnits, oppositeDirection is boolean)
{
    opOffsetFace(context, id + "offsetFaces", {
                "moveFaces" : qOwnedByBody(sheetQuery, EntityType.FACE),
                "offsetDistance" : thickness * 0.5 * (oppositeDirection ? -1 : 1)
            });
}

function thickenToSheetMetal(context is Context, id is Id, definition is map)
{
    const evaluatedFaceQueries = evaluateQuery(context, definition.regions);
    if (size(evaluatedFaceQueries) == 0)
    {
        throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["regions"]);
    }
    var sketchPlaneToFacesMap = {};
    var facesToConvert = [];
    var index = 0;
    for (var evaluatedFace in evaluatedFaceQueries)
    {
        var key = try(evOwnerSketchPlane(context, { "entity" : evaluatedFace }));
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
    var bendEdgesQ = qNothing();
    var nFaces = size(facesToConvert);
    var nBends = size(evaluateQuery(context, definition.bends));
    if (nFaces != 0)
    {
        bendEdgesQ = convertFaces(context, id, definition, qUnion(facesToConvert));
    }
    definition.keepInputParts = true;
    definition.remindToSelectBends = (nFaces > 1 && nBends == 0);
    annotateConvertedFaces(context, id, definition, bendEdgesQ);
}

function convertRegion(context is Context, id is Id, definition is map)
{
    const extrudeId = id + "extrude";
    const sign = definition.oppositeDirection ? -1 : 1;
    opExtrude(context, extrudeId, {
                "entities" : definition.regions,
                "direction" : sign * evPlane(context, { "face" : definition.regions }).normal,
                "endBound" : BoundingType.BLIND,
                "endDepth" : definition.thickness / 2 + definition.clearance,
                "hasSecondDirection" : true,
                "secondDirectionOppositeDirection" : false,
                "secondDirectionDepth" : definition.clearance
            });
    var createdQuery = qCreatedBy(extrudeId, EntityType.BODY);
    var isStartCap = false;
    opExtractSurface(context, id + "extract", { "faces" : qEntityFilter(qCapEntity(extrudeId, isStartCap), EntityType.FACE) });
    opDeleteBodies(context, id + "deleteBodies", {
                "entities" : createdQuery
            });
}

function extrudeSketchCurves(context is Context, id is Id, definition is map) returns Query
{
    var sketchCurves = qGeometry(definition.sketchCurves, GeometryType.LINE);
    const resolvedEntities = evaluateQuery(context, sketchCurves);
    if (size(resolvedEntities) > 0)
    {
        const extrudeAxis = getExtrudeDirection(context, resolvedEntities[0]);
        addExtrudeManipulator(context, id, definition, extrudeAxis);
        const extrudeId = id + "extrude";

        definition.entities = sketchCurves;
        definition.direction = extrudeAxis.direction;
        if (definition.oppositeExtrudeDirection)
            definition.direction *= -1;

        if (definition.depth != undefined && definition.depth < 0)
        {
            definition.depth *= -1;
            if (definition.endBound == SMExtrudeBoundingType.BLIND)
                definition.direction *= -1;
        }

        definition.startBound = BoundingType.BLIND;
        definition.startDepth = 0;
        definition.endDepth = definition.depth;
        definition.isStartBoundOpposite = false;

        if (definition.endBound == SMExtrudeBoundingType.SYMMETRIC)
        {
            definition.endBound = BoundingType.BLIND;
            definition.startDepth = definition.depth * -0.5;
            definition.endDepth = definition.depth * 0.5;
        }

        opExtrude(context, extrudeId, definition);
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

function addExtrudeManipulator(context is Context, id is Id, definition is map, extrudeAxis is Line)
{
    var offset = definition.depth;
    if (definition.endBound == SMExtrudeBoundingType.SYMMETRIC)
        offset *= 0.5;
    if (definition.oppositeExtrudeDirection)
        offset *= -1;
    addManipulators(context, id, { (DEPTH_MANIPULATOR) :
                    linearManipulator(extrudeAxis.origin,
                        extrudeAxis.direction,
                        offset,
                        definition.entities) });
}

function addSheetMetalDataToSheet(context is Context, id is Id, surfaceBodies is Query, definition is map)
{
    var sharpEdges = [];
    for (var edge in evaluateQuery(context, qOwnedByBody(surfaceBodies, EntityType.EDGE)))
    {
        if (!edgeIsTwoSided(context, edge))
        {
            continue;
        }
        var convexity = evEdgeConvexity(context, { "edge" : edge });
        if (convexity == EdgeConvexityType.CONVEX || convexity == EdgeConvexityType.CONCAVE)
        {
            sharpEdges = append(sharpEdges, edge);
        }
    }
    var surfaceData =
    {
        "defaultRadius" : definition.radius,
        "surfaceBodies" : surfaceBodies,
        "bendEdges" : qUnion(sharpEdges),
        "specialRadiiBends" : [],
        "thickness" : definition.thickness,
        "controlsThickness" : true,
        "minimalClearance" : definition.minimalClearance,
        "kFactor" : definition.kFactor,
        "defaultTwoCornerStyle" : getDefaultTwoCornerStyle(definition),
        "defaultThreeCornerStyle" : getDefaultThreeCornerStyle(definition),
        "defaultBendReliefStyle" : getDefaultBendReliefStyle(definition),
        "defaultCornerReliefScale" : definition.defaultCornerReliefScale,
        "defaultBendReliefScale" : definition.defaultBendReliefScale
    };

    try
    {
        annotateSmSurfaceBodies(context, id, surfaceData, 0);
        if (getFeatureError(context, id) != undefined)
        {
            return;
        }
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_CANNOT_THICKEN);
    }

    return qUnion([qOwnedByBody(surfaceData.surfaceBodies, EntityType.FACE), qUnion(sharpEdges)]);
}

function getDefaultTwoCornerStyle(definition is map) returns SMCornerStyle
{
    if (definition.defaultCornerStyle == SMCornerStrategyType.RECTANGLE)
    {
        return SMCornerStyle.SQUARE;
    }
    else if (definition.defaultCornerStyle == SMCornerStrategyType.ROUND)
    {
        return SMCornerStyle.ROUND;
    }
    else if (definition.defaultCornerStyle == SMCornerStrategyType.CLOSED)
    {
        return SMCornerStyle.ARC;
    }
    else if (definition.defaultCornerStyle == SMCornerStrategyType.SIMPLE)
    {
        return SMCornerStyle.SIMPLE;
    }
    else
    {
        return SMCornerStyle.SQUARE;
    }
}

function getDefaultThreeCornerStyle(definition is map) returns SMCornerStyle
{
    if (definition.defaultCornerStyle == SMCornerStrategyType.RECTANGLE)
    {
        return SMCornerStyle.SQUARE;
    }
    else if (definition.defaultCornerStyle == SMCornerStrategyType.ROUND)
    {
        return SMCornerStyle.ROUND;
    }
    else if (definition.defaultCornerStyle == SMCornerStrategyType.SIMPLE)
    {
        return SMCornerStyle.SIMPLE;
    }
    else
    {
        return SMCornerStyle.SQUARE;
    }
}

function getDefaultBendReliefStyle(definition is map) returns SMCornerStyle
{
    if (definition.defaultBendReliefStyle == SMBendStrategyType.RECTANGLE)
    {
        return SMCornerStyle.SQUARE;
    }
    else if (definition.defaultBendReliefStyle == SMBendStrategyType.OBROUND)
    {
        return SMCornerStyle.OBROUND;
    }
    else if (definition.defaultBendReliefStyle == SMBendStrategyType.TEAR)
    {
        return SMCornerStyle.TEAR;
    }
    else
    {
        return SMCornerStyle.OBROUND;
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
}
{
    var associationAttributes = getAttributes(context, {
            "entities" : definition.bodies,
            "attributePattern" : {} as SMAssociationAttribute
        });
    if (size(associationAttributes) != 0)
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
}, {"kFactor" : 0.45, "minimalClearance" : 2e-5 * meter});

function makeSurfaceBody(context is Context, id is Id, group is map)
{
    var out = { "thickness" : 0.5 * (group.offsetLow + group.offsetHigh) };
    try
    {
        opExtractSurface(context, id, {
                    "faces" : qUnion(group.side0),
                    "offset" : -0.5 * out.thickness
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
    for (var edge in evaluateQuery(context, qOwnedByBody(out.surfaceBodies, EntityType.EDGE)))
    {
        if (!edgeIsTwoSided(context, edge))
        {
            continue;
        }
        var convexity = evEdgeConvexity(context, { "edge" : edge });
        if (convexity == EdgeConvexityType.CONVEX || convexity == EdgeConvexityType.CONCAVE)
        {
            sharpEdges = append(sharpEdges, edge);
        }
    }
    out.bendEdges = qUnion(sharpEdges);

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
        var boundingFaces = evaluateQuery(context, qEdgeAdjacent(cylFaces[i], EntityType.FACE));
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

            var edges = evaluateQuery(context, qIntersection([qEdgeAdjacent(boundingFaces[0], EntityType.EDGE),
                        qEdgeAdjacent(boundingFaces[1], EntityType.EDGE)]));
            for (var edge in edges)
            {
                out.specialRadiiBends = append(out.specialRadiiBends, [edge, cylSurface.radius - 0.5 * out.thickness]);
            }
        }
        catch
        {
        }
    }
    return out;
}

/**
 * @internal
 */
export function sheetMetalStartManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    if (newManipulators[DEPTH_MANIPULATOR] is map)
    {
        var newOffset = newManipulators[DEPTH_MANIPULATOR].offset;
        if (definition.endBound == SMExtrudeBoundingType.SYMMETRIC)
            newOffset *= 2;
        definition.oppositeExtrudeDirection = newOffset < 0 * meter;
        definition.depth = abs(newOffset);
    }

    return definition;
}

/**
 * @internal
 */
export function sheetMetalStartEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (oldDefinition == {})
    {
        const bodies = qEntityFilter(definition.initEntities, EntityType.BODY);
        const planarFaces = qGeometry(qEntityFilter(definition.initEntities, EntityType.FACE), GeometryType.PLANE);
        const edges = qEntityFilter(definition.initEntities, EntityType.EDGE);
        definition.process = SMProcessType.CONVERT;
        if (size(evaluateQuery(context, bodies)) > 0)
        {
            definition.partToConvert = bodies;
        }
        else if (size(evaluateQuery(context, planarFaces)) > 0)
        {
            definition.regions = planarFaces;
            definition.process = SMProcessType.THICKEN;
        }
        else if (size(evaluateQuery(context, edges)) > 0)
        {
            definition.sketchCurves = edges;
            definition.process = SMProcessType.EXTRUDE;
        }
    }
    return definition;
}

