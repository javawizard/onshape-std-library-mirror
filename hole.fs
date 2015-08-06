FeatureScript ✨; /* Automatically generated version */
export import(path : "onshape/std/box.fs", version : "");
export import(path : "onshape/std/coordSystem.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/extrude.fs", version : "");
export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/math.fs", version : "");
export import(path : "onshape/std/matrix.fs", version : "");
export import(path : "onshape/std/sketch.fs", version : "");
export import(path : "onshape/std/revolve.fs", version : "");
export import(path : "onshape/std/transform.fs", version : "");
export import(path : "onshape/std/vector.fs", version : "");

export enum HoleStyle
{
    annotation { "Name" : "Simple" }
    SIMPLE,
    annotation { "Name" : "Counterbore" }
    C_BORE,
    annotation { "Name" : "Countersink" }
    C_SINK
}

export enum HoleEndStyle
{
    annotation { "Name" : "Through" }
    THROUGH,
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Blind in last" }
    BLIND_IN_LAST
}

annotation { "Feature Type Name" : "Hole" }
export const hole = defineFeature(function(context is Context, id is Id, holeDefinition is map)
    precondition
    {
        annotation { "Name" : "Hole style" }
        holeDefinition.style is HoleStyle;

        annotation { "Name" : "Hole diameter" }
        isLength(holeDefinition.holeDiameter, HOLE_DIAMETER_BOUNDS);

        if (holeDefinition.style == HoleStyle.C_BORE)
        {
            annotation { "Name" : "Counterbore diameter" }
            isLength(holeDefinition.cBoreDiameter, HOLE_BORE_DIAMETER_BOUNDS);
            annotation { "Name" : "Counterbore depth" }
            isLength(holeDefinition.cBoreDepth, HOLE_BORE_DEPTH_BOUNDS);
        }
        else if (holeDefinition.style == HoleStyle.C_SINK)
        {
            annotation { "Name" : "Countersink diameter" }
            isLength(holeDefinition.cSinkDiameter, HOLE_BORE_DIAMETER_BOUNDS);
        }

        annotation { "Name" : "Hole termination" }
        holeDefinition.endStyle is HoleEndStyle;

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        holeDefinition.oppositeDirection is boolean;

        if (holeDefinition.endStyle == HoleEndStyle.BLIND || holeDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST)
        {
            annotation { "Name" : "Hole depth" }
            isLength(holeDefinition.holeDepth, HOLE_DEPTH_BOUNDS);
        }

        annotation { "Name" : "Sketch points to place holes",
                     "Filter" : EntityType.VERTEX && SketchObject.YES && ConstructionObject.NO }
        holeDefinition.locations is Query;

        annotation { "Name" : "Merge scope",
                     "Filter" : (EntityType.BODY && BodyType.SOLID) }
        holeDefinition.scope is Query;

    }
    {
        var locations = evaluateQuery(context, holeDefinition.locations);
        if (@size(locations) == 0)
        {
            reportFeatureError(context, id, ErrorStringEnum.HOLE_NO_POINTS, ["locations"]);
            return;
        }

        var scopeTest = evaluateQuery(context, holeDefinition.scope);
        if (@size(scopeTest) == 0)
        {
            reportFeatureError(context, id, ErrorStringEnum.HOLE_EMPTY_SCOPE, ["scope"]);
            return;
        }

        var scopeSizeResult = scopeSize(context, holeDefinition.scope);
        if (hasError(scopeSizeResult))
        {
            reportFeatureError(context, id, ErrorStringEnum.HOLE_FAIL_BBOX, ["scope"]);
            return;
        }
        holeDefinition.scopeSize = scopeSizeResult.result;

        var startingBodyCount = @size(evaluateQuery(context, qEverything(EntityType.BODY)));
        // ------------- Perform the operation ---------------

        // for each hole
        var holeNumber = -1;
        var numSuccess = 0;
        for (var location in locations)
        {
            /**
             * for each point (vertex or circular arc) compute a cSys with the point at the
             * origin and +z pointing in the hole direction. At each location, cast cylindrical rays
             * from 'infinity' at the front and back of the targets. For each cylinder, find the furthest
             * point of contact. Find the closest of these points, this the 'start' of the hole.
             * Now cut the hole.
             */
            holeNumber += 1;
            var sketchPlane = evOwnerSketchPlane(context, { "entity" : location });
            if (hasError(sketchPlane))
            {
                continue;
            }
            var startPointCSys = planeToCSys(sketchPlane.result) as CoordSystem;
            if (hasError(startPointCSys))
            {
                continue;
            }
            var pointResult = evVertexPoint(context, { "vertex" : location });
            if (hasError(pointResult))
            {
                continue;
            }
            var point is Vector = pointResult.result;

            var sign = holeDefinition.oppositeDirection ? 1 : -1;
            startPointCSys = coordSystem(point, startPointCSys.xAxis, sign * startPointCSys.zAxis);

            var maxDiameter = holeDefinition.holeDiameter;
            if (holeDefinition.style == HoleStyle.C_BORE)
            {
                maxDiameter = holeDefinition.cBoreDiameter;
            } else if (holeDefinition.style == HoleStyle.C_SINK)
            {
                maxDiameter = holeDefinition.cSinkDiameter;
            }

            var holeId = id + ("hole-" ~ holeNumber);
            var startDistances = cylinderCastBiDir(context, holeId, {
                "scopeSize" : holeDefinition.scopeSize,
                "cSys" : startPointCSys,
                "diameter": maxDiameter,
                "scope" : holeDefinition.scope });
            if (!hasError(startDistances))
            {
                var result = cutHole (context, holeId, holeDefinition, startDistances, startPointCSys);
                if (!hasError(result))
                {
                    numSuccess += 1;
                }
            }
        }
        if (numSuccess == 0)
        {
            reportFeatureError(context, id, ErrorStringEnum.HOLE_NO_HITS);
        }

        var finalBodyCount = @size(evaluateQuery(context, qEverything(EntityType.BODY)));
        if (finalBodyCount > startingBodyCount)
        {
            reportFeatureError(context, id, ErrorStringEnum.HOLE_DISJOINT);
        }
    }, { endStyle : HoleEndStyle.BLIND, style : HoleStyle.SIMPLE, oppositeDirection : false,
    cSinkAngle : 45 * degree, cSinkUseDepth : false, cSinkDepth : 0 * meter });

const HOLE_DIAMETER_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0001, 0.005, 250],
    (millimeter) : [0.1,    5.0,   250000],
    (centimeter) : [0.01,   0.5,   25000],
    (inch)       : [0.001,  0.25,   10000],
    (foot)       : [0.0001, 0.015, 1000],
    (yard)       : [0.0001, 0.005, 250]
} as LengthBoundSpec;

const HOLE_BORE_DIAMETER_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0001, 0.01, 250],
    (millimeter) : [0.1,    10.0,   250000],
    (centimeter) : [0.01,   1.0,   25000],
    (inch)       : [0.001,  0.5,   10000],
    (foot)       : [0.0001, 0.03, 1000],
    (yard)       : [0.0001, 0.01, 250]
} as LengthBoundSpec;

const HOLE_DEPTH_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0001, 0.02, 250],
    (millimeter) : [0.1,    20.0,   250000],
    (centimeter) : [0.01,   2.0,   25000],
    (inch)       : [0.001,  1.5,   10000],
    (foot)       : [0.0001, 0.125, 1000],
    (yard)       : [0.0001, 0.041666667, 250]
} as LengthBoundSpec;

const HOLE_BORE_DEPTH_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0, 0.05,         250],
    (millimeter) : [0.0, 5.0,       250000],
    (centimeter) : [0.0, 0.5,        25000],
    (inch)       : [0.0, 0.25,       10000],
    (foot)       : [0.0, 0.020833333, 1000],
    (yard)       : [0.0, 0.069444444,  250]
} as LengthBoundSpec;

function hasError(result is map) returns boolean
{
  return (result.error != undefined);
}

/**
 * returns { dist: Length - z distance to first point of contact in cSys frame }
 */
function cylinderCast(context is Context, idIn is Id, arg is map) returns map
precondition
{
    isLength(arg.scopeSize);
    isLength(arg.diameter);
    arg.cSys is CoordSystem;
    arg.isFront is boolean;
    arg.findClosest is boolean;
    arg.scope is Query;
}
{
    var shotName = arg.isFront ? "shot_front" : "shot_back";
    var id = idIn + shotName;
    var distance = arg.scopeSize;

    var direction = arg.cSys.zAxis;
    if (!arg.isFront)
    {
        direction = -direction;
    }

    var startingCSys = coordSystem(arg.cSys.origin - distance * direction, arg.cSys.xAxis, direction);

    var sketchPlane = plane(startingCSys.origin, direction); // plane is rotated

    //------------- Create profile ----------------
    var sketchName = "raySketch";
    var sketch = newSketchOnPlane(context, id + sketchName, { "sketchPlane" : sketchPlane });

    if (sketch == undefined)
    {
        return { "error" : "Failed to create sketch." };
    }

    skCircle(sketch, "circle", { "center" : vector(0, 0) * meter, "radius" : arg.diameter / 2 });
    skSolve(sketch);

    var bestDist = undefined;
    var targets = evaluateQuery(context, arg.scope);
    if (@size(targets) == 0)
    {
        return { "error" : "No targets" };
    }

    var shotNum = -1;
    var sketchEntityQuery = sketchEntityQuery(id + (sketchName ~ ".wireOp"), EntityType.EDGE, "circle");
    var curves = evaluateQuery(context, sketchEntityQuery);

    for (var targetQuery in targets)
    {
        shotNum += 1;
        var operationId = id + shotNum;
        extrude(context, operationId, {
          "bodyType" : ToolBodyType.SURFACE,
          "operationType" : NewBodyOperationType.NEW,
          "surfaceEntities" : qUnion([sketchEntityQuery]),
          "endBound" : BoundingType.UP_TO_BODY,
          "endBoundEntity" : qUnion([targetQuery])
        });

        var bodyQuery = qCreatedBy(operationId, EntityType.BODY);
        var resolvedBodyQuery = evaluateQuery(context, bodyQuery);
        if (@size(resolvedBodyQuery) == 0)
        {
            continue;
        }

        var cylBox = evBox3d(context, { "topology" : bodyQuery, "cSys" : toWorld(arg.cSys) } );
        if (hasError(cylBox))
        {
            continue;
        }
        cylBox = cylBox.result;
        var d =  arg.isFront ? cylBox.maxCorner[2] : cylBox.minCorner[2];

        if (bestDist == undefined)
        {
            bestDist = d;
        }
        else if ((arg.findClosest && arg.isFront) || (!arg.findClosest && !arg.isFront))
        {
            if (d < bestDist)
            {
                bestDist = d;
            }
        }
        else
        {
            if (d > bestDist)
            {
                bestDist = d;
            }
        }

        deleteBodies(context, id  + ("delete_" ~ shotNum), { "entities" : bodyQuery });
    }
    deleteBodies(context, id  + "delete", { "entities" : qCreatedBy(id + sketchName, EntityType.BODY) });

    if (bestDist != undefined)
    {
        return { "dist" : bestDist };
    }
    return { "error" : "Hole not entirely on one body." };
}

function cylinderCastBiDir(context is Context, id is Id, arg is map) returns map
precondition
{
    arg.cSys is CoordSystem;
    isLength(arg.diameter);
    arg.scope is Query;
}
{
    // Small but finite distance.
    // A large distance does not work when trying to drill out from the inside of a box.
    // A zero distance produces no surface for the cast if the the target is planar face parallel to the sketch
    var smallFrontOffset = 0.01 * millimeter;

    var resultFront = cylinderCast(context, id, {
        "scopeSize" : smallFrontOffset,
        "cSys" : arg.cSys,
        "isFront" : true,
        "findClosest" : true,
        "diameter" : arg.diameter,
        "scope" : arg.scope });

    // The back of the part uses a large distance until we can find a better method.
    var resultBack = cylinderCast(context, id, {
        "scopeSize" : arg.scopeSize,
        "cSys" : arg.cSys,
        "isFront" : false,
        "findClosest" : true,
        "diameter" : arg.diameter,
        "scope" : arg.scope });
    var min = 0 * meter;
    var max = 0 * meter;
    var result = {};
    if (!hasError(resultFront))
    {
        result.frontDist = resultFront.dist;
    }
    else
    {
        result.frontDist = 0 * meter;
    }
    if (!hasError(resultBack))
    {
        result.backDist = resultBack.dist;
    }

    return result;
}

function cutHole(context is Context, id is Id, holeDefinition is map, startDistances is map, cSys is CoordSystem) returns map
{
    var result;
    var frontDist = 0 * meter;
    var backDist = 0 * meter;
    var isCBore = holeDefinition.style == HoleStyle.C_BORE;
    var isCSink = holeDefinition.style == HoleStyle.C_SINK;
    if (!hasError(startDistances))
    {
        frontDist = startDistances.frontDist;
        if (frontDist < 0)
        {
            frontDist = 0 * meter;
        }
        backDist = startDistances.backDist;
    }

    var sign = holeDefinition.oppositeDirection ? 1 : -1;
    var startCSys = coordSystem(cSys.origin, cSys.xAxis, sign * cSys.zAxis);

    var sketchPlane = plane(cSys.origin, cSys.xAxis, cSys.zAxis); // plane is rotated

    //------------- Create profile ----------------
    var sketchName = "sketch";
    var sketch = newSketchOnPlane(context, id + sketchName, { "sketchPlane" : sketchPlane });

    var startDepth = 0 * meter;
    if (isCBore)
    {
        result = sketchCBore(context, {
            "prefix" : "cbore_start",
            "sketch" : sketch,
            "startDepth" : startDepth,
            "endDepth" : frontDist + holeDefinition.cBoreDepth,
            "cBoreDiameter" : holeDefinition.cBoreDiameter });
        if (hasError(result))
        {
            return result;
        }
        startDepth = frontDist;
        frontDist = 0 * meter;
    }

    if (isCSink)
    {
        var cSinkStartDepth = startDepth;
        if (isCBore)
        {
            cSinkStartDepth += holeDefinition.cBoreDepth;
        }
        result = sketchCSink(context, {
            "prefix" : "csink_start",
            "sketch" : sketch,
            "isPositive" : true,
            "startDepth"     : cSinkStartDepth,
            "clearanceDepth" : frontDist,
            "cSinkUseDepth"  : holeDefinition.cSinkUseDepth,
            "cSinkDepth"     : holeDefinition.cSinkDepth,
            "cSinkDiameter"  : holeDefinition.cSinkDiameter,
            "cSinkAngle"     : holeDefinition.cSinkAngle });

        if (hasError(result))
        {
            return result;
        }
        if (frontDist != 0 * meter)
        {
            startDepth = frontDist;
        }
        frontDist = 0 * meter;
    }

    // Handle hole core
    result = sketchToolCore(context, id, {
        "prefix" : "core",
        "sketch" : sketch,
        "cSys" : cSys,
        "startDepth" : startDepth,
        "clearanceDepth" : frontDist,
        "holeDefinition" : holeDefinition });
    if (hasError(result))
    {
        return result;
    }


    skSolve(sketch);

    var axisQuery = sketchEntityQuery(id + ("sketch" ~ ".wireOp"), EntityType.EDGE, "core_line_0");
    var sketchQuery = qSketchRegion(id + sketchName, false);
    spinCut(context, id, sketchQuery, axisQuery, holeDefinition.scope, false);
    deleteBodies(context, id  + "delete_sketch", { "entities" : qCreatedBy(id + sketchName, EntityType.BODY) });
    // Also delete any unused bodies from the spin
    deleteBodies(context, id  + "delete", { "entities" : qCreatedBy(id, EntityType.BODY) });

    var newFaces = evaluateQuery(context, qCreatedBy(id, EntityType.FACE));
    if (@size(newFaces) == 0)
    {
        return { "error": "No faces created" };
    }

    return {};
}

function spinCut(context is Context, id is Id, sketchQuery is Query, axisQuery is Query, scopeQuery is Query, makeNew is boolean)
{
    revolve(context, id, {
        "bodyType" : ToolBodyType.SOLID,
        "operationType" : makeNew ? NewBodyOperationType.NEW : NewBodyOperationType.REMOVE,
        "entities" : qUnion([sketchQuery]),
        "axis" : qUnion([axisQuery]),
        "revolveType" : RevolveType.FULL,
        "booleanScope" : scopeQuery,
        "defaultScope" : false });
}

function sketchPoly(context is Context, prefix is string, sketch is Sketch, points is array) returns map
{
    if (sketch == undefined)
    {
        return { "error" : -1 };
    }

    var numPoints = @size(points);
    for (var index = 0; index < numPoints; index += 1)
    {
        var lineId = prefix ~ "_line_" ~ index;
        skLineSegment(sketch, lineId, { "start" : points[index], "end" : points[(index + 1) % numPoints] });
    }

    return {};
}

function sketchCBore(context is Context, arg is map) returns map
precondition
{
    arg.prefix is string;
    arg.sketch is Sketch;
    isLength(arg.startDepth);
    isLength(arg.endDepth);
    isLength(arg.cBoreDiameter);
}
{
    var cBoreRadius = arg.cBoreDiameter / 2;
    var points = [vector(arg.startDepth, 0 * meter),
                  vector(arg.startDepth + arg.endDepth, 0 * meter),
                  vector(arg.startDepth + arg.endDepth, cBoreRadius),
                  vector(arg.startDepth, cBoreRadius)];
    return sketchPoly(context, arg.prefix, arg.sketch, points);
}

function sketchCSink(context is Context, arg is map) returns map
precondition
{
    arg.prefix is string;
    arg.sketch is Sketch;
    arg.cSinkUseDepth is boolean;
    arg.isPositive is boolean;
    isLength(arg.startDepth);
    isLength(arg.clearanceDepth);
    isLength(arg.cSinkDepth);
    isLength(arg.cSinkDiameter);
    isAngle(arg.cSinkAngle);
}
{
    var cSinkRadius = 0 * meter;
    var cSinkDepth = arg.cSinkDepth;
    if (arg.cSinkUseDepth)
    {
        cSinkRadius = sin(arg.cSinkAngle) * cSinkDepth;
    }
    else
    {
        cSinkRadius = arg.cSinkDiameter / 2;
        cSinkDepth = cSinkRadius / sin(arg.cSinkAngle);
    }

    var sign = arg.isPositive ? 1 : -1;
    var points;
    if (arg.clearanceDepth > 0 * meter)
    {
        points = [vector(arg.startDepth, 0 * meter),
                  vector(arg.startDepth + sign * (arg.clearanceDepth + cSinkDepth), 0 * meter),
                  vector(arg.startDepth + sign * (arg.clearanceDepth), cSinkRadius),
                  vector(arg.startDepth, cSinkRadius)];
    }
    else
    {
        points = [vector(arg.startDepth, 0 * meter),
                  vector(arg.startDepth + sign * cSinkDepth, 0 * meter),
                  vector(arg.startDepth, cSinkRadius)];

    }
    return sketchPoly(context, arg.prefix, arg.sketch, points);
}

function sketchToolCore(context is Context, id is Id, arg is map) returns map
precondition
{
  is3dLengthVector(arg.cSys.origin);
    arg.prefix is string;
    arg.sketch is Sketch;
    arg.cSys is CoordSystem;
    isLength(arg.startDepth);
    isLength(arg.clearanceDepth);
    arg.holeDefinition is map;
}
{
    var radius = arg.holeDefinition.holeDiameter * 0.5;
    var tipAngle = 118.0 / 2.0 * degree;
    var depth = arg.holeDefinition.holeDepth;
    var useTipLength = false;
    if (arg.holeDefinition.endStyle == HoleEndStyle.THROUGH)
    {
        depth = arg.holeDefinition.scopeSize * 2;
    }

    else if (arg.holeDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST)
    {
        // Find shoulder depth
        var result = cylinderCast(context, id + "limit_surf_cast", {
            "scopeSize" : arg.holeDefinition.scopeSize * 2,
            "cSys" : arg.cSys,
            "isFront" : true,
            "findClosest" : false,
            "diameter" : 2 * radius,
            "scope" : arg.holeDefinition.scope });
        if (!hasError(result))
        {
            depth += result.dist - arg.startDepth - arg.clearanceDepth;
        }

        if (useTipLength)
        {
            depth += cos(tipAngle) * radius;
        }
    }

    var points;
    if (useTipLength)
    {
        points = [vector(arg.startDepth, 0 * meter),
                  vector(arg.startDepth + arg.clearanceDepth + depth, 0 * meter),
                  vector(arg.startDepth + arg.clearanceDepth + depth - radius * cos(tipAngle), radius),
                  vector(arg.startDepth, radius)];
    }
    else
    {
        points = [vector(arg.startDepth, 0 * meter),
                  vector(arg.startDepth + arg.clearanceDepth + depth + radius * cos(tipAngle), 0 * meter),
                  vector(arg.startDepth + arg.clearanceDepth + depth, radius),
                  vector(arg.startDepth, radius)];

    }

    return sketchPoly(context, arg.prefix, arg.sketch, points);
    return {};
}

function scopeSize(context is Context, scope is Query) returns map
{
    var scopeBox = evBox3d(context, { "topology" : scope});
    if (!hasError(scopeBox))
    {
        scopeBox = scopeBox.result;
        var depth = norm(scopeBox.maxCorner - scopeBox.minCorner);
        return { "result" :  depth };
    }
    else
    {
        return scopeBox;
    }
}

export function holeScopeFlipHeuristicsCall(context is Context, holeDefinition is map, featureInfo is map) returns map
{
    var numAligned = 0;
    var numAntiAligned = 0;
    var locations = evaluateQuery(context, holeDefinition.locations);
    var queries = [];
    for (var location in locations)
    {
        var sketchPlane = evOwnerSketchPlane(context, { "entity" : location });
        if (sketchPlane.error != undefined)
        {
            continue;
        }
        var pointResult = evVertexPoint(context, { "vertex": location });
        var solidBodiesQuery is Query = qBodyType(qEverything(EntityType.BODY), BodyType.SOLID);
        if (featureInfo.excludeBodies is Query)
        {
          solidBodiesQuery = qSubtraction(solidBodiesQuery, featureInfo.excludeBodies);
        }
        var faces = evaluateQuery(context, qContainsPoint(qOwnedByPart(solidBodiesQuery, EntityType.FACE), pointResult.result));
        if (@size(faces) == 1)
        {
            // Only add non-ambiguous parts
            var bodyQuery = evaluateQuery(context, qOwnerPart(faces[0]));
            if (@size(bodyQuery) == 1)
            {
                var facePlane = evPlane(context, { "face" : faces[0] });
                if (facePlane.error == undefined)
                {
                    var needFlip = dot(sketchPlane.result.normal, facePlane.result.normal) < 0;
                    if (needFlip)
                    {
                        numAntiAligned += 1;
                    }
                    else
                    {
                        numAligned += 1;
                    }
                }
                queries = append(queries, bodyQuery[0]);
            }
        }
    }
    // This collapses the list to only the unique queries.
    holeDefinition.scope = qUnion(evaluateQuery(context, qUnion(queries)));

    if (numAligned == 0 && numAntiAligned != 0)
    {
        holeDefinition.oppositeDirection = true;
    }
    else if (numAntiAligned == 0 && numAligned != 0)
    {
        holeDefinition.oppositeDirection = false;
    }
    return holeDefinition;
}

