export import(path : "onshape/std/geomUtils.fs", version : "");

//Chamfer Feature
annotation {"Feature Type Name" : "Chamfer"}
export function chamfer(context is Context, id is Id, chamferDefinition is map)
precondition
{
    annotation {"Name" : "Entities to chamfer",
                "Filter" :  ( (EntityType.EDGE && EdgeTopology.TWO_SIDED) || EntityType.FACE) && ConstructionObject.NO && SketchObject.NO}
    chamferDefinition.entities is Query;
    annotation {"Name" : "Width"}
    isLength(chamferDefinition.width, BLEND_BOUNDS);
    if(chamferDefinition.tangentPropagation != undefined)
    {
        annotation {"Name" : "Tangent propagation", "Default" : true}
        chamferDefinition.tangentPropagation is boolean;
    }
}
{
    startFeature(context, id, chamferDefinition);
    if(chamferDefinition.tangentPropagation == undefined)
        chamferDefinition.tangentPropagation = false;
    opChamfer(context, id, chamferDefinition);
    endFeature(context, id);
}

