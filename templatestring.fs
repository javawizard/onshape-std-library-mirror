FeatureScript 2543; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

export import(path : "onshape/std/units.fs", version : "2543.0");

/** Represents a [ValueWithUnits] which, when put in a [TemplateString] will render with a specified precision override. */
export type ValueWithUnitsAndPrecision typecheck canBeValueWithUnitsAndPrecision;

/** Typecheck for [ValueWithUnitsAndPrecision] */
export predicate canBeValueWithUnitsAndPrecision(value)
{
    value.value is ValueWithUnits;
    value.precision is number;
}

/** Constructs a [ValueWithUnitsAndPrecision] given the value and precision. */
export function valueWithUnitsAndPrecision(value is ValueWithUnits, precision is number) returns ValueWithUnitsAndPrecision
{
    return { "value" : value, "precision" : precision } as ValueWithUnitsAndPrecision;
}

/**
 * A `TemplateString` represents a value that will be formatted by template substitution.
 * It is useful when, for instance, a table cell needs to display some text in combination with a length
 * formatted in the document length units.
 *
 * The `TemplateString` is a map with a string field `template`.  Other fields represent parameters
 * to substitute and may be strings, numbers or [ValueWithUnits].
 *
 * Formatting happens as follows: Text in `template` that does not
 * contain the number sign `#` is unchanged.  `#identifier` (where `identifier` is a valid FeatureScript identifier)
 * causes a substitution with the result of looking up `identifier` in the map.  `##` is changed to `#`.
 * `# ` (The number sign followed by a space) is removed, which can be useful for separating a substitution from text.
 *
 * @example `{ 'template' : 'Length = #len', 'len' : foot }` gets formatted as `Length = 12 in` if document units are inches.
 * @example `{ 'template' : '###var# bar', 'var' : 'foo' }` get formatted as `#foobar`.
 */
export type TemplateString typecheck canBeTemplateString;

/** Typecheck for [TemplateString]. */
export predicate canBeTemplateString(value)
{
    value is map;
    value.template is string;
    for (var entry in value)
    {
        entry.key is string;
        entry.value is string || entry.value is number || entry.value is ValueWithUnits || entry.value is ValueWithUnitsAndPrecision;
    }
    // Other entries are referenced by the template
}

/** Constructor for [TemplateString].
 *
 * @param value: A map with a "template" field and any number of other fields, which may be
 *      referenced in the template string as e.g. `#myValue`. Used in FeatureScript tables.
 *      See [TemplateString] docs for more info.
 *      @eg `{ "template" : "Value of #myValue", "myValue" : 42 }`
 */
export function templateString(value is map) returns TemplateString
{
    return value as TemplateString;
}

