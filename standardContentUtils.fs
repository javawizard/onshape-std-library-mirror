FeatureScript 608; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/containers.fs", version : "608.0");
import(path : "onshape/std/string.fs", version : "608.0");
import(path : "onshape/std/units.fs", version : "608.0");
export import(path : "onshape/std/standardcontentvaluefieldtype.gen.fs", version : "608.0");

/*
 ******************************************
 * Utility functions for internal development Onshape Standard Content.

 * These functions are not yet intended for general use, and may be removed or have modified protocols in future versions of the Onshape Standard Library.
 ******************************************
 */

/**
 * @internal
 * Data table definition used for standard content
 * @type {{
 *      @field tableName {string}: The name of the data table
 *      @field fields {array}: An array of string field names
 *      @field rows {array}: An array of arrays for each row in the table. Each row array is an array of string field values and must contain the same number of values as the number of fields
 * }}
 */
export type DataTable typecheck canBeDataTable;

/**
 * @internal
 * Typecheck for [DataTable]
 */
export predicate canBeDataTable(value)
{
    value is map;
    value.fields is array;
    for (var fieldName in value.fields)
    {
      fieldName is string;
    }
    value.rows is array;
    for (var rowValues in value.rows)
    {
      rowValues is array;
      size(rowValues) == size(value.fields);
      for (var rowValue in rowValues)
      {
        rowValue is string;
      }
    }
}

/**
 * @internal
 * All data tables used by standard content
 * @type {map} : A map of of the data tables, each string key is the table name, value is a DataTable
 */
export type DataTables typecheck canBeDataTables;

/**
 * @internal
 * Typecheck for [DataTables]
 */
export predicate canBeDataTables(values)
{
    values is map;
    for (var dataTable in values)
    {
        dataTable.key is string;
        dataTable.value is DataTable;
    }
}

/**
 * @internal
 * Parameters with its default value
 * @type {array} : An array of maps, where the map object key is the string parameter name, and the value is the default string value for the parameter
 */
export type ParameterSpecs typecheck canBeParameterSpecs;

/**
 * @internal
 * Typecheck for [ParameterSpecs]
 */
export predicate canBeParameterSpecs(value)
{
    value is array;
    for (var parameterSpec in value)
    {
        parameterSpec is map;
        size(parameterSpec) == 1;
        for (var entry in parameterSpec)
        {
          entry.key is string;
          entry.value is string;
        }
   }
}

/**
 * @internal
 * Parameter default value and the data table row index for that value
 * @type {{
 *      @field value {string}: The default value
 *      @field row {number}: The data table row index for that value
  * }}
 */
export type ParameterValue typecheck canBeParameterValue;

/**
 * @internal
 * Typecheck for [ParameterValue]
 */
export predicate canBeParameterValue(value)
{
    value is map;
    size(value) == 2;
    value["value"] is string;
    value["row"] is number;
}

/**
 * @internal
 * Filter results for a parameter
 * @type {{
 *      @field ParameterName {string} : name of the parameter
 *      @field Visible {boolean} : whether (true) or not (false) the parameter is visible in the UI
 *      @field DataTable {string} : the data table name referenced by this parameter, if any
 *      @field DefaultValue {ParameterValue} : default value for this parameter
 *      @field ValueFieldType {StandardContentValueFieldType} : the parameter's value field control type
 *      @field ParameterValues {array} : Array of ParameterValue, containing all valid values for this parameter
 *      @field ConfigurationParameters {{
 *        @field key {string} : the name of the configuration parameter to set
 *        @field value : the value to set the configuration parameter to
 *      }}
 * }}
 */
export type ParameterFilter typecheck canBeParameterFilter;

/**
 * @internal
 * Typecheck for [ParameterFilter]
 */
export predicate canBeParameterFilter(value)
{
    value is map;
    value.ParameterName is string;
    value.DataTable is string;
    value.Visible is boolean;
    value.ValueFieldType is undefined || value.ValueFieldType is StandardContentValueFieldType;
    value.DefaultValue is ParameterValue;
    value.ParameterValues is array;
    for (var parameterValue in value.ParameterValues)
    {
      parameterValue is ParameterValue;
    }
    if (!(value.ConfigurationParameters is undefined))
    {
      value.ConfigurationParameters is map;
      for (var configParameter in value.ConfigurationParameters)
      {
        configParameter.key is string;
      }
    }
}

/**
 * @internal
 * Option map definition used for standard content
 * @type  {{
 *        @field key {string} : the name of the option
 *        @field value : the value of the option
 *        }}
 */
export type OptionMap typecheck canBeOptionMap;

/**
 * @internal
 * Typecheck for [OptionMap]
 */
export predicate canBeOptionMap(values)
{
    values is map;
    for (var value in values)
    {
        value.key is string;
    }
}

/**
 * @internal
 * Total filter results for all parameters
 * @type {{
 *      @field parameters {array} : Array of ParameterFilter
 *      @field parameterIndexes {{
 *        @field key {string} : the name of the configuration parameter
 *        @field value {number} : the index into the parameters array containing this parameters' specification
 *      }}
 *      @field options {map} : A mapping of OptionMap option types
 * }}
 */
export type ContentFilter typecheck canBeContentFilter;

/**
 * @internal
 * Typecheck for [ContentFilter]
 */
export predicate canBeContentFilter(value)
{
    value is map;
    value.parameters is array;
    for (var parameterEntry in value.parameters)
    {
      parameterEntry is ParameterFilter;
    }
    value.parameterIndexes is map;
    for (var parameterIndex in value.parameterIndexes)
    {
        parameterIndex.key is string;
        parameterIndex.value is number;
    }
    value.options is map;
    for (var option in value.options)
    {
      option is OptionMap;
    }
}

/**
 * @internal
 * Constructs a default filter map for the parameters specified.
 *
 * @param parameters {ParameterSpecs} : the array of parameter map object, where the map object key is the parameter name, and the value is the default value for the parameter
 * @returns {ContentFilter} : the default content filter map
 */
export function scMakeFilterArray(parameters is ParameterSpecs) returns ContentFilter
{
    var parameterFilterArray is array = [];
    var parameterIndexes is map = {};
    var numberOfParameters = size(parameters);
    for (var index = 0; index < numberOfParameters; index += 1)
    {
        var parameterEntry = parameters[index];
        if (parameterEntry is map)
        {
            var parameterName = "<unknown>";
            for (var parameterSpec in parameterEntry)
            {
                parameterName = parameterSpec.key;

                // there should only be one entry in the map
                break;
            }
            parameterFilterArray = append(parameterFilterArray, {
                                                "Visible" : true,
                                                "DefaultValue" : { "value" : "", "row" : -1 } as ParameterValue,
                                                "DataTable" : "",
                                                "ParameterValues" : [],
                                                "ParameterName" : parameterName } as ParameterFilter
                                        );
            parameterIndexes[parameterName] = index;
        }
    }

    return { "parameters" : parameterFilterArray, "parameterIndexes" : parameterIndexes, "options" : {} } as ContentFilter;
}

/**
 * @internal
 * Builds and returns a parameter values array of static values that can be used as filtered values in the master filter results array.

 * @param staticValues {array} : array of strings to build the parameter values array from
 * @returns {array} : array of ParameterValue that can be used as the filtered values in the master filter results array
*/
export function scStaticParameterValues(staticValues is array) returns array
{
    var parameterValues is array = [];
    for (var staticValueStr in staticValues)
    {
        parameterValues = append(parameterValues, { "value" : staticValueStr, "row" : -1 } as ParameterValue);
    }

    return parameterValues;
}

/**
 * @internal
 * Calculates and returns the default value within the specified parameter array.
 * Will look in the existing values of the parameter values, if the desiredDefaultValue exists, will use that.
 * If not, will use the first existing value. If no existing value, uses an empty string.

 * @param parameterDesiredDefaultValue {string} : the desired default value
 * @param parameterValueArray {array} : the filter result parameter array to set the default value to
 * @returns {ParameterValue} : the ParameterValue of the default value that was calculated for the specified parameter array
*/
export function scSetDefaultValue(parameterDesiredDefaultValue is string, parameterValueArray is array) returns ParameterValue
{
    // if desired default value exists in value array, use it
    for (var parameterSpec in parameterValueArray)
    {
        if (parameterSpec["value"] != undefined && parameterSpec["value"] == parameterDesiredDefaultValue)
        {
            return { "value" : parameterDesiredDefaultValue, "row" : parameterSpec["row"] } as ParameterValue;
        }
    }

    // desired default value didn't exist in values array, use first value if present
    if (size(parameterValueArray) > 0 && parameterValueArray[0]["value"] != undefined)
    {
        return { "value" : parameterValueArray[0]["value"], "row" : parameterValueArray[0]["row"] } as ParameterValue;
    }

    // no default or current values, empty string
    return { "value" : "", "row" : -1 } as ParameterValue;
}

/**
 * @internal
 * Gets the default value within the specified parameter specification map.

 * @param parameterSpecMap {ParameterFilter} : the filter result parameter specification map
 * @returns {string} : the current default value, if doesn't exist, uses empty string
*/
export function scGetDefaultValue(parameterSpecMap is ParameterFilter) returns string
{
    if (parameterSpecMap.DefaultValue != undefined && parameterSpecMap.DefaultValue.value != undefined)
    {
        return parameterSpecMap.DefaultValue.value;
    }

    return "";
}

/**
 * @internal
 * Gets the default data value from the specified parameter specification map.
 * Some parameters get their data from a data table, and if so, the current value of that parameter will come from a specific row index
 * in the table. This method is used to get another field value within that current default value row.

 * @param dataTables {DataTables} : the data tables map
 * @param fieldName {string} : the field name to get the data value for
 * @param parameterSpecMap {ParameterFilter} : the parameter specification map to get the default data value for
 * @returns {string} : the current default data value of the specified field within the specified parameter specification map, if the parameter specification map doesn't
 *                      use a data table, or the field doesn't exists, an empty string is used
*/
export function scGetDefaultDataValue(dataTables is DataTables, fieldName is string, parameterSpecMap is ParameterFilter) returns string
{
    if (parameterSpecMap.DataTable != undefined)
    {
        if (parameterSpecMap.DefaultValue != undefined && parameterSpecMap.DefaultValue["row"] != undefined)
        {
            return scFindAndGetRowFieldValueFromTable(dataTables, parameterSpecMap.DataTable, fieldName,parameterSpecMap.DefaultValue["row"]);
        }
    }

    return "";
}

/**
 * @internal
 * Gets the default value for a specified parameter within the specified current filter map.

 * @param parameterName {string} : the name of the parameter to get the default value for
 * @param currentFilter {ContentFilter} : the current content filter results map
 * @returns {string} : the current default value of the specified parameter, if doesn't exist, uses empty string
*/
export function scGetParameterDefaultFilterValue(parameterName is string, currentFilter is ContentFilter) returns string
{
    if (currentFilter.parameters != undefined)
    {
        var parameterIndex = currentFilter.parameterIndexes[parameterName];

        if (parameterIndex != undefined)
        {
            var parameterSpecMap = currentFilter.parameters[parameterIndex];
            if (parameterSpecMap.DefaultValue != undefined && parameterSpecMap.DefaultValue.value != undefined)
            {
                return parameterSpecMap.DefaultValue.value;
            }
        }
    }

    return "";
}

/**
 * @internal
 * Get all the field values for the specified field in the specified table, where the value of each field specified in the match field map matches its specified match field value.

 * @param dataTables {DataTables} : map of data tables
 * @param tableName {string} : the table name in the data tables map
 * @param fieldName {string} : the field name in the table to retrieve values for
 * @param matchFields {{}} : key (string), value (string) pairs of the field name (key) that must match the field value (value). If all match, then the fieldName value is used.
 *                           An empty map eliminates any matching and returns all field values for the specified field in the specified table.
 * @returns {array} : an array of ParameterValue found in the data table, in which all the matchFields equal their specified matchField value, or all field
 *                    values for the specified field in the specified table if the match fields map is empty.
 */
export function scGetFieldValuesFromTable(dataTables is DataTables, tableName is string, fieldName is string, matchFields is map) returns array
{
    var table = dataTables[tableName];

    if (table == undefined)
    {
        return [];
    }

    return scGetFieldValuesFromTable(table, fieldName, matchFields);
}

/**
 * @internal
 * Gets the default data value for a specified parameter for a specified data field from within the specified current filter map.
 * Some parameters get their data from a data table, and if so, the current value of that parameter will come from a specific row index
 * in the table. This method is used to get another field value within that current default value row.

 * @param dataTables {DataTables} : the data tables map
 * @param parameterName {string} : the name of the parameter to get the default data value for
 * @param fieldName {string} : the field name to get the data value for
 * @param currentFilter {ContentFilter} : the current filter results map
 * @returns {string} : the current default data value of the specified field for the specified parameter, if the parameter doesn't use a data table, or the field doesn't
 *                      exists, an empty string is used.
*/

export function scGetParameterDefaultDataValue(dataTables is DataTables, parameterName is string, fieldName is string, currentFilter is ContentFilter) returns string
{
    var parameterIndex = currentFilter.parameterIndexes[parameterName];

    if (parameterIndex != undefined)
    {
        var parameterSpecMap = currentFilter.parameters[parameterIndex];
        var dataTable = parameterSpecMap["DataTable"];

        if (dataTable != undefined && dataTable != "")
        {
            var defaultValue = parameterSpecMap["DefaultValue"];
            if (defaultValue != undefined)
            {
                var parameterDefaultDataRowIndex = defaultValue["row"];
                if (parameterDefaultDataRowIndex != undefined && parameterDefaultDataRowIndex >= 0)
                {
                    var table = dataTables[dataTable];

                    if (table != undefined && size(table) > 0)
                    {
                        return scGetRowFieldValueFromTable(table, fieldName, parameterDefaultDataRowIndex);
                    }
                }
            }
        }
    }

    return "";
}

/**
 * @internal
 * Concatenates the 2 field values from the 2 specified field names from the specified table for all rows in the table. The field values
 * are delimited by the specified delimiter string.

 * @param dataTables {DataTables} : map of data tables
 * @param tableName {string} : the table name in the data tables map of which to get values from
 * @param fieldName1 {string} : the first field name to get values from
 * @param delimiter {string} : the delimiter string that separates the 2 field values
 * @param fieldName2 {string} : the second field name to get values from
 * @returns {array} : an array of ParameterValue of the concatenation of the 2 fields, delimited by the delimiter string, for each row in the table
*/
export function scConcatenate2DataFields(dataTables is DataTables, tableName is string, fieldName1 is string, delimiter is string, fieldName2 is string) returns array
{
    var returnValues = [];

    var table = dataTables[tableName];

    if (table == undefined || size(table) == 0)
    {
        return returnValues;
    }

    var field1Values = scGetFieldValuesFromTable(table, fieldName1, {});

    if (size(field1Values) == 0)
    {
        return returnValues;
    }

    var field2Values = scGetFieldValuesFromTable(table, fieldName2, {});

    if (size(field2Values) == 0)
    {
        return returnValues;
    }

    const valueSize = size(field1Values);

    if (valueSize == size(field2Values))
    {
        for (var valueIndex = 0; valueIndex < valueSize; valueIndex += 1)
        {
            returnValues = append(returnValues, {
                                        "value" : field1Values[valueIndex]["value"] ~ delimiter ~ field2Values[valueIndex]["value"],
                                        "row" : field1Values[valueIndex]["row"] } as ParameterValue
                                 );
        }
    }

    return scRemoveDupsFromParameterValuesArray(returnValues);
}

/**
 * @internal
 * Generate a ValueWithUnits type for the specified string that represent a valid number in the specified units

 * @param value {string} : the string that represent a valid number in the specified units
 * @param unit {ValueWithUnits} : the units the specified string represents
 * @returns {ValueWithUnits} : a ValueWithUnits type for the specified string
*/
export function scStringToValueWithUnits(value is string, unit is ValueWithUnits) returns ValueWithUnits
{
    return stringToNumber(value) * unit;
}

/**
 * @internal
 * Prints out the specified filter array in a readable format.

 * @param currentFilter {ContentFilter} : the current filter results map
*/
export function scPrettyPrint(currentFilter is ContentFilter)
{
    for (var filterEntry in currentFilter)
    {
        if (filterEntry.key == "parameterIndexes")
        {
            for (var parameterIndex in filterEntry.value)
            {
                println("Parameter index: \"" ~ parameterIndex.key ~ "\" = " ~ parameterIndex.value);
            }

        }
        else if (filterEntry.key == "parameters")
        {
            for (var parameterSpecMap in filterEntry.value)
            {
                println("Parameter Name: " ~ parameterSpecMap["ParameterName"]);
                println("    visible: " ~ parameterSpecMap["Visible"]);
                println("    data table: " ~ parameterSpecMap["DataTable"]);
                var valueFieldType = parameterSpecMap["valueFieldType"];
                if (valueFieldType == undefined)
                {
                    println("    value field type: " ~ StandardContentValueFieldType.DROPLIST);
                }
                else
                {
                    println("    value field type: " ~ valueFieldType);
                }
                println("    default value: " ~ parameterSpecMap["DefaultValue"]["value"]);
                println("    parameter values:");
                for (var parameterValueSpecEntry in parameterSpecMap["ParameterValues"])
                {
                    println("        " ~ parameterValueSpecEntry["value"]);
                }
                if (parameterSpecMap["ConfigurationParameters"] != undefined)
                {
                    for (var configParameterEntry in parameterSpecMap["ConfigurationParameters"])
                    {
                      for (var configParameter in configParameterEntry)
                      {
                          println("    Configuration parameter: \"" ~ configParameter.key ~ "\" = " ~ configParameter.value);
                      }
                    }
                }
                println("");
            }
        }
    }
}

/**
 * @internal
 * Removes duplicates from the specified string array.

 * @param sourceArray {array} : an array of ParameterValue to check duplicates from
 * @returns {array} : the resultant array of ParameterValue with no duplicates
*/
function scRemoveDupsFromParameterValuesArray(sourceArray is array) returns array
{
    var uniqueArray is array = [];
    var uniqueMap is map = {};

    for (var parameterMap in sourceArray)
    {
        if (uniqueMap[parameterMap["value"]] == undefined)
        {
            uniqueArray = append(uniqueArray, parameterMap);
            uniqueMap[parameterMap["value"]] = 0;
        }
    }

    return uniqueArray;
}

/**
 * @internal
 * Finds the field index of the specified field in the specified table.

 * @param table {DataTable} : the data table map
 * @param fieldName {string} : the field name in the table to find the field index for
 * @returns {number} : the field index (base 0) for the specified field, returns -1 if field does not exist in the table
*/
function scFindFieldIndexInTable(table is DataTable, fieldName is string) returns number
{
    var fieldIndex is number = 0;

    for (var field in table.fields)
    {
        if (field == fieldName)
        {
            return fieldIndex;
        }

        fieldIndex += 1;
    }

    return -1;
}

/**
 * @internal
 * Get all the field values for the specified field in the specified table, where the value of each field specified in the match field map matches its specified match field value.

 * @param dataTables {DataTables} : map of data tables
 * @param tableName {string} : the table name in the data tables map
 * @param fieldName {string} : the field name in the table to retrieve values for
 * @param matchFields {{}} : key (string), value (string) pairs of the field name (key) that must match the field value (value). If all match, then the fieldName value is used.
 *                           An empty map eliminates any matching and returns all field values for the specified field in the specified table.
 * @returns {array} : an array of ParameterValue found in the data table, in which all the matchFields equal their specified matchField value, or all field
 *                    values for the specified field in the specified table if the match fields map is empty.
 */
/**
 * @internal
 * Get all the field values for the specified field in the specified table, where the value of each field specified in the match field map matches its specified match field value.

 * @param table {DataTable} : the data table map
 * @param fieldName {string} : the field name in the table to retrieve values for
 * @param matchFields {{}} : key (string), value (string) pairs of the field name (key) that must match the field value (value). If all match, then the fieldName value is used.
 *                           An empty map eliminates any matching and returns all field values for the specified field in the specified table.
 * @returns {array} : an array of ParameterValue found in the data table, in which all the matchFields equal their specified matchField value, or all field
 *                    values for the specified field in the specified table if the match fields map is empty.
*/
function scGetFieldValuesFromTable(table is DataTable, fieldName is string, matchFields is map) returns array
{
    var fieldIndex is number = scFindFieldIndexInTable(table, fieldName);

    if (fieldIndex == -1)
    {
        // specified source field doesn't exist
        return [];
    }

    var matchFieldIndexes is map = {};

    for (var matchField in matchFields)
    {
        var matchFieldIndex = scFindFieldIndexInTable(table, matchField.key);
        if (matchFieldIndex == -1)
        {
            // specified match field doesn't exist
            return [];
        }

        matchFieldIndexes[matchFieldIndex] = matchField.value;
    }

    var fieldValues = [];

    var numberOfRows = size(table.rows);

    for (var rowIndex = 0; rowIndex < numberOfRows; rowIndex += 1)
    {
        var numberOfFieldValues = size(table.rows[rowIndex]);

        if (numberOfFieldValues > fieldIndex)
        {
            var allMatched is boolean = true;
            for (var matchFieldIndex in matchFieldIndexes)
            {
                if (table.rows[rowIndex][matchFieldIndex.key] != matchFieldIndex.value)
                {
                    allMatched = false;
                    break;
                }
            }
            if (allMatched)
            {
                fieldValues = append(fieldValues, { "value" : table.rows[rowIndex][fieldIndex], "row" : rowIndex } as ParameterValue);
            }
        }
    }

    return fieldValues;
}

/**
 * @internal
 * Gets the value of the specified field from the specified table at the specified row index in the table.

 * @param table {DataTable} : the data table map
 * @param fieldName {string} : the field name in the table to retrieve the value for
 * @param rowIndex {number} : the row index (base 0) in the table to get the field value for
 * @returns {string} : the value of the specified field in the specified table within the specified row index, empty string if not found
*/
function scGetRowFieldValueFromTable(table is DataTable, fieldName is string, rowIndex is number) returns string
{
    if (table.rows != undefined && rowIndex < size(table.rows))
    {
      var fieldIndex is number = 0;

      for (var field in table.fields)
      {
          if (field == fieldName)
          {
              return table.rows[rowIndex][fieldIndex];
          }

          fieldIndex += 1;
      }
    }

    return "";
}

/**
 * @internal
 * Get the value of the specified field from the specified table at the specified row index.

 * @param dataTables {DataTables} : the data tables map
 * @param dataTableName {string} : the name of the data table in the data tables map
 * @param fieldName {string} : the field name to get the data value for
 * @param rowIndex {number} : the row index (base 0) in the table to get the field value for
 * @returns {string} : the value of the specified field in the specified table within the specified row index, empty string if not found
*/
function scFindAndGetRowFieldValueFromTable(dataTables is DataTables, dataTableName is string, fieldName is string, rowIndex is number) returns string
{
    if (rowIndex >= 0 && length(dataTableName) > 0)
    {
        var table = dataTables[dataTableName];

        if (table != undefined && size(table) > 0)
        {
            return scGetRowFieldValueFromTable(table, fieldName, rowIndex);
        }
    }

    return "";
}

