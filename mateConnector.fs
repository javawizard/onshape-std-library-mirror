FeatureScript 2599; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2599.0");
export import(path : "onshape/std/entityinferencetype.gen.fs", version : "2599.0");
export import(path : "onshape/std/mateconnectoraxistype.gen.fs", version : "2599.0");
export import(path : "onshape/std/origincreationtype.gen.fs", version : "2599.0");
export import(path : "onshape/std/rotationtype.gen.fs", version : "2599.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "2599.0");
import(path : "onshape/std/evaluate.fs", version : "2599.0");
import(path : "onshape/std/feature.fs", version : "2599.0");
import(path : "onshape/std/tool.fs", version : "2599.0");
import(path : "onshape/std/valueBounds.fs", version : "2599.0");
import(path : "onshape/std/string.fs", version : "2599.0");

/**
 * @internal
 * A `RealBoundSpec` for the x, y, z of the normal.
 */
export const NORMAL_PARAMETER_BOUNDS =
{
    (unitless) : [-1.0, 0, 1]
} as RealBoundSpec;

/**
 * Feature performing an [opMateConnector].
 *
 * The parameters below are designed for interactive use in the feature dialog. In FeatureScript, it is preferred to
 * calculate the resulting coordinate system directly, and pass this coordinate system to `opMateConnector`.
 *
 * @param id : @autocomplete `id + "mateConnector1"`
 * @param definition {{
 *      @field originQuery {Query} : The entity on which to place the mate connector
 *          @ex `qFarthestAlong(qCreatedBy(id + "extrude1", EntityType.VERTEX), vector(1, 1, 1))`
 *      @field entityInferenceType {EntityInferenceType} : A method of producing the coordinate system
 *          @eg `EntityInferenceType.POINT` to place on a vertex, with the world coordinate system
 *          @ex `EntityInferenceType.MID_POINT` to place at the midpoint of an edge, with the Z axis along the edge
 *      @field secondaryOriginQuery {query} : @optional Additional entity to inference with, used in some inference types.
 *      @field originType {OriginCreationType} : @optional @ex `OriginCreationType.BETWEEN_ENTITIES` to place mate connector
 *          origin on the midpoint between the speicified origin and the location of `originAdditionalQuery`. Default
 *          is `OriginCreationType.ON_ENTITY`.
 *      @field originAdditionalQuery {Query} : @requiredIf {`originType` is `OriginCreationType.BETWEEN_ENTITIES`}
 *      @field flipPrimary {boolean} : @optional @ex `true` to flip the resulting Z axis
 *      @field secondaryAxisType {MateConnectorAxisType} : @optional Changes which axis (perpendicular to Z) will point along the
 *          secondary direction. Default is `MateConnectorAxisType.PLUS_X`
 *      @field realign {boolean} : @optional `true` to change direction of axes
 *      @field primaryAxisQuery {Query} : @requiredIf {`realign` is `true`} Entity with axis to define the resulting Z direction
 *      @field secondaryAxisQuery {Query} : @requiredIf {`realign` is `true`} Entity with axis to define the secondary axis (set by `secondaryAxisType`)
 *      @field transform {boolean} : @optional Whether to change the origin position with `translationX`, `translationY`, and `translationZ`.
 *          The X/Y/Z directions of this translation are affected by `primaryAxisQuery` and `secondaryAxisQuery` and `secondaryAxisType`, but not by
 *          `rotationType` and `rotation`
 *      @field translationX {ValueWithUnits} : @requiredIf {`transform` is `true`} Distance to move the resulting origin along resulting X direction.
 *      @field translationY {ValueWithUnits} : @requiredIf {`transform` is `true`} Distance to move the resulting origin along resulting Y direction
 *      @field translationZ {ValueWithUnits} : @requiredIf {`transform` is `true`} Distance to move the resulting origin along resulting Z direction
 *      @field rotationType {RotationType} : @optional Axis to rotate around (does not change origin position)
 *      @field rotation {ValueWithUnits} : @optional Angle to rotate
 *      @field requireOwnerPart {boolean} : @optional Whether to error if owner part is not provided. Default is `true`. If `false`, the mate connector
 *          may be an independent body with no owner part.
 *      @field ownerPart {Query} : @requiredIf {`requireOwnerPart` is not `false`} Part on which to attach the resulting mate connector
 *          @autocomplete `ownerPart`
 *      @field specifyNormal {boolean} : @optional @ex `true` override the Z direction with the known vector `(definition.nx, definition.ny, definition.nz)`.
 *          Used internally when the mate connector is placed on a mesh.
 *      @field nx {number} : @requiredIf {`specifyNormal` is `true`}
 *      @field ny {number} : @requiredIf {`specifyNormal` is `true`}
 *      @field nz {number} : @requiredIf {`specifyNormal` is `true`}
 * }}
 */
annotation { "Feature Type Name" : "Mate connector", "UIHint" : UIHint.CONTROL_VISIBILITY , "Editing Logic Function" : "connectorEditLogic" }
export const mateConnector = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Origin type" }
        definition.originType is OriginCreationType;

        annotation { "Name" : "Origin entity",
                     "Filter" : ((EntityType.EDGE || EntityType.VERTEX) || (EntityType.FACE && ConstructionObject.NO)) && ModifiableEntityOnly.YES && AllowMeshGeometry.YES,
                     "MaxNumberOfPicks" : 1,
                     "UIHint" : UIHint.UNCONFIGURABLE }
        definition.originQuery is Query;

        annotation { "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.entityInferenceType is EntityInferenceType;

        annotation { "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.secondaryOriginQuery is Query;

        if (definition.originType == OriginCreationType.BETWEEN_ENTITIES)
        {
            annotation { "Name" : "Between entity", "Filter" : EntityType.FACE && AllowMeshGeometry.YES, "MaxNumberOfPicks" : 1,  "UIHint" : UIHint.UNCONFIGURABLE }
            definition.originAdditionalQuery is Query;
        }

        annotation { "Name" : "Realign" }
        definition.realign is boolean;

        if (definition.realign)
        {
            annotation { "Name" : "Primary axis entity",
                         "Filter" : EntityType.FACE || EntityType.EDGE,
                         "MaxNumberOfPicks" : 1 }
            definition.primaryAxisQuery is Query;

            annotation { "Name" : "Secondary axis entity",
                         "Filter" : EntityType.FACE || EntityType.EDGE,
                         "MaxNumberOfPicks" : 1 }
            definition.secondaryAxisQuery is Query;
        }

        annotation { "Name" : "Move" }
        definition.transform is boolean;

        if (definition.transform)
        {
            annotation { "Name" : "X translation" }
            isLength(definition.translationX, ZERO_DEFAULT_LENGTH_BOUNDS);

            annotation { "Name" : "Y translation" }
            isLength(definition.translationY, ZERO_DEFAULT_LENGTH_BOUNDS);

            annotation { "Name" : "Z translation" }
            isLength(definition.translationZ, ZERO_DEFAULT_LENGTH_BOUNDS);

            annotation { "Name" : "Rotation axis", "Default" : RotationType.ABOUT_Z }
            definition.rotationType is RotationType;

            annotation { "Name" : "Rotation angle" }
            isAngle(definition.rotation, ANGLE_360_ZERO_DEFAULT_BOUNDS);
        }

        annotation { "Name" : "Allow owner entity", "Default" : true, "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.allowOwnerEntity is boolean;

        if (definition.allowOwnerEntity)
        {
            annotation { "Name" : "Owner entity", "Default" : true }
            definition.requireOwnerPart is boolean;

            if (definition.requireOwnerPart)
            {
                // The mate connector owner part should be the one in the part list, thus it should be modifiable
                annotation { "Name" : "Select owner entity", "Filter" : EntityType.BODY && (BodyType.SOLID || GeometryType.MESH || BodyType.SHEET || BodyType.WIRE || BodyType.COMPOSITE)
                 && AllowMeshGeometry.YES && ModifiableEntityOnly.YES, "MaxNumberOfPicks" : 1 }
                definition.ownerPart is Query;
            }
        }

        annotation { "Name" : "Specify normal", "UIHint" : UIHint.ALWAYS_HIDDEN }
        definition.specifyNormal is boolean;

        if (definition.specifyNormal)
        {
            annotation { "Name" : "Normal x", "UIHint" : UIHint.ALWAYS_HIDDEN }
            isReal(definition.nx, NORMAL_PARAMETER_BOUNDS);
            annotation { "Name" : "Normal y", "UIHint" : UIHint.ALWAYS_HIDDEN }
            isReal(definition.ny, NORMAL_PARAMETER_BOUNDS);
            annotation { "Name" : "Normal z", "UIHint" : UIHint.ALWAYS_HIDDEN }
            isReal(definition.nz, NORMAL_PARAMETER_BOUNDS);
        }

        annotation { "Name" : "Flip primary axis", "UIHint" : [ "OPPOSITE_DIRECTION", "FIRST_IN_ROW" ] }
        definition.flipPrimary is boolean;

        annotation { "Name" : "Reorient secondary axis", "UIHint" : UIHint.MATE_CONNECTOR_AXIS_TYPE, "Default" : MateConnectorAxisType.PLUS_X }
        definition.secondaryAxisType is MateConnectorAxisType;

        annotation { "UIHint" : UIHint.ALWAYS_HIDDEN, "Default" : false }
        definition.isForSubFeature is boolean;
    }
    {
        if (definition.isForSubFeature && isInFeaturePattern(context))
        {
            // Mate conectors subfeatures do not behave as expected in many feature patterns, and, being subfeatures, they cannot be easily
            // excluded from these patterns. A better default behavior is to always treat them as static (non-patterned) by not recreating them here.
            // Eventually, getRemainderPatternTransform should track and consider the subFeature's dependencies when determining the references
            // of the parent feature.
            return;
        }

        definition.rotation = adjustAngle(context, definition.rotation);

        var transformQueries = [definition.originQuery];
        if (definition.originType == OriginCreationType.BETWEEN_ENTITIES)
            transformQueries = append(transformQueries, definition.originAdditionalQuery);
        if (definition.realign)
            transformQueries = concatenateArrays([transformQueries, [definition.primaryAxisQuery, definition.secondaryAxisQuery, definition.ownerPart]]);
        var remainingTransform = getRemainderPatternTransform(context,
            {"references" : qUnion(transformQueries)});
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V858_SM_FLAT_BUG_FIXES))
        {
            verifyNoSheetMetalFlatQuery(context, qUnion(transformQueries), "", ErrorStringEnum.FLATTENED_SHEET_METAL_SKETCH_PROHIBTED);
        }

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2390_MATE_CONNECTOR_NORMAL_TO_CURVED_FACE))
        {
            definition.remainingTransform = remainingTransform;
        }
        const mateConnectorCoordSystem = evMateConnectorCoordSystem(context, definition);

        if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V285_CONNECTOR_OWNER_EDIT_LOGIC))
        {
            var onlyPartInStudio = qNothing();
            const allBodies = qEverything(EntityType.BODY);
            const allParts = qBodyType(allBodies, BodyType.SOLID);

            if (size(evaluateQuery(context, allParts)) == 1)
            {
                onlyPartInStudio = allParts;
            }

            const possibleBodyOwners = [definition.ownerPart,
                                    definition.originQuery,
                                    definition.originAdditionalQuery,
                                    definition.primaryAxisQuery,
                                    definition.secondaryAxisQuery,
                                    onlyPartInStudio];

            definition.ownerPart = findOwnerBody(context, definition, possibleBodyOwners);
        }

        if (definition.requireOwnerPart)
        {
            verifyNonemptyQuery(context, definition, "ownerPart", ErrorStringEnum.MATECONNECTOR_OWNER_PART_NOT_RESOLVED);
        }
        else
        {
            definition.ownerPart = qNothing();
        }

        opMateConnector(context, id, { "owner" : definition.ownerPart, "coordSystem" : mateConnectorCoordSystem });
        if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V2390_MATE_CONNECTOR_NORMAL_TO_CURVED_FACE))
        {
            transformResultIfNecessary(context, id, remainingTransform);
        }
    }, {
        "originType" : OriginCreationType.ON_ENTITY,
        "originAdditionalQuery" : qNothing(),
        "secondaryOriginQuery" : qNothing(),
        "flipPrimary" : false,
        "secondaryAxisType" : MateConnectorAxisType.PLUS_X,
        "realign" : false,
        "primaryAxisQuery" : qNothing(),
        "secondaryAxisQuery" : qNothing(),
        "transform" : false,
        "translationX" : 0 * meter,
        "translationY" : 0 * meter,
        "translationZ" : 0 * meter,
        "rotationType" : RotationType.ABOUT_Z,
        "rotation" : 0 * radian,
        "allowOwnerEntity" : true,
        "requireOwnerPart" : true,
        "ownerPart" : qNothing(),
        "specifyNormal" : false,
        "nx" : 0.0,
        "ny" : 0.0,
        "nz" : 0.0,
        "isForSubFeature" : false
    });

/** @internal */
export function connectorEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
   specifiedParameters is map) returns map
{
    //only called on create so no need to version
    if (specifiedParameters.ownerPart != true ||
       (oldDefinition.originQuery != definition.originQuery && isQueryEmpty(context, definition.ownerPart)))
    {
        var possibleBodyOwners = [  definition.originQuery,
                                    definition.originAdditionalQuery,
                                    definition.primaryAxisQuery,
                                    definition.secondaryAxisQuery];
        // If there are no selections, reset owner part, don't try to recompute
        if (isQueryEmpty(context, qUnion(possibleBodyOwners)))
        {
            definition.ownerPart = qUnion([]);
            return definition;
        }
        // If there's a single part or surface in the studio, consider it as an owner.
        const allParts = qBodyType(qEverything(EntityType.BODY), BodyType.SOLID);
        const allSurfaces = qModifiableEntityFilter(qConstructionFilter(qSketchFilter(qBodyType(qEverything(EntityType.BODY), BodyType.SHEET), SketchObject.NO), ConstructionObject.NO));
        const allCurves = qModifiableEntityFilter(qConstructionFilter(qSketchFilter(qBodyType(qEverything(EntityType.BODY), BodyType.WIRE), SketchObject.NO), ConstructionObject.NO));
        const allPartsSurfacesCurves = qUnion([allParts, allSurfaces, allCurves]);
        if (size(evaluateQuery(context, allPartsSurfacesCurves)) == 1)
            possibleBodyOwners = append(possibleBodyOwners, allPartsSurfacesCurves);

        var ownerBodyQuery = findOwnerBody(context, definition, possibleBodyOwners);

        if (ownerBodyQuery != undefined && !isQueryEmpty(context, ownerBodyQuery))
            definition.ownerPart = qUnion(evaluateQuery(context, ownerBodyQuery));
        else
            definition.ownerPart = qUnion([]);
    }
    return definition;
}

function findOwnerBody(context is Context, definition is map, possibleBodyOwners is array)
{
    var ownerBodyQuery;
    for (var possibleBodyOwner in possibleBodyOwners)
    {
        const meshQuery = qSourceMesh(possibleBodyOwner, EntityType.BODY);
        if (evaluateQuery(context, meshQuery) != [])
        {
            ownerBodyQuery = meshQuery;
            break;
        }

        const solidQuery = qBodyType(qOwnerBody(possibleBodyOwner), BodyType.SOLID);
        if (!isQueryEmpty(context, solidQuery))
        {
            ownerBodyQuery = solidQuery;
            break;
        }

        const wireQuery = qModifiableEntityFilter(qConstructionFilter(qSketchFilter(qBodyType(qOwnerBody(possibleBodyOwner), BodyType.WIRE), SketchObject.NO), ConstructionObject.NO));
        if (!isQueryEmpty(context, wireQuery))
        {
            ownerBodyQuery = wireQuery;
            break;
        }

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2253_MATE_CONNECTOR))
        {
            const sketchQuery = qModifiableEntityFilter(qConstructionFilter(qSketchFilter(qOwnerBody(possibleBodyOwner), SketchObject.YES), ConstructionObject.NO));
            if (!isQueryEmpty(context, sketchQuery))
            {
                ownerBodyQuery = sketchQuery;
                break;
            }
        }
        // After V285, findOwnerBody is only called when creating a new feature (so no versioning is required).
        // However, before V285, findOwnerBody is called on rebuild.
        // As a result, sheets need to be excluded from possible owners before V285.
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V285_CONNECTOR_OWNER_EDIT_LOGIC))
        {
            const modifiableSurfaceQuery = qModifiableEntityFilter(qSketchFilter(qBodyType(qOwnerBody(possibleBodyOwner), BodyType.SHEET), SketchObject.NO));
            if (!isQueryEmpty(context, modifiableSurfaceQuery))
            {
                ownerBodyQuery = modifiableSurfaceQuery;
                break;
            }
        }
    }
    return ownerBodyQuery;
}

