FeatureScript 2022; /* Automatically generated version */
import(path : "onshape/std/feature.fs", version : "2022.0");
import(path : "onshape/std/topologyUtils.fs", version : "2022.0");

/**
 * Creates one or more Curves that are a combination of edges from various sources, be they parts, surfaces,
 * sketches or other Curves.
 */
annotation { "Feature Type Name" : "Composite curve",
        "UIHint" : UIHint.NO_PREVIEW_PROVIDED }
export const compositeCurve = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Edges", "Filter" : EntityType.EDGE || (EntityType.BODY && BodyType.WIRE) }
        definition.edges is Query;
    }
    {
        verifyNoMesh(context, definition, "edges");
        definition.edges = dissolveWires(definition.edges);

        var remainingTransform = getRemainderPatternTransform(context,
            {
                "references" : definition.edges
            });

        opExtractWires(context, id + "opExtractWires", definition);

        transformResultIfNecessary(context, id, remainingTransform);
    });

