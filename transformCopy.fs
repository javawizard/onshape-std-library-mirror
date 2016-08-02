FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/mateconnectoraxistype.gen.fs", version : "✨");
export import(path : "onshape/std/query.fs", version : "✨");

// Features using manipulators must export these.
export import(path : "onshape/std/manipulator.fs", version : "✨");
export import(path : "onshape/std/tool.fs", version : "✨");

// Imports used internally
import(path : "onshape/std/box.fs", version : "✨");
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/coordSystem.fs", version : "✨");
import(path : "onshape/std/curveGeometry.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/mathUtils.fs", version : "✨");
import(path : "onshape/std/surfaceGeometry.fs", version : "✨");
import(path : "onshape/std/tool.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");

/**
 * Defines how a the transform for a `transform` feature should be specified.
 */
export enum TransformType
{
    annotation { "Name" : "Translate by line" }
    TRANSLATION_ENTITY,
    annotation { "Name" : "Translate by distance" }
    TRANSLATION_DISTANCE,
    annotation { "Name" : "Translate by XYZ" }
    TRANSLATION_3D,
    annotation { "Name" : "Transform by mate connectors" }
    TRANSFORM_MATE_CONNECTORS,
    annotation { "Name" : "Rotate" }
    ROTATION,
    annotation { "Name" : "Copy in place" }
    COPY,
    annotation { "Name" : "Scale uniformly" }
    SCALE_UNIFORMLY
}

/* Manipulator names */
const ROTATE = "rotate";
const OFFSET_LINE = "offsetLine";
const TRANSLATION = "translation";

/* Reduce from [-2 pi, +2 pi] to [-pi, +pi] radians */
function reduceAngle(angle is ValueWithUnits) returns ValueWithUnits
precondition
{
    isAngle(angle);
}
{
    const CIRCLE = 2 * PI * radian;
    const REDUCE_ADD = 3 * PI * radian;
    const REDUCE_SUB = PI * radian;
    return (angle + REDUCE_ADD) % CIRCLE - REDUCE_SUB;
}

/* Find a point to attach the manipulator bar. */
function findCenter(context is Context, id is Id, entities is Query) returns Vector
{
    const boxResult = evBox3d(context, { topology : entities, 'tight' : false });
    return (boxResult.minCorner + boxResult.maxCorner) / 2;
}

function reportCoincident(context is Context, id is Id, distance is Vector)
{
    if (norm(distance).value > TOLERANCE.zeroLength)
        return;
    throw regenError(ErrorStringEnum.POINTS_COINCIDENT);
}

// Note that transform() is also defined in transform.fs
// with different signatures.  This is written as a wrapper around defineFeature to keep overloads working.
/**
 * Move and/or rotate a body or bodies with a single `Transform`, constructed with input according to the
 * selected `TransformType`.
 *
 * Internally, performs an `opTransform` when not copying, and an `opPattern` when copying. For simple
 * transforms, prefer calling `opTransform` or `opPattern` directly.
 *
 * @param definition {{
 *      @field entities {Query} : The bodies to transform.
 *
 *      @field transformType {TransformType} : Defines how the transform type should be specified.
 *          @eg `TransformType.TRANSLATION_3D`
 *
 *      @field dx {ValueWithUnits} : @requiredIf {`transformType` is `TransformType.TRANSLATION_3D`}
 *          A value with length units specifying the distance to move in the world `x` direction.
 *          @eg `1 * inch`
 *      @field dy {ValueWithUnits} : @requiredIf {`transformType` is `TransformType.TRANSLATION_3D`}
 *          A value with length units specifying the distance to move in the world `y` direction.
 *          @eg `1 * inch`
 *      @field dz {ValueWithUnits} : @requiredIf {`transformType` is `TransformType.TRANSLATION_3D`}
 *          A value with length units specifying the distance to move in the world `z` direction.
 *          @eg `1 * inch`
 *
 *      @field transformLine {Query} : @requiredIf {`transformType` is `TransformType.TRANSLATION_ENTITY`}
 *          A `Query` for either a single line or a pair of points, specifying the direction and
 *          distance to transform.
 *      @field oppositeDirectionEntity {boolean} : @requiredIf {`transformType` is `TransformType.TRANSLATION_ENTITY`}
 *          @ex `true` to flip the transform direction.
 *
 *      @field transformAxis {Query} : @requiredIf {`transformType` is `TransformType.ROTATION`}
 *          A `Query` for a line, cylinder, etc. to specify the transform direction.
 *      @field angle {ValueWithUnits} : @requiredIf {`transformType` is `TransformType.ROTATION`}
 *          A value with angle units specifying the angle to rotate.
 *
 *      @field transformDirection {Query} : @requiredIf {`transformType` is `TransformType.TRANSLATION_DISTANCE`}
 *          A `Query` for either a single line or a pair of points, specifying the direction to transform.
 *      @field distance {ValueWithUnits} : @requiredIf {`transformType` is `TransformType.TRANSLATION_DISTANCE`}
 *          A value with length units specifying the distance to move.
 *      @field oppositeDirection {boolean} : @requiredIf {`transformType` is `TransformType.TRANSLATION_DISTANCE`
 *              or `TransformType.ROTATION`}
 *          @ex `true` to transform in the opposite direction.
 *
 *      @field scale {number} : @requiredIf {`transformType` is `TransformType.SCALE_UNIFORMLY`}
 *          A positive real number specifying the scale factor.
 *      @field scalePoint {Query} : @requiredIf {`transformType` is `TransformType.SCALE_UNIFORMLY`}
 *
 *      @field baseConnector {Query} : @requiredIf {`transformType` is `TransformType.TRANSFORM_MATE_CONNECTORS`}
 *          The mate connector to transform from.
 *      @field destinationConnector {Query} : @requiredIf {`transformType` is `TransformType.TRANSFORM_MATE_CONNECTORS`}
 *          The mate connector to transform to.
 *      @field oppositeDirectionMateAxis {boolean} : @requiredIf {`transformType` is `TransformType.TRANSFORM_MATE_CONNECTORS`}
 *      @field secondaryAxisType {MateConnectorAxisType} : @requiredIf {`transformType` is `TransformType.TRANSFORM_MATE_CONNECTORS`}
 *
 *      @field makeCopy {boolean} : @requiredIf {`transformType` is not `TransformType.COPY`}
 * }}
 */
annotation { "Feature Type Name" : "Transform",
             "Manipulator Change Function" : "transformManipulatorChange",
             "Filter Selector" : "allparts" }
export function transform(context is Context, id is Id, definition is map)
{
    fTransform(context, id, definition);
}

const fTransform = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Parts to transform or copy",
                     "Filter" : EntityType.BODY && AllowMeshGeometry.YES }
        definition.entities is Query;

        annotation { "Name" : "Transform type" }
        definition.transformType is TransformType;

        if (definition.transformType == TransformType.TRANSLATION_ENTITY)
        {
            annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
            definition.oppositeDirectionEntity is boolean;

            annotation { "Name" : "Line or points",
                         "Filter" : EntityType.VERTEX || EntityType.EDGE,
                         "MaxNumberOfPicks" : 2 }
            definition.transformLine is Query;
        }
        else if (definition.transformType == TransformType.ROTATION)
        {
            annotation { "Name" : "Axis",
                         "Filter" : QueryFilterCompound.ALLOWS_AXIS,
                         "MaxNumberOfPicks" : 1 }
            definition.transformAxis is Query;
        }
        else if (definition.transformType == TransformType.TRANSLATION_DISTANCE)
        {
            annotation { "Name" : "Direction",
                         "Filter" : QueryFilterCompound.ALLOWS_AXIS || GeometryType.PLANE || EntityType.VERTEX,
                         "MaxNumberOfPicks" : 2 }
            definition.transformDirection is Query;
            annotation { "Name" : "Distance" }
            isLength(definition.distance, NONNEGATIVE_LENGTH_BOUNDS);
        }
        else if (definition.transformType == TransformType.SCALE_UNIFORMLY)
        {
            annotation { "Name" : "Scale" }
            isReal(definition.scale, SCALE_BOUNDS);
        }
        else if (definition.transformType == TransformType.TRANSFORM_MATE_CONNECTORS)
        {
            annotation { "Name" : "From mate connector",
                         "Filter" : BodyType.MATE_CONNECTOR,
                         "MaxNumberOfPicks" : 1 }
            definition.baseConnector is Query;

            annotation { "Name" : "To mate connector",
                         "Filter" : BodyType.MATE_CONNECTOR,
                         "MaxNumberOfPicks" : 1 }
            definition.destinationConnector is Query;

            if (definition.oppositeDirectionMateAxis != undefined)
            {
                annotation { "Name" : "Flip primary axis", "UIHint" : "PRIMARY_AXIS" }
                definition.oppositeDirectionMateAxis is boolean;
            }

            if (definition.secondaryAxisType != undefined)
            {
                annotation { "Name" : "Reorient secondary axis", "UIHint" : "MATE_CONNECTOR_AXIS_TYPE", "Default" : MateConnectorAxisType.PLUS_X }
                definition.secondaryAxisType is MateConnectorAxisType;
            }
        }

        if (definition.transformType == TransformType.ROTATION)
        {
            annotation { "Name" : "Angle" }
            isAngle(definition.angle, ANGLE_360_BOUNDS);
        }

        if (definition.transformType == TransformType.ROTATION ||
            definition.transformType == TransformType.TRANSLATION_DISTANCE)
        {
            annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
            definition.oppositeDirection is boolean;
        }

        if (definition.transformType == TransformType.TRANSLATION_3D)
        {
            annotation { "Name" : "X translation" }
            isLength(definition.dx, ZERO_DEFAULT_LENGTH_BOUNDS);

            annotation { "Name" : "Y translation" }
            isLength(definition.dy, ZERO_DEFAULT_LENGTH_BOUNDS);

            annotation { "Name" : "Z translation" }
            isLength(definition.dz, ZERO_DEFAULT_LENGTH_BOUNDS);
        }

        if (definition.transformType == TransformType.SCALE_UNIFORMLY)
        {
            annotation { "Name" : "Point",
                         "Filter" : EntityType.VERTEX,
                         "MaxNumberOfPicks" : 1 }
            definition.scalePoint is Query;
        }

        if (definition.transformType != TransformType.COPY)
        {
            annotation { "Name" : "Copy part" }
            definition.makeCopy is boolean;
        }
    }
    {
        //Start by figuring out the transform
        var transformMatrix = identityTransform();
        const transformType = definition.transformType;

        /* Prior to V74
           1. Transform of no part was not an error (bug compatibility)
           2. Null transform was not an error (behavior change)
         */
        const validateInputs = isAtVersionOrLater(context, FeatureScriptVersionNumber.V74_TRANSFORM_CHECKING);

        if (validateInputs && size(evaluateQuery(context, definition.entities)) == 0)
            throw regenError(ErrorStringEnum.CANNOT_RESOLVE_ENTITIES, ["entities"]);

        if (transformType == TransformType.TRANSLATION_ENTITY ||
            transformType == TransformType.TRANSLATION_DISTANCE)
        {
            const selection = (transformType == TransformType.TRANSLATION_ENTITY ?
                definition.transformLine : definition.transformDirection);
            //For a translation / translation by distance, figure out which entities are used to specify it
            const distanceSpecified = transformType == TransformType.TRANSLATION_DISTANCE;
            const vertices = evaluateQuery(context, qEntityFilter(selection, EntityType.VERTEX));
            const edges = evaluateQuery(context, qEntityFilter(selection, EntityType.EDGE));
            const nedges = @size(edges);
            const faces = evaluateQuery(context, qEntityFilter(selection, EntityType.FACE));
            var translation;

            if (@size(vertices) >= 2)
            {
                const v0 = evVertexPoint(context, { "vertex" : vertices[0] });
                const v1 = evVertexPoint(context, { "vertex" : vertices[1] });
                translation = v1 - v0;
                if (validateInputs)
                    reportCoincident(context, id, translation);
            }
            else if (validateInputs && nedges > 1)
            {
                throw regenError(ErrorStringEnum.TOO_MANY_ENTITIES_SELECTED,
                        [distanceSpecified ? "transformDirection" : "transformLine"]);
            }
            else if (nedges >= 1)
            {
                const evalResult = evEdgeTangentLines(context, { "edge" : edges[0], "parameters" : [0, 1] });
                translation = evalResult[1].origin - evalResult[0].origin;
                if (validateInputs)
                    reportCoincident(context, id, translation);
            }
            else if (distanceSpecified && @size(faces) >= 1) // A plane only provides direction
            {
                const planeResult = try(evPlane(context, { "face" : faces[0] }));
                if (planeResult is Plane)
                    translation = planeResult.normal * meter;
                else
                    translation = evAxis(context, { "axis" : faces[0] }).direction * meter;
            }
            else
            {
                if (distanceSpecified)
                    throw regenError(ErrorStringEnum.TRANSFORM_TRANSLATE_BY_DISTANCE_INPUT, ["transformDirection"]);
                else
                    throw regenError(ErrorStringEnum.TRANSFORM_TRANSLATE_INPUT, ["transformLine"]);
            }

            if (distanceSpecified)
            {
                if (norm(translation).value < TOLERANCE.zeroLength)
                    throw regenError(ErrorStringEnum.NO_TRANSLATION_DIRECTION, ["transformDirection"]);

                const target = definition.entities;
                const origin = findCenter(context, id, target);
                const direction = normalize(translation);
                var distance = definition.distance;
                if (definition.oppositeDirection)
                    distance = -distance;
                addManipulators(context, id, {
                            (OFFSET_LINE) : linearManipulator(origin, direction, distance, target) });
                translation = direction * definition.distance;
            }
            transformMatrix = transform(translation);
        }
        else if (transformType == TransformType.ROTATION)
        {
            const axis = evAxis(context, { "axis" : definition.transformAxis });
            const target = definition.entities;
            const origin = findCenter(context, id, target);
            if (origin is undefined)
                return;
            var angle = reduceAngle(definition.angle);
            if (definition.oppositeDirection)
                angle = -angle;
            addManipulators(context, id,
                    { (ROTATE) :
                      angularManipulator({ "axisOrigin" : project(axis, origin),
                                           "axisDirection" : axis.direction,
                                           "rotationOrigin" : origin,
                                           "angle" : angle,
                                           "sources" : target }) });
            transformMatrix = rotationAround(axis, angle);
        }
        else if (transformType == TransformType.TRANSLATION_3D)
        {
            const dx = definition.dx;
            const dy = definition.dy;
            const dz = definition.dz;
            const transformVector = vector(dx, dy, dz);
            transformMatrix = transform(transformVector);
            const target = definition.entities;
            const origin = findCenter(context, id, target);
            if (origin is undefined)
                return;
            addManipulators(context, id, { (TRANSLATION) : triadManipulator(origin, transformVector, target) });
        }
        else if (transformType == TransformType.SCALE_UNIFORMLY)
        {
            const vertices = evaluateQuery(context, definition.scalePoint);
            if (@size(vertices) == 0)
            {
                throw regenError(ErrorStringEnum.TRANSFORM_SCALE_UNIFORMLY);
            }

            const matrix = identityMatrix(3) * definition.scale;

            const centerPoint = evVertexPoint(context, { "vertex" : definition.scalePoint });
            transformMatrix = transform(matrix, centerPoint - matrix * centerPoint);
        }
        else if (transformType == TransformType.TRANSFORM_MATE_CONNECTORS)
        {
            const q1 = evaluateQuery(context, definition.baseConnector);
            const q2 = evaluateQuery(context, definition.destinationConnector);
            if ((@size(q1) == 0) || (@size(q2) == 0))
            {
                throw regenError(ErrorStringEnum.TRANSFORM_MATE_CONNECTORS);
            }

            const c1 = evMateConnector(context, { "mateConnector" : definition.baseConnector });
            const c2 = evMateConnector(context, { "mateConnector" : definition.destinationConnector });
            var xAxis = c2.xAxis;
            var zAxis = definition.oppositeDirectionMateAxis ? -c2.zAxis : c2.zAxis;
            if (definition.secondaryAxisType != undefined)
            {
                if (definition.secondaryAxisType == MateConnectorAxisType.PLUS_Y)
                {
                    xAxis = cross(zAxis, xAxis);
                }
                else if (definition.secondaryAxisType == MateConnectorAxisType.MINUS_X)
                {
                    xAxis = -xAxis;
                }
                else if (definition.secondaryAxisType == MateConnectorAxisType.MINUS_Y)
                {
                    xAxis = -cross(zAxis, xAxis);
                }
            }
            const A = toWorld(c1);
            const B = toWorld(coordSystem(c2.origin, xAxis, zAxis));
            transformMatrix = (B * inverse(A));
        }

        /* Reversal for rotation and 3D translation is handled above.
           Reversal is not applicable to copy. */
        if ((transformType == TransformType.TRANSLATION_ENTITY &&
             definition.oppositeDirectionEntity) ||
            (transformType == TransformType.TRANSLATION_DISTANCE &&
             definition.oppositeDirection))
            transformMatrix = inverse(transformMatrix);

        if (transformType == TransformType.COPY || definition.makeCopy)
        {
            opPattern(context, id,
                      { "entities" : definition.entities,
                        "transforms" : [transformMatrix],
                        "instanceNames" : ["1"] });
        }
        else
        {
            const subId = validateInputs ? id : id + "transform";
            opTransform(context, subId,
                        { "bodies" : definition.entities,
                          "transform" : transformMatrix });
        }
    }, { oppositeDirection : false, scale : 1.0 });

function extractOffset(input is map, axis is string)
{
    const submap = input[axis];
    if (submap is undefined)
        return undefined;
    return submap.offset;
}

/**
 * @internal
 * Manipulator change function for `transform`.
 */
export function transformManipulatorChange(context is Context, output is map, input is map) returns map
{
    if (input[ROTATE] is map)
    {
        const angle = reduceAngle(input[ROTATE].angle);
        output.angle = abs(angle);
        output.oppositeDirection = angle < 0 * radian;
    }
    const distance = extractOffset(input, OFFSET_LINE);
    if (distance is ValueWithUnits)
    {
        output.distance = abs(distance);
        output.oppositeDirection = distance.value < 0;
    }
    if (input[TRANSLATION] is map)
    {
        const offset = input[TRANSLATION].offset;
        output.dx = offset[0];
        output.dy = offset[1];
        output.dz = offset[2];
    }
    return output;
}

annotation { "Deprecated" : "Use `opPattern`, or the `transform` feature instead.",
    "Feature Type Name" : "Copy part", "Filter Selector" : "allparts" }
export const copyPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities to copy", "Filter" : EntityType.BODY }
        definition.entities is Query;
    }
    {
        const transform = identityTransform();
        opPattern(context, id, { "entities" : definition.entities,
                                 "transforms" : [transform],
                                 "instanceNames" : ["1"],
                                 notFoundErrorKey("entities") : ErrorStringEnum.COPY_SELECT_PARTS });
    });

