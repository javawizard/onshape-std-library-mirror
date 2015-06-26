FeatureScript 156; /* Automatically generated version */
export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/manipulatorstyleenum.gen.fs", version : "");

export enum ManipulatorType
{
    LINEAR_3D,
    LINEAR_1D,
    ANGULAR,
    FLIP
}

export type Manipulator typecheck canBeManipulator;

export predicate canBeManipulator(value)
{
    value is map;
    value.manipulatorType is ManipulatorType;
    value.style is undefined || value.style is ManipulatorStyleEnum;
    //Anything else is up to the specific kind
}

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


export function linearManipulator(base is Vector, direction is Vector, offset is ValueWithUnits) returns Manipulator
{
    return linearManipulator(base, direction, offset, undefined);
}

export function linearManipulator(base is Vector, direction is Vector, offset is ValueWithUnits, sources) returns Manipulator
{
    return linearManipulator(base, direction, offset, sources, ManipulatorStyleEnum.DEFAULT);
}

export function linearManipulator(base is Vector, direction is Vector, offset is ValueWithUnits, sources, style is ManipulatorStyleEnum) returns Manipulator
precondition
{
    is3dLengthVector(base);
    is3dDirection(direction);
    isLength(offset);
    sources == undefined || sources is Query;
}
{
    return { "manipulatorType" : ManipulatorType.LINEAR_1D,
             "base" : base,
             "direction" : direction,
             "offset" : offset,
             "sources" : sources,
             "style" : style } as Manipulator;
}

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

export function flipManipulator(base is Vector, direction is Vector, flipped is boolean) returns Manipulator
{
    return flipManipulator(base, direction, flipped, undefined);
}

export function flipManipulator(base is Vector, direction is Vector, flipped is boolean, sources) returns Manipulator
{
    return flipManipulator(base, direction, flipped, sources, ManipulatorStyleEnum.DEFAULT);
}

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

