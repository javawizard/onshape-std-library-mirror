FeatureScript 244; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/tool.fs", version : "");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "");
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/evaluate.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");

/**
 * @see `opSweep`.
 */
annotation { "Feature Type Name" : "Sweep", "Filter Selector" : "allparts" }
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
        opSweep(context, id, definition);

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            const reconstructOp = function(id) { opSweep(context, id, definition); };
            processNewBodyIfNeeded(context, id, definition, reconstructOp);
        }
    }, { bodyType : ToolBodyType.SOLID, operationType : NewBodyOperationType.NEW, keepProfileOrientation : false });

