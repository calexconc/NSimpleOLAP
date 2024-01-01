using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Data.Interfaces.Output;
using NSimpleOLAP.Query.Layout;

namespace NSimpleOLAP.Data.Output
{
  public class Table<T> : ITable<T> 
    where T : struct, IComparable
  {
    private List<IColumn<T>> _columns;

    private List<IRow<T>> _rows;

    public Table()
    {
      _columns = new List<IColumn<T>>();
      _rows = new List<IRow<T>>();
    }

    public IRow<T> this[int index]
    {
      get { return _rows[index]; }
    }

    public IColumn<T> this[string key]
    {
      get { return null; }
    }

    public IEnumerable<IColumn<T>> Columns { get { return _columns; } }

    public IEnumerable<IRow<T>> Rows { get { return _rows; } }


    internal void AddColumn(KeyValuePair<T, T>[] coords, KeyValuePair<string, string>[] descriptors)
    {

    }

    internal void AddRow(IRowCaption<T> caption, OutputCell<T>[] cells)
    {

    }
  }
}
