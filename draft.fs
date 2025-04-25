FeatureScript 2641; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2641.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "2641.0");
import(path : "onshape/std/coordSystem.fs", version : "2641.0");
import(path : "onshape/std/curveGeometry.fs", version : "2641.0");
import(path : "onshape/std/drafttype.gen.fs", version : "2641.0");
import(path : "onshape/std/evaluate.fs", version : "2641.0");
import(path : "onshape/std/feature.fs", version : "2641.0");
import(path : "onshape/std/manipulator.fs", version : "2641.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2641.0");
import(path : "onshape/std/topologyUtils.fs", version : "2641.0");
import(path : "onshape/std/valueBounds.fs", version : "2641.0");
import(path : "onshape/std/vector.fs", version : "2641.0");

/**
 * Types of drafts available for the draft feature.
 * @value NEUTRAL_PLANE: Draft by holding the intersection between a set of faces and a neutral plane as a constant.
 * @value PARTING_LINE: Draft by holding a set of parting edges as a constant.
 */
export enum DraftFeatureType
{
    annotation { "Name" : "Neutral plane" }
    NEUTRAL_PLANE,
    annotation { "Name" : "Parting line" }
    PARTING_LINE
}

/**
 * Specifies which faces to draft when drafting with DraftFeatureType.PARTING_LINE.
 * @value ONE_SIDED: Draft one of the faces attached to the parting line.
 * @value SYMMETRIC: Draft both of the faces attached to the parting line symmetrically.
 * @value TWO_SIDED: Draft both of the faces attached to the parting line with separate draft angles.
 */
export enum PartingLineSides
{
    annotation { "Name" : "One sided" }
    ONE_SIDED,
    annotation { "Name" : "Symmetric" }
    SYMMETRIC,
    annotation { "Name" : "Two sided" }
    TWO_SIDED
}

// Steepness tolerance for `steepness` output of getOrderedFaceData.
const STEEPNESS_TOLERANCE = sin(TOLERANCE.zeroAngle * radian);

/**
 * Feature performing an [opDraft].
 *
 * @param id : @autocomplete `id + "draft1"`
 * @param definition {{
 *      @field draftFeatureType {DraftFeatureType}: @optional
 *              Specifies a `NEUTRAL_PLANE` or `PARTING_LINE` draft. Default is `NEUTRAL_PLANE`.
 *      @field neutralPlane {Query}: @requiredif { `draftFeatureType` is `NEUTRAL_PLANE` }
 *              A planar face or mate connector defining both the neutral plane and pull direction of the the draft.
 *              The intersection of the drafted faces and the neutral plane remains unchanged.
 *              The pull direction of the draft will be the face normal or mate connector z-axis.
 *              @autocomplete `neutralPlane`
 *      @field draftFaces {Query}: @requiredif { `draftFeatureType` is `NEUTRAL_PLANE` }
 *              The faces to draft for a `NEUTRAL_PLANE` draft.
 *              @autocomplete `draftFaces`
 *      @field pullDirectionEntity {Query}: @requiredif { `draftFeatureType` is `PARTING_LINE` }
 *              An entity defining the pull direction of the draft. This entity should conform to the `ALLOWS_DIRECTION`
 *              specification in [QueryFilterCompound].
 *      @field partingEdges {Query}: @requiredif { `draftFeatureType` is `PARTING_LINE` }
 *              Edges defining the parting line of the draft. These edges will remain unchanged as some adjacent faces,
 *              as defined by `partingLineSides`, are drafted.
 *      @field hintFaces {Query} : @optional
 *              For Onshape internal use. For `PARTING_LINE` draft, specifies in advance which adjacent faces of
 *              `partingEdges` should be treated as more along the pull direction. When unspecified, the feature will
 *              use a geometric calculation to determine this distinction.
 *      @field partingLineSides {PartingLineSides}: @requiredif { `draftFeatureType` is `PARTING_LINE` }
 *              Specifies whether to draft one or both faces adjacent to the parting edges, and whether the draft should
 *              be symmetrical if drafting both faces.  See [PartingLineSides].
 *      @field alongPull {boolean}: @requiredif { `PartingLineSides` is `ONE_SIDED` }
 *              Specifies which face will be drafted in a `ONE_SIDED` `PARTING_LINE` draft.  If `true`, the face which
 *              is more along the pull direction from the perspective of the parting edge is drafted.  If `false`, the
 *              face which is more away from the pull direction from the perspective of the parting edge is drafted.
 *
 *      @field angle {ValueWithUnits}:
 *              The draft angle, must be between 0 and 89.9 degrees.
 *              @eg `3 * degree`
 *      @field pullDirection {boolean}: @optional
 *              Whether the pull direction of the draft should be reversed.  In equivalent terms, whether the draft
 *              should use `-angle` as the draft angle (`true`), or `angle` as the draft angle (`false`).
 *              Default is `false`.
 *      @field secondAngle {ValueWithUnits}: @requiredif { `PartingLineSides` is `TWO_SIDED` }
 *              The second draft angle for a `TWO_SIDED` `PARTING_LINE` draft. Must be between 0 and 89.9 degrees.
 *              Note that when using `TWO_SIDED`, `angle` will be applied to faces that are away from the pull direction,
 *              from the perspective of the edge.  `secondAngle` will be applied to faces that are along the pull direction,
 *              from the perspective of the edge.
 *              @ex `3 * degree`
 *      @field secondPullDirection {boolean}: @optional
 *              Whether the pull direction of the draft should be reversed for the second angle of a `TWO_SIDED` draft.
 *              In equivalent terms, whether the draft should use `-secondAngle` as the second draft angle (`true`), or
 *              `secondAngle` as the second draft angle (`false`).
 *              Default is `false`.
 *
 *      @field tangentPropagation {boolean}: @optional
 *              For a `NEUTRAL_PLANE` draft, `true` to propagate draft across tangent faces.
 *              Default is `false`.
 *      @field referenceEdgePropagation {boolean}: @optional
 *              For a `PARTING_LINE` draft, `true` to extend the parting line across connected edges of already included
 *              faces and tangent faces.
 *              Default is `false`.
 *
 *      @field reFillet {boolean}: @optional
 *              `true` to attempt to defillet draft faces before the draft and reapply the fillets after.
 *               Default is `false`.
 * }}
 */
annotation { "Feature Type Name" : "Draft",
        "Manipulator Change Function" : "draftManipulatorChange",
        "Editing Logic Function" : "draftEditLogic",
        "Filter Selector" : "allparts" }
export const draft = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Draft type", "UIHint" : UIHint.HORIZONTAL_ENUM }
        definition.draftFeatureType is DraftFeatureType;

        if (definition.draftFeatureType == DraftFeatureType.NEUTRAL_PLANE)
        {
            annotation { "Name" : "Neutral plane",
                        "Filter" : QueryFilterCompound.ALLOWS_PLANE,
                        "MaxNumberOfPicks" : 1 }
            definition.neutralPlane is Query;

            annotation { "Name" : "Entities to draft", "UIHint" : UIHint.INITIAL_FOCUS_ON_EDIT,
                        "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES }
            definition.draftFaces is Query;
        }
        else if (definition.draftFeatureType == DraftFeatureType.PARTING_LINE)
        {
            annotation { "Name" : "Pull direction",
                        "Filter" : QueryFilterCompound.ALLOWS_DIRECTION,
                        "MaxNumberOfPicks" : 1 }
            definition.pullDirectionEntity is Query;

            annotation { "Name" : "Parting edges", "UIHint" : [UIHint.SHOW_CREATE_SELECTION, UIHint.INITIAL_FOCUS_ON_EDIT],
                        "Filter" : EntityType.EDGE && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO && ModifiableEntityOnly.YES }
            definition.partingEdges is Query;

            annotation { "UIHint" : UIHint.ALWAYS_HIDDEN }
            definition.hintFaces is Query;

            annotation { "Name" : "Sides" }
            definition.partingLineSides is PartingLineSides;

            if (definition.partingLineSides == PartingLineSides.ONE_SIDED)
            {
                annotation { "Name" : "Switch face", "UIHint" : UIHint.OPPOSITE_DIRECTION, "Default" : true }
                definition.alongPull is boolean;
            }
        }

        annotation { "Name" : "Draft angle",  "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        isAngle(definition.angle, ANGLE_STRICT_90_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION_CIRCULAR, "Default" : false }
        definition.pullDirection is boolean;

        if (definition.draftFeatureType == DraftFeatureType.PARTING_LINE && definition.partingLineSides == PartingLineSides.TWO_SIDED)
        {
            annotation { "Name" : "Second draft angle", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
            isAngle(definition.secondAngle, ANGLE_STRICT_90_BOUNDS);

            annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION_CIRCULAR, "Default" : true }
            definition.secondPullDirection is boolean;
        }

        if (definition.draftFeatureType == DraftFeatureType.NEUTRAL_PLANE)
        {
            annotation { "Name" : "Tangent propagation", "Default" : true }
            definition.tangentPropagation is boolean;
        }
        else if (definition.draftFeatureType == DraftFeatureType.PARTING_LINE)
        {
            annotation { "Name" : "Parting line propagation", "Default" : true }
            definition.referenceEdgePropagation is boolean;
        }

        annotation { "Name" : "Reapply fillet", "Default" : false }
        definition.reFillet is boolean;
    }
    {
        if (definition.draftFeatureType == DraftFeatureType.NEUTRAL_PLANE)
        {
            verifyNoMesh(context, definition, "neutralPlane");
            verifyNoMesh(context, definition, "draftFaces");
        }
        else if (definition.draftFeatureType == DraftFeatureType.PARTING_LINE)
        {
            verifyNoMesh(context, definition, "pullDirectionEntity");
            verifyNoMesh(context, definition, "partingEdges");
        }
        definition = switch (definition.draftFeatureType)
        {
            DraftFeatureType.NEUTRAL_PLANE : initReferenceSurfaceDraft(context, id, definition),
            DraftFeatureType.PARTING_LINE : initReferenceEntityDraft(context, id, definition)
        };
        opDraft(context, id, definition);
    },
    {
        draftFeatureType : DraftFeatureType.NEUTRAL_PLANE,
        pullDirection : false, secondPullDirection : false,
        tangentPropagation : false, referenceEdgePropagation : false,
        reFillet : false, hintFaces : qNothing()
    });


function initReferenceSurfaceDraft(context is Context, id is Id, definition is map) returns map
{
    definition.draftType = DraftType.REFERENCE_SURFACE;
    const cSys = try silent(evMateConnector(context, { "mateConnector" : definition.neutralPlane }));
    if (cSys != undefined)
    {
        definition = initDraftFromMateConnector(context, id, definition, cSys);
    }
    else
    {
        definition = initDraftFromFaceQuery(context, id, definition);
    }
    definition.pullVec = getPullVec(definition, -definition.rawNeutralPlane.normal);
    addNeutralPlaneDraftAngularManipulator(context, id, definition, definition.rawNeutralPlane);
    return definition;
}

function getPullVec(definition is map, trueVector is Vector) returns Vector
{
    return (definition.pullDirection) ? trueVector : -trueVector;
}

function initDraftFromFaceQuery(context is Context, id is Id, definition is map) returns map
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2171_DRAFT_REFERENCE_SURFACE))
    {
        definition.referenceSurface = definition.neutralPlane;
    }
    else
    {
        definition.referenceFace = definition.neutralPlane;
    }

    //rawNeutralPlane is only used to find pullVec and to locate draft manipulators
    const rawNeutralPlane = try(evFaceTangentPlane(context, {
                    "face" : definition.neutralPlane,
                    "parameter" : vector(0.5, 0.5)
                }));
    if (rawNeutralPlane == undefined)
    {
        throw regenError(ErrorStringEnum.DRAFT_SELECT_NEUTRAL, ["neutralPlane"]);
    }
    definition.rawNeutralPlane = rawNeutralPlane;
    return definition;
}

function initDraftFromMateConnector(context is Context, id is Id, definition is map, cSys is CoordSystem) returns map
{
    definition.rawNeutralPlane = plane(cSys);
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2171_DRAFT_REFERENCE_SURFACE))
    {
        definition.referenceSurface = definition.rawNeutralPlane;
    }
    else
    {
        definition.referencePlane = definition.rawNeutralPlane;
    }

    return definition;
}

function initReferenceEntityDraft(context is Context, id is Id, definition is map) returns map
{
    definition.draftType = DraftType.REFERENCE_ENTITY;
    const rawPullDirection = getPullDirection(context, definition);
    definition.pullVec = getPullVec(definition, -rawPullDirection);
    var edgeToOrderedFaceData = getEdgeToOrderedFaceData(context, definition.partingEdges, rawPullDirection, definition.hintFaces, true);
    // Add draft manipulators before getting reference entity draft options.
    // `getReferenceEntityDraftOptions` may fail on cases where we still want to show manipulators.
    addReferenceEntityDraftManipulators(context, id, rawPullDirection, edgeToOrderedFaceData, definition);
    definition.referenceEntityDraftOptions = getReferenceEntityDraftOptions(context, id, rawPullDirection, edgeToOrderedFaceData, definition);
    return definition;
}

function getPullDirection(context is Context, definition is map) returns Vector
{
    const directionResult = extractDirection(context, definition.pullDirectionEntity);
    if (directionResult == undefined)
    {
        throw regenError(ErrorStringEnum.DRAFT_SELECT_PULL_DIRECTION_ENTITY, ["pullDirectionEntity"]);
    }
    return directionResult;
}

////////// Parting Line Utilities //////////

/**
 * Assemble referenceEntityDraftOptions map to pass into [opDraft].
 */
function getReferenceEntityDraftOptions(context is Context, topLevelId is Id, rawPullDirection is Vector, edgeToOrderedFaceData is map,
    definition is map) returns array
{
    const partingEdges = verifyNonemptyQuery(context, definition, "partingEdges", ErrorStringEnum.DRAFT_SELECT_PARTING_EDGES);

    var draftOptions = [];
    if (definition.partingLineSides == PartingLineSides.ONE_SIDED)
    {
        // The first item in the faceData array corresponds to the face that is more along the pull direction.
        const faceIndex = definition.alongPull ? 0 : 1;

        for (var edge in partingEdges)
        {
            const faceData = edgeToOrderedFaceData[edge][faceIndex];
            if (abs(faceData.steepness) < STEEPNESS_TOLERANCE)
            {
                // Face normal is parallel to pull direction.
                throw regenError(ErrorStringEnum.DRAFT_FAILED, qUnion([edge, faceData.face]));
            }

            draftOptions = append(draftOptions, {
                        "face" : edgeToOrderedFaceData[edge][faceIndex].face,
                        "references" : edge
                    });
        }
    }
    else // SYMMETRIC or TWO_SIDED
    {
        const angles = getAlongAndAwayDraftAngles(definition);

        for (var edge in partingEdges)
        {
            // Draft both of the faces in the orderedFaceData for this edge
            for (var faceData in edgeToOrderedFaceData[edge])
            {
                if (abs(faceData.steepness) < STEEPNESS_TOLERANCE)
                {
                    // Face normal is parallel to pull direction.
                    throw regenError(ErrorStringEnum.DRAFT_FAILED, qUnion([edge, faceData.face]));
                }

                draftOptions = append(draftOptions, {
                            "face" : faceData.face,
                            "references" : edge,
                            "angle" : faceData.isAlong ? angles.along : angles.away
                        });
            }
        }
    }

    return draftOptions;
}

function getAlongAndAwayDraftAngles(definition is map) returns map
{
    // Faces that are away always get the first angle
    var angles = { "away" : definition.angle };

    if (definition.partingLineSides == PartingLineSides.SYMMETRIC)
    {
        // Create symmetry with opposite angle
        angles.along = -1 * definition.angle;
    }
    else if (definition.partingLineSides == PartingLineSides.TWO_SIDED)
    {
        // Switch the sign of the second angle if the flippers don't match
        const sign = (definition.pullDirection == definition.secondPullDirection) ? 1 : -1;
        angles.along = sign * definition.secondAngle;
    }
    else
    {
        // Along and away get the same angle
        angles.along = definition.angle;
    }

    return angles;
}

/**
 * Build a mapping from each edge in `partingEdges` to an array of ordered face data for that edge.  See `getOrderedFaceData(...)`
 * for information about the ordering and contents of ordered face data.
 */
function getEdgeToOrderedFaceData(context is Context, partingEdges is Query, rawPullDirection is Vector,
    hintFaces is Query, failOnError is boolean) returns map
{
    var edgeToOrderedFaceData = {};
    var moreAlongFirst = new box(undefined);
    const stable = isAtVersionOrLater(context, FeatureScriptVersionNumber.V847_PL_DRAFT_STABLE);
    for (var edge in evaluateQuery(context, partingEdges))
    {
        if (!stable)
            moreAlongFirst[] = undefined;

        try
        {
            edgeToOrderedFaceData[edge] = getOrderedFaceData(context, edge, rawPullDirection, moreAlongFirst, hintFaces,
                    stable ? edgeToOrderedFaceData : {});
        }
        catch (e)
        {
            if (failOnError)
            {
                var errEnum = ErrorStringEnum.DRAFT_FAILED;
                if (e is ErrorStringEnum)
                    errEnum = e;

                throw regenError(errEnum, ["partingEdges"], edge);
            }
            else
            {
                edgeToOrderedFaceData[edge] = undefined;
            }
        }
    }

    return edgeToOrderedFaceData;
}

/**
 * Each adjacent face to `edge` can be along, orthogonal, or away from the draft direction from the perspective of the
 * edge in question. Return an array of information for both adjacent faces. Order the faces by the boolean contained in
 * the `moreAlongFirst` box (If true, the face that is more along the pull direction will be first, if false, the face
 * that is more away from the pull direction will be first).  If the `moreAlongFirst` box contains `undefined`, try to
 * order the faces by matching exactly one of them to the hint faces; return this matched face first and fill the
 * `moreAlongFirst` box based on whether the matched face is more along or more away from the pull direction.  If no
 * hint face can be matched, simply order with the more along face first, and fill `moreAlongFirst` with true`. In cases
 * of ambiguity (where neither face is more along or more away from the pull direction), check if an ordering can be
 * determined from a neighboring draft using `edgeToOrderedFaceData`; fall back on trying to match exactly on of the
 * faces with the hint faces, returning the matched face first.  Accompany this data with:
 *     `isAlong`: whether the face is along the pull direction from the perspective of the edge.
 *     `steepness`: a measure of the steepness of the face between -1 and 1.  -1 means the face is fully away from
 *                  the pull direction from the perspective of the edge. 0 means the normal of this face's plane
 *                  aligns with the pull direction.  1 means the face is fully along the the pull direction from
 *                  the perspective of the edge. Decimal values indicate that the face has some sort of lean in the
 *                  positive or negative direction.
 *
 *                  Example: imagine rectangular planar face aligned canonically with the front plane.  With a pull
 *                  direction of "up", the face has a steepness of -1 from the perspective of its top edge and a
 *                  steepness of 1 from the perspective of its bottom edge.  If instead we have a pull direction of
 *                  "forward", the face has a steepness of 0 from the perspective of all its edges.
 *
 * In cases where the edge is a laminar edge of a surface, return an array of two identical elements both representing
 * the face of the surface.  In cases where there is no matching hintFace and the two faces have equal steepness, throw
 * an error.
 */
function getOrderedFaceData(context is Context, edge is Query, rawPullDirection is Vector, moreAlongFirst is box,
    hintFaces is Query, edgeToOrderedFaceData is map) returns array
{
    const bothFaces = evaluateQuery(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE));
    if (size(bothFaces) == 0 || size(bothFaces) > 2)
    {
        // Should never be hit by UI user
        throw ("Unsupported edge with " ~ size(bothFaces) ~ " faces");
    }

    var faceData = [];
    for (var face in bothFaces)
    {
        const faceInfo = getFaceTangentPlaneAndCoEdgeLineAtParameterOfEdge(context, face, edge, 0.5);
        const faceTangentPlane = faceInfo.faceTangentPlane;
        const coEdgeLine = faceInfo.coEdgeLine;

        if (parallelVectors(coEdgeLine.direction, rawPullDirection))
        {
            throw ErrorStringEnum.DRAFT_PARALLEL_PARTING_EDGE;
        }

        // if the coEdge direction cross the pull direction points out of the face, then the face is along the pull
        // direction (from the perspective of the edge).  Otherwise it is away from the pull direction.
        const steepness = dot(faceTangentPlane.normal, normalize(cross(coEdgeLine.direction, rawPullDirection)));

        faceData = append(faceData, {
                    "face" : face,
                    "isAlong" : steepness > 0,
                    "steepness" : steepness
                });
    }

    // Laminar edge of a surface returns two copies of identical data.
    if (size(faceData) == 1)
    {
        return [faceData[0], faceData[0]];
    }

    const unambiguous = abs(faceData[0].steepness - faceData[1].steepness) > STEEPNESS_TOLERANCE;
    if (unambiguous)
    {
        const face0MoreAlong = faceData[0].steepness > faceData[1].steepness;

        if (moreAlongFirst[] == undefined)
        {
            // Default to ordering with the more along face first.
            moreAlongFirst[] = true;

            // Override the default by putting the hint face first, if it exists.
            const matchingFace = getFaceMatchingHintFaces(context, qUnion(bothFaces), hintFaces);
            if (matchingFace != undefined)
            {
                const face0IsHintFace = (bothFaces[0] == matchingFace);
                moreAlongFirst[] = (face0MoreAlong == face0IsHintFace);
            }
        }

        return (moreAlongFirst[] == face0MoreAlong) ? faceData : [faceData[1], faceData[0]];
    }
    else
    {
        // First, try to order by neighboring drafts
        const adjacentEdges = qAdjacent(edge, AdjacencyType.VERTEX, EntityType.EDGE);
        const processedEdges = qUnion(keys(edgeToOrderedFaceData));
        const processedAdjacentEdges = evaluateQuery(context, qIntersection([adjacentEdges, processedEdges]));
        if (size(processedAdjacentEdges) > 0)
        {
            const face0AndAdjacent = qSubtraction(qUnion([faceData[0].face, qAdjacent(faceData[0].face, AdjacencyType.EDGE, EntityType.FACE)]), faceData[1].face);
            const face1AndAdjacent = qSubtraction(qUnion([faceData[1].face, qAdjacent(faceData[1].face, AdjacencyType.EDGE, EntityType.FACE)]), faceData[0].face);

            var matchedFace = -1;
            for (var processedAdjacentEdge in processedAdjacentEdges)
            {
                const neighborFirstFace = edgeToOrderedFaceData[processedAdjacentEdge][0].face;

                const face0Matches = !isQueryEmpty(context, qIntersection([neighborFirstFace, face0AndAdjacent]));
                const face1Matches = !isQueryEmpty(context, qIntersection([neighborFirstFace, face1AndAdjacent]));

                if (face0Matches == face1Matches)
                {
                    // Neither or both sides match the neighbor.  Ambiguous.
                    matchedFace = -1;
                    break;
                }

                const currentMatch = face0Matches ? 0 : 1;
                if (matchedFace != -1 && matchedFace != currentMatch)
                {
                    // Current match conflicts with previously found match.  Ambiguous.
                    matchedFace = -1;
                    break;
                }
                matchedFace = currentMatch;
            }

            if (matchedFace != -1)
            {
                return [faceData[matchedFace], faceData[1 - matchedFace]];
            }
        }

        // Fall back on ordering by hint face
        const matchingFace = getFaceMatchingHintFaces(context, qUnion(bothFaces), hintFaces);
        if (matchingFace != undefined)
        {
            return (matchingFace == faceData[0].face) ? faceData : [faceData[1], faceData[0]];
        }

        throw "Ambiguous edge with no neighbors or hint face";
    }
}

function getFaceMatchingHintFaces(context is Context, faces is Query, hintFaces is Query)
{
    const matchedFaces = evaluateQuery(context, qIntersection([faces, hintFaces]));
    if (size(matchedFaces) == 1)
    {
        return matchedFaces[0];
    }
    return undefined;
}

function getFaceTangentPlaneAndCoEdgeLineAtParameterOfEdge(context is Context, face is Query, edge is Query, parameter is number) returns map
{
    try
    {
        const faceTangentPlane = evFaceTangentPlaneAtEdge(context, {
                    "face" : face,
                    "edge" : edge,
                    "parameter" : parameter,
                    "usingFaceOrientation" : true
                });

        const coEdgeLine = evEdgeTangentLine(context, {
                    "edge" : edge,
                    "parameter" : parameter,
                    "face" : face
                });

        return {
                "faceTangentPlane" : faceTangentPlane,
                "coEdgeLine" : coEdgeLine
            };
    }
    catch
    {
        throw regenError(ErrorStringEnum.DRAFT_FAILED);
    }
}

/**
 * Augment an ordered face data array with coEdge and tangent plane information for quarter-way and halfway along the edge
 */
function augmentOrderedFaceDataWithGeometryData(context is Context, edge is Query, orderedFaceData is array) returns array
{
    orderedFaceData[0] = mergeMaps(orderedFaceData[0], {
                "quarterGeometry" : getFaceTangentPlaneAndCoEdgeLineAtParameterOfEdge(context, orderedFaceData[0].face, edge, 0.25),
                "halfGeometry" : getFaceTangentPlaneAndCoEdgeLineAtParameterOfEdge(context, orderedFaceData[0].face, edge, 0.5)
            });

    // Use .75 for other face so that the coEdgeLine origin matches up.
    orderedFaceData[1] = mergeMaps(orderedFaceData[1], {
                "quarterGeometry" : getFaceTangentPlaneAndCoEdgeLineAtParameterOfEdge(context, orderedFaceData[1].face, edge, 0.75),
                "halfGeometry" : getFaceTangentPlaneAndCoEdgeLineAtParameterOfEdge(context, orderedFaceData[1].face, edge, 0.5)
            });

    return orderedFaceData;
}

/**
 * Return a map with "manipulatorFaceData" as the faceData map (augmented with geometry data) for the face that
 * will receive a manipulator and "otherFaceData" as the faceData map (augmented with geometry data) for the
 * other face attached to the edge with the manipulator. "otherFaceData" may also receive a manipulator for
 * `TWO_SIDED` draft, depending on the circumstances.
 *
 * `addGeometryData` may be turned off for a performance gain if geometry data is not needed.
 */
function getDataForManipulator(context is Context, rawPullDirection is Vector, edgeToOrderedFaceData is map,
    definition is map, addGeometryData is boolean) returns map
{
    const edges = evaluateQuery(context, definition.partingEdges);
    const manipulatorEdge = edges[size(edges) - 1];
    var lastOrderedFaceData = edgeToOrderedFaceData[manipulatorEdge];
    if (lastOrderedFaceData == undefined)
        throw "Could not find ordered face data for manipulator edge";

    if (addGeometryData)
    {
        // adds .quarterGeometry and .halfGeometry to each of the faceData maps. This data is used in addDraftManipulators(...)
        lastOrderedFaceData = augmentOrderedFaceDataWithGeometryData(context, manipulatorEdge, lastOrderedFaceData);
    }

    var index;
    if (definition.partingLineSides == PartingLineSides.ONE_SIDED)
    {
        index = definition.alongPull ? 0 : 1;
    }
    else
    {
        // Put the manipulator on the more away face
        if (lastOrderedFaceData[1].steepness < lastOrderedFaceData[0].steepness ||
            abs(lastOrderedFaceData[1].steepness - lastOrderedFaceData[0].steepness) < STEEPNESS_TOLERANCE)
        {
            index = 1;
        }
        else
        {
            index = 0;
        }
    }

    return {
            "manipulatorEdge" : manipulatorEdge,
            "manipulatorFaceData" : lastOrderedFaceData[index],
            "otherFaceData" : lastOrderedFaceData[1 - index]
        };
}

////////// Manipulators //////////

const ANGLE_MANIPULATOR = "angleManipulator";
const SYMMETRIC_ALONG_ANGLE_MANIPULATOR = "symmetricAlongAngleManipulator";
const SECOND_ANGLE_MANIPULATOR = "secondAngleManipulator";
const FLIP_MANIPULATOR = "flipManipulator";

function addReferenceEntityDraftManipulators(context is Context, topLevelId is Id, rawPullDirection is Vector,
    edgeToOrderedFaceData is map, definition is map)
{
    try silent
    {
        const manipulatorData = getDataForManipulator(context, rawPullDirection, edgeToOrderedFaceData, definition, true);
        const manipulatorFaceData = manipulatorData.manipulatorFaceData;
        const otherFaceData = manipulatorData.otherFaceData;

        // Flip manipulator dictates which face we are drafting.
        if (definition.partingLineSides == PartingLineSides.ONE_SIDED && edgeIsTwoSided(context, manipulatorData.manipulatorEdge))
        {
            addPartingLineDraftFlipManipulator(context, topLevelId, manipulatorFaceData, rawPullDirection);
        }

        if (definition.partingLineSides == PartingLineSides.ONE_SIDED || definition.partingLineSides == PartingLineSides.SYMMETRIC)
        {
            const angles = getAlongAndAwayDraftAngles(definition);
            const angle = manipulatorFaceData.isAlong ? angles.along : angles.away;

            var manipulatorName = ANGLE_MANIPULATOR;
            if (definition.partingLineSides == PartingLineSides.SYMMETRIC && manipulatorFaceData.isAlong)
            {
                manipulatorName = SYMMETRIC_ALONG_ANGLE_MANIPULATOR;
            }

            addPartingLineDraftAngularManipulator(context, topLevelId, manipulatorName, manipulatorFaceData, rawPullDirection,
                    angle, definition.pullDirection, ManipulatorStyleEnum.DEFAULT, "angle");
        }
        else if (definition.partingLineSides == PartingLineSides.TWO_SIDED)
        {
            // Add a manipulator on the designated face
            const firstParams = getTwoSidedManipulatorParameters(manipulatorFaceData.isAlong, definition);
            const added = addPartingLineDraftAngularManipulator(context, topLevelId, firstParams.manipulatorName, manipulatorFaceData,
                    rawPullDirection, firstParams.angle, firstParams.flipped, firstParams.style, firstParams.parameterId);

            // Add an additional manipulator if possible
            if (!added || manipulatorFaceData.isAlong != otherFaceData.isAlong)
            {
                // Ideally we would like to add both manipulators, but if both faces are along or both faces are
                // away, the manipulator will be redundant.  If for some reason we failed to add the first
                // manipulator, we are also safe adding this one.
                const secondParams = getTwoSidedManipulatorParameters(otherFaceData.isAlong, definition);
                addPartingLineDraftAngularManipulator(context, topLevelId, secondParams.manipulatorName, otherFaceData,
                        rawPullDirection, secondParams.angle, secondParams.flipped, secondParams.style, secondParams.parameterId);
            }
        }
    }
}

function addNeutralPlaneDraftAngularManipulator(context is Context, topLevelId is Id, definition is map, neutralPlane is Plane)
{
    try silent
    {
        const firstFacePlane = evFaceTangentPlane(context, {
                    "face" : qNthElement(definition.draftFaces, 0),
                    "parameter" : vector(0.5, 0.5)
                });

        const manipulator = angularManipulator({
                    "axisOrigin" : project(neutralPlane, firstFacePlane.origin),
                    "axisDirection" : normalize(cross(firstFacePlane.normal, neutralPlane.normal)),
                    "rotationOrigin" : firstFacePlane.origin,
                    "angle" : definition.pullDirection ? -definition.angle : definition.angle,
                    "minValue" : -ANGLE_STRICT_90_BOUNDS[degree][2] * degree,
                    "maxValue" : ANGLE_STRICT_90_BOUNDS[degree][2] * degree,
                    "primaryParameterId" : "angle"
                });
        addManipulators(context, topLevelId, { (ANGLE_MANIPULATOR) : manipulator });
    }
}

function addPartingLineDraftFlipManipulator(context is Context, topLevelId is Id, faceData is map, rawPullDirection is Vector)
{
    // Flip manipulator will point directly into the face from the perspective of its anchor point on the edge. Use
    // `quarterGeometry` to put the flip manipulator off to the side so that it does not overlap with the angular manipulator.
    const manipulatorOrigin = faceData.quarterGeometry.coEdgeLine.origin;
    const manipulatorDirection = cross(faceData.quarterGeometry.faceTangentPlane.normal, faceData.quarterGeometry.coEdgeLine.direction);

    const manipulator = flipManipulator({
                "base" : manipulatorOrigin,
                "direction" : manipulatorDirection,
                "flipped" : false
            });
    addManipulators(context, topLevelId, { (FLIP_MANIPULATOR) : manipulator });
}

function addPartingLineDraftAngularManipulator(context is Context, topLevelId is Id, manipulatorName is string, faceData is map,
    rawPullDirection is Vector, angle is ValueWithUnits, flipped is boolean, style is ManipulatorStyleEnum, parameterId is string) returns boolean
{
    try silent
    {
        if (abs(faceData.steepness) > STEEPNESS_TOLERANCE)
        {
            const axisOrigin = faceData.halfGeometry.coEdgeLine.origin;
            const manipulator = angularManipulator({
                        "axisOrigin" : axisOrigin,
                        "axisDirection" : normalize(cross(faceData.halfGeometry.faceTangentPlane.normal, rawPullDirection)),
                        "rotationOrigin" : axisOrigin + ((faceData.isAlong ? 1 : -1) * inch * rawPullDirection),
                        "angle" : flipped ? -angle : angle,
                        "minValue" : -ANGLE_STRICT_90_BOUNDS[degree][2] * degree,
                        "maxValue" : ANGLE_STRICT_90_BOUNDS[degree][2] * degree,
                        "style" : style,
                        "primaryParameterId" : parameterId
                    });
            addManipulators(context, topLevelId, { (manipulatorName) : manipulator });
            return true;
        }
    }
    return false;
}

function getTwoSidedManipulatorParameters(isAlong is boolean, definition is map) returns map
{
    // See getReferenceEntityDraftOptions(...):  "away" corresponds to first angle.  "along" corresponds to second angle.
    return {
            "manipulatorName" : isAlong ? SECOND_ANGLE_MANIPULATOR : ANGLE_MANIPULATOR,
            "angle" : isAlong ? definition.secondAngle : definition.angle,
            "flipped" : isAlong ? definition.secondPullDirection : definition.pullDirection,
            "style" : isAlong ? ManipulatorStyleEnum.SECONDARY : ManipulatorStyleEnum.DEFAULT,
            "parameterId" : isAlong ? "secondAngle" : "angle"
        };
}

/**
 * @internal
 * The manipulator change function for [draft].
 */
export function draftManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    if (newManipulators[ANGLE_MANIPULATOR] is map)
    {
        const newAngle = newManipulators[ANGLE_MANIPULATOR].angle;
        definition.pullDirection = newAngle < 0 * degree;
        definition.angle = abs(newAngle);
    }

    if (newManipulators[SYMMETRIC_ALONG_ANGLE_MANIPULATOR] is map)
    {
        const newAngle = -1.0 * newManipulators[SYMMETRIC_ALONG_ANGLE_MANIPULATOR].angle;
        definition.pullDirection = newAngle < 0 * degree;
        definition.angle = abs(newAngle);
    }

    if (newManipulators[SECOND_ANGLE_MANIPULATOR] is map)
    {
        const newAngle = newManipulators[SECOND_ANGLE_MANIPULATOR].angle;
        definition.secondPullDirection = newAngle < 0 * degree;
        definition.secondAngle = abs(newAngle);
    }

    if (newManipulators[FLIP_MANIPULATOR] is map && definition.draftFeatureType == DraftFeatureType.PARTING_LINE)
    {
        if (newManipulators[FLIP_MANIPULATOR].flipped)
        {
            definition.alongPull = !definition.alongPull;
        }
    }

    return definition;
}

////////// Editing Logic //////////

/**
 * @internal
 * The editing logic function for [draft].
 */
export function draftEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    isCreating is boolean, specifiedParameters is map) returns map
{
    if (definition.draftFeatureType == DraftFeatureType.PARTING_LINE)
    {
        var edgeToOrderedFaceDataBox = new box(undefined);

        if (canGenerateHintFaces(context, oldDefinition, definition))
        {
            definition.hintFaces = generateHintFaces(context, edgeToOrderedFaceDataBox, definition);
        }

        if (canFlipAlongPull(context, oldDefinition, definition, specifiedParameters))
        {
            definition = flipAlongPull(context, edgeToOrderedFaceDataBox, definition);
        }

        if (canAdjustDirectionsForSideChange(oldDefinition, definition, specifiedParameters))
        {
            definition = adjustDirectionsForSideChange(context, edgeToOrderedFaceDataBox, oldDefinition, definition);
        }
    }

    return definition;
}

/*
 * Parting line draft: determine, in advance, which face should be treated as more along the pull direction (or choose one
 * deterministically) and store that information in the feature definition to make feature regeneration more robust.
 * This storage of information attempts to ensure that upstream changes do not change which face is being drafted.
 * Additionally, this storage imposes a deterministic ordering on faces which cannot be ordered by steepness (and
 * ensures that that ordering is robust to upstream changes).
 */
predicate canGenerateHintFaces(context is Context, oldDefinition is map, definition is map)
{
    // If user selects parting edges before pull direction entity, hint faces may be empty.  Make sure to generate hint
    // faces when we go from no pull direction entity to some pull direction entity. `try silent` for old definition
    // because oldDefinition.pullDirectionEntity is undefined when first creating the feature.
    oldDefinition.partingEdges != definition.partingEdges ||
        (try silent(isQueryEmpty(context, oldDefinition.pullDirectionEntity)) && size(evaluateQuery(context, definition.pullDirectionEntity)) == 1);
}

function generateHintFaces(context is Context, edgeToOrderedFaceDataBox is box, definition is map) returns Query
{
    try silent
    {
        const rawPullDirection = getPullDirection(context, definition);
        edgeToOrderedFaceDataBox[] = getEdgeToOrderedFaceData(context, definition.partingEdges, rawPullDirection, definition.hintFaces, false);

        var hintFacesArr = [];
        for (var edge in evaluateQuery(context, definition.partingEdges))
        {
            const orderedFaceData = edgeToOrderedFaceDataBox[][edge];
            if (orderedFaceData != undefined)
            {
                hintFacesArr = append(hintFacesArr, orderedFaceData[0].face);
            }
            else
            {
                // `qAdjacent(...)` evaluation is ordered by transientId
                const adjacentFaces = evaluateQuery(context, qAdjacent(edge, AdjacencyType.EDGE, EntityType.FACE));
                hintFacesArr = append(hintFacesArr, adjacentFaces[0]);
            }
        }

        return qUnion(hintFacesArr);
    }

    return qNothing();
}

/*
 * Parting line draft: When moving from 0 to 1 selections, try to guess which face the user would prefer to draft (for
 * ONE_SIDED drafts).
 */
predicate canFlipAlongPull(context is Context, oldDefinition is map, definition is map, specifiedParameters is map)
{
    specifiedParameters.partingEdges;
    oldDefinition.partingEdges != definition.partingEdges;
    isQueryEmpty(context, oldDefinition.partingEdges);
    size(evaluateQuery(context, definition.partingEdges)) == 1;
}

function flipAlongPull(context is Context, edgeToOrderedFaceDataBox is box, definition is map) returns map
{
    try silent
    {
        const rawPullDirection = getPullDirection(context, definition);
        if (edgeToOrderedFaceDataBox[] == undefined)
        {
            edgeToOrderedFaceDataBox[] = getEdgeToOrderedFaceData(context, definition.partingEdges, rawPullDirection,
                    definition.hintFaces, true);
        }
        const manipulatorData = getDataForManipulator(context, rawPullDirection, edgeToOrderedFaceDataBox[], definition, false);
        const manipulatorFaceData = manipulatorData.manipulatorFaceData;
        const otherFaceData = manipulatorData.otherFaceData;

        const equalSteepness = abs(abs(manipulatorFaceData.steepness) - abs(otherFaceData.steepness)) < STEEPNESS_TOLERANCE;
        if (equalSteepness)
        {
            // Favor the "away" face
            const currentAlong = abs(manipulatorFaceData.steepness) > STEEPNESS_TOLERANCE && manipulatorFaceData.steepness > 0;
            const otherAway = abs(otherFaceData.steepness) > STEEPNESS_TOLERANCE && otherFaceData.steepness < 0;
            if (currentAlong && otherAway)
            {
                definition.alongPull = !definition.alongPull;
            }
        }
        else
        {
            const currentFaceSteeper = abs(manipulatorFaceData.steepness) > abs(otherFaceData.steepness);
            if (!currentFaceSteeper)
            {
                // flip pull direction if the other face is steeper
                definition.alongPull = !definition.alongPull;
            }
        }
    }

    return definition;
}

/*
 * Parting line draft: when switching between partingLineSides modes, make sure that the transitions feel smooth
 */
predicate canAdjustDirectionsForSideChange(oldDefinition is map, definition is map, specifiedParameters is map)
{
    specifiedParameters.partingLineSides;
    oldDefinition.partingLineSides != definition.partingLineSides;
}

function adjustDirectionsForSideChange(context is Context, edgeToOrderedFaceDataBox is box, oldDefinition is map, definition is map) returns map
{
    try silent
    {
        const rawPullDirection = getPullDirection(context, definition);

        if (edgeToOrderedFaceDataBox[] == undefined)
        {
            edgeToOrderedFaceDataBox[] = getEdgeToOrderedFaceData(context, definition.partingEdges, rawPullDirection,
                    definition.hintFaces, true);
        }

        const oldFaceData = getDataForManipulator(context, rawPullDirection, edgeToOrderedFaceDataBox[], oldDefinition, false).manipulatorFaceData;
        const newFaceData = getDataForManipulator(context, rawPullDirection, edgeToOrderedFaceDataBox[], definition, false).manipulatorFaceData;

        const intoTwoSided = definition.partingLineSides == PartingLineSides.TWO_SIDED;
        const outOfTwoSided = oldDefinition.partingLineSides == PartingLineSides.TWO_SIDED;

        if (!intoTwoSided && !outOfTwoSided)
        {
            // When switching between ONE_SIDED and SYMMETRIC, make sure the face with the manipulator stays inwards if
            // it was inward, or stays outward if it was outward.
            if (oldFaceData.isAlong != newFaceData.isAlong)
            {
                definition.pullDirection = !definition.pullDirection;
            }
        }
        else
        {
            // When switching into TWO_SIDED, make sure the face that had the manipulator does not change.
            const oldFaceCorrespondsToSecondAngle = intoTwoSided && oldFaceData.isAlong;
            // When switching out of TWO_SIDED, make sure the face that will have the manipulator does not change.
            const newFaceCorrespondsToSecondAngle = outOfTwoSided && newFaceData.isAlong;

            if (oldFaceCorrespondsToSecondAngle || newFaceCorrespondsToSecondAngle)
            {
                // The "along" face corresponds to secondAngle, so if the manipulator was controlling or will be
                // controlling an "along" face, we should move that data to secondAngle.
                definition.secondAngle = oldDefinition.angle;
                definition.secondPullDirection = oldDefinition.pullDirection;
                // Swap old second angle into first angle; otherwise this will just result in symmetry.
                definition.angle = oldDefinition.secondAngle;
                definition.pullDirection = oldDefinition.secondPullDirection;
            }
        }
    }

    return definition;
}

