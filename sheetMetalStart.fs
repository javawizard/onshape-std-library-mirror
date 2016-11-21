FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */

export import(path : "onshape/std/sheetMetalFlange.fs", version : "✨");
export import(path : "onshape/std/sheetMetalFlangeOld.fs", version : "✨");

export import(path : "onshape/std/query.fs", version : "✨");

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
export enum SMEnclosureType
{
    annotation { "Name" : "Use profile" }
    PROFILE,
    annotation { "Name" : "Enclose parts" }
    PARTS
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
annotation { "Feature Type Name" : "Start Sheet Metal",
             "Manipulator Change Function" : "smStartManipulatorChange" }
export const smStartSheetMetal = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Process", "UIHint" : "HORIZONTAL_ENUM" }
        definition.process is SMProcessType;

        // First the entities
        if (definition.process == SMProcessType.CONVERT)
        {
            annotation { "Name" : "Parts and surfaces to convert",
                        "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO && ConstructionObject.NO }
            definition.partToConvert is Query;

            annotation { "Name" : "Faces to Exclude", "Filter" : EntityType.FACE }
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
            annotation { "Name" : "Faces to thicken",
                        "Filter" : EntityType.FACE && GeometryType.PLANE }
            definition.regions is Query;
        }

        if (definition.process == SMProcessType.THICKEN || definition.process == SMProcessType.CONVERT)
        {
            annotation { "Name" : "Bends", "Filter" : EntityType.EDGE && EdgeTopology.TWO_SIDED && GeometryType.LINE && SketchObject.NO }
            definition.bends is Query;
        }

        // Then some common parameters
        annotation { "Name" : "K Factor" }
        isReal(definition.kFactor, K_FACTOR_BOUNDS);

        annotation { "Name" : "Minimal clearance" }
        isLength(definition.minimalClearance, SM_MINIMAL_CLEARANCE_BOUNDS);

        annotation { "Name" : "Thickness" }
        isLength(definition.thickness, SHELL_OFFSET_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Bend Radius" }
        isLength(definition.radius, BLEND_BOUNDS);
        if (definition.process == SMProcessType.CONVERT)
        {
            annotation { "Name" : "Keep Input Parts" }
            definition.keepInputParts is boolean;
        }
        if (definition.process == SMProcessType.THICKEN || definition.process == SMProcessType.CONVERT)
        {
            annotation { "Name" : "Clearance" }
            isLength(definition.clearance, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

            annotation { "Name" : "Bends Included in Clearance" }
            definition.bendsIncluded is boolean;
        }
    }
    {
        if (definition.process == SMProcessType.CONVERT)
        {
            convertExistingPart(context, id, definition);
        }
        else if (definition.process == SMProcessType.EXTRUDE)
        {
            extrudeSheetMetal(context, id, definition);
        }
        else if (definition.process == SMProcessType.THICKEN)
        {
            thickenToSheetMetal(context, id, definition);
        }
    }, { "kFactor" : 0.45, "minimalClearance" : 2e-5 * meter, "oppositeDirection" : false });

/**
 * @internal
 */
export enum SMProcessTypeInternal
{
    annotation { "Name" : "Recognize" }
    RECOGNIZE,
    annotation { "Name" : "Enclose" }
    ENCLOSE
}

/**
 * @internal
 * For testing use only, to expose enclose and recognize options to tests.
 */
annotation { "Feature Type Name" : "Start Sheet Metal", "Manipulator Change Function" : "smStartManipulatorChange", "UIHint" : "ALWAYS_HIDDEN" }
export const smStartSheetMetalInternal = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
precondition
    {
    }
    {
        if (definition.process == SMProcessTypeInternal.RECOGNIZE)
        {
            smRecognize(context, id, definition);
        }
        else if (definition.process == SMProcessTypeInternal.ENCLOSE)
        {
            throw regenError("Functionality pending adjustment to new SM approach", ['process']);
            if (definition.SMEnclosureType == SMEnclosureType.PROFILE)
            {
                makeProfileEnclosure(context, id, definition);
            }
            else
            {
                makeBoundingBoxEnclosure(context, id, definition);
            }
        }
    }, {"kFactor" : 0.45, "minimalClearance" : 2e-5 * meter});

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

    // Let's be careful to screen out unwanted faces here, i.e. anything that isn't cylindrical or planar
    var planarFaces = qGeometry(complimentFacesQ, GeometryType.PLANE);
    var badFaces = qSubtraction(complimentFacesQ, planarFaces);
    if (size(evaluateQuery(context, badFaces)) > 0)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_CONVERT_PLANE, ["partToConvert", "facesToExclude"], badFaces);
    }

    var bendEdgesQ = convertFaces(context, id, definition, complimentFacesQ);
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
                    "kFactor" : definition.kFactor }, 0);
        if (getFeatureError(context, id) != undefined)
        {
            return;
        }
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_CANNOT_THICKEN);
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

    if (size(facesToConvert) != 0)
    {
        bendEdgesQ = convertFaces(context, id, definition, qUnion(facesToConvert));
    }
    definition.keepInputParts = true;
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
                "endDepth" : definition.thickness / 2
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
        "kFactor" : definition.kFactor
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

/*
 * Methods for RECOGNIZE
 */

/**
 *  This function uses evOffsetDetection functionality to recognize sheet metal body,
 *  extracts definition sheet surface, replaces cylinders with sharp edges, when possible.
 *  Sheet body is annotated as Model, planar faces are annotated as Walls,
 *  cylinders or sharp edges replacing them are annotated as Bends preserving original radius,
 *  Original sharp edges are annotated as Bends of input radius. TODO : recognize Rips.
 */
function smRecognize(context is Context, id is Id, definition is map)
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
}

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

/*
 * Methods for ENCLOSE
 */

const HEIGHT_MANIPULATOR = "heightManipulator";
const WIDTH_MANIPULATOR = "widthManipulator";
const MULTIPLE_MANIPULATORS = false;

const ANGLE_90_DEGREE = 90 * degree;

function makeBase(context is Context, id is Id, index is number, entities is Query, normal is Vector, definition is map) returns Query
{
    opExtrude(context, id + ("extrude_base_" ~ index), {
                "entities" : entities,
                "direction" : normal,
                "endBound" : BoundingType.BLIND,
                "endDepth" : definition.thickness
            });
    smRecognize(context, id + ("recognize_" ~ index), {
                bodies : qCreatedBy(id + ("extrude_base_" ~ index), EntityType.BODY)
            });

    var topolQuery = makeQuery(id + ("extrude_base_" ~ index), "CAP_FACE", EntityType.FACE, {});
    return topolQuery;
}

function makeCover(context is Context, id is Id, index is number, entities is Query, normal is Vector, definition is map)
{
    opExtrude(context, id + ("extrude_cover_" ~ index), {
                "entities" : entities,
                "direction" : normal,
                "startBound" : BoundingType.BLIND,
                "startDepth" : -definition.height + definition.thickness,
                "endBound" : BoundingType.BLIND,
                "endDepth" : definition.height
            });
    smRecognize(context, id + ("recognize_cover_" ~ index), {
                bodies : qCreatedBy(id + ("extrude_cover_" ~ index), EntityType.BODY)
            });
    var growSize = 0 * meter;
    if (definition.bendType == smFlangeJointType.NONE)
        growSize = definition.radius + definition.thickness;
    else if (definition.bendType == smFlangeJointType.FLUSH_INNER_INNER)
        growSize = definition.thickness;
    else if (definition.bendType == smFlangeJointType.FLUSH_OUTER_INNER)
        growSize = 0 * meter;
    else if (definition.bendType == smFlangeJointType.FLUSH_INNER_OUTER)
        growSize = 2 * definition.thickness;
    if (growSize != 0)
    {
        var faces = qSubtraction(qCreatedBy(id + ("extrude_cover_" ~ index), EntityType.FACE),
            qUnion([
                    qCapEntity(id + ("extrude_cover_" ~ index), true),
                    qCapEntity(id + ("extrude_cover_" ~ index), false)
                ]));
        opOffsetFace(context, id + ("offsetFace_" ~ index), {
                    "moveFaces" : faces,
                    "offsetDistance" : growSize
                });
    }
}

function isFaceRectangular(context is Context, face is Query) returns boolean
{
    var edges = evaluateQuery(context, qEdgeAdjacent(face, EntityType.EDGE));
    var lines = [];
    for (var edge in edges)
    {
        try
        {
            var line = evLine(context, {
                    "edge" : edge
                });
            lines = append(lines, line);
        }
        catch
        {
            return false;
        }
    }
    if (size(lines) != 4)
        return false;
    var numPerpendicular = 0;
    var numParallel = 0;
    for (var i = 1; i < size(lines); i += 1)
    {
        if (squaredNorm(cross(lines[0].direction, lines[i].direction)) < TOLERANCE.zeroAngle * TOLERANCE.zeroAngle)
        {
            numParallel += 1;
        }
        else if (dot(lines[0].direction, lines[i].direction) < TOLERANCE.zeroAngle)
        {
            numPerpendicular += 1;
        }
    }
    return numParallel == 1 && numPerpendicular == 2;
}

/**
 * This feature produces an enclosure built around a profile
 */
function makeProfileEnclosure(context is Context, id is Id, definition is map)
{
    definition.bendAngle = ANGLE_90_DEGREE;
    definition.oppositeDirection = false;
    var index = -1;
    var faces = evaluateQuery(context, definition.profileFaces);
    for (var face in faces)
    {
        index += 1;
        if (!isFaceRectangular(context, face))
            throw regenError(ErrorStringEnum.FACE_IS_NOT_RECTANGLE);
        var facePlane = evPlane(context, {
                "face" : face
            });
        var topolQuery = makeBase(context, id, index, face, facePlane.normal, definition);
        if (index == 0)
        {
            var edges = evaluateQuery(context, qEdgeAdjacent(topolQuery, EntityType.EDGE));
            addHeightManipulator(context, id, edges, definition);
            addTopFlangeWidthManipulator(context, id, edges, definition);
        }
        var cSys = coordSystem(facePlane.origin, facePlane.x, facePlane.normal);
        makeEnclosure(context, id, topolQuery, cSys, definition);
        if (definition.cover)
        {
            makeCover(context, id, index, face, facePlane.normal, definition);
        }
    }
}

/**
 * This feature produces an enclosure built around a bounding box
 */
function makeBoundingBoxEnclosure(context is Context, id is Id, definition is map)
{
    var cSys = coordSystem(vector(0, 0, 0) * meter, vector(1, 0, 0), vector(0, 0, 1));
    var mateConnectors = evaluateQuery(context, definition.refConnector);
    if (size(mateConnectors) == 1)
    {
        cSys = evMateConnector(context, {
                    "mateConnector" : definition.refConnector
                });
    }
    var bBox = evBox3d(context, {
            "topology" : definition.parts,
            "cSys" : cSys
        });

    bBox = extendBox3d(bBox, definition.margin, 0);
    var width = bBox.maxCorner[0] - bBox.minCorner[0];
    var depth = bBox.maxCorner[1] - bBox.minCorner[1];
    var height = bBox.maxCorner[2] - bBox.minCorner[2];

    var origin = toWorld(cSys, bBox.minCorner);
    var sketchCSys = coordSystem(origin, cSys.xAxis, cSys.zAxis);
    var sketch = newSketchOnPlane(context, id + "enclosure_sketch", {
            "sketchPlane" : plane(sketchCSys)
        });
    skRectangle(sketch, "base", {
                "firstCorner" : vector(0, 0) * inch,
                "secondCorner" : vector(width, depth)
            });

    skSolve(sketch);

    var topolQuery = makeBase(context, id, 0, qSketchRegion(id + "enclosure_sketch", true), cSys.zAxis, definition);

    var edges = evaluateQuery(context, qEdgeAdjacent(topolQuery, EntityType.EDGE));
    definition.height = height;
    addTopFlangeWidthManipulator(context, id, edges, definition);
    makeEnclosure(context, id, topolQuery, cSys, definition);
    if (definition.cover)
    {
        makeCover(context, id, 0, qSketchRegion(id + "enclosure_sketch", true), cSys.zAxis, definition);
    }
}

function makeEnclosure(context is Context, id is Id, topolQuery is Query, cSys is CoordSystem, definition is map)
{
    if (definition.cover)
    {
        definition.height -= definition.thickness;
    }

    var flangeResult = smFlangeFunction(context, id + "base_flange", false, {
            "innerRadius" : definition.radius,
            "height" : definition.height,
            "bendAngle" : ANGLE_90_DEGREE,
            "bendType" : definition.bendType == undefined ? smFlangeJointType.FLUSH_INNER_INNER : definition.bendType,
            "flushSides" : true,
            "mitered" : false,
            "oppositeDirection" : false,
            "relief" : false,
            "topols" : topolQuery
        });
    if (definition.topFlange)
    {
        smFlangeOld(context, id + "top_flange", {
                    "innerRadius" : definition.radius,
                    "height" : definition.width,
                    "bendAngle" : ANGLE_90_DEGREE,
                    "bendType" : smFlangeJointType.FLUSH_OUTER_INNER,
                    "flushSides" : false,
                    "mitered" : true,
                    "miterBend" : true,
                    "miterAngle" : 45 * degree,
                    "oppositeDirection" : false,
                    "relief" : false,
                    "topols" : flangeResult.innerEdgeQueries
                });
    }
}

function addHeightManipulator(context is Context, id is Id, edges is array, definition is map)
{
    if (size(edges) == 0)
        return;
    var i = -1;
    for (var edge in edges)
    {
        i += 1;
        if (i == 0 || MULTIPLE_MANIPULATORS)
        {
            var cSys = getEdgeCSys(context, edge);
            var yAxis = yAxis(cSys);
            const usedEntities = edge;
            var v = cos(definition.bendAngle) * -yAxis + sin(definition.bendAngle) * cSys.zAxis;

            var offset = definition.height;
            addManipulators(context, id, { (HEIGHT_MANIPULATOR ~ "_" ~ i) :
                            linearManipulator(cSys.origin,
                                v,
                                offset,
                                usedEntities) });
        }
    }
}

function addTopFlangeWidthManipulator(context is Context, id is Id, edges is array, definition is map)
{
    if (size(edges) == 0)
        return;
    var i = -1;
    for (var edge in edges)
    {
        i += 1;
        if (i == 0 || MULTIPLE_MANIPULATORS)
        {
            var cSys = getEdgeCSys(context, edge);
            var yAxis = yAxis(cSys);
            const usedEntities = edge;
            var angle = definition.bendAngle == undefined ? ANGLE_90_DEGREE : definition.bendAngle;
            var v0 = cos(angle) * -yAxis + sin(angle) * cSys.zAxis;
            var v1 = -sin(angle) * -yAxis + cos(angle) * cSys.zAxis;

            var offset = definition.width;
            addManipulators(context, id, { (WIDTH_MANIPULATOR ~ "_" ~ i) :
                            linearManipulator(cSys.origin + definition.height * v0 - (definition.radius + 2 * definition.thickness) * v1,
                                v1,
                                offset,
                                usedEntities) });
        }
    }
}

/**
 * @internal
 */
export function smStartManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    if (newManipulators[DEPTH_MANIPULATOR] is map)
    {
        var newOffset = newManipulators[DEPTH_MANIPULATOR].offset;
        if (definition.endBound == SMExtrudeBoundingType.SYMMETRIC)
            newOffset *= 2;
        definition.oppositeExtrudeDirection = newOffset < 0 * meter;
        definition.depth = abs(newOffset);
    }

    for (var entry in newManipulators)
    {
        var matchResult;

        matchResult = match(entry.key, HEIGHT_MANIPULATOR ~ "_.*");
        if (matchResult.hasMatch && newManipulators[entry.key] is map)
        {
            var newOffset = newManipulators[entry.key].offset;
            definition.height = abs(newOffset);
        }

        matchResult = match(entry.key, WIDTH_MANIPULATOR ~ "_.*");
        if (matchResult.hasMatch && newManipulators[entry.key] is map)
        {
            definition.width = newManipulators[entry.key].offset;
        }
    }

    return definition;
}

