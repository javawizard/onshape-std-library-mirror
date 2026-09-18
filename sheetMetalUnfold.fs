FeatureScript 3083; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */

import(path : "onshape/std/attributes.fs", version : "3083.0");
import(path : "onshape/std/containers.fs", version : "3083.0");
import(path : "onshape/std/feature.fs", version : "3083.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "3083.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "3083.0");

/**
 * Warning: This feature can not be used as a part of sheet metal workflow - sheet metal features following it will not work correctly.
 * sheetMetalUnfold feature modifies selected sheet metal bends and rolled walls by flattening them in the 3d view as well.
 */
annotation { "Feature Type Name" : "Unfold", "Editing Logic Function" : "unfoldEditLogic" }
export const sheetMetalUnfold = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Unfold all" , "Default" : true}
        definition.unfoldAll is boolean;

        if (definition.unfoldAll)
        {
            annotation { "Name" : "Sheet metal part", "Filter" : EntityType.BODY && ActiveSheetMetal.YES && ModifiableEntityOnly.YES, "MaxNumberOfPicks" : 1}
            definition.partToUnfold is Query;
        }
        else
        {
            annotation { "Name" : "Bend or non-planar faces",
                        "Filter" : EntityType.FACE && !GeometryType.PLANE && (SheetMetalDefinitionEntityType.EDGE || SheetMetalDefinitionEntityType.FACE) && ModifiableEntityOnly.YES}
            definition.entitiesToUnfold is Query;
        }

        annotation { "Name" : "Wall face or bounding edge to hold",
                    "Filter" : (SheetMetalDefinitionEntityType.EDGE || SheetMetalDefinitionEntityType.FACE) && ModifiableEntityOnly.YES, "MaxNumberOfPicks" : 1 }
        definition.holdEntity is Query;
    }
    {
        //this is not necessary but helps with correct error reporting in feature pattern
        checkNotInFeaturePattern(context, qUnion([definition.partToUnfold, definition.entitiesToUnfold, definition.holdEntity]), ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        var smEntsQ = qNothing();
        if (definition.unfoldAll)
        {
            if (isQueryEmpty(context, definition.partToUnfold))
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_UNFOLD_NO_PART_SELECTED, ["partToUnfold"]);
            }
            const planarFacesQ = definition.partToUnfold->qOwnedByBody(EntityType.FACE)->qGeometry(GeometryType.PLANE);
            const nonPlanarFacesQ = qSubtraction(definition.partToUnfold->qOwnedByBody(EntityType.FACE), planarFacesQ);
            smEntsQ = qUnion(getSMDefinitionEntities(context, nonPlanarFacesQ));
        }
        else
        {
            if (isQueryEmpty(context, definition.entitiesToUnfold))
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_UNFOLD_NO_BEND_OR_ROLLED_WALL_SELECTED, ["entitiesToUnfold"]);
            }
            if (evaluateQueryCount(context, definition.entitiesToUnfold->qOwnerBody()) > 1)
            {
                throw regenError(ErrorStringEnum.SHEET_METAL_UNFOLD_SELECT_FROM_SAME_PART, ["entitiesToUnfold"]);
            }
            smEntsQ = qUnion(getSMDefinitionEntities(context, definition.entitiesToUnfold));
        }
        if (isQueryEmpty(context, smEntsQ))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_UNFOLD_NOTHING_TO_UNFOLD, ["partToUnfold", "entitiesToUnfold"]);
        }

        if (isQueryEmpty(context, definition.holdEntity))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_UNFOLD_NO_HOLD_ENTITY, ["holdEntity"]);
        }
        const smHoldEnts = getSMDefinitionEntities(context, definition.holdEntity);
        const nSMHoldEnts = size(smHoldEnts);
        if (nSMHoldEnts == 0)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_UNFOLD_NO_HOLD_ENTITY, ["holdEntity"]);
        }
        else if (nSMHoldEnts > 1)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_UNFOLD_SINGLE_HOLD_ENTITY, ["holdEntity"]);
        }

        if (evaluateQueryCount(context, qUnion(definition.unfoldAll ? definition.partToUnfold : definition.entitiesToUnfold, definition.holdEntity)->qOwnerBody()) > 1)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_UNFOLD_HOLD_ENTITY_NOT_ON_PART, ["partToUnfold", "entitiesToUnfold", "holdEntity"]);
        }

        const bendsToUnfoldQ = smEntsQ->qEntityFilter(EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.TWO_SIDED);
        const planarFacesQ = smEntsQ->qEntityFilter(EntityType.FACE)->qGeometry(GeometryType.PLANE);
        const rolledWallsToUnfoldQ = qSubtraction(smEntsQ->qEntityFilter(EntityType.FACE), planarFacesQ);
        const toUnfoldQ = qUnion(bendsToUnfoldQ, rolledWallsToUnfoldQ);

        var affected = evaluateQuery(context, toUnfoldQ);
        if (affected == [])
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_UNFOLD_NOTHING_TO_UNFOLD, ["partToUnfold", "entitiesToUnfold"]);
        }

        var unfolded = [];
        for (var smEnt in affected)
        {
            if (setEntityAsUnfolded(context, id, smEnt, definition.unfoldAll))
            {
                unfolded = append(unfolded, smEnt);
            }
        }
        if (unfolded == [])
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_UNFOLD_NOTHING_TO_UNFOLD, ["partToUnfold", "entitiesToUnfold"]);
        }

        setEntityAsHeldForUnfold(context, definition.unfoldAll ? definition.partToUnfold : definition.entitiesToUnfold->qOwnerBody(), smHoldEnts[0]);

        updateSheetMetalGeometry(context, id, { "entities" : qUnion(unfolded),
                    "associatedChanges" : qUnion(unfolded)
                });
    }, {});

function setEntityAsUnfolded(context is Context, id is Id, entity is Query, unfoldAll is boolean) returns boolean
{
    var existingAttribute = getWallAttribute(context, entity);
    if (existingAttribute == undefined)
    {
        existingAttribute = getJointAttribute(context, entity);
        if (existingAttribute == undefined)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_UNFOLD_ENTITY_NOT_WALL_OR_JOINT, ["entitiesToUnfold"], entity);
        }
        if (existingAttribute.jointType == undefined)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_UNFOLD_JOINT_NO_TYPE, ["entitiesToUnfold"], entity);
        }
        if (existingAttribute.jointType.value == SMJointType.RIP)
        {
            if (!unfoldAll)
            {
                reportFeatureWarning(context, id, "A joint selected to unfold is a rip, not a bend");
            }
            return false;
        }
        if (existingAttribute.jointType.value != SMJointType.BEND)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_UNFOLD_JOINT_NOT_A_BEND, ["entitiesToUnfold"], entity);
        }
    }
    if (existingAttribute.unfolded == true)
    {
        if (!unfoldAll)
        {
            reportFeatureWarning(context, id, "Entity to unfold is already marked as unfolded");
        }
        return false;
    }
    else
    {
        removeAttributes(context, {
                    "entities" : entity,
                    "attributePattern" : asSMAttribute({})
                });
        existingAttribute.unfolded = true;
        setAttribute(context, {
                    "entities" : entity,
                    "attribute" : existingAttribute
                });
        return true;
    }
}

function setEntityAsHeldForUnfold(context is Context, partToUnfold is Query, entity is Query)
{
    const existingHeldEntity = getEntityHeldForUnfold(context, partToUnfold);
    if (isQueryEmpty(context, qSubtraction(entity, existingHeldEntity)))
    {
        return;
    }

    if (!isQueryEmpty(context, existingHeldEntity->qEntityFilter(EntityType.FACE)))
    {
        var wallAttribute = getWallAttribute(context, existingHeldEntity);
        removeAttributes(context, {
            "entities" : existingHeldEntity,
            "attributePattern" : asSMAttribute({})
        });
        wallAttribute.holdFaceForUnfold = undefined;
        setAttribute(context, {
                "entities" : existingHeldEntity,
                "attribute" : wallAttribute
        });
    }
    else if (!isQueryEmpty(context, existingHeldEntity->qEntityFilter(EntityType.EDGE)))
    {
        setAttribute(context, {
                "entities" : existingHeldEntity,
                "name" : "holdEdgeForUnfold",
                "attribute" : undefined
        });
    }

    var wallAttribute = getWallAttribute(context, entity);
    if (wallAttribute == undefined)
    {
        if (isQueryEmpty(context, entity->qEntityFilter(EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.ONE_SIDED)) ||
            getWallAttribute(context, entity->qAdjacent(AdjacencyType.EDGE, EntityType.FACE)) == undefined)
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_UNFOLD_HOLD_ENTITY_NOT_WALL_OR_BOUNDARY_EDGE, ["holdEntity"]);
        }
        setAttribute(context, {
                "entities" : entity,
                "name" : "holdEdgeForUnfold",
                "attribute" : true
        });
    }
    else
    {
        removeAttributes(context, {
            "entities" : entity,
            "attributePattern" : asSMAttribute({})
        });
        wallAttribute.holdFaceForUnfold = true;
        setAttribute(context, {
                "entities" : entity,
                "attribute" : wallAttribute
        });
    }
}

function getEntityHeldForUnfold(context is Context, partToUnfold is Query) returns Query
{
    const smFacesQ = qUnion(getSMDefinitionEntities(context, partToUnfold->qOwnedByBody(EntityType.FACE), EntityType.FACE));
    for (var smFace in evaluateQuery(context, smFacesQ))
    {
        const wallAttribute = getWallAttribute(context, smFace);
        if (wallAttribute != undefined &&
            wallAttribute.holdFaceForUnfold != undefined &&
            wallAttribute.holdFaceForUnfold == true)
        {
            return smFace;
        }
        for (var smEdge in evaluateQuery(context, smFace->qAdjacent(AdjacencyType.EDGE, EntityType.EDGE)->qEdgeTopologyFilter(EdgeTopology.ONE_SIDED)))
        {
            if (getAttribute(context, {"entity" : smEdge, "name" : "holdEdgeForUnfold"}) == true)
            {
                return smEdge;
            }
        }
    }
    return qNothing();
}

/**
 * @internal
 * If user hasn't selected a hold entity explicitly, and if the part has a hold entity, from a previous Unfold feature, put it into hold entity for this feature
 */
export function unfoldEditLogic(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean) returns map
{
    if ((oldDefinition.holdEntity == undefined || isQueryEmpty(context, oldDefinition.holdEntity)) && isQueryEmpty(context, definition.holdEntity))
    {
        const entityHeldForUnfold = getEntityHeldForUnfold(context, definition.unfoldAll ? definition.partToUnfold : definition.entitiesToUnfold->qOwnerBody());
        const partEntitiesHeldForUnfoldQ = getSMCorrespondingInPart(context, entityHeldForUnfold,
                                                                    isQueryEmpty(context, entityHeldForUnfold->qEntityFilter(EntityType.FACE)) ? EntityType.EDGE : EntityType.FACE);
        const partEntitiesHeldForUnfold = evaluateQuery(context, partEntitiesHeldForUnfoldQ);
        if (size(partEntitiesHeldForUnfold) == 2)
        {
            definition.holdEntity = partEntitiesHeldForUnfold[0];
        }
    }

    return definition;
}

/**
 * @internal
 */

annotation { "Feature Type Name" : "Refold" }
export const sheetMetalRefold = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Parts or bends",
                    "Filter" : ActiveSheetMetal.YES && (EntityType.BODY || SheetMetalDefinitionEntityType.EDGE) && ModifiableEntityOnly.YES }
        definition.toRefold is Query;

    }
    {
        annotateBendsForUnfold(context, id, definition.toRefold, false);
    }, {});

function annotateBendsForUnfold(context is Context, id is Id, bodiesOrBends is Query, setUnfolded is boolean)
{
    if (!areEntitiesFromSingleActiveSheetMetalModel(context, bodiesOrBends))
    {
        throw regenError(ErrorStringEnum.SHEET_METAL_SINGLE_MODEL_NEEDED, ["toRefold"]);
    }

    var jointEntities = [];
    var countNonBendSelections = 0;
    var smEntities = qUnion(getSMDefinitionEntities(context, bodiesOrBends));
    var smNonBodies = qSubtraction(smEntities, qEntityFilter(smEntities, EntityType.BODY));
    for (var smEntity in evaluateQuery(context, smNonBodies))
    {
        var attributes = getSmObjectTypeAttributes(context, smEntity, SMObjectType.JOINT);
        if (size(attributes) != 1 || attributes[0].jointType.value != SMJointType.BEND)
        {
            countNonBendSelections += 1;
            continue;
        }
        if (attributes[0].unfolded == setUnfolded)
        {
            continue;
        }
        jointEntities = append(jointEntities, smEntity);
        var newAttribute = attributes[0];
        newAttribute.unfolded = setUnfolded;
        replaceSMAttribute(context, attributes[0], newAttribute);
    }
    if (countNonBendSelections > 0)
    {
        reportFeatureWarning(context, id, "Some non-bends among selections");
    }
    // sheet metal bodies that did not have any faces/edges selected get fully unfolded
    var smBodies = qSubtraction(qOwnerBody(smEntities), qOwnerBody(smNonBodies));
    var edgesOrCylinders = qUnion([qOwnedByBody(smBodies, EntityType.EDGE), qGeometry(qOwnedByBody(smBodies, EntityType.FACE), GeometryType.CYLINDER)]);
    for (var smEntity in evaluateQuery(context, edgesOrCylinders))
    {
        var attributes = getSmObjectTypeAttributes(context, smEntity, SMObjectType.JOINT);
        if (size(attributes) != 1 ||
            attributes[0].jointType.value != SMJointType.BEND ||
            attributes[0].unfolded == setUnfolded)
        {
            continue;
        }
        jointEntities = append(jointEntities, smEntity);
        var newAttribute = attributes[0];
        newAttribute.unfolded = setUnfolded;
        replaceSMAttribute(context, attributes[0], newAttribute);
    }
    updateSheetMetalGeometry(context, id, { "entities" : qUnion(jointEntities) });
}

