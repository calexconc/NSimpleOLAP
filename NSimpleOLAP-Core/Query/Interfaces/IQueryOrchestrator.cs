using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Query.Interfaces
{
  public interface IQueryOrchestrator<T, U>
    where T : struct, IComparable
    where U : IOutputCell<T>
  {
    IEnumerable<U> GetByCells(Query<T> query);

    IEnumerable<IOutputCell<T>[]> GetByRows(Query<T> query);
  }
}