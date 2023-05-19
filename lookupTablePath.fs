FeatureScript 2045; /* Automatically generated version */
import(path : "onshape/std/math.fs", version : "2045.0");
import(path : "onshape/std/string.fs", version : "2045.0");
import(path : "onshape/std/units.fs", version : "2045.0");

/**
 * A `LookupTablePath` identifies a map of keys into a multi-level table.
 *
 * The fields on a LookupTablePath depend on its related LookupTable.
 *
 * @type {{
 *      @field entityType {EntityType} : @optional
 * }}
 */
export type LookupTablePath typecheck canBeLookupTablePath;

/** Typecheck for [LookupTablePath] */
export predicate canBeLookupTablePath(value)
{
    value is map;
    for (var entry in value)
    {
        entry.key is string;
        entry.value is string;
    }
}

/**
 * Creates a `lookupTablePath`.
 */
export function lookupTablePath(source is map) returns LookupTablePath
{
    return source as LookupTablePath;
}

/**
 * Convert a table expression in string form into a ValueWithUnits.
 *
 * @param expression : Currently the following forms are supported:
 *          @ex `{number} <*> {units}`
 *          @ex `{number}/{number} <*> {units}`
 *          @ex `{number} <+> {number}/{number} <*> {units}`
 *              Where `{number} is <-><nnn><.><nnn><e<+/->nnn>` or <->inf
 *          @ex `{} indicate an entity, <> indicate the contents are optional`
 *              spaces between entities are optional unless they are required to separate entities.
 */
export function lookupTableEvaluate(expression is string) returns ValueWithUnits
{
    var matchResult;
    const optMultiply = "(?:\\s+|\\s*\\*\\s*)";
    const suffix = optMultiply ~ REGEX_UNITS;
    var regex;

    // n units
    regex = addCustomNumberMatching("(\\f)" ~ suffix);
    matchResult = match(expression, regex);
    if (matchResult.hasMatch)
    {
        return stringToNumber(matchResult.captures[1]) * stringToUnit(matchResult.captures[2]);
    }

    // n/n units
    regex = addCustomNumberMatching("(\\f)\\s*/\\s*(\\f)" ~ suffix);
    matchResult = match(expression, regex);
    if (matchResult.hasMatch)
    {
        return stringToNumber(matchResult.captures[1]) / stringToNumber(matchResult.captures[2]) * stringToUnit(matchResult.captures[3]);
    }

    // n <+> n/n units
    regex = addCustomNumberMatching("(\\f)(?: |\\s*\\+\\s*)(\\f)\\s*/\\s*(\\f)" ~ suffix);
    matchResult = match(expression, regex);
    if (matchResult.hasMatch)
    {
        return (stringToNumber(matchResult.captures[1]) + stringToNumber(matchResult.captures[2]) / stringToNumber(matchResult.captures[3])) * stringToUnit(matchResult.captures[4]);
    }

    // (n <+> n/n) * units
    regex = addCustomNumberMatching("\\(\\s*(\\f)(?: |\\s*\\+\\s*)(\\f)\\s*/\\s*(\\f)\\s*\\)" ~ suffix);
    matchResult = match(expression, regex);
    if (matchResult.hasMatch)
    {
        return (stringToNumber(matchResult.captures[1]) + stringToNumber(matchResult.captures[2]) / stringToNumber(matchResult.captures[3])) * stringToUnit(matchResult.captures[4]);
    }
    throw "Unexpected expression: " ~ expression;
}

/**
 * insert plus sign and parenthesis as needed to make a valid expression
 */
export function lookupTableFixExpression(expression is string) returns string
{
   const optMultiply = "(?:\\s+|\\s*\\*\\s*)";
   const suffix = optMultiply ~ REGEX_UNITS;
   var regex = addCustomNumberMatching("(\\f)(?: |\\s*\\+\\s*)(\\f)\\s*/\\s*(\\f)" ~ suffix);
   var matchResult = match(expression, regex);
   if (matchResult.hasMatch)
   {
       return "(" ~ matchResult.captures[1] ~ "+" ~ matchResult.captures[2] ~ "/" ~ matchResult.captures[3] ~ ") * " ~ matchResult.captures[4];
   }
   return expression;
}


/**
 * Find a terminal/content table from a path and convert into expression/value form
 */
export function getLookupTable(table is map, path is LookupTablePath)
{
    while (table != undefined && table.entries != undefined && table.name != undefined)
    {
        var pathKey = path[table.name];
        var nextEntry = table.entries[pathKey];
        if (nextEntry == undefined)
        {
            return {};
        }
        if (!(nextEntry is map))
        {
            return nextEntry;
        }
        table = nextEntry;
    }
    return table;
}

/**
 * Extract the value portion of expression/value maps or evaluate expressions.
 * value maybe an expression or a value with units
 */
export function lookupTableGetValue(value) returns ValueWithUnits
precondition
{
    (value is string) || (value is ValueWithUnits);
}
{
    if (value is string)
    {
        return lookupTableEvaluate(value);
    }
    else
    {
        return value;
    }
}

/**
 * Test if the current definition violates the table.
 */
export function isLookupTableViolated(definition is map, table is map) returns boolean
{
  return isLookupTableViolated(definition, table, {});
}

/**
 * @internal
 * Determines if the specified table entry map has any value violations based upon the specified master definition map
 *
 * @param definition : the master definition map to check against
 * @param table : the table map to check for any violations against the definition map
 * @param ignoreProperties : map of property names to ignore, if value is true, will not check for any violation against that property's value
 * @returns : true if there is a violation, false if no violations
 */
export function isLookupTableViolated(definition is map, table is map, ignoreProperties is map) returns boolean
{
    for (var entry in table)
    {
        if (ignoreProperties[entry.key] == true)
        {
            continue;
        }
        var v1 = lookupTableGetValue(definition[entry.key]);
        var v2 = lookupTableGetValue(entry.value);
        var diff = abs(v1 - v2);
        if (isLength(diff))
        {
            if (diff > TOLERANCE.zeroLength * meter)
            {
                return true;
            }
        }
        else if (isAngle(diff))
        {
            if (diff > TOLERANCE.zeroAngle * radian)
            {
                return true;
            }
        }
        else
        {
            // Someday if more unit comparisons are defined use them here
            if (diff.value > TOLERANCE.zeroLength)
            {
                return true;
            }
        }
    }
    return false;
}

