FeatureScript 1521; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

export import(path : "onshape/std/containers.fs", version : "1521.0");
export import(path : "onshape/std/context.fs", version : "1521.0");
export import(path : "onshape/std/evaluate.fs", version : "1521.0");
export import(path : "onshape/std/math.fs", version : "1521.0");
export import(path : "onshape/std/properties.fs", version : "1521.0");
export import(path : "onshape/std/query.fs", version : "1521.0");
export import(path : "onshape/std/string.fs", version : "1521.0");
export import(path : "onshape/std/valueBounds.fs", version : "1521.0");
export import(path : "onshape/std/tabletextalignment.gen.fs", version : "1521.0");

/**
 * This function takes a table generation function and wraps it to define a table.
 * It is analogous to [defineFeature], except for tables.  A typical usage is something like:
 * ```
 * annotation { "Table Type Name" : "MyTable" } // This annotation is required for Onshape to recognize myTable as a table.
 * export const myTable = defineTable(function(context is Context, definition is map) returns Table
 *     precondition
 *     {
 *         ... // Specify the parameters that this table takes, if any
 *     }
 *     {
 *         ... // Compute and return the table, using the context and the parameters
 *     });
 * ```
 *
 * For more information on writing tables, see `FeatureScript Tables` (TODO) in the
 * language guide.
 *
 * @param table : A function that takes a `context` and a `definition` and returns a [Table] or a [TableArray]
 *          @autocomplete
 * ```
 * function(context is Context, definition is map)
 *     precondition
 *     {
 *         // Specify the parameters that this table takes
 *     }
 *     {
 *         // Compute and return the table
 *     }
 * ```
 */
export function defineTable(table is function) returns function
{
    return function(context is Context, definition is map)
    {
        return table(context, definition);
    };
}

// ----------------------------------- Table -----------------------------------

/**
 * A `Table` represents a read-only table, consisting of rows, columns, and associated entities.
 * Custom tables return a `Table` or an array of `Table`s (tagged as a [TableArray]) as their output.
 *
 * The user-visible strings in a table, like column headings and cell values can be specified as either
 * a string, a number, a [ValueWithUnits] or a [TemplateString].  These are referred to as "table values"
 * and checked by the [isTableValue] predicate.
 *
 * @type {{
 *      @field title : A table value specifying the title of the table
 *      @field columnDefinitions {array} : An array of [TableColumnDefinition]s specifying the columns.
 *      @field rows {array} : An array of [TableRow]s specifying the data in the table.
 *      @field entities {Query} : An optional [Query] specifying the entities associated with the entire table. @optional
 * }}
 */
export type Table typecheck canBeTable;

/** Typecheck for [Table] */
export predicate canBeTable(value)
{
    value is map;
    isTableValue(value.title);
    value.columnDefinitions is array;
    for (var columnDefinition in value.columnDefinitions)
         columnDefinition is TableColumnDefinition;
    value.rows is array;
    for (var row in value.rows)
        row is TableRow;
    value.entities == undefined || value.entities is Query; // entities associated with the table, if any
}

/** Constructs a [Table] given a title, column definitions, and rows */
export function table(title, columnDefinitions is array, rows is array) returns Table
precondition isTableValue(title);
{
    return { "title" : title, "columnDefinitions" : columnDefinitions, "rows" : rows } as Table;
}

/**
 * @internal
 * Constructs a [Table] given a title, column definitions, rows, and entities
 */
export function table(title, columnDefinitions is array, rows is array, entities is Query) returns Table
precondition isTableValue(title);
{
    return { "title" : title, "columnDefinitions" : columnDefinitions, "rows" : rows, "entities" : entities } as Table;
}

// ----------------------------------- Table Column Definition -----------------------------------

/**
 * A `TableColumnDefinition` represents a column in a [Table].
 *
 * @type {{
 *      @field id {string} : The internal id of the column.  Referenced in [TableRow]s to specify cell values.
 *      @field name : A table value specifying the column name, which is displayed as the heading
 *      @field alignment {TableTextAlignment} : How text is aligned in the column.  Default is `LEFT`. @optional
 *      @field entities {Query} : An optional [Query] specifying the entities associated with the column. @optional
 * }}
 */
export type TableColumnDefinition typecheck canBeTableColumnDefinition;

/** Typecheck for [TableColumnDefinition] */
export predicate canBeTableColumnDefinition(value)
{
    value is map;
    value.id is string;
    isTableValue(value.name);
    value.alignment == undefined || value.alignment is TableTextAlignment;
    value.entities == undefined || value.entities is Query;
}

/**
 * Constructs a [TableColumnDefinition] given an id and a name.
 */
export function tableColumnDefinition(id is string, name) returns TableColumnDefinition
precondition isTableValue(name);
{
    return { "id" : id, "name" : name } as TableColumnDefinition;
}

/**
 * Constructs a [TableColumnDefinition] given an id, a name, and a [TableTextAlignment] controlling
 * how its cell content is aligned.
 */
export function tableColumnDefinition(id is string, name, alignment is TableTextAlignment) returns TableColumnDefinition
precondition isTableValue(name);
{
    return { "id" : id, "name" : name, "alignment" : alignment } as TableColumnDefinition;
}

/**
 * Constructs a [TableColumnDefinition] given an id, a name, and entities to cross-highlight when mousing
 * over the column.
 */
export function tableColumnDefinition(id is string, name, entities is Query) returns TableColumnDefinition
precondition isTableValue(name);
{
    return { "id" : id, "name" : name, "entities" : entities } as TableColumnDefinition;
}



// ----------------------------------- Table Row -----------------------------------

/**
 * A `TableRow` represents a row in a table, including the cells in that row.
 *
 * @type {{
 *      @field columnIdToCell {map} : The cell values.  Keys are column ids, as specified in the table
 *                                    column definitions.  Values are table values or [TableCellError]s.
 *      @field entities {Query} : An optional [Query] specifying the entities associated with the row. @optional
 * }}
 */
export type TableRow typecheck canBeTableRow;

/** Typecheck for [TableRow] */
export predicate canBeTableRow(value)
{
    value is map;
    value.columnIdToCell is map;
    for (var entry in value.columnIdToCell)
    {
        entry.key is string;
        isTableValue(entry.value) || entry.value is TableCellError;
    }
    value.entities == undefined || value.entities is Query; // entities associated with the row, if any
    value.callout == undefined || isTableValue(value.callout); // Not used for now
}

/** Constructs a [TableRow] given the cell values. */
export function tableRow(columnIdToCell is map) returns TableRow
{
    return { "columnIdToCell" : columnIdToCell } as TableRow;
}

/** Constructs a [TableRow] given the cell values and entities. */
export function tableRow(columnIdToCell is map, entities is Query) returns TableRow
{
    return { "columnIdToCell" : columnIdToCell, "entities" : entities } as TableRow;
}

// ----------------------------------- Table Values -----------------------------------

/** Returns `true` if the input is a table value, that is a string, a number, a [ValueWithUnits] or a [TemplateString]. */
export predicate isTableValue(value)
{
    value is string || value is number || value is ValueWithUnits || value is TemplateString;
}

// ----------------------------------- Table Array -----------------------------------

/** Represents an array of [Table]s.  One possible output of a table function, the other being [Table].  */
export type TableArray typecheck canBeTableArray;

/** Typecheck for [TableArray] */
export predicate canBeTableArray(value)
{
    value is array;
    for (var item in value)
        item is Table;
}

/** Constructs a [TableArray] given an array. */
export function tableArray(value is array) returns TableArray
{
    return value as TableArray;
}

// ----------------------------------- Table Cell Error -----------------------------------

/**
 * A `TableCellError` represents a table cell in an error state.  Such a cell has a displayed value
 * as well as an error message that appears as a tooltip over the cell.
 *
 * @type {{
 *      @field value : The displayed value, provided as a table value.
 *      @field error : The error message, provided as a table value.
 * }}
 */
export type TableCellError typecheck canBeTableCellError;

/** Typecheck for [TableCellError]. */
export predicate canBeTableCellError(value)
{
    value is map;
    isTableValue(value.value);
    isTableValue(value.error);
}

/** Constructs a [TableCellError] given a displayed value and an error message. */
export function tableCellError(value, error) returns TableCellError
precondition
{
    isTableValue(value);
    isTableValue(error);
}
{
    return { "value" : value, "error" : error } as TableCellError;
}

// ----------------------------------- Template String -----------------------------------

/**
 * A `TemplateString` represents a table value that will be formatted by template substitution.
 * It is useful when, for instance, a cell needs to display some text in combination with a length
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
        entry.value is string || entry.value is number || entry.value is ValueWithUnits;
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

// ----------------------------------- All parts query -----------------------------------

/**
 * Returns an array of maps for all modifiable non-mesh solids and closed composites.  Each map has a key `part`, which
 * maps to a query for the solid or composite and `bodies`, which maps to either the solid or all constituent bodies.
 * This is useful for iterating over what a user may consider to be "individual parts" in a context.
 *
 * @example ```
 * for (var partAndBodies in allSolidsAndClosedComposites(context))
 * {
 *     var name = getProperty(context, { "entity" : partAndBodies.part, "propertyType" : PropertyType.NAME } );
 *     var volume = evVolume(context, { "entities" : partAndBodies.bodies });
 * }
 * ```
 */
export function allSolidsAndClosedComposites(context is Context) returns array
{
    const allSolidsNotConsumed = qConsumed(qAllModifiableSolidBodies(), Consumed.NO);

    var result = [];

    for (var solid in evaluateQuery(context, allSolidsNotConsumed))
        result = append(result, { "part" : solid, "bodies" : solid });

    const allClosedComposites = qCompositePartTypeFilter(qEverything(EntityType.BODY), CompositePartType.CLOSED);

    for (var composite in evaluateQuery(context, allClosedComposites))
        result = append(result, { "part" : composite, "bodies" : qContainedInCompositeParts(composite) });

    return result;
}

// ----------------------------------- toString -----------------------------------

const MAX_SUBSTITUTIONS = 1000;

/**
 * Tries to convert a table to string form.  Because the actual table output depends on
 * user-chosen units and precision, the result of this function may not match it.
 */
export function toString(table is Table) returns string
{
    var columnIdToWidth = {};
    var columnIdToText = [];
    var columnIdToName = {};

    for (var column in table.columnDefinitions)
    {
        columnIdToName[column.id] = toString(column.name);
        columnIdToWidth[column.id] = length(columnIdToName[column.id]);
    }
    for (var row in table.rows)
    {
        var columnIdToTextForRow = {};
        for (var column in table.columnDefinitions)
        {
            var text = toString(row.columnIdToCell[column.id]);
            columnIdToTextForRow[column.id] = text;
            columnIdToWidth[column.id] = max(columnIdToWidth[column.id], length(text));
        }
        columnIdToText = append(columnIdToText, columnIdToTextForRow);
    }

    var result = toString(table.title) ~ '\n';

    var first = true;
    var totalWidth = 0;
    for (var column in table.columnDefinitions)
    {
        if (!first)
        {
            result ~= '|';
            totalWidth += 1;
        }
        totalWidth += columnIdToWidth[column.id];
        result ~= pad(columnIdToName[column.id], columnIdToWidth[column.id], ' ');
        first = false;
    }
    result ~= '\n' ~ pad('', totalWidth, '-');
    for (var row in columnIdToText)
    {
        result ~= '\n';
        first = true;
        for (var column in table.columnDefinitions)
        {
            if (!first)
                result ~= '|';
            result ~= pad(row[column.id], columnIdToWidth[column.id], ' ');
            first = false;
        }
    }

    return result;
}

export function toString(value is TemplateString) returns string
{
    var toProcess = value.template;
    var replaced = "";
    for (var i = 0; i < MAX_SUBSTITUTIONS; i += 1) // Don't need to handle more than 1000 substitutions
    {
        var m = match(toProcess, "([^#]*)[#](#| |[a-zA-Z_][0-9a-zA-Z_]*)([\\s\\S]*)");
        if (!m.hasMatch)
            return replaced ~ toProcess;
        replaced ~= m.captures[1];
        if (m.captures[2] == '#')
            replaced ~= '#';
        else if (m.captures[2] != ' ')
            replaced ~= toString(value[m.captures[2]]);
        toProcess = m.captures[3];
    }
    return replaced;
}

function pad(input is string, width is number, padding is string) returns string
{
    var toPad = width - length(input);
    if (toPad <= 0)
        return input;
    while (toPad > 1)
    {
        input = padding ~ input ~ padding;
        toPad -= 2;
    }
    if (toPad > 0)
        input ~= padding;
    return input;
}

