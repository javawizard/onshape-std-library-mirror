FeatureScript 307; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

//wrappers around mathematical builtin functions and some constants

export const PI = 3.1415926535897932384626433832795;

export const TOLERANCE =
{
    "zeroAngle" : 1e-11,
    "zeroLength" : 1e-8
};

/** Absolute value */
export function abs(value)
{
    return value < 0 ? -value : value;
}

/** Square root */
export function sqrt(value is number)
{
    return @sqrt(value);
}

/** Natural logarithm */
export function log(value is number)
{
    return @log(value);
}

/** Base 10 logarithm */
export function log10(value is number)
{
    return @log10(value);
}

// sin, cos, tan, asin, acos, atan, atan2  are in units.fs to deal with angular units properly
// corresponding builtin functions (@sin etc) can be used to bypass the units.

/** Hyperbolic sine */
export function sinh(value is number)
{
    return @sinh(value);
}

/** Hyperbolic cosine */
export function cosh(value is number)
{
    return @cosh(value);
}

/** Hyperbolic tangent */
export function tanh(value is number)
{
    return @tanh(value);
}

/** Hyperbolic arcsine (inverse cosine) */
export function asinh(value is number)
{
    return @asinh(value);
}

/** Hyperbolic arccosine (inverse cosine) */
export function acosh(value is number)
{
    return @acosh(value);
}

/** Hyperbolic arctangent (inverse tangent) */
export function atanh(value is number)
{
    return @atanh(value);
}

/** Exponentiation, e^x */
export function exp(value is number)
{
    return @exp(value);
}

/** Exponentiation, 2^x */
export function exp2(value is number)
{
    return @exp2(value);
}

/** First argument raised to power of second, x^y */
export function pow(base is number, exponent is number)
{
    return @pow(base, exponent);
}

/**
 * Hypotenuse function, `hypot(x, y) == sqrt(x^2 + y^2)` but without any
 * surprising results due to finite numeric precision.
 */
export function hypot(a is number, b is number)
{
    return @hypot(a, b);
}

/** Round a number down to the nearest integer. */
export function floor(value is number)
{
    return @floor(value);
}

/** Round a number up to the nearest integer. */
export function ceil(value is number)
{
    return @ceil(value);
}

/**
 * Round a number to the nearest integer. x.5 always rounds up to (x+1).
 */
export function round(value is number)
{
    return @floor(value + 0.5);
}

/**
 * Round a number to the nearest integer only for possible roundoff errors
 */
export function roundWithinTolerance(value is number)
{
    var rounded = round(value);
    if (abs(rounded - value) <= TOLERANCE.zeroLength)
    {
        return rounded;
    }
    return value;
}

/** Return the lesser of two values, which must be comparable with `<`. */
export function min(value1, value2)
{
    return (value1 < value2) ? value1 : value2;
}

/** Return the greater of two values, which must be comparable with `<`. */
export function max(value1, value2)
{
    return (value1 < value2) ? value2 : value1;
}

/**
 * Return the least of an array of values, as determined by operator `<`,
 * or undefined if the array is empty.
 */
export function min(arr is array)
{
    var minVal = undefined;
    for (var entry in arr)
    {
        if (minVal == undefined || entry < minVal)
        {
            minVal = entry;
        }
    }
    return minVal;
}

/**
 * Return the greatest of an array of values, as determined by operator `<`,
 * or undefined if the array is empty.
 */
export function max(arr is array)
{
    var maxVal = undefined;
    for (var entry in arr)
    {
        if (maxVal == undefined || maxVal < entry)
        {
            maxVal = entry;
        }
    }
    return maxVal;
}

/**
 * Return the index of the smallest element of an array, as determined
 * by operator `<`, or undefined if the array is empty.
 */
export function argMin(arr is array)
{
    var minVal = undefined;
    var minIndex = undefined;
    for (var i = 0; i < @size(arr); i += 1)
    {
        if (minVal == undefined || arr[i] < minVal)
        {
            minVal = arr[i];
            minIndex = i;
        }
    }
    return minIndex;
}

/**
 * Return the index of the largest element of an array, as determined
 * by the `>` operator, or undefined if the array is empty.
 */
export function argMax(arr is array)
{
    var maxVal = undefined;
    var maxIndex = undefined;
    for (var i = 0; i < @size(arr); i += 1)
    {
        if (maxVal == undefined || maxVal < arr[i])
        {
            maxVal = arr[i];
            maxIndex = i;
        }
    }
    return maxIndex;
}

/**
 * Return an array of numbers (normally integers) in a range.
 */
export function range(from is number, to is number)
{
    return range(from, (to < from) ? -1 : 1, to);
}

/**
 * Return an array of numbers, or other values that behave like
 * numbers when added (`operator +`), subtracted (`operator -`),
 * and divided (`operator /`), in a range.  If the difference between
 * bounds is a multiple of step size, the upper bound is included.
 * `range(0, 2, 10) == [0, 2, 4, 6, 8, 10]`
 * `range(0, 1.5, 5) == [0, 1.5, 3, 4.5]`
 */
export function range(from, step, to)
precondition
{
    (step > 0 && (from <= to)) ||
        (step < 0 && (from >= to));
}
{
    const num = floor(1 + (to - from) / step);
    var out = @resize([], num);
    var cur = from;
    for (var i = 0; i < num; i += 1)
    {
        out[i] = cur;
        cur += step;
    }
    return out;
}

/**
 * Force a value into a range, @example `clamp(-1, 0, 20)` returns `0`,
 * @example `clamp(10, 0, 20)` returns `10`, and
 * @example `clamp(30, 0, 20)` returns `20`.
 */
export function clamp(value, lowerBound, higherBound)
precondition
{
    lowerBound <= higherBound;
}
{
    if (value < lowerBound)
        return lowerBound;
    if (value > higherBound)
        return higherBound;
    return value;
}

export predicate isInteger(value)
{
    value is number;
    @floor(value) == value;
    value < inf;
    value > -inf;
}

export predicate isNonNegativeInteger(value)
{
    value is number;
    value >= 0;
    value < inf;
    @floor(value) == value;
}

export predicate isPositiveInteger(value)
{
    value is number;
    value > 0;
    value < inf;
    @floor(value) == value;
}

