using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimpleOLAP.Common
{
  internal class KeyEqualityComparer<T> : IEqualityComparer<KeyValuePair<T, T>>
    where T : struct, IComparable
  {
    public bool Equals(KeyValuePair<T, T> x, KeyValuePair<T, T> y)
    {
      return x.Equals(y);
    }

    public int GetHashCode(KeyValuePair<T, T> obj)
    {
      return obj.Key.GetHashCode()
        ^ obj.Value.GetHashCode();
    }
  }

  internal class KeyComparer<T> : IComparer<KeyValuePair<T, T>>
    where T : struct, IComparable
  {
    public int Compare(KeyValuePair<T, T> x, KeyValuePair<T, T> y)
    {
      var keyCompare = x.Key.CompareTo(y.Key);

      if (keyCompare != 0)
        return keyCompare;

      return x.Value.CompareTo(y.Value);
    }
  }

  internal class AllKeyComparer<T> : IComparer<KeyValuePair<T, T>>
    where T : struct, IComparable
  {
    public int Compare(KeyValuePair<T, T> x, KeyValuePair<T, T> y)
    {
      var keyCompare = x.Key.CompareTo(y.Key);

      return keyCompare;
    }
  }

  internal class AllKeysComparer<T> : IComparer<KeyValuePair<T, T>[]>
    where T : struct, IComparable
  {
    private KeysBaseEqualityComparer<T> _allKeyComparer = new KeysBaseEqualityComparer<T>();

    public int Compare(KeyValuePair<T, T>[] x, KeyValuePair<T, T>[] y)
    {
      return _allKeyComparer.GetHashCode(x).CompareTo(_allKeyComparer.GetHashCode(y));
    }
  }

  internal class KeysBaseEqualityComparer<T> : IEqualityComparer<KeyValuePair<T, T>[]>
    where T : struct, IComparable
  {

    public bool Equals(KeyValuePair<T, T>[] x, KeyValuePair<T, T>[] y)
    {
      return ComparePairs(x, y);
    }

    public int GetHashCode(KeyValuePair<T, T>[] obj)
    {
      var first = obj.FirstOrDefault();
      var result = first.Key.GetHashCode()
        ^ first.Value.GetHashCode();

      foreach (var item in obj.Skip(1))
        result ^= item.Key.GetHashCode()
          ^ item.Value.GetHashCode();

      return result;
    }

    private bool ComparePairs(KeyValuePair<T, T>[] cellCoords, KeyValuePair<T, T>[] scoords)
    {
      var ret = false;
      var results = cellCoords
        .Join(scoords, x => x.Key, y => y.Key, (x, y) => new { x, y })
        .Where(x => x.x.Value.Equals(x.y.Value))
        .Count();

      if (results == scoords.Length)
        ret = true;

      return ret;
    }
  }

  internal class KeysEqualityComparer<T> : IEqualityComparer<KeyValuePair<T, T>[]>
    where T : struct, IComparable
  {

    public bool Equals(KeyValuePair<T, T>[] x, KeyValuePair<T, T>[] y)
    {
      return ComparePairs(x, y);
    }

    public int GetHashCode(KeyValuePair<T, T>[] obj)
    {
      var first = obj.FirstOrDefault();
      var result = first.Key.GetHashCode();

      foreach (var item in obj.Skip(1))
        result ^= item.Key.GetHashCode();

      return result;
    }

    private bool ComparePairs(KeyValuePair<T, T>[] cellCoords, KeyValuePair<T, T>[] scoords)
    {
      var ret = false;
      var results = cellCoords
        .Join(scoords, x => x.Key, y => y.Key, (x, y) => new { x, y })
        .Count();

      if (results == scoords.Length)
        ret = true;

      return ret;
    }
  }
}