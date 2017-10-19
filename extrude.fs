FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/boundingtype.gen.fs", version : "✨");
export import(path : "onshape/std/query.fs", version : "✨");
export import(path : "onshape/std/tool.fs", version : "✨");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "✨");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "✨");
import(path : "onshape/std/booleanHeuristics.fs", version : "✨");
import(path : "onshape/std/box.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/coordSystem.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/draft.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/mathUtils.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/transform.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");

/**
 * Similar to `BoundingType`, but made for the second direction of an `extrude`.
 * Thus, `SYMMETRIC` is not an option.
 */
export enum SecondDirectionBoundingType
{
    annotation { "Name" : "Blind" }
    BLIND,
    annotation { "Name" : "Up to next" }
    UP_TO_NEXT,
    annotation { "Name" : "Up to face" }
    UP_TO_SURFACE,
    annotation { "Name" : "Up to part" }
    UP_TO_BODY,
    annotation { "Name" : "Up to vertex" }
    UP_TO_VERTEX,
    annotation { "Name" : "Through all" }
    THROUGH_ALL
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

        if (definition.bodyType != ToolBodyType.SURFACE)
        {
            annotation { "Name" : "Faces and sketch regions to extrude",
                         "Filter" : (EntityType.FACE && GeometryType.PLANE)
                            && ConstructionObject.NO }
            definition.entities is Query;
        }
        else
        {
            annotation { "Name" : "Sketch curves to extrude",
                         "Filter" : (EntityType.EDGE && SketchObject.YES && ModifiableEntityOnly.YES && ConstructionObject.NO ) }
            definition.surfaceEntities is Query;
        }

        annotation { "Name" : "End type" }
        definition.endBound is BoundingType;

        if (definition.endBound != BoundingType.SYMMETRIC)
        {
            annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
            definition.oppositeDirection is boolean;
        }

        if (definition.endBound == BoundingType.BLIND ||
            definition.endBound == BoundingType.SYMMETRIC)
        {
            annotation { "Name" : "Depth" }
            isLength(definition.depth, LENGTH_BOUNDS);
        }
        else if (definition.endBound == BoundingType.UP_TO_SURFACE)
        {
            annotation { "Name" : "Up to face",
                "Filter" : EntityType.FACE && SketchObject.NO,
                "MaxNumberOfPicks" : 1 }
            definition.endBoundEntityFace is Query;
        }
        else if (definition.endBound == BoundingType.UP_TO_BODY)
        {
            annotation { "Name" : "Up to surface or part",
                         "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO,
                         "MaxNumberOfPicks" : 1 }
            definition.endBoundEntityBody is Query;
        }
        else if (definition.endBound == BoundingType.UP_TO_VERTEX)
        {
            annotation {"Name" : "Up to vertex or mate connector",
                "Filter" : EntityType.VERTEX || BodyType.MATE_CONNECTOR,
                "MaxNumberOfPicks" : 1 }
            definition.endBoundEntityVertex is Query;
        }

        if (definition.endBound == BoundingType.UP_TO_NEXT ||
            definition.endBound == BoundingType.UP_TO_SURFACE ||
            definition.endBound == BoundingType.UP_TO_BODY ||
            definition.endBound == BoundingType.UP_TO_VERTEX)
        {
            annotation {"Name" : "Offset distance", "Column Name" : "Has offset", "UIHint" : "DISPLAY_SHORT" }
            definition.hasOffset is boolean;

            if (definition.hasOffset)
            {
                annotation {"Name" : "Offset distance", "UIHint" : "DISPLAY_SHORT" }
                isLength(definition.offsetDistance, LENGTH_BOUNDS);

                annotation {"Name" : "Opposite direction", "Column Name" : "Offset opposite direction", "UIHint" : "OPPOSITE_DIRECTION"}
                definition.offsetOppositeDirection is boolean;
            }
        }

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

                if (definition.secondDirectionBound == SecondDirectionBoundingType.BLIND)
                {
                    annotation { "Name" : "Depth", "Column Name" : "Second depth" }
                    isLength(definition.secondDirectionDepth, LENGTH_BOUNDS);
                }
                else if (definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_SURFACE)
                {
                    annotation { "Name" : "Up to face", "Column Name" : "Second up to face",
                        "Filter" : EntityType.FACE && SketchObject.NO,
                        "MaxNumberOfPicks" : 1 }
                    definition.secondDirectionBoundEntityFace is Query;
                }
                else if (definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_BODY)
                {
                    annotation { "Name" : "Up to surface or part", "Column Name" : "Second up to surface or part",
                                 "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO,
                                 "MaxNumberOfPicks" : 1 }
                    definition.secondDirectionBoundEntityBody is Query;
                }

                else if (definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_VERTEX)
                {
                    annotation { "Name" : "Up to vertex or mate connector", "Column Name" : "Second up to vertex or mate connector",
                        "Filter" : EntityType.VERTEX || BodyType.MATE_CONNECTOR,
                        "MaxNumberOfPicks" : 1 }
                    definition.secondDirectionBoundEntityVertex is Query;
                }

                if (definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_NEXT ||
                    definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_SURFACE ||
                    definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_BODY ||
                    definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_VERTEX)
                {
                    annotation {"Name" : "Offset distance", "Column Name" : "Second direction has offset", "UIHint" : "DISPLAY_SHORT" }
                    definition.hasSecondDirectionOffset is boolean;

                    if (definition.hasSecondDirectionOffset)
                    {
                        annotation {"Name" : "Offset distance", "Column Name" : "Second offset distance", "UIHint" : "DISPLAY_SHORT" }
                        isLength(definition.secondDirectionOffsetDistance, LENGTH_BOUNDS);

                        annotation {"Name" : "Opposite direction", "Column Name" : "Second offset opposite direction", "UIHint" : "OPPOSITE_DIRECTION"}
                        definition.secondDirectionOffsetOppositeDirection is boolean;
                    }
                }

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
    {
        // Handle negative inputs
        if (definition.endBound == BoundingType.BLIND && definition.depth < 0)
        {
            definition.depth *= -1;
            definition.oppositeDirection = !definition.oppositeDirection;
        }

        if (definition.hasSecondDirection && definition.secondDirectionBound == SecondDirectionBoundingType.BLIND && definition.secondDirectionDepth < 0)
        {
            definition.secondDirectionDepth *= -1;
            definition.secondDirectionOppositeDirection = !definition.secondDirectionOppositeDirection;
        }

        definition.entities = getEntitiesToUse(context, definition);

        // ------------- Handle pattern feature instance transform if needed ----------
        definition.transform = getRemainderPatternTransform(context, {"references" : definition.entities});

        // compute the draftCondition before definition gets changed.
        const draftCondition is map = getDraftConditions(definition);

        const resolvedEntities = evaluateQuery(context, definition.entities);
        if (size(resolvedEntities) == 0)
        {
            if (definition.bodyType == ToolBodyType.SOLID)
                throw regenError(ErrorStringEnum.EXTRUDE_NO_SELECTED_REGION, ["entities"]);
            else
                throw regenError(ErrorStringEnum.EXTRUDE_SURF_NO_CURVE, ["surfaceEntities"]);
        }

        // ------------- Get the extrude axis ---------------
        const extrudeAxis = try(computeExtrudeAxis(context, resolvedEntities[0], definition.transform));
        if (extrudeAxis == undefined)
            throw regenError(ErrorStringEnum.EXTRUDE_NO_DIRECTION);

        // Add manipulator
        addExtrudeManipulator(context, id, definition, extrudeAxis);

        // ------------- Determine the direction ---------------

        definition.direction = extrudeAxis.direction;

        if (definition.oppositeDirection)
            definition.direction *= -1;

        definition.isStartBoundOpposite = false;

        // ------------- Determine the bounds ---------------

        var vertexPlaneId = undefined;
        var secondVertexPlaneId = undefined;

        if (definition.endBound == BoundingType.UP_TO_SURFACE)
        {
            verifyNonemptyQuery(context, definition, "endBoundEntityFace", ErrorStringEnum.EXTRUDE_SELECT_TERMINATING_SURFACE);
            definition.endBoundEntity = definition.endBoundEntityFace;
        }
        else if (definition.endBound == BoundingType.UP_TO_BODY)
        {
            verifyNonemptyQuery(context, definition, "endBoundEntityBody", ErrorStringEnum.EXTRUDE_SELECT_TERMINATING_BODY);
            definition.endBoundEntity = definition.endBoundEntityBody;
        }
        else if (definition.endBound == BoundingType.UP_TO_VERTEX)
        {
            verifyNonemptyQuery(context, definition, "endBoundEntityVertex", ErrorStringEnum.EXTRUDE_SELECT_TERMINATING_VERTEX);
            vertexPlaneId = id + "vertexPlane";
            definition.endBoundEntity = createVertexBoundaryPlane(context, definition, vertexPlaneId);
            definition.endBound = BoundingType.UP_TO_SURFACE;
        }

        definition.endTranslationalOffset = 0;

        if (definition.hasOffset &&
           (definition.endBound == BoundingType.UP_TO_NEXT ||
            definition.endBound == BoundingType.UP_TO_SURFACE ||
            definition.endBound == BoundingType.UP_TO_BODY))
        {
            // @opExtrude takes endTranslationalOffset in terms of extrude direction, but positive, non-flipped offset
            // in extrude feature should pull away from the body.
            if (!definition.offsetOppositeDirection)
            {
                definition.offsetDistance *= -1;
            }
            definition.endTranslationalOffset = definition.offsetDistance;
        }

        definition.startBound = BoundingType.BLIND;
        definition.startDepth = 0;
        definition.startTranslationalOffset = 0;
        definition.endDepth = definition.depth;

        if (definition.endBound == BoundingType.SYMMETRIC)
        {
            definition.endBound = BoundingType.BLIND;
            definition.startDepth = definition.depth * -0.5;
            definition.endDepth = definition.depth * 0.5;
        }
        else if (definition.hasSecondDirection)
        {
            // ------------- Check the second direction ---------------

            if (definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_SURFACE)
            {
                verifyNonemptyQuery(context, definition, "secondDirectionBoundEntityFace", ErrorStringEnum.EXTRUDE_SELECT_TERMINATING_SURFACE);
                definition.secondDirectionBoundEntity = definition.secondDirectionBoundEntityFace;
            }
            else if (definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_BODY)
            {
                verifyNonemptyQuery(context, definition, "secondDirectionBoundEntityBody", ErrorStringEnum.EXTRUDE_SELECT_TERMINATING_BODY);
                definition.secondDirectionBoundEntity = definition.secondDirectionBoundEntityBody;
            }
            else if (definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_VERTEX)
            {
                verifyNonemptyQuery(context, definition, "secondDirectionBoundEntityVertex", ErrorStringEnum.EXTRUDE_SELECT_TERMINATING_VERTEX);
                secondVertexPlaneId = id + "secondVertexPlane";
                definition.secondDirectionBoundEntity = createVertexBoundaryPlane(context, definition, secondVertexPlaneId);
                definition.secondDirectionBound = SecondDirectionBoundingType.UP_TO_SURFACE;
            }

            definition.startBound = definition.secondDirectionBound as BoundingType;
            definition.startBoundEntity = definition.secondDirectionBoundEntity;

            definition.startDepth = definition.secondDirectionDepth;
            if (definition.secondDirectionOppositeDirection != definition.oppositeDirection)
                definition.isStartBoundOpposite = true;

            if (definition.hasSecondDirectionOffset &&
               (definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_NEXT ||
                definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_SURFACE ||
                definition.secondDirectionBound == SecondDirectionBoundingType.UP_TO_BODY))
            {
                if ((definition.secondDirectionOffsetOppositeDirection && !definition.isStartBoundOpposite) ||
                    (!definition.secondDirectionOffsetOppositeDirection && definition.isStartBoundOpposite))
                {
                    definition.secondDirectionOffsetDistance *= -1;
                }
                definition.startTranslationalOffset = definition.secondDirectionOffsetDistance;
            }
        }

        // ------------- Perform the operation ---------------

        extrudeWithDraft(context, id, definition, draftCondition);

        const reconstructOp = function(id) { extrudeWithDraft(context, id, definition, draftCondition); };

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            processNewBodyIfNeeded(context, id, definition, reconstructOp);
        }
        else if (definition.surfaceOperationType == NewSurfaceOperationType.ADD)
        {
            var matches = createTopologyMatchesForSurfaceJoin(context, id, definition, qCapEntity(id, true), definition.surfaceEntities, definition.transform);
            checkForNotJoinableSurfacesInScope(context, id, definition, matches);
            joinSurfaceBodies(context, id, matches, false, reconstructOp);
        }

        if (vertexPlaneId != undefined)
        {
            opDeleteBodies(context, id + "deleteVertexPlane", {
                "entities" : qCreatedBy(vertexPlaneId, EntityType.BODY)});
        }

        if (secondVertexPlaneId != undefined)
        {
            opDeleteBodies(context, id + "deleteSecondVertexPlane", {
                "entities" : qCreatedBy(secondVertexPlaneId, EntityType.BODY)});
        }

    }, { endBound : BoundingType.BLIND, oppositeDirection : false,
            bodyType : ToolBodyType.SOLID, operationType : NewBodyOperationType.NEW,
            secondDirectionBound : SecondDirectionBoundingType.BLIND,
            secondDirectionOppositeDirection : true, hasSecondDirection : false,
            hasOffset: false, hasSecondDirectionOffset: false,
            offsetOppositeDirection: false, secondDirectionOffsetOppositeDirection: false,
            hasDraft: false, hasSecondDirectionDraft: false,
            draftPullDirection : false, secondDirectionDraftPullDirection : false,
            surfaceOperationType : NewSurfaceOperationType.NEW,
            defaultSurfaceScope : true
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

function createVertexBoundaryPlane(context is Context, definition is map, id is Id)
{
    if (definition.endBound == BoundingType.UP_TO_VERTEX)
    {
        opPlane(context, id, {
            "plane" : plane(evVertexPoint(context, {
                "vertex" : definition.endBoundEntityVertex}), definition.direction)});
        return qCreatedBy(id, EntityType.FACE);
    }
    else // If this is called and the first endBound isn't UP_TO_VERTEX, then the second bound must be.
    {
        opPlane(context, id, {
            "plane" : plane(evVertexPoint(context, {
                "vertex" : definition.secondDirectionBoundEntityVertex}), definition.direction)});
        return qCreatedBy(id, EntityType.FACE);
    }
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

function getEntitiesToUse(context is Context, definition is map)
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

// Manipulator functions

const DEPTH_MANIPULATOR = "depthManipulator";
const FLIP_MANIPULATOR = "flipManipulator";
const SECOND_DEPTH_MANIPULATOR = "secondDirectionDepthManipulator";
const SECOND_FLIP_MANIPULATOR = "secondDirectionFlipManipulator";

function addExtrudeManipulator(context is Context, id is Id, definition is map, extrudeAxis is Line)
{
    const usedEntities = getEntitiesToUse(context, definition);

    if (definition.endBound != BoundingType.BLIND && definition.endBound != BoundingType.SYMMETRIC)
    {
        addManipulators(context, id, { (FLIP_MANIPULATOR) :
                        flipManipulator(extrudeAxis.origin,
                                        extrudeAxis.direction,
                                        definition.oppositeDirection,
                                        usedEntities) });
    }
    else
    {
        var depthOffset = definition.depth;
        if (definition.endBound == BoundingType.SYMMETRIC)
            depthOffset *= 0.5;
        if (definition.oppositeDirection)
            depthOffset *= -1;
        addManipulators(context, id, { (DEPTH_MANIPULATOR) :
                        linearManipulator(extrudeAxis.origin,
                            extrudeAxis.direction,
                            depthOffset,
                            usedEntities) });
    }

    if (definition.hasSecondDirection && definition.endBound != BoundingType.SYMMETRIC)
    {
        if (definition.secondDirectionBound != SecondDirectionBoundingType.BLIND)
        {
            addManipulators(context, id, { (SECOND_FLIP_MANIPULATOR) :
                            flipManipulator(extrudeAxis.origin,
                                            extrudeAxis.direction,
                                            definition.secondDirectionOppositeDirection,
                                            usedEntities,
                                            ManipulatorStyleEnum.SECONDARY) });
        }
        else
        {
            var secondDirectionDepthOffset = definition.secondDirectionDepth;
            if (definition.secondDirectionOppositeDirection == true)
                secondDirectionDepthOffset *= -1;
            addManipulators(context, id, { (SECOND_DEPTH_MANIPULATOR) :
                            linearManipulator(extrudeAxis.origin,
                                              extrudeAxis.direction,
                                              secondDirectionDepthOffset,
                                              usedEntities,
                                              ManipulatorStyleEnum.SECONDARY) });
        }
    }
}

/**
 * @internal
 * The manipulator change function used in the `extrude` feature.
 */
export function extrudeManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    if (newManipulators[DEPTH_MANIPULATOR] is map &&
        (definition.endBound == BoundingType.BLIND ||
         definition.endBound == BoundingType.SYMMETRIC))
    {
        var newDepthOffset = newManipulators[DEPTH_MANIPULATOR].offset;
        if (definition.endBound == BoundingType.SYMMETRIC)
            newDepthOffset *= 2;
        definition.oppositeDirection = newDepthOffset < 0;
        definition.depth = abs(newDepthOffset);
    }
    else if (newManipulators[FLIP_MANIPULATOR] is map &&
             definition.endBound != BoundingType.BLIND &&
             definition.endBound != BoundingType.SYMMETRIC)
    {
        definition.oppositeDirection = newManipulators[FLIP_MANIPULATOR].flipped;
    }

    if (definition.hasSecondDirection && definition.endBound != BoundingType.SYMMETRIC)
    {
        if (newManipulators[SECOND_DEPTH_MANIPULATOR] is map &&
            definition.secondDirectionBound == SecondDirectionBoundingType.BLIND)
        {
            const newSecondDirectionDepthOffset = newManipulators[SECOND_DEPTH_MANIPULATOR].offset;
            definition.secondDirectionOppositeDirection = newSecondDirectionDepthOffset < 0;
            definition.secondDirectionDepth = abs(newSecondDirectionDepthOffset);
        }
        else if (newManipulators[SECOND_FLIP_MANIPULATOR] is map &&
            definition.secondDirectionBound != SecondDirectionBoundingType.BLIND)
        {
            definition.secondDirectionOppositeDirection = newManipulators[SECOND_FLIP_MANIPULATOR].flipped;
        }
    }

    return definition;
}

function shouldFlipExtrudeDirection(context is Context, endBound is BoundingType,
    endBoundEntity is Query,
    extrudeAxis is Line)
{
    if (endBound != BoundingType.UP_TO_SURFACE &&
        endBound != BoundingType.UP_TO_BODY &&
        endBound != BoundingType.UP_TO_VERTEX)
        return false;

    if (endBound == BoundingType.UP_TO_SURFACE)
    {
        const refPlane = try(evPlane(context, { "face" : endBoundEntity }));
        if (refPlane == undefined)
            return false; //err on side of not flipping, TODO: surfaceXline
        const isecResult = intersection(refPlane, extrudeAxis);
        if (isecResult == undefined || isecResult.dim != 0)
            return false;

        const dotProduct = stripUnits(dot(isecResult.intersection - extrudeAxis.origin, extrudeAxis.direction));
        return dotProduct < -TOLERANCE.zeroLength;
    }
    if (endBound == BoundingType.UP_TO_VERTEX)
    {
        const targetVertex = try(evVertexPoint(context, { "vertex" : endBoundEntity}));
        if (targetVertex == undefined)
            return false;
        const dotProduct = stripUnits(dot(targetVertex - extrudeAxis.origin, extrudeAxis.direction));
        return dotProduct < -TOLERANCE.zeroLength;
    }
    const pln = plane(extrudeAxis.origin, extrudeAxis.direction);
    const boxResult = try(evBox3d(context, { 'topology' : endBoundEntity, 'cSys' : planeToCSys(pln), 'tight' : false }));
    if (boxResult == undefined)
        return false;

    return (stripUnits(boxResult.minCorner[2]) < -TOLERANCE.zeroLength &&
            stripUnits(boxResult.maxCorner[2]) < TOLERANCE.zeroLength);
}

function upToBoundaryFlip(context is Context, featureDefinition is map) returns map
{
    const usedEntities = getEntitiesToUse(context, featureDefinition);
    const resolvedEntities = evaluateQuery(context, usedEntities);
    if (@size(resolvedEntities) == 0)
    {
        return featureDefinition;
    }
    const extrudeAxis = computeExtrudeAxis(context, resolvedEntities[0], featureDefinition.transform);
    if (extrudeAxis == undefined)
    {
        return featureDefinition;
    }
    var direction = extrudeAxis.direction;
    if (featureDefinition.oppositeDirection == true)
        direction *= -1;
    var endBoundEntity;
    if (featureDefinition.endBound == BoundingType.UP_TO_SURFACE)
    {
        endBoundEntity = featureDefinition.endBoundEntityFace;
    }
    else if (featureDefinition.endBound == BoundingType.UP_TO_BODY)
    {
        endBoundEntity = featureDefinition.endBoundEntityBody;
    }
    else if (featureDefinition.endBound == BoundingType.UP_TO_VERTEX)
    {
        endBoundEntity = featureDefinition.endBoundEntityVertex;
    }
    if (endBoundEntity is Query &&
        shouldFlipExtrudeDirection(context,
                                   featureDefinition.endBound,
                                   endBoundEntity,
                                   line(extrudeAxis.origin, direction)))
    {
        featureDefinition.oppositeDirection = (featureDefinition.oppositeDirection == true) ? false : true;
    }
    return featureDefinition;
}

function canSetUpToFlip(definition is map, specifiedParameters is map) returns boolean
{
    if (definition.endBound == BoundingType.UP_TO_SURFACE)
    {
        return specifiedParameters.endBoundEntityFace;
    }
    if (definition.endBound == BoundingType.UP_TO_BODY)
    {
        return specifiedParameters.endBoundEntityBody;
    }
    if (definition.endBound == BoundingType.UP_TO_VERTEX)
    {
        return specifiedParameters.endBoundEntityVertex;
    }
    return false;
}

/**
 * @internal
 * The editing logic function used in the [extrude] feature.
 */
export function extrudeEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    var retestDirectionFlip = false;
    // If flip has not been specified and there is no second direction we can adjust flip either based on location of
    // bounding surface/body or based on boolean operation
    if (!definition.hasSecondDirection &&
        definition.endBound != BoundingType.SYMMETRIC &&
        !specifiedParameters.oppositeDirection)
    {
        if (canSetUpToFlip(definition, specifiedParameters))
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
    if (canSetSecondDirectionFlip(definition, specifiedParameters))
    {
        definition.secondDirectionOppositeDirection = !definition.oppositeDirection;
    }
    var newDefinition =  booleanStepEditLogic(context, id, oldDefinition, definition,
                                specifiedParameters, hiddenBodies, extrude);
    // booleanStepEditLogic might change boolean operation type,
    // if flip was not adjusted above, re-test it
    if (retestDirectionFlip && canSetBooleanFlip(definition, newDefinition, specifiedParameters))
    {
        newDefinition.oppositeDirection = !newDefinition.oppositeDirection;
        if (canSetSecondDirectionFlip(newDefinition, specifiedParameters))
        {
            newDefinition.secondDirectionOppositeDirection = !newDefinition.oppositeDirection;
        }
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

function canSetSecondDirectionFlip(definition is map, specifiedParameters is map) returns boolean
{
    if (specifiedParameters.secondDirectionOppositeDirection is undefined ||
        specifiedParameters.secondDirectionOppositeDirection ||
        !definition.hasSecondDirection)
    {
        return false;
    }

    return (definition.secondDirectionOppositeDirection == definition.oppositeDirection);
}

