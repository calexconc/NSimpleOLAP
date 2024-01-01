using NSimpleOLAP.Common.Collections;
using NSimpleOLAP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimpleOLAP.Storage.Molap.Graph
{
  /// <summary>
  /// Description of NodeCollection.
  /// </summary>
  internal class NodeCollection<T, U> : ICollection<Node<T, U>>, IDisposable
    where T : struct, IComparable
    where U : class, ICell<T>
  {
    private TSDictionary<T, Node<T, U>> _innerdict;

    public NodeCollection()
    {
      _innerdict = new TSDictionary<T, Node<T, U>>();
    }

    public Node<T, U> this[T key]
    {
      get
      {
        return _innerdict[key];
      }
    }

    public bool ContainsKey(T key)
    {
      return _innerdict.ContainsKey(key);
    }

    public void Add(T key, Node<T, U> item)
    {
      _innerdict.Add(key, item);
    }

    #region

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

    public void Add(Node<T, U> item)
    {
      _innerdict.Add(item.Key, item);
    }

    public void Clear()
    {
      _innerdict.Clear();
    }

    public bool Contains(Node<T, U> item)
    {
      return _innerdict.Contains(new KeyValuePair<T, Node<T, U>>(item.Key, item));
    }

    public void CopyTo(Node<T, U>[] array, int arrayIndex)
    {
      this._innerdict.ToArray().CopyTo(array, arrayIndex);
    }

    public bool Remove(Node<T, U> item)
    {
      return _innerdict.Remove(item.Key);
    }

    public IEnumerator<Node<T, U>> GetEnumerator()
    {
      foreach (var item in _innerdict.Values)
        yield return item;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      foreach (var item in _innerdict.Values)
        yield return item;
    }

    #endregion

    #region IDisposable implementation

    public void Dispose()
    {
      foreach (var item in _innerdict.Values)
        item.Dispose();

      _innerdict.Dispose();
    }

    #endregion
  }
}