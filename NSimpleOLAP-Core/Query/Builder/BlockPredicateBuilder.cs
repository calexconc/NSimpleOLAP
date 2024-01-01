using NSimpleOLAP.Query.Interfaces;
using NSimpleOLAP.Query.Predicates;
using System;

namespace NSimpleOLAP.Query.Builder
{
  /// <summary>
  /// Description of BlockPredicateBuilder.
  /// </summary>
  public class BlockPredicateBuilder<T> : IPredicateBuilder<T>
    where T : struct, IComparable
  {
    private IPredicateBuilder<T> _innerPredicate;
    private PredicateBuilderFactory<T> _factory;

    public BlockPredicateBuilder(PredicateBuilderFactory<T> factory)
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

    public NotPredicateBuilder<T> Not(Func<NotPredicateBuilder<T>, IPredicateBuilder<T>> notPred)
    {
      var predBuilder = (NotPredicateBuilder<T>)_factory.CreateNotPredicate();

      _innerPredicate = predBuilder;
      notPred(predBuilder);

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

    #endregion Fluent interface

    public IPredicate<T> Build()
    {
      if (_innerPredicate == null)
        return new NullPredicate<T>();

      var predicate = new BlockPredicate<T>(_innerPredicate.Build());

      return predicate;
    }
  }
}