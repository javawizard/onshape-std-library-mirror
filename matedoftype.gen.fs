FeatureScript 1077; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

/* Automatically generated file -- DO NOT EDIT */

/**
 * Specifies a single degree of freedom of a mate.
 *
 * @value Tx : Translation along the X axis.
 * @value Ty : Translation along the Y axis.
 * @value Tz : Translation along the Z axis.
 * @value Rz : Rotation around the Z axis.
 * @value Rz : Rotation around the Z axis.
 * @value Ryp : Rotation around the transformed Y axis from previous transform sequence.
 * @value Rzp : Rotation around the transformed z axis from previous transform sequence .
 */
export enum MateDOFType
{
    annotation {"Name" : "Translational X"}
    Tx,
    annotation {"Name" : "Translational Y"}
    Ty,
    annotation {"Name" : "Translational Z"}
    Tz,
    annotation {"Name" : "Rotational Z"}
    Rz,
    annotation {"Name" : "Rotational Y'"}
    Ryp,
    annotation {"Name" : "Rotational Z'"}
    Rzp
}


