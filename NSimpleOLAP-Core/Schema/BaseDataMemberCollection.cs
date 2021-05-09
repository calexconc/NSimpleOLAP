using NSimpleOLAP.Schema.Interfaces;
using NSimpleOLAP.Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimpleOLAP.Schema
{
  /// <summary>
  /// Description of BaseDataMemberCollection.
  /// </summary>
  public abstract class BaseDataMemberCollection<T, D> : IDataItemCollection<T, D>
    where T : struct, IComparable
    where D : class, IDataItem<T>
  {
    protected IMemberStorage<T, D> _storage;

    protected void Init()
    {
    }

    public virtual D this[T key]
    {
      get
      {
        return _storage[key];
      }
    }

    public virtual D this[string name]
    {
      get
      {
        return _storage[name];
      }
    }

    public abstract D Next(T key);

    public abstract D Previous(T key);

    public int Count
    {
      get
      {
        return _storage.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return _storage.IsReadOnly;
      }
    }

    public virtual void Add(D item)
    {
      _storage.Add(item);
    }

    public virtual void Clear()
    {
      _storage.Clear();
    }

    public virtual bool Contains(D item)
    {
      return _storage.Contains(item);
    }

    public virtual bool Contains(string item)
    {
      return _storage.Any(x => x.Name.Equals(item));
    }

    public virtual bool ContainsKey(T key)
    {
      return _storage.ContainsKey(key);
    }

    public virtual void CopyTo(D[] array, int arrayIndex)
    {
      _storage.CopyTo(array, arrayIndex);
    }

    public virtual bool Remove(D item)
    {
      return _storage.Remove(item);
    }

    public virtual IEnumerator<D> GetEnumerator()
    {
      foreach (var item in _storage)
        yield return item;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      foreach (var item in _storage)
        yield return item;
    }

    public void Dispose()
    {
      _storage.Dispose();
    }
  }
}