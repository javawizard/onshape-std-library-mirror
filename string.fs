FeatureScript 236; /* Automatically generated version */
//Pretty printing and toString methods

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
 * Return the number of characters in a string.
 */
export function length(s is string) returns number
{
    return @size(@splitIntoCharacters(s));
}

