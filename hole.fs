FeatureScript 244; /* Automatically generated version */
import(path : "onshape/std/boolean.fs", version : "");
import(path : "onshape/std/boundingtype.gen.fs", version : "");
import(path : "onshape/std/box.fs", version : "");
import(path : "onshape/std/clashtype.gen.fs", version : "");
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/coordSystem.fs", version : "");
import(path : "onshape/std/evaluate.fs", version : "");
import(path : "onshape/std/extrude.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/mathUtils.fs", version : "");
import(path : "onshape/std/revolve.fs", version : "");
import(path : "onshape/std/sketch.fs", version : "");
import(path : "onshape/std/surfaceGeometry.fs", version : "");
import(path : "onshape/std/tool.fs", version : "");
import(path : "onshape/std/valueBounds.fs", version : "");
import(path : "onshape/std/string.fs", version : "");

/**
 * TODO: description
 */
export enum HoleStyle
{
    annotation { "Name" : "Simple" }
    SIMPLE,
    annotation { "Name" : "Counterbore" }
    C_BORE,
    annotation { "Name" : "Countersink" }
    C_SINK
}

/**
 * TODO: description
 */
export enum HoleEndStyle
{
    annotation { "Name" : "Through" }
    THROUGH,
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Blind in last" }
    BLIND_IN_LAST
}

/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */

annotation { "Feature Type Name" : "Hole", "Editing Logic Function" : "holeEditLogic"}
export const hole = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Hole style" }
        definition.style is HoleStyle;

        annotation { "Name" : "Hole diameter" }
        isLength(definition.holeDiameter, HOLE_DIAMETER_BOUNDS);

        if (definition.style == HoleStyle.C_BORE)
        {
            annotation { "Name" : "Counterbore diameter" }
            isLength(definition.cBoreDiameter, HOLE_BORE_DIAMETER_BOUNDS);
            annotation { "Name" : "Counterbore depth" }
            isLength(definition.cBoreDepth, HOLE_BORE_DEPTH_BOUNDS);
        }
        else if (definition.style == HoleStyle.C_SINK)
        {
            annotation { "Name" : "Countersink diameter" }
            isLength(definition.cSinkDiameter, HOLE_BORE_DIAMETER_BOUNDS);
        }

        annotation { "Name" : "Hole termination" }
        definition.endStyle is HoleEndStyle;

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;

        if (definition.endStyle == HoleEndStyle.BLIND || definition.endStyle == HoleEndStyle.BLIND_IN_LAST)
        {
            annotation { "Name" : "Hole depth" }
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

            const cSinkDepth = (definition.cSinkDiameter / 2) / tan(definition.cSinkAngle);
            if (definition.endStyle == HoleEndStyle.BLIND && definition.holeDepth < cSinkDepth)
                reportFeatureError(context, id, ErrorStringEnum.HOLE_CSINK_TOO_DEEP);
        }
        const locations = reduceLocations(context, definition.locations);
        if (size(locations) == 0)
            throw regenError(ErrorStringEnum.HOLE_NO_POINTS, ["locations"]);

        const scopeTest = evaluateQuery(context, definition.scope);
        if (size(scopeTest) == 0)
            throw regenError(ErrorStringEnum.HOLE_EMPTY_SCOPE, ["scope"]);

        definition.scopeSize = try(scopeSize(context, definition.scope));
        if (definition.scopeSize == undefined)
            throw regenError(ErrorStringEnum.HOLE_FAIL_BBOX, ["scope"]);

        const startingBodyCount = size(evaluateQuery(context, qEverything(EntityType.BODY)));
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
                const sketchPlane = evOwnerSketchPlane(context, { "entity" : location });
                var startPointCSys = planeToCSys(sketchPlane);
                const point is Vector = evVertexPoint(context, { "vertex" : location });

                const sign = definition.oppositeDirection ? 1 : -1;
                startPointCSys = coordSystem(point, startPointCSys.xAxis, sign * startPointCSys.zAxis);

                var maxDiameter = definition.holeDiameter;
                if (definition.style == HoleStyle.C_BORE)
                    maxDiameter = definition.cBoreDiameter;
                else if (definition.style == HoleStyle.C_SINK)
                    maxDiameter = definition.cSinkDiameter;

                const holeId = id + ("hole-" ~ holeNumber);

                const startDistances = cylinderCastBiDir(context, holeId, {
                    "scopeSize" : definition.scopeSize,
                    "cSys" : startPointCSys,
                    "diameter": maxDiameter,
                    "scope" : definition.scope });

                cutHole(context, holeId, definition, startDistances, startPointCSys);

                numSuccess += 1;
            }
        }
        if (numSuccess == 0)
            throw regenError(ErrorStringEnum.HOLE_NO_HITS);

        const finalBodyCount = size(evaluateQuery(context, qEverything(EntityType.BODY)));
        if (finalBodyCount > startingBodyCount)
            throw regenError(ErrorStringEnum.HOLE_DISJOINT);
    }, { endStyle : HoleEndStyle.BLIND, style : HoleStyle.SIMPLE, oppositeDirection : false,
         cSinkAngle : 45 * degree, cSinkUseDepth : false, cSinkDepth : 0 * meter });

function reduceLocations(context is Context, rawLocationQuery is Query) returns array
{
    const rawLocations = evaluateQuery(context, rawLocationQuery);
    var pts = [];
    var locations = [];
    for (var rawLocation in rawLocations)
    {
        const ptResult = try(evVertexPoint(context, { "vertex" : rawLocation }));
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
    const shotName = arg.isFront ? "shot_front" : "shot_back";
    const id = idIn + shotName;
    const distance = arg.scopeSize;

    var direction = arg.cSys.zAxis;
    if (!arg.isFront)
        direction = -direction;

    const startingCSys = coordSystem(arg.cSys.origin - distance * direction, arg.cSys.xAxis, direction);

    const sketchPlane = plane(startingCSys.origin, direction); // plane is rotated

    //------------- Create profile ----------------
    const sketchName = "raySketch";
    const sketch = newSketchOnPlane(context, id + sketchName, { "sketchPlane" : sketchPlane });

    skCircle(sketch, "circle", { "center" : vector(0, 0) * meter, "radius" : arg.diameter / 2 });
    skSolve(sketch);

    var bestDist = undefined;
    const AOL_HOLE_FEATURE_HANDLE_FAILED_EXTRUDE = isAtVersionOrLater(context, FeatureScriptVersionNumber.V208_HOLE_FEATURE_HANDLE_FAILED_EXTRUDE);
    const AOL_HOLE_FEATURE_FIX_BROKEN_DEPTH_CALC = isAtVersionOrLater(context, FeatureScriptVersionNumber.V225_HOLE_FEATURE_FIX_BROKEN_DEPTH_CALC);
    // Fix for 208 was too broad, don't fix it here, fix later
    if (AOL_HOLE_FEATURE_HANDLE_FAILED_EXTRUDE && !AOL_HOLE_FEATURE_FIX_BROKEN_DEPTH_CALC)
    {
        bestDist = 0 * meter;
    }
    const targets = evaluateQuery(context, arg.scope);
    if (size(targets) == 0)
        throw "No targets";

    var shotNum = -1;
    const sketchEntityQuery = sketchEntityQuery(id + (sketchName ~ ".wireOp"), EntityType.EDGE, "circle");
    const curves = evaluateQuery(context, sketchEntityQuery);

    for (var targetQuery in targets)
    {
        shotNum += 1;
        const operationId = id + shotNum;
        try {
            extrude(context, operationId, {
                "bodyType" : ToolBodyType.SURFACE,
                "operationType" : NewBodyOperationType.NEW,
                "surfaceEntities" : qUnion([sketchEntityQuery]),
                "endBound" : BoundingType.UP_TO_BODY,
                "endBoundEntityBody" : qUnion([targetQuery])
            });
        } catch (e) {
            if (AOL_HOLE_FEATURE_HANDLE_FAILED_EXTRUDE)
            {
                continue;
            }
            else
            {
                throw e;
            }
        }
        const bodyQuery = qCreatedBy(operationId, EntityType.BODY);

        const cylBox = evBox3d(context, { "topology" : bodyQuery, "cSys" : arg.cSys } );

        var d = arg.isFront ? cylBox.maxCorner[2] : cylBox.minCorner[2];

        // This is the front plane clamp to zero
        const isMinimumCase = (arg.findClosest && arg.isFront) || (!arg.findClosest && !arg.isFront);
        if (AOL_HOLE_FEATURE_FIX_BROKEN_DEPTH_CALC && isMinimumCase && d < 0) {
            d = 0 * meter;
        }

        if (AOL_HOLE_FEATURE_FIX_BROKEN_DEPTH_CALC) {
            const collisions = evCollision(context, {'targets': targetQuery, 'tools': bodyQuery});
            for (var collision in collisions) {
                if (collision['type'] == ClashType.INTERFERE) {
                // If the shot cylinder interferes with the target, then we began the shot on the interior,
                // fall back to starting from the sketch plane.
                    d = 0 * meter;
                    break;
                }
            }
        }

        if (bestDist == undefined)
        {
            bestDist = d;
        }
        else if (isMinimumCase)
        {
            if (d < bestDist)
                bestDist = d;
        }
        else
        {
            if (d > bestDist)
                bestDist = d;
        }

        opDeleteBodies(context, id + ("delete_" ~ shotNum), { "entities" : bodyQuery });
    }
    opDeleteBodies(context, id + "delete", { "entities" : qCreatedBy(id + sketchName, EntityType.BODY) });

    if (AOL_HOLE_FEATURE_FIX_BROKEN_DEPTH_CALC) {
        if (bestDist == undefined) {
            bestDist = 0 * meter;
        }
    } else if (bestDist == undefined) {
        throw "Hole not entirely on one body.";
    }

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
    const smallFrontOffset = 0.01 * millimeter;

    const resultFront = try(cylinderCast(context, id, {
        "scopeSize" : smallFrontOffset,
        "cSys" : arg.cSys,
        "isFront" : true,
        "findClosest" : true,
        "diameter" : arg.diameter,
        "scope" : arg.scope }));

    // The back of the part uses a large distance until we can find a better method.
    const resultBack = try(cylinderCast(context, id, {
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
    var frontDist = 0 * meter;
    const isCBore = holeDefinition.style == HoleStyle.C_BORE;
    const isCSink = holeDefinition.style == HoleStyle.C_SINK;

    frontDist = max(startDistances.frontDist, 0 * meter);

    const sign = holeDefinition.oppositeDirection ? 1 : -1;
    const startCSys = coordSystem(cSys.origin, cSys.xAxis, sign * cSys.zAxis);

    const sketchPlane = plane(cSys.origin, cSys.xAxis, cSys.zAxis); // plane is rotated

    //------------- Create profile ----------------
    const sketchName = "sketch";
    const sketch = newSketchOnPlane(context, id + sketchName, { "sketchPlane" : sketchPlane });

    var startDepth = 0 * meter;
    if (isCBore)
    {
        sketchCBore(context, {
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

        sketchCSink(context, {
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
    sketchToolCore(context, id, {
        "prefix" : "core",
        "sketch" : sketch,
        "cSys" : cSys,
        "startDepth" : startDepth,
        "clearanceDepth" : frontDist,
        "holeDefinition" : holeDefinition });

    skSolve(sketch);

    const axisQuery = sketchEntityQuery(id + ("sketch" ~ ".wireOp"), EntityType.EDGE, "core_line_0");
    const sketchQuery = qSketchRegion(id + sketchName, false);
    spinCut(context, id, sketchQuery, axisQuery, holeDefinition.scope, false);
    opDeleteBodies(context, id + "delete_sketch", { "entities" : qCreatedBy(id + sketchName, EntityType.BODY) });
    // Also delete any unused bodies from the spin
    const extraBodies = evaluateQuery(context, qCreatedBy(id, EntityType.BODY));
    if (size(extraBodies) > 0)
    {
        opDeleteBodies(context, id + "delete", { "entities" : qCreatedBy(id, EntityType.BODY) });
    }
    const newFaces = evaluateQuery(context, qCreatedBy(id, EntityType.FACE));
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
    const numPoints = size(points);
    for (var index = 0; index < numPoints; index += 1)
    {
        const lineId = prefix ~ "_line_" ~ index;
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
    const cBoreRadius = arg.cBoreDiameter / 2;
    const points = [vector(arg.startDepth, 0 * meter),
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

    const sign = arg.isPositive ? 1 : -1;
    var points;
    const startDepth = arg.startDepth;
    if (arg.clearanceDepth > 0 * meter)
    {
        points = [vector(startDepth, 0 * meter),
                  vector(startDepth + sign * (arg.clearanceDepth + cSinkDepth), 0 * meter),
                  vector(startDepth + sign * (arg.clearanceDepth), cSinkRadius),
                  vector(startDepth, cSinkRadius)];
    }
    else
    {
        points = [vector(startDepth, 0 * meter),
                  vector(startDepth + sign * cSinkDepth, 0 * meter),
                  vector(startDepth, cSinkRadius)];

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
    const radius = arg.holeDefinition.holeDiameter * 0.5;
    const tipAngle = 118.0 / 2.0 * degree;
    var depth = arg.holeDefinition.holeDepth;
    var useTipLength = false;
    const tipDepth = depthOfRadius(context, radius, tipAngle);
    if (arg.holeDefinition.endStyle == HoleEndStyle.THROUGH)
    {
        depth = arg.holeDefinition.scopeSize * 2;
    }
    else if (arg.holeDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST)
    {
        // Find shoulder depth
        const result = try(cylinderCast(context, id + "limit_surf_cast", {
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
    const scopeBox = evBox3d(context, { "topology" : scope});
    return norm(scopeBox.maxCorner - scopeBox.minCorner);
}

/**
 * implements heuristics for hole feature
 * @param context
 * @param id
 * @param oldDefinition {{
 *      @field TODO
 * }}
 * @param definition {{
 *      @field TODO
 * }}
 * @param isCreating
 * @param specifiedParameters {{
 *      @field TODO
 * }}
 * @param hiddenBodies
 */
export function holeEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (oldDefinition.locations != definition.locations)
    {
        definition.locations =  clusterVertexQueries(context, definition.locations);
    }
    if (!isCreating || (specifiedParameters.scope && specifiedParameters.oppositeDirection))
    {
        return definition;
    }
    return holeScopeFlipHeuristicsCall(context, definition, specifiedParameters, hiddenBodies);
}

/**
 * TODO: description
 * @param context
 * @param holeDefinition {{
 *      @field TODO
 * }}
 * @param specifiedParameters {{
 *      @field TODO
 * }}
 * @param hiddenBodies
 */
export function holeScopeFlipHeuristicsCall(context is Context, holeDefinition is map, specifiedParameters is map, hiddenBodies is Query) returns map
{
    var scopeSet = specifiedParameters.scope;
    var oppositeDirectionSet = specifiedParameters.oppositeDirection;
    var oppositeDirectionChanged = false;
    var oppositeDirection = holeDefinition.oppositeDirection;

    if (scopeSet && oppositeDirectionSet)
        return {};

    const locations = evaluateQuery(context, holeDefinition.locations);
    var queries = [];
    for (var location in locations)
    {
        const sketchPlane = try(evOwnerSketchPlane(context, { "entity" : location }));
        if (sketchPlane == undefined)
            continue;

        const pointResult = evVertexPoint(context, { "vertex": location });
        var solidBodiesQuery is Query = qNothing();
        if (scopeSet)
        {
            solidBodiesQuery = holeDefinition.scope;
        }
        else
        {
            solidBodiesQuery = qBodyType(qEverything(EntityType.BODY), BodyType.SOLID);
            solidBodiesQuery = qSubtraction(solidBodiesQuery, hiddenBodies);
        }
        const faces = evaluateQuery(context, qContainsPoint(qGeometry(qOwnedByBody(solidBodiesQuery, EntityType.FACE), GeometryType.PLANE), pointResult));
        if (@size(faces) != 1 && !oppositeDirectionSet)
            continue;

        for (var face in faces)
        {
            const facePlane = try(evPlane(context, { "face" : face }));
            if (facePlane != undefined)
            {
                const needFlip = dot(sketchPlane.normal, facePlane.normal) < 0;
                if (!oppositeDirectionSet)
                {
                    oppositeDirectionChanged = (oppositeDirection != needFlip);
                    oppositeDirection = needFlip;
                    oppositeDirectionSet = true;
                }
                if (needFlip == oppositeDirection)
                {
                    // Only add non-ambiguous parts
                    const bodyQuery = evaluateQuery(context, qOwnerBody(face));
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

/**
*  Expects selected query to evaluate to a set of vertices. Throws if non-vertex is returned
*  Clusters coincident vertices created by the same operation, uses one representative for each cluster
*  union query of cluster representative queries is returned
*/
function clusterVertexQueries(context is Context, selected is Query) returns Query
{
    var perFeature = {};
    for (var tId in evaluateQuery(context, selected))
    {
        var operationId = lastModifyingOperationId(context, tId);
        if (perFeature[operationId] == undefined)
        {
            perFeature[operationId] = [];
        }
        perFeature[operationId] = append(perFeature[operationId], tId);
    }
    var clusterQueries = [];
    var didCluster = false;
    for ( var entry in perFeature)
    {
        var nPoints = size(entry.value);
        if (nPoints == 1)
        {
            clusterQueries = append(clusterQueries, entry.value[0]);
        }
        else
        {
            var points = makeArray(nPoints);
            for (var i = 0; i < nPoints; i = i + 1)
            {
                points[i] = evVertexPoint(context, {'vertex' : entry.value[i]});
            }
            var clusters = clusterPoints(points, TOLERANCE.zeroLength);
            if (size(clusters) != size(points))
            {
                didCluster = true;
            }
            for (var cluster in clusters)
            {
                clusterQueries = append(clusterQueries, entry.value[cluster[0]]);
            }
        }
    }
    if (didCluster)
    {
        return qUnion(clusterQueries);
    }
    else
    {
        return selected;
    }
}

