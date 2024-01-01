using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSimpleOLAP.Data.Interfaces.Output
{
  public interface IColumnStack<T>
    where T : struct, IComparable
  {
    IEnumerable<IColumn<T>> Children { get; }
  }
}
