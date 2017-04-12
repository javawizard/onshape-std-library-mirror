FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

export import(path : "onshape/std/smcornerbreakstyle.gen.fs", version : "✨");

import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/units.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");

/**
 *  @internal
 */
annotation { "Feature Type Name" : "Corner break", "Filter Selector" : "allparts" }
export const sheetMetalCornerBreak = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Corners to break",
                     "Filter" : (EntityType.EDGE || EntityType.VERTEX) && AllowEdgePoint.NO }
        definition.entities is Query;

        annotation { "Name" : "Corner break style", "Default" : SMCornerBreakStyle.FILLET }
        definition.cornerBreakStyle is SMCornerBreakStyle;

        annotation { "Name" : "Range" }
        isLength(definition.range, BLEND_BOUNDS);
    }
    {
        var entityArray = evaluateQuery(context, definition.entities);

        var definitionVertices = getDefinitionVertices(context, entityArray);
        var wallIds = getWallIds(context, entityArray);

        applyCornerBreaks(context, id, entityArray, definitionVertices, wallIds, definition.cornerBreakStyle, definition.range);

        var changedEntities = qUnion(definitionVertices);
        updateSheetMetalGeometry(context, id, { "entities" : changedEntities, "associatedChanges" : changedEntities });
    }, {});

/**
 * Get the underlying sheet metal vertices from the supplied entities. Throw an informative error if the supplied entities
 * are not from a sheet metal model, or the underlying sheet metal entities do not refer to vertices.
 */
function getDefinitionVertices(context is Context, entityArray is array) returns array
{
    var errorEntities = [];

    // Process entities individually or we will miss edges that map to the same vertex, but belong to different walls.
    var definitionEntities = [];
    for (var entity in entityArray)
    {
        var definitionEntity = getSMDefinitionEntities(context, entity);
        if (size(definitionEntity) == 1)
        {
            definitionEntities = append(definitionEntities, definitionEntity[0]);
        }
        else
        {
            errorEntities = append(errorEntities, entity);
        }
    }
    if (size(errorEntities) != 0)
    {
        errorEntities = qUnion(errorEntities);
        throw regenError("Corner Break entities must be from an active sheet metal model", errorEntities);
    }

    // Make sure every definitionEntity is a vertex.
    var definitionVerticesQuery = qEntityFilter(qUnion(definitionEntities), EntityType.VERTEX);
    var definitionNonVertices = evaluateQuery(context, qSubtraction(qUnion(definitionEntities), definitionVerticesQuery));
    if (size(definitionNonVertices) != 0)
    {
        var errorEntities = [];
        for (var i = 0; i < size(definitionEntities); i += 1)
        {
            if (size(evaluateQuery(context, qEntityFilter(definitionEntities[i], EntityType.VERTEX))) == 0)
            {
                errorEntities = append(errorEntities, entityArray[i]);
            }
        }
        errorEntities = qUnion(errorEntities);

        throw regenError("Selected sheet metal entities do not specify a corner", errorEntities);
    }

    return definitionEntities;
}

/**
 * Get the underlying sheet metals walls associated with the supplied entities.  Throw an informative error if the supplied
 * entities have more than one associated wall.
 */
function getWallIds(context is Context, entityArray is array) returns array
{
    var wallIds = [];
    var errorEntities = [];
    for (var entity in entityArray)
    {
        var adjacentFaceAssociations = getSMDefinitionEntities(context, qVertexAdjacent(entity, EntityType.FACE));

        var associatedWall = evaluateQuery(context, qEntityFilter(qUnion(adjacentFaceAssociations), EntityType.FACE));
        var associatedEdges = evaluateQuery(context, qEntityFilter(qUnion(adjacentFaceAssociations), EntityType.EDGE));

        var oneAssociatedWall = size(associatedWall) == 1;
        var twoAssociatedEdges = size(associatedEdges) == 2;
        if (!oneAssociatedWall || !twoAssociatedEdges)
        {
            errorEntities = append(errorEntities, entity);
            continue;
        }

        var edgesAreBlankOrRips = true;
        for (var edge in associatedEdges)
        {
            var jointAttribute = getJointAttribute(context, edge);
            if (jointAttribute != undefined)
            {
                if (jointAttribute.jointType.value == SMJointType.BEND)
                {
                    edgesAreBlankOrRips = false;
                    break;
                }
            }
        }
        if (!edgesAreBlankOrRips)
        {
            errorEntities = append(errorEntities, entity);
            continue;
        }

        var wallAttribute = getWallAttribute(context, associatedWall[0]);
        if (wallAttribute == undefined || wallAttribute.attributeId == undefined)
        {
            throw regenError("SHEET_METAL_ACTIVE_WALL_NEEDED");
        }
        wallIds = append(wallIds, wallAttribute.attributeId);
    }

    if (size(errorEntities) != 0)
    {
        throw regenError("Cannot apply a corner break to a bend or corner.", qUnion(errorEntities));
    }
    return wallIds;
}

/**
 *
 */
function applyCornerBreaks(context is Context, id is Id, entityArray is array, definitionVertices is array,
        wallIds is array, cornerBreakStyle is SMCornerBreakStyle, range is ValueWithUnits)
{
    var numEntities = size(entityArray);
    var errorEntities = [];
    for (var i = 0; i < numEntities; i += 1)
    {
        var existingAttribute = getCornerAttribute(context, definitionVertices[i]);
        var hasExistingAttribute = (existingAttribute != undefined);
        if (hasExistingAttribute && (existingAttribute.cornerStyle != undefined))
        {
            errorEntities = append(errorEntities, entityArray[i]);
            continue;
        }

        var newAttribute = hasExistingAttribute ? existingAttribute : makeSMCornerAttribute(toAttributeId(id + ("" ~ i)));
        var cornerBreak = makeSMCornerBreak(cornerBreakStyle, range, wallIds[i]);
        var cornerBreakMap = { "value" : cornerBreak, "canBeEdited" : false, "controllingFeatureId" : id };
        newAttribute = addCornerBreakToSMAttribute(newAttribute, cornerBreakMap);

        if (hasExistingAttribute)
        {
            replaceSMAttribute(context, existingAttribute, newAttribute);
        }
        else
        {
            setAttribute(context, { "entities" : definitionVertices[i], "attribute" : newAttribute });
        }
    }

    if (size(errorEntities) != 0)
    {
        throw regenError("Cannot apply a corner break to a existing corner.", qUnion(errorEntities));
    }
}

