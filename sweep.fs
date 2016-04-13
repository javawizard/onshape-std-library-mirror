FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "✨");
export import(path : "onshape/std/tool.fs", version : "✨");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "✨");
import(path : "onshape/std/booleanHeuristics.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");

/**
 * Feature performing an `opSweep`, followed by an `opBoolean`. For simple sweeps, prefer using
 * `opSweep` directly.
 */
annotation { "Feature Type Name" : "Sweep",
             "Filter Selector" : "allparts",
             "Editing Logic Function" : "sweepEditLogic" }
export const sweep = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Creation type" }
        definition.bodyType is ToolBodyType;

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            booleanStepTypePredicate(definition);

            annotation { "Name" : "Faces and sketch regions to sweep",
                         "Filter" : (EntityType.FACE && GeometryType.PLANE) && ConstructionObject.NO }
            definition.profiles is Query;
        }
        else
        {
            annotation { "Name" : "Edges and sketch curves to sweep",
                         "Filter" : (EntityType.EDGE && ConstructionObject.NO) }
            definition.surfaceProfiles is Query;
        }

        annotation { "Name" : "Sweep path", "Filter" : EntityType.EDGE && ConstructionObject.NO }
        definition.path is Query;

        annotation { "Name" : "Keep profile orientation" }
        definition.keepProfileOrientation is boolean;

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            booleanStepScopePredicate(definition);
        }
    }
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V203_SWEEP_PATH_NO_CONSTRUCTION))
        {
            const pathQuery = definition.path;
            definition.path = qConstructionFilter(definition.path, ConstructionObject.NO);
            if (pathQuery.queryType == QueryType.UNION && size(pathQuery.subqueries) > 0)
            {
                const queryResults = evaluateQuery(context, definition.path);
                if (size(queryResults) == 0)
                    throw regenError(ErrorStringEnum.SWEEP_PATH_NO_CONSTRUCTION, ["path"]);
            }
        }
        if (definition.bodyType == ToolBodyType.SURFACE)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V177_CONSTRUCTION_OBJECT_FILTER))
            {
                definition.profiles = qConstructionFilter(definition.surfaceProfiles, ConstructionObject.NO);
            }
            else
            {
                definition.profiles = definition.surfaceProfiles;
            }
        }

        var remainingTransform = getRemainderPatternTransform(context,
                {"references" : qUnion([definition.profiles, definition.path])});

        opSweep(context, id, definition);
        transformResultIfNecessary(context, id, remainingTransform);

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            const reconstructOp = function(id)
            {
                opSweep(context, id, definition);
                transformResultIfNecessary(context, id, remainingTransform);
            };
            processNewBodyIfNeeded(context, id, definition, reconstructOp);
        }
    }, { bodyType : ToolBodyType.SOLID, operationType : NewBodyOperationType.NEW, keepProfileOrientation : false });


/**
 * @internal
 * Editing logic function for sweep feature.
 */
export function sweepEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    return booleanStepEditLogic(context, id, oldDefinition, definition,
                                specifiedParameters, hiddenBodies, sweep);
}

