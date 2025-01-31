FeatureScript 2581; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

import(path : "onshape/std/containers.fs", version : "2581.0");
import(path : "onshape/std/coordSystem.fs", version : "2581.0");
import(path : "onshape/std/evaluate.fs", version : "2581.0");
import(path : "onshape/std/feature.fs", version : "2581.0");
import(path : "onshape/std/formedUtils.fs", version : "2581.0");
import(path : "onshape/std/hole.fs", version : "2581.0");
import(path : "onshape/std/instantiator.fs", version : "2581.0");
import(path : "onshape/std/registerSheetMetalFormedTools.fs", version : "2581.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2581.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2581.0");
import(path : "onshape/std/vector.fs", version : "2581.0");

/**
 * Creates forms of specified dimensions and style, based either on standard
 * forms, or by user-defined forms. Each form's position and orientation are
 * specified using sketch points or mate connectors.
 * For sketch points sketch coordinate system specifies orientation.
 */
annotation { "Feature Type Name" : "Form", "Editing Logic Function" : "formedEditLogic" }
export const sheetMetalFormed = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation {
            "Library Definition" : "65dcc2bb2c4ff1c239467eca", // This is the id of the Onshape Form Library definition
            "Name" : "Form Part Studio",
            "Filter" : PartStudioItemType.ENTIRE_PART_STUDIO,
            "ComputedConfigurationInputs" : [ "thickness" ],
            "MaxNumberOfPicks" : 1,
            "UIHint" : UIHint.REMEMBER_PREVIOUS_VALUE
        }
        definition.formPartStudio is PartStudioData;
        annotation { "Name" : "Location(s)", "Filter" : BodyType.MATE_CONNECTOR || (EntityType.VERTEX && SketchObject.YES && ModifiableEntityOnly.YES) }
        definition.locations is Query;

        annotation { "Name" : "Flip direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.flipDirection is boolean;

        annotation { "Name" : "Target face(s)", "Filter" : GeometryType.PLANE && ActiveSheetMetal.YES && SheetMetalDefinitionEntityType.FACE && ModifiableEntityOnly.YES }
        definition.targetFaces is Query;

    }
    {
        checkNotInFeaturePattern(context, definition.targetFaces, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        if (isQueryEmpty(context, definition.locations))
        {
            throw regenError(ErrorStringEnum.FORMED_SELECT_LOCATION, ["locations"]);
        }

        if (definition.formPartStudio.partQuery == undefined || definition.formPartStudio.partQuery.subqueries == [])
        {
            throw regenError(ErrorStringEnum.FORMED_NO_PART_STUDIO_SELECTED, ["formPartStudio"]);
        }

        const instantiator = newInstantiator(id, {});

        const output = doChecksAndAddFormBodyInstances(context, id, definition, instantiator);
        const definitionFaceToFormedBodies = output.definitionFaceToFormedBodies;
        const allFormedBodies = output.allFormedBodies;

        try
        {
            instantiate(context, instantiator);
        }
        catch //details will be in FS notices
        {
            throw regenError(ErrorStringEnum.FORMED_FAILED_TO_DERIVE, ["formPartStudio"]);
        }

        const wallToFormedBodyIds = callSubfeatureAndProcessStatus(id, registerSheetMetalFormedTools, context, id + "sheetMetalFormed", {
                                            "definitionFaceToFormedBodies" : definitionFaceToFormedBodies,
                                            "doUpdateSMGeometry" : true
                                        });

        var sketchBodies = [];
        for (var formedBodies in values(wallToFormedBodyIds))
        {
            for (var formedBodyIds in formedBodies)
            {
                if (formedBodyIds.sketchBodyIds != undefined)
                {
                    for (var sketchBodyId in formedBodyIds.sketchBodyIds)
                    {
                        sketchBodies = append(sketchBodies, qTransient(sketchBodyId));
                    }
                }
            }
        }

        // If any of the tools couldn't create forms, because they collide with curved walls, side walls etc.,
        // those tools will remain public. Report an info about them and then delete them.
        const bodiesToDelete = qSubtraction(allFormedBodies, qUnion(sketchBodies));
        if (!isQueryEmpty(context, bodiesToDelete))
        {
            if (getFeatureInfo(context, id) == undefined &&
                getFeatureWarning(context, id) == undefined &&
                getFeatureError(context, id) == undefined)
            {
                const errorBodies = qBodiesWithFormAttributes(bodiesToDelete, [FORM_BODY_POSITIVE_PART, FORM_BODY_NEGATIVE_PART, FORM_BODY_SKETCH_FOR_FLAT_VIEW]);
                if (!isQueryEmpty(context, errorBodies))
                {
                    if (wallToFormedBodyIds == {})
                    {
                        throw regenError(ErrorStringEnum.SHEET_METAL_CANNOT_CUT, errorBodies);
                    }
                    else
                    {
                        reportFeatureWarning(context, id, ErrorStringEnum.SHEET_METAL_CANNOT_CUT);
                        setErrorEntities(context, id, { "entities" : errorBodies });
                    }
                }
            }

            opDeleteBodies(context, id + "deleteUnneededEntities", { "entities" : bodiesToDelete });
        }
    }, {});

/**
 * @internal
 * Editing logic for formed feature.
 */
export function formedEditLogic(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean) returns map
{
    if (definition.locations == oldDefinition.locations)
    {
        return definition;
    }

    definition.locations = qUnion(clusterVertexQueries(context, definition.locations));

    definition.targetFaces = qNothing();
    var targetFaces = [];
    const activeSMFaces = qAllModifiableSolidBodiesNoMesh()->qActiveSheetMetalFilter(ActiveSheetMetal.YES)->qOwnedByBody(EntityType.FACE);
    for (var location in evaluateQuery(context, definition.locations))
    {
        const cSys = evaluateCSys(context, location);
        targetFaces = append(targetFaces, activeSMFaces->qParallelPlanes(cSys.zAxis, true)->qContainsPoint(cSys.origin));
    }
    // Make sure non-SheetMetalDefinitionEntityType.FACE faces do not sneak in
    const targetFacesQ = qUnion(targetFaces);
    targetFaces = [];
    for (var targetFace in evaluateQuery(context, targetFacesQ))
    {
        const definitionEntities = getSMDefinitionEntities(context, targetFace);
        if (size(definitionEntities) == 1 && !isQueryEmpty(context, qEntityFilter(definitionEntities[0], EntityType.FACE)))
        {
            targetFaces = append(targetFaces, targetFace);
        }
    }
    definition.targetFaces = qUnion(targetFaces);

    return definition;
}

/**
 * @internal
 * Do error and warning checks on the inputs for formed and add the form tool body instances.
 */
function doChecksAndAddFormBodyInstances(context is Context, id is Id, definition is map, instantiator is Instantiator) returns map
{
    var addInstancePartStudio = definition.formPartStudio;
    addInstancePartStudio.partQuery = qBodiesWithFormAttributes(definition.formPartStudio.partQuery, [FORM_BODY_POSITIVE_PART,
                                                FORM_BODY_NEGATIVE_PART, FORM_BODY_SKETCH_FOR_FLAT_VIEW, FORM_BODY_CSYS_MATE_CONNECTOR]);
    var definitionFaceToFormedBodies = {};
    var allFormedBodies = qNothing();
    var mateConnectorsNotOnActiveFace = [];
    var mateConnectorsNotNormalToFace = [];
    var duplicateLocations = [];
    var allPointsOnBodies = [];
    for (var location in evaluateQuery(context, definition.locations))
    {
        var cSys = evaluateCSys(context, location);
        const faces = evaluateQuery(context, qContainsPoint(definition.targetFaces, cSys.origin)->qActiveSheetMetalFilter(ActiveSheetMetal.YES));
        const nFaces = size(faces);
        if (nFaces == 0)
        {
            mateConnectorsNotOnActiveFace = append(mateConnectorsNotOnActiveFace, location);
            continue;
        }
        else if (nFaces > 1)
        {
            throw regenError(ErrorStringEnum.FORMED_LOCATION_ON_MULTIPLE_FACES, ["locations", "targetFaces"], qUnion(location, qUnion(faces)));
        }
        const definitionEntities = getSMDefinitionEntities(context, faces[0]);
        if (size(definitionEntities) != 1)
        {
            throw regenError(ErrorStringEnum.FORMED_NOT_ON_HOLE_FORMED_FACE, ["locations"], location);
        }
        const smDefinitionModels = evaluateQuery(context, qOwnerBody(definitionEntities[0]));
        if (size(smDefinitionModels) != 1)
        {
            throw "size(smDefinitionModels) is " ~ size(smDefinitionModels);
        }
        const smAttributes = getSmObjectTypeAttributes(context, smDefinitionModels[0], SMObjectType.MODEL);
        if (size(smAttributes) != 1)
        {
            throw "Found model with more than one SMObjectType.MODEL attribute";
        }
        const thickness = smAttributes[0].frontThickness == undefined ? smAttributes[0].backThickness : smAttributes[0].frontThickness;
        if (thickness == undefined || thickness.value == undefined)
        {
            throw "Thickness of sheet metal model undefined";
        }
        const facePlane = evPlane(context, { "face" : faces[0] });
        if (!parallelVectors(cSys.zAxis, facePlane.normal))
        {
            mateConnectorsNotNormalToFace = concatenateArrays([mateConnectorsNotNormalToFace, [location, faces[0]]]);
            continue;
        }
        if (definition.flipDirection)
        {
            cSys.zAxis = -cSys.zAxis;
        }
        if (dot(cSys.zAxis, facePlane.normal) < 0.0)
        {
            // The mate connector is anti-parallel to the face selected.
            // Move the origin by the thickness of the sheet so that the form is on the opposite face and hence parallel to that face's normal.
            cSys.origin += (cSys.zAxis)*(thickness.value);
        }
        if (isPointOnBodyInPointsOnBodies(context, { "point" : cSys.origin, "body" : qOwnerBody(faces[0]) }, allPointsOnBodies))
        {
            duplicateLocations = append(duplicateLocations, location);
            continue;
        }
        else
        {
            allPointsOnBodies = append(allPointsOnBodies, { "point" : cSys.origin, "body" : qOwnerBody(faces[0]) });
        }
        const formedBodies = addInstance(instantiator, addInstancePartStudio, {
                                "transform" : toWorld(cSys),
                                "identity" : location,
                                "configurationOverride" : { "thickness" : thickness.value },
                                "mateConnector" : qBodiesWithFormAttribute(FORM_BODY_CSYS_MATE_CONNECTOR)
                            });
        definitionFaceToFormedBodies = insertIntoMapOfArrays(definitionFaceToFormedBodies, definitionEntities[0], formedBodies);
        allFormedBodies = qUnion(allFormedBodies, formedBodies);
    }
    if (definitionFaceToFormedBodies == {})
    {
        if (mateConnectorsNotOnActiveFace != [])
        {
            throw regenError(ErrorStringEnum.FORMED_SELECT_LOCATION_ON_ACTIVE_FACE, ["locations"], qUnion(mateConnectorsNotOnActiveFace));
        }
        else if (mateConnectorsNotNormalToFace != [])
        {
            throw regenError(ErrorStringEnum.FORMED_TOOL_NOT_NORMAL_TO_FACE, ["locations", "targetFaces"], qUnion(mateConnectorsNotNormalToFace));
        }
        else if (duplicateLocations != [])
        {
            throw regenError(ErrorStringEnum.FORMED_NOT_SAME_LOCATION, ["locations"], qUnion(duplicateLocations));
        }
        else
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_FORMED_REBUILD_FAILED);
        }
    }
    else
    {
        if (mateConnectorsNotOnActiveFace != [])
        {
            reportFeatureWarning(context, id, ErrorStringEnum.FORMED_SELECT_LOCATION_ON_ACTIVE_FACE, ["locations"]);
            setErrorEntities(context, id, { "entities" : qUnion(mateConnectorsNotOnActiveFace) });
        }
        else if (mateConnectorsNotNormalToFace != [])
        {
            reportFeatureWarning(context, id, ErrorStringEnum.FORMED_TOOL_NOT_NORMAL_TO_FACE, ["locations", "targetFaces"]);
            setErrorEntities(context, id, { "entities" : qUnion(mateConnectorsNotNormalToFace) });
        }
        else if (duplicateLocations != [])
        {
            reportFeatureWarning(context, id, ErrorStringEnum.FORMED_NOT_SAME_LOCATION);
            setErrorEntities(context, id, { "entities" : qUnion(duplicateLocations) });
        }
    }

    return { "definitionFaceToFormedBodies" : definitionFaceToFormedBodies, "allFormedBodies" : allFormedBodies };
}

/**
 * @internal
 * Check whether a point on a body already existis in an array of points on bodies up to tolerance.
 */
function isPointOnBodyInPointsOnBodies(context is Context, pointOnBody is map, pointsOnBodies is array) returns boolean
{
    for (var pointsOnBodiesElement in pointsOnBodies)
    {
        if (tolerantEquals(pointOnBody.point, pointsOnBodiesElement.point) &&
            !isQueryEmpty(context, qIntersection(pointOnBody.body, pointsOnBodiesElement.body)))
        {
            return true;
        }
    }
    return false;
}

function evaluateCSys(context is Context, location is Query) returns CoordSystem
{
    if (isQueryEmpty(context, location->qBodyType(BodyType.MATE_CONNECTOR)))
    {
        const skPlane = evOwnerSketchPlane(context, {
                "entity" : location
        });
        const point = evVertexPoint(context, {
                "vertex" : location
        });
        return coordSystem(point, skPlane.x, skPlane.normal);
    }
    else
    {
        return evMateConnector(context, { "mateConnector" : location });
    }
}

