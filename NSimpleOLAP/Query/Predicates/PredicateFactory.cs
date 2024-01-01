using NSimpleOLAP.Query.Builder;
using NSimpleOLAP.Query.Interfaces;
using System;
using NSimpleOLAP.Common.Utils;

namespace NSimpleOLAP.Query.Predicates
{
  /// <summary>
  /// Description of PredicateFactory.
  /// </summary>
  public class PredicateBuilderFactory<T>
    where T : struct, IComparable
  {
    private NamespaceResolver<T> _resolver;

    public PredicateBuilderFactory(NamespaceResolver<T> resolver)
    {
      _resolver = resolver;
    }

    #region Create Predicates

    public IPredicateBuilder<T> CreateDimensionSlicer()
    {
      return new DimensionSlicerBuilder<T>(_resolver);
    }

    public IPredicateBuilder<T> CreateMeasureSlicer()
    {
      return new MeasureSlicerBuilder<T>(_resolver);
    }

    internal IPredicateBuilder<T> CreateAndPredicate()
    {
      return new AndPredicateBuilder<T>(this);
    }

    internal IPredicateBuilder<T> CreateOrPredicate()
    {
      return new OrPredicateBuilder<T>(this);
    }

    internal IPredicateBuilder<T> CreateBlockPredicate()
    {
      return new BlockPredicateBuilder<T>(this);
    }

    internal IPredicateBuilder<T> CreateNotPredicate()
    {
      return new NotPredicateBuilder<T>(this);
    }

    #endregion Create Predicates
  }
}