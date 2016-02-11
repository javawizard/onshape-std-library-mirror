FeatureScript 307; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/* Calls to vector(...) and abs(...) are generated by the server,
   so those functions need to be exported. */
export import(path : "onshape/std/math.fs", version : "");
export import(path : "onshape/std/manipulatorstyleenum.gen.fs", version : "");
export import(path : "onshape/std/manipulatortype.gen.fs", version : "");

import(path : "onshape/std/context.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/mathUtils.fs", version : "");
import(path : "onshape/std/valueBounds.fs", version : "");

/**
 * TODO: description
 */
export type Manipulator typecheck canBeManipulator;

export predicate canBeManipulator(value)
{
    value is map;
    value.manipulatorType is ManipulatorType;
    value.style is undefined || value.style is ManipulatorStyleEnum;
    //Anything else is up to the specific kind
}

/**
 * TODO: description
 * @param base
 * @param offset
 * @param sources
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
 * TODO: description
 * @param base
 * @param direction
 * @param offset
 */
export function linearManipulator(base is Vector, direction is Vector, offset is ValueWithUnits) returns Manipulator
{
    return linearManipulator(base, direction, offset, undefined);
}

export function linearManipulator(base is Vector, direction is Vector, offset is ValueWithUnits, sources) returns Manipulator
{
    return linearManipulator(base, direction, offset, sources, ManipulatorStyleEnum.DEFAULT);
}

export function linearManipulator(base is Vector, direction is Vector, offset is ValueWithUnits, sources, style is ManipulatorStyleEnum) returns Manipulator
{
    return linearManipulator({ "base" : base,
                               "direction" : direction,
                               "offset" : offset,
                               "sources" : sources,
                               "style" : style });
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
        definition.minValue = -PLANE_SIZE_BOUNDS.max;
    }
    if (definition.maxValue == undefined)
    {
        definition.maxValue = PLANE_SIZE_BOUNDS.max;
    }
    return definition as Manipulator;
}

/**
 * TODO: description
 * @param definition {{
 *      @field TODO
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
 * TODO: description
 * @param base
 * @param direction
 * @param flipped
 */
export function flipManipulator(base is Vector, direction is Vector, flipped is boolean) returns Manipulator
{
    return flipManipulator(base, direction, flipped, undefined);
}

export function flipManipulator(base is Vector, direction is Vector, flipped is boolean, sources) returns Manipulator
{
    return flipManipulator(base, direction, flipped, sources, ManipulatorStyleEnum.DEFAULT);
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
 * @param sources {Query}: entities the manipulator will flip @optional
 * @param style {ManipulatorStyleEnum}: @optional Graphical appearance of the manipulator.
 *      @eg `ManipulatorStyleEnum.DEFAULT`
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

/**
 * TODO: description
 * @param context
 * @param id
 * @param manipulators {{
 *      @field TODO
 * }}
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

//Returns a map of changed fields.
/**
 * TODO: description
 * @param oldDefinition {{
 *      @field TODO
 * }}
 * @param newDefinition {{
 *      @field TODO
 * }}
 */
export function processDefinitionDifference(oldDefinition is map, newDefinition is map) returns map

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
    //TODO: if we need queries, make this function take a context and evaluate the new ones.

    return result;
}

