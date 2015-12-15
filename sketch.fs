FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/**
 * Functions used to create sketches, and add entities to sketches.
 *
 * Unless otherwise specified, vectors passed into sketch functions are 2D
 * `Vector`s in sketch coordinates, where `Vector(0, 0)` indicates the origin
 * of the sketch plane.
 */
// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/evaluate.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/mathUtils.fs", version : "");
import(path : "onshape/std/surfaceGeometry.fs", version : "");
import(path : "onshape/std/tool.fs", version : "");
import(path : "onshape/std/valueBounds.fs", version : "");

// These are not used in the library, but are made available to programs.
export import(path : "onshape/std/dimensionalignment.gen.fs", version : "");
export import(path : "onshape/std/dimensionhalfspace.gen.fs", version : "");
export import(path : "onshape/std/radiusdisplay.gen.fs", version : "");
export import(path : "onshape/std/sketchtooltype.gen.fs", version : "");
export import(path : "onshape/std/sketchsilhouettedisambiguation.gen.fs", version : "");
export import(path : "onshape/std/constrainttype.gen.fs", version : "");


annotation { "Deprecated" : true }
export enum DimensionDirection
{
    MINIMUM,
    HORIZONTAL,
    VERTICAL
}

annotation { "Deprecated" : true }
export enum SketchProjectionType
{
    USE,
    SILHOUETTE_START,
    SILHOUETTE_END,
    USE_END
}


/**
 * A `Sketch` object represents a sketch.  Sketches can be created
 * by calls to `newSketch`, to attach a sketch to existing or user-selected
 * geometry, or by `newSketchOnPlane`, to sketch on an arbitrary plane that
 * need not belong to any part.
 */
export type Sketch typecheck canBeSketch;

/**
 * Succeeds if argument is builtin Sketch type understood by the runtime.
 */
export predicate canBeSketch(value)
{
    @isSketch(value); /* implies (value is builtin) */
}

/**
 * Create a new sketch as a feature.
 */
annotation { "Feature Type Name" : "Sketch", "UIHint" : "CONTROL_VISIBILITY" }
export function newSketch(context is Context, id is Id, value is map) returns Sketch
precondition
{
    annotation { "Name" : "Sketch plane",
                "Filter" : GeometryType.PLANE,
                "MaxNumberOfPicks" : 1 }
    value.sketchPlane is Query;
}
{
    recordQueries(context, id, value);
    value.planeReference = value.sketchPlane;
    const planeDefinition = { "face" : value.sketchPlane, "asVersion" : value.asVersion };
    var sketchPlane = try(evPlane(context, planeDefinition));
    if (sketchPlane == undefined)
    {
        reportFeatureError(context, id, ErrorStringEnum.SKETCH_NO_PLANE);
        sketchPlane = XY_PLANE;
        value.planeReference = qNothing();
    }
    value.sketchPlane = sketchPlane;

    // We can't use the usual wrapped function because the context does not have the version set here yet
    if (@isAtVersionOrLater(context, FeatureScriptVersionNumber.V186_PLANE_COORDINATES, value.asVersion))
        value.sketchPlane.origin = project(value.sketchPlane, vector(0, 0, 0) * meter);

    return newSketchOnPlane(context, id, value);
}

/**
 * Create a new sketch for internal use within a feature.
 * @param value {{ @field sketchPlane }}
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
 * Call this function once all entities and constraints are created.
 * A sketch is not complete until solved, even if there are no constraints.
 */
export function skSolve(sketch is Sketch)
{
    return @skSolve(sketch);
}

/**
 * This function should not be called by user code.
 * Pass values to individual sketch functions instead.
 *
 * The initial guess is a map from string (sketch entity ID)
 * to array of number (entity-dependent values).  It is used
 * when sketch data is written out of line, as in generated
 * Part Studios.
 * @param initialGuess {map}
 */
export function skSetInitialGuess(sketch is Sketch, initialGuess is map)
{
    return @skSetInitialGuess(sketch, initialGuess);
}

/**
 * Add a point to a sketch.
 * @param value {{
 *      @field position {Vector}
 * }}
 * @return {{ @field pointId }}
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
 * @param value {{
 *      @field start {Vector}
 *      @field end {Vector}
 *      @field construction {boolean} : @eg `true` for a construction line @optional
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
 * @param value {{
 *      @field text {string}: A string of text to write. May contain newlines.
 *
 *      @field fontName {string}: A font name, with extension ".ttf" or ".otf".
 *              To change font weight, replace "-Regular" with "-Bold",
 *              "-Italic", or "-BoldItalic".
 *              TODO: Can we just make this accept a name and two booleans instead?
 *
 *          Must be one of the following fonts:
 *
 *          @eg `"OpenSans-Regular.ttf"`        Sans-serif font. Default if no match is found.
 *          @eg `"AllertaStencil-Regular.ttf"`  Stencil font. No bold/italic options.
 *          @eg `"Arimo-Regular.ttf"`           Sans-serif font.
 *          @eg `"DroidSansMono-Regular.ttf"`   Monospaced sans-serif font. No bold/italic options.
 *          @eg `"NotoSans-Regular.ttf"`        Sans-serif font.
 *          @eg `"NotoSansCJKjp-Regular.otf"`   Japanese font. No italic options.
 *          @eg `"NotoSansCJKkr-Regular.otf"`   Korean font. No italic options.
 *          @eg `"NotoSansCJKsc-Regular.otf"`   Chinese (simplified) font. No italic options.
 *          @eg `"NotoSansCJKtc-Regular.otf"`   Chinese (tranditional) font. No italic options.
 *          @eg `"NotoSans-Regular.ttf"`        Serif font.
 *          @eg `"RobotoSlab-Regular.ttf"`      Sans-serif font. No italic options.
 *          @eg `"Tinos-Regular.ttf"`           Serif font. Metrically compatible with Times New Roman.
 *
 *      @field construction {boolean} : @eg `true` for a construction line @optional
 * }}
 */
export function skText(sketch is Sketch, textId is string, value is map)
precondition
{
    value.fontName is string;
    value.text is string;
    value.construction is undefined || value.construction is boolean;
}
{
    return @skText(sketch, textId, value);
}

/**
 * Add an image rectangle, and return ids of corner points.
 *
 * @param value {{
 *      @field blobInfo {map} : TODO: what goes here?
 *      @field firstCorner {Vector}  : One corner of the rectangle into which the image will be placed.
 *      @field secondCorner {Vector} : The other corner of the rectangle into which the image will be placed.
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
 * @param value {{
 *      @field center {Vector}
 *      @field radius {ValueWithUnits} : A non-negative value with length units.
 *      @field construction {boolean} : @eg `true` for a construction line @optional
 * }}
 * @return {{ @field centerId }}
 */
export function skCircle(sketch is Sketch, circleId is string, value is map)
precondition
{
    value.center is undefined || is2dPoint(value.center);
    value.radius is undefined || isLength(value.radius, NONNEGATIVE_LENGTH_BOUNDS);
    value.construction is undefined || value.construction is boolean;
}
{
    return @skCircle(sketch, circleId, value);
}

/**
 * Add an ellipse
 *
 * @param value {{
 *      @field center {Vector}
 *      @field majorRadius {Vector}
 *      @field minorRadius {Vector}
 *      @field construction {boolean} : @eg `true` for a construction line @optional
 * }}
 * @return {{ @field centerId }}
 */
export function skEllipse(sketch is Sketch, ellipseId is string, value is map)
precondition
{
    value.center is undefined || is2dPoint(value.center);
    value.majorAxis is undefined || is2dPoint(value.majorAxis);
    value.minorRadius is undefined || isLength(value.minorRadius, NONNEGATIVE_LENGTH_BOUNDS);
    value.majorRadius is undefined || isLength(value.majorRadius, NONNEGATIVE_LENGTH_BOUNDS);
    value.construction is undefined || value.construction is boolean;
}
{
    return @skEllipse(sketch, ellipseId, value);
}

/**
 * Add an arc.
 *
 * @param value {{
 *      @field start {Vector}
 *      @field mid {Vector}
 *      @field end {Vector}
 *      @field construction {boolean} : @eg `true` for a construction line @optional
 * }}
 * @return {{ @field startId @field endId }}
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
 * Add an elliptical arc
 * The ellipse has a period of 1, a parameter of 0 at the major axis and 0.25 at the minor axis.
 * The arc is drawn counterclockwise from the start point to the end point.
 *
 * @param value {{
 *      @field center {Vector}
 *      @field majorAxis {Vector} : The direction, in sketch coordinates, in which the major axis of the ellipse lies.
 *      @field minorRadius {ValueWithUnits} : A non-negative value with length units.
 *      @field majorRadius {ValueWithUnits} : A non-negative value with length units. Does not need to be greater than
 *              the minor radius
 *      @field startParameter {number} : The parameter of the start point.
 *      @field endParameter {number} : The parameter of the end point.
 *      @field construction {boolean} : @eg `true` for a construction line @optional
 * }}
 * @return {{ @field startId @field endId }}
 */
export function skEllipticalArc(sketch is Sketch, arcId is string, value is map)
precondition
{
    value.center is undefined || is2dPoint(value.center);
    value.majorAxis is undefined || is2dPoint(value.majorAxis);
    value.minorRadius is undefined || isLength(value.minorRadius, NONNEGATIVE_LENGTH_BOUNDS);
    value.majorRadius is undefined || isLength(value.majorRadius, NONNEGATIVE_LENGTH_BOUNDS);
    value.startParameter is undefined || value.startParameter is number;
    value.endParameter is undefined || value.endParameter is number;
    value.construction is undefined || value.construction is boolean;
}
{
    return @skEllipticalArc(sketch, arcId, value);
}

/**
 * Add a closed spline. For Onshape internal use only.
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
 * Add a spline segment (i.e open spline or piece of a closed spline)
 * For Onshape internal use only.
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
 * Create a closed spline through a list of points. TODO: how to pass the list of points?
 * @param value {{
 *      @field splinePointCount {number} : The number of points in this spline.
 *      @field construction {boolean} : @eg `true` for a construction line @optional
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
 * Add a spline segment (i.e open spline or piece of a closed spline)
 * For Onshape internal use only.
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
 * @param value {{
 *      @field points : An array of points, each a `Vector` of two lengths
 *                   (x and y in the sketch plane coordinate system).
 *                   If the start and end points are the same the spline
 *                   is closed.
 *      @field construction {boolean} : @eg `true` for a construction line @optional
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


/**
 * Add a constraint.  (TODO: Explain how constraints work.)
 *
 * @param value {{
 *      @field constraintType {ConstraintType}
 *      @field length {ValueWithUnits} : For constraints that require a length. Must have length units. @optional
 *      @field angle {ValueWithUnits}  : For constraints that require a angle. Must have angle units. @optional
 * }}
 */
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
 * @param value {{
 *      @field firstCorner {Vector}
 *      @field secondCorner {Vector}
 *      @field construction {boolean} : @eg `true` for a construction line @optional
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

