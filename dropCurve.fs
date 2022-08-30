FeatureScript ✨; /* Automatically generated version */
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/vector.fs", version : "✨");
import(path : "onshape/std/topologyUtils.fs", version : "✨");
import(path : "onshape/std/manipulator.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");

export import(path : "onshape/std/projectiontype.gen.fs", version : "✨");

/**
 *  Projects selected curves on a face along a direction or the target surface normal.
 */
annotation { "Feature Type Name" : "Drop curve",
             "Manipulator Change Function" : "dropCurveManipulatorChange",
             "UIHint" : UIHint.NO_PREVIEW_PROVIDED }
export const dropCurve = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Edges", "Filter" : EntityType.EDGE && ConstructionObject.NO }
        definition.dropTools is Query;
        annotation { "Name" : "Projection type" }
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
        annotation { "Name" : "Targets", "Filter" : EntityType.FACE ||  (EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO) }
        definition.targets is Query;
    }
    {
        if (isQueryEmpty(context, definition.dropTools))
        {
            throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["dropTools"]);
        }
        definition.tools = definition.dropTools;
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
        opDropCurve(context, id, definition);
    });

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
export function dropCurveManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
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

