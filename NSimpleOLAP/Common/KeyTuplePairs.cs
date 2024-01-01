using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimpleOLAP.Common
{
  public class KeyTuplePairs<T>
    where T : struct, IComparable
  {
    private readonly bool _sameValue;

    public KeyTuplePairs(KeyValuePair<T, T>[] anchor, KeyValuePair<T, T>[] selector, KeyValuePair<T, T>[] col, KeyValuePair<T, T>[] row)
    {
      AnchorTuple = anchor;
      SelectorTuple = selector;
      Selectors = GetSelectorPairs().ToArray();
      XAnchorTuple = col;
      YAnchorTuple = row;
      _sameValue = anchor.Length == selector.Length;
    }

    public bool HasSelectors
    {
      get
      {
        return !_sameValue;
      }
    }

    public KeyValuePair<T, T>[] AnchorTuple { get; private set; }

    public KeyValuePair<T, T>[] XAnchorTuple { get; private set; }

    public KeyValuePair<T, T>[] YAnchorTuple { get; private set; }

    public KeyValuePair<T, T>[] SelectorTuple { get; private set; }

    public Tuple<KeyValuePair<T, T>, KeyValuePair<T, T>>[] Selectors { get; private set; }

    public KeyValuePair<T, T>? GetSelector(KeyValuePair<T, T> value)
    {
      var query = SelectorTuple.ToList();
      var index = query
        .FindIndex(x => value.Key.Equals(x.Key) && value.Value.Equals(x.Value));

      if (index + 1 < SelectorTuple.Length)
      {
        var result = SelectorTuple[index + 1];

        if (result.IsReservedValue())
          return result;
      }

      return null;
    }

    private IEnumerable<Tuple<KeyValuePair<T, T>, KeyValuePair<T, T>>> GetSelectorPairs()
    {
      var list = SelectorTuple.
        Select((x, i) => new { Tup = x, Index = i, IsSelector = x.IsReservedValue() })
        .ToList();

      foreach (var item in list)
      {
        if (item.IsSelector)
        {
          yield return new Tuple<KeyValuePair<T, T>, KeyValuePair<T, T>>(list[item.Index - 1].Tup, item.Tup);
        }
      }
    }

    public int SelectorCount()
    {
      var query = SelectorTuple
        .Where(x => x.IsReservedValue());

      return query.Count();
    }
  }
}