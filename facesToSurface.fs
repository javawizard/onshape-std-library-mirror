FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */

export import(path : "onshape/std/query.fs", version : "✨");
export import(path : "onshape/std/tool.fs", version : "✨");
export import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");

import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/geomOperations.fs", version : "✨");
import(path : "onshape/std/math.fs", version : "✨");
import(path : "onshape/std/moveFace.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");

/**
 * @internal
 */
export function facesToSurfaceFunction(context is Context, id is Id, definition is map)
precondition
{
    definition.tangentPropagation is boolean;
    definition.oppositeDirection is boolean;
    isLength(definition.offset, MOVE_FACE_OFFSET_BOUNDS);
    definition.faces is Query;
}
{
    definition.tolerance = ToleranceLevel.VERY_TIGHT;

    opExtractSurface(context, id + "extractSurface", definition);

    if (abs(definition.offset) > TOLERANCE.zeroLength * meter)
    {
        var offset = definition.offset;
        if (definition.oppositeDirection)
            offset = -offset;
        opOffsetFace(context, id + "offsetFace", {
                "moveFaces" : qCreatedBy(id + "extractSurface", EntityType.FACE),
                "offsetDistance" : offset
        });
    }
}


