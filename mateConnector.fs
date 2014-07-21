export import(path : "onshape/std/geomUtils.fs", version : "");
export import(path : "onshape/std/evaluate.fs", version : "");
export import(path : "onshape/std/errorstringenum.gen.fs", version : "");
export import(path : "onshape/std/entityinferencetype.gen.fs", version : "");
export import(path : "onshape/std/mateconnectoraxistype.gen.fs", version : "");
export import(path : "onshape/std/origincreationtype.gen.fs", version : "");
export import(path : "onshape/std/rotationtype.gen.fs", version : "");

annotation {"Feature Type Name" : "Mate Connector"}
export function mateConnector(context is Context, id is Id, mateConnectorDefinition is map)
precondition
{
    if(mateConnectorDefinition.originType != undefined)
    {
        annotation {"Name" : "Origin type"}
        mateConnectorDefinition.originType is OriginCreationType;
    }

    annotation {"Name" : "Origin entity",
                "Filter": (EntityType.EDGE || EntityType.FACE || EntityType.VERTEX) && ConstructionObject.NO,
                "MaxNumberOfPicks" : 1 }
    mateConnectorDefinition.originQuery is Query;

    annotation {"UIHint" : "AlwaysHidden" }
    mateConnectorDefinition.entityInferenceType is EntityInferenceType;

    annotation {"UIHint" : "AlwaysHidden" }
    mateConnectorDefinition.secondaryOriginQuery is Query;

    if (mateConnectorDefinition.originType == OriginCreationType.BETWEEN_ENTITIES)
    {
        if(mateConnectorDefinition.originAdditionalQuery != undefined)
        {
            annotation {"Name" : "Between entity", "Filter" : EntityType.FACE, "MaxNumberOfPicks" : 1 }
            mateConnectorDefinition.originAdditionalQuery is Query;
        }
    }

    if(mateConnectorDefinition.flipPrimary != undefined)
    {
        annotation {"Name" : "Flip primary axis", "UIHint" : "AlwaysHidden"}
        mateConnectorDefinition.flipPrimary is boolean;
    }

    if(mateConnectorDefinition.secondaryAxisType != undefined)
    {
        annotation {"Name" : "Secondary axis type", "UIHint" : "AlwaysHidden", "Default" : MateConnectorAxisType.PLUS_X}
        mateConnectorDefinition.secondaryAxisType is MateConnectorAxisType;
    }

    if(mateConnectorDefinition.realign != undefined)
    {
        annotation {"Name" : "Realign"}
        mateConnectorDefinition.realign is boolean;
    }

    if(mateConnectorDefinition.realign == true)
    {
        if(mateConnectorDefinition.primaryAxisQuery != undefined)
        {
            annotation {"Name" : "Primary axis entity",
                        "Filter": EntityType.FACE || EntityType.EDGE,
                        "MaxNumberOfPicks" : 1 }
            mateConnectorDefinition.primaryAxisQuery is Query;
        }

        if(mateConnectorDefinition.secondaryAxisQuery != undefined)
        {
            annotation {"Name" : "Secondary axis entity",
                        "Filter": EntityType.FACE || EntityType.EDGE,
                        "MaxNumberOfPicks" : 1 }
            mateConnectorDefinition.secondaryAxisQuery is Query;
        }
    }

    if(mateConnectorDefinition.transform != undefined)
    {
        annotation {"Name" : "Move"}
        mateConnectorDefinition.transform is boolean;
    }

    if(mateConnectorDefinition.transform == true)
    {
        if(mateConnectorDefinition.translationX != undefined)
        {
            annotation {"Name" : "X translation"}
            isLength(mateConnectorDefinition.translationX, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
        }

        if(mateConnectorDefinition.translationY != undefined)
        {
            annotation {"Name" : "Y translation"}
            isLength(mateConnectorDefinition.translationY, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
        }

        if(mateConnectorDefinition.translationZ != undefined)
        {
            annotation {"Name" : "Z translation"}
            isLength(mateConnectorDefinition.translationZ, NONNEGATIVE_ZERO_DEFAULT_LENGTH_BOUNDS);
        }

        if(mateConnectorDefinition.rotationType != undefined)
        {
            annotation {"Name" : "Rotation axis",  "Default" : RotationType.ABOUT_Z}
            mateConnectorDefinition.rotationType is RotationType;
        }

        if(mateConnectorDefinition.rotation != undefined)
        {
            annotation {"Name" : "Rotation angle"}
            isAngle(mateConnectorDefinition.rotation, ANGLE_360_ZERO_DEFAULT_BOUNDS);
        }
    }

    if(mateConnectorDefinition.ownerPart != undefined)
    {
        annotation {"Name" : "Owner part", "Filter" : EntityType.BODY && BodyType.SOLID}
        mateConnectorDefinition.ownerPart is Query;
    }

}
{
  startFeature(context, id, mateConnectorDefinition);
  var mateConnectorTransform = evMateConnectorTransform(context, mateConnectorDefinition);
  if (mateConnectorTransform.error != undefined)
  {
      reportFeatureError(context, id, mateConnectorTransform.error);
      return;
  }

  var onlyPartInStudio = qNothing();
  var allBodies = qEverything(EntityType.BODY);
  var allParts = qBodyType(allBodies, BodyType.SOLID);

  if (@size(evaluateQuery(context, allParts)) == 1)
  {
    onlyPartInStudio = allParts;
  }

  var possiblePartOwners = [ mateConnectorDefinition.ownerPart,
                             mateConnectorDefinition.originQuery,
                             mateConnectorDefinition.originAdditionalQuery,
                             mateConnectorDefinition.primaryAxisQuery,
                             mateConnectorDefinition.secondaryAxisQuery,
                             onlyPartInStudio ];

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
      reportFeatureError(context, id, ErrorStringEnum.MATECONNECTOR_OWNER_PART_NOT_RESOLVED);
      return;
  }

  opMateConnector(context, id, { "owner" : ownerPartQuery, "transform" : mateConnectorTransform.result});

  endFeature(context, id);
}

