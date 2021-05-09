using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Schema.Interfaces
{
  public interface IDataItemCollection<T, D> : ICollection<D>, IDisposable
      where T : struct, IComparable
    where D : class, IDataItem<T>
  {
    D this[T key] { get; }
    D this[string name] { get; }

    bool ContainsKey(T key);
  }
}