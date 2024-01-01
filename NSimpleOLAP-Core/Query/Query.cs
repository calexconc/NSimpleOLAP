using NSimpleOLAP.Common;
using NSimpleOLAP.Query.Interfaces;
using System;
using System.Collections.Generic;

namespace NSimpleOLAP.Query
{
  /// <summary>
  /// Description of Query.
  /// </summary>
  public abstract class Query<T> : IQuery<T, IOutputCell<T>>
    where T : struct, IComparable
  {
    protected Axis<T> axis;

    protected IPredicate<T> predicates;

    protected Cube<T> cube;

    protected List<T> measures;

    protected List<LinearSummaries> summaries;

    protected IQueryOrchestrator<T, IOutputCell<T>> queryOrchestrator;

    private bool? _hasRowTotals;
    private bool? _hasColumnTotals;
    private bool? _hasRowBaseTotals;
    private bool? _hasColumnBaseTotals;

    protected IQueryOrchestrator<T, IOutputCell<T>> Orchestrator
    {
      get
      {
        return queryOrchestrator;
      }
    }

    internal Cube<T> Cube
    {
      get { return cube; }
    }

    internal Axis<T> Axis
    {
      get { return axis; }
    }

    internal List<T> MeasuresOrMetrics
    {
      get { return measures; }
    }

    internal IPredicate<T> PredicateTree
    {
      get { return predicates; }
    }

    internal List<LinearSummaries> Summaries
    {
      get { return summaries; }
    }

    internal bool HasRowTotals
    {
      get
      {
        if (!_hasRowTotals.HasValue)
          _hasRowTotals = Summaries.Contains(LinearSummaries.ROW_TOTALS);

        return _hasRowTotals.Value;
      }
    }

    internal bool HasColumnTotals
    {
      get
      {
        if (!_hasColumnTotals.HasValue)
          _hasColumnTotals = Summaries.Contains(LinearSummaries.COLUMN_TOTALS);

        return _hasColumnTotals.Value;
      }
    }

    internal bool HasRowBaseTotals
    {
      get
      {
        if (!_hasRowBaseTotals.HasValue)
          _hasRowBaseTotals = Summaries.Contains(LinearSummaries.ROW_BASE_TOTALS);

        return _hasRowBaseTotals.Value;
      }
    }

    internal bool HasColumnBaseTotals
    {
      get
      {
        if (!_hasColumnBaseTotals.HasValue)
          _hasColumnBaseTotals = Summaries.Contains(LinearSummaries.COLUMN_BASE_TOTALS);

        return _hasColumnBaseTotals.Value;
      }
    }

    public IEnumerable<IOutputCell<T>> StreamCells()
    {
      return Orchestrator.GetByCells(this);
    }

    public IEnumerable<IOutputCell<T>[]> StreamRows()
    {
      return Orchestrator.GetByRows(this);
    }
  }
}