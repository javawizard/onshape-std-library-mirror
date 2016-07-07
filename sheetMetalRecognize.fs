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

/**
* @internal
*  This feature uses evOffsetDetection functionality to recognize sheet metal objects of thin bodies
*  and assign attributes to Model, Walls, Bends and sharp Joints . TODO : recognize Rips
*  Order of pairs in offsetGroup is deterministic hence processing order is deterministic
*  and indices can be used for attribute id generation
*/
annotation { "Feature Type Name" : "smRecognize" }
export const smRecognize = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Thin bodies", "Filter" : EntityType.BODY }
        definition.bodies is Query;

    }
    {
        clearSmAttributes(context, qOwnedByBody(definition.bodies));
        var offsetGroups = evOffsetDetection(context, definition);

        var objectCount = 0;
        for (var group in offsetGroups)
        {
            for (var i = 0; i < size(group.side0); i += 1)
            {
                objectCount = annotateWallsAndBends(context, id, group, i, objectCount);
            }
            for (var i = 0; i < size(group.side0); i += 1)
            {
                objectCount = annotateSharpJoints(context, id, group.side0[i], group.side1[i], objectCount);
            }
            annotateBody(context, id, qOwnerBody(group.side0[0]), [group.offsetLow, group.offsetHigh]);
        }
    });

/**
* planar offset pairs are marked as walls, cylindrical offset pairs are marked as bends
*/
function annotateWallsAndBends(context is Context, id is Id, group is OffsetGroup,
                index is number, objectCount is number) returns number
{
    var face0 = group.side0[index];
    var face1 = group.side1[index];
    var surface = evSurfaceDefinition(context, { "face" : face0 });

    var attributeId = toAttributeId(id + objectCount);
    var attribute;
    if (surface is Cylinder)
    {
        attribute = makeSMJointAttribute(attributeId, SMJointType.BEND);
        attribute.radius = {'value' : min(surface.radius, evSurfaceDefinition(context, { "face" : face1}).radius),
            'canBeEdited' : true };
    }
    else if (surface is Plane)//TODO  : more surface types
    {
        attribute = makeSMWallAttribute(attributeId);
    }
    else
    {
        throw "Unsupported surface type" ~ surface;
    }
    setAttribute(context, {
            "entities" : qUnion([face0, face1]),
            "attribute" : attribute
    });
    return objectCount + 1;
}

/**
* edges between two walls are marked as sharp joints
*/
function annotateSharpJoints(context is Context, id is Id, face0 is Query, face1 is Query, objectCount is number) returns number
{
    if (size(getSmObjectTypeAttributes(context, face0, SMObjectType.WALL)) == 0)
    {
        return objectCount;
    }
    var face0Neighbors = qEdgeAdjacent(face0, EntityType.FACE);
    var face1Neighbors = qEdgeAdjacent(face1, EntityType.FACE);
    var wallNeighbors0Q = qAttributeFilter(face0Neighbors, asSMAttribute({'objectType' : SMObjectType.WALL}));

    var  wallNeighbors0 = evaluateQuery(context, wallNeighbors0Q);
    for (var neighbor0 in wallNeighbors0)
    {
        var wallAttributes = getSmObjectTypeAttributes(context, neighbor0, SMObjectType.WALL);
        if (size(wallAttributes) != 1)
        {
            throw "Expected 1 wall attribute, got " ~ size(wallAttributes);
        }
        var neighbor1 = evaluateQuery(context, qAttributeFilter(face1Neighbors, wallAttributes[0]));
        if (size(neighbor1) != 1)
        {
            throw "Expected 1 face, got " ~ size(neighbor1);
        }
        var edgesQ = qUnion([commonEdgesQuery(neighbor0, face0), commonEdgesQuery(neighbor1[0], face1)]);
        if ( size(getAttributes(context, {
                "entities" : edgesQ,
                "attributePattern" : asSMAttribute({'objectType' : SMObjectType.JOINT})
                })) == 0)
        {
            var attributeId = toAttributeId(id + objectCount);
            objectCount += 1;
            var attribute = makeSMJointAttribute(attributeId, SMJointType.SHARP);
            attribute.angle = angleBetweenFaces(context, face0, neighbor0);
            setAttribute(context, {
                "entities" : edgesQ,
                "attribute" : attribute
                });
        }
    }
    return objectCount;
}

function angleBetweenFaces(context is Context, face0 is Query, face1 is Query)
{
    var plane0 = evPlane(context, { "face" : face0 });
    var plane1 = evPlane(context, { "face" : face1 });
    return angleBetween(plane0.normal, plane1.normal);
}

function commonEdgesQuery(face0 is Query, face1 is Query) returns Query
{
    return qIntersection([qEdgeAdjacent(face0, EntityType.EDGE), qEdgeAdjacent(face1, EntityType.EDGE)]);
}

function annotateBody(context is Context, id is Id, body is Query, offsetRange is array)
{
    // All bodies processed by this feature belong to the same model
    var attributeId = toAttributeId(id);
    var attribute = makeSMModelAttribute(attributeId);
    attribute.thickness = {'value' : 0.5 * (offsetRange[0] + offsetRange[1])};
    attribute.offsetTolerance = {'value' : 0.5 *  (offsetRange[1] - offsetRange[0])};
    setAttribute(context, {
            "entities" : body,
            "attribute" : attribute
    });
}

