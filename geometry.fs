FeatureScript 2559; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

/**
 * This module makes all Onshape Standard Library features and functions
 * available.
 *
 * New Feature Studios begin with an import of this module,
 * ```import(path : "onshape/std/geometry.fs", version : "");```
 * with current version inserted (e.g. `300.0`). This gives that Feature Studio
 * access to all functions, types, enums, and constants defined in
 * the Onshape Standard Library.
 */
/* Common Onshape standard library functions */
export import(path : "onshape/std/common.fs", version : "2559.0");

/* Onshape standard library features */
export import(path : "onshape/std/bodyDraft.fs", version : "2559.0");
export import(path : "onshape/std/bridgingCurve.fs", version : "2559.0");
export import(path : "onshape/std/bsurf.fs", version : "2559.0");
export import(path : "onshape/std/chamfer.fs", version : "2559.0");
export import(path : "onshape/std/compositeCurve.fs", version : "2559.0");
export import(path : "onshape/std/compositePart.fs", version : "2559.0");
export import(path : "onshape/std/cplane.fs", version : "2559.0");
export import(path : "onshape/std/cpoint.fs", version : "2559.0");
export import(path : "onshape/std/cutlist.fs", version : "2559.0");
export import(path : "onshape/std/decal.fs", version : "2559.0");
export import(path : "onshape/std/deleteBodies.fs", version : "2559.0");
export import(path : "onshape/std/deleteFace.fs", version : "2559.0");
export import(path : "onshape/std/draft.fs", version : "2559.0");
export import(path : "onshape/std/editCurve.fs", version : "2559.0");
export import(path : "onshape/std/enclose.fs", version : "2559.0");
export import(path : "onshape/std/extend.fs", version : "2559.0");
export import(path : "onshape/std/externalThread.fs", version : "2559.0");
export import(path : "onshape/std/extrude.fs", version : "2559.0");
export import(path : "onshape/std/endcap.fs", version : "2559.0");
export import(path : "onshape/std/faceBlend.fs", version : "2559.0");
export import(path : "onshape/std/faceIntersection.fs", version : "2559.0");
export import(path : "onshape/std/fillSurface.fs", version : "2559.0");
export import(path : "onshape/std/fillet.fs", version : "2559.0");
export import(path : "onshape/std/fitSpline.fs", version : "2559.0");
export import(path : "onshape/std/frame.fs", version : "2559.0");
export import(path : "onshape/std/frameAttributes.fs", version : "2559.0");
export import(path : "onshape/std/frameTrim.fs", version : "2559.0");
export import(path : "onshape/std/frameUtils.fs", version : "2559.0");
export import(path : "onshape/std/gtolconstrainttype.gen.fs", version : "2559.0");
export import(path : "onshape/std/gusset.fs", version : "2559.0");
export import(path : "onshape/std/helix.fs", version : "2559.0");
export import(path : "onshape/std/hole.fs", version : "2559.0");
export import(path : "onshape/std/holeTable.fs", version : "2559.0");
export import(path : "onshape/std/importDerived.fs", version : "2559.0");
export import(path : "onshape/std/importForeign.fs", version : "2559.0");
export import(path : "onshape/std/isocline.fs", version : "2559.0");
export import(path : "onshape/std/isoparametricCurve.fs", version : "2559.0");
export import(path : "onshape/std/loft.fs", version : "2559.0");
export import(path : "onshape/std/lofttopology.gen.fs",  version : "2559.0");
export import(path : "onshape/std/massProperty.fs", version : "2559.0");
export import(path : "onshape/std/mateConnector.fs", version : "2559.0");
export import(path : "onshape/std/mirror.fs", version : "2559.0");
export import(path : "onshape/std/modifyFillet.fs", version : "2559.0");
export import(path : "onshape/std/moveCurveBoundary.fs", version : "2559.0");
export import(path : "onshape/std/moveFace.fs", version : "2559.0");
export import(path : "onshape/std/mutualTrim.fs", version : "2559.0");
export import(path : "onshape/std/nameEntity.fs", version : "2559.0");
export import(path : "onshape/std/offsetCurveOnFace.fs", version : "2559.0");
export import(path : "onshape/std/offsetSurface.fs", version : "2559.0");
export import(path : "onshape/std/pattern.fs", version : "2559.0");
export import(path : "onshape/std/projectCurves.fs", version : "2559.0");
export import(path : "onshape/std/replaceFace.fs", version : "2559.0");
export import(path : "onshape/std/revolve.fs", version : "2559.0");
export import(path : "onshape/std/rib.fs", version : "2559.0");
export import(path : "onshape/std/ruledSurface.fs", version : "2559.0");
export import(path : "onshape/std/sectionpart.fs", version : "2559.0");
export import(path : "onshape/std/sheetMetalBend.fs", version : "2559.0");
export import(path : "onshape/std/sheetMetalBendRelief.fs", version : "2559.0");
export import(path : "onshape/std/sheetMetalCorner.fs", version : "2559.0");
export import(path : "onshape/std/sheetMetalCornerBreakAttributeBased.fs", version : "2559.0");
export import(path : "onshape/std/sheetMetalCornerBreak.fs", version : "2559.0");
export import(path : "onshape/std/sheetMetalEnd.fs", version : "2559.0");
export import(path : "onshape/std/sheetMetalFlange.fs", version : "2559.0");
export import(path : "onshape/std/sheetMetalHem.fs", version : "2559.0");
export import(path : "onshape/std/sheetMetalJoint.fs", version : "2559.0");
export import(path : "onshape/std/sheetMetalMakeJoint.fs", version : "2559.0");
export import(path : "onshape/std/sheetMetalRip.fs", version : "2559.0");
export import(path : "onshape/std/sheetMetalStart.fs", version : "2559.0");
export import(path : "onshape/std/sheetMetalTab.fs", version : "2559.0");
export import(path : "onshape/std/sheetMetalUnfold.fs", version : "2559.0");
export import(path : "onshape/std/shell.fs", version : "2559.0");
export import(path : "onshape/std/splitpart.fs", version : "2559.0");
export import(path : "onshape/std/sweep.fs", version : "2559.0");
export import(path : "onshape/std/tag.fs", version : "2559.0");
export import(path : "onshape/std/thicken.fs", version : "2559.0");
export import(path : "onshape/std/transformCopy.fs", version : "2559.0");
export import(path : "onshape/std/wrap.fs", version : "2559.0");

