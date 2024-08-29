FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "✨");
import(path : "onshape/std/context.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/debug.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/flatOperationType.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalBuiltIns.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/query.fs", version : "✨");

/**
 *  Adds or removes material of sheet metal part as specified by faces attached to its its flat pattern
 *  Is the implementation of sheet metal extrude in flat.
 *  @param definition {{
 *          @field faces {Query} :
 *              Faces associated with sheet metal flat,  typically sketch faces or region of sketch in flat
 *          @field flatOperationType {FlatOperationType}
 *  }}
 **/
export const SMFlatOp = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    {
        const bodyQ = qUnion([qPartsAttachedTo(definition.faces), qOwnerBody(definition.faces)]);
        if (!areEntitiesFromSingleActiveSheetMetalModel(context, bodyQ))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_MODEL_REQUIRED);
        }
        const smDefinitionBodiesQ = getSheetMetalModelForPart(context, bodyQ);
        const sheetMetalEntitiesQ = qUnion([qOwnedByBody(smDefinitionBodiesQ, EntityType.EDGE), qOwnedByBody(smDefinitionBodiesQ, EntityType.FACE), smDefinitionBodiesQ]);
        const tracking = startTracking(context, sheetMetalEntitiesQ);

        var initialDataPerBody = [];
        var initialData;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V860_SM_FLAT_ERRORS))
        {
            for (var smBody in evaluateQuery(context, smDefinitionBodiesQ))
            {
                initialDataPerBody = append(initialDataPerBody, {  'query' : qUnion([smBody, startTracking(context, smBody)]),
                                                'entitiesAndAttributes' : getInitialEntitiesAndAttributes(context, smBody) });
            }
        }
        else
        {
            initialData = getInitialEntitiesAndAttributes(context, smDefinitionBodiesQ);
        }

        definition.operationType = definition.flatOperationType == FlatOperationType.ADD ? BooleanOperationType.UNION : BooleanOperationType.SUBTRACTION;
        opSMFlatOperation(context, id, definition);

        makeFaceJointsUneditable(context, qCreatedBy(id, EntityType.FACE));

        const newEntities = qUnion([qCreatedBy(id), tracking]);
        const affectedBodyQ = qOwnerBody(newEntities);
        if (initialData == undefined) // data was collected in initialDataPerBody
        {
            initialData = combineInitialData(context, initialDataPerBody, affectedBodyQ);
        }
        const toUpdate = assignSMAttributesToNewOrSplitEntities(context, affectedBodyQ, initialData, id);

        callSubfeatureAndProcessStatus(id, updateSheetMetalGeometry, context, id + "smUpdate", {
                    "entities" : toUpdate.modifiedEntities,
                    "deletedAttributes" : toUpdate.deletedAttributes,
                    "associatedChanges" : tracking
                });
    }, {});

// Find any faces in `faces` which are joints, and make sure their radius and jointType are uneditable
function makeFaceJointsUneditable(context is Context, faces is Query)
{
    const jointTypeNotEditable = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1045_SHEET_BOOLEAN_ALIGN_FACE);
    const faceJoints = qAttributeFilter(faces, asSMAttribute({ 'objectType' : SMObjectType.JOINT }));
    for (var face in evaluateQuery(context, faceJoints))
    {
        const oldJointAttribute = getJointAttribute(context, face);
        if (oldJointAttribute == undefined)
        {
            if (jointTypeNotEditable)
                throw "qAttributeFilter failed";
            else
                continue;
        }

        var newJointAttribute = oldJointAttribute;
        if (oldJointAttribute.radius != undefined && oldJointAttribute.radius.canBeEdited)
        {
            // Joints that are faces cannot have their radius changed
            newJointAttribute.radius.canBeEdited = false;
        }
        if (jointTypeNotEditable && oldJointAttribute.jointType.canBeEdited)
        {
            // Joints that are faces cannot have their joint type changed
            newJointAttribute.jointType.canBeEdited = false;
        }

        if (newJointAttribute != oldJointAttribute)
        {
            removeAttributes(context, { "entities" : face, "attributePattern" : oldJointAttribute });
            setAttribute(context, { "entities" : face, "attribute" : newJointAttribute });
        }
    }
}

// Initial data was collected for all model definition bodies, bodiesQ - bodies affected by operation
// combine initial data for those bodies only
function combineInitialData(context is Context, initialDataPerBody is array, bodiesQ is Query)
{
    var targets = {};
    for (var body in evaluateQuery(context, bodiesQ))
    {
        targets[body] = true;
    }
    var originalEntitiesArr = [];
    var initialAssociationAttributesArr = [];
    var originalEntitiesTrackingArr =[];
    for (var bodyData in initialDataPerBody)
    {
        for (var body in evaluateQuery(context, bodyData.query))
        {
            if (targets[body] == true)
            {
                originalEntitiesArr = append(originalEntitiesArr, bodyData.entitiesAndAttributes.originalEntities);
                initialAssociationAttributesArr = append(initialAssociationAttributesArr,
                                            bodyData.entitiesAndAttributes.initialAssociationAttributes);
                originalEntitiesTrackingArr = append(originalEntitiesTrackingArr, bodyData.entitiesAndAttributes.originalEntitiesTracking);
            }
        }
    }
    return {'originalEntities' : concatenateArrays(originalEntitiesArr),
            'initialAssociationAttributes' : concatenateArrays(initialAssociationAttributesArr),
            'originalEntitiesTracking' : concatenateArrays(originalEntitiesTrackingArr)};
}

