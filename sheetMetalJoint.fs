FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */


export import(path : "onshape/std/smjointtype.gen.fs", version : "✨");
export import(path : "onshape/std/smjointstyle.gen.fs", version : "✨");

import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");
import(path : "onshape/std/modifyFillet.fs", version : "✨");

/**
 * @internal
 */
annotation { "Feature Type Name" : "smJoint" }
export const smJoint = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "entity", "Filter" : EntityType.FACE || EntityType.EDGE, "MaxNumberOfPicks" : 1 }
        definition.entity is Query;

        annotation { "Name" : "Joint type" }
        definition.jointType is SMJointType;

        if (definition.jointType == SMJointType.BEND)
        {
            annotation { "Name" : "Radius" }
            isLength(definition.radius, BLEND_BOUNDS);
        }

        if (definition.jointType == SMJointType.RIP)
        {
            annotation { "Name" : "Joint style"}
            definition.jointStyle is SMJointStyle;
        }
    }
    {
        var modelAttributes = getSmObjectTypeAttributes(context,  qOwnerBody(definition.entity), SMObjectType.MODEL);
        if (size(modelAttributes) != 1)
        {
            throw "Selected entity does not belong to a recognized sheet metal model";
        }

        if (!(modelAttributes[0].thickness is map) || !isLength(modelAttributes[0].thickness.value))
        {
            throw "Thickness is not defined for sheet metal model";
        }
        definition.thickness = modelAttributes[0].thickness.value;
        var jointData = getJointData(context, definition);


        removeAttributes(context, {
            "entities" : jointData.jointTopology,
            "attributePattern" : asSMAttribute({})
        });


        if (definition.jointType == jointData.jointType)
        {
            jointData.result = modifyJoint(context, id, jointData, definition);
        }
        else
        {
            jointData.result = changeJointType(context, id, jointData, definition);
        }
        if (jointData.result != undefined)
        {
            assignJointAttribute(context, id, jointData, definition);
        }
        else if (definition.jointType == jointData.jointType)
        {
            throw "Modifying " ~ jointData.jointType ~ "is not implemented";
        }
        else
        {
            throw "Changing " ~  jointData.jointType ~ " to " ~ definition.jointType ~ " is not implemented";
        }
    });

    /**
     * @internal
     * @param arg {{
     * @field entity {Query}
     * @field thickness {ValueWithUnits}
     * }}
     */
    export function getJointData(context is Context, arg is map) returns map
    {
        var attributes =  getSmObjectTypeAttributes(context, arg.entity, SMObjectType.JOINT);
        if (size(attributes) != 1)
        {
            throw "Selected entity does not belong to a recognized sheet metal joint";
        }
        var out = attributes[0];
        out.jointTopology = qUnion(evaluateQuery(context, qAttributeQuery(asSMAttribute({'attributeId' : out.attributeId}))));
        if (out.jointType == SMJointType.BEND)
        {
            var bendFaces = evaluateQuery(context, qGeometry(out.jointTopology, GeometryType.CYLINDER));
            var nCylinders = size(bendFaces);
            if (nCylinders < 2)
            {
                throw "Bend is expected to have 2 or more cylindrical faces, found " ~ nCylinders;
            }
            out.radius = out.radius.value;
            out.outerRadius = out.radius + arg.thickness;
            out.innerFaces = [];
            out.outerFaces = [];
            for (var f in bendFaces)
            {
                var cylinder = evSurfaceDefinition(context, { "face" : f}) as Cylinder;
                if (abs(cylinder.radius - out.radius) < abs(cylinder.radius - out.outerRadius))
                {
                    out.innerFaces = append(out.innerFaces, f);
                }
                else
                {
                    out.outerFaces = append(out.outerFaces, f);
                }
            }
            if (size(out.innerFaces) == 0)
            {
                throw "No inner faces found for bend";
            }
            if (size(out.outerFaces) == 0)
            {
                throw "No outer faces found for bend";
            }
        }
        else if (out.jointType == SMJointType.SHARP)
        {
            var sharpEdges = evaluateQuery(context, qEntityFilter(arg.entity, EntityType.EDGE));
            var nEdges = size(sharpEdges);
            if (nEdges < 2)
            {
                throw "Sharp joint is expected to have at least two edges, found " ~ nEdges;
            }
            out.outerEdges = [];
            out.innterEdges = [];
            for (var edge in sharpEdges)
            {
                var convexity = evEdgeConvexity(context, {"edge" : edge });
                if (convexity == EdgeConvexityType.CONVEX)
                {
                    out.outerEdges = append(out.outerEdges, edge);
                }
                else if (convexity == EdgeConvexityType.CONCAVE)
                {
                    out.innerEdges = append(out.innerEdges, edge);
                }
            }
            if (size(out.outerEdges) == 0)
            {
                throw "No outer edges in sharp joint";
            }
            if (size(out.innerEdges) == 0)
            {
                throw "No inner edges in sharp joint";
            }
        }
        else
        {
            throw "Not implemented for " ~ out.jointType;
        }
        return out;
    }

    function modifyJoint(context is Context, id is Id, jointData is map, definition is map)
    {
        if (jointData.jointType == SMJointType.BEND)
        {
            var trackingQuery = startTracking(context, jointData.jointTopology);
            modifyBend(context, id, jointData, definition);
            return trackingQuery;
        }
        return undefined;
    }

    function modifyBend(context is Context, id is Id, jointData is map, definition is map)
    {
        if (definition.radius < jointData.radius) //outer, then inner
        {
            opModifyFillet(context, id + "outer", {
                    "faces" : qUnion(jointData.outerFaces),
                    "modifyFilletType" : ModifyFilletType.CHANGE_RADIUS,
                    "radius" : definition.radius + definition.thickness
            });

            opModifyFillet(context, id + "inner", {
                    "faces" : qUnion(jointData.innerFaces),
                    "modifyFilletType" : ModifyFilletType.CHANGE_RADIUS,
                    "radius" : definition.radius
            });
        }
        else // inner, then outer
        {
            opModifyFillet(context, id + "inner", {
                    "faces" : qUnion(jointData.innerFaces),
                    "modifyFilletType" : ModifyFilletType.CHANGE_RADIUS,
                    "radius" : definition.radius
            });

            opModifyFillet(context, id + "outer", {
                    "faces" : qUnion(jointData.outerFaces),
                    "modifyFilletType" : ModifyFilletType.CHANGE_RADIUS,
                    "radius" : definition.radius + definition.thickness
            });
        }
    }

    function changeJointType(context is Context, id is Id, jointData is map, definition is map) returns boolean
    {
        return false;
    }

    function assignJointAttribute(context is Context, id is Id, jointData is map, definition is map)
    {
       var attribute = makeSMJointAttribute(jointData.attributeId, definition.jointType);
       if (definition.jointType == SMJointType.BEND)
       {
           attribute.radius = {'value' : definition.radius,
                        'controllingFeatureId' : toAttributeId(id),
                        'parameterIdInFeature' : "radius",
                        'canBeEdited' : true};
       }
       else
       {
           throw "Not implemented for joint type " ~ definition.jointType;
       }
       setAttribute(context, {
               "entities" : jointData.result,
               "attribute" : attribute
       });
    }
