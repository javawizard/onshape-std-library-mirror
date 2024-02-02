FeatureScript 2260; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2260.0");

// Imports used internally
import(path : "onshape/std/feature.fs", version : "2260.0");

/**
 * Feature performing an [opEnclose].
 */
annotation { "Feature Type Name" : "Enclose" }
export const enclose = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities", "Filter" : ((EntityType.BODY && (BodyType.SHEET || BodyType.SOLID)) || EntityType.FACE) && AllowMeshGeometry.YES }
        definition.entities is Query;

        annotation {"Name" : "Keep tools"}
        definition.keepTools is boolean;
    }
    {
        // Evaluate inputs here for delete later so that passing tracking queries won't cause the results to get deleted
        // Exclude construction planes and sketch regions so they don't get deleted.
        var evaluatedTools = qUnion(evaluateQuery(context, qSketchFilter(qConstructionFilter(definition.entities,
                            ConstructionObject.NO), SketchObject.NO)));
        opEnclose(context, id + "enclose", {
                    "entities" : definition.entities,
                    "mergeResults" : definition.mergeResults
                });

        if (!definition.keepTools)
        {
            try silent
            {
                evaluatedTools = isAtVersionOrLater(context, FeatureScriptVersionNumber.V647_ENCLOSE_DELETE_MODIFIABLE_TOOLS) ?
                    qModifiableEntityFilter(evaluatedTools) : evaluatedTools;
                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V989_ENCLOSE_DONT_DELETE_SKETCHES))
                {
                    evaluatedTools = qSketchFilter(evaluatedTools, SketchObject.NO);
                }
                opDeleteBodies(context, id + "delete",
                    { "entities" : evaluatedTools
                });
            }
        }
    }, { keepTools : false });

