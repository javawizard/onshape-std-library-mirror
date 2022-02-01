FeatureScript 1691; /* Automatically generated version */
export import(path : "onshape/std/partstudioitemtype.gen.fs", version : "1691.0");

import(path : "onshape/std/query.fs", version : "1691.0");

/**
 * The value of a Part Studio reference parameter, specifying user-selected parts or other bodies from another
 * Part Studio. The bodies can be added to the current context using an [Instantiator](/FsDoc/library.html#module-instantiator).
 *
 * Full documentation and examples uses can be found [here](/FsDoc/imports.html#part-studio).
 *
 * @seeAlso [addInstance(Instantiator, PartStudioData, map)]
 *
 * @type {{
 *      @field buildFunction {BuildFunction}: A function with one argument (a configuration map) which returns a new context
 *           containing all parts of the referenced Part Studio.
 *      @field configuration {map}: The user-input values for the configuration of the selected Part Studio. The keys of this
 *           map are the configuration inputs' [FeatureScript ids](https://forum.onshape.com/discussion/9001/configurations-update-edit-featurescript-ids),
 *           and the map can be passed (either as-is or modified) into the buildFunction above, or to [addInstance].
 *      @field partQuery {Query}: A query for the user-selected parts in the other context.
 * }}
 */
export type PartStudioData typecheck canBePartStudioData;

/** @internal */
export predicate canBePartStudioData(value) {
    value is map;
    value.buildFunction is function || value.buildFunction == undefined; // BuildFunction
    value.configuration is map || value.configuration == undefined;
    value.partQuery is Query || value.partQuery == undefined;
}

/**
 * The value of an image reference parameter, which can be placed in a Part Studio using [skImage]. Outside of use
 * with skImage, color data for individual pixels is not accessable from FeatureScript.
 *
 * Full documentation and example uses can be found [here](/FsDoc/imports.html#image).
 *
 * @type {{
 *      @field imageWidth {number} : Width of the image, in pixels
 *      @field imageHeight {number} : Height of the image, in pixels
 *      @field mediaType {string} : MIME type of the uploaded image
 * }}
 */
export type ImageData typecheck canBeImageData;

/** @internal */
export predicate canBeImageData(value) {
    value is map;
    value.imageWidth is number || value.imageWidth == undefined;
    value.imageHeight is number || value.imageHeight == undefined;
    value.mediaType is string || value.mediaType == undefined;
}

/**
 * The value of a CSV reference parameter, containing the file's tabular data as an array of arrays.
 *
 * Full documentation and example uses can be found [here](/FsDoc/imports.html#csv).
 *
 * @type {{
 *      @field csvData {array} : If the CSV contains a single rows, this value is a single array of strings or numbers.
 *          If the value contains multiple rows, this value is an array of arrays of strings or numbers. Individual cell
 *          values can be accessed by indexing into these arrays (`var row1column2 = definition.myTable.csvData[0][1]`),
 *          or by iterating through them (`for (var row in definition.myTable.csvData) { println(row); }`).
 * }}
 */
export type TableData typecheck canBeTableData;

/** @internal */
export predicate canBeTableData(value) {
    value is map;
    value.csvData is array || value.csvData == undefined;
}

/**
 * The value of a JSON reference parameter, containing the file's data.
 *
 * @type {{
 *      @field jsonData : A value that represents the top-level entity of the imported JSON file: this is a map if
 *          the JSON entity is an object, an array if the JSON entity is an array, and likewise for the standard JSON
 *          types. Note that JSON `null` values are imported as `undefined`.
 * }}
 */
export type JSONData typecheck canBeJSONData;

/** @internal */
export predicate canBeJSONData(value) {
    value is map;
}

/**
 * The value of a CAD import reference parameter, which can be used by a Part Studio import feature.
 * The data is not accessible outside of an import operation.
 *
 * Full documentation and example can be found [here](/FsDoc/imports.html#cad-import).
 *
 */
export type CADImportData typecheck canBeCADImportData;

/** @internal */
export predicate canBeCADImportData(value)
{
    value is map;
}

