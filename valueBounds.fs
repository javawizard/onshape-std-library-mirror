export import(path : "onshape/std/units.fs", version : "");
export import(path : "onshape/std/utils.fs", version : "");

export predicate defineUIDefaultValue(value, defaultValue)
{
    //Do nothing -- for UI only
}

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

export predicate isInteger(value, boundSpec is IntegerBoundSpec)
{
    isInteger(value);
    defineBounds(value, boundSpec);
}

export predicate isReal(value, boundSpec is RealBoundSpec)
{
    isUnitless(value);
    defineBounds(value, boundSpec);
}

export const PLANE_SIZE_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0001, 0.15, 250],
    (millimeter) : [0.1,    150,  250000],
    (centimeter) : [0.01,   15,   25000],
    (inch)       : [0.001,  6,    10000],
    (foot)       : [0.0001, 0.5,    1000],
    (yard)       : [0.0001, 0.166667, 250]
} as LengthBoundSpec;

export const PLANE_OFFSET_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0, 0.15, 250],
    (millimeter) : [0.0, 150,  250000],
    (centimeter) : [0.0, 15,   25000],
    (inch)       : [0.0, 6,    10000],
    (foot)       : [0.0, 0.5,    1000],
    (yard)       : [0.0, 0.166667, 250]
} as LengthBoundSpec;

export const BLEND_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0001, 0.005, 250],
    (millimeter) : [0.1,    5.0,   250000],
    (centimeter) : [0.01,   0.5,   25000],
    (inch)       : [0.001,  0.2,   1000],
    (foot)       : [0.0001, 0.015, 1000],
    (yard)       : [0.0001, 0.005, 250]
} as LengthBoundSpec;

export const SHELL_OFFSET_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0001, 0.0025, 250],
    (millimeter) : [0.1,    2.5,    250000],
    (centimeter) : [0.01,   0.25,   25000 ],
    (inch)       : [0.001,  0.1,    1000],
    (foot)       : [0.0001, 0.01,   1000],
    (yard)       : [0.0001, 0.0025, 250]
} as LengthBoundSpec;

export const NONNEGATIVE_LENGTH_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0001, 0.025, 250],
    (millimeter) : [0.1,    25.0,  250000],
    (centimeter) : [0.01,   2.5,   25000],
    (inch)       : [0.001,  1.0,   1000],
    (foot)       : [0.001,  0.1,   1000],
    (yard)       : [0.0001, 0.025, 250]
} as LengthBoundSpec;

export const NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0, 0.025, 250],
    (millimeter) : [0.0,    25.0,  250000],
    (centimeter) : [0.0,   2.5,   25000],
    (inch)       : [0.0,  1.0,   1000],
    (foot)       : [0.0,  0.1,   1000],
    (yard)       : [0.0, 0.025, 250]
} as LengthBoundSpec;

export const NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS =
{
    "min"        : -TOLERANCE.zeroLength * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0, 0.0, 250],
    (millimeter) : [0.0, 0.0,  250000],
    (centimeter) : [0.0, 0.0,   25000],
    (inch)       : [0.0, 0.0,   1000],
    (foot)       : [0.0, 0.0,   1000],
    (yard)       : [0.0, 0.00, 250]
} as LengthBoundSpec;

export const LENGTH_BOUNDS =
{
    "min"        : -500 * meter,
    "max"        : 500 * meter,
    (meter)      : [0.0001, 0.025, 250],
    (millimeter) : [0.1,    25.0,  250000],
    (centimeter) : [0.01,   2.5,   25000],
    (inch)       : [0.001,  1.0,   1000],
    (foot)       : [0.001,  0.1,   1000],
    (yard)       : [0.0001, 0.025, 250]
} as LengthBoundSpec;

export const ZERO_DEFAULT_LENGTH_BOUNDS =
{
    "min"        : -500 * meter,
    "max"        : 500 * meter,
    (meter)      : [-250, 0.0, 250],
    (millimeter) : [-250000, 0.0,  250000],
    (centimeter) : [-25000, 0.0,   25000],
    (inch)       : [-1000, 0.0,   1000],
    (foot)       : [-1000, 0.0,   1000],
    (yard)       : [-250, 0.00, 250]
} as LengthBoundSpec;

export const ANGLE_360_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (2 * PI + TOLERANCE.zeroAngle) * radian,
    (degree) : [0, 30, 360],
    (radian) : [0, 1,  2 * PI]
} as AngleBoundSpec;

export const ANGLE_360_ZERO_DEFAULT_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (2 * PI + TOLERANCE.zeroAngle) * radian,
    (degree) : [0, 0, 360],
    (radian) : [0, 0,  2 * PI]
} as AngleBoundSpec;

export const ANGLE_STRICT_180_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (PI - TOLERANCE.zeroAngle) * radian,
    (degree) : [0, 30, 179],
    (radian) : [0, 1,  PI]
} as AngleBoundSpec;

export const ANGLE_STRICT_90_BOUNDS =
{
    "min"    : -TOLERANCE.zeroAngle * radian,
    "max"    : (PI * 0.5 - TOLERANCE.zeroAngle) * radian,
    (degree) : [0, 3, 89],
    (radian) : [0, PI * 0.01667,  PI * 0.5]
} as AngleBoundSpec;

export const POSITIVE_COUNT_BOUNDS =
{
    "min"      : 1,
    "max"      : 1e9,
    (unitless) : [1, 2, 1e5]
} as IntegerBoundSpec;


export const POSITIVE_COUNT_BOUNDS_DEFAULT_1 =
{
    "min"      : 1,
    "max"      : 1e9,
    (unitless) : [1, 1, 1e5]
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
    (unitless) : [0.0, 0.5, 1.0]
} as RealBoundSpec;

export const HELIX_TURN_BOUNDS =
{
    "min"      : 0,
    "max"      : 1e9,
    (unitless) : [.0001, 4, 1e5]
} as RealBoundSpec;



//Type checking follows

export type LengthBoundSpec typecheck canBeLengthBoundSpec;
export type AngleBoundSpec typecheck canBeAngleBoundSpec;
export type IntegerBoundSpec typecheck canBeIntegerBoundSpec;
export type RealBoundSpec typecheck canBeRealBoundSpec;

export predicate canBeBoundSpec(value)
{
    value is map;
    if(!(value.min is number && value.max is number))
    {
        value.min is ValueWithUnits;
        value.max is ValueWithUnits;
        value.min.unit == value.max.unit;
    }
    value.min <= value.max;
    for(var entry in value)
    {
        if(entry.key != "min" && entry.key != "max")
        {
            entry.value is array;
            @size(entry.value) == 3;
            for(var i = 0; i < 3; i += 1)
            {
                entry.value[i] is number;
                if(i > 0)
                    entry.value[i - 1] <= entry.value[i];
            }
        }
    }
}

export predicate canBeLengthBoundSpec(value)
{
    canBeBoundSpec(value);
    isLength(value.min);
    for(var entry in value)
        entry.key == "min" || entry.key == "max" || isLength(entry.key);
}

export predicate canBeAngleBoundSpec(value)
{
    canBeBoundSpec(value);
    isAngle(value.min);
    for(var entry in value)
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
    isUnitless(value.min);
    isUnitless(value.max);
    @size(value) == 3;
    value[unitless] is array;
}

