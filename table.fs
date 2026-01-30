FeatureScript 2878; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

export import(path : "onshape/std/containers.fs", version : "2878.0");
export import(path : "onshape/std/context.fs", version : "2878.0");
export import(path : "onshape/std/evaluate.fs", version : "2878.0");
export import(path : "onshape/std/math.fs", version : "2878.0");
export import(path : "onshape/std/properties.fs", version : "2878.0");
export import(path : "onshape/std/query.fs", version : "2878.0");
export import(path : "onshape/std/string.fs", version : "2878.0");
export import(path : "onshape/std/valueBounds.fs", version : "2878.0");
export import(path : "onshape/std/tabletextalignment.gen.fs", version : "2878.0");
export import(path : "onshape/std/templatestring.fs", version : "2878.0");
export import(path : "onshape/std/tolerance.fs", version : "2878.0");

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
        isTableValue(entry.value) || entry.value is TableCellError || entry.value is TableCellWithInfo;
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
    value is string || value is number || value is ValueWithUnits || value is TemplateString || value is StringWithTolerances || value is ValueWithUnitsAndPrecision;
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

// ----------------------------------- Table Cell With Info -----------------------------------

/**
 * A `TableCellWithInfo` represents a table cell with both a value and an info message. Such a cell has a displayed
 * value as well an info icon and a message that appears as a tooltip over the info icon.
 *
 * @type {{
 *      @field value : The displayed value, provided as a table value.
 *      @field info : The info message, provided as a table value.
 * }}
 */
export type TableCellWithInfo typecheck canBeTableCellWithInfo;

/** Typecheck for [TableCellWithInfo]. */
export predicate canBeTableCellWithInfo(value)
{
    value is map;
    isTableValue(value.value);
    isTableValue(value.info);
}

/** Constructs a [TableCellWithInfo] given a displayed value and an info message. */
export function tableCellWithInfo(value, info) returns TableCellWithInfo
precondition
{
    isTableValue(value);
    isTableValue(info);
}
{
    return { "value" : value, "info" : info } as TableCellWithInfo;
}

// ----------------------------------- Tolerance strings -----------------------------------

/**
 * @internal
 * Determines whether or not a value is string-like
 */
predicate isStringOrTemplateString(value)
{
    value is string || value is TemplateString;
}

/**
 * Represents a component with an inline part followed by stacked upper and lower components.
 * @type {{
 *      @field value : A value to be displayed as a regular row of text.
 *      @field upper : The upper component of the tolerance.
 *      @field lower : The lower component of the tolerance.
 * }}
 */
export type StringToleranceComponent typecheck canBeStringToleranceComponent;

/**
 * @internal
 * Typecheck for StringToleranceComponent
 */
export predicate canBeStringToleranceComponent(value)
{
    value is map;
    isStringOrTemplateString(value.value);
    isStringOrTemplateString(value.upper);
    isStringOrTemplateString(value.lower);
    isStringOrTemplateString(value.classification);
}

/** Constructor for StringToleranceComponent */
export function stringToleranceComponent(value is map) returns StringToleranceComponent
precondition canBeStringToleranceComponent(value);
{
    return value as StringToleranceComponent;
}

/**
 * Represents a compound string which may contain toleranced components.
 * @type {{
 *     @field components : An array of either strings, TemplateStrings, or StringToleranceComponents.
 * }}
 */
export type StringWithTolerances typecheck canBeStringWithTolerances;

/**
 * @internal
 * Typecheck for StringWithTolerances
 */
export predicate canBeStringWithTolerances(value)
{
    value is map;
    value.components is array;
    for (var component in value.components)
    {
        canBeStringToleranceComponent(component) || isStringOrTemplateString(component);
    }
}

/** Constructor for StringWithTolerances */
export function stringWithTolerances(value is map)
precondition canBeStringWithTolerances(value);
{
    return value as StringWithTolerances;
}

export function toString(value is StringToleranceComponent) returns string
{
    var output = toString(value.value);
    var tolerance = "";
    if (!isUndefinedOrEmptyString(value.upper))
    {
        tolerance ~= "upper: " ~ toString(value.upper);
    }
    if (!isUndefinedOrEmptyString(value.lower))
    {
        if (!isUndefinedOrEmptyString(tolerance))
        {
            tolerance ~= ", ";
        }
        tolerance ~= "lower: " ~ toString(value.lower);
    }
    if (!isUndefinedOrEmptyString(tolerance))
    {
        output ~= " [" ~ tolerance ~ "]";
    }
    return output;
}

export function toString(value is StringWithTolerances) returns string
{
    var output = "";

    for (var component in value.components)
    {
        output ~= toString(component);
    }

    return output;
}

/** Concantenates two [StringWithTolerances] values together. */
export function concatenateStringsWithTolerances(a is StringWithTolerances, b is StringWithTolerances) returns StringWithTolerances
{
    var result = stringWithTolerances({
        "components" : concatenateArrays([a.components, b.components])
    });
    return result;
}

predicate canBeToleranceComponent(value)
{
    value is string || value is TemplateString || canBeStringToleranceComponent(value);
}

/** Appends either a `string`, a [TemplateString], or a [StringToleranceComponent] to an existing [StringWithTolerances]. */
export function appendToleranceComponent(result is StringWithTolerances, component) returns StringWithTolerances
precondition canBeToleranceComponent(component);
{
    return stringWithTolerances({
        "components" : append(result.components, component)
    });
}

/** Creates a [StringWithTolerances] wrapping the specified component. */
export function createStringWithTolerances(component) returns StringWithTolerances
precondition canBeToleranceComponent(component);
{
    return stringWithTolerances({
        "components" : [component]
    });
}

function includePrecisionIfNeeded(value is ValueWithUnits, toleranceInfo is map) returns map
{
    if (toleranceInfo.usePrecisionOverride)
    {
        return valueWithUnitsAndPrecision(value, toleranceInfo.precision);
    }
    return value;
}

predicate isValueWithUnitsAndMaybePrecision(value)
{
    value is ValueWithUnits || value is ValueWithUnitsAndPrecision;
}

function valueWithUnitsAndMaybePrecisionToNumber(value) returns number
precondition isValueWithUnitsAndMaybePrecision(value);
{
    if (value is ValueWithUnits)
    {
        return value.value;
    }
    else
    {
        // [ValueWithUnitsAndPrecision] contains a [ValueWithUnits], which in
        // turn contains a numerical field
        return value.value.value;
    }
}

function signedValueToString(value) returns TemplateString
precondition isValueWithUnitsAndMaybePrecision(value);
{
    if (valueWithUnitsAndMaybePrecisionToNumber(value) < 0)
    {
        return templateString({ "template" : "#value", "value" : value });
    }
    else
    {
        return templateString({ "template" : "+#value", "value" : value });
    }
}

/**
 * Converts a ValueWithUnits and an associated ToleranceInfo into a StringWithTolerances.
 */
export function tolerancedValueToString(prefix is string, value is ValueWithUnits, tolerance) returns StringWithTolerances
precondition isToleranceInfoOrUndefined(tolerance);
{
    var component = stringToleranceComponent({
        "value" : templateString({
            "template" : "#prefix#field",
            "prefix" : prefix,
            "field" : value
        }),
        "upper" : "",
        "lower" : "",
        "classification" : ""
    });

    if (tolerance == undefined)
    {
        return createStringWithTolerances(component);
    }

    // Round and convert main field value
    component.value.field = includePrecisionIfNeeded(value, tolerance);

    var upper = tolerance.upper;
    var lower = tolerance.lower;

    if (upper != undefined)
    {
        upper = includePrecisionIfNeeded(upper, tolerance);
    }
    if (lower != undefined)
    {
        lower = includePrecisionIfNeeded(lower, tolerance);
    }

    if (tolerance.toleranceType == ToleranceType.DEVIATION)
    {
        // By default, the lower deviation is stored as a positive number
        // To render it with a - sign, we need to negate it
        lower = includePrecisionIfNeeded(-1 * tolerance.lower, tolerance);
        component.upper = signedValueToString(upper);
        component.lower = signedValueToString(lower);
    }
    else if (tolerance.toleranceType == ToleranceType.LIMITS ||
            tolerance.toleranceType == ToleranceType.FIT_WITH_TOLERANCE ||
            tolerance.toleranceType == ToleranceType.FIT_TOLERANCE_ONLY)
    {
        upper = includePrecisionIfNeeded(value + tolerance.upper, tolerance);
        lower = includePrecisionIfNeeded(value - tolerance.lower, tolerance);
        const upperTemplate = templateString({ "template" : " #upperValue", "upperValue" : upper });
        const lowerTemplate = templateString({ "template" : " #lowerValue", "lowerValue" : lower });
        component.upper = upperTemplate;
        component.lower = lowerTemplate;
    }

    if (tolerance.toleranceFitInfo != undefined && (tolerance.toleranceType == ToleranceType.FIT || tolerance.toleranceType == ToleranceType.FIT_WITH_TOLERANCE))
    {
        const classificationTemplate = templateString({ "template" : " #class", "class" : tolerance.toleranceFitInfo.holeClass });
        component.classification = classificationTemplate;
    }

    var result = createStringWithTolerances(component);

    if (tolerance.toleranceType == ToleranceType.SYMMETRICAL)
    {
        const templateEntry = templateString({ "template" : " ±#tolerance", "tolerance" : upper });
        result = appendToleranceComponent(result, templateEntry);
    }
    else if (tolerance.toleranceType == ToleranceType.MIN)
    {
        result = appendToleranceComponent(result, " MIN");
    }
    else if (tolerance.toleranceType == ToleranceType.MAX)
    {
        result = appendToleranceComponent(result, " MAX");
    }

    return result;
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

