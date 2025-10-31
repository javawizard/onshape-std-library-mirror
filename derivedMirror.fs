FeatureScript 2796; /* Automatically generated version */
// This module is part of the FeatureScript Standard Library and is distributed under the MIT License.
// See the LICENSE tab for the license text.
// Copyright (c) 2013-Present Onshape Inc.

import(path : "onshape/std/containers.fs", version : "2796.0");
import(path : "onshape/std/feature.fs", version : "2796.0");
import(path : "onshape/std/instantiator.fs", version : "2796.0");
import(path : "onshape/std/tabReferences.fs", version : "2796.0");
import(path : "onshape/std/surfaceGeometry.fs", version : "2796.0");
import(path : "onshape/std/evaluate.fs", version : "2796.0");
import(path : "onshape/std/coordSystem.fs", version : "2796.0");
import(path: "onshape/std/vector.fs", version : "2796.0");

/**
 * @internal
 * Internal feature for assembly mirror.
 *
 * @param id : @autocomplete `id + "derivedMirror"`
 */
annotation { "Feature Type Name" : "Derived mirror", "Feature Type Description" : "" }
export const derivedMirror = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Part Studio",
                     "UIHint" : [UIHint.READ_ONLY, UIHint.UNCONFIGURABLE] }
        definition.partStudio is PartStudioData;
    }
    {
        if (definition.partStudio.buildFunction == undefined)
        {
            throw regenError(ErrorStringEnum.DERIVED_NO_PARTS, ["partStudio"]);
        }

        const instantiator = newInstantiator(id, { "clearCustomProperties" : true, "nameSuffix" : "-Mirrored" });
        addInstance(instantiator, definition.partStudio, {});
        instantiate(context, instantiator);

        const allBodies = qEverything(EntityType.BODY);
        const allParts = qUnion(
            allBodies->qBodyType(BodyType.SOLID),
            allBodies->qBodyType(BodyType.SHEET),
            allBodies->qBodyType(BodyType.COMPOSITE)
        )->qConstructionFilter(ConstructionObject.NO);
        const allMateConnectors = qBodyType(allBodies, BodyType.MATE_CONNECTOR);
        opTransform(context, id + "mirrorTransform", {
            "bodies" : allParts,
            "transform" : mirrorAcross(XY_PLANE)
        });
        for (var index, mateConnectorQuery in evaluateQuery(context, allMateConnectors)) {
            var mateConnector = evMateConnector(context, { "mateConnector" : mateConnectorQuery });
            var mirroredCS = coordSystem(vector(mateConnector.origin[0], mateConnector.origin[1], -1 * mateConnector.origin[2]), vector(mateConnector.xAxis[0], mateConnector.xAxis[1], -1 * mateConnector.xAxis[2]), vector(mateConnector.zAxis[0], mateConnector.zAxis[1], -1 * mateConnector.zAxis[2]));
            opTransform(context, id + "transform" + index, {
                    "bodies" : mateConnectorQuery,
                    "transform" :
                    toWorld(mirroredCS) *
                    fromWorld(mateConnector)  });
        }
    });

