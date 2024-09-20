FeatureScript 2473; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/common.fs", version : "2473.0");
export import(path : "onshape/std/movecurveboundarytype.gen.fs", version : "2473.0");
export import(path : "onshape/std/curveextensionendcondition.gen.fs", version : "2473.0");
export import(path : "onshape/std/curveextensionshape.gen.fs", version : "2473.0");

/**
 * Extend or trim a curve. This is a thin wrapper around [opMoveCurveBoundary].
 */
annotation { "Feature Type Name" : "Trim curve",
             "UIHint" : UIHint.NO_PREVIEW_PROVIDED}
export const trimCurve = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Move curve boundary type", "UIHint" : [UIHint.HORIZONTAL_ENUM, UIHint.REMEMBER_PREVIOUS_VALUE] }
        definition.moveBoundaryType is MoveCurveBoundaryType;

        annotation { "Name" : "Curves to adjust", "Filter" : EntityType.BODY && BodyType.WIRE && SketchObject.NO }
        definition.wires is Query;

        if (definition.moveBoundaryType == MoveCurveBoundaryType.EXTEND)
        {
            annotation { "Name" : "End condition", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE] }
            definition.endCondition is CurveExtensionEndCondition;

            if (definition.endCondition == CurveExtensionEndCondition.BLIND)
            {
                annotation { "Name" : "Distance" }
                isLength(definition.extensionDistance, NONNEGATIVE_LENGTH_BOUNDS);
            }
            else
            {
                annotation { "Name" : "Up to entity", "Filter" : (EntityType.BODY && SketchObject.NO) || EntityType.FACE || EntityType.EDGE || EntityType.VERTEX || BodyType.MATE_CONNECTOR }
                definition.extendTo is Query;
            }

            annotation { "Name" : "Extension shape", "UIHint" : [UIHint.REMEMBER_PREVIOUS_VALUE] }
            definition.extensionShape is CurveExtensionShape;
        }
        else
        {
            annotation { "Name" : "Up to entity", "Filter" : (EntityType.BODY && SketchObject.NO) || EntityType.FACE || EntityType.EDGE || EntityType.VERTEX || BodyType.MATE_CONNECTOR }
            definition.trimTo is Query;
        }

        annotation { "Name" : "Help point", "UIHint" : UIHint.PREVENT_CREATING_NEW_MATE_CONNECTORS,
         "Filter" : EntityType.VERTEX || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
        definition.helpPoint is Query;

        annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.flipHeuristics is boolean;
    }
    {
        if (isQueryEmpty(context, definition.wires))
        {
            throw regenError(ErrorStringEnum.MOVE_CURVE_BOUNDARY_SELECT_CURVE, ["wires"]);
        }
        if (definition.moveBoundaryType != MoveCurveBoundaryType.EXTEND && isQueryEmpty(context, definition.trimTo))
        {
            throw regenError(ErrorStringEnum.MOVE_CURVE_BOUNDARY_SELECT_TRIM_BOUNDARY, ["trimTo"]);
        }
        opMoveCurveBoundary(context, id, definition);
    });

