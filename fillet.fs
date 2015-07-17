FeatureScript 172; /* Automatically generated version */
export import(path : "onshape/std/geomUtils.fs", version : "");

//Fillet feature
annotation { "Feature Type Name" : "Fillet", "Filter Selector" : "allparts" }
export const fillet = defineFeature(function(context is Context, id is Id, filletDefinition is map)
    precondition
    {
        annotation { "Name" : "Entities to fillet",
                     "Filter" : ((EntityType.EDGE && EdgeTopology.TWO_SIDED) || EntityType.FACE) && ConstructionObject.NO && SketchObject.NO }
        filletDefinition.entities is Query;

        annotation { "Name" : "Radius" }
        isLength(filletDefinition.radius, BLEND_BOUNDS);

        annotation { "Name" : "Tangent propagation", "Default" : true }
        filletDefinition.tangentPropagation is boolean;

        annotation { "Name" : "Conic fillet" }
        filletDefinition.conicFillet is boolean;

        if (filletDefinition.conicFillet)
        {
            annotation { "Name" : "Rho" }
            isReal(filletDefinition.rho, FILLET_RHO_BOUNDS);
        }
    }
    {
        opFillet(context, id, filletDefinition);
    }, { tangentPropagation : false, conicFillet : false });

