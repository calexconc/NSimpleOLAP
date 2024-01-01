using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Data.Interfaces.Output;

namespace NSimpleOLAP.Data.Output
{
  public class Column<T> : IColumn<T>
    where T : struct, IComparable
  {
    public Column(string label, KeyValuePair<T, T>[] keys, ColumType columnType)
    {
      Label = label;
      ColumnType = columnType;
      Keys = keys;
    }

    public Column(string label, KeyValuePair<T, T>[] keys, ColumType columnType, IColumn<T> parent):this(label, keys, columnType)
    {
      Parent = parent;
    }

    public string Label { get; private set; }

    public IColumn<T> Parent { get; private set; }

    public ColumType ColumnType { get; private set; }

    public KeyValuePair<T, T>[] Keys { get; private set; }
  }
}
