export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/errorstringenum.gen.fs", version : "");
export import(path : "onshape/std/entityinferencetype.gen.fs", version : "");
export import(path : "onshape/std/matepointaxistype.gen.fs", version : "");
export import(path : "onshape/std/origincreationtype.gen.fs", version : "");
export import(path : "onshape/std/rotationtype.gen.fs", version : "");

annotation {"Feature Type Name" : "Mate Point"}
export function matePoint(context is Context, id is Id, matePointDefinition is map)
precondition
{
    if(matePointDefinition.originType != undefined)
    {
        annotation {"Name" : "Origin type"}
        matePointDefinition.originType is OriginCreationType;
    }

    annotation {"Name" : "Origin entity",
                "Filter": (EntityType.EDGE || EntityType.FACE || EntityType.VERTEX) && ConstructionObject.NO,
                "MaxNumberOfPicks" : 1 }
    matePointDefinition.originQuery is Query;

    annotation {"UIHint" : "AlwaysHidden" }
    matePointDefinition.entityInferenceType is EntityInferenceType;

    annotation {"UIHint" : "AlwaysHidden" }
    matePointDefinition.secondaryOriginQuery is Query;

    if (matePointDefinition.originType == OriginCreationType.BETWEEN_ENTITIES)
    {
        if(matePointDefinition.originAdditionalQuery != undefined)
        {
            annotation {"Name" : "Between entity", "Filter" : EntityType.FACE, "MaxNumberOfPicks" : 1 }
            matePointDefinition.originAdditionalQuery is Query;
        }
    }

    if(matePointDefinition.flipPrimary != undefined)
    {
        annotation {"Name" : "Flip primary axis", "UIHint" : "AlwaysHidden"}
        matePointDefinition.flipPrimary is boolean;
    }

    if(matePointDefinition.secondaryAxisType != undefined)
    {
        annotation {"Name" : "Secondary axis type", "UIHint" : "AlwaysHidden", "Default" : MatePointAxisType.PLUS_X}
        matePointDefinition.secondaryAxisType is MatePointAxisType;
    }

    if(matePointDefinition.realign != undefined)
    {
        annotation {"Name" : "Realign"}
        matePointDefinition.realign is boolean;
    }

    if(matePointDefinition.realign == true)
    {
        if(matePointDefinition.primaryAxisQuery != undefined)
        {
            annotation {"Name" : "Primary axis entity",
                        "Filter": EntityType.FACE || EntityType.EDGE,
                        "MaxNumberOfPicks" : 1 }
            matePointDefinition.primaryAxisQuery is Query;
        }

        if(matePointDefinition.secondaryAxisQuery != undefined)
        {
            annotation {"Name" : "Secondary axis entity",
                        "Filter": EntityType.FACE || EntityType.EDGE,
                        "MaxNumberOfPicks" : 1 }
            matePointDefinition.secondaryAxisQuery is Query;
        }
    }

    if(matePointDefinition.transform != undefined)
    {
        annotation {"Name" : "Move"}
        matePointDefinition.transform is boolean;
    }

    if(matePointDefinition.transform == true)
    {
        if(matePointDefinition.translationX != undefined)
        {
            annotation {"Name" : "X translation"}
            isLength(matePointDefinition.translationX, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
        }

        if(matePointDefinition.translationY != undefined)
        {
            annotation {"Name" : "Y translation"}
            isLength(matePointDefinition.translationY, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
        }

        if(matePointDefinition.translationZ != undefined)
        {
            annotation {"Name" : "Z translation"}
            isLength(matePointDefinition.translationZ, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
        }

        if(matePointDefinition.rotationType != undefined)
        {
            annotation {"Name" : "Rotation axis",  "Default" : RotationType.ABOUT_Z}
            matePointDefinition.rotationType is RotationType;
        }

        if(matePointDefinition.rotation != undefined)
        {
            annotation {"Name" : "Rotation angle"}
            isAngle(matePointDefinition.rotation, ANGLE_360_ZERO_DEFAULT_BOUNDS);
        }
    }

    if(matePointDefinition.ownerPart != undefined)
    {
        annotation {"Name" : "Owner part", "Filter" : EntityType.BODY && BodyType.SOLID}
        matePointDefinition.ownerPart is Query;
    }

}
{
  startFeature(context, id, matePointDefinition);
  var matePointTransform = evMatePointTransform(context, matePointDefinition);
  if (matePointTransform.error != undefined)
  {
      reportFeatureError(context, id, matePointTransform.error);
      return;
  }

  var possiblePartOwners = [ matePointDefinition.ownerPart,
                             matePointDefinition.originQuery,
                             matePointDefinition.originAdditionalQuery,
                             matePointDefinition.primaryAxisQuery,
                             matePointDefinition.secondaryAxisQuery ];

  var ownerPartQuery;
  for(var i = 0; i < size(possiblePartOwners); i += 1)
  {
      var currentQuery = qBodyType(qOwnerPart(possiblePartOwners[i]), BodyType.SOLID);
      if (size(evaluateQuery(context, currentQuery)) != 0 )
      {
          ownerPartQuery = currentQuery;
          break;
      }
  }

  if(ownerPartQuery == undefined)
  {
      reportFeatureError(context, id, ErrorStringEnum.MATEPOINT_OWNER_PART_NOT_RESOLVED);
      return;
  }

  opMatePoint(context, id, { "owner" : ownerPartQuery, "transform" : matePointTransform.result});

  endFeature(context, id);
}

