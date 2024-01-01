using NSimpleOLAP.Data;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Storage.Interfaces
{
  public interface IFactsProviderCache<T, FROW> : IDisposable
    where T : struct, IComparable
    where FROW : IFactRow<T>
  {
    int Count { get; }

    void AddFRow(KeyValuePair<T, T>[] pairs, MeasureValuesCollection<T> data);

    IEnumerable<FROW> FetchFacts(KeyValuePair<T, T>[] pairs);

    IEnumerable<FROW> EnumerateFacts();

    bool RemoveRows(KeyValuePair<T, T>[] pairs);

    bool RemoveRows(params int[] indexes);

    void Clear();
  }
}