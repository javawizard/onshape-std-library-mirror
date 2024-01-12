FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/context.fs", version : "✨");
import(path : "onshape/std/coordSystem.fs", version : "✨");

/** @internal */
export type PersistentCoordSystem typecheck canBePersistentCoordSystem;

/** @internal */
export predicate canBePersistentCoordSystem(value)
{
    value is map;
    value.coordSystem is CoordSystem;
    value.coordSystemId is string;
}

/** @internal */
export function persistentCoordSystem(coordSystem is CoordSystem, coordSystemId is string) returns PersistentCoordSystem
{
    return {
        "coordSystem": coordSystem,
        "coordSystemId": coordSystemId
    } as PersistentCoordSystem;
}

