FeatureScript 2625; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/attributes.fs", version : "2625.0");
import(path : "onshape/std/containers.fs", version : "2625.0");
import(path : "onshape/std/coordSystem.fs", version : "2625.0");
import(path : "onshape/std/debug.fs", version : "2625.0");
import(path : "onshape/std/error.fs", version : "2625.0");
import(path : "onshape/std/evaluate.fs", version : "2625.0");
import(path : "onshape/std/feature.fs", version : "2625.0");
import(path : "onshape/std/featureList.fs", version : "2625.0");
import(path : "onshape/std/formedUtils.fs", version : "2625.0");
import(path : "onshape/std/frameAttributes.fs", version : "2625.0");
import(path : "onshape/std/frameUtils.fs", version : "2625.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2625.0");
import(path : "onshape/std/units.fs", version : "2625.0");
import(path : "onshape/std/vector.fs", version : "2625.0");

/**
 * Defines the kind of entity being tagged in the feature.
 * @value FRAME : Tag a frame profile with metadata. The metadata will be displayed in the cut list for frames derived from the
 * tagged profile.
 * @value FORM : Tag a form with metadata. The metadata will be used for formed features derived from the form.
 */
export enum TagPurpose
{
    annotation { "Name" : "Frame" }
    FRAME,
    annotation { "Name" : "Form" }
    FORM
}

/**
 * Tag an entity with metadata. The metadata will be used for formed and frame features.
 */

annotation { "Feature Type Name" : "Tag" }
export const tag = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Tag purpose", "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE }
        definition.tagPurpose is TagPurpose;

        if (definition.tagPurpose == TagPurpose.FRAME)
        {
            annotation { "Name" : "Sketch profile", "MaxNumberOfPicks" : 1 }
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
        else if (definition.tagPurpose == TagPurpose.FORM)
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
    }, {
            tagPurpose : TagPurpose.FRAME,
            standard : "",
            description : "",
            additionalColumns : [],
            alignmentPoints : qNothing(),
            showDefaultAlignmentPoints : false,
            positivePart : qNothing(),
            negativePart : qNothing(),
            cSysMateConnector : qNothing()
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


