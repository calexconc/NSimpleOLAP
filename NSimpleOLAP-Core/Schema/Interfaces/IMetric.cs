using NSimpleOLAP.Configuration;
using NSimpleOLAP.CubeExpressions;
using System;

namespace NSimpleOLAP.Schema.Interfaces
{
  public interface IMetric<T> : IDataItem<T>
      where T : struct, IComparable
  {
    MetricsExpression<T> MetricExpression { get; set; }
    MetricConfig Config { get; set; }
    Type DataType { get; set; }
  }
}