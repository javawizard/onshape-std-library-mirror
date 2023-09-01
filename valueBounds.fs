FeatureScript 2130; /* Automatically generated version */
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
 *     (degree) : [0, 45, 360],
 *     (radian) : 1
 * } as AngleBoundSpec;
 *
 * ...
 *     isAngle(definition.myAngle, MY_BOUNDS);
 * ...
 * ```
 */

import(path : "onshape/std/containers.fs", version : "2130.0");
import(path : "onshape/std/math.fs", version : "2130.0");
import(path : "onshape/std/error.fs", version : "2130.0");
export import(path : "onshape/std/units.fs", version : "2130.0");

/** @internal */
function verifyBounds(value, boundSpec is map) returns boolean
{
    for (var entry in boundSpec)
    {
        if (entry.value is array)
        {
            if (entry.value[0] * entry.key <= value && value <= entry.value[2] * entry.key)
                return true;
            throw regenError(ErrorStringEnum.PARAMETER_OUT_OF_RANGE);
        }
    }
    // Triggers error if boundSpec is invalid
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
    verifyBounds(value, boundSpec);
}

/**
 * True for a value with area units which conforms to the given bounds.
 *
 * Used in feature preconditions to specify an area parameter.
 *
 * @param boundSpec {AreaBoundSpec} : Specifies a min, a max, and a default
 *      value. These values are possibly different in different units for the
 *      sake of round numbers.
 *
 *      To specify a parameter with different default value or different
 *      limits, use a custom `AreaBoundSpec`.
 */
export predicate isArea(value, boundSpec is AreaBoundSpec)
{
    isArea(value);
    verifyBounds(value, boundSpec);
}

/**
 * True for a value with volume units which conforms to the given bounds.
 *
 * Used in feature preconditions to specify a volume parameter.
 *
 * @param boundSpec {VolumeBoundSpec} : Specifies a min, a max, and a default
 *      value. These values are possibly different in different units for the
 *      sake of round numbers.
 *
 *      To specify a parameter with different default value or different
 *      limits, use a custom `VolumeBoundSpec`.
 */
export predicate isVolume(value, boundSpec is VolumeBoundSpec)
{
    isVolume(value);
    verifyBounds(value, boundSpec);
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
    verifyBounds(value, boundSpec);
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
    verifyBounds(value, boundSpec);
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
    verifyBounds(value, boundSpec);
}

/**
 * True for a value with force units which conforms to the given bounds.
 *
 * Used in feature preconditions to specify a force parameter.
 *
 * @param boundSpec {ForceBoundSpec} : Specifies a min, a max, and a default
 *      value. These values are possibly different in different units for the
 *      sake of round numbers.
 *
 *      To specify a parameter with different default value or different
 *      limits, use a custom `ForceBoundSpec`.
 */
export predicate isForce(value, boundSpec is ForceBoundSpec)
{
    isForce(value);
    verifyBounds(value, boundSpec);
}

/**
 * True for a value with pressure units which conforms to the given bounds.
 *
 * Used in feature preconditions to specify a pressure parameter.
 *
 * @param boundSpec {PressureBoundSpec} : Specifies a min, a max, and a default
 *      value. These values are possibly different in different units for the
 *      sake of round numbers.
 *
 *      To specify a parameter with different default value or different
 *      limits, use a custom `PressureBoundSpec`.
 */
export predicate isPressure(value, boundSpec is PressureBoundSpec)
{
    isPressure(value);
    verifyBounds(value, boundSpec);
}

/**
 * True for a value with moment units which conforms to the given bounds.
 *
 * Used in feature preconditions to specify a moment parameter.
 *
 * @param boundSpec {MomentBoundSpec} : Specifies a min, a max, and a default
 *      value. These values are possibly different in different units for the
 *      sake of round numbers.
 *
 *      To specify a parameter with different default value or different
 *      limits, use a custom `MomentBoundSpec`.
 */
export predicate isMoment(value, boundSpec is MomentBoundSpec)
{
    isMoment(value);
    verifyBounds(value, boundSpec);
}

/**
 * True for a value with acceleration units which conforms to the given bounds.
 *
 * Used in feature preconditions to specify an acceleration parameter.
 *
 * @param boundSpec {AccelerationBoundSpec} : Specifies a min, a max, and a default
 *      value. These values are possibly different in different units for the
 *      sake of round numbers.
 *
 *      To specify a parameter with different default value or different
 *      limits, use a custom `AccelerationBoundSpec`.
 */
export predicate isAcceleration(value, boundSpec is AccelerationBoundSpec)
{
    isAcceleration(value);
    verifyBounds(value, boundSpec);
}

/**
 * True for a value with angular velocity units which conforms to the given bounds.
 *
 * Used in feature preconditions to specify an angular velocity parameter.
 *
 * @param boundSpec {AngularVelocityBoundSpec} : Specifies a min, a max, and a default
 *      value. These values are possibly different in different units for the
 *      sake of round numbers.
 *
 *      To specify a parameter with different default value or different
 *      limits, use a custom `AngularVelocityBoundSpec`.
 */
export predicate isAngularVelocity(value, boundSpec is AngularVelocityBoundSpec)
{
    isAngularVelocity(value);
    verifyBounds(value, boundSpec);
}

/**
 * True for a value with energy units which conforms to the given bounds.
 *
 * Used in feature preconditions to specify an energy parameter.
 *
 * @param boundSpec {EnergyBoundSpec} : Specifies a min, a max, and a default
 *      value. These values are possibly different in different units for the
 *      sake of round numbers.
 *
 *      To specify a parameter with different default value or different
 *      limits, use a custom `EnergyBoundSpec`.
 */
export predicate isEnergy(value, boundSpec is EnergyBoundSpec)
{
    isEnergy(value);
    verifyBounds(value, boundSpec);
}

/**
 * A `LengthBoundSpec` for a positive or negative length.
 */
export const LENGTH_BOUNDS =
{
    (meter)      : [-500, 0.025, 500],
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
    (meter)      : [1e-5, 0.0025, 500],
    (centimeter) : 0.25,
    (millimeter) : 2.5,
    (inch)       : 0.1,
    (foot)       : 0.01,
    (yard)       : 0.0025
} as LengthBoundSpec;

/**
 * A `LengthBoundSpec` for an offset thickness, for a length greater than or equal to 0, with defaults
 * greater than NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS
 */
export const ZERO_INCLUSIVE_OFFSET_BOUNDS =
{
    (meter)      : [0.0, 0.005, 500],
    (centimeter) : 0.5,
    (millimeter) : 5.0,
    (inch)       : 0.25,
    (foot)       : 0.025,
    (yard)       : 0.01
} as LengthBoundSpec;

/**
 * @internal
 * A `LengthBoundSpec` for the size of a construction plane.
 */
export const PLANE_SIZE_BOUNDS =
{
    (meter)      : [1e-5, 0.15, 500],
    (centimeter) : 15,
    (millimeter) : 150,
    (inch)       : 6,
    (foot)       : 0.5,
    (yard)       : 0.166667
} as LengthBoundSpec;

/**
 * An `AreaBoundSpec` for an area greater than or equal to 0.
 */
export const AREA_BOUNDS =
{
    (squareMeter)      : [0.0, 0.000625, 500],
    (squareCentimeter) : 6.25,
    (squareMillimeter) : 625.0,
    (squareInch)       : 1.0,
    (squareFoot)       : 0.01,
    (squareYard)       : 0.001
} as AreaBoundSpec;

/**
 * A `VolumeBoundSpec` for a volume greater than or equal to 0.
 */
export const VOLUME_BOUNDS =
{
    (cubicMeter)      : [0.0, 1e-5, 500],
    (cubicCentimeter) : 10,
    (cubicMillimeter) : 1e4,
    (cubicInch)       : 1.0,
    (cubicFoot)       : 0.001,
    (cubicYard)       : 0.0001
} as VolumeBoundSpec;

/**
 * An `AngleBoundSpec` for an angle between 0 and 360 degrees, defaulting to 30 degrees.
 */
export const ANGLE_360_BOUNDS =
{
    (degree) : [-1e5, 30, 1e5],
    (radian) : 1
} as AngleBoundSpec;

/**
 * An `AngleBoundSpec` for an angle between 0 and 360 degrees, defaulting to 330 degrees.
 */
export const ANGLE_360_REVERSE_DEFAULT_BOUNDS =
{
    (degree) : [-1e5, 330, 1e5],
    (radian) : 2
} as AngleBoundSpec;

/**
 * An `AngleBoundSpec` for an angle between 0 and 360 degrees, defaulting to 0 degrees.
 */
export const ANGLE_360_ZERO_DEFAULT_BOUNDS =
{
    (degree) : [-1e5, 0, 1e5],
    (radian) : 0
} as AngleBoundSpec;

/**
 * An `AngleBoundSpec` for an angle between 0 and 360 degrees, defaulting to 360 degrees.
 */
export const ANGLE_360_FULL_DEFAULT_BOUNDS =
{
    (degree) : [-1e5, 360, 1e5],
    (radian) : 2 * PI
} as AngleBoundSpec;

/**
 * An `AngleBoundSpec` for an angle between 0 and 360 degrees, defaulting to 90 degrees.
 */
export const ANGLE_360_90_DEFAULT_BOUNDS =
{
    (degree) : [-1e5, 90, 1e5],
    (radian) : PI / 2
} as AngleBoundSpec;

/**
 * An `AngleBoundSpec` for an angle strictly less than 180 degrees.
 */
export const ANGLE_STRICT_180_BOUNDS =
{
    (degree) : [0, 30, 179.9],
    (radian) : 0.1667 * PI
} as AngleBoundSpec;

/**
 * An `AngleBoundSpec` for an angle strictly less than 90 degrees.
 */
export const ANGLE_STRICT_90_BOUNDS =
{
    (degree) : [0, 3, 89.9],
    (radian) : 0.01667 * PI
} as AngleBoundSpec;

/**
 * An `AngleBoundSpec` for an angle between -180 and 180 degrees, defaulting to 0 degrees.
 */
export const ANGLE_180_MINUS_180_BOUNDS =
{
    (degree) : [-180, 0, 180],
    (radian) : 1
} as AngleBoundSpec;

/**
 * An `IntegerBoundSpec` for an integer strictly greater than zero, defaulting to 2.
 */
export const POSITIVE_COUNT_BOUNDS =
{
    (unitless) : [1, 2, 1e5]
} as IntegerBoundSpec;

/**
 * A `RealBoundSpec` for a number greater than or equal to zero, defaulting to 1.
 */
export const POSITIVE_REAL_BOUNDS =
{
    (unitless) : [0, 1, 1e5]
} as RealBoundSpec;

/**
 * A `RealBoundSpec` for the positive or negative scale factor on a transform, defaulting to `1`.
 */
export const SCALE_BOUNDS =
{
    (unitless) : [1e-5, 1, 1e5]
} as RealBoundSpec;

/**
 * @internal
 * Count bounds for the primary direction of linear patterns
 */
export const PRIMARY_PATTERN_BOUNDS =
{
    (unitless) : [1, 2, 2500]
} as IntegerBoundSpec;

/**
 * @internal
 * Bounds for the secondary direction of a linear patterns
 */
export const SECONDARY_PATTERN_BOUNDS =
{
    (unitless) : [1, 1, 2500]
} as IntegerBoundSpec;

/**
 * @internal
 * Count bounds for a circular pattern
 */
export const CIRCULAR_PATTERN_BOUNDS =
{
    (unitless) : [1, 4, 2500]
} as IntegerBoundSpec;

/**
 * @internal
 * Bounds for a curve pattern
 */
export const CURVE_PATTERN_BOUNDS =
{
    (unitless) : [1, 2, 2500]
} as IntegerBoundSpec;

/**
 * @internal
 * The bounds for the density of an isocurve grid
 */
export const ISO_GRID_BOUNDS =
{
    (unitless) : [1, 10, 50]
} as IntegerBoundSpec;

/** @internal */
export const CLAMP_MAGNITUDE_REAL_BOUNDS =
{
    (unitless) : [-1e5, 1, 1e5]
} as RealBoundSpec;

/**
 * A `ForceBoundSpec` for a positive or negative force.
 */
export const FORCE_BOUNDS =
{
    (newton)     : [-1e5, 1, 1e5],
    (kilonewton) : 0.001,
    (poundForce) : 0.25
} as ForceBoundSpec;

/**
 * A `PressureBoundSpec` for a positive or negative pressure.
 */
export const PRESSURE_BOUNDS =
{
    (pascal)                 : [-1e10, 1000, 1e10],
    (kilopascal)             : 1,
    (megapascal)             : 1e-3,
    (gigapascal)             : 1e-6,
    (poundPerSquareInch)     : 0.1,
    (kilopoundPerSquareInch) : 0.0001
} as PressureBoundSpec;

/**
 * A `MomentBoundSpec` for a positive or negative moment.
 */
export const MOMENT_BOUNDS =
{
    (newtonMeter)      : [-1e5, 1, 1e5],
    (newtonMillimeter) : 1000,
    (inchPound)        : 10,
    (footPound)        : 1
} as MomentBoundSpec;

/**
 * An `AccelerationBoundSpec` for a positive or negative acceleration.
 */
export const ACCELERATION_BOUNDS =
{
    (meterPerSecondSquared)      : [-1e5, 1, 1e5],
    (millimeterPerSecondSquared) : 1000,
    (inchPerSecondSquared)       : 40,
    (footPerSecondSquared)       : 3
} as AccelerationBoundSpec;

/**
 * An `AngularVelocityBoundSpec` for a positive or negative angular velocity.
 */
export const ANGULAR_VELOCITY_BOUNDS =
{
    (degreePerSecond) : [-1e5, 30, 1e5],
    (radianPerSecond) : 0.5
} as AngularVelocityBoundSpec;

/**
 * An `EnergyBoundSpec` for an energy greater than or equal to 0.
 */
export const ENERGY_BOUNDS =
{
    (joule) : [0, 1, 1e5],
    (footPoundForce) : 1
} as EnergyBoundSpec;

/**
 * @internal
 * Return the bounds in a BoundSpec as an array with the first element being
 * the lower bound, and the second element being the upper bound.
 */
export function boundsRange(boundSpec is map) returns array
precondition
{
    canBeBoundSpec(boundSpec);
}
{
    for (var entry in boundSpec)
    {
        if (entry.value is array)
            return [entry.value[0] * entry.key, entry.value[2] * entry.key];
    }
    // If not found will throw an error
}

/** @internal */
export predicate canBeBoundSpec(value)
{
    value is map;
    for (var entry in value)
    {
        if (entry.value is number)
            continue;
        entry.value is array;
        @size(entry.value) == 3;
        for (var i = 0; i < 3; i += 1)
            entry.value[i] is number;
        entry.value[0] <= entry.value[1];
        entry.value[1] <= entry.value[2];
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
 *     (meter)      : [-500, 0.0025, 500],
 *     (centimeter) : .25,
 *     (millimeter) : 2.50,
 *     (inch)       : 0.1,
 *     (foot)       : 0.01,
 *     (yard)       : 0.0025
 * } as LengthBoundSpec;
 * ```
 *
 * The values for `(meter)`, `(inch)`, etc. define the bounds and default values
 * for a feature parameter defined with `MY_LENGTH_BOUNDS`. The default values will
 * be different for users who have set different default units.
 *
 * Specifically, the first unit listed specified defines the minimum value, default value,
 * and the maximum value (in terms of that unit) and the subsequent units define default
 * values for those units, when a user with those default units first opens the dialog.
 * The default value for a unit that is not listed is the default value of the first unit
 * so `{ (inch) : [0, 1, 1e4] } as LengthBoundSpec` will give a default of `2.54 cm` in
 * a Part Studio with centimeter units
 */
export type LengthBoundSpec typecheck canBeLengthBoundSpec;

/** Typecheck for LengthBoundSpec */
export predicate canBeLengthBoundSpec(value)
{
    canBeBoundSpec(value);
    for (var entry in value)
        isLength(entry.key);
}

/**
 * A spec to be used with the `isArea` predicate to define allowable area values
 * and customize UI behaviors for feature dialog parameters that take in an area.
 *
 * A typical declaration looks like:
 * ```
 * export const AREA_BOUNDS =
 * {
 *     (squareMeter)      : [0.0, 0.000625, 500],
 *     (squareCentimeter) : 6.25,
 *     (squareMillimeter) : 625.0,
 *     (squareInch)       : 1.0,
 *     (squareFoot)       : 0.01,
 *     (squareYard)       : 0.001
 * } as AreaBoundSpec;
 * ```
 *
 * For more information on what the various fields signify, see
 * `LengthBoundSpec`.
 */
export type AreaBoundSpec typecheck canBeAreaBoundSpec;

/** Typecheck for AreaBoundSpec */
export predicate canBeAreaBoundSpec(value)
{
    canBeBoundSpec(value);
    for (var entry in value)
        isArea(entry.key);
}

/**
 * A spec to be used with the `isVolume` predicate to define allowable volume values
 * and customize UI behaviors for feature dialog parameters that take in a volume.
 *
 * A typical declaration looks like:
 * ```
 * export const VOLUME_BOUNDS =
 * {
 *     (cubicMeter)      : [0.0, 1e-5, 500],
 *     (cubicCentimeter) : 10,
 *     (cubicMillimeter) : 1e4,
 *     (cubicInch)       : 1.0,
 *     (cubicFoot)       : 0.001,
 *     (cubicYard)       : 0.0001
 * } as VolumeBoundSpec;
 * ```
 *
 * For more information on what the various fields signify, see
 * `LengthBoundSpec`.
 */
export type VolumeBoundSpec typecheck canBeVolumeBoundSpec;

/** Typecheck for VolumeBoundSpec */
export predicate canBeVolumeBoundSpec(value)
{
    canBeBoundSpec(value);
    for (var entry in value)
        isVolume(entry.key);
}

/**
 * A spec to be used with the `isAngle` predicate to define allowable angles
 * and customize UI behaviors for feature dialog parameters that take in an angle.
 *
 * A typical declaration looks like:
 * ```
 * const ANGLE_360_BOUNDS =
 * {
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
    for (var entry in value)
        isAngle(entry.key);
}

/**
 * A spec to be used with the `isInteger` predicate to define allowable numbers
 * and customize UI behaviors for feature dialog parameters that take in a number.
 *
 * A typical declaration looks like:
 * ```
 * const POSITIVE_COUNT_BOUNDS =
 * {
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
    @size(value) == 1;
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
    @size(value) == 1;
    value[unitless] is array;
}

/**
 * @internal
 * A `RealBoundSpec` for a normalized parameter along an edge's length, with 0 being the start of the edge and 1 the end
 * Default UI value is 0.5, i.e. the midpoint of an open edge
 */
export const EDGE_PARAMETER_BOUNDS =
{
    (unitless) : [0.0, 0.5, 1]
} as RealBoundSpec;

/**
 * A spec to be used with the `isForce` predicate to define allowable force values
 * and customize UI behaviors for feature dialog parameters that take in a force.
 *
 * A typical declaration looks like:
 * ```
 * export const FORCE_BOUNDS =
 * {
 *     (newton)     : [-1e5, 1, 1e5],
 *     (kilonewton) : 0.001,
 *     (poundForce) : 0.25
 * } as ForceBoundSpec;
 * ```
 *
 * For more information on what the various fields signify, see
 * `LengthBoundSpec`.
 */
export type ForceBoundSpec typecheck canBeForceBoundSpec;

/** Typecheck for ForceBoundSpec */
export predicate canBeForceBoundSpec(value)
{
    canBeBoundSpec(value);
    for (var entry in value)
        isForce(entry.key);
}

/**
 * A spec to be used with the `isPressure` predicate to define allowable pressure values
 * and customize UI behaviors for feature dialog parameters that take in a pressure.
 *
 * A typical declaration looks like:
 * ```
 * const PRESSURE_BOUNDS =
 * {
 *     (pascal)                 : [-1e10, 1000, 1e10],
 *     (kilopascal)             : 1,
 *     (megapascal)             : 1e-3,
 *     (gigapascal)             : 1e-6,
 *     (poundPerSquareInch)     : 0.1,
 *     (kilopoundPerSquareInch) : 0.0001
 * } as PressureBoundSpec;
 * ```
 *
 * For more information on what the various fields signify, see
 * `LengthBoundSpec`.
 */
export type PressureBoundSpec typecheck canBePressureBoundSpec;

/** Typecheck for PressureBoundSpec */
export predicate canBePressureBoundSpec(value)
{
    canBeBoundSpec(value);
    for (var entry in value)
        isPressure(entry.key);
}

/**
 * A spec to be used with the `isMoment` predicate to define allowable moment values
 * and customize UI behaviors for feature dialog parameters that take in a moment.
 *
 * A typical declaration looks like:
 * ```
 * const MOMENT_BOUNDS =
 * {
 *     (newtonMeter)      : [-1e5, 1, 1e5],
 *     (newtonMillimeter) : 1000,
 *     (inchPound)        : 10,
 *     (footPound)        : 1
 * } as MomentBoundSpec;
 * ```
 *
 * For more information on what the various fields signify, see
 * `LengthBoundSpec`.
 */
export type MomentBoundSpec typecheck canBeMomentBoundSpec;

/** Typecheck for MomentBoundSpec */
export predicate canBeMomentBoundSpec(value)
{
    canBeBoundSpec(value);
    for (var entry in value)
        isMoment(entry.key);
}

/**
 * A spec to be used with the `isAcceleration` predicate to define allowable acceleration values
 * and customize UI behaviors for feature dialog parameters that take in an acceleration.
 *
 * A typical declaration looks like:
 * ```
 * const ACCELERATION_BOUNDS =
 * {
 *     (meterPerSecondSquared)      : [-1e5, 1, 1e5],
 *     (millimeterPerSecondSquared) : 1000,
 *     (inchPerSecondSquared)       : 40,
 *     (footPerSecondSquared)       : 3
 * } as AccelerationBoundSpec;
 * ```
 *
 * For more information on what the various fields signify, see
 * `LengthBoundSpec`.
 */
export type AccelerationBoundSpec typecheck canBeAccelerationBoundSpec;

/** Typecheck for AccelerationBoundSpec */
export predicate canBeAccelerationBoundSpec(value)
{
    canBeBoundSpec(value);
    for (var entry in value)
        isAcceleration(entry.key);
}

/**
 * A spec to be used with the `isAngularVelocity` predicate to define allowable angular velocity values
 * and customize UI behaviors for feature dialog parameters that take in an angular velocity.
 *
 * A typical declaration looks like:
 * ```
 * const ANGULAR_VELOCITY_BOUNDS =
 * {
 *     (degreePerSecond) : [-1e5, 30, 1e5],
 *     (radianPerSecond) : 0.5
 * } as AngularVelocityBoundSpec;
 * ```
 *
 * For more information on what the various fields signify, see
 * `LengthBoundSpec`.
 */
export type AngularVelocityBoundSpec typecheck canBeAngularVelocityBoundSpec;

/** Typecheck for AngularVelocityBoundSpec */
export predicate canBeAngularVelocityBoundSpec(value)
{
    canBeBoundSpec(value);
    for (var entry in value)
        isAngularVelocity(entry.key);
}

/**
 * A spec to be used with the `isEnergy` predicate to define allowable energy values
 * and customize UI behaviors for feature dialog parameters that take in an energy.
 *
 * A typical declaration looks like:
 * ```
 * const ENERGY_BOUNDS =
 * {
 *     (joule) : [0, 1, 1e5],
 *     (footPoundForce) : 1
 * } as EnergyBoundSpec;
 * ```
 *
 * For more information on what the various fields signify, see
 * `LengthBoundSpec`.
 */
export type EnergyBoundSpec typecheck canBeEnergyBoundSpec;

/** Typecheck for EnergyBoundSpec */
export predicate canBeEnergyBoundSpec(value)
{
    canBeBoundSpec(value);
    for (var entry in value)
        isEnergy(entry.key);
}

/**
 * @internal
 * A `RealBoundSpec` for a value greater than or equal to zero and strictly less than 1.
 */
export const FILLET_RHO_BOUNDS =
{
    (unitless) : [0.0, 0.5, 0.99999]
} as RealBoundSpec;

