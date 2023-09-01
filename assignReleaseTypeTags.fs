FeatureScript 2130; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// For Onshape internal use.

import(path : "onshape/std/containers.fs", version : "2130.0");
import(path : "onshape/std/metadata.fs", version : "2130.0");
import(path : "onshape/std/releases.fs", version : "2130.0");
import(path : "onshape/std/workflow.fs", version : "2130.0");

/**
 * @internal
 * NodeStatus values listed in order, for number-to-enum conversion.
 */
const ORDERED_NODE_STATUSES = [NodeStatus.OK, NodeStatus.INFO, NodeStatus.WARNING, NodeStatus.ERROR, NodeStatus.UNKNOWN];

/**
 * @internal
 * Assigns type tags appropriate for a `ReleaseItemError`
 */
export function assignReleaseItemErrorTypeTags(value is map) returns ReleaseItemError
precondition
{
    value.severity is number;
}
{
    // Severity comes from the server as a GBTNodeStatusType ordinal; convert it to an enum using a list
    value.severity = ORDERED_NODE_STATUSES[value.severity];
    return value as ReleaseItemError;
}

/**
 * @internal
 * Assigns type tags appropriate for a `ReleasePackage`
 */
export function assignReleasePackageTypeTags(value is map) returns ReleasePackage
precondition
{
    value.items is array;
    for (var item in value.items)
    {
        item is map;
    }

    value.properties is array;
    for (var prop in value.properties)
    {
        prop is map;
    }

    value.workflow is map;
}
{
    value.items = value.items->mapArray(function(item) { return assignReleasePackageItemTypeTags(item); });
    value.properties = value.properties->mapArray(function(prop) { return assignMetadataPropertyTypeTags(prop); });

    value.workflow = assignWorkflowTypeTags(value.workflow);

    return value as ReleasePackage;
}

/**
 * @internal
 * Assigns type tags appropriate for a `ReleasePackageItem`
 */
export function assignReleasePackageItemTypeTags(value is map) returns ReleasePackageItem
precondition
{
    value.children == undefined || value.children is array;
    if (value.children is array)
    {
        for (var child in value.children)
        {
            child is map;
        }
    }

    value.properties is array;
    for (var prop in value.properties)
    {
        prop is map;
    }
}
{
    if (value.children is array)
    {
        value.children = value.children->mapArray(function(child) { return assignReleasePackageItemTypeTags(child); });
    }

    value.properties = value.properties->mapArray(function(prop) { return assignMetadataPropertyTypeTags(prop); });

    if (value.errors is array)
    {
        value.errors = value.errors->mapArray(function(err) { return assignReleaseItemErrorTypeTags(err); });
    }

    return value as ReleasePackageItem;
}

/**
 * @internal
 * Assigns type tags appropriate for a `Workflow`
 */
export function assignWorkflowTypeTags(value is map) returns Workflow
precondition
{
    value.actions is array;
    for (var action in value.actions)
    {
        action is map;
    }

    value.state is map;
}
{
    value.actions = value.actions->mapArray(function(action) { return action as WorkflowAction; });
    value.state = value.state as WorkflowState;

    return value as Workflow;
}

/**
 * @internal
 * Assigns type tags appropriate for a `WorkflowTransition`
 */
export function assignWorkflowTransitionTypeTags(value is map) returns WorkflowTransition
precondition
{
    value["type"] is string;
}
{
    value["type"] = value["type"] as TransitionType;
    return value as WorkflowTransition;
}

/**
 * @internal
 * Assigns type tags appropriate for a `MetadataProperty`
 */
export function assignMetadataPropertyTypeTags(value is map) returns MetadataProperty
precondition
{
    value.valueType is string;
}
{
    value.valueType = value.valueType as MetadataValueType;
    return value as MetadataProperty;
}

