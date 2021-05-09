using NSimpleOLAP.Common;
using NSimpleOLAP.Query.Interfaces;
using NSimpleOLAP.Query.Predicates;
using NSimpleOLAP.Schema;
using System;
using System.Collections.Generic;
using NSimpleOLAP.Common.Utils;

namespace NSimpleOLAP.Query.Builder
{
  /// <summary>
  /// Description of DimensionSlicerBuilder.
  /// </summary>
  public class DimensionSlicerBuilder<T> : IPredicateBuilder<T>
    where T : struct, IComparable
  {
    private T _dimension;
    private List<T> _members;
    private LogicalOperators _operator;
    private NamespaceResolver<T> _resolver;

    public DimensionSlicerBuilder(NamespaceResolver<T> resolver)
    {
      _members = new List<T>();
      _resolver = resolver;
    }

    #region fluent interface

    internal DimensionSlicerBuilder<T> SetDim(string dimension)
    {
      _dimension = _resolver.GetDimension(dimension);

      return this;
    }

    internal DimensionSlicerBuilder<T> SetDim(T dimensionKey)
    {
      _dimension = dimensionKey;

      return this;
    }

    internal DimensionSlicerBuilder<T> SetOperationSegments(LogicalOperators loperator, params string[] members)
    {
      _operator = loperator;

      foreach (var item in members)
        _members.Add(_resolver.GetDimensionMember(_dimension, item));

      return this;
    }

    internal DimensionSlicerBuilder<T> SetOperationSegments(LogicalOperators loperator, params T[] memberKeys)
    {
      _operator = loperator;
      _members.AddRange(memberKeys);

      return this;
    }

    #endregion fluent interface

    public IPredicate<T> Build()
    {
      return new SliceByDimensionMembers<T>(_dimension, _operator, _members.ToArray());
    }
  }
}