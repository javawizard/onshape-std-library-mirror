export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");

//Section Part Feature
export const sectionPart = defineFeature(function(context is Context, id is Id, sectionPartDefinition is map)
    precondition
    {
        sectionPartDefinition.targets is Query;
        sectionPartDefinition.plane is Plane;
    }
    {
        // Create construction plane for sectioning
        var cplaneDefinition =
            {
                "plane": sectionPartDefinition.plane,
                "size": 1 * meter
            };
        var planeId = id + "plane";
        opPlane(context, planeId, cplaneDefinition);
        processSubfeatureStatus(context, planeId, id);
        var planeTool = qOwnerPart(qCreatedBy(planeId));

        // Split part on plane
        var splitPartDefinition =
            {
                "targets": sectionPartDefinition.targets,
                "tool": planeTool,
                "keepTools": true
            };
        var splitPartId = id + "splitPart";
        opSplitPart(context, splitPartId, splitPartDefinition);
        processSubfeatureStatus(context, splitPartId, id);

        // Delete parts in front of plane
        var splitBodies = qBodySplitBy(splitPartId, false);
        var bodiesToDelete = qUnion([splitBodies, planeTool]);
        var transform = planeToWorld(sectionPartDefinition.plane);
        var boxResult = evBox3d(context, { 'topology' : sectionPartDefinition.targets, 'cSys' : transform} );
        if (boxResult.error == undefined) {
            if (stripUnits(boxResult.result.minCorner[2]) > -TOLERANCE.zeroLength) {
                bodiesToDelete = qUnion([splitBodies, planeTool, sectionPartDefinition.targets]);
            }
        }

        // Delete parts in front of plane
        var deleteBodiesId = id + "deleteBody";
        opDeleteBodies(context, deleteBodiesId, {"entities" : bodiesToDelete});
        processSubfeatureStatus(context, deleteBodiesId, id);
    });

