using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NSimpleOLAP.Common.Interfaces;

namespace NSimpleOLAP.Configuration
{
  public abstract class AbstractConfigCollection<T> : IConfigCollection<T>
  {
    protected Dictionary<string, int> indexer;

    protected List<T> elements;

    #region Properties

    /// <summary>
    /// Retrieve and item in the collection by index.
    /// </summary>
    public T this[int index]
    {
      get
      {
        return elements[index];
      }
      set
      {
        elements[index] = value;
      }
    }

    public T this[string name]
    {
      get
      {
        return elements[indexer[name]];
      }
      set
      {
        elements[indexer[name]] = value;
      }
    }

    public int Count => elements.Count;

    public bool IsReadOnly => false;

    #endregion Properties

    /// <summary>
    /// Adds a CubeElement to the configuration file.
    /// </summary>
    /// <param name="element">The CubeElement to add.</param>
    public abstract void Add(T element);

    public void Clear()
    {
      elements.Clear();
      indexer.Clear();
    }

    public bool Contains(CubeConfig item)
    {
      return indexer.ContainsKey(item.Name);
    }

    public bool Contains(T item)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(CubeConfig[] array, int arrayIndex)
    {
      elements.ToArray().CopyTo(array, arrayIndex);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator()
    {
      foreach (var item in elements)
        yield return item;
    }

    /// <summary>
    /// Removes a CubeElement with the given name.
    /// </summary>
    /// <param name="name">The name of the CubeElement to remove.</param>
    public abstract void Remove(string name);

    public abstract bool Remove(T item);

    IEnumerator IEnumerable.GetEnumerator()
    {
      foreach (var item in elements)
        yield return item;
    }
  }
}
