using System;

namespace NSimpleOLAP.Query.Interfaces
{
  /// <summary>
  /// Description of IPredicateBuilder.
  /// </summary>
  public interface IPredicateBuilder<T>
    where T : struct, IComparable
  {
    IPredicate<T> Build();
  }
}