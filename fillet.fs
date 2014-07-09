export import(path : "onshape/std/geomUtils.fs", version : "");

//Fillet feature
annotation {"Feature Type Name" : "Fillet"}
export function fillet(context is Context, id is Id, filletDefinition is map)
precondition
{
    annotation {"Name" : "Entities to fillet",
                "Filter" :  ( (EntityType.EDGE && EdgeTopology.TWO_SIDED) || EntityType.FACE) && ConstructionObject.NO && SketchObject.NO}
    filletDefinition.entities is Query;

    annotation {"Name" : "Radius"}
    isLength(filletDefinition.radius, BLEND_BOUNDS);
    if(filletDefinition.tangentPropagation != undefined)
    {
        annotation {"Name" : "Tangent propagation", "Default" : true}
        filletDefinition.tangentPropagation is boolean;
    }
    if(filletDefinition.conicFillet != undefined)
    {
        annotation{"Name": "Conic fillet"}
        filletDefinition.conicFillet is boolean;
    }
    if(filletDefinition.conicFillet == true)
    {
        annotation {"Name" : "Rho"}
        isReal(filletDefinition.rho, FILLET_RHO_BOUNDS);
    }
}
{
    startFeature(context, id, filletDefinition);
    if(filletDefinition.tangentPropagation == undefined)
        filletDefinition.tangentPropagation = false;

    if(filletDefinition.conicFillet == undefined)
        filletDefinition.conicFillet = false;

    opFillet(context, id, filletDefinition);
    endFeature(context, id);
}

