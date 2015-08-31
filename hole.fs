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
export const hole = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Hole style", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        definition.style is HoleStyle;

        annotation { "Name" : "Hole diameter", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        isLength(definition.holeDiameter, HOLE_DIAMETER_BOUNDS);

        if (definition.style == HoleStyle.C_BORE)
        {
            annotation { "Name" : "Counterbore diameter", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            isLength(definition.cBoreDiameter, HOLE_BORE_DIAMETER_BOUNDS);
            annotation { "Name" : "Counterbore depth", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            isLength(definition.cBoreDepth, HOLE_BORE_DEPTH_BOUNDS);
        }
        else if (definition.style == HoleStyle.C_SINK)
        {
            annotation { "Name" : "Countersink diameter", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            isLength(definition.cSinkDiameter, HOLE_BORE_DIAMETER_BOUNDS);
        }

        annotation { "Name" : "Hole termination", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        definition.endStyle is HoleEndStyle;

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;

        if (definition.endStyle == HoleEndStyle.BLIND || definition.endStyle == HoleEndStyle.BLIND_IN_LAST)
        {
            annotation { "Name" : "Hole depth", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            isLength(definition.holeDepth, HOLE_DEPTH_BOUNDS);
        }

        annotation { "Name" : "Sketch points to place holes",
                     "Filter" : EntityType.VERTEX && SketchObject.YES && ConstructionObject.NO }
        definition.locations is Query;

        annotation { "Name" : "Merge scope",
                     "Filter" : (EntityType.BODY && BodyType.SOLID) }
        definition.scope is Query;

    }
    {
        // V206 was the current version when it was determined that a version check was needed
        if (definition.style == HoleStyle.C_BORE && isAtVersionOrLater(context, FeatureScriptVersionNumber.V206_LINEAR_RANGE))
        {
            if (definition.holeDiameter > definition.cBoreDiameter)
                reportFeatureError(context, id, ErrorStringEnum.HOLE_CBORE_TOO_SMALL);

            if (definition.endStyle == HoleEndStyle.BLIND && definition.holeDepth < definition.cBoreDepth)
                reportFeatureError(context, id, ErrorStringEnum.HOLE_CBORE_TOO_DEEP);
        }
        if (definition.style == HoleStyle.C_SINK && isAtVersionOrLater(context, FeatureScriptVersionNumber.V206_LINEAR_RANGE))
        {
            if (definition.holeDiameter > definition.cSinkDiameter)
                reportFeatureError(context, id, ErrorStringEnum.HOLE_CSINK_TOO_SMALL);

            var cSinkDepth = (definition.cSinkDiameter / 2) / tan(definition.cSinkAngle);
            if (definition.endStyle == HoleEndStyle.BLIND && definition.holeDepth < cSinkDepth)
                reportFeatureError(context, id, ErrorStringEnum.HOLE_CSINK_TOO_DEEP);
        }
        var locations = reduceLocations(context, definition.locations);
        if (size(locations) == 0)
            throw regenError(ErrorStringEnum.HOLE_NO_POINTS, ["locations"]);

        var scopeTest = evaluateQuery(context, definition.scope);
        if (size(scopeTest) == 0)
            throw regenError(ErrorStringEnum.HOLE_EMPTY_SCOPE, ["scope"]);

        definition.scopeSize = try(scopeSize(context, definition.scope));
        if (definition.scopeSize == undefined)
            throw regenError(ErrorStringEnum.HOLE_FAIL_BBOX, ["scope"]);

        var startingBodyCount = size(evaluateQuery(context, qEverything(EntityType.BODY)));
        // ------------- Perform the operation ---------------

        // for each hole
        var holeNumber = -1;
        var numSuccess = 0;
        for (var location in locations)
        {
            try
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
                var startPointCSys = planeToCSys(sketchPlane);
                var point is Vector = evVertexPoint(context, { "vertex" : location });

                var sign = definition.oppositeDirection ? 1 : -1;
                startPointCSys = coordSystem(point, startPointCSys.xAxis, sign * startPointCSys.zAxis);

                var maxDiameter = definition.holeDiameter;
                if (definition.style == HoleStyle.C_BORE)
                    maxDiameter = definition.cBoreDiameter;
                else if (definition.style == HoleStyle.C_SINK)
                    maxDiameter = definition.cSinkDiameter;

                var holeId = id + ("hole-" ~ holeNumber);

                var startDistances = cylinderCastBiDir(context, holeId, {
                    "scopeSize" : definition.scopeSize,
                    "cSys" : startPointCSys,
                    "diameter": maxDiameter,
                    "scope" : definition.scope });

                cutHole (context, holeId, definition, startDistances, startPointCSys);

                numSuccess += 1;
            }
        }
        if (numSuccess == 0)
            throw regenError(ErrorStringEnum.HOLE_NO_HITS);

        var finalBodyCount = size(evaluateQuery(context, qEverything(EntityType.BODY)));
        if (finalBodyCount > startingBodyCount)
            throw regenError(ErrorStringEnum.HOLE_DISJOINT);
    }, { endStyle : HoleEndStyle.BLIND, style : HoleStyle.SIMPLE, oppositeDirection : false,
         cSinkAngle : 45 * degree, cSinkUseDepth : false, cSinkDepth : 0 * meter });

function reduceLocations(context is Context, rawLocationQuery is Query) returns array
{
    var rawLocations = evaluateQuery(context, rawLocationQuery);
    var pts = [];
    var locations = [];
    for (var rawLocation in rawLocations)
    {
        var ptResult = try(evVertexPoint(context, { "vertex" : rawLocation }));
        if (ptResult != undefined)
        {
            var found = false;
            for (var testPt in pts)
            {
                if (samePoint(ptResult, testPt))
                    found = true;
            }
            if (!found)
            {
                pts = append(pts, ptResult);
                locations = append(locations, rawLocation);
            }
        }
    }
    return locations;
}

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


function depthOfRadius(context is Context, radius is ValueWithUnits, angle is ValueWithUnits)
precondition
{
    isLength(radius);
    isAngle(angle);
}
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V190_HOLE_FEATURE_ANGLE_MATH))
    {
        return radius / tan(angle);
    }
    else
    {
        return cos(angle) * radius;
    }
}

function radiusOfDepth(context is Context, depth is ValueWithUnits, angle is ValueWithUnits)
precondition
{
    isLength(depth);
    isAngle(angle);
}
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V190_HOLE_FEATURE_ANGLE_MATH))
    {
        return depth * tan(angle);
    }
    else
    {
        return sin(angle) * depth;
    }
}

/**
 * returns { dist: Length - z distance to first point of contact in cSys frame }
 */
function cylinderCast(context is Context, idIn is Id, arg is map) returns ValueWithUnits
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
        direction = -direction;

    var startingCSys = coordSystem(arg.cSys.origin - distance * direction, arg.cSys.xAxis, direction);

    var sketchPlane = plane(startingCSys.origin, direction); // plane is rotated

    //------------- Create profile ----------------
    var sketchName = "raySketch";
    var sketch = newSketchOnPlane(context, id + sketchName, { "sketchPlane" : sketchPlane });

    skCircle(sketch, "circle", { "center" : vector(0, 0) * meter, "radius" : arg.diameter / 2 });
    skSolve(sketch);

    var bestDist = undefined;
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V208_HOLE_FEATURE_HANDLE_FAILED_EXTRUDE))
    {
        bestDist = 0 * meter;
    }
    var targets = evaluateQuery(context, arg.scope);
    if (size(targets) == 0)
        throw "No targets";

    var shotNum = -1;
    var sketchEntityQuery = sketchEntityQuery(id + (sketchName ~ ".wireOp"), EntityType.EDGE, "circle");
    var curves = evaluateQuery(context, sketchEntityQuery);

    for (var targetQuery in targets)
    {
        shotNum += 1;
        var operationId = id + shotNum;
          try {
              extrude(context, operationId, {
                "bodyType" : ToolBodyType.SURFACE,
                "operationType" : NewBodyOperationType.NEW,
                "surfaceEntities" : qUnion([sketchEntityQuery]),
                "endBound" : BoundingType.UP_TO_BODY,
                "endBoundEntityBody" : qUnion([targetQuery])
              });
          } catch (e) {
              if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V208_HOLE_FEATURE_HANDLE_FAILED_EXTRUDE))
              {
                  continue;
              }
              else
              {
                  throw e;
              }
          }
        var bodyQuery = qCreatedBy(operationId, EntityType.BODY);

        var cylBox = evBox3d(context, { "topology" : bodyQuery, "cSys" : toWorld(arg.cSys) } );

        var d =  arg.isFront ? cylBox.maxCorner[2] : cylBox.minCorner[2];

        if (bestDist == undefined)
        {
            bestDist = d;
        }
        else if ((arg.findClosest && arg.isFront) || (!arg.findClosest && !arg.isFront))
        {
            if (d < bestDist)
                bestDist = d;
        }
        else
        {
            if (d > bestDist)
                bestDist = d;
        }

        deleteBodies(context, id  + ("delete_" ~ shotNum), { "entities" : bodyQuery });
    }
    deleteBodies(context, id  + "delete", { "entities" : qCreatedBy(id + sketchName, EntityType.BODY) });

    if (bestDist == undefined)
        throw "Hole not entirely on one body.";

    return bestDist;
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

    var resultFront = try(cylinderCast(context, id, {
        "scopeSize" : smallFrontOffset,
        "cSys" : arg.cSys,
        "isFront" : true,
        "findClosest" : true,
        "diameter" : arg.diameter,
        "scope" : arg.scope }));

    // The back of the part uses a large distance until we can find a better method.
    var resultBack = try(cylinderCast(context, id, {
        "scopeSize" : arg.scopeSize,
        "cSys" : arg.cSys,
        "isFront" : false,
        "findClosest" : true,
        "diameter" : arg.diameter,
        "scope" : arg.scope }));

    var result = {};

    if (resultFront != undefined)
        result.frontDist = resultFront;
    else
        result.frontDist = 0 * meter;

    result.backDist = resultBack; // ok if undefined

    return result;
}

function cutHole(context is Context, id is Id, holeDefinition is map, startDistances is map, cSys is CoordSystem)
{
    var result;
    var frontDist = 0 * meter;
    var backDist = 0 * meter;
    var isCBore = holeDefinition.style == HoleStyle.C_BORE;
    var isCSink = holeDefinition.style == HoleStyle.C_SINK;

    frontDist = max(startDistances.frontDist, 0 * meter);
    backDist = startDistances.backDist;

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

        startDepth = frontDist;
        frontDist = 0 * meter;
    }

    if (isCSink)
    {
        var cSinkStartDepth = startDepth;
        if (isCBore)
            cSinkStartDepth += holeDefinition.cBoreDepth;

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

        if (frontDist != 0 * meter)
            startDepth = frontDist;
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

    skSolve(sketch);

    var axisQuery = sketchEntityQuery(id + ("sketch" ~ ".wireOp"), EntityType.EDGE, "core_line_0");
    var sketchQuery = qSketchRegion(id + sketchName, false);
    spinCut(context, id, sketchQuery, axisQuery, holeDefinition.scope, false);
    deleteBodies(context, id  + "delete_sketch", { "entities" : qCreatedBy(id + sketchName, EntityType.BODY) });
    // Also delete any unused bodies from the spin
    var extraBodies = evaluateQuery(context, qCreatedBy(id, EntityType.BODY));
    if (size(extraBodies) > 0)
    {
        deleteBodies(context, id  + "delete", { "entities" : qCreatedBy(id, EntityType.BODY) });
    }
    var newFaces = evaluateQuery(context, qCreatedBy(id, EntityType.FACE));
    if (size(newFaces) == 0)
        throw "No faces created";
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

function sketchPoly(context is Context, prefix is string, sketch is Sketch, points is array)
{
    var numPoints = size(points);
    for (var index = 0; index < numPoints; index += 1)
    {
        var lineId = prefix ~ "_line_" ~ index;
        skLineSegment(sketch, lineId, { "start" : points[index], "end" : points[(index + 1) % numPoints] });
    }
}

function sketchCBore(context is Context, arg is map)
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
    sketchPoly(context, arg.prefix, arg.sketch, points);
}

function sketchCSink(context is Context, arg is map)
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
        cSinkRadius = radiusOfDepth(context, cSinkDepth, arg.cSinkAngle);
    }
    else
    {
        cSinkRadius = arg.cSinkDiameter / 2;
        cSinkDepth = depthOfRadius(context, cSinkRadius, arg.cSinkAngle);
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
    sketchPoly(context, arg.prefix, arg.sketch, points);
}

function sketchToolCore(context is Context, id is Id, arg is map)
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
    var tipDepth = depthOfRadius(context, radius, tipAngle);
    if (arg.holeDefinition.endStyle == HoleEndStyle.THROUGH)
    {
        depth = arg.holeDefinition.scopeSize * 2;
    }
    else if (arg.holeDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST)
    {
        // Find shoulder depth
        var result = try(cylinderCast(context, id + "limit_surf_cast", {
            "scopeSize" : arg.holeDefinition.scopeSize * 2,
            "cSys" : arg.cSys,
            "isFront" : true,
            "findClosest" : false,
            "diameter" : 2 * radius,
            "scope" : arg.holeDefinition.scope }));
        if (result != undefined)
            depth += result - arg.startDepth - arg.clearanceDepth;

        if (useTipLength)
        {
            depth += tipDepth;
        }
    }

    var points;
    if (useTipLength)
    {
        points = [vector(arg.startDepth, 0 * meter),
                  vector(arg.startDepth + arg.clearanceDepth + depth, 0 * meter),
                  vector(arg.startDepth + arg.clearanceDepth + depth - tipDepth, radius),
                  vector(arg.startDepth, radius)];
    }
    else
    {
        points = [vector(arg.startDepth, 0 * meter),
                  vector(arg.startDepth + arg.clearanceDepth + depth + tipDepth, 0 * meter),
                  vector(arg.startDepth + arg.clearanceDepth + depth, radius),
                  vector(arg.startDepth, radius)];

    }

    sketchPoly(context, arg.prefix, arg.sketch, points);
}

function scopeSize(context is Context, scope is Query) returns map
{
    var scopeBox = evBox3d(context, { "topology" : scope});
    return norm(scopeBox.maxCorner - scopeBox.minCorner);
}

export function holeScopeFlipHeuristicsCall(context is Context, holeDefinition is map, featureInfo is map) returns map
{
    var scopeSet = false;
    var oppositeDirectionSet = false;
    var oppositeDirectionChanged = false;
    var oppositeDirection = holeDefinition.oppositeDirection;
    if (featureInfo.specifiedParameters != undefined)
    {
        scopeSet = featureInfo.specifiedParameters.scope != undefined;
        oppositeDirectionSet = featureInfo.specifiedParameters.oppositeDirection != undefined;
    }

    if (scopeSet && oppositeDirectionSet)
        return {};

    var locations = evaluateQuery(context, holeDefinition.locations);
    var queries = [];
    for (var location in locations)
    {
        var sketchPlane = try(evOwnerSketchPlane(context, { "entity" : location }));
        if (sketchPlane == undefined)
            continue;

        var pointResult = evVertexPoint(context, { "vertex": location });
        var solidBodiesQuery is Query = qNothing();
        if (scopeSet)
        {
            solidBodiesQuery = holeDefinition.scope;
        }
        else
        {
            solidBodiesQuery = qBodyType(qEverything(EntityType.BODY), BodyType.SOLID);
            if (featureInfo.excludeBodies is Query)
            {
              solidBodiesQuery = qSubtraction(solidBodiesQuery, featureInfo.excludeBodies);
            }
        }
        var faces = evaluateQuery(context, qContainsPoint(qGeometry(qOwnedByPart(solidBodiesQuery, EntityType.FACE), GeometryType.PLANE), pointResult));
        if (@size(faces) != 1 && !oppositeDirectionSet)
            continue;

        for (var face in faces)
        {
            var facePlane = try(evPlane(context, { "face" : face }));
            if (facePlane != undefined)
            {
                var needFlip = dot(sketchPlane.normal, facePlane.normal) < 0;
                if (!oppositeDirectionSet)
                {
                    oppositeDirectionChanged = (oppositeDirection != needFlip);
                    oppositeDirection = needFlip;
                    oppositeDirectionSet = true;
                }
                if (needFlip == oppositeDirection)
                {
                    // Only add non-ambiguous parts
                    var bodyQuery = evaluateQuery(context, qOwnerPart(face));
                    if (@size(bodyQuery) == 1)
                        queries = append(queries, bodyQuery[0]);
                }
            }
        }
    }
    // This collapses the list to only the unique queries.
    if (!scopeSet)
        holeDefinition.scope = qUnion(evaluateQuery(context, qUnion(queries)));

    if (oppositeDirectionChanged)
        holeDefinition.oppositeDirection = oppositeDirection;

    return holeDefinition;
}

