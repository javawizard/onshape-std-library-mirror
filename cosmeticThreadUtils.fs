FeatureScript 2737; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "2737.0");
import(path : "onshape/std/context.fs", version : "2737.0");
import(path : "onshape/std/coordSystem.fs", version : "2737.0");
import(path : "onshape/std/persistentCoordSystem.fs", version : "2737.0");
import(path : "onshape/std/query.fs", version : "2737.0");
import(path : "onshape/std/string.fs", version : "2737.0");
import(path : "onshape/std/vector.fs", version : "2737.0");

/**
 * Data required to properly render cosmetic threads onto tapped surfaces.
 *
 * @type {{
 *      @field threadOriginCoordSys {PersistentCoordSystem} : A coordinate system representing
 *          the origin of a thread. For external threads, this represents the center of the
 *          circular edge selected for the thread. For tapped holes, it represents
 *          the vertex selected to create the hole.
 *      @field threadPitch {number} : Pitch of the thread per the selected ANSI or ISO standard.
 *      @field tappedDepth {number} : Distance from the thread origin to the thread's termination.
 * }}
 */
export type CosmeticThreadData typecheck canBeCosmeticThreadData;

/** @internal */
const THREADS_ATTRIBUTE_NAME = "cosmeticThreadAttribute";

/** @internal */
export predicate canBeCosmeticThreadData(value)
{
    value is map;
    value.threadOriginCoordSys is PersistentCoordSystem;
    value.threadPitch is number;
    value.tappedDepth is number;
}

/**
 * Applies a cosmetic thread attribute to the specified entities.
 *
 * @seealso [CosmeticThreadData]
 *
 * @param context {Context} : The application context.
 * @param entities {Query} : A query for entities to requiring thread data.
 * @param cosmeticThreadData {CosmeticThreadData} : Data required to properly render
 *          tapped threads on faces.
 */
export function addCosmeticThreadAttribute(context is Context,
                                           entities is Query,
                                           cosmeticThreadData is CosmeticThreadData)
{
    for (var entity in evaluateQuery(context, entities))
    {
        setAttribute(context, {
            "entities" : entity,
            "name" : THREADS_ATTRIBUTE_NAME,
            "attribute" : cosmeticThreadData
        });
    }
}

/**
 * Creates a cosmetic thread data attribute to be applied to face entities using [addCosmeticThreadAttribute].
 *
 * @seealso [CosmeticThreadData]
 *
 * @param threadCoordSys {CoordSystem} : Coordinate system used to determine the orientation and bounds of the thread.
 *          The origin of this coordinate system is considered the starting point of the thread.
 * @param tappedDepth {number} : Indicates how far the thread should be rendered from the thread origin.
 * @param threadPitch {number} : Distance between each adjacent thread.
 */
export function createCosmeticThreadDataFromEntity(threadCoordSys is CoordSystem, tappedDepth is number,
    threadPitch is number)
    returns CosmeticThreadData
{
    return {
        'threadOriginCoordSys': persistentCoordSystem(threadCoordSys, toString("threadOriginSystem"), true),
        'tappedDepth': tappedDepth,
        'threadPitch': threadPitch
    } as CosmeticThreadData;
}

