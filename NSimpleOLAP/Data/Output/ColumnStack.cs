using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Data.Interfaces.Output;

namespace NSimpleOLAP.Data.Output
{
  public class ColumnStack<T> : IColumnStack<T>, IColumn<T>
    where T : struct, IComparable
  {
    private List<IColumn<T>> _children;

    public ColumnStack(string label, KeyValuePair<T, T>[] keys)
    {
      Label = label;
      Keys = keys;
      _children = new List<IColumn<T>>();
    }

    public ColumnStack(string label, KeyValuePair<T, T>[] keys, IEnumerable<IColumn<T>> columns):this(label, keys)
    {
      Label = label;
      _children = new List<IColumn<T>>(columns);
    }

    public string Label { get; private set; }

    public IEnumerable<IColumn<T>> Children { get { return _children; } }

    public IColumn<T> Parent { get; private set; }

    public ColumType ColumnType { get { return ColumType.COLUMN_STACK; } }

    public KeyValuePair<T, T>[] Keys { get; private set; }

    internal void AddColumn(IColumn<T> column)
    {
      _children.Add(column);
    }
  }
}
