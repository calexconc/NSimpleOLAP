using NSimpleOLAP.Common;
using NSimpleOLAP.Common.Collections;
using NSimpleOLAP.Configuration;
using NSimpleOLAP.Interfaces;
using NSimpleOLAP.Storage.Interfaces;
using NSimpleOLAP.Storage.Molap.Graph;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSimpleOLAP.Storage.Molap

{
  internal class MolapAggregationsStorage<T, U> : IDisposable
    where T : struct, IComparable
    where U : class, ICell<T>
  {
    private T _rootCubeid;
    private StorageConfig _config;
    private TSDictionary<T, Graph<T, U>> _aggregationGraphs;
    private MolapCellValuesHelper<T, U> _cellValuesHelper;
    private CanonicFormater<T> _canonicFormater;
    private MolapKeyHandler<T> _keyHandler;

    public MolapAggregationsStorage(T cubeid, StorageConfig config, MolapCellValuesHelper<T, U> cellValuesHelper, CanonicFormater<T> canonicFormater)
    {
      _rootCubeid = cubeid;
      _config = config;
      _cellValuesHelper = cellValuesHelper;
      _canonicFormater = canonicFormater;
      _keyHandler = new MolapKeyHandler<T>(config.MolapConfig);
      _aggregationGraphs = new TSDictionary<T, Graph<T, U>>();
    }

    public Graph<T, U> this[T key]
    {
      get
      {
        return _aggregationGraphs[key];
      }
    }

    public T CreateAggregation(KeyValuePair<T, T>[] pairs, int predicateKey)
    {
      var key = _keyHandler.GetKey(pairs);

      _aggregationGraphs.Add(key, new Graph<T, U>(key, _config, _cellValuesHelper, null, predicateKey));

      return key;
    }

    public void FillAggregation(KeyValuePair<T, T>[] pairs, IEnumerable<IFactRow<T>> facts)
    {
      var key = _keyHandler.GetKey(pairs);
      var graph = _aggregationGraphs[key];

      foreach (var item in facts)
        graph.AddRowInfo(item.Data, item.Pairs);
    }

    public bool AggregationExists(KeyValuePair<T, T>[] pairs)
    {
      var key = _keyHandler.GetKey(pairs);

      return _aggregationGraphs.ContainsKey(key);
    }

    public bool AggregationHasFilter(KeyValuePair<T, T>[] pairs, int predicateKey)
    {
      var key = _keyHandler.GetKey(pairs);

      if (!_aggregationGraphs.ContainsKey(key))
        return false;

      return _aggregationGraphs[key].PredicateKey == predicateKey;
    }

    public T GetAggregationKey(KeyValuePair<T, T>[] pairs)
    {
      return _keyHandler.GetKey(pairs);
    }

    public void RemoveAggregation(KeyValuePair<T, T>[] pairs)
    {
      var key = _keyHandler.GetKey(pairs);

      if (_aggregationGraphs.ContainsKey(key))
      {
        var graph = _aggregationGraphs[key];

        _aggregationGraphs.Remove(key);

        Task.Run(() => { graph.Dispose(); });
      }
    }

    public void Clear()
    {
      var options = new ParallelOptions { MaxDegreeOfParallelism = 3 };
      var result = Parallel.ForEach(_aggregationGraphs, options, x => x.Value.Dispose());

      _aggregationGraphs.Clear();
    }

    public void Dispose()
    {
      if (_aggregationGraphs.Count > 0)
        Clear();

      _aggregationGraphs.Dispose();
    }
  }
}