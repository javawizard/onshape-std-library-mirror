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

import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/tool.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");
import(path : "onshape/std/error.fs", version : "✨");
import(path : "onshape/std/geomOperations.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/modifyFillet.fs", version : "✨");

/**
* @internal
*  This feature uses evOffsetDetection functionality to recognize sheet metal body,
*  extracts definition sheet surface, replaces cylinders with sharp edges, when possible.
*  Sheet body is annotated as Model, planar faces are annotated as Walls,
*  cylinders or sharp edges replacing them are annotated as Bends preserving original radius,
*  Original sharp edges are annotated as Bends of input radius. TODO : recognize Rips.
*/
annotation { "Feature Type Name" : "smRecognize" }
export const smRecognize = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Thin bodies", "Filter" : EntityType.BODY }
        definition.bodies is Query;

        annotation { "Name" : "Keep Input Parts", "Default" : false }
        definition.keepInputParts is boolean;

        annotation { "Name" : "Bend Radius" }
        isLength(definition.radius, BLEND_BOUNDS);

        annotation { "Name" : "Change Thickness", "Default" : false }
        definition.changeThickness is boolean;

        if (definition.changeThickness)
        {
            annotation { "Name" : "Thickness" }
            isLength(definition.thickness, SHELL_OFFSET_BOUNDS);

        }
    }
    {
        startSheetMetalFeature(context, id);
        var asociationAttributes = getAttributes(context, {
                "entities" : definition.bodies,
                "attributePattern" : {} as SMAssociationAttribute
        });
        if (size(asociationAttributes) != 0)
        {
            regenError("Sheet Metal body is selected for recognition", ["bodies"]);
        }
        var offsetGroups = evOffsetDetection(context, definition);

        if (size(offsetGroups) != size(evaluateQuery(context, definition.bodies)))
        {
            //TODO: - actually a group per body check
            regenError("Selected bodies can not be recognized as sheet metal");
        }

        var objectCount = 0;
        var groupCount = 0;
        var smFacesAndEdgesQ = qNothing();
        for (var group in offsetGroups)
        {
            var surfaceId = id + ("surface_" ~  groupCount);
            var surfaceData = makeSurfaceBody(context, surfaceId, group);
            surfaceData.defaultRadius = definition.radius;
            surfaceData.controllsThickness = definition.changeThickness;
            if (definition.changeThickness)
            {
                surfaceData.thickness = definition.thickness;
            }
            groupCount += 1;
            smFacesAndEdgesQ = qUnion([smFacesAndEdgesQ, qCreatedBy(surfaceId, EntityType.FACE), qCreatedBy(surfaceId, EntityType.EDGE)]);
            objectCount = annotateSmSurfaceBodies(context, id, surfaceData, objectCount);
        }
        if (!definition.keepInputParts)
        {
            opDeleteBodies(context, id + "deleteBodies", {
                    "entities" : definition.bodies
            });
        }
        endSheetMetalFeature(context, id, {"entities" : smFacesAndEdgesQ});
    }, {"keepInputParts" : false, "changeThickness" : false});

function makeSurfaceBody(context is Context, id is Id, group is map)
{
    var out = {"thickness" : 0.5 * (group.offsetLow + group.offsetHigh)};
    opExtractSurface(context, id, {
                "faces" : qUnion(group.side0),
                "offset" : -0.5 * out.thickness
            });
    var srfBodies = evaluateQuery(context, qCreatedBy(id, EntityType.BODY));
    if (size(srfBodies) != 1)
    {
        regenError("Unexpected number of surfaces extracted");
    }
    out.surfaceBodies = srfBodies[0];
    //Collect sharp edges to mark them as default radius bends
    var sharpEdges = [];
    for (var edge in evaluateQuery(context, qOwnedByBody(out.surfaceBodies, EntityType.EDGE)))
    {
        if (!edgeIsTwoSided(context, edge))
        {
            continue;
        }
        var convexity = evEdgeConvexity(context, {"edge" : edge});
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
        try {
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

function edgeIsTwoSided(context is Context, edge is Query) returns boolean
{
    return size(evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE))) == 2;
}


