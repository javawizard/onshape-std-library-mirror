FeatureScript 464; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */


import(path : "onshape/std/attributes.fs", version : "464.0");
import(path : "onshape/std/boolean.fs", version : "464.0");
import(path : "onshape/std/containers.fs", version : "464.0");
import(path : "onshape/std/error.fs", version : "464.0");
import(path : "onshape/std/feature.fs", version : "464.0");
import(path : "onshape/std/evaluate.fs", version : "464.0");
import(path : "onshape/std/extendsheetboundingtype.gen.fs", version : "464.0");
import(path : "onshape/std/geomOperations.fs", version : "464.0");
import(path : "onshape/std/query.fs", version : "464.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "464.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "464.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "464.0");
import(path : "onshape/std/topologyUtils.fs", version : "464.0");
import(path : "onshape/std/units.fs", version : "464.0");

/**
* @internal
* same as SMJointStyle but missing Flat joint
*/
export enum SMJointRipStyle
{
    annotation {"Name" : "Edge Joint"}
    EDGE,
    annotation {"Name" : "Butt Joint - Primary"}
    BUTT,
    annotation {"Name" : "Butt Joint - Secondary"}
    BUTT2
}

const ripStyleMap = { SMJointRipStyle.EDGE  : SMJointStyle.EDGE,
                      SMJointRipStyle.BUTT  : SMJointStyle.BUTT,
                      SMJointRipStyle.BUTT2 : SMJointStyle.BUTT2 };

/**
* @internal
*  TODO : This feature produces a sheet metal rip
*/
annotation { "Feature Type Name" : "Make joint", "Filter Selector" : "allparts"  }
export const sheetMetalMakeJoint = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation {"Name" : "Edges to join as rip",
                    "Filter" : EntityType.EDGE && BodyType.SOLID && ModifiableEntityOnly.YES,
                    "MaxNumberOfPicks" : 2}
        definition.entities is Query;

        annotation {"Name" : "Joint style"}
        definition.jointType is SMJointRipStyle;
    }
    {
        //this is not necessary but helps with correct error reporting in feature pattern
        checkNotInFeaturePattern(context, definition.entities);

        //entities should be from the same sm model, but can be from different parts
        if (!areEntitiesFromSingleActiveSheetMetalModel(context, definition.entities))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_JOIN_NEEDED, ["entities"]);
        }

        var smEntities = qUnion(getSMDefinitionEntities(context, definition.entities));
        createEdgeJoint(context, id, smEntities, definition.jointType);
    }, {jointType : SMJointRipStyle.EDGE}
    );


function createEdgeJoint(context is Context, id is Id, smEntities is Query, jointType is SMJointRipStyle)
{
    var edges = evaluateQuery(context, smEntities);
    var faces = [];

    //For selected edges, we need to get to the wall they're on.
    for (var edge in edges)
    {
        var adjacentFaces = evaluateQuery(context, qEdgeAdjacent(edge, EntityType.FACE));
        if (size(adjacentFaces) != 1)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_RIP_FAIL_INTERNAL_EDGE, ["entities"]);
        }
        faces = append(faces, adjacentFaces[0]);
    }
    if (size(faces) != 2)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_MAKE_JOINT_FAIL, ["entities"]);
    }

    var originalEdges = startTracking(context, smEntities);
    var plane1 = try(evPlane(context, {"face" : faces[0]}));
    var plane2 = try(evPlane(context, {"face" : faces[1]}));

    if (plane1 == undefined || plane2 == undefined)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_RIP_FAIL_NON_PLANAR, ["entities"]);
    }


    //get originals before any changes
    var smBodies = evaluateQuery(context, qOwnerBody(smEntities));
    if (size(smBodies) > 1)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_RIP_MULTI_BODY, ["entities"]);
    }

    var smBodiesQ = qUnion(smBodies);
    var initialAssociationAttributes = getAttributes(context, {
        "entities" : qOwnedByBody(smBodiesQ),
        "attributePattern" : {} as SMAssociationAttribute
    });
    var allOriginalEntities = evaluateQuery(context, qOwnedByBody(smBodiesQ));


    var intersectionData = intersection(plane1, plane2);
    if (intersectionData == undefined)
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_MAKE_JOINT_FAIL, ["entities"]);
    }

    // Both walls are being extended/trimmed to the bisector plane.
    var planeToExtendTo = plane(intersectionData.origin, plane2.normal - plane1.normal);

    try
    {
        opExtendSheetBody(context, id + "extend", {
            "extendMethod" : ExtendSheetBoundingType.EXTEND_TO_SURFACE,
            "offset" : 0 * inch,
            "entities" : qUnion([edges[0], edges[1]]),
            "limitEntity" : planeToExtendTo
        });
        if (getFeatureError(context, id + "extend") != undefined)
            throw regenError(ErrorStringEnum.SHEET_METAL_MAKE_JOINT_FAIL, ["entities"]);
    }
    catch
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_MAKE_JOINT_FAIL, ["entities"]);
    }



    var count = 0;
    for (var resultingEdge in evaluateQuery(context, originalEdges))
    {
        if (edgeIsTwoSided(context, resultingEdge))
        {
            count += 1;
            addRipAttribute(context, originalEdges, toAttributeId(id), ripStyleMap[jointType], undefined);
        }
    }
    if (count > 1)
    {
        //we should have only one new rip edge
        throw regenError(ErrorStringEnum.SHEET_METAL_MAKE_JOINT_FAIL, ["entities"]);
    }

    // Add association attributes where needed and compute deleted attributes
    var toUpdate = assignSMAttributesToNewOrSplitEntities(context, smBodiesQ, allOriginalEntities, initialAssociationAttributes);
    updateSheetMetalGeometry(context, id, { "entities" : toUpdate.modifiedEntities,
                                           "deletedAttributes" : toUpdate.deletedAttributes});
}


