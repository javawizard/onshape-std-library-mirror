FeatureScript 1301; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "1301.0");
import(path : "onshape/std/boolean.fs", version : "1301.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "1301.0");
import(path : "onshape/std/box.fs", version : "1301.0");
import(path : "onshape/std/clashtype.gen.fs", version : "1301.0");
import(path : "onshape/std/containers.fs", version : "1301.0");
import(path : "onshape/std/coordSystem.fs", version : "1301.0");
import(path : "onshape/std/curveGeometry.fs", version : "1301.0");
import(path : "onshape/std/cylinderCast.fs", version : "1301.0");
import(path : "onshape/std/evaluate.fs", version : "1301.0");
import(path : "onshape/std/feature.fs", version : "1301.0");
import(path : "onshape/std/holetables.gen.fs", version : "1301.0");
import(path : "onshape/std/lookupTablePath.fs", version : "1301.0");
import(path : "onshape/std/mathUtils.fs", version : "1301.0");
import(path : "onshape/std/revolve.fs", version : "1301.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "1301.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "1301.0");
import(path : "onshape/std/sketch.fs", version : "1301.0");
import(path : "onshape/std/string.fs", version : "1301.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "1301.0");
import(path : "onshape/std/tool.fs", version : "1301.0");
import(path : "onshape/std/valueBounds.fs", version : "1301.0");

export import(path : "onshape/std/holeAttribute.fs", version : "1301.0");
export import(path : "onshape/std/holesectionfacetype.gen.fs", version : "1301.0");
export import(path : "onshape/std/holeUtils.fs", version : "1301.0");

/**
 * Defines the end bound for the hole cut.
 * @value THROUGH : Cut holes with a through-all extrude.
 * @value BLIND : Cut holes to a specific depth.
 * @value BLIND_IN_LAST : Cut holes through all parts but the last, then cut
 *          to a specific depth in the last part.
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

const MAX_LOCATIONS = 100;
const HOLE_FEATURE_COUNT_VARIABLE_NAME = "-holeFeatureCount"; // Not a valid identifier, so it is not offered in autocomplete

/*
 * IB: the call structure of the principal functions in this file is something like this:
 *
 * // hole does the definition checks
 * hole           --> holeOp
 *                --> reduceLocations
 *
 * // holeOp makes all the holes and booleans them with the merge scope
 * holeOp         --> getCutOption                                 // getCutOption checks whether to boolean one hole at a time or all at once at the end
 *                --> holeAtLocation
 *                --> createAttributesFromTracking, assignSheetMetalHoleAttributes
 *
 * // holeAtLocation creates hole body at location (and cuts if one-cut-at-a-time is on)
 * holeAtLocation --> computeCSys                                  // computeCSys figures out the coordinate system for the hole
 *                --> calculateStartPoint
 *                --> cylinderCastBiDirectional
 *                --> cutHole
 *
 * cutHole        --> getSheetMetalModels
 *                --> sketchCBore, sketchCSink, sketchToolCore
 *                --> startSketchTracking
 *                --> spinCut                                     // opRevolve, if individual cuts, opBoolean
 *                --> createAttributesFromTracking, createSheetMetalHoleAttributes
 *
 * createAttributesFromTracking --> createHoleAttribute
 *
 * createSheetMetalHoleAttributes --> assignSheetMetalHoleAttributes --> createAttributesForSheetMetalHole --> createHoleAttribute
 *
 * createHoleAttribute --> makeHoleAttribute
 *                     --> addCommonAttributeProperties
 *                     --> addSectionSpecsToAttribute
 *
 * sketchToolCore --> cylinderCast // For "shoulder depth" for blind in last holes
 */

/**
 * Creates holes of specific dimensions and style, based either on standard
 * hole size, or by user-defined values. Each hole's position and orientation
 * are specified using sketch points.
 */
annotation { "Feature Type Name" : "Hole", "Editing Logic Function" : "holeEditLogic" }
export const hole = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Style", "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "INITIAL_FOCUS"] }
        definition.style is HoleStyle;

        annotation { "Name" : "Termination", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        definition.endStyle is HoleEndStyle;

        annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.oppositeDirection is boolean;

        if (definition.endStyle != HoleEndStyle.BLIND_IN_LAST && definition.standardThrough != undefined)
        {
            annotation { "Name" : "Standard", "Lookup Table" : tappedOrClearanceHoleTable, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "UNCONFIGURABLE"] }
            definition.standardTappedOrClearance is LookupTablePath;
        }
        else if (definition.endStyle == HoleEndStyle.BLIND_IN_LAST && definition.standardBlindInLast != undefined)
        {
            annotation { "Name" : "Standard", "Lookup Table" : blindInLastHoleTable, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "UNCONFIGURABLE"] }
            definition.standardBlindInLast is LookupTablePath;
        }

        annotation { "Name" : "Diameter", "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_EXPRESSION"] }
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

            annotation { "Name" : "Countersink angle", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
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

        if (definition.majorDiameter != undefined)
        {
            annotation { "Name" : "Tap major diameter", "UIHint" : ["ALWAYS_HIDDEN"] }
            isLength(definition.majorDiameter, HOLE_MAJOR_DIAMETER_BOUNDS);
        }

        /*
         * showTappedDepth, tappedDepth and tapClearance are for hole annotations;
         * they currently have no effect on geometry regeneration, but is stored in HoleAttribute. If we modeled the hole's
         * threads, then they would have an effect.
         */
        annotation { "Name" : "Tapped details", "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.showTappedDepth is boolean;

        if (definition.endStyle != HoleEndStyle.THROUGH)
        {
            annotation { "Name" : "Depth", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            isLength(definition.holeDepth, HOLE_DEPTH_BOUNDS);
        }
        if (definition.showTappedDepth)
        {
            if (definition.endStyle == HoleEndStyle.THROUGH)
            {
                annotation { "Name" : "Tap through all", "Default" : true, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                definition.isTappedThrough is boolean;
            }

            if (definition.endStyle != HoleEndStyle.THROUGH || !definition.isTappedThrough)
            {
                annotation { "Name" : "Tapped depth", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition.tappedDepth, HOLE_DEPTH_BOUNDS);
            }

            if (definition.endStyle != HoleEndStyle.THROUGH)
            {
                annotation { "Name" : "Tap clearance (number of thread pitch lengths)", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isReal(definition.tapClearance, HOLE_CLEARANCE_BOUNDS);
            }
        }

        if (definition.endStyle == HoleEndStyle.BLIND || (definition.endStyle == HoleEndStyle.THROUGH && definition.style != HoleStyle.SIMPLE))
        {
            annotation { "Name" : "Start from sketch plane", "Default" : true }
            definition.startFromSketch is boolean;
        }

        annotation { "Name" : "Sketch points to place holes",
                    "Filter" : EntityType.VERTEX && SketchObject.YES && ModifiableEntityOnly.YES || BodyType.MATE_CONNECTOR }
        definition.locations is Query;

        annotation { "Name" : "Merge scope",
                    "Filter" : (EntityType.BODY && BodyType.SOLID && ModifiableEntityOnly.YES) }
        definition.scope is Query;
    }
    {
        // ------------- Error checking -------------

        // Holes are now supported in sheet metal so the queryContainsActiveSheetMetal check is not wanted in newer parts
        if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V621_SHEET_METAL_HOLES) && queryContainsActiveSheetMetal(context, definition.scope))
        {
            const smQueries = separateSheetMetalQueries(context, definition.scope).sheetMetalQueries;
            throw regenError(ErrorStringEnum.SHEET_METAL_PARTS_PROHIBITED, ["scope"], smQueries);
        }

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

            if (definition.endStyle != HoleEndStyle.THROUGH)
            {
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
        }

        // ------------- Definition adjustment -------------

        if (definition.endStyle != HoleEndStyle.BLIND && (definition.endStyle != HoleEndStyle.THROUGH || definition.style == HoleStyle.SIMPLE))
        {
            definition.startFromSketch = false;
        }

        if ((definition.style == HoleStyle.C_BORE && tolerantEquals(definition.holeDiameter, definition.cBoreDiameter)) ||
            (definition.style == HoleStyle.C_SINK && tolerantEquals(definition.holeDiameter, definition.cSinkDiameter)))
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1142_HOLE_FIXES))
                definition.style = HoleStyle.SIMPLE;
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

        definition.transform = getRemainderPatternTransform(context, { "references" : definition.locations });

        const locations = reduceLocations(context, definition.locations);
        if (locations == [])
        {
            throw regenError(ErrorStringEnum.HOLE_NO_POINTS, ["locations"]);
        }
        if (size(locations) > MAX_LOCATIONS && isAtVersionOrLater(context, FeatureScriptVersionNumber.V274_HOLE_LIMIT_NUM_LOCATIONS_100))
        {
            throw regenError(ErrorStringEnum.HOLE_EXCEEDS_MAX_LOCATIONS, ["locations"]);
        }

        // -- If any feature status is set above this line, `produceHoles` will display the hole tools as error entities --

        // ------------- Perform the operation -------------
        produceHoles(context, id, definition, locations);

        // Verify consistency between pitch, tap depth, and clearance (BEL-120375)
        // This must be done after `produceHoles`, because that function will display error entities if the feature has a status.
        if (definition.showTappedDepth && definition.endStyle != HoleEndStyle.THROUGH && isAtVersionOrLater(context, FeatureScriptVersionNumber.V1135_HOLE_TAP_CHECK))
        {
            var pitch = computePitch(definition);
            if (pitch != undefined && !tolerantEquals(definition.holeDepth, definition.tappedDepth + definition.tapClearance * pitch))
            {
                reportFeatureWarning(context, id, ErrorStringEnum.HOLE_INCONSISTENT_TAP_INFO);
            }
        }
    }, {
            endStyle : HoleEndStyle.BLIND,
            style : HoleStyle.SIMPLE,
            oppositeDirection : false,
            tipAngle : 118 * degree,
            useTipDepth : false,
            cSinkUseDepth : false,
            cSinkDepth : 0 * meter,
            cSinkAngle : 90 * degree,
            startFromSketch : false,
            showTappedDepth : false,
            tappedDepth : 0.5 * inch,
            tapClearance : 3,
            isTappedThrough : false
        });

function reportingStatus(context is Context, id is Id) returns boolean
{
    return getFeatureError(context, id) != undefined || getFeatureWarning(context, id) != undefined || getFeatureInfo(context, id) != undefined;
}

function hasErrors(context is Context, id is Id) returns boolean
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V782_HOLE_ERROR_REPORTING))
        return getFeatureError(context, id) != undefined;
    else
        return reportingStatus(context, id);
}

function produceHoles(context is Context, topLevelId is Id, definition is map, locations is array)
{
    // ------------- Check scope ---------------
    definition.scopeSize = 0.1 * meter; // Set a default so that if we fail early, we can still generate the error entities (BEL-139027)
    const scopeTest = evaluateQuery(context, definition.scope);
    if (size(scopeTest) == 0)
    {
        throwRegenErrorWithToolErrorEntities(context, topLevelId, definition, locations, ErrorStringEnum.HOLE_EMPTY_SCOPE, ["scope"]);
    }
    definition.scopeSize = try(scopeSize(context, definition));
    if (definition.scopeSize == undefined)
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1285_HOLE_SCOPE_DEFAULT))
        {
            // Give some default so that we do not fail during error entity generation
            definition.scopeSize = 0.1 * meter;
        }
        throwRegenErrorWithToolErrorEntities(context, topLevelId, definition, locations, ErrorStringEnum.HOLE_FAIL_BBOX, ["scope"]);
    }

    var trackedBodies = [];
    for (var body in evaluateQuery(context, definition.scope))
    {
        trackedBodies = append(trackedBodies, qUnion([startTracking(context, body), body]));
    }
    const startingBodyCount = size(evaluateQuery(context, qEverything(EntityType.BODY)));

    // ------------- Perform the operation ---------------
    definition.generateErrorBodies = false;
    var opResult = holeOp(context, topLevelId, locations, definition);

    // ------------- Check the result ---------------
    if (opResult.numSuccess == 0)
    {
        const errorInResult = isAtVersionOrLater(context, FeatureScriptVersionNumber.V362_HOLE_IMPROVED_DEPTH_FINDING);
        const error = (errorInResult && opResult.error != undefined) ? opResult.error : ErrorStringEnum.HOLE_NO_HITS;
        throwRegenErrorWithToolErrorEntities(context, topLevelId, definition, locations, error, ["scope"]);
    }

    // The V371_HOLE_IMPROVED_DISJOINT_CHECK check and contained code fixes obscure behaviors and prevents valid cases from working, so we'll ignore it going forward
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V371_HOLE_IMPROVED_DISJOINT_CHECK) &&
        !isAtVersionOrLater(context, FeatureScriptVersionNumber.V621_SHEET_METAL_HOLES))
    {
        for (var trackedBody in trackedBodies)
        {
            var trackedCount = size(evaluateQuery(context, trackedBody));
            if (trackedCount != 1)
            {
                const error = trackedCount > 1 ? ErrorStringEnum.HOLE_DISJOINT : ErrorStringEnum.HOLE_DESTROY_SOLID;
                throwRegenErrorWithToolErrorEntities(context, topLevelId, definition, locations, error, ["scope"]);
            }
        }
    }
    else
    {
        const finalBodyCount = size(evaluateQuery(context, qEverything(EntityType.BODY)));
        if (finalBodyCount > startingBodyCount)
        {
            throwRegenErrorWithToolErrorEntities(context, topLevelId, definition, locations, ErrorStringEnum.HOLE_DISJOINT, ["scope"]);
        }
        else if (opResult.warning != undefined)
        {
            reportFeatureWarning(context, topLevelId, opResult.warning);
        }
    }

    // This is bad practice, and it would be good to avoid it if possible, but it currently captures cases where
    // feature statuses are set deeper in the operation (such as tools and targets not intersecting in the boolean)
    if (reportingStatus(context, topLevelId))
    {
        displayToolErrorEntities(context, topLevelId, definition, locations);
    }
}

function throwRegenErrorWithToolErrorEntities(context is Context, topLevelId is Id, definition is map, locations is array, error is ErrorStringEnum, faultyParameters is array)
{
    displayToolErrorEntities(context, topLevelId, definition, locations);
    throw regenError(error, faultyParameters);
}

function displayToolErrorEntities(context is Context, topLevelId is Id, definition is map, locations is array)
{
    const errorId = topLevelId + "errorEntities";
    definition.generateErrorBodies = true;
    holeOp(context, errorId, locations, definition);
    var errorBodyQuery = qCreatedBy(errorId, EntityType.BODY);
    setErrorEntities(context, topLevelId, { "entities" : errorBodyQuery });
    opDeleteBodies(context, topLevelId + "delete", { "entities" : errorBodyQuery });
}

function getAndUpdateHoleFeatureCount(context is Context) returns number
{
    var value = try silent(getVariable(context, HOLE_FEATURE_COUNT_VARIABLE_NAME));
    if (value == undefined)
        value = 0;
    setVariable(context, HOLE_FEATURE_COUNT_VARIABLE_NAME, value + 1);
    return value;
}

function holeOp(context is Context, id is Id, locations is array, definition is map) returns map
{
    var result = { "numSuccess" : 0 };
    // for each hole
    var holeNumber = -1;
    definition.cutOption = getCutOption(context, definition);
    var holeNumberToResult = {};
    const holeBodiesId = isAtVersionOrLater(context, FeatureScriptVersionNumber.V763_HOLE_CUT_ALL) ? id + "holeBodies" : id;

    definition.holeFeatureCount = getAndUpdateHoleFeatureCount(context);

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
            result = holeAtLocation(context, holeBodiesId, holeNumber, location, definition, result);
            holeNumberToResult[holeNumber] = result;
        }
    }
    if (definition.cutOption == HoleCutOption.CUT_ALL && result.numSuccess > 0)
    {
        try
        {
            booleanBodies(context, id + "boolean", { "targets" : definition.scope,
                        "tools" : qBodyType(qCreatedBy(holeBodiesId, EntityType.BODY), BodyType.SOLID),
                        "operationType" : BooleanOperationType.SUBTRACTION
                    });
        }
        processSubfeatureStatus(context, id, { "subfeatureId" : id + "boolean",
                    "propagateErrorDisplay" : true,
                    "featureParameterMap" : { "tools" : "locations", "targets" : "scope" }
                });
        if (hasErrors(context, id))
        {
            result.numSuccess = 0;
            result.error = ErrorStringEnum.HOLE_CUT_FAIL;
        }
        else
        {
            var sheetMetalModels = getSheetMetalModels(context, definition);
            var holeEdgesQ = qNothing();
            if (sheetMetalModels != undefined)
            {
                holeEdgesQ = getSheetMetalHoleEdgesQuery(id + "boolean", sheetMetalModels, true);
            }
            for (var holeTracking in holeNumberToResult)
            {
                const holeId = id + ("hole-" ~ holeTracking.key);
                createAttributesFromTracking(context, holeId, definition, holeTracking.key, holeTracking.value.faceTracking, definition.style, holeTracking.value.startDistances, holeTracking.value.holeDepth);
                if (sheetMetalModels != undefined && holeTracking.value.instanceTracking != undefined)
                {
                    const instanceHoleEdges = evaluateQuery(context, qIntersection([holeEdgesQ, holeTracking.value.instanceTracking]));
                    assignSheetMetalHoleAttributes(context, holeId, instanceHoleEdges, definition, definition.style, holeNumber);
                }
            }
        }
    }
    return result;
}

function computeCSys(context is Context, location is Query, definition is map) returns CoordSystem
{
    const sign = definition.oppositeDirection ? 1 : -1;

    var point;
    var locationPlane;
    var mateConnectorCSys = try silent(evMateConnector(context, { "mateConnector" : location }));
    if (mateConnectorCSys != undefined)
    {
        if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V1021_HOLE_MATE_CONNECTOR_CSYS))
        {
            // This accidentally disregards need for pattern transform
            mateConnectorCSys.zAxis *= sign;
            return mateConnectorCSys;
        }

        locationPlane = plane(mateConnectorCSys);
        point = mateConnectorCSys.origin;
    }
    else
    {
        locationPlane = evOwnerSketchPlane(context, { "entity" : location });
        point = evVertexPoint(context, { "vertex" : location });
    }

    var startPointCSys;
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V364_HOLE_FIX_FEATURE_MIRROR))
    {
        var ray = line(point, locationPlane.normal);
        if (definition.transform != undefined)
        {
            ray = definition.transform * ray;
        }
        startPointCSys = coordSystem(ray.origin, perpendicularVector(ray.direction), ray.direction);

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V388_HOLE_FIX_FEATURE_PATTERN_TRANSFORM))
        {
            point = startPointCSys.origin;
        }
    }
    else
    {
        startPointCSys = planeToCSys(locationPlane);
        if (definition.transform != undefined)
        {
            point = definition.transform * point;
            startPointCSys = definition.transform * startPointCSys;
        }
    }

    return coordSystem(point, startPointCSys.xAxis, sign * startPointCSys.zAxis);
}

function holeAtLocation(context is Context, id is Id, holeNumber is number, location is Query, definition is map, result is map) returns map
{
    var startPointCSys = computeCSys(context, location, definition);

    const useUnstableComponent = isAtVersionOrLater(context, FeatureScriptVersionNumber.V960_HOLE_IDENTITY);
    const holeId = (useUnstableComponent) ? id + unstableIdComponent("hole-" ~ holeNumber) : id + ("hole-" ~ holeNumber);

    var startDistances = { "resultFront" : [{ "distance" : 0 * meter }] };
    const changeStartPoint = calculateStartPoint(context, definition);
    if (changeStartPoint || size(evaluateQuery(context, definition.scope)) > 1)
    {
        var cylinderCastDiameter = maxDiameter(definition);
        var firstBodyCastDiameter = undefined;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V571_HOLE_BLIND_IN_LAST_PROJECTION) && definition.endStyle == HoleEndStyle.BLIND_IN_LAST &&
            definition.style != HoleStyle.SIMPLE && definition.tapDrillDiameter != undefined)
        {
            // if we are doing a 'blind in last' non-simple hole, use the tap drill diameter to determine last body hit point
            cylinderCastDiameter = definition.tapDrillDiameter;

            // and use the cbore/csink diameter to determine first body hit point
            firstBodyCastDiameter = maxDiameter(definition);
        }

        startDistances = cylinderCastBiDirectional(context, holeId, {
                    "scopeSize" : definition.scopeSize,
                    "cSys" : startPointCSys,
                    "diameter" : cylinderCastDiameter,
                    "firstBodyCastDiameter" : firstBodyCastDiameter,
                    "scope" : definition.scope,
                    "needBack" : false });
        if (definition.startFromSketch)
        {
            // If we're not actually changing the start point and the distances are only
            // for the callouts, put the 0-distance start point back in the array:
            startDistances.resultFront = concatenateArrays([[{ "distance" : 0 * meter }], startDistances.resultFront]);
        }
    }
    if (useUnstableComponent)
    {
        setExternalDisambiguation(context, holeId, location);
    }
    var cutHoleResult = cutHole(context, holeId, definition, holeNumber, startDistances, startPointCSys);
    if (cutHoleResult.success)
    {
        result.numSuccess += result.numSuccess + 1;
        result.faceTracking = cutHoleResult.faceTracking;
        result.instanceTracking = cutHoleResult.instanceTracking;
        result.startDistances = startDistances.resultFront;
        result.holeDepth = cutHoleResult.holeDepth;
    }
    if (result.error == undefined && cutHoleResult.error != undefined)
    {
        result.error = cutHoleResult.error;
    }
    if (result.warning == undefined && cutHoleResult.warning != undefined)
    {
        result.warning = cutHoleResult.warning;
    }
    return result;
}

function calculateStartPoint(context is Context, definition is map) returns boolean
{
    if (definition.generateErrorBodies || definition.heuristics == true)
        return false;
    if (definition.endStyle == HoleEndStyle.THROUGH && definition.style == HoleStyle.SIMPLE)
        return false;

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V401_HOLE_CLEAR_START_FROM_SKETCH))
    {
        return true;
    }
    return !definition.startFromSketch;
}

function maxDiameter(definition is map) returns ValueWithUnits
{
    var result = definition.holeDiameter;
    if (definition.style == HoleStyle.C_BORE)
        result = definition.cBoreDiameter;
    else if (definition.style == HoleStyle.C_SINK)
        result = definition.cSinkDiameter;
    return result;
}

function reduceLocations(context is Context, rawLocationQuery is Query) returns array
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1283_HOLE_REDUCE))
    {
        return clusterVertexQueries(context, rawLocationQuery);
    }

    // -- Very bad performance, clusterVertexQueries is much more optimized --
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

const HOLE_CLEARANCE_BOUNDS =
{
            (unitless) : [0, 3, 100]
        } as RealBoundSpec;

const HOLE_DIAMETER_BOUNDS =
{
            (meter) : [1e-5, 0.005, 500],
            (centimeter) : 0.5,
            (millimeter) : 5.0,
            (inch) : 0.25,
            (foot) : 0.02,
            (yard) : 0.007
        } as LengthBoundSpec;

const HOLE_MAJOR_DIAMETER_BOUNDS =
{
            (meter) : [1e-5, 0.005, 500],
            (centimeter) : 0.5,
            (millimeter) : 5.0,
            (inch) : 0.25,
            (foot) : 0.02,
            (yard) : 0.007
        } as LengthBoundSpec;

const HOLE_BORE_DIAMETER_BOUNDS =
{
            (meter) : [1e-5, 0.01, 500],
            (centimeter) : 1.0,
            (millimeter) : 10.0,
            (inch) : 0.5,
            (foot) : 0.04,
            (yard) : 0.014
        } as LengthBoundSpec;

const HOLE_DEPTH_BOUNDS =
{
            (meter) : [1e-5, 0.012, 500],
            (centimeter) : 1.2,
            (millimeter) : 12.0,
            (inch) : 0.5,
            (foot) : 0.04,
            (yard) : 0.014
        } as LengthBoundSpec;

const HOLE_BORE_DEPTH_BOUNDS =
{
            (meter) : [0.0, 0.005, 500],
            (centimeter) : 0.5,
            (millimeter) : 5.0,
            (inch) : 0.25,
            (foot) : 0.02,
            (yard) : 0.007
        } as LengthBoundSpec;

/**
 * Angle bounds for a hole countersink.
 */
export const CSINK_ANGLE_BOUNDS =
{
            (degree) : [44.9, 90, 135.1],
            (radian) : 0.5 * PI
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

function getSheetMetalModels(context is Context, holeDefinition is map)
{
    var sheetMetalModels;
    const smPartition = partitionSheetMetalParts(context, holeDefinition.scope);
    if (size(smPartition.sheetMetalPartsMap) > 0)
    {
        var sheetMetalParts = [];
        for (var entry in smPartition.sheetMetalPartsMap)
        {
            sheetMetalParts = concatenateArrays([sheetMetalParts, entry.value]);
        }
        sheetMetalModels = getSheetMetalModelForPart(context, qUnion(sheetMetalParts));
    }
    return sheetMetalModels;
}

function cutHole(context is Context, id is Id, holeDefinition is map, holeNumber is number, startDistances is map, cSys is CoordSystem) returns map
{
    // Need to get the sheet metal models from the merge scope so that we can look for changes to the model following the cuts.
    var sheetmetalModels;
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V621_SHEET_METAL_HOLES))
    {
        sheetmetalModels = getSheetMetalModels(context, holeDefinition);
    }

    var result = {};
    var frontDist = 0 * meter;
    const isCBore = holeDefinition.style == HoleStyle.C_BORE;
    const isCSink = holeDefinition.style == HoleStyle.C_SINK;

    if (size(startDistances.resultFront) > 0)
    {
        frontDist = startDistances.resultFront[0].distance;
    }

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V831_HOLE_TOLERANT_DIST) &&
        abs(frontDist / meter) < TOLERANCE.zeroLength)
    {
        frontDist = 0 * meter;
    }

    const sign = holeDefinition.oppositeDirection ? 1 : -1;
    const startCSys = coordSystem(cSys.origin, cSys.xAxis, sign * cSys.zAxis);

    const sketchPlane = plane(cSys.origin, cSys.xAxis, cSys.zAxis); // plane is rotated

    //------------- Create profile ----------------
    const sketchId = id + "sketch";
    const sketch = newSketchOnPlane(context, sketchId, { "sketchPlane" : sketchPlane });

    var startDepth = 0 * meter;
    var cboreTrackingSpecs = [];
    var csinkTrackingSpecs = [];
    var holeStyle = HoleStyle.SIMPLE;

    //Using common start prefix ensures intersection edge reference stability towards hole type  change.
    const useCommonPrefix = isAtVersionOrLater(context, FeatureScriptVersionNumber.V960_HOLE_IDENTITY);
    const commonStartPrefix = "start";
    if (isCBore)
    {
        holeStyle = HoleStyle.C_BORE;
        cboreTrackingSpecs = sketchCBore(context, {
                    "prefix" : (useCommonPrefix) ? commonStartPrefix : "cbore_start",
                    "sketch" : sketch,
                    "startDepth" : startDepth,
                    "endDepth" : frontDist + holeDefinition.cBoreDepth,
                    "cBoreDiameter" : holeDefinition.cBoreDiameter });

        startDepth = frontDist;
        frontDist = 0 * meter;
    }

    if (isCSink)
    {
        holeStyle = HoleStyle.C_SINK;
        var cSinkStartDepth = startDepth;
        if (isCBore)
            cSinkStartDepth += holeDefinition.cBoreDepth;

        csinkTrackingSpecs = sketchCSink(context, {
                    "prefix" : (useCommonPrefix) ? commonStartPrefix : "csink_start",
                    "sketch" : sketch,
                    "isPositive" : true,
                    "startDepth" : cSinkStartDepth,
                    "clearanceDepth" : frontDist,
                    "cSinkUseDepth" : holeDefinition.cSinkUseDepth,
                    "cSinkDepth" : holeDefinition.cSinkDepth,
                    "cSinkDiameter" : holeDefinition.cSinkDiameter,
                    "cSinkAngle" : holeDefinition.cSinkAngle });

        if (frontDist != 0 * meter)
            startDepth = frontDist;
        frontDist = 0 * meter;
    }

    // Handle hole core
    var coreResult = sketchToolCore(context, id, {
            "prefix" : "core",
            "sketch" : sketch,
            "cSys" : cSys,
            "startDepth" : startDepth,
            "clearanceDepth" : frontDist,
            "startDistances" : startDistances,
            "holeDefinition" : holeDefinition });
    if (coreResult.error != undefined)
    {
        result.success = false;
        result.error = coreResult.error;
        return result;
    }
    var blindBody = coreResult.lastBody;
    skSolve(sketch);

    // start tracking the required sketch entities that will create faces in which need to add attributes onto later
    var coreTrackingSpecs = startSketchTracking(context, sketchId, coreResult.trackingSpecs);
    cboreTrackingSpecs = startSketchTracking(context, sketchId, cboreTrackingSpecs);
    csinkTrackingSpecs = startSketchTracking(context, sketchId, csinkTrackingSpecs);

    const axisQuery = sketchEntityQuery(id + ("sketch" ~ ".wireOp"), EntityType.EDGE, "core_line_0");
    const sketchQuery = qSketchRegion(sketchId, false);
    var doCut = (holeDefinition.cutOption == HoleCutOption.CUT_PER_LOCATION);

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V429_HOLE_SAFE_SKETCH_CLEANUP))
    {
        try
        {
            spinCut(context, id, sketchQuery, axisQuery, holeDefinition.scope, !doCut);
            var warning = getFeatureWarning(context, id);
            if (warning is ErrorStringEnum &&
                (warning == ErrorStringEnum.SHEET_METAL_COULD_NOT_UNFOLD || warning == ErrorStringEnum.SHEET_METAL_SELF_INTERSECTING_FLAT))
            {
                result.warning = warning;
            }
        }
        catch (error)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V621_SHEET_METAL_HOLES))
            {
                if (error is map && (error.message == ErrorStringEnum.SHEET_METAL_REBUILD_ERROR || error.message == ErrorStringEnum.SHEET_METAL_CUT_JOINT))
                {
                    result.error = error.message;
                }
            }
        }
    }
    else
    {
        spinCut(context, id, sketchQuery, axisQuery, holeDefinition.scope, !doCut);
    }
    opDeleteBodies(context, id + "delete_sketch", { "entities" : qCreatedBy(sketchId, EntityType.BODY) });

    const newFaces = evaluateQuery(context, qCreatedBy(id, EntityType.FACE));
    var success = true;
    if (size(newFaces) == 0)
        success = false;

    const faceTracking = concatenateArrays([coreTrackingSpecs, cboreTrackingSpecs, csinkTrackingSpecs]);

    if (doCut)
    {
        if (success)
        {
            // add required attributes onto faces that were created based upon our tracked sketch entities
            createAttributesFromTracking(context, id, holeDefinition, holeNumber, faceTracking, holeStyle, startDistances.resultFront, coreResult.holeDepth);

            if (sheetmetalModels != undefined)
            {
                createSheetMetalHoleAttributes(context, id, sheetmetalModels, holeDefinition, holeStyle, holeNumber);
            }
        }

        if (blindBody != undefined && tappedHoleWithOffset(holeDefinition))
        {
            // Find the cylindrical face drilled by this feature in the body with the blind hole.
            var targetFace = qGeometry(qOwnedByBody(qCreatedBy(id, EntityType.FACE), blindBody), GeometryType.CYLINDER);
            opOffsetFace(context, id + "offset", {
                        "moveFaces" : targetFace,
                        "offsetDistance" : (holeDefinition.holeDiameter - holeDefinition.tapDrillDiameter) / 2
                    });
        }
    }
    else if (success && holeDefinition.cutOption == HoleCutOption.CUT_ALL)
    {
        result.faceTracking = faceTracking;
        result.holeDepth = coreResult.holeDepth;
        //Instance tracking is used to disambiguate topology corresponding to different hole instances created after cut
        const circularEdgesQ = qGeometry(qCreatedBy(id, EntityType.EDGE), GeometryType.CIRCLE);
        const cylindricalFacesQ = qGeometry(qCreatedBy(id, EntityType.FACE), GeometryType.CYLINDER);
        result.instanceTracking = startTracking(context, {
                    "subquery" : qUnion([circularEdgesQ, cylindricalFacesQ]),
                    "trackPartialDependency" : true
                });
    }
    result.success = success;
    return result;
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

function getSheetMetalHoleEdgesQuery(id is Id, sheetMetalModels is Query, includeArcs is boolean) returns Query
{
    // Now we look for edges affected by the sheet metal model
    const createdEdges = qBodyType(qCreatedBy(id, EntityType.EDGE), BodyType.SHEET);
    const smEdges = qOwnedByBody(createdEdges, sheetMetalModels);
    if (includeArcs)
        return qUnion([qGeometry(smEdges, GeometryType.CIRCLE), qGeometry(smEdges, GeometryType.ARC)]);
    else
        return qGeometry(smEdges, GeometryType.CIRCLE);
}

function assignSheetMetalHoleAttributes(context is Context, id is Id, holeEdges is array,
    holeDefinition is map, holeStyle is HoleStyle, holeNumber is number)
{
    for (var holeEdge in holeEdges)
    {
        var associations = getSMAssociationAttributes(context, holeEdge);
        for (var association in associations)
        {
            // qBodyType filter has a side-effect of filtering out private bodies.
            // We need to assign attribute to associated faces of private patches so that
            // attribute propagates in downstream operations where the corresponding patch is not rebuilt.
            const holeFacesQ = (isAtVersionOrLater(context, FeatureScriptVersionNumber.V859_SM_HOLE_ATTRIBUTE)) ?
                qEntityFilter(qAttributeQuery(association), EntityType.FACE) :
                qEntityFilter(qBodyType(qAttributeQuery(association), BodyType.SOLID), EntityType.FACE);
            const holeFaces = evaluateQuery(context, holeFacesQ);
            if (size(holeFaces) > 0)
            {
                createAttributesForSheetMetalHole(context, id, holeEdge, holeFacesQ, holeDefinition, holeStyle, holeNumber);
            }
        }
    }
}

function createSheetMetalHoleAttributes(context is Context, id is Id, sheetMetalModels is Query, holeDefinition is map, holeStyle is HoleStyle, holeNumber is number)
{
    const holeEdgesQ = getSheetMetalHoleEdgesQuery(id, sheetMetalModels, false);
    assignSheetMetalHoleAttributes(context, id, evaluateQuery(context, holeEdgesQ), holeDefinition, holeStyle, holeNumber);
}

function createAttributesForSheetMetalHole(context is Context, id is Id, holeEdge is Query, holeFaces is Query, holeDefinition is map, holeStyle is HoleStyle, holeNumber is number)
{
    clearHoleAttributes(context, holeFaces);
    var holeAttribute;
    var cylinder = evSurfaceDefinition(context, {
            "face" : holeFaces
        });
    if (cylinder is Cylinder)
    {
        // Sheet metal holes are always simple and through
        holeDefinition.holeDiameter = cylinder.radius * 2;
        holeDefinition.endStyle = HoleEndStyle.THROUGH;
        holeAttribute = createHoleAttribute(id, holeDefinition, HoleStyle.SIMPLE, HoleSectionFaceType.THROUGH_FACE, holeNumber);
        setAttribute(context, { "entities" : qUnion([holeEdge, holeFaces]), "attribute" : holeAttribute });
    }
}

function setupTrackingLineIds(context is Context, sketchLinePrefix is string, lineIndexIdAndType is array) returns array
{
    var trackingArray = [];
    const numIndexes = size(lineIndexIdAndType);
    for (var index = 0; index < numIndexes; index += 1)
    {
        if (lineIndexIdAndType[index].lineIndex != undefined && lineIndexIdAndType[index].holeTrackType != undefined)
        {
            const lineId = sketchLinePrefix ~ "_line_" ~ lineIndexIdAndType[index].lineIndex;
            trackingArray = append(trackingArray, createTrackingObject(lineId, lineIndexIdAndType[index].holeTrackType));
        }
    }
    return trackingArray;
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

function sketchCBore(context is Context, arg is map) returns array
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

    var lineTracking = [
        // line that starts from the second point in the cbore sketch represents the line that creates the cbore depth face
        { "lineIndex" : 1, "holeTrackType" : HoleSectionFaceType.CBORE_DEPTH_FACE },
        // line that starts from the third point in the cbore sketch represents the line that creates the cbore diameter face
        { "lineIndex" : 2, "holeTrackType" : HoleSectionFaceType.CBORE_DIAMETER_FACE }
    ];

    return setupTrackingLineIds(context, arg.prefix, lineTracking);
}

function sketchCSink(context is Context, arg is map) returns array
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

    var lineTracking = [
        // line that starts from the second point in the csink sketch represents the line that creates the csink angular face
        { "lineIndex" : 1, "holeTrackType" : HoleSectionFaceType.CSINK_FACE },
        // line that starts from the third point in the csink sketch represents the line that creates the csink cbore diameterface
        { "lineIndex" : 2, "holeTrackType" : HoleSectionFaceType.CSINK_CBORE_FACE }
    ];

    return setupTrackingLineIds(context, arg.prefix, lineTracking);
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
    arg.startDistances is map;
    arg.holeDefinition is map;
}
{
    var result = {};
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
        const distance = isAtVersionOrLater(context, FeatureScriptVersionNumber.V299_HOLE_FEATURE_FIX_BLIND_IN_LAST_FLIP) ? 0 * meter : arg.holeDefinition.scopeSize * 2;
        var resultFront = arg.startDistances.resultFront;
        if (resultFront != undefined && resultFront is array && isAtVersionOrLater(context, FeatureScriptVersionNumber.V362_HOLE_IMPROVED_DEPTH_FINDING))
        {
            var startDist = 0 * meter;
            if (arg.holeDefinition.generateErrorBodies)
            {
                startDist = 0 * meter;
            }
            else if (size(resultFront) == 0)
            {
                // report this error early otherwise we report the wrong error
                result.error = ErrorStringEnum.HOLE_NO_HITS;
                return result;
            }
            else if (size(resultFront) == 1)
            {
                result.error = ErrorStringEnum.HOLE_CANNOT_DETERMINE_LAST_BODY;
                return result;
            }
            else
            {
                var lastResult = resultFront[size(resultFront) - 1];
                lastBody = lastResult.target;
                startDist = lastResult.distance;
                if (startDist < 0)
                {
                    startDist = 0 * meter;
                }
            }
            result.holeDepth = startDist + depth;
            depth += startDist - arg.startDepth - arg.clearanceDepth;
            if (arg.holeDefinition.holeDiameter > arg.holeDefinition.tapDrillDiameter + TOLERANCE.zeroLength * meter)
            {
                var delta = (arg.holeDefinition.holeDiameter - arg.holeDefinition.tapDrillDiameter) / 2 / tan(tipAngle / 2);
                depth -= delta;
            }
        }
        else
        {
            // Find shoulder depth
            try
            {

                const castResult = cylinderCast(context, id + "foo" + "limit_surf_cast", {
                            "distance" : distance,
                            "cSys" : arg.cSys,
                            "isFront" : true,
                            "findClosest" : false,
                            "diameter" : 2 * radius,
                            "scope" : arg.holeDefinition.scope });
                if (castResult.distance != undefined)
                {
                    var startDist = castResult.distance;
                    if (startDist < 0 && isAtVersionOrLater(context, FeatureScriptVersionNumber.V291_HOLE_FEATURE_STANDARD_DEFAULTS))
                    {
                        startDist = 0 * meter;
                    }
                    result.holeDepth = startDist + depth;
                    depth += startDist - arg.startDepth - arg.clearanceDepth;
                    if (arg.holeDefinition.holeDiameter > arg.holeDefinition.tapDrillDiameter + TOLERANCE.zeroLength * meter)
                    {
                        var delta = (arg.holeDefinition.holeDiameter - arg.holeDefinition.tapDrillDiameter) / 2 / tan(tipAngle / 2);
                        depth -= delta;
                    }
                    lastBody = castResult.target;
                }
            }
        }
        if (useTipDepth)
        {
            depth += tipDepth;
        }
    }
    else
    {
        result.holeDepth = arg.startDepth + arg.clearanceDepth + depth - (useTipDepth ? tipDepth : 0 * meter);
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
    result.lastBody = lastBody;

    var lineTracking = [
        // line that starts from the second point in the core sketch represents the line that sweeps the bottom of the hole
        { "lineIndex" : 1, "holeTrackType" : HoleSectionFaceType.BLIND_TIP_FACE },
        // line that starts from the third point in the core sketch represents the line that creates the through hole face
        { "lineIndex" : 2, "holeTrackType" : HoleSectionFaceType.THROUGH_FACE }
    ];

    result.trackingSpecs = setupTrackingLineIds(context, arg.prefix, lineTracking);
    return result;
}

function scopeSize(context is Context, definition is map) returns map
{
    const scopeBox = evBox3d(context, { "topology" : qUnion([definition.scope, definition.locations]) });
    return norm(scopeBox.maxCorner - scopeBox.minCorner);
}

function createTrackingObject(trackId is string, holeTrackType is HoleSectionFaceType) returns map
{
    return { "trackingEntity" : trackId, "sectionType" : holeTrackType };
}

function startSketchTracking(context is Context, sketchId is Id, sketchTracking is array) returns array
{
    var resultTrackingArray = [];

    if (sketchTracking != undefined)
    {
        resultTrackingArray = sketchTracking;
        var sketchTrackingSize = size(resultTrackingArray);
        for (var index = 0; index < sketchTrackingSize; index += 1)
        {
            resultTrackingArray[index].trackingQuery = startTracking(context, sketchId, resultTrackingArray[index].trackingEntity);
        }
    }

    return resultTrackingArray;
}

function createAttributesFromTracking(context is Context, id is Id, holeDefinition is map, holeNumber is number, sketchTracking is array, holeStyle is HoleStyle, startDistances is array, holeDepth)
{
    sketchTracking = filter(sketchTracking, function(track)
        {
            return track.trackingQuery != undefined;
        });

    var trackingQueries = [];
    for (var track in sketchTracking)
        trackingQueries = append(trackingQueries, track.trackingQuery);

    var partToStartDistance = {};
    var lastBody;
    for (var startDistance in startDistances)
    {
        if (startDistance.target != undefined)
        {
            var parts = evaluateQuery(context, startDistance.target);
            if (size(parts) == 1)
            {
                partToStartDistance[parts[0]] = startDistance.distance;
                lastBody = parts[0];
            }
        }
    }

    // Split by parts
    for (var part in evaluateQuery(context, qOwnerBody(qUnion(trackingQueries))))
    {
        var faceTypes = {};
        var entityToSectionType = {};
        var allFaces = [];
        for (var track in sketchTracking)
        {
            const sketchTrackingQuery = qOwnedByBody(qEntityFilter(track.trackingQuery, EntityType.FACE), part);
            allFaces = append(allFaces, sketchTrackingQuery);
            const trackingQueryEntities = evaluateQuery(context, sketchTrackingQuery);
            for (var entity in trackingQueryEntities)
            {
                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V623_SHEETMETAL_HOLE_ATTRIBUTE_FIX) &&
                    queryContainsActiveSheetMetal(context, entity))
                {
                    // Do not want active sheet metal in the regular attribute processing
                    continue;
                }
                entityToSectionType[entity] = track.sectionType;
                faceTypes[track.sectionType] = true;
            }
        }
        var modifiedHoleDefinition = holeDefinition;
        var modifiedHoleStyle = holeStyle;
        // Remove countersink and counterbore if necessary
        if (holeStyle == HoleStyle.C_SINK)
        {
            if (faceTypes[HoleSectionFaceType.CSINK_FACE] == undefined && faceTypes[HoleSectionFaceType.CSINK_CBORE_FACE] == undefined)
            {
                modifiedHoleStyle = HoleStyle.SIMPLE;
            }
        }
        else if (holeStyle == HoleStyle.C_BORE)
        {
            if (faceTypes[HoleSectionFaceType.CBORE_DIAMETER_FACE] == undefined && faceTypes[HoleSectionFaceType.CBORE_DEPTH_FACE] == undefined)
            {
                modifiedHoleStyle = HoleStyle.SIMPLE;
            }
        }
        // Check if this is a thru hole -- it is if there are no tip faces
        if (faceTypes[HoleSectionFaceType.BLIND_TIP_FACE] == undefined)
        {
            modifiedHoleDefinition.endStyle = HoleEndStyle.THROUGH;
        }
        else if (holeDepth != undefined) // Adjust the depth based on startDistances
        {
            var distance = partToStartDistance[part];
            if (distance != undefined)
            {
                modifiedHoleDefinition.holeDepth = holeDepth - distance;
                if (holeDefinition.tappedDepth != undefined)
                    modifiedHoleDefinition.tappedDepth = holeDefinition.tappedDepth + modifiedHoleDefinition.holeDepth - holeDefinition.holeDepth;
            }
        }
        if (holeDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST)
        {
            // For a blind-in-last hole, use the blind-in-last standard to determine tap even if modified hole end style is through
            modifiedHoleDefinition.standardTappedOrClearance = holeDefinition.standardBlindInLast;
        }
        if (part != lastBody && holeDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST)
        {
            modifiedHoleDefinition.standardTappedOrClearance = undefined;
        }
        if (part == lastBody && tappedHoleWithOffset(holeDefinition)) // If this is a tapped hole, adjust diameter
        {
            modifiedHoleDefinition.holeDiameter = holeDefinition.tapDrillDiameter;
        }
        if (modifiedHoleDefinition.endStyle == HoleEndStyle.THROUGH)
        {
            try
            {
                // Check if going through only a subset of the part
                var holeAxis = computeHoleAxis(context, qUnion(allFaces));
                var distance = evDistance(context, { side0 : part, side1 : holeAxis }).distance;
                modifiedHoleDefinition.partialThrough = distance * 2 < modifiedHoleDefinition.holeDiameter - TOLERANCE.zeroLength * meter;
            }
        }
        var actualHoleDepth;
        for (var entry in entityToSectionType)
        {
            clearHoleAttributes(context, entry.key);
            var holeAttribute = createHoleAttribute(id, modifiedHoleDefinition, modifiedHoleStyle, entry.value, holeNumber);
            if (holeAttribute != undefined)
            {
                if (holeAttribute.isTappedHole == true && holeDefinition.endStyle != HoleEndStyle.THROUGH) // If the hole style is thorugh, isTappedThrough is set explicitly
                {
                    try // This shouldn't fail, but might for some reason on a legacy hole.  Don't break the feature in that case
                    {
                        if (actualHoleDepth == undefined)
                            actualHoleDepth = computeActualHoleDepth(context, qUnion(allFaces));
                        holeAttribute.isTappedThrough = (modifiedHoleDefinition.tappedDepth + TOLERANCE.zeroLength * meter) > actualHoleDepth;
                    }
                }
                setAttribute(context, { "entities" : entry.key, "attribute" : holeAttribute });
            }
        }
    }
}

function computeHoleAxis(context is Context, faces is Query) returns Line
{
    var axialFaces = qUnion([qGeometry(faces, GeometryType.CYLINDER), qGeometry(faces, GeometryType.CONE)]);
    return evAxis(context, { "axis" : qNthElement(axialFaces, 0) });
}

function computeActualHoleDepth(context is Context, faces is Query)
{
    var holeDirection = computeHoleAxis(context, faces).direction;
    var holeBox is Box3d = evBox3d(context, {
            "topology" : faces,
            "cSys" : coordSystem(vector(0, 0, 0) * meter, perpendicularVector(holeDirection), holeDirection),
            "tight" : true
        });
    return holeBox.maxCorner[2] - holeBox.minCorner[2];
}

/*
 * !!!!Attention developers! If a change is made to content of hole attributes corresponding changes should be made to
 * SBTHoleAttributeSpec.java and BTHoleUtilities.cpp
 */
function createHoleAttribute(id is Id, holeDefinition is map, holeStyle is HoleStyle, holeFaceType is HoleSectionFaceType, holeNumber is number) returns HoleAttribute
{
    // make the base hole attribute
    var holeAttribute = makeHoleAttribute(toAttributeId(id), holeStyle);

    // add tag info
    holeAttribute.holeNumber = holeNumber;
    holeAttribute.holeFeatureCount = holeDefinition.holeFeatureCount;

    // add common properties
    holeAttribute = addCommonAttributeProperties(holeAttribute, holeStyle, holeDefinition);

    // add properties specific to the section (for example, properties needed for the cBore diameter if this is the cBore diameter section)
    holeAttribute = addSectionSpecsToAttribute(holeAttribute, holeFaceType, holeDefinition);

    return holeAttribute;
}

function addCommonAttributeProperties(attribute is HoleAttribute, holeStyle is HoleStyle, holeDefinition is map) returns HoleAttribute
{
    var resultAttribute = attribute;

    // Through, Blind or Blind in Last
    resultAttribute.endType = holeDefinition.endStyle;
    resultAttribute.showTappedDepth = holeDefinition.showTappedDepth;
    resultAttribute.isTappedThrough = holeDefinition.isTappedThrough;
    resultAttribute.majorDiameter = holeDefinition.majorDiameter;

    // Through hole diameter
    resultAttribute.holeDiameter = holeDefinition.holeDiameter;

    if (resultAttribute.endType == HoleEndStyle.THROUGH)
    {
        resultAttribute.partialThrough = holeDefinition.partialThrough;
    }
    else
    {
        // blind hole depth
        resultAttribute.holeDepth = holeDefinition.holeDepth;
    }

    // initialize tapped hole information
    resultAttribute.isTappedHole = false;
    resultAttribute.tapSize = "";
    var tapSize;
    var tapPitch;
    var isStandardComponentBasedHole = false;
    var isStandardDrillBasedHole = false;
    var standardSizeDesignation = undefined;

    var standardSpec = holeDefinition.standardTappedOrClearance;
    if (holeDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST)
    {
        standardSpec = holeDefinition.standardBlindInLast;
    }

    // determine if tapped hole and setup tapped hole details
    if (standardSpec != undefined)
    {
        for (var entry in standardSpec)
        {
            if (entry.key == "type")
            {
                var matchResult = match(entry.value, ".*[Tt]apped.*");
                resultAttribute.isTappedHole = matchResult.hasMatch;
                isStandardComponentBasedHole = true;
                if (!resultAttribute.isTappedHole)
                {
                    matchResult = match(entry.value, ".*[Dd]rilled.*");

                    // drilled holes are based upon a drill size, not a component size
                    if (matchResult.hasMatch)
                    {
                        isStandardComponentBasedHole = false;
                        isStandardDrillBasedHole = true;
                    }
                }
            }
            else if (entry.key == "size")
            {
                tapSize = entry.value;
                standardSizeDesignation = tapSize;
            }
            else if (entry.key == "pitch")
            {
                tapPitch = entry.value;
            }
        }
    }

    if (standardSizeDesignation != undefined)
    {
        if (isStandardComponentBasedHole)
        {
            resultAttribute.standardComponentSizeDesignation = standardSizeDesignation;
        }
        else if (isStandardDrillBasedHole)
        {
            resultAttribute.standardDrillSizeDesignation = standardSizeDesignation;
        }
    }

    // is this a tapped hole and we found its size?
    if (resultAttribute.isTappedHole && tapSize != undefined && tapPitch != undefined)
    {
        // format tap pitch based upon units
        var pitch = tapPitch;
        var pitchWithUnits;
        var delimiter = "x";
        var result = match(tapPitch, "([0123456789.]*)\\s*(tpi|mm)");
        if (result.hasMatch)
        {
            if (result.captures[2] == "tpi")
            {
                pitch = result.captures[1];
                pitchWithUnits = (1.0 / stringToNumber(pitch)) * inch;

                // use '-' instead of 'x'
                delimiter = "-";
            }
            else if (result.captures[2] == "mm")
            {
                pitch = result.captures[1];
                pitchWithUnits = stringToNumber(pitch) * millimeter;
            }
        }

        // set tap size
        resultAttribute.tapSize = tapSize ~ delimiter ~ pitch;
        if (holeDefinition.tappedDepth != undefined)
            resultAttribute.tappedDepth = holeDefinition.tappedDepth;
        else if (holeDefinition.isTappedThrough)
            resultAttribute.tappedDepth = 0 * meter; // it doesn't really matter, just not undefined

        if (resultAttribute.endType != HoleEndStyle.THROUGH && holeDefinition.tapClearance != undefined)
            resultAttribute.tapClearance = holeDefinition.tapClearance;

        resultAttribute.threadPitch = pitchWithUnits;
    }

    // add properties specific to the hole type
    if (holeStyle == HoleStyle.SIMPLE)
    {
        resultAttribute = addSimpleHoleAttributeProperties(resultAttribute, holeDefinition);
    }
    else if (holeStyle == HoleStyle.C_BORE)
    {
        resultAttribute = addCBoreHoleAttributeProperties(resultAttribute, holeDefinition);
    }
    else if (holeStyle == HoleStyle.C_SINK)
    {
        resultAttribute = addCSinkHoleAttributeProperties(resultAttribute, holeDefinition);
    }

    return resultAttribute;
}

function addSimpleHoleAttributeProperties(attribute is HoleAttribute, holeDefinition is map) returns HoleAttribute
{
    // currently, nothing more to add for simple holes
    return attribute;
}

function addCBoreHoleAttributeProperties(attribute is HoleAttribute, holeDefinition is map) returns HoleAttribute
{
    var resultAttribute = attribute;

    // add cbore specific data for cbore hole types
    resultAttribute.cBoreDiameter = holeDefinition.cBoreDiameter;
    resultAttribute.cBoreDepth = holeDefinition.cBoreDepth;

    return resultAttribute;
}

function addCSinkHoleAttributeProperties(attribute is HoleAttribute, holeDefinition is map) returns HoleAttribute
{
    var resultAttribute = attribute;

    // add csink specific data for csink hole types
    resultAttribute.cSinkDiameter = holeDefinition.cSinkDiameter;
    resultAttribute.cSinkAngle = holeDefinition.cSinkAngle;

    if (holeDefinition.cSinkUseDepth)
    {
        resultAttribute.cSinkDepth = holeDefinition.cSinkDepth;
    }

    return resultAttribute;
}

function addSectionSpecsToAttribute(attribute is HoleAttribute, holeFaceType is HoleSectionFaceType, holeDefinition is map) returns HoleAttribute
{
    var resultAttribute = attribute;

    if (holeFaceType == HoleSectionFaceType.THROUGH_FACE)
    {
        resultAttribute.sectionFace = getThroughSectionAttributeSpecs(holeDefinition);
    }
    else if (holeFaceType == HoleSectionFaceType.CBORE_DIAMETER_FACE)
    {
        resultAttribute.sectionFace = getCBoreDiameterSectionAttributeSpecs(holeDefinition);
    }
    else if (holeFaceType == HoleSectionFaceType.CBORE_DEPTH_FACE)
    {
        resultAttribute.sectionFace = getCBoreDepthSectionAttributeSpecs(holeDefinition);
    }
    else if (holeFaceType == HoleSectionFaceType.CSINK_FACE)
    {
        resultAttribute.sectionFace = getCSinkSectionAttributeSpecs(holeDefinition);
    }
    else if (holeFaceType == HoleSectionFaceType.CSINK_CBORE_FACE)
    {
        resultAttribute.sectionFace = getCSinkCBoreSectionAttributeSpecs(holeDefinition);
    }
    else if (holeFaceType == HoleSectionFaceType.BLIND_TIP_FACE)
    {
        resultAttribute.sectionFace = getBlindTipSectionAttributeSpecs(holeDefinition);
    }

    return resultAttribute;
}

function getThroughSectionAttributeSpecs(holeDefinition is map) returns map
{
    var throughFaceSpec = { "type" : HoleSectionFaceType.THROUGH_FACE };

    // add anything else?
    return throughFaceSpec;
}

function getCBoreDiameterSectionAttributeSpecs(holeDefinition is map) returns map
{
    var cBoreDiameterFaceSpec = { "type" : HoleSectionFaceType.CBORE_DIAMETER_FACE };

    // add anything else?
    return cBoreDiameterFaceSpec;
}

function getCBoreDepthSectionAttributeSpecs(holeDefinition is map) returns map
{
    var cBoreDepthFaceSpec = { "type" : HoleSectionFaceType.CBORE_DEPTH_FACE };

    // add anything else?
    return cBoreDepthFaceSpec;
}

function getCSinkSectionAttributeSpecs(holeDefinition is map) returns map
{
    var cSinkFaceSpec = { "type" : HoleSectionFaceType.CSINK_FACE };

    // add anything else?
    return cSinkFaceSpec;
}

function getCSinkCBoreSectionAttributeSpecs(holeDefinition is map) returns map
{
    var cSinkCboreFaceSpec = { "type" : HoleSectionFaceType.CSINK_CBORE_FACE };

    // add anything else?
    return cSinkCboreFaceSpec;
}

function getBlindTipSectionAttributeSpecs(holeDefinition is map) returns map
{
    var blindTipFaceSpec = { "type" : HoleSectionFaceType.BLIND_TIP_FACE };

    // add anything else?
    return blindTipFaceSpec;
}

enum HoleCutOption
{
    NO_CUT,
    CUT_PER_LOCATION,
    CUT_ALL
}

function getCutOption(context is Context, holeDefinition is map) returns HoleCutOption
{
    if (holeDefinition.generateErrorBodies == true || holeDefinition.heuristics == true)
        return HoleCutOption.NO_CUT;
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V763_HOLE_CUT_ALL) ||
        tappedHoleWithOffset(holeDefinition))
        return HoleCutOption.CUT_PER_LOCATION;
    return HoleCutOption.CUT_ALL;
}

function tappedHoleWithOffset(holeDefinition is map) returns boolean
{
    return holeDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST &&
        holeDefinition.tapDrillDiameter < (holeDefinition.holeDiameter - TOLERANCE.zeroLength * meter);
}

/**
 * @internal
 * Editing logic for hole feature.
 */
export function holeEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (oldDefinition.locations != definition.locations)
    {
        definition.locations = qUnion(clusterVertexQueries(context, definition.locations));
    }
    if (oldDefinition.standardTappedOrClearance != definition.standardTappedOrClearance ||
        oldDefinition.standardBlindInLast != definition.standardBlindInLast ||
        oldDefinition.endStyle != definition.endStyle)
    {
        definition = updateHoleDefinitionWithStandard(oldDefinition, definition);
    }
    definition = setToCustomIfStandardViolated(definition);

    definition = adjustThreadDepth(oldDefinition, definition);

    if (isCreating && (!specifiedParameters.scope || !specifiedParameters.oppositeDirection))
    {
        try
        {
            definition = holeScopeFlipHeuristicsCall(context, id, definition, specifiedParameters, hiddenBodies);
        }
    }
    return definition;
}

/**
 * @internal
 * Update the feature definition to have the correct majorDiameter parameter.
 */
export function holeDefinitionSetMajorDiameter(context is Context, definition is map, changes is map) returns map
{
    if (definition.majorDiameter == undefined)
    {
        definition.majorDiameter = computeMajorDiameter(definition);
    }
    return definition;
}

function computeMajorDiameter(definition is map)
{
    const result = getStandardAndTable(definition);
    const standard = result.standard;
    const table = result.table;
    if (standard != undefined && table != undefined)
    {
        const entry = getLookupTable(table, standard);
        if (entry.majorDiameter != undefined)
        {
            return lookupTableEvaluate(entry.majorDiameter);
        }
    }
    return undefined;
}

function computePitch(definition is map)
{
    var standard = getStandardAndTable(definition).standard;
    if (standard != undefined && standard.pitch != undefined)
    {
        // Check for NN.N tpi or NN.N mm
        var result = match(standard.pitch, "([0123456789.]*)\\s*(tpi|mm)");
        if (result.hasMatch)
        {
            if (result.captures[2] == "tpi")
            {
                return 1.0 / stringToNumber(result.captures[1]) * inch;
            }
            else if (result.captures[2] == "mm")
            {
                return stringToNumber(result.captures[1]) * millimeter;
            }
        }
    }
    return undefined;
}

/*
 * Accounts for tapped holes by computing depth or threaded depth based on pitch
 */
function adjustThreadDepth(oldDefinition is map, definition is map) returns map
{
    if (threadPitchChanged(oldDefinition, definition))
    {
        definition.showTappedDepth = false;
        var pitch = computePitch(definition);
        if (pitch != undefined)
        {
            definition.showTappedDepth = true;

            // if blind hole type and have valid tap clearance value, then calculate and set either tapped or hole depth
            if ((definition.endStyle == HoleEndStyle.BLIND || definition.endStyle == HoleEndStyle.BLIND_IN_LAST) && definition.tapClearance != undefined)
            {
                if (definition.holeDepth != oldDefinition.holeDepth)
                {
                    if (definition.holeDepth != undefined)
                    {
                        definition.tappedDepth = definition.holeDepth - definition.tapClearance * pitch;
                    }
                }
                else
                {
                    if (definition.tappedDepth != undefined)
                    {
                        definition.holeDepth = definition.tappedDepth + definition.tapClearance * pitch;
                    }
                }
            }
        }
    }
    return definition;
}

function threadPitchChanged(oldDefinition is map, definition is map) returns boolean
{
    return oldDefinition.standardBlindInLast != definition.standardBlindInLast ||
        oldDefinition.standardTappedOrClearance != definition.standardTappedOrClearance ||
        oldDefinition.endStyle != definition.endStyle ||
        oldDefinition.holeDepth != definition.holeDepth ||
        oldDefinition.tappedDepth != definition.tappedDepth ||
        oldDefinition.tapClearance != definition.tapClearance;
}

function getStandardAndTable(definition is map) returns map
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
    return { "standard" : standard, "table" : table };
}

function getStandardTable(definition is map) returns map
{
    var result = getStandardAndTable(definition);

    if (result.standard == undefined)
    {
        return {};
    }
    result.table = getLookupTable(result.table, result.standard);
    if (result.table == undefined)
    {
        return {};
    }
    return result.table;
}

/**
 * @internal
 * Implements standard hole sizes. Set the appropriate standard string for the hole type
 * in the same format as the UI specification and it will set the appropriate values,
 * and then return an updated definition.
 */
export function updateHoleDefinitionWithStandard(oldDefinition is map, definition is map) returns map
{
    definition = syncStandards(oldDefinition, definition);
    var evaluatedDefinition = definition;
    var table = getStandardTable(definition);
    for (var entry in table)
    {
        definition[entry.key] = lookupTableFixExpression(entry.value);
        evaluatedDefinition[entry.key] = lookupTableGetValue(definition[entry.key]);
    }

    if (evaluatedDefinition.tapDrillDiameter > evaluatedDefinition.holeDiameter)
    {
        definition.tapDrillDiameter = definition.holeDiameter;
    }
    if (evaluatedDefinition.cBoreDiameter < evaluatedDefinition.holeDiameter)
    {
        definition.cBoreDiameter = definition.holeDiameter;
    }
    if (evaluatedDefinition.cSinkDiameter < evaluatedDefinition.holeDiameter)
    {
        definition.cSinkDiameter = definition.holeDiameter;
    }

    return definition;
}

function syncStandards(oldDefinition is map, definition is map) returns map
{
    if (oldDefinition.standardTappedOrClearance != undefined && oldDefinition.standardTappedOrClearance != definition.standardTappedOrClearance)
    {
        definition.standardBlindInLast = definition.standardTappedOrClearance;
    }
    else if (oldDefinition.standardBlindInLast != undefined && oldDefinition.standardBlindInLast != definition.standardBlindInLast)
    {
        definition.standardTappedOrClearance = definition.standardBlindInLast;
    }
    return definition;
}

function setToCustomIfStandardViolated(definition is map) returns map
{
    var table = getStandardTable(definition);
    if (isLookupTableViolated(definition, table, ignoreInvalidateStandardProperties(table)))
    {
        definition.standardTappedOrClearance = lookupTablePath({ "standard" : "Custom" });
        definition.standardBlindInLast = lookupTablePath({ "standard" : "Custom" });
    }

    return definition;
}

/**
 * Some standard based sizes do not have standard based cbore and/or csink specifications for that size. If that is
 * the case (indicated by a value of -1 in the cbore or csink diameter of the standard data definition), then don't
 * invalidate the standard, but allow the user to put in his own desired cbore/csink values for that standard size
 *
 * @param table : the current table map for the active size
 * @returns : map of property names, if value is true, then that property's value should not be used to invalidate the standard setting
 */
function ignoreInvalidateStandardProperties(table is map) returns map
{
    var ignoreProperties = {};

    // if there's no standard cbore or csink diameter defined for the current size (indicated by a negative value in the data definition),
    // don't invalidate the standard, keep the standard active and allow user to put in what they want
    if (!shouldPropertyValueInvalidateStandard(table, "cBoreDiameter"))
    {
        ignoreProperties["cBoreDiameter"] = true;
        // if the cbore diameter doesn't invalidate the standard, the depth shouldn't either
        ignoreProperties["cBoreDepth"] = true;
    }
    if (!shouldPropertyValueInvalidateStandard(table, "cSinkDiameter"))
    {
        ignoreProperties["cSinkDiameter"] = true;
        // if the csink diameter doesn't invalidate the standard, the angle shouldn't either
        ignoreProperties["cSinkAngle"] = true;
    }

    return ignoreProperties;
}

/**
 * Determines if the specified property name should be used to invalid the standard based upon the specified data table map
 *
 * @param table : the current table map for the active size
 * @param propertyName : the property name to check
 * @returns : true if the property name should be used to check for standard invalidity, else false
 */
function shouldPropertyValueInvalidateStandard(table is map, propertyName is string) returns boolean
{
    var fieldValue = table[propertyName];
    // default data values less than 0 mean this property value should not be used to check for standard invalidity
    return fieldValue == undefined || lookupTableGetValue(fieldValue) >= 0;
}

/**
 * @internal
 */
export function holeScopeFlipHeuristicsCall(context is Context, id is Id, holeDefinition is map, specifiedParameters is map, hiddenBodies is Query) returns map
{
    // This takes about 70 ms per location for a 4 body model. It will probably take longer as the number of bodies goes up.
    // It would be good if we could store and retrieve clash results from previous calls to avoid duplicate computations.
    // All the work is done for this except storing/retrieving the result. There is no way to do that now.
    var scopeIsSet = specifiedParameters.scope;
    var oppositeDirectionSet = specifiedParameters.oppositeDirection;

    var numberOfLocations = size(evaluateQuery(context, holeDefinition.locations));
    if (numberOfLocations == 0 || (scopeIsSet && oppositeDirectionSet))
    {
        // If scope is not set and we have no locations,
        // reset scope to empty.
        if (!scopeIsSet && numberOfLocations == 0)
            holeDefinition.scope = qUnion([]);

        return holeDefinition;
    }

    var solidBodiesQuery is Query = qNothing();
    if (scopeIsSet)
    {
        solidBodiesQuery = holeDefinition.scope;
    }
    else
    {
        solidBodiesQuery = qAllModifiableSolidBodies();
        solidBodiesQuery = qSubtraction(solidBodiesQuery, hiddenBodies);
    }

    var bbox = evBox3d(context, {
            "topology" : qUnion([solidBodiesQuery, holeDefinition.locations])
        });
    var scopeSize = 1.1 * norm(bbox.maxCorner - bbox.minCorner);

    var minSize = holeDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST ? 2 : 1;

    var evaluatedDefinition = holeDefinition;
    var table = getStandardTable(holeDefinition);
    if (table != {})
    {
        for (var entry in table)
        {
            evaluatedDefinition[entry.key] = lookupTableGetValue(holeDefinition[entry.key]);
        }
    }
    const locations = evaluateQuery(context, holeDefinition.locations);
    var numOpposite = 0;
    var numSame = 0;
    var numAmbiguous = 0;
    var scopeSetSame = {};
    var scopeSetOpposite = {};
    var holeNumber = -1;
    var newCollisions = {};
    var oldCollisions = {}; // TODO retrieve old collisions from local storage.
    for (var location in locations)
    {
        holeNumber += 1;
        if (oldCollisions[location] != undefined)
        {
            newCollisions[location] = oldCollisions[location];
            continue;
        }
        var doSame = !oppositeDirectionSet || !holeDefinition.oppositeDirection;
        var doOpposite = !oppositeDirectionSet || holeDefinition.oppositeDirection;
        var resultSame = { "queries" : [], "scopeSet" : {} };
        var resultOpposite = { "queries" : [], "scopeSet" : {} };

        if (doSame)
        {
            resultSame = findCollisions(context, id, holeNumber, solidBodiesQuery, location, scopeSize, false, evaluatedDefinition);
        }
        if (doOpposite)
        {
            resultOpposite = findCollisions(context, id, holeNumber, solidBodiesQuery, location, scopeSize, true, evaluatedDefinition);
        }
        newCollisions[location] = {
                "resultSame" : resultSame,
                "resultOpposite" : resultOpposite
            };
    }
    //    TODO store newCollisions in local storage
    for (var location in locations)
    {
        var collisionResults = newCollisions[location];
        const hasSame = size(collisionResults.resultSame.queries) >= minSize;
        const hasOpposite = size(collisionResults.resultOpposite.queries) >= minSize;
        if (hasSame && !hasOpposite)
        {
            numSame += 1;
            scopeSetSame = mergeMaps(scopeSetSame, collisionResults.resultSame.scopeSet);
        }
        else if (!hasSame && hasOpposite)
        {
            numOpposite += 1;
            scopeSetOpposite = mergeMaps(scopeSetOpposite, collisionResults.resultOpposite.scopeSet);
        }
        else if (hasSame && hasOpposite)
        {
            numAmbiguous += 1;
        }
    }

    // This collapses the list to only the unique queries.
    if (!scopeIsSet)
    {
        if (numAmbiguous != 0 || (numSame != 0 && numOpposite != 0) || (numSame == 0 && numOpposite == 0))
        {
            holeDefinition.scope = qUnion([]);
        }
        else if (numSame != 0 || numOpposite != 0)
        {
            var scopeSet = (numSame > numOpposite) ? scopeSetSame : scopeSetOpposite;
            var queries = [];
            for (var entry in scopeSet)
            {
                queries = append(queries, entry.key);
            }
            holeDefinition.scope = qUnion(queries);
        }
    }
    if (!oppositeDirectionSet)
    {
        holeDefinition.oppositeDirection = numOpposite > numSame;
    }
    return holeDefinition;
}

function processCollisions(context is Context, solidBodiesQuery is Query, collisions is array) returns map
{
    var scopeSet = {};
    for (var collision in collisions)
    {
        if (collision["type"] == ClashType.INTERFERE)
        {
            scopeSet[collision.targetBody] = 1;
        }
    }

    var queries = [];
    for (var entry in scopeSet)
    {
        if (entry.key is Query)
        {
            queries = append(queries, entry.key);
        }
    }
    var result = {
        "queries" : evaluateQuery(context, qUnion(queries)),
        "scopeSet" : scopeSet
    };
    return result;
}

function findCollisions(context is Context, id is Id, holeNumber is number, solidBodiesQuery is Query, location is Query, scopeSize is ValueWithUnits, oppositeDirection is boolean, definition is map)
{
    var tempDefinition = mergeMaps(definition, {
            "heuristics" : true,
            "startFromSketch" : true,
            "style" : HoleStyle.SIMPLE,
            "tipAngle" : 118 * degree,
            "useTipDepth" : false,
            "cSinkUseDepth" : false,
            "cSinkDepth" : 0 * meter,
            "generateErrorBodies" : false,
            "oppositeDirection" : oppositeDirection
        });

    if (tempDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST)
    {
        tempDefinition.endStyle = HoleEndStyle.THROUGH;
    }
    if (tempDefinition.endStyle == HoleEndStyle.THROUGH)
    {
        tempDefinition.scopeSize = scopeSize;
    }

    var heuristicsId = id + (oppositeDirection ? "heuristics_opposite" : "heuristics_same");
    var collisions = [];
    startFeature(context, heuristicsId, tempDefinition);
    try
    {
        var result = { "numSuccess" : 0 };
        result = holeAtLocation(context, heuristicsId, holeNumber, location, tempDefinition, result);
        var toolQuery = qCreatedBy(heuristicsId, EntityType.BODY);
        collisions = evCollision(context, { "tools" : toolQuery, "targets" : qSubtraction(solidBodiesQuery, toolQuery) });
    }
    abortFeature(context, heuristicsId);

    return processCollisions(context, solidBodiesQuery, collisions);
}

/**
 * Expects `selected` query to evaluate to a set of vertices. Throws if non-vertex is passed.
 *
 * Clusters coincident vertices created by the same operation, it is important to group by
 * operation because we may have multiple sketches with different normals which share a location.
 * We would still want to make two holes in that case.
 *
 * Returns an array of vertices, each representing the "first" vertex of a cluster, where "first"
 * is determined by the query evaluation order of `selected`.  The overall ordering of the returned
 * array will also respect the query evaluation order of `selected`.
 */
function clusterVertexQueries(context is Context, selected is Query) returns array
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
    for (var entry in perFeature)
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
                points[i] = evVertexPoint(context, { 'vertex' : entry.value[i] });
            }
            var clusters = clusterPoints(points, TOLERANCE.zeroLength * meter);
            for (var cluster in clusters)
            {
                clusterQueries = append(clusterQueries, entry.value[cluster[0]]);
            }
        }
    }
    return evaluateQuery(context, qIntersection([selected, qUnion(clusterQueries)]));
}

