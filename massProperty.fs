FeatureScript 1821; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2020-Present Onshape Inc.

import(path : "onshape/std/context.fs", version : "1821.0");
import(path : "onshape/std/error.fs", version : "1821.0");
import(path : "onshape/std/errorstringenum.gen.fs", version : "1821.0");
import(path : "onshape/std/evaluate.fs", version : "1821.0");
import(path : "onshape/std/properties.fs", version : "1821.0");
import(path : "onshape/std/query.fs", version : "1821.0");
import(path : "onshape/std/units.fs", version : "1821.0");
import(path : "onshape/std/computedPartProperty.fs", version : "1821.0");
import(path : "onshape/std/volumeaccuracy.gen.fs", version : "1821.0");

/** @internal */
annotation { "Property Function Name" : "computeMass" }
export const computeMass = defineComputedPartProperty(function(context is Context, part is Query, definition is map) returns ValueWithUnits
    {
        var density is ValueWithUnits = 0 * kilogram / meter ^ 3;
        try
        {
            density = getProperty(context, {
                "entity" : part,
                "propertyType" : PropertyType.MATERIAL
            }).density;
        }
        catch
        {
            if (!isQueryEmpty(context, qBodyType(part, BodyType.COMPOSITE)))
            {
                throw regenError(ErrorStringEnum.NO_MATERIAL_FOR_COMPOSITE_PART_COMPUTED_MASS);
            }
            throw regenError(ErrorStringEnum.NO_MATERIAL_FOR_MASS_PROPERTY);
        }
        // Calculate the volume outside the try block so that any geometry errors will be passed through.
        // Expensive, so only do if there's material, and use the same accuracy used by the Mass properties dialog.
        const volume is ValueWithUnits = evVolume(context, { "entities" : part, "accuracy" : VolumeAccuracy.LOW });
        return density * volume;
    });

