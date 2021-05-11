using NSimpleOLAP.Common;
using NSimpleOLAP.Common.Interfaces;
using NSimpleOLAP.Configuration;
using NSimpleOLAP.Data.Interfaces;
using NSimpleOLAP.Data.Readers;
using NSimpleOLAP.Schema.Interfaces;
using NSimpleOLAP.Storage.Interfaces;
using System;

namespace NSimpleOLAP.Schema
{
  /// <summary>
  /// Description of Dimension.
  /// </summary>
  public class Dimension<T> : IDimension<T>, IProcess
    where T : struct, IComparable
  {
    protected DimensionType typeOf;

    protected MemberCollection<T> members;

    protected IDataSource dataSource;

    public Dimension()
    {
      typeOf = DimensionType.Numeric;
    }

    public Dimension(DimensionConfig dimconfig, IDataSource datasource):this()
    {
      this.Config = dimconfig;
      dataSource = datasource;
    }

    public string Name
    {
      get;
      set;
    }

    public T ID
    {
      get;
      set;
    }

    public ItemType ItemType
    {
      get { return ItemType.Dimension; }
    }

    public virtual DimensionType TypeOf { get { return typeOf; } }

    public MemberCollection<T> Members
    {
      get { return members; }
    }

    public DimensionConfig Config
    {
      get;
      set;
    }

    public IDataSource DataSource
    {
      get 
      {
        return dataSource;
      }
    }

    public virtual bool HasLevels { get { return false; } }

    public virtual int LevelPosition { get { return 0; } }

    public virtual void Process()
    {
      using (AbsReader reader = this.DataSource.GetReader())
      {
        if (!Config.SourceIsGenerated)
        {
          while (reader.Next())
          {
            this.Members.Add(new Member<T>()
            {
              ID = (T)reader.Current[this.Config.ValueFieldName],
              Name = reader.Current[this.Config.DesFieldName].ToString()
            });
          }
        }
        else
        {
          while (reader.Next())
          {
            this.Members.Add(new MemberGenerated<T>((IDataTransformer)reader.Current["Transformer"])
            {
              ID = (T)reader.Current["Value"],
              Name = reader.Current["Name"].ToString()
            });
          }
        }
      }
    }

    public virtual void Refresh(bool all)
    {
      throw new NotImplementedException();
    }

    #region

    internal virtual void SetMembersStorage(IMemberStorage<T, Member<T>> storage)
    {
      members = new MemberCollection<T>(storage);
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
      this.Members.Dispose();
    }

    #endregion
  }
}