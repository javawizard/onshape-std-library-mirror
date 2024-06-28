FeatureScript 2399; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "2399.0");
import(path : "onshape/std/containers.fs", version : "2399.0");
import(path : "onshape/std/context.fs", version : "2399.0");
import(path : "onshape/std/coordSystem.fs", version : "2399.0");
import(path : "onshape/std/imagemappingtype.gen.fs", version : "2399.0");
import(path : "onshape/std/math.fs", version : "2399.0");
import(path : "onshape/std/persistentCoordSystem.fs", version : "2399.0");
import(path : "onshape/std/query.fs", version : "2399.0");
import(path : "onshape/std/string.fs", version : "2399.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2399.0");
import(path : "onshape/std/tabReferences.fs", version : "2399.0");
import(path : "onshape/std/transformUV.fs", version : "2399.0");
import(path : "onshape/std/units.fs", version : "2399.0");
import(path : "onshape/std/vector.fs", version : "2399.0");

/**
 * Data representing a decal that is mapped onto a face.
 *
 * @type {{
 *      @field decalId {Id} : A unique id to represent the decal in the context of the face on which it is placed.
 *          The id should correspond to the id of the creating feature, or be a sub-id of the creating feature.
 *      @field imageMappingType {ImageMappingType} : The type of projection mapping to use for this decal
 *      @field image {ImageData} : The image that is being mapped.
 *      @field uvTransform {TransformUV} : A post-projection transformation that is applied in UV space.
 *          This can be used to further translate, rotate, and scale a projected image.
 *
 *      @field planeSystem {PersistentCoordSystem} : @optional A coordinate system representing the plane for the ImageMappingType.PLANAR type.
 *          This field must be defined if the `imageMappingType` field is of type `ImageMappingType.PLANAR`.
 *          The center of the image will project to the plane's origin.
 *          The right edge of the image is along the positive X direction of the coordinate system.
 *          The top edge of the image is along the positive Y direction of the coordinate system.
 *          See [planeToCSys] for deriving a coordinate system from a [Plane] object.

 *      @field cylinder {Cylinder} : @optional The cylinder onto which the decal is mapped.
 *          This field must be defined if the `imageMappingType` field is of type `ImageMappingType.CYLINDRICAL`.
 *      @field cylinderSystem {PersistentCoordSystem} : @optional A coordinate system used in projecting the image onto the given cylinder.
 *          This field must be defined if the `imageMappingType` field is of type `ImageMappingType.CYLINDRICAL`.
 *          The coordinate system's origin must lie on the cylinder's axis with the system's Z axis aligned with the cylinder's own axis.
 *          The center of the horizontal extents of the image coincide with the intersection of the coordinate system's x axis and the cylinder.
 *          The center of the vertical extents of the image coincide with the projection of the coordinate system's origin on the cylinder.
 *          The top edge of the image is along the positive Z direction of the coordinate system projected on the cylinder.
 * }}
 */
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

/**
 * Creates data for a planar decal.  This can be applied to a face using [associateDecalAttribute].
 *
 * @seealso [DecalData]
 *
 * @param decalId : The id to for the decal
 * @param image : The image to use for the data
 * @param planeSystem : The coordinate system to use for the planar projection
 * @param uvTransform : A post-projection transform to apply to the decal
 */
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

/**
 * Creates data for a cylindrical decal.  This can be applied to a face using [associateDecalAttribute].
 *
 * @seealso [DecalData]
 *
 * @param decalId : The id to for the decal
 * @param image : The image to use for the data
 * @param cylinder : The cylinder definition to use for projection
 * @param uvTransform : A post-projection transform to apply to the decal
 */
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

/**
 * Associate the given decal data as an attribute on the entities provided.
 * This will append the decal to any existing decals associated with the
 * given entities.
 *
 * Associating a decal in this way will cause the data to be transmitted
 * to Onshape clients where they will be rendered.
 */
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

/**
 * Creates a UV transform suitable for scaling, mirroring, and rotating a decal after it's been projected.
 *
 * @param decalWidth : The width of the decal, post-transformation
 * @param mirrorHorizontal : If true, the image will be mirrored about its center horizontally
 * @param decalHeight : The height of the decal, post-transformation
 * @param mirrorVertical : If true, the image will be mirrored about its center vertically
 * @param decalRotation : An amount of rotation to apply to decal about the image center.
 */
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

/**
 * Unprojects the given point in UV space to its corresponding world position
 * for the given decal data. The UV is equivalent to the texture coordinate for
 * the UV that is ultimately used to render the decal.
 * @seeAlso [getDecalUvSpacePosition]
 */
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
 * Projects the given world position into UV space for the given decal data.
 * The UV computed is equivalent to the texture coordinate for the UV that
 * is ultimately used to render the decal.
 * @seeAlso [getWorldSpacePosition]
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

