using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Interfaces
{
  /// <summary>
  /// Description of ICellCollection.
  /// </summary>
  public interface ICellCollection<T, U> : ICollection<U>
    where T : struct, IComparable
    where U : class, ICell<T>
  {
    U this[KeyValuePair<T, T>[] keys] { get; }

    bool ContainsKey(KeyValuePair<T, T>[] keys);
  }
}