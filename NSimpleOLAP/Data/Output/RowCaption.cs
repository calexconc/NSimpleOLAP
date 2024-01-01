using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Data.Interfaces.Output;

namespace NSimpleOLAP.Data.Output
{
  public class RowCaption<T> : IRowCaption<T>
    where T : struct, IComparable
  {
    public string Label { get; private set; }

  }
}
