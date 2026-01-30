FeatureScript 2878; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/units.fs", version : "2878.0");

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
    FIT_TOLERANCE_ONLY,

    annotation { "Name" : "Default", "Hidden" : true }
    DEFAULT,

    annotation { "Name" : "Default", "Hidden" : true }
    BASIC
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
 *
 * Upper and lower tolerances are differences from the nominal value and are both added to the nominal value.
 * @ex ```
 * // +/- 1mm
 * {
 *  toleranceType: ToleranceType.SYMMETRICAL,
 *  upper: 1 * millimeter,
 *  lower: -1 * millimeter
 * }```
 *
 * Tolerances need not be positive and negative. The only requirement is that the upper tolerance is greater than the lower.
 * @ex ```
 * // +0.1mm / +0.02mm
 * {
 *  toleranceType: ToleranceType.DEVIATION,
 *  upper: 0.1 * millimeter,
 *  lower: 0.02 * millimeter
 * }```
 * @seeAlso [setDimensionedEntities]
 * @type {{
 *      @field usePrecisionOverride {boolean} : @optional
 *          Whether a precision override is defined
 *      @field precision {number} : @requiredIf {`usePrecisionOverride` is `true`}
 *          The number of decimal places output for the parameter's nominal value and tolerances
 *      @field toleranceType {ToleranceType}
 *      @field toleranceFitInfo {ToleranceFitInfo} : @requiredIf {`toleranceType` is `FIT`, `FIT_WITH_TOLERANCE` or `FIT_TOLERANCE_ONLY`}
 *          Specifies the fit standard to be used for a hole or shaft
 *      @field upper {ValueWithUnits} : @requiredIf {`toleranceType` is `SYMMETRICAL`, `DEVIATION` or `LIMITS`}
 *          Specifies the upper tolerance.
 *      @field lower {ValueWithUnits} : @requiredIf {`toleranceType` is `SYMMETRICAL`, `DEVIATION` or `LIMITS`}
 *          Specifies the lower tolerance.
 * }}
 */
export type ToleranceInfo typecheck canBeToleranceInfo;

/**
 * Type checking predicate for the [ToleranceInfo] type.
 */
export predicate canBeToleranceInfo(val)
{
    val is map;
    if (val.usePrecisionOverride != undefined) {
        val.usePrecisionOverride is boolean;
        if (val.usePrecisionOverride)
        {
            val.precision is number;
        }
    }

    val.toleranceType is ToleranceType;
    if (val.toleranceType == ToleranceType.FIT ||
        val.toleranceType == ToleranceType.FIT_WITH_TOLERANCE ||
        val.toleranceType == ToleranceType.FIT_TOLERANCE_ONLY)
    {
        val.toleranceFitInfo is ToleranceFitInfo;
    }
    if (val.toleranceType == ToleranceType.SYMMETRICAL || val.toleranceType == ToleranceType.DEVIATION || val.toleranceType == ToleranceType.LIMITS)
    {
        val.upper is ValueWithUnits;
        val.lower is ValueWithUnits;
    }
    else if (val.toleranceType == ToleranceType.FIT_WITH_TOLERANCE || val.toleranceType == ToleranceType.FIT_TOLERANCE_ONLY)
    {
        val.upper == undefined || val.upper is ValueWithUnits;
        val.lower == undefined || val.lower is ValueWithUnits;
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
 * @seeAlso [setDimensionedEntities], [ToleranceInfo]
 *
 * @type {{
 *      @field standard {string} :
 *          `"ISO"` or `"ANSI"`
 *      @field holeClass {string} :
 *          The class of the hole
 *      @field shaftClass {string} : @optional
 *          The class of the shaft
 * }}
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
 * Defines the precision of a tolerant quantity.
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

