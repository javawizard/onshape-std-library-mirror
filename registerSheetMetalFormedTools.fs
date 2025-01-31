FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used internally
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/formedUtils.fs", version : "✨");
import(path : "onshape/std/registerSheetMetalBooleanTools.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/transform.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");

/**
 * @internal
 */
function isFormFootPrintOnFace(context is Context, form is Query, faceDefinitionEntity is Query,
                               targetToDefinitionEntity is function)
{
    const smDefinitionBody = evaluateQuery(context, qOwnerBody(faceDefinitionEntity));
    if (size(smDefinitionBody) != 1)
    {
        throw size(smDefinitionBody) ~ " definition bodies";
    }
    const associationAttributes = getSMAssociationAttributes(context, smDefinitionBody[0]);
    if (size(associationAttributes) != 1)
    {
        throw size(associationAttributes) ~ " association Attributes";
    }

    const collisions = evCollision(context, {
            "tools" : qBodiesWithFormAttributes(form, [FORM_BODY_POSITIVE_PART, FORM_BODY_NEGATIVE_PART]),
            "targets" : qSheetMetalFlatFilter(qBodyType(qOwnerBody(qAttributeQuery(associationAttributes[0])), BodyType.SOLID), SMFlatType.NO)
    });

    if (size(collisions) == 0)
    {
        return false;
    }

    for (var collision in collisions)
    {
        if (!isInstersectingClashType(collision['type']))
        {
            continue;
        }
        const definitionEntity = targetToDefinitionEntity(collision.target);
        if (definitionEntity == qNothing() ||
            definitionEntity != faceDefinitionEntity)
        {
            // Do not allow users to register form tools that interact with
            // hole walls, side walls, rolled walls, rips, joints, or corners
            return false;
        }
    }

    return true;
}

/**
 * @internal
 *
 * Register various formed feature tool bodies to be booleaned with sheet metal bodies.
 *
 * Internally, performs an [evCollision] between the tools and targets, associates the
 * tools with the underlying sheet metal master body's walls as [SMAttribute]s and if requested,
 * calls [updateSheetMetalGeometry] which uses this information to perform the booleans on the
 * walls' 3d folded bodies.
 *
 * @param definition {{
 *      @field faceToFormedBodies {map}:
 *              Map from the faces to the formed feature tool bodies to be booleaned with the sheet metal bodies.
 *      @field doUpdateSMGeometry {boolean} :
 *              true : Call [updateSheetMetalGeometry] so that the actual boolean is performed
 *              false: The caller might want to call [updateSheetMetalGeometry] themselves
 * }}
 * @returns : Map of master body's walls to the corresponding tool bodies registered to be booleaned
 */
export const registerSheetMetalFormedTools = function(context is Context, id is Id, definition is map)
    {
        const targetToDefinitionEntity = makeDefinitionEntityCache(context);
        const definitionFaceToWallIsPlanarCache = makeIsEntityPlanarCache(context);
        var wallToFormedToolBodyIds = {};
        for (var definitionFace, formedBodies in definition.definitionFaceToFormedBodies)
        {
            if (!definitionFaceToWallIsPlanarCache(definitionFace))
            {
                // Do not allow users to register form tools that interact with
                // hole walls, side walls, rolled walls, rips, joints, or corners
                continue;
            }
            for (var form in formedBodies)
            {
                if (!isQueryEmpty(context, form->qBodyType(BodyType.SOLID)) &&
                    !isFormFootPrintOnFace(context, form, definitionFace, targetToDefinitionEntity))
                {
                    continue;
                }
                const positiveBodies = evaluateQuery(context, qBodiesWithFormAttribute(form, FORM_BODY_POSITIVE_PART));
                const nPositiveBodies = size(positiveBodies);
                if (nPositiveBodies > 1)
                {
                    throw nPositiveBodies ~ " positive bodies!";
                }
                const negativeBodies = evaluateQuery(context, qBodiesWithFormAttribute(form, FORM_BODY_NEGATIVE_PART));
                const nNegativeBodies = size(negativeBodies);
                if (nNegativeBodies > 1)
                {
                    throw nNegativeBodies ~ " negative bodies!";
                }
                const sketchBodies = evaluateQuery(context, qBodiesWithFormAttribute(form, FORM_BODY_SKETCH_FOR_FLAT_VIEW));
                var sketchBodyIds = [];
                for (var sketchBody in sketchBodies)
                {
                    sketchBodyIds = append(sketchBodyIds, sketchBody.transientId);
                }
                const mateConnector = evaluateQuery(context, qBodiesWithFormAttribute(form, FORM_BODY_CSYS_MATE_CONNECTOR));
                const formData = {  "positiveBodyId" : nPositiveBodies == 1 ? positiveBodies[0].transientId : undefined,
                                    "negativeBodyId" : nNegativeBodies == 1 ? negativeBodies[0].transientId : undefined,
                                    "sketchBodyIds" : sketchBodies != [] ? sketchBodyIds : undefined,
                                    "placementBodyId" : size(mateConnector) == 1 ? mateConnector[0].transientId : undefined
                                  };
                wallToFormedToolBodyIds = insertIntoMapOfArrays(wallToFormedToolBodyIds, definitionFace, formData);
            }
        }

        var updatedSMEntities = [];
        var robustWallToFormedToolBodyIds = {};
        for (var wall, formedToolBodyIds in wallToFormedToolBodyIds)
        {
            const wallAttribute = getWallAttribute(context, wall);
            if (wallAttribute == undefined)
            {
                throw "Unexpected as planes have been filtered";
            }
            var alteredWallAttribute = wallAttribute;
            if (alteredWallAttribute.formedToolBodyIds == undefined)
            {
                alteredWallAttribute.formedToolBodyIds = formedToolBodyIds;
            }
            else
            {
                alteredWallAttribute.formedToolBodyIds = concatenateArrays([alteredWallAttribute.formedToolBodyIds, formedToolBodyIds]);
            }
            replaceSMAttribute(context, wallAttribute, alteredWallAttribute);
            updatedSMEntities = append(updatedSMEntities, wall);
            robustWallToFormedToolBodyIds[makeRobustQuery(context, wall)] = formedToolBodyIds;
        }

        if (definition.doUpdateSMGeometry && updatedSMEntities != [])
        {
            updateSheetMetalGeometry(context, id, {
                        "entities" : qUnion(updatedSMEntities)
                    });
        }
        return robustWallToFormedToolBodyIds;
    };

