FeatureScript 3083; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/common.fs", version : "3083.0");
import(path : "onshape/std/feature.fs", version : "3083.0");
import(path : "onshape/std/query.fs", version : "3083.0");
export import(path : "onshape/std/sheetMetalStart.fs", version : "3083.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "3083.0");

/**
 * Initializes a Flex PCB model by converting existing parts, extruding sketch curves, or thickening the selected faces or sketch regions.
 * All operations on an active PCB model will automatically be represented in the flat pattern and the table.
 * PCB models may consist of multiple parts. Multiple PCB models can be active.
 */
annotation { "Feature Type Name" : "Flex PCB model",
             "Filter Selector" : "allparts",
             "Manipulator Change Function" : "pcbZoneStartManipulatorChange",
             "Editing Logic Function" : "pcbStartEditLogic" }
export const pcbStart = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        // Hidden parameter which collects the preselection so that `pcbStartEditLogic` can route it to the
        // parameter matching the preselected entity type. It must be the first query parameter in the
        // precondition, since the client offers the preselection to the first query parameter of the feature.
        annotation { "Name" : "Entities", "UIHint" : UIHint.ALWAYS_HIDDEN,
                     "Filter" : (EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO) ||
                                ((EntityType.FACE || EntityType.EDGE) && SketchObject.YES && ConstructionObject.NO) }
        definition.initEntities is Query;

        annotation { "Name" : "Process", "UIHint" : UIHint.HORIZONTAL_ENUM }
        definition.process is SMProcessType;

        annotation { "Group Name" : "Selections", "Collapsed By Default" : false }
        {
            if (definition.process == SMProcessType.CONVERT)
            {
                annotation { "Name" : "Parts and surfaces to convert",
                            "Filter" : EntityType.BODY && (BodyType.SOLID || BodyType.SHEET) && SketchObject.NO && ConstructionObject.NO && AllowMeshGeometry.YES }
                definition.partToConvert is Query;

                annotation { "Name" : "Faces to exclude", "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO && AllowMeshGeometry.YES }
                definition.facesToExclude is Query;
            }
            else if (definition.process == SMProcessType.EXTRUDE)
            {
                smExtrudeParameters(definition);
            }
            else if (definition.process == SMProcessType.THICKEN)
            {
                annotation { "Name" : "Faces or sketch regions to thicken",
                            "Filter" : ConstructionObject.NO && (GeometryType.PLANE || GeometryType.CYLINDER || GeometryType.EXTRUDED || GeometryType.CONE) }
                definition.regions is Query;

                annotation { "Name" : "Tangent propagation", "Default" : false }
                definition.tangentPropagation is boolean;
            }

            if (definition.process == SMProcessType.THICKEN || definition.process == SMProcessType.CONVERT)
            {
                annotation { "Name" : "Edges or cylinders to bend",
                             "Filter" : ((EntityType.EDGE && EdgeTopology.TWO_SIDED && GeometryType.LINE) ||
                                         (EntityType.FACE && GeometryType.CYLINDER)) && SketchObject.NO }
                definition.bends is Query;

                annotation { "Name" : "Clearance from input" }
                isLength(definition.clearance, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

                annotation { "Name" : "Include bends", "Description" : "Check to include the clearance for bends" }
                definition.bendsIncluded is boolean;
            }

            if (definition.process == SMProcessType.CONVERT)
            {
                annotation { "Name" : "Keep input parts" }
                definition.keepInputParts is boolean;
            }
        }

        smGeneralParameters(definition);
    }
    {
        if (definition.process == SMProcessType.EXTRUDE)
        {
            const sketchCurves = qConstructionFilter(qUnion([definition.bendArcs, definition.sketchCurves]), ConstructionObject.NO);
            const resolvedEntities = evaluateQuery(context, sketchCurves);
            if (size(resolvedEntities) > 0)
            {
                const manipulatorDefinition = adjustExtrudeDirectionForBlind(definition);
                const tangentAtEdge = evEdgeTangentLine(context, { "edge" : resolvedEntities[0], "parameter" : 0.5 });
                const entityPlane = evOwnerSketchPlane(context, { "entity" : resolvedEntities[0] });
                const extrudeAxis = line(tangentAtEdge.origin, entityPlane.normal);
                addExtrudeManipulator(context, id, manipulatorDefinition, sketchCurves, extrudeAxis, false);
            }
        }

        callSubfeatureAndProcessStatusSameParameters(id, sheetMetalStart, context, id + "sheetMetalStart", mergeMaps(definition, {
            "kFactor" : 0,
            "kFactorRolled": 0,
            "smApplicationType" : SMApplicationType.FLEXIBLE_PCB
        }));

        const resultSheetBodies = qCreatedBy(id + "sheetMetalStart", EntityType.BODY);
        addFlipDirectionUpManipulator(resultSheetBodies, FLIP_DIRECTION_UP_MANIPULATOR_NAME, id, context, definition);
    }, {
        "initEntities" : qNothing(),
        "process" : SMProcessType.THICKEN,
        "oppositeDirection" : false,
        "flipDirectionUp" : false,
        "tangentPropagation" : false,
        "bendsIncluded" : false,
        "keepInputParts" : false,
        "clearance" : 0 * meter,
        "bends" : qNothing(),
        "oppositeExtrudeDirection" : false,
        "hasSecondDirection" : false,
        "symmetric" : false,
        "defaultBendReliefStyle": SMBendStrategyType.TEAR
    });

/**
 * @internal
 */
export function pcbStartEditLogic(context is Context, id is Id, oldDefinition is map, definition is map) returns map
{
    // Preselection processing. `oldDefinition` is empty only on the first call, when the dialog is opened.
    if (oldDefinition == {})
    {
        const bodies = qEntityFilter(definition.initEntities, EntityType.BODY);
        const faces = qEntityFilter(definition.initEntities, EntityType.FACE);
        const edges = qModifiableEntityFilter(qEntityFilter(definition.initEntities, EntityType.EDGE));
        if (!isQueryEmpty(context, bodies))
        {
            definition.partToConvert = bodies;
            definition.process = SMProcessType.CONVERT;
        }
        else if (!isQueryEmpty(context, faces))
        {
            definition.regions = faces;
            definition.process = SMProcessType.THICKEN;
        }
        else if (!isQueryEmpty(context, edges))
        {
            definition.sketchCurves = edges;
            definition.process = SMProcessType.EXTRUDE;
        }
        // Clear out the pre-selection data: this is especially important if the query is to imported data
        definition.initEntities = qNothing();
    }

    return definition;
}

/**
 * @internal
 */
export function pcbZoneStartManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    for (var manipulator in newManipulators)
    {
        if (manipulator.key == FLIP_DIRECTION_UP_MANIPULATOR_NAME)
        {
            definition.flipDirectionUp = manipulator.value.flipped;
            return definition;
        }
        else
        {
            return extrudeManipulatorChange(context, definition, newManipulators);
        }
    }
    return definition;
}

