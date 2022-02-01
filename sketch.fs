FeatureScript 1691; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/**
 * Functions used to create sketches, and add entities to sketches.
 *
 * A sketch used in FeatureScript generally has the following form:
 * ```
 * var sketch1 = newSketch(context, id + "sketch1", {
 *         "sketchPlane" : qCreatedBy(makeId("Top"), EntityType.FACE)
 * });
 * skRectangle(sketch1, "rectangle1", {
 *         "firstCorner" : vector(0, 0),
 *         "secondCorner" : vector(1, 1)
 * });
 * skSolve(sketch1);
 *
 * extrude(context, id + "extrude1", {
 *         "entities" : qSketchRegion(id + "sketch1"),
 *         "endBound" : BoundingType.BLIND,
 *         "depth" : 0.5 * inch
 * });
 * ```
 *
 * A [Sketch] object should always be created first, with either [newSketch]
 * or [newSketchOnPlane].
 *
 * Next, any number of sketch entities may be added to the sketch using the
 * functions in this module. The inputs to sketch functions usually involve 2D
 * [Vector]s, which are positions relative to the sketch plane's origin and x-axis.
 * To create such a point based on a projected 3D point in world space, use
 * [worldToPlane(Plane, Vector)].
 *
 * When building sketches in FeatureScript, constraints may be added, but
 * are almost always unnecessary, since you already have the ability to place
 * the entities precisely where you intend them to be.
 *
 * Finally, the sketch is solved and added to the context by calling [skSolve].
 * As a result of [skSolve], all edges of the sketch will become `WIRE` bodies in the
 * context. Any regions enclosed in the sketch will become `SURFACE` bodies in
 * the context. Any vertices which are not edge endpoints (such as points created
 * by [skPoint] or the center point of [skCircle]) will become `POINT` bodies
 * in the context. These newly created bodies can be queried for and used in
 * all subsequent operations and features.
 */
// Imports used in interface
export import(path : "onshape/std/query.fs", version : "1691.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "1691.0");
import(path : "onshape/std/evaluate.fs", version : "1691.0");
import(path : "onshape/std/feature.fs", version : "1691.0");
import(path : "onshape/std/mathUtils.fs", version : "1691.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "1691.0");
import(path : "onshape/std/sheetMetalBuiltIns.fs", version : "1691.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1691.0");
import(path : "onshape/std/tool.fs", version : "1691.0");
import(path : "onshape/std/valueBounds.fs", version : "1691.0");
import(path : "onshape/std/matrix.fs", version : "1691.0");

// These are not used in the library, but are made available to programs.
export import(path : "onshape/std/dimensionalignment.gen.fs", version : "1691.0");
export import(path : "onshape/std/dimensionhalfspace.gen.fs", version : "1691.0");
export import(path : "onshape/std/radiusdisplay.gen.fs", version : "1691.0");
export import(path : "onshape/std/sketchtooltype.gen.fs", version : "1691.0");
export import(path : "onshape/std/sketchsilhouettedisambiguation.gen.fs", version : "1691.0");
export import(path : "onshape/std/constrainttype.gen.fs", version : "1691.0");
export import(path : "onshape/std/fixedparameterposition.gen.fs", version : "1691.0");

/**
 * @internal
 */
export enum DimensionDirection
{
    MINIMUM,
    HORIZONTAL,
    VERTICAL
}

/**
 * @internal
 */
export enum SketchProjectionType
{
    USE,
    SILHOUETTE_START,
    SILHOUETTE_END,
    USE_END
}

/**
 * A `LengthBoundSpec` for the radius of sketch circles and ellipses
 */
const SKETCH_RADIUS_BOUNDS =
{
    (meter)      : [5e-6, 0.025, 500],
    (centimeter) : 2.5,
    (millimeter) : 25.0,
    (inch)       : 1.0,
    (foot)       : 0.1,
    (yard)       : 0.025
} as LengthBoundSpec;

/**
 * A `Sketch` object represents a Onshape sketch, to which sketch entities
 * can be added.
 *
 * Sketches can be created by calls to [newSketch] or [newSketchOnPlane].
 */
export type Sketch typecheck canBeSketch;

/** Typecheck for builtin `Sketch` */
export predicate canBeSketch(value)
{
    @isSketch(value); /* implies (value is builtin) */
}

/**
 * Check whether an [Id] represents a Sketch operation.
 */
export function isIdForSketch(context is Context, id is Id)
{
    try silent
    {
        evOwnerSketchPlane(context, { "entity" : qCreatedBy(id, EntityType.BODY) });
        return true;
    }
    return false;
}

/**
 * Create a new sketch on an existing planar entity.  The sketch coordinate system follows the canonical plane
 * orientation and the sketch origin is the projection of the world origin onto the plane.
 *
 * To make a sketch in the coordinate system of an arbitrary [Plane], use `newSketchOnPlane`.
 *
 * @param id : @autocomplete `id + "sketch1"`
 * @param value {{
 *      @field sketchPlane {Query} : A Query for a single, planar entity.
 *              @eg `qCreatedBy(makeId("Top"), EntityType.FACE)` to sketch on default "Top" plane.
 *      @field disableImprinting {boolean} : @optional
 *              Prevents `sketchPlane` from imprinting on the sketch. Default is `false`.
 * }}
 */
// TODO: Is there a nice way to combine this and newSketchOnPlane without upsetting precondition analysis?
annotation { "Feature Type Name" : "Sketch", "UIHint" : UIHint.CONTROL_VISIBILITY }
export function newSketch(context is Context, id is Id, value is map) returns Sketch
precondition
{
    annotation { "Name" : "Sketch plane",
                "Filter" : (GeometryType.PLANE && AllowFlattenedGeometry.NO) || (SheetMetalDefinitionEntityType.FACE && AllowFlattenedGeometry.YES && GeometryType.PLANE) || BodyType.MATE_CONNECTOR,
                "MaxNumberOfPicks" : 1 }
    value.sketchPlane is Query;

    if (value.disableImprinting != undefined)
    {
        annotation { "Name" : "Disable imprinting" }
        value.disableImprinting is boolean;
    }
}
{
    recordParameters(context, id, value);

    startFeature(context, id + "sketchPlane", { asVersion : value.asVersion });

    var remainingTransform = getRemainderPatternTransform(context, {"references" : qUnion([value.sketchPlane])});
    var fullTransform = getFullPatternTransform(context);

    const mateConnectorCSys = try silent(evMateConnector(context, { "mateConnector" : value.sketchPlane }));
    var planeIsMateConnector = mateConnectorCSys != undefined;

    value.planeReference = planeIsMateConnector ? qNothing() : value.sketchPlane;
    const planeDefinition = { "face" : value.sketchPlane, "asVersion" : value.asVersion };
    value.sketchPlane = try silent(evPlane(context, planeDefinition));

    // After V1004_MATE_CONNECTOR_AS_PLANE, value.sketchPlane should already be set by evPlane for both planes and mate connectors.
    const allowMateConnectors = @isAtVersionOrLater(context, FeatureScriptVersionNumber.V740_PROPAGATE_PROPERTIES_IN_PATTERNS, value.asVersion);
    if (allowMateConnectors)
    {
        if (value.sketchPlane == undefined && planeIsMateConnector)
        {
            value.sketchPlane = plane(mateConnectorCSys);
        }
    }
    else
    {
        planeIsMateConnector = false;
    }

    if (value.sketchPlane == undefined)
    {
        reportFeatureError(context, id, ErrorStringEnum.SKETCH_NO_PLANE);
        value.sketchPlane = XY_PLANE;
        value.planeReference = qNothing();
    }

    if (!queryContainsFlattenedSheetMetal(context, value.planeReference))
    {
        // We can't use the usual wrapped function because the context does not have the version set here yet
        if (@isAtVersionOrLater(context, FeatureScriptVersionNumber.V186_PLANE_COORDINATES, value.asVersion) && !planeIsMateConnector)
            value.sketchPlane.origin = project(value.sketchPlane, vector(0, 0, 0) * meter);

        if (@isAtVersionOrLater(context, FeatureScriptVersionNumber.V305_UPGRADE_TEST_FAIL, value.asVersion))
        {
            // R * S = F => S = inv(R) * F => inv(S) = inv(F) * R
            var planeOriginal = (inverse(fullTransform) * remainingTransform) * value.sketchPlane;
            if (!planeIsMateConnector)
            {
                // If not a mate connector (which uses the mate connector's coordinate system) realign axes canonically
                planeOriginal = alignCanonically(context, planeOriginal);
                planeOriginal.origin = project(planeOriginal, vector(0, 0, 0) * meter);
            }
            if (@isAtVersionOrLater(context, FeatureScriptVersionNumber.V325_FEATURE_MIRROR, value.asVersion))
            {
                value.sketchPlane = planeOriginal;
                value.transform = fullTransform;
            }
            else
                value.sketchPlane = fullTransform * planeOriginal;
        }
    }
    else if (!queryContainsActiveSheetMetal(context, value.planeReference))
    {
        reportFeatureError(context, id, ErrorStringEnum.SHEET_METAL_ACTIVE_MODEL_REQUIRED);
        value.sketchPlane = XY_PLANE;
        value.planeReference = qNothing();
    }

    endFeature(context, id + "sketchPlane");

    return newSketchOnPlane(context, id, value);
}

/**
 * Create a new sketch on a custom plane, specified by a [Plane] object.  The sketch coordinate system
 * will match the coordinate system of the plane.
 *
 * @param id : @autocomplete `id + "sketch1"`
 * @param value {{
 *      @field sketchPlane {Plane} :
 *              @eg `plane(vector(0, 0, 0) * inch, vector(0, 0, 1))` to sketch on the world XY plane.
 * }}
 */
export function newSketchOnPlane(context is Context, id is Id, value is map) returns Sketch
precondition
{
    value.sketchPlane is Plane;
}
{
    const result = @newSketch(context, id, value);
    return result as Sketch;
}

/**
 * Solve any constraints in the sketch and add all sketch entities form the
 * `sketch` to its context.
 *
 * Even if there are no constraints, a sketch must be solved before its
 * entities are created.
 *
 * @param sketch : @autocomplete `sketch1`
 */
export function skSolve(sketch is Sketch)
{
    return @skSolve(sketch);
}

/**
 * @internal
 *
 * The initial guess is a map from string (sketch entity ID)
 * to array of number (entity-dependent values).  It is used
 * when sketch data is written out of line, as in generated
 * Part Studios.
 */
export function skSetInitialGuess(sketch is Sketch, initialGuess is map)
{
    return @skSetInitialGuess(sketch, initialGuess);
}

/**
 * Add a point to a sketch.
 *
 * @param sketch : @autocomplete `sketch1`
 * @param pointId : @autocomplete `"point1"`
 * @param value {{
 *      @field position {Vector} : @eg `vector(0, 1) * inch`
 * }}
 * @return {{
 *      @field pointId
 * }}
 */
export function skPoint(sketch is Sketch, pointId is string, value is map)
precondition
{
    value.position is undefined || is2dPoint(value.position);
}
{
    return @skPoint(sketch, pointId, value);
}

/**
 * Add a line segment to a sketch.
 *
 * @param sketch : @autocomplete `sketch1`
 * @param lineId : @autocomplete `"line1"`
 * @param value {{
 *      @field start {Vector} : @eg `vector(0, 0) * inch`
 *      @field end {Vector} : @eg `vector(1, 1) * inch`
 *      @field construction {boolean} : `true` for a construction line @optional
 * }}
 * @return {{ @field startId @field endId }}
 */
export function skLineSegment(sketch is Sketch, lineId is string, value is map)
precondition
{
    value.start is undefined || is2dPoint(value.start);
    value.end is undefined || is2dPoint(value.end);
    value.construction is undefined || value.construction is boolean;
}
{
    return @skLineSegment(sketch, lineId, value);
}

/**
 * Add a text rectangle to a sketch.
 *
 * @param sketch : @autocomplete `sketch1`
 * @param textId : @autocomplete `"text1"`
 * @param value {{
 *      @field text {string}: A string of text to write. May contain newlines (encoded as `\\n`).
 *          @autocomplete `"Example Text"`
 *
 *      @field fontName {string}: A font name, with extension ".ttf" or ".otf".
 *              To change font weight, replace "-Regular" with "-Bold",
 *              "-Italic", or "-BoldItalic".
 *
 *          Must be one of the following fonts:
 *
 *          @eg `"OpenSans-Regular.ttf"`        Sans-serif font. Default if no match is found.
 *          @eg `"AllertaStencil-Regular.ttf"`  Stencil font. No bold/italic options.
 *          @eg `"Arimo-Regular.ttf"`           Sans-serif font.
 *          @eg `"DroidSansMono.ttf"`           Monospaced sans-serif font. No bold/italic options.
 *          @eg `"NotoSans-Regular.ttf"`        Sans-serif font.
 *          @eg `"NotoSansCJKjp-Regular.otf"`   Japanese font. No italic options.
 *          @eg `"NotoSansCJKkr-Regular.otf"`   Korean font. No italic options.
 *          @eg `"NotoSansCJKsc-Regular.otf"`   Chinese (simplified) font. No italic options.
 *          @eg `"NotoSansCJKtc-Regular.otf"`   Chinese (traditional) font. No italic options.
 *          @eg `"NotoSerif-Regular.ttf"`       Serif font.
 *          @eg `"RobotoSlab-Regular.ttf"`      Sans-serif font. No italic options.
 *          @eg `"Tinos-Regular.ttf"`           Serif font. Metrically compatible with Times New Roman.
 *
 *      @field construction {boolean} : `true` for a construction line @optional
 *      @field firstCorner {Vector}  : @optional One corner of the rectangle into which the text will be placed. Text
 *                                     will start at the left of the rectangle and extend to the right, overflowing the
 *                                     right if necessary.  The first line of text will fill the height of the rectangle,
 *                                     with subsequent lines below the rectangle (or above if mirrored vertically).
 *                                     @eg `vector(0, 0) * inch`
 *      @field secondCorner {Vector} : @optional The other corner of the rectangle into which the text will be placed. Text
 *                                     will start at the left of the rectangle and extend to the right, overflowing the
 *                                     right if necessary.  The first line of text will fill the height of the rectangle,
 *                                     with subsequent lines below the rectangle (or above if mirrored vertically).
 *                                     @eg `vector(1, 1) * inch`
 *      @field mirrorHorizontal {boolean} : `true` for flipping text horizontally @optional
 *      @field mirrorVertical {boolean} : `true` for flipping text vertically @optional
 * }}
 * @return {{
 *      @field textId
 * }}
 */
// TODO: Can we just make `fontName` accept a name and two booleans for bold/italic instead?
export function skText(sketch is Sketch, textId is string, value is map)
precondition
{
    value.fontName is string;
    value.text is string;
    value.construction is undefined || value.construction is boolean;
    value.firstCorner is undefined || is2dPoint(value.firstCorner);
    value.secondCorner is undefined || is2dPoint(value.secondCorner);
    value.mirrorHorizontal is undefined || value.mirrorHorizontal is boolean;
    value.mirrorVertical is undefined || value.mirrorVertical is boolean;
}
{
    return @skText(sketch, textId, value);
}

/**
 * Add an image rectangle and return ids of corner points.
 *
 * To use an image uploaded in your document, import the image (possibly into a namespace).
 *
 * @param sketch : @autocomplete `sketch1`
 * @param imageId : @autocomplete `"image1"`
 * @param value {{
 *      @field blobInfo {map} :
 *          @eg `BLOB_DATA` will use the image from an image file imported into this Feature Studio.
 *          @eg `MyImage::BLOB_DATA` will use an image imported into the namespace `MyImage` (e.g. using `MyImage::import(...)`)
 *      @field firstCorner {Vector}  : One corner of the rectangle into which the image will be placed.
 *          @eg `vector(0, 0) * inch`
 *      @field secondCorner {Vector} : The other corner of the rectangle into which the image will be placed.
 *          @eg `vector(1, 1) * inch`
 * }}
 * @return {{
 *      @field imageId
 * }}
 */
export function skImage(sketch is Sketch, imageId is string, value is map)
precondition
{
    value.blobInfo is undefined || value.blobInfo is map; // We'll let the builtin do the real error checking
    value.firstCorner is undefined || is2dPoint(value.firstCorner);
    value.secondCorner is undefined || is2dPoint(value.secondCorner);
}
{
    if (value.blobInfo != undefined)
        value = mergeMaps(value.blobInfo, value);
    return @skImage(sketch, imageId, value);
}

/**
 * Add a circle to a sketch.
 *
 * @param sketch : @autocomplete `sketch1`
 * @param circleId : @autocomplete `"circle1"`
 * @param value {{
 *      @field center {Vector} : @eg `vector(0, 0) * inch`
 *      @field radius {ValueWithUnits} : @eg `1 * inch`
 *      @field construction {boolean} : `true` for a construction line @optional
 * }}
 * @return {{
 *      @field centerId
 * }}
 */
export function skCircle(sketch is Sketch, circleId is string, value is map)
precondition
{
    value.center is undefined || is2dPoint(value.center);
    value.radius is undefined || isLength(value.radius, SKETCH_RADIUS_BOUNDS);
    value.construction is undefined || value.construction is boolean;
}
{
    return @skCircle(sketch, circleId, value);
}

/**
 * Add an ellipse to a sketch.
 *
 * @param sketch : @autocomplete `sketch1`
 * @param ellipseId : @autocomplete `"ellipse1"`
 * @param value {{
 *      @field center {Vector} : @eg `vector(0, 0) * inch`
 *      @field majorRadius {ValueWithUnits} : @eg `2 * inch`
 *      @field minorRadius {ValueWithUnits} : @eg `1 * inch`
 *      @field majorAxis {Vector} : @optional A unitless 2D direction, specifying the orientation of the major axis
 *      @field construction {boolean} : `true` for a construction line @optional
 * }}
 * @return {{
 *      @field centerId
 * }}
 */
export function skEllipse(sketch is Sketch, ellipseId is string, value is map)
precondition
{
    value.center is undefined || is2dPoint(value.center);
    value.majorAxis is undefined || is2dDirection(value.majorAxis) || is2dPoint(value.majorAxis);
    value.minorRadius is undefined || isLength(value.minorRadius, SKETCH_RADIUS_BOUNDS);
    value.majorRadius is undefined || isLength(value.majorRadius, SKETCH_RADIUS_BOUNDS);
    value.construction is undefined || value.construction is boolean;
}
{
    return @skEllipse(sketch, ellipseId, value);
}

/**
 * Add an arc through three points to a sketch.
 *
 * @param sketch : @autocomplete `sketch1`
 * @param arcId : @autocomplete `"arc1"`
 * @param value {{
 *      @field start {Vector} : @eg `vector(1, 0) * inch`
 *      @field mid {Vector} : @eg `vector(0, 1) * inch`
 *      @field end {Vector} : @eg `vector(-1, 0) * inch`
 *      @field construction {boolean} : `true` for a construction line @optional
 * }}
 * @return {{
 *      @field startId
 *      @field endId
 * }}
 */
export function skArc(sketch is Sketch, arcId is string, value is map)
precondition
{
    value.start is undefined || is2dPoint(value.start);
    value.mid is undefined || is2dPoint(value.mid);
    value.end is undefined || is2dPoint(value.end);
    value.construction is undefined || value.construction is boolean;
}
{
    return @skArc(sketch, arcId, value);
}

/**
 * Add an elliptical arc to a sketch.
 * The ellipse has a period of 1, a parameter of 0 at the major axis and 0.25 at the minor axis.
 * The arc is drawn counterclockwise from the start point to the end point.
 *
 * @param sketch : @autocomplete `sketch1`
 * @param arcId : @autocomplete `"ellipticalArc1"`
 * @param value {{
 *      @field center {Vector} :
 *              @eg `vector(0, 0) * inch`
 *      @field majorAxis {Vector} : The direction, in sketch coordinates, in which the major axis of the ellipse lies.
 *              @eg `normalize(vector(1, 1))`
 *      @field minorRadius {ValueWithUnits} : A non-negative value with length units.
 *              @eg `1 * inch`
 *      @field majorRadius {ValueWithUnits} : A non-negative value with length units. Does not need to be greater than
 *              the minor radius.
 *              @eg `2 * inch`
 *      @field startParameter {number} : The parameter of the start point.
 *              @eg `0`
 *      @field endParameter {number} : The parameter of the end point.
 *              @eg `0.25`
 *      @field construction {boolean} : `true` for a construction line @optional
 * }}
 * @return {{
 *      @field startId
 *      @field endId
 * }}
 */
export function skEllipticalArc(sketch is Sketch, arcId is string, value is map)
precondition
{
    value.center is undefined || is2dPoint(value.center);
    value.majorAxis is undefined || is2dDirection(value.majorAxis) || is2dPoint(value.majorAxis);
    value.minorRadius is undefined || isLength(value.minorRadius, SKETCH_RADIUS_BOUNDS);
    value.majorRadius is undefined || isLength(value.majorRadius, SKETCH_RADIUS_BOUNDS);
    value.startParameter is undefined || value.startParameter is number;
    value.endParameter is undefined || value.endParameter is number;
    value.construction is undefined || value.construction is boolean;
}
{
    return @skEllipticalArc(sketch, arcId, value);
}

/**
 * @internal
 *
 * Add a closed spline.
 */
export function skSpline(sketch is Sketch, splineId is string, value is map)
precondition
{
    value.construction is undefined || value.construction is boolean;
    value.guess is undefined || value.guess is array;
}
{
    return @skSpline(sketch, splineId, value);
}

/**
 * @internal
 *
 * Add a spline segment (i.e open spline or piece of a closed spline)
 */
export function skSplineSegment(sketch is Sketch, splineId is string, value is map)
precondition
{
    value.construction is undefined || value.construction is boolean;
}
{
    return @skSplineSegment(sketch, splineId, value);
}

/**
 * @internal
 * Create a closed spline through a list of points.
 *
 * This function relies on the positions of the points being set in the
 * initial guess data. In custom features, prefer using `skFitSpline`.
 *
 * @param sketch : @autocomplete `sketch1`
 * @param splineId : @autocomplete `"spline1"`
 * @param value {{
 *      @field splinePointCount {number} : The number of points in this spline.
 *      @field construction {boolean} : `true` for a construction line @optional
 * }}
 */
export function skInterpolatedSpline(sketch is Sketch, splineId is string, value is map)
precondition
{
    value.construction is undefined || value.construction is boolean;
    value.splinePointCount is undefined || value.splinePointCount is number;
}
{
    return @skInterpolatedSpline(sketch, splineId, value);
}

/**
 * @internal
 * Add a spline segment (i.e open spline or piece of a closed spline)
 */
export function skInterpolatedSplineSegment(sketch is Sketch, splineId is string, value is map)
precondition
{
    value.construction is undefined || value.construction is boolean;
}
{
    return @skInterpolatedSplineSegment(sketch, splineId, value);
}

/**
 * Create an interpolated spline through the given points.
 *
 * @param sketch : @autocomplete `sketch1`
 * @param splineId : @autocomplete `"spline1"`
 * @param value {{
 *      @field points : An array of points. If the start and end points are
 *              the same, the spline is closed.
 * @eg ```
 * [
 *     vector( 0,  0) * inch,
 *     vector( 0, -1) * inch,
 *     vector( 1,  1) * inch,
 *     vector(-1,  0) * inch,
 *     vector( 0,  0) * inch
 * ]
 * ```
 *      @field parameters : An array of doubles, parameters corresponding to the points. @optional
 *      @field construction {boolean} : `true` for a construction line @optional
 *      @field startDerivative {Vector} : A 2D `Vector` with length units that specifies the derivative at the start of
 *          the resulting spline.  Ignored if spline is closed.  @optional
 *      @field endDerivative {Vector} : A 2D `Vector` with length units that specifies the derivative at the end of
 *          the resulting spline.  Ignored if spline is closed.  @optional
 * }}
 */
export function skFitSpline(sketch is Sketch, splineId is string, value is map)
precondition
{
    value.construction is undefined || value.construction is boolean;
    is2dPointVector(value.points);
    size(value.points) > 1;
}
{
    return @skFitSpline(sketch, splineId, value);
}

function rectangleSideStartPoint(value, side)
{
    if (value.firstCorner is undefined)
        return undefined;
    if (side == "left" || side == "top")
    {
        return value.firstCorner;
    }
    if (side == "right")
    {
        if (value.secondCorner is undefined)
            return undefined;
        return vector(value.secondCorner[0], value.firstCorner[1]);
    }
    if (side == "bottom")
    {
        if (value.secondCorner is undefined)
            return undefined;
        return vector(value.firstCorner[0], value.secondCorner[1]);
    }
}

function rectangleSideEndPoint(value, side)
{
    if (value.secondCorner is undefined)
        return undefined;
    if (side == "right" || side == "bottom")
    {
        return value.secondCorner;
    }
    if (side == "top")
    {
        if (value.secondCorner is undefined)
            return undefined;
        return vector(value.secondCorner[0], value.firstCorner[1]);
    }
    if (side == "left")
    {
        if (value.secondCorner is undefined)
            return undefined;
        return vector(value.firstCorner[0], value.secondCorner[1]);
    }
}

/**
 * Add a rectangle (four line segments, properly constrained) to a sketch.
 *
 * @param sketch : @autocomplete `sketch1`
 * @param rectangleId : @autocomplete `"rectangle1"`
 * @param value {{
 *      @field firstCorner {Vector} : @eg `vector(0, 0) * inch`
 *      @field secondCorner {Vector} : @eg `vector(1, 1) * inch`
 *      @field construction {boolean} : `true` for a construction line @optional
 * }}
 */
export function skRectangle(sketch is Sketch, rectangleId is string, value is map)
precondition
{
    value.firstCorner is undefined || is2dPoint(value.firstCorner);
    value.secondCorner is undefined || is2dPoint(value.secondCorner);
    value.construction is undefined || value.construction is boolean;
}
{
    //Line segments
    const segIds = ["left", "right", "top", "bottom"];
    const locVal = stripUnits(value);
    const construction = value.construction;
    for (var sId in segIds)
    {
        const fullId = rectangleId ~ "." ~ sId;
        @skLineSegment(sketch, fullId,
                { "start" : rectangleSideStartPoint(locVal, sId),
                    "end" : rectangleSideEndPoint(locVal, sId),
           "construction" : construction });
    }

    //corner constraints
    var constrInput = { "constraintType" : ConstraintType.COINCIDENT,
                        "localFirst" : rectangleId ~ ".left.start",
                        "localSecond" : rectangleId ~ ".top.start" };
    var constraintId = rectangleId ~ ".corner0";

    @skConstraint(sketch, constraintId, constrInput);

    constrInput = { "constraintType" : ConstraintType.COINCIDENT,
                     "localFirst" : rectangleId ~ ".left.end",
                     "localSecond" : rectangleId ~ ".bottom.start" };
    constraintId = rectangleId ~ ".corner1";
    @skConstraint(sketch, constraintId, constrInput);

    constrInput = { "constraintType" : ConstraintType.COINCIDENT,
                    "localFirst" : rectangleId ~ ".right.end",
                    "localSecond" : rectangleId ~ ".bottom.end" };
    constraintId = rectangleId ~ ".corner2";
    @skConstraint(sketch, constraintId, constrInput);

    constrInput = { "constraintType" : ConstraintType.COINCIDENT,
                    "localFirst" : rectangleId ~ ".right.start",
                    "localSecond" : rectangleId ~ ".top.end" };
    constraintId = rectangleId ~ ".corner3";
    @skConstraint(sketch, constraintId, constrInput);

    //parallel constraints
    constrInput = { "constraintType" : ConstraintType.PARALLEL,
                    "localFirst" : rectangleId ~ ".left",
                    "localSecond" : rectangleId ~ ".right" };
    constraintId = rectangleId ~ ".vertical.parallel";
    @skConstraint(sketch, constraintId, constrInput);

    constrInput = { "constraintType" : ConstraintType.PARALLEL,
                    "localFirst" : rectangleId ~ ".top",
                    "localSecond" : rectangleId ~ ".bottom" };
    constraintId = rectangleId ~ ".horizontal.parallel";
    @skConstraint(sketch, constraintId, constrInput);

    //vertical/horizontal constraints
    constrInput = { "constraintType" : ConstraintType.VERTICAL,
                    "localFirst" : rectangleId ~ ".left" };
    constraintId = rectangleId ~ ".vertical";
    @skConstraint(sketch, constraintId, constrInput);

    constrInput = { "constraintType" : ConstraintType.HORIZONTAL,
                    "localFirst" : rectangleId ~ ".top" };
    constraintId = rectangleId ~ ".horizontal";
    @skConstraint(sketch, constraintId, constrInput);
}

/**
 * Add a regular polygon to the sketch.  Unconstrained.
 *
 * @param sketch : @autocomplete `sketch1`
 * @param polygonId : @autocomplete `"polygon1"`
 * @param value {{
 *      @field center {Vector} : @eg `vector(0, 0) * inch`
 *      @field firstVertex {Vector} : Distance to the center determines the radius.
 *              @eg `vector(0, 1) * inch`
 *      @field sides {number} : Number of polygon sides. Must be an integer 3 or greater.
 *              @autocomplete `6`
 *      @field construction {boolean} : `true` for a construction line @optional
 * }}
 */
export function skRegularPolygon(sketch is Sketch, polygonId is string, value is map)
precondition
{
    is2dPoint(value.center);
    is2dPoint(value.firstVertex);
    !tolerantEquals(value.center, value.firstVertex);
    isPositiveInteger(value.sides);
    value.sides > 2;
    value.construction is undefined || value.construction is boolean;
}
{
    var points = [value.firstVertex];
    const axis = value.firstVertex - value.center;
    const angleIncrement = 2 * PI * radian / value.sides;
    for (var i = 0; i < value.sides - 1; i += 1)
    {
        const angle = (i + 1) * angleIncrement;
        const s = sin(angle);
        const c = cos(angle);
        const rotated = vector(c * axis[0] - s * axis[1], s * axis[0] + c * axis[1]);
        points = append(points, value.center + rotated);
    }
    points = append(points, value.firstVertex);
    skPolyline(sketch, polygonId, { "points" : points, "construction" : value.construction, "constrained" : false });
}

/**
 * Add a polyline (line segments, optionally with constrained endpoints) or a polygon to a sketch.
 *
 * @param sketch : @autocomplete `sketch1`
 * @param polylineId : @autocomplete `"polyline1"`
 * @param value {{
 *      @field points {array} : An array of points, each a `Vector` of two lengths.
 *              If first and last point are the same, the polyline is closed.
 * @eg ```
 * [
 *     vector( 0,  0) * inch,
 *     vector( 0, -1) * inch,
 *     vector( 1,  1) * inch,
 *     vector(-1,  0) * inch,
 *     vector( 0,  0) * inch
 * ]
 * ```
 *      @field construction {boolean} : `true` for a construction line.  Default false. @optional
 *      @field constrained {boolean} : `true` if constraints should be created.  Default false. @optional
 * }}
 */
export function skPolyline(sketch is Sketch, polylineId is string, value is map)
precondition
{
    is2dPointVector(value.points);
    size(value.points) > 1;
    size(value.points) > 2 || !tolerantEquals(value.points[0], value.points[size(value.points) - 1]);
    value.construction is undefined || value.construction is boolean;
    value.constrained is undefined || value.constrained is boolean;
}
{
    //Line segments and intermediate constraints
    const construction = value.construction;
    var numPoints = size(value.points);
    for (var i = 0; i + 1 < numPoints; i += 1)
    {
        const fullId = polylineId ~ ".line" ~ i;

        skLineSegment(sketch, fullId,
                { "start" : value.points[i],
                    "end" : value.points[i + 1],
                    "construction" : construction });

        if (i > 0 && value.constrained == true)
        {
            skConstraint(sketch, polylineId ~ ".constraint" ~ i,
                    { "constraintType" : ConstraintType.COINCIDENT,
                        "localFirst" : polylineId ~ ".line" ~ (i - 1) ~ ".end",
                        "localSecond" : polylineId ~ ".line" ~ i ~ ".start" });
        }
    }

    if (value.constrained == true && tolerantEquals(value.points[0], value.points[numPoints - 1])) // closed
    {
        skConstraint(sketch, polylineId ~ ".closed",
                { "constraintType" : ConstraintType.COINCIDENT,
                    "localFirst" : polylineId ~ ".line0" ~ ".start",
                    "localSecond" : polylineId ~ ".line" ~ (numPoints - 2) ~ ".end" });
    }
}

/**
 * Add a constraint.
 *
 * @param sketch : @autocomplete `sketch1`
 * @param constraintId : @autocomplete `"constraint1"`
 * @param value {{
 *      @field constraintType {ConstraintType}
 *      @field length {ValueWithUnits} : For constraints that require a length. Must have length units. @optional
 *      @field angle {ValueWithUnits}  : For constraints that require a angle. Must have angle units. @optional
 * }}
 */
// TODO: Explain how constraints work.
export function skConstraint(sketch is Sketch, constraintId is string, value is map)
precondition
{
    value.constraintType is ConstraintType;
}
{
    // If the units are wrong, make sure that the constraint is in error
    if (value.length != undefined && !isLength(value.length))
        value.length = undefined;
    if (value.angle != undefined && !isAngle(value.angle))
        value.angle = undefined;

    return @skConstraint(sketch, constraintId, value);
}

/**
 * @internal
 * Creates a conic section with the given rho value through the start and end points using the control point.
 * If x is the distance between controlPoint and the line between start and end, and y is the maximum distance between
 * the conic section and the same line, then rho is x/y.
 * @param sketch : @autocomplete `sketch1`
 * @param conicId : @autocomplete `"conic1"`
 * @param value {{
 *      @field start {Vector} :
 *              @eg `vector(0, 0) * inch`
 *      @field controlPoint {Vector} :
 *              @eg `vector(0.5, 0.5) * inch`
 *      @field end {Vector} :
 *              @eg `vector(1, 0) * inch`
 *      @field rho {number} : 0 < rho < 0.5 => elliptical arc, rho = 0.5 => parabola, 0.5 < rho < 1 => hyperbola
 *              @eg `0.5`
 *      @field lowParameter {number} : starting parameter of the segment @optional
 *      @field highParameter {number} : ending parameter of the segment @optional
 *      @field construction {boolean} : `true` for a construction conic @optional
 *      @field fixedRho {boolean} : `true` to fix rho during sketch solve @optional
 * }}
 */
export function skConicSegment(sketch is Sketch, conicId is string, value is map)
precondition
{
    value.start is undefined || is2dPoint(value.start);
    value.end is undefined || is2dPoint(value.end);
    value.controlPoint is undefined || is2dPoint(value.controlPoint);
    value.rho is undefined || value.rho is number;
    value.lowParameter is undefined || value.lowParameter is number;
    value.highParameter is undefined || value.highParameter is number;
    value.construction is undefined || value.construction is boolean;
    value.fixedRho is undefined || value.fixedRho is boolean;
}
{
    return @skConicSegment(sketch, conicId, value);
}

