FeatureScript 293; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/math.fs", version : "");
export import(path : "onshape/std/units.fs", version : "");

export predicate defineBounds(value, boundSpec is map)
precondition
{
    canBeBoundSpec(boundSpec);
}
{
    value >= boundSpec.min;
    value <= boundSpec.max;
}

export predicate isLength(value, boundSpec is LengthBoundSpec)
{
    isLength(value);
    defineBounds(value, boundSpec);
}

export predicate isAngle(value, boundSpec is AngleBoundSpec)
{
    isAngle(value);
    defineBounds(value, boundSpec);
}

/* Overloads isInteger in math.fs */
export predicate isInteger(value, boundSpec is IntegerBoundSpec)
{
    isInteger(value);
    defineBounds(value, boundSpec);
}

export predicate isReal(value, boundSpec is RealBoundSpec)
{
    value is number;
    defineBounds(value, boundSpec);
}

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

export const PLANE_OFFSET_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0, 0.15, 500],
    (centimeter) : 15,
    (millimeter) : 150,
    (inch)       : 6,
    (foot)       : 0.5,
    (yard)       : 0.166667
} as LengthBoundSpec;

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

export const THICKEN_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0, 0.005, 500],
    (centimeter) : 0.5,
    (millimeter) : 5.0,
    (inch)       : 0.25,
    (foot)       : 0.025,
    (yard)       : 0.01
} as LengthBoundSpec;

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

export const ANGLE_360_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (2 * PI + TOLERANCE.zeroAngle) * radian,
    (degree) : [0, 30, 360],
    (radian) : 1
} as AngleBoundSpec;

export const ANGLE_360_REVERSE_DEFAULT_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (2 * PI + TOLERANCE.zeroAngle) * radian,
    (degree) : [0, 330, 360],
    (radian) : 2
} as AngleBoundSpec;

export const ANGLE_360_ZERO_DEFAULT_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (2 * PI + TOLERANCE.zeroAngle) * radian,
    (degree) : [0, 0, 360],
    (radian) : 0
} as AngleBoundSpec;

export const ANGLE_STRICT_180_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (PI - TOLERANCE.zeroAngle) * radian,
    (degree) : [0, 30, 179.9],
    (radian) : 0.1667 * PI
} as AngleBoundSpec;

export const ANGLE_STRICT_90_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (PI * 0.5 - TOLERANCE.zeroAngle) * radian,
    (degree) : [0, 3, 89.9],
    (radian) : 0.01667 * PI
} as AngleBoundSpec;

export const CHAMFER_ANGLE_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (PI - TOLERANCE.zeroAngle) * radian,
    (degree) : [0.1, 45, 179.9],
    (radian) : 0.25 * PI
} as AngleBoundSpec;

export const POSITIVE_COUNT_BOUNDS =
{
    "min"      : 1,
    "max"      : 1e9,
    (unitless) : [1, 2, 1e5]
} as IntegerBoundSpec;

export const POSITIVE_REAL_BOUNDS =
{
    "min"      : 0,
    "max"      : 1e9,
    (unitless) : [0, 1, 1e5]
} as RealBoundSpec;

export const FILLET_RHO_BOUNDS =
{
    "min"      : 0.0,
    "max"      : 1.0,
    (unitless) : [0.0, 0.5, 0.99999]
} as RealBoundSpec;

export const SCALE_BOUNDS =
{
    "min"      : 0,
    "max"      : 1e9,
    (unitless) : [1e-5, 1, 1e5]
} as RealBoundSpec;

export const HELIX_TURN_BOUNDS =
{
    "min"      : 0,
    "max"      : 1e9,
    (unitless) : [1e-5, 4, 1e5]
} as RealBoundSpec;

export const PRIMARY_PATTERN_BOUNDS =
{
    "min"      : 1,
    "max"      : 2500,
    (unitless) : [1, 2, 2500]
} as IntegerBoundSpec;

export const SECONDARY_PATTERN_BOUNDS =
{
    "min"      : 1,
    "max"      : 2500,
    (unitless) : [1, 1, 2500]
} as IntegerBoundSpec;

/**
 * Return the intersection of all bounds in a BoundSpec as an
 * array with the first element being the lower bound, and the second element being the upper bound.
 * For example, if a BoundSpec allows 1/32 inch to 1 yard for Imperial
 * units and and 1 mm to 1 meter for metric, the result is `[1 * mm, 1 * yard]`.
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

//Type checking follows

/**
 * TODO: description
 */
export type LengthBoundSpec typecheck canBeLengthBoundSpec;
/**
 * TODO: description
 */
export type AngleBoundSpec typecheck canBeAngleBoundSpec;
/**
 * TODO: description
 */
export type IntegerBoundSpec typecheck canBeIntegerBoundSpec;
/**
 * TODO: description
 */
export type RealBoundSpec typecheck canBeRealBoundSpec;

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

export predicate canBeLengthBoundSpec(value)
{
    canBeBoundSpec(value);
    isLength(value.min);
    for (var entry in value)
        entry.key == "min" || entry.key == "max" || isLength(entry.key);
}

export predicate canBeAngleBoundSpec(value)
{
    canBeBoundSpec(value);
    isAngle(value.min);
    for (var entry in value)
        entry.key == "min" || entry.key == "max" || isAngle(entry.key);
}

export predicate canBeIntegerBoundSpec(value)
{
    canBeBoundSpec(value);
    isInteger(value.min);
    isInteger(value.max);
    @size(value) == 3;
    value[unitless] is array;
}

export predicate canBeRealBoundSpec(value)
{
    canBeBoundSpec(value);
    value.min is number;
    value.max is number;
    @size(value) == 3;
    value[unitless] is array;
}

