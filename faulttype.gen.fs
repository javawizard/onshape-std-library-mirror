FeatureScript 3112; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

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
    annotation {"Name" : "Open or non-periodic curve on ring edge"}
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
    GENERAL_FAULT,
    annotation {"Name" : "Mesh is self-intersecting"}
    MESH_SELF_INT,
    annotation {"Name" : "Mesh facet has zero area"}
    MESH_FLAT_MFACET,
    annotation {"Name" : "Mesh vertex normal opposed to adjacent mesh facet normals"}
    MESH_BAD_MVX_NORMAL,
    annotation {"Name" : "Geometry is self-intersecting"}
    GEOM_SELF_INT,
    annotation {"Name" : "Some or all of entity is outside size box"}
    TOPOL_SIZE_BOX,
    annotation {"Name" : "Mesh has a slit"}
    MESH_SLIT,
    annotation {"Name" : "Face is self-intersecting"}
    FACE_SELF_INT,
    annotation {"Name" : "Face loops inconsistent"}
    FACE_BAD_LOOPS,
    annotation {"Name" : "Mesh facet has a zero length mesh fin"}
    MESH_DEGEN_MFACET,
    annotation {"Name" : "Edge order around the face does not match the order of the edges' curves"}
    FACE_BAD_EDGE_ORDER,
    annotation {"Name" : "A surface singularity has no accompanying vertex"}
    FACE_NO_VTX_AT_SING,
    annotation {"Name" : "Topology's geometry is not G1 continuous"}
    TOPOL_NOT_G1,
    annotation {"Name" : "Topology's geometry is closed but not periodic"}
    TOPOL_BAD_CLOSED,
    annotation {"Name" : "Topology's geometry has an invalid parametric degeneracy in the u and v parameter directions"}
    TOPOL_PARM_DEGEN,
    annotation {"Name" : "Topology's geometry has an invalid parametric degeneracy in the u parameter direction"}
    TOPOL_U_PARM_DEGEN,
    annotation {"Name" : "Topology's geometry has an invalid parametric degeneracy in the v parameter direction"}
    TOPOL_V_PARM_DEGEN,
    annotation {"Name" : "Tolerant curve not within edge's tolerance"}
    EDGE_BAD_SPCURVE,
    annotation {"Name" : "Edge tolerance is too small"}
    EDGE_BAD_TOL,
    annotation {"Name" : "Topology's geometry has an invalid physical degeneracy in the u and v parameter directions"}
    TOPOL_PHYS_DEGEN,
    annotation {"Name" : "Topology's geometry has an invalid physical degeneracy in the u parameter direction"}
    TOPOL_U_PHYS_DEGEN,
    annotation {"Name" : "Topology's geometry has an invalid physical degeneracy in the v parameter direction"}
    TOPOL_V_PHYS_DEGEN,
    annotation {"Name" : "Shell has a face which is not connected to the topologies in the shell"}
    SHELL_FAS_FRAGMENTED,
    annotation {"Name" : "Tolerant curve not within edge's tolerance of nominal geometry"}
    EDGE_BAD_SP_NMNL,
    annotation {"Name" : "Face has a disjoint mesh attached"}
    FACE_DISJOINT_MESH,
    annotation {"Name" : "Inconsistent shells"}
    REGION_BAD_SHELLS,
    annotation {"Name" : "Geometry is outside size box"}
    GEOM_OUTSIDE_BOX,
    annotation {"Name" : "Fin data structures are linked incorrectly"}
    FIN_NOT_ED_NEXT_PREV,
    annotation {"Name" : "Topology has invalid box"}
    TOPOL_BAD_BOX,
    annotation {"Name" : "Shell-shell inconsistency"}
    SHELL_BAD_SH_SH,
    annotation {"Name" : "Regions of body are inconsistent"}
    BODY_BAD_REGIONS,
    annotation {"Name" : "Fin pointing to the wrong vertex"}
    FIN_WRONG_VERTEX,
    annotation {"Name" : "Traversing the chain of fins entering a vertex gives inconsistent results"}
    VERTEX_FIN_CHAINS
}


