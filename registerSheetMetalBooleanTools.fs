FeatureScript ✨; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

// Imports used internally
import(path : "onshape/std/containers.fs", version : "✨");
import(path : "onshape/std/evaluate.fs", version : "✨");
import(path : "onshape/std/feature.fs", version : "✨");
import(path : "onshape/std/holepropagationtype.gen.fs", version : "✨");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "✨");
import(path : "onshape/std/sheetMetalUtils.fs", version : "✨");
import(path : "onshape/std/transform.fs", version : "✨");

/**
 * @internal
 */
function isInstersectingClashType(clashType is ClashType) returns boolean
{
    return (clashType == ClashType.INTERFERE ||
            clashType == ClashType.EXISTS ||
            clashType == ClashType.ABUT_TOOL_IN_TARGET ||
            clashType == ClashType.TARGET_IN_TOOL ||
            clashType == ClashType.TOOL_IN_TARGET);
}

/**
 * @internal
 * Create a cache that can find sheet metal master body entity corresponding to `target` input.
 */
function makeDefinitionEntityCache(context is Context) returns function
{
    return memoizeFunction(function(target is Query)
    {
        const definitionEntities = getSMDefinitionEntities(context, target);
        if (size(definitionEntities) != 1)
        {
            if (definitionEntities != [])
            {
                throw "Unexpected number of definition entities";
            }
            else
            {
                //The faces created by holes no longer have a corresponding definition entity
                return qNothing();
            }
        }
        else
        {
            return definitionEntities[0];
        }
    });
}

/**
 * @internal
 * Create a cache that computes whether the passed in entity is planar or not.
 */
function makeIsEntityPlanarCache(context is Context) returns function
{
    return memoizeFunction(function(entity is Query)
    {
        return !isQueryEmpty(context, qGeometry(entity, GeometryType.PLANE));
    });
}

/**
 * @internal
 *
 * Register various tool bodies to be booleaned with sheet metal bodies.
 *
 * Internally, performs an [evCollision] between the tools and targets, associates the
 * tools with the underlying sheet metal master body's walls as [SMAttribute]s and if requested, calls
 * [updateSheetMetalGeometry] which uses this information to perform the booleans on the
 * walls' patch bodies.
 *
 * @param definition {{
 *      @field subtractiveTools {Query}:
 *              The cutting tool bodies you want subtracted from the sheet metal target bodies.
 *      @field targets {Query}:
 *              The sheet metal target bodies you want to cut the tool bodies from.
 *      @field doUpdateSMGeometry {boolean} :
 *              true : Call [updateSheetMetalGeometry] so that the actual boolean is performed
 *              false: The caller might want to call [updateSheetMetalGeometry] themselves
 * }}
 * @returns : Map of master body's walls to the correspodning tool bodies registered to be booleaned
 */
export const registerSheetMetalBooleanTools = function(context is Context, id is Id, definition is map)
    {
        var collisions = evCollision(context, {
                "tools" : definition.subtractiveTools,
                "targets" : definition.targets
            });

        var unregisteredToolsSet = {};
        const targetToDefinitionEntity = makeDefinitionEntityCache(context);
        const definitionEntityToWallIsPlanarCache = makeIsEntityPlanarCache(context);
        var toolToPlanarWalls = {};
        for (var collision in collisions)
        {
            if (unregisteredToolsSet[collision.toolBody] != undefined)
            {
                continue;
            }
            if (isInstersectingClashType(collision['type']))
            {
                const definitionEntity = targetToDefinitionEntity(collision.target);
                if (definitionEntity == qNothing())
                {
                    continue;
                }
                const wallIsPlanar = definitionEntityToWallIsPlanarCache(definitionEntity);
                if (wallIsPlanar)
                {
                    if (toolToPlanarWalls[collision.toolBody] == undefined)
                    {
                        toolToPlanarWalls[collision.toolBody] = {};
                    }
                    toolToPlanarWalls[collision.toolBody][definitionEntity] = true;
                }
                else
                {
                    // Do not allow users to register boolean tools that interact with side walls, rolled walls, rips, joints, or corners
                    toolToPlanarWalls[collision.toolBody] = undefined;
                    unregisteredToolsSet[collision.toolBody] = true;
                }
            }
        }

        var wallToCuttingToolBodies = {};
        for (var tool, planarWalls in toolToPlanarWalls)
        {
            for (var planarWall in keys(planarWalls))
            {
                wallToCuttingToolBodies = insertIntoMapOfArrays(wallToCuttingToolBodies, planarWall, tool);
            }
        }

        var wallToCuttingToolBodyIdSet = {};
        var activeTools = [];
        var iPattern = 0;
        for (var definitionWallFace, cuttingToolBodies in wallToCuttingToolBodies)
        {
            for (var cuttingToolBody in cuttingToolBodies)
            {
                if (wallToCuttingToolBodyIdSet[definitionWallFace] == undefined)
                {
                    wallToCuttingToolBodyIdSet[definitionWallFace] = {};
                }
                if (wallToCuttingToolBodyIdSet[definitionWallFace][cuttingToolBody.transientId] == undefined)
                {
                    if (isIn(cuttingToolBody, activeTools))
                    {
                        iPattern = iPattern + 1;
                        var opPatternId = id + "copyTool" + iPattern;
                        opPattern(context, opPatternId, {
                            "entities" : cuttingToolBody,
                            "transforms" : [identityTransform()],
                            "instanceNames" : ["instance" ~ iPattern],
                            "holePropagationType" : HolePropagationType.PROPAGATE_SAME_HOLE
                        });
                        const patternedBodies = evaluateQuery(context, qCreatedBy(opPatternId, EntityType.BODY));
                        if (size(patternedBodies) != 1)
                        {
                            throw "Unexpected number(" ~ size(patternedBodies) ~ ") of patterned bodies";
                        }
                        wallToCuttingToolBodyIdSet[definitionWallFace][cuttingToolBody.transientId] = patternedBodies[0].transientId;
                    }
                    else
                    {
                        activeTools = append(activeTools, cuttingToolBody);
                        wallToCuttingToolBodyIdSet[definitionWallFace][cuttingToolBody.transientId] = cuttingToolBody.transientId;
                    }
                }
            }
        }

        var updatedSMEntities = [];
        var robustWallToCuttingToolBodyIdSet = {};
        for (var wall, cuttingToolBodyIdSet in wallToCuttingToolBodyIdSet)
        {
            const cuttingToolBodyIds = values(cuttingToolBodyIdSet);
            const wallAttribute = getWallAttribute(context, wall);
            if (wallAttribute == undefined)
            {
                throw "Unexpected as planes have been filtered";
            }
            var alteredWallAttribute = wallAttribute;
            if (alteredWallAttribute.cuttingToolBodyIds == undefined)
            {
                alteredWallAttribute.cuttingToolBodyIds = cuttingToolBodyIds;
            }
            else
            {
                alteredWallAttribute.cuttingToolBodyIds = concatenateArrays([alteredWallAttribute.cuttingToolBodyIds, cuttingToolBodyIds]);
            }
            replaceSMAttribute(context, wallAttribute, alteredWallAttribute);
            updatedSMEntities = append(updatedSMEntities, wall);
            robustWallToCuttingToolBodyIdSet[makeRobustQuery(context, wall)] = cuttingToolBodyIdSet;
        }

        if (definition.doUpdateSMGeometry && updatedSMEntities != [])
        {
            updateSheetMetalGeometry(context, id, {
                        "entities" : qUnion(updatedSMEntities)
                    });
        }
        return robustWallToCuttingToolBodyIdSet;
    };

