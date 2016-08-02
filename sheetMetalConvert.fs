FeatureScript 392; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */


import(path : "onshape/std/query.fs", version : "392.0");
import(path : "onshape/std/feature.fs", version : "392.0");
import(path : "onshape/std/valueBounds.fs", version : "392.0");
import(path : "onshape/std/geomOperations.fs", version : "392.0");
import(path : "onshape/std/containers.fs", version : "392.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "392.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "392.0");
import(path : "onshape/std/attributes.fs", version : "392.0");
import(path : "onshape/std/evaluate.fs", version : "392.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "392.0");
import(path : "onshape/std/vector.fs",  version : "392.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "392.0");
import(path : "onshape/std/string.fs", version : "392.0");

/**
 * @internal
 */
annotation { "Feature Type Name" : "smConvert" }
export const smConvert = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Faces to Exclude", "Filter" : EntityType.FACE && BodyType.SOLID}
        definition.faces is Query;

        annotation { "Name" : "Bends", "Filter" : EntityType.EDGE && EdgeTopology.TWO_SIDED && GeometryType.LINE && BodyType.SOLID }
        definition.bends is Query;

        annotation { "Name" : "Thickness" }
        isLength(definition.thickness, SHELL_OFFSET_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Clearance" }
        isLength(definition.clearance, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

        annotation { "Name" : "Bends Included in Clearance"}
        definition.bendsIncluded is boolean;


        annotation { "Name" : "Bend Radius"}
        isLength(definition.radius, BLEND_BOUNDS);

        annotation { "Name" : "Keep Input Parts" }
        definition.keepInputParts is boolean;

    }
    {
        startSheetMetalFeature(context, id);
        var faceBodyQuery = qOwnerBody(definition.faces);
        var edgesOnUnknownBodies = evaluateQuery(context, qSubtraction(qOwnerBody(definition.bends), faceBodyQuery));
        if (size(edgesOnUnknownBodies) > 0)
        {
            reportFeatureWarning(context, id, "Edges should come from the same bodies as faces");
        }

        var complimentFacesQ = qSubtraction(qOwnedByBody(faceBodyQuery, EntityType.FACE), definition.faces);
        var surfaceId = id + "extractSurface";
        var bendEdgesQ = startTracking(context, {"subquery" : definition.bends});
        var offset = computeSurfaceOffset(context, definition);
        opExtractSurface(context, surfaceId, {
                "faces" : complimentFacesQ,
                "offset" : offset});

        annotateSmSurfaceBodies(context, id, {
            "surfaceBodies" : qCreatedBy(surfaceId, EntityType.BODY),
            "bendEdges" : bendEdgesQ,
            "specialRadiiBends" : [],
            "defaultRadius" : definition.radius,
            "controllsThickness" : true,
            "thickness" : definition.thickness}, 0);
        if (!definition.keepInputParts)
        {
            opDeleteBodies(context, id + "deleteBodies", {
                    "entities" : faceBodyQuery
            });
        }
        endSheetMetalFeature(context, id, {"entities" : qUnion([qCreatedBy(surfaceId, EntityType.FACE), qCreatedBy(surfaceId, EntityType.EDGE)])});
    });

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
                var adjacentWalls = qSubtraction(qEdgeAdjacent(edge, EntityType.FACE), definition.faces);
                if (size(evaluateQuery(context, adjacentWalls)) == 0)
                {
                    continue;
                }
                var convexity = evEdgeConvexity(context, {"edge" : edge});
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
    var offset =  0.5 * definition.thickness + wallClearance;
    if (definition.oppositeDirection)
    {
        offset = - offset;
    }
    return offset;
}

