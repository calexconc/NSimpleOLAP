using NSimpleOLAP.Common;
using NSimpleOLAP.Data;
using NSimpleOLAP.Query.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Query.Predicates
{
  /// <summary>
  /// Description of OrPredicate.
  /// </summary>
  internal class OrPredicate<T> : IPredicate<T>
    where T : struct, IComparable
  {
    private List<IPredicate<T>> _predicates;

    public OrPredicate()
    {
      _predicates = new List<IPredicate<T>>();
    }

    public void AddPredicate(params IPredicate<T>[] predicates)
    {
      _predicates.AddRange(predicates);
    }

    public IEnumerable<IPredicate<T>> Predicates
    {
      get { return _predicates; }
    }

    public PredicateType TypeOf
    {
      get { return PredicateType.OR; }
    }

    public override bool Equals(object obj)
    {
      return base.Equals(obj);
    }

    public override int GetHashCode()
    {
      var result = (int)TypeOf;

      foreach (var item in _predicates)
        result ^= item.GetHashCode();

      return result;
    }

    public bool Execute(KeyValuePair<T, T>[] pairs, MeasureValuesCollection<T> data)
    {
      var result = false;

      foreach (var item in _predicates)
      {
        var value = item.Execute(pairs, data);

        if (value)
        {
          result = value;
          break;
        }
      }

      return result;
    }

    public bool FiltersOnFacts()
    {
      foreach (var item in _predicates)
      {
        if (item.FiltersOnFacts())
          return true;
      }

      return false;
    }

    public bool FiltersOnAggregation()
    {
      foreach (var item in _predicates)
      {
        if (item.FiltersOnAggregation())
          return true;
      }

      return false;
    }

    public IEnumerable<Tuple<LogicalOperators, KeyValuePair<T, T>[]>> ExtractFilterDimensionality()
    {
      if (FiltersOnAggregation())
      {
        foreach (var item in _predicates)
        {
          if (!item.FiltersOnAggregation())
            continue;

          foreach (var dims in item.ExtractFilterDimensionality())
            yield return dims;
        }
      }
    }

    public bool Execute(KeyValuePair<T, T>[] pairs)
    {
      var result = false;

      foreach (var item in _predicates)
      {
        var value = item.Execute(pairs);

        if (value)
        {
          result = value;
          break;
        }
      }

      return result;
    }
  }
}