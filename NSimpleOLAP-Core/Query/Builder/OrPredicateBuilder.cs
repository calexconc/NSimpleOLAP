using NSimpleOLAP.Query.Interfaces;
using NSimpleOLAP.Query.Predicates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimpleOLAP.Query.Builder
{
  /// <summary>
  /// Description of OrPredicateBuilder.
  /// </summary>
  public class OrPredicateBuilder<T> : IPredicateBuilder<T>
    where T : struct, IComparable
  {
    private List<IPredicateBuilder<T>> _predicates;
    private PredicateBuilderFactory<T> _factory;

    public OrPredicateBuilder(PredicateBuilderFactory<T> factory)
    {
      _factory = factory;
      _predicates = new List<IPredicateBuilder<T>>();
    }

    #region Fluent interface

    public NotPredicateBuilder<T> Not(Func<NotPredicateBuilder<T>, IPredicateBuilder<T>> notPred)
    {
      var predBuilder = (NotPredicateBuilder<T>)_factory.CreateNotPredicate();

      _predicates.Add(predBuilder);
      notPred(predBuilder);

      return predBuilder;
    }

    public DimensionSlicerBuilder<T> Dimension(string dimension)
    {
      var predBuilder = (DimensionSlicerBuilder<T>)_factory.CreateDimensionSlicer();

      _predicates.Add(predBuilder);

      return predBuilder.SetDim(dimension);
    }

    public MeasureSlicerBuilder<T> Measure(string measure)
    {
      var predBuilder = (MeasureSlicerBuilder<T>)_factory.CreateMeasureSlicer();

      _predicates.Add(predBuilder);

      return predBuilder.SetMeasure(measure);
    }

    public BlockPredicateBuilder<T> Block(Func<BlockPredicateBuilder<T>, IPredicateBuilder<T>> blockPred)
    {
      var predBuilder = (BlockPredicateBuilder<T>)_factory.CreateBlockPredicate();

      _predicates.Add(predBuilder);
      blockPred(predBuilder);

      return predBuilder;
    }

    #endregion Fluent interface

    public IPredicate<T> Build()
    {
      var builders = from item in _predicates
                     select item.Build();
      var predicate = new OrPredicate<T>();

      predicate.AddPredicate(builders.ToArray());

      return predicate;
    }
  }
}