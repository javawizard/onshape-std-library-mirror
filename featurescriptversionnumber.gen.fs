FeatureScript 347; /* Automatically generated version */
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
    V347_SHEET_METAL_FLATTEN
}

/**
 * @internal
 * The current FeatureScript version.
 *
 * The version at regeneration stored on the `context`, so logic which checks the
 * FeatureScript version should instead call
 * `isAtVersionOrLater(context, version)`
 */
export const FeatureScriptVersionNumberCurrent is FeatureScriptVersionNumber = FeatureScriptVersionNumber.V347_SHEET_METAL_FLATTEN;


