using NSimpleOLAP.Common;
using NSimpleOLAP.Common.Hashing;
using NSimpleOLAP.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimpleOLAP.Query
{
  /// <summary>
  /// Description of Axis.
  /// </summary>
  public class Axis<T>
    where T : struct, IComparable
  {
    private Hasher<T> _hasher;
    private List<KeyValuePair<T, T>[]> _rowsAxis;
    private List<KeyValuePair<T, T>[]> _columnsAxis;
    private List<T> _rowHashes;
    private List<T> _columnHashes;
    private KeyComparer<T> _keyComparer;
    private KeyEqualityComparer<T> _keyEqualityComparer;
    private DataSchema<T> _schema;

    public Axis(MolapHashTypes hashingtype, DataSchema<T> schema)
    {
      _schema = schema;
      _rowsAxis = new List<KeyValuePair<T, T>[]>();
      _columnsAxis = new List<KeyValuePair<T, T>[]>();
      _rowHashes = new List<T>();
      _columnHashes = new List<T>();
      _hasher = Hasher<T>.Create(hashingtype);
      _keyComparer = new KeyComparer<T>();
      _keyEqualityComparer = new KeyEqualityComparer<T>();
    }

    #region props

    public IEnumerable<KeyValuePair<T, T>[]> RowAxis
    {
      get { return _rowsAxis; }
    }

    public IEnumerable<KeyValuePair<T, T>[]> ColumnAxis
    {
      get { return _columnsAxis; }
    }

    public IEnumerable<KeyValuePair<T, T>> UnionAxis
    {
      get { return GetAllUniquePairs(); }
    }

    public IEnumerable<KeyTuplePairs<T>> UnionAxisTuples
    {
      get { return GetAllUniquePairs(GetTuplePairs()); }
    }

    public bool HasRows
    {
      get
      {
        return _rowsAxis.Count > 0;
      }
    }

    public bool HasColumns
    {
      get
      {
        return _columnsAxis.Count > 0;
      }
    }

    #endregion props

    #region public methods

    public void AddRowTuples(params KeyValuePair<T, T>[] tuples)
    {
      var rtuples = tuples;

      if (tuples.Any(x => x.IsPositionalHint()))
        rtuples = ReplacePositionalHints(tuples).ToArray();

      T hash = _hasher.HashTuples(rtuples);

      if (!_rowHashes.Contains(hash))
      {
        _rowsAxis.Add(rtuples);
        _rowHashes.Add(hash);
      }
    }

    public void AddColumnTuples(params KeyValuePair<T, T>[] tuples)
    {
      var rtuples = tuples;

      if (tuples.Any(x => x.IsPositionalHint()))
        rtuples = ReplacePositionalHints(tuples).ToArray();

      T hash = _hasher.HashTuples(rtuples);

      if (!_columnHashes.Contains(hash))
      {
        _columnsAxis.Add(rtuples);
        _columnHashes.Add(hash);
      }
    }

    #endregion public methods

    #region private methods

    private bool ContainsDimension(KeyValuePair<T, T>[] tuples)
    {
      return tuples.Any(x => IsDimension(x));
    }

    private IEnumerable<KeyValuePair<T, T>> GetAllUniquePairs()
    {
      var query = GetAllPairs()
        .Where(x => !x.IsWildcard())
        .Distinct(_keyEqualityComparer)
        .OrderBy(x => x, _keyComparer);

      foreach (var item in query)
        yield return item;
    }

    private IEnumerable<KeyTuplePairs<T>> GetAllUniquePairs(IEnumerable<Tuple<KeyValuePair<T, T>[], KeyValuePair<T, T>[], KeyValuePair<T, T>[]>> tuples)
    {
      foreach (var item in tuples)
      {
        var query = item.Item1
        .Where(x => !x.IsReservedValue())
        .Distinct(_keyEqualityComparer)
        .OrderBy(x => x, _keyComparer);
        var selectors = item.Item1
          .Select((x, index) => new { Pair = x, Index = index })
          .Where(x => x.Pair.IsWildcard())
          .ToArray();
        var list = query.ToList();
        var anchor = list.ToArray();
        // to do optimize this
        foreach (var selector in selectors)
        {
          var value = item.Item1[selector.Index - 1];
          var index = list
            .FindIndex(x => value.Key.Equals(x.Key) && value.Value.Equals(x.Value));

          list.Insert(index + 1, selector.Pair);
        }

        yield return new KeyTuplePairs<T>(anchor, list.ToArray(), item.Item2, item.Item3);
      }
    }

    private IEnumerable<KeyValuePair<T, T>> GetAllPairs()
    {
      foreach (var item in _rowsAxis)
      {
        foreach (var pair in item)
          yield return pair;
      }

      foreach (var item in _columnsAxis)
      {
        foreach (var pair in item)
          yield return pair;
      }
    }

    private IEnumerable<Tuple<KeyValuePair<T, T>[], KeyValuePair<T, T>[], KeyValuePair<T, T>[]>> GetTuplePairs()
    {
      if (_columnsAxis.Count > 0 && _rowsAxis.Count > 0)
      {
        foreach (var col in _columnsAxis)
        {
          foreach (var row in _rowsAxis)
          {
            var result = new KeyValuePair<T, T>[col.Length + row.Length];

            col.CopyTo(result, 0);
            row.CopyTo(result, col.Length);

            yield return new Tuple<KeyValuePair<T, T>[], KeyValuePair<T, T>[], KeyValuePair<T, T>[]>(result,
              col.Where(x => !x.IsWildcard()).ToArray(), row.Where(x => !x.IsWildcard()).ToArray());
          }
        }
      }
      if (_columnsAxis.Count == 0 && _rowsAxis.Count > 0)
      {
        foreach (var row in _rowsAxis)
        {
          var result = new KeyValuePair<T, T>[row.Length];

          row.CopyTo(result, 0);

          yield return new Tuple<KeyValuePair<T, T>[], KeyValuePair<T, T>[], KeyValuePair<T, T>[]>(result,
            new KeyValuePair<T, T>[] { }, row.Where(x => !x.IsWildcard()).ToArray());
        }
      }

      if (_columnsAxis.Count > 0 && _rowsAxis.Count == 0)
      {
        foreach (var col in _columnsAxis)
        {
          var result = new KeyValuePair<T, T>[col.Length];

          col.CopyTo(result, 0);

          yield return new Tuple<KeyValuePair<T, T>[], KeyValuePair<T, T>[], KeyValuePair<T, T>[]>(result,
            col.Where(x => !x.IsWildcard()).ToArray(), new KeyValuePair<T, T>[] { });
        }
      }
    }

    private IEnumerable<KeyValuePair<T, T>> ReplacePositionalHints(KeyValuePair<T, T>[] values)
    {
      var query = values
          .Select((x, index) => new { Pair = x, Index = index, Hint = x.IsPositionalHint() })
          .ToArray();

      foreach (var item in query)
      {
        var last = item.Index > values.Length - 1
          ? item.Index : item.Index + 1;

        if (item.Hint)
        {
          var keyPair = values[item.Index];

          if (item.Pair.IsNext())
          {
            var member = _schema.Dimensions[keyPair.Key].Members.Next(keyPair.Value);

            yield return new KeyValuePair<T, T>(keyPair.Key, member.ID);
          }
          else if (item.Pair.IsPrevious())
          {
            var member = _schema.Dimensions[keyPair.Key].Members.Previous(keyPair.Value);

            yield return new KeyValuePair<T, T>(keyPair.Key, member.ID);
          }
        }
        else if (!query[last].Hint)
          yield return item.Pair;
      }
    }

    private bool IsDimension(KeyValuePair<T, T> dim)
    {
      var dvalue = default(T);

      return !dim.Key.Equals(dvalue)
        && dim.Value.Equals(dvalue);
    }

    #endregion private methods
  }
}