using NSimpleOLAP.Data;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Storage.Interfaces
{
  public interface IFactRow<T>
    where T : struct, IComparable
  {
    int Index { get; }

    T HashCode { get; }

    KeyValuePair<T, T>[] Pairs { get; }

    MeasureValuesCollection<T> Data { get; }
  }
}