FeatureScript 3083; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/context.fs", version : "3083.0");
import(path : "onshape/std/errorstringenum.gen.fs", version : "3083.0");
import(path : "onshape/std/query.fs", version : "3083.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "3083.0");
import(path : "onshape/std/uihint.gen.fs", version : "3083.0");
import(path : "onshape/std/valueBounds.fs", version : "3083.0");

export import(path : "onshape/std/sheetMetalBendUtils.fs", version : "3083.0");

/**
 * Bend a sheet metal model along a reference line, with additional bend control options.
 */
annotation { "Feature Type Name" : "Bend", "Editing Logic Function" : "onBendChange" }
export const sheetMetalBend = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        sheetMetalBendTopPredicate(definition);

        if (definition.angleControlType == BendAngleControlType.BEND_ANGLE)
        {
            annotation { "Name" : "Bend angle" }
            isAngle(definition.bendAngle, SM_BEND_ANGLE_BOUNDS);
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
    }
    {
        checkNotInFeaturePattern(context, definition.face, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        const isJog = false;
        const modelFaceQ = checkInputs(context, id, definition, isJog).modelFaceQ;
        checkKFactorModificationForPcb(context, id, definition, modelFaceQ);
        const initialData = getInitialEntitiesAndAttributes(context, qOwnerBody(modelFaceQ));

        const smBendReturn = doSheetMetalBend(context, id, definition, modelFaceQ, isJog, false);

        // Add association attributes where needed and compute deleted attributes
        var toUpdate = assignSMAttributesToNewOrSplitEntities(context, smBendReturn.fixedSurface, initialData, id);
        updateSheetMetalGeometry(context, id, { "entities" : toUpdate.modifiedEntities,
                    "deletedAttributes" : toUpdate.deletedAttributes });
    },
    {
            oppositeAngle : false,
            holdOtherSide : false,
            useDefaultRadius : true,
            useDefaultKFactor : true,
            bendAlignment : BendAlignment.BEND_LINE,
            angleControlType : BendAngleControlType.BEND_ANGLE
        });

