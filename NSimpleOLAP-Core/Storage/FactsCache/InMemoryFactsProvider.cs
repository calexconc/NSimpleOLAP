using NSimpleOLAP.Common;
using NSimpleOLAP.Common.Collections;
using NSimpleOLAP.Common.Hashing;
using NSimpleOLAP.Data;
using NSimpleOLAP.Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimpleOLAP.Storage.FactsCache
{
  public class InMemoryFactsProvider<T> : IFactsProviderCache<T, FactsRow<T>>
    where T : struct, IComparable
  {
    private Hasher<T> _hasher;
    private MolapHashTypes _hashingtype;
    private TSList<FactsRow<T>> _innerList;

    public InMemoryFactsProvider(MolapHashTypes hashingType)
    {
      _hashingtype = hashingType;
      _innerList = new TSList<FactsRow<T>>();
      this.Init();
    }

    public int Count
    {
      get { return _innerList.Count; }
    }

    #region private methods

    private void Init()
    {
      _hasher = Hasher<T>.Create(this._hashingtype);
    }

    #endregion private methods

    public void AddFRow(KeyValuePair<T, T>[] pairs, MeasureValuesCollection<T> data)
    {
      _innerList.Add(new FactsRow<T>(_innerList.Count, _hasher.HashTuples(pairs), pairs, data));
    }

    public void Clear()
    {
      _innerList.Clear();
    }

    public void Dispose()
    {
      _innerList.Dispose();
    }

    public IEnumerable<FactsRow<T>> EnumerateFacts()
    {
      foreach (var item in _innerList)
        yield return item;
    }

    public IEnumerable<FactsRow<T>> FetchFacts(KeyValuePair<T, T>[] pairs)
    {
      var hash = _hasher.HashTuples(pairs);

      return _innerList.Where(x => x.HashCode.Equals(hash));
    }

    public bool RemoveRows(KeyValuePair<T, T>[] pairs)
    {
      var query = FetchFacts(pairs)
        .Select(x => x.Index).ToArray();

      return RemoveRows(query);
    }

    public bool RemoveRows(params int[] indexes)
    {
      var result = false;

      foreach (var index in indexes)
      {
        var count = _innerList.Count;

        _innerList.RemoveAt(index);

        result = result || _innerList.Count < count;
      }

      return result;
    }
  }
}