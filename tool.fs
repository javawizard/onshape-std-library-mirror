FeatureScript ✨; /* Automatically generated version */
/* This file contains enum types shared by multiple features. */
// TODO: these should be generated

// Used by boolean, moveFace
/**
 * TODO: description
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

// Used by extrude, hole, loft, revolve, sweep
/**
 * TODO: description
 */
export enum ToolBodyType
{
    annotation { "Name" : "Solid" }
    SOLID,
    annotation { "Name" : "Surface" }
    SURFACE
}

// Used by boolean, extrude, hole, loft, mirror, pattern, sweep, thicken
/**
 * TODO: description
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


