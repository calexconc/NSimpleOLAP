using NSimpleOLAP.Common;
using NSimpleOLAP.Common.Collections;
using NSimpleOLAP.Schema.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimpleOLAP.Storage.Molap
{
  /// <summary>
  /// Description of AbsMolapNameSpace.
  /// </summary>
  public abstract class AbsMolapNameSpace<T> : INamespace<T>
    where T : struct, IComparable
  {
    private TSDictionary<T, IDataItem<T>> _items;
    private TSDictionary<string, T> _index;
    protected AbsIdentityKey<T> _keybuilder;

    protected void Init()
    {
      _index = new TSDictionary<string, T>();
      _items = new TSDictionary<T, IDataItem<T>>();
    }

    #region INamespace<T> implementation

    public IDataItem<T> this[T key]
    {
      get
      {
        return _items[key];
      }
    }

    public IDataItem<T> this[string name]
    {
      get
      {
        return _items[_index[name]];
      }
    }

    public int Count
    {
      get
      {
        return _items.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public void Add(IDataItem<T> item)
    {
      if (item.ID.Equals(default(T)))
        item.ID = _keybuilder.GetNextKey();

      _items.Add(item.ID, item);
      _index.Add(item.Name, item.ID);
    }

    public void Clear()
    {
      _items.Clear();
      _index.Clear();
    }

    public bool Contains(IDataItem<T> item)
    {
      return _items.ContainsKey(item.ID) || _index.ContainsKey(item.Name);
    }

    public void CopyTo(IDataItem<T>[] array, int arrayIndex)
    {
      _items.Values.CopyTo(array, arrayIndex);
    }

    public bool Remove(IDataItem<T> item)
    {
      return _items.Remove(item.ID) && _index.Remove(item.Name);
    }

    public IEnumerator<IDataItem<T>> GetEnumerator()
    {
      foreach (var item in _items.Values)
        yield return item;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      foreach (var item in _items.Values)
        yield return item;
    }

    public void Dispose()
    {
      _items.Dispose();
      _index.Dispose();
    }

    public void Clear(ItemType type)
    {
      IEnumerable<IDataItem<T>> members =
        from item in _items.Values
        where item.ItemType == type
        select item;
      IDataItem<T>[] array = members.ToArray();

      foreach (var member in array)
        this.Remove(member);
    }

    #endregion INamespace<T> implementation

    public bool HasEntity(string memberKey)
    {
      return _index.ContainsKey(memberKey);
    }

    public bool HasEntity(T memberKey)
    {
      return _items.ContainsKey(memberKey);
    }
  }
}