FeatureScript ✨; /* Automatically generated version */
export import(path : "onshape/std/feature.fs", version : "");
export import(path : "onshape/std/boolean.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/manipulator.fs", version : "");
export import(path : "onshape/std/vector.fs", version : "");


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


annotation { "Feature Type Name" : "Revolve", "Manipulator Change Function" : "revolveManipulatorChange", "Filter Selector" : "allparts" }
export const revolve = defineFeature(function(context is Context, id is Id, revolveDefinition is map)
    precondition
    {
        annotation { "Name" : "Creation type" }
        revolveDefinition.bodyType is ToolBodyType;

        if (revolveDefinition.bodyType != ToolBodyType.SURFACE)
        {
            booleanStepTypePredicate(revolveDefinition);
        }

        if (revolveDefinition.bodyType == ToolBodyType.SOLID)
        {
            annotation { "Name" : "Faces and sketch regions to revolve",
                         "Filter" : (EntityType.FACE && GeometryType.PLANE) && ConstructionObject.NO }
            revolveDefinition.entities is Query;
        }
        else
        {
            annotation { "Name" : "Edges and sketch curves to revolve",
                         "Filter" : EntityType.EDGE && ConstructionObject.NO }
            revolveDefinition.surfaceEntities is Query;
        }

        annotation { "Name" : "Revolve axis", "Filter" : QueryFilterCompound.ALLOWS_AXIS, "MaxNumberOfPicks" : 1 }
        revolveDefinition.axis is Query;

        annotation { "Name" : "Revolve type" }
        revolveDefinition.revolveType is RevolveType;

        if (revolveDefinition.revolveType != RevolveType.SYMMETRIC
            && revolveDefinition.revolveType != RevolveType.FULL)
        {
            annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
            revolveDefinition.oppositeDirection is boolean;
        }

        if (revolveDefinition.revolveType != RevolveType.FULL)
        {
            annotation { "Name" : "Revolve angle" }
            isAngle(revolveDefinition.angle, ANGLE_360_BOUNDS);
        }

        if (revolveDefinition.revolveType == RevolveType.TWO_DIRECTIONS)
        {
            annotation { "Name" : "Second revolve angle" }
            isAngle(revolveDefinition.angleBack, ANGLE_360_BOUNDS);
        }

        if (revolveDefinition.bodyType != ToolBodyType.SURFACE)
        {
            booleanStepScopePredicate(revolveDefinition);
        }
    }
    {
        revolveDefinition.entities = getEntitiesToUse(context, revolveDefinition);
        var resolvedEntities = evaluateQuery(context, revolveDefinition.entities);
        if (@size(resolvedEntities) == 0)
        {
            if (revolveDefinition.bodyType == ToolBodyType.SOLID)
                reportFeatureError(context, id, ErrorStringEnum.REVOLVE_SELECT_FACES, ["entities"]);
            else
                reportFeatureError(context, id, ErrorStringEnum.REVOLVE_SURF_NO_CURVE, ["surfaceEntities"]);
            return;
        }

        var axis = evAxis(context, revolveDefinition);
        if (axis.error != undefined)
        {
            reportFeatureError(context, id, ErrorStringEnum.REVOLVE_SELECT_AXIS, ["axis"]);
            return;
        }
        revolveDefinition.axis = axis.result;

        addRevolveManipulator(context, id, revolveDefinition);

        if (revolveDefinition.revolveType == RevolveType.FULL)
        {
            revolveDefinition.angleForward = 2 * PI;
            revolveDefinition.angleBack = 0;
        }
        if (revolveDefinition.revolveType == RevolveType.ONE_DIRECTION)
        {
            revolveDefinition.angleForward = revolveDefinition.angle;
            revolveDefinition.angleBack = 0;
        }
        if (revolveDefinition.revolveType == RevolveType.SYMMETRIC)
        {
            revolveDefinition.angleForward = revolveDefinition.angle / 2;
            revolveDefinition.angleBack = revolveDefinition.angle / 2;
        }
        if (revolveDefinition.revolveType == RevolveType.TWO_DIRECTIONS)
        {
            revolveDefinition.angleForward = revolveDefinition.angle;
        }
        if (revolveDefinition.oppositeDirection)
        {
            revolveDefinition.axis.direction *= -1; // To be consistent with extrude
        }
        opRevolve(context, id, revolveDefinition);

        if (revolveDefinition.bodyType == ToolBodyType.SOLID)
        {
            if (!processNewBodyIfNeeded(context, id, revolveDefinition))
            {
                var statusToolId = id + "statusTools";
                startFeature(context, statusToolId, revolveDefinition);
                opRevolve(context, statusToolId, revolveDefinition);
                setBooleanErrorEntities(context, id, statusToolId);
                endFeature(context, statusToolId);
            }
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

    var entities = getEntitiesToUse(context, revolveDefinition);

    //Compute manipulator parameters
    var revolvePoint;
    var faceResult = evFaceTangentPlane(context, { "face" : qNthElement(entities, 0), "parameter" : vector(0.5, 0.5) });
    if (faceResult.result != undefined)
    {
        revolvePoint = faceResult.result.origin;
    }
    else
    {
        var edgeResult = evEdgeTangentLine(context, { "edge" : qNthElement(entities, 0), "parameter" : 0.5 });
        if (edgeResult.result != undefined)
            revolvePoint = edgeResult.result.origin;
        else
            return;
    }
    var axisOrigin = project(revolveDefinition.axis, revolvePoint);

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

export function revolveManipulatorChange(context is Context, revolveDefinition is map, newManipulators is map) returns map
precondition
{
    newManipulators[ANGLE_MANIPULATOR] is Manipulator;
    revolveDefinition.revolveType == RevolveType.ONE_DIRECTION || revolveDefinition.revolveType == RevolveType.SYMMETRIC;
}
{
    var newAngle = newManipulators[ANGLE_MANIPULATOR].angle;

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

