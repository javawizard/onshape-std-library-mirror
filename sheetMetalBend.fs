FeatureScript 3044; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/context.fs", version : "3044.0");
import(path : "onshape/std/errorstringenum.gen.fs", version : "3044.0");
import(path : "onshape/std/query.fs", version : "3044.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "3044.0");

export import(path : "onshape/std/sheetMetalBendUtils.fs", version : "3044.0");

/**
 * Bend a sheet metal model along a reference line, with additional bend control options.
 */
annotation { "Feature Type Name" : "Bend", "Editing Logic Function" : "onBendChange" }
export const sheetMetalBend = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        sheetMetalBendPredicate(definition);
    }
    {
        checkNotInFeaturePattern(context, definition.face, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        const modelFaceQ = checkInputQueries(context, definition);
        const initialData = getInitialEntitiesAndAttributes(context, qOwnerBody(modelFaceQ));

        const smBendReturn = doSheetMetalBend(context, id, definition, modelFaceQ);

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

