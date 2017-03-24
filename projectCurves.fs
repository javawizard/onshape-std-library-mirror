FeatureScript 543; /* Automatically generated version */
import(path : "onshape/std/boundingtype.gen.fs", version : "543.0");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "543.0");
import(path : "onshape/std/feature.fs", version : "543.0");
import(path : "onshape/std/evaluate.fs", version : "543.0");
import(path : "onshape/std/vector.fs", version : "543.0");

/**
 *  Performs [opExtrude] twice to extrude two sketches and then [opBoolean] to produce the intersection of the extruded surfaces
 */
annotation { "Feature Type Name" : "Project curves" }
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
        opBoolean(context, id + "boolean", {
                "tools" : toolsQ,
                "operationType" : BooleanOperationType.INTERSECTION,
                "allowSheets" : true,
                "eraseImprintedEdges" : false
        });
    });

