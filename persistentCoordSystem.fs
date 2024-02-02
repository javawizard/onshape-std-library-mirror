FeatureScript 2260; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/context.fs", version : "2260.0");
import(path : "onshape/std/coordSystem.fs", version : "2260.0");

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

