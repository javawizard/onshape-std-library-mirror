FeatureScript 1403; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/**
 * This module contains functions for working with FeatureScript arrays (e.g. `[1, 2, 3]`) and maps (e.g. `{ "x" : 1, "y" : true }`)
 */
import(path : "onshape/std/math.fs", version : "1403.0");

/**
 * Create a new array with given `size`, filled with `fillValue`.
 * @example `makeArray(3, 0)` returns `[0, 0, 0]`
 */
export function makeArray(size is number, fillValue) returns array
precondition isNonNegativeInteger(size);
{
    return @resize([], size, fillValue);
}

/**
 * Create a new array with given `size`, filled with `undefined`.
 * @example `makeArray(3)` returns `[undefined, undefined, undefined]`
 */
export function makeArray(size is number) returns array
precondition isNonNegativeInteger(size);
{
    return @resize([], size);
}

/**
 * Returns the size of an array.  This counts only direct children; it does not
 * recursively examine containers inside.
 * @example `size([1, 2, 3])` returns `3`
 * @example `size([1, [2, 3]])` returns `2`
 */
export function size(container is array) returns number
{
    return @size(container);
}

/**
 * Returns the size of an map. This counts only direct children; it does not
 * recursively examine containers inside.
 * @example `size({ "x" : 1, "y" : 2 })` returns `2`
 */
export function size(container is map) returns number
{
    return @size(container);
}

/**
 * Returns `true` if `value` appears in an array, using `==` for comparison.
 * @example `isIn(1 * inch, [0 * inch, 1 * inch, 2 * inch])` returns `true`
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
 * Returns `true` if `value` appears as the value of a map entry, using `==`
 * for comparison.
 *
 * @example `isValueIn(true, { "a" : true, "b" : 0 })` returns `true`
 * @example `isValueIn("b",  { "a" : true, "b" : 0 })` returns `false`
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
 * Returns a new array, with the same size as `arr`, created by mapping each
 * element of `arr` through a `mapFunction`.
 *
 * @example `mapArray([0, 1], function(x) { return -x; })` returns `[0, -1]`
 *
 * @param mapFunction : A function which takes in one argument (a member of the
 *          input array) and returns a value.
 */
export function mapArray(arr is array, mapFunction is function) returns array
{
    var result = @resize(arr, 0); // keep type tag
    for (var element in arr)
        result = @resize(result, @size(result) + 1, mapFunction(element)); // inlined append
    return result;
}

/**
 * Returns a copy of an array with size changed to `newSize`. If the new size
 * is larger than the original size, the extra values are set to `newValue`.
 *
 * @example `resize([1, 2, 3], 2, 0)` returns `[1, 2]`
 * @example `resize([1, 2, 3], 5, 0)` returns `[1, 2, 3, 0, 0]`
 */
export function resize(arr is array, newSize is number, newValue) returns array
precondition isNonNegativeInteger(newSize);
{
    return @resize(arr, newSize, newValue);
}

/**
 * Returns a copy of an array with size changed to `newSize`. If the new size
 * is larger than the original size, the extra values are set to `undefined`.
 */
export function resize(arr is array, newSize is number) returns array
precondition isNonNegativeInteger(newSize);
{
    return @resize(arr, newSize);
}

/**
 * Returns a copy of an array with a single value added to the end.
 *
 * @example `append([1, 2], 3)` returns `[1, 2, 3]`
 */
export function append(arr is array, newValue) returns array
{
    return @resize(arr, @size(arr) + 1, newValue);
}

/**
 * Given an array of arrays, concatenate the contents of the inner arrays.
 *
 * @example `concatenateArrays([[1, 2], [3, 4]])` returns `[1, 2, 3, 4]`
 * @example `concatenateArrays([[1], [], [2, undefined], [[3]]])` returns
 *      `[1, 2, undefined, [3]]`
 */
export function concatenateArrays(arr is array) returns array
{
    return @concatenateArrays(arr);
}

/**
 * Add each key-value pair in the second map to a copy of first and return the
 * result. Since later-added entries take precedence, nothing from the second
 * map will be lost.
 *
 * In other words, any keys from `defaults` which are missing from `m` will be
 * filled in with their values from `defaults`.
 *
 * @example `mergeMaps({a:0}, {a:1})` returns `{a:1}`
 * @example `mergeMaps({a:0}, {b:1})` returns `{a:0, b:1}`
 */
export function mergeMaps(defaults is map, m is map) returns map
{
    return @mergeMaps(defaults, m);
}

/**
 * Return a copy of an array with elements in reverse order.
 *
 * @example `reverse([1, 2, 3])` returns `[3, 2, 1]`
 */
export function reverse(arr is array) returns array
{
    return @reverse(arr);
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
 * Return a sorted copy of an array. Current implementation uses merge sort.
 *
 * @example `sort([3, 1, 2], function(a, b) { return a - b; })` returns `[1, 2, 3]`
 *
 * @param compareFunction : A function that takes two values, returns a
 *          negative value if the first is before the second, `0` if the two
 *          are equal, and positive value if the second is before the first.
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
 * Return the members of an array matching a predicate function, preserving
 * element order.
 *
 * Throws exception if `filterFunction` throws, or if the `filterFunction` does
 * not return `boolean`.
 *
 * @example `filter([1, 2, 3, 4, 5, 6], function(x) { return x % 2 == 0; })`
 *          returns `[2, 4, 6]`
 *
 * @param filterFunction : A function which takes one argument (a member
 *          of the input array) and returns a `boolean`.
 */
export function filter(arr is array, filterFunction is function)
{
    var result = @resize(arr, 0); // keep type tag
    for (var element in arr)
    {
        if (filterFunction(element))
            result = @resize(result, @size(result) + 1, element); // inlined append
    }
    return result;
}

/**
 * Returns the keys in the supplied map in map iteration order.
 *
 *
 * @example `keys({ "a" : 1, "c" : 2, "b" : 3 })`
 *          returns `["a", "b", "c"]`
 */
export function keys(container is map) returns array
{
    var result = [];
    for (var entry in container)
        result = @resize(result, @size(result) + 1, entry.key); // inlined append
    return result;
}

/**
 * Returns the values in the supplied map ordered by the map iteration ordering of their associated keys.
 *
 * @example `keys({ "a" : 1, "c" : 2, "b" : 3 })`
 *          returns `[1, 3, 2]`
 */
export function values(container is map) returns array
{
    var result = [];
    for (var entry in container)
        result = @resize(result, @size(result) + 1, entry.value); // inlined append
    return result;
}

/**
 * Returns the sub array `[startIndex, endIndex)`
 */
export function subArray(input is array, startIndex is number, endIndex is number) returns array
{
    var result = @resize(input, 0); // keep type tag
    for (var i = startIndex; i < endIndex; i += 1)
        result = @resize(result, @size(result) + 1, input[i]); // inlined append
    return result;
}

