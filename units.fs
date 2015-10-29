FeatureScript 244; /* Automatically generated version */
import(path : "onshape/std/math.fs", version : "");
import(path : "onshape/std/expressionvalidationresult.gen.fs", version : "");

/**
 * For Onshape internal use only.
 * A `UnitSpec` is a fundamental dimension like length and time.
 * Angle is treated as a dimension although formally it is dimensionless.
 */
export type UnitSpec typecheck canBeUnitSpec;

export const LENGTH_UNITS = { "meter" : 1 } as UnitSpec;
export const ANGLE_UNITS = { "radian" : 1 } as UnitSpec;
export const MASS_UNITS = { "kilogram" : 1 } as UnitSpec;
export const TEMPERATURE_UNITS = { "kelvin" : 1 } as UnitSpec;
export const TIME_UNITS = { "second" : 1 } as UnitSpec;
export const CURRENT_UNITS = { "ampere" : 1 } as UnitSpec;

//TODO: we probably want separate documents for standard units so as not to pollute the namespace
export const unitless = 1;

annotation { "Name" : "Meter", "Abbreviation" : "m" }
export const meter = { "value" : 1, "unit" : LENGTH_UNITS } as ValueWithUnits;
annotation { "Name" : "Centimeter", "Abbreviation" : "cm" }
export const centimeter = 0.01 * meter;
annotation { "Name" : "Millimeter", "Abbreviation" : "mm" }
export const millimeter = 0.001 * meter;

annotation { "Name" : "Inch", "Abbreviation" : "in" }
export const inch = 2.54 * centimeter;
annotation { "Name" : "Foot", "Abbreviation" : "ft" }
export const foot = 12 * inch;
annotation { "Name" : "Yard", "Abbreviation" : "yd" }
export const yard = 3 * foot;

annotation { "Name" : "Radian", "Abbreviation" : "rad" }
export const radian = { "value" : 1, "unit" : ANGLE_UNITS } as ValueWithUnits;
annotation { "Name" : "Degree", "Abbreviation" : "deg" }
export const degree = 0.0174532925199432957692 * radian;

annotation { "Name" : "Kilogram", "Abbreviation" : "kg" }
export const kilogram = { "value" : 1, "unit" : MASS_UNITS } as ValueWithUnits;
annotation { "Name" : "Gram", "Abbreviation" : "g" }
export const gram = 0.001 * kilogram;
annotation { "Name" : "Ounce", "Abbreviation" : "oz" }
export const ounce = 28.349523 * gram;
annotation { "Name" : "Pound", "Abbreviation" : "lb" }
export const pound = 16 * ounce;

/**
 * A `ValueWithUnits` is a number with dimensions, such as 1 kilogram,
 * 90 degrees, or 9.81 meters per second per second.
 *
 * Values can be multiplied and divided.   The preceding values are
 * `1 * kilogram`, `90 * degree`, and `9.81 * meter / second / second`.
 *
 * Values with the same units can be added and subtracted.
 * Square root works if the units appear as even powers, as
 * in `sqrt(4 * meter * meter)`.
 *
 * Equality considers the underlying value, so 25.4 millimeters is the
 * same as 1 inch.  (But `PI * radian` may not equal `180 * degree`
 * because of finite precision arithmetic.)
 */
export type ValueWithUnits typecheck canBeValueWithUnits;

export predicate canBeValueWithUnits(value)
{
    value is map;
    value.value is number;
    value.unit is UnitSpec;
}

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

export predicate isLength(val)
{
    val is ValueWithUnits;
    val.unit == LENGTH_UNITS;
}

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
precondition rhs == 0;
{
    return lhs.value < rhs;
}

export operator<(lhs is number, rhs is ValueWithUnits) returns boolean
precondition lhs == 0;
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
    var newUnit = lhs.unit;
    for (var unit in rhs.unit)
    {
        if (lhs.unit[unit.key] == undefined)
        {
            newUnit[unit.key] = unit.value;
        }
        else
        {
            const sum = lhs.unit[unit.key] + unit.value;
            if (sum == 0)
                newUnit[unit.key] = undefined;
            else
                newUnit[unit.key] = sum;
        }
    }

    if (@size(newUnit) == 0)
        return lhs.value * rhs.value;

    return { "value" : lhs.value * rhs.value, "unit" : newUnit } as ValueWithUnits;
}

function reciprocal(val is ValueWithUnits) returns ValueWithUnits
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
precondition
{
    lhs.unit == rhs.unit;
}
{
    lhs.value %= rhs.value;
    return lhs;
}

export operator^(lhs is ValueWithUnits, rhs is number) returns ValueWithUnits
precondition
{
    for (var unit in lhs.unit)
        (unit.value * rhs) % 1 == 0;
}
{
    lhs.value = lhs.value ^ rhs;
    for (var unit in lhs.unit)
    {
        lhs.unit[unit.key] = unit.value * rhs;
    }
    return lhs;
}

/**
 * Square root
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
 * Sine
 */
export function sin(value is ValueWithUnits) returns number
precondition isAngle(value);
{
    return @sin(value.value);
}

/**
 * Cosine
 */
export function cos(value is ValueWithUnits) returns number
precondition isAngle(value);
{
    return @cos(value.value);
}

/**
 * Tangent
 */
export function tan(value is ValueWithUnits) returns number
precondition isAngle(value);
{
    return @tan(value.value);
}

/**
 * Arcsine (inverse sine)
 * @param value
 */
export function asin(value is number) returns ValueWithUnits
{
    return @asin(value) * radian;
}

/**
 * Arccosine (inverse cosine)
 * @param value
 */
export function acos(value is number) returns ValueWithUnits
{
    return @acos(value) * radian;
}

/**
 * Arctangent (inverse tangent)
 */
export function atan(value is number) returns ValueWithUnits
{
    return @atan(value) * radian;
}

/**
 * `atan2(y, x)` returns the counterclockwise angle from [0, 1] to [x, y].
 * The angle is negative if y is negative.  This is equivalent to `atan(y/x)`
 * except the result respects the quadrant of the input and is well-behaved
 * near x == 0.
 */
export function atan2(y is number, x is number) returns ValueWithUnits
{
    return @atan2(y, x) * radian;
}

/**
 * `atan2(y, x)` returns the inverse tangent of `(y/x)`, using the
 * signs of its argument to choose a quadrant for the result.
 */
export function atan2(value1 is ValueWithUnits, value2 is ValueWithUnits) returns ValueWithUnits
precondition value1.units == value2.units;
{
    return @atan2(value1.value, value2.value) * radian;
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
 * For Onshape internal use.
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

export function stripUnits(value is ValueWithUnits)
{
    return value.value;
}

export function stripUnits(value is array) returns array
{
    for (var i = 0; i < @size(value); i += 1)
    {
        value[i] = stripUnits(value[i]);
    }
    return value as array;
}

export function stripUnits(value is map) returns map
{
    for (var entry in value)
    {
        value[entry.key] = stripUnits(entry.value);
    }
    return value as map;
}

/**
 * TODO: description
 * @param expression
 * @param expectedUnit
 */
export function evaluateExpression(expression is number, expectedUnit is number) returns map
{
    return { 'status' : ExpressionValidationResult.VALID, 'value' : expression };
}

export function evaluateExpression(expression is number, expectedUnit is ValueWithUnits) returns map
{
    return { 'status' : ExpressionValidationResult.NO_UNIT, 'value' : expression * expectedUnit.value };
}

export function evaluateExpression(expression is ValueWithUnits, expectedUnit is ValueWithUnits) returns map
{
    if (expression.unit == expectedUnit.unit)
    {
        return { 'status' : ExpressionValidationResult.VALID, 'value' : expression.value };
    }

    return { 'status' : ExpressionValidationResult.ERROR, 'value' : 0.0 };
}




