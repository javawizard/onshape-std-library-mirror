FeatureScript 2752; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/common.fs", version : "2752.0");
import(path : "onshape/std/decalUtils.fs", version : "2752.0");
import(path : "onshape/std/error.fs", version : "2752.0");
import(path : "onshape/std/imagemappingtype.gen.fs", version : "2752.0");
import(path : "onshape/std/mateConnector.fs", version : "2752.0");
import(path : "onshape/std/topologyUtils.fs", version : "2752.0");

const IMAGE_ORIGIN_COLUMN_COUNT = 3;  // low, medium, high

const IMAGE_ORIGIN_POINT_COUNT = IMAGE_ORIGIN_COLUMN_COUNT * IMAGE_ORIGIN_COLUMN_COUNT;

const DEFAULT_IMAGE_ORIGIN_CENTER = 4; // middle row and column in point grid

const IMAGE_ORIGIN_INDEX_BOUNDS =
{
            (unitless) : [0, DEFAULT_IMAGE_ORIGIN_CENTER, IMAGE_ORIGIN_POINT_COUNT - 1]
}
as IntegerBoundSpec;

/** @internal */
export enum ImageAspectRatioConstraint
{
    annotation { "Name" : "Width" }
    WIDTH_DRIVING,
    annotation { "Name" : "Height" }
    HEIGHT_DRIVING
}

const MAX_IMAGE_DIMENSION = 4096;

/**
 * Feature to place and position decal attributes on a surface.
 */
annotation { "Feature Type Name" : "Decal",
             "UIHint" : UIHint.NO_PREVIEW_PROVIDED,
             "Editing Logic Function" : "onDecalFeatureChange",
             "Manipulator Change Function" : "onDecalManipulatorChange" }
/** @internal */
export const decal = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Image" }
        definition.image is ImageData;

        annotation { "Name" : "Face",
            "Filter" : EntityType.FACE && (GeometryType.PLANE || GeometryType.CYLINDER) && SketchObject.NO && ConstructionObject.NO && ModifiableEntityOnly.YES,
            "MaxNumberOfPicks": 1 }
        definition.face is Query;

        annotation { "Name" : "Angle" }
        isAngle(definition.angle, ANGLE_360_ZERO_DEFAULT_BOUNDS);

        annotation { "Name" : "Angle opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION_CIRCULAR, "Default" : false }
        definition.angleOppositeDirection is boolean;

        annotation { "Name" : "U shift" }
        isLength(definition.uShift, IMAGE_OFFSET_BOUNDS);

        annotation { "Name" : "V shift" }
        isLength(definition.vShift, IMAGE_OFFSET_BOUNDS);

        annotation { "Name" : "Realign", "Default" : false }
        definition.realign is boolean;

        if (definition.realign)
        {
            annotation { "Name" : "Placement origin",
                "Filter" : BodyType.MATE_CONNECTOR,
                "MaxNumberOfPicks" : 1 }
            definition.originConnector is Query;

            annotation { "Name" : "Horizontal reference", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
            definition.horizontalReference is Query;
        }

        annotation { "Name" : "Maintain aspect ratio", "Default" : true }
        definition.maintainAspectRatio is boolean;

        if (definition.maintainAspectRatio)
        {
            annotation { "Name" : "Aspect ratio constraint" }
            definition.aspectRatioConstraint is ImageAspectRatioConstraint;
        }

        if (!definition.maintainAspectRatio || definition.aspectRatioConstraint == ImageAspectRatioConstraint.WIDTH_DRIVING)
        {
            annotation { "Name" : "Width" }
            isLength(definition.width, IMAGE_SIZE_BOUNDS);
        }

        if (!definition.maintainAspectRatio || definition.aspectRatioConstraint == ImageAspectRatioConstraint.HEIGHT_DRIVING)
        {
            annotation { "Name" : "Height" }
            isLength(definition.height, IMAGE_SIZE_BOUNDS);
        }

        annotation { "Name" : "Image origin index", "UIHint" : UIHint.ALWAYS_HIDDEN }
        isInteger(definition.imageOriginIndex, IMAGE_ORIGIN_INDEX_BOUNDS);
    }
    {

        if (definition.face == undefined || isQueryEmpty(context, definition.face))
            throw regenError(ErrorStringEnum.DECAL_NO_FACE_SELECTION, ["face"]);

        if (!imageDataIsSpecified(definition.image))
            throw regenError(ErrorStringEnum.DECAL_NO_IMAGE_SELECTION, ["image"]);

        if (definition.image.imageWidth > MAX_IMAGE_DIMENSION || definition.image.imageHeight > MAX_IMAGE_DIMENSION)
            throw regenError(ErrorStringEnum.DECAL_IMAGE_TOO_LARGE, ["image"]);

        var fullTransform = getFullPatternTransform(context);
        if (abs(determinant(fullTransform.linear) + 1) < TOLERANCE.zeroLength) // det == -1
        {
            // We have a reflection on the input body.  Since we can't support attribute coordinate system mirror,
            // we disable it at the feature level for consistency.  We can revisit this if we add support
            // for decal mirror in the future (BEL-215933)
            return;
        }

        definition.angle = definition.angle * (definition.angleOppositeDirection ? -1 : 1);

        if (definition.maintainAspectRatio && definition.aspectRatioConstraint == ImageAspectRatioConstraint.WIDTH_DRIVING)
            definition.height = definition.width / getImageAspectRatio(definition);
        else if (definition.maintainAspectRatio && definition.aspectRatioConstraint == ImageAspectRatioConstraint.HEIGHT_DRIVING)
            definition.width = definition.height * getImageAspectRatio(definition);

        var decalData is DecalData = getDecalDataForDefinition(context, id, definition, true /* computeOrigin */);

        associateDecalAttribute(context, definition.face, decalData);

        setHighlightedEntities(context, { "entities": definition.face });

        addDecalManipulators(context, id, definition, decalData);

        if (!isDecalNearFace(context, definition, decalData))
            throw regenError(ErrorStringEnum.DECAL_PROJECTED_OFF_FACE);
    });

/** @internal */
export function onDecalFeatureChange(context is Context, id is Id, oldDefinition is map, definition is map,
                                     isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (!definitionHasEnoughForDecalDisplay(context, oldDefinition) &&
        definitionHasEnoughForDecalDisplay(context, definition))
    {
        const defaultSize = getDefaultDecalSize(context, id, definition);
        definition.width = defaultSize.width;
        definition.height = defaultSize.height;
    }

    return definition;
}

function getImageAspectRatio(definition is map) returns number
{
    if (!imageDataIsSpecified(definition.image))
        return 1.0;
    else
        return definition.image.imageWidth / definition.image.imageHeight;
}

function definitionHasEnoughForDecalDisplay(context is Context, definition is map) returns boolean
{
    if (definition.face == undefined)
        return false;

    return imageDataIsSpecified(definition.image) && !isQueryEmpty(context, definition.face);
}

function getDefaultDecalSize(context is Context, id is Id, definition is map) returns map
{
    // Get the decal data in order to evaluate the extent of the face in that system.
    // For this we do not need to compute the origin.
    const decalData is DecalData = getDecalDataForDefinition(context, id, definition, false /* computeOrigin */);

    var regionWidth;
    var regionHeight;

    if (decalData.imageMappingType == ImageMappingType.PLANAR)
    {
        const regionBounds is Box3d = evBox3d(
            context, {
                "topology" : definition.face,
                "tight" : true
        });

        // Use the bounds in UV space to determine the projected extents
        const uvBounds = projectBox3d(regionBounds, decalData.planeSystem.coordSystem);

        regionWidth = uvBounds.maxCorner[0] - uvBounds.minCorner[0];
        regionHeight = uvBounds.maxCorner[1] - uvBounds.minCorner[1];
    }
    else if (decalData.imageMappingType == ImageMappingType.CYLINDRICAL)
    {
        const cylinder is Cylinder = decalData.cylinder;
        const projectionInformation = getFaceCylinderProjectionInformation(context, definition, cylinder);

        regionWidth = ((projectionInformation.angleMax - projectionInformation.angleMin) / radian) * cylinder.radius;
        regionHeight = projectionInformation.zMax - projectionInformation.zMin;
    }
    else
        throw "Unsupported image mapping type";

    const aspectRatio = getImageAspectRatio(definition);

    if (regionWidth / aspectRatio > regionHeight)
    {
        return {
            width : regionHeight * aspectRatio,
            height : regionHeight
        };
    }
    else
    {
        return {
            width : regionWidth,
            height : regionWidth / aspectRatio
        };
    }
}

function definitionHasFaceOriginSpecified(context is Context, definition is map) returns boolean
{
    return definition.realign && !isQueryEmpty(context, definition.originConnector);
}

function getDecalDataForDefinition(context is Context, id is Id, definition is map, computeOrigin is boolean) returns DecalData
{
    const isPlanar = !isQueryEmpty(context, qGeometry(definition.face, GeometryType.PLANE));
    const isCylinder = !isQueryEmpty(context, qGeometry(definition.face, GeometryType.CYLINDER));

    var queriesForTransform = [definition.face];

    var horizontalReferenceDirection;
    if (definition.realign)
    {
        horizontalReferenceDirection = extractDirection(context, definition.horizontalReference);
        if (!isQueryEmpty(context, definition.originConnector))
            queriesForTransform = append(queriesForTransform, definition.originConnector);
    }

    var decalData;
    if (isPlanar)
    {
        const zeroUvHorizontalReference = 0 * radian;
        var uvTransform = getUvTransform(context, definition, zeroUvHorizontalReference);

        var selectedOrigin = vector(0, 0, 0) * meter;
        if (computeOrigin)
        {
            if (definitionHasFaceOriginSpecified(context, definition))
            {
                queriesForTransform = append(queriesForTransform, definition.originConnector);
                selectedOrigin = evMateConnector(context, { "mateConnector" : definition.originConnector }).origin;
            }
            else
            {
                selectedOrigin = evMateConnectorCoordSystem(context, {
                    "originQuery" : definition.face,
                    "entityInferenceType" : EntityInferenceType.CENTROID,
                    "requireOwnerPart" : false
                }).origin;
            }
        }

        var planeSystem is CoordSystem = planeToCSys(evPlane(context, { "face" : definition.face }));
        if (horizontalReferenceDirection != undefined)
        {
            queriesForTransform = append(queriesForTransform, definition.originConnector);
            verify(!parallelVectors(planeSystem.zAxis, horizontalReferenceDirection),
                ErrorStringEnum.DECAL_HORIZONTAL_REFERENCE_INVALID_ENTITY, {"faultyParameters" : ["horizontalReference"] });

            const orthogonalVector = normalize(cross(planeSystem.zAxis, horizontalReferenceDirection));
            planeSystem.xAxis = cross(orthogonalVector, planeSystem.zAxis);

            if (dot(vector(1, 0, 0), planeSystem.xAxis) < 0)
                planeSystem.xAxis = -planeSystem.xAxis;
        }
        else
        {
            planeSystem.xAxis = vectorInPlanePerpendicularToZ(planeSystem.zAxis);
        }

        planeSystem.origin = selectedOrigin;

        planeSystem.origin = toWorld(planeSystem, vector(definition.uShift, definition.vShift, 0 * meter));

        const patternTransform = getRemainderPatternTransform(context, {"references" : qUnion(queriesForTransform)});
        if (patternTransform != identityTransform())
        {
            const updatedPlaneSystem = patternTransform * planeSystem;
            if (!tolerantEquals(updatedPlaneSystem.zAxis, planeSystem.zAxis))
                throw "Pattern result must not change plane orientation";

            planeSystem = updatedPlaneSystem;
        }

        decalData = createPlanarDecal(id,
            definition.image,
            planeSystem,
            uvTransform
        );
    }
    else if (isCylinder)
    {
        var cylinder is Cylinder = evSurfaceDefinition(context, { "face": definition.face });

        // Align the direction of the cylinder's axis with world up, reorienting the cylinder
        // coordinate system, if needed.
        if (dot(cylinder.coordSystem.zAxis, vector(0, 0, 1)) < 0)
        {
            cylinder.coordSystem.zAxis = -cylinder.coordSystem.zAxis;
            cylinder.coordSystem.xAxis = -cylinder.coordSystem.xAxis;
        }

        if (computeOrigin)
        {
            const projectionInformation = getFaceCylinderProjectionInformation(context, definition, cylinder);

            var zCenter = (projectionInformation.zMax + projectionInformation.zMin) * 0.5;
            var centerAngle = (projectionInformation.angleMax + projectionInformation.angleMin) * 0.5;

            // If user defined an origin coordinate system, use it for the center of the projection of the decal
            if (definitionHasFaceOriginSpecified(context, definition))
            {
                queriesForTransform = append(queriesForTransform, definition.originConnector);
                const mateConnectorCSys = evMateConnector(context, { "mateConnector" : definition.originConnector });
                centerAngle = pointAngleInCylinder(cylinder, mateConnectorCSys.origin);
                zCenter = dot(cylinder.coordSystem.zAxis, mateConnectorCSys.origin - cylinder.coordSystem.origin);
            }

            const uShiftAngle = getAngleForCylinderArcSegment(cylinder, definition.uShift);
            const originOnCylinder = cylinder.coordSystem.origin + cylinder.coordSystem.zAxis * (zCenter + definition.vShift) +
                cos(centerAngle + uShiftAngle) * cylinder.coordSystem.xAxis * cylinder.radius +
                sin(centerAngle + uShiftAngle) * yAxis(cylinder.coordSystem) * cylinder.radius;

            // Project the selected origin onto the z axis to compute the cylinder's adjusted origin,
            // and alter the x-axis to point to selected origin.
            const selectedOriginToCylinderOrigin = originOnCylinder - cylinder.coordSystem.origin;
            if (!parallelVectors(selectedOriginToCylinderOrigin, cylinder.coordSystem.zAxis))
            {
                cylinder.coordSystem.origin = cylinder.coordSystem.origin +
                    dot(selectedOriginToCylinderOrigin, cylinder.coordSystem.zAxis) * cylinder.coordSystem.zAxis;
                cylinder.coordSystem.xAxis = normalize(originOnCylinder - cylinder.coordSystem.origin);
            }
        }

        var horizontalReferenceAngle = 0 * radian;
        if (horizontalReferenceDirection != undefined)
        {
            verify(!parallelVectors(cylinder.coordSystem.xAxis, horizontalReferenceDirection),
                ErrorStringEnum.DECAL_HORIZONTAL_REFERENCE_INVALID_ENTITY, {"faultyParameters" : ["horizontalReference"] });

            var projectedVector = vector(0, dot(horizontalReferenceDirection, yAxis(cylinder.coordSystem)),
                dot(horizontalReferenceDirection, cylinder.coordSystem.zAxis));
            projectedVector = normalize(projectedVector);
            horizontalReferenceAngle = -atan2(projectedVector[2], projectedVector[1]);
        }

        var uvTransform = getUvTransform(context, definition, horizontalReferenceAngle);

        const patternTransform = getRemainderPatternTransform(context, {"references" : qUnion(queriesForTransform)});
        if (patternTransform != identityTransform())
        {
            const updatedCylinderCoordSystem = patternTransform * cylinder.coordSystem;
            if (!tolerantEquals(updatedCylinderCoordSystem.zAxis, cylinder.coordSystem.zAxis) ||
                !(tolerantEquals(updatedCylinderCoordSystem.origin, cylinder.coordSystem.origin) ||
                  parallelVectors(updatedCylinderCoordSystem.origin - cylinder.coordSystem.origin, cylinder.coordSystem.zAxis)))
                throw "Pattern result must not change cylinder axis or translate cylinder origin laterally";

            cylinder.coordSystem = updatedCylinderCoordSystem;
        }

        decalData = createCylindricalDecal(id,
            definition.image,
            cylinder,
            uvTransform
        );
        decalData.horizontalReferenceAngle = horizontalReferenceAngle;
    }
    else
        throw "Unsupported face selection for decal";

    return decalData;
}

function getUvTransform(context is Context, definition is map, horizontalReferenceAngle is ValueWithUnits)
{
    var uvTransform = createUvTransform(definition.width, false /* mirrorHorizontal */, definition.height, false /* mirrorVertical */, horizontalReferenceAngle + definition.angle);
    if (definitionHasFaceOriginSpecified(context, definition))
        uvTransform.translation = getUvOffsetForImageOriginIndex(definition.imageOriginIndex);
    else
        uvTransform.translation = getUvOffsetForImageOriginIndex(DEFAULT_IMAGE_ORIGIN_CENTER);
    return uvTransform;
}

/**
 * For a plane, get the x direction that is perpendicular to the global Z axis
 */
export function vectorInPlanePerpendicularToZ(planeNormal is Vector) returns Vector
precondition
{
    is3dDirection(planeNormal);
}
{
    const Z_AXIS = vector(0, 0, 1);
    if (!parallelVectors(planeNormal, Z_AXIS))
    {
        const projectedZ = normalize(Z_AXIS - project(planeNormal, Z_AXIS));
        return cross(projectedZ, planeNormal);
    }
    else
    {
        const X_AXIS = vector(1, 0, 0);
        return X_AXIS;
    }
}

/**
 * Like transformBox3d, but projects the given world box into the local coordinate system
 */
function projectBox3d(boxIn is Box3d, cSys is CoordSystem) returns Box3d
{
    var transformedPoints = [];
    var coords = makeArray(3, undefined);
    for (var i = 0; i < 3; i += 1)
    {
        coords[i] =  [boxIn.minCorner[i], boxIn.maxCorner[i]];
    }
    for (var x in coords[0])
    {
        for (var y in coords[1])
        {
            for (var z in coords[2])
            {
                transformedPoints = append(transformedPoints, fromWorld(cSys, vector(x, y, z)));
            }
        }
    }
    return box3d(transformedPoints);
}

/**
 * Returns information on how the selected face projects onto the cylinder.  The high-level goal is to quickly get
 * information to allow for initial placement and sizing of the decal based on the trimmed cylinder.
 */
function getFaceCylinderProjectionInformation(context is Context, definition is map, cylinder is Cylinder) returns map
{
    const minPoint = evFaceTangentPlane(context, {
        face: definition.face,
        parameter: vector(0.0, 0.0)
    }).origin;

    const midPoint = evFaceTangentPlane(context, {
        face: definition.face,
        parameter: vector(0.5, 0.5)
    }).origin;

    const maxPoint = evFaceTangentPlane(context, {
        face: definition.face,
        parameter: vector(1.0, 1.0)
    }).origin;

    var angleMin = pointAngleInCylinder(cylinder, minPoint);
    const angleMid = pointAngleInCylinder(cylinder, midPoint);
    var angleMax = pointAngleInCylinder(cylinder, maxPoint);

    var results = {
        zMin: fromWorld(cylinder.coordSystem, minPoint)[2],
        zMax: fromWorld(cylinder.coordSystem, maxPoint)[2]
    };

    if (results.zMin > results.zMax)
    {
        const toSwap = results.zMin;
        results.zMin = results.zMax;
        results.zMax = toSwap;
    }

    // If the min and max points meet, then the cylinder extends the full circumference.
    if (abs(angleMax - angleMin) < TOLERANCE.zeroAngle * radian)
    {
        results.angleMin = -PI * radian;
        results.angleMax = PI * radian;
    }
    else
    {
        if (angleMin > angleMax)
        {
            const toSwap = angleMin;
            angleMin = angleMax;
            angleMax = toSwap;
        }

        // Make sure midpoint lies between the min and max angle.  If it does not, then the
        // portion of the circumference of the trimmed cylinder is the inverse of what the
        // current min/max indicate, so they need to be adjusted to get the correct angular bounds.
        if ((angleMin - angleMid) * (angleMid - angleMax) < 0)
        {
            results.angleMin = angleMax - 2 * PI * radian;
            results.angleMax = angleMin;
        }
        else
        {
            results.angleMin = angleMin;
            results.angleMax = angleMax;
        }
    }

    return results;
}

/**
 * Returns the angle of this point in a plane perpendicular to the cylinders axis
 * aligned with the cylinder's coordinate system's XY axes.
 */
function pointAngleInCylinder(cylinder is Cylinder, point is Vector) returns ValueWithUnits
{
    const projectedCenter = fromWorld(cylinder.coordSystem, point);
    if (stripUnits(squaredNorm(projectedCenter)) < TOLERANCE.zeroLength * TOLERANCE.zeroLength)
    {
        return 0 * radian;
    }
    else
    {
        const angle = atan2(projectedCenter[1], projectedCenter[0]);
        return angle < 0 ? angle + 2 * PI * radian : angle;
    }
}

/////////////////////// Manipulators /////////////////////////

const IMAGE_ORIGIN_MANIPULATOR = "imageOriginManipulator";
const U_SHIFT_MANIPULATOR = "uShiftManipulator";
const V_SHIFT_MANIPULATOR = "vShiftManipulator";
const SCALE_MANIPULATOR = "scaleManipulator";
const ANGLE_MANIPULATOR = "angleManipulator";

// The decal scale arrow will be placed this proportion along
// the diagonal from the image center to the upper right of the image.
const SCALE_VECTOR_OFFSET_RATIO = 0.2;

function addDecalManipulators(context is Context, id is Id, definition is map, decalData is DecalData)
{
    // Image placement origin points
    var points = [];
    for (var i = 0; i < IMAGE_ORIGIN_POINT_COUNT; i += 1)
    {
        points = append(points, getWorldSpacePosition(decalData, getUvOffsetForImageOriginIndex(i)));
    }

    // We only allow specification of the image origin when there is a face origin selected for its relative placement.
    var imageOriginManipulator = undefined;
    if (definitionHasFaceOriginSpecified(context, definition))
        imageOriginManipulator = pointsManipulator({ "points" : points, "index" : definition.imageOriginIndex });

    // Plane origin linear manipulators
    var uShiftManipulator;
    var vShiftManipulator;

    var scaleManipulator;

    // decal rotation manipulator
    var angleManipulator;

    const rotationHandleOffset = definition.width / 4;

    if (decalData.imageMappingType == ImageMappingType.PLANAR)
    {
        const coordSystem = decalData.planeSystem.coordSystem;
        uShiftManipulator = linearManipulator({
                // We shift the base of the manipulator here and in other manipulators such that
                // we can offset the manipulator by its full value, but keep the base of the arrows
                // at the origin.
                "base" : coordSystem.origin - coordSystem.xAxis * definition.uShift,
                "direction" : coordSystem.xAxis,
                "offset" : definition.uShift,
                "primaryParameterId" : "uShift"
        });
        const vDirection = yAxis(coordSystem);
        vShiftManipulator = linearManipulator({
                "base" : coordSystem.origin - vDirection * definition.vShift,
                "direction" : vDirection,
                "offset" : definition.vShift,
                "primaryParameterId" : "vShift"
        });

        const scaleVector = coordSystem.xAxis * (definition.width / 2) + yAxis(coordSystem) * (definition.height / 2);
        scaleManipulator = linearManipulator({
                "base" : coordSystem.origin + scaleVector * SCALE_VECTOR_OFFSET_RATIO,
                "direction" : normalize(scaleVector),
                "offset" : 0 * meter,
                "style" : ManipulatorStyleEnum.SIMPLE,
                "primaryParameterId" : getPrimaryScaleParameterId(definition)
        });

        angleManipulator = angularManipulator({
                "axisOrigin" : coordSystem.origin,
                "axisDirection" : -coordSystem.zAxis,
                "rotationOrigin" : toWorld(coordSystem, vector(rotationHandleOffset, 0 * meter, 0 * meter)),
                "angle" : definition.angle,
                "primaryParameterId": "angle"
        });
    }
    else if (decalData.imageMappingType == ImageMappingType.CYLINDRICAL)
    {
        const coordSystem = decalData.cylinderSystem.coordSystem;
        const originOnCylinder = coordSystem.origin +
            coordSystem.xAxis * decalData.cylinder.radius;
        const tangentDirection = yAxis(coordSystem);

        // If the user has set a reference direction for the angle, then we compute the angular manipulator
        // relative to that direction for cylindrical mapping.
        const horizontalReferenceTangent = cos(-decalData.horizontalReferenceAngle) * yAxis(coordSystem) + sin(-decalData.horizontalReferenceAngle) * coordSystem.zAxis;

        uShiftManipulator = linearManipulator({
                "base" : originOnCylinder - tangentDirection * definition.uShift,
                "direction" : tangentDirection,
                "offset" : definition.uShift,
                "primaryParameterId" : "uShift"
        });
        vShiftManipulator = linearManipulator({
            "base" : originOnCylinder - coordSystem.zAxis * definition.vShift,
            "direction" : coordSystem.zAxis,
            "offset" : definition.vShift,
            "primaryParameterId" : "vShift"
        });
        scaleManipulator = getCylinderScaleManipulator(definition, decalData);
        angleManipulator = angularManipulator({
                "axisOrigin" : originOnCylinder,
                "axisDirection" : -coordSystem.xAxis,
                "rotationOrigin" : originOnCylinder + horizontalReferenceTangent * rotationHandleOffset,
                "angle" : definition.angle,
                "primaryParameterId": "angle"
        });
    }
    else
        throw "Unsupported image mapping type";

    addManipulators(context, id, {
        (IMAGE_ORIGIN_MANIPULATOR) : imageOriginManipulator,
        (U_SHIFT_MANIPULATOR) : uShiftManipulator,
        (V_SHIFT_MANIPULATOR) :  vShiftManipulator,
        (SCALE_MANIPULATOR): scaleManipulator,
        (ANGLE_MANIPULATOR) : angleManipulator
    });
}

/** @internal */
export function onDecalManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    if (newManipulators[IMAGE_ORIGIN_MANIPULATOR] is map)
        definition.imageOriginIndex = newManipulators[IMAGE_ORIGIN_MANIPULATOR].index;

    if (newManipulators[U_SHIFT_MANIPULATOR] is map)
        definition.uShift = newManipulators[U_SHIFT_MANIPULATOR].offset;

    if (newManipulators[V_SHIFT_MANIPULATOR] is map)
        definition.vShift = newManipulators[V_SHIFT_MANIPULATOR].offset;

    if (newManipulators[SCALE_MANIPULATOR] is map)
    {
        const scaleVectorTheta = atan(definition.height / definition.width);
        const scaleVectorLength = getScaleVectorNorm(definition);
        const newWidth = 2 * cos(scaleVectorTheta) * ((scaleVectorLength + newManipulators[SCALE_MANIPULATOR].offset) / SCALE_VECTOR_OFFSET_RATIO);
        const scaleFactor = newWidth / definition.width;
        const newHeight = definition.height * scaleFactor;
        const minimumDimension = TOLERANCE.zeroLength * meter;
        if (newWidth >= minimumDimension && newHeight >= minimumDimension)
        {
            definition.width = newWidth;
            definition.height = newHeight;
        }
    }

    if (newManipulators[ANGLE_MANIPULATOR] is map)
    {
        definition.angle = abs(newManipulators[ANGLE_MANIPULATOR].angle);
        definition.angleOppositeDirection = newManipulators[ANGLE_MANIPULATOR].angle < 0 * radian;
    }

    return definition;
}

function getPrimaryScaleParameterId(definition is map)
{
    if (definition.maintainAspectRatio)
        return definition.aspectRatioConstraint == ImageAspectRatioConstraint.HEIGHT_DRIVING ? "height" : "width";
    else
        // Both dimensions are scaled, there is no primary parameter
        return undefined;
}

function getCylinderScaleManipulator(definition is map, decalData is DecalData) returns Manipulator
{
    // This routine finds the projection of a point in the direction of the decal's top right
    // corner.  Since we're working with a projection, the point and direction are found
    // using UV space to keep the manipulator on the cylinder.

    const scaleVectorTheta = atan(definition.height / definition.width);
    const scaleVectorLength = getScaleVectorNorm(definition);
    const vectorX = cos(scaleVectorTheta) * scaleVectorLength;
    const vectorY = sin(scaleVectorTheta) * scaleVectorLength;

    const baseUV = vector(0.5 + vectorX / definition.width, 0.5 + vectorY / definition.height);
    const offsetUV = baseUV * 1.1;

    const scaleBasePosition = getWorldSpacePosition(decalData, baseUV);
    const scaleOffsetPositon = getWorldSpacePosition(decalData, offsetUV);

    return linearManipulator({
        "base" : scaleBasePosition,
        "direction" : normalize(scaleOffsetPositon - scaleBasePosition),
        "offset" : 0 * meter,
        "style" : ManipulatorStyleEnum.SIMPLE,
        "primaryParameterId" : getPrimaryScaleParameterId(definition)
    });
}

/**
 * Returns whether or not the decal as currently defined projects onto the face.  We use the UV bounds of
 * a set of points from the face to judge whether the decal is on the face.  This is relatively inexpensive
 * to do, but does not exactly dictate whether the face intersects with the decal projection.  Since we
 * use UV bounds, the face may be near the decal, but not intersect if the face does not fully occupy
 * its UV bounds (i.e., in the case of a trimmed face). Hence this method only returns if the decal and
 * face are near, and not whether they have overlap.
 */
function isDecalNearFace(context is Context, definition is map, decalData is DecalData) returns boolean
{
    if (decalData.imageMappingType == ImageMappingType.PLANAR)
        return isDecalNearPlanarFace(context, definition, decalData);
    else if (decalData.imageMappingType == ImageMappingType.CYLINDRICAL)
        return isDecalNearCylindricalFace(context, definition, decalData);

    throw "Unsupported image mapping type";
}

function isDecalNearPlanarFace(context is Context, definition is map, decalData is DecalData)
{
    var minU = inf;
    var maxU = -inf;
    var minV = inf;
    var maxV = -inf;

    for (var u = 0; u <= 1; u += 1)
    {
        for (var v = 0; v <= 1; v += 1)
        {
           const facePoint = evFaceTangentPlane(context, {
                face: definition.face,
                parameter: vector(u, v)
            }).origin;

            const decalUv = getDecalUvSpacePosition(decalData, facePoint);
            if (decalUv[0] < minU)
                minU = decalUv[0];
            if (decalUv[0] > maxU)
                maxU = decalUv[0];
            if (decalUv[1] < minV)
                minV = decalUv[1];
            if (decalUv[1] > maxV)
                maxV = decalUv[1];
        }
    }

    return minU < 1.0 && maxU > 0.0 && minV < 1.0 && maxV > 0.0;
}

/**
 * For cylinder, we consider a decal near the cylinder if it is within the "vertical" extents of
 * the cylinder, where the vertical axis is defined by the cylinder's Z axis.
 */
function isDecalNearCylindricalFace(context is Context, definition is map, decalData is DecalData)
{
    const cylinder is Cylinder = evSurfaceDefinition(context, { "face": definition.face });
    const cylinderFaceProjection = getFaceCylinderProjectionInformation(context, definition, cylinder);

    const bottomPoint = getWorldSpacePosition(decalData, vector(0, 0));
    const topPoint = getWorldSpacePosition(decalData, vector(1, 1));

    const bottomZ = fromWorld(cylinder.coordSystem, bottomPoint)[2];
    const topZ = fromWorld(cylinder.coordSystem, topPoint)[2];

    return (((bottomZ < topZ) && (bottomZ < cylinderFaceProjection.zMax && topZ > cylinderFaceProjection.zMin)) ||
        ((bottomZ >= topZ) && (topZ < cylinderFaceProjection.zMax && bottomZ > cylinderFaceProjection.zMin)));
}

function getScaleVectorNorm(definition is map)
{
    return sqrt((definition.width / 2) ^ 2 + (definition.height / 2) ^ 2) * SCALE_VECTOR_OFFSET_RATIO;
}

function getUvOffsetForImageOriginIndex(index is number) returns Vector
{
    const row = floor(index / IMAGE_ORIGIN_COLUMN_COUNT);
    const column = index % IMAGE_ORIGIN_COLUMN_COUNT;
    return vector(0.5 * column, 0.5 * row);
}

