FeatureScript 1112; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 ******************************************
 * Under development, not for general use!!
 ******************************************
 */

import(path : "onshape/std/error.fs", version : "1112.0");
import(path : "onshape/std/feature.fs", version : "1112.0");
import(path : "onshape/std/query.fs", version : "1112.0");



/**
* @internal
*  TODO : This feature assigns a name to selected entity. This name becomes its identity.
*  qNamed(name) will resolve to this entity or its modifications.
*  When referenced qNamed feature is generated for this entity.
*/
annotation { "Feature Type Name" : "Name entity",
             "Feature Name Template": "Name #entityName",
             "UIHint" : "NO_PREVIEW_PROVIDED"}
export const nameEntity = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entity", "Filter" : EntityType.FACE || EntityType.BODY || EntityType.EDGE, "MaxNumberOfPicks" : 1 }
        definition.entity is Query;

        annotation { "Name" : "Name" }
        definition.entityName is string;
    }
    {
        opNameEntity(context, id, definition);
    });

