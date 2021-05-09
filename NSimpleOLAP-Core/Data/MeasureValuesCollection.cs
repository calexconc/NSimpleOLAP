using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimpleOLAP.Data
{
  /// <summary>
  /// Description of MeasuresCollection.
  /// </summary>
  public class MeasureValuesCollection<T> : IDictionary<T, object>
    where T : struct, IComparable
  {
    private Dictionary<T, object> _innerdict;

    public MeasureValuesCollection()
    {
      _innerdict = new Dictionary<T, object>();
    }

    public object this[T key]
    {
      get
      {
        return _innerdict[key];
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public ICollection<T> Keys
    {
      get
      {
        return _innerdict.Keys;
      }
    }

    public ICollection<object> Values
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

    public void Add(T key, object value)
    {
      _innerdict.Add(key, value);
    }

    public bool Remove(T key)
    {
      throw new NotImplementedException();
    }

    public bool TryGetValue(T key, out object value)
    {
      return _innerdict.TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<T, object> item)
    {
      _innerdict.Add(item.Key, item.Value);
    }

    public void Clear()
    {
      _innerdict.Clear();
    }

    public bool Contains(KeyValuePair<T, object> item)
    {
      return _innerdict.ContainsKey(item.Key);
    }

    public void CopyTo(KeyValuePair<T, object>[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<T, object> item)
    {
      throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<T, object>> GetEnumerator()
    {
      foreach (var item in _innerdict)
        yield return item;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      foreach (var item in _innerdict)
        yield return item;
    }

    public override bool Equals(object obj)
    {
      var values = (MeasureValuesCollection<T>)obj;

      return !this.Except(values).Any();
    }

    public override int GetHashCode()
    {
      var result = 0;

      foreach (var item in this)
      {
        result ^= item.Key.GetHashCode();
        result ^= item.Value.GetHashCode();
      }

      return result;
    }
  }
}