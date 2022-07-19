FeatureScript 1803; /* Automatically generated version */
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
 *         annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
 *         definition.shouldFlip is boolean;
 *     }
 *     {
 *         var extrudePlane is Plane = evFaceTangentPlane(context, {
 *                 "face" : definition.faces,
 *                 "parameter" : vector(0.5, 0.5)
 *         });
 *         var extrudeManipulator is Manipulator = linearManipulator({
 *                 "base" : extrudePlane.origin,
 *                 "direction" : extrudePlane.normal,
 *                 "offset" : definition.shouldFlip ? definition.depth : -definition.depth,
 *                 "primaryParameterId" : "depth"
 *         });
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
export import(path : "onshape/std/math.fs", version : "1803.0");
export import(path : "onshape/std/manipulatorstyleenum.gen.fs", version : "1803.0");
export import(path : "onshape/std/manipulatortype.gen.fs", version : "1803.0");

import(path : "onshape/std/context.fs", version : "1803.0");
import(path : "onshape/std/feature.fs", version : "1803.0");
import(path : "onshape/std/mathUtils.fs", version : "1803.0");
import(path : "onshape/std/valueBounds.fs", version : "1803.0");
import(path : "onshape/std/evaluate.fs", version : "1803.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1803.0");

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
    value.primaryParameterId is undefined || value.primaryParameterId is string;
    //Anything else is up to the specific kind
}

/*
 * Useful documentation for `sources` parameter once BEL-122076 is fixed:
 *     @field sources {Query} : @optional
 *          If a query for entities is passed, creates a shadow of these
 *          entities and renders that shadow rigidly locked to the manipulator
 *          (such that the shadow will move before the feature finishes
 *          regenerating).
 */

/**
 * Create a manipulator represented by a triad of perpendicular arrows, aligned with
 * the world axes, which specify a 3D position. See `transformCopy` for an
 * example.
 *
 * @param definition {{
 *      @field base : The position of the manipulator when the offset is `0`.
 *      @field offset : The 3D position of the triad, relative to the `base`.
 *      @field sources : @optional For Onshape internal use.
 * }}
 */
export function triadManipulator(definition is map) returns Manipulator
precondition
{
    is3dLengthVector(definition.base);
    is3dLengthVector(definition.offset);
    definition.sources == undefined || definition.sources is Query; // BEL-122076: This only works for extrude and revolve.
}
{
    definition.manipulatorType = ManipulatorType.LINEAR_3D;
    return definition as Manipulator;
}

// ----- Deprecated triadManipulator overloads -----

annotation { "Deprecated" : "Use [triadManipulator(map)]" }
export function triadManipulator(base is Vector, offset is Vector, sources) returns Manipulator
{
    return triadManipulator({
                "base" : base,
                "offset" : offset,
                "sources" : sources,
                "style" : ManipulatorStyleEnum.DEFAULT
            }) as Manipulator;
}

// -------------------------------------------------

/**
 * Create a manipulator represented by a single arrow which can move along a single axis. See `extrude` for an example.
 *
 * @param definition {{
 *      @field base : The position of the manipulator when the offset is `0`.
 *      @field direction : A 3D unit vector pointing on the axis on which the manipulator can be dragged.
 *      @field offset : The positive or negative distance along `direction` from the base to the manipulator.
 *      @field sources : @optional For Onshape internal use.
 *      @field minValue {ValueWithUnits} : @optional The minimum offset allowed.
 *      @field maxValue {ValueWithUnits} : @optional The maximum offset allowed.
 *      @field style {ManipulatorStyleEnum} : @optional
 *      @field primaryParameterId {string} : @optional The id of the `definition` field which is being manipulated.
 *          When set, the feature dialog focus will be shifted to the parameter in question when the manipulator is manipulated.
 * }}
 */
export function linearManipulator(definition is map) returns Manipulator
precondition
{
    is3dLengthVector(definition.base);
    is3dDirection(definition.direction);
    isLength(definition.offset);
    definition.sources == undefined || definition.sources is Query; // BEL-122076: This only works for extrude and revolve.
    definition.minValue == undefined || isLength(definition.minValue);
    definition.maxValue == undefined || isLength(definition.maxValue);
    definition.style == undefined || definition.style is ManipulatorStyleEnum;
    definition.primaryParameterId == undefined || definition.primaryParameterId is string;
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

// ----- Deprecated linearManipulator overloads -----

annotation { "Deprecated" : "Use [linearManipulator(map)]" }
export function linearManipulator(base is Vector, direction is Vector, offset is ValueWithUnits, sources, style is ManipulatorStyleEnum) returns Manipulator
{
    return linearManipulator({
                "base" : base,
                "direction" : direction,
                "offset" : offset,
                "sources" : sources,
                "style" : style
            });
}

annotation { "Deprecated" : "Use [linearManipulator(map)]" }
export function linearManipulator(base is Vector, direction is Vector, offset is ValueWithUnits, sources) returns Manipulator
{
    return linearManipulator({
                "base" : base,
                "direction" : direction,
                "offset" : offset,
                "sources" : sources,
                "style" : ManipulatorStyleEnum.DEFAULT
            });
}

annotation { "Deprecated" : "Use [linearManipulator(map)]" }
export function linearManipulator(base is Vector, direction is Vector, offset is ValueWithUnits) returns Manipulator
{
    return linearManipulator({
                "base" : base,
                "direction" : direction,
                "offset" : offset,
                "style" : ManipulatorStyleEnum.DEFAULT
            });
}

// --------------------------------------------------

/**
 * Create a manipulator represented by a curved arrow which can move along a circumference to specify an angle,
 * with the start and end of the rotation angle delimited by radial lines. See `revolve` for an example.
 *
 * @param definition {{
 *      @field axisOrigin {Vector} : The origin of the axis to rotate around.
 *              @eg `project(axis, rotationOrigin)`
 *      @field axisDirection {Vector} : The direction of the axis to rotate around.
 *              @eg `axis.direction`
 *      @field rotationOrigin {Vector} : Point at the tip of the revolve manipulator.
 *      @field angle {ValueWithUnits}  : Current angle value of the manipulator.
 *      @field sources : @optional For Onshape internal use.
 *      @field minValue {ValueWithUnits} : @optional The minimum angle allowed.
 *      @field maxValue {ValueWithUnits} : @optional The maximum angle allowed.
 *      @field style {ManipulatorStyleEnum} : @optional
 *      @field primaryParameterId {string} : @optional The id of the `definition` field which is being manipulated.
 *          When set, the feature dialog focus will be shifted to the parameter in question when the manipulator is manipulated.
 *      @field disableMinimumOffset {boolean} : @optional Removes the minimum offset between the arrow and `axisOrigin`.
 * }}
 */
export function angularManipulator(definition is map) returns Manipulator
precondition
{
    is3dLengthVector(definition.axisOrigin);
    is3dDirection(definition.axisDirection);
    is3dLengthVector(definition.rotationOrigin);
    isAngle(definition.angle);
    definition.sources == undefined || definition.sources is Query; // BEL-122076: This only works for extrude and revolve.
    definition.minValue == undefined || isAngle(definition.minValue);
    definition.maxValue == undefined || isAngle(definition.maxValue);
    definition.style == undefined || definition.style is ManipulatorStyleEnum;
    definition.primaryParameterId == undefined || definition.primaryParameterId is string;
    definition.disableMinimumOffset == undefined || definition.disableMinimumOffset is boolean;
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
 * A set of points which can be selected one at a time.
 *
 * @param definition {{
 *      @field points {array} : Array of 3d locations for points
 *      @field index {number} : The index of the currently selected point
 * }}
 */
export function pointsManipulator(definition is map) returns Manipulator
precondition
{
    definition.points is array;
    for (var entry in definition.points)
    {
        is3dLengthVector(entry);
    }
    definition.index is number;
}
{
    definition.manipulatorType = ManipulatorType.POINTS;
    return definition as Manipulator;
}

/**
 * Create a manipulator represented by a single arrow which flips direction when clicked.
 * @param definition {{
 *      @field base {Vector}: A 3d point at the manipulator's origin
 *              @eg `vector(0, 0, 0) * meter`
 *      @field direction {Vector}: A 3d vector pointing in the unflipped direction
 *              @eg `vector(0, 0, 1)` points manipulator along the z axis
 *      @field flipped {boolean}:
 *              @eg `false` points the manipulator along +direction
 *              @eg `true`  points the manipulator along -direction, or otherDirection if defined
 *      @field sources : @optional For Onshape internal use.
 *      @field style {ManipulatorStyleEnum} : @optional
 *      @field otherDirection {Vector} : @optional A 3d vector for the flipped direction
 * }}
 */
export function flipManipulator(definition is map) returns Manipulator
precondition
{
    is3dLengthVector(definition.base);
    is3dDirection(definition.direction);
    definition.flipped is boolean;
    definition.sources == undefined || definition.sources is Query; // BEL-122076: This only works for extrude and revolve.
    definition.style == undefined || definition.style is ManipulatorStyleEnum;
    definition.otherDirection == undefined || is3dDirection(definition.otherDirection);
}
{
    definition.manipulatorType = ManipulatorType.FLIP;
    return definition as Manipulator;
}

// ----- Deprecated flipManipulator overloads -----

annotation { "Deprecated" : "Use [flipManipulator(map)]" }
export function flipManipulator(base is Vector, direction is Vector, flipped is boolean, sources, style is ManipulatorStyleEnum) returns Manipulator
{
    return flipManipulator({
                "base" : base,
                "direction" : direction,
                "flipped" : flipped,
                "sources" : sources,
                "style" : style
            });
}

annotation { "Deprecated" : "Use [flipManipulator(map)]" }
export function flipManipulator(base is Vector, direction is Vector, flipped is boolean, sources) returns Manipulator
{
    return flipManipulator({
                "base" : base,
                "direction" : direction,
                "flipped" : flipped,
                "sources" : sources,
                "style" : ManipulatorStyleEnum.DEFAULT
            });
}

annotation { "Deprecated" : "Use [flipManipulator(map)]" }
export function flipManipulator(base is Vector, direction is Vector, flipped is boolean) returns Manipulator
{
    return flipManipulator({
                "base" : base,
                "direction" : direction,
                "flipped" : flipped,
                "style" : ManipulatorStyleEnum.DEFAULT
            });
}

// ------------------------------------------------

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
    // First clear out any unchanged entries, so we don't have to eval queries for them
    for (var newEntry in newDefinition)
    {
        if (newEntry.value == oldDefinition[newEntry.key])
        {
            oldDefinition[newEntry.key] = undefined;
            newDefinition[newEntry.key] = undefined;
        }
    }
    // Now evaluate queries
    newDefinition = evaluateQueries(context, newDefinition);
    oldDefinition = evaluateQueries(context, oldDefinition);
    // And finally, compare
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
    return result;
}

function evaluateQueries(context is Context, definition is map) returns map
{
    for (var entry in definition)
    {
        if (entry.value is Query)
        {
            definition[entry.key] = qUnion(evaluateQuery(context, entry.value));
        }
        else if (entry.value is array)
        {
            const arraySize = @size(entry.value);
            for (var i = 0; i < arraySize; i += 1)
            {
                if (entry.value[i] is map)
                {
                    for (var itemEntry in entry.value[i])
                    {
                        if (itemEntry.value is Query)
                        {
                            definition[entry.key][i][itemEntry.key] = qUnion(evaluateQuery(context, itemEntry.value));
                        }
                    }
                }
            }
        }
    }
    return definition;
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

/**
 * @internal
 *
 * If a manipulator needs to be placed on a face that could be mesh, we need to preprocess the parameters
 * before computing the tangent plane.
 */
 export function getDirectEditManipulatorPlane(context is Context, face is Query) returns Plane
 {
    var parameter;
    if (isQueryEmpty(context, qMeshGeometryFilter(face, MeshGeometry.YES)))
    {
        parameter = vector(0.5, 0.5);
    }
    else
    {
        const approximateCentroid = evApproximateCentroid(context, {"entities" : face});
        const distanceResult = evDistance(context, {
                    "side0" : face,
                    "side1" : approximateCentroid
        });
        parameter = distanceResult.sides[0].parameter;
    }
    return evFaceTangentPlane(context, { "face" : face, "parameter" : parameter });
 }
