FeatureScript 1095; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/booleanoperationtype.gen.fs", version : "1095.0");
import(path : "onshape/std/boundingtype.gen.fs", version : "1095.0");
import(path : "onshape/std/containers.fs", version : "1095.0");
import(path : "onshape/std/evaluate.fs", version : "1095.0");
import(path : "onshape/std/feature.fs", version : "1095.0");
import(path : "onshape/std/math.fs", version : "1095.0");
import(path : "onshape/std/string.fs", version : "1095.0");
import(path : "onshape/std/topologyUtils.fs", version : "1095.0");
import(path : "onshape/std/transform.fs", version : "1095.0");
import(path : "onshape/std/valueBounds.fs", version : "1095.0");
import(path : "onshape/std/vector.fs", version : "1095.0");

/**
 * Specifies the direction of the rib extrusion starting from the profile
 * going up to the part.
 *
 * @value NORMAL_TO_SKETCH_PLANE : The direction of the rib extrusion goes normal to the profile sketch plane.
 * @value PARALLEL_TO_SKETCH_PLANE : The direction of the rib extrusion goes parallel to the profile sketch plane.
 */
export enum RibExtrusionDirection
{
    annotation { "Name" : "Parallel to sketch plane" }
    PARALLEL_TO_SKETCH_PLANE,
    annotation { "Name" : "Normal to sketch plane" }
    NORMAL_TO_SKETCH_PLANE
}

/**
 * Creates ribs from selected profiles. The ribs can be either free standing or merged with their mating part.
 * Profiles must be non-construction sketch edges.
 * @param id : @autocomplete `id + "rib1"`
 * @param definition {{
 *      @field profiles {Query}:
 *              Edges which form the center lines of the ribs.
 *      @field parts {Query}:
 *              Parts which form the boundary of the ribs.
 *      @field thickness {ValueWithUnits}:
 *              Thickness of the ribs.
 *      @field ribExtrusionDirection {RibExtrusionDirection}:
 *              Whether the rib is extruded perpendicular or parallel to the plane.
 *      @field oppositeDirection {boolean}:
 *              Whether the ribs are extruded in the positive or negative direction.
 *      @field extendProfilesUpToPart {boolean}:
 *              Whether the ribs are extruded up to a boundary part.
 *      @field mergeRibs {boolean}:
 *              Whether the ribs are merged with the mating part.
 * }}
 */
annotation { "Feature Type Name" : "Rib", "Editing Logic Function" : "ribEditLogic" }
export const rib = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Sketch profiles", "Filter" : EntityType.EDGE && SketchObject.YES && ConstructionObject.NO }
        definition.profiles is Query;

        annotation { "Name" : "Parts", "Filter" : EntityType.BODY && BodyType.SOLID && ModifiableEntityOnly.YES }
        definition.parts is Query;

        annotation { "Name" : "Thickness" }
        isLength(definition.thickness, SHELL_OFFSET_BOUNDS);

        annotation { "Name" : "Rib extrusion direction" }
        definition.ribExtrusionDirection is RibExtrusionDirection;

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION", "Default" : true }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Extend profiles to part" }
        definition.extendProfilesUpToPart is boolean;

        annotation { "Name" : "Merge ribs", "Default" : true }
        definition.mergeRibs is boolean;
    }
    {
        const useRobustProfilesQ = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1076_TRANSIENT_QUERY);
        const profiles = (useRobustProfilesQ) ? makeRobustQueriesBatched(context, definition.profiles) : evaluateQuery(context, definition.profiles);
        const numberOfRibs = size(profiles);
        if (profiles == [])
        {
            throw regenError(ErrorStringEnum.RIB_NO_PROFILES, ["profiles"]);
        }

        if (evaluateQuery(context, definition.parts) == [])
        {
            throw regenError(ErrorStringEnum.RIB_NO_PARTS);
        }

        // Create a transform for making the feature patternable via feature pattern.
        var remainingTransform = getRemainderPatternTransform(context,
                {"references" : qUnion([definition.profiles, definition.parts])});

        const extendLength = calculateLength(context, qUnion([definition.parts, definition.profiles]));

        // List of noop booleans aka failed ribs
        var badSubtractions = [];

        // Create each rib (one rib per profile) as its own body.
        for (var i = 0; i < numberOfRibs; i += 1)
        {
            const profile = profiles[i];
            const instanceId = id + unstableIdComponent(i);
            var thickenId;

            // Cannot determine the direction of extrusion when profile is closed.
            if (definition.ribExtrusionDirection == RibExtrusionDirection.PARALLEL_TO_SKETCH_PLANE && isClosed(context, profile))
            {
                throw regenError(ErrorStringEnum.RIB_ONLY_OPEN_PROFILES);
            }

            try
            {
                var entitiesToExtrudeResult = createEntitiesToExtrude(context, instanceId, profile, remainingTransform, extendLength, definition);
                const entitiesToExtrude = entitiesToExtrudeResult.entitiesToExtrude;
                const profileEndTangentLines = entitiesToExtrudeResult.profileEndTangentLines;
                const extendProfiles = entitiesToExtrudeResult.extendProfiles;
                const extendedEndPoints = entitiesToExtrudeResult.extendedEndPoints;

                var extrudeResult = extrudeRibs(context, instanceId, profile, entitiesToExtrudeResult, remainingTransform, extendLength, true, definition);
                // Set ribDirection here so that extrudeRibs can reuse it for the next profile
                definition.ribDirection = extrudeResult.ribDirection;
                badSubtractions = concatenateArrays([badSubtractions, extrudeResult.badSubtractions]);
                thickenId = extrudeResult.thickenId;
            }
            catch
            {
                throw regenError(ErrorStringEnum.RIB_PROFILE_FAILED, profile);
            }

            // Fail early if the rib body can't be created.
            if (evaluateQuery(context, qCreatedBy(thickenId, EntityType.BODY)) == [])
            {
                throw regenError(ErrorStringEnum.RIB_BODY_FAILED, profile);
            }
        }

        if (size(badSubtractions) > 0)
        {
            setErrorEntities(context, id, { "entities" : qUnion(badSubtractions) });
            if (size(badSubtractions) == numberOfRibs)
            {
                throw regenError(ErrorStringEnum.RIB_NO_INTERSECTIONS);
            }
            opDeleteBodies(context, id + "deleteBadSubtractions", {
                    "entities" : qUnion(badSubtractions)
            });
        }
        // Optionally, merge the new ribs with the original parts.
        if (definition.mergeRibs)
        {
            var parameters;
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V432_RIB_GROUP_BOOLEANS))
            {
                parameters = {
                    "tools" : qCreatedBy(id, EntityType.BODY),
                    "targets" : definition.parts,
                    "targetsAndToolsNeedGrouping" : true,
                    "operationType" : BooleanOperationType.UNION
                };
            }
            else
            {
                // The original parts are first in the tools query so that they
                // will maintain their names.
                var toMerge = qUnion([definition.parts, qCreatedBy(id, EntityType.BODY)]);
                parameters =  {
                    "tools" : toMerge,
                    "operationType" : BooleanOperationType.UNION
                };
            }
            try
            {
                opBoolean(context, id + "mergeRibsWithParts", parameters);
            }
            catch
            {
                throw regenError(ErrorStringEnum.RIB_MERGE_FAILED);
            }
        }
    },
        {
            oppositeDirection : true,
            ribExtrusionDirection : RibExtrusionDirection.PARALLEL_TO_SKETCH_PLANE,
            extendProfilesUpToPart : false,
            mergeRibs : true
        });

function patternTransform(context, id, query, transform)
{
    if (transform == identityTransform())
        return;
    opTransform(context, id, {
            "bodies" : qOwnerBody(query),
            "transform" : transform
    });
}

/**
 * Before evaluating the profiles to create the ribs, we find out how big the parts are
 * so if any extending is necessary for any rib end, we know how far we need to extend.
 * To ensure the extended profile will always go past the part(s), we use the
 * diagonal of the bounding box of the part(s) and profile(s) as the extend length.
 */
function calculateLength(context is Context, parts is Query)
{
    const partBoundingBox = evBox3d(context, {
                "topology" : parts
            });
    return norm(partBoundingBox.maxCorner - partBoundingBox.minCorner);
}

function createEntitiesToExtrude(context is Context, id is Id, profile is Query, remainingTransform is Transform, extendLength is ValueWithUnits, definition is map) returns map
{
    // Keep track of the entities we will extrude as a surface which will later
    // be thickened to create the rib.  The profile and any
    // profile extensions will need to be included in the extrude operation.
    var entitiesToExtrude = [profile];

    // Get the endpoints of the profile and the normal direction at those endpoints
    // so we can determine what needs to be extended and what direction to extend.
    const profileEndTangentLines = evEdgeTangentLines(context, {
                "edge" : profile,
                "parameters" : [0, 1]
            });

    const extendDirections = [-profileEndTangentLines[0].direction, profileEndTangentLines[1].direction];

    // There  are 2 reasons we might need to extend the given profiles:
    // 1.  If the profile touches the part(s), make an extension of the profile past the part to ensure
    //     that there are no gaps when we thicken the profile (this can happen if the profile is not normal
    //     to the part where they intersect).
    // 2.  The extend profiles up to part checkbox has been selected.

    // return whether we need to force an extension at the given end of the profile.
    const endNeedsExtension = function(end is number) returns boolean
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V438_RIB_EXTEND_LOGIC))
        {
            // New test, test if point[end] is on one or more faces and not tangent to at least one of the face normals.
            var allFaces = qOwnedByBody(definition.parts, EntityType.FACE);
            var contactFaces = evaluateQuery(context, qContainsPoint(allFaces, remainingTransform * profileEndTangentLines[end].origin));
            for (var contactFace in contactFaces)
            {
                var distanceResult = evDistance(context, {
                        "side0" : profileEndTangentLines[end].origin,
                        "side1" : contactFace
                });
                var parameter = distanceResult.sides[1].parameter;
                var tangentPlane = evFaceTangentPlane(context, {
                        "face" : contactFace,
                        "parameter" : parameter
                });

                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V536_RIB_PROFILE_EXTN_CHECK_ANGLE))
                {
                    // Extend profile when the angle between extendDirection and face normal is within (90, 270) degrees.
                    if (dot(extendDirections[end], tangentPlane.normal) < -TOLERANCE.zeroAngle)
                    {
                        return true;
                    }
                }
                else if (!perpendicularVectors(extendDirections[end], tangentPlane.normal))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            // old method, test if the point is in or on one of the parts.
            return evaluateQuery(context, qContainsPoint(definition.parts, remainingTransform * profileEndTangentLines[end].origin)) != [];
        }
    };

    var extendProfiles = makeArray(2);
    var extendedEndPoints = makeArray(2);

    // If the profile is closed, then there is nothing to extend.
    const isProfileClosed = isClosed(context, profile);

    for (var end in [0, 1]) // Potentially extend both endpoints of the profile curve
    {
        extendProfiles[end] = !isProfileClosed && (definition.extendProfilesUpToPart || endNeedsExtension(end));
        if (extendProfiles[end])
        {
            extendedEndPoints[end] = profileEndTangentLines[end].origin + (extendDirections[end] * extendLength);
            // This is actually a quick way to create a line in 3D
            opFitSpline(context, id + ("extendProfile" ~ end), {
                        "points" : [
                                profileEndTangentLines[end].origin,
                                extendedEndPoints[end]
                            ]
                    });
            entitiesToExtrude = append(entitiesToExtrude, qCreatedBy(id + ("extendProfile" ~ end), EntityType.EDGE));
        }
    }
    return {
        "entitiesToExtrude" : entitiesToExtrude,
        "profileEndTangentLines" : profileEndTangentLines,
        "extendProfiles" : extendProfiles,
        "extendedEndPoints" : extendedEndPoints
    };
}

function extrudeRibs(context is Context,
    id is Id,
    profile is Query,
    entitiesToExtrudeResult is map,  // result of calling createEntitiesToExtrude
    remainingTransform is Transform, // Feature pattern transform
    extendLength is ValueWithUnits,  // Length to extrude profiles
    performBoolean is boolean,       // Whether or not to trim the extrusion to the parts
    definition is map) returns map
{
    const entitiesToExtrude = entitiesToExtrudeResult.entitiesToExtrude;
    const profileEndTangentLines = entitiesToExtrudeResult.profileEndTangentLines;
    const extendProfiles = entitiesToExtrudeResult.extendProfiles;
    const extendedEndPoints = entitiesToExtrudeResult.extendedEndPoints;

    // List of noop booleans aka failed ribs
    var badSubtractions = [];

    // Find the direction to extrude a surface that will later be thickened to produce the rib.
    // First determine the normal or parallel direction, then, if specified,
    // choose the opposite of the normal or parallel direction.
    var ribDirection;
    if (definition.ribDirection == undefined)
    {
        const profilePlane = evOwnerSketchPlane(context, { "entity" : profile });
        if (definition.ribExtrusionDirection == RibExtrusionDirection.PARALLEL_TO_SKETCH_PLANE)
        {
            // To get the parallel direction with the sketch plane, find the direction perpendicular
            // to the sketch plane normal and the line that connects the start and end point of the profile.
            const profileDirection = normalize(profileEndTangentLines[1].origin - profileEndTangentLines[0].origin);
            ribDirection = cross(profilePlane.normal, profileDirection);
        }
        else
        {
            ribDirection = profilePlane.normal;
        }

        if (definition.oppositeDirection)
        {
            ribDirection = ribDirection * -1;
        }
    }
    else
    {
        ribDirection = definition.ribDirection;
    }

    // Extrude a surface from the extended profile into the part(s), using the extend length
    // as the extrude depth to make sure the surface goes through the part(s).
    opExtrude(context, id + "surfaceExtrude", {
                "entities" : qUnion(entitiesToExtrude),
                "direction" : ribDirection,
                "endDepth" : extendLength,
                "endBound" : BoundingType.BLIND
            });

    // Transform the extruded surface if needed to support feature pattern.
    transformResultIfNecessary(context, id + "surfaceExtrude", remainingTransform);

    // Thicken the surface to make the rib plus some excess material around the part(s).
    const halfThickness = definition.thickness / 2;
    const thickenId = id + "thickenRib";
    opThicken(context, thickenId, {
                "entities" : qCreatedBy(id + "surfaceExtrude", EntityType.FACE),
                "thickness1" : halfThickness,
                "thickness2" : halfThickness
            });

    // Split the rib with the part(s) to separate the rib body from the thicken excess.
    var ribPartsQuery = qCreatedBy(thickenId, EntityType.BODY);
    var didBoolean = performBoolean;
    if (performBoolean)
    {
        opBoolean(context, id + "splitOffRibExcess", {
                    "tools" : definition.parts,
                    "targets" : ribPartsQuery,
                    "operationType" : BooleanOperationType.SUBTRACTION,
                    "keepTools" : true
                });
        var boolQuery = qCreatedBy(id + "splitOffRibExcess", EntityType.FACE);
        if (size(evaluateQuery(context, boolQuery)) == 0 || size(evaluateQuery(context, ribPartsQuery)) == 1)
        {
            badSubtractions = append(badSubtractions, ribPartsQuery);
            didBoolean = false;
        }
    }

    // Apply the remaining transform to the profile before doing collision testing.
    patternTransform(context, id + "tr1", profile, remainingTransform);
    // Do collision testing to help determine which parts of the thicken are excess.
    var clashes = evCollision(context, {
            "tools" : ribPartsQuery,
            "targets" : profile
        });

    // Since we don't want the profile to actually move
    // move it back to its original location after checking for collisions.
    patternTransform(context, id + "tr2", profile, inverse(remainingTransform));
    var clashBodies = mapArray(clashes, function(clash)
    {
        return clash.toolBody;
    });

    // Specify a point at the end of the surface extrude.
    // Any thicken body that intersects with this point is excess.
    const surfaceExtrudeEndPoint = profileEndTangentLines[0].origin + (extendLength * ribDirection);

    // Collect up all the thicken excess and any other entities we've created leading
    // up to the thicken operation, because all of these need to be deleted.
    var entitiesToDelete = [
        // Remove rib thicken excess sections that don't intersect the original profile.
        qSubtraction(ribPartsQuery, qUnion(clashBodies)),

        // Remove the surface extrude, now that the thicken is completed and we don't need it anymore.
        qCreatedBy(id + "surfaceExtrude", EntityType.BODY)
    ];

    if (didBoolean)
    {
        // Remove rib thicken excess sections that extend all the way to the end of
        // the surface extrude (which we deliberately had extend well past the part,
        // i.e. well past where a rib should be created).
        entitiesToDelete = append(entitiesToDelete, qContainsPoint(ribPartsQuery, remainingTransform * surfaceExtrudeEndPoint));
    }
    // Delete any profile extensions created now that we don't need them anymore.
    // Also, any thicken section that intersects with the far end of an extension
    // (i.e. not the end that intersects with the profile) is thicken excess and should be deleted.
    for (var end in [0, 1])
    {
        if (extendProfiles[end])
        {
            entitiesToDelete = append(entitiesToDelete, qCreatedBy(id + ("extendProfile" ~ end), EntityType.BODY));
            if (didBoolean)
            {
                entitiesToDelete = append(entitiesToDelete, qContainsPoint(ribPartsQuery, extendedEndPoints[end]));
            }
        }
    }

    opDeleteBodies(context, id + "deleteRibExcess", {
                "entities" : qUnion(entitiesToDelete)
                    });

    return { "badSubtractions" : badSubtractions, "thickenId" : thickenId, "ribDirection" : ribDirection };
}

function getBodyCollisions(context is Context, id is Id, solidBodiesQuery is Query, scopeSize is ValueWithUnits, definition is map) returns array
{
    // Create a transform for making the feature patternable via feature pattern.
    var remainingTransform = getRemainderPatternTransform(context,
            {"references" : qUnion([definition.profiles, solidBodiesQuery])});

    const profiles = evaluateQuery(context, definition.profiles);
    var targetHitIds = {};
    for (var i = 0; i < size(profiles); i += 1)
    {
        //Build a list of target queries, don't include targets already in the list
        var targetQuery = qSubtraction(solidBodiesQuery, qCreatedBy(id, EntityType.BODY));
        for (var entry in targetHitIds)
        {
            targetQuery = qSubtraction(targetQuery, entry.key);
        }

        // If there are no targets remaining, break
        if (size(evaluateQuery(context, targetQuery)) == 0)
        {
            break;
        }
        var profile = profiles[i];
        const instanceId = id + unstableIdComponent(i);
        var entitiesToExtrudeResult = createEntitiesToExtrude(context, instanceId, profile, remainingTransform, scopeSize, definition);
        const entitiesToExtrude = entitiesToExtrudeResult.entitiesToExtrude;
        const profileEndTangentLines = entitiesToExtrudeResult.profileEndTangentLines;
        const extendProfiles = entitiesToExtrudeResult.extendProfiles;
        const extendedEndPoints = entitiesToExtrudeResult.extendedEndPoints;

        const extendResult = extrudeRibs(context, instanceId, profile, entitiesToExtrudeResult, remainingTransform, scopeSize, false, definition);
        // Set ribDirection here so that extrudeRibs can reuse it for the next profile
        definition.ribDirection = extendResult.ribDirection;
        var thickenQuery = qCreatedBy(extendResult.thickenId, EntityType.BODY);
        try
        {
            var collisionResult = evCollision(context, {
                    "tools" : thickenQuery,
                    "targets" : targetQuery
            });
            var profileHitIds = {};
            for (var collision in collisionResult)
            {
                const clash is ClashType = collision['type'];
                if (clash == ClashType.INTERFERE ||
                    clash == ClashType.TARGET_IN_TOOL ||
                    clash == ClashType.TOOL_IN_TARGET)
                {
                    profileHitIds[collision.targetBody] = 1;
                }
            }
            if (size(profileHitIds) == 1)
            {
                targetHitIds = mergeMaps(targetHitIds, profileHitIds);
            }
        }
        try
        {
            opDeleteBodies(context, id + ("deleteBodies_" ~ i), {
                "entities" : thickenQuery
            });
        }
    }

    var partQueries = [];
    for (var entry in targetHitIds)
    {
        partQueries = append(partQueries, entry.key);
    }

    return partQueries;
}

/**
 * @internal
 * Edit logic function for rib
 */
export function ribEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
                              specifiedParameters is map, hiddenBodies is Query) returns map
{
    var partsAreSet = specifiedParameters.parts;
    var oppositeDirectionSet = specifiedParameters.oppositeDirection;
    var numParts = size(evaluateQuery(context, definition.parts));

    // If editing or the user changes any heuristic setting, no heuristics.
    if (partsAreSet || (oppositeDirectionSet && numParts != 0))
    {
        return definition;
    }
    var numProfiles = size(evaluateQuery(context, definition.profiles));

    if (numProfiles == 0)
    {
        definition.parts = qUnion([]);
        return definition;
    }

    var solidBodiesQuery is Query = qNothing();
    solidBodiesQuery = qAllModifiableSolidBodies();
    solidBodiesQuery = qSubtraction(solidBodiesQuery, hiddenBodies);

    var scopeSize = calculateLength(context, qUnion([solidBodiesQuery, definition.profiles]));

    var tempDefinition = definition;
    tempDefinition.extendProfilesUpToPart = false;
    if (!partsAreSet)
    {
        tempDefinition.parts = qUnion([]);
    }
    if (oppositeDirectionSet)
    {
        var hits = getBodyCollisions(context, id, solidBodiesQuery, scopeSize, tempDefinition);
        definition.parts = qUnion(hits);
    }
    else
    {
        tempDefinition.oppositeDirection = false;
        var positiveHits = getBodyCollisions(context, id + "positive", solidBodiesQuery, scopeSize, tempDefinition);

        tempDefinition.oppositeDirection = true;
        var negativeHits = getBodyCollisions(context, id + "negative", solidBodiesQuery, scopeSize, tempDefinition);

        var newParts = qUnion([]);
        if (size(positiveHits) > 0 && size(negativeHits) == 0)
        {
            newParts = qUnion(positiveHits);
            definition.oppositeDirection = false;
        }
        else if (size(negativeHits) > 0 && size(positiveHits) == 0)
        {
            newParts = qUnion(negativeHits);
            definition.oppositeDirection = true;
        }
        definition.parts = newParts;
    }

    return definition;
}

