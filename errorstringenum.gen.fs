FeatureScript 819; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * @internal
 * The built-in user-facing errors available within Onshape.
 *
 * To report a custom error, pass a string as the first argument of regenError.
 *
 * See `error.fs` for usage.
 * @default @value INVALID_RESULT */
export enum ErrorStringEnum
{
    NO_ERROR,
    /* Unknown operation type. */
    UNKNOWN_OPERATION,
    /* Too many entities are selected. */
    TOO_MANY_ENTITIES_SELECTED,
    /* Two or more points are coincident. */
    POINTS_COINCIDENT,
    /* Cannot determine translation direction. */
    NO_TRANSLATION_DIRECTION,
    /* Cannot determine rotation axis. */
    NO_ROTATION_AXIS,
    /* Cannot compute tangent plane. */
    NO_TANGENT_PLANE,
    /* Cannot compute tangent line. */
    NO_TANGENT_LINE,
    /* Invalid input selections. */
    INVALID_INPUT,
    /* Cannot resolve entities. */
    CANNOT_RESOLVE_ENTITIES,
    /* Cannot evaluate vertex position. */
    CANNOT_EVALUATE_VERTEX,
    /* Cannot resolve plane. */
    CANNOT_RESOLVE_PLANE,
    /* Cannot compute bounding box. */
    CANNOT_COMPUTE_BBOX,
    /* Parts or Surfaces must be present in the Part Studio. */
    CANNOT_BE_EMPTY,
    /* Writing to cache failed. */
    CACHE_WRITE_FAILED,
    /* Reading from cache failed. */
    CACHE_READ_FAILED,
    /* Failed to create line rendering of topology. */
    HLR_FAILED,
    /* Some of the geometry intersects itself or is degenerate. */
    BAD_GEOMETRY,
    /* Operation created invalid geometry. */
    INVALID_RESULT,
    /* Some external references are missing. */
    MISSING_EXT_REF,
    /* Failed to read model. */
    READ_FAILED,
    /* Failed to write model. */
    WRITE_FAILED,
    /* Wrong type of entity selected for operation. */
    WRONG_TYPE,
    /* Tangent propagation failed. */
    TANGENT_PROPAGATION_FAILED,
    /* Error regenerating. */
    REGEN_ERROR,
    /* Could not compute transform. */
    COULD_NOT_COMPUTE_TRANSFORM,
    /* Mate is invalid. */
    MATE_INVALID_MATE,
    /* Mate connectors are invalid. */
    MATECONNECTOR_INVALID_MATE,
    /* Mate needs two mate connectors. */
    MATE_TWO_MATECONNECTORS_NEEDED,
    /* Mate connectors are on same instance. */
    MATECONNECTORS_ON_SAME_OCCURRENCE,
    /* Mate is over defined. */
    MATE_OVERDEFINED,
    /* Mate is not consistent. */
    MATE_INCONSISTENT,
    /* No merge scope selected. */
    BOOLEAN_NEED_ONE_SOLID,
    /* Boolean operation failed to return a valid part. */
    BOOLEAN_INVALID,
    /* Intersection failed. */
    BOOLEAN_INTERSECT_FAIL,
    /* Cannot subtract a part from itself. */
    BOOLEAN_SAME_INPUT,
    /* Need at least two parts for a Boolean operation. */
    BOOLEAN_BAD_INPUT,
    /* Boolean resulted in no geometry change. The parts either do not intersect or are totally contained. */
    BOOLEAN_UNION_NO_OP,
    /* The parts either do not intersect or are totally contained. */
    BOOLEAN_INTERSECT_NO_OP,
    /* Selected tools and targets do not intersect. */
    BOOLEAN_SUBTRACT_NO_OP,
    /* Mid plane requires 2 points, 2 planes or 1 open edge. */
    CPLANE_INPUT_MIDPLANE,
    /* Offset plane requires a plane to offset from. */
    CPLANE_INPUT_OFFSET_PLANE,
    /* Point-Plane requires a point and a plane. */
    CPLANE_INPUT_POINT_PLANE,
    /* Line-Angle plane requires a reference line. */
    CPLANE_INPUT_LINE_ANGLE,
    /* Point-Normal plane requires a point and an axis. */
    CPLANE_INPUT_POINT_LINE,
    /* Three point plane requires 3 points. */
    CPLANE_INPUT_THREE_POINT,
    /* Could not create construction plane. */
    CPLANE_FAILED,
    /* Could not resolve the neutral plane. */
    DRAFT_NO_NEUTRAL_PLANE,
    /* Could not resolve faces to draft. */
    DRAFT_NO_DRAFT_FACE,
    /* Selected faces could not be drafted. */
    DRAFT_FAILED,
    /* Reference must be to a face. */
    EXTRUDE_INVALID_REF_FACE,
    /* End condition must be a surface. */
    EXTRUDE_INVALID_REF_SURFACE,
    /* Failed to extrude selections, check input. */
    EXTRUDE_FAILED,
    /* Cannot compute extrude direction. */
    EXTRUDE_NO_DIRECTION,
    /* Could not use selected entities to extrude. */
    EXTRUDE_INVALID_ENTITIES,
    /* Cannot have more than 2500 instances in a pattern. */
    PATTERN_INPUT_TOO_MANY_INSTANCES,
    /* Instance count cannot be less than 2. */
    PATTERN_INPUT_TOO_FEW_INSTANCES,
    /* Failed to create pattern, check input. */
    PATTERN_FACE_FAILED,
    /* Pattern could not be created on the same part. */
    PATTERN_NOT_ON_BODY,
    /* Could not pattern selected parts. */
    PATTERN_BODY_FAILED,
    /* Translation requires two vertices or an edge. */
    TRANSFORM_TRANSLATE_INPUT,
    /* Translation by distance requires two vertices, an edge, a plane, or a cylindrical face. */
    TRANSFORM_TRANSLATE_BY_DISTANCE_INPUT,
    /* Could not transform part. */
    TRANSFORM_FAILED,
    /* Could not shell part with selections. */
    SHELL_FAILED,
    /* Cannot blend smooth edges. */
    EDGEBLEND_SMOOTH,
    /* Could not blend edges. */
    EDGEBLEND_FAILED,
    /* Wrong type of entity for concentric constraint. */
    DIRECT_EDIT_WRONG_CONCENTRIC,
    /* Wrong type of entity for equal radius constraint. */
    DIRECT_EDIT_WRONG_EQ_RADIUS,
    /* Could not identify filleted faces. */
    DIRECT_EDIT_NO_FILLET_FACES,
    /* Could not offset surface. */
    DIRECT_EDIT_NO_OFFSET,
    /* Could not constrain faces as selected. */
    DIRECT_EDIT_CONSTRAIN_FACE_FAILED,
    /* Could not replace faces as requested. */
    DIRECT_EDIT_REPLACE_FACE_FAILED,
    /* Could not delete selected faces. */
    DIRECT_EDIT_DELETE_FACE_FAILED,
    /* Could not modify selected fillets. */
    DIRECT_EDIT_MODIFY_FILLET_FAILED,
    /* Could not modify selected faces as requested. */
    DIRECT_EDIT_MODIFY_FACE_FAILED,
    /* Could not transform selected faces as requested. */
    DIRECT_EDIT_MOVE_FACE_FAILED,
    /* Could not offset selected faces as requested. */
    DIRECT_EDIT_OFFSET_FACE_FAILED,
    /* Part import failed. */
    IMPORT_PART_FAILED,
    /* Assembly import failed. */
    IMPORT_ASSEMBLY_FAILED,
    /* Could not imprint entities on selected plane. */
    IMPRINT_FAILED,
    /* Revolve would create self-intersecting part. */
    REVOLVE_FAILED,
    /* Failed to revolve in the second direction. */
    REVOLVE_2ND_DIR_FAILED,
    /* Revolved face is not planar. */
    REVOLVE_NOT_PLANAR,
    /* Revolved face is perpendicular to axis. */
    REVOLVE_PERPENDICULAR,
    /* Could not use selected entities to revolve. */
    REVOLVE_INVALID_ENTITIES,
    /* Tool entity cannot split the selected part. */
    SPLIT_FAILED,
    /* Incorrect input for tool type. */
    SPLIT_INVALID_INPUT,
    /* Sweep path curves are not all connected. */
    SWEEP_INVALID_PATH,
    /* Could not create valid swept body, check input. */
    SWEEP_FAILED,
    /* Sweep path is self intersecting. */
    SWEEP_PATH_FAILED,
    /* Could not use profile selections. */
    SWEEP_PROFILE_FAILED,
    /* Could not create a wire part from curves. */
    WIRE_CREATION_FAILED,
    /* Select a sketch plane. */
    SKETCH_NO_PLANE,
    /* Some of the entities could not be used. */
    SKETCH_INPUT_INVALID,
    /* There is no active sketch. */
    SKETCH_NOT_ACTIVE,
    /* Could not initialize solver. */
    SKETCH_SOLVER_NOT_INITIALIZED,
    /* Sketch evaluation failed. */
    SKETCH_EVALUATION_FAILED,
    /* Sketch modification failed. */
    SKETCH_MODIFICATION_FAILED,
    /* Sketch geometry could not be updated after solve. */
    SKETCH_UPDATE_FAILED,
    /* Sketch could not be solved. */
    SKETCH_SOLVE_FAILED,
    /* Could not add constraint. */
    SKETCH_ADD_CONSTRAINT_FAILED,
    /* Could not add dimension. */
    SKETCH_ADD_DIMENSION_FAILED,
    /* Could not position dimension. */
    SKETCH_POSITION_DIMENSION_FAILED,
    /* A constraint must involve something from the sketch. */
    SKETCH_CONSTRAINT_NEEDS_SKETCH_ENTITY,
    /* Unknown constraint type. */
    SKETCH_CONSTRAINT_UNKNOWN,
    /* Cannot find sketch entity. */
    SKETCH_MISSING_ENTITY,
    /* Cannot fillet selected point. */
    SKETCH_FILLET_INVALID_POINT,
    /* Cannot fillet parallel edges. */
    SKETCH_FILLET_PARALLEL,
    /* Cannot add sketch fillet. */
    SKETCH_FILLET_FAIL,
    /* The face could not be used in the sketch. */
    SKETCH_USE_FAILED,
    /* Some of the face edges could not be used in the sketch. */
    SKETCH_USE_PARTIAL,
    /* Could not create spline through selected points. */
    SKETCH_SPLINE_FAILED,
    /* These points make a bad spline, likely self-intersecting. */
    SKETCH_BAD_SPLINE,
    /* Could not dynamically update sketch during drag. */
    SKETCH_DRAG_ERROR,
    /* Could not project the selected entities into the current sketch. */
    SKETCH_PROJ_FAILED,
    /* Some entities could not projected unto the current sketch. */
    SKETCH_PROJ_PARTIAL,
    /* Failed to add tangent arc. */
    SKETCH_TANGENT_ARC_FAILED,
    /* Could not find tangent at curve endpoint. */
    SKETCH_TANGENT_NOT_FOUND,
    /* Could not offset entities. */
    SKETCH_OFFSET_FAILED,
    /* Offset could not be created at this distance. */
    SKETCH_OFFSET_DISTANCE,
    /* Could not trim this selected entity. */
    SKETCH_TRIM_FAILED,
    /* Could not add inferences. */
    SKETCH_INFERENCE_FAILED,
    /* Could not modify dimension. */
    SKETCH_MODIFY_DIM_FAILED,
    /* Had no actively dragged sketch to stop dragging. */
    SKETCH_DRAG_NO_SKETCH,
    /* Could not infer sketch dimension value. */
    SKETCH_INFER_DIM_FAILED,
    /* Cannot delete points used by curve. */
    SKETCH_DELETE_PTS_FAILED,
    /* None of the selected entities could be deleted. */
    SKETCH_DELETE_FAILED,
    /* The arc could not be created through these three points. */
    SKETCH_ARC_FAILED,
    /* The line could not be created between these two points. */
    SKETCH_LINE_FAILED,
    /* The circle could not be created with selected points. */
    SKETCH_CIRCLE_FAILED,
    /* The rectangle could not be created with selected points. */
    SKETCH_RECTANGLE_FAILED,
    /* Start entity was not at the end of a curve. */
    SKETCH_TANGENT_ARC_INVALID_START,
    /* Construction point could not be created. */
    SKETCH_CONSTRUCTION_POINT_FAILED,
    /* Internal error : Deserialization failed. */
    SYS_INTERNAL_DESERIALIZATION,
    /* Onshape encountered a problem with your last operation. If the problem persists, please contact support. */
    SYS_SERVER_EXCEPTION,
    /* Current part studio cannot regenerate. */
    SYS_ERROR_REGEN,
    /* Internal error : Messaging exception. */
    SYS_ERROR_MESSAGING,
    /* Failed to resolve element. */
    CANNOT_RESOLVE_ELEMENT,
    /* Nothing selected. */
    NOTHING_SELECTED,
    /* Angle dimensions require two lines. */
    SKETCH_ANGLE_TWO_LINES,
    /* A dimension can only be added between two different geometries. */
    SKETCH_DIMENSION_DIFF_ENTITIES,
    /* Cannot add constraint between an entity and itself. */
    SKETCH_CONSTRAINT_DIFF_ENTITIES,
    /* A constraint requires two entities. */
    SKETCH_CONSTRAINT_TWO_ENTITIES,
    /* A dimension requires two entities. */
    SKETCH_DIMENSION_TWO_ENTITIES,
    /* Could not create coincident constraint. */
    SKETCH_COINCIDENT_FAILED,
    /* A coincident constraint cannot be applied to two curves of different types. */
    SKETCH_COINCIDENT_INPUT_ERROR,
    /* A coincident constraint cannot be added to two points from the same curve. */
    SKETCH_COINCIDENT_DIFF_POINTS,
    /* A concentric constraint can only be added to circles, arcs, ellipses and points. */
    SKETCH_CONCENTRIC_INPUT_ERROR,
    /* Could not create concentric constraint. */
    SKETCH_CONCENTRIC_FAILED,
    /* An equal constraint can only be added between two lines or two curves. */
    SKETCH_EQUAL_INPUT_ERROR,
    /* Cannot find ends of a segment for equal constraint. */
    SKETCH_EQUAL_NO_ENDS,
    /* Could not create equal constraint. */
    SKETCH_EQUAL_FAILED,
    /* A fix constraint requires one entity. */
    SKETCH_FIX_ONE_ENT,
    /* Cannot add fix constraint. */
    SKETCH_FIX_FAILED,
    /* Could not constrain points to internal line. */
    SKETCH_DIR_INTERNAL,
    /* A directional constraint requires one line or ellipse or two points. */
    SKETCH_DIR_INPUT,
    /* Could not create horizontal constraint. */
    SKETCH_HORIZONTAL_FAILED,
    /* Could not create vertical constraint. */
    SKETCH_VERTICAL_FAILED,
    /* Could not create offset constraint. */
    SKETCH_OFFSET_CONSTRAINT_FAILED,
    /* Could not create parallel constraint. */
    SKETCH_PARALLEL_CONSTRAINT_FAILED,
    /* A parallel constraint requires two lines. */
    SKETCH_PARALLEL_INPUT_ERROR,
    /* A dimension cannot be added between a curve and one of its points. */
    SKETCH_DIMENSION_INPUT_ERROR,
    /* Distance dimension value is not a length. */
    SKETCH_DIMENSION_DIST_ERROR,
    /* The distance dimension could not be created. */
    SKETCH_DIMENSION_FAILED,
    /* Normal constraint requires a line. */
    SKETCH_NORMAL_NEED_LINE,
    /* Cannot add normal constraint between these geometries. */
    SKETCH_NORMAL_INPUT_ERROR,
    /* A normal constraint requires a line and a curve. */
    SKETCH_NORMAL_INPUT_NEEDED,
    /* Could not split geometries into groups. */
    SKETCH_CANNOT_SPLIT_INTO_GROUPS,
    /* An offset can only be added to a pair of curves of the same type, or a circle and a point. */
    SKETCH_OFFSET_BAD_PAIR,
    /* An offset constraint can only be added between one or two pairs of entities. */
    SKETCH_OFFSET_INPUT_ERROR,
    /* Midpoint constraint requires a point and a line or arc. */
    SKETCH_MIDPOINT_INPUT_ERROR,
    /* Midpoint constraint requires a point. */
    SKETCH_MIDPOINT_NEED_POINT,
    /* Cannot make a midpoint constraint between a segment and one of its points. */
    SKETCH_MIDPOINT_NEED_DIFF_POINT,
    /* Cannot find ends of the segment for midpoint constraint. */
    SKETCH_MIDPOINT_MISSING_ENDS,
    /* Cannot find all three points for midpoint constraint. */
    SKETCH_MIDPOINT_MISSING_PTS,
    /* Could not create internal line for midpoint constraint. */
    SKETCH_MIDPOINT_NO_INTERNAL_LINE,
    /* Cannot add coincident constraint between point and segment for midpoint constraint. */
    SKETCH_MIDPOINT_NO_COINCIDENT,
    /* Cannot add midpoint constraint between point and end points for midpoint constraint. */
    SKETCH_MIDPOINT_FAILED,
    /* A perpendicular constraint requires two lines. */
    SKETCH_PERPENDICULAR_INPUT_ERROR,
    /* Could not create perpendicular constraint. */
    SKETCH_PERPENDICULAR_FAILED,
    /* All the entities must be points or all must be lines. */
    SKETCH_POINT_LINE_ONLY,
    /* Unknown projection type. */
    SKETCH_PROJECTION_UNKNOWN,
    /* The projection could not be updated. */
    SKETCH_PROJECTION_FAILED,
    /* A silhouette projection requires a point and a segment. */
    SKETCH_SIL_PROJECTION_INPUT_ERROR,
    /* Could not find silhouette point. */
    SKETCH_SIL_PROJECTION_MISSING_POINT,
    /* Length dimensions require a single line, spline or conic. */
    SKETCH_LENGTH_DIM_INPUT_ERROR,
    /* Cannot find ends of the line for the length dimension. */
    SKETCH_LENGTH_DIM_MISSING_ENDS,
    /* Could not find dimension length. */
    SKETCH_LENGTH_DIM_NOT_FOUND,
    /* The length dimension could not be created. */
    SKETCH_LENGTH_DIM_FAILED,
    /* Radius dimensions require a single circle or arc. */
    SKETCH_RADIUS_INPUT_ERROR,
    /* The radius dimension could not be created. */
    SKETCH_RADIUS_DIM_FAILED,
    /* A tangent constraint requires two curves, one of which must not be a line. */
    SKETCH_TANGENT_INPUT_ERROR,
    /* Could not create tangent constraint. */
    SKETCH_TANGENT_FAILED,
    /* Failed to resolve part. */
    PART_QUERY_FAILED,
    /* Query resolved into multiple parts. */
    PART_QUERY_MULTI,
    /* Failed to resolve mate connector. */
    MATECONNECTOR_QUERY_FAILED,
    /* Failed to resolve mate connector origin. */
    MATECONNECTOR_QUERY_ORIGIN_FAILED,
    /* Failed to resolve mate connector axis. */
    MATECONNECTOR_QUERY_AXIS_FAILED,
    /* Failed to resolve mate connector coordinate system. */
    MATECONNECTOR_QUERY_CSYS_FAILED,
    /* The instance could not be inserted as it would create a cyclic relationship. */
    ASSEMBLY_INSERT_WILL_CAUSE_CYCLES,
    /* Mirror line selection is not valid. */
    SKETCH_MIRROR_NEED_VALID_MIRROR_LINE,
    /* Select entities to be mirrored. */
    SKETCH_MIRROR_NEED_ENTITIES_TO_MIRROR,
    /* The system failed to create a mirror constraint. */
    SKETCH_MIRROR_CONSTRAINT_FAILED,
    /* Unable to mirror the selected geometry. */
    SKETCH_MIRROR_FAILED,
    /* Self-intersecting curve selected. */
    SELF_INTERSECTING_CURVE_SELECTED,
    /* For best results ensure that the path start point and the profile are on the same plane. */
    SWEEP_START_NOT_ON_PROFILE,
    /* Parallel directions selected for first and second direction. */
    PATTERN_DIRECTIONS_PARALLEL,
    /* Failed to resolve the instance for mate. */
    MATE_OCCURRENCE_NOT_VALID,
    /* Mate cannot be added between member of same group. */
    MATE_WITHIN_SAME_GROUP,
    /* Failed to create assembly instance. */
    EXPORT_ASSEMBLY_UNKNOWN_NODE_TYPE,
    /* Failed to create assembly instance. */
    EXPORT_ASSEMBLY_CREATE_INSTANCE_FAILED,
    /* Only bodies can be exported. */
    EXPORT_PARTS_AS_XTS_NOT_A_BODY,
    /* Failed to export to XT format. */
    EXPORT_PARTS_AS_XTS_FAILED_TO_WRITE_XT,
    /* Cannot determine owner part of mate connector. */
    MATECONNECTOR_OWNER_PART_NOT_RESOLVED,
    /* Some curves could not be added to the model. */
    WIRE_CREATION_PARTIAL_FAILURE,
    /* Server is in invalid state. */
    SERVER_IS_IN_INVALID_STATE,
    /* Unable to extend the selected geometry. */
    SKETCH_EXTEND_FAILED,
    /* Cannot follow a user who is already following you. */
    FOLLOW_CYCLE_ERROR,
    /* The current fillet radius produces invalid geometry. */
    SKETCH_FILLET_INVALID_RADIUS,
    /* A coincident constraint requires two entities. */
    SKETCH_CONSTRAINT_COINCIDENT_TWO_ENTITIES,
    /* A concentric constraint requires two entities. */
    SKETCH_CONSTRAINT_CONCENTRIC_TWO_ENTITIES,
    /* An equal constraint requires two entities. */
    SKETCH_CONSTRAINT_EQUAL_TWO_ENTITIES,
    /* A midpoint constraint requires two entities. */
    SKETCH_CONSTRAINT_MIDPOINT_TWO_ENTITIES,
    /* Select face or sketch region to extrude. */
    EXTRUDE_NO_SELECTED_REGION,
    /* Selected sketch contains no region (has only open curves). */
    EXTRUDE_NO_REGION_IN_SKETCH,
    /* Select parts to delete. */
    DELETE_SELECT_PARTS,
    /* Select parts to copy. */
    COPY_SELECT_PARTS,
    /* Selected entity does not split selected parts. */
    SPLIT_NO_CHANGE,
    /* Select a mirror plane. */
    MIRROR_NO_PLANE,
    /* Select parts to mirror. */
    MIRROR_SELECT_PARTS,
    /* Select a pattern axis. */
    PATTERN_CIRCULAR_NO_AXIS,
    /* Select faces to pattern. */
    PATTERN_SELECT_FACES,
    /* Select parts to pattern. */
    PATTERN_SELECT_PARTS,
    /* Select a pattern direction. */
    PATTERN_LINEAR_NO_DIR,
    /* Select faces to remove. */
    SHELL_SELECT_FACES,
    /* Select a neutral plane. */
    DRAFT_SELECT_NEUTRAL,
    /* Select faces to draft. */
    DRAFT_SELECT_FACES,
    /* Select edges or faces to chamfer. */
    CHAMFER_SELECT_EDGES,
    /* Select edges or faces to fillet. */
    FILLET_SELECT_EDGES,
    /* Select sketch curves to extrude. */
    EXTRUDE_SURF_NO_CURVE,
    /* Select a part to terminate the extrude. */
    EXTRUDE_SELECT_TERMINATING_BODY,
    /* Select a face to terminate the extrude. */
    EXTRUDE_SELECT_TERMINATING_SURFACE,
    /* Select anchor entity. */
    DIRECT_EDIT_SELECT_ANCHOR,
    /* Select sketch curves to revolve. */
    REVOLVE_SURF_NO_CURVE,
    /* Select face or sketch region to revolve. */
    REVOLVE_SELECT_FACES,
    /* Select the axis to revolve around. */
    REVOLVE_SELECT_AXIS,
    /* Select faces or sketch regions to sweep. */
    SWEEP_SELECT_PROFILE,
    /* Select entities for sweep path. */
    SWEEP_SELECT_PATH,
    /* Select faces to delete. */
    DIRECT_EDIT_DELETE_SELECT_FACES,
    /* Select fillet faces to modify. */
    DIRECT_EDIT_MODIFY_FILLET_SELECT,
    /* Select faces to modify. */
    DIRECT_EDIT_MODIFY_FACE_SELECT,
    /* Select faces to replace. */
    DIRECT_EDIT_REPLACE_FACE_SELECT,
    /* Select faces to offset. */
    DIRECT_EDIT_OFFSET_FACE_SELECT,
    /* Select faces to move. */
    DIRECT_EDIT_MOVE_FACE_SELECT,
    /* Select a mate connector for */
    SELECT_MATECONNECTOR,
    /* Assembly is overdefined by */
    OVERDEFINED_ASSEMBLY,
    /* Successfully upgraded workspace. */
    PART_STUDIO_UPGRADE_SUCCESSFUL,
    /* Workspace upgrade failed. */
    PART_STUDIO_UPGRADE_FAILED,
    /* No upgrade available. */
    PART_STUDIO_UPGRADE_NONE,
    /* Failed to resolve all instances in group. */
    MATE_GROUP_OCCURRENCES_UNRESOLVED,
    /* Select sketch curves to sweep. */
    SWEEP_SURF_NO_CURVE_PROFILE,
    /* Resetting mates had no effect. */
    MATE_RESET_HAD_NO_EFFECT,
    /* Mate connector cannot be defined on multiple instances. */
    MATECONNECTOR_MULTIPLE_OCCURRENCES,
    /* Cannot find instance of mate connector. */
    MATECONNECTOR_OCCURRENCE_NOT_RESOLVED,
    /* Operation failed because it would create cyclical references. */
    ELEMENT_REFERENCE_CYCLE_DETECTED,
    /* Mate overdefines the assembly. */
    MATE_OVERDEFINES_ASSEMBLY,
    /* Mate cannot resolve mate connectors. */
    MATE_CANNOT_RESOLVE_CONNECTORS,
    /* This operation can not complete because it creates geometry that is beyond the system limit. */
    SKETCH_EXCEEDS_BOUNDS,
    /* Result of sweep intersects itself, adjust path or profile selections. */
    SWEEP_SELF_INT,
    /* Some constraints are not applicable to the current external references and have not been solved. */
    SKETCH_UNSOLVABLE_CONSTRAINT,
    /* Operation cannot be completed because source or target instances are not valid. */
    RESTRUCTURE_INVALID_SOURCE_OR_TARGET,
    /* Curve point plane requires a curve and a point. */
    CPLANE_INPUT_CURVE_POINT,
    /* Moving instances had no effect. */
    TRANSFORM_OCCURRENCES_HAD_NO_EFFECT,
    /* Could not create helix. */
    HELIX_FAILED,
    /* This helix type requires selection of a cone or cylinder. */
    HELIX_INPUT_CONE,
    /* The document could not be rendered. The renderer is not available. */
    RENDERER_NOT_AVAILABLE,
    /* The document could not be rendered. A Problem occurred during rendering. */
    RENDERER_FAILED_TO_RENDER,
    /* Expression failed validation. */
    EXPRESSION_FAILED_VALIDATION,
    /* Library version mismatch. */
    VERSION_MISMATCH_ERROR,
    /* Up to next termination problem, consider switching it to up to part. */
    EXTRUDE_UPTO_NEXT_NO_DIVISION,
    /* Mate is between fixed instances. */
    MATE_BETWEEN_FIXED_OCCURRENCES,
    /* Select surfaces or faces to thicken. */
    THICKEN_SELECT_ENTITIES,
    /* Failed to thicken all selections. */
    THICKEN_FAILED,
    /* Successfully upgraded workspace. */
    WORKSPACE_UPGRADE_SUCCESSFUL,
    /* Workspace upgrade failed. */
    WORKSPACE_UPGRADE_FAILED,
    /* No upgrade available. */
    WORKSPACE_UPGRADE_NONE,
    /* Unable to create a circular pattern with this geometry. */
    SKETCH_CIRCULAR_PATTERN_FAILED,
    /* Need to select a face which is not a fillet. */
    DIRECT_EDIT_ALL_FILLET_FACES_SELECTED,
    /* Failed to identify fillet faces. */
    DIRECT_EDIT_FAILED_TO_IDENTIFY_FILLETS,
    /* Parasolid data import failed. */
    PARASOLID_IMPORT_FAILED,
    /* Leader does not have follow mode functionality. */
    FOLLOW_LEADER_HAS_NO_FUNCTIONALITY_ERROR,
    /* Select faces to mirror. */
    MIRROR_SELECT_FACES,
    /* Relation is invalid. */
    RELATION_INVALID_RELATION,
    /* One or more mates in this relation are invalid. */
    RELATION_INVALID_MATE,
    /* Gear relations require two rotational degrees of freedom. */
    GEAR_RELATION_INVALID_MATE_TYPES,
    /* Screw relations require one cylindrical mate. */
    SCREW_RELATION_INVALID_MATE_TYPES,
    /* Rack and pinion relations require one translational degree of freedom and one rotational degree of freedom. */
    RACK_RELATION_INVALID_MATE_TYPES,
    /* Rolling relations require one pin slot mate. */
    ROLLING_RELATION_INVALID_MATE_TYPES,
    /* Linear relations require two translational degrees of freedom. */
    LINEAR_RELATION_INVALID_MATE_TYPES,
    /* Relation overdefines assembly. */
    RELATION_OVERDEFINED,
    /* Relation is not consistent. */
    RELATION_INCONSISTENT,
    /* Mates in relation cannot involve the same instance. */
    RELATION_SAME_OCCURRENCES,
    /* Unable to split selected geometry. */
    SKETCH_SPLIT_FAILED,
    /* A pierce constraint requires a curve or point from the sketch and an external edge. */
    SKETCH_CONSTRAINT_PIERCE_TWO_ENTITIES,
    /* Could not create the pierce constraint. */
    SKETCH_PIERCE_FAILED,
    /* Failed to create face mirror, check input. */
    MIRROR_FACE_FAILED,
    /* Could not mirror selected parts. */
    MIRROR_BODY_FAILED,
    /* You cannot pierce the sketch with a plane. */
    SKETCH_CANNOT_PIERCE_WITH_PLANE,
    WITH_SUPPORT_CODE,
    /* Cannot fillet smooth edges. */
    FILLET_FAIL_SMOOTH,
    /* Failed to fillet selections. */
    FILLET_FAILED,
    /* Cannot chamfer smooth edges. */
    CHAMFER_FAIL_SMOOTH,
    /* Failed to chamfer selections. */
    CHAMFER_FAILED,
    /* Select tool faces to offset. */
    BOOLEAN_OFFSET_NO_FACES,
    /* One or more mated instance is suppressed. */
    MATE_OCCURRENCE_SUPPRESSED,
    /* Instance of mate connector is suppressed. */
    MATECONNECTOR_OCCURRENCE_SUPPRESSED,
    /* Spline points are too close together to create a valid spline. */
    SKETCH_SPLINE_NEW_POINTS_TOO_CLOSE,
    /* Endpoints of a spline cannot be deleted. */
    SKETCH_SPLINE_CANNOT_DELETE_ENDPOINTS,
    /* Cannot delete spline tangent handles. */
    SKETCH_SPLINE_POINT_TO_DELETE_NOT_FOUND,
    /* Failed to insert instance into assembly. */
    ASSEMBLY_INSERT_FAILED,
    /* The sketch pattern could not be created. Please contact support. */
    SKETCH_PATTERN_UNKNOWN_FAILURE,
    /* This pattern would create too much geometry. */
    SKETCH_PATTERN_TOO_LARGE,
    /* A linear pattern must have a distance greater than zero between instances. */
    SKETCH_LINEAR_PATTERN_ZERO_LENGTH,
    /* The two directions in a linear pattern cannot be parallel. */
    SKETCH_LINEAR_PATTERN_PARALLEL_DIRECTIONS,
    /* A circular pattern must have an angle greater than zero between instances. */
    SKETCH_CIRCULAR_PATTERN_ZERO_ANGLE,
    /* The ellipse could not be created with selected points. */
    SKETCH_ELLIPSE_FAILED,
    /* The ellipse was too small to create. */
    SKETCH_ELLIPSE_FAILED_TOO_SMALL,
    /* Could not delete selected parts. */
    DELETE_PARTS_FAILED,
    /* Some selected parts could not be deleted. */
    DELETE_PARTS_PARTIAL,
    /* Radius dimensions require a single circle, arc or ellipse. */
    SKETCH_ELLIPSE_RADIUS_INPUT_ERROR,
    /* A quadrant constraint requires a point and an ellipse. */
    QUADRANT_CONSTRAINT_INPUT,
    /* Could not create quadrant constraint. */
    SKETCH_QUADRANT_FAILED,
    /* Closed splines must have at least three spline points. */
    SKETCH_SPLINE_TOO_FEW_POINTS,
    /* Cannot add/delete points on curve. */
    SKETCH_SPLINE_NOT_INTERPOLATED_SPLINE,
    /* Some spline points could not be deleted. */
    SKETCH_SPLINE_POINTS_NOT_DELETED,
    /* The text rectangle could not be created with selected points. */
    SKETCH_TEXT_RECTANGLE_FAILED,
    /* Failed to recreate derived objects. */
    IMPORT_DERIVED_NO_PARTS,
    /* Select profiles in order from start to finish. */
    LOFT_SELECT_PROFILES,
    /* Select a single sketch region or face per profile. */
    LOFT_PROFILE_SINGLE_FACE,
    /* Select faces or sketch regions as profiles for solid loft. */
    LOFT_PROFILE_SOLID,
    /* Point profiles can only exist as first or last profiles. */
    LOFT_PROFILE_POINT,
    /* Could not create valid profiles from selections. */
    LOFT_PROFILE_FAILED,
    /* Select guides. */
    LOFT_SELECT_GUIDES,
    /* Could not create valid guides from selections. */
    LOFT_GUIDE_FAILED,
    /* Need at least 3 profiles, a closed guide or a closed path for a closed loft. */
    LOFT_PERIODIC_ERROR,
    /* Guide selection does not touch the point profile. */
    LOFT_GUIDE_POINT_INTERSECTION,
    /* All guides should intersect each profile boundary. */
    LOFT_GUIDE_PROFILE_INTERSECTION,
    /* To match vertices between profiles, one vertex from each profile must be selected. */
    LOFT_VERTEX_MATCHING,
    /* Could not determine loft direction. */
    LOFT_DIRECTION_ERROR,
    /* Could not align profiles to loft direction. */
    LOFT_PROFILE_ALIGNMENT,
    /* Guide could not be used to determine loft direction. */
    LOFT_GUIDE_ALIGNMENT,
    /* Could not add internal vertices, split profiles at desired points. */
    LOFT_VERTEX_ADDITIONS,
    /* Could not create loft with given information. Check profile order, guide/profile intersections or end conditions. */
    LOFT_FAILED,
    /* Current selections would create a self-intersecting body. */
    LOFT_INVALID,
    /* For most reliable results, select a guide curve or match profile vertices for alignment. */
    LOFT_ALIGNMENT_INFO,
    /* Some vertices selected for matching are not on profile. */
    LOFT_VERTEX_NOT_ON_PROFILE,
    /* Cannot use faces or regions with inner loops as profiles. */
    LOFT_PROFILE_NO_INNER_LOOPS,
    /* Select at least two profiles, a closed guide curve or a closed path. */
    LOFT_TWO_PROFILES,
    /* Ellipses cannot be offset yet. */
    CANNOT_OFFSET_ELLIPSE,
    /* A symmetry constraint requires a line and two other geometries of the same type. */
    SKETCH_MIRROR_NEEDS_LINE_AND_TWO_OTHERS,
    /* A polygon must have fifty sides or fewer. */
    SKETCH_POLYGON_BAD_SIDE_COUNT,
    /* The entities must be all points or all lines and ellipses. */
    SKETCH_DIRECTIONAL_GROUP_INPUT,
    /* Name is already in use. New view not saved. */
    NAMED_VIEWS_DUPLICATE_NAME,
    /* Could not create the silhouette */
    SILHOUETTE_USE_FAILED,
    /* Highlighted entities have changed due to differing feature libraries. */
    PASTE_SKETCH_METRICS_FAILURE,
    /* Sketch could not be pasted due to differing feature libraries. */
    PASTE_SKETCH_LIBRARY_MISMATCH,
    /* Clipboard is empty. */
    PASTE_SKETCH_CLIPBOARD_EMPTY,
    /* Cannot mirror offset splines. */
    SKETCH_MIRROR_OFFSET_SPLINE,
    /* Cannot mirror curve points. */
    SKETCH_MIRROR_CURVE_POINT,
    /* Cannot use a mix of open and closed guide curves for loft. */
    LOFT_PERIODIC_GUIDE_ERROR,
    /* Select parts to hollow. */
    SHELL_SELECT_PARTS,
    /* One or more mates in this relation do not exist. */
    RELATION_MATE_DOES_NOT_EXIST,
    /* One or more mates in this relation are suppressed. */
    RELATION_MATE_IS_SUPPRESSED,
    /* Variable name must be a letter followed by a string of letters or numbers. */
    VARIABLE_NAME_INVALID,
    /* Some selections could not be used as guides, check that selections are not construction entities. */
    LOFT_GUIDE_INFO,
    /* No hole points selected. */
    HOLE_NO_POINTS,
    /* Failed to compute bounding box of scope. */
    HOLE_FAIL_BBOX,
    /* No target parts selected for hole. */
    HOLE_EMPTY_SCOPE,
    /* None of the holes intersected a part. */
    HOLE_NO_HITS,
    WITH_EXTRA_DATA,
    /* Hole operation split part into multiple parts. */
    HOLE_DISJOINT,
    /* Some entities could not be converted properly. */
    SKETCH_INSERT_DWG_CONVERSION_FAILURE,
    /* The counterbore diameter is smaller than the hole diameter. */
    HOLE_CBORE_TOO_SMALL,
    /* The counterbore depth is deeper than the hole depth. */
    HOLE_CBORE_TOO_DEEP,
    /* The countersink diameter is smaller than the hole diameter. */
    HOLE_CSINK_TOO_SMALL,
    /* The countersink depth is deeper than the hole depth. */
    HOLE_CSINK_TOO_DEEP,
    /* Could not create path, check that selections are not construction entities. */
    SWEEP_PATH_NO_CONSTRUCTION,
    /* The image rectangle could not be created with selected points. */
    SKETCH_IMAGE_RECTANGLE_FAILED,
    /* One or more seed parts are not mated to other supported geometry. */
    ASSEMBLY_REPLICATE_NO_VALID_TARGET,
    /* Selected target did not contain identical geometry. */
    ASSEMBLY_REPLICATE_NO_MATCHING_TARGET,
    /* Failed to apply shape controls to loft, check input. */
    LOFT_SHAPE_CONTROL_FAILED,
    /* Failed to apply shape controls to the start profile. */
    LOFT_START_CONDITIONS_FAILED,
    /* Failed to apply shape controls to the end profile. */
    LOFT_END_CONDITIONS_FAILED,
    /* Start profile has no adjacent faces to match tangent or curvature to. */
    LOFT_NO_FACE_FOR_START_CLAMP,
    /* End profile has no adjacent faces to match tangent or curvature to. */
    LOFT_NO_FACE_FOR_END_CLAMP,
    /* Only planar profiles are valid for Normal/Tangent to profile start condition. */
    LOFT_NO_PLANE_FOR_START_CLAMP,
    /* Only planar profiles are valid for Normal/Tangent to profile end condition. */
    LOFT_NO_PLANE_FOR_END_CLAMP,
    /* End conditions cannot be applied to point profiles. */
    LOFT_NO_CLAMPS_ON_POINT_PROFILE,
    /* Element does not support export in selected format. */
    EXPORT_NOT_IMPLEMENTED,
    /* Cannot make a polygon with zero radius. */
    SKETCH_POLYGON_ZERO_RADIUS_FAIL,
    /* Failed to resolve view reference. */
    DRAWING_FAILED_TO_RESOLVE_VIEW_REFERENCE,
    /* No part found in the part studio after section cut. */
    DRAWING_PARTSTUDIO_EMPTY_AFTER_SECTION_CUT,
    /* Assembly does not contain any visible instances. */
    DRAWING_ASSEMBLY_DOES_NOT_CONTAIN_VISIBLE_INSTANCES,
    /* No instances left in assembly after section cut. */
    DRAWING_ASSEMBLY_EMPTY_AFTER_SECTION_CUT,
    /* Failed to generate view. */
    DRAWING_VIEW_GENERATION_FAILED,
    /* A slot could not be created for the selected curve. */
    SKETCH_SLOT_FAILURE,
    /* Some slots could not be created for the selected curves. */
    SKETCH_SLOT_PARTIAL_FAILURE,
    /* Value is missing required units. */
    NO_UNIT,
    /* Operation cannot be completed because one or more source instances are not valid. */
    RESTRUCTURE_INVALID_SOURCE,
    /* Operation cannot be completed because target is not valid. */
    RESTRUCTURE_INVALID_TARGET,
    /* Minimum mate limit should be smaller than maximum limit. */
    MATE_MIN_MAX_LIMIT_VIOLATION,
    /* Could not load document during REST assembly operation. */
    REST_ASSEMBLY_GET_DOCUMENT_FAILED,
    /* Operation attempted to insert an instance of unknown type during REST assembly operation. */
    REST_ASSEMBLY_UNKNOWN_INSERTABLE_TYPE,
    /* Setup exception occurred during REST assembly modification. */
    REST_ASSEMBLY_SETUP_EXCEPTION,
    /* Failure during setup of REST assembly operation */
    REST_ASSEMBLY_BEGIN_OPERATION_FAILED,
    /* Failure during insertion of REST assembly operation */
    REST_ASSEMBLY_INSERT_INSTANCE_FAILED,
    /* Failure committing REST insertion operation. */
    REST_ASSEMBLY_COMMIT_OPERATION_FAILED,
    /* Failure closing client state during REST assembly operation */
    REST_ASSEMBLY_CLOSE_CLIENT_FAILED,
    /* List of instances is NULL during REST assembly operation. */
    REST_ASSEMBLY_NULL_OCCURRENCES,
    /* One or more instances are null or empty during REST assembly operation. */
    REST_ASSEMBLY_EMPTY_OCCURRENCE,
    /* The transform is not 9, 12 or 16 entries long during REST assembly operation. */
    REST_ASSEMBLY_TRANSFORM_WRONG_SIZE,
    /* List of instances is empty during REST assembly operation. */
    ASSEMBLY_EMPTY_OCCURRENCE_LIST,
    /* Failed to provide a transform matrix. */
    ASSEMBLY_NULL_TRANSFORM,
    /* Provided transform matrix is not a rigid rotation. */
    ASSEMBLY_TRANSFORM_NOT_RIGID,
    /* Attempt to transform a fixed instance(s) failed. */
    ASSEMBLY_CANNOT_TRANSFORM_FIXED_OCCURRENCE,
    /* Unable to apply transform. Instance(s) may be constrained. */
    ASSEMBLY_TRANSFORM_FAILED,
    /* Could not find given instance. */
    ASSEMBLY_OCCURRENCE_NOT_FOUND,
    /* Seed parts must only have a single mate to other geometry. */
    ASSEMBLY_REPLICATE_MULTIPLE_VALID_TARGET,
    /* A target scope must be selected. */
    ASSEMBLY_REPLICATE_NO_TARGET_SELECTED,
    /* Line-Angle plane requires a reference line and a point, plane, or axis. */
    CPLANE_INPUT_LINE_ANGLE2,
    /* Could not determine the reference direction for a Line-Angle plane. */
    CPLANE_DEGENERATE_SELECTION,
    /* Select an additional point, plane, or axis to specify where the angle is measured from. */
    CPLANE_SELECT_LINE_ANGLE_REFERENCE,
    /* One or more seed instances are not valid. */
    ASSEMBLY_REPLICATE_INVALID_SEED_INSTANCE,
    /* Variables are not supported for pattern counts. */
    CANNOT_USE_VARIABLES_IN_SKETCH_PATTERNS,
    /* Cannot mirror offset ellipses. */
    SKETCH_MIRROR_OFFSET_ELLIPSE,
    /* Creation of linked document reference failed */
    EXTERNAL_REFERENCE_FAILED_TO_CREATE,
    /* Selected entities do not split selected faces. */
    SPLIT_FACE_NO_CHANGE,
    /* Could not intersect the selected face. */
    SKETCH_INTERSECTION_FAILED,
    /* Could not intersect the selected faces. */
    SKETCH_INTERSECTION_MULTIPLE_FAILED,
    /* Could not intersect some of the selected faces. */
    SKETCH_INTERSECTION_PARTIAL_FAILED,
    /* Feature id in path does not match body */
    FEATURE_ID_IN_PATH_DOES_NOT_MATCH_BODY,
    /* Feature not found */
    FEATURE_NOT_FOUND,
    /* Feature does not match */
    FEATURE_DOES_NOT_MATCH,
    /* Feature has invalid type */
    FEATURE_HAS_INVALID_TYPE,
    /* Feature does not match its feature spec */
    FEATURE_DOES_NOT_MATCH_ITS_FEATURE_SPEC,
    /* Bad serialization version */
    FEATURE_BAD_SERIALIZATION_VERSION,
    /* Wrong serialization version */
    FEATURE_WRONG_SERIALIZATION_VERSION,
    /* Invalid rollback index */
    FEATURE_INVALID_ROLLBACK_INDEX,
    /* Error in input */
    FEATURE_ERROR_IN_INPUT,
    /* A concurrent update interfered with this operation */
    FEATURE_CONCURRENCY_ERROR,
    /* The change would result in the feature list becoming invalid */
    FEATURE_CHANGE_BREAKS_MODEL,
    /* The feature nodeIds do not correspond to their original nodes */
    FEATURE_NODE_IDS_INVALID,
    /* The specified rollback index is not valid */
    ROLLBACK_INDEX_INVALID,
    /* Feature did not create any solids. */
    FEATURE_NO_SOLIDS,
    /* External geometry included in the sketch has changed types. */
    SKETCH_EXTERNAL_GEOMETRY_MISMATCH,
    /* Too many hole locations were selected, maximum: 100. */
    HOLE_EXCEEDS_MAX_LOCATIONS,
    /* An empty string was submitted for sketch text. */
    SKETCH_TEXT_IS_EMPTY,
    /* Selected file contains too many entities to import. */
    SKETCH_INSERT_DWG_MAX_ENTITIES_EXCEEDED,
    /* The tap drill diameter is greater than the hole diameter. */
    HOLE_TAP_DIA_TOO_LARGE,
    /* Empty or invalid body parameters. */
    ASSEMBLY_EMPTY_BODY,
    /* File cannot be imported. Invalid parts. */
    SIMPLIFY_BODY_FAILED,
    /* Name contains invalid characters. New view not saved. */
    INVALID_VIEW_NAME,
    /* Select features to pattern. */
    PATTERN_SELECT_FEATURES,
    /* Select features to mirror. */
    MIRROR_SELECT_FEATURES,
    /* Could not pattern selected features. */
    PATTERN_FEATURE_FAILED,
    /* Failed to transform selected geometry. */
    SKETCH_TRANSFORM_FAILED,
    /* Tangent mate needs two entities. */
    TANGENT_MATE_TWO_ENTITIES_NEEDED,
    /* The last part could not be determined. Blind in last requires full intersections with at least two parts. */
    HOLE_CANNOT_DETERMINE_LAST_BODY,
    /* Operation cannot be completed as it would modify a saved version. */
    RESTRUCTURE_CANNOT_MODIFY_SAVED_VERSION,
    /* Linked document references require a version identifier */
    REST_ASSEMBLY_EXTERNAL_REFERENCE_REQUIRES_VERSION,
    /* Microversions may not be used with linked document references */
    REST_ASSEMBLY_EXTERNAL_REFERENCE_DISALLOWS_MICROVERSION,
    /* A version Id may be used only with linked document references */
    REST_ASSEMBLY_VERSION_SUPPORTED_ONLY_FOR_EXTERNAL_REFERENCES,
    /* Lock faces selection is invalid. */
    SWEEP_BAD_LOCK_FACES,
    /* Cannot set construction on sketch text. */
    SKETCH_TEXT_CANNOT_BE_CONSTRUCTION,
    CUSTOM_ERROR,
    /* Geometric configuration cannot be flattened/folded. Usually because and edge or vertex is not a precise offset of another. */
    BEND_BAD_CONFIGURATION,
    /* Flatten/fold failed because the number of seed entities is incorrect */
    BEND_WRONG_NUMBER_OF_ENTITIES,
    /* Incorrect or bad curves selected to bend opertion. */
    BEND_BAD_CURVES,
    /* An error occurred during the bend operation. */
    BEND_GENERAL_ERROR,
    /* No edges were selected for the bend edge operation. */
    BEND_EDGE_NO_EDGES,
    /* No seed entity was selected for the bend edge operation. */
    BEND_EDGE_NO_SEED_ENTITY,
    /* No body was selected for the extend sheet body operation. */
    EXTEND_SHEET_BODY_NO_BODY,
    /* No faces were selected for the extract surface operation. */
    EXTRACT_SURFACE_NO_FACES,
    /* No edges were selected for the flatten operation. */
    FLATTEN_NO_EDGES,
    /* No faces were selected for the flatten operation. */
    FLATTEN_NO_FACES,
    /* No bodies were selected for the fold operation. */
    FOLD_NO_BODIES,
    /* No faces were selected for the prepare for bending operation. */
    BEND_PREP_NO_FACES,
    /* No bodies were selected for the prepare for bending operation. */
    BEND_PREP_NO_BODIES,
    /* An error occurred while locating postions for break edges. */
    BEND_PREP_ERROR_FINDING_EDGE_LOCATIONS,
    /* An error occurred while imprinting break edges. */
    BEND_PREP_ERROR_IMPRINTING_EDGES,
    /* Animation start value must be less than or equal to the end value. */
    ASSEMBLY_ANIMATE_MATE_START_AFTER_END,
    /* No mate selected. */
    ASSEMBLY_ANIMATE_NO_MATE,
    /* Suppressed mates are not supported for animate. */
    ASSEMBLY_ANIMATE_MATE_SUPPRESSED,
    /* Geometry is not supported for tangent mate. */
    TANGENT_MATE_GEOMETRY_NOT_SUPPORTED,
    /* The dimension evaluated to an infinite value and cannot be satisfied. */
    SKETCH_DIMENSION_INFINITY,
    /* Not all inputs are solids. */
    BOLEAN_INPUTS_NOT_SOLID,
    /* One or more input faces are not rectangles. */
    FACE_IS_NOT_RECTANGLE,
    /* The hole destroyed one or more parts. */
    HOLE_DESTROY_SOLID,
    /* This helix type requires selection of a circle or arc. */
    HELIX_INPUT_CIRCLE,
    /* Scaling a non-mesh import is not allowed. */
    IMPORT_SCALING_NON_MESH_DATA,
    /* Cannot evaluate face tangent planes for meshes. */
    EVALUATE_FACE_TANGENT_FOR_MESHES,
    /* Cannot compute centroid. */
    CANNOT_COMPUTE_CENTROID,
    /* Cannot evaluate the dimension of an entity. */
    CANNOT_EVALUATE_DIMENSION,
    /* Cannot import mesh files to your current document. */
    CANNOT_IMPORT_MESH,
    /* An ellipse needs a non-zero major axis. */
    SKETCH_ELLIPSE_ZERO_AXIS,
    /* Scale uniformly requires a vertex. */
    TRANSFORM_SCALE_UNIFORMLY,
    /* Transform by mate connectors requires two mate connectors. */
    TRANSFORM_MATE_CONNECTORS,
    /* The specified element is not an assembly. */
    ASSEMBLY_WRONG_ELEMENT_TYPE,
    /* The specified element does not exist. */
    ASSEMBLY_ELEMENT_NOT_FOUND,
    /* Edit cannot be applied. */
    SHEET_METAL_TABLE_UNKNOWN_ERROR,
    /* Sheet metal model could not be rebuilt after applying edits. Edits have not been applied. */
    SHEET_METAL_TABLE_REGEN_ERROR,
    /* Cell is read only. */
    SHEET_METAL_TABLE_READ_ONLY,
    /* Invalid pattern type. */
    ASSEMBLY_PATTERN_INVALID_TYPE,
    /* Failed to compute pattern direction. */
    ASSEMBLY_PATTERN_DIRECTION_ERROR,
    /* Linear pattern distance must be positive. */
    ASSEMBLY_PATTERN_NONPOSITIVE_LINEAR_DISTANCE,
    /* Circular pattern angle must be positive. */
    ASSEMBLY_PATTERN_NONPOSITIVE_ANGLE,
    /* Select one or more instances. */
    ASSEMBLY_PATTERN_INVALID_SEED,
    /* Faied to determine pattern reference mate connector. */
    ASSEMBLY_PATTERN_INVALID_REFERENCE_MATE_CONNECTOR,
    /* Failed to restore feature. */
    RESTORE_FEATURE_FAILED,
    /* Some faces are not owned by the parts. */
    FACES_NOT_OWNED_BY_PARTS,
    /* Some edges are not owned by the parts. */
    EDGES_NOT_OWNED_BY_PARTS,
    /* Sheet metal model cannot be built. */
    SHEET_METAL_REBUILD_ERROR,
    /* This feature cannot be used on sheet metal parts. */
    SHEET_METAL_INPUT_BODY_SHOULD_NOT_BE_SHEET_METAL,
    /* Selected parts cannot be recognized as sheet metal. */
    SHEET_METAL_CANNOT_RECOGNIZE_PARTS,
    /* Sheet metal model contains faces that cannot be thickened. */
    SHEET_METAL_CANNOT_THICKEN,
    /* Only planar faces can be converted to sheet metal. */
    SHEET_METAL_CONVERT_PLANE,
    /* Failed to compute pattern axis. */
    ASSEMBLY_PATTERN_AXIS_ERROR,
    /* Select sketch profiles to create the ribs. */
    RIB_NO_PROFILES,
    /* Select parts where the rib will be fitted into. */
    RIB_NO_PARTS,
    /* Failed to create a rib from a selected profile. */
    RIB_PROFILE_FAILED,
    /* Selected profile did not produce a rib body.  Make sure the rib direction and alignment are correct. */
    RIB_BODY_FAILED,
    /* None of the ribs intersected a part. */
    RIB_NO_INTERSECTIONS,
    /* Failed to merge ribs into parts. */
    RIB_MERGE_FAILED,
    /* Failed to save named position. */
    ASSEMBLY_NAMED_POSITIONS_SAVE_FAILED,
    /* Mates or parts have changed. Position may not be exact. */
    ASSEMBLY_NAMED_POSITIONS_LOAD_FAILED,
    /* A non-empty assembly is required in order to save a named position. */
    ASSEMBLY_NAMED_POSITIONS_NO_MATES_TO_SAVE,
    /* Named position not found. */
    ASSEMBLY_NAMED_POSITIONS_POSITION_NOT_FOUND,
    /* Create sphere failed. */
    SPHERE_FAILED,
    /* This document contains assembly patterns, which are not supported by your version of Onshape on this mobile device. */
    ASSEMBLY_PATTERN_NOT_SUPPORTED,
    /* Some mate values could not be applied when loading the named position. */
    ASSEMBLY_NAMED_POSITIONS_PARTIAL_LOAD_FAILURE,
    /* Saved mate values were applied, but the number of mates has changed since the position was saved. */
    ASSEMBLY_NAMED_POSITIONS_LOAD_SUCCEEDED_WITH_EXTRA_MATES,
    /* None of the mates in the named position could be found. */
    ASSEMBLY_NAMED_POSITIONS_SAVED_MATE_NOT_FOUND_ON_LOAD,
    /* Selected parts should be from the same active sheet metal model. */
    SHEET_METAL_SINGLE_MODEL_NEEDED,
    /* Select a joint from an active sheet metal model. */
    SHEET_METAL_ACTIVE_JOIN_NEEDED,
    /* Failed to resolve instance. */
    INSTANCE_QUERY_FAILED,
    /* Selected edges should be from active sheet metal models. */
    SHEET_METAL_ACTIVE_EDGE_NEEDED,
    /* Select edges to flange. */
    SHEET_METAL_FLANGE_NO_EDGES,
    /* This feature does not support meshes. */
    MESH_NOT_SUPPORTED,
    /* Active sheet metal models are not allowed. */
    SHEET_METAL_PARTS_PROHIBITED,
    /* Cannot evaluate the variable. */
    VARIABLE_CANNOT_EVALUATE,
    /* The section cut location results in invalid geometry. */
    DRAWING_ASSEMBLY_INVALID_SECTION_CUT,
    /* The section cut location results in invalid geometry. */
    DRAWING_PARTSTUDIO_INVALID_SECTION_CUT,
    /* Sheet metal model cannot be unfolded. */
    SHEET_METAL_COULD_NOT_UNFOLD,
    /* Error regenerating: a parameter is out of range. */
    PARAMETER_OUT_OF_RANGE,
    /* Edge between tangent walls can not be a bend. */
    SHEET_METAL_NO_0_ANGLE_BEND,
    /* Flat joint cannot be edited. */
    SHEET_METAL_FLAT_RIP_NO_EDIT,
    /* Joint style can not be changed to Flat. */
    SHEET_METAL_CANT_CHANGE_TO_FLAT,
    /* Error regenerating. */
    PARAMETER_PRECONDITION_FAILED,
    /* Error regenerating. */
    PARAMETER_SYNTAX_ERROR,
    /* Only Remove can be used with sheet metal. Add Finish sheet metal model to allow other operations. */
    SHEET_METAL_CAN_ONLY_REMOVE,
    /* Only Subtract can be used with sheet metal. Add Finish sheet metal model to allow other operations. */
    SHEET_METAL_CAN_ONLY_SUBTRACT,
    /* The feature Id does not identify a sketch. */
    REST_ASSEMBLY_INVALID_FEATURE,
    /* The part Id does not identify a solid or surface. */
    REST_ASSEMBLY_INVALID_BODY_TYPE,
    /* Failed to move tab. You must have WRITE and LINK permission to the target document. */
    PARTING_OUT_TARGET_READONLY,
    /* Model bend radius cannot be used for selections from multiple sheet metal models. */
    SHEET_METAL_MULTI_SM_DEFAULT_RADIUS,
    /* Flange cannot not be created with selected alignment. */
    SHEET_METAL_FLANGE_FAIL_ALIGNMENT,
    /* Flange cannot be created up to selected entity. */
    SHEET_METAL_FLANGE_FAIL_UP_TO,
    /* Cannot resolve limit entity for flange end condition. */
    SHEET_METAL_FLANGE_FAIL_UP_TO_ENTITY,
    /* Flange cannot be created. */
    SHEET_METAL_FLANGE_FAIL,
    /* Flange direction is opposite of limit entity. */
    SHEET_METAL_FLANGE_FAIL_LIMIT_OPP_FLANGE,
    /* Bends or side faces cannot be split. */
    CANT_SPLIT_SHEET_METAL_BEND_FACE,
    /* Context assembly instance is not valid. */
    IN_CONTEXT_INSTANCE_INVALID_TARGET,
    /* Collision in sheet metal model. */
    SHEET_METAL_SELF_INTERSECTING_MODEL,
    /* Collision in sheet metal flat pattern. */
    SHEET_METAL_SELF_INTERSECTING_FLAT,
    /* Rip style is only supported for 90 degree joints. */
    SHEET_METAL_NON_90_BUTT,
    /* Cannot apply selected style to rip. */
    SHEET_METAL_RIP_STYLE_ERROR,
    /* Only feature pattern can be used to pattern mate connectors. */
    CANNOT_USE_MATECONNECTORS_IN_PATTERN,
    /* Mate connectors can not be copied. */
    CANNOT_COPY_MATECONNECTORS,
    /* Only Offset can be used with sheet metal. Add Finish sheet metal model to allow other operations. */
    SHEET_METAL_CAN_ONLY_OFFSET,
    /* Only modifiable entity is allowed. */
    MODIFIABLE_ENTITY_ONLY,
    /* Context assembly is deleted. In-context features have not been updated. */
    IN_CONTEXT_UPDATE_DELETED_ASSEMBLY,
    /* Context assembly primary instance is not set. Finish inserting part or set primary instance of context before updating the assembly context. */
    IN_CONTEXT_UPDATE_EMPTY_INSTANCE,
    /* One or more in-context references failed to update. */
    IN_CONTEXT_UPDATE_INVALID_SOURCE,
    /* Context assembly primary instance is not valid. Set primary instance of context before updating the assembly context. */
    IN_CONTEXT_UPDATE_INVALID_TARGET,
    /* Sheet metal features cannot be patterned. */
    SHEET_METAL_NO_FEATURE_PATTERN,
    /* The definition of this custom feature was not found. */
    CUSTOM_FEATURE_DEFINITION_NOT_FOUND,
    /* Select edges to add bends. */
    SHEET_METAL_START_SELECT_BENDS,
    /* Subsequent features modifying the selected sheet metal model will not affect the flat pattern or table values. */
    SHEET_METAL_END_DONE,
    /* Edges do not form a continuous path. */
    PATH_EDGES_NOT_CONTINUOUS,
    /* An internal edge cannot be used to create a joint. */
    SHEET_METAL_RIP_FAIL_INTERNAL_EDGE,
    /* Cannot create a rip on wall. */
    SHEET_METAL_RIP_FAIL,
    /* Cannot connect multiple parts to create a joint. */
    SHEET_METAL_RIP_MULTI_BODY,
    /* Cannot create joint between non planar walls. */
    SHEET_METAL_RIP_FAIL_NON_PLANAR,
    /* Select a path to pattern along. */
    PATTERN_CURVE_NO_EDGES,
    /* Sheet metal corner not found. */
    SHEET_METAL_RIP_NO_CORNER,
    /* Select pairs of vertices. */
    SHEET_METAL_RIP_EVEN,
    /* Cannot find a unique, planar wall to split. */
    SHEET_METAL_RIP_WALL_NOT_FOUND,
    /* Same underlying vertex selected in pair. */
    SHEET_METAL_RIP_SAME_VERTEX,
    /* Select enough vertex pairs to create at least one new face. */
    SHEET_METAL_RIP_NEED_MORE_VERTICES,
    /* Cannot add joint joining selected entities. */
    SHEET_METAL_MAKE_JOINT_FAIL,
    /* For best results ensure that the path start point and pattern seed are touching. */
    CURVE_PATTERN_START_OFF_PATH,
    /* Failed to load part studio */
    PART_LOAD_FAILED,
    /* Cannot move non-planar side faces of sheet metal. */
    SHEET_METAL_MOVE_NOT_PLANAR,
    /* Cannot create a joint between adjacent faces. */
    SHEET_METAL_JOINT_FAIL_ADJACENT_FACES,
    /* The selected Parasolid version does not support the current geometry contents. */
    WRONG_PARASOLID_VERSION,
    /* Selected edges should be from the same active sheet metal model. */
    SHEET_METAL_SINGLE_MODEL_NEEDED_EDGES,
    /* The instance document Id must be provided. */
    REST_ASSEMBLY_MISSING_INSTANCE_DOCUMENT_ID,
    /* The instance element Id must be provided. */
    REST_ASSEMBLY_MISSING_INSTANCE_ELEMENT_ID,
    /* Model faces intersect. */
    FACE_CLASH,
    /* For best results ensure that the path and pattern seed are touching. */
    CURVE_PATTERN_START_OFF_CLOSED_PATH,
    /* Cut should either not touch a joint or cut through it. */
    SHEET_METAL_CUT_JOINT,
    /* No valid parts were found for STL export. */
    EXPORT_STL_NO_PARTS,
    /* Name is required and cannot exceed 256 characters. */
    INPUT_NAME_TOO_LONG,
    /* File was imported but contained faults. */
    IMPORT_BODY_FAILED_CHECK,
    /* Parts were derived but contained faults. */
    DERIVED_BODIES_HAVE_FAULTS,
    /* Some selected features affecting sheet metal models cannot be patterned. */
    SHEET_METAL_BLOCKED_PATTERN,
    /* Cannot create flange on an edge that is a joint, fillet, chamfer, or internal to the sheet metal model. */
    SHEET_METAL_FLANGE_INTERNAL,
    /* Cannot create sheet metal model of requested thickness. */
    SHEET_METAL_TOO_THICK,
    /* Selection is a bend relief, not a corner. Use the Bend relief feature to modify this selection. */
    SHEET_METAL_BEND_END_NOT_A_CORNER,
    /* The corner is not closed or has more than two adjacent bends. Closed relief can not be applied. */
    SHEET_METAL_NOT_A_CLOSED_CORNER,
    /* Selection is a corner, not a bend relief. Use the Corner feature to modify this selection. */
    SHEET_METAL_CORNER_NOT_A_BEND_END,
    /* Only open sketch profiles can be used when rib direction is parallel to sketch plane. */
    RIB_ONLY_OPEN_PROFILES,
    /* The tab no longer exists in the original workspace. */
    TAB_NO_LONGER_EXISTS,
    /* Could not create construction line. */
    CLINE_FAILED,
    /* Operation attempted to modify an unmodifiable entity. */
    ILLEGAL_MODIFICATION,
    /* Value cannot be applied. */
    ASSEMBLY_MATE_VALUE_SET_FAILED,
    /* Selected offset is too large; decrease the value. */
    EXTRUDE_OFFSET_TOO_DEEP,
    /* Faces of bends cannot be moved. */
    SHEET_METAL_CANNOT_MOVE_BEND_EDGE,
    /* Limit face must be parallel to face being moved. */
    UP_TO_FACE_NOT_PARALLEL,
    /* When moving up to a plane, first selected face must be planar. */
    TRANSLATION_FACE_NOT_PLANAR,
    /* First face can not be moved to limit entity. */
    MOVE_FACE_NO_INTERSECTION,
    /* For best results ensure that the path intersects each profile or its plane. */
    SWEEP_PATH_PROFILE_NO_INTERSECTION,
    /* Select faces, surfaces, or sketch regions to offset from. */
    DIRECT_EDIT_MOVE_FACE_CREATE_SELECT,
    /* In-context faces are not connected. To thicken multiple in-context faces first copy the part then thicken faces of the copy. */
    SHEET_METAL_THICKEN_IN_CONTEXT_INFO,
    /* Rho dimensions cannot be deleted. */
    CANNOT_DELETE_RHO_DIMENSION,
    /* Rho must be between 0 and 1 exclusive. */
    INVALID_RHO,
    /* Could not find dimension rho. */
    SKETCH_RHO_DIM_NOT_FOUND,
    /* Failed to add conic to the sketch. */
    SKETCH_CONIC_FAILED,
    /* Tab name cannot exceed 256 characters. */
    TAB_NAME_TOO_LONG,
    /* Cannot delete all faces of a part or surface. */
    DIRECT_EDIT_DELETE_FACE_ALL_FACES,
    /* Failed to break selected corners. */
    SHEET_METAL_CORNER_BREAK_FAILED,
    /* Surfaces do not share an edge and cannot be added. */
    BOOLEAN_NO_TARGET_SURFACE,
    /* The definition of the B-spline is invalid. */
    BAD_BSPLINECURVE_DEFINITION,
    /* The B-spline definition must define a 3d curve. */
    REQUIRE_3D_BSPLINECURVE_DATA,
    /* The B-spline definition is marked as periodic but the data does not define a periodic curve. */
    PERIODIC_BSPLINECURVE_NOT_CLOSED,
    /* The B-spline definition is marked as periodic but would not be smooth at the join. */
    PERIODIC_BSPLINECURVE_NOT_SMOOTH,
    /* The B-spline definition contains negative weights. */
    RATIONAL_BSPLINECURVE_WEIGHT_NEGATIVE,
    /* The B-spline definition contains a G1 discontinuity. */
    BSPLINECURVE_NOT_G1,
    /* Select a single vertex for both sides of the bridge. */
    BRIDGING_CURVE_VERTEX_BOTH_SIDES,
    /* Select a single edge for tangency matching. */
    BRIDGING_CURVE_ONE_EDGE_EACH_SIDE,
    /* The vertex must lie at the end of the edge. */
    BRIDGING_CURVE_VERTEX_AT_END_OF_EDGE,
    /* The sketches cannot be parallel. */
    PROJECT_CURVES_PARALLEL_PLANES,
    /* The curves in each list must be from the same sketch. */
    PROJECT_CURVES_DIFFERENT_SKETCHES,
    /* Select a sheet metal part. */
    SHEET_METAL_SELECT_PART,
    /* Variable has not been set. */
    VARIABLE_NOT_FOUND,
    /* Cannot edit a fixed conic. */
    CANNOT_EDIT_FIXED_CONIC,
    /* The edges can only meet at their ends, overlapping or crossing edges are prohibited. */
    EXTRACT_WIRES_OVERLAPPING_EDGES,
    /* More than two edges cannot meet at a single point. */
    EXTRACT_WIRES_NON_MANIFOLD,
    /* At least one edge is required. */
    EXTRACT_WIRES_NEEDS_EDGES,
    /* Select at least two vertices to make a spline. */
    SPLINE_TWO_POINTS,
    /* Select at least three vertices to make a closed spline. */
    CLOSED_SPLINE_THREE_POINTS,
    /* Select exactly one entity for end condition. */
    TANGENCY_ONE_EDGE,
    /* Cannot evaluate tangency for end condition. */
    FIT_SPLINE_CANNOT_EVALUATE_END_CONDITION,
    /* Interpolation points cannot be repeated. */
    FIT_SPLINE_REPEATED_POINT,
    /* A feature id is required */
    FEATURE_ID_REQUIRED,
    PARAMETER_NOT_FOUND,
    PARAMETER_DOES_NOT_MATCH_ITS_FEATURE_SPEC,
    /* Fillet and chamfer not allowed on active sheet metal models. */
    SHEET_METAL_CORNER_BREAK_DISABLED,
    /* Can only create a circular cross section constant fillet on an active sheet metal model. */
    SHEET_METAL_FILLET_NO_CONIC,
    /* Cannot create a two distance chamfer on an active sheet metal model. */
    SHEET_METAL_CHAMFER_NO_TWO_OFFSETS,
    /* Cannot create a distance and angle chamfer on an active sheet metal model. */
    SHEET_METAL_CHAMFER_NO_OFFSET_ANGLE,
    /* Can only create an equal distance chamfer on an active sheet metal model. */
    SHEET_METAL_CHAMFER_MUST_BE_EQUAL_OFFSETS,
    /* Selected entities should be from active sheet metal models. */
    SHEET_METAL_ACTIVE_ENTITY_NEEDED,
    /* Selected sheet metal entities do not specify a corner. */
    SHEET_METAL_CORNER_BREAK_NOT_A_CORNER,
    /* Selected corner does not have an associated wall. */
    SHEET_METAL_CORNER_BREAK_NO_WALL,
    /* Cannot apply a corner break to a bend, corner, or existing corner break. */
    SHEET_METAL_CORNER_BREAK_VERTEX_NOT_FREE,
    /* Cannot apply a corner break to an existing corner or corner break. */
    SHEET_METAL_CORNER_BREAK_ATTRIBUTE_EXISTS,
    /* Start magnitude cannot be 0. */
    FIT_SPLINE_ZERO_START_MAGNITUDE,
    /* End magnitude cannot be 0. */
    FIT_SPLINE_ZERO_END_MAGNITUDE,
    /* Select corners to break. */
    SHEET_METAL_CORNER_BREAK_SELECT_ENTITIES,
    /* Select a vertex or mate connector to terminate the extrude. */
    EXTRUDE_SELECT_TERMINATING_VERTEX,
    /* Select laminar edges or curves as fill surface boundary. */
    FILL_SURFACE_NO_EDGES,
    /* A boundary edge can have only one type of continuity constraint. */
    FILL_SURFACE_DOUBLE_SELECTION,
    /* Cannot create fill surface between multiple loops. */
    FILL_SURFACE_MULTI_LOOP,
    /* Boundary edges selected do not form a closed loop. */
    FILL_SURFACE_OPEN_LOOP,
    /* Cannot create fill surface with selected boundary conditions. Continuity requirement might be too high. */
    FILL_SURFACE_FAIL,
    /* Cannot add surface to existing geometry. */
    FILL_SURFACE_ATTACH_FAIL,
    /* Could not meet curvature constraints at highlighted points. */
    FILL_SURFACE_G2_FAIL,
    /* Could not satisfy all guide constraints. */
    FILL_SURFACE_VERTEX_INTERPOLATION_FAIL,
    /* Loft path must be a contiguous set of curves. */
    LOFT_SPINE_DISJOINT_PATH,
    /* Loft path is self intersecting. */
    LOFT_SPINE_SELF_INTERSECTING_PATH,
    /* Loft path must pass through all the profiles or their planes. */
    LOFT_SPINE_PATH_PROFILE_NO_INTERSECTION,
    /* Failed to generate intermediate sections. */
    LOFT_SPINE_FAILED_XSECTIONS,
    /* Guide curves cannot be used with point profiles when adding intermediate sections using path. */
    LOFT_SPINE_GUIDE_WITH_POINT_PROFILE,
    /* Profiles are not in order along the loft path. */
    LOFT_SPINE_PROFILES_NOT_IN_ORDER,
    /* Select entities for loft path. */
    LOFT_SELECT_SPINE,
    /* Document not found. */
    DOCUMENT_NOT_FOUND,
    /* Element not found. */
    ELEMENT_NOT_FOUND,
    /* Selections do not enclose a region. */
    ENCLOSE_NO_REGION,
    /* Failed to merge resulting parts. */
    ENCLOSE_CANNOT_MERGE_REGIONS,
    /* Failed to create a part from enclosed region. */
    ENCLOSE_CANNOT_CREATE_SOLID,
    /* Select faces, parts, or planes to form a solid. */
    ENCLOSE_NOTHING_SELECTED,
    /* Enclose failed for unknown reason. */
    ENCLOSE_UNKNOWN_ERROR,
    /* You are not authorized to access this resource. */
    ACCESS_NOT_ALLOWED,
    /* Cannot have more than 3 guides when using path. */
    LOFT_SPINE_TOO_MANY_GUIDES,
    FEATURE_INVALID_NAMESPACE,
    FEATURE_NULL_NOT_ALLOWED,
    /* The dimension value is outside its limits. */
    SKETCH_DIMENSION_LIMIT_ERROR,
    /* Cannot create tangency or curvature constraint for a curve selection. */
    FILL_SURFACE_WIRE_CONTINUITY_MISMATCH,
    /* Cannot create tangency or curvature constraint for an internal edge. */
    FILL_SURFACE_INTERNAL_CONTINUITY_MISMATCH,
    /* Unsuitable end condition. */
    LOFT_START_OR_END_CONDITIONS_FAILED,
    /* Magnitude of end condition has no effect since it is determined by the guide curve. */
    LOFT_START_OR_END_CONDITIONS_MAGNITUDE_NO_EFFECT,
    /* Leader is in a tab that does not support follow mode. */
    FOLLOW_LEADER_IS_IN_UNFOLLOWABLE_TAB,
    /* The selected end condition or guide constraint may be in conflict. */
    LOFT_START_OR_END_CONDITIONS_WITH_GUIDES_FAILED,
    /* The transformGroups array must not be null. */
    REST_ASSEMBLY_MISSING_TRANSFORM_GROUPS,
    /* The transformGroups array must not contain null instances. */
    REST_ASSEMBLY_NULL_TRANSFORM_GROUP,
    /* The transformGroup instances array must not be null. */
    REST_ASSEMBLY_NULL_TRANSFORM_GROUP_INSTANCES,
    /* The transformGroup instances array must not contain null instances. */
    REST_ASSEMBLY_NULL_TRANSFORM_GROUP_INSTANCE,
    /* A guide has no adjacent faces to match tangent or curvature to. */
    LOFT_NO_FACE_FOR_GUIDE_CLAMP,
    /* Internal guides cannot have a specified continuity. */
    LOFT_NO_CONTINUITY_CONDITION_AT_INTERNAL_GUIDE,
    SKETCH_CONSTRAINT_NOT_FOUND,
    SKETCH_CONSTRAINT_PARAMETER_NOT_FOUND,
    SKETCH_ENTITY_NOT_FOUND,
    SKETCH_ENTITY_PARAMETER_NOT_FOUND,
    FEATURE_PARAMETER_TYPE_MISMATCH,
    FEATURE_CONFIGURED_PARAMETER_NO_VALUES,
    FEATURE_CONFIGURED_PARAMETER_INCONSISTENT_TYPES,
    /* Cannot create automatic miter between selected edges. */
    SHEET_METAL_FLANGE_FAIL_AUTO_MITER,
    /* Flange edge and alignment edge cannot be parallel. */
    SHEET_METAL_FLANGE_FAIL_PARALLEL_EDGE,
    /* Flange edge and alignment plane must be parallel. */
    SHEET_METAL_FLANGE_FAIL_PARALLEL_PLANE,
    /* Flange edge and alignment direction cannot be parallel. */
    SHEET_METAL_FLANGE_FAIL_PARALLEL_DIRECTION,
    /* Cannot create a flange with a 0 degree bend angle. */
    SHEET_METAL_FLANGE_FAIL_NO_BEND,
    /* Select an entity to align to. */
    SHEET_METAL_FLANGE_NO_PARALLEL_ENTITY,
    /* Select an entity to align from. */
    SHEET_METAL_FLANGE_NO_DIRECTION_ENTITY,
    /* Tab can only be added to a free edge or rip. */
    SHEET_METAL_TAB_NO_BEND,
    /* No surface selected in merge scope. */
    BOOLEAN_NO_SURFACE_IN_MERGE_SCOPE,
    /* Selected surfaces do not share any edge with the created surface and cannot be added. */
    BOOLEAN_NO_SHARED_EDGE_WITH_SURFACE_IN_MERGE_SCOPE,
    /* Selection is not a bend relief. */
    SHEET_METAL_BEND_RELIEF_NO_CORNER,
    /* Select a corner to modify. */
    SHEET_METAL_CORNER_SELECT_ENTITIES,
    /* Select a bend relief to modify. */
    SHEET_METAL_BEND_RELIEF_SELECT_ENTITIES,
    /* Radius value is required at each vertex. */
    VRFILLET_RADIUS_REQUIRED_AT_VERTEX,
    /* Rho value is required at each vertex of conic section. */
    VRFILLET_RHO_REQUIRED_AT_VERTEX,
    /* Magnitude is required at each vertex of curvature matched cross section. */
    VRFILLET_MAG_REQUIRED_AT_VERTEX,
    /* Select vertices on fillet chain. */
    VRFILLET_SELECT_VERTICES,
    /* Some selected vertices are not on an edge chain. */
    VRFILLET_VERTEX_NOT_ON_CHAIN,
    /* At least two vertices need to be selected for variable fillet on closed edge chains. */
    VRFILLET_INVALID_CHAIN,
    /* No tabs meet the selected sheet metal. */
    SHEET_METAL_TAB_NO_MERGE,
    /* Tab must be planar. */
    SHEET_METAL_TAB_NONPLANAR,
    /* Select a sheet metal wall to join the tab to. */
    SHEET_METAL_TAB_NO_WALL,
    /* Select a face for the tab. */
    SHEET_METAL_TAB_NO_TAB,
    /* Default settings are not used when overridden by variable fillet settings. */
    VRFILLET_NO_EFFECT,
    /* A vertex can appear in variable fillet settings only once. */
    VRFILLET_MULTI_SELECTION,
    /* Highlighted tabs have no parallel wall to join. */
    SHEET_METAL_TAB_NO_PARALLEL_WALL,
    /* A corner relief is too small to be successfully applied. */
    SHEET_METAL_CORNER_UNDER_SIZED,
    /* Could not fillet selections on some parts. */
    FILLET_PARTIAL_FAIL,
    /* Only end vertices of an edge chain can have zero radius. */
    VRFILLET_INTERNAL_ZERO,
    /* Tabs are entirely contained by the sheet metal walls. */
    SHEET_METAL_TAB_NO_EFFECT,
    /* Clearance of tab is less than minimal clearance. */
    SHEET_METAL_TAB_LOW_CLEARANCE,
    /* Fails to add tab. */
    SHEET_METAL_TAB_FAILS_MERGE,
    /* Tabs interfere with sheet metal walls. */
    SHEET_METAL_TAB_COLLISION,
    /* Replace face would split or merge faces. */
    REPLACE_FACE_FACE_COUNT_CHANGED,
    /* This operation creates overlapping geometry. */
    FACE_OVERLAP,
    /* Entire face would be removed by operation. */
    FACE_REMOVED,
    /* This operation creates intersecting edges. */
    INTERSECTING_EDGES,
    /* This operation would split the face. */
    CANNOT_SPLIT_FACE,
    /* Guide entities cannot be used as selected. */
    FILL_SURFACE_BAD_SUPPORT,
    /* Guide entities should touch the boundary. */
    FILL_SURFACE_SUPPORT_NOT_ON_BOUNDARY,
    /* All intersecting guides need to meet smoothly and guides intersecting the boundary must match the selected boundary condition. */
    FILL_SURFACE_SUPPORT_NOT_SMOOTH,
    /* Guide curves and vertices cannot be used in combination in Precise mode. Vertex selections are not used. */
    FILL_CURVE_OR_POINT_CONSTRAINTS,
    /* Sheet metal cannot be added to non-sheet metal or other sheet metal models. Sheet metal can only be added to the same sheet metal model. */
    SHEET_METAL_ADD_WRONG_MODEL,
    /* Cannot remove or intersect patterned sheet metal bodies. */
    SHEET_METAL_PATTERN_DISABLED_BOOLEANS,
    /* All instances have been hidden. */
    DRAWING_ALL_INSTANCES_HIDDEN,
    /* All tangents at highlighted vertices should be coplanar. */
    FILL_SUPPORT_NOT_SMOOTH_INTERNAL,
    /* External constraints can only be to the same flattened sheet metal as the sketch. */
    SKETCH_CONSTRAINT_WRONG_SHEET_METAL_BODY,
    /* A sketch on flattened sheet metal cannot have constraints to 3D parts. */
    SKETCH_CONSTRAINT_FLAT_IN_3D,
    /* A sketch on 3D parts cannot have constraints to flattened sheet metal. */
    SKETCH_CONSTRAINT_3D_IN_FLAT,
    /* Can match curvature only if direction is defined by an edge. */
    FIT_SPLINE_CURVATURE_FACE,
    /* Cannot evaluate curvature for end condition. */
    FIT_SPLINE_CANNOT_EVALUATE_CURVATURE_END_CONDITION,
    /* Cannot match curvature if direction is not defined. */
    FIT_SPLINE_NEED_DIRECTION_FOR_CURVATURE,
    /* The configuration parameter list has invalid entries */
    CONFIGURATION_HAS_BAD_PARAMETERS,
    /* The current configuration parameter list has invalid entries */
    CONFIGURATION_HAS_BAD_CURRENT_CONFIGURATION,
    FEATURE_CONFIGURED_PARAMETER_VALUES_HAVE_IDS,
    /* The workspace no longer exists. */
    WORKSPACE_NO_LONGER_EXISTS,
    /* Cannot stack, invalid stack mode specified. */
    CONTENT_STACKING_INVALID_MODE,
    /* Existing components do not exist to perform stacking. */
    CONTENT_STACKING_INVALID_COMPONENTS,
    /* Component on top of existing stack cannot accept additional top stack components. */
    CONTENT_STACKING_INVALID_TOP_STACK,
    /* Cannot pattern faces of joints. */
    SHEET_METAL_FACE_PATTERN_NO_JOINT,
    /* Some patterned faces could not cut the sheet metal model. */
    SHEET_METAL_FACE_PATTERN_FLOATING_CUT,
    /* Patterned faces could not join the sheet metal model. */
    SHEET_METAL_FACE_PATTERN_FLOATING_WALL,
    /* No faces meet flattened sheet metal bodies. */
    SM_FLAT_OP_NO_INTERSECT,
    /* Some faces don't meet flattened sheet metal bodies. */
    SM_FLAT_OP_PARTIAL_INTERSECT,
    /* Added material can't cross edges of flattened sheet metal body. */
    SM_FLAT_OP_ADD_CROSSES_EDGE,
    /* Can only use planar faces. */
    SM_FLAT_OP_NON_PLANAR_TOOL,
    /* Can only modify planar faces of flattened sheet metal body. */
    SM_FLAT_OP_NON_PLANAR_TARGET,
    /* Sheet metal flat operation failed. Please open a JIRA ticket. */
    SM_FLAT_OPERATION_FAILED,
    /* Sheet metal flat operation only works on sheet metal created at V629 or later. */
    SM_FLAT_OP_LEGACY_MODEL,
    /* Only linear edges can be used to flange. */
    SHEET_METAL_FLANGE_NON_LINEAR_EDGES,
    /* Selected cylinder cannot be used as a bend. */
    SHEET_METAL_CYLINDER_BEND,
    /* Only planar, cylindrical or extruded faces can be converted to sheet metal. */
    SHEET_METAL_INVALID_FACE,
    /* Only Simple corner relief can be applied to rolled walls and cylindrical bends. */
    SHEET_METAL_ROLLED_CORNER_RELIF,
    /* Failed to apply some reliefs. */
    SHEET_METAL_RELIEF_FAILURES,
    /* Failed to modify edge. */
    EDGE_CHANGE_FAILED,
    /* Not all inputs are solids. */
    BOOLEAN_INPUTS_NOT_SOLID,
    /* Scale requires a vertex or a mate connector. */
    TRANSFORM_SCALE_SELECTION,
    /* Removal destroys sheet metal part. */
    SHEET_METAL_SUBTRACT_DESTROYS_SHEET,
    /* Faces selected as split tools must be kept. */
    SPLIT_KEEP_TOOLS_WITH_FACE,
    /* Trim to face boundaries option requires the selection of a single face. */
    SPLIT_TRIM_WITH_SINGLE_FACE,
    /* One of the faces sketched on has moved to another flat part. */
    SHEET_METAL_SKETCH_DETACHED_FACE,
    /* Hole creates invalid geometry. */
    HOLE_CUT_FAIL,
    /* Cannot wrap sketch to sheet metal. */
    SHEET_METAL_FLAT_OP_ROLL_FAIL,
    /* Could not pattern selected edges. */
    PATTERN_EDGE_FAILED,
    /* Cannot pattern fillets, chamfers, reliefs, or corners. */
    SHEET_METAL_FACE_PATTERN_NO_VERTEX,
    /* Some patterned faces could not join the sheet metal model. */
    SHEET_METAL_FACE_PATTERN_PARTIAL_FLOATING_WALL,
    /* Cannot select from the flattened sheet metal view and model view at the same time. */
    EXTRUDE_3D_AND_FLAT,
    /* Could not create all instances as entered. Try selecting "Apply per instance" option. */
    PATTERN_SWITCH_TO_PER_INSTANCE,
    /* Selected features do not create any geometry that may be patterned. */
    PATTERN_NO_GEOM_FROM_FEATURES,
    /* You do not have LINK permission to one or more revisions. */
    RM_NO_LINK_PERMISSION_TO_REVISION,
    /* Total pattern instances cannot exceed 2500. */
    ASSEMBLY_PATTERN_EXCEED_MAX_INSTANCE_COUNT
}


