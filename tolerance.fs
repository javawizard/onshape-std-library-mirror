FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");

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
    MAX
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
    if (!(val.toleranceType == ToleranceType.NONE || val.toleranceType == ToleranceType.MIN || val.toleranceType == ToleranceType.MAX))
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

const LENGTH_TOLERANCE_BOUNDS =
{
    (meter) : [1e-5, 0.1, 500],
    (centimeter) : 0.1,
    (millimeter) : 0.1,
    (inch) : 0.1,
    (foot) : 0.1,
    (yard) : 0.1
} as LengthBoundSpec;

const ANGLE_TOLERANCE_BOUNDS =
{
    (degree) : [1e-5, 1, 180],
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
            annotation { "Name" : "Precision", "Column Name" : parentParameterName ~ PRECISION_SUFFIX, "Default" : PrecisionType.DEFAULT, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            definition[field ~ PRECISION] is PrecisionType;

            annotation { "Name" : "Tolerance type", "Column Name" : parentParameterName ~ TOLERANCE_TYPE_SUFFIX, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            definition[field ~ TOLERANCE_TYPE] is ToleranceType;

            if (definition[field ~ TOLERANCE_TYPE] == ToleranceType.SYMMETRICAL)
            {
                annotation { "Name" : "Deviation", "Column Name" : parentParameterName ~ DEVIATION_SUFFIX, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition[field ~ SYMMETRICAL_BOUNDS], LENGTH_TOLERANCE_BOUNDS);
            }
            else if (definition[field ~ TOLERANCE_TYPE] == ToleranceType.DEVIATION)
            {
                annotation { "Name" : "Upper", "Column Name" : parentParameterName ~ UPPER_DEVIATION_SUFFIX, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition[field ~ DEVIATION_UPPER], LENGTH_TOLERANCE_BOUNDS);

                annotation { "Name" : "Lower", "Column Name" : parentParameterName ~ LOWER_DEVIATION_SUFFIX, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition[field ~ DEVIATION_LOWER], LENGTH_TOLERANCE_BOUNDS);
            }
            else if (definition[field ~ TOLERANCE_TYPE] == ToleranceType.LIMITS)
            {
                annotation { "Name" : "Upper", "Column Name" : parentParameterName ~ UPPER_LIMIT_SUFFIX, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition[field ~ LIMITS_UPPER], LENGTH_TOLERANCE_BOUNDS);

                annotation { "Name" : "Lower", "Column Name" : parentParameterName ~ LOWER_LIMIT_SUFFIX, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isLength(definition[field ~ LIMITS_LOWER], LENGTH_TOLERANCE_BOUNDS);
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
            annotation { "Name" : "Precision", "Column Name" : parentParameterName ~ PRECISION_SUFFIX, "Default" : PrecisionType.DEFAULT, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            definition[field ~ PRECISION] is PrecisionType;

            annotation { "Name" : "Tolerance type", "Column Name" : parentParameterName ~ TOLERANCE_TYPE_SUFFIX, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            definition[field ~ TOLERANCE_TYPE] is ToleranceType;

            if (definition[field ~ TOLERANCE_TYPE] == ToleranceType.SYMMETRICAL)
            {
                annotation { "Name" : "Deviation", "Column Name" : parentParameterName ~ DEVIATION_SUFFIX, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isAngle(definition[field ~ SYMMETRICAL_BOUNDS], ANGLE_TOLERANCE_BOUNDS);
            }
            else if (definition[field ~ TOLERANCE_TYPE] == ToleranceType.DEVIATION)
            {
                annotation { "Name" : "Upper", "Column Name" : parentParameterName ~ UPPER_DEVIATION_SUFFIX, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isAngle(definition[field ~ DEVIATION_UPPER], ANGLE_TOLERANCE_BOUNDS);

                annotation { "Name" : "Lower", "Column Name" : parentParameterName ~ LOWER_DEVIATION_SUFFIX, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isAngle(definition[field ~ DEVIATION_LOWER], ANGLE_TOLERANCE_BOUNDS);
            }
            else if (definition[field ~ TOLERANCE_TYPE] == ToleranceType.LIMITS)
            {
                annotation { "Name" : "Upper", "Column Name" : parentParameterName ~ UPPER_LIMIT_SUFFIX, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isAngle(definition[field ~ LIMITS_UPPER], ANGLE_TOLERANCE_BOUNDS);

                annotation { "Name" : "Lower", "Column Name" : parentParameterName ~ LOWER_LIMIT_SUFFIX, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
                isAngle(definition[field ~ LIMITS_LOWER], ANGLE_TOLERANCE_BOUNDS);
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

    return modifiedDefinition;
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
    const toleranceType = definition[field ~ TOLERANCE_TYPE];

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

    return toleranceInfo(info);
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
 * specified [ToleranceInfo].
 */
export function getToleranceBounds(nominal is ValueWithUnits, tolerance) returns array
precondition isToleranceInfoOrUndefined(tolerance);
{
    if (tolerance == undefined)
    {
        return [nominal, nominal];
    }
    if (tolerance.toleranceType == ToleranceType.LIMITS)
    {
        return [tolerance.lower, tolerance.upper];
    }
    else if (tolerance.toleranceType == ToleranceType.DEVIATION)
    {
        return [nominal - tolerance.lower, nominal + tolerance.upper];
    }
    else if (tolerance.toleranceType == ToleranceType.SYMMETRICAL)
    {
        return [nominal + tolerance.lower, nominal + tolerance.upper];
    }

    else if (tolerance.toleranceType == ToleranceType.MIN)
    {
        return [nominal, 180 * degree];
    }
    else if (tolerance.toleranceType == ToleranceType.MAX)
    {
        return [0 * degree, nominal];
    }

    return [nominal, nominal];
}
