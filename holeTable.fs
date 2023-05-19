FeatureScript 2045; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "2045.0");
import(path : "onshape/std/hole.fs", version : "2045.0");
import(path : "onshape/std/table.fs", version : "2045.0");

const TAG_COLUMN_KEY = "tag";
const SIZE_COLUMN_KEY = "size";
const ANGLE_COLUMN_KEY = "angle";
const QUANTITY_COLUMN_KEY = "quantity";

/** Computes one hole table for each part */
annotation { "Table Type Name" : "Hole table" }
export const holeTable = defineTable(function(context is Context, definition is map) returns TableArray
    precondition
    {
    }
    {
        const attributePattern = {} as HoleAttribute;

        var tables = [];

        var partAndBodyToRowToData = {};

        var showAngle = false;
        var lastAngle = undefined;

        for (var partAndBodies in allSolidsAndClosedComposites(context))
        {
            var featureAndNumberToFaces = {};
            var sizeToRowData = {};

            var holeFacesQuery = qAttributeFilter(qOwnedByBody(partAndBodies.bodies, EntityType.FACE), attributePattern);
            for (var face in evaluateQuery(context, holeFacesQuery))
            {
                var attribute = @getAttributes(context, { "entities" : face, "attributePattern" : attributePattern })[0];
                if (attribute.isExternalThread == true)
                {
                    continue;
                }
                var featureAndNumber = [attribute.holeFeatureCount, attribute.holeNumber, attribute.attributeId];
                var current = featureAndNumberToFaces[featureAndNumber];
                if (current != undefined)
                    featureAndNumberToFaces[featureAndNumber] = append(current, face);
                else
                    featureAndNumberToFaces[featureAndNumber] = [face];

                var tipAngle = computeAngle(attribute);
                if (lastAngle == undefined)
                    lastAngle = tipAngle;
                else if (tipAngle != lastAngle)
                    showAngle = true;
            }

            var numRows = 0;
            var tag = 'A';
            for (var entry in featureAndNumberToFaces)
            {
                var attribute = @getAttributes(context, { "entities" : entry.value[0], "attributePattern" : attributePattern })[0];
                var size = computeSize(attribute);
                var sizeSignature = sizeToSizeSignature(size);

                const tipAngle = computeAngle(attribute);

                const holeKey = [sizeSignature, tipAngle];

                const current = sizeToRowData[holeKey];
                if (current != undefined)
                {
                    sizeToRowData[holeKey].quantity += 1;
                    sizeToRowData[holeKey].faces = append(current.faces, entry.value);
                }
                else
                {
                    sizeToRowData[holeKey] = { (QUANTITY_COLUMN_KEY) : 1, "row" : numRows, "faces" : [entry.value], (TAG_COLUMN_KEY) : tag, (ANGLE_COLUMN_KEY) : tipAngle, (SIZE_COLUMN_KEY) : size };
                    tag = nextTag(tag);
                    numRows += 1;
                }
            }
            var rowToData = {};
            for (var entry in sizeToRowData)
                rowToData[entry.value.row] = entry.value;

            partAndBodyToRowToData[partAndBodies] = rowToData;
        }

        var columnDefinitions = [];
        columnDefinitions = append(columnDefinitions, tableColumnDefinition(TAG_COLUMN_KEY, "Tag"));
        columnDefinitions = append(columnDefinitions, tableColumnDefinition(SIZE_COLUMN_KEY, "Size"));

        if (showAngle)
        {
            columnDefinitions = append(columnDefinitions, tableColumnDefinition(ANGLE_COLUMN_KEY, "Drill angle"));
        }

        columnDefinitions = append(columnDefinitions, tableColumnDefinition(QUANTITY_COLUMN_KEY, "Qty"));

        for (var partAndBodies in allSolidsAndClosedComposites(context))
        {
            var rows = [];

            var rowToData = partAndBodyToRowToData[partAndBodies];

            for (var entry in rowToData)
            {
                var holeFaces = qUnion(concatenateArrays(entry.value.faces));
                var holeEdges = qAdjacent(holeFaces, AdjacencyType.EDGE, EntityType.EDGE);

                var rowMap = {
                    (TAG_COLUMN_KEY) : entry.value.tag,
                    (SIZE_COLUMN_KEY) : entry.value.size,
                    (QUANTITY_COLUMN_KEY) : entry.value.quantity
                };
                if (showAngle)
                {
                    rowMap[ANGLE_COLUMN_KEY] = entry.value.angle;
                }
                rows = append(rows, tableRow(rowMap, qUnion([holeFaces, holeEdges])));
            }
            //for local testing comment the getProperty method
            var partName = getProperty(context, { "entity" : partAndBodies.part, "propertyType" : PropertyType.NAME });
            var title = { "template" : "#name", "name" : partName } as TemplateString; // Use a template so we don't try to translate the part name

            tables = append(tables, table(title, columnDefinitions, rows, partAndBodies.part));
        }
        return tableArray(tables);
    });

function computeAngle(attribute is HoleAttribute) returns StringWithTolerances
{
    if (attribute.endType != HoleEndStyle.THROUGH)
    {
        return tolerancedValueToString("", attribute.tipAngle, attribute.tolerances.tipAngle);
    }
    else
    {
        return createStringWithTolerances(" ");
    }
}

function computeSize(attribute is HoleAttribute) returns StringWithTolerances
{
    if (attribute.isExternalThread == true)
    {
        return createStringWithTolerances("External Thread");
    }

    var result = tolerancedValueToString("⌀", attribute.holeDiameter, attribute.tolerances.holeDiameter);

    // THRU or depth
    if (attribute.endType == HoleEndStyle.THROUGH)
    {
        if (attribute.partialThrough != true)
        {
            result = appendToleranceComponent(result, " THRU");
        }
    }
    else
    {
        result = concatenateStringsWithTolerances(
            result,
            tolerancedValueToString("↧", attribute.holeDepth, attribute.tolerances.holeDepth)
        );
    }

    // Counterbore or countersink
    if (attribute.holeType == HoleStyle.C_BORE)
    {
        result = concatenateStringsWithTolerances(
            result,
            tolerancedValueToString("\n⌴⌀", attribute.cBoreDiameter, attribute.tolerances.cBoreDiameter)
        );
        result = concatenateStringsWithTolerances(
            result,
            tolerancedValueToString("↧", attribute.cBoreDepth, attribute.tolerances.cBoreDepth)
        );
    }

    if (attribute.holeType == HoleStyle.C_SINK)
    {
        result = concatenateStringsWithTolerances(
            result,
            tolerancedValueToString("\n⌵⌀", attribute.cSinkDiameter, attribute.tolerances.cSinkDiameter)
        );
        result = concatenateStringsWithTolerances(
            result,
            tolerancedValueToString("X ", attribute.cSinkAngle, attribute.tolerances.cSinkAngle)
        );
    }

    // Tapped
    if (attribute.isTappedHole)
    {
        result = appendToleranceComponent(result, "\n" ~ attribute.tapSize);
        if (attribute.isTappedThrough)
        {
            result = appendToleranceComponent(result, " THRU");
        }
        else
        {
            result = concatenateStringsWithTolerances(
                result,
                tolerancedValueToString("↧", attribute.tappedDepth, attribute.tolerances.tappedDepth)
            );
        }
    }
    else if (attribute.isTaperedPipeTapHole)
    {
        result = appendToleranceComponent(result, "\n" ~ attribute.tapSize);
        if (attribute.standard == "ANSI")
        {
            result = appendToleranceComponent(result, " NPT");
        }
        else if (attribute.standard == "ISO")
        {
            result = appendToleranceComponent(result, " RC TAPPED HOLE");
        }
    }

    return result;
}

function roundStringValues(template is map, precision is number) returns TemplateString
{
    if (template is TemplateString)
    {
        for (var key, value in template)
        {
            if (value is ValueWithUnits)
            {
                template[key].value = roundToPrecision(value.value, precision);
            }
        }
    }
    return template;
}

function roundToleranceComponentField(component is map, field is string, precision is number)
{
    if (!isUndefinedOrEmptyString(component[field]))
    {
        component[field] = roundStringValues(component[field], precision);
    }
}

function roundToleranceComponent(component is StringToleranceComponent, precision is number) returns StringToleranceComponent
{
    roundToleranceComponentField(component, "value", precision);
    roundToleranceComponentField(component, "upper", precision);
    roundToleranceComponentField(component, "lower", precision);

    return component;
}

function roundStringWithTolerances(template is StringWithTolerances, precision is number) returns StringWithTolerances
{
    for (var index, component in template.components)
    {
        if (component is StringToleranceComponent)
        {
            template.components[index] = roundToleranceComponent(component, precision);
        }
        else if (component is TemplateString)
        {
            template.components[index] = roundStringValues(component, precision);
        }
    }
    return template;
}

function sizeToSizeSignature(holeSize is StringWithTolerances) returns map
{
    return roundStringWithTolerances(holeSize, 8);
}

// Tag logic

const TAG_ALPHABET = "ABCDEFGHJKLMNPRTUVWY";
const NEXT_TAG = initNextTag(TAG_ALPHABET);

function initNextTag(alphabet is string)
{
    var result = {};
    var arr = splitIntoCharacters(alphabet);
    for (var i = 0; i + 1 < size(arr); i += 1)
        result[arr[i]] = arr[i + 1];
    return result;
}

function nextTag(tag is string) returns string
{
    var characters = splitIntoCharacters(tag);
    for (var i = size(characters) - 1; i >= 0; i -= 1)
    {
        var next = NEXT_TAG[characters[i]];
        if (next != undefined)
        {
            characters[i] = next;
            break;
        }
        characters[i] = 'A';
        if (i == 0)
            characters = concatenateArrays([['A'], characters]);
    }
    var result = "";
    for (var c in characters)
        result ~= c;
    return result;
}

