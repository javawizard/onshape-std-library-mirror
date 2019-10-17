FeatureScript 1174; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/* Automatically generated file -- DO NOT EDIT */

/** @internal */
export enum FeatureScriptVersionNumber
{
    V0_ORIGINAL_VERSION,
    V1_SKETCH_DIRECTED_DIMENSIONS,
    V2_DCM_VERSION_63_3,
    V3_SWEEP_GRID_TOPOLOGY_WITH_REMOVE_REDUNDANT,
    V4_DCM_VERSION_63_3_1,
    V5_IMPRINT_USE_MIN_DET_ID_IN_IMPRINTS,
    V6_SKETCH_DIAMETRAL_DIMENSIONS,
    V7_PS_VERSION_27_0_161,
    V8_DRAFT_LAMINAR_EDGE_DRAW,
    V9_REVOLVE_PLACEHOLDER_FEATURESCRIPT_ONLY_CHANGE,
    V10_SWEEP_PROFILE_START_VERTEX,
    V11_SKETCH_ENFORCE_CENTRIPETAL_KNOTS,
    V12_SKETCH_PROJECTED_ENDS,
    V13_SKETCH_SPECIFIC_CIRCLE_AND_ARC_PROJECTIONS,
    V14_SPLIT_ERROR_STATUS_CHECK,
    V15_PATTERN_CHECK_FOR_BAD_HISTORY_RECORDS,
    V16_SKETCH_TOOL_TYPES,
    V17_PS_VERSION_27_0_178,
    V18_STRICTER_DISAMBIGUATION_IN_EXACT_MATCH_QUERY_EVAL,
    V19_BLEND_UNIQUE_SET_OF_DEPENDENCIES_IN_BLENDED_RECORD_FIXED,
    V20_DCM_VERSION_63_3_2,
    V21_CREATION_RECORD_COMPARISON_DEPENDENCIES_FIRST,
    V22_SWEEP_ORDER_BEFORE_REMOVE_REDUNDANT,
    V23_PROCEDURAL_SWEPT_SURFACES,
    V24_SWEPTFEATURETRACKER_UPDATE_FIX,
    V25_BLEND_PROCESS_RECORDS_AFTER_MERGE,
    V26_SWEPTFEATURETRACKER_TWEAK_RECORDS,
    V27_SKETCH_ALLOW_IMPRINT_OF_FACES_ONLY,
    V28_SKETCH_FLAG_UNSOLVABLE_CONSTRAINTS,
    V29_DCM_VERSION_63_3_4,
    V30_BOOLEAN_COPY_RECORDS_FOR_TOOL_DERIVED,
    V31_PROFILES_FROM_SKETCH_TRACKED_VIA_BOUNDARY,
    V32_AXIS_NEAR_BBOX_CENTER,
    V33_SUPPRESSED_PROFILES_FROM_SKETCH_TRACKED_VIA_BOUNDARY,
    V34_NEW_VERSIONING_SCHEME,
    V35_NESTED_PROFILES,
    V36_INVALID,
    V37_INVALID,
    V38_INVALID,
    V39_INVALID,
    V40_INVALID,
    V41_INVALID,
    V42_INVALID,
    V43_INVALID,
    V44_INVALID,
    V45_INVALID,
    V46_NEW_VERSION_NUMBERING_SCHEME,
    V47_FEATURE_SPECIFIC_VERSIONING,
    V48_JOURNAL_FAILING_FEATURES,
    V49_EDGE_BLEND_SURF_INTERSECTION_CHECK,
    V50_HELIX,
    V51_RESUME_PROFILES_FROM_SKETCH_TRACKED_VIA_BOUNDARY,
    V52_CREATE_EMPTY_BOOLEAN_RECORDS,
    V53_ENFORCE_PRECONDITIONS,
    V54_RESUME_PROFILES_FROM_SKETCH_TRACKED_VIA_BOUNDARY,
    V55_SUPPORT_EXPRESSIONS,
    V56_FIX_UP_UNITS,
    V57_FIX_RADIAN_VALUE_BOUNDS,
    V58_HELIX_DIRECTION,
    V59_TRIG_FUNCTIONS_AND_DRAFT,
    V60_TRANSFORM_PART,
    V61_DCM_VERSION_63_3_6,
    V62_FACE_OPERATIONS_MODIFY_CHECK_TYPE,
    V63_NEW_CHAMFER_TYPES,
    V64_FIXED_CHAMFER_PRECONDITION,
    V65_UPTO_NEXT_TARGETS_ONLY,
    V66_CHAMFER_RANGE_AND_UNDO_UPTO_NEXT,
    V67_THICKEN,
    V68_TRANSFORM_MANIPULATORS,
    V69_IMPRINTED_EDGE_DIRECTION,
    V70_SKETCH_POLYGON,
    V71_SKETCH_REGION_QUERY_UPDATE,
    V72_EXPERIMENT,
    V73_CHAMFER_THICKEN_FS_BUGS,
    V74_TRANSFORM_CHECKING,
    V75_THICKEN_ENCHANCEMENTS,
    V76_RETRIEVAL_UPGRADE,
    V77_MINOR_FS_BUGS,
    V78_DCM_VERSION_64_0_0,
    V79_FILTER_FILLETS_FROM_MOVE_FACE,
    V80_TRANSFORM_TRIAD,
    V81_VARIABLES_SHADOW_FUNCTIONS,
    V82_FILTER_FILLETS_FROM_DIRECT_EDIT_OPS,
    V83_HIDE_PLANE_SIZE,
    V84_IMPORT_Y_UP,
    V85_IMPORT_FS_TEST,
    V86_IMPORT_PARAMETER_FIX,
    V87_SKETCH_REGION_ORDERING,
    V88_FILLET_SELECTION_TYPO,
    V89_ARGUMENT_DEPENDENT_LOOKUP,
    V90_DEFINE_FEATURE,
    V91_PS27_1_165,
    V92_ERROR_PARAMETERS,
    V93_FACE_MIRROR,
    V94_TRACK_TWEAK_EDGES_BY_TWEAK_FACE_DEPENDENCIES,
    V95_DCM_VERSION_64_3_0,
    V96_BOOLEAN_OFFSET,
    V97_LESS_APPROXIMATE_SPLINE_ENDS,
    V98_PIERCE_CONSTRAINT,
    V99_MIRROR_ERROR_FIX,
    V100_BOOLEAN_OFFSET_NO_FACE_ERROR,
    V101_RETRIEVAL_UPGRADE,
    V102_DUMMY,
    V103_BOOLEAN_OFFSET_ERROR_PROPAGATION,
    V104_DRAFT_CHECK_STEEP,
    V105_ANGULAR_MANIPULATOR_RANGE,
    V106_DIRECT_EDIT_IMPROVE_REDUNDANT_EDGE_REMOVAL,
    V107_ENFORCE_MINIMUM_INTERPOLATED_KNOT_DISTANCE,
    V108_SWEEP_ALIGNMENT_OPTION,
    V109_FIX_ROTATE_FACE_ZERO_LENGTH_MANIPULATOR,
    V110_ANGULAR_MANIPULATOR_LIMIT_DEFAULTS,
    V111_PREVENT_DUPLICATE_DET_IDS_ON_TWEAKED_TOPOLS,
    V112_DEGENERATE_SKETCH_ENTITY_DISPLAY,
    V113_ALLOW_NEGATIVE_VALUES_IN_VECTOR_DEFINITIONS,
    V114_RESTRICT_MATECONNECTOR_OWNER_TO_ONE,
    V115_SKETCH_LINEAR_PATTERNS,
    V116_UI_HINT_ENUM,
    V117_ERROR_CONTAINMENT,
    V118_MORE_ACCURATE_HELIX,
    V119_PATTERN_PRESELECTION_FIX,
    V120_BOOLEAN_OFFSET_REAPPLY_FILLET,
    V121_SKETCH_PIERCE_LOCATION_FIX,
    V122_SKETCH_ELLIPSE,
    V123_UNIFORM_SCALE,
    V124_DELETE_IMPORTED_INSTANCE_FIX,
    V125_REMOVE_SCALE_MANIPULATOR,
    V126_DCM_VERSION_64_3_1,
    V127_ELLIPSE_RADIUS_DIMENSIONS,
    V128_FIX_SPLIT_VERTEX_IMPRINT,
    V129_QUADRANT_CONSTRAINT,
    V130_SKETCH_TEXT,
    V131_DIRECT_EDIT_PREVENT_EMPTY_SELECTION,
    V132_DCM_VERSION_64_3_2,
    V133_DIAMETER_CONSTRAINT,
    V134_LOFT_SELECTION_FILTERS,
    V135_SECTION_PART_FIX,
    V136_LOFT_ERRORS,
    V137_VERSION_HISTORY_ENTRY,
    V138_COEDGE_QUERIES,
    V139_DIG_DEEPER_FOR_SKETCH_PLANE,
    V140_SKETCH_REGION_VIA_FLOOD_FILL,
    V141_THICKEN_RECORDS_FIX,
    V142_TWEAKS_AND_NO_EXPORT_IMPORT,
    V143_SECTION_PART_FIX,
    V144_QUADRANT_CONSTRAINT_REWRITE,
    V145_REMOVE_SCAR_EDGES,
    V146_VERSIONING,
    V147_SPLIT_DET_ID_STABILITY,
    V148_SKETCH_SILHOUETTES,
    V149_SKETCH_SILHOUETTES_2,
    V150_APP_ELEMENT_INTERNAL,
    V151_EXTRUDE_SECOND_DIRECTION,
    V152_ADD_MANIPULATOR_STYLE,
    V153_ADD_MIRROR_PATTERN_BOOLEANS,
    V154_GET_LANGUAGE_VERSION,
    V155_DISABLE_MIRROR_PATTERN_BOOLEANS,
    V156_EXTRUDE_SECOND_DIRECTION_SYMMETRIC_FIX,
    V157_PS_VERSION_28_0_159,
    V158_MATECONNECTOR_QUERY,
    V159_REVOLVE_SECOND_DIRECTION,
    V160_FIX_MIRROR_PATTERN_BOOLEAN,
    V161_MATECONNECTORS_ON_CONSTRUCTION,
    V162_REVOLVE_SECOND_DIRECTION_FIX,
    V163_PERIODIC_LOFT_FIXES,
    V164_PS_VERSION_28_0_167,
    V165_HOLLOWING_WITH_SHELL,
    V166_FLATTEN_IMPORT,
    V167_PROJECT_ELLIPSES,
    V168_FILTER_SELECTOR,
    V169_HOLLOWING_WITH_SHELL_FILTER,
    V170_CHANGE_MIRROR_BOOLEAN_TOOLS,
    V171_SHELL_PRESELECTION_REGRESSION_FIX,
    V172_REVOLVE_TWO_DIRECTION_MANIPULATOR_REVERT,
    V173_INTERSECTION_CREATES_BODIES,
    V174_PS_VERSION_28_0_174,
    V175_DCM_VERSION_64_3_3,
    V176_DCM_VERSION_64_3_4,
    V177_CONSTRUCTION_OBJECT_FILTER,
    V178_SPLINE_LENGTH_DIMENSIONS,
    V179_SUBTRACT_COMPLEMENT_HANDLED_IN_FS,
    V180_INTERSECT_KEEP_TOOLS,
    V181_MASS_UNITS,
    V182_HOLE_FEATURE,
    V183_CONSOLIDATE_DEGENERATE_SKETCH_WIRE_CHECKS,
    V184_HOLE_FEATURE_HEURISTICS,
    V185_HOLE_FEATURE_QLV_CHANGES,
    V186_PLANE_COORDINATES,
    V187_HOLE_FEATURE_IMPROVEMENTS,
    V188_HOLE_FEATURE_QLV_PROMPT,
    V189_HOLE_FEATURE_SPELLING,
    V190_HOLE_FEATURE_ANGLE_MATH,
    V191_SKETCH_PROJECTION_ERROR,
    V192_HOLE_FEATURE_REMOVE_COINC_POINTS,
    V193_PS_VERSION_28_0_180,
    V194_HOLE_FEATURE_SPELLING,
    V195_MOVE_FACE_TRANSFORM_CONTAINED_FILLET_GROUPS,
    V196_SPLIT_MERGE_REDUNDANT,
    V197_REDUCED_SKETCH_STATUS_CHECKS,
    V198_HOLE_FEATURE_HEURISTICS_2,
    V199_EXTRUDE_SEPARATE_BOUND_ENTITY_QUERY,
    V200_CENTERLINE_DIMENSIONS,
    V201_ERROR_HANDLING,
    V202_REMOVE_CONSTRAINT_TYPE,
    V203_SWEEP_PATH_NO_CONSTRUCTION,
    V204_SKETCH_IMAGE,
    V205_REMEMBER_FEATURE_VALUES,
    V206_LINEAR_RANGE,
    V207_FIXED_FILLET_MANIPULATUOR,
    V208_HOLE_FEATURE_HANDLE_FAILED_EXTRUDE,
    V209_CHANGE_VARIABLE_ANNOTATION_CASE,
    V210_LOFT_END_CONDITIONS,
    V211_EXTRUDE_WITH_DRAFT,
    V212_FIX_BOOLEAN_STATUS,
    V213_EXTRUDE_WITH_DRAFT2,
    V218_PS_VERSION_28_0_188,
    V219_FACE_PATTERN_FEATURE_QLV,
    V220_DRAFT_IS_STEEP_CHECK,
    V221_HELIX_REDIRECTION,
    V222_REMOVE_LOFT_INFO,
    V223_CHANGE_VARIABLE_NAME_TEMPLATE,
    V224_VERSION_BUMP,
    V225_HOLE_FEATURE_FIX_BROKEN_DEPTH_CALC,
    V230_LARGE_ASSEMBLY_IMPORT_FIX,
    V231_SURFACE_SPLIT,
    V232_SKETCH_IMAGE_WITH_BLOB,
    V233_FS_EV_CURVE_DEFINITION,
    V234_REVOLVE_TWO_DIRECTION,
    V235_TRANSFORM_BY_MATE_CONNECTORS,
    V236_DEFAULT_MAX_ASSEMBLIES,
    V241_PS_VERSION_28_0_197,
    V242_MATE_CONNECTOR_CSYS_AND_ANGLE_RENORM,
    V243_IMPRINT_USE_VERTEX_TOLERANCE,
    V244_SKETCH_REGION_UPDATE,
    V250_PS_VERSION_28_0_202,
    V251_THICKEN_RECORDS_FIX2,
    V252_HOLE_FEATURE_FIX_ERROR_CHECK,
    V253_ADD_SPLIT_FACE,
    V254_SKETCH_INTERSECTION,
    V255_HOLE_FEATURE_FIX_ERROR_DISPLAY,
    V261_FACE_DISAMBIGUATION_ERROR,
    V262_DCM_VERSION_65_0_0,
    V263_SURFACE_PATTERN_BOOLEAN,
    V264_SKETCH_CONSTRAINT_PREPROCESS_FAILURE,
    V265_PS_VERSION_28_0_204,
    V266_ALLOW_ZERO_OFFSETS,
    V267_DISPLAY_TEARDROP_SPLINES,
    V268_LOFT_BUGS,
    V269_COUNT_MERGE_TOWARDS_SPLIT,
    V270_SKETCH_TEXT_CUSTOM_MISSING_CHAR,
    V271_MATE_CONNECTOR_PARALLEL_FIX,
    V272_MODIFY_DISAMBIGUATION,
    V273_HISTORY_RECORD_FIXES,
    V274_HOLE_LIMIT_NUM_LOCATIONS_100,
    V275_SKETCH_PATTERN_LOWER_LIMIT,
    V281_SEPARATE_REDUNDANDANT_TOPOLS_BY_TYPES,
    V282_DCM_VERSION_65_5_1,
    V283_ROUND_FOR_ERRORS,
    V284_SWEEP_PROFILE_ALIGNMENT,
    V285_CONNECTOR_OWNER_EDIT_LOGIC,
    V286_HOLE_ADD_STANDARDS,
    V287_NEW_BTMFEATURE,
    V288_PS_VERSION_28_1_177,
    V289_EVDISTANCE,
    V290_FS_USE_STD_REGEX,
    V291_HOLE_FEATURE_STANDARD_DEFAULTS,
    V292_HOLE_FEATURE_CHANGE_TAP_DRILL_NAME,
    V293_HOLE_FEATURE_BLIND_IN_LAST_ERROR_FIX,
    V296_DCM_VERSION_65_5_2,
    V297_GEOMETRY_BUGS,
    V298_PS_VERSION_28_1_194,
    V299_HOLE_FEATURE_FIX_BLIND_IN_LAST_FLIP,
    V300_FEATURE_PATTERN,
    V301_PLANE_DEFAULT_SIZE,
    V302_SECTION_PART_STUDIOS,
    V303_FEAT_PATTERN_SKETCH_FIX,
    V304_DCM_VERSION_65_5_1,
    V305_UPGRADE_TEST_FAIL,
    V306_HOLE_FEATURE_FIX_UNDETERMINED_TARGET_BODY,
    V307_HOLE_FEATURE_FIX_UNDETERMINED_TARGET_BODY_2,
    V308_ADD_RIGHT_PAD_TO_TEXT,
    V309_MOVE_FACE_SUPPORT_360_DEG_ROTATION,
    V310_SKETCH_SILHOUETTES_IN_FACE,
    V311_MATECONNECTOR_EDGE_INFERENCE_FIX,
    V312_EXTRUDE_UP_TO_FACE_USE_OWNER_TOPOLOGY,
    V313_ORDER_INDEPENDENT_SKETCH_OFFSETS,
    V314_HOLE_FIX_CYL_CAST_EARLY_RETURN,
    V315_DCM_VERSION_65_5_4,
    V316_PLANE_DEFAULT_SIZE_FIX,
    V322_VERSION_BUMP_1_43,
    V323_CONSISTENT_ELLIPSE_ORIENTATION,
    V324_UPDATE_IMPRINT_CURVE_NORMAL,
    V325_FEATURE_MIRROR,
    V326_PS_VERSION_28_1_224,
    V327_LOOKUP_TABLE_CAPITALIZE,
    V328_INTERNAL_DOCUMENTATION,
    V333_UPDATE_SKETCH_IMPRINT,
    V334_PS_VERSION_28_1_228,
    V335_REQUIRE_STD_VERSIONS,
    V336_FS_ATTRIBUTES_IN_MERGE,
    V342_CHECK_INITIAL_GUESS,
    V343_SUBFEATURES,
    V344_BOOLEAN_BLEND_BUGS,
    V345_PS_VERSION_28_1_231,
    V346_TRACKING_WITH_DET_IDS,
    V347_SHEET_METAL_FLATTEN,
    V353_DCM_VERSION_65_5_6,
    V354_BETWEEN_ENTITY_ADDITIONAL_QUERY_REQUIRED,
    V355_DOCUMENTATION_UPDATE,
    V360_FEATURE_MAP,
    V361_DETECT_INFINITE_SKETCH_DIMENSION,
    V362_HOLE_IMPROVED_DEPTH_FINDING,
    V363_FS_ATTRIBUTE_MANAGEMENT,
    V364_HOLE_FIX_FEATURE_MIRROR,
    V365_BOOLEAN_SURFACE_ERROR_FIX,
    V366_OPPOSITE_DIRECTION_CIRCULAR_UIHINT,
    V367_VERTEX_EXTRUDE_DEGENERATE_FIX,
    V368_HOLE_FIELD_NAME_MISMATCH_FIX,
    V369_FS_ERROR_EXTRUDE_FIX,
    V370_UPGRADE_COMPRESS_FIX,
    V371_HOLE_IMPROVED_DISJOINT_CHECK,
    V372_EVDISTANCE_INTERIOR_FIX,
    V373_MESH_SUPPORT,
    V374_HELIX_ALIGNMENT,
    V375_HOLE_FIX_EDIT_LOGIC,
    V376_FS_VERSION_BUMP,
    V379_EDITING_LOGIC_QUERIES,
    V380_UPDATE_ELLIPSE_OFFSET_DATA,
    V381_SKIP,
    V382_NORMALIZE_ELLIPSE_AXIS,
    V383_PS_VERSION_29_0_155,
    V384_DCM_VERSION_66_0_0,
    V385_IMPORT_RECORD,
    V386_NEW_IMPRINT_API,
    V387_PROPER_CIRCLE_INVERSION,
    V388_HOLE_FIX_FEATURE_PATTERN_TRANSFORM,
    V389_FACE_PATTERN_SKIP_LOOP_CHECK,
    V390_TIGHT_BOUNDING_BOX,
    V391_DOCUMENTATION_CORRECTION,
    V392_HOLE_FIX_EDIT_LOGIC,
    V396_WILD_CARD_IN_QUERIES,
    V397_FLIP_CONE_HELIX,
    V398_NORMAL_BY_ROTATION,
    V399_PS_VERSION_29_0_166,
    V400_SHELL_FIX_TRACKING_ID,
    V401_HOLE_CLEAR_START_FROM_SKETCH,
    V402_FACE_PATTERN_TRACKING,
    V403_ACCOUNT_FOR_REVERSED_CURVE,
    V404_HANDLE_COLLAPSED_TEXT,
    V405_ERROR_DISPLAY_AND_FEATURE_STATUS,
    V406_SPHERE_PRIMITIVE,
    V407_WITHIN_RADIUS,
    V408_ERROR_ENUM_ASSEMBLY_PATTERN_NOT_SUPPORTED,
    V412_CHECK_VALID_BSPLINE_APPROX,
    V413_AUTOFIX_HELP_POINTS,
    V414_ASYMMETRIC_CHAMFER_MIRROR_BUG,
    V415_CPLANE_MANIPULATOR,
    V416_MESH_NOT_SUPPORTED_ERROR,
    V417_HOLE_CHANGE_TAP_CLEARANCE_TOOLTIP,
    V418_RIB_FIX_EXTEND_LOGIC,
    V419_EVBOX3D_FIXES,
    V420_RIB_SIMPLER_HEURISTICS,
    V421_TRIM_HELIX_AT_CONE_APEX,
    V422_RIB_DELETE_ERROR_BODIES,
    V423_UNWRAP_POINT_BODIES,
    V424_VARIABLE_WARNINGS,
    V425_MORE_DEGENERATE_SKETCH_WIRE_CHECKS,
    V426_MOVE_FACE_IN_MIRROR,
    V427_LOCK_SWEEP_DIRECTION_HELIX,
    V428_EDGE_DISAMBIGUATION_BY_SIDES,
    V429_HOLE_SAFE_SKETCH_CLEANUP,
    V430_CONFIGURATIONS_PREP,
    V431_FIX_BODY_FILTERS,
    V432_RIB_GROUP_BOOLEANS,
    V437_CONSISTENT_BOUNDS,
    V438_RIB_EXTEND_LOGIC,
    V439_TWEAK_OP_TRACKING,
    V440_SYNTAX_ERRORS,
    V441_SHOW_LABEL_UIHINT,
    V442_PARTING_OUT_TARGET_READONLY,
    V447_SPLIT_SHEET_METAL,
    V448_OWNER_BODY_DISAMBIGUATION,
    V449_PS_VERSION_29_1_146,
    V450_SPLIT_TRACKING_MERGED_EDGES,
    V451_NO_MATECONNECTORS_IN_PATTERN,
    V452_PARAMETER_GROUP_UIHINT,
    V453_REMOVED_PARAMETER_GROUP,
    V454_CORRECT_IMPORT_DEFAULT,
    V455_HOLE_INIT_IS_TAPPED_THROUGH,
    V459_TOP_DISAMBIGUATION_IDS,
    V460_ENABLE_SHEET_METAL,
    V461_CURVE_PATTERN_FEATURE,
    V462_BLOCK_INCONTEXT_FACE_CURVE_PATTERN,
    V463_UPDATE_ISO_SCREW_STANDARDS,
    V464_CURVE_PATTERN_STABILIZATION,
    V467_STRICTER_DISAMBIGUATION,
    V468_PROPAGATE_MERGE_ERROR,
    V469_CHANGE_FACE_SWEPT_EDGES,
    V470_PS_VERSION_29_1_189,
    V471_FEATURE_LIST_KEY,
    V472_INCONTEXT_UPGRADE,
    V473_SHEET_METAL_COMMENTS,
    V474_MOVE_FACE_FIX,
    V475_KEEP_TOOLS_AUTO_SELECTION,
    V476_SM_BOOLEAN_PATTERN_ERROR,
    V477_END_SM_UPDATE,
    V481_CLEAR_SHEET_METAL_DATA_IN_SECTION,
    V482_PS_VERSION_29_1_199,
    V483_FLAT_QUERY_EVAL_FIX,
    V484_MOVE_FACE_0_DISTANCE,
    V485_EXTEND_SHEET_BODY_SPLIT,
    V486_MOVE_FACE_PROPAGATE_INFO,
    V487_IMPORT_FILTER_POINT_BODIES,
    V488_CLASSIFY_CORNER_RETURNS_MAP,
    V489_FLANGE_BUGS,
    V490_BEND_RELIEF_LOCATION,
    V491_CURVE_PATTERN_WIRES_ONLY,
    V492_MULTIPLE_OUTLINE_PIECES,
    V493_FLANGE_BASE_SHIFT_FIX,
    V494_RECORD_SWAP_MULTIPLICITY,
    V495_MOVE_FACE_ROTATION_AXIS,
    V496_COMPLEX_BEND_ENDS,
    V497_BLOCK_ILLEGAL_MODIFICATION,
    V498_SM_SPATIAL_VERTEX_MATCHING,
    V499_PS_VERSION_29_1_214,
    V500_EXTERNAL_DISAMBIGUATION,
    V501_BEND_END_ADAPTIVE_SMOOTHING,
    V502_CORNER_CLASSIFICATION_FIX,
    V503_EXTRUDE_WITH_OFFSET,
    V504_MODIFY_TO_SPLIT,
    V505_EXTRUDE_WITH_OFFSET_DIALOG,
    V512_MOVE_FACE_OVERLAP,
    V513_CONFIGURATION_DATA_UPGRADE,
    V514_OFFSET_FACE,
    V515_EXTRUDE_CHECK_LOCAL_STATUS,
    V516_SURFACE_BOOLEAN_EDGE_EVENTS,
    V517_MOVE_FACE_CHECK_BEND_EDGE,
    V518_MIRRORING_LIN_PATTERNS,
    V519_EXPANDED_CORNER_VERTEX_SET,
    V520_BLOCK_ILLEGAL_MODIFICATION,
    V521_SM_CLEARANCE,
    V522_MOVE_FACE_NONPLANAR,
    V523_SWEEP_PATH_START_VERTEX,
    V524_HOLE_TAP_MAJOR_DIAMETER,
    V525_SM_THICKEN_NO_NEIGHBORS,
    V526_FLANGE_SIDE_PLANE_DIR,
    V527_MOVE_FACE_MOVE_CREATE_TAB,
    V528_MOVE_FACE_MERGE,
    V529_MOVE_FACE_UI_UPDATES,
    V530_MOVE_FACE_SCOPE,
    V531_HOLE_MAJOR_DIAMETER_UPGRADE,
    V533_OFFSET_CIRCLE_HELP_POINTS,
    V534_INFORM_IN_CONTEXT_SM_THICKEN,
    V535_DETERMINISTIC_DISAMBIGUATION,
    V536_RIB_PROFILE_EXTN_CHECK_ANGLE,
    V537_IN_FLAT_QUERY,
    V538_GENERAL_SHEET_INTERSECTION,
    V539_SKETCH_CONICS,
    V540_SHEET_METAL_REMOVE_REDUNDANT,
    V541_REVERT_540,
    V542_REMAP_ATTACHED,
    V543_RECURSIVE_UNION,
    V546_GEOMETRY_QUERY_FIX,
    V547_SM_CORNERS_MERGE_SHORT_RIP_EDGES,
    V548_PS_VERSION_29_1_233,
    V549_SM_ALIGNMENT_APPROXIMATE_ANGLE,
    V550_EXTRUDE_OFFSET_VERIFICATION,
    V551_DELETE_FACE_NO_HEAL,
    V552_FILLET_EARLY_VERIFICATION,
    V553_MULTI_LOOP_ALIGNMENT,
    V554_SURFACE_JOIN,
    V555_CURVE_FEATURE_TEXT_CHANGES,
    V556_SURFACE_JOIN_FS_FIXES,
    V557_NO_BEND_CENTERLINE_IN_DERIVED,
    V558_PROJECT_EDGES_SAME_SKETCH,
    V559_REMOVE_LOGGING,
    V562_REMOVE_SCAR_EDGES,
    V563_EDGE_BLEND_FILTER_ORDER,
    V564_Q_FARTHEST_ALONG_SIGN,
    V565_COMPUTED_DATA_SKETCH_TRANSFORM,
    V566_MODIFIABLE_ONLY_IN_DERIVED,
    V567_GET_VARIABLE_THROWS,
    V568_RHO_DIMENSION_NOT_CONSISTENT,
    V569_FLANGE_NEXT_TO_RIP,
    V570_DISAMBIGUATION_PRESENCE,
    V571_HOLE_BLIND_IN_LAST_PROJECTION,
    V572_TAP_CLEARANCE_TO_REAL,
    V573_FS_ATTRIBUTES_MERGED_TO_WIRES,
    V574_SKETCH_PROJECT_VERTEX_TOLERANCE,
    V575_SHEET_METAL_FILLET_CHAMFER,
    V576_GET_WIRE_LAMINAR_DEPENDENCIES,
    V577_CORNER_BREAK_ADJUST,
    V578_FIX_DELETE_FACE_MODIFICATION,
    V579_FIT_SPLINE_ZERO_MAGNITUDE,
    V580_CORNER_BREAK_STABILIZATION,
    V581_REMOVE_HELIX_CONTROL_VISIBILITY,
    V582_MERGE_MAPS_BUG,
    V583_AVOID_SKETCH_TEXT_TOUCHING,
    V584_FILTER_FACES_IN_THICKEN,
    V585_SHOW_TAPPED_DEPTH_UPDATE,
    V586_DUPLICATE_EXTRUDE_TRANSFORM,
    V587_SPINE_IN_LOFT,
    V588_MERGE_IN_FILLET_FIX,
    V589_STABLE_BREAK_REMOVAL,
    V590_DEFER_SM_BODY_DELETE,
    V591_BREAK_TOUCHING_WALL,
    V592_LOFT_FS_CHANGES_IN_REL,
    V593_MASTER_MODEL_DOC,
    V596_ENCLOSE_FEATURE,
    V597_VERTEX_TOL_LIMITS,
    V598_EXTRUDE_NO_FLAT,
    V599_SM_ASSOCIATION_FIX,
    V600_PIERCE_COEDGE,
    V601_CHECK_IMPORT_NAMES,
    V602_TRACE_THROUGH_COPY,
    V603_SUBQUERY_EVALUATION,
    V604_DCM_VERSION_67_5_1,
    V605_EXTRUDE_CAP_ENTITIES,
    V606_TOLERANT_BRIDGING_CURVE,
    V607_HOLE_FEATURE_FIT_UPDATE,
    V608_MERGE_FROM_TOOLS,
    V614_CHANGE_FACE_GROW,
    V615_DCM_VERSION_67_5_2,
    V616_DRILL_SIZES_UPDATE,
    V617_GEOM_BUGS,
    V618_SKETCH_LIMIT_DIMENSIONS,
    V619_PS_VERSION_30_0_185,
    V620_DONT_MERGE_SECTION_FACE,
    V621_SHEET_METAL_HOLES,
    V622_LOFT_GUIDE_WITH_END_CONDITION,
    V623_SHEETMETAL_HOLE_ATTRIBUTE_FIX,
    V624_FILL_SURFACE_UI_UPDATES,
    V625_FILL_FS_FIXES,
    V626_COLLAPSE_ARRAY_ITEMS_UIHINT,
    V628_LOFT_BUG_FIXES,
    V629_SM_MODEL_FRONT_N_BACK,
    V630_SM_BOOLEAN_NOOP_HANDLING,
    V631_THROW_ID_TREE_ERROR,
    V632_PROFILE_THROUGH_ALL,
    V633_EXTRUDE_CONNECTED_PROFILE,
    V634_MERGE_FROM_TOOLS_EVAL,
    V635_PREVENT_MOVE_FACE_DISASSOCIATION,
    V636_CONIC_FILLET_API_UPDATE,
    V637_PS_VERSION_30_0_185,
    V638_LONGER_BEND_RELIEF_AT_START,
    V644_TYPE_FOR_VARIABLE_FEATURE,
    V645_LOFT_BUGS,
    V646_SM_MULTI_TOOL_FIX,
    V647_ENCLOSE_DELETE_MODIFIABLE_TOOLS,
    V648_EXTRUDE_REFERENCE_ALL_PROFILES,
    V649_FLANGE_LOOSEN_EDGE_Y,
    V650_TAB_IDENTITY,
    V651_BLOCK_SM_FEATURE_PATTERN,
    V652_DCM_VERSION_67_5_4,
    V653_EV_EDGE_TANGENT_LINES_FIX,
    V654_MISMATCHED_BEND_END,
    V655_EXTEND_SHEET_BODY_WITH_SPLIT,
    V656_VRFILLET_SERVER,
    V657_SURFACE_JOIN_BUGS,
    V658_SURFACE_JOIN_FS_CHANGES,
    V659_HOLE_FEATURE_INITIAL_FOCUS_BUG,
    V660_ENCLOSE_AUTO_MATCHING,
    V663_REVOLVE_ID_FIX,
    V664_FLAT_JOINT_TO_RIP,
    V665_PERTURBED_SM_TOL,
    V666_FEATURE_PATTERN_ENTITIES,
    V667_PS_VERSION_30_0_226,
    V668_ASME_HOLE_SCREW_CLEARANCES,
    V669_TIGHTER_SKETCH_OFFSET_APPROXIMATION,
    V670_VR_FILLET_UPGRADE_TASK,
    V671_FRESH_CORNER_OVERRIDE,
    V672_TAB_IDENTITY,
    V673_VRFILLET_FIXES,
    V674_TAB_FIXES,
    V675_MORE_TAB_FIXES,
    V680_SHELL_RECORD_FIX,
    V681_ANGLE_BOUNDS_CHANGE,
    V682_REPLACE_FACE_NO_MERGE,
    V683_LOFT_ARRAY_PARAMETERS,
    V684_SM_SWEPT_SUPPORT,
    V685_EXTEND_SHEET_BODY_STEP_EDGES,
    V686_LOFT_FS_BUG_FIXES,
    V691_FILL_GUIDE_CURVES,
    V692_VECTOR_IMPRINT,
    V693_SM_PATTERN,
    V694_FILL_GUIDE_CURVES_FS,
    V695_SM_SWEPT_SUPPORT,
    V696_REMOVE_ADDED_REDUNDANCY,
    V697_FILL_UPDATE,
    V698_EXTEND_SHEET_PERIODIC,
    V699_SM_PATTERN_FIXES,
    V700_REVERTED_TANGENT_JOINT,
    V701_REVERTED_SM_FACE_PATTERN,
    V703_EXTERNAL_ROLL_BACK,
    V704_MOVE_FACE_ROLLED_SM,
    V705_G2_CURVES,
    V706_SM_PATTERN_RIP,
    V707_FITSPLINE_UPGRADE,
    V708_SM_BOOLEAN,
    V711_LOFT_BUG_FIX,
    V712_SKIP_TARGET_BOOLEAN,
    V713_SM_PATTERN_COLORS,
    V714_SM_BEND_DETERMINISM,
    V715_SM_PATTERN_FAIL_MIRROR,
    V716_PS_VERSION_30_0_280,
    V717_DEFER_SM_BODY_DELETE_2,
    V718_ADD_DEPRECATED_FS_FOR_MOBILE,
    V722_DELETE_FACE_REDUNDANCY,
    V723_REMAP_TAB_BREAKS,
    V724_SM_MAKE_JOINT_TYPE,
    V725_FLANGE_ROLLED_SIDE,
    V726_ROLLED_CORNER_RELIEF,
    V727_SM_SUPPORT_ROLLED,
    V728_ACTIVE_SKETCH_ON_FLAT,
    V729_UPGRADE_FIXES,
    V730_EXTEND_SHEET_BODY_INTERSECTION_CLASSIFICATION,
    V731_SM_BLEND_PARTIAL,
    V732_HOLE_PROPAGATE_EDGE,
    V733_EDGE_CHANGE,
    V734_PS_VERSION_30_1_168,
    V735_MOVE_FACE_BUG_FIXES,
    V736_SM_74,
    V740_PROPAGATE_PROPERTIES_IN_PATTERNS,
    V741_FLATTENED_BEND_SWAP,
    V742_REPLACE_FACES_UPGRADE,
    V743_EDGE_ORIENTATION,
    V744_SM_FLANGE_PATTERN_EDGE_CHANGE,
    V745_COMPRESSED_QUERIES,
    V746_EXTERNAL_DISAMBIGUATION,
    V747_SPLIT_ALLOW_FACES,
    V748_SM_FAIL_SHEET_DELETION,
    V749_SPLIT_UX_FIXES,
    V758_PATTERN_MATE_CONNECTORS,
    V759_MULTI_MODEL_SM_REBUILD,
    V760_LOFT_REFACTOR,
    V761_WIRE_EDGE_DET_ID,
    V762_FLANGE_NEAR_ROLLED,
    V763_HOLE_CUT_ALL,
    V764_EDGE_PATTERN,
    V765_SM_PATTERN_FAIL_MIRROR_2,
    V775_DETACHED_FILLET,
    V776_SURFACE_JOIN_BUG_FIX,
    V777_HOLE_ATTRIBUTE_FIX,
    V778_MOVE_FACE_UPGRADE,
    V779_BOOLEAN_TRACK_MERGE,
    V780_THICKEN_STRICT_ORDER,
    V781_THREE_BEND_SIZED,
    V782_HOLE_ERROR_REPORTING,
    V792_OFFSET_CORRECT_LOOP,
    V793_CHECK_UNROLLED_MATCH,
    V794_EDGE_PATTERN_BREAKS,
    V795_SPLIT_FIXES,
    V796_PLANE_ORIENTATION_INSTABILITY,
    V797_EDGE_DISAMBIGUATION,
    V798_BOOLEAN_TRACKING,
    V799_FIX_UNROLL_ORIGIN,
    V810_EXTRUDE_OFFSET_EDGE_CAP,
    V811_BRACKETS_CONFIG_ACCESS,
    V812_PS_VERSION_30_1_232,
    V813_FILLET_BEND,
    V814_EDGE_CHANGE_PRECISION,
    V815_STABLE_SWEPT_EDGE,
    V816_FAST_FEATURE_PATTERN,
    V817_SM_OPTIMIZATION,
    V818_FFP_FIX,
    V819_ERROR_STRING_ENUM_ASSEMBLY_PATTERN_EXCEED_MAX_INSTANCE_COUNT,
    V828_DRAFT_ONLY_SELECTED,
    V829_REDEFINE_BEND_RELIEF_SCALE,
    V830_SWEPT_EDGE_ON_VERTEX,
    V831_HOLE_TOLERANT_DIST,
    V832_PARTING_LINE_DRAFT,
    V833_PARTING_LINE_DRAFT_FIXES,
    V834_PARTING_LINE_DRAFT_FLIP,
    V843_RESTORE_COPY_VS_MODIFY,
    V844_PS_VERSION_30_1_256,
    V845_SM_BUG_FIXES,
    V846_ERROR_STRING_VERSION_BUMP,
    V847_PL_DRAFT_STABLE,
    V856_EVCURVATURE_RENAME,
    V857_BEND_INTERVAL,
    V858_SM_FLAT_BUG_FIXES,
    V859_SM_HOLE_ATTRIBUTE,
    V860_SM_FLAT_ERRORS,
    V870_DELETE_SKETCH_UNFOLD_FAILURE,
    V871_MATE_CONNECTORS_AS_AXES,
    V872_DCM_VERSION_68_2_2,
    V873_SM_OFFSET_ORDER,
    V874_CORNER_BY_BODY,
    V875_MC_USE_BOTTOM_AXIS_FIX,
    V876_PS_VERSION_31_0_154,
    V877_SM_CORNERS,
    V888_MATE_CONNECTOR_REMAP,
    V889_IMPORT_WITH_FAULTS_PARAM,
    V890_FIX_FIT_SPLINE,
    V891_EXPOSE_REFERENCE_PARAMETER,
    V900_USE_CIRCLE,
    V901_DET_ID_FIRST,
    V912_TRACK_TRANFORMS,
    V913_TOOL_CLASH_FINE,
    V914_SM_JOINT_TO_WALL,
    V915_PS_VERSION_31_0_203,
    V916_DERIVED_VISIBILITY,
    V917_SM_BOOLEAN_MATCH,
    V918_SM_BOOLEAN_TOOLS,
    V919_SPLIT_JOINT_TYPO,
    V920_FS_ROUND_TO_PRECISION,
    V929_SM_ALIGN_UNROLLED,
    V930_SUBQUERY_STABILIZATION,
    V931_MESH_BOOLEAN,
    V932_SPLIT_PART_BOX,
    V933_SM_MOVE_FACE_NO_MERGE,
    V934_FIT_SPLINE_PARAM,
    V935_FILL_TRACKING_FIXES,
    V936_CUSTOM_PROPERTY_DOC_FIX,
    V937_SM_RADIUS_BOUNDS_FIX,
    V945_STOP_REQUIRING_MATE_CONNECTOR_OWNER_PART,
    V946_BEND_RELIEF_PLACEMENT,
    V947_EVDISTANCE_ARCLENGTH,
    V948_BOOLEAN_TOOLS_STRICTER,
    V949_BLOCK_EXCESSIVE_MERGE,
    V950_PARTIAL_BEND_CUT,
    V951_FAIL_SURFACE_BOOLEAN,
    V959_SKETCH_FAIL_SPLIT_VERTEX,
    V960_HOLE_IDENTITY,
    V961_HEM_AS_BEND,
    V972_PS_VERSION_31_0_249,
    V973_TOLERANT_LOFT,
    V974_ERROR_AFTER_SPLIT,
    V975_BEND_LINE_EXTENTS,
    V984_DCM_VERSION_69_1_0,
    V985_BEND_SMOOTHING,
    V986_STRICT_INTERSECT,
    V987_DERIP_SIGNED_DISTANCE,
    V988_FIX_HELIX_START,
    V989_ENCLOSE_DONT_DELETE_SKETCHES,
    V990_REFERENCE_PARAMETER_IN_INSTANCE,
    V991_ROLL_SURFACE_BUMP,
    V992_FLANGE_END_FIX,
    V993_CLAMP_BASE_CONTEXT_VERSION,
    V1000_FEATURESCRIPT_V1K,
    V1001_COMPARE_ORIGINALS,
    V1002_UNHIDE_MATE_CONNECTOR_FLIP_PARAMETERS,
    V1003_DUMMY_FEATURE,
    V1004_MATE_CONNECTOR_AS_PLANE,
    V1005_INTERNAL_HEM,
    V1006_CPLANE_MATE_CONNECTOR,
    V1007_RENAME_AXIS_PARAM,
    V1008_SKIP_MATE_CONNECTOR_PATTERNS,
    V1009_SM_BOOLEAN_TRACK,
    V1010_SECTION_PART_NO_NEGATIVE_DEPTH,
    V1015_SUBSTUTUTION_EVAL,
    V1016_EXPECTED_VECTOR_SIZE,
    V1017_SUBTRACT_COMPLEMENT,
    V1018_DERIVED,
    V1019_PS_VERSION_31_1_188,
    V1020_SURFACE_BOOLEAN,
    V1021_HOLE_MATE_CONNECTOR_CSYS,
    V1022_FIX_MASS_PROPS,
    V1023_FIX_HOLE_NAME,
    V1024_UPDATE_DOCS,
    V1031_BODY_NET_IN_LOFT,
    V1032_MESH_SCALING_IN_TRANSLATOR,
    V1033_BODY_NET_IN_FILL,
    V1034_EVRAYCAST_FIX,
    V1035_SURFACE_MATCH,
    V1036_IMPORT_NAMES_AND_COLORS,
    V1044_COPLANAR_PLANES,
    V1045_SHEET_BOOLEAN_ALIGN_FACE,
    V1046_FAILED_PARAMETERS,
    V1047_MOVE_FACE_JOINTS,
    V1048_FLANGE_AND_HEM_EDGES,
    V1049_COMMON_FS,
    V1050_HEM_REATED_FIXES,
    V1051_SM_MOVE_FACE_BEND_ERROR,
    V1052_HEM_BUMP,
    V1053_HEM_RELEASE,
    V1058_PS_VERSION_31_1_232,
    V1059_SM_ALIGMENT,
    V1060_SET_CHAMFERS_SEPARATELY,
    V1061_DISAMBIGUATE_COPY,
    V1062_GET_SM_ENTS,
    V1063_ORPHANED_BEND,
    V1073_REFINE_SKETCH_INTERSECTIONS,
    V1074_SKIP_IN_CONTEXT_PATTERN,
    V1075_DEDUPLICATE_CREATE,
    V1076_TRANSIENT_QUERY,
    V1077_ERROR_STRING,
    V1086_EDGE_PATTERN_THICKNESS,
    V1087_EXTEND_SURFACE,
    V1088_SUBSTITUTION_EVAL,
    V1089_SKETCH_CURVATURE_CONSTRAINT,
    V1090_KEEP_SKETCHES_IN_SECTION_CUT,
    V1091_BUMP_FOR_FEATURE_SPECS_TRANSLATION,
    V1092_SURFACE_MATE_CONNECTOR_OWNER,
    V1093_HOLE_THREAD_FIX,
    V1094_HOLE_SM_FIX,
    V1095_INTERNAL_FEATURE,
    V1096_FIX_STD,
    V1101_SM_BEND_END,
    V1102_DCM_VERSION_70_0_0,
    V1103_CONSTRUCTION_VERTEX_CHANGE,
    V1104_WRAP,
    V1105_ALLOW_SHEETS_IN_SECTION,
    V1106_WRAP_BUMP,
    V1107_HOLE_SELECTION,
    V1108_HOLE_TAP_FIX,
    V1109_CLUSTER_VERTICES,
    V1110_WRAP_POSTPONED,
    V1111_LOFT_VERTEX,
    V1112_LOFT_VERTEX_REGEN,
    V1115_SM_CORNER_FIX,
    V1116_WRAP_OVERLAP_CHECK,
    V1117_NO_GUIDES_ERROR,
    V1118_SKETCH_TEXT_CONSTRUCTION,
    V1119_INTERNAL_ENUM_VALUE,
    V1120_SCALE_NONUNIFORMLY,
    V1128_PATTERN_OF_ONE,
    V1129_NEWLY_LAMINAR_SPLIT,
    V1130_SURFACING_IMPROVEMENTS,
    V1131_DISALLOW_SPLIT_IN_COPY,
    V1132_WRAP_RELEASE,
    V1133_BLIND_IN_LAST_FIX,
    V1134_WRAP_BUMP,
    V1135_HOLE_TAP_CHECK,
    V1142_HOLE_FIXES,
    V1143_ROLLED_OUTLINE_INNER_LOOPS,
    V1144_PS_VERSION_32_0_152,
    V1145_SKETCH_HELPERS_CONSTRUCTION,
    V1146_COMPOSITE_ERROR_VERSION_BUMP,
    V1147_EDGE_BLEND_HISTORY,
    V1148_SECTION_PART,
    V1149_COMPOSITE_INPUT_CHECKS,
    V1150_COMPOSITE_FEATURE_PATTERN,
    V1156_PS_VERSION_32_0_170,
    V1157_UNSTABLE_COMP_QUERY,
    V1158_SM_UNFOLDER,
    V1159_BETTER_HOLE_TAGS,
    V1160_SECTION_PART,
    V1169_UVMAPPER_TOLERANCE,
    V1170_ROLLED_OUTLINE_REVERT,
    V1171_BCURVE_DEFINITION_ADJUST,
    V1172_LOFT_BUG,
    V1173_BSPLINESRUFACE_BUMP,
    V1174_BSPLINESRUFACE_BUMP_2
}

/**
 * @internal
 * The current FeatureScript version.
 *
 * The version at regeneration stored on the `context`, so logic which checks the
 * FeatureScript version should instead call
 * `isAtVersionOrLater(context, version)`
 */
export const FeatureScriptVersionNumberCurrent is FeatureScriptVersionNumber = FeatureScriptVersionNumber.V1174_BSPLINESRUFACE_BUMP_2;


