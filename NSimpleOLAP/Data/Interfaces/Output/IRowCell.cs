using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSimpleOLAP.Data.Interfaces.Output
{
  public interface IRowCell<T>
    where T : struct, IComparable
  {
    IColumn<T> Column { get; }

    ValueType this[string label] { get; }

    IEnumerable<ValueType> Values { get; }

    IEnumerable<string> Labels { get; }
  }
}
