using NSimpleOLAP.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Storage.Molap.Graph
{
  /// <summary>
  /// Description of Node.
  /// </summary>
  internal abstract class Node<T, U> : IDisposable
    where T : struct, IComparable
    where U : class, ICell<T>
  {
    public T Key
    {
      get;
      set;
    }

    public KeyValuePair<T, T>[] Coords
    {
      get;
      set;
    }

    public bool IsRootDim
    {
      get;
      set;
    }

    public NodeCollection<T, U> Adjacent
    {
      get;
      protected set;
    }

    public U Container
    {
      get;
      protected set;
    }

    protected abstract Node<T, U> Create(T childkey, KeyValuePair<T, T>[] coords);

    public int GetNodeCount()
    {
      int count = 0;

      foreach (Node<T, U> node in this.Adjacent)
      {
        if (this.IsRootDim && this.Coords.Length > 2)
        {
          if (node.IsRootDim)
            count++;
        }
        else
          count++;
      }

      return count;
    }

    public Node<T, U> GetNode(T[] coords)
    {
      return this.GetNode(coords, 0);
    }

    public Node<T, U> GetNode(T[] coords, int index)
    {
      Node<T, U> node = null;
      T key = coords[index];

      if (index < coords.Length - 1 && this.Adjacent.ContainsKey(key))
        node = this.Adjacent[key].GetNode(coords, index + 1);
      else if (coords.Length - 1 == index && this.Adjacent.ContainsKey(key))
        node = this.Adjacent[key];

      return node;
    }

    internal static KeyValuePair<T, T>[] GetCoords(KeyValuePair<T, T>[] pairs, KeyValuePair<T, T> pair)
    {
      List<KeyValuePair<T, T>> values = new List<KeyValuePair<T, T>>(pairs);
      values.Add(pair);

      return values.ToArray();
    }

    internal Node<T, U> InsertChildNodeIfNotExists(T childkey, KeyValuePair<T, T>[] coords)
    {
      Node<T, U> nnode = null;

      if (this.Adjacent.ContainsKey(childkey))
        nnode = this.Adjacent[childkey];
      else
      {
        nnode = this.Create(childkey, coords);
        this.Adjacent.Add(childkey, nnode);
      }

      return nnode;
    }

    internal void InsertNode(Node<T, U> childnode)
    {
      if (!this.Adjacent.ContainsKey(childnode.Key))
        this.Adjacent.Add(childnode.Key, childnode);
    }

    internal IEnumerable<Node<T, U>> NodesEnumerator()
    {
      foreach (Node<T, U> item in this.Adjacent)
      {
        yield return item;

        if (item.IsRootDim && item.Coords.Length > 2)
        {
          foreach (Node<T, U> sitem in item.NodesEnumerator())
          {
            if (item.IsRootDim)
              yield return sitem;
          }
        }
        else
        {
          foreach (Node<T, U> sitem in item.NodesEnumerator())
            yield return sitem;
        }
      }
    }

    #region IDisposable implementation

    public void Dispose()
    {
      this.Adjacent.Dispose();
    }

    #endregion IDisposable implementation
  }
}