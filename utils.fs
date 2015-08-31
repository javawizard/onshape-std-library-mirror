FeatureScript ✨; /* Automatically generated version */
//simple utilities

export function isIn(value, container is array) returns boolean
{
    for (var element in container)
    {
        if (element == value)
            return true;
    }
    return false;
}

export function isValueIn(value, container is map) returns boolean
{
    for (var entry in container)
    {
        if (entry.value == value)
            return true;
    }
    return false;
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

export function makeArray(size is number) returns array
precondition isNonNegativeInteger(size);
{
    return @resize([], size);
}

export function makeArray(size is number, fillValue) returns array
precondition isNonNegativeInteger(size);
{
    return @resize([], size, fillValue);
}

export function size(container is array) returns number
{
    return @size(container);
}

export function size(container is map) returns number
{
    return @size(container);
}

export function resize(arr is array, newSize is number) returns array
precondition isNonNegativeInteger(newSize);
{
    return @resize(arr, newSize);
}

export function resize(arr is array, newSize is number, newValue) returns array
precondition isNonNegativeInteger(newSize);
{
    return @resize(arr, newSize, newValue);
}

export function append(arr is array, newValue) returns array
{
    return @resize(arr, @size(arr) + 1, newValue);
}

export function concatenateArrays(arr is array) returns array
precondition
{
    for (var a in arr)
        a is array;
}
{
    var totalSize = 0;

    for (var a in arr)
        totalSize += @size(a);

    var out = makeArray(totalSize);

    var i = 0;
    for (var a in arr)
    {
        for (var j = 0; j < @size(a); j += 1)
        {
            out[i] = a[j];
            i += 1;
        }
    }
    return out;
}

export function mergeMaps(map1 is map, map2 is map) returns map
{
    return @mergeMaps(map1, map2);
}

export function reverse(arr is array) returns array
{
    var size = @size(arr);
    var out = makeArray(size);
    for (var i = 0; i < size; i += 1)
    {
        out[i] = arr[size - 1 - i];
    }
    return out;
}

export function splitIntoCharacters(s is string) returns array
{
    return @splitIntoCharacters(s);
}

export function replace(s is string, regExp is string, replacement is string) returns string
{
    return @replace(s, regExp, replacement);
}

export function length(s is string) returns number
{
    return size(@splitIntoCharacters(s));
}

/* mapLookup is a hopefully temporary function.

   mapLookup(a, ["b","c","d","e"])

   is the same as

   try (a.b.c.d.e)

   except it does not log a warning.
 */
export function mapLookup(m is map, keys is array)
{
    var result = m;
    for (var key in keys)
    {
        if (!(result is map))
            return undefined;
        result = result[key];
    }
    return result;
}

export function libraryLanguageVersion()
{
    return @getLanguageVersion();
}

export function clamp(value is number, lowerBound is number, higherBound is number)
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

/*
 Merge Sort

 compareFunction(a, b) takes two entities, and returns
    -1 if a is before b
    0  if a == b
    1  if a is after b
*/
export function sort(entities is array, compareFunction is function)
{
    const totalSize is number = size(entities);
    var result = [entities, makeArray(totalSize)];
    var t = 0;
    var length = 1;
    var doubleLength = length * 2;
    while (length < totalSize)
    {
        t = 1 - t;
        for (var start = 0; start < totalSize; start += doubleLength)
        {
            var endLeft = clamp(start + length, 0, totalSize);
            var endRight = clamp(start + doubleLength, 0, totalSize);
            var leftIndex = start;
            var rightIndex = endLeft;
            var index = start;
            for (; leftIndex < endLeft || rightIndex < endRight; index += 1)
            {
                if (leftIndex >= endLeft)
                {
                    result[t][index] = result[1 - t][rightIndex];
                    rightIndex += 1;
                }
                else if (rightIndex >= endRight)
                {
                    result[t][index] = result[1 - t][leftIndex];
                    leftIndex += 1;
                }
                else if (compareFunction(result[1 - t][leftIndex], result[1 - t][rightIndex]) <= 0)
                {
                    result[t][index] = result[1 - t][leftIndex];
                    leftIndex += 1;
                }
                else
                {
                    result[t][index] = result[1 - t][rightIndex];
                    rightIndex += 1;
                }
            }
        }
        length += length;
        doubleLength += doubleLength;
    }
    return result[t];
}

/*
 Filter

 filterFunction(a) takes one entity.
 It returns true to keep the entity; otherwise false.
 */
export function filter(entities is array, filterFunction is function)
{
    var result = [];
    const totalSize is number = size(entities);
    for (var i = 0; i < totalSize; i += 1)
    {
        if (filterFunction(entities[i]))
        {
            result = append(result, entities[i]);
        }
    }
    return result;
}
