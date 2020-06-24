FeatureScript 1311; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "1311.0");
export import(path : "onshape/std/tool.fs", version : "1311.0");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "1311.0");

// Imports used internally
import(path : "onshape/std/evaluate.fs", version : "1311.0");
import(path : "onshape/std/feature.fs", version : "1311.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1311.0");
import(path : "onshape/std/valueBounds.fs", version : "1311.0");
import(path : "onshape/std/vector.fs", version : "1311.0");
import(path : "onshape/std/string.fs", version : "1311.0");

/**
 * Feature performing an [opReplaceFace].
 */
annotation { "Feature Type Name" : "Replace face", "Manipulator Change Function" : "replaceFaceManipulatorChange", "Filter Selector" : "allparts", "Editing Logic Function" : "replaceFaceEditLogic" }
export const replaceFace = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Faces to replace",
                     "UIHint" : UIHint.SHOW_CREATE_SELECTION,
                     "Filter" : (EntityType.FACE) && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES }
        definition.replaceFaces is Query;

        annotation { "Name" : "Surface to replace with", "Filter" : EntityType.FACE, "MaxNumberOfPicks" : 1 }
        definition.templateFace is Query;

        // oppositeSense is the sense between the template surface and its face, used to define what sense to use in the
        // face being replaced. (e.g to determine if the outside or inside of a cylindrical surface is to be used as template)
        // Basically, if oppositeSense is false, we use the same sense as the template face, so the normal of the face
        // will point in the same direction as the template. If oppositeSense is true it will point in opposite direction
        annotation { "Name" : "Flip alignment", "Default" : false }
        definition.oppositeSense is boolean;

        annotation { "Name" : "Offset distance" }
        isLength(definition.offset, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.oppositeDirection is boolean;
    }
    //============================ Body =============================
    {
        if (definition.oppositeDirection)
            definition.offset = -definition.offset;

        // Only draw the offset manipulator if the replace face is defined
        var templateFacePlane = try(computeFacePlane(context, id, definition.templateFace));
        if (templateFacePlane != undefined)
        {
            if (definition.oppositeSense)
            {
                templateFacePlane.normal = -templateFacePlane.normal;
            }
            addOffsetManipulator(context, id, definition, templateFacePlane);
        }
        opReplaceFace(context, id, definition);
    }, { oppositeSense : false, oppositeDirection : false });

//======================= Manipulators ==========================

const OFFSET_MANIPULATOR = "offsetManipulator";

function addOffsetManipulator(context is Context, id is Id, replaceFaceDefinition is map, replaceFacePlane is Plane)
{
    // Don't try anything fancy to guess where to place the manipulator. With replace face it is too hard to guess what the
    // result will look like. Place the manipulator offset from the template face.  This will allow
    // the manipulator to travel relative to the face where the offset happens.
    const offsetOrigin = replaceFacePlane.origin;
    var offsetDirection = replaceFacePlane.normal;
    if (replaceFaceDefinition.oppositeSense)
        offsetDirection = -offsetDirection;

    addManipulators(context, id, {
                (OFFSET_MANIPULATOR) : linearManipulator({
                            "base" : offsetOrigin,
                            "direction" : offsetDirection,
                            "offset" : replaceFaceDefinition.offset,
                            "primaryParameterId" : "offset"
                        })
            });
}

function computeFacePlane(context is Context, id is Id, entitiesQuery is Query)
{
    return evFaceTangentPlane(context, { "face" : entitiesQuery, "parameter" : vector(0.5, 0.5) });
}

/**
 * @internal
 * Manipulator change function for `replaceFace`.
 */
export function replaceFaceManipulatorChange(context is Context, replaceFaceDefinition is map, newManipulators is map) returns map
{
    if (newManipulators[OFFSET_MANIPULATOR] != undefined)
    {
        replaceFaceDefinition.oppositeDirection = newManipulators[OFFSET_MANIPULATOR].offset < 0 * meter;
        replaceFaceDefinition.offset = abs(newManipulators[OFFSET_MANIPULATOR].offset);
    }

    return replaceFaceDefinition;
}

/**
 * @internal
 * Editing logic funtion for `replaceFace`.
 */
export function replaceFaceEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (specifiedParameters.oppositeSense != true)
    {
        const replaceFacePlane = try(computeFacePlane(context, id, definition.replaceFaces));
        var templateFacePlane = try(computeFacePlane(context, id, definition.templateFace));
        if (replaceFacePlane != undefined && templateFacePlane != undefined)
        {
            definition.oppositeSense = dot(replaceFacePlane.normal, templateFacePlane.normal) < 0;
        }
    }
    return definition;
}

