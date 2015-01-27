// Functions for constructing queries
export import(path : "onshape/std/surfaceGeometry.fs", version : "");



// When evaluated all the queries except for UNION order their output by deterministic ids
// UNION query preserves order of sub-queries.
export enum QueryType
{
    //Special
    NOTHING,
    EVERYTHING,
    NTH_ELEMENT,
    ENTITY_FILTER,
    HISTORICAL,
    CREATED_BY,
    SKETCH_REGION,
    TRANSIENT,
    //Boolean
    UNION,
    INTERSECTION,
    SUBTRACTION,
    //Topological
    OWNED_BY_PART,
    OWNER_PART,
    VERTEX_ADJACENT,
    EDGE_ADJACENT,
    LOOP_AROUND_FACE,
    SHELL_CONTAINING_FACE,
    //Geometry types
    GEOMETRY,
    BODY_TYPE,
    //Geometry matching -- TODO
    PLANE_NORMAL,
    //Tangency
    TANGENT_EDGES,
    TANGENT_FACES,
    // face related queries
    CONVEX_CONNECTED_FACES,
    CONCAVE_CONNECTED_FACES,
    TANGENT_CONNECTED_FACES,
    LOOP_BOUNDED_FACES,
    FACE_OR_EDGE_BOUNDED_FACES,
    HOLE_FACES,
    FILLET_FACES,
    PATTERN,
    //Containment / Intersection
    CONTAINS_POINT,
    INTERSECTS_LINE,
    INTERSECTS_PLANE,
    INTERSECTS_BALL,
    //Optimization
    CLOSEST_TO, //point
    FARTHEST_ALONG, //direction
    LARGEST,
    SMALLEST
}
// Following enums can be used in query filters
export enum EntityType
{
    VERTEX,
    EDGE,
    FACE,
    BODY
}

export enum GeometryType
{
    LINE,
    CIRCLE,
    ARC,
    OTHER_CURVE,
    PLANE,
    CYLINDER,
    CONE,
    SPHERE,
    TORUS,
    OTHER_SURFACE
}

export enum BodyType
{
    SOLID,
    SHEET,
    WIRE,
    ACORN
}

export enum ConstructionObject
{
    YES,
    NO
}

export enum SketchObject
{
    YES,
    NO
}
export enum EdgeTopology
{
    LAMINAR,
    TWO_SIDED
}

export enum CompareType
{
    EQUAL,
    LESS,
    LESS_EQUAL,
    GREATER,
    GREATER_EQUAL
}

//Short hands expanded in precondition processing
export enum QueryFilterCompound
{
    ALLOWS_AXIS            // = GeometryType.LINE || GeometryType.CIRCLE || GeometryType.ARC || GeometryType.CYLINDER
}
export type Query typecheck canBeQuery;

export predicate canBeQuery(value)
{
    value is map;
    value.queryType is QueryType || value.historyType is string;
    value.entityType is undefined || value.entityType is EntityType;
}

//put Query type on a map
export function makeQuery(value is map) returns Query
{
    return value as Query;
}

//Don't strip units off historical queries
export function stripUnits(value is Query)
{
    if(value.historyType != undefined)
        return value;
    return ::stripUnits(value as map);
}


// =========================== Special Queries ===============================

export function qNothing() returns Query
{
    return { queryType : QueryType.NOTHING } as Query;
}

export function qEverything() returns Query
{
    return { queryType : QueryType.EVERYTHING } as Query;
}

export function qEverything(entityType is EntityType) returns Query
{
    return { queryType : QueryType.EVERYTHING, "entityType" : entityType } as Query;
}

export function qNthElement(subquery is Query, n is number) returns Query //zero-based.  n == -1 means last element
precondition
{
    isInteger(n);
}
{
    return { queryType : QueryType.NTH_ELEMENT, "n" : n, "subquery" : subquery } as Query;
}

export function qEntityFilter(subquery is Query, entityType is EntityType) returns Query
{
    return { queryType : QueryType.ENTITY_FILTER, "entityType" : entityType, "subquery" : subquery } as Query;
}

export function qCreatedBy(featureId is Id) returns Query
{
    return { "queryType" : QueryType.CREATED_BY, "featureId" : featureId } as Query;
}

export function qCreatedBy(featureId is Id, entityType is EntityType) returns Query
{
    return { "queryType" : QueryType.CREATED_BY, "featureId" : featureId, "entityType" : entityType } as Query;
}

export function qTransient(id is TransientId) returns Query
{
    return { "queryType" : QueryType.TRANSIENT, "transientId" : id } as Query;
}

export function transientQueriesToStrings(query is Query)
{
    if (query.queryType == QueryType.TRANSIENT)
        return @transientIdToString(query.transientId);
    else
        return transientQueriesToStrings(query as map);
}

export function transientQueriesToStrings(value is map) returns map
{
    for(var entry in value)
    {
        if (!(entry.key is array) && !(entry.key is map))
            value[entry.key] = transientQueriesToStrings(entry.value);
        else
        {
            value[entry.key] =  undefined;
            value[transientQueriesToStrings(entry.key)] = transientQueriesToStrings(entry.value);
        }
    }
    return value;
}
export function transientQueriesToStrings(value is array) returns array
{
    for(var i = 0; i < @size(value); i += 1)
    {
        value[i] = transientQueriesToStrings(value[i]);
    }
    return value;
}
export function transientQueriesToStrings(value)
{
    return value;
}
// ===================================== Boolean Queries ================================

// When evaluated qUnion preserves order of subQueries in its output
export function qUnion(subqueries is array) returns Query
precondition
{
    for(var subquery in subqueries)
        subquery is Query;
}
{
    return { "queryType" : QueryType.UNION, "subqueries" : subqueries } as Query;
}

export function qIntersection(subqueries is array) returns Query
precondition
{
    for(var subquery in subqueries)
        subquery is Query;
}
{
    return { "queryType" : QueryType.INTERSECTION, "subqueries" : subqueries } as Query;
}

export function qSubtraction(query1 is Query, query2 is Query) returns Query
{
    return { "queryType" : QueryType.SUBTRACTION, "query1" : query1, "query2" : query2} as Query;
}

export function qSymmetricDifference(query1 is Query, query2 is Query) returns Query
{
    return qUnion([qSubtraction(query1, query2), qSubtraction(query2, query1)]);
}

// ===================================== Topological Queries ===================================

export function qOwnedByPart(part is Query) returns Query
{
    return { "queryType" : QueryType.OWNED_BY_PART, "part" : part } as Query;
}

export function qOwnedByPart(part is Query, entityType is EntityType) returns Query
{
    return { "queryType" : QueryType.OWNED_BY_PART, "part" : part, "entityType" : entityType } as Query;
}

export function qOwnedByPart(subquery is Query, part is Query) returns Query
{
    return { "queryType" : QueryType.OWNED_BY_PART, "subquery" : subquery, "part" : part } as Query;
}

export function qOwnerPart(query is Query) returns Query
{
    return { "queryType" : QueryType.OWNER_PART, "query" : query } as Query;
}

//Returns entities of specified type that share a vertex with any of those returned by the input query
export function qVertexAdjacent(query is Query, entityType is EntityType) returns Query
precondition
{
    entityType != EntityType.BODY;
}
{
    return { "queryType" : QueryType.VERTEX_ADJACENT, "query" : query, "entityType" : entityType } as Query;
}

//Returns entities of specified type that share an edge with any of those returned by the input query
export function qEdgeAdjacent(query is Query, entityType is EntityType) returns Query
precondition
{
    entityType != EntityType.BODY;
    entityType != EntityType.VERTEX;
}
{
    return { "queryType" : QueryType.EDGE_ADJACENT, "query" : query, "entityType" : entityType } as Query;
}

//LOOP_AROUND_FACE,
//SHELL_CONTAINING_FACE,
//===================================== Geometry Type Queries =====================================

export function qGeometry(subquery is Query, geometryType is GeometryType) returns Query
{
    return { "queryType" : QueryType.GEOMETRY, "geometryType" : geometryType, "subquery" : subquery } as Query;
}

export function qBodyType(subquery is Query, bodyType is BodyType) returns Query
{
    return { "queryType" : QueryType.BODY_TYPE, "bodyType" : bodyType, "subquery" : subquery } as Query;
}
// ===================================== Geometry matching Queries =====================================
/* Not done yet
export function qPlanarNormal(subquery is Query, normal is Vector) returns Query
{
    return { "queryType" : QueryType.PLANE_NORMAL, "subquery" : subquery, "normal" : normal} as Query;
}
*/
// ===================================== Tangency Queries =====================================
//TANGENT_EDGES,
//TANGENT_FACES,

// ===================================== Faces Related Queries =====================================
export function qConvexConnectedFaces(subquery is Query) returns Query
{
    return { "queryType" : QueryType.CONVEX_CONNECTED_FACES, "subquery" : subquery} as Query;
}
export function qConcaveConnectedFaces(subquery is Query) returns Query
{
    return { "queryType" : QueryType.CONCAVE_CONNECTED_FACES, "subquery" : subquery} as Query;
}
export function qTangentConnectedFaces(subquery is Query) returns Query
{
    return { "queryType" : QueryType.TANGENT_CONNECTED_FACES, "subquery" : subquery} as Query;
}
export function qLoopBoundedFaces(subquery is Query) returns Query
{
    return { "queryType" : QueryType.LOOP_BOUNDED_FACES, "subquery" : subquery} as Query;
}
export function qFaceOrEdgeBoundedFaces(subquery is Query) returns Query
{
    return { "queryType" : QueryType.FACE_OR_EDGE_BOUNDED_FACES, "subquery" : subquery} as Query;
}
export function qHoleFaces(subquery is Query) returns Query
{
    return { "queryType" : QueryType.HOLE_FACES, "subquery" : subquery} as Query;
}

export function qSketchRegion(featureId is Id) returns Query
{
    return { "queryType" : QueryType.SKETCH_REGION, "featureId" : featureId, "filterInnerLoops" : false } as Query;
}

export function qSketchRegion(featureId is Id, filterInnerLoops is boolean) returns Query
{
    return { "queryType" : QueryType.SKETCH_REGION, "featureId" : featureId, "filterInnerLoops" : filterInnerLoops } as Query;
}

// find fillet faces of radius equal to , less than and equal to, greater than and equal to the
// input faces. Will find the fillet radius from the faces and then compare to find all the faces
// of fillets that satisfy the compareType. The input faces should be from a fillet other wise not
// faces will be found.
export function qFilletFaces(subquery is Query, compareType is CompareType) returns Query
precondition
{
    compareType == CompareType.EQUAL || compareType == CompareType.LESS_EQUAL || CompareType == CompareType.GREATER_EQUAL;
}
{
    return { "queryType" : QueryType.FILLET_FACES, "compareType" : compareType, "subquery" : subquery} as Query;
}

export function qMatchingFaces(subquery is Query) returns Query
{
    return { "queryType" : QueryType.PATTERN, "subquery" : subquery} as Query;
}


//===================================== Containment / Intersection Queries =====================================
export function qContainsPoint(subquery is Query, point is Vector) returns Query
precondition
{
    is3dLengthVector(point);
}
{
    point = stripUnits(point);
    return { "queryType" : QueryType.CONTAINS_POINT, "subquery" : subquery, "point" : point } as Query;
}

//INTERSECTS_LINE,

export function qIntersectsPlane(subquery is Query, plane is Plane) returns Query
{
    return { "queryType" : QueryType.INTERSECTS_PLANE, "subquery" : subquery, "plane" : stripUnits(plane) } as Query;
}

//INTERSECTS_BALL,
//===================================== Optimization Queries =====================================
/* Not done yet
export function qClosestTo(subquery is Query, point is Vector) returns Query
precondition
{
    is3dLengthVector(point);
}
{
    return { "queryType" : QueryType.CLOSEST_TO, "subquery" : subquery, "point" : point} as Query;
}
*/
//FARTHEST_ALONG, //direction
//LARGEST,
//SMALLEST

// ==================================== Historical Query stuff ================================

//historical query function
export function makeQuery(operationId is Id, queryType is string, entityType is EntityType, value is map) returns Query
{
    return @mergeMaps(value,
                    {"operationId" : operationId, "queryType" : queryType,
                      "entityType" : entityType, "historyType" : "CREATION"}) as Query;
}

export function dummyQuery(operationId is Id, entityType is EntityType, disambiguationOrder is number) returns Query
{
    return makeQuery({ "operationId" : operationId, historyType : "CREATION", "entityType" : entityType,
    queryType : "DUMMY", disambiguationData : [{ disambiguationType : "ORDER", order : disambiguationOrder }] });
}
export function dummyQuery(operationId is Id, entityType is EntityType) returns Query
{
    return makeQuery({ "operationId" : operationId, historyType : "CREATION",
                "entityType" : entityType, queryType : "DUMMY"});
}

export function qBodySplitBy(featureId is Id, backBody is boolean)
{
    return makeQuery(featureId, "SPLIT", EntityType.BODY, {"isFromBackBody" : backBody});
}
export function sketchEntityQuery(operationId is Id, entityType is EntityType, sketchEntityId is string) returns Query
{
    return makeQuery(operationId, "SKETCH_ENTITY", entityType,
                      { "sketchEntityId" : sketchEntityId });
}
export function orderDisambiguation(order is number)
{
    return { disambiguationType : "ORDER", "order" : order };
}
export function topologyDisambiguation(topology is array)
{
    return { disambiguationType : "TOPOLOGY", entities : topology };
}

export function originalSetDisambiguation(queries is array)
{
    return { disambiguationType : "ORIGINAL_DEPENDENCY", originals : queries};
}
export function trueDependencyDisambiguation(queries is array)
{
    return { disambiguationType : "TRUE_DEPENDENCY", derivedFrom : queries};
}
export function ownerDisambiguation(topology is array)
{
    return { disambiguationType : "OWNER", owners : topology};
}

export type TransientId typecheck canBeTransientId;
export predicate canBeTransientId(value)
{
    value is builtin;
}

export function toString(value is TransientId)
{
    return "Tr:" ~ @transientIdToString(value);
}

//====================== Id ========================
export type Id typecheck canBeId;

export predicate canBeId(value)
{
    value is array;
    for (var comp in value)
    {
            comp is string;
            replace(comp, "\\*?[a-zA-Z0-9_.+/\\-]", "") == ""; //All characters should be of this form
    }
}

export const baseId = [] as Id;
export function newId() returns Id
{
    return [] as Id;
}

export function id(idComp is string) returns Id
{
    return [idComp] as Id;
}

export const ANY_ID = '*';

export function unstableIdComponent(addend) returns string
{
    return (ANY_ID ~ addend);
}

export operator+(id is Id, addend is string) returns Id
{
    return append(id, addend) as Id;
}

export operator+(id is Id, addend is number) returns Id
{
    return id + replace("" ~ addend, "\\.", "_");
}

export operator+(id is Id, addend is Id) returns Id
{
    return concatenateArrays([id, addend]) as Id;
}

//==================

export function notFoundErrorKey(paramName is string) returns string
{
    return paramName ~ "notFoundError";
}

//backward compatibility -- do not use these functions.  Will need to figure out a way to remove them.
export function query(operationId is Id, queryType is string, entityType is EntityType, value is map) returns Query
{
    return makeQuery(operationId, queryType, entityType, value);
}

export function query(value is map) returns Query
{
    return makeQuery(value);
}

