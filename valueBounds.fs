FeatureScript 336; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/**
 * Value bounds are used to define minimum, maximum, and default values for
 * numbers and values with units. These bounds are in the feature dialog UI
 * for parameters with the given bounds.
 *
 * In standard usage, a parameter can be specified with e.g. angle bounds
 * in a feature precondition as follows:
 * ```
 * isAngle(definition.myAngle, ANGLE_360_BOUNDS);
 * ```
 *
 * To change the bounds and defaults on the above declaration, a user may
 * replace ANGLE_360_BOUNDS with another `AngleBoundSpec` in this module, or
 * define their own. For instance, the following code creates a parameter whose
 * default value is 45 degrees (if the user's settings have degrees as the
 * default) or 1 radian (if the user's settings have radians as the default).
 * ```
 * const MY_BOUNDS =
 * {
 *     "min"    : -TOLERANCE.zeroAngle * radian,
 *     "max"    : (2 * PI + TOLERANCE.zeroAngle) * radian,
 *     (degree) : [0, 45, 360],
 *     (radian) : 1
 * } as AngleBoundSpec;
 *
 * ...
 *     isAngle(definition.myAngle, MY_BOUNDS);
 * ...
 * ```
 */

import(path : "onshape/std/containers.fs", version : "336.0");
import(path : "onshape/std/math.fs", version : "336.0");
export import(path : "onshape/std/units.fs", version : "336.0");

/** @internal */
export predicate defineBounds(value, boundSpec is map)
precondition
{
    canBeBoundSpec(boundSpec);
}
{
    value >= boundSpec.min;
    value <= boundSpec.max;
}

/**
 * True for a value with length units which conforms to the given bounds.
 *
 * Used in feature preconditions to specify a length parameter.
 *
 * @param boundSpec {LengthBoundSpec} : Specifies a min, a max, and a default
 *      value. These values are possibly different in different units for the
 *      sake of round numbers.
 *
 *      To specify a parameter with different default value or different
 *      limits, use a different or custom `LengthBoundSpec`.
 */
export predicate isLength(value, boundSpec is LengthBoundSpec)
{
    isLength(value);
    defineBounds(value, boundSpec);
}

/**
 * True for a value with angle units which conforms to the given bounds.
 *
 * Used in feature preconditions to specify an angle parameter.
 *
 * @param boundSpec {AngleBoundSpec} : Specifies a min, a max, and a default
 *      value. These values are possibly different in different units for the
 *      sake of round numbers.
 *
 *      To specify a parameter with different default value or different
 *      limits, use a different or custom `AngleBoundSpec`.
 */
export predicate isAngle(value, boundSpec is AngleBoundSpec)
{
    isAngle(value);
    defineBounds(value, boundSpec);
}

/**
 * True for a `number` that is an integer and conforms to the given bounds.
 *
 * Used in feature preconditions to specify an integer or count parameter.
 *
 * @param boundSpec {IntegerBoundSpec} : Specifies a min, a max, and a default
 *      value. These values are possibly different in different units for the
 *      sake of round numbers.
 *
 *      To specify a parameter with different default value or different
 *      limits, use a different or custom `IntegerBoundSpec`.
 */
export predicate isInteger(value, boundSpec is IntegerBoundSpec)
{
    isInteger(value);
    defineBounds(value, boundSpec);
}

/**
 * True for a real number which conforms to the given bounds.
 *
 * Used in feature preconditions to specify a unitless numeric parameter.
 *
 * @param boundSpec {RealBoundSpec} : Specifies a min, a max, and a default
 *      value. These values are possibly different in different units for the
 *      sake of round numbers.
 *
 *      To specify a parameter with different default value or different
 *      limits, use a different or custom `RealBoundSpec`.
 */
export predicate isReal(value, boundSpec is RealBoundSpec)
{
    value is number;
    defineBounds(value, boundSpec);
}

/**
 * A `LengthBoundSpec` for a positive or negative length.
 */
export const LENGTH_BOUNDS =
{
    "min"        : -500 * meter,
    "max"        : 500 * meter,
    (meter)      : [1e-5, 0.025, 500],
    (centimeter) : 2.5,
    (millimeter) : 25.0,
    (inch)       : 1.0,
    (foot)       : 0.1,
    (yard)       : 0.025
} as LengthBoundSpec;

/**
 * A `LengthBoundSpec` for a length strictly greater than 0.
 */
export const NONNEGATIVE_LENGTH_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [1e-5, 0.025, 500],
    (centimeter) : 2.5,
    (millimeter) : 25.0,
    (inch)       : 1.0,
    (foot)       : 0.1,
    (yard)       : 0.025
} as LengthBoundSpec;

/**
 * A `LengthBoundSpec` for a length greater than or equal to 0.
 */
export const NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0, 0.025, 500],
    (centimeter) : 2.5,
    (millimeter) : 25.0,
    (inch)       : 1.0,
    (foot)       : 0.1,
    (yard)       : 0.025
} as LengthBoundSpec;

/**
 * A `LengthBoundSpec` for a length greater than or equal to 0, with UI defaults of 0.0 for all units.
 */
export const NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0, 0.0, 500],
    (centimeter) : 0.0,
    (millimeter) : 0.0,
    (inch)       : 0.0,
    (foot)       : 0.0,
    (yard)       : 0.0
} as LengthBoundSpec;

/**
 * A `LengthBoundSpec` for a positive or negative length, with UI defaults of 0.0 for all units.
 */
export const ZERO_DEFAULT_LENGTH_BOUNDS =
{
    "min"        : -500 * meter,
    "max"        : 500 * meter,
    (meter)      : [-500, 0.0, 500],
    (centimeter) : 0.0,
    (millimeter) : 0.0,
    (inch)       : 0.0,
    (foot)       : 0.0,
    (yard)       : 0.0
} as LengthBoundSpec;

/**
 * A `LengthBoundSpec` for fillets and chamfers, with smaller defaults than `NONNEGATIVE_LENGTH_BOUNDS`
 * (`0.2 * inch`, etc.).
 */
export const BLEND_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [1e-5, 0.005, 500],
    (centimeter) : 0.5,
    (millimeter) : 5.0,
    (inch)       : 0.2,
    (foot)       : 0.015,
    (yard)       : 0.005
} as LengthBoundSpec;

/**
 * A `LengthBoundSpec` for a shell or offset thickness, with smaller defaults than `NONNEGATIVE_LENGTH_BOUNDS`.
 * (`0.1 * inch`, etc.).
 */
export const SHELL_OFFSET_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [1e-5, 0.0025, 500],
    (centimeter) : 0.25,
    (millimeter) : 2.5,
    (inch)       : 0.1,
    (foot)       : 0.01,
    (yard)       : 0.0025
} as LengthBoundSpec;

/**
 * @internal
 * A `LengthBoundSpec` for the size of a construction plane.
 */
export const PLANE_SIZE_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [1e-5, 0.15, 500],
    (centimeter) : 15,
    (millimeter) : 150,
    (inch)       : 6,
    (foot)       : 0.5,
    (yard)       : 0.166667
} as LengthBoundSpec;

/**
 * An `AngleBoundSpec` for an angle between 0 and 360 degrees, defaulting to 30 degrees.
 */
export const ANGLE_360_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (2 * PI + TOLERANCE.zeroAngle) * radian,
    (degree) : [0, 30, 360],
    (radian) : 1
} as AngleBoundSpec;

/**
 * An `AngleBoundSpec` for an angle between 0 and 360 degrees, defaulting to 330 degrees.
 */
export const ANGLE_360_REVERSE_DEFAULT_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (2 * PI + TOLERANCE.zeroAngle) * radian,
    (degree) : [0, 330, 360],
    (radian) : 2
} as AngleBoundSpec;

/**
 * An `AngleBoundSpec` for an angle between 0 and 360 degrees, defaulting to 0 degrees.
 */
export const ANGLE_360_ZERO_DEFAULT_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (2 * PI + TOLERANCE.zeroAngle) * radian,
    (degree) : [0, 0, 360],
    (radian) : 0
} as AngleBoundSpec;

/**
 * An `AngleBoundSpec` for an angle strictly less than 180 degrees.
 */
export const ANGLE_STRICT_180_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (PI - TOLERANCE.zeroAngle) * radian,
    (degree) : [0, 30, 179.9],
    (radian) : 0.1667 * PI
} as AngleBoundSpec;

/**
 * An `AngleBoundSpec` for an angle strictly less than 90 degrees.
 */
export const ANGLE_STRICT_90_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (PI * 0.5 - TOLERANCE.zeroAngle) * radian,
    (degree) : [0, 3, 89.9],
    (radian) : 0.01667 * PI
} as AngleBoundSpec;

/**
 * An `IntegerBoundSpec` for an integer strictly greater than zero, defaulting to 2.
 */
export const POSITIVE_COUNT_BOUNDS =
{
    "min"      : 1,
    "max"      : 1e9,
    (unitless) : [1, 2, 1e5]
} as IntegerBoundSpec;

/**
 * A `RealBoundSpec` for a number greater than or equal to zero, defaulting to 1.
 */
export const POSITIVE_REAL_BOUNDS =
{
    "min"      : 0,
    "max"      : 1e9,
    (unitless) : [0, 1, 1e5]
} as RealBoundSpec;

/**
 * A `RealBoundSpec` for the positive or negative scale factor on a transform, defaulting to `1`.
 */
export const SCALE_BOUNDS =
{
    "min"      : 0,
    "max"      : 1e9,
    (unitless) : [1e-5, 1, 1e5]
} as RealBoundSpec;

/**
 * @internal
 * Count bounds for a circular pattern, and the primary direction of linear patterns
 */
export const PRIMARY_PATTERN_BOUNDS =
{
    "min"      : 1,
    "max"      : 2500,
    (unitless) : [1, 2, 2500]
} as IntegerBoundSpec;

/**
 * @internal
 * Bounds for the secondary direction of a linear patterns
 */
export const SECONDARY_PATTERN_BOUNDS =
{
    "min"      : 1,
    "max"      : 2500,
    (unitless) : [1, 1, 2500]
} as IntegerBoundSpec;

/**
 * @internal
 * Return the intersection of all bounds in a BoundSpec as an
 * array with the first element being the lower bound, and the second element being the upper bound.
 * For example, if a BoundSpec allows 1/32 inch to 1 yard for Imperial
 * units and 1 mm to 1 meter for metric, the result is `[1 * mm, 1 * yard]`.
 */
export function tightestBounds(boundSpec is map) returns array
precondition
{
    canBeBoundSpec(boundSpec);
}
{
    var bounds = makeArray(2);
    bounds[0] = boundSpec["min"];
    bounds[1] = boundSpec["max"];
    for (var entry in boundSpec)
    {
        if (entry.key != "min" && entry.key != "max" && entry.value is array)
        {
            bounds[0] = max(bounds[0], entry.value[0] * entry.key);
            bounds[1] = min(bounds[1], entry.value[2] * entry.key);
        }
    }
    return bounds;
}

/** @internal */
export predicate canBeBoundSpec(value)
{
    value is map;
    if (!(value.min is number && value.max is number))
    {
        value.min is ValueWithUnits;
        value.max is ValueWithUnits;
        value.min.unit == value.max.unit;
    }
    value.min <= value.max;
    for (var entry in value)
    {
        if (entry.key != "min" && entry.key != "max")
        {
            if (entry.value is number)
                continue;
            entry.value is array;
            @size(entry.value) == 3;
            for (var i = 0; i < 3; i += 1)
            {
                entry.value[i] is number;
                if (i > 0)
                    entry.value[i - 1] <= entry.value[i];
            }
        }
    }
}

/**
 * A spec to be used with the `isLength` predicate to define allowable lengths
 * and customize UI behaviors for feature dialog parameters that take in a length.
 *
 * A typical declaration looks like:
 * ```
 * const MY_LENGTH_BOUNDS =
 * {
 *     "min"        : -500 * meter,
 *     "max"        : 500 * meter,
 *     (meter)      : [-400, 0.0025, 400],
 *     (centimeter) : .25,
 *     (millimeter) : 2.50,
 *     (inch)       : 0.1,
 *     (foot)       : 0.01,
 *     (yard)       : 0.0025
 * } as LengthBoundSpec;
 * ```
 *
 * The "min" and "max" values refer to bounds checked in the FeatureScript
 * precondition. Thus, `isLength(1000 * meter, MY_LENGTH_BOUNDS)` returns `false`.
 *
 * The values for `(meter)`, `(inch)`, etc. define UI behaviors for a feature
 * parameter defined with `MY_LENGTH_BOUNDS`. These behaviors will be different
 * for users who have set different default units.
 *
 * Specifically, the center value of the array defined for a unit (or the only
 * value, if only one is provided) defines the default value, when a user with
 * those default units first opens the dialog.
 *
 * The end two values define the UI minimum and maximum. A user who types a
 * value of 1000 meters will see that value change to 400 meters. A user who
 * types a value of -1000 meters will see that values change to -400 meters.
 * If no min and max values are provided for some units, the values from
 * another unit will be used.
 */
export type LengthBoundSpec typecheck canBeLengthBoundSpec;

/** Typecheck for LengthBoundSpec */
export predicate canBeLengthBoundSpec(value)
{
    canBeBoundSpec(value);
    isLength(value.min);
    for (var entry in value)
        entry.key == "min" || entry.key == "max" || isLength(entry.key);
}

/**
 * A spec to be used with the `isAngle` predicate to define allowable angles
 * and customize UI behaviors for feature dialog parameters that take in an angle.
 *
 * A typical declaration looks like:
 * ```
 * const ANGLE_360_BOUNDS =
 * {
 *     "min"    : -TOLERANCE.zeroAngle * radian,
 *     "max"    : (2 * PI + TOLERANCE.zeroAngle) * radian,
 *     (degree) : [0, 30, 360],
 *     (radian) : 1
 * } as AngleBoundSpec;
 * ```
 *
 * For more information on what the various fields signify, see
 * `LengthBoundSpec`.
 */
export type AngleBoundSpec typecheck canBeAngleBoundSpec;

/** Typecheck for AngleBoundSpec */
export predicate canBeAngleBoundSpec(value)
{
    canBeBoundSpec(value);
    isAngle(value.min);
    for (var entry in value)
        entry.key == "min" || entry.key == "max" || isAngle(entry.key);
}

/**
 * A spec to be used with the `isInteger` predicate to define allowable numbers
 * and customize UI behaviors for feature dialog parameters that take in a number.
 *
 * A typical declaration looks like:
 * ```
 * const POSITIVE_COUNT_BOUNDS =
 * {
 *     "min"      : 1,
 *     "max"      : 1e9,
 *     (unitless) : [1, 2, 1e5]
 * } as IntegerBoundSpec;
 * ```
 *
 * For more information on what the various fields signify, see
 * `LengthBoundSpec`.
 */
export type IntegerBoundSpec typecheck canBeIntegerBoundSpec;

/** Typecheck for IntegerBoundSpec */
export predicate canBeIntegerBoundSpec(value)
{
    canBeBoundSpec(value);
    isInteger(value.min);
    isInteger(value.max);
    @size(value) == 3;
    value[unitless] is array;
}

/**
 * A spec to be used with the `isReal` predicate to define allowable real numbers
 * and customize UI behaviors for feature dialog parameters that take in a real number.
 *
 * A typical declaration looks like:
 * ```
 * const POSITIVE_REAL_BOUNDS =
 * {
 *     "min"      : 0,
 *     "max"      : 1e9,
 *     (unitless) : [0, 1, 1e5]
 * } as RealBoundSpec;
 * ```
 *
 * For more information on what the various fields signify, see
 * `LengthBoundSpec`.
 */
export type RealBoundSpec typecheck canBeRealBoundSpec;

/** Typecheck for RealBoundSpec */
export predicate canBeRealBoundSpec(value)
{
    canBeBoundSpec(value);
    value.min is number;
    value.max is number;
    @size(value) == 3;
    value[unitless] is array;
}

