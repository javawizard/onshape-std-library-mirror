FeatureScript 2144; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used in interface
export import(path : "onshape/std/query.fs", version : "2144.0");
export import(path : "onshape/std/surfaceGeometry.fs", version : "2144.0");

// Imports used internally
import(path : "onshape/std/attributes.fs", version : "2144.0");
import(path : "onshape/std/booleanoperationtype.gen.fs", version : "2144.0");
import(path : "onshape/std/box.fs", version : "2144.0");
import(path : "onshape/std/containers.fs", version : "2144.0");
import(path : "onshape/std/coordSystem.fs", version : "2144.0");
import(path : "onshape/std/evaluate.fs", version : "2144.0");
import(path : "onshape/std/extrude.fs", version : "2144.0");
import(path : "onshape/std/feature.fs", version : "2144.0");
import(path : "onshape/std/holeAttribute.fs", version : "2144.0");
import(path : "onshape/std/math.fs", version : "2144.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2144.0");
import(path : "onshape/std/sketch.fs", version : "2144.0");
import(path : "onshape/std/tool.fs", version : "2144.0");
import(path : "onshape/std/transform.fs", version : "2144.0");
import(path : "onshape/std/units.fs", version : "2144.0");
import(path : "onshape/std/vector.fs", version : "2144.0");
import(path : "onshape/std/curveGeometry.fs", version : "2144.0");

// Expand bounding box by 1% for purposes of creating cutting geometry
const BOX_TOLERANCE = 0.01;
// Expand bounding box absolutely a small amount to account for bounding boxes with zero length in a dimension
const BOX_ABSOLUTE_TOLERANCE = 1e-5 * meter;

// ATTENTION DEVELOPERS:
// If you version a fix to functionality used in section view
// Bump SBTAppElementViewVersionNumber and change BTPartStudioRenderingAgent.getSectionVersion()
// to return new FS version For new views

//Given a plane definition and a input part query will return a list of bodies that one needs to delete so that
//the only bodies that remain are the ones split by the plane, unless none are split by the plane, in which case
//the only bodies that remain are the ones behind the plane. Used by drawings to render a section view
function performSectionCutAndGetBodiesToDelete(context is Context, id is Id, plane is Plane, partToSection is Query) returns Query
{
    var allBodies = qBodyType(qEverything(EntityType.BODY), BodyType.SOLID);

    const useTightBox = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V932_SPLIT_PART_BOX);
    // The bbox of the body in plane coordinate system with positive z being in front of the plane
    const boxResult = evBox3d(context, { 'topology' : partToSection,
                                         'cSys' : planeToCSys(plane),
                                         'tight' : useTightBox });

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
    const cplaneDefinition =
    {
        "plane" : plane,
        "width" : 1 * meter,
        "height" : 1 * meter
    };

    const planeId = id + "plane";
    opPlane(context, planeId, cplaneDefinition);
    const planeTool = qOwnerBody(qCreatedBy(planeId));

    //The plane needs to be deleted so that it is not processed as a section face
    allBodies = qUnion([allBodies, planeTool]);

    // Split part on plane
    const splitPartDefinition =
    {
        "targets" : partToSection,
        "tool" : planeTool,
        "keepTools" : false
    };

    const splitPartId = id + "splitPart";
    opSplitPart(context, splitPartId, splitPartDefinition);
    const splitPartResultQ = qSplitBy(splitPartId, EntityType.BODY, true);
    //Split plane might miss the body because the box was not sufficiently accurate
    //check body location by midBox z coordinate
    if (!useTightBox && isQueryEmpty(context, splitPartResultQ))
    {
        // Correct middle z coordinate and version it
        var midBox;
        if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1532_CORRECT_Z_MID_COORDINATE))
        {
            midBox = 0.5 * (boxResult.minCorner[2] + boxResult.maxCorner[2]);
        }
        else
        {
            midBox = 0.5 * (boxResult.minCorner[2] + boxResult.maxCorner[0]);
        }

        if (midBox < TOLERANCE.zeroLength * meter)
        {
            return qSubtraction(allBodies, partToSection);
        }
        else
        {
            return allBodies;
        }
    }

    // Split was success. Retain everything behind the plane
    return qSubtraction(allBodies, splitPartResultQ);
}

//Section Part Feature

/**
 * Feature creating a section of a part behind a plane. Internally, performs
 * an [opSplitPart] followed by an [opDeleteBodies].
 *
 * Not exposed in the Part Studio UI.
 */
// Drawings don't have an upgrade process like part studios, so this cannot be changed or it will break
// drawing queries.
export const sectionPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        definition.targets is Query;
        definition.plane is Plane;
    }
    {
        retainHoleAttributes(context, definition.targets);
        var bodiesToDelete = qEverything(EntityType.BODY); // Delete everything if there's an error
        try
        {
            bodiesToDelete = performSectionCutAndGetBodiesToDelete(context, id, definition.plane, definition.targets);
        }
        // TODO: how are errors reported?
        const deleteBodiesId = id + "deleteBody";
        opDeleteBodies(context, deleteBodiesId, { "entities" : bodiesToDelete });
    });


/**
 * Split a set of parts with a plane and delete all bodies in front of the face. Unlike sectionPart, bodies which are
 * entirely behind the split plane are retained. Any bodies not included in the target query are deleted.
 * @param definition {{
 *      @field target {Query} : Bodies to be split.
 *      @field plane {Plane} :  Plane that splits the bodies. Everything
 *              on the positive z side of the plane will be removed.
 * }}
 */
export const planeSectionPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        definition.target is Query;
        definition.plane is Plane;
    }
    {
        // define sketch plane orthogonal to the cut plane, with the cut plane's x axis as the sketch plane's normal
        // and the cut plane's reversed normal (jog & plane section have reversed sense of cutting plane direction)
        // as the x axis, which is an arbitrary choice.
        const sketchPlane = plane(definition.plane.origin, definition.plane.x, -definition.plane.normal);

        const coordinateSystem = planeToCSys(sketchPlane);

        // The bbox of the bodies in sketchPlane coordinate system with positive x being in front of the plane
        const useTightBox = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V932_SPLIT_PART_BOX);
        var boxResult = evBox3d(context, { 'topology' : definition.target,
                                           'cSys' : coordinateSystem,
                                           'tight' : useTightBox });

        // Boolean remove will complain if we don't hit anything
        if (boxResult.maxCorner[0] < -TOLERANCE.zeroLength * meter)
        {
            return;
        }

        // Extend the box slightly to make sure we get everything
        boxResult = extendBox3d(boxResult, 0 * meter, BOX_TOLERANCE);

        // Define the "jog section" points as a line extending across the bounding box y extents
        const cutPoints = [ toWorld(coordinateSystem, vector(0 * meter, boxResult.minCorner[1], boxResult.minCorner[2])),
                            toWorld(coordinateSystem, vector(0 * meter, boxResult.maxCorner[1], boxResult.minCorner[2])) ];
        definition.jogPoints = convertToPointsArray(false, cutPoints, []);
        definition.sketchPlane = sketchPlane;
        jogSectionCut(context, id, definition);
    }, {"isPartialSection" : false, "keepSketches" : false, "isBrokenOut" : false, "isCropView" : false, "isAlignedSection" : false });

/**
 * Split a part down a jogged section line and delete all back bodies. Used by drawings. Needs to be a feature
 * so that drawings created by queries can resolve. Any bodies not included in the target query are deleted.
 * @param definition {{
 *      @field target {Query} : Bodies to be split.
 *      @field sketchPlane {Plane} :  Plane that the jog line will be drawn in and extruded normal to. Everything
 *                                    on the positive x side of the jog line will be removed.
 *      @field jogPoints {array} : Points that the cutting line goes through in world coordinates.
 *      @field isPartialSection {boolean} : Whether or not it is a partial section cut.
 *      @field keepSketches {boolean} : Whether or not sketches will be kept in the section cut results.
 *      @field isBrokenOut {boolean} : Whether or not it is a broken-out section cut.
 *      @field isCropView {boolean} : Whether or not it is a crop section cut.
 *      @field brokenOutPointNumbers {array} : Array of the number of spline points of each broken-out section cut.
 *      @field brokenOutEndConditions {array} : Array of end conditions of each broken-out section cut.
 *      @field offsetPoints {array} : Array of points for offsetting the section lines.
 *      @field isAlignedSection {boolean} : Whether or not it is an aligned section cut.
* }}
 */
export const jogSectionPart = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        definition.target is Query;
        definition.sketchPlane is Plane;
        definition.jogPoints is array;
        for (var point in definition.jogPoints)
        {
            if (point != undefined)
            {
                is3dLengthVector(point);
            }
        }
        if (definition.bbox != undefined)
        {
            definition.bbox is Box3d;
        }
        definition.isPartialSection is boolean;
        definition.keepSketches is boolean;
        definition.isBrokenOut is boolean;
        definition.isCropView is boolean;
        definition.brokenOutPointNumbers is array;
        definition.brokenOutEndConditions is array;
        definition.offsetPoints is array;
        definition.isAlignedSection is boolean;
    }
    {
        const numberOfPoints = definition.jogPoints != undefined ? size(definition.jogPoints) : 0;
        const brokenOutPointNumbers = definition.brokenOutPointNumbers != undefined ? definition.brokenOutPointNumbers : [];
        definition.jogPoints = convertToPointsArray(definition.isBrokenOut || definition.isCropView, definition.jogPoints, brokenOutPointNumbers);
        definition.offsetDistances = definition.brokenOutEndConditions != undefined ? getOffsetDistancesArray(definition.brokenOutEndConditions) : [];
        jogSectionCut(context, id, definition);
    }, {isPartialSection : false, keepSketches : false, isBrokenOut : false, isCropView : false, brokenOutPointNumbers : [],
              brokenOutEndConditions : [], offsetPoints : [], isAlignedSection : false });

/**
 * @internal
 * Array parameter entry for sectionTransformedParts definition.targets
 * When using for assembly section transformations are occurrence cumulative transformations,
 * instanceNames are compressed occurrence pathes.
 * @type{{
 *      @field part {Query} : bodies to be patterned and sectioned.
 *      @field transformations {array} : array of transformations to be used for part pattern.
 *      @field instanceNames {array} : array of strings, same size as transformations to be used as identities of pattern instances.
 *      }}
 */
export type SectionTarget typecheck canBeSectionTarget;

/** @internal */
export predicate canBeSectionTarget(value)
{
    value is map;
    value.part is Query;
    value.transformations is array;
    for (var transform in value.transformations)
    {
        transform is Transform;
    }
    value.instanceNames is array;
    size(value.transformations) == size(value.instanceNames);
    for (var name in value.instanceNames)
    {
        name is string;
    }
}

/** @internal */
export function sectionTarget(instanceNames is array, part is Query, transformations is array) returns SectionTarget
{
    return  {'instanceNames' : instanceNames,  'part' : part, 'transformations' : transformations} as SectionTarget;
}

/**
 * @internal
 * method for processing all part studio parts for assembly section
 */
export const sectionTransformedParts = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        definition.targets is array;
        for (var target in definition.targets)
        {
            target is SectionTarget;
        }
        definition.sketchPlane is Plane;
        definition.jogPoints is array;
        for (var point in definition.jogPoints)
        {
            if (point != undefined)
            {
                is3dLengthVector(point);
            }
        }
        if (definition.bbox != undefined)
        {
            definition.bbox is Box3d;
        }
        definition.isPartialSection is boolean;
        definition.isBrokenOut is boolean;
        definition.isCropView is boolean;
        definition.keepSketches is boolean;
        definition.brokenOutPointNumbers is array;
        definition.brokenOutEndConditions is array;
        definition.offsetPoints is array;
        definition.isAlignedSection is boolean;
    }
    {
        // remove sheet metal attributes and helper bodies
        clearSheetMetalData(context, id + "sheetMetal", undefined, isAtVersionOrLater(context, FeatureScriptVersionNumber.V1246_SM_SECTION_PART_FIXES));
        //Collect patterned parts
        var allTargetParts = [];
        definition.sectionIndexToExcludeTargets = {};
        if (definition.excludedOccurrencesMap == undefined)
        {
            definition.excludedOccurrencesMap = {};
        }
        for (var i = 0; i < size(definition.targets); i += 1)
        {
            var returnMap = patternTarget(context, id, id + unstableIdComponent(i), definition.targets[i],
                                          definition.excludedOccurrencesMap, definition.sectionIndexToExcludeTargets);
            allTargetParts = append(allTargetParts, returnMap.targetQuery);
            definition.sectionIndexToExcludeTargets = returnMap.sectionIndexToExcludeTargets;
        }

        //making a single array from array of arrays
        allTargetParts = concatenateArrays(allTargetParts);
        const targetQ = qUnion(allTargetParts);
        definition.target = targetQ;

        const numberOfPoints = definition.jogPoints != undefined ? size(definition.jogPoints) : 0;
        const brokenOutPointNumbers = definition.brokenOutPointNumbers != undefined ? definition.brokenOutPointNumbers : [];
        definition.jogPoints = convertToPointsArray(definition.isBrokenOut || definition.isCropView, definition.jogPoints, brokenOutPointNumbers);
        definition.offsetDistances = definition.brokenOutEndConditions != undefined ? getOffsetDistancesArray(definition.brokenOutEndConditions) : [];
        const offsetPoints = definition.offsetPoints != undefined ? definition.offsetPoints : [];

        jogSectionCut(context, id + "cut", definition);

        // need to transform the cut results to their original posistion in the part studio
        for (var i = 0; i < size(definition.targets); i += 1)
        {
            transformCutResult(context, id, id + unstableIdComponent(i ~ "move"), definition.targets[i]);
        }
    }, {isPartialSection : false, isBrokenOut : false, isCropView : false, keepSketches : false,
            brokenOutPointNumbers : [], brokenOutEndConditions : [], offsetPoints : [], isAlignedSection : false });

function patternTarget(context is Context, parentId is Id, id is Id, args is SectionTarget, excludedOccurrences is map, sectionIndexToExcludeTargets is map) returns map
{
    // for [1188, 1237), unpack composites before pattern
    // for further versions, unpack composites after pattern
    const unpackAfterPattern = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1237_ASSEMBLY_SECTION_CUT_FIXES);
    const unpackBeforePattern = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1188_FS_EXPOSE_COMPOSITE_PARTS) && !unpackAfterPattern;

    var query = args.part;
    if (unpackBeforePattern)
    {
        query = qFlattenedCompositeParts(query);
    }

    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V2040_SECTION_VIEW_FIX_COMPOSITE_APPEARANCE))
    {
        // If we have any open composite parts, also include any closed composites containing its constituents to keep appearance.
        const flattened = qFlattenedCompositeParts(query);
        query = qUnion(query, qCompositePartsContaining(flattened, CompositePartType.CLOSED));
    }
    opPattern(context, id, {
            "entities" : query,
            "transforms" : args.transformations,
            "instanceNames" : args.instanceNames
            });
    query = qCreatedBy(id, EntityType.BODY);
    var returnMap = {};
    var sectionIndexToExcludeTargetsLocal = sectionIndexToExcludeTargets;
    for (var i = 0; i < size(args.instanceNames); i += 1)
    {
        var instance = qPatternInstances(id, args.instanceNames[i], EntityType.BODY);
        var instanceQuery = evaluateQuery(context, qFlattenedCompositeParts(instance));
        instance = qUnion([instance, startTracking(context, instance)]);
        setAttribute(context, {
                "entities" : instance,
                "name" : parentId ~ args.instanceNames[i],
                "attribute" : parentId ~ args.instanceNames[i]
        });
        if (excludedOccurrences != undefined)
        {
            var indices = excludedOccurrences[args.instanceNames[i]];
            if (indices != undefined)
            {
                for (var j = 0; j < size(indices); j += 1)
                {
                    sectionIndexToExcludeTargetsLocal = insertIntoMapOfArrays(sectionIndexToExcludeTargetsLocal, indices[j], instanceQuery);
                }
            }
        }
    }
    if (unpackAfterPattern)
    {
        query = qFlattenedCompositeParts(query);
    }
    returnMap.targetQuery = evaluateQuery(context, query);
    returnMap.sectionIndexToExcludeTargets = sectionIndexToExcludeTargetsLocal;
    return returnMap;
}

function transformCutResult(context is Context, parentId is Id, moveId is Id, args is SectionTarget)
{
    for (var i = 0; i < size(args.instanceNames); i += 1)
    {
        var instanceQuery = qHasAttribute(parentId ~ args.instanceNames[i]);
        // it could be empty, i.e., the instance is not cut at all
        if (isQueryEmpty(context, instanceQuery))
        {
            continue;
        }

        opTransform(context, moveId + unstableIdComponent(i), {
                    "bodies" : instanceQuery,
                    "transform" : inverse(args.transformations[i])
                });
    }
}

/**
 * @internal
 * Calling this method will clear all intermediate sheet metal data, limited to internal use only
 */
export const jogSectionPartInternal = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        definition.target is Query;
        definition.sketchPlane is Plane;
        definition.jogPoints is array;
        for (var point in definition.jogPoints)
        {
            if (point != undefined)
            {
                is3dLengthVector(point);
            }
        }
        if (definition.bbox != undefined)
        {
            definition.bbox is Box3d;
        }
        definition.isPartialSection is boolean;
        definition.keepSketches is boolean;
        definition.isBrokenOut is boolean;
        definition.isCropView is boolean;
        definition.brokenOutPointNumbers is array;
        definition.brokenOutEndConditions is array;
        definition.offsetPoints is array;
        definition.isAlignedSection is boolean;
    }
    {
        // remove sheet metal attributes and helper bodies
        clearSheetMetalData(context, id + "sheetMetal", undefined, isAtVersionOrLater(context, FeatureScriptVersionNumber.V1246_SM_SECTION_PART_FIXES));
        const brokenOutPointNumbers = definition.brokenOutPointNumbers != undefined ? definition.brokenOutPointNumbers : [];
        definition.jogPoints = convertToPointsArray(definition.isBrokenOut || definition.isCropView, definition.jogPoints, brokenOutPointNumbers);
        definition.offsetDistances  = definition.brokenOutEndConditions != undefined ? getOffsetDistancesArray(definition.brokenOutEndConditions) : [];
        jogSectionCut(context, id, definition);
    }, {isPartialSection : false, keepSketches : false, isBrokenOut : false, isCropView : false, brokenOutPointNumbers : [], brokenOutEndConditions : [],
            offsetPoints : [], isAligendSection : false });

/**
 * Collect the spline points and the depth point from each broken-out section and convert it into an array of array
 * for broken-out section, we store the spline points and depth points for multiple broken-out sections in 'jogPoints'.
 * Format of 'jogPoints': [pointsForBrokenOut1, depthPoint1, pointsForBrokenOut2, depthPoint2, ...]
 * 'brokenOutPointNumbers' tells us how many points each broken-out section has: [size(pointsForBrokenOut1), size(pointsForBrokenOut2), ...]
 */
function convertToPointsArray(isBrokenOutOrCropView is boolean, jogPoints is array, brokenOutPointNumbers is array)
{
    var jogPointsArray;
    if (isBrokenOutOrCropView && brokenOutPointNumbers != undefined && size(brokenOutPointNumbers) > 0)
    {
        var jogPointIndex = 0;
        var numberOfBrokenOut = size(brokenOutPointNumbers);
        jogPointsArray = makeArray(numberOfBrokenOut);
        for (var i = 0; i < numberOfBrokenOut; i = i + 1)
        {
            const numberOfJogPoints = brokenOutPointNumbers[i];
            var points = makeArray(numberOfJogPoints + 1); // need to add an additional one for the depth point
            for (var j = 0; j < numberOfJogPoints + 1; j = j + 1)
            {
                points[j] = jogPoints[jogPointIndex];
                jogPointIndex = jogPointIndex + 1;
            }
            jogPointsArray[i] = points;
        }
    }
    else
    {
        jogPointsArray = makeArray(1);
        jogPointsArray[0] = jogPoints;
    }
    return jogPointsArray;
}

function getOffsetDistancesArray(endConditions is array)
{
    var offsetDistancesArray = [];
    if (endConditions != undefined && size(endConditions) > 0)
    {
        var numberOfOffset = size(endConditions);
        offsetDistancesArray = makeArray(numberOfOffset);
        for (var i = 0; i < numberOfOffset; i = i + 1)
        {
            var offset = undefined;
            if (endConditions[i] != undefined && endConditions[i].hasOffset && endConditions[i].offsetDistance != undefined)
            {
                offset = endConditions[i].offsetDistance;
                if (endConditions[i].offsetOppositeDirection)
                {
                    offset *= -1;
                }
            }
            offsetDistancesArray[i] = offset;
        }
    }
    return offsetDistancesArray;
}

type HoleAttributesOnPart typecheck canBeHoleAttributesOnPart;

predicate canBeHoleAttributesOnPart(value)
{
    value is array;
    for (var item in value)
        item is HoleAttribute;
}

function retainHoleAttributes(context is Context, bodies is Query)
{
    for (var part in evaluateQuery(context, qBodyType(bodies, BodyType.SOLID)))
    {
        if (getAttributes(context, {
                "entities" : part,
                "attributePattern" : [] as HoleAttributesOnPart
        }) == [])
        {
            var holeAttributes = getHoleAttributes(context, qOwnedByBody(part, EntityType.FACE));
            setAttribute(context, {
                        "entities" : part,
                        "attribute" : holeAttributes as HoleAttributesOnPart
                    });
        }
    }
}

function jogSectionCut(context is Context, id is Id, definition is map)
{
    const target = qUnion([qBodyType(definition.target, BodyType.SOLID), qBodyType(definition.target, BodyType.SHEET)]);
    const targetTracking = qUnion([target, startTracking(context, target)]);
    const sketchPlane = definition.sketchPlane;
    const bboxIn = definition.bbox;
    const isPartialSection = definition.isPartialSection;
    const jogPointsArray = definition.jogPoints;
    const offsetDistancesArray = definition.offsetDistances;
    const keepSketches = definition.keepSketches;
    const isBrokenOut = definition.isBrokenOut;
    const isCropView = definition.isCropView;
    const offsetPoints = definition.offsetPoints;
    const versionOperationUse = (definition.versionOperationUse == true);
    const isAlignedSection = definition.isAlignedSection;

    var toKeepQ = target;
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1218_SECTION_PART_KEEP_COMPOSITES)) {
        toKeepQ = qUnion([target, qCompositePartsContaining(target)]);
    }
    var toDeleteQ = qSubtraction(qEverything(EntityType.BODY), toKeepQ);
    if (keepSketches)
    {
        toDeleteQ = qSketchFilter(toDeleteQ, SketchObject.NO);
    }
    try silent(opDeleteBodies(context, id + "initialDelete", { "entities" : toDeleteQ }));

    retainHoleAttributes(context, qEverything(EntityType.BODY));

    try
    {
        var bboxInSketchCS = undefined;
        if (bboxIn != undefined)
        {
            // convert the bbox from world coordinate system to the sketch plane
            bboxInSketchCS = transformBox3d(bboxIn, fromWorld(planeToCSys(sketchPlane)));
        }
        if (isBrokenOut || isCropView)
        {
            var sectionIndexToExcludeTargets = definition.sectionIndexToExcludeTargets != undefined ? definition.sectionIndexToExcludeTargets : {};
            brokenOutSectionCut(context, id, target, sketchPlane, bboxInSketchCS, jogPointsArray, offsetDistancesArray,
                                isCropView, versionOperationUse, sectionIndexToExcludeTargets);
        }
        else if (jogPointsArray != undefined && size(jogPointsArray) == 1)
        {
            const jogPoints = jogPointsArray[0];
            const coordinateSystem = planeToCSys(sketchPlane);
            const useTightBox = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V932_SPLIT_PART_BOX);
            var boxResult = evBox3d(context, { 'topology' : target,
                                               'cSys' : coordinateSystem,
                                               'tight' : useTightBox });
            boxResult = extendBox3d(boxResult, 0 * meter, BOX_TOLERANCE);
            // Shift the plane and box to the box's min corner
            var origin = toWorld(coordinateSystem, boxResult.minCorner);
            const offsetPlane = plane(origin, sketchPlane.normal, sketchPlane.x);
            boxResult.maxCorner = boxResult.maxCorner - boxResult.minCorner;
            boxResult.minCorner = vector(0, 0, 0) * meter;
            const numberOfPoints = size(jogPoints);
            var projectedPoints = makeArray(numberOfPoints);
            for (var i = 0; i < numberOfPoints; i = i + 1)
            {
                projectedPoints[i] = worldToPlane(offsetPlane, jogPoints[i]);
            }
            if (!isAlignedSection)
            {
                checkJogDirection(projectedPoints);
            }
            // check if this is a section cut with offset
            var isOffsetCut = false;
            var offsetDistance = 0.0 * meter;
            const hasOffsetDistance = offsetDistancesArray != undefined && size(offsetDistancesArray) == 1 && offsetDistancesArray[0] != undefined;
            const offsetPoint = offsetPoints != undefined && size(offsetPoints) > 0 ? offsetPoints[0] : undefined;
            if (numberOfPoints == 2 && (hasOffsetDistance || offsetPoint != undefined))
            {
                isOffsetCut = true;
                if (hasOffsetDistance)
                {
                    offsetDistance = offsetDistancesArray[0];
                }
                // adjust the offset distance if the offset point is specified
                if (offsetPoint != undefined)
                {
                    const projectedOffsetPoint = worldToPlane(offsetPlane, offsetPoint);
                    offsetDistance += (projectedOffsetPoint[0] - projectedPoints[0][0]);
                }
                // we do not allow negative offset. If it happens, we treat it as a regular section cut
                if (offsetDistance < 0)
                {
                    isOffsetCut = false;
                }
            }

            var polygon;
            if (isOffsetCut)
            {
                polygon = createJogPolygonForOffsetCut(projectedPoints, boxResult, offsetPlane, offsetDistance);
            }
            else if (isPartialSection)
            {
                polygon = createJogPolygonForPartialSection(context, projectedPoints, boxResult, offsetPlane);
            }
            else if (isAlignedSection)
            {
                polygon = createJogPolygonForAlignedSection(projectedPoints, boxResult, offsetPlane);
            }
            else
            {
                polygon = createJogPolygon(projectedPoints, boxResult, offsetPlane);
            }

            // Only need to track single/multi parts in section/aligned section view generation for Part Studio because metadata can
            // be linked to newly created bodies (eg., rotated body in aligned section view) in assemblies
            var partIds = [];
            for (var partTarget in evaluateQuery(context, target))
            {
                var detId = partTarget.transientId;
                // Encode the deterministic id in the attribute name, which can be used to find both the parent and children bodies
                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1634_ALIGNED_SECTION_BODY_MAP))
                {
                    setAttribute(context, {
                            "entities" : partTarget,
                            "name" : id ~ detId,
                            "attribute" : id ~ detId
                    });
                }
                else
                {
                    setAttribute(context, {
                            "entities" : partTarget,
                            "attribute" : {
                                "name" : id ~ detId
                            }
                    });
                }
                // Track all the composites that contain the current part, encode its part id in the query
                if (isAlignedSection && isAtVersionOrLater(context, FeatureScriptVersionNumber.V1651_MODIFY_COMPOSITE_BY_ROTATED_BODY))
                {
                    var compQ = qCompositePartsContaining(partTarget);
                    partIds = append(partIds, detId);
                    if (!isQueryEmpty(context, compQ))
                    {
                        setAttribute(context, {
                                "entities" : compQ,
                                "name" : "composite" ~ id ~ detId,
                                "attribute" : "composite" ~ id ~ detId
                        });
                    }
                }
            }

            sketchAndExtrudeCut(context, id, target, polygon, offsetPlane, sketchPlane, boxResult.maxCorner[2], versionOperationUse, isOffsetCut);
            if (isAlignedSection && !isQueryEmpty(context, targetTracking))
            {
                definition.target = targetTracking;
                var sectionFacesQuery = alignedSectionRotateAndCut(context, id, definition, partIds);
                if (!isQueryEmpty(context, sectionFacesQuery))
                {
                    setAttribute(context, {
                            "entities" : sectionFacesQuery,
                            "attribute" : {
                                "name" : id ~ "sectionFaces"
                            }
                    });
                }
                if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1651_MODIFY_COMPOSITE_BY_ROTATED_BODY) &&
                    !isAtVersionOrLater(context, FeatureScriptVersionNumber.V1670_MODIFY_COMPOSITE_BEFORE_EXTRUDE_CUT))
                {
                    addToComposites(context, id, partIds);
                }
            }
        }
    }
    catch
    {
        opDeleteBodies(context, id + "delete", { "entities" : qEverything(EntityType.BODY) });
    }
}

/**
 * 'jogPointsArray' is an array of array, each array contains the spline section points and the depth point
 */
function brokenOutSectionCut(context is Context, id is Id, target is Query, sketchPlane is Plane, bboxIn, jogPointsArray is array,
                            offsetDistancesArray is array, isCropView is boolean, versionOperationUse is boolean, sectionIndexToExcludeTargets is map)
{
    const coordinateSystem = planeToCSys(sketchPlane);
    const useTightBox = !isAtVersionOrLater(context, FeatureScriptVersionNumber.V932_SPLIT_PART_BOX);
    var boxResult = bboxIn != undefined ? bboxIn : evBox3d(context, { 'topology' : target,
                                       'cSys' : coordinateSystem,
                                       'tight' : useTightBox });
    if (bboxIn == undefined || (isCropView && isAtVersionOrLater(context, FeatureScriptVersionNumber.V1618_ENLARGE_CROP_VIEW_BBOX)))
    {
        boxResult = extendBox3d(boxResult, BOX_ABSOLUTE_TOLERANCE, BOX_TOLERANCE);
    }
    // Shift the plane and box to the box's min corner
    var defaultOrigin = toWorld(coordinateSystem, boxResult.minCorner);
    var numberOfBrokenOut = size(jogPointsArray);
    for (var brokenOutIndex = 0; brokenOutIndex < numberOfBrokenOut; brokenOutIndex = brokenOutIndex + 1)
    {
        var targetCurrentSection = target;
        if (sectionIndexToExcludeTargets != undefined && sectionIndexToExcludeTargets[brokenOutIndex] != undefined)
        {
            for (var j = 0; j < size(sectionIndexToExcludeTargets[brokenOutIndex]); j = j + 1)
            {
                targetCurrentSection = qSubtraction(targetCurrentSection, qUnion(sectionIndexToExcludeTargets[brokenOutIndex][j]));
            }
        }
        if (isQueryEmpty(context, targetCurrentSection))
        {
            continue;
        }
        const jogPoints = jogPointsArray[brokenOutIndex];
        var numberOfJogPoints = size(jogPoints) - 1;
        var jogPointsForBrokenOut = makeArray(numberOfJogPoints);
        for (var i = 0; i < numberOfJogPoints; i = i + 1)
        {
            jogPointsForBrokenOut[i] = jogPoints[i];
        }
        var uptoPoint = jogPoints[numberOfJogPoints]; //the last point in the array is the depth point

        // Shift the plane and box to 'uptoPoint' if it is a broken-out section view and uptoPoint is specified
        var offset = 0 * meter;
        if (isCropView) // for crop view, there is no depth, always from minCorner to maxCorner
        {
            uptoPoint = toWorld(coordinateSystem, boxResult.minCorner);
        }

        if (uptoPoint == undefined)
        {
            uptoPoint = toWorld(coordinateSystem, boxResult.maxCorner);
        }
        if (uptoPoint != undefined)
        {
            const dir = uptoPoint - defaultOrigin;
            offset = dot(dir, sketchPlane.normal);
        }
        if (offsetDistancesArray != undefined && size(offsetDistancesArray) > brokenOutIndex && offsetDistancesArray[brokenOutIndex] != undefined && !isCropView)
        {
            offset += offsetDistancesArray[brokenOutIndex];
        }
        var origin = defaultOrigin + offset * sketchPlane.normal;
        const offsetPlane = plane(origin, sketchPlane.normal, sketchPlane.x);

        const isClosedSpline = tolerantEquals(jogPointsForBrokenOut[0], jogPointsForBrokenOut[numberOfJogPoints - 1]);
        // if it is not closed, we add one to close it
        if (!isClosedSpline)
        {
            jogPointsForBrokenOut = concatenateArrays([jogPointsForBrokenOut, makeArray(1)]);
            jogPointsForBrokenOut[numberOfJogPoints] = jogPointsForBrokenOut[0];
            numberOfJogPoints = numberOfJogPoints + 1;
        }
        var projectedPoints = makeArray(numberOfJogPoints);
        for (var i = 0; i < numberOfJogPoints; i = i + 1)
        {
            projectedPoints[i] = worldToPlane(offsetPlane, jogPointsForBrokenOut[i]);
        }

        const sketchId = id + ("sketch" ~ brokenOutIndex);
        sketchSplineSection(context, sketchId, projectedPoints, offsetPlane);

        const extrudeId = id + ("extrude" ~ brokenOutIndex);
        const sketchRegionQuery = qCreatedBy(sketchId, EntityType.FACE);
        extrudeCut(context, extrudeId, targetCurrentSection, sketchPlane.normal, sketchRegionQuery, undefined, isCropView, versionOperationUse);
        opDeleteBodies(context, id + ("deleteSketch" ~ brokenOutIndex), { "entities" : qCreatedBy(sketchId, EntityType.BODY) });
    }
}

function extrudeCut(context is Context, id is Id, target is Query, direction, sketchRegionQuery is Query, depth,
                    isIntersect is boolean, versionOperationUse is boolean)
{
    var noMerge = isAtVersionOrLater(context, FeatureScriptVersionNumber.V620_DONT_MERGE_SECTION_FACE);
    if (depth != undefined && depth < 2 * TOLERANCE.booleanDefaultTolerance * meter)
    {
        depth = (2 * TOLERANCE.booleanDefaultTolerance) * meter;
    }
    // BEL-110102 in rel-1.99 use of operations change was released without versioning.
    // It causes missing parts in section views held back to versions earlier than V1017_SUBTRACT_COMPLEMENT
    // In order to preserve drawings created between rel-1.99 and release of this fix, versionOperationUse is controlled
    // by drawing view version.
    if (!versionOperationUse  || isAtVersionOrLater(context, FeatureScriptVersionNumber.V1017_SUBTRACT_COMPLEMENT))
    {
        //Evaluate before extrude to avoid qEverything or such picking up extruded body
        const evaluatedTarget = qUnion(evaluateQuery(context, target));
        opExtrude(context, id, {
                "entities" : sketchRegionQuery,
                "direction" : direction,
                "endBound" : depth == undefined ? BoundingType.THROUGH_ALL : BoundingType.BLIND,
                "endDepth" : depth
        });

        opBoolean(context, id + "boolean", {
                "tools" : qBodyType(qCreatedBy(id, EntityType.BODY), BodyType.SOLID),
                "operationType" : isIntersect ? BooleanOperationType.SUBTRACT_COMPLEMENT : BooleanOperationType.SUBTRACTION,
                "targets" : evaluatedTarget,
                "allowSheets" : true,
                "eraseImprintedEdges" : noMerge ? false : true
        });
    }
    else
    {
        const extrudeDefinition = {"bodyType" : ExtendedToolBodyType.SOLID,
            "operationType" : isIntersect ? NewBodyOperationType.INTERSECT : NewBodyOperationType.REMOVE,
            "entities" : sketchRegionQuery,
            "endBound" : depth == undefined ? BoundingType.THROUGH_ALL : BoundingType.BLIND,
            "depth" : depth,
            "defaultScope" : false,
            "eraseImprintedEdges" : noMerge ? false : true,
            "allowSheets" : true,
            "booleanScope" : target};

        extrude(context, id, extrudeDefinition);

    }
}

function checkJogDirection(pointsInPlane is array)
{
    var increasingYCount = 0;
    var decreasingYCount = 0;
    var length = size(pointsInPlane);
    for (var i = 0; i < length - 1; i = i + 1)
    {
        const deltaY = pointsInPlane[i + 1][1] - pointsInPlane[i][1];
        if (deltaY > TOLERANCE.zeroLength * meter)
        {
            increasingYCount = increasingYCount + 1;
        }
        if (deltaY < -TOLERANCE.zeroLength * meter)
        {
            decreasingYCount = decreasingYCount + 1;
        }
    }
    if (increasingYCount == 0 && decreasingYCount == 0)
    {
        throw regenError(ErrorStringEnum.SELF_INTERSECTING_CURVE_SELECTED);
    }
    if (increasingYCount > 0 && decreasingYCount > 0)
    {
        throw regenError(ErrorStringEnum.SELF_INTERSECTING_CURVE_SELECTED);
    }
}

function createJogPolygonForOffsetCut(points is array, boundingBox is Box3d, sketchPlane is Plane, offsetDistance is ValueWithUnits) returns array
{
    var polygonVertices = concatenateArrays([makeArray(1), points, makeArray(2)]);

    const pointCount = size(points);
    polygonVertices[0] = vector(points[pointCount - 1][0] + offsetDistance, points[0][1]);
    polygonVertices[pointCount + 1] = vector(points[pointCount - 1][0] + offsetDistance, points[pointCount - 1][1]);
    polygonVertices[pointCount + 2] = polygonVertices[0];

    return polygonVertices;
}

function createJogPolygon(points is array, boundingBox is Box3d, sketchPlane is Plane) returns array
{
    var polygonVertices = concatenateArrays([makeArray(1), points, makeArray(2)]);

    const pointCount = size(points);
    const boxRadius = norm(boundingBox.maxCorner) / 2;
    const boxCenterInPlane = vector(boundingBox.maxCorner[0] / 2, boundingBox.maxCorner[1] / 2);
    const alignedDistanceToJogStart = abs(boxCenterInPlane[0] - points[0][0]);
    const alignedDistanceToJogEnd = abs(boxCenterInPlane[0] - points[pointCount - 1][0]);
    polygonVertices[0] = vector(0 * meter, points[0][1]);
    polygonVertices[pointCount + 1] = vector(0 * meter, points[pointCount - 1][1]);
    polygonVertices[pointCount + 2] = polygonVertices[0];

    return polygonVertices;
}

function createJogPolygonForPartialSection(context is Context, points is array, boundingBox is Box3d, sketchPlane is Plane) returns array
{
    if (!isAtVersionOrLater(context, FeatureScriptVersionNumber.V1871_PARTIAL_SECTION_CUT_TOOL_CORRECTION))
    {
        var polygonVertices = concatenateArrays([points, makeArray(7)]);

        const pointCount = size(points);
        const boxRadius = norm(boundingBox.maxCorner) / 2;
        const boxCenterInPlane = vector(boundingBox.maxCorner[0] / 2, boundingBox.maxCorner[1] / 2);
        const alignedDistanceToJogStart = abs(boxCenterInPlane[0] - points[0][0]);
        const alignedDistanceToJogEnd = abs(boxCenterInPlane[0] - points[pointCount - 1][0]);
        const flipY = points[pointCount - 1][1] < points[0][1];

        polygonVertices[pointCount] = vector(boundingBox.maxCorner[0], points[pointCount - 1][1]);
        polygonVertices[pointCount + 1] = vector(polygonVertices[pointCount][0], flipY ? boundingBox.minCorner[1] : boundingBox.maxCorner[1]);
        polygonVertices[pointCount + 2] = vector(boundingBox.minCorner[0], polygonVertices[pointCount + 1][1]);
        polygonVertices[pointCount + 3] = vector(polygonVertices[pointCount + 2][0], flipY ? boundingBox.maxCorner[1] : boundingBox.minCorner[1]);
        polygonVertices[pointCount + 4] = vector(boundingBox.maxCorner[0], polygonVertices[pointCount + 3][1]);
        polygonVertices[pointCount + 5] = vector(polygonVertices[pointCount + 4][0], points[0][1]);
        polygonVertices[pointCount + 6] = polygonVertices[0];

        return polygonVertices;
    }
    // Avoid self intersection in polygon when start or end point is outside bounding box. Also, the order
    // of vertices is consistent with that in createJogPolygon. Needed for associative data query resolution.
    var polygonVertices = concatenateArrays([makeArray(1), points]);
    var pointCount = size(points);
    const flipY = points[pointCount - 1][1] < points[0][1];
    if ((flipY && points[pointCount - 1][1] <= boundingBox.minCorner[1]) || (!flipY && points[pointCount - 1][1] >= boundingBox.maxCorner[1]))
    {
        polygonVertices = append(polygonVertices, vector(boundingBox.minCorner[0], points[pointCount - 1][1]));
    }
    else
    {
        polygonVertices = append(polygonVertices, vector(boundingBox.maxCorner[0], points[pointCount - 1][1]));
        polygonVertices = append(polygonVertices, vector(polygonVertices[pointCount + 1][0], flipY ? boundingBox.minCorner[1] : boundingBox.maxCorner[1]));
        polygonVertices = append(polygonVertices, vector(boundingBox.minCorner[0], polygonVertices[pointCount + 2][1]));
    }
    polygonVertices = append(polygonVertices, vector(boundingBox.minCorner[0], flipY ? boundingBox.maxCorner[1] : boundingBox.minCorner[1]));
    pointCount = size(polygonVertices);
    if ((flipY && points[0][1] >= boundingBox.maxCorner[1]) || (!flipY && points[0][1] <= boundingBox.minCorner[1]))
    {
        polygonVertices[pointCount - 1] = vector(polygonVertices[pointCount - 1][0], points[0][1]);
    }
    else
    {
        polygonVertices = append(polygonVertices, vector(boundingBox.maxCorner[0], polygonVertices[pointCount - 1][1]));
        polygonVertices = append(polygonVertices, vector(polygonVertices[pointCount][0], points[0][1]));
    }
    polygonVertices[0] = polygonVertices[size(polygonVertices) - 1];
    return polygonVertices;
}

function sketchPolyline(context is Context, sketchId is Id, points is array, sketchPlane is Plane)
{
    const numberOfPoints = size(points);
    const sketch = newSketchOnPlane(context, sketchId, { "sketchPlane" : sketchPlane });

    for (var i = 0; i < numberOfPoints - 1; i = i + 1)
    {
        skLineSegment(sketch, "line_" ~ i, { "start" : points[i], "end" : points[i + 1] });
    }
    skSolve(sketch);
}

function sketchSplineSection(context is Context, sketchId is Id, points is array, sketchPlane is Plane)
{
    const sketch = newSketchOnPlane(context, sketchId, { "sketchPlane" : sketchPlane });
    skFitSpline(sketch, "spline", {
                "points" : points
            });
    skSolve(sketch);
}

function createJogPolygonForAlignedSection(points is array, boundingBox is Box3d, sketchPlane is Plane) returns array
{
    const pointCount = size(points);
    if (pointCount < 3)
    {
        return [];
    }
    var polygonVertices = concatenateArrays([points, makeArray(5)]);
    const distanceXToBoxMinCorner = abs(points[pointCount-1][0] - boundingBox.minCorner[0]);
    const distanceXToBoxMaxCorner = abs(points[pointCount-1][0] - boundingBox.maxCorner[0]);
    const flipX = distanceXToBoxMinCorner > distanceXToBoxMaxCorner;

    const distanceYToBoxMinCorner = abs(points[0][1] - boundingBox.minCorner[1]);
    const distanceYToBoxMaxCorner = abs(points[0][1] - boundingBox.maxCorner[1]);
    const flipY = distanceYToBoxMinCorner < distanceYToBoxMaxCorner;

    polygonVertices[pointCount] = vector(flipX ? boundingBox.maxCorner[0] : boundingBox.minCorner[0], points[pointCount - 1][1]);
    polygonVertices[pointCount + 1] = vector(polygonVertices[pointCount][0], flipY ? boundingBox.maxCorner[1] : boundingBox.minCorner[1]);
    polygonVertices[pointCount + 2] = vector(boundingBox.minCorner[0], polygonVertices[pointCount + 1][1]);
    polygonVertices[pointCount + 3] = vector(polygonVertices[pointCount + 2][0], points[0][1]);
    polygonVertices[pointCount + 4] = polygonVertices[0];

    return polygonVertices;
}

function createJogPolygonForSourceParts(points is array, boundingBox is Box3d) returns array
{
    var polygonVertices = makeArray(7);
    const distanceYToBoxMinCorner = abs(points[0][1] - boundingBox.minCorner[1]);
    const distanceYToBoxMaxCorner = abs(points[0][1] - boundingBox.maxCorner[1]);
    const flipY = distanceYToBoxMinCorner < distanceYToBoxMaxCorner;

    polygonVertices[0] = points[0];
    polygonVertices[1] = points[1];
    polygonVertices[2] = vector(boundingBox.maxCorner[0], points[1][1]);
    polygonVertices[3] = vector(boundingBox.maxCorner[0], flipY ? boundingBox.maxCorner[1] : boundingBox.minCorner[1]);
    polygonVertices[4] = vector(boundingBox.minCorner[0], polygonVertices[3][1]);
    polygonVertices[5] = vector(boundingBox.minCorner[0], flipY ? boundingBox.minCorner[1] : boundingBox.maxCorner[1]);
    polygonVertices[6] = polygonVertices[0];

    return polygonVertices;
}

function createJogPolygonForRotatedParts(points is array, boundingBox is Box3d) returns array
{
    var polygonVertices = makeArray(7);
    const distanceYToBoxMinCorner = abs(points[0][1] - boundingBox.minCorner[1]);
    const distanceYToBoxMaxCorner = abs(points[0][1] - boundingBox.maxCorner[1]);
    const flipY = distanceYToBoxMinCorner < distanceYToBoxMaxCorner;

    polygonVertices[0] = vector(points[1][0], flipY ? boundingBox.maxCorner[1] : boundingBox.minCorner[1]);
    polygonVertices[1] = points[1];
    polygonVertices[2] = vector(boundingBox.maxCorner[0], points[1][1]);
    polygonVertices[3] = vector(boundingBox.maxCorner[0], flipY ? boundingBox.minCorner[1] : boundingBox.maxCorner[1]);
    polygonVertices[4] = vector(boundingBox.minCorner[0], polygonVertices[3][1]);
    polygonVertices[5] = vector(boundingBox.minCorner[0], flipY ? boundingBox.maxCorner[1] : boundingBox.minCorner[1]);
    polygonVertices[6] = polygonVertices[0];

    return polygonVertices;
}

function sketchAndExtrudeCut(context is Context, id is Id, target is Query, polygon is array, offsetPlane is Plane,
                             sketchPlane is Plane, depth, versionOperationUse is boolean, isIntersect is boolean)
{
    const sketchId = id + "sketch";
    const extrudeId = id + "extrude";
    const deleteId = id + "deleteSketch";
    sketchPolyline(context, sketchId, polygon, offsetPlane);
    const sketchRegionQuery = qCreatedBy(sketchId, EntityType.FACE);
    extrudeCut(context, extrudeId, target, sketchPlane.normal, sketchRegionQuery, depth, isIntersect, versionOperationUse);
    opDeleteBodies(context, deleteId, { "entities" : qCreatedBy(sketchId, EntityType.BODY) });
}

// returned query resolves to section faces both in original and rotated parts
function alignedSectionRotateAndCut(context is Context, id is Id, definition is map, partIds is array) returns Query
{
    // define rotation axis
    const jogPoints = definition.jogPoints[0];
    const sketchPlane = definition.sketchPlane;
    const numberOfPoints = size(jogPoints);
    var projected3DPoints = makeArray(numberOfPoints);
    for (var i = 0; i < numberOfPoints; i = i + 1)
    {
        projected3DPoints[i] = project(sketchPlane, jogPoints[i]);
    }
    const normalizedStartLine = normalize(projected3DPoints[0] - projected3DPoints[1]);
    const normalizedEndLine = normalize(projected3DPoints[numberOfPoints-1] - projected3DPoints[numberOfPoints-2]);
    const rotationAngle = PI * radian - angleBetween(normalizedStartLine, normalizedEndLine);
    var rotationDirection = cross(normalizedStartLine, normalizedEndLine);
    // if the angle between two lines is 180 degree, set rotation direction as sketch plane normal
    if (squaredNorm(rotationDirection) < TOLERANCE.zeroLength)
    {
        rotationDirection = sketchPlane.normal;
    }
    const rotationAxis = line(jogPoints[1], rotationDirection);
     // tracking faces generated from extrude cut and divide them into two groups
    const facesByExtrudeCut = qCreatedBy(id + "extrude", EntityType.FACE);
    const facesAlignedWithViewPlane = qParallelPlanes(facesByExtrudeCut, sketchPlane.x, true);
    const facesAlignedWithRevolvedPlane = rotationAngle < TOLERANCE.zeroAngle * radian
                                      ? facesAlignedWithViewPlane
                                      : qSubtraction(facesByExtrudeCut, facesAlignedWithViewPlane);
    // for lines with 180 degree, tracking is needed because the section faces aligned with view plane could be split in the following
    // sketch and extrude cut step, which could modify the original face query
    const trackFacesAlignedWithViewPlane = qUnion([facesAlignedWithViewPlane, startTracking(context, facesAlignedWithViewPlane)]);
    const trackFacesAlignedWithRevolvedPlane = startTracking(context, facesAlignedWithRevolvedPlane);
    // make rotated copies
    opPattern(context, id + "pattern", {
            "entities" : definition.target,
            "transforms" : [rotationAround(rotationAxis, rotationAngle)],
            "instanceNames" : ['patternInstancesForPartStudio']
    });
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1670_MODIFY_COMPOSITE_BEFORE_EXTRUDE_CUT))
    {
        addToComposites(context, id, partIds);
    }
    var sourceParts = qSubtraction(definition.target, qCreatedBy(id + "pattern", EntityType.BODY));
    var bodyTypes = [BodyType.SOLID];
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1598_SHEET_BODY_ALIGNED_SECTION_VIEW_FIX))
    {
        bodyTypes = append(bodyTypes, BodyType.SHEET);
    }
    var rotatedParts = qBodyType(qCreatedBy(id + "pattern", EntityType.BODY), bodyTypes);
    const versionOperationUse = (definition.versionOperationUse == true);
    const coordinateSystem = planeToCSys(sketchPlane);
    var boxResult = evBox3d(context, {
        'topology' : qUnion([sourceParts, rotatedParts]),
        'cSys' : coordinateSystem,
        'tight' : false
    });
    boxResult = extendBox3d(boxResult, 0 * meter, BOX_TOLERANCE);
    // Shift the plane and box to the box's min corner
    var origin = toWorld(coordinateSystem, boxResult.minCorner);
    const offsetPlane = plane(origin, sketchPlane.normal, sketchPlane.x);
    boxResult.maxCorner = boxResult.maxCorner - boxResult.minCorner;
    boxResult.minCorner = vector(0, 0, 0) * meter;
    var projected2DPoints = makeArray(numberOfPoints);
    for (var i = 0; i < numberOfPoints; i = i + 1)
    {
         projected2DPoints[i] = worldToPlane(offsetPlane, jogPoints[i]);
    }

    var polygonForSourceParts = createJogPolygonForSourceParts(projected2DPoints, boxResult);
    var polygonForRotatedParts = createJogPolygonForRotatedParts(projected2DPoints, boxResult);

    sketchAndExtrudeCut(context, id + "sourceParts", sourceParts, polygonForSourceParts, offsetPlane,
                        sketchPlane, boxResult.maxCorner[2], versionOperationUse, false);

    sketchAndExtrudeCut(context, id + "rotatedParts", rotatedParts, polygonForRotatedParts, offsetPlane,
                        sketchPlane, boxResult.maxCorner[2], versionOperationUse, false);

    var sectionFacesParallelToViewPlane = qUnion([qOwnedByBody(trackFacesAlignedWithViewPlane, sourceParts), qOwnedByBody(trackFacesAlignedWithRevolvedPlane, rotatedParts)]);
    var sectionFacesPerpendicularToViewPlane = qUnion([qCreatedBy(id + "sourceParts", EntityType.FACE), qCreatedBy(id + "rotatedParts", EntityType.FACE)]);
    var sectionEdgesPerpendicularToViewPlane = qLoopEdges(sectionFacesPerpendicularToViewPlane);
    var sectionEdgesParallelToViewPlane = qLoopEdges(sectionFacesParallelToViewPlane);
    var planePerpendicularToViewPlane = plane(jogPoints[1], cross(sketchPlane.normal, sketchPlane.x));
    var touchingEdges = qUnion([qCoincidesWithPlane(sectionEdgesPerpendicularToViewPlane, planePerpendicularToViewPlane),
                                qCoincidesWithPlane(sectionEdgesParallelToViewPlane, planePerpendicularToViewPlane)]);
    if (!isQueryEmpty(context, touchingEdges))
    {
        setAttribute(context, {
                "entities" : touchingEdges,
                "attribute" : {
                    "name" : id ~ "sectionTouchingEdges"
                }
        });
    }
    return sectionFacesParallelToViewPlane;
}

function addToComposites(context is Context, id is Id, partIds is array)
{
    for (var partId in partIds)
    {
        const toAddParts = qHasAttribute(id ~ partId);
        if (!isQueryEmpty(context, toAddParts))
        {
            const existingComposites = evaluateQuery(context, qHasAttribute("composite" ~ id ~ partId));
            for (var i = 0; i < size(existingComposites); i += 1)
            {
                if (!isQueryEmpty(context, qSubtraction(toAddParts, qContainedInCompositeParts(existingComposites[i]))))
                {
                    opModifyCompositePart(context, id + unstableIdComponent(partId ~ i), {
                            "composite" : existingComposites[i],
                            "toAdd" : qSubtraction(toAddParts, qContainedInCompositeParts(existingComposites[i]))
                    });
                }
            }
        }
    }
}

