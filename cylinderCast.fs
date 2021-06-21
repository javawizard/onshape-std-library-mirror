FeatureScript 1540; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/*
 * Utility functions used by hole feature
 * Finds the projection of a cylinder against a part.
 */

import(path : "onshape/std/boolean.fs", version : "1540.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "1540.0");
import(path : "onshape/std/box.fs", version : "1540.0");
import(path : "onshape/std/clashtype.gen.fs", version : "1540.0");
import(path : "onshape/std/containers.fs", version : "1540.0");
import(path : "onshape/std/coordSystem.fs", version : "1540.0");
import(path : "onshape/std/evaluate.fs", version : "1540.0");
import(path : "onshape/std/extrude.fs", version : "1540.0");
import(path : "onshape/std/feature.fs", version : "1540.0");
import(path : "onshape/std/mathUtils.fs", version : "1540.0");
import(path : "onshape/std/sketch.fs", version : "1540.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1540.0");
import(path : "onshape/std/tool.fs", version : "1540.0");
import(path : "onshape/std/string.fs", version : "1540.0");
import(path : "onshape/std/units.fs", version : "1540.0");

/**
 * @internal
 */
export function cylinderCast(context is Context, idIn is Id, arg is map)
precondition
{
    isLength(arg.distance);
    isLength(arg.diameter);
    arg.cSys is CoordSystem;
    arg.isFront is boolean;
    arg.findClosest is boolean;
    arg.scope is Query;
}
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V362_HOLE_IMPROVED_DEPTH_FINDING))
    {
        return cylinderCast_rev_2(context, idIn, arg);
    }
    else if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V299_HOLE_FEATURE_FIX_BLIND_IN_LAST_FLIP))
    {
        return cylinderCast_rev_1(context, idIn, arg);
    }
    else
    {
        return cylinderCast_rev_0(context, idIn, arg);
    }
}

/*
 * Deprecated, too many repairs, starting new function
 */
function cylinderCast_rev_0(context is Context, idIn is Id, arg is map) returns map
{
    const shotName = arg.isFront ? "shot_front" : "shot_back";
    const id = idIn + shotName;
    const distance = arg.distance;

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
            return { "distance" : 0 * meter };
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

    return { "distance" : bestDist, "target" : foundTarget };
}

/*
 * Deprecated, new version returns all results in an array
 */
function cylinderCast_rev_1(context is Context, idIn is Id, arg is map) returns map
{
    const targets = evaluateQuery(context, arg.scope);
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V314_HOLE_FIX_CYL_CAST_EARLY_RETURN) && size(targets) == 0)
    {
        return { "distance" : 0 * meter };
    }

    const shotName = arg.isFront ? "shot_front" : "shot_back";
    const id = idIn + shotName;
    var direction = arg.cSys.zAxis;

    if (!arg.isFront)
    {
        direction = -direction;
    }
    const cSys = coordSystem(arg.cSys.origin - arg.distance * direction, arg.cSys.xAxis, direction);

    // Small but finite distance.
    // A large distance does not work when trying to drill out from the inside of a box.
    // A zero distance produces no surface for the cast if the the target is a planar face parallel to the sketch
    const smallOffset = 0.01 * millimeter;
    const sketchPlane = plane(cSys.origin - smallOffset * direction, direction); // plane is rotated

    var bestDist = undefined;
    var foundTarget = undefined;

    //------------- Create profile ----------------
    startFeature(context, id, {});
    const sketchName = "raySketch";
    const sketch = newSketchOnPlane(context, id + sketchName, { "sketchPlane" : sketchPlane });

    skCircle(sketch, "circle", { "center" : vector(0, 0) * meter, "radius" : arg.diameter / 2 });
    skSolve(sketch);

    // Legacy code, left for older versions. Never hit in later versions because return call above
    if (size(targets) == 0)
    {
        return { "distance" : 0 * meter };
    }

    const sketchEntityQuery = qCreatedBy(id + (sketchName ~ ".wireOp"), EntityType.EDGE);

    var shotNum = 0;
    for (var targetQuery in targets)
    {
        const extrudeDefinition = {
                                      "bodyType" : ToolBodyType.SURFACE,
                                      "operationType" : NewBodyOperationType.NEW,
                                      "surfaceEntities" : sketchEntityQuery,
                                      "endBound" : BoundingType.UP_TO_BODY,
                                      "endBoundEntityBody" : targetQuery
        };
        const extrudeId = id + ("extrude_" ~ shotNum);
        shotNum += 1;
        var d;
        var hasCollision is boolean = false;
        try
        {
            extrude(context, extrudeId, extrudeDefinition);
            const bodyQuery = qCreatedBy(extrudeId, EntityType.BODY);
            const cylBox = evBox3d(context, { "topology" : bodyQuery, "cSys" : cSys } );
            d = cylBox.maxCorner[2];
            const collisions = evCollision(context, {'targets' : targetQuery, 'tools' : bodyQuery});
            for (var collision in collisions) {
                if (collision['type'] == ClashType.INTERFERE) {
                    // If the shot cylinder interferes with the target, then we began the shot on the interior,
                    // fall back to starting from the sketch plane.
                    hasCollision = true;
                }
            }
        }

        if (hasCollision)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V306_HOLE_FEATURE_FIX_UNDETERMINED_TARGET_BODY))
            {
                // Fix, targets with collisions indicate the hole started inside the body and should be ignored
                continue;
            }
            else
            {
                bestDist = 0 * meter;
                foundTarget = targetQuery;
                break;
            }
        }
        if ((d != undefined) && ((bestDist == undefined) || (arg.findClosest ? d < bestDist : d > bestDist)))
        {
            bestDist = d;
            foundTarget = targetQuery;
        }
    }

    abortFeature(context, id);

    if (bestDist == undefined || bestDist < 0) {
        bestDist = 0 * meter;
    }

    if (!arg.isFront)
    {
        const p = cSys.origin + bestDist * cSys.zAxis;
        bestDist = (p - arg.cSys.origin).dot(arg.cSys.zAxis);
    }
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V306_HOLE_FEATURE_FIX_UNDETERMINED_TARGET_BODY) && foundTarget == undefined)
    {
        if (size(targets) == 1)
        {
            foundTarget = targets[0];
        }
        else
        {
            throw regenError(ErrorStringEnum.HOLE_CANNOT_DETERMINE_LAST_BODY);
        }
    }
    return { "distance" : bestDist, "target" : foundTarget };
}

function cylinderCast_rev_2(context is Context, idIn is Id, arg is map) returns array
{
    const targets = evaluateQuery(context, arg.scope);
    const numberOfTargets = size(targets);

    if (numberOfTargets == 0)
    {
        return [];
    }

    const shotName = arg.isFront ? "shot_front" : "shot_back";
    const id = idIn + shotName;
    var direction = arg.cSys.zAxis;

    if (!arg.isFront)
    {
        direction = -direction;
    }
    const cSys = coordSystem(arg.cSys.origin - arg.distance * direction, arg.cSys.xAxis, direction);

    // Small but finite distance.
    // A large distance does not work when trying to drill out from the inside of a box.
    // A zero distance produces no surface for the cast if the the target is a planar face parallel to the sketch
    const smallOffset = 0.01 * millimeter;
    const sketchPlane = plane(cSys.origin - smallOffset * direction, direction); // plane is rotated

    startFeature(context, id, {});
    const sketchName = "raySketch";
    const sketch = newSketchOnPlane(context, id + sketchName, { "sketchPlane" : sketchPlane });

    var doRecalculateFirstBodyDistance is boolean = false;

    if (numberOfTargets == 1 && arg.firstBodyCastDiameter != undefined)
    {
        // only 1 body, use the specified first body cast diameter for the one and only cast entity needed
        skCircle(sketch, "circle1", { "center" : vector(0, 0) * meter, "radius" : arg.firstBodyCastDiameter / 2 });
    }
    else
    {
        // the first and maybe only cast entity
        skCircle(sketch, "circle1", { "center" : vector(0, 0) * meter, "radius" : arg.diameter / 2 });
    }

    if (numberOfTargets > 1 && arg.firstBodyCastDiameter != undefined)
    {
        // will need a second cast to recalculate the first body starting distance, sketch the circle that will be used for that
        skCircle(sketch, "circle2", { "center" : vector(0, 0) * meter, "radius" : arg.firstBodyCastDiameter / 2 });
        doRecalculateFirstBodyDistance = true;
    }

    skSolve(sketch);

    var query = sketchEntityQuery(id + (sketchName ~ ".wireOp"), EntityType.EDGE, "circle1");

    var result = getBodyHitDistances(context, id, cSys, query, direction, targets);

    // did we need a second cast to recalculate the first body starting distance?
    if (doRecalculateFirstBodyDistance && size(result) > 0)
    {
        query = sketchEntityQuery(id + (sketchName ~ ".wireOp"), EntityType.EDGE, "circle2");

        var result2 = getBodyHitDistances(context, id + "2", cSys, query, direction, [result[0].target]);

        // got it?
        if (size(result2) > 0 && result[0].target == result2[0].target)
        {
            // update the new hit point distance on the first body
            result[0].distance = result2[0].distance;
        }
    }

    abortFeature(context, id);

    return result;
}

/**
 * @internal
 * Extrudes the specified entities query up to all the specified body targets and returns the distance from the
 * specified coordinate system to the farthest hit point of each body. It is assumed a start feature operation
 * has already been started.

 * @param context {Context} : The target context.
 * @param id {Id}: identifier of the feature
 * @param cSys {Coord} : the coordinate system used to define the bounding box of the extrude to determine the hit
 *  point distances
 * @param entitiesToExtrude {Query} : the entities query used for the extrude
 * @param extrudeDirection {Vector} : the direction of the extrude
 * @param targets {array} : array of bodies to determine the hit point distances for
 * @returns {array} : array of maps containing the body target ("target") and the farthest hit point distance to that
 *  target ("distance"). The array will be returned sorted from closest target body to farthest target body.
*/
function getBodyHitDistances(context is Context, id is Id, cSys is CoordSystem, entitiesToExtrude is Query, extrudeDirection is Vector, targets is array) returns array
{
    var shotNum = 0;
    var result = [];
    for (var targetQuery in targets)
    {
        const extrudeId = id + ("extrude_" ~ shotNum);
        shotNum += 1;
        var d;
        var hasCollision is boolean = false;
        try silent
        {
            var extrudeDefinition;
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V364_HOLE_FIX_FEATURE_MIRROR))
            {
                // New version using opextrude so as to use correct normal vector
                extrudeDefinition = {
                    "entities" : entitiesToExtrude,
                    "direction" : extrudeDirection,
                    "endBound" : BoundingType.UP_TO_BODY,
                    "endBoundEntity" : targetQuery
                };
              if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V429_HOLE_SAFE_SKETCH_CLEANUP))
                  try silent(opExtrude(context, extrudeId, extrudeDefinition));
              else
                  opExtrude(context, extrudeId, extrudeDefinition);
            }
            else
            {
                // Old version calling feature extrude.
                extrudeDefinition = {
                    "bodyType" : ToolBodyType.SURFACE,
                    "operationType" : NewBodyOperationType.NEW,
                    "surfaceEntities" : entitiesToExtrude,
                    "endBound" : BoundingType.UP_TO_BODY,
                    "endBoundEntityBody" : targetQuery
                };
                extrude(context, extrudeId, extrudeDefinition);
            }
            const bodyQuery = qCreatedBy(extrudeId, EntityType.BODY);
            const cylBox = evBox3d(context, { "topology" : bodyQuery, "cSys" : cSys } );
            d = cylBox.maxCorner[2];
            const collisions = evCollision(context, {'targets' : targetQuery, 'tools' : bodyQuery});
            for (var collision in collisions) {
                if (collision['type'] == ClashType.INTERFERE) {
                    // If the shot cylinder interferes with the target, then we began the shot on the interior,
                    // fall back to starting from the sketch plane.
                    hasCollision = true;
                }
            }
        }

        if (hasCollision)
        {
            // Fix, targets with collisions indicate the hole started inside the body and should be ignored
            continue;
        }
        if (d != undefined)
        {
            result = append(result, { "distance" : d, "target" : targetQuery });
        }
    }

    if (size(result) > 1) {
        result = sort(result, function(a, b) {
            return a.distance - b.distance;
        });
    }

    return result;
}

/**
 * @internal
 * Returns the result of casting a cylinder from front and or back of the scope.
 */
export function cylinderCastBiDirectional(context is Context, id is Id, arg is map) returns map
precondition
{
    arg.cSys is CoordSystem;
    isLength(arg.diameter);
    arg.scope is Query;
    arg.needFront is boolean || arg.needFront is undefined; // default true
    arg.needBack is boolean || arg.needBack is undefined; // default true
}
{
    const smallFrontOffset = isAtVersionOrLater(context, FeatureScriptVersionNumber.V299_HOLE_FEATURE_FIX_BLIND_IN_LAST_FLIP) ? 0 * meter : 0.01 * millimeter;

    var resultFront = [];
    if (arg.needFront != false)
    {
        try {
            resultFront = cylinderCast(context, id, {
                "distance" : smallFrontOffset,
                "cSys" : arg.cSys,
                "isFront" : true,
                "findClosest" : true,
                "diameter" : arg.diameter,
                "firstBodyCastDiameter" : arg.firstBodyCastDiameter,
                "scope" : arg.scope });
        }
    }

    // The back of the part uses a large distance until we can find a better method.
    var resultBack = [];
    if (arg.needBack != false)
    {
        try {
            resultBack = cylinderCast(context, id, {
                "distance" : arg.scopeSize,
                "cSys" : arg.cSys,
                "isFront" : false,
                "findClosest" : true,
                "diameter" : arg.diameter,
                "scope" : arg.scope });
        }
    }

    var result = {};
    result.resultFront = constructResult(resultFront);
    result.resultBack = constructResult(resultBack);
    return result;
}

function constructResult(data) returns array
{
    if (data is array)
    {
        return data;
    }
    else
    {
        if (data.distance == undefined)
            data.distance = 0 * meter;
        return [data];
    }
}

