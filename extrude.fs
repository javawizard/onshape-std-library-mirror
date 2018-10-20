FeatureScript 937; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/extrudeCommon.fs", version : "937.0");
export import(path : "onshape/std/query.fs", version : "937.0");
export import(path : "onshape/std/tool.fs", version : "937.0");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "937.0");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "937.0");
import(path : "onshape/std/boolean.fs", version : "937.0");
import(path : "onshape/std/booleanHeuristics.fs", version : "937.0");
import(path : "onshape/std/box.fs", version : "937.0");
import(path : "onshape/std/containers.fs", version : "937.0");
import(path : "onshape/std/coordSystem.fs", version : "937.0");
import(path : "onshape/std/curveGeometry.fs", version : "937.0");
import(path : "onshape/std/drafttype.gen.fs", version : "937.0");
import(path : "onshape/std/evaluate.fs", version : "937.0");
import(path : "onshape/std/feature.fs", version : "937.0");
import(path : "onshape/std/mathUtils.fs", version : "937.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "937.0");
import(path : "onshape/std/sheetMetalBuiltIns.fs", version : "937.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "937.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "937.0");
import(path : "onshape/std/transform.fs", version : "937.0");
import(path : "onshape/std/valueBounds.fs", version : "937.0");

/**
 * The viewer being operated in
 * @internal
 */
export enum OperationDomain
{
    MODEL,
    FLAT
}

/**
 * @internal
 */
export enum FlatOperationType
{
    annotation { "Name" : "Add" }
    ADD,
    annotation { "Name" : "Remove" }
    REMOVE
}

/**
 * Create an extrude, as used in Onshape's extrude feature.
 *
 * Internally, performs an [opExtrude], followed by an [opBoolean], possibly followed by a
 * [opDraft], possibly in two directions. If creating a simple extrusion, prefer using
 * [opExtrude] alone.
 *
 * @param id : @autocomplete `id + "extrude1"`
 * @param definition {{
 *      @field bodyType {ToolBodyType}: @optional
 *              Specifies a `SOLID` or `SURFACE` extrude. Default is `SOLID`.
 *      @field entities {Query}: @requiredif {`bodyType` is `SOLID`}
 *              The planar faces and/or sketch regions to extrude.
 *              @eg `qSketchRegion(id + "sketch1")` specifies all sketch regions of a given sketch.
 *      @field surfaceEntities {Query}: @requiredif {`bodyType` is `SURFACE`}
 *              The sketch curves to extrude.
 *              @example `qCreatedBy(id + "sketch1", EntityType.EDGE)`
 *
 *      @field endBound {BoundingType}: @optional
 *              The end bounding condition for the extrude. Default is `BLIND`.
 *              @autocomplete `BoundingType.BLIND`
 *      @field oppositeDirection {boolean}: @optional
 *              @ex `true` to flip the direction of the extrude to point opposite the face/sketch
 *              normal.
 *
 *      @field depth {ValueWithUnits}: @requiredif {`endBound` is `BLIND` or `SYMMETRIC`}
 *              A length specifying the extrude depth. For a symmetric extrude, specifies the full
 *              extrude depth. For a blind extrude, specifies the depth of the first extrude
 *              direction.
 *              @eg `0.5 * inch`
 *      @field endBoundEntityFace {Query}: @requiredif {`endBound` is `UP_TO_SURFACE`}
 *              Specifies the face or surface to bound the extrude.
 *      @field endBoundEntityBody {Query}: @requiredif {`endBound` is `UP_TO_BODY`}
 *              Specifies the surface or solid body to bound the extrude.
 *      @field endBoundEntityVertex {Query}: @requiredif {`endBound` is `UP_TO_VERTEX`}
 *              Specifies the vertex to bound the extrude.
 *
 *      @field hasOffset {boolean}: @optional
 *              @ex `true` to add a translational offset from the selected face, surface, solid body or vertex.
 *      @field offsetDistance {ValueWithUnits}: @requiredif {`offset` is `true`}
 *              The translational distance between the selected face, surface, solid body or vertex and the cap of the
 *              extrude. @ex `0.5 * inch`
 *      @field offsetOppositeDirection {boolean}: @optional
 *              @ex `false` to offset away from the selected end bound (default)
 *              @ex `true` to offset into the selected end bound
 *
 *      @field hasDraft {boolean} : @optional
 *              @ex `true` to add a draft to the extrude.
 *      @field draftAngle {ValueWithUnits}: @requiredif {`hasDraft` is `true`}
 *              The angle, as measured from the extrude direction, at which to draft.
 *              @ex `10 * degree`
 *      @field draftPullDirection {boolean} : @optional
 *              @ex `false` to draft outwards (default)
 *              @ex `true` to draft inwards
 *
 *
 *      @field hasSecondDirection {boolean} : @optional
 *              @example `true` to specify a second direction.
 *
 *      @field secondDirectionBound {SecondDirectionBoundingType}: @optional
 *              The bounding type of the second direction. Can be different from the bounding type of the first direction.
 *      @field secondDirectionOppositeDirection {boolean} : @optional
 *              @ex `true` will flip the second end direction to align with the plane/face's normal.
 *
 *      @field secondDirectionDepth {ValueWithUnits}: @requiredif {`secondDirectionBound` is `BLIND`}
 *              A length specifying the second direction's extrude depth.
 *      @field secondDirectionBoundEntityFace {Query}: @requiredif {`secondDirectionBound` is `UP_TO_SURFACE`}
 *              specifies the face or surface to bound the extrude.
 *      @field secondDirectionBoundEntityBody {Query}: @requiredif {`secondDirectionBound` is `UP_TO_BODY`}
 *              specifies the surface or solid body to bound the extrude.
 *      @field secondDirectionBoundEntityVertex {Query}: @requiredif {`secondDirectionBound` is `UP_TO_VERTEX`}
 *              specifies the vertex to bound the extrude.
 *
 *      @field hasSecondDirectionOffset {boolean}: @optional
 *              @ex `true` to add a translational offset from the selected face, surface, solid body or vertex.
 *      @field secondDirectionOffsetDistance {ValueWithUnits}: @requiredif {`offset` is `true`}
 *              The translational distance between the selected face, surface, solid body or vertex and the cap of the
 *              extrude. @ex `0.5 * inch`
 *      @field secondDirectionOffsetOppositeDirection {boolean}: @optional
 *              @ex `false` to offset away from the selected second direction end bound (default)
 *              @ex `true` to offset into the selected second direction end bound
 *
 *      @field hasSecondDirectionDraft {boolean} : @optional
 *              @ex `true` to add a draft to the second direction extrude.
 *      @field secondDirectionDraftPullDirection {boolean} : @optional
 *              @ex `false` to draft the second direction outwards (default)
 *              @ex `true` to draft the second direction inwards
 *
 *      @field operationType {NewBodyOperationType} : @optional
 *              Specifies how the newly created body will be merged with existing bodies.
 *      @field defaultScope {boolean} : @optional
 *              @ex `true` to merge with all other bodies
 *              @ex `false` to merge with `booleanScope`
 *      @field booleanScope {Query} : @requiredif {`defaultScope` is `false`}
 *              The specified bodies to merge with.
 * }}
 */
annotation { "Feature Type Name" : "Extrude",
             "Manipulator Change Function" : "extrudeManipulatorChange",
             "Filter Selector" : "allparts",
             "Editing Logic Function" : "extrudeEditLogic" }
export const extrude = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "UIHint" : "ALWAYS_HIDDEN", "Default" : OperationDomain.MODEL }
        definition.domain is OperationDomain;

        if (definition.domain != OperationDomain.FLAT)
        {
            annotation { "Name" : "Creation type", "UIHint" : "HORIZONTAL_ENUM" }
            definition.bodyType is ToolBodyType;

            if (definition.bodyType != ToolBodyType.SURFACE)
            {
                booleanStepTypePredicate(definition);
            }
            else
            {
                surfaceOperationTypePredicate(definition);
            }
        }
        else
        {
            annotation { "Name" : "Creation type", "UIHint" : "HORIZONTAL_ENUM", "Default" : FlatOperationType.REMOVE }
            definition.flatOperationType is FlatOperationType;
        }

        if (definition.bodyType != ToolBodyType.SURFACE || definition.domain == OperationDomain.FLAT)
        {
            annotation { "Name" : "Faces and sketch regions to extrude",
                         "Filter" : ((AllowFlattenedGeometry.YES && SketchObject.YES && EntityType.FACE) ||
                          (GeometryType.PLANE && AllowFlattenedGeometry.NO && EntityType.FACE)) && ConstructionObject.NO
                          }
            definition.entities is Query;
        }
        else
        {
            annotation { "Name" : "Sketch curves to extrude",
                         "Filter" : (EntityType.EDGE && SketchObject.YES && ModifiableEntityOnly.YES && ConstructionObject.NO ) }
            definition.surfaceEntities is Query;
        }

        if (definition.domain != OperationDomain.FLAT)
        {
            mainViewExtrudePredicate(definition);
        }
    }
    {
        if (definition.bodyType == ToolBodyType.SOLID)
        {
            const modelEntities = qSMFlatFilter(definition.entities, SMFlatType.NO);
            const containsModelEntities = (size(evaluateQuery(context, modelEntities)) > 0);
            const containsFlattened = queryContainsFlattenedSheetMetal(context, definition.entities);
            if (definition.domain == OperationDomain.FLAT)
            {
                if (containsModelEntities)
                {
                    const errorString = (containsFlattened) ? ErrorStringEnum.EXTRUDE_3D_AND_FLAT :
                                                                        ErrorStringEnum.DEFINED_IN_SM_FLAT_CANT_REFERENCE_3D;
                    throw regenError(errorString, ["entities"], modelEntities);
                }
                try(SMFlatOp(context, id + "flatOp", { "faces" : definition.entities,
                                "flatOperationType" : definition.flatOperationType }));
                processSubfeatureStatus(context, id, { "subfeatureId" : id + "flatOp", "propagateErrorDisplay" : true });
                return;
            }
            else if (containsFlattened)
            {
                const errorString = (containsModelEntities) ? ErrorStringEnum.EXTRUDE_3D_AND_FLAT :
                                                                        ErrorStringEnum.DEFINED_IN_3D_CANT_REFERENCE_SM_FLAT;
                throw regenError(errorString, ["entities"]);
            }
        }

        // Handle negative inputs
        definition = adjustExtrudeDirectionForBlind(definition);

        const entities = getEntitiesToUse(context, definition);

        const resolvedEntities = evaluateQuery(context, entities);
        if (size(resolvedEntities) == 0)
        {
            if (definition.bodyType == ToolBodyType.SOLID)
                throw regenError(ErrorStringEnum.EXTRUDE_NO_SELECTED_REGION, ["entities"]);
            else
                throw regenError(ErrorStringEnum.EXTRUDE_SURF_NO_CURVE, ["surfaceEntities"]);
        }

        // Handle pattern feature instance transform if needed
        definition.transform = getRemainderPatternTransform(context, {"references" : entities});

        // Compute the draftCondition before definition gets changed
        const draftCondition is map = getDraftConditions(definition);

        // Get the extrude axis
        const extrudeAxis = try(computeExtrudeAxis(context, resolvedEntities[0], definition.transform));
        if (extrudeAxis == undefined)
            throw regenError(ErrorStringEnum.EXTRUDE_NO_DIRECTION);

        // Add manipulator
        addExtrudeManipulator(context, id, definition, entities, extrudeAxis);

        // Transform the definition
        definition = transformExtrudeDefinitionForOpExtrude(context, id, entities, extrudeAxis.direction, definition);

        // Perform the operation
        extrudeWithDraft(context, id, definition, draftCondition);

        const reconstructOp = function(id) { extrudeWithDraft(context, id, definition, draftCondition); };

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            processNewBodyIfNeeded(context, id, definition, reconstructOp);
        }
        else if (definition.surfaceOperationType == NewSurfaceOperationType.ADD)
        {
            var matches = createTopologyMatchesForSurfaceJoin(context, id, definition, qCapEntity(id, CapType.START), definition.surfaceEntities, definition.transform);
            checkForNotJoinableSurfacesInScope(context, id, definition, matches);
            joinSurfaceBodies(context, id, matches, false, reconstructOp);
        }

        cleanupVertexBoundaryPlane(context, id, definition);

    }, { endBound : BoundingType.BLIND, oppositeDirection : false,
            bodyType : ToolBodyType.SOLID, operationType : NewBodyOperationType.NEW,
            secondDirectionBound : SecondDirectionBoundingType.BLIND,
            secondDirectionOppositeDirection : true, hasSecondDirection : false,
            hasOffset: false, hasSecondDirectionOffset: false,
            offsetOppositeDirection: false, secondDirectionOffsetOppositeDirection: false,
            hasDraft: false, hasSecondDirectionDraft: false,
            draftPullDirection : false, secondDirectionDraftPullDirection : false,
            surfaceOperationType : NewSurfaceOperationType.NEW,
            defaultSurfaceScope : true,
            domain : OperationDomain.MODEL,
            flatOperationType : FlatOperationType.REMOVE
    });

predicate supportsDraft(definition is map)
{
    definition.bodyType == ToolBodyType.SOLID;
}

predicate firstDirectionNeedsDraft(definition is map)
{
    supportsDraft(definition);
    definition.hasDraft;
}

predicate symmetricNeedsDraft(definition is map)
{
    firstDirectionNeedsDraft(definition);
    definition.endBound == BoundingType.SYMMETRIC;
}

predicate hasSecondDirectionExtrude(definition is map)
{
    definition.endBound != BoundingType.SYMMETRIC;
    definition.hasSecondDirection;
}

predicate secondDirectionNeedsDraft(definition is map)
{
    supportsDraft(definition);
    hasSecondDirectionExtrude(definition);
    definition.secondDirectionOppositeDirection != definition.oppositeDirection;
    definition.hasSecondDirectionDraft;
}

predicate needsSplit(definition is map)
{
    definition.endBound == BoundingType.SYMMETRIC || (hasSecondDirectionExtrude(definition) && definition.secondDirectionOppositeDirection != definition.oppositeDirection);
}

predicate mainViewExtrudePredicate(definition is map)
{
    annotation { "Name" : "End type" }
    definition.endBound is BoundingType;

    if (definition.endBound != BoundingType.SYMMETRIC)
    {
        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;
    }

    extrudeBoundParametersPredicate(definition);

    if (definition.bodyType == ToolBodyType.SOLID)
    {
        annotation { "Name" : "Draft", "UIHint" : "DISPLAY_SHORT" }
        definition.hasDraft is boolean;

        if (definition.hasDraft == true)
        {
            annotation { "Name" : "Draft angle", "UIHint" : "DISPLAY_SHORT" }
            isAngle(definition.draftAngle, ANGLE_STRICT_90_BOUNDS);

            annotation { "Name" : "Opposite direction", "Column Name" : "Draft opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
            definition.draftPullDirection is boolean;
        }
    }

    if (definition.endBound != BoundingType.SYMMETRIC)
    {
        annotation { "Name" : "Second end position" }
        definition.hasSecondDirection is boolean;

        if (definition.hasSecondDirection)
        {
            annotation { "Name" : "End type", "Column Name" : "Second end type" }
            definition.secondDirectionBound is SecondDirectionBoundingType;

            annotation { "Name" : "Opposite direction", "Column Name" : "Second opposite direction",
                         "UIHint" : "OPPOSITE_DIRECTION", "Default" : true }
            definition.secondDirectionOppositeDirection is boolean;

            extrudeSecondDirectionBoundParametersPredicate(definition);

            if (definition.bodyType == ToolBodyType.SOLID &&
                ((definition.secondDirectionOppositeDirection && !definition.oppositeDirection) ||
                 (!definition.secondDirectionOppositeDirection && definition.oppositeDirection)))
            {
                annotation { "Name" : "Draft", "Column Name" : "Second draft", "UIHint" : "DISPLAY_SHORT" }
                definition.hasSecondDirectionDraft is boolean;

                if (definition.hasSecondDirectionDraft)
                {
                    annotation { "Name" : "Draft angle", "Column Name" : "Second draft angle", "UIHint" : "DISPLAY_SHORT" }
                    isAngle(definition.secondDirectionDraftAngle, ANGLE_STRICT_90_BOUNDS);

                    annotation { "Name" : "Opposite direction", "Column Name" : "Second draft opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
                    definition.secondDirectionDraftPullDirection is boolean;
                }
            }
        }
    }
    if (definition.bodyType != ToolBodyType.SURFACE)
    {
        booleanStepScopePredicate(definition);
    }
    else
    {
        surfaceJoinStepScopePredicate(definition);
    }
}

const SMFlatOp = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    {
        const bodyQ = qUnion([qPartsAttachedTo(definition.faces), qOwnerBody(definition.faces)]);
        if (!areEntitiesFromSingleActiveSheetMetalModel(context, bodyQ))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_ACTIVE_MODEL_REQUIRED);
        }
        const smDefinitionBodiesQ = getSheetMetalModelForPart(context, bodyQ);
        const sheetMetalEntitiesQ = qUnion([qOwnedByBody(smDefinitionBodiesQ, EntityType.EDGE), qOwnedByBody(smDefinitionBodiesQ, EntityType.FACE), smDefinitionBodiesQ]);
        const tracking = startTracking(context, sheetMetalEntitiesQ);

        var initialDataPerBody = [];
        var initialData;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V860_SM_FLAT_ERRORS))
        {
            for (var smBody in evaluateQuery(context, smDefinitionBodiesQ))
            {
                initialDataPerBody = append(initialDataPerBody, {  'query' : qUnion([smBody, startTracking(context, smBody)]),
                                                'entitiesAndAttributes' : getInitialEntitiesAndAttributes(context, smBody) });
            }
        }
        else
        {
            initialData = getInitialEntitiesAndAttributes(context, smDefinitionBodiesQ);
        }

        definition.operationType = definition.flatOperationType == FlatOperationType.ADD ? BooleanOperationType.UNION : BooleanOperationType.SUBTRACTION;
        opSMFlatOperation(context, id, definition);

        for (var face in evaluateQuery(context, qEntityFilter(qCreatedBy(id), EntityType.FACE)))
        {
            var jointAttribute = getJointAttribute(context, face);
            if (jointAttribute != undefined && jointAttribute.radius != undefined && jointAttribute.radius.canBeEdited)
            {
                removeAttributes(context, { "entities" : face, "attributePattern" : jointAttribute });
                jointAttribute.radius.canBeEdited = false;
                setAttribute(context, { "entities" : face, "attribute" : jointAttribute });
            }
        }

        const newEntities = qUnion([qCreatedBy(id), tracking]);
        const affectedBodyQ = qOwnerBody(newEntities);
        if (initialData == undefined) // data was collected in initialDataPerBody
        {
            initialData = combineInitialData(context, initialDataPerBody, affectedBodyQ);
        }
        const toUpdate = assignSMAttributesToNewOrSplitEntities(context, affectedBodyQ, initialData, id);

        try (updateSheetMetalGeometry(context, id + "smUpdate", {
                    "entities" : toUpdate.modifiedEntities,
                    "deletedAttributes" : toUpdate.deletedAttributes,
                    "associatedChanges" : tracking }));
        processSubfeatureStatus(context, id, { "subfeatureId" : id + "smUpdate", "propagateErrorDisplay" : true });
    }, {});

function extrudeWithDraft(context is Context, id is Id, definition is map, draftCondition is map)
{
    opExtrude(context, id, definition);

    if (draftCondition.firstDirectionNeedsDraft || draftCondition.secondDirectionNeedsDraft)
    {
        const extrudeBodies = qSubtraction(qCreatedBy(id, EntityType.BODY), qUnion([qCreatedBy(id + "vertexPlane", EntityType.BODY), qCreatedBy(id + "secondVertexPlane", EntityType.BODY)]));
        const extrudeBodyArray = evaluateQuery(context, extrudeBodies);
        const useWildCard = isAtVersionOrLater(context, FeatureScriptVersionNumber.V396_WILD_CARD_IN_QUERIES);
        for (var i = 0; i < size(extrudeBodyArray); i += 1)
        {
            const getSubfeatureId = function (subfeatureType is string) returns Id {
                    if (useWildCard)
                    {
                        return id + ("*" ~ i) + subfeatureType;
                    }
                    else
                    {
                        return id + (subfeatureType ~ i);
                    }
                };
            draftExtrudeBody(context, id, getSubfeatureId, definition, extrudeBodyArray[i], draftCondition);
        }
    }
}

function getDraftConditions(definition is map)
{
    return
    {
        "secondDirectionNeedsDraft" : secondDirectionNeedsDraft(definition),
        "symmetricNeedsDraft" : symmetricNeedsDraft(definition),
        "firstDirectionNeedsDraft" : firstDirectionNeedsDraft(definition),
        "needsSplit" : needsSplit(definition)
    };
}

function applyDraft(context is Context, draftId is Id, draftFaces is Query,
                    draftDefinition is map, referenceFace is Query, neutralPlane is Plane)
{
    draftDefinition.draftType = DraftType.NEUTRAL_PLANE;
    draftDefinition.tangentPropagation = false;
    draftDefinition.reFillet = false;
    draftDefinition.draftFaces = draftFaces;
    draftDefinition.neutralPlane = referenceFace;
    draftDefinition.pullVec = neutralPlane.normal;
    if (!draftDefinition.pullDirection)
    {
        draftDefinition.pullVec = -draftDefinition.pullVec;
    }

    opDraft(context, draftId, draftDefinition);
}

function createNeutralPlane(context is Context, id is Id, extrudeBody is Query, direction is Vector, transform is Transform) returns Plane
{
    const dependencies = evaluateQuery(context, qDependency(extrudeBody));
    if (size(dependencies) == 0)
        throw "Cannot find any dependency for the extruded body";
    var neutralPlane;
    try
    {
        const line = evEdgeTangentLine(context, { "edge" : dependencies[0], "parameter" : 0.5 });
        neutralPlane = plane(transform * line.origin, direction);
    }
    catch
    {
        const facePlane = evFaceTangentPlane(context, { "face" : dependencies[0], "parameter" : vector(0.5, 0.5) });
        neutralPlane = plane(transform * facePlane.origin, direction);
    }

    // reference face
    const splitPlaneDefinition = { "plane" : neutralPlane, "width" : 1 * meter, "height" : 1 * meter };
    opPlane(context, id, splitPlaneDefinition);
    return neutralPlane;
}

function draftExtrudeBody(context is Context, id is Id, getSubfeatureId is function, definition is map, extrudeBody is Query, conditions is map)
{
    const neutralPlaneId = getSubfeatureId("neutralPlane");
    const neutralPlane = try(createNeutralPlane(context, neutralPlaneId, extrudeBody, definition.direction, definition.transform));

    if (neutralPlane == undefined)
        throw regenError(ErrorStringEnum.DRAFT_SELECT_NEUTRAL);

    const neutralPlaneFace = qCreatedBy(neutralPlaneId, EntityType.FACE);
    const neutralPlaneQuery = qOwnerBody(qCreatedBy(neutralPlaneId));

    if (conditions.needsSplit)
    {
        // Split the body
        const splitId = getSubfeatureId("split");
        opSplitPart(context, splitId, { targets: extrudeBody, tool: neutralPlaneQuery, keepTools: false });

        const firstBodies = qSplitBy(splitId, EntityType.BODY, false);
        const secondBodies = qSplitBy(splitId, EntityType.BODY, true);
        const firstBodyArray = evaluateQuery(context, firstBodies);
        const secondBodyArray = evaluateQuery(context, secondBodies);
        if (size(firstBodyArray) != size(secondBodyArray))
            throw regenError(ErrorStringEnum.SPLIT_INVALID_INPUT);

        const draftFirstId = getSubfeatureId("draftFirst");
        const draftSecondId = getSubfeatureId("draftSecond");

        const firstFaces = qSplitBy(splitId, EntityType.FACE, false);
        const secondFaces = qSplitBy(splitId, EntityType.FACE, true);

        var draftFirstDefinition = undefined;
        var draftSecondDefinition = undefined;
        // Apply draft
        if (conditions.symmetricNeedsDraft)
        {
            draftFirstDefinition = { "angle" : definition.draftAngle,
                "pullDirection" : definition.draftPullDirection };
            draftSecondDefinition = { "angle" : definition.draftAngle,
                "pullDirection" : !definition.draftPullDirection };
        }
        else
        {
            if (conditions.firstDirectionNeedsDraft)
            {
                draftFirstDefinition = { "angle" : definition.draftAngle,
                    "pullDirection" : definition.draftPullDirection };
            }
            if (conditions.secondDirectionNeedsDraft)
            {
                draftSecondDefinition = { "angle" : definition.secondDirectionDraftAngle,
                    "pullDirection" : !definition.secondDirectionDraftPullDirection };
            }
        }

        if (draftFirstDefinition != undefined)
            applyDraft(context, draftFirstId, firstFaces, draftFirstDefinition, neutralPlaneFace, neutralPlane);
        if (draftSecondDefinition != undefined)
            applyDraft(context, draftSecondId, secondFaces, draftSecondDefinition, neutralPlaneFace, neutralPlane);

        const toolsQ = qUnion([firstBodies, secondBodies]);
        booleanBodies(context, getSubfeatureId("booleanUnion"),
                      { "operationType" : BooleanOperationType.UNION, "tools" : toolsQ });
    }
    else
    {
        const draftDefinition = { "angle" : definition.draftAngle,
                                  "pullDirection" : definition.draftPullDirection };
        // TODO: replace this with the merged query enum
        const draftFaces = qOwnedByBody(makeQuery(id, "SWEPT_FACE", EntityType.FACE, {}), extrudeBody);
        applyDraft(context, getSubfeatureId("draft"), draftFaces, draftDefinition, neutralPlaneFace, neutralPlane);
    }

    opDeleteBodies(context, getSubfeatureId("deleteNeutralPlane"), { "entities" : qCreatedBy(neutralPlaneId) });
}

function getEntitiesToUse(context is Context, definition is map) returns Query
{
    if (definition.bodyType == ToolBodyType.SOLID)
    {
        return definition.entities;
    }
    else
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V177_CONSTRUCTION_OBJECT_FILTER))
        {
            return qConstructionFilter(definition.surfaceEntities, ConstructionObject.NO);
        }
        else
        {
            return definition.surfaceEntities;
        }
    }
}

function computeExtrudeAxis(context is Context, entity is Query, transform)
precondition
{
    transform is undefined || transform is Transform;
}
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V325_FEATURE_MIRROR))
    {
        var planes = evaluateQuery(context, qGeometry(entity, GeometryType.PLANE));
        var extrudeAxis;
        if (size(planes) == 1)
        {
            const entityPlane = evPlane(context, { "face" : entity });
            extrudeAxis = line(entityPlane.origin, entityPlane.normal);
            if (transform == undefined || transform == identityTransform())
                return extrudeAxis;
            else
                return transform * extrudeAxis;
        }
        else
        {
            //The extrude axis should start in the middle of the edge and point in the sketch plane normal
            const tangentAtEdge = evEdgeTangentLine(context, { "edge" : entity, "parameter" : 0.5 });
            const entityPlane = evOwnerSketchPlane(context, { "entity" : entity });
            var direction = entityPlane.normal;
            if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V565_COMPUTED_DATA_SKETCH_TRANSFORM))
            {
                // We don't transform sketch *planes* during pattern transformation, just entities. So this direction
                // does not represent the transforms, if any, on the stack.
                var fullTransform = getFullPatternTransform(context);
                if (fullTransform != identityTransform())
                    direction = fullTransform.linear * entityPlane.normal;
            }
            else if (transform != undefined && transform != identityTransform())
            {
                // More recently the sketch plane IS transformed so we don't need the full transform
                // but we still need to transform the sketch normal by the provided transform
                direction = transform.linear * entityPlane.normal;
            }
            //make sure to handle the origin and transform with remainder transform passed in
            if (transform == undefined || transform == identityTransform())
                return line(tangentAtEdge.origin, direction);
            else
                return line(transform * tangentAtEdge.origin, direction);
        }
    }
    else
    {
        return computeExtrudeAxisOld(context, entity, transform);
    }
}

function computeExtrudeAxisOld(context is Context, entity is Query, transform)
{
    var planes = evaluateQuery(context, qGeometry(entity, GeometryType.PLANE));
    var extrudeAxis;
    if (size(planes) == 1)
    {
        const entityPlane = evPlane(context, { "face" : entity });
        extrudeAxis = line(entityPlane.origin, entityPlane.normal);
    }
    else
    {
        //The extrude axis should start in the middle of the edge and point in the sketch plane normal
        const tangentAtEdge = evEdgeTangentLine(context, { "edge" : entity, "parameter" : 0.5 });
        const entityPlane = evOwnerSketchPlane(context, { "entity" : entity });
        extrudeAxis = line(tangentAtEdge.origin, entityPlane.normal);
    }
    if (transform != undefined && transform != identityTransform())
    {
        extrudeAxis = transform * extrudeAxis;
    }
    return extrudeAxis;
}

function upToBoundaryFlip(context is Context, definition is map) returns map
{
    const usedEntities = getEntitiesToUse(context, definition);
    const resolvedEntities = evaluateQuery(context, usedEntities);
    if (size(resolvedEntities) == 0)
    {
        return definition;
    }
    const extrudeAxis = computeExtrudeAxis(context, resolvedEntities[0], definition.transform);
    if (extrudeAxis == undefined)
    {
        return definition;
    }
    return extrudeUpToBoundaryFlipCommon(context, extrudeAxis, definition);
}

/**
 * @internal
 * The editing logic function used in the [extrude] feature.
 */
export function extrudeEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (definition.bodyType == ToolBodyType.SOLID)
    {
        const hasFlatEntities = size(evaluateQuery(context, qSMFlatFilter(definition.entities, SMFlatType.YES))) > 0;
        const hasModelEntities = size(evaluateQuery(context, qSMFlatFilter(definition.entities, SMFlatType.NO))) > 0;
        if (hasFlatEntities && !hasModelEntities)
        {
            definition.domain = OperationDomain.FLAT;
        }
        else if (hasModelEntities && !hasFlatEntities)
        {
            definition.domain = OperationDomain.MODEL;
        }
    }
    // If this function is changed, make sure to reflect the change in sheetMetalStart::sheetMetalStartEditLogic.

    var retestDirectionFlip = false;
    // If flip has not been specified and there is no second direction we can adjust flip either based on location of
    // bounding surface/body or based on boolean operation
    if (canSetExtrudeFlips(definition, specifiedParameters))
    {
        if (canSetExtrudeUpToFlip(definition, specifiedParameters))
        {
            definition = upToBoundaryFlip(context, definition);
        }
        else if (canSetBooleanFlip(oldDefinition, definition, specifiedParameters))
        {
            definition.oppositeDirection = !definition.oppositeDirection;
        }
        else
        {
            retestDirectionFlip = true;
        }
    }
    definition = setExtrudeSecondDirectionFlip(definition, specifiedParameters);

    var newDefinition =  booleanStepEditLogic(context, id, oldDefinition, definition,
                                specifiedParameters, hiddenBodies, extrude);
    // booleanStepEditLogic might change boolean operation type,
    // if flip was not adjusted above, re-test it
    if (retestDirectionFlip && canSetBooleanFlip(definition, newDefinition, specifiedParameters))
    {
        newDefinition.oppositeDirection = !newDefinition.oppositeDirection;
        newDefinition = setExtrudeSecondDirectionFlip(newDefinition, specifiedParameters);
    }

    if (definition.bodyType == ToolBodyType.SURFACE)
    {
        if (!specifiedParameters.surfaceOperationType)
        {
            if (definition.hasSecondDirection)
            {
                newDefinition.surfaceOperationType = NewSurfaceOperationType.NEW;
            }
            else
            {
                newDefinition =  surfaceOperationTypeEditLogic(context, id, newDefinition, specifiedParameters, definition.surfaceEntities, hiddenBodies);
            }
        }
    }
    return newDefinition;
}
// Initial data was collected for all model definition bodies, bodiesQ - bodies affected by operation
// combine initial data for those bodies only
function combineInitialData(context is Context, initialDataPerBody is array, bodiesQ is Query)
{
    var targets = {};
    for (var body in evaluateQuery(context, bodiesQ))
    {
        targets[body] = true;
    }
    var originalEntitiesArr = [];
    var initialAssociationAttributesArr = [];
    var originalEntitiesTrackingArr =[];
    for (var bodyData in initialDataPerBody)
    {
        for (var body in evaluateQuery(context, bodyData.query))
        {
            if (targets[body] == true)
            {
                originalEntitiesArr = append(originalEntitiesArr, bodyData.entitiesAndAttributes.originalEntities);
                initialAssociationAttributesArr = append(initialAssociationAttributesArr,
                                            bodyData.entitiesAndAttributes.initialAssociationAttributes);
                originalEntitiesTrackingArr = append(originalEntitiesTrackingArr, bodyData.entitiesAndAttributes.originalEntitiesTracking);
            }
        }
    }
    return {'originalEntities' : concatenateArrays(originalEntitiesArr),
            'initialAssociationAttributes' : concatenateArrays(initialAssociationAttributesArr),
            'originalEntitiesTracking' : concatenateArrays(originalEntitiesTrackingArr)};
}


