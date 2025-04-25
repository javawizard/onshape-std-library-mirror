FeatureScript 2641; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports used in interface
export import(path : "onshape/std/blendcontroltype.gen.fs", version : "2641.0");
export import(path : "onshape/std/chamfermethod.gen.fs", version : "2641.0");
export import(path : "onshape/std/chamfertype.gen.fs", version : "2641.0");
export import(path : "onshape/std/edgeBlendCommon.fs", version : "2641.0");
export import(path : "onshape/std/filletcrosssection.gen.fs", version : "2641.0");
export import(path : "onshape/std/manipulator.fs", version : "2641.0");
export import(path : "onshape/std/query.fs", version : "2641.0");

import(path : "onshape/std/containers.fs", version : "2641.0");
import(path : "onshape/std/edgeconvexitytype.gen.fs", version : "2641.0");
import(path : "onshape/std/evaluate.fs", version : "2641.0");
import(path : "onshape/std/feature.fs", version : "2641.0");
import(path : "onshape/std/sheetMetalAttribute.fs", version : "2641.0");
import(path : "onshape/std/sheetMetalInFlat.fs", version : "2641.0");
import(path : "onshape/std/sheetMetalUtils.fs", version : "2641.0");
import(path : "onshape/std/valueBounds.fs", version : "2641.0");
import(path : "onshape/std/vector.fs", version : "2641.0");

/**
 * Specifies type of edge blend
 */
export enum EdgeBlendType
{
    annotation {"Name" : "Fillet"}
    FILLET,
    annotation {"Name" : "Chamfer"}
    CHAMFER
}

const FILLET_WIDTH = "filletWidth";

/**
*   Sheet metal specific feature combining functionality of edge fillet and chamfer.
*   It calls [SMEdgeBlendImpl] to apply fillets or chamfers in flat and then change the definition surface accordingly
*   As a result of this change rips corner/bend reliefs are also "baked" into the definition surface and flexibility
*   of sheet metal model is lost. For this reason we recommend that this feature is used after sheet metal flanges, joints
*   and reliefs are finalized.
*/
annotation { "Feature Type Name" : "Corner break",
             "Manipulator Change Function" : "smEdgeBlendManipulatorChange",
             "Filter Selector" : "allparts" }
export const sheetMetalCornerBreak = defineSheetMetalFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Type", "UIHint" : [UIHint.HORIZONTAL_ENUM, UIHint.REMEMBER_PREVIOUS_VALUE]}
        definition.blendType is EdgeBlendType;

        annotation { "Name" : "Entities to fillet or chamfer",
                     "Filter" : (GeometryType.LINE || EntityType.VERTEX) &&
                        SheetMetalDefinitionEntityType.VERTEX && AllowFlattenedGeometry.YES && ModifiableEntityOnly.YES,
                     "AdditionalBoxSelectFilter" : EntityType.EDGE }
        definition.entities is Query;

        if (definition.blendType == EdgeBlendType.CHAMFER)
        {
            chamferOptions(definition);
        }
        else
        {
            filletOptions(definition);
        }
    }
    {
        // this is not necessary but helps with correct error reporting in feature pattern
        checkNotInFeaturePattern(context, definition.entities, ErrorStringEnum.SHEET_METAL_NO_FEATURE_PATTERN);

        if (isQueryEmpty(context, definition.entities))
        {
            throw regenError(ErrorStringEnum.SHEET_METAL_CORNER_BREAK_SELECT_ENTITIES, ["entities"]);
        }
        if (definition.blendType == EdgeBlendType.FILLET)
        {
            if (definition.blendControlType == BlendControlType.WIDTH)
            {
                definition.width = definition[FILLET_WIDTH];
            }
            addFilletManipulator(context, id, definition);
        }
        else
            definition.crossSection = FilletCrossSection.CHAMFER;

        SMEdgeBlendImpl(context, id, definition);
    },
    {
        crossSection : FilletCrossSection.CIRCULAR,
        allowEdgeOverflow : false,
        keepEdges : qNothing(),
        blendControlType : BlendControlType.RADIUS,
        isAsymmetric : false,
        oppositeDirection : false,
        tangentPropagation : false,
        chamferMethod : ChamferMethod.FACE_OFFSET,
        directionOverrides : qNothing()
    });

predicate chamferOptions(definition is map)
{
    chamferCommonOptions(definition);

    if (definition.chamferType == ChamferType.OFFSET_ANGLE ||
        definition.chamferType == ChamferType.TWO_OFFSETS)
    {
        annotation {"Name" : "Direction overrides",
                 "Filter" : (GeometryType.LINE || EntityType.VERTEX) && SheetMetalDefinitionEntityType.VERTEX && AllowFlattenedGeometry.YES }
        definition.directionOverrides is Query;
    }
}

predicate filletOptions(definition is map)
{
    edgeFilletCommonOptions(definition, FILLET_WIDTH);
    asymmetricFilletOption(definition);
    if (definition.crossSection != FilletCrossSection.CURVATURE)
    {
        annotation { "Name" : "Allow edge overflow", "Default" : false }
        definition.allowEdgeOverflow is boolean;

        if (definition.allowEdgeOverflow)
        {
            annotation { "Group Name" : "Edge overflow", "Driving Parameter" : "allowEdgeOverflow" }
            {
                annotation { "Name" : "Edges to keep",
                            "Filter" : (EntityType.EDGE && EdgeTopology.TWO_SIDED) && ActiveSheetMetal.YES && AllowFlattenedGeometry.YES }
                definition.keepEdges is Query;
            }
        }
    }
}

/*
 * Create a linear manipulator for the fillet
 */
function addFilletManipulator(context is Context, id is Id, definition is map)
{
    // get last last edge (or arbitrary edge of the last face) from the qlv
    const operativeEntity = try(findManipulationEntity(context, definition));
    if (operativeEntity != undefined)
    {
        addFilletControlManipulator(context, id, definition, operativeEntity);
    }
}

/*
 * Start with the final element in the qlv.
 * If it is an edge, return it.
 * If it is a face and it has edges, return one arbitrarily.
 * In case of active sheet metal we allow edge and vertex selections in 3d and flat,
 * Some additional processing to find correct edge in 3d to use for manipulator placement
 * Continue through the list in reverse order until an edge can be found.
 */
function findManipulationEntity(context is Context, definition is map) returns Query
{
    const resolvedEntities = evaluateQuery(context, definition.entities);
    const nResolved = size(resolvedEntities);

    var candidatesInFlat = [];
    for (var i = nResolved - 1; i >= 0; i -= 1)
    {
        const entity = resolvedEntities[i];

        if (!isQueryEmpty(context, entity->qSheetMetalFlatFilter(SMFlatType.YES)))
        {
            // keep looking in case we find a better 3d candidate
            candidatesInFlat = append(candidatesInFlat, entity);
        }
        else if (!isQueryEmpty(context, entity->qEntityFilter(EntityType.EDGE)))
        {
            return entity;
        }
        else
        {
            const edge = findEdgeByVertex(context, entity);
            if (edge != undefined)
            {
                return edge;
            }
        }
    }

    for (var entity in candidatesInFlat)
    {
        const requestType = isQueryEmpty(context, entity->qEntityFilter(EntityType.EDGE)) ? EntityType.VERTEX : EntityType.EDGE;
        const correspondingIn3dQ = getSMCorrespondingInPart(context, entity, requestType);
        const entIn3d = findExactCorresponding(context, correspondingIn3dQ, entity);
        if (entIn3d != undefined)
        {
            if (requestType == EntityType.EDGE)
            {
                return entIn3d;
            }
            const edge = findEdgeByVertex(context, entIn3d);
            if (edge != undefined)
            {
                return edge;
            }
        }
    }

    throw {};
}

function findExactCorresponding(context is Context, associatedIn3d is Query, entityInFlat is Query)
{
    for (var entity in evaluateQuery(context, associatedIn3d))
    {
        if (isQueryEmpty(context, entity->qCorrespondingInFlat()->qSubtraction(entityInFlat)))
            return entity;
    }
}

function findEdgeByVertex(context is Context, vertexQ is Query)
{
    const attributes = getSMAssociationAttributes(context, vertexQ);
    var edgeQ = vertexQ->qAdjacent(AdjacencyType.VERTEX, EntityType.EDGE)->qGeometry(GeometryType.LINE);
    if (attributes != [])
    {
        edgeQ = edgeQ->qAttributeFilter(attributes[0]);
    }
    const edges = evaluateQuery(context, edgeQ);
    if (size(edges) == 1)
    {
        return edges[0];
    }
    // look for edge whose adjacent faces are not walls
    for (var edge in edges)
    {
        const faceDefEnts = getSMDefinitionEntities(context, edge->qAdjacent(AdjacencyType.EDGE, EntityType.FACE));
        if (isQueryEmpty(context, faceDefEnts->qUnion()->qEntityFilter(EntityType.FACE)))
        {
            return edge;
        }
    }
}


/**
 * @internal
 * Manipulator change function for `sheetMetalCornerBreak`.
 */
export function smEdgeBlendManipulatorChange(context is Context, definition is map, newManipulators is map) returns map
{
    const operativeEntity = findManipulationEntity(context, definition);
    if (operativeEntity != undefined)
        return onFilletControlManipulatorChange(context, definition, newManipulators, operativeEntity, FILLET_WIDTH);
    else
        return definition;
}

