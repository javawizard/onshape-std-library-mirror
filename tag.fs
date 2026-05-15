FeatureScript 2960; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "2960.0");
import(path : "onshape/std/containers.fs", version : "2960.0");
import(path : "onshape/std/coordSystem.fs", version : "2960.0");
import(path : "onshape/std/debug.fs", version : "2960.0");
import(path : "onshape/std/defaultFeatures.fs", version : "2960.0");
import(path : "onshape/std/error.fs", version : "2960.0");
import(path : "onshape/std/evaluate.fs", version : "2960.0");
import(path : "onshape/std/feature.fs", version : "2960.0");
import(path : "onshape/std/featureList.fs", version : "2960.0");
import(path : "onshape/std/formedUtils.fs", version : "2960.0");
import(path : "onshape/std/frameAttributes.fs", version : "2960.0");
import(path : "onshape/std/frameUtils.fs", version : "2960.0");
import(path : "onshape/std/string.fs", version : "2960.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2960.0");
import(path : "onshape/std/units.fs", version : "2960.0");
import(path : "onshape/std/vector.fs", version : "2960.0");

/**
 * Defines the kind of entity being tagged in the feature.
 * @value FRAME : Tag a frame profile with metadata. The metadata will be displayed in the cut list for frames derived from the
 * tagged profile.
 * @value FORM : Tag a form with metadata. The metadata will be used for formed features derived from the form.
 * @value PCB_HOLES : Tag holes or cutouts with ECAD metadata. The metadata will be used for importing to PCB Studio.
 */
export enum TagPurpose
{
    annotation { "Name" : "Frame" }
    FRAME,
    annotation { "Name" : "Form" }
    FORM,
    annotation { "Name" : "PCB holes" }
    PCB_HOLES
}

/**
 * Type of PCB entities being tagged by a Tag feature.
 * @value HOLE : Entities will be imported as holes in PCB Studio.
 * @value CUTOUT : Entities will be imported as cutouts in PCB Studio.
 */
export enum PcbEntityType
{
    annotation { "Name" : "Holes" }
    HOLE,
    annotation { "Name" : "Cutouts" }
    CUTOUT
}

/**
 * Predefined purpose values for a plated PCB hole tagged by a Tag feature.
 * @value NONE : No purpose specified.
 * @value VIA : Hole is associated with a conductive via.
 * @value THERMAL_VIA : Hole is associated with a thermal via.
 * @value PIN : Hole is associated with a component pin.
 * @value CUSTOM : User-defined purpose.
 */
export enum PcbPlatedHolePurpose
{
    annotation { "Name" : "None" }
    NONE,
    annotation { "Name" : "Via" }
    VIA,
    annotation { "Name" : "Thermal via" }
    THERMAL_VIA,
    annotation { "Name" : "Pin" }
    PIN,
    annotation { "Name" : "Custom" }
    CUSTOM
}

/**
 * Predefined purpose values for a non-plated PCB hole tagged by a Tag feature.
 * @value NONE : No purpose specified.
 * @value MOUNTING : Hole is used for mounting purposes.
 * @value TOOLING : Hole is used for tooling purposes.
 * @value CUSTOM : User-defined purpose.
 */
export enum PcbNonPlatedHolePurpose
{
    annotation { "Name" : "None" }
    NONE,
    annotation { "Name" : "Mounting" }
    MOUNTING,
    annotation { "Name" : "Tooling" }
    TOOLING,
    annotation { "Name" : "Custom" }
    CUSTOM
}

/**
 * Owner of a PCB hole tagged by a Tag feature.
 * @value UNOWNED : Hole can be modified in either system.
 * @value ECAD : Hole is owned by the Electrical system and should not be modified in the Mechanical system.
 * @value MCAD : Hole is owned by the Mechanical system and should not be modified in the Electrical system.
 */
export enum PcbHoleOwner
{
    annotation { "Name" : "Unowned" }
    UNOWNED,
    annotation { "Name" : "ECAD" }
    ECAD,
    annotation { "Name" : "MCAD" }
    MCAD
}

/**
 * @internal
 * Name of the attribute tagging PCB Studio holes
 */
export const PCB_HOLE_ATTRIBUTE_NAME = "pcbStudioHole";

/**
 * @internal
 * Type of the attribute tagging PCB Studio holes
 */
export type PcbHoleAttribute typecheck canBePcbHoleAttribute;

/**
 * @internal
 * Type check for PcbHoleAttribute
 */
export predicate canBePcbHoleAttribute(value)
{
    value is map;
    value.holeId is string;
    value.designator is string;
    value.componentDesignator is string;
    value.pcbEntityType is PcbEntityType;
    value.plated is boolean;
    value.owner is PcbHoleOwner;
    value.purpose is string;
    value.startFace is Query;
    value.endFace is Query;
}

/**
 * Define Tag feature parameters when the tag purpose is FRAME.
 */
predicate defineFrameTagParams(definition is map)
{
    annotation { "Name" : "Sketch profile", "MaxNumberOfPicks" : 1, "UIHint" : UIHint.NO_QUERY_VARIABLE }
    definition.sketch is FeatureList;

    annotation { "Group Name" : "Additional alignment points", "Collapsed By Default" : true }
    {
        annotation { "Name" : "Sketch points", "Filter" : EntityType.VERTEX && SketchObject.YES }
        definition.alignmentPoints is Query;
    }

    // Standard and Description must appear as explicit fields because array parameter fields are not configurable
    // (for now - BEL-85948), and these fields must be configurable in order for us to construct the standard
    // library of profiles.
    annotation { "Name" : "Standard" }
    definition.standard is string;

    annotation { "Name" : "Description" }
    definition.description is string;

    annotation {
                "Name" : "Additional columns",
                "Item name" : "Column",
                "Item label template" : "#header = #value",
                "UIHint" : UIHint.PREVENT_ARRAY_REORDER,
                "showDefaultAlignmentPoints" : true
            }
    definition.additionalColumns is array;
    for (var column in definition.additionalColumns)
    {
        annotation { "Name" : "Header" }
        column.header is string;

        annotation { "Name" : "Value" }
        column.value is string;
    }

    annotation { "Name" : "Show default points", "Default" : true, "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
    definition.showDefaultAlignmentPoints is boolean;
}

/**
 * Define Tag feature parameters when the tag purpose is FORM.
 */
predicate defineFormTagParams(definition is map)
{
    annotation { "Name" : "Part to add", "Filter" : EntityType.BODY && BodyType.SOLID, "MaxNumberOfPicks" : 1 }
    definition.positivePart is Query;

    annotation { "Name" : "Part to remove", "Filter" : EntityType.BODY && BodyType.SOLID, "MaxNumberOfPicks" : 1 }
    definition.negativePart is Query;

    annotation { "Name" : "Sketch for flat view" }
    definition.flatFormSketch is FeatureList;

    annotation { "Name" : "Form origin mate connector", "Description" : "If none selected, this feature will create one at Origin",
                 "Filter" : BodyType.MATE_CONNECTOR, "MaxNumberOfPicks" : 1 }
    definition.cSysMateConnector is Query;
}

/**
 * Define Tag feature parameters when the tag purpose is PCB_HOLES.
 */
predicate definePcbHolesTagParams(definition is map)
{
    annotation { "Name" : "PCB entity type", "UIHint" : [UIHint.SHOW_LABEL] }
    definition.pcbEntityType is PcbEntityType;

    annotation { "Name" : "Plated" }
    definition.plated is boolean;

    annotation { "Name" : "Base plane", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1 }
    definition.basePlane is Query;

    annotation { "Name" : "Holes", "Item name" : "Hole", "Item label template" : "Hole #designator",
                 "UIHint" : [UIHint.COLLAPSE_ARRAY_ITEMS, UIHint.INITIAL_FOCUS_ON_EDIT],
                 "Driven query" : "interiorFaces" }
    definition.holes is array;

    for (var hole in definition.holes)
    {
        annotation { "Name" : "Interior faces", "Filter" : EntityType.FACE && BodyType.SOLID }
        hole.interiorFaces is Query;

        annotation { "Name" : "Hole ID", "UIHint" : [UIHint.ALWAYS_HIDDEN] }
        hole.holeId is string;

        annotation { "Name" : "Designator" }
        hole.designator is string;

        if (definition.pcbEntityType == PcbEntityType.HOLE)
        {
            if (definition.plated)
            {
                annotation { "Name" : "Purpose", "Default" : "VIA" }
                hole.platedPurpose is PcbPlatedHolePurpose;
            }
            else
            {
                annotation { "Name" : "Purpose" }
                hole.nonPlatedPurpose is PcbNonPlatedHolePurpose;
            }
        }

        if (definition.pcbEntityType == PcbEntityType.CUTOUT
            || (definition.pcbEntityType == PcbEntityType.HOLE && definition.plated && hole.platedPurpose == PcbPlatedHolePurpose.CUSTOM)
            || (definition.pcbEntityType == PcbEntityType.HOLE && !definition.plated && hole.nonPlatedPurpose == PcbNonPlatedHolePurpose.CUSTOM))
        {
            annotation { "Name" : "Purpose", "MinLength" : 1 }
            hole.customPurpose is string;
        }

        if (definition.pcbEntityType == PcbEntityType.HOLE)
        {
            annotation { "Name" : "Associated component instance" }
            hole.componentDesignator is string;
        }

        annotation { "Name" : "Owner", "UIHint" : [UIHint.SHOW_LABEL] }
        hole.holeOwner is PcbHoleOwner;
    }
}

/**
 * Tag an entity with metadata. The metadata will be used for formed and frame features or imported to PCB Studio.
 */

annotation { "Feature Type Name" : "Tag" }
export const tag = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Tag purpose", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        definition.tagPurpose is TagPurpose;

        if (definition.tagPurpose == TagPurpose.FRAME)
        {
            defineFrameTagParams(definition);
        }
        else if (definition.tagPurpose == TagPurpose.FORM)
        {
            defineFormTagParams(definition);
        }
        else if (definition.tagPurpose == TagPurpose.PCB_HOLES)
        {
            definePcbHolesTagParams(definition);
        }
    }
    {
        if (definition.tagPurpose == TagPurpose.FRAME)
        {
            doTagProfile(context, id, definition);
        }
        else if (definition.tagPurpose == TagPurpose.FORM)
        {
            doTagForm(context, id, definition);
        }
        else if (definition.tagPurpose == TagPurpose.PCB_HOLES)
        {
            doTagPcbHoles(context, id, definition);
        }
    }, {
            tagPurpose : TagPurpose.FRAME,
            standard : "",
            description : "",
            additionalColumns : [],
            alignmentPoints : qNothing(),
            showDefaultAlignmentPoints : false,
            positivePart : qNothing(),
            negativePart : qNothing(),
            cSysMateConnector : qNothing(),
            pcbEntityType : PcbEntityType.HOLE,
            plated : false,
            basePlane : qTopPlane(EntityType.FACE),
            holes : []
        });

function doTagForm(context is Context, topLevelId is Id, definition is map)
{
    verify(!isInFeaturePattern(context), ErrorStringEnum.FORMED_TAG_FORM_NO_FEATURE_PATTERN);

    const bodiesWithFormAttribute = qBodiesWithFormAttributes(qEverything(EntityType.BODY), [FORM_BODY_POSITIVE_PART,
                                        FORM_BODY_NEGATIVE_PART, FORM_BODY_SKETCH_FOR_FLAT_VIEW, FORM_BODY_CSYS_MATE_CONNECTOR]);
    if (!isQueryEmpty(context, bodiesWithFormAttribute))
    {
        throw regenError(ErrorStringEnum.FORMED_TAG_FORM_BODIES_ALREADY_TAGGED, bodiesWithFormAttribute);
    }

    var positivePartSelected = !isQueryEmpty(context, definition.positivePart);
    if (positivePartSelected)
    {
        if (isQueryEmpty(context, qBodyType(definition.positivePart, BodyType.SOLID)))
        {
            throw regenError(ErrorStringEnum.FORMED_TAG_FORM_POSITIVE_PART_NOT_SOLID, ["positivePart"], definition.positivePart);
        }
        else if (!isQueryEmpty(context, qConsumed(definition.positivePart, Consumed.YES)))
        {
            throw regenError(ErrorStringEnum.FORMED_TAG_FORM_POSITIVE_PART_CONSUMED, ["positivePart"], definition.positivePart);
        }
    }
    var negativePartSelected = !isQueryEmpty(context, definition.negativePart);
    if (negativePartSelected)
    {
        if (isQueryEmpty(context, qBodyType(definition.negativePart, BodyType.SOLID)))
        {
            throw regenError(ErrorStringEnum.FORMED_TAG_FORM_NEGATIVE_PART_NOT_SOLID, ["negativePart"], definition.negativePart);
        }
        else if (!isQueryEmpty(context, qConsumed(definition.negativePart, Consumed.YES)))
        {
            throw regenError(ErrorStringEnum.FORMED_TAG_FORM_NEGATIVE_PART_CONSUMED, ["negativePart"], definition.negativePart);
        }
    }
    if (positivePartSelected && negativePartSelected &&
        !isQueryEmpty(context, qIntersection(definition.positivePart, definition.negativePart)))
    {
        throw regenError(ErrorStringEnum.FORMED_TAG_FORM_SELECT_DIFFERENT_PARTS, ["positivePart", "negativePart"],
                         qIntersection(definition.positivePart, definition.negativePart));
    }

    if (!positivePartSelected && !negativePartSelected)
    {
        throw regenError(ErrorStringEnum.FORMED_TAG_FORM_SELECT_SOMETHING);
    }

    var nSketchSelected = size(definition.flatFormSketch);
    if (nSketchSelected != 0)
    {
        if (nSketchSelected > 1)
        {
            throw regenError(ErrorStringEnum.FORMED_TAG_FORM_SELECT_SKETCH, ["flatFormSketch"]);
        }
        else if (isQueryEmpty(context, qCreatedBy(definition.flatFormSketch)->qBodyType([BodyType.WIRE, BodyType.POINT])->qSketchFilter(SketchObject.YES)))
        {
            throw regenError(ErrorStringEnum.FORMED_TAG_FORM_SELECT_SKETCH_WITH_WIRE_POINT, ["flatFormSketch"]);
        }
    }

    if (positivePartSelected)
    {
        setFormAttribute(context, definition.positivePart, FORM_BODY_POSITIVE_PART);
    }
    if (negativePartSelected)
    {
        setFormAttribute(context, definition.negativePart, FORM_BODY_NEGATIVE_PART);
    }
    if (nSketchSelected != 0)
    {
        setFormAttribute(context, qCreatedBy(definition.flatFormSketch, EntityType.BODY)->qBodyType([BodyType.WIRE, BodyType.POINT]), FORM_BODY_SKETCH_FOR_FLAT_VIEW);
    }

    var cSysMateConnector = definition.cSysMateConnector;
    if (isQueryEmpty(context, cSysMateConnector))
    {
        const originMateConnectorId = topLevelId + "originMateConnector";
        opMateConnector(context, originMateConnectorId, { "coordSystem" : WORLD_COORD_SYSTEM });
        cSysMateConnector = qCreatedBy(originMateConnectorId, EntityType.BODY)->qBodyType(BodyType.MATE_CONNECTOR);
    }
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2591_WARN_FORM_ORIGIN_OUTSIDE_TOOLS_BBOX))
    {
        const toolsBoundingBox = evBox3d(context, { "topology" : qUnion(definition.positivePart, definition.negativePart), "tight" : false });
        const formCSys = evMateConnector(context, { "mateConnector" : cSysMateConnector });
        if (!insideBox3d(formCSys.origin, toolsBoundingBox))
        {
            reportFeatureWarning(context, topLevelId, ErrorStringEnum.FORMED_TAG_FORM_ORIGIN_OUTSIDE_TOOLS_BBOX, ["cSysMateConnector"]);
        }
    }
    setFormAttribute(context, qOwnerBody(cSysMateConnector), FORM_BODY_CSYS_MATE_CONNECTOR);
}


function doTagProfile(context is Context, topLevelId is Id, definition is map)
{
    verify(!isInFeaturePattern(context), ErrorStringEnum.FRAME_TAG_PROFILE_NO_FEATURE_PATTERN);

    const sketchRegions = qCreatedBy(definition.sketch, EntityType.FACE)->qSketchFilter(SketchObject.YES);
    if (size(definition.sketch) != 1 || isQueryEmpty(context, sketchRegions))
    {
        throw regenError(ErrorStringEnum.FRAME_TAG_PROFILE_SELECT_SKETCH, ["instanceFunction"]);
    }

    var profileAttribute = {};
    if (definition.standard != "")
    {
        profileAttribute[CUTLIST_STANDARD] = definition.standard;
    }
    if (definition.description != "")
    {
        profileAttribute[CUTLIST_DESCRIPTION] = definition.description;
    }
    for (var index, column in definition.additionalColumns)
    {
        const faultyHeader = { "faultyParameters" : [faultyArrayParameterId("additionalColumns", index, "header")] };
        const faultyValue = { "faultyParameters" : [faultyArrayParameterId("additionalColumns", index, "value")] };
        verify(column.header != "", ErrorStringEnum.FRAME_TAG_PROFILE_HEADER_EMPTY, faultyHeader);
        verify(column.value != "", ErrorStringEnum.FRAME_TAG_PROFILE_VALUE_EMPTY, faultyValue);

        const headerIsReserved = (column.header == CUTLIST_STANDARD || column.header == CUTLIST_DESCRIPTION);
        verify(!headerIsReserved, ErrorStringEnum.FRAME_TAG_PROFILE_HEADER_RESERVED, faultyHeader);

        profileAttribute[column.header] = column.value;
    }

    setFrameProfileAttribute(context, sketchRegions, frameProfileAttribute(profileAttribute));
    displayAlignmentPoints(context, sketchRegions, definition.showDefaultAlignmentPoints);
    doTagCustomAlignmentPoints(context, topLevelId, definition);
}

function doTagCustomAlignmentPoints(context is Context, topLevelId is Id, definition is map)
{
    const allPointsQuery = qCreatedBy(definition.sketch)->qEntityFilter(EntityType.VERTEX);
    const goodPointsQuery = qIntersection([allPointsQuery, definition.alignmentPoints]);
    const badPointsQuery = qSubtraction(definition.alignmentPoints, goodPointsQuery);
    verify(isQueryEmpty(context, badPointsQuery), ErrorStringEnum.FRAME_CUSTOM_ALIGNMENT_POINTS_NOT_IN_SKETCH, { "entities" : badPointsQuery });
    setCustomFrameAlignmentPointAttribute(context, goodPointsQuery);
}

// This is only for visualization so it returns silently in event of error.
function displayAlignmentPoints(context is Context, sketchRegions is Query, showPoints is boolean)
{

    if (!showPoints || !isAtVersionOrLater(context, FeatureScriptVersionNumber.V2442_USE_BOUNDING_BOX_CENTER))
    {
        return;
    }

    try silent
    {
        const sketchPlane = evOwnerSketchPlane(context, {
                    "entity" : sketchRegions
                });
        const sketchPlaneCS = coordSystem(sketchPlane);
        const offsetData = getAlignmentPointOffsetData(context, sketchRegions, sketchPlaneCS);
        for (var offset in offsetData.offsets)
        {
            // offsets are in bounding box coordinate system
            // so for display we have to transform them in to sketch CS
            const offsetPointInPlane3d = offset + offsetData.centerOffset;
            addDebugPoint(context, toWorld(sketchPlaneCS, offsetPointInPlane3d), DebugColor.MAGENTA);
        }
    }
    catch
    {
        return;
    }
}

function doTagPcbHoles(context is Context, topLevelId is Id, definition is map)
{
    var ids = {};

    verifyNonemptyQuery(context, definition, "basePlane", ErrorStringEnum.PCB_HOLE_NO_BASE_PLANE);
    const basePlane = context->evPlane({
            "face" : definition.basePlane
    });
    var boardRegions = {};

    // Map of ErrorStringEnum to box<{faultyParameters, entities}> to report all holes in certain error states at once
    const errorToDetails = new box({});
    const addError = function(error is ErrorStringEnum, parameterId is string, entities is Query)
    {
        if (errorToDetails[][error] == undefined)
        {
            errorToDetails[][error] = {
                "faultyParameters" : [],
                "entities" : []
            };
        }
        errorToDetails[][error].faultyParameters = errorToDetails[][error].faultyParameters->append(parameterId);
        errorToDetails[][error].entities = errorToDetails[][error].entities->append(entities);
    };

    const idStr = topLevelId->join("_");
    for (var i, hole in definition.holes)
    {
        if (context->isQueryEmpty(hole.interiorFaces))
        {
            addError(ErrorStringEnum.PCB_HOLE_NO_INTERIOR_FACE, faultyArrayParameterId("holes", i, "interiorFaces"), qNothing());
            continue;
        }

        const holeId = !isUndefinedOrEmptyString(hole.holeId)
            ? hole.holeId
            : "Onshape_hole_" ~ idStr ~ "_" ~ i;
        if (ids[holeId] == true)
        {
            throw regenError(ErrorStringEnum.PCB_HOLE_DUPLICATE_HOLE_ID ~ " " ~ hole.holeId);
        }
        ids[holeId] = true;

        // Make sure the body has 2 flat faces parallel to the base plane, these are the start/end faces of all holes in the region
        const boardRegion = context->evaluateQuery(qOwnerBody(hole.interiorFaces));
        if (size(boardRegion) != 1)
        {
            throw regenError(ErrorStringEnum.PCB_HOLE_INTERIOR_FACES_MUST_BELONG_TO_SAME_BODY, [faultyArrayParameterId("holes", i, "interiorFaces")], hole.interiorFaces);
        }

        const region = boardRegion[0];
        var regionInfo = boardRegions[region];
        if (regionInfo == undefined)
        {
            const parallelFaces = qOwnedByBody(region, EntityType.FACE)->qParallelPlanes(basePlane);
            const parallelFaceCount = context->evaluateQueryCount(parallelFaces);

            if (parallelFaceCount == 0)
            {
                throw regenError(ErrorStringEnum.PCB_HOLE_LESS_THAN_2_PARALLEL_FACES, [faultyArrayParameterId("holes", i, "interiorFaces")]);
            }
            if (parallelFaceCount == 1)
            {
                throw regenError(ErrorStringEnum.PCB_HOLE_LESS_THAN_2_PARALLEL_FACES, [faultyArrayParameterId("holes", i, "interiorFaces")], parallelFaces);
            }
            if (parallelFaceCount > 2)
            {
                reportFeatureWarning(context, topLevelId, ErrorStringEnum.PCB_HOLE_MORE_THAN_2_PARALLEL_FACES, [faultyArrayParameterId("holes", i, "interiorFaces")]);
            }

            regionInfo = {
                startFace: parallelFaces->qFarthestAlong(-basePlane.normal),
                endFace: parallelFaces->qFarthestAlong(basePlane.normal)
            };

            // Find the outside edge of the start face;
            // the face on the other side of that edge from the start face is on the outside of the part
            const edgeOnOuterLoop = getEdgeOnOuterLoop(context, regionInfo.startFace);
            const faceOnOutside = regionInfo.startFace->qAdjacent(AdjacencyType.EDGE, EntityType.FACE)
                ->qIntersection(edgeOnOuterLoop->qAdjacent(AdjacencyType.EDGE, EntityType.FACE));

            // Save all the outer-loop faces of the part - these aren't allowed to be tagged as they're part of the outline
            regionInfo.outerFaces = qFaceOrEdgeBoundedFaces(qUnion([faceOnOutside, regionInfo.startFace, regionInfo.endFace]));

            boardRegions[region] = regionInfo;
        }

        const allInteriorFaces = qFaceOrEdgeBoundedFaces(qUnion([hole.interiorFaces->qNthElement(0), regionInfo.startFace, regionInfo.endFace]));

        if (!context->isQueryEmpty(qIntersection(allInteriorFaces, regionInfo.outerFaces)))
        {
            addError(ErrorStringEnum.PCB_HOLE_FACE_ON_OUTLINE_OF_REGION, faultyArrayParameterId("holes", i, "interiorFaces"), hole.interiorFaces);
            continue;
        }

        // Make sure all selected faces are part of the same hole
        if (!context->areQueriesEquivalent(qIntersection(allInteriorFaces, hole.interiorFaces), hole.interiorFaces))
        {
            throw regenError(ErrorStringEnum.PCB_HOLE_INTERIOR_FACES_MUST_BELONG_TO_SAME_HOLE, [faultyArrayParameterId("holes", i, "interiorFaces")], hole.interiorFaces);
        }

        // Make sure no face in the hole is already tagged as a different hole
        const alreadyTagged = allInteriorFaces->qHasAttribute(PCB_HOLE_ATTRIBUTE_NAME);
        if (!context->isQueryEmpty(alreadyTagged))
        {
            addError(ErrorStringEnum.PCB_HOLE_ALREADY_TAGGED, faultyArrayParameterId("holes", i, "interiorFaces"), hole.interiorFaces);
            continue;
        }

        // Tag all interior faces with the hole info
        context->setAttribute({
                "entities" : allInteriorFaces,
                "name" : PCB_HOLE_ATTRIBUTE_NAME,
                "attribute" : {
                    "holeId": holeId,
                    "designator": hole.designator,
                    "componentDesignator": hole.componentDesignator,
                    "pcbEntityType": definition.pcbEntityType,
                    "plated": definition.plated,
                    "owner": hole.holeOwner,
                    "purpose": getPurpose(definition.pcbEntityType, definition.plated, hole.platedPurpose, hole.nonPlatedPurpose, hole.customPurpose),
                    "startFace": regionInfo.startFace,
                    "endFace": regionInfo.endFace
                } as PcbHoleAttribute
        });

        context->setHighlightedEntities({ "entities": allInteriorFaces });
    }

    // Grab the first error we have details for and report it
    for (var error, details in errorToDetails[])
    {
        const allEntities = qUnion(details.entities);
        throw context->isQueryEmpty(allEntities)
            ? regenError(error, details.faultyParameters)
            : regenError(error, details.faultyParameters, allEntities);
    }
}

/**
 * @internal
 * Return an arbitrary edge on the outer loop of a face.
 */
function getEdgeOnOuterLoop(context is Context, face is Query) returns Query
{
    // Find the outer loop of the face by looking for the edge closest to a point outside the bounding box
    const faceBoundingBox = context->evBox3d({
            "topology" : face
    });

    // Take the vector from minCorner to maxCorner and go that far out from maxCorner to get a far-enough-away point
    const pointOutsideBox = faceBoundingBox.maxCorner + faceBoundingBox.maxCorner - faceBoundingBox.minCorner;

    // The edge closest to that point is on the outer loop of the face
    const allStartFaceEdges = face->qAdjacent(AdjacencyType.EDGE, EntityType.EDGE)->qConstructionFilter(ConstructionObject.NO);
    return allStartFaceEdges->qClosestTo(pointOutsideBox);
}

/**
 * @internal
 * Determines a purpose string given a PCB hole definition with either a predefined or custom purpose value.
 */
function getPurpose(holeType is PcbEntityType, plated is boolean,
    platedPurpose is PcbPlatedHolePurpose, nonPlatedPurpose is PcbNonPlatedHolePurpose, customPurpose is string) returns string
{
    if (holeType == PcbEntityType.CUTOUT
        || (plated && platedPurpose == PcbPlatedHolePurpose.CUSTOM)
        || (!plated && nonPlatedPurpose == PcbNonPlatedHolePurpose.CUSTOM))
    {
        return customPurpose ?? "";
    }

    if (holeType == PcbEntityType.HOLE)
    {
        if (plated)
        {
            if (platedPurpose == PcbPlatedHolePurpose.VIA)
                return "Via";
            if (platedPurpose == PcbPlatedHolePurpose.THERMAL_VIA)
                return "Thermal via";
            if (platedPurpose == PcbPlatedHolePurpose.PIN)
                return "Pin";
        }
        else
        {
            if (nonPlatedPurpose == PcbNonPlatedHolePurpose.MOUNTING)
                return "Mounting";
            if (nonPlatedPurpose == PcbNonPlatedHolePurpose.TOOLING)
                return "Tooling";
        }
    }

    return "";
}

