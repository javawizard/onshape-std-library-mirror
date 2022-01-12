FeatureScript 1675; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * For a [HoleProfile], a reference point along the hole axis to which the `position` of the profile is relative.
 * To calculate these references, the cylinder of the hole is intersected with the `targets` of the hole. If the hole
 * cylinder encounters a slanted or otherwise irregular face at the intersection with the target, the reference may by a range
 * rather than a single point. Users may specify that they want the end of this range by using a [holeProfile], or
 * the beginning of this range by using a [holeProfileBeforeReference]; or they may control this behavior directly by setting
 * the `beforeReference` flag of the `HoleProfile`.
 *
 * Typically, the beginning of the reference range should be used for the first profile of the hole, and the end of the
 * reference range should be used for the rest of the profiles of the hole. Using the beginning of the reference range
 * for the first profile ensures that when the hole tool is cut from a target with a slanted or otherwise irregular
 * entrance face, the first profile is far back enough to ensure that no undesirable overhang is left behind on the
 * target, preventing the fastener from entering the hole. Using the end of the reference range for the rest of the
 * profiles ensures that any nominal distances are measured from where the hole cylinder fully enters the part. As
 * an example of both of these concepts, set the first two profiles of a [HoleDefinition] as a
 * [holeProfileBeforeReference] referencing `TARGET_START` with a `position` of 0 inches, and a [holeProfile] referencing
 * `TARGET_START` with a `position` of 2 inches, respectively. If this layout of profiles is used against a target that has
 * a slanted entrance face, the first profile will be placed right where the cylinder first intersects the part, ensuring
 * that there is no overhang when the hole tool is subtracted from the target.  The second profile will be placed 2 inches
 * from where the hole cylinder fully enters the target, such that the request for the hole to be 2 inches deep is understood as
 * the hole having a full two inches of depth in the target, exluding the area where the hole is only partially cut into the
 * target. Notably, to accomplish this the first and second profiles are placed more than two inches apart.
 *
 * When calculating references, any intersection which is fully behind the `origin` of the axis [Line] (i.e. the axis point)
 * is always ignored. Range intersections where the end of the range is ahead of the axis point but the start of the range is
 * behind the axis point are not ignored.
 *
 * @value AXIS_POINT: When calling [opHole], the user will supply one or more `axes` defining where the holes are to be placed.
 *     When using this reference, the `position` of the profile will be in reference to the `origin` of the axis [Line] of the hole.
 * @value TARGET_START: When using this reference, the `position` of the profile will be in reference to the closest intersection
 *     of the hole cylinder with any target.
 * @value LAST_TARGET_START: For each target, find the first intersection of the hole cylinder into the target. When using this
 *     reference, the `position` of the profile will be in reference to the furthest of those intersections. Notably, this means
 *     that if certain targets are geometrically constructed such that the hole cylinder enters and exits the target multiple
 *     times, only the first of the entrances into the target is considered.
 * @value LAST_TARGET_END: When using this reference, the `position` of the profile will be in reference to the furthest exit
 *     of the hole cylinder from any target.
 */
export enum HolePositionReference
{
    AXIS_POINT,
    TARGET_START,
    LAST_TARGET_START,
    LAST_TARGET_END
}


