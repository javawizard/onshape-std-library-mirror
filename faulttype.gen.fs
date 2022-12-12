FeatureScript 1930; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * Description of possible faults.
 *
 */
export enum FaultType
{
    annotation {"Name" : "No fault"}
    NO_FAULT,
    annotation {"Name" : "Entity is corrupt"}
    CORRUPT_ENTITY,
    annotation {"Name" : "Part has invalid identifiers"}
    BODY_INVALID_IDENTIFIERS,
    annotation {"Name" : "Part is inside out"}
    BODY_INSIDE_OUT,
    annotation {"Name" : "Edge tolerances are too high"}
    EDGE_OPEN,
    annotation {"Name" : "Vertex does not lie on edge"}
    EDGE_BAD_VERTEX,
    annotation {"Name" : "Curve of edge is in the wrong direction"}
    EDGE_REVERSED,
    annotation {"Name" : "Vertices are within tolerance"}
    VERTICES_TOUCH,
    annotation {"Name" : "Edges intersect"}
    WIRE_INTERSECT,
    annotation {"Name" : "Entity data structure is incorrect"}
    ENTITY_INVALID,
    annotation {"Name" : "Vertex is not on face"}
    FACE_BAD_VERTEX,
    annotation {"Name" : "Edge is not on face"}
    FACE_BAD_EDGE,
    annotation {"Name" : "Edge intersects face not at a vertex"}
    FACE_INTERSECTS_EDGE,
    annotation {"Name" : "Could not check entity"}
    CHECKING_FAILED,
    annotation {"Name" : "Faces intersect"}
    FACE_FACE_INTERSECTION,
    annotation {"Name" : "Entity is self-intersecting"}
    SELF_INTERSECTION,
    annotation {"Name" : "Entity is not tangent continuous"}
    ENTITY_NOT_G1,
    annotation {"Name" : "Entity exceeds maximum model bounds"}
    BOUNDING_BOX_VIOLATION,
    annotation {"Name" : "Entity has no geometry"}
    NO_GEOMETRY,
    annotation {"Name" : "Face has an undefined alignment"}
    UNDEFINED_FACE_SENSE,
    annotation {"Name" : "Tolerance applied to entity is too small"}
    TOLERANCE_TOO_SMALL,
    annotation {"Name" : "Edges touch at a point other than a vertex"}
    EDGES_TOUCH,
    annotation {"Name" : "Geometry is degenerate"}
    DEGENERATE_GEOMETRY,
    annotation {"Name" : "Entity has a fault"}
    GENERAL_FAULT
}


