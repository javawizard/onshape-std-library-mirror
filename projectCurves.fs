FeatureScript 2716; /* Automatically generated version */
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "2716.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "2716.0");
import(path : "onshape/std/containers.fs", version : "2716.0");
import(path : "onshape/std/evaluate.fs", version : "2716.0");
import(path : "onshape/std/feature.fs", version : "2716.0");
import(path : "onshape/std/manipulator.fs", version : "2716.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2716.0");
import(path : "onshape/std/topologyUtils.fs", version : "2716.0");
import(path : "onshape/std/units.fs", version : "2716.0");
import(path : "onshape/std/vector.fs", version : "2716.0");
import(path : "onshape/std/approximationUtils.fs", version : "2716.0");

export import(path : "onshape/std/projectiontype.gen.fs", version : "2716.0");

/**
 * Specifies the method used for generating intersection curves.
 *
 * @value TWO_SKETCHES: Performs [opExtrude] to convert sketches into surface, then uses [opBoolean] to intersect
 *          those surfaces.
 * @value CURVE_TO_FACE: Performs [opDropCurve] to project curves onto faces.
 */
export enum CurveProjectionType
{
    annotation { "Name" : "Two sketches" }
    TWO_SKETCHES,
    annotation { "Name" : "Curve to face" }
    CURVE_TO_FACE
}

/**
  * Feature creating projected curves.
  *
  * @param id : @autocomplete `id + "projectCurves1"`
  * @param definition {{
  *      @field curveProjectionType {CurveProjectionType}: @optional
  *            The method used for generating intersection curves.
  *            Default is `TWO_SKETCHES`.
  *      @field sketchEdges1 {Query}: @requiredif { `curveProjectionType` is `TWO_SKETCHES` }
  *            Edges from a single sketch that will be extruded to perform an intersection.
  *      @field sketchEdges2 {Query}: @requiredif { `curveProjectionType` is `TWO_SKETCHES` }
  *            Edges from a single sketch that will be extruded to perform an intersection.
  *      @field dropTools {Query}: @requiredif { `curveProjectionType` is `CURVE_TO_FACE` }
  *            Edges that will be projected onto `targets`.
  *      @field projectionType {ProjectionType}: @requiredif { `curveProjectionType` is `CURVE_TO_FACE` }
  *            Specifies whether to project along a direction or the face normal of `targets`.
  *      @field directionQuery {Query}: @requiredif { `projectionType` is `DIRECTION` }
  *            Specifies the direction of projection.
  *      @field oppositeDirection {boolean}: @requiredif { `projectionType` is `DIRECTION` }
  *            If true, negates the direction supplied by `directionQuery`.
  *      @field targets {Query}: @requiredif { `curveProjectionType` is `CURVE_TO_FACE` }
  *            Faces, sheet bodies, or solid bodies to project onto.
  * }}
 */
annotation { "Feature Type Name" : "Projected curve",
             "Manipulator Change Function" : "projectedCurveManipulatorChange",
             "Editing Logic Function" : "projectCurvesEditLogic",
             "UIHint" : UIHint.NO_PREVIEW_PROVIDED }
export const projectCurves = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Preselection", "UIHint" : UIHint.ALWAYS_HIDDEN, "Filter" : EntityType.EDGE && ConstructionObject.NO }
        definition.preselectedEntities is Query;
        annotation { "Name" : "Curve projection type", "UIHint" : [UIHint.HORIZONTAL_ENUM, UIHint.REMEMBER_PREVIOUS_VALUE] }
        definition.curveProjectionType is CurveProjectionType;
        if (definition.curveProjectionType == CurveProjectionType.TWO_SKETCHES)
        {
            annotation { "Name" : "First sketch", "Filter" : EntityType.EDGE && SketchObject.YES && ConstructionObject.NO }
            definition.sketchEdges1 is Query;
            annotation { "Name" : "Second sketch", "Filter" : EntityType.EDGE && SketchObject.YES && ConstructionObject.NO }
            definition.sketchEdges2 is Query;
        }
        else
        {
            annotation { "Name" : "Edges", "Filter" : EntityType.EDGE && ConstructionObject.NO }
            definition.dropTools is Query;
            annotation { "Name" : "Projection direction type" }
            definition.projectionType is ProjectionType;
            if (definition.projectionType == ProjectionType.DIRECTION)
            {
                annotation { "Name" : "Direction",
                            "Filter" : QueryFilterCompound.ALLOWS_DIRECTION,
                            "MaxNumberOfPicks" : 1 }
                definition.directionQuery is Query;
                annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                definition.oppositeDirection is boolean;
            }
            annotation { "Name" : "Targets", "Filter" : EntityType.FACE || (EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO) }
            definition.targets is Query;
        }

        curveApproximationPredicate(definition);
    }
    {
        if (definition.curveProjectionType == CurveProjectionType.TWO_SKETCHES)
        {
            projectSketches(context, id, definition);
        }
        else
        {
            dropCurve(context, id, definition);
        }

        if (definition.approximate)
        {
            approximateResults(context, id, definition);
        }
    }, { curveProjectionType : CurveProjectionType.TWO_SKETCHES,
         preselectedEntities : qNothing(),
         approximate : false });

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

function projectSketches(context is Context, id is Id, definition is map)
{
    // We want to extrude the sketch edges
    // Then we want to intersect the resulting faces with the target faces
    // Then we want to delete the extruded faces

    const edgeQ1 = qConstructionFilter(definition.sketchEdges1, ConstructionObject.NO);
    const edgeQ2 = qConstructionFilter(definition.sketchEdges2, ConstructionObject.NO);

    if (isQueryEmpty(context, edgeQ1))
    {
        throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["sketchEdges1"]);
    }

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V558_PROJECT_EDGES_SAME_SKETCH))
    {
        verifySameSketch(context, edgeQ1, "sketchEdges1");
    }

    if (isQueryEmpty(context, edgeQ2))
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

    const sizeAll = box3dDiagonalLength(boxAll);

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
}

function dropCurve(context is Context, id is Id, definition is map)
{
    const tools = qConstructionFilter(definition.dropTools, ConstructionObject.NO);
    if (isQueryEmpty(context, tools))
    {
        throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["dropTools"]);
    }
    if (definition.projectionType == ProjectionType.DIRECTION)
    {
        if (isQueryEmpty(context, definition.directionQuery))
        {
            throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["directionQuery"]);
        }
        else
        {
            definition.direction = extractDirection(context, definition.directionQuery) * (definition.oppositeDirection ? -1 : 1);
            addDropCurveManipulator(context, id, definition);
        }
    }
    if (isQueryEmpty(context, definition.targets))
    {
        throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["targets"]);
    }
    var remainingTransform = getRemainderPatternTransform(context,
        {
            "references" : qUnion(definition.dropTools, definition.targets)
        });

    opDropCurve(context, id, mergeMaps(definition, { "tools" : tools, "showCurves" : !definition.approximate }));

    transformResultIfNecessary(context, id, remainingTransform);
}


/**
 * @internal
 * Manipulator to keep front/back side of split
 */
function addDropCurveManipulator(context is Context, id is Id, definition is map)
{
    if (definition.projectionType != ProjectionType.DIRECTION || isQueryEmpty(context, definition.directionQuery))
    {
        return;
    }
    var manipulatorPlane;
    if (!isQueryEmpty(context, qGeometry(definition.directionQuery, GeometryType.PLANE)))
    {
        manipulatorPlane = evPlane(context, {
                "face" : definition.directionQuery
        });
    }
    else
    {
        const axis = evAxis(context, {
                "axis" : definition.directionQuery
        });
        manipulatorPlane = plane(axis.origin, axis.direction);
    }
    var manipulator is Manipulator = flipManipulator({
        "base" : manipulatorPlane.origin,
        "direction" : manipulatorPlane.normal,
        "flipped" : definition.oppositeDirection
    });
    addManipulators(context, id, {
        "flipManipulator" : manipulator
    });
}

/**
 * @internal
 * Manipulator change function for `curveOnSurface`.
 */
export function projectedCurveManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    for (var manipulator in newManipulators)
    {
        if (manipulator.key == "flipManipulator")
        {
            definition.oppositeDirection = manipulator.value.flipped;
        }
    }
    return definition;
}

function needDirectionFlip(context is Context, id is Id, definition is map) returns boolean
{
    var direction = extractDirection(context, definition.directionQuery) * (definition.oppositeDirection ? -1 : 1);
    const directionCSys = coordSystem(plane(vector(0, 0, 0) * meter, direction));

    const toolBB = evBox3d(context, {
            "topology" : definition.dropTools,
            "cSys" : directionCSys
    });
    const targetBB = evBox3d(context, {
            "topology" : definition.targets,
            "cSys" : directionCSys
    });

    // Only flip if the target bounding box is outside the tool bounding box in the other direction
    return toolBB.minCorner[2] > targetBB.maxCorner[2];
}

function isDefinitionCompleteForDirectionProjection(context is Context, definition is map) returns boolean
{
    return !isQueryEmpty(context, definition.dropTools) &&
           !isQueryEmpty(context, definition.targets) &&
           !isQueryEmpty(context, definition.directionQuery);
}


/**
 * @internal
 * The editing logic function used in the `projectCurves` feature.
 */
export function projectCurvesEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (oldDefinition == {})
    {
        if (definition.curveProjectionType == CurveProjectionType.TWO_SKETCHES)
        {
            definition.sketchEdges1 = qSketchFilter(definition.preselectedEntities, SketchObject.YES);
        }
        else
        {
            definition.dropTools = definition.preselectedEntities;
        }
    }
    else if (definition.curveProjectionType == CurveProjectionType.CURVE_TO_FACE)
    {
        if (definition.curveProjectionType == CurveProjectionType.CURVE_TO_FACE && !specifiedParameters.oppositeDirection &&
            !isDefinitionCompleteForDirectionProjection(context, oldDefinition) &&
            isDefinitionCompleteForDirectionProjection(context, definition))
        {
            definition.oppositeDirection = needDirectionFlip(context, id, definition) ? !definition.oppositeDirection : definition.oppositeDirection;
        }
    }
    return definition;
}

