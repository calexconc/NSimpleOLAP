using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Storage.Interfaces
{
  /// <summary>
  /// Description of IVarData.
  /// </summary>
  public interface IVarData<T> : IDictionary<T, object>
    where T : struct, IComparable
  {
  }
}