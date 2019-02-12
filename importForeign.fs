FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/valueBounds.fs", version : "✨");
import(path : "onshape/std/units.fs", version: "✨");
import(path : "onshape/std/tabReferences.fs", version : "✨");

/**
 * A `string` representing a foreign element, such as the `dataId` from an
 * imported tab.
 * @type {string}
 */
export type ForeignId typecheck canBeForeignId;

/** Typecheck for [ForeignId] */
export predicate canBeForeignId(value)
{
    value is string;
    //TODO: other checks
}

/**
 * @internal
 */
export enum LengthUnitNames
{
    annotation { "Name" : "Centimeter" }
    Centimeter,
    annotation { "Name" : "Foot" }
    Foot,
    annotation { "Name" : "Inch" }
    Inch,
    annotation { "Name" : "Millimeter" }
    Millimeter,
    annotation { "Name" : "Meter" }
    Meter,
    annotation { "Name" : "Yard" }
    Yard
}

/**
 * Feature performing an [opImportForeign], transforming the result if necessary.
 */
annotation { "Feature Type Name" : "Import" }
export const importForeign = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Depends on blob", "UIHint" : "ALWAYS_HIDDEN", "Default" : false }
        definition.dependsOnBlob is boolean;

        if (definition.dependsOnBlob)
        {
            annotation { "Name" : "Blob Data", "UIHint" : "ALWAYS_HIDDEN" }
            definition.blobData is CADImportData;
        }
        else
        {
            annotation { "Name" : "Foreign Id", "UIHint" : "ALWAYS_HIDDEN" }
            definition.foreignId is ForeignId;
        }

        annotation { "Name" : "Source is 'Y Axis Up'" }
        definition.yAxisIsUp is boolean;

        annotation { "Name" : "Allow faulty parts" }
        definition.allowFaultyParts is boolean;

        annotation { "UIHint" : "ALWAYS_HIDDEN" }
        definition.specifyUnits is boolean;

        if (definition.specifyUnits)
        {
            annotation { "Name" : "Unit" }
            definition.unit is LengthUnitNames;
        }

        annotation {"Name" : "Flatten assembly", "UIHint" : "ALWAYS_HIDDEN"}
        definition.flatten is boolean;

        annotation {"Name" : "Maximum number of assemblies created", "UIHint" : "ALWAYS_HIDDEN"}
        isInteger(definition.maxAssembliesToCreate, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "IsInContext", "UIHint" : "ALWAYS_HIDDEN", "Default" : false  }
        definition.isInContext is boolean;
    }
    {
        var remainingTransform = getRemainderPatternTransform(context,
            {"references" : qNothing()});
        if (definition.specifyUnits)
        {
            definition.scale = stringToUnit(definition.unit as string).value;
        }
        else
        {
            definition.scale = 1.0;
        }
        definition.isModifiable = !definition.isInContext;
        if (definition.dependsOnBlob)
        {
            definition.foreignId = undefined;
            definition.processedDataId = definition.blobData.processedDataId;
        }
        else
        {
            definition.processedDataId = undefined;
        }

        opImportForeign(context, id, definition);

        if (!definition.isInContext && isAtVersionOrLater(context, FeatureScriptVersionNumber.V487_IMPORT_FILTER_POINT_BODIES))
        {
            try silent(opDeleteBodies(context, id + "filterPoints", {
                "entities": qBodyType(qCreatedBy(id, EntityType.BODY), BodyType.POINT)}));
        }

        transformResultIfNecessary(context, id, remainingTransform);
    }, { yAxisIsUp : false, flatten : false, maxAssembliesToCreate : 10, specifyUnits : false, unit : LengthUnitNames.Meter, isInContext : false, allowFaultyParts : false, dependsOnBlob : false });

