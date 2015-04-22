export import(path: "onshape/std/geomUtils.fs", version : "");
export import(path: "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");
export import(path : "onshape/std/manipulator.fs", version : "");

annotation {"Feature Type Name" : "Replace face", "Manipulator Change Function" : "manipulatorChanged" }
export const replaceFace = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation {"Name" : "Faces to replace",
                    "UIHint" : "SHOW_CREATE_SELECTION",
                    "Filter": (EntityType.FACE) && ConstructionObject.NO && SketchObject.NO }
        definition.replaceFaces is Query;

        annotation {"Name" : "Surface to replace with", "Filter" : EntityType.FACE, "MaxNumberOfPicks" : 1}
        definition.templateFace is Query;

        // oppositeSense is the sense between the template surface and its face, used to define what sense to use in the
        // face being replaced. (e.g to determine if the outside or inside of a cylindrical surface is to be used as template)
        // Basically, if oppositeSense is false, we use the same sense as the template face, so the normal of the face
        // will point in the same direction as the template. If oppositeSense is true it will point in opposite direction
        annotation {"Name" : "Flip alignment", "Default" : false}
        definition.oppositeSense is boolean;

        annotation {"Name" : "Offset distance"}
        isLength(definition.offset, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

        annotation {"Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION"}
        definition.oppositeDirection is boolean;
    }
    //============================ Body =============================
    {
        if(definition.oppositeDirection)
            definition.offset = -definition.offset;

        // Only draw the offset manipulator if the replace face is defined
        var replaceFacePlane = computeFacePlane(context, id, definition.replaceFaces);
        if(replaceFacePlane != undefined)
            addOffsetManipulator(context, id, definition, replaceFacePlane);

        opReplaceFace(context, id, definition);
    }, { oppositeSense : false, oppositeDirection : false });

//======================= Manipulators ==========================

const OFFSET_MANIPULATOR = "offsetManipulator";

function addOffsetManipulator(context is Context, id is Id, replaceFaceDefinition is map, replaceFace is Plane)
{
    // Don't try anything fancy to guess where to place the manipulator. With replace face it is too hard to guess what the
    // result will look like. Place the manipulator offset from the face to be replaced.  This will allow
    // the manipulator to travel relative to the face where the replacement happens.
    var offsetOrigin = replaceFace.origin;
    var offsetDirection = replaceFace.normal;
    if (replaceFaceDefinition.oppositeSense)
        offsetDirection = -offsetDirection;

    addManipulators(context, id, { (OFFSET_MANIPULATOR) :
                                   linearManipulator(offsetOrigin, offsetDirection, replaceFaceDefinition.offset) });

}

function computeFacePlane(context is Context, id is Id, entitiesQuery is Query)
{
    return evFaceTangentPlane(context, {"face" : entitiesQuery, "parameter" : vector(0.5, 0.5)}).result;
}

export function manipulatorChanged(context is Context, replaceFaceDefinition is map, newManipulators is map) returns map
precondition
{
}
{
    if (newManipulators[OFFSET_MANIPULATOR] != undefined)
    {
      replaceFaceDefinition.oppositeDirection = newManipulators[OFFSET_MANIPULATOR].offset < 0 * meter;
      replaceFaceDefinition.offset = abs(newManipulators[OFFSET_MANIPULATOR].offset);
    }

    return replaceFaceDefinition;
}

