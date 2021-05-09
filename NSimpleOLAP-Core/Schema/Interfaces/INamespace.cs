using NSimpleOLAP.Common;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Schema.Interfaces
{
  /// <summary>
  /// Description of INamespace.
  /// </summary>
  public interface INamespace<T> : ICollection<IDataItem<T>>, IDisposable
    where T : struct, IComparable
  {
    IDataItem<T> this[T key] { get; }
    IDataItem<T> this[string name] { get; }

    bool HasEntity(string memberKey);

    bool HasEntity(T memberKey);

    void Clear(ItemType type);
  }
}