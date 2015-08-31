FeatureScript 213; /* Automatically generated version */
export import(path : "onshape/std/geomOperations.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/box.fs", version : "");

//Given a plane definition and a input part query will return a list of bodies that one needs to delete so that
//the only bodies that remain are the ones split by the plane and behind it. Used by drawings to render a section view
function performSectionCutAndGetBodiesToDelete(context is Context, id is Id, plane is Plane, partToSection is Query) returns Query
{
    var allBodies = qBodyType(qEverything(EntityType.BODY), BodyType.SOLID);

    var transform = planeToWorld(plane);
    // The bbox of the body in plane coordinate system with positive z being in front of the plane
    var boxResult = evBox3d(context, { 'topology' : partToSection, 'cSys' : transform });

    // Body is fully behind the plane. Retain only the input body. no splitting needed
    if (boxResult.maxCorner[2] < TOLERANCE.zeroLength * meter)
    {
        return qSubtraction(allBodies, partToSection);
    }

    // Body is fully in front of plane. Delete all bodies no splitting needed
    if (boxResult.minCorner[2] > -TOLERANCE.zeroLength * meter)
    {
        return allBodies;
    }

    // Create construction plane for sectioning
    var cplaneDefinition =
    {
        "plane" : plane,
        "size" : 1 * meter
    };

    var planeId = id + "plane";
    opPlane(context, planeId, cplaneDefinition);
    var planeTool = qOwnerPart(qCreatedBy(planeId));

    //The plane needs to be deleted so that it is not processed as a section face
    allBodies = qUnion([allBodies, planeTool]);

    // Split part on plane
    var splitPartDefinition =
    {
        "targets" : partToSection,
        "tool" : planeTool,
        "keepTools" : false
    };

    var splitPartId = id + "splitPart";
    opSplitPart(context, splitPartId, splitPartDefinition);

    // Split was success. Retain everything behind the plane
    return qSubtraction(allBodies, qSplitBy(splitPartId, EntityType.BODY, true));
}

//Section Part Feature
export const sectionPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        definition.targets is Query;
        definition.plane is Plane;
    }
    {
        var bodiesToDelete = qEverything(EntityType.BODY); // Delete everything if there's an error
        try
        {
            bodiesToDelete = performSectionCutAndGetBodiesToDelete(context, id, definition.plane, definition.targets);
        }
        // TODO: how are errors reported?
        var deleteBodiesId = id + "deleteBody";
        opDeleteBodies(context, deleteBodiesId, { "entities" : bodiesToDelete });
    });





