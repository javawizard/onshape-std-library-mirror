FeatureScript 2345; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/context.fs", version : "2345.0");
import(path : "onshape/std/coordSystem.fs", version : "2345.0");

/**
 * A coordinate system that can persist as part of an attribute associated
 * with an entity.  This coordinate system will be transformed along with
 * its parent entity as that entity undergoes transformations.
 *
 * As with other attributes, the coordinate system will be propagated to
 * copied entities, such as instances in a pattern.  These copied persistent
 * coordinate systems will take on the transforms of their new parents.
 *
 * When [getAttribute] is used to retrieve a previously-set persistent coordinate
 * system, the value of coordSystem will be in its transformed state for the
 * current point in the feature execution.  If a transform is applied such that
 * the coordinate system is know longer right-handed, then the coordSystem value
 * will be undefined. For instance, this would happen in the case of a mirrored
 * coordinate system.
 *
 * @type {{
 *      @field coordSystem {CoordSystem}: The coordinate system to persist
 *      @field coordSystemId {string}: An id to associate with the coordinate system.
 *          This id must be unique within the context of the parent entity.
 * }}
 */
export type PersistentCoordSystem typecheck canBePersistentCoordSystem;

/** @internal */
export predicate canBePersistentCoordSystem(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    value.coordSystemId is string;
}

/**
 * Creates a persistent coordinate system.
 *
 * @seealso [PersistentCoordSystem]
 *
 * @param coordSystem : The coordinate system
 * @param coordSystemId : An id with which to associate the coordinate system.
 *     This id must be unique within the context of the parent entity that
 *     an becomes associated with this persistent coordinate system through [setAttribute].
 */
export function persistentCoordSystem(coordSystem is CoordSystem, coordSystemId is string) returns PersistentCoordSystem
{
    return {
        "coordSystem": coordSystem,
        "coordSystemId": coordSystemId
    } as PersistentCoordSystem;
}

