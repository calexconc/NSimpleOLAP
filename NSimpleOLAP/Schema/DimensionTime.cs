using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration;
using System;
using System.Collections.Generic;
using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.Common.Interfaces;
using NSimpleOLAP.Data.Interfaces;
using NSimpleOLAP.Data.Readers;
using NSimpleOLAP.Schema.Interfaces;
using NSimpleOLAP.Storage.Interfaces;

namespace NSimpleOLAP.Schema
{
  public class DimensionTime<T> : Dimension<T>
    where T : struct, IComparable
  {
    private int _level;
    // private MemberDateTimeCollection<T> _members;

    public DimensionTime(DimensionConfig dimconfig, TimeLevels level, int levelIndex)
    {
      typeOf = DimensionType.Time;
      this.Config = dimconfig;
      TimeLevel = level;
      _level = levelIndex;
      LevelDimensions = new List<DimensionTime<T>>();
    }

    public TimeLevels TimeLevel { get; private set; }

    public new int LevelPosition { get { return _level; } }

    public new bool HasLevels { get { return LevelDimensions.Count > 0; } }

    public IList<DimensionTime<T>> LevelDimensions
    {
      get;
      private set;
    }

    public override void Process()
    {
      foreach (var item in PrePopulate())
      {
        this.Members.Add(new Member<T>()
        {
          ID = item.Item1,
          Name = item.Item2
        });
      }
    }

    internal override void SetMembersStorage(IMemberStorage<T, Member<T>> storage)
    {
      members = new MemberCollection<T>(storage); //todo change this
    }

    private IEnumerable<Tuple<T, string>> PrePopulate()
    {
      var tempDate = DateTime.Now;

      switch (TimeLevel)
      {
        case TimeLevels.HOUR:
          return DateTimeMemberGenerator.GetAllHours<T>();
        case TimeLevels.MINUTES:
          return DateTimeMemberGenerator.GetAllMinutes<T>();
        case TimeLevels.SECONDS:
          return DateTimeMemberGenerator.GetAllSeconds<T>();
        default:
          return new Tuple<T, string>[] { };
      }
    }


    // Change Members
  }
}
