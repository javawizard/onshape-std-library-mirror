FeatureScript 675; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/**
 * A manipulator is an alternative, graphical UI for controlling a feature's definition.
 * For example, in Onshape's `extrude` feature, the arrow which appears at the end of a blind extrusion
 * is a manipulator controlling the `depth` parameter. A manipulator can be one of a few
 * `ManipulatorTypes`, which are generally draggable arrows designed to control different degrees of freedom.
 *
 * The manipulator is added inside the feature function, and will be rendered
 * whenever that feature is being edited. Changes to a manipulator will be
 * processed by a `"Manipulator Change Function"` associated with the feature.
 *
 * A small example using a manipulator to control the depth and direction of an [opExtrude] is below:
 *
 * ```
 * annotation { "Feature Type Name" : "Fake extrude",
 *         "Manipulator Change Function" : "fakeExtrudeManipulatorChange" }
 * export const fakeExtrude = defineFeature(function(context is Context, id is Id, definition is map)
 *     precondition
 *     {
 *         annotation { "Name" : "Faces to extrude", "Filter" : EntityType.FACE }
 *         definition.faces is Query;
 *         annotation { "Name" : "My Length" }
 *         isLength(definition.depth, LENGTH_BOUNDS);
 *         annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
 *         definition.shouldFlip is boolean;
 *     }
 *     {
 *         var extrudePlane is Plane = evFaceTangentPlane(context, {
 *                 "face" : definition.faces,
 *                 "parameter" : vector(0.5, 0.5)
 *         });
 *         var extrudeManipulator is Manipulator = linearManipulator(
 *                 extrudePlane.origin,
 *                 extrudePlane.normal,
 *                 definition.shouldFlip ? definition.depth : -definition.depth
 *         );
 *
 *         addManipulators(context, id, {
 *                 "myManipulator" : extrudeManipulator
 *         });
 *
 *         opExtrude(context, id + "extrude1", {
 *                 "entities" : definition.faces,
 *                 "direction" : definition.shouldFlip ? extrudePlane.normal : -extrudePlane.normal,
 *                 "endBound" : BoundingType.BLIND,
 *                 "endDepth" : definition.depth
 *         });
 *     }, {});
 *
 * export function fakeExtrudeManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
 * {
 *     var newDepth is ValueWithUnits = newManipulators["myManipulator"].offset;
 *     definition.depth = abs(newDepth);
 *     definition.shouldFlip = newDepth > 0;
 *     return definition;
 * }
 * ```
 *
 * The manipulator change function is responsible for changing the definition
 * such that the feature will regenerate correctly. It may change the definition
 * in any way, and need not be restricted to the pattern of one manipulator
 * changing one parameter.
 *
 * The feature function is only aware of the definition passed in; it makes no
 * distinction about whether the definition was produced from a manipulator
 * change, or by a change in the feature dialog, or by another custom feature.
 */

/* Calls to vector(...) and abs(...) are generated by the server,
   so those functions need to be exported. */
export import(path : "onshape/std/math.fs", version : "675.0");
export import(path : "onshape/std/manipulatorstyleenum.gen.fs", version : "675.0");
export import(path : "onshape/std/manipulatortype.gen.fs", version : "675.0");

import(path : "onshape/std/context.fs", version : "675.0");
import(path : "onshape/std/feature.fs", version : "675.0");
import(path : "onshape/std/mathUtils.fs", version : "675.0");
import(path : "onshape/std/valueBounds.fs", version : "675.0");

/**
 * A `Manipulator` is a type which can be passed into `addManipulators`,
 * containing the necessary information to position the manipulator in
 * the context. Altered copies of these manipulators are passed into a
 * manipulator change function as `newManipulators`.
 *
 * Can be constructed with [triadManipulator], [linearManipulator],
 * and other functions below.
 */
export type Manipulator typecheck canBeManipulator;

/** Typecheck for [Manipulator] */
export predicate canBeManipulator(value)
{
    value is map;
    value.manipulatorType is ManipulatorType;
    value.style is undefined || value.style is ManipulatorStyleEnum;
    //Anything else is up to the specific kind
}

/**
 * Create a manipulator represented by a triad of perpendicular arrows, aligned with
 * the world axes, which specify a 3D position. See `transformCopy` for an
 * example.
 *
 * @param base : The position of the manipulator when the offset is `0`.
 * @param offset : The 3D position of the triad, relative to the `base`.
 * @param sources {Query} :
 *          If a query for entities is passed, creates a shadow of these
 *          entities and renders that shadow rigidly locked to the manipulator
 *          (such that the shadow will move before the feature finishes
 *          regenerating).
 *
 *          Can be undefined if this behavior is not needed.
 */
export function triadManipulator(base is Vector, offset is Vector, sources) returns Manipulator
precondition
{
    is3dLengthVector(base);
    is3dLengthVector(offset);
    sources == undefined || sources is Query;
}
{
    return { "manipulatorType" : ManipulatorType.LINEAR_3D,
             "base" : base,
             "offset" : offset,
             "sources" : sources,
             "style" : ManipulatorStyleEnum.DEFAULT } as Manipulator;
}

/**
 * Create a manipulator represented by a single arrow which can move along a single axis. See
 * `extrude` for an example.
 *
 * @param base : The position of the manipulator when the offset is `0`.
 * @param direction : A 3D unit vector pointing on the axis on which the manipulator can be dragged.
 * @param offset : The positive or negative distance along `direction` from the base to the manipulator.
 * @param sources {Query} : @optional
 *          If a query for entities is passed, creates a shadow of these
 *          entities and renders that shadow rigidly locked to the manipulator
 *          (such that the shadow will move before the feature finishes
 *          regenerating).
 */
export function linearManipulator(base is Vector, direction is Vector, offset is ValueWithUnits, sources, style is ManipulatorStyleEnum) returns Manipulator
{
    return linearManipulator({ "base" : base,
                               "direction" : direction,
                               "offset" : offset,
                               "sources" : sources,
                               "style" : style });
}

export function linearManipulator(base is Vector, direction is Vector, offset is ValueWithUnits, sources) returns Manipulator
{
    return linearManipulator(base, direction, offset, sources, ManipulatorStyleEnum.DEFAULT);
}

export function linearManipulator(base is Vector, direction is Vector, offset is ValueWithUnits) returns Manipulator
{
    return linearManipulator(base, direction, offset, undefined);
}

export function linearManipulator(definition is map) returns Manipulator
precondition
{
    is3dLengthVector(definition.base);
    is3dDirection(definition.direction);
    isLength(definition.offset);
    definition.sources == undefined || definition.sources is Query;
    definition.minValue == undefined || isLength(definition.minValue);
    definition.maxValue == undefined || isLength(definition.maxValue);
}
{
    definition.manipulatorType = ManipulatorType.LINEAR_1D;
    if (definition.minValue == undefined)
    {
        definition.minValue = -500 * meter;
    }
    if (definition.maxValue == undefined)
    {
        definition.maxValue = 500 * meter;
    }
    return definition as Manipulator;
}

/**
 * A curved arrow which can move along a circumference to specify an angle,
 * with the start and end of the rotation angle delimited by radial lines. See
 * `revolve` for an example.
 *
 * @param definition {{
 *      @field axisOrigin {Vector} : The origin of the axis to rotate around.
 *              @eg `project(axis, rotationOrigin)`
 *      @field axisDirection {Vector} : The direction of the axis to rotate around.
 *              @eg `axis.direction`
 *      @field rotationOrigin {Vector} : Point at the tip of the revolve manipulator.
 *      @field sources {Query} : @optional
 *          If a query for entities is passed, creates a shadow of these
 *          entities and renders that shadow rigidly locked to the manipulator
 *          (such that the shadow will move before the feature finishes
 *          regenerating).
 *      @field minValue {ValueWithUnits} : @optional The minimum angle allowed.
 *      @field maxValue {ValueWithUnits} : @optional The maximum angle allowed.
 * }}
 */
export function angularManipulator(definition is map) returns Manipulator
precondition
{
    is3dLengthVector(definition.axisOrigin);
    is3dDirection(definition.axisDirection);
    is3dLengthVector(definition.rotationOrigin);
    isAngle(definition.angle);
    definition.sources == undefined || definition.sources is Query;
    definition.minValue == undefined || isAngle(definition.minValue);
    definition.maxValue == undefined || isAngle(definition.maxValue);
}
{
    definition.manipulatorType = ManipulatorType.ANGULAR;
    if (definition.minValue == undefined || definition.maxValue == undefined)
    {
        definition.minValue = -PI * radian;
        definition.maxValue = PI * radian;
    }
    return definition as Manipulator;
}

/**
 * Create a manipulator which can invert the direction of a feature.
 *
 * @param base {Vector}: A 3d point at the manipulator's origin
 *      @eg `Vector(0, 0, 0) * meter`
 * @param direction {Vector}: A 3d vector pointing in the unflipped direction
 *      @eg `Vector(0, 0, 1)` points manipulator along the z axis
 * @param flipped {boolean}:
 *      @eg `false` points the manipulator along +direction
 *      @eg `true`  points the manipulator along -direction
 * @param sources {Query}: @optional
 * @param style {ManipulatorStyleEnum}: @optional.
 */
export function flipManipulator(base is Vector, direction is Vector, flipped is boolean, sources, style is ManipulatorStyleEnum) returns Manipulator
precondition
{
    is3dLengthVector(base);
    is3dDirection(direction);
    sources == undefined || sources is Query;
}
{
    return { "manipulatorType" : ManipulatorType.FLIP,
             "base" : base,
             "direction" : direction,
             "flipped" : flipped,
             "sources" : sources,
             "style" : style } as Manipulator;
}

export function flipManipulator(base is Vector, direction is Vector, flipped is boolean, sources) returns Manipulator
{
    return flipManipulator(base, direction, flipped, sources, ManipulatorStyleEnum.DEFAULT);
}

export function flipManipulator(base is Vector, direction is Vector, flipped is boolean) returns Manipulator
{
    return flipManipulator(base, direction, flipped, undefined);
}

/**
 * Add a manipulator to this feature, which will be visible and interactable
 * when a user edits the feature.
 *
 * `addManipulators` should be called within the feature function, with the
 * offset on the added manipulator set to match the state of the definition.
 *
 * @param manipulators : A `map` whose keys will match the keys of
 *          `newManipulators` (passed into the Manipulator Change Function),
 *          and whose values are the `Manipulators` to be added.
 */
export function addManipulators(context is Context, id is Id, manipulators is map)
precondition
{
    for (var entry in manipulators)
    {
        entry.key is string;
        entry.value is Manipulator;
    }
}
{
    @addManipulators(context, id, manipulators);
}

/**
 * @internal
 *
 * Returns a map representing the difference between `oldDefinition` and
 * `newDefinition`. The result will include added or changed entries (with
 * the value set to the new value), and removed entries (with the value set
 * to `[undefined]`).
 */
export function processDefinitionDifference(context is Context, oldDefinition is map, newDefinition is map) returns map
{
    var result = {};
    for (var newEntry in newDefinition)
    {
        if (newEntry.value != oldDefinition[newEntry.key])
            result[newEntry.key] = newEntry.value;
    }
    for (var oldEntry in oldDefinition)
    {
        if (newDefinition[oldEntry.key] == undefined)
            //We have to distinguish removal from no change, so we use [undefined] to indicate parameter removal
            result[oldEntry.key] = [undefined];
    }
    for (var entry in result)
    {
        if (entry.value is Query)
        {
            const newEvaluation = evaluateQuery(context, entry.value);
            const oldQuery = oldDefinition[entry.key];
            if (oldQuery is Query && evaluateQuery(context, oldQuery) == newEvaluation)
                result[entry.key] = undefined; // No change
            else
                result[entry.key] = qUnion(newEvaluation);
        }
    }

    return result;
}

/**
 * @internal
 */
 export type CopyParameter typecheck canBeCopyParameter;

/**
 * @internal
 */
 export predicate canBeCopyParameter(value)
 {
    value is string;
 }

/**
 * @internal
 *
 * In an editing logic function, set a parameter to copyParameter("anotherParameter") to clone anotherParameter, including
 * configurations.  This works if the old parameter is a quantity (length, real, anything, etc.) and the new parameter is the
 * same type of quantity or an isAnything.
 */
 export function copyParameter(parameterId is string) returns CopyParameter
 {
    return parameterId as CopyParameter;
 }


