FeatureScript 1160; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * Enumeration of the types of items that can be selected inside a [Part Studio reference parameter](/FsDoc/imports.html#part-studio).
 * By default this parameter allows selecting all of the item types below. It can be filtered by specifying a union of
 * any number of PartStudioItemTypes (e.g. `"Filter" : PartStudioItemType.SOLID || PartStudioItemType.ENTIRE_PART_STUDIO`)
 * in the parameter's annotation.
 *
 * @value SOLID : A body with `BodyType.SOLID` (i.e. a part in the parts list)
 * @value SURFACE : A non-sketch body with `BodyType.SURFACE` (i.e. a surface in the parts list)
 * @value WIRE : A non-sketch body with `BodyType.WIRE` (i.e. a curve in the parts list)
 * @value MESH : An imported mesh (i.e. a mesh in the parts list)
 * @value SKETCH : An entire sketch feature (i.e. all `POINT`, `WIRE`, and `SURFACE` bodies created by the sketch)
 * @value FLATTENED_SHEET_METAL : A flattened sheet metal part (available from any Part Studio with sheet metal features)
 * @value CONSTRUCTION_PLANE : A plane created with Plane feature or with `opPlane`
 * @value ENTIRE_PART_STUDIO : The entire Part Studio. Setting this option allows clicking the top item
 *      (with the full Part Studio's tumbnail and name), which sets the `partQuery` to
 *      `qEverything(EntityType.BODY)`.
 */
export enum PartStudioItemType
{
    SOLID,
    SURFACE,
    WIRE,
    MESH,
    SKETCH,
    FLATTENED_SHEET_METAL,
    ENTIRE_PART_STUDIO,
    CONSTRUCTION_PLANE,
    COMPOSITE_PART
}


