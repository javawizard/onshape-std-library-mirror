FeatureScript 2737; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "2737.0");
import(path : "onshape/std/boolean.fs", version : "2737.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "2737.0");
import(path : "onshape/std/box.fs", version : "2737.0");
import(path : "onshape/std/clashtype.gen.fs", version : "2737.0");
import(path : "onshape/std/containers.fs", version : "2737.0");
import(path : "onshape/std/coordSystem.fs", version : "2737.0");
import(path : "onshape/std/curveGeometry.fs", version : "2737.0");
import(path : "onshape/std/cylinderCast.fs", version : "2737.0");
import(path : "onshape/std/evaluate.fs", version : "2737.0");
import(path : "onshape/std/feature.fs", version : "2737.0");
import(path : "onshape/std/holetables.gen.fs", version : "2737.0");
import(path : "onshape/std/lookupTablePath.fs", version : "2737.0");
import(path : "onshape/std/mathUtils.fs", version : "2737.0");
import(path : "onshape/std/registerSheetMetalBooleanTools.fs", version : "2737.0");
import(path : "onshape/std/revolve.fs", version : "2737.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2737.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2737.0");
import(path : "onshape/std/sketch.fs", version : "2737.0");
import(path : "onshape/std/string.fs", version : "2737.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2737.0");
import(path : "onshape/std/tool.fs", version : "2737.0");
import(path : "onshape/std/units.fs", version : "2737.0");
import(path : "onshape/std/valueBounds.fs", version : "2737.0");
import(path : "onshape/std/cosmeticThreadUtils.fs", version : "2737.0");

export import(path : "onshape/std/holeAttribute.fs", version : "2737.0");
export import(path : "onshape/std/holesectionfacetype.gen.fs", version : "2737.0");
export import(path : "onshape/std/holeUtils.fs", version : "2737.0");
export import(path : "onshape/std/tolerance.fs", version : "2737.0");

/**
 * Defines the end bound for the hole cut.
 * @value THROUGH : Cut holes with a through-all extrude.
 * @value BLIND : Cut holes to a specific depth.
 * @value BLIND_IN_LAST : Cut holes through all parts but the last, then cut
 *          to a specific depth in the last part.
 */
export enum HoleEndStyle
{
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Blind in last" }
    BLIND_IN_LAST,
    annotation { "Name" : "Up to next" }
    UP_TO_NEXT,
    annotation { "Name" : "Up to entity" }
    UP_TO_ENTITY,
    annotation { "Name" : "Through all" }
    THROUGH
}

/**
 * Defines the end bound for the hole cut.
 * @value THROUGH : Cut holes with a through-all extrude.
 * @value BLIND : Cut holes to a specific depth.
 */
export enum HoleEndStyleV2
{
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Up to next" }
    UP_TO_NEXT,
    annotation { "Name" : "Up to entity" }
    UP_TO_ENTITY,
    annotation { "Name" : "Through all" }
    THROUGH
}

/**
 * Defines the tip angle style for the hole tip
 * @value DEGREE118 : Tip angle is set at 118 degrees
 * @value DEGREE135 : Tip angle is set at 135 degrees
 * @value FLAT : Tip angle is flat or 180 degrees
 * @value CUSTOM : User inputs specific angle value
 */
export enum TipAngleStyle
{
    annotation { "Name" : "118 deg" }
    DEGREE118,
    annotation { "Name" : "135 deg" }
    DEGREE135,
    annotation { "Name" : "Flat" }
    FLAT,
    annotation { "Name" : "Custom" }
    CUSTOM
}

/** @internal */
export enum ThreadStandard
{
    UNSET,
    ANSI,
    ISO
}

/**
 * Defines the options to adjust hole position.
 * @value PART : Cut holes starting from the hole location.
 * @value SKETCH : Cut holes starting from the first full entrance.
 * @value PLANE : Cut holes starting at the selected input.
 */
export enum HoleStartStyle
{
    annotation { "Name" : "Start from part" }
    PART,
    annotation { "Name" : "Start from sketch plane" }
    SKETCH,
    annotation { "Name" : "Start from selected plane" }
    PLANE
}

/** @internal */
export enum UnitsSystem
{
    annotation { "Name" : "Inch" }
    INCH ,
    annotation { "Name" : "Metric" }
    METRIC
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

/**
 * Parse and split the numerical and unit portion of a pitch string, e.g. "20 tpi"
 */
export function parsePitch(context is Context, pitch is string)
{
    return match(pitch, "(\\d+(?:\\.\\d+)?(?:\\s+\\d+\\/\\d+)?)\\s*(tpi|mm)(\\s*\\([A-Za-z]+\\))?");
}

/**
 * Parse a pitch string, e.g. "20 tpi" or "1.5 mm, and return its annotation suffix in imperial or metric format, e.g. -20 or x1.5
 */
export function buildPitchAnnotation(context is Context, pitch is string)
{
    const parsedPitch = parsePitch(context, pitch);
    var delimeter = "x";
    if (parsedPitch.hasMatch)
    {
        if (parsedPitch.captures[2] == "tpi")
        {
            delimeter = "-";
        }
        return delimeter ~ parsedPitch.captures[1];
    }
    return pitch;
}

const HOLE_FEATURE_COUNT_VARIABLE_NAME = "-holeFeatureCount"; // Not a valid identifier, so it is not offered in autocomplete

// When `isTappedThrough` is set to `true`, `tappedDepth` should be set to a consistent value to prevent issues in
// drawings tables. The value in question is unimportant, so we will use 0.
const TAPPED_DEPTH_FOR_TAPPED_THROUGH = 0 * meter;

const HOLE_DIAMETER_NAME = "Diameter";
const HOLE_DEPTH_NAME = "Depth";
const C_BORE_DIAMETER_NAME = "Counterbore diameter";
const C_BORE_DEPTH_NAME = "Counterbore depth";
const C_SINK_DIAMETER_NAME = "Countersink diameter";
const C_SINK_ANGLE_NAME = "Countersink angle";
const TAP_DRILL_DIAMETER_NAME = "Tap drill diameter";
const TIP_ANGLE_NAME = "Tip angle";
const TAPPED_DEPTH_NAME = "Tapped depth";
const TAPPED_ANGLE_NAME = "Tapped angle";

function getTolerancedFields(definition is map) returns array
{
    var fields = ["holeDiameter"];

    if (definition.isV2)
    {
        fields = append(fields, "holeDiameterV2");
        if (definition.hasClearance)
        {
            fields = append(fields, "tapDrillDiameterV2");
        }
    }

    if (definition.endStyle != HoleEndStyle.THROUGH)
    {
        fields = append(fields, "holeDepth");
        if (definition.tipAngleStyle == TipAngleStyle.CUSTOM)
        {
            fields = append(fields, "tipAngle");
        }
    }

    if (definition.endStyle == HoleEndStyle.BLIND_IN_LAST || definition.hasClearance)
    {
        if (definition.tapDrillDiameter != undefined)
        {
            fields = append(fields, "tapDrillDiameter");
        }
    }

    if (definition.showTappedDepth)
    {
        if (definition.endStyle != HoleEndStyle.THROUGH || !definition.isTappedThrough)
        {
            fields = append(fields, "tappedDepth");
        }
    }

    if (definition.style == HoleStyle.C_BORE)
    {
        fields = append(fields, "cBoreDepth");
        fields = append(fields, "cBoreDiameter");
    }

    if (definition.style == HoleStyle.C_SINK)
    {
        fields = append(fields, "cSinkAngle");
        fields = append(fields, "cSinkDiameter");
    }

    return fields;
}

function getTolerancesMap(definition is map) returns map
{
    var tolerancesMap = {};
    const fields = getTolerancedFields(definition);
    for (var field in fields)
    {
        const toleranceInfo = getToleranceInfo(definition, field);
        if (isToleranceSet(toleranceInfo))
        {
            tolerancesMap[field] = toleranceInfo;
        }
    }
    return tolerancesMap;
}

function flipLowerBoundIfOldFeature(context is Context, info is ToleranceInfo) returns ToleranceInfo
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V1992_FLIP_LOWER_TOLERANCE_BOUND) && info.lower != undefined)
    {
        info.lower = -info.lower;
    }
    return info;
}

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
annotation { "Feature Type Name" : "Hole", "Editing Logic Function" : "holeEditLogic", "Feature Name Template": "#featureName" }
export const hole = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Feature version", "Default" : true, "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.isV2 is boolean;

        annotation { "Name" : "Initial Entities", "UIHint" : UIHint.ALWAYS_HIDDEN,
                    "Filter" : EntityType.VERTEX && SketchObject.YES && ModifiableEntityOnly.YES || BodyType.MATE_CONNECTOR }
        definition.initEntities is Query;

        annotation { "Name" : "Feature Name Template", "UIHint" : UIHint.ALWAYS_HIDDEN}
        definition.featureName is string;

        annotation { "Name" : "Thread standard", "UIHint" : UIHint.ALWAYS_HIDDEN}
        definition.threadStandard is ThreadStandard;

        annotation { "Name" : "Has clearance", "Default" : false, "UIHint" : UIHint.ALWAYS_HIDDEN  }
        definition.hasClearance is boolean;

        if (definition.isV2)
        {
            annotation { "Name" : "Units", "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "HORIZONTAL_ENUM", "UNCONFIGURABLE"] }
            definition.unitsSystem is UnitsSystem;

            annotation { "Name" : "Style", "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "HORIZONTAL_ENUM", "UNCONFIGURABLE"] }
            definition.styleV2 is HoleStyle;
        }
        else
        {
            annotation { "Name" : "Style", "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "HORIZONTAL_ENUM"] }
            definition.style is HoleStyle;
        }

        annotation { "Name" : "Sketch points to place holes",
                    "Filter" : EntityType.VERTEX && SketchObject.YES && ModifiableEntityOnly.YES || BodyType.MATE_CONNECTOR,
                    "UIHint" : UIHint.INITIAL_FOCUS }
        definition.locations is Query;

        annotation { "Name" : "Merge scope",
                    "Filter" : (EntityType.BODY && BodyType.SOLID && ModifiableEntityOnly.YES && AllowMeshGeometry.YES) }
        definition.scope is Query;

        if (definition.isV2)
        {
            if (definition.unitsSystem == UnitsSystem.INCH)
            {
                if (definition.style == HoleStyle.SIMPLE)
                {
                    annotation { "Name" : "Standard", "Lookup Table" : ANSI_HoleTableEx, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "UNCONFIGURABLE"] }
                    definition.ansiHoleTableEx is LookupTablePath;
                }
                else
                {
                    annotation { "Name" : "Standard", "Lookup Table" : ANSI_HoleTable, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "UNCONFIGURABLE"] }
                    definition.ansiHoleTable is LookupTablePath;
                }
            }
            else if (definition.unitsSystem == UnitsSystem.METRIC)
            {
                if (definition.style == HoleStyle.SIMPLE)
                {
                    annotation { "Name" : "Standard", "Lookup Table" : ISO_HoleTableEx, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "UNCONFIGURABLE"] }
                    definition.isoHoleTableEx is LookupTablePath;
                }
                else
                {
                    annotation { "Name" : "Standard", "Lookup Table" : ISO_HoleTable, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "UNCONFIGURABLE"] }
                    definition.isoHoleTable is LookupTablePath;
                }
            }
        }

        if (definition.threadStandard != ThreadStandard.UNSET && definition.isV2)
        {
            annotation { "Name" : "Thread class", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            definition.showThreadClassV2 is boolean;

            if (definition.showThreadClassV2)
            {
                annotation { "Group Name" : "Thread class", "Driving Parameter" : "showThreadClassV2", "Collapsed By Default" : false }
                {
                    if (definition.threadStandard == ThreadStandard.ANSI)
                    {
                        annotation { "Name" : "Thread class", "Lookup Table" : ANSI_ThreadClassHoleTable, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "UNCONFIGURABLE"] }
                        definition.ansiThreadClassV2 is LookupTablePath;
                    }
                    else
                    {
                        annotation { "Name" : "Thread class", "Lookup Table" : ISO_ThreadClassHoleTable, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "UNCONFIGURABLE"] }
                        definition.isoThreadClassV2 is LookupTablePath;
                    }
                }
            }
        }

        /*
         * showTappedDepth, tappedDepth, tappedAngle and tapClearance are for hole annotations;
         * they currently have no effect on geometry regeneration, but is stored in HoleAttribute. If we modeled the hole's
         * threads, then they would have an effect.
         */
        annotation { "Name" : "Tapped details", "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.showTappedDepth is boolean;

        if (definition.isV2)
        {
            annotation { "Name" : HOLE_DIAMETER_NAME, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_EXPRESSION"] }
            isLength(definition.holeDiameterV2, HOLE_DIAMETER_BOUNDS);
            defineLengthToleranceExtended(definition, "holeDiameterV2", HOLE_DIAMETER_NAME);

            if (definition.hasClearance)
            {
                annotation { "Name" : TAP_DRILL_DIAMETER_NAME, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_EXPRESSION"] }
                isLength(definition.tapDrillDiameterV2, HOLE_DIAMETER_BOUNDS);
                defineLengthTolerance(definition, "tapDrillDiameterV2", TAP_DRILL_DIAMETER_NAME);
            }
        }

        annotation { "Name" : "Start plane", "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_LABEL"] }
        definition.startStyle is HoleStartStyle;

        if (definition.startStyle == HoleStartStyle.PLANE)
        {
            annotation { "Name" : "Hole start plane or mate connector", "Filter" : (EntityType.FACE && GeometryType.PLANE) || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
            definition.startBoundEntity is Query;
        }

        if (definition.isV2)
        {
            annotation { "Name" : "Termination", "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_LABEL"] }
            definition.endStyleV2 is HoleEndStyleV2;
        }
        else
        {
            annotation { "Name" : "Termination", "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_LABEL"] }
            definition.endStyle is HoleEndStyle;
        }

        annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.oppositeDirection is boolean;

        if ((!definition.isV2 && (definition.endStyle == HoleEndStyle.UP_TO_ENTITY ||
            definition.endStyle == HoleEndStyle.UP_TO_NEXT)) ||
            (definition.isV2 &&
            (definition.endStyleV2 == HoleEndStyleV2.UP_TO_ENTITY ||
            definition.endStyleV2 == HoleEndStyleV2.UP_TO_NEXT)))
        {
            if ((!definition.isV2 && definition.endStyle == HoleEndStyle.UP_TO_ENTITY) || (definition.isV2 && definition.endStyleV2 == HoleEndStyleV2.UP_TO_ENTITY))
            {
                annotation {"Name" : "Up to entity or mate connector",
                    "Filter" : (EntityType.FACE && SketchObject.NO && AllowMeshGeometry.YES) || QueryFilterCompound.ALLOWS_VERTEX,
                    "MaxNumberOfPicks" : 1 }
                definition.endBoundEntity is Query;
            }

            annotation {"Name" : "Offset from tip", "Column Name" : "Has offset", "UIHint" : [ "DISPLAY_SHORT", "FIRST_IN_ROW" ] }
            definition.offset is boolean;

            if (definition.offset)
            {
                annotation {"Name" : "Offset from tip", "UIHint" : UIHint.DISPLAY_SHORT }
                isLength(definition.offsetDistance, ZERO_INCLUSIVE_OFFSET_BOUNDS);

                annotation {"Name" : "Opposite direction", "Column Name" : "Offset opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION}
                definition.oppositeOffsetDirection is boolean;
            }
        }

        if (!definition.isV2)
        {
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
        }

        if (definition.threadStandard != ThreadStandard.UNSET && !definition.isV2)
        {
            annotation { "Name" : "Thread class", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            definition.showThreadClass is boolean;

            if (definition.showThreadClass)
            {
                annotation { "Group Name" : "Thread class", "Driving Parameter" : "showThreadClass", "Collapsed By Default" : false }
                {
                    if (definition.threadStandard == ThreadStandard.ANSI)
                    {
                        annotation { "Name" : "Thread class", "Lookup Table" : ANSI_ThreadClassHoleTable, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "UNCONFIGURABLE"] }
                        definition.ansiThreadClass is LookupTablePath;
                    }
                    else
                    {
                        annotation { "Name" : "Thread class", "Lookup Table" : ISO_ThreadClassHoleTable, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "UNCONFIGURABLE"] }
                        definition.isoThreadClass is LookupTablePath;
                    }
                }
            }
        }

        if (!definition.isV2)
        {
            annotation { "Name" : HOLE_DIAMETER_NAME, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_EXPRESSION"] }
            isLength(definition.holeDiameter, HOLE_DIAMETER_BOUNDS);
            defineLengthToleranceExtended(definition, "holeDiameter", HOLE_DIAMETER_NAME);
        }

        if (definition.style == HoleStyle.C_BORE)
        {
            annotation { "Name" : C_BORE_DIAMETER_NAME, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_EXPRESSION"] }
            isLength(definition.cBoreDiameter, HOLE_BORE_DIAMETER_BOUNDS);
            defineLengthTolerance(definition, "cBoreDiameter", C_BORE_DIAMETER_NAME);

            annotation { "Name" : C_BORE_DEPTH_NAME, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_EXPRESSION"] }
            isLength(definition.cBoreDepth, HOLE_BORE_DEPTH_BOUNDS);
            defineLengthTolerance(definition, "cBoreDepth", C_BORE_DEPTH_NAME);
        }
        else if (definition.style == HoleStyle.C_SINK)
        {
            annotation { "Name" : C_SINK_DIAMETER_NAME, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_EXPRESSION"] }
            isLength(definition.cSinkDiameter, HOLE_BORE_DIAMETER_BOUNDS);
            defineLengthTolerance(definition, "cSinkDiameter", C_SINK_DIAMETER_NAME);

            annotation { "Name" : C_SINK_ANGLE_NAME, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            isAngle(definition.cSinkAngle, CSINK_ANGLE_BOUNDS);
            defineAngleTolerance(definition, "cSinkAngle", C_SINK_ANGLE_NAME);
        }

        if (definition.endStyle == HoleEndStyle.BLIND_IN_LAST)
        {
            if (definition.tapDrillDiameter != undefined)
            {
                annotation { "Name" : TAP_DRILL_DIAMETER_NAME, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "SHOW_EXPRESSION"] }
                isLength(definition.tapDrillDiameter, HOLE_DIAMETER_BOUNDS);
                defineLengthTolerance(definition, "tapDrillDiameter", TAP_DRILL_DIAMETER_NAME);
            }
        }

        if (definition.majorDiameter != undefined)
        {
            annotation { "Name" : "Tap major diameter", "UIHint" : ["ALWAYS_HIDDEN"] }
            isLength(definition.majorDiameter, HOLE_MAJOR_DIAMETER_BOUNDS);
            // We don't define a tolerance here because it is always hidden
        }

        annotation { "Name" : "Multiple", "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.isMultiple is boolean;


        if ((!definition.isV2 && definition.endStyle != HoleEndStyle.THROUGH) || (definition.isV2 && definition.endStyleV2 != HoleEndStyleV2.THROUGH))
        {
            if (definition.isMultiple)
            {
                annotation { "Name" : HOLE_DEPTH_NAME, "Default": "Multiple", "UIHint" : [UIHint.READ_ONLY] }
                definition.holeDepthMultiple is string;
                defineLengthTolerance(definition, "holeDepthMultiple", HOLE_DEPTH_NAME);
            }
            else if ((!definition.isV2 && (definition.endStyle == HoleEndStyle.UP_TO_ENTITY || definition.endStyle == HoleEndStyle.UP_TO_NEXT)) ||
                (definition.isV2 && (definition.endStyleV2 == HoleEndStyleV2.UP_TO_ENTITY || definition.endStyleV2 == HoleEndStyleV2.UP_TO_NEXT)))
            {
                annotation { "Name" : HOLE_DEPTH_NAME, "UIHint" : [UIHint.READ_ONLY] }
                isLength(definition.holeDepthComputed, HOLE_DEPTH_BOUNDS);
                defineLengthTolerance(definition, "holeDepthComputed", HOLE_DEPTH_NAME);
            }
            else
            {
                annotation { "Name" : HOLE_DEPTH_NAME, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition.holeDepth, HOLE_DEPTH_BOUNDS);
                defineLengthTolerance(definition, "holeDepth", HOLE_DEPTH_NAME);
            }
            annotation { "Name" : "Tip angle style", "UIHint" : UIHint.SHOW_LABEL }
            definition.tipAngleStyle is TipAngleStyle;

            if (definition.tipAngleStyle == TipAngleStyle.CUSTOM)
            {
                annotation { "Name" : TIP_ANGLE_NAME, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isAngle(definition.tipAngle, TIP_ANGLE_BOUNDS);
                defineAngleTolerance(definition, "tipAngle", TIP_ANGLE_NAME);
            }
        }
        if (definition.showTappedDepth)
        {
            if ((!definition.isV2 && definition.endStyle == HoleEndStyle.THROUGH) || (definition.isV2 && definition.endStyleV2 == HoleEndStyleV2.THROUGH))
            {
                annotation { "Name" : "Tap through all", "Default" : true, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                definition.isTappedThrough is boolean;
            }

            if ((!definition.isV2 && definition.endStyle != HoleEndStyle.THROUGH) || !definition.isTappedThrough || (definition.isV2 && (definition.endStyleV2 != HoleEndStyleV2.THROUGH)))
            {
                annotation { "Name" : TAPPED_DEPTH_NAME, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition.tappedDepth, HOLE_DEPTH_BOUNDS);
                defineLengthTolerance(definition, "tappedDepth", TAPPED_DEPTH_NAME);
            }

            if (definition.endStyle != HoleEndStyle.BLIND_IN_LAST && definition.standardTappedOrClearance != undefined)
            {
                annotation { "Name" : TAPPED_ANGLE_NAME, "UIHint" : UIHint.ALWAYS_HIDDEN }
                isAngle(definition.tappedAngle, HOLE_TAPPED_ANGLE_BOUNDS);
            }

            if ((!definition.isV2 && (definition.endStyle == HoleEndStyle.BLIND || definition.endStyle == HoleEndStyle.BLIND_IN_LAST)) ||
                (definition.isV2 && definition.endStyleV2 == HoleEndStyleV2.BLIND))
            {
                annotation { "Name" : "Tap clearance (number of thread pitch lengths)", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isReal(definition.tapClearance, HOLE_CLEARANCE_BOUNDS);
            }
        }
    }
    {
        definition = syncHoleDefinitionV2Params(definition);

        // Set a generated feature name template. Version is not required as old features will be displayed with their saved names
        setFeatureComputedParameter(context, id, {
            "name" : "featureName",
            "value" : generateFeatureNameTemplate(context, definition)
        });

        for (var field in getTolerancedFields(definition))
        {
            definition = updateFitToleranceFields(context, id, definition, field);
        }

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
            var cBoreTooSmall;
            // Do not allow holeDiameter to be equal to cBoreDiameter
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1900_EQUAL_HOLE_CBORE_FIX))
            {
                cBoreTooSmall = definition.holeDiameter > (definition.cBoreDiameter - TOLERANCE.zeroLength * meter);
            }
            else
            {
                cBoreTooSmall = definition.holeDiameter > (definition.cBoreDiameter + TOLERANCE.zeroLength * meter);
            }

            if (cBoreTooSmall)
            {
                const holeDiameterUIid = definition.isV2 ? "holeDiameterV2" : "holeDiameter";
                throw regenError(ErrorStringEnum.HOLE_CBORE_TOO_SMALL, [holeDiameterUIid, "cBoreDiameter"]);
            }

            if (definition.endStyle == HoleEndStyle.BLIND)
            {
                var cBoreTooDeep;
                // Do not allow holeDepth to be equal to cBoreDepth
                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1900_EQUAL_HOLE_CBORE_FIX))
                {
                    cBoreTooDeep = definition.holeDepth < (definition.cBoreDepth + TOLERANCE.zeroLength * meter);
                }
                else
                {
                    cBoreTooDeep = definition.holeDepth < (definition.cBoreDepth - TOLERANCE.zeroLength * meter);
                }

                if (cBoreTooDeep)
                {
                    throw regenError(ErrorStringEnum.HOLE_CBORE_TOO_DEEP, ["holeDepth", "cBoreDepth"]);
                }
            }
        }

        if (definition.style == HoleStyle.C_SINK && isAtVersionOrLater(context, FeatureScriptVersionNumber.V206_LINEAR_RANGE))
        {
            var cSinkTooSmall;
            // Do not allow holeDiameter to be equal to cSinkDiameter
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1900_EQUAL_HOLE_CBORE_FIX))
            {
                cSinkTooSmall = definition.holeDiameter > (definition.cSinkDiameter - TOLERANCE.zeroLength * meter);
            }
            else
            {
                cSinkTooSmall = definition.holeDiameter > (definition.cSinkDiameter + TOLERANCE.zeroLength * meter);
            }

            if (cSinkTooSmall)
            {
                const holeDiameterUIid = definition.isV2 ? "holeDiameterV2" : "holeDiameter";
                throw regenError(ErrorStringEnum.HOLE_CSINK_TOO_SMALL, [holeDiameterUIid, "cSinkDiameter"]);
            }

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
                if (definition.endStyle == HoleEndStyle.BLIND && tipDepth < cSinkDepth - TOLERANCE.zeroLength * meter
                    && !isAtVersionOrLater(context, FeatureScriptVersionNumber.V2558_HOLE_DISABLE_CSINK_DEPTH_CHECK))
                    throw regenError(ErrorStringEnum.HOLE_CSINK_TOO_DEEP, ["holeDepth", "cSinkDepth"]);
            }
        }

        if ((definition.style == HoleStyle.C_BORE || definition.style == HoleStyle.C_SINK) && isPipeTapHole(definition))
        {
            reportFeatureInfo(context, id, ErrorStringEnum.HOLE_CBORE_CSINK_VALUES_NON_STD);
        }

        if (definition.style == HoleStyle.C_SINK && isAtVersionOrLater(context, FeatureScriptVersionNumber.V1945_HOLE_CSINK_TOLERANCE_BOUNDS_CHECK))
        {
            const cSinkAngleToleranceInfo = getToleranceInfo(definition, "cSinkAngle");
            const cSinkAngleBounds = getToleranceBounds(definition.cSinkAngle, flipLowerBoundIfOldFeature(context, cSinkAngleToleranceInfo), {
                "minimum" : 0 * degree,
                "maximum" : 180 * degree,
                "useDrawingLimitsFix" : isAtVersionOrLater(context, FeatureScriptVersionNumber.V1989_FIX_LIMITS_BOUNDS)
            });
            const boundsParameterIds = getToleranceBoundsParameterIds("cSinkAngle", cSinkAngleToleranceInfo);
            const boundsSize = size(boundsParameterIds);
            if (cSinkAngleBounds[0] < 0 * degree)
            {
                var errorFields = [];
                if (boundsSize == 2)
                {
                    // Either limits or deviation; we use the lower bound
                    errorFields = [boundsParameterIds[1]];
                }
                else if (boundsSize == 1)
                {
                    // Symmetrical bounds
                    errorFields = [boundsParameterIds[0]];
                }
                throw regenError(ErrorStringEnum.HOLE_CSINK_ANGLE_TOO_NARROW, errorFields);
            }
            if (cSinkAngleBounds[1] > 180 * degree)
            {
                var errorFields = [];
                if (boundsSize > 0)
                {
                    // Use the upper bound
                    // It does not matter if this is symmetrical or not; the upper bound is always first
                    errorFields = [boundsParameterIds[0]];
                }
                throw regenError(ErrorStringEnum.HOLE_CSINK_ANGLE_TOO_WIDE, errorFields);
            }
        }

        var boundsErrors = [];

        // Check that upper bounds > lower bounds
        const hasDrawingLimitsFix = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1989_FIX_LIMITS_BOUNDS);
        for (var parameterId, toleranceInfo in getTolerancesMap(definition))
        {
            // Get the absolute upper and lower bounds in terms of the parameter's unit
            const unit = getUnitOfValue(definition[parameterId]);
            const absoluteLower = unit * -inf;
            const absoluteUpper = unit * inf;
            const bounds = getToleranceBounds(definition[parameterId], flipLowerBoundIfOldFeature(context, toleranceInfo), {
                "minimum" : absoluteLower,
                "maximum" : absoluteUpper,
                "useDrawingLimitsFix" : hasDrawingLimitsFix
            });

            if (bounds[0] > bounds[1])
            {
                boundsErrors = concatenateArrays([boundsErrors, getToleranceBoundsParameterIds(parameterId, toleranceInfo)]);
            }
        }

        if (size(boundsErrors) > 0)
        {
            throw regenError(ErrorStringEnum.HOLE_REVERSED_BOUNDS, boundsErrors);
        }

        // ------------- Definition adjustment -------------

        if (definition.tipAngleStyle == TipAngleStyle.DEGREE118)
        {
            definition.tipAngle = 118 * degree;
        }
        else if (definition.tipAngleStyle == TipAngleStyle.DEGREE135)
        {
            definition.tipAngle = 135 * degree;
        }
        else if (definition.tipAngleStyle == TipAngleStyle.FLAT)
        {
            definition.tipAngle = 180 * degree;
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

        if (definition.hasClearance && tolerantGreaterThanOrEqual(definition.tapDrillDiameterV2, definition.holeDiameterV2))
        {
            throw regenError(ErrorStringEnum.HOLE_TAP_DIA_TOO_LARGE_OR_EQUAL, ["holeDiameterV2", "tapDrillDiameterV2"]);
        }

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1829_HOLE_IS_TAPPED_THROUGH))
        {
            definition = definition->setIsTappedThroughAndFixTappedDepth(definition.endStyle == HoleEndStyle.THROUGH && definition.isTappedThrough);
        }

        definition.transform = getRemainderPatternTransform(context, { "references" : definition.locations });

        if (definition.endStyle == HoleEndStyle.UP_TO_ENTITY && isQueryEmpty(context, definition.endBoundEntity))
        {
            throw regenError(ErrorStringEnum.HOLE_NO_END_BOUNDS, ["endBoundEntity"]);
        }

        if (definition.startStyle == HoleStartStyle.PLANE && isQueryEmpty(context, definition.startBoundEntity))
        {
            throw regenError(ErrorStringEnum.HOLE_NO_START_BOUND, ["startBoundEntity"]);
        }

        const locations = reduceLocations(context, definition.locations);
        if (locations == [])
        {
            throw regenError(ErrorStringEnum.HOLE_NO_POINTS, ["locations"]);
        }
        enforceMaxLocations(context, size(locations));

        if (definition.isV2)
        {
            var table = getStandardTable(definition);
            if (isLookupTableViolated(definition, table, ignoreStandardProperties(table)))
            {
                reportFeatureInfo(context, id, ErrorStringEnum.HOLE_PARAMS_OVERRIDDEN_INFO);
            }
        }

        // -- If any feature status is set above this line, `produceHoles` will display the hole tools as error entities --

        // ------------- Perform the operation -------------
        produceHoles(context, id, definition, locations);

        // Verify consistency between pitch, tap depth, and clearance (BEL-120375)
        // This must be done after `produceHoles`, because that function will display error entities if the feature has a status.
        if (definition.showTappedDepth && definition.endStyle != HoleEndStyle.THROUGH &&
            definition.endStyle != HoleEndStyle.UP_TO_NEXT &&
            definition.endStyle != HoleEndStyle.UP_TO_ENTITY &&
            isAtVersionOrLater(context, FeatureScriptVersionNumber.V1135_HOLE_TAP_CHECK))
        {
            var pitch = computePitch(context, definition);
            if (pitch != undefined && !tolerantEquals(definition.holeDepth, definition.tappedDepth + definition.tapClearance * pitch))
            {
                reportFeatureWarning(context, id, ErrorStringEnum.HOLE_INCONSISTENT_TAP_INFO);
            }
        }
    }, {
            endStyle : HoleEndStyle.BLIND,
            startStyle : HoleStartStyle.PART,
            style : HoleStyle.SIMPLE,
            styleV2 : HoleStyle.SIMPLE,
            oppositeDirection : false,
            tipAngle : 118 * degree,
            tipAngleStyle : TipAngleStyle.DEGREE118,
            useTipDepth : false,
            cSinkUseDepth : false,
            cSinkDepth : 0 * meter,
            cSinkAngle : 90 * degree,
            showTappedDepth : false,
            hasClearance : false,
            showThreadClass : false,
            showThreadClassV2 : false,
            threadStandard : ThreadStandard.UNSET,
            holeDepth : 0.5 * inch,
            holeDepthComputed : 0.0 * inch,
            tappedDepth : 0.5 * inch,
            tappedAngle : 0.0 * degree,
            tapClearance : 3,
            isTappedThrough : false,
            oppositeOffsetDirection : false,
            isMultiple : false,
            initEntities : qNothing(),
            featureName : "",
            isV2 : false,

            // Defaults for precision and tolerance. These are needed or else
            // the upgrade task fails for old holes.
            holeDiameterPrecision : PrecisionType.DEFAULT,
            holeDiameterToleranceType : ToleranceTypeExtended.NONE,
            cBoreDiameterPrecision : PrecisionType.DEFAULT,
            cBoreDiameterToleranceType : ToleranceType.NONE,
            cBoreDepthPrecision : PrecisionType.DEFAULT,
            cBoreDepthToleranceType : ToleranceType.NONE,
            cSinkDiameterPrecision : PrecisionType.DEFAULT,
            cSinkDiameterToleranceType : ToleranceType.NONE,
            tapDrillDiameterPrecision : PrecisionType.DEFAULT,
            tapDrillDiameterToleranceType : ToleranceType.NONE,
            holeDepthPrecision : PrecisionType.DEFAULT,
            holeDepthToleranceType : ToleranceType.NONE,
            tappedDepthPrecision : PrecisionType.DEFAULT,
            tappedDepthToleranceType : ToleranceType.NONE,
            cSinkAnglePrecision : PrecisionType.DEFAULT,
            cSinkAngleToleranceType : ToleranceType.NONE,
            tipAnglePrecision : PrecisionType.DEFAULT,
            tipAngleToleranceType : ToleranceType.NONE,
            holeDepthComputedPrecision : PrecisionType.DEFAULT,
            holeDepthComputedToleranceType : ToleranceType.NONE,
            holeDepthMultiplePrecision : PrecisionType.DEFAULT,
            holeDepthMultipleToleranceType : ToleranceType.NONE
        });

function getActiveLookupTable(definition is map) returns map
{
    if (definition.isV2)
    {
        if (definition.unitsSystem == UnitsSystem.INCH)
        {
            return definition.style == HoleStyle.SIMPLE ? ANSI_HoleTableEx : ANSI_HoleTable;
        }
        else if (definition.unitsSystem == UnitsSystem.METRIC)
        {
            return definition.style == HoleStyle.SIMPLE ? ISO_HoleTableEx : ISO_HoleTable;
        }
    }
    else
    {
        if (definition.endStyle == HoleEndStyle.BLIND_IN_LAST)
        {
            return blindInLastHoleTable;
        }
        else
        {
            return tappedOrClearanceHoleTable;
        }
    }

    return {};
}

function getActiveTable(definition is map)
{
    if (definition != undefined && definition != {})
    {
        if (definition.isV2)
        {
            if (definition.unitsSystem == UnitsSystem.INCH)
            {
                return definition.styleV2 == HoleStyle.SIMPLE ? definition.ansiHoleTableEx : definition.ansiHoleTable;
            }
            else if (definition.unitsSystem == UnitsSystem.METRIC)
            {
                return definition.styleV2 == HoleStyle.SIMPLE ? definition.isoHoleTableEx : definition.isoHoleTable;
            }
        }
        else
        {
            if (definition.endStyle == HoleEndStyle.BLIND_IN_LAST)
            {
                return definition.standardBlindInLast;
            }
            else
            {
                return definition.standardTappedOrClearance;
            }
        }
    }
    return undefined;
}

/**
 * @param table : the current table map for the active size
 * @returns : map of property names, if value is true, then that property's value should not be used to invalidate the standard setting
 */
function ignoreStandardProperties(table is map) returns map
{
    var ignoreProperties = {};

    ignoreProperties["cBoreDiameter"] = true;
    ignoreProperties["cBoreDepth"] = true;

    ignoreProperties["cSinkDiameter"] = true;
    ignoreProperties["cSinkAngle"] = true;

    ignoreProperties["tappedDepth"] = true;
    ignoreProperties["holeDepth"] = true;

    return ignoreProperties;
}

function getThreadClassTable(definition is map) returns LookupTablePath
{
    if (definition.showThreadClass)
    {
        if (definition.threadStandard == ThreadStandard.ANSI)
        {
            return definition.ansiThreadClass;
        }
        else if (definition.threadStandard == ThreadStandard.ISO)
        {
            return definition.isoThreadClass;
        }
    }

    return lookupTablePath({});
}

function updateThreadClassDefinition(context is Context, definition is map)
{
    definition.threadStandard = ThreadStandard.UNSET;
    var standard = getStandardAndTable(definition).standard;
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2356_HOLE_ADDED_THREAD_FORM_ANNOTATION) && standard != undefined && standard["type"] == "Straight tap")
    {
        definition.threadStandard = definition.unitsSystem == UnitsSystem.INCH ? ThreadStandard.ANSI : ThreadStandard.ISO;
    }
    else if (standard != undefined && standard["type"] == "Tapped")
    {
        if (standard["standard"] == "ANSI")
        {
            definition.threadStandard = ThreadStandard.ANSI;
        }
        else if (standard["standard"] == "ISO")
        {
            definition.threadStandard = ThreadStandard.ISO;
        }
    }
    return definition;
}

function isPipeTapHole(definition is map) returns boolean
{
    return isTaperedPipeTapHole(definition) || isStraightPipeTapHole(definition);
}

function isStraightPipeTapHole(definition is map) returns boolean
{
    if (definition.isV2)
    {
        const path = getActiveTable(definition);
        return path["type"] == "Straight Pipe Tap";
    }
    return definition.endStyle != HoleEndStyle.BLIND_IN_LAST && definition.standardTappedOrClearance != undefined && definition.standardTappedOrClearance["type"] == "Straight Pipe Tap";
}

function isTaperedPipeTapHole(definition is map) returns boolean
{
    if (definition.isV2)
    {
        const path = getActiveTable(definition);
        return path["type"] == "Tapered Pipe Tap";
    }
    return definition.endStyle != HoleEndStyle.BLIND_IN_LAST && definition.standardTappedOrClearance != undefined && definition.standardTappedOrClearance["type"] == "Tapered Pipe Tap";
}

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
                    "useSMDefinitionTopologyId" : sheetMetalTargetInfo.hasSheetMetalTargets,
                    "keepTools" : sheetMetalTargetInfo.hasSheetMetalTargets
                });
    }
    catch (error)
    {
        displayToolErrorEntities(context, topLevelId, definition, locations, error);
        if (error == ErrorStringEnum.HOLE_TARGETS_DO_NOT_DIFFER)
        {
            // HOLE_TARGETS_DO_NOT_DIFFER comes from opHole when the user has chosen BLIND_IN_LAST mode, and we set
            // `targetMustDifferFromPrevious` on the transition to LAST_TARGET_START to ask opHole to make sure that
            // there is a distinct "last target". If thrown, switch to a more specific error which specifically mentions
            // blind in last.
            error = definition.isV2 ? ErrorStringEnum.HOLE_CANNOT_DETERMINE_TAPPED_BODY : ErrorStringEnum.HOLE_CANNOT_DETERMINE_LAST_BODY;
        }
        throw error;
    }

    // The following two pipelines are not mutually exclusive. It is possible to have both sheet metal and non-sheet metal targets.
    const successfulHoles = new box({});
    var wallToCuttingToolBodyIds = {};
    if (sheetMetalTargetInfo.hasSheetMetalTargets)
    {
        // If we have sheet metal targets, opHole will leave behind the solid hole tools. Use them for sheet metal,
        // consuming them in the process. This must happen before `createAttributesFromQuery` or that function will get
        // confused by the solid tools still existing.
        wallToCuttingToolBodyIds = handleSheetMetalCutAndAttribution(context, topLevelId, opHoleId, definition, opHoleInfo.returnMapPerHole,
            sheetMetalTargetInfo, locations, successfulHoles);
    }
    if (sheetMetalTargetInfo.hasNonSheetMetalTargets ||
        isAtVersionOrLater(context, FeatureScriptVersionNumber.V2209_COUNTER_HOLES_IN_SHEET_METAL))
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
                locations[i], opHoleInfo.returnMapPerHole[i], i, opHoleInfo.holeDepth, wallToCuttingToolBodyIds);
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
    const startBoundEntity = definition.startStyle == HoleStartStyle.PLANE && isAtVersionOrLater(context, FeatureScriptVersionNumber.V2154_HOLE_START_STYLE_UPGRADE_FIX) ? definition.startBoundEntity : qNothing();
    var axes = computeAxes(context, locations, definition.oppositeDirection, /* feature pattern transform */ definition.transform, startBoundEntity);

    const firstPositionReference = definition.startStyle != HoleStartStyle.PART ? HolePositionReference.AXIS_POINT : HolePositionReference.TARGET_START;

    const startProfileInfo = computeStartProfiles(context, definition, firstPositionReference);
    const endProfileInfo = computeEndProfiles(context, definition, firstPositionReference);

    const profiles = concatenateArrays([startProfileInfo.profiles, endProfileInfo.profiles]);
    const faceTypes = concatenateArrays([startProfileInfo.faceTypes, endProfileInfo.faceTypes]);
    const faceNames = mapArray(faceTypes, faceType => faceTypeToFaceTypeData[faceType].name);

    const holeDef = holeDefinition(profiles, { "faceNames" : faceNames });

    var endBound = qNothing();
    if (definition.endStyle == HoleEndStyle.UP_TO_ENTITY)
    {
        endBound = definition.endBoundEntity;
    }

    const returnMapPerHole = callSubfeatureAndProcessStatus(topLevelId, opHole, context, opHoleId, mergeMaps({
                    "holeDefinition" : holeDef,
                    "axes" : axes,
                    "identities" : locations,
                    "targets" : definition.scope,
                    "endBoundEntity" : endBound
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

// Occurs after the call to `opHole`.  When called, we are in a state where the hole tools still exist.
// The function is responsible for
// - Before V2209_COUNTER_HOLES_IN_SHEET_METAL - using the tools to cut into the sheet metal targets,
//   consuming the tools in the process.  When finished, the tools should be gone, and the sheet metal
//   should be cut, rebuilt, and attributed properly.
// - At or after V2209_COUNTER_HOLES_IN_SHEET_METAL - registering the tools with the underlying sheet
//   metal master body's walls as SMAttribute and calling updateSheetMetalGeometry which uses this
//   information to perform the cuts on the walls' thickened patch bodies.
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
    var wallToCuttingToolBodyIds = {};
    try
    {
        const enableCounterHolesInSM = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2209_COUNTER_HOLES_IN_SHEET_METAL);
        const holeToolBodiesQ = qBodyType(qCreatedBy(opHoleId, EntityType.BODY), BodyType.SOLID);
        if (enableCounterHolesInSM)
        {
            wallToCuttingToolBodyIds = callSubfeatureAndProcessStatus(topLevelId, registerSheetMetalBooleanTools, context, booleanId, {
                        "targets" : definition.scope,
                        "subtractiveTools" : holeToolBodiesQ,
                        "doUpdateSMGeometry" : true
                    });
        }
        // If any of the tools couldn't create holes the new way using registerSheetMetalBooleanTools above, because they collide with curved walls, side walls etc.,
        // those tools will remain public. Create holes using the old way for such tools so that there is no regression.
        if (!isQueryEmpty(context, holeToolBodiesQ))
        {
            const showUnsupportedCounterHoleEntities = enableCounterHolesInSM &&
                    (definition.style == HoleStyle.C_BORE || definition.style == HoleStyle.C_SINK);
            callSubfeatureAndProcessStatus(topLevelId, booleanBodies, context, enableCounterHolesInSM ? booleanId + "old" : booleanId, {
                        "targets" : definition.scope,
                        "tools" : holeToolBodiesQ,
                        "keepTools" : showUnsupportedCounterHoleEntities,
                        "operationType" : BooleanOperationType.SUBTRACTION
                    });
            if (showUnsupportedCounterHoleEntities)
            {
                if (getFeatureInfo(context, topLevelId) == undefined &&
                    getFeatureWarning(context, topLevelId) == undefined &&
                    getFeatureError(context, topLevelId) == undefined)
                {
                    reportFeatureInfo(context, topLevelId, ErrorStringEnum.SHEET_METAL_COUNTER_HOLE_UNSUPPORTED);
                    setErrorEntities(context, topLevelId, { "entities" : holeToolBodiesQ });
                }
                opDeleteBodies(context, booleanId + "deleteErrorEntities", { "entities" : holeToolBodiesQ });
            }
        }
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
        const instanceProducedEdges = assignSheetMetalHoleAttributesForInstance(context, topLevelId, createdUsingNewHolePipeline,
            buildHoleAttributeId(topLevelId, i), smHoleEdgeQueries, subtopologyTrackingPerTool[i], definition, i);
        if (instanceProducedEdges)
        {
            successfulHoles[][i] = true;
        }
    }

    return wallToCuttingToolBodyIds;
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

    // Show an WARN and highlight unsuccessful locations if the hole has partially failed
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

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2039_HOLE_FEATURE_BAD_POSITION_WARNING))
        {
            reportFeatureWarning(context, topLevelId, ErrorStringEnum.HOLE_PARTIAL_FAILURE);
        }
        else
        {
            reportFeatureInfo(context, topLevelId, ErrorStringEnum.HOLE_PARTIAL_FAILURE);
        }
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
// Clearance profiles
const CLEARANCE_TRANSITION_OUTER_PROFILE_NAME = "clearanceOuterTransition";
const CLEARANCE_TRANSITION_INNER_PROFILE_NAME = "clearanceInnerTransition";

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
    TIP,
    // Clearance cylindrical bore face
    CLEARANCE_CYLINDER_FACE,
    // Clearance planar diameter face
    CLEARANCE_PLANE_FACE
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
        },
        HoleFaceType.CLEARANCE_CYLINDER_FACE : {
            "name" : "clearanceCylinder",
            "sectionFaceType" : HoleSectionFaceType.CLEARANCE_DIAMETER_FACE
        },
        HoleFaceType.CLEARANCE_PLANE_FACE : {
            "name" : "clearancePlane",
            "sectionFaceType" : HoleSectionFaceType.CLEARANCE_DEPTH_FACE
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
function computeStartProfiles(context is Context, definition is map, firstPositionReference is HolePositionReference) returns map
{
    const shaftRadius = definition.holeDiameter / 2.0;

    var profiles = [];
    var faceTypes = [];
    if (definition.style == HoleStyle.SIMPLE)
    {
        profiles = [holeProfileBeforeReference(firstPositionReference, 0 * meter, shaftRadius, { "name" : BEFORE_SHAFT_PROFILE_NAME })];
        faceTypes = [HoleFaceType.CAP];
    }
    else if (definition.style == HoleStyle.C_BORE)
    {
        const cBoreRadius = definition.cBoreDiameter / 2.0;
        profiles = [
                    holeProfileBeforeReference(firstPositionReference, 0 * meter, cBoreRadius, { "name" : BEFORE_CBORE_PROFILE_NAME }),
                    holeProfile(firstPositionReference, definition.cBoreDepth, cBoreRadius, { "name" : CBORE_TRANSITION_PROFILE_NAME }),
                    holeProfile(firstPositionReference, definition.cBoreDepth, shaftRadius, { "name" : BEFORE_SHAFT_PROFILE_NAME })
                ];
        faceTypes = [HoleFaceType.CAP, HoleFaceType.CBORE_CYLINDER_FACE, HoleFaceType.CBORE_PLANE_FACE];
    }
    else if (definition.style == HoleStyle.C_SINK)
    {
        const cSinkRadius = definition.cSinkDiameter / 2.0;
        const cSinkDepth = (cSinkRadius - shaftRadius) / tan(definition.cSinkAngle / 2.0);
        profiles = [
                    // When the entry face is flat, the second profile will be collapsed onto the first profile by
                    // opHole. When collapsing the earliest name in the list is kept, so BEFORE_CSINK_PROFILE_NAME will
                    // survive, with CSINK_TRANSITION_PROFILE_NAME skipped.
                    holeProfileBeforeReference(firstPositionReference, 0 * meter, cSinkRadius, { "name" : BEFORE_CSINK_PROFILE_NAME }),
                    holeProfile(firstPositionReference, 0 * meter, cSinkRadius, { "name" : CSINK_TRANSITION_PROFILE_NAME }),
                    holeProfile(firstPositionReference, cSinkDepth, shaftRadius, { "name" : BEFORE_SHAFT_PROFILE_NAME })
                ];
                // If the second profile is consumed (as described above), the HoleFaceType.NEAR_SIDE_FEATURE_PRE_PRIMARY
                // is also consumed.
        faceTypes = [HoleFaceType.CAP, HoleFaceType.CSINK_CYLINDER_FACE, HoleFaceType.CSINK_CONE_FACE];
    }
    else
    {
        throw "Unrecognized hole style: " ~ definition.style;
    }

    if (definition.hasClearance &&
        (definition.endStyle != HoleEndStyle.THROUGH && !isAtVersionOrLater(context, FeatureScriptVersionNumber.V2335_HOLE_FASTENER_FIT_FIX) ||
        isAtVersionOrLater(context, FeatureScriptVersionNumber.V2335_HOLE_FASTENER_FIT_FIX)))
    {
        const tapRadius = definition.tapDrillDiameter / 2;

        const targetMustDifferFromPrevious = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V2335_HOLE_FASTENER_FIT_FIX);
        const notApplicableForFirstTarget = isAtVersionOrLater(context, FeatureScriptVersionNumber.V2335_HOLE_FASTENER_FIT_FIX);

        profiles = append(profiles, matchedHoleProfile(HolePositionReference.LAST_TARGET_START_IN_DEPTH, shaftRadius, { "name" : CLEARANCE_TRANSITION_OUTER_PROFILE_NAME, "targetMustDifferFromPrevious" : targetMustDifferFromPrevious, "notApplicableForFirstTarget" : notApplicableForFirstTarget }));
        profiles = append(profiles, matchedHoleProfile(HolePositionReference.LAST_TARGET_START_IN_DEPTH, tapRadius, { "name" : CLEARANCE_TRANSITION_INNER_PROFILE_NAME, "notApplicableForFirstTarget" : notApplicableForFirstTarget }));

        faceTypes = concatenateArrays([faceTypes, [HoleFaceType.CLEARANCE_CYLINDER_FACE, HoleFaceType.CLEARANCE_PLANE_FACE]]);
    }

    return {
        "profiles" : profiles,
        "faceTypes" : faceTypes
    };
}

// Returns a map containing `profiles` and `faceTypes`.  The map will also include a `holeDepth` if the hole is not a
// THROUGH hole; this value represents the depth (excluding the tip) the has been requested by the user, and is
// measured from the final HolePositionReference.
function computeEndProfiles(context is Context, definition is map, firstPositionReference is HolePositionReference) returns map
{
    var shaftRadius = definition.holeDiameter / 2.0;

    var profiles = [];
    var faceTypes = [HoleFaceType.SHAFT]; // The face before the end profiles is always the shaft
    var finalPositionReference;
    var holeDepth = undefined;

    if (definition.hasClearance &&
        (definition.endStyle != HoleEndStyle.THROUGH && !isAtVersionOrLater(context, FeatureScriptVersionNumber.V2335_HOLE_FASTENER_FIT_FIX) ||
        isAtVersionOrLater(context, FeatureScriptVersionNumber.V2335_HOLE_FASTENER_FIT_FIX)))
    {
        shaftRadius = definition.tapDrillDiameter / 2;
    }

    if (definition.endStyle == HoleEndStyle.THROUGH)
    {
        if (definition.hasClearance && !isAtVersionOrLater(context, FeatureScriptVersionNumber.V2335_HOLE_FASTENER_FIT_FIX))
        {
            const tapRadius = definition.tapDrillDiameter / 2;

            profiles = [
                    matchedHoleProfile(HolePositionReference.LAST_TARGET_START, shaftRadius, {
                            "name" : CLEARANCE_TRANSITION_OUTER_PROFILE_NAME,
                            "targetMustDifferFromPrevious" : true
                        }),
                    matchedHoleProfile(HolePositionReference.LAST_TARGET_START, tapRadius, { "name" : CLEARANCE_TRANSITION_INNER_PROFILE_NAME }),
                    matchedHoleProfile(HolePositionReference.LAST_TARGET_END, tapRadius, { "name" : BEFORE_TIP_PROFILE_NAME }),
                    matchedHoleProfile(HolePositionReference.LAST_TARGET_END, 0 * meter, { "name" : TIP_PROFILE_NAME })
                ];

            faceTypes = concatenateArrays([faceTypes, [HoleFaceType.BLIND_IN_LAST_MATCHED, HoleFaceType.OFFSET_SHAFT, HoleFaceType.TIP]]);
            finalPositionReference = HolePositionReference.LAST_TARGET_END;
        }
        else
        {
            // Put the end of the hole slightly past the end of the last part, so that the exit edge references the shaft
            // instead of having a complicated interaction with the final edge
            const padding = definition.hasClearance ? scopeSize(context, definition) : 1000 * TOLERANCE.zeroLength * meter;
            profiles = [
                    holeProfile(HolePositionReference.LAST_TARGET_END, padding, shaftRadius, { "name" : BEFORE_TIP_PROFILE_NAME }),
                    holeProfile(HolePositionReference.LAST_TARGET_END, padding, 0 * meter, { "name" : TIP_PROFILE_NAME })
                ];
            faceTypes = append(faceTypes, HoleFaceType.TIP);
            finalPositionReference = HolePositionReference.LAST_TARGET_END;
        }
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
    else if (definition.endStyle == HoleEndStyle.UP_TO_ENTITY ||
             definition.endStyle == HoleEndStyle.UP_TO_NEXT)
    {
        finalPositionReference = HolePositionReference.UP_TO_ENTITY;
        if (definition.endStyle == HoleEndStyle.UP_TO_NEXT)
        {
            finalPositionReference = HolePositionReference.UP_TO_NEXT;
        }
        var tipDepth = shaftRadius / tan(definition.tipAngle / 2.0);
        var tipOffset = 0 * meter;
        if (definition.offset)
        {
            if (!definition.oppositeOffsetDirection)
            {
                definition.offsetDistance = -definition.offsetDistance;
            }
            tipOffset = definition.offsetDistance - tipDepth;
            tipDepth = definition.offsetDistance;
        }
        profiles = [
                holeProfile(finalPositionReference, tipOffset, shaftRadius, { "name" : BEFORE_TIP_PROFILE_NAME }),
                holeProfile(finalPositionReference, tipDepth, 0 * meter, { "name" : TIP_PROFILE_NAME })
            ];
        faceTypes = append(faceTypes, HoleFaceType.TIP);
        holeDepth = tipOffset;
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
        displayToolErrorEntities(context, topLevelId, definition, locations, error);
    }
    else
    {
        displayToolErrorEntitiesDeprecated(context, topLevelId, definition, locations);
    }
    throw regenError(error, faultyParameters);
}

function displayToolErrorEntities(context is Context, topLevelId is Id, definition is map, locations is array, error is ErrorStringEnum)
{
    const errorId = topLevelId + "errorEntities";

    if (definition.endStyle != HoleEndStyle.BLIND || definition.hasClearance)
    {
        const userStyle = definition.endStyle;

        // Switch to BLIND hole to ensure we do not need to cylinder cast against any targets
        definition.endStyle = HoleEndStyle.BLIND;

        // For through, pick a depth that is sufficiently far for error display. For blind in last, just keep the user
        // specified hole depth for the last part (which is already stored as `holeDepth`), except for certain error cases.
        if (definition.hasClearance ||
            userStyle == HoleEndStyle.THROUGH ||
            (userStyle == HoleEndStyle.BLIND_IN_LAST &&
             (error == ErrorStringEnum.HOLE_EMPTY_SCOPE || error == ErrorStringEnum.HOLE_NO_HITS || error == ErrorStringEnum.HOLE_TARGETS_DO_NOT_DIFFER)))
        {
            const targets = (isQueryEmpty(context, definition.scope)) ? qEverything(EntityType.BODY)->qBodyType(BodyType.SOLID) : definition.scope;
            var bbox = evBox3d(context, { "topology" : qUnion([targets, qUnion(locations)]) });
            // Ensure we do not hit an error if there are no solids in the part studio, and the user has only selected one location
            definition.holeDepth = max(box3dDiagonalLength(bbox), 1 * inch);
        }
    }

    // Start from sketch plane so that we do not need to cylinder cast for the starting position
    definition.startStyle = HoleStartStyle.SKETCH;
    // Do not pass any targets to opHole, this call should be purely AXIS_POINT
    definition.scope = qNothing();
    definition.hasClearance = false;

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
    const startingBodyCount = evaluateQueryCount(context, qEverything(EntityType.BODY));

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
            var trackedCount = evaluateQueryCount(context, trackedBody);
            if (trackedCount != 1)
            {
                const error = trackedCount > 1 ? ErrorStringEnum.HOLE_DISJOINT : ErrorStringEnum.HOLE_DESTROY_SOLID;
                throwRegenErrorWithToolErrorEntities(context, topLevelId, definition, locations, error, ["scope"]);
            }
        }
    }
    else
    {
        const finalBodyCount = evaluateQueryCount(context, qEverything(EntityType.BODY));
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
                    assignSheetMetalHoleAttributesForInstance(context, topLevelId, createdUsingNewHolePipeline, attributeId,
                        smHoleEdgeQueries, holeResult.instanceTracking, definition, holeNumber);
                }
            }
        }
    }
    return result;
}

function computeAxes(context is Context, locations is array, oppositeDirection is boolean, xform is Transform, startBoundEntity is Query) returns array
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
            const planeOnStart = try silent(evPlane(context, { "face" : startBoundEntity }));
            if (planeOnStart != undefined)
            {
                if (!parallelVectors(axis.direction, planeOnStart.normal))
                {
                    throw ErrorStringEnum.HOLE_START_BOUND_INVALID;
                }
                const intersect = intersection(planeOnStart, axis);
                if (intersect.dim == 0)
                {
                    axis.origin = intersect.intersection;
                }
            }
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
    if (changeStartPoint || evaluateQueryCount(context, definition.scope) > 1)
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
        if (definition.startStyle != HoleStartStyle.PART)
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
    return definition.startStyle == HoleStartStyle.PART;
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

const TIP_ANGLE_BOUNDS =
{
    (degree) : [0.1, 118, 180],
    (radian) : 0.5 * PI
} as AngleBoundSpec;

const HOLE_CLEARANCE_BOUNDS =
{
    (unitless) : [0, 3, 500]
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

const HOLE_TAPERED_PIPE_TAP_ANGLE = 1.789911;
const HOLE_TAPERED_PIPE_TAP_CLEARANCE = 5;

/**
 * Angle bounds for a tapered pipe tap angle.
 */
const HOLE_TAPPED_ANGLE_BOUNDS =
{
            (degree) : [0.0, HOLE_TAPERED_PIPE_TAP_ANGLE, 9.9],
            (radian) : 0.0 * PI
        } as AngleBoundSpec;

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
                assignSheetMetalHoleAttributes(context, id, createdUsingNewHolePipeline, attributeId, holeEdgesQ,
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
                "bodyType" : ExtendedToolBodyType.SOLID,
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
function assignSheetMetalHoleAttributesForInstance(context is Context, topLevelId is Id, createdUsingNewHolePipeline is boolean,
    attributeId is string, smHoleEdgeQueries is map, instanceTopology is Query, featureDefinition is map,
    holeNumber is number) returns boolean
{
    // When doing the sheet metal boolean as a batch subtraction, we have access to `allHoleEdges` (all the edges created by
    // all the holes on the underlying sheet metal models) and `instanceTopology` (all the topology created by this single
    // hole instance). Intersecting these queries gives us the underlying hole edges for just this specific hole instance.
    const circularHoleEdgesForInstance = qIntersection([smHoleEdgeQueries.circularEdges, instanceTopology]);
    assignSheetMetalHoleAttributes(context, topLevelId, createdUsingNewHolePipeline, attributeId, circularHoleEdgesForInstance,
        featureDefinition, holeNumber);

    const allEdgesForInstance = qIntersection([smHoleEdgeQueries.allEdges, instanceTopology]);
    return !isQueryEmpty(context, allEdgesForInstance);
}

function assignSheetMetalHoleAttributes(context is Context, topLevelId is Id, createdUsingNewHolePipeline is boolean,
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
                createAttributesForSheetMetalHole(context, topLevelId, createdUsingNewHolePipeline, attributeId, circularHoleEdge,
                    holeFacesQ, featureDefinition, holeNumber);
            }
        }
    }
}

function createAttributesForSheetMetalHole(context is Context, topLevelId is Id, createdUsingNewHolePipeline is boolean,
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
        holeAttribute = createHoleAttribute(context, createdUsingNewHolePipeline, attributeId, featureDefinition,
            HoleSectionFaceType.THROUGH_FACE, holeNumber, featureDefinition);
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2041_SAME_HOLE_ON_SHEET_METAL_ERROR))
        {
            try silent
            {
                setAttribute(context, { "entities" : qUnion([holeEdge, holeFaces]), "attribute" : holeAttribute });
            }
            catch
            {
                reportFeatureInfo(context, topLevelId, ErrorStringEnum.HOLE_PARTIAL_FAILURE);
            }
        }
        else
        {
            setAttribute(context, { "entities" : qUnion([holeEdge, holeFaces]), "attribute" : holeAttribute });
        }
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

function getHoleFaces(context is Context, opHoleId is Id, faceTypeToSectionFaceType is map, holeIdentity is Query, faceOwnerBodies is Query) returns map
{
    var faceTypes = {};
    var sectionFaceTypes = {};
    var faceToSectionFaceType = {};
    for (var faceTypeWrapper in HoleFaceType)
    {
        const faceType = faceTypeWrapper.value;
        // May be undefined if we do not care about this face for the purpose of hole attribution.  For example, CAP faces.
        const sectionFaceType = faceTypeToSectionFaceType[faceType];

        const faces = evaluateQuery(context, qOpHoleFace(opHoleId, { "name" : faceTypeToFaceTypeData[faceType].name, "identity" : holeIdentity })->qOwnedByBody(faceOwnerBodies));
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

    return { "faceTypes" : faceTypes, "sectionFaceTypes" : sectionFaceTypes, "faceToSectionFaceType" : faceToSectionFaceType };
}

// Create attributes for a single hole created using opHole.  `userDefinedHoleDepth` can be undefined for THROUGH holes.
// Returns whether any faces were created by this hole.
function createAttributesFromQuery(context is Context, topLevelId is Id, opHoleId is Id, featureDefinition is map,
    finalPositionReference is HolePositionReference, attributeId is string, faceTypeToSectionFaceType is map,
    holeIdentity is Query, singleHoleReturnValue is map, holeIndex is number, userDefinedHoleDepth, wallToCuttingToolBodyIds) returns boolean
{
    if (isQueryEmpty(context, qOpHoleFace(opHoleId, { "identity" : holeIdentity })))
    {
        return false;
    }

    // Check if optional profiles have been skipped
    // If so, reset Clearance params to default
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2335_HOLE_FASTENER_FIT_FIX) &&
        featureDefinition.hasClearance &&
        singleHoleReturnValue.positionReferenceInfo[HolePositionReference.LAST_TARGET_START_IN_DEPTH] == undefined)
    {
        featureDefinition.hasClearance = false;
        featureDefinition.holeDiameter = featureDefinition.tapDrillDiameter;
        featureDefinition = copyToleranceInfo(featureDefinition, featureDefinition, "tapDrillDiameter", "holeDiameter");
        reportFeatureInfo(context, topLevelId, ErrorStringEnum.HOLE_FASTENER_FIT_IS_NOT_APPLICABLE);
    }

    var tappedDepthreadjusted = false;
    var isTappedHole = false;
    if (featureDefinition.endStyle == HoleEndStyle.UP_TO_ENTITY || featureDefinition.endStyle == HoleEndStyle.UP_TO_NEXT)
    {
        featureDefinition.holeDepth = singleHoleReturnValue.holeDepth;

        if (!featureDefinition.isMultiple)
        {
            setFeatureComputedParameter(context, topLevelId, { "name" : "holeDepthComputed", "value" : featureDefinition.holeDepth });
        }

        if (!featureDefinition.hasClearance && featureDefinition.tappedDepth > featureDefinition.holeDepth)
        {
            tappedDepthreadjusted = true;
            featureDefinition.tappedDepth = featureDefinition.holeDepth;
        }
    }

    const smCuttingToolMaps = getSheetMetalCuttingToolMaps(context, wallToCuttingToolBodyIds);
    const cuttingToolToSMDefintionBody = smCuttingToolMaps.cuttingToolToSMDefintionBody;
    const cuttingToolToSM3dBody = smCuttingToolMaps.cuttingToolToSM3dBody;

    const targetBodies = evaluateQuery(context, qOwnerBody(qOpHoleFace(opHoleId, { "identity" : holeIdentity })));

    //Group sheet metal hidden patch target bodies by user-visible 3d folded part bodies
    const smHiddenPatchMaps = getSheetMetalHiddenPatchMaps(context, targetBodies);
    const hiddenPatchToSM3dBody = smHiddenPatchMaps.hiddenPatchToSM3dBody;
    const sm3dBodyToHiddenPatches = smHiddenPatchMaps.sm3dBodyToHiddenPatches;
    var nonCuttingToolFaceAttributed = false;

    // Split by part
    var hiddenPatchesOfSM3dBodyProcessed = {};
    for (var target in targetBodies)
    {
        var faceOwnerBodies;
        var smDefinitionEntities = [];
        if (cuttingToolToSM3dBody[target.transientId] != undefined)
        {
            faceOwnerBodies = cuttingToolToSM3dBody[target.transientId];
        }
        else if (hiddenPatchToSM3dBody[target] != undefined)
        {
            nonCuttingToolFaceAttributed = true;
            if (hiddenPatchesOfSM3dBodyProcessed[hiddenPatchToSM3dBody[target]] == undefined)
            {
                hiddenPatchesOfSM3dBodyProcessed[hiddenPatchToSM3dBody[target]] = true;
                faceOwnerBodies = sm3dBodyToHiddenPatches[hiddenPatchToSM3dBody[target]];
                smDefinitionEntities = getSMDefinitionEntities(context, hiddenPatchToSM3dBody[target]);
            }
            else
            {
                continue;
            }
        }
        else
        {
            nonCuttingToolFaceAttributed = true;
            faceOwnerBodies = target;
            smDefinitionEntities = getSMDefinitionEntities(context, target);
        }

        var faces = getHoleFaces(context, opHoleId, faceTypeToSectionFaceType, holeIdentity, faceOwnerBodies);
        var faceTypes = faces.faceTypes;
        var sectionFaceTypes = faces.sectionFaceTypes;
        var faceToSectionFaceType = faces.faceToSectionFaceType;

        const finalPositionReferenceInfo = singleHoleReturnValue.positionReferenceInfo[finalPositionReference];
        var depthExtremes = singleHoleReturnValue.targetToDepthExtremes[target];
        if (depthExtremes == undefined && size(smDefinitionEntities) == 1)
        {
            depthExtremes = singleHoleReturnValue.targetToDepthExtremes[smDefinitionEntities[0]];
        }
        if (depthExtremes == undefined && cuttingToolToSMDefintionBody[target.transientId] != undefined)
        {
            depthExtremes = singleHoleReturnValue.targetToDepthExtremes[cuttingToolToSMDefintionBody[target.transientId]];
        }
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2127_HOLE_CLEAR_FS_NOTICES) &&
            (depthExtremes == undefined || depthExtremes == {}))
        {
            return false;
        }

        const firstEntranceInFinalPositionReferenceSpace = depthExtremes.firstEntrance - finalPositionReferenceInfo.referenceRootEnd;
        const fullEntranceInFinalPositionReferenceSpace = depthExtremes.fullEntrance - finalPositionReferenceInfo.referenceRootEnd;
        const fullExitInFinalPositionReferenceSpace = depthExtremes.fullExit - finalPositionReferenceInfo.referenceRootEnd;

        var entranceInFinalPositionReferenceSpace;
        if (featureDefinition.startStyle != HoleStartStyle.PART && isAtVersionOrLater(context, FeatureScriptVersionNumber.V1902_START_FROM_SKETCH_MEASURE))
        {
            // When starting from sketch plane, ensure that we do not have negative values for depth by measuring
            // from where the hole cylinder starts to intersect the part, rather than from where it fully intersects
            // the part. This is not ideal for holes that enter the part on a slanted face, but is a good compromise
            // for holes that interact with complex target geometry (such as a large-radius shallow hole being
            // drilled into a a position where there is already a small-radius deep hole)
            entranceInFinalPositionReferenceSpace = firstEntranceInFinalPositionReferenceSpace;
        }
        else
        {
            // The ideal measurement criteria: measure distance from where the hole cylinder fully intersects the part.
            entranceInFinalPositionReferenceSpace = fullEntranceInFinalPositionReferenceSpace;
        }

        var depthInPart;
        var tappedDepthInPart;
        if (userDefinedHoleDepth != undefined)
        {
            depthInPart = userDefinedHoleDepth - entranceInFinalPositionReferenceSpace;
        }

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2060_HOLE_ADDED_END_CONDITIONS) && featureDefinition.endStyle == HoleEndStyle.THROUGH)
        {
            tappedDepthInPart = featureDefinition.tappedDepth - depthExtremes.fullEntrance;
        }

        var isLastTarget = false;
        var lastTargetReference = HolePositionReference.LAST_TARGET_START;
        if (featureDefinition.hasClearance &&
            (featureDefinition.endStyle != HoleEndStyle.THROUGH && !isAtVersionOrLater(context, FeatureScriptVersionNumber.V2335_HOLE_FASTENER_FIT_FIX) ||
            isAtVersionOrLater(context, FeatureScriptVersionNumber.V2335_HOLE_FASTENER_FIT_FIX)))
        {
            lastTargetReference = HolePositionReference.LAST_TARGET_START_IN_DEPTH;
        }

        if (singleHoleReturnValue.positionReferenceInfo[lastTargetReference] != undefined)
        {
            isLastTarget = ((target == singleHoleReturnValue.positionReferenceInfo[lastTargetReference].target) ||
                            (size(smDefinitionEntities) == 1 &&
                             smDefinitionEntities[0] == singleHoleReturnValue.positionReferenceInfo[lastTargetReference].target) ||
                            (cuttingToolToSMDefintionBody[target.transientId] != undefined &&
                             cuttingToolToSMDefintionBody[target.transientId] == singleHoleReturnValue.positionReferenceInfo[lastTargetReference].target));
        }

        if (featureDefinition.hasClearance && isLastTarget && featureDefinition.endStyle == HoleEndStyle.THROUGH)
        {
            const depthInPartThrough = depthExtremes.fullExit - depthExtremes.firstEntrance;
            featureDefinition.isTappedThrough = featureDefinition.isTappedThrough || featureDefinition.tappedDepth > depthInPartThrough;
        }

        var featureDefinitionForAttribute = adjustDefinitionForAttribute(context, featureDefinition, sectionFaceTypes, isLastTarget, depthInPart, tappedDepthInPart);

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

        if (cuttingToolToSM3dBody[target.transientId] != undefined)
        {
            faces = getHoleFaces(context, opHoleId, faceTypeToSectionFaceType, holeIdentity, target);
            faceTypes = faces.faceTypes;
            sectionFaceTypes = faces.sectionFaceTypes;
            faceToSectionFaceType = faces.faceToSectionFaceType;
        }

        for (var faceAndSectionFaceType in faceToSectionFaceType)
        {
            const face = faceAndSectionFaceType.key;
            clearHoleAttributes(context, face);
            if (smDefinitionEntities != [] && hiddenPatchToSM3dBody[target] == undefined &&
                isAtVersionOrLater(context, FeatureScriptVersionNumber.V2250_SM_COUNTER_HOLE_BUG_FIXES))
            {
                clearHoleAttributes(context, qCorrespondingInFlat(face));
            }
            const createdUsingNewHolePipeline = true;
            var holeAttribute = createHoleAttribute(context, createdUsingNewHolePipeline, attributeId,
                featureDefinitionForAttribute, faceAndSectionFaceType.value, holeIndex, featureDefinition);
            if (holeAttribute != undefined)
            {
                if (featureDefinition.hasClearance)
                {
                    holeAttribute.isTappedHole = isLastTarget;
                    if (holeAttribute.isTappedHole && depthInPart != undefined && featureDefinition.tappedDepth > depthInPart)
                    {
                        tappedDepthreadjusted = true;
                        holeAttribute.tappedDepth = depthInPart;
                    }
                    holeAttribute.isClearanceAndTapped = holeAttribute.isTappedHole && featureDefinition.endStyle == HoleEndStyle.BLIND;
                }

                // Adjust `isTappedThrough`
                if ((holeAttribute.isTappedHole == true || holeAttribute.isStraightPipeTapHole) && (featureDefinition.endStyle != HoleEndStyle.THROUGH || isAtVersionOrLater(context, FeatureScriptVersionNumber.V2060_HOLE_ADDED_END_CONDITIONS))) // If the hole style is through, isTappedThrough is set explicitly
                {
                    // BEL-141916: This check for this if statement should be
                    // holeAttribute.isTappedHole == true && holeAttribute.endType == HoleEndStyle.THROUGH && !holeAttribute.isTappedThrough
                    // but it cannot be changed easily because fullExitInFinalPositionReferenceSpace does not represent
                    // the correct value for THROUGH holes; to get the correct value we would need more info from opHole.

                    isTappedHole = true;
                    var isTappedThrough;
                    if (tappedDepthInPart != undefined && (featureDefinition.endStyle == HoleEndStyle.THROUGH && !featureDefinition.isTappedThrough))
                    {
                        isTappedThrough = tappedDepthInPart > (fullExitInFinalPositionReferenceSpace - entranceInFinalPositionReferenceSpace - (TOLERANCE.zeroLength * meter));
                    }
                    else if (featureDefinition.endStyle == HoleEndStyle.BLIND || featureDefinition.endStyle == HoleEndStyle.BLIND_IN_LAST)
                    {
                        isTappedThrough = featureDefinition.tappedDepth > (fullExitInFinalPositionReferenceSpace - (TOLERANCE.zeroLength * meter));
                    }
                    else if (featureDefinition.endStyle == HoleEndStyle.UP_TO_ENTITY || featureDefinition.endStyle == HoleEndStyle.UP_TO_NEXT)
                    {
                        const firstEntranceDelta = abs(featureDefinition.holeDepth - userDefinedHoleDepth - finalPositionReferenceInfo.referenceRootEnd);
                        isTappedThrough = featureDefinitionForAttribute.endStyle == HoleEndStyle.THROUGH && featureDefinition.tappedDepth > (depthExtremes.fullExit - firstEntranceDelta - (TOLERANCE.zeroLength * meter));
                    }
                    if (isTappedThrough != undefined)
                    {
                        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1829_HOLE_IS_TAPPED_THROUGH))
                        {
                            holeAttribute = holeAttribute->setIsTappedThroughAndFixTappedDepth(isTappedThrough);
                        }
                        else
                        {
                            holeAttribute.isTappedThrough = isTappedThrough;
                        }
                    }
                }

                var hasThreadData = (holeAttribute.tappedDepth != undefined) && (holeAttribute.threadPitch != undefined);
                var isTapped = isTappedHole || holeAttribute.isTaperedPipeTapHole == true || holeAttribute.isStraightPipeTapHole;
                var cosmeticThreadData = undefined;
                if (hasThreadData && isTapped && faceAndSectionFaceType.value == HoleSectionFaceType.THROUGH_FACE)
                {
                    const threadOrigin = evVertexPoint(context, {
                        "vertex" : holeIdentity
                    });
                    const threadedSurface = evSurfaceDefinition(context, { "face" : face });
                    var threadCoordSys = threadedSurface.coordSystem;
                    threadCoordSys.origin = threadOrigin;

                    var threadDepth = holeAttribute.tappedDepth.value;

                    // Tapped depth is adjusted based on the origin and parameters of the hole feature. If the hole
                    // isn't deemed to tap through the part, add the distance between the thread origin and the full
                    // entrance point. This ensures the thread will render correctly for holes on sloped surfaces,
                    // that go through multiple parts, and that were created from points on offset planes.
                    if (threadDepth > TAPPED_DEPTH_FOR_TAPPED_THROUGH.value)
                    {
                        threadDepth += depthExtremes.fullEntrance.value;
                    }

                    cosmeticThreadData = createCosmeticThreadDataFromEntity(threadCoordSys, threadDepth,
                        holeAttribute.threadPitch.value);
                    addCosmeticThreadAttribute(context, face, cosmeticThreadData);
                }

                setAttribute(context, { "entities" : face, "attribute" : holeAttribute });
                if (smDefinitionEntities != [] && hiddenPatchToSM3dBody[target] == undefined)
                {
                    setAttribute(context, { "entities" : qCorrespondingInFlat(face), "attribute" : holeAttribute});
                    if (cosmeticThreadData != undefined)
                    {
                        addCosmeticThreadAttribute(context, qCorrespondingInFlat(face), cosmeticThreadData);
                    }
                }
            }
        }
    }

    if (tappedDepthreadjusted && isTappedHole)
    {
        reportFeatureInfo(context, topLevelId, ErrorStringEnum.HOLE_TAP_TOO_DEEP);
    }

    if ((featureDefinition.endStyle == HoleEndStyle.UP_TO_NEXT || featureDefinition.endStyle == HoleEndStyle.UP_TO_ENTITY) &&
        featureDefinition.style != HoleStyle.SIMPLE)
    {
        const cSinkDepth = (featureDefinition.cSinkDiameter / 2) / tan(featureDefinition.cSinkAngle / 2);
        if (featureDefinition.style == HoleStyle.C_BORE && featureDefinition.cBoreDepth > featureDefinition.holeDepth + TOLERANCE.zeroLength * meter)
        {
            throw regenError(ErrorStringEnum.HOLE_CBORE_TOO_DEEP, ["holeDepth", "cBoreDepth"]);
        }
        else if (featureDefinition.style == HoleStyle.C_SINK && cSinkDepth > featureDefinition.holeDepth + TOLERANCE.zeroLength * meter
            && !isAtVersionOrLater(context, FeatureScriptVersionNumber.V2558_HOLE_DISABLE_CSINK_DEPTH_CHECK))
        {
            throw regenError(ErrorStringEnum.HOLE_CSINK_TOO_DEEP, ["holeDepth", "cSinkDepth"]);
        }
    }

    return nonCuttingToolFaceAttributed;
}

function createAttributesFromTracking(context is Context, attributeId is string, holeDefinition is map,
    holeNumber is number, sketchTracking is array, startDistances is array, holeDepth)
{
    sketchTracking = filter(sketchTracking, track => track.trackingQuery != undefined);

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

        var holeDefinitionForAttribute = adjustDefinitionForAttribute(context, holeDefinition, sectionFaceTypes, part == lastBody, depthInPart, undefined);

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
            var holeAttribute = createHoleAttribute(context, createdUsingNewHolePipeline, attributeId,
            holeDefinitionForAttribute, entry.value, holeNumber, holeDefinition);
            if (holeAttribute != undefined)
            {
                // Adjust `isTappedThrough`
                if ((holeAttribute.isTappedHole == true || holeAttribute.isStraightPipeTapHole) && holeDefinition.endStyle != HoleEndStyle.THROUGH) // If the hole style is thorugh, isTappedThrough is set explicitly
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
function adjustDefinitionForAttribute(context is Context, featureDefinition is map, sectionFaceTypes is map, isLastTarget is boolean, depthInPart, tappedDepthInPart) returns map
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
        if (!featureDefinition.hasClearance && featureDefinition.tappedDepth != undefined)
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
    if ((isLastTarget && (featureDefinition.hasClearance || tappedHoleWithOffset(featureDefinition)))) // If this is a tapped hole, adjust diameter
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1947_TAP_DRILL_DIAMETER_FIX))
        {
            modifiedFeatureDefinition = copyToleranceInfo(featureDefinition, modifiedFeatureDefinition, "tapDrillDiameter", "holeDiameter");
        }
        modifiedFeatureDefinition.holeDiameter = featureDefinition.tapDrillDiameter;
    }
    if (featureDefinition.endStyle == HoleEndStyle.UP_TO_ENTITY || featureDefinition.endStyle == HoleEndStyle.UP_TO_NEXT)
    {
        if (featureDefinition.isMultiple)
        {
            modifiedFeatureDefinition = copyToleranceInfo(featureDefinition, modifiedFeatureDefinition, "holeDepthMultiple", "holeDepth");
        }
        else
        {
            modifiedFeatureDefinition = copyToleranceInfo(featureDefinition, modifiedFeatureDefinition, "holeDepthComputed", "holeDepth");
        }
    }

    if (!featureDefinition.hasClearance && tappedDepthInPart != undefined)
    {
        modifiedFeatureDefinition.tappedDepth = tappedDepthInPart;
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

function isTappedDepthPositive(context is Context, holeDefinition is map) returns boolean
{
    const depthThreshold = (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2617_HOLE_CALLOUTS_FIX) ? TOLERANCE.zeroLength : 0) * meter;
    const isPositiveDepth = holeDefinition.tappedDepth > depthThreshold;

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1829_HOLE_IS_TAPPED_THROUGH))
    {
        return isPositiveDepth || holeDefinition.isTappedThrough;
    }
    else if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1801_HOLE_TAPPED_ONLY_IF_TAPPED_DEPTH_POSITIVE))
    {
        return isPositiveDepth;
    }
    else
    {
        return true;
    }
}

// holeMap can be a hole definition or a hole attribute
function setIsTappedThroughAndFixTappedDepth(holeMap is map, isTappedThrough is boolean) returns map
{
    holeMap.isTappedThrough = isTappedThrough;
    if (isTappedThrough)
    {
        holeMap.tappedDepth = TAPPED_DEPTH_FOR_TAPPED_THROUGH;
    }
    return holeMap;
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
function createHoleAttribute(context is Context, createdUsingNewHolePipeline is boolean, attributeId is string, holeDefinitionForAttribute is map,
    holeFaceType is HoleSectionFaceType, holeNumber is number, definition is map) returns HoleAttribute
{
    // make the base hole attribute
    var holeAttribute = makeHoleAttribute(createdUsingNewHolePipeline, attributeId, holeDefinitionForAttribute.style);

    // add tag info
    holeAttribute.holeNumber = holeNumber;
    holeAttribute.holeFeatureCount = holeDefinitionForAttribute.holeFeatureCount;

    // add common properties
    holeAttribute = addCommonAttributeProperties(context, holeAttribute, holeDefinitionForAttribute, definition);

    // add properties specific to precision and tolerance
    holeAttribute = addToleranceAttributeProperties(context, holeAttribute, holeDefinitionForAttribute);

    // add properties specific to the section (for example, properties needed for the cBore diameter if this is the cBore diameter section)
    holeAttribute = addSectionSpecsToAttribute(holeAttribute, holeFaceType, holeDefinitionForAttribute);

    return holeAttribute;
}

function addCommonAttributeProperties(context is Context, attribute is HoleAttribute, holeDefinitionForAttribute is map, definition is map) returns HoleAttribute
{
    var resultAttribute = attribute;

    // Through, Blind or Blind in Last
    resultAttribute.endType = holeDefinitionForAttribute.endStyle;
    resultAttribute.showTappedDepth = holeDefinitionForAttribute.showTappedDepth;
    resultAttribute.majorDiameter = holeDefinitionForAttribute.majorDiameter;

    // Update: later in the function, include this parameter only if it is relevant.
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V1829_HOLE_IS_TAPPED_THROUGH))
    {
        resultAttribute.isTappedThrough = holeDefinitionForAttribute.isTappedThrough;
    }

    // Through hole diameter
    resultAttribute.holeDiameter = holeDefinitionForAttribute.holeDiameter;

    if (resultAttribute.endType == HoleEndStyle.THROUGH)
    {
        resultAttribute.partialThrough = holeDefinitionForAttribute.partialThrough;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2317_HOLE_PARTIAL_THROUGH_FIX) && resultAttribute.partialThrough == true)
        {
            resultAttribute.holeDepth = holeDefinitionForAttribute.holeDepth;
        }
    }
    else
    {
        // blind hole depth
        resultAttribute.holeDepth = holeDefinitionForAttribute.holeDepth;
        resultAttribute.tipAngle = holeDefinitionForAttribute.tipAngle;
    }

    // initialize tapped hole information
    resultAttribute.isTappedHole = false;
    resultAttribute.isTaperedPipeTapHole = false;
    resultAttribute.isStraightPipeTapHole = false;
    resultAttribute.tappedAngle = 0.0;
    resultAttribute.tapSize = "";
    var tapSize;
    var tapPitch;
    var standard;
    var tapClass;
    var threadType;
    var isStandardComponentBasedHole = false;
    var isStandardDrillBasedHole = false;
    var standardSizeDesignation = undefined;

    var standardSpec = getActiveTable(definition.isV2 ? definition : holeDefinitionForAttribute);

    // determine if tapped hole and setup tapped hole details
    if (standardSpec != undefined)
    {
        if (holeDefinitionForAttribute.isV2)
        {
            standard = holeDefinitionForAttribute.unitsSystem == UnitsSystem.INCH ? "ANSI" : "ISO";
            for (var entry in standardSpec)
            {
                if (entry.key == "type")
                {
                    isStandardComponentBasedHole = true;
                    if (match(entry.value, ".*[Ss]traight tap.*").hasMatch)
                    {
                        resultAttribute.isTappedHole = isTappedDepthPositive(context, holeDefinitionForAttribute);
                    }
                    else if (match(entry.value, ".*[Ss]traight [Pp]ipe [Tt]ap.*").hasMatch)
                    {
                        resultAttribute.isStraightPipeTapHole = isTappedDepthPositive(context, holeDefinitionForAttribute);
                    }
                    else if (match(entry.value, ".*[Tt]apered [Pp]ipe [Tt]ap.*").hasMatch)
                    {
                        resultAttribute.isTaperedPipeTapHole = isTappedDepthPositive(context, holeDefinitionForAttribute);
                    }
                }
                else if (entry.key == "holeType")
                {
                    isStandardComponentBasedHole = true;
                    if (match(entry.value, ".*[Dd]rilled.*").hasMatch)
                    {
                        // drilled holes are based upon a drill size, not a component size
                        isStandardComponentBasedHole = false;
                        isStandardDrillBasedHole = true;
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
                else if (entry.key == "threadType")
                {
                    threadType = entry.value;
                }
                else if (entry.key == "class" && !isAtVersionOrLater(context, FeatureScriptVersionNumber.V2356_HOLE_ADDED_THREAD_FORM_ANNOTATION))
                {
                    tapClass = entry.value;
                }
            }
        }
        else
        {
            for (var entry in standardSpec)
            {
                if (entry.key == "standard")
                {
                    standard = entry.value;
                }
                else if (entry.key == "type")
                {
                    isStandardComponentBasedHole = true;
                    if (match(entry.value, ".*[Tt]apped.*").hasMatch)
                    {
                        resultAttribute.isTappedHole = isTappedDepthPositive(context, holeDefinitionForAttribute);
                    }
                    else if (match(entry.value, ".*[St]raight [Pp]ipe [Tt]ap.*").hasMatch)
                    {
                        resultAttribute.isStraightPipeTapHole = isTappedDepthPositive(context, holeDefinitionForAttribute);
                    }
                    else if (match(entry.value, ".*[Tt]apered [Pp]ipe [Tt]ap.*").hasMatch)
                    {
                        resultAttribute.isTaperedPipeTapHole = isTappedDepthPositive(context, holeDefinitionForAttribute);
                    }
                    else if (match(entry.value, ".*[Dd]rilled.*").hasMatch)
                    {
                        // drilled holes are based upon a drill size, not a component size
                        isStandardComponentBasedHole = false;
                        isStandardDrillBasedHole = true;
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

    // is this a tapped or tapered pipe tap hole and we found its size?
    if ((resultAttribute.isTappedHole || resultAttribute.isTaperedPipeTapHole || resultAttribute.isStraightPipeTapHole) && tapSize != undefined && tapPitch != undefined)
    {
        // format tap pitch based upon units
        const pitchWithUnits = computePitchValue(context, tapPitch);
        const pitchAnnotation = buildPitchAnnotation(context, tapPitch);

        // set tap size
        resultAttribute.tapSize = tapSize ~ pitchAnnotation;
        if (resultAttribute.isStraightPipeTapHole)
            resultAttribute.tapSize ~= " " ~ (standard == "ANSI" ? "BSPP" : (threadType ~ " TAPPED HOLE"));

        if (resultAttribute.isTaperedPipeTapHole)
            resultAttribute.tapSize ~= standard == "ANSI" ? " NPT" : " RC TAPPED HOLE";

        // set tappedDepth and isTappedThrough
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1829_HOLE_IS_TAPPED_THROUGH))
        {
            resultAttribute.tappedDepth = holeDefinitionForAttribute.tappedDepth;
            resultAttribute = resultAttribute->setIsTappedThroughAndFixTappedDepth(holeDefinitionForAttribute.isTappedThrough);
        }
        else
        {
            // resultAttribute.isTappedThrough has already been set earlier in the function.

            if (holeDefinitionForAttribute.tappedDepth != undefined)
            {
                // This is likely always called and will put a hidden, irrelevant value into tappedDepth when
                // isTappedThrough is true.  It may not be called in some legacy cases.
                resultAttribute.tappedDepth = holeDefinitionForAttribute.tappedDepth;
            }
            else if (holeDefinitionForAttribute.isTappedThrough)
            {
                // This is likely never called because tappedDepth is always set to something, even when hidden.
                // It may be called in some legacy cases, it is left in for safety.
                resultAttribute.tappedDepth = TAPPED_DEPTH_FOR_TAPPED_THROUGH;
            }
        }

        if ((resultAttribute.isTaperedPipeTapHole || resultAttribute.isStraightPipeTapHole) && holeDefinitionForAttribute.tappedAngle != undefined)
            resultAttribute.tappedAngle = holeDefinitionForAttribute.tappedAngle;

        if (resultAttribute.endType != HoleEndStyle.THROUGH &&
            resultAttribute.endStyle != HoleEndStyle.UP_TO_NEXT &&
            resultAttribute.endStyle != HoleEndStyle.UP_TO_ENTITY &&
            holeDefinitionForAttribute.tapClearance != undefined)
            resultAttribute.tapClearance = holeDefinitionForAttribute.tapClearance;

        resultAttribute.threadPitch = pitchWithUnits;
    }

    if (tapSize != undefined)
    {
        if (tapClass != undefined)
        {
            resultAttribute.tapSize ~= " - " ~ tapClass;
        }
        else if ((!holeDefinitionForAttribute.isV2 || isAtVersionOrLater(context, FeatureScriptVersionNumber.V2356_HOLE_ADDED_THREAD_FORM_ANNOTATION)) &&
            holeDefinitionForAttribute.showThreadClass)
        {
            const table = getThreadClassTable(holeDefinitionForAttribute);
            if (table != undefined)
            {
                for (var entry in table)
                {
                    if (entry.key == "class")
                    {
                        resultAttribute.tapSize ~= " - " ~ entry.value;
                    }
                }
            }
        }
    }

    // add properties specific to the hole type
    if (holeDefinitionForAttribute.style == HoleStyle.SIMPLE)
    {
        resultAttribute = addSimpleHoleAttributeProperties(resultAttribute, holeDefinitionForAttribute);
    }
    else if (holeDefinitionForAttribute.style == HoleStyle.C_BORE)
    {
        resultAttribute = addCBoreHoleAttributeProperties(resultAttribute, holeDefinitionForAttribute);
    }
    else if (holeDefinitionForAttribute.style == HoleStyle.C_SINK)
    {
        resultAttribute = addCSinkHoleAttributeProperties(resultAttribute, holeDefinitionForAttribute);
    }

    return resultAttribute;
}

function addToleranceForField(context is Context, tolerances is map, field is string, definition is map) returns map
{
    var tolerance = getToleranceInfo(definition, field);

    // Do not include the tolerance info if it is set to all default values
    if (isToleranceSet(tolerance))
    {
        // Consumers of the hole attribute don't yet have the change to how lower bounds are handled
        // Changing the hole attribute is a hacky fix
        // This is a slightly odd-looking way to flip the lower tolerance if it's a new feature
        if (tolerance.lower != undefined)
        {
            tolerance.lower = -tolerance.lower;
            tolerance = flipLowerBoundIfOldFeature(context, tolerance);
        }
        tolerances[field] = tolerance;
    }

    return tolerances;
}

function addToleranceAttributeProperties(context is Context, attribute is HoleAttribute, holeDefinition is map) returns HoleAttribute
{
    var tolerances = {};

    const tolerancedFields = getTolerancedFields(holeDefinition);

    for (var field in tolerancedFields)
    {
        tolerances = addToleranceForField(context, tolerances, field, holeDefinition);
    }

    attribute.tolerances = tolerances;

    return attribute;
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
    else if (holeFaceType == HoleSectionFaceType.CLEARANCE_DIAMETER_FACE)
    {
        resultAttribute.sectionFace = getClearanceDiameterSectionAttributeSpecs(holeDefinition);
    }
    else if (holeFaceType == HoleSectionFaceType.CLEARANCE_DEPTH_FACE)
    {
        resultAttribute.sectionFace = getClearanceDepthSectionAttributeSpecs(holeDefinition);
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

function getClearanceDiameterSectionAttributeSpecs(holeDefinition is map) returns map
{
    return { "type" : HoleSectionFaceType.CLEARANCE_DIAMETER_FACE };
}

function getClearanceDepthSectionAttributeSpecs(holeDefinition is map) returns map
{
    return { "type" : HoleSectionFaceType.CLEARANCE_DEPTH_FACE };
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

function generateFeatureNameTemplate(context is Context, definition is map) returns string
{
    var tapSize;
    var tapPitch;
    var standard;
    var threadType;
    var isTappedHole = false;
    var isTaperedPipeTapHole = false;
    var isStraightPipeTapHole = false;

    var standardSpec = getActiveTable(definition);

    // determine if tapped hole and setup tapped hole details
    if (standardSpec != undefined)
    {
        standard = definition.isV2 ? (definition.unitsSystem == UnitsSystem.INCH ? "ANSI" : "ISO") : standardSpec["standard"];
        tapSize = standardSpec["size"];
        tapPitch = standardSpec["pitch"];
        threadType = standardSpec["threadType"];

        if (standardSpec["type"] != undefined)
        {
            if (match(standardSpec["type"], ".*[Ss]traight tap.*").hasMatch)
            {
                isTappedHole = isTappedDepthPositive(context, definition);
            }
            else if (match(standardSpec["type"], ".*[Ss]traight [Pp]ipe [Tt]ap.*").hasMatch)
            {
                isStraightPipeTapHole = isTappedDepthPositive(context, definition);
            }
            else if (match(standardSpec["type"], ".*[Tt]apered [Pp]ipe [Tt]ap.*").hasMatch)
            {
                isTaperedPipeTapHole = isTappedDepthPositive(context, definition);
            }
            else if (match(standardSpec["type"], ".*[Tt]apped.*").hasMatch)
            {
                isTappedHole = isTappedDepthPositive(context, definition);
            }
        }
    }

    if ((isTappedHole || isTaperedPipeTapHole || isStraightPipeTapHole) && tapSize != undefined && tapPitch != undefined)
    {
        var pitchAnnotation = buildPitchAnnotation(context, tapPitch);

        tapSize = replace(tapSize, "#", "##");
        tapSize = tapSize ~ pitchAnnotation;
        if (isTaperedPipeTapHole && standard != undefined)
            tapSize ~= standard == "ANSI" ? " NPT" : " RC TAPPED HOLE";
        else if (isStraightPipeTapHole && standard != undefined)
        {
            tapSize ~= " " ~ (standard == "ANSI" ? "NPT" : (threadType ~ " TAPPED HOLE"));
        }
    }

    var featureName = "";
    if ((isTappedHole || isTaperedPipeTapHole || isStraightPipeTapHole) && tapSize != undefined)
    {
        featureName ~= tapSize;
    }
    else
    {
        featureName ~= definition.isV2 ? "Ø #holeDiameterV2" : "Ø #holeDiameter";
    }

    if (definition.endStyle == HoleEndStyle.BLIND || definition.endStyle == HoleEndStyle.BLIND_IN_LAST)
    {
        featureName ~= " ↧ #holeDepth";
    }
    else if ((definition.endStyle == HoleEndStyle.UP_TO_ENTITY || definition.endStyle == HoleEndStyle.UP_TO_NEXT) && !definition.isMultiple)
    {
        featureName ~= " ↧ #holeDepthComputed";
    }
    else if (definition.endStyle == HoleEndStyle.THROUGH)
    {
        featureName ~= " THRU";
    }

    if (definition.style == HoleStyle.C_BORE)
    {
        featureName ~= " | ⌴Ø " ~ "#cBoreDiameter";
        featureName ~= " ↧ #cBoreDepth";
    }
    else if (definition.style == HoleStyle.C_SINK)
    {
        featureName ~= " | ⌵Ø " ~ "#cSinkDiameter";
        if (definition.cSinkAngle is string) // Evaluate if a value is an expression
        {
            definition.cSinkAngle = lookupTableEvaluate(definition.cSinkAngle);
        }
        featureName ~= " X " ~ toString(roundToPrecision(definition.cSinkAngle / degree, 2)) ~ "°";
    }
    return featureName;
}

/** @internal */
function syncHoleEndStyle(endStyle) returns HoleEndStyle
{
    if (endStyle is HoleEndStyleV2)
    {
        return switch (endStyle)
        {
            HoleEndStyleV2.BLIND : HoleEndStyle.BLIND,
            HoleEndStyleV2.UP_TO_NEXT : HoleEndStyle.UP_TO_NEXT,
            HoleEndStyleV2.UP_TO_ENTITY : HoleEndStyle.UP_TO_ENTITY,
            HoleEndStyleV2.THROUGH : HoleEndStyle.THROUGH
        };
    }
    else if (endStyle is HoleEndStyle)
    {
        return endStyle;
    }
    return HoleEndStyle.THROUGH;
}

/** @internal */
function syncHoleDefinitionV2Params(definition is map) returns map
{
    if (definition != {} && definition.isV2)
    {
        definition.style = definition.styleV2;
        definition.holeDiameter = definition.holeDiameterV2;
        definition.tapDrillDiameter = definition.tapDrillDiameterV2;
        definition.endStyle = syncHoleEndStyle(definition.endStyleV2);

        definition = copyToleranceInfo(definition, definition, "holeDiameterV2", "holeDiameter");
        definition = copyToleranceInfo(definition, definition, "tapDrillDiameterV2", "tapDrillDiameter");
        definition.showThreadClass = definition.showThreadClassV2;
        definition.ansiThreadClass = definition.ansiThreadClassV2;
        definition.isoThreadClass = definition.isoThreadClassV2;
    }
    return definition;
}

/**
 * @internal
 * Editing logic for hole feature.
 */
export function holeEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    // Preselection only
    if (oldDefinition == {} && !isQueryEmpty(context, definition.initEntities))
    {
        definition.locations = definition.initEntities;

        // Clear out the pre-selection data: this is especially important if the query is to imported data
        definition.initEntities = qNothing();
    }

    if (oldDefinition.locations != definition.locations)
    {
        definition.locations = qUnion(clusterVertexQueries(context, definition.locations));
    }

    if (isCreating)
    {
        definition.isV2 = true;
    }

    if (definition.isV2)
    {
        if (getActiveTable(oldDefinition) != getActiveTable(definition))
        {
            definition = updateHoleDefinitionWithStandard(oldDefinition, definition);
            definition.holeDiameterV2 = definition.holeDiameter;
            definition.tapDrillDiameterV2 = definition.tapDrillDiameter;
        }

        definition = syncHoleDefinitionV2Params(definition);
    }
    else
    {
        if (oldDefinition.standardTappedOrClearance != definition.standardTappedOrClearance ||
            oldDefinition.standardBlindInLast != definition.standardBlindInLast ||
            oldDefinition.endStyle != definition.endStyle)
        {
            definition = updateHoleDefinitionWithStandard(oldDefinition, definition);
        }
    }

    definition = adjustDepthAndThreadParameters(context, oldDefinition, definition, specifiedParameters);
    /* For Tapered Pipe Tap, the holeDepth and tappedDepth are also specified by the standard(ANSI, ISO).
       So we need to adjust the depths above before we check if the adjusted depths violate the standards below. */
    if (!definition.isV2)
    {
        definition = setToCustomIfStandardViolated(definition);
    }

    definition = holeScopeFlipHeuristicsCall(context, oldDefinition, definition, specifiedParameters, hiddenBodies);

    definition = updateThreadClassDefinition(context, definition);

    definition.isMultiple = false;
    if ((definition.endStyle == HoleEndStyle.UP_TO_ENTITY || definition.endStyle == HoleEndStyle.UP_TO_NEXT) && oldDefinition != {})
    {
        definition.isMultiple = size(evaluateQuery(context, definition.locations)) > 1;

        if (definition.isMultiple != oldDefinition.isMultiple)
        {
            if (definition.isMultiple)
            {
                definition = copyToleranceInfo(oldDefinition, definition, "holeDepthComputed", "holeDepthMultiple");
            }
            else
            {
                definition = copyToleranceInfo(oldDefinition, definition, "holeDepthMultiple", "holeDepthComputed");
            }
        }
    }
    if (definition.featureName != undefined)
    {
        definition.featureName = generateFeatureNameTemplate(context, definition);
    }

    for (var field in getTolerancedFields(definition))
    {
        definition = updateFitToleranceFields(context, id, definition, field);
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

/**
 *
 * Extract value from pitch string
 */
export function computePitchValue(context is Context, pitch is string)
{
    var result = parsePitch(context, pitch);
    if (result.hasMatch)
    {
        // Check for NN.N tpi or NN.N mm
        if (result.captures[2] == "tpi")
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2336_HOLE_PARSE_PITCH_FIX))
            {
                return inch / (lookupTableEvaluate(result.captures[1] ~ " * inch")) * inch;
            }
            return 1.0 / stringToNumber(result.captures[1]) * inch;
        }
        else if (result.captures[2] == "mm")
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2336_HOLE_PARSE_PITCH_FIX))
            {
                return lookupTableEvaluate(result.captures[1] ~ " * millimeter");
            }
            return stringToNumber(result.captures[1]) * millimeter;
        }
    }
    return undefined;
}

function computePitch(context is Context, definition is map)
{
    var standard = getStandardAndTable(definition).standard;
    if (standard != undefined && standard.pitch != undefined)
    {
        return computePitchValue(context, standard.pitch);
    }
    return undefined;
}

function checkIfHasClearance(definition is map)
{
    if (definition.isV2)
    {
        var standard = getStandardAndTable(definition).standard;
        if (standard != undefined && standard.fit != undefined)
        {
            return standard.fit != "None";
        }
    }
    return false;
}

/**
 * Adjust the `holeDepth`, `tappedDepth`, and `tapClearance` parameters to be consistent
 */
function adjustDepthAndThreadParameters(context is Context, oldDefinition is map, definition is map, specifiedParameters is map) returns map
{
    if (!definitionChangeAffectsDepthAndThreadParameters(oldDefinition, definition))
    {
        return definition;
    }

    const pitch = computePitch(context, definition);
    if (pitch == undefined)
    {
        definition.showTappedDepth = false;
        definition.hasClearance = false;
        return definition;
    }
    definition.showTappedDepth = true;
    definition.hasClearance = checkIfHasClearance(definition);

    if (!(definition.endStyle == HoleEndStyle.BLIND || definition.endStyle == HoleEndStyle.BLIND_IN_LAST))
    {
        return definition;
    }

    const fieldInfos = [
            {
                "fieldName" : "holeDepth",
                "precalculatedValue" : definition.tappedDepth + definition.tapClearance * pitch,
                "allowedToBeZero" : false
            }, {
                "fieldName" : "tappedDepth",
                "precalculatedValue" : definition.holeDepth - definition.tapClearance * pitch,
                "allowedToBeZero" : false
            }, {
                "fieldName" : "tapClearance",
                // Pitch should be non-zero if `computePitch` returns successfully above, so dividing by it should be ok
                "precalculatedValue" : (definition.holeDepth - definition.tappedDepth) / pitch,
                "allowedToBeZero" : true
            }
        ];
    var candidates = [];
    for (var fieldInfo in fieldInfos)
    {
        const valueIsBeingEdited = definition[fieldInfo.fieldName] != oldDefinition[fieldInfo.fieldName];
        // When this is the "initial edit" made by the system when opening the dialog, it is ok to edit even though the value is undergoing a change.
        const valueIsBeingEditedByUser = valueIsBeingEdited && oldDefinition[fieldInfo.fieldName] != undefined;

        var valueIsPositive = fieldInfo.precalculatedValue > 0;
        if (fieldInfo.allowedToBeZero)
        {
            valueIsPositive = valueIsPositive || fieldInfo.precalculatedValue == 0;
        }

        // Field is a candidate if it is not currently being edited, and is not going to be set to a negative number
        if (!valueIsBeingEditedByUser && valueIsPositive)
        {
            candidates = candidates->append(fieldInfo);
        }
    }

    if (candidates == [])
    {
        // No fields available to change. Let the feature issue a HOLE_INCONSISTENT_TAP_INFO warning
        return definition;
    }

    // Try to pick a parameter that has not been edited yet
    var fieldToChange = undefined;
    for (var i = size(candidates) - 1; i >= 0; i -= 1) // Go backwards to prioritize fields at the end
    {
        if (specifiedParameters[candidates[i].fieldName] == false)
        {
            fieldToChange = candidates[i];
        }
    }
    // Fall back on the last available field, if all the fields have been edited
    fieldToChange = last(candidates);

    definition[fieldToChange.fieldName] = fieldToChange.precalculatedValue;
    return definition;
}

function definitionChangeAffectsDepthAndThreadParameters(oldDefinition is map, definition is map) returns boolean
{
    return getActiveTable(oldDefinition) != getActiveTable(definition) || oldDefinition.standardBlindInLast != definition.standardBlindInLast ||
        oldDefinition.standardTappedOrClearance != definition.standardTappedOrClearance ||
        oldDefinition.endStyle != definition.endStyle ||
        oldDefinition.holeDepth != definition.holeDepth ||
        oldDefinition.tappedDepth != definition.tappedDepth ||
        oldDefinition.tapClearance != definition.tapClearance;
}

function getStandardAndTable(definition is map) returns map
{
    const table = getActiveLookupTable(definition);
    const standard = getActiveTable(definition);

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
        if (entry.key == "tappedDepth" || entry.key == "holeDepth")
        {
            definition[entry.key] = lookupTableEvaluate(entry.value);
        }
        else
        {
            definition[entry.key] = lookupTableFixExpression(entry.value);
        }
        evaluatedDefinition[entry.key] = lookupTableGetValue(definition[entry.key]);
    }

    if (isPipeTapHole(definition))
    {
        definition.tapClearance = HOLE_TAPERED_PIPE_TAP_CLEARANCE;
        definition.tappedAngle = HOLE_TAPERED_PIPE_TAP_ANGLE * degree;
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
        definition.showTappedDepth = false;
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

function calculateHoleDepth(context is Context, definition is map, axis is Line)
{
    if (definition.endStyle == HoleEndStyle.UP_TO_ENTITY && definition.endBoundEntity != undefined)
    {
        // Handle the planar face, plan or mate connector
        const planeOnLimit = try silent(evPlane(context, { "face" : definition.endBoundEntity }));
        if (planeOnLimit != undefined)
        {
            const intersect = intersection(planeOnLimit, axis);
            if (intersect.dim == 0)
            {
                return norm(axis.origin - intersect.intersection);
            }
            else
            {
                return undefined;
            }
        }
        // Handle a vertex
        const pointOnLimit = try silent(evVertexPoint(context, { "vertex" : definition.endBoundEntity }));
        if (pointOnLimit != undefined)
        {
            const plane = plane(pointOnLimit, axis.direction);
            const distanceResult = try silent(evDistance(context, {
                "side0" : axis.origin,
                "side1" : plane
            }));
            return distanceResult.distance;
        }
        // Handle a surface
        const raycastResults = try silent(evRaycast(context, {
            "entities" : definition.endBoundEntity,
            "ray" : axis,
            "closest" : false,
            "includeIntersectionsBehind" : false
        }));

        if (raycastResults != undefined && size(raycastResults) > 0)
        {
            return raycastResults[0].distance;
        }
    }
    else if (definition.endStyle == HoleEndStyle.UP_TO_NEXT)
    {
        axis.origin -= axis.direction * (TOLERANCE.zeroLength * 1000 * meter);
        const raycastResults = try silent(evRaycast(context, {
                "entities" : qEverything(EntityType.FACE)->qMeshGeometryFilter(MeshGeometry.NO)->qConstructionFilter(ConstructionObject.NO),
                "ray" : axis,
                "closest" : false,
                "includeIntersectionsBehind" : false
        }));

        if (raycastResults != undefined && size(raycastResults) > 1)
        {
            return raycastResults[1].distance;
        }
    }
    return undefined;
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

    const startLocationsHaveChanged = (definition.startStyle != oldDefinition.startStyle &&
        (definition.startStyle == HoleStartStyle.PLANE || oldDefinition.startStyle == HoleStartStyle.PLANE)) ||
        (definition.startStyle == HoleStartStyle.PLANE && definition.startBoundEntity != oldDefinition.startBoundEntity);
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

    if (!locationsHaveChanged && !raycastInputsHaveChanged && !startLocationsHaveChanged)
    {
        return definition;
    }

    // If the change that is being made is to incrementally add locations, just do a basic calculation to add necessary
    // targets to the scope.  Otherwise, fully recalculate the scope and flip.
    const allLocationsAreNew = (isQueryEmpty(context, qIntersection([oldDefinition.locations, definition.locations])));
    const comprehensive = raycastInputsHaveChanged || allLocationsAreNew || startLocationsHaveChanged;

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
    const startBoundEntity = definition.startStyle == HoleStartStyle.PLANE ? definition.startBoundEntity : qNothing();
    const newAxes = computeAxes(context, evaluateQuery(context, newLocations), definition.oppositeDirection, identityTransform(), startBoundEntity);

    // -- CAUTION: `definition` should not be passed into `raycastForScopeFlipResults`. Creating the barrier of
    // -- `raycastInputs` allows for an abstraction where any `definition` inputs needed for ray casting are extracted
    // -- in `extractRaycastInputs`, which exposes an `isEquivalent` function that allows us to see whether any of the
    // -- raycast inputs are changing (and do a comprehensive heuristic if so). If any additional `definition`
    // -- parameters are needed for `raycastForScopeFlipResults` they should be extracted in `extractRaycastInputs`
    // -- and added to `isEquivalent`.
    const scopeFlipResults = raycastForScopeFlipResults(context, raycastInputs, newAxes,
        comprehensive, canEditScope, canEditFlip, hiddenBodies, definition);
    definition.scope = scopeFlipResults.scope;
    definition.oppositeDirection = scopeFlipResults.oppositeDirection;
    return definition;
}

function extractRaycastInputs(definition is map)
{
    // THROUGH and BLIND_IN_LAST can each go infinitely far.  BLIND has a limit to how far it can go.
    const hasBoundLimit = (definition.endStyle == HoleEndStyle.UP_TO_ENTITY || definition.endStyle == HoleEndStyle.UP_TO_NEXT);
    const hasDepthLimit = (definition.endStyle == HoleEndStyle.BLIND);

    var depthLimit = hasDepthLimit ? definition.holeDepth : undefined;
    if (hasBoundLimit)
    {
        if (definition.holeDiameter is string == true) // Evaluate if a value is an expression
        {
            definition.holeDiameter = lookupTableEvaluate(definition.holeDiameter);
        }
        const shaftRadius = definition.holeDiameter / 2.0;
        depthLimit = shaftRadius / tan(definition.tipAngle / 2.0);
        if (definition.offset && hasBoundLimit)
        {
            depthLimit = (definition.oppositeOffsetDirection) ? definition.offsetDistance : -definition.offsetDistance;
        }
    }

    return {
            "oppositeDirection" : definition.oppositeDirection,
            "hasDepthLimit" : hasDepthLimit || hasBoundLimit,
            "depthLimit" : depthLimit,
            "scope" : definition.scope,
            "hasBoundLimit" : hasBoundLimit,
            "endBoundEntity" : definition.endStyle == HoleEndStyle.UP_TO_ENTITY ? definition.endBoundEntity : qNothing(),
            // `isEquivalent` is used to tell whether we need to do a comprehensive heuristic over all of the
            // locations, or an incremental heuristic over just the new locations. If adding additional parameters to
            // this map, the appropriate comparison should be added to `isEquivalent`.
            "isEquivalent" :
            function(context, self, other)
            {
                return self.oppositeDirection == other.oppositeDirection
                    && self.hasDepthLimit == other.hasDepthLimit
                    && (!self.hasDepthLimit || tolerantEquals(self.depthLimit, other.depthLimit))
                    && areQueriesEquivalent(context, self.scope, other.scope)
                    && self.hasBoundLimit == other.hasBoundLimit
                    && (!self.hasBoundLimit || areQueriesEquivalent(context, self.endBoundEntity, other.endBoundEntity));
            }
        };
}

function raycastForScopeFlipResults(context is Context, raycastInputs is map, axes is array,
    comprehensive is boolean, canEditScope is boolean, canEditFlip is boolean, hiddenBodies is Query, definition is map)
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
    const depthOffset = raycastInputs.depthLimit;
    for (var axis in axes)
    {
        if (remainingPossibleTargetSet == {})
        {
            // All possible targets have been consumed
            break;
        }

        if (definition.endStyle == HoleEndStyle.UP_TO_ENTITY || definition.endStyle == HoleEndStyle.UP_TO_NEXT)
        {
            raycastInputs.depthLimit = calculateHoleDepth(context, definition, axis);
            if (raycastInputs.depthLimit == undefined)
            {
                continue;
            }
            raycastInputs.depthLimit += depthOffset;
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
export function clusterVertexQueries(context is Context, selected is Query) returns array
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

