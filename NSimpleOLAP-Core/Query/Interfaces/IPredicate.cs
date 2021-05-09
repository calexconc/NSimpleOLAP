using NSimpleOLAP.Common;
using NSimpleOLAP.Data;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Query.Interfaces
{
  /// <summary>
  /// Description of IPredicate.
  /// </summary>
  public interface IPredicate<T>
    where T : struct, IComparable
  {
    PredicateType TypeOf { get; }

    bool Execute(KeyValuePair<T, T>[] pairs, MeasureValuesCollection<T> data);

    bool Execute(KeyValuePair<T, T>[] pairs);

    bool FiltersOnFacts();

    bool FiltersOnAggregation();

    IEnumerable<Tuple<LogicalOperators, KeyValuePair<T, T>[]>> ExtractFilterDimensionality();
  }
}