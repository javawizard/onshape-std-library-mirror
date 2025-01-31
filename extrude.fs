FeatureScript 2581; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/extrudeCommon.fs", version : "2581.0");
export import(path : "onshape/std/flatOperationType.fs", version : "2581.0");
export import(path : "onshape/std/query.fs", version : "2581.0");
export import(path : "onshape/std/tool.fs", version : "2581.0");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "2581.0");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "2581.0");
import(path : "onshape/std/boolean.fs", version : "2581.0");
import(path : "onshape/std/booleanHeuristics.fs", version : "2581.0");
import(path : "onshape/std/box.fs", version : "2581.0");
import(path : "onshape/std/containers.fs", version : "2581.0");
import(path : "onshape/std/coordSystem.fs", version : "2581.0");
import(path : "onshape/std/curveGeometry.fs", version : "2581.0");
import(path : "onshape/std/drafttype.gen.fs", version : "2581.0");
import(path : "onshape/std/evaluate.fs", version : "2581.0");
import(path : "onshape/std/feature.fs", version : "2581.0");
import(path : "onshape/std/mathUtils.fs", version : "2581.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2581.0");
import(path : "onshape/std/sheetMetalBuiltIns.fs", version : "2581.0");
import(path : "onshape/std/sheetMetalInFlat.fs", version : "2581.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2581.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2581.0");
import(path : "onshape/std/topologyUtils.fs", version : "2581.0");
import(path : "onshape/std/transform.fs", version : "2581.0");
import(path : "onshape/std/valueBounds.fs", version : "2581.0");

//imports for Thin wall extrusion
import(path : "onshape/std/path.fs", version : "2581.0");
import(path : "onshape/std/string.fs", version : "2581.0");

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
 * Create an extrude, as used in Onshape's extrude feature.
 *
 * Internally, performs an [opExtrude], followed by an [opBoolean], possibly followed by a
 * [opDraft], possibly in two directions. If creating a simple extrusion, prefer using
 * [opExtrude] alone.
 *
 * @param id : @autocomplete `id + "extrude1"`
 * @param definition {{
 *      @field bodyType {ExtendedToolBodyType}: @optional
 *              Specifies a `SOLID` or `SURFACE` or `THIN` extrude. Default is `SOLID`.
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
 *      @field depth {ValueWithUnits}: @requiredif {`endBound` is `BLIND`}
 *              A length specifying the extrude depth. For a blind extrude,
 *              specifies the depth of the first extrude direction.
 *              For a symmetric extrude, specifies the full extrude depth.
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
 *      @field secondDirectionBound {BoundingType}: @optional
 *              The bounding type of the second direction. Can be different from the bounding type of the first direction.
 *      @field secondDirectionOppositeDirection {boolean} : @optional
 *              @ex `true` will flip the second end direction to align opposite the plane/face's normal.
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
 *
 *      @field wallShape {Query} : @requiredif {`bodyType` is `THIN`}
 *              The specified planar face or sketch edges defining the thin wall shape.
 *      @field thickness1 {ValueWithUnits} : @requiredif {`bodyType` is `THIN`}
 *              The outwards thickness of the thin wall.
 *      @field thickness2 {ValueWithUnits} : @requiredif {`bodyType` is `THIN`}
 *              The inwards thickness of the thin wall.
 *      @field flipWall {boolean} : @optional
 *              @ex `true` to apply the first offset value instead of the second and vice versa
 * }}
 */
annotation { "Feature Type Name" : "Extrude",
             "Manipulator Change Function" : "extrudeManipulatorChange",
             "Filter Selector" : "allparts",
             "Editing Logic Function" : "extrudeEditLogic" }
export const extrude = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "UIHint" : UIHint.ALWAYS_HIDDEN, "Default" : OperationDomain.MODEL }
        definition.domain is OperationDomain;

        if (definition.domain != OperationDomain.FLAT)
        {
            annotation { "Name" : "Creation type", "UIHint" : [UIHint.HORIZONTAL_ENUM, UIHint.REMEMBER_PREVIOUS_VALUE] }
            definition.bodyType is ExtendedToolBodyType;

            if (definition.bodyType != ExtendedToolBodyType.SURFACE)
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
            annotation { "Name" : "Creation type", "UIHint" : UIHint.HORIZONTAL_ENUM, "Default" : FlatOperationType.REMOVE }
            definition.flatOperationType is FlatOperationType;
        }

        if (definition.bodyType != ExtendedToolBodyType.SURFACE || definition.domain == OperationDomain.FLAT)
        {
            if (definition.bodyType != ExtendedToolBodyType.THIN)
            {
            annotation { "Name" : "Faces and sketch regions to extrude",
                     "Filter" : ((AllowFlattenedGeometry.YES && SketchObject.YES && EntityType.FACE) ||
                      (GeometryType.PLANE && AllowFlattenedGeometry.NO && EntityType.FACE)) && ConstructionObject.NO
                      }
            definition.entities is Query;
            }
        }
        else
        {
            if (definition.bodyType != ExtendedToolBodyType.THIN)
            {
                {
                annotation { "Name" : "Sketch curves to extrude",
                         "Filter" : (EntityType.EDGE && SketchObject.YES && ModifiableEntityOnly.YES && ConstructionObject.NO ) }
                definition.surfaceEntities is Query;
                }
            }
        }

        if (definition.bodyType == ExtendedToolBodyType.THIN)
        {
            annotation { "Name" : "Faces and sketch regions to extrude",
            "Filter" : (EntityType.FACE && GeometryType.PLANE && ConstructionObject.NO && ModifiableEntityOnly.NO) ||
                       (EntityType.EDGE && SketchObject.YES && ConstructionObject.NO && ModifiableEntityOnly.YES) }
            definition.wallShape is Query;

            annotation { "Name" : "Mid plane", "Default" : false}
            definition.midplane is boolean;

            if (!definition.midplane)
            {
                annotation { "Name" : "Thickness 1" }
                isLength(definition.thickness1, ZERO_INCLUSIVE_OFFSET_BOUNDS);

                annotation { "Name" : "Flip wall", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                definition.flipWall is boolean;

                annotation { "Name" : "Thickness 2" }
                isLength(definition.thickness2, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
            }
            else
            {
                annotation { "Name" : "Thickness" }
                isLength(definition.thickness, ZERO_INCLUSIVE_OFFSET_BOUNDS);
            }
        }

        if (definition.domain != OperationDomain.FLAT)
        {
            mainViewExtrudePredicate(definition);
        }
    }
    {
        if (definition.bodyType == ExtendedToolBodyType.SOLID)
        {
            const modelEntities = qSheetMetalFlatFilter(definition.entities, SMFlatType.NO);
            const containsModelEntities = (!isQueryEmpty(context, modelEntities));
            const containsFlattened = queryContainsFlattenedSheetMetal(context, definition.entities);
            if (definition.domain == OperationDomain.FLAT)
            {
                if (containsModelEntities)
                {
                    const errorString = (containsFlattened) ? ErrorStringEnum.EXTRUDE_3D_AND_FLAT :
                                                                        ErrorStringEnum.DEFINED_IN_SM_FLAT_CANT_REFERENCE_3D;
                    throw regenError(errorString, ["entities"], modelEntities);
                }
                callSubfeatureAndProcessStatus(id, SMFlatOp, context, id + "flatOp", {
                            "faces" : definition.entities,
                            "flatOperationType" : definition.flatOperationType
                        });
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
        if (resolvedEntities == [])
        {
            if (definition.bodyType == ExtendedToolBodyType.SOLID)
                throw regenError(ErrorStringEnum.EXTRUDE_NO_SELECTED_REGION, ["entities"]);
            else
                throw regenError(ErrorStringEnum.EXTRUDE_SURF_NO_CURVE, ["surfaceEntities"]);
        }

        // Handle pattern feature instance transform if needed
        definition.transform = getRemainderPatternTransform(context, {"references" : entities});

        //Thin wall feature should not be patterned if the source face is modifying by the feature during rebuild - in case of boolean interaction.
        //In fact rebuild is possible but provides the wrong result. So it should be failed.
        if (isInFeaturePattern(context) && definition.bodyType == ExtendedToolBodyType.THIN)
        {
            const selectedFaces = evaluateQuery(context, qEntityFilter(definition.wallShape, EntityType.FACE));
            for (var singleFace in selectedFaces)
            {
                if (lastModifyingOperationId(context, singleFace)[0] == id[size(id) - 1]) //This face has been modified by the seed feature of the pattern
                {
                    throw regenError(ErrorStringEnum.EXTRUDE_INVALID_REF_FACE);
                }
            }
        }

        // Compute the draftCondition before definition gets changed
        var draftCondition is map = getDraftConditions(definition);

        // Get the plane normal defined by the first profile.
        const normalAndAxis = getPlaneNormalAndExtrudeAxis(context, definition, resolvedEntities[0], definition.transform);
        var planeNormal = normalAndAxis.planeNormal;
        const extrudeAxis = normalAndAxis.extrudeAxis;

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2102_EXTRUDE_START_OFFSET_ENT_FIX))
        {
            const normalAndAxisNoTransform = getPlaneNormalAndExtrudeAxis(context, definition, resolvedEntities[0], identityTransform());
            definition = processStartOffsetData(context, definition, normalAndAxisNoTransform.extrudeAxis, normalAndAxisNoTransform.planeNormal, extrudeAxis);
        }
        else
        {
            definition = processStartOffsetData(context, definition, extrudeAxis, planeNormal, extrudeAxis);
        }

        // Add manipulator
        // We need to transform the extrude axis origin to take the offset into account.)
        var originWithStartOffset = extrudeAxis.origin;
        if (definition.startOffset)
        {
            originWithStartOffset = definition.transform * originWithStartOffset;
        }
        addExtrudeManipulator(context, id, definition, entities, line(originWithStartOffset, extrudeAxis.direction), true);

        // Transform the definition
        definition = transformExtrudeDefinitionForOpExtrude(context, id, entities, extrudeAxis.direction, definition, planeNormal);

        // The rest of the code needs the plane normal to have the correct orientation
        if (isOppositeDirection(definition))
        {
            planeNormal *= -1;
        }
        // We need to pass the original plane normal for the draft operation
        draftCondition.planeNormal = planeNormal;

        var reconstructOp = undefined;

        if (definition.bodyType == ExtendedToolBodyType.THIN)
        {
            //Create thin wall geometry
            thinWallWithDraft(context, id, definition, planeNormal, draftCondition);
            reconstructOp = function(id){thinWallWithDraft(context, id, definition, planeNormal, draftCondition); };
        }
        else
        {
            // Perform ordinary solid extrude operation
            extrudeWithDraft(context, id, definition, draftCondition);
            reconstructOp = function(id) { extrudeWithDraft(context, id, definition, draftCondition); };
        }

        if (definition.bodyType == ExtendedToolBodyType.SOLID || definition.bodyType == ExtendedToolBodyType.THIN)
        {
            processNewBodyIfNeeded(context, id, definition, reconstructOp);
        }
        else if (definition.surfaceOperationType == NewSurfaceOperationType.ADD)
        {
           if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1197_DETECT_SURFACE_JOIN_CPP))
           {
                joinSurfaceBodiesWithAutoMatching(context, id, definition, false, reconstructOp);
           }
           else
           {
                var matches = createTopologyMatchesForSurfaceJoin(context, id, definition, qCapEntity(id, CapType.START), definition.surfaceEntities, definition.transform);
                checkForNotJoinableSurfacesInScope(context, id, definition, matches);
                joinSurfaceBodies(context, id, matches, false, reconstructOp);
           }
        }

        // This cleanup should be done after boolean processing.
        // Otherwise reconstuctOp would fail in case of extrude up-to-vertex
        cleanupTemporaryBoundaryPlanes(context, id, definition);


    }, { endBound : BoundingType.BLIND, oppositeDirection : false, symmetric : false,
            bodyType : ExtendedToolBodyType.SOLID, operationType : NewBodyOperationType.NEW,
            secondDirectionBound : BoundingType.BLIND,
            secondDirectionOppositeDirection : true, hasSecondDirection : false,
            hasOffset: false, hasSecondDirectionOffset: false,
            offsetOppositeDirection: false, secondDirectionOffsetOppositeDirection: false,
            hasDraft: false, hasSecondDirectionDraft: false,
            draftPullDirection : false, secondDirectionDraftPullDirection : false,
            surfaceOperationType : NewSurfaceOperationType.NEW,
            defaultSurfaceScope : true,
            domain : OperationDomain.MODEL,
            flatOperationType : FlatOperationType.REMOVE,
            startOffset : false,
            hasExtrudeDirection : false,
            flipWall : false,
            midplane : false,
            thickness : 0.25 * inch
    });

predicate supportsDraft(definition is map)
{
    definition.bodyType == ExtendedToolBodyType.SOLID || definition.bodyType == ExtendedToolBodyType.THIN;
}

predicate firstDirectionNeedsDraft(definition is map)
{
    supportsDraft(definition);
    definition.hasDraft;
}

predicate symmetricNeedsDraft(definition is map)
{
    firstDirectionNeedsDraft(definition);
    isSymmetricExtrude(definition);
}

predicate hasSecondDirectionExtrude(definition is map)
{
    !isSymmetricExtrude(definition);
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
    isSymmetricExtrude(definition) || (hasSecondDirectionExtrude(definition) && definition.secondDirectionOppositeDirection != definition.oppositeDirection);
}

predicate mainViewExtrudePredicate(definition is map)
{
    annotation { "Name" : "End type" }
    definition.endBound is BoundingType;

    annotation { "Name" : "Opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
    definition.oppositeDirection is boolean;

    extrudeBoundParametersPredicate(definition);

    if (definition.bodyType != ExtendedToolBodyType.THIN)
    {
        extrudeDirectionPredicate(definition);
    }

    extrudeOffsetPredicate(definition);

    if (definition.endBound == BoundingType.BLIND || definition.endBound == BoundingType.THROUGH_ALL)
    {
        annotation { "Name" : "Symmetric" }
        definition.symmetric is boolean;
    }

    if (definition.bodyType == ExtendedToolBodyType.SOLID || definition.bodyType == ExtendedToolBodyType.THIN)
    {
        annotation { "Name" : "Draft", "UIHint" : [ "DISPLAY_SHORT", "FIRST_IN_ROW" ] }
        definition.hasDraft is boolean;

        if (definition.hasDraft == true)
        {
            annotation { "Name" : "Draft angle", "UIHint" : UIHint.DISPLAY_SHORT }
            isAngle(definition.draftAngle, ANGLE_STRICT_90_BOUNDS);

            annotation { "Name" : "Opposite direction", "Column Name" : "Draft opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION_CIRCULAR }
            definition.draftPullDirection is boolean;
        }
    }

    if (!isSymmetricExtrude(definition))
    {
        annotation { "Name" : "Second end position",
                     "UIHint" : UIHint.FIRST_IN_ROW }
        definition.hasSecondDirection is boolean;

        annotation { "Group Name" : "Second end position", "Driving Parameter" : "hasSecondDirection", "Collapsed By Default" : false }
        {
            if (definition.hasSecondDirection)
            {
                annotation { "Name" : "End type", "Column Name" : "Second end type" }
                definition.secondDirectionBound is BoundingType;

                annotation { "Name" : "Opposite direction", "Column Name" : "Second opposite direction",
                            "UIHint" : UIHint.OPPOSITE_DIRECTION, "Default" : true }
                definition.secondDirectionOppositeDirection is boolean;

                extrudeSecondDirectionBoundParametersPredicate(definition);

                if ((definition.bodyType == ExtendedToolBodyType.SOLID || definition.bodyType == ExtendedToolBodyType.THIN) &&
                    ((definition.secondDirectionOppositeDirection && !definition.oppositeDirection) ||
                    (!definition.secondDirectionOppositeDirection && definition.oppositeDirection)))
                {
                    annotation { "Name" : "Draft", "Column Name" : "Second draft", "UIHint" : [ "DISPLAY_SHORT", "FIRST_IN_ROW" ] }
                    definition.hasSecondDirectionDraft is boolean;

                    if (definition.hasSecondDirectionDraft)
                    {
                        annotation { "Name" : "Draft angle", "Column Name" : "Second draft angle", "UIHint" : UIHint.DISPLAY_SHORT }
                        isAngle(definition.secondDirectionDraftAngle, ANGLE_STRICT_90_BOUNDS);

                        annotation { "Name" : "Opposite direction", "Column Name" : "Second draft opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION_CIRCULAR }
                        definition.secondDirectionDraftPullDirection is boolean;
                    }
                }
            }
        }
    }
    if (definition.bodyType != ExtendedToolBodyType.SURFACE)
    {
        booleanStepScopePredicate(definition);
    }
    else
    {
        surfaceJoinStepScopePredicate(definition);
    }
}


function extrudeWithDraft(context is Context, id is Id, definition is map, draftCondition is map)
{
    opExtrude(context, id, definition);

    if (draftCondition.firstDirectionNeedsDraft || draftCondition.secondDirectionNeedsDraft)
    {
        const extrudeBodies = qSubtraction(qCreatedBy(id, EntityType.BODY), qUnion([
            qCreatedBy(id + "vertexPlane", EntityType.BODY),
            qCreatedBy(id + "secondVertexPlane", EntityType.BODY),
            qBodyType(qCreatedBy(id, EntityType.BODY), BodyType.SHEET)]));

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
                    draftDefinition is map, referenceFace is Query, neutralPlane is Plane,
                    pullVector is Vector)
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2082_EXTRUDE_NO_DRAFT_IF_ZERO_ANGLE) && draftDefinition.angle == 0 * degree)
    {
        return;
    }
    draftDefinition.draftType = DraftType.NEUTRAL_PLANE;
    draftDefinition.tangentPropagation = false;
    draftDefinition.reFillet = false;
    draftDefinition.draftFaces = draftFaces;
    draftDefinition.neutralPlane = referenceFace;
    draftDefinition.pullVec = pullVector;
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
    try silent
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
    const neutralPlane = try(createNeutralPlane(context, neutralPlaneId, extrudeBody, conditions.planeNormal, definition.transform));

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
            applyDraft(context, draftFirstId, firstFaces, draftFirstDefinition, neutralPlaneFace, neutralPlane, definition.direction);
        if (draftSecondDefinition != undefined)
            applyDraft(context, draftSecondId, secondFaces, draftSecondDefinition, neutralPlaneFace, neutralPlane, definition.direction);

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
        applyDraft(context, getSubfeatureId("draft"), draftFaces, draftDefinition, neutralPlaneFace, neutralPlane, definition.direction);
    }

    opDeleteBodies(context, getSubfeatureId("deleteNeutralPlane"), { "entities" : qCreatedBy(neutralPlaneId) });
}

function getEntitiesToUse(context is Context, definition is map) returns Query
{
    if (definition.bodyType == ExtendedToolBodyType.SOLID)
    {
        verifyNoMesh(context, definition, "entities");
        return definition.entities;
    }
    else
    {
        if (definition.bodyType == ExtendedToolBodyType.THIN)
        {
            verifyNonemptyQuery(context, definition, "wallShape", ErrorStringEnum.OFFSET_WIRE_SELECT_WALL_PATH);

            //replace every selected face with the adjecent edges.
            var faceEntities = qEntityFilter(definition.wallShape, EntityType.FACE);
            if (!isQueryEmpty(context, faceEntities))
            {
                var onlyEdges = qSubtraction(definition.wallShape, faceEntities);
                var extactedEdges = qAdjacent(faceEntities, AdjacencyType.EDGE, EntityType.EDGE);
                return qConstructionFilter(qUnion([extactedEdges, onlyEdges]), ConstructionObject.NO);
            }
            return qConstructionFilter(definition.wallShape, ConstructionObject.NO);
        }
        else
        {
            verifyNoMesh(context, definition, "surfaceEntities");
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
}

function computeProfilePlaneNormal(context is Context, definition is map, entity is Query, transform)
precondition
{
    transform is undefined || transform is Transform;
}
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V325_FEATURE_MIRROR))
    {
        const planeSource = definition.bodyType == ExtendedToolBodyType.THIN ? definition.wallShape : entity;
        const planes = evaluateQuery(context, qGeometry(planeSource , GeometryType.PLANE));

        var extrudeAxis;
        if (size(planes) >= 1)
        {
            const entityPlane = evPlane(context, { "face" : planes[0] });
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
    const profilePlaneNormal = computeProfilePlaneNormal(context, definition, resolvedEntities[0], definition.transform);
    if (profilePlaneNormal == undefined)
    {
        return definition;
    }
    return extrudeUpToBoundaryFlipCommon(context, profilePlaneNormal, definition);
}

/**
 * @internal
 * The editing logic function used in the [extrude] feature.
 */
export function extrudeEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (definition.bodyType == ExtendedToolBodyType.SOLID)
    {
        const hasFlatEntities = !isQueryEmpty(context, qSheetMetalFlatFilter(definition.entities, SMFlatType.YES));
        const hasModelEntities = !isQueryEmpty(context, qSheetMetalFlatFilter(definition.entities, SMFlatType.NO));
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

    if (definition.bodyType == ExtendedToolBodyType.SOLID || definition.bodyType == ExtendedToolBodyType.THIN)
    {
        var newDefinition = definition;
        if (retestDirectionFlip)
        {
            // Always retest forward to stabilize the case where the forward and backward test both result in a flip to
            // each other.  Example: BEL-106989
            newDefinition.oppositeDirection = false;
            newDefinition = setExtrudeSecondDirectionFlip(newDefinition, specifiedParameters);
        }

        newDefinition = booleanStepEditLogic(context, id, oldDefinition, newDefinition, specifiedParameters, hiddenBodies, extrude);

        // booleanStepEditLogic might change boolean operation type,
        // if flip was not adjusted above, re-test it
        if (retestDirectionFlip)
        {
            if (canSetBooleanFlip(definition, newDefinition, specifiedParameters))
            {
                newDefinition.oppositeDirection = true;
            }
            else
            {
                newDefinition.oppositeDirection = definition.oppositeDirection;
            }
            newDefinition = setExtrudeSecondDirectionFlip(newDefinition, specifiedParameters);
        }

        definition = newDefinition;
    }
    else if (definition.bodyType == ExtendedToolBodyType.SURFACE)
    {
        if (!specifiedParameters.surfaceOperationType)
        {
            if (definition.hasSecondDirection)
            {
                definition.surfaceOperationType = NewSurfaceOperationType.NEW;
            }
            else
            {
                definition =  surfaceOperationTypeEditLogic(context, id, definition, specifiedParameters, definition.surfaceEntities, hiddenBodies);
            }
        }
    }
    return definition;
}

//======================THIN WALL FUNCTIONS===============================
function thinWallWithDraft(context is Context, id is Id, definition is map, direction is Vector, draftCondition is map)
{
    //Use common direction for all shapes
    definition.commonDirection = direction;

    //Create thin wall region shapes
    const extrusionFaceSet = getThinWallRegions(context, id, definition);

    //Perform extrusion only if all region shapes been created successfully
    if (!isQueryEmpty(context, extrusionFaceSet))
    {
        //EXTRUSION performing HERE
        definition.entities = extrusionFaceSet;
        draftCondition.planeNormal = direction;
        extrudeWithDraft(context, id, definition, draftCondition);

        //Delete planar shapes
        opDeleteBodies(context, id + "deleteThinWallShapes", {
                    "entities" : extrusionFaceSet
                });
    }
}

// Start Offset code

predicate extrudeOffsetPredicate(definition is map)
{
    annotation { "Name" : "Starting offset" }
    definition.startOffset is boolean;
    if (definition.startOffset)
    {
        annotation { "Group Name" : "Starting offset", "Driving Parameter" : "startOffset", "Collapsed By Default" : false }
        {
            annotation { "Name" : "Starting offset bound" }
            definition.startOffsetBound is StartOffsetType;
            if (definition.startOffsetBound == StartOffsetType.BLIND)
            {
                annotation { "Name" : "Depth", "Column Name" : "Starting offset depth" }
                isLength(definition.startOffsetDistance, LENGTH_BOUNDS);
                annotation { "Name" : "Opposite direction", "Column Name" : "Starting offset opposite direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
                definition.startOffsetOppositeDirection is boolean;
            }
            else
            {
                annotation { "Name" : "Entity",
                            "Filter" : (GeometryType.PLANE && EntityType.FACE) || EntityType.EDGE || EntityType.VERTEX || BodyType.MATE_CONNECTOR,
                            "MaxNumberOfPicks" : 1, "Column Name" : "Starting offset entity" }
                definition.startOffsetEntity is Query;
            }
        }
    }
}

// returns { "planeNormal" is Vector, "extrudeAxis" is Line }
function getPlaneNormalAndExtrudeAxis(context is Context, definition is map, entity is Query, transform is Transform) returns map
{
        const profilePlaneNormal = try(computeProfilePlaneNormal(context, definition, entity, transform));
        if (profilePlaneNormal == undefined)
        {
            throw regenError(ErrorStringEnum.EXTRUDE_NO_DIRECTION);
        }
        // We still want to keep the origin of the extrude axis even if a direction has been provided by the user
        var planeNormal = profilePlaneNormal.direction;
        const extrudeAxis = line(profilePlaneNormal.origin, processExtrudeDirection(context, definition, planeNormal));
        return { "planeNormal" : planeNormal, "extrudeAxis" : extrudeAxis };
}

function checkPlaneParallel(context is Context, entity is Query, planeNormal is Vector)
{
    // If we have an entity that is an edge or a face, it needs to be planar
    const edge = qEntityFilter(entity, EntityType.EDGE);
    const face = qEntityFilter(entity, EntityType.FACE);
    var plane;
    if (!isQueryEmpty(context, edge))
    {
        try silent
        {
            plane = evPlanarEdge(context, { "edge" : edge});
        }
    }
    else if (!isQueryEmpty(context, face))
    {
        try silent
        {
            plane = evPlane(context, { "face" : face});
        }
    }
    else
    {
        // We have a vertex or plane connector, we don't need to check for a plane
        return;
    }
    if (plane == undefined)
    {
        throw regenError(ErrorStringEnum.EXTRUDE_START_OFFSET_BOUND_NOT_PLANAR, ["startOffsetEntity"], entity);
    }
    // If we have a planar entity, its plane needs to be parallel to the extruded profiles
    if (!parallelVectors(plane.normal, planeNormal))
    {
        throw regenError(ErrorStringEnum.EXTRUDE_START_OFFSET_BOUND_NOT_PARALLEL_TO_EXTRUDED_ENTITIES, ["startOffsetEntity"], entity);
    }
}

function checkLineParallel(context is Context, edge is Query, planeNormal is Vector) returns boolean
{
    // If the entity is a line, it needs to be normal to the extruded profiles plane normal
    var line;
    try silent
    {
        line = evLine(context, { "edge" : edge });
    }
    if (line == undefined)
    {
        // The entity is not a line
        return false;
    }
    if (!tolerantEquals(dot(line.direction, planeNormal), 0))
    {
        throw regenError(ErrorStringEnum.EXTRUDE_START_OFFSET_BOUND_NOT_PARALLEL_TO_EXTRUDED_ENTITIES, ["startOffsetEntity"], edge);
    }
    // The entity is a line and it's a valid offset entity
    return true;
}

function checkStartOffsetEntity(context is Context, definition is map, planeNormal is Vector)
{
    if (!definition.startOffset || definition.startOffsetBound == StartOffsetType.BLIND)
    {
        return;
    }
    // We need an entity to offset up to.
    if (isQueryEmpty(context, definition.startOffsetEntity))
    {
        throw regenError(ErrorStringEnum.EXTRUDE_SELECT_START_OFFSET_ENTITY, ["startOffsetEntity"]);
    }

    if (checkLineParallel(context, definition.startOffsetEntity, planeNormal))
    {
        return;
    }
    checkPlaneParallel(context, definition.startOffsetEntity, planeNormal);
}

function processStartOffsetData(context is Context, definition is map, extrudeAxis is Line, planeNormal is Vector, extrudeAxisWithTransform is Line) returns map
{
    if (!definition.startOffset)
    {
        return definition;
    }
    checkStartOffsetEntity(context, definition, planeNormal);
    // the direction should already be normalized
    var distance;
    if (definition.startOffsetBound == StartOffsetType.BLIND)
    {
        distance = definition.startOffsetDistance;
        if (definition.startOffsetOppositeDirection)
        {
            distance = -distance;
        }
    }
    else
    {
        // extrudeAxis's origin is located at the first entity.
        // We take the minimum distance between the start offset entity and the first entity
        // in the direction of the extrude.
        var distanceResult = evDistance(context, {
                "side0" : extrudeAxis.origin,
                "side1" : definition.startOffsetEntity
        });
        var distanceVector = distanceResult.sides[1].point - extrudeAxis.origin;
        distance = dot(distanceVector, planeNormal) / dot(extrudeAxis.direction, planeNormal);
    }
    definition.distanceForManipulator = distance;
    const offsetTransform = transform(extrudeAxisWithTransform.direction * distance);
    if (definition.transform == undefined)
    {
        definition.transform = offsetTransform;
    }
    else if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1975_FIX_EXTRUDE_OFFSET_MIRROR))
    {
        definition.transform = offsetTransform * definition.transform;
    }
    else
    {
        definition.transform = definition.transform * offsetTransform;
    }
    return definition;
}

// Extrude direction

predicate extrudeDirectionPredicate(definition is map)
{
    annotation { "Name" : "Direction" }
    definition.hasExtrudeDirection is boolean;

    annotation { "Group Name" : "Direction", "Driving Parameter" : "hasExtrudeDirection", "Collapsed By Default" : false }
    {
        if (definition.hasExtrudeDirection)
        {
            annotation { "Name" : "Extrude direction", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION || BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
            definition.extrudeDirection is Query;
        }
    }
}

function processExtrudeDirection(context is Context, definition is map, planeNormal  is Vector) returns Vector
{
    if (!definition.hasExtrudeDirection || definition.bodyType == ExtendedToolBodyType.THIN)
    {
        return planeNormal;
    }
    if (isQueryEmpty(context, definition.extrudeDirection))
    {
        throw regenError(ErrorStringEnum.EXTRUDE_SELECT_DIRECTION, ["extrudeDirection"]);
    }
    const userProvidedExtrudeDirection = extractDirection(context, definition.extrudeDirection);
    if (userProvidedExtrudeDirection == undefined)
    {
        throw regenError(ErrorStringEnum.EXTRUDE_DIRECTION_INVALID_ENTITY, ["extrudeDirection"], definition.extrudeDirection);
    }
    const dotProduct = dot(userProvidedExtrudeDirection, planeNormal);
    if (tolerantEquals(dotProduct, 0))
    {
        throw regenError(ErrorStringEnum.EXTRUDE_DIRECTION_COPLANAR, ["extrudeDirection"], definition.extrudeDirection);
    }
    // Makes sure the direction picked by the user aligns with the original extrude direction to avoid flips
    if (dotProduct < 0)
    {
        return -userProvidedExtrudeDirection;
    }
    else
    {
        return userProvidedExtrudeDirection;
    }
}

