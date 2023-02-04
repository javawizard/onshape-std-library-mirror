FeatureScript 1963; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// This file exists to prevent a circular dependency between patternUtils.fs and sheetMetalPattern.fs

/**
 * The type of pattern.
 * @value PART : Creates copies of bodies.
 * @value FEATURE : Calls a feature function multiple times, first informing the
 *          `context` of the transform to be applied.
 * @value FACE : Creates copies of faces and attempts to merge them with
 *          existing bodies.
 */
export enum PatternType
{
    annotation { "Name" : "Part pattern" }
    PART,
    annotation { "Name" : "Feature pattern" }
    FEATURE,
    annotation { "Name" : "Face pattern" }
    FACE
}

/**
 * The type of mirror.
 * @seealso [PatternType]
 */
export enum MirrorType
{
    annotation { "Name" : "Part mirror" }
    PART,
    annotation { "Name" : "Feature mirror" }
    FEATURE,
    annotation { "Name" : "Face mirror" }
    FACE
}

/**
 * @internal
 * @param patternType : Either a `PatternType` or a `MirrorType`
 * @return {boolean} : `true` if the given enum value represents a feature pattern.
 */
export function isFeaturePattern(patternType)
{
    return (patternType == PatternType.FEATURE || patternType == MirrorType.FEATURE);
}

/**
 * @internal
 * @param patternType : Either a `PatternType` or a `MirrorType`
 * @return {boolean} : `true` if the given enum value represents a part pattern.
 */
export function isPartPattern(patternType)
{
    return (patternType == PatternType.PART || patternType == MirrorType.PART);
}

/**
 * @internal
 * @param patternType : Either a `PatternType` or a `MirrorType`
 * @return {boolean} : `true` if the given enum value represents a face pattern.
 */
export function isFacePattern(patternType)
{
    return (patternType == PatternType.FACE || patternType == MirrorType.FACE);
}

/**
 * @internal
 * @param patternType : Either a `PatternType` or a `MirrorType`
 * @return {boolean} : `true` if the given enum value represents a mirror.
 */
export function isMirror(patternType)
{
    return (patternType == MirrorType.PART || patternType == MirrorType.FEATURE || patternType == MirrorType.FACE);
}

