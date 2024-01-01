using NSimpleOLAP.CubeExpressions;
using NSimpleOLAP.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Storage.Molap.Graph
{
  internal class GraphCellContext<T> : CellContext<T>
    where T : struct, IComparable
  {
    public GraphCellContext(ICell<T> currentCell, ICell<T> rootCell)
    {
      this.currentCell = currentCell;
      this.rootCell = rootCell;
      previousValues = new Dictionary<T, ValueType>();
      newValues = new Dictionary<T, ValueType>();
    }
  }
}