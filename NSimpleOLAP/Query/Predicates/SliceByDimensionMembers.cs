using NSimpleOLAP.Common;
using NSimpleOLAP.Data;
using NSimpleOLAP.Query.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimpleOLAP.Query.Predicates
{
  /// <summary>
  /// Description of SliceByDimensionMember.
  /// </summary>
  internal class SliceByDimensionMembers<T> : IPredicate<T>
    where T : struct, IComparable
  {
    private T _dimension;
    private LogicalOperators _operator;
    private List<T> _values;

    public SliceByDimensionMembers(T dimensionKey, LogicalOperators loperator, params T[] values)
    {
      _values = new List<T>();
      _dimension = dimensionKey;
      _operator = loperator;
      _values.AddRange(values);
    }

    public T Dimension
    {
      get { return _dimension; }
    }

    public LogicalOperators Operator
    {
      get { return _operator; }
    }

    public IEnumerable<T> Values
    {
      get { return _values; }
    }

    public PredicateType TypeOf
    {
      get { return PredicateType.DIMENSION; }
    }

    public override bool Equals(object obj)
    {
      return base.Equals(obj);
    }

    public bool Execute(KeyValuePair<T, T>[] pairs, MeasureValuesCollection<T> data)
    {
      var results = pairs
        .Where(x => x.Key.Equals(Dimension))
        .ToArray();

      if (results.Length > 0)
      {
        if (_operator == LogicalOperators.EQUALS || _operator == LogicalOperators.NOTEQUALS)
        {
          return Execute(results[0].Value, _values[0]);
        }

        if (_operator == LogicalOperators.IN)
        {
          return results
            .Join(_values, x => x.Value, y => y, (x, y) => y)
            .Any();
        }
      }

      return (_operator == LogicalOperators.EQUALS || _operator == LogicalOperators.IN)
        ? false : true;
    }

    public bool FiltersOnAggregation()
    {
      return true;
    }

    public bool FiltersOnFacts()
    {
      return false;
    }

    public override int GetHashCode()
    {
      var result = TypeOf.GetHashCode()
        ^ Operator.GetHashCode();

      foreach (var item in _values)
        result ^= item.GetHashCode();

      return result;
    }

    public IEnumerable<Tuple<LogicalOperators, KeyValuePair<T, T>[]>> ExtractFilterDimensionality()
    {
      if (FiltersOnAggregation())
      {
        var keypairs = Values
          .Select(x => new KeyValuePair<T, T>(Dimension, x))
          .ToArray();

        yield return new Tuple<LogicalOperators, KeyValuePair<T, T>[]>(Operator, keypairs);
      }
    }

    public bool Execute(KeyValuePair<T, T>[] pairs)
    {
      var results = pairs
        .Where(x => x.Key.Equals(Dimension) && !x.Value.Equals(default(T)))
        .ToArray();

      if (results.Length > 0)
      {
        if (_operator == LogicalOperators.EQUALS || _operator == LogicalOperators.NOTEQUALS)
        {
          return Execute(results[0].Value, _values[0]);
        }

        if (_operator == LogicalOperators.IN)
        {
          return results
            .Join(_values, x => x.Value, y => y, (x, y) => y)
            .Any();
        }
      }

      return (_operator == LogicalOperators.EQUALS || _operator == LogicalOperators.IN)
        ? false : true;
    }

    private bool Execute(T inValue, T compareValue)
    {
      switch (_operator)
      {
        case LogicalOperators.EQUALS:
          return inValue.Equals(compareValue);

        case LogicalOperators.NOTEQUALS:
          return !inValue.Equals(compareValue);

        case LogicalOperators.IN:
          return inValue.Equals(compareValue);

        default:
          return true;
      }
    }
  }
}