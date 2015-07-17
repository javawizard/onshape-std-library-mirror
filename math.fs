FeatureScript ✨; /* Automatically generated version */
//wrappers around mathematical builtin functions and some constants

export const PI = 3.1415926535897932384626433832795;

export const TOLERANCE =
{
    "zeroAngle" : 1e-11,
    "zeroLength" : 1e-8
};

export function abs(value is number)
{
    return value < 0 ? -value : value;
}

export function sqrt(value is number)
{
    return @sqrt(value);
}

export function log(value is number)
{
    return @log(value);
}

export function log10(value is number)
{
    return @log10(value);
}

// sin, cos, tan, asin, acos, atan, atan2  are in units.fs to deal with angular units properly
// corresponding builtin functions (@sin etc) can be used to bypass the units.

export function sinh(value is number)
{
    return @sinh(value);
}

export function cosh(value is number)
{
    return @cosh(value);
}

export function tanh(value is number)
{
    return @tanh(value);
}

export function asinh(value is number)
{
    return @asinh(value);
}

export function acosh(value is number)
{
    return @acosh(value);
}

export function atanh(value is number)
{
    return @atanh(value);
}

export function exp(value is number)
{
    return @exp(value);
}

export function exp2(value is number)
{
    return @exp2(value);
}

export function pow(value1 is number, value2 is number)
{
    return @pow(value1, value2);
}

export function hypot(a is number, b is number)
{
    return @hypot(a, b);
}

export function degreesToRadians(value is number)
{
    return (value * (PI / 180));
}

export function radiansToDegrees(value is number)
{
    return (value * (180 / PI));
}

export function floor(value is number)
{
    return @floor(value);
}

export function ceil(value is number)
{
    return @ceil(value);
}

export function round(value is number)
{
    return @floor(value + 0.5);
}

export function min(value1, value2)
{
    return (value1 < value2) ? value1 : value2;
}

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

export function range(from is number, to is number)
{
    return range(from, (to < from) ? -1 : 1, to);
}

export function range(from is number, step is number, to is number)
precondition
{
    (step > 0 && (from <= to)) ||
        (step < 0 && (from >= to));
}
{
    var num = floor(1 + (to - from) / step);
    var out = @resize([], num);
    var cur = from;
    for (var i = 0; i < num; i += 1)
    {
        out[i] = cur;
        cur += step;
    }
    return out;
}

