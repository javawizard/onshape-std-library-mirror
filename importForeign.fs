FeatureScript 236; /* Automatically generated version */
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/valueBounds.fs", version : "");

/**
 * TODO: description
 */
export type ForeignId typecheck canBeForeignId;

export predicate canBeForeignId(value)
{
    value is string;
    //TODO: other checks
}

/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
annotation { "Feature Type Name" : "Import" }
export const importForeign = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Foreign Id" }
        definition.foreignId is ForeignId;

        annotation { "Name" : "Source is 'Y Axis Up'" }
        definition.yAxisIsUp is boolean;

        annotation {"Name" : "Flatten assembly"}
        definition.flatten is boolean;

        annotation {"Name" : "Maximum number of assemblies created", "UIHint" : "ALWAYS_HIDDEN"}
        isInteger(definition.maxAssembliesToCreate, POSITIVE_COUNT_BOUNDS);
    }
    {
        opImportForeign(context, id, definition);
    }, { yAxisIsUp : false, flatten : false, maxAssembliesToCreate : 10});

