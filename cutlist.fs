FeatureScript 1711; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/cutlistMath.fs", version : "1711.0");
import(path : "onshape/std/error.fs", version : "1711.0");
import(path : "onshape/std/feature.fs", version : "1711.0");
import(path : "onshape/std/frameAttributes.fs", version : "1711.0");
import(path : "onshape/std/frameUtils.fs", version : "1711.0");
import(path : "onshape/std/table.fs", version : "1711.0");

/**
 * @internal
 * Construct a proper column override by injecting necessary defaults.
 */
export function cutlistColumnOverride(override is map)
{
    return mergeMaps({
                "isFirst" : false, // Not used programatically by feature.
                "sameFramesAsPrevious" : false,
                "overrideAllFrames" : false
            }, override);
}

/**
 * Create a cut list from a set of frame selections.
 */
annotation {
        "Feature Type Name" : "Cut list",
        "Editing Logic Function" : "cutlistEditLogic"
    }
export const cutlist = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Select all frames" }
        definition.selectAllFrames is boolean;

        if (!definition.selectAllFrames)
        {
            // BEL-172690: add filter for frames
            annotation {
                        "Name" : "Frames",
                        "Filter" : EntityType.BODY && BodyType.SOLID
                    }
            definition.frames is Query;
        }

        annotation {
                    "Name" : "Overrides",
                    "Item name" : "Column Override",
                    "Item label template" : "#column = #value",
                    "UIHint" : UIHint.PREVENT_ARRAY_REORDER
                }
        definition.columnOverrides is array;
        for (var columnOverride in definition.columnOverrides)
        {
            // Set by editing logic and used in dialog only.  Feature should not rely on this parameter and should
            // instead just check use the array index of the override if needed.
            annotation {
                        "Name" : "Is first",
                        "UIHint" : UIHint.ALWAYS_HIDDEN
                    }
            columnOverride.isFirst is boolean;

            if (!columnOverride.isFirst)
            {
                annotation {
                            "Name" : "Same frames as previous",
                            "Default" : true
                        }
                columnOverride.sameFramesAsPrevious is boolean;
            }

            if (columnOverride.isFirst || !columnOverride.sameFramesAsPrevious)
            {
                annotation {
                            "Name" : "All frames",
                            "Default" : false
                        }
                columnOverride.overrideAllFrames is boolean;

                if (!columnOverride.overrideAllFrames)
                {
                    annotation {
                                "Name" : "Frames",
                                "Filter" : EntityType.BODY
                            }
                    columnOverride.overrideFrames is Query;
                }
            }

            annotation {
                        "Name" : "Column",
                        "MaxLength" : 10000
                    }
            columnOverride.column is string;

            annotation {
                        "Name" : "Value",
                        "MaxLength" : 10000
                    }
            columnOverride.value is string;
        }
    }
    {
        doOneCutlist(context, id, definition);
    },
    { selectAllFrames : false, columnOverrides : [] });

function doOneCutlist(context is Context, id is Id, definition is map)
{
    verify(!isInFeaturePattern(context), ErrorStringEnum.FRAME_CUTLIST_NO_FEATURE_PATTERN);

    definition = adjustFramesAndOverrides(context, definition);
    const rows = getRows(context, id, definition);
    const columns = getColumns(context, definition, rows);
    createOpenComposite(context, id, definition.frames, { "rows" : rows, "columns" : columns });
}

// ========== Adjust inputs ==========

predicate columnOverrideIsSameFramesAsPrevious(overrideIndex is number, override is map)
{
    overrideIndex != 0; // First selection cannot be same as previous
    override.sameFramesAsPrevious == true;
}

// Update definition.frames and each columnOverrides[i].overrideFrames to reflect the exact frames in question, so that
// downstream code does not need understand all the parameters in question.
function adjustFramesAndOverrides(context is Context, definition is map)
{
    // Expand and decompose base frames selection
    if (definition.selectAllFrames)
    {
        definition.frames = qFrameAllBodies();
    }
    verifyNonemptyQuery(context, definition, "frames", "Select frames to create a cutlist");
    definition.frames = decomposeNonCutlistCompositesOrderDependent(context, definition.frames);

    // Scan through manual override selections, adding them to the `definition.frames` in case these selections were
    // only made in override and not base selection.
    var explicitFrameSelections = [];
    for (var i, override in definition.columnOverrides)
    {
        if (!columnOverrideIsSameFramesAsPrevious(i, override) && !override.overrideAllFrames)
        {
            // Decompose in the same way we decompose base frame selection
            const selection = decomposeNonCutlistCompositesOrderIndependent(definition.columnOverrides[i].overrideFrames);
            definition.columnOverrides[i].overrideFrames = selection;
            explicitFrameSelections = append(explicitFrameSelections, selection);
        }
    }
    definition.frames = qUnion([definition.frames, qUnion(explicitFrameSelections)]);

    // Handle `sameFramesAsPrevious` and `overrideAllFrames`
    for (var i, override in definition.columnOverrides)
    {
        if (columnOverrideIsSameFramesAsPrevious(i, override))
        {
            definition.columnOverrides[i].overrideFrames = definition.columnOverrides[i - 1].overrideFrames;
        }
        else if (override.overrideAllFrames)
        {
            // Doing this depends on first adding all `explicitFrameSelections` to `definition.frames`
            definition.columnOverrides[i].overrideFrames = definition.frames;
        }
    }

    return definition;
}

function decomposeNonCutlistCompositesOrderDependent(context is Context, frames is Query) returns Query
{
    // For loop is needed rather than set math so that we can maintain the order of the selections while decomposing
    // specific composites in-place.
    var framesToUse = [];
    for (var frame in evaluateQuery(context, frames))
    {
        // Decompose non-cutlist composites into their constituent parts. Cutlists should remain a single line-item in
        // this cutlist.  Note that both a cutlist and some of its consituents could both be selected into the frames
        // QLV; in  this case there should be line items for both the cutlist and the frames themselves.
        if (!isQueryEmpty(context, qBodyType(frame, BodyType.COMPOSITE)) && isQueryEmpty(context, qFrameCutlist(frame)))
        {
            // If this is a composite that is not a cutlist, decompose it.
            framesToUse = append(framesToUse, qContainedInCompositeParts(frame));
        }
        else
        {
            framesToUse = append(framesToUse, frame);
        }
    }
    return qUnion(framesToUse);
}

// Same as the above function, but can be accomplished without a for loop if we do not care about the evaluation order
function decomposeNonCutlistCompositesOrderIndependent(frames is Query) returns Query
{
    const composites = qBodyType(frames, BodyType.COMPOSITE);
    const nonCutlistComposites = qSubtraction(composites, qFrameCutlist(composites));
    return qUnion([
                qSubtraction(frames, nonCutlistComposites),
                qContainedInCompositeParts(nonCutlistComposites)
            ]);
}

// ========== Gather rows ==========

function getRows(context is Context, topLevelId is Id, definition is map)
{
    const frameToRowInfo = buildUngroupedRows(context, definition);
    return groupRows(context, topLevelId, definition, frameToRowInfo);
}

// Returns a map from individual frames to their row info.  Incorporates information from both the attributes attached
// to frames, and the column overrides present in the feature.
function buildUngroupedRows(context is Context, definition is map) returns map
{
    var frameToRowInfo = {};
    const makeRowInfo = function(index is number, groupable is boolean, data is map)
        {
            return { "index" : index, "groupable" : groupable, "data" : data };
        };

    // == From attribute only ==
    for (var index, frame in evaluateQuery(context, definition.frames))
    {
        const frameProfileAttribute = try silent(getFrameProfileAttribute(context, frame));
        if (frameProfileAttribute != undefined)
        {
            const data = frameProfileAttribute as map; // Strip FrameProfileAttribute type tag for proper mapping
            frameToRowInfo[frame] = makeRowInfo(index, true, data);
            continue;
        }

        const cutlistAttribute = try silent(getCutlistAttribute(context, frame));
        if (cutlistAttribute != undefined)
        {
            frameToRowInfo[frame] = makeRowInfo(index, false, { (CUTLIST_DESCRIPTION) : CUTLIST_DESCRIPTION_CUTLIST_ENTRY });
            continue;
        }

        // non-frame, non-cutlist
        frameToRowInfo[frame] = makeRowInfo(index, true, {});
    }

    // == Incorporate overrides ==
    for (var index, override in definition.columnOverrides)
    {
        const faultyColumn = { "faultyParameters" : [faultyArrayParameterId("columnOverrides", index, "column")] };
        verify(override.column != "", ErrorStringEnum.FRAME_CUTLIST_COLUMN_EMPTY, faultyColumn);

        for (var frame in evaluateQuery(context, override.overrideFrames))
        {
            // adjustFramesAndOverrides should ensure that all frames appear in this map
            verify(frameToRowInfo[frame] != undefined, "Frame not present in frameToRowInfo");

            // If value is left blank, unset the cell(s) in question rather than setting them to the empty string.
            const valueToUse = (override.value == "") ? undefined : override.value;
            frameToRowInfo[frame].data[override.column] = valueToUse;
        }
    }

    return frameToRowInfo;
}

// frameToRowInfo comes from `buildUngroupedRows`
function groupRows(context is Context, topLevelId is Id, definition is map, frameToRowInfo is map) returns array
{
    // == Create candidate groups ==
    // reverse the map to pre-group by similar row data
    var ungroupables = [];
    var groupableRowDataToFrames = {};
    for (var frame, rowInfo in frameToRowInfo)
    {
        if (!rowInfo.groupable)
        {
            ungroupables = append(ungroupables, frame);
            continue;
        }

        if (groupableRowDataToFrames[rowInfo.data] == undefined)
        {
            groupableRowDataToFrames[rowInfo.data] = [];
        }
        groupableRowDataToFrames[rowInfo.data] = append(groupableRowDataToFrames[rowInfo.data], frame);
    }

    // == Determine group membership and sort ==
    const unorderedGroups = finalizeGroupMembership(context, ungroupables, values(groupableRowDataToFrames));
    const orderedGroups = sortGroups(frameToRowInfo, unorderedGroups);

    // == Transform into table rows ==
    var tableRows = [];
    var bodiesToDelete = new box([]);
    for (var index, group in orderedGroups)
    {
        var rowData = frameToRowInfo[group[0]].data; // Safe to use 0 because we partitioned groups by matching row data

        // = Add additional row data
        const rowIndex = index + 1;
        rowData[CUTLIST_ITEM] = (index + 1);
        rowData[CUTLIST_QTY] = size(group);
        // Length and angle data
        for (var frame in group)
        {
            // Pull length and angle data from the first beam that we find in the group, skip if there are no frames
            if (try silent(getFrameProfileAttribute(context, frame) != undefined))
            {
                const createdCurveId = topLevelId + "lengthAndAngle" + unstableIdComponent(rowIndex);
                var lengthAndAngle = getCutlistLengthAndAngles(context, topLevelId, createdCurveId, frame, bodiesToDelete);
                rowData[CUTLIST_LENGTH] = lengthAndAngle.length;
                rowData[CUTLIST_ANGLE_1] = lengthAndAngle.angle1;
                rowData[CUTLIST_ANGLE_2] = lengthAndAngle.angle2;
                break;
            }
        }

        // = Create row with entities from group
        tableRows = append(tableRows, tableRow(rowData, qUnion(group)));
    }

    // delete any temporary isoparam curves that may have been created
    cleanUpBodies(context, topLevelId + "cleanup", bodiesToDelete);
    return tableRows;
}

function finalizeGroupMembership(context is Context, ungroupables is array, candidateGroups is array) returns array
{
    var groups = [];

    // each ungroupable gets its own group
    for (var ungroupable in ungroupables)
    {
        groups = append(groups, [ungroupable]);
    }

    // take the candidate groups and group by geometry
    for (var frames in candidateGroups)
    {
        const groupsFromRowCandidate = groupFramesByGeometry(context, frames);
        for (var groupFromRowCandidate in groupsFromRowCandidate)
        {
            groups = append(groups, groupFromRowCandidate);
        }
    }

    return groups;
}

// Returns an array of arrays where each entry is an array of parts that are geometrically matched to each other.
function groupFramesByGeometry(context is Context, frames is array) returns array
{
    var groups = [];
    const clusters = @clusterBodies(context, { "bodies" : qUnion(frames), "relativeTolerance" : 0.01 });
    for (var cluster in clusters)
    {
        const groupedParts = mapArray(cluster, function(x)
            {
                return frames[x];
            });
        groups = append(groups, groupedParts);
    }
    return groups;
}

function sortGroups(frameToRowInfo is map, unorderedGroups is array) returns array
{
    var groupsWithIndexInfo = mapArray(unorderedGroups, function(group)
    {
        var smallestIndex = inf;
        for (var frame in group)
        {
            const index = frameToRowInfo[frame].index;
            smallestIndex = (index < smallestIndex) ? index : smallestIndex;
        }
        return { "group" : group, "smallestIndex" : smallestIndex };
    });

    const orderedGroupsWithIndexInfo = sort(groupsWithIndexInfo, function(groupA, groupB)
        {
            return groupA.smallestIndex - groupB.smallestIndex;
        });

    // Strip index info and return just the group itself
    return mapArray(orderedGroupsWithIndexInfo, function(groupWithIndexInfo)
        {
            return groupWithIndexInfo.group;
        });
}

function cleanUpBodies(context is Context, cleanupId is Id, bodiesToDelete is box)
{
    if (bodiesToDelete[] != [])
    {
        opDeleteBodies(context, cleanupId, {
                    "entities" : qUnion(bodiesToDelete[])
                });
    }
}

// ========== Gather columns ==========

function getColumns(context is Context, definition is map, rows is array) returns array
{
    // Item, Qty, Standard, and Description should come at the beginning of the table, if present.
    // Length and Angles should come at the end of the table, if present
    // Other custom columns should come in the middle.
    var startColumns = {
        (CUTLIST_ITEM) : { "index" : 0, "seen" : false },
        (CUTLIST_QTY) : { "index" : 1, "seen" : false },
        (CUTLIST_STANDARD) : { "index" : 2, "seen" : false },
        (CUTLIST_DESCRIPTION) : { "index" : 3, "seen" : false }
    };
    var endColumns = {
        (CUTLIST_LENGTH) : { "index" : 0, "seen" : false },
        (CUTLIST_ANGLE_1) : { "index" : 1, "seen" : false },
        (CUTLIST_ANGLE_2) : { "index" : 2, "seen" : false }
    };
    var midColumnSet = {};
    for (var row in rows)
    {
        for (var columnId, _ in row.columnIdToCell)
        {
            if (startColumns[columnId] != undefined)
            {
                startColumns[columnId].seen = true;
            }
            else if (endColumns[columnId] != undefined)
            {
                endColumns[columnId].seen = true;
            }
            else
            {
                midColumnSet[columnId] = true;
            }
        }
    }

    // Take only the seen columns and apply the proper ordering
    const getSeenColumnNamesInOrder = function(columnMap is map)
        {
            // Take only seen columns
            var unorderedColumns = [];
            for (var columnKeyValuePair in columnMap)
            {
                if (columnKeyValuePair.value.seen)
                {
                    unorderedColumns = append(unorderedColumns, columnKeyValuePair);
                }
            }
            // Order appropriately
            const orderedColumns = sort(unorderedColumns, function(columnKeyValuePairA, columnKeyValuePairB)
                {
                    return columnKeyValuePairA.value.index - columnKeyValuePairB.value.index;
                });
            // Strip down to the name
            return mapArray(orderedColumns, function(columnKeyValuePair)
                {
                    return columnKeyValuePair.key;
                });
        };

    var columns = getSeenColumnNamesInOrder(startColumns);
    // midColumns are ordered in map iteration order
    for (var midColumnName, _ in midColumnSet)
    {
        columns = append(columns, midColumnName);
    }
    columns = concatenateArrays([columns, getSeenColumnNamesInOrder(endColumns)]);

    return mapArray(columns, function(column)
        {
            return tableColumnDefinition(column, column);
        });
}

// ========== Finalize ===========

function createOpenComposite(context is Context, compositePartId is Id, frames is Query, tableData is map)
{
    opCreateCompositePart(context, compositePartId, { "bodies" : frames, "closed" : false });
    const compositePartQuery = qCreatedBy(compositePartId, EntityType.BODY)->qCompositePartTypeFilter(CompositePartType.OPEN);
    const bodyQuery = qCreatedBy(compositePartId, EntityType.BODY);
    const attribute = cutlistAttribute(compositePartId, table("Cutlist", tableData.columns, tableData.rows, compositePartQuery));
    setCutlistAttribute(context, bodyQuery, attribute);
}

// ========== Editing logic =========

/** internal */
export function cutlistEditLogic(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean) returns map
{
    for (var i, _ in definition.columnOverrides)
    {
        // For proper display control of `sameFramesAsPrevious` checkbox
        definition.columnOverrides[i].isFirst = (i == 0);
    }
    return definition;
}

// ========== Table ==========

/** @internal */
annotation { "Table Type Name" : "Cut list table" }
export const cutlistTable = defineTable(function(context is Context, definition is map) returns TableArray
    precondition
    {
    }
    {
        var tables = [];
        const composites = qFrameCutlist(qBodyType(qEverything(EntityType.BODY), BodyType.COMPOSITE));
        for (var composite in evaluateQuery(context, composites))
        {
            const attribute = getCutlistAttribute(context, composite);
            verify(attribute != undefined, "Can't find cutlist table for composite part");
            var currentTable = attribute.table;
            currentTable.entities = composite; // reassociate the table with current composite, in case of mirror or pattern
            const featureName = try silent(getFeatureName(context, attribute.featureId));
            currentTable.title = (featureName != undefined) ? featureName : "Cut List";
            tables = append(tables, currentTable);
        }
        return tableArray(tables);
    });

