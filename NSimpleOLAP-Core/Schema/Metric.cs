using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration;
using NSimpleOLAP.CubeExpressions;
using NSimpleOLAP.Schema.Interfaces;
using System;

namespace NSimpleOLAP.Schema
{
  /// <summary>
  /// Description of Metric.
  /// </summary>
  public class Metric<T> : IMetric<T>
    where T : struct, IComparable
  {
    public Metric()
    {
    }

    public Metric(MetricConfig config)
    {
      this.Config = config;
      this.Init(this.Config);
    }

    public MetricsExpression<T> MetricExpression
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
      get { return ItemType.Metric; }
    }

    public MetricConfig Config
    {
      get;
      set;
    }

    public Type DataType { get; set; }

    #region private members

    private void Init(MetricConfig config)
    {
      this.DataType = config.DataType;
      this.Name = config.Name;

      try
      {
        // todo parser for expressions
        // this.MetricExpression = (MetricsExpression<T>)config.MetricFunction;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        throw;
        // todo extra error handling
      }
    }

    #endregion private members
  }
}