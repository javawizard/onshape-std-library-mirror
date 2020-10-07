FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "✨");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "✨");
export import(path : "onshape/std/filletcrosssection.gen.fs", version : "✨");

// Imports used internally
import(path : "onshape/std/edgeconvexitytype.gen.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalCornerBreak.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/tool.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");

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


/**
 * Feature performing an [opFillet].
 */
annotation { "Feature Type Name" : "Fillet", "Manipulator Change Function" : "filletManipulatorChange",
             "Filter Selector" : "allparts",  "Editing Logic Function" : "filletEditLogic"}
export const fillet = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities to fillet",
                    "Filter" : ((ActiveSheetMetal.NO && ((EntityType.EDGE && EdgeTopology.TWO_SIDED) || EntityType.FACE))
                                || (EntityType.EDGE && SheetMetalDefinitionEntityType.VERTEX))
                        && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES,
                    "AdditionalBoxSelectFilter" : EntityType.EDGE }
        definition.entities is Query;

        annotation { "Name" : "Tangent propagation", "Default" : true }
        definition.tangentPropagation is boolean;

        annotation { "Name" : "Cross section", "UIHint" : UIHint.SHOW_LABEL }
        definition.crossSection is FilletCrossSection;

        annotation { "Name" : "Radius" }
        isLength(definition.radius, BLEND_BOUNDS);

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
        annotation {"Name" : "Defaults changed", "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.defaultsChanged is boolean;

        if (definition.crossSection != FilletCrossSection.CURVATURE)
        {
            annotation {"Name" : "Allow edge overflow", "Default" : true }
            definition.allowEdgeOverflow is boolean;
        }

        annotation {"Name" : "Variable fillet"}
        definition.isVariable is boolean;

        if (definition.isVariable)
        {
            annotation { "Name" : "Vertices", "Item name" : "vertex",
                        "Driven query" : "vertex", "Item label template" : "[#vertexRadius] #vertex",
                        "UIHint" : UIHint.PREVENT_ARRAY_REORDER }
            definition.vertexSettings is array;
            for (var setting in definition.vertexSettings)
            {
                annotation { "Name" : "Vertex", "Filter" : ModifiableEntityOnly.YES && EntityType.VERTEX && ConstructionObject.NO && SketchObject.NO,
                            "MaxNumberOfPicks" : 1 ,
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
            annotation {"Name" : "Smooth transition"}
            definition.smoothTransition is boolean;
        }
    }
    {
        definition.allowEdgeOverflow = (definition.crossSection == FilletCrossSection.CURVATURE) ? true : definition.allowEdgeOverflow;

        if (!definition.isVariable)
        {
            try(addFilletManipulator(context, id, definition));
        }
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V575_SHEET_METAL_FILLET_CHAMFER))
        {
            sheetMetalAwareFillet(context, id, definition);
        }
        else
        {
            opFillet(context, id, definition);
        }

        if (!definition.defaultsChanged) //defaults did not change, suppress info if needed
        {
            var result = getFeatureStatus(context, id);
            if (result != undefined && result.statusEnum == ErrorStringEnum.VRFILLET_NO_EFFECT)
            {
                clearFeatureStatus(context, id, {"withDisplayData" : false}); //keep supplemental graphics
            }
        }

    }, { tangentPropagation : false, crossSection : FilletCrossSection.CIRCULAR, isVariable : false, smoothTransition : false, defaultsChanged : false, allowEdgeOverflow: true });


/**
 * @internal
 * Edit logic to check if a default value has been changed. So that we only display the info when needed
 */
export function filletEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    definition.defaultsChanged = false;
    if (specifiedParameters.radius  || specifiedParameters.rho || specifiedParameters.magnitude)
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
    var hasSheetMetalQueries = evaluateQuery(context, separatedQueries.sheetMetalQueries) != [];
    var hasNonSheetMetalQueries = evaluateQuery(context, separatedQueries.nonSheetMetalQueries) != [];

    if (!hasSheetMetalQueries && !hasNonSheetMetalQueries)
    {
        throw regenError(ErrorStringEnum.FILLET_SELECT_EDGES, ["entities"]);
    }

    if (hasSheetMetalQueries)
    {
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
        try(sheetMetalCornerBreak(context, id + "smFillet", cornerBreakDefinition));
        processSubfeatureStatus(context, id, {"subfeatureId" : id + "smFillet", "propagateErrorDisplay" : true});
        if (featureHasError(context, id))
            return;
    }

    if (hasNonSheetMetalQueries)
    {
        definition.entities = separatedQueries.nonSheetMetalQueries;
        opFillet(context, id, definition);
    }
}

const FILLET_RADIUS_MANIPULATOR = "filletRadiusManipulator";

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

            const offset = convexity * definition.radius * findRadiusToOffsetRatio(normals);

            addManipulators(context, id, {
                        (FILLET_RADIUS_MANIPULATOR) : linearManipulator({
                                    "base" : origin,
                                    "direction" : direction,
                                    "offset" : offset,
                                    "minValue" : minDragValue,
                                    "maxValue" : maxDragValue,
                                    "primaryParameterId" : "radius"
                                })
                    });
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
        if (newManipulators[FILLET_RADIUS_MANIPULATOR] is map)
        {
            // convert given offset and edge topology into new radius
            const operativeEntity = findManipulationEntity(context, definition);
            const origin = evEdgeTangentLine(context, { "edge" : operativeEntity, "parameter" : 0.5 }).origin;
            const normals = findSurfaceNormalsAtEdge(context, operativeEntity, origin);
            const convexity = isEdgeConvex(context, operativeEntity) ? -1.0 : 1.0;

            definition.radius = convexity * newManipulators[FILLET_RADIUS_MANIPULATOR].offset / findRadiusToOffsetRatio(normals);
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
        if (@size(evaluateQuery(context, qEntityFilter(operativeEntity, EntityType.FACE))) != 0)
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

