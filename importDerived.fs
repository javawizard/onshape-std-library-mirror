FeatureScript 225; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/tool.fs", version : "");

/**
 * TODO: description
 */
export type BuildFunction typecheck canBeBuildFunction;

export predicate canBeBuildFunction(value)
{
    value is function;
}

/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
annotation { "Feature Type Name" : "Derived" }
export const importDerived = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Parts to import", "UIHint" : "ALWAYS_HIDDEN" }
        definition.parts is Query;
        annotation { "UIHint" : "ALWAYS_HIDDEN" }
        definition.buildFunction is BuildFunction;
    }
    {
        const otherContext = definition.buildFunction();
        if (otherContext != undefined)
        {
            if (size(evaluateQuery(otherContext, definition.parts)) == 0)
                throw regenError(ErrorStringEnum.IMPORT_DERIVED_NO_PARTS, ["parts"]);

            recordQueries(otherContext, id, definition);
            const bodiesToKeep = qUnion([definition.parts, qMateConnectorsOfParts(definition.parts)]);

            const deleteDefinition = {
                "entities" : qSubtraction(qEverything(EntityType.BODY), bodiesToKeep)
            };
            opDeleteBodies(otherContext, id + "delete", deleteDefinition);

            var mergeDefinition = definition; // to pass such general parameters as asVersion
            mergeDefinition.contextFrom = otherContext;
            opMergeContexts(context, id + "merge", mergeDefinition);
        }
    });

