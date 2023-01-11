FeatureScript 1948; /* Automatically generated version */
import(path : "onshape/std/feature.fs", version : "1948.0");

/**
 *  Creates curves where two faces intersect.
 */
annotation { "Feature Type Name" : "Intersection curve",
             "UIHint" : UIHint.NO_PREVIEW_PROVIDED }
export const intersectionCurve = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Group 1", "Filter" : EntityType.FACE ||  (EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO) }
        definition.group1 is Query;
        annotation { "Name" : "Group 2", "Filter" : EntityType.FACE ||  (EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO) }
        definition.group2 is Query;
    }
    {
        if (isQueryEmpty(context, definition.group1))
        {
            throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["group1"]);
        }
        definition.tools = definition.group1;
        if (isQueryEmpty(context, definition.group2))
        {
            throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["group2"]);
        }
        definition.targets = definition.group2;

        const bodyFaces1 = qOwnedByBody(qEntityFilter(definition.group1, EntityType.BODY), EntityType.FACE);
        const faces1 = qEntityFilter(definition.group1, EntityType.FACE);
        const union1 = qUnion([faces1, bodyFaces1]);

        const bodyFaces2 = qOwnedByBody(qEntityFilter(definition.group2, EntityType.BODY), EntityType.FACE);
        const faces2 = qEntityFilter(definition.group2, EntityType.FACE);
        const union2 = qUnion([faces2, bodyFaces2]);

        const inputIntersection = qUnion([qIntersection([faces1, bodyFaces1]),
                                          qIntersection([faces2, bodyFaces2]),
                                          qIntersection([union1, union2])]);

        if (!isQueryEmpty(context, inputIntersection))
        {
            throw regenError(ErrorStringEnum.FACE_INTERSECTION_UNIQUE_SELECTION, inputIntersection);
        }

        var remainingTransform = getRemainderPatternTransform(context,
            {
                "references" : qUnion(definition.group1, definition.group2)
            });

        opIntersectFaces(context, id, definition);

        transformResultIfNecessary(context, id, remainingTransform);
    });
