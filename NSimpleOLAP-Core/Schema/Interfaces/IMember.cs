using System;

namespace NSimpleOLAP.Schema.Interfaces
{
  public interface IMember<T> : IDataItem<T>
      where T : struct, IComparable
  {
    string Alias { get; }
  }
}