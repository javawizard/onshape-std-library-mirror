FeatureScript 937; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/patternUtils.fs", version : "937.0");

// Useful export for users
export import(path : "onshape/std/path.fs", version : "937.0");

// Imports used internally
import(path : "onshape/std/curveGeometry.fs", version : "937.0");
import(path : "onshape/std/mathUtils.fs", version : "937.0");
import(path : "onshape/std/sketch.fs", version : "937.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "937.0");
import(path : "onshape/std/topologyUtils.fs", version : "937.0");

/**
 * Performs a body, face, or feature curve pattern. Internally, performs
 * an [applyPattern], which in turn performs an [opPattern] or, for a feature
 * pattern, calls the feature function.
 *
 * @param id : @autocomplete `id + "curvePattern1"`
 * @param definition {{
 *      @field patternType {PatternType}: @optional
 *              Specifies a `PART`, `FEATURE`, or `FACE` pattern. Default is `PART`.
 *              @autocomplete `PatternType.PART`
 *      @field entities {Query}: @requiredif{`patternType` is `PART`}
 *              The parts to pattern.
 *              @eg `qCreatedBy(id + "extrude1", EntityType.BODY)`
 *      @field faces {Query}: @requiredif{`patternType` is `FACE`}
 *              The faces to pattern.
 *      @field instanceFunction {FeatureList}: @requiredif{`patternType` is `FEATURE`}
 *              The [FeatureList] of the features to pattern.
 *
 *      @field edges {Query}:
 *              A [Query] for a set of edges to pattern along. The edges must form a continuous path.
 *      @field instanceCount {number}:
 *              The resulting number of pattern entities.
 *              @eg `2` to have 2 resulting pattern entities (including the seed).
 *      @field keepOrientation {boolean}: @optional
 *              @ex `true` for the pattern entities to keep the orientation of the seed
 *              @ex `false` for the pattern entities to reorient along the path (default)
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
annotation { "Feature Type Name" : "Curve pattern", "Filter Selector" : "allparts" }
export const curvePattern = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Pattern type" }
        definition.patternType is PatternType;

        if (definition.patternType == PatternType.PART)
        {
            booleanStepTypePredicate(definition);

            annotation { "Name" : "Entities to pattern", "Filter" : EntityType.BODY || BodyType.MATE_CONNECTOR }
            definition.entities is Query;
        }
        else if (definition.patternType == PatternType.FACE)
        {
            annotation { "Name" : "Faces to pattern",
                         "UIHint" : ["ALLOW_FEATURE_SELECTION", "SHOW_CREATE_SELECTION"],
                         "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES }
            definition.faces is Query;
        }
        else if (definition.patternType == PatternType.FEATURE)
        {
            annotation { "Name" : "Features to pattern" }
            definition.instanceFunction is FeatureList;
        }

        annotation { "Name" : "Path to pattern along", "Filter" : EntityType.EDGE || (EntityType.BODY && BodyType.WIRE) }
        definition.edges is Query;

        annotation { "Name" : "Instance count" }
        isInteger(definition.instanceCount, CURVE_PATTERN_BOUNDS);

        annotation { "Name" : "Keep orientation"}
        definition.keepOrientation is boolean;

        if (definition.patternType == PatternType.PART)
        {
            booleanStepScopePredicate(definition);
        }
        if (definition.patternType == PatternType.FEATURE)
        {
            annotation { "Name" : "Apply per instance" }
            definition.fullFeaturePattern is boolean;
        }
    }
    {
        definition = adjustPatternDefinitionEntities(context, definition, false);

        // must be done before transforming definition.instanceFunction with valuesSortedById(...)
        var referenceEntities = collectReferenceEntities(context, definition);

        verifyPatternSize(context, id, definition.instanceCount);

        if (definition.instanceCount < 2)
            throw regenError(ErrorStringEnum.PATTERN_INPUT_TOO_FEW_INSTANCES, ["instanceCount"]);

        if (size(evaluateQuery(context, definition.edges)) == 0)
            throw regenError(ErrorStringEnum.PATTERN_CURVE_NO_EDGES, ["edges"]);

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V576_GET_WIRE_LAMINAR_DEPENDENCIES))
        {
            definition.edges = dissolveWires(definition.edges);
        }

        var remainingTransform = getRemainderPatternTransform(context, { "references" : qUnion([getReferencesForRemainderTransform(definition), definition.edges]) });

        var constructPathResult;
        try
        {
            constructPathResult = constructPath(context, definition.edges, referenceEntities);
        }
        catch
        {
            throw regenError(ErrorStringEnum.PATH_EDGES_NOT_CONTINUOUS, ["edges"]);
        }

        var path = constructPathResult.path;
        var withinBoundingBox = constructPathResult.pathDistanceInformation.withinBoundingBox;

        // If the path is open, the parameters are {0.0, 1 / (instanceCount - 1), ..., (instanceCount - 2) / (instanceCount - 1), 1.0}
        // If the path is closed, the parameters are {0.0, 1 / (instanceCount), ..., (instanceCount - 2) / (instanceCount), (instanceCount - 1) / (instanceCount)}
        var divisor = path.closed ? definition.instanceCount : definition.instanceCount - 1;
        var parameters = [];
        for (var i = 0; i < definition.instanceCount; i += 1)
        {
            parameters = append(parameters, i / divisor);
        }

        // Get tangent planes or lines from computePatternTangents
        var patternTangentResult = computePatternTangents(context, id, path, parameters, referenceEntities, definition.keepOrientation);
        var tangents = patternTangentResult.tangents;
        withinBoundingBox = withinBoundingBox || patternTangentResult.pathDistanceInformation.withinBoundingBox;

        if (!withinBoundingBox)
        {
            var message = path.closed ? ErrorStringEnum.CURVE_PATTERN_START_OFF_CLOSED_PATH : ErrorStringEnum.CURVE_PATTERN_START_OFF_PATH;
            reportFeatureInfo(context, id, message);
        }

        // Transform(..., ...) works with planes or lines
        var transforms = [transform(tangents[0], tangents[1])];
        var names = ["1"];
        for (var i = 2; i < definition.instanceCount; i += 1)
        {
            transforms = append(transforms, transform(tangents[i - 1], tangents[i]) * transforms[i - 2]);
            names = append(names, "" ~ i);
        }

        definition.transforms = transforms;
        definition.instanceNames = names;
        definition.seed = definition.entities;

        applyPattern(context, id, definition, remainingTransform);
    }, { patternType : PatternType.PART, operationType : NewBodyOperationType.NEW, keepOrientation : false, fullFeaturePattern : false });

/**
 * Collect reference entities for the distance heuristic used to determine the path direction.
 * Must be called before transforming  definition.instanceFunction with valuesSortedById(...)
 */
function collectReferenceEntities(context is Context, definition is map) returns Query
{
    var referenceEntities;
    if (definition.patternType == PatternType.FEATURE)
    {
        var referenceEntityQueries = [];
        for (var idToFunction in definition.instanceFunction)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V491_CURVE_PATTERN_WIRES_ONLY) && isIdForSketch(context, idToFunction.key))
            {
                referenceEntityQueries = append(referenceEntityQueries, qBodyType(qCreatedBy(idToFunction.key, EntityType.BODY), BodyType.WIRE));
            }
            else
            {
                referenceEntityQueries = append(referenceEntityQueries, qCreatedBy(idToFunction.key));
            }
        }
        referenceEntities = qUnion(referenceEntityQueries);

        if (size(evaluateQuery(context, qMeshGeometryFilter(referenceEntities, MeshGeometry.YES))) > 0)
            throw regenError(ErrorStringEnum.MESH_NOT_SUPPORTED, ["instanceFunction"]);
    }
    else
    {
        // Part and face pattern
        referenceEntities = definition.entities;
    }
    return referenceEntities;
}

/**
 * If keepOrientation is true, return tangent [Lines]s whose `origin`s lay at correct intervals along the [Path], and
 * whose `direction`s all match the direction of the first [Line]. Otherwise, sweeps a line segment over the supplied
 * [Path] to compute tangent planes for curvePattern.  In the very rare case that this individual sweep fails, returns
 * tangent [Line]s instead.
 *
 * If `path` is closed, the parameters are remapped such that 0 is the closest point on the [Path] to the referenceEntities
 * instead of 0 being at the start of the [Path].
 *
 * @returns {{
 *      @field tangents {array} : the tangent [Line]s or [Plane]s at the specified parameters
 *      @field pathDistanceInformation {PathDistanceInformation} : A map containing the distance from the remapped 0
 *          parameter to the center of the bounding box of `referenceGeometry` and whether the remapped 0 parameter falls
 *          inside that bounding box. For non-closed paths `distance` is infinity and `withinBoundingBox` is false
 * }}
 */
function computePatternTangents(context is Context, id is Id, path is Path, parameters is array, referenceEntities is Query, keepOrientation is boolean) returns map
{
    var pathTangents;
    var pathDistanceInformation;
    if (path.closed)
    {
        pathTangents = evPathTangentLines(context, path, parameters, referenceEntities);
        pathDistanceInformation = pathTangents.pathDistanceInformation;
    }
    else
    {
        pathTangents = evPathTangentLines(context, path, parameters);
        pathDistanceInformation = defaultPathDistanceInformation();
    }

    var tangentLines = pathTangents.tangentLines;
    var tangentEdgeIndices = pathTangents.edgeIndices;

    if (keepOrientation)
    {
        var referenceDirection = tangentLines[0].direction;
        for (var i = 1; i < size(tangentLines); i += 1)
        {
            tangentLines[i].direction = referenceDirection;
        }
        return {"tangents" : tangentLines, "pathDistanceInformation" : pathDistanceInformation};
    }

    var tangents;
    var featureId = id + "tangents";
    startFeature(context, featureId, {});
    try silent
    {
        tangents = refinePatternTangents(context, featureId, path, tangentLines, tangentEdgeIndices);
    }
    catch
    {
        tangents = tangentLines;
    }
    // remove utility sketches and sweeps
    abortFeature(context, featureId);

    return { "tangents" : tangents, "pathDistanceInformation" : pathDistanceInformation };
}

/**
 * Refine tangent [Line]s into tangent [Plane]s by sweeping a sketch line segment along the [Path] and evaluating the
 * surface normals along the resultant surface. [opSweep] along the path can fail in very rare cases, in which case
 * this function will throw an error.
 */
function refinePatternTangents(context is Context, id is Id, path is Path, tangentLines is array, tangentEdgeIndices is array) returns array
{
    var sweepFaces = [];
    var prevEdgeEndpointTangents;
    var currEdgeEndpointTangents;
    var trackingQuery;
    for (var i = 0; i < size(path.edges); i += 1)
    {
        // find the tangents at the endpoints of the current edge, and flip them if necessary.
        currEdgeEndpointTangents = evEdgeTangentLines(context, { "edge" : path.edges[i], "parameters" : [0, 1] });
        if (path.flipped[i])
        {
            currEdgeEndpointTangents[0].direction = -currEdgeEndpointTangents[0].direction;
            currEdgeEndpointTangents[1].direction = -currEdgeEndpointTangents[1].direction;

            currEdgeEndpointTangents = reverse(currEdgeEndpointTangents);
        }

        // sketch a small line on the plane perpendicular to the start of the edge. The line should have one endpoint at
        // the start of the edge, and another at sketchPoint
        var sketchPlane = plane(currEdgeEndpointTangents[0].origin, currEdgeEndpointTangents[0].direction);
        var sketchPoint;

        if (i == 0)
        {
            // for the first edge, pick an arbitrary point for sketchPoint
            const sweepThickness = (10000 * TOLERANCE.zeroLength) * meter;
            sketchPoint = vector(0 * meter, sweepThickness);
        }
        else
        {
            // for subsequent edges, calculate the sketch point using getSketchPoint
            sketchPoint = getSketchPoint(context, id, i, prevEdgeEndpointTangents[1], currEdgeEndpointTangents[0], sketchPlane, trackingQuery);
        }

        var sketchId = id + ("sketch" ~ i);
        var sketch = newSketchOnPlane(context, sketchId, {
                "sketchPlane" : sketchPlane
            });

        skLineSegment(sketch, "line1", {
                "start" : vector(0 * inch, 0 * inch),
                "end" : sketchPoint
            });

        skSolve(sketch);

        trackingQuery = startTracking(context, {
                "subquery" :  sketchEntityQuery(sketchId, undefined, "line1.end"),
                "secondarySubquery" : path.edges[i]
            });

        // In very rare cases this can fail, in which case computePatternTangents catches the failure and uses tangent
        // lines instead of refining into tangent planes
        var sweepId = id + ("sweep" ~ i);
        opSweep(context, sweepId, {
                "profiles" : qCreatedBy(id + ("sketch" ~ i), EntityType.EDGE),
                "path" : path.edges[i]
            });

        sweepFaces = append(sweepFaces, qCreatedBy(sweepId, EntityType.FACE));

        prevEdgeEndpointTangents = currEdgeEndpointTangents;
    }

    // If we have made it to this point, none of the sweeps failed and we can use them to determine tangent planes.
    // Note that tangentLines contains the parameterization of the Path based on evPathTangentLines().
    var tangentPlanes = [];
    for (var i = 0; i < size(tangentLines); i += 1)
    {
        // Find the closest point on the swept edge that touches parameter i of the Path
        var distanceResult = evDistance(context, { "side0" : sweepFaces[tangentEdgeIndices[i]] , "side1" : tangentLines[i].origin});
        var parameter = distanceResult.sides[0].parameter;
        // Evaluate the tangent plane of the sweep at parameter i of the Path
        var tangentPlane = evFaceTangentPlane(context, { "face" : sweepFaces[tangentEdgeIndices[i]], "parameter" : parameter });

        // To prevent roll in curve pattern, set the x direction of the plane to the tangent line at parameter i of the
        // Path. Because of approximations in evaluation, the tangent line may be slightly off from orthogonal to the
        // normal of the plane; so project the tangent line into the plane before setting it as x.
        tangentPlane.x = project(tangentPlane, tangentLines[i]).direction;
        tangentPlanes = append(tangentPlanes, tangentPlane);
    }

    return tangentPlanes;
}

/**
 * Returns the next sketchPoint in terms of the coordinate system `sketchPlane` by querying for the result of sweeping
 * the previous sketchPoint and transforming that point by the difference between the tangent at the end of the last edge
 * and the tangent at the beginning of the current edge.
 */
function getSketchPoint(context is Context, id is Id, sketchIndex is number, prevEdgeEndPointTangent is Line,
        currEdgeStartPointTangent is Line, sketchPlane is Plane, trackingQuery is Query) returns Vector
{
    // Transform the result of sweeping the previous sketchPoint into the current sketchPlane.
    // The transformation from the previous plane into the current plane is the same as the transform between the
    // end tangent of the last edge and the start tangent of the current edge.
    var sketchPointTransform = transform(prevEdgeEndPointTangent, currEdgeStartPointTangent);

    // find the vertex that was the result of sweeping the previous sketchPoint
    var prevEndCapPointQueries = qCapEntity(id + ("sweep" ~ (sketchIndex - 1)), CapType.END, EntityType.VERTEX);
    var prevSketchPoint3D = evVertexPoint(context, { "vertex" : qIntersection([prevEndCapPointQueries, trackingQuery]) });

    // calculate the new sketchPoint
    var sketchPoint3D = sketchPointTransform * prevSketchPoint3D;
    var sketchPoint = worldToPlane(sketchPlane, sketchPoint3D);

    return sketchPoint;
}

