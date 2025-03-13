FeatureScript 2615; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/patternUtils.fs", version : "2615.0");
export import(path : "onshape/std/curvepatternorientationtype.gen.fs", version : "2615.0");

// Useful export for users
export import(path : "onshape/std/path.fs", version : "2615.0");

// Imports used internally
import(path : "onshape/std/curveGeometry.fs", version : "2615.0");
import(path : "onshape/std/manipulator.fs", version : "2615.0");
import(path : "onshape/std/mathUtils.fs", version : "2615.0");
import(path : "onshape/std/sketch.fs", version : "2615.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2615.0");
import(path : "onshape/std/topologyUtils.fs", version : "2615.0");
import(path : "onshape/std/recordpatterntype.gen.fs", version : "2615.0");
import(path : "onshape/std/profilecontrolmode.gen.fs", version : "2615.0");

/**
 * Specifies the type of spacing between pattern instances.
 * @value EQUAL : Equal-spaced instances along the length of curve
 * @value DISTANCE : Instances spaced by custom distance
 */
export enum CurvePatternSpacingType
{
    annotation { "Name" : "Equal spacing" }
    EQUAL,
    annotation { "Name" : "Distance" }
    DISTANCE
}

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
 *      @field spacingType {CurvePatternSpacingType}:
 *              Specifies the type of spacing between pattern entities. Default is `EQUAL`.
 *              @autocomplete `CurvePatternSpacingType.EQUAL`
 *      @field distance {ValueWithUnits}: @requiredif{`spacingType` is `DISTANCE`}
 *              The distance between each pattern entity.
 *              @eg `1.0 * inch` to space the pattern entities 1 inch apart.
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
 *
 *      @field skipInstances {boolean}: @optional
 *              Whether to exclude certain instances of the pattern.
 *      @field skippedInstances {array}: @requiredif {`skipInstances` is `true`}
 *              Which instances of the pattern to skip. Each is denoted by a single positive index.
 *              @ex `[{ index: 2 }, { index: 5 }]`
 * }}
 */
annotation { "Feature Type Name" : "Curve pattern", "Filter Selector" : "allparts", "Manipulator Change Function" : "curvePatternPointChange" }
export const curvePattern = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        patternTypePredicate(definition);

        annotation { "Name" : "Path to pattern along", "Filter" : EntityType.EDGE || (EntityType.BODY && BodyType.WIRE && SketchObject.NO) }
        definition.edges is Query;

        annotation { "Name" : "Spacing type" }
        definition.spacingType is CurvePatternSpacingType;

        if (definition.spacingType == CurvePatternSpacingType.DISTANCE)
        {
            annotation { "Name" : "Distance" }
            isLength(definition.distance, PATTERN_OFFSET_BOUND);
        }

        annotation { "Name" : "Instance count" }
        isInteger(definition.instanceCount, CURVE_PATTERN_BOUNDS);

        annotation { "Name" : "Orientation", "Default" : CurvePatternOrientationType.DEFAULT, "UIHint" : [UIHint.SHOW_LABEL] }
        definition.orientationType is CurvePatternOrientationType;

        if (definition.orientationType == CurvePatternOrientationType.LOCK_FACES)
        {
            annotation { "Name" : "Faces to be normal to", "Filter" : EntityType.FACE && ConstructionObject.NO }
            definition.lockedFaces is Query;
        }

        if (definition.patternType == PatternType.PART)
        {
            booleanPatternScopePredicate(definition);
        }
        if (definition.patternType == PatternType.FEATURE)
        {
            annotation { "Name" : "Reapply features" }
            definition.fullFeaturePattern is boolean;
        }

        annotation { "Name" : "Skip instances" }
        definition.skipInstances is boolean;

        annotation { "Group Name" : "Skip instances", "Driving Parameter" : "skipInstances", "Collapsed By Default" : false }
        {
            if (definition.skipInstances)
            {
                annotation { "Name" : "Instances to skip", "Item name" : "instance", "Item label template" : "#index", "Show labels only" : true, "UIHint" : [UIHint.INITIAL_FOCUS, UIHint.PREVENT_ARRAY_REORDER, UIHint.ALLOW_ARRAY_FOCUS] }
                definition.skippedInstances is array;

                for (var instance in definition.skippedInstances)
                {
                    annotation { "Name" : "Index" }
                    isInteger(instance.index, { (unitless) : [-1e5, 1, 1e5] } as IntegerBoundSpec);
                }
            }
        }
    }
    {
        verifyNoMesh(context, definition, "edges");

        definition = adjustPatternDefinitionEntities(context, definition, false);

        // must be done before transforming definition.instanceFunction with valuesSortedById(...)
        const referenceEntities = collectReferenceEntities(context, definition);

        verifyPatternSize(context, id, definition.instanceCount);

        if (tooFewPatternInstances(context, definition.instanceCount))
            throw regenError(ErrorStringEnum.PATTERN_INPUT_TOO_FEW_INSTANCES, ["instanceCount"]);

        verifyNonemptyQuery(context, definition, "edges", ErrorStringEnum.PATTERN_CURVE_NO_EDGES);

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V576_GET_WIRE_LAMINAR_DEPENDENCIES))
        {
            definition.edges = dissolveWires(definition.edges);
        }

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2454_CURVE_PATTERN_FS_CHANGES_FOR_LOCK_FACES)
            && definition.orientationType == CurvePatternOrientationType.LOCK_FACES
            && isQueryEmpty(context, definition.lockedFaces))
        {
            throw regenError(ErrorStringEnum.CURVE_PATTERN_MISSING_FACE_SELECTION, ["lockedFaces"]);
        }

        const remainingTransform = getRemainderPatternTransform(context, { "references" : qUnion([getReferencesForRemainderTransform(definition), definition.edges]) });

        var constructPathResult;
        try
        {
            constructPathResult = constructPath(context, definition.edges, referenceEntities);
        }
        catch
        {
            throw regenError(ErrorStringEnum.PATH_EDGES_NOT_CONTINUOUS, ["edges"]);
        }

        const path = constructPathResult.path;
        var withinBoundingBox = constructPathResult.pathDistanceInformation.withinBoundingBox;

        // If there are more than 1 entities to pattern, depending on the type of spacing selected, generate curve parameters for pattern.
        // Parameters generated for curve pattern are numbers between 0 and 1, where 0 represents start of the curve and 1 represents the end.
        if (definition.instanceCount > 1)
        {
            var parameters = [];
            if (definition.spacingType == CurvePatternSpacingType.DISTANCE)
            {
                var pathLength = evPathLength(context, constructPathResult.path);
                const minPathLengthRequired = path.closed ? (definition.distance * definition.instanceCount) : (definition.distance) * (definition.instanceCount - 1);
                pathLength = path.closed ? pathLength - definition.distance : pathLength;

                // Throw an error if minimum length required to generate pattern exceeds the total length of the curve
                if ((pathLength - minPathLengthRequired) < -TOLERANCE.zeroLength * meter)
                {
                    throw regenError(ErrorStringEnum.CURVE_PATTERN_DISTANCE_TOO_LARGE, ["distance"]);
                }
                for (var i = 0; i < definition.instanceCount; i += 1)
                {
                    parameters = append(parameters, (definition.distance * i) / pathLength);
                }
            }
            else
            {
                // If the path is open, the parameters are {0.0, 1 / (instanceCount - 1), ..., (instanceCount - 2) / (instanceCount - 1), 1.0}
                // If the path is closed, the parameters are {0.0, 1 / (instanceCount), ..., (instanceCount - 2) / (instanceCount), (instanceCount - 1) / (instanceCount)}
                const divisor = path.closed ? definition.instanceCount : definition.instanceCount - 1;
                for (var i = 0; i < definition.instanceCount; i += 1)
                {
                    parameters = append(parameters, i / divisor);
                }
            }

            const keepOrientation = definition.orientationType == CurvePatternOrientationType.KEEP_ORIENTATION;
            const lockFaces = definition.orientationType == CurvePatternOrientationType.LOCK_FACES;

            // Get tangent planes or lines from computePatternTangents
            const patternTangentResult = computePatternTangents(context, id, path, parameters, referenceEntities, keepOrientation, lockFaces, definition.lockedFaces);
            withinBoundingBox = withinBoundingBox || patternTangentResult.pathDistanceInformation.withinBoundingBox;

            if (!withinBoundingBox)
            {
                const message = path.closed ? ErrorStringEnum.CURVE_PATTERN_START_OFF_CLOSED_PATH : ErrorStringEnum.CURVE_PATTERN_START_OFF_PATH;
                reportFeatureInfo(context, id, message);
            }

            const curvePatternTransforms = computeCurvePatternTransforms(context, definition, patternTangentResult.tangents);

            if (definition.skipInstances)
            {
                reportAnyInvalidEntries(context, id, definition);

                addManipulators(context, id, { "points" : {
                                    "points" : curvePatternTransforms.manipulatorPoints,
                                    "selectedIndices" : mapArray(definition.skippedInstances, instance => instance.index),
                                    "suppressedIndices" : [0],
                                    "manipulatorType" : ManipulatorType.TOGGLE_POINTS } as Manipulator });
            }

            definition.transforms = curvePatternTransforms.transforms;
            definition.instanceNames = curvePatternTransforms.instanceNames;
        }
        else
        {
            if (definition.skipInstances)
            {
                const startPoint = try silent(getStartPoint(context, getReferencesForStartPoint(definition)));

                reportAnyInvalidEntries(context, id, definition);

                addManipulators(context, id, { "points" : {
                                    "points" : startPoint is Vector ? [startPoint] : [],
                                    "selectedIndices" : mapArray(definition.skippedInstances, instance => instance.index),
                                    "suppressedIndices" : [0],
                                    "manipulatorType" : ManipulatorType.TOGGLE_POINTS } as Manipulator });
            }

            definition.transforms = [];
            definition.instanceNames = [];
        }

        definition.seed = definition.entities;

        definition.sketchPatternInfo = ErrorStringEnum.CURVE_PATTERN_SKETCH_REAPPLY_INFO;

        applyPattern(context, id, definition, remainingTransform);
        setPatternData(context, id, RecordPatternType.CURVE, []);
    }, {
            patternType : PatternType.PART,
            operationType : NewBodyOperationType.NEW,
            orientationType : CurvePatternOrientationType.DEFAULT,
            lockedFaces : qNothing(),
            fullFeaturePattern : false,
            spacingType : CurvePatternSpacingType.EQUAL,
            skipInstances : false,
            skippedInstances : []
        });

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

        if (!isQueryEmpty(context, qMeshGeometryFilter(referenceEntities, MeshGeometry.YES)))
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
function computePatternTangents(context is Context, id is Id, path is Path, parameters is array, referenceEntities is Query, keepOrientation is boolean, lockFaces is boolean, lockedFaces is Query) returns map
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
    const tangentEdgeIndices = pathTangents.edgeIndices;

    if (keepOrientation)
    {
        var referenceDirection = tangentLines[0].direction;
        for (var i = 1; i < size(tangentLines); i += 1)
        {
            tangentLines[i].direction = referenceDirection;
        }
        return { "tangents" : tangentLines, "pathDistanceInformation" : pathDistanceInformation };
    }

    var tangents;
    const featureId = id + "tangents";
    startFeature(context, featureId, {});
    try silent
    {
        tangents = refinePatternTangents(context, featureId, path, tangentLines, tangentEdgeIndices, lockFaces, lockedFaces);
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
function refinePatternTangents(context is Context, id is Id, path is Path, tangentLines is array, tangentEdgeIndices is array, lockFaces is boolean, lockedFaces is Query) returns array
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
                    "subquery" : sketchEntityQuery(sketchId, undefined, "line1.end"),
                    "secondarySubquery" : path.edges[i]
                });

        // In very rare cases this can fail, in which case computePatternTangents catches the failure and uses tangent
        // lines instead of refining into tangent planes
        var sweepId = id + ("sweep" ~ i);
        const profileControlMode = lockFaces ? ProfileControlMode.LOCK_FACES : ProfileControlMode.NONE;
        const lockedFacesQuery = !isQueryEmpty(context, lockedFaces) && lockFaces ? lockedFaces : qNothing();

        opSweep(context, sweepId, {
                    "profiles" : qCreatedBy(id + ("sketch" ~ i), EntityType.EDGE),
                    "path" : path.edges[i],
                    "profileControl" : profileControlMode,
                    "lockFaces" : lockedFacesQuery
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
        var distanceResult = evDistance(context, {
                "side0" : sweepFaces[tangentEdgeIndices[i]],
                "side1" : tangentLines[i].origin,
                "arcLengthParameterization" : false
            });
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

function reportAnyInvalidEntries(context is Context, id is Id, definition is map)
{
    var hasSeedIndex = false;
    var hasOutsideRangeIndex = false;

    for (var instance in definition.skippedInstances)
    {
        if (instance.index == 0)
        {
            hasSeedIndex = true;
        }

        if (instance.index >= definition.instanceCount || instance.index < 0)
        {
            hasOutsideRangeIndex = true;
        }
    }

    if (hasSeedIndex)
    {
        reportFeatureInfo(context, id, ErrorStringEnum.PATTERN_SKIPPED_INSTANCES_SEED_INDEX);
    }
    else if (hasOutsideRangeIndex)
    {
        reportFeatureInfo(context, id, ErrorStringEnum.PATTERN_SKIPPED_INSTANCES_OUT_OF_RANGE_INDEX);
    }
}

function computeCurvePatternTransforms(context is Context, definition is map, tangents is array) returns PatternTransforms
{
    // Features held back from before the @computeCurvePatternTransforms builtin was introduced should run the previous code for computing the list of transforms.
    // definition.computeTransformsWithoutBuiltin will be true for such features until the next time the feature is opened, after which it will be undefined.
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2338_PATTERN_SKIP_INSTANCES) || definition.computeTransformsWithoutBuiltin != true)
    {
        definition.tangents = tangents;

        if (definition.skipInstances)
        {
            definition.startPoint = try silent (getStartPoint(context, getReferencesForStartPoint(definition)));
        }

        return @computeCurvePatternTransforms(context, definition) as PatternTransforms;
    }

    // Transform(..., ...) works with planes or lines
    var transforms = [transform(tangents[0], tangents[1])];
    var instanceNames = ["1"];
    for (var i = 2; i < definition.instanceCount; i += 1)
    {
        transforms = append(transforms, transform(tangents[i - 1], tangents[i]) * transforms[i - 2]);
        instanceNames = append(instanceNames, "" ~ i);
    }

    return { "transforms" : transforms, "instanceNames" : instanceNames, "manipulatorPoints" : [] } as PatternTransforms;
}

/**
 * @internal
 * The manipulator change function used in the `curvePattern` feature.
 */
export function curvePatternPointChange(context is Context, definition is map, newManipulators is map) returns map
{
    definition.skippedInstances = mapArray(newManipulators["points"].selectedIndices, index => { "index" : index });
    return definition;
}

