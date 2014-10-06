export import(path : "onshape/std/geomUtils.fs", version : "");

//Section Part Feature
export function sectionPart(context is Context, id is Id, sectionPartDefinition is map)
precondition
{
    sectionPartDefinition.targets is Query;
    sectionPartDefinition.plane is Plane;
}
{
    startFeature(context, id, sectionPartDefinition);
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
    var deleteBodiesId = id + "deleteBody";
    var splitBodies = qBodySplitBy(splitPartId, false);
    opDeleteBodies(context, deleteBodiesId, {"entities" : qUnion([splitBodies, planeTool])});
    processSubfeatureStatus(context, deleteBodiesId, id);

    endFeature(context, id);
}

