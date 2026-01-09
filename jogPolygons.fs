FeatureScript 2856; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/surfaceGeometry.fs", version : "2856.0");

// Imports used internally
import(path : "onshape/std/box.fs", version : "2856.0");
import(path : "onshape/std/containers.fs", version : "2856.0");
import(path : "onshape/std/evaluate.fs", version : "2856.0");
import(path : "onshape/std/feature.fs", version : "2856.0");
import(path : "onshape/std/math.fs", version : "2856.0");
import(path : "onshape/std/units.fs", version : "2856.0");
import(path : "onshape/std/vector.fs", version : "2856.0");
import(path : "onshape/std/string.fs", version : "2856.0");
import(path : "onshape/std/curveGeometry.fs", version : "2856.0");
import(path : "onshape/std/coordSystem.fs", version : "2856.0");

// ATTENTION DEVELOPERS:
// If you version a fix to functionality used in section view
// Bump SBTAppElementViewVersionNumber and change BTPartStudioRenderingAgent.getSectionVersion()
// to return new FS version For new views

/** @internal */
export function jogPolygon(context is Context, points is array, boundingBox is Box3d, sketchPlane is Plane, offsetDistance is ValueWithUnits, isOffsetCut is boolean, isPartialSection is boolean, isAlignedSection is boolean) returns array
{
    if (isOffsetCut)
    {
        return createJogPolygonForOffsetCut(points, boundingBox, sketchPlane, offsetDistance);
    }
    else if (isPartialSection)
    {
        return createJogPolygonForPartialSection(context, points, boundingBox, sketchPlane, isAlignedSection);
    }
    else if (isAlignedSection)
    {
        return createJogPolygonForAlignedSection(points, boundingBox, sketchPlane);
    }
    else
    {
        return createJogPolygon(points, boundingBox, sketchPlane);
    }
}

function createJogPolygonForOffsetCut(points is array, boundingBox is Box3d, sketchPlane is Plane, offsetDistance is ValueWithUnits) returns array
{
    var polygonVertices = concatenateArrays([makeArray(1), points, makeArray(2)]);

    const pointCount = size(points);
    polygonVertices[0] = vector(points[pointCount - 1][0] + offsetDistance, points[0][1]);
    polygonVertices[pointCount + 1] = vector(points[pointCount - 1][0] + offsetDistance, points[pointCount - 1][1]);
    polygonVertices[pointCount + 2] = polygonVertices[0];

    return polygonVertices;
}

function createJogPolygon(points is array, boundingBox is Box3d, sketchPlane is Plane) returns array
{
    var polygonVertices = concatenateArrays([makeArray(1), points, makeArray(2)]);

    const pointCount = size(points);
    polygonVertices[0] = vector(0 * meter, points[0][1]);
    polygonVertices[pointCount + 1] = vector(0 * meter, points[pointCount - 1][1]);
    polygonVertices[pointCount + 2] = polygonVertices[0];

    return polygonVertices;
}

function createJogPolygonForPartialSection(context is Context, points is array, boundingBox is Box3d, sketchPlane is Plane,
                                           isAlignedSection is boolean) returns array
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V1871_PARTIAL_SECTION_CUT_TOOL_CORRECTION) ||
        (isAlignedSection && isAtVersionOrLater(context, FeatureScriptVersionNumber.V2397_DRAWINGS_PARTIAL_ALIGNED_TOOL_CORRECTION)))
    {
        var polygonVertices = concatenateArrays([points, makeArray(7)]);

        const pointCount = size(points);
        const flipY = points[pointCount - 1][1] < points[0][1];

        polygonVertices[pointCount] = vector(boundingBox.maxCorner[0], points[pointCount - 1][1]);
        polygonVertices[pointCount + 1] = vector(polygonVertices[pointCount][0], flipY ? boundingBox.minCorner[1] : boundingBox.maxCorner[1]);
        polygonVertices[pointCount + 2] = vector(boundingBox.minCorner[0], polygonVertices[pointCount + 1][1]);
        polygonVertices[pointCount + 3] = vector(polygonVertices[pointCount + 2][0], flipY ? boundingBox.maxCorner[1] : boundingBox.minCorner[1]);
        polygonVertices[pointCount + 4] = vector(boundingBox.maxCorner[0], polygonVertices[pointCount + 3][1]);
        polygonVertices[pointCount + 5] = vector(polygonVertices[pointCount + 4][0], points[0][1]);
        polygonVertices[pointCount + 6] = polygonVertices[0];

        return polygonVertices;
    }
    // Avoid self intersection in polygon when start or end point is outside bounding box. Also, the order
    // of vertices is consistent with that in createJogPolygon. Needed for associative data query resolution.
    var polygonVertices = concatenateArrays([makeArray(1), points]);
    var pointCount = size(points);
    const flipY = points[pointCount - 1][1] < points[0][1];
    if ((flipY && points[pointCount - 1][1] <= boundingBox.minCorner[1]) || (!flipY && points[pointCount - 1][1] >= boundingBox.maxCorner[1]))
    {
        polygonVertices = append(polygonVertices, vector(boundingBox.minCorner[0], points[pointCount - 1][1]));
    }
    else
    {
        polygonVertices = append(polygonVertices, vector(boundingBox.maxCorner[0], points[pointCount - 1][1]));
        polygonVertices = append(polygonVertices, vector(polygonVertices[pointCount + 1][0], flipY ? boundingBox.minCorner[1] : boundingBox.maxCorner[1]));
        polygonVertices = append(polygonVertices, vector(boundingBox.minCorner[0], polygonVertices[pointCount + 2][1]));
    }
    polygonVertices = append(polygonVertices, vector(boundingBox.minCorner[0], flipY ? boundingBox.maxCorner[1] : boundingBox.minCorner[1]));
    pointCount = size(polygonVertices);
    if ((flipY && points[0][1] >= boundingBox.maxCorner[1]) || (!flipY && points[0][1] <= boundingBox.minCorner[1]))
    {
        polygonVertices[pointCount - 1] = vector(polygonVertices[pointCount - 1][0], points[0][1]);
    }
    else
    {
        polygonVertices = append(polygonVertices, vector(boundingBox.maxCorner[0], polygonVertices[pointCount - 1][1]));
        polygonVertices = append(polygonVertices, vector(polygonVertices[pointCount][0], points[0][1]));
    }
    polygonVertices[0] = polygonVertices[size(polygonVertices) - 1];
    return polygonVertices;
}

function createJogPolygonForAlignedSection(points is array, boundingBox is Box3d, sketchPlane is Plane) returns array
{
    const pointCount = size(points);
    if (pointCount < 3)
    {
        return [];
    }
    var polygonVertices = concatenateArrays([points, makeArray(5)]);
    const distanceXToBoxMinCorner = abs(points[pointCount-1][0] - boundingBox.minCorner[0]);
    const distanceXToBoxMaxCorner = abs(points[pointCount-1][0] - boundingBox.maxCorner[0]);
    const flipX = distanceXToBoxMinCorner > distanceXToBoxMaxCorner;

    const distanceYToBoxMinCorner = abs(points[0][1] - boundingBox.minCorner[1]);
    const distanceYToBoxMaxCorner = abs(points[0][1] - boundingBox.maxCorner[1]);
    const flipY = distanceYToBoxMinCorner < distanceYToBoxMaxCorner;

    polygonVertices[pointCount] = vector(flipX ? boundingBox.maxCorner[0] : boundingBox.minCorner[0], points[pointCount - 1][1]);
    polygonVertices[pointCount + 1] = vector(polygonVertices[pointCount][0], flipY ? boundingBox.maxCorner[1] : boundingBox.minCorner[1]);
    polygonVertices[pointCount + 2] = vector(boundingBox.minCorner[0], polygonVertices[pointCount + 1][1]);
    polygonVertices[pointCount + 3] = vector(polygonVertices[pointCount + 2][0], points[0][1]);
    polygonVertices[pointCount + 4] = polygonVertices[0];

    return polygonVertices;
}

function getYOrientation(context is Context, points is array, boundingBox is Box3d) returns boolean
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V2725_DRAWINGS_PARTIAL_ALIGNED_TOOL_CORRECTION))
    {
        const distanceYToBoxMinCorner = abs(points[0][1] - boundingBox.minCorner[1]);
        const distanceYToBoxMaxCorner = abs(points[0][1] - boundingBox.maxCorner[1]);
        return (distanceYToBoxMinCorner < distanceYToBoxMaxCorner);
    }
    return points[1][1] > points[0][1];
}

/** @internal */
export function createJogPolygonForSourceParts(context is Context, points is array, boundingBox is Box3d) returns array
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V2762_DRAWINGS_PARTIAL_ALIGNED_TOOL_REFACTOR)) {
        return [ createJogPolygonForSourcePartsPre2762(context, points, boundingBox) ];
    }

    var polygonVertices = [points[0], points[1]];
    const flipY = getYOrientation(context, points, boundingBox);

    return appendJogPolygonForAlignedSection(polygonVertices, boundingBox, !flipY);
}

/** @internal */
export function createJogPolygonForRotatedParts(context is Context, points is array, boundingBox is Box3d) returns array
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V2762_DRAWINGS_PARTIAL_ALIGNED_TOOL_REFACTOR)) {
        return [ createJogPolygonForRotatedPartsPre2762(context, points, boundingBox) ];
    }

    const flipY = getYOrientation(context, points, boundingBox);

    const legLength = norm(points[2] - points[1]);
    var polygonVertices = [vector(points[1][0], points[1][1] + (flipY ? 1 : -1) * legLength), points[1]];

    return appendJogPolygonForAlignedSection(polygonVertices, boundingBox, flipY);
}

function appendJogPolygonForAlignedSection(polygonVertices is array, boundingBox is Box3d, flip is boolean) returns array
{
    polygonVertices = append(polygonVertices, vector(boundingBox.maxCorner[0], last(polygonVertices)[1]));
    polygonVertices = append(polygonVertices, vector(boundingBox.maxCorner[0], flip ? boundingBox.minCorner[1] : boundingBox.maxCorner[1]));
    polygonVertices = append(polygonVertices, vector(boundingBox.minCorner[0], last(polygonVertices)[1]));
    polygonVertices = append(polygonVertices, vector(boundingBox.minCorner[0], flip ? boundingBox.maxCorner[1] : boundingBox.minCorner[1]));
    polygonVertices = append(polygonVertices, vector(polygonVertices[0][0], last(polygonVertices)[1]));
    polygonVertices = append(polygonVertices, polygonVertices[0]);

    if (polygonVertices[0][1] > boundingBox.maxCorner[1])
    {
        polygonVertices[0][1] = boundingBox.maxCorner[1];
        polygonVertices[size(polygonVertices) - 1][1] = boundingBox.maxCorner[1];
    } else if (polygonVertices[0][1] < boundingBox.minCorner[1])
    {
        polygonVertices[0][1] = boundingBox.minCorner[1];
        polygonVertices[size(polygonVertices) - 1][1] = boundingBox.minCorner[1];
    } else
    {
        // Partial section - remove everything above the clipping plane.
        // This needs to be a separate polygon and extrude operation to ensure that any annotations to the cut edges
        // become invalid when the section is switched between full and partial.
        var clippingVertices = [ vector(polygonVertices[0][0], flip ? boundingBox.maxCorner[1] : boundingBox.minCorner[1]) ];
        clippingVertices = append(clippingVertices, vector(boundingBox.maxCorner[0], last(clippingVertices)[1]));
        clippingVertices = append(clippingVertices, vector(last(clippingVertices)[0], polygonVertices[0][1]));
        clippingVertices = append(clippingVertices, polygonVertices[0]);
        clippingVertices = append(clippingVertices, clippingVertices[0]);

        return [ polygonVertices, clippingVertices ];
    }

    return [ polygonVertices ];
}

function createJogPolygonForSourcePartsPre2762(context is Context, points is array, boundingBox is Box3d) returns array
{
    var polygonVertices = makeArray(7);
    const flipY = getYOrientation(context, points, boundingBox);

    polygonVertices[0] = points[0];
    polygonVertices[1] = points[1];
    polygonVertices[2] = vector(boundingBox.maxCorner[0], points[1][1]);
    polygonVertices[3] = vector(boundingBox.maxCorner[0], flipY ? boundingBox.maxCorner[1] : boundingBox.minCorner[1]);
    polygonVertices[4] = vector(boundingBox.minCorner[0], polygonVertices[3][1]);
    polygonVertices[5] = vector(boundingBox.minCorner[0], flipY ? boundingBox.minCorner[1] : boundingBox.maxCorner[1]);
    polygonVertices[6] = polygonVertices[0];

    return polygonVertices;
}

function createJogPolygonForRotatedPartsPre2762(context is Context, points is array, boundingBox is Box3d) returns array
{
    var polygonVertices = makeArray(7);
    const flipY = getYOrientation(context, points, boundingBox);

    polygonVertices[0] = vector(points[1][0], flipY ? boundingBox.maxCorner[1] : boundingBox.minCorner[1]);
    polygonVertices[1] = points[1];
    polygonVertices[2] = vector(boundingBox.maxCorner[0], points[1][1]);
    polygonVertices[3] = vector(boundingBox.maxCorner[0], flipY ? boundingBox.minCorner[1] : boundingBox.maxCorner[1]);
    polygonVertices[4] = vector(boundingBox.minCorner[0], polygonVertices[3][1]);
    polygonVertices[5] = vector(boundingBox.minCorner[0], flipY ? boundingBox.maxCorner[1] : boundingBox.minCorner[1]);
    polygonVertices[6] = polygonVertices[0];

    return polygonVertices;
}
