// wrappers around geometric operations
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/lineplane.fs", version : "");
export import(path : "onshape/std/dimensionalignment.gen.fs", version : "");
export import(path : "onshape/std/dimensionhalfspace.gen.fs", version : "");
export import(path : "onshape/std/errorstringenum.gen.fs", version : "");
export import(path : "onshape/std/radiusdisplay.gen.fs", version : "");


export enum ConstraintType
{
    NONE,
    COINCIDENT,
    PARALLEL,
    VERTICAL,
    HORIZONTAL,
    PERPENDICULAR,
    CONCENTRIC,
    MIRROR,
    MIDPOINT,
    TANGENT,
    EQUAL,
    LENGTH,
    DISTANCE,
    ANGLE,
    RADIUS,
    NORMAL,
    FIX,
    PROJECTED,
    OFFSET
}

export enum DimensionDirection
{
    MINIMUM,
    HORIZONTAL,
    VERTICAL
}

export enum SketchProjectionType
{
    USE,
    SILHOUETTE_START,
    SILHOUETTE_END,
    USE_END
}

export predicate is2dPoint(value)
{
    ::isLengthVector(value);
    @size(value) == 2;
}

export type Sketch typecheck canBeSketch;

export predicate canBeSketch(value)
{
    value is builtin; //TODO: have a builtin call for verification
}

annotation {"Feature Type Name" : "Sketch"}
export function newSketch(context is Context, id is Id, value is map) returns Sketch
precondition
{
    annotation {"Name" : "Sketch plane",
                "Filter" : GeometryType.PLANE,
                "MaxNumberOfPicks" : 1}
    value.sketchPlane is Query;
}
{
    recordQueries(context, id, value);
    value.planeReference = value.sketchPlane;
    var sketchPlane = evPlane(context, { "face" : value.sketchPlane });
    if(sketchPlane.error != undefined)
    {
        reportFeatureError(context, id, ErrorStringEnum.SKETCH_NO_PLANE);
        sketchPlane.result = XY_PLANE;
        value.planeReference = qNothing();
    }
    value.sketchPlane = sketchPlane.result;

    return newSketchOnPlane(context, id, value);
}

export function newSketchOnPlane(context is Context, id is Id, value is map) returns Sketch
precondition
{
    value.sketchPlane is Plane;
}
{
    var result = @newSketch(context, id, 6, value);
    reportFeatureError(context, id, result.error);
    return result.result as Sketch;
}

export function skSolve(sketch is Sketch)
{
        return @skSolve(sketch);
}

export function skSetInitialGuess(sketch is Sketch, initialGuess is map)
{
    return @skSetInitialGuess(sketch, initialGuess);
}
// adds a sketch point, returns map { pointId : string}
export function skPoint(sketch is Sketch, pointId is string, value is map)
precondition
{
    value.position is undefined || is2dPoint(value.position);
 }
 {
    return @skPoint(sketch, pointId, value);
 }
// adds a line segment, returns map {startId:string, endId:string}
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

//adds a circle, returns map {centerId:string}
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

//adds an arc, returns map {startId:string, endId:string}
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

//adds a closed spline
export function skSpline(sketch is Sketch, splineId is string, value is map)
precondition
{
    value.construction is undefined || value.construction is boolean;
}
{
    return @skSpline(sketch, splineId, value);
}

//adds a spline segment (i.e open spline or piece of a closed spline)
export function skSplineSegment(sketch is Sketch, splineId is string, value is map)
precondition
{
   value.construction is undefined || value.construction is boolean;
}
{
    return @skSplineSegment(sketch, splineId, value);
}

//adds a closed interpolated spline
export function skInterpolatedSpline(sketch is Sketch, splineId is string, value is map)
precondition
{
    value.construction is undefined || value.construction is boolean;
}
{
    return @skInterpolatedSpline(sketch, splineId, value);
}

//adds a spline segment (i.e open spline or piece of a closed spline)
export function skInterpolatedSplineSegment(sketch is Sketch, splineId is string, value is map)
precondition
{
   value.construction is undefined || value.construction is boolean;
}
{
    return @skInterpolatedSplineSegment(sketch, splineId, value);
}

export function skConstraint(sketch is Sketch, constraintId is string, value is map)
precondition
{
    value.constraintType is ConstraintType;
}
{
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
export function skRectangle(sketch is Sketch, rectangleId is string, value is map)
precondition
{
    value.firstCorner is undefined || is2dPoint(value.firstCorner);
    value.secondCorner is undefined || is2dPoint(value.secondCorner);
    value.construction is undefined || value.construction is boolean;
}
{
    //Line segments
    var segIds = ["left", "right", "top", "bottom"];
    var locVal = ::stripUnits(value);
    for (var sId in segIds)
    {
        var fullId = rectangleId ~ "." ~ sId;
        @skLineSegment(sketch, fullId,
                          { "start" : rectangleSideStartPoint(locVal, sId),
                            "end" : rectangleSideEndPoint(locVal, sId)});
    }

    //corner constraints
    var constrInput = { "constraintType" : ConstraintType.COINCIDENT, "local0" : rectangleId~".left.start", "local1" : rectangleId ~".top.start" };
    var constraintId = rectangleId~".corner0";

    @skConstraint(sketch, constraintId, constrInput);

    constrInput = { "constraintType" : ConstraintType.COINCIDENT, "local0" : rectangleId~".left.end", "local1" : rectangleId ~".bottom.start" };
    constraintId = rectangleId~".corner1";
    @skConstraint(sketch, constraintId, constrInput);

    constrInput = { "constraintType" : ConstraintType.COINCIDENT, "local0" : rectangleId~".right.end", "local1" : rectangleId ~".bottom.end" };
    constraintId = rectangleId~".corner2";
    @skConstraint(sketch, constraintId, constrInput);

    constrInput = { "constraintType" : ConstraintType.COINCIDENT, "local0" : rectangleId~".right.start", "local1" : rectangleId ~".top.end" };
    constraintId = rectangleId~".corner3";
    @skConstraint(sketch, constraintId, constrInput);

    //parallel constraints
    constrInput = { "constraintType" : ConstraintType.PARALLEL, "local0" : rectangleId~".left", "local1" : rectangleId ~".right" };
    constraintId = rectangleId~".vertical.parallel";
    @skConstraint(sketch, constraintId, constrInput);

    constrInput = { "constraintType" : ConstraintType.PARALLEL, "local0" : rectangleId~".top", "local1" : rectangleId ~".bottom" };
    constraintId = rectangleId~".horizontal.parallel";
    @skConstraint(sketch, constraintId, constrInput);

    //vertical/horizontal constraints
    constrInput = { "constraintType" : ConstraintType.VERTICAL, "local0" : rectangleId~".left" };
    constraintId = rectangleId~".vertical";
    @skConstraint(sketch, constraintId, constrInput);

    constrInput = { "constraintType" : ConstraintType.HORIZONTAL, "local0" : rectangleId~".top" };
    constraintId = rectangleId~".horizontal";
    @skConstraint(sketch, constraintId, constrInput);
}

