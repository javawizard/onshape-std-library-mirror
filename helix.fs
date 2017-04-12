FeatureScript 559; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "559.0");

// Features using manipulators must export manipulator.fs.
export import(path : "onshape/std/manipulator.fs", version : "559.0");

// Imports used internally
import(path : "onshape/std/box.fs", version : "559.0");
import(path : "onshape/std/curveGeometry.fs", version : "559.0");
import(path : "onshape/std/evaluate.fs", version : "559.0");
import(path : "onshape/std/feature.fs", version : "559.0");
import(path : "onshape/std/mathUtils.fs", version : "559.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "559.0");
import(path : "onshape/std/valueBounds.fs", version : "559.0");
import(path : "onshape/std/containers.fs", version : "559.0");


/**
 * Describes the direction a helix turns while traveling along its axis.
 * @value CW : Clockwise.
 * @value CCW : Counterclockwise.
 */
export enum Direction
{
    annotation { "Name" : "Clockwise" }
    CW,
    annotation { "Name" : "Counterclockwise" }
    CCW
}

/**
 * Specifies how the helix will be defined.
 * @value TURNS : User defines the number of turns, and the helical pitch is
 *                  computed based on the input entity's height.
 * @value PITCH : User defines the helical pitch, and the number of turns is
 *                  computed based on the input entity's height.
 * @value HEIGHT_TURNS : User defines both the height and number of turns, and
 *                  the height is computed based on these terms.
 * @value HEIGHT_PITCH : User defines both the height and helical pitch, and
 *                  the number of turns is computed based on these terms.
 * @value TURNS_PITCH : User defines both the number of turns and helical pitch,
 *                  and the height is computed based on these terms.
 */
export enum HelixType
{
    annotation { "Name" : "Turns" }
    TURNS,
    annotation { "Name" : "Pitch" }
    PITCH,
    annotation { "Name" : "Height and Turns" }
    HEIGHT_TURNS,
    annotation { "Name" : "Height and Pitch" }
    HEIGHT_PITCH,
    annotation { "Name" : "Turns and Pitch" }
    TURNS_PITCH
}

const needConeOrCylinderMessage = ErrorStringEnum.HELIX_INPUT_CONE;
const needCircleMessage = ErrorStringEnum.HELIX_INPUT_CIRCLE;

const HELIX_TURN_BOUNDS =
{
    (unitless) : [1e-5, 4, 1000]
} as RealBoundSpec;

/**
 * Feature performing an [opHelix].
 */
annotation { "Feature Type Name" : "Helix", "UIHint" : "CONTROL_VISIBILITY",
             "Manipulator Change Function" : "helixManipulatorChange",
             "Editing Logic Function" : "helixLogic" }
export const helix = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {

        // This hidden "placeholder" Query allows preselection of both cones/cylinders AND circles/arcs
        // We purposely accept all all EDGEs to allow for preselection in the feature tree.
        // Further constraints are applied in the helixLogic preselection function
        annotation { "Name" : "Entities", "UIHint" : "ALWAYS_HIDDEN", "Filter" : (EntityType.FACE && QueryFilterCompound.ALLOWS_AXIS) || EntityType.EDGE }
        definition.initEntities is Query;

        annotation { "Name" : "Helix type" }
        definition.helixType is HelixType;

        if (definition.helixType == HelixType.HEIGHT_TURNS ||
            definition.helixType == HelixType.HEIGHT_PITCH ||
            definition.helixType == HelixType.TURNS_PITCH)
        {
            annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
            definition.oppositeDirection is boolean;    // false by default
        }

        if (definition.helixType == HelixType.TURNS || definition.helixType == HelixType.PITCH)
        {
            annotation { "Name" : "Conical or cylindrical face", "MaxNumberOfPicks" : 1, "Filter" : EntityType.FACE && QueryFilterCompound.ALLOWS_AXIS }
            definition.entities is Query;
        }
        else
        {
            annotation { "Name" : "Circular edge", "MaxNumberOfPicks" : 1, "Filter" : GeometryType.CIRCLE || GeometryType.ARC }
            definition.edge is Query;
        }

        annotation { "Name" : "Handedness" }
        definition.handedness is Direction;

        if (definition.helixType == HelixType.HEIGHT_TURNS || definition.helixType == HelixType.HEIGHT_PITCH)
        {
            annotation { "Name" : "Height" }
            isLength(definition.height, NONNEGATIVE_ZERO_INCLUSIVE_LENGTH_BOUNDS);
        }

        if (definition.helixType == HelixType.TURNS ||
            definition.helixType == HelixType.HEIGHT_TURNS ||
            definition.helixType == HelixType.TURNS_PITCH)
        {
            annotation { "Name" : "Revolutions" }
            isReal(definition.revolutions, HELIX_TURN_BOUNDS);
        }

        if (definition.helixType == HelixType.PITCH ||
            definition.helixType == HelixType.HEIGHT_PITCH ||
            definition.helixType == HelixType.TURNS_PITCH)
        {
            annotation { "Name" : "Helical pitch" }
            isLength(definition.helicalPitch, NONNEGATIVE_LENGTH_BOUNDS);
        }

        annotation { "Name" : "Start angle" }
        isAngle(definition.startAngle, ANGLE_360_ZERO_DEFAULT_BOUNDS);

    }
    //===================================================<body>=======================================================
    {

        var definitionOut = {};
        var remainingTransform;
        var revolutions;

        if (definition.helixType == HelixType.TURNS || definition.helixType == HelixType.PITCH)
        {
            remainingTransform = getRemainderPatternTransform(context, {"references" : definition.entities});
            var surface = evSurfaceDefinition(context, { "face" : definition.entities });
            if ((surface is Cone) || (surface is Cylinder))
            {
                var endRadius;
                var baseRadius;
                const boxResult = evBox3d(context, { "topology" : definition.entities, "cSys" : surface.coordSystem });
                var minZ = boxResult.minCorner[2];
                var maxZ = boxResult.maxCorner[2];
                if (surface is Cylinder)
                {
                    endRadius = surface.radius;
                    baseRadius = surface.radius;
                }
                else if (surface is Cone)
                {
                    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V421_TRIM_HELIX_AT_CONE_APEX))
                    {
                        // Origin of coordinate system is guaranteed to be on the apex of a cone. The bounding box can be
                        // oversized by some tolerance (~1E-10m), so ensure helix won't cross through apex.
                        minZ = max(0 * meter, minZ);
                    }
                    const slope = tan(surface.halfAngle);
                    baseRadius = slope * minZ;
                    endRadius = slope * maxZ;
                }
                const height = maxZ - minZ;
                surface.coordSystem.origin += minZ * surface.coordSystem.zAxis;

                if (definition.helixType == HelixType.PITCH)
                {
                    definitionOut.helicalPitch = definition.helicalPitch;
                    revolutions = height.value / definition.helicalPitch.value;
                }
                else
                {
                    revolutions = definition.revolutions;
                    definitionOut.helicalPitch = height / revolutions;
                }

                definitionOut = updateDefinition(context, definitionOut, surface, baseRadius, endRadius, revolutions);
            }
            else
            {
                throw regenError(needConeOrCylinderMessage, ["entities"]);
            }
        }
        else
        {
            remainingTransform = getRemainderPatternTransform(context, {"references" : definition.edge});
            var edge = evCurveDefinition(context, { "edge" : definition.edge });
            if (!(edge is Circle))
            {
                throw regenError(needCircleMessage, ["entities"]);
            }
            var height;
            var baseRadius = edge.radius;
            var endRadius = baseRadius;

            if (definition.helixType == HelixType.TURNS_PITCH)
            {
                revolutions = definition.revolutions;
                definitionOut.helicalPitch = definition.helicalPitch;
                height = revolutions * definition.helicalPitch;
            }
            else
            {
                height = definition.height;
                if (definition.helixType == HelixType.HEIGHT_TURNS)
                {
                    revolutions = definition.revolutions;
                    definitionOut.helicalPitch = height / revolutions;
                }
                else if (definition.helixType == HelixType.HEIGHT_PITCH)
                {
                    definitionOut.helicalPitch = definition.helicalPitch;
                    revolutions = height / definition.helicalPitch;
                }
                // Add manipulator for the height of the helix
                var extrudeAxis = line(edge.coordSystem.origin, edge.coordSystem.zAxis);
                addHelixManipulator(context, id, definition, extrudeAxis);
            }

            if (definition.oppositeDirection)
            {
                edge.coordSystem.zAxis *= -1;
            }

            definitionOut = updateDefinition(context, definitionOut, edge, baseRadius, endRadius, revolutions);
        }

        const startPointVector = definitionOut.startPoint - definitionOut.axisStart;
        definitionOut.startPoint = (rotationMatrix3d(definitionOut.direction, definition.startAngle) * startPointVector) + definitionOut.axisStart;

        definitionOut.interval = [0, revolutions];

        if (definition.handedness == Direction.CW)
        {
            definitionOut.clockwise = true;
        }
        else
        {
            definitionOut.clockwise = false;
        }

        opHelix(context, id, definitionOut);
        transformResultIfNecessary(context, id, remainingTransform);

    }, { "initEntities" : qNothing() } );


function updateDefinition(context is Context, definitionOut is map, geometry is map, baseRadius is map,
                          endRadius is map, revolutions is number) returns map
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V374_HELIX_ALIGNMENT))
    {
        // Align the "base" of the helix based on the alignCanonically() function
        // This keeps the coordSystems of circles/arcs and cylinders/cones consistent
        var alignmentPlane = plane(geometry.coordSystem);
        alignmentPlane = alignCanonically(context, alignmentPlane);

        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V397_FLIP_CONE_HELIX))
        {
            // Cones always start drawing the helix from the "wrong" side (the tip)
            // This turns it around
            if (geometry is Cone)
            {
                // Relocate the origin:  Direction (unit vector) * Height (magnitude)
                alignmentPlane.origin += alignmentPlane.normal * definitionOut.helicalPitch * revolutions;
                // Flip the direction
                alignmentPlane.normal *= -1;
                // Realign the plane in its new location
                alignmentPlane = alignCanonically(context, alignmentPlane);
                // Swap base and end radius
                const swp = baseRadius;
                baseRadius = endRadius;
                endRadius = swp;
            }
        }
        definitionOut.direction = alignmentPlane.normal;
        definitionOut.axisStart = alignmentPlane.origin;
        definitionOut.startPoint = (alignmentPlane.x * baseRadius) + definitionOut.axisStart;
    }
    else
    {
        definitionOut.direction = geometry.coordSystem.zAxis;
        definitionOut.axisStart = geometry.coordSystem.origin;
        definitionOut.startPoint = (geometry.coordSystem.xAxis * baseRadius) + definitionOut.axisStart;
    }
    definitionOut.spiralPitch = (endRadius - baseRadius) / revolutions;
    return definitionOut;
}


// Manipulator functions

const HEIGHT_MANIPULATOR = "heightManipulator";

function addHelixManipulator(context is Context, id is Id, definition is map, extrudeAxis is Line)
{
    const usedEntities = definition.edge;
    var offset = definition.height;
    if (definition.oppositeDirection)
    {
        offset *= -1;
    }
    addManipulators(context, id, { (HEIGHT_MANIPULATOR) :
                    linearManipulator(extrudeAxis.origin,
                        extrudeAxis.direction,
                        offset,
                        usedEntities) });
}

/**
 * @internal
 * The manipulator change function used in the `helix` feature.
 */
export function helixManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    var newOffset = newManipulators[HEIGHT_MANIPULATOR].offset;
    definition.oppositeDirection = newOffset < 0 * meter;
    definition.height = abs(newOffset);
    return definition;
}


/**
 * Preselection Logic:  Heuristics to determine the type of helix to be constructed, based on user preselection.
 */
export function helixLogic(context is Context, id is Id, oldDefinition is map, definition is map) returns map
{
    // Preselection only
    if (oldDefinition == {})
    {
        const faces = qEntityFilter(definition.initEntities, EntityType.FACE);
        const circles = qGeometry(definition.initEntities, GeometryType.CIRCLE);
        const arcs = qGeometry(definition.initEntities, GeometryType.ARC);
        const edges = qUnion([circles, arcs]);
        if (size(evaluateQuery(context, faces)) == 1)
        {
            definition.helixType = HelixType.TURNS;
            definition.entities = faces;
        }
        else if (size(evaluateQuery(context, edges)) == 1)
        {
            definition.helixType = HelixType.HEIGHT_TURNS;
            definition.edge = edges;
        }
        // Clear out the pre-selection data: this is especially important if the query is to imported data
        definition.initEntities = qNothing();
    }
    return definition;
}

