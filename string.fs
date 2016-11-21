FeatureScript 455; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

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
 * Split a string into an array of characters, each represented as a string.
 */
export function splitIntoCharacters(s is string) returns array
{
    return @splitIntoCharacters(s);
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
    return @size(@splitIntoCharacters(s));
}


