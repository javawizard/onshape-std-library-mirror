FeatureScript 189; /* Automatically generated version */
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/boolean.fs", version : "");
export import(path : "onshape/std/geomOperations.fs", version : "");
export import(path : "onshape/std/feature.fs", version : "");

annotation { "Feature Type Name" : "Loft", "Filter Selector" : "allparts" }
export const loft = defineFeature(function(context is Context, id is Id, loftDefinition is map)
    precondition
    {

        annotation { "Name" : "Creation type" }
        loftDefinition.bodyType is ToolBodyType;

        if (loftDefinition.bodyType == ToolBodyType.SOLID)
        {
            booleanStepTypePredicate(loftDefinition);
        }

        if (loftDefinition.bodyType == ToolBodyType.SOLID)
        {
            annotation { "Name" : "Profiles",
                         "Filter" : ((EntityType.FACE && GeometryType.PLANE) || EntityType.VERTEX) && ConstructionObject.NO }
            loftDefinition.sheetProfiles is Query;
        }
        else
        {
            annotation { "Name" : "Profiles",
                         "Filter" : (EntityType.VERTEX || EntityType.EDGE || EntityType.FACE) && ConstructionObject.NO }
            loftDefinition.wireProfiles is Query;
        }

        annotation { "Name" : "Add guides" }
        loftDefinition.addGuides is boolean;
        if (loftDefinition.addGuides)
        {
            annotation { "Name" : "Guides", "Filter" : EntityType.EDGE && ConstructionObject.NO }
            loftDefinition.guides is Query;
        }

        annotation { "Name" : "Match vertices" }
        loftDefinition.matchVertices is boolean;
        if (loftDefinition.matchVertices)
        {
            annotation { "Name" : "Vertices", "Filter" : EntityType.VERTEX }
            loftDefinition.vertices is Query;
        }

        annotation { "Name" : "Make periodic", "UIHint" : "ALWAYS_HIDDEN" }
        loftDefinition.makePeriodic is boolean;

        if (loftDefinition.bodyType == ToolBodyType.SOLID)
        {
            booleanStepScopePredicate(loftDefinition);
        }
    }
    {
        var profileQuery = (loftDefinition.bodyType == ToolBodyType.SOLID) ? loftDefinition.sheetProfiles : loftDefinition.wireProfiles;
        if (profileQuery.queryType == QueryType.UNION)
        {
            var subQ = wrapSubqueriesInConstructionFilter(context, profileQuery.subqueries);
            if (size(subQ) < 1)
            {
                var errorEntities = (loftDefinition.bodyType == ToolBodyType.SOLID) ? "sheetProfiles" : "wireProfiles";
                reportFeatureError(context, id, ErrorStringEnum.LOFT_SELECT_PROFILES, [errorEntities]);
                return;
            }

            loftDefinition.profileSubqueries = subQ;
        }

        if (loftDefinition.addGuides)
        {
            var guideQuery = loftDefinition.guides;
            if (guideQuery.queryType == QueryType.UNION)
            {
                var subQ = guideQuery.subqueries;
                loftDefinition.guideSubqueries = wrapSubqueriesInConstructionFilter(context, subQ);
            }
        }
        if (!loftDefinition.matchVertices)
        {
            loftDefinition.vertices = qUnion([]);
        }

        opLoft(context, id, loftDefinition);

        if (loftDefinition.bodyType == ToolBodyType.SOLID)
        {
            if (!processNewBodyIfNeeded(context, id, loftDefinition))
            {
                var statusToolId = id + "statusTools";
                startFeature(context, statusToolId, loftDefinition);
                opLoft(context, statusToolId, loftDefinition);
                setBooleanErrorEntities(context, id, statusToolId);
                endFeature(context, statusToolId);
            }
        }
    }, { makePeriodic : false, bodyType : ToolBodyType.SOLID, operationType : NewBodyOperationType.NEW, addGuides : false, matchVertices : false });

export function wrapSubqueriesInConstructionFilter(context is Context, subqueries is array) returns array
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V177_CONSTRUCTION_OBJECT_FILTER))
    {
        var wrappedSubqueries = [];
        for (var i = 0; i < @size(subqueries); i += 1)
        {
            wrappedSubqueries = append(wrappedSubqueries, qConstructionFilter(subqueries[i], ConstructionObject.NO));
        }
        return wrappedSubqueries;
    }
    return subqueries;
}

