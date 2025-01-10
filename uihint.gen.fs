FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * List of available UI Hints, which control how a parameter input is displayed in the feature dialog.
 *
 * @example ```
 * annotation { "Name" : "Flip", "UIHint" : UIHint.OPPOSITE_DIRECTION }
 * definition.isFlipped is boolean;
 * ```
 *
 * Multiple `UIHint`s can be specified in an array
 * (e.g. `[ UIHint.OPPOSITE_DIRECTION, UIHint.REMEMBER_PREVIOUS_VALUE ]`).
 *
 * Raw strings (e.g. "OPPOSITE_DIRECTION") may be used in place of the enum access (e.g. UIHint.OPPOSITE_DIRECTION)
 * for the same result.
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
 * @value SHOW_LABEL : Show a label above an enum parameter.
 * @value HORIZONTAL_ENUM : Display an enum as a horizontal list of clickable text, rather than the default select control
 * @value UNCONFIGURABLE : For Onshape internal use.
 * @value MATCH_LAST_ARRAY_ITEM : Inside an array parameter, set the default value on a new item to match
 *      what is set on the last item.
 * @value COLLAPSE_ARRAY_ITEMS : For an array parameter, create new items (and items in a newly opened
 *      dialog) as collapsed by default.
 * @value INITIAL_FOCUS_ON_EDIT : When an existing feature is edited, the first visible parameter with this UI hint will get focus.
 * @value INITIAL_FOCUS : When creating or editing, the first visible parameter with this UI hint will get focus
 *      (but when editing, a parameter with INITIAL_FOCUS_ON_EDIT takes precedence).
 * @value DISPLAY_CURRENT_VALUE_ONLY : For Onshape internal use.
 * @value READ_ONLY : Prevent changes to the parameter from the feature dialog.  A read-only parameter can be modified by the editing logic function.
 * @value PREVENT_CREATING_NEW_MATE_CONNECTORS : For a query parameter allowing BodyType.MATE_CONNECTOR, only allow preexisting mate connectors,
 *      and don't provide a button to allow creating new mate connectors specificaly for this parameter.
 * @value FIRST_IN_ROW : Guarantee that, regardless of other layout requirements, this parameter will always be the first parameter in
 *      its displayed row.
 * @value ALLOW_QUERY_ORDER : Enable reordering of queries in a query parameter.
 * @value PREVENT_ARRAY_REORDER : Disable reordering of items in an array parameter.
 * @value FOCUS_INNER_QUERY : When adding a new item to an array parameter, focus the driving inner QLV if the parameter is         selection-driven, and focus the first inner QLV otherwise.
 * @internal @value VARIABLE_NAME : Indicates that parameter is used as variable name.
 * @value SHOW_TOLERANCE : For a boolean parameter, display as a toggle button with tolerance icon next to
 *      the previous parameter.
 * @value ALLOW_ARRAY_FOCUS : Allow focusing an array parameter with no driving or inner QLV, as if it were selection-driven.
 * @value SHOW_INLINE_CONFIG_INPUTS : Inline the configuration parameters in the configure dialog for the feature.
 * @value FOCUS_ON_VISIBLE : For a query parameter, selects it when it becomes visible during editing.
 * @value CAN_BE_TOLERANT : For Onshape internal use.
 * @value CAN_BE_TOLERANT_DIAMETER : For Onshape internal use.
 * @value PLUS_MINUS : Like `OPPOSITE_DIRECTION` but indicating whether the associated quantity is positive or negative.
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
    OPPOSITE_DIRECTION_CIRCULAR,
    SHOW_LABEL,
    HORIZONTAL_ENUM,
    UNCONFIGURABLE,
    MATCH_LAST_ARRAY_ITEM,
    COLLAPSE_ARRAY_ITEMS,
    INITIAL_FOCUS_ON_EDIT,
    INITIAL_FOCUS,
    DISPLAY_CURRENT_VALUE_ONLY,
    READ_ONLY,
    PREVENT_CREATING_NEW_MATE_CONNECTORS,
    FIRST_IN_ROW,
    ALLOW_QUERY_ORDER,
    PREVENT_ARRAY_REORDER,
    VARIABLE_NAME,
    FOCUS_INNER_QUERY,
    SHOW_TOLERANCE,
    ALLOW_ARRAY_FOCUS,
    SHOW_INLINE_CONFIG_INPUTS,
    FOCUS_ON_VISIBLE,
    CAN_BE_TOLERANT,
    CAN_BE_TOLERANT_DIAMETER,
    PLUS_MINUS
}


