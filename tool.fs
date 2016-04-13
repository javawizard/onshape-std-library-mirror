FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/* This file contains enum types shared by multiple features. */
// TODO: these should be generated

/**
 * Defines how face should be transformed in a `moveFace` feature.
 *
 * Also used by `boolean`.
 */
export enum MoveFaceType
{
    annotation { "Name" : "Translate" }
    TRANSLATE,
    annotation { "Name" : "Rotate" }
    ROTATE,
    annotation { "Name" : "Offset" }
    OFFSET
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
 * Defines how a new body from a body-creating feature (extrude, revolve, etc.)
 * should be merged with other bodies in the context.
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


