FeatureScript 1447; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "1447.0");
export import(path : "onshape/std/tool.fs", version : "1447.0");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "1447.0");
import(path : "onshape/std/booleanHeuristics.fs", version : "1447.0");
import(path : "onshape/std/containers.fs", version : "1447.0");
import(path : "onshape/std/evaluate.fs", version : "1447.0");
import(path : "onshape/std/topologyUtils.fs", version : "1447.0");
import(path : "onshape/std/transform.fs", version : "1447.0");
import(path : "onshape/std/feature.fs", version : "1447.0");

/**
 * Feature performing an [opSweep], followed by an [opBoolean]. For simple sweeps, prefer using
 * [opSweep] directly.
 */
annotation { "Feature Type Name" : "Sweep",
             "Filter Selector" : "allparts",
             "Editing Logic Function" : "sweepEditLogic" }
export const sweep = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Creation type", "UIHint" : UIHint.HORIZONTAL_ENUM }
        definition.bodyType is ToolBodyType;

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            booleanStepTypePredicate(definition);

            annotation { "Name" : "Faces and sketch regions to sweep",
                         "Filter" : (EntityType.FACE && GeometryType.PLANE) && ConstructionObject.NO }
            definition.profiles is Query;
        }
        else
        {
            surfaceOperationTypePredicate(definition);

            annotation { "Name" : "Edges and sketch curves to sweep",
                         "Filter" : (EntityType.EDGE && ConstructionObject.NO) || (EntityType.BODY && BodyType.WIRE)}
            definition.surfaceProfiles is Query;
        }

        annotation { "Name" : "Sweep path", "Filter" : (EntityType.EDGE && ConstructionObject.NO)  || (EntityType.BODY && BodyType.WIRE) }
        definition.path is Query;

        annotation { "Name" : "Keep profile orientation" }
        definition.keepProfileOrientation is boolean;

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            booleanStepScopePredicate(definition);
        }
        else
        {
            surfaceJoinStepScopePredicate(definition);
        }
    }
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V203_SWEEP_PATH_NO_CONSTRUCTION))
        {
            const pathQuery = definition.path;
            definition.path = qConstructionFilter(definition.path, ConstructionObject.NO);
            if (pathQuery.queryType == QueryType.UNION && size(pathQuery.subqueries) > 0)
            {
                verifyNonemptyQuery(context, definition, "path", ErrorStringEnum.SWEEP_PATH_NO_CONSTRUCTION);
            }
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V576_GET_WIRE_LAMINAR_DEPENDENCIES))
            {
                definition.path = followWireEdgesToLaminarSource(context, definition.path);
            }
        }
        if (definition.bodyType == ToolBodyType.SURFACE)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V177_CONSTRUCTION_OBJECT_FILTER))
            {
                definition.profiles = qConstructionFilter(definition.surfaceProfiles, ConstructionObject.NO);
            }
            else
            {
                definition.profiles = definition.surfaceProfiles;
            }
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V576_GET_WIRE_LAMINAR_DEPENDENCIES))
            {
                definition.profiles = followWireEdgesToLaminarSource(context, definition.profiles);
            }
        }

        var remainingTransform = getRemainderPatternTransform(context,
                {"references" : qUnion([definition.profiles, definition.path])});

        opSweep(context, id, definition);
        transformResultIfNecessary(context, id, remainingTransform);

        const reconstructOp = function(id)
        {
            opSweep(context, id, definition);
            transformResultIfNecessary(context, id, remainingTransform);
        };
        if (definition.bodyType == ToolBodyType.SOLID)
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
                var matches = createMatchesForSurfaceJoin(context, id, definition, remainingTransform);
                joinSurfaceBodies(context, id, matches, false, reconstructOp);
            }
        }
    }, { bodyType : ToolBodyType.SOLID, operationType : NewBodyOperationType.NEW, keepProfileOrientation : false, surfaceOperationType : NewSurfaceOperationType.NEW, defaultSurfaceScope : true });


/**
 * @internal
 * Editing logic function for sweep feature.
 */
export function sweepEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    if (definition.bodyType == ToolBodyType.SOLID)
    {
        return booleanStepEditLogic(context, id, oldDefinition, definition,
                                    specifiedParameters, hiddenBodies, sweep);
    }
    else
    {
        return surfaceOperationTypeEditLogic(context, id, definition, specifiedParameters, definition.surfaceProfiles, hiddenBodies);
    }
}

function createMatchesForSurfaceJoin(context is Context, id is Id, definition is map, transform is Transform) returns array
{
    var matches = [];
    if (definition.bodyType == ToolBodyType.SURFACE && definition.surfaceOperationType == NewSurfaceOperationType.ADD)
    {
        var capMatches = createTopologyMatchesForSurfaceJoin(context, id, definition, qCapEntity(id, CapType.EITHER), definition.profiles, transform);
        var sweptMatches = createTopologyMatchesForSurfaceJoin(context, id, definition, makeQuery(id, "SWEPT_EDGE", EntityType.EDGE, {}), definition.path, transform);
        matches = concatenateArrays([capMatches, sweptMatches]);
        checkForNotJoinableSurfacesInScope(context, id, definition, matches);
    }
    return matches;
}

