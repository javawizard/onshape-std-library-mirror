FeatureScript 2241; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "2241.0");
import(path : "onshape/std/containers.fs", version : "2241.0");
import(path : "onshape/std/context.fs", version : "2241.0");
import(path : "onshape/std/coordSystem.fs", version : "2241.0");
import(path : "onshape/std/imagemappingtype.gen.fs", version : "2241.0");
import(path : "onshape/std/math.fs", version : "2241.0");
import(path : "onshape/std/persistentCoordSystem.fs", version : "2241.0");
import(path : "onshape/std/query.fs", version : "2241.0");
import(path : "onshape/std/string.fs", version : "2241.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2241.0");
import(path : "onshape/std/tabReferences.fs", version : "2241.0");
import(path : "onshape/std/transformUV.fs", version : "2241.0");
import(path : "onshape/std/units.fs", version : "2241.0");
import(path : "onshape/std/vector.fs", version : "2241.0");

/** @internal */
export type DecalData typecheck canBeDecalData;

/** @internal */
const DECALS_ATTRIBUTE_NAME = "decals";

/** @internal */
export predicate canBeDecalData(value)
{
    value is map;
    value.decalId is Id;
    value.imageMappingType == undefined || value.imageMappingType is ImageMappingType;
    value.image is ImageData;
    value.uvTransform is TransformUV;
}

/** @internal */
export function createPlanarDecal(decalId is Id,
                                  image is ImageData,
                                  planeSystem is CoordSystem,
                                  uvTransform is TransformUV)
    returns DecalData
{
    return {
        'imageMappingType': ImageMappingType.PLANAR,
        'decalId': decalId,
        'image': image,
        'planeSystem': persistentCoordSystem(planeSystem, toString(decalId + "planeSystem")),
        'uvTransform': uvTransform
    } as DecalData;
}

/** @internal */
export function createCylindricalDecal(decalId is Id,
                                       image is ImageData,
                                       cylinder is Cylinder,
                                       uvTransform is TransformUV)
    returns DecalData
{
    return {
        'imageMappingType': ImageMappingType.CYLINDRICAL,
        'decalId': decalId,
        'image': image,
        'cylinder': cylinder,
        'cylinderSystem': persistentCoordSystem(cylinder.coordSystem, toString(decalId + "cylinderSystem")),
        'uvTransform': uvTransform
    } as DecalData;
}

/** @internal */
export function associateDecalAttribute(context is Context,
                                        entities is Query,
                                        decalData is DecalData)
{
    for (var entity in evaluateQuery(context, entities))
    {
        var decalArray = getAttribute(context, {
            "entity" : entity,
            "name" : DECALS_ATTRIBUTE_NAME
        });

        if (decalArray == undefined)
            decalArray = [];

        decalArray = append(decalArray, decalData);

        setAttribute(context, {
            "entities" : entity,
            "name" : DECALS_ATTRIBUTE_NAME,
            "attribute" : decalArray
        });
    }
}

/** @internal */
export function createUvTransform(decalWidth is ValueWithUnits,
                                  mirrorHorizontal is boolean,
                                  decalHeight is ValueWithUnits,
                                  mirrorVertical is boolean,
                                  decalRotation is ValueWithUnits)
    returns TransformUV
{
    const xMirror = mirrorHorizontal ? -1.0 : 1.0;
    const yMirror = mirrorVertical ? -1.0 : 1.0;
    return scaleNonuniformly(xMirror / (decalWidth / meter), yMirror / (decalHeight / meter)) * rotate(decalRotation);
}

/** @internal */
export function getWorldSpacePosition(decalData is DecalData,
                                      uv is Vector)
precondition
{
    isUvVector(uv);
}
{
    uv = inverse(decalData.uvTransform) * uv;

    if (decalData.imageMappingType == ImageMappingType.PLANAR)
    {
        return toWorld(decalData.planeSystem.coordSystem, vector(uv[0], uv[1], 0) * meter);
    }
    else if (decalData.imageMappingType == ImageMappingType.CYLINDRICAL)
    {
        const theta = getAngleForCylinderArcSegment(decalData.cylinder, uv[0] * meter);

        const coordSystem = decalData.cylinderSystem.coordSystem;
        return coordSystem.origin +
            coordSystem.zAxis * uv[1] * meter +
            coordSystem.xAxis * cos(theta) * decalData.cylinder.radius +
            yAxis(coordSystem) * sin(theta) * decalData.cylinder.radius;
    }

    throw "Unsupported image mapping type";
}

/**
 * @internal
 * Projects the given world position into UV space for the given decal data.
 * The UV computed is equivalent to the texture coordinate for the UV that
 * is ultimately used to render the decal.
 */
export function getDecalUvSpacePosition(decalData is DecalData,
                                        worldPosition is Vector)
precondition
{
    is3dLengthVector(worldPosition);
}
{
    var uv;

    if (decalData.imageMappingType == ImageMappingType.PLANAR)
    {
        const projection = fromWorld(decalData.planeSystem.coordSystem, worldPosition);
        uv = vector(projection[0], projection[1]) / meter;
    }
    else if (decalData.imageMappingType == ImageMappingType.CYLINDRICAL)
    {
        const coordSystem = decalData.cylinderSystem.coordSystem;

        const vectorToCylinderOrigin = worldPosition - coordSystem.origin;
        const directionFromCylinderAxis = normalize(vectorToCylinderOrigin);

        var theta = atan2(dot(yAxis(coordSystem), directionFromCylinderAxis), dot(coordSystem.xAxis, directionFromCylinderAxis));

        // Depending on how the cylinder UV is offset, we need to adjust theta such that
        // it makes a continuous U value as it crosses it crosses PI.
       if (decalData.uvTransform.translation[0] <= 0.0 && theta < 0.0) {
          theta += 2 * PI;
        } else if (decalData.uvTransform.translation[0] >= 1.0 && theta > 0.0) {
          theta -= 2 * PI;
        }

        uv = vector((theta * decalData.cylinder.radius) / radian,
            dot(vectorToCylinderOrigin, coordSystem.zAxis)) / meter;
    }
    else
    {
        throw "Unsupported image mapping type";
    }

    return decalData.uvTransform * uv;
}

/** @internal */
export function getAngleForCylinderArcSegment(cylinder is Cylinder, arcSegmentLength is ValueWithUnits) returns ValueWithUnits
{
    return (arcSegmentLength / cylinder.radius) * radian;
}

