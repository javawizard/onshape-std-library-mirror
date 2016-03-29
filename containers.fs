FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/math.fs", version : "");

/**
 * Return true if an array contains the given value, using `==` for comparison.
 */
export function isIn(value, container is array) returns boolean
{
    for (var element in container)
    {
        if (element == value)
            return true;
    }
    return false;
}

/**
 * Return true if a given value appears as the value of a map entry.
 * @example `isValueIn(true, { a : true, b : 0 })` returns true
 * @example `isValueIn('b', { a : true, b : 0 })` returns false
 */
export function isValueIn(value, container is map) returns boolean
{
    for (var entry in container)
    {
        if (entry.value == value)
            return true;
    }
    return false;
}

/**
 * Create a new array with a given size.  If no fill value is provided,
 * the array is filled with `undefined`.
 */
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

/**
 * Build a new array by applying a constructor function to each
 * element of the source array.
 * @example `mapArray([0, 1], function(x) { return -x; })` returns `[0, -1]`
 */
export function mapArray(a is array, constructor is function) returns array
{
    const size = @size(a);
    for (var i = 0; i < size; i = i + 1) {
        a[i] = constructor(a[i]);
    }
    return a;
}

/**
 * Return the size of a container.  This counts only direct children;
 * it does not recursively examine containers inside.
 * @example `size([1, [2]])` returns 2
 */
export function size(container is array) returns number
{
    return @size(container);
}

export function size(container is map) returns number
{
    return @size(container);
}

/**
 * Return a copy of an array with size changed to `newSize`.
 * If the new size is larger than the original size, the extra values
 * are set to `newValue`.  If `newValue` is omitted, `undefined` is used.
 */
export function resize(arr is array, newSize is number, newValue) returns array
precondition isNonNegativeInteger(newSize);
{
    return @resize(arr, newSize, newValue);
}

export function resize(arr is array, newSize is number) returns array
precondition isNonNegativeInteger(newSize);
{
    return @resize(arr, newSize);
}

/**
 * Add a new value onto the end of an array, increasing size by one.
 */
export function append(arr is array, newValue) returns array
{
    return @resize(arr, @size(arr) + 1, newValue);
}

/**
 * Given an array of arrays, concatenate the contents of the inner arrays.
 * @example `concatenateArrays([[], [1], [], [undefined, 2], [3], [])`
 */
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

/**
 * Given two maps, use the first map to provide default values for the second.
 * More formally, add each key-value pair in the second map to the first and
 * return the result.  Since later-added entries take precedence, nothing from
 * the second map will be lost.
 * @example `mergeMaps({}, x)` returns (a copy of) its second argument
 * @example `mergemaps({a:0}, {a:1})` returns `{a:1}`
 * @example `mergemaps({a:0}, {b:1})` returns `{a:0, b:1}`
 */
export function mergeMaps(defaults is map, m is map) returns map
{
    return @mergeMaps(defaults, m);
}

/**
 * Return a copy of an array with elements in reverse order.
 */
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

/**
 * @internal
 *
 * `mapLookup(a, ["b","c","d","e"])` is the same as `try (a.b.c.d.e)`
 * except it does not log a warning when used in the standard library.
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

/**
 * Return a sorted copy of an array.  (Currently implemented with merge sort.)
 * @param compareFunction : a function that takes two values and returns
 * negative if the first is before the second, 0 if the two are equal,
 * and positive if the second is before the first.
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
            const endLeft = clamp(start + length, 0, totalSize);
            const endRight = clamp(start + doubleLength, 0, totalSize);
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

/**
 * Return the members of an array matching a predicate function.
 * The result is an array containing only elements for which the
 * function returns true.  Order is preserved.
 *
 * Throws exception if an array element violates an `is` constraint
 * of the function or if the function does not return `boolean`.
 * @param entities
 * @param filterFunction : A function taking one argument (a member
 *                         of the input array) and returning `boolean`.
 */
export function filter(entities is array, filterFunction is function)
{
    var result = [];
    for (var entity in entities)
    {
        if (filterFunction(entity))
        {
            result = append(result, entity);
        }
    }
    return result;
}

