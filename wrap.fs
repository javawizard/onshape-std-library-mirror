FeatureScript 2780; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2780.0");
export import(path : "onshape/std/tool.fs", version : "2780.0");
export import(path : "onshape/std/wraptype.gen.fs", version : "2780.0");

// Features using manipulators must export manipulator.fs
export import(path : "onshape/std/manipulator.fs", version : "2780.0");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "2780.0");
import(path : "onshape/std/booleanHeuristics.fs", version : "2780.0");
import(path : "onshape/std/box.fs", version : "2780.0");
import(path : "onshape/std/containers.fs", version : "2780.0");
import(path : "onshape/std/coordSystem.fs", version : "2780.0");
import(path : "onshape/std/curveGeometry.fs", version : "2780.0");
import(path : "onshape/std/debug.fs", version : "2780.0");
import(path : "onshape/std/evaluate.fs", version : "2780.0");
import(path : "onshape/std/feature.fs", version : "2780.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2780.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2780.0");
import(path : "onshape/std/transform.fs", version : "2780.0");
import(path : "onshape/std/valueBounds.fs", version : "2780.0");
import(path : "onshape/std/vector.fs", version : "2780.0");
import(path : "onshape/std/wrapSurface.fs", version : "2780.0");

/**
 * Defines what type of output the Wrap feature should produce.
 * @value SOLID : The wrap operation will produce thickened solid bodies.
 * @value SURFACE : The wrap operation will produce surface bodies.
 * @value IMPRINT : The wrap operation will imprint edges onto the destination face.
 */
export enum WrapResultType
{
    annotation { "Name" : "Solid" }
    SOLID,
    annotation { "Name" : "Surface" }
    SURFACE,
    annotation { "Name" : "Split" }
    IMPRINT
}

/**
 * Feature performing an [opWrap].
 */
annotation { "Feature Type Name" : "Wrap",
             "Manipulator Change Function" : "wrapManipulatorChange",
             "Editing Logic Function" : "wrapEditLogic" }
export const wrap = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Creation type", "UIHint" : UIHint.HORIZONTAL_ENUM, "Default" : WrapResultType.SURFACE }
        definition.resultType is WrapResultType;

        if (definition.resultType == WrapResultType.SOLID)
        {
            booleanStepTypePredicate(definition);
        }

        annotation { "Name" : "Tools", "Filter" : EntityType.FACE && GeometryType.PLANE && ConstructionObject.NO }
        definition.source is Query;

        annotation { "Name" : "Target", "Filter" : EntityType.FACE && (GeometryType.CYLINDER || GeometryType.CONE), "MaxNumberOfPicks" : 1 }
        definition.destination is Query;

        annotation { "Name" : "Flip alignment", "UIHint" : UIHint.OPPOSITE_DIRECTION, "Default" : false }
        definition.flipAlignment is boolean;

        if (definition.resultType != WrapResultType.IMPRINT)
        {
            annotation { "Name" : "Trim to target", "Default" : false }
            definition.trim is boolean;
        }

        annotation { "Name" : "Specify anchor points", "Default" : false }
        definition.customAnchors is boolean;

        if (definition.customAnchors)
        {
            annotation { "Name" : "Tools anchor point", "Filter" : QueryFilterCompound.ALLOWS_VERTEX, "MaxNumberOfPicks" : 1 }
            definition.sourceAnchor is Query;

            annotation { "Name" : "Target anchor point", "Filter" : QueryFilterCompound.ALLOWS_VERTEX, "MaxNumberOfPicks" : 1 }
            definition.destinationAnchor is Query;
        }

        annotation { "Group Name" : "Position", "Collapsed By Default" : true }
        {
            annotation { "Name" : "Angle" }
            isAngle(definition.angle, ANGLE_360_ZERO_DEFAULT_BOUNDS);

            annotation { "Name" : "Angle opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION_CIRCULAR, "Default" : false }
            definition.angleOppositeDirection is boolean;

            annotation { "Name" : "U shift" }
            isLength(definition.uShift, ZERO_DEFAULT_LENGTH_BOUNDS);

            annotation { "Name" : "U shift opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION, "Default" : false }
            definition.uShiftOppositeDirection is boolean;

            annotation { "Name" : "V shift" }
            isLength(definition.vShift, ZERO_DEFAULT_LENGTH_BOUNDS);

            annotation { "Name" : "V shift opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION, "Default" : false }
            definition.vShiftOppositeDirection is boolean;
        }

        if (definition.resultType == WrapResultType.SOLID)
        {
            annotation { "Name" : "Thickness" }
            isLength(definition.thickness, SHELL_OFFSET_BOUNDS);

            annotation { "Name" : "Thickness opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION, "Default" : false }
            definition.oppositeDirection is boolean;
        }

        if (definition.resultType == WrapResultType.SOLID)
        {
            booleanStepScopePredicate(definition);
        }
    }
    {
        verifyNoMesh(context, definition, "destination");
        verifyNoMesh(context, definition, "source");

        // ----- Source and Destination -----
        verifyNonemptyQuery(context, definition, "source", ErrorStringEnum.WRAP_SELECT_TOOLS);
        if (size(evaluateQuery(context, definition.destination)) != 1)
        {
            throw regenError(ErrorStringEnum.WRAP_SELECT_TARGET, ["destination"]);
        }

        // Adjust definition variables to apply flips and fall within some reasonable bounds
        definition.angle = adjustAngle(context, definition.angle) * (definition.angleOppositeDirection ? -1 : 1);
        definition.uShift = definition.uShift * (definition.uShiftOppositeDirection ? -1 : 1);
        definition.vShift = definition.vShift * (definition.vShiftOppositeDirection ? -1 : 1);
        // Null out flipper fields to prevent accidental reliance on them
        definition.angleOppositeDirection = undefined;
        definition.uShiftOppositeDirection = undefined;
        definition.vShiftOppositeDirection = undefined;

        // Gather information defining the source and destination surfaces
        const sourceInfo = getSourceInfo(context, definition);
        const destinationSurfaceDefinition = evSurfaceDefinition(context, { "face" : definition.destination });
        checkDestinationSurfaceType(definition, destinationSurfaceDefinition);

        // Construct anchors based on the source and destination surfaces
        const anchorInfo = getAnchorInfo(context, sourceInfo, destinationSurfaceDefinition, definition);

        // Get the base starting angle so that "0" angle is as expected. See [getCanonicalAngle] for description of what `canonicalAngle` represents
        const canonicalAngle = getCanonicalAngle(sourceInfo.plane, anchorInfo);

        // Construct wrap surfaces for opWrap
        const wrapSurfaces = constructWrapSurfaces(context, sourceInfo.plane, anchorInfo, canonicalAngle, destinationSurfaceDefinition, definition);

        addWrapManipulators(context, id, canonicalAngle, sourceInfo, destinationSurfaceDefinition, anchorInfo, definition);

        // ----- Show a useful display along with an error if the user is trying to imprint on sheet metal -----
        if (instructedToImprintOnSheetMetal(context, definition))
        {
            opWrap(context, id, {
                        "wrapType" : WrapType.SIMPLE,
                        "entities" : definition.source,
                        "source" : wrapSurfaces.source,
                        "destination" : wrapSurfaces.destination
                    });
            const errorEntities = qUnion([definition.destination, qCreatedBy(id, EntityType.EDGE)]);
            throw regenError(ErrorStringEnum.WRAP_IMPRINT_SHEET_METAL, ["resultType", "destination"], errorEntities);
        }

        // ----- Apply geometric changes -----
        opWrap(context, id, {
                    "wrapType" : getWrapType(definition),
                    "entities" : definition.source,
                    "source" : wrapSurfaces.source,
                    "destination" : wrapSurfaces.destination
                });

        if (definition.resultType == WrapResultType.SOLID)
        {
            thickenAndBoolean(context, id, definition);
        }

    }, {
            resultType : WrapResultType.SURFACE, flipAlignment : false, trim : false, customAnchors : false,
            angle : 0 * radian, angleOppositeDirection : false,
            uShift : 0 * meter, uShiftOppositeDirection : false,
            vShift : 0 * meter, vShiftOppositeDirection : false
        });

// ----- Anchors -----

/**
 * Gather information about the source and destination anchor points
 *
 * @return {{
 *      @field sourceAnchor {Vector} : The 3d world position of the source anchor.
 *      @field destinationAnchorInfo {map} : See [projectDestinationAnchor] documentation.
 *      @field sourceAndDestinationAntiAligned {boolean} : Whether the source plane normal and destination surface normal point against each other.
 *      @field directionHints {map} : See [getDirectionHints] documentation.  Might be empty if the user has not provided mate connectors.
 * }}
 */
function getAnchorInfo(context is Context, sourceInfo is map, destinationSurfaceDefinition is map, definition is map) returns map
{
    var sourceAnchor;
    var destinationAnchorInfo;
    var directionHints = {};
    if (definition.customAnchors)
    {
        // Get anchor points
        const worldSourceAnchor = try(evVertexPoint(context, { "vertex" : definition.sourceAnchor }));
        const worldDestinationAnchor = try(evVertexPoint(context, { "vertex" : definition.destinationAnchor }));
        if (worldSourceAnchor == undefined || worldDestinationAnchor == undefined)
        {
            // Highlight either or both missing fields in red
            var errorFields = [];
            if (worldSourceAnchor == undefined)
                errorFields = append(errorFields, "sourceAnchor");
            if (worldDestinationAnchor == undefined)
                errorFields = append(errorFields, "destinationAnchor");
            throw regenError(ErrorStringEnum.WRAP_SELECT_ANCHORS, errorFields);
        }

        // Project anchor points onto appropriate surface
        sourceAnchor = project(sourceInfo.plane, worldSourceAnchor);
        destinationAnchorInfo = projectDestinationAnchor(context, destinationSurfaceDefinition, definition.destination, worldDestinationAnchor, true);

        // Try to get anchor directions in case mate connectors are given
        directionHints = getDirectionHints(context, definition, sourceInfo, destinationAnchorInfo);
    }
    else
    {
        // Automatically select anchors
        var defaultAnchors = try(getDefaultAnchors(context, sourceInfo, destinationSurfaceDefinition, definition.destination));
        if (defaultAnchors == undefined)
        {
            throw regenError(ErrorStringEnum.WRAP_NEEDS_ANCHOR, ["customAnchors"]);
        }
        sourceAnchor = defaultAnchors.sourceAnchor;
        destinationAnchorInfo = defaultAnchors.destinationAnchorInfo;
    }

    return {
            "sourceAnchor" : sourceAnchor,
            "destinationAnchorInfo" : destinationAnchorInfo,
            "sourceAndDestinationAntiAligned" : dot(destinationAnchorInfo.surfaceNormal, sourceInfo.plane.normal) < -TOLERANCE.zeroAngle,
            "directionHints" : directionHints
        };
}

/**
 * Gather source and destination mate connector xAxis direction.  Only return this information if both anchors are defined
 * by mate connectors, and both mate connector zAxis directions are parallel (or antiparallel) with the surface normal of
 * the surface they are anchoring.  Otherwise return an empty map.
 *
 * @return {{
 *      @field sourceDirection {Vector} : The xAxis of the source mate connector (or `undefined`).
 *      @field destinationDirection {Vector} : The xAxis of the destination mate connector (or `undefined`).
 * }}
 */
function getDirectionHints(context is Context, definition is map, sourceInfo is map, destinationAnchorInfo is map) returns map
{
    // Find the source and destination direction hints.  Only return hints if we manage to calculate both.
    const sourceDirection = getDirectionHint(context, definition.sourceAnchor, sourceInfo.plane.normal);
    if (sourceDirection == undefined)
    {
        return {};
    }
    const destinationDirection = getDirectionHint(context, definition.destinationAnchor, destinationAnchorInfo.surfaceNormal);
    if (destinationDirection == undefined)
    {
        return {};
    }
    return {
            "sourceDirection" : sourceDirection,
            "destinationDirection" : destinationDirection
        };
}

// Returns the xAxis of the given mate connector (undefined if its zAxis is not parallel to the given normal)
function getDirectionHint(context, mateConnector is Query, anchorNormal is Vector)
{
    const anchorDirectionCSys = try silent(evMateConnector(context, { "mateConnector" : mateConnector }));
    if (anchorDirectionCSys != undefined && parallelVectors(anchorDirectionCSys.zAxis, anchorNormal))
    {
        return anchorDirectionCSys.xAxis;
    }
    return undefined;
}

/**
 * Calculate the default anchors for destination surfaces which provide a central axis.
 *
 * See [getDefaultAnchors] documentation for detailed information about returned map.
 */
function getDefaultAnchorsForDestinationWithAxis(context is Context, sourceInfo is map, destinationAxis is Line, destinationSurfaceDefinition, destinationFace is Query) returns map
{
    const sourcePlane = sourceInfo.plane;
    const planeCSys = coordSystem(sourcePlane);
    const toPlaneCSys = fromWorld(planeCSys);
    const fromPlaneCSys = toWorld(planeCSys);

    // Source box is in terms of planeCSys
    const sourceBox = sourceInfo.bbox;
    const sourceBoxCenter = box3dCenter(sourceBox);
    const sourceBoxMidlines = [
            line(sourceBoxCenter, vector(1, 0, 0)),
            line(sourceBoxCenter, vector(0, 1, 0))
        ];

    // If the plane is parallel to the axis, try to find the midpoint of the intersection
    // between the projected axis with the bounding box of the source.
    // Otherwise use the center of the bounding box.
    var sourceAnchor;
    if (intersection(sourcePlane, destinationAxis).dim != 0)
    {
        // Axis on sourcePlane in terms of planeCSys
        const worldAxisOnPlane = toPlaneCSys * project(sourcePlane, destinationAxis);

        var closestValidIntersection;
        var closestValidIntersectionDistanceSq;
        for (var midline in sourceBoxMidlines)
        {
            const intersectionResult = intersection(worldAxisOnPlane, midline);
            if (intersectionResult.dim != 0)
            {
                // Midline and axis are parallel
                continue;
            }
            const intersectionPoint = intersectionResult.intersection;
            if (!insideBox3d(intersectionPoint, sourceBox))
            {
                // Intersection point is not within the bounding box
                // TODO: Consider not caring whether the intersection is inside the bounding box
                continue;
            }
            const intersectionDistanceSq = squaredNorm(intersectionPoint - sourceBoxCenter);
            if (closestValidIntersection == undefined || intersectionDistanceSq < closestValidIntersectionDistanceSq)
            {
                // Found new closest midline intersection
                closestValidIntersection = intersectionPoint;
                closestValidIntersectionDistanceSq = intersectionDistanceSq;
            }
        }
        if (closestValidIntersection != undefined)
        {
            sourceAnchor = fromPlaneCSys * closestValidIntersection;
        }
    }
    if (sourceAnchor == undefined)
    {
        sourceAnchor = fromPlaneCSys * sourceBoxCenter;
    }

    var worldDestinationAnchor = sourceAnchor;
    // If worldDestinationAnchor lies on the axis, offset it along the sourcePlane normal
    // (Projection onto destination will still fail if sourcePlane normal is parallel to axis)
    if (tolerantEquals(worldDestinationAnchor, project(destinationAxis, worldDestinationAnchor)))
    {
        worldDestinationAnchor = worldDestinationAnchor + -planeCSys.zAxis * inch;
    }

    const destinationAnchorInfo = try silent(projectDestinationAnchor(context, destinationSurfaceDefinition, destinationFace, worldDestinationAnchor, false));
    if (destinationAnchorInfo == undefined && destinationSurfaceDefinition is Cone)
    {
        const tangentPlane = evFaceTangentPlane(context, {
                "face" : destinationFace,
                "parameter" : vector(0.5, 0.5)
        });
        worldDestinationAnchor = tangentPlane.origin;
    }
    return {
            "sourceAnchor" : sourceAnchor,
            "destinationAnchorInfo" : projectDestinationAnchor(context, destinationSurfaceDefinition, destinationFace, worldDestinationAnchor, false)
    };
}

// ----- Angle -----

// Return the angle that the destination u direction should be rotated counterclockwise about the destination normal to become the baseline "0" direction.
function getCanonicalAngle(sourcePlane is Plane, anchorInfo is map) returns ValueWithUnits
{
    const destinationAnchor = anchorInfo.destinationAnchorInfo.anchor;
    const sourceDirectionHint = anchorInfo.directionHints.sourceDirection;
    const destinationDirectionHint = anchorInfo.directionHints.destinationDirection;

    var canonicalAngle = getCanonicalAngleForDirections(sourcePlane, anchorInfo, sourcePlane.x, anchorInfo.destinationAnchorInfo.uDirection, false);
    if (sourceDirectionHint != undefined && destinationDirectionHint != undefined)
    {
        canonicalAngle += getCanonicalAngleForDirections(sourcePlane, anchorInfo, sourceDirectionHint, destinationDirectionHint, true);
    }
    return canonicalAngle;
}

function getCanonicalAngleForDirections(sourcePlane is Plane, anchorInfo is map, sourceDirection is Vector, destinationDirection is Vector, forHint is boolean) returns ValueWithUnits
{
    const destinationDirectionOnPlane = try silent(project(sourcePlane, line(anchorInfo.destinationAnchorInfo.anchor, destinationDirection)));
    if (destinationDirectionOnPlane != undefined)
    {
        // If source and destination are anti-aligned, a CCW spin with respect to the source normal is actually a CW spin with respect to the destination normal.
        const alignmentFlip = anchorInfo.sourceAndDestinationAntiAligned ? -1 : 1;
        // If we are doing this calculation for underlying surfaces, we want the CCW angle from destination to source so that we can spin the
        // destination u to the source u.  If we are doing this calculation for the hint directions, we want the CCW angle from the source to
        // the destination (-1 * CCW angle from destination to source) so that we can apply that as additional CCW spin to the destination u.
        const purposeFlip = forHint ? -1 : 1;

        return alignmentFlip * purposeFlip * angleBetweenCCW(destinationDirectionOnPlane.direction, sourceDirection, sourcePlane.normal);
    }
    else
    {
        // The destination direction is parallel to source plane normal.  No need to incur any additional spin.
        return 0 * radian;
    }
}

function angleBetweenCCW(vector1 is Vector, vector2 is Vector, normal is Vector)
{
    return atan2(scalarTripleProduct(normal, vector1, vector2), dot(vector1, vector2));
}

// ----- Wrap Surfaces -----

/**
 * Construct wrap surfaces for [opWrap] based on the source and destination information
 *
 * @return {{
 *      @field source {WrapSurface} : The source `WrapSurface` for `opWrap`.
 *      @field destination {WrapSurface} : The destination `WrapSurface` for `opWrap`.
 * }}
 */
function constructWrapSurfaces(context is Context, sourcePlane is Plane, anchorInfo is map, canonicalAngle is ValueWithUnits, destinationSurfaceDefinition, definition is map)
{
    throwUnsupportedDestinationError();
}

function constructWrapSurfaces(context is Context, sourcePlane is Plane, anchorInfo is map, canonicalAngle is ValueWithUnits, destinationSurfaceDefinition is Cylinder, definition is map) returns map
{
    // -- Source --
    const userFlip = definition.flipAlignment ? -1 : 1;
    const sourceAndDestinationAlignmentFlip = anchorInfo.sourceAndDestinationAntiAligned ? -1 : 1;
    const faceAndSurfaceAlignmentFlip = anchorInfo.destinationAnchorInfo.faceAndSurfaceAntiAligned ? -1 : 1;

    // Translate the source based on user-assigned shift.  Because the user is specifying the destination shift,
    // we actually need to translate the source backwards.
    const shiftVector = vector(-definition.uShift, userFlip * sourceAndDestinationAlignmentFlip * -definition.vShift, 0 * meter);
    // Rotate source based on user-assigned rotation.  Because the user is specifying the destination rotation,
    // we actually need to rotate the source backwards.
    const angleToRotateSource = sourceAndDestinationAlignmentFlip * -1 * (canonicalAngle + definition.angle);
    const sourceDirection = (cos(angleToRotateSource) * userFlip * sourcePlane.x) + (sin(angleToRotateSource) * yAxis(sourcePlane));
    // User defined flip, anti-aligned source and destination, and anti-aligned destination face and surface all trigger flips,
    // one being true triggers a flip, two being true cancels out, and all three being true triggers a flip
    const adjustedSourcePlaneNormal = sourcePlane.normal * (userFlip * sourceAndDestinationAlignmentFlip * faceAndSurfaceAlignmentFlip);
    const sourceShiftedAnchor = toWorld(coordSystem(anchorInfo.sourceAnchor, sourceDirection, sourcePlane.normal), shiftVector);
    const sourceSurface = makeWrapPlane(plane(sourcePlane.origin, adjustedSourcePlaneNormal), sourceShiftedAnchor, sourceDirection);

    // -- Destination --
    const remainingTransform = getRemainderPatternTransform(context, { "references" : definition.source });
    const destinationAnchorLine = remainingTransform * line(anchorInfo.destinationAnchorInfo.anchor, anchorInfo.destinationAnchorInfo.uDirection);
    const destinationSurface = makeWrapSurface(context, definition.destination, destinationAnchorLine.origin, destinationAnchorLine.direction);

    return {
            "source" : sourceSurface,
            "destination" : destinationSurface
        };
}

function constructWrapSurfaces(context is Context, sourcePlane is Plane, anchorInfo is map, canonicalAngle is ValueWithUnits, destinationSurfaceDefinition is Cone, definition is map) returns map
{
    // -- Source --
    const userFlip = definition.flipAlignment ? -1 : 1;
    const sourceAndDestinationAlignmentFlip = anchorInfo.sourceAndDestinationAntiAligned ? -1 : 1;
    const faceAndSurfaceAlignmentFlip = anchorInfo.destinationAnchorInfo.faceAndSurfaceAntiAligned ? -1 : 1;

    // Given the user assigned uShift, vShift and angle, here we compute the transforms to apply to the source plane
    const halfAngle = destinationSurfaceDefinition.halfAngle;
    const distanceToApex = norm(anchorInfo.destinationAnchorInfo.anchor - destinationSurfaceDefinition.coordSystem.origin);
    const radius = distanceToApex * sin(halfAngle);

    // gamma is the angle at the apex of the cone as defined by the uShift.
    const gamma = uShiftToAngle(-sourceAndDestinationAlignmentFlip * definition.uShift, radius) * sin(halfAngle);

    // compute how much a point shifts in u and v on the plane given the angle gamma(i.e. uShift) and the vShift
    const sourceUShift = (distanceToApex + definition.vShift) * (sin(gamma));
    const sourceVShift = distanceToApex * (1 - cos(gamma)) - definition.vShift * cos(gamma);

    // Translate the source based on user-assigned shift
    const shiftVector = sourceAndDestinationAlignmentFlip * vector(sourceUShift, sourceVShift * userFlip,  0 * meter);
    // Rotate source based on user-assigned rotation.
    const angleToRotateSource = -sourceAndDestinationAlignmentFlip * (canonicalAngle + definition.angle);

    const sourceDirection = (cos(angleToRotateSource) * userFlip * sourcePlane.x) + (sin(angleToRotateSource) * yAxis(sourcePlane));
    // User defined flip, anti-aligned source and destination, and anti-aligned destination face and surface all trigger flips,
    // one being true triggers a flip, two being true cancels out, and all three being true triggers a flip
    const adjustedSourcePlaneNormal = sourcePlane.normal * (userFlip * sourceAndDestinationAlignmentFlip * faceAndSurfaceAlignmentFlip);
    const sourceShiftedAnchor = toWorld(coordSystem(anchorInfo.sourceAnchor, sourceDirection, sourcePlane.normal), shiftVector);
    const sourceSurface = makeWrapPlane(plane(anchorInfo.sourceAnchor, adjustedSourcePlaneNormal), anchorInfo.sourceAnchor, sourceDirection);

    // -- Destination --
    const remainingTransform = getRemainderPatternTransform(context, { "references" : definition.source });
    const destinationManipulatorInfo = getDestinationManipulatorInfo(context, destinationSurfaceDefinition, anchorInfo.destinationAnchorInfo, definition);
    const destinationAnchorLine = remainingTransform * line(destinationManipulatorInfo.shiftedAnchor, destinationManipulatorInfo.shiftedUDirection);
    const destinationSurface = makeWrapSurface(context, definition.destination, destinationAnchorLine.origin, destinationAnchorLine.direction);

    return {
            "source" : sourceSurface,
            "destination" : destinationSurface
        };
}

// ----- Source specific functions -----

/**
 * Gather useful information about the planar source surface
 *
 * @return {{
 *      @field plane {Vector} : The oriented plane on which the source entities lie.
 *      @field bbox {Box3d} : The extents of the source entities on the source `plane`, in terms of the coordinate system defined by that `plane`.
 * }}
 */
function getSourceInfo(context is Context, definition is map) returns map
{
    const firstSource = qNthElement(definition.source, 0);
    var sourcePlane = try silent(evOwnerSketchPlane(context, { "entity" : firstSource }));
    if (sourcePlane == undefined)
    {
        sourcePlane = evPlane(context, { "face" : firstSource });
    }
    const bbox = evBox3d(context, {
                "topology" : definition.source,
                "cSys" : coordSystem(sourcePlane)
            });
    if (!tolerantEquals(bbox.minCorner[2], bbox.maxCorner[2]))
    {
        // Source entities should all lie on the same plane
        throw regenError(ErrorStringEnum.WRAP_SOURCE_DIFFERING_PLANES, ["source"], definition.source);
    }

    return {
            "plane" : sourcePlane,
            "bbox" : bbox
        };
}

// ----- Destination specific functions -----

// All functions in this section should have overloads for all available destination types, and a fallback function
// if an unsupported destination is passed in.

function throwUnsupportedDestinationError()
{
    // This should never happen for a UI user
    throw "Unsupported destination";
}

// - checkDestinationSurfaceType -

/**
 * Throw an appropriate user-facing error if the destination surface is not one of the supported types.
 */
function checkDestinationSurfaceType(definition is map, destinationSurfaceDefinition)
{
    // WRAP_SELECT_TARGET should reference all the types of surfaces we can wrap around, otherwise we need a new error here
    throw regenError(ErrorStringEnum.WRAP_SELECT_TARGET, ["destination"], definition.destination);
}

function checkDestinationSurfaceType(definition is map, destinationSurfaceDefinition is Cylinder)
{
    // Do nothing, as the point of this function is to throw an error for unsupported types.
}

function checkDestinationSurfaceType(definition is map, destinationSurfaceDefinition is Cone)
{
    // Do nothing, as the point of this function is to throw an error for unsupported types.
}

// - projectDestinationAnchor -

/**
 * Project the destination anchor onto the destination surface, and return some useful geometric information about the destination at that anchor point.
 *
 * @return {{
 *      @field anchor {Vector} : The 3d world position of the destination anchor.
 *      @field uDirection {Vector} : The 3d world direction of the u parameterization of the destination surface at the anchor.
 *      @field vDirection {Vector} : The 3d world direction of the v parameterization of the destination surface at the anchor.
 *      @field surfaceNormal {Vector} : The 3d world direction of the underlying surface normal of the selected destination face at the anchor.
 *      @field faceAndSurfaceAntiAligned {boolean} : Whether the face and its underlying surface have opposite normals.
 * }}
 */
function projectDestinationAnchor(context is Context, destinationSurfaceDefinition, destinationFace is Query, worldDestinationAnchor is Vector, isCustomAnchor is boolean)
{
    throwUnsupportedDestinationError();
}

function projectDestinationAnchor(context is Context, destinationCone is Cone, destinationFace is Query, worldDestinationAnchor is Vector, isCustomAnchor is boolean) returns map
{
    const coneCSys = destinationCone.coordSystem;
    const coneAxis = line(coneCSys.origin, coneCSys.zAxis);
    // Project destination anchor onto cone
    var anchorOnAxis = project(coneAxis, worldDestinationAnchor);
    // Check that destination anchor doesn't already lie on the cone axis
    if (tolerantEquals(worldDestinationAnchor, anchorOnAxis))
    {
        if (!isCustomAnchor)
        {
            throw regenError(ErrorStringEnum.WRAP_NEEDS_ANCHOR, ["destinationAnchor"]);
        }
        else
        {
            addDebugPoint(context, worldDestinationAnchor, DebugColor.RED);
            throw regenError(ErrorStringEnum.WRAP_NEEDS_DIFFERENT_ANCHOR, ["destinationAnchor"]);
        }
    }
    const radiusDirection = normalize(worldDestinationAnchor - anchorOnAxis);

    // anchorOnAxis is not within the cone, but on the opposite side of the apex
    // reflect the point around the apex so that it's in the cone.
    if (dot((anchorOnAxis - coneCSys.origin), coneAxis.direction) < TOLERANCE.zeroLength * meter)
    {
        anchorOnAxis = coneCSys.origin + norm(anchorOnAxis - coneCSys.origin) * coneAxis.direction;
    }
    const radius = norm(anchorOnAxis - coneCSys.origin) * tan(destinationCone.halfAngle);
    if (radius < TOLERANCE.zeroLength * meter) // we're at the cone apex
    {
        if (!isCustomAnchor)
        {
            throw regenError(ErrorStringEnum.WRAP_NEEDS_ANCHOR, ["destinationAnchor"]);
        }
        else
        {
            addDebugPoint(context, worldDestinationAnchor, DebugColor.RED);
            throw regenError(ErrorStringEnum.WRAP_NEEDS_DIFFERENT_ANCHOR, ["destinationAnchor"]);
        }
    }
    const anchor = anchorOnAxis + radiusDirection * radius;
    const heightBelow = radius * tan(destinationCone.halfAngle);
    const surfaceNormal = normalize(anchor - (anchorOnAxis + heightBelow * destinationCone.coordSystem.zAxis));

    const vDir = normalize(anchor - coneCSys.origin);
    return {
            "anchor" :anchor,
            "uDirection" : cross(vDir, surfaceNormal),
            "vDirection" : vDir,
            "surfaceNormal" : surfaceNormal,
            "faceAndSurfaceAntiAligned" : surfaceIsInward(context, anchorOnAxis, destinationFace)
        };
}

function projectDestinationAnchor(context is Context, destinationCylinder is Cylinder, destinationFace is Query, worldDestinationAnchor is Vector, isCustomAnchor is boolean) returns map
{
    const cylinderCSys = destinationCylinder.coordSystem;
    const cylinderAxis = line(cylinderCSys.origin, cylinderCSys.zAxis);
    // Check that destination anchor doesn't already lie on the cylinder axis
    if (tolerantEquals(worldDestinationAnchor, project(cylinderAxis, worldDestinationAnchor)))
    {
        if (!isCustomAnchor || !context->isAtVersionOrLater(FeatureScriptVersionNumber.V2597_CONE_WRAP_ANCHOR_AT_APEX))
        {
            throw regenError(ErrorStringEnum.WRAP_NEEDS_ANCHOR, ["destinationAnchor"]);
        }
        else
        {
            addDebugPoint(context, worldDestinationAnchor, DebugColor.RED);
            throw regenError(ErrorStringEnum.WRAP_NEEDS_DIFFERENT_ANCHOR, ["destinationAnchor"]);
        }
    }
    // Project destination anchor onto cylinder
    const anchorOnAxis = project(cylinderAxis, worldDestinationAnchor);
    const axisToAnchor = normalize(worldDestinationAnchor - anchorOnAxis);
    return {
            "anchor" : anchorOnAxis + axisToAnchor * destinationCylinder.radius,
            "uDirection" : cross(cylinderCSys.zAxis, axisToAnchor),
            "vDirection" : cylinderCSys.zAxis,
            "surfaceNormal" : axisToAnchor,
            "faceAndSurfaceAntiAligned" : surfaceIsInward(context, destinationCylinder.coordSystem.origin, destinationFace)
        };
}

// - getDefaultAnchors -

/**
 * Calculate the default anchor points when the user has not specified explicit anchors
 *
 * @return {{
 *      @field sourceAnchor {Vector} : The 3d world position of the source anchor.
 *      @field destinationAnchorInfo {map} : See [projectDestinationAnchor] documentation.
 * }}
 */
function getDefaultAnchors(context is Context, sourceInfo is map, destinationSurfaceDefinition, destinationFace is Query)
{
    throwUnsupportedDestinationError();
}

function getDefaultAnchors(context is Context, sourceInfo is map, destinationCylinder is Cylinder, destinationFace is Query) returns map
{
    const cylinderAxis = line(destinationCylinder.coordSystem.origin, destinationCylinder.coordSystem.zAxis);
    return getDefaultAnchorsForDestinationWithAxis(context, sourceInfo, cylinderAxis, destinationCylinder, destinationFace);
}

function getDefaultAnchors(context is Context, sourceInfo is map, destinationCone is Cone, destinationFace is Query) returns map
{
    const coneAxis = line(destinationCone.coordSystem.origin, destinationCone.coordSystem.zAxis);
    return getDefaultAnchorsForDestinationWithAxis(context, sourceInfo, coneAxis, destinationCone, destinationFace);
}

// - getManipulatorInformation -

/**
 * Get some auxiliary information which helps construct the uv shift manipulators.
 *
 * @return {{
 *      @field axis {Line} : @optional The central axis of the destination surface, if the destination surface is a type if surface that
 *                           has an axis
 *      @field uShiftedAnchor {Vector} : The 3d world position of the destination anchor shifted only by the u shift
 *      @field vShiftedAnchor {Vector} : The 3d world position of the destination anchor shifted only by the v shift
 *      @field shiftedAnchor {Vector} : The 3d world position of the shifted destination anchor
 *      @field shiftedSurfaceNormal {Vector} : The 3d world direction of the normal of the destination face at the shifted anchor point
 *      @field shiftedUDirection {Vector} : The 3d world direction of the u direction of the destination face at the shifted anchor point
 * }}
 */
function getDestinationManipulatorInfo(context is Context, destinationSurfaceDefinition, destinationAnchorInfo is map, definition is map)
{
    throwUnsupportedDestinationError();
}
function getDestinationManipulatorInfo(context is Context, destinationCone is Cone, destinationAnchorInfo is map, definition is map) returns map
{
    const axis = line(destinationCone.coordSystem.origin, destinationCone.coordSystem.zAxis);

    const vDir = destinationAnchorInfo.anchor - destinationCone.coordSystem.origin ;
    const distToApex = norm(vDir);
    const vTranslation = transform(definition.vShift * normalize(vDir));

    const radius = distToApex * sin(destinationCone.halfAngle);
    const uRotation = rotationAround(axis, uShiftToAngle(definition.uShift, radius));

    const destinationAnchorTransform = uRotation * vTranslation;
    const shiftedAnchorSurfaceNormalLine = destinationAnchorTransform * line(destinationAnchorInfo.anchor, destinationAnchorInfo.surfaceNormal);
    const shiftedAnchorULine = destinationAnchorTransform * line(destinationAnchorInfo.anchor, destinationAnchorInfo.uDirection);
    const shiftedAnchorVLine = destinationAnchorTransform * line(destinationAnchorInfo.anchor, destinationAnchorInfo.vDirection);

    return {
            "axis" : axis,
            "uShiftedAnchor" : uRotation * destinationAnchorInfo.anchor,
            "vShiftedAnchor" : vTranslation * destinationAnchorInfo.anchor,
            "shiftedAnchor" : shiftedAnchorSurfaceNormalLine.origin,
            "shiftedSurfaceNormal" : shiftedAnchorSurfaceNormalLine.direction,
            "shiftedUDirection" : shiftedAnchorULine.direction,
            "shiftedVDirection" : shiftedAnchorVLine.direction
        };
}


function getDestinationManipulatorInfo(context is Context, destinationCylinder is Cylinder, destinationAnchorInfo is map, definition is map) returns map
{
    const axis = line(destinationCylinder.coordSystem.origin, destinationCylinder.coordSystem.zAxis);

    const vTranslation = transform(definition.vShift * axis.direction);
    const uRotation = rotationAround(axis, uShiftToAngle(definition.uShift, destinationCylinder.radius));

    const destinationAnchorTransform = uRotation * vTranslation;
    const shiftedAnchorSurfaceNormalLine = destinationAnchorTransform * line(destinationAnchorInfo.anchor, destinationAnchorInfo.surfaceNormal);
    const shiftedAnchorULine = destinationAnchorTransform * line(destinationAnchorInfo.anchor, destinationAnchorInfo.uDirection);

    return {
            "axis" : axis,
            "uShiftedAnchor" : uRotation * destinationAnchorInfo.anchor,
            "vShiftedAnchor" : vTranslation * destinationAnchorInfo.anchor,
            "shiftedAnchor" : shiftedAnchorSurfaceNormalLine.origin,
            "shiftedSurfaceNormal" : shiftedAnchorSurfaceNormalLine.direction,
            "shiftedUDirection" : shiftedAnchorULine.direction
        };
}


// ----- Engrave/Emboss -----

function thickenAndBoolean(context is Context, topLevelId is Id, definition is map)
{
    // -- Thicken --
    // Need to filter wrappedSheets by BodyType.SHEET because by the time we get to reconstructOp, we
    // may have already had one successful thicken, and thicken will fail if we pass solids into it.
    const wrappedSheets = qBodyType(qCreatedBy(topLevelId, EntityType.BODY), BodyType.SHEET);
    thickenWrappedSheets(context, topLevelId + "thicken", definition, wrappedSheets);
    const reconstructOp = function(reconstructId)
        {
            thickenWrappedSheets(context, reconstructId, definition, wrappedSheets);
        };
    // -- Boolean --
    processNewBodyIfNeeded(context, topLevelId, definition, reconstructOp);
    // Delete the wrapped sheets so that only the thickened bodies are left.  Do it after
    // processNewBodyIfNeeded, because reconstructOp needs them.
    opDeleteBodies(context, topLevelId + "deleteSheets", {
                "entities" : wrappedSheets
            });
}

function thickenWrappedSheets(context is Context, thickenId is Id, definition is map, wrappedSheets is Query)
{
    opThicken(context, thickenId, {
                "entities" : wrappedSheets,
                "thickness1" : definition.oppositeDirection ? 0 * inch : definition.thickness,
                "thickness2" : definition.oppositeDirection ? definition.thickness : 0 * inch
            });
}

// ----- Utilities -----

function instructedToImprintOnSheetMetal(context is Context, definition is map) returns boolean
{
    return definition.resultType == WrapResultType.IMPRINT && queryContainsActiveSheetMetal(context, definition.destination);
}

function getWrapType(definition is map)
{
    if (definition.resultType == WrapResultType.IMPRINT)
    {
        return WrapType.IMPRINT;
    }
    else
    {
        return (definition.trim ? WrapType.TRIM : WrapType.SIMPLE);
    }
}

function surfaceIsInward(context is Context, surfaceOrigin is Vector, face is Query) returns boolean
{
    const tangentPlane = evFaceTangentPlane(context, {
                "face" : face,
                "parameter" : vector(0.5, 0.5)
            });
    return dot(tangentPlane.normal, tangentPlane.origin - surfaceOrigin) < 0;
}

function uShiftToAngle(uShift is ValueWithUnits, radius is ValueWithUnits) returns ValueWithUnits
{
    // angle = (uShift / 2 * pi * radius) * 2 * pi * radian
    return (uShift / radius) * radian;
}

function angleToUShift(angle is ValueWithUnits, radius is ValueWithUnits) returns ValueWithUnits
{
    // uShift = (angle / 2 * pi * radian) * 2 * pi * radius
    return (angle / radian) * radius;
}

// ----- Manipulator and editing logic functions -----

const ANGLE_MANIPULATOR = "angleManipulator";
const U_ANGLE_MANIPULATOR = "uShiftAngleManipulator";
const V_MANIPULATOR = "vShiftManipulator";

function addWrapManipulators(context is Context, id is Id, canonicalAngle is ValueWithUnits, sourceInfo is map, destinationSurfaceDefinition, anchorInfo is map, definition is map)
{
    const destinationManipulatorInfo = getDestinationManipulatorInfo(context, destinationSurfaceDefinition, anchorInfo.destinationAnchorInfo, definition);
    var manipulators = {
        (ANGLE_MANIPULATOR) : getAngleManipulator(canonicalAngle, sourceInfo, anchorInfo.destinationAnchorInfo, destinationManipulatorInfo, definition),
        (V_MANIPULATOR) : getVManipulator(anchorInfo.destinationAnchorInfo, destinationManipulatorInfo, definition, destinationSurfaceDefinition)
    };
    // We may need to use different types of manipulators for the u manipulator based on the type of destination surface.  Make sure that these
    // manipulators have different names so that the manipulator change function can detect which is in use and respond effectively.
    const uManipulator = getUManipulatorAndName(destinationManipulatorInfo, definition, destinationSurfaceDefinition);
    if (uManipulator.name != undefined)
    {
        manipulators[uManipulator.name] = uManipulator.manipulator;
    }

    addManipulators(context, id, manipulators);
}

const ANGLE_MANIPULATOR_RADIUS_SCALE = 0.1;

function getAngleManipulator(canonicalAngle is ValueWithUnits, sourceInfo is map, destinationAnchorInfo is map, destinationManipulatorInfo is map, definition is map) returns Manipulator
{
    const axisOrigin = destinationManipulatorInfo.shiftedAnchor;
    const axisNormal = destinationManipulatorInfo.shiftedSurfaceNormal;
    const zeroDirectionLine = rotationAround(line(axisOrigin, axisNormal), canonicalAngle) * line(axisOrigin, destinationManipulatorInfo.shiftedUDirection);
    const angleManipulatorRadius = ANGLE_MANIPULATOR_RADIUS_SCALE * box3dDiagonalLength(sourceInfo.bbox);
    const rotationOrigin = axisOrigin + zeroDirectionLine.direction * angleManipulatorRadius;
    return angularManipulator({
                "axisOrigin" : axisOrigin,
                "axisDirection" : axisNormal,
                "rotationOrigin" : rotationOrigin,
                "angle" : definition.angle,
                "primaryParameterId" : "angle"
            });
}

function getVManipulator(destinationAnchorInfo is map, destinationManipulatorInfo is map, definition is map, destinationSurfaceDefinition) returns Manipulator
{
    return linearManipulator({
                "base" : destinationManipulatorInfo.uShiftedAnchor,
                "direction" : destinationSurfaceDefinition is Cone ? destinationManipulatorInfo.shiftedVDirection : destinationAnchorInfo.vDirection,
                "offset" : definition.vShift,
                "primaryParameterId" : "vShift"
            });
}

/**
 * @return {{
 *      @field name {string} : The name to use for the manipulator
 *      @field manipulator {Manipulator} : The manipulator itself
 * }}
 */
function getUManipulatorAndName(destinationManipulatorInfo is map, definition is map, destinationSurfaceDefinition) returns map
{
    var name;
    var manipulator;

    if (destinationManipulatorInfo.axis != undefined)
    {
        const rotationOrigin = destinationManipulatorInfo.vShiftedAnchor;
        const axisOrigin = project(destinationManipulatorInfo.axis, rotationOrigin);
        var radius = norm(rotationOrigin - axisOrigin);
        if (destinationSurfaceDefinition is Cone)
        {
            radius = radius - ((definition.vShift) * sin(destinationSurfaceDefinition.halfAngle));
        }
        name = U_ANGLE_MANIPULATOR;
        manipulator = angularManipulator({
                    "axisOrigin" : axisOrigin,
                    "axisDirection" : destinationManipulatorInfo.axis.direction,
                    "rotationOrigin" : rotationOrigin,
                    "angle" : uShiftToAngle(definition.uShift, radius),
                    "style" : ManipulatorStyleEnum.SIMPLE,
                    "primaryParameterId" : "uShift"
                });
    }
    // There may be different kinds of U manipulators in the future, for example, extruded surfaces won't use an angle manipulator
    return {
            "name" : name,
            "manipulator" : manipulator
        };
}

/**
 * @internal
 * Manipulator change function for `wrap`.
 */
export function wrapManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    if (newManipulators[ANGLE_MANIPULATOR] is Manipulator)
    {
        const newAngle = newManipulators[ANGLE_MANIPULATOR].angle;
        definition.angle = abs(newAngle);
        definition.angleOppositeDirection = newAngle < 0 * radian;
    }
    if (newManipulators[V_MANIPULATOR] is Manipulator)
    {
        const newVShift = newManipulators[V_MANIPULATOR].offset;
        definition.vShift = abs(newVShift);
        definition.vShiftOppositeDirection = newVShift < 0 * meter;
    }
    if (newManipulators[U_ANGLE_MANIPULATOR] is Manipulator)
    {
        const newAngle = newManipulators[U_ANGLE_MANIPULATOR].angle;
        var radius = norm(newManipulators[U_ANGLE_MANIPULATOR].rotationOrigin - newManipulators[U_ANGLE_MANIPULATOR].axisOrigin);

        const surfDef = try silent(evSurfaceDefinition(context, {
                "face" : definition.destination
        }));
        if (surfDef is Cone)
        {
            var vShiftFactor = definition.vShift * sin(surfDef.halfAngle);
            if (definition.vShiftOppositeDirection)
            {
                vShiftFactor *= -1;
            }
            radius = radius - vShiftFactor;
        }

        var angleAsUShift = angleToUShift(newAngle, radius);
        definition.uShift = abs(angleAsUShift);
        definition.uShiftOppositeDirection = angleAsUShift < 0 * meter;
    }
    return definition;
}

/**
 * @internal
 * Editing logic function for `wrap`.
 */
export function wrapEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map) returns map
{
    const sourceInfo = try silent(getSourceInfo(context, definition));
    const destinationSurfaceDefinition = try silent(evSurfaceDefinition(context, { "face" : definition.destination }));
    if (sourceInfo != undefined && destinationSurfaceDefinition != undefined)
    {
        if (!definition.customAnchors && !specifiedParameters.customAnchors)
        {
            try silent
            {
                getDefaultAnchors(context, sourceInfo, destinationSurfaceDefinition, definition.destination);
            }
            catch
            {
                definition.customAnchors = true;
            }
        }
    }

    // When selecting a new target, set the merge scope to be that target
    if (definition.destination != oldDefinition.destination && !specifiedParameters.booleanScope)
    {
        definition.booleanScope = qBodyType(qOwnerBody(definition.destination), BodyType.SOLID);

        // If the user has not specified an operation type, assign an appropriate operation type
        if (!specifiedParameters.operationType)
        {
            if (!isQueryEmpty(context, definition.booleanScope))
            {
                // We have a solid destination, remove or union depending on the direction of thickness
                definition.operationType = definition.oppositeDirection ? NewBodyOperationType.REMOVE : NewBodyOperationType.ADD;
            }
            else
            {
                definition.operationType = NewBodyOperationType.NEW;
            }
        }
    }
    // Flip thicken direction when flipping between add and remove
    if (!specifiedParameters.oppositeDirection && canSetBooleanFlip(oldDefinition, definition, specifiedParameters))
    {
        definition.oppositeDirection = !definition.oppositeDirection;
    }

    return definition;
}

