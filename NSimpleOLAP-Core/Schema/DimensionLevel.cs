using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration;
using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.Common.Interfaces;
using NSimpleOLAP.Data.Interfaces;
using NSimpleOLAP.Data.Readers;
using NSimpleOLAP.Schema.Interfaces;
using NSimpleOLAP.Storage.Interfaces;

namespace NSimpleOLAP.Schema
{
  public class DimensionLevel<T> : Dimension<T>
    where T : struct, IComparable
  {
    private int _level;

    public DimensionLevel(DimensionConfig dimconfig, int levelIndex, IDataSource datasource)
    {
      typeOf = DimensionType.Levels;
      this.Config = dimconfig;
      _level = levelIndex;
      dataSource = datasource;
      LevelDimensions = new List<DimensionLevel<T>>();
    }

    public new int LevelPosition { get { return _level; } }

    public new bool HasLevels { get { return LevelDimensions.Count > 0; } }

    public IList<DimensionLevel<T>> LevelDimensions
    {
      get;
      private set;
    }

    public override void Process()
    {
      using (AbsReader reader = this.DataSource.GetReader())
      {
        while (reader.Next())
        {
          var member = new MemberLevel<T>()
          {
            ID = (T)reader.Current[this.Config.ValueFieldName],
            Name = reader.Current[this.Config.DesFieldName].ToString()
          };

          foreach (var dim in LevelDimensions)
          {
            var column = reader.Current[dim.Config.DesFieldName].ToString();
            var value = (T)reader.Current[dim.Config.ValueFieldName];

            member.Levels.Add(dim.Name, value);
          }

          if (!Members.ContainsKey(member.ID))
          {
            this.Members.Add(member);
          }
        }
      }
    }

    internal override void SetMembersStorage(IMemberStorage<T, Member<T>> storage)
    {
      members = new MemberCollection<T>(storage); //todo change this
    }
  }
}
