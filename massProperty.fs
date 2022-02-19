FeatureScript 1711; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2020-Present Onshape Inc.

import(path : "onshape/std/context.fs", version : "1711.0");
import(path : "onshape/std/error.fs", version : "1711.0");
import(path : "onshape/std/errorstringenum.gen.fs", version : "1711.0");
import(path : "onshape/std/evaluate.fs", version : "1711.0");
import(path : "onshape/std/properties.fs", version : "1711.0");
import(path : "onshape/std/query.fs", version : "1711.0");
import(path : "onshape/std/units.fs", version : "1711.0");
import(path : "onshape/std/computedPartProperty.fs", version : "1711.0");

/** @internal */
annotation { "Property Function Name" : "computeMass" }
export const computeMass = defineComputedPartProperty(function(context is Context, part is Query, definition is map) returns ValueWithUnits
    {
        // calculate the volume outside the try block so that any geometry errors will be passed through
        const volume is ValueWithUnits = evVolume(context, { "entities" : part });
        try
        {
            const material is Material = getProperty(context, {
                "entity" : part,
                "propertyType" : PropertyType.MATERIAL
            });
            return material.density * volume;
        }
        catch
        {
            if (isQueryEmpty(context, qBodyType(part, BodyType.COMPOSITE)))
            {
                throw regenError(ErrorStringEnum.NO_MATERIAL_FOR_MASS_PROPERTY);
            }
            else
            {
                throw regenError(ErrorStringEnum.NO_MATERIAL_FOR_COMPOSITE_PART_COMPUTED_MASS);
            }
        }
    });

