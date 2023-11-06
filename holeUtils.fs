FeatureScript 2180; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/containers.fs", version : "2180.0");
import(path : "onshape/std/units.fs", version : "2180.0");

// Used in `opHole` interface.  Exposed here because `geomOperations` uses this file to represent the dependencies of
// `opHole`
export import(path : "onshape/std/holepositionreference.gen.fs", version : "2180.0");
export import(path : "onshape/std/holeprofiletype.gen.fs", version : "2180.0");

/**
 * Defines whether each hole should have a countersink, a counterbore, or neither.
 */
export enum HoleStyle
{
    annotation { "Name" : "Simple" }
    SIMPLE,
    annotation { "Name" : "Counterbore" }
    C_BORE,
    annotation { "Name" : "Countersink" }
    C_SINK
}

/**
 * Describes a single profile for a [HoleDefinition].
 * @seealso [holeProfile] for standard circular profiles.
 * @seealso [holeProfileBeforeReference] for a circular profile meant to be used as the first profile of the hole.
 * @seealso [matchedHoleProfile] for a profile that geometrically matches the [HolePositionReference].
 * @type {{
 *      @field positionReference {HolePositionReference} : The reference along the hole axis to which this profile is relative.
 *      @field beforeReference {boolean} : @optional Whether the `position` of the profile should reference the start
 *          or end of the given `positionReference`. See [holeProfileBeforeReference] for more detail. Default is `false`
 *          if not provided. Only considered when `profileType` is `POSITIONED`, ignored otherwise.
 *      @field profileType {HoleProfileType} : How the profile should be constructed in relation to the given `positionReference`
 *      @field position {ValueWithUnits} : @requiredIf {`profileType` is `POSITIONED`} The position of the profile along
 *          the hole axis, relative to the given `positionReference`.
 *      @field radius {ValueWithUnits} : The radius of the profile.  Can be `0` to specify that the profile forms a point.
 *      @field targetMustDifferFromPrevious {boolean} : @optional If `true`, instructs [opHole] to skip the construction
 *          of any holes in which the target referenced by this profile's `positionReference` is not different than the
 *          target reference by the previous profile's `positionReference`. Cannot be set to `true` on the first profile
 *          or any profile that does not have a different `positionReference` than the previous profile. Default is `false`.
 *      @field name {string} : @optional A name for to assign to the edge created by [opHole] which corresponds to this profile.
 *          Supplying a `name` allows for the querying of profile edges by name when using [qOpHoleProfile].
 *
 * }}
 */
export type HoleProfile typecheck canBeHoleProfile;

/** Typecheck for [HoleProfile] */
export predicate canBeHoleProfile(value)
{
    value is map;
    value.positionReference is HolePositionReference;
    value.profileType is HoleProfileType;
    if (value.profileType == HoleProfileType.POSITIONED)
    {
        value.beforeReference is boolean || value.beforeReference is undefined;
        isLength(value.position);
    }
    isLength(value.radius);
    value.targetMustDifferFromPrevious is boolean || value.targetMustDifferFromPrevious == undefined;
    value.name is string || value.name == undefined;
}

/**
 * Returns a new circular [HoleProfile] at a given `position` in relation to the end of the range of the
 * `positionReference`. See [HolePositionReference] for further detail about the range of the `positionReference`.
 *
 * @param optionalParameters {{
 *     @field targetMustDifferFromPrevious {boolean} : @optional See [HoleProfile].
 *     @field name {string} : @optional See [HoleProfile].
 *
 * }}
 */
export function holeProfile(positionReference is HolePositionReference, position is ValueWithUnits,
    radius is ValueWithUnits, optionalParameters is map) returns HoleProfile
{
    return {
                "positionReference" : positionReference,
                "beforeReference" : false,
                "profileType" : HoleProfileType.POSITIONED,
                "position" : position,
                "radius" : radius,
                "targetMustDifferFromPrevious" : optionalParameters.targetMustDifferFromPrevious,
                "name" : optionalParameters.name
            } as HoleProfile;
}

export function holeProfile(positionReference is HolePositionReference, position is ValueWithUnits,
    radius is ValueWithUnits) returns HoleProfile
{
    return holeProfile(positionReference, position, radius, {});
}

/**
 * Returns a new circular [HoleProfile] at a given `position` in relation to the beginning of the range of the
 * `positionReference`. See [HolePositionReference] for further detail about the range of the `positionReference`.
 * This type of profile is useful as the first profile of a hole, such that if the hole cylinder intersects the first
 * target at a slanted or otherwise irregular face, the first profile is backed up enough such that when the hole tool
 * is subtracted from the target, there is no undesirable overhang left behind.
 *
 * @param optionalParameters {{
 *     @field targetMustDifferFromPrevious {boolean} : @optional See [HoleProfile].
 *     @field name {string} : @optional See [HoleProfile].
 *
 * }}
 */
export function holeProfileBeforeReference(positionReference is HolePositionReference, position is ValueWithUnits,
    radius is ValueWithUnits, optionalParameters is map) returns HoleProfile
{
    return {
                "positionReference" : positionReference,
                "beforeReference" : true,
                "profileType" : HoleProfileType.POSITIONED,
                "position" : position,
                "radius" : radius,
                "targetMustDifferFromPrevious" : optionalParameters.targetMustDifferFromPrevious,
                "name" : optionalParameters.name
            } as HoleProfile;
}

export function holeProfileBeforeReference(positionReference is HolePositionReference, position is ValueWithUnits,
    radius is ValueWithUnits) returns HoleProfile
{
    return holeProfileBeforeReference(positionReference, position, radius, {});
}

/**
 * Returns a new [HoleProfile] that is geometrically matched to the `positionReference`. This is useful for
 * configurations like blind-in-last, where a transition from one radius to another must be made that matches the shape
 * of the position reference, to avoid the hole tool intersecting incorrectly with the part(s) in question.  To form a
 * valid [HoleDefinition], `MATCHED` profiles must come in pairs of different radii.
 *
 * @param optionalParameters {{
 *     @field targetMustDifferFromPrevious {boolean} : @optional See [HoleProfile].
 *     @field name {string} : @optional See [HoleProfile].
 *
 * }}
 */
export function matchedHoleProfile(positionReference is HolePositionReference, radius is ValueWithUnits,
    optionalParameters is map) returns HoleProfile
{
    return {
                "positionReference" : positionReference,
                "profileType" : HoleProfileType.MATCHED,
                "radius" : radius,
                "targetMustDifferFromPrevious" : optionalParameters.targetMustDifferFromPrevious,
                "name" : optionalParameters.name
            } as HoleProfile;
}

export function matchedHoleProfile(positionReference is HolePositionReference, radius is ValueWithUnits) returns HoleProfile
{
    return matchedHoleProfile(positionReference, radius, {});
}

/**
 * Describes the shape of a hole using a series of [HoleProfile]s.
 *
 * @seealso [opHole]
 *
 * @type {{
 *      @field profiles {array} : An array of [HoleProfile]s which define the shape of the hole.  The profiles are
 *          interpreted in order, from the top to the bottom of the hole.  The final profile must have a radius of `0`.
 *          Each profile must specify a unique `name`, or all of the profiles must leave their `name` field `undefined`.
 *          If two or more adjacent profiles in the list end up being identical (in the same position with the same
 *          radius) when their final placement is determined in [opHole], the identical profiles will be collapsed into
 *          a single profile, which uses the name of the first of the identical profiles.
 *      @field faceNames {array} : @optional A list of names to assign to the faces created by [opHole].  Should be the
 *          same length as the `profiles` array, where `faceNames[i]` is the name of the face created between profiles
 *          `i - 1` and `i`, and `faceNames[0]` is the name of the top cap face (the face before profile `0`).
 *          If any profiles are collapsed, the names of the faces between the collapsed profiles are skipped.
 *          Supplying `faceNames` allows for the querying of faces by name when using [qOpHoleFace].
 * }}
 */
export type HoleDefinition typecheck canBeHoleDefinition;

/** Typecheck for [HoleDefinition] */
export predicate canBeHoleDefinition(value)
{
    value is map;
    value.profiles is array;
    for (var profile in value.profiles)
    {
        profile is HoleProfile;
    }
    value.profiles[size(value.profiles) - 1].radius == 0 * meter;

    value.faceNames is array || value.faceNames == undefined;
    if (value.faceNames is array)
    {
        size(value.faceNames) == size(value.profiles);
        for (var faceName in value.faceNames)
        {
            faceName is string;
        }
    }
}

/**
 * Returns a new [HoleDefinition].
 *
 * @param profiles : @autocomplete `[holeProfile(HolePositionReference.AXIS_POINT, 0 * inch, 0.1 * inch), holeProfile(HolePositionReference.AXIS_POINT, 1 * inch, 0 * inch)]`
 * @param optionalParameters {{
 *     @field faceNames {string} : @optional See [HoleDefinition].
 * }}
 */
export function holeDefinition(profiles is array, optionalParameters is map) returns HoleDefinition
{
    return {
                "profiles" : profiles,
                "faceNames" : optionalParameters.faceNames
            } as HoleDefinition;
}

/**
 * @param profiles : @autocomplete `[holeProfile(HolePositionReference.AXIS_POINT, 0 * inch, 0.1 * inch), holeProfile(HolePositionReference.AXIS_POINT, 1 * inch, 0 * inch)]`
 */
export function holeDefinition(profiles is array) returns HoleDefinition
{
    return holeDefinition(profiles, {});
}

