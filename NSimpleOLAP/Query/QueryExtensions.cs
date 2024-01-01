using NSimpleOLAP.Query.Builder;
using System;

namespace NSimpleOLAP.Query
{
  /// <summary>
  /// Description of QueryExtensions.
  /// </summary>
  public static class QueryExtensions
  {
    public static QueryBuilder<T> BuildQuery<T>(this Cube<T> cube)
      where T : struct, IComparable
    {
      return new QueryBuilder<T>.QueryBuilderImpl(cube);
    }
  }
}