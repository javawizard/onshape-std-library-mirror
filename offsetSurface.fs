FeatureScript 2144; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2144.0");
export import(path : "onshape/std/context.fs", version : "2144.0");
export import(path : "onshape/std/manipulator.fs", version : "2144.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "2144.0");
import(path : "onshape/std/evaluate.fs", version : "2144.0");
import(path : "onshape/std/feature.fs", version : "2144.0");
import(path : "onshape/std/geomOperations.fs", version : "2144.0");
import(path : "onshape/std/mathUtils.fs", version : "2144.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2144.0");
import(path : "onshape/std/topologyUtils.fs", version : "2144.0");
import(path : "onshape/std/valueBounds.fs", version : "2144.0");
import(path : "onshape/std/vector.fs", version : "2144.0");


/**
 * Feature performing an [opExtractSurface]. Allows creation of an offset surface from faces, surfaces, or sketch regions.
 * Offset may be zero. Offset direction may be flipped using the oppositeDirection flag.
 */
annotation { "Feature Type Name" : "Offset surface" ,
             "Manipulator Change Function" : "offsetSurfaceManipulatorChange",
             "Filter Selector" : "allparts"}
export const offsetSurface = defineFeature(function(context is Context, id is Id, definition is map)
precondition
{
    annotation { "Name" : "Faces, surfaces, and sketch regions to offset", "UIHint" : UIHint.SHOW_CREATE_SELECTION,
                 "Filter" : (EntityType.FACE || (BodyType.SHEET && EntityType.BODY)) && ConstructionObject.NO && AllowMeshGeometry.YES }
    definition.surfacesAndFaces is Query;

    annotation { "Name" : "Offset", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
    isLength(definition.offset, ZERO_INCLUSIVE_OFFSET_BOUNDS);

    annotation { "Name" : "Opposite offset direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
    definition.oppositeDirection is boolean;
}
{
    var qSurfacesAndFaces = qUnion([qEntityFilter(definition.surfacesAndFaces, EntityType.FACE),
        qOwnedByBody(qEntityFilter(definition.surfacesAndFaces, EntityType.BODY), EntityType.FACE)]);

    var remainingTransform = getRemainderPatternTransform(context,
            {"references" : qSurfacesAndFaces});

    const resolvedEntities = evaluateQuery(context, qSurfacesAndFaces);
    if (resolvedEntities == [])
        throw regenError(ErrorStringEnum.DIRECT_EDIT_MOVE_FACE_CREATE_SELECT, ["surfacesAndFaces"]);

    definition.offset = definition.offset * (definition.oppositeDirection ? -1 : 1);

    // Extract an axis defined by the offset face for use in the manipulators.
    // We want our manipulator to be on the last selected non-mesh face if available.
    // Otherwise it will be on the last selected face.
    var lastFace = getLastNonMeshFace(context, qUnion(resolvedEntities));
    if (isQueryEmpty(context, lastFace))
    {
        lastFace = resolvedEntities[size(resolvedEntities) - 1];
    }
    var facePlane = try(getDirectEditManipulatorPlane(context, lastFace));
    if (facePlane == undefined)
        throw regenError(ErrorStringEnum.NO_TANGENT_PLANE, [definition.surfacesAndFaces]);
    addOffsetManipulator(context, id, definition.offset, facePlane);

    opExtractSurface(context, id, {"faces" : qSurfacesAndFaces, "offset" : definition.offset,
            "useFacesAroundToTrimOffset" : false });

    transformResultIfNecessary(context, id, remainingTransform);

}, { "oppositeDirection" : false });

const OFFSET_MANIPULATOR = "offsetManipulator";
function addOffsetManipulator(context is Context, id is Id, offsetDistance is ValueWithUnits, facePlane is Plane)
{
    addManipulators(context, id, {
                (OFFSET_MANIPULATOR) : linearManipulator({
                            "base" : facePlane.origin,
                            "direction" : facePlane.normal,
                            "offset" : offsetDistance,
                            "primaryParameterId" : "offset"
                        })
            });
}

/**
 * @internal
 * Manipulator change function for `offsetSurface`.
 */
export function offsetSurfaceManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    const newValue = newManipulators[OFFSET_MANIPULATOR].offset;
    definition.offset = abs(newValue);
    definition.oppositeDirection = newValue < 0;
    return definition;
}


