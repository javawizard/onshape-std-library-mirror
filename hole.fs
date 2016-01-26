FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

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
import(path : "onshape/std/holetables.gen.fs", version : "");
import(path : "onshape/std/lookupTablePath.fs", version : "");

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

const CSINK_ANGLE = 90 * degree;
const MAX_LOCATIONS = 100;

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
        annotation { "Name" : "Hole style", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        definition.style is HoleStyle;

        annotation { "Name" : "Hole termination", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
        definition.endStyle is HoleEndStyle;

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;

        if (definition.endStyle != HoleEndStyle.BLIND_IN_LAST && definition.standardThrough != undefined)
        {
            annotation { "Name" : "Standard", "Lookup table" : tappedOrClearanceHoleTable, "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            definition.standardTappedOrClearance is LookupTablePath;
        }
        else if (definition.endStyle == HoleEndStyle.BLIND_IN_LAST && definition.standardBlindInLast != undefined)
        {
            annotation { "Name" : "Standard", "Lookup table" : blindInLastHoleTable, "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            definition.standardBlindInLast is LookupTablePath;
        }

        annotation { "Name" : "Hole diameter", "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_EXPRESSION"] }
        isLength(definition.holeDiameter, HOLE_DIAMETER_BOUNDS);

        if (definition.style == HoleStyle.C_BORE)
        {
            annotation { "Name" : "Counterbore diameter", "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_EXPRESSION"] }
            isLength(definition.cBoreDiameter, HOLE_BORE_DIAMETER_BOUNDS);

            annotation { "Name" : "Counterbore depth", "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_EXPRESSION"] }
            isLength(definition.cBoreDepth, HOLE_BORE_DEPTH_BOUNDS);
        }
        else if (definition.style == HoleStyle.C_SINK)
        {
            annotation { "Name" : "Countersink diameter", "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_EXPRESSION"] }
            isLength(definition.cSinkDiameter, HOLE_BORE_DIAMETER_BOUNDS);

            annotation { "Name" : "Countersink angle", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            isAngle(definition.cSinkAngle, CSINK_ANGLE_BOUNDS);
        }

        if (definition.endStyle == HoleEndStyle.BLIND_IN_LAST)
        {
            if (definition.tapDrillDiameter != undefined)
            {
                annotation { "Name" : "Tap drill diameter", "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_EXPRESSION"] }
                isLength(definition.tapDrillDiameter, HOLE_DIAMETER_BOUNDS);
            }
        }
        if (definition.endStyle == HoleEndStyle.BLIND || definition.endStyle == HoleEndStyle.BLIND_IN_LAST)
        {
            annotation { "Name" : "Hole depth", "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            isLength(definition.holeDepth, HOLE_DEPTH_BOUNDS);
        }

        if (definition.endStyle != HoleEndStyle.THROUGH || definition.style != HoleStyle.SIMPLE)
        {
            annotation { "Name" : "Start from sketch plane", "Default" : false, "UIHint" : "REMEMBER_PREVIOUS_VALUE" }
            definition.startFromSketch is boolean;
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
            if (definition.holeDiameter > definition.cBoreDiameter + TOLERANCE.zeroLength * meter)
                throw regenError(ErrorStringEnum.HOLE_CBORE_TOO_SMALL, ["holeDiameter", "cBoreDiameter"]);

            if (definition.endStyle == HoleEndStyle.BLIND && definition.holeDepth < definition.cBoreDepth - TOLERANCE.zeroLength * meter)
                throw regenError(ErrorStringEnum.HOLE_CBORE_TOO_DEEP, ["holeDepth", "cBoreDepth"]);
        }

        if (definition.style == HoleStyle.C_SINK && isAtVersionOrLater(context, FeatureScriptVersionNumber.V206_LINEAR_RANGE))
        {
            if (definition.holeDiameter > definition.cSinkDiameter + TOLERANCE.zeroLength * meter)
                throw regenError(ErrorStringEnum.HOLE_CSINK_TOO_SMALL, ["holeDiameter", "cSinkDiameter"]);

            // tipDepth is a local used for error checking
            var tipDepth = definition.holeDepth;
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V252_HOLE_FEATURE_FIX_ERROR_CHECK) && !definition.useTipDepth)
            {
                // Account for measuring hole depth to the shoulder of the drill
                tipDepth = tipDepth + (definition.holeDiameter / 2) / tan(definition.tipAngle / 2);
            }
            const cSinkDepth = (definition.cSinkDiameter / 2) / tan(definition.cSinkAngle / 2);
            if (definition.endStyle == HoleEndStyle.BLIND && tipDepth < cSinkDepth - TOLERANCE.zeroLength * meter)
                throw regenError(ErrorStringEnum.HOLE_CSINK_TOO_DEEP, ["holeDepth", "cSinkDepth"]);
        }

        if (definition.tapDrillDiameter == undefined)
        {
            definition.tapDrillDiameter = definition.holeDiameter;
        }

        if (definition.endStyle == HoleEndStyle.BLIND_IN_LAST)
        {
            if (definition.tapDrillDiameter > definition.holeDiameter + TOLERANCE.zeroLength * meter)
            {
                throw regenError(ErrorStringEnum.HOLE_TAP_DIA_TOO_LARGE, ["holeDiameter", "tapDrillDiameter"]);
            }
        }

        const locations = reduceLocations(context, definition.locations);
        if (size(locations) == 0)
        {
            throw regenError(ErrorStringEnum.HOLE_NO_POINTS, ["locations"]);
        }
        else
        {
            if (size(locations) > MAX_LOCATIONS && isAtVersionOrLater(context, FeatureScriptVersionNumber.V274_HOLE_LIMIT_NUM_LOCATIONS_100))
            {
                throw regenError(ErrorStringEnum.HOLE_EXCEEDS_MAX_LOCATIONS, ["locations"]);
            }

            definition.scopeSize = 0.1 * meter;
            const scopeTest = evaluateQuery(context, definition.scope);
            if (size(scopeTest) == 0)
            {
                reportFeatureError(context, id, ErrorStringEnum.HOLE_EMPTY_SCOPE, ["scope"]);
            }
            else
            {
                definition.scopeSize = try(scopeSize(context, definition));
                if (definition.scopeSize == undefined)
                {
                    reportFeatureError(context, id, ErrorStringEnum.HOLE_FAIL_BBOX, ["scope"]);
                }
            }
        }

        const startingBodyCount = size(evaluateQuery(context, qEverything(EntityType.BODY)));

        // If we have no errors, test for sucess and disjoint and report those errors
        if (!hasErrors(context, id))
        {
            // ------------- Perform the operation ---------------
            var numSuccess = holeOp(context, id, locations, definition);

            if (numSuccess == 0)
                reportFeatureError(context, id, ErrorStringEnum.HOLE_NO_HITS, ["scope"]);
            else
            {
                const finalBodyCount = size(evaluateQuery(context, qEverything(EntityType.BODY)));
                if (finalBodyCount > startingBodyCount)
                    reportFeatureError(context, id, ErrorStringEnum.HOLE_DISJOINT, ["scope"]);
            }
        }
        // Test for errors again, now with success and disjoint check
        if (hasErrors(context, id))
        {
            const errorId = id + "errorEntities";
            definition.generateErrorBodies = true;
            holeOp(context, errorId, locations, definition);
            var errorBodyQuery = qCreatedBy(errorId, EntityType.BODY);
            setErrorEntities(context, id, { "entities" : errorBodyQuery });
            opDeleteBodies(context, id + "delete", { "entities" : errorBodyQuery });
        }

    }, { endStyle : HoleEndStyle.BLIND, style : HoleStyle.SIMPLE, oppositeDirection : false,
        tipAngle : 118 * degree, useTipDepth : false,
        cSinkUseDepth : false, cSinkDepth : 0 * meter, cSinkAngle : 90 * degree,
        generateErrorBodies : false, startFromSketch : false});

function hasErrors(context is Context, id is Id) returns boolean
{
    return getFeatureError(context, id) != undefined || getFeatureWarning(context, id) != undefined || getFeatureInfo(context, id) != undefined;
}

function holeOp(context is Context, id is Id, locations is array, definition is map)
{

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

            var startDistances = { frontDist : 0 * meter, backDist : 0 * meter };
            var calcuateStartPoint = !definition.startFromSketch && !(definition.endStyle == HoleEndStyle.THROUGH && definition.style == HoleStyle.SIMPLE);
            if (!definition.generateErrorBodies && calcuateStartPoint)
            {
                startDistances = cylinderCastBiDir(context, holeId, {
                    "scopeSize" : definition.scopeSize,
                    "cSys" : startPointCSys,
                    "diameter": maxDiameter,
                    "scope" : definition.scope,
                    "needBack": false });
            }
            if (cutHole(context, holeId, definition, startDistances, startPointCSys))
            {
                numSuccess += 1;
            }
        }
    }
    return numSuccess;
}

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
                if (tolerantEquals(ptResult, testPt))
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
    (meter)      : [1e-5, 0.005, 500],
    (centimeter) : 0.5,
    (millimeter) : 5.0,
    (inch)       : 0.25,
    (foot)       : 0.02,
    (yard)       : 0.007
} as LengthBoundSpec;

const HOLE_BORE_DIAMETER_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [1e-5, 0.01, 500],
    (centimeter) : 1.0,
    (millimeter) : 10.0,
    (inch)       : 0.5,
    (foot)       : 0.04,
    (yard)       : 0.014
} as LengthBoundSpec;

const HOLE_DEPTH_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [1e-5, 0.012, 500],
    (centimeter) : 1.2,
    (millimeter) : 12.0,
    (inch)       : 0.5,
    (foot)       : 0.04,
    (yard)       : 0.014
} as LengthBoundSpec;

const HOLE_BORE_DEPTH_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0, 0.005, 500],
    (centimeter) : 0.5,
    (millimeter) : 5.0,
    (inch)       : 0.25,
    (foot)       : 0.02,
    (yard)       : 0.007
} as LengthBoundSpec;

export const CSINK_ANGLE_BOUNDS =
{
    "min"    : (0.25 * PI - TOLERANCE.zeroAngle) * radian,
    "max"    : (0.75 * PI + TOLERANCE.zeroAngle) * radian,
    (degree) : [44.9, 90, 135.1],
    (radian) : [0.25 * PI, 0.5 * PI,  0.75 * PI]
} as AngleBoundSpec;

function depthOfRadius(context is Context, radius is ValueWithUnits, halfAngle is ValueWithUnits)
precondition
{
    isLength(radius);
    isAngle(halfAngle);
}
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V190_HOLE_FEATURE_ANGLE_MATH))
    {
        return radius / tan(halfAngle);
    }
    else
    {
        return cos(halfAngle) * radius;
    }
}

function radiusOfDepth(context is Context, depth is ValueWithUnits, halfAngle is ValueWithUnits)
precondition
{
    isLength(depth);
    isAngle(halfAngle);
}
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V190_HOLE_FEATURE_ANGLE_MATH))
    {
        return depth * tan(halfAngle);
    }
    else
    {
        return sin(halfAngle) * depth;
    }
}

/**
 * returns { dist : Length - z distance to first point of contact in cSys frame }
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
    var foundTarget = undefined;
    const AOL_HOLE_FEATURE_HANDLE_FAILED_EXTRUDE = isAtVersionOrLater(context, FeatureScriptVersionNumber.V208_HOLE_FEATURE_HANDLE_FAILED_EXTRUDE);
    const AOL_HOLE_FEATURE_FIX_BROKEN_DEPTH_CALC = isAtVersionOrLater(context, FeatureScriptVersionNumber.V225_HOLE_FEATURE_FIX_BROKEN_DEPTH_CALC);
    // Fix for 208 was too broad, don't fix it here, fix later
    if (AOL_HOLE_FEATURE_HANDLE_FAILED_EXTRUDE && !AOL_HOLE_FEATURE_FIX_BROKEN_DEPTH_CALC)
    {
        bestDist = 0 * meter;
    }
    const targets = evaluateQuery(context, arg.scope);
    if (size(targets) == 0)
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V286_HOLE_ADD_STANDARDS))
            return { "dist" : 0 * meter };
        else
            throw "No targets";
    }

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
            const collisions = evCollision(context, {'targets' : targetQuery, 'tools' : bodyQuery});
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
            foundTarget = targetQuery;
        }
        else if (isMinimumCase)
        {
            if (d < bestDist)
            {
                bestDist = d;
                foundTarget = targetQuery;
            }
        }
        else
        {
            if (d > bestDist) {
                foundTarget = targetQuery;
                bestDist = d;
            }
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

    return { "dist" : bestDist, "target" : foundTarget };
}

function cylinderCastBiDir(context is Context, id is Id, arg is map) returns map
precondition
{
    arg.cSys is CoordSystem;
    isLength(arg.diameter);
    arg.scope is Query;
    arg.needFront is boolean || arg.needFront is undefined; // default true
    arg.needBack is boolean || arg.needBack is undefined; // default true
}
{
    // Small but finite distance.
    // A large distance does not work when trying to drill out from the inside of a box.
    // A zero distance produces no surface for the cast if the the target is planar face parallel to the sketch
    const smallFrontOffset = 0.01 * millimeter;

    var resultFront = {"dist" : 0 * meter};
    if (arg.needFront != false)
    {
        try {
            resultFront = cylinderCast(context, id, {
                "scopeSize" : smallFrontOffset,
                "cSys" : arg.cSys,
                "isFront" : true,
                "findClosest" : true,
                "diameter" : arg.diameter,
                "scope" : arg.scope });
        }
    }

    // The back of the part uses a large distance until we can find a better method.
    var resultBack = {"dist" : 0 * meter};
    if (arg.needBack != false)
    {
        try {
            resultBack = cylinderCast(context, id, {
                "scopeSize" : arg.scopeSize,
                "cSys" : arg.cSys,
                "isFront" : false,
                "findClosest" : true,
                "diameter" : arg.diameter,
                "scope" : arg.scope });
        }
    }

    var result = {};

    if (resultFront.dist != undefined)
        result.frontDist = resultFront.dist;
    else
        result.frontDist = 0 * meter;

    if (resultBack.dist != undefined)
        result.backDist = resultBack.dist;
    else
        result.backDist = 0 * meter;

    return result;
}

function cutHole(context is Context, id is Id, holeDefinition is map, startDistances is map, cSys is CoordSystem) returns boolean
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
    var blindBody = sketchToolCore(context, id, {
        "prefix" : "core",
        "sketch" : sketch,
        "cSys" : cSys,
        "startDepth" : startDepth,
        "clearanceDepth" : frontDist,
        "holeDefinition" : holeDefinition }).lastBody;

    skSolve(sketch);

    var offsetTappedHole = holeDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST &&
    blindBody != undefined &&
    holeDefinition.tapDrillDiameter < (holeDefinition.holeDiameter - TOLERANCE.zeroLength * meter);
    const axisQuery = sketchEntityQuery(id + ("sketch" ~ ".wireOp"), EntityType.EDGE, "core_line_0");
    const sketchQuery = qSketchRegion(id + sketchName, false);
    spinCut(context, id, sketchQuery, axisQuery, holeDefinition.scope, holeDefinition.generateErrorBodies);
    opDeleteBodies(context, id + "delete_sketch", { "entities" : qCreatedBy(id + sketchName, EntityType.BODY) });

    const newFaces = evaluateQuery(context, qCreatedBy(id, EntityType.FACE));
    var success = true;
    if (size(newFaces) == 0)
        success = false;
    if (offsetTappedHole && blindBody != undefined)
    {
        // Find the cylindrical face drilled by this feature in the body with the blind hole.
        var targetFace = qGeometry(qOwnedByBody(qCreatedBy(id, EntityType.FACE), blindBody), GeometryType.CYLINDER);
        opOffsetFace(context, id + "offset", {
            "moveFaces" : targetFace,
            "offsetDistance" : (holeDefinition.holeDiameter - holeDefinition.tapDrillDiameter) / 2
        });
    }
    return success;
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
        cSinkRadius = radiusOfDepth(context, cSinkDepth, arg.cSinkAngle / 2);
    }
    else
    {
        cSinkRadius = arg.cSinkDiameter / 2;
        cSinkDepth = depthOfRadius(context, cSinkRadius, arg.cSinkAngle / 2);
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
    var lastBody;
    const scopeSizeGrowFactor = arg.holeDefinition.generateErrorBodies ? 1 : 2;
    const radius = arg.holeDefinition.holeDiameter * 0.5;
    const tipAngle = arg.holeDefinition.tipAngle;
    var depth = arg.holeDefinition.holeDepth;
    var useTipDepth = arg.holeDefinition.useTipDepth;
    const tipDepth = depthOfRadius(context, radius, tipAngle / 2.0);
    if (arg.holeDefinition.endStyle == HoleEndStyle.THROUGH)
    {
        depth = arg.holeDefinition.scopeSize * scopeSizeGrowFactor;
    }
    else if (arg.holeDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST)
    {
        // Find shoulder depth
        try {
            const result = cylinderCast(context, id + "limit_surf_cast", {
                "scopeSize" : arg.holeDefinition.scopeSize * 2,
                "cSys" : arg.cSys,
                "isFront" : true,
                "findClosest" : false,
                "diameter" : 2 * radius,
                "scope" : arg.holeDefinition.scope });
            if (result.dist != undefined)
            {
                var startDist = result.dist;
                if (startDist < 0 && isAtVersionOrLater(context, FeatureScriptVersionNumber.V291_HOLE_FEATURE_STANDARD_DEFAULTS))
                {
                    startDist = 0 * meter;
                }
                depth += startDist - arg.startDepth - arg.clearanceDepth;
                if (arg.holeDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST &&
                    arg.holeDefinition.holeDiameter > arg.holeDefinition.tapDrillDiameter + TOLERANCE.zeroLength * meter)
                {
                    var delta = (arg.holeDefinition.holeDiameter - arg.holeDefinition.tapDrillDiameter) / 2 / tan(tipAngle / 2);
                    depth -= delta;
                }
                lastBody = result.target;
            }
        }
        if (useTipDepth)
        {
            depth += tipDepth;
        }
    }

    var points;
    if (useTipDepth)
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
    return {"lastBody" : lastBody};
}

function scopeSize(context is Context, definition is map) returns map
{
    const scopeBox = evBox3d(context, { "topology" : qUnion([definition.scope, definition.locations])});
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
    if (oldDefinition.standardTappedOrClearance != definition.standardTappedOrClearance ||
        oldDefinition.standardBlindInLast != definition.standardBlindInLast ||
        oldDefinition.endStyle != definition.endStyle)
    {
        definition = updateHoleDefinitionWithStandard(oldDefinition, definition);
    }
    definition = setToCustomIfStandardViolated(definition);

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


function getStandardTable(definition is map) returns map
{
    var standard;
    var table;
    if (definition.endStyle == HoleEndStyle.BLIND_IN_LAST)
    {
        standard = definition.standardBlindInLast;
        table = blindInLastHoleTable;
    }
    else
    {
        standard = definition.standardTappedOrClearance;
        table = tappedOrClearanceHoleTable;
    }

    table = getLookupTable(table, standard);
    if (table == undefined)
    {
        return {};
    }
    return table;
}

/**
 * implements standard hole sizes. Set the appropriate standard string for the hole type
 * in the same format as the UI specification and it will set the appropriate values,
 * and then return an updated definition
 * @param definition {{
 *      @field TODO
 * }}
 */
export function updateHoleDefinitionWithStandard(oldDefinition is map, definition is map) returns map
{
    definition = syncStandards(oldDefinition, definition);
    var table = getStandardTable(definition);
    for (var entry in table)
    {
        definition[entry.key] = lookupTableFixExpression(entry.value);
    }

    if (lookupTableGetValue(definition.tapDrillDiameter) > lookupTableGetValue(definition.holeDiameter))
    {
      definition.tapDrillDiameter = definition.holeDiameter;
    }
    if (lookupTableGetValue(definition.cBoreDiameter) < lookupTableGetValue(definition.holeDiameter))
    {
      definition.cBoreDiameter = definition.holeDiameter;
    }
    if (lookupTableGetValue(definition.cSinkDiameter) < lookupTableGetValue(definition.holeDiameter))
    {
      definition.cSinkDiameter = definition.holeDiameter;
    }

    return definition;
}

function syncStandards(oldDefinition is map, definition is map) returns map
{
    if (oldDefinition.standardTappedOrClearance != definition.standardTappedOrClearance)
    {
        definition.standardBlindInLast = definition.standardTappedOrClearance;
    }
    else if (oldDefinition.standardBlindInLast != definition.standardBlindInLast)
    {
        definition.standardTappedOrClearance = definition.standardBlindInLast;
    }
    return definition;
}

function setToCustomIfStandardViolated(definition is map) returns map
{
    var table = getStandardTable(definition);
    if (isLookupTableViolated(definition, table))
    {
        definition.standardTappedOrClearance = lookupTablePath({"standard": "Custom"});
        definition.standardBlindInLast = lookupTablePath({"standard": "Custom"});
    }

    return definition;
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

        const pointResult = evVertexPoint(context, { "vertex" : location });
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

