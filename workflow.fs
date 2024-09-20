FeatureScript 2473; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/containers.fs", version : "2473.0");

/**
 * @internal
 * Runtime workflow information.  Contains the current state and any actions that are possible.
 * @type {{
 *      @field actions : List of actions (transitions) that are possible from the current state
 *      @field isDiscarded {boolean} : Whether the workflow is discarded
 *      @field isFrozen {boolean} : Whether the workflow is frozen (discarded or in a terminal state)
 *      @field isSetup {boolean} : Whether the workflow is in a setup state
 *      @field state {WorkflowState} : Current state of the workflow
 * }}
 */
export type Workflow typecheck canBeWorkflow;

/** @internal Typecheck for [Workflow] */
export predicate canBeWorkflow(value)
{
    value is map;

    value.actions is array;
    for (var action in value.actions)
    {
        canBeWorkflowAction(action);
    }

    value.isDiscarded is boolean;
    value.isFrozen is boolean;
    value.isSetup is boolean;

    canBeWorkflowState(value.state);
}

/**
 * @internal
 * An action (transition) that is possible from the current state of a workflow.
 * @type {{
 *      @field action {string} : Workflow transition name (from workflow definition)
 *      @field isApproverAction {boolean} : Whether the transition is of type APPROVE or REJECT
 *      @field label {string} : Workflow transition display name (from workflow definition)
 *      @field requiredProperties : Workflow property ids that are required in order to perform the transition
 * }}
 */
export type WorkflowAction typecheck canBeWorkflowAction;

/** @internal Typecheck for [WorkflowAction] */
export predicate canBeWorkflowAction(value)
{
    value is map;

    value.action is string;
    value.isApproverAction is boolean;
    value.label is string;

    value.requiredProperties is array;
    for (var propId in value.requiredProperties)
    {
        propId is string;
    }
}

/**
 * @internal
 * State of a workflow.
 * @type {{
 *      @field approverSourceProperty {string} : Workflow property id that contains this state's approvers
 *      @field displayName {string} : Workflow state display name (from workflow definition)
 *      @field editableProperties : Workflow property ids that are editable at this state
 *      @field name {string} : Workflow state name (from workflow definition)
 *      @field notifierSourceProperty {string} : Workflow property id that contains this state's observers
 *      @field requiredProperties : Workflow property ids that are required in order to leave this state
 *      @field requiredItemProperties : Metadata property ids that are required in order to leave this state
 * }}
 */
export type WorkflowState typecheck canBeWorkflowState;

/** @internal Typecheck for [WorkflowState] */
export predicate canBeWorkflowState(value)
{
    value is map;

    value.approverSourceProperty == undefined || value.approverSourceProperty is string;
    value.displayName is string;

    value.editableProperties is array;
    for (var propId in value.editableProperties)
    {
        propId is string;
    }

    value.name is string;
    value.notifierSourceProperty == undefined || value.notifierSourceProperty is string;

    value.requiredItemProperties is array;
    for (var propId in value.requiredItemProperties)
    {
        propId is string;
    }

    value.requiredProperties is array;
    for (var propId in value.requiredProperties)
    {
        propId is string;
    }
}

/**
 * @internal
 * Types of transitions allowed on a [custom workflow](https://cad.onshape.com/help/Content/custom_workflow.htm) definition.
 *
 * @value DEFAULT : For Onshape internal use
 * @value SUBMIT : Initial transition into a pending state
 * @value APPROVE : Occurs after approval by all required approvers
 * @value APPROVE_DIRECT : For Onshape internal use
 * @value REJECT : Occurs if any approver rejects
 * @value DELETE : For Onshape internal use
 * @value COMMENT : For Onshape internal use
 */
export enum TransitionType {
    DEFAULT,
    SUBMIT,
    APPROVE,
    APPROVE_DIRECT,
    REJECT,
    DELETE,
    COMMENT
}

/**
 * @internal
 * State of a workflow.
 * @type {{
 *      @field name {string} : Transition name (id)
 *      @field displayName {string} : Transition display name
 *      @field type {TransitionType} : Transition type (SUBMIT, APPROVE, REJECT)
 *      @field uiHint {string} : Bootstrap UI color (primary, success, danger)
 *      @field sourceState {string} : Transition source state
 *      @field targetState {string} : Transition target state
 *      @field requiredProperties : Workflow property ids that are required for this transition to occur
 *      @field requiredItemProperties : Metadata property ids that are required on all items for this transition to occur
 *      @field actions : Workflow actions to be performed as part of the transition
 * }}
 */
export type WorkflowTransition typecheck canBeWorkflowTransition;

/** @internal Typecheck for [WorkflowTransition] */
export predicate canBeWorkflowTransition(value)
{
    value is map;
    value.name is string;
    value.displayName is string;
    value["type"] is TransitionType;
    value.uiHint is string;

    value.sourceState is string;
    value.targetState is string;

    value.requiredProperties is array;
    for (var propId in value.requiredProperties)
    {
        propId is string;
    }

    value.requiredItemProperties is array;
    for (var propId in value.requiredItemProperties)
    {
        propId is string;
    }

    value.actions is array;
    for (var action in value.actions)
    {
        action.name is string;
        action.params is undefined || action.params is map;
    }
}

