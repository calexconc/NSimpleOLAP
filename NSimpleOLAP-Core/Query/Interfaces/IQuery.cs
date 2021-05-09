using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Query.Interfaces
{
  public interface IQuery<T, U>
    where T : struct, IComparable
    where U : class, IOutputCell<T>
  {
    IEnumerable<U> StreamCells();

    IEnumerable<U[]> StreamRows();
  }
}