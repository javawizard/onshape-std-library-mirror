FeatureScript 156; /* Automatically generated version */
export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");

//Given a plane definition and a input part query will return a list of bodies that one needs to delete so that
//the only bodies that remain are the ones split by the plane and behind it. Used by drawings to render a section view
function performSectionCutAndGetBodiesToDelete(context is Context, id is Id, plane is Plane, partToSection is Query) returns Query
{
    var allBodies = qBodyType(qEverything(EntityType.BODY), BodyType.SOLID);

    var transform = planeToWorld(plane);
    // The bbox of the body in plane coordinate system with positive z being in front of the plane
    var boxResult = evBox3d(context, { 'topology' : partToSection, 'cSys' : transform});

    // Error while bbox computation we delete all bodies
    if (boxResult.error != undefined) {
        return allBodies;
    }

    boxResult = stripUnits(boxResult);

    // Body is fully behind the plane. Retain only the input body. no splitting needed
    if (boxResult.result.maxCorner[2] < TOLERANCE.zeroLength) {
        return qSubtraction(allBodies, partToSection);
    }

    // Body is fully in front of plane. Delete all bodies no splitting needed
    if (boxResult.result.minCorner[2] > -TOLERANCE.zeroLength) {
        return allBodies;
    }

    // Create construction plane for sectioning
    var cplaneDefinition =
        {
            "plane": plane,
            "size": 1 * meter
        };

    var planeId = id + "plane";
    opPlane(context, planeId, cplaneDefinition);
    processSubfeatureStatus(context, planeId, id);
    var planeTool = qOwnerPart(qCreatedBy(planeId));

    //The plane needs to be deleted so that it is not processed as a section face
    allBodies = qUnion([allBodies, planeTool]);

    // Split part on plane
    var splitPartDefinition =
        {
            "targets": partToSection,
            "tool": planeTool,
            "keepTools": false
        };

    var splitPartId = id + "splitPart";
    opSplitPart(context, splitPartId, splitPartDefinition);

    // Can happen if the split feature creates non manifold geometry
    if (featureHasError(context, splitPartId))
    {
        return allBodies;
    }

   // Split was success. Retain everything behind the plane
   return qSubtraction(allBodies, qBodySplitBy(splitPartId, true));
}

//Section Part Feature
export const sectionPart = defineFeature(function(context is Context, id is Id, sectionPartDefinition is map)
    precondition
    {
        sectionPartDefinition.targets is Query;
        sectionPartDefinition.plane is Plane;
    }
    {
        var bodiesToDelete = performSectionCutAndGetBodiesToDelete(context, id , sectionPartDefinition.plane, sectionPartDefinition.targets);
        var deleteBodiesId = id + "deleteBody";
        opDeleteBodies(context, deleteBodiesId, {"entities" : bodiesToDelete});
        processSubfeatureStatus(context, deleteBodiesId, id);
    });





