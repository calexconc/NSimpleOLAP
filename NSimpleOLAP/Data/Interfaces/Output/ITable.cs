using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Data.Interfaces.Output
{
  public interface ITable<T> where T : struct, IComparable
  {
    IRow<T> this[int index] { get; }
    IColumn<T> this[string key] { get; }

    IEnumerable<IColumn<T>> Columns { get; }
    IEnumerable<IRow<T>> Rows { get; }
  }
}