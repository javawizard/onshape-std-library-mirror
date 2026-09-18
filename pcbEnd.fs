FeatureScript 3083; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "3083.0");
import(path : "onshape/std/containers.fs", version : "3083.0");
import(path : "onshape/std/coordSystem.fs", version : "3083.0");
import(path : "onshape/std/evaluate.fs", version : "3083.0");
import(path : "onshape/std/feature.fs", version : "3083.0");
import(path : "onshape/std/persistentCoordSystem.fs", version : "3083.0");
import(path : "onshape/std/query.fs", version : "3083.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "3083.0");
import(path : "onshape/std/sheetMetalEnd.fs", version : "3083.0");
import(path : "onshape/std/sheetMetalUnfold.fs", version : "3083.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "3083.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "3083.0");
import(path : "onshape/std/transform.fs", version : "3083.0");
import(path : "onshape/std/units.fs", version : "3083.0");
import(path : "onshape/std/vector.fs", version : "3083.0");

// ---- Bend lines attribute ----

const PCB_BEND_LINES_ATTRIBUTE_NAME = "pcbZoneBendLines";

/**
 * A single bend centerline segment, stored as a [PersistentCoordSystem] that tracks the
 * bend line position and direction through transforms and patterns, along with the bend
 * angle, bend radius, and bend direction.
 *
 * The bend line endpoints can be recovered via [pcbBendLineStartPoint] and [pcbBendLineEndPoint].
 *
 * @type {{
 *      @field bendLineCoordSystem {PersistentCoordSystem} : A persistent coordinate system whose origin is the
 *              start point of the bend centerline and whose zAxis is the direction from start to end.
 *      @field endPointDistance {ValueWithUnits} : The distance from the start point to the end point along the
 *              bend centerline.
 *      @field angle {ValueWithUnits} : The bend angle.
 *      @field radius {ValueWithUnits} : The bend radius.
 *      @field bendUp {boolean} : The bend direction. `true` indicates the bend goes upward
 *              relative to the PCB model's up direction.
 * }}
 */
export type PcbBendLineInfo typecheck canBePcbBendLineInfo;

/** @internal */
export predicate canBePcbBendLineInfo(value)
{
    value is map;
    value.bendLineCoordSystem is PersistentCoordSystem;
    isLength(value.endPointDistance);
    isAngle(value.angle);
    isLength(value.radius);
    value.bendUp is boolean;
}

/**
 * Creates a [PcbBendLineInfo] from the bend line start and end points, bend angle, bend radius,
 * and bend direction.
 *
 * @param coordSystemId {string} : A unique identifier for this bend line's persistent coordinate system.
 * @param startPoint {Vector} : The 3D start point of the bend centerline in world space.
 * @param endPoint {Vector} : The 3D end point of the bend centerline in world space.
 * @param angle {ValueWithUnits} : The bend angle.
 * @param radius {ValueWithUnits} : The bend radius.
 * @param bendUp {boolean} : The bend direction.
 */
export function pcbBendLineInfo(coordSystemId is string, startPoint is Vector, endPoint is Vector,
    angle is ValueWithUnits, radius is ValueWithUnits, bendUp is boolean) returns PcbBendLineInfo
{
    const direction = normalize(endPoint - startPoint);
    const xAxis = perpendicularVector(direction);
    return {
        "bendLineCoordSystem" : persistentCoordSystem(coordSystem(startPoint, xAxis, direction), coordSystemId, true),
        "endPointDistance" : norm(endPoint - startPoint),
        "angle" : angle,
        "radius" : radius,
        "bendUp" : bendUp
    } as PcbBendLineInfo;
}

/**
 * Returns the start point of a bend line from its [PcbBendLineInfo].
 */
export function pcbBendLineStartPoint(bendLineInfo is PcbBendLineInfo) returns Vector
{
    return bendLineInfo.bendLineCoordSystem.coordSystem.origin;
}

/**
 * Returns the end point of a bend line from its [PcbBendLineInfo].
 */
export function pcbBendLineEndPoint(bendLineInfo is PcbBendLineInfo) returns Vector
{
    return bendLineInfo.bendLineCoordSystem.coordSystem.origin +
           bendLineInfo.bendLineCoordSystem.coordSystem.zAxis * bendLineInfo.endPointDistance;
}

/**
 * Stores an array of [PcbBendLineInfo] as an attribute on a face of `body` (typically the finished PCB model body).
 * The attribute is stored on a face rather than the body itself so that PersistentCoordSystem
 * values are correctly transformed when the body is patterned.
 * Any previously stored bend lines on the body are replaced.
 *
 * @param context {Context} : The feature context.
 * @param body {Query} : The body to store bend line information on.
 * @param bendLineInfos {array} : An array of [PcbBendLineInfo] values describing each bend centerline.
 */
export function setPcbBendLinesAttribute(context is Context, body is Query, bendLineInfos is array)
precondition
{
    for (var lineInfo in bendLineInfos)
        lineInfo is PcbBendLineInfo;
}
{
    setAttribute(context, {
        "entities" : body,
        "name" : PCB_BEND_LINES_ATTRIBUTE_NAME,
        "attribute" : bendLineInfos
    });
}

/**
 * Returns the array of [PcbBendLineInfo] stored on `body` by [setPcbBendLinesAttribute],
 * or an empty array if no bend lines attribute is present.
 *
 * @param context {Context} : The feature context.
 * @param body {Query} : The body to read bend line information from.
 */
export function getPcbBendLinesAttribute(context is Context, body is Query) returns array
{
    const attributes = getAttributes(context, {
        "entities" : body,
        "name" : PCB_BEND_LINES_ATTRIBUTE_NAME
    });
    return attributes == [] ? [] : attributes[0];
}

// ---- Up direction attribute ----

const PCB_UP_DIRECTION_ATTRIBUTE_NAME = "pcbZoneUpDirection";

/**
 * Stores the `upDirection` vector from the PCB model on a face of `body` as a [PersistentCoordSystem]
 * so that it is automatically transformed when the body is patterned or transformed.
 *
 * @param context {Context} : The feature context.
 * @param body {Query} : The body to store the up direction on.
 * @param upDirection {Vector} : A unit vector representing the PCB model's up direction.
 */
export function setPcbUpDirectionAttribute(context is Context, body is Query, upDirection is Vector)
{
    const xAxis = perpendicularVector(upDirection);
    setAttribute(context, {
        "entities" : body,
        "name" : PCB_UP_DIRECTION_ATTRIBUTE_NAME,
        "attribute" : persistentCoordSystem(coordSystem(WORLD_ORIGIN, xAxis, upDirection), "pcbUpDirection", true)
    });
}

/**
 * Returns the `upDirection` vector stored on `body` by [setPcbUpDirectionAttribute],
 * or a zero vector if no attribute is present.
 *
 * @param context {Context} : The feature context.
 * @param body {Query} : The body to read the up direction from.
 */
export function getPcbUpDirectionAttribute(context is Context, body is Query) returns Vector
{
    const attributes = getAttributes(context, {
        "entities" : body,
        "name" : PCB_UP_DIRECTION_ATTRIBUTE_NAME
    });
    if (attributes == [])
        return vector(0, 0, 0);
    const persistentCSys = attributes[0];
    if (persistentCSys.coordSystem == undefined)
        return vector(0, 0, 0);
    return persistentCSys.coordSystem.zAxis;
}

/**
 * Controls how the flex PCB model is unfolded before finishing.
 */
export enum PcbUnfoldType
{
    annotation { "Name" : "Unfold all" }
    UNFOLD_ALL,
    annotation { "Name" : "Unfold selected" }
    UNFOLD_SELECTED,
    annotation { "Name" : "Unfold none" }
    UNFOLD_NONE
}

/**
 * Combines the Unfold and Finish operations into a single step, intended for use with flexible PCB models.
 * First unfolds the selected bends or rolled walls, then deactivates the PCB model of the specified parts.
 * This feature removes the PCB model from the flat pattern.
 *
 * @param definition {{
 *      @field unfoldType {PcbUnfoldType} : If `UNFOLD_ALL`, unfold all bends in the selected part.
 *              If `UNFOLD_SELECTED`, only the entities specified by `entitiesToUnfold` are unfolded. Default is `UNFOLD_ALL`.
 *      @field entitiesToUnfold {Query}: @requiredif {`unfoldType` is `UNFOLD_SELECTED`}
 *              Specific bend or non-planar faces to unfold.
 *      @field holdEntity {Query}: @requiredif {`unfoldType` is not `UNFOLD_NONE`}
 *              A wall face or bounding edge that remains fixed during the unfold.
 *      @field pcbPart {Query} : The PCB model body to deactivate (finish).
 * }}
 */
annotation { "Feature Type Name" : "Finish flex PCB model", "Editing Logic Function" : "pcbEndEditLogic" }
export const pcbEnd = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        // Hidden parameter which collects the preselection so that `pcbEndEditLogic` can route it to `holdEntity`.
        // It must be the first query parameter in the precondition, since the client offers the preselection to the
        // first query parameter of the feature, and `entitiesToUnfold` is not shown for the default unfold type.
        annotation { "Name" : "Entities", "UIHint" : UIHint.ALWAYS_HIDDEN,
                     "Filter" : (EntityType.FACE || EntityType.EDGE) && ActiveSheetMetal.YES && SMApplicationType.FLEXIBLE_PCB &&
                                (SheetMetalDefinitionEntityType.EDGE || SheetMetalDefinitionEntityType.FACE) && ModifiableEntityOnly.YES }
        definition.initEntities is Query;

        annotation { "Name" : "Unfold type" }
        definition.unfoldType is PcbUnfoldType;

        if (definition.unfoldType == PcbUnfoldType.UNFOLD_SELECTED)
        {
            annotation { "Name" : "Bend or non-planar faces to unfold",
                         "UIHint" : UIHint.FOCUS_ON_VISIBLE,
                         "Filter" : EntityType.FACE && !GeometryType.PLANE && ActiveSheetMetal.YES && SMApplicationType.FLEXIBLE_PCB && (SheetMetalDefinitionEntityType.EDGE || SheetMetalDefinitionEntityType.FACE) && ModifiableEntityOnly.YES }
            definition.entitiesToUnfold is Query;
        }

        if (definition.unfoldType != PcbUnfoldType.UNFOLD_NONE)
        {
            annotation { "Name" : "Wall face or bounding edge to hold",
             "Filter" : ActiveSheetMetal.YES && SMApplicationType.FLEXIBLE_PCB && (SheetMetalDefinitionEntityType.EDGE || SheetMetalDefinitionEntityType.FACE) && ModifiableEntityOnly.YES,
             "MaxNumberOfPicks" : 1 }
            definition.holdEntity is Query;
        }

        annotation { "Name" : "PCB part to finish",
                     "Filter" : EntityType.BODY && ActiveSheetMetal.YES && SMApplicationType.FLEXIBLE_PCB && ModifiableEntityOnly.YES,
                     "MaxNumberOfPicks" : 1 }
        definition.pcbPart is Query;
    }
    {
        if (definition.unfoldType != PcbUnfoldType.UNFOLD_NONE)
        {
            const unfoldAll = definition.unfoldType == PcbUnfoldType.UNFOLD_ALL;

            if (unfoldAll || !isQueryEmpty(context, definition.entitiesToUnfold))
            {
                const entitiesToCheck = unfoldAll ? definition.holdEntity : definition.entitiesToUnfold;
                if (!isQueryEmpty(context, entitiesToCheck) && !isQueryEmpty(context, definition.pcbPart) &&
                    !isQueryEmpty(context, qSubtraction(entitiesToCheck->qOwnerBody(), definition.pcbPart)))
                {
                    throw regenError(ErrorStringEnum.PCB_PARTS_TO_UNFOLD_MISMATCH, ["pcbPart"]);
                }

                callSubfeatureAndProcessStatus(id, sheetMetalUnfold, context, id + "unfold", {
                    "unfoldAll" : unfoldAll,
                    "partToUnfold" : unfoldAll ? definition.pcbPart : qNothing(),
                    "entitiesToUnfold" : unfoldAll ? qNothing() : definition.entitiesToUnfold,
                    "holdEntity" : definition.holdEntity
                });
            }
        }

        const flatPatternBodies = evaluateQuery(context, qCorrespondingInFlat(definition.pcbPart)->qEntityFilter(EntityType.BODY));

        // Scope SM queries to only the model that owns the parts being finished
        const smModelQ = qOwnerBody(qUnion(getSMDefinitionEntities(context, definition.pcbPart)));
        const smEdgeQ = qGeometry(qEdgeTopologyFilter(qOwnedByBody(smModelQ, EntityType.EDGE), EdgeTopology.TWO_SIDED), GeometryType.LINE);
        const smCylinderQ = qGeometry(qOwnedByBody(smModelQ, EntityType.FACE), GeometryType.CYLINDER);
        var bendLineBodiesArr = [];
        var bendLineInfos = [];

        // Read flipDirectionUp from the SM model attribute
        const modelAttributes = getSmObjectTypeAttributes(context, smModelQ, SMObjectType.MODEL);
        const flipDirectionUp = modelAttributes != [] && modelAttributes[0].flipDirectionUp == true;

        var flatToFolded = identityTransform();
        if (!isQueryEmpty(context, definition.holdEntity))
        {
            flatToFolded = inverse(evSheetMetalFlatTransformation(context, { "face" : definition.holdEntity }));
        }
        for (var attribute in getSMAssociationAttributes(context, qUnion([smEdgeQ, smCylinderQ])))
        {
            const wireBodyQ = qEntityFilter(qBodyType(qAttributeQuery(attribute), BodyType.WIRE), EntityType.BODY);
            if (isQueryEmpty(context, wireBodyQ))
            {
                continue;
            }
            bendLineBodiesArr = append(bendLineBodiesArr, wireBodyQ);

            // Get the SM definition entity (edge or cylinder face) to read the joint attribute
            const smDefinitionEntity = qNthElement(qAttributeFilter(qUnion([smEdgeQ, smCylinderQ]), attribute), 0);
            const jointAttribute = getJointAttribute(context, smDefinitionEntity);
            if (jointAttribute == undefined || jointAttribute.jointType.value != SMJointType.BEND)
                continue;

            const bendAngle = jointAttribute.angle.value;
            const bendRadius = jointAttribute.radius.value;

            // Read bend direction from the Parasolid attribute on the centerline wire body
            const bendUp = try silent(evSheetMetalBendUp(context, { "wireBody" : wireBodyQ })) == true;

            for (var wireEdge in evaluateQuery(context, qOwnedByBody(wireBodyQ, EntityType.EDGE)))
            {
                const startPoint = flatToFolded * evVertexPoint(context, { "vertex" : qEdgeVertex(wireEdge, false) });
                const endPoint = flatToFolded * evVertexPoint(context, { "vertex" : qEdgeVertex(wireEdge, true) });
                bendLineInfos = append(bendLineInfos, pcbBendLineInfo(
                    "pcbBendLine_" ~ id[0] ~ size(bendLineInfos), startPoint, endPoint, bendAngle, bendRadius, bendUp));
            }
        }
        const bendLineBodies = bendLineBodiesArr == [] ? [] : evaluateQuery(context, qUnion(bendLineBodiesArr));
        if (!isQueryEmpty(context, definition.holdEntity))
        {
            const holdEntityQ = qUnion(getSMDefinitionEntities(context, definition.holdEntity));
            var holdPlane;
            if (!isQueryEmpty(context, holdEntityQ->qEntityFilter(EntityType.FACE)))
            {
                holdPlane = evPlane(context, { "face" : holdEntityQ });
            }
            else
            {
                // holdEntity is a one-sided edge; get the adjacent face
                const adjacentFace = holdEntityQ->qAdjacent(AdjacencyType.EDGE, EntityType.FACE);
                holdPlane = try silent(evPlane(context, { "face" : adjacentFace }));
                if (holdPlane == undefined)
                {
                    // Adjacent face is not planar — evaluate tangent plane at edge midpoint
                    holdPlane = evFaceTangentPlaneAtEdge(context, {
                        "edge" : holdEntityQ,
                        "face" : adjacentFace,
                        "parameter" : 0.5
                    });
                }
            }
            holdPlane = flipDirectionUp ? holdPlane : flip(holdPlane);

            // Store bend lines and upDirection while SM is still active, so that the
            // persistent coordinate system is assigned to both the SM definition topology and the 3D face.
            for (var part in evaluateQuery(context, definition.pcbPart))
            {
                setPcbBendLinesAttribute(context, part, bendLineInfos);
                setPcbUpDirectionAttribute(context, part, holdPlane.normal);
            }
        }

        callSubfeatureAndProcessStatus(id, sheetMetalEnd, context, id + "end", {
            "sheetMetalParts" : definition.pcbPart
        });

        if (flatPatternBodies != [] || bendLineBodies != [])
        {
            opDeleteBodies(context, id + "deleteFlatPatterns", { "entities" : qUnion(concatenateArrays([flatPatternBodies, bendLineBodies])) });
        }
    }, { "initEntities" : qNothing() });

/** @internal */
export function pcbEndEditLogic(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean) returns map
{
    // Preselection processing. `oldDefinition` is empty only on the first call, when the dialog is opened.
    if (oldDefinition == {})
    {
        if (definition.unfoldType != PcbUnfoldType.UNFOLD_NONE && isQueryEmpty(context, definition.holdEntity) &&
            !isQueryEmpty(context, definition.initEntities))
        {
            // `holdEntity` accepts a single pick, so only the first preselected entity is used.
            definition.holdEntity = qNthElement(definition.initEntities, 0);
        }
        // Clear out the pre-selection data: this is especially important if the query is to imported data
        definition.initEntities = qNothing();
    }

    // Auto-populate pcbPart only if it wasn't explicitly cleared by the user
    if (isQueryEmpty(context, definition.pcbPart) && (oldDefinition.pcbPart == undefined || isQueryEmpty(context, oldDefinition.pcbPart)))
    {
        var ownerBody = qNothing();
        if (definition.unfoldType == PcbUnfoldType.UNFOLD_SELECTED && !isQueryEmpty(context, definition.entitiesToUnfold))
        {
            ownerBody = definition.entitiesToUnfold->qOwnerBody();
        }
        else if (!isQueryEmpty(context, definition.holdEntity))
        {
            ownerBody = definition.holdEntity->qOwnerBody();
        }
        const body = qEntityFilter(qBodyType(ownerBody, BodyType.SOLID), EntityType.BODY)->qActiveSheetMetalFilter(ActiveSheetMetal.YES);
        if (!isQueryEmpty(context, body))
        {
            definition.pcbPart = qNthElement(body, 0);
        }
    }
    return definition;
}

