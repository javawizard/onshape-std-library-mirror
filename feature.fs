FeatureScript 2473; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present PTC Inc.

// Imports that most features will need to use.
export import(path : "onshape/std/context.fs", version : "2473.0");
export import(path : "onshape/std/error.fs", version : "2473.0");
export import(path : "onshape/std/featuredimensiontype.gen.fs", version : "2473.0");
export import(path : "onshape/std/geomOperations.fs", version : "2473.0");
export import(path : "onshape/std/query.fs", version : "2473.0");
export import(path : "onshape/std/uihint.gen.fs", version : "2473.0");

// Imports used internally
import(path : "onshape/std/containers.fs", version : "2473.0");
import(path : "onshape/std/math.fs", version : "2473.0");
import(path : "onshape/std/recordpatterntype.gen.fs", version : "2473.0");
import(path : "onshape/std/string.fs", version : "2473.0");
import(path : "onshape/std/transform.fs", version : "2473.0");
import(path : "onshape/std/units.fs", version : "2473.0");
import(path : "onshape/std/tabReferences.fs", version : "2473.0");

/**
 * This function takes a regeneration function and wraps it to create a feature. It is exactly like
 * the one-argument version of `defineFeature` but the additional argument enables setting
 * default values for feature parameters when they are not passed in.
 *
 * @param defaults : A map of default parameter values for when this feature is
 *          called in FeatureScript.
 *
 *          This does NOT control the user-visible default value when creating
 *          this feature. To change the user-visible default for booleans, enums,
 *          and strings, use the "Default" annotation. To change the user-visible
 *          default for a length, angle, or number, see the `valueBounds`
 *          module.
 *
 *          @eg `{}` will not modify the `definition`.
 *          @eg `{ "shouldFillet" : false }` will set the parameter
 *              `"shouldFillet"` to `false` if the feature is called from
 *              FeatureScript without the "shouldFillet" parameter.
 */
export function defineFeature(feature is function, defaults is map) returns function
{
    return function(context is Context, id is Id, definition is map)
        {
            var token is map = {};
            var started = false;
            try
            {
                // TODO: definition = @convert(definition, CurrentVersion);
                definition = mergeDefaultsAndCleanFailed(context, definition, defaults);
                var visible = definition; /* visible to feature */
                definition.lock = true;
                visible.asVersion = undefined; // Don't let the feature body know if there's been an upgrade
                token = startFeature(context, id, definition);
                started = true;
                feature(context, id, visible);
                const error = getFeatureError(context, id);
                if (error != undefined && error != ErrorStringEnum.NO_ERROR)
                {
                    if (!isTopLevelId(id))
                        throw regenError(error);
                    else
                        @abortFeature(context, id, token);
                }
                else
                {
                    @endFeature(context, id, token);
                }
            }
            catch (error)
            {
                if (try(processError(context, id, error)) == undefined)
                    reportFeatureError(context, id, ErrorStringEnum.REGEN_ERROR);
                if (started)
                    @abortFeature(context, id, token);
                if (!isTopLevelId(id))
                    throw error; // rethrow
            }
        };
}

/**
 * This function takes a regeneration function and wraps it to create a feature. The wrapper handles certain argument
 * recording for the UI and error handling.  A typical usage is something like:
 * ```
 * annotation { "Feature Type Name" : "Widget" } // This annotation is required for Onshape to recognize widget as a feature.
 * export const widget = defineFeature(function(context is Context, id is Id, definition is map)
 *     precondition
 *     {
 *         ... // Specify the parameters that this feature takes
 *     }
 *     {
 *         ... // Specify what the feature does when regenerating
 *     });
 * ```
 *
 * For more information on writing features, see `Specifying feature UI` in the
 * language guide.
 *
 * @param feature : A function that takes a `context`, an `id`, and a
 *          `definition` and regenerates the feature.
 *          @autocomplete
 * ```
 * function(context is Context, id is Id, definition is map)
 *     precondition
 *     {
 *         // Specify the parameters that this feature takes
 *     }
 *     {
 *         // Specify what the feature does when regenerating
 *     }
 * ```
 */
export function defineFeature(feature is function) returns function
{
    return defineFeature(feature, {});
}

// =====================================================================
/**
 * @internal
 *
 * Starts the feature and associates the queries with the feature id in the context.
 */
export function startFeature(context is Context, id is Id, definition is map)
{
    var token = @startFeature(context, id, definition);
    recordParameters(context, id, definition);
    return token;
}

export function startFeature(context is Context, id is Id)
{
    return startFeature(context, id, {});
}

/**
 * @internal
 *
 * Rolls back the feature.
 */
export function abortFeature(context is Context, id is Id)
{
    @abortFeature(context, id, {});
}

/**
 * @internal
 *
 * Ends the feature; if the feature has an associated error, it is rolled back.
 */
export function endFeature(context is Context, id is Id)
{
    if (getFeatureError(context, id) != undefined)
    {
        @abortFeature(context, id, {});
    }
    else
    {
        @endFeature(context, id, {});
    }
}

/**
 * This function can be used to call a subfeature or sub-operation (such as `extrude` or `opExtrude`, respectively). It
 * will properly handle any statuses as if they came from the top level feature. That is, reported `INFO` will display
 * as a blue message bubble, `WARNING` will turn the feature orange with a warning tooltip on hover, and `ERROR` will
 * throw an error after status handling (aborting feature execution if it is not caught). Any error entities that the
 * subfeature emits will also be displayed.
 *
 * @ex `callSubfeatureAndProcessStatus(id, booleanBodies, context, id + "boolean", booleanDefinition);` (where `id` is
 *     the top-level feature id passed into the feature) will call
 *     `booleanBodies(context, id + "boolean", booleanDefinition)`, propagate its status onto the current feature, and
 *     show any error entities coming from the boolean.
 *
 * Internally, calls the supplied function, and attaches any status it produces to the `topLevelId` using
 * [processSubfeatureStatus]. If calling the function produces an error, the error is re-thrown. If the function
 * produces a return value, that value is returned.
 *
 * @param topLevelId   : @autocomplete `id`
 * @param fn           : @autocomplete `booleanBodies`
 * @param subfeatureId : @autocomplete `id + "boolean"`
 * @param definition   : @autocomplete `booleanDefinition`
 */
export function callSubfeatureAndProcessStatus(topLevelId is Id, fn is function, context is Context, subfeatureId is Id, definition is map)
{
    return callSubfeatureAndProcessStatus(topLevelId, fn, context, subfeatureId, definition, { "propagateErrorDisplay" : true });
}

/**
 * See [callSubfeatureAndProcessStatus](callSubfeatureAndProcessStatus(Id, function, Context, Id, map)).
 *
 * @param topLevelId   : @autocomplete `id`
 * @param fn           : @autocomplete `booleanBodies`
 * @param subfeatureId : @autocomplete `id + "boolean"`
 * @param definition   : @autocomplete `booleanDefinition`
 * @param processSubfeatureStatusOptions : Passed as the `definition` argument to [processSubfeatureStatus]. Setting
 *                                         `subfeatureId` in this map is not required, and will be ignored in favor of
 *                                         the `subfeatureId` passed into this function.
 *                                         @autocomplete `{ "propagateErrorDisplay" : true }`
 */
export function callSubfeatureAndProcessStatus(topLevelId is Id, fn is function, context is Context, subfeatureId is Id,
    definition is map, processSubfeatureStatusOptions is map)
{
    var returnValue;
    var thrownError;
    try
    {
        returnValue = fn(context, subfeatureId, definition);
    }
    catch (e)
    {
        thrownError = e;
    }

    processSubfeatureStatusOptions.subfeatureId = subfeatureId;
    processSubfeatureStatus(context, topLevelId, processSubfeatureStatusOptions);

    // Re-throw feature error if it exists. Do not throw WARN or INFO level status. Important to do this before
    // re-throwing `throwError` because `processSubfeatureStatus` may have changed the error in question (such as
    // remapping the set of faulty parameters).
    const featureError = getFeatureError(context, topLevelId);
    if (featureError != undefined)
    {
        throw featureError;
    }

    // Re-throw any thrown error that did not create a ERROR level feature status.  This is rare, but could happen in
    // cases where `fn` is not an operation, or not a true feature (a function created using `defineFeature`).
    if (thrownError != undefined && isAtVersionOrLater(context, FeatureScriptVersionNumber.V1560_STATUS_ON_THROW))
    {
        throw thrownError;
    }

    return returnValue;
}

/**
 * Iterate through all entities provided by a query, calling the provided function once for each geometric entity
 * resolved by the provided `query`.
 *
 * `forEachEntity` behaves much like the code:
 * ```
 * const evaluated = evaluateQuery(context, query);
 * for (var i = 0; i < size(evaluated); i += 1)
 * {
 *     operationToPerform(id + i, evaluated[i]);
 * }
 * ```
 * However, `forEachEntity` has one additional benefit: The `entId` this function provides to `operationToPerform` is tied to
 * the entity itself, not its index `i`. This means that downstream features made in the Part Studio will hold up better across
 * upstream changes.
 *
 * For example, imagine the following scenario: A user inserts a custom feature which places a slot on every selected line. That
 * feature calls `forEachEntity(context, lineQuery ...)`. The user then makes a sketch downstream which uses geometry from e.g. the
 * third slot. Finally, the user decides some slots are unnecessary and deletes some of the lines. Since the feature used
 * `forEachEntity`, the user's downstream sketch will still reference the same slots. If the feature instead used the code above,
 * the user's sketch would break or jump around, since a different slot would suddenly become "slot 3".
 *
 * Aside from that difference, the two are interchangable.
 *
 * Like any expression function, be warned that the provided `operationToPerform` can read but can NOT modify the values of
 * variables outside the function. It can, however, modify values inside a [box](https://cad.onshape.com/FsDoc/variables.html#box).
 * @example ```
 * const allParts = qAllModifiableSolidBodies();
 * const threshold = 0.01 * inch^3;
 * var deletedSizes is box = new box([]); // box containing an empty array
 * forEachEntity(context, id + "deleteSmallParts", allParts, function(entity is Query, id is Id)
 * {
 *     const size = evVolume(context, {
 *             "entities" : entity
 *     });
 *     if (size < threshold)
 *     {
 *         opDeleteBodies(context, id + "deleteBodies1", {
 *                 "entities" : entity
 *         });
 *         deletedSizes[] = append(deletedSizes[], size);
 *     }
 * });
 * println(deletedSizes[]);
 * ```
 *
 * @param id: @autocomplete `id + "operation"`
 * @param operationToPerform: @eg ```function(entity is Query, id is Id)
 * {
 *     // perform operations with the entity
 * }
 * ```
 *
 * The first argument to this function is a query resolving to one single entity of the input `query`.
 *
 * The second argument to this function is a unique id tied to the `entity`. By default it is named "`id`", which will shadow
 * (i.e. take precedence over) the outer variable named "`id`". If you need access to that outer `id`, simply rename this
 * argument to e.g. `innerId`.
 */
export function forEachEntity(context is Context, id is Id, query is Query, operationToPerform is function)
{
    const evaluated = evaluateQuery(context, query);
    const querySize = size(evaluated);
    for (var i = 0; i < querySize; i += 1)
    {
        const innerId = id + unstableIdComponent(i);
        const entity = evaluated[i];
        setExternalDisambiguation(context, innerId, entity);
        operationToPerform(entity, innerId);
    }
}


/**
 * @internal
 *
 * Returns a path in the id tree where an additional operation can be inserted without breaking feature execution.
 * Temporary operations may be started by adding to this id.
 */
export function getLastActiveId(context is Context) returns Id
{
    return @getLastActiveId(context) as Id;
}

/**
 * @internal
 * Gather data used in feature dialog
 */
export function recordParameters(context is Context, id is Id, definition is map)
{
    if (size(id) == 1)
    {
        recordParameters(context, id, definition, undefined, undefined);
    }
}

/**
 * @param arrayParameter: A `string` of the enclosing array, or undefined if not an array parameter.
 * @param itemIndex: A `number` index of the array item that definition represents, or undefined if not an array parameter.
 */
function recordParameters(context is Context, id is Id, definition is map, arrayParameter, itemIndex)
{
    for (var paramEntry in definition)
    {
        // Doesn't handle nested arrays, which are not allowed in feature specs
        if (arrayParameter == undefined && isArrayParameter(paramEntry.value))
        {
            var i = 0;
            for (var element in paramEntry.value)
            {
                recordParameters(context, id, element, paramEntry.key, i);
                i += 1;
            }
        }
        else
        {
            var parameterId = (arrayParameter == undefined) ? paramEntry.key : arrayParameterId(arrayParameter, itemIndex, paramEntry.key);
            if (parameterId is string)
            {
                if (paramEntry.value is Query)
                {
                    @recordQuery(context, id, { (parameterId) : paramEntry.value });
                    continue;
                }
                if (paramEntry.value is PartStudioData || paramEntry.value is TableData || paramEntry.value is ImageData)
                {
                    continue;
                }
                setFeatureComputedParameter(context, id, { "name" : parameterId, "value" : paramEntry.value });
            }
        }
    }
}

/** Returns true if the value is an array of maps where every map key is a string. */
predicate isArrayParameter(value)
{
    value is array;
    for (var element in value)
    {
        element is map;
        element as map == element; // no type tag
        for (var key, _ in element)
            key is string;
    }
}

/**
 * Associates a FeatureScript value with a given string. This value can then be referenced in a feature name using
 * the string. The provided value can be used in a feature name by including e.g. "#myValue" in the Feature
 * Name Template.
 * @param definition {{
 *      @field name {string} : @eg `myValue`
 *      @field value
 * }}
 */
export function setFeatureComputedParameter(context is Context, id is Id, definition is map)
{
    @setFeatureComputedParameter(context, id, definition);
}

/**
 * @internal
 *
 * Builds stack of patternInstanceData, endFeature/abortFeature on id parent pops the stack.
 * @param id {Id} : instance id
 * @param definition {{
 *      @field transform {Transform}
 *  }}
 */
export function setFeaturePatternInstanceData(context is Context, id is Id, definition is map)
{
    @setFeaturePatternInstanceData(context, id, definition);
}

/**
 * @internal
 *
 * pop patternInstanceData stack if id matches throw otherwice
 * @param id {Id} : instance id
 */
export function unsetFeaturePatternInstanceData(context is Context, id is Id)
{
    @unsetFeaturePatternInstanceData(context, id);
}

/**
 * When in feature pattern scope returns composition of all pattern transforms pushed by `setFeaturePatternInstanceData`
 * returns identity transform when out of scope
 */
export function getFullPatternTransform(context is Context) returns Transform
{
    return transformFromBuiltin(@getFullPatternTransform(context));
}

/**
 * Making a feature work correctly with feature patterns is usually done with two functions: this one
 * and `transformResultIfNecessary`.
 *
 * Feature patterns work by first computing a sequence of transforms, one for each instance.  For each
 * transform, the pattern pushes it onto the pattern stack (using `setFeaturePatternInstanceData`), executes
 * the patterned features, and then pops the transform off the stack (using `unsetFeaturePatternInstanceData`)
 * before pushing the next one.  The stack depth corresponds to the level of nesting of feature patterns.
 * Feature authors are responsible for reading the pattern stack and transforming themselves accordingly.
 *
 * The basic principle is that inside one feature pattern (as opposed to nested feature patterns), if any
 * entities that the feature references come from a feature that is also being patterned, then the feature
 * ignores the pattern transform.  Otherwise, the feature uses the pattern transform in a "natural" way,
 * applying it to an input, the output, or somewhere in between.
 *
 * For example, suppose the patterned feature creates a 3D line between two arbitrary vertices.  If the
 * first vertex is also patterned, but not the second, then the result should be a bunch of lines from
 * different instances of the first vertex to the unpatterned second vertex (this is accomplished by not
 * applying any transform to the line).  If neither vertex is patterned, the line should be transformed
 * by the pattern transform and the result will be as expected, as if a body pattern of these lines was
 * performed.  Other features may need to apply the transform differently: for example, a sweep can
 * transform the result of [opSweep] prior to the boolean, but an extrude needs to transform the profile
 * prior to [opExtrude] to accommodate up-to-next correctly.
 *
 * The general case is more complicated because feature patterns may be nested, and this function is
 * designed to handle them.  This function takes `references`, a query for everything the feature
 * geometrically depends on (typically a `qUnion` of the queries in the feature definition), and computes
 * the portion of the pattern transform that is not applied to any of the references and hence should
 * be applied to the feature.  For example, if one of the references is patterned by the current feature
 * pattern or if there is no feature pattern, it will return the identity transform.  If `references`
 * evaluates to nothing, it will return the current feature pattern transform.
 *
 * More precisely:
 * Among references find topology created by pattern instance deepest in the pattern transform stack.
 * If the transformation on the stack for that instance is S and the full transformation is F,
 * the remainder R is such that R * S = F
 *
 * A simple feature may use this function and `transformResultIfNecessary` as follows:
 * ```
 * ... // Feature definition boilerplate and precondition
 *     { // Feature body
 *         // Call getRemainderPatternTransform before performing any operations
 *         var remainingTransform = getRemainderPatternTransform(context, { "references" : definition.vertexToBuildOn });
 *         ... // Create a cube using definition.vertexToBuildOn as the reference location
 *         // Inside a feature pattern, the following will transform the cube if definition.vertexToBuildOn is not getting patterned:
 *         transformResultIfNecessary(context, id, remainingTransform);
 *         ... // Perhaps boolean the results to something in the context
 *     });
 * ```
 *
 * @param definition {{
 *     @field references {Query}
 * }}
 */
export function getRemainderPatternTransform(context is Context, definition is map) returns Transform
precondition
{
    definition.references is Query;
}
{
    return transformFromBuiltin(@getRemainderPatternTransform(context, definition));
}

/**
 * Applies transformation to bodies created by operation with id if transform argument is non-trivial
 */
export function transformResultIfNecessary(context is Context, id is Id, transform is Transform)
{
    if (transform != identityTransform())
    {
        opTransform(context, id + "transform",
                { "bodies" : qCreatedBy(id, EntityType.BODY),
                  "transform" : transform
                });
    }
}

//================ Compatibility with early expressions ================
/**
 * A predicate which always returns true.
 * Used to create a generic feature parameter that can be any featurescript
 * expression.
 *
 * Note that to change the user-visible default value, the "Default" annotation must be a string containing a valid parameter expression.
 * For example, to make the default value the string `"My string"`, pass in an escaped string: `annotation{ "Default": "\\"My string\\"" }`.
 */
export predicate isAnything(value)
{
}


/**
 * Returns id of operation that created or last modified the first entity to
 * which `query` resolves.
 *
 * Throws if `query` resolves to nothing.
 * @param context
 * @param query
 */
export function lastModifyingOperationId(context is Context, query is Query) returns Id
{
    return @lastModifyingOperationId(context, {"entity" : query}) as Id;
}


// ======================= Tracking Query ==================================== //
/**
* Generates a tracking query, which will evaluate to entities derived from subquery in features between
* startTracking and when query is evaluated. If secondarySubquery is specified, the query would evaluate to
* entities derived from both objects. If trackPartialDependency is set to true, query would also include entities
* that are not exclusively derived from subquery1. Optional field lastOperationId can be used to specify the
* starting operation of tracking. Use example:
 * ```
 * // "sketch1" constructs a polygon of "line0", "line1", etc.
* var extrudedFromLine0 = startTracking(context, id + "sketch1", "line0");
* extrudeOp(context, id + "extrude1", {"entities" : qSketchRegion(id + "sketch1",....});
 * var fromLine0 = evaluateQuery(context, extrudedFromLine0);
 * // fromLine0 contains a face and two edges (top and bottom) corresponding to line0 in the extrude.
 * ```
*/
export function startTracking(context is Context, arg is map) returns Query
precondition
{
    arg.subquery is Query;
    arg.secondarySubquery == undefined || arg.secondarySubquery is Query;
}
{
    var out = arg;
    out.subquery = undefined;
    out.secondarySubquery = undefined;
    out.queryType = QueryType.TRACKING;
    if (arg.subquery != undefined)
    {
        out.subquery1 = evaluateQuery(context, arg.subquery);
    }
    if (arg.secondarySubquery != undefined)
    {
        out.subquery2 = evaluateQuery(context, arg.secondarySubquery);
    }
    if (arg.trackPartialDependency != undefined)
    {
        out.trackPartialDependency = arg.trackPartialDependency;
    }
    if (arg.lastOperationId != undefined)
    {
        out.lastOperationId = arg.lastOperationId;
    }
    else
    {
        out.lastOperationId = lastOperationId(context);
    }
    return out as Query;
}

export function startTracking(context is Context, subquery is Query) returns Query
{
    return startTracking(context, {'subquery' : subquery});
}

export function startTracking(context is Context, sketchId is Id, sketchEntityId is string) returns Query
{
    var sketchQuery = sketchEntityQuery(sketchId, undefined, sketchEntityId);
    return startTracking(context, {
        'subquery' : qUnion([sketchQuery, makeQuery(sketchId, "IMPRINT", undefined, {"derivedFrom" : sketchQuery})])
        });
}


/**
* Generates a tracking query, which will evaluate to the new entities inheriting the identity of
* subquery evaluation result.
*/
export function startTrackingIdentity(context is Context, subquery is Query) returns Query
{
    return startTrackingIdentityFromOp(evaluateQuery(context, subquery), lastOperationId(context));
}

/**
 * @internal
 * Generate an array of identity-tracking queries for each entity of the subquery
 */
export function startTrackingIdentityBatched(context is Context, subquery is Query) returns array
{
    var out = [];
    const lastOperationId = lastOperationId(context);
    for (var ent in evaluateQuery(context, subquery))
    {
        out = append(out, startTrackingIdentityFromOp([ent], lastOperationId));
    }
    return out;
}

/**
* Generates query robust to identity-preserving changes
*/
export function makeRobustQuery(context is Context, subquery is Query) returns Query
{
    return qUnion(append(evaluateQuery(context, subquery), startTrackingIdentity(context, subquery)));
}

/**
* Generates array of robust queries for each entity of the subquery
*/
export function makeRobustQueriesBatched(context is Context, subquery is Query) returns array
{
    var out = [];
    const lastOperationId = lastOperationId(context);
    for (var ent in evaluateQuery(context, subquery))
    {
        out = append(out, qUnion([ent, startTrackingIdentityFromOp([ent], lastOperationId)]));
    }
    return out;
}

/**
* @internal
* Used in `startTrackingIdentity`
*/
function startTrackingIdentityFromOp(subqueries is array, operationId is Id) returns Query
{
    return {
        "subquery1" : subqueries,
        "lastOperationId" : operationId,
        "identityPreservingOnly" : true,
        "queryType" : QueryType.TRACKING
        } as Query;
}

/**
* @internal
* Used in `startTracking`
*/
function lastOperationId(context is Context) returns Id
{
    return @lastOperationId(context) as Id;
}

/**
* Used to set external disambiguation for operations with unstable component in id.
* The disambiguation will be applied to results of sub-operations which otherwise don't
* track dependency e.g. [Sketch] , [opPlane], [opPoint]
* @param id {Id} : ends in unstable component
*/
export function setExternalDisambiguation(context is Context, id is Id, query is Query)
{
    @setExternalDisambiguation(context, id, {"entity" : query});
}

/**
 * @internal
 * Suppose you have a sequence of operations with unstable components that are creating entities that
 * will be referenced within the feature and then deleted.  If this method is applied to mark the original
 * operation, order-based disambiguation of the original operation will be skipped, allowing the identity
 * of entities created by downstream operations to be based on other criteria (for instance, external disambiguation).
 *
 * Because this allows ambiguous entities to exist in the context, if any operation is marked with this without its
 * entities being deleted at the end of a feature, a warning will be reported.
 *
 * Used by the instantiator.
 */
export function skipOrderDisambiguation(context is Context, id is Id)
{
    @skipOrderDisambiguation(context, id);
}

/** @internal */
export function isInFeaturePattern(context is Context)
{
    return @isInFeaturePattern(context);
}

/**
 * @internal
 * Only accesible during tables, editing logic and manipulator change function
 */
export function getFeatureName(context is Context, id is Id)
{
    return @getFeatureName(context, id);
}

/**
 * @internal
 * @seealso [verifyNonemptyQuery(Context, map, string, string)]
 */
export function verifyNonemptyQuery(context is Context, definition is map, parameterName is string, errorToReport is ErrorStringEnum) returns array
{
    return verifyNonemptyQueryInternal(context, definition, parameterName, errorToReport);
}

/**
 * Throws a [regenError] and marks the specified [Query] parameter as faulty if the specified `Query` parameter is not
 * a `Query` which resolves to at least one entity. This happens when the user has not made any selection into the `Query`
 * parameter, or when upstream geometry has changed such that the `Query`s of the `Query` parameter no longer resolve to
 * anything. Should be used to check all non-optional `Query` parameters in a feature. By convention, `errorToReport`
 * should take the form "Select parameter display name." For example, a parameter declared as follows:
```
annotation { "Name" : "Entities to use", "Filter" : EntityType.FACE }
definition.entitiesToUse is Query;
```
 * should verify that the input is nonempty in the following way:
 * @ex `verifyNonemptyQuery(context, definition, "entitiesToUse", "Select entities to use.")`
 *
 * @returns : An array representing the result of evaluating the `Query` parameter with [evaluateQuery]
 */
export function verifyNonemptyQuery(context is Context, definition is map, parameterName is string, errorToReport is string) returns array
{
    return verifyNonemptyQueryInternal(context, definition, parameterName, errorToReport);
}

function verifyNonemptyQueryInternal(context is Context, definition is map, parameterName is string, errorToReport) returns array
{
    if (definition[parameterName] is Query)
    {
        const result = evaluateQuery(context, definition[parameterName]);
        if (result != [])
        {
            return result;
        }
    }
    // Parameter was either not a query, or was a query with no content.
    throw regenError(errorToReport, [parameterName]);
}

/**
 * @internal
 * @seealso [verifyNonemptyArray(Context, map, string, string)]
 */
export function verifyNonemptyArray(context is Context, definition is map, parameterName is string, errorToReport is ErrorStringEnum)
{
    verifyNonemptyArrayInternal(context, definition, parameterName, errorToReport);
}

/**
 * Throws a [regenError] and marks the specified array parameter as faulty if the specified array parameter does not
 * have any entries. A parameter declared as follows:
```
annotation { "Name" : "Array items", "Item name" : "Array item" }
definition.arrayItems is array;
for (var arrayItem in definition.arrayItems)
{
    ...
}
```
 * could verify that the input is nonempty in the following way:
 * @ex `verifyNonemptyArray(context, definition, "arrayItems", "Add an array item.")`
 */
export function verifyNonemptyArray(context is Context, definition is map, parameterName is string, errorToReport is string)
{
    verifyNonemptyArrayInternal(context, definition, parameterName, errorToReport);
}

function verifyNonemptyArrayInternal(context is Context, definition is map, parameterName is string, errorToReport)
{
    if (!(definition[parameterName] is array) || size(definition[parameterName]) == 0)
    {
        throw regenError(errorToReport, [parameterName]);
    }
}

/**
 * @internal
 * Throws a regeneration error if query references topology of sheet metal flat pattern
 */
export function verifyNoSheetMetalFlatQuery(context is Context, query is Query,
    parameterName is string, errorToReport is ErrorStringEnum)
{
    if (!isQueryEmpty(context, qSheetMetalFlatFilter(query, SMFlatType.YES)))
    {
        throw regenError(errorToReport, [parameterName]);
    }
}

function verifyNoMeshInQuery(context is Context, query is Query, parameterName is string)
{
    var meshEntities = qMeshGeometryFilter(query, MeshGeometry.YES);
    if (evaluateQuery(context, meshEntities) != [])
    {
        throw regenError(ErrorStringEnum.MESH_NOT_SUPPORTED, [parameterName], meshEntities);
    }
}

/**
 * Verifies that the `definition[parameterName]` [Query] does not contain mesh or mixed entities.
 * Throws a [regenError] if `definition[parameterName]` references mesh topologies.
 */
export function verifyNoMesh(context is Context, definition is map, parameterName is string)
{
    const query = definition[parameterName];
    if (query != undefined)
    {
        verifyNoMeshInQuery(context, query, parameterName);
    }
}

/**
 * Verifies no body containing the specified query contains any mesh.
 * @param context {Context} : The application context.
 * @param definition {map} : The feature definition.
 * @param parameterName {string} : The key of `definition` that will be accessed to find the query.
 */
export function verifyNoMeshInBody(context is Context, definition is map, parameterName is string)
{
    const query = definition[parameterName];
    if (query != undefined)
    {
        verifyNoMeshInQuery(context, qOwnerBody(query), parameterName);
    }
}


/**
 * Adjust angle out of bounds angles to lie in `[0 to 2pi]` if the feature is new,
 * do a range check otherwise.
 */
export function adjustAngle(context is Context, angle is ValueWithUnits) returns ValueWithUnits
precondition
{
    isAngle(angle);
}
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V681_ANGLE_BOUNDS_CHANGE))
    {
        const CIRCLE = 2 * PI * radian;
        if (abs(angle - CIRCLE) < TOLERANCE.zeroAngle * radian)
            return CIRCLE;
        return angle % CIRCLE;
    }
    if (angle < 0 * degree || angle > 360 * degree)
        throw regenError(ErrorStringEnum.PARAMETER_OUT_OF_RANGE);
    return angle;
}

/**
 * @internal
 * Parameters set to invalid expressions are temporarily set to undefined with this type tag. That value
 * is never seen inside feature code, since it is replaced with an ordinary `undefined` in
 * cleanUpFailedParameters(). A feature default will not override a failed parameter.
 */
export type FailedParameter typecheck canBeFailedParameter;

/** @internal */
export predicate canBeFailedParameter(value) {
    value is undefined;
}

const FAILED_PARAMETER = undefined as FailedParameter;

function cleanUpFailedParameters(definition is map) returns map
{
    for (var paramEntry in definition)
    {
        if (paramEntry.value == FAILED_PARAMETER)
        {
            definition[paramEntry.key] = undefined;
        }
        else if (isArrayParameter(paramEntry.value))
        {
            const size = @size(paramEntry.value);
            for (var i = 0; i < size; i += 1)
            {
                definition[paramEntry.key][i] = cleanUpFailedParameters(paramEntry.value[i]);
            }
        }
    }
    return definition;
}

function mergeDefaultsAndCleanFailed(context is Context, definition is map, defaults is map)
{
    // Holdback not yet applied, so check asVersion
    const isNewVersion = isAtVersionOrLater(context, FeatureScriptVersionNumber.V1046_FAILED_PARAMETERS);
    const isHeldBack = definition.asVersion != undefined && !isAtVersionOrLater(definition.asVersion, FeatureScriptVersionNumber.V1046_FAILED_PARAMETERS);
    if (isNewVersion && !isHeldBack)
    {
        // New: Failed parameters do not get replaced by defaults
        definition = mergeMaps(defaults, definition);
        definition = cleanUpFailedParameters(definition);
    }
    else
    {
        // Old: Clean up failed parameters immediately to preserve legacy behavior
        definition = cleanUpFailedParameters(definition);
        definition = mergeMaps(defaults, definition);
    }
    return definition;
}

/**
 * @internal
 * A feature that does nothing
 */
export const dummyFeature = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
    }
    {
    });

/**
 * @internal
 * Returns an unstable id-generating function that auto-increments each time it is called.
 * @example ```
 * const idGenerator = getUnstableIncrementingId(id + "plane");
 * //at each call to idGenerator() a new ID is created, eg:
 * //const id1 = idGenerator(); //id1 equals "FxbNP1KoQHJVVjC_1.plane.*0"
 * //const id2 = idGenerator(); //id2 equals "FxbNP1KoQHJVVjC_1.plane.*1"
 * for (var point in evaluateQuery(context, definition.points))
 * {
 *     const origin = evVertexPoint(context, { "vertex" : point });
 *     const planeId = idGenerator(); //creates an id with an unstable component
 *     opPlane(context, planeId, { "plane" : plane(origin, Z_DIRECTION) });
 * }
 * ```
 */
export function getUnstableIncrementingId(prefix is Id) returns function
{
    var index = new box(0);
    return function()
        {
            const newId = prefix + unstableIdComponent(index[]);
            index[] += 1;
            return newId;
        };
}

/**
 * @internal
 * Returns an unstable id-generating function that auto-increments each time it is called.
 * The id-generating function requires a disambiguation query as argument.

 * @example ```
 * const idGenerator = getDisambiguatedIncrementingId(context, id + "plane");
 * //at each call to idGenerator() a new ID is created, eg:
 * //const id1 = idGenerator(point1); //id1 equals "FxbNP1KoQHJVVjC_1.plane.*0"
 * //const id2 = idGenerator(point2); //id1 equals "FxbNP1KoQHJVVjC_1.plane.*1"
 * for (var point in evaluateQuery(context, definition.points))
 * {
 *     const origin = evVertexPoint(context, { "vertex" : point });
 *     const planeId = idGenerator(point); //uses point transient query to disambiguate
 *     opPlane(context, planeId, { "plane" : plane(origin, Z_DIRECTION) });
 * }
 * ```
 */
export function getDisambiguatedIncrementingId(context is Context, prefix is Id) returns function
{
    var index = new box(0);
    return function(query)
        {
            const newId = prefix + unstableIdComponent(index[]);
            setExternalDisambiguation(context, newId, query);
            index[] += 1;
            return newId;
        };
}

/** @internal */
export function setHighlightedEntities(context is Context, definition is map)
precondition
{
    definition.entities is Query;
}
{
    if (isAtVersionOrLater(context, FeatureScriptVersionNumber.V1952_DO_NOT_THROW_FOR_HIGHLIGHT_IN_PATTERN_FEATURES))
    {
        try silent(@setHighlightedEntities(context, definition));
    }
    else
    {
        try(@setHighlightedEntities(context, definition));
    }
}

/**
 * @internal
 *
 * Set computed data for pattern type and axes.
 * @param id {Id} : Feature id of pattern feature.
 * @param patternType {RecordPatternType} : Type of feature pattern.
 * @param directions {array} : Axes of pattern as an array of vectors.
 */
export function setPatternData(context is Context, id is Id, patternType is RecordPatternType, directions is array)
{
    @setPatternData(context, id, {
        "recordPatternType" : patternType,
        "patternDirections" : directions
    });
}

/**
 * True if the value is undefined and creates a button parameter.
 */
export predicate isButton(value)
{
    value is undefined;
}

/**
 * @internal
 *
 * Set the queries dimensioned by a length or angle parameter
 * Can only be called when editing a feature
 */
export function setDimensionedEntities(context is Context, definition is map)
precondition
{
    definition.parameterId is string;
    definition.queries is array;
    definition.dimensionType is FeatureDimensionType;
    if (definition.dimensionType == FeatureDimensionType.DISTANCE || definition.dimensionType == FeatureDimensionType.ANGLE)
        size(definition.queries) == 2;
    if (definition.dimensionType == FeatureDimensionType.RADIUS || definition.dimensionType == FeatureDimensionType.DIAMETER)
        size(definition.queries) == 1;
    for (var query in definition.queries)
    {
        query is Query;
    }
}
{
    @setDimensionedEntities(context, definition);
}
