FeatureScript 1150; /* Automatically generated version */
import(path : "onshape/std/boundingtype.gen.fs", version : "1150.0");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "1150.0");
import(path : "onshape/std/containers.fs", version : "1150.0");
import(path : "onshape/std/feature.fs", version : "1150.0");
import(path : "onshape/std/evaluate.fs", version : "1150.0");
import(path : "onshape/std/vector.fs", version : "1150.0");

/**
 *  Performs [opExtrude] twice to extrude two sketches and then [opBoolean] to produce the intersection of the extruded surfaces
 */
annotation { "Feature Type Name" : "Projected curve",
        "UIHint" : "NO_PREVIEW_PROVIDED" }
export const projectCurves = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "First sketch", "Filter" : EntityType.EDGE && SketchObject.YES && ConstructionObject.NO }
        definition.sketchEdges1 is Query;
        annotation { "Name" : "Second sketch", "Filter" : EntityType.EDGE && SketchObject.YES && ConstructionObject.NO }
        definition.sketchEdges2 is Query;
    }
    {
        // We want to extrude the sketch edges
        // Then we want to intersect the resulting faces with the target faces
        // Then we want to delete the extruded faces

        const edgeQ1 = qConstructionFilter(definition.sketchEdges1, ConstructionObject.NO);
        const edgeQ2 = qConstructionFilter(definition.sketchEdges2, ConstructionObject.NO);

        if (size(evaluateQuery(context, edgeQ1)) == 0)
        {
            throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["sketchEdges1"]);
        }

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V558_PROJECT_EDGES_SAME_SKETCH))
        {
            verifySameSketch(context, edgeQ1, "sketchEdges1");
        }

        if (size(evaluateQuery(context, edgeQ2)) == 0)
        {
            throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["sketchEdges2"]);
        }

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V558_PROJECT_EDGES_SAME_SKETCH))
        {
            verifySameSketch(context, edgeQ2, "sketchEdges2");
        }

        const plane1 = evOwnerSketchPlane(context, {
                    "entity" : definition.sketchEdges1
                });
        const plane2 = evOwnerSketchPlane(context, {
                    "entity" : definition.sketchEdges2
                });
        if (parallelVectors(plane1.normal, plane2.normal))
        {
            throw regenError(ErrorStringEnum.PROJECT_CURVES_PARALLEL_PLANES);
        }

        const boxAll = evBox3d(context, {
                    "topology" : qUnion([edgeQ1, edgeQ2]),
                    "tight" : false
                });

        const sizeAll = norm(boxAll.maxCorner - boxAll.minCorner);

        opExtrude(context, id + "extrude1", {
                    "entities" : edgeQ1,
                    "direction" : evOwnerSketchPlane(context, { "entity" : edgeQ1 }).normal,
                    "endBound" : BoundingType.BLIND,
                    "endDepth" : sizeAll,
                    "startBound" : BoundingType.BLIND,
                    "startDepth" : sizeAll
                });
        opExtrude(context, id + "extrude2", {
                    "entities" : edgeQ2,
                    "direction" : evOwnerSketchPlane(context, { "entity" : edgeQ2 }).normal,
                    "endBound" : BoundingType.THROUGH_ALL,
                    "startBound" : BoundingType.THROUGH_ALL
                });

        var toolsQ = qUnion([qCreatedBy(id + "extrude1", EntityType.BODY), qCreatedBy(id + "extrude2", EntityType.BODY)]);
        try
        {
            opBoolean(context, id + "boolean", {
                        "tools" : toolsQ,
                        "operationType" : BooleanOperationType.INTERSECTION,
                        "allowSheets" : true,
                        "eraseImprintedEdges" : false
                    });
        }
        catch
        {
            throw regenError(ErrorStringEnum.REGEN_ERROR);
        }
        const booleanResult = qCreatedBy(id + "boolean", EntityType.BODY);
        const resultCount = size(evaluateQuery(context, booleanResult));
        const wireCount = size(evaluateQuery(context, qBodyType(booleanResult, BodyType.WIRE)));
        if (resultCount != wireCount || wireCount < 1)
        {
            throw regenError(ErrorStringEnum.REGEN_ERROR);
        }
    });

function verifySameSketch(context is Context, edges is Query, source is string)
{
    const masterId = lastModifyingOperationId(context, edges);
    const allEdges = evaluateQuery(context, edges);
    for (var edge in allEdges)
    {
        if (masterId != lastModifyingOperationId(context, edge))
        {
            throw regenError(ErrorStringEnum.PROJECT_CURVES_DIFFERENT_SKETCHES, [source]);
        }
    }
}

