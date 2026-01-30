FeatureScript 2878; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.
import(path : "onshape/std/containers.fs", version : "2878.0");
import(path : "onshape/std/math.fs", version : "2878.0");
import(path : "onshape/std/vector.fs", version : "2878.0");

/**
 * Remove as many knots as possible from a NURBS defined by points, knots and curveDegree.
 */
export function removeKnots(points is array, knots is array, curveDegree is number) returns map
{
    // We reverse the knots order to avoid adjusting indices after removal
    var indicesAndMults = reverse(knotsLastIndicesAndMultiplicities(knots));
    for (var i = 0; i < size(indicesAndMults); i += 1)
    {
        const removalResult = removeKnot(indicesAndMults[i].index, indicesAndMults[i].mult, points, knots, curveDegree);
        if (removalResult.nRemoved == 0)
        {
            continue;
        }
        points = removalResult.points;
        knots = removalResult.knots;
    }
    return { "points" : points, "knots" : knots };
}

// FS implementation of Algorithm 5.8 from The NURBS Book, Piegl, L., & Tiller, W. (1997) (doi:10.1007/978-3-642-59223-2)
function removeKnot(knotIndex is number, mult is number, points is array, knots is array, curveDegree is number) returns map
{
    // Extract all the variables
    const numPoints = size(points);
    const knot = knots[knotIndex];
    const numToRemove = mult - 1;
    const numKnots = size(knots);
    const order = curveDegree + 1;
    // First control point out
    const firstOut = floor((2 * knotIndex - mult - curveDegree) / 2);
    var first = knotIndex - curveDegree;
    var last = knotIndex - mult;
    var t = 0;
    for (t = 0; t < numToRemove; t += 1)
    {
        // Index difference between temp and points
        const off = first - 1;
        var temp = makeArray(last - off + 2);
        temp[0] = points[off];
        temp[last + 1 - off] = points[last + 1];
        var i = first;
        var j = last;
        var ii = 1;
        var jj = last - off;
        var remFlag = false;
        while (j - i > t)
        {
            // For each removal step, we compute the new control points
            const alphaI = (knot - knots[i]) / (knots[i + order + t] - knots[i]);
            const alphaJ = (knot - knots[j - t]) / (knots[j + order] - knots[j - t]);
            temp[ii] = (points[i] - (1 - alphaI) * temp[ii - 1]) / alphaI;
            temp[jj] = (points[j] - alphaJ * temp[jj + 1]) / (1 - alphaJ);
            i += 1;
            ii += 1;
            j -= 1;
            jj -= 1;
        }
        // If the control points meet in the middle, we can remove the knot
        if (j - i < t)
        {
            if (weightedPointsTolerantEquals(temp[ii - 1], temp[jj + 1]))
            {
                remFlag = true;
            }
        }
        else
        {
            const alphaI = (knot - knots[i]) / (knots[i + order + t] - knots[i]);
            const pt = alphaI * temp[ii + t + 1] + (1 - alphaI) * temp[ii - 1];

            if (weightedPointsTolerantEquals(points[i], pt))
            {
                remFlag = true;
            }
        }
        if (!remFlag)
        {
            break;
        }
        // Copy the temporary control points into the points array for the next removal step
        i = first;
        j = last;
        while (j - i > t)
        {
            points[i] = temp[i - off];
            points[j] = temp[j - off];
            i += 1;
            j -= 1;
        }
        first -= 1;
        last += 1;
    }
    if (t == 0)
    {
        return { "points" : points, "knots" : knots, "nRemoved" : 0 };
    }
    // shift the subsequent knots by the amount of removed knots
    for (var k = knotIndex + 1; k < numKnots; k += 1)
    {
        knots[k - t] = knots[k];
    }
    // similarly shift the control points
    var j = firstOut;
    var i = j;
    for (var k = 1; k < t; k += 1)
    {
        if (k % 2 == 1)
        {
            i += 1;
        }
        else
        {
            j -= 1;
        }
    }
    for (var k = i + 1; k < numPoints; k += 1)
    {
        points[j] = points[k];
        j += 1;
    }
    return { "points" : subArray(points, 0, numPoints - t), "knots" : subArray(knots, 0, numKnots - t), "nRemoved" : t };
}

// For each knot, returns the max index of this knot in the knot vector and its multiplicity
function knotsLastIndicesAndMultiplicities(knots is array) returns array
{
    var result = [];
    const firstKnot = knots[0];
    const lastKnot = knots[size(knots) - 1];
    var currentMult = 0;
    var currentKnot = firstKnot;
    for (var i = 0; i < size(knots); i += 1)
    {
        if (knots[i] == firstKnot)
        {
            continue;
        }
        if (knots[i] == currentKnot)
        {
            currentMult += 1;
            continue;
        }
        if (currentMult > 1)
        {
            result = append(result, { "index" : i - 1, "mult" : currentMult });
        }
        if (knots[i] == lastKnot)
        {
            break;
        }
        currentKnot = knots[i];
        currentMult = 1;
    }
    return result;
}

function weightedPointsTolerantEquals(wp1 is Vector, wp2 is Vector) returns boolean
{
    const vectorsAndWeights = separatePointsAndWeights([wp1, wp2]);
    return tolerantEquals(vectorsAndWeights.points[0], vectorsAndWeights.points[1]) && tolerantEquals(vectorsAndWeights.weights[0], vectorsAndWeights.weights[1]);
}

/**
 * Separates a 4d array containing weighted points into a 3d unweighted points array and a 1d weights array.
 */
export function separatePointsAndWeights(points is array) returns map
{
    const numPoints = size(points);
    var unweightedPoints = makeArray(numPoints);
    var weights = makeArray(numPoints);
    for (var i = 0; i < numPoints; i += 1)
    {
        unweightedPoints[i] = subArray(points[i], 0, 3) / points[i][3];
        weights[i] = points[i][3];
    }
    return { "points" : unweightedPoints, "weights" : weights };
}

/**
 * Combines a 3d unweighted points array and a 1d weights array into a 4d weighted points array
 */
export function combinePointsAndWeights(points is array, weights is array) returns array
{
    const numPoints = size(points);
    var weightedPoints = makeArray(numPoints);
    for (var i = 0; i < numPoints; i += 1)
    {
        const point = points[i];
        const weight = weights[i];
        var weightedPoint = vector(makeArray(4, 0));
        for (var j = 0; j < 3; j += 1)
        {
            weightedPoint[j] = weight * point[j];
        }
        weightedPoint[3] = weight;
        weightedPoints[i] = weightedPoint;
    }
    return weightedPoints;
}
