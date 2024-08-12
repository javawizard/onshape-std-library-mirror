FeatureScript 2433; /* Automatically generated version */
import(path : "onshape/std/evaluate.fs", version : "2433.0");
import(path : "onshape/std/feature.fs", version : "2433.0");
import(path : "onshape/std/manipulator.fs", version : "2433.0");
import(path : "onshape/std/math.fs", version : "2433.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2433.0");
import(path : "onshape/std/topologyUtils.fs", version : "2433.0");
import(path : "onshape/std/valueBounds.fs", version : "2433.0");
import(path : "onshape/std/vector.fs", version : "2433.0");
export import(path : "onshape/std/bodydraftcornertype.gen.fs", version : "2433.0");
export import(path : "onshape/std/bodydraftconcaverepairtype.gen.fs", version : "2433.0");
export import(path : "onshape/std/bodydraftmatchfacetype.gen.fs", version : "2433.0");
export import(path : "onshape/std/bodydraftselectiontype.gen.fs", version : "2433.0");

/**
 * An operation that performs an [opBodyDraft].
 */
annotation { "Feature Type Name" : "Body draft",
        "Editing Logic Function" : "bodyDraftEditLogic",
        "Manipulator Change Function" : "bodyDraftManipulatorChange" }
export const bodyDraft = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation {
                    "Name" : "Preselection",
                    "UIHint" : UIHint.ALWAYS_HIDDEN,
                    "Filter" : BodyType.SOLID && (EntityType.BODY || EntityType.FACE || EntityType.EDGE) && ModifiableEntityOnly.YES && ActiveSheetMetal.NO
                }
        definition.preselection is Query;

        annotation { "Name" : "Draft scope", "UIHint" : [UIHint.SHOW_LABEL, UIHint.REMEMBER_PREVIOUS_VALUE] }
        definition.selectionType is BodyDraftSelectionType;

        if (definition.selectionType == BodyDraftSelectionType.EDGES)
        {
            annotation { "Name" : "Edges", "Filter" : EntityType.EDGE && BodyType.SOLID && ModifiableEntityOnly.YES && ActiveSheetMetal.NO }
            definition.topEdges is Query;
        }
        else if (definition.selectionType == BodyDraftSelectionType.FACES)
        {
            annotation { "Name" : "Faces", "Filter" : EntityType.FACE && BodyType.SOLID && ModifiableEntityOnly.YES && ActiveSheetMetal.NO }
            definition.faces is Query;
        }
        else
        {
            annotation { "Name" : "Parts", "Filter" : EntityType.BODY && BodyType.SOLID && ModifiableEntityOnly.YES && ActiveSheetMetal.NO }
            definition.bodies is Query;
            annotation { "Name" : "Faces to exclude", "Filter" : EntityType.FACE && BodyType.SOLID && ModifiableEntityOnly.YES && ActiveSheetMetal.NO }
            definition.excludeFaces is Query;
        }

        annotation { "Name" : "Draft on self" }
        definition.draftOnSelf is boolean;

        annotation { "Name" : "Pull direction",
                    "Filter" : QueryFilterCompound.ALLOWS_PLANE || QueryFilterCompound.ALLOWS_DIRECTION,
                    "MaxNumberOfPicks" : 1 }
        definition.pullDirection is Query;

        annotation { "Name" : "Flip pull direction", "UIHint" : UIHint.OPPOSITE_DIRECTION }
        definition.flipPullDirection is boolean;

        if (!definition.draftOnSelf)
        {
            annotation { "Name" : "Parting entity",
                        "Filter" : (EntityType.BODY && BodyType.SHEET && SketchObject.NO) || EntityType.FACE || BodyType.MATE_CONNECTOR,
                        "MaxNumberOfPicks" : 1 }
            definition.partingObject is Query;
        }

        annotation { "Name" : "Angle" }
        isAngle(definition.angle, ANGLE_STRICT_90_BOUNDS);

        annotation { "Name" : "Symmetric" }
        definition.bothSides is boolean;

        if (definition.bothSides)
        {
            if (definition.selectionType == BodyDraftSelectionType.EDGES)
            {
                annotation { "Name" : "Bottom edges", "Filter" : EntityType.EDGE && ModifiableEntityOnly.YES && ActiveSheetMetal.NO }
                definition.bottomEdges is Query;
            }

            if (!definition.draftOnSelf)
            {
                annotation { "Name" : "Match faces at parting", "Default" : true }
                definition.matchFacesAtParting is boolean;
                if (definition.matchFacesAtParting)
                {
                    annotation { "Group Name" : "Match face options", "Collapsed By Default" : true, "Driving Parameter" : "matchFacesAtParting" }
                    {
                        annotation { "Name" : "Match type", "UIHint" : UIHint.SHOW_LABEL, "Default" : BodyDraftMatchFaceType.TANGENT_TO_FACE }
                        definition.matchFaceType is BodyDraftMatchFaceType;

                        annotation { "Name" : "Concave repair", "UIHint" : UIHint.SHOW_LABEL }
                        definition.concaveRepair is BodyDraftConcaveRepairType;
                        if (definition.concaveRepair != BodyDraftConcaveRepairType.NONE)
                        {
                            annotation { "Name" : "Radius" }
                            isLength(definition.concaveRepairRadius, BLEND_BOUNDS);
                        }
                    }
                }
            }
        }

        annotation { "Name" : "Corner type", "UIHint" : UIHint.SHOW_LABEL }
        definition.cornerType is BodyDraftCornerType;

        annotation { "Name" : "Keep material" }
        definition.keepMaterial is boolean;
    }
    {
        definition.pullDirection = createManipulators(context, id, definition);

        if (definition.selectionType == BodyDraftSelectionType.EDGES)
        {
            verifyNonemptyQuery(context, definition, "topEdges", ErrorStringEnum.BODY_DRAFT_SELECT_EDGES);
            verifyNoMeshInBody(context, definition, "topEdges");
            verifyNoMeshInBody(context, definition, "bottomEdges");
        }
        else if (definition.selectionType == BodyDraftSelectionType.FACES)
        {
            verifyNonemptyQuery(context, definition, "faces", ErrorStringEnum.BODY_DRAFT_SELECT_FACES);
            verifyNoMeshInBody(context, definition, "faces");
        }
        else if (definition.selectionType == BodyDraftSelectionType.PARTS)
        {
            verifyNonemptyQuery(context, definition, "bodies", ErrorStringEnum.BODY_DRAFT_SELECT_PARTS);
            verifyNoMesh(context, definition, "bodies");
        }

        verify(definition.pullDirection != undefined, ErrorStringEnum.BODY_DRAFT_BAD_PULL_DIRECTION, {
                    "faultyParameters" : ["pullDirection"]
                });
        definition.pullDirection = definition.flipPullDirection ? -definition.pullDirection : definition.pullDirection;

        if (!definition.bothSides)
        {
            definition.matchFacesAtParting = false;
        }
        opBodyDraft(context, id, definition);
    }, { "selectionType" : BodyDraftSelectionType.EDGES, "draftOnSelf" : false, "flipPullDirection" : false, "bothSides" : false,
            "matchFacesAtParting" : false, "cornerType" : BodyDraftCornerType.EXTEND, "preselection" : qNothing(), "keepMaterial" : false });

const flipManipulatorId = "flipManipulator";

function createManipulators(context is Context, topLevelId is Id, definition is map)
{
    const direction = try silent(extractDirection(context, definition.pullDirection));
    if (direction == undefined)
    {
        return direction;
    }

    var position;
    if (definition.draftOnSelf)
    {
        var referencedTopology;
        if (definition.selectionType == BodyDraftSelectionType.EDGES)
        {
            referencedTopology = definition.bothSides ? qUnion([definition.topEdges, definition.bottomEdges]) : definition.topEdges;
        }
        else if (definition.selectionType == BodyDraftSelectionType.FACES)
        {
            referencedTopology = definition.faces;
        }
        else
        {
            referencedTopology = definition.bodies;
        }
        if (isQueryEmpty(context, referencedTopology))
        {
            return direction;
        }

        const distanceResult = try silent(evDistance(context, {
                        "side0" : qNthElement(referencedTopology, 0),
                        "side1" : definition.pullDirection
                    }));

        if (distanceResult == undefined)
        {
            return direction;
        }
        position = distanceResult.sides[0].point;
    }
    else
    {
        if (isQueryEmpty(context, definition.partingObject))
        {
            return direction;
        }

        const coords = try silent(evMateConnector(context, {
                        "mateConnector" : definition.partingObject
                    }));
        if (coords == undefined)
        {
            const faces = qUnion([qEntityFilter(definition.partingObject, EntityType.FACE), qOwnedByBody(definition.partingObject, EntityType.FACE)]);
            const plane = evFaceTangentPlane(context, {
                        "face" : faces,
                        "parameter" : vector(0.5, 0.5)
                    });
            position = plane.origin;
        }
        else
        {
            position = coords.origin;
        }
    }

    const manip = flipManipulator({
                "base" : position,
                "direction" : direction,
                "flipped" : definition.flipPullDirection
            });

    addManipulators(context, topLevelId, { (flipManipulatorId) : manip });

    return direction;
}

/**
 * body draft editing logic
 */
export function bodyDraftEditLogic(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean, specifiedParameters is map) returns map
{
    if (oldDefinition == {})
    {
        if (!isQueryEmpty(context, qEntityFilter(definition.preselection, EntityType.BODY)))
        {
            definition.selectionType = BodyDraftSelectionType.PARTS;
            definition.bodies = qEntityFilter(definition.preselection, EntityType.BODY);
        }
        else if (!isQueryEmpty(context, qEntityFilter(definition.preselection, EntityType.FACE)))
        {
            definition.selectionType = BodyDraftSelectionType.FACES;
            definition.faces = qEntityFilter(definition.preselection, EntityType.FACE);
        }
        else if (!isQueryEmpty(context, qEntityFilter(definition.preselection, EntityType.EDGE)))
        {
            definition.selectionType = BodyDraftSelectionType.EDGES;
            definition.topEdges = qEntityFilter(definition.preselection, EntityType.EDGE);
        }
        definition.preselection = qNothing();
    }
    if (specifiedParameters.pullDirection != true && isQueryEmpty(context, definition.pullDirection) &&
        (oldDefinition.partingObject == undefined || isQueryEmpty(context, oldDefinition.partingObject)))
    {
        const plane = try silent(evPlane(context, {
                        "face" : definition.partingObject
                    }));
        if (plane == undefined)
        {
            return definition;
        }
        definition.pullDirection = definition.partingObject;
    }

    if (specifiedParameters.partingObject != true && isQueryEmpty(context, definition.partingObject) &&
        (oldDefinition.pullDirection == undefined || isQueryEmpty(context, oldDefinition.pullDirection)))
    {
        // pull direction allows edges but parting object does not.
        if (isQueryEmpty(context, qEntityFilter(definition.pullDirection, EntityType.EDGE)) &&
            !isQueryEmpty(context, definition.pullDirection))
        {
            definition.partingObject = definition.pullDirection;
        }
    }
    return definition;
}

/**
 * @internal
 * Manipulator change function for `bodyDraft` feature.
 */
export function bodyDraftManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    if (newManipulators[flipManipulatorId] != undefined)
    {
        definition.flipPullDirection = !definition.flipPullDirection;
    }
    return definition;
}

