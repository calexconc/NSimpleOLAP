using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration;
using NSimpleOLAP.Schema.Interfaces;
using System;
using System.Linq.Expressions;

namespace NSimpleOLAP.Schema
{
  /// <summary>
  /// Description of Measure.
  /// </summary>
  public class Measure<T> : IMeasure<T>
    where T : struct, IComparable
  {
    public Measure()
    {
    }

    public Measure(MeasureConfig config)
    {
      this.Config = config;
      this.Init(this.Config);
    }

    public Type DataType
    {
      get;
      set;
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
      get { return ItemType.Measure; }
    }

    public MeasureConfig Config
    {
      get;
      set;
    }

    public Expression MergeFunction { get; set; }

    #region private members

    private void Init(MeasureConfig config)
    {
      this.DataType = config.DataType;
      this.Name = config.Name;
      this.MergeFunction = config.MergeFunction;
    }

    #endregion private members
  }
}