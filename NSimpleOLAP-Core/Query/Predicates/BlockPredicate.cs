using NSimpleOLAP.Common;
using NSimpleOLAP.Data;
using NSimpleOLAP.Query.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Query.Predicates
{
  /// <summary>
  /// Description of BlockPredicate.
  /// </summary>
  internal class BlockPredicate<T> : IPredicate<T>
    where T : struct, IComparable
  {
    private IPredicate<T> _predicate;

    public BlockPredicate(IPredicate<T> predicate)
    {
      _predicate = predicate;
    }

    public IPredicate<T> Predicate
    {
      get { return _predicate; }
    }

    public PredicateType TypeOf
    {
      get { return PredicateType.BLOCK; }
    }

    public bool Execute(KeyValuePair<T, T>[] pairs, MeasureValuesCollection<T> data)
    {
      return _predicate.Execute(pairs, data);
    }

    public bool FiltersOnFacts()
    {
      return _predicate.FiltersOnFacts();
    }

    public bool FiltersOnAggregation()
    {
      return _predicate.FiltersOnAggregation();
    }

    public override bool Equals(object obj)
    {
      return base.Equals(obj);
    }

    public override int GetHashCode()
    {
      var result = -1
        ^ _predicate.GetHashCode();

      return result;
    }

    public IEnumerable<Tuple<LogicalOperators, KeyValuePair<T, T>[]>> ExtractFilterDimensionality()
    {
      if (FiltersOnAggregation())
      {
        foreach (var dims in _predicate.ExtractFilterDimensionality())
          yield return dims;
      }
    }

    public bool Execute(KeyValuePair<T, T>[] pairs)
    {
      return _predicate.Execute(pairs);
    }
  }
}