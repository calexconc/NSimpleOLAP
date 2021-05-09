using NSimpleOLAP.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.CubeExpressions.Interfaces
{
  public interface IExpressionContext<T, U>
    where T : struct, IComparable
    where U : class, ICell<T>
  {
    U CurrentCell { get; }

    U RootCell { get; }

    object Result { get; set; }

    T CurrentMetric { get; set; }

    IDictionary<T, ValueType> PreviousValues { get; }

    IDictionary<T, ValueType> NewValues { get; }
  }
}