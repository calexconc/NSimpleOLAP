using System;
using System.Collections.Generic;
using NSimpleOLAP.Triggers.Interfaces;

namespace NSimpleOLAP.Interfaces
{
  /// <summary>
  /// Description of ICell.
  /// </summary>
  public interface ICell<T>
    where T : struct, IComparable
  {
    KeyValuePair<T, T>[] Coords { get; }

    uint Occurrences { get; }

    IValueCollection<T> Values { get; }

    IList<ITrigger<T>> Triggers { get; }
  }
}