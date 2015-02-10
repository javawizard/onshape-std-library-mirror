export import(path : "onshape/std/geomUtils.fs", version : "");

//Planes
export enum DefaultPlaneType
{
    XY,
    YZ,
    XZ
}

// Tool body
export enum ToolBodyType
{
    annotation {"Name" : "Solid"}
    SOLID,
    annotation {"Name" : "Surface"}
    SURFACE
}

export function defaultPlane(context is Context, id is Id, defaultType is DefaultPlaneType, size)
precondition
{
    isLength(size);
}
{
    var definition = {  "defaultType" : defaultType,
                        "size"        : size
                    };

    startFeature(context, id, definition);
    var origin = vector(0, 0, 0) * meter;
    if(defaultType == DefaultPlaneType.XY)
      definition.plane = plane(origin, vector(0, 0, 1), vector(1, 0, 0));
    if(defaultType == DefaultPlaneType.YZ)
      definition.plane = plane(origin, vector(1, 0, 0), vector(0, 1, 0));
    if(defaultType == DefaultPlaneType.XZ)
      definition.plane = plane(origin, vector(0, -1, 0), vector(1, 0, 0));
    opPlane(context, id, definition);
    endFeature(context, id);
}

export function origin(context is Context)
{
    var id = makeId("Origin");
    startFeature(context, id, {});
    var out = opPoint(context, id, {"point" : vector(0, 0, 0) * meter, "origin" : true});
    endFeature(context, id);
    return out;
}

//Import Feature
export type ForeignId typecheck canBeForeignId;

export predicate canBeForeignId(value)
{
    value is string;
    //TODO: other checks
}

annotation {"Feature Type Name" : "Import"}
export const importForeign = defineFeature(function(context is Context, id is Id, importDefinition is map)
    precondition
    {
        annotation {"Name" : "Foreign Id"}
        importDefinition.foreignId is ForeignId;

        annotation {"Name" : "Source is 'Y Axis Up'"}
        importDefinition.yAxisIsUp is boolean;
    }
    {
        opImportForeign(context, id, importDefinition);
    }, { yAxisIsUp : false });

annotation {"Feature Type Name" : "Delete Part"}
export const deleteBodies = defineFeature(function(context is Context, id is Id, deleteDefinition is map)
    precondition
    {
        annotation {"Name" : "Entities to delete",
        "Filter" : EntityType.BODY}
        deleteDefinition.entities is Query;
    }
    {
        opDeleteBodies(context, id, deleteDefinition);
    });

