FeatureScript 2345; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/geomOperations.fs", version : "2345.0");
import(path : "onshape/std/feature.fs", version : "2345.0");
import(path : "onshape/std/valueBounds.fs", version : "2345.0");
import(path : "onshape/std/evaluate.fs", version : "2345.0");
import(path : "onshape/std/manipulator.fs", version : "2345.0");
import(path : "onshape/std/containers.fs", version : "2345.0");
import(path : "onshape/std/string.fs", version : "2345.0");
import(path : "onshape/std/vector.fs", version : "2345.0");
import(path : "onshape/std/path.fs", version : "2345.0");
export import(path : "onshape/std/movecurveboundarytype.gen.fs", version : "2345.0");
export import(path : "onshape/std/offsetcurvetype.gen.fs", version : "2345.0");

/**
 *  Drives the `extend` and `imprint` booleans in opOffsetCurveOnFace.
 */
export enum OffsetCurveScope
{
    annotation { "Name" : "Offset" }
    OFFSET,
    annotation { "Name" : "Offset and extend" }
    OFFSET_AND_EXTEND,
    annotation { "Name" : "Offset, extend, and split" }
    OFFSET_EXTEND_AND_SPLIT
}

/**
 *  Whether to use linear or rounded extensions to connect discontinuities within offset wires.
 */
export enum GapFill
{
    annotation { "Name" : "Linear" }
    LINEAR,
    annotation { "Name" : "Round" }
    ROUND
}

const START_TRIM_MANIPULATOR = "startTrimManipulator";
const END_TRIM_MANIPULATOR = "endTrimManipulator";

/**
 *  Feature performing an offset curve.
 */
annotation { "Feature Type Name" : "Offset curve",
        "UIHint" : UIHint.NO_PREVIEW_PROVIDED,
        "Manipulator Change Function" : "offsetCurveOnFaceManipulators" }
export const offsetCurveOnFace = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Edges", "Filter" : EntityType.EDGE && ConstructionObject.NO && SketchObject.NO && !BodyType.WIRE }
        definition.edges is Query;

        annotation { "Name" : "Distance" }
        isLength(definition.distance, NONNEGATIVE_LENGTH_BOUNDS);

        annotation { "Name" : "Offset type", "UIHint" : UIHint.SHOW_LABEL }
        definition.offsetType is OffsetCurveType;

        annotation { "Name" : "Flip", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Gap fill", "UIHint" : UIHint.SHOW_LABEL }
        definition.gapFill is GapFill;

        annotation { "Name" : "Scope" }
        definition.scope is OffsetCurveScope;

        if (definition.scope == OffsetCurveScope.OFFSET_AND_EXTEND)
        {
            annotation { "Name" : "Trim" }
            definition.trim is boolean;

            if (definition.trim)
            {
                annotation { "Group Name" : "Trim control", "Collapsed By Default" : false, "Driving Parameter" : "trim" }
                {
                    annotation { "Name" : "Start trim" }
                    isLength(definition.startTrim, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

                    annotation { "Name" : "Equal trim" }
                    definition.equalTrim is boolean;

                    if (!definition.equalTrim)
                    {
                        annotation { "Name" : "End trim" }
                        isLength(definition.endTrim, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
                    }
                }
            }
        }

        annotation { "Name" : "Targets", "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO && ModifiableEntityOnly.YES }
        definition.targets is Query;
    }
    {
        var extend = false;
        var imprint = false;
        if (definition.scope != OffsetCurveScope.OFFSET)
        {
            extend = true;
        }
        if (definition.scope == OffsetCurveScope.OFFSET_EXTEND_AND_SPLIT)
        {
            imprint = true;
        }
        var roundedCorners = false;
        if (definition.gapFill == GapFill.ROUND)
        {
            roundedCorners = true;
        }
        if (definition.equalTrim)
        {
            definition.endTrim = definition.startTrim;
        }
        const isAnyTrimmingDone = definition.scope == OffsetCurveScope.OFFSET_AND_EXTEND && definition.trim && (definition.startTrim > TOLERANCE.zeroLength * meter || definition.endTrim > TOLERANCE.zeroLength * meter);

        @opOffsetCurveOnFace(context, id, {
                    "edges" : definition.edges,
                    "oppositeDirection" : definition.oppositeDirection,
                    "imprint" : imprint,
                    "extend" : extend,
                    "distance" : definition.distance,
                    "offsetType" : definition.offsetType,
                    "targets" : definition.targets,
                    "roundedCorners" : roundedCorners,
                    "displayResults" : !isAnyTrimmingDone
                });

        const wires = evaluateQuery(context, qCreatedBy(id, EntityType.BODY));
        if (definition.scope == OffsetCurveScope.OFFSET_AND_EXTEND && definition.trim && size(wires) > 0)
        {
            // We need to separate the first wire from the rest in order to place manipulators even if the trim distances are 0.
            var trimPositions = getTrimPositionsAndDisplayTrimManipulators(context, id, wires[0], definition.startTrim, definition.endTrim, definition.equalTrim);
            var startTrimPosition = trimPositions[0];
            var endTrimPosition = trimPositions[1];
            var helpPointPosition = trimPositions[2];

            if (definition.startTrim > TOLERANCE.zeroLength * meter || definition.endTrim > TOLERANCE.zeroLength * meter)
            {
                doOneTrim(context, id, 0, wires[0], [startTrimPosition, endTrimPosition], helpPointPosition);

                for (var i = 1; i < size(wires); i += 1)
                {

                    trimPositions = getTrimPositions(context, wires[i], definition.startTrim, definition.endTrim);
                    startTrimPosition = trimPositions[0];
                    endTrimPosition = trimPositions[1];
                    helpPointPosition = trimPositions[2];
                    doOneTrim(context, id, i, wires[i], [startTrimPosition, endTrimPosition], helpPointPosition);
                }
            }
        }
    },
    {
            isZeroLength : false,
            oppositeDirection : false,
            gapFill : GapFill.LINEAR,
            scope : OffsetCurveScope.OFFSET,
            trim : false,
            startTrim : 0 * inch,
            endTrim : 0 * inch,
            equalTrim : false,
            targets : qNothing()
        });

function doOneTrim(context is Context, id is Id, index is number, wire is Query, positions is array, helpPoint is Vector)
{
    const trimId = id + "trim" + toString(index);
    opMoveCurveBoundary(context, trimId, {
                "wires" : wire,
                "moveBoundaryType" : MoveCurveBoundaryType.TRIM,
                "trimToPoints" : positions,
                "helpPointPosition" : helpPoint
            });
    processSubfeatureStatus(context, id, {
                "subfeatureId" : trimId,
                "propagateErrorDisplay" : true
            });
}

function getTrimPositions(context is Context, wire is Query, startLength is ValueWithUnits, endLength is ValueWithUnits) returns array
{
    return getTrimPositionsAndDisplayTrimManipulators(context, newId(), wire, startLength, endLength, false);
}

// returns [startTrimPosition, endTrimPosition, helpPointPosition]
function getTrimPositionsAndDisplayTrimManipulators(context is Context, id is Id, wire is Query, startLength is ValueWithUnits, endLength is ValueWithUnits, equalTrim is boolean) returns array
{
    const paths = constructPaths(context, qOwnedByBody(wire, EntityType.EDGE), {});
    if (size(paths) != 1)
    {
        throw regenError(ErrorStringEnum.TRIM_FAILED);
    }
    const path = paths[0];
    if (path.closed)
    {
        throw regenError(ErrorStringEnum.OFFSET_CURVE_ON_FACE_CLOSED_CURVE_NO_TRIM, wire);
    }
    const length = evPathLength(context, path);
    // BEL-217386: If startLength + endLength == length we get a bug where sometimes we don't fail and the end doesn't actually get trimmed.
    const tolerance = (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2328_OFFSET_CURVE_FIX_TRIM_TOLERANCE) ? TOLERANCE.zeroLength : 0) * meter;
    const trimFail = startLength + endLength > length - tolerance;
    var parameters;
    if (trimFail)
    {
        if (id == newId())
        {
            throw regenError(ErrorStringEnum.TRIM_FAILED, wire);
        }
        parameters = [min(startLength / length, 1), max(1 - endLength / length, 0)];
    }
    else
    {
        parameters = [startLength / length, 1 - endLength / length, (startLength + (length - startLength - endLength) / 2) / length];
    }
    const lines = evPathTangentLines(context, path, parameters).tangentLines;

    if (id != newId())
    {
        addManipulators(context, id, { (START_TRIM_MANIPULATOR) :
                    linearManipulator({
                            "base" : lines[0].origin + max(0 * meter, startLength - length) * lines[0].direction,
                            "direction" : lines[0].direction,
                            "offset" : 0 * meter,
                            "style" : ManipulatorStyleEnum.TANGENTIAL,
                            "primaryParameterId" : "startTrim"
                        })
                });
        if (!equalTrim)
        {
            addManipulators(context, id, { (END_TRIM_MANIPULATOR) :
                        linearManipulator({
                                "base" : lines[1].origin + min(0 * meter, length - endLength) * lines[1].direction,
                                "direction" : lines[1].direction,
                                "offset" : 0 * meter,
                                "style" : ManipulatorStyleEnum.TANGENTIAL,
                                "primaryParameterId" : "endTrim"
                            })
                    });
        }
    }

    if (trimFail)
    {
        throw regenError(ErrorStringEnum.TRIM_FAILED, { "entities" : wire, "faultyParameters" : ["startTrim", "endTrim"] });
    }

    return mapArray(lines, function(line)
        {
            return line.origin;
        });
}

/**
 * @internal
 * The manipulator change function used in the `offsetCurveOnFace` feature.
 */
export function offsetCurveOnFaceManipulators(context is Context, definition is map, newManipulators is map) returns map
{
    if (newManipulators[START_TRIM_MANIPULATOR] is map)
    {
        const manipulator = newManipulators[START_TRIM_MANIPULATOR];
        definition.startTrim = max(definition.startTrim + manipulator.offset, 0 * meter);
    }
    if (newManipulators[END_TRIM_MANIPULATOR] is map)
    {
        const manipulator = newManipulators[END_TRIM_MANIPULATOR];
        definition.endTrim = max(definition.endTrim - manipulator.offset, 0 * meter);
    }
    return definition;
}

