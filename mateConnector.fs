FeatureScript 236; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/entityinferencetype.gen.fs", version : "");
export import(path : "onshape/std/mateconnectoraxistype.gen.fs", version : "");
export import(path : "onshape/std/origincreationtype.gen.fs", version : "");
export import(path : "onshape/std/rotationtype.gen.fs", version : "");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "");
import(path : "onshape/std/evaluate.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/tool.fs", version : "");
import(path : "onshape/std/valueBounds.fs", version : "");

// IB: are all the undefined comparisons necessary in the precondition?  Can they be turned into defaults?
/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
annotation { "Feature Type Name" : "Mate connector", "UIHint" : "CONTROL_VISIBILITY" }
export const mateConnector = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        if (definition.originType != undefined)
        {
            annotation { "Name" : "Origin type" }
            definition.originType is OriginCreationType;
        }

        annotation { "Name" : "Origin entity",
                     "Filter" : (EntityType.EDGE || EntityType.VERTEX) || (EntityType.FACE && ConstructionObject.NO),
                     "MaxNumberOfPicks" : 1 }
        definition.originQuery is Query;

        annotation { "UIHint" : "ALWAYS_HIDDEN" }
        definition.entityInferenceType is EntityInferenceType;

        annotation { "UIHint" : "ALWAYS_HIDDEN" }
        definition.secondaryOriginQuery is Query;

        if (definition.originType == OriginCreationType.BETWEEN_ENTITIES)
        {
            if (definition.originAdditionalQuery != undefined)
            {
                annotation { "Name" : "Between entity", "Filter" : EntityType.FACE, "MaxNumberOfPicks" : 1 }
                definition.originAdditionalQuery is Query;
            }
        }

        if (definition.flipPrimary != undefined)
        {
            annotation { "Name" : "Flip primary axis", "UIHint" : "ALWAYS_HIDDEN" }
            definition.flipPrimary is boolean;
        }

        if (definition.secondaryAxisType != undefined)
        {
            annotation { "Name" : "Secondary axis type", "UIHint" : "ALWAYS_HIDDEN", "Default" : MateConnectorAxisType.PLUS_X }
            definition.secondaryAxisType is MateConnectorAxisType;
        }

        if (definition.realign != undefined)
        {
            annotation { "Name" : "Realign" }
            definition.realign is boolean;
        }

        if (definition.realign == true)
        {
            if (definition.primaryAxisQuery != undefined)
            {
                annotation { "Name" : "Primary axis entity",
                             "Filter" : EntityType.FACE || EntityType.EDGE,
                             "MaxNumberOfPicks" : 1 }
                definition.primaryAxisQuery is Query;
            }

            if (definition.secondaryAxisQuery != undefined)
            {
                annotation { "Name" : "Secondary axis entity",
                             "Filter" : EntityType.FACE || EntityType.EDGE,
                             "MaxNumberOfPicks" : 1 }
                definition.secondaryAxisQuery is Query;
            }
        }

        if (definition.transform != undefined)
        {
            annotation { "Name" : "Move" }
            definition.transform is boolean;
        }

        if (definition.transform == true)
        {
            if (definition.translationX != undefined)
            {
                annotation { "Name" : "X translation" }
                isLength(definition.translationX, ZERO_DEFAULT_LENGTH_BOUNDS);
            }

            if (definition.translationY != undefined)
            {
                annotation { "Name" : "Y translation" }
                isLength(definition.translationY, ZERO_DEFAULT_LENGTH_BOUNDS);
            }

            if (definition.translationZ != undefined)
            {
                annotation { "Name" : "Z translation" }
                isLength(definition.translationZ, ZERO_DEFAULT_LENGTH_BOUNDS);
            }

            if (definition.rotationType != undefined)
            {
                annotation { "Name" : "Rotation axis", "Default" : RotationType.ABOUT_Z }
                definition.rotationType is RotationType;
            }

            if (definition.rotation != undefined)
            {
                annotation { "Name" : "Rotation angle" }
                isAngle(definition.rotation, ANGLE_360_ZERO_DEFAULT_BOUNDS);
            }
        }

        if (definition.ownerPart != undefined)
        {
            annotation { "Name" : "Owner part", "Filter" : EntityType.BODY && BodyType.SOLID, "MaxNumberOfPicks" : 1 }
            definition.ownerPart is Query;
        }
    }
    {
        const mateConnectorTransform = evMateConnectorTransform(context, definition);

        var onlyPartInStudio = qNothing();
        const allBodies = qEverything(EntityType.BODY);
        const allParts = qBodyType(allBodies, BodyType.SOLID);

        if (@size(evaluateQuery(context, allParts)) == 1)
        {
            onlyPartInStudio = allParts;
        }

        const possiblePartOwners = [definition.ownerPart,
                                    definition.originQuery,
                                    definition.originAdditionalQuery,
                                    definition.primaryAxisQuery,
                                    definition.secondaryAxisQuery,
                                    onlyPartInStudio];

        var ownerPartQuery;
        for (var i = 0; i < size(possiblePartOwners); i += 1)
        {
            const currentQuery = qBodyType(qOwnerPart(possiblePartOwners[i]), BodyType.SOLID);
            if (size(evaluateQuery(context, currentQuery)) != 0)
            {
                ownerPartQuery = currentQuery;
                break;
            }
        }

        if (ownerPartQuery == undefined)
            throw regenError(ErrorStringEnum.MATECONNECTOR_OWNER_PART_NOT_RESOLVED, ["ownerPart"]);

        opMateConnector(context, id, { "owner" : ownerPartQuery, "transform" : mateConnectorTransform });
    });

