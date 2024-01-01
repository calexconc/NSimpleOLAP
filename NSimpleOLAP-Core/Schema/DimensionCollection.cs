using NSimpleOLAP.Storage.Interfaces;
using System;

namespace NSimpleOLAP.Schema
{
  /// <summary>
  /// Description of DimensionsCollection.
  /// </summary>
  public class DimensionCollection<T> : BaseDataMemberCollection<T, Dimension<T>>
    where T : struct, IComparable
  {
    public DimensionCollection(IMemberStorage<T, Dimension<T>> storage)
    {
      _storage = storage;
      base.Init();
    }

    public override Dimension<T> Next(T key)
    {
      throw new NotImplementedException();
    }

    public override Dimension<T> Previous(T key)
    {
      throw new NotImplementedException();
    }
  }
}