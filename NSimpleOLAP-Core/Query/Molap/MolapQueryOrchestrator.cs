using NSimpleOLAP.Common;
using NSimpleOLAP.Query.Interfaces;
using NSimpleOLAP.Query.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using NSimpleOLAP.Common.Utils;

namespace NSimpleOLAP.Query.Molap
{
  internal class MolapQueryOrchestrator<T> : IQueryOrchestrator<T, IOutputCell<T>>
    where T : struct, IComparable
  {
    private Cube<T> _cube;
    private NamespaceResolver<T> _resolver;
    private AllKeysComparer<T> _allKeysComparer;
    private KeysBaseEqualityComparer<T> _pairsEqualityComparer;

    public MolapQueryOrchestrator(Cube<T> cube)
    {
      _cube = cube;
      _resolver = new NamespaceResolver<T>(cube);
      _allKeysComparer = new AllKeysComparer<T>();
      _pairsEqualityComparer = new KeysBaseEqualityComparer<T>();
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

    private IEnumerable<IOutputCell<T>[]> LayerByRow(IEnumerable<IOutputCell<T>> cells, Query<T> query)
    {
      var ocells = cells.OrderBy(x => x.YCoords, _allKeysComparer).ToArray();
      var colsSegments = ocells.Select(x => x.XCoords).Distinct(_pairsEqualityComparer).ToArray();
      var rowSegments = ocells.Select(x => x.YCoords).Distinct(_pairsEqualityComparer).ToArray();

      if (query.Axis.HasColumns)
      {
        var columns = GetColumnCells(colsSegments, query);

        yield return columns.ToArray();
      }

      var index = 0;

      if (query.Axis.HasColumns && query.Axis.HasRows)
      {
        foreach (var row in rowSegments)
        {
          var values = new IOutputCell<T>[colsSegments.Length + 1];

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

          yield return values;
        }
      }

      if (query.Axis.HasColumns && !query.Axis.HasRows)
      {
        var values = new IOutputCell<T>[colsSegments.Length + 1];

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

    private IEnumerable<IOutputCell<T>> GetColumnCells(IEnumerable<KeyValuePair<T, T>[]> pairs, Query<T> query)
    {
      var schemaDims = query.Cube.Schema.Dimensions;
      var defaultValue = default(T);

      yield return null;

      foreach (var col in pairs)
      {
        var descriptors = new List<KeyValuePair<string, string>>();

        foreach (var item in col)
        {
          if (!item.Value.Equals(defaultValue))
          {
            var value = new KeyValuePair<string, string>(schemaDims[item.Key].Name, schemaDims[item.Key].Members[item.Value].Name);

            descriptors.Add(value);
          }
          
        }

        yield return new OutputCell<T>(col, descriptors.ToArray(), OutputCellType.COLUMN_LABEL);
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