export import(path : "onshape/std/geomUtils.fs", version : "");

export enum ManipulatorType
{
    LINEAR_1D,
    ANGULAR,
    FLIP
}

export type Manipulator typecheck canBeManipulator;

export predicate canBeManipulator(value)
{
    value is map;
    value.manipulatorType is ManipulatorType;
    //Anything else is up to the specific kind
}

export function linearManipulator(base is Vector, direction is Vector, offset is ValueWithUnits) returns Manipulator
precondition
{
    is3dLengthVector(base);
    is3dDirection(direction);
    isLength(offset);
}
{
    return stripUnits({ "manipulatorType" : ManipulatorType.LINEAR_1D,
                        "base" : base,
                        "direction" : direction,
                        "offset" : offset}) as Manipulator;
}

export function linearManipulatorWithSources(base is Vector, direction is Vector, offset is ValueWithUnits, sources is Query) returns Manipulator
precondition
{
    is3dLengthVector(base);
    is3dDirection(direction);
    isLength(offset);
}
{
    return stripUnits({ "manipulatorType" : ManipulatorType.LINEAR_1D,
                        "base" : base,
                        "direction" : direction,
                        "offset" : offset,
                        "sources" : sources }) as Manipulator;
}

export function angularManipulator(axisOrigin is Vector,
                                   axisDirection is Vector,
                                   rotationOrigin is Vector,
                                   angle is ValueWithUnits) returns Manipulator
precondition
{
    is3dLengthVector(axisOrigin);
    is3dDirection(axisDirection);
    is3dLengthVector(rotationOrigin);
    isAngle(angle);
}
{
    return stripUnits({ "manipulatorType" : ManipulatorType.ANGULAR,
                        "axisOrigin" : axisOrigin, "axisDirection" : axisDirection,
                        "rotationOrigin" : rotationOrigin, "angle" : angle }) as Manipulator;
}

export function angularManipulator(axisOrigin is Vector,
                                   axisDirection is Vector,
                                   rotationOrigin is Vector,
                                   angle is ValueWithUnits,
                                   sources is Query) returns Manipulator
precondition
{
    is3dLengthVector(axisOrigin);
    is3dDirection(axisDirection);
    is3dLengthVector(rotationOrigin);
    isAngle(angle);
}
{
    return stripUnits({ "manipulatorType" : ManipulatorType.ANGULAR,
                        "axisOrigin" : axisOrigin, "axisDirection" : axisDirection,
                        "rotationOrigin" : rotationOrigin,
                        "angle" : angle,
                        "sources" : sources }) as Manipulator;
}

export function flipManipulator(base is Vector, direction is Vector, flipped is boolean, entities is Query) returns Manipulator
precondition
{
    is3dLengthVector(base);
    is3dDirection(direction);
}
{
    return stripUnits({ "manipulatorType" : ManipulatorType.FLIP,
                        "base" : base,
                        "direction" : direction,
                        "flipped" : flipped,
                        "sources" : entities }) as Manipulator;
}

export function addManipulators(context is Context, id is Id, manipulators is map)
precondition
{
    for(var entry in manipulators)
    {
        entry.key is string;
        entry.value is Manipulator;
    }
}
{
    @addManipulators(context, id, manipulators);
}

//Returns a map of changed fields.
export function processDefinitionDifference(oldDefinition is map, newDefinition is map) returns map

{
    var result = {};
    for(var newEntry in newDefinition)
    {
        if(newEntry.value != oldDefinition[newEntry.key])
            result[newEntry.key] = newEntry.value;
    }
    for(var oldEntry in oldDefinition)
    {
        if(newDefinition[oldEntry.key] == undefined)
            //We have to distinguish removal from no change, so we use [undefined] to indicate parameter removal
            result[oldEntry.key] = [undefined];
    }
    //TODO: if we need queries, make this function take a context and evaluate the new ones.

    return result;
}

