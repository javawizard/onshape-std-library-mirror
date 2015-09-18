FeatureScript 225; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/query.fs", version : "");
export import(path : "onshape/std/tool.fs", version : "");

// Imports used internally
import(path : "onshape/std/patternUtils.fs", version : "");
import(path : "onshape/std/transform.fs", version : "");

/**
 * TODO: description
 * @param context
 * @param id : @eg `id + TODO`
 * @param definition {{
 *      @field TODO
 * }}
 */
annotation { "Feature Type Name" : "Curve pattern", "Filter Selector" : "allparts" }
export const curvePattern = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Entities to pattern", "Filter" : EntityType.BODY }
        definition.entities is Query;

        annotation { "Name" : "Curve", "Filter" : EntityType.EDGE }
        definition.curve is Query;

        annotation { "Name" : "Instance count" }
        isInteger(definition.instanceCount, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Opposite direction", "UIHint" : "OPPOSITE_DIRECTION" }
        definition.oppositeDirection is boolean;

        annotation { "Name" : "Follow curve" }
        definition.followCurve is boolean;

        booleanStepPredicate(definition);
    }
    {
        // Compute a vector of transforms
        var transforms = [];
        var parameters = [];
        var instNames = [];
        // Equally spaced
        const count = definition.instanceCount;
        const isClosed = size(evaluateQuery(context, qVertexAdjacent(definition.curve, EntityType.VERTEX))) < 2;
        const parameterStep = 1 / (count - (isClosed ? 0 : 1));
        for (var i = 0; i < count; i += 1)
        {
            parameters = append(parameters, i * parameterStep);
            instNames = append(instNames, "" ~ i);
        }

        if (definition.oppositeDirection)
        {
            parameters = reverse(parameters);
        }
        const evaluated = evEdgeTangentLines(context, { "edge" : definition.curve, "parameters" : parameters });
        var lastPoint;
        var lastTan;
        var curTransform;
        for (var positionAndTangent in evaluated)
        {
            const pos = positionAndTangent.origin;
            const tangent = positionAndTangent.direction;

            if (curTransform == undefined)
            {
                curTransform = identityTransform();
            }
            else
            {
                if (definition.followCurve)
                {
                    // Compute a rotation from the old to the new
                    const rotation = rotationMatrix3d(lastTan, tangent);
                    curTransform = transform(rotation, pos) * transform(-lastPoint) * curTransform;
                }
                else
                {
                    curTransform = transform(pos - lastPoint) * curTransform;
                }
            }
            lastTan = tangent;
            lastPoint = pos;
            transforms = append(transforms, curTransform);
        }

        definition.transforms = transforms;
        definition.instanceNames = instNames;
        opPattern(context, id, definition);
        const reconstructOp = function(id) { opPattern(context, id, definition); };
        processNewBodyIfNeeded(context, id, definition, reconstructOp);
    });

