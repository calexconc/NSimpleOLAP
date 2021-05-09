using NSimpleOLAP.Common.Collections;
using NSimpleOLAP.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Storage.Molap
{
  /// <summary>
  /// Description of VarsCollection.
  /// </summary>
  public class MolapValuesCollection<T> : IValueCollection<T>
    where T : struct, IComparable
  {
    private TSDictionary<T, ValueType> _innerdict;

    public MolapValuesCollection()
    {
      _innerdict = new TSDictionary<T, ValueType>();
    }

    public ValueType this[T key]
    {
      get
      {
        return _innerdict[key];
      }
      set
      {
        _innerdict[key] = value;
      }
    }

    public ICollection<T> Keys
    {
      get
      {
        return _innerdict.Keys;
      }
    }

    public ICollection<ValueType> Values
    {
      get
      {
        return _innerdict.Values;
      }
    }

    public int Count
    {
      get
      {
        return _innerdict.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public bool ContainsKey(T key)
    {
      return _innerdict.ContainsKey(key);
    }

    public void Add(T key, ValueType value)
    {
      _innerdict.Add(key, value);
    }

    public bool Remove(T key)
    {
      return _innerdict.Remove(key);
    }

    public bool TryGetValue(T key, out ValueType value)
    {
      return _innerdict.TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<T, ValueType> item)
    {
      _innerdict.Add(item.Key, item.Value);
    }

    public void Clear()
    {
      _innerdict.Clear();
    }

    public bool Contains(KeyValuePair<T, ValueType> item)
    {
      return _innerdict.ContainsKey(item.Key);
    }

    public void CopyTo(KeyValuePair<T, ValueType>[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<T, ValueType> item)
    {
      return _innerdict.Remove(item.Key);
    }

    public IEnumerator<KeyValuePair<T, ValueType>> GetEnumerator()
    {
      foreach (var item in _innerdict)
        yield return item;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      foreach (var item in _innerdict)
        yield return item;
    }
  }
}