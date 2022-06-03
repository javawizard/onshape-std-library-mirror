FeatureScript 1777; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/feature.fs", version : "1777.0");
import(path : "onshape/std/valueBounds.fs", version : "1777.0");
import(path : "onshape/std/units.fs", version: "1777.0");
import(path : "onshape/std/tabReferences.fs", version : "1777.0");

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
        annotation { "Name" : "Depends on blob", "UIHint" : UIHint.ALWAYS_HIDDEN, "Default" : false }
        definition.dependsOnBlob is boolean;

        if (definition.dependsOnBlob)
        {
            annotation { "Name" : "Blob Data", "UIHint" : UIHint.ALWAYS_HIDDEN }
            definition.blobData is CADImportData;
        }
        else
        {
            annotation { "Name" : "Foreign Id", "UIHint" : UIHint.ALWAYS_HIDDEN }
            definition.foreignId is ForeignId;
        }

        annotation { "Name" : "Source is 'Y Axis Up'" }
        definition.yAxisIsUp is boolean;

        annotation { "Name" : "Allow faulty parts" }
        definition.allowFaultyParts is boolean;

        annotation {"UIHint" : UIHint.ALWAYS_HIDDEN}
        definition.specifyUnits is boolean;

       if (definition.specifyUnits)
        {
            annotation { "Name" : "Unit", "Default" : LengthUnitNames.Meter }
            definition.unit is LengthUnitNames;

            annotation { "Name" : "Original unit", "UIHint" : UIHint.ALWAYS_HIDDEN, "Default" : LengthUnitNames.Meter}
            definition.originalUnit is LengthUnitNames;
        }

        annotation {"Name" : "Flatten assembly", "UIHint" : UIHint.ALWAYS_HIDDEN}
        definition.flatten is boolean;

        annotation {"Name" : "Maximum number of assemblies created", "UIHint" : UIHint.ALWAYS_HIDDEN}
        isInteger(definition.maxAssembliesToCreate, POSITIVE_COUNT_BOUNDS);

        annotation {"Name" : "IsInContext", "UIHint" : UIHint.ALWAYS_HIDDEN, "Default" : false}
        definition.isInContext is boolean;

        annotation {"Name" : "Create composite"}
        definition.createComposite is boolean;

        if (definition.createComposite)
        {
            annotation {"Name" : "Composite name", "UIHint" : UIHint.ALWAYS_HIDDEN}
            definition.compositeName is string;
        }
    }
    {
        if (isInFeaturePattern(context) && definition.isInContext && isAtVersionOrLater(context, FeatureScriptVersionNumber.V1074_SKIP_IN_CONTEXT_PATTERN))
        {
            // In-context subfeatures have no external references that can be patterned, so correct behavior is to never
            // pattern the subfeatures, and only pattern features that depend on them.
            // In-context entities are unmodifiable, so this subfeature would fail anyway, but to prevent extra work and
            // notices we just return early here.
            return;
        }
        var remainingTransform = getRemainderPatternTransform(context,
            {"references" : qNothing()});
        if (definition.specifyUnits)
        {
            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1032_MESH_SCALING_IN_TRANSLATOR))
                definition.scale = stringToUnit(definition.unit as string).value / stringToUnit(definition.originalUnit as string).value;
            else
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
    }, { yAxisIsUp : false, flatten : false, maxAssembliesToCreate : 10, specifyUnits : false, unit : LengthUnitNames.Meter, originalUnit : LengthUnitNames.Meter, isInContext : false, allowFaultyParts : false, dependsOnBlob : false, createComposite : false});



