using NSimpleOLAP.Common;
using NSimpleOLAP.Configuration;
using NSimpleOLAP.Data;
using NSimpleOLAP.Interfaces;
using NSimpleOLAP.Query.Interfaces;
using NSimpleOLAP.Schema;
using NSimpleOLAP.Schema.Interfaces;
using NSimpleOLAP.Storage.FactsCache;
using NSimpleOLAP.Storage.Interfaces;
using NSimpleOLAP.Storage.Molap.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSimpleOLAP.CubeExpressions;
using NSimpleOLAP.Triggers.Interfaces;
using NSimpleOLAP.Triggers;

namespace NSimpleOLAP.Storage.Molap
{
  /// <summary>
  /// Description of MolapStorage.
  /// </summary>
  public class MolapStorage<T, U> : IStorage<T, U>
    where T : struct, IComparable
    where U : class, ICell<T>
  {
    private Graph<T, U> _globalGraph;
    private TriggerHelper<T, U> _triggerHelper;
    private T _cubeid;
    private CanonicFormater<T> _canonicFormater;
    private IFactsProviderCache<T, FactsRow<T>> _factsCache;
    private MolapCellValuesHelper<T, U> _cellValuesHelper;
    private MolapAggregationsStorage<T, U> _onDemandAggregations;

    public MolapStorage(T cubeid, StorageConfig config, IList<ITrigger<T>> triggers)
    {
      _cubeid = cubeid;
      this.Config = config;
      _canonicFormater = new CanonicFormater<T>();
      _factsCache = new InMemoryFactsProvider<T>(this.Config.MolapConfig.HashType);
      _triggerHelper = new TriggerHelper<T, U>(triggers);

      this.Init();
    }

    #region private methods

    private void Init()
    {
      this.NameSpace = new ImpNameSpace(AbsIdentityKey<T>.Create());
      this.Dimensions = new MembersCollection<Dimension<T>>(
        ItemType.Dimension,
        (dimension) =>
        {
          this.NameSpace.Add(dimension);
          
          if (dimension.TypeOf == DimensionType.Date)
          {
            //todo change this
            ((DimensionDate<T>)dimension).SetMembersStorage(new DimensionMembersCollection());
          }
          else if (dimension.TypeOf == DimensionType.Levels)
          {
            //todo change this
            ((DimensionLevel<T>)dimension).SetMembersStorage(new DimensionMembersCollection());
          }
          else if (dimension.TypeOf == DimensionType.Time)
          {
            //todo change this
            ((DimensionTime<T>)dimension).SetMembersStorage(new DimensionMembersCollection());
          }
          else
            dimension.SetMembersStorage(new DimensionMembersCollection());
        },
        (storage) =>
        {
          this.NameSpace.Clear(ItemType.Dimension);

          foreach (Dimension<T> item in storage)
            item.Dispose();
        });
      this.Measures = new MembersCollection<Measure<T>>(
        ItemType.Measure,
        (measure) => { this.NameSpace.Add(measure); },
        (storage) => this.NameSpace.Clear(ItemType.Measure));
      this.Metrics = new MembersCollection<Metric<T>>(
        ItemType.Metric,
        (metric) => { this.NameSpace.Add(metric); },
        (storage) => this.NameSpace.Clear(ItemType.Metric));

      _cellValuesHelper = new CellValuesHelper(this.Measures, this.Dimensions, this.Metrics);
      _globalGraph = new Graph<T, U>(_cubeid, this.Config, _cellValuesHelper, _triggerHelper);
      _onDemandAggregations = new MolapAggregationsStorage<T, U>(_cubeid, this.Config, _cellValuesHelper, _canonicFormater);
    }

    #endregion private methods

    #region IStorage<T,U> implementation

    public IEnumerable<U> GetCells(KeyTuplePairs<T> pairs)
    {
      KeyValuePair<T, T>[] cpairs = _canonicFormater.Format(pairs.AnchorTuple);

      foreach (var item in _globalGraph.GetNodes(cpairs, pairs))
        yield return item.Container;
    }

    public IEnumerable<U> CellEnumerator()
    {
      foreach (Node<T, U> item in _globalGraph.NodesEnumerator())
        yield return item.Container;
    }

    public U GetCell(KeyValuePair<T, T>[] pairs)
    {
      KeyValuePair<T, T>[] cpairs = _canonicFormater.Format(pairs);
      Node<T, U> node = _globalGraph.GetNode(cpairs);

      if (node != null)
        return node.Container;
      else
        return null;
    }

    public IEnumerable<U> GetCells(T key, KeyTuplePairs<T> pairs)
    {
      var graph = _onDemandAggregations[key];

      KeyValuePair<T, T>[] cpairs = _canonicFormater.Format(pairs.AnchorTuple);

      foreach (var item in graph.GetNodes(cpairs, pairs))
        yield return item.Container;
    }

    public U GetCell(T key, KeyValuePair<T, T>[] pairs)
    {
      var graph = _onDemandAggregations[key];

      KeyValuePair<T, T>[] cpairs = _canonicFormater.Format(pairs);
      Node<T, U> node = graph.GetNode(cpairs);

      if (node != null)
        return node.Container;
      else
        return null;
    }

    public void AddRowData(KeyValuePair<T, T>[] pairs, MeasureValuesCollection<T> data)
    {
      var tasks = Task.WhenAll(
        Task.Run(() => _factsCache.AddFRow(pairs, data)),
        Task.Run(() =>
        {
          if (this.Config.MolapConfig.OperationType == OperationMode.PreAggregate)
            _globalGraph.AddRowInfo(data, pairs);
        }));

      tasks.Wait();
    }

    public int GetCellCount()
    {
      int count = 0;
      IEnumerable<int> cellscounts = from item in _globalGraph.NodesEnumerator()
                                     select item.GetNodeCount();
      count = cellscounts.Sum();

      return count;
    }

    public void DropCells()
    {
      _factsCache.Clear();
      _onDemandAggregations.Clear();
      var oldGraph = _globalGraph;
      _globalGraph = new Graph<T, U>(_cubeid, this.Config, _cellValuesHelper, _triggerHelper);
      oldGraph.Dispose();
    }

    public void Dispose()
    {
      _globalGraph.Dispose();
      _onDemandAggregations.Dispose();
      NameSpace.Dispose();
      _factsCache.Dispose();
    }

    public T CreateAggregation(KeyValuePair<T, T>[] axisPairs, IPredicate<T> predicateRoot)
    {
      if (_onDemandAggregations.AggregationExists(axisPairs))
        _onDemandAggregations.RemoveAggregation(axisPairs);

      return _onDemandAggregations.CreateAggregation(axisPairs, predicateRoot.GetHashCode());
    }

    public bool AggregationExists(KeyValuePair<T, T>[] axisPairs, IPredicate<T> predicateRoot)
    {
      if (predicateRoot == null || predicateRoot.GetHashCode() == 0)
        return true;

      return _onDemandAggregations.AggregationHasFilter(axisPairs, predicateRoot.GetHashCode());
    }

    public T GetAggregationId(KeyValuePair<T, T>[] axisPairs, IPredicate<T> predicateRoot)
    {
      if (predicateRoot == null || predicateRoot.GetHashCode() == 0)
        return default(T);

      return _onDemandAggregations.GetAggregationKey(axisPairs);
    }

    public void PopulateNewAggregation(T key, IPredicate<T> predicateRoot)
    {
      var graph = _onDemandAggregations[key];

      if (predicateRoot.FiltersOnFacts())
      {
        foreach (var item in _factsCache.EnumerateFacts())
        {
          if (predicateRoot.Execute(item.Pairs, item.Data))
          {
            graph.AddRowInfo(item.Data, item.Pairs);
          }
        }
      }
      else
        throw new Exception("Predicate isn't filterling on facts.");
    }

    public void RegisterTrigger(ITrigger<T> trigger)
    {
      if (_triggerHelper != null) // change this
        _triggerHelper.TryRegister(trigger, _globalGraph);
    }

    public void DeRegisterTrigger(ITrigger<T> trigger)
    {
      if (_triggerHelper != null)
        _triggerHelper.TryDeRegister(trigger, _globalGraph);
    }

    public void RunQueuedTriggers()
    {
      if (_triggerHelper != null)
        _triggerHelper.ServiceQueue();
    }

    public StorageType StorageType { get { return StorageType.Molap; } }

    public INamespace<T> NameSpace
    {
      get;
      private set;
    }

    public IMemberStorage<T, Dimension<T>> Dimensions
    {
      get;
      private set;
    }

    public IMemberStorage<T, Measure<T>> Measures
    {
      get;
      private set;
    }

    public IMemberStorage<T, Metric<T>> Metrics
    {
      get;
      private set;
    }

    public StorageConfig Config
    {
      get;
      private set;
    }

    #endregion IStorage<T,U> implementation

    #region private classes

    private class MembersCollection<TMember> : AbsMolapMemberCollection<T, TMember>
      where TMember : IDataItem<T>
    {
      public MembersCollection(ItemType type, Action<TMember> onaddmember, Action<IMemberStorage<T, TMember>> onclear)
      {
        _type = type;
        this.memberOnAdd = onaddmember;
        this.onClear = onclear;
        this.Init();
      }
    }

    private class DimensionMembersCollection : AbsMolapMemberCollection<T, Member<T>>
    {
      private AbsIdentityKey<T> _keybuilder;

      public DimensionMembersCollection()
      {
        _keybuilder = AbsIdentityKey<T>.Create();
        _type = ItemType.Member;
        this.memberOnAdd = (item) =>
        {
          if (item.ID.Equals(default(T)))
            item.ID = _keybuilder.GetNextKey();
        };
        this.Init();
      }
    }

    private class ImpNameSpace : AbsMolapNameSpace<T>
    {
      public ImpNameSpace(AbsIdentityKey<T> keybuilder)
      {
        this._keybuilder = keybuilder;
        this.Init();
      }
    }

    private class CellValuesHelper : MolapCellValuesHelper<T, U>
    {
      private IMemberStorage<T, Measure<T>> _measures;
      private IMemberStorage<T, Dimension<T>> _dimensions;
      private IMemberStorage<T, Metric<T>> _metrics;

      public CellValuesHelper(IMemberStorage<T, Measure<T>> measures, IMemberStorage<T, Dimension<T>> dimensions, IMemberStorage<T, Metric<T>> metrics)
      {
        _measures = measures;
        _dimensions = dimensions;
        _metrics = metrics;
      }

      public override void UpdateMeasures(U cell, MeasureValuesCollection<T> measures, CellContext<T> context)
      {
        MolapCell<T> mcell = (MolapCell<T>)(object)cell;

        foreach (var previousValue in mcell.Values)
        {
          context.UpdateOldValue(previousValue.Key, previousValue.Value);
        }

        mcell.IncrementOcurrences();

        foreach (KeyValuePair<T, object> item in measures)
        {
          if (item.Value != null)
          {
            ValueType nvalue = (ValueType)item.Value;

            if (mcell.Values.ContainsKey(item.Key))
            {
              Type measuretype = this._measures[item.Key].DataType;
              ValueType ovalue = mcell.Values[item.Key];
              var functor = GetMeasureAggregationFunction(measuretype);

              if (functor != null)
              {
                context.UpdateNewValue(item.Key, nvalue);
                mcell.Values[item.Key] = this.Add(ovalue, nvalue, functor);
              }
            }
            else
            {
              context.UpdateNewValue(item.Key, nvalue);
              mcell.Values.Add(item.Key, nvalue);
            }
              
          }
        }
      }

      public override void UpdateMetrics(U cell, CellContext<T> context)
      {
        MolapCell<T> mcell = (MolapCell<T>)(object)cell;
        
        foreach (var item in _metrics)
        {
          object nvalue = null;

          try
          {
            context.CurrentMetric = item.ID;
            nvalue = item.MetricExpression.Evaluate(context);
          }
          catch (Exception ex)
          {
            //to do logging error and error behavior
            Console.WriteLine(ex.Message);
          }

          if (nvalue != null)
          {
            if (mcell.Values.ContainsKey(item.ID))
              mcell.Values[item.ID] = (ValueType)nvalue;
            else
              mcell.Values.Add(item.ID, (ValueType)nvalue);
          }
          else
            mcell.Values.Remove(item.ID);
        }
      }

      public override void ClearCell(U cell)
      {
        ((MolapCell<T>)(object)cell).Reset();
      }

      private Func<ValueType, ValueType, ValueType> GetMeasureAggregationFunction(Type measuretype)
      {
        Func<ValueType, ValueType, ValueType> functor = null;

        if (measuretype == typeof(int))
          functor = (x, y) => (int)x + (int)y;
        else if (measuretype == typeof(long))
          functor = (x, y) => (long)x + (long)y;
        else if (measuretype == typeof(uint))
          functor = (x, y) => (uint)x + (uint)y;
        else if (measuretype == typeof(ulong))
          functor = (x, y) => (ulong)x + (ulong)y;
        else if (measuretype == typeof(decimal))
          functor = (x, y) => (decimal)x + (decimal)y;
        else if (measuretype == typeof(float))
          functor = (x, y) => (float)x + (float)y;
        else if (measuretype == typeof(double))
          functor = (x, y) => (double)x + (double)y;

        return functor;
      }
    }

    #endregion private classes
  }
}