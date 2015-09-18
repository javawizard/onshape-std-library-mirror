FeatureScript ✨; /* Automatically generated version */
//wrappers around mathematical builtin functions and some constants

export const PI = 3.1415926535897932384626433832795;

export const TOLERANCE =
{
    "zeroAngle" : 1e-11,
    "zeroLength" : 1e-8
};

/**
 * TODO: description
 * @param value
 */
export function abs(value)
{
    return value < 0 ? -value : value;
}

/**
 * TODO: description
 * @param value
 */
export function sqrt(value is number)
{
    return @sqrt(value);
}

/**
 * TODO: description
 * @param value
 */
export function log(value is number)
{
    return @log(value);
}

/**
 * TODO: description
 * @param value
 */
export function log10(value is number)
{
    return @log10(value);
}

// sin, cos, tan, asin, acos, atan, atan2  are in units.fs to deal with angular units properly
// corresponding builtin functions (@sin etc) can be used to bypass the units.

/**
 * TODO: description
 * @param value
 */
export function sinh(value is number)
{
    return @sinh(value);
}

/**
 * TODO: description
 * @param value
 */
export function cosh(value is number)
{
    return @cosh(value);
}

/**
 * TODO: description
 * @param value
 */
export function tanh(value is number)
{
    return @tanh(value);
}

/**
 * TODO: description
 * @param value
 */
export function asinh(value is number)
{
    return @asinh(value);
}

/**
 * TODO: description
 * @param value
 */
export function acosh(value is number)
{
    return @acosh(value);
}

/**
 * TODO: description
 * @param value
 */
export function atanh(value is number)
{
    return @atanh(value);
}

/**
 * TODO: description
 * @param value
 */
export function exp(value is number)
{
    return @exp(value);
}

/**
 * TODO: description
 * @param value
 */
export function exp2(value is number)
{
    return @exp2(value);
}

/**
 * TODO: description
 * @param value1
 * @param value2
 */
export function pow(value1 is number, value2 is number)
{
    return @pow(value1, value2);
}

/**
 * TODO: description
 * @param a
 * @param b
 */
export function hypot(a is number, b is number)
{
    return @hypot(a, b);
}

/**
 * TODO: description
 * @param value
 */
export function degreesToRadians(value is number)
{
    return (value * (PI / 180));
}

/**
 * TODO: description
 * @param value
 */
export function radiansToDegrees(value is number)
{
    return (value * (180 / PI));
}

/**
 * TODO: description
 * @param value
 */
export function floor(value is number)
{
    return @floor(value);
}

/**
 * TODO: description
 * @param value
 */
export function ceil(value is number)
{
    return @ceil(value);
}

/**
 * TODO: description
 * @param value
 */
export function round(value is number)
{
    return @floor(value + 0.5);
}

/**
 * TODO: description
 * @param value1
 * @param value2
 */
export function min(value1, value2)
{
    return (value1 < value2) ? value1 : value2;
}

/**
 * TODO: description
 * @param value1
 * @param value2
 */
export function max(value1, value2)
{
    return (value1 < value2) ? value2 : value1;
}

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
 * TODO: description
 * @param arr
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
 * TODO: description
 * @param arr
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
 * TODO: description
 * @param from
 * @param to
 */
export function range(from is number, to is number)
{
    return range(from, (to < from) ? -1 : 1, to);
}

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
 * TODO: description
 * @param value
 * @param lowerBound
 * @param higherBound
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
}

export predicate isNonNegativeInteger(value)
{
    value is number;
    value >= 0;
    @floor(value) == value;
}

export predicate isPositiveInteger(value)
{
    value is number;
    value > 0;
    @floor(value) == value;
}

