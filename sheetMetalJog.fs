FeatureScript 3083; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/containers.fs", version : "3083.0");
import(path : "onshape/std/evaluate.fs", version : "3083.0");
import(path : "onshape/std/feature.fs", version : "3083.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "3083.0");
import(path : "onshape/std/valueBounds.fs", version : "3083.0");

export import(path : "onshape/std/sheetMetalBendUtils.fs", version : "3083.0");

/**
 * Offset a sheet metal model at a reference line with two bends in opposite directions with additional jog controls.
 */
annotation { "Feature Type Name" : "Jog", "Editing Logic Function" : "onJogChange" }
export const sheetMetalJog = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        sheetMetalBendTopPredicate(definition);

        if (definition.angleControlType == BendAngleControlType.BEND_ANGLE)
        {
            annotation { "Name" : "Bend angle set programmatically", "UIHint" : [UIHint.ALWAYS_HIDDEN, UIHint.READ_ONLY] }
            definition.bendAngleSetProgrammatically is boolean;
            if (definition.bendAngleSetProgrammatically)
            {
                annotation { "Name" : "Bend angle", "UIHint" : UIHint.READ_ONLY }
                isAngle(definition.bendAngleReadOnly, SM_BEND_ANGLE_BOUNDS);
            }
            else
            {
                annotation { "Name" : "Bend angle", "UIHint" : UIHint.UNCONFIGURABLE }
                isAngle(definition.bendAngle, SM_BEND_ANGLE_BOUNDS);
            }
        }
        else if (definition.angleControlType == BendAngleControlType.ALIGN_GEOMETRY)
        {
            annotation { "Name" : "Parallel to", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION && !BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
            definition.parallelEntity is Query;
        }
        else if (definition.angleControlType == BendAngleControlType.ANGLE_FROM_DIRECTION)
        {
            annotation { "Name" : "Direction", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION && !BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
            definition.directionEntity is Query;
            annotation { "Name" : "Angle" }
            isAngle(definition.angleFromDirection, SM_BEND_DIRECTION_ANGLE_BOUNDS);
            annotation { "Name" : "Opposite direction angle", "UIHint" : UIHint.OPPOSITE_DIRECTION }
            definition.oppositeAngleFromDirection is boolean;
        }

        sheetMetalBendBottomPredicate(definition);

        annotation { "Name" : "Bounding type", "UIHint" : UIHint.SHOW_LABEL }
        definition.offsetType is JogOffsetBoundingType;
        if (definition.offsetType == JogOffsetBoundingType.BLIND)
        {
            annotation { "Name" : "Jog offset" }
            isLength(definition.bendOffset, ZERO_INCLUSIVE_OFFSET_BOUNDS);
        }
        else if (definition.offsetType == JogOffsetBoundingType.UP_TO_ENTITY)
        {
            annotation { "Name" : "Up to entity", "Filter" : (EntityType.VERTEX || EntityType.EDGE || EntityType.FACE || EntityType.BODY) && AllowMeshGeometry.YES, "MaxNumberOfPicks" : 1 }
            definition.jogLimit is Query;

            annotation { "Name" : "Offset distance", "Column Name" : "Has offset", "UIHint" : ["DISPLAY_SHORT", "FIRST_IN_ROW"] }
            definition.jogLimitOffset is boolean;

            if (definition.jogLimitOffset)
            {
                annotation { "Name" : "Offset distance", "UIHint" : UIHint.DISPLAY_SHORT }
                isLength(definition.jogLimitDistance, ZERO_INCLUSIVE_OFFSET_BOUNDS);

                annotation { "Name" : "Opposite direction", "Column Name" : "Offset opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                definition.jogLimitOppositeDirection is boolean;
            }
        }
        else // definition.offsetType == JogOffsetBoundingType.THICKNESS
        {
            annotation { "Name" : "Thickness factor" }
            isReal(definition.thicknessFactor, POSITIVE_REAL_BOUNDS);
        }

        annotation { "Name" : "Jog offset anchor", "UIHint" : UIHint.SHOW_LABEL }
        definition.bendOffsetAnchor is JogOffsetAnchor;

        annotation { "Name" : "Preserve material", "Default" : true }
        definition.preserveMaterial is boolean;
    }
    {
        checkNotInFeaturePattern(context, definition.face, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        const checkInputsReturn = checkInputs(context, id, definition, true);
        definition = checkInputsReturn.definition;
        const modelFaceQ = checkInputsReturn.modelFaceQ;
        const modelBodyQ = qOwnerBody(modelFaceQ);
        checkKFactorModificationForPcb(context, id, definition, modelBodyQ);
        const modParams = getModelParameters(context, modelBodyQ);

        const initialData = getInitialEntitiesAndAttributes(context, modelBodyQ);
        const smReturn1 = doSheetMetalBend(context, id + "bend1", definition, modelFaceQ, true, true);

        transformBendReference(context, id, modParams, definition, smReturn1);

        const bend2Id = id + "bend2";
        var bend2Definition = {};
        bend2Definition.bendReference = qCreatedBy(id + "fitWire", EntityType.EDGE);
        if (!tolerantEquals(evLine(context, { "edge" : definition.bendReference }).direction, evLine(context, { "edge" : bend2Definition.bendReference }).direction))
        {
            throw "Bend references not in the same direction";
        }
        bend2Definition.face = smReturn1.movingSurface;
        // Using BendAlignment.BENT_MIDPLANE below fails for large bend angles around and greater than 180 degrees
        // as the fixedBoundaryPlane and movingBoundaryPlane generated no longer intersect the modelFace.
        // So using BendAlignment.HELD_EDGE instead as it gives reliable results even for large bend angles.
        bend2Definition.bendAlignment = BendAlignment.HELD_EDGE;
        bend2Definition.oppositeAngle = !definition.oppositeAngle;
        bend2Definition.bendAngle = smReturn1.angle;
        bend2Definition.angleControlType = BendAngleControlType.BEND_ANGLE;
        bend2Definition = mergeMaps(definition, bend2Definition);

        const smReturn2 = doSheetMetalBend(context, bend2Id, bend2Definition, bend2Definition.face, true, false);

        opDeleteBodies(context, bend2Id + "deleteBodies", {
                    "entities" : qCreatedBy(id + "fitWire", EntityType.BODY)
                });

        // Add association attributes where needed and compute deleted attributes
        var toUpdate = assignSMAttributesToNewOrSplitEntities(context, qUnion(smReturn1.fixedSurface, smReturn2.fixedSurface), initialData, id);
        updateSheetMetalGeometry(context, id, { "entities" : toUpdate.modifiedEntities,
                    "deletedAttributes" : toUpdate.deletedAttributes });
    }, {
            oppositeAngle : false,
            holdOtherSide : false,
            bendAngleSetProgrammatically : false,
            useDefaultRadius : true,
            useDefaultKFactor : true,
            bendAlignment : BendAlignment.BEND_LINE,
            angleControlType : BendAngleControlType.BEND_ANGLE
        });

