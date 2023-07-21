FeatureScript 2091; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2091.0");
export import(path : "onshape/std/variabletype.gen.fs", version : "2091.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "2091.0");
import(path : "onshape/std/debug.fs", version : "2091.0");
import(path : "onshape/std/evaluate.fs", version : "2091.0");
import(path : "onshape/std/feature.fs", version : "2091.0");
import(path : "onshape/std/string.fs", version : "2091.0");
import(path : "onshape/std/tool.fs", version : "2091.0");
import(path : "onshape/std/valueBounds.fs", version : "2091.0");
import(path : "onshape/std/manipulator.fs", version : "2091.0");
import(path : "onshape/std/vector.fs", version : "2091.0");
import(path : "onshape/std/curveGeometry.fs", version : "2091.0");
import(path : "onshape/std/topologyUtils.fs", version : "2091.0");
import(path : "onshape/std/defaultFeatures.fs", version : "2091.0");
import(path : "onshape/std/coordSystem.fs", version : "2091.0");

/**
 * Whether the variable is measured or assigned.
 */
export enum VariableMode
{
    annotation { "Name" : "Assigned" }
    ASSIGNED,
    annotation { "Name" : "Measured" }
    MEASURED
}

/**
 * Whether to measure distance or length.
 */
export enum VariableMeasurementMode
{
    annotation { "Name" : "Distance" }
    DISTANCE,
    annotation { "Name" : "Length" }
    LENGTH,
    annotation { "Name" : "Diameter" }
    DIAMETER
}

/**
 * Axis selection
 */
export enum AxisWithCustom
{
    annotation { "Name" : "Distance" }
    DISTANCE,
    annotation { "Name" : "Along X" }
    X,
    annotation { "Name" : "Along Y" }
    Y,
    annotation { "Name" : "Along Z" }
    Z,
    annotation { "Name" : "Along custom" }
    CUSTOM
}

/**
 * Min or max selection
 */
export enum VariableMinMaxSelection
{
    annotation { "Name" : "Minimum" }
    MINIMUM,
    annotation { "Name" : "Maximum" }
    MAXIMUM
}

const AXIS_COLORS = [
    DebugColor.RED,
    DebugColor.GREEN,
    DebugColor.BLUE
];

/**
 * Feature performing a `setVariable` allowing a user to assign a FeatureScript
 * value to a context variable. This variable may be retrieved within a feature by
 * calling `getVariable`, or in a Part Studio using `#` syntax (e.g. `#foo`)
 * inside any parameter which allows an expression (including the `value`
 * parameter of another variable!)
 *
 * @param definition {{
 *      @field mode {VariableMode} : Whether the variable is measured or assigned.
 *      @field name {string} : Must be an identifier.
 *      @field description {string} : Description of the variable. Maximum length of 256 characters.
 *      @field variableType {VariableType} : The type of variable.  If it is not ANY, the value is restricted
 *          to be a length, angle, or number and is passed through the `lengthValue`, `angleValue`, or `numberValue`
 *          field, respectively.
 *      @field lengthValue {ValueWithUnits} : Used if `variableType` is `LENGTH`.
 *      @field angleValue {ValueWithUnits} : Used if `variableType` is `ANGLE`.
 *      @field numberValue {number} : Used if `variableType` is `NUMBER`.
 *      @field anyValue : Used if `variableType` is `ANY`.  Can be any immutable FeatureScript value, including a length, an array, or a function.
 *      @field measurementMode {VariableMeasurementMode} : Whether to measure distance, length, or diameter.
 *      @field entityCouple {Query} : Query for distance mode, containing two entites to measure the distance between.
 *      @field minmax {VariableMinMaxSelection} : Whether to measure the minimum or maximum distance.
 *      @field axis {AxisWithCustom} : Axis to measure distance along.
 *      @field extendEntities {boolean} : Extend selected planes and lines out to infinity. Incompatible with maximum.
 *      @field measureFromAxis {boolean} : Measure from the axis of geometry with an axis, rather than from the edge. Incompatible with maximum.
 *      @field componentSelector {AxisWithCustom} : Select an axis to measure along.
 *      @field customDirection {Query} : If componentSelector is CUSTOM, a query containing geometry with a directon to measure along.
 *      @field lengthEntities {Query} : Query for length mode, containing entities to measure the length of.
 *      @field radius {boolean} : Whether to measure the radius rather than the diameter.
 *      @field diameterEntity {Query} : Query for diameter mode, containing an entity to measure the diameter of.
 * }}
 */
annotation { "Feature Type Name" : "Variable", "Feature Name Template" : "###name = #value", "UIHint" : UIHint.NO_PREVIEW_PROVIDED,
        "Tooltip Template" : "###name = #value #description",
        "Editing Logic Function" : "variableEditLogic" }
export const assignVariable = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities", "UIHint" : UIHint.ALWAYS_HIDDEN, "Filter" : (EntityType.FACE || EntityType.EDGE || EntityType.VERTEX || EntityType.BODY || BodyType.MATE_CONNECTOR) && AllowFlattenedGeometry.YES }
        definition.initEntities is Query;

        annotation { "Name" : "Mode", "UIHint" : UIHint.HORIZONTAL_ENUM }
        definition.mode is VariableMode;

        if (definition.mode == VariableMode.ASSIGNED)
        {
            annotation { "Name" : "Variable type", "UIHint" : ["HORIZONTAL_ENUM", "UNCONFIGURABLE"] }
            definition.variableType is VariableType;
        }
        else if (definition.mode == VariableMode.MEASURED)
        {
            annotation { "Name" : "Measurement", "UIHint" : UIHint.HORIZONTAL_ENUM, "Description" : "Select distance to measure the linear distance between two entities./nSelect length to measure the total length of the selected entities." }
            definition.measurementMode is VariableMeasurementMode;
        }

        annotation { "Name" : "Name", "UIHint" : [UIHint.UNCONFIGURABLE, UIHint.VARIABLE_NAME], "MaxLength" : 10000 }
        definition.name is string;

        if (definition.mode == VariableMode.ASSIGNED)
        {
            if (definition.variableType == VariableType.LENGTH)
            {
                annotation { "Name" : "Value", "UIHint" : UIHint.INITIAL_FOCUS_ON_EDIT }
                isLength(definition.lengthValue, ZERO_DEFAULT_LENGTH_BOUNDS);
            }
            else if (definition.variableType == VariableType.ANGLE)
            {
                annotation { "Name" : "Value", "UIHint" : UIHint.INITIAL_FOCUS_ON_EDIT }
                isAngle(definition.angleValue, ANGLE_360_ZERO_DEFAULT_BOUNDS);
            }
            else if (definition.variableType == VariableType.NUMBER)
            {
                annotation { "Name" : "Value", "UIHint" : UIHint.INITIAL_FOCUS_ON_EDIT }
                isReal(definition.numberValue, { (unitless) : [-1e12, 0, 1e12] } as RealBoundSpec);
            }
            else if (definition.variableType == VariableType.ANY)
            {
                annotation { "Name" : "Value", "UIHint" : UIHint.INITIAL_FOCUS_ON_EDIT }
                isAnything(definition.anyValue);
            }

            annotation { "UIHint" : UIHint.ALWAYS_HIDDEN }
            isAnything(definition.value);
        }
        else if (definition.mode == VariableMode.MEASURED)
        {
            if (definition.measurementMode == VariableMeasurementMode.DISTANCE)
            {
                annotation { "Name" : "Entities to measure between", "UIHint" : UIHint.PREVENT_CREATING_NEW_MATE_CONNECTORS,
                            "Filter" : (EntityType.FACE || EntityType.EDGE || EntityType.VERTEX || EntityType.BODY || BodyType.MATE_CONNECTOR) && AllowFlattenedGeometry.YES,
                            "MaxNumberOfPicks" : 2 }
                definition.entityCouple is Query;

                annotation { "Name" : "Minimum/Maximum",
                            "Default" : VariableMinMaxSelection.MINIMUM,
                            "Description" : "Choose to measure minimum or maximum distance between entities" }
                definition.minmax is VariableMinMaxSelection;

                if (definition.minmax != VariableMinMaxSelection.MAXIMUM)
                {
                    annotation { "Name" : "Extend entities", "Default" : false,
                                "Description" : "Measure to infinitely extended lines, arcs, cylinders, etc." }
                    definition.extendEntities is boolean;

                    annotation { "Name" : "Use axis", "Default" : false,
                                "Description" : "Measure to axis of cylinders, circles, arcs, or revolved surfaces" }
                    definition.measureFromAxis is boolean;
                }

                annotation { "Group Name" : "Axis measurements", "Collapsed By Default" : true }
                {
                    annotation { "Name" : "Distance", "UIHint" : UIHint.READ_ONLY }
                    isLength(definition.distance, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

                    annotation { "Name" : "Along X", "UIHint" : UIHint.READ_ONLY }
                    isLength(definition.xOffset, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

                    annotation { "Name" : "Along Y", "UIHint" : UIHint.READ_ONLY }
                    isLength(definition.yOffset, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

                    annotation { "Name" : "Along Z", "UIHint" : UIHint.READ_ONLY }
                    isLength(definition.zOffset, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);

                    annotation { "Name" : "Value to record", "UIHint" : UIHint.SHOW_LABEL }
                    definition.componentSelector is AxisWithCustom;

                    if (definition.componentSelector == AxisWithCustom.CUSTOM)
                    {
                        annotation { "Name" : "Custom direction", "Filter" : QueryFilterCompound.ALLOWS_DIRECTION, "MaxNumberOfPicks" : 1 }
                        definition.customDirection is Query;

                        annotation { "Name" : "Along custom", "UIHint" : UIHint.READ_ONLY }
                        isLength(definition.customOffset, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
                    }
                }
            }
            else if (definition.measurementMode == VariableMeasurementMode.LENGTH)
            {
                annotation { "Name" : "Entities", "Filter" : (EntityType.EDGE || (EntityType.BODY && BodyType.WIRE)) && AllowFlattenedGeometry.YES }
                definition.lengthEntities is Query;
            }
            else if (definition.measurementMode == VariableMeasurementMode.DIAMETER)
            {
                annotation { "Name" : "Radius" }
                definition.radius is boolean;

                annotation { "Name" : "Entity", "Filter" : (GeometryType.CIRCLE || GeometryType.ARC || GeometryType.CYLINDER || GeometryType.SPHERE) && AllowFlattenedGeometry.YES, MaxNumberOfPicks : 1 }
                definition.diameterEntity is Query;
            }
        }

        annotation { "Name" : "Description", "MaxLength" : 256, "Default" : "" }
        definition.description is string;
    }
    {
        if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V1846_BEND_LINE_ATTACHED))
        {
            verifyVariableName(definition.name, "name");
        }

        var value;

        if (definition.mode == VariableMode.ASSIGNED)
        {
            if (definition.variableType == VariableType.ANGLE &&
                !isAtVersionOrLater(context, FeatureScriptVersionNumber.V694_FILL_GUIDE_CURVES_FS))
            {
                definition.angleValue = adjustAngle(context, definition.angleValue);
            }

            if (definition.variableType == VariableType.LENGTH)
                value = definition.lengthValue;
            else if (definition.variableType == VariableType.ANGLE)
                value = definition.angleValue;
            else if (definition.variableType == VariableType.NUMBER)
                value = definition.numberValue;
            else if (definition.variableType == VariableType.ANY)
                value = definition.anyValue;

            if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V424_VARIABLE_WARNINGS))
            {
                if (value == undefined)
                {
                    reportFeatureWarning(context, id, ErrorStringEnum.VARIABLE_CANNOT_EVALUATE);
                }
            }
        }
        else if (definition.mode == VariableMode.MEASURED)
        {
            if (definition.measurementMode == VariableMeasurementMode.DISTANCE)
            {
                verifyNonemptyQuery(context, definition, "entityCouple", ErrorStringEnum.VARIABLE_SELECT_FIRST_ENTITY);

                var hasFlatEntities = false;
                if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V1846_BEND_LINE_ATTACHED))
                {
                    hasFlatEntities = !isQueryEmpty(context, definition.entityCouple->qSheetMetalFlatFilter(SMFlatType.YES));
                    if (hasFlatEntities && size(evaluateQuery(context, qOwnerBody(definition.entityCouple))) > 1)
                    {
                        throw regenError(ErrorStringEnum.VARIABLE_FLATTENED_ENTITIES_MUST_BE_SAME_BODY);
                    }
                }

                if (size(evaluateQuery(context, definition.entityCouple)) == 1)
                {
                    throw regenError(ErrorStringEnum.VARIABLE_SELECT_SECOND_ENTITY);
                }

                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1846_BEND_LINE_ATTACHED))
                {
                    const flatOwnerBodySideA = getFlatOwnerBody(context, qNthElement(definition.entityCouple, 0));
                    const flatOwnerBodySideB = getFlatOwnerBody(context, qNthElement(definition.entityCouple, 1));
                    hasFlatEntities = flatOwnerBodySideA != undefined || flatOwnerBodySideB != undefined;

                    // If both entities are in flats, check that they're in the same flat.
                    if (flatOwnerBodySideA != undefined && flatOwnerBodySideB != undefined)
                    {
                        if (size(evaluateQuery(context, qUnion(flatOwnerBodySideA, flatOwnerBodySideB))) > 1)
                        {
                            throw regenError(ErrorStringEnum.VARIABLE_FLATTENED_ENTITIES_MUST_BE_SAME_BODY);
                        }
                    }
                    // Else, if either entity is in a flat, we're measuring between a flat and something else, so throw.
                    else if (hasFlatEntities)
                    {
                        throw regenError(ErrorStringEnum.VARIABLE_FLATTENED_ENTITIES_MUST_BE_SAME_BODY);
                    }
                }

                if (definition.componentSelector == AxisWithCustom.CUSTOM)
                {
                    verifyNonemptyQuery(context, definition, "customDirection", ErrorStringEnum.VARIABLE_SELECT_CUSTOM_DIRECTION);
                }

                const distanceResultMap = measureDistance(context, {
                            entities : definition.entityCouple,
                            measureFromAxis : definition.measureFromAxis,
                            customDirection : definition.customDirection,
                            extendEntities : definition.extendEntities,
                            maximum : definition.minmax == VariableMinMaxSelection.MAXIMUM
                        });
                value = selectDistanceComponent(distanceResultMap, definition.componentSelector);

                if (!hasFlatEntities)
                {
                    try silent
                    {
                        addDebugLine(context, distanceResultMap.firstPoint, distanceResultMap.secondPoint, DebugColor.MAGENTA);
                        if (definition.componentSelector != AxisWithCustom.DISTANCE && definition.componentSelector != AxisWithCustom.CUSTOM)
                        {
                            addAxisDecompositionDebugLines(context, distanceResultMap.firstPoint, distanceResultMap.secondPoint);
                        }
                        if (definition.componentSelector == AxisWithCustom.CUSTOM)
                        {
                            var customDirection = extractDirection(context, definition.customDirection);
                            if (dot(customDirection, normalize(distanceResultMap.secondPoint - distanceResultMap.firstPoint)) < 0)
                            {
                                customDirection *= -1;
                            }
                            addDebugLine(context, distanceResultMap.firstPoint, distanceResultMap.firstPoint + value * customDirection, DebugColor.CYAN);
                        }
                    }
                }

                // Show axis decomposition in the feature dialog
                setFeatureComputedParameter(context, id, { "name" : "distance", "value" : selectDistanceComponent(distanceResultMap, AxisWithCustom.DISTANCE) });
                setFeatureComputedParameter(context, id, { "name" : "xOffset", "value" : selectDistanceComponent(distanceResultMap, AxisWithCustom.X) });
                setFeatureComputedParameter(context, id, { "name" : "yOffset", "value" : selectDistanceComponent(distanceResultMap, AxisWithCustom.Y) });
                setFeatureComputedParameter(context, id, { "name" : "zOffset", "value" : selectDistanceComponent(distanceResultMap, AxisWithCustom.Z) });
                if (definition.componentSelector == AxisWithCustom.CUSTOM)
                {
                    setFeatureComputedParameter(context, id, { "name" : "customOffset", "value" : selectDistanceComponent(distanceResultMap, AxisWithCustom.CUSTOM) });
                }
                setHighlightedEntities(context, { "entities": definition.entityCouple });
            }
            else if (definition.measurementMode == VariableMeasurementMode.LENGTH)
            {
                verifyNonemptyQuery(context, definition, "lengthEntities", ErrorStringEnum.VARIABLE_SELECT_ENTITIES_TO_MEASURE);
                value = evLength(context, {
                    "entities" : definition.lengthEntities
                });
                setHighlightedEntities(context, { "entities": definition.lengthEntities });
            }
            else if (definition.measurementMode == VariableMeasurementMode.DIAMETER)
            {
                verifyNonemptyQuery(context, definition, "diameterEntity", ErrorStringEnum.VARIABLE_SELECT_ENTITY_TO_MEASURE);
                value = measureDiameter(context, {
                    entity: definition.diameterEntity,
                    radius: definition.radius
                });
                setHighlightedEntities(context, { "entities": definition.diameterEntity });
            }
        }

        verifyVariableName(definition.name, "name");
        publishVariableValue(definition.name, context, id, value);
    },
    {
        // Default mode is ASSIGNED.
        mode : VariableMode.ASSIGNED,
        description : "",
        variableType : VariableType.ANY,

        // If mode is set to MEASURED, the default measurement mode is DISTANCE.
        measurementMode : VariableMeasurementMode.DISTANCE,
        componentSelector : AxisWithCustom.DISTANCE,

        distance : 0 * meter,
        xOffset : 0 * meter,
        yOffset : 0 * meter,
        zOffset : 0 * meter,
        customOffset : 0 * meter,

        entityCouple : qNothing(),
        lengthEntities : qNothing(),
        diameterEntity : qNothing(),

        // MEASURED mode options are set such that the ability to enable them is true, but the default value is false.
        // In the case of the UI, the editing logic handles updating these defaults to their appropriate values.
        minmax: VariableMinMaxSelection.MINIMUM,
        extendEntities: false,
        measureFromAxis: false,
        radius: false,
        initEntities: qNothing()
    });

/**
 * Throws an error if `name` is not a valid identifier.
 */
export function verifyVariableName(name is string, faultyParameter is string)
{
    if (length(name) > 10000)
        throw regenError(ErrorStringEnum.VARIABLE_NAME_TOO_LONG, [faultyParameter]);
    const replaceNameWithRegExpShouldBeBlank = replace(name, '[a-zA-Z_][a-zA-Z_0-9]*', '');
    if (name == '' || replaceNameWithRegExpShouldBeBlank != '')
        throw regenError(ErrorStringEnum.VARIABLE_NAME_INVALID, [faultyParameter]);
}

function publishVariableValue(name is string, context is Context, id is Id, value)
{
    setVariable(context, name, value);
    const quotedValue = (value is string) ? ('"' ~ value ~ '"') : value;
    setFeatureComputedParameter(context, id, { "name" : "value", "value" : quotedValue });
}

/**
 * Make a function to look up variables from the given context.  Used in generated part studio code.
 */
export function makeLookupFunction(context is Context, id is Id) returns function
{
    return function(name is string)
        {
            return getVariable(context, name);
        };
}

/** Measure the distance between two entities.
 * @param context {Context}
 * @param arg {{
 *     @field entities {Query} : a query containing two entities to measure the distance between.
 *     @field measureFromAxis {boolean} : measure from the axis of the selected geometry, if an axis is available. Incompatible with the `maximum` option. Defaults to `false`. @optional
 *     @field customDirection {Query} : measure the distance between two entities along a direction selected by this query. @optional
 *     @field extendEntities {boolean} : extend the entities out to infinity. Incompatible with the `maximum` option. Defaults to `false`. @optional
 *     @field maximum {boolean} : measure the maximum distance between the selected entities, instead of the minimum by default. Defaults to `false`. @optional
 * }}
 */
export function measureDistance(context is Context, arg is map) returns map
precondition
{
    arg.entities is Query;
    arg.measureFromAxis == undefined || arg.measureFromAxis is boolean;
    arg.customDirection == undefined || arg.customDirection is Query;
    arg.extendEntities == undefined || arg.extendEntities is boolean;
    arg.maximum == undefined || arg.maximum is boolean;
}
{
    const maximum = arg.maximum == true;
    const measureFromAxis = !maximum && arg.measureFromAxis == true;
    const extendEntities = !maximum && arg.extendEntities == true;

    const flattenedEntitiesMap = prepareSideEntities(context, arg.entities, measureFromAxis);

    const sideASet = flattenedEntitiesMap.axisEntitiesMap.linesAside == [] ? flattenedEntitiesMap.entityAQuery : flattenedEntitiesMap.axisEntitiesMap.linesAside;
    const sideBSet = flattenedEntitiesMap.axisEntitiesMap.linesBside == [] ? flattenedEntitiesMap.entityBQuery : flattenedEntitiesMap.axisEntitiesMap.linesBside;

    const extendSideA = extendEntities || sideContainsConstructionPlanes(context, sideASet);
    const extendSideB = extendEntities || sideContainsConstructionPlanes(context, sideBSet);

    if ((extendSideA || extendSideB) && maximum) {
        throw regenError(ErrorStringEnum.VARIABLE_CANNOT_USE_MAXIMUM_WITH_INFINITE_ENTITIES);
    }

    var distanceResult is DistanceResult = evDistance(context, {
            "side0" : sideASet,
            "side1" : sideBSet,
            "extendSide0" : extendSideA,
            "extendSide1" : extendSideB,
            "maximum" : maximum
        });

    const entityOnSideA = getSideEntity(sideASet, distanceResult, 0);
    const entityOnSideB = getSideEntity(sideBSet, distanceResult, 1);

    distanceResult = recomputeForInfiniteEntitiesIfNeeded(context, extendEntities, entityOnSideA, entityOnSideB, distanceResult);

    const distanceMap = {
        "distance" : distanceResult.distance,
        "firstPoint" : distanceResult.sides[0].point,
        "secondPoint" : distanceResult.sides[1].point,
        "side0" : entityOnSideA,
        "side1" : entityOnSideB,
        "customDirection" : arg.customDirection
    };

    return addAxisDecomposition(context, distanceMap);
}

/**
 * Returns appropriate entry in distanceMap returned from measureDistance given the selected AxisWithCustom.
 */
export function selectDistanceComponent(distanceMap is map, componentSelector is AxisWithCustom)
{
    return switch (componentSelector)
        {
                AxisWithCustom.DISTANCE : distanceMap.distance,
                AxisWithCustom.X : distanceMap.xOffset,
                AxisWithCustom.Y : distanceMap.yOffset,
                AxisWithCustom.Z : distanceMap.zOffset,
                AxisWithCustom.CUSTOM : distanceMap.customOffset
            };
}

/** Measure the diameter (or radius) of the selected entity.
 * @param context {Context}
 * @param arg {{
 *     @field entity {Query} : a query containing an entity of GeometryType.CIRCLE, GeometryType.ARC, GeometryType.CYLINDER, or GeometryType.SPHERE.
 *     @field radius {boolean} : measure the radius instead of the diameter. Defaults to `false`. @optional
 * }}
 */
export function measureDiameter(context is Context, arg is map)
precondition
{
    arg.entity is Query;
    arg.radius == undefined || arg.radius is boolean;
}
{
    var curveQuery = qUnion([
            qGeometry(arg.entity, GeometryType.CIRCLE),
            qGeometry(arg.entity, GeometryType.ARC)
        ]);

    var surfaceQuery = qUnion([
            qGeometry(arg.entity, GeometryType.CYLINDER),
            qGeometry(arg.entity, GeometryType.SPHERE)
        ]);

    var radius;
    if (evaluateQuery(context, curveQuery) != [])
    {
        radius = evCurveDefinition(context, { "edge" : curveQuery }).radius;
    }
    else if (evaluateQuery(context, surfaceQuery) != [])
    {
        radius = evSurfaceDefinition(context, { "face" : surfaceQuery }).radius;
    }
    else
    {
        throw regenError(ErrorStringEnum.VARIABLE_NO_GEOMETRY_WITH_DIAMETER);
    }

    return  arg.radius == true ? radius : 2 * radius;
}

/**
 * Processing of the initial queries before distance evaluation
 */
function prepareSideEntities(context is Context, entities is Query, axisMode is boolean) returns map
{
    var entityAQuery = undefined;
    var entityBQuery = undefined;

    if (size(evaluateQuery(context, entities)) > 2)
    {
        throw regenError(ErrorStringEnum.VARIABLE_ONLY_TWO_ENTITIES_ALLOWED);
    }
    entityAQuery = splitToSubEntities(qNthElement(entities, 0));
    entityBQuery = splitToSubEntities(qNthElement(entities, 1));

    var axisEntitiesMap = {
        "linesAside" : [],
        "indexAside" : [],
        "linesBside" : [],
        "indexBside" : []
    };

    // Calculate axis map only when "Use axis" option is active.
    if (axisMode)
    {
        // Locate possible axis entities on side A and B.
        var axisMapSideA = replaceWithCenterLines(context, entityAQuery);
        var axisMapSideB = replaceWithCenterLines(context, entityBQuery);
        if (axisMapSideA != { "axisArray" : [], "indexArray" : [] } || axisMapSideB != { "axisArray" : [], "indexArray" : [] })
        {
            axisEntitiesMap = {
                    "linesAside" : axisMapSideA.axisArray,
                    "indexAside" : axisMapSideA.indexArray,
                    "linesBside" : axisMapSideB.axisArray,
                    "indexBside" : axisMapSideB.indexArray
                };
        }
        else
        {
            throw regenError(ErrorStringEnum.VARIABLE_NO_AXIS_ENTITIES);
        }
    }

    return {
            "entityAQuery" : entityAQuery,
            "entityBQuery" : entityBQuery,
            "axisEntitiesMap" : axisEntitiesMap
        };
}

/**
 * Turn a body query into a query for its sub-entities.
 */
function splitToSubEntities(queryToSplit is Query) returns Query
{
    return qSubtraction(qUnion([queryToSplit, qOwnedByBody(queryToSplit)]), qEntityFilter(queryToSplit, EntityType.BODY));
}

/**
 * Replace side entities with their axes (excludes lines, planes, and mate connectors).
 */
function replaceWithCenterLines(context, entityQuery is Query) returns map
{
    var axisArray = [];
    var indexArray = [];

    for (var i, singleEntity in evaluateQuery(context, entityQuery))
    {
        const axis = getAxis(context, singleEntity);
        if (axis != undefined)
        {
            axisArray = append(axisArray, axis);
            indexArray = append(indexArray, i);
        }
    }
    return { "axisArray" : axisArray, "indexArray" : indexArray };
}

/**
 * Returns axis of the entity if it belongs to GeometryType ARC, CIRCLE, CYLINDER, CONE, TORUS, or REVOLVED.
 */
function getAxis(context is Context, entity is Query)
{
    const axis = try silent(evAxis(context, { "axis" : entity }));
    if (axis == undefined)
    {
        return undefined;
    }

    const approvedGeometryTypes = [
            GeometryType.ARC,
            GeometryType.CIRCLE,
            GeometryType.CYLINDER,
            GeometryType.CONE,
            GeometryType.TORUS,
            GeometryType.REVOLVED
        ];

    const queriesForApprovedGeometryTypes = mapArray(approvedGeometryTypes, function(geomType)
        {
            return qGeometry(entity, geomType);
        });
    if (!isQueryEmpty(context, qUnion(queriesForApprovedGeometryTypes)))
        return axis;
    else
        return undefined;
}

/**
 * Computes the different axis measurements for a distance measurement.
 */
function addAxisDecomposition(context is Context, distanceMap is map) returns map
{
    var axisDecomposition =
    {
        "xOffset" : 0 * meter,
        "yOffset" : 0 * meter,
        "zOffset" : 0 * meter,
        "customOffset" : 0 * meter,
        "pointA" : undefined,
        "pointB" : undefined
    };
    if (distanceMap.distance != 0 * meter)
    {
        // Calculate offset components.
        axisDecomposition.xOffset = abs(distanceMap.firstPoint[0] - distanceMap.secondPoint[0]);
        axisDecomposition.yOffset = abs(distanceMap.firstPoint[1] - distanceMap.secondPoint[1]);
        axisDecomposition.zOffset = abs(distanceMap.firstPoint[2] - distanceMap.secondPoint[2]);

        axisDecomposition.pointA = vector([distanceMap.secondPoint[0], distanceMap.firstPoint[1], distanceMap.firstPoint[2]]);
        axisDecomposition.pointB = vector([distanceMap.secondPoint[0], distanceMap.secondPoint[1], distanceMap.firstPoint[2]]);

        // Calculate projection length on custom vector.
        if (distanceMap.customDirection != undefined && !isQueryEmpty(context, distanceMap.customDirection))
        {
            const direction = extractDirection(context, distanceMap.customDirection);
            const projectLine = line(WORLD_ORIGIN, direction);

            var pointCA = project(projectLine, distanceMap.firstPoint);
            var pointCB = project(projectLine, distanceMap.secondPoint);
            var customDistance = norm(pointCB - pointCA);

            axisDecomposition = mergeMaps(axisDecomposition, { "customOffset" : customDistance, "pointCA" : pointCA, "pointCB" : pointCB });
        }
    }
    return mergeMaps(distanceMap, axisDecomposition);
}

function addAxisDecompositionDebugLines(context is Context, from is Vector, to is Vector)
{
    const difference = to - from;
    var current = from;
    for (var axis = 0; axis < 3; axis += 1)
    {
        var next = current;
        next[axis] += difference[axis];
        addDebugLine(context, current, next, AXIS_COLORS[axis]);
        current = next;
    }
}

/**
 * For one of the sides of a DistanceResult, return the entity or line where the extremum was found.
 */
function getSideEntity(sideInput, distanceResult is DistanceResult, sideIndex is number)
{
    var entityIndex = distanceResult.sides[sideIndex].index;
    if (sideInput is Query)
    {
        return qNthElement(sideInput, entityIndex);
    }
    else
    {
        return sideInput[entityIndex];
    }
}

/**
 * When we have two infinite entities, there may be an infinite number of closest points, so recompute from one of the sides.
 * This lets us put the debug line on the actual entities.
 */
function recomputeForInfiniteEntitiesIfNeeded(context is Context, extendEntities is boolean, side0, side1, distanceResult is map) returns map
{
    const lineSide0 = getLineFromSide(context, side0);
    const lineSide1 = getLineFromSide(context, side1);

    if (lineSide0 == undefined || lineSide1 == undefined || !parallelVectors(lineSide0.direction, lineSide1.direction))
    {
        return distanceResult;
    }

    // If either of the sides is an infinite line (from axis entities), or we're extending edges, recompute.
    if (side0 is Line || side1 is Line || extendEntities)
    {
        distanceResult.sides[0].point = lineSide0.origin;
        distanceResult.sides[1].point = evDistance(context, {
                        "side0" : lineSide1,
                        "side1" : distanceResult.sides[0].point
                    }).sides[0].point;
    }

    return distanceResult;
}

/**
 * Return line defnition from side when it is a Line or a linear edge.
 */
function getLineFromSide(context is Context, sideEntity)
{
    if (sideEntity is Line)
    {
        return sideEntity;
    }
    else if (sideEntity is Query && !isQueryEmpty(context, qGeometry(sideEntity, GeometryType.LINE)))
    {
        return evLine(context, { "edge" : sideEntity });
    }
    return undefined;
}

/**
 * @internal
 * Editing logic function for populating the displayValue parameter
 */
export function variableEditLogic(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean) returns map
{
    if (definition.mode == VariableMode.ASSIGNED)
    {
        definition.value = switch (definition.variableType) {
                    VariableType.LENGTH : copyParameter("lengthValue"),
                    VariableType.ANGLE : copyParameter("angleValue"),
                    VariableType.NUMBER : copyParameter("numberValue"),
                    VariableType.ANY : copyParameter("anyValue")
                };
    }
    else
    {
        definition.value = "?";
    }

    if (oldDefinition == {} && !isQueryEmpty(context, definition.initEntities))
    {
        if (definition.mode == VariableMode.MEASURED)
        {
            if (definition.measurementMode == VariableMeasurementMode.DISTANCE)
            {
                definition.entityCouple = definition.initEntities;
            }
            else if (definition.measurementMode == VariableMeasurementMode.LENGTH)
            {
                definition.lengthEntities = qUnion(
                  definition.initEntities->qEntityFilter(EntityType.EDGE),
                  definition.initEntities->qBodyType(BodyType.WIRE)
                );
            }
            else if (definition.measurementMode == VariableMeasurementMode.DIAMETER)
            {
                definition.diameterEntity = qUnion([
                    definition.initEntities->qGeometry(GeometryType.CIRCLE),
                    definition.initEntities->qGeometry(GeometryType.ARC),
                    definition.initEntities->qGeometry(GeometryType.CYLINDER),
                    definition.initEntities->qGeometry(GeometryType.SPHERE)
                ])->qNthElement(0);
            }
        }
        definition.initEntities = qNothing();
    }

    return definition;
}

function sideContainsConstructionPlanes(context is Context, side) returns boolean
{
    if (side is Query)
    {
        return !isQueryEmpty(context, side->qConstructionFilter(ConstructionObject.YES)->qGeometry(GeometryType.PLANE));
    }
    return false;
}

function getFlatOwnerBody(context is Context, entity is Query)
{
    const smFlatOwnerBodyQuery = qUnion(
        entity->qSheetMetalFlatFilter(SMFlatType.YES)->qOwnerBody(), // regular flat entities
        qPartsAttachedTo(entity) // bend lines
    )->qBodyType(BodyType.SOLID); // solid because sheet metal sketches will get the sketch body and the flat
    return isQueryEmpty(context, smFlatOwnerBodyQuery) ? undefined : smFlatOwnerBodyQuery;
}

