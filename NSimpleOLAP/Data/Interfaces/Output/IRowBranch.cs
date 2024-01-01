using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSimpleOLAP.Data.Interfaces.Output
{
  public interface IRowBranch<T>
    where T : struct, IComparable
  {
    IRowBranch<T> Parent { get; }

    IRowCaption<T> Caption { get; }

    IEnumerable<IRow<T>> Rows { get; }

    KeyValuePair<T, T>[] Keys { get; }
  }
}
