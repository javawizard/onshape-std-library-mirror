FeatureScript 2399; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/feature.fs", version : "2399.0");
import(path : "onshape/std/valueBounds.fs", version : "2399.0");
import(path : "onshape/std/lookupTablePath.fs", version : "2399.0");
export import(path : "onshape/std/fittolerancetables.gen.fs", version : "2399.0");

/**
 * Defines the tolerance type of a hole feature's parameter.
 * @value NONE : Defines no tolerance.
 * @value SYMMETRICAL : Defines a tolerance type where the allowed tolerance is a symmetrical deviation.
 * @value DEVIATION : Defines a tolerance type where the allowed tolerance is an asymmetrical deviation.
 * @value LIMITS : Defines a tolerance type where the allowed tolerance has defined upper and lower limits.
 * @value MIN : Defines a tolerance type where the parameter's value is considered the minimum allowed value.
 * @value MAX : Defines a tolerance type where the parameter's value is considered the maximum allowed value.
 */
export enum ToleranceType
{
    annotation { "Name" : "No tolerance" }
    NONE,

    annotation { "Name" : "Symmetrical" }
    SYMMETRICAL,

    annotation { "Name" : "Deviation" }
    DEVIATION,

    annotation { "Name" : "Limits" }
    LIMITS,

    annotation { "Name" : "Min" }
    MIN,

    annotation { "Name" : "Max" }
    MAX,

    annotation { "Name" : "Fit", "Hidden" : true }
    FIT,

    annotation { "Name" : "Fit with tolerance", "Hidden" : true }
    FIT_WITH_TOLERANCE,

    annotation { "Name" : "Fit (tolerance only)", "Hidden" : true }
    FIT_TOLERANCE_ONLY
}

/**
 * Defines the tolerance type of a hole feature's parameter.
 * @value NONE : Defines no tolerance.
 * @value SYMMETRICAL : Defines a tolerance type where the allowed tolerance is a symmetrical deviation.
 * @value DEVIATION : Defines a tolerance type where the allowed tolerance is an asymmetrical deviation.
 * @value LIMITS : Defines a tolerance type where the allowed tolerance has defined upper and lower limits.
 * @value MIN : Defines a tolerance type where the parameter's value is considered the minimum allowed value.
 * @value MAX : Defines a tolerance type where the parameter's value is considered the maximum allowed value.
 * @value FIT : Defines a tolerance type where the upper and lower limits are defined by the specified fit tolerance class.
 * @value FIT_WITH_TOLERANCE : Defines a tolerance type where the upper and lower limits are defined by the specified fit tolerance class.
 * @value FIT_TOLERANCE_ONLY : Defines a tolerance type where the upper and lower limits are defined by the specified fit tolerance class.
 */
export enum ToleranceTypeExtended
{
    annotation { "Name" : "No tolerance" }
    NONE,

    annotation { "Name" : "Symmetrical" }
    SYMMETRICAL,

    annotation { "Name" : "Deviation" }
    DEVIATION,

    annotation { "Name" : "Limits" }
    LIMITS,

    annotation { "Name" : "Min" }
    MIN,

    annotation { "Name" : "Max" }
    MAX,

    annotation { "Name" : "Fit" }
    FIT,

    annotation { "Name" : "Fit with tolerance" }
    FIT_WITH_TOLERANCE,

    annotation { "Name" : "Fit (tolerance only)" }
    FIT_TOLERANCE_ONLY
}

/**
 * Stores tolerance-related info for a specific field in a feature. This info includes an optional
 * precision override, a tolerance type, and, if applicable, an upper and lower bound associated
 * with that tolerance type.
 */
export type ToleranceInfo typecheck canBeToleranceInfo;

/**
 * Type checking predicate for the [ToleranceInfo] type.
 */
export predicate canBeToleranceInfo(val)
{
    val is map;
    if (val.precision != undefined)
    {
        val.precision is number;
    }
    val.usePrecisionOverride is boolean;
    val.toleranceType is ToleranceType;
    if (val.toleranceType == ToleranceType.FIT ||
        val.toleranceType == ToleranceType.FIT_WITH_TOLERANCE ||
        val.toleranceType == ToleranceType.FIT_TOLERANCE_ONLY)
    {
        val.toleranceFitInfo is ToleranceFitInfo;
    }
    if (!(val.toleranceType == ToleranceType.NONE || val.toleranceType == ToleranceType.MIN || val.toleranceType == ToleranceType.MAX || val.toleranceType == ToleranceType.FIT))
    {
        val.upper is ValueWithUnits;
        val.lower is ValueWithUnits;
    }
}

/**
 * Constructor for the ToleranceInfo type.
 */
export function toleranceInfo(info is map)
precondition canBeToleranceInfo(info);
{
    return info as ToleranceInfo;
}

/**
 * Stores fit tolerance-related info for a specific field in a feature.
 */
export type ToleranceFitInfo typecheck canBeToleranceFitInfo;

/**
 * Type checking predicate for the [ToleranceFitInfo] type.
 */
export predicate canBeToleranceFitInfo(val)
{
    val is map;
    val.standard is string;
    val.holeClass is string;
    val.shaftClass is string || val.shaftClass is undefined;
}

/**
 * Constructor for the ToleranceFitInfo type.
 */
export function toleranceFitInfo(info is map)
precondition canBeToleranceFitInfo(info);
{
    return info as ToleranceFitInfo;
}

/**
 * Defines the precision of a hole feature's parameter.
 * @value ONES : Display precision up to '1'.
 * @value TENTHS : Display precision up to '0.1'.
 * @value HUNDREDTHS : Display precision up to '0.01'.
 * @value THOUSANDTHS : Display precision up to '0.001'.
 * @value TEN_THOUSANDTHS : Display precision up to '0.0001'.
 * @value HUNDRED_THOUSANDTHS : Display precision up to '0.00001'.
 * @value MILLIONTHS : Display precision up to '0.000001'.
 */
export enum PrecisionType {
    annotation { "Name": "Workspace precision" }
    DEFAULT,

    annotation { "Name" : "0" }
    ONES,

    annotation { "Name" : "0.1" }
    TENTHS,

    annotation { "Name" : "0.12" }
    HUNDREDTHS,

    annotation { "Name" : "0.123" }
    THOUSANDTHS,

    annotation { "Name" : "0.1234" }
    TEN_THOUSANDTHS,

    annotation { "Name" : "0.12345" }
    HUNDRED_THOUSANDTHS,

    annotation { "Name" : "0.123456" }
    MILLIONTHS
}

const PRECISION = "Precision";
const TOLERANCE_TYPE = "ToleranceType";
const SYMMETRICAL_BOUNDS = "ToleranceBoundSymmetrical";
const DEVIATION_UPPER = "ToleranceBoundDeviationUpper";
const DEVIATION_LOWER = "ToleranceBoundDeviationLower";
const LIMITS_UPPER = "ToleranceBoundLimitsUpper";
const LIMITS_LOWER = "ToleranceBoundLimitsLower";
const HOLE_CLASS = "holeClass";
const FIT_TOLERANCE_STANDARD = "standard";
const FIT_TOLERANCE_TABLE = "FitToleranceTable";

const UPPER_LENGTH_TOLERANCE_BOUNDS =
{
    (meter) : [-500, 0.1, 500],
    (centimeter) : 0.1,
    (millimeter) : 0.1,
    (inch) : 0.1,
    (foot) : 0.1,
    (yard) : 0.1
} as LengthBoundSpec;

const LOWER_LENGTH_TOLERANCE_BOUNDS =
{
    (meter) : [-500, -0.1, 500],
    (centimeter) : -0.1,
    (millimeter) : -0.1,
    (inch) : -0.1,
    (foot) : -0.1,
    (yard) : -0.1
} as LengthBoundSpec;

const SYMMETRICAL_LENGTH_TOLERANCE_BOUNDS =
{
    (meter) : [0, 0.1, 500],
    (centimeter) : 0.1,
    (millimeter) : 0.1,
    (inch) : 0.1,
    (foot) : 0.1,
    (yard) : 0.1
} as LengthBoundSpec;

const UPPER_ANGLE_TOLERANCE_BOUNDS =
{
    (degree) : [-180, 1, 180],
    (radian) : 0.1
} as AngleBoundSpec;

const LOWER_ANGLE_TOLERANCE_BOUNDS =
{
    (degree) : [-180, -1, 180],
    (radian) : -0.1
} as AngleBoundSpec;

const SYMMETRICAL_ANGLE_TOLERANCE_BOUNDS =
{
    (degree) : [0, 1, 180],
    (radian) : 0.1
} as AngleBoundSpec;

const TOLERANCE_CONTROLS_SUFFIX = " tolerance controls";
const PRECISION_SUFFIX = " precision";
const TOLERANCE_TYPE_SUFFIX = " tolerance type";
const DEVIATION_SUFFIX = " deviation";
const UPPER_DEVIATION_SUFFIX = " upper deviation";
const LOWER_DEVIATION_SUFFIX = " lower deviation";
const UPPER_LIMIT_SUFFIX = " upper limit";
const LOWER_LIMIT_SUFFIX = " lower limit";
const FIT_TOLERANCE_TABLE_SUFFIX = " fit tolerance table";

/**
 * Creates a parameter group containing length tolerance controls for the specified field in the
 * specified definition.
 */
export predicate defineLengthTolerance(definition is map, field is string, parentParameterName is string)
{
    if (definition[field] != undefined)
    {
        annotation { "Group Name" : parentParameterName ~ TOLERANCE_CONTROLS_SUFFIX, "Collapsed By Default" : true, "Driving Parameter": field }
        {
            annotation { "Name" : "Precision", "Column Name" : parentParameterName ~ PRECISION_SUFFIX, "Default" : PrecisionType.DEFAULT }
            definition[field ~ PRECISION] is PrecisionType;

            annotation { "Name" : "Tolerance type", "Column Name" : parentParameterName ~ TOLERANCE_TYPE_SUFFIX }
            definition[field ~ TOLERANCE_TYPE] is ToleranceType;

            if (definition[field ~ TOLERANCE_TYPE] == ToleranceType.SYMMETRICAL)
            {
                annotation { "Name" : "Deviation", "Column Name" : parentParameterName ~ DEVIATION_SUFFIX }
                isLength(definition[field ~ SYMMETRICAL_BOUNDS], SYMMETRICAL_LENGTH_TOLERANCE_BOUNDS);
            }
            else if (definition[field ~ TOLERANCE_TYPE] == ToleranceType.DEVIATION)
            {
                annotation { "Name" : "Upper", "Column Name" : parentParameterName ~ UPPER_DEVIATION_SUFFIX }
                isLength(definition[field ~ DEVIATION_UPPER], UPPER_LENGTH_TOLERANCE_BOUNDS);

                annotation { "Name" : "Lower", "Column Name" : parentParameterName ~ LOWER_DEVIATION_SUFFIX }
                isLength(definition[field ~ DEVIATION_LOWER], LOWER_LENGTH_TOLERANCE_BOUNDS);
            }
            else if (definition[field ~ TOLERANCE_TYPE] == ToleranceType.LIMITS)
            {
                annotation { "Name" : "Upper", "Column Name" : parentParameterName ~ UPPER_LIMIT_SUFFIX }
                isLength(definition[field ~ LIMITS_UPPER], UPPER_LENGTH_TOLERANCE_BOUNDS);

                annotation { "Name" : "Lower", "Column Name" : parentParameterName ~ LOWER_LIMIT_SUFFIX }
                isLength(definition[field ~ LIMITS_LOWER], LOWER_LENGTH_TOLERANCE_BOUNDS);
            }
        }
    }
}

/**
 * Creates a parameter group containing length fit tolerance controls for the specified field in the
 * specified definition.
 */
export predicate defineLengthToleranceExtended(definition is map, field is string, parentParameterName is string)
{
    if (definition[field] != undefined)
    {
        annotation { "Group Name" : parentParameterName ~ TOLERANCE_CONTROLS_SUFFIX, "Collapsed By Default" : true, "Driving Parameter": field }
        {
            annotation { "Name" : "Precision", "Column Name" : parentParameterName ~ PRECISION_SUFFIX, "Default" : PrecisionType.DEFAULT }
            definition[field ~ PRECISION] is PrecisionType;

            annotation { "Name" : "Tolerance type", "Column Name" : parentParameterName ~ TOLERANCE_TYPE_SUFFIX }
            definition[field ~ TOLERANCE_TYPE] is ToleranceTypeExtended;

            if (definition[field ~ TOLERANCE_TYPE] == ToleranceTypeExtended.SYMMETRICAL)
            {
                annotation { "Name" : "Deviation", "Column Name" : parentParameterName ~ DEVIATION_SUFFIX }
                isLength(definition[field ~ SYMMETRICAL_BOUNDS], SYMMETRICAL_LENGTH_TOLERANCE_BOUNDS);
            }
            else if (definition[field ~ TOLERANCE_TYPE] == ToleranceTypeExtended.DEVIATION)
            {
                annotation { "Name" : "Upper", "Column Name" : parentParameterName ~ UPPER_DEVIATION_SUFFIX }
                isLength(definition[field ~ DEVIATION_UPPER], UPPER_LENGTH_TOLERANCE_BOUNDS);

                annotation { "Name" : "Lower", "Column Name" : parentParameterName ~ LOWER_DEVIATION_SUFFIX }
                isLength(definition[field ~ DEVIATION_LOWER], LOWER_LENGTH_TOLERANCE_BOUNDS);
            }
            else if (definition[field ~ TOLERANCE_TYPE] == ToleranceTypeExtended.LIMITS)
            {
                annotation { "Name" : "Upper", "Column Name" : parentParameterName ~ UPPER_LIMIT_SUFFIX }
                isLength(definition[field ~ LIMITS_UPPER], UPPER_LENGTH_TOLERANCE_BOUNDS);

                annotation { "Name" : "Lower", "Column Name" : parentParameterName ~ LOWER_LIMIT_SUFFIX }
                isLength(definition[field ~ LIMITS_LOWER], LOWER_LENGTH_TOLERANCE_BOUNDS);
            }
            else if (definition[field ~ TOLERANCE_TYPE] == ToleranceTypeExtended.FIT ||
                    definition[field ~ TOLERANCE_TYPE] == ToleranceTypeExtended.FIT_WITH_TOLERANCE ||
                    definition[field ~ TOLERANCE_TYPE] == ToleranceTypeExtended.FIT_TOLERANCE_ONLY)
            {
                annotation { "Name" : "Fit type", "Column Name" : parentParameterName ~ FIT_TOLERANCE_TABLE_SUFFIX, "Lookup Table" : FitToleranceTable, "UIHint" : ["REMEMBER_PREVIOUS_VALUE", "UNCONFIGURABLE"] }
                definition[field ~ FIT_TOLERANCE_TABLE] is LookupTablePath;
            }

            if (definition[field ~ TOLERANCE_TYPE] == ToleranceTypeExtended.FIT_WITH_TOLERANCE ||
                definition[field ~ TOLERANCE_TYPE] == ToleranceTypeExtended.FIT_TOLERANCE_ONLY)
            {
                annotation { "Name" : "Upper", "Column Name" : parentParameterName ~ UPPER_LIMIT_SUFFIX, "UIHint" : UIHint.READ_ONLY }
                isLength(definition[field ~ HOLE_CLASS ~ LIMITS_UPPER], UPPER_LENGTH_TOLERANCE_BOUNDS);

                annotation { "Name" : "Lower", "Column Name" : parentParameterName ~ LOWER_LIMIT_SUFFIX, "UIHint" : UIHint.READ_ONLY }
                isLength(definition[field ~ HOLE_CLASS ~ LIMITS_LOWER], LOWER_LENGTH_TOLERANCE_BOUNDS);
            }
        }
    }
}

/**
 * Creates a parameter group containing angle tolerance controls for the specified field in the
 * specified definition.
 */
export predicate defineAngleTolerance(definition is map, field is string, parentParameterName is string)
{
    if (definition[field] != undefined)
    {
        annotation { "Group Name" : parentParameterName ~ TOLERANCE_CONTROLS_SUFFIX, "Collapsed By Default" : true, "Driving Parameter": field }
        {
            annotation { "Name" : "Precision", "Column Name" : parentParameterName ~ PRECISION_SUFFIX, "Default" : PrecisionType.DEFAULT }
            definition[field ~ PRECISION] is PrecisionType;

            annotation { "Name" : "Tolerance type", "Column Name" : parentParameterName ~ TOLERANCE_TYPE_SUFFIX }
            definition[field ~ TOLERANCE_TYPE] is ToleranceType;

            if (definition[field ~ TOLERANCE_TYPE] == ToleranceType.SYMMETRICAL)
            {
                annotation { "Name" : "Deviation", "Column Name" : parentParameterName ~ DEVIATION_SUFFIX }
                isAngle(definition[field ~ SYMMETRICAL_BOUNDS], SYMMETRICAL_ANGLE_TOLERANCE_BOUNDS);
            }
            else if (definition[field ~ TOLERANCE_TYPE] == ToleranceType.DEVIATION)
            {
                annotation { "Name" : "Upper", "Column Name" : parentParameterName ~ UPPER_DEVIATION_SUFFIX }
                isAngle(definition[field ~ DEVIATION_UPPER], UPPER_ANGLE_TOLERANCE_BOUNDS);

                annotation { "Name" : "Lower", "Column Name" : parentParameterName ~ LOWER_DEVIATION_SUFFIX }
                isAngle(definition[field ~ DEVIATION_LOWER], LOWER_ANGLE_TOLERANCE_BOUNDS);
            }
            else if (definition[field ~ TOLERANCE_TYPE] == ToleranceType.LIMITS)
            {
                annotation { "Name" : "Upper", "Column Name" : parentParameterName ~ UPPER_LIMIT_SUFFIX }
                isAngle(definition[field ~ LIMITS_UPPER], UPPER_ANGLE_TOLERANCE_BOUNDS);

                annotation { "Name" : "Lower", "Column Name" : parentParameterName ~ LOWER_LIMIT_SUFFIX }
                isAngle(definition[field ~ LIMITS_LOWER], LOWER_ANGLE_TOLERANCE_BOUNDS);
            }
        }
    }
}

const PRECISION_MAP = {
    (PrecisionType.ONES) : 0,
    (PrecisionType.TENTHS) : 1,
    (PrecisionType.HUNDREDTHS) : 2,
    (PrecisionType.THOUSANDTHS) : 3,
    (PrecisionType.TEN_THOUSANDTHS) : 4,
    (PrecisionType.HUNDRED_THOUSANDTHS) : 5,
    (PrecisionType.MILLIONTHS) : 6
};

/** @internal */
export function copyToleranceInfo(fromDefinition is map, toDefinition is map, fromField is string, toField is string) returns map
{
    const fieldsToCopy = [
        PRECISION,
        TOLERANCE_TYPE,
        SYMMETRICAL_BOUNDS,
        DEVIATION_LOWER,
        DEVIATION_UPPER,
        LIMITS_LOWER,
        LIMITS_UPPER
    ];

    var modifiedDefinition = toDefinition;

    for (var fieldToCopy in fieldsToCopy)
    {
        modifiedDefinition[toField ~ fieldToCopy] = fromDefinition[fromField ~ fieldToCopy];
    }

    const toleranceType = syncToleranceTypes(fromDefinition[fromField ~ TOLERANCE_TYPE]);

    if (toleranceType == ToleranceType.FIT ||
        toleranceType == ToleranceType.FIT_WITH_TOLERANCE ||
        toleranceType == ToleranceType.FIT_TOLERANCE_ONLY)
    {
        modifiedDefinition[toField ~ FIT_TOLERANCE_TABLE] = fromDefinition[fromField ~ FIT_TOLERANCE_TABLE];
    }

    return modifiedDefinition;
}

/** @internal */
function syncToleranceTypes(toleranceType) returns ToleranceType
{
    if (toleranceType is ToleranceTypeExtended)
    {
        return switch (toleranceType)
        {
            ToleranceTypeExtended.NONE : ToleranceType.NONE,
            ToleranceTypeExtended.SYMMETRICAL : ToleranceType.SYMMETRICAL,
            ToleranceTypeExtended.DEVIATION : ToleranceType.DEVIATION,
            ToleranceTypeExtended.LIMITS : ToleranceType.LIMITS,
            ToleranceTypeExtended.MIN : ToleranceType.MIN,
            ToleranceTypeExtended.MAX : ToleranceType.MAX,
            ToleranceTypeExtended.FIT : ToleranceType.FIT,
            ToleranceTypeExtended.FIT_WITH_TOLERANCE : ToleranceType.FIT_WITH_TOLERANCE,
            ToleranceTypeExtended.FIT_TOLERANCE_ONLY : ToleranceType.FIT_TOLERANCE_ONLY
        };
    }
    else if (toleranceType is ToleranceType)
    {
        return toleranceType;
    }
    return ToleranceType.NONE;
}

/**
 * Extracts the [ToleranceInfo] associated with a given field in the given feature definition. The
 * [ToleranceInfo] is gathered from parameters which are created using either the
 * [defineLengthTolerance] or [defineAngleTolerance] predicates.
 */
export function getToleranceInfo(definition is map, field is string) returns ToleranceInfo
{
    var info = {};
    const precision = definition[field ~ PRECISION];
    const toleranceType = syncToleranceTypes(definition[field ~ TOLERANCE_TYPE]);

    info.usePrecisionOverride = !(precision == undefined || precision == PrecisionType.DEFAULT);

    if (info.usePrecisionOverride)
    {
        info.precision = PRECISION_MAP[precision];
    }

    if (toleranceType != undefined)
    {
        info.toleranceType = toleranceType;
    }
    else
    {
        info.toleranceType = ToleranceType.NONE;
    }

    if (toleranceType == ToleranceType.SYMMETRICAL)
    {
        var symmetricalTolerance = definition[field ~ SYMMETRICAL_BOUNDS];
        info.lower = -symmetricalTolerance;
        info.upper = symmetricalTolerance;
    }
    else if (toleranceType == ToleranceType.DEVIATION)
    {
        var lowerTolerance = definition[field ~ DEVIATION_LOWER];
        var upperTolerance = definition[field ~ DEVIATION_UPPER];
        info.lower = lowerTolerance;
        info.upper = upperTolerance;
    }
    else if (toleranceType == ToleranceType.LIMITS)
    {
        var lowerTolerance = definition[field ~ LIMITS_LOWER];
        var upperTolerance = definition[field ~ LIMITS_UPPER];
        info.lower = lowerTolerance;
        info.upper = upperTolerance;
    }

    if (toleranceType == ToleranceType.FIT ||
        toleranceType == ToleranceType.FIT_WITH_TOLERANCE ||
        toleranceType == ToleranceType.FIT_TOLERANCE_ONLY)
    {
        var fitInfo = {};
        fitInfo.standard = definition[field ~ FIT_TOLERANCE_TABLE][FIT_TOLERANCE_STANDARD];
        fitInfo.holeClass = definition[field ~ FIT_TOLERANCE_TABLE][HOLE_CLASS];
        const shaftClass = getLookupTable(FitToleranceTable, definition[field ~ FIT_TOLERANCE_TABLE]);
        if (shaftClass != {}) {
            fitInfo.shaftClass = shaftClass;
        }
        info.toleranceFitInfo = toleranceFitInfo(fitInfo);
    }
    if (toleranceType == ToleranceType.FIT_WITH_TOLERANCE || toleranceType == ToleranceType.FIT_TOLERANCE_ONLY)
    {
        definition = updateFitToleranceLimits(definition, field);
        var lowerTolerance = definition[field ~ HOLE_CLASS ~ LIMITS_LOWER];
        var upperTolerance = definition[field ~ HOLE_CLASS ~ LIMITS_UPPER];
        info.lower = lowerTolerance;
        info.upper = upperTolerance;
    }

    return toleranceInfo(info);
}

 /**
 * Updates the fit tolerance field information associated with a specified field in the given feature definition.
 * The tolerance information is extracted from parameters created using the [defineLengthToleranceExtended] predicates.
 * @param context {Context} : The target context.
 * @param id {Id} : Identifier of the feature.
 * @param definition {map} : The feature definition from which to extract and update tolerance information.
 * @param field {string} : The field name for which to update tolerance information.
 * @returns {map} : The updated feature definition with updated tolerance information.
 */
export function updateFitToleranceFields(context is Context, id is Id, definition is map, field is string) returns map
{
    const toleranceType = syncToleranceTypes(definition[field ~ TOLERANCE_TYPE]);

    if (toleranceType != undefined &&
        (toleranceType == ToleranceType.FIT_WITH_TOLERANCE ||
         toleranceType == ToleranceType.FIT_TOLERANCE_ONLY))
    {
        definition = updateFitToleranceLimits(definition, field);
        setFeatureComputedParameter(context, id, {
            "name" : field ~ HOLE_CLASS ~ LIMITS_LOWER,
            "value" : definition[field ~ HOLE_CLASS ~ LIMITS_LOWER]
        });
        setFeatureComputedParameter(context, id, {
            "name" : field ~ HOLE_CLASS ~ LIMITS_UPPER,
            "value" : definition[field ~ HOLE_CLASS ~ LIMITS_UPPER]
        });
    }
    return definition;
}

/** @internal */
function updateFitToleranceLimits(definition is map, field is string) returns map
{
    var nominalSize = definition[field];
    if (nominalSize is string) // Evaluate if a value is an expression
    {
        nominalSize = lookupTableEvaluate(nominalSize);
    }
    const standard = definition[field ~ FIT_TOLERANCE_TABLE][FIT_TOLERANCE_STANDARD];
    const holeclass = definition[field ~ FIT_TOLERANCE_TABLE][HOLE_CLASS];

    const limits = getFitToleranceLimits(nominalSize, standard, holeclass, true);
    definition[field ~ HOLE_CLASS ~ LIMITS_LOWER] = lookupTableEvaluate(limits.lower);
    definition[field ~ HOLE_CLASS ~ LIMITS_UPPER] = lookupTableEvaluate(limits.upper);
    return definition;
}

/**
 * Determines whether or not a tolerance is set in a given [ToleranceInfo]. A tolerance is considered
 * to be "set" if either its tolerance type is set to a value other than `NONE`, or if it has a
 * precision override value set.
 */
export function isToleranceSet(tolerance is ToleranceInfo) returns boolean
{
    return tolerance.toleranceType != ToleranceType.NONE || tolerance.usePrecisionOverride;
}

/**
 * Determines if a given value is either a [ToleranceInfo] or `undefined`.
 */
export predicate isToleranceInfoOrUndefined(val)
{
    val is undefined || val is ToleranceInfo;
}

/**
 * Produces an array containing the upper and lower bounds of a [ValueWithUnits] given a
 * specified [ToleranceInfo] as well as a map of options.
 * @param nominal {ValueWithUnits} : the nominal value
 * @param tolerance {ToleranceInfo} : the tolerance info for the given value
 * @param options {{
 *   @field minimum {ValueWithUnits} : the upper bound if the tolerance type is maximum
 *   @field maximun {ValueWithUnits} : the lower bound if the tolerance type is minimum
 *   @field useDrawingLimitsFix {boolean} : if false, uses the old bounds calculation of \[lowerLim, upperLim\]
 * }}
 */
export function getToleranceBounds(nominal is ValueWithUnits, tolerance, options is map) returns array
precondition isToleranceInfoOrUndefined(tolerance);
{
    if (tolerance == undefined)
    {
        return [nominal, nominal];
    }
    if (tolerance.toleranceType == ToleranceType.LIMITS)
    {
        // If no option is passed, we want to use the fix
        if (options.useDrawingLimitsFix == false)
        {
            return [tolerance.lower, tolerance.upper];
        }
        else
        {
            return [nominal + tolerance.lower, nominal + tolerance.upper];
        }
    }
    else if (tolerance.toleranceType == ToleranceType.DEVIATION)
    {
        return [nominal + tolerance.lower, nominal + tolerance.upper];
    }
    else if (tolerance.toleranceType == ToleranceType.SYMMETRICAL)
    {
        // Symmetrical tolerances only have one meaningful tolerance value
        return [nominal - tolerance.upper, nominal + tolerance.upper];
    }

    else if (tolerance.toleranceType == ToleranceType.MIN)
    {
        return [nominal, options.maximum];
    }
    else if (tolerance.toleranceType == ToleranceType.MAX)
    {
        return [options.minimum, nominal];
    }
    else if (tolerance.toleranceType == ToleranceType.FIT_WITH_TOLERANCE ||
        tolerance.toleranceType == ToleranceType.FIT_TOLERANCE_ONLY)
    {
        return [nominal + tolerance.lower, nominal + tolerance.upper];
    }

    return [nominal, nominal];
}

/**
 * Produces an array containing the upper and lower bounds of a [ValueWithUnits] given a
 * specified [ToleranceInfo].
 */
export function getToleranceBounds(nominal is ValueWithUnits, tolerance, minimum is ValueWithUnits, maximum is ValueWithUnits) returns array
precondition isToleranceInfoOrUndefined(tolerance);
{
    return getToleranceBounds(nominal, tolerance, {
        "minimum" : minimum,
        "maximum" : maximum
    });
}

/** @internal */
export function getToleranceBoundsParameterIds(parameter is string, tolerance is ToleranceInfo) returns array
{
    const toleranceType = tolerance.toleranceType;
    if (toleranceType == ToleranceType.SYMMETRICAL)
    {
        return [parameter ~ SYMMETRICAL_BOUNDS];
    }
    if (toleranceType == ToleranceType.DEVIATION)
    {
        return [parameter ~ DEVIATION_UPPER, parameter ~ DEVIATION_LOWER];
    }
    if (toleranceType == ToleranceType.LIMITS)
    {
        return [parameter ~ LIMITS_UPPER, parameter ~ LIMITS_LOWER];
    }
    if (toleranceType == ToleranceType.FIT_WITH_TOLERANCE ||
        toleranceType == ToleranceType.FIT_TOLERANCE_ONLY)
    {
        return [parameter ~ HOLE_CLASS ~ LIMITS_UPPER, parameter ~ HOLE_CLASS ~ LIMITS_LOWER];
    }
    return [];
}
