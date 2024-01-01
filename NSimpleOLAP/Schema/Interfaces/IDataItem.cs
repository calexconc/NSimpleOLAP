using NSimpleOLAP.Common;
using System;

namespace NSimpleOLAP.Schema.Interfaces
{
  /// <summary>
  /// Description of IDataItem.
  /// </summary>
  public interface IDataItem<T>
    where T : struct, IComparable
  {
    string Name { get; set; }
    T ID { get; set; }
    ItemType ItemType { get; }
  }
}