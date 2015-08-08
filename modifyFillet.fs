FeatureScript 190; /* Automatically generated version */
export import(path : "onshape/std/geomOperations.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/valueBounds.fs", version : "");

export enum ModifyFilletType
{
    annotation { "Name" : "Change radius" }
    CHANGE_RADIUS,
    annotation { "Name" : "Remove fillet" }
    REMOVE_FILLET
}

annotation { "Feature Type Name" : "Modify fillet", "Filter Selector" : "allparts" }
export const modifyFillet = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Fillet faces to modify",
                     "Filter" : (EntityType.FACE) && ConstructionObject.NO && SketchObject.NO && !GeometryType.PLANE }
        definition.faces is Query;

        annotation { "Name" : "Modification type" }
        definition.modifyFilletType is ModifyFilletType;

        if (definition.modifyFilletType == ModifyFilletType.CHANGE_RADIUS)
        {
            annotation { "Name" : "Fillet radius" }
            isLength(definition.radius, BLEND_BOUNDS);

            annotation { "Name" : "Reapply fillet", "Default" : true }
            definition.reFillet is boolean;
        }
    }
    {
        opModifyFillet(context, id, definition);
    }, { reFillet : false });

