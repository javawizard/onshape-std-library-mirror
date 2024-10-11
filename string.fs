FeatureScript 2491; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/** Pretty printing and toString methods */

/**
 * Return a string representation of any value.
 *
 * Overloads of this method are found for many standard and custom types in
 * this and other modules. When overloaded, the `toString` method will be
 * called inside `print` and `println`, allowing users to change how custom
 * types are printed.
 */
export function toString(value) returns string
{
    return "" ~ value;
}

export function toString(value is string) returns string
{
    return value as string; // Strip off type tag
}

export function toString(value is array) returns string
{
    var result = "[";
    var first = true;
    for (var element in value)
    {
        if (first)
            first = false;
        else
            result ~= ",";
        result ~= " " ~ toString(element) ~ " ";
    }
    return result ~ "]";
}

export function toString(value is map) returns string
{
    var result = "{";
    var first = true;
    for (var element in value)
    {
        if (first)
            first = false;
        else
            result ~= ",";
        result ~= " " ~ toString(element.key) ~ " : " ~ toString(element.value) ~ " ";
    }
    return result ~ "}";
}

/**
 * Print a message to the FeatureScript notices pane.
 *
 * This has no effect on model state or rendering.
 */
export function print(value)
{
    @print(toString(value));
}

/**
 * Print a message to the FeatureScript notices pane, followed by a newline.
 *
 * This has no effect on model state or rendering.
 */
export function println(value)
{
    @print(toString(value) ~ '\n');
}

/**
 * Print a newline to the FeatureScript notices pane.
 */
export function println()
{
    @print('\n');
}

/**
 * Split a string into an array of characters, each represented as a string.
 */
export function splitIntoCharacters(s is string) returns array
{
    return @splitIntoCharacters(s);
}

/**
 * Parse a JSON string into either a map or array. Null values in the JSON
 * are returned as `undefined`. Throws if the string is not well-formed JSON.
 *
 * @return: A map or an array corresponding to the JSON value.
 */
export function parseJson(s is string)
{
    return @parseJson(s, {});
}

const REGEX_EXP = "(?:[eE][+-]?\\d+)?";
const REGEX_FULL_FORM = "(?:-?\\d+\\.\\d*)";
const REGEX_LEADING_DECIMAL = "(?:-?\\.?\\d+)";

/**
 * Matches a number in the string, with or without decimals or exponents.
 */
export const REGEX_NUMBER = "(?:(?:" ~ REGEX_FULL_FORM ~ "|" ~ REGEX_LEADING_DECIMAL ~ ")" ~ REGEX_EXP ~ "|-?inf)";

/**
 * Matches a number in the string, with or without decimals or exponents and captures it.
 */
export const REGEX_NUMBER_CAPTURE = "((?:" ~ REGEX_FULL_FORM ~ "|" ~ REGEX_LEADING_DECIMAL ~ ")" ~ REGEX_EXP ~ "|-?inf)";

/**
 * Extends regular expression syntax by adding \\f to indicate a complete number
 */
export function addCustomNumberMatching(regExp is string) returns string
{
    regExp = replace(regExp, "\\(\\\\f\\)", REGEX_NUMBER_CAPTURE);
    regExp = replace(regExp, "\\\\f", REGEX_NUMBER);
    return regExp;
}

/**
 * Test if `s` matches `regExp` in its entirety.
 *
 * @returns {{
 *      @field hasMatch {boolean} : `true` if `regExp` matches `s`
 *      @field captures {array} : The first element is always the input string `s`.
 *              The following elements are a list of all captured groups.
 * }}
 */
export function match(s is string, regExp is string) returns map
{
    return @match(s, regExp);
}

/**
 * Returns a copy of `s` with every match of `regExp` replaced with the string `replacement`.
 *
 * @example `replace("a~~aa~a", "a.", "X")` returns `X~X~a`
 */
export function replace(s is string, regExp is string, replacement is string) returns string
{
    return @replace(s, regExp, replacement);
}

/**
 * Convert a number in string form, into a FS number.
 * Note that this function will not accept trailing non numeric text,
 * the entire string must be a single valid number.
 *
 * @example `stringToNumber("1")` returns the number `1`
 * @example `stringToNumber("1.0")` returns the number `1`
 * @example `stringToNumber("1e2")` returns the number `100`
 */
export function stringToNumber(s is string) returns number
{
    return @stringToNumber(s);
}

/**
 * Return the number of characters in a string.
 */
export function length(s is string) returns number
{
    return @length(s);
}

/**
 * Return a substring of a string starting at the specified start index.
 * @example `substring("refactoring", 7)` returns `"ring"`
 */
export function substring(s is string, startIndex is number) returns string
{
    return @substring(s, startIndex);
}

/**
 * Return a substring of a string starting at the specified start index and ending at the specified end index.
 * @example `substring("refactoring", 2, 6)` returns `"fact"`
 */
export function substring(s is string, startIndex is number, endIndex is number) returns string
{
    return @substring(s, startIndex, endIndex);
}

/**
 * Return whether a string starts with the specified prefix.
 */
export function startsWith(s is string, prefix is string) returns boolean
{
    return @startsWith(s, prefix);
}

/**
 * Return whether a string ends with the specified suffix.
 */
export function endsWith(s is string, suffix is string) returns boolean
{
    return @endsWith(s, suffix);
}

/**
 * Split a string into parts based on a regular expression separator.
 *
 * @example `splitByRegexp("this, truly, is a test.", "[,. ]+")` returns `[ "this" , "truly" , "is" , "a" , "test" ]`
 * @example `splitByRegexp("fooooobazzoo", "oo")` returns `[ "f" , "" , "obazz" ]`
 * @example `splitByRegex("foo", "")` returns `[ "" , "" , "" , "" ]`
 */
export function splitByRegexp(s is string, separatorRegexp is string) returns array
{
    return @splitByRegexp(s, separatorRegexp);
}

/**
 * Return the index of a substring in a string, or -1 if the substring is not found.
 */
export function indexOf(s is string, substr is string) returns number
{
    return @indexOfString(s, substr);
}

/**
 * Return the index of a substring in a string starting the search at a specified start index, or -1 if the substring is not found.
 */
export function indexOf(s is string, substr is string, startIndex is number) returns number
{
    return @indexOfString(s, substr, startIndex);
}

/**
 * Return the first index of a regular expression match in a string, or -1 if not found.
 */
export function indexOfRegexp(s is string, regexp is string) returns number
{
    return @indexOfRegexp(s, regexp);
}

/**
 * Return the first index of a regular expression match in a string starting at the specified start index, or -1 if not found.
 */
export function indexOfRegexp(s is string, regexp is string, startIndex is number) returns number
{
    return @indexOfRegexp(s, regexp, startIndex);
}

/**
 * Return a string made by repeating the first argument a specified number of times.
 * For example: repeatString("foo", 5) returns "foofoofoofoofoo".
 */
export function repeatString(s is string, count is number) returns string
{
    return @repeatString(s, count);
}

/**
 *  Is undefined or empty string.
 */
export predicate isUndefinedOrEmptyString(val)
{
    val == undefined || val == "";
}


