FeatureScript 2581; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2581.0");
export import(path : "onshape/std/boolean.fs", version : "2581.0");

// Imports used internally
import(path : "onshape/std/feature.fs", version : "2581.0");

/**
 * Feature performing an [opEnclose].
 */
annotation { "Feature Type Name" : "Enclose" }
export const enclose = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        booleanStepTypePredicate(definition);

        annotation { "Name" : "Entities", "Filter" : ((EntityType.BODY && (BodyType.SHEET || BodyType.SOLID) && SketchObject.NO) || EntityType.FACE) && AllowMeshGeometry.YES }
        definition.entities is Query;

        if (definition.operationType != NewBodyOperationType.NEW)
        {
            booleanStepScopePredicate(definition);
        }

        annotation { "Name" : "Keep tools" }
        definition.keepTools is boolean;
    }
    {
        // Evaluate inputs here for delete later so that passing tracking queries won't cause the results to get deleted
        // Exclude construction planes and sketch regions so they don't get deleted.
        var evaluatedTools = qUnion(evaluateQuery(context, qSketchFilter(qConstructionFilter(definition.entities,
                    ConstructionObject.NO), SketchObject.NO)));
        const encloseDefinition = {
                "entities" : definition.entities,
                "mergeResults" : definition.mergeResults
            };
        opEnclose(context, id + "enclose", encloseDefinition);

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
                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2540_APPX_CURVE_ENCLOSE_FIXES))
                {
                    definition.mergeScopeExclusion = evaluatedTools;
                }
            }
        }

        var reconstructOp = function(id)
            {
                opEnclose(context, id + "enclose", encloseDefinition);
            };
        processNewBodyIfNeeded(context, id, definition, reconstructOp);

        if (!definition.keepTools)
        {
            try silent
            {
                opDeleteBodies(context, id + "delete",
                    { "entities" : evaluatedTools
                        });
            }
        }
    }, { keepTools : false, operationType : NewBodyOperationType.NEW });

