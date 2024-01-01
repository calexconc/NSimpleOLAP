using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Data.Output;

namespace NSimpleOLAP.Data.Interfaces.Output
{
  public interface IRow<T>
    where T : struct, IComparable
  {
    IRowCaption<T> Caption { get; }

    IEnumerable<IRowCell<T>> Cells { get; }

    RowType RowType { get; }

    IRowBranch<T> Parent { get; }

    int CellCount { get; }

    KeyValuePair<T, T>[] Keys { get; }
  }
}
