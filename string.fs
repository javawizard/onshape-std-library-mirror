FeatureScript 328; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/** Pretty printing and toString methods */

/**
 * Return a string representation of any value.
 */
export function toString(value) returns string
{
    return "" ~ value;
}

export function toString(value is string) returns string
{
    return value;
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
 * Print a message to the FeatureScript message area.
 * This has no effect on model state or rendering.
 */
export function print(value)
{
    @print(toString(value));
}

/**
 * Print a message to the FeatureScript message area.
 * This has no effect on model state or rendering.
 */
export function println(value)
{
    @print(toString(value) ~ '\n');
}

/**
 * Split a string into characters, each represented as a string.
 */
export function splitIntoCharacters(s is string) returns array
{
    return @splitIntoCharacters(s);
}

const REGEX_EXP = "(?:[eE][+-]?\\d+)?";
const REGEX_FULL_FORM = "(?:-?\\d+\\.\\d*)";
const REGEX_LEADING_DECIMAL = "(?:-?\\.?\\d+)";

/**
 * Matches a number in the string, with our without decimals or exponents.
 */
export const REGEX_NUMBER = "(?:(?:" ~ REGEX_FULL_FORM ~ "|" ~ REGEX_LEADING_DECIMAL ~ ")" ~ REGEX_EXP ~ "|-?inf)";

/**
 * Matches a number in the string, with our without decimals or exponents and captures it.
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
 * Test if s matches regExp in it's entirety.
 * Returns a map with the test result in `result.hasMatch` and any captures
 * in `result.captures`.
 * `result.captures[0]` is always equal to the input string `s`
 */
export function match(s is string, regExp is string) returns map
{
    return @match(s, regExp);
}

/**
 * TODO: description
 * @param s
 * @param regExp
 * @param replacement
 */
export function replace(s is string, regExp is string, replacement is string) returns string
{
    return @replace(s, regExp, replacement);
}

/**
 * Convert a number in string form, into a FS number.
 * Note that this function will not accept trailing non numeric text,
 * the entire string must be a single valid number.
 * @param s
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


