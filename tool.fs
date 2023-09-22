FeatureScript 2144; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/** The tool.fs module contains enum types shared by multiple features. */
// TODO: these should be generated

/**
 * Defines how face should be transformed in a `moveFace` feature.
 *
 * Also used by `boolean`.
 */
export enum MoveFaceType
{
    annotation { "Name" : "Offset" }
    OFFSET,
    annotation { "Name" : "Translate" }
    TRANSLATE,
    annotation { "Name" : "Rotate" }
    ROTATE
}

/**
 * Defines what type of body a body-creating feature (extrude, revolve, etc.)
 * should create.
 */
export enum ToolBodyType
{
    annotation { "Name" : "Solid" }
    SOLID,
    annotation { "Name" : "Surface" }
    SURFACE
}

/**
 * Defines what type of body a body-creating feature (extrude, revolve, etc.)
 * should create.
 */
export enum ExtendedToolBodyType
{
    annotation { "Name" : "Solid" }
    SOLID,
    annotation { "Name" : "Surface" }
    SURFACE,
    annotation { "Name" : "Thin" }
    THIN
}

/**
 * Defines how a new body from a body-creating feature (extrude, revolve, etc.)
 * should be merged with other bodies in the context.
 *
 * To include this enum with the same styling as the extrude dialog (and others),
 * use `booleanStepTypePredicate(definition)`.
 *
 * @value NEW : Creates a new body in the context with the geometry resulting
 *          from the operation.
 * @value ADD : Performs a boolean union between the new body and all bodies
 *          in the merge scope.
 * @value REMOVE : Performs a boolean subtraction of the new body from all
 *          bodies in the merge scope.
 * @value INTERSECT : Performs a boolean intersection between each new body
 *          and each body in the merge scope.
 */
export enum NewBodyOperationType
{
    annotation { "Name" : "New" }
    NEW,
    annotation { "Name" : "Add" }
    ADD,
    annotation { "Name" : "Remove" }
    REMOVE,
    annotation { "Name" : "Intersect" }
    INTERSECT
}

/**
 * Defines how a new surface from a surface-creating feature (sweep, loft, revolve, etc.)
 * should be merged with other surfaces in the context.
 *
 * To include this enum with the same styling as the extrude dialog (and others),
 * use `booleanStepTypePredicate(definition)`.
 *
 * @value NEW : Creates a new surface in the context with the geometry resulting
 *          from the operation.
 * @value ADD : Performs a surface union between the new surface and all surfaces
 *          used as input.
 */
export enum NewSurfaceOperationType
{
    annotation { "Name" : "New" }
    NEW,
    annotation { "Name" : "Add" }
    ADD
}

