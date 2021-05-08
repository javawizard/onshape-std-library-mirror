FeatureScript 1511; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/math.fs", version : "1511.0");
import(path : "onshape/std/expressionvalidationresult.gen.fs", version : "1511.0");
import(path : "onshape/std/string.fs", version : "1511.0");

/**
 * A `ValueWithUnits` is a number with dimensions, such as 1.5 inches,
 * 90 degrees, or 9.81 meters per second per second.
 * ```
 * const width is ValueWithUnits = 1.5 * inch;
 * const angle is ValueWithUnits = 90 * degree;
 * const g     is ValueWithUnits = 9.81 * meter / second / second;
 * ```
 *
 * Values with the same dimensions can be added and subtracted, even if
 * they were created in different unit systems.
 * ```
 * const length       = 3 * meter + 1 * inch;
 * const longerLength = length + 0.01 * inch;
 * const nonsense     = 3 * meter + 3 * degree;     // Throws an error from dimension mismatch
 * ```
 *
 * Multiplication (`*`) will multiply both the values and the units. An
 * expression where the units all cancel evaluates to plain `number`.
 * ```
 * var doubleLength   = 2 * length;                 // ValueWithUnits with length units
 * var area           = (20 * foot) * (30 * foot);  // ValueWithUnits with area units
 * var numberOfBricks = (64 * foot) / (9 * inch);   // number with no units
 * ```
 *
 * Values with units can be raised to numerical powers with the `^` operator.
 * Base units like `inch` or `second` can be exponentiated in the same way.
 * ```
 * var squareArea   = (3 * meter)^2;
 * var g            = 9.81 * meter / second^2;
 * ```
 *
 * Functions in the standard library require a ValueWithUnits for arguments where
 * units are needed. Thus, the `depth` in [opExtrude] requires a value with length
 * units (rather than assuming meters). The argument of [sin] is a value with angle
 * units (rather than assuming radians). The argument of [sqrt](sqrt(ValueWithUnits))
 * can be any value whose units are even powers.
 * ```
 * var ladderHeight   = ladderLength * sin(75 * degree); // Has length units
 * var pendulumPeriod = 2 * PI * sqrt(armLength / g);    // Has time units
 * ```
 *
 * Equality of `ValueWithUnits` considers the underlying value, so
 * `25.4 * millimeter` is equal to `1 * inch`. However, `PI * radian / 5`
 * does not equal `36 * degree` because of finite precision arithmetic.
 * To check equality of `ValueWithUnits`, you should use
 * [tolerantEquals](tolerantEquals(ValueWithUnits, ValueWithUnits)).
 * ```
 * if (tolerantEquals(myLength, 0 * inch))
 * {
 *     ...
 * ```
 *
 * Keeping correct units on variables is always best practice, in order to benefit
 * from easy unit conversions and runtime unit checks. However, when printing, you
 * may wish to divide out the units in order to display a value in a different system
 * of units.
 * ```
 * const length = 42 * centimeter;
 * println(length);                                 // prints "0.42 meter"
 * println("length: " ~ toString(length));          // prints "length: 0.42 meter"
 * println(length / inch ~ " inches");              // prints "16.535433070866137 inches"
 * println(roundToPrecision(length / inch, 3) ~ " inches"); // prints "16.535 inches"
 * ```
 */
export type ValueWithUnits typecheck canBeValueWithUnits;

/** @internal */
export predicate canBeValueWithUnits(value)
{
    value is map;
    value.value is number;
    value.unit is UnitSpec;
}

/**
 * @internal
 *
 * A `UnitSpec` is a fundamental dimension like length and time.
 * Angle is treated as a dimension although formally it is dimensionless.
 */
export type UnitSpec typecheck canBeUnitSpec;

/** @internal */
export predicate canBeUnitSpec(value)
{
    value is map;
    @size(value) > 0;
    for (var unit in value)
    {
        unit.key is string;
        unit.value is number;
        unit.value != 0;
    }
}

/** @internal */
export const LENGTH_UNITS = { "meter" : 1 } as UnitSpec;
/** @internal */
export const ANGLE_UNITS = { "radian" : 1 } as UnitSpec;
/** @internal */
export const MASS_UNITS = { "kilogram" : 1 } as UnitSpec;
/** @internal */
export const TEMPERATURE_UNITS = { "kelvin" : 1 } as UnitSpec;
/** @internal */
export const TIME_UNITS = { "second" : 1 } as UnitSpec;
/** @internal */
export const CURRENT_UNITS = { "ampere" : 1 } as UnitSpec;
/** @internal */
export const DENSITY_UNITS = { "kilogram" : 1, "meter" : -3 } as UnitSpec;
/** @internal */
export const AREA_UNITS = { "meter" : 2 } as UnitSpec;
/** @internal */
export const VOLUME_UNITS = { "meter" : 3 } as UnitSpec;
/** @internal */
export const INERTIA_UNITS = { "kilogram" : 1, "meter" : 2 } as UnitSpec;

//TODO: we probably want separate documents for standard units so as not to pollute the namespace
/**
 * The constant `1`, with no units.
 */
export const unitless = 1;

/** A constant equal to 1 meter. */
annotation { "Name" : "Meter", "Abbreviation" : "m" }
export const meter = { "value" : 1, "unit" : LENGTH_UNITS } as ValueWithUnits;

/** A constant equal to 1 centimeter. */
annotation { "Name" : "Centimeter", "Abbreviation" : "cm" }
export const centimeter = 0.01 * meter;

/** A constant equal to 1 millimeter. */
annotation { "Name" : "Millimeter", "Abbreviation" : "mm" }
export const millimeter = 0.001 * meter;

/** A constant equal to 1 inch. */
annotation { "Name" : "Inch", "Abbreviation" : "in" }
export const inch = 2.54 * centimeter;

/** A constant equal to 1 foot. */
annotation { "Name" : "Foot", "Abbreviation" : "ft" }
export const foot = 12 * inch;

/** A constant equal to 1 yard. */
annotation { "Name" : "Yard", "Abbreviation" : "yd" }
export const yard = 3 * foot;

/**
 * A constant equal to 1 radian.
 *
 * Formally, radians are unitless, so in certain situations you may need to
 * multiply or divide by `radian`
 *
 * @example `var myAngle = PI / 6 * radian`
 * @example `var arcLength = radius * arcAngle / radian`
 */
annotation { "Name" : "Radian", "Abbreviation" : "rad" }
export const radian = { "value" : 1, "unit" : ANGLE_UNITS } as ValueWithUnits;

/** A constant equal to 1 degree. */
annotation { "Name" : "Degree", "Abbreviation" : "deg" }
export const degree = 0.0174532925199432957692 * radian;

/** A constant equal to 1 kilogram. */
annotation { "Name" : "Kilogram", "Abbreviation" : "kg" }
export const kilogram = { "value" : 1, "unit" : MASS_UNITS } as ValueWithUnits;

/** A constant equal to 1 gram. */
annotation { "Name" : "Gram", "Abbreviation" : "g" }
export const gram = 0.001 * kilogram;

/** A constant equal to 1 ounce. */
annotation { "Name" : "Ounce", "Abbreviation" : "oz" }
export const ounce = 28.349523 * gram;

/** A constant equal to 1 pound. */
annotation { "Name" : "Pound", "Abbreviation" : "lb" }
export const pound = 16 * ounce;

/** A constant equal to 1 second */
annotation { "Name" : "Second", "Abbreviation" : "s" }
export const second = { "value" : 1, "unit" : TIME_UNITS } as ValueWithUnits;

/** @internal */
export const STRING_TO_UNIT_MAP = {
  "Meter" : meter,
  "meter" : meter,
  "m" : meter,
  "Centimeter" : centimeter,
  "centimeter" : centimeter,
  "cm" : centimeter,
  "Millimeter" : millimeter,
  "millimeter" : millimeter,
  "mm" : millimeter,
  "Inch" : inch,
  "inch" : inch,
  "in" : inch,
  "Foot" : foot,
  "foot" : foot,
  "ft" : foot,
  "Yard" : yard,
  "yard" : yard,
  "yd" : yard,
  "Radian" : radian,
  "radian" : radian,
  "rad" : radian,
  "Degree" : degree,
  "degree" : degree,
  "deg" : degree,
  "Kilogram" : kilogram,
  "kilogram" : kilogram,
  "kg" : kilogram,
  "Gram" : gram,
  "gram" : gram,
  "g" : gram,
  "Ounce" : ounce,
  "ounce" : ounce,
  "oz" : ounce,
  "Pound" : pound,
  "pound" : pound,
  "lb" : pound
};

/**
 * True for any value with length units.
 */
export predicate isLength(val)
{
    val is ValueWithUnits;
    val.unit == LENGTH_UNITS;
}

/**
 * True for any value with angle units.
 */
export predicate isAngle(val)
{
    val is ValueWithUnits;
    val.unit == ANGLE_UNITS;
}

export operator<(lhs is ValueWithUnits, rhs is ValueWithUnits) returns boolean
precondition lhs.unit == rhs.unit;
{
    return lhs.value < rhs.value;
}

export operator<(lhs is ValueWithUnits, rhs is number) returns boolean
precondition
{
    annotation { 'Message' : 'ValueWithUnits compared to nonzero number' }
    rhs == 0;
}
{
    return lhs.value < rhs;
}

export operator<(lhs is number, rhs is ValueWithUnits) returns boolean
precondition
{
    annotation { 'Message' : 'ValueWithUnits compared to nonzero number' }
    lhs == 0;
}
{
    return lhs < rhs.value;
}


export operator+(lhs is ValueWithUnits, rhs is ValueWithUnits) returns ValueWithUnits
precondition lhs.unit == rhs.unit;
{
    lhs.value += rhs.value;
    return lhs;
}

export operator-(lhs is ValueWithUnits, rhs is ValueWithUnits) returns ValueWithUnits
precondition lhs.unit == rhs.unit;
{
    lhs.value -= rhs.value;
    return lhs;
}

export operator-(rhs is ValueWithUnits) returns ValueWithUnits
{
    rhs.value = -rhs.value;
    return rhs;
}

export operator*(lhs is number, rhs is ValueWithUnits) returns ValueWithUnits
{
    rhs.value *= lhs;
    return rhs;
}

export operator*(lhs is ValueWithUnits, rhs is number) returns ValueWithUnits
{
    lhs.value *= rhs;
    return lhs;
}

/**
 * Multiplication of two `ValueWithUnits` normally returns a `ValueWithUnits`.
 * Multiplying two lengths returns an area.  As a special case, if the product
 * is unitless the result is a scalar `number`.
 */
export operator*(lhs is ValueWithUnits, rhs is ValueWithUnits) // May return ValueWithUnits or number
{
    var newUnit = unitProduct(lhs.unit, rhs.unit);

    if (newUnit == unitless)
        return lhs.value * rhs.value;

    return { "value" : lhs.value * rhs.value, "unit" : newUnit } as ValueWithUnits;
}

/**
 * Inverts a value, including units.
 */
export function reciprocal(val is ValueWithUnits) returns ValueWithUnits
{
    for (var unit in val.unit)
    {
        val.unit[unit.key] *= -1;
    }
    val.value = 1 / val.value;
    return val;
}

export operator/(lhs is ValueWithUnits, rhs is ValueWithUnits) // May return ValueWithUnits or number
{
    return lhs * reciprocal(rhs);
}

export operator/(lhs is number, rhs is ValueWithUnits) returns ValueWithUnits
{
    return lhs * reciprocal(rhs);
}

export operator/(lhs is ValueWithUnits, rhs is number) returns ValueWithUnits
{
    lhs.value /= rhs;
    return lhs;
}

export operator%(lhs is ValueWithUnits, rhs is ValueWithUnits) returns ValueWithUnits
precondition lhs.unit == rhs.unit;
{
    lhs.value %= rhs.value;
    return lhs;
}

export operator^(lhs is ValueWithUnits, rhs is number)
precondition
{
    for (var unit in lhs.unit)
        (unit.value * rhs) % 1 == 0;
}
{
    if (rhs == 0)
        return 1;

    lhs.value = lhs.value ^ rhs;
    for (var unit in lhs.unit)
    {
        lhs.unit[unit.key] = unit.value * rhs;
    }
    return lhs;
}

/** Returns true if angles are equal up to zeroAngle or anything else is equal up to zeroLength */
export predicate tolerantEquals(value1 is ValueWithUnits, value2 is ValueWithUnits)
precondition value1.unit == value2.unit;
{
    if (value1.unit == ANGLE_UNITS)
        abs(value1.value - value2.value) < TOLERANCE.zeroAngle;
    else
        abs(value1.value - value2.value) < TOLERANCE.zeroLength;
}

/**
 * Square root of a `ValueWithUnits`.
 *
 * @example `sqrt(4 * meter^2)` equals `2 * meter`.
 * @example `2 * PI * sqrt(armLength / (9.8 * meter/second^2))` equals the
 *          period of a pendulum, in seconds.
 * @example `sqrt(4 * meter)` throws an error, since FeatureScript has no
 *          concept of the square root of a meter.
 * @param value : @autocomplete `4 * inch^2`
 */
export function sqrt(value is ValueWithUnits) returns ValueWithUnits
precondition
{
    for (var unit in value.unit)
        unit.value % 2 == 0;
}
{
    value.value = sqrt(value.value);
    for (var unit in value.unit)
        value.unit[unit.key] = unit.value / 2;
    return value;
}

/**
 * Hypotenuse function, as `sqrt(a^2 + b^2)`, but without any
 * surprising results due to finite numeric precision.
 *
 * @example `hypot(3 * foot, 4 * foot)` equals `5 * foot`
 */
export function hypot(a is ValueWithUnits, b is ValueWithUnits)
precondition a.unit == b.unit;
{
    return { value : @hypot(a.value, b.value), unit : a.unit } as ValueWithUnits;
}

/**
 * Sine, the ratio of the opposite side over the hypotenuse in a right triangle
 * of the specified angle.
 *
 * @example `sin(30 * degree)` returns approximately `0.5`
 * @example `sin(PI * radian)` returns approximately `0`
 * @param value : @autocomplete `30 * degree`
 */
export function sin(value is ValueWithUnits) returns number
precondition isAngle(value);
{
    return @sin(value.value);
}

/**
 * Cosine, the ratio of the adjacent side over the hypotenuse in a right triangle
 * of the specified angle.
 *
 * @example `cos(60 * degree)` returns approximately `0.5`
 * @example `cos(PI * radian)` returns approximately `-1`
 * @param value : @autocomplete `30 * degree`
 */
export function cos(value is ValueWithUnits) returns number
precondition isAngle(value);
{
    return @cos(value.value);
}

/**
 * Tangent, the ratio of the opposite side over the adjacent side in a right
 * triangle of the specified angle.
 *
 * @example `tan(45 * degree)` returns approximately `1`
 * @example `tan(PI * radian)` returns approximately `0`
 * @param value : @autocomplete `30 * degree`
 */
export function tan(value is ValueWithUnits) returns number
precondition isAngle(value);
{
    return @tan(value.value);
}

/**
 * Arcsine, i.e. inverse sine.
 *
 * Returns a value between `-90 * degree` and `90 * degree`.
 *
 * @example `asin(0.5)` equals approximately `30 * degree`
 * @example `asin(1.5)` throws an error, since there is no value where
 *          `sin(value)` is `1.5`
 */
export function asin(value is number) returns ValueWithUnits
{
    return @asin(value) * radian;
}

/**
 * Arccosine, i.e. inverse cosine.
 *
 * Returns a value between `0 * degree` and `180 * degree`.
 *
 * @example `acos(0.5)` equals approximately `60 * degree`
 * @example `acos(1.5)` throws an error, since there is no value where
 *          `cos(value)` is `1.5`
 */
export function acos(value is number) returns ValueWithUnits
{
    return @acos(value) * radian;
}

/**
 * Arctangent, i.e. inverse tangent.
 *
 * Returns a value between `-90 * degree` and `90 * degree`.
 *
 * @example `atan(1)` equals approximately `45 * degree`
 * @example `atan(inf)` equals approximately `90 * degree`
 */
export function atan(value is number) returns ValueWithUnits
{
    return @atan(value) * radian;
}

/**
 * Returns the counterclockwise angle from the vector `[0, 1]` to the vector `[x, y]`.
 * The angle is negative if y is negative. This is equivalent to `atan(y/x)`
 * except the result respects the quadrant of the input and is well-behaved
 * near x == 0.
 *
 * @example `atan2(0, 1)` equals approximately `0 * degree`
 * @example `atan2(1, 0)` equals approximately `90 * degree`
 * @example `atan2(0, -1)` equals approximately `180 * degree`
 * @example `atan2(-1, 0)` equals approximately `-90 * degree`
 */
export function atan2(y is number, x is number) returns ValueWithUnits
{
    return @atan2(y, x) * radian;
}

/**
 * Returns the counterclockwise angle from the vector `[0, 1]`
 * to the vector `[x, y]`, assuming the units of `y` and `x`
 * match.
 *
 * @seealso [atan2(number, number)]
 * @param y : @autocomplete `1 * inch`
 * @param x : @autocomplete `-1 * inch`
 */
export function atan2(y is ValueWithUnits, x is ValueWithUnits) returns ValueWithUnits
precondition y.units == x.units;
{
    return @atan2(y.value, x.value) * radian;
}

/**
 * Round a value down to nearest given multiple.
 *
 * @example `floor(125, 10)` returns `120`
 * @example `floor(-15, 10)` returns `-20`
 * @example `floor(3.14 * inch, 0.1 * inch)` equals `3.1 * inch`
 */
export function floor(value, multiple)
precondition unitsMatch(value, multiple);
{
    return @floor(value / multiple) * multiple;
}

/**
 * Round a value up to nearest given multiple.
 *
 * @example `ceil(125, 10)` returns `130`
 * @example `ceil(-15, 10)` returns `-10`
 * @example `ceil(3.14 * inch, 0.1 * inch)` equals `3.2 * inch`
 */
export function ceil(value, multiple)
precondition unitsMatch(value, multiple);
{
    return @ceil(value / multiple) * multiple;
}

/**
 * Round a value to nearest given multiple.
 *
 * @example `round(125, 10)` returns `130`
 * @example `round(-15, 10)` returns `-10`
 * @example `round((10 / 3) * meter, centimeter)` equals `3.33 * meter`
 * @example `round(1 * meter, .001 * inch)` equals `39.37 * inch`
 *
 * For small values of `multiple`, [roundToPrecision] is preferred to reduce
 * floating point errors.
 */
export function round(value, multiple)
precondition unitsMatch(value, multiple);
{
    return @floor((value / multiple) + 0.5) * multiple;
}

predicate unitsMatch(value, multiple)
{
    if (value is number)
    {
        annotation { 'Message' : 'Rounding multiple must be unitless if rounded value is unitless' }
        multiple is number;
    }
    else if (value is ValueWithUnits)
    {
        annotation { 'Message' : 'Rounding multiple must have same units as rounded value' }
        multiple is ValueWithUnits;
        annotation { 'Message' : 'Rounding multiple must have same units as rounded value' }
        value.unit == multiple.unit;
    }
    else
    {
        (value / multiple) is number;
    }
}

/**
 * General value to string conversion.
 */
export function toString(value is ValueWithUnits) returns string
{
    var result = value.value ~ "";
    for (var unit in value.unit)
    {
        result ~= " " ~ unit.key;
        if (unit.value != 1)
            result ~= "^" ~ unit.value;
    }
    return result;
}

/**
 * @internal
 *
 * `stripUnits` removes all units from a `ValueWithUnits` leaving only
 * the underlying numeric value in Onshape internal units.  If the argument
 * is a container, units are stripped recursively from contents.
 * Resulting values are meant for use in builtin functions.
 */
export function stripUnits(value)
{
    return value;
}

/** @internal */
export function stripUnits(value is ValueWithUnits)
{
    return value.value;
}

/** @internal */
export function stripUnits(value is array) returns array
{
    for (var i = 0; i < @size(value); i += 1)
    {
        value[i] = stripUnits(value[i]);
    }
    return value as array;
}

/** @internal */
export function stripUnits(value is map) returns map
{
    for (var entry in value)
    {
        value[entry.key] = stripUnits(entry.value);
    }
    return value as map;
}

/** @internal */
export function evaluateExpression(expression is number, expectedUnit is number) returns map
{
    return { 'status' : ExpressionValidationResult.VALID, 'value' : expression };
}

/** @internal */
export function evaluateExpression(expression is number, expectedUnit is ValueWithUnits) returns map
{
    return { 'status' : ExpressionValidationResult.NO_UNIT, 'value' : expression * expectedUnit.value };
}

/** @internal */
export function evaluateExpression(expression is ValueWithUnits, expectedUnit is ValueWithUnits) returns map
{
    if (expression.unit == expectedUnit.unit)
    {
        return { 'status' : ExpressionValidationResult.VALID, 'value' : expression.value };
    }

    return { 'status' : ExpressionValidationResult.ERROR, 'value' : 0.0 };
}

/** @internal */
export const REGEX_UNITS = buildRegexUnits();

function buildRegexUnits() returns string
{
  var result = "((?:";
  for (var entry in STRING_TO_UNIT_MAP)
  {
      result = result ~ entry.key ~ "|";
  }
  result = replace(result, "\\|$", ")\\b)");
  return result;
}

/** @internal */
export function stringToUnit(unitStr is string) returns ValueWithUnits
{
    var result = STRING_TO_UNIT_MAP[unitStr];
    if (result != undefined)
    {
        return result;
    }
    throw "Unexpected unit:" ~ unitStr;
}

/** @internal
 *
 * Given two values with units, return the units of their product. If the result
 * is unitless, returns the unitless constant.
 */
export function unitProduct(unit1 is UnitSpec, unit2 is UnitSpec)
{
    var newUnit = unit1;
    for (var unit in unit2)
    {
        if (unit1[unit.key] == undefined)
        {
            newUnit[unit.key] = unit.value;
        }
        else
        {
            const sum = unit1[unit.key] + unit.value;
            if (sum == 0)
                newUnit[unit.key] = undefined;
            else
                newUnit[unit.key] = sum;
        }
    }

    if (@size(newUnit) == 0)
        return unitless;

    return newUnit;
}

/**
 * Parse a JSON string into either a map or array. Null values in the JSON
 * are returned as `undefined`. Throws if the string is not well-formed JSON.
 * Applicable strings are parsed into a ValueWithUnits. For instance, "3 inch"
 * will map to a `ValueWithUnits` with length units that repreresents 3 inches.
 *
 * @return: A map or an array corresponding to the JSON value.
 */
export function parseJsonWithUnits(s is string)
{
    return @parseJson(s, {stringToUnitMap: STRING_TO_UNIT_MAP});
}
