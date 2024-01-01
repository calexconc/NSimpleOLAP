using NSimpleOLAP.Common;
using NSimpleOLAP.Data;
using NSimpleOLAP.Query.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Query.Predicates
{
  public class NullPredicate<T> : IPredicate<T>
     where T : struct, IComparable
  {
    public PredicateType TypeOf
    {
      get { return PredicateType.NULL; }
    }

    public override bool Equals(object obj)
    {
      return base.Equals(obj);
    }

    public bool Execute(KeyValuePair<T, T>[] pairs, MeasureValuesCollection<T> data)
    {
      return true;
    }

    public bool FiltersOnAggregation()
    {
      return false;
    }

    public bool FiltersOnFacts()
    {
      return false;
    }

    public override int GetHashCode()
    {
      return 0;
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