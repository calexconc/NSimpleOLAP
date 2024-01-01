using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.Query.Interfaces;
using NSimpleOLAP.Query.Molap;
using System;
using NSimpleOLAP.Common;
using System.Collections.Generic;

namespace NSimpleOLAP.Query.Builder
{
  /// <summary>
  /// Description of QueryBuilder.
  /// </summary>
  public abstract class QueryBuilder<T>
    where T : struct, IComparable
  {
    protected WhereBuilder<T> _wherebuilder;
    protected Cube<T> _innerCube;
    private NamespaceResolver<T> _resolver;
    protected AxisBuilder<T> _axisBuilder;
    protected List<T> _measureOrMetricKeys;
    protected List<LinearSummaries> _summaries;

    protected void Init()
    {
      _measureOrMetricKeys = new List<T>();
      _resolver = new NamespaceResolver<T>(_innerCube);
      _wherebuilder = new WhereBuilder<T>(_resolver);
      _axisBuilder = new AxisBuilder<T>(_innerCube.Config.Storage.MolapConfig.HashType, _innerCube.Schema);
      _summaries = new List<LinearSummaries>();
    }

    #region fluent interface

    /// <summary>
    /// Assign tuples to appear in row
    /// </summary>
    /// <param name="tuples">Tuples are inserted in "dimension.member" form</param>
    /// <returns></returns>
    public QueryBuilder<T> OnRows(params string[] tuples)
    {
      foreach (var item in tuples)
        _axisBuilder.AddRowTuples(_resolver.DimensionTranslate(item));

      return this;
    }

    /// <summary>
    /// Assign tuples to appear in row
    /// </summary>
    /// <param name="tuples">Tuples are inserted in KeyValuePairs</param>
    /// <returns></returns>
    public QueryBuilder<T> OnRows(params KeyValuePair<T, T>[] tuples)
    {
      _axisBuilder.AddRowTuples(tuples);

      return this;
    }

    /// <summary>
    /// Assign tuples to appear in column
    /// </summary>
    /// <param name="tuples">Tuples are inserted in "dimension.member" form</param>
    /// <returns></returns>
    public QueryBuilder<T> OnColumns(params string[] tuples)
    {
      foreach (var item in tuples)
        _axisBuilder.AddColumnTuples(_resolver.DimensionTranslate(item));

      return this;
    }

    /// <summary>
    /// Assign tuples to appear in column
    /// </summary>
    /// <param name="tuples">Tuples are inserted in KeyValuePairs</param>
    /// <returns></returns>
    public QueryBuilder<T> OnColumns(params KeyValuePair<T, T>[] tuples)
    {
      _axisBuilder.AddColumnTuples(tuples);

      return this;
    }

    /// <summary>
    /// Assign measures
    /// </summary>
    /// <param name="measuresOrMetrics">Measure's names</param>
    /// <returns></returns>
    public QueryBuilder<T> AddMeasuresOrMetrics(params string[] measuresOrMetrics)
    {
      foreach (var item in measuresOrMetrics)
      {
        var dataItem = _resolver.GetDataItemInfo(item);

        if (dataItem.ItemType == Common.ItemType.Measure
          || dataItem.ItemType == Common.ItemType.Metric)
          _measureOrMetricKeys.Add(dataItem.ID);
        else
          throw new Exception($"Entity {item} does not exist as a measure or as a metric.");
      }

      return this;
    }

    /// <summary>
    /// Assign measures
    /// </summary>
    /// <param name="measureOrMetricsKeys">Measure's keys</param>
    /// <returns></returns>
    public QueryBuilder<T> AddMeasuresOrMetrics(params T[] measureOrMetricsKeys)
    {
      _measureOrMetricKeys.AddRange(measureOrMetricsKeys);

      return this;
    }

    /// <summary>
    /// construct where cause
    /// </summary>
    /// <param name="whereBuild"></param>
    /// <returns></returns>
    public QueryBuilder<T> Where(Action<WhereBuilder<T>> whereBuild)
    {
      //  _wherebuilder.Define()

      //    Func<BlockPredicateBuilder<T>, IPredicateBuilder<T>> blockBuilder
      whereBuild(_wherebuilder);

      return this;
    }

    public QueryBuilder<T> Where(Func<BlockPredicateBuilder<T>, IPredicateBuilder<T>> blockBuilder)
    {
      _wherebuilder.Define(blockBuilder);

      return this;
    }

    public Query<T> Create()
    {
      return new QueryImplementation(_innerCube, _axisBuilder.Build(), _measureOrMetricKeys, _wherebuilder.Build(), _summaries);
    }

    public QueryBuilder<T> GetRowTotals()
    {
      if (!_summaries.Contains(LinearSummaries.ROW_TOTALS))
        _summaries.Add(LinearSummaries.ROW_TOTALS);

      return this;
    }

    public QueryBuilder<T> GetColumnTotals()
    {
      if (!_summaries.Contains(LinearSummaries.COLUMN_TOTALS))
        _summaries.Add(LinearSummaries.COLUMN_TOTALS);

      return this;
    }

    public QueryBuilder<T> GetBaseRowTotals()
    {
      if (!_summaries.Contains(LinearSummaries.ROW_BASE_TOTALS))
        _summaries.Add(LinearSummaries.ROW_BASE_TOTALS);

      return this;
    }

    public QueryBuilder<T> GetBaseColumnTotals()
    {
      if (!_summaries.Contains(LinearSummaries.COLUMN_BASE_TOTALS))
        _summaries.Add(LinearSummaries.COLUMN_BASE_TOTALS);

      return this;
    }

    #endregion fluent interface

    #region

    private class QueryImplementation : Query<T>
    {
      public QueryImplementation(Cube<T> cube, Axis<T> axis, List<T> measures, IPredicate<T> predicateTree, List<LinearSummaries> summaries)
      {
        this.cube = cube;
        this.axis = axis;
        this.measures = measures;
        this.predicates = predicateTree;
        this.summaries = summaries;
        this.queryOrchestrator = new MolapQueryOrchestrator<T>(this.cube);
      }
    }

    internal class QueryBuilderImpl : QueryBuilder<T>
    {
      public QueryBuilderImpl(Cube<T> cube)
      {
        this._innerCube = cube;
        this.Init();
      }
    }

    #endregion
  }
}