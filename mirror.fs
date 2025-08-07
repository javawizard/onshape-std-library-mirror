FeatureScript 2737; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2737.0");
export import(path : "onshape/std/tool.fs", version : "2737.0");
export import(path : "onshape/std/patternUtils.fs", version : "2737.0");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "2737.0");
import(path : "onshape/std/booleanHeuristics.fs", version : "2737.0");
import(path : "onshape/std/containers.fs", version : "2737.0");
import(path : "onshape/std/evaluate.fs", version : "2737.0");
import(path : "onshape/std/feature.fs", version : "2737.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2737.0");
import(path : "onshape/std/transform.fs", version : "2737.0");
import(path : "onshape/std/recordpatterntype.gen.fs", version : "2737.0");


/**
 * Feature creating a single copy of some features, bodies, or faces, mirrored
 * about a given entity. Internally, performs an [applyPattern], which in turn
 * performs an [opPattern] or, for a feature mirror, calls the feature
 * function.
 */
annotation { "Feature Type Name" : "Mirror",
             "Filter Selector" : "allparts",
             "Editing Logic Function" : "mirrorEditLogic" }
export const mirror = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Mirror type" }
        definition.patternType is MirrorType;

        if (definition.patternType == MirrorType.PART)
        {
            booleanStepTypePredicate(definition);

            annotation { "Name" : "Entities to mirror", "Filter" : EntityType.BODY && AllowMeshGeometry.YES && SketchObject.NO }
            definition.entities is Query;
        }
        else if (definition.patternType == MirrorType.FACE)
        {
            annotation { "Name" : "Faces to mirror",
                         "UIHint" : ["ALLOW_FEATURE_SELECTION", "SHOW_CREATE_SELECTION"],
                         "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES }
            definition.faces is Query;
        }
        else if (definition.patternType == MirrorType.FEATURE)
        {
            annotation { "Name" : "Features to mirror" }
            definition.instanceFunction is FeatureList;
        }

        annotation { "Name" : "Mirror plane", "Filter" : QueryFilterCompound.ALLOWS_PLANE, "MaxNumberOfPicks" : 1 }
        definition.mirrorPlane is Query;

        if (definition.patternType == MirrorType.PART)
        {
            booleanPatternScopePredicate(definition);
        }

        if (definition.patternType == MirrorType.FEATURE)
        {
            annotation { "Name" : "Reapply features" }
            definition.fullFeaturePattern is boolean;
        }
    }
    {
        verifyNoMesh(context, definition, "mirrorPlane");
        if (definition.patternType == MirrorType.FACE)
        {
            verifyNoMesh(context, definition, "faces");
        }

        definition = adjustPatternDefinitionEntities(context, definition, true);

        var remainingTransform;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V325_FEATURE_MIRROR))
            remainingTransform = getRemainderPatternTransform(context, {"references" : qUnion([definition.entities])});
        else
            remainingTransform = getRemainderPatternTransform(context, {"references" : qUnion([definition.entities, definition.mirrorPlane])});

        const mateConnectorCSys = try silent(evMateConnector(context, { "mateConnector" : definition.mirrorPlane }));

        var planeResult;
        if (mateConnectorCSys != undefined)
        {
            planeResult = plane(mateConnectorCSys);
        }
        else
        {
            definition.mirrorPlane = qGeometry(definition.mirrorPlane, GeometryType.PLANE);
            planeResult = try(evPlane(context, { "face" : definition.mirrorPlane}));
        }

        if (planeResult != undefined && isFeaturePattern(definition.patternType))
            planeResult = inverse(remainingTransform) * planeResult; // we don't want to transform the mirror plane

        if (planeResult == undefined)
            throw regenError(ErrorStringEnum.MIRROR_NO_PLANE, ["mirrorPlane"]);

        definition.mirrorPlaneCalculated = planeResult;
        const transform = mirrorAcross(planeResult);

        definition.transforms = [transform];
        definition.instanceNames = ["1"];
        definition[notFoundErrorKey("entities")] = ErrorStringEnum.MIRROR_SELECT_PARTS;
        // We only include original body in the tools if the operation is UNION
        if (definition.patternType == MirrorType.PART && definition.operationType == NewBodyOperationType.ADD)
        {
            definition.seed = definition.entities;
            if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V1200_JOIN_SURFACE_PATTERN))
            {
                // Optimization: after this version we no longer need the matches, as they are calculated automatically
                definition.surfaceJoinMatches = createMatchesForSurfaceJoin(context, id, definition, planeResult);
            }
        }

        definition.sketchPatternInfo = ErrorStringEnum.MIRROR_SKETCH_REAPPLY_INFO;

        applyPattern(context, id, definition, remainingTransform);
        setPatternData(context, id, RecordPatternType.MIRROR, []);
    }, { patternType : MirrorType.PART, operationType : NewBodyOperationType.NEW, fullFeaturePattern : true});

/**
 * @internal
 * implements heuristics for mirror feature
 */
export function mirrorEditLogic(context is Context, id is Id, oldDefinition is map, definition is map,
    specifiedParameters is map, hiddenBodies is Query) returns map
{
    return booleanStepEditLogic(context, id, oldDefinition, definition,
                                specifiedParameters, hiddenBodies, mirror);
}

/** @internal */
function createMatchesForSurfaceJoin(context is Context, id is Id, definition is map, mirrorPlane is Plane) returns array
{
    var matches = [];

    if (definition.patternType == MirrorType.PART)
    {
        var edgesOnPlane = evaluateQuery(context, qCoincidesWithPlane(qEdgeTopologyFilter(qOwnedByBody(definition.entities, EntityType.EDGE), EdgeTopology.ONE_SIDED), mirrorPlane));
        matches = makeArray(size(edgesOnPlane));
        for (var i, edge in edgesOnPlane)
        {
            var mirrorEdge = startTracking(context, edge);
            matches[i] = { "topology1" : edge, "topology2" : mirrorEdge, "matchType" : TopologyMatchType.COINCIDENT };
        }
        return matches;
    }
}

