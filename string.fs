FeatureScript 225; /* Automatically generated version */
//Pretty printing and toString methods

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
 * TODO: description
 * @param value
 */
export function print(value)
{
    @print(toString(value));
}

/**
 * TODO: description
 * @param value
 */
export function println(value)
{
    @print(toString(value) ~ '\n');
}

/**
 * TODO: description
 * @param s
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
 * TODO: description
 * @param s
 */
export function length(s is string) returns number
{
    return @size(@splitIntoCharacters(s));
}

