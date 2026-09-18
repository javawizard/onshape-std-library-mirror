FeatureScript 3083; /* Automatically generated version */
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
export import(path : "onshape/std/common.fs", version : "3083.0");

/* Onshape standard library features */
export import(path : "onshape/std/annotationdimensiondirection.gen.fs", version : "3083.0");
export import(path : "onshape/std/annotationattachmentlocation.gen.fs", version : "3083.0");
export import(path : "onshape/std/bodyDraft.fs", version : "3083.0");
export import(path : "onshape/std/bridgingCurve.fs", version : "3083.0");
export import(path : "onshape/std/bsurf.fs", version : "3083.0");
export import(path : "onshape/std/chamfer.fs", version : "3083.0");
export import(path : "onshape/std/compositeCurve.fs", version : "3083.0");
export import(path : "onshape/std/compositePart.fs", version : "3083.0");
export import(path : "onshape/std/constrainedSurface.fs", version : "3083.0");
export import(path : "onshape/std/cplane.fs", version : "3083.0");
export import(path : "onshape/std/cpoint.fs", version : "3083.0");
export import(path : "onshape/std/cutlist.fs", version : "3083.0");
export import(path : "onshape/std/decal.fs", version : "3083.0");
export import(path : "onshape/std/deleteBodies.fs", version : "3083.0");
export import(path : "onshape/std/deleteFace.fs", version : "3083.0");
export import(path : "onshape/std/derivedMirror.fs", version : "3083.0");
export import(path : "onshape/std/draft.fs", version : "3083.0");
export import(path : "onshape/std/editCurve.fs", version : "3083.0");
export import(path : "onshape/std/enclose.fs", version : "3083.0");
export import(path : "onshape/std/extend.fs", version : "3083.0");
export import(path : "onshape/std/externalThread.fs", version : "3083.0");
export import(path : "onshape/std/extrude.fs", version : "3083.0");
export import(path : "onshape/std/endcap.fs", version : "3083.0");
export import(path : "onshape/std/faceBlend.fs", version : "3083.0");
export import(path : "onshape/std/faceIntersection.fs", version : "3083.0");
export import(path : "onshape/std/fieldweldflag.gen.fs", version : "3083.0");
export import(path : "onshape/std/fillSurface.fs", version : "3083.0");
export import(path : "onshape/std/fillet.fs", version : "3083.0");
export import(path : "onshape/std/fitSpline.fs", version : "3083.0");
export import(path : "onshape/std/frame.fs", version : "3083.0");
export import(path : "onshape/std/frameAttributes.fs", version : "3083.0");
export import(path : "onshape/std/frameTrim.fs", version : "3083.0");
export import(path : "onshape/std/frameUtils.fs", version : "3083.0");
export import(path : "onshape/std/gtolconstrainttype.gen.fs", version : "3083.0");
export import(path : "onshape/std/gtolextendedconstrainttype.gen.fs", version : "3083.0");
export import(path : "onshape/std/gusset.fs", version : "3083.0");
export import(path : "onshape/std/helix.fs", version : "3083.0");
export import(path : "onshape/std/hole.fs", version : "3083.0");
export import(path : "onshape/std/holeTable.fs", version : "3083.0");
export import(path : "onshape/std/importDerived.fs", version : "3083.0");
export import(path : "onshape/std/importForeign.fs", version : "3083.0");
export import(path : "onshape/std/isocline.fs", version : "3083.0");
export import(path : "onshape/std/isoparametricCurve.fs", version : "3083.0");
export import(path : "onshape/std/loft.fs", version : "3083.0");
export import(path : "onshape/std/lofttopology.gen.fs",  version : "3083.0");
export import(path : "onshape/std/massProperty.fs", version : "3083.0");
export import(path : "onshape/std/mateConnector.fs", version : "3083.0");
export import(path : "onshape/std/mirror.fs", version : "3083.0");
export import(path : "onshape/std/modifyFillet.fs", version : "3083.0");
export import(path : "onshape/std/moveCurveBoundary.fs", version : "3083.0");
export import(path : "onshape/std/moveFace.fs", version : "3083.0");
export import(path : "onshape/std/mutualTrim.fs", version : "3083.0");
export import(path : "onshape/std/nameEntity.fs", version : "3083.0");
export import(path : "onshape/std/offsetCurveOnFace.fs", version : "3083.0");
export import(path : "onshape/std/offsetSurface.fs", version : "3083.0");
export import(path : "onshape/std/pattern.fs", version : "3083.0");
export import(path : "onshape/std/pcbStart.fs", version : "3083.0");
export import(path : "onshape/std/pcbEnd.fs", version : "3083.0");
export import(path : "onshape/std/projectCurves.fs", version : "3083.0");
export import(path : "onshape/std/queryVariable.fs", version : "3083.0");
export import(path : "onshape/std/replaceFace.fs", version : "3083.0");
export import(path : "onshape/std/revolve.fs", version : "3083.0");
export import(path : "onshape/std/rib.fs", version : "3083.0");
export import(path : "onshape/std/routingCurve.fs", version : "3083.0");
export import(path : "onshape/std/ruledSurface.fs", version : "3083.0");
export import(path : "onshape/std/sectionpart.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalBend.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalBendRelief.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalCorner.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalCornerBreakAttributeBased.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalCornerBreak.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalEnd.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalFlange.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalFormed.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalHem.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalJog.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalJoint.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalLoft.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalMakeJoint.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalRip.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalStart.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalTab.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalUnfold.fs", version : "3083.0");
export import(path : "onshape/std/shell.fs", version : "3083.0");
export import(path : "onshape/std/splitpart.fs", version : "3083.0");
export import(path : "onshape/std/sweep.fs", version : "3083.0");
export import(path : "onshape/std/tag.fs", version : "3083.0");
export import(path : "onshape/std/thicken.fs", version : "3083.0");
export import(path : "onshape/std/transformCopy.fs", version : "3083.0");
export import(path : "onshape/std/weldcontourtype.gen.fs", version : "3083.0");
export import(path : "onshape/std/weldfinishing.gen.fs", version : "3083.0");
export import(path : "onshape/std/weldjointtype.gen.fs", version : "3083.0");
export import(path : "onshape/std/weldstandard.gen.fs", version : "3083.0");
export import(path : "onshape/std/weldtype.gen.fs", version : "3083.0");
export import(path : "onshape/std/wrap.fs", version : "3083.0");

