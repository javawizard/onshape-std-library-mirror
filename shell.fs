FeatureScript 686; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "686.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "686.0");
import(path : "onshape/std/feature.fs", version : "686.0");
import(path : "onshape/std/tool.fs", version : "686.0");
import(path : "onshape/std/valueBounds.fs", version : "686.0");

/**
 * Feature performing an [opShell].
 */
annotation { "Feature Type Name" : "Shell", "Filter Selector" : "allparts" }
export const shell = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Hollow", "Default" : false }
        definition.isHollow is boolean;

        if (!definition.isHollow)
        {
            annotation { "Name" : "Faces to remove",
                "Filter" : EntityType.FACE && BodyType.SOLID && ModifiableEntityOnly.YES && ActiveSheetMetal.NO}
            definition.entities is Query;
        }
        else
        {
            annotation { "Name" : "Parts to hollow",
                "Filter" : EntityType.BODY && BodyType.SOLID && ModifiableEntityOnly.YES && ActiveSheetMetal.NO }
            definition.parts is Query;
        }

        annotation { "Name" : "Shell thickness" }
        isLength(definition.thickness, SHELL_OFFSET_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;
    }
    {
        if (definition.isHollow)
            definition.entities = qEntityFilter(definition.parts, EntityType.BODY);
        else
            definition.entities = qEntityFilter(definition.entities, EntityType.FACE);

        if (size(evaluateQuery(context, definition.entities)) == 0)
        {
            if (definition.isHollow)
                throw regenError(ErrorStringEnum.SHELL_SELECT_PARTS, ["parts"]);
            else
                throw regenError(ErrorStringEnum.SHELL_SELECT_FACES, ["entities"]);
        }

        if (!definition.oppositeDirection)
            definition.thickness = -definition.thickness;
        opShell(context, id, definition);
    }, { oppositeDirection : false, isHollow : false });

