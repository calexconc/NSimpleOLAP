using NSimpleOLAP.Common;
using NSimpleOLAP.Common.Utils;
using NSimpleOLAP.CubeExpressions;
using NSimpleOLAP.Query.Interfaces;
using NSimpleOLAP.Query.Layout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimpleOLAP.Query.Molap
{
  internal class MolapQueryOrchestrator<T> : IQueryOrchestrator<T, IOutputCell<T>>
    where T : struct, IComparable
  {
    private Cube<T> _cube;
    private NamespaceResolver<T> _resolver;
    private AllKeysComparer<T> _allKeysComparer;
    private KeysEqualityComparer<T> _keysEqualityComparer;
    private KeysBaseEqualityComparer<T> _pairsEqualityComparer;

    public MolapQueryOrchestrator(Cube<T> cube)
    {
      _cube = cube;
      _resolver = new NamespaceResolver<T>(cube);
      _allKeysComparer = new AllKeysComparer<T>();
      _pairsEqualityComparer = new KeysBaseEqualityComparer<T>();
      _keysEqualityComparer = new KeysEqualityComparer<T>();
    }

    public IEnumerable<IOutputCell<T>> GetByCells(Query<T> query)
    {
      if (query.PredicateTree.FiltersOnFacts())
      {
        var id = CreateNewOrReuseAggregation(query);

        return GetCells(id, query);
      }

      return GetCells(query);
    }

    public IEnumerable<IOutputCell<T>[]> GetByRows(Query<T> query)
    {
      if (query.PredicateTree.FiltersOnFacts())
      {
        var id = CreateNewOrReuseAggregation(query);

        return LayerByRow(GetCells(id, query), query);
      }

      return LayerByRow(GetCells(query), query);
    }

    private T CreateNewOrReuseAggregation(Query<T> query)
    {
      var tuples = query.Axis.UnionAxis.ToArray();
      T cubeId;

      if (!_cube.Storage.AggregationExists(tuples, query.PredicateTree))
      {
        cubeId = _cube.Storage.CreateAggregation(tuples, query.PredicateTree);

        _cube.Storage.PopulateNewAggregation(cubeId, query.PredicateTree);
      }
      else
        cubeId = _cube.Storage.GetAggregationId(tuples, query.PredicateTree);

      return cubeId;
    }

    private IEnumerable<IOutputCell<T>> GetCells(T aggregationId, Query<T> query)
    {
      var tuples = query.Axis.UnionAxisTuples;

      foreach (var tuple in tuples)
      {
        var cells = _cube.Storage.GetCells(aggregationId, tuple);

        foreach (var cell in cells)
          yield return Map(cell, tuple, query);
      }
    }

    private IEnumerable<IOutputCell<T>> GetCells(Query<T> query)
    {
      var tuples = query.Axis.UnionAxisTuples;
      var filterOnAgregation = query
        .PredicateTree
        .FiltersOnAggregation();

      foreach (var tuple in tuples)
      {
        var cells = _cube.Storage.GetCells(tuple);

        foreach (var cell in cells)
        {
          if (filterOnAgregation
            && !query.PredicateTree.Execute(cell.Coords))
          {
            continue;
          }

          yield return Map(cell, tuple, query);
        }
      }
    }

    private IOutputCell<T> GetCell(Query<T> query, KeyValuePair<T, T>[] coords, KeyValuePair<T, T>[] xTuples, KeyValuePair<T, T>[] yTuples, OutputCellType cellType = OutputCellType.DATA)
    {
      var cell = _cube.Storage.GetCell(coords);

      return Map(cell, coords, xTuples, yTuples, query, cellType);
    }

    private IOutputCell<T> Map(Cell<T> cell, KeyTuplePairs<T> tuples, Query<T> query)
    {
      var ocell = new OutputCell<T>(cell.Coords, tuples.XAnchorTuple, tuples.YAnchorTuple);

      foreach (var item in query.MeasuresOrMetrics)
      {
        var dataItem = _resolver.GetDataItemInfo(item);
        var value = cell.Values[item];

        ocell.Add(dataItem.Name, value);
      }

      return ocell;
    }

    /*
    private IOutputCell<T> Map(Cell<T> cell, KeyValuePair<T, T>[] coords, KeyValuePair<T,T>[] xTuples, KeyValuePair<T, T>[] yTuples, Query<T> query)
    {
      var ocell = new OutputCell<T>(coords, xTuples, yTuples);

      foreach (var item in query.MeasuresOrMetrics)
      {
        var dataItem = _resolver.GetDataItemInfo(item);
        var value = cell.Values[item];

        ocell.Add(dataItem.Name, value);
      }

      return ocell;
    }*/

    private IOutputCell<T> Map(Cell<T> cell, KeyValuePair<T, T>[] coords, KeyValuePair<T, T>[] xTuples, KeyValuePair<T, T>[] yTuples, Query<T> query, OutputCellType cellType)
    {
      var ocell = new OutputCell<T>(coords, xTuples, yTuples, new KeyValuePair<string, string>[] { }, cellType);

      foreach (var item in query.MeasuresOrMetrics)
      {
        var dataItem = _resolver.GetDataItemInfo(item);
        var value = cell.Values[item];

        ocell.Add(dataItem.Name, value);
      }

      return ocell;
    }

    private IEnumerable<IOutputCell<T>[]> LayerByRow(IEnumerable<IOutputCell<T>> cells, Query<T> query)
    {
      var ocells = cells.OrderBy(x => x.YCoords, _allKeysComparer).ToArray();
      var colsSegments = ocells.Select(x => x.XCoords).Distinct(_pairsEqualityComparer).ToArray();
      var rowSegments = ocells.Select(x => x.YCoords).Distinct(_pairsEqualityComparer).ToArray();
      IOutputCell<T>[] columnTotals = null;
      IOutputCell<T>[] columnBaseTotals = null;
      IOutputCell<T>[] columns = null;

      if (query.Axis.HasColumns)
      {
        columns = GetColumnCells(colsSegments, query).ToArray();

        if (query.HasColumnTotals)
        {
          columnTotals = new IOutputCell<T>[columns.Length];
          columnTotals[0] = new OutputCell<T>(new KeyValuePair<T, T>[] { }, new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("Sum", "Row Total") }, OutputCellType.ROW_LABEL);

          for (var i = 1; i < columns.Length; i++)
            columnTotals[i] = new OutputCell<T>(columns[i].Coords, columns[i].XCoords, columns[i].YCoords, columns[i].Column, OutputCellType.COLUMN_TOTAL);
        }

        if (query.HasColumnBaseTotals)
        {
          columnBaseTotals = new IOutputCell<T>[columns.Length];
          columnBaseTotals[0] = new OutputCell<T>(new KeyValuePair<T, T>[] { }, new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("Base","Row Total") }, OutputCellType.ROW_LABEL);

          GetBaseColumnTotals(columns, columnBaseTotals, query);
        }

        yield return columns;
      }

      var index = 0;

      if (query.Axis.HasColumns && query.Axis.HasRows)
      {
        foreach (var row in rowSegments)
        {
          var values = new IOutputCell<T>[columns.Length];

          values[0] = GetRowCell(row, query);

          for (var i = 0; i < colsSegments.Length; i++)
          {
            if (index >= ocells.Length)
              break;

            var cell = ocells[index];

            if (_pairsEqualityComparer.Equals(cell.YCoords, row))
            {
              var cindex = Array.FindIndex(colsSegments, x => _pairsEqualityComparer.Equals(x, cell.XCoords));

              if (cindex >= 0)
              {
                values[cindex + 1] = cell;
                index++;
              }
            }
          }

          if (query.HasRowTotals)
            CalculateRowTotals(columns, values);

          if (query.HasRowBaseTotals)
            GetBaseRowTotals(columns, values, query);

          yield return values;

          if (query.HasColumnTotals)
            CalculateColumnTotals(columnTotals, values);
        }

        if (query.HasColumnTotals)
          yield return columnTotals;

        if (query.HasColumnBaseTotals)
          yield return columnBaseTotals;
      }

      if (query.Axis.HasColumns && !query.Axis.HasRows)
      {
        var values = new IOutputCell<T>[columns.Length];

        values[0] = GetMeasureCell(query.MeasuresOrMetrics, OutputCellType.ROW_LABEL);

        for (var i = 0; i < colsSegments.Length; i++)
        {
          if (index >= ocells.Length)
            break;

          var cell = ocells[index];
          var cindex = Array.FindIndex(colsSegments, x => _pairsEqualityComparer.Equals(x, cell.XCoords));

          if (cindex >= 0)
          {
            values[cindex + 1] = cell;
            index++;
          }
        }

        if (query.HasRowTotals)
          CalculateRowTotals(columns, values);

        yield return values;
      }

      if (!query.Axis.HasColumns && query.Axis.HasRows)
      {
        var header = new IOutputCell<T>[2];

        header[1] = GetMeasureCell(query.MeasuresOrMetrics, OutputCellType.COLUMN_LABEL);

        yield return header;

        foreach (var row in rowSegments)
        {
          var values = new IOutputCell<T>[2];

          values[0] = GetRowCell(row, query);

          var cell = ocells[index];

          if (_pairsEqualityComparer.Equals(cell.YCoords, row))
          {
            values[1] = cell;
            index++;
          }

          yield return values;
        }
      }
    }

    private void CalculateRowTotals(IOutputCell<T>[] columns, IOutputCell<T>[] values)
    {
      var totals = columns.Select((x, i) => new { Total = x, Index = i })
              .Where(x => x.Total != null && x.Total.CellType == OutputCellType.ROW_TOTAL);

      foreach (var total in totals)
      {
        var vcells = values
          .Where(x => x != null && x.CellType != OutputCellType.ROW_LABEL)
          .Where(x => _keysEqualityComparer.Equals(x.XCoords, total.Total.XCoords))
          .ToArray();

        if (vcells.Length > 0)
        {
          var outputCell = new OutputCell<T>(total.Total.XCoords, total.Total.XCoords, vcells[0].YCoords);
          var calculatedValues = new Dictionary<string, object>();
          values[total.Index] = outputCell;

          foreach (var xcell in vcells)
          {
            foreach (var xvalue in xcell)
            {
              if (_cube.Schema.Measures[xvalue.Key].DataType.IsValueType)
              {
                if (!calculatedValues.ContainsKey(xvalue.Key)
                  && xvalue.Value != null)
                  calculatedValues.Add(xvalue.Key, xvalue.Value);
                else if (xvalue.Value != null)
                  calculatedValues[xvalue.Key] = ((ValueType)calculatedValues[xvalue.Key]).Sum((ValueType)xvalue.Value);
              }
            }
          }

          foreach (var item in calculatedValues)
            outputCell.Add(item.Key, item.Value);
        }
      }
    }

    private void GetBaseRowTotals(IOutputCell<T>[] columns, IOutputCell<T>[] values, Query<T> query)
    {
      var totals = columns.Select((x, i) => new { Total = x, Index = i })
              .Where(x => x.Total != null && x.Total.CellType == OutputCellType.ROW_BASE_TOTAL)
              .FirstOrDefault();

      var yCords = values[0].YCoords;
      var cell = GetCell(query, yCords, new KeyValuePair<T, T>[] { }, yCords, OutputCellType.COLUMN_BASE_TOTAL);

      values[totals.Index] = cell;
    }

    private void CalculateColumnTotals(IOutputCell<T>[] columnTotals, IOutputCell<T>[] values)
    {
      for (var i = 1; i < values.Length; i++)
      {
        var current = values[i];
        var currentTotal = (OutputCell<T>)columnTotals[i];

        if (current != null)
        {
          foreach (var item in current)
          {
            if (item.Value != null)
            {
              if (!currentTotal.ContainsKey(item.Key))
                currentTotal.Add(item.Key, item.Value);
              else
                currentTotal[item.Key] = ((ValueType)currentTotal[item.Key]).Sum((ValueType)item.Value);
            }
          }
        }
      }
    }

    private void GetBaseColumnTotals(IOutputCell<T>[] columns, IOutputCell<T>[] columnBaseTotals, Query<T> query)
    {
      for (var i = 1; i < columns.Length; i++)
      {
        var current = columns[i];

        if (current != null && current.CellType != OutputCellType.ROW_BASE_TOTAL)
        {
          var xCords = current.XCoords;
          columnBaseTotals[i] = GetCell(query, xCords, xCords, new KeyValuePair<T, T>[] { });
        }
      }
    }

    private IEnumerable<IOutputCell<T>> GetColumnCells(IEnumerable<KeyValuePair<T, T>[]> pairs, Query<T> query)
    {
      var schemaDims = query.Cube.Schema.Dimensions;
      var defaultValue = default(T);

      yield return null;

      KeyValuePair<T, T>[] currentRowTotal = null;
      OutputCell<T> currentRowTotalCell = null;
      OutputCell<T> currentRowBaseTotalCell = null;
      var rowTotalsList = new List<OutputCell<T>>();

      foreach (var col in pairs)
      {
        var descriptors = new List<KeyValuePair<string, string>>();
        var rowTotals = new List<KeyValuePair<T, T>>();

        foreach (var item in col)
        {
          if (!item.Value.Equals(defaultValue))
          {
            var value = new KeyValuePair<string, string>(schemaDims[item.Key].Name, schemaDims[item.Key].Members[item.Value].Name);

            descriptors.Add(value);

            if (query.HasRowTotals || query.HasRowBaseTotals)
            {
              rowTotals.Add(new KeyValuePair<T, T>(item.Key, defaultValue));
            }
          }
        }

        yield return new OutputCell<T>(col, descriptors.ToArray(), OutputCellType.COLUMN_LABEL);

        if (query.HasRowTotals || query.HasRowBaseTotals)
        {
          if (currentRowTotal == null)
          {
            currentRowTotal = rowTotals.ToArray();

            if (query.HasRowTotals)
            {
              currentRowTotalCell = new OutputCell<T>(currentRowTotal, descriptors.Select(x => new KeyValuePair<string, string>(x.Key, "Total")).ToArray(), OutputCellType.ROW_TOTAL);

              rowTotalsList.Add(currentRowTotalCell);
            }

            if (query.HasRowBaseTotals)
              currentRowBaseTotalCell = new OutputCell<T>(currentRowTotal, col,  new KeyValuePair<T, T>[] { },descriptors.Select(x => new KeyValuePair<string, string>(x.Key, "Base Total")).ToArray(), OutputCellType.ROW_BASE_TOTAL);
          }
          else
          {
            if (_allKeysComparer.Compare(currentRowTotal, rowTotals.ToArray()) != 0)
            {
              currentRowTotal = rowTotals.ToArray();

              if (query.HasRowTotals)
              {
                currentRowTotalCell = new OutputCell<T>(currentRowTotal, descriptors.Select(x => new KeyValuePair<string, string>(x.Key, "")).ToArray(), OutputCellType.ROW_TOTAL);

                rowTotalsList.Add(currentRowTotalCell);
              }
            }
          }
        }
      }

      if (query.HasRowBaseTotals)
        rowTotalsList.Add(currentRowBaseTotalCell);

      if (query.HasRowTotals || query.HasRowBaseTotals)
      {
        foreach (var item in rowTotalsList)
          yield return item;
      }
    }

    private IOutputCell<T> GetRowCell(KeyValuePair<T, T>[] tuple, Query<T> query)
    {
      var descriptors = new List<KeyValuePair<string, string>>();
      var schemaDims = query.Cube.Schema.Dimensions;

      foreach (var item in tuple)
      {
        var value = new KeyValuePair<string, string>(schemaDims[item.Key].Name, schemaDims[item.Key].Members[item.Value].Name);

        descriptors.Add(value);
      }

      return new OutputCell<T>(tuple, descriptors.ToArray(), OutputCellType.ROW_LABEL);
    }

    private IOutputCell<T> GetMeasureCell(IEnumerable<T> measures, OutputCellType cellType)
    {
      var descriptors = new List<KeyValuePair<string, string>>();

      foreach (var item in measures)
      {
        var dataItem = _resolver.GetDataItemInfo(item);
        var value = new KeyValuePair<string, string>("Measure", dataItem.Name);

        descriptors.Add(value);
      }

      return new OutputCell<T>(new KeyValuePair<T, T>[] { }, descriptors.ToArray(), cellType);
    }
  }
}