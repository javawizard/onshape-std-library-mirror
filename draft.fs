FeatureScript 293; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");

// Imports used internally
import(path : "onshape/std/evaluate.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/valueBounds.fs", version : "");
import(path : "onshape/std/vector.fs", version : "");

/**
 * @see `opDraft`.
 */
annotation { "Feature Type Name" : "Draft", "Filter Selector" : "allparts" }
export const draft = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Neutral plane",
                     "Filter" : GeometryType.PLANE,
                     "MaxNumberOfPicks" : 1 }
        definition.neutralPlane is Query;

        annotation { "Name" : "Entities to draft", "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO }
        definition.draftFaces is Query;

        annotation { "Name" : "Draft angle" }
        isAngle(definition.angle, ANGLE_STRICT_90_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.pullDirection is boolean;

        annotation { "Name" : "Tangent propagation", "Default" : true }
        definition.tangentPropagation is boolean;

        annotation { "Name" : "Reapply fillet", "Default" : false }
        definition.reFillet is boolean;
    }
    {
        const planeResult = try(evFaceTangentPlane(context, { "face" : definition.neutralPlane, "parameter" : vector(0.5, 0.5) }));
        if (planeResult == undefined)
            throw regenError(ErrorStringEnum.DRAFT_SELECT_NEUTRAL, ["neutralPlane"]);

        definition.pullVec = planeResult.normal;

        if (definition.pullDirection)
            definition.pullVec = -definition.pullVec;

        opDraft(context, id, definition);
    }, { pullDirection : false, tangentPropagation : false, reFillet : false });

