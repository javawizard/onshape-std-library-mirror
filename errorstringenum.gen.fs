FeatureScript ✨;
/* Automatically generated file -- DO NOT EDIT */

export enum ErrorStringEnum
{
    NO_ERROR,
    UNKNOWN_OPERATION,
    TOO_MANY_ENTITIES_SELECTED,
    POINTS_COINCIDENT,
    NO_TRANSLATION_DIRECTION,
    NO_ROTATION_AXIS,
    NO_TANGENT_PLANE,
    NO_TANGENT_LINE,
    INVALID_INPUT,
    CANNOT_RESOLVE_ENTITIES,
    CANNOT_EVALUATE_VERTEX,
    CANNOT_RESOLVE_PLANE,
    CANNOT_COMPUTE_BBOX,
    CANNOT_BE_EMPTY,
    CACHE_WRITE_FAILED,
    CACHE_READ_FAILED,
    HLR_FAILED,
    BAD_GEOMETRY,
    INVALID_RESULT,
    MISSING_EXT_REF,
    READ_FAILED,
    WRITE_FAILED,
    WRONG_TYPE,
    TANGENT_PROPAGATION_FAILED,
    REGEN_ERROR,
    COULD_NOT_COMPUTE_TRANSFORM,
    MATE_INVALID_MATE,
    MATECONNECTOR_INVALID_MATE,
    MATE_TWO_MATECONNECTORS_NEEDED,
    MATECONNECTORS_ON_SAME_OCCURRENCE,
    MATE_OVERDEFINED,
    MATE_INCONSISTENT,
    BOOLEAN_NEED_ONE_SOLID,
    BOOLEAN_INVALID,
    BOOLEAN_INTERSECT_FAIL,
    BOOLEAN_SAME_INPUT,
    BOOLEAN_BAD_INPUT,
    BOOLEAN_UNION_NO_OP,
    BOOLEAN_INTERSECT_NO_OP,
    BOOLEAN_SUBTRACT_NO_OP,
    CPLANE_INPUT_MIDPLANE,
    CPLANE_INPUT_OFFSET_PLANE,
    CPLANE_INPUT_POINT_PLANE,
    CPLANE_INPUT_LINE_ANGLE,
    CPLANE_INPUT_POINT_LINE,
    CPLANE_INPUT_THREE_POINT,
    CPLANE_FAILED,
    DRAFT_NO_NEUTRAL_PLANE,
    DRAFT_NO_DRAFT_FACE,
    DRAFT_FAILED,
    EXTRUDE_INVALID_REF_FACE,
    EXTRUDE_INVALID_REF_SURFACE,
    EXTRUDE_FAILED,
    EXTRUDE_NO_DIRECTION,
    EXTRUDE_INVALID_ENTITIES,
    PATTERN_INPUT_TOO_MANY_INSTANCES,
    PATTERN_INPUT_TOO_FEW_INSTANCES,
    PATTERN_FACE_FAILED,
    PATTERN_NOT_ON_BODY,
    PATTERN_BODY_FAILED,
    TRANSFORM_TRANSLATE_INPUT,
    TRANSFORM_TRANSLATE_BY_DISTANCE_INPUT,
    TRANSFORM_FAILED,
    SHELL_FAILED,
    EDGEBLEND_SMOOTH,
    EDGEBLEND_FAILED,
    DIRECT_EDIT_WRONG_CONCENTRIC,
    DIRECT_EDIT_WRONG_EQ_RADIUS,
    DIRECT_EDIT_NO_FILLET_FACES,
    DIRECT_EDIT_NO_OFFSET,
    DIRECT_EDIT_CONSTRAIN_FACE_FAILED,
    DIRECT_EDIT_REPLACE_FACE_FAILED,
    DIRECT_EDIT_DELETE_FACE_FAILED,
    DIRECT_EDIT_MODIFY_FILLET_FAILED,
    DIRECT_EDIT_MODIFY_FACE_FAILED,
    DIRECT_EDIT_MOVE_FACE_FAILED,
    DIRECT_EDIT_OFFSET_FACE_FAILED,
    IMPORT_PART_FAILED,
    IMPORT_ASSEMBLY_FAILED,
    IMPRINT_FAILED,
    REVOLVE_FAILED,
    REVOLVE_2ND_DIR_FAILED,
    REVOLVE_NOT_PLANAR,
    REVOLVE_PERPENDICULAR,
    REVOLVE_INVALID_ENTITIES,
    SPLIT_FAILED,
    SPLIT_INVALID_INPUT,
    SWEEP_INVALID_PATH,
    SWEEP_FAILED,
    SWEEP_PATH_FAILED,
    SWEEP_PROFILE_FAILED,
    WIRE_CREATION_FAILED,
    SKETCH_NO_PLANE,
    SKETCH_INPUT_INVALID,
    SKETCH_NOT_ACTIVE,
    SKETCH_SOLVER_NOT_INITIALIZED,
    SKETCH_EVALUATION_FAILED,
    SKETCH_MODIFICATION_FAILED,
    SKETCH_UPDATE_FAILED,
    SKETCH_SOLVE_FAILED,
    SKETCH_ADD_CONSTRAINT_FAILED,
    SKETCH_ADD_DIMENSION_FAILED,
    SKETCH_POSITION_DIMENSION_FAILED,
    SKETCH_CONSTRAINT_NEEDS_SKETCH_ENTITY,
    SKETCH_CONSTRAINT_UNKNOWN,
    SKETCH_MISSING_ENTITY,
    SKETCH_FILLET_INVALID_POINT,
    SKETCH_FILLET_PARALLEL,
    SKETCH_FILLET_FAIL,
    SKETCH_USE_FAILED,
    SKETCH_USE_PARTIAL,
    SKETCH_SPLINE_FAILED,
    SKETCH_BAD_SPLINE,
    SKETCH_DRAG_ERROR,
    SKETCH_PROJ_FAILED,
    SKETCH_PROJ_PARTIAL,
    SKETCH_TANGENT_ARC_FAILED,
    SKETCH_TANGENT_NOT_FOUND,
    SKETCH_OFFSET_FAILED,
    SKETCH_OFFSET_DISTANCE,
    SKETCH_TRIM_FAILED,
    SKETCH_INFERENCE_FAILED,
    SKETCH_MODIFY_DIM_FAILED,
    SKETCH_DRAG_NO_SKETCH,
    SKETCH_INFER_DIM_FAILED,
    SKETCH_DELETE_PTS_FAILED,
    SKETCH_DELETE_FAILED,
    SKETCH_ARC_FAILED,
    SKETCH_LINE_FAILED,
    SKETCH_CIRCLE_FAILED,
    SKETCH_RECTANGLE_FAILED,
    SKETCH_TANGENT_ARC_INVALID_START,
    SKETCH_CONSTRUCTION_POINT_FAILED,
    SYS_INTERNAL_DESERIALIZATION,
    SYS_SERVER_EXCEPTION,
    SYS_ERROR_REGEN,
    SYS_ERROR_MESSAGING,
    CANNOT_RESOLVE_ELEMENT,
    NOTHING_SELECTED,
    SKETCH_ANGLE_TWO_LINES,
    SKETCH_DIMENSION_DIFF_ENTITIES,
    SKETCH_CONSTRAINT_DIFF_ENTITIES,
    SKETCH_CONSTRAINT_TWO_ENTITIES,
    SKETCH_DIMENSION_TWO_ENTITIES,
    SKETCH_COINCIDENT_FAILED,
    SKETCH_COINCIDENT_INPUT_ERROR,
    SKETCH_COINCIDENT_DIFF_POINTS,
    SKETCH_CONCENTRIC_INPUT_ERROR,
    SKETCH_CONCENTRIC_FAILED,
    SKETCH_EQUAL_INPUT_ERROR,
    SKETCH_EQUAL_NO_ENDS,
    SKETCH_EQUAL_FAILED,
    SKETCH_FIX_ONE_ENT,
    SKETCH_FIX_FAILED,
    SKETCH_DIR_INTERNAL,
    SKETCH_DIR_INPUT,
    SKETCH_HORIZONTAL_FAILED,
    SKETCH_VERTICAL_FAILED,
    SKETCH_OFFSET_CONSTRAINT_FAILED,
    SKETCH_PARALLEL_CONSTRAINT_FAILED,
    SKETCH_PARALLEL_INPUT_ERROR,
    SKETCH_DIMENSION_INPUT_ERROR,
    SKETCH_DIMENSION_DIST_ERROR,
    SKETCH_DIMENSION_FAILED,
    SKETCH_NORMAL_NEED_LINE,
    SKETCH_NORMAL_INPUT_ERROR,
    SKETCH_NORMAL_INPUT_NEEDED,
    SKETCH_CANNOT_SPLIT_INTO_GROUPS,
    SKETCH_OFFSET_BAD_PAIR,
    SKETCH_OFFSET_INPUT_ERROR,
    SKETCH_MIDPOINT_INPUT_ERROR,
    SKETCH_MIDPOINT_NEED_POINT,
    SKETCH_MIDPOINT_NEED_DIFF_POINT,
    SKETCH_MIDPOINT_MISSING_ENDS,
    SKETCH_MIDPOINT_MISSING_PTS,
    SKETCH_MIDPOINT_NO_INTERNAL_LINE,
    SKETCH_MIDPOINT_NO_COINCIDENT,
    SKETCH_MIDPOINT_FAILED,
    SKETCH_PERPENDICULAR_INPUT_ERROR,
    SKETCH_PERPENDICULAR_FAILED,
    SKETCH_POINT_LINE_ONLY,
    SKETCH_PROJECTION_UNKNOWN,
    SKETCH_PROJECTION_FAILED,
    SKETCH_SIL_PROJECTION_INPUT_ERROR,
    SKETCH_SIL_PROJECTION_MISSING_POINT,
    SKETCH_LENGTH_DIM_INPUT_ERROR,
    SKETCH_LENGTH_DIM_MISSING_ENDS,
    SKETCH_LENGTH_DIM_NOT_FOUND,
    SKETCH_LENGTH_DIM_FAILED,
    SKETCH_RADIUS_INPUT_ERROR,
    SKETCH_RADIUS_DIM_FAILED,
    SKETCH_TANGENT_INPUT_ERROR,
    SKETCH_TANGENT_FAILED,
    PART_QUERY_FAILED,
    PART_QUERY_MULTI,
    MATECONNECTOR_QUERY_FAILED,
    MATECONNECTOR_QUERY_ORIGIN_FAILED,
    MATECONNECTOR_QUERY_AXIS_FAILED,
    MATECONNECTOR_QUERY_CSYS_FAILED,
    ASSEMBLY_INSERT_WILL_CAUSE_CYCLES,
    SKETCH_MIRROR_NEED_VALID_MIRROR_LINE,
    SKETCH_MIRROR_NEED_ENTITIES_TO_MIRROR,
    SKETCH_MIRROR_CONSTRAINT_FAILED,
    SKETCH_MIRROR_FAILED,
    SELF_INTERSECTING_CURVE_SELECTED,
    SWEEP_START_NOT_ON_PROFILE,
    PATTERN_DIRECTIONS_PARALLEL,
    MATE_OCCURRENCE_NOT_VALID,
    MATE_WITHIN_SAME_GROUP,
    EXPORT_ASSEMBLY_UNKNOWN_NODE_TYPE,
    EXPORT_ASSEMBLY_CREATE_INSTANCE_FAILED,
    EXPORT_PARTS_AS_XTS_NOT_A_BODY,
    EXPORT_PARTS_AS_XTS_FAILED_TO_WRITE_XT,
    MATECONNECTOR_OWNER_PART_NOT_RESOLVED,
    WIRE_CREATION_PARTIAL_FAILURE,
    SERVER_IS_IN_INVALID_STATE,
    SKETCH_EXTEND_FAILED,
    FOLLOW_CYCLE_ERROR,
    SKETCH_FILLET_INVALID_RADIUS,
    SKETCH_CONSTRAINT_COINCIDENT_TWO_ENTITIES,
    SKETCH_CONSTRAINT_CONCENTRIC_TWO_ENTITIES,
    SKETCH_CONSTRAINT_EQUAL_TWO_ENTITIES,
    SKETCH_CONSTRAINT_MIDPOINT_TWO_ENTITIES,
    EXTRUDE_NO_SELECTED_REGION,
    EXTRUDE_NO_REGION_IN_SKETCH,
    DELETE_SELECT_PARTS,
    COPY_SELECT_PARTS,
    SPLIT_NO_CHANGE,
    MIRROR_NO_PLANE,
    MIRROR_SELECT_PARTS,
    PATTERN_CIRCULAR_NO_AXIS,
    PATTERN_SELECT_FACES,
    PATTERN_SELECT_PARTS,
    PATTERN_LINEAR_NO_DIR,
    SHELL_SELECT_FACES,
    DRAFT_SELECT_NEUTRAL,
    DRAFT_SELECT_FACES,
    CHAMFER_SELECT_EDGES,
    FILLET_SELECT_EDGES,
    EXTRUDE_SURF_NO_CURVE,
    EXTRUDE_SELECT_TERMINATING_BODY,
    EXTRUDE_SELECT_TERMINATING_SURFACE,
    DIRECT_EDIT_SELECT_ANCHOR,
    REVOLVE_SURF_NO_CURVE,
    REVOLVE_SELECT_FACES,
    REVOLVE_SELECT_AXIS,
    SWEEP_SELECT_PROFILE,
    SWEEP_SELECT_PATH,
    DIRECT_EDIT_DELETE_SELECT_FACES,
    DIRECT_EDIT_MODIFY_FILLET_SELECT,
    DIRECT_EDIT_MODIFY_FACE_SELECT,
    DIRECT_EDIT_REPLACE_FACE_SELECT,
    DIRECT_EDIT_OFFSET_FACE_SELECT,
    DIRECT_EDIT_MOVE_FACE_SELECT,
    SELECT_MATECONNECTOR,
    OVERDEFINED_ASSEMBLY,
    PART_STUDIO_UPGRADE_SUCCESSFUL,
    PART_STUDIO_UPGRADE_FAILED,
    PART_STUDIO_UPGRADE_NONE,
    MATE_GROUP_OCCURRENCES_UNRESOLVED,
    SWEEP_SURF_NO_CURVE_PROFILE,
    MATE_RESET_HAD_NO_EFFECT,
    MATECONNECTOR_MULTIPLE_OCCURRENCES,
    MATECONNECTOR_OCCURRENCE_NOT_RESOLVED,
    ELEMENT_REFERENCE_CYCLE_DETECTED,
    MATE_OVERDEFINES_ASSEMBLY,
    MATE_CANNOT_RESOLVE_CONNECTORS,
    SKETCH_EXCEEDS_BOUNDS,
    SWEEP_SELF_INT,
    SKETCH_UNSOLVABLE_CONSTRAINT,
    RESTRUCTURE_INVALID_SOURCE_OR_TARGET,
    CPLANE_INPUT_CURVE_POINT,
    TRANSFORM_OCCURRENCES_HAD_NO_EFFECT,
    HELIX_FAILED,
    HELIX_INPUT_CONE,
    RENDERER_NOT_AVAILABLE,
    RENDERER_FAILED_TO_RENDER,
    EXPRESSION_FAILED_VALIDATION,
    VERSION_MISMATCH_ERROR,
    EXTRUDE_UPTO_NEXT_NO_DIVISION,
    MATE_BETWEEN_FIXED_OCCURRENCES,
    THICKEN_SELECT_ENTITIES,
    THICKEN_FAILED,
    WORKSPACE_UPGRADE_SUCCESSFUL,
    WORKSPACE_UPGRADE_FAILED,
    WORKSPACE_UPGRADE_NONE,
    SKETCH_CIRCULAR_PATTERN_FAILED,
    DIRECT_EDIT_ALL_FILLET_FACES_SELECTED,
    DIRECT_EDIT_FAILED_TO_IDENTIFY_FILLETS,
    PARASOLID_IMPORT_FAILED,
    FOLLOW_LEADER_HAS_NO_FUNCTIONALITY_ERROR,
    MIRROR_SELECT_FACES,
    RELATION_INVALID_RELATION,
    RELATION_INVALID_MATE,
    GEAR_RELATION_INVALID_MATE_TYPES,
    SCREW_RELATION_INVALID_MATE_TYPES,
    RACK_RELATION_INVALID_MATE_TYPES,
    ROLLING_RELATION_INVALID_MATE_TYPES,
    LINEAR_RELATION_INVALID_MATE_TYPES,
    RELATION_OVERDEFINED,
    RELATION_INCONSISTENT,
    RELATION_SAME_OCCURRENCES,
    SKETCH_SPLIT_FAILED,
    SKETCH_CONSTRAINT_PIERCE_TWO_ENTITIES,
    SKETCH_PIERCE_FAILED,
    MIRROR_FACE_FAILED,
    MIRROR_BODY_FAILED,
    SKETCH_CANNOT_PIERCE_WITH_PLANE,
    WITH_SUPPORT_CODE,
    FILLET_FAIL_SMOOTH,
    FILLET_FAILED,
    CHAMFER_FAIL_SMOOTH,
    CHAMFER_FAILED,
    BOOLEAN_OFFSET_NO_FACES,
    MATE_OCCURRENCE_SUPPRESSED,
    MATECONNECTOR_OCCURRENCE_SUPPRESSED,
    SKETCH_SPLINE_NEW_POINTS_TOO_CLOSE,
    SKETCH_SPLINE_CANNOT_DELETE_ENDPOINTS,
    SKETCH_SPLINE_POINT_TO_DELETE_NOT_FOUND,
    ASSEMBLY_INSERT_FAILED,
    SKETCH_PATTERN_UNKNOWN_FAILURE,
    SKETCH_PATTERN_TOO_LARGE,
    SKETCH_LINEAR_PATTERN_ZERO_LENGTH,
    SKETCH_LINEAR_PATTERN_PARALLEL_DIRECTIONS,
    SKETCH_CIRCULAR_PATTERN_ZERO_ANGLE,
    SKETCH_ELLIPSE_FAILED,
    SKETCH_ELLIPSE_FAILED_TOO_SMALL,
    DELETE_PARTS_FAILED,
    DELETE_PARTS_PARTIAL,
    SKETCH_ELLIPSE_RADIUS_INPUT_ERROR,
    QUADRANT_CONSTRAINT_INPUT,
    SKETCH_QUADRANT_FAILED,
    SKETCH_SPLINE_TOO_FEW_POINTS,
    SKETCH_SPLINE_NOT_INTERPOLATED_SPLINE,
    SKETCH_SPLINE_POINTS_NOT_DELETED,
    SKETCH_TEXT_RECTANGLE_FAILED,
    IMPORT_DERIVED_NO_PARTS,
    LOFT_SELECT_PROFILES,
    LOFT_PROFILE_SINGLE_FACE,
    LOFT_PROFILE_SOLID,
    LOFT_PROFILE_POINT,
    LOFT_PROFILE_FAILED,
    LOFT_SELECT_GUIDES,
    LOFT_GUIDE_FAILED,
    LOFT_PERIODIC_ERROR,
    LOFT_GUIDE_POINT_INTERSECTION,
    LOFT_GUIDE_PROFILE_INTERSECTION,
    LOFT_VERTEX_MATCHING,
    LOFT_DIRECTION_ERROR,
    LOFT_PROFILE_ALIGNMENT,
    LOFT_GUIDE_ALIGNMENT,
    LOFT_VERTEX_ADDITIONS,
    LOFT_FAILED,
    LOFT_INVALID,
    LOFT_ALIGNMENT_INFO,
    LOFT_VERTEX_NOT_ON_PROFILE,
    LOFT_PROFILE_NO_INNER_LOOPS,
    LOFT_TWO_PROFILES,
    CANNOT_OFFSET_ELLIPSE,
    SKETCH_MIRROR_NEEDS_LINE_AND_TWO_OTHERS,
    SKETCH_POLYGON_BAD_SIDE_COUNT,
    SKETCH_DIRECTIONAL_GROUP_INPUT,
    NAMED_VIEWS_DUPLICATE_NAME,
    SILHOUETTE_USE_FAILED,
    PASTE_SKETCH_METRICS_FAILURE,
    PASTE_SKETCH_LIBRARY_MISMATCH,
    PASTE_SKETCH_CLIPBOARD_EMPTY,
    SKETCH_MIRROR_OFFSET_SPLINE,
    SKETCH_MIRROR_CURVE_POINT,
    LOFT_PERIODIC_GUIDE_ERROR,
    SHELL_SELECT_PARTS
}


