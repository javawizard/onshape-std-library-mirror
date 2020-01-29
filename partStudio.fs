FeatureScript 1224; /* Automatically generated version */
import(path : "onshape/std/context.fs", version : "1224.0");
import(path : "onshape/std/defaultFeatures.fs", version : "1224.0");

import(path : "onshape/std/containers.fs", version : "1224.0");
import(path : "onshape/std/units.fs", version : "1224.0");

/** @internal */
export function definePartStudio(partStudio is function, defaultLengthUnit is ValueWithUnits, defaults is map) returns function
{
    return function(configuration is map) returns Context
        {
            var mergedConfiguration = defaults;
            for (var configurationParameter in defaults)
            {
                var specified = configuration[configurationParameter.key];
                if (specified != undefined)
                    mergedConfiguration[configurationParameter.key] = specified;
            }
            var context is Context = newContext();
            const lookup is function = function(name is string) { return try(getVariable(context, name)); };
            for (var configurationParameter in mergedConfiguration)
            {
                if (configurationParameter.key is string)
                    setVariable(context, configurationParameter.key, configurationParameter.value);
            }
            addDefaultFeatures(context, defaultLengthUnit);
            try(partStudio(context, mergedConfiguration, lookup));
            return context;
        };
}

