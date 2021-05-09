using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration;
using NSimpleOLAP.Data;
using NSimpleOLAP.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Storage.Molap.Graph
{
  internal enum CoordsCase { Point = 0, PointAll = 1, ALL = 2 }

  /// <summary>
  /// Description of Graph.
  /// </summary>
  internal class Graph<T, U> : IDisposable
    where T : struct, IComparable
    where U : class, ICell<T>
  {
    private MolapKeyHandler<T> _keyHandler;
    private MolapCellValuesHelper<T, U> _cellValueHelper;
    private int _predicateKey;
    private AllKeyComparer<T> _allKeyComparer;

    public Graph(T root, StorageConfig config, MolapCellValuesHelper<T, U> cellValueHelper)
    {
      this.Root = new ImpNode(new KeyValuePair<T, T>[] { new KeyValuePair<T, T>(default(T), root) }) { IsRootDim = true };
      _cellValueHelper = cellValueHelper;
      _keyHandler = new MolapKeyHandler<T>(config.MolapConfig);
      this.Root.Key = _keyHandler.GetKey(this.Root.Coords);
      _allKeyComparer = new AllKeyComparer<T>();
    }

    public Graph(T root, StorageConfig config, MolapCellValuesHelper<T, U> cellValueHelper, int predicateKey) : this(root, config, cellValueHelper)
    {
      _predicateKey = predicateKey;
    }

    #region public members

    public int PredicateKey
    {
      get { return _predicateKey; }
    }

    public Node<T, U> Root
    {
      get;
      private set;
    }

    public void AddRowInfo(MeasureValuesCollection<T> vardata, KeyValuePair<T, T>[] coords)
    {
      CreateNodes(coords, this.Root, null, 0, vardata);
    }

    public IEnumerable<Node<T, U>> GetNodes(KeyValuePair<T, T>[] coords, KeyTuplePairs<T> selectors)
    {
      CoordsCase coordstype = CoordsType(coords, selectors);
      IEnumerable<Node<T, U>> nodes = null;

      switch (coordstype)
      {
        case CoordsCase.Point:
          nodes = GetSingleNode(coords);
          break;

        case CoordsCase.PointAll:
          nodes = GetPointAllNodes(coords, selectors);
          break;

        case CoordsCase.ALL:
          nodes = GetAllNodes(coords);
          break;
      }

      foreach (var item in nodes)
        yield return item;
      // (1.1, 2.0, 3.1) : (1.0.2.0,3.0) -> (1.1,2.n,3.1)
      // (1.0,2.0,3.0)
    }

    public Node<T, U> GetNode(KeyValuePair<T, T>[] coords)
    {
      Node<T, U> currnode = this.Root.GetNode(GetHashPoints(coords));

      return currnode;
    }

    public IEnumerable<Node<T, U>> NodesEnumerator()
    {
      foreach (Node<T, U> item in this.Root.NodesEnumerator())
        yield return item;
    }

    #endregion public members

    #region private methods

    private IEnumerable<Node<T, U>> GetSingleNode(KeyValuePair<T, T>[] coords)
    {
      Node<T, U> currnode = this.Root.GetNode(GetHashPoints(coords));

      if (currnode != null)
        yield return currnode;
    }

    private IEnumerable<Node<T, U>> GetPointAllNodes(KeyValuePair<T, T>[] coords, KeyTuplePairs<T> selectors)
    {
      var selectorList = selectors.Selectors;

      if (selectorList.Length > 0)
      {
        var ncoords = new KeyValuePair<T, T>[] { };

        foreach (var item in TravelNodes(this.Root, coords, 0, selectors, 0))
          yield return item;
      }
    }

    private Node<T, U> FilterNodesShallow(Node<T, U> node, Tuple<KeyValuePair<T, T>, KeyValuePair<T, T>> selector, KeyValuePair<T, T>[] coords, int coordsLastIndex)
    {
      if (!node.IsRootDim && FilterNode(node, new[] { selector.Item1 }))
        return node;

      return null;
    }

    private Node<T, U> FilterNodesExtend(Node<T, U> node, Tuple<KeyValuePair<T, T>, KeyValuePair<T, T>> selector, KeyValuePair<T, T>[] coords, int coordsLastIndex)
    {
      if (!node.IsRootDim && FilterNode(node, new[] { selector.Item1 }))
      {
        var xcoords = coordsLastIndex != 0 ? new KeyValuePair<T, T>[coords.Length] : new KeyValuePair<T, T>[coords.Length + 1];
        var index = node.Coords.Length - 1;

        if (coordsLastIndex == 0)
        {
          Array.Copy(node.Coords, 1, xcoords, 0, node.Coords.Length - 1);
          Array.Copy(coords, 1, xcoords, node.Coords.Length - 1, xcoords.Length - (node.Coords.Length - 1));
        }
        else
        {
          Array.Copy(coords, xcoords, coords.Length);
          xcoords[coordsLastIndex] = node.Coords[node.Coords.Length - 1];
        }

        var xnode = node.GetNode(GetHashPoints(xcoords), index);

        if (xnode != null)
          return xnode;
      }

      return null;
    }

    private IEnumerable<Node<T, U>> GetLeafNodes(Node<T, U> node, Tuple<KeyValuePair<T, T>, KeyValuePair<T, T>> selector, KeyValuePair<T, T>[] coords, int coordsLastIndex)
    {
      if (selector.Item2.IsAll())
      {
        Func<Node<T, U>, Tuple<KeyValuePair<T, T>, KeyValuePair<T, T>>, KeyValuePair<T, T>[], int, Node<T, U>> functor = FilterNodesExtend;

        if (coords.Length == coordsLastIndex + 1)
          functor = FilterNodesShallow;

        foreach (var item in node.Adjacent)
        {
          var xnode = functor(item, selector, coords, coordsLastIndex);

          if (xnode != null)
            yield return xnode;
        }
      }
    }

    private IEnumerable<Node<T, U>> TravelNodes(Node<T, U> node, KeyValuePair<T, T>[] coords, int coordsLastIndex, KeyTuplePairs<T> selectors, int selectorIndex)
    {
      var currSelector = selectors.Selectors[selectorIndex];
      var index = Array.FindIndex(coords, x => currSelector.Item1.Key.Equals(x.Key)
          && currSelector.Item1.Value.Equals(x.Value));
      var length = index == 0 ? 1 : index;
      var ncoords = new KeyValuePair<T, T>[length];

      Array.Copy(coords, ncoords, length);

      var nxnode = FilterNode(node, ncoords) ? node : node.GetNode(GetHashPoints(ncoords));

      if (nxnode == null &&
        index >= node.Coords.Length)
      {
        nxnode = node.GetNode(GetHashPoints(ncoords), node.Coords.Length -1);
      }

      if (nxnode != null)
      {
        if (selectors.Selectors.Length > selectorIndex + 1)
        {
          if (currSelector.Item2.IsAll())
          {
            foreach (var item in nxnode.Adjacent)
            {
              if (!item.IsRootDim && FilterNode(item, new[] { currSelector.Item1 }))
              {
                var nlength = index != 0 ? coords.Length : coords.Length + 1;
                var xcoords = new KeyValuePair<T, T>[nlength];

                if (index == 0)
                {
                  Array.Copy(item.Coords, 1, xcoords, 0, item.Coords.Length - 1);
                  Array.Copy(coords, 1, xcoords, item.Coords.Length - 1, xcoords.Length - (item.Coords.Length - 1));
                }
                else
                {
                  Array.Copy(coords, xcoords, coords.Length);
                  xcoords[index] = item.Coords[item.Coords.Length - 1];
                }

                foreach (var nxitem in TravelNodes(item, xcoords, index, selectors, selectorIndex + 1))
                  yield return nxitem;
              }
            }
          }
        }
        else
        {
          foreach (var item in GetLeafNodes(nxnode, currSelector, coords, index))
            yield return item;
        }
      }
    }

    private IEnumerable<Node<T, U>> GetAllNodes(KeyValuePair<T, T>[] coords)
    {
      Node<T, U> currnode = this.Root.GetNode(GetHashPoints(coords));

      if (currnode == null)
      {
        foreach (var item in currnode.Adjacent)
        {
          if (!item.IsRootDim)
            yield return item;
        }
      }
    }

    private bool FilterNode(Node<T, U> node, KeyValuePair<T, T>[] scoords)
    {
      bool ret = true;

      foreach (var item in scoords)
      {
        int val = Array.BinarySearch(node.Coords, item, _allKeyComparer);

        if (val < 0)
        {
          ret = false;
          break;
        }
      }

      return ret;
    }

    private T[] GetHashPoints(KeyValuePair<T, T>[] coords)
    {
      List<KeyValuePair<T, T>> pairs = new List<KeyValuePair<T, T>>();
      List<T> hslist = new List<T>();

      if (coords.Length > 0 && !coords[0].Equals(this.Root.Coords[0]))
        pairs.Add(this.Root.Coords[0]);

      for (int i = 0; i < coords.Length; i++)
      {
        pairs.Add(coords[i]);
        hslist.Add(_keyHandler.GetKey(pairs.ToArray()));
      }

      return hslist.ToArray();
    }

    private CoordsCase CoordsType(KeyValuePair<T, T>[] coords, KeyTuplePairs<T> selectors)
    {
      KeyValuePair<T, T>[] bcoords = Array.FindAll(coords, x => x.Value.Equals(default(T)));

      if (!selectors.HasSelectors)
        return CoordsCase.Point;
      else if (selectors.HasSelectors && bcoords.Length > selectors.SelectorCount())
        return CoordsCase.PointAll;
      else
        return CoordsCase.PointAll; // redefine this todo
    }

    private void CreateNodes(KeyValuePair<T, T>[] coords, Node<T, U> rootnode, Node<T, U> connode, int index, MeasureValuesCollection<T> vardata)
    {
      for (int i = index; i < coords.Length; i++)
      {
        KeyValuePair<T, T> pair = coords[i];
        Node<T, U> dimnode = CreateNDimNode(rootnode, new KeyValuePair<T, T>(pair.Key, default(T)), vardata);
        Node<T, U> cellnode = null;

        if (connode != null)
          cellnode = CreateNDimNode(connode, pair, vardata);
        else
        {
          cellnode = CreateNDimNode(dimnode, pair, vardata);
          dimnode.InsertNode(cellnode);
        }

        this.CreateNodes(coords, dimnode, cellnode, i + 1, vardata);
      }
    }

    private Node<T, U> CreateNDimNode(Node<T, U> rootnode, KeyValuePair<T, T> pair, MeasureValuesCollection<T> vardata)
    {
      KeyValuePair<T, T>[] coords = Node<T, U>.GetCoords(rootnode.Coords, pair);
      T hashkey = _keyHandler.GetKey(coords);
      Node<T, U> rnode = rootnode.InsertChildNodeIfNotExists(hashkey, coords);
      var context = new GraphCellContext<T>(rnode.Container, rootnode.Container);
      _cellValueHelper.UpdateMeasures(rnode.Container, vardata, context);
      _cellValueHelper.UpdateMetrics(rnode.Container, context);

      return rnode;
    }

    #endregion private methods

    #region Node<T,U> implementation

    private class ImpNode : Node<T, U>
    {
      public ImpNode(KeyValuePair<T, T>[] coords)
      {
        this.Container = (U)(object)(new MolapCell<T>(coords));
        this.Coords = coords;
        this.Adjacent = new NodeCollection<T, U>();
      }

      protected override Node<T, U> Create(T childkey, KeyValuePair<T, T>[] coords)
      {
        ImpNode node = new ImpNode(coords) { Key = childkey, IsRootDim = SetRootDim(coords) };

        return node;
      }

      private bool SetRootDim(KeyValuePair<T, T>[] coords)
      {
        bool ret = false;

        if (coords[coords.Length - 1].Value.Equals(default(T)))
          ret = true;

        return ret;
      }
    }

    #endregion Node<T,U> implementation

    #region IDisposable implementation

    public void Dispose()
    {
      Root.Dispose();
    }

    #endregion IDisposable implementation
  }
}