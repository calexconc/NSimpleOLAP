using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Data.Output;

namespace NSimpleOLAP.Data.Interfaces.Output
{
  public interface IColumn<T>
    where T : struct, IComparable
  {
    string Label { get; }

    IColumn<T> Parent { get; }

    ColumType ColumnType { get; }

    KeyValuePair<T, T>[] Keys { get; }
  }
}
