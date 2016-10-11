FeatureScript 432; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * List of available UI Hints, which control how a parameter input is displayed in the feature dialog.
 *
 * @example ```
 * annotation { "Name" : "Flip", "UIHint" : "OPPOSITE_DIRECTION" }
 * definition.isFlipped is boolean;
 * ```
 *
 * Multiple `UIHint`s can be specified in an array.
 *
 * `OPPOSITE_DIRECTION` and `ALWAYS_HIDDEN` behaviors are considered stable. Other `UIHint`s can be used,
 * but their behaviors may change in future versions of Onshape.
 *
 * @value OPPOSITE_DIRECTION : For a boolean parameter, display as a toggle button with arrow next to
 *      the previous parameter.
 * @value ALWAYS_HIDDEN : Do not display this parameter or allow a user to edit it (useful for parameters
 *      changed only in editing logic or manipulator change functions).
 * @value SHOW_CREATE_SELECTION : For a query parameter, include a button to open the "Create selection"
 *      dialog.
 * @value CONTROL_VISIBILITY : For Onshape internal use.
 * @value NO_PREVIEW_PROVIDED : For a feature, hide the preview slider which controls before/after
 *      transparency, and only show the final version.
 * @value REMEMBER_PREVIOUS_VALUE : When a user makes a new instance of this feature, set this parameter's
 *      default value to the value they set the last time they used this feature.
 * @value DISPLAY_SHORT : Two consecutive boolean or quantity parameters marked as DISPLAY_SHORT may be
 *      placed on the same line.
 * @value ALLOW_FEATURE_SELECTION : For Onshape internal use.
 * @value MATE_CONNECTOR_AXIS_TYPE : For Onshape internal use.
 * @value PRIMARY_AXIS : For Onshape internal use.
 * @value SHOW_EXPRESSION : If an expression (like #foo or 1/4 in) is entered, always show the expression,
 *      and never the resulting value.
 * @value OPPOSITE_DIRECTION_CIRCULAR : Like `OPPOSITE_DIRECTION`, but with circular arrows.
 */
export enum UIHint
{
    OPPOSITE_DIRECTION,
    ALWAYS_HIDDEN,
    SHOW_CREATE_SELECTION,
    CONTROL_VISIBILITY,
    NO_PREVIEW_PROVIDED,
    REMEMBER_PREVIOUS_VALUE,
    DISPLAY_SHORT,
    ALLOW_FEATURE_SELECTION,
    MATE_CONNECTOR_AXIS_TYPE,
    PRIMARY_AXIS,
    SHOW_EXPRESSION,
    OPPOSITE_DIRECTION_CIRCULAR
}


