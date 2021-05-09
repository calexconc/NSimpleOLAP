using NSimpleOLAP.Interfaces;
using NSimpleOLAP.Storage.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP
{
  /// <summary>
  /// Description of CellCollection.
  /// </summary>
  public class CellCollection<T> : ICellCollection<T, Cell<T>>
    where T : struct, IComparable
  {
    private IStorage<T, Cell<T>> _storage;

    public CellCollection(IStorage<T, Cell<T>> storage)
    {
      _storage = storage;
    }

    public Cell<T> this[KeyValuePair<T, T>[] keys]
    {
      get
      {
        return _storage.GetCell(keys);
      }
    }

    public int Count
    {
      get
      {
        return _storage.GetCellCount();
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return true;
      }
    }

    public bool ContainsKey(KeyValuePair<T, T>[] keys)
    {
      Cell<T> cell = _storage.GetCell(keys);

      if (cell != null)
        return true;
      else
        return false;
    }

    public void Add(Cell<T> item)
    {
      throw new NotImplementedException();
    }

    public void Clear()
    {
      throw new NotImplementedException();
    }

    public bool Contains(Cell<T> item)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(Cell<T>[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public bool Remove(Cell<T> item)
    {
      throw new NotImplementedException();
    }

    public IEnumerator<Cell<T>> GetEnumerator()
    {
      foreach (Cell<T> item in _storage.CellEnumerator())
        yield return item;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      foreach (Cell<T> item in _storage.CellEnumerator())
        yield return item;
    }
  }
}