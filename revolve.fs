FeatureScript ✨; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/tool.fs", version : "");

// Features using manipulators must export manipulator.fs
export import(path : "onshape/std/manipulator.fs", version : "");

// Imports used internally
import(path : "onshape/std/boolean.fs", version : "");
import(path : "onshape/std/evaluate.fs", version : "");
import(path : "onshape/std/feature.fs", version : "");
import(path : "onshape/std/mathUtils.fs", version : "");
import(path : "onshape/std/valueBounds.fs", version : "");

/**
 * TODO: description
 */
export enum RevolveType
{
    annotation { "Name" : "Full" }
    FULL,
    annotation { "Name" : "One direction" }
    ONE_DIRECTION,
    annotation { "Name" : "Symmetric" }
    SYMMETRIC,
    annotation { "Name" : "Two directions" }
    TWO_DIRECTIONS
}

/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
annotation { "Feature Type Name" : "Revolve", "Manipulator Change Function" : "revolveManipulatorChange", "Filter Selector" : "allparts" }
export const revolve = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Creation type" }
        definition.bodyType is ToolBodyType;

        if (definition.bodyType != ToolBodyType.SURFACE)
        {
            booleanStepTypePredicate(definition);
        }

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            annotation { "Name" : "Faces and sketch regions to revolve",
                         "Filter" : (EntityType.FACE && GeometryType.PLANE) && ConstructionObject.NO }
            definition.entities is Query;
        }
        else
        {
            annotation { "Name" : "Edges and sketch curves to revolve",
                         "Filter" : EntityType.EDGE && ConstructionObject.NO }
            definition.surfaceEntities is Query;
        }

        annotation { "Name" : "Revolve axis", "Filter" : QueryFilterCompound.ALLOWS_AXIS, "MaxNumberOfPicks" : 1 }
        definition.axis is Query;

        annotation { "Name" : "Revolve type" }
        definition.revolveType is RevolveType;

        if (definition.revolveType != RevolveType.SYMMETRIC
            && definition.revolveType != RevolveType.FULL)
        {
            annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
            definition.oppositeDirection is boolean;
        }

        if (definition.revolveType != RevolveType.FULL)
        {
            annotation { "Name" : "Revolve angle" }
            isAngle(definition.angle, ANGLE_360_BOUNDS);
        }

        if (definition.revolveType == RevolveType.TWO_DIRECTIONS)
        {
            annotation { "Name" : "Second revolve angle" }
            isAngle(definition.angleBack, ANGLE_360_BOUNDS);
        }

        if (definition.bodyType != ToolBodyType.SURFACE)
        {
            booleanStepScopePredicate(definition);
        }
    }
    {
        definition.entities = getEntitiesToUse(context, definition);
        const resolvedEntities = evaluateQuery(context, definition.entities);
        if (@size(resolvedEntities) == 0)
        {
            if (definition.bodyType == ToolBodyType.SOLID)
                throw regenError(ErrorStringEnum.REVOLVE_SELECT_FACES, ["entities"]);
            else
                throw regenError(ErrorStringEnum.REVOLVE_SURF_NO_CURVE, ["surfaceEntities"]);
            return;
        }

        definition.axis = try(evAxis(context, definition));
        if (definition.axis == undefined)
            throw regenError(ErrorStringEnum.REVOLVE_SELECT_AXIS, ["axis"]);

        addRevolveManipulator(context, id, definition);

        if (definition.revolveType == RevolveType.FULL)
        {
            definition.angleForward = 2 * PI;
            definition.angleBack = 0;
        }
        if (definition.revolveType == RevolveType.ONE_DIRECTION)
        {
            definition.angleForward = definition.angle;
            definition.angleBack = 0;
        }
        if (definition.revolveType == RevolveType.SYMMETRIC)
        {
            definition.angleForward = definition.angle / 2;
            definition.angleBack = definition.angle / 2;
        }
        if (definition.revolveType == RevolveType.TWO_DIRECTIONS)
        {
            definition.angleForward = definition.angle;
        }
        if (definition.oppositeDirection)
        {
            definition.axis.direction *= -1; // To be consistent with extrude
        }
        opRevolve(context, id, definition);

        if (definition.bodyType == ToolBodyType.SOLID)
        {
            const reconstructOp = function(id) { opRevolve(context, id, definition); };
            processNewBodyIfNeeded(context, id, definition, reconstructOp);
        }
    }, { bodyType : ToolBodyType.SOLID, oppositeDirection : false, operationType : NewBodyOperationType.NEW });

//Manipulator functions

const ANGLE_MANIPULATOR = "angleManipulator";

function getEntitiesToUse(context is Context, revolveDefinition is map)
{
    if (revolveDefinition.bodyType == ToolBodyType.SOLID)
    {
        return revolveDefinition.entities;
    }
    else
    {
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V177_CONSTRUCTION_OBJECT_FILTER))
        {
            return qConstructionFilter(revolveDefinition.surfaceEntities, ConstructionObject.NO);
        }
        else
        {
            return revolveDefinition.surfaceEntities;
        }
    }
}

function addRevolveManipulator(context is Context, id is Id, revolveDefinition is map)
{
    if (revolveDefinition.revolveType != RevolveType.ONE_DIRECTION && revolveDefinition.revolveType != RevolveType.SYMMETRIC )
        return;

    const entities = getEntitiesToUse(context, revolveDefinition);

    //Compute manipulator parameters
    var revolvePoint;
    const faceResult = try(evFaceTangentPlane(context, { "face" : qNthElement(entities, 0), "parameter" : vector(0.5, 0.5) }));
    if (faceResult != undefined)
    {
        revolvePoint = faceResult.origin;
    }
    else
    {
        const edgeResult = try(evEdgeTangentLine(context, { "edge" : qNthElement(entities, 0), "parameter" : 0.5 }));
        if (edgeResult != undefined)
            revolvePoint = edgeResult.origin;
        else
            return;
    }
    const axisOrigin = project(revolveDefinition.axis, revolvePoint);

    var minValue = -2 * PI * radian;
    var maxValue = 2 * PI * radian;

    //Compute value
    var angle = revolveDefinition.angle;
    if (revolveDefinition.oppositeDirection == true)
        angle *= -1;
    if (revolveDefinition.revolveType == RevolveType.SYMMETRIC)
    {
        angle *= .5;
        minValue = -PI * radian;
        maxValue = PI * radian;
    }


    addManipulators(context, id, { (ANGLE_MANIPULATOR) :
                        angularManipulator({ "axisOrigin" : axisOrigin,
                                             "axisDirection" : revolveDefinition.axis.direction,
                                             "rotationOrigin" : revolvePoint,
                                             "angle" : angle,
                                             "sources" : entities,
                                             "minValue" : minValue,
                                             "maxValue" : maxValue })});
}

/**
 * TODO: description
 * @param context
 * @param revolveDefinition {{
 *      @field TODO
 * }}
 * @param newManipulators {{
 *      @field TODO
 * }}
 */
export function revolveManipulatorChange(context is Context, revolveDefinition is map, newManipulators is map) returns map
precondition
{
    newManipulators[ANGLE_MANIPULATOR] is Manipulator;
    revolveDefinition.revolveType == RevolveType.ONE_DIRECTION || revolveDefinition.revolveType == RevolveType.SYMMETRIC;
}
{
    const newAngle = newManipulators[ANGLE_MANIPULATOR].angle;

    revolveDefinition.oppositeDirection = newAngle < 0 * radian;
    revolveDefinition.angle = abs(newAngle);

    if (revolveDefinition.revolveType == RevolveType.SYMMETRIC)
    {
        revolveDefinition.angle *= 2;
        if (revolveDefinition.angle > 2 * PI * radian)
        {
            // for the effect of one-directional manip loop
            revolveDefinition.angle = 4 * PI * radian - revolveDefinition.angle;
        }
    }
    return revolveDefinition;
}

