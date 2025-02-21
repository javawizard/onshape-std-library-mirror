FeatureScript 2599; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/containers.fs", version : "2599.0");
import(path : "onshape/std/metadata.fs", version : "2599.0");
import(path : "onshape/std/workflow.fs", version : "2599.0");

/**
 * @internal
 * Represents the status of an object or the severity of a message.
 */
export enum NodeStatus {
    annotation { "Name" : "OK" }
    OK,
    annotation { "Name" : "Info" }
    INFO,
    annotation { "Name" : "Warning" }
    WARNING,
    annotation { "Name" : "Error" }
    ERROR,
    annotation { "Name" : "Unknown" }
    UNKNOWN
}

/**
 * @internal
 * An item-level message on a Release candidate, indicating a potential problem or notification on the item.
 * @type {{
 *      @field ordinal {number} : Message code
 *      @field severity {NodeStatus} : Message type (INFO, WARNING, ERROR)
 *      @field message {string} : Message string
 * }}
 */
export type ReleaseItemError typecheck canBeReleaseItemError;

/** @internal Typecheck for [ReleaseItemError] */
export predicate canBeReleaseItemError(value)
{
    value is map;
    value.ordinal is number;
    value.severity is NodeStatus;
    value.message is string;
}

/**
 * @internal
 * A Release candidate, including all its properties and items.
 * @type {{
 *      @field comments {array} : Comments on the release
 *      @field companyId {string} : Release's company id
 *      @field description {string} : Release description
 *      @field documentId {string} : Release's document id
 *      @field href {string} : URL to view the release
 *      @field id {string} : Release package id
 *      @field isObsoletion {boolean} : Whether the release is an obsoletion package
 *      @field items {array} : Top-level release items
 *      @field linkedVersionIds {array} : Version ids from linked documents represented in the release
 *      @field name {string} : Release name
 *      @field packageThumbnail {string} : URL for the release thumbnail
 *      @field parentComments {array} : Comments on releases that this release clones
 *      @field properties {array} : Release workflow properties
 *      @field revisionRuleId {string} : Revision rule id used by release items
 *      @field versionId {string} : Release's version id
 *      @field workspace {string} : Release's workspace id
 *      @field workflow {Workflow} : Current state of the release's workflow
 *      @field workflowId : Workflow used by the release
 * }}
 */
export type ReleasePackage typecheck canBeReleasePackage;

/** @internal Typecheck for [ReleasePackage] */
export predicate canBeReleasePackage(value)
{
    value is map;

    value.comments == undefined || value.comments is array;

    value.companyId is string;
    value.description is string;
    value.documentId is string;
    value.href is string;
    value.id is string;
    value.isObsoletion is boolean;

    value.items is array;
    for (var item in value.items)
    {
        canBeReleasePackageItem(item);
    }

    value.linkedVersionIds is array;
    for (var versionId in value.linkedVersionIds)
    {
        versionId is string;
    }

    value.name is string;
    value.packageThumbnail is string;

    value.parentComments == undefined || value.parentComments is array;

    value.properties is array;
    for (var prop in value.properties)
    {
        canBeMetadataProperty(prop);
        prop.isApproverProperty is boolean;
        prop.isNotifierProperty is boolean;
    }

    value.revisionRuleId is string;
    value.versionId == undefined || value.versionId is string;
    value.workspaceId == undefined || value.workspaceId is string;

    canBeWorkflow(value.workflow);

    value.workflowId is map;
    value.workflowId.companyId is string;
    value.workflowId.versionId is string;
    value.workflowId.workflowId is string;
}

/**
 * @internal
 * An item on a Release candidate.  May have child items.
 * @type {{
 *      @field children : Child release items
 *      @field companyId {string} : Release's company id
 *      @field configuration {string} : Configuration of the item in the form a=b;c=d
 *      @field configurationKey {string} : Configuration of the item, base64 encoded
 *      @field documentId {string} : Item's document id
 *      @field elementId {string} : Item's element id
 *      @field elementType {string} : Item's element type
 *      @field errors {array} : Any errors detected on the item
 *      @field id {string} : Item id
 *      @field isRevisionManaged {boolean} : Whether the item is revision managed according to the "Not revision managed" metadata property on itself, its element, or its document
 *      @field isRootItem {boolean} : Whether the item is a top-level release item, i.e. has no parent
 *      @field isTranslatable {boolean} : Whether the item can be translated/exported into other file formats
 *      @field mimeType {string} : Data type of the item
 *      @field name {string} : Element or part name of the item
 *      @field obsoletionRevisionId {string} : On an obsoletion package, the revision id being obsoleted
 *      @field partType {string} : For a part, the part type (solid, sheet, composite, etc)
 *      @field properties {array} : Metadata properties of the item
 *      @field rpid {string} : Release package id
 *      @field smallThumbnailHref {string} : URL of the item's thumbnail
 *      @field versionId {string} : Item's version id
 *      @field workspaceId {string} : Item's workspace id
 *      @field viewRef {string} : URL to view the item in its Onshape document
 * }}
 */
export type ReleasePackageItem typecheck canBeReleasePackageItem;

/** @internal Typecheck for [ReleasePackageItem] */
export predicate canBeReleasePackageItem(value)
{
    value is map;

    value.children == undefined || value.children is array;
    if (value.children is array)
    {
        for (var child in value.children)
        {
            canBeReleasePackageItem(child);
        }
    }

    value.companyId is string;
    value.configuration == undefined || value.configuration is string;
    value.configurationKey == undefined || value.configurationKey is string;
    value.documentId is string;
    value.elementId is string;
    value.elementType is number;

    value.errors is array;
    for (var error in value.errors)
    {
        canBeReleaseItemError(error);
    }

    value.id is string;
    value.isRevisionManaged is boolean;
    value.isRootItem is boolean;
    value.isTranslatable is boolean;
    value.mimeType is string;
    value.name is string;
    value.obsoletionRevisionId == undefined || value.obsoletionRevisionId is string;
    value.partType is string;

    value.properties is array;
    for (var prop in value.properties)
    {
        canBeMetadataProperty(prop);
    }

    value.rpid is string;
    value.smallThumbnailHref is string;
    value.versionId == undefined || value.versionId is string;
    value.workspaceId == undefined || value.workspaceId is string;
    value.viewRef is string;
}

/**
 * @internal
 * Returns a flattened list of all `ReleasePackageItem`s in a Release candidate
 */
export function getAllPackageItems(rp is ReleasePackage) returns array
precondition
{
    canBeReleasePackage(rp);
}
{
    if (rp.items == undefined)
    {
        return [];
    }

    const itemizedChildren = rp.items->mapArray(item => getAllChildren(item as ReleasePackageItem));
    return itemizedChildren->concatenateArrays();
}

/**
 * @internal
 * Gathers all children of a Release item into a flat list
 */
export function getAllChildren(rpItem is ReleasePackageItem) returns array
precondition
{
    canBeReleasePackageItem(rpItem);
}
{
    if (rpItem.children == undefined)
    {
        return [rpItem];
    }

    const itemizedChildren = rpItem.children->mapArray(child => getAllChildren(child as ReleasePackageItem));
    const flatChildren = itemizedChildren->concatenateArrays();

    return concatenateArrays([[rpItem], flatChildren]);
}

/**
 * @internal
 * Returns a metadata property of a Release candidate or item by its property id.
 */
export function getPropertyById(rpOrItem is map, propertyId is string)
precondition
{
    canBeReleasePackage(rpOrItem) || canBeReleasePackageItem(rpOrItem);
}
{
    if (rpOrItem.properties == undefined)
    {
        return undefined;
    }

    for (var prop in rpOrItem.properties)
    {
        if (prop.propertyId == propertyId)
        {
            return prop as MetadataProperty;
        }
    }

    return undefined;
}

