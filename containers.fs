FeatureScript 2559; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/**
 * This module contains functions for working with FeatureScript arrays (e.g. `[1, 2, 3]`) and maps (e.g. `{ "x" : 1, "y" : true }`)
 */
import(path : "onshape/std/math.fs", version : "2559.0");
import(path : "onshape/std/string.fs", version : "2559.0");

/**
 * Create a new array with given `size`, filled with `fillValue`.
 * Note: this is equivalent to assigning each individual array
 * element to `fillValue`; boxes and builtins will not be deep-copied.
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
    return @indexOf(container, value) != -1;
}

/**
 * Return the index of the `value` in `container`, or -1 if the value is not found.
 */
export function indexOf(container is array, value) returns number
{
    return @indexOf(container, value);
}

/**
 * Return the index of the `value` in `container` starting the search at a specified start index, or -1 if the value is not found.
 */
export function indexOf(container is array, value, startIndex is number) returns number
{
    return @indexOf(container, value, startIndex);
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
    return @indexOf(@values(container), value) != -1;
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
 * Compute the intersection of the keysets of the input maps.  In other words,
 * returns a map whose keys are present in all input maps and whose values are
 * taken from the last map.
 *
 * @example `intersectMaps([{a:0}, {a:1}])` returns `{a:1}`
 * @example `intersectMaps([{a:0}, {b:1}])` returns `{}`
 * @example `intersectMaps([{a:0, b:1}, {a:0, b:2}])` returns `{a:0, b:2}`
 */
export function intersectMaps(maps is array) returns map
{
    return @intersectMaps(maps);
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
 * Returns a sorted copy of `values`, where any sequence of values within `tolerance` of
 * each other is sorted in the order of the original array.
 *
 * This is useful when sorting by a geometric measurement (like length, area, or volume)
 * because it makes it much less likely that a tiny change in that computed value will
 * change the resulting sort order.
 *
 * @example `tolerantSort([5, 1.000001, 1, 8], 0.001)` returns `[1.000001, 1, 5, 8]`
 * @example `tolerantSort( [1 * inch, 1.00009 * inch, 0.99991 * inch], 0.0001 * inch)`
 *      returns `[1 * inch, 1.00009 * inch, 0.99991 * inch]`. The order is entirely
 *      unchanged since two pairs of values are within the tolerance (even though
 *      the third pair isn't).
 *
 * @param values : An array of `number` or `ValueWithUnits`.
 * @param tolerance : Tolerance for comparing elements of `values`.
 */
export function tolerantSort(values is array, tolerance) returns array
{
    return tolerantSort(values, tolerance, undefined);
}

/**
 * Performs a [tolerantSort](tolerantSort(array, ?)) of `entities`, ordering by the value
 * returned by `mapFunction`. Like `tolerantSort`, the original order will be preserved
 * for values within `tolerance` for stability.
 *
 * @param tolerance : @eg `1e-7 * meter`
 * @param mapFunction : A function taking in a single entity and returning a sortable
        `number` or `ValueWithUnits`.
 *      @eg `function(entity is Query) { return evLength(context, {"entities" : entity}); }` to sort entities by increasing length.
 */
export function tolerantSort(entities is array, tolerance, mapFunction) returns array
precondition
{
    tolerance > 0;
    mapFunction is function || mapFunction is undefined;
}
{
    if (entities == [])
    {
        return entities;
    }
    var values = entities;
    if (mapFunction != undefined)
    {
        values = mapArray(values, mapFunction);
    }
    if (!((values[0] / tolerance) is number))
        throw "Tolerance " ~ toString(tolerance) ~ " must have the same units as array value " ~ toString(values[0]);
    const indices = @tolerantSort(values, tolerance);
    var result = [];
    for (var index in indices)
        result = append(result, entities[index]);
    return result;
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
 * Return the first item in a map
 */
export function first(m is map)
{
    for (var result in m)
        return result.value;
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
    return @keys(container);
}

/**
 * Returns the values in the supplied map ordered by the map iteration ordering of their associated keys.
 *
 * @example `values({ "a" : 1, "c" : 2, "b" : 3 })`
 *          returns `[1, 3, 2]`
 */
export function values(container is map) returns array
{
    return @values(container);
}

/**
 * Returns the subarray beginning at `startIndex`
 */
export function subArray(input is array, startIndex is number) returns array
{
    return @subArray(input, startIndex);
}

/**
 * Returns the subarray `[startIndex, endIndex)`
 */
export function subArray(input is array, startIndex is number, endIndex is number) returns array
{
    return @subArray(input, startIndex, endIndex);
}

/**
 * Inserts `value` into the array keyed by `key`, returns the updated map
 */
export function insertIntoMapOfArrays(mapToInsertInto is map, key, value) returns map
{
    if (mapToInsertInto[key] == undefined)
    {
        mapToInsertInto[key] = [value];
    }
    else
    {
        mapToInsertInto[key] = append(mapToInsertInto[key], value);
    }
    return mapToInsertInto;
}

/**
 * Returns last element of array.
 */
export function last(elements is array)
{
    return elements[@size(elements) - 1];
}

/**
 * Returns a rotated array of the same elements.
 * `step` less than zero moves elements towards the front.
 * `step` greater than zero moves elements towards the back.
 * @example `rotateArray([0, 1, 2], -1)`
 *          returns `[1, 2, 0]`
 */
export function rotateArray(elements is array, step is number) returns array
{
    const length = @size(elements);
    if (length == 0)
    {
        return elements;
    }
    step = step % length;
    if (step == 0)
    {
        return elements;
    }

    const head = @subArray(elements, length - step, length);
    const tail = @resize(elements, length - step);
    const rotatedArray = @concatenateArrays([head, tail]);
    return rotatedArray;
}

/**
 * Returns an array with `value` inserted at `index`.
 */
export function insertElementAt(arr is array, index is number, value) returns array
{
    return @concatenateArrays([@subArray(arr, 0, index), [value], @subArray(arr, index)]);
}

/**
 * Returns an array with the element at `index` removed.
 */
export function removeElementAt(arr is array, index is number) returns array
{
    return @concatenateArrays([@subArray(arr, 0, index), @subArray(arr, index + 1)]);
}

/**
 * Returns `true` if and only if all elements of an array, when passed into the `checkFunction`, return `true`.
 * @seeAlso [all(array)]
 * @ex `all([0, 2, 4], function(e){ return e % 2 == 0; })` returns `true`
 * @ex `all([], function(e){ return false; })` returns `true`
 *
 * @param arr {array} : An array of elements to be checked.
 * @param checkFunction {function} : A unary function that returns a boolean.
 * @returns {boolean} : `true` if and only if all `checkFunction(element)` calls return `true`.
 **/
export function all(arr is array, checkFunction is function) returns boolean
{
    for (var x in arr)
    {
        if (!checkFunction(x))
            return false;
    }
    return true;
}

/**
 * Returns true if all elements in the passed array are `true`.
 * @seeAlso [all(array, function)]
 * @ex `all([])` returns `true`
 * @ex `all([false, false, true])` returns `false`
 * @ex `all([true, true, true])` returns `true`
 *
 * @param arr {array} : An array of booleans.
 * @returns {boolean} : `true` if and only if all `element`s are `true`.
 **/
export function all(arr is array) returns boolean
{
    for (var x in arr)
    {
        if (!x)
            return false;
    }
    return true;
}

predicate isArrayOfArrays(arr is array)
{
    for (var a in arr)
    {
        a is array;
    }
}

/**
 * Creates all possible combinations from arrays of values for each element.
 * @ex `allCombinations([[1,2], [3,4]])` returns `[[1,3], [1,4], [2,3], [2,4]]`
 * @ex `allCombinations([[0, 1, 2, 3]])` returns `[[0], [1], [2], [3]]`
 * @ex `allCombinations([[], [0, 1, 2]])` returns `[]`
 *
 * @param arr {array} : An array of arrays, where each array represents all possible values for the given array's index in the returned arrays.
 * @returns {array} : An array of all possible combinations that have exactly one element from each of the input arrays.
 **/
export function allCombinations(arr is array) returns array
precondition
{
    isArrayOfArrays(arr);
}
{
    const n = size(arr);
    if (n == 0)
    {
        return [];
    }
    // Base case - [[0,1,2,3]]->[[0], [1], [2], [3]]
    if (n == 1)
    {
        var result = [];
        for (var x in arr[0])
        {
            result = append(result, [x]);
        }
        return result;
    }
    else
    {
        const otherCombinations = allCombinations(removeElementAt(arr, n - 1));
        return mapArray(otherCombinations, function(combination)
                {
                    return mapArray(arr[n - 1], function(el)
                        {
                            return append(combination, el);
                        });
                })->concatenateArrays();
    }
}

/**
 * Returns `true` if any element of an array, when passed into the `check` function, returns `true`.
 * @seeAlso [any(array)]
 * @ex `any([1, 3, 4], function(e){ return e % 2 == 0; })` returns `true`
 * @ex `any([], function(e){ return true; })` returns `false`
 *
 * @param arr {array} : An array of elements.
 * @returns {boolean} : `true` if and only if at least one `check(element)` call returns `true`.
 **/
export function any(arr is array, check is function) returns boolean
{
    for (var x in arr)
    {
        if (check(x))
            return true;
    }
    return false;
}

/**
 * Returns true if any element in the passed array is `true`.
 * @seeAlso [any(array, function)]
 * @ex `any([])` returns `false`
 * @ex `any([false, false, true])` returns `true`
 *
 * @param arr {array} : An array of booleans.
 * @returns {boolean} : `true` if and only if at least one `element` is `true`.
 **/
export function any(arr is array) returns boolean
{
    for (var x in arr)
    {
        if (x)
            return true;
    }
    return false;
}

/**
 * Returns the average of an array. All array elements must be mutually addable and divisible by a number.
 * @ex `average([1, 2, 3, 4])` returns `2.5`
 * @ex `average([vector([1, 0, 0])*meter, vector([0, 0, 0])*meter, vector([0, 1, 0])*meter])` returns `vector(1/3, 1/3, 0) * meter`
 *
 * @param arr {array} : An array of mutually addable and divisible elements.
 * @returns : The average of values in the passed in array.
 **/
export function average(arr is array)
{
    return sum(arr) / size(arr);
}

/**
 * Deduplicate an array. Maintains original array order, eliminating all but the first occurrence of a given duplicate.
 * @ex `deduplicate([1, 2, 1, 3, 2, 0, 0])` returns `[1, 2, 3, 0]`
 * @ex `deduplicate([])` returns `[]`
 *
 * @param arr {array} : An array of values to be deduplicated.
 * @returns {array} : An array of deduplicated values.
 **/
export function deduplicate(arr is array) returns array
{
    var result = [];
    var deDuplicatedSet = {};
    for (var potentialDuplicate in arr)
    {
        if (deDuplicatedSet[potentialDuplicate] == undefined)
        {
            deDuplicatedSet[potentialDuplicate] = true;
            result = append(result, potentialDuplicate);
        }
    }
    return result;
}

/**
 * Folds an array from left to right with a `foldFunction`.
 *
 * @ex `foldArray([1, 2, 3], 0, function(accumulator, element) { return accumulator + element; })` returns `6`
 *
 * @param arr {array} : An array to fold.
 * @param seed : The initial value of the accumulator to be passed into the `foldFunction`.
 * @param foldFunction : A binary function which takes in an accumulator (the seed for the first iteration, and the result of the
 *      previous call for subsequent iterations) and an element of the passed in array.
 * @returns : The accumulator after all elements of `arr` have been folded.
 */
export function foldArray(arr is array, seed, foldFunction is function)
{
    var response = seed;
    for (var element in arr)
    {
        response = foldFunction(response, element);
    }
    return response;
}

/**
 * Calls `foldArray` with the `seed` set to the first element of `arr`.
 * @seeAlso [foldArray]
 */
export function foldArray(arr is array, foldFunction is function)
{
    return arr == [] ? undefined : foldArray(arr->removeElementAt(0), arr[0], foldFunction);
}

/**
 * Returns a new array, with the same size as `arr`, created by mapping each
 * index of `arr` through a `mapIndexFunction`.
 * @ex `const myArray = [1, 3, 5]; mapArrayIndices(myArray, function(i) { return myArray[i] + myArray[ (i+1) % size(myArray)]; })` returns `[4, 8, 6]`
 *
 * @param mapIndexFunction : A function which takes in one argument (an index of the
 *          input array) and returns a value.
 * @returns {array} : The results of calling `mapIndexFunction` on the indices of all elements in the passed in `arr`.
 */
export function mapArrayIndices(arr is array, mapIndexFunction is function) returns array
{
    const n = size(arr);
    if (n < 1)
    {
        return [];
    }
    return range(0, size(arr) - 1)->mapArray(mapIndexFunction);
}

/**
 * Map a value using a mapFunction and return the result. Particularly useful when using a lambda function inline to dynamically change some value.
 * @ex `mapValue(4, function(n){ return n+1; })` returns `5`
 * @ex `couldBeUndefined->mapValue(function(v){ return v == undefined ? 'a great default' : v; })`
 *
 * @param value : Anything that the passed in mapFunction will accept as a parameter.
 * @param mapFunction : A function that will be called on the passed in `value`.
 * @returns : The result of calling `mapFunction` with `value`.
 **/
export function mapValue(value, mapFunction is function)
{
    return mapFunction(value);
}

/**
 * Memoize a unary (one-parameter) function. Once memoized, if the returned function is called with the same parameter twice,
 * the second return value will be fetched from an internal cache. This can dramatically speed up calculations - particularly
 * when `f` is called with the same parameter many times. The overhead of memoizing a function is negligible. Note that
 * memoization will not properly work with functions that have side effects, such as modifying a box.
 * @example ```
 * const square = memoizeFunction(function(n){ return n^2; });
 * println(square(5)); // calls f internally and prints 25
 * println(square(5)); // retrieves cached value of 25 and returns it
 * ```
 * @param f : A unary function to be memoized.
 * @returns : A memoized function that will return the same thing as f.
 **/
export function memoizeFunction(f is function) returns function
{
    var cache = new box({});
    return function(parameter) {
        if (cache[][parameter] == undefined)
            cache[][parameter] = f(parameter);

        return cache[][parameter];
    };
}

/**
 * Merge maps at a particular location as specified by the `keyList`. If either the destination node specified by the `keyList`
 *      or the `newNode` is not a map, the `newNode` will replace the destination node.
 * @seeAlso [mergeMaps(map, map)]
 * @ex `mergeMaps({ a: [ { b: 2 } ] }, ['a', 0, 'b'], 4)` returns `{ a : [ { b : 4 } ] }`
 * @ex `mergeMaps(5, [], 4)` returns `4`
 * @ex `mergeMaps({ a : 5 }, ['a'], 4)` returns `{a: 4 }`
 *
 * @param defaults : A container (array or map) that will be merged at the location specified by the `keyList`.
 *      If `defaults` is not an array or map and `keyList` is empty, the result is `newNode` since a merge isn't possible.
 * @param keyList {array} : An array of map keys or array indices that collectively specify a location within `defaults` to perform the merge.
 * @param newNode : A value that will be merged into defaults at the location specified by `keyList`. If the `newNode` specified is a map and
 *      the node identified by `keyList` is a map, then this will perform a `mergeMaps` operation.
 * @returns : The merged or replaced value.
 **/
export function mergeMaps(defaults, keyList is array, newNode)
{
    if (size(keyList) == 0)
    {
        if (defaults is map && newNode is map)
        {
            return mergeMaps(defaults, newNode);
        }
        else
        {
            return newNode;
        }
    }
    const firstKey = keyList[0];
    defaults[firstKey] = mergeMaps(defaults[firstKey], keyList->subArray(1, size(keyList)), newNode);
    return defaults;
}

/**
 * Sum an array of elements that are all mutually addable. An empty array returns 0.
 * @ex `sum(range(0,5))` returns `15`
 * @ex `sum([vector(1, 2, 3) * meter, vector(4, 5, 6) * meter])` returns `vector(5, 7, 9) * meter`
 * @ex `sum([])` returns `0`
 *
 * @param arr {array} : An array of mutually addable elements to be summed.
 * @returns : The sum of values in the passed in array.
 **/
export function sum(arr is array)
{
    if (arr == [])
    {
        return 0;
    }
    var result = arr[0];
    for (var x in arr->removeElementAt(0))
    {
        result += x;
    }
    return result;
}

/**
 * Makes an array that aggregates elements from each of the arrays. Returns an array of arrays, where the i-th array
 * contains the i-th element from each of the argument arrays. The array stops when the shortest input array is exhausted.
 * With a single array argument, it returns an array of single element arrays. With no arguments, it returns an empty array.
 * @seeAlso [zip(array, array)]
 * @ex `zip([range(0,3), range(10,13), range(20,26)])` returns `[[0, 10, 20], [1, 11, 21], [2, 12, 22], [3, 13, 23]]`
 * @ex `zip([])` returns `[]`
 * @ex `zip([range(0, 3)])` returns `[[0],[1],[2],[3]]`
 *
 * @param arr {array} : An array of arrays to aggregate.
 * @returns {array} : An array where the i'th element contains the i'th element from each of the passed in arrays.
 **/
export function zip(arr is array) returns array
precondition
{
    isArrayOfArrays(arr);
}
{
    const n = size(arr);
    if (n == 0)
        return [];
    var zipSize = size(arr[0]);
    for (var x in arr)
    {
        const n = size(x);
        if (n < zipSize)
        {
            zipSize = n;
        }
    }
    var result = [];
    for (var i = 0; i < zipSize; i += 1)
    {
        var zippedEntry = [];
        for (var arrEntry in arr)
        {
            zippedEntry = append(zippedEntry, arrEntry[i]);
        }
        result = append(result, zippedEntry);
    }
    return result;
}

/**
 * An alternative way to call [zip(array)] that facilitates chaining arguments.
 * @seeAlso [zip(array)]
 * @ex `zip(range(0,3), range(1, 4))` returns `[[0, 1], [1, 2], [2, 3], [3, 4]]`
 * @ex `zip(range(0,3), [])` returns `[]`
 *
 * @param a {array} : The first array to zip.
 * @param b {array} : The second array to zip.
 * @returns {array} : An array of length-2 arrays. For the i'th array, the first element is the i'th element of `a` and the second is the i'th element of `b`.
 **/
export function zip(a is array, b is array)
{
    return zip([a, b]);
}

