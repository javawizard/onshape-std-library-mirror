FeatureScript 2433; /* Automatically generated version */
// Imports used in interface
export import(path : "onshape/std/facecurvecreationtype.gen.fs", version : "2433.0");

// Imports used internally
import(path : "onshape/std/feature.fs", version : "2433.0");
import(path : "onshape/std/valueBounds.fs", version : "2433.0");
import(path : "onshape/std/evaluate.fs", version : "2433.0");
import(path : "onshape/std/containers.fs", version : "2433.0");

/** @internal */
export enum DirectionType
{
    annotation { "Name" : "U direction" }
    U_DIRECTION,
    annotation { "Name" : "V direction" }
    V_DIRECTION
}

/** @internal */
const ISOPARAMETERIC_CURVE_BOUNDS =
{
    (unitless) : [2, 10, 50]
} as IntegerBoundSpec;

/**
 *  Creates isoparametric curves on faces by using [opCreateCurvesOnFace]
 */
annotation { "Feature Type Name" : "Isoparametric curve", "UIHint" : UIHint.NO_PREVIEW_PROVIDED }
export const isoparametricCurve = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Select face",
                     "Filter" : EntityType.FACE && ConstructionObject.NO && SketchObject.NO,
                     "MaxNumberOfPicks" : 1 }
        definition.face is Query;

        annotation { "Name" : "Direction", "UIHint" : UIHint.SHOW_LABEL }
        definition.directionType is DirectionType;

        annotation { "Name" : "Equal spacing" }
        definition.equalSpacing is boolean;

        if (definition.equalSpacing)
        {
            annotation { "Name" : "Instance count"}
            isInteger(definition.nCurves, ISOPARAMETERIC_CURVE_BOUNDS);
        }
        else
        {
            annotation { "Name" : "Position", "Item name" : "Curve" }
            definition.uvParamList is array;
            for (var param in definition.uvParamList)
            {
                annotation { "Name" : "Select point" }
                param.selectPoint is boolean;

                if (param.selectPoint)
                {
                    annotation { "Name" : "Select point" , "Filter" : EntityType.VERTEX || BodyType.MATE_CONNECTOR , "MaxNumberOfPicks" : 1}
                    param.point is Query;
                }
                else
                {
                    annotation { "Name" : "Location" }
                    isReal(param.uvValue, EDGE_PARAMETER_BOUNDS);
                }
            }
        }
    }
    {
        var numFaces = size(evaluateQuery(context, definition.face));
        if (numFaces == 0)
        {
            throw regenError(ErrorStringEnum.ISOPARAMETRIC_CURVE_SELECT_FACE, ["face"]);
        }

        if (numFaces > 1)
        {
            throw regenError(ErrorStringEnum.ISOPARAMETRIC_CURVE_SELECT_SINGLE_FACE, ["face"]);
        }

        var paramList = [];
        var count;
        if (definition.equalSpacing)
        {
            count = definition.nCurves;
        }
        else
        {
            if (size(definition.uvParamList) == 0)
            {
                throw regenError(ErrorStringEnum.ISOPARAMETRIC_CURVE_SELECT_POSITION);
            }

            for (var uvParam in definition.uvParamList)
            {
                var point;
                if (uvParam.selectPoint)
                {
                    if (isQueryEmpty(context, uvParam.point))
                    {
                        reportFeatureWarning(context, id, ErrorStringEnum.ISOPARAMETRIC_CURVE_SELECT_POINT);
                        continue;
                    }

                    point = evVertexPoint(context, {
                            "vertex" : uvParam.point
                        });

                    if (isQueryEmpty(context, qContainsPoint(definition.face, point)))
                    {
                        throw regenError(ErrorStringEnum.ISOPARAMETRIC_CURVE_POINT_NOT_ON_FACE, uvParam.point);
                    }

                    var distance = evDistance(context, {
                            "side0" : uvParam.point,
                            "side1" : definition.face,
                            "useFaceParameter" : true
                        });
                    const uvParameter = definition.directionType == DirectionType.U_DIRECTION ? distance.sides[1].parameter[0] : distance.sides[1].parameter[1];
                    paramList = append(paramList, uvParameter);
                }
                else
                {
                    paramList = append(paramList, uvParam.uvValue);
                }
            }

            count = size(paramList);
            if (count == 0)
            {
                return;
            }
        }

        var faceCurveCreationType;
        if (definition.directionType == DirectionType.U_DIRECTION)
        {
            faceCurveCreationType = definition.equalSpacing ? FaceCurveCreationType.DIR1_AUTO_SPACED_ISO : FaceCurveCreationType.DIR1_ISO;
        }
        else
        {
            faceCurveCreationType = definition.equalSpacing ? FaceCurveCreationType.DIR2_AUTO_SPACED_ISO : FaceCurveCreationType.DIR2_ISO;
        }

        const curveDef = curveOnFaceDefinition(definition.face, faceCurveCreationType, createCurveNames(count), definition.equalSpacing ? count : paramList);

        var createCurvesOnFaceDefinition = { "curveDefinition" : [curveDef], "showCurves" : true, "useFaceParameter" : true };
        opCreateCurvesOnFace(context, id, createCurvesOnFaceDefinition);
    });

function createCurveNames(count is number) returns array
{
    var curveNames = [];
    for (var i = 0; i < count; i += 1)
    {
        curveNames = append(curveNames, "curve" ~ i);
    }
    return curveNames;
}

