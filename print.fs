FeatureScript 172; /* Automatically generated version */
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

export function print(value)
{
    @print(toString(value));
}

export function println(value)
{
    @print(toString(value) ~ '\n');
}

