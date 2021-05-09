using System;
using System.Collections.Generic;

namespace NSimpleOLAP
{
  /// <summary>
  /// Description of CubeSet.
  /// </summary>
  public class CubeSet<T> : ICollection<Cube<T>>, IDisposable
    where T : struct, IComparable
  {
    public CubeSet()
    {
    }

    #region ICollection<Cube<T>> implementation

    public int Count
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public bool IsReadOnly
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public void Add(Cube<T> item)
    {
      throw new NotImplementedException();
    }

    public void Clear()
    {
      throw new NotImplementedException();
    }

    public bool Contains(Cube<T> item)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(Cube<T>[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public bool Remove(Cube<T> item)
    {
      throw new NotImplementedException();
    }

    public IEnumerator<Cube<T>> GetEnumerator()
    {
      throw new NotImplementedException();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      throw new NotImplementedException();
    }

    #endregion ICollection<Cube<T>> implementation

    #region IDisposable implementation

    public void Dispose()
    {
      throw new NotImplementedException();
    }

    #endregion IDisposable implementation
  }
}