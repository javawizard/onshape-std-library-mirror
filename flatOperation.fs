FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Under development, internal use only
// Imports used in interface
export import(path : "onshape/std/query.fs", version : "✨");

import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");

/**
 * @internal
 * Feature performing an [opSMFlatOperation]
 */
annotation { "Feature Type Name" : "SM flat operation" }
export const SMFlatOperation = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Faces and sketch regions", "Filter" : EntityType.FACE }
        definition.faces is Query;
        annotation { "Name" : "Add" }
        definition.add is boolean;
    }
    {
        const smEdgesAndFaces = qUnion([qOwnedByBody(qAttributeQuery(asSMAttribute({ objectType : SMObjectType.MODEL })), EntityType.EDGE),
                    qAttributeQuery(asSMAttribute({ objectType : SMObjectType.WALL }))]);
        const tracking = startTracking(context, smEdgesAndFaces);
        definition.operationType = definition.add ? BooleanOperationType.UNION : BooleanOperationType.SUBTRACTION;
        opSMFlatOperation(context, id, definition);
        const newEntities = qUnion([qCreatedBy(id), tracking]);
        const originalEntities = evaluateQuery(context, qSubtraction(qOwnedByBody(qOwnerBody(tracking)), newEntities));
        const initialAssociationAttributes = getAttributes(context, {
                    "entities" : qOwnedByBody(qOwnerBody(tracking)),
                    "attributePattern" : {} as SMAssociationAttribute });
        const toUpdate = assignSMAttributesToNewOrSplitEntities(context, qOwnerBody(tracking),
                originalEntities, initialAssociationAttributes);

        updateSheetMetalGeometry(context, id + "smUpdate", {
                    "entities" : toUpdate.modifiedEntities,
                    "deletedAttributes" : toUpdate.deletedAttributes,
                    "associatedChanges" : tracking });
    }, {});
