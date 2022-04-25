FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "✨");
import(path : "onshape/std/boolean.fs", version : "✨");
import(path : "onshape/std/boundingtype.gen.fs", version : "✨");
import(path : "onshape/std/box.fs", version : "✨");
import(path : "onshape/std/clashtype.gen.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/coordSystem.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/cylinderCast.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/holetables.gen.fs", version : "✨");
import(path : "onshape/std/lookupTablePath.fs", version : "✨");
import(path : "onshape/std/mathUtils.fs", version : "✨");
import(path : "onshape/std/revolve.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/sketch.fs", version : "✨");
import(path : "onshape/std/string.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/tool.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");

export import(path : "onshape/std/holeAttribute.fs", version : "✨");
export import(path : "onshape/std/holesectionfacetype.gen.fs", version : "✨");
export import(path : "onshape/std/holeUtils.fs", version : "✨");

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

const MAX_LOCATIONS_V274 = 100;
const MAX_LOCATIONS_V1548 = 500;

function enforceMaxLocations(context is Context, nLocations is number)
{
    const initialLimit = isAtVersionOrLater(context, FeatureScriptVersionNumber.V274_HOLE_LIMIT_NUM_LOCATIONS_100);
    if (!initialLimit)
    {
        return; // No limit before V274
    }

    const increasedLimit = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1617_HOLE_NEW_PIPELINE_2);
    const limit = increasedLimit ? MAX_LOCATIONS_V1548 : MAX_LOCATIONS_V274;
    if (nLocations > limit)
    {
        const errorEnum = increasedLimit ? ErrorStringEnum.HOLE_EXCEEDS_MAX_LOCATIONS_500 : ErrorStringEnum.HOLE_EXCEEDS_MAX_LOCATIONS;
        throw regenError(errorEnum, ["locations"]);
    }
}

const HOLE_FEATURE_COUNT_VARIABLE_NAME = "-holeFeatureCount"; // Not a valid identifier, so it is not offered in autocomplete

/*
 * JAR/IB: The call structure of the principal functions in this file is something like this:
 *
 * // hole does the definition checks
 * hole --> reduceLocations
 *      --> produceHoles
 *
 * // produceHoles does a few additional checks, and then version-branches into the appropriate code path
 * produceHoles --> produceHolesUsingOpHole
 *              --> produceHolesDeprecated
 *
 * // -- New (optmized with opHole) code path --
 *
 * // produceHolesUsingOpHole cuts all the holes out of all the targets using the opHole operation
 * produceHolesUsingOpHole --> buildOpHoleDefinitionAndCallOpHole
 *                         --> handleSheetMetalCutAndAttribution
 *                         --> buildFaceTypeToSectionFaceType // Builds a useful mapping for the attribution pipeline
 *                         --> createAttributesFromQuery      // Creates the attribues
 *
 * // buildOpHoleDefinitionAndCallOpHole transforms the feature definition into an opHole HoleDefinition and calls opHole
 * buildOpHoleDefinitionAndCallOpHole --> computeAxes
 *                                    --> computeStartProfiles // Builds the first half of the hole profiles based on definition.style
 *                                    --> computeEndProfiles   // Builds the second half of the hole profiles based on definition.endStyle
 *                                    --> opHole
 *
 * // createAttributesFromQuery creates attribues using qOpHole<Face/Profile> queries to find the created hole faces
 * createAttributesFromQuery --> adjustDefinitionForAttribute
 *                           --> createHoleAttribute
 *
 * createHoleAttribute --> makeHoleAttribute
 *                     --> addCommonAttributeProperties
 *                     --> addSectionSpecsToAttribute
 *
 * // -- Deprecated code path (common functions already mentioned in new code path elided) --
 *
 * // produceHolesDeprecated stores some state before the hole operation, dispatches to create the holes, and checks
 * // the state after the operation, erroring if necessary.
 * produceHolesDeprecated --> holeOp
 *
 * // holeOp makes all the holes and booleans them with the merge scope
 * holeOp         --> getCutOption                                 // getCutOption checks whether to boolean one hole at a time or all at once at the end
 *                --> holeAtLocation
 *                --> createAttributesFromTracking, assignSheetMetalHoleAttributes
 *
 * // holeAtLocation creates hole body at location (and cuts if one-cut-at-a-time is on)
 * holeAtLocation --> computeCSysDeprecated     // computeCSysDeprecated figures out the coordinate system for the hole
 *                --> calculateStartPoint
 *                --> cylinderCastBiDirectional
 *                --> cutHole
 *
 * cutHole        --> getSheetMetalModelsDeprecated
 *                --> sketchCBore, sketchCSink, sketchToolCore
 *                --> startSketchTracking
 *                --> spinCut                                     // opRevolve, if individual cuts, opBoolean
 *                --> createAttributesFromTracking, assignSheetMetalHoleAttributes
 *
 * createAttributesFromTracking --> adjustDefinitionForAttribute
 *                              --> createHoleAttribute
 *
 * assignSheetMetalHoleAttributes --> createAttributesForSheetMetalHole --> createHoleAttribute
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

        if (definition.endStyle != HoleEndStyle.BLIND_IN_LAST && definition.standardTappedOrClearance != undefined)
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
            annotation { "Name" : "Start from sketch plane", "Default" : false }
            definition.startFromSketch is boolean;
        }

        annotation { "Name" : "Sketch points to place holes",
                    "Filter" : EntityType.VERTEX && SketchObject.YES && ModifiableEntityOnly.YES || BodyType.MATE_CONNECTOR }
        definition.locations is Query;

        annotation { "Name" : "Merge scope",
                    "Filter" : (EntityType.BODY && BodyType.SOLID && ModifiableEntityOnly.YES && AllowMeshGeometry.YES) }
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
        enforceMaxLocations(context, size(locations));

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

    // Used in attribution. Should only be called once because it increments a global variable.
    definition.holeFeatureCount = getAndUpdateHoleFeatureCount(context);

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1617_HOLE_NEW_PIPELINE_2))
    {
        produceHolesUsingOpHole(context, topLevelId, definition, locations);
    }
    else
    {
        produceHolesDeprecated(context, topLevelId, definition, locations);
    }
}

function produceHolesUsingOpHole(context is Context, topLevelId is Id, definition is map, locations is array)
{
    const nHoles = size(locations);
    const sheetMetalTargetInfo = getSheetMetalTargetInfo(context, definition);

    const opHoleId = topLevelId + "opHole";
    var opHoleInfo;
    try
    {
        opHoleInfo = buildOpHoleDefinitionAndCallOpHole(context, topLevelId, opHoleId, definition, locations, {
                    // Allow opHole to do the subtraction of non-sheet metal targets internally (if there are any non-sheet
                    // metal targets). If there are any sheet metal targets, ask opHole to exclude them from that
                    // subtraction, and keep the tools around so that we may use them for a follow-up subtraction in
                    // `handleSheetMetalCutAndAttribution(...)`
                    "subtractFromTargets" : sheetMetalTargetInfo.hasNonSheetMetalTargets,
                    "targetsToExcludeFromSubtraction" : sheetMetalTargetInfo.sheetMetalTargets,
                    "keepTools" : sheetMetalTargetInfo.hasSheetMetalTargets
                });
    }
    catch (error)
    {
        displayToolErrorEntities(context, topLevelId, definition, locations);
        if (error == ErrorStringEnum.HOLE_TARGETS_DO_NOT_DIFFER)
        {
            // HOLE_TARGETS_DO_NOT_DIFFER comes from opHole when the user has chosen BLIND_IN_LAST mode, and we set
            // `targetMustDifferFromPrevious` on the transition to LAST_TARGET_START to ask opHole to make sure that
            // there is a distinct "last target". If thrown, switch to a more specific error which specifically mentions
            // blind in last.
            error = ErrorStringEnum.HOLE_CANNOT_DETERMINE_LAST_BODY;
        }
        throw error;
    }

    // The following two pipelines are not mutually exclusive. It is possible to have both sheet metal and non-sheet metal targets.
    const successfulHoles = new box({});
    if (sheetMetalTargetInfo.hasSheetMetalTargets)
    {
        // If we have sheet metal targets, opHole will leave behind the solid hole tools. Use them for sheet metal,
        // consuming them in the process. This must happen before `createAttributesFromQuery` or that function will get
        // confused by the solid tools still existing.
        handleSheetMetalCutAndAttribution(context, topLevelId, opHoleId, definition, opHoleInfo.returnMapPerHole,
            sheetMetalTargetInfo, locations, successfulHoles);
    }
    if (sheetMetalTargetInfo.hasNonSheetMetalTargets)
    {
        const faceTypeToSectionFaceType = buildFaceTypeToSectionFaceType(opHoleInfo.faceTypes, definition.style);
        for (var i = 0; i < nHoles; i += 1)
        {
            if (!opHoleInfo.returnMapPerHole[i].success)
            {
                continue;
            }

            const instanceProducedFaces = createAttributesFromQuery(context, topLevelId, opHoleId, definition,
                    opHoleInfo.finalPositionReference, buildHoleAttributeId(topLevelId, i), faceTypeToSectionFaceType,
                    locations[i], opHoleInfo.returnMapPerHole[i], i, opHoleInfo.holeDepth);
            if (instanceProducedFaces)
            {
                successfulHoles[][i] = true;
            }
        }
    }

    adjustFeatureStatusAfterProducingHoles(context, topLevelId, definition, locations, successfulHoles);
}

// Used to create holes in produceHolesUsingOpHole, and also to display error entities in displayToolErrorEntities.
function buildOpHoleDefinitionAndCallOpHole(context is Context, topLevelId is Id, opHoleId, definition is map, locations is array, opHoleOverrides is map) returns map
{
    const axes = computeAxes(context, locations, definition.oppositeDirection, /* feature pattern transform */ definition.transform);

    const firstPositionReference = definition.startFromSketch ? HolePositionReference.AXIS_POINT : HolePositionReference.TARGET_START;

    const startProfileInfo = computeStartProfiles(definition, firstPositionReference);
    const endProfileInfo = computeEndProfiles(definition, firstPositionReference);

    const profiles = concatenateArrays([startProfileInfo.profiles, endProfileInfo.profiles]);
    const faceTypes = concatenateArrays([startProfileInfo.faceTypes, endProfileInfo.faceTypes]);
    const faceNames = mapArray(faceTypes, function(faceType)
        {
            return faceTypeToFaceTypeData[faceType].name;
        });

    const holeDef = holeDefinition(profiles, { "faceNames" : faceNames });

    const returnMapPerHole = callSubfeatureAndProcessStatus(topLevelId, opHole, context, opHoleId, mergeMaps({
                    "holeDefinition" : holeDef,
                    "axes" : axes,
                    "identities" : locations,
                    "targets" : definition.scope
                }, opHoleOverrides), {
                "featureParameterMap" : { "targets" : "scope" },
                "propagateErrorDisplay" : true
            });

    return {
            "finalPositionReference" : endProfileInfo.finalPositionReference,
            "holeDepth" : endProfileInfo.holeDepth,
            "faceTypes" : faceTypes,
            "returnMapPerHole" : returnMapPerHole
        };
}

// Occurs after the call to `opHole`.  When called, we are in a state where the hole tools still exist. The function is
// responsible for using the tools to cut into the sheet metal targets, consuming the tools in the process.  When
// finished, the tools should be gone, and the sheet metal should be cut, rebuilt, and attributed properly.
function handleSheetMetalCutAndAttribution(context is Context, topLevelId is Id, opHoleId is Id, definition is map,
    returnMapPerHole is array, sheetMetalTargetInfo is map, locations is array, successfulHoles is box)
{
    const nHoles = size(locations);
    var subtopologyTrackingPerTool = makeArray(nHoles);
    for (var i = 0; i < nHoles; i += 1)
    {
        if (returnMapPerHole[i].success)
        {
            subtopologyTrackingPerTool[i] = startTracking(context, {
                        "subquery" : qUnion([qOpHoleProfile(opHoleId, { "identity" : locations[i] }), qOpHoleFace(opHoleId, { "identity" : locations[i] })]),
                        "trackPartialDependency" : true
                    });
        }
        // otherwise just leave `undefined` in the array
    }

    const booleanId = topLevelId + "sheetMetalHoleCut";
    try
    {
        callSubfeatureAndProcessStatus(topLevelId, booleanBodies, context, booleanId, {
                    "targets" : definition.scope,
                    "tools" : qBodyType(qCreatedBy(opHoleId, EntityType.BODY), BodyType.SOLID),
                    "operationType" : BooleanOperationType.SUBTRACTION
                });
    }
    catch
    {
        // Convert error to HOLE_CUT_FAIL
        throwRegenErrorWithToolErrorEntities(context, topLevelId, definition, locations, ErrorStringEnum.HOLE_CUT_FAIL, ["scope"]);
    }

    const smHoleEdgeQueries = getSheetMetalHoleEdgeQueries(booleanId, sheetMetalTargetInfo.underlyingModels, true);
    for (var i = 0; i < nHoles; i += 1)
    {
        if (subtopologyTrackingPerTool[i] == undefined)
        {
            // `undefined` was left in the array earlier in the function because creation of the hole tool was unsuccessful
            continue;
        }

        const createdUsingNewHolePipeline = true;
        const instanceProducedEdges = assignSheetMetalHoleAttributesForInstance(context, createdUsingNewHolePipeline,
                buildHoleAttributeId(topLevelId, i), smHoleEdgeQueries, subtopologyTrackingPerTool[i], definition, i);
        if (instanceProducedEdges)
        {
            successfulHoles[][i] = true;
        }
    }
}

function adjustFeatureStatusAfterProducingHoles(context is Context, topLevelId is Id, definition is map,
    locations is array, successfulHoles is box)
{
    const featureInfo = getFeatureInfo(context, topLevelId);
    if (successfulHoles[] == {})
    {
        // If both opHole and handleSheetMetalCutAndAttribution did not cut anything, we will finish bearing an INFO
        // status; convert up to an ERROR.  We will not arrive here if there are only regular parts (no sheet metal),
        // because in that case keepTools of opHole will be set to false, allowing opHole to throw an ERROR instead of
        // an INFO.
        throwRegenErrorWithToolErrorEntities(context, topLevelId, definition, locations, ErrorStringEnum.HOLE_NO_HITS, ["scope"]);
    }
    else if (featureInfo == ErrorStringEnum.HOLE_NO_HITS || featureInfo == ErrorStringEnum.BOOLEAN_SUBTRACT_NO_OP)
    {
        // One of regular and sheet metal cut was a no-op, and the other was successful. Get rid of the INFO.
        clearFeatureStatus(context, topLevelId, {});
    }

    // Show an INFO and highlight unsuccessful locations if the hole has partially failed
    const nLocations = size(locations);
    if (size(successfulHoles[]) != nLocations)
    {
        var unsuccessfulLocations = [];
        for (var i = 0; i < nLocations; i += 1)
        {
            if (successfulHoles[][i] == undefined)
            {
                unsuccessfulLocations = append(unsuccessfulLocations, locations[i]);
            }
        }

        reportFeatureInfo(context, topLevelId, ErrorStringEnum.HOLE_PARTIAL_FAILURE);
        setErrorEntities(context, topLevelId, { "entities" : qUnion(unsuccessfulLocations) });
    }
}

// -- Profile names --

// The lip of a counterbore hole
const BEFORE_CBORE_PROFILE_NAME = "beforeCBore";
// The transition from cylinder to plane of the cbore
const CBORE_TRANSITION_PROFILE_NAME = "cboreTransition";
// The lip of a countersink hole
const BEFORE_CSINK_PROFILE_NAME = "beforeCSink";
// The transition from countersink undercut resolution cylinder to countersink cone. If there is no countersink
// undercut resolution cylinder, this profile is collapsed, and BEFORE_CSINK_PROFILE_NAME will remain as the
// lip profile before the countersink cone.
const CSINK_TRANSITION_PROFILE_NAME = "csinkTransition";
// The start of the hole shaft. For simple holes (without counterbore or countersink) this also represents the lip of the hole
const BEFORE_SHAFT_PROFILE_NAME = "beforeShaft";
// Blind in last matched profiles
const BLIND_IN_LAST_OUTER_MATCHED_PROFILE_NAME = "blindInLastOuterMatched";
const BLIND_IN_LAST_INNER_MATCHED_PROFILE_NAME = "blindInLastInnerMatched";
// Profile dividing the SHAFT or OFFSET_SHAFT face from the TIP face
const BEFORE_TIP_PROFILE_NAME = "beforeTip";
// Tip vertex
const TIP_PROFILE_NAME = "tip";

// -- Face data --

enum HoleFaceType
{
    // The top cap of the hole profile tool solid
    CAP,
    // Counterbore cylindrical bore face
    CBORE_CYLINDER_FACE,
    // Counterbore planar diameter face
    CBORE_PLANE_FACE,
    // Countersink cylindrical undercut resolution face
    CSINK_CYLINDER_FACE,
    // Countersink conical sink face
    CSINK_CONE_FACE,
    // Shaft face (excluding tap)
    SHAFT,
    // Blind in last matched face
    BLIND_IN_LAST_MATCHED,
    // Second shaft face, if blind in last requires an offset cylinder
    OFFSET_SHAFT,
    // Tip face
    TIP
}

// The pieces of data which do not contain `sectionFaceType` are not expected to create any faces on the target.
const faceTypeToFaceTypeData = {
        HoleFaceType.CAP : {
                "name" : "cap"
            },
        // COMMENT_FOR_REVIEW: Testing revealed I had these reversed.
        HoleFaceType.CBORE_CYLINDER_FACE : {
                "name" : "cboreCylinder",
                "sectionFaceType" : HoleSectionFaceType.CBORE_DIAMETER_FACE
            },
        HoleFaceType.CBORE_PLANE_FACE : {
                "name" : "cborePlane",
                "sectionFaceType" : HoleSectionFaceType.CBORE_DEPTH_FACE
            },
        HoleFaceType.CSINK_CYLINDER_FACE : {
                "name" : "csinkCylinder",
                "sectionFaceType" : HoleSectionFaceType.CSINK_CBORE_FACE
            },
        HoleFaceType.CSINK_CONE_FACE : {
                "name" : "csinkCone",
                "sectionFaceType" : HoleSectionFaceType.CSINK_FACE
            },
        HoleFaceType.SHAFT : {
                "name" : "shaft",
                "sectionFaceType" : HoleSectionFaceType.THROUGH_FACE
            },
        HoleFaceType.BLIND_IN_LAST_MATCHED : {
                "name" : "blindInLastMatched"
            },
        HoleFaceType.OFFSET_SHAFT : {
                "name" : "offsetShaft",
                "sectionFaceType" : HoleSectionFaceType.THROUGH_FACE
            },
        HoleFaceType.TIP : {
                "name" : "tip",
                "sectionFaceType" : HoleSectionFaceType.BLIND_TIP_FACE
            }
    };

// Face types that are not expected to create faces on the target are skipped, and not present in the returned mapping.
function buildFaceTypeToSectionFaceType(holeFaceTypes is array, holeStyle is HoleStyle) returns map
{
    var holeFaceTypeToSectionFaceType = {};
    for (var holeFaceType in holeFaceTypes)
    {
        var sectionFaceType = faceTypeToFaceTypeData[holeFaceType].sectionFaceType;
        if (sectionFaceType == undefined)
        {
            // Some hole face types are not expected to create any faces on the target, and therefore do not have a sectionFaceType
            continue;
        }

        if (!(sectionFaceType is HoleSectionFaceType))
        {
            // This cannot be hit as long as `sectionFaceType`s in `holeFaceTypeToHoleFaceTypeData` are well-formed.
            throw "Found invalid section face type " ~ sectionFaceType ~ " for " ~ holeFaceType ~ ", " ~ holeStyle;
        }
        holeFaceTypeToSectionFaceType[holeFaceType] = sectionFaceType;
    }
    return holeFaceTypeToSectionFaceType;
}

// Returns a map containing `profiles` and `faceTypes`
function computeStartProfiles(definition is map, firstPositionReference is HolePositionReference) returns map
{
    const shaftRadius = definition.holeDiameter / 2.0;

    if (definition.style == HoleStyle.SIMPLE)
    {
        return {
                "profiles" : [holeProfileBeforeReference(firstPositionReference, 0 * meter, shaftRadius, { "name" : BEFORE_SHAFT_PROFILE_NAME })],
                "faceTypes" : [HoleFaceType.CAP]
            };
    }
    else if (definition.style == HoleStyle.C_BORE)
    {
        const cBoreRadius = definition.cBoreDiameter / 2.0;
        return {
                "profiles" : [
                        holeProfileBeforeReference(firstPositionReference, 0 * meter, cBoreRadius, { "name" : BEFORE_CBORE_PROFILE_NAME }),
                        holeProfile(firstPositionReference, definition.cBoreDepth, cBoreRadius, { "name" : CBORE_TRANSITION_PROFILE_NAME }),
                        holeProfile(firstPositionReference, definition.cBoreDepth, shaftRadius, { "name" : BEFORE_SHAFT_PROFILE_NAME })
                    ],
                "faceTypes" : [HoleFaceType.CAP, HoleFaceType.CBORE_CYLINDER_FACE, HoleFaceType.CBORE_PLANE_FACE]
            };
    }
    else if (definition.style == HoleStyle.C_SINK)
    {
        const cSinkRadius = definition.cSinkDiameter / 2.0;
        const cSinkDepth = (cSinkRadius - shaftRadius) / tan(definition.cSinkAngle / 2.0);
        return {
                "profiles" : [
                        // When the entry face is flat, the second profile will be collapsed onto the first profile by
                        // opHole. When collapsing the earliest name in the list is kept, so BEFORE_CSINK_PROFILE_NAME will
                        // survive, with CSINK_TRANSITION_PROFILE_NAME skipped.
                        holeProfileBeforeReference(firstPositionReference, 0 * meter, cSinkRadius, { "name" : BEFORE_CSINK_PROFILE_NAME }),
                        holeProfile(firstPositionReference, 0 * meter, cSinkRadius, { "name" : CSINK_TRANSITION_PROFILE_NAME }),
                        holeProfile(firstPositionReference, cSinkDepth, shaftRadius, { "name" : BEFORE_SHAFT_PROFILE_NAME })
                    ],
                // If the second profile is consumed (as described above), the HoleFaceType.NEAR_SIDE_FEATURE_PRE_PRIMARY
                // is also consumed.
                "faceTypes" : [HoleFaceType.CAP, HoleFaceType.CSINK_CYLINDER_FACE, HoleFaceType.CSINK_CONE_FACE]
            };
    }
    else
    {
        throw "Unrecognized hole style: " ~ definition.style;
    }
}

// Returns a map containing `profiles` and `faceTypes`.  The map will also include a `holeDepth` if the hole is not a
// THROUGH hole; this value represents the depth (excluding the tip) the has been requested by the user, and is
// measured from the final HolePositionReference.
function computeEndProfiles(definition is map, firstPositionReference is HolePositionReference) returns map
{
    const shaftRadius = definition.holeDiameter / 2.0;

    var profiles = [];
    var faceTypes = [HoleFaceType.SHAFT]; // The face before the end profiles is always the shaft
    var finalPositionReference;
    var holeDepth = undefined;
    if (definition.endStyle == HoleEndStyle.THROUGH)
    {
        // Put the end of the hole slightly past the end of the last part, so that the exit edge references the shaft
        // instead of having a complicated interaction with the final edge
        const padding = 1000 * TOLERANCE.zeroLength * meter;
        profiles = [
                holeProfile(HolePositionReference.LAST_TARGET_END, padding, shaftRadius, { "name" : BEFORE_TIP_PROFILE_NAME }),
                holeProfile(HolePositionReference.LAST_TARGET_END, padding, 0 * meter, { "name" : TIP_PROFILE_NAME })
            ];
        faceTypes = append(faceTypes, HoleFaceType.TIP);
        finalPositionReference = HolePositionReference.LAST_TARGET_END;
    }
    else if (definition.endStyle == HoleEndStyle.BLIND)
    {
        const tipDepth = shaftRadius / tan(definition.tipAngle / 2.0);
        profiles = [
                holeProfile(firstPositionReference, definition.holeDepth, shaftRadius, { "name" : BEFORE_TIP_PROFILE_NAME }),
                holeProfile(firstPositionReference, definition.holeDepth + tipDepth, 0 * meter, { "name" : TIP_PROFILE_NAME })
            ];
        faceTypes = append(faceTypes, HoleFaceType.TIP);
        finalPositionReference = firstPositionReference;
        holeDepth = definition.holeDepth;
    }
    else if (definition.endStyle == HoleEndStyle.BLIND_IN_LAST)
    {
        const tapRadius = definition.tapDrillDiameter / 2;
        const tipDepth = tapRadius / tan(definition.tipAngle / 2.0);


        var targetMustDifferFromPreviousHasBeenSet = false;
        if (!tolerantEquals(tapRadius, shaftRadius))
        {
            // If tap and clearance radii differ, transition needs to be a face matched to the top of the
            // LAST_TARGET_START to avoid cutting into the last part incorrectly
            profiles = [
                    matchedHoleProfile(HolePositionReference.LAST_TARGET_START, shaftRadius, {
                                "name" : BLIND_IN_LAST_OUTER_MATCHED_PROFILE_NAME,
                                // Do not allow opHole to build any hole in which the "last target" is not distinct
                                "targetMustDifferFromPrevious" : true
                            }),
                    matchedHoleProfile(HolePositionReference.LAST_TARGET_START, tapRadius, { "name" : BLIND_IN_LAST_INNER_MATCHED_PROFILE_NAME })
                ];
            targetMustDifferFromPreviousHasBeenSet = true;
            faceTypes = concatenateArrays([faceTypes, [HoleFaceType.BLIND_IN_LAST_MATCHED, HoleFaceType.OFFSET_SHAFT]]);
        }

        profiles = concatenateArrays([profiles, [
                    holeProfile(HolePositionReference.LAST_TARGET_START, definition.holeDepth, tapRadius, {
                                "name" : BEFORE_TIP_PROFILE_NAME,
                                // Do not allow opHole to build any hole in which the "last target" is not distinct.
                                // Only set this if this is the first profile referencing LAST_TARGET_START; i.e. only
                                //  set if it was not already set above.
                                "targetMustDifferFromPrevious" : targetMustDifferFromPreviousHasBeenSet ? undefined : true
                            }),
                    holeProfile(HolePositionReference.LAST_TARGET_START, definition.holeDepth + tipDepth, 0 * meter, { "name" : TIP_PROFILE_NAME })
                ]]);
        faceTypes = append(faceTypes, HoleFaceType.TIP);
        finalPositionReference = HolePositionReference.LAST_TARGET_START;
        holeDepth = definition.holeDepth;
    }
    else
    {
        throw "Unrecognized hole end style: " ~ definition.endStyle;
    }

    return {
            "profiles" : profiles,
            "faceTypes" : faceTypes,
            "finalPositionReference" : finalPositionReference,
            "holeDepth" : holeDepth // may be undefined
        };
}

function throwRegenErrorWithToolErrorEntities(context is Context, topLevelId is Id, definition is map,
    locations is array, error is ErrorStringEnum, faultyParameters is array)
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1617_HOLE_NEW_PIPELINE_2))
    {
        displayToolErrorEntities(context, topLevelId, definition, locations);
    }
    else
    {
        displayToolErrorEntitiesDeprecated(context, topLevelId, definition, locations);
    }
    throw regenError(error, faultyParameters);
}

function displayToolErrorEntities(context is Context, topLevelId is Id, definition is map, locations is array)
{
    const errorId = topLevelId + "errorEntities";

    if (definition.endStyle != HoleEndStyle.BLIND)
    {
        const userStyle = definition.endStyle;

        // Switch to BLIND hole to ensure we do not need to cylinder cast against any targets
        definition.endStyle = HoleEndStyle.BLIND;

        // For through, pick a depth that is sufficiently far for error display. For blind in last, just keep the user
        // specified hole depth for the last part (which is already stored as `holeDepth`).
        if (userStyle == HoleEndStyle.THROUGH)
        {
            const targets = (isQueryEmpty(context, definition.scope)) ? qEverything(EntityType.BODY)->qBodyType(BodyType.SOLID) : definition.scope;
            var bbox = evBox3d(context, { "topology" : qUnion([targets, qUnion(locations)]) });
            // Ensure we do not hit an error if there are no solids in the part studio, and the user has only selected one location
            definition.holeDepth = max(box3dDiagonalLength(bbox), 1 * inch);
        }
    }

    // Start from sketch plane so that we do not need to cylinder cast for the starting position
    definition.startFromSketch = true;
    // Do not pass any targets to opHole, this call should be purely AXIS_POINT
    definition.scope = qNothing();

    startFeature(context, errorId);
    try
    {
        // Pass `errorId` as the topLevelId so that we do not process any statuses onto the real topLevelId
        buildOpHoleDefinitionAndCallOpHole(context, errorId, errorId + "opHole", definition, locations, {
                    // Do not attempt any subtraction and keep the tools for error display
                    "subtractFromTargets" : false,
                    "keepTools" : true
                });
        const errorBodyQuery = qCreatedBy(errorId, EntityType.BODY);
        setErrorEntities(context, topLevelId, { "entities" : errorBodyQuery });
    }
    abortFeature(context, errorId);
}

function produceHolesDeprecated(context is Context, topLevelId is Id, definition is map, locations is array)
{

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
        displayToolErrorEntitiesDeprecated(context, topLevelId, definition, locations);
    }
}

function displayToolErrorEntitiesDeprecated(context is Context, topLevelId is Id, definition is map, locations is array)
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

function holeOp(context is Context, topLevelId is Id, locations is array, definition is map) returns map
{
    var result = { "numSuccess" : 0 };
    // for each hole
    var holeNumber = -1;
    definition.cutOption = getCutOption(context, definition);
    var holeNumberToResult = {};
    const holeBodiesId = isAtVersionOrLater(context, FeatureScriptVersionNumber.V763_HOLE_CUT_ALL) ? topLevelId + "holeBodies" : topLevelId;

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
        const booleanId = topLevelId + "boolean";
        try
        {
            booleanBodies(context, booleanId, { "targets" : definition.scope,
                        "tools" : qBodyType(qCreatedBy(holeBodiesId, EntityType.BODY), BodyType.SOLID),
                        "operationType" : BooleanOperationType.SUBTRACTION
                    });
        }
        processSubfeatureStatus(context, topLevelId, { "subfeatureId" : booleanId,
                    "propagateErrorDisplay" : true,
                    "featureParameterMap" : { "tools" : "locations", "targets" : "scope" }
                });
        if (hasErrors(context, topLevelId))
        {
            result.numSuccess = 0;
            result.error = ErrorStringEnum.HOLE_CUT_FAIL;
        }
        else
        {
            var sheetMetalModels = getSheetMetalModelsDeprecated(context, definition);
            var smHoleEdgeQueries;
            if (sheetMetalModels != undefined)
            {
                smHoleEdgeQueries = getSheetMetalHoleEdgeQueries(booleanId, sheetMetalModels, true);
            }
            for (var holeNumber, holeResult in holeNumberToResult)
            {
                const attributeId = buildHoleAttributeId(topLevelId, holeNumber);
                createAttributesFromTracking(context, attributeId, definition, holeNumber, holeResult.faceTracking,
                    holeResult.startDistances, holeResult.holeDepth);
                if (sheetMetalModels != undefined && holeResult.instanceTracking != undefined)
                {
                    const createdUsingNewHolePipeline = false;
                    assignSheetMetalHoleAttributesForInstance(context, createdUsingNewHolePipeline, attributeId,
                        smHoleEdgeQueries, holeResult.instanceTracking, definition, holeNumber);
                }
            }
        }
    }
    return result;
}

function computeAxes(context is Context, locations is array, oppositeDirection is boolean, xform is Transform) returns array
{
    const sign = oppositeDirection ? 1 : -1;
    return mapArray(locations, function(location)
        {
            // This handles both sketch points and mate connectors
            var axis = evAxis(context, {
                    "axis" : location,
                    "allowSketchPoints" : true
                });
            axis.direction *= sign;
            return (xform * axis);
        });
}

function computeCSysDeprecated(context is Context, location is Query, definition is map) returns CoordSystem
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
    const startPointCSys = computeCSysDeprecated(context, location, definition);

    const holeIdExtension = buildHoleIdExtension(holeNumber);
    const useUnstableComponent = isAtVersionOrLater(context, FeatureScriptVersionNumber.V960_HOLE_IDENTITY);
    const holeId = id + ((useUnstableComponent) ? unstableIdComponent(holeIdExtension) : holeIdExtension);

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
            (degree) : [0.1, 90, 179.9],
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

function getSheetMetalTargetInfo(context is Context, featureDefinition is map)
{
    const separatedQueries = separateSheetMetalQueries(context, featureDefinition.scope);
    const sheetMetalTargets = separatedQueries.sheetMetalQueries;
    const hasSheetMetalTargets = !isQueryEmpty(context, sheetMetalTargets);

    return {
            "hasSheetMetalTargets" : hasSheetMetalTargets,
            "hasNonSheetMetalTargets" : !isQueryEmpty(context, separatedQueries.nonSheetMetalQueries),
            "sheetMetalTargets" : hasSheetMetalTargets ? makeRobustQuery(context, sheetMetalTargets) : undefined,
            "underlyingModels" : hasSheetMetalTargets ? makeRobustQuery(context, getSheetMetalModelForPart(context, sheetMetalTargets)) : undefined
        };
}

function getSheetMetalModelsDeprecated(context is Context, holeDefinition is map)
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
        sheetmetalModels = getSheetMetalModelsDeprecated(context, holeDefinition);
    }

    var result = {};
    var frontDist = 0 * meter;

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

    //Using common start prefix ensures intersection edge reference stability towards hole type  change.
    const useCommonPrefix = isAtVersionOrLater(context, FeatureScriptVersionNumber.V960_HOLE_IDENTITY);
    const commonStartPrefix = "start";
    if (holeDefinition.style == HoleStyle.C_BORE)
    {
        cboreTrackingSpecs = sketchCBore(context, {
                    "prefix" : (useCommonPrefix) ? commonStartPrefix : "cbore_start",
                    "sketch" : sketch,
                    "startDepth" : startDepth,
                    "endDepth" : frontDist + holeDefinition.cBoreDepth,
                    "cBoreDiameter" : holeDefinition.cBoreDiameter });

        startDepth = frontDist;
        frontDist = 0 * meter;
    }
    else if (holeDefinition.style == HoleStyle.C_SINK)
    {
        csinkTrackingSpecs = sketchCSink(context, {
                    "prefix" : (useCommonPrefix) ? commonStartPrefix : "csink_start",
                    "sketch" : sketch,
                    "isPositive" : true,
                    "startDepth" : startDepth,
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
            // Legacy: use id directly as attribute id rather than `buildHoleAttributeId`
            const attributeId = toAttributeId(id);

            // add required attributes onto faces that were created based upon our tracked sketch entities
            createAttributesFromTracking(context, attributeId, holeDefinition, holeNumber, faceTracking,
                startDistances.resultFront, coreResult.holeDepth);

            if (sheetmetalModels != undefined)
            {
                const createdUsingNewHolePipeline = false;
                const holeEdgesQ = getSheetMetalHoleEdgeQueries(id, sheetmetalModels, false).circularEdges;
                assignSheetMetalHoleAttributes(context, createdUsingNewHolePipeline, attributeId, holeEdgesQ,
                    holeDefinition, holeNumber);
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

function getSheetMetalHoleEdgeQueries(id is Id, sheetMetalModels is Query, includeArcs is boolean) returns map
{
    // Now we look for edges affected by the sheet metal model
    const createdEdges = qBodyType(qCreatedBy(id, EntityType.EDGE), BodyType.SHEET);
    const smEdges = qOwnedByBody(createdEdges, sheetMetalModels);
    var circularEdges;
    if (includeArcs)
    {
        circularEdges = qUnion([qGeometry(smEdges, GeometryType.CIRCLE), qGeometry(smEdges, GeometryType.ARC)]);
    }
    else
    {
        circularEdges = qGeometry(smEdges, GeometryType.CIRCLE);
    }

    return {
            "allEdges" : smEdges,
            "circularEdges" : circularEdges
        };
}

// Returns whether the hole instance produced any edges
function assignSheetMetalHoleAttributesForInstance(context is Context, createdUsingNewHolePipeline is boolean,
    attributeId is string, smHoleEdgeQueries is map, instanceTopology is Query, featureDefinition is map,
    holeNumber is number) returns boolean
{
    // When doing the sheet metal boolean as a batch subtraction, we have access to `allHoleEdges` (all the edges created by
    // all the holes on the underlying sheet metal models) and `instanceTopology` (all the topology created by this single
    // hole instance). Intersecting these queries gives us the underlying hole edges for just this specific hole instance.
    const circularHoleEdgesForInstance = qIntersection([smHoleEdgeQueries.circularEdges, instanceTopology]);
    assignSheetMetalHoleAttributes(context, createdUsingNewHolePipeline, attributeId, circularHoleEdgesForInstance,
        featureDefinition, holeNumber);

    const allEdgesForInstance = qIntersection([smHoleEdgeQueries.allEdges, instanceTopology]);
    return !isQueryEmpty(context, allEdgesForInstance);
}

function assignSheetMetalHoleAttributes(context is Context, createdUsingNewHolePipeline is boolean,
    attributeId is string, circularHoleEdges is Query, featureDefinition is map, holeNumber is number)
{
    const circularHoleEdgesEvaluated = evaluateQuery(context, circularHoleEdges);
    if (circularHoleEdgesEvaluated == [])
    {
        return;
    }

    for (var circularHoleEdge in circularHoleEdgesEvaluated)
    {
        var associations = getSMAssociationAttributes(context, circularHoleEdge);
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
                createAttributesForSheetMetalHole(context, createdUsingNewHolePipeline, attributeId, circularHoleEdge,
                    holeFacesQ, featureDefinition, holeNumber);
            }
        }
    }
}

function createAttributesForSheetMetalHole(context is Context, createdUsingNewHolePipeline is boolean,
    attributeId is string, holeEdge is Query, holeFaces is Query, featureDefinition is map, holeNumber is number)
{
    const tappedFixes = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1743_SM_BLIND_IN_LAST_HOLE);

    clearHoleAttributes(context, holeFaces);
    var holeAttribute;
    var cylinder = evSurfaceDefinition(context, {
            "face" : holeFaces
        });
    if (cylinder is Cylinder)
    {
        // Sheet metal holes are always simple and through
        const diameter = cylinder.radius * 2;
        var isLastTarget = false;
        // If the cylinder is not exactly the same as the definition diameter, make sure the definition is used (BEL-152781)
        if (!tolerantEquals(featureDefinition.holeDiameter, diameter))
        {
            featureDefinition.holeDiameter = diameter;
            if (featureDefinition.style == HoleStyle.C_BORE && tolerantEquals(featureDefinition.cBoreDiameter, diameter))
            {
                // Set to exactly the cBoreDiameter
                featureDefinition.holeDiameter = featureDefinition.cBoreDiameter;
            }
            else if (featureDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST && tolerantEquals(featureDefinition.tapDrillDiameter, diameter))
            {
                isLastTarget = true;
                if (tappedFixes)
                {
                    // Set to exactly tapDrillDiameter
                    featureDefinition.holeDiameter = featureDefinition.tapDrillDiameter;
                }
            }
        }
        featureDefinition.style = HoleStyle.SIMPLE;
        if (tappedFixes && featureDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST)
        {
            // We are going to use THROUGH instead of BLIND_IN_LAST as the endStyle, so migrate the appropriate data to
            // standardTappedOrClearance depending on whether this is the last target (the tapped target) or not.
            featureDefinition.standardTappedOrClearance = isLastTarget ? featureDefinition.standardBlindInLast : undefined;
        }
        featureDefinition.endStyle = HoleEndStyle.THROUGH;
        if (tappedFixes)
        {
            // In addition to endStyle THROUGH, we need to tell the hole attribute that the tap goes all the way through.
            featureDefinition.isTappedThrough = true;
        }
        holeAttribute = createHoleAttribute(createdUsingNewHolePipeline, attributeId, featureDefinition,
            HoleSectionFaceType.THROUGH_FACE, holeNumber);
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
        const resultFront = arg.startDistances.resultFront;

        var lastBody;
        var lastBodyStartDist;
        if (resultFront != undefined && resultFront is array && isAtVersionOrLater(context, FeatureScriptVersionNumber.V362_HOLE_IMPROVED_DEPTH_FINDING))
        {
            if (arg.holeDefinition.generateErrorBodies)
            {
                lastBodyStartDist = 0 * meter;
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
                const lastResult = resultFront[size(resultFront) - 1];
                lastBody = lastResult.target;
                lastBodyStartDist = lastResult.distance;
            }
        }
        else
        {
            // Find shoulder depth
            try
            {
                const distance = isAtVersionOrLater(context, FeatureScriptVersionNumber.V299_HOLE_FEATURE_FIX_BLIND_IN_LAST_FLIP)
                    ? 0 * meter : arg.holeDefinition.scopeSize * 2;
                const castResult = cylinderCast(context, id + "foo" + "limit_surf_cast", {
                            "distance" : distance,
                            "cSys" : arg.cSys,
                            "isFront" : true,
                            "findClosest" : false,
                            "diameter" : 2 * radius,
                            "scope" : arg.holeDefinition.scope });
                if (castResult.distance != undefined)
                {
                    lastBody = castResult.target;
                    lastBodyStartDist = castResult.distance;
                }
            }
        }

        if (lastBodyStartDist != undefined)
        {
            if (lastBodyStartDist < 0 && isAtVersionOrLater(context, FeatureScriptVersionNumber.V291_HOLE_FEATURE_STANDARD_DEFAULTS))
            {
                lastBodyStartDist = 0 * meter;
            }
            result.holeDepth = lastBodyStartDist + depth;
            depth += lastBodyStartDist - arg.startDepth - arg.clearanceDepth;
            if (arg.holeDefinition.holeDiameter > arg.holeDefinition.tapDrillDiameter + TOLERANCE.zeroLength * meter)
            {
                const delta = (arg.holeDefinition.holeDiameter - arg.holeDefinition.tapDrillDiameter) / 2 / tan(tipAngle / 2);
                depth -= delta;
            }

            result.lastBodyStartDist = lastBodyStartDist;
        }
        if (lastBody != undefined)
        {
            result.lastBody = lastBody;
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
    return box3dDiagonalLength(scopeBox);
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

// Create attributes for a single hole created using opHole.  `userDefinedHoleDepth` can be undefined for THROUGH holes.
// Returns whether any faces were created by this hole.
function createAttributesFromQuery(context is Context, topLevelId is Id, opHoleId is Id, featureDefinition is map,
    finalPositionReference is HolePositionReference, attributeId is string, faceTypeToSectionFaceType is map,
    holeIdentity is Query, singleHoleReturnValue is map, holeIndex is number, userDefinedHoleDepth) returns boolean
{
    if (isQueryEmpty(context, qOpHoleFace(opHoleId, { "identity" : holeIdentity })))
    {
        return false;
    }

    // Split by part
    for (var target in evaluateQuery(context, qOwnerBody(qOpHoleFace(opHoleId, { "identity" : holeIdentity }))))
    {
        var faceTypes = {};
        var sectionFaceTypes = {};
        var faceToSectionFaceType = {};
        for (var faceTypeWrapper in HoleFaceType)
        {
            const faceType = faceTypeWrapper.value;
            // May be undefined if we do not care about this face for the purpose of hole attribution.  For example, CAP faces.
            const sectionFaceType = faceTypeToSectionFaceType[faceType];

            const faces = evaluateQuery(context, qOpHoleFace(opHoleId, { "name" : faceTypeToFaceTypeData[faceType].name, "identity" : holeIdentity })->qOwnedByBody(target));
            if (faces != [])
            {
                faceTypes[faceType] = true;
                if (sectionFaceType != undefined)
                {
                    sectionFaceTypes[sectionFaceType] = true;
                    for (var face in faces)
                    {
                        faceToSectionFaceType[face] = sectionFaceType;
                    }
                }
            }
        }

        const finalPositionReferenceInfo = singleHoleReturnValue.positionReferenceInfo[finalPositionReference];
        const depthExtremes = singleHoleReturnValue.targetToDepthExtremes[target];
        const fullEntranceInFinalPositionReferenceSpace = depthExtremes.fullEntrance - finalPositionReferenceInfo.referenceRootEnd;
        const fullExitInFinalPositionReferenceSpace = depthExtremes.fullExit - finalPositionReferenceInfo.referenceRootEnd;


        var depthInPart;
        if (userDefinedHoleDepth != undefined)
        {
            depthInPart = userDefinedHoleDepth - fullEntranceInFinalPositionReferenceSpace;
        }
        var isLastTarget = false;
        if (singleHoleReturnValue.positionReferenceInfo[HolePositionReference.LAST_TARGET_START] != undefined)
        {
            isLastTarget = (target == singleHoleReturnValue.positionReferenceInfo[HolePositionReference.LAST_TARGET_START].target);
        }
        var featureDefinitionForAttribute = adjustDefinitionForAttribute(featureDefinition, sectionFaceTypes, isLastTarget, depthInPart);

        // Adjust `partialThrough`
        if (featureDefinitionForAttribute.endStyle == HoleEndStyle.THROUGH)
        {
            if (faceTypes[HoleFaceType.CAP] == true)
            {
                // If cap faces are present, the hole may be starting inside a part, and should not be considered a full through
                featureDefinitionForAttribute.partialThrough = true;
            }
            else if (userDefinedHoleDepth != undefined)
            {
                // Ensure that the hole goes all the way to the full exit, not just through a portion of the target
                featureDefinitionForAttribute.partialThrough = userDefinedHoleDepth < (fullExitInFinalPositionReferenceSpace - (TOLERANCE.zeroLength * meter));
            }
            else
            {
                // No user defined depth, meaning that user has selected THROUGH in the dialog.  This is guaranteed to go all the way through
                featureDefinitionForAttribute.partialThrough = false;
            }
        }

        for (var faceAndSectionFaceType in faceToSectionFaceType)
        {
            const face = faceAndSectionFaceType.key;
            clearHoleAttributes(context, face);
            const createdUsingNewHolePipeline = true;
            var holeAttribute = createHoleAttribute(createdUsingNewHolePipeline, attributeId,
                featureDefinitionForAttribute, faceAndSectionFaceType.value, holeIndex);
            if (holeAttribute != undefined)
            {
                // Adjust `isTappedThrough`
                if (holeAttribute.isTappedHole == true && featureDefinition.endStyle != HoleEndStyle.THROUGH) // If the hole style is thorugh, isTappedThrough is set explicitly
                {
                    const userDefinedTappedDepth = featureDefinition.tappedDepth;
                    holeAttribute.isTappedThrough = userDefinedTappedDepth > (fullExitInFinalPositionReferenceSpace - (TOLERANCE.zeroLength * meter));
                }

                setAttribute(context, { "entities" : face, "attribute" : holeAttribute });
            }
        }
    }

    return true;
}

function createAttributesFromTracking(context is Context, attributeId is string, holeDefinition is map,
    holeNumber is number, sketchTracking is array, startDistances is array, holeDepth)
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
        var sectionFaceTypes = {};
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
                sectionFaceTypes[track.sectionType] = true;
            }
        }

        var depthInPart;
        if (holeDepth != undefined)
        {
            var distance = partToStartDistance[part];
            if (distance != undefined)
            {
                depthInPart = holeDepth - distance;
            }
        }

        var holeDefinitionForAttribute = adjustDefinitionForAttribute(holeDefinition, sectionFaceTypes, part == lastBody, depthInPart);

        // Adjust `partialThrough`
        if (holeDefinitionForAttribute.endStyle == HoleEndStyle.THROUGH)
        {
            try
            {
                // Check if going through only a subset of the part
                var holeAxis = computeHoleAxis(context, qUnion(allFaces));
                var distance = evDistance(context, { side0 : part, side1 : holeAxis }).distance;
                holeDefinitionForAttribute.partialThrough = distance * 2 < holeDefinitionForAttribute.holeDiameter - TOLERANCE.zeroLength * meter;
            }
        }

        var actualHoleDepth;
        for (var entry in entityToSectionType)
        {
            clearHoleAttributes(context, entry.key);
            const createdUsingNewHolePipeline = false;
            var holeAttribute = createHoleAttribute(createdUsingNewHolePipeline, attributeId,
            holeDefinitionForAttribute, entry.value, holeNumber);
            if (holeAttribute != undefined)
            {
                // Adjust `isTappedThrough`
                if (holeAttribute.isTappedHole == true && holeDefinition.endStyle != HoleEndStyle.THROUGH) // If the hole style is thorugh, isTappedThrough is set explicitly
                {
                    try // This shouldn't fail, but might for some reason on a legacy hole.  Don't break the feature in that case
                    {
                        if (actualHoleDepth == undefined)
                            actualHoleDepth = computeActualHoleDepth(context, qUnion(allFaces));
                        holeAttribute.isTappedThrough = (holeDefinitionForAttribute.tappedDepth + TOLERANCE.zeroLength * meter) > actualHoleDepth;
                    }
                }

                setAttribute(context, { "entities" : entry.key, "attribute" : holeAttribute });
            }
        }
    }
}

// This function does not handle `partialThrough` or `isTappedThrough`.  Caller should follow up with adjustments to those parameters.
function adjustDefinitionForAttribute(featureDefinition is map, sectionFaceTypes is map, isLastTarget is boolean, depthInPart) returns map
{
    var modifiedFeatureDefinition = featureDefinition;
    // Remove countersink and counterbore if necessary
    if (featureDefinition.style == HoleStyle.C_SINK)
    {
        if (sectionFaceTypes[HoleSectionFaceType.CSINK_FACE] == undefined && sectionFaceTypes[HoleSectionFaceType.CSINK_CBORE_FACE] == undefined)
        {
            modifiedFeatureDefinition.style = HoleStyle.SIMPLE;
        }
    }
    else if (featureDefinition.style == HoleStyle.C_BORE)
    {
        if (sectionFaceTypes[HoleSectionFaceType.CBORE_DIAMETER_FACE] == undefined && sectionFaceTypes[HoleSectionFaceType.CBORE_DEPTH_FACE] == undefined)
        {
            modifiedFeatureDefinition.style = HoleStyle.SIMPLE;
        }
    }
    // Check if this is a thru hole -- it is if there are no tip faces
    if (sectionFaceTypes[HoleSectionFaceType.BLIND_TIP_FACE] == undefined)
    {
        modifiedFeatureDefinition.endStyle = HoleEndStyle.THROUGH;
    }
    else if (depthInPart != undefined) // Adjust the depth based on startDistances
    {
        modifiedFeatureDefinition.holeDepth = depthInPart;
        if (featureDefinition.tappedDepth != undefined)
            modifiedFeatureDefinition.tappedDepth = featureDefinition.tappedDepth + modifiedFeatureDefinition.holeDepth - featureDefinition.holeDepth;
    }
    if (featureDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST)
    {
        // For a blind-in-last hole, use the blind-in-last standard to determine tap even if modified hole end style is through
        modifiedFeatureDefinition.standardTappedOrClearance = featureDefinition.standardBlindInLast;
    }
    if (!isLastTarget && featureDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST)
    {
        modifiedFeatureDefinition.standardTappedOrClearance = undefined;
    }
    if (isLastTarget && tappedHoleWithOffset(featureDefinition)) // If this is a tapped hole, adjust diameter
    {
        modifiedFeatureDefinition.holeDiameter = featureDefinition.tapDrillDiameter;
    }
    return modifiedFeatureDefinition;
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

const HOLE_ID_EXTENSION_PREFIX = "hole-";

function buildHoleIdExtension(holeIndex is number) returns string
{
    return HOLE_ID_EXTENSION_PREFIX ~ holeIndex;
}

function buildHoleAttributeId(topLevelId is Id, holeIndex is number) returns string
{
    return toAttributeId(topLevelId + buildHoleIdExtension(holeIndex));
}

/*
 * !!!!Attention developers! If a change is made to content of hole attributes corresponding changes should be made to
 * SBTHoleAttributeSpec.java and BTHoleUtilities.cpp
 */
function createHoleAttribute(createdUsingNewHolePipeline is boolean, attributeId is string, holeDefinition is map,
    holeFaceType is HoleSectionFaceType, holeNumber is number) returns HoleAttribute
{
    // make the base hole attribute
    var holeAttribute = makeHoleAttribute(createdUsingNewHolePipeline, attributeId, holeDefinition.style);

    // add tag info
    holeAttribute.holeNumber = holeNumber;
    holeAttribute.holeFeatureCount = holeDefinition.holeFeatureCount;

    // add common properties
    holeAttribute = addCommonAttributeProperties(holeAttribute, holeDefinition);

    // add properties specific to the section (for example, properties needed for the cBore diameter if this is the cBore diameter section)
    holeAttribute = addSectionSpecsToAttribute(holeAttribute, holeFaceType, holeDefinition);

    return holeAttribute;
}

function addCommonAttributeProperties(attribute is HoleAttribute, holeDefinition is map) returns HoleAttribute
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
    if (holeDefinition.style == HoleStyle.SIMPLE)
    {
        resultAttribute = addSimpleHoleAttributeProperties(resultAttribute, holeDefinition);
    }
    else if (holeDefinition.style == HoleStyle.C_BORE)
    {
        resultAttribute = addCBoreHoleAttributeProperties(resultAttribute, holeDefinition);
    }
    else if (holeDefinition.style == HoleStyle.C_SINK)
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

    if (isCreating)
    {
        definition = holeScopeFlipHeuristicsCall(context, oldDefinition, definition, specifiedParameters, hiddenBodies);
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
export function holeScopeFlipHeuristicsCall(context is Context, oldDefinition is map, definition is map, specifiedParameters is map, hiddenBodies is Query)
{
    if (oldDefinition.locations == undefined)
    {
        // If this editing logic is running as the feature is first opening, there may be some selections in
        // definition.locations due to a preselection, but the oldDefinition is completely blank.  Massage the
        // oldDefinition a bit to make it easier to work with.
        oldDefinition.locations = qNothing();
    }

    const locationsHaveChanged = (definition.locations != oldDefinition.locations);

    // Raycast inputs represents the set of parameters required for deciding which targets should be included in the
    // scope, and which direction the flip should be set to.  If the raycast inputs change, we must recalculate the
    // scope and flip from scratch, because the existing scope and flip may be entirely wrong. Some examples are:
    // changing the depth of a blind hole may add or remove some targets from the scope, switching from blind to
    // through may add further targets, flipping the oppositeDirection flipper could change the scope to an entirely
    // different set of bodies, or changing the scope manually could change the correct choice of oppositeDirection.
    const raycastInputs = extractRaycastInputs(definition);
    const oldRaycastInputs = extractRaycastInputs(oldDefinition);
    const raycastInputsHaveChanged = !raycastInputs.isEquivalent(context, raycastInputs, oldRaycastInputs);

    if (!locationsHaveChanged && !raycastInputsHaveChanged)
    {
        return definition;
    }

    // If the change that is being made is to incrementally add locations, just do a basic calculation to add necessary
    // targets to the scope.  Otherwise, fully recalculate the scope and flip.
    const allLocationsAreNew = (isQueryEmpty(context, qIntersection([oldDefinition.locations, definition.locations])));
    const comprehensive = raycastInputsHaveChanged || allLocationsAreNew;

    const canEditScope = !specifiedParameters.scope;

    // If editing incrementally, we will only be raycasting for the new locations, so we will not have enough
    // information to make a decision about the flip. Only attempt to edit the flip when running comprehensively.
    const canEditFlip = (!specifiedParameters.oppositeDirection && comprehensive);

    const noLocations = (isQueryEmpty(context, definition.locations));
    if (noLocations || (!canEditScope && !canEditFlip))
    {
        // If scope is not set and we have no locations, reset scope to empty.
        if (canEditScope && noLocations)
        {
            definition.scope = qNothing();
        }

        return definition;
    }

    const newLocations = comprehensive ? definition.locations : qSubtraction(definition.locations, oldDefinition.locations);
    const newAxes = computeAxes(context, evaluateQuery(context, newLocations), definition.oppositeDirection, identityTransform());

    // -- CAUTION: `definition` should not be passed into `raycastForScopeFlipResults`. Creating the barrier of
    // -- `raycastInputs` allows for an abstraction where any `definition` inputs needed for ray casting are extracted
    // -- in `extractRaycastInputs`, which exposes an `isEquivalent` function that allows us to see whether any of the
    // -- raycast inputs are changing (and do a comprehensive heuristic if so). If any additional `definition`
    // -- parameters are needed for `raycastForScopeFlipResults` they should be extracted in `extractRaycastInputs`
    // -- and added to `isEquivalent`.
    const scopeFlipResults = raycastForScopeFlipResults(context, raycastInputs, newAxes,
            comprehensive, canEditScope, canEditFlip, hiddenBodies);
    definition.scope = scopeFlipResults.scope;
    definition.oppositeDirection = scopeFlipResults.oppositeDirection;
    return definition;
}

function extractRaycastInputs(definition is map)
{
    // THROUGH and BLIND_IN_LAST can each go infinitely far.  BLIND has a limit to how far it can go.
    const hasDepthLimit = (definition.endStyle == HoleEndStyle.BLIND);
    return {
            "oppositeDirection" : definition.oppositeDirection,
            "hasDepthLimit" : hasDepthLimit,
            "depthLimit" : (hasDepthLimit ? definition.holeDepth : undefined),
            "scope" : definition.scope,
            // `isEquivalent` is used to tell whether we need to do a comprehensive heuristic over all of the
            // locations, or an incremental heuristic over just the new locations. If adding additional parameters to
            // this map, the appropriate comparison should be added to `isEquivalent`.
            "isEquivalent" :
                function(context, self, other)
                {
                    return self.oppositeDirection == other.oppositeDirection
                        && self.hasDepthLimit == other.hasDepthLimit
                        && (!self.hasDepthLimit || tolerantEquals(self.depthLimit, other.depthLimit))
                        && areQueriesEquivalent(context, self.scope, other.scope);
                }
        };
}

function raycastForScopeFlipResults(context is Context, raycastInputs is map, axes is array,
    comprehensive is boolean, canEditScope is boolean, canEditFlip is boolean, hiddenBodies is Query)
{
    // If we are just adding locations incrementally, we should just be adding to the existing scope.  If running comprehensively,
    // we should be rebuilding the scope from empty.
    const baseScope = comprehensive ? qNothing() : raycastInputs.scope;

    var possibleTargets;
    if (canEditScope)
    {
        possibleTargets = qAllModifiableSolidBodies()->qSubtraction(hiddenBodies)->qSubtraction(baseScope);
    }
    else
    {
        // We can't change the scope, but we can change the flip. Check all existing targets to discern correct flip.
        possibleTargets = raycastInputs.scope;
    }

    // Use a set to avoid calling `evaluateQuery` inside the per-axis loop. If there are a large number of possible targets,
    // the unpacking of transient ids into transient queries after calling @evaluateQuery can be a bottleneck.
    var remainingPossibleTargetSet = {};
    for (var possibleTarget in evaluateQuery(context, possibleTargets))
    {
        remainingPossibleTargetSet[possibleTarget] = true;
    }

    var targetToTargetLocation = {};
    for (var axis in axes)
    {
        if (remainingPossibleTargetSet == {})
        {
            // All possible targets have been consumed
            break;
        }
        const targetToTargetLocationForAxis = raycastForViableTargets(context, raycastInputs, axis, qUnion(keys(remainingPossibleTargetSet)));
        for (var targetAndTargetLocation in targetToTargetLocationForAxis)
        {
            const target = targetAndTargetLocation.key;
            if (targetToTargetLocation[target] == undefined)
            {
                targetToTargetLocation[target] = targetAndTargetLocation.value;
            }
            else if (targetToTargetLocation[target] != targetAndTargetLocation.value)
            {
                targetToTargetLocation[target] = TargetLocationRelativeToAxisPoint.AMBIGUOUS;
            }

            // Once the target is found to be both IN_FRONT and BEHIND, we can optimize by taking it out of the raycast
            // pool, since there is no more information we can get for that target.
            var shouldSkipTarget = targetToTargetLocation[target] == TargetLocationRelativeToAxisPoint.AMBIGUOUS;
            if (!canEditFlip)
            {
                // If we can't edit the flip, then we will be taking all IN_FRONT and AMBIGUOUS targets into the scope;
                // there is no point continuing to cast against this target just to try to upgrade it from IN_FRONT to
                // AMBIGUOUS, since both will have the same effect later in the function.
                shouldSkipTarget = shouldSkipTarget || (targetToTargetLocation[target] == TargetLocationRelativeToAxisPoint.IN_FRONT);
            }

            if (shouldSkipTarget)
            {
                remainingPossibleTargetSet[target] = undefined;
            }
        }
    }

    var scopeAndFlip = {
        "scope" : raycastInputs.scope,
        "oppositeDirection" : raycastInputs.oppositeDirection
    };

    var targetLocationToSkip = TargetLocationRelativeToAxisPoint.BEHIND;
    if (canEditFlip)
    {
        var counts = {
            TargetLocationRelativeToAxisPoint.IN_FRONT : 0,
            TargetLocationRelativeToAxisPoint.BEHIND : 0,
            TargetLocationRelativeToAxisPoint.AMBIGUOUS : 0
        };
        for (var targetAndTargetLocation in targetToTargetLocation)
        {
            counts[targetAndTargetLocation.value] += 1;
        }

        if (counts[TargetLocationRelativeToAxisPoint.BEHIND] > counts[TargetLocationRelativeToAxisPoint.IN_FRONT])
        {
            scopeAndFlip.oppositeDirection = !scopeAndFlip.oppositeDirection;
            targetLocationToSkip = TargetLocationRelativeToAxisPoint.IN_FRONT;
        }
    }

    if (canEditScope)
    {
        var toAdd = [];
        for (var targetAndTargetLocation in targetToTargetLocation)
        {
            if (targetAndTargetLocation.value != targetLocationToSkip)
            {
                toAdd = append(toAdd, targetAndTargetLocation.key);
            }
        }
        scopeAndFlip.scope = qUnion([baseScope, qUnion(toAdd)]);
    }

    return scopeAndFlip;
}

enum TargetLocationRelativeToAxisPoint
{
    IN_FRONT,
    BEHIND,
    AMBIGUOUS
}

predicate intersectionIsTooFar(raycastInputs is map, intersectionDistance is ValueWithUnits)
{
    raycastInputs.hasDepthLimit; // If not, the intersection cannot be too far
    intersectionDistance > (raycastInputs.depthLimit + (TOLERANCE.zeroLength * meter));
}


// Return a map from target query to TargetLocationRelativeToAxisPoint for all targets that are intersected
// by a raycast, and are close enough to intersect (if endStyle is BLIND)
function raycastForViableTargets(context is Context, raycastInputs is map, axis is Line, possibleTargets is Query)
{
    const raycastResults = evRaycast(context, {
                "ray" : axis,
                "entities" : possibleTargets,
                "closest" : false,
                "includeIntersectionsBehind" : true
            });
    var targetInfo = {};
    for (var raycastResult in raycastResults)
    {
        // Often hole locations are sketched right on the face they are going to cut. When this happens, we will get
        // a result with a distance of 0, and another result with either a positive or negative distance (where the ray
        // exits the part). Process `isCloseEnough` and `targetDirection` separately, because the former result will
        // tell us that the body is close enough (but not be able give us any info about whether the body is in front
        // of or behind the axis point), and the latter result will tell us the TargetDirection (even if the latter is
        // technically "too far" for the blind to reach)

        const body = evaluateQuery(context, qOwnerBody(raycastResult.entity))[0];
        const isCloseEnough = !intersectionIsTooFar(raycastInputs, raycastResult.distance);
        if (targetInfo[body] == undefined)
        {
            targetInfo[body] = { "isCloseEnough" : isCloseEnough };
        }
        else
        {
            targetInfo[body].isCloseEnough = (targetInfo[body].isCloseEnough || isCloseEnough);
        }

        if (abs(raycastResult.distance) > (TOLERANCE.zeroLength * meter))
        {
            const targetDirection = raycastResult.distance > 0 ? TargetLocationRelativeToAxisPoint.IN_FRONT : TargetLocationRelativeToAxisPoint.BEHIND;
            if (targetInfo[body].targetDirection == undefined)
            {
                targetInfo[body].targetDirection = targetDirection;
            }
            else if (targetInfo[body].targetDirection != targetDirection)
            {
                targetInfo[body].targetDirection = TargetLocationRelativeToAxisPoint.AMBIGUOUS;
            }
        }
    }

    var returnValue = {};
    for (var targetAndInfo in targetInfo)
    {
        if (targetAndInfo.value.isCloseEnough)
        {
            returnValue[targetAndInfo.key] = targetAndInfo.value.targetDirection;
        }
    }
    return returnValue;
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

