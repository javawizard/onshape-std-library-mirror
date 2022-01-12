FeatureScript 1675; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */

export import(path : "onshape/std/smjointtype.gen.fs", version : "1675.0");
export import(path : "onshape/std/smjointstyle.gen.fs", version : "1675.0");

import(path : "onshape/std/sheetMetalAttribute.fs", version : "1675.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "1675.0");
import(path : "onshape/std/attributes.fs", version : "1675.0");
import(path : "onshape/std/feature.fs", version : "1675.0");
import(path : "onshape/std/containers.fs", version : "1675.0");
import(path : "onshape/std/string.fs", version : "1675.0");

/**
 * @internal
 */

annotation { "Feature Type Name" : "Unfold" }
export const sheetMetalUnfold = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Parts or bends",
                    "Filter" : ActiveSheetMetal.YES && (EntityType.BODY || SheetMetalDefinitionEntityType.EDGE) && ModifiableEntityOnly.YES }
        definition.toUnfold is Query;

    }
    {
        annotateBendsForUnfold(context, id, definition.toUnfold, true);
    }, {});

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

