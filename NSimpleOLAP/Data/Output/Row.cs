using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Data.Interfaces.Output;

namespace NSimpleOLAP.Data.Output
{
  public class Row<T> : IRow<T>
    where T : struct, IComparable
  {
    private IRowCell<T>[] _rowCells;

    public Row(RowType rowType, KeyValuePair<T, T>[] keys, IRowCaption<T> caption, IRowCell<T>[] rowCells)
    {
      RowType = rowType;
      Caption = caption;
      Keys = keys;
      _rowCells = rowCells;
    }

    public IRowCaption<T> Caption { get; private set; }

    public IEnumerable<IRowCell<T>> Cells { get { return _rowCells; } }

    public RowType RowType { get; private set; }

    public IRowBranch<T> Parent => throw new NotImplementedException();

    public int CellCount { get { return _rowCells.Length; } }

    public KeyValuePair<T, T>[] Keys { get; private set; }
  }
}
