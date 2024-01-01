using NSimpleOLAP.Configuration;
using System;
using System.Linq.Expressions;

namespace NSimpleOLAP.Schema.Interfaces
{
  public interface IMeasure<T> : IDataItem<T>
      where T : struct, IComparable
  {
    Type DataType { get; set; }
    MeasureConfig Config { get; set; }
    Expression MergeFunction { get; set; }
  }
}