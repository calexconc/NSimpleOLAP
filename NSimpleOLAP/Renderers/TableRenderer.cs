using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Common;
using NSimpleOLAP.Query.Interfaces;
using NSimpleOLAP.Data.Output;
using NSimpleOLAP.Data.Interfaces.Output;

namespace NSimpleOLAP.Renderers
{
  internal class TableRenderer<T>
    where T : struct, IComparable
  {
    public ITable<T> Render(IEnumerable<IOutputCell<T>[]> rows)
    {
      if (rows == null)
        return null;

      var table = new Table<T>();

     // table.
      return table;
    }

    private IEnumerable<IColumn<T>> CreateHeader(List<IOutputCell<T>[]> rows)
    {
      return null;
    }

    private IEnumerable<IRow<T>> CreateRowDataCells(IOutputCell<T>[] rows)
    {

      return null;
    }
  }
}
