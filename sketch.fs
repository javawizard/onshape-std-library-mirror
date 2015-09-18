FeatureScript 225; /* Automatically generated version */
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

/**
 * TODO: description
 */
export enum DimensionDirection
{
    MINIMUM,
    HORIZONTAL,
    VERTICAL
}

/**
 * TODO: description
 */
export enum SketchProjectionType
{
    USE,
    SILHOUETTE_START,
    SILHOUETTE_END,
    USE_END
}

export predicate is2dPoint(value)
{
    isLengthVector(value);
    @size(value) == 2;
}

/**
 * TODO: description
 */
export type Sketch typecheck canBeSketch;

export predicate canBeSketch(value)
{
    value is builtin; //TODO: have a builtin call for verification
}

/**
 * TODO: description
 * @param context
 * @param id
 * @param value {{
 *      @field TODO
 * }}
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
 * TODO: description
 * @param context
 * @param id
 * @param value {{
 *      @field TODO
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
 * TODO: description
 * @param sketch
 */
export function skSolve(sketch is Sketch)
{
    return @skSolve(sketch);
}

/**
 * TODO: description
 * @param sketch
 * @param initialGuess {{
 *      @field TODO
 * }}
 */
export function skSetInitialGuess(sketch is Sketch, initialGuess is map)
{
    return @skSetInitialGuess(sketch, initialGuess);
}

// adds a sketch point, returns map { pointId : string}
/**
 * TODO: description
 * @param sketch
 * @param pointId
 * @param value {{
 *      @field TODO
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

// adds a line segment, returns map {startId:string, endId:string}
/**
 * TODO: description
 * @param sketch
 * @param lineId
 * @param value {{
 *      @field TODO
 * }}
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

// Creates a text rectangle.
/**
 * TODO: description
 * @param sketch
 * @param textId
 * @param value {{
 *      @field TODO
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

//adds an image rectangle, and returns ids of corner points.
/**
 * TODO: description
 * @param sketch
 * @param imageId
 * @param value {{
 *      @field TODO
 * }}
 */
export function skImage(sketch is Sketch, imageId is string, value is map)
precondition
{
    // sourceId is the foreign data id string pointing to a specific version of an uploaded image
    value.blobInfo.sourceId is string;
    value.blobInfo.aspectRatio is undefined || (value.blobInfo.aspectRatio is number && value.blobInfo.aspectRatio > 0);
    value.firstCorner is undefined || is2dPoint(value.firstCorner);
    value.secondCorner is undefined || is2dPoint(value.secondCorner);
}
{
    value = mergeMaps(value.blobInfo, value);
    return @skImage(sketch, imageId, value);
}

//adds a circle, returns map {centerId:string}
/**
 * TODO: description
 * @param sketch
 * @param circleId
 * @param value {{
 *      @field TODO
 * }}
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

//adds an ellipse, returns map {centerId:string}
/**
 * TODO: description
 * @param sketch
 * @param ellipseId
 * @param value {{
 *      @field TODO
 * }}
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

//adds an arc, returns map {startId:string, endId:string}
/**
 * TODO: description
 * @param sketch
 * @param arcId
 * @param value {{
 *      @field TODO
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

//adds an arc, returns map {startId:string, endId:string}
/**
 * TODO: description
 * @param sketch
 * @param arcId
 * @param value {{
 *      @field TODO
 * }}
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

//adds a closed spline
/**
 * TODO: description
 * @param sketch
 * @param splineId
 * @param value {{
 *      @field TODO
 * }}
 */
export function skSpline(sketch is Sketch, splineId is string, value is map)
precondition
{
    value.construction is undefined || value.construction is boolean;
}
{
    return @skSpline(sketch, splineId, value);
}

//adds a spline segment (i.e open spline or piece of a closed spline)
/**
 * TODO: description
 * @param sketch
 * @param splineId
 * @param value {{
 *      @field TODO
 * }}
 */
export function skSplineSegment(sketch is Sketch, splineId is string, value is map)
precondition
{
    value.construction is undefined || value.construction is boolean;
}
{
    return @skSplineSegment(sketch, splineId, value);
}

//adds a closed interpolated spline
/**
 * TODO: description
 * @param sketch
 * @param splineId
 * @param value {{
 *      @field TODO
 * }}
 */
export function skInterpolatedSpline(sketch is Sketch, splineId is string, value is map)
precondition
{
    value.construction is undefined || value.construction is boolean;
}
{
    return @skInterpolatedSpline(sketch, splineId, value);
}

//adds a spline segment (i.e open spline or piece of a closed spline)
/**
 * TODO: description
 * @param sketch
 * @param splineId
 * @param value {{
 *      @field TODO
 * }}
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
 * TODO: description
 * @param sketch
 * @param constraintId
 * @param value {{
 *      @field TODO
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

// Creates rectangle ( four line segments properly constrained) and returns ids of corner points.
/**
 * TODO: description
 * @param sketch
 * @param rectangleId
 * @param value {{
 *      @field TODO
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
    for (var sId in segIds)
    {
        const fullId = rectangleId ~ "." ~ sId;
        @skLineSegment(sketch, fullId,
                { "start" : rectangleSideStartPoint(locVal, sId),
                    "end" : rectangleSideEndPoint(locVal, sId) });
    }

    //corner constraints
    var constrInput = { "constraintType" : ConstraintType.COINCIDENT, "local0" : rectangleId ~ ".left.start", "local1" : rectangleId ~ ".top.start" };
    var constraintId = rectangleId ~ ".corner0";

    @skConstraint(sketch, constraintId, constrInput);

    constrInput = { "constraintType" : ConstraintType.COINCIDENT, "local0" : rectangleId ~ ".left.end", "local1" : rectangleId ~ ".bottom.start" };
    constraintId = rectangleId ~ ".corner1";
    @skConstraint(sketch, constraintId, constrInput);

    constrInput = { "constraintType" : ConstraintType.COINCIDENT, "local0" : rectangleId ~ ".right.end", "local1" : rectangleId ~ ".bottom.end" };
    constraintId = rectangleId ~ ".corner2";
    @skConstraint(sketch, constraintId, constrInput);

    constrInput = { "constraintType" : ConstraintType.COINCIDENT, "local0" : rectangleId ~ ".right.start", "local1" : rectangleId ~ ".top.end" };
    constraintId = rectangleId ~ ".corner3";
    @skConstraint(sketch, constraintId, constrInput);

    //parallel constraints
    constrInput = { "constraintType" : ConstraintType.PARALLEL, "local0" : rectangleId ~ ".left", "local1" : rectangleId ~ ".right" };
    constraintId = rectangleId ~ ".vertical.parallel";
    @skConstraint(sketch, constraintId, constrInput);

    constrInput = { "constraintType" : ConstraintType.PARALLEL, "local0" : rectangleId ~ ".top", "local1" : rectangleId ~ ".bottom" };
    constraintId = rectangleId ~ ".horizontal.parallel";
    @skConstraint(sketch, constraintId, constrInput);

    //vertical/horizontal constraints
    constrInput = { "constraintType" : ConstraintType.VERTICAL, "local0" : rectangleId ~ ".left" };
    constraintId = rectangleId ~ ".vertical";
    @skConstraint(sketch, constraintId, constrInput);

    constrInput = { "constraintType" : ConstraintType.HORIZONTAL, "local0" : rectangleId ~ ".top" };
    constraintId = rectangleId ~ ".horizontal";
    @skConstraint(sketch, constraintId, constrInput);
}

