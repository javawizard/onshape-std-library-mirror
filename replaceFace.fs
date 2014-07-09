export import(path: "onshape/std/geomUtils.fs", version : "");
export import(path: "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");
export import(path : "onshape/std/manipulator.fs", version : "");

annotation {"Feature Type Name" : "Replace face", "Manipulator Change Function" : "manipulatorChanged" }
export function replaceFace(context is Context, id is Id, definition is map)
precondition
{
    annotation {"Name" : "Faces to replace",
                "UIHint" : "ShowCreateSelection",
                "Filter": (EntityType.FACE) && ConstructionObject.NO && SketchObject.NO }
    definition.replaceFaces is Query;

    annotation {"Name" : "Surface to replace with", "Filter" : EntityType.FACE, "MaxNumberOfPicks" : 1}
    definition.templateFace is Query;

    // oppositeSense is the sense between the template surface and its face, used to define what sense to use in the
    // face being replaced. (e.g to determine if the outside or inside of a cylindrical surface is to be used as template)
    // Basically, if oppositeSense is false, we use the same sense as the template face, so the normal of the face
    // will point in the same direction as the template. If oppositeSense is true it will point in opposite direction
    if(definition.oppositeSense != undefined)
    {
        annotation {"Name" : "Flip alignment", "Default" : false}
        definition.oppositeSense is boolean;
    }

    annotation {"Name" : "Offset distance"}
    isLength(definition.offset, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

    if(definition.oppositeDirection != undefined)
    {
        annotation {"Name" : "Opposite direction", "UIHint" : "OppositeDirection"}
        definition.oppositeDirection is boolean;
    }
}
//============================ Body =============================
{
    startFeature(context, id, definition);

    if(definition.oppositeDirection)
        definition.offset = -definition.offset;

    if(definition.oppositeSense == undefined)
        definition.oppositeSense = false;

    // We can always draw the opposite sense manipulator, as long as the template face is defined
    var templateFacePlane = computeFacePlane(context, id, definition.templateFace);
    if(templateFacePlane != undefined)
        addOppositeSenseManipulator(context, id, definition, templateFacePlane);

    opReplaceFace(context, id, definition);

    // Only draw the offset manipulator if both the replaced face and the template face are defined
    var replacedFacePlane = computeFacePlane(context, id, definition.replaceFaces);
    if(replacedFacePlane != undefined && templateFacePlane != undefined)
        addOffsetManipulator(context, id, definition, replacedFacePlane);

    endFeature(context, id);
}

//======================= Manipulators ==========================

const OFFSET_MANIPULATOR = "offsetManipulator";
const FLIP_MANIPULATOR = "flipManipulator";

function addOppositeSenseManipulator(context is Context, id is Id, replaceFaceDefinition is map, templateFacePlane is Plane)
{
    addManipulators(context, id, { (FLIP_MANIPULATOR) :
                                   flipManipulator(templateFacePlane.origin,
                                                   templateFacePlane.normal,
                                                   replaceFaceDefinition.oppositeSense, replaceFaceDefinition.templateFace) });
}

function addOffsetManipulator(context is Context, id is Id, replaceFaceDefinition is map, replacedFace is Plane)
{

    var offsetOrigin;
    var offsetDirection = replacedFace.normal;

    if (!featureHasError(context, id))
    {
        // If feature was successful, then use the origin and normal of the post replace face to compute an origin
        // that travels back along the face's normal by the offset amount.  This will keep the manipulator fixed to the
        // result face.

        // If the opposite sense is enabled, the normal of the manipulator needs to be flipped. This keeps the measurement
        // correct relative to the template face.
        if (replaceFaceDefinition.oppositeSense)
          offsetDirection = -offsetDirection;
        offsetOrigin = replacedFace.origin - offsetDirection * replaceFaceDefinition.offset;
    }
    else
    {
        // If feature was unsuccessful, place the manipulator offset from the replaced face.  This will allow
        // the manipulator to travel relative to the face so that the user can search for a valid value.
        offsetOrigin = replacedFace.origin;
        if (replaceFaceDefinition.oppositeSense)
            offsetDirection = -offsetDirection;
    }

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
    if (newManipulators[FLIP_MANIPULATOR] != undefined)
    {
      replaceFaceDefinition.oppositeSense = newManipulators[FLIP_MANIPULATOR].flipped;
    }

    return replaceFaceDefinition;
}

