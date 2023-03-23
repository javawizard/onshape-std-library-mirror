FeatureScript 1993; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/attributes.fs", version : "1993.0");
import(path : "onshape/std/containers.fs", version : "1993.0");
import(path : "onshape/std/error.fs", version : "1993.0");
import(path : "onshape/std/feature.fs", version : "1993.0");
import(path : "onshape/std/featureList.fs", version : "1993.0");
import(path : "onshape/std/frameAttributes.fs", version : "1993.0");
import(path : "onshape/std/frameUtils.fs", version : "1993.0");

/**
 * Tag a frame profile with metadata. The metadata will be displayed in the cut list for frames derived from the
 * tagged profile.
 */
annotation { "Feature Type Name" : "Tag profile" }
export const tagProfile = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Sketch profile", "MaxNumberOfPicks" : 1 }
        definition.sketch is FeatureList;

        annotation { "Group Name" : "Additional alignment points", "Collapsed By Default" : true }
        {
            annotation { "Name" : "Sketch points", "Filter" : EntityType.VERTEX && SketchObject.YES }
            definition.alignmentPoints is Query;
        }

        // --
        // Standard and Description must appear as explicit fields because array parameter fields are not configurable
        // (for now - BEL-85948), and these fields must be configurable in order for us to construct the standard
        // library of profiles.
        annotation { "Name" : "Standard" }
        definition.standard is string;

        annotation { "Name" : "Description" }
        definition.description is string;
        // --

        annotation {
                    "Name" : "Additional columns",
                    "Item name" : "Column",
                    "Item label template" : "#header = #value",
                    "UIHint" : UIHint.PREVENT_ARRAY_REORDER
                }
        definition.additionalColumns is array;
        for (var column in definition.additionalColumns)
        {
            annotation { "Name" : "Header" }
            column.header is string;

            annotation { "Name" : "Value" }
            column.value is string;
        }
    }
    {
        doTagProfile(context, id, definition);
    }, {
            standard : "",
            description : "",
            additionalColumns : [],
            alignmentPoints : qNothing()
        });

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
    doTagCustomAlignmentPoints(context, topLevelId, definition);
}

function doTagCustomAlignmentPoints(context is Context, topLevelId is Id, definition is map)
{
    const allPointsQuery = qCreatedBy(definition.sketch) -> qEntityFilter(EntityType.VERTEX);
    const goodPointsQuery = qIntersection([allPointsQuery, definition.alignmentPoints]);
    const badPointsQuery = qSubtraction(definition.alignmentPoints, goodPointsQuery);
    verify(isQueryEmpty(context, badPointsQuery), ErrorStringEnum.FRAME_CUSTOM_ALIGNMENT_POINTS_NOT_IN_SKETCH, { "entities" : badPointsQuery });
    setCustomFrameAlignmentPointAttribute(context, goodPointsQuery);
}
