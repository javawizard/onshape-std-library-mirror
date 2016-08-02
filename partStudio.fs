FeatureScript ✨; /* Automatically generated version */
import(path : "onshape/std/context.fs", version : "✨");
import(path : "onshape/std/defaultFeatures.fs", version : "✨");

import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/units.fs", version : "✨");

/** @internal */
export function definePartStudio(partStudio is function, defaultLengthUnit is ValueWithUnits, defaults is map) returns function
{
    return function(configuration is map) returns Context
        {
            configuration = mergeMaps(defaults, configuration);
            var context is Context = newContextWithDefaults(defaultLengthUnit);
            const lookup is function = function(name is string) { return getVariable(context, name); };
            for (var configurationParameter in configuration)
            {
                if (configurationParameter.key is string)
                    setVariable(context, configurationParameter.key, configurationParameter.value);
            }
            return partStudio(context, configuration, lookup);
        };
}

