using NSimpleOLAP.Query.Interfaces;
using NSimpleOLAP.Query.Predicates;
using System;

namespace NSimpleOLAP.Query.Builder
{
  /// <summary>
  /// Description of NotPredicateBuilder.
  /// </summary>
  public class NotPredicateBuilder<T> : IPredicateBuilder<T>
    where T : struct, IComparable
  {
    private IPredicateBuilder<T> _innerPredicate;
    private PredicateBuilderFactory<T> _factory;

    public NotPredicateBuilder(PredicateBuilderFactory<T> factory)
    {
      _factory = factory;
    }

    #region Fluent interface

    public AndPredicateBuilder<T> And(params Func<AndPredicateBuilder<T>, IPredicateBuilder<T>>[] andPreds)
    {
      var predBuilder = (AndPredicateBuilder<T>)_factory.CreateAndPredicate();

      _innerPredicate = predBuilder;

      foreach (var builder in andPreds)
        builder(predBuilder);

      return predBuilder;
    }

    public OrPredicateBuilder<T> Or(params Func<OrPredicateBuilder<T>, IPredicateBuilder<T>>[] orPreds)
    {
      var predBuilder = (OrPredicateBuilder<T>)_factory.CreateOrPredicate();

      _innerPredicate = predBuilder;

      foreach (var builder in orPreds)
        builder(predBuilder);

      return predBuilder;
    }

    public DimensionSlicerBuilder<T> Dimension(string dimension)
    {
      var predBuilder = (DimensionSlicerBuilder<T>)_factory.CreateDimensionSlicer();

      _innerPredicate = predBuilder;

      return predBuilder.SetDim(dimension);
    }

    public MeasureSlicerBuilder<T> Measure(string measure)
    {
      var predBuilder = (MeasureSlicerBuilder<T>)_factory.CreateMeasureSlicer();

      _innerPredicate = predBuilder;

      return predBuilder.SetMeasure(measure);
    }

    public BlockPredicateBuilder<T> Block(Func<BlockPredicateBuilder<T>, IPredicateBuilder<T>> blockPred)
    {
      var predBuilder = (BlockPredicateBuilder<T>)_factory.CreateMeasureSlicer();

      _innerPredicate = predBuilder;

      blockPred(predBuilder);

      return predBuilder;
    }

    #endregion Fluent interface

    public IPredicate<T> Build()
    {
      var predicate = new NotPredicate<T>(_innerPredicate.Build());

      return predicate;
    }
  }
}