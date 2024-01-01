using NSimpleOLAP.Common;
using NSimpleOLAP.Data;
using NSimpleOLAP.Query.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Query.Predicates
{
  /// <summary>
  /// Description of SliceByMeasure.
  /// </summary>
  internal class SliceByMeasure<T> : IPredicate<T>
    where T : struct, IComparable
  {
    private T _measure;
    private LogicalOperators _operator;
    private object _value;
    private Type _measureType;

    public SliceByMeasure(T measureKey, Type measureType,
                          LogicalOperators loperator, object value)
    {
      _measure = measureKey;
      _operator = loperator;
      _measureType = measureType;
      _value = value;
    }

    public T MeasureKey
    {
      get { return _measure; }
    }

    public LogicalOperators Operator
    {
      get { return _operator; }
    }

    public Type MeasureType
    {
      get { return _measureType; }
    }

    public object Value
    {
      get { return _value; }
    }

    public PredicateType TypeOf
    {
      get { return PredicateType.MEASURE; }
    }

    public override bool Equals(object obj)
    {
      return base.Equals(obj);
    }

    public bool Execute(KeyValuePair<T, T>[] pairs, MeasureValuesCollection<T> data)
    {
      if (data.ContainsKey(MeasureKey))
      {
        var value = data[MeasureKey];

        switch (_operator)
        {
          case LogicalOperators.EQUALS:
            return _value.Equals(value);

          case LogicalOperators.NOTEQUALS:
            return !_value.Equals(value);

          case LogicalOperators.GREATERTHAN:
            return value.GreaterThan(_value);

          case LogicalOperators.GREATEROREQUALS:
            return value.GreaterOrEquals(_value);

          case LogicalOperators.LOWERTHAN:
            return value.LowerThan(_value);

          case LogicalOperators.LOWEROREQUALS:
            return value.LowerOrEquals(_value);
        }
      }

      return false;
    }

    public bool FiltersOnAggregation()
    {
      return false;
    }

    public bool FiltersOnFacts()
    {
      return true;
    }

    public override int GetHashCode()
    {
      var ioperator = Operator == LogicalOperators.EQUALS ? -111 : (int)Operator;
      var result = _value.GetHashCode()
        ^ ioperator
        ^ (int)TypeOf;

      return result;
    }

    public IEnumerable<Tuple<LogicalOperators, KeyValuePair<T, T>[]>> ExtractFilterDimensionality()
    {
      yield break;
    }

    public bool Execute(KeyValuePair<T, T>[] pairs)
    {
      return true;
    }
  }
}